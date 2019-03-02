using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common;

public class GridInfo
{
    public ulong ID;
    public string Icon;

    public GridInfo(ulong _ID, string _Icon)
    {
        ID = _ID;
        Icon = _Icon;
    }
}

public class UI_Travel_Backpack_ItemUnit : MonoBehaviour
{
    public UISprite Icon;
    public UILabel Name;
    public UILabel Num;
    public UILabel Des;

    public GameObject SelSign;

    private PlayerBagItem SerItemInfo = null;
    private ItemConfig ItemCfg = null;

    void Start()
    {
        UI_Travel_Backpack tb = GameApp.Instance.TravelUI.TravelBackpack;

        UIButton btn = GetComponent<UIButton>();
        if (btn != null)
        {
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(() =>
            {
                /*for (int i = 0; i < transform.parent.childCount; i++)
                {
                    Transform child = transform.parent.GetChild(i);
                    UI_Travel_Backpack_ItemUnit tbiu = child.GetComponent<UI_Travel_Backpack_ItemUnit>();
                    tbiu.SetSelState(false);
                }*/
                if (!tb.PackageRoot.activeSelf && !tb.PrepareRoot.activeSelf)
                    return;

                if (SelSign.activeSelf)
                {
                    SetSelState(false);
                    tb.SetGrid(null);
                }
                else
                {
                    SetSelState(true);
                    tb.SetGrid(new GridInfo(SerItemInfo.m_id, ItemCfg.Icon));
                }
            }));
        }
    }

    /*void Update()
    {

    }*/

    /// <summary> 设置物品数据 </summary>
    public void SetItemData(PlayerBagItem pbi, ItemConfig itemCfg, List<ulong> IDLst)
    {
        gameObject.SetActive(true);

        SerItemInfo = pbi;
        ItemCfg = itemCfg;

        Icon.spriteName = ItemCfg.Icon;
        Icon.MakePixelPerfect();
        Icon.transform.localScale = Vector3.one * 1.2f;

        Name.text = ItemCfg.Name;
        Num.text = pbi.m_item_count.ToString();

        if (ItemCfg.Describe.Length > 38)
            Des.text = StringBuilderTool.ToInfoString(ItemCfg.Describe.Substring(0, 36), "...");
        else
            Des.text = ItemCfg.Describe;

        for (int i = 0; i < IDLst.Count; i++)
        {
            if (SerItemInfo.m_id == IDLst[i])
            {
                SetSelState(true);
                break;
            }
        }
    }

    public void SetSelState(bool isSel)
    {
        SelSign.SetActive(isSel);
    }
}
