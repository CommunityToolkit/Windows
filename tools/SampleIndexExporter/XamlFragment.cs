// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace CommunityToolkit.SampleIndex;

/// <summary>What extracting one sample's XAML produced.</summary>
/// <param name="Xaml">The pasteable fragment, or null when the sample was withheld.</param>
/// <param name="XmlnsImports">Namespace declarations the fragment actually uses.</param>
/// <param name="OptionsResolved">Option bindings replaced with their default, as Name=Value.</param>
/// <param name="OptionBindingsDropped">Attributes removed because their binding had no static value.</param>
/// <param name="Error">Why the sample was withheld, when it was.</param>
internal sealed record XamlExtraction(
    string? Xaml,
    List<string> XmlnsImports,
    List<string> OptionsResolved,
    List<string> OptionBindingsDropped,
    string? Error);

/// <summary>
/// Turns a sample's <c>.xaml</c> file into something a reader can paste into their own app.
/// </summary>
/// <remarks>
/// A sample file is a page in the sample app: it has an <c>x:Class</c> naming a type in that
/// app, a <c>&lt;Page&gt;</c> root, and bindings to members the sample generator adds to that
/// page. None of those exist in the reader's project, so publishing the file verbatim would
/// publish something that cannot compile. What is published instead is the body of the root
/// element, with the sample-app bindings settled.
///
/// <para>What changes here is environment, never substance. The markup still demonstrates
/// exactly what its author wrote it to demonstrate.</para>
/// </remarks>
internal static partial class XamlFragment
{
    /// <summary>The XAML language namespace, declared by every XAML file as <c>x</c>.</summary>
    private const string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";

    /// <summary>The default presentation namespace, declared by every XAML file with no prefix.</summary>
    private const string PresentationNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

    /// <summary>Design-time namespaces, which mean nothing outside a designer.</summary>
    private static readonly HashSet<string> DesignTimeNamespaces =
    [
        "http://schemas.microsoft.com/expression/blend/2008",
        "http://schemas.openxmlformats.org/markup-compatibility/2006",
    ];

    /// <summary>A markup extension's prefix, e.g. the <c>ui</c> of <c>{ui:FontIcon ...}</c>.</summary>
    [GeneratedRegex(@"\{\s*(?<prefix>[A-Za-z_][\w.-]*):")]
    private static partial Regex MarkupExtensionPrefixRegex();

    /// <summary>Two or more consecutive blank lines, left behind when an attribute is removed.</summary>
    [GeneratedRegex(@"\n[ \t]*\n[ \t]*\n")]
    private static partial Regex BlankLineRunRegex();

    /// <summary>
    /// Extract the pasteable fragment from a sample file.
    /// </summary>
    /// <param name="xamlText">The verbatim contents of the sample's <c>.xaml</c> file.</param>
    /// <param name="options">Options declared on the sample, whose generated members its markup may bind to.</param>
    public static XamlExtraction Extract(string xamlText, IReadOnlyList<SampleOption> options)
    {
        var text = xamlText.Replace("\r\n", "\n").Replace('\r', '\n');

        XDocument document;
        try
        {
            document = XDocument.Parse(text, LoadOptions.PreserveWhitespace);
        }
        catch (XmlException ex)
        {
            return Withheld($"the file is not well-formed XML: {ex.Message}");
        }

        var root = document.Root;
        if (root is null)
        {
            return Withheld("the file has no root element");
        }

        var body = RootBody(text, root.Name.LocalName);
        if (body is null)
        {
            return Withheld("the root element's body could not be located");
        }

        if (body.Trim().Length == 0)
        {
            return Withheld("the root element is empty");
        }

        var (resolved, resolvedNames, droppedAttributes) = ResolveSampleAppBindings(body, options);

        // Removing an attribute leaves the blank line it occupied. Collapsing runs keeps the
        // published markup looking like the styled source it came from.
        resolved = BlankLineRunRegex().Replace(resolved, "\n\n");

        var fragment = Dedent(resolved);
        if (fragment.Length == 0)
        {
            return Withheld("nothing remained after removing sample-app-only markup");
        }

        var relocation = RelocateRootProperties(fragment, root.Name.LocalName);
        if (relocation.Error is not null)
        {
            return Withheld(relocation.Error);
        }

        fragment = relocation.Xaml!;

        var declarations = NamespaceDeclarations(root);
        fragment = StripAliasPrefixes(fragment, declarations, out declarations);

        XElement wrapper;
        try
        {
            wrapper = ParseFragment(fragment, declarations);
        }
        catch (XmlException ex)
        {
            return Withheld($"the extracted fragment is not well-formed XML: {ex.Message}");
        }

        var imports = UsedImports(wrapper, fragment, declarations);

        return new XamlExtraction(fragment, imports, resolvedNames, droppedAttributes, Error: null);
    }

    private static XamlExtraction Withheld(string reason) => new(null, [], [], [], reason);

    /// <summary>
    /// Find bindings in a published fragment whose source does not exist outside the sample app.
    /// </summary>
    /// <remarks>
    /// This is the check that keeps the contract's promise: published XAML has to be pasteable,
    /// so a binding left pointing at an options-pane member or at the discarded page element is
    /// a defect in this extractor rather than something to publish with a warning attached.
    /// It reads the binding expressions only — an attribute may legitimately be *named* after
    /// an option, as in <c>IsAlwaysOn="{x:Bind IsAlwaysOn}"</c>, where resolving the binding
    /// leaves <c>IsAlwaysOn="True"</c> and nothing is wrong.
    /// </remarks>
    public static List<string> UnsettledBindings(string xaml, IReadOnlyList<SampleOption> options)
    {
        var declaredNames = DeclaredElementNames(xaml);
        var problems = new List<string>();

        foreach (var attribute in Attributes(xaml))
        {
            foreach (var binding in Bindings(attribute.Value))
            {
                foreach (var option in options)
                {
                    if (MentionsIdentifier(binding.Arguments, option.Name))
                    {
                        problems.Add(
                            $"{attribute.Name}=\"{attribute.Value}\" binds to the sample-app option "
                            + $"'{option.Name}'");
                    }
                }

                if (binding.ElementName is { Length: > 0 } element && !declaredNames.Contains(element))
                {
                    problems.Add(
                        $"{attribute.Name}=\"{attribute.Value}\" binds to element '{element}', which the "
                        + "fragment does not declare");
                }
            }
        }

        return problems;
    }

    /// <summary>
    /// Return the raw text between the root element's start and end tags.
    /// </summary>
    /// <remarks>
    /// Taken from the text rather than rebuilt from the parsed tree so the author's formatting
    /// survives. Re-serializing would reflow attributes and expand character references, which
    /// turns a styled, readable sample into something that no longer matches its own source.
    /// </remarks>
    private static string? RootBody(string text, string rootName)
    {
        var start = text.IndexOf("<" + rootName, StringComparison.Ordinal);
        if (start < 0)
        {
            return null;
        }

        // Walk to the end of the start tag, ignoring '>' inside attribute values.
        var quote = '\0';
        var index = start;
        for (; index < text.Length; index++)
        {
            var c = text[index];
            if (quote != '\0')
            {
                if (c == quote)
                {
                    quote = '\0';
                }

                continue;
            }

            if (c is '"' or '\'')
            {
                quote = c;
            }
            else if (c == '>')
            {
                break;
            }
        }

        if (index >= text.Length)
        {
            return null;
        }

        // A self-closing root has no body.
        if (text[index - 1] == '/')
        {
            return string.Empty;
        }

        var end = text.LastIndexOf("</" + rootName, StringComparison.Ordinal);
        return end < 0 || end <= index ? null : text[(index + 1)..end];
    }

    /// <summary>
    /// Settle every binding whose source is part of the sample app rather than the sample.
    /// </summary>
    /// <remarks>
    /// Two kinds of binding do not survive extraction, and both point at the page the sample
    /// file declares:
    ///
    /// <list type="bullet">
    /// <item><description>An options-pane member. The options pane belongs to the sample app;
    /// the sample generator emits its members onto the page class, so markup binding to one
    /// names something the reader's project does not have.</description></item>
    /// <item><description>An <c>ElementName</c> naming the root element. The root is the
    /// <c>&lt;Page&gt;</c> itself, which extraction discards, so the binding has no source
    /// left once the fragment stands alone.</description></item>
    /// </list>
    ///
    /// <para>A binding whose whole path is an option becomes that option's default value, which
    /// is what the sample showed before anyone touched the options pane. Anything else — a
    /// function binding through a converter, a property of the discarded root — has no static
    /// value, so the attribute carrying it is removed and the target control's own default
    /// applies. Removing an attribute is the only edit with that property; there is no
    /// equivalent for a value embedded in an expression.</para>
    /// </remarks>
    private static (string Text, List<string> Resolved, List<string> Dropped) ResolveSampleAppBindings(
        string body,
        IReadOnlyList<SampleOption> options)
    {
        var resolved = new List<string>();
        var dropped = new List<string>();

        var byName = options.ToDictionary(o => o.Name, StringComparer.Ordinal);
        var declaredNames = DeclaredElementNames(body);
        var builder = new StringBuilder();
        var cursor = 0;

        foreach (var attribute in Attributes(body))
        {
            var bindings = Bindings(attribute.Value);
            if (bindings.Count == 0)
            {
                continue;
            }

            var settlement = Settle(bindings, byName, declaredNames);
            if (settlement is null)
            {
                continue;
            }

            builder.Append(body, cursor, attribute.Start - cursor);

            if (settlement.Value.Replacement is { } value)
            {
                builder.Append(attribute.Name).Append("=\"").Append(EscapeAttributeValue(value)).Append('"');
                resolved.Add(settlement.Value.Note!);
            }
            else
            {
                // Drop the attribute. The whitespace that separated it goes too, otherwise the
                // line it occupied survives as trailing indentation.
                dropped.Add(attribute.Name);
                TrimTrailingInlineWhitespace(builder);
            }

            cursor = attribute.End;
        }

        builder.Append(body, cursor, body.Length - cursor);

        resolved.Sort(StringComparer.Ordinal);
        dropped.Sort(StringComparer.Ordinal);

        return (builder.ToString(), Distinct(resolved), Distinct(dropped));
    }

    /// <summary>
    /// Decide what to do with one attribute's bindings: substitute a default, drop the
    /// attribute, or leave it alone.
    /// </summary>
    /// <returns>
    /// Null when the attribute binds only to things that survive extraction. Otherwise a
    /// replacement value, or null replacement meaning the attribute must go.
    /// </returns>
    private static (string? Replacement, string? Note)? Settle(
        List<BindingExpression> bindings,
        Dictionary<string, SampleOption> options,
        HashSet<string> declaredNames)
    {
        var affected = false;

        foreach (var binding in bindings)
        {
            var danglingSource = binding.ElementName is { Length: > 0 } element && !declaredNames.Contains(element);
            var mentionsOption = options.Keys.Any(name => MentionsIdentifier(binding.Arguments, name));

            if (!danglingSource && !mentionsOption)
            {
                continue;
            }

            affected = true;

            // Only a lone binding whose entire path is an option has a value to substitute.
            // Anything else leaves part of the expression unaccounted for.
            if (bindings.Count == 1
                && binding.Path is { } path
                && options.TryGetValue(path, out var option)
                && option.DefaultValue is { } value)
            {
                return (value, $"{option.Name}={value}");
            }
        }

        return affected ? (null, null) : null;
    }

    /// <summary>Every <c>x:Name</c> declared inside the fragment.</summary>
    /// <remarks>
    /// Used to tell a binding that still has a source from one that lost it. The root element's
    /// own name is absent by construction — the fragment is the root's body — which is exactly
    /// how an <c>ElementName</c> pointing at the discarded page is detected.
    /// </remarks>
    private static HashSet<string> DeclaredElementNames(string body)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);

        foreach (var attribute in Attributes(body))
        {
            if (attribute.Name is "x:Name" or "Name")
            {
                names.Add(attribute.Value);
            }
        }

        return names;
    }

    private static List<string> Distinct(List<string> values) =>
        [.. values.Distinct(StringComparer.Ordinal)];

    /// <summary>Remove the run of spaces and tabs just written, and a newline if that is all that is left.</summary>
    private static void TrimTrailingInlineWhitespace(StringBuilder builder)
    {
        var index = builder.Length - 1;
        while (index >= 0 && (builder[index] == ' ' || builder[index] == '\t'))
        {
            index--;
        }

        if (index >= 0 && builder[index] == '\n')
        {
            index--;
        }

        builder.Length = index + 1;
    }

    /// <summary>An attribute found in the raw text, with the span it occupies.</summary>
    private readonly record struct RawAttribute(string Name, string Value, int Start, int End);

    /// <summary>
    /// Index just past the tag beginning at <paramref name="tagStart"/>, for the tags that carry
    /// no attributes.
    /// </summary>
    /// <remarks>
    /// A comment ends at <c>--&gt;</c> and a CDATA section at <c>]]&gt;</c>, not at the first
    /// <c>&gt;</c>. Stopping at the first one would resume the scan inside the comment and read
    /// markup the author commented out as though it were live: an <c>x:Name</c> in a comment
    /// would count as a declared element, and an option binding in a comment would be rewritten
    /// or deleted in the published text.
    /// </remarks>
    private static int EndOfNonAttributeTag(string text, int tagStart)
    {
        var tail = text.AsSpan(tagStart);

        var terminator =
            tail.StartsWith("<!--", StringComparison.Ordinal) ? "-->"
            : tail.StartsWith("<![CDATA[", StringComparison.Ordinal) ? "]]>"
            : tail.StartsWith("<?", StringComparison.Ordinal) ? "?>"
            : ">";

        var end = text.IndexOf(terminator, tagStart, StringComparison.Ordinal);
        return end < 0 ? -1 : end + terminator.Length;
    }

    /// <summary>True when a tag carries no attributes: an end tag, comment, CDATA section or PI.</summary>
    private static bool IsNonAttributeTag(string text, int tagStart) =>
        tagStart + 1 < text.Length && text[tagStart + 1] is '/' or '!' or '?';

    /// <summary>True when a tag is a comment, CDATA section, declaration or processing instruction.</summary>
    /// <remarks>Unlike an end tag, its contents are not markup and must never be rewritten.</remarks>
    private static bool IsCommentLikeTag(string text, int tagStart) =>
        tagStart + 1 < text.Length && text[tagStart + 1] is '!' or '?';

    /// <summary>The text with comments, CDATA sections and processing instructions removed.</summary>
    /// <remarks>
    /// For the scans that read the raw text looking for markup. What an author commented out is
    /// prose as far as this tool is concerned, and reading it as markup makes the tool report
    /// requirements the published fragment does not have.
    /// </remarks>
    private static string WithoutComments(string text)
    {
        var builder = new StringBuilder();
        var index = 0;

        while (index < text.Length)
        {
            var open = text.IndexOf('<', index);
            if (open < 0 || !IsCommentLikeTag(text, open))
            {
                if (open < 0)
                {
                    break;
                }

                builder.Append(text, index, open + 1 - index);
                index = open + 1;
                continue;
            }

            var end = EndOfNonAttributeTag(text, open);
            if (end < 0)
            {
                return builder.Append(text, index, open - index).ToString();
            }

            builder.Append(text, index, open - index);
            index = end;
        }

        return builder.Append(text, index, text.Length - index).ToString();
    }

    /// <summary>
    /// Walk the attributes of every start tag in the fragment, in source order.
    /// </summary>
    /// <remarks>
    /// Done over the text because the edits are textual: the parsed tree knows which attributes
    /// exist but not where they sit in the author's formatting, and rebuilding the text from
    /// the tree is exactly what this extraction avoids.
    /// </remarks>
    private static IEnumerable<RawAttribute> Attributes(string text)
    {
        var index = 0;

        while (index < text.Length)
        {
            var tagStart = text.IndexOf('<', index);
            if (tagStart < 0)
            {
                yield break;
            }

            // Skip comments, CDATA, processing instructions and end tags: none carry attributes.
            if (IsNonAttributeTag(text, tagStart))
            {
                var skipTo = EndOfNonAttributeTag(text, tagStart);
                if (skipTo < 0)
                {
                    yield break;
                }

                index = skipTo;
                continue;
            }

            var position = tagStart + 1;

            // Element name.
            while (position < text.Length && !char.IsWhiteSpace(text[position]) && text[position] != '>' && text[position] != '/')
            {
                position++;
            }

            while (position < text.Length && text[position] != '>')
            {
                if (char.IsWhiteSpace(text[position]) || text[position] == '/')
                {
                    position++;
                    continue;
                }

                var nameStart = position;
                while (position < text.Length && text[position] != '=' && text[position] != '>' && !char.IsWhiteSpace(text[position]))
                {
                    position++;
                }

                var name = text[nameStart..position];

                while (position < text.Length && char.IsWhiteSpace(text[position]))
                {
                    position++;
                }

                if (position >= text.Length || text[position] != '=')
                {
                    continue;
                }

                position++;
                while (position < text.Length && char.IsWhiteSpace(text[position]))
                {
                    position++;
                }

                if (position >= text.Length || (text[position] != '"' && text[position] != '\''))
                {
                    continue;
                }

                var quote = text[position];
                var valueStart = ++position;
                while (position < text.Length && text[position] != quote)
                {
                    position++;
                }

                if (position >= text.Length)
                {
                    yield break;
                }

                yield return new RawAttribute(name, text[valueStart..position], nameStart, position + 1);
                position++;
            }

            index = position + 1;
        }
    }

    /// <summary>One binding markup extension found in an attribute value.</summary>
    /// <param name="Arguments">Everything between the extension name and its closing brace.</param>
    /// <param name="Path">The binding path, from the positional argument or <c>Path=</c>.</param>
    /// <param name="ElementName">The <c>ElementName</c> argument, when the binding names a source element.</param>
    internal readonly record struct BindingExpression(string Arguments, string? Path, string? ElementName);

    /// <summary>
    /// Read every <c>{x:Bind}</c> and <c>{Binding}</c> extension in an attribute value.
    /// </summary>
    /// <remarks>
    /// Both forms matter. <c>x:Bind</c> resolves against the page class, so it reaches the
    /// generated option members; <c>Binding</c> with an <c>ElementName</c> reaches the page
    /// element itself. Nested extensions such as <c>Converter={StaticResource ...}</c> are
    /// stepped over rather than treated as bindings of their own.
    /// </remarks>
    internal static List<BindingExpression> Bindings(string attributeValue)
    {
        var bindings = new List<BindingExpression>();

        for (var index = 0; index < attributeValue.Length; index++)
        {
            if (attributeValue[index] != '{')
            {
                continue;
            }

            var nameStart = index + 1;
            while (nameStart < attributeValue.Length && char.IsWhiteSpace(attributeValue[nameStart]))
            {
                nameStart++;
            }

            var nameEnd = nameStart;
            while (nameEnd < attributeValue.Length
                   && !char.IsWhiteSpace(attributeValue[nameEnd])
                   && attributeValue[nameEnd] != '}'
                   && attributeValue[nameEnd] != ',')
            {
                nameEnd++;
            }

            var name = attributeValue[nameStart..nameEnd];
            if (name is not ("x:Bind" or "Binding"))
            {
                continue;
            }

            var arguments = ExtensionArguments(attributeValue, nameEnd, out var end);
            bindings.Add(Parse(arguments));

            // Skip past this extension so a nested one is not read as a second binding.
            index = end;
        }

        return bindings;
    }

    /// <summary>The text of an extension's arguments, with braces balanced.</summary>
    private static string ExtensionArguments(string value, int start, out int end)
    {
        var depth = 0;
        var index = start;

        for (; index < value.Length; index++)
        {
            var c = value[index];
            if (c == '{')
            {
                depth++;
            }
            else if (c == '}')
            {
                if (depth == 0)
                {
                    break;
                }

                depth--;
            }
        }

        end = index;
        return value[start..index];
    }

    private static BindingExpression Parse(string arguments)
    {
        string? path = null;
        string? elementName = null;

        foreach (var argument in SplitTopLevel(arguments))
        {
            var separator = TopLevelEquals(argument);
            if (separator < 0)
            {
                // The positional argument is the path, e.g. {x:Bind IsCardEnabled, Mode=OneWay}.
                path ??= StripCast(argument.Trim()) is { Length: > 0 } value ? value : null;
                continue;
            }

            var key = argument[..separator].Trim();
            var argumentValue = argument[(separator + 1)..].Trim();

            if (key == "Path")
            {
                path = StripCast(argumentValue);
            }
            else if (key == "ElementName")
            {
                elementName = argumentValue;
            }
        }

        return new BindingExpression(arguments, path, elementName);
    }

    /// <summary>Remove a cast from the front of a binding path.</summary>
    /// <remarks>
    /// <c>x:Bind</c> spells a cast as a parenthesised type ahead of the path, as in
    /// <c>(x:Int32)Columns</c>, which still binds to <c>Columns</c>. Leaving the cast attached
    /// means the path matches no option, so a binding that has a perfectly readable default to
    /// substitute is treated as having none and its attribute is removed instead.
    ///
    /// <para>A parenthesised group with nothing after it is an attached property rather than a
    /// cast — <c>{Binding (Grid.Row)}</c> — and is part of the path, so it is left alone.</para>
    /// </remarks>
    private static string StripCast(string path)
    {
        var trimmed = path.TrimStart();
        if (trimmed.Length == 0 || trimmed[0] != '(')
        {
            return path;
        }

        var close = trimmed.IndexOf(')');
        if (close < 0)
        {
            return path;
        }

        var rest = trimmed[(close + 1)..].TrimStart();
        return rest.Length == 0 ? path : rest;
    }

    /// <summary>Split extension arguments on commas that are not inside a nested extension.</summary>
    private static List<string> SplitTopLevel(string arguments)
    {
        var parts = new List<string>();
        var depth = 0;
        var start = 0;

        for (var index = 0; index < arguments.Length; index++)
        {
            switch (arguments[index])
            {
                case '{':
                    depth++;
                    break;
                case '}':
                    depth--;
                    break;
                case ',' when depth == 0:
                    parts.Add(arguments[start..index]);
                    start = index + 1;
                    break;
            }
        }

        parts.Add(arguments[start..]);
        return parts;
    }

    /// <summary>Index of the argument's <c>=</c>, ignoring any inside a nested extension.</summary>
    private static int TopLevelEquals(string argument)
    {
        var depth = 0;

        for (var index = 0; index < argument.Length; index++)
        {
            switch (argument[index])
            {
                case '{':
                    depth++;
                    break;
                case '}':
                    depth--;
                    break;
                case '=' when depth == 0:
                    return index;
            }
        }

        return -1;
    }

    /// <summary>
    /// True when the text contains the identifier as a whole word.
    /// </summary>
    /// <remarks>
    /// Whole-word matching keeps an option named <c>Color</c> from claiming
    /// <c>ColorsCollection</c>, which binds to something else entirely. A preceding dot also
    /// disqualifies a match: in <c>ViewPanel.OpenPaneLength</c> the trailing name is a property
    /// of a named element, not the option member.
    /// </remarks>
    internal static bool MentionsIdentifier(string text, string identifier)
    {
        var index = text.IndexOf(identifier, StringComparison.Ordinal);

        while (index >= 0)
        {
            var beforeOk = index == 0 || (!IsIdentifierChar(text[index - 1]) && text[index - 1] != '.');
            var after = index + identifier.Length;
            var afterOk = after >= text.Length || !IsIdentifierChar(text[after]);

            if (beforeOk && afterOk)
            {
                return true;
            }

            index = text.IndexOf(identifier, index + 1, StringComparison.Ordinal);
        }

        return false;
    }

    private static bool IsIdentifierChar(char c) => char.IsLetterOrDigit(c) || c == '_';

    private static string EscapeAttributeValue(string value) =>
        value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

    /// <summary>Remove the indentation the fragment carried inside its root element.</summary>
    private static string Dedent(string text)
    {
        var lines = text.Split('\n');
        var indent = int.MaxValue;

        foreach (var line in lines)
        {
            if (line.Trim().Length == 0)
            {
                continue;
            }

            var count = 0;
            while (count < line.Length && (line[count] == ' ' || line[count] == '\t'))
            {
                count++;
            }

            indent = Math.Min(indent, count);
        }

        if (indent is 0 or int.MaxValue)
        {
            return text.Trim('\n', ' ', '\t');
        }

        var builder = new StringBuilder();
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            builder.Append(line.Length >= indent ? line[indent..] : line.TrimStart());
            if (i < lines.Length - 1)
            {
                builder.Append('\n');
            }
        }

        return builder.ToString().Trim('\n', ' ', '\t');
    }

    /// <summary>The namespace declarations the sample file's root element carries.</summary>
    private static List<(string Prefix, string Uri)> NamespaceDeclarations(XElement root)
    {
        var declarations = new List<(string Prefix, string Uri)>();

        foreach (var attribute in root.Attributes())
        {
            if (!attribute.IsNamespaceDeclaration)
            {
                continue;
            }

            var prefix = attribute.Name.LocalName == "xmlns" ? string.Empty : attribute.Name.LocalName;
            declarations.Add((prefix, attribute.Value));
        }

        return declarations;
    }

    /// <summary>
    /// Remove prefixes that are only another spelling of the default namespace.
    /// </summary>
    /// <remarks>
    /// Several samples declare <c>xmlns:win="…/presentation"</c> and write <c>&lt;win:TextBox&gt;</c>.
    /// That is Uno's conditional XAML, where the prefix means "only on Windows"; on Windows it
    /// resolves to the same namespace as no prefix at all, so <c>&lt;win:TextBox&gt;</c> and
    /// <c>&lt;TextBox&gt;</c> are the same element. Publishing the prefix would hand the reader
    /// a multi-platform concept they did not ask about, together with an xmlns they have to
    /// carry to make it parse. Dropping it leaves markup that is identical where it will run.
    /// </remarks>
    private static string StripAliasPrefixes(
        string fragment,
        List<(string Prefix, string Uri)> declarations,
        out List<(string Prefix, string Uri)> remaining)
    {
        var aliases = declarations
            .Where(d => d.Prefix.Length > 0 && d.Uri == PresentationNamespace)
            .Select(d => d.Prefix)
            .ToHashSet(StringComparer.Ordinal);

        remaining = declarations.Where(d => !aliases.Contains(d.Prefix)).ToList();

        if (aliases.Count == 0)
        {
            return fragment;
        }

        var pattern = "(" + string.Join('|', aliases.Select(Regex.Escape)) + ")";
        var builder = new StringBuilder();
        var index = 0;

        while (index < fragment.Length)
        {
            var open = fragment.IndexOf('<', index);
            if (open < 0)
            {
                builder.Append(fragment, index, fragment.Length - index);
                break;
            }

            // A comment or CDATA section declares no prefixes and must not be rewritten. It also
            // has to be stepped over as a unit: EndOfStartTag reads an apostrophe in prose as an
            // opening quote, which would swallow the rest of the fragment.
            if (IsCommentLikeTag(fragment, open))
            {
                var skipTo = EndOfNonAttributeTag(fragment, open);
                if (skipTo < 0)
                {
                    builder.Append(fragment, index, fragment.Length - index);
                    break;
                }

                builder.Append(fragment, index, skipTo - index);
                index = skipTo;
                continue;
            }

            var close = EndOfStartTag(fragment, open);
            if (close < 0)
            {
                builder.Append(fragment, index, fragment.Length - index);
                break;
            }

            var tag = fragment[open..(close + 1)];
            var stripped = Regex.Replace(tag, $@"(?<=^</?){pattern}:", string.Empty);
            stripped = Regex.Replace(stripped, $@"(?<=\s){pattern}:(?=[A-Za-z_])", string.Empty);

            builder.Append(fragment, index, open - index);
            builder.Append(stripped == tag ? tag : Realign(stripped, IndentOf(fragment, open)));
            index = close + 1;
        }

        return builder.ToString();
    }

    /// <summary>Width of the whitespace run that begins the line containing <paramref name="position"/>.</summary>
    private static int IndentOf(string text, int position)
    {
        var lineStart = text.LastIndexOf('\n', Math.Max(position - 1, 0)) + 1;
        return position - lineStart;
    }

    /// <summary>
    /// Re-indent a start tag's continuation lines so its attributes line up again.
    /// </summary>
    /// <remarks>
    /// Removing a prefix shortens the element name, which leaves every attribute on the lines
    /// below it indented to a column that no longer exists. The repository formats XAML with
    /// attributes aligned one space past the element name, so that is the column they are put
    /// back at — the reader sees the house style, not the fact that something was edited.
    /// </remarks>
    private static string Realign(string tag, int baseIndent)
    {
        var lines = tag.Split('\n');
        if (lines.Length == 1)
        {
            return tag;
        }

        var name = Regex.Match(lines[0], @"^</?(?<name>[\w.:]+)");
        if (!name.Success)
        {
            return tag;
        }

        var column = new string(' ', baseIndent + 1 + name.Groups["name"].Value.Length + 1);

        for (var i = 1; i < lines.Length; i++)
        {
            // Only lines that begin with an attribute are the tag's own layout. A line that
            // starts mid-value belongs to the value and is left exactly as the author wrote it.
            if (!Regex.IsMatch(lines[i], @"^\s*[\w.:]+\s*=|^\s*/?>$"))
            {
                return tag;
            }

            lines[i] = column + lines[i].TrimStart();
        }

        return string.Join('\n', lines);
    }

    /// <summary>What relocating a root property element produced.</summary>
    private readonly record struct Relocation(string? Xaml, string? Error);

    /// <summary>
    /// Move property elements belonging to the discarded root onto an element that survives.
    /// </summary>
    /// <remarks>
    /// A sample that defines a converter writes <c>&lt;Page.Resources&gt;</c>, which belongs to
    /// the <c>&lt;Page&gt;</c> the fragment drops. Pasted into a <c>&lt;Grid&gt;</c> that markup
    /// does not compile, because a Grid has no Page property — so the snippet would fail on the
    /// reader's first build, in their file. Attaching the resources to the sample's own root
    /// element instead keeps every key in scope for the markup that uses it, and is what the
    /// author would have written had there been no page.
    /// </remarks>
    private static Relocation RelocateRootProperties(string fragment, string rootName)
    {
        var propertyElement = Regex.Match(
            fragment,
            $@"^[ \t]*<{Regex.Escape(rootName)}\.(?<property>\w+)>.*?</{Regex.Escape(rootName)}\.\k<property>>[ \t]*\n?",
            RegexOptions.Singleline | RegexOptions.Multiline);

        if (!propertyElement.Success)
        {
            return new Relocation(fragment, null);
        }

        var block = propertyElement.Value.Trim('\n');
        var property = propertyElement.Groups["property"].Value;
        var rest = fragment.Remove(propertyElement.Index, propertyElement.Length).Trim('\n');

        if (Regex.IsMatch(rest, $@"<{Regex.Escape(rootName)}\.\w+>"))
        {
            return new Relocation(
                null,
                $"the sample declares more than one <{rootName}.*> block, which cannot be attached "
                + "to a single element");
        }

        var host = Regex.Match(rest, @"^<(?<name>[\w.:]+)");
        if (!host.Success)
        {
            return new Relocation(
                null,
                $"the sample's <{rootName}.{property}> has no sibling element to attach to");
        }

        var hostName = host.Groups["name"].Value;
        var startTagEnd = EndOfStartTag(rest, 0);
        if (startTagEnd < 0)
        {
            return new Relocation(null, $"the start tag of <{hostName}> could not be located");
        }

        var selfClosing = rest[startTagEnd - 1] == '/';
        var moved = Indent(
            block.Replace($"<{rootName}.{property}>", $"<{hostName}.{property}>")
                 .Replace($"</{rootName}.{property}>", $"</{hostName}.{property}>"),
            "    ");

        if (selfClosing)
        {
            // <Button ... /> has to become <Button ...> … </Button> to hold the resources.
            var openTag = rest[..(startTagEnd - 1)].TrimEnd() + ">";
            var after = rest[(startTagEnd + 1)..];
            return new Relocation($"{openTag}\n{moved}\n</{hostName}>{after}", null);
        }

        return new Relocation(
            $"{rest[..(startTagEnd + 1)]}\n{moved}{rest[(startTagEnd + 1)..]}",
            null);
    }

    /// <summary>Index of the '&gt;' closing the start tag beginning at <paramref name="start"/>.</summary>
    private static int EndOfStartTag(string text, int start)
    {
        var quote = '\0';

        for (var i = start; i < text.Length; i++)
        {
            var c = text[i];

            if (quote != '\0')
            {
                if (c == quote)
                {
                    quote = '\0';
                }

                continue;
            }

            if (c is '"' or '\'')
            {
                quote = c;
            }
            else if (c == '>')
            {
                return i;
            }
        }

        return -1;
    }

    private static string Indent(string text, string prefix) =>
        string.Join(
            '\n',
            text.Split('\n').Select(line => line.Length == 0 ? line : prefix + line));

    /// <summary>
    /// Parse the fragment on its own, under the declarations its file provided, so that a
    /// fragment referencing an undeclared prefix is caught here rather than by the reader.
    /// </summary>
    private static XElement ParseFragment(string fragment, List<(string Prefix, string Uri)> declarations)
    {
        var builder = new StringBuilder("<SampleIndexFragment");

        foreach (var (prefix, uri) in declarations)
        {
            builder.Append(prefix.Length == 0 ? " xmlns=\"" : $" xmlns:{prefix}=\"");
            builder.Append(EscapeAttributeValue(uri)).Append('"');
        }

        builder.Append('>').Append(fragment).Append("</SampleIndexFragment>");

        return XElement.Parse(builder.ToString(), LoadOptions.PreserveWhitespace);
    }

    /// <summary>
    /// The declarations the fragment actually needs, written out as they appear in the source.
    /// </summary>
    /// <remarks>
    /// Only what is used: an import a sample does not need reads as an instruction to reference
    /// a package it does not need. The default presentation namespace and the <c>x</c> language
    /// namespace are left out because every XAML file the reader could paste into already
    /// declares both, and the design-time namespaces are left out because they mean nothing
    /// outside a designer.
    /// </remarks>
    private static List<string> UsedImports(
        XElement fragment,
        string fragmentText,
        List<(string Prefix, string Uri)> declarations)
    {
        var usedUris = new HashSet<string>(StringComparer.Ordinal);

        foreach (var element in fragment.DescendantsAndSelf())
        {
            usedUris.Add(element.Name.NamespaceName);

            foreach (var attribute in element.Attributes())
            {
                if (!attribute.IsNamespaceDeclaration)
                {
                    usedUris.Add(attribute.Name.NamespaceName);
                }
            }
        }

        // Markup extensions live inside attribute values, where the XML parser sees only text,
        // so their prefixes have to be read from the text. Commented-out markup is excluded:
        // a prefix used only there is not one the reader has to declare.
        var usedPrefixes = new HashSet<string>(StringComparer.Ordinal);
        foreach (Match match in MarkupExtensionPrefixRegex().Matches(WithoutComments(fragmentText)))
        {
            usedPrefixes.Add(match.Groups["prefix"].Value);
        }

        var imports = new List<string>();

        foreach (var (prefix, uri) in declarations)
        {
            if (prefix.Length == 0 || uri == PresentationNamespace || uri == XamlNamespace || DesignTimeNamespaces.Contains(uri))
            {
                continue;
            }

            if (usedUris.Contains(uri) || usedPrefixes.Contains(prefix))
            {
                imports.Add($"xmlns:{prefix}=\"{uri}\"");
            }
        }

        imports.Sort(StringComparer.Ordinal);
        return imports;
    }
}
