using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingView : BaseView
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.loadingView = this;
    }

    protected override void AddListener()
    {

    }

    protected override void RemoveListener()
    {

    }

    protected override void RealRefresh()
    {

    }
}
