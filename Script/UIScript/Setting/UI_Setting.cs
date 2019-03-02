using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_Setting : MonoBehaviour
{
    //public SetString MainPlayer_Name;
    //public SetInt MainPlayer_ThrowDictCD;

    //public SetBool AIRobot_Enable;
    //public SetInt AIRobot_RoleID;
    //public SetString AIRobot_Name;
    /*public SetInt AIRobot_RTDBeginTime;
    public SetInt AIRobot_RTDEndTime;
    public SetInt AIRobot_RRBeginTime;
    public SetInt AIRobot_RREndTime;
    public SetInt AIRobot_Accuracy;
    public SetInt AIRobot_UseItemPR;*/

    //public SetInt QandA_NormalCD;
    //public SetInt QandA_BossCD;

    //public SetInt Shop_CD;

    //public SetBool Fight_ShowCustomDice;

    //public SetInt Fight_BossTicketsScore;
    //public SetInt Fight_StartPointScore;

    //public SetInt Fight_Angel_RoundCount;
    //public SetInt Fight_Angel_RandomAppearMinGrid;
    //public SetInt Fight_Angel_RandomAppearMaxGrid;

    //public SetBool[] MainPlayer_MagicCard_HasState;

    //public SetBool Camera_EnableDepthOfField;
    //public SetBool Camera_EnableBloom;
    //public SetBool Camera_EnableAmbientOcclusion;

    void Awake()
    {
        /*MainPlayer_Name.Set("玩家名称", GameApp.Instance.MainPlayerData.Name, (value) =>
            {
                GameApp.Instance.MainPlayerData.Name = value;
                PlayerPrefs.SetString("MainPlayerName", value);
            });*/
        /*MainPlayer_ThrowDictCD.Set("等待玩家掷骰子倒计时", GameApp.Instance.PlayerThrowDictCD, (value) =>
        {
            GameApp.Instance.PlayerThrowDictCD = value;
        });*/

        /*AIRobot_Enable.Set("是否与机器人对战", AIRobotParam.Instance.EnableAIRobot, (value) =>
            {
                AIRobotParam.Instance.EnableAIRobot = value;
                PlayerPrefs.SetInt("AIRobotEnable", value ? 1 : 0);
            });*/
        /*AIRobot_RoleID.Set("机器人角色模型ID", (int)GameApp.Instance.AIRobotData.RoleID, (value) =>
            {
                GameApp.Instance.AIRobotData.RoleID = (uint)value;
                PlayerPrefs.SetInt("AIRobotRoleID", value);
            });*/
        /*AIRobot_Name.Set("机器人名称", GameApp.Instance.AIRobotData.Name, (value) =>
        {
            GameApp.Instance.AIRobotData.Name = value;
            PlayerPrefs.SetString("AIRobotName", value);
        });*/
        /*AIRobot_RTDBeginTime.Set("机器人随机延时掷骰子 最小秒数", AIRobotParam.Instance.RandomThrowDictBeginTime, (value) =>
            {
                AIRobotParam.Instance.RandomThrowDictBeginTime = value;
                PlayerPrefs.SetInt("AIRobotRTDBeginTime", value);
            });
        AIRobot_RTDEndTime.Set("最大秒数", AIRobotParam.Instance.RandomThrowDictEndTime, (value) =>
            {
                AIRobotParam.Instance.RandomThrowDictEndTime = value;
                PlayerPrefs.SetInt("AIRobotRTDEndTime", value);
            });
        AIRobot_RRBeginTime.Set("机器人随机延时作答 最小秒数", AIRobotParam.Instance.RandomRespondenceBeginTime, (value) =>
            {
                AIRobotParam.Instance.RandomRespondenceBeginTime = value;
                PlayerPrefs.SetInt("AIRobotRRBeginTime", value);
            });
        AIRobot_RREndTime.Set("最大秒数", AIRobotParam.Instance.RandomRespondenceEndTime, (value) =>
            {
                AIRobotParam.Instance.RandomRespondenceEndTime = value;
                PlayerPrefs.SetInt("AIRobotRREndTime", value);
            });
        AIRobot_Accuracy.Set("机器人答题正确率(0-100)", AIRobotParam.Instance.Accuracy, (value) =>
            {
                AIRobotParam.Instance.Accuracy = value;
                PlayerPrefs.SetInt("AIRobotAccuracy", value);
            });
        AIRobot_UseItemPR.Set("机器人使用道具概率(0-100)", AIRobotParam.Instance.UseItemPR, (value) =>
            {
                AIRobotParam.Instance.UseItemPR = value;
                PlayerPrefs.SetInt("AIRobotUseItemPR", value);
            });*/

        /*QandA_NormalCD.Set("普通问答倒计时", GameApp.Instance.NormalQandACD, (value) =>
            {
                GameApp.Instance.NormalQandACD = value;
            });
        QandA_BossCD.Set("Boss问答倒计时", GameApp.Instance.BossQandACD, (value) =>
            {
                GameApp.Instance.BossQandACD = value;
            });*/

        /*Shop_CD.Set("商店倒计时", GameApp.Instance.ShopInFightCD, (value) =>
            {
                GameApp.Instance.ShopInFightCD = value;
            });*/


        /*Fight_ShowCustomDice.Set("是否显示自定义骰子", GameApp.Instance.ShowCustomDice, (value) =>
            {
                GameApp.Instance.ShowCustomDice = value;
            });

        Fight_BossTicketsScore.Set("挑战Boss需要扣除的积分", GameApp.Instance.BossTicketsScore, (value) =>
            {
                GameApp.Instance.BossTicketsScore = value;
            });
        Fight_StartPointScore.Set("经过起点时增加的积分", GameApp.Instance.StartPointScore, (value) =>
            {
                GameApp.Instance.StartPointScore = value;
            });*/

        /*Fight_Angel_RoundCount.Set("红天使效果回合数", GameApp.Instance.AngelRoundCount, (value) =>
            {
                GameApp.Instance.AngelRoundCount = value;
            });
        Fight_Angel_RandomAppearMinGrid.Set("红天使随机出现 最小格子序号", GameApp.Instance.AngelRandomAppearMinGrid, (value) =>
            {
                GameApp.Instance.AngelRandomAppearMinGrid = value;
            });
        Fight_Angel_RandomAppearMaxGrid.Set("最大格子序号", GameApp.Instance.AngelRandomAppearMaxGrid, (value) =>
            {
                GameApp.Instance.AngelRandomAppearMaxGrid = value;
            });*/

        /*int i = 0;
        foreach (KeyValuePair<int, MagicCardConfig> pair in CsvConfigTables.Instance.MagicCardCsvDic)
        {
            MainPlayer_MagicCard_HasState[i].Set("是否拥有【" + pair.Value.Name + "】魔卡", GameApp.Instance.CardHoldCountLst[pair.Value.CardID] > 0, (value) =>
                {
                    GameApp.Instance.AddCardHoldCount(pair.Value.CardID, value ? 1 : 0);
                    GameApp.Instance.MainPlayerData.MagicPower = UI_NewMagicBook.CalcMagicPower();
                });
            i++;
        }*/

        /*Camera_EnableDepthOfField.Set("是否启用景深效果", GameApp.Instance.EnableDepthOfField, (value) =>
            {
                GameApp.Instance.EnableDepthOfField = value;
            });
        Camera_EnableBloom.Set("是否启用全屏泛光效果", GameApp.Instance.EnableBloom, (value) =>
            {
                GameApp.Instance.EnableBloom = value;
            });
        Camera_EnableAmbientOcclusion.Set("是否启用环境光遮蔽效果", GameApp.Instance.EnableAmbientOcclusion, (value) =>
            {
                GameApp.Instance.EnableAmbientOcclusion = value;
            });*/
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {

        }
    }

    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }
    /// <summary> 点击恢复默认</summary>
    public void OnClick_RestoreDefault()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【恢复默认】");

        PlayerPrefs.DeleteAll();
    } 
    #endregion
}
