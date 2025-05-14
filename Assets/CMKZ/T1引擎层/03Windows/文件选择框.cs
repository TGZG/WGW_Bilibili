using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static string OpenFile(string 默认文件 = "*.png", params string[] 文件类型) {
            string 文件类型字符串 = $"{默认文件}\0{默认文件}";
            foreach (var item in 文件类型) {
                文件类型字符串 += $"\0{item}";
                文件类型字符串 += $"\0{item}";
            }
            var A = new FileOpenDialog();
            A.structSize = Marshal.SizeOf(A);
            //dialog.filter = "png图片文件\0*.png\0";
            A.filter = 文件类型字符串;
            A.file = new string(new char[256]);
            A.maxFile = A.file.Length;
            A.fileTitle = new string(new char[64]);
            A.maxFileTitle = A.fileTitle.Length;
            A.initialDir = Application.dataPath;  //默认路径
            A.title = "Open File Dialog";
            A.defExt = "png";//显示文件的类型
            //注意一下项目不一定要全选 但是0x00000008项不要缺少
            A.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
            if (GetOpenFileName(A)) {
                return A.file;//返回选择的文件路径
            }
            return null;
        }
        /// <summary>
        /// 选择文件夹
        /// </summary>
        public static string ChooseWinFolder() {
            var A = new OpenDialogDir();
            A.pszDisplayName = new string(new char[2000]); ;     // 存放目录路径缓冲区  
            A.title = "选择文件夹";// 标题  
            //ofn.ulFlags = BIF_NEWDIALOGSTYLE | BIF_EDITBOX; // 新的样式,带编辑框  
            IntPtr pidlPtr = SHBrowseForFolder(A);
            char[] charArray = new char[2000];
            for (int i = 0; i < 2000; i++) {
                charArray[i] = '\0';
            }
            SHGetPathFromIDList(pidlPtr, charArray);
            string fullDirPath = new(charArray);
            return fullDirPath[..fullDirPath.IndexOf('\0')];
            //fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));
            //Debug.Log(fullDirPath);//这个就是选择的目录路径
        }
        /// <summary>
        /// 选择文件
        /// </summary>
        public static string ChooseWinFile() {
            var A = new OpenFileName();
            A.structSize = Marshal.SizeOf(A);
            A.filter = "应用程序(*.exe)\0*.exe";
            A.file = new string(new char[1024]);
            A.maxFile = A.file.Length;
            A.fileTitle = new string(new char[64]);
            A.maxFileTitle = A.fileTitle.Length;
            //OpenFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
            A.title = "选择exe文件";
            A.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
            if (GetOpenFileName(A))
                return A.file;
            else
                return null;
        }
        //从默认路径打开文件夹选择窗口
        public static string 打开文件选择窗口(string 默认路径, string 文件格式) {
            var A = new OpenFileName();
            A.structSize = Marshal.SizeOf(A);
            A.filter = $"(*.{文件格式})\0*.{文件格式}";
            A.file = new string(new char[1024]);
            A.maxFile = A.file.Length;
            A.fileTitle = new string(new char[64]);
            A.maxFileTitle = A.fileTitle.Length;
            A.initialDir = 默认路径.Replace('/', '\\');//默认路径
            A.title = $"选择{文件格式}文件";
            A.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
            if (GetOpenFileName(A))
                return A.file;
            else
                return null;
        }
        public static string 打开路径选择窗口() {
            var A = new OpenDialogDir();
            A.pszDisplayName = new string(new char[2000]); ;     // 存放目录路径缓冲区  
            A.title = "选择文件夹";// 标题  
            //ofn.ulFlags = BIF_NEWDIALOGSTYLE | BIF_EDITBOX; // 新的样式,带编辑框  
            IntPtr pidlPtr = SHBrowseForFolder(A);
            char[] charArray = new char[2000];
            for (int i = 0; i < 2000; i++) {
                charArray[i] = '\0';
            }
            SHGetPathFromIDList(pidlPtr, charArray);
            string fullDirPath = new(charArray);
            return fullDirPath[..fullDirPath.IndexOf('\0')];
            //fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));
            //Debug.Log(fullDirPath);//这个就是选择的目录路径
        }
    }
}