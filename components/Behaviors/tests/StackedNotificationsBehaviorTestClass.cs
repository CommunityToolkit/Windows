// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BehaviorsTests;
using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.WinUI.Behaviors;
using Microsoft.Xaml.Interactivity;

namespace StackedNotificationsBehaviorTests;

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
        infobar.Severity = MUXC.InfoBarSeverity.Error;

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

    [UIThreadTestMethod]
    public async Task NotificationBehaviorDurationTest(StackedNotificationsBehaviorsTestPage page)
    {
        var infobar = page.FindDescendant<MUXC.InfoBar>();
        var queue = Interaction.GetBehaviors(infobar)[0] as StackedNotificationsBehavior;

        Assert.IsNotNull(infobar, "Could not find infobar control.");
        Assert.IsNotNull(queue, "Could not find notification behavior.");

        Notification notification = new()
        {
            Message = "Test Message",
            Duration = TimeSpan.FromMilliseconds(1000),
        };

        queue.Show(notification);

        // Wait for UI update;
        await EnqueueAsync(() => { });

        Assert.IsTrue(infobar.IsOpen, "InfoBar didn't open.");
        Assert.AreEqual(string.Empty, infobar.Title, "Expected no Title value");
        Assert.AreEqual("Test Message", infobar.Message, "Message not equal to expected value");
        Assert.AreEqual(MUXC.InfoBarSeverity.Informational, infobar.Severity, "Severity not equal to default value");

        // Wait for little bit more than Duration
        await Task.Delay(1100);

        Assert.IsFalse(infobar.IsOpen, "InfoBar didn't close after.");
    }

    [UIThreadTestMethod]
    public async Task NotificationBehaviorQueueBehindTest(StackedNotificationsBehaviorsTestPage page)
    {
        var infobar = page.FindDescendant<MUXC.InfoBar>();
        var queue = Interaction.GetBehaviors(infobar)[0] as StackedNotificationsBehavior;

        Assert.IsNotNull(infobar, "Could not find infobar control.");
        Assert.IsNotNull(queue, "Could not find notification behavior.");

        Notification notification1 = new()
        {
            Message = "Test Message",
            Duration = TimeSpan.FromMilliseconds(1000),
        };
        Notification notification2 = new()
        {
            Message = "Test Message 2",
            Duration = TimeSpan.FromMilliseconds(1000),
        };

        queue.Show(notification1);
        // Queue next message behind first
        queue.Show(notification2);

        // Wait for UI update;
        await EnqueueAsync(() => { });

        Assert.IsTrue(infobar.IsOpen, "InfoBar didn't open.");
        Assert.AreEqual("Test Message", infobar.Message, "First Message not equal to expected value.");

        // Wait for little bit more than Duration
        await Task.Delay(1100);

        Assert.IsTrue(infobar.IsOpen, "InfoBar didn't remain open.");
        Assert.AreEqual("Test Message 2", infobar.Message, "Message didn't change to 2nd message value.");

        await Task.Delay(1000);

        Assert.IsFalse(infobar.IsOpen, "InfoBar didn't close after all messages displayed.");
    }


    [UIThreadTestMethod]
    public async Task NotificationBehaviorCancelCloseTest(StackedNotificationsBehaviorsTestPage page)
    {
        var infobar = page.FindDescendant<MUXC.InfoBar>();
        var queue = Interaction.GetBehaviors(infobar)[0] as StackedNotificationsBehavior;

        Assert.IsNotNull(infobar, "Could not find infobar control.");
        Assert.IsNotNull(queue, "Could not find notification behavior.");

        bool attemptedClose = false;
        infobar.Closing += (sender, args) =>
        {
            attemptedClose = true;
            args.Cancel = true;
        };

        Notification notification = new()
        {
            Message = "Test Message",
            Duration = TimeSpan.FromMilliseconds(500),
        };

        queue.Show(notification);

        // Wait for UI update;
        await EnqueueAsync(() => { });

        Assert.IsTrue(infobar.IsOpen, "InfoBar didn't open.");
        Assert.AreEqual("Test Message", infobar.Message, "Message not equal to expected value");

        await Task.Delay(600);

        Assert.IsTrue(attemptedClose, "InfoBar never attempted to close.");
        Assert.IsTrue(infobar.IsOpen, "InfoBar didn't remain open.");
    }
}
