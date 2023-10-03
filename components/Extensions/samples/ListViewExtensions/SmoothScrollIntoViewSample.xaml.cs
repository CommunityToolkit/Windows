// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI;

#if WINAPPSDK
using ListView = Microsoft.UI.Xaml.Controls.ListView;
#else
using ListView = Windows.UI.Xaml.Controls.ListView;
#endif

namespace ExtensionsExperiment.Samples.ListViewExtensions;

[ToolkitSample(id: nameof(SmoothScrollIntoViewSample), "SmoothScrollIntoView Extension", description: "A sample for showing how to use the SmoothScrollIntoViewWithIndexAsync API.")]
public sealed partial class SmoothScrollIntoViewSample : Page
{
    public ObservableCollection<string> Items { get; set; } = new();

    public SmoothScrollIntoViewSample()
    {
        Items = GetOddEvenSource(201);

        this.InitializeComponent();
    }

    private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListView listView)
        {
            await listView.SmoothScrollIntoViewWithIndexAsync(listView.SelectedIndex, ScrollItemPlacement.Center, false, true);
        }
    }

    private ObservableCollection<string> GetOddEvenSource(int count)
    {
        ObservableCollection<string> oddEvenSource = new();

        for (int number = 0; number < count; number++)
        {
            var item = (number % 2) == 0 ? $"{number} - Even" : $"{number} - Odd";
            oddEvenSource.Add(item);
        }

        return oddEvenSource;
    }
}
