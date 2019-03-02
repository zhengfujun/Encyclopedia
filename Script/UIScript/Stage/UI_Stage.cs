using UnityEngine;
using System.Collections;

public class UI_Stage : UI_Method
{
    private UISprite NameBg;
    private UISprite Icon;
    private Transform SelSpr;
    private Transform LockSpr;

    private BoxCollider BtnBC;

    //[HideInInspector]
    //public EStageState StageState = EStageState.eNull;

    private StageConfig CurSC = null;

    protected override void Start()
    {
        UIButton btn = GetComponent<UIButton>();
        if (btn != null)
        {
            ///点击自身
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(() =>
            {
                if (CurIndex == -1)
                    return;

                if (GameApp.Instance.FirstChessGuideUI != null)
                    GameApp.Instance.FirstChessGuideUI.SetGuideState(3);

                GameApp.Instance.CurFightStageCfg = CurSC;

                if (GameApp.Instance.PlayerData != null)
                {
                    GameApp.SendMsg.EnableEnterStage((uint)CurSC.StageID);
                }
                else
                {
                    GameApp.Instance.HomePageUI.StageMapUI.ShowSelMode();
                }
            }));
        }

        //SetState(EStageState.eLock);
    }

    public void SetInfo(StageConfig sc)
    {
        Name = transform.Find("Name").GetComponent<UILabel>();
        NameBg = transform.Find("NameBg").GetComponent<UISprite>();
        Icon = transform.Find("Icon").GetComponent<UISprite>();
        SelSpr = transform.Find("Sel");
        LockSpr = transform.Find("Lock");

        CurIndex = int.Parse(MyTools.GetLastString(gameObject.name, '_'));

        BtnBC = GetComponent<BoxCollider>();

        CurSC = sc;
        Name.text = CurSC.Name;
        Icon.spriteName = CurSC.Icon;
    }

    public override void SetState(EStageState State)
    {
        /*bool IsShow = true;
        switch (GameApp.Instance.SelModeUI.CurSelMode)
        {
            case ESelectedArea.Single:
                IsShow = (CurSC.IsShow_Single > 0);
                break;
            case ESelectedArea.Cooperate:
                IsShow = (CurSC.IsShow_Cooperate > 0);
                break;
        }

        if (!IsShow)
        {
            State = EStageState.eLock;

        }

        gameObject.SetActive(IsShow);

        Transform LineObj = transform.parent.Find("Line_" + (CurIndex - 1) + "_" + CurIndex);
        if (LineObj != null)
        {
            LineObj.gameObject.SetActive(IsShow);
        }*/

        //if (StageState != State)
        {
            switch (State)
            {
                case EStageState.eLock:
                    BtnBC.enabled = false;
                    Icon.spriteName = CurSC.Icon + "_disC";
                    NameBg.spriteName = "bg_guanka_ming_disC";
                    SelSpr.gameObject.SetActive(false);
                    LockSpr.gameObject.SetActive(true);
                    break;
                case EStageState.eUnlock:
                    BtnBC.enabled = true;
                    NameBg.spriteName = "bg_guanka_ming";
                    SelSpr.gameObject.SetActive(true);
                    LockSpr.gameObject.SetActive(false);
                    break;
                case EStageState.ePass:
                    BtnBC.enabled = true;
                    NameBg.spriteName = "bg_guanka_ming";
                    SelSpr.gameObject.SetActive(false);
                    LockSpr.gameObject.SetActive(false);
                    break;
            }

            //StageState = State;
        }
    }
}