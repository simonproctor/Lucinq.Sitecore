using Lucene.Net.Search;
using Lucinq.Interfaces;
using Lucinq.Querying;
using Sitecinq.Constants;

namespace Sitecinq.Querying
{
	public class SitecoreQueryBuilder : QueryBuilder
	{
		#region [ Methods ]

		public TermQuery TemplateId(string templateId, float? boost = null, BooleanClause.Occur occur = null, string key = null)
		{
			return Term(SitecoreFields.TemplateId, templateId, occur, boost, key);
		}

		public IQueryBuilder TemplateIds(string[] values, float? boost = null, BooleanClause.Occur occur = null, string key = null)
		{
			foreach (string templateId in values)
			{
				Term(SitecoreFields.TemplateId, templateId, occur, boost, key);
			}
			return this;
		}

		public TermQuery Name(string value, float? boost = null, BooleanClause.Occur occur = null, string key = null)
		{
			return Term(SitecoreFields.Name, value.ToLower(), occur, boost, key);
		}

		public IQueryBuilder Names(string[] values, float? boost = null, BooleanClause.Occur occur = null, string key = null)
		{
			foreach (string templateId in values)
			{
				Term(SitecoreFields.TemplateId, templateId, occur, boost, key);
			}
			return this;
		}

		protected override IQueryBuilder CreateNewChildGroup(BooleanClause.Occur occur = null, BooleanClause.Occur childrenOccur = null)
		{
			return new SitecoreQueryBuilder{ Occur = occur, DefaultChildrenOccur = childrenOccur };
		}

		#endregion
	}
}
