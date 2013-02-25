using System;
using System.Collections.Generic;
using Lucinq.Querying;
using Lucinq.Sitecore.Extensions;
using Lucinq.Sitecore.Querying;
using NUnit.Framework;
using Sitecinq.IntegrationTests;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Lucinq.Sitecore.UnitTests.IntegrationTests
{
	[TestFixture]
	public class QueryTests
	{
		#region [ Fields ]

		private SitecoreSearch search;

		#endregion

		#region [ Setup / Teardown ]

		[TestFixtureSetUp]
		public void Setup()
		{
			search = new SitecoreSearch(Constants.IndexPath, new TestDatabaseHelper());
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
			ID templateId = new ID("{8A255FA5-4198-4FAA-B56D-3DF6116F9342}");

			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.TemplateId(templateId));
			// queryBuilder.TemplateId(templateId);

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder, 20);

			Assert.Greater(sitecoreSearchResult.LuceneSearchResult.TotalHits, 0);

			List<Item> items = sitecoreSearchResult.GetPagedItems(0, 10);
			foreach (Item item in items)
			{
				Console.WriteLine(item.Name);
			}
			Assert.Greater(items.Count, 0);
		}

		[Test]
		public void GetByName()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.Name("JCB"));
			//queryBuilder.Name("JCB");
			//

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder, 20);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			List<Item> items = sitecoreSearchResult.GetPagedItems(0, 10);
			foreach (Item item in items)
			{
				Console.WriteLine(item.Name);
			}
			Assert.Greater(items.Count, 0);
		}

		#endregion
	}
}
