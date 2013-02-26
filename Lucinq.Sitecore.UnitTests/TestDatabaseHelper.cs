using System;
using Lucinq.SitecoreIntegration.Interfaces;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Lucinq.Sitecore.UnitTests
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

		public Item GetItem(ID itemId, Language language, string databaseName = null)
		{
			return GetDatabase(databaseName).GetItem(itemId, language);
		}

		public Item GetItem(ID itemId, string databaseName = null)
		{
			Language language = Language.Parse("en-GB");
			return GetItem(itemId, language, databaseName);
		}

		public Item GetItem(string itemPath, Language language, string databaseName = null)
		{
			return GetDatabase(databaseName).GetItem(itemPath, language);
		}
	}
}
