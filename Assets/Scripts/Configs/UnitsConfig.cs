using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New UnitsConfig", menuName = "[YoJia]配置/单位配置列表", order = 0)]
public class UnitsConfig : BaseConfig<UnitsConfigItem>
{

}

[System.Serializable]
public class UnitsConfigItem : BaseConfigItem
{
    [Tooltip("单位类型")]
    public UnitType type;
    [Tooltip("单位移动类型")]
    public MoveType moveType;
    [Min(0)]
    [Tooltip("单位等级")]
    public int level;
    [Min(0)]
    [Tooltip("下一等级单位ID列表")]
    public List<int> nextLevelIDList;
    [Tooltip("阵营类型")]
    public FactionType faction;
    [Tooltip("种族类型")]
    public RaceType race;
    [Min(0)]
    [Tooltip("默认最大韧性值")]
    public int defaultMaxToughness;
    [Min(0)]
    [Tooltip("默认速度（格每秒）")]
    public float defaultSPD = 10f;
    [Tooltip("攻击类型")]
    public AttackType attackType;
    [Tooltip("伤害类型")]
    public DamageType damageType;
    [Min(0.01f)]
    [Tooltip("攻击间隔（秒）")]
    public float attackDuration = 1f;
    [Min(0f)]
    [Tooltip("攻击半径（格）")]
    public float attackRadius = 10f;
    [Tooltip("击退距离（格）（负数代表吸引）")]
    public float knockbackDistance;
    [Tooltip("是否可部署")]
    public bool isbuildable = true;
    [Tooltip("是否可碰撞")]
    public bool isCollidable = true;
    [Tooltip("是否可被己方作为目标")]
    public bool isAllyTargetable = true;
    [Tooltip("是否可被敌方作为目标")]
    public bool isEnemyTargetable = true;
    [Tooltip("单位初始化事件\n参数1：初始化的单位")]
    public UnityEvent<Unit> onInit;

    /// <summary>
    /// 默认最大生命值
    /// </summary>
    public int DefaultMaxHP => type == UnitType.Free ? 10 : 100;

    /// <summary>
    /// 默认伤害值
    /// </summary>
    public int DefaultATK => type == UnitType.Free ? 1 : 10;
}
