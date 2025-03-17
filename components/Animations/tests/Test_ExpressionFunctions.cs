// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINUI2
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
#elif WINUI3
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
#endif

using System.Numerics;
using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;
using CommunityToolkit.WinUI.Animations.Expressions;

namespace AnimationsTests;

[TestClass]
[TestCategory(nameof(Test_ExpressionFunctions))]
public partial class Test_ExpressionFunctions : VisualUITestBase
{
    [UIThreadTestMethod]
    public void ColorLerpRgb(Grid rootGrid)
    {
        // See https://github.com/CommunityToolkit/Windows/issues/303
        var compositor = ElementCompositionPreview.GetElementVisual(rootGrid).Compositor;
        var brush = compositor.CreateColorBrush();
        var temp = ExpressionFunctions.ColorRgb(255f, 255f, 0f, 0f);
        var color = ExpressionFunctions.ColorLerpRgb(temp, temp, 0.5f);

        brush.StartAnimation("Color", color);

        var visual = compositor.CreateSpriteVisual();
        visual.Brush = brush;
        visual.RelativeSizeAdjustment = Vector2.One;

        ElementCompositionPreview.SetElementChildVisual(rootGrid, visual);
    }

    [UIThreadTestMethod]
    public void ColorLerpHsl(Grid rootGrid)
    {
        // See https://github.com/CommunityToolkit/Windows/issues/303
        var compositor = ElementCompositionPreview.GetElementVisual(rootGrid).Compositor;
        var brush = compositor.CreateColorBrush();
        var temp = ExpressionFunctions.ColorHsl(255f, 255f, 0f);
        var color = ExpressionFunctions.ColorLerpHsl(temp, temp, 0.5f);

        brush.StartAnimation("Color", color);

        var visual = compositor.CreateSpriteVisual();
        visual.Brush = brush;
        visual.RelativeSizeAdjustment = Vector2.One;

        ElementCompositionPreview.SetElementChildVisual(rootGrid, visual);
    }
}
