// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace TokenizingTextBoxExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>
[ToolkitSampleTextOption("TitleText", "This is a title", Title = "Input the text")]
[ToolkitSampleMultiChoiceOption("LayoutOrientation", "Horizontal", "Vertical", Title = "Orientation")]

[ToolkitSample(id: nameof(TokenizingTextBoxCustomSample), "Custom control", description: $"A sample for showing how to create and use a {nameof(TokenizingTextBox)} custom control.")]
public sealed partial class TokenizingTextBoxCustomSample : Page
{
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

    public ObservableCollection<SampleDataType> SelectedTokens { get; set; }

    public TokenizingTextBoxCustomSample()
    {
        this.InitializeComponent();
        SelectedTokens = new();
        TokenBox.SuggestedItemsSource = _samples;

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
           // _acv.RefreshFilter();
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
}
