using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common;

public enum ETravelBackpackItemType
{
    eFood = 0,
    eLucky,
    eEquipment,
    eAll
}

public class UI_Travel_Backpack : MonoBehaviour
{
    [HideInInspector]
    public bool isShowing = false;

    public GameObject GridInfoPrefab;

    public GameObject PackageRoot;
    public GameObject PackageBtn;
    public GameObject RearrangeBtn;

    public GameObject PrepareRoot;
    public GameObject AffirmClearRoot;

    public GameObject ItemLstRoot;
    public UIToggle[] TypeTog;
    public GameObject ItemUnitPrefab;
    public UIGrid ItemGrid;

    public GameObject ClearPreparePanel;
    public UILabel ClearPrepareProgLab;

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

        if(GameApp.Instance.PlayerData != null)
        {
            switch ((ActionType)GameApp.Instance.PlayerData.m_player_action.m_type)
            {
                case ActionType.ActionType_Travel:
                    PrepareRoot.transform.localPosition = Vector3.zero;
                    PackageRoot.gameObject.SetActive(false);
                    PrepareRoot.gameObject.SetActive(true);
                    break;
                case ActionType.ActionType_Playing:
                    PackageRoot.transform.localPosition = Vector3.zero;
                    PrepareRoot.transform.localPosition = new Vector3(1100, 0, 0);
                    PackageRoot.gameObject.SetActive(true);
                    PrepareRoot.gameObject.SetActive(true);
                    break;
            }

            PlayerTravelBag ptb = GameApp.Instance.PlayerData.m_player_travel_bag;
            for (int i = 0; i < ptb.m_items.Count; i++)
            {
                PlayerTravelBagItem ptbi = ptb.m_items[i];
                ItemConfig ItemCfg = null;
                CsvConfigTables.Instance.ItemCsvDic.TryGetValue(SerPlayerData.SerIDToItemID(ptbi.m_item_id), out ItemCfg);
                if (ItemCfg != null)
                {
                    GameObject GridInfoObj = null;
                    switch ((PosType)ptbi.m_id)
                    {
                        case PosType.PosType_Bag_Food:
                            GridInfoObj = PackageRoot.transform.Find("Grid_Package_Food").gameObject;
                            break;
                        case PosType.PosType_Bag_Luck:
                            GridInfoObj = PackageRoot.transform.Find("Grid_Package_Lucky").gameObject;
                            break;
                        case PosType.PosType_Bag_Equip1:
                            GridInfoObj = PackageRoot.transform.Find("Grid_Package_Equipment1").gameObject;
                            break;
                        case PosType.PosType_Bag_Equip2:
                            GridInfoObj = PackageRoot.transform.Find("Grid_Package_Equipment2").gameObject;
                            break;
                        case PosType.PosType_Home_Food1:
                            GridInfoObj = PrepareRoot.transform.Find("Grid_Prepare_Food1").gameObject;
                            break;
                        case PosType.PosType_Home_Food2:
                            GridInfoObj = PrepareRoot.transform.Find("Grid_Prepare_Food2").gameObject;
                            break;
                        case PosType.PosType_Home_Luck1:
                            GridInfoObj = PrepareRoot.transform.Find("Grid_Prepare_Lucky1").gameObject;
                            break;
                        case PosType.PosType_Home_Luck2:
                            GridInfoObj = PrepareRoot.transform.Find("Grid_Prepare_Lucky2").gameObject;
                            break;
                        case PosType.PosType_Home_Equip1:
                            GridInfoObj = PrepareRoot.transform.Find("Grid_Prepare_Equipment1").gameObject;
                            break;
                        case PosType.PosType_Home_Equip2:
                            GridInfoObj = PrepareRoot.transform.Find("Grid_Prepare_Equipment2").gameObject;
                            break;
                        case PosType.PosType_Home_Equip3:
                            GridInfoObj = PrepareRoot.transform.Find("Grid_Prepare_Equipment3").gameObject;
                            break;
                        case PosType.PosType_Home_Equip4:
                            GridInfoObj = PrepareRoot.transform.Find("Grid_Prepare_Equipment4").gameObject;
                            break;
                        default:
                            break;
                    }
                    CreateGridInfo(GridInfoObj, new GridInfo(ptbi.m_item_id, ItemCfg.Icon));
                }
            }
        }
    }

    public void DirectShowShowItemLst()
    {
        isShowing = true;

        TweenAlpha.Begin(gameObject, 0.1f, 1);

        PackageRoot.gameObject.SetActive(false);
        PrepareRoot.gameObject.SetActive(false);

        ShowItemLst(ETravelBackpackItemType.eAll, null);
    }

    public void Hide()
    {
        if (CurWaitSetGridObj != null)
        {
            TweenAlpha.Begin(ItemLstRoot, 0.1f, 0);
            CurWaitSetGridObj = null;
            return;
        }

        isShowing = false;

        TweenAlpha.Begin(gameObject, 0.1f, 0);

        TweenAlpha.Begin(ItemLstRoot, 0.1f, 0);
    }

    public uint GetPosTypeFromGridName(string name)
    {
        switch (name)
        {
            case "Grid_Package_Food":
                return (uint)PosType.PosType_Bag_Food;
            case "Grid_Package_Lucky":
                return (uint)PosType.PosType_Bag_Luck;
            case "Grid_Package_Equipment1":
                return (uint)PosType.PosType_Bag_Equip1;
            case "Grid_Package_Equipment2":
                return (uint)PosType.PosType_Bag_Equip2;
            case "Grid_Prepare_Food1":
                return (uint)PosType.PosType_Home_Food1;
            case "Grid_Prepare_Food2":
                return (uint)PosType.PosType_Home_Food2;
            case "Grid_Prepare_Lucky1":
                return (uint)PosType.PosType_Home_Luck1;
            case "Grid_Prepare_Lucky2":
                return (uint)PosType.PosType_Home_Luck2;
            case "Grid_Prepare_Equipment1":
                return (uint)PosType.PosType_Home_Equip1;
            case "Grid_Prepare_Equipment2":
                return (uint)PosType.PosType_Home_Equip2;
            case "Grid_Prepare_Equipment3":
                return (uint)PosType.PosType_Home_Equip3;
            case "Grid_Prepare_Equipment4":
                return (uint)PosType.PosType_Home_Equip4;
            default:
                return 0;
        }
    }

    public void OnClick_Package_Grid(GameObject btnObj)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log(StringBuilderTool.ToInfoString("点击【背包-格子】", btnObj.name));

        CurTypeGrids.Clear();

        switch (btnObj.name)
        {
            case "Grid_Package_Food":
                CurTypeGrids.Add(btnObj);
                CurTypeGrids.Add(PrepareRoot.transform.Find("Grid_Prepare_Food1").gameObject);
                CurTypeGrids.Add(PrepareRoot.transform.Find("Grid_Prepare_Food2").gameObject);
                ShowItemLst(ETravelBackpackItemType.eFood, btnObj);
                break;
            case "Grid_Package_Lucky":
                CurTypeGrids.Add(btnObj);
                CurTypeGrids.Add(PrepareRoot.transform.Find("Grid_Prepare_Lucky1").gameObject);
                CurTypeGrids.Add(PrepareRoot.transform.Find("Grid_Prepare_Lucky2").gameObject);
                ShowItemLst(ETravelBackpackItemType.eLucky, btnObj);
                break;
            case "Grid_Package_Equipment1":
            case "Grid_Package_Equipment2":
                CurTypeGrids.Add(PackageRoot.transform.Find("Grid_Package_Equipment1").gameObject);
                CurTypeGrids.Add(PackageRoot.transform.Find("Grid_Package_Equipment2").gameObject);
                CurTypeGrids.Add(PrepareRoot.transform.Find("Grid_Prepare_Equipment1").gameObject);
                CurTypeGrids.Add(PrepareRoot.transform.Find("Grid_Prepare_Equipment2").gameObject);
                CurTypeGrids.Add(PrepareRoot.transform.Find("Grid_Prepare_Equipment3").gameObject);
                CurTypeGrids.Add(PrepareRoot.transform.Find("Grid_Prepare_Equipment4").gameObject);
                ShowItemLst(ETravelBackpackItemType.eEquipment, btnObj);
                break;
        }
    }

    public void OnClick_Package_Package()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【背包-打包】");

        PackageBtn.SetActive(false);
        RearrangeBtn.SetActive(true);
    }

    public void OnClick_Package_Rearrange()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【背包-重新整理】");

        PackageBtn.SetActive(true);
        RearrangeBtn.SetActive(false);
    }

    public void OnClick_Package_SwitchToPrepare()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【背包-切换至准备】");

        TweenPosition.Begin(PackageRoot, 0.1f, new Vector3(-1100, 0, 0));
        TweenPosition.Begin(PrepareRoot, 0.1f, Vector3.zero);
    }

    public void OnClick_Prepare_Grid(GameObject btnObj)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log(StringBuilderTool.ToInfoString("点击【准备-格子】", btnObj.name));

        switch (btnObj.name)
        {
            case "Grid_Prepare_Food1":
            case "Grid_Prepare_Food2":
                CurTypeGrids.Add(PackageRoot.transform.Find("Grid_Package_Food").gameObject);
                CurTypeGrids.Add(btnObj.transform.parent.Find("Grid_Prepare_Food1").gameObject);
                CurTypeGrids.Add(btnObj.transform.parent.Find("Grid_Prepare_Food2").gameObject);
                ShowItemLst(ETravelBackpackItemType.eFood, btnObj);
                break;
            case "Grid_Prepare_Lucky1":
            case "Grid_Prepare_Lucky2":
                CurTypeGrids.Add(PackageRoot.transform.Find("Grid_Package_Lucky").gameObject);
                CurTypeGrids.Add(btnObj.transform.parent.Find("Grid_Prepare_Lucky1").gameObject);
                CurTypeGrids.Add(btnObj.transform.parent.Find("Grid_Prepare_Lucky2").gameObject);
                ShowItemLst(ETravelBackpackItemType.eLucky, btnObj);
                break;
            case "Grid_Prepare_Equipment1":
            case "Grid_Prepare_Equipment2":
            case "Grid_Prepare_Equipment3":
            case "Grid_Prepare_Equipment4":
                CurTypeGrids.Add(PackageRoot.transform.Find("Grid_Package_Equipment1").gameObject);
                CurTypeGrids.Add(PackageRoot.transform.Find("Grid_Package_Equipment2").gameObject);
                CurTypeGrids.Add(btnObj.transform.parent.Find("Grid_Prepare_Equipment1").gameObject);
                CurTypeGrids.Add(btnObj.transform.parent.Find("Grid_Prepare_Equipment2").gameObject);
                CurTypeGrids.Add(btnObj.transform.parent.Find("Grid_Prepare_Equipment3").gameObject);
                CurTypeGrids.Add(btnObj.transform.parent.Find("Grid_Prepare_Equipment4").gameObject);
                ShowItemLst(ETravelBackpackItemType.eEquipment, btnObj);
                break;
        }
    }

    public void OnClick_Prepare_Clear()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【准备-清空】");

        TweenAlpha.Begin(AffirmClearRoot, 0.1f, 1);
    }

    public void OnClick_Prepare_Clear_Yes()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【准备-确认清空】");

        ClearPreparePanel.SetActive(true);
        ClearPrepareProgLab.text = StringBuilderTool.ToString("正在清空...");
        TweenAlpha.Begin(AffirmClearRoot, 0.1f, 0);

        List<string> PrepareGridLst = new List<string>()
        {
            "Grid_Prepare_Food1",
            "Grid_Prepare_Food2",
            "Grid_Prepare_Lucky1",
            "Grid_Prepare_Lucky2",
            "Grid_Prepare_Equipment1",
            "Grid_Prepare_Equipment2",
            "Grid_Prepare_Equipment3",
            "Grid_Prepare_Equipment4"
        };
        StartCoroutine("ForeachPrepareGrid", PrepareGridLst);
    }
    private bool IsClearPrepareGrid = false;
    private GameObject WaitClearPrepareGridObj = null;
    IEnumerator ForeachPrepareGrid(List<string> NodeLst)
    {
        IsClearPrepareGrid = false;
        for(int k = 0; k < NodeLst.Count; k++)
        {
            GameObject WaitClearGridObj = PrepareRoot.transform.Find(NodeLst[k]).gameObject;
            ulong ID = 0;
            for (int i = 0; i < WaitClearGridObj.transform.childCount; i++)
            {
                Transform child = WaitClearGridObj.transform.GetChild(i);
                if (child.name.Contains("GridInfo_"))
                {
                    WaitClearPrepareGridObj = child.gameObject;
                    ID = ulong.Parse(MyTools.GetLastString(child.name, '_'));
                }
            }
            if (ID != 0)
            {
                IsClearPrepareGrid = true;
                GameApp.SendMsg.PutItem(ID, GetPosTypeFromGridName(WaitClearGridObj.name), 0);

                while (IsClearPrepareGrid)
                    yield return new WaitForEndOfFrame();
            }
        }
        ClearPreparePanel.SetActive(false);
    }

    public void OnClick_Prepare_Clear_No()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【准备-取消清空】");

        TweenAlpha.Begin(AffirmClearRoot, 0.1f, 0);
    }

    public void OnClick_Prepare_SwitchToPackage()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【准备-切换至背包】");

        TweenPosition.Begin(PackageRoot, 0.1f, Vector3.zero);
        TweenPosition.Begin(PrepareRoot, 0.1f, new Vector3(1100, 0, 0));
    }

    public void OnTypeToggleChange(GameObject typeObj)
    {
        Debug.Log(StringBuilderTool.ToInfoString("点击【准备-切换类型】", typeObj.name));
        int CurTypeIdx = int.Parse(MyTools.GetLastString(typeObj.name, '_'));
        switch (CurTypeIdx)
        {
            case 0:
                StartCoroutine("RefreshItemList", ETravelBackpackItemType.eFood);
                break;
            case 1:
                StartCoroutine("RefreshItemList", ETravelBackpackItemType.eLucky);
                break;
            case 2:
                StartCoroutine("RefreshItemList", ETravelBackpackItemType.eEquipment);
                break;
        }
    }

    [HideInInspector]
    public GameObject CurWaitSetGridObj = null;
    private List<GameObject> CurTypeGrids = new List<GameObject>();
    public void ShowItemLst(ETravelBackpackItemType TBIType, GameObject Obj)
    {
        CurWaitSetGridObj = Obj;

        TweenAlpha.Begin(ItemLstRoot, 0.1f, 1);

        if ((int)TBIType >= TypeTog.Length)
        {
            CurTypeGrids.Clear();

            TypeTog[0].value = true;
            for(int i = 0; i < TypeTog.Length; i++)
            {
                TypeTog[i].gameObject.GetComponent<BoxCollider>().enabled = true;
                UISprite BackgroundSpr = TypeTog[i].transform.Find("Background").GetComponent<UISprite>();
                BackgroundSpr.spriteName = "btn_anjian_1";
                UISprite CheckmarkSpr = TypeTog[i].transform.Find("Checkmark").GetComponent<UISprite>();
                CheckmarkSpr.spriteName = "btn_anjian_2";
                CheckmarkSpr.width = 176;
                CheckmarkSpr.height = 67;
            }
            StartCoroutine("RefreshItemList", ETravelBackpackItemType.eFood);
        }
        else
        {
            TypeTog[(int)TBIType].value = true;
            for (int i = 0; i < TypeTog.Length; i++)
            {
                TypeTog[i].gameObject.GetComponent<BoxCollider>().enabled = false;
                UISprite BackgroundSpr = TypeTog[i].transform.Find("Background").GetComponent<UISprite>();
                BackgroundSpr.spriteName = "btn_anjian_0";
                UISprite CheckmarkSpr = TypeTog[i].transform.Find("Checkmark").GetComponent<UISprite>();
                CheckmarkSpr.spriteName = "sdgsg";
                CheckmarkSpr.width = 194;
                CheckmarkSpr.height = 74;
            }
            StartCoroutine("RefreshItemList", TBIType);
        }
    }
    IEnumerator RefreshItemList(ETravelBackpackItemType TBIType)
    {
        List<ulong> IDLst = new List<ulong>();
        for(int i = 0; i < CurTypeGrids.Count; i++)
        {
            for (int j = 0; j < CurTypeGrids[i].transform.childCount; j++)
            {
                Transform child = CurTypeGrids[i].transform.GetChild(j);
                if (child.name.Contains("GridInfo_"))
                {
                    IDLst.Add(ulong.Parse(MyTools.GetLastString(child.name, '_')));
                }
            }
        }

        MyTools.DestroyImmediateChildNodes(ItemGrid.transform);
        UIScrollView sv = ItemGrid.transform.parent.GetComponent<UIScrollView>();

        if(GameApp.Instance.PlayerData != null)
        {
            PlayerBag pb = GameApp.Instance.PlayerData.m_player_bag;
            for (int i = 0, p = 0; i < pb.m_items.Count; i++)
            {
                ItemConfig ItemCfg = null;
                CsvConfigTables.Instance.ItemCsvDic.TryGetValue((int)pb.m_items[i].m_item_id, out ItemCfg);
                if (ItemCfg != null)
                {
                    if (ItemCfg.Type == 10 + (int)TBIType)
                    {
                        GameObject newUnit = NGUITools.AddChild(ItemGrid.gameObject, ItemUnitPrefab);
                        newUnit.SetActive(true);
                        newUnit.name = "ItemUnit_" + p;
                        newUnit.transform.localPosition = new Vector3(0, -150 * p / 3, 0);

                        UI_Travel_Backpack_ItemUnit iu = newUnit.GetComponent<UI_Travel_Backpack_ItemUnit>();
                        iu.SetItemData(pb.m_items[i], ItemCfg, IDLst);

                        ItemGrid.repositionNow = true;
                        sv.ResetPosition();

                        yield return new WaitForEndOfFrame();
                        p++;
                    }
                }
            }
        }
    }

    public void CreateGridInfo(GameObject GridObj, GridInfo gi)
    {
        GameObject newGridInfo = NGUITools.AddChild(GridObj, GridInfoPrefab);
        newGridInfo.SetActive(true);
        newGridInfo.name = StringBuilderTool.ToString("GridInfo_", gi.ID);
        newGridInfo.transform.localPosition = Vector3.zero;
        UISprite spr = newGridInfo.GetComponent<UISprite>();
        spr.spriteName = gi.Icon;
        spr.MakePixelPerfect();
        spr.width = 78;
        spr.height = 78;
    }
    public void SetGrid(GridInfo gi)
    {
        TweenAlpha.Begin(ItemLstRoot, 0.1f, 0);

        if (gi != null)
        {
            CurWaitSetGridInfo = gi;

            GameApp.SendMsg.PutItem(gi.ID, 0, GetPosTypeFromGridName(CurWaitSetGridObj.name));            
        }
        else
        {
            ulong ID = 0;
            for (int i = 0; i < CurWaitSetGridObj.transform.childCount; i++)
            {
                Transform child = CurWaitSetGridObj.transform.GetChild(i);
                if (child.name.Contains("GridInfo_"))
                {
                    ID = ulong.Parse(MyTools.GetLastString(child.name, '_'));
                }
            }
            if (ID == 0)
            {
                GameApp.Instance.CommonHintDlg.OpenHintBox("该物品被其他格子占用，请先移除后再放置！");
                return;
            }
            GameApp.SendMsg.PutItem(ID, GetPosTypeFromGridName(CurWaitSetGridObj.name), 0);
        }
    }

    private GridInfo CurWaitSetGridInfo = null;
    public void SetGridRes()
    {
        if (IsClearPrepareGrid)
        {
            DestroyImmediate(WaitClearPrepareGridObj);
            IsClearPrepareGrid = false;
            return;
        }

        if (CurWaitSetGridObj != null)
        {
            List<Transform> childLst = new List<Transform>();
            for (int i = 0; i < CurWaitSetGridObj.transform.childCount; i++)
            {
                childLst.Add(CurWaitSetGridObj.transform.GetChild(i));
            }
            for (int i = 0; i < childLst.Count; i++)
            {
                if (childLst[i].name.Contains("GridInfo_"))
                {
                    DestroyImmediate(childLst[i].gameObject);
                }
            }
        }

        if (CurWaitSetGridObj != null && CurWaitSetGridInfo != null)
            CreateGridInfo(CurWaitSetGridObj, CurWaitSetGridInfo);

        CurWaitSetGridObj = null;
        CurWaitSetGridInfo = null;
    }
}
