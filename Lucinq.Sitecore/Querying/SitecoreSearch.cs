using Lucene.Net.Store;
using Lucinq.Interfaces;
using Lucinq.Querying;
using Lucinq.SitecoreIntegration.DatabaseManagement;
using Lucinq.SitecoreIntegration.DatabaseManagement.Interfaces;
using Sitecore.Data.Items;

namespace Lucinq.SitecoreIntegration.Querying
{
	public class SitecoreSearch : LuceneItemSearch<SitecoreSearchResult, Item>
	{
		#region [ Constructors ]

		/// <summary>
		/// Convenience constructor with a default context database helper
		/// </summary>
		/// <param name="indexPath">The path to the index</param>
		public SitecoreSearch(string indexPath) : this(indexPath, new DatabaseHelper())
		{
			
		}

		/// <summary>
		/// Allows for dependency injected alternative database helpers
		/// </summary>
		/// <param name="indexPath"></param>
		/// <param name="databaseHelper"></param>
        public SitecoreSearch(string indexPath, IDatabaseHelper databaseHelper)
            : base(indexPath)
		{
            DatabaseHelper = databaseHelper;
		}

        public SitecoreSearch(Directory directory, IDatabaseHelper databaseHelper)
            : base(directory)
        {
            DatabaseHelper = databaseHelper;
        }

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Gets the database helper object
		/// </summary>
		public IDatabaseHelper DatabaseHelper { get; private set; }

		#endregion

	    protected override SitecoreSearchResult GetItemCreator(ILuceneSearchResult searchResult)
	    {
	        return new SitecoreSearchResult(searchResult, DatabaseHelper);
	    }
	}
}
