using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom;
using UnityEngine;
using Microsoft.CodeAnalysis;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void LoadMods() {
            foreach (string i in Directory.GetFiles(Path.Combine(Application.dataPath, "Mods"), "*.dll", SearchOption.AllDirectories)) {
                Assembly.LoadFrom(i);
            }
            foreach (string i in Directory.GetFiles(Path.Combine(Application.dataPath, "Mods"), "*.cs", SearchOption.AllDirectories)) {
                FileRead(i).RunCode();
            }
        }
    }
}