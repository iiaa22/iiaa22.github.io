using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalView : BaseView
{
    public PopWindow popWindow;

    private void Awake()
    {
        GameManager.globalView = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            popWindow.Close();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            popWindow.Open();
        }
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
