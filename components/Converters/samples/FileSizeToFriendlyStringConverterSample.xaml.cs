// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ConvertersExperiment.Samples;

[ToolkitSample(id: nameof(FileSizeToFriendlyStringConverterSample), "FileSizeToFriendlyStringConverter", description: $"A sample for showing how to use the FileSizeToFriendlyStringConverter.")]
public sealed partial class FileSizeToFriendlyStringConverterSample : Page
{
    public long FileSizeInBytes = 2400000;

    public FileSizeToFriendlyStringConverterSample()
    {
        this.InitializeComponent();
    }
}
