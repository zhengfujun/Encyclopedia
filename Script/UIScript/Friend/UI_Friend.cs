using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using common;

public class UI_Friend : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public int CurShowPanelType = 0;
    public UIToggle Type_1;
    public UIToggle Type_2;
    public UIToggle Type_3;

    /////////////////////////////////////////

    public GameObject NoFriendsHintRoot;

    public GameObject MyFriendUnitPrefab;
    public UIGrid MyFriendGrid;

    public GameObject MyFriendLstCover;

    public UILabel MyFriendNumLab;
    public UIButton AllGiveBtn;

    public GameObject MyFriendDetailRoot;
    public UISprite Detail_HeadPortrait;
    public UILabel Detail_ID;
    public UILabel Detail_Name;
    public UILabel Detail_Stage;

    public GameObject AffirmDeleteRoot;
    public UISprite AffirmDelete_HeadPortrait;
    public UILabel AffirmDelete_Name;

    /////////////////////////////////////////

    public GameObject SearchFailureHintRoot;

    public GameObject AddFriendUnitPrefab;
    public UIGrid AddFriendGrid;

    public GameObject AddFriendLstCover;

    public UIInput PlayerIDInput;
    public UIButton AddAllBtn;
    public UIButton RefreshBtn;
    public UIButton BackBtn;

    /////////////////////////////////////////

    public GameObject NoApplyHintRoot;

    public GameObject ApplyUnitPrefab;
    public UIGrid ApplyGrid;

    public GameObject ApplyLstCover;

    public UIButton AgreeAllBtn;
    public UIButton IgnoreAllBtn;

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
        if (Input.GetKeyUp(KeyCode.B))
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

            if (!Type_1.value)
                Type_1.value = true;

            List<FriendInfo> TempMyFriendLst = new List<FriendInfo>()
            {
                new FriendInfo(10001,"认真的茨纳米",4,1010101),
                new FriendInfo(10002,"勇敢的驮多",5,1010103),
                new FriendInfo(10003,"果断的酷雷伏",2,1010104),
                new FriendInfo(10004,"健谈的阿罗",7,1010102),
                new FriendInfo(10005,"进取的飞影",8,1010201),
                new FriendInfo(10006,"守信的风鹰",6,1010301),
                new FriendInfo(10007,"刚毅的卡魄",1,1010105),
                new FriendInfo(10008,"实际的埃戈士",9,1010301),
                new FriendInfo(10009,"高尚的黑犀",3,1010106),
                new FriendInfo(10010,"清廉的雪獒",7,1010104)
            };
            StartCoroutine("RefreshMyFriendList", TempMyFriendLst);
        }
        else
        {
            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);

                if (Type_2.value)
                {
                    RefreshBtn.gameObject.SetActive(true);
                    BackBtn.gameObject.SetActive(false);

                    SearchFailureHintRoot.SetActive(false);
                    AddFriendGrid.transform.parent.gameObject.SetActive(true);
                }
                else if (Type_3.value)
                {
                    NoApplyHintRoot.SetActive(false);
                }
            });
        }
    }

    IEnumerator RefreshMyFriendList(List<FriendInfo> RILst)
    {
        MyFriendLstCover.SetActive(true);
        MyTools.DestroyImmediateChildNodes(MyFriendGrid.transform);
        UIScrollView sv = MyFriendGrid.transform.parent.GetComponent<UIScrollView>();
        for (int i = 0; i < RILst.Count; i++)
        {
            GameObject newUnit = NGUITools.AddChild(MyFriendGrid.gameObject, MyFriendUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "MyFriendUnit_" + i;
            newUnit.transform.localPosition = new Vector3(0, -110 * i, 0);

            UI_FriendUnit fu = newUnit.GetComponent<UI_FriendUnit>();
            fu.Set(RILst[i]);

            MyFriendGrid.repositionNow = true;
            sv.ResetPosition();

            yield return new WaitForEndOfFrame();
        }
        MyFriendLstCover.SetActive(false);

        RefreshMyFriendBottomInfo();
    }

    public void RefreshMyFriendBottomInfo()
    {
        int MyFriendNum = MyFriendGrid.transform.childCount;
        MyFriendNumLab.text = MyFriendNum.ToString();
        NoFriendsHintRoot.SetActive(MyFriendNum == 0);

        bool haveFriendEnableGive = false;
        for (int i = 0; i < MyFriendNum; i++)
        {
            Transform child = MyFriendGrid.transform.GetChild(i);
            UI_FriendUnit fu = child.GetComponent<UI_FriendUnit>();
            if(!fu.CurFI.IsGived)
            {
                haveFriendEnableGive = true;
                break;
            }
        }
        AllGiveBtn.isEnabled = haveFriendEnableGive;
    }

    IEnumerator RefreshAddFriendList(List<FriendInfo> RILst)
    {
        AddFriendLstCover.SetActive(true);
        MyTools.DestroyImmediateChildNodes(AddFriendGrid.transform);
        UIScrollView sv = AddFriendGrid.transform.parent.GetComponent<UIScrollView>();
        for (int i = 0; i < RILst.Count; i++)
        {
            GameObject newUnit = NGUITools.AddChild(AddFriendGrid.gameObject, AddFriendUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "AddFriendUnit_" + i;
            newUnit.transform.localPosition = new Vector3(0, -110 * i, 0);

            UI_FriendUnit fu = newUnit.GetComponent<UI_FriendUnit>();
            fu.Set(RILst[i]);

            AddFriendGrid.repositionNow = true;
            sv.ResetPosition();

            yield return new WaitForEndOfFrame();
        }
        AddFriendLstCover.SetActive(false);

        RefreshAddFriendBottomInfo();
    }

    public void RefreshAddFriendBottomInfo()
    {
        bool haveFriendEnableAdd = false;
        for (int i = 0; i < AddFriendGrid.transform.childCount; i++)
        {
            Transform child = AddFriendGrid.transform.GetChild(i);
            UI_FriendUnit fu = child.GetComponent<UI_FriendUnit>();
            if (!fu.CurFI.IsAdded)
            {
                haveFriendEnableAdd = true;
                break;
            }
        }
        AddAllBtn.isEnabled = haveFriendEnableAdd;
    }

    IEnumerator RefreshApplyList(List<FriendInfo> RILst)
    {
        ApplyLstCover.SetActive(true);
        MyTools.DestroyImmediateChildNodes(ApplyGrid.transform);
        UIScrollView sv = ApplyGrid.transform.parent.GetComponent<UIScrollView>();
        for (int i = 0; i < RILst.Count; i++)
        {
            GameObject newUnit = NGUITools.AddChild(ApplyGrid.gameObject, ApplyUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "ApplyUnit_" + i;
            newUnit.transform.localPosition = new Vector3(0, -110 * i, 0);

            UI_FriendUnit fu = newUnit.GetComponent<UI_FriendUnit>();
            fu.Set(RILst[i]);

            ApplyGrid.repositionNow = true;
            sv.ResetPosition();

            yield return new WaitForEndOfFrame();
        }
        ApplyLstCover.SetActive(false);

        RefreshApplyBottomInfo();
    }

    public void RefreshApplyBottomInfo()
    {
        ApplyGrid.enabled = true;

        int ApplyNum = ApplyGrid.transform.childCount;
        if (ApplyNum == 0)
        {
            NoApplyHintRoot.SetActive(true);
            AgreeAllBtn.isEnabled = false;
            IgnoreAllBtn.isEnabled = false;
        }
        else
        {
            NoApplyHintRoot.SetActive(false);
            AgreeAllBtn.isEnabled = true;
            IgnoreAllBtn.isEnabled = true;
        }
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }
    /// <summary> 分类标签 </summary>
    public void OnTypeToggleChange()
    {
        if (UIToggle.current.value)
        {
            switch (UIToggle.current.name)
            {
                case "Type_1":
                    CurShowPanelType = 1;

                   
                    break;
                case "Type_2":
                    CurShowPanelType = 2;

                    OnClick_RefreshRecommendFriends();
                    break;
                case "Type_3":
                    CurShowPanelType = 3;

                    List<FriendInfo> TempApplyLst = new List<FriendInfo>()
                    {
                        new FriendInfo(30001,"时光凤凰",4,1010101),
                        new FriendInfo(30002,"寒冰守护者",5,1010103),
                        new FriendInfo(30003,"符文射手",6,1010104)
                    };
                    StartCoroutine("RefreshApplyList", TempApplyLst);
                    break;
            }
            Debug.Log(UIToggle.current.name);
        }
    }

    /// <summary> 点击全部赠送 </summary>
    public void OnClick_AllGive()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【全部赠送】");

        for (int i = 0; i < MyFriendGrid.transform.childCount; i++)
        {
            Transform child = MyFriendGrid.transform.GetChild(i);
            UI_FriendUnit fu = child.GetComponent<UI_FriendUnit>();
            fu.SetGiveState(true);
        }
        GameApp.Instance.CommonHintDlg.OpenHintBox("赠送的礼物已发出");
    }

    /// <summary> 点击全部添加 </summary>
    public void OnClick_AddAll()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【全部添加】");

        for (int i = 0; i < AddFriendGrid.transform.childCount; i++)
        {
            Transform child = AddFriendGrid.transform.GetChild(i);
            UI_FriendUnit fu = child.GetComponent<UI_FriendUnit>();
            fu.SetAddState(true);
        }
        GameApp.Instance.CommonHintDlg.OpenHintBox("好友请求已发送");
    }
    /// <summary> 点击换一批 </summary>
    public void OnClick_RefreshRecommendFriends()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【换一批】");

        List<FriendInfo> TempAddFriendLst = new List<FriendInfo>()
            {
                new FriendInfo(20001,"暴走米魔灵",1,1010101),
                new FriendInfo(20002,"生化布尔",2,1010103),
                new FriendInfo(20003,"潮汐守护者",3,1010104),
                new FriendInfo(20004,"暗黑射手",4,1010102),
                new FriendInfo(20005,"水晶法师",5,1010201),
                new FriendInfo(20006,"托马斯斥候",6,1010301),
                new FriendInfo(20007,"暗影剑圣",7,1010105),
                new FriendInfo(20008,"查德巫灵",8,1010301),
                new FriendInfo(20009,"邪恶大师",9,1010104),
                new FriendInfo(20010,"蒸汽之刃",1,1010102),
                new FriendInfo(20011,"冰晶巨兽",2,1010201),
                new FriendInfo(20012,"时光死神",3,1010301),
                new FriendInfo(20013,"寒冰先锋",4,1010105)
            };
        List<FriendInfo> RandomAddFriendLst = new List<FriendInfo>();
        int[] RandomIndexLst = MyTools.GetRandomNumArray4Barring(5,TempAddFriendLst.Count,-1);
        for(int i = 0; i < RandomIndexLst.Length; i++)
        {
            RandomAddFriendLst.Add(TempAddFriendLst[RandomIndexLst[i]]);
        }
        StartCoroutine("RefreshAddFriendList", RandomAddFriendLst);
    }
    /// <summary> 点击查找返回 </summary>
    public void OnClick_BackFromSearch()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【查找返回】");

        OnClick_RefreshRecommendFriends();

        RefreshBtn.gameObject.SetActive(true);
        BackBtn.gameObject.SetActive(false);

        SearchFailureHintRoot.SetActive(false);
        AddFriendGrid.transform.parent.gameObject.SetActive(true);
    }
    /// <summary> 点击查找 </summary>
    public void OnClick_Search()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【查找】 玩家ID：" + PlayerIDInput.value);

        if (PlayerIDInput.value.Length == 0)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("请先输入有效ID！");
            return;
        }

        AddAllBtn.isEnabled = false;
        RefreshBtn.gameObject.SetActive(false);
        BackBtn.gameObject.SetActive(true);

        SearchFailureHintRoot.SetActive(true);
        AddFriendGrid.transform.parent.gameObject.SetActive(false);
    }
    /// <summary> 点击清空带查找ID </summary>
    public void OnClick_ClearPlayerID()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【清空带查找ID】");

        PlayerIDInput.value = "";
    }

    /// <summary> 点击全部同意 </summary>
    public void OnClick_AgreeAll()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【全部同意】");

        MyTools.DestroyImmediateChildNodes(ApplyGrid.transform);
        GameApp.Instance.CommonHintDlg.OpenHintBox("添加好友成功");
        RefreshApplyBottomInfo();
    }
    /// <summary> 点击全部忽略 </summary>
    public void OnClick_IgnoreAll()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【全部忽略】");

        MyTools.DestroyImmediateChildNodes(ApplyGrid.transform);
        GameApp.Instance.CommonHintDlg.OpenHintBox("已忽略好友请求");
        RefreshApplyBottomInfo();
    }
    #endregion

    #region _我的好友详情
    private UI_FriendUnit CurClickFriend = null;
    public void ShowMyFriendDetail(UI_FriendUnit fu)
    {
        CurClickFriend = fu;
        Detail_ID.text = StringBuilderTool.ToString("ID: ", CurClickFriend.CurFI.PlayerID);
        Detail_Name.text = CurClickFriend.Name.text;


        int RoleID = Math.Max(CurClickFriend.CurFI.RoleID, 1);
        RoleConfig rc = null;
        if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue(RoleID, out rc))
        {
            Detail_HeadPortrait.spriteName = rc.PortraitEx;
        }

        string StageStr = CurClickFriend.Stage.text;
        string[] tempSS = StageStr.Split('-');
        Detail_Stage.text = StringBuilderTool.ToInfoString(tempSS[0], "-", tempSS[1], "\n", tempSS[2]);

        TweenAlpha.Begin(MyFriendDetailRoot, 0.2f, 1).from = 0;
    }
    public void OnClick_CloseMyFriendDetail()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭我的好友详情界面】");

        TweenAlpha.Begin(MyFriendDetailRoot, 0.2f, 0).from = 1;
    }
    public void OnClick_GotoAffirmDelete()
    {
        Debug.Log("点击【前往确认删除】");

        ShowAffirmDelete();
    }
    #endregion

    #region _删除确认
    public void ShowAffirmDelete()
    {
        AffirmDelete_Name.text = Detail_Name.text;
        AffirmDelete_HeadPortrait.spriteName = Detail_HeadPortrait.spriteName;

        TweenAlpha.Begin(AffirmDeleteRoot, 0.2f, 1).from = 0;
    }
    public void OnClick_CloseAffirmDelete()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭删除确认界面】");

        TweenAlpha.Begin(AffirmDeleteRoot, 0.2f, 0).from = 1;
    }
    public void OnClick_DeleteMyFriend()
    {
        Debug.Log("点击【删除我的好友】");

        TweenAlpha.Begin(MyFriendDetailRoot, 0.2f, 0).from = 1;
        TweenAlpha.Begin(AffirmDeleteRoot, 0.2f, 0).from = 1;

        DestroyImmediate(CurClickFriend.gameObject);
        MyFriendGrid.enabled = true;

        RefreshMyFriendBottomInfo();
    }
    #endregion
}
