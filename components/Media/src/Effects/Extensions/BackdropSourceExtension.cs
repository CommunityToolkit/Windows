// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using CommunityToolkit.WinUI.Media.Pipelines;

namespace CommunityToolkit.WinUI.Media;

/// <summary>
/// A backdrop effect that can sample from a specified source
/// </summary>
[MarkupExtensionReturnType(ReturnType = typeof(PipelineBuilder))]
public sealed partial class BackdropSourceExtension : MarkupExtension
{
    #if WINUI2
    /// Gets or sets the background source mode for the effect (the default is <see cref="AcrylicBackgroundSource.Backdrop"/>).
    /// </summary>
    public AcrylicBackgroundSource BackgroundSource { get; set; } = AcrylicBackgroundSource.Backdrop;
    #endif

    /// <inheritdoc/>
    protected override object ProvideValue()
    {
#if WINUI3
        return PipelineBuilder.FromBackdrop();
#elif WINUI2
        return BackgroundSource switch
        {
            AcrylicBackgroundSource.Backdrop => PipelineBuilder.FromBackdrop(),
            AcrylicBackgroundSource.HostBackdrop => PipelineBuilder.FromHostBackdrop(),
            _ => throw new ArgumentException($"Invalid source for backdrop effect: {BackgroundSource}")
        };
#endif
    }
}
