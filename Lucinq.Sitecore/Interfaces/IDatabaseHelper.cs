using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecinq.Interfaces
{
	public interface IDatabaseHelper
	{
		Database GetDatabase(string name = null);

		Item GetItem(ID itemId, string databaseName = null);
		
		Item GetItem(string itemId, string databaseName = null);
	}
}
