// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;

namespace CommunityToolkit.WinUI.Controls;

internal class WrapLayoutState
{
    private readonly List<WrapItem> _items = new();
    private readonly VirtualizingLayoutContext _context;

    public WrapLayoutState(VirtualizingLayoutContext context)
    {
        _context = context;
    }

    public Orientation Orientation { get; private set; }

    public UvMeasure Spacing { get; internal set; }

    public double AvailableU { get; internal set; }

    internal WrapItem GetItemAt(int index)
    {
        if (index < 0)
        {
            throw new IndexOutOfRangeException();
        }

        if (index <= (_items.Count - 1))
        {
            return _items[index];
        }
        else
        {
            var item = new WrapItem(index);
            _items.Add(item);
            return item;
        }
    }

    internal void Clear()
    {
        _items.Clear();
    }

    internal void RemoveFromIndex(int index)
    {
        if (index >= _items.Count)
        {
            // Item was added/removed but we haven't realized that far yet
            return;
        }

        var numToRemove = _items.Count - index;
        _items.RemoveRange(index, numToRemove);
    }

    internal void SetOrientation(Orientation orientation)
    {
        foreach (var item in _items.Where(i => i.Measure.HasValue))
        {
            var measure = item.Measure!.Value;
            (measure.V, measure.U) = (measure.U, measure.V);
            item.Measure = measure;
            item.Position = null;
        }

        Orientation = orientation;
        AvailableU = 0;
    }

    internal void ClearPositions()
    {
        foreach (var item in _items)
        {
            item.Position = null;
        }
    }

    internal double GetHeight()
    {
        if (_items.Count is 0)
        {
            return 0;
        }

        UvMeasure? lastPosition = null;
        double maxV = 0;

        for (var i = _items.Count - 1; i >= 0; --i)
        {
            var item = _items[i];

            if (item.Position is null || item.Measure is null)
            {
                continue;
            }

            if (lastPosition is not null && lastPosition.Value.V > item.Position.Value.V)
            {
                // This is a row above the last item.
                break;
            }

            lastPosition = item.Position;
            maxV = Math.Max(maxV, item.Measure.Value.V);
        }

        return lastPosition?.V + maxV ?? 0;
    }

    internal void RecycleElementAt(int index)
    {
        var element = _context.GetOrCreateElementAt(index);
        _context.RecycleElement(element);
    }
}
