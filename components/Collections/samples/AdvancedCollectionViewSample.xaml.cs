// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CollectionsExperiment.Samples;

[ToolkitSample(id: nameof(AdvancedCollectionViewSample), "AdvancedCollectionView", description: $"A sample for showing how to create and use a {nameof(AdvancedCollectionView)} for sorting and filtering.")]
public sealed partial class AdvancedCollectionViewSample : Page
{
    public ObservableCollection<Employee> EmployeeCollection { get; private set; }

    public AdvancedCollectionView CollectionView { get; private set; }

    public AdvancedCollectionViewSample()
    {
        this.InitializeComponent();
        Setup();
    }

    [MemberNotNull(nameof(EmployeeCollection))]
    [MemberNotNull(nameof(CollectionView))]
    private void Setup()
    {
        // left list
        EmployeeCollection = new()
            {
                new() { Name = "Staff" },
                new() { Name = "42" },
                new() { Name = "Swan" },
                new() { Name = "Orchid" },
                new() { Name = "15" },
                new() { Name = "Flame" },
                new() { Name = "16" },
                new() { Name = "Arrow" },
                new() { Name = "Tempest" },
                new() { Name = "23" },
                new() { Name = "Pearl" },
                new() { Name = "Hydra" },
                new() { Name = "Lamp Post" },
                new() { Name = "4" },
                new() { Name = "Looking Glass" },
                new() { Name = "8" },
            };

        // right list
        AdvancedCollectionView acv = new(EmployeeCollection);
        acv.Filter = x => !int.TryParse(((Employee)x).Name, out _);
        acv.SortDescriptions.Add(new(nameof(Employee.Name), SortDirection.Ascending));

        CollectionView = acv;
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NewItemBox.Text))
        {
            EmployeeCollection.Insert(0, new Employee { Name = NewItemBox.Text });
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
