using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 所有窗口的父类。
/// </summary>
public abstract class BaseWindow : MonoBehaviour
{
    /// <summary>
    /// 是否为浮动窗口
    /// </summary>
    [Tooltip("是否为浮动窗口")]
    public bool isFloating = false;
    /// <summary>
    /// 视图关闭时自动关闭窗口
    /// </summary>
    [Tooltip("视图关闭时自动关闭窗口")]
    public bool autoClose = true;
    /// <summary>
    /// 窗口关闭按钮
    /// </summary>
    [Tooltip("窗口关闭按钮")]
    public Button closeButton;
    protected CanvasGroup canvasGroup;
    private bool refreshTag = false;
    private static HashSet<BaseWindow> openedFloatingWindows = new();

    /// <summary>
    /// 窗口打开事件
    /// </summary>
    public static event UnityAction<BaseWindow> OnWindowOpen;
    /// <summary>
    /// 窗口关闭事件
    /// </summary>
    public static event UnityAction<BaseWindow> OnWindowClose;

    /// <summary>
    /// 当前窗口。
    /// </summary>
    public static BaseWindow Current { get; private set; }
    /// <summary>
    /// 是否存在打开的窗口。
    /// </summary>
    public static bool IsAnyWindowOpen => Current != null || openedFloatingWindows.Count > 0;

    protected virtual void Awake()
    {
        if (!TryGetComponent(out canvasGroup)) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    protected virtual void OnEnable()
    {
        canvasGroup.alpha = 0f;
        FrameEntry.OnGlobalRefresh += CheckRefresh;
        BaseView.OnViewClose += OnViewClose;
        if (closeButton != null) closeButton.onClick.AddListener(Close);
        AddListener();
    }

    protected virtual void OnDisable()
    {
        Close();
        FrameEntry.OnGlobalRefresh -= CheckRefresh;
        BaseView.OnViewClose -= OnViewClose;
        if (closeButton != null) closeButton.onClick.RemoveListener(Close);
        RemoveListener();
    }

    /// <summary>
    /// 打开窗口。
    /// </summary>
    public void Open()
    {
        if (isFloating)
        {
            openedFloatingWindows.Add(this);
        }
        else
        {
            if (Current != null) Current.Close();
            Current = this;
        }
        OnWindowOpen?.Invoke(this);
        PlayOpenAnimation();
        RealRefresh();
    }

    /// <summary>
    /// 关闭窗口。
    /// </summary>
    public void Close()
    {
        if (!IsOpen) return;
        if (isFloating)
        {
            openedFloatingWindows.Remove(this);
        }
        else
        {
            Current = null;
        }
        OnWindowClose?.Invoke(this);
        PlayCloseAnimation();
    }

    /// <summary>
    /// 切换窗口打开状态。
    /// </summary>
    public void OpenOrClose()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    /// <summary>
    /// 窗口是否已经打开
    /// </summary>
    public bool IsOpen => isFloating ? openedFloatingWindows.Contains(this) : Current == this;

    /// <summary>
    /// 刷新窗口。
    /// 为避免重复调用方法导致频繁刷新的情况，调用该方法不会立即刷新窗口，实际刷新会在最多20毫秒内触发。
    /// </summary>
    public void Refresh()
    {
        refreshTag = true;
    }

    /// <summary>
    /// 设置窗口出现动画。
    /// 注意窗口默认不透明度为0，需要将canvasGroup.alpha设为1才能正常显示。
    /// </summary>
    protected virtual void PlayOpenAnimation()
    {
        canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// 设置窗口关闭动画。
    /// </summary>
    protected virtual void PlayCloseAnimation()
    {
        canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// 添加监听器。
    /// </summary>
    protected abstract void AddListener();

    /// <summary>
    /// 移除监听器。
    /// </summary>
    protected abstract void RemoveListener();

    /// <summary>
    /// 实际刷新窗口。
    /// </summary>
    protected abstract void RealRefresh();

    private void CheckRefresh()
    {
        if (refreshTag)
        {
            RealRefresh();
            refreshTag = false;
        }
    }

    private void OnViewClose(BaseView view)
    {
        if (autoClose) Close();
    }
}
