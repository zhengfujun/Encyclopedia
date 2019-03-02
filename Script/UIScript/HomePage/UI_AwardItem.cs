using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AwardItem : MonoBehaviour
{
    public UISprite Icon;
    public UILabel Name;
    public GameObject getSign;

    private ItemConfig ItemCfg = null;
    private Transform ItemDetailsRoot = null;

    //void Start()
    //{

    //}

    //void Update()
    //{

    //}

    /// <summary> 设置道具数据 </summary>
    public void SetItemData(int itemID, int itemCount)
    {
        gameObject.SetActive(true);

        CsvConfigTables.Instance.ItemCsvDic.TryGetValue(itemID, out ItemCfg);
        if (ItemCfg != null)
        {
            if (ItemCfg.Type == 2)
            {
                Icon.enabled = false;
                UITexture cardIcon = Icon.gameObject.AddComponent<UITexture>();
                cardIcon.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("MagicCard/", ItemCfg.GetIcon())) as Texture;
                cardIcon.width = 40;
                cardIcon.height = 60;
                cardIcon.depth = 14;
            }
            else
            {
                Icon.spriteName = ItemCfg.Icon;
                Icon.MakePixelPerfect();
            }

            Name.text = StringBuilderTool.ToString(ItemCfg.Name, "x", itemCount);
        }
    }

    /// <summary> 显示道具详情 </summary>
    public void ShowItemDetails()
    {
        //Debug.Log("显示道具详情");

        if(ItemDetailsRoot == null)
        {
            ItemDetailsRoot = transform.parent.parent.Find("ItemDetails");
            if (ItemDetailsRoot == null)
                return;
        }
        ItemDetailsRoot.gameObject.SetActive(true);
        ItemDetailsRoot.localPosition = transform.localPosition + new Vector3(8, -10, 0);

        UISprite IDIcon = ItemDetailsRoot.Find("Icon").GetComponent<UISprite>();
        IDIcon.spriteName = Icon.spriteName;

        UILabel IDName = ItemDetailsRoot.Find("Name").GetComponent<UILabel>();
        IDName.text = ItemCfg.Name;

        UILabel IDExplain = ItemDetailsRoot.Find("Explain").GetComponent<UILabel>();
        IDExplain.text = ItemCfg.Describe;
    }
    /// <summary> 隐藏道具详情 </summary>
    public void HideItemDetails()
    {
        //Debug.Log("隐藏道具详情");

        if (ItemDetailsRoot == null)
        {
            ItemDetailsRoot = transform.parent.parent.Find("ItemDetails");
            if (ItemDetailsRoot == null)
                return;
        }
        ItemDetailsRoot.gameObject.SetActive(false);
    }

    public void ShowGetSign(bool isShow)
    {
        if (getSign != null)
            getSign.SetActive(isShow);
    }
}
