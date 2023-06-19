---
title: WeakEventListener
author: nmetulev
description: The WeakEventListener allows the owner to be garbage collected if its only remaining link is an event handler.
keywords: Helpers, WeakEventListener
dev_langs:
  - csharp
category: Helpers
subcategory: Layout
discussion-id: 0
issue-id: 0
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


## Properties

| Property | Type | Description |
| -- | -- | -- |
| OnDetachAction | WeakEventListener<TInstance,TSource,TEventArgs>> | Gets or sets the method to call when detaching from the event |
| OnEventAction | Action<TInstance,TSource,TEventArgs> | Gets or sets the method to call when the event fires |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Detach() | void | Detaches from the subscribed event |
| OnEvent(TSource, TEventArgs) | void | Handler for the subscribed event calls OnEventAction to handle it |
