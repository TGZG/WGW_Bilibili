using System;
using System.Collections.Generic;//List
using System.Linq;//from XX select XX
using System.Text.RegularExpressions;
using TMPro;//InputField
using static CMKZ.LocalStorage;
using UnityEngine;

namespace CMKZ {
    public enum Language : int {
        arabic,    //阿拉伯语
        bulgarian, //保加利亚语
        schinese,  //简体中文
        tchinese,  //繁体中文
        czech,     //捷克语
        danish,    //丹麦语
        dutch,     //荷兰语
        english,   //英语
        finnish,   //芬兰语
        french,    //法语
        german,    //德语
        greek,     //希腊语
        hungarian, //匈牙利语
        indonesian,//印度尼西亚语
        italian,   //意大利语
        japanese,  //日语
        koreana,   //韩语
        norwegian, //挪威语
        polish,    //波兰语
        portuguese,//葡萄牙语
        brazilian, //葡萄牙语 - 巴西
        romanian,  //罗马尼亚语
        russian,   //俄语
        spanish,   //西班牙语 - 西班牙
        latam,     //西班牙语 - 拉丁美洲
        swedish,   //瑞典语
        thai,      //泰语
        turkish,   //土耳其语
        ukrainian, //乌克兰语
        vietnamese,//越南语
    }
    public static partial class LocalStorage {
        public static Language 语言;
        public static string 语言项目前缀;
        public static bool IsLanguageInit;
        public static Dictionary<string, string> 语言集 = new();
        public static Dictionary<TextMeshProUGUI, string> LanguageObject = new();//key
        /// <![CDATA[
        /// <?xml version="1.0" encoding="UTF-8"?>
        /// <Data>
        /// 	<String Key="Game.Name" Value="超级扫雷" />
        /// </Data>
        /// ]]>
        public static void SetLanguage(Language X) {
            if (语言项目前缀 == null) throw new Exception("请先设置语言前缀（游戏名）");
            var A = $"{语言项目前缀}_{X.ToString()}";
            PrintSystem($"设置本地化语言：{A}");
            IsLanguageInit = true;
            语言 = X;

            var XML文本 = AllText[A];
            if (XML文本 == null) {
                if (X != Language.english) {
                    SetLanguage(Language.english);
                } else {
                    throw new Exception("英语不存在");
                }
            } else {
                语言集 = Regex.Matches(XML文本, @"Key=""(.*?)"" Value=""(.*?)""").ToDictionary(t => t.Groups[1].Value, t => t.Groups[2].Value);
                LanguageObject.RemoveAll(t => t.Key == null);
                LanguageObject.ForEach(t => SetLocal(t.Key, t.Value, false));
            }
        }
        public static void InitLanguage() {
            if (!IsLanguageInit) {
                PrintWarning("未初始化语言。已自动设置为中文");
                SetLanguage(Language.schinese);
            }
        }
        public static string GetLocal(string X) {
            InitLanguage();
            return 语言集[X] ?? "Localization Error. Please Criticize.";
        }
        [Obsolete("请改为使用SetLanguageText")]
        public static string L<T>(T X) where T : Enum {
            return GetLocal(X.ToString());
        }

        //Save用于优化性能
        public static void SetLocal(TextMeshProUGUI X, string Y, bool Save = true) {
            InitLanguage();
            X.text = 语言集[Y];
            if (Save && !LanguageObject.ContainsKey(X)) {
                LanguageObject[X] = Y;
            }
        }
        public static void SetLocal(TextMeshProUGUI X, string Y, Func<string, string> Z, bool Save = true) {
            InitLanguage();
            X.text = Z(语言集[Y]);
            if (Save && !LanguageObject.ContainsKey(X)) {
                LanguageObject[X] = Y;
            }
        }
        public static void SetLocal(TextMeshProUGUI X, string Y, string Y2, Func<string, string, string> Z, bool Save = true) {
            InitLanguage();
            X.text = Z(语言集[Y], 语言集[Y2]);
            if (Save && !LanguageObject.ContainsKey(X)) {
                LanguageObject[X] = Y;
            }
        }
        public static void 设置本地化<T>(this TextMeshProUGUI X, T Y) where T : Enum {
            SetLocal(X, Y.ToString());
        }
        public static void 设置本地化<T>(this TextMeshProUGUI X, T Y, Func<string, string> Z) where T : Enum {
            SetLocal(X, Y.ToString(), t => Z(t));
        }
        public static void 设置本地化<T>(this TextMeshProUGUI X, T Y, T Y2, Func<string, string, string> Z) where T : Enum {
            SetLocal(X, Y.ToString(), Y2.ToString(), (t, t2) => Z(t, t2));
        }
        public static GameObject SetLanguageText<T>(this GameObject X, T Y) where T : Enum {
            X.GetComponentInChildren<TextMeshProUGUI>().设置本地化(Y);
            return X;
        }
        public static GameObject SetLanguageText<T>(this GameObject X, T Y, Func<string, string> Z) where T : Enum {
            X.GetComponentInChildren<TextMeshProUGUI>().设置本地化(Y, Z);
            return X;
        }
        public static GameObject SetLanguageText<T>(this GameObject X, T Y, T Y2, Func<string, string, string> Z) where T : Enum {
            X.GetComponentInChildren<TextMeshProUGUI>().设置本地化(Y, Y2, Z);
            return X;
        }
    }
}