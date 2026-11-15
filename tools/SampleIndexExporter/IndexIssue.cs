// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.SampleIndex;

/// <summary>How serious a finding is.</summary>
internal enum IssueSeverity
{
    /// <summary>The index is still publishable, but something upstream needs attention.</summary>
    Warning,

    /// <summary>The index cannot be published as-is.</summary>
    Error,
}

/// <summary>One finding about the sample data, tied to the file that caused it.</summary>
/// <param name="Severity">Whether this blocks publishing.</param>
/// <param name="File">Repository-relative path of the file at fault.</param>
/// <param name="Message">What is wrong, in terms the file's author would recognize.</param>
internal sealed record IndexIssue(IssueSeverity Severity, string File, string Message)
{
    public override string ToString() => $"{File}: {Message}";
}

/// <summary>A sample that was left out of the index, and why.</summary>
/// <param name="SampleId">The sample's <c>[ToolkitSample]</c> id.</param>
/// <param name="Reason">Why it could not be published.</param>
internal sealed record WithheldSample(string SampleId, string Reason)
{
    public override string ToString() => $"{SampleId}: {Reason}";
}

/// <summary>Everything one generation run produced.</summary>
/// <param name="Index">The index, ready to serialize.</param>
/// <param name="Issues">Findings, in file order.</param>
/// <param name="Withheld">Samples deliberately left out, in id order.</param>
internal sealed record GenerationResult(
    SampleIndex Index,
    IReadOnlyList<IndexIssue> Issues,
    IReadOnlyList<WithheldSample> Withheld)
{
    public IEnumerable<IndexIssue> Errors => Issues.Where(i => i.Severity == IssueSeverity.Error);

    public IEnumerable<IndexIssue> Warnings => Issues.Where(i => i.Severity == IssueSeverity.Warning);
}
