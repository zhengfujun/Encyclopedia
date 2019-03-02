using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common;

public class RankInfo
{
    public int Rank;
    public PlayerRankInfo ServerRankInfo;

    public RankInfo(int rank, PlayerRankInfo SerRI)
    {
        Rank = rank;
        ServerRankInfo = SerRI;
    }
}

public class UI_RankUnit : MonoBehaviour
{
    public UISprite Bg;
    public UILabel RankLab;
    public UISprite RankIcon;
    public UILabel Title;
    public UILabel PlayerName;
    public UILabel Score;

    private bool AutoHide = false;
    private float HideYMaxBorder = 1f;
    private float HideYMinBorder = -1f;
    public void SetHideYBorder(float max,float min)
    {
        HideYMaxBorder = max;
        HideYMinBorder = min;
    }
    public void SetBgType()
    {
        int rank = int.Parse(RankLab.text);
        if (rank == 1)
        {
            Bg.spriteName = "bg_paiming_1";

            RankLab.gameObject.SetActive(false);

            RankIcon.gameObject.SetActive(true);
            RankIcon.spriteName = "icon_paiming_1";

            Title.gameObject.SetActive(true);

            switch (GameApp.Instance.HomePageUI.RankingUI.CurShowRankType)
            {
                case 1: Title.text = "灰常腻害"; break;
                case 2: Title.text = "灰常萌"; break;
                case 3: Title.text = "灰常高"; break;
                case 4: Title.text = "灰常多"; break;
            }
        }
        else if (rank == 2)
        {
            Bg.spriteName = "bg_paiming_2";

            RankLab.gameObject.SetActive(false);

            RankIcon.gameObject.SetActive(true);
            RankIcon.spriteName = "icon_paiming_2";

            Title.gameObject.SetActive(true);

            switch (GameApp.Instance.HomePageUI.RankingUI.CurShowRankType)
            {
                case 1: Title.text = "很腻害"; break;
                case 2: Title.text = "很萌"; break;
                case 3: Title.text = "很高"; break;
                case 4: Title.text = "很多"; break;
            }
        }
        else if (rank == 3)
        {
            Bg.spriteName = "bg_paiming_3";

            RankLab.gameObject.SetActive(false);

            RankIcon.gameObject.SetActive(true);
            RankIcon.spriteName = "icon_paiming_3";

            Title.gameObject.SetActive(true);
            
            switch (GameApp.Instance.HomePageUI.RankingUI.CurShowRankType)
            {
                case 1: Title.text = "腻害"; break;
                case 2: Title.text = "萌"; break;
                case 3: Title.text = "高"; break;
                case 4: Title.text = "多"; break;
            }
        }
        else if (rank % 2 == 0)
        {
            Bg.color = new Color(0.8f,0.8f,0.8f);

            RankIcon.gameObject.SetActive(false);

            Title.gameObject.SetActive(false);
        }
        else if (rank % 2 == 1)
        {
            RankIcon.gameObject.SetActive(false);

            Title.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (AutoHide)
        {
            //if (transform.name == "RankUnit_0")
            //    Debug.Log(transform.name + " " + transform.position.y);
            if (transform.position.y > HideYMaxBorder || transform.position.y < HideYMinBorder)
            {
                ShowChild(false);//超出显示范围的隐藏子控件
            }
            else
            {
                ShowChild(true);
            }
        }
    }

    public void Set()
    {
        AutoHide = false;
        
        RankLab.text = "未上榜";
        PlayerName.text = "我自己";
        Score.text = "0";

        RankIcon.gameObject.SetActive(false);
        Title.gameObject.SetActive(false);
    }

    public void Set(RankInfo ri)
    {
        AutoHide = true;

        RankLab.text = ri.Rank.ToString();
        PlayerName.text = ri.ServerRankInfo.name;
        Score.text = ri.ServerRankInfo.score.ToString();

        SetBgType();
    }

    private void ShowChild(bool isShow)
    {
        if (PlayerName.gameObject.activeSelf == isShow)
            return;

        PlayerName.gameObject.SetActive(isShow);
        Score.gameObject.SetActive(isShow);
    }
}
