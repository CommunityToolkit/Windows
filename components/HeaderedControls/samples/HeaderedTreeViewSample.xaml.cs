// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;

namespace HeaderedControlsExperiment.Samples;

[ToolkitSample(id: nameof(HeaderedTreeViewSample), "HeaderedTreeView", description: $"A sample for showing how to create and use a {nameof(HeaderedTreeView)} control.")]
public sealed partial class HeaderedTreeViewSample : Page
{
    public HeaderedTreeViewSample()
    {
        this.InitializeComponent();
        Items = GetData();
    }
    public ObservableCollection<ExplorerItem> Items { get; }

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
