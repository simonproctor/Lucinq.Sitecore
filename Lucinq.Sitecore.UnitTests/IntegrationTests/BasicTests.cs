using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lucinq.Querying;
using Lucinq.SitecoreIntegration.Extensions;
using Lucinq.SitecoreIntegration.Querying;
using NUnit.Framework;
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

			SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 10);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
					item =>
					{
						Console.WriteLine(item.Name);
						Assert.AreEqual("{8A255FA5-4198-4FAA-B56D-3DF6116F9342}", item.TemplateID.ToString());
					});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		#endregion

		#region [ Id Tests ]

		[Ignore("Needs Finishing")]
		[Test]
		public void GetById()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			ID itemId = new ID("{14CEA008-749F-46FA-8CA1-C929B92176B7}");
			queryBuilder.Setup(x => x.Id(itemId));

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 10);

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
			queryBuilder.Setup(x => x.Name("story"));

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 10);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
				item =>
				{
					Console.WriteLine(item.Name);
					Assert.IsTrue(item.Name.IndexOf("story", StringComparison.InvariantCultureIgnoreCase) >= 0);
				});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		[Test]
		public void GetByNameWildCard()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.NameWildCard("*loader*"));

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 100);

			Console.WriteLine("Lucene Elapsed Time: {0}", sitecoreSearchResult.ElapsedTimeMs);
			Console.WriteLine("Sitecore Elapsed Time: {0}", sitecoreItemResult.ElapsedTimeMs);

			sitecoreItemResult.Items.ForEach(
				item =>
					{
						Console.WriteLine(item.Name);
						Assert.IsTrue(item.Name.IndexOf("loader", StringComparison.InvariantCultureIgnoreCase) > 0);
					});
			Assert.Greater(sitecoreItemResult.Items.Count, 0);
		}

		[Ignore("Needs Finishing")]
		[Test]
		public void GetByLanguage()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			Language language = Language.Parse("en-gb");
			queryBuilder.Setup(x => x.Language(language));

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 100);

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

		#region [ Heirarchy Extensions ]


		[Test]
		public void GetByAncestor()
		{
			Ancestor();
		}

		private void Ancestor(bool displayOutput = true)
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			queryBuilder.Setup(x => x.Ancestor(new ID("{14CEA008-749F-46FA-8CA1-C929B92176B7}")));

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 10);

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


		[Ignore("Needs Finishing")]
		[Test]
		public void GetByParent()
		{
			QueryBuilder queryBuilder = new QueryBuilder();
			ID parentId = new ID("{14CEA008-749F-46FA-8CA1-C929B92176B7}");
			queryBuilder.Setup(x => x.Parent(parentId));

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 9);

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

			ID templateId = new ID("{6C167AF3-090C-4E8A-8FC8-3DEC15B9EC19}");
			queryBuilder.Setup(x => x.BaseTemplateId(templateId));

			SitecoreSearchResult sitecoreSearchResult = search.Execute(queryBuilder);
			Assert.Greater(sitecoreSearchResult.TotalHits, 0);
			SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 9);

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
	}
}
