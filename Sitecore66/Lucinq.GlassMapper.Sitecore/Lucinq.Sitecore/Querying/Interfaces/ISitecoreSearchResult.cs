using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucinq.Interfaces;
using Lucinq.SitecoreIntegration.DatabaseManagement.Interfaces;
using Sitecore.Data.Items;

namespace Lucinq.SitecoreIntegration.Querying
{
    public interface ISitecoreSearchResult
    {
        SitecoreMode SitecoreMode { get; }
        IDatabaseHelper DatabaseHelper { get; }
        int TotalHits { get; }
        long ElapsedTimeMs { get; set; }
        List<Item> Items { get; }

        /// <summary>
        /// Gets an item from mthe document
        /// </summary>
        /// <param name="document">The lucene document to use</param>
        /// <returns></returns>
        Item GetItem(Document document);

        Item GetItem();
        IEnumerator<Item> GetEnumerator();
        IItemResult<Item> GetTopItems();
        IItemResult<Item> GetRange(int start, int end);
        IItemResult<Item> GetPagedItems(int start, int end);
    }
}