// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AnimationsExperiment.Samples.ConnectedAnimations;

public sealed partial class SecondPage : Page
{
    private static ObservableCollection<PhotoDataItem>? items;

    public SecondPage()
    {
        this.InitializeComponent();
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        Frame.Navigate(typeof(ThirdPage), e.ClickedItem, new SuppressNavigationTransitionInfo());
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (items == null)
        {
            items = new ObservableCollection<PhotoDataItem>
            {
                new PhotoDataItem
                {
                    Title = "Big Four Summer Heat",
                    Thumbnail = "ms-appx:///Assets/BigFourSummerHeat2.jpg",
                },
                new PhotoDataItem
                {
                    Title = "Bison Badlands Chillin",
                    Thumbnail = "ms-appx:///Assets/BisonBadlandsChillin2.jpg",
                },
                new PhotoDataItem
                {
                    Title = "Columbia River Gorge",
                    Thumbnail = "ms-appx:///Assets/ColumbiaRiverGorge.jpg",
                },
                new PhotoDataItem
                {
                    Title = "Grand Tetons",
                    Thumbnail = "ms-appx:///Assets/GrandTetons.jpg",
                },
                new PhotoDataItem
                {
                    Title = "Oregon Winery Namaste",
                    Thumbnail = "ms-appx:///Assets/OregonWineryNamaste.jpg",
                },
                new PhotoDataItem
                {
                    Title = "Running Dog Pacific City",
                    Thumbnail = "ms-appx:///Assets/RunningDogPacificCity.jpg",
                }
            };
        }

        listView.ItemsSource = items;
    }
}

public class PhotoDataItem
{
    public string? Title { get; set; }

    public string? Thumbnail { get; set; }

    public override string ToString()
    {
        return Title!;
    }
}
