
# Windows Community Toolkit - Triggers

This package is part of the [Windows Community Toolkit](https://aka.ms/toolkit/windows) from the [.NET Foundation](https://dotnetfoundation.org).

## Package Contents

This package contains the following triggers in the `CommunityToolkit.WinUI` namespace:

- CompareStateTrigger
- ControlSizeTrigger
- IsEqualStateTrigger
- IsNotEqualStateTrigger
- IsNullOrEmptyStateTrigger
- NetworkConnectionStateTrigger
- RegexStateTrigger
- UserHandPreferenceStateTrigger

## Which Package is for me?

If you're developing with _UWP/WinUI 2 or Uno.UI_ you should be using the `CommunityToolkit.Uwp.Triggers` package.

If you're developing with _WindowsAppSDK/WinUI 3 or Uno.WinUI_ you should be using the `CommunityToolkit.WinUI.Triggers` package.

## WinUI Resources (UWP)

For UWP projects, the WinUI 2 reference requires you include the WinUI XAML Resources in your App.xaml file:

```xml
    <Application.Resources>
        <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
    </Application.Resources>
```

See [Getting Started in WinUI 2](https://learn.microsoft.com/windows/apps/winui/winui2/getting-started) for more information.

## Documentation

Further documentation about these components can be found at: https://aka.ms/windowstoolkitdocs

## License

MIT

See License.md in package for more details.
