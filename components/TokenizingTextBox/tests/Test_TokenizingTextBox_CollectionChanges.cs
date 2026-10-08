// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CommunityToolkit.Tests;
using CommunityToolkit.WinUI.Controls;

namespace TokenizingTextBoxTests;

[TestClass]
public class Test_TokenizingTextBox_CollectionChanges : VisualUITestBase
{
    [TestCategory("Test_TokenizingTextBox_CollectionChanges")]
    [TestMethod]
    public async Task Test_ItemsSourceRangeAdd()
    {
        await App.DispatcherQueue.EnqueueAsync(() =>
        {
            var source = new RangeObservableCollection<object> { 0, 3 };
            var tokenBox = new TokenizingTextBox { ItemsSource = source };

            source.InsertRange(1, new object[] { 1, 2 });

            Assert.AreEqual(4, source.Count);
            Assert.AreEqual(5, tokenBox.Items.Count);
            Assert.AreEqual(0, tokenBox.Items[0]);
            Assert.AreEqual(1, tokenBox.Items[1]);
            Assert.AreEqual(2, tokenBox.Items[2]);
            Assert.AreEqual(3, tokenBox.Items[3]);
        });
    }

    [TestCategory("Test_TokenizingTextBox_CollectionChanges")]
    [TestMethod]
    public async Task Test_ItemsSourceRangeRemove()
    {
        await App.DispatcherQueue.EnqueueAsync(() =>
        {
            var source = new RangeObservableCollection<object> { 0, 1, 2, 3 };
            var tokenBox = new TokenizingTextBox { ItemsSource = source };

            source.RemoveRange(1, 2);

            Assert.AreEqual(2, source.Count);
            Assert.AreEqual(3, tokenBox.Items.Count);
            Assert.AreEqual(0, tokenBox.Items[0]);
            Assert.AreEqual(3, tokenBox.Items[1]);
        });
    }

    private sealed class RangeObservableCollection<T> : ObservableCollection<T>
    {
        public void InsertRange(int index, IReadOnlyList<T> items)
        {
            if (items.Count == 0)
            {
                return;
            }

            CheckReentrancy();

            var insertedItems = new List<T>(items);
            for (var offset = 0; offset < insertedItems.Count; offset++)
            {
                Items.Insert(index + offset, insertedItems[offset]);
            }

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, insertedItems, index));
        }

        public void RemoveRange(int index, int count)
        {
            if (count == 0)
            {
                return;
            }

            CheckReentrancy();

            var removedItems = new List<T>(count);
            for (var offset = 0; offset < count; offset++)
            {
                removedItems.Add(Items[index]);
                Items.RemoveAt(index);
            }

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems, index));
        }
    }
}
