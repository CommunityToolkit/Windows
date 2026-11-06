// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Win32;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace CommunityToolkit.WinUI.HelpersRns;

#if WINAPPSDK
internal class ThemeListenerHelperWindow
{
    public delegate void ThemeChangedHandler(ApplicationTheme theme);

    public static ThemeListenerHelperWindow Instance = new();

    public event ThemeChangedHandler? ThemeChanged;

    private static string s_className = "ThemeListenerHelperWindow";

    private HWND m_hwnd;

    private ThemeListenerHelperWindow()
    {
        s_className = "ThemeListenerHelperWindow";
        RegisterClass();
        m_hwnd = CreateWindow();
    }

    private static unsafe void RegisterClass()
    {
        WNDCLASSEXW wcx = default;
        wcx.cbSize = (uint)sizeof(WNDCLASSEXW);
        fixed (char* pClassName = s_className)
        {
            wcx.lpszClassName = pClassName;
            wcx.lpfnWndProc = &WndProc;
            if (PInvoke.RegisterClassEx(in wcx) is 0)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
            }
        }
    }

    private static unsafe HWND CreateWindow()
    {
        HWND hwnd = PInvoke.CreateWindowEx(0, s_className, string.Empty, 0, 0, 0, 0, 0, default, null, null, null);
        if (hwnd == HWND.Null)
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }
        return hwnd;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static LRESULT WndProc(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam)
    {
        if (msg is PInvoke.WM_SETTINGCHANGE)
        {
            // REG_DWORD
            object? value = Registry.GetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                "AppsUseLightTheme",
                true);

            if (value is int dword)
            {
                Instance.ThemeChanged?.Invoke(dword is 1 ? ApplicationTheme.Light : ApplicationTheme.Dark);
            }
        }

        return PInvoke.DefWindowProc(hWnd, msg, wParam, lParam);
    }
}
#endif
