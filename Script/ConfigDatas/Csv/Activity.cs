using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ActivityConfig
{
    #region Configuration 文件中数据段
    /// <summary>ID</summary>
    public int CommodityID;

    /// <summary>活动类型（1累计登陆 2周累计登陆）</summary>
    public int Type;

    /// <summary>活动参数</summary>
    public int Value;

    /// <summary>活动开始时间</summary>
    public string Stime;

    /// <summary>活动结束时间</summary>
    public string Etime;

    /// <summary>活动说明</summary>
    public string Des;

    /// <summary>奖励</summary>
    public List<string> ItemID;
    #endregion

    #region Extend 扩展数据段
    public Dictionary<int, int> GetAward()
    {
        Dictionary<int, int> ItemLst = new Dictionary<int, int>();
        for(int i = 0; i < ItemID.Count; i++)
        {
            string[] tempSplit = ItemID[i].Split('&');
            ItemLst.Add(int.Parse(tempSplit[0]), int.Parse(tempSplit[1]));
        }
        return ItemLst;
    }
    #endregion
}
