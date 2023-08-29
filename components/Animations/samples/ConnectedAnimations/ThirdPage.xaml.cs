// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AnimationsExperiment.Samples.ConnectedAnimations;

public sealed partial class ThirdPage : Page
{
    private PhotoDataItem item = new();

    public ThirdPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is PhotoDataItem photoItem)
        {
            item = photoItem;
        }

        base.OnNavigatedTo(e);
    }
}
