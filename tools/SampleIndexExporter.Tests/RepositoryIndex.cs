// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.SampleIndex.Tests;

/// <summary>
/// Generates the index once for the whole test run, against the real repository.
/// </summary>
/// <remarks>
/// The gates below all ask questions about the actual sample data rather than about fixtures,
/// because the failure they exist to catch is a change to the samples, not a change to this
/// code. Generating is fast enough to do once and share.
/// </remarks>
internal static class RepositoryIndex
{
    private static readonly Lazy<GenerationResult> LazyResult = new(() => IndexGenerator.Generate(Root));

    private static readonly Lazy<string> LazyRoot =
        new(() => IndexGenerator.FindRepoRoot(AppContext.BaseDirectory));

    public static string Root => LazyRoot.Value;

    public static GenerationResult Result => LazyResult.Value;

    public static SampleIndex Index => Result.Index;

    /// <summary>The committed index exactly as a consumer fetches it.</summary>
    /// <remarks>
    /// For the contract gates. A test that asks the in-memory object whether a field is correct
    /// is asking the code that produced it, and gets the answer that code was written to give;
    /// the artifact is the only place the serialized field names and values can be observed.
    /// </remarks>
    public static string CommittedJson =>
        File.ReadAllText(Path.Combine(Root, IndexGenerator.IndexRelativePath.Replace('/', Path.DirectorySeparatorChar)));

    public static IEnumerable<(IndexedControl Control, IndexedSample Sample)> Samples =>
        Index.Controls.SelectMany(control => control.Samples.Select(sample => (control, sample)));
}
