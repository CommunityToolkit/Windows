// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.SampleIndex;

/// <summary>
/// Regenerates or verifies the committed sample index.
/// </summary>
/// <remarks>
/// <code>
/// dotnet run --project tools/SampleIndexExporter -- generate [--repo-root &lt;path&gt;]
/// dotnet run --project tools/SampleIndexExporter -- check    [--repo-root &lt;path&gt;]
/// </code>
///
/// <para><c>generate</c> writes the index. <c>check</c> rebuilds it in memory and fails when
/// the committed file is missing or out of date, without touching anything on disk. CI runs
/// <c>check</c>, which is what stops the published index drifting away from the samples it
/// describes.</para>
/// </remarks>
internal static class Program
{
    internal static int Main(string[] args)
    {
        if (args.Length == 0 || (args[0] != "generate" && args[0] != "check"))
        {
            Console.Error.WriteLine(
                "Usage: dotnet run --project tools/SampleIndexExporter -- <generate|check> [--repo-root <path>]");
            return 2;
        }

        var command = args[0];
        string repoRoot;

        try
        {
            repoRoot = ParseRepoRoot(args) ?? IndexGenerator.FindRepoRoot(Directory.GetCurrentDirectory());
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 2;
        }

        var result = IndexGenerator.Generate(repoRoot);

        foreach (var warning in result.Warnings)
        {
            Console.WriteLine($"warning: {warning}");
        }

        foreach (var withheld in result.Withheld)
        {
            Console.WriteLine($"withheld: {withheld}");
        }

        var errors = result.Errors.ToList();
        if (errors.Count > 0)
        {
            foreach (var error in errors)
            {
                Console.Error.WriteLine($"error: {error}");
            }

            Console.Error.WriteLine(
                $"{errors.Count} error(s) in the sample sources. The index was not written.");
            return 1;
        }

        var content = IndexGenerator.Serialize(result.Index);
        var absolutePath = Path.Combine(
            repoRoot,
            IndexGenerator.IndexRelativePath.Replace('/', Path.DirectorySeparatorChar));

        if (command == "generate")
        {
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);
            File.WriteAllText(absolutePath, content);

            Console.WriteLine(
                $"Wrote {result.Index.ControlCount} entries and {result.Index.SampleCount} samples to "
                + IndexGenerator.IndexRelativePath + ".");
            return 0;
        }

        if (!File.Exists(absolutePath))
        {
            Console.Error.WriteLine(
                $"{IndexGenerator.IndexRelativePath} does not exist. Run "
                + "'dotnet run --project tools/SampleIndexExporter -- generate' and commit the result.");
            return 1;
        }

        if (File.ReadAllText(absolutePath).Replace("\r\n", "\n") != content)
        {
            Console.Error.WriteLine(
                $"{IndexGenerator.IndexRelativePath} is out of date. Run "
                + "'dotnet run --project tools/SampleIndexExporter -- generate' and commit the result.");
            return 1;
        }

        Console.WriteLine(
            $"{IndexGenerator.IndexRelativePath} is up to date "
            + $"({result.Index.ControlCount} entries, {result.Index.SampleCount} samples).");
        return 0;
    }

    private static string? ParseRepoRoot(string[] args)
    {
        for (var i = 1; i < args.Length - 1; i++)
        {
            if (args[i] == "--repo-root")
            {
                return Path.GetFullPath(args[i + 1]);
            }
        }

        return null;
    }
}
