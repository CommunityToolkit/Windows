// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using VirtualKey = Windows.System.VirtualKey;
using VirtualKeyModifiers = Windows.System.VirtualKeyModifiers;

namespace CommunityToolkit.WinUI.Controls;
public partial class RadialGauge : RangeBase
{
    private void RadialGauge_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        SetGaugeValueFromPoint(e.Position);
    }

    private void RadialGauge_Tapped(object sender, TappedRoutedEventArgs e)
    {
        SetGaugeValueFromPoint(e.GetPosition(this));
    }

    private void RadialGauge_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (IsInteractive)
        {
            e.Handled = true;
        }
    }

    private void SetKeyboardAccelerators()
    {
        // Small step
        AddKeyboardAccelerator(VirtualKeyModifiers.None, VirtualKey.Left, static (_, kaea) =>
        {
            if (kaea.Element is RadialGauge gauge)
            {
                gauge.Value = Math.Max(gauge.Minimum, gauge.Value - Math.Max(gauge.StepSize, gauge.SmallChange));
                kaea.Handled = true;
            }
        });

        AddKeyboardAccelerator(VirtualKeyModifiers.None, VirtualKey.Up, static (_, kaea) =>
        {
            if (kaea.Element is RadialGauge gauge)
            {
                gauge.Value = Math.Min(gauge.Maximum, gauge.Value + Math.Max(gauge.StepSize, gauge.SmallChange));
                kaea.Handled = true;
            }
        });

        AddKeyboardAccelerator(VirtualKeyModifiers.None, VirtualKey.Right, static (_, kaea) =>
        {
            if (kaea.Element is RadialGauge gauge)
            {
                gauge.Value = Math.Min(gauge.Maximum, gauge.Value + Math.Max(gauge.StepSize, gauge.SmallChange));
                kaea.Handled = true;
            }
        });

        AddKeyboardAccelerator(VirtualKeyModifiers.None, VirtualKey.Down, static (_, kaea) =>
        {
            if (kaea.Element is RadialGauge gauge)
            {
                gauge.Value = Math.Max(gauge.Minimum, gauge.Value - Math.Max(gauge.StepSize, gauge.SmallChange));
                kaea.Handled = true;
            }
        });

        // Large step
        AddKeyboardAccelerator(VirtualKeyModifiers.Control, VirtualKey.Left, static (_, kaea) =>
        {
            if (kaea.Element is RadialGauge gauge)
            {
                gauge.Value = Math.Max(gauge.Minimum, gauge.Value - Math.Max(gauge.StepSize, gauge.LargeChange));
                kaea.Handled = true;
            }
        });

        AddKeyboardAccelerator(VirtualKeyModifiers.Control, VirtualKey.Up, static (_, kaea) =>
        {
            if (kaea.Element is RadialGauge gauge)
            {
                gauge.Value = Math.Min(gauge.Maximum, gauge.Value + Math.Max(gauge.StepSize, gauge.LargeChange));
                kaea.Handled = true;
            }
        });

        AddKeyboardAccelerator(VirtualKeyModifiers.Control, VirtualKey.Right, static (_, kaea) =>
        {
            if (kaea.Element is RadialGauge gauge)
            {
                gauge.Value = Math.Min(gauge.Maximum, gauge.Value + Math.Max(gauge.StepSize, gauge.LargeChange));
                kaea.Handled = true;
            }
        });

        AddKeyboardAccelerator(VirtualKeyModifiers.Control, VirtualKey.Down, static (_, kaea) =>
        {
            if (kaea.Element is RadialGauge gauge)
            {
                gauge.Value = Math.Max(gauge.Minimum, gauge.Value - Math.Max(gauge.StepSize, gauge.LargeChange));
                kaea.Handled = true;
            }
        });
    }

    private void AddKeyboardAccelerator(
        VirtualKeyModifiers keyModifiers,
        VirtualKey key,
        TypedEventHandler<KeyboardAccelerator, KeyboardAcceleratorInvokedEventArgs> handler)
    {
        var accelerator = new KeyboardAccelerator()
        {
            Modifiers = keyModifiers,
            Key = key
        };
        accelerator.Invoked += handler;
        KeyboardAccelerators.Add(accelerator);
    }
}
