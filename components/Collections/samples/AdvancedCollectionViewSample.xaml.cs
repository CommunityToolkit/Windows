// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CollectionsExperiment.Samples;

[ToolkitSample(id: nameof(AdvancedCollectionViewSample), "AdvancedCollectionView", description: $"A sample for showing how to create and use a {nameof(AdvancedCollectionView)} for sorting and filtering.")]
public sealed partial class AdvancedCollectionViewSample : Page
{
    public ObservableCollection<Person> Original { get; private set; }

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
        Original = new ObservableCollection<Person>
            {
                new Person { Name = "Staff" },
                new Person { Name = "42" },
                new Person { Name = "Swan" },
                new Person { Name = "Orchid" },
                new Person { Name = "15" },
                new Person { Name = "Flame" },
                new Person { Name = "16" },
                new Person { Name = "Arrow" },
                new Person { Name = "Tempest" },
                new Person { Name = "23" },
                new Person { Name = "Pearl" },
                new Person { Name = "Hydra" },
                new Person { Name = "Lamp Post" },
                new Person { Name = "4" },
                new Person { Name = "Looking Glass" },
                new Person { Name = "8" },
            };

        // right list
        var acv = new AdvancedCollectionView(Original);
        int nul;
        acv.Filter = x => !int.TryParse(((Person)x).Name, out nul);
        acv.SortDescriptions.Add(new SortDescription("Name", SortDirection.Ascending));

        CollectionView = acv;
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NewItemBox.Text))
        {
            Original.Insert(0, new Person { Name = NewItemBox.Text });
            NewItemBox.Text = "";
        }
    }

    /// <summary>
    /// A sample class used to show how to use the <see cref="IIncrementalSource{TSource}"/> interface.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        public string? Name { get; set; }
    }
}
