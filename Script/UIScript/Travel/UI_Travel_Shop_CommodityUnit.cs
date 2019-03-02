using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Travel_Shop_CommodityUnit : MonoBehaviour
{
    public UISprite Bg;
    public UISprite Icon;
    public UILabel PriceNum;
    public UILabel Name;

    private ItemConfig ItemCfg = null;

    void Start()
    {
        UI_Travel_Shop ts = GameApp.Instance.TravelUI.TravelShop;

        UIButton btn = GetComponent<UIButton>();
        if (btn != null)
        {
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(() =>
            {
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    Transform child = transform.parent.GetChild(i);
                    UI_Travel_Shop_CommodityUnit tscu = child.GetComponent<UI_Travel_Shop_CommodityUnit>();
                    tscu.SetSelState(false);
                }
                SetSelState(true);

                ts.ShowCommodityDetails(ItemCfg);
            }));
        }
    }

    /*void Update()
    {

    }*/

    /// <summary> 设置商品数据 </summary>
    public void SetCommodityData(int CommodityID)
    {
        gameObject.SetActive(true);

        CsvConfigTables.Instance.ItemCsvDic.TryGetValue(CommodityID, out ItemCfg);
        if (ItemCfg != null)
        {
            Icon.spriteName = ItemCfg.Icon;
            Icon.MakePixelPerfect();
            Icon.transform.localScale = Vector3.one * 1.5f;

            Name.text = ItemCfg.Name;

            PriceNum.text = ItemCfg.PriceValue.ToString();
        }
    }

    public void SetSelState(bool isSel)
    {
        Bg.spriteName = (isSel ? "lvxingshangdianshangpinBG_Sel" : "lvxingshangdianshangpinBG");
    }
}
