// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

#if WINAPPSDK
using Microsoft.UI;
using Microsoft.UI.Text;
#else
using Windows.UI;
using Windows.UI.Text;
#endif

namespace RichSuggestBoxExperiment.Samples;

[ToolkitSample(id: nameof(RichSuggestBoxMultiplePrefixesSample), "RichSuggestBox with multiple prefixes", description: $"A sample for showing how to create and use a {nameof(RichSuggestBox)} with multiple prefixes.")]
public sealed partial class RichSuggestBoxMultiplePrefixesSample : Page
{
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

    private readonly List<SampleDataType> _samples = new List<SampleDataType>()
        {
            new SampleDataType() { Text = "Account", Icon = Symbol.Account },
            new SampleDataType() { Text = "Add Friend", Icon = Symbol.AddFriend },
            new SampleDataType() { Text = "Attach", Icon = Symbol.Attach },
            new SampleDataType() { Text = "Attach Camera", Icon = Symbol.AttachCamera },
            new SampleDataType() { Text = "Audio", Icon = Symbol.Audio },
            new SampleDataType() { Text = "Block Contact", Icon = Symbol.BlockContact },
            new SampleDataType() { Text = "Calculator", Icon = Symbol.Calculator },
            new SampleDataType() { Text = "Calendar", Icon = Symbol.Calendar },
            new SampleDataType() { Text = "Camera", Icon = Symbol.Camera },
            new SampleDataType() { Text = "Contact", Icon = Symbol.Contact },
            new SampleDataType() { Text = "Favorite", Icon = Symbol.Favorite },
            new SampleDataType() { Text = "Link", Icon = Symbol.Link },
            new SampleDataType() { Text = "Mail", Icon = Symbol.Mail },
            new SampleDataType() { Text = "Map", Icon = Symbol.Map },
            new SampleDataType() { Text = "Phone", Icon = Symbol.Phone },
            new SampleDataType() { Text = "Pin", Icon = Symbol.Pin },
            new SampleDataType() { Text = "Rotate", Icon = Symbol.Rotate },
            new SampleDataType() { Text = "Rotate Camera", Icon = Symbol.RotateCamera },
            new SampleDataType() { Text = "Send", Icon = Symbol.Send },
            new SampleDataType() { Text = "Tags", Icon = Symbol.Tag },
            new SampleDataType() { Text = "UnFavorite", Icon = Symbol.UnFavorite },
            new SampleDataType() { Text = "UnPin", Icon = Symbol.UnPin },
            new SampleDataType() { Text = "Zoom", Icon = Symbol.Zoom },
            new SampleDataType() { Text = "ZoomIn", Icon = Symbol.ZoomIn },
            new SampleDataType() { Text = "ZoomOut", Icon = Symbol.ZoomOut },
        };
    public RichSuggestBoxMultiplePrefixesSample()
    {
        this.InitializeComponent();
        TokenListView.ItemsSource = SuggestingBox.Tokens;
    }

    private void SuggestingBox_SuggestionChosen(RichSuggestBox sender, SuggestionChosenEventArgs args)
    {
        if (args.Prefix == "#")
        {
            args.Format!.BackgroundColor = Colors.LightSlateGray;
            args.Format.ForegroundColor = Colors.White;
            args.Format.Bold = FormatEffect.On;
            args.DisplayText = ((SampleDataType)args.SelectedItem!).Text;
        }
        else
        {
            args.DisplayText = ((SampleEmailDataType)args.SelectedItem!).DisplayName;
        }
    }

    private void SuggestingBox_SuggestionRequested(RichSuggestBox sender, SuggestionRequestedEventArgs args)
    {
        if (args.Prefix == "#")
        {
            sender.ItemsSource = this._samples.Where(x => x.Text.Contains(args.QueryText!, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            sender.ItemsSource = this._emailSamples.Where(x => x.DisplayName.Contains(args.QueryText!, StringComparison.OrdinalIgnoreCase));
        }
    }
}
