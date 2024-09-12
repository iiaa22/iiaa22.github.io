using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 所有视图的父类。
/// </summary>
public abstract class BaseView : MonoBehaviour
{
    /// <summary>
    /// 是否为浮动视图
    /// </summary>
    [Tooltip("是否为浮动视图")]
    public bool isFloating = false;
    /// <summary>
    /// 视图内的所有按钮
    /// </summary>
    [HideInInspector]
    public List<Button> allButtons = new();
    private Dictionary<Button, UnityAction> buttonClickHandlers = new();
    private bool refreshTag = false;
    private static HashSet<BaseView> openedFloatingViews = new();

    /// <summary>
    /// 视图打开事件
    /// </summary>
    public static event UnityAction<BaseView> OnViewOpen;
    /// <summary>
    /// 视图关闭事件
    /// </summary>
    public static event UnityAction<BaseView> OnViewClose;
    /// <summary>
    /// 按钮点击事件
    /// </summary>
    public static event UnityAction<BaseView, Button> OnButtonClick;

    /// <summary>
    /// 当前视图。
    /// </summary>
    public static BaseView Current { get; private set; }
    /// <summary>
    /// 是否存在打开的视图。
    /// </summary>
    public static bool IsAnyViewOpen => Current != null || openedFloatingViews.Count > 0;

    protected virtual void OnEnable()
    {
        if (IsOpen) return;
        if (isFloating)
        {
            openedFloatingViews.Add(this);
        }
        else
        {
            if (Current != null) Current.Close();
            Current = this;
        }
        transform.GetComponentsInChildren(true, allButtons);
        allButtons.ForEach(button =>
        {
            buttonClickHandlers[button] = () => OnButtonClick?.Invoke(this, button);
            button.onClick.AddListener(buttonClickHandlers[button]);
        });
        OnViewOpen?.Invoke(this);
        FrameEntry.OnGlobalRefresh += CheckRefresh;
        AddListener();
        RealRefresh();
    }

    protected virtual void OnDisable()
    {
        if (!IsOpen) return;
        if (isFloating)
        {
            openedFloatingViews.Remove(this);
        }
        else
        {
            Current = null;
        }
        allButtons.ForEach(button =>
        {
            if (!button.IsDestroyed() && buttonClickHandlers.ContainsKey(button))
            {
                button.onClick.RemoveListener(buttonClickHandlers[button]);
                buttonClickHandlers.Remove(button);
            }
        });
        OnViewClose?.Invoke(this);
        FrameEntry.OnGlobalRefresh -= CheckRefresh;
        RemoveListener();
    }

    /// <summary>
    /// 打开视图。
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 关闭视图。
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 视图是否已经打开。
    /// </summary>
    public bool IsOpen => isFloating ? openedFloatingViews.Contains(this) : Current == this;

    /// <summary>
    /// 刷新视图。
    /// 为避免重复调用方法导致频繁刷新的情况，调用该方法不会立即刷新视图，实际刷新会在最多20毫秒内触发。
    /// </summary>
    public void Refresh()
    {
        refreshTag = true;
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
    /// 实际刷新视图。
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
}
