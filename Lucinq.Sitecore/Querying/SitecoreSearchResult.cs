using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Lucinq.Interfaces;
using Sitecinq.Constants;
using Sitecinq.Interfaces;
using Sitecore.Data.Items;

namespace Sitecinq.Querying
{
	public class SitecoreSearchResult
	{
		#region [ Fields ]

		#endregion

		#region [ Constructors ]

		public SitecoreSearchResult(ISearchResult searchResult, IDatabaseHelper databaseHelper)
		{
			DatabaseHelper = databaseHelper;
			SearchResult = searchResult;
		}

		#endregion

		#region [ Properties ]

		public IDatabaseHelper DatabaseHelper { get; private set; }

		public ISearchResult SearchResult { get; private set; }

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Gets a list of items for the documents
		/// </summary>
		/// <param name="documents"></param>
		/// <returns></returns>
		public List<Item> GetItems(List<Document> documents)
		{
			List<Item> items = new List<Item>();
			documents.ForEach(
				document =>
					{
						string itemId = document.GetValues(SitecoreFields.Id).FirstOrDefault();
						if (String.IsNullOrEmpty(itemId))
						{
							return;
						}
						Item item = DatabaseHelper.GetItem(itemId);
						items.Add(item);
					}
				);
			return items;
		}

		#endregion
	}
}
