Lucinq.Sitecore - A Fluent / Expressive API for Lucene.Net With Sitecore
=================================================

This project is based upon Lucene.Net and aims to give fluent style API to Lucene.Net used from the standard Sitecore indexes. The primary purpose of which is to make Lucene a little less verbose 
to accomplish most tasks whilst retaining the power and speed of Lucene. It has been designed with the goal with driving the
existing lucene API, and keeping the abstraction to a minimum.

For more information on the associated projects

- https://github.com/cardinal252/Lucinq.Sitecore

Features
========

- A fluent style api for working with lucene queries for most of the query types and grouping.
- Lucene based paging
- Lucene based sorting
- Query by (including but not limited to) - Name (including wildcards), Direct Template, Base Template, Ancestor, Parent, Language or any combination
- Query manipulation - remove terms and re-run for example.

Why Use Lucinq For Sitecore
===========================

- In short, its faster. Tested on the same index (containing 90,000 entries from a sitecore db of 264,0000 items). Lucinq returned results on average 25% faster with disposed indexes and 45% faster with undisposed indexes than the equivalent sitecore searches. This is including Lucinq having the actual Sitecore item (unlike Sitecore's which is re-populating POCOs from the index)
- Quick and easy debugging - queryBuilder.Build().ToString() gives you the lucene query being executed whilst in debug.
- Easier integration testing - Lucinq does not require the Sitecore initialize pipelines to have been run - simply the location of the index and the connection strings for the required db's
- Can query by derived template (ie: get me all items inheriting from my SEO template).
- Glass Mapper integration to return fully populated POCO's straight from the index using the Sitecore item's

Coming Soon
===========

- Creating queries using the rules engine!

Support
=======

Currently Lucinq supports Sitecore 6.6 & 7.x. It could be made to support Sitecore 6.2 + with additional work.

NuGet
=====

Get it now from Nuget https://nuget.org/packages/Lucinq.SitecoreIntegration/

PM> Install-Package Lucinq.SitecoreIntegration

PM> Install-Package Lucinq.SitecoreIntegration.Glass


Getting Started
===============

Install the nuget package into your sitecore solution, if you are using sitecore 6.6 - you will need to configure the indexes, if Sitecore 7 you can use the default indexes (though it helps to uncomment the entry for _templates)

If you wish to get the unit tests running:

- Copy the binaries from sitecore to the /sitecore folder
- Compile the project
- Modify the constants in the unit tests to use your own sitecore installation
- Run the index rebuild unit test to build the index
- Examine and run the other unit tests at your leisure

Sitecore Configuration
======================

```
<index id="TestSearchIndex" type="Sitecore.Search.Index, Sitecore.Kernel">
	<param desc="name">$(id)</param>
	<param desc="folder">$(id)</param>
	<Analyzer ref="search/analyzer"/>
	<locations hint="list:AddCrawler">
		<master type="Lucinq.SitecoreIntegration.Indexing.StandardCrawler, Lucinq.SitecoreIntegration">
			<Database>master</Database>
			<Root>/sitecore/content/Sites/</Root>
			<IndexAllFields>true</IndexAllFields>
		</master>
		<web type="Lucinq.SitecoreIntegration.Indexing.StandardCrawler, Lucinq.SitecoreIntegration">
			<Database>web</Database>
			<Root>/sitecore/content/Sites/</Root>
			<IndexAllFields>true</IndexAllFields>
		</web>
	</locations>
</index>
```

Example Syntax
==============

Further examples can be found in the integration tests, however here is a quick overview of how the syntax looks
```C#
SitecoreSearch search = new SitecoreSearch(indexPath, new DataBaseHelper()));

IQueryBuilder query = new QueryBuilder();

query.Name("itemname");

SitecoreSearchResult results = search.Execute(query.Build());	

SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 9);
sitecoreItemResult.Items.ForEach(item => Console.WriteLine(item["My Property"]));
```

OR - By Template

```C#
SitecoreSearch search = new SitecoreSearch(indexPath, new DataBaseHelper()));

ID templateId = new ID("{8A255FA5-4198-4FAA-B56D-3DF6116F9342}");

IQueryBuilder query = new QueryBuilder();

query.Setup(
	x => x.TemplateId(templateId)
);

SitecoreSearchResult results = search.Execute(query.Build());

SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 9);
sitecoreItemResult.Items.ForEach(item => Console.WriteLine(item["My Property"]));
```

OR - By Language

```C#
SitecoreSearch search = new SitecoreSearch(indexPath, new DataBaseHelper()));

Language language = Language.Parse("de-de");

IQueryBuilder query = new QueryBuilder();

query.Setup(
	x => x.Language(language),
	x => x.Name("itemname")
);

SitecoreSearchResult results = search.Execute(query.Build());

SitecoreItemResult sitecoreItemResult = sitecoreSearchResult.GetPagedItems(0, 9);
sitecoreItemResult.Items.ForEach(item => Console.WriteLine(item["My Property"]));

```

License
=======
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.