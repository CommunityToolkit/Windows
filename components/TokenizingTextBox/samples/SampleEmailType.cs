// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenizingTextBoxExperiment.Samples;

/// <summary>
/// Sample of strongly-typed email address simulated data for <see cref="CommunityToolkit.WinUI.Controls.TokenizingTextBox"/>.
/// </summary>
public class SampleEmailDataType
{
    /// <summary>
    /// Gets the initials to Display
    /// </summary>
    public string Initials => string.Empty + FirstName[0] + FamilyName[0];

    /// <summary>
    /// Gets or sets the first name .
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the family name .
    /// </summary>
    public string FamilyName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Gets the display text.
    /// </summary>
    public string DisplayName => $"{FirstName} {FamilyName}";

    /// <summary>
    /// Gets the formatted email address
    /// </summary>
    public string EmailAddress => $"{DisplayName} <{FirstName}.{FamilyName}@contoso.com>";

    public override string ToString()
    {
        return EmailAddress;
    }
}
