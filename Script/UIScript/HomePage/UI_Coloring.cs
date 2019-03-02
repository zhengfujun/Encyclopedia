using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using IndieStudio.DrawingAndColoring.Logic;

public enum EColorinToolType
{
    ePencil,
    eWaterColor,
    eStamp,
    ePaintingCan,
    eEraser,
}

public class UI_Coloring : MonoBehaviour
{
    public GameObject[] HideObjsWhenColoringShow;

    public GameObject ColorListRoot;
    public UIGrid ColorListGrid;
    public GameObject ColorUnitPrefab;

    public GameObject StampListRoot;
    public UIGrid StampListGrid;
    public GameObject StampUnitPrefab;

    private EColorinToolType CurToolType = EColorinToolType.ePencil;
    public UI_ToolUnit[] ToolBtnList;

    public ThicknessSize MediumTS;
    public ThicknessSize LargeTS;
    public ThicknessSize XLargeTS;

    public GameObject ModelRoot;
    private GameObject Model;

    public GameObject SeaSceneObj;
    public GameObject AboveSeaSceneUI;

    public GameObject RotateTrigger;

    public GameManager ColoringMgr
    {
        get
        {
            if (_ColoringMgr == null)
            {
                _ColoringMgr = GameObject.FindObjectOfType<GameManager>();
            }
            return _ColoringMgr;
        }
    }
    private GameManager _ColoringMgr = null;

    public Tool PencilTool
    {
        get
        {
            if(_PencilTool == null)
            {
                _PencilTool = GetTool("PencilTool");
            } 
            return _PencilTool;
        }
    }
    private Tool _PencilTool = null;

    public Tool WaterColorTool
    {
        get
        {
            if (_WaterColorTool == null)
            {
                _WaterColorTool = GetTool("WaterColorTool");
            }
            return _WaterColorTool;
        }
    }
    private Tool _WaterColorTool = null;

    public Tool PaintingCanTool
    {
        get
        {
            if (_PaintingCanTool == null)
            {
                _PaintingCanTool = GetTool("PaintingCanTool");
            }
            return _PaintingCanTool;
        }
    }
    private Tool _PaintingCanTool = null;

    public Tool StampTool
    {
        get
        {
            if (_StampTool == null)
            {
                _StampTool = GetTool("StampTool");
            }
            return _StampTool;
        }
    }
    private Tool _StampTool = null;

    public Tool EraserTool
    {
        get
        {
            if (_EraserTool == null)
            {
                _EraserTool = GetTool("EraserTool");
            }
            return _EraserTool;
        }
    }
    private Tool _EraserTool = null;

    private Tool GetTool(string ToolName)
    {
        for (int i = 0; i < ColoringMgr.tools.Length; i++)
        {
            if (ColoringMgr.tools[i] != null && ColoringMgr.tools[i].name == ToolName)
            {
                return ColoringMgr.tools[i];
            }
        }
        return null;
    }

    void Start()
    {
        UIEventListener el = UIEventListener.Get(RotateTrigger);
        el.onDrag = OnDrag;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {

        }
        if (Input.GetKeyUp(KeyCode.W))
        {

        }
    }

    public void Show()
    {

        GameApp.Instance.FadeHelperInstance.FadeIn(0.05f, Color.black, () =>
        {
            for (int i = 0; i < HideObjsWhenColoringShow.Length; i++)
            {
                HideObjsWhenColoringShow[i].SetActive(false);
            }
            GameApp.Instance.UICurrency.Show(false);

            gameObject.SetActive(true);
            
            StartCoroutine("Init");

            GameApp.Instance.FadeHelperInstance.FadeOut(0.05f, () =>
                {

                });
        });

        
    }

    private bool IsFirstInit = true;
    IEnumerator Init()
    {
        while (ColoringMgr == null)
        {
            yield return new WaitForEndOfFrame();
        }
        while (PencilTool == null || 
                WaterColorTool == null ||
                PaintingCanTool == null ||
                StampTool == null || 
                EraserTool == null)
        {
            yield return new WaitForEndOfFrame();
        }

        if (IsFirstInit)
        {
            IsFirstInit = false;
        }
        else
        {
            ColoringMgr.InstantiateDrawingContents();
            yield return new WaitForEndOfFrame();
            ShapesCanvas.instance.InstantiateShapes();
            yield return new WaitForEndOfFrame();
            ColoringMgr.LoadCurrentShape();
        }

        ToolBtnList[(int)EColorinToolType.ePencil].OnClick_Sel();

        MyTools.DestroyImmediateChildNodes(ColorListGrid.transform);
        for (int i = 0; i < PencilTool.contents.Count; i++)
        {
            GameObject newUnit = NGUITools.AddChild(ColorListGrid.gameObject, ColorUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "Color_" + i;
            newUnit.transform.localPosition = new Vector3(0, -74 * i, 0);

            UI_ColorUnit fu = newUnit.GetComponent<UI_ColorUnit>();
            fu.Set(PencilTool.contents[i].GetComponent<ToolContent>());

            ColorListGrid.repositionNow = true;

            yield return new WaitForEndOfFrame();
        }

        MyTools.DestroyImmediateChildNodes(StampListGrid.transform);
        for (int i = 0; i < StampTool.contents.Count; i++)
        {
            GameObject newUnit = NGUITools.AddChild(StampListGrid.gameObject, StampUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "Stamp_" + i;
            newUnit.transform.localPosition = new Vector3(0, -74 * i, 0);

            UI_StampUnit su = newUnit.GetComponent<UI_StampUnit>();
            su.Set(StampTool.contents[i].GetComponent<Image>().mainTexture);

            StampListGrid.repositionNow = true;

            yield return new WaitForEndOfFrame();
        }

    }

    private void Hide()
    {
        GameApp.Instance.FadeHelperInstance.FadeIn(0.05f, Color.black, () =>
        {
            for (int i = 0; i < HideObjsWhenColoringShow.Length; i++)
            {
                HideObjsWhenColoringShow[i].SetActive(true);
            }
            GameApp.Instance.UICurrency.Show(true);

            IsFirstSelPencil = true;
            IsFirstSelWaterColor = true;
            IsFirstSelPaintingCan = true;

            Destroy(Model);

            BackToColoringFromSeaScene();

            gameObject.SetActive(false);

            ColoringMgr.CleanCurrentShapeScreen();

            GameApp.Instance.FadeHelperInstance.FadeOut(0.05f, () =>
            {

            });
        });
    }

    public void ChangeToolColor(int colorIdx)
    {
        ToolContent tc = null;
        switch (CurToolType)
        {
            case EColorinToolType.ePencil:
                tc = PencilTool.contents[colorIdx].GetComponent<ToolContent>();
                break;
            case EColorinToolType.eWaterColor:
                tc = WaterColorTool.contents[colorIdx].GetComponent<ToolContent>();
                break;
            case EColorinToolType.ePaintingCan:
                tc = PaintingCanTool.contents[colorIdx].GetComponent<ToolContent>();
                break;
        }
        GameManager.uiEvents.ToolContentClickEvent(tc);

        ToolBtnList[(int)CurToolType].ChangeColor(tc.gradientColor.Evaluate(0));
    }

    private bool IsFirstSelPencil = true;
    private bool IsFirstSelWaterColor = true;
    private bool IsFirstSelPaintingCan = true;
    public void SelTool(EColorinToolType toolType)
    {
        CurToolType = toolType;
        switch (CurToolType)
        {
            case EColorinToolType.ePencil:
                GameManager.uiEvents.ToolClickEvent(PencilTool);//铅笔PencilTool
                GameManager.uiEvents.ThicknessSizeEvent(MediumTS);
                ShowColorList();
                if (IsFirstSelPencil)
                {
                    ChangeToolColor(0);
                    IsFirstSelPencil = false;
                }
                break;
            case EColorinToolType.eWaterColor:
                GameManager.uiEvents.ToolClickEvent(WaterColorTool);//水彩笔WaterColorTool
                GameManager.uiEvents.ThicknessSizeEvent(LargeTS);
                ShowColorList();
                if (IsFirstSelWaterColor)
                {
                    ChangeToolColor(1);
                    IsFirstSelWaterColor = false;
                }
                break;
            case EColorinToolType.eStamp:
                GameManager.uiEvents.ToolClickEvent(StampTool);//印章StampTool
                ShowStampList();
                break;
            case EColorinToolType.ePaintingCan:
                GameManager.uiEvents.ToolClickEvent(PaintingCanTool);//油漆罐PaintingCanTool
                ShowColorList();
                if (IsFirstSelPaintingCan)
                {
                    ChangeToolColor(7);
                    IsFirstSelPaintingCan = false;
                }
                break;
            case EColorinToolType.eEraser:
                GameManager.uiEvents.ToolClickEvent(EraserTool);//橡皮擦EraserTool
                GameManager.uiEvents.ThicknessSizeEvent(XLargeTS);
                break;
        }
    }

    private void ShowColorList()
    {
        TweenAlpha.Begin(ColorListRoot, 0.1f, 1);
        TweenAlpha.Begin(StampListRoot, 0.1f, 0);
    }
    private void ShowStampList()
    {
        TweenAlpha.Begin(ColorListRoot, 0.1f, 0);
        TweenAlpha.Begin(StampListRoot, 0.1f, 1);
    }

    public void ChangeStamp(int stampIdx)
    {
        ToolContent tc = StampTool.contents[stampIdx].GetComponent<ToolContent>();
                
        GameManager.uiEvents.ToolContentClickEvent(tc);
    }

    public void OnClick_Save()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("UI_Coloring OnClick_Save");

        ColoringMgr.IsPause = true;
        GameManager.uiEvents.PrintClickEvent();
    }
    public void OnClick_Modify()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("UI_Coloring OnClick_Modify");

        ColoringMgr.IsPause = false;
        BackToColoringFromSeaScene();
    }
    public void OnClick_Confirm()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("UI_Coloring OnClick_Confirm");
        
        string imagePath = StringBuilderTool.ToString(Application.persistentDataPath,"/NewFishTexture_", ShapesManager.lastSelectedShape, ".jpg");
#if UNITY_EDITOR
        Debug.Log(imagePath);
#endif
        byte[] bytes = RecordCurTexture.EncodeToJPG();

        FileStream cache = new System.IO.FileStream(imagePath, System.IO.FileMode.Create);
        cache.Write(bytes, 0, bytes.Length);
        cache.Close();

        GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox("涂色贴图已保存，可前往魔法水塘查看", (ok) =>
            {
                GameApp.SendMsg.GMOrder(StringBuilderTool.ToString("AddItem ", UI_Painting.RecordCurPaintingFishCard.CardID, " 1"));

                ColoringMgr.IsPause = false;
                Hide();
            });
    }
    public void OnClick_Close()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("UI_Coloring OnClick_Close");

        Hide();
    }

    Texture2D RecordCurTexture = null;
    public void SwitchToPreview(Texture2D texture)
    {
        //Image XiaoChouYuUIImg = ShapesManager.instance.shapes[ShapesManager.lastSelectedShape].gamePrefab.GetComponent<Image>();
        MyTools.DestroyImmediateChildNodes(ModelRoot.transform);

        RecordCurTexture = texture;

        MagicCardConfig CardCfg = null;
        CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(Const.FishCardFirshID + ShapesManager.lastSelectedShape, out CardCfg);
        if(CardCfg == null)
        {
            return;
        }

        GameObject ModelObj = Resources.Load<GameObject>(StringBuilderTool.ToInfoString("Prefabs/Actor/", CardCfg.ColoringModelName));
        if (ModelObj != null)
        {
            Model = GameObject.Instantiate(ModelObj);
            Model.transform.parent = ModelRoot.transform;
            Model.transform.localPosition = Vector3.zero;
            Model.transform.localEulerAngles = Vector3.zero;
            if (CardCfg.CardID == 50203)
            {
                Model.transform.localScale = Vector3.one * 0.5f;
            }
            else
            {
                Model.transform.localScale = Vector3.one;
            }
            Model.SetActive(true);

            SkinnedMeshRenderer ModelSMR = Model.transform.Find("body").GetComponent<SkinnedMeshRenderer>();
            if(ModelSMR != null)
            {
                Material ModelMat = ModelSMR.materials[0];
                if (ModelMat != null)
                {
                    ModelMat.mainTexture = RecordCurTexture;
                }
            }
        }

        AboveSeaSceneUI.SetActive(true);

        SeaSceneObj.SetActive(true);
        RenderSettings.ambientLight = new Color(0.088f, 0.082f, 0.06f);
        RenderSettings.fog = true;
    }

    public void BackToColoringFromSeaScene()
    {
        AboveSeaSceneUI.SetActive(false);

        SeaSceneObj.SetActive(false);
        RenderSettings.ambientLight = Color.white;
        RenderSettings.fog = false;
    }

    void OnDrag(GameObject obj, Vector2 delta)
    {
        ModelRoot.transform.Rotate(0, delta.x * -0.5f, 0);
    }
}
