using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Lucene.Net.Documents;
using Lucinq.Building;
using Lucinq.SitecoreIntegration.Constants;
using Lucinq.SitecoreIntegration.Extensions;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Search;
using Sitecore.Search.Crawlers;
using Sitecore.Xml;
using DateField = Sitecore.Data.Fields.DateField;
using Field = Sitecore.Data.Fields.Field;
using LuceneField = Lucene.Net.Documents.Field;

namespace Lucinq.SitecoreIntegration.Indexing
{
	public class StandardCrawler : DatabaseCrawler
	{
		#region [ Fields ]

		private static readonly Dictionary<string, string> fieldTypes = new Dictionary<string, string>();

		private static readonly Dictionary<ID, FieldConfiguration> fieldConfigurations = new Dictionary<ID, FieldConfiguration>();

		#endregion

		#region [ Properties ]

		public static Dictionary<string, string> FieldTypes { get { return fieldTypes; } }

		public static Dictionary<ID, FieldConfiguration> FieldConfigurations { get { return fieldConfigurations; } }

		#endregion

		#region [ Methods ]

		public virtual void AddFieldTypes(XmlNode configNode)
		{
			Assert.ArgumentNotNull(configNode, "configNode");
			var fieldName = XmlUtil.GetAttribute("name", configNode);
			var fieldType = XmlUtil.GetAttribute("type", configNode);
			FieldTypes.Add(fieldName.ToLowerInvariant(), fieldType.ToLower());
		}

		public virtual void AddFieldConfigurations(XmlNode configNode)
		{
			Assert.ArgumentNotNull(configNode, "configNode");
			var fieldName = XmlUtil.GetAttribute("name", configNode);
			var idString = XmlUtil.GetAttribute("id", configNode);
			ID fieldId;
			if (!ID.TryParse(idString, out fieldId))
			{
				return;
			}
			FieldConfiguration fieldConfiguration = new FieldConfiguration();
			bool store;
			fieldConfiguration.Store = bool.TryParse(XmlUtil.GetAttribute("store", configNode), out store) && store;
			bool analyze;
			fieldConfiguration.Analyze = bool.TryParse(XmlUtil.GetAttribute("analyze", configNode), out analyze) && analyze;

			FieldConfigurations.Add(fieldId, fieldConfiguration);
		}

		protected override void IndexVersion(Item item, Item latestVersion, IndexUpdateContext context)
		{
			Assert.ArgumentNotNull(item, "item");
			Assert.ArgumentNotNull(latestVersion, "latestVersion");
			Assert.ArgumentNotNull(context, "context");

			if (item.Template == null)
			{
				Log.Warn(string.Format("Cannot update item version. Reason: Template is NULL in item '{0}'.", item.Paths.FullPath), this);
				return;
			}
			
			Document document = new Document();
			AddVersionIdentifiers(item, latestVersion, document);
			AddAllFields(document, item, true);
			AddSpecialFields(document, item);
			AdjustBoost(document, item);
			context.AddDocument(document);
		}

		protected override void AddAllFields(Document document, Item item, bool versionSpecific)
		{
			ProcessFields(document, item);
			// base.AddAllFields(document, item, versionSpecific);
		}

		protected List<Field> GetFields(Item item)
		{
			return item.Fields.Where(IncludeField).ToList();
		}

		protected virtual void ProcessFields(Document document, Item item)
		{
			item.Fields.ReadAll();
			AddLucinqFields(document, item);
			AddCustomFields(document, item);

			List<Field> fields = GetFields(item);
			fields.ForEach(field => AddField(document, field));
		}

		protected virtual void AddField(Document document, Field field)
		{
			string fieldType = "value";
			string fieldTypeKey = field.TypeKey.ToLower();
			if (FieldTypes.ContainsKey(fieldTypeKey))
			{
				fieldType = FieldTypes[fieldTypeKey].ToLower();
			}

			FieldConfiguration fieldConfiguration;
			switch (fieldType)
			{
				case "multilist":
					fieldConfiguration = GetFieldConfiguration(field);
					AddMultilistField(document, field, fieldConfiguration);
					break;
				case "link":
					fieldConfiguration = GetFieldConfiguration(field, false);
					AddLinkField(document, field, fieldConfiguration);
					break;
				case "datetime":
					fieldConfiguration = GetFieldConfiguration(field, false);
					AddDateTimeField(document, field, fieldConfiguration);
					break;
				default:
					fieldConfiguration = GetFieldConfiguration(field, false);
					AddValueField(document, field, fieldConfiguration);
					break;
			}
		}

		protected virtual FieldConfiguration GetFieldConfiguration(Field field, bool analyze = true, bool store = false)
		{
			FieldConfiguration fieldConfiguration = new FieldConfiguration { FieldId = field.ID, Analyze = analyze, Store = store };
			if (FieldConfigurations.ContainsKey(field.ID))
			{
				fieldConfiguration = FieldConfigurations[field.ID];
			}
			return fieldConfiguration;
		}

		protected virtual void AddMultilistField(Document document, MultilistField field, FieldConfiguration fieldConfiguration)
		{
			if (field == null || fieldConfiguration == null)
			{
				return;
			}

			StringBuilder fieldBuilder = new StringBuilder();
			bool first = true;
			foreach (ID targetId in field.TargetIDs)
			{
				if (!first)
				{
					fieldBuilder.Append(" ");
				}
				first = false;
				fieldBuilder.Append(targetId.ToLuceneId());
			}

			if (fieldConfiguration.Analyze)
			{
				document.AddAnalysedField(field.InnerField.Name.ToLower(), fieldBuilder.ToString(), fieldConfiguration.Store);
				return;
			}

			document.AddNonAnalysedField(field.InnerField.Name.ToLower(), fieldBuilder.ToString(), fieldConfiguration.Store);
		}

		protected void AddDateTimeField(Document document, DateField field, FieldConfiguration fieldConfiguration)
		{
			if (field == null || fieldConfiguration == null)
			{
				return;
			}

			if (fieldConfiguration.Analyze)
			{
				document.AddAnalysedField(field.InnerField.Name.ToLower(), DateTools.DateToString(field.DateTime, DateTools.Resolution.MINUTE), fieldConfiguration.Store);
				return;
			}

			document.AddNonAnalysedField(field.InnerField.Name.ToLower(), DateTools.DateToString(field.DateTime, DateTools.Resolution.MINUTE), fieldConfiguration.Store);
		}

		protected virtual void AddLinkField(Document document, LinkField field, FieldConfiguration fieldConfiguration)
		{
			if (field == null || fieldConfiguration == null)
			{
				return;
			}

			if (fieldConfiguration.Analyze)
			{
				document.AddAnalysedField(field.InnerField.Name.ToLower(), field.TargetID.ToLuceneId(), fieldConfiguration.Store);
				return;
			}
			document.AddNonAnalysedField(field.InnerField.Name.ToLower(), field.TargetID.ToLuceneId(), fieldConfiguration.Store);
		}

		protected virtual void AddValueField(Document document, Field field, FieldConfiguration fieldConfiguration)
		{
			if (field == null || fieldConfiguration == null)
			{
				return;
			}

			if (fieldConfiguration.Analyze)
			{
				document.AddAnalysedField(field.Name.ToLower(), field.Value, fieldConfiguration.Store);
				return;
			}
			document.AddNonAnalysedField(field.Name.ToLower(), field.Value, fieldConfiguration.Store);
		}

		/// <summary>
		/// Override this method to add your additional fields
		/// </summary>
		/// <param name="document"></param>
		/// <param name="item"></param>
		protected virtual void AddCustomFields(Document document, Item item)
		{
			// Placeholder method to add additional fields specific to the implementation
		}

		/// <summary>
		/// Adds the lucinq specific fields
		/// </summary>
		/// <param name="document"></param>
		/// <param name="item"></param>
		protected void AddLucinqFields(Document document, Item item)
		{
			StringBuilder templatePathBuilder = new StringBuilder();
			AddTemplatePath(item.Template, templatePathBuilder);

			StringBuilder pathBuilder = new StringBuilder();
			AddItemPath(item, pathBuilder);

			document.Setup(
					x => x.AddAnalysedField(SitecoreFields.Id, item.ID.ToLuceneId(), true),
					x => x.AddNonAnalysedField(SitecoreFields.Language, item.Language.Name, true),
					x => x.AddNonAnalysedField(SitecoreFields.TemplateId, item.TemplateID.ToLuceneId(), true),
					x => x.AddAnalysedField(SitecoreFields.Name, item.Name, true),
					x => x.AddAnalysedField(SitecoreFields.TemplatePath, templatePathBuilder.ToString()),
					x => x.AddAnalysedField(SitecoreFields.Path, pathBuilder.ToString()),
					x => x.AddAnalysedField(SitecoreFields.Parent, item.ParentID.ToLuceneId())
				);
		}

		protected virtual void AddItemPath(Item item, StringBuilder stringBuilder)
		{
			if (item.Parent == null)
			{
				return;
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(item.ParentID.ToLuceneId());
			AddItemPath(item.Parent, stringBuilder);
		}

		protected virtual void AddTemplatePath(TemplateItem templateItem, StringBuilder stringBuilder)
		{
			if (templateItem.ID == Sitecore.TemplateIDs.StandardTemplate)
            {
                return;
            }

			stringBuilder.Append(templateItem.ID.ToLuceneId());
            foreach (TemplateItem template in templateItem.BaseTemplates)
            {
	            if (stringBuilder.Length > 0)
	            {
		            stringBuilder.Append(" ");
	            }
	            AddTemplatePath(template, stringBuilder);
            }
		}

		protected virtual LuceneField ProcessField(Field field)
		{
			LuceneField luceneField = new LuceneField(field.Name.ToLower(), field.Value.ToLower(), LuceneField.Store.YES,
			                                          LuceneField.Index.ANALYZED);
			return luceneField;
		}

		protected virtual bool IncludeField(Field field)
		{
			// todo: Allow Ommision of Fields
			return true;
		}

		#endregion
	}
}
