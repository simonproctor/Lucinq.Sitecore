using Lucinq.Interfaces;

namespace Lucinq.SitecoreIntegration.Querying.Interfaces
{
	public interface ISitecoreSearchResult
	{
		ILuceneSearchResult LuceneSearchResult { get; }
		int TotalHits { get; }
		long ElapsedTimeMs { get; set; }

		/// <summary>
		/// Gets a list of items for the documents
		/// </summary>
		/// <returns></returns>
		ISitecoreItemResult GetPagedItems(int start, int end);
	}
}