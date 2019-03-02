using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelControl : MonoBehaviour
{
    public AngelTiePoint TiePoint;
    public AngelOnGrid OnGrid;
    public UI_Angel AngelUI;

    private int ResidueRoundCount = 0;

    /*void Start()
    {
        
    }

    void Update()
    {
        
    }*/

    public void Create(Transform Grid)
    {
        OnGrid.Init(Grid);
    }

    public bool Detection(Transform Grid, Action ShowEndCB)
    {
        if (GameApp.Instance.PlayerData == null || GameApp.Instance.IsFightingRobot)
        {
            if (GameApp.Instance.FightUI.RecordCurRoleType == ERoleType.ePlayer_1)
            {
                //GameApp.Instance.CommonHintDlg.OpenHintBox("暂不处理机器人经过触发红天使事件");
                return false;
            }
        }

        bool stepon = OnGrid.IsStepOn(Grid);
        if (stepon)
        {
            ResidueRoundCount = GameApp.Instance.GetParameter("AngelRoundCount");

            AngelUI.SetResidueRoundCount(ResidueRoundCount);
            AngelUI.Show(ShowEndCB);
        }
        return stepon;
    }

    public bool Use()
    {
        bool isHasAngel = (ResidueRoundCount > 0);
        if (isHasAngel)
            AngelUI.ShowHasAngelHint(true);
        ResidueRoundCount--;
        AngelUI.SetResidueRoundCount(ResidueRoundCount);

        if (ResidueRoundCount == 0)
            TiePoint.ClearModel();

        return isHasAngel;
    }
}
