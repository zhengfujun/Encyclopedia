using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SetQuestion : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public GameObject SelSystemAndThemeHintRoot;
    public GameObject SetQuestionRoot;
    public GameObject SubmitSucceedHintRoot;

    public GameObject SystemLstCover;
    public UI_ChapterUnit[] SystemLst;

    public UIGrid AlternativeGrid;
    public GameObject AlternativeAnswerUnitPrefab;

    private int CurSystemID = 0;
    private int CurThemeID = 0;

    public UIInput StemInp;
    public UILabel RightAnswerLab;
    public UILabel[] ErrorAnswerLab;

    public UIButton SubmitBtn;

    void Start()
    {
        for (int i = 0; i < SystemLst.Length; i++)
        {
            if (CsvConfigTables.Instance.ChapterCsvDic.ContainsKey(i + 1))
                SystemLst[i].Set(i + 1);
            else
                SystemLst[i].gameObject.SetActive(false);
        }

        InitStemAndAnswer();
    }

    /*void Update()
    {

    }*/

    public void Show(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            GameApp.Instance.UICurrency.OnlyShowState();


        }
        else
        {
            GameApp.Instance.UICurrency.Show(false);

            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }

    public void OnClick_System(UI_ChapterUnit cu)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        for (int i = 0; i < cu.transform.parent.childCount; i++)
        {
            Transform child = cu.transform.parent.GetChild(i);
            child.GetComponent<UI_ChapterUnit>().Bg.spriteName = "bg_yeqian_2";

            if(child.name != cu.transform.name)
            {
                Transform TweenTrans = child.Find("Tween");
                if (TweenTrans.gameObject.activeSelf)
                {
                    UIPlayTween pt = child.GetComponent<UIPlayTween>();
                    pt.Play(false);

                    for (int j = 0; j < TweenTrans.childCount; j++)
                    {
                        Transform child2 = TweenTrans.GetChild(j);
                        if (child2.name == "Bg")
                            continue;
                        child2.GetComponent<UISprite>().spriteName = "fujiaxuanxiang_0";
                    }
                }
            }
        }
        cu.Bg.spriteName = "bg_yeqian_1";

        StartCoroutine("_SwitchSystemCD");

        MyTools.DestroyChildNodes(AlternativeGrid.transform);
    }

    IEnumerator _SwitchSystemCD()
    {
        SystemLstCover.SetActive(true);
        yield return new WaitForSeconds(0.55f);
        SystemLstCover.SetActive(false);
    }

    public void OnClick_Theme(UISprite btn)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        CurThemeID = int.Parse(MyTools.GetLastString(btn.name, '_'));
        CurSystemID = CurThemeID / 100;

        Debug.Log("点击【" + CurThemeID + "主题】");

        for (int i = 0; i < btn.transform.parent.childCount; i++)
        {
            Transform child = btn.transform.parent.GetChild(i);
            if (child.name == "Bg")
                continue;
            child.GetComponent<UISprite>().spriteName = "fujiaxuanxiang_0";
        }
        btn.spriteName = "fujiaxuanxiang_1";

        TweenAlpha.Begin(SelSystemAndThemeHintRoot, 0.2f, 0);
        TweenAlpha.Begin(SetQuestionRoot, 0.2f, 1);

        RefreshAlternativeAnswerList();
    }

    void RefreshAlternativeAnswerList()
    {
        MyTools.DestroyChildNodes(AlternativeGrid.transform);

        foreach (KeyValuePair<int, MagicCardConfig> pair in CsvConfigTables.Instance.MagicCardCsvDic)
        {
            if (pair.Value.SystemID == CurSystemID && pair.Value.ThemeID == CurThemeID)
            {
                GameObject newUnit = NGUITools.AddChild(AlternativeGrid.gameObject, AlternativeAnswerUnitPrefab);
                newUnit.SetActive(true);
                newUnit.name = "AlternativeAnswerUnit_" + pair.Key;

                UI_AlternativeAnswerUnit mc = newUnit.GetComponent<UI_AlternativeAnswerUnit>();
                mc.Set(pair.Value);

                AlternativeGrid.repositionNow = true;
            }
        }

        RefreshAlternativeAnswerBg();
    }

    public UISprite GetAlternativeAnswerBg(string widgetName)
    {
        return AlternativeGrid.transform.Find(widgetName).GetComponent<UISprite>();
    }

    public void OnStemInpValueChange()
    {
        EnableSubmitBtn();
    }

    public void SetAnswer(int SignIdx,string text)
    {
        if(SignIdx == -1)
        {
            for (int i = 0; i < ErrorAnswerLab.Length; i++)
            {
                if(ErrorAnswerLab[i].text == text)
                {
                    ErrorAnswerLab[i].text = "错误答案";
                }
            }
            RightAnswerLab.text = text;
        }
        else if (SignIdx >= 0 && SignIdx < ErrorAnswerLab.Length)
        {
            if (RightAnswerLab.text == text)
            {
                RightAnswerLab.text = "正确答案";
            }
            ErrorAnswerLab[SignIdx].text = text;
        }

        EnableSubmitBtn();
        RefreshAlternativeAnswerBg();
    }

    private void InitStemAndAnswer()
    {
        StemInp.value = "题目：";
        RightAnswerLab.text = "正确答案";
        for (int i = 0; i < ErrorAnswerLab.Length; i++)
        {
            ErrorAnswerLab[i].text = "错误答案";
        }
        EnableSubmitBtn();
    }

    private void EnableSubmitBtn()
    {
        if(StemInp.value == "题目：" || StemInp.value.Length == 0)
        {
            SubmitBtn.isEnabled = false;
            return;
        }

        if (RightAnswerLab.text == "正确答案")
        {
            SubmitBtn.isEnabled = false;
            return;
        }

        for (int i = 0; i < ErrorAnswerLab.Length; i++)
        {
            if (ErrorAnswerLab[i].text == "错误答案")
            {
                SubmitBtn.isEnabled = false;
                return;
            }
        }

        SubmitBtn.isEnabled = true;
    }

    private void RefreshAlternativeAnswerBg()
    {
        for (int i = 0; i < AlternativeGrid.transform.childCount; i++)
        {
            Transform child = AlternativeGrid.transform.GetChild(i);
            UI_AlternativeAnswerUnit aau = child.GetComponent<UI_AlternativeAnswerUnit>();
            aau.Bg.spriteName = "katonganniu_tuhuang";

            if (aau.Name.text == RightAnswerLab.text)
                aau.Bg.spriteName = "katonganniu_lv";
            for (int j = 0; j < ErrorAnswerLab.Length; j++)
            {
                if (aau.Name.text == ErrorAnswerLab[j].text)
                    aau.Bg.spriteName = "katonganniu_juhuang";
            }
        }
    }

    public void OnClick_Submit()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【提交】");

        TweenAlpha.Begin(SubmitSucceedHintRoot, 0.2f, 1);
    }
    public void OnClick_HideSubmitSucceedHint()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【隐藏提交成功提示】");

        TweenAlpha.Begin(SubmitSucceedHintRoot, 0.2f, 0);

        InitStemAndAnswer();
        RefreshAlternativeAnswerBg();
    }
}
