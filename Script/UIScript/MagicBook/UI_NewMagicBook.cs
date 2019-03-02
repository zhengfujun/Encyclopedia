using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common;

public class UI_NewMagicBook : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public GameObject QualityRoot;

    public UI_ChapterUnit[] ChapterLst;

    public int CurChapterID = 0;
    public int CurGroupID = 0;
    public int CurQuality = 0;

    public GameObject ThemeLstRoot;
    public UI_ThemeUnit[] ThemeLst;

    public GameObject CardLstRoot;
    public GameObject CardUnitPrefab;
    public UITable CardTable;

    public GameObject CompoundRoot;
    public UI_MagicCard OutputCard;
    public UI_MagicCard[] MaterialCards;

    public GameObject ResultPanel;
    //public UI_MagicCard ResultCard;
    public UILabel ResultTitle;

    public GameObject CardDetailsRoot;
    public UITexture GroupBg;
    public UITexture GroupPlatform;
    public UI_MagicCard DetailsCard;
    public Transform CardModelRoot;
    private GameObject CardModel;
    //private GameObject ReferencerModel;
    public GameObject ClickAnimBtn;
    public GameObject PrevBtn;
    public GameObject NextBtn;

    GameObject CompoundEff = null;

    public GameObject GroupExplain;
    public UITexture GroupExplain_Pic;
    public UILabel GroupExplain_Title;
    public UILabel GroupExplain_Explain;

    /*public static int CalcMagicPower()
    {
        int mp = 0;
        foreach (KeyValuePair<int, MagicCardConfig> pair in CsvConfigTables.Instance.MagicCardCsvDic)
        {
            if (GameApp.Instance.CardHoldCountLst.ContainsKey(pair.Value.CardID))
            {
                if (GameApp.Instance.CardHoldCountLst[pair.Value.CardID] > 0)
                {
                    mp += pair.Value.MagicVal;
                }
            }
        }
        return mp;
    }*/

    void Start()
    {
        for (int i = 0; i < ChapterLst.Length;i++ )
        {
            if (CsvConfigTables.Instance.ChapterCsvDic.ContainsKey(i + 1))
                ChapterLst[i].Set(i + 1);
            else
                ChapterLst[i].gameObject.SetActive(false);
        }

        UIEventListener el = UIEventListener.Get(ClickAnimBtn);
        el.onDragStart = OnDragStart;
        el.onDrag = OnDrag;
    }

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            //GameApp.SendMsg.Compose(ComposeType.ComposeType_Item,50001);            
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            //GameApp.SendMsg.Compose(ComposeType.ComposeType_Coin, 50002);            
        }
    }*/

    public void Show(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            GameApp.Instance.UICurrency.OnlyShowMagicPower();
            UIPanel CurrencyPanel = GameApp.Instance.UICurrency.GetComponent<UIPanel>();
            CurrencyPanel.depth = 17;
            CurrencyPanel.sortingOrder = 27;

            //if(CurGroupID != 0)
            //    StartCoroutine("RefreshCardList");
        }
        else
        {
            UIPanel CurrencyPanel = GameApp.Instance.UICurrency.GetComponent<UIPanel>();
            CurrencyPanel.depth = 12;
            CurrencyPanel.sortingOrder = 22;

            GameApp.Instance.UICurrency.Show(false);

            BackToThemeLst();

            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    public void OnClick_Back()
    {
        Debug.Log("点击【退出】");

        /*if (CardLstRoot.GetComponent<UIPanel>().alpha >= 0.9f)
        {
            TweenAlpha.Begin(ThemeLstRoot, 0.2f, 1);
            TweenAlpha.Begin(CardLstRoot, 0.2f, 0);
            TweenAlpha.Begin(QualityRoot, 0.2f, 0); 
        }*/
        if (GroupExplain.GetComponent<UIPanel>().alpha >= 0.9f)
        {
            TweenAlpha.Begin(GroupExplain, 0.2f, 0);
        }

        if (CardDetailsRoot.GetComponent<UIPanel>().alpha >= 0.9f)
        {
            HideCardDetails();
        }
        else
        {
            BackToThemeLst();
            Show(false);
        }

        GameApp.Instance.SoundInstance.PlaySe("button");
    }

    public void OnChapterToggleChange()
    {
        if (UIToggle.current.value)
        {
            ThemeLstRoot.GetComponent<UIScrollView>().ResetPosition();

            CurChapterID = int.Parse(MyTools.GetLastString(UIToggle.current.name, '_'));
            RefreshThemes();

            Debug.Log(UIToggle.current.name);
        }
    }
    public void BackToThemeLst()
    {
        if (CardLstRoot.GetComponent<UIPanel>().alpha >= 0.9f)
        {
            TweenAlpha.Begin(ThemeLstRoot, 0.2f, 1);
            TweenAlpha.Begin(CardLstRoot, 0.2f, 0);
            TweenAlpha.Begin(QualityRoot, 0.2f, 0);
            TweenAlpha.Begin(GroupExplain, 0.2f, 0);
        }
    }
    private void RefreshThemes()
    {
        ChapterConfig cc = null;
        if (CsvConfigTables.Instance.ChapterCsvDic.TryGetValue(CurChapterID, out cc))
        {
            for (int i = 0; i < ThemeLst.Length; i++)
            {
                ThemeLst[i].gameObject.SetActive(false);
            }

            int k = 0;
            foreach (KeyValuePair<int, GroupConfig> pair in CsvConfigTables.Instance.GroupCsvDic)
            {
                if (k < ThemeLst.Length && pair.Value.GroupID / 100 == CurChapterID)
                {
                    ThemeLst[k].gameObject.SetActive(true);
                    ThemeLst[k].Set(pair.Value);
                    k++;
                }
            }
        }
    }

    public void ShowCardLst(int ThemeID)
    {
        TweenAlpha.Begin(ThemeLstRoot, 0.2f, 0);
        TweenAlpha.Begin(CardLstRoot, 0.2f, 1);
        TweenAlpha.Begin(QualityRoot, 0.2f, 1); 

        CurGroupID = ThemeID;
        StartCoroutine("RefreshCardList");
    }

    List<int> HoldLst = new List<int>();
    IEnumerator RefreshCardList()
    {
        MyTools.DestroyChildNodes(CardTable.transform);

        HoldLst.Clear();
        List<int> WithoutLst = new List<int>();
        List<int> WithoutAndEnableCompLst = new List<int>();
        List<int> WithoutAndUnableCompLst = new List<int>();
        foreach (KeyValuePair<int, MagicCardConfig> pair in CsvConfigTables.Instance.MagicCardCsvDic)
        {
            if (pair.Value.SystemID == CurChapterID && pair.Value.ThemeID == CurGroupID && (CurQuality == 0 ? true : pair.Value.Quality == CurQuality))
            {
                if (SerPlayerData.GetItemCount(pair.Key) > 0)
                    HoldLst.Add(pair.Key);
                else
                    WithoutLst.Add(pair.Key);
            }
        }

        if(HoldLst.Count == 0 && WithoutLst.Count == 0)
        {
            TweenAlpha.Begin(GroupExplain, 0.2f, 1);

            GroupConfig gc = null;
            CsvConfigTables.Instance.GroupCsvDic.TryGetValue(CurGroupID, out gc);
            if (gc != null)
            {
                GroupExplain_Pic.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("MagicCardTheme/", gc.ThemePic)) as Texture;
                GroupExplain_Title.text = gc.Name;
                GroupExplain_Explain.text = gc.Explain;
            }
            yield break;
        }
        else
        {
            TweenAlpha.Begin(GroupExplain, 0.2f, 0);
        }

        for(int i = 0;i < HoldLst.Count; i++)
        {
            AddCard(HoldLst[i]);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < WithoutLst.Count; i++)
        {
            if (EnableCompound(WithoutLst[i]))
                WithoutAndEnableCompLst.Add(WithoutLst[i]);
            else
                WithoutAndUnableCompLst.Add(WithoutLst[i]);            
        }
        for (int i = 0; i < WithoutAndEnableCompLst.Count; i++)
        {
            AddCard(WithoutAndEnableCompLst[i]);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < WithoutAndUnableCompLst.Count; i++)
        {
            AddCard(WithoutAndUnableCompLst[i]);
            yield return new WaitForEndOfFrame();
        }

        CardLstRoot.GetComponent<UIScrollView>().ResetPosition();
    }
    private void AddCard(int CardID)
    {
        GameObject newUnit = NGUITools.AddChild(CardTable.gameObject, CardUnitPrefab);
        newUnit.SetActive(true);
        newUnit.name = "Card_" + CardID;

        UI_MagicCard mc = newUnit.GetComponent<UI_MagicCard>();
        mc.Show(CardID);

        CardTable.repositionNow = true;
    }
    private bool EnableCompound(int CardID)
    {
        MagicCardConfig MCCfg = null;
        bool isEnableComp = false;
        if (CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(CardID, out MCCfg))
        {
            isEnableComp = (MCCfg.NeedItemsLst.Count > 0);
            for (int i = 0; i < MCCfg.NeedItemsLst.Count; i++)
            {
                if (SerPlayerData.GetItemCount(MCCfg.NeedItemsLst[i].ItemID) == 0)
                {
                    isEnableComp = false;
                    break;
                }
            }
        }
        return isEnableComp;
    }

    public void OnClick_Quality(GameObject btn)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        CurQuality = int.Parse(MyTools.GetLastString(btn.name, '_'));

        Debug.Log("点击【" + CurQuality + "阶】");

        StartCoroutine("RefreshCardList");
    }

    public void ShowCompound(int CardID)
    {
        TweenAlpha.Begin(CompoundRoot, 0.2f, 1);

        OutputCard.UnconditionalShow(CardID);

        MagicCardConfig MCCfg = null;
        if (CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(CardID, out MCCfg))
        {
            for (int i = 0; i < MCCfg.NeedItemsLst.Count; i++)
            {
                if (i < MaterialCards.Length)
                {
                    MaterialCards[i].UnconditionalShow(MCCfg.NeedItemsLst[i].ItemID);
                }
            }
        }
    }
    public void OnClick_HideCompound()
    {
        TweenAlpha.Begin(CompoundRoot, 0.2f, 0);
    }

    /*public void OnClick_CompoundByMagicPower()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        GameApp.Instance.CommonMsgDlg.OpenMsgBox(StringBuilderTool.ToString("是否消耗", OutputCard.MCCfg.NeedCurrencyItemInfo.ItemCnt, "点魔力值合成", OutputCard.FullName(), "?"),
                    (isCompound) =>
                    {
                        if (isCompound)
                        {
                            if (GameApp.Instance.MainPlayerData.MagicPower >= OutputCard.MCCfg.NeedCurrencyItemInfo.ItemCnt)
                            {
                                GameApp.Instance.MainPlayerData.MagicPower -= OutputCard.MCCfg.NeedCurrencyItemInfo.ItemCnt;

                                GameApp.Instance.AddCardHoldCount(OutputCard.MCCfg.CardID, 1);
                                
                                CompoundSuccess();
                            }
                            else
                            {
                                GameApp.Instance.CommonMsgDlg.OpenMsgBox("魔力值不足！");
                            }
                        }
                    });
    }*/

    public void OnClick_CompoundByCard()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        GameApp.Instance.CommonMsgDlg.OpenMsgBox(StringBuilderTool.ToString("是否消耗", OutputCard.MCCfg.NeedItemsLst.Count, "张材料卡牌合成", OutputCard.FullName(), "?"),
                   (isCompound) =>
                   {
                       if (isCompound)
                       {
                           if (GameApp.Instance.PlayerData != null)
                           {
                               GameApp.SendMsg.Compose(common.ComposeType.ComposeType_Item, (uint)OutputCard.MCCfg.CardID);
                           }
                           else
                           {
                               GameApp.Instance.AddCardHoldCount(OutputCard.MCCfg.CardID, 1);
                               for (int i = 0; i < OutputCard.MCCfg.NeedItemsLst.Count; i++)
                               {
                                   if (i < MaterialCards.Length)
                                   {
                                       GameApp.Instance.AddCardHoldCount(OutputCard.MCCfg.NeedItemsLst[i].ItemID, -1);
                                   }
                               }
                               CompoundSuccess(OutputCard.MCCfg.CardID);
                           }
                       }
                   });
    }

    public void CompoundSuccess(int CardID)
    {
        StartCoroutine("_CompoundSuccess", CardID);
    }
    IEnumerator _CompoundSuccess(int CardID)
    {
        GameObject eff = Resources.Load<GameObject>("Prefabs/Effect/Effect_UI_kapaihecheng001");
        if (eff != null)
        {
            CompoundEff = GameObject.Instantiate(eff);
            CompoundEff.transform.parent = CompoundRoot.transform;
            CompoundEff.transform.localPosition = Vector3.zero;
            CompoundEff.transform.localEulerAngles = Vector3.zero;
            CompoundEff.transform.localScale = Vector3.one * 360f;

            Transform OBg = CompoundEff.transform.Find("kapai/kapai_aaa/kapai_bg_aaaaaa001/GameObject/Quad");
            Material OBgMat = OBg.GetComponent<MeshRenderer>().material;
            OBgMat.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("BigUITexture/", OutputCard.Bg.spriteName)) as Texture;

            Transform OCard = CompoundEff.transform.Find("kapai/kapai_aaa/kapai_hechengaaaaaaa/GameObject/Quad");
            Material OCardMat = OCard.GetComponent<MeshRenderer>().material;
            OCardMat.mainTexture = OutputCard.Icon.mainTexture;

            for (int i = 0; i < MaterialCards.Length; i++)
            {
                Transform Bg = CompoundEff.transform.Find(StringBuilderTool.ToString("kapai/kapai_00", (i + 1), "/kapai_bg_aaaaaa001/GameObject/Quad"));
                Material BgMat = Bg.GetComponent<MeshRenderer>().material;
                BgMat.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("BigUITexture/", MaterialCards[i].Bg.spriteName)) as Texture;

                Transform Card = CompoundEff.transform.Find(StringBuilderTool.ToString("kapai/kapai_00", (i + 1), "/kapai_hechengaaaaaaa/GameObject/Quad"));
                Material CardMat = Card.GetComponent<MeshRenderer>().material;
                CardMat.mainTexture = MaterialCards[i].Icon.mainTexture;
            }
        }

        yield return new WaitForSeconds(0.5f);

        ResultTitle.text = StringBuilderTool.ToString("恭喜您，获得 [FEE209]", OutputCard.FullName(), "[-]");
        ResultPanel.SetActive(true);
        TweenAlpha.Begin(ResultPanel, 0.2f, 1).from = 0;

        OnClick_HideCompound();

        yield return new WaitForSeconds(0.5f);

        UI_MagicCard.AddShowNewSignCardID(CardID);

        StartCoroutine("RefreshCardList");

        //GameApp.Instance.CommonHintDlg.OpenHintBox("合成成功！");

        //ResultCard.UnconditionalShow(CardID);
    }

    public void OnClick_CloseResult()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭结果界面】");

        TweenAlpha.Begin(ResultPanel, 0.2f, 0).onFinished.Add(new EventDelegate(() =>
        {
            ResultPanel.SetActive(false);
            Destroy(CompoundEff);
        }));
    }

    int CurShowDetailsCardID = 0;
    public void ShowCardDetails(int CardID)
    {
        TweenAlpha.Begin(QualityRoot, 0.2f, 0);
        TweenAlpha.Begin(CardDetailsRoot, 0.2f, 1).from = 0;

        _ShowCardDetails(CardID);
    }
    void _ShowCardDetails(int CardID)
    {
        CurShowDetailsCardID = CardID;
        DetailsCard.UnconditionalShow(CardID);

        GroupConfig gc = null;
        CsvConfigTables.Instance.GroupCsvDic.TryGetValue(CurGroupID, out gc);
        if (gc != null)
        {
            GroupBg.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("BigUITexture/", gc.BHpg)) as Texture;

            GroupPlatform.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("BigUITexture/", gc.TBpg)) as Texture;
            GroupPlatform.MakePixelPerfect();
        }

        MagicCardConfig MCCfg = null;
        CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(CardID, out MCCfg);
        if (MCCfg != null)
        {
            CardModelRoot.localScale = Vector3.one;

            GameObject MonsterObj = Resources.Load<GameObject>("Prefabs/Actor/" + MCCfg.ModelName);
            if (MonsterObj != null)
            {
                CardModel = GameObject.Instantiate(MonsterObj);
                MyTools.setLayerDeep(CardModel, LayerMask.NameToLayer("UI"));
                CardModel.transform.parent = CardModelRoot;
                {
                    Vector3 pos = Vector3.zero;
                    if (gc != null && gc.Mroot == 1)
                    {
                        pos = new Vector3(0, 194, 0);
                    }
                    CardModel.transform.localPosition = pos;
                }
                CardModel.transform.localEulerAngles = new Vector3(3.8f, -133, 4.4f);
				CardModel.transform.localScale = MonsterObj.transform.localScale * 746;

                /*GameObject ReferencerObj = Resources.Load<GameObject>("Prefabs/Actor/DemoRole_Girl_3");
                if (ReferencerObj != null)
                {
                    ReferencerModel = GameObject.Instantiate(ReferencerObj);
                    MyTools.setLayerDeep(ReferencerModel, LayerMask.NameToLayer("UI"));
                    ReferencerModel.transform.parent = CardModelRoot;
                    ReferencerModel.transform.localPosition = new Vector3(-24, 0, 0);
                    ReferencerModel.transform.localEulerAngles = new Vector3(0, 180, 0);
                    ReferencerModel.transform.localScale = Vector3.one * 20;
                }*/
            }
        }

        if (CardModelRoot != null)
        {
            //Debug.Log(CardID);
            switch (CardID)
            {
                case 50001:
                    //ReferencerModel.transform.localPosition = new Vector3(-146, 0, 0);
                    break;
                case 50002:
                    //ReferencerModel.transform.localPosition = new Vector3(-102, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 1.34f;
                    break;
                case 50003:
                    break;
                case 50004:
                    //ReferencerModel.transform.localPosition = new Vector3(-115, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 1.68f;
                    break;
                case 50005:
                    break;
                case 50006:
                    //ReferencerModel.transform.localPosition = new Vector3(-63, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 2.95f;
                    break;
                case 50007:
                    //ReferencerModel.transform.localPosition = new Vector3(-56.7f, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 2.38f;
                    break;
                case 50008:
                    //ReferencerModel.transform.localPosition = new Vector3(-266, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 0.7f;
                    break;
                case 50009:
                    //ReferencerModel.transform.localPosition = new Vector3(-82.6f, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 2.5f;
                    break;
                case 50010:
                    //ReferencerModel.transform.localPosition = new Vector3(-49.8f, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 4.56f;
                    break;
                case 50011:
                    //ReferencerModel.transform.localPosition = new Vector3(-116.7f, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 1.63f;
                    break;
                case 50012:
                    //ReferencerModel.transform.localPosition = new Vector3(-73.2f, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 2.32f;
                    break;
                case 50013:
                    //ReferencerModel.transform.localPosition = new Vector3(-100.8f, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 1.88f;
                    break;
                case 50014:
                    //ReferencerModel.transform.localPosition = new Vector3(-60, 0, 0);
                    CardModelRoot.localScale = Vector3.one * 3.65f;
                    break;
                case 50015:
                    CardModelRoot.localScale = Vector3.one * 9;
                    break;
                case 50016:
                    CardModelRoot.localScale = Vector3.one * 9;
                    break;
                case 50017:
                    break;
                case 50018:
                    break;
                case 50019:
                    break;
                case 50020:
                    break;
                case 50021:
                    break;
                case 50022:
                    CardModelRoot.localScale = Vector3.one * 9;
                    break;
                case 50023:
                    break;
                case 50024:
                    break;
            }
        }
        
        int index = HoldLst.IndexOf(CurShowDetailsCardID);
        PrevBtn.SetActive(index != 0);
        NextBtn.SetActive(index != HoldLst.Count - 1);
    }
    public void HideCardDetails()
    {
        TweenAlpha.Begin(QualityRoot, 0.2f, 1);
        TweenAlpha.Begin(CardDetailsRoot, 0.2f, 0);

        GameApp.Instance.SoundInstance.StopAllSe();

        StopCoroutine("_PlayCardModelClickAnim");
        MyTools.DestroyChildNodes(CardModelRoot);
    }
    public void ShowPrevCardDetails()
    {
        int index = HoldLst.IndexOf(CurShowDetailsCardID);
        int prevIdx = index - 1;

        PrevBtn.SetActive(prevIdx != 1);
        NextBtn.SetActive(true);

        MyTools.DestroyImmediateChildNodes(CardModelRoot);
        _ShowCardDetails(HoldLst[prevIdx]);
    }
    public void ShowNextCardDetails()
    {
        int index = HoldLst.IndexOf(CurShowDetailsCardID);
        int nextIdx = index + 1;

        NextBtn.SetActive(nextIdx != HoldLst.Count - 1);
        PrevBtn.SetActive(true);

        MyTools.DestroyImmediateChildNodes(CardModelRoot);
        _ShowCardDetails(HoldLst[nextIdx]);
    }

    /// <summary> 点击魔卡模型 </summary>
    public void OnClick_CardModel()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【魔卡模型】");

        StartCoroutine("_PlayCardModelClickAnim");
    }
    IEnumerator _PlayCardModelClickAnim()
    {
        Animation anim = CardModel.GetComponent<Animation>();
        anim.CrossFade("click", 0.2f);
        yield return new WaitForSeconds(anim["click"].length);
        anim.CrossFade("standby", 0.2f);
    }
    void OnDragStart(GameObject obj)
    {

    }
    void OnDrag(GameObject obj, Vector2 delta)
    {
        CardModel.transform.Rotate(0, delta.x * -0.5f, 0);
    }
}
