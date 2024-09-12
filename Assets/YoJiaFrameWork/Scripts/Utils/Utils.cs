using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 工具类。
/// </summary>
public static class Utils
{
    /// <summary>
    /// 获得本地化文本。
    /// </summary>
    /// <param name="key">文本key值</param>
    /// <param name="replace">替换字符串，按顺序会依次替换文本中的{0}、{1}……</param>
    /// <returns>目标文本</returns>
    public static string ToLocalizedString(this string key, params string[] replace)
    {
        string str = null;
        LanguageConfigItem languageConfigItem = ConfigManager.GetConfigDataByName<LanguageConfigItem>(UserDataManager.systemSettings.Language.ToString());
        foreach (LanguageConfigItem.LanguageInfo languageInfo in languageConfigItem.texts)
        {
            if (languageInfo.key == key)
            {
                str = languageInfo.value;
            }
        }
        if (str == null) return key;
        for (int i = 0; i < replace.Length; i++)
        {
            str = str.Replace($"{{{i}}}", replace[i]);
        }
        return str;
    }
}
