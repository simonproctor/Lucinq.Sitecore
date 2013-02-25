using Lucinq.Querying;
using NUnit.Framework;
using Sitecinq.Querying;

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
			
		}

		#endregion
	}
}
