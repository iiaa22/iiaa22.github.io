using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayView : BaseView
{
    private void Start()
    {
        UserDataManager.GetUserData<Player>().LearnSkill(new(0));
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
