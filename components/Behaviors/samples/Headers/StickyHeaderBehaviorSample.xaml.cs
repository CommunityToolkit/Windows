// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Behaviors;

namespace BehaviorsExperiment.Samples.Headers;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
[ToolkitSample(id: nameof(StickyHeaderBehaviorSample), nameof(StickyHeaderBehavior), description: $"A sample for showing how to use the {nameof(StickyHeaderBehavior)}.")]
public sealed partial class StickyHeaderBehaviorSample : Page
{
    public ObservableCollection<ExplorerItem> Items { get; }

    public StickyHeaderBehaviorSample()
    {
        this.InitializeComponent();
        Items = GetData();
    }

    private ObservableCollection<ExplorerItem> GetData()
    {
        var list = new ObservableCollection<ExplorerItem>();
        ExplorerItem folder1 = new ExplorerItem()
        {
            Name = "Work Documents",
            Children =
                {
                    new ExplorerItem()
                    {
                        Name = "Functional Specifications",
                        Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "TreeView spec",
                              }
                        }
                    },
                    new ExplorerItem()
                    {
                        Name = "Feature Schedule",
                    },
                    new ExplorerItem()
                    {
                        Name = "Overall Project Plan",
                    },
                    new ExplorerItem()
                    {
                        Name = "Feature Resources Allocation",
                    }
                }
        };
        ExplorerItem folder2 = new ExplorerItem()
        {
            Name = "Personal Folder",
            Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "Home Remodel Folder",
                                Children =
                                {
                                    new ExplorerItem()
                                    {
                                        Name = "Contractor Contact Info",
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Paint Color Scheme",
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Flooring Woodgrain type",
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Kitchen Cabinet Style",
                                    }
                                }
                            }
                        }
        };

        list.Add(folder1);
        list.Add(folder2);

        for (int i = 0; i < 40; i++)
        {
            list.Add(new() { Name = $"Folder {i + 1}" });
        }

        return list;
    }
}

public class ExplorerItem
{
    public string? Name { get; set; }
    private ObservableCollection<ExplorerItem>? _children;
    public ObservableCollection<ExplorerItem> Children
    {
        get
        {
            if (_children == null)
            {
                _children = new ObservableCollection<ExplorerItem>();
            }
            return _children;
        }
        set
        {
            _children = value;
        }
    }
}
