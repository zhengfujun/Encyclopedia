using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AchievementUnit : MonoBehaviour
{
    public int AchievementID { get { return AchievementCfg.AchievementID; } }

    private AchievementConfig AchievementCfg = null;

    public UILabel Name;
    public UILabel Des;
    public UISprite Icon;
    public UISlider SchedulePB;
    public UILabel ScheduleValue;

    public GameObject GetAwardBtnObj;
    public GameObject InProgressHintObj;
    public GameObject PassedHintObj;

    public UI_AwardItem[] AwardItem;

    private string GMOrderItemStr = "";
    private Dictionary<int, int> AwardItemLst = new Dictionary<int,int>();

    public GameObject PutThroughObj;
    public bool IsPutThrough { get;set; }

    public bool IsHasAwardEnableGet { get { return GetAwardBtnObj.activeSelf; } }

    void Start()
    {
       
    }

    public void Set(int AchievementID)
    {
        if (CsvConfigTables.Instance.AchievementCsvDic.TryGetValue(AchievementID, out AchievementCfg))
        {
            Name.text = AchievementCfg.Name;
            Des.text = AchievementCfg.AchievementTarget.Replace("#", AchievementCfg.TargetParameters.ToString());
            Icon.spriteName = AchievementCfg.Icon;

            bool isFinish = false;
            long Schedule = 0;
            int deno = 0;
            switch (AchievementCfg.Type)
            {
                case 0://套卡收集
                    //if ( <= SerPlayerData.GetMainStageProgress())
                    {
                        Schedule = SerPlayerData.GetCardCounLimitThemet(AchievementCfg.AchievementParameters);
                        foreach (KeyValuePair<int, MagicCardConfig> pair in CsvConfigTables.Instance.MagicCardCsvDic)
                        {
                            if (pair.Value.ThemeID == AchievementCfg.AchievementParameters)
                            {
                                deno++;
                            }
                        }
                        ScheduleValue.text = Schedule + "/" + deno;
                        SchedulePB.value = (float)Schedule / (float)deno;

                        isFinish = (Schedule >= deno) && deno != 0;
                    }
                    break;
        //        case 1://关卡类型
        //            for (int i = 0; i < AchievementCfg.AchievementParameters.Count; i++)
        //            {
        //                Schedule += GameApp.Instance.TIDS.GetMethodPassedCnt(AchievementCfg.AchievementParameters[i]);
        //            }
        //            if (Schedule >= AchievementCfg.TargetParameters)
        //            {
        //                isFinish = true;
        //            }
        //            break;
        //        case 2://击杀指定怪物
        //            Schedule = GameApp.Instance.TIDS.GetKillMonsterCnt(AchievementCfg.AchievementParameters[0]);
        //            if (Schedule >= AchievementCfg.TargetParameters)
        //            {
        //                isFinish = true;
        //            }
        //            break;
        //        case 3://造成伤害
        //            Schedule = GameApp.Instance.TIDS.GetTotalHarmValue();
        //            if (Schedule >= AchievementCfg.TargetParameters)
        //            {
        //                isFinish = true;
        //            }
        //            break;
        //        case 4://强化技能
        //            WeaponSkillConfig wsc = null;
        //            if (CsvConfigTables.Instance.WeaponSkillCsvDic.TryGetValue((int)PeripheralEventMgr.Instance.weaponType, out wsc))
        //            {
        //                for (int i = 0; i < wsc.SuperSkillIDs.Count; i++)
        //                {
        //                    common.PlayerSkillItem psi = SerPlayerData.GetSkillInfo(wsc.SuperSkillIDs[i]);
        //                    if (psi != null)
        //                    {
        //                        if (psi.m_level >= AchievementCfg.TargetParameters)
        //                        {
        //                            Schedule++;
        //                        }
        //                    }
        //                }
        //            }
        //            if (Schedule >= 3)
        //            {
        //                isFinish = true;
        //            }
        //            break;
        //        case 5://击杀指定阵营怪物
        //            Schedule = GameApp.Instance.TIDS.GetKillCampCnt(AchievementCfg.AchievementParameters[0]);
        //            if (Schedule >= AchievementCfg.TargetParameters)
        //            {
        //                isFinish = true;
        //            }
        //            break;
            }
            GetAwardBtnObj.SetActive(isFinish);
            InProgressHintObj.SetActive(!isFinish);
        //    PassedHintObj.SetActive(false);

        //    
            
        //    

            string[] TempSplit = AchievementCfg.Pool.Split('&');
            AwardItem[0].SetItemData(int.Parse(TempSplit[0]), int.Parse(TempSplit[1]));
            GMOrderItemStr = TempSplit[0] + " " + TempSplit[1];
            AwardItemLst.Clear();
            AwardItemLst.Add(int.Parse(TempSplit[0]), int.Parse(TempSplit[1]));

            //if (GameApp.Instance.TIDS.GetReceiveAwardState(AchievementID))
            if (PlayerPrefs.HasKey(GetLocalSaveKey()))
            {
                if (PlayerPrefs.GetInt(GetLocalSaveKey()) > 0)
                {
                    ResetNextAchievementInfo();
                }
            }
        }
    }

    /// <summary> 领取奖励 </summary>
    public void OnClickGetAward()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        GameApp.SendMsg.GMOrder("AddItem " + GMOrderItemStr);

        GameApp.Instance.GetItemsDlg.OpenGetItemsBox(AwardItemLst);

        //GameApp.Instance.TIDS.SetReceiveAwardState(AchievementCfg.AchievementID);
        ResetNextAchievementInfo();
    }

    /// <summary> 显示下一阶任务 </summary>
    private void ResetNextAchievementInfo()
    {
        if(AchievementCfg.FollowupAchievement != -1)
        {
            //将本条任务更新为后续任务的信息
            Set(AchievementCfg.FollowupAchievement);
        }
        else
        {
            //没有后续任务了，置灰，下移
            IsPutThrough = true;
            PutThroughObj.SetActive(true);

            GetAwardBtnObj.SetActive(false);
            InProgressHintObj.SetActive(false);
            PassedHintObj.SetActive(true);

            PlayerPrefs.SetInt(GetLocalSaveKey(), 1);
        }
    }

    private string GetLocalSaveKey()
    {
        return StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_Achievement_", AchievementID);
    }
}
