// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AnimationsExperiment.Samples.ConnectedAnimations;

public sealed partial class ThirdPage : Page
{
    private PhotoDataItem item;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ThirdPage()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {

#pragma warning disable CS8601 // Possible null reference assignment.
            item = e.Parameter as PhotoDataItem;
#pragma warning restore CS8601 // Possible null reference assignment.
        base.OnNavigatedTo(e);
    }
}
