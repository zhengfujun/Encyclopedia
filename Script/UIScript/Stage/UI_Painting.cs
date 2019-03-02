using UnityEngine;
using System.Collections;
using IndieStudio.DrawingAndColoring.Logic;

public class UI_Painting : UI_Method
{
    private MagicCardConfig CurMCCfg = null;

    private UISprite NameBg;
    private UITexture Icon;
    private Transform SelSpr;
    private Transform LockSpr;

    private BoxCollider BtnBC;

    public static MagicCardConfig RecordCurPaintingFishCard = null;

    protected override void Start()
    {
        UIButton btn = GetComponent<UIButton>();
        if (btn != null)
        {
            ///点击自身
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(() =>
            {
                if (CurIndex == -1)
                    return;

                if (Icon.mainTexture == null)
                {
                    GameApp.Instance.CommonHintDlg.OpenHintBox("", EHintBoxStyle.eStyle_ComingSoon);
                    return;
                }

                RecordCurPaintingFishCard = CurMCCfg;

                ShapesManager.lastSelectedShape = CurIndex;

                GameApp.Instance.HomePageUI.Coloring.Show();
            }));
        }
    }

    public void SetInfo(MagicCardConfig mcc)
    {
        CurMCCfg = mcc;

        CurIndex = mcc.CardID - Const.FishCardFirshID;
        gameObject.name = StringBuilderTool.ToString("Stage_", CurIndex);

        Name = transform.Find("Name").GetComponent<UILabel>();
        NameBg = transform.Find("NameBg").GetComponent<UISprite>();
        Icon = transform.Find("Icon").GetComponent<UITexture>();
        SelSpr = transform.Find("Sel");
        LockSpr = transform.Find("Lock");
        
        BtnBC = GetComponent<BoxCollider>();

        //CurSC = sc;
        Name.text = mcc.Name;

        Icon.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("ColoringFishIcon/", mcc.ColouredIcon)) as Texture;
    }

    public override void SetState(EStageState State)
    {
        switch (State)
        {
            case EStageState.eLock:
                BtnBC.enabled = false;
                //Icon.spriteName = CurSC.Icon + "_disC";
                NameBg.spriteName = "bg_guanka_ming_disC";
                SelSpr.gameObject.SetActive(false);
                LockSpr.gameObject.SetActive(true);
                break;
            case EStageState.eUnlock:
                BtnBC.enabled = true;
                NameBg.spriteName = "bg_guanka_ming";
                SelSpr.gameObject.SetActive(true);
                LockSpr.gameObject.SetActive(false);
                break;
            case EStageState.ePass:
                BtnBC.enabled = true;
                NameBg.spriteName = "bg_guanka_ming";
                SelSpr.gameObject.SetActive(false);
                LockSpr.gameObject.SetActive(false);
                break;
        }
    }
}
