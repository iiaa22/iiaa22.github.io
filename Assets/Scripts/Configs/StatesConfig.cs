using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New StatesConfig", menuName = "[YoJia]配置/状态配置列表", order = 0)]
public class StatesConfig : BaseConfig<StatesConfigItem>
{

}

[System.Serializable]
public class StatesConfigItem : BaseConfigItem
{
    [Min(0f)]
    [Tooltip("默认持续时间（秒）（0代表无限时间）")]
    public float defaultDuration;
    [Min(0)]
    [Tooltip("默认物理护盾")]
    public int defaultPhysicalShield;
    [Min(0)]
    [Tooltip("默认能量护盾")]
    public int defaultEnergyShield;
    [Min(0)]
    [Tooltip("默认全能护盾")]
    public int defaultUniversalShield;
    [Tooltip("是否在菜单中隐藏")]
    public bool hideInMenu;
    [Tooltip("状态获得事件\n参数1：获得的单位\n参数2：获得的状态")]
    public UnityEvent<Unit, State> onGained;
    [Tooltip("状态持续事件，此事件会在状态维持期间每帧调用一次\n参数1：持续的单位\n参数2：持续的状态")]
    public UnityEvent<Unit, State> onUpdate;
    [Tooltip("状态失去事件\n参数1：失去的单位\n参数2：失去的状态")]
    public UnityEvent<Unit, State> onLost;
}
