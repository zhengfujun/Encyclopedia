using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_CommodityItem : MonoBehaviour
{
    [HideInInspector]
    public ItemConfig ItemCfg = null;

    public UILabel Name;
    public UILabel Price;
    public UISprite PriceIcon;
    public UISprite Icon;
    public GameObject PurchasedSign;

    [HideInInspector]
    public int GoodsID = 0;
    /*void Awake()
    {

    }

    void Start()
    {

    }

    void OnDestroy()
    {

    }

    void Update()
    {

    }*/

    public void Show(int ItemID, float fPrice, int PriceType)
    {
        CsvConfigTables.Instance.ItemCsvDic.TryGetValue(ItemID, out ItemCfg);
        if (ItemCfg != null)
        {
            Icon.spriteName = ItemCfg.Icon;
            Icon.MakePixelPerfect();
            Icon.transform.localScale = Vector3.one * 1.6f;

            Name.text = ItemCfg.Name;
        }

        Price.text = fPrice.ToString();

        ItemConfig PriceTypeCfg = null;
        CsvConfigTables.Instance.ItemCsvDic.TryGetValue(PriceType, out PriceTypeCfg);
        if (PriceTypeCfg != null)
        {
            PriceIcon.spriteName = PriceTypeCfg.Icon;
            PriceIcon.MakePixelPerfect();
            PriceIcon.transform.localScale = Vector3.one * 0.6f;
        }
        else
        {
            if(PriceType == 0)
            {
                PriceIcon.spriteName = "rmb";
                PriceIcon.MakePixelPerfect();
                PriceIcon.transform.localScale = Vector3.one * 0.6f;
            }
        }
    }

    public void OverrideName(string NewName)
    {
        Name.text = NewName;
    }

    public void SetPurchased()
    {
        Price.text = "已购买";
        Price.color = Color.white;
        Price.transform.localPosition = new Vector3(-38, -122, 0);
        transform.Find("PriceIcon").gameObject.SetActive(false);
        transform.Find("Bg").GetComponent<UISprite>().spriteName = "tupianbiankuang";
        PurchasedSign.SetActive(true);
    }
    public void RecoverFromPurchased()
    {
        Price.text = StringBuilderTool.ToInfoString("[FFFB63]", Price.text, "[-]");
        Price.transform.localPosition = new Vector3(-10, -122, 0);
        transform.Find("PriceIcon").gameObject.SetActive(true);
        transform.Find("Bg").GetComponent<UISprite>().spriteName = "lankuang";
        PurchasedSign.SetActive(false);
    }

    public void OnClick()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击："+ Name.text);        
        Debug.Log("显示购买确认");

        GameApp.Instance.HomePageUI.MarketUI.ShowPurchaseConfirmation(gameObject);
    }
}
