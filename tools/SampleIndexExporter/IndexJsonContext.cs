// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace CommunityToolkit.SampleIndex;

/// <summary>
/// Source-generated serialization for the index. The project disables reflection-based
/// serialization, so a type reached by the serializer that is not listed here fails loudly at
/// runtime rather than being written with default conventions.
/// </summary>
[JsonSourceGenerationOptions(
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(SampleIndex))]
internal sealed partial class IndexJsonContext : JsonSerializerContext
{
    /// <summary>
    /// Serialize the index exactly as it is committed.
    /// </summary>
    /// <remarks>
    /// The encoder is widened past the default so that the characters that actually occur in
    /// this corpus — Segoe icon glyphs, typographic quotes, non-breaking spaces — are written
    /// as themselves instead of as <c>\uXXXX</c> escapes. The file is read by people as well as
    /// by tools, and the default encoder would render a page of XAML unreadable to both.
    ///
    /// <para>Line endings are forced to <c>\n</c>. The indenting writer uses the running
    /// machine's newline, so leaving it alone would mean a Windows contributor and a Linux CI
    /// agent generate byte-different files from identical sources, and the staleness check
    /// would fail on one of them for no reason a reader could act on. Output ends with a single
    /// newline so the file is well-formed for text tooling.</para>
    /// </remarks>
    public static string Serialize(SampleIndex index)
    {
        var options = new JsonSerializerOptions(Default.Options)
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            NewLine = "\n",
        };

        return JsonSerializer.Serialize(index, typeof(SampleIndex), new IndexJsonContext(options)) + "\n";
    }
}
