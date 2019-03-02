using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemConfig
{
    #region Configuration 文件中数据段

    /// <summary>道具ID</summary>
    public int ItemID;

    /// <summary>图标</summary>
    public string Icon;

    /// <summary>类型(1:普通道具;2:卡牌;3:货币;10:食物(旅行);11:幸运物(旅行);12:装备(旅行);13:礼物(旅行))</summary>
    public int Type;

    /// <summary>名称</summary>
    public string Name;

    /// <summary>是否可购买</summary>
    public int EnableBuy;

    /// <summary>是否可叠加</summary>
    public int EnableSuperposition;

    /// <summary>叠加上限</summary>
    public int SuperpositionUpperLimit;

    /// <summary>是否消耗</summary>
    public int Reuse;

    /// <summary>购买时所需货币类型（道具表ID）</summary>
    public int PriceType;

    /// <summary>当前售价（需要消耗的货币道具数量）</summary>
    public int PriceValue;

    /// <summary>道具描述</summary>
    public string Describe;

    /// <summary>语音</summary>
    public string Voice;
    #endregion

    #region Extend 扩展数据段
    private MagicCardConfig GetMagicCardCfg()
    {
        MagicCardConfig MCCfg = null;
        CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(ItemID, out MCCfg);
        return MCCfg;
    }
    public string GetIcon()
    {
        if (Icon.Length == 0)
        {
            Icon = GetMagicCardCfg().ColouredIcon;
        }
        return Icon;
    }
    #endregion
}
