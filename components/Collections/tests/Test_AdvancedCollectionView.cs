// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Collections;

using System.Runtime.CompilerServices;

namespace CollectionsTests;

[TestClass]
public class Test_AdvancedCollectionView
{
    private class SampleClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int val;

        public virtual int Val
        {
            get
            {
                return val;
            }

            set
            {
                val = value;
                OnPropertyChanged();
            }
        }

        public int GetPropertyChangedEventHandlerSubscriberLength()
        {
            return PropertyChanged is null ? 0 : PropertyChanged.GetInvocationList().Length;
        }

        public SampleClass(int val)
        {
            this.Val = val;
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    private class DerivedClass : SampleClass
    {
        public DerivedClass(int val) : base(val)
        {
        }

        public override int Val
        {
            get
            {
                return 101;
            }

            set
            {
            }
        }
    }

    [TestCategory("Helpers")]
    [UITestMethod]
    public void Test_SourceNcc_CollectionChanged_Add()
    {
        // Create ref list with all test items:
        List<SampleClass> refList = new List<SampleClass>();
        for (int e = 0; e < 100; e++)
        {
            refList.Add(new SampleClass(e));
        }

        ObservableCollection<SampleClass> col = new ObservableCollection<SampleClass>();
        AdvancedCollectionView acv = new AdvancedCollectionView(col, true);

        Assert.AreEqual(0, col.Count);
        Assert.AreEqual(0, acv.Count);

        // Add all items to collection while DeferRefresh() is active:
        using (acv.DeferRefresh())
        {
            foreach (var item in refList)
            {
                col.Add(item);
            }
        }

        Assert.AreNotEqual(0, col.Count);
        Assert.AreNotEqual(0, acv.Count);

        Assert.AreEqual(refList.Count, col.Count);
        Assert.AreEqual(refList.Count, acv.Count);

        // Make sure each item added to source is in the expected place in the view:
        for (int i = 0; i < refList.Count; i++)
        {
            var sourceItem = col[i];
            var collectionViewItem = (SampleClass)acv[i];

            Assert.AreEqual(sourceItem.Val, collectionViewItem.Val);
        }

        // Check if subscribed to all items:
        foreach (var item in refList)
        {
            Assert.IsTrue(item.GetPropertyChangedEventHandlerSubscriberLength() == 1);
        }

        // Add a filter and item to filter
        var itemToFilter = new SampleClass(1000);
        Func<object, bool> filter = x => ((SampleClass)x).Val == itemToFilter.Val;
        acv.Filter = x => !filter(x);
        col.Add(itemToFilter);

        var filteredItemInAcv = (SampleClass?)acv.FirstOrDefault(filter);
        Assert.IsNull(filteredItemInAcv, $"Filtered item unexpectedly included in collection view: {filteredItemInAcv?.Val}.");

        col.Remove(itemToFilter);

        // With filter set, create and add an item that isn't filtered
        var itemToNotFilter = new SampleClass(itemToFilter.Val + 1);
        col.Add(itemToNotFilter);

        var addedItemInAcv = (SampleClass?)acv.FirstOrDefault(x => ((SampleClass)x).Val == itemToNotFilter.Val);

        Assert.IsNotNull(addedItemInAcv, $"Unfiltered item unexpectedly filtered out of collection view: {addedItemInAcv?.Val}.");

        // Ensure added (not filtered) item is added to the last position in view.
        var indexOfNotFilteredItemInAcv = acv.IndexOf(addedItemInAcv);
        Assert.AreEqual(acv.Count - 1, indexOfNotFilteredItemInAcv, "Unfiltered item added to source collection not last in view.");
    }

    [TestCategory("Helpers")]
    [UITestMethod]
    public void Test_SourceNcc_CollectionChanged_Remove()
    {
        // Create ref list with all test items:
        List<SampleClass> refList = new List<SampleClass>();
        for (int e = 0; e < 100; e++)
        {
            refList.Add(new SampleClass(e));
        }

        ObservableCollection<SampleClass> col = new ObservableCollection<SampleClass>();
        AdvancedCollectionView acv = new AdvancedCollectionView(col, true);

        // Add all items to collection:
        foreach (var item in refList)
        {
            col.Add(item);
        }

        // Remove all items from collection while DeferRefresh() is active:
        using (acv.DeferRefresh())
        {
            while (col.Count > 0)
            {
                col.RemoveAt(0);
            }
        }

        // Check if unsubscribed from all items:
        foreach (var item in refList)
        {
            Assert.IsTrue(item.GetPropertyChangedEventHandlerSubscriberLength() == 0);
        }
    }

    [TestCategory("Helpers")]
    [UITestMethod]
    public void Test_DerivedTypesInList()
    {
        // Create ref list with elements of different types:
        List<SampleClass> refList = new List<SampleClass>();
        for (int e = 0; e < 100; e++)
        {
            if ((e % 2) == 1)
            {
                refList.Add(new SampleClass(e));
            }
            else
            {
                refList.Add(new DerivedClass(e));
            }
        }
        ObservableCollection<SampleClass> col = new ObservableCollection<SampleClass>();

        // Add all items to collection:
        foreach (var item in refList)
        {
            col.Add(item);
        }

        // Sort elements using a property that is overriden in the derived class
        AdvancedCollectionView acv = new AdvancedCollectionView(col, true);
        acv.SortDescriptions.Add(new SortDescription("Val", SortDirection.Descending));
    }
}
