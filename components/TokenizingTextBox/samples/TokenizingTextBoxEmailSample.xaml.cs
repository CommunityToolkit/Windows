// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace TokenizingTextBoxExperiment.Samples;

[ToolkitSample(id: nameof(TokenizingTextBoxEmailSample), "Email sample", description: $"A sample for showing how to create and use a {nameof(TokenizingTextBox)}.")]
public sealed partial class TokenizingTextBoxEmailSample : Page
{
    private readonly List<SampleEmailDataType> _Samples = new List<SampleEmailDataType>()
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
        };

    public ObservableCollection<SampleDataType> SelectedTokens { get; set; }

    public TokenizingTextBoxEmailSample()
    {
        this.InitializeComponent();
        SelectedTokens = new();
    }
    private void TokenItemAdded(TokenizingTextBox sender, object data)
    {
        // TODO: Add InApp Notification?
        if (data is SampleDataType sample)
        {
            Debug.WriteLine("Added Token: " + sample.Text);
        }
        else
        {
            Debug.WriteLine("Added Token: " + data);
        }
    }

    private void TokenItemRemoved(TokenizingTextBox sender, TokenItemRemovingEventArgs args)
    {
        if (args.Item is SampleDataType sample)
        {
            Debug.WriteLine("Removed Token: " + sample.Text);
        }
        else
        {
            Debug.WriteLine("Removed Token: " + args.Item);
        }
    }

    private void TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.CheckCurrent() && args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
           // TO DO: Filter items
        }
    }

    private void TokenItemCreating(object sender, TokenItemAddingEventArgs e)
    {
        // Take the user's text and convert it to our data type (if we have a matching one).
#if !HAS_UNO
        e.Item = _samples.FirstOrDefault((item) => item.Text!.Contains(e.TokenText, StringComparison.CurrentCultureIgnoreCase));
#else
        e.Item = _samples.FirstOrDefault((item) => item.Text!.Contains(e.TokenText));
#endif
        // Otherwise, create a new version of our data type
        if (e.Item == null)
        {
            e.Item = new SampleDataType()
            {
                Text = e.TokenText,
                Icon = Symbol.OutlineStar
            };
        }
    }

    private void TokenBox_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is SampleDataType selectedItem)
        {
            clickedItem.Text = selectedItem.Text!;
        }
    }
}
