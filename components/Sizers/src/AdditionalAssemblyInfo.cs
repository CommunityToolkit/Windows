// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

// These `InternalsVisibleTo` calls are intended to make it easier for
// for any internal code to be testable in all the different test projects
// used with the Labs infrastructure.
[assembly: InternalsVisibleTo("SizerBase.Tests.Uwp")]
[assembly: InternalsVisibleTo("SizerBase.Tests.WinAppSdk")]
[assembly: InternalsVisibleTo("CommunityToolkit.Tests.Uwp")]
[assembly: InternalsVisibleTo("CommunityToolkit.Tests.WinAppSdk")]
