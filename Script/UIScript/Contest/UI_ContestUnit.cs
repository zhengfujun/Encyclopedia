using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ContestUnit : MonoBehaviour
{
    private int Index = 0;

    public UILabel Name;
    public UILabel Time;
    public UISprite Icon;

    public UI_AwardItem HighestItem;

    public UI_AwardItem[] EntranceItem;

    public GameObject IntoBtnObj;
    public GameObject SignUpBtnObj;
    public GameObject ShowConditionBtnObj;
    public GameObject HaveEnteredHintObj;

    void Start()
    {
       
    }

    public void Set(int index, string name, string time, Dictionary<int, int> HighestItemDic, Dictionary<int, int> EntranceItemDic)
    {
        Index = index;

        Name.text = name;
        Time.text = time;

        foreach (KeyValuePair<int, int> pair in HighestItemDic)
        {
            HighestItem.SetItemData(pair.Key, pair.Value);
        }

        int i = 0;
        foreach (KeyValuePair<int, int> pair in EntranceItemDic)
        {
            if (i < EntranceItem.Length)
            {
                EntranceItem[i].SetItemData(pair.Key, pair.Value);
            }
            i++;
        }
        if(EntranceItemDic.Count == 1)
        {
            EntranceItem[0].transform.localPosition = new Vector3(188,0,0);
            EntranceItem[1].gameObject.SetActive(false);
        }

        switch(index)
        {
            case 1:
                IntoBtnObj.SetActive(false);
                SignUpBtnObj.SetActive(true);
                ShowConditionBtnObj.SetActive(false);
                HaveEnteredHintObj.SetActive(false);
                break;
            case 2:
                IntoBtnObj.SetActive(false);
                SignUpBtnObj.SetActive(false);
                ShowConditionBtnObj.SetActive(false);
                HaveEnteredHintObj.SetActive(true);
                break;
            case 3:
                IntoBtnObj.SetActive(true);
                SignUpBtnObj.SetActive(false);
                ShowConditionBtnObj.SetActive(false);
                HaveEnteredHintObj.SetActive(false);
                break;
            case 4:
            case 5:
                IntoBtnObj.SetActive(false);
                SignUpBtnObj.SetActive(false);
                ShowConditionBtnObj.SetActive(true);
                HaveEnteredHintObj.SetActive(false);
                break;
        }
    }

    public void OnClick_ShowDetails()
    {
        GameApp.Instance.HomePageUI.ContestUI.ShowDetails(this);
    }

    public void OnClick_Into()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        GameApp.Instance.HomePageUI.ContestUI.ShowMatching();
    }

    public void OnClick_SignUp()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (IntoBtnObj.activeSelf || ShowConditionBtnObj.activeSelf || HaveEnteredHintObj.activeSelf)
            return;

        SignUpBtnObj.SetActive(false);
        HaveEnteredHintObj.SetActive(true);
    }

    public void OnClick_ShowCondition()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (Index == 4)
            GameApp.Instance.HomePageUI.ContestUI.ShowConditionHint("拥有的角色个数达到10个");
        else if (Index == 5)
            GameApp.Instance.HomePageUI.ContestUI.ShowConditionHint("持有的5星卡牌个数达到20张");
    }

}
