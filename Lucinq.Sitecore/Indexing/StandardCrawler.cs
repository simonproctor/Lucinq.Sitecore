using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;
using Lucinq.Building;
using Lucinq.SitecoreIntegration.Constants;
using Lucinq.SitecoreIntegration.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Search;
using Sitecore.Search.Crawlers;
using Field = Sitecore.Data.Fields.Field;
using LuceneField = Lucene.Net.Documents.Field;

namespace Lucinq.SitecoreIntegration.Indexing
{
	public class StandardCrawler : DatabaseCrawler
	{
		#region [ Methods ]

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

		protected override void AddSpecialFields(Document document, Item item)
		{
			// base.AddSpecialFields(document, item);

		}


		protected List<Field> GetFields(Item item)
		{
			return item.Fields.ToList();
		}

		protected virtual void ProcessFields(Document document, Item item)
		{
			item.Fields.ReadAll();
			AddLucinqFields(document, item);
			var fields = GetFields(item);
			fields.ForEach(field =>
				{
					LuceneField luceneField = ProcessField(field);
					if (luceneField == null)
					{
						return;
					}
					document.AddAnalysedField(field.Name, field.Value);
				});
		}

		protected virtual void AddLucinqFields(Document document, Item item)
		{
			StringBuilder templatePathBuilder = new StringBuilder();
			AddTemplatePath(item.Template, templatePathBuilder);

			StringBuilder pathBuilder = new StringBuilder();
			AddItemPath(item, pathBuilder);

			document.Setup(
					x => x.AddAnalysedField(SitecoreFields.Id, item.ID.ToLuceneId(), true),
					x => x.AddNonAnalysedField(SitecoreFields.Language, item.Language.Name, true),
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
			if (!IncludeField(field))
			{
				return null;
			}
			LuceneField luceneField = new LuceneField(field.Name.ToLower(), field.Value.ToLower(), LuceneField.Store.YES,
			                                          LuceneField.Index.ANALYZED);
			return luceneField;
		}

		protected virtual bool IncludeField(Field field)
		{
			return true;
		}

		#endregion
	}
}
