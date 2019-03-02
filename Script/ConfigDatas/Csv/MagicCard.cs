using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInfo
{
    public int ItemID;
    public int ItemCnt;

    public ItemInfo(int ID, int Cnt)
    {
        ItemID = ID;
        ItemCnt = Cnt;
    }
}

public class MagicCardConfig
{
    #region Configuration 文件中数据段

    /// <summary>魔卡ID</summary>
    public int CardID;

    /// <summary>知识体系(同ChapterID)</summary>
    public int SystemID;

    /// <summary>主题(同GroupID)</summary>
    public int ThemeID;

    /// <summary>有颜色的图标</summary>
    public string ColouredIcon;

    /// <summary>没有颜色的图标</summary>
    public string ColorlessIcon;

    /// <summary> 模型名 </summary>
    public string ModelName;

    /// <summary> 涂色用模型名 </summary>
    public string ColoringModelName;

    /// <summary>名称</summary>
    public string Name;

    /// <summary>品质(1:白;2:绿;3:蓝;4:紫;5:金)</summary>
    public int Quality;

    /// <summary>星数</summary>
    public int StarLv;

    /// <summary>魔力值</summary>
    public int MagicVal;

    /// <summary>时期</summary>
    public string Epoch;

    /// <summary>时间</summary>
    public string Time;

    /// <summary>守护星</summary>
    public string Ruler;

    /// <summary>科</summary>
    public string Family;
    
    /// <summary>拉丁学名</summary>
    public string LatinName;

    /// <summary>身长</summary>
    public string Height;

    /// <summary>栖息地</summary>
    public string Habitat;

    /// <summary>食物</summary>
    public string Food;

    /// <summary>体重</summary>
    public string Weight;

    /// <summary>距太阳的平均距离</summary>
    public string Distance;

    /// <summary>表面温度</summary>
    public string Temperature;

    /// <summary>直径</summary>
    public string Diameter;

    /// <summary>一日的时长</summary>
    public string Day;
    
    /// <summary>一年的时长</summary>
    public string Year;

    /// <summary>卫星数量</summary>
    public string Satellite;

    /// <summary>称号</summary>
    public string Title;

    /// <summary>合成需要消耗的道具(道具ID:数量/道具ID:数量/...)</summary>
    public string NeedItems;

    /// <summary>合成需要消耗的货币(货币道具ID:数量)</summary>
    public string NeedCurrency;

    /// <summary>副本快捷方式(关卡ID)</summary>
    public int Shortcut;

    /// <summary>描述</summary>
    public string Describe;

    /// <summary>语音</summary>
    public string Voice;
    #endregion

    #region Extend 扩展数据段
    /// <summary> 合成需要消耗的道具数据 </summary>
    public List<ItemInfo> NeedItemsLst
    {
        get
        {
            if (_NeedItemsLst.Count == 0)
            {
                string[] TempSplit = NeedItems.Split('/');
                for (int i = 0; i < TempSplit.Length; i++)
                {
                    
                    if (TempSplit[i] == "")
                        continue;

                    string[] ItemInfoSplit = TempSplit[i].Split(':');
                    _NeedItemsLst.Add(new ItemInfo(int.Parse(ItemInfoSplit[0]),int.Parse(ItemInfoSplit[1])));
                }
            }
            return _NeedItemsLst;
        }
    }
    private List<ItemInfo> _NeedItemsLst = new List<ItemInfo>();

    /// <summary> 合成需要消耗的货币 </summary>
    public ItemInfo NeedCurrencyItemInfo
    {
        get
        {
            if (_NeedCurrencyItemInfo == null)
            {
                string[] ItemInfoSplit = NeedCurrency.Split(':');
                _NeedCurrencyItemInfo = new ItemInfo(int.Parse(ItemInfoSplit[0]), int.Parse(ItemInfoSplit[1]));
            }
            return _NeedCurrencyItemInfo;
        }
    }
    private ItemInfo _NeedCurrencyItemInfo = null;
    #endregion
}

