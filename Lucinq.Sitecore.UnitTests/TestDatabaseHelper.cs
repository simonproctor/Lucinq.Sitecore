using System;
using Sitecinq.Interfaces;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecinq.IntegrationTests
{
	public class TestDatabaseHelper : IDatabaseHelper
	{
		public Database GetDatabase(string name = null)
		{
			if (String.IsNullOrEmpty(name))
			{
				name = "web";
			}
			return Database.GetDatabase(name);
		}

		public Item GetItem(string itemId, string databaseName = null)
		{
			return GetItem(new ID(itemId));
		}

		public Item GetItem(ID itemId, string databaseName = null)
		{
			return GetDatabase(databaseName).GetItem(itemId);
		}
	}
}
