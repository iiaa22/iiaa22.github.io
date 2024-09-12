using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理单位的类。
/// </summary>
[System.Serializable]
public class Unit : ObservableUserData<Unit>
{
    public readonly int configID;
    private UnitsConfigItem _config;
    [SerializeField]
    private int _maxHP;
    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _maxToughness;
    [SerializeField]
    private int _toughness;
    [SerializeField]
    private int _basicATK;
    [SerializeField]
    private int _basicDEF;
    [SerializeField]
    private int _basicEDF;
    [SerializeField]
    private float _basicSPD;
    [SerializeField]
    private float _basicRES;
    [SerializeField]
    private float _basicERS;
    [SerializeField]
    private List<Skill> _skillList = new();
    [SerializeField]
    private List<State> _stateList = new();

    /// <summary>
    /// 受到伤害等事件用委托。
    /// </summary>
    /// <param name="attacker">攻击者</param>
    /// <param name="damageInfo">伤害信息</param>
    public delegate void DamagedHandler(Unit attacker, DamageInfo damageInfo);
    /// <summary>
    /// 受到攻击事件。
    /// </summary>
    public event DamagedHandler OnAttacked;
    /// <summary>
    /// 受到伤害事件。
    /// </summary>
    public event DamagedHandler OnDamaged;
    /// <summary>
    /// 死亡事件。
    /// </summary>
    public event DamagedHandler OnDead;

    public Unit(int configID)
    {
        this.configID = configID;
        _config = ConfigManager.GetConfigDataByID<UnitsConfigItem>(configID);
        _maxHP = _config.DefaultMaxHP;
        _hp = _maxHP;
        _maxToughness = _config.defaultMaxToughness;
        _toughness = _maxToughness;
        _basicATK = _config.DefaultATK;
        _basicSPD = _config.defaultSPD;
        AddListener();
        _config.onInit?.Invoke(this);
    }

    protected virtual void AddListener()
    {
        foreach (Skill skill in _skillList)
        {
            skill.OnDataChanged += skill => TriggerDataChanged(this);
        }
        foreach (State state in _stateList)
        {
            state.OnDataChanged += state => TriggerDataChanged(this);
        }
    }

    public override void BeforeSave()
    {

    }

    public override void AfterLoad()
    {
        _config = ConfigManager.GetConfigDataByID<UnitsConfigItem>(configID);
        AddListener();
    }

    /// <summary>
    /// 单位的基础配置。
    /// </summary>
    public UnitsConfigItem Config { get => _config; private set => _config = value; }

    /// <summary>
    /// 攻击是否被禁用。
    /// </summary>
    public bool IsAttackDisabled
    {
        get
        {
            foreach (Skill skill in GetSkills())
            {
                if (skill.Config.disableAttack) return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 移动是否被禁用。
    /// </summary>
    public bool IsMoveDisabled
    {
        get
        {
            foreach (Skill skill in GetSkills())
            {
                if (skill.Config.disableMove) return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 单位的生命值上限。
    /// </summary>
    public int MaxHP
    {
        get => _maxHP;
        set
        {
            if (_maxHP != value)
            {
                _maxHP = value;
                if (_hp > _maxHP) _hp = _maxHP;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 单位的生命值。
    /// </summary>
    public int HP
    {
        get => _hp;
        set
        {
            value = Mathf.Max(0, Mathf.Min(Mathf.Max(_hp, _maxHP), value));
            if (_hp != value)
            {
                _hp = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 攻击单位。
    /// </summary>
    /// <param name="target">目标单位</param>
    public void Attack(Unit target)
    {
        target.Damage(this, new()
        {
            damageType = Config.damageType,
            damage = Mathf.RoundToInt(Config.damageType == DamageType.Physical ? (ATK - target.DEF) * (100 - target.RES) / 100 : (ATK - target.EDF) * (100 - target.ERS) / 100)
        });
    }

    /// <summary>
    /// 受到伤害。
    /// </summary>
    /// <param name="attacker">攻击者</param>
    /// <param name="damageInfo">伤害信息</param>
    public void Damage(Unit attacker, DamageInfo damageInfo)
    {
        int tempDamage = damageInfo.damage;
        if (damageInfo.damageType == DamageType.Physical)
        {
            foreach (State state in GetStates())
            {
                int shieldDamage = Mathf.Min(tempDamage, state.PhysicalShield);
                state.PhysicalShield -= shieldDamage;
                tempDamage -= shieldDamage;
            }
        }
        else
        {
            foreach (State state in GetStates())
            {
                int shieldDamage = Mathf.Min(tempDamage, state.EnergyShield);
                state.EnergyShield -= shieldDamage;
                tempDamage -= shieldDamage;
            }
        }
        foreach (State state in GetStates())
        {
            int shieldDamage = Mathf.Min(tempDamage, state.UniversalShield);
            state.UniversalShield -= shieldDamage;
            tempDamage -= shieldDamage;
        }
        HP -= tempDamage;
        OnAttacked?.Invoke(attacker, damageInfo);
        foreach (Skill skill in GetSkills())
        {
            skill.Config.onAttacked?.Invoke(attacker, this, skill, damageInfo);
        }
        if (tempDamage > 0)
        {
            damageInfo.damage = tempDamage;
            OnDamaged?.Invoke(attacker, damageInfo);
            foreach (Skill skill in GetSkills())
            {
                skill.Config.onDamaged?.Invoke(attacker, this, skill, damageInfo);
            }
        }
        if (HP == 0)
        {
            foreach (Skill skill in GetSkills())
            {
                skill.Config.beforeDead?.Invoke(attacker, this, skill, damageInfo);
            }
            if (HP == 0) OnDead?.Invoke(attacker, damageInfo);
        }
    }

    /// <summary>
    /// 单位的韧性值上限。
    /// </summary>
    public int MaxToughness
    {
        get => _maxToughness;
        set
        {
            if (_maxToughness != value)
            {
                _maxToughness = value;
                if (_toughness > _maxToughness) _toughness = _maxToughness;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 单位的韧性值。
    /// </summary>
    public int Toughness
    {
        get => _toughness;
        set
        {
            value = Mathf.Max(0, Mathf.Min(Mathf.Max(_toughness, _maxToughness), value));
            if (_toughness != value)
            {
                _toughness = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 单位的基础攻击力。
    /// </summary>
    public int BasicATK
    {
        get => _basicATK;
        set
        {
            if (_basicATK != value)
            {
                _basicATK = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 单位的攻击力加成。
    /// </summary>
    public int BonusATK
    {
        get
        {
            int bonusATK = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusATK += skill.Config.bonusATK;
            }
            return bonusATK;
        }
    }

    /// <summary>
    /// 单位的攻击力加成百分比（%）。
    /// </summary>
    public int BonusATK_P
    {
        get
        {
            int bonusATK_P = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusATK_P += skill.Config.bonusATK_P;
            }
            return bonusATK_P;
        }
    }

    /// <summary>
    /// 单位的攻击力。
    /// </summary>
    public int ATK => BasicATK * (100 + BonusATK_P) / 100 + BonusATK;

    /// <summary>
    /// 单位的基础物理防御力。
    /// </summary>
    public int BasicDEF
    {
        get => _basicDEF;
        set
        {
            if (_basicDEF != value)
            {
                _basicDEF = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 单位的物理防御力加成。
    /// </summary>
    public int BonusDEF
    {
        get
        {
            int bonusDEF = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusDEF += skill.Config.bonusDEF;
            }
            return bonusDEF;
        }
    }

    /// <summary>
    /// 单位的物理防御力加成百分比（%）。
    /// </summary>
    public int BonusDEF_P
    {
        get
        {
            int bonusDEF_P = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusDEF_P += skill.Config.bonusDEF_P;
            }
            return bonusDEF_P;
        }
    }

    /// <summary>
    /// 单位的物理防御力。
    /// </summary>
    public int DEF => BasicDEF * (100 + BonusDEF_P) / 100 + BonusDEF;

    /// <summary>
    /// 单位的基础能量防御力。
    /// </summary>
    public int BasicEDF
    {
        get => _basicEDF;
        set
        {
            if (_basicEDF != value)
            {
                _basicEDF = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 单位的能量防御力加成。
    /// </summary>
    public int BonusEDF
    {
        get
        {
            int bonusEDF = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusEDF += skill.Config.bonusEDF;
            }
            return bonusEDF;
        }
    }

    /// <summary>
    /// 单位的能量防御力加成百分比（%）。
    /// </summary>
    public int BonusEDF_P
    {
        get
        {
            int bonusEDF_P = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusEDF_P += skill.Config.bonusEDF_P;
            }
            return bonusEDF_P;
        }
    }

    /// <summary>
    /// 单位的能量防御力。
    /// </summary>
    public int EDF => BasicEDF * (100 + BonusEDF_P) / 100 + BonusEDF;

    /// <summary>
    /// 单位的基础速度（格每秒）。
    /// </summary>
    public float BasicSPD
    {
        get => _basicSPD;
        set
        {
            if (_basicSPD != value)
            {
                _basicSPD = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 单位的速度加成（格每秒）。
    /// </summary>
    public float BonusSPD
    {
        get
        {
            float bonusSPD = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusSPD += skill.Config.bonusSPD;
            }
            return bonusSPD;
        }
    }

    /// <summary>
    /// 单位的速度加成百分比（%）。
    /// </summary>
    public int BonusSPD_P
    {
        get
        {
            int bonusSPD_P = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusSPD_P += skill.Config.bonusSPD_P;
            }
            return bonusSPD_P;
        }
    }

    /// <summary>
    /// 单位的速度（格每秒）。
    /// </summary>
    public float SPD => BasicSPD * (100 + BonusSPD_P) / 100 + BonusSPD;

    /// <summary>
    /// 单位的基础物理抗性（%）。
    /// </summary>
    public float BasicRES
    {
        get => _basicRES;
        set
        {
            if (_basicRES != value)
            {
                _basicRES = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 单位的物理抗性加成（%）。
    /// </summary>
    public float BonusRES
    {
        get
        {
            float bonusRES = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusRES = 100 - (100 - bonusRES) * (100 - skill.Config.bonusRES) / 100;
            }
            return bonusRES;
        }
    }

    /// <summary>
    /// 单位的物理抗性（%）。
    /// </summary>
    public float RES => 100 - (100 - BasicRES) * (100 - BonusRES) / 100;

    /// <summary>
    /// 单位的基础能量抗性（%）。
    /// </summary>
    public float BasicERS
    {
        get => _basicERS;
        set
        {
            if (_basicERS != value)
            {
                _basicERS = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 单位的能量抗性加成（%）。
    /// </summary>
    public float BonusERS
    {
        get
        {
            float bonusERS = 0;
            foreach (Skill skill in GetSkills())
            {
                bonusERS = 100 - (100 - bonusERS) * (100 - skill.Config.bonusERS) / 100;
            }
            return bonusERS;
        }
    }

    /// <summary>
    /// 单位的能量抗性（%）。
    /// </summary>
    public float ERS => 100 - (100 - BasicERS) * (100 - BonusERS) / 100;

    /// <summary>
    /// 获得单位的技能列表。
    /// </summary>
    /// <returns>技能列表</returns>
    public List<Skill> GetSkills() => new(_skillList);

    /// <summary>
    /// 习得技能。
    /// </summary>
    /// <param name="skill">技能</param>
    public void LearnSkill(Skill skill)
    {
        _skillList.Add(skill);
        skill.OnDataChanged += skill => TriggerDataChanged(this);
        skill.Config.onLearned?.Invoke(this, skill);
        TriggerDataChanged(this);
    }

    /// <summary>
    /// 遗忘技能。
    /// </summary>
    /// <param name="skill">技能</param>
    /// <returns>单位是否带有此技能</returns>
    public bool ForgetSkill(Skill skill)
    {
        if (_skillList.Remove(skill))
        {
            skill.Config.onForgotten?.Invoke(this, skill);
            TriggerDataChanged(this);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获得单位的状态列表。
    /// </summary>
    /// <returns>状态列表</returns>
    public List<State> GetStates() => new(_stateList);

    /// <summary>
    /// 添加状态。
    /// </summary>
    /// <param name="state">状态</param>
    public void AddState(State state)
    {
        _stateList.Add(state);
        state.OnDataChanged += state => TriggerDataChanged(this);
        state.OnDurationEnd += state => RemoveState(state);
        state.Config.onGained?.Invoke(this, state);
        TriggerDataChanged(this);
    }

    /// <summary>
    /// 移除状态。
    /// </summary>
    /// <param name="state">状态</param>
    /// <returns>单位是否带有此状态</returns>
    public bool RemoveState(State state)
    {
        if (_stateList.Remove(state))
        {
            state.Config.onLost?.Invoke(this, state);
            TriggerDataChanged(this);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 单位的物理护盾总量。
    /// </summary>
    public int PhysicalShield
    {
        get
        {
            int shield = 0;
            foreach (var state in _stateList)
            {
                shield += state.PhysicalShield;
            }
            return shield;
        }
    }

    /// <summary>
    /// 单位的能量护盾总量。
    /// </summary>
    public int EnergyShield
    {
        get
        {
            int shield = 0;
            foreach (var state in _stateList)
            {
                shield += state.EnergyShield;
            }
            return shield;
        }
    }

    /// <summary>
    /// 单位的全能护盾总量。
    /// </summary>
    public int UniversalShield
    {
        get
        {
            int shield = 0;
            foreach (var state in _stateList)
            {
                shield += state.UniversalShield;
            }
            return shield;
        }
    }
}

/// <summary>
/// 伤害信息
/// </summary>
public struct DamageInfo
{
    /// <summary>
    /// 伤害类型
    /// </summary>
    public DamageType damageType;
    /// <summary>
    /// 伤害量
    /// </summary>
    public int damage;
}
