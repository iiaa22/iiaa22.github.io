using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 语言配置列表。
/// </summary>
[CreateAssetMenu(fileName = "New LanguageConfig", menuName = "YoJiaFrameWork配置/语言配置列表", order = 0)]
public class LanguageConfig : BaseConfig<LanguageConfigItem>
{
    private void OnValidate()
    {
        List<LanguageType> languages = new(System.Enum.GetValues(typeof(LanguageType)) as LanguageType[]);
        if (dataList.Count < languages.Count)
        {
            dataList.Clear();
            foreach (LanguageType language in languages)
            {
                dataList.Add(new()
                {
                    name = language.ToString()
                });
            }
        }
        while (dataList.Count > languages.Count)
        {
            dataList.RemoveAt(dataList.Count - 1);
        }
    }
}

/// <summary>
/// 语言配置数据。
/// </summary>
[System.Serializable]
public class LanguageConfigItem : BaseConfigItem
{
    /// <summary>
    /// 语言文本键值对信息。
    /// </summary>
    [System.Serializable]
    public struct LanguageInfo
    {
        /// <summary>
        /// 文本键值
        /// </summary>
        public string key;
        /// <summary>
        /// 目标文本
        /// </summary>
        public string value;
    }

    /// <summary>
    /// 语言文本列表
    /// </summary>
    [Tooltip("语言文本列表")]
    public List<LanguageInfo> texts;
}
