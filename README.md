Lucinq.Sitecore - A Fluent / Expressive API for Lucene.Net With Sitecore
=================================================

This project is based upon Lucene.Net and aims to give fluent style API to Lucene.Net used from the standard Sitecore indexes. The primary purpose of which is to make Lucene a little less verbose 
to accomplish most tasks whilst retaining the power and speed of Lucene. It has been designed with the goal with driving the
existing lucene API, and keeping the abstraction to a minimum.

Features
========

- A fluent style api for working with lucene queries for most of the query types and grouping.
- Paging
- Query manipulation - remove terms and re-run for example.


Example Syntax
==============

Further examples can be found in the integration tests, however here is a quick overview of how the syntax looks
```C#
SitecoreSearch search = new SitecoreSearch(indexPath, new DataBaseHelper()));

IQueryBuilder query = new QueryBuilder();

query.Name("itemname");

SitecoreSearchResult results = search.Execute(query.Build(), 20);	
foreach (Item item in result.GetPagedItems(0, 10))
{
	Console.WriteLine(item["My Property"]);
}
```

OR

```C#
SitecoreSearch search = new SitecoreSearch(indexPath, new DataBaseHelper()));

ID templateId = new ID("{8A255FA5-4198-4FAA-B56D-3DF6116F9342}");

IQueryBuilder query = new QueryBuilder();

query.Setup(
	x => x.TemplateId(templateId)
);

SitecoreSearchResult results = search.Execute(query.Build(), 20);	
foreach (Item item in result.GetPagedItems(0, 10))
{
	Console.WriteLine(item["My Property"]);
}
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