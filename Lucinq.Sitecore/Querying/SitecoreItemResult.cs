using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Lucinq.Sitecore.Querying
{
	public class SitecoreItemResult
	{
		#region [ Constructors ]

		public SitecoreItemResult(List<Item> items)
		{
			Items = items;
		}

		#endregion

		#region [ Properties ]

		public List<Item> Items { get; private set; }

		public long ElapsedTimeMs { get; set; }

		#endregion
	}
}
