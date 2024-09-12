using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New SkillsConfig", menuName = "[YoJia]配置/技能配置列表", order = 0)]
public class SkillsConfig : BaseConfig<SkillsConfigItem>
{

}

[System.Serializable]
public class SkillsConfigItem : BaseConfigItem
{
    [Tooltip("禁用攻击")]
    public bool disableAttack;
    [Tooltip("禁用移动")]
    public bool disableMove;
    [Tooltip("攻击力加成值")]
    public int bonusATK = 0;
    [Tooltip("攻击力加成百分比（%）")]
    public int bonusATK_P = 0;
    [Tooltip("物理防御力加成值")]
    public int bonusDEF = 0;
    [Tooltip("物理防御力加成百分比（%）")]
    public int bonusDEF_P = 0;
    [Tooltip("能量防御力加成值")]
    public int bonusEDF = 0;
    [Tooltip("能量防御力加成百分比（%）")]
    public int bonusEDF_P = 0;
    [Tooltip("速度加成值")]
    public float bonusSPD = 0;
    [Tooltip("速度加成百分比（%）")]
    public int bonusSPD_P = 0;
    [Tooltip("物理抗性加成值（%）")]
    public float bonusRES = 0;
    [Tooltip("能量抗性加成值（%）")]
    public float bonusERS = 0;
    [Tooltip("是否在菜单中隐藏")]
    public bool hideInMenu;
    [Tooltip("技能习得事件\n参数1：习得的单位\n参数2：习得的技能")]
    public UnityEvent<Unit, Skill> onLearned;
    [Tooltip("技能遗忘事件\n参数1：遗忘的单位\n参数2：遗忘的技能")]
    public UnityEvent<Unit, Skill> onForgotten;
    [Tooltip("技能使用事件\n参数1：使用的单位\n参数2：使用的技能")]
    public UnityEvent<Unit, Skill> onUsed;
    [Tooltip("受到攻击事件\n参数1：攻击者\n参数2：受击者\n参数3：触发的技能\n参数4：伤害信息")]
    public UnityEvent<Unit, Unit, Skill, DamageInfo> onAttacked;
    [Tooltip("受到伤害事件\n参数1：攻击者\n参数2：受击者\n参数3：触发的技能\n参数4：伤害信息")]
    public UnityEvent<Unit, Unit, Skill, DamageInfo> onDamaged;
    [Tooltip("死亡前事件\n参数1：攻击者\n参数2：受击者\n参数3：触发的技能\n参数4：伤害信息")]
    public UnityEvent<Unit, Unit, Skill, DamageInfo> beforeDead;
}
