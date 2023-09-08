// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using System.Windows.Input;

namespace MetadataControlExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>
[ToolkitSampleTextOption("Separator", " • ", Title = "Separator")]
[ToolkitSampleTextOption("AccessibleSeparator", ", ", Title = "AccessibleSeparator")]

[ToolkitSample(id: nameof(MetadataControlSample), "MetadataControl", description: $"A sample for showing how to create and use a {nameof(MetadataControl)} control.")]
public sealed partial class MetadataControlSample : Page
{
    private static readonly string[] Labels = "Lorem ipsum dolor sit amet consectetur adipiscing elit".Split(' ');

    private readonly Random _random;
    private readonly ObservableCollection<MetadataItem> _units;
    private readonly DelegateCommand<object> _command;

    public MetadataControlSample()
    {
        this.InitializeComponent();
        _random = new Random();
        _units = new ObservableCollection<MetadataItem>
        {
            new MetadataItem { Label = GetRandomLabel() },
            new MetadataItem { Label = GetRandomLabel() }
        };
        _command = new DelegateCommand<object>(OnExecuteCommand);
        metadataControl.Items = _units;
    }

    private string GetRandomLabel() => Labels[_random!.Next(Labels.Length)];

    private void OnExecuteCommand(object obj)
    {
        OutputTxt.Text = $"Command invoked - parameter: {obj}";
    }

    private void AddLabel_Click(object sender, RoutedEventArgs e)
    {
        if (_units != null)
        {
            _units.Add(new MetadataItem { Label = GetRandomLabel() });
        }
    }

    private void AddCommand_Click(object sender, RoutedEventArgs e)
    {
        if (_units != null)
        {
            var label = GetRandomLabel();
            _units.Add(new MetadataItem
            {
                Label = label,
                Command = _command!,
                CommandParameter = label,
            });
        }
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        if (_units != null)
        {
            OutputTxt.Text = "";
            _units.Clear();
        }
    }
}
