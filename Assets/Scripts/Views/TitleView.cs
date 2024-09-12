using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleView : BaseView
{
    public TextMeshProUGUI startButtonText;
    public TextMeshProUGUI loadButtonText;

    protected override void AddListener()
    {

    }

    protected override void RemoveListener()
    {

    }

    protected override void RealRefresh()
    {
        startButtonText.text = "TitleView.StartButton".ToLocalizedString();
        loadButtonText.text = "TitleView.LoadButton".ToLocalizedString();
    }

    public void StartGame()
    {
        GameManager.loadingView.Open();
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadGame()
    {
        List<UserDataManager.UserDataInfo> userDataInfos = UserDataManager.LoadUserDataInfoList(false);
        if (userDataInfos.Count > 0)
        {
            GameManager.loadingView.Open();
            UserDataManager.LoadUserData(userDataInfos[0]);
            SceneManager.LoadSceneAsync(1);
        }
    }
}
