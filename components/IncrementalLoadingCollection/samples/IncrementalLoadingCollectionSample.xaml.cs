// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI;

namespace IncrementalLoadingCollectionExperiment.Samples;

[ToolkitSample(id: nameof(IncrementalLoadingCollectionSample), "Incremental Loading Collection", description: $"A sample for showing how to create and use a IncrementalLoadingCollection.")]
public sealed partial class IncrementalLoadingCollectionSample : Page
{
    public IncrementalLoadingCollectionSample()
    {
        this.InitializeComponent();
        Load();
    }
    private void Load()
    {
        // IncrementalLoadingCollection can be bound to a GridView or a ListView. In this case it is a ListView called PeopleListView.
        var collection = new IncrementalLoadingCollection<PeopleSource, Person>();
        PeopleListView.ItemsSource = collection;

        // Binds the collection to the page DataContext in order to use its IsLoading and HasMoreItems properties.
        DataContext = collection;
    }

    private async void RefreshCollection(object sender, RoutedEventArgs e)
    {
        var collection = (IncrementalLoadingCollection<PeopleSource, Person>)PeopleListView.ItemsSource;
        await collection.RefreshAsync();
    }
}
