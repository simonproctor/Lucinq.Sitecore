using System;
using Lucene.Net.Analysis;
using Lucene.Net.Search;
using Lucinq.Interfaces;
using Lucinq.SitecoreIntegration.Constants;
using Sitecore.Data;
using Sitecore.Globalization;

namespace Lucinq.SitecoreIntegration.Extensions
{
	public static class SitecoreQueryBuilderExtensions
	{
		#region [ ID Extensions ]

		public static TermQuery Id(this IQueryBuilder inputQueryBuilder, ID itemId, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			string luceneItemId = itemId.ToLuceneId();
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

		/// <summary>
		/// To find items that directly inherit from a particular template
		/// </summary>
		/// <param name="inputQueryBuilder"></param>
		/// <param name="templateId"></param>
		/// <param name="occur"></param>
		/// <param name="boost"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static TermQuery TemplateId(this IQueryBuilder inputQueryBuilder, ID templateId, BooleanClause.Occur occur = null,  float? boost = null, string key = null)
		{
			string luceneTemplateId = templateId.ToLuceneId();
			return inputQueryBuilder.Term(SitecoreFields.TemplateId, luceneTemplateId, occur, boost, key);
		}

		/// <summary>
		/// Helper to add multiple templates
		/// </summary>
		/// <param name="inputQueryBuilder"></param>
		/// <param name="templateIds"></param>
		/// <param name="boost"></param>
		/// <param name="occur"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static IQueryBuilder TemplateIds(this IQueryBuilder inputQueryBuilder, ID[] templateIds, float? boost = null, BooleanClause.Occur occur = null, string key = null)
		{
			foreach (ID templateId in templateIds)
			{
				inputQueryBuilder.TemplateId(templateId, occur, boost, key);
			}
			return inputQueryBuilder;
		}

		/// <summary>
		/// Get items derived from the given template at some point in their heirarchy
		/// </summary>
		/// <param name="inputQueryBuilder"></param>
		/// <param name="templateId"></param>
		/// <param name="occur"></param>
		/// <param name="boost"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static TermQuery BaseTemplateId(this IQueryBuilder inputQueryBuilder, ID templateId, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			string luceneTemplateId = templateId.ToLuceneId();
			return inputQueryBuilder.Term(SitecoreFields.TemplatePath, luceneTemplateId, occur, boost, key);
		}
		
		#endregion

		#region [ Name Extensions ]

		public static Query Name(this IQueryBuilder inputQueryBuilder, string value, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			return inputQueryBuilder.Field(SitecoreFields.Name, value, occur, boost, key);
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

		public static Query Language(this IQueryBuilder inputQueryBuilder, Language language, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			string languageString = language.CultureInfo.Name.ToLower();
			KeywordAnalyzer keywordAnalyzer = new KeywordAnalyzer();
			return inputQueryBuilder.Raw(SitecoreFields.Language, languageString, occur, boost, key, keywordAnalyzer);
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

		#region [ Heirarchy Extensions ]

		public static TermQuery Ancestor(this IQueryBuilder inputQueryBuilder, ID ancestorId, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			string ancestorIdString = ancestorId.ToLuceneId();
			return inputQueryBuilder.Term(SitecoreFields.Path, ancestorIdString, occur, boost, key);
		}

		public static TermQuery Parent(this IQueryBuilder inputQueryBuilder, ID parentId, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			string parentIdString = parentId.ToLuceneId();
			return inputQueryBuilder.Term(SitecoreFields.Parent, parentIdString, occur, boost, key);
		}

		#endregion

		#region [ Field Extensions ]

		public static Query Field(this IQueryBuilder inputQueryBuilder, string fieldName, string value, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			if (value.Contains("*"))
			{
				return inputQueryBuilder.WildCard(fieldName.ToLower(), value.ToLower(), occur, boost, key);
			}
			return inputQueryBuilder.Term(fieldName.ToLower(), value.ToLower(), occur, boost, key);
		}

		public static Query Field(this IQueryBuilder inputQueryBuilder, string fieldName, ID value, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			return inputQueryBuilder.Term(fieldName.ToLower(), value.ToLuceneId(), occur, boost, key);
		}

		#endregion

		#region [ Database ]

		public static Query Database(this IQueryBuilder inputQueryBuilder, string value, BooleanClause.Occur occur = null, float? boost = null, string key = null)
		{
			return inputQueryBuilder.Term(SitecoreFields.Database, value.ToLower(), occur, boost, key);
		}

		#endregion

		#region [ Id Extensions ]

		public static string ToLuceneId(this ID itemId)
		{
			return itemId.ToShortID().ToString().ToLowerInvariant();
		}

		#endregion
	}
}
