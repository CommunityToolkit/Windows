// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
#if !WINAPPSDK
using Windows.UI.Xaml.Shapes;
#else
using Microsoft.UI.Xaml.Shapes;
#endif
namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// RangeSelector is a "double slider" control for range values.
/// </summary>
[TemplateVisualState(Name = NormalState, GroupName = CommonStates)]
[TemplateVisualState(Name = PointerOverState, GroupName = CommonStates)]
[TemplateVisualState(Name = MinPressedState, GroupName = CommonStates)]
[TemplateVisualState(Name = MaxPressedState, GroupName = CommonStates)]
[TemplateVisualState(Name = DisabledState, GroupName = CommonStates)]
[TemplateVisualState(Name = HorizontalState, GroupName = OrientationStates)]
[TemplateVisualState(Name = VerticalState, GroupName = OrientationStates)]
[TemplatePart(Name = "OutOfRangeContentContainer", Type = typeof(Border))]
[TemplatePart(Name = "ActiveRectangle", Type = typeof(Rectangle))]
[TemplatePart(Name = "MinThumb", Type = typeof(Thumb))]
[TemplatePart(Name = "MaxThumb", Type = typeof(Thumb))]
[TemplatePart(Name = "ContainerCanvas", Type = typeof(Canvas))]
[TemplatePart(Name = "ControlGrid", Type = typeof(Grid))]
[TemplatePart(Name = "ToolTip", Type = typeof(Grid))]
[TemplatePart(Name = "ToolTipText", Type = typeof(TextBlock))]

public partial class RangeSelector : Control
{
    internal const string CommonStates = "CommonStates";
    internal const string NormalState = "Normal";
    internal const string PointerOverState = "PointerOver";
    internal const string DisabledState = "Disabled";
    internal const string MinPressedState = "MinPressed";
    internal const string MaxPressedState = "MaxPressed";
    internal const string OrientationStates = "OrientationStates";
    internal const string HorizontalState = "Horizontal";
    internal const string VerticalState = "Vertical";

    private const double Epsilon = 0.01;
    private const double DefaultMinimum = 0.0;
    private const double DefaultMaximum = 10.0;
    private const double DefaultStepFrequency = 1;
    private static readonly TimeSpan TimeToHideToolTipOnKeyUp = TimeSpan.FromSeconds(1);

    private Rectangle? _activeRectangle;
    private Thumb? _minThumb;
    private Thumb? _maxThumb;
    private Canvas? _containerCanvas;
    private double _oldValue;
    private bool _valuesAssigned;
    private bool _minSet;
    private bool _maxSet;
    private bool _pointerManipulatingMin;
    private bool _pointerManipulatingMax;
    private double _absolutePosition;
    private Grid? _toolTip;
    private TextBlock? _toolTipText;

    /// <summary>
    /// Initializes a new instance of the <see cref="RangeSelector"/> class.
    /// Create a default range selector control.
    /// </summary>
    public RangeSelector()
    {
        DefaultStyleKey = typeof(RangeSelector);
    }

    /// <summary>
    /// Update the visual state of the control when its template is changed.
    /// </summary>
    protected override void OnApplyTemplate()
    {
        if (_minThumb != null)
        {
            _minThumb.DragCompleted -= Thumb_DragCompleted;
            _minThumb.DragDelta -= MinThumb_DragDelta;
            _minThumb.DragStarted -= MinThumb_DragStarted;
            _minThumb.KeyDown -= MinThumb_KeyDown;
        }

        if (_maxThumb != null)
        {
            _maxThumb.DragCompleted -= Thumb_DragCompleted;
            _maxThumb.DragDelta -= MaxThumb_DragDelta;
            _maxThumb.DragStarted -= MaxThumb_DragStarted;
            _maxThumb.KeyDown -= MaxThumb_KeyDown;
        }

        if (_containerCanvas != null)
        {
            _containerCanvas.SizeChanged -= ContainerCanvas_SizeChanged;
            _containerCanvas.PointerPressed -= ContainerCanvas_PointerPressed;
            _containerCanvas.PointerMoved -= ContainerCanvas_PointerMoved;
            _containerCanvas.PointerReleased -= ContainerCanvas_PointerReleased;
            _containerCanvas.PointerExited -= ContainerCanvas_PointerExited;
        }

        IsEnabledChanged -= RangeSelector_IsEnabledChanged;

        // Need to make sure the values can be set in XAML and don't overwrite each other
        VerifyValues();
        _valuesAssigned = true;

        _activeRectangle = GetTemplateChild("ActiveRectangle") as Rectangle;
        _minThumb = GetTemplateChild("MinThumb") as Thumb;
        _maxThumb = GetTemplateChild("MaxThumb") as Thumb;
        _containerCanvas = GetTemplateChild("ContainerCanvas") as Canvas;
        _toolTip = GetTemplateChild("ToolTip") as Grid;
        _toolTipText = GetTemplateChild("ToolTipText") as TextBlock;

        if (_minThumb != null)
        {
            _minThumb.DragCompleted += Thumb_DragCompleted;
            _minThumb.DragDelta += MinThumb_DragDelta;
            _minThumb.DragStarted += MinThumb_DragStarted;
            _minThumb.KeyDown += MinThumb_KeyDown;
            _minThumb.KeyUp += Thumb_KeyUp;
        }

        if (_maxThumb != null)
        {
            _maxThumb.DragCompleted += Thumb_DragCompleted;
            _maxThumb.DragDelta += MaxThumb_DragDelta;
            _maxThumb.DragStarted += MaxThumb_DragStarted;
            _maxThumb.KeyDown += MaxThumb_KeyDown;
            _maxThumb.KeyUp += Thumb_KeyUp;
        }

        if (_containerCanvas != null)
        {
            _containerCanvas.SizeChanged += ContainerCanvas_SizeChanged;
            _containerCanvas.PointerEntered += ContainerCanvas_PointerEntered;
            _containerCanvas.PointerPressed += ContainerCanvas_PointerPressed;
            _containerCanvas.PointerMoved += ContainerCanvas_PointerMoved;
            _containerCanvas.PointerReleased += ContainerCanvas_PointerReleased;
            _containerCanvas.PointerExited += ContainerCanvas_PointerExited;
        }

        VisualStateManager.GoToState(this, IsEnabled ? NormalState : DisabledState, false);
        VisualStateManager.GoToState(this, Orientation == Orientation.Horizontal ? HorizontalState : VerticalState, false);

        IsEnabledChanged += RangeSelector_IsEnabledChanged;

        // Measure our min/max text longest value so we can avoid the length of the scrolling reason shifting in size during use.
        var tb = new TextBlock { Text = Maximum.ToString() };
        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

        // Ensure thumbs and active rectangle are synced after the control is fully loaded
        Loaded += RangeSelector_Loaded;

        base.OnApplyTemplate();
    }

    private void RangeSelector_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= RangeSelector_Loaded;
        SyncThumbs();
    }

    private static void UpdateToolTipText(RangeSelector rangeSelector, TextBlock toolTip, double newValue)
    {
        if (toolTip != null)
        {
            toolTip.Text = string.Format("{0:0.##}", newValue);
        }
    }

    private void ContainerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        SyncThumbs();
    }

    private void VerifyValues()
    {
        if (Minimum > Maximum)
        {
            Minimum = Maximum;
            Maximum = Maximum;
        }

        if (Minimum == Maximum)
        {
            Maximum += Epsilon;
        }

        if (!_maxSet)
        {
            RangeEnd = Maximum;
        }

        if (!_minSet)
        {
            RangeStart = Minimum;
        }

        if (RangeStart < Minimum)
        {
            RangeStart = Minimum;
        }

        if (RangeEnd < Minimum)
        {
            RangeEnd = Minimum;
        }

        if (RangeStart > Maximum)
        {
            RangeStart = Maximum;
        }

        if (RangeEnd > Maximum)
        {
            RangeEnd = Maximum;
        }

        if (RangeEnd < RangeStart)
        {
            RangeStart = RangeEnd;
        }
    }

    private void RangeMinToStepFrequency()
    {
        RangeStart = MoveToStepFrequency(RangeStart);
    }

    private void RangeMaxToStepFrequency()
    {
        RangeEnd = MoveToStepFrequency(RangeEnd);
    }

    private double MoveToStepFrequency(double rangeValue)
    {
        double newValue = Minimum + (((int)Math.Round((rangeValue - Minimum) / StepFrequency)) * StepFrequency);

        if (newValue < Minimum)
        {
            return Minimum;
        }
        else if (newValue > Maximum || Maximum - newValue < StepFrequency)
        {
            return Maximum;
        }
        else
        {
            return newValue;
        }
    }

    private void SyncThumbs(bool fromMinKeyDown = false, bool fromMaxKeyDown = false)
    {
        if (_containerCanvas == null || _minThumb == null || _maxThumb == null)
        {
            return;
        }

        var relativeStart = ((RangeStart - Minimum) / (Maximum - Minimum)) * DragWidth();
        var relativeEnd = ((RangeEnd - Minimum) / (Maximum - Minimum)) * DragWidth();
        var isHorizontal = Orientation == Orientation.Horizontal;

        if (isHorizontal)
        {
            Canvas.SetLeft(_minThumb, relativeStart);
            Canvas.SetLeft(_maxThumb, relativeEnd);
        }
        else
        {
            // Vertical: invert positions so min is at bottom, max is at top
            Canvas.SetTop(_minThumb, DragWidth() - relativeStart);
            Canvas.SetTop(_maxThumb, DragWidth() - relativeEnd);
        }

        if (fromMinKeyDown || fromMaxKeyDown)
        {
            var thumb = fromMinKeyDown ? _minThumb : _maxThumb;
            var position = fromMinKeyDown ? relativeStart : relativeEnd;

            if (isHorizontal)
            {
                var min = fromMinKeyDown ? 0 : Canvas.GetLeft(_minThumb);
                var max = fromMinKeyDown ? Canvas.GetLeft(_maxThumb) : DragWidth();
                DragThumb(thumb, min, max, position);
            }
            else
            {
                var invertedPosition = DragWidth() - position;
                var min = fromMinKeyDown ? Canvas.GetTop(_maxThumb) : 0;
                var max = fromMinKeyDown ? DragWidth() : Canvas.GetTop(_minThumb);
                DragThumbVertical(thumb, min, max, invertedPosition);
            }

            if (_toolTipText != null)
            {
                UpdateToolTipText(this, _toolTipText, fromMinKeyDown ? RangeStart : RangeEnd);
            }
        }

        SyncActiveRectangle();
    }

    private void SyncActiveRectangle()
    {
        if (_containerCanvas == null || _minThumb == null || _maxThumb == null || _activeRectangle == null)
        {
            return;
        }

        if (Orientation == Orientation.Horizontal)
        {
            var relativeLeft = Canvas.GetLeft(_minThumb);
            Canvas.SetLeft(_activeRectangle, relativeLeft);
            Canvas.SetTop(_activeRectangle, (_containerCanvas.ActualHeight - _activeRectangle.ActualHeight) / 2);
            _activeRectangle.Width = Math.Max(0, Canvas.GetLeft(_maxThumb) - relativeLeft);
        }
        else
        {
            var relativeTop = Canvas.GetTop(_maxThumb);
            Canvas.SetTop(_activeRectangle, relativeTop);
            Canvas.SetLeft(_activeRectangle, (_containerCanvas.ActualWidth - _activeRectangle.ActualWidth) / 2);
            _activeRectangle.Height = Math.Max(0, Canvas.GetTop(_minThumb) - relativeTop);
        }
    }

    private void RangeSelector_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        VisualStateManager.GoToState(this, IsEnabled ? NormalState : DisabledState, true);
    }
}
