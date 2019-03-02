using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common;

public class UI_Travel_Present : MonoBehaviour
{
    [HideInInspector]
    public bool isShowing = false;

    public GameObject PresentUnitPrefab;
    public UIGrid PresentGrid;

    public GameObject SelHintRoot;

    public GameObject DetailsRoot;
    public GameObject Details_Bg;
    public UILabel Details_Name;
    public UILabel Details_Des;

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

        StartCoroutine("RefreshPresentList");

        SelHintRoot.SetActive(true);
        Details_Bg.SetActive(false);
    }

    public void Hide()
    {
        isShowing = false;
        TweenAlpha.Begin(gameObject, 0.1f, 0);
        TweenAlpha.Begin(DetailsRoot, 0.1f, 0);
    }

    IEnumerator RefreshPresentList()
    {
        MyTools.DestroyImmediateChildNodes(PresentGrid.transform);
        UIScrollView sv = PresentGrid.transform.parent.GetComponent<UIScrollView>();

        PlayerBag pb = GameApp.Instance.PlayerData.m_player_bag;
        for (int i = 0, p = 0; i < pb.m_items.Count; i++)
        {
            ItemConfig ItemCfg = null;
            CsvConfigTables.Instance.ItemCsvDic.TryGetValue((int)pb.m_items[i].m_item_id, out ItemCfg);
            if (ItemCfg != null)
            {
                if (ItemCfg.Type == 13)
                {
                    GameObject newUnit = NGUITools.AddChild(PresentGrid.gameObject, PresentUnitPrefab);
                    newUnit.SetActive(true);
                    newUnit.name = "PresentUnit_" + p;
                    int x = 0;
                    if (p % 3 == 0)
                        x = 0;
                    else if (p % 3 == 1)
                        x = 160;
                    else if (p % 3 == 2)
                        x = 320;
                    newUnit.transform.localPosition = new Vector3(x, -160 * (p / 3), 0);

                    UI_Travel_Present_PresentUnit fu = newUnit.GetComponent<UI_Travel_Present_PresentUnit>();
                    fu.SetPresentData(ItemCfg.ItemID);

                    PresentGrid.repositionNow = true;
                    sv.ResetPosition();

                    yield return new WaitForEndOfFrame();
                    p++;
                }
            }
        }

        PresentGrid.repositionNow = true;
        sv.ResetPosition();
    }

    /// <summary> 显示礼物详情 </summary>
    public void ShowPresentDetails(ItemConfig ItemCfg)
    {
        TweenAlpha.Begin(DetailsRoot, 0.1f, 1);

        Details_Name.text = ItemCfg.Name;
        Details_Des.text = ItemCfg.Describe;

        SelHintRoot.SetActive(false);
        Details_Bg.SetActive(true);
    }

    public void OnClick_()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【】");


    }
}
