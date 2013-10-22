using Lucinq.Extensions;
using Lucinq.Interfaces;
using Lucinq.Querying;
using Lucinq.SitecoreIntegration.Extensions;
using Lucinq.SitecoreIntegration.Querying;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Lucinq.SitecoreIntegration.Sitecore7.Extensions;
using NUnit.Framework;

namespace Lucinq.SitecoreIntegration.Sitecore7.UnitTests
{
    [TestFixture]
    public class FieldNameUnitTests
    {
        #region [ Field Name Tests ]

        [Test]
        public void GetIndexFieldName()
        {
            TestSitecore7Class testClass = new TestSitecore7Class();
            string fieldName = FieldExtensions.GetFieldName<TestSitecore7Class>(t => t.TemplateId);
            Assert.AreEqual("_template", fieldName);
            Assert.AreEqual("_template", testClass.GetFieldName(t => t.TemplateId));
        }

        [Test]
        public void GetIndexFieldNameWhereNoneSpecified()
        {
            TestSitecore7Class testClass = new TestSitecore7Class();
            string fieldName = FieldExtensions.GetFieldName<TestSitecore7Class>(t => t.UnspecifiedField);
            Assert.AreEqual("unspecifiedfield", fieldName);
            Assert.AreEqual("unspecifiedfield", testClass.GetFieldName(t => t.UnspecifiedField));
        }

        #endregion

        #region [ Field Extensions ]

        [Test]
        public void Term()
        {
            ISitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
            queryBuilder.Term<TestSitecore7Class>(t => t.Content, "value");

            IQueryBuilder queryBuilder2 = new QueryBuilder();
            queryBuilder2.Term("_content", "value");

            Assert.AreEqual(queryBuilder2.Build().ToString(), queryBuilder.Build().ToString());
        }

        [Test]
        public void Terms()
        {
            ISitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
            queryBuilder.Terms<TestSitecore7Class>(t => t.Content, new[] { "value", "value2" });

            IQueryBuilder queryBuilder2 = new QueryBuilder();
            queryBuilder2.Terms("_content", new []{ "value", "value2"});

            Assert.AreEqual(queryBuilder2.Build().ToString(), queryBuilder.Build().ToString());
        }

        [Test]
        public void Keyword()
        {
            ISitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
            queryBuilder.Keyword<TestSitecore7Class>(t => t.Content, "value");

            IQueryBuilder queryBuilder2 = new QueryBuilder();
            queryBuilder2.Keyword("_content", "value");

            Assert.AreEqual(queryBuilder2.Build().ToString(), queryBuilder.Build().ToString());
        }


        [Test]
        public void FieldById()
        {
            ISitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
            queryBuilder.Field<TestSitecore7Class>(t => t.Content, SitecoreIds.HomeItemId);

            IQueryBuilder queryBuilder2 = new QueryBuilder();
            queryBuilder2.Term("_content", SitecoreIds.HomeItemId.ToLuceneId());

            Assert.AreEqual(queryBuilder2.Build().ToString(), queryBuilder.Build().ToString());
        }

        [Test]
        public void Fuzzy()
        {
            ISitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
            queryBuilder.Fuzzy<TestSitecore7Class>(t => t.DatabaseName, "value");

            IQueryBuilder queryBuilder2 = new QueryBuilder();
            queryBuilder2.Fuzzy("_database", "value");

            Assert.AreEqual(queryBuilder2.Build().ToString(), queryBuilder.Build().ToString());
        }

        [Test]
        public void Phrase()
        {
            ISitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
            var phrase = queryBuilder.Phrase(2);
            phrase.AddTerm<TestSitecore7Class>(t => t.Content, "value");

            IQueryBuilder queryBuilder2 = new QueryBuilder();
            var phrase2 = queryBuilder2.Phrase(2);
            phrase2.AddTerm("_content", "value");

            Assert.AreEqual(queryBuilder2.Build().ToString(), queryBuilder.Build().ToString());
            
        }

        [Test]
        public void WildCard()
        {
            ISitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
            queryBuilder.WildCard<TestSitecore7Class>(t => t.Content, "value*");

            IQueryBuilder queryBuilder2 = new QueryBuilder();
            queryBuilder2.WildCard("_content", "value*");

            Assert.AreEqual(queryBuilder2.Build().ToString(), queryBuilder.Build().ToString());
        }

        [Test]
        public void WildCards()
        {
            ISitecoreQueryBuilder queryBuilder = new SitecoreQueryBuilder();
            queryBuilder.WildCards<TestSitecore7Class>(t => t.Content, new[] {"value*", "value1*"});

            IQueryBuilder queryBuilder2 = new QueryBuilder();
            queryBuilder2.WildCards("_content", new[] { "value*", "value1*" });

            Assert.AreEqual(queryBuilder2.Build().ToString(), queryBuilder.Build().ToString());
        }

        #endregion
    }
}
