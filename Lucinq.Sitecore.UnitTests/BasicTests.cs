using System;
using Lucinq.Querying;
using NUnit.Framework;
using Sitecinq.Querying;
using Sitecore.Data;

namespace Sitecinq.IntegrationTests
{
	[TestFixture]
	public class QueryTests
	{
		#region [ Fields ]

		private LuceneSearch search;

		#endregion

		#region [ Setup / Teardown ]

		[TestFixtureSetUp]
		public void Setup()
		{
			search = new LuceneSearch(Constants.IndexPath);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			search.Dispose();
		}

		#endregion

		#region [ Template Tests ]

		[Test]
		public void GetByTemplateId()
		{
			SitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
			ID templateId = new ID("{8A255FA5-4198-4FAA-B56D-3DF6116F9342}");
			queryBuilder.TemplateId(templateId.ToShortID().ToString());
			var luceneResult = search.Execute(queryBuilder, 20);
			Assert.Greater(luceneResult.TotalHits, 0);
			SitecoreSearchResult result = new SitecoreSearchResult(luceneResult, new TestDatabaseHelper());
			var items = result.GetItems(luceneResult.GetPagedDocuments(0, 10));
			foreach (var item in items)
			{
				Console.WriteLine(item.Name);
			}
			Assert.Greater(items.Count, 0);
		}

		[Test]
		public void GetByName()
		{
			SitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
			queryBuilder.Name("JCB");
			var luceneResult = search.Execute(queryBuilder, 20);
			Assert.Greater(luceneResult.TotalHits, 0);
			SitecoreSearchResult result = new SitecoreSearchResult(luceneResult, new TestDatabaseHelper());
			var items = result.GetItems(luceneResult.GetPagedDocuments(0, 10));
			foreach (var item in items)
			{
				Console.WriteLine(item.Name);
			}
			Assert.Greater(items.Count, 0);
		}

		#endregion
	}
}
