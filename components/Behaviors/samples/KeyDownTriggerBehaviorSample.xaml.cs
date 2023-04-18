// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Behaviors;

namespace BehaviorsExperiment.Samples;

[ToolkitSample(id: nameof(KeyDownTriggerBehaviorSample), nameof(KeyDownTriggerBehavior), description: $"A sample for showing how to use the {nameof(KeyDownTriggerBehavior)}.")]
public sealed partial class KeyDownTriggerBehaviorSample : Page, INotifyPropertyChanged
{
    public KeyDownTriggerBehaviorSample()
    {
        this.InitializeComponent();
    }

    public int Count { get; set; }

    public void IncrementCount()
    {
        Count++;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
