// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Converters;

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// Internal helpers for use between <see cref="SwitchPresenter"/> and <see cref="SwitchConverter"/>.
/// The logic here is the main code which looks across a <see cref="CaseCollection"/>  to match a specific <see cref="Case"/> with a given value while converting types based on the <see cref="SwitchPresenter.TargetType"/> property. This will handle <see cref="Enum"/> values as well as values compatible with the <see cref="XamlBindingHelper.ConvertValue(Type, object)"/> method.
/// </summary>
internal static partial class SwitchHelpers
{
    /// <summary>
    /// Extension method for a set of cases to find the matching case given its value and type.
    /// </summary>
    /// <param name="switchCases">The collection of <see cref="Case"/>s in a <see cref="CaseCollection"/></param>
    /// <param name="value">The value of the <see cref="Case"/> to find</param>
    /// <param name="targetType">The desired type of the result for automatic conversion</param>
    /// <returns>The discovered value, the default value, or <c>null</c></returns>
    internal static Case? EvaluateCases(this CaseCollection switchCases, object value, Type targetType)
    {
        if (switchCases == null ||
            switchCases.Count == 0)
        {
            // If we have no cases, then we can't match anything.
            return null;
        }

        Case? xdefault = null;
        Case? newcase = null;

        foreach (Case xcase in switchCases)
        {
            if (xcase.IsDefault)
            {
                // If there are multiple default cases provided, this will override and just grab the last one, the developer will have to fix this in their XAML. We call this out in the case comments.
                xdefault = xcase;
                continue;
            }

            if (CompareValues(value, xcase.Value, targetType))
            {
                newcase = xcase;
                break;
            }
        }

        if (newcase == null && xdefault != null)
        {
            // Inject default if we found one without matching anything
            newcase = xdefault;
        }

        return newcase;
    }

    /// <summary>
    /// Compares two values using the TargetType.
    /// </summary>
    /// <param name="compare">Our main value in our SwitchPresenter.</param>
    /// <param name="value">The value from the case to compare to.</param>
    /// <param name="targetType">The desired type of the result for automatic conversion.</param>
    /// <returns>true if the two values are equal</returns>
    internal static bool CompareValues(object compare, object value, Type targetType)
    {
        if (compare == null || value == null)
        {
            return compare == value;
        }

        if (targetType == null ||
            (targetType == compare.GetType() &&
             targetType == value.GetType()))
        {
            // Default direct object comparison or we're all the proper type
            return compare.Equals(value);
        }
        else if (compare.GetType() == targetType)
        {
            // If we have a TargetType and the first value is the right type
            // Then our 2nd value isn't, so convert to string and coerce.
            var valueBase2 = ConvertValue(targetType, value);

            return compare.Equals(valueBase2);
        }

        // Neither of our two values matches the type so
        // we'll convert both to a String and try and coerce it to the proper type.
        var compareBase = ConvertValue(targetType, compare);

        var valueBase = ConvertValue(targetType, value);

        return compareBase.Equals(valueBase);
    }

    /// <summary>
    /// Helper method to convert a value from a source type to a target type.
    /// </summary>
    /// <param name="targetType">The target type</param>
    /// <param name="value">The value to convert</param>
    /// <returns>The converted value</returns>
    internal static object ConvertValue(Type targetType, object value)
    {
        if (targetType.IsInstanceOfType(value))
        {
            return value;
        }
        else if (targetType.IsEnum && value is string str)
        {
#if HAS_UNO
            if (Enum.IsDefined(targetType, str))
            {
                return Enum.Parse(targetType, str);
            }
#else
            if (Enum.TryParse(targetType, str, out object? result))
            {
                return result!;
            }
#endif

            static object ThrowExceptionForKeyNotFound()
            {
                throw new InvalidOperationException("The requested enum value was not present in the provided type.");
            }

            return ThrowExceptionForKeyNotFound();
        }
        else
        {
            return XamlBindingHelper.ConvertValue(targetType, value);
        }
    }
}
