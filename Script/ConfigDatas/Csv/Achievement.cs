using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AchievementConfig
{
    #region Configuration 文件中数据段    
    /// <summary>
    /// 成就id
    /// </summary>
    public int AchievementID;
    /// <summary>
    /// 成就图标
    /// </summary>
    public string Icon;
    /// <summary>
    /// 成就类型（0：套卡收集(AchievementParameters是卡牌的ThemeID)、1、2、3、4、5）
    /// </summary>
    public int Type;
    /// <summary>
    /// 成就名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 成就目标
    /// </summary>
    public string AchievementTarget;
    /// <summary>
    /// 目标参数1
    /// </summary>
    public int AchievementParameters;
    /// <summary>
    /// 目标参数2
    /// </summary>
    public int TargetParameters;
    /// <summary>
    /// 前置成就
    /// </summary>
    public int Predecessors;
    /// <summary>
    /// 后续成就
    /// </summary>
    public int FollowupAchievement;
    /// <summary>
    /// 成就奖励（道具Id&数量）
    /// </summary>
    public string Pool;
    #endregion

    #region Extend 扩展数据段
    
    #endregion
}
