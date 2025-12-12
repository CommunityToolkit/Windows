// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Converters;

namespace ConvertersTests;

[TestClass]
public class Test_StringFormatConverter
{
    private static readonly object NotEmptyString = "Hello, world";
    private static readonly DateTime Date = DateTime.Now;
    
    [TestCategory("Converters")]
    [TestMethod]
    [DataRow(null)]
    [DataRow("en-us")]
    public void WhenValueIsNull_ThenReturnNull(string language)
    {
        var converter = new StringFormatConverter();
        var result = converter.Convert(null!, typeof(string), null!, language);

        Assert.IsNull(result);
    }

    [TestCategory("Converters")]
    [TestMethod]
    [DataRow(null)]
    [DataRow("en-us")]
    public void WhenValueExistsAndParameterIsNull_ThenReturnValue(string language)
    {
        var converter = new StringFormatConverter();
        var result = converter.Convert(NotEmptyString, typeof(string), null!, language);

        Assert.AreEqual(NotEmptyString, result);
    }

    [TestCategory("Converters")]
    [TestMethod]
    [DataRow(null)]
    [DataRow("en-us")]
    public void WhenParameterIsInvalidFormat_ThenReturnValue(string language)
    {
        var converter = new StringFormatConverter();
        var result = converter.Convert(Date, typeof(string), "{1:}", language);

        Assert.AreEqual(Date, result);
    }

    [TestCategory("Converters")]
    [TestMethod]
    [DataRow(null)]
    [DataRow("en-us")]
    public void WhenParameterIsNotAString_ThenReturnValue(string language)
    {
        var converter = new StringFormatConverter();
        var result = converter.Convert(NotEmptyString, typeof(string), 172, language);

        Assert.AreEqual(NotEmptyString, result);
    }
}
