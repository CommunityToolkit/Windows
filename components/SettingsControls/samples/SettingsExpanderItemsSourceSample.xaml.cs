// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.System;

namespace SettingsControlsExperiment.Samples;

[ToolkitSample(id: nameof(SettingsExpanderItemsSourceSample), "SettingsExpanderItemsSource", description: "The SettingsExpander can also be filled with items based on a collection.")]
public sealed partial class SettingsExpanderItemsSourceSample : Page
{

    public ObservableCollection<MyDataModel> MyDataSet = new() {
        new()
        {
            Name = "First Item",
            Info = "More about first item.",
            ItemType = "Item type: Button",
            LinkDescription = "Click here for more on first item.",
            Url = "https://microsoft.com/",
        },
        new()
        {
            Name = "Second Item",
            Info = "More about second item.",
            ItemType = "Item type: Link button",
            LinkDescription = "Click here for more on second item.",
            Url = "https://xbox.com/",
        },
        new()
        {
            Name = "Third Item",
            Info = "More about third item.",
            ItemType = "Item type: No button",
            LinkDescription = "Click here for more on third item.",
            Url = "https://toolkitlabs.dev/",
        },
    };

    public SettingsExpanderItemsSourceSample()
    {
        this.InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        _ = await Launcher.LaunchUriAsync(new("https://microsoft.com/"));
    }
}

public class MyDataModel
{
    public string? Name { get; set; }

    public string? Info { get; set; }

    public string? ItemType { get; set; }

    public string? LinkDescription { get; set; }

    public string? Url { get; set; }
}

public class MyDataModelTemplateSelector : DataTemplateSelector
{
    public DataTemplate? ButtonTemplate { get; set; }
    public DataTemplate? LinkButtonTemplate { get; set; }
    public DataTemplate? NoButtonTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        var itm = (MyDataModel)item;
        if (itm.ItemType?.EndsWith("Button") == true)
        {
            return ButtonTemplate!;
        }
        else if (itm.ItemType?.EndsWith("Link button") == true)
        {
            return LinkButtonTemplate!;
        }
        else
        {
            return NoButtonTemplate!;
        }
    }
}
