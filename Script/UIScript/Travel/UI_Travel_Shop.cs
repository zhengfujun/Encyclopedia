using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Travel_Shop : MonoBehaviour
{
    [HideInInspector]
    public bool isShowing = false;

    public GameObject CommodityUnitPrefab;
    public UIGrid CommodityGrid;

    public UILabel CandyCnt;

    private ItemConfig Details_ItemCfg;
    public GameObject DetailsRoot;
    public UILabel Details_Name;
    public UILabel Details_Des;
    public UILabel Details_HoldNum;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Show()
    {
        isShowing = true;
        TweenAlpha.Begin(gameObject, 0.1f, 1);

        CandyCnt.text = SerPlayerData.GetItemCount(1003).ToString();

        StartCoroutine("RefreshCommodityList");
    }

    public void Hide()
    {
        isShowing = false;
        TweenAlpha.Begin(gameObject, 0.1f, 0);
    }

    IEnumerator RefreshCommodityList()
    {
        MyTools.DestroyImmediateChildNodes(CommodityGrid.transform);
        UIScrollView sv = CommodityGrid.transform.parent.GetComponent<UIScrollView>();
        int i = 0;
        foreach (KeyValuePair<int, ItemConfig> pair in CsvConfigTables.Instance.ItemCsvDic)
        {
            if ((pair.Value.Type == 10 || pair.Value.Type == 11 || pair.Value.Type == 12) &&
                (pair.Value.PriceType != 0 && pair.Value.PriceValue != 0))
            {
                GameObject newUnit = NGUITools.AddChild(CommodityGrid.gameObject, CommodityUnitPrefab);
                newUnit.SetActive(true);
                newUnit.name = "CommodityUnit_" + i;
                int x = 0;
                if (i % 3 == 0)
                    x = 0;
                else if (i % 3 == 1)
                    x = 150;
                else if (i % 3 == 2)
                    x = 300;
                newUnit.transform.localPosition = new Vector3(x, -178 * (i / 3), 0);

                UI_Travel_Shop_CommodityUnit fu = newUnit.GetComponent<UI_Travel_Shop_CommodityUnit>();
                fu.SetCommodityData(pair.Value.ItemID);

                CommodityGrid.repositionNow = true;
                sv.ResetPosition();

                yield return new WaitForEndOfFrame();
                i++;
            }
        }
    }

    /// <summary> 显示商品详情 </summary>
    public void ShowCommodityDetails(ItemConfig ItemCfg)
    {
        TweenAlpha.Begin(DetailsRoot, 0.1f, 1);

        Details_ItemCfg = ItemCfg;
        Details_Name.text = ItemCfg.Name;
        Details_Des.text = ItemCfg.Describe;
        Details_HoldNum.text = StringBuilderTool.ToString("已拥有：", SerPlayerData.GetItemCount(ItemCfg.ItemID));
    }

    /// <summary> 点击购买 </summary>
    public void OnClick_Buy()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【购买】");

        if (SerPlayerData.GetItemCount(1003) < Details_ItemCfg.PriceValue)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("糖果不足！");
            return;
        }
        if (SerPlayerData.GetItemCount(Details_ItemCfg.ItemID) > 0 && Details_ItemCfg.EnableSuperposition == 0)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString(Details_ItemCfg.Name, "只能拥有一个！"));
            return;
        }

        GameApp.SendMsg.BuyItem((uint)Details_ItemCfg.ItemID);
    }

    public void BuyRes()
    {
        CandyCnt.text = SerPlayerData.GetItemCount(1003).ToString();

        if (Details_ItemCfg != null)
        {
            Details_HoldNum.text = StringBuilderTool.ToString("已拥有：", SerPlayerData.GetItemCount(Details_ItemCfg.ItemID));
        }
    }
}
