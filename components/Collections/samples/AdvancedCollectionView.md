---
title: AdvancedCollectionView
author: nmetulev
description: The AdvancedCollectionView is a collection view implementation that support filtering, sorting and incremental loading. It's meant to be used in a view or viewmodel.
keywords: AdvancedCollectionView, CollectionViewSource, data, sorting, filtering, Collections
dev_langs:
  - csharp
category: Helpers
subcategory: Data
discussion-id: 0
issue-id: 0
icon: Assets/AdvancedCollectionView.png
---

## Usage

In your view or viewmodel instead of having a public [IEnumerable](https://learn.microsoft.com/dotnet/core/api/system.collections.generic.ienumerable-1) of some sort to be bound to an eg. [Listview](https://learn.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.ListView), create a public AdvancedCollectionView and pass your list in the constructor to it. If you've done that you can use the many useful features it provides:

* sorting your list using the `SortDirection` helper: specify any number of property names to sort on with the direction desired
* filtering your list using a [Predicate](https://learn.microsoft.com/dotnet/core/api/system.predicate-1): this will automatically filter your list only to the items that pass the check by the predicate provided
* deferring notifications using the `NotificationDeferrer` helper: with a convenient _using_ pattern you can increase performance while doing large-scale modifications in your list by waiting with updates until you've completed your work
* incremental loading: if your source collection supports the feature then AdvancedCollectionView will do as well (it simply forwards the calls)
* live shaping: when constructing the `AdvancedCollectionView` you may specify that the collection use live shaping. This means that the collection will re-filter or re-sort if there are changes to the sort properties or filter properties that are specified using `ObserveFilterProperty`

The `AdvancedCollectionView` is a good replacement for WPF's `CollectionViewSource`.

## Example

The following is a complete example of how to perform ...

> [!Sample AdvancedCollectionViewSample]

## Remarks

_What source can I use?_

It's not necessary to use an eg. [ObservableCollection](https://learn.microsoft.com/dotnet/core/api/system.collections.objectmodel.observablecollection-1) to use the AdvancedCollectionView. It works as expected even when providing a simple [List](https://learn.microsoft.com/dotnet/core/api/system.collections.generic.list-1) in the constructor.

_Any performance guidelines?_

If you're removing, modifying or inserting large amounts of items while having filtering and/or sorting set up, it's recommended that you use the `NotificationDeferrer` helper provided. It skips any performance heavy logic while it's in use, and automatically calls the `Refresh` method when disposed.

```csharp
using (acv.DeferRefresh())
{
    for (var i = 0; i < 500; i++)
    {
        acv.Add(new Person { Name = "defer" });
    }
} // acv.Refresh() gets called here
```
