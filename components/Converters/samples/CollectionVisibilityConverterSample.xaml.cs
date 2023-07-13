// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ConvertersExperiment.Samples;

[ToolkitSample(id: nameof(CollectionVisibilityConverterSample), "CollectionVisibilityConverter", description: $"A sample for showing how to use the CollectionVisibilityConverter.")]
public sealed partial class CollectionVisibilityConverterSample : Page
{
    public ObservableCollection<string> Items { get; set; }

    public CollectionVisibilityConverterSample()
    {
        this.InitializeComponent();
        Items = new();


    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        Items.Clear();
        Items.Add("Item 1");
        Items.Add("Item 2");
        Items.Add("Item 3");
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        Items.Clear();
    }
}
