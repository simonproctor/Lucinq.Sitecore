using System;
using Lucinq.Interfaces;
using Lucinq.Querying;
using Lucinq.SitecoreIntegration.DatabaseManagement;
using Lucinq.SitecoreIntegration.Extensions;
using Lucinq.SitecoreIntegration.Querying;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using NUnit.Framework;
using Sitecore.Data;
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
			ID templateId = new ID(Constants.TestTemplateId);

			IQueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.TemplateId(templateId));
			// queryBuilder.TemplateId(templateId);

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder, 20);

			Assert.Greater(sitecoreSearchResult.LuceneSearchResult.TotalHits, 0);

			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 10);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
					item =>
					{
						Console.WriteLine(item.Name);
						Assert.AreEqual(Constants.TestTemplateId, item.TemplateID.ToString());
					});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		#endregion

		#region [ Failure Tests ]

		[Test]
		public void MoreResultsThanLuceneHits()
		{
			ID templateId = new ID(Constants.TestTemplateId);

			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.TemplateId(templateId));
			// queryBuilder.TemplateId(templateId);

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder, 20);

			Assert.Greater(sitecoreSearchResult.LuceneSearchResult.TotalHits, 0);

			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 20);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			Assert.AreEqual(20, sitecoreItemResult.Items.Count);

			sitecoreItemResult.Items.ForEach(
					item =>
					{
						Console.WriteLine(item.Name);
					});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		#endregion

		#region [ Id Tests ]

		[Test]
		public void GetById()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			ID itemId = new ID(Constants.HomeItemId);
			queryBuilder.Setup(x => x.Id(itemId));

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 10);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
					Assert.AreEqual(itemId, item.ID);
				});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		#endregion

		#region [ Name Tests ]

		[Test]
		public void GetByName()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.Name("bird"));

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 10);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
					Assert.IsTrue(item.Name.IndexOf("bird", StringComparison.InvariantCultureIgnoreCase) >= 0);
				});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		[Test]
		public void GetByNameWildCard()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.Name("*a*"));

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 100);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
				item =>
					{
						Console.WriteLine(item.Name);
						Assert.IsTrue(item.Name.IndexOf("a", StringComparison.InvariantCultureIgnoreCase) >= 0);
					});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		[Test]
		public void GetByLanguage()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			Language language = Language.Parse("de");
			queryBuilder.Setup(x => x.Language(language));

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 100);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
					Assert.AreEqual(language, item.Language);
				});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		#endregion

		#region [ Field ]

		[Test]
		public void GetByFieldValue()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.Field("Display Name", "news"));

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 10);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
					Assert.IsTrue(item["Display Name"].IndexOf("news", StringComparison.InvariantCultureIgnoreCase) >= 0);
				});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		#endregion

		#region [ Heirarchy Extensions ]


		[Test]
		public void GetByAncestor()
		{
			Ancestor();
		}

		private void Ancestor(bool displayOutput = true)
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.Ancestor(new ID(Constants.HomeItemId)));

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);

			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 9);

			Assert.Greater(sitecoreSearchResult.TotalHits, 0);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			if (displayOutput)
			{
				sitecoreItemResult.Items.ForEach(
					item =>
						{
							Console.WriteLine(item.Name);
						});
			}

			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}


		[Test]
		public void GetByParent()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			ID parentId = new ID(Constants.HomeItemId);
			queryBuilder.Setup(x => x.Parent(parentId));

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 9);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
					Assert.AreEqual(parentId, item.Parent.ID);
				});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		[Test]
		public void GetFromDerivedTemplate()
		{
			QueryBuilder queryBuilder = new QueryBuilder();

			ID templateId = new ID(Constants.DerivedTemplateId);
			queryBuilder.Setup(x => x.BaseTemplateId(templateId));

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 9);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
				});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		#endregion

		#region [ Performance Tests ]

		[Test]
		public void RepeatAncestorTests()
		{
			Console.WriteLine("The return from sitecore and lucene should get quicker");
			Console.WriteLine();

			Console.WriteLine("Pass 1");
			Ancestor(false);
			Console.WriteLine();

			Console.WriteLine("Pass 2");
			Ancestor(false);
			Console.WriteLine();

			Console.WriteLine("Pass 3");
			Ancestor(false);
		}
		#endregion

		#region [ Paging Tests ]
		
		[Test]
		public  void Paging()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.Ancestor(new ID(Constants.HomeItemId)));

			ISitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);

			ISitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 9);

			Assert.AreEqual(10, sitecoreItemResult.Items.Count);

			ISitecoreItemResult sitecoreItemResult2 = sitecoreSearchResult.GetPagedItems(0, 19);

			Assert.AreEqual(20, sitecoreItemResult2.Items.Count);

			ISitecoreItemResult sitecoreItemResult3 = sitecoreSearchResult.GetPagedItems(10, 29);

			Assert.AreEqual(20, sitecoreItemResult3.Items.Count);
		}
		#endregion
	}
}
