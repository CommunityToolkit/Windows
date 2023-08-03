// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Behaviors;

namespace BehaviorsExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>

[ToolkitSample(id: nameof(StackedNotificationsBehaviorCustomSample), "Stacked Notifications", description: $"A sample for showing how to create and use a {nameof(StackedNotificationsBehavior)} custom behavior.")]
public sealed partial class StackedNotificationsBehaviorCustomSample : Page
{
    public StackedNotificationsBehaviorCustomSample()
    {
        this.InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var notification = new Notification
        {
            Title = $"Notification {DateTimeOffset.Now}",
            Message = GetRandomText(),
            Severity = MUXC.InfoBarSeverity.Informational,
        };

        NotificationQueue.Show(notification);
    }

    private static string GetRandomText()
    {
        var random = new Random();
        var result = random.Next(1, 4);

        switch (result)
        {
            case 1: return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec sollicitudin bibendum enim at tincidunt. Praesent egestas ipsum ligula, nec tincidunt lacus semper non.";
            case 2: return "Pellentesque in risus eget leo rhoncus ultricies nec id ante.";
            case 3: default: return "Sed quis nisi quis nunc condimentum varius id consectetur metus. Duis mauris sapien, commodo eget erat ac, efficitur iaculis magna. Morbi eu velit nec massa pharetra cursus. Fusce non quam egestas leo finibus interdum eu ac massa. Quisque nec justo leo. Aenean scelerisque placerat ultrices. Sed accumsan lorem at arcu commodo tristique.";
        }
    }
}
