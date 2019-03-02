using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterInfo
{
    public string Des;
    public Dictionary<int, int> ItemLst = new Dictionary<int, int>();

    public LetterInfo(string _Des, Dictionary<int, int> _ItemLst)
    {
        Des = _Des;
        ItemLst = _ItemLst;
    }
}

public class UI_Travel_LetterBox_LetterUnit : MonoBehaviour
{
    public UISprite Icon;
    public UILabel Des;
    public List<UI_AwardItem> AwardItemLst;

    void Start()
    {

    }

    /*void Update()
    {

    }*/

    /// <summary> 设置信件数据 </summary>
    public void SetLetterData(LetterInfo li)
    {
        gameObject.SetActive(true);

        Des.text = li.Des;
        
        for (int k = 0; k < AwardItemLst.Count; k++)
        {
            AwardItemLst[k].gameObject.SetActive(false);
        }

        if(li.ItemLst != null)
        {
            int i = 0;
            foreach (KeyValuePair<int, int> pair in li.ItemLst)
            {
                AwardItemLst[i].SetItemData(pair.Key, pair.Value);
                AwardItemLst[i].gameObject.SetActive(true);
                i++;
            }
        }
    }

    /// <summary> 领取按钮 </summary>
    public void OnClick_Get()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【领取】");

    }
}
