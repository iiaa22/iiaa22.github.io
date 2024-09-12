using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 处理状态的类。
/// </summary>
[System.Serializable]
public class State : ObservableUserData<State>
{
    public readonly int configID;
    private StatesConfigItem _config;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private int _physicalShield;
    [SerializeField]
    private int _energyShield;
    [SerializeField]
    private int _universalShield;

    /// <summary>
    /// 持续时间结束事件
    /// </summary>
    public event UnityAction<State> OnDurationEnd;

    public State(int configID)
    {
        this.configID = configID;
        _config = ConfigManager.GetConfigDataByID<StatesConfigItem>(configID);
        _duration = _config.defaultDuration;
        _physicalShield = _config.defaultPhysicalShield;
        _energyShield = _config.defaultEnergyShield;
        _universalShield = _config.defaultUniversalShield;
    }

    public override void BeforeSave()
    {

    }

    public override void AfterLoad()
    {
        _config = ConfigManager.GetConfigDataByID<StatesConfigItem>(configID);
    }

    /// <summary>
    /// 状态的基础配置。
    /// </summary>
    public StatesConfigItem Config { get => _config; private set => _config = value; }

    /// <summary>
    /// 状态的持续时间（秒）。
    /// </summary>
    public float Duration
    {
        get => _duration;
        set
        {
            if (_duration != value)
            {
                if (_duration > 0f && value <= 0f)
                {
                    _duration = value;
                    OnDurationEnd?.Invoke(this);
                }
                else
                {
                    _duration = value;
                }
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 状态提供的物理护盾。
    /// </summary>
    public int PhysicalShield
    {
        get => _physicalShield;
        set
        {
            if (_physicalShield != value)
            {
                _physicalShield = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 状态提供的能量护盾。
    /// </summary>
    public int EnergyShield
    {
        get => _energyShield;
        set
        {
            if (_energyShield != value)
            {
                _energyShield = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 状态提供的全能护盾。
    /// </summary>
    public int UniversalShield
    {
        get => _universalShield;
        set
        {
            if (_universalShield != value)
            {
                _universalShield = value;
                TriggerDataChanged(this);
            }
        }
    }
}
