// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;

namespace CommunityToolkit.WinUI.HelpersRns;

#if WINAPPSDK

internal class ThemeListenerHelperWindow
{
    public delegate void ThemeChangedHandler(ApplicationTheme theme);

    public static ThemeListenerHelperWindow Instance = new();
    private static string s_className = "ThemeListenerHelperWindow";
    private IntPtr m_hwnd;
    public event ThemeChangedHandler? ThemeChanged;
    delegate Windows.Win32.Foundation.LRESULT WndProc(Windows.Win32.Foundation.HWND hWnd, uint msg, Windows.Win32.Foundation.WPARAM wParam, Windows.Win32.Foundation.LPARAM lParam);
    private WndProc m_wnd_proc_delegate;
    private void registerClass()
    {
        unsafe
        {
            var wcx = new Windows.Win32.UI.WindowsAndMessaging.WNDCLASSEXW();
            wcx.cbSize = (uint)Marshal.SizeOf(wcx);
            fixed (char* pClassName = s_className)
            {
                wcx.lpszClassName = new Windows.Win32.Foundation.PCWSTR(pClassName);
            }
            wcx.lpfnWndProc = wndProc;
            if (PInvoke.RegisterClassEx(in wcx) == 0)
            {
                Debug.WriteLine(new Win32Exception(Marshal.GetLastWin32Error()).Message);
            }
        }

    }

    static Windows.Win32.Foundation.LRESULT wndProc(Windows.Win32.Foundation.HWND hWnd, uint msg, Windows.Win32.Foundation.WPARAM wParam, Windows.Win32.Foundation.LPARAM lParam)
    {
        switch (msg)
        {
            case PInvoke.WM_SETTINGCHANGE:
                var value = Registry.GetValue(
                    """HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize""",
                    "AppsUseLightTheme",
                    true
                    );
                if (value != null)
                {
                    Instance.ThemeChanged?.Invoke((int)value == 1 ? ApplicationTheme.Light : ApplicationTheme.Dark);
                }
                break;
        }
        return PInvoke.DefWindowProc(hWnd, msg, wParam, lParam);
    }

    unsafe private static IntPtr createWindow()
    {
        var hwnd = PInvoke.CreateWindowEx(
            0,
            s_className,
            "",
            0,
            0, 0, 0, 0,
            new Windows.Win32.Foundation.HWND(),
            null,
            null,
            null);
        if (hwnd == IntPtr.Zero)
        {
            Debug.WriteLine(new Win32Exception(Marshal.GetLastWin32Error()).Message);
        }
        return hwnd;
    }
    private ThemeListenerHelperWindow()
    {
        m_wnd_proc_delegate = wndProc;
        s_className = "ThemeListenerHelperWindow";
        registerClass();
        m_hwnd = createWindow();
    }
}
#endif
