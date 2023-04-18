// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace SizersExperiment.Samples;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
[ToolkitSample(id: nameof(GridSplitterPage), "GridSplitter Example", description: "Splitter that redistributes space between columns or rows of a Grid Control")]
public sealed partial class GridSplitterPage : Page
{
    public GridSplitterPage()
    {
        this.InitializeComponent();
    }
}
