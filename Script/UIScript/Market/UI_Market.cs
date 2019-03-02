using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common;
using System;

public class UI_Market : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    private int CurType;

    public UILabel CDDesLab;

    public GameObject CommodityWaitLoad;
    public UILabel CommodityLoadProgLab;

    public GameObject CommodityLstRoot;
    public GameObject CommodityUnit_CardPrefab;
    public GameObject CommodityUnit_ItemPrefab;
    public UITable CommodityTable;

    public GameObject CardLstRoot;
    public GameObject CardUnit_CardPrefab;
    public UITable CardTable;

    public GameObject GoodsLstRoot;
    public GameObject GoodsUnit_Prefab;
    public UITable GoodsTable;

    public GameObject PurchaseConfirmationRoot;
    public Transform CommodityRoot;
    public UIButton PurchaseBtn;

    public GameObject GoldCoinNotEnoughRoot;

    public GameObject AbleExchangeCardLstRoot;
    public GameObject AbleExchangeCardPrefab;
    public UITable AbleExchangeTable;
    public UIButton ExchangeBtn;

    List<Commodity> TempCommodityLst = new List<Commodity>();
    List<MagicCardConfig> TempToExchangeCardLst = new List<MagicCardConfig>();

    void Start()
    {
        TempCommodityLst.Add(new Commodity(1,20001,ECommodityType.eItem,50));
        TempCommodityLst.Add(new Commodity(2,20002,ECommodityType.eItem,100));
        int index = 3;
        int[] RandomLst1 = MyTools.GetRandomNumArray4Barring(8, 17, -1);
        for (int i = 0; i < RandomLst1.Length; i++)
        {
            MagicCardConfig mcc = null;
            if (CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(RandomLst1[i] + 50001, out mcc))
            {
                TempCommodityLst.Add(new Commodity(index++, mcc.CardID, ECommodityType.eCard, mcc.Quality * 100 + mcc.StarLv * 10));
            }
        }

        int[] RandomLst2 = MyTools.GetRandomNumArray4BarringEx(8, 17, RandomLst1);
        for (int i = 0; i < RandomLst2.Length; i++)
        {
            MagicCardConfig mcc = null;
            if (CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(RandomLst2[i] + 50001, out mcc))
            {
                TempToExchangeCardLst.Add(mcc);
            }
        }

        InvokeRepeating("UpdateCDDes", 0, 1f);
    }

    void UpdateCDDes()
    {
        switch (CurType)
        {
            case 1:
                CDDesLab.text = "";//StringBuilderTool.ToString("市场更新倒计时 [2be95e]" + (23 - DateTime.Now.Hour) + "[-] 小时 [2be95e]" + (60 - DateTime.Now.Minute) + "[-] 分");
                break;
            case 2:
                CDDesLab.text = "";//StringBuilderTool.ToString("换卡更新倒计时 [2be95e]" + (23 - DateTime.Now.Hour) + "[-] 小时 [2be95e]" + (60 - DateTime.Now.Minute) + "[-] 分");
                break;
            case 3:
                CDDesLab.text = "";
                break;
        }
    }

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            
        }
    }*/

    public void Show(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            GameApp.Instance.UICurrency.Show(true);

        }
        else
        {
            GameApp.Instance.UICurrency.Show(false);

            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    public void OnClick_Back()
    {
        Debug.Log("点击【退出】");

        Show(false);

        GameApp.Instance.SoundInstance.PlaySe("button");
    }

    public void OnTypeToggleChange()
    {
        if (UIToggle.current.value)
        {
            Debug.Log(UIToggle.current.name);
            CurType = int.Parse(MyTools.GetLastString(UIToggle.current.name, '_'));
            switch(CurType)
            {
                case 1:
                    ShowCommodityLst();
                    break;
                case 2:
                    ShowCardLst();
                    break;
                case 3:
                    ShowStore();
                    break;
            }
            UpdateCDDes();
        }
    }

    #region _市场
    public void ShowCommodityLst()
    {
        TweenAlpha.Begin(CommodityLstRoot, 0.2f, 1);
        TweenAlpha.Begin(CardLstRoot, 0.2f, 0);
        TweenAlpha.Begin(GoodsLstRoot, 0.2f, 0);

        StartCoroutine("RefreshCommodityList");
    }

    enum ECommodityType
    {
        eCard,
        eItem
    }
    class Commodity
    {
        public int Index;
        public int ID;
        public ECommodityType Type;
        public int Price;

        public Commodity(int _Index, int _ID, ECommodityType _Type, int _Price)
        {
            Index = _Index;
            ID = _ID;
            Type = _Type;
            Price = _Price;
        }
    }
    IEnumerator RefreshCommodityList()
    {
        MyTools.DestroyChildNodes(CommodityTable.transform);
        CommodityWaitLoad.SetActive(true);
        int CurProg = 0;
        foreach (Commodity c in TempCommodityLst)
        {
            if (c.Type == ECommodityType.eCard)
            {
                GameObject newUnit = NGUITools.AddChild(CommodityTable.gameObject, CommodityUnit_CardPrefab);
                newUnit.SetActive(true);
                newUnit.name = "Commodity_Card_" + c.ID;

                UI_MagicCard mc = newUnit.GetComponent<UI_MagicCard>();
                mc.UnconditionalShow(c.ID);
                mc.SetPrice(c.Price);
            }
            else if (c.Type == ECommodityType.eItem)
            {
                GameObject newUnit = NGUITools.AddChild(CommodityTable.gameObject, CommodityUnit_ItemPrefab);
                newUnit.SetActive(true);
                newUnit.name = "Commodity_Item_" + c.ID;

                UI_CommodityItem ci = newUnit.GetComponent<UI_CommodityItem>();
                ci.Show(c.ID, c.Price, 1001);
            }

            CommodityTable.repositionNow = true;
            yield return new WaitForEndOfFrame();
            CommodityLstRoot.GetComponent<UIScrollView>().ResetPosition();

            CurProg++;
            CommodityLoadProgLab.text = StringBuilderTool.ToString("商品加载中(", CurProg, "/",TempCommodityLst.Count, ")...");
        }
        CommodityWaitLoad.SetActive(false);
    }

    private GameObject CurCommodityObj = null;
    public void ShowPurchaseConfirmation(GameObject CommodityObj)
    {
        CurCommodityObj = CommodityObj;
        MyTools.DestroyChildNodes(CommodityRoot);
        GameObject NewCurCommodityObj = GameObject.Instantiate(CurCommodityObj);
        NewCurCommodityObj.transform.parent = CommodityRoot;
        NewCurCommodityObj.transform.localPosition = Vector3.zero;
        NewCurCommodityObj.transform.localScale = Vector3.one;

        bool Purchased = false;
        UI_MagicCard mc = NewCurCommodityObj.GetComponent<UI_MagicCard>();
        UI_CommodityItem ci = NewCurCommodityObj.GetComponent<UI_CommodityItem>();
        if (mc != null)
        {
            Purchased = mc.PurchasedSign.activeSelf;
            if (Purchased)
                mc.RecoverFromPurchased();
        }
        else if (ci != null)
        {
            Purchased = ci.PurchasedSign.activeSelf;
            if (Purchased)
                ci.RecoverFromPurchased();
        }
        PurchaseBtn.isEnabled = !Purchased;
        PurchaseBtn.transform.GetChild(0).GetComponent<UILabel>().text = Purchased ? "已购买" : "购买";

        TweenAlpha.Begin(PurchaseConfirmationRoot, 0.2f, 1).from = 0;
    }
    public void OnClick_ClosePurchaseConfirmation()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭购买确认界面】");

        TweenAlpha.Begin(PurchaseConfirmationRoot, 0.2f, 0).from = 1;
    }
    public void OnClick_Purchase()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【购买】");

        switch (CurType)
        {
            case 1:
                {
                    uint Gold = SerPlayerData.GetItemCount(1001);
                    uint CommodityID = 0;
                    uint CommodityPrice = 0;

                    UI_MagicCard mc = CurCommodityObj.GetComponent<UI_MagicCard>();
                    UI_CommodityItem ci = CurCommodityObj.GetComponent<UI_CommodityItem>();
                    if (mc != null)
                    {
                        CommodityID = (uint)mc.MCCfg.CardID;
                        CommodityPrice = uint.Parse(mc.Price.text);
                    }
                    else if (ci != null)
                    {
                        CommodityID = (uint)ci.ItemCfg.ItemID;
                        CommodityPrice = uint.Parse(ci.Price.text);
                    }
                    else
                        return;

                    if (Gold >= CommodityPrice)
                    {
                        if (mc != null)
                        {
                            mc.SetPurchased();

                            GameApp.SendMsg.GMOrder(StringBuilderTool.ToString("AddItem ", mc.MCCfg.CardID, " 1"));
                        }
                        else if (ci != null)
                        {
                            ci.SetPurchased();

                            GameApp.SendMsg.GMOrder(StringBuilderTool.ToString("AddItem ", ci.ItemCfg.ItemID, " 1"));
                        }

                        TweenAlpha.Begin(PurchaseConfirmationRoot, 0.2f, 0).from = 1;

                        GameApp.SendMsg.GMOrder(StringBuilderTool.ToString("AddItem 1001 ", -CommodityPrice));
                    }
                    else
                    {
                        ShowGoldCoinNotEnough();
                    }
                }
                break;
            case 2:
                break;
            case 3:
                {
                    UI_CommodityItem ci = CurCommodityObj.GetComponent<UI_CommodityItem>();
                    if (ci != null)
                    {
                        if (GameApp.Instance.Platform != null && GameApp.Instance.LeSDKInstance != null)
                            GameApp.Instance.Platform.CreateOrder(ci.GoodsID);
                    }
                }
                break;
        }
    }

    public void ShowGoldCoinNotEnough()
    {
        TweenAlpha.Begin(GoldCoinNotEnoughRoot, 0.2f, 1).from = 0;
    }
    public void OnClick_CloseGoldCoinNotEnough()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭金币不够界面】");

        TweenAlpha.Begin(GoldCoinNotEnoughRoot, 0.2f, 0).from = 1;
    }
    public void OnClick_GotoStage()
    {
        Debug.Log("点击【前往关卡】");

        TweenAlpha.Begin(PurchaseConfirmationRoot, 0.2f, 0).from = 1;
        TweenAlpha.Begin(GoldCoinNotEnoughRoot, 0.2f, 0).from = 1;

        OnClick_Back();

        GameApp.Instance.SoundInstance.PlaySe("DinosaurRoar");
        GameApp.Instance.SoundInstance.PlaySe("Elephant");

        GameApp.Instance.HomePageUI.StageMapUI.Show(true, 1);
    }
    #endregion

    #region _换卡
    public void ShowCardLst()
    {
        TweenAlpha.Begin(CommodityLstRoot, 0.2f, 0);
        TweenAlpha.Begin(CardLstRoot, 0.2f, 1);
        TweenAlpha.Begin(GoodsLstRoot, 0.2f, 0);

        StartCoroutine("RefreshCardList");
    }

    IEnumerator RefreshCardList()
    {
        MyTools.DestroyChildNodes(CardTable.transform);
        CommodityWaitLoad.SetActive(true);
        int CurProg = 0;
        foreach (MagicCardConfig CardCfg in TempToExchangeCardLst)
        {
            GameObject newUnit = NGUITools.AddChild(CardTable.gameObject, CardUnit_CardPrefab);
            newUnit.SetActive(true);
            newUnit.name = "Card_" + CardCfg.CardID;

            UI_MagicCard mc = newUnit.GetComponent<UI_MagicCard>();
            mc.UnconditionalShow(CardCfg.CardID);

            CardTable.repositionNow = true;
            yield return new WaitForEndOfFrame();
            CardLstRoot.GetComponent<UIScrollView>().ResetPosition();

            CurProg++;
            CommodityLoadProgLab.text = StringBuilderTool.ToString("卡牌加载中(", CurProg, "/", TempToExchangeCardLst.Count, ")...");
        }
        CommodityWaitLoad.SetActive(false);
    }

    private UI_MagicCard CurExchangeCard_InMarket = null;
    private UI_MagicCard CurExchangeCard_InBag = null;
    public void ShowAbleExchangeCardLst(UI_MagicCard ExchangeCard)
    {
        TweenAlpha.Begin(AbleExchangeCardLstRoot, 0.2f, 1).from = 0;

        CurExchangeCard_InMarket = ExchangeCard;

        ExchangeBtn.isEnabled = false;

        StartCoroutine("RefreshAbleExchangeCardList", CurExchangeCard_InMarket.MCCfg.Quality);
    }
    IEnumerator RefreshAbleExchangeCardList(int Quality)
    {
        MyTools.DestroyChildNodes(AbleExchangeTable.transform);

        UIScrollView sv = AbleExchangeTable.transform.parent.GetComponent<UIScrollView>();
        foreach (KeyValuePair<int, MagicCardConfig> pair in CsvConfigTables.Instance.MagicCardCsvDic)
        {
            if (pair.Value.Quality <= Quality && SerPlayerData.GetItemCount(pair.Value.CardID) > 1)
            {
                GameObject newUnit = NGUITools.AddChild(AbleExchangeTable.gameObject, AbleExchangeCardPrefab);
                newUnit.SetActive(true);
                newUnit.name = "Card_" + pair.Value.CardID;

                UI_MagicCard mc = newUnit.GetComponent<UI_MagicCard>();
                mc.UnconditionalShow(pair.Value.CardID);

                AbleExchangeTable.repositionNow = true;

                yield return new WaitForEndOfFrame();

                sv.ResetPosition();
            }            
        }
    }
    public void OnClick_CloseAbleExchangeCardLst()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭可换魔卡界面】");

        TweenAlpha.Begin(AbleExchangeCardLstRoot, 0.2f, 0).from = 1;
    }
    public void SelectExchangeCard(UI_MagicCard ExchangeCard)
    {
        CurExchangeCard_InBag = ExchangeCard;

        ExchangeBtn.isEnabled = true;
    }
    public void OnClick_Exchange()
    {
        Debug.Log("点击【确定换卡】");

        TweenAlpha.Begin(AbleExchangeCardLstRoot, 0.2f, 0).from = 1;

        GameApp.Instance.AddCardHoldCount(CurExchangeCard_InMarket.MCCfg.CardID, 1);
        GameApp.Instance.AddCardHoldCount(CurExchangeCard_InBag.MCCfg.CardID, -1);

        CurExchangeCard_InMarket.SetExchanged();
    }
    #endregion

    #region _商城
    public void ShowStore()
    {
        TweenAlpha.Begin(CommodityLstRoot, 0.2f, 0);
        TweenAlpha.Begin(CardLstRoot, 0.2f, 0);
        TweenAlpha.Begin(GoodsLstRoot, 0.2f, 1);

        StartCoroutine("RefreshGoodsList");
    }
    IEnumerator RefreshGoodsList()
    {
        CommodityWaitLoad.SetActive(true);
        if (GameApp.Instance.Platform != null)
        {
            CommodityLoadProgLab.text = StringBuilderTool.ToString("等待获取商品列表...");
            if (!GameApp.Instance.Platform.IsGetGetGoodsListOver)
            {
                GameApp.Instance.Platform.GetGoodsList();

                while (!GameApp.Instance.Platform.IsGetGetGoodsListOver)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            MyTools.DestroyChildNodes(GoodsTable.transform);
            foreach (KeyValuePair<int, GoodsInfo> pair in GameApp.Instance.Platform.GoodsDic)
            {
                if (pair.Value.ID >= 35 && pair.Value.ID <= 37)
                {
                    GameObject newUnit = NGUITools.AddChild(GoodsTable.gameObject, GoodsUnit_Prefab);
                    newUnit.SetActive(true);
                    newUnit.name = "Goods_" + pair.Value.ID;

                    UI_CommodityItem ci = newUnit.GetComponent<UI_CommodityItem>();
                    ci.Show(pair.Value.CfgItemID, float.Parse(pair.Value.GetPriceStr()), 0);
                    ci.OverrideName(pair.Value.Name);
                    ci.GoodsID = pair.Value.ID;

                    GoodsTable.repositionNow = true;

                    yield return new WaitForEndOfFrame();

                    GoodsLstRoot.GetComponent<UIScrollView>().ResetPosition();
                }
            }
        }
        else
        {
            CommodityLoadProgLab.text = StringBuilderTool.ToString("为连接运营平台，无法获取商品列表！");
            yield return new WaitForSeconds(2f);
        }
        CommodityWaitLoad.SetActive(false);
    }
    #endregion
}
