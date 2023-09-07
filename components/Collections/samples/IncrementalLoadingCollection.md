---
title: IncrementalLoadingCollection
author: nmetulev
description: The IncrementalLoadingCollection helpers greatly simplify the definition and usage of collections whose items can be loaded incrementally only when needed by the view (such as a ScrollViewer).
keywords: IncrementalLoadingCollection, Control, Data, Incremental, Loading, Collections
dev_langs:
  - csharp
category: Helpers
subcategory: Data
discussion-id: 0
issue-id: 0
icon: Assets/IncrementalLoadingCollection.png
---

> [!Sample IncrementalLoadingCollectionSample]

`IIncrementalSource` - An interface that represents a data source whose items can be loaded incrementally.

`IncrementalLoadingCollection` - An extension of [ObservableCollection](https://learn.microsoft.com/dotnet/api/system.collections.objectmodel.observablecollection-1) such that its items are loaded only when needed.

## Example

`IIncrementalSource` allows to define the data source:

```csharp
// Be sure to include the using at the top of the file:
//using CommunityToolkit.WinUI.Collections;

public class Person
{
    public string Name { get; set; }
}

public class PeopleSource : IIncrementalSource<Person>
{
    private readonly List<Person> people;

    public PeopleSource()
    {
        // Creates an example collection.
        people = new List<Person>();

        for (int i = 1; i <= 200; i++)
        {
            var p = new Person { Name = "Person " + i };
            people.Add(p);
        }
    }

    public async Task<IEnumerable<Person>> GetPagedItemsAsync(int pageIndex, int pageSize)
    {
        // Gets items from the collection according to pageIndex and pageSize parameters.
        var result = (from p in people
                        select p).Skip(pageIndex * pageSize).Take(pageSize);

        // Simulates a longer request...
        await Task.Delay(1000);

        return result;
    }
}
```

The *GetPagedItemsAsync* method is invoked every time the view need to show more items.

`IncrementalLoadingCollection` can then be bound to a [ListView](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.ListView) or a [GridView-like](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.GridView) control:

```csharp
var collection = new IncrementalLoadingCollection<PeopleSource, Person>();
PeopleListView.ItemsSource = collection;
```
