// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Tests;
using CommunityToolkit.Tooling.TestGen;

namespace ExtensionsComponent.Tests;

[TestClass]
public partial class FrameworkElementExtensionsTests : VisualUITestBase
{
    [TestCategory("FrameworkElementExtension")]
    [UIThreadTestMethod]
    public void FrameworkElementExtension_RelativeAncestor_InDataTemplate(FrameworkElementRelativeAncestorDataTemplateTestPage page)
    {
        var list = page.FindDescendant<ListView>();

        Assert.IsNotNull(list, "Couldn't find listview");

        int count = 0;
        foreach (var item in list.FindDescendants().OfType<TextBlock>())
        {
            count++;
            Assert.AreEqual("Hello", item.Text, "Text didn't match binding of ancestor tag property");
        }

        Assert.AreEqual(3, count, "Didn't find three textblocks");
    }
}
