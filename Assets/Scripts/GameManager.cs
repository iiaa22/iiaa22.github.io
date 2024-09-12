using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

/// <summary>
/// 管理游戏进程的类。
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 加载视图
    /// </summary>
    public static LoadingView loadingView;
    /// <summary>
    /// 全局视图
    /// </summary>
    public static GlobalView globalView;

    /// <summary>
    /// 初始化游戏。
    /// 本方法在进入游戏时自动执行。
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateGameManager()
    {
        Addressables.InstantiateAsync("LoadingView");
        Addressables.InstantiateAsync("GameManager");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(InitGame());
    }

    private IEnumerator InitGame()
    {
        yield return FrameEntry.Init();
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
