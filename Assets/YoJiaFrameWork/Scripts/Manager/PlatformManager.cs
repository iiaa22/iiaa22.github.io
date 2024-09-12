using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台管理器。
/// </summary>
public class PlatformManager
{
    /// <summary>
    /// 当前平台。
    /// </summary>
    public static PlatformType Current { get; private set; }

    /// <summary>
    /// 初始化管理器。
    /// </summary>
    public static void Init()
    {
        Current = PlatformType.Default;
    }
}
