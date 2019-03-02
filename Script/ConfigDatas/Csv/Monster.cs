using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MonsterConfig
{
    #region Configuration 文件中数据段

    /// <summary>怪物ID</summary>
    public int MonsterID;

    /// <summary> 形象</summary>
    public string Portrayal;

    /// <summary> 模型名 </summary>
    public string ModelName;

    /// <summary>名称</summary>
    public string Name;

    /// <summary>描述</summary>
    public string Describe;

    /// <summary>魔力值(作废字段2018.11.20)</summary>
    [Obsolete("", true)]
    public int MagicPower;

    /// <summary>积分奖励(作废字段2018.11.20)</summary>
    [Obsolete("", true)]
    public int ScoreReward;

    /// <summary>金币奖励(作废字段2018.11.20)</summary>
    [Obsolete("", true)]
    public int GoldReward;

    /// <summary>界面缩放值</summary>
    public int UIScale;
    #endregion

    #region Extend 扩展数据段
    #endregion
}
