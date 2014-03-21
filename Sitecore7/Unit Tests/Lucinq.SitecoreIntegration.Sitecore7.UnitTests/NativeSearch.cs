﻿using System;
using System.Diagnostics;
using System.Linq;
using Lucene.Net.Store;
using Lucinq.SitecoreIntegration.DatabaseManagement;
using Lucinq.SitecoreIntegration.Querying;
using Lucinq.SitecoreIntegration.Sitecore7.Extensions;
using NUnit.Framework;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Pipelines;
using Directory = System.IO.Directory;

namespace Lucinq.SitecoreIntegration.Sitecore7.UnitTests
{
    [TestFixture]
    public class NativeSearch
    {
        private SitecoreSearch sitecoreSearch;
        private SitecoreSearch undisposedSitecoreSearch;
        private static ID makesPageId = new ID("{CF059C5C-DE5B-4C6B-9794-C00CE5A5F140}");
        private FSDirectory directory;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            Sitecore.Configuration.State.HttpRuntime.AppDomainAppPath = Directory.GetCurrentDirectory();
            CorePipeline.Run("initialize", new PipelineArgs());
            Console.WriteLine("Initialized");
            sitecoreSearch = new SitecoreSearch(Sitecore.Configuration.Settings.IndexFolder + "\\lucinq_master_index", new DatabaseHelper("master"));
            directory = FSDirectory.Open(Sitecore.Configuration.Settings.IndexFolder + "\\lucinq_master_index");
            undisposedSitecoreSearch = new SitecoreSearch(directory, new DatabaseHelper("master"));
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            directory.Dispose();
        }

        [Test]
        public void ContentSearch()
        {
            GetSitecoreResults("Ford");
        }

        [Test]
        public void LucinqEquivalent()
        {
            GetLucinqEquivalentResults("Ford");
        }

        [Test]
        public void LucinqUndisposedEquivalent()
        {
            GetLucinqEquivalentUndisposedResults("Ford");
        }

        [Test]
        public void RepeatedLucinqResults()
        {
            LucinqEquivalent();
            LucinqEquivalent();
            LucinqEquivalent();
            LucinqEquivalent();
            LucinqEquivalent();
        }

        [Test]
        public void RepeatedLucinqUndisposedResults()
        {
            LucinqUndisposedEquivalent();
            LucinqUndisposedEquivalent();
            LucinqUndisposedEquivalent();
            LucinqUndisposedEquivalent();
            LucinqUndisposedEquivalent();
        }

        [Test]
        public void RepeatedLucinqProper()
        {
            LucinqProper();
            LucinqProper();
            LucinqProper();
            LucinqProper();
            LucinqProper();
        }

        [Test]
        public void RepeatedNativeResults()
        {
            ContentSearch();
            ContentSearch();
            ContentSearch();
            ContentSearch();
            ContentSearch();
        }

        [Test]
        public void RepeatedMixedProperResults()
        {
            GetLucinqProperResults("Ford");
            GetLucinqProperResults("Volkswagen");
            GetLucinqProperResults("Mazda");
            GetLucinqProperResults("Audi");
            GetLucinqProperResults("Bmw");
        }

        [Test]
        public void RepeatedSideBySideProperResults()
        {
            Console.WriteLine("Ignore - Getting Both Warmed Up");
            GetLucinqProperResults("Ford");
            GetSitecoreResults("Ford");

            Console.WriteLine("Right.... GO!!");
            GetLucinqProperResults("Ford", false);
            GetSitecoreResults("Ford", false);
            GetLucinqProperResults("Volkswagen", false);
            GetSitecoreResults("Volkswagen", false);
            GetLucinqProperResults("Mazda", false);
            GetSitecoreResults("Mazda", false);
            GetLucinqProperResults("Audi", false);
            GetSitecoreResults("Audi", false);
            GetLucinqProperResults("Bmw", false);
            GetSitecoreResults("Bmw", false);

            Console.WriteLine("Right.... Pass 2!!");
            GetLucinqProperResults("Ford", false);
            GetSitecoreResults("Ford", false);
            GetLucinqProperResults("Volkswagen", false);
            GetSitecoreResults("Volkswagen", false);
            GetLucinqProperResults("Mazda", false);
            GetSitecoreResults("Mazda", false);
            GetLucinqProperResults("Audi", false);
            GetSitecoreResults("Audi", false);
            GetLucinqProperResults("Bmw", false);
            GetSitecoreResults("Bmw", false);
        }

        [Test]
        public void RepeatedMixedProperUndisposedResults()
        {
            GetLucinqEquivalentUndisposedResults("Ford");
            GetLucinqEquivalentUndisposedResults("Volkswagen");
            GetLucinqEquivalentUndisposedResults("Mazda");
            GetLucinqEquivalentUndisposedResults("Audi");
            GetLucinqEquivalentUndisposedResults("Bmw");
        }

        [Test]
        public void RepeatedMixedLucinqEquivalentResults()
        {
            GetLucinqEquivalentResults("Ford");
            GetLucinqEquivalentResults("Volkswagen");
            GetLucinqEquivalentResults("Mazda");
            GetLucinqEquivalentResults("Audi");
            GetLucinqEquivalentResults("Bmw");
        }

        [Test]
        public void RepeatedMixedNativeResults()
        {
            GetSitecoreResults("Ford");
            GetSitecoreResults("Volkswagen");
            GetSitecoreResults("Mazda");
            GetSitecoreResults("Audi");
            GetSitecoreResults("Bmw");
        }

        [Test]
        public void LucinqProper()
        {
            GetLucinqProperResults("Ford");
        }

        private void GetLucinqProperResults(string itemName, bool showOutput = true)
        {
            Console.WriteLine("Lucinq:{0}", itemName);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            SitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder(t => t.DescendantOf(SitecoreIds.HomeItemId), t => t.TemplateId(SitecoreIds.AdvertTemplateId));
            queryBuilder.Term<SearchResultItem>(t => t.Name, itemName);
            Console.WriteLine("Query set up: {0}", stopWatch.ElapsedMilliseconds);
            var result = sitecoreSearch.Execute(queryBuilder);
            Console.WriteLine("Total Search Results {0}: {1}", result.TotalHits, stopWatch.ElapsedMilliseconds);
            foreach (var searchResultItem in result.GetPagedItems(0, 50))
            {
                if (!showOutput)
                {
                    continue;
                }
                Console.WriteLine(searchResultItem.Name);
            }
            Console.WriteLine("Done: {0}\r\n", stopWatch.ElapsedMilliseconds);
        }


        private void GetLucinqEquivalentResults(string itemName)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            SitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder(t => t.DescendantOf(SitecoreIds.HomeItemId), t => t.TemplateId(SitecoreIds.AdvertTemplateId));
            queryBuilder.Term<SearchResultItem>(t => t.Name, itemName);
            Console.WriteLine("Query set up: {0}", stopWatch.ElapsedMilliseconds);
            var result = sitecoreSearch.Execute(queryBuilder);
            Console.WriteLine("Total Search Results {0}: {1}", result.TotalHits, stopWatch.ElapsedMilliseconds);
            foreach (var searchResultItem in result.GetPagedItems(0, 50))
            {
                Console.WriteLine(searchResultItem.Name);
            }
            Console.WriteLine("Done: {0}\r\n", stopWatch.ElapsedMilliseconds);
        }

        private void GetLucinqEquivalentUndisposedResults(string itemName)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            SitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder(t => t.DescendantOf(SitecoreIds.HomeItemId), t => t.TemplateId(SitecoreIds.AdvertTemplateId));
            queryBuilder.Term<SearchResultItem>(t => t.Name, itemName);
            Console.WriteLine("Query set up: {0}", stopWatch.ElapsedMilliseconds);
            var result = undisposedSitecoreSearch.Execute(queryBuilder);
            Console.WriteLine("Total Search Results {0}: {1}", result.TotalHits, stopWatch.ElapsedMilliseconds);
            foreach (var searchResultItem in result.GetPagedItems(0, 50))
            {
                Console.WriteLine(searchResultItem.Name);
            }
            Console.WriteLine("Done: {0}\r\n", stopWatch.ElapsedMilliseconds);
        }

        private static void GetSitecoreResults(string itemName, bool showOutput = true)
        {
            Console.WriteLine("Sitecore 7:{0}", itemName);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var context2 = ContentSearchManager.GetIndex("lucinq_master_index").CreateSearchContext())
            {
                IQueryable<SearchResultItem> results =
                    context2.GetQueryable<SearchResultItem>().Where(item => item.Name == itemName && item.Paths.Contains(SitecoreIds.HomeItemId) && item.TemplateId == SitecoreIds.AdvertTemplateId);
                Console.WriteLine("Query set up: {0}", stopWatch.ElapsedMilliseconds);
                var result = results.GetResults();
                Console.WriteLine("Total Search Results {0}: {1}", result.TotalSearchResults, stopWatch.ElapsedMilliseconds);
                foreach (var searchResultItem in results.Take(50))
                {
                    if (!showOutput)
                    {
                        continue;
                    }
                    Console.WriteLine(searchResultItem.Name);
                }
                Console.WriteLine("Done: {0}\r\n", stopWatch.ElapsedMilliseconds);
            }
        }
    }
}