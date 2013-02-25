using Lucinq.Interfaces;

namespace Sitecinq.Querying
{
	public class SitecoreLuceneSearch
	{
		#region [ Constructors ]

		public SitecoreLuceneSearch(ILuceneSearch luceneSearch)
		{
			LuceneSearch = luceneSearch;
		}

		#endregion

		#region [ Properties ]

		public ILuceneSearch LuceneSearch { get; private set; }

		#endregion

		#region [ Methods ]

		#endregion
	}
}
