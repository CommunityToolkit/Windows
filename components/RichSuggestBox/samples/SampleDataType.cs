// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace RichSuggestBoxExperiment.Samples;

/// </summary>
public class SampleDataType
{
    /// <summary>
    /// Gets or sets symbol to display.
    /// </summary>
    public Symbol Icon { get; set; }

    /// <summary>
    /// Gets or sets text to display.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Text { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public override string ToString()
    {
        return Text;
    }
}
