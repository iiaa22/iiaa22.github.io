using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有配置表的父类。
/// </summary>
/// <typeparam name="T">配置数据类型</typeparam>
public abstract class BaseConfig<T> : ScriptableObject where T : BaseConfigItem
{
    /// <summary>
    /// 配置表版本
    /// </summary>
    public int version = 100;
    /// <summary>
    /// 是否启用控制台输出
    /// </summary>
    public bool enableConsole = true;
    /// <summary>
    /// 数据列表
    /// </summary>
    public List<T> dataList = new();

    /// <summary>
    /// 根据ID获得数据。
    /// </summary>
    /// <param name="id">数据ID</param>
    /// <returns>数据对象</returns>
    public T this[int id]
    {
        get
        {
            if (id >= 0 && id < dataList.Count)
            {
                return dataList[id];
            }
            if (enableConsole) Debug.LogError($"配置表错误, 找不到对应ID数据, 配置表名称: {GetType()}, ID: {id}");
            return null;
        }
    }

    /// <summary>
    /// 根据名称获得数据。
    /// </summary>
    /// <param name="name">数据名称</param>
    /// <returns>数据对象</returns>
    public T this[string name]
    {
        get
        {
            foreach (T data in dataList)
            {
                if (data.name == name) return data;
            }
            if (enableConsole) Debug.LogError($"配置表错误, 找不到对应ID数据, 配置表名称: {GetType()}, ID: {name}");
            return null;
        }
    }
    
    /// <summary>
    /// 获得配置表长度。
    /// </summary>
    public int DataCount => dataList.Count;
}

/// <summary>
/// 所有配置表数据的父类。
/// </summary>
[System.Serializable]
public abstract class BaseConfigItem
{
    /// <summary>
    /// 名称
    /// </summary>
    [Tooltip("名称")]
    public string name;
    /// <summary>
    /// 描述
    /// </summary>
    [TextArea]
    [Tooltip("描述")]
    public string description;
    /// <summary>
    /// 备注
    /// </summary>
    [TextArea]
    [Tooltip("备注")]
    public string remark;
}
