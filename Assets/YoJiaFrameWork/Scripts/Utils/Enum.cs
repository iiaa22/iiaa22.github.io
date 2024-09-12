using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 语言类型
/// </summary>
public enum LanguageType
{
    /// <summary>
    /// 简体中文
    /// </summary>
    [Tooltip("简体中文")]
    SChinese,
    /// <summary>
    /// 繁体中文
    /// </summary>
    [Tooltip("繁体中文")]
    TChinese,
    /// <summary>
    /// 英文
    /// </summary>
    [Tooltip("英文")]
    English
}

/// <summary>
/// 平台类型
/// </summary>
public enum PlatformType
{
    /// <summary>
    /// 默认
    /// </summary>
    [Tooltip("默认")]
    Default
}
