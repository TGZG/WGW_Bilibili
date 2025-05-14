using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Timers;
using UnityEngine;
using System.Diagnostics;

namespace CMKZ {
    public static partial class LocalStorage {
        public delegate bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);//设置光标位置
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);//获取光标位置。用于测试编程
        [DllImport("user32.dll")]
        public static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, int extraInfo);//鼠标事件
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void Keybd_event(byte bvk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsCallback lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, char[] lpWindowText, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hwnd, bool bInvert);
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] FileOpenDialog dialog);
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SHBrowseForFolder([In, Out] OpenDialogDir ofn);
        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        //C02：我引用这一行报错，因此先注释掉
        //[DllImport("gdi32.dll")]
        //public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
    //个人函数
    public static partial class LocalStorage {
        public static IntPtr SelfHand = Process.GetCurrentProcess().MainWindowHandle;
        public static System.Timers.Timer FlashTimer;
        public static void Flash(bool X) {
            if (FlashTimer == null) {
                FlashTimer = new(100);
                //FlashTimer.AutoReset = true;
                FlashTimer.Elapsed += (sender, e) => {
                    FlashWindow(SelfHand, true);
                };
                OnAppQuit(() => {
                    if (FlashTimer != null) {
                        FlashTimer.Enabled = false;
                        FlashTimer.Dispose();
                    }
                });
            }
            FlashTimer.Enabled = X;
        }
        public static bool GetOFN([In, Out] OpenFileName ofn) {
            return GetOpenFileName(ofn);
        }
        public static bool GetSFN([In, Out] OpenFileName ofn) {
            return GetSaveFileName(ofn);
        }
        public static void 左键单击(int x, int y) {
            SetCursorPos(x, y);
            mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, 0);
            mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, 0);
        }
        public static void 左键单击(Vector2Int X) {
            左键单击(X.x, X.y);
        }
        public static void 右键单击(int x, int y) {
            SetCursorPos(x, y);
            mouse_event(MouseEventFlag.RightDown, 0, 0, 0, 0);
            mouse_event(MouseEventFlag.RightUp, 0, 0, 0, 0);
        }
        public static void 右键单击(Vector2Int X) {
            右键单击(X.x, X.y);
        }
    }
}