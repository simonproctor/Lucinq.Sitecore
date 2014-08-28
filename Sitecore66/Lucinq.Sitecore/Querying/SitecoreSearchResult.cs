using System;
using System.Linq;
using Lucene.Net.Documents;
using Lucinq.Interfaces;
using Lucinq.Querying;
using Lucinq.SitecoreIntegration.Constants;
using Lucinq.SitecoreIntegration.DatabaseManagement.Interfaces;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Lucinq.SitecoreIntegration.Querying
{
	public class SitecoreSearchResult : ItemSearchResult<Item>, ISitecoreSearchResult
	{
		#region [ Constructors ]

		public SitecoreSearchResult(ILuceneSearchResult searchResult, IDatabaseHelper databaseHelper) : base(searchResult)
		{
			DatabaseHelper = databaseHelper;
		}

		#endregion

		#region [ Properties ]

        public SitecoreMode SitecoreMode { get; private set; }

		public IDatabaseHelper DatabaseHelper { get; private set; }

		#endregion

		#region [ Methods ]


		/// <summary>
		/// Gets an item from mthe document
		/// </summary>
		/// <param name="document">The lucene document to use</param>
		/// <returns></returns>
		public override Item GetItem(Document document)
		{
			string itemShortId = document.GetValues(SitecoreFields.Id).FirstOrDefault();
			if (String.IsNullOrEmpty(itemShortId))
			{
				return null;
			}
			ID itemId = new ID(itemShortId);
			string language = document.GetValues(SitecoreFields.Language).FirstOrDefault();
			if (String.IsNullOrEmpty(language))
			{
				throw new Exception("The language could not be retrieved from the lucene return");
			}
			Language itemLanguage = Language.Parse(language);

			Item item = DatabaseHelper.GetItem(itemId, itemLanguage);
		    if (item == null)
		    {
		        return null;
		    }
			return item.Versions.Count > 0 ? item : null;
		}

	    public Item GetItem()
	    {
	        var firstDocument = LuceneSearchResult.GetTopItems().FirstOrDefault();
	        return firstDocument == null ? null : GetItem(firstDocument);
	    }

		#endregion
	}
}
