// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BehaviorsExperiment.Tests;
using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.WinUI.Behaviors;
using Microsoft.Xaml.Interactivity;

namespace StackedNotificationsBehaviorExperiment.Tests;

[TestClass]
public partial class StackedNotificationsBehaviorTestClass : VisualUITestBase
{
    [UIThreadTestMethod]
    public async Task NotificationBehaviorSingleTest(StackedNotificationsBehaviorsTestPage page)
    {
        var infobar = page.FindDescendant<MUXC.InfoBar>();
        var queue = Interaction.GetBehaviors(infobar)[0] as StackedNotificationsBehavior;

        Assert.IsNotNull(infobar, "Could not find infobar control.");
        Assert.IsNotNull(queue, "Could not find notification behavior.");

        Notification notification = new()
        {
            Title = "Test Title",
            Message = "Test Message",
            Severity = MUXC.InfoBarSeverity.Error,
        };

        queue.Show(notification);

        // Wait for UI update;
        await EnqueueAsync(() => { });

        Assert.IsTrue(infobar.IsOpen, "InfoBar didn't open.");
        Assert.AreEqual("Test Title", infobar.Title, "Title not equal to expected value");
        Assert.AreEqual("Test Message", infobar.Message, "Message not equal to expected value");
        Assert.AreEqual(MUXC.InfoBarSeverity.Error, infobar.Severity, "Severity not equal to expected value");
    }

    [UIThreadTestMethod]
    public async Task NotificationBehaviorOverrideTest(StackedNotificationsBehaviorsTestPage page)
    {
        var infobar = page.FindDescendant<MUXC.InfoBar>();
        var queue = Interaction.GetBehaviors(infobar)[0] as StackedNotificationsBehavior;

        Assert.IsNotNull(infobar, "Could not find infobar control.");
        Assert.IsNotNull(queue, "Could not find notification behavior.");

        infobar.Title = "Prior Title";
        infobar.Content = "Prior Content";
        infobar.Severity = InfoBarSeverity.Error;

        Notification notification = new()
        {
            Title = "Test Title",
            Content = "Some Content",
        };

        queue.Show(notification);

        // Wait for UI update;
        await EnqueueAsync(() => { });

        Assert.IsTrue(infobar.IsOpen, "InfoBar didn't open.");
        Assert.AreEqual("Test Title", infobar.Title, "Title not equal to expected value");
        Assert.AreEqual("Some Content", infobar.Content, "Content not equal to expected value");
        Assert.AreEqual(MUXC.InfoBarSeverity.Error, infobar.Severity, "Severity not equal to set parent value");

        infobar.IsOpen = false;

        // Wait for UI update;
        await EnqueueAsync(() => { });

        Assert.AreEqual("Prior Title", infobar.Title, "InfoBar title not reset");
        Assert.AreEqual("Prior Content", infobar.Content, "InfoBar content not reset");
    }
}
