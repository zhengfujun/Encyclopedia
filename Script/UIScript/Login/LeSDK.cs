using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using MyMiniJSON;
using System.Threading;

public class LeSDKInfo
{
    public string AccessToken;
    public int UserId;
    public string UserName;
}

public class LePayInfo
{
    public string Price;

    public string ProductID;
    public string ProductName;
    public string ProductDes;

    public string NotifyURL;
    public string ExtraInfo;

    public LePayInfo(string price,string productID,string productName,string productDes,string notifyURL,string extraInfo)
    {
        Price = price;
        ProductID = productID;
        ProductName = productName;
        ProductDes = productDes;
        NotifyURL = notifyURL;
        ExtraInfo = extraInfo;
    }

    public string ToString(LeSDKInfo lesdkinfo)
    {
        return StringBuilderTool.ToString(
                /*0*/lesdkinfo.AccessToken, "_",
                /*1*/lesdkinfo.UserId, "_",
                /*2*/Price, "_",
                /*3*/ProductID, "_",
                /*4*/ProductName, "_",
                /*5*/ProductDes, "_",
                /*6*/NotifyURL, "_",
                /*7*/ExtraInfo);
    }
}

public class LeSDK : MonoBehaviour
{
    AndroidJavaClass PlayerActivity = null;
    AndroidJavaObject playerActivity = null;
    AndroidJavaObject TestPlugins = null;

    [HideInInspector]
    public LeSDKInfo SDKInfo = new LeSDKInfo();

    bool CallSDKFun(string FunName)
    {
        if (TestPlugins != null && playerActivity != null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            TestPlugins.Call(FunName, playerActivity);
#endif
            return true;
        }
        else
            return false;
    }
    bool CallSDKFun(string FunName, string Param)
    {
        if (TestPlugins != null && playerActivity != null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            TestPlugins.Call(FunName, playerActivity, Param);
#endif
            return true;
        }
        else
            return false;
    }

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");
            TestPlugins = new AndroidJavaObject("com.sdk.le.main");
            if(CallSDKFun("Create"))
            {
                GameApp.Instance.LeSDKInstance = this;
            }
        }
#endif
    }

    void OnDestroy()
    {
        CallSDKFun("Destroy");
    }

    void OnApplicationFocus(bool bFocus)
    {
        if (bFocus)
        {
            //CallSDKFun("Resume");//支付后会闪退，暂时注掉2018.12.7， 有空研究一下。。。
        }
    }

    void OnApplicationPause(bool bPause)
    {
        if (bPause)
        {
            //CallSDKFun("Pause");//支付后会闪退，暂时注掉2018.12.7， 有空研究一下。。。
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Exit();
        }
        /*if (Input.GetKeyUp(KeyCode.A))
        {
            GameApp.Instance.Platform.ThirdOauthLogin("XXXqgKJLzQ8nZvnm58MCgm24tJ7m169k9m3PZsQWm2eSyNaxm3RBlm5JsQ67p50Vp29lm1gxkrQkSQZneRRxrSywlI83hGV1gDa69HtkQm2dSxZ2U5ozddUm4");
        }
        //if (Input.GetKeyUp(KeyCode.B))
        {
            //GameApp.Instance.Platform.GetRole();
        } 
        //if (Input.GetKeyUp(KeyCode.C))
        {
            //GameApp.Instance.Platform.CheckRoleName("萨瓦迪卡");
        }
        //if (Input.GetKeyUp(KeyCode.D))
        {
            //GameApp.Instance.Platform.GetGoodsList();
        }
        //if (Input.GetKeyUp(KeyCode.E))
        {
            //GameApp.Instance.Platform.CreateOrder(GameApp.Instance.Platform.GoodsDic[36].ID);
        }

        //if (Input.GetKeyUp(KeyCode.Z))
        {
            
        }*/
    }

    public void Exit()
    {
        if (CallSDKFun("Exit"))
        {
            GameApp.SendMsg.StartWaitUI();
            InvokeRepeating("UpdateExitRes", 0, 0.5f);
        }
        else
        {
            GameApp.Instance.CommonMsgDlg.OpenMsgBox(Localization.Get("isQuit"),
                (isClickOK) =>
                {
                    if (isClickOK)
                    {
                        Application.Quit();
                    }
                });
        }
    }
    void UpdateExitRes()
    {
        Action Over = () =>
        {
            CancelInvoke("UpdateExitRes");
            GameApp.SendMsg.EndWaitUI();
        };

        string ExitRes = TestPlugins.Call<string>("GetExitRes");
        Debug.Log(StringBuilderTool.ToInfoString("退出结果：", ExitRes));

        if (ExitRes.Length > 0)
        {
            switch (ExitRes)
            {
                case "Wait":
                    break;
                case "Success":
                    Over();
                    Application.Quit();
                    break;                
                case "Cancel":
                    Over();
                    break;
            }
        }
    }

    public void Login()
    {
        Debug.Log("登陆");

        GameApp.Instance.CurAccountType = AccountType.LeSDK;

        if (CallSDKFun("Login"))
        {
            GameApp.SendMsg.StartWaitUI();
            InvokeRepeating("UpdateLoginRes", 0, 0.5f);
        }
    }
    public void SwitchAccount()
    {
        Debug.Log("切换账号");

        if (CallSDKFun("SwitchUser"))
        {
            GameApp.SendMsg.StartWaitUI();
            InvokeRepeating("UpdateLoginRes", 0, 0.5f);
        }
    }    
    void UpdateLoginRes()
    {
        Action Over = () =>
            {
                CancelInvoke("UpdateLoginRes");
                GameApp.SendMsg.EndWaitUI();
            };

        string LoginRes = TestPlugins.Call<string>("GetLoginRes");
        Debug.Log(StringBuilderTool.ToInfoString("登陆结果：", LoginRes));

        string[] s = LoginRes.Split('_');
        if(s.Length > 0)
        {
            switch (s[0])
            {
                case "Wait":
                    break;
                case "Success":
                    Debug.Log("登录成功！");
                    Over();

                    SDKInfo.AccessToken = s[1];
                    SDKInfo.UserId = int.Parse(s[2]);
                    SDKInfo.UserName = s[3];

                    Debug.Log(StringBuilderTool.ToInfoString("AccessToken：", SDKInfo.AccessToken));
                    Debug.Log(StringBuilderTool.ToString("UserId：", SDKInfo.UserId));
                    Debug.Log(StringBuilderTool.ToInfoString("UserName：", SDKInfo.UserName));

                    GameApp.Instance.UILogin.RecordNickName = SDKInfo.UserName;
                    GameApp.Instance.UILogin.Account = SDKInfo.UserName;

                    GameApp.Instance.Platform.LeSDKLogin(s[1]);
                    break;
                case "NullUserInfo":
                    GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox("登录异常！用户信息为空！");
                    Over();
                    break;
                case "Failure":
                    GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString("登录失败！\n错误码：", s[1], "\n错误描述：", s[2]));
                    Over();
                    break;
                case "Cancel":
                    Debug.Log("取消登录！");
                    Over();

                    //清空数据，等待重新登陆...
                    SDKInfo = new LeSDKInfo();
                    GameApp.Instance.Platform.PlatformInfo = new KOIPlatformInfo();
                    GameApp.Instance.UILogin.ShowLeSDKLogin();
                    break;
            }
        }
    }

    public void Pay(GoodsInfo CurWaitBuyGoodsInfo)
    {
        Debug.Log("支付");

#if UNITY_ANDROID
        TDGAVirtualCurrency.OnChargeRequest(GameApp.Instance.Platform.PlatformInfo.orderId, CurWaitBuyGoodsInfo.ID.ToString(), CurWaitBuyGoodsInfo.GetPrice(), "CH", CurWaitBuyGoodsInfo.GetPrice(), "LePay");
#endif

        LePayInfo tempPI = new LePayInfo(CurWaitBuyGoodsInfo.GetPriceStr(),
            CurWaitBuyGoodsInfo.ID.ToString(), CurWaitBuyGoodsInfo.Name, CurWaitBuyGoodsInfo.Des,
            "http://platform01.baobaolong.club/platform/billing/callback/letvCallback", GameApp.Instance.Platform.PlatformInfo.orderId);

        if (CallSDKFun("Pay", tempPI.ToString(SDKInfo)))
        {
            GameApp.SendMsg.StartWaitUI();
            InvokeRepeating("UpdatePayRes", 0, 0.5f);
        }
    }
    void UpdatePayRes()
    {
        Action Over = () =>
        {
            CancelInvoke("UpdatePayRes");
            GameApp.SendMsg.EndWaitUI();
        };

        string PayRes = TestPlugins.Call<string>("GetPayRes");
        Debug.Log(StringBuilderTool.ToInfoString("支付结果：", PayRes));

        string[] s = PayRes.Split('#');
        if (s.Length > 0)
        {
            if (s[0] != "Wait")
            {
                Over();

                if (s[0] == "SUCCESS")
                {
                    if (s.Length > 1)
                    {
                        string[] msg = s[1].Split('&');
                        for(int i = 0; i < msg.Length; i++)
                        {
                            string[] data = msg[i].Split('=');
                            if (data.Length == 2)
                            {
                                Debug.Log(StringBuilderTool.ToInfoString(data[0], "=", data[1], "\n"));
                            }
                        }
                    }
                    GameApp.Instance.CommonHintDlg.OpenHintBox("支付成功！");
#if UNITY_ANDROID
                    TDGAVirtualCurrency.OnChargeSuccess(GameApp.Instance.Platform.PlatformInfo.orderId);
#endif
                    //GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString("支付成功！\n", s[1]));
                }
                else
                {
                    GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString("支付失败！\n", s[1]));
                }
            }
        }
    }
}
