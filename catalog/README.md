# Sample index

`toolkit-samples.json` is a machine-readable index of every sample in this repository. It exists so that tools outside the Toolkit — documentation sites, search, AI coding assistants — can offer Toolkit samples without scraping the repository and guessing at its layout.

The file is generated. Do not edit it by hand.

## What is in it

One entry per documentation page under `components/*/samples/`, and one sample per `[!SAMPLE]` marker in that page, in the order the page presents them.

```jsonc
{
  "schemaVersion": 1,
  "source": "toolkit",
  "controls": [
    {
      "id": "settingscard",
      "name": "SettingsCard",
      "description": "A card control that can be used to create Windows 11 style settings experiences.",
      "nugetPackage": "CommunityToolkit.WinUI.Controls.SettingsControls",
      "curatedKeywords": ["SettingsCard", "Control", "Layout", "Settings"],
      "usings": ["System.ComponentModel"],
      "docs": [{ "uri": "https://github.com/CommunityToolkit/Windows/blob/main/components/..." }],
      "samples": [
        {
          "header": "SettingsCard",
          "xaml": "<StackPanel Spacing=\"4\">…</StackPanel>",
          "xmlnsImports": ["xmlns:controls=\"using:CommunityToolkit.WinUI.Controls\""],
          "toolkit": { "sampleId": "SettingsCardSample", "sourcePath": "components/…/SettingsCardSample.xaml" }
        }
      ]
    }
  ]
}
```

Pages that document APIs with no markup to show — most of `Extensions` and `Helpers` — appear with an empty `samples` array, so the index carries the whole component surface rather than only the parts that happen to have XAML.

Everything specific to this repository lives under a `toolkit` object, leaving the rest of each entry portable across sample sources.

## The XAML is meant to be pasted

Each `xaml` value is the sample's markup with the sample app removed from it:

- The `<Page>` wrapper and its `x:Class` are gone, along with design-time namespaces.
- Options that the sample app renders as sliders and toggles are replaced by the value the app starts with, so the snippet shows the sample in the state the gallery opens it in. An option whose value cannot be written as a literal has its attribute removed instead of guessed at, which leaves the control at its own default.
- `<Page.Resources>` moves onto the element that survives, so referenced keys stay in scope.
- Conditional `win:` prefixes are dropped, since on Windows they name the same elements as no prefix at all.
- `xmlnsImports` lists only the namespace declarations the snippet actually uses.

What changes is the environment, never what the sample demonstrates. A sample whose markup cannot be made pasteable is left out and reported rather than published broken.

## The C# is the sample, not the page

A sample that has code-behind also carries a `code` value: the handlers its markup calls and the types its markup binds to, as members to drop into a page. The license header, the sample app's namespace, the page class, the `[ToolkitSample…]` attributes and the `InitializeComponent` call are all scaffolding for an app the reader is not building, so none of them appear. Conditional branches are resolved for WinAppSDK, so the reader is not handed a choice that has already been made.

Most samples have nothing left once that is removed, and those publish no `code` at all rather than a constructor that says nothing. A constructor that does something the sample needs is kept, named after the sample — rename it to your own page, the same adaptation the markup's `x:Class` already asks for.

The namespaces that code needs are published once per entry, as `usings`, rather than repeated as `using` lines inside every snippet — consumers prepend them. The list is narrowed to what the published members actually use: a sample file imports whatever its whole page needed, and most of that page is scaffolding nobody is handed, so publishing the file's imports verbatim would ask you to reference packages for code you never got. Namespaces that exist only in this repository's sample app are never published, since they would not resolve anywhere a snippet is pasted.

## Regenerating

```shell
dotnet run --project tools/SampleIndexExporter -- generate
```

Commit the result alongside the sample change that caused it. CI runs the same tool in `check` mode and fails if the committed file no longer matches the samples, so an index that drifts is caught in the pull request that caused the drift.

```shell
dotnet run --project tools/SampleIndexExporter -- check
dotnet test tools/SampleIndexExporter.Tests
```

The tests are the guarantees consumers rely on: the committed file matches the samples, generation is deterministic, every published snippet parses and declares the prefixes and namespaces it uses, and anything excluded is listed by name rather than disappearing quietly.
