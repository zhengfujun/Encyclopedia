using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShopConfig
{
    #region Configuration 文件中数据段    
    /// <summary> 商品ID </summary>
    public int CommodityID;

    /// <summary> 商店类型（1：出现在主界面商店的第一页签中；10：出现在战斗商店中；20：旅行中的商店） </summary>
    public int Type;

    /// <summary> 道具ID </summary>
    public int ItemID;

    /// <summary> 图标（不填时使用道具图标） </summary>
    public string Icon;

    /// <summary> 商品名称（不填时使用道具名称） </summary>
    public string Name;

    /// <summary> 商品说明（不填时使用道具说明）</summary>
    public string Explain;

    /// <summary> 数量 </summary>
    public int Number;

    /// <summary> 货币类型（道具表ID） </summary>
    public int PriceType;

    /// <summary> 当前售价 </summary>
    public int NowPriceValue;

    /// <summary> 原价 </summary>
    public int OriginalPriceValue;
    #endregion

    #region Extend 扩展数据段    
    private ItemConfig GetItemCfg()
    {
        ItemConfig ItemCfg = null;
        CsvConfigTables.Instance.ItemCsvDic.TryGetValue(ItemID, out ItemCfg);
        return ItemCfg;
    }
    public string GetIcon()
    {
        if(Icon.Length == 0)
        {
            Icon = GetItemCfg().Icon;
        }
        return Icon;
    }
    public string GetName()
    {
        if (Name.Length == 0)
        {
            Name = GetItemCfg().Name;
        }
        return Name;
    }
    public string GetDescribe()
    {
        if (Explain.Length == 0)
        {
            Explain = GetItemCfg().Describe;
        }
        return Explain;
    }
    public string GetVoice()
    {
        return GetItemCfg().Voice;
    }
    #endregion
}
