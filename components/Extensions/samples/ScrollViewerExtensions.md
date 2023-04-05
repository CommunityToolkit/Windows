---
title: ScrollViewerExtensions
author: ST-Apps
description: ScrollViewerExtensions type provides a simple way to manage Margin for any ScrollBar inside any container.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, ScrollViewer, extensions
dev_langs:
  - csharp
category: Extensions
subcategory: Layout
discussion-id: 0
issue-id: 0
---

# ScrollViewerExtensions

The [`ScrollViewerExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.scrollviewerextensions) type provides extension methods to improve your [`ScrollViewer`](/uwp/api/windows.ui.xaml.controls.scrollviewer) implementation.

> **Platform APIs:** [`ScrollViewerExtensions`](/dotnet/api/microsoft.toolkit.uwp.ui.scrollviewerextensions)

## ScrollBarMargin

The `ScrollBarMargin` property provides a way to assign a [`Thickness`](/dotnet/api/system.windows.thickness) to the vertical/horizontal [`ScrollBar`](/uwp/api/windows.ui.xaml.controls.primitives.scrollbar) of your container.

Here is how this property can be used in XAML:

```xaml
<Page xmlns:ui="using:Microsoft.Toolkit.Uwp.UI">
    <ListView ui:ScrollViewerExtensions.HorizontalScrollBarMargin="2, 2, 2, 2">
        <!-- ListView Item -->
    </ListView>

    <ListView ui:ScrollViewerExtensions.VerticalScrollBarMargin="2, 2, 2, 2">
        <!-- ListView Item -->
    </ListView>
</Page>
```

And here it is in action in a more complex example, where the margin is also bound:

```xaml
<ListView
    Name="listView"
    xmlns:ui="using:Microsoft.Toolkit.Uwp.UI"
    ui:ScrollViewerExtensions.VerticalScrollBarMargin="{Binding MinHeight, ElementName=MyHeaderGrid, Converter={StaticResource DoubleTopThicknessConverter}}">
    <ListView.Header>
        <controls:ScrollHeader Mode="Sticky">
            <Grid
                x:Name="MyHeaderGrid"
                MinHeight="100"
                Background="{ThemeResource SystemControlAccentAcrylicElementAccentMediumHighBrush}">
                <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
                    <TextBlock
                        Margin="12"
                        FontSize="48"
                        FontWeight="Bold"
                        Foreground="{StaticResource Brush-White}"
                        Text="Scroll Header"
                        TextAlignment="Center"
                        TextWrapping="WrapWholeWords" />
                </StackPanel>
            </Grid>
        </controls:ScrollHeader>
    </ListView.Header>
</ListView>
```

This converter is used to just bind to top margin, moving only the `ScrollBar`'s top end.

```csharp
public class DoubleTopThicknessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return new Thickness(0, (double)value, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return ((Thickness)value).Top;
    }
}
```

> [!NOTE]
> It would also be possible to use `{x:Bind}` and replace the converter with a static function, which would also result in the code being checked at build-time and also running faster and more efficiently.

## MiddleClickScrolling

`MiddleClickScrolling` allows you to scroll by click middle mouse button (scroll wheel button) and move the pointer of the direction to be scrolled. This extension method can be used directly in `ScrollViewer` or ancestor of `ScrollViewer`.

Here is how this property can be used in XAML:

```xaml
<!-- Setting MiddleClickScrolling directely for ScrollViewer -->
<ScrollViewer
    xmlns:ui="using:Microsoft.Toolkit.Uwp.UI"
    ui:ScrollViewerExtensions.EnableMiddleClickScrolling="True">
    <!-- ScrollViewer items -->
</ScrollViewer>

<!-- Setting MiddleClickScrolling fot the ancestor of ScrollViewer -->
<ListView
    xmlns:ui="using:Microsoft.Toolkit.Uwp.UI"
    ui:ScrollViewerExtensions.EnableMiddleClickScrolling="True">
    <!-- ListView items -->
</ListView>
```

This code results in the following UI:

![Middle click scrolling UI](../resources/images/Extensions/MiddleClickScrolling.gif)

### Changing Cursor Type

> [!IMPORTANT]
> Resource file must be manually added to change the cursor type when middle click scrolling. If you didn't add then the cursor won't change when middle click scrolling but functionality won't be affected.

#### Using Existing Resource File

1. Download [MiddleClickScrolling-CursorType.res](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/Microsoft.Toolkit.Uwp.UI/Extensions/ScrollViewer/MiddleClickScrolling-CursorType.res) file
2. Move this file into your project's folder
3. Open .csproj file of your project in [Visual Studio Code](https://code.visualstudio.com/) or in any other code editor
4. Add `<Win32Resource>MiddleClickScrolling-CursorType.res</Win32Resource>` in the first `<PropertyGroup>`

### Edit Existing Resource File

You can easily edit the existing resource file to customize the cursor depending upon your needs.

1. Follow the above steps to add the resource file
2. Open MiddleClickScrolling-CursorType.res file in Visual Studio
3. Open Cursor folder
4. Now you can edit the cursor by opening 101, 102, ....., 109

## Examples

You can find more examples in the [unit tests](https://github.com/windows-toolkit/WindowsCommunityToolkit/tree/rel/7.1.0/UnitTests).
