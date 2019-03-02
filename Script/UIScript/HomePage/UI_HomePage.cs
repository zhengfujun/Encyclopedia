using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//using MyMiniJSON;

public class UI_HomePage : MonoBehaviour
{
    public UI_Role RoleUI;
    public UI_NewMagicBook MagicBookUI;
    public UI_StageMap StageMapUI;
    public UI_GashaponMachine GashaponMachineUI;
    public UI_Achievement AchievementUI;
    public UI_Mail MailUI;
    public UI_Ranking RankingUI;
    public UI_Recharge RechargeUI;
    public UI_SetQuestion SetQuestionUI;
    public UI_Activity ActivityUI;
    public UI_Contest ContestUI;
    public UI_Market MarketUI;
    public UI_Friend FriendUI;
    public UI_NewSetting NewSettingUI;

    public UI_PlayerInfo MainPlayerInfo;

    public UI_Chapter[] ChapterLst;

    public UI_Coloring Coloring;

    public GameObject[] HideForIOS;
    public Transform[] ReSetForIOS;

    void Awake()
    {
        GameApp.Instance.HomePageUI = this;

#if UNITY_IPHONE
        for (int i = 0; i < HideForIOS.Length; i++)
        {
            HideForIOS[i].SetActive(false);
        }
        for (int i = 0; i < ReSetForIOS.Length; i++)
        {
            if (ReSetForIOS[i].name.Contains("Friend"))
                ReSetForIOS[i].localPosition = new Vector3(-260, -60, 0);

            if (ReSetForIOS[i].name.Contains("Mail"))
                ReSetForIOS[i].localPosition = new Vector3(-160, -60, 0);
        }
#endif
    }

    void Start()
    {
        GameApp.Instance.SoundInstance.PlayBgm("BGM_HomePage");

        MainPlayerInfo.UseBigHeadPortrait();
        MainPlayerInfo.Init(GameApp.Instance.MainPlayerData);

        int i = 0;
        foreach (KeyValuePair<int, ChapterConfig> pair in CsvConfigTables.Instance.ChapterCsvDic)
        {
            if (i < ChapterLst.Length)
            {
                ChapterLst[i].Set(pair.Value.ChapterID);
                i++;
            }
        }

        Invoke("CheckPrivilegeAward", 1f);

        if (GameApp.Instance.PlayerData != null)
        {
            bool NeedShowFirstChessGuide = true;
            string key = StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_NeedShow_FirstChessGuide");
            if (PlayerPrefs.HasKey(key))
            {
                NeedShowFirstChessGuide = (PlayerPrefs.GetInt(key) == 1);
            }
            else
            {
                PlayerPrefs.SetInt(key, 1);
            }
            if (NeedShowFirstChessGuide)
            {
                GameObject FirstChessGuideObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UI_FirstChessGuide"));
                if (FirstChessGuideObj != null)
                {
                    UI_FirstChessGuide fcg = FirstChessGuideObj.GetComponent<UI_FirstChessGuide>();
                    if (fcg != null)
                    {
                        fcg.SetGuideState(0);
                    }
                }
            }
        }
    }

    void CheckPrivilegeAward()
    {
        //GameApp.Instance.RollingNoticeDlg.AddRollingNotice("欢迎试玩《魔卡百科》Demo版本！");

        Dictionary<EPrivilegeType, int> PrivilegeLst = new Dictionary<EPrivilegeType, int>();
        PrivilegeLst.Add(EPrivilegeType.eMonth, 2001);
        PrivilegeLst.Add(EPrivilegeType.eSeason, 2002);
        PrivilegeLst.Add(EPrivilegeType.eYear, 2003);
        foreach (KeyValuePair<EPrivilegeType, int> pair in PrivilegeLst)
        {
            uint ItemCount = SerPlayerData.GetItemCount(pair.Value);
            if (ItemCount > 0)
            {
                string des = "";
                uint AccountID = SerPlayerData.GetAccountID();
                string awardKey = "";
                switch (pair.Key)
                {
                    case EPrivilegeType.eMonth:
                        des = "月";
                        awardKey = StringBuilderTool.ToString(AccountID, "_Award_Month_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        break;
                    case EPrivilegeType.eSeason:
                        des = "季";
                        awardKey = StringBuilderTool.ToString(AccountID, "_Award_Season_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        break;
                    case EPrivilegeType.eYear:
                        des = "年";
                        awardKey = StringBuilderTool.ToString(AccountID, "_Award_Year_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        break;
                }

                if (!PlayerPrefs.HasKey(awardKey))
                {
                    PlayerPrefs.SetInt(awardKey, 1);

                    Dictionary<int, int> AwardItemDic = new Dictionary<int, int>();
                    switch (pair.Key)
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
                    foreach (KeyValuePair<int, int> pair2 in AwardItemDic)
                    {
                        GameApp.SendMsg.GMOrder(StringBuilderTool.ToString("AddItem ", pair2.Key, " ", pair2.Value));
                    }
                    GameApp.Instance.GetItemsDlg.OpenGetItemsBox(AwardItemDic);
                    GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString(des,"卡特权每日奖励！"));
                }
            }
        }
    }

    void OnDestroy()
    {
        GameApp.Instance.HomePageUI = null;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            //Dictionary<int, int> AwardItemLst = new Dictionary<int, int>();
            //AwardItemLst.Add(1, 12);
            //AwardItemLst.Add(101, 24);
            //AwardItemLst.Add(105, 35);
            //GameApp.Instance.GetItemsDlg.OpenGetItemsBox(AwardItemLst);

            //GameApp.SendMsg.SetGM(9527);
            //GameApp.SendMsg.SetTaskData("怎么样，有没有保存啊！");
            //GameApp.SendMsg.SaveContestMemInfo("我就试试能不能存上！");
            //GameApp.SendMsg.GetServerTime();
            //GameApp.SendMsg.SetAvatar(9527);
            //GameApp.SendMsg.GMOrder("AddItem 50005 9");
            //GameApp.SendMsg.BuyItem(30001);
            //GameApp.SendMsg.PVEFinish(2);
            
            /*string res = "{\"status\":1,\"platform\":24,\"reqID\":6,\"res\":{\"figureurl\":\"\",\"province\":\"上海\",\"figureurl_qq_1\":\"\",\"nickname\":\"ΦωΦ\",\"yellow_vip_level\":\"0\",\"constellation\":\"\",\"city\":\"浦东新\",\"year\":\"1986\",\"figureurl_1\":\"\",\"figureurl_2\":\"\",\"gender\":\"男\",\"level\":\"0\",\"is_yellow_year_vip\":\"0\",\"is_lost\":0,\"ret\":0,\"vip\":\"0\",\"credential\":{\"openid\":\"312CBBE774A0C5546C535FBA93980632\",\"pf_key\":\"2b0f7269d32030f9da1bc511b892bc7a\",\"pf\":\"openmobile_ios\",\"pay_token\":\"A69201C7E11ACBCCE34E6B7CE46C3ADD\",\"msg\":\"\",\"access_token\":\"AE545D297EF76174FE944C8264BC4730\",\"ret\":0,\"expires_in\":7775999.9992300272},\"figureurl_qq_2\":\"\",\"msg\":\"\",\"is_yellow_vip\":\"0\"},\"action\":1}";

            LitJson.JsonData jsonRes = LitJson.JsonMapper.ToObject(res);
            LitJson.JsonData josnData = jsonRes["res"];
            LitJson.JsonData credentialData = josnData["credential"];

            Debug.Log(josnData["nickname"].ToString());
            Debug.Log(credentialData["openid"].ToString());*/
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            //GameApp.SendMsg.PutItem(30001, 0, 0);
            //GameApp.SendMsg.Compose(common.ComposeType.ComposeType_Item, 50001);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            //GameApp.SendMsg.GetOfflineCandy(1);
            //GameApp.SendMsg.GMOrder("AddItem 50005 6");
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            //GameApp.SendMsg.GMOrder("AddItem 1003 99999");
            //GameApp.SendMsg.GMOrder("AddItem 40002 2");
            //GameApp.SendMsg.GMOrder("AddItem 40010 12");
            //GameApp.SendMsg.Compose(common.ComposeType.ComposeType_Item, 50002);
        }
    }

    public bool IsSomeUIShowing()
    {
        return (RoleUI.AppearEffect.isShowing ||
            MagicBookUI.AppearEffect.isShowing ||
            StageMapUI.AppearEffect.isShowing ||
            GashaponMachineUI.AppearEffect.isShowing ||
            AchievementUI.AppearEffect.isShowing ||
            MailUI.AppearEffect.isShowing ||
            RankingUI.AppearEffect.isShowing ||
            RechargeUI.AppearEffect.isShowing ||
            SetQuestionUI.AppearEffect.isShowing ||
            ActivityUI.AppearEffect.isShowing ||
            ContestUI.AppearEffect.isShowing ||
            MarketUI.AppearEffect.isShowing ||
            FriendUI.AppearEffect.isShowing ||
            NewSettingUI.AppearEffect.isShowing);
    }
    
    #region _按钮
    /// <summary> 点击竞赛 </summary>
    public void OnClick_Contest()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【竞赛】");

        ContestUI.Show(true);
    }
    // <summary> 点击出题 </summary>
    public void OnClick_Question()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【出题】");

        SetQuestionUI.Show(true);
    }
    // <summary> 点击好友 </summary>
    public void OnClick_Friend()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【好友】");

        FriendUI.Show(true);
    }
    // <summary> 点击排行 </summary>
    public void OnClick_Ranking()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【排行】");

        RankingUI.Show(true);
    }
    // <summary> 点击成就 </summary>
    public void OnClick_Achievement()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【成就】");

        AchievementUI.Show(true);
    }
    // <summary> 点击扭蛋 </summary>
    public void OnClick_Gashapon()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【扭蛋】");

        GashaponMachineUI.Show(true);
    }
    // <summary> 点击充值 </summary>
    public void OnClick_Recharge()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【充值】");

        RechargeUI.Show(true);
    }
    /// <summary> 点击设置 </summary>
    public void OnClick_Setting()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【设置】");
        
        NewSettingUI.Show(true);
    }
    /// <summary> 点击头像 </summary>
    public void OnClick_HeadPortrait()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【头像】");

        RoleUI.Show(true);
    }
    /// <summary> 点击魔法书 </summary>
    public void OnClick_MagicBook()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【魔法书】");

        MagicBookUI.Show(true);
    }
    /// <summary> 点击邮件 </summary>
    public void OnClick_Mail()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【邮件】");

        MailUI.Show(true);
    }
    /// <summary> 点击市场 </summary>
    public void OnClick_Market()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【市场】");

        MarketUI.Show(true);
    }
    /// <summary> 点击活动 </summary>
    public void OnClick_Activity()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【活动】");

        ActivityUI.Show(true);
    }
    /// <summary> 点击旅行 </summary>
    public void OnClick_Travel()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【旅行】");

        GameApp.Instance.SceneCtlInstance.ChangeScene(SceneControl.Travel);
    }
    /// <summary> 点击章节 </summary>
    public void OnClick_Chapter(GameObject BtnObj)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        int ChapterID = int.Parse(MyTools.GetLastString(BtnObj.name, '_'));
        Debug.Log("点击第" + ChapterID + "章节");

        /*if (ChapterID == 1)
        {
            GameApp.Instance.SoundInstance.PlaySe("DinosaurRoar");
            GameApp.Instance.SoundInstance.PlaySe("Elephant");

            GameApp.Instance.HomePageUI.StageMapUI.Show(true, ChapterID);

            if (GameApp.Instance.FirstChessGuideUI != null)
                GameApp.Instance.FirstChessGuideUI.SetGuideState(1);
        }
        else
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("", EHintBoxStyle.eStyle_ComingSoon);
        }*/

        if (ChapterID == 1)
        {
            GameApp.Instance.SoundInstance.PlaySe("DinosaurRoar");
            GameApp.Instance.SoundInstance.PlaySe("Elephant");

            if (GameApp.Instance.FirstChessGuideUI != null)
                GameApp.Instance.FirstChessGuideUI.SetGuideState(1);
        }
        GameApp.Instance.HomePageUI.StageMapUI.Show(true, ChapterID);
    }
    #endregion
}
