using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台通用配置列表。
/// </summary>
[CreateAssetMenu(fileName = "New PlatformCommonConfig", menuName = "YoJiaFrameWork配置/平台配置列表", order = 0)]
public class PlatformCommonConfig : BaseConfig<PlatformCommonConfigItem>
{
    private void OnValidate()
    {
        List<PlatformType> platforms = new(System.Enum.GetValues(typeof(PlatformType)) as PlatformType[]);
        if (dataList.Count < platforms.Count)
        {
            dataList.Clear();
            foreach (PlatformType platform in platforms)
            {
                dataList.Add(new()
                {
                    name = platform.ToString()
                });
            }
        }
        while (dataList.Count > platforms.Count)
        {
            dataList.RemoveAt(dataList.Count - 1);
        }
    }
}

/// <summary>
/// 平台通用配置数据。
/// </summary>
[System.Serializable]
public class PlatformCommonConfigItem : BaseConfigItem
{
    /// <summary>
    /// 游戏ID
    /// </summary>
    [Tooltip("游戏ID")]
    public string appID = "-1";
    /// <summary>
    /// 游戏版本
    /// </summary>
    [Tooltip("游戏版本")]
    public string appVersion = "0.1-alpha";
    /// <summary>
    /// 默认语言
    /// </summary>
    [Tooltip("默认语言")]
    public LanguageType defaultLanguage = LanguageType.SChinese;
    /// <summary>
    /// 测试模式
    /// </summary>
    [Tooltip("测试模式")]
    public bool debugMode = false;
}
