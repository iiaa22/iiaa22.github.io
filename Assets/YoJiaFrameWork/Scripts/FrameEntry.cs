using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 框架入口
/// </summary>
public class FrameEntry : MonoBehaviour
{
    /// <summary>
    /// 是否自动存档
    /// </summary>
    public static bool autoSave = false;

    /// <summary>
    /// 全局刷新事件，每20毫秒触发一次
    /// 目前仅用于刷新UI。
    /// </summary>
    public static event UnityAction OnGlobalRefresh;

    /// <summary>
    /// 初始化框架。
    /// </summary>
    /// <returns></returns>
    public static IEnumerator Init()
    {
        PlatformManager.Init();
        ConfigManager.Load();
        UserDataManager.LoadSystemSettings();
        Debug.Log("YoJiaFrameWork For Unity---版本号: 20240912");
        GameObject frameEntry = new();
        frameEntry.name = "FrameEntry";
        frameEntry.AddComponent<FrameEntry>();
        yield return null;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(InitGame());
    }

    private IEnumerator InitGame()
    {
        StartCoroutine(CheckAutoSave());
        while (true)
        {
            OnGlobalRefresh?.Invoke();
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator CheckAutoSave()
    {
        while (true)
        {
            if (autoSave && UserDataManager.systemSettings.AutoSave) UserDataManager.AutoSaveUserData();
            yield return new WaitForSeconds(UserDataManager.systemSettings.AutoSaveDuration);
        }
    }
}
