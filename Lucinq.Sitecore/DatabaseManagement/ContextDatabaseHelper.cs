using System;
using Lucinq.SitecoreIntegration.Interfaces;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Lucinq.SitecoreIntegration.DatabaseManagement
{
	public class ContextDatabaseHelper : IDatabaseHelper
	{
		public Database GetDatabase(string name = null)
		{
			if (!String.IsNullOrEmpty(name))
			{
				return Database.GetDatabase(name);
			}
			return Sitecore.Context.Database;
		}

		public Item GetItem(ID itemId, Language language, string databaseName = null)
		{
			return GetDatabase(databaseName).GetItem(itemId, language);
		}

		public Item GetItem(ID itemId, string databaseName = null)
		{
			Language language = Sitecore.Context.Language;
			return GetItem(itemId, language, databaseName);
		}

		public Item GetItem(string itemPath, Language language, string databaseName = null)
		{
			return GetDatabase(databaseName).GetItem(itemPath, language);
		}
	}
}
