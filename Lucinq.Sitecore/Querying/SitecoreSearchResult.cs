using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lucinq.Interfaces;
using Lucinq.SitecoreIntegration.Constants;
using Lucinq.SitecoreIntegration.Interfaces;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Lucinq.SitecoreIntegration.Querying
{
	public class SitecoreSearchResult : ISitecoreSearchResult
	{
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

		public int TotalHits { get { return LuceneSearchResult.TotalHits; } }

		public long ElapsedTimeMs { get; set; }

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Gets a list of items for the documents
		/// </summary>
		/// <returns></returns>
		public ISitecoreItemResult GetPagedItems(int start, int end)
		{
			List<Item> items = new List<Item>();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int difference = end - start;
			int numberAdded = 0;
			int maxCycle = end + (difference*3);
			// Sometimes the items aren't published to web, in this case, continue beyond the original number of results.
			// This currently cycles through 3 times the initial number of rows
			LuceneSearchResult.GetPagedDocuments(start, maxCycle).ForEach(
				document =>
					{
						if (numberAdded >= end)
						{
							return;
						}
						string itemShortId = document.GetValues(SitecoreFields.Id).FirstOrDefault();
						if (String.IsNullOrEmpty(itemShortId))
						{
							return;
						}
						ID itemId = new ID(itemShortId);
						Language itemLanguage = Language.Parse(document.GetValues(SitecoreFields.Language).FirstOrDefault());

						Item item = DatabaseHelper.GetItem(itemId, itemLanguage);
						if (item == null)
						{
							return;
						}
						numberAdded++;
						items.Add(item);
					}
				);
			stopwatch.Stop();
			return new SitecoreItemResult(items) { ElapsedTimeMs = stopwatch.ElapsedMilliseconds };
		}

		#endregion
	}
}
