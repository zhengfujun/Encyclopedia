using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using common;

public class UI_Ranking : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public GameObject RankUnitPrefab;
    public UIGrid RankingGrid;
    public UI_RankUnit MyselfRank;
    private RankInfo MyselfRI = new RankInfo(0,new PlayerRankInfo(0,"",0));

    public UILabel RankTitleLab;
    public UISprite RankTitleIcon;
    public UILabel RankValueDes;

    public int CurShowRankType = 0;

    public GameObject Cover;

    /*void Awake()
    {

    }*/

    void Start()
    {

    }

    /*void OnDestroy()
    {

    }*/

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            GameApp.SendMsg.GetRank(RankType.RankType_Achievement);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            GameApp.SendMsg.GetRank(RankType.RankType_PVE);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            
        }
    }*/

    public void Show(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            /////////////////////////////////////////////////////
            MyselfRI.ServerRankInfo.name = SerPlayerData.GetName();
            MyselfRI.ServerRankInfo.score = SerPlayerData.GetCardCount();

            CurShowRankType = 4;

            RankTitleLab.text = "收集卡牌排行榜";
            RankTitleIcon.spriteName = "pic_biaoti_paiming";
            RankValueDes.text = "卡牌数";

            GameApp.SendMsg.GetRank(RankType.RankType_Card);
            /////////////////////////////////////////////////////
        }
        else
        {
            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    private List<RankInfo> TempPreliminaryRankDataLst_1 = new List<RankInfo>()
    {
        new RankInfo(1,new PlayerRankInfo(10001,"守信的风鹰",864985)),
        new RankInfo(2,new PlayerRankInfo(10002,"高尚的黑犀",764563)),
        new RankInfo(3,new PlayerRankInfo(10003,"清廉的雪獒",732345)),
        new RankInfo(4,new PlayerRankInfo(10004,"贤能的地虎",653453)),
        new RankInfo(5,new PlayerRankInfo(10005,"勤奋的刑天",643342)),
        new RankInfo(6,new PlayerRankInfo(10006,"进取的飞影",587234)),
        new RankInfo(7,new PlayerRankInfo(10007,"谦虚的金刚",573543)),
        new RankInfo(8,new PlayerRankInfo(10008,"恒心的拿瓦",554345)),
        new RankInfo(9,new PlayerRankInfo(10009,"认真的茨纳米",473455)),
        new RankInfo(10,new PlayerRankInfo(10010,"勇敢的驮拏多",355677)),
        new RankInfo(11,new PlayerRankInfo(10011,"果断的酷雷伏",295746)),
        new RankInfo(12,new PlayerRankInfo(10012,"刚毅的卡魄",181756)),
        new RankInfo(13,new PlayerRankInfo(10013,"健谈的阿罗伊",54566)),
        new RankInfo(14,new PlayerRankInfo(10014,"实际的埃戈士",45645)),
        new RankInfo(15,new PlayerRankInfo(10015,"可靠的炎龙",34566)),
    };
    private List<RankInfo> TempPreliminaryRankDataLst_2 = new List<RankInfo>()
    {
        new RankInfo(1,new PlayerRankInfo(10001,"进取的飞影",568)),
        new RankInfo(2,new PlayerRankInfo(10002,"果断的酷雷伏",532)),
        new RankInfo(3,new PlayerRankInfo(10003,"勤奋的刑天",435)),
        new RankInfo(4,new PlayerRankInfo(10004,"刚毅的卡魄",425)),
        new RankInfo(5,new PlayerRankInfo(10005,"清廉的雪獒",412)),
        new RankInfo(6,new PlayerRankInfo(10006,"守信的风鹰",389)),
        new RankInfo(7,new PlayerRankInfo(10007,"谦虚的金刚",358)),
        new RankInfo(8,new PlayerRankInfo(10008,"恒心的拿瓦",345)),
        new RankInfo(9,new PlayerRankInfo(10009,"认真的茨纳米",220)),
        new RankInfo(10,new PlayerRankInfo(10010,"勇敢的驮拏多",212)),
        new RankInfo(11,new PlayerRankInfo(10011,"高尚的黑犀",147)),
        new RankInfo(12,new PlayerRankInfo(10012,"贤能的地虎",58)),
        new RankInfo(13,new PlayerRankInfo(10013,"健谈的阿罗伊",42))
    };                                    
    private List<RankInfo> TempPreliminaryRankDataLst_3 = new List<RankInfo>()
    {
        new RankInfo(1,new PlayerRankInfo(10001,"认真的茨纳米",99)),
        new RankInfo(2,new PlayerRankInfo(10002,"勇敢的驮拏多",99)),
        new RankInfo(3,new PlayerRankInfo(10003,"果断的酷雷伏",99)),
        new RankInfo(4,new PlayerRankInfo(10004,"健谈的阿罗伊",95)),
        new RankInfo(5,new PlayerRankInfo(10005,"进取的飞影",94)),
        new RankInfo(6,new PlayerRankInfo(10006,"守信的风鹰",94)),
        new RankInfo(7,new PlayerRankInfo(10007,"刚毅的卡魄",88)),
        new RankInfo(8,new PlayerRankInfo(10008,"实际的埃戈士",85)),
        new RankInfo(9,new PlayerRankInfo(10009,"高尚的黑犀",75)),
        new RankInfo(10,new PlayerRankInfo(10010,"清廉的雪獒",72)),
        new RankInfo(11,new PlayerRankInfo(10011,"恒心的拿瓦",65)),
        new RankInfo(12,new PlayerRankInfo(10012,"贤能的地虎",64)),
        new RankInfo(13,new PlayerRankInfo(10013,"谦虚的金刚",63)),
        new RankInfo(14,new PlayerRankInfo(10014,"勤奋的刑天",54))
    };                                    
    IEnumerator RefreshRankingList(List<RankInfo> RILst)
    {
        Cover.SetActive(true);
        MyTools.DestroyImmediateChildNodes(RankingGrid.transform);
        UIScrollView sv1 = RankingGrid.transform.parent.GetComponent<UIScrollView>();
        for (int i = 0; i < RILst.Count; i++)
        {
            GameObject newUnit = NGUITools.AddChild(RankingGrid.gameObject, RankUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "RankUnit_" + i;
            newUnit.transform.localPosition = new Vector3(0, -120 * i, 0);

            //newUnit.GetComponent<UIDragScrollView>().scrollView = sv1;

            UI_RankUnit ru = newUnit.GetComponent<UI_RankUnit>();
            ru.Set(RILst[i]);

            RankingGrid.repositionNow = true;
            sv1.ResetPosition();

            yield return new WaitForEndOfFrame();
        }
        Cover.SetActive(false);
    }
    public void RefreshRank(List<PlayerRankInfo> cciLst)
    {
        MyselfRank.Set();
        List<RankInfo> TempRankDataLst = new List<RankInfo>();
        for (int i = 0; i < cciLst.Count; i++)
        {
            if (MyselfRI.ServerRankInfo.name == cciLst[i].name)
            {
                MyselfRI.Rank = (i + 1);
                MyselfRank.Set(MyselfRI);
            }
            TempRankDataLst.Add(new RankInfo((i+1),cciLst[i]));
        }
        StartCoroutine("RefreshRankingList", TempRankDataLst);
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }
    /// <summary> 分类标签 </summary>
    public void OnTypeToggleChange()
    {
        if (UIToggle.current.value)
        {
            switch (UIToggle.current.name)
            {
                case "Type_1":
                    CurShowRankType = 1;

                    RankTitleLab.text = "最强魔法师排行榜";
                    RankTitleIcon.spriteName = "pic_biaoti_paiming";
                    RankValueDes.text = "魔力值";

                    StartCoroutine("RefreshRankingList", TempPreliminaryRankDataLst_1);
                    break;
                case "Type_2":
                    CurShowRankType = 2;

                    RankTitleLab.text = "最萌魔法师排行榜";
                    RankTitleIcon.spriteName = "pic_biaoti_paiming";
                    RankValueDes.text = "萌点";

                    StartCoroutine("RefreshRankingList", TempPreliminaryRankDataLst_2);
                    break;
                case "Type_3":
                    CurShowRankType = 3;

                    RankTitleLab.text = "最高魔法师排行榜";
                    RankTitleIcon.spriteName = "pic_biaoti_paiming";
                    RankValueDes.text = "等级";

                    StartCoroutine("RefreshRankingList", TempPreliminaryRankDataLst_3);
                    break;
                case "Type_4":
                    
                    break;
            }
            Debug.Log(UIToggle.current.name);
        }
    }
    #endregion
}
