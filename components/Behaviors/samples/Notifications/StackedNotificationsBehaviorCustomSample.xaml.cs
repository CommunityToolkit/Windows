// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Behaviors;

namespace BehaviorsExperiment.Samples.Notifications;

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

    private static int _current = 0;

    private static string GetRandomText() => (_current++ % 4) switch
    {
        1 => "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec sollicitudin bibendum enim at tincidunt. Praesent egestas ipsum ligula, nec tincidunt lacus semper non.",
        2 => "Pellentesque in risus eget leo rhoncus ultricies nec id ante.",
        3 => "Sed quis nisi quis nunc condimentum varius id consectetur metus. Duis mauris sapien, commodo eget erat ac, efficitur iaculis magna. Morbi eu velit nec massa pharetra cursus.",
        _ => "Fusce non quam egestas leo finibus interdum eu ac massa. Quisque nec justo leo. Aenean scelerisque placerat ultrices. Sed accumsan lorem at arcu commodo tristique.",
    };
}
