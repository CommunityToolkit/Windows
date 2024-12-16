// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace PrimitivesExperiment.Tests;

public sealed partial class SwitchConverterBrushSample : Page
{
    public SwitchConverterBrushSample()
    {
        this.InitializeComponent();
    }
}

public enum CheckStatus
{
    Error,
    Warning,
    Success,
}