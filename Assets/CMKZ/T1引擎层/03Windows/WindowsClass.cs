using System;
using System.Runtime.InteropServices;
using System.Timers;

namespace CMKZ {
    [Flags]
    public enum MouseEventFlag : uint {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        Absolute = 0x8000
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class FileOpenDialog {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public String filter = null;
        public String customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public String file = null;
        public int maxFile = 0;
        public String fileTitle = null;
        public int maxFileTitle = 0;
        public String initialDir = null;
        public String title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public String defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public String templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }
    /// <summary>
    /// windows系统文件选择窗口
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct OpenFileName {
        public int structSize;
        public IntPtr dlgOwner;
        public IntPtr instance;
        public String filter;
        public String customFilter;
        public int maxCustFilter;
        public int filterIndex;
        public String file;
        public int maxFile;
        public String fileTitle;
        public int maxFileTitle;
        public String initialDir;
        public String title;
        public int flags;
        public short fileOffset;
        public short fileExtension;
        public String defExt;
        public IntPtr custData;
        public IntPtr hook;
        public String templateName;
        public IntPtr reservedPtr;
        public int reservedInt;
        public int flagsEx;
    }
    /// <summary>
    /// windows系统文件夹选择窗口
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct OpenDialogDir {
        public IntPtr hwndOwner;
        public IntPtr pidlRoot;
        public String pszDisplayName;
        public String title;
        public UInt32 ulFlags;
        public IntPtr lpfno;
        public IntPtr lParam;
        public int iImage;
    }
}