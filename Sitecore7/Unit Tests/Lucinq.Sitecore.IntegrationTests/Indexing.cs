using NUnit.Framework;
using Sitecore.Search;

namespace Lucinq.SitecoreIntegration.IntegrationTests
{
	[TestFixture]
	public class Indexing
	{
		#region [ Constants ]

		private const string IndexName = "lucinq_master_index";

		#endregion

		[Ignore("Index doesn't need rebuilding every time")]
		[Test]
		public void RebuildSearchIndex()
		{
			Index searchIndex = SearchManager.GetIndex(IndexName);
			searchIndex.Rebuild();
		}
	}
}
