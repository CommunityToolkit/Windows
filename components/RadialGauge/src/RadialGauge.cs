// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Helpers;
using System.Numerics;
#if WINAPPSDK
using Path = Microsoft.UI.Xaml.Shapes.Path;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Composition;
#else
using Path = Windows.UI.Xaml.Shapes.Path;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
#endif

namespace CommunityToolkit.WinUI.Controls;
/// <summary>
/// A Modern UI Radial Gauge using XAML and Composition API.
/// The scale of the gauge is a clockwise arc that sweeps from MinAngle (default lower left, at -150°) to MaxAngle (default lower right, at +150°).
/// </summary>
//// All calculations are for a 200x200 square. The viewbox will do the rest.
[TemplatePart(Name = ContainerPartName, Type = typeof(Grid))]
[TemplatePart(Name = ScalePartName, Type = typeof(Path))]
[TemplatePart(Name = TrailPartName, Type = typeof(Path))]
[TemplatePart(Name = ValueTextPartName, Type = typeof(TextBlock))]
[TemplateVisualState(Name = NormalState, GroupName = CommonStates)]
[TemplateVisualState(Name = DisabledState, GroupName = CommonStates)]
public partial class RadialGauge : RangeBase
{
    // States
    private const string NormalState = "Normal";
    private const string DisabledState = "Disabled";
    private const string CommonStates = "CommonStates";

    // Template Parts.
    private const string ContainerPartName = "PART_Container";
    private const string ScalePartName = "PART_Scale";
    private const string TrailPartName = "PART_Trail";
    private const string ValueTextPartName = "PART_ValueText";
    private const string UnitTextPartName = "PART_UnitText";

    // For convenience.
    private const double Degrees2Radians = Math.PI / 180;

    // High-contrast accessibility
    private static readonly ThemeListener ThemeListener = new ThemeListener();
    private SolidColorBrush? _needleBrush;
    private SolidColorBrush? _needleBorderBrush;
    private Brush? _trailBrush;
    private Brush? _scaleBrush;
    private SolidColorBrush? _scaleTickBrush;
    private SolidColorBrush? _tickBrush;
    private Brush? _foreground;

    private double _normalizedMinAngle;
    private double _normalizedMaxAngle;

    private Compositor? _compositor;
    private ContainerVisual? _root;
    private CompositionSpriteShape? _needle;

    /// <summary>
    /// Initializes a new instance of the <see cref="RadialGauge"/> class.
    /// Create a default radial gauge control.
    /// </summary>
    public RadialGauge()
    {
        DefaultStyleKey = typeof(RadialGauge);

        SmallChange = 1;
        LargeChange = 10;

        SetKeyboardAccelerators();
    }

    private void RadialGauge_Unloaded(object sender, RoutedEventArgs e)
    {
        // TODO: We should just use a WeakEventListener for ThemeChanged here, but ours currently doesn't support it.
        // See proposal for general helper here: https://github.com/CommunityToolkit/dotnet/issues/404
        ThemeListener.ThemeChanged -= ThemeListener_ThemeChanged;
        PointerReleased -= RadialGauge_PointerReleased;
        IsEnabledChanged -= RadialGauge_IsEnabledChanged;
        Unloaded -= RadialGauge_Unloaded;
    }

    /// <summary>
    /// Update the visual state of the control when its template is changed.
    /// </summary>
    protected override void OnApplyTemplate()
    {
        PointerReleased -= RadialGauge_PointerReleased;
        ThemeListener.ThemeChanged -= ThemeListener_ThemeChanged;
        IsEnabledChanged -= RadialGauge_IsEnabledChanged;
        Unloaded -= RadialGauge_Unloaded;
        
        // Remember local brushes.
        _needleBrush = ReadLocalValue(NeedleBrushProperty) as SolidColorBrush;
        _needleBorderBrush = ReadLocalValue(NeedleBorderBrushProperty) as SolidColorBrush;
        _trailBrush = ReadLocalValue(TrailBrushProperty) as SolidColorBrush;
        _scaleBrush = ReadLocalValue(ScaleBrushProperty) as SolidColorBrush;
        _scaleTickBrush = ReadLocalValue(ScaleTickBrushProperty) as SolidColorBrush;
        _tickBrush = ReadLocalValue(TickBrushProperty) as SolidColorBrush;
        _foreground = ReadLocalValue(ForegroundProperty) as SolidColorBrush; 

        PointerReleased += RadialGauge_PointerReleased;
        ThemeListener.ThemeChanged += ThemeListener_ThemeChanged;
        IsEnabledChanged += RadialGauge_IsEnabledChanged;
        Unloaded += RadialGauge_Unloaded;

        // Apply color scheme.
        OnColorsChanged();
        OnUnitChanged(this);
        OnEnabledChanged();
        OnInteractivityChanged(this);
        base.OnApplyTemplate();
    }

    private void ThemeListener_ThemeChanged(ThemeListener sender)
    {
        OnColorsChanged();
    }

    private void RadialGauge_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        OnEnabledChanged();
    }

    /// <summary>
    /// Gets the normalized minimum angle.
    /// </summary>
    /// <value>The minimum angle in the range from -180 to 180.</value>
    protected double NormalizedMinAngle => _normalizedMinAngle;

    /// <summary>
    /// Gets the normalized maximum angle.
    /// </summary>
    /// <value>The maximum angle, in the range from -180 to 540.</value>
    protected double NormalizedMaxAngle => _normalizedMaxAngle;

    /// <inheritdoc/>
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new RadialGaugeAutomationPeer(this);
    }

    /// <inheritdoc/>
    protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
    {
        base.OnMinimumChanged(oldMinimum, newMinimum);
        OnScaleChanged(this);
    }

    /// <inheritdoc/>
    protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
    {
        base.OnMaximumChanged(oldMaximum, newMaximum);
        OnScaleChanged(this);
    }

    /// <inheritdoc/>
    protected override void OnValueChanged(double oldValue, double newValue)
    {
        OnValueChanged(this);
        base.OnValueChanged(oldValue, newValue);
        if (AutomationPeer.ListenerExists(AutomationEvents.LiveRegionChanged))
        {
            var peer = FrameworkElementAutomationPeer.FromElement(this) as RadialGaugeAutomationPeer;
            peer?.RaiseValueChangedEvent(oldValue, newValue);
        }
    }

    private static void OnValueChanged(DependencyObject d)
    {
        RadialGauge radialGauge = (RadialGauge)d;
        if (!double.IsNaN(radialGauge.Value))
        {
            if (radialGauge.StepSize != 0)
            {
                radialGauge.Value = radialGauge.RoundToMultiple(radialGauge.Value, radialGauge.StepSize);
            }

            var middleOfScale = 100 - radialGauge.ScalePadding - (radialGauge.ScaleWidth / 2);
            if (middleOfScale >= 0)
            {
                var valueText = radialGauge.GetTemplateChild(ValueTextPartName) as TextBlock;
                radialGauge.ValueAngle = radialGauge.ValueToAngle(radialGauge.Value);

                // Needle
                if (radialGauge._needle != null)
                {
                    radialGauge._needle.RotationAngleInDegrees = (float)radialGauge.ValueAngle;
                }

                // Trail
                var trail = radialGauge.GetTemplateChild(TrailPartName) as Path;
                if (trail != null)
                {
                    if (radialGauge.ValueAngle > radialGauge.NormalizedMinAngle)
                    {
                        trail.Visibility = Visibility.Visible;

                        if (radialGauge.ValueAngle - radialGauge.NormalizedMinAngle == 360)
                        {
                            // Draw full circle.
                            var eg = new EllipseGeometry
                            {
                                Center = new Point(100, 100),
                                RadiusX = 100 - radialGauge.ScalePadding - (radialGauge.ScaleWidth / 2)
                            };
                            eg.RadiusY = eg.RadiusX;
                            trail.Data = eg;
                        }
                        else
                        {
                            // Draw arc.
                            var pg = new PathGeometry();
                            var pf = new PathFigure
                            {
                                IsClosed = false,
                                StartPoint = radialGauge.ScalePoint(radialGauge.NormalizedMinAngle, middleOfScale)
                            };
                            var seg = new ArcSegment
                            {
                                SweepDirection = SweepDirection.Clockwise,
                                IsLargeArc = radialGauge.ValueAngle > (180 + radialGauge.NormalizedMinAngle),
                                Size = new Size(middleOfScale, middleOfScale),
                                Point = radialGauge.ScalePoint(Math.Min(radialGauge.ValueAngle, radialGauge.NormalizedMaxAngle), middleOfScale)  // On overflow, stop trail at MaxAngle.
                            };
                            pf.Segments.Add(seg);
                            pg.Figures.Add(pf);
                            trail.Data = pg;
                        }
                    }
                    else
                    {
                        trail.Visibility = Visibility.Collapsed;
                    }

                }

                // Value Text
                if (valueText != null)
                {
                    valueText.Text = radialGauge.Value.ToString(radialGauge.ValueStringFormat);
                }
                ToolTipService.SetToolTip(radialGauge, radialGauge.Value);
            }
        }
    }

    private static void OnInteractivityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        OnInteractivityChanged(d);
    }

    private static void OnInteractivityChanged(DependencyObject d)
    {
        RadialGauge radialGauge = (RadialGauge)d;

        if (radialGauge.IsInteractive)
        {
            radialGauge.Tapped += radialGauge.RadialGauge_Tapped;
            radialGauge.ManipulationDelta += radialGauge.RadialGauge_ManipulationDelta;
            radialGauge.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
        }
        else
        {
            radialGauge.Tapped -= radialGauge.RadialGauge_Tapped;
            radialGauge.ManipulationDelta -= radialGauge.RadialGauge_ManipulationDelta;
            radialGauge.ManipulationMode = ManipulationModes.None;
        }
    }

    private static void OnScaleChanged(DependencyObject d)
    {
        RadialGauge radialGauge = (RadialGauge)d;

        radialGauge.UpdateNormalizedAngles();

        if (radialGauge.GetTemplateChild(ScalePartName) is Path scale)
        { 
            if (radialGauge.NormalizedMaxAngle - radialGauge.NormalizedMinAngle == 360)
            {
                // Draw full circle.
                var eg = new EllipseGeometry
                {
                    Center = new Point(100, 100),
                    RadiusX = 100 - radialGauge.ScalePadding - (radialGauge.ScaleWidth / 2)
                };
                eg.RadiusY = eg.RadiusX;
                scale.Data = eg;
            }
            else
            {
                // Draw arc.
                var pg = new PathGeometry();
                var pf = new PathFigure
                {
                    IsClosed = false
                };
                var middleOfScale = 100 - radialGauge.ScalePadding - (radialGauge.ScaleWidth / 2);
                pf.StartPoint = radialGauge.ScalePoint(radialGauge.NormalizedMinAngle, middleOfScale);
                var seg = new ArcSegment
                {
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = radialGauge.NormalizedMaxAngle > (radialGauge.NormalizedMinAngle + 180),
                    Size = new Size(middleOfScale, middleOfScale),
                    Point = radialGauge.ScalePoint(radialGauge.NormalizedMaxAngle, middleOfScale)
                };
                pf.Segments.Add(seg);
                pg.Figures.Add(pf);
                scale.Data = pg;
            }

            if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode)
            {
                OnFaceChanged(radialGauge);
            }
        }
    }

    private static void OnFaceChanged(DependencyObject d)
    {
        RadialGauge radialGauge = (RadialGauge)d;

        var container = radialGauge.GetTemplateChild(ContainerPartName) as Grid;

        if (container == null || DesignTimeHelpers.IsRunningInLegacyDesignerMode)
        {
            // Bad template.
            return;
        }

        // TO DO: Replace with _radialGauge._root = container.GetVisual();
        var hostVisual = ElementCompositionPreview.GetElementVisual(container);
        var root = hostVisual.Compositor.CreateContainerVisual();
        ElementCompositionPreview.SetElementChildVisual(container, root);
        radialGauge._root = root;
        //

        radialGauge._root.Children.RemoveAll();
        radialGauge._compositor = radialGauge._root.Compositor;

        if (radialGauge.TickSpacing > 0)
        {
            // Ticks.
            var tick = radialGauge._compositor.CreateShapeVisual();
            tick.Size = new Vector2((float)(radialGauge.Height), (float)(radialGauge.Width));
            tick.BorderMode = CompositionBorderMode.Soft;
            tick.Opacity = (float)radialGauge.TickBrush.Opacity;

            var roundedTickRectangle = radialGauge._compositor.CreateRoundedRectangleGeometry();
            roundedTickRectangle.Size = new Vector2((float)radialGauge.TickWidth, (float)radialGauge.TickLength);
            roundedTickRectangle.CornerRadius = new Vector2((float)radialGauge.TickCornerRadius, (float)radialGauge.TickCornerRadius);

            for (double i = radialGauge.Minimum; i <= radialGauge.Maximum; i += radialGauge.TickSpacing)
            {
                var tickSpriteShape = radialGauge._compositor.CreateSpriteShape(roundedTickRectangle);
                tickSpriteShape.FillBrush = radialGauge._compositor.CreateColorBrush(radialGauge.TickBrush.Color);
                tickSpriteShape.Offset = new Vector2(100 - ((float)radialGauge.TickWidth / 2), (float)radialGauge.TickPadding);
                tickSpriteShape.CenterPoint = new Vector2((float)radialGauge.TickWidth / 2, 100 - (float)radialGauge.TickPadding);
                tickSpriteShape.RotationAngleInDegrees = (float)radialGauge.ValueToAngle(i);
                tick.Shapes.Add(tickSpriteShape);
            }

            radialGauge._root.Children.InsertAtTop(tick);

            // Scale Ticks.
            var scaleTick = radialGauge._compositor.CreateShapeVisual();
            scaleTick.Size = new Vector2((float)(radialGauge.Height), (float)(radialGauge.Width));
            scaleTick.BorderMode = CompositionBorderMode.Soft;
            scaleTick.Opacity = (float)radialGauge.ScaleTickBrush.Opacity;

            var roundedScaleTickRectangle = radialGauge._compositor.CreateRoundedRectangleGeometry();
            roundedScaleTickRectangle.Size = new Vector2((float)radialGauge.ScaleTickWidth, (float)radialGauge.ScaleTickLength);
            roundedScaleTickRectangle.CornerRadius = new Vector2((float)radialGauge.ScaleTickCornerRadius, (float)radialGauge.ScaleTickCornerRadius);

            for (double i = radialGauge.Minimum; i <= radialGauge.Maximum; i += radialGauge.TickSpacing)
            {
                var scaleTickSpriteShape = radialGauge._compositor.CreateSpriteShape(roundedScaleTickRectangle);
                scaleTickSpriteShape.FillBrush = radialGauge._compositor.CreateColorBrush(radialGauge.ScaleTickBrush.Color);
                scaleTickSpriteShape.Offset = new Vector2(100 - ((float)radialGauge.ScaleTickWidth / 2), (float)radialGauge.ScalePadding);
                scaleTickSpriteShape.CenterPoint = new Vector2((float)radialGauge.ScaleTickWidth / 2, 100 - (float)radialGauge.ScalePadding);
                scaleTickSpriteShape.RotationAngleInDegrees = (float)radialGauge.ValueToAngle(i);
                scaleTick.Shapes.Add(scaleTickSpriteShape);
            }
            radialGauge._root.Children.InsertAtTop(scaleTick);
        }

        // Needle.
        var shapeVisual = radialGauge._compositor.CreateShapeVisual();
        shapeVisual.Size = new Vector2((float)radialGauge.Height, (float)radialGauge.Width);
        shapeVisual.BorderMode = CompositionBorderMode.Soft;
        shapeVisual.Opacity = (float)radialGauge.NeedleBrush.Opacity;
        var roundedNeedleRectangle = radialGauge._compositor.CreateRoundedRectangleGeometry();
        roundedNeedleRectangle.Size = new Vector2((float)radialGauge.NeedleWidth, (float)radialGauge.NeedleLength);
        roundedNeedleRectangle.CornerRadius = new Vector2((float)radialGauge.NeedleWidth / 2, (float)radialGauge.NeedleWidth / 2);
        radialGauge._needle = radialGauge._compositor.CreateSpriteShape(roundedNeedleRectangle);
        radialGauge._needle.FillBrush = radialGauge._compositor.CreateColorBrush(radialGauge.NeedleBrush.Color);
        radialGauge._needle.CenterPoint = new Vector2((float)radialGauge.NeedleWidth / 2, (float)radialGauge.NeedleLength);
        radialGauge._needle.Offset = new Vector2(100 - ((float)radialGauge.NeedleWidth / 2), 100 - (float)radialGauge.NeedleLength);
        radialGauge._needle.StrokeThickness = (float)radialGauge.NeedleBorderThickness;
        radialGauge._needle.StrokeBrush = radialGauge._compositor.CreateColorBrush(radialGauge.NeedleBorderBrush.Color);
        shapeVisual.Shapes.Add(radialGauge._needle);
        
        radialGauge._root.Children.InsertAtTop(shapeVisual);

        OnValueChanged(radialGauge);
    }

    private void OnColorsChanged()
    {
        if (ThemeListener.IsHighContrast)
        {
            // Apply High Contrast Theme.
            ClearBrush(_needleBrush, NeedleBrushProperty);
            ClearBrush(_needleBorderBrush, NeedleBorderBrushProperty);
            ClearBrush(_trailBrush, TrailBrushProperty);
            ClearBrush(_scaleBrush, ScaleBrushProperty);
            ClearBrush(_scaleTickBrush, ScaleTickBrushProperty);
            ClearBrush(_tickBrush, TickBrushProperty);
            ClearBrush(_foreground, ForegroundProperty);
        }
        else
        {
            // Apply User Defined or Default Theme.
            RestoreBrush(_needleBrush, NeedleBrushProperty);
            RestoreBrush(_needleBorderBrush, NeedleBorderBrushProperty);
            RestoreBrush(_trailBrush, TrailBrushProperty);
            RestoreBrush(_scaleBrush, ScaleBrushProperty);
            RestoreBrush(_scaleTickBrush, ScaleTickBrushProperty);
            RestoreBrush(_tickBrush, TickBrushProperty);
            RestoreBrush(_foreground, ForegroundProperty);
        }

        OnScaleChanged(this);
    }

    private void OnEnabledChanged()
    {
        VisualStateManager.GoToState(this, IsEnabled ? NormalState : DisabledState, true);
        // OnColorsChanged();
    }

    private static void OnUnitChanged(DependencyObject d)
    {
        RadialGauge radialGauge = (RadialGauge)d;
        if (radialGauge.GetTemplateChild(UnitTextPartName) is TextBlock unitTextBlock)
        {
            if (string.IsNullOrEmpty(radialGauge.Unit))
            {
                unitTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                unitTextBlock.Visibility = Visibility.Visible;
            }
        }
    }

    private void ClearBrush(Brush? brush, DependencyProperty prop)
    {
        if (brush != null)
        {
            ClearValue(prop);
        }
    }

    private void RestoreBrush(Brush? source, DependencyProperty prop)
    {
        if (source != null)
        {
            SetValue(prop, source);
        }
    }

    private void UpdateNormalizedAngles()
    {
        var result = Mod(MinAngle, 360);

        if (result >= 180)
        {
            result = result - 360;
        }

        _normalizedMinAngle = result;

        result = Mod(MaxAngle, 360);

        if (result < 180)
        {
            result = result + 360;
        }

        if (result > NormalizedMinAngle + 360)
        {
            result = result - 360;
        }

        _normalizedMaxAngle = result;
    }

    private void SetGaugeValueFromPoint(Point p)
    {
        var pt = new Point(p.X - (ActualWidth / 2), -p.Y + (ActualHeight / 2));

        var angle = Math.Atan2(pt.X, pt.Y) / Degrees2Radians;
        var divider = Mod(NormalizedMaxAngle - NormalizedMinAngle, 360);
        if (divider == 0)
        {
            divider = 360;
        }

        var value = Minimum + ((Maximum - Minimum) * Mod(angle - NormalizedMinAngle, 360) / divider);
        if (value < Minimum || value > Maximum)
        {
            // Ignore positions outside the scale angle.
            return;
        }

        Value = RoundToMultiple(value, StepSize);
    }

    private Point ScalePoint(double angle, double middleOfScale)
    {
        return new Point(100 + (Math.Sin(Degrees2Radians * angle) * middleOfScale), 100 - (Math.Cos(Degrees2Radians * angle) * middleOfScale));
    }

    private double ValueToAngle(double value)
    {
        // Off-scale on the left.
        if (value < Minimum)
        {
            return MinAngle;
        }

        // Off-scale on the right.
        if (value > Maximum)
        {
            return MaxAngle;
        }

        return ((value - Minimum) / (Maximum - Minimum) * (NormalizedMaxAngle - NormalizedMinAngle)) + NormalizedMinAngle;
    }

    private double Mod(double number, double divider)
    {
        var result = number % divider;
        result = result < 0 ? result + divider : result;
        return result;
    }

    private double RoundToMultiple(double number, double multiple)
    {
        double modulo = number % multiple;
        if (double.IsNaN(modulo))
        {
            return number;
        }

        if ((multiple - modulo) <= modulo)
        {
            modulo = multiple - modulo;
        }
        else
        {
            modulo *= -1;
        }

        return number + modulo;
    }
}
