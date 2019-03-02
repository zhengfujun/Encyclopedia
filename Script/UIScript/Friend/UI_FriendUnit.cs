using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common;

public class FriendInfo
{
    public int PlayerID;
    public string Name;
    public int RoleID;
    public int StageProgress;
    public bool IsGived;
    public bool IsAdded;

    public FriendInfo(int _PlayerID, string _Name, int _RoleID, int _StageProgress)
    {
        PlayerID = _PlayerID;
        Name = _Name;
        RoleID = _RoleID;
        StageProgress = _StageProgress;
        IsGived = false;
        IsAdded = false;
    }
}

public class UI_FriendUnit : MonoBehaviour
{
    [HideInInspector]
    public FriendInfo CurFI = null;

    public UISprite HeadPortrait;
    public UILabel Name;
    public UILabel Stage;
    public UIButton GiveBtn;
    public UIButton AddBtn;
    public UIButton InviteBtn;

    public void Set(FriendInfo fi)
    {
        CurFI = fi;

        Name.text = CurFI.Name;

        int RoleID = Math.Max(fi.RoleID, 1);
        RoleConfig rc = null;
        if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue(RoleID, out rc))
        {
            HeadPortrait.spriteName = rc.Portrait;
        }

        StageConfig sc = null;
        CsvConfigTables.Instance.StageCsvDic.TryGetValue(CurFI.StageProgress, out sc);
        if (sc != null)
        {
            ChapterConfig cc = null;
            CsvConfigTables.Instance.ChapterCsvDic.TryGetValue(sc.ChapterID, out cc);

            GroupConfig gc = null;
            CsvConfigTables.Instance.GroupCsvDic.TryGetValue(sc.GroupID, out gc);
            if (cc != null && gc != null)
            {
                Stage.text = StringBuilderTool.ToInfoString(cc.Name, "-", gc.Name, "-", sc.Name);
            }
        }        
    }

    public void SetGiveState(bool isGived)
    {
        if(isGived)
        {
            GiveBtn.isEnabled = false;
            GiveBtn.transform.Find("Des").GetComponent<UILabel>().text = "已赠送";
        }
        else
        {
            GiveBtn.isEnabled = true;
            GiveBtn.transform.Find("Des").GetComponent<UILabel>().text = "赠送";
        }

        CurFI.IsGived = isGived;
    }

    public void SetAddState(bool isAdded)
    {
        if (isAdded)
        {
            AddBtn.isEnabled = false;
            AddBtn.transform.Find("Icon").gameObject.SetActive(false);
            AddBtn.transform.Find("Des").gameObject.SetActive(true);
        }
        else
        {
            
        }

        CurFI.IsAdded = isAdded;
    }

    public void OnClick()
    {
        if (AddBtn != null)
            return;

        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【查看好友详细信息】");

        GameApp.Instance.HomePageUI.FriendUI.ShowMyFriendDetail(this);
    }

    public void OnClick_Give()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【赠送】");

        SetGiveState(true);
        GameApp.Instance.CommonHintDlg.OpenHintBox("赠送的礼物已发出");

        GameApp.Instance.HomePageUI.FriendUI.RefreshMyFriendBottomInfo();
    }

    public void OnClick_Add()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【添加】");

        SetAddState(true);
        GameApp.Instance.CommonHintDlg.OpenHintBox("好友请求已发送");

        GameApp.Instance.HomePageUI.FriendUI.RefreshAddFriendBottomInfo();
    }

    public void OnClick_Agree()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【同意】");

        DestroyImmediate(gameObject);
        GameApp.Instance.CommonHintDlg.OpenHintBox("添加好友成功");

        GameApp.Instance.HomePageUI.FriendUI.RefreshApplyBottomInfo();
    }
    public void OnClick_Ignore()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【忽略】");

        DestroyImmediate(gameObject);
        GameApp.Instance.CommonHintDlg.OpenHintBox("已忽略好友请求");

        GameApp.Instance.HomePageUI.FriendUI.RefreshApplyBottomInfo();
    }

    public void OnClick_Invite()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【邀请】");

        InviteBtn.isEnabled = false;
        GameApp.Instance.CommonHintDlg.OpenHintBox("邀请请求已发送");

        if (CurFI.RoleID == 1001)
        {
            if (GameApp.Instance.FirstChessGuideUI != null)
            {
                GameApp.Instance.HomePageUI.StageMapUI.OnClick_CloseInviteFriend();
                GameApp.Instance.FirstChessGuideUI.SetGuideState(7);
            }

            GameApp.Instance.CurRoomPlayerLst.Add(new PVE_Room_Player((ulong)GameApp.Instance.AIRobotData.RoleID, GameApp.Instance.AIRobotData.Name, GameApp.Instance.AIRobotData.RoleID, true, 0,0,0));
            GameApp.Instance.HomePageUI.StageMapUI.RefreshRoomInfo();
        }
    }
}
