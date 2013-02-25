using System;
using System.Collections.Generic;
using Lucinq.Querying;
using Lucinq.Sitecore.Extensions;
using Lucinq.Sitecore.Querying;
using NUnit.Framework;
using Sitecinq.IntegrationTests;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

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
			items.ForEach(
					item =>
					{
						Console.WriteLine(item.Name);
						Assert.AreEqual("{8A255FA5-4198-4FAA-B56D-3DF6116F9342}", item.TemplateID.ToString());
					});
			Assert.Greater(items.Count, 0);
		}

		#endregion

		#region [ Id Tests ]

		[Test]
		public void GetById()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			ID itemId = new ID("{14CEA008-749F-46FA-8CA1-C929B92176B7}");
			queryBuilder.Setup(x => x.Id(itemId));
			//queryBuilder.Name("JCB");
			//

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			List<Item> items = sitecoreSearchResult.GetPagedItems(0, 10);
			items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
					Assert.IsTrue(item.Name.IndexOf("JCB", StringComparison.InvariantCultureIgnoreCase) >= 0);
				});
			Assert.Greater(items.Count, 0);
		}

		#endregion

		#region [ Name Tests ]

		[Test]
		public void GetByName()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.Name("JCB"));
			//queryBuilder.Name("JCB");
			//

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			List<Item> items = sitecoreSearchResult.GetPagedItems(0, 10);
			items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
					Assert.IsTrue(item.Name.IndexOf("JCB", StringComparison.InvariantCultureIgnoreCase) >= 0);
				});
			Assert.Greater(items.Count, 0);
		}

		[Test]
		public void GetByNameWildCard()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.NameWildCard("*loader*"));

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			List<Item> items = sitecoreSearchResult.GetPagedItems(0, 100);
			items.ForEach(
				item =>
					{
						Console.WriteLine(item.Name);
						Assert.IsTrue(item.Name.IndexOf("loader", StringComparison.InvariantCultureIgnoreCase) > 0);
					});
			Assert.Greater(items.Count, 0);
		}

		[Test]
		public void GetByLanguage()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			Language language = Language.Parse("en-gb");
			queryBuilder.Setup(x => x.Language(language));

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			List<Item> items = sitecoreSearchResult.GetPagedItems(0, 100);
			items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
					Assert.AreEqual(language, item.Language);
				});
			Assert.Greater(items.Count, 0);
		}

		#endregion
	}
}
