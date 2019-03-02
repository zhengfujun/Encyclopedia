using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CommodityInFight : MonoBehaviour
{
    private ShopConfig CommodityCfg = null;
    private ItemConfig PriceItemCfg = null;

    public UISprite CommodityIcon;
    public UILabel CommodityName;
    public UILabel CommodityDes;
    public UILabel PriceDes;

    /*void Start()
    {

    }*/

    /*void Update()
    {

    }*/

    public void Set(ShopConfig Cfg)
    {
        if (Cfg != null)
        {
            CommodityCfg = Cfg;

            CommodityIcon.spriteName = CommodityCfg.GetIcon();
            CommodityName.text = CommodityCfg.GetName();
            CommodityDes.text = CommodityCfg.GetDescribe();

            if (CsvConfigTables.Instance.ItemCsvDic.TryGetValue(CommodityCfg.PriceType, out PriceItemCfg))
            {
                PriceDes.text = CommodityCfg.NowPriceValue + PriceItemCfg.Name;
            }
        }
    }
     /// <summary> 点击播放商品说明 </summary>
    public void OnClick_PlayExplainSound()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【播放商品说明】");

        GameApp.Instance.SoundInstance.StopAllSe();
        GameApp.Instance.SoundInstance.PlayVoice(CommodityCfg.GetVoice());
    }

    /// <summary> 点击购买 </summary>
    public void OnClick_Buy()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【购买】" + CommodityCfg.Name);

        switch(CommodityCfg.PriceType)
        {
            case 2:
                if (GameApp.Instance.MainPlayerData.GoldCoin < CommodityCfg.NowPriceValue)
                {
                    //GameApp.Instance.CommonHintDlg.OpenHintBox(PriceItemCfg.Name + "不足！");
                    //GameApp.Instance.CommonMsgDlg.OpenMsgBox(PriceItemCfg.Name + "不足！",EMessageBoxStyle.OkayOnly);
                    GameApp.Instance.FightUI.ShopInFightUI.ShowHint(PriceItemCfg.Name + "不足！");
                    return;
                }
                break;
        }

        GameApp.Instance.FightUI.ShopInFightUI.ShowConfirm("花费" + CommodityCfg.NowPriceValue + PriceItemCfg.Name + "购买" + CommodityCfg.Name + "？",
            () =>
            {
                GameApp.Instance.MainPlayerData.AddItem(CommodityCfg.ItemID, 1);
                GameApp.Instance.MainPlayerData.GoldCoin -= CommodityCfg.NowPriceValue;

                //GameApp.Instance.CommonHintDlg.OpenHintBox("成功购买" + CommodityCfg.Name + "!");
                GameApp.Instance.FightUI.ShopInFightUI.ShowHint("成功购买" + CommodityCfg.Name + "!");
            });

    }
}
