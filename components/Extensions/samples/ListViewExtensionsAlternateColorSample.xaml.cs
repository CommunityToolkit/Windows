// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ExtensionsExperiment.Samples;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
[ToolkitSample(id: nameof(ListViewExtensionsAlternateColorSample), nameof(ListViewExtensionsAlternateColorSample), description: $"A sample for showing how to use the ListViewExtensions.AlternateColor attached property.")]
public sealed partial class ListViewExtensionsAlternateColorSample : Page
{
    public ListViewExtensionsAlternateColorSample()
    {
        this.InitializeComponent();
    }

    public static string NaiveHumanize(int num)
    {
        return num switch
        {
            0 => "zero",
            1 => "one",
            2 => "two",
            3 => "three",
            4 => "four",
            5 => "five",
            6 => "six",
            7 => "seven",
            8 => "eight",
            9 => "nine",
            10 => "ten",
            _ => num.ToString(),
        };
    }
}
