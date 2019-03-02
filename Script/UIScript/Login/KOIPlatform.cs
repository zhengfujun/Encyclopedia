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

public class KOIPlatformInfo
{
    public int userId;
    public int gameId = 1005;
    public int siteId
    {
        get
        {
#if UNITY_EDITOR
            return 211;
#elif UNITY_ANDROID
            return 213;
#elif UNITY_IPHONE
            return 212;
#endif
        }
    }
    public string accessToken;
    public string refreshToken//下次快捷登录时使用
    {
        get
        {
            if (PlayerPrefs.HasKey("KOIPlatform_refreshToken"))
            {
                return PlayerPrefs.GetString("KOIPlatform_refreshToken");
            }
            else
            {
                return "";
            }
        }
        set
        {
            PlayerPrefs.SetString("KOIPlatform_refreshToken", value);
        }
    }

    public long roleId;

    public string orderId;
}

public enum HttpResult
{
    ConnectNone,
    ConnectContinue,
    ConnectSuccess,
    ConnectException,
}

public enum EPlatformFunType
{
    eLeSDKLogin,
    eGetRole,
    eCheckRoleName,
    eGetGoodsList,
    eCreateOrder,

    eSendSmsToUser,
    eRegisterPhoneNumAccount,
    ePhoneNumLogin,
    ePhoneNumLoginUseVerificationCode,
    eTokenLogin,
    eQuickLogin,

    eWeChatLogin,
}

public class GoodsInfo
{
    public int ID;
    public string Code;
    public int CfgItemID;
    public int Amount;
    public int Count;
    public string Name;
    public string Des;
    public string Icon;

    public GoodsInfo(int goodsId, string goodsCode, int relatedGoodsId, int goodsAmount, int goodsCount, string goodsName, string goodsDesc, string goodsPic)
    {
        ID = goodsId;
        Code = goodsCode;
        CfgItemID = relatedGoodsId;
        Amount = goodsAmount;
        Count = goodsCount;
        Name = goodsName;
        Des = goodsDesc;
        Icon = goodsPic;
    }

    public double GetPrice()
    {
        return double.Parse((((float)Amount / 100f).ToString("f2")));
    }

    public string GetPriceStr()
    {
        return ((float)Amount / 100f).ToString("f2");
    }
}

public class KOIPlatform : MonoBehaviour
{
    private string deviceType
    {
        get
        {
#if UNITY_ANDROID
            return "1";
#elif UNITY_IPHONE
            return "2";
#endif
        }
    }

    private HttpResult curHttpResult = HttpResult.ConnectNone;
    private string ConnectExceptionInfo = string.Empty;
    private Thread httpThread;
    private string platformRetStrInfo = null;

    [HideInInspector]
    public KOIPlatformInfo PlatformInfo = new KOIPlatformInfo();

    [HideInInspector]
    public Dictionary<int, GoodsInfo> GoodsDic = new Dictionary<int, GoodsInfo>();
    [HideInInspector]
    public bool IsGetGetGoodsListOver = false;

    void Start()
    {
        GameApp.Instance.Platform = this;
    }

    string GetPlatformURL(EPlatformFunType FunType)
    {
        string Url = "";
        switch(Const.PlatformUrlType)
        {
            case 0:
                Url = "http://platformces.koigame.cn:8888/";
                break;
            case 1:
                Url = "http://platform01.baobaolong.club/";
                break;
        }
        switch (FunType)
        {
            case EPlatformFunType.eLeSDKLogin:
                Url += "platform/user/thirdOauthLogin?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&accountType=1&userId={0}&deviceType=" + deviceType + "&gameVersion=100&appVersion=200";
                break;
            case EPlatformFunType.eGetRole:
                Url += "platform/game/getRole?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&areaCode=3001&userId=" + PlatformInfo.userId + "&accessToken=" + PlatformInfo.accessToken + "&channel=" + PlatformInfo.siteId + "&giftId=0";
                break;
            case EPlatformFunType.eCheckRoleName:
                Url += "platform/game/checkRoleName?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&areaCode=3001&roleId=" + PlatformInfo.roleId + "&roleName={0}&accessToken=" + PlatformInfo.accessToken + "&userId=" + PlatformInfo.userId;
                break;
            case EPlatformFunType.eGetGoodsList:
                Url += "platform/billing/getGoodsList?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&areaCode=3001&roleId=" + PlatformInfo.roleId + "&shopType=1";
                break;
            case EPlatformFunType.eCreateOrder:
                Url += "platform/billing/createOrder?areaCode=3001&roleId=" + PlatformInfo.roleId + "&goodsCode={0}&payType=0&orderDesc={1}&payAmount={2}&gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&userId=" + PlatformInfo.userId + "&accessToken=" + PlatformInfo.accessToken;
                break;
            case EPlatformFunType.eSendSmsToUser:
                Url += "platform/user/public/sendSmsToUser?gameId=" + PlatformInfo.gameId + "&accountName={0}";
                break;
            case EPlatformFunType.eRegisterPhoneNumAccount:
                Url += "platform/user/registerAccount?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&accountType=6&deviceType=" + deviceType + "&gameVersion=100&accountName={0}&password=123456&signCode={1}";
                break;
            case EPlatformFunType.ePhoneNumLogin:
                Url += "platform/user/login?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&accountType=4&deviceType=" + deviceType + "&gameVersion=100&accountName={0}&password={1}";
                break;
            case EPlatformFunType.ePhoneNumLoginUseVerificationCode:
                Url += "platform/user/loginByPhoneTicket?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&accountType=6&deviceType=" + deviceType + "&gameVersion=100&accountName={0}&password=123456&signCode={1}";
                break;
            case EPlatformFunType.eTokenLogin:
                Url += "platform/user/tokenLogin?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&accountType=4&deviceType=" + deviceType + "&gameVersion=100&refreshToken=" + PlatformInfo.refreshToken;
                break;
            case EPlatformFunType.eQuickLogin:
                Url += "platform/user/public/quickLogin?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&accountType=1&userId=" + SystemInfo.deviceModel + MyTools.GenerateUniqueText(8) + "&deviceType=" + deviceType + "&gameVersion=100&appVersion=200";
                break;
            case EPlatformFunType.eWeChatLogin:
                Url += "platform/user/thirdOauthLogin?gameId=" + PlatformInfo.gameId + "&siteId=" + PlatformInfo.siteId + "&accountType=1&userId={0}&accountName={1}&accessToken={2}&deviceType=" + deviceType + "&gameVersion=100&appVersion=200";
                break;
        }
        return Url;
    }

    void HttpLinkGet(string postDataStr)
    {
        curHttpResult = HttpResult.ConnectContinue;
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        StreamReader myStreamReader = null;
        Stream myResponseStream = null;
        Debug.Log(StringBuilderTool.ToInfoString("postDataStr：", postDataStr));
        httpThread = new Thread(delegate()
        {
            try
            {
                request = (HttpWebRequest)WebRequest.Create(postDataStr);
                request.Timeout = 5000;
                request.ReadWriteTimeout = 5000;
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                platformRetStrInfo = myStreamReader.ReadToEnd();
                curHttpResult = HttpResult.ConnectSuccess;
                Debug.Log(StringBuilderTool.ToInfoString("platformRetStrInfo：", platformRetStrInfo));
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                curHttpResult = HttpResult.ConnectException;
                ConnectExceptionInfo = e.Message;
            }
            finally
            {
                if (request != null) request.Abort();
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                    myStreamReader.Dispose();
                }
                if (myResponseStream != null)
                {
                    myResponseStream.Close();
                    myResponseStream.Dispose();
                }
                if (response != null)
                    response.Close();
            }
        });
        httpThread.IsBackground = true;
        httpThread.Start();
    }

    private Action SendSmsSucceedCallback = null;
    public void SendSmsToUser(string PhoneNum, Action SendSucceed)
    {
        GameApp.SendMsg.StartWaitUI();

        HttpLinkGet(StringBuilderTool.AppendFormat(GetPlatformURL(EPlatformFunType.eSendSmsToUser), PhoneNum));

        SendSmsSucceedCallback = SendSucceed;

        StartCoroutine("WaitToSendSmsToUser");
    }
    IEnumerator WaitToSendSmsToUser()
    {
        while (curHttpResult == HttpResult.ConnectContinue)
        {
            yield return new WaitForEndOfFrame();
        }

        if (curHttpResult == HttpResult.ConnectException)
        {
            GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString("连接异常！\nWaitToSendSmsToUser\n", ConnectExceptionInfo));
            Clear();
            yield break;
        }
        else
        {
            LitJson.JsonData jsonRes = LitJson.JsonMapper.ToObject(platformRetStrInfo);
            if (jsonRes["status"].ToString() == "0")
            {
                /*{
                    "message":"SUCCESS",
                    "status":0
                }*/

                GameApp.Instance.CommonHintDlg.OpenHintBox("验证码已发送！请注意查收");
                
                GameApp.Instance.WaitLoadHintDlg.CloseHintBox();

                if(SendSmsSucceedCallback != null)
                {
                    SendSmsSucceedCallback();
                    SendSmsSucceedCallback = null;
                }
            }
            else
            {
                LitJson.JsonData josnData = jsonRes["data"];
                int errorCode = int.Parse(josnData["errorCode"].ToString());
                string errorMessage = josnData["errorMessage"].ToString();
                GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(
                    StringBuilderTool.ToString("获取短信验证码失败！\n错误码：", errorCode, "\n错误描述：", errorMessage));
            }
        }

        Clear();
    }

    public void RegisterPhoneNumAccount(string PhoneNum, string VerificationCode)
    {
        GameApp.SendMsg.StartWaitUI();
        GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在使用手机号注册...");

        HttpLinkGet(StringBuilderTool.AppendFormat(GetPlatformURL(EPlatformFunType.eRegisterPhoneNumAccount), PhoneNum, VerificationCode));
        StartCoroutine("WaitToLogin");
    }

    public void PhoneNumLogin(string PhoneNum, string Password)
    {
        GameApp.SendMsg.StartWaitUI();
        GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在使用手机号登录...");

        HttpLinkGet(StringBuilderTool.AppendFormat(GetPlatformURL(EPlatformFunType.ePhoneNumLogin), PhoneNum, Password));
        StartCoroutine("WaitToLogin");
    }

    private Action TokenLoginSucceedCallback = null;
    private Action TokenLoginFailureCallback = null;
    public void TokenLogin(Action LoginSucceed, Action LoginFailure)
    {
        TokenLoginSucceedCallback = LoginSucceed;
        TokenLoginFailureCallback = LoginFailure;

        if (PlatformInfo.refreshToken.Length > 0)
        {
            GameApp.SendMsg.StartWaitUI();
            GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在使用上次登录信息进行快速登录...");

            HttpLinkGet(GetPlatformURL(EPlatformFunType.eTokenLogin));

            StartCoroutine("WaitToLogin");
        }
        else
        {
            if(TokenLoginFailureCallback != null)
            {
                TokenLoginFailureCallback();
                TokenLoginFailureCallback = null;
            }
        }
    }

    private Action QuickLoginSucceedCallback = null;
    public void QuickLogin(Action LoginSucceed)
    {
        GameApp.SendMsg.StartWaitUI();
        GameApp.Instance.WaitLoadHintDlg.OpenHintBox("快速登录...");

        HttpLinkGet(GetPlatformURL(EPlatformFunType.eQuickLogin));

        QuickLoginSucceedCallback = LoginSucceed;

        StartCoroutine("WaitToLogin");
    }

    public void PhoneNumLoginUseVerificationCode(string PhoneNum, string VerificationCode)
    {
        GameApp.SendMsg.StartWaitUI();
        GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在使用手机号登录...");

        HttpLinkGet(StringBuilderTool.AppendFormat(GetPlatformURL(EPlatformFunType.ePhoneNumLoginUseVerificationCode), PhoneNum, VerificationCode));
        StartCoroutine("WaitToLogin");
    }

    public void LeSDKLogin(string AccessToken)
    {
        GameApp.SendMsg.StartWaitUI();
        GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在登录游戏平台...");

        HttpLinkGet(StringBuilderTool.AppendFormat(GetPlatformURL(EPlatformFunType.eLeSDKLogin), AccessToken));
        StartCoroutine("WaitToLogin");
    }

    public void WeChatLogin(string unionid, string openid,  string accessToken)
    {
        GameApp.SendMsg.StartWaitUI();
        GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在登录游戏平台...");

        HttpLinkGet(StringBuilderTool.AppendFormat(GetPlatformURL(EPlatformFunType.eWeChatLogin), unionid, openid, accessToken));
        StartCoroutine("WaitToLogin");
    }

    IEnumerator WaitToLogin()
    {
        while (curHttpResult == HttpResult.ConnectContinue)
        {
            yield return new WaitForEndOfFrame();
        }

        if (curHttpResult == HttpResult.ConnectException)
        {
            GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString("连接异常！\nWaitToLogin\n", ConnectExceptionInfo));
            Clear();
            yield break;
        }
        else
        {
            LitJson.JsonData jsonRes = LitJson.JsonMapper.ToObject(platformRetStrInfo);
            LitJson.JsonData josnData = jsonRes["data"];
            if (jsonRes["status"].ToString() == "0")
            {
                /*{
                    "message":"SUCCESS",
                    "data":
                    {
                        "yqStatus":0,
                        "accountId":12561,
                        "accessToken":"46794704657f7178160d4ac88dcad756bba1ad3f87509375",
                        "shareStatus":1,
                        "desktop":1,
                        "version":0,
                        "areaRoleProps":
                        {
                            "gameAreaVoList":[
                            {
                                "clientEnterAddr":
                                    [
                                        {"port":21001,"serverName":"logic","connectType":"DATABASE","ipAddress":"58.246.123.230"},
                                        {"port":20000,"serverName":"center","connectType":"DATABASE","ipAddress":"58.246.123.230"}
                                    ],
                                "maintenanceDesc":"",
                                "areaName":"魔卡一区",
                                "areaCode":"3001",
                                "areaTag":1,
                                "areaDesc":"",
                                "gameId":1005,
                                "loadStatus":1,
                                "displayOrder":1,
                                "availableTime":"2018-08-22 14:21:23",
                                "maintenanceStatus":1
                            }]
                        },
                        "chargeMoney":0,
                        "userId":12551,
                        "gameVersionInfos":"{\"appNeedUpdate\":false,\"appVerCode\":0,\"resourceNeedUpdate\":false,\"resourceUpdateConfigs\":[],\"resourceVerCode\":0}",
                        "giftId":0,
                        "newUser":0,
                        "gameId":1005,
                        "privatePolicy":"https://files1-1256951520.cos.ap-shanghai.myqcloud.com/champion-wx/fiies/private_policy.txt",
                        "refreshToken":"NDY3OTQ3MDQ2NTdmNzE3ODE2MGQ0YWM4OGRjYWQ3NTZiYmExYWQzZjg3NTA5Mzc1",
                        "worldAnnounce":[
                        {
                             "noticeLittleType":"1",
                             "noticeLittleTitle":"欢迎",
                             "noticeLittleText":"欢迎来到游戏"
                             }],
                        "accountName":"248140813",
                        "alipay":0,
                        "siteId":213,
                        "accountType":11,
                        "userAgree":"https://files1-1256951520.cos.ap-shanghai.myqcloud.com/champion-wx/fiies/user_agree.txt",
                        "wechat":0
                    },
                    "status":0
                }*/

                GameApp.Instance.CommonHintDlg.OpenHintBox("登陆平台成功！");

                int yqStatus = int.Parse(josnData["yqStatus"].ToString());
                int accountId = int.Parse(josnData["accountId"].ToString());
                PlatformInfo.accessToken = josnData["accessToken"].ToString();
                int shareStatus = int.Parse(josnData["shareStatus"].ToString());
                int desktop = int.Parse(josnData["desktop"].ToString());
                int version = int.Parse(josnData["version"].ToString());
                int chargeMoney = int.Parse(josnData["chargeMoney"].ToString());
                PlatformInfo.userId = int.Parse(josnData["userId"].ToString());
                int giftId = int.Parse(josnData["giftId"].ToString());
                int newUser = int.Parse(josnData["newUser"].ToString());
                PlatformInfo.gameId = int.Parse(josnData["gameId"].ToString());
                string privatePolicy = josnData["privatePolicy"].ToString();
                PlatformInfo.refreshToken = josnData["refreshToken"].ToString();
                string accountName = josnData["accountName"].ToString();
                int alipay = int.Parse(josnData["alipay"].ToString());
                int siteId = int.Parse(josnData["siteId"].ToString());
                int accountType = int.Parse(josnData["accountType"].ToString());
                string userAgree = josnData["userAgree"].ToString();
                int wechat = int.Parse(josnData["wechat"].ToString());

                Debug.Log(StringBuilderTool.ToString("yqStatus：", yqStatus));
                Debug.Log(StringBuilderTool.ToString("accountId：", accountId));
                Debug.Log(StringBuilderTool.ToInfoString("accessToken：", PlatformInfo.accessToken));
                Debug.Log(StringBuilderTool.ToString("shareStatus：", shareStatus));
                Debug.Log(StringBuilderTool.ToString("desktop：", desktop));
                Debug.Log(StringBuilderTool.ToString("version：", version));
                Debug.Log(StringBuilderTool.ToString("chargeMoney：", chargeMoney));
                Debug.Log(StringBuilderTool.ToString("userId：", PlatformInfo.userId));
                Debug.Log(StringBuilderTool.ToString("giftId：", giftId));
                Debug.Log(StringBuilderTool.ToString("newUser：", newUser));
                Debug.Log(StringBuilderTool.ToString("gameId：", PlatformInfo.gameId));
                Debug.Log(StringBuilderTool.ToInfoString("privatePolicy：", privatePolicy));
                Debug.Log(StringBuilderTool.ToInfoString("refreshToken：", PlatformInfo.refreshToken));
                Debug.Log(StringBuilderTool.ToInfoString("accountName：", accountName));
                Debug.Log(StringBuilderTool.ToString("alipay：", alipay));
                Debug.Log(StringBuilderTool.ToString("siteId：", siteId));
                Debug.Log(StringBuilderTool.ToString("accountType：", accountType));
                Debug.Log(StringBuilderTool.ToInfoString("userAgree：", userAgree));
                Debug.Log(StringBuilderTool.ToString("wechat：", wechat));

                LitJson.JsonData areaData = josnData["areaRoleProps"];
                LitJson.JsonData gameAreaData = areaData["gameAreaVoList"];
                if (gameAreaData.IsArray)
                {
                    for (int i = 0; i < gameAreaData.Count; i++)
                    {
                        LitJson.JsonData AddrData = gameAreaData[i]["clientEnterAddr"];
                        if (AddrData.IsArray)
                        {
                            Debug.LogWarning(StringBuilderTool.ToString("CCCC：", AddrData.Count));
                            for (int j = 0; j < AddrData.Count; j++)
                            {
                                string ipAddress = AddrData[j]["ipAddress"].ToString();
                                int port = int.Parse(AddrData[j]["port"].ToString());
                                string serverName = AddrData[j]["serverName"].ToString();
                                string connectType = AddrData[j]["connectType"].ToString();
                                Debug.Log(StringBuilderTool.ToInfoString("ipAddress：", ipAddress));
                                Debug.Log(StringBuilderTool.ToString("port：", port));
                                Debug.Log(StringBuilderTool.ToInfoString("serverName：", serverName));
                                Debug.Log(StringBuilderTool.ToInfoString("connectType：", connectType));

                                /*if(serverName == "center")
                                {
                                    Const.GameServerIP = ipAddress;
                                    Const.GameServerPort = port;
                                }*/
                            }
                        }
                        string maintenanceDesc = gameAreaData[i]["maintenanceDesc"].ToString();
                        string areaName = gameAreaData[i]["areaName"].ToString();
                        int areaCode = int.Parse(gameAreaData[i]["areaCode"].ToString());
                        int areaTag = int.Parse(gameAreaData[i]["areaTag"].ToString());
                        string areaDesc = gameAreaData[i]["areaDesc"].ToString();
                        int gameId4gameArea = int.Parse(gameAreaData[i]["gameId"].ToString());
                        int loadStatus = int.Parse(gameAreaData[i]["loadStatus"].ToString());
                        int displayOrder = int.Parse(gameAreaData[i]["displayOrder"].ToString());
                        string availableTime = gameAreaData[i]["availableTime"].ToString();
                        int maintenanceStatus = int.Parse(gameAreaData[i]["maintenanceStatus"].ToString());
                        Debug.Log(StringBuilderTool.ToInfoString("maintenanceDesc：", maintenanceDesc));
                        Debug.Log(StringBuilderTool.ToInfoString("areaName：", areaName));
                        Debug.Log(StringBuilderTool.ToString("areaCode：", areaCode));
                        Debug.Log(StringBuilderTool.ToString("areaTag：", areaTag));
                        Debug.Log(StringBuilderTool.ToInfoString("areaDesc：", areaDesc));
                        Debug.Log(StringBuilderTool.ToString("gameId：", gameId4gameArea));
                        Debug.Log(StringBuilderTool.ToString("loadStatus：", loadStatus));
                        Debug.Log(StringBuilderTool.ToString("displayOrder：", displayOrder));
                        Debug.Log(StringBuilderTool.ToInfoString("availableTime：", availableTime));
                        Debug.Log(StringBuilderTool.ToString("maintenanceStatus：", maintenanceStatus));
                    }
                }

                Dictionary<string, object> verDic = Json.Deserialize(josnData["gameVersionInfos"].ToString()) as Dictionary<string, object>;
                bool appNeedUpdate = bool.Parse(verDic["appNeedUpdate"].ToString());
                int appVerCode = int.Parse(verDic["appVerCode"].ToString());
                bool resourceNeedUpdate = bool.Parse(verDic["resourceNeedUpdate"].ToString());
                //resourceUpdateConfigs
                int resourceVerCode = int.Parse(verDic["resourceVerCode"].ToString());
                Debug.Log(StringBuilderTool.ToString("appNeedUpdate：", appNeedUpdate));
                Debug.Log(StringBuilderTool.ToString("appVerCode：", appVerCode));
                Debug.Log(StringBuilderTool.ToString("resourceNeedUpdate：", resourceNeedUpdate));
                Debug.Log(StringBuilderTool.ToString("resourceVerCode：", resourceVerCode));

                LitJson.JsonData noticeData = josnData["worldAnnounce"];
                if (noticeData.IsArray)
                {
                    for (int i = 0; i < noticeData.Count; i++)
                    {
                        int noticeLittleType = int.Parse(noticeData[i]["noticeLittleType"].ToString());
                        string noticeLittleTitle = noticeData[i]["noticeLittleTitle"].ToString();
                        string noticeLittleText = noticeData[i]["noticeLittleText"].ToString();
                        Debug.Log(StringBuilderTool.ToString("noticeLittleType：", noticeLittleType));
                        Debug.Log(StringBuilderTool.ToInfoString("noticeLittleTitle：", noticeLittleTitle));
                        Debug.Log(StringBuilderTool.ToInfoString("noticeLittleText：", noticeLittleText));
                    }
                }
                
                StartCoroutine("_TryGetRole");
            }
            else
            {
                int errorCode = int.Parse(josnData["errorCode"].ToString());
                string errorMessage = josnData["errorMessage"].ToString();
                GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(
                    StringBuilderTool.ToString("登陆平台失败！\n错误码：", errorCode, "\n错误描述：", errorMessage));
                
                if (TokenLoginFailureCallback != null)
                {
                    TokenLoginFailureCallback();
                    TokenLoginFailureCallback = null;
                }
            }
        }

        GameApp.Instance.WaitLoadHintDlg.CloseHintBox();

        Clear();
    }

    IEnumerator _TryGetRole()
    {
        GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在获取玩家账号信息...");
        while (httpThread != null || curHttpResult != HttpResult.ConnectNone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        GetRole();
    }
    void GetRole()
    {
        GameApp.SendMsg.StartWaitUI();

        HttpLinkGet(GetPlatformURL(EPlatformFunType.eGetRole));
        StartCoroutine("WaitToGetRole");
    }
    IEnumerator WaitToGetRole()
    {
        while (curHttpResult == HttpResult.ConnectContinue)
        {
            yield return new WaitForEndOfFrame();
        }

        if (curHttpResult == HttpResult.ConnectException)
        {
            GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString("连接异常！\nWaitToGetRole\n", ConnectExceptionInfo));
            Clear();
            yield break;
        }
        else
        {
            LitJson.JsonData jsonRes = LitJson.JsonMapper.ToObject(platformRetStrInfo);
            LitJson.JsonData josnData = jsonRes["data"];
            if (jsonRes["status"].ToString() == "0")
            {
                /*{
                    "data":
                    {
                        "areaCode":3001,
                        "areaId":152,
                        "createTime":1543980341683,
                        "gameId":1005,
                        "id":7901,
                        "lastLoginTime":1543980341683,
                        "roleId":30010231470792,
                        "roleLevel":0,
                        "roleName":"",
                        "siteId":213,
                        "totalPayAmount":0,
                        "totalPayTimes":0,
                        "userId":12551,
                        "validStatus":"VALID"
                    },
                    "message":"SUCCESS",
                    "status":0
                }*/

                GameApp.Instance.CommonHintDlg.OpenHintBox("获取玩家账号信息成功！");

                int areaCode = int.Parse(josnData["areaCode"].ToString());
                int areaId = int.Parse(josnData["areaId"].ToString());
                int gameId = int.Parse(josnData["gameId"].ToString());
                int siteId = int.Parse(josnData["siteId"].ToString());
                DateTime createTime = getTime(long.Parse(josnData["createTime"].ToString()));
                DateTime lastLoginTime = getTime(long.Parse(josnData["lastLoginTime"].ToString()));
                int id = int.Parse(josnData["id"].ToString());
                PlatformInfo.roleId = long.Parse(josnData["roleId"].ToString());
                GameApp.AccountID = (uint)PlatformInfo.roleId;
                int roleLevel = int.Parse(josnData["roleLevel"].ToString());
                string roleName = josnData["roleName"].ToString();
                int totalPayAmount = int.Parse(josnData["totalPayAmount"].ToString());
                int totalPayTimes = int.Parse(josnData["totalPayTimes"].ToString());
                int userId = int.Parse(josnData["userId"].ToString());
                string validStatus = josnData["validStatus"].ToString();

                Debug.Log(StringBuilderTool.ToString("areaCode：", areaCode));
                Debug.Log(StringBuilderTool.ToString("areaId：", areaId));
                Debug.Log(StringBuilderTool.ToString("gameId：", gameId));
                Debug.Log(StringBuilderTool.ToString("siteId：", siteId));
                Debug.Log(StringBuilderTool.ToInfoString("createTime：", createTime.ToString()));
                Debug.Log(StringBuilderTool.ToInfoString("lastLoginTime：", lastLoginTime.ToString()));
                Debug.Log(StringBuilderTool.ToString("id：", id));
                Debug.Log(StringBuilderTool.ToString("roleId：", PlatformInfo.roleId));
                Debug.Log(StringBuilderTool.ToString("roleLevel：", roleLevel));
                Debug.Log(StringBuilderTool.ToInfoString("roleName：", roleName));
                Debug.Log(StringBuilderTool.ToString("totalPayAmount：", totalPayAmount));
                Debug.Log(StringBuilderTool.ToString("totalPayTimes：", totalPayTimes));
                Debug.Log(StringBuilderTool.ToString("userId：", userId));
                Debug.Log(StringBuilderTool.ToInfoString("validStatus：", validStatus));

                if (TokenLoginSucceedCallback != null)
                {
                    TokenLoginSucceedCallback();
                    TokenLoginSucceedCallback = null;
                }

                if (QuickLoginSucceedCallback != null)
                {
                    QuickLoginSucceedCallback();
                    QuickLoginSucceedCallback = null;
                }
                
                GameApp.Instance.UILogin.ShowBeginGame();
            }
            else
            {
                int errorCode = int.Parse(josnData["errorCode"].ToString());
                string errorMessage = josnData["errorMessage"].ToString();
                GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(
                    StringBuilderTool.ToString("获取玩家账号信息失败！\n错误码：", errorCode, "\n错误描述：", errorMessage));
                
                if (TokenLoginFailureCallback != null)
                {
                    TokenLoginFailureCallback();
                    TokenLoginFailureCallback = null;
                }
            }
        }

        GameApp.Instance.WaitLoadHintDlg.CloseHintBox();

        Clear();
    }

    public void CheckRoleName(string Name)
    {
        if (PlatformInfo.userId == 0)
            return;

        GameApp.SendMsg.StartWaitUI();

        HttpLinkGet(StringBuilderTool.AppendFormat(GetPlatformURL(EPlatformFunType.eCheckRoleName), Name));
        StartCoroutine("WaitToCheckRoleName");
    }
    IEnumerator WaitToCheckRoleName()
    {
        while (curHttpResult == HttpResult.ConnectContinue)
        {
            yield return new WaitForEndOfFrame();
        }

        if (curHttpResult == HttpResult.ConnectException)
        {
            GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString("连接异常！\nWaitToCheckRoleName\n", ConnectExceptionInfo));
            Clear();
            yield break;
        }
        else
        {
            LitJson.JsonData jsonRes = LitJson.JsonMapper.ToObject(platformRetStrInfo);
            if (jsonRes["status"].ToString() == "0")
            {
                //GameApp.Instance.CommonHintDlg.OpenHintBox("角色名可用！");
            }
            else
            {
                //GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox("角色名不可用！");
            }
        }

        Clear();
    }

    public void GetGoodsList()
    {
        GameApp.SendMsg.StartWaitUI();

        GoodsDic.Clear();
        IsGetGetGoodsListOver = false;

        HttpLinkGet(GetPlatformURL(EPlatformFunType.eGetGoodsList));
        StartCoroutine("WaitToGetGoodsList");
    }
    IEnumerator WaitToGetGoodsList()
    {
        while (curHttpResult == HttpResult.ConnectContinue)
        {
            yield return new WaitForEndOfFrame();
        }

        if (curHttpResult == HttpResult.ConnectException)
        {
            GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString("连接异常！\nWaitToGetGoodsList\n", ConnectExceptionInfo));
            Clear();
            yield break;
        }
        else
        {
            LitJson.JsonData jsonRes = LitJson.JsonMapper.ToObject(platformRetStrInfo);
            LitJson.JsonData josnData = jsonRes["data"];
            if (jsonRes["status"].ToString() == "0")
            {
                /*{
                    "data":[
                    {
                        "chargeExtra":0,
                        "firstPay":0,
                        "gameId":1005,
                        "goodsAmount":1,
                        "goodsCode":"com.mokagame.gem1000",
                        "goodsCount":99,
                        "goodsDesc":"包含99颗钻石",
                        "goodsId":35,
                        "goodsMarkTime":1543128786,
                        "goodsName":"一小袋钻石",
                        "goodsPic":"item_1",
                        "goodsShowTime":1574146389,
                        "position":"",
                        "relatedGoodsId":"1000",
                        "shopType":1,
                        "showAmount":1
                    },
                    {
                        "chargeExtra":0,
                        "firstPay":0,
                        "gameId":1005,
                        "goodsAmount":1,
                        "goodsCode":"com.mokagame.gem1003",
                        "goodsCount":500,
                        "goodsDesc":"包含500颗糖果",
                        "goodsId":36,
                        "goodsMarkTime":1543128786,
                        "goodsName":"一大包糖果",
                        "goodsPic":"item_1003",
                        "goodsShowTime":1574146389,
                        "position":"",
                        "relatedGoodsId":"1003",
                        "shopType":1,
                        "showAmount":1
                    },
                    {
                        "chargeExtra":0,
                        "firstPay":0,
                        "gameId":1005,
                        "goodsAmount":1,
                        "goodsCode":"com.mokagame.gem40024",
                        "goodsCount":1,
                        "goodsDesc":"写着看不懂的文字和图画",
                        "goodsId":37,
                        "goodsMarkTime":1543128786,
                        "goodsName":"藏宝图",
                        "goodsPic":"item_40024",
                        "goodsShowTime":1574146389,
                        "position":"",
                        "relatedGoodsId":"40024",
                        "shopType":1,
                        "showAmount":1
                    },
                    {
                        "chargeExtra":0,
                        "firstPay":0,
                        "gameId":1005,
                        "goodsAmount":1,
                        "goodsCode":"com.mokagame.gem2001",
                        "goodsCount":1,
                        "goodsDesc":"月卡",
                        "goodsId":38,
                        "goodsMarkTime":1543128786,
                        "goodsName":"月卡",
                        "goodsPic":"item_2001",
                        "goodsShowTime":1574146389,
                        "position":"",
                        "relatedGoodsId":"2001",
                        "shopType":1,
                        "showAmount":1
                    },
                    {
                        "chargeExtra":0,
                        "firstPay":0,
                        "gameId":1005,
                        "goodsAmount":1,
                        "goodsCode":"com.mokagame.gem2002",
                        "goodsCount":1,
                        "goodsDesc":"季卡",
                        "goodsId":39,
                        "goodsMarkTime":1543128786,
                        "goodsName":"季卡",
                        "goodsPic":"item_2002",
                        "goodsShowTime":1574146389,
                        "position":"",
                        "relatedGoodsId":"2002",
                        "shopType":1,
                        "showAmount":1
                    },
                    {
                        "chargeExtra":0,
                        "firstPay":0,
                        "gameId":1005,
                        "goodsAmount":1,
                        "goodsCode":"com.mokagame.gem2003",
                        "goodsCount":1,
                        "goodsDesc":"年卡",
                        "goodsId":40,
                        "goodsMarkTime":1543128786,
                        "goodsName":"年卡",
                        "goodsPic":"item_2003",
                        "goodsShowTime":1574146389,
                        "position":"",
                        "relatedGoodsId":"2003",
                        "shopType":1,
                        "showAmount":1
                    }],
                    "message":"SUCCESS",
                    "status":0
                }*/

                GameApp.Instance.CommonHintDlg.OpenHintBox("获取商品列表成功！");

                if (josnData.IsArray)
                {
                    for (int i = 0; i < josnData.Count; i++)
                    {
                        int chargeExtra = int.Parse(josnData[i]["chargeExtra"].ToString());
                        int firstPay = int.Parse(josnData[i]["firstPay"].ToString());
                        int gameId = int.Parse(josnData[i]["gameId"].ToString());
                        int goodsAmount = int.Parse(josnData[i]["goodsAmount"].ToString());
                        int goodsCount = int.Parse(josnData[i]["goodsCount"].ToString());
                        int goodsId = int.Parse(josnData[i]["goodsId"].ToString());
                        int relatedGoodsId = int.Parse(josnData[i]["relatedGoodsId"].ToString());
                        int shopType = int.Parse(josnData[i]["shopType"].ToString());
                        int showAmount = int.Parse(josnData[i]["showAmount"].ToString());
                        DateTime goodsMarkTime = getTime(long.Parse(josnData[i]["goodsMarkTime"].ToString()) * 1000);
                        DateTime goodsShowTime = getTime(long.Parse(josnData[i]["goodsShowTime"].ToString()) * 1000);
                        string goodsCode = josnData[i]["goodsCode"].ToString();
                        string goodsDesc = josnData[i]["goodsDesc"].ToString();
                        string goodsName = josnData[i]["goodsName"].ToString();
                        string goodsPic = josnData[i]["goodsPic"].ToString();
                        string position = josnData[i]["position"].ToString();

                        GoodsDic.Add(goodsId, new GoodsInfo(goodsId, goodsCode, relatedGoodsId, goodsAmount, goodsCount, goodsName, goodsDesc, goodsPic));

                        Debug.Log(StringBuilderTool.ToString("chargeExtra：", chargeExtra));
                        Debug.Log(StringBuilderTool.ToString("firstPay：", firstPay));
                        Debug.Log(StringBuilderTool.ToString("gameId：", gameId));
                        Debug.Log(StringBuilderTool.ToString("goodsAmount：", goodsAmount));
                        Debug.Log(StringBuilderTool.ToString("goodsCount：", goodsCount));
                        Debug.Log(StringBuilderTool.ToString("goodsId：", goodsId));
                        Debug.Log(StringBuilderTool.ToString("relatedGoodsId：", relatedGoodsId));
                        Debug.Log(StringBuilderTool.ToString("shopType：", shopType));
                        Debug.Log(StringBuilderTool.ToString("showAmount：", showAmount));
                        Debug.Log(StringBuilderTool.ToInfoString("goodsMarkTime：", goodsMarkTime.ToString()));
                        Debug.Log(StringBuilderTool.ToInfoString("goodsShowTime：", goodsShowTime.ToString()));
                        Debug.Log(StringBuilderTool.ToInfoString("goodsCode：", goodsCode));
                        Debug.Log(StringBuilderTool.ToInfoString("goodsDesc：", goodsDesc));
                        Debug.Log(StringBuilderTool.ToInfoString("goodsName：", goodsName));
                        Debug.Log(StringBuilderTool.ToInfoString("goodsPic：", goodsPic));
                        Debug.Log(StringBuilderTool.ToInfoString("position：", position));
                    }
                }
                IsGetGetGoodsListOver = true;
            }
            else
            {
                int errorCode = int.Parse(josnData["errorCode"].ToString());
                string errorMessage = josnData["errorMessage"].ToString();
                GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(
                    StringBuilderTool.ToString("获取商品列表失败！\n错误码：", errorCode, "\n错误描述：", errorMessage));
            }
        }

        Clear();
    }

    GoodsInfo CurWaitBuyGoodsInfo = null;
    public void CreateOrder(int GoodsID)
    {
        if (GoodsDic.TryGetValue(GoodsID, out CurWaitBuyGoodsInfo))
        {
            GameApp.SendMsg.StartWaitUI();

            HttpLinkGet(StringBuilderTool.AppendFormat(GetPlatformURL(EPlatformFunType.eCreateOrder), CurWaitBuyGoodsInfo.Code, CurWaitBuyGoodsInfo.Name, CurWaitBuyGoodsInfo.Amount));
            StartCoroutine("WaitToCreateOrder");
        }
        else
        {
            GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox("未记录的商品！");
        }
    }
    IEnumerator WaitToCreateOrder()
    {
        while (curHttpResult == HttpResult.ConnectContinue)
        {
            yield return new WaitForEndOfFrame();
        }

        if (curHttpResult == HttpResult.ConnectException)
        {
            GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString("连接异常！\nWaitToCreateOrder\n", ConnectExceptionInfo));
            Clear();
            yield break;
        }
        else
        {
            LitJson.JsonData jsonRes = LitJson.JsonMapper.ToObject(platformRetStrInfo);
            LitJson.JsonData josnData = jsonRes["data"];
            if (jsonRes["status"].ToString() == "0")
            {
                /*{
                    "data":
                    {
                        "orderId":"201812948870"
                    },
                    "message":"SUCCESS",
                    "status":0
                }*/

                GameApp.Instance.CommonHintDlg.OpenHintBox("创建订单成功！");

                PlatformInfo.orderId = josnData["orderId"].ToString();
                Debug.Log(StringBuilderTool.ToInfoString("orderId：", PlatformInfo.orderId));

                if (GameApp.Instance.LeSDKInstance)
                    GameApp.Instance.LeSDKInstance.Pay(CurWaitBuyGoodsInfo);
            }
            else
            {
                int errorCode = int.Parse(josnData["errorCode"].ToString());
                string errorMessage = josnData["errorMessage"].ToString();
                GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(
                    StringBuilderTool.ToString("创建订单失败！\n错误码：", errorCode, "\n错误描述：", errorMessage));
            }
        }

        Clear();
    }

    public void Clear()
    {
        if (httpThread != null)
        {
            httpThread.Abort();
            httpThread = null;
        }
        curHttpResult = HttpResult.ConnectNone;
        platformRetStrInfo = null;

        GameApp.SendMsg.EndWaitUI();
    }

    DateTime getTime(long _time)
    {
        string timeStamp = _time.ToString();
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(StringBuilderTool.ToInfoString(timeStamp, "0000"));
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime dtResult = dtStart.Add(toNow);
        return dtResult;
    }
}
