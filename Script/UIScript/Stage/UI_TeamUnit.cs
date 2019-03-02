using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETeamMemberType
{
    eOwner,     //房主
    eBeReady,   //已准备
    eNotReady,  //为准备
    eInvite,    //邀请
    eWaitJoin,  //等待加入
    eNull
}

public class UI_TeamUnit : MonoBehaviour
{
    public UISprite Bg;
    public UISprite Portrait;
    public UISprite PortraitBg;
    public UILabel Name;

    public GameObject[] TypeObjs;

    public ETeamMemberType TeamMemberType = ETeamMemberType.eWaitJoin;

    [HideInInspector]
    public bool IsValid = false;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Set(string PlayerName, int RoleID, ETeamMemberType TMType)
    {
        IsValid = (PlayerName.Length > 0);

        Name.text = PlayerName;

        int _RoleID = Math.Max(RoleID, 1);
        RoleConfig rc = null;
        if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue(_RoleID, out rc))
        {
            Portrait.spriteName = rc.PortraitEx;
        }

        TeamMemberType = TMType;
        switch(TeamMemberType)
        {
            case ETeamMemberType.eOwner:
                Bg.spriteName = "bg_yeqian_1";
                Portrait.gameObject.SetActive(true);
                PortraitBg.gameObject.SetActive(true);
                PortraitBg.spriteName = "bg_touxiang_zj";
                break;
            case ETeamMemberType.eBeReady:
                Bg.spriteName = "bg_yeqian_2";
                Portrait.gameObject.SetActive(true);
                PortraitBg.gameObject.SetActive(true);
                PortraitBg.spriteName = "bg_touxiang";
                break;
            case ETeamMemberType.eNotReady:
                Bg.spriteName = "bg_yeqian_2";
                Portrait.gameObject.SetActive(true);
                PortraitBg.gameObject.SetActive(true);
                PortraitBg.spriteName = "bg_touxiang";
                break;
            case ETeamMemberType.eInvite:
                Bg.spriteName = "bg_yeqian_2";
                Portrait.gameObject.SetActive(false);
                PortraitBg.gameObject.SetActive(true);
                Name.text = "等待玩家进入...";
                PortraitBg.spriteName = "bg_touxiang";
                break;
            case ETeamMemberType.eWaitJoin:
                Bg.spriteName = "bg_yeqian_2";
                Portrait.gameObject.SetActive(false);
                PortraitBg.gameObject.SetActive(false);
                Name.text = "等待玩家进入...";
                break;
        }

        for(int i = 0; i < TypeObjs.Length; i++)
        {
            TypeObjs[i].SetActive(i == (int)TeamMemberType);
        }

        gameObject.SetActive(true);
    }

    /// <summary> 邀请 </summary>
    public void OnClick_Invite()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【邀请】");

        if (GameApp.Instance.FirstChessGuideUI != null)
            GameApp.Instance.FirstChessGuideUI.SetGuideState(6);

        GameApp.Instance.HomePageUI.StageMapUI.ShowInviteFriend();
    }
}
