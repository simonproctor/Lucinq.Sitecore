using System;
using System.Linq;
using Glass.Mapper.Sc;
using Lucene.Net.Documents;
using Lucinq.Interfaces;
using Lucinq.Querying;
using Lucinq.SitecoreIntegration.Constants;
using Sitecore.Data;
using Sitecore.Globalization;

namespace Lucinq.GlassMapper.SitecoreIntegration
{
	public class GlassSearchResult<T> : ItemSearchResult<T> where T : class
	{
		#region [ Constructors ]

		public GlassSearchResult(ISitecoreService sitecoreService, ILuceneSearchResult luceneSearchResult) : base(luceneSearchResult)
		{
			SitecoreService = sitecoreService;
		}

		#endregion

		#region [ Properties ]

		public ISitecoreService SitecoreService { get; private set; }

		#endregion

		#region [ Methods ]

	    public override T GetItem(Document document)
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

			return SitecoreService.GetItem<T>(itemId.ToGuid(), itemLanguage);
		}

		#endregion
	}
}
