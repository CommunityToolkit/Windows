// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ConvertersExperiment.Samples;

[ToolkitSample(id: nameof(CollectionVisibilityConverterSample), "CollectionVisibilityConverter", description: $"A sample for showing how to use the CollectionVisibilityConverter.")]
public sealed partial class CollectionVisibilityConverterSample : Page
{
    public ObservableCollection<string> EmptyCollection = new();

    public CollectionVisibilityConverterSample()
    {
        this.InitializeComponent();
    }
}
