using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopWindow : BaseWindow
{
    public Transform top;
    public Image progress;
    public TextMeshProUGUI exampleText;
    [TextArea]
    [SerializeField]
    private List<string> textList = new();
    [TextArea]
    [SerializeField]
    private List<string> allTextList = new();
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    protected override void RealRefresh()
    {
        progress.DOKill();
        progress.fillAmount = 0;
        top.gameObject.SetActive(true);
        RefreshText(textList);
        progress.DOFillAmount(1f, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            top.gameObject.SetActive(false);
            RefreshText(allTextList);
        });
    }

    protected override void AddListener()
    {

    }

    protected override void RemoveListener()
    {

    }

    protected override void PlayOpenAnimation()
    {
        rectTransform.anchoredPosition = Input.mousePosition;
        base.PlayOpenAnimation();
    }

    private void RefreshText(List<string> textList)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name.StartsWith("TempText"))
            {
                Destroy(child.gameObject);
            }
        }
        foreach (string text in textList)
        {
            TextMeshProUGUI textUI = Instantiate(exampleText, transform);
            textUI.name = "TempText";
            textUI.text = text;
            textUI.gameObject.SetActive(true);
        }
    }
}
