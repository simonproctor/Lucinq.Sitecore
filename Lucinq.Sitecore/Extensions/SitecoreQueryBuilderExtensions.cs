using Lucene.Net.Search;
using Lucinq.Interfaces;
using Lucinq.Sitecore.Constants;
using Sitecore.Data;

namespace Lucinq.Sitecore.Extensions
{
	public static class SitecoreQueryBuilderExtensions
	{
		#region [ Template Extensions ]

		public static TermQuery TemplateId(this IQueryBuilder inputQueryBuilder, ID templateId, BooleanClause.Occur occur = null,  float? boost = null, string key = null)
		{
			string luceneTemplateId = templateId.ToShortID().ToString().ToLower();
			return inputQueryBuilder.Term(SitecoreFields.TemplateId, luceneTemplateId, occur, boost, key);
		}

		public static IQueryBuilder TemplateIds(this IQueryBuilder inputQueryBuilder, ID[] templateIds, float? boost = null, BooleanClause.Occur occur = null, string key = null)
		{
			foreach (ID templateId in templateIds)
			{
				inputQueryBuilder.TemplateId(templateId, occur, boost, key);
			}
			return inputQueryBuilder;
		}

		#endregion

		#region [ Name Extensions ]

		public static TermQuery Name(this IQueryBuilder inputQueryBuilder, string value, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			return inputQueryBuilder.Term(SitecoreFields.Name, value.ToLower(), occur, boost, key);
		}

		public static WildcardQuery NameWildCard(this IQueryBuilder inputQueryBuilder, string value, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			return inputQueryBuilder.WildCard(SitecoreFields.Name, value.ToLower(), occur, boost, key);
		}

		public static IQueryBuilder Names(this IQueryBuilder inputQueryBuilder, string[] values, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			foreach (string templateId in values)
			{
				inputQueryBuilder.Term(SitecoreFields.TemplateId, templateId, occur, boost, key);
			}
			return inputQueryBuilder;
		}

		#endregion
	}
}
