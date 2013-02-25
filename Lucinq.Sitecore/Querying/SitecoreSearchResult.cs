using System;
using System.Collections.Generic;
using System.Linq;
using Lucinq.Interfaces;
using Lucinq.Sitecore.Constants;
using Lucinq.Sitecore.Interfaces;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Lucinq.Sitecore.Querying
{
	public class SitecoreSearchResult : ISearchResult
	{
		#region [ Fields ]

		#endregion

		#region [ Constructors ]

		public SitecoreSearchResult(ILuceneSearchResult searchResult, IDatabaseHelper databaseHelper)
		{
			DatabaseHelper = databaseHelper;
			LuceneSearchResult = searchResult;
		}

		#endregion

		#region [ Properties ]

		public IDatabaseHelper DatabaseHelper { get; private set; }

		public ILuceneSearchResult LuceneSearchResult { get; private set; }

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Gets a list of items for the documents
		/// </summary>
		/// <returns></returns>
		public List<Item> GetPagedItems(int start, int end)
		{
			List<Item> items = new List<Item>();
			LuceneSearchResult.GetPagedDocuments(start, end).ForEach(
				document =>
					{
						string itemShortId = document.GetValues(SitecoreFields.Id).FirstOrDefault();
						if (String.IsNullOrEmpty(itemShortId))
						{
							return;
						}
						ID itemId = new ID(itemShortId);
						Item item = DatabaseHelper.GetItem(itemId);
						items.Add(item);
					}
				);
			return items;
		}

		#endregion

		public int TotalHits { get { return LuceneSearchResult.TotalHits; } }
	}
}
