using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_Currency : MonoBehaviour
{
    public GameObject GoldRoot;
    public GameObject DiamondRoot;
    //public GameObject CandyRoot;

    public UILabel GoldCnt;
    public UILabel DiamondCnt;
    public UILabel CandyCnt;

    public GameObject[] StrengthSpr;
    public UISprite PhoneBatteryInSpr;

    void Awake()
    {
        GameApp.Instance.UICurrency = this;

        Refresh();
    }

    void Start()
    {
        StartCoroutine("UpdataBattery");
    }

    void OnDestroy()
    {
        GameApp.Instance.UICurrency = null;
    }

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {

        }
    }*/

    public void Refresh()
    {
        if (GameApp.Instance.FightUI != null)
            return;

        uint Gold = SerPlayerData.GetItemCount(1001);
        //uint Diamond = SerPlayerData.GetItemCount(1002);

        GoldCnt.text = Gold.ToString();
        //DiamondCnt.text = Diamond.ToString();

        if(CandyCnt != null)
        {
            uint Candy = SerPlayerData.GetItemCount(1003);
            CandyCnt.text = Candy.ToString();
        }
    }

    public void AccumulationGold(uint val)
    {
        GoldCnt.text = (int.Parse(GoldCnt.text) + val).ToString();
    }

    public void Show(bool isShow)
    {
        GoldRoot.SetActive(true);
        //DiamondRoot.SetActive(true);

        if (isShow)
        {
            TweenAlpha.Begin(gameObject,0.3f,1);
        }
        else
        {
            TweenAlpha.Begin(gameObject, 0.1f, 0);
        }
    }

    public void OnlyShowMagicPower()
    {
        Show(true);

        GoldRoot.SetActive(false);
    }

    public void OnlyShowState()
    {
        Show(true);

        GoldRoot.SetActive(false);
        //DiamondRoot.SetActive(false);
    }

    //刷新网络连接状态
    public void RefreshPingState(ulong ping)
    {
        int state = 0;
        if (ping < 100)
        {
            state = 3;
        }
        else if (ping >= 100 && ping < 300)
        {
            state = 2;
        }
        else
        {
            state = 1;
        }

        for (int i = 0; i < StrengthSpr.Length; i++)
        {
            StrengthSpr[i].SetActive(i < state);
        }
#if UNITY_EDITOR
        Debug.Log("游戏服 Ping值：" + ping + "ms");
#endif
    }

    //更新手机电量
    IEnumerator UpdataBattery()
    {
        WaitForSeconds wfs = new WaitForSeconds(300f);
        while (true)
        {
            int curBattery = GetBatteryLevel();
            if (curBattery < 0)
                PhoneBatteryInSpr.fillAmount = 1f;
            else
                PhoneBatteryInSpr.fillAmount = (float)curBattery / 100f;

            yield return wfs;
        }
    }

    //读取手机电量
    int GetBatteryLevel()
    {
        try
        {
            string CapacityString = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
            return int.Parse(CapacityString);
        }
        catch (Exception e)
        {
            Debug.Log("读取失败; " + e.Message);
        }
        return -1;
    }
}
