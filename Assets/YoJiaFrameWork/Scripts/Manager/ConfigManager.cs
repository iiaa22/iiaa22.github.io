using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 配置管理器。
/// 该管理器会自动查找Resources文件夹下的所有配置表数据并记录。
/// </summary>
public class ConfigManager
{
    private static List<ScriptableObject> configList = new();

    /// <summary>
    /// 加载配置。
    /// </summary>
    public static void Load()
    {
        configList.AddRange(Resources.LoadAll<ScriptableObject>(""));
    }

    /// <summary>
    /// 获得配置数据。
    /// </summary>
    /// <typeparam name="T">配置数据类型</typeparam>
    /// <param name="id">数据ID</param>
    /// <returns>配置数据</returns>
    public static T GetConfigDataByID<T>(int id) where T : BaseConfigItem
    {
        foreach (ScriptableObject config in configList)
        {
            if (config is BaseConfig<T>)
            {
                BaseConfig<T> temp = config as BaseConfig<T>;
                return temp[id];
            }
        }
        Debug.LogError($"配置错误, 找不到对应配置表, 配置数据名称: {typeof(T)}");
        return null;
    }

    /// <summary>
    /// 获得配置数据。
    /// </summary>
    /// <typeparam name="T">配置数据类型</typeparam>
    /// <param name="name">数据名</param>
    /// <returns>配置数据</returns>
    public static T GetConfigDataByName<T>(string name) where T : BaseConfigItem
    {
        foreach (ScriptableObject config in configList)
        {
            if (config is BaseConfig<T>)
            {
                BaseConfig<T> temp = config as BaseConfig<T>;
                return temp[name];
            }
        }
        Debug.LogError($"配置错误, 找不到对应配置表, 配置数据名称: {typeof(T)}");
        return null;
    }

    /// <summary>
    /// 获得配置表长度。
    /// </summary>
    /// <typeparam name="T">配置数据类型</typeparam>
    /// <returns>配置表长度</returns>
    public static int GetConfigDataLength<T>() where T : BaseConfigItem
    {
        foreach (ScriptableObject config in configList)
        {
            if (config is BaseConfig<T>)
            {
                BaseConfig<T> temp = config as BaseConfig<T>;
                return temp.DataCount;
            }
        }
        Debug.LogError($"配置错误, 找不到对应配置表, 配置数据名称: {typeof(T)}");
        return 0;
    }
}
