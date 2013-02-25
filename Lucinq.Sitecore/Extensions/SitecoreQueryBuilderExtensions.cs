using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucinq.Interfaces;
using Lucinq.Sitecore.Constants;
using Sitecore.Data;
using Sitecore.Globalization;

namespace Lucinq.Sitecore.Extensions
{
	public static class SitecoreQueryBuilderExtensions
	{
		#region [ ID Extensions ]

		public static TermQuery Id(this IQueryBuilder inputQueryBuilder, ID itemId, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			string luceneItemId = itemId.ToShortID().ToString().ToLower();
			return inputQueryBuilder.Term(SitecoreFields.Id, luceneItemId, occur, boost, key);
		}

		public static IQueryBuilder Ids(this IQueryBuilder inputQueryBuilder, ID[] itemIds, float? boost = null, BooleanClause.Occur occur = null, string key = null)
		{
			foreach (ID itemId in itemIds)
			{
				inputQueryBuilder.TemplateId(itemId, occur, boost, key);
			}
			return inputQueryBuilder;
		}

		#endregion 

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

		#region [ Regionalisation ]

		public static TermQuery Language(this IQueryBuilder inputQueryBuilder, Language language, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			string languageString = language.CultureInfo.Name.ToLower();
			return inputQueryBuilder.Term(SitecoreFields.Language, QueryParser.Escape(languageString), occur, boost, key);
		}

		public static IQueryBuilder Languages(this IQueryBuilder inputQueryBuilder, Language[] languages, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			foreach (Language language in languages)
			{
				inputQueryBuilder.Language(language, occur, boost, key);
			}
			return inputQueryBuilder;
		}
		#endregion
	}
}
