using Sitecore.ContentSearch;
using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.ContentSearch.Maintenance;

namespace Lucinq.SitecoreIntegration.Sitecore7.Indexing
{
    public class LucinqIndex : LuceneIndex
    {
        #region [ Fields ]

        private readonly string rootPaths;

        #endregion

        #region [ Constructors ]

        public LucinqIndex(string name, string folder, IIndexPropertyStore propertyStore, string rootPaths) : base(name, folder, propertyStore)
        {
            this.rootPaths = rootPaths;
        }

        #endregion

        #region [ Properties ]

        // this is 'new' cause sitecore changed their api between versions - later versions are virtual, earlier release's were not.
        public new virtual IIndexOperations Operations
        {
            get
            {
                string[] rootPathArray = GetRootPaths(rootPaths);
                return new IndexOperations(this, rootPathArray);
            }
        }

        #endregion

        #region [ Methods ]

        protected virtual string[] GetRootPaths(string input)
        {
            string[] rootPathArray;
            if (input.Contains("|"))
            {
                rootPathArray = input.Split('|');
                return rootPathArray;
            }
            rootPathArray = new[] { input };
            return rootPathArray;
        }

        
        #endregion
    }
}
