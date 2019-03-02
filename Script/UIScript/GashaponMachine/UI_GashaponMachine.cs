using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum EGashaponType
{
    eNormal,
    eGold
}

public class UI_GashaponMachine : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public UILabel NormalCoinCountLab;
    public UILabel GoldCoinCountLab;

    //public UISprite GashaponAnimSpr;
    public Animation MachineRockAnim;
    public Animation BallAnim;

    public UILabel[] BroadcastLabs;
    private int CurShowBroadLabIdx = 0;

    private string[] TempMsgs = new string[9]
    {
        "[00ffff]布赖恩仙女[-]扭到[ffff00]一个地鲶鱼[-]",
        "[00ffff]西德尼术士[-]扭到[ffff00]一个红毛猩猩[-]",
        "[00ffff]托马斯酋长[-]扭到[ffff00]一个黑斑羚[-]",
        "[00ffff]所罗门怒兽[-]扭到[ffff00]一个河马[-]",
        "[00ffff]斯帕克行者[-]扭到[ffff00]一个小丑鱼[-]",
        "[00ffff]斯考特焰魂[-]扭到[ffff00]一个伤齿龙[-]",
        "[00ffff]史蒂文女皇[-]扭到[ffff00]一个剑鱼[-]",
        "[00ffff]史蒂夫龙龟[-]扭到[ffff00]一个美颌龙[-]",
        "[00ffff]布鲁诺收割者[-]扭到[ffff00]一个灯笼鱼[-]"
    };

    public GameObject WaitOpenPanel;

    private int GashaponResult = 0;

    public GameObject AwardPanel;
    public UI_MagicCard AwardCard;
    public UILabel AwardHint;

    /*void Awake()
    {

    }*/

    void Start()
    {
        StartCoroutine("TempRunMsg");
    }

    /*void OnDestroy()
    {

    }*/

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            //GameApp.SendMsg.GMOrder("AddItem 1001 8000");
            //GameApp.SendMsg.GMOrder("AddItem 1002 2000");
            //GameApp.SendMsg.GMOrder("AddItem 20001 20");
            //GameApp.SendMsg.GMOrder("AddItem 20002 20");
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            //GameApp.SendMsg.Gacha(1);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            //GameApp.SendMsg.Gacha(2);
        }
    }*/

    public void Show(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            GameApp.Instance.UICurrency.Show(true);
            
            RefreshCoinCount();
        }
        else
        {
            if (GameApp.Instance.HomePageUI != null)
                GameApp.Instance.UICurrency.Show(false);

            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    public void Gashapon(int CardID)
    {
        GashaponResult = CardID;

        RefreshCoinCount();

        StartCoroutine("_DoGashapon");
    }

    IEnumerator _DoGashapon()
    {
        /*int cnt = 0;
        int PicIdx = 0;
        while (cnt <= 30)
        {
            PicIdx = cnt % 10 + 1;
            GashaponAnimSpr.spriteName = "gundan_" + (PicIdx <= 9 ? "0" + PicIdx : PicIdx.ToString());
            cnt++;
            yield return new WaitForSeconds(Time.deltaTime);
        }*/

        MachineRockAnim.Play();

        yield return new WaitForSeconds(1.2f);

        WaitOpenPanel.SetActive(true);
        BallAnim.Play("ballinit");
        TweenAlpha.Begin(WaitOpenPanel, 0.2f, 1).from = 0;
    }

    //广播其他人扭到珍贵道具的消息
    IEnumerator TempRunMsg()
    {
        int idx = 0;
        while(true)
        {
            BroadcastOthersGetItemsMsg(TempMsgs[idx]);
            idx++;
            if (idx >= TempMsgs.Length)
                idx = 0;

            yield return new WaitForSeconds(3);
        }
    }
    private void BroadcastOthersGetItemsMsg(string msg)
    {
        BroadcastLabs[CurShowBroadLabIdx].text = msg;

        StartCoroutine("_DoRollMsg");
    }
    IEnumerator _DoRollMsg()
    {
        GameObject CurLabObj = BroadcastLabs[CurShowBroadLabIdx].gameObject;
        GameObject LastLabObj = BroadcastLabs[CurShowBroadLabIdx == 0 ? 1 : 0].gameObject;

        TweenPosition.Begin(CurLabObj, 0.2f, new Vector3(0, -4, 0));
        TweenAlpha.Begin(CurLabObj, 0.2f, 1);

        TweenPosition.Begin(LastLabObj, 0.2f, new Vector3(0, 46, 0));
        TweenAlpha.Begin(LastLabObj, 0.2f, 0);

        CurShowBroadLabIdx++;
        if(CurShowBroadLabIdx > 1)
            CurShowBroadLabIdx = 0;

        yield return new WaitForSeconds(0.3f);

        LastLabObj.transform.localPosition = new Vector3(0,-54,0);
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }
    /// <summary> 点击普通扭蛋 </summary>
    public void OnClick_NormalGashapon()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【普通扭蛋】");

        if (GameApp.SocketClient_Game == null)
            Gashapon(UnityEngine.Random.Range(50005, 50012));
        else
            GameApp.SendMsg.Gacha(1);
    }
    /// <summary> 点击金色扭蛋 </summary>
    public void OnClick_GoldGashapon()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【金色扭蛋】");

        if (GameApp.SocketClient_Game == null)
            Gashapon(UnityEngine.Random.Range(50012, 50020));
        else
            GameApp.SendMsg.Gacha(2);
    }
    /// <summary> 点击直接打开 </summary>
    public void OnClick_DirectOpen()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【直接打开】");

        StartCoroutine("_Open");
    }
    IEnumerator _Open()
    {
        BallAnim["ballopen"].speed = 2;
        BallAnim.Play("ballopen");

        yield return new WaitForSeconds(0.25f);

        AwardPanel.SetActive(true);
        TweenAlpha.Begin(AwardPanel, 0f, 1).from = 0;
        TweenScale.Begin(AwardPanel, 0.2f, Vector3.one).from = Vector3.zero;
        AwardCard.UnconditionalShow(GashaponResult);

        AwardHint.text = StringBuilderTool.ToString("恭喜您，获得 [FEE209]", AwardCard.FullName(), "[-]");

        TweenAlpha.Begin(WaitOpenPanel, 0.2f, 0).onFinished.Add(new EventDelegate(() =>
        {
            WaitOpenPanel.SetActive(false);
        }));
    }

    void RefreshCoinCount()
    {
        NormalCoinCountLab.text = SerPlayerData.GetItemCount(20001).ToString();
        GoldCoinCountLab.text = SerPlayerData.GetItemCount(20002).ToString();
    }

    /// <summary> 点击关闭奖励界面 </summary>
    public void OnClick_CloseAward()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭奖励界面】");

        TweenAlpha.Begin(AwardPanel, 0.2f, 0).onFinished.Add(new EventDelegate(() =>
        {
            AwardPanel.SetActive(false);

            AwardPanel.transform.localScale = Vector3.zero;
        }));
    }
    #endregion
}
