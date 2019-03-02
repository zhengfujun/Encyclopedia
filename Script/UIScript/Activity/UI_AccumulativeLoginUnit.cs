using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AccumulativeLoginUnit : MonoBehaviour
{
    private int CurIndex = 0;

    public UILabel Title;
    public UILabel Schedule;

    public UIButton GetBtn;
    public UILabel GetBtnText;

    public GameObject[] AwardItemRoot;
    public UISprite[] AwardItemIcon;
    public UILabel[] AwardItemCnt;

    Dictionary<int, int> AwardItemDic = new Dictionary<int, int>();

    void Update()
    {

    }

    public void Set(int Index,int CurAL)
    {
        CurIndex = Index;

        Title.text = StringBuilderTool.ToString("累计登录", Index, "天");
        Schedule.text = StringBuilderTool.ToString(CurAL,"/",Index);

        bool IsGet = false;
        string StateStr = "";
        string IsGetStateKey = StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_AccumulativeLogin_IsGetState");
        if (PlayerPrefs.HasKey(IsGetStateKey))
        {
            StateStr = PlayerPrefs.GetString(IsGetStateKey);
            string[] tempSplit = StateStr.Split('_');
            for (int k = 0; k < tempSplit.Length; k++)
            {
                if (int.Parse(tempSplit[k]) == CurIndex)
                {
                    IsGet = true;
                    break;
                }
            }
        }

        if (CurAL - Index == 1 || (CurAL == Index && Index == 1))
        {
            if (IsGet)
            {
                GetBtn.isEnabled = false;
                GetBtnText.text = "已领取";
            }
            else
            {
                GetBtn.isEnabled = true;
                GetBtnText.text = "领取";
            }
        }
        else
        {
            if (IsGet)
            {
                GetBtn.isEnabled = false;
                GetBtnText.text = "已领取";
            }
            else
            {
                GetBtn.isEnabled = false;
                GetBtnText.text = "未完成";
            }
        }

        for(int k =0; k< AwardItemRoot.Length;k++)
        {
            AwardItemRoot[k].SetActive(false);
        }
        ActivityConfig ACfg = null;
        CsvConfigTables.Instance.ActivityCsvDic.TryGetValue(Index, out ACfg);
        if (ACfg != null)
        {
            int i = 0;
            
            AwardItemDic = ACfg.GetAward();
            foreach (KeyValuePair<int, int> pair in AwardItemDic)
            {
                if (i < AwardItemIcon.Length)
                {
                    AwardItemRoot[i].SetActive(true);

                    ItemConfig ItemCfg = null;
                    CsvConfigTables.Instance.ItemCsvDic.TryGetValue(pair.Key, out ItemCfg);
                    if (ItemCfg != null)
                    {
                        if (ItemCfg.Type == 2)
                        {
                            AwardItemIcon[i].enabled = false;

                            UITexture cardIcon = AwardItemIcon[i].gameObject.AddComponent<UITexture>();
                            cardIcon.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("MagicCard/", ItemCfg.GetIcon())) as Texture;
                            cardIcon.width = 40;
                            cardIcon.height = 60;
                            cardIcon.depth = 14;
                        }
                        else
                            AwardItemIcon[i].spriteName = ItemCfg.Icon;
                    }

                    //AwardItemDic[pair.Key] = pair.Value * Index;
                    AwardItemCnt[i].text = pair.Value.ToString();
                }
                i++;
            }
        }
    }

    public void OnClick_Get()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        GameApp.Instance.GetItemsDlg.OpenGetItemsBox(AwardItemDic);

        GetBtn.isEnabled = false;
        GetBtnText.text = "已领取";

        foreach (KeyValuePair<int, int> pair in AwardItemDic)
        {
            GameApp.SendMsg.GMOrder(StringBuilderTool.ToString("AddItem ",pair.Key, " ", pair.Value));
        }

        string IsGetStateKey = StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_AccumulativeLogin_IsGetState");
        if (PlayerPrefs.HasKey(IsGetStateKey))
        {
            string OldState = PlayerPrefs.GetString(IsGetStateKey);
            OldState += StringBuilderTool.ToString("_", CurIndex);
            PlayerPrefs.SetString(IsGetStateKey, OldState);
        }
        else
        {
            PlayerPrefs.SetString(IsGetStateKey, CurIndex.ToString());
        }
    }
}
