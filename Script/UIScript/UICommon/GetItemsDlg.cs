using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 获得道具提示框
/// </summary>
public class GetItemsDlg : UIAppearEffect
{
    public GameObject BoxObj;   //消息框结点

    public GameObject ItemLstGrid;
    public GameObject ItemUnitPrefab;

    public GameObject OKBtnObj;

    public UILabel AutoCloseCD;

    void Start()
    {
        GameApp.Instance.GetItemsDlg = this;
    }

    //显示提示框 
    public void OpenGetItemsBox(Dictionary<int, int> ItemLst,float delayShowOK = 0f)
    {
        base.Open(AppearType.Popup, BoxObj);

        MyTools.DestroyImmediateChildNodes(ItemLstGrid.transform);

        int i = 0;
        foreach (KeyValuePair<int, int> pair in ItemLst)
        {
            GameObject newUnit = NGUITools.AddChild(ItemLstGrid, ItemUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "Item_" + i;

            newUnit.GetComponent<UI_AwardItem>().SetItemData(pair.Key, pair.Value);
            i++;
        }
        ItemLstGrid.transform.GetComponent<UIGrid>().repositionNow = true;
                
        if(delayShowOK != 0)
        {
            StartCoroutine("DelayShowOK", delayShowOK);
        }

        StartCoroutine("_Countdown");
    }

    public void CloseGetItemsBox()
    {
        base.Close(AppearType.Diffusion, () =>
            {
                BoxObj.SetActive(false);
            });

        StopCoroutine("_Countdown");
    }

    IEnumerator DelayShowOK(float delay)
    {
        OKBtnObj.SetActive(false);
        yield return new WaitForSeconds(delay);
        OKBtnObj.SetActive(true);
    }

    IEnumerator _Countdown()
    {
        int cd = 10;
        while (cd > 0)
        {
            AutoCloseCD.text = StringBuilderTool.ToString("点击任意位置继续 [AAEE00]", cd--, "秒[-]");
            yield return new WaitForSeconds(1);
        }
        CloseGetItemsBox();
    }
}
