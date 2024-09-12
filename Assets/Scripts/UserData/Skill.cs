using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理技能的类。
/// </summary>
[System.Serializable]
public class Skill : ObservableUserData<Skill>
{
    public readonly int configID;
    private SkillsConfigItem _config;
    [SerializeField]
    private List<int> _intList = new();
    [SerializeField]
    private List<float> _floatList = new();
    [SerializeField]
    private List<string> _stringList = new();

    public Skill(int configID)
    {
        this.configID = configID;
        _config = ConfigManager.GetConfigDataByID<SkillsConfigItem>(configID);
    }

    public override void BeforeSave()
    {

    }

    public override void AfterLoad()
    {
        _config = ConfigManager.GetConfigDataByID<SkillsConfigItem>(configID);
    }

    /// <summary>
    /// 技能的基础配置。
    /// </summary>
    public SkillsConfigItem Config { get => _config; private set => _config = value; }

    /// <summary>
    /// 获得int参数。
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>值</returns>
    public int GetInt(int index)
    {
        while (index >= _intList.Count)
        {
            _intList.Add(0);
        }
        return _intList[index];
    }

    /// <summary>
    /// 设置int参数。
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="value">值</param>
    public void SetInt(int index, int value)
    {
        while (index >= _intList.Count)
        {
            _intList.Add(0);
        }
        if (_intList[index] != value)
        {
            _intList[index] = value;
            TriggerDataChanged(this);
        }
    }

    /// <summary>
    /// 获得float参数。
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>值</returns>
    public float GetFloat(int index)
    {
        while (index >= _floatList.Count)
        {
            _floatList.Add(0f);
        }
        return _floatList[index];
    }

    /// <summary>
    /// 设置float参数。
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="value">值</param>
    public void SetFloat(int index, float value)
    {
        while (index >= _floatList.Count)
        {
            _floatList.Add(0f);
        }
        if (_floatList[index] != value)
        {
            _floatList[index] = value;
            TriggerDataChanged(this);
        }
    }

    /// <summary>
    /// 获得string参数。
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>值</returns>
    public string GetString(int index)
    {
        while (index >= _stringList.Count)
        {
            _stringList.Add("");
        }
        return _stringList[index];
    }

    /// <summary>
    /// 设置string参数。
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="value">值</param>
    public void SetString(int index, string value)
    {
        while (index >= _stringList.Count)
        {
            _stringList.Add("");
        }
        if (_stringList[index] != value)
        {
            _stringList[index] = value;
            TriggerDataChanged(this);
        }
    }
}
