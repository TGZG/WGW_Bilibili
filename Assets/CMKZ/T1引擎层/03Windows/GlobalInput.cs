using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using static CMKZ.LocalStorage;

/* 全局钩子。本类的用法与Input类一样，区别只在于：当游戏最小化时，本类依然生效。本类用于：
 * 1.老板键。一键隐藏/显示游戏。
 * 2.网游外挂中，开启/结束外挂。
 */
namespace CMKZ {
    public class GlobalInput : MonoBehaviour {
        public static bool[] KeyStates = new bool[255];
        public static bool[] KeyDownStates = new bool[255];
        public static bool[] KeyUpStates = new bool[255];
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static IntPtr hookID = IntPtr.Zero;
        public static bool GetKey(GlobalKeyCode key) {
            return KeyStates[(int)key];
        }
        public static bool GetKeyDown(GlobalKeyCode key) {
            if (KeyDownStates[(int)key]) {
                KeyDownStates[(int)key] = false;
                return true;
            }
            return false;
        }
        public static bool GetKeyUp(GlobalKeyCode key) {
            if (KeyUpStates[(int)key]) {
                KeyUpStates[(int)key] = false;
                return true;
            }
            return false;
        }
        static GlobalInput() {
            hookID = SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, (nCode, wParam, lParam) => {
                if (nCode >= 0) {
                    int vkCode = Marshal.ReadInt32(lParam);
                    if (wParam == (IntPtr)WM_KEYDOWN) {
                        SetKeyState(vkCode, true);
                    }
                    if (wParam == (IntPtr)WM_KEYUP) {
                        SetKeyState(vkCode, false);
                    }
                }
                return CallNextHookEx(hookID, nCode, wParam, lParam);
            }, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
            DontDestroyOnLoad(new GameObject("GlobalInputManager").AddComponent<GlobalInput>());
            UnityEngine.Debug.Log("Init GlobalInput");
        }
        private static void SetKeyState(int key, bool state) {
            if (!KeyStates[key]) {
                KeyDownStates[key] = state;
            }
            KeyStates[key] = state;
            KeyUpStates[key] = !state;
        }
        void OnDisable() {
            UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
        }
    }
    public enum HookType : int {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }
    public enum GlobalKeyCode : int {
        LBUTTON = 0x01,
        RBUTTON = 0x02,
        CANCEL = 0x03,
        MBUTTON = 0x04,
        BACK = 0x08,
        TAB = 0x09,
        CLEAR = 0x0C,
        RETURN = 0x0D,
        SHIFT = 0x10,
        CONTROL = 0x11,
        MENU = 0x12,
        PAUSE = 0x13,
        CAPITAL = 0x14,
        ESCAPE = 0x1B,
        SPACE = 0x20,
        PRIOR = 0x21,
        NEXT = 0x22,
        END = 0x23,
        HOME = 0x24,
        LEFT = 0x25,
        UP = 0x26,
        RIGHT = 0x27,
        DOWN = 0x28,
        SELECT = 0x29,
        PRINT = 0x2A,
        EXECUTE = 0x2B,
        SNAPSHOT = 0x2C,
        INSERT = 0x2D,
        DELETE = 0x2E,
        HELP = 0x2F,
        // Number keys
        N0 = 0x30,
        N1 = 0x31,
        N2 = 0x32,
        N3 = 0x33,
        N4 = 0x34,
        N5 = 0x35,
        N6 = 0x36,
        N7 = 0x37,
        N8 = 0x38,
        N9 = 0x39,
        // Letter keys
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        LWIN = 0x5B,
        RWIN = 0x5C,
        APPS = 0x5D,
        SLEEP = 0x5F,
        // Numpad keys
        NUMPAD0 = 0x60,
        NUMPAD1 = 0x61,
        NUMPAD2 = 0x62,
        NUMPAD3 = 0x63,
        NUMPAD4 = 0x64,
        NUMPAD5 = 0x65,
        NUMPAD6 = 0x66,
        NUMPAD7 = 0x67,
        NUMPAD8 = 0x68,
        NUMPAD9 = 0x69,
        MULTIPLY = 0x6A,
        ADD = 0x6B,
        SEPARATOR = 0x6C,
        SUBTRACT = 0x6D,
        DECIMAL = 0x6E,
        DIVIDE = 0x6F,
        // Function keys
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,
        // Lock keys
        NUMLOCK = 0x90,
        SCROLL = 0x91,
        // Modifiers
        LSHIFT = 0xA0,
        RSHIFT = 0xA1,
        LCONTROL = 0xA2,
        RCONTROL = 0xA3,
        LMENU = 0xA4,
        RMENU = 0xA5,
        // Media keys
        BROWSER_BACK = 0xA6,
        BROWSER_FORWARD = 0xA7,
        BROWSER_REFRESH = 0xA8,
        BROWSER_STOP = 0xA9,
        BROWSER_SEARCH = 0xAA,
        BROWSER_FAVORITES = 0xAB,
        BROWSER_HOME = 0xAC,
        VOLUME_MUTE = 0xAD,
        VOLUME_DOWN = 0xAE,
        VOLUME_UP = 0xAF,
        MEDIA_NEXT_TRACK = 0xB0,
        MEDIA_PREV_TRACK = 0xB1,
        MEDIA_STOP = 0xB2,
        MEDIA_PLAY_PAUSE = 0xB3,
        LAUNCH_MAIL = 0xB4,
        LAUNCH_MEDIA_SELECT = 0xB5,
        LAUNCH_APP1 = 0xB6,
        LAUNCH_APP2 = 0xB7,
        // Symbols
        COLON = 0xBA,
        PLUS = 0xBB,
        COMMA = 0xBC,
        HYPHEN = 0xBD,
        PERIOD = 0xBE,
        FSLASH = 0xBF,
        TILDE = 0xC0,
        BACKQUOTE = 0xC0, // duplicate of tilde
        LSQUAREBRACKET = 0xDB,
        BSLASH = 0xDC,
        PIPE = 0xDC,    // duplicate of back slash
        RSQUAREBRACKET = 0xDD,
        QUOTE = 0xDE,
        // ???
        PLAY = 0xFA,
        ZOOM = 0xFB,
    }
}
