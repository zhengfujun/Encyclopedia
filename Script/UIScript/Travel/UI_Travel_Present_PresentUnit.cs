using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Travel_Present_PresentUnit : MonoBehaviour
{
    public UISprite Bg;
    public UISprite Icon;
    public UILabel Name;

    private ItemConfig ItemCfg = null;

    void Start()
    {
        UI_Travel_Present tp = GameApp.Instance.TravelUI.TravelPresent;

        UIButton btn = GetComponent<UIButton>();
        if (btn != null)
        {
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(() =>
            {
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    Transform child = transform.parent.GetChild(i);
                    UI_Travel_Present_PresentUnit tppu = child.GetComponent<UI_Travel_Present_PresentUnit>();
                    tppu.SetSelState(false);
                }
                SetSelState(true);

                tp.ShowPresentDetails(ItemCfg);
            }));
        }
    }

    /*void Update()
    {

    }*/

    /// <summary> 设置礼物数据 </summary>
    public void SetPresentData(int ItemID)
    {
        gameObject.SetActive(true);

        CsvConfigTables.Instance.ItemCsvDic.TryGetValue(ItemID, out ItemCfg);
        if (ItemCfg != null)
        {
            Icon.spriteName = ItemCfg.Icon;
            Icon.MakePixelPerfect();
            Icon.transform.localScale = Vector3.one * 1.5f;

            Name.text = ItemCfg.Name;
        }
    }

    public void SetSelState(bool isSel)
    {
        Bg.spriteName = (isSel ? "liwuBg_Sel" : "liwuBg");
    }
}
