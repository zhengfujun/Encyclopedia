using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum EPrivilegeType
{
    eMonth, //月卡
    eSeason,//季卡
    eYear   //年卡
}

public class UI_Recharge : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public GameObject ParentsValidationRoot;
    public GameObject RechargeRoot;

    public UI_Privilege[] PrivilegeItems;

    public UILabel QuestionLab;
    public UILabel[] AnswerLab;
    private int CurQuestionRightAnswers;

    /*void Awake()
    {

    }*/

    void Start()
    {
        
    }

    /*void OnDestroy()
    {

    }*/

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {

        }
    }*/

    public void Show(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            TweenAlpha.Begin(ParentsValidationRoot, 0, 1);
            TweenAlpha.Begin(RechargeRoot, 0, 0);

            SetValidationQuestion();
        }
        else
        {
            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    public void SetValidationQuestion()
    {
        int VQID = UnityEngine.Random.Range(1, CsvConfigTables.Instance.ValidationLibraryCsvDic.Count);
        ValidationLibraryConfig vlCfg = CsvConfigTables.Instance.ValidationLibraryCsvDic[VQID];
        QuestionLab.text = vlCfg.QuestionStem;
        AnswerLab[0].text = vlCfg.AnswerOptionText_1;
        AnswerLab[1].text = vlCfg.AnswerOptionText_2;
        AnswerLab[2].text = vlCfg.AnswerOptionText_3;
        CurQuestionRightAnswers = vlCfg.RightAnswers;
    }

    IEnumerator WaitShowRechargePanel()
    {
        if (GameApp.Instance.Platform != null)
            GameApp.Instance.Platform.GetGoodsList();

        while (!GameApp.Instance.Platform.IsGetGetGoodsListOver)
        {
            yield return new WaitForEndOfFrame();
        }

        RefreshPrivilege();

        TweenAlpha.Begin(ParentsValidationRoot, 0.1f, 0);
        TweenAlpha.Begin(RechargeRoot, 0.2f, 1);
    }
    public void RefreshPrivilege()
    {
        Debug.Log("RefreshPrivilege 0");
        for (int i = 0; i < PrivilegeItems.Length; i++)
        {
            PrivilegeItems[i].Refresh();
        }
        Debug.Log("RefreshPrivilege 1");
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }
    /// <summary> 点击刷新验证问题 </summary>
    public void OnClick_Refresh()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【刷新验证问题】");

        SetValidationQuestion();
    }
    /// <summary> 点击验证问题的答案 </summary>
    public void OnClick_Answer(GameObject btn)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【验证问题的答案】" + btn.name);

        int AnswerIndex = int.Parse(MyTools.GetLastString(btn.name, '_'));

        if(AnswerIndex == CurQuestionRightAnswers)
        {
            StartCoroutine("WaitShowRechargePanel");
        }
        else
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("回答错误！为您更换一道验证问题");
            SetValidationQuestion();
        }
    }
    #endregion
}
