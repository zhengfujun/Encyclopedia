using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Travel_Seafloor : MonoBehaviour
{
    [HideInInspector]
    public bool isShowing = false;

    public GameObject FishLstRoot;
    public UIScrollView FishLst;
    public GameObject CardPrefab;
    public UITable LstTable;

    public GameObject OpenFishLstBtn;
    public GameObject CloseFishLstBtn;

    public UIToggle SwitchToggle;

    //public GameObject ConfirmationRoot;
    //public Transform CardRoot4CR;

    public List<Transform> AlternativePathsPos = new List<Transform>();

    public Transform FishModelRoot;
    public Dictionary<int, GameObject> FishModelObjLst = new Dictionary<int, GameObject>();
    public string DisplayedFishRecord
    {
        get
        {
            if (PlayerPrefs.HasKey(key_DFR))
            {
                return PlayerPrefs.GetString(key_DFR);
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            PlayerPrefs.SetString(key_DFR, value);
        }
    }
    private string key_DFR
    {
        get
        {
            return StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_DisplayedFishRecord");
        }
    }

    void Start()
    {
        StartCoroutine("InitModel");
    }

    void Update()
    {

    }

    public void Show()
    {
        isShowing = true;

        TweenAlpha.Begin(gameObject, 0.1f, 1);

        StartCoroutine("RefreshFishList");

        RenderSettings.ambientLight = new Color(0.088f, 0.082f, 0.06f);
        RenderSettings.fogColor = new Color(6f / 255f, 41f / 255f, 83f / 255f, 1);
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.04f;
    }

    public void Hide()
    {
        isShowing = false;

        TweenAlpha.Begin(gameObject, 0.1f, 0);
        
        RenderSettings.ambientLight = Color.white;
        RenderSettings.fogColor = new Color(161f / 255f, 206f / 255f, 233f / 255f, 1);
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 100;
        RenderSettings.fogEndDistance = 300;
    }

    IEnumerator RefreshFishList()
    {
        MyTools.DestroyChildNodes(LstTable.transform);
        foreach (KeyValuePair<int, MagicCardConfig> pair in CsvConfigTables.Instance.MagicCardCsvDic)
        {
            if (pair.Value.ThemeID != 103)
                continue;

            if(SerPlayerData.GetItemCount(pair.Key) > 0)
            {
                GameObject newUnit = NGUITools.AddChild(LstTable.gameObject, CardPrefab);
                newUnit.SetActive(true);
                newUnit.name = "Card_" + pair.Key;

                UI_MagicCard mc = newUnit.GetComponent<UI_MagicCard>();
                mc.UnconditionalShow(pair.Key);

                LstTable.repositionNow = true;
                yield return new WaitForEndOfFrame();
                FishLst.ResetPosition();
            }
        }
    }

    IEnumerator InitModel()
    {
        if (DisplayedFishRecord.Length > 0)
        {
            string[] IDs = DisplayedFishRecord.Split('_');
            for (int i = 0; i < IDs.Length; i++)
            {
                PutFishModelInScene(int.Parse(IDs[i]));
                yield return new WaitForEndOfFrame();
            }
        }
    }
    
    // <summary> 点击显示鱼列表 </summary>
    public void OnClick_OpenFishLst()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【显示鱼列表】");

        OpenFishLstBtn.SetActive(false);
        CloseFishLstBtn.SetActive(true);

        TweenPosition.Begin(FishLstRoot, 0.2f, Vector3.zero);
    }

    /// <summary> 点击关闭鱼列表 </summary>
    public void OnClick_CloseFishLst()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭鱼列表】");

        OpenFishLstBtn.SetActive(true);
        CloseFishLstBtn.SetActive(false);

        TweenPosition.Begin(FishLstRoot, 0.2f, new Vector3(1170, 0, 0));
    }
    public void OnSwitchToggleValueChange(UIToggle toggle)
    {
        for (int i = 0; i < LstTable.transform.childCount; i++)
        {
            Transform child = LstTable.transform.GetChild(i);
            if (child != null)
            {
                UI_MagicCard mc = child.GetComponent<UI_MagicCard>();
                if (mc != null)
                {
                    if (toggle.value)
                    {

                        //Debug.Log("显示涂色贴图");
                        mc.SetCustomColoringTexture(mc.MCCfg.CardID - Const.FishCardFirshID);
                    }
                    else
                    {
                        //Debug.Log("显示原始贴图");
                        mc.SetCustomColoringTexture(-1);
                    }
                }
            }
        }

        foreach (KeyValuePair<int, GameObject> pair in FishModelObjLst)
        {
            foreach (Transform child in pair.Value.transform)
            {
                if (toggle.value)
                {
                    child.gameObject.SetActive(child.name.Contains("_Coloring"));
                }
                else
                {
                    child.gameObject.SetActive(child.name.Contains("_Original"));
                }
            }
        }
    }

    //private GameObject CurCardObj = null;
    public void PutFishModelInScene(MagicCardConfig mcc, bool RecordToPlayerPrefs = true)
    {
        /*CurCardObj = CardObj;
        MyTools.DestroyChildNodes(CardRoot4CR);
        GameObject NewCurCardObj = GameObject.Instantiate(CurCardObj);
        NewCurCardObj.transform.parent = CardRoot4CR;
        NewCurCardObj.transform.localPosition = Vector3.zero;
        NewCurCardObj.transform.localScale = Vector3.one;

        TweenAlpha.Begin(ConfirmationRoot, 0.2f, 1).from = 0;*/

        if (mcc != null)
        {
            if (FishModelObjLst.ContainsKey(mcc.CardID))
            {
                GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToString(mcc.Name, "已在水塘中！"));
                return;
            }

            int FishIndex = mcc.CardID - Const.FishCardFirshID;

            GameObject FishModeParent = new GameObject();
            FishModeParent.name = StringBuilderTool.ToString("FishModeParent_", mcc.CardID);
            FishModeParent.transform.parent = FishModelRoot;
            FishModeParent.transform.localPosition = Vector3.zero;
            FishModeParent.transform.localEulerAngles = new Vector3(0, 90, 0);
            FishModeParent.transform.localScale = Vector3.one * 2.5f;
            BoxCollider bc = FishModeParent.AddComponent<BoxCollider>();
            bc.size = Vector3.one * 0.5f;

            GameObject ModelObj_Original = Resources.Load<GameObject>("Prefabs/Actor/" + mcc.ModelName);
            if (ModelObj_Original != null)
            {
                GameObject FishMode_Original = GameObject.Instantiate(ModelObj_Original);
                FishMode_Original.name = StringBuilderTool.ToString(mcc.ModelName, "_Original");
                FishMode_Original.transform.parent = FishModeParent.transform;
                FishMode_Original.transform.localPosition = Vector3.zero;
                FishMode_Original.transform.localEulerAngles = new Vector3(0, 90, 0);
                //FishMode_Original.transform.localScale = FishMode_Original.transform.localScale * 1.5f;
                FishMode_Original.SetActive(!SwitchToggle.value);
            }

            GameObject ModelObj_Coloring = Resources.Load<GameObject>("Prefabs/Actor/" + mcc.ColoringModelName);
            if (ModelObj_Coloring != null)
            {
                GameObject FishMode_Coloring = GameObject.Instantiate(ModelObj_Coloring);
                FishMode_Coloring.name = StringBuilderTool.ToString(mcc.ModelName, "_Coloring");
                FishMode_Coloring.transform.parent = FishModeParent.transform;
                FishMode_Coloring.transform.localPosition = Vector3.zero;
                FishMode_Coloring.transform.localEulerAngles = new Vector3(0, 90, 0);
                //FishMode_Coloring.transform.localScale = FishMode_Coloring.transform.localScale * 1.5f;
                FishMode_Coloring.SetActive(SwitchToggle.value);

                StartCoroutine(ReadColoringTexture(FishIndex, FishMode_Coloring));
            }

            SimplePath sp = FishModeParent.AddComponent<SimplePath>();
            int[] RandomLst = MyTools.GetRandomNumArray4Barring(5, AlternativePathsPos.Count, -1);
            sp.paths = new Transform[6];
            for (int i = 0; i < RandomLst.Length; i++)
            {
                sp.paths[i] = AlternativePathsPos[RandomLst[i]];
            }
            sp.paths[5] = AlternativePathsPos[RandomLst[0]];
            sp.speed = 1.5f;
            sp.IsOrientToPath = true;
            sp.Run();

            FishModelObjLst.Add(mcc.CardID, FishModeParent);

            if (RecordToPlayerPrefs)
            {
                DisplayedFishRecord += StringBuilderTool.ToString(DisplayedFishRecord.Length > 0 ? "_" : "", FishIndex);
            }
        }
    }
    IEnumerator ReadColoringTexture(int fishIndex, GameObject Model)
    {
        string url = StringBuilderTool.ToString("file://", Application.persistentDataPath, "/NewFishTexture_", fishIndex, ".jpg");

        WWW www = new WWW(url);
        yield return www;
        if (www.isDone && www.error == null)
        {
            SkinnedMeshRenderer ModelSMR = Model.transform.Find("body").GetComponent<SkinnedMeshRenderer>();
            if (ModelSMR != null)
            {
                Material ModelMat = ModelSMR.materials[0];
                if (ModelMat != null)
                {
                    ModelMat.mainTexture = www.texture;
                }
            }
        }
    }

    private void PutFishModelInScene(int fishIndex)
    {
        MagicCardConfig CardCfg = null;
        CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(Const.FishCardFirshID + fishIndex, out CardCfg);
        PutFishModelInScene(CardCfg,false);
    }

    /*public void OnClick_CloseConfirmation()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭确认界面】");

        TweenAlpha.Begin(ConfirmationRoot, 0.2f, 0).from = 1;
    }*/
    /*public void OnClick_Delete()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【删除】");

        UI_MagicCard mc = CurCardObj.GetComponent<UI_MagicCard>();

        foreach (GameObject obj in FishObjLst)
        {
            int id = int.Parse(MyTools.GetLastString(obj.name,'_'));
            if (id == mc.MCCfg.CardID)
            {
                Destroy(obj);
                FishObjLst.Remove(obj);
                break;
            }
        }

        for(int i = 0; i < LstTable.transform.childCount; i++)
        {
            Transform child = LstTable.transform.GetChild(i);
            int id = int.Parse(MyTools.GetLastString(child.name, '_'));
            if (id == mc.MCCfg.CardID)
            {
                Destroy(child.gameObject);

                LstTable.repositionNow = true;
                FishLst.ResetPosition();
                break;
            }
        }

        TweenAlpha.Begin(ConfirmationRoot, 0.2f, 0).from = 1;
    }*/
}