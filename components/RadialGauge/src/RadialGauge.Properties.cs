// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Helpers;

namespace CommunityToolkit.WinUI.Controls;
public partial class RadialGauge : RangeBase
{
    /// <summary>
    /// Identifies the <see cref="IsInteractive"/> property.
    /// </summary>
    public static readonly DependencyProperty IsInteractiveProperty =
        DependencyProperty.Register(nameof(IsInteractive), typeof(bool), typeof(RadialGauge), new PropertyMetadata(true, OnInteractivityChanged));

    /// <summary>
    /// Identifies the ScaleWidth dependency property.
    /// </summary>
    public static readonly DependencyProperty ScaleWidthProperty =
        DependencyProperty.Register(nameof(ScaleWidth), typeof(double), typeof(RadialGauge), new PropertyMetadata(12.0, OnScaleChanged));

    /// <summary>
    /// Identifies the optional StepSize property.
    /// </summary>
    public static readonly DependencyProperty StepSizeProperty =
        DependencyProperty.Register(nameof(StepSize), typeof(double), typeof(RadialGauge), new PropertyMetadata(0.0));

    /// <summary>
    /// Identifies the NeedleBrush dependency property.
    /// </summary>
    public static readonly DependencyProperty NeedleBrushProperty =
        DependencyProperty.Register(nameof(NeedleBrush), typeof(SolidColorBrush), typeof(RadialGauge), new PropertyMetadata(null, OnFaceChanged));

    /// <summary>
    /// Identifies the NeedleBrush dependency property.
    /// </summary>
    public static readonly DependencyProperty NeedleBorderBrushProperty =
        DependencyProperty.Register(nameof(NeedleBorderBrush), typeof(SolidColorBrush), typeof(RadialGauge), new PropertyMetadata(null, OnFaceChanged));

    /// <summary>
    /// Identifies the Unit dependency property.
    /// </summary>
    public static readonly DependencyProperty UnitProperty =
        DependencyProperty.Register(nameof(Unit), typeof(string), typeof(RadialGauge), new PropertyMetadata(string.Empty, OnUnitChanged));

    /// <summary>
    /// Identifies the TrailBrush dependency property.
    /// </summary>
    public static readonly DependencyProperty TrailBrushProperty =
        DependencyProperty.Register(nameof(TrailBrush), typeof(Brush), typeof(RadialGauge), new PropertyMetadata(null));

    /// <summary>
    /// Identifies the ScaleBrush dependency property.
    /// </summary>
    public static readonly DependencyProperty ScaleBrushProperty =
        DependencyProperty.Register(nameof(ScaleBrush), typeof(Brush), typeof(RadialGauge), new PropertyMetadata(null));

    /// <summary>
    /// Identifies the ScaleTickBrush dependency property.
    /// </summary>
    public static readonly DependencyProperty ScaleTickBrushProperty =
        DependencyProperty.Register(nameof(ScaleTickBrush), typeof(Brush), typeof(RadialGauge), new PropertyMetadata(null, OnFaceChanged));

    /// <summary>
    /// Identifies the TickBrush dependency property.
    /// </summary>
    public static readonly DependencyProperty TickBrushProperty =
        DependencyProperty.Register(nameof(TickBrush), typeof(SolidColorBrush), typeof(RadialGauge), new PropertyMetadata(null, OnFaceChanged));

    /// <summary>
    /// Identifies the ValueStringFormat dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueStringFormatProperty =
        DependencyProperty.Register(nameof(ValueStringFormat), typeof(string), typeof(RadialGauge), new PropertyMetadata("N0", (s, e) => OnValueChanged(s)));

    /// <summary>
    /// Identifies the NeedleLength dependency property.
    /// </summary>
    public static readonly DependencyProperty NeedleLengthProperty =
        DependencyProperty.Register(nameof(NeedleLength), typeof(double), typeof(RadialGauge), new PropertyMetadata(58d, OnFaceChanged));

    /// <summary>
    /// Identifies the NeedleLength dependency property.
    /// </summary>
    public static readonly DependencyProperty NeedleBorderThicknessProperty =
        DependencyProperty.Register(nameof(NeedleBorderThickness), typeof(double), typeof(RadialGauge), new PropertyMetadata(1d, OnFaceChanged));

    /// <summary>
    /// Identifies the NeedleWidth dependency property.
    /// </summary>
    public static readonly DependencyProperty NeedleWidthProperty =
        DependencyProperty.Register(nameof(NeedleWidth), typeof(double), typeof(RadialGauge), new PropertyMetadata(5d, OnFaceChanged));

    /// <summary>
    /// Identifies the ScalePadding dependency property.
    /// </summary>
    public static readonly DependencyProperty ScalePaddingProperty =
        DependencyProperty.Register(nameof(ScalePadding), typeof(double), typeof(RadialGauge), new PropertyMetadata(0d, OnFaceChanged));

    /// <summary>
    /// Identifies the ScaleTickWidth dependency property.
    /// </summary>
    public static readonly DependencyProperty ScaleTickWidthProperty =
        DependencyProperty.Register(nameof(ScaleTickWidth), typeof(double), typeof(RadialGauge), new PropertyMetadata(0d, OnFaceChanged));

    /// <summary>
    /// Identifies the ScaleTickWidth dependency property.
    /// </summary>
    public static readonly DependencyProperty ScaleTickLengthProperty =
        DependencyProperty.Register(nameof(ScaleTickLength), typeof(double), typeof(RadialGauge), new PropertyMetadata(12d, OnFaceChanged));


    /// <summary>
    /// Identifies the ScaleTickWidth dependency property.
    /// </summary>
    public static readonly DependencyProperty ScaleTickCornerRadiusProperty =
        DependencyProperty.Register(nameof(ScaleTickCornerRadius), typeof(double), typeof(RadialGauge), new PropertyMetadata(2d, OnFaceChanged));

    /// <summary>
    /// Identifies the TickSpacing dependency property.
    /// </summary>
    public static readonly DependencyProperty TickSpacingProperty =
    DependencyProperty.Register(nameof(TickSpacing), typeof(int), typeof(RadialGauge), new PropertyMetadata(15, OnFaceChanged));

    /// <summary>
    /// Identifies the TickWidth dependency property.
    /// </summary>
    public static readonly DependencyProperty TickWidthProperty =
        DependencyProperty.Register(nameof(TickWidth), typeof(double), typeof(RadialGauge), new PropertyMetadata(2d, OnFaceChanged));

    /// <summary>
    /// Identifies the TickLength dependency property.
    /// </summary>
    public static readonly DependencyProperty TickLengthProperty =
        DependencyProperty.Register(nameof(TickLength), typeof(double), typeof(RadialGauge), new PropertyMetadata(6d, OnFaceChanged));

    /// <summary>
    /// Identifies the TickPadding dependency property.
    /// </summary>
    public static readonly DependencyProperty TickPaddingProperty =
        DependencyProperty.Register(nameof(TickPadding), typeof(double), typeof(RadialGauge), new PropertyMetadata(24d, OnFaceChanged));

    /// <summary>
    /// Identifies the TickCornerRadius dependency property.
    /// </summary>
    public static readonly DependencyProperty TickCornerRadiusProperty =
        DependencyProperty.Register(nameof(TickCornerRadius), typeof(double), typeof(RadialGauge), new PropertyMetadata(2d, OnFaceChanged));

    /// <summary>
    /// Identifies the MinAngle dependency property.
    /// </summary>
    public static readonly DependencyProperty MinAngleProperty =
        DependencyProperty.Register(nameof(MinAngle), typeof(int), typeof(RadialGauge), new PropertyMetadata(-150, OnScaleChanged));

    /// <summary>
    /// Identifies the MaxAngle dependency property.
    /// </summary>
    public static readonly DependencyProperty MaxAngleProperty =
        DependencyProperty.Register(nameof(MaxAngle), typeof(int), typeof(RadialGauge), new PropertyMetadata(150, OnScaleChanged));

    /// <summary>
    /// Identifies the ValueAngle dependency property.
    /// </summary>
    protected static readonly DependencyProperty ValueAngleProperty =
        DependencyProperty.Register(nameof(ValueAngle), typeof(double), typeof(RadialGauge), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the rounding interval for the Value.
    /// </summary>
    public double StepSize
    {
        get { return (double)GetValue(StepSizeProperty); }
        set { SetValue(StepSizeProperty, value); }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control accepts setting its value through interaction.
    /// </summary>
    public bool IsInteractive
    {
        get { return (bool)GetValue(IsInteractiveProperty); }
        set { SetValue(IsInteractiveProperty, value); }
    }

    /// <summary>
    /// Gets or sets the width of the scale, in percentage of the gauge radius.
    /// </summary>
    public double ScaleWidth
    {
        get { return (double)GetValue(ScaleWidthProperty); }
        set { SetValue(ScaleWidthProperty, value); }
    }

    /// <summary>
    /// Gets or sets the displayed unit measure.
    /// </summary>
    public string Unit
    {
        get { return (string)GetValue(UnitProperty); }
        set { SetValue(UnitProperty, value); }
    }

    /// <summary>
    /// Gets or sets the needle brush.
    /// </summary>
    public SolidColorBrush NeedleBrush
    {
        get { return (SolidColorBrush)GetValue(NeedleBrushProperty); }
        set { SetValue(NeedleBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets the needle border brush.
    /// </summary>
    public SolidColorBrush NeedleBorderBrush
    {
        get { return (SolidColorBrush)GetValue(NeedleBorderBrushProperty); }
        set { SetValue(NeedleBorderBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets the trail brush.
    /// </summary>
    public Brush TrailBrush
    {
        get { return (Brush)GetValue(TrailBrushProperty); }
        set { SetValue(TrailBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets the scale brush.
    /// </summary>
    public Brush ScaleBrush
    {
        get { return (Brush)GetValue(ScaleBrushProperty); }
        set { SetValue(ScaleBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets the scale tick brush.
    /// </summary>
    public SolidColorBrush ScaleTickBrush
    {
        get { return (SolidColorBrush)GetValue(ScaleTickBrushProperty); }
        set { SetValue(ScaleTickBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets the scale tick cornerradius.
    /// </summary>
    public double ScaleTickCornerRadius
    {
        get { return (double)GetValue(ScaleTickCornerRadiusProperty); }
        set { SetValue(ScaleTickCornerRadiusProperty, value); }
    }

    
    /// <summary>
    /// Gets or sets the outer tick brush.
    /// </summary>
    public SolidColorBrush TickBrush
    {
        get { return (SolidColorBrush)GetValue(TickBrushProperty); }
        set { SetValue(TickBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets the value string format.
    /// </summary>
    public string ValueStringFormat
    {
        get { return (string)GetValue(ValueStringFormatProperty); }
        set { SetValue(ValueStringFormatProperty, value); }
    }

    /// <summary>
    /// Gets or sets the tick spacing, in units. Values of zero or less will be ignored when drawing.
    /// </summary>
    public int TickSpacing
    {
        get { return (int)GetValue(TickSpacingProperty); }
        set { SetValue(TickSpacingProperty, value); }
    }

    /// <summary>
    /// Gets or sets the needle length, in percentage of the gauge radius.
    /// </summary>
    public double NeedleLength
    {
        get { return (double)GetValue(NeedleLengthProperty); }
        set { SetValue(NeedleLengthProperty, value); }
    }


    /// <summary>
    /// Gets or sets the needle length, in percentage of the gauge radius.
    /// </summary>
    public double NeedleBorderThickness
    {
        get { return (double)GetValue(NeedleBorderThicknessProperty); }
        set { SetValue(NeedleBorderThicknessProperty, value); }
    }

    /// <summary>
    /// Gets or sets the needle width, in percentage of the gauge radius.
    /// </summary>
    public double NeedleWidth
    {
        get { return (double)GetValue(NeedleWidthProperty); }
        set { SetValue(NeedleWidthProperty, value); }
    }

    /// <summary>
    /// Gets or sets the distance of the scale from the outside of the control, in percentage of the gauge radius.
    /// </summary>
    public double ScalePadding
    {
        get { return (double)GetValue(ScalePaddingProperty); }
        set { SetValue(ScalePaddingProperty, value); }
    }

    /// <summary>
    /// Gets or sets the distance of the ticks from the outside of the control, in percentage of the gauge radius.
    /// </summary>
    public double TickPadding
    {
        get { return (double)GetValue(TickPaddingProperty); }
        set { SetValue(TickPaddingProperty, value); }
    }

    /// <summary>
    /// Gets or sets the width of the scale ticks, in percentage of the gauge radius.
    /// </summary>
    public double ScaleTickWidth
    {
        get { return (double)GetValue(ScaleTickWidthProperty); }
        set { SetValue(ScaleTickWidthProperty, value); }
    }

    /// <summary>
    /// Gets or sets the length of the ticks, in percentage of the gauge radius.
    /// </summary>
    public double ScaleTickLength
    {
        get { return (double)GetValue(ScaleTickLengthProperty); }
        set { SetValue(ScaleTickLengthProperty, value); }
    }

    /// <summary>
    /// Gets or sets the length of the ticks, in percentage of the gauge radius.
    /// </summary>
    public double TickLength
    {
        get { return (double)GetValue(TickLengthProperty); }
        set { SetValue(TickLengthProperty, value); }
    }

    /// <summary>
    /// Gets or sets the width of the ticks, in percentage of the gauge radius.
    /// </summary>
    public double TickWidth
    {
        get { return (double)GetValue(TickWidthProperty); }
        set { SetValue(TickWidthProperty, value); }
    }

    /// <summary>
    /// Gets or sets the CornerRadius of the ticks, in percentage of the gauge radius.
    /// </summary>
    public double TickCornerRadius
    {
        get { return (double)GetValue(TickCornerRadiusProperty); }
        set { SetValue(TickCornerRadiusProperty, value); }
    }

    /// <summary>
    /// Gets or sets the start angle of the scale, which corresponds with the Minimum value, in degrees.
    /// </summary>
    /// <remarks>Changing MinAngle may require retemplating the control.</remarks>
    public int MinAngle
    {
        get { return (int)GetValue(MinAngleProperty); }
        set { SetValue(MinAngleProperty, value); }
    }

    /// <summary>
    /// Gets or sets the end angle of the scale, which corresponds with the Maximum value, in degrees.
    /// </summary>
    /// <remarks>Changing MaxAngle may require retemplating the control.</remarks>
    public int MaxAngle
    {
        get { return (int)GetValue(MaxAngleProperty); }
        set { SetValue(MaxAngleProperty, value); }
    }

    /// <summary>
    /// Gets or sets the current angle of the needle (between MinAngle and MaxAngle). Setting the angle will update the Value.
    /// </summary>
    protected double ValueAngle
    {
        get { return (double)GetValue(ValueAngleProperty); }
        set { SetValue(ValueAngleProperty, value); }
    }

    private static void OnUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        OnUnitChanged(d);
    }
    private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        OnScaleChanged(d);
    }

    private static void OnFaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!DesignTimeHelpers.IsRunningInLegacyDesignerMode)
        {
            OnFaceChanged(d);
        }
    }
}
