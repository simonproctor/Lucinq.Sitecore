using Sitecore.Data;
using Sitecore.Data.Items;

namespace Lucinq.Sitecore.Interfaces
{
	public interface IDatabaseHelper
	{
		Database GetDatabase(string name = null);

		Item GetItem(ID itemId, string databaseName = null);
		
		Item GetItem(string itemId, string databaseName = null);
	}
}
