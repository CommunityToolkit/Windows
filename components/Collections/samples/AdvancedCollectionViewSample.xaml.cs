// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CollectionsExperiment.Samples;

[ToolkitSample(id: nameof(AdvancedCollectionViewSample), "AdvancedCollectionView", description: $"A sample for showing how to create and use a {nameof(AdvancedCollectionView)} for sorting and filtering.")]
public sealed partial class AdvancedCollectionViewSample : Page
{
    public ObservableCollection<Employee> Original { get; private set; }

    public AdvancedCollectionView CollectionView { get; private set; }

    public AdvancedCollectionViewSample()
    {
        this.InitializeComponent();
        Setup();
    }

    [MemberNotNull(nameof(Original))]
    [MemberNotNull(nameof(CollectionView))]
    private void Setup()
    {
        // left list
        Original = new ObservableCollection<Employee>
            {
                new Employee { Name = "Staff" },
                new Employee { Name = "42" },
                new Employee { Name = "Swan" },
                new Employee { Name = "Orchid" },
                new Employee { Name = "15" },
                new Employee { Name = "Flame" },
                new Employee { Name = "16" },
                new Employee { Name = "Arrow" },
                new Employee { Name = "Tempest" },
                new Employee { Name = "23" },
                new Employee { Name = "Pearl" },
                new Employee { Name = "Hydra" },
                new Employee { Name = "Lamp Post" },
                new Employee { Name = "4" },
                new Employee { Name = "Looking Glass" },
                new Employee { Name = "8" },
            };

        // right list
        var acv = new AdvancedCollectionView(Original);
        acv.Filter = x => !int.TryParse(((Employee)x).Name, out _);
        acv.SortDescriptions.Add(new SortDescription("Name", SortDirection.Ascending));

        CollectionView = acv;
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NewItemBox.Text))
        {
            Original.Insert(0, new Employee { Name = NewItemBox.Text });
            NewItemBox.Text = "";
        }
    }
}

/// <summary>
/// A sample class used to show how to use the <see cref="AdvancedCollectionView"/> class.
/// </summary>
public partial class Employee
{
    /// <summary>
    /// Gets or sets the name of the person.
    /// </summary>
    public string? Name { get; set; }
}
