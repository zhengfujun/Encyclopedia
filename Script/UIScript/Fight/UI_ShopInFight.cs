using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShopInFight : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public UI_PlayerInfo MainPlayerInfo;

    public UI_CommodityInFight[] Commoditys;

    public UILabel CountdownLab;
    public UISprite CountdownIcon;

    public GameObject Confirm;
    public UILabel BuyCommodityDes;
    
    private Action CloseCB = null;

    public GameObject HintObj;
    public UILabel HintDes;

    private int ShopInFightCD;

    /*void Awake()
    {

    }*/

    void Start()
    {
        MainPlayerInfo.Init(GameApp.Instance.MainPlayerData);

        int CommodityIdx = 0;
        foreach(KeyValuePair<int, ShopConfig> pair in CsvConfigTables.Instance.ShopCsvDic)
        {
            if(pair.Value.Type == 10)
            {
                if (CommodityIdx < Commoditys.Length)
                {
                    Commoditys[CommodityIdx].Set(pair.Value);
                    CommodityIdx++;
                }
            }
        }

        ShopInFightCD = GameApp.Instance.GetParameter("ShopInFightCD");
    }

    /*void OnDestroy()
    {

    }*/

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {

        }
    }*/

    public void Show(bool isShow, Action CloseCallback = null)
    {
        if (isShow)
        {
            gameObject.SetActive(true);
            StartCoroutine("DelayShow");
    
            CloseCB = CloseCallback;
        }
        else
        {
            Confirm.SetActive(false);
            AppearEffect.Close(AppearType.Popup, () => 
            {
                if (CloseCB != null)
                {
                    CloseCB();
                    CloseCB = null;
                }

                gameObject.SetActive(false); 
            });
        }
    }
    IEnumerator DelayShow()
    {
        //yield return new WaitForSeconds(0.1f);
        //transform.localPosition = new Vector3(-100, 0, 0);
        AppearEffect.transform.localScale = Vector3.one*0.01f;
        //yield return new WaitForSeconds(0.1f);
        AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);
        yield return new WaitForSeconds(AppearEffect.OpenConsuming);

        GameApp.Instance.SoundInstance.PlayVoice("Peddle");

        StartCoroutine("Countdown");
    }
    IEnumerator Countdown()
    {
        CountdownIcon.spriteName = "biao-02";
        CountdownIcon.transform.localScale = Vector3.one;

        int cd = ShopInFightCD;
        while (cd >= 0)
        {
            cd--;
            CountdownLab.text = cd.ToString();

            if (cd == 10)
            {
                CountdownIcon.spriteName = "biao-01";
                CountdownIcon.transform.localScale = Vector3.one * 1.2f;
            }
            else if (cd < 10)
            {
                GameApp.Instance.SoundInstance.PlaySe("countdown");
                iTween.ShakePosition(CountdownLab.gameObject, new Vector3(0.05f, 0.03f, 0), 0.2f);
            }

            if (cd == 0)
            {
                Debug.Log("关闭界面");
                Show(false);
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    private Action BuyCallback = null;
    public void ShowConfirm(string des, Action callback)
    {
        Confirm.SetActive(true);
        BuyCommodityDes.text = des;

        BuyCallback = callback;
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        StopCoroutine("Countdown");
        Show(false);
    }
    /// <summary> 点击确认购买 </summary>
    public void OnClick_ConfirmBuy()
    {
        //GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【确认购买】");

        if(BuyCallback != null)
        {
            BuyCallback();
            BuyCallback = null;
        }
        Confirm.SetActive(false);

        GameApp.Instance.SoundInstance.PlaySe("Buy");
    }
    /// <summary> 点击放弃购买 </summary>
    public void OnClick_CancelBuy()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【放弃购买】");

        Confirm.SetActive(false);
    }
    #endregion

    public void ShowHint(string des)
    {
        HintObj.SetActive(true);
        HintDes.text = des;
        StartCoroutine("DelayHideHint");
    }
    IEnumerator DelayHideHint()
    {
        yield return new WaitForSeconds(1.5f);
        HintObj.SetActive(false);
    }
}
