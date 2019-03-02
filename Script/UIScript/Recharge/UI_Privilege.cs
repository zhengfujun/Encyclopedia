using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Privilege : MonoBehaviour
{
    public EPrivilegeType PrivilegeType;
    public int ItemID;

    public UILabel IconTextLab;
    public UILabel PriceLab;
    public UILabel DesLab;
    public UILabel ResidueTimeDesLab;

    public GameObject SelObj;

    private GoodsInfo CurGoodsInfo = null;

    bool isNet()
    {
        return (GameApp.Instance.Platform != null && GameApp.Instance.LeSDKInstance != null);
    }
    bool GetGoodsInfo()
    {
        foreach (KeyValuePair<int, GoodsInfo> pair in GameApp.Instance.Platform.GoodsDic)
        {
            if (pair.Value.CfgItemID == ItemID)
            {
                CurGoodsInfo = pair.Value;
                return true;
            }
        }
        return false;
    }

    public void Refresh()
    {
        switch (PrivilegeType)
        {
            case EPrivilegeType.eMonth:
                IconTextLab.text = "月";
                DesLab.text = "金色扭蛋币：3个/每天\n金币：100个/每天\n糖果：100个/每天";
                if (isNet())
                {
                    if (GetGoodsInfo())
                    {
                        PriceLab.text = StringBuilderTool.ToInfoString("¥", CurGoodsInfo.GetPriceStr());
                    }
                }
                else
                {
                    PriceLab.text = "¥30";
                }
                break;
            case EPrivilegeType.eSeason:
                IconTextLab.text = "季";
                DesLab.text = "金色扭蛋币：4个/每天\n金币：200个/每天\n糖果：200个/每天";
                if (isNet())
                {
                    if (GetGoodsInfo())
                    {
                        PriceLab.text = StringBuilderTool.ToInfoString("¥", CurGoodsInfo.GetPriceStr());
                    }
                }
                else
                {
                    PriceLab.text = "¥68";
                }
                break;
            case EPrivilegeType.eYear:
                IconTextLab.text = "年";
                DesLab.text = "金色扭蛋币：5个/每天\n金币：300个/每天\n糖果：300个/每天";
                if (isNet())
                {
                    if (GetGoodsInfo())
                    {
                        PriceLab.text = StringBuilderTool.ToInfoString("¥", CurGoodsInfo.GetPriceStr());
                    }
                }
                else
                {
                    PriceLab.text = "¥198";
                }
                break;
        }

        RefreshResidueTime();
    }

    public void RefreshResidueTime()
    {
        uint ItemCount = SerPlayerData.GetItemCount(ItemID);
        if(ItemCount > 0)
        {
            string des = "";
            int DaySum = 0;
            uint AccountID = SerPlayerData.GetAccountID();
            string itemKey = "";
            string awardKey = "";
            switch (PrivilegeType)
            {
                case EPrivilegeType.eMonth:
                    des = "月";
                    DaySum = 30 * (int)ItemCount;
                    itemKey = StringBuilderTool.ToString(AccountID, "_Privilege_Month");
                    awardKey = StringBuilderTool.ToString(AccountID, "_Award_Month_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    break;
                case EPrivilegeType.eSeason:
                    des = "季";
                    DaySum = 90 * (int)ItemCount;
                    itemKey = StringBuilderTool.ToString(AccountID, "_Privilege_Season");
                    awardKey = StringBuilderTool.ToString(AccountID, "_Award_Season_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    break;
                case EPrivilegeType.eYear:
                    des = "年";
                    DaySum = 365 * (int)ItemCount;
                    itemKey = StringBuilderTool.ToString(AccountID, "_Privilege_Year");
                    awardKey = StringBuilderTool.ToString(AccountID, "_Award_Year_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); 
                    break;
            }

            if (ItemCount == 1)
            {
                PlayerPrefs.SetString(itemKey, DateTime.Now.Ticks.ToString());
            }

            string RecordFirstItemTime = PlayerPrefs.GetString(itemKey);
            DateTime dtFirstItemTime = new DateTime(long.Parse(RecordFirstItemTime));
            TimeSpan ResidueTime = DateTime.Now - dtFirstItemTime;
            DaySum -= ResidueTime.Days;
            ResidueTimeDesLab.text = StringBuilderTool.ToString("[6BFF78]", des, "卡特权已开通[-]([FEE209]", DaySum, "[-]天)");

            if (!PlayerPrefs.HasKey(awardKey))
            {
                PlayerPrefs.SetInt(awardKey, 1);

                Dictionary<int, int> AwardItemDic = new Dictionary<int, int>();
                switch (PrivilegeType)
                {
                    case EPrivilegeType.eMonth:
                        AwardItemDic.Add(20002, 3);
                        AwardItemDic.Add(1001, 100);
                        AwardItemDic.Add(1003, 100);
                        break;
                    case EPrivilegeType.eSeason:
                        AwardItemDic.Add(20002, 4);
                        AwardItemDic.Add(1001, 200);
                        AwardItemDic.Add(1003, 200);
                        break;
                    case EPrivilegeType.eYear:
                        AwardItemDic.Add(20002, 5);
                        AwardItemDic.Add(1001, 300);
                        AwardItemDic.Add(1003, 300);
                        break;
                }
                foreach (KeyValuePair<int, int> pair in AwardItemDic)
                {
                    GameApp.SendMsg.GMOrder(StringBuilderTool.ToString("AddItem ", pair.Key, " ", pair.Value));
                }
                GameApp.Instance.GetItemsDlg.OpenGetItemsBox(AwardItemDic);
            }
        }
        else
        {
            ResidueTimeDesLab.text = "[C74343]未开通[-]";
        }
    }

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            GameApp.SendMsg.GMOrder("AddItem 2002 1");
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            RefreshResidueTime();
        }
    }*/

    /// <summary> 点击购买 </summary>
    public void OnClick_Buy()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【购买】");

#if UNITY_ANDROID
        if (isNet())
            GameApp.Instance.Platform.CreateOrder(CurGoodsInfo.ID);
#elif UNITY_IPHONE
        GameApp.Instance.CommonHintDlg.OpenHintBox("", EHintBoxStyle.eStyle_ComingSoon);
#endif
    }
}
