using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式父类。
/// </summary>
public abstract class BaseSingleton<T> where T : new()
{
    private static T _instance;

    /// <summary>
    /// 获得单例对象。
    /// </summary>
    public static T Instance => _instance ??= new T();
}
