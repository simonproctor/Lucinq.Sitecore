using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Lucinq.SitecoreIntegration.Querying.Interfaces
{
	public interface ISitecoreItemResult
	{
		List<Item> Items { get; }
		
		long ElapsedTimeMs { get; set; }

		int TotalHits { get; set; }
	}
}