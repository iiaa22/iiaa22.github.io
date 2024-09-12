using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

/// <summary>
/// 处理系统设置的类。
/// </summary>
[System.Serializable]
public class SystemSettings : ObservableUserData<SystemSettings>
{
    [SerializeField]
    private int _bgmVolume = 100;
    [SerializeField]
    private int _seVolume = 100;
    [SerializeField]
    private LanguageType _language = LanguageType.SChinese;
    [SerializeField]
    private bool _autoSave = true;
    [SerializeField]
    private int _autoSaveDuration = 30;
    [SerializeField]
    private int _maxAutoSaveCount = 10;
    [SerializeField]
    private int _lastAutoSaveSlot = -1;
    [SerializeField]
    private List<int> _saveSlotList = new();
    private ObservableCollection<int> _saveSlotObservableCollection = new();
    [SerializeField]
    private List<int> _autoSaveSlotList = new();
    private ObservableCollection<int> _autoSaveSlotObservableCollection = new();

    public SystemSettings()
    {
        AddListener();
    }

    private void AddListener()
    {
        _saveSlotObservableCollection.CollectionChanged += (sender, e) => TriggerDataChanged(this);
        _autoSaveSlotObservableCollection.CollectionChanged += (sender, e) => TriggerDataChanged(this);
    }

    public override void BeforeSave()
    {
        _saveSlotList = _saveSlotObservableCollection.ToList();
        _autoSaveSlotList = _autoSaveSlotObservableCollection.ToList();
    }

    public override void AfterLoad()
    {
        _saveSlotObservableCollection = new(_saveSlotList);
        _autoSaveSlotObservableCollection = new(_autoSaveSlotList);
        AddListener();
    }

    /// <summary>
    /// 背景音乐音量（0-100）。
    /// </summary>
    public int BGMVolume
    {
        get => _bgmVolume;
        set
        {
            if (_bgmVolume != value)
            {
                _bgmVolume = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 音效音量（0-100）。
    /// </summary>
    public int SEVolume
    {
        get => _seVolume;
        set
        {
            if (_seVolume != value)
            {
                _seVolume = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 语言。
    /// </summary>
    public LanguageType Language
    {
        get => _language;
        set
        {
            if (_language != value)
            {
                _language = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 自动存档开关。
    /// </summary>
    public bool AutoSave
    {
        get => _autoSave;
        set
        {
            if (_autoSave != value)
            {
                _autoSave = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 自动存档间隔（秒）。
    /// </summary>
    public int AutoSaveDuration
    {
        get => _autoSaveDuration;
        set
        {
            if (_autoSaveDuration != value)
            {
                _autoSaveDuration = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 最大自动存档数量。
    /// </summary>
    public int MaxAutoSaveCount
    {
        get => _maxAutoSaveCount;
        set
        {
            if (_maxAutoSaveCount != value)
            {
                _maxAutoSaveCount = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 上一次自动存档的位置。
    /// </summary>
    public int LastAutoSaveSlot
    {
        get => _lastAutoSaveSlot;
        set
        {
            if (_lastAutoSaveSlot != value)
            {
                _lastAutoSaveSlot = value;
                TriggerDataChanged(this);
            }
        }
    }

    /// <summary>
    /// 可用存档的存档位列表。
    /// </summary>
    public ObservableCollection<int> SaveSlotList => _saveSlotObservableCollection;

    /// <summary>
    /// 可用自动存档的存档位列表。
    /// </summary>
    public ObservableCollection<int> AutoSaveSlotList => _autoSaveSlotObservableCollection;
}
