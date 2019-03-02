using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using common;

public class UI_StageMap : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public int CurChapterID = 0;
    public int CurGroupID = 0;    

    public UILabel ChapterLab;

    public GameObject SelectStageRoot;//选择关卡界面
    public GameObject SelModeRoot;//选择模式界面
    public GameObject StageDetailsRoot;//关卡详情界面

    public UILabel StageDetails_StageName;
    public UITexture StageDetails_StagePic;
    public UI_MagicCard[] StageDetails_FixedAward;

    public UIScrollView GroupScrollView;
    public UILabel[] GroupsLabs;
    public UISprite[] GroupsIcons;

    public GameObject StageGroupRoot;
    public UI_StageGroup JustShowIntro;
    public UI_StageGroup[] StageGroups;

    public UILabel RoomName;
    public UIButton BeginFightBtn;
    public UILabel AutoBeginFightHint;
    private bool PauseAutoBeginFight = false;
    public UI_TeamUnit[] TeamMembers;

    public GameObject InviteFriendRoot;
    public GameObject LstRoot;
    public GameObject NoFriendHintRoot;
    public GameObject SearchFailureHintRoot;
    public GameObject LstSV;
    public UIButton InviteAllBtn;
    public GameObject InviteUnitPrefab;
    public UIGrid InviteGrid;
    public UIInput PlayerIDInput;

    private bool IsOwner = false;

    void Awake()
    {
        for (int i = 0; i < StageGroups.Length; i++)
        {
            string[] split = StageGroups[i].name.Split('_');
            if (split.Length == 3)
            {
                if (split[1].Contains("Open") || split[1].Contains("Close"))
                {
                    StageGroups[i].gameObject.SetActive(false);

                    Transform t = null;
#if UNITY_ANDROID
                    t = StageGroups[i].transform.parent.Find(StringBuilderTool.ToInfoString("StageGroup_Open_", split[2]));
#elif UNITY_IPHONE
                    t = StageGroups[i].transform.parent.Find(StringBuilderTool.ToInfoString("StageGroup_Close_", split[2]));
#endif
                    if (t != null)
                    {
                        t.gameObject.SetActive(true);

                        UI_StageGroup sg = t.GetComponent<UI_StageGroup>();
                        if (sg != null)
                        {
                            StageGroups[i] = sg;
                        }
                    }
                }
            }
        }
    }

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

    public void Show(bool isShow, int ChapterID)
    {
        if (isShow)
        {
            GameApp.Instance.IsFightingRobot = (GameApp.Instance.PlayerData == null);

            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            GameApp.Instance.UICurrency.Show(true);

            GroupScrollView.ResetPosition();

            CurChapterID = ChapterID;
            bool isChapter1 = (ChapterID == 1);
            StageGroupRoot.SetActive(isChapter1);
            JustShowIntro.gameObject.SetActive(!isChapter1);
            if (isChapter1)
                RefreshStageData();
            else
                RefreshStageData4JustShowIntro();
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

    private void RefreshStageData()
    {
        ChapterConfig cc = null;
        if (CsvConfigTables.Instance.ChapterCsvDic.TryGetValue(CurChapterID, out cc))
        {
            ChapterLab.text = cc.Name;

            for (int i = 0; i < GroupsLabs.Length; i++)
            {
                GroupsLabs[i].transform.parent.gameObject.SetActive(false);
            }

            int k = 0;
            foreach (KeyValuePair<int, GroupConfig> pair in CsvConfigTables.Instance.GroupCsvDic)
            {
                if (k < GroupsLabs.Length && pair.Value.GroupID / 100 == CurChapterID)
                {
                    GroupsLabs[k].transform.parent.gameObject.SetActive(true);
                    GroupsLabs[k].text = pair.Value.Name;
                    GroupsIcons[k].spriteName = pair.Value.Icon;

                    if (k < StageGroups.Length)
                    {
                        if (StageGroups[k] != null)
                        {
                            StageGroups[k].gameObject.GetComponent<UIPanel>().alpha = GroupsLabs[k].transform.parent.Find("SelBg").gameObject.activeSelf ? 1 : 0;
                            
                            StageGroups[k].SetStageInfo(CurChapterID, pair.Value.GroupID);
                            StageGroups[k].UpdateStageState();
                        }

                        GameApp.Instance.LoadingDlg.SetLoadingPicName(pair.Value.LoadingPic);
                    }
                    k++;
                }
            }
        }
    }

    private void RefreshStageData4JustShowIntro()
    {
        UISprite[] AllSprs = GroupsLabs[0].transform.parent.parent.GetComponentsInChildren<UISprite>(true);
        for (int i = 0; i < AllSprs.Length; i++)
        {
            if (AllSprs[i].name == "SelBg")
            {
                AllSprs[i].gameObject.SetActive(false);
            }
        }
        GroupsLabs[0].transform.parent.Find("SelBg").gameObject.SetActive(true);

        ChapterConfig cc = null;
        if (CsvConfigTables.Instance.ChapterCsvDic.TryGetValue(CurChapterID, out cc))
        {
            ChapterLab.text = cc.Name;

            for (int i = 0; i < GroupsLabs.Length; i++)
            {
                GroupsLabs[i].transform.parent.gameObject.SetActive(false);
            }

            int k = 0;
            foreach (KeyValuePair<int, GroupConfig> pair in CsvConfigTables.Instance.GroupCsvDic)
            {
                if (k < GroupsLabs.Length && pair.Value.GroupID / 100 == CurChapterID)
                {
                    GroupsLabs[k].transform.parent.gameObject.SetActive(true);
                    GroupsLabs[k].text = pair.Value.Name;
                    GroupsIcons[k].spriteName = pair.Value.Icon;

                    if (k == 0)
                    {
                        if (JustShowIntro != null)
                        {
                            //StageGroups[k].gameObject.GetComponent<UIPanel>().alpha = GroupsLabs[k].transform.parent.Find("SelBg").gameObject.activeSelf ? 1 : 0;

                            JustShowIntro.SetStageInfo(CurChapterID, pair.Value.GroupID);
                            //JustShowIntro.UpdateStageState();
                        }

                        //GameApp.Instance.LoadingDlg.SetLoadingPicName(pair.Value.LoadingPic);
                    }
                    k++;
                }
            }
        }
    }

    public void ShowSelMode()
    {
        TweenAlpha.Begin(SelModeRoot, 0.2f, 1f);
    }

    public void RefreshRoomInfo(bool NoRoom = false/*int RoomID,int StageID*/)
    {
        if ((GameApp.Instance.CurRoomPlayerLst.Count > 0 && DefaultRule.PlayerIDToAccountID(GameApp.Instance.CurRoomPlayerLst[0].id) == GameApp.AccountID) ||
            (GameApp.Instance.PlayerData == null) ||
            NoRoom)
        {
            IsOwner = true;
        }

        GameApp.Instance.CurRoomPlayerLoadStateLst.Clear();
        for (int i = 0; i < TeamMembers.Length; i++)
        {
            TeamMembers[i].Set("", 0, IsOwner ? ETeamMemberType.eInvite : ETeamMemberType.eWaitJoin);
        }
        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
        {
            PVE_Room_Player player = GameApp.Instance.CurRoomPlayerLst[i];

            GameApp.Instance.CurRoomPlayerLoadStateLst.Add(player.id, 0);

            ETeamMemberType TMType = ETeamMemberType.eNull;

            if (i == 0)
            {
                RoomName.text = StringBuilderTool.ToInfoString(player.name, "的房间");
                TMType = ETeamMemberType.eOwner;
            }
            else
            {
                if (player.ready)
                    TMType = ETeamMemberType.eBeReady;
                else
                    TMType = ETeamMemberType.eNotReady;
            }

            TeamMembers[i].Set(player.name, (int)player.icon, TMType);
        }

        BeginFightBtn.isEnabled = (IsOwner && (GameApp.Instance.CurRoomPlayerLst.Count > 1));
    }

    public void ShowStageDetails()
    {
        TweenAlpha.Begin(SelectStageRoot, 0.1f, 0f);
        TweenAlpha.Begin(SelModeRoot, 0.1f, 0f);
        TweenAlpha.Begin(StageDetailsRoot, 0.2f, 1f);

        StageDetails_StageName.text = GroupsLabs[oldShowStageGroupIdx].text + "—" + GameApp.Instance.CurFightStageCfg.Name;

        StageDetails_StagePic.mainTexture = Resources.Load("StageThumb/" + GameApp.Instance.CurFightStageCfg.Thumb) as Texture;

        for (int i = 0; i < StageDetails_FixedAward.Length; i++)
        {
            if (i < GameApp.Instance.CurFightStageCfg.FixedAward.Count)
            {
                StageDetails_FixedAward[i].UnconditionalShow(GameApp.Instance.CurFightStageCfg.FixedAward[i]);
            }
            else
            {
                StageDetails_FixedAward[i].gameObject.SetActive(false);
            }
        }

        ChapterLab.gameObject.SetActive(false);

        if (IsOwner)
        {
            StartCoroutine("_AutoBeginFight");
        }
        else
        {
            AutoBeginFightHint.text = "等待房主开始游戏...";
        }
    }

    IEnumerator _AutoBeginFight()
    {
        int cd = GameApp.Instance.GetParameter("AutoBeginFight");
        while (cd > 0)
        {
            while ((GameApp.Instance.CurRoomPlayerLst.Count == 1))
            {
                AutoBeginFightHint.gameObject.SetActive(false);
                cd = GameApp.Instance.GetParameter("AutoBeginFight");
                yield return new WaitForSeconds(1);
            }
            AutoBeginFightHint.gameObject.SetActive(true);

            if (!PauseAutoBeginFight)
                AutoBeginFightHint.text = StringBuilderTool.ToString((cd--), "秒自动开始游戏");

            if (cd <= 10)
                GameApp.Instance.CommonHintDlg.OpenHintBox(AutoBeginFightHint.text);

            yield return new WaitForSeconds(1);
        }

        OnClick_GotoFight();
    }

    #region _邀请好友
    public void ShowInviteFriend()
    {
        TweenAlpha.Begin(InviteFriendRoot, 0.2f, 1).from = 0;

        List<FriendInfo> TempInviteLst = new List<FriendInfo>()
            {
                new FriendInfo(40001,"宝宝龙",1001,0),
                new FriendInfo(40002,"勇敢的驮多",2,0),
                new FriendInfo(40003,"果断的酷雷伏",7,0),
                new FriendInfo(40004,"健谈的阿罗",5,0),
                new FriendInfo(40005,"进取的飞影",6,0),
                new FriendInfo(40006,"守信的风鹰",8,0),
                new FriendInfo(40007,"刚毅的卡魄",3,0),
                new FriendInfo(40008,"实际的埃戈士",4,0)
            };
        if (GameApp.Instance.CurRoomPlayerLst != null)
        {
            if(GameApp.Instance.CurRoomPlayerLst.Count >= 2)
            {
                TempInviteLst.RemoveAt(0);
            }
        }
        StartCoroutine("RefreshInviteList", TempInviteLst);

        PauseAutoBeginFight = true;
    }
    public void OnClick_CloseInviteFriend()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭邀请好友界面】");

        TweenAlpha.Begin(InviteFriendRoot, 0.2f, 0).AddOnFinished(new EventDelegate(() =>
        {
            LstSV.SetActive(true);
            InviteAllBtn.gameObject.SetActive(true);
            InviteAllBtn.isEnabled = true;

            PlayerIDInput.value = "";

            SearchFailureHintRoot.SetActive(false);
        }));

        PauseAutoBeginFight = false;
    }
    IEnumerator RefreshInviteList(List<FriendInfo> RILst)
    {
        MyTools.DestroyImmediateChildNodes(InviteGrid.transform);
        UIScrollView sv = InviteGrid.transform.parent.GetComponent<UIScrollView>();
        for (int i = 0; i < RILst.Count; i++)
        {
            GameObject newUnit = NGUITools.AddChild(InviteGrid.gameObject, InviteUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "InviteUnit_" + i;
            newUnit.transform.localPosition = new Vector3(i%2 == 1 ? 370 : 0, -110 * i, 0);

            UI_FriendUnit fu = newUnit.GetComponent<UI_FriendUnit>();
            fu.Set(RILst[i]);

            InviteGrid.repositionNow = true;
            sv.ResetPosition();

            yield return new WaitForEndOfFrame();
        }
    }
    public void OnClick_InviteAll()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【全部邀请】");

        for (int i = 0; i < InviteGrid.transform.childCount; i++)
        {
            Transform child = InviteGrid.transform.GetChild(i);
            UI_FriendUnit fu = child.GetComponent<UI_FriendUnit>();
            fu.InviteBtn.isEnabled = false;
        }
        GameApp.Instance.CommonHintDlg.OpenHintBox("邀请请求已发送");

        InviteAllBtn.isEnabled = false;
    }
    public void OnClick_Search()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【查找】 玩家ID：" + PlayerIDInput.value);

        if (PlayerIDInput.value.Length == 0)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("请先输入有效ID！");
            return;
        }

        LstSV.SetActive(false);
        InviteAllBtn.gameObject.SetActive(false);

        SearchFailureHintRoot.SetActive(true);
    }
    public void OnClick_ClearPlayerID()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【清空带查找ID】");

        PlayerIDInput.value = "";
    }
    #endregion

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        if (StageDetailsRoot.GetComponent<UIWidget>().alpha > 0.9f)
        {
            ChapterLab.gameObject.SetActive(true);

            TweenAlpha.Begin(StageDetailsRoot, 0.1f, 0f);
            TweenAlpha.Begin(SelectStageRoot, 0.2f, 1f);

            if (GameApp.Instance.PlayerData != null)
            {
                IsOwner = false;
                GameApp.SendMsg.QuitRoom();
                GameApp.Instance.CurRoomPlayerLst.Clear();
                GameApp.Instance.CommonHintDlg.OpenHintBox("已退出当前房间！");
            }
        }
        else if (SelModeRoot.GetComponent<UIPanel>().alpha > 0.9f)
        {
            TweenAlpha.Begin(SelModeRoot, 0.1f, 0f);
        }
        else
        {
            Show(false, CurChapterID);
        }
    }

    /// <summary> 点击关卡组 </summary>
    private int oldShowStageGroupIdx = 0;
    public void SwitchShowStageGroup(GameObject btnObj)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (GameApp.Instance.FirstChessGuideUI != null)
            GameApp.Instance.FirstChessGuideUI.SetGuideState(2);

        int BtnIdx = int.Parse(MyTools.GetLastString(btnObj.name, '_'));
        int curShowStageGroupIdx = BtnIdx - 1;
        if (curShowStageGroupIdx == oldShowStageGroupIdx)
            return;

        /*bool IsComingSoon = true;
#if UNITY_ANDROID
        IsComingSoon = (curShowStageGroupIdx != 0 && curShowStageGroupIdx != 2);
#elif UNITY_IPHONE
        IsComingSoon = (curShowStageGroupIdx != 0);
#endif
        if (IsComingSoon)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("", EHintBoxStyle.eStyle_ComingSoon);
            //GameApp.Instance.CommonHintDlg.OpenHintBox("需要通关【" + GroupsLabs[curShowStageGroupIdx - 1].text + "】！");
            return;
        }
        //if (StageGroupObjs[curShowStageGroupIdx].GetComponent<UI_StageGroup>().AllStageDotShow)
        //{
        //    return;
        //}*/

        if (CurChapterID == 1)
        {
            TweenAlpha.Begin(StageGroups[oldShowStageGroupIdx].gameObject, 0.1f, 0f);
            TweenAlpha.Begin(StageGroups[curShowStageGroupIdx].gameObject, 0.2f, 1f);
        }
        else
        {
            JustShowIntro.SetStageInfo(CurChapterID, CurChapterID * 100 + BtnIdx);
        }

        oldShowStageGroupIdx = curShowStageGroupIdx;

        UISprite[] AllSprs = btnObj.transform.parent.GetComponentsInChildren<UISprite>(true);
        for (int i = 0; i < AllSprs.Length; i++)
        {
            if (AllSprs[i].name == "SelBg")
            {
                AllSprs[i].gameObject.SetActive(false);
            }
        }
        btnObj.transform.Find("SelBg").gameObject.SetActive(true);
    }
    /// <summary> 点击创建房间 </summary>
    public void OnClick_CreateRoom()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【创建房间】");

        if (GameApp.Instance.FirstChessGuideUI != null)
            GameApp.Instance.FirstChessGuideUI.SetGuideState(4);

        if (GameApp.Instance.PlayerData != null)
        {
            GameApp.SendMsg.CreateRoom((uint)GameApp.Instance.CurFightStageCfg.StageID);
        }
        else
        {
            GameApp.Instance.CurRoomPlayerLst.Clear();
            GameApp.Instance.CurRoomPlayerLst.Add(new PVE_Room_Player((ulong)GameApp.Instance.MainPlayerData.RoleID, GameApp.Instance.MainPlayerData.Name, GameApp.Instance.MainPlayerData.RoleID, true, 0,0,0));
            GameApp.Instance.CurRoomPlayerLst.Add(new PVE_Room_Player((ulong)GameApp.Instance.AIRobotData.RoleID, GameApp.Instance.AIRobotData.Name, GameApp.Instance.AIRobotData.RoleID, true, 0,0,0));
            RefreshRoomInfo();
            ShowStageDetails();
        }
    }
    /// <summary> 点击加入房间 </summary>
    public void OnClick_JoinRoom()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【加入房间】");

        if (GameApp.Instance.PlayerData != null)
        {
            GameApp.SendMsg.JoinRoom((uint)GameApp.Instance.CurFightStageCfg.StageID);
        }
        else
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("单机模式下不支持加入房间操作！");
        }
    }
    /// <summary> 开始游戏 </summary>
    public void OnClick_GotoFight()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【开始游戏】");

        if (GameApp.Instance.FirstChessGuideUI != null)
            GameApp.Instance.FirstChessGuideUI.SetGuideState(8);

        if (GameApp.Instance.PlayerData != null)
        {
            for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
            {
                if(GameApp.Instance.CurRoomPlayerLst[i].icon == 1001)
                {
                    GameApp.Instance.IsFightingRobot = true;

                    GameApp.SendMsg.QuitRoom();
                    GameApp.Instance.CurRoomPlayerLst = new List<PVE_Room_Player>();

                    GameApp.Instance.SceneCtlInstance.ChangeScene(GameApp.Instance.CurFightStageCfg.FightSceneName);
                    break;
                }
            }

            GameApp.SendMsg.StartGame();
        }
        else
        {
            GameApp.Instance.SceneCtlInstance.ChangeScene(GameApp.Instance.CurFightStageCfg.FightSceneName);
        }
    }
    /// <summary> 提醒准备 </summary>
    public void OnClick_RemindPrepare()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【提醒准备】");

    }
    /// <summary> 房间锁定 </summary>
    public void OnValueChange_RoomLock(UIToggle toggle)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【房间锁定】");

        if (toggle.value)
        {

        }
    }
    /// <summary> 查看奖励 </summary>
    public void OnClick_AwardHint()
    {
        Debug.Log("点击【查看奖励】");

        if (GameApp.Instance.FirstChessGuideUI != null)
            GameApp.Instance.FirstChessGuideUI.SetGuideState(5);
    }
    #endregion
}
