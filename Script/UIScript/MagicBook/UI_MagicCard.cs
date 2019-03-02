using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*public enum DetailsShowType
{
    e
}*/

public class UI_MagicCard : MonoBehaviour
{
    [HideInInspector]
    public MagicCardConfig MCCfg = null;

    public UISprite Bg;
    public UISprite QualitySign;
    public UILabel Name;
    public UILabel MagicVal;
    public UILabel Des;
    public UILabel Count;
    public UITexture Icon;
    public UISprite[] Star;
    public UISprite NewSign;
    public GameObject EnableCompound;

    public UILabel System;
    public UILabel Theme;
    public UILabel Quality;
    public UILabel Epoch;
    public UILabel Family;
    public UILabel LatinName;
    public UILabel Satellite;
    public UILabel Temperature;
    public UILabel Distance;
    public UILabel Diameter;
    public UILabel Day;
    public UILabel Year;
    public UILabel Height;
    public UILabel Habitat;
    public UILabel Food;
    public UILabel Weight;
    public UILabel Title;
    public UILabel Time;
    public UILabel Ruler;

    public UILabel GetWayFromStage;
    
    public UILabel Price;
    public GameObject PurchasedSign;
    public UILabel ExchangeLab;
    
    public GameObject Sel;

    static List<int> ShowNewSignCardIDLst = new List<int>();
    static public void AddShowNewSignCardID(int CardID)
    {
        if (ShowNewSignCardIDLst.Contains(CardID))
            return;
        ShowNewSignCardIDLst.Add(CardID);
    }
    static public void RemoveShowNewSignCardID(int CardID)
    {
        if (ShowNewSignCardIDLst.Contains(CardID))
        {
            ShowNewSignCardIDLst.Remove(CardID);
        }
    }

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

    public void Show(int CardID)
    {
        if (CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(CardID, out MCCfg))
        {
            gameObject.SetActive(true);

            Name.text = MCCfg.Name;
            
            if (MagicVal != null)
                MagicVal.text = MCCfg.MagicVal.ToString();

            if (Des != null)
                Des.text = MCCfg.Describe;

            if (Count != null)
                Count.text = SerPlayerData.GetItemCount(MCCfg.CardID).ToString();

            if (SerPlayerData.GetItemCount(MCCfg.CardID) > 0)
            {
                Icon.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("MagicCard/", MCCfg.ColouredIcon)) as Texture;

                for (int i = 0; i < Star.Length; i++)
                {
                    Star[i].spriteName = "c_t_star01";
                    Star[i].gameObject.SetActive(i < MCCfg.StarLv);
                }

                if (Bg != null)
                {
                    switch (MCCfg.Quality)
                    {
                        case 1:
                            Bg.spriteName = "lvbian";
                            break;
                        case 2:
                            Bg.spriteName = "lanbian";
                            break;
                        case 3:
                            Bg.spriteName = "zibian";
                            break;
                    }
                }

                if (QualitySign != null)
                {
                    switch (MCCfg.Quality)
                    {
                        case 1:
                            QualitySign.spriteName = "lvbiao";
                            break;
                        case 2:
                            QualitySign.spriteName = "lanbiao";
                            break;
                        case 3:
                            QualitySign.spriteName = "zibiao";
                            break;
                    }
                    QualitySign.MakePixelPerfect();
                }
            }
            else
            {
                Icon.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("MagicCard/", MCCfg.ColorlessIcon)) as Texture;

                for (int i = 0; i < Star.Length; i++)
                {
                    Star[i].spriteName = "c_t_star01_hui";
                    Star[i].gameObject.SetActive(i < MCCfg.StarLv);
                }

                if (Bg != null)
                {
                    Bg.spriteName = "huibian";
                }

                if (QualitySign != null)
                {
                    QualitySign.spriteName = "";
                }
            }

            bool isEnableComp = (MCCfg.NeedItemsLst.Count > 0);
            if(isEnableComp)
            {
                bool MaterialCardsNotEnough = false;
                Dictionary<int, int> ItemsDic = new Dictionary<int, int>();
                for (int i = 0; i < MCCfg.NeedItemsLst.Count; i++)
                {
                    if (ItemsDic.ContainsKey(MCCfg.NeedItemsLst[i].ItemID))
                    {
                        ItemsDic[MCCfg.NeedItemsLst[i].ItemID] += MCCfg.NeedItemsLst[i].ItemCnt;
                    }
                    else
                    {
                        ItemsDic.Add(MCCfg.NeedItemsLst[i].ItemID, MCCfg.NeedItemsLst[i].ItemCnt);
                    }
                }
                foreach (KeyValuePair<int, int> pair in ItemsDic)
                {
                    if (SerPlayerData.GetItemCount(pair.Key) < pair.Value)
                    {
                        MaterialCardsNotEnough = true;
                        break;
                    }
                }
                if (MaterialCardsNotEnough)
                {
                    isEnableComp = false;
                }
            }
            if (EnableCompound != null)
            {
                EnableCompound.SetActive(isEnableComp);
            }

            if (NewSign != null)
            {
                if (ShowNewSignCardIDLst.Contains(CardID))
                    NewSign.alpha = 1;
                else
                    NewSign.alpha = 0;
            }

            switch (MCCfg.StarLv)
            {
                case 1:
                    Star[0].transform.localPosition = Vector3.zero;
                    break;
                case 2:
                    Star[0].transform.localPosition = new Vector3(-12, 0, 0);
                    Star[1].transform.localPosition = new Vector3(12, 0, 0);
                    break;
                case 3:
                    Star[0].transform.localPosition = new Vector3(-24, 0, 0);
                    Star[1].transform.localPosition = Vector3.zero;
                    Star[2].transform.localPosition = new Vector3(24, 0, 0);
                    break;
                case 4:
                    Star[0].transform.localPosition = new Vector3(-36, 0, 0);
                    Star[1].transform.localPosition = new Vector3(-12, 0, 0);
                    Star[2].transform.localPosition = new Vector3(12, 0, 0);
                    Star[3].transform.localPosition = new Vector3(36, 0, 0);
                    break;
                case 5:
                    break;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void UnconditionalShow(int CardID)
    {
        if (CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(CardID, out MCCfg))
        {
            gameObject.SetActive(true);

            Name.text = MCCfg.Name;

            if (MagicVal != null)
                MagicVal.text = MCCfg.MagicVal.ToString();

            if (Des != null)
            {
                Des.text = StringBuilderTool.ToInfoString("　　", MCCfg.Describe.Replace("\\n", "\n"));
                BoxCollider bc = Des.gameObject.GetComponent<BoxCollider>();
                if(bc != null)
                {
                    bc.center = new Vector3(Des.width / 2, -Des.height/2,0);
                    bc.size = new Vector3(Des.width, Des.height,0);
                }
                UITable tab = Des.transform.parent.GetComponent<UITable>();
                if(tab != null)
                {
                    tab.Reposition();
                    UIScrollView sv = tab.transform.parent.GetComponent<UIScrollView>();
                    if (sv != null)
                    {
                        sv.ResetPosition();
                    }
                }
            }

            if (Count != null)
                Count.text = SerPlayerData.GetItemCount(MCCfg.CardID).ToString();

            Icon.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("MagicCard/", MCCfg.ColouredIcon)) as Texture;

            for (int i = 0; i < Star.Length; i++)
            {
                Star[i].spriteName = "c_t_star01";
                Star[i].gameObject.SetActive(i < MCCfg.StarLv);
            }


            if (MCCfg.ThemeID == 101)//恐龙世界
            {
                if (Title != null)
                    Title.gameObject.SetActive(true);
                if (LatinName != null)
                    LatinName.gameObject.SetActive(true);
                if (Epoch != null)
                    Epoch.gameObject.SetActive(true);
                if (Family != null)
                    Family.gameObject.SetActive(true);                
                if (Weight != null)
                    Weight.gameObject.SetActive(false);
                if (Height != null)
                    Height.gameObject.SetActive(true);
                if (Habitat != null)
                    Habitat.gameObject.SetActive(true);
                if (Food != null)
                    Food.gameObject.SetActive(true);
                if (Satellite != null)
                    Satellite.gameObject.SetActive(false);
                if (Distance != null)
                    Distance.gameObject.SetActive(false);
                if (Temperature != null)
                    Temperature.gameObject.SetActive(false);
                if (Diameter != null)
                    Diameter.gameObject.SetActive(false);
                if (Day != null)
                    Day.gameObject.SetActive(false);
                if (Year != null)
                    Year.gameObject.SetActive(false);
                if (Time != null)
                    Time.gameObject.SetActive(false);
                if (Ruler != null)
                    Ruler.gameObject.SetActive(false);
            }
            else if (MCCfg.ThemeID == 102)//动物园
            {
                if (Title != null)
                    Title.gameObject.SetActive(true);
                if (LatinName != null)
                    LatinName.gameObject.SetActive(true);
                if (Epoch != null)
                    Epoch.gameObject.SetActive(false);
                if (Family != null)
                    Family.gameObject.SetActive(false);
                if (Weight != null)
                    Weight.gameObject.SetActive(true);
                if (Height != null)
                    Height.gameObject.SetActive(true);
                if (Habitat != null)
                    Habitat.gameObject.SetActive(true);
                if (Food != null)
                    Food.gameObject.SetActive(true);
                if (Satellite != null)
                    Satellite.gameObject.SetActive(false);
                if (Distance != null)
                    Distance.gameObject.SetActive(false);
                if (Temperature != null)
                    Temperature.gameObject.SetActive(false);
                if (Diameter != null)
                    Diameter.gameObject.SetActive(false);
                if (Day != null)
                    Day.gameObject.SetActive(false);
                if (Year != null)
                    Year.gameObject.SetActive(false);
                if (Time != null)
                    Time.gameObject.SetActive(false);
                if (Ruler != null)
                    Ruler.gameObject.SetActive(false);
            }
            else if (MCCfg.ThemeID == 103)//海洋世界
            {
                if (Title != null)
                    Title.gameObject.SetActive(true);
                if (LatinName != null)
                    LatinName.gameObject.SetActive(true);
                if (Epoch != null)
                    Epoch.gameObject.SetActive(false);
                if (Family != null)
                    Family.gameObject.SetActive(false);                
                if (Weight != null)
                    Weight.gameObject.SetActive(true);
                if (Height != null)
                    Height.gameObject.SetActive(true);
                if (Habitat != null)
                    Habitat.gameObject.SetActive(true);
                if (Food != null)
                    Food.gameObject.SetActive(true);
                if (Satellite != null)
                    Satellite.gameObject.SetActive(false);
                if (Distance != null)
                    Distance.gameObject.SetActive(false);
                if (Temperature != null)
                    Temperature.gameObject.SetActive(false);
                if (Diameter != null)
                    Diameter.gameObject.SetActive(false);
                if (Day != null)
                    Day.gameObject.SetActive(false);
                if (Year != null)
                    Year.gameObject.SetActive(false);
                if (Time != null)
                    Time.gameObject.SetActive(false);
                if (Ruler != null)
                    Ruler.gameObject.SetActive(false);
            }
            else if (MCCfg.ThemeID == 201)//太阳系
            {
                if (Title != null)
                    Title.gameObject.SetActive(true);
                if (LatinName != null)
                    LatinName.gameObject.SetActive(true);
                if (Epoch != null)
                    Epoch.gameObject.SetActive(false);
                if (Family != null)
                    Family.gameObject.SetActive(false);
                if (Weight != null)
                    Weight.gameObject.SetActive(false);
                if (Height != null)
                    Height.gameObject.SetActive(false);
                if (Habitat != null)
                    Habitat.gameObject.SetActive(false);
                if (Food != null)
                    Food.gameObject.SetActive(false);
                if (Satellite != null)
                    Satellite.gameObject.SetActive(true);
                if (Distance != null)
                    Distance.gameObject.SetActive(true);
                if (Temperature != null)
                    Temperature.gameObject.SetActive(true);
                if (Diameter != null)
                    Diameter.gameObject.SetActive(true);
                if (Day != null)
                    Day.gameObject.SetActive(true);
                if (Year != null)
                    Year.gameObject.SetActive(true);
                if (Time != null)
                    Time.gameObject.SetActive(false);
                if (Ruler != null)
                    Ruler.gameObject.SetActive(false);
            }
            else if (MCCfg.ThemeID == 601)//春节
            {
                if (Title != null)
                    Title.gameObject.SetActive(false);
                if (LatinName != null)
                    LatinName.gameObject.SetActive(false);
                if (Epoch != null)
                    Epoch.gameObject.SetActive(false);
                if (Family != null)
                    Family.gameObject.SetActive(false);
                if (Weight != null)
                    Weight.gameObject.SetActive(false);
                if (Height != null)
                    Height.gameObject.SetActive(false);
                if (Habitat != null)
                    Habitat.gameObject.SetActive(false);
                if (Food != null)
                    Food.gameObject.SetActive(false);
                if (Satellite != null)
                    Satellite.gameObject.SetActive(false);
                if (Distance != null)
                    Distance.gameObject.SetActive(false);
                if (Temperature != null)
                    Temperature.gameObject.SetActive(false);
                if (Diameter != null)
                    Diameter.gameObject.SetActive(false);
                if (Day != null)
                    Day.gameObject.SetActive(false);
                if (Year != null)
                    Year.gameObject.SetActive(false);
                if (Time != null)
                    Time.gameObject.SetActive(true);
                if (Ruler != null)
                    Ruler.gameObject.SetActive(false);
            }
            else if (MCCfg.ThemeID == 603)//星座
            {
                if (Title != null)
                    Title.gameObject.SetActive(false);
                if (LatinName != null)
                    LatinName.gameObject.SetActive(true);
                if (Epoch != null)
                    Epoch.gameObject.SetActive(false);
                if (Family != null)
                    Family.gameObject.SetActive(false);
                if (Weight != null)
                    Weight.gameObject.SetActive(false);
                if (Height != null)
                    Height.gameObject.SetActive(false);
                if (Habitat != null)
                    Habitat.gameObject.SetActive(false);
                if (Food != null)
                    Food.gameObject.SetActive(false);
                if (Satellite != null)
                    Satellite.gameObject.SetActive(false);
                if (Distance != null)
                    Distance.gameObject.SetActive(false);
                if (Temperature != null)
                    Temperature.gameObject.SetActive(false);
                if (Diameter != null)
                    Diameter.gameObject.SetActive(false);
                if (Day != null)
                    Day.gameObject.SetActive(false);
                if (Year != null)
                    Year.gameObject.SetActive(false);
                if (Time != null)
                    Time.gameObject.SetActive(true);
                if (Ruler != null)
                    Ruler.gameObject.SetActive(true);
            }

            if (Bg != null)
            {
                switch (MCCfg.Quality)
                {
                    case 1:
                        Bg.spriteName = "lvbian";
                        break;
                    case 2:
                        Bg.spriteName = "lanbian";
                        break;
                    case 3:
                        Bg.spriteName = "zibian";
                        break;
                }
            }

            if (QualitySign != null)
            {
                switch (MCCfg.Quality)
                {
                    case 1:
                        QualitySign.spriteName = "lvbiao";
                        break;
                    case 2:
                        QualitySign.spriteName = "lanbiao";
                        break;
                    case 3:
                        QualitySign.spriteName = "zibiao";
                        break;
                }
                QualitySign.MakePixelPerfect();
            }

            if (System != null)
            {
                ChapterConfig cc = null;
                if (CsvConfigTables.Instance.ChapterCsvDic.TryGetValue(MCCfg.SystemID, out cc))
                {
                    System.text = cc.Name;
                }
            }

            if (Theme != null)
            {
                GroupConfig gc = null;
                if (CsvConfigTables.Instance.GroupCsvDic.TryGetValue(MCCfg.ThemeID, out gc))
                {
                    Theme.text = gc.Name;
                }
            }

            if (Quality != null)
            {
                Quality.text = StringBuilderTool.ToInfoString(ArabicToChinese(MCCfg.Quality), "阶卡");
            }

            if (Epoch != null)
            {
                Epoch.text = MCCfg.Epoch;
            }

            if (Family != null)
            {
                Family.text = MCCfg.Family;
            }
            
            if (LatinName != null)
            {
                LatinName.text = MCCfg.LatinName;
            }

            if (Height != null)
            {
                Height.text = MCCfg.Height;
            }
            
            if (Habitat != null)
            {
                Habitat.text = MCCfg.Habitat;
            }

            if (Food != null)
            {
                Food.text = MCCfg.Food;
            }

            if (Weight != null)
            {
                if (MCCfg.Weight.Length == 0)
                    Weight.gameObject.SetActive(false);
                Weight.text = MCCfg.Weight;
            }

            if (Satellite != null)
            {
                if (MCCfg.Satellite.Length == 0)
                    Satellite.gameObject.SetActive(false);
                Satellite.text = MCCfg.Satellite;
            }

            if (Temperature != null)
            {
                if (MCCfg.Temperature.Length == 0)
                    Temperature.gameObject.SetActive(false);
                Temperature.text = MCCfg.Temperature;
            }

            if (Distance != null)
            {
                if (MCCfg.Distance.Length == 0)
                    Distance.gameObject.SetActive(false);
                Distance.text = MCCfg.Distance;
            }

            if (Diameter != null)
            {
                if (MCCfg.Diameter.Length == 0)
                    Diameter.gameObject.SetActive(false);
                Diameter.text = MCCfg.Diameter;
            }

            if (Day != null)
            {
                if (MCCfg.Day.Length == 0)
                    Day.gameObject.SetActive(false);
                Day.text = MCCfg.Day;
            }

            if (Year != null)
            {
                if (MCCfg.Year.Length == 0)
                    Year.gameObject.SetActive(false);
                Year.text = MCCfg.Year;
            }

            if (Time != null)
            {
                if (MCCfg.Time.Length == 0)
                    Time.gameObject.SetActive(false);
                Time.text = MCCfg.Time;
            }

            if (Title != null)
            {
                if (MCCfg.Title.Length == 0)
                    Title.gameObject.SetActive(false);
                Title.text = MCCfg.Title;
            }

            if (GetWayFromStage != null)
            {
                bool showShortcut = (MCCfg.Shortcut != 0);
                GetWayFromStage.gameObject.SetActive(showShortcut);
                if(showShortcut)
                {
                    StageConfig sc = null;
                    if (CsvConfigTables.Instance.StageCsvDic.TryGetValue(MCCfg.Shortcut, out sc))
                    {
                        GetWayFromStage.text = sc.Name;
                    }
                }
            }

            if (EnableCompound != null)
                EnableCompound.SetActive(false);

            if (NewSign != null)
                NewSign.alpha = 0;

            switch (MCCfg.StarLv)
            {
                case 1:
                    Star[0].transform.localPosition = Vector3.zero;
                    break;
                case 2:
                    Star[0].transform.localPosition = new Vector3(-12, 0, 0);
                    Star[1].transform.localPosition = new Vector3(12, 0, 0);
                    break;
                case 3:
                    Star[0].transform.localPosition = new Vector3(-24, 0, 0);
                    Star[1].transform.localPosition = Vector3.zero;
                    Star[2].transform.localPosition = new Vector3(24, 0, 0);
                    break;
                case 4:
                    Star[0].transform.localPosition = new Vector3(-36, 0, 0);
                    Star[1].transform.localPosition = new Vector3(-12, 0, 0);
                    Star[2].transform.localPosition = new Vector3(12, 0, 0);
                    Star[3].transform.localPosition = new Vector3(36, 0, 0);
                    break;
                case 5:
                    break;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetPrice(int nPrice)
    {
        PriceValue = nPrice;

        if(Price != null)
            Price.text = PriceValue.ToString();
    }

    [HideInInspector]
    public int PriceValue = 0;
    public void SetPurchased()
    {
        Price.text = "已购买";
        Price.color = Color.white;
        Price.transform.localPosition = new Vector3(-38,-122,0);
        transform.Find("PriceIcon").gameObject.SetActive(false);
        transform.Find("Bg").GetComponent<UISprite>().spriteName = "tupianbiankuang";
        PurchasedSign.SetActive(true);
    }
    public void RecoverFromPurchased()
    {
        Price.text = StringBuilderTool.ToString("[FFFB63]",PriceValue,"[-]");
        Price.transform.localPosition = new Vector3(-10, -122, 0);
        transform.Find("PriceIcon").gameObject.SetActive(true);
        transform.Find("Bg").GetComponent<UISprite>().spriteName = "lankuang";
        PurchasedSign.SetActive(false);
    }

    public void SetExchanged()
    {
        ExchangeLab.text = "已交换";
        transform.GetComponent<UIButton>().isEnabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
    }

    public void ShowSelSign(bool isShow)
    {
        if (Sel != null)
        {
            if (isShow && !Sel.activeSelf)
            {
                Sel.SetActive(true);
                TweenAlpha.Begin(Sel, 0.1f, 1);
            }
            else if (!isShow && Sel.activeSelf)
            {
                TweenAlpha.Begin(Sel, 0.1f, 0).onFinished.Add(new EventDelegate(() =>
                {
                    Sel.SetActive(false);
                }));
            }
        }
    }

    public void SetCustomColoringTexture(int fishIndex)
    {
        if (fishIndex < 0)
        {
            Icon.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("MagicCard/", MCCfg.ColouredIcon)) as Texture;
            Icon.transform.localPosition = Vector3.zero;
            Icon.width = 154;
            Icon.height = 204;
            return;
        }
        StartCoroutine("ReadTexture", fishIndex);
    }
    IEnumerator ReadTexture(int fishIndex)
    {
        string url = StringBuilderTool.ToString("file://", Application.persistentDataPath, "/NewFishTexture_", fishIndex, ".jpg");

        WWW www = new WWW(url);
        yield return www;
        if (www.isDone && www.error == null)
        {
            Icon.mainTexture = www.texture;
        }
        else
        {
            Icon.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("BigUITexture/NoColoring")) as Texture;
        }
        Icon.transform.localPosition = new Vector3(0, 20, 0);
        Icon.width = 148;
        Icon.height = 148;
    }

    public void OnClick()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击："+ Name.text);

        //if (MCCfg != null && GameApp.Instance.CardHoldCountLst[MCCfg.CardID] > 0)
        //{
            //GameApp.Instance.HomePageUI.MagicBookUI.FullCard.Show(MCCfg.CardID);

            //Debug.Log(Name.text);
        //}
        //else
        //{
            //GameApp.Instance.CommonHintDlg.OpenHintBox("请先收集到此魔卡后再查看详情！");
        //}
        if (GameApp.Instance.TravelUI != null)
        {
            if (GameApp.Instance.TravelUI.TravelSeafloor != null)
            {
                GameApp.Instance.TravelUI.TravelSeafloor.PutFishModelInScene(MCCfg);
            }
            return;
        }

        if (Price != null)
        {
            Debug.Log("显示购买确认");
            GameApp.Instance.HomePageUI.MarketUI.ShowPurchaseConfirmation(gameObject);
            return;
        }

        if (ExchangeLab != null)
        {
            Debug.Log("显示可换魔卡界面");
            GameApp.Instance.HomePageUI.MarketUI.ShowAbleExchangeCardLst(this);
            return;
        }

        if (Sel != null)
        {
            Debug.Log("选中待更换的魔卡");
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                Transform child = transform.parent.GetChild(i);
                if (child.name == transform.name)
                    continue;

                UI_MagicCard mc = child.GetComponent<UI_MagicCard>();
                mc.ShowSelSign(false);
            }
            ShowSelSign(true);
            GameApp.Instance.HomePageUI.MarketUI.SelectExchangeCard(this);
            return;
        }

        if (EnableCompound != null && EnableCompound.activeSelf)
        {
            if (GameApp.Instance.HomePageUI != null)
                GameApp.Instance.HomePageUI.MagicBookUI.ShowCompound(MCCfg.CardID);
            else if(GameApp.Instance.TravelUI != null)
                GameApp.Instance.TravelUI.MagicBookUI.ShowCompound(MCCfg.CardID);
        }
        else
        {
            if (SerPlayerData.GetItemCount(MCCfg.CardID) > 0)
            {
                RemoveShowNewSignCardID(MCCfg.CardID);
                if (NewSign != null)
                {
                    NewSign.alpha = 0;
                }

                if (GameApp.Instance.HomePageUI != null)
                    GameApp.Instance.HomePageUI.MagicBookUI.ShowCardDetails(MCCfg.CardID);
                else if (GameApp.Instance.TravelUI != null)
                    GameApp.Instance.TravelUI.MagicBookUI.ShowCardDetails(MCCfg.CardID);

            }
            else
            {
                GameApp.Instance.CommonHintDlg.OpenHintBox("请先收集到此魔卡后再查看详情！");
            }
            //GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString("查看", FullName(), "的卡牌详情！"));
        }
    }

    #region _按钮
    /// <summary> 关闭卡片界面 </summary>
    public void OnClick_Close()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭卡片界面】");

        GameApp.Instance.SoundInstance.StopSe(MCCfg.Voice);

        Show(-1);
    }
    /// <summary> 播放说明文字音频 </summary>
    public void OnClick_PlaySound()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【播放说明文字音频】");


        GameApp.Instance.SoundInstance.StopSe(MCCfg.Voice);
        GameApp.Instance.SoundInstance.PlayVoice(MCCfg.Voice);
    }
    /// <summary> 获取途径 </summary>
    public void OnClick_GetWay(GameObject btn)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        int idx = int.Parse(MyTools.GetLastString(btn.name, '_'));
        Debug.Log("点击【获取途径" + idx + "】");

        switch (idx)
        {
            case 1: 
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
    #endregion

    public string FullName()
    {
        string n = "";
        n += ArabicToChinese(MCCfg.StarLv);        
        n += "星";
        n += MCCfg.Name;
        return n;
    }

    public string ArabicToChinese(int num)
    {
        switch (num)
        {
            default:
            case 1: return "一";
            case 2: return "二";
            case 3: return "三";
            case 4: return "四";
            case 5: return "五";
        }
    }
}
