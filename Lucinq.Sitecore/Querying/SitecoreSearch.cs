using System;
using Lucene.Net.Search;
using Lucinq.Interfaces;
using Lucinq.Querying;
using Lucinq.SitecoreIntegration.Interfaces;

namespace Lucinq.SitecoreIntegration.Querying
{
	public class SitecoreSearch : IDisposable
	{
		#region [ Constructors ]

		public SitecoreSearch(string indexPath, IDatabaseHelper databaseHelper) : this(new LuceneSearch(indexPath), databaseHelper)
		{
			
		}
		
		public SitecoreSearch(ILuceneSearch<LuceneSearchResult> luceneSearch, IDatabaseHelper databaseHelper)
		{
			LuceneSearch = luceneSearch;
			DatabaseHelper = databaseHelper;
		}

		#endregion

		#region [ Properties ]

		public ILuceneSearch<LuceneSearchResult> LuceneSearch { get; private set; }

		public IDatabaseHelper DatabaseHelper { get; private set; }

		#endregion

		#region [ Methods ]

		public SitecoreSearchResult Execute(Query query, int noOfResults = Int32.MaxValue - 1, Sort sort = null)
		{
			var luceneResult = LuceneSearch.Execute(query, noOfResults, sort);
			return new SitecoreSearchResult(luceneResult, DatabaseHelper) { ElapsedTimeMs = luceneResult.ElapsedTimeMs };
		}

		public SitecoreSearchResult Execute(IQueryBuilder queryBuilder, int noOfResults = Int32.MaxValue - 1, Sort sort = null)
		{
			return Execute(queryBuilder.Build(), noOfResults, sort);
		}

		public void Dispose()
		{
			LuceneSearch.IndexSearcher.Dispose();
		}

		#endregion
	}
}
