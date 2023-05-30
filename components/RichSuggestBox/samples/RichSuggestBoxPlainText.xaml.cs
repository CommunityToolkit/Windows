// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

#if WINAPPSDK
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Text;
#else
using Windows.UI;
using Windows.System;
using Windows.UI.Text;
#endif

namespace RichSuggestBoxExperiment.Samples;

[ToolkitSample(id: nameof(RichSuggestBoxPlainTextSample), "RichSuggestBox with plain text", description: $"A sample for showing how to create and use a {nameof(RichSuggestBox)} with plain text.")]
public sealed partial class RichSuggestBoxPlainTextSample : Page
{
    private DispatcherQueue _dispatcherQueue;
    private readonly List<SampleEmailDataType> _emailSamples = new List<SampleEmailDataType>()
        {
            new SampleEmailDataType() { FirstName = "Marcus", FamilyName = "Perryman" },
            new SampleEmailDataType() { FirstName = "Michael", FamilyName = "Hawker" },
            new SampleEmailDataType() { FirstName = "Matt", FamilyName = "Lacey" },
            new SampleEmailDataType() { FirstName = "Alexandre", FamilyName = "Chohfi" },
            new SampleEmailDataType() { FirstName = "Filip", FamilyName = "Wallberg" },
            new SampleEmailDataType() { FirstName = "Shane", FamilyName = "Weaver" },
            new SampleEmailDataType() { FirstName = "Vincent", FamilyName = "Gromfeld" },
            new SampleEmailDataType() { FirstName = "Sergio", FamilyName = "Pedri" },
            new SampleEmailDataType() { FirstName = "Alex", FamilyName = "Wilber" },
            new SampleEmailDataType() { FirstName = "Allan", FamilyName = "Deyoung" },
            new SampleEmailDataType() { FirstName = "Adele", FamilyName = "Vance" },
            new SampleEmailDataType() { FirstName = "Grady", FamilyName = "Archie" },
            new SampleEmailDataType() { FirstName = "Megan", FamilyName = "Bowen" },
            new SampleEmailDataType() { FirstName = "Ben", FamilyName = "Walters" },
            new SampleEmailDataType() { FirstName = "Debra", FamilyName = "Berger" },
            new SampleEmailDataType() { FirstName = "Emily", FamilyName = "Braun" },
            new SampleEmailDataType() { FirstName = "Christine", FamilyName = "Cline" },
            new SampleEmailDataType() { FirstName = "Enrico", FamilyName = "Catteneo" },
            new SampleEmailDataType() { FirstName = "Davit", FamilyName = "Badalyan" },
            new SampleEmailDataType() { FirstName = "Diego", FamilyName = "Siciliani" },
            new SampleEmailDataType() { FirstName = "Raul", FamilyName = "Razo" },
            new SampleEmailDataType() { FirstName = "Miriam", FamilyName = "Graham" },
            new SampleEmailDataType() { FirstName = "Lynne", FamilyName = "Robbins" },
            new SampleEmailDataType() { FirstName = "Lydia", FamilyName = "Holloway" },
            new SampleEmailDataType() { FirstName = "Nestor", FamilyName = "Wilke" },
            new SampleEmailDataType() { FirstName = "Patti", FamilyName = "Fernandez" },
            new SampleEmailDataType() { FirstName = "Pradeep", FamilyName = "Gupta" },
            new SampleEmailDataType() { FirstName = "Joni", FamilyName = "Sherman" },
            new SampleEmailDataType() { FirstName = "Isaiah", FamilyName = "Langer" },
            new SampleEmailDataType() { FirstName = "Irvin", FamilyName = "Sayers" },
            new SampleEmailDataType() { FirstName = "Tung", FamilyName = "Huynh" },
        };

    public RichSuggestBoxPlainTextSample()
    {
        this.InitializeComponent();
        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        TokenListView.ItemsSource = SuggestingBox.Tokens;
    }

    private void SuggestingBox_SuggestionChosen(RichSuggestBox sender, SuggestionChosenEventArgs args)
    {
            args.DisplayText = ((SampleEmailDataType)args.SelectedItem!).DisplayName;
    }

    private void SuggestingBox_SuggestionRequested(RichSuggestBox sender, SuggestionRequestedEventArgs args)
    {
            sender.ItemsSource = this._emailSamples.Where(x => x.DisplayName.Contains(args.QueryText!, StringComparison.OrdinalIgnoreCase));
    }

    private void SuggestingBox_TokenPointerOver(RichSuggestBox sender, RichSuggestTokenPointerOverEventArgs args)
    {
        var flyout = (Flyout)FlyoutBase.GetAttachedFlyout(sender);
        var pointerPosition = args.CurrentPoint!.Position;

        if (flyout?.Content is ContentPresenter cp && sender.TextDocument!.Selection.Type != SelectionType.Normal &&
            (!flyout.IsOpen || cp.Content != args.Token!.Item))
        {
            this._dispatcherQueue.TryEnqueue(() =>
            {
                cp.Content = args.Token!.Item;
                flyout.ShowAt(sender, new FlyoutShowOptions
                {
                    Position = pointerPosition,
                    ExclusionRect = sender.GetRectFromRange(args.Range!),
                    ShowMode = FlyoutShowMode.TransientWithDismissOnPointerMoveAway,
                });
            });
        }
    }
}
