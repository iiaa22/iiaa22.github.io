using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单位类型
/// </summary>
[System.Flags]
public enum UnitType
{
    [Tooltip("武器")]
    Weapon = 1 << 0,
    [Tooltip("法器")]
    Artifact = 1 << 1,
    [Tooltip("功法")]
    Technique = 1 << 2,
    [Tooltip("自由")]
    Free = 1 << 3
}

/// <summary>
/// 单位移动类型
/// </summary>
public enum MoveType
{
    [Tooltip("自由")]
    Free,
    [Tooltip("固定")]
    Fixed
}

/// <summary>
/// 阵营类型
/// </summary>
public enum FactionType
{
    [Tooltip("中立")]
    Neutral,
    [Tooltip("敌方")]
    Enemy,
    [Tooltip("我方")]
    Ally
}

/// <summary>
/// 种族类型
/// </summary>
public enum RaceType
{
    [Tooltip("人族")]
    Human,
    [Tooltip("洪荒")]
    Primordial
}

/// <summary>
/// 攻击类型
/// </summary>
public enum AttackType
{
    [Tooltip("有源头发射")]
    SourceFired,
    [Tooltip("无源头发射")]
    NonSourceFired,
    [Tooltip("挥舞")]
    Swing,
    [Tooltip("环绕")]
    Surround
}

/// <summary>
/// 伤害类型
/// </summary>
public enum DamageType
{
    [Tooltip("物理")]
    Physical,
    [Tooltip("能量")]
    Energy
}