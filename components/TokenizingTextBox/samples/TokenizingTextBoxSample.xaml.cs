// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace TokenizingTextBoxExperiment.Samples;

[ToolkitSample(id: nameof(TokenizingTextBoxSample), "Basic sample", description: $"A sample for showing how to create and use a {nameof(TokenizingTextBox)}.")]
public sealed partial class TokenizingTextBoxSample : Page
{
    public readonly List<SampleDataType> _samples = new List<SampleDataType>()
        {
            new SampleDataType() { Text = "Account", Icon = Symbol.Account },
            new SampleDataType() { Text = "Add friend", Icon = Symbol.AddFriend },
            new SampleDataType() { Text = "Attach", Icon = Symbol.Attach },
            new SampleDataType() { Text = "Attach camera", Icon = Symbol.AttachCamera },
            new SampleDataType() { Text = "Audio", Icon = Symbol.Audio },
            new SampleDataType() { Text = "Block contact", Icon = Symbol.BlockContact },
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
            new SampleDataType() { Text = "Rotate camera", Icon = Symbol.RotateCamera },
            new SampleDataType() { Text = "Send", Icon = Symbol.Send },
            new SampleDataType() { Text = "Tags", Icon = Symbol.Tag },
            new SampleDataType() { Text = "UnFavorite", Icon = Symbol.UnFavorite },
            new SampleDataType() { Text = "UnPin", Icon = Symbol.UnPin },
            new SampleDataType() { Text = "Zoom", Icon = Symbol.Zoom },
            new SampleDataType() { Text = "ZoomIn", Icon = Symbol.ZoomIn },
            new SampleDataType() { Text = "ZoomOut", Icon = Symbol.ZoomOut },
        };

    public ObservableCollection<SampleDataType> SelectedTokens { get; set; }

    public TokenizingTextBoxSample()
    {
        this.InitializeComponent();
        SelectedTokens = new()
        {
            _samples[0],
            _samples[1]
        };
    
    }

    private void TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        currentEdit.Text = TokenBox.Text;
        SetSelectedTokenText();
    }

    private void SetSelectedTokenText()
    {
        selectedItemsString.Text = TokenBox.SelectedTokenText;
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

    private void TokenBox_Loaded(object sender, RoutedEventArgs e)
    {
        SetSelectedTokenText();
    }
}
