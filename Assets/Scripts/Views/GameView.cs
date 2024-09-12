using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameView : BaseView
{
    public Transform top;
    public Transform bottom;
    public Transform left;
    public TextMeshProUGUI hpText;

    protected override void AddListener()
    {
        OnViewOpen += OnAnyViewOpen;
        UserDataManager.GetUserData<Player>().OnDataChanged += OnPlayerDataChanged;
    }

    protected override void RemoveListener()
    {
        OnViewOpen -= OnAnyViewOpen;
        UserDataManager.GetUserData<Player>().OnDataChanged -= OnPlayerDataChanged;
    }

    private void OnPlayerDataChanged(Unit userData)
    {
        Refresh();
    }

    protected override void RealRefresh()
    {
        hpText.text = $"HP: {UserDataManager.GetUserData<Player>().HP}/{UserDataManager.GetUserData<Player>().MaxHP}";
    }

    private void OnAnyViewOpen(BaseView view)
    {
        top.gameObject.SetActive(view is PlayView);
        bottom.gameObject.SetActive(view is PlayView);
        left.gameObject.SetActive(view is PlayView);
    }
}
