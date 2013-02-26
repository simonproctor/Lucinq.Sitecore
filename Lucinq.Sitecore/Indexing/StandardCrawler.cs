using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Sitecore.Data.Items;
using Sitecore.Search;
using Sitecore.Search.Crawlers;
using Field = Sitecore.Data.Fields.Field;
using LuceneField = Lucene.Net.Documents.Field;

namespace Lucinq.SitecoreIntegration.Indexing
{
	public class StandardCrawler : DatabaseCrawler
	{
		protected override void IndexVersion(Item item, Item latestVersion, IndexUpdateContext context)
		{
			if (item.Template == null)
			{
				return;
			}
			base.IndexVersion(item, latestVersion, context);
		}

		protected override void AddAllFields(Document document, Item item, bool versionSpecific)
		{
			ProcessFields(document, item);
			// base.AddAllFields(document, item, versionSpecific);
		}

		protected List<Field> GetFields(Item item)
		{
			return item.Fields.ToList();
		}

		protected virtual void ProcessFields(Document document, Item item)
		{
			item.Fields.ReadAll();
			var fields = GetFields(item);
			fields.ForEach(field =>
				{
					LuceneField luceneField = ProcessField(field);
					if (luceneField == null)
					{
						return;
					}
					document.Add(luceneField);
				});
		}

		protected virtual LuceneField ProcessField(Field field)
		{
			if (!IncludeField(field))
			{
				return null;
			}
			LuceneField luceneField = new LuceneField(field.Name.ToLower(), field.Value, LuceneField.Store.YES,
			                                          LuceneField.Index.ANALYZED);
			return luceneField;
		}

		protected virtual bool IncludeField(Field field)
		{
			return true;
		}
	}
}
