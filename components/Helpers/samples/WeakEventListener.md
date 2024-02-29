---
title: WeakEventListener
author: nmetulev
description: The WeakEventListener allows the owner to be garbage collected if its only remaining link is an event handler.
keywords: Helpers, WeakEventListener
dev_langs:
  - csharp
category: Helpers
subcategory: Miscellaneous
discussion-id: 0
issue-id: 0
icon: Assets/WeakEventListener.png
---

```csharp
var inpc = rowGroupInfo.CollectionViewGroup as INotifyPropertyChanged;
var weakPropertyChangedListener = new WeakEventListener<DataGrid, object, PropertyChangedEventArgs>(this)
{
    OnEventAction = static (instance, source, eventArgs) => instance.CollectionViewGroup_PropertyChanged(source, eventArgs),
    OnDetachAction = (weakEventListener) => inpc.PropertyChanged -= weakEventListener.OnEvent // Use Local References Only
}
inpc.PropertyChanged += weakPropertyChangedListener.OnEvent;
```
