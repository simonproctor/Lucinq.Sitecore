using Lucinq.Interfaces;

namespace Lucinq.SitecoreIntegration.Querying.Interfaces
{
	public interface ISitecoreSearchResult : ISearchResult
	{
		ILuceneSearchResult LuceneSearchResult { get; }

		/// <summary>
		/// Gets a list of items for the documents
		/// </summary>
		/// <returns></returns>
		ISitecoreItemResult GetPagedItems(int start, int end, int multiplier = 3);
	}
}