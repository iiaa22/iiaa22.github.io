using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理用户数据的类。
/// 本类为抽象类，具体数据由子类管理。
/// </summary>
public abstract class BaseUserData
{
    /// <summary>
    /// 保存前触发的事件。
    /// </summary>
    public abstract void BeforeSave();

    /// <summary>
    /// 读取后触发的事件。
    /// </summary>
    public abstract void AfterLoad();
}

/// <summary>
/// 处理可监听用户数据的类。
/// 本类为抽象类，具体数据由子类管理。
/// </summary>
/// <typeparam name="T">子类的泛型</typeparam>
public abstract class ObservableUserData<T> : BaseUserData where T : ObservableUserData<T>
{
    /// <summary>
    /// 数据改变事件用委托
    /// </summary>
    /// <param name="userData">被改变的数据</param>
    public delegate void DataChangedHandler(T userData);
    /// <summary>
    /// 数据发生改变时触发该事件
    /// </summary>
    public event DataChangedHandler OnDataChanged;

    /// <summary>
    /// 触发数据改变事件。
    /// </summary>
    /// <param name="data">数据对象（this）</param>
    protected void TriggerDataChanged(T data)
    {
        OnDataChanged?.Invoke(data);
    }
}
