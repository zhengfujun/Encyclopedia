using UnityEngine;
using UnityEngine.Video;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using MyMiniJSON;
using common;

public class UI_Login : MonoBehaviour
{
    public UILabel versionLabel;

    public UIAppearEffect AppearEffect;

    public GameObject LeSDKLoginRoot;
    public GameObject BeginGameRoot;
    public GameObject LoginTypeRoot;
    public GameObject MobilePhoneLoginRoot;
    public GameObject ResetPasswordRoot;
    public GameObject RegisterAccountRoot;
    public GameObject PasswordLoginRoot;

    public UILabel LastLoginAccount;

    public GameObject UserAgreementTextLstRoot;
    public UIScrollView UserAgreementSV;
    public GameObject PrivacyPolicyTextLstRoot;
    public UIScrollView PrivacyPolicySV;

    public UIInput MobilePhoneLogin_Inp_PhoneNum;
    public UIInput MobilePhoneLogin_Inp_VerificationCode;

    public UIInput ResetPassword_Inp_PhoneNum;
    public UIInput ResetPassword_Inp_VerificationCode;
    public UIInput ResetPassword_Inp_NewPassword;

    public UIInput RegisterAccount_Inp_PhoneNum;
    public UIInput RegisterAccount_Inp_VerificationCode;

    public UIInput PasswordLogin_Inp_PhoneNum;
    public UIInput PasswordLogin_Inp_Password;

    private string AccountLDKey = "MKBK_Login_Account";
    private string PasswordLDKey = "MKBK_Login_Password";

    [HideInInspector]
    public string Account = string.Empty;
    private string VerificCode = string.Empty;
    private string Password = string.Empty;

    public ShareSDKCtrl ShareCtrl;

    public UI_Role RoleUI;
    
    void Start()
    {
        GameApp.Instance.UILogin = this;

        versionLabel.text = "版本: " + RecordVersion.Read();

        GameApp.Instance.SoundInstance.PlayBgm("BGM_Login01");

        //
        if (PlayerPrefs.HasKey(AccountLDKey))
        {
            Account = PlayerPrefs.GetString(AccountLDKey);
        }
        if (PlayerPrefs.HasKey(PasswordLDKey))
        {
            Password = PlayerPrefs.GetString(PasswordLDKey);
        }

        MobilePhoneLogin_Inp_PhoneNum.value = Account;
        PasswordLogin_Inp_PhoneNum.value = Account;
        ResetPassword_Inp_PhoneNum.value = Account;
        PasswordLogin_Inp_Password.value = Password;

        //
        LeSDKLoginRoot.transform.localScale = Vector3.zero;
        BeginGameRoot.transform.localScale = Vector3.zero;
        LoginTypeRoot.transform.localScale = Vector3.zero;

        MobilePhoneLoginRoot.transform.localScale = Vector3.zero;
        MobilePhoneLoginRoot.GetComponent<UIWidget>().alpha = 0;

        ResetPasswordRoot.transform.localScale = Vector3.zero;
        ResetPasswordRoot.GetComponent<UIWidget>().alpha = 0;

        RegisterAccountRoot.transform.localScale = Vector3.zero;
        RegisterAccountRoot.GetComponent<UIWidget>().alpha = 0;

        PasswordLoginRoot.transform.localScale = Vector3.zero;
        PasswordLoginRoot.GetComponent<UIWidget>().alpha = 0;
    }

    void OnDestroy()
    {
        GameApp.Instance.UILogin = null;
    }

    /*void Update()
    {
    15145109279
     * 18202146541
    }*/

    public void ShowLeSDKLogin()
    {
        BeginGameRoot.transform.localScale = Vector3.zero;
        AppearEffect.Open(AppearType.Popup, LeSDKLoginRoot);
    }
    public void ShowBeginGame()
    {
        if (MobilePhoneLoginRoot.activeSelf ||
            RegisterAccountRoot.activeSelf)
        {
            AppearEffect.Close(AppearType.Popup);

            GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在进入游戏...");
            GameApp.SendMsg.CreateGameServerSocket();
        }
        else
        {
            LeSDKLoginRoot.transform.localScale = Vector3.zero;
            AppearEffect.Close(AppearType.Popup, () =>
                {
                    LastLoginAccount.text = StringBuilderTool.ToInfoString("欢迎您，", Account);
                    AppearEffect.Open(AppearType.Popup, BeginGameRoot);
                });
        }
    }
    public void ShowLoginType()
    {
       GameApp.Instance.Platform.TokenLogin(()=>
           {

           },
           ()=>
           {
               GameApp.Instance.Platform.PlatformInfo.refreshToken = "";
               AppearEffect.Open(AppearType.Popup, LoginTypeRoot);
           });
    }

    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【返回】");

        if(MobilePhoneLoginRoot.activeSelf)
        {
            AppearEffect.Close(AppearType.Popup, () =>
            {
                LoginTypeRoot.GetComponent<UIWidget>().alpha = 1;
                AppearEffect.Open(AppearType.Popup, LoginTypeRoot);
            });
        }
        else if (RegisterAccountRoot.activeSelf)
        {
            AppearEffect.Close(AppearType.Popup, () =>
            {
                LoginTypeRoot.GetComponent<UIWidget>().alpha = 1;
                AppearEffect.Open(AppearType.Popup, LoginTypeRoot);
            });
        }
        else if (PasswordLoginRoot.activeSelf)
        {
            AppearEffect.Close(AppearType.Popup, () =>
            {
                MobilePhoneLoginRoot.GetComponent<UIWidget>().alpha = 1;
                AppearEffect.Open(AppearType.Popup, MobilePhoneLoginRoot);
            });
        }
        else if (ResetPasswordRoot.activeSelf)
        {
            AppearEffect.Close(AppearType.Popup, () =>
            {
                PasswordLoginRoot.GetComponent<UIWidget>().alpha = 1;
                AppearEffect.Open(AppearType.Popup, PasswordLoginRoot);
            });
        }
    }
    public void OnClick_BeginGame()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【开始游戏】");

        GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在进入游戏...");
        GameApp.SendMsg.CreateGameServerSocket();
    }
    public void OnClick_SwitchAccount()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GameApp.Instance.LeSDKInstance)
            GameApp.Instance.LeSDKInstance.SwitchAccount();
#elif UNITY_IPHONE || UNITY_EDITOR
        AppearEffect.Close(AppearType.Popup, () =>
        {
            //BeginGameRoot.GetComponent<UIWidget>().alpha = 1;
            AppearEffect.Open(AppearType.Popup, LoginTypeRoot);
        });
#endif
    }

    #region _安卓版乐视登录
    public void LeSDKLogin()
    {
        if (GameApp.Instance.LeSDKInstance)
            GameApp.Instance.LeSDKInstance.Login();
    }
    public void OnClick_LeSDKLogin()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【乐视账号登录】");

        LeSDKLogin();
    }
    #endregion

    #region _游客登录
    public void OnClick_VisitorsLogin()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【游客登录】");
        
        GameApp.Instance.CurAccountType = AccountType.Official_Visitors;

        GameApp.Instance.Platform.QuickLogin(() =>
            {
                Account = "游客";
                PlayerPrefs.SetString(AccountLDKey, Account);
            });
    }
    #endregion

    #region _手机登录
    public void OnClick_MobilePhoneLogin()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【手机登录】");

        MobilePhoneLogin_Inp_PhoneNum.value = Account;

        AppearEffect.Close(AppearType.Popup, () =>
        {
            MobilePhoneLoginRoot.GetComponent<UIWidget>().alpha = 1;
            AppearEffect.Open(AppearType.Popup, MobilePhoneLoginRoot);            
        });
    }
    public void OnClick_MobilePhoneLogin_Login()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【手机登录 — 登录】");

        Account = MobilePhoneLogin_Inp_PhoneNum.value;
        VerificCode = MobilePhoneLogin_Inp_VerificationCode.value;
        
        if (IsMobilePhoneError(Account))
            return;

        if (IsVerificationCodeError(VerificCode))
            return;

        PlayerPrefs.SetString(AccountLDKey, Account);
        
        GameApp.Instance.CurAccountType = AccountType.Official_MobilePhone;

        if (Account == "13012345678")
        {
            GameApp.AccountID = 12345678;

            ShowBeginGame();
            return;
        }

        GameApp.Instance.Platform.PhoneNumLoginUseVerificationCode(Account, VerificCode);
    }
    public void OnClick_MobilePhoneLogin_SendVerificationCode(UILabel CDLab)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【手机登录 — 发送验证码】");

        Account = MobilePhoneLogin_Inp_PhoneNum.value;

        if (IsMobilePhoneError(Account))
            return;

        CurCDLab = CDLab;
        GameApp.Instance.Platform.SendSmsToUser(Account, () =>
            {
                StartCoroutine("SetPhoneVerifyValidityLimit");
            });
        /*if (GetPhoneVerify(Account))
        {
            StartCoroutine("SetPhoneVerifyValidityLimit", CDLab);
        }*/
    }
    public void OnClick_MobilePhoneLogin_RegisterAccount()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【手机登录 — 注册账号】");

        AppearEffect.Close(AppearType.Popup, () =>
        {
            RegisterAccountRoot.GetComponent<UIWidget>().alpha = 1;
            AppearEffect.Open(AppearType.Popup, RegisterAccountRoot);
        });
    }
    public void OnClick_MobilePhoneLogin_PasswordLogin()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【手机登录 — 密码登录】");

        AppearEffect.Close(AppearType.Popup, () =>
        {
            PasswordLoginRoot.GetComponent<UIWidget>().alpha = 1;
            AppearEffect.Open(AppearType.Popup, PasswordLoginRoot);
        });
    }
    public void OnClick_MobilePhoneLogin_UserAgreement()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【用户协议】");

        UserAgreementTextLstRoot.SetActive(true);
        TweenAlpha.Begin(UserAgreementTextLstRoot,0.1f,1);
        UserAgreementSV.ResetPosition();
        //Application.OpenURL("http://www.immomo.com/agreement.html");
    }
    public void OnClick_Close_UserAgreement()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭 — 用户协议】");

        TweenAlpha.Begin(UserAgreementTextLstRoot, 0.1f, 0).onFinished.Add(new EventDelegate(() =>
            {
                UserAgreementTextLstRoot.SetActive(false);
            }));
    }

    public void OnClick_MobilePhoneLogin_PrivacyPolicy()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【隐私政策】");

        PrivacyPolicyTextLstRoot.SetActive(true);
        TweenAlpha.Begin(PrivacyPolicyTextLstRoot, 0.1f, 1);
        PrivacyPolicySV.ResetPosition();
        //Application.OpenURL("https://privacy.qq.com/");
    }
    public void OnClick_Close_PrivacyPolicy()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭 — 隐私政策】");

        TweenAlpha.Begin(PrivacyPolicyTextLstRoot, 0.1f, 0).onFinished.Add(new EventDelegate(() =>
        {
            PrivacyPolicyTextLstRoot.SetActive(false);
        })); ;
    }
    #endregion

    #region _注册账号
    public void OnClick_RegisterAccount()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【注册账号】");

        AppearEffect.Close(AppearType.Popup, () =>
        {
            RegisterAccountRoot.GetComponent<UIWidget>().alpha = 1;
            AppearEffect.Open(AppearType.Popup, RegisterAccountRoot);
        });
    }
    public void OnClick_RegisterAccount_CreateAccount()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【注册账号 — 创建账号】");

        Account = RegisterAccount_Inp_PhoneNum.value;
        VerificCode = RegisterAccount_Inp_VerificationCode.value;

        if (IsMobilePhoneError(Account))
            return;

        if (IsVerificationCodeError(VerificCode))
            return;

        PlayerPrefs.SetString(AccountLDKey, Account);

        GameApp.Instance.Platform.RegisterPhoneNumAccount(Account, VerificCode);
    }
    public void OnClick_RegisterAccount_LoginAlreadyHaveAccount()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【注册账号 — 登录已有账号】");

        AppearEffect.Close(AppearType.Popup, () =>
        {
            MobilePhoneLoginRoot.GetComponent<UIWidget>().alpha = 1;
            AppearEffect.Open(AppearType.Popup, MobilePhoneLoginRoot);
        });
    }
    public void OnClick_RegisterAccount_SendVerificationCode(UILabel CDLab)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【注册账号 — 发送验证码】");

        Account = RegisterAccount_Inp_PhoneNum.value;

        if (IsMobilePhoneError(Account))
            return;

        CurCDLab = CDLab;
        GameApp.Instance.Platform.SendSmsToUser(Account, () =>
        {
            StartCoroutine("SetPhoneVerifyValidityLimit");
        });
        /*if (GetPhoneVerify(Account))
        {
            StartCoroutine("SetPhoneVerifyValidityLimit", CDLab);
        }*/
    }
    #endregion

    #region _密码登录
    public void OnClick_PasswordLogin_Login()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【密码登录 — 登录】");

        Account = PasswordLogin_Inp_PhoneNum.value;
        Password = PasswordLogin_Inp_Password.value;

        GameApp.AccountID = uint.Parse(Account.Substring(0, 9));

        if (IsMobilePhoneError(Account))
            return;

        if (IsPasswordError(Password))
            return;

        PlayerPrefs.SetString(AccountLDKey, Account);
        PlayerPrefs.SetString(PasswordLDKey, Password);
        
        GameApp.Instance.CurAccountType = AccountType.Official_MobilePhone;

    }
    public void OnClick_PasswordLogin_ForgetPassword()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【密码登录 — 忘记密码】");

        AppearEffect.Close(AppearType.Popup, () =>
        {
            ResetPasswordRoot.GetComponent<UIWidget>().alpha = 1;
            AppearEffect.Open(AppearType.Popup, ResetPasswordRoot);
        });
    }
    public void OnClick_PasswordLogin_VerificationCodeLogin()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【密码登录 — 验证码登录】");

        AppearEffect.Close(AppearType.Popup, () =>
        {
            MobilePhoneLoginRoot.GetComponent<UIWidget>().alpha = 1;
            AppearEffect.Open(AppearType.Popup, MobilePhoneLoginRoot);
        });
    }
    #endregion

    #region _重设密码
    public void OnClick_ResetPassword_Login()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【重设密码 — 登录】");

        Account = ResetPassword_Inp_PhoneNum.value;
        VerificCode = ResetPassword_Inp_VerificationCode.value;
        Password = ResetPassword_Inp_NewPassword.value;

        if (IsMobilePhoneError(Account))
            return;

        if (IsVerificationCodeError(VerificCode))
            return;

        if (IsPasswordError(Password))
            return;

        PlayerPrefs.SetString(AccountLDKey, Account);
        PlayerPrefs.SetString(PasswordLDKey, Password);

        GameApp.Instance.SceneCtlInstance.ChangeScene(SceneControl.HomePage);
    }
    public void OnClick_ResetPassword_SendVerificationCode(UILabel CDLab)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【重设密码 — 发送验证码】");

        Account = ResetPassword_Inp_PhoneNum.value;

        if (IsMobilePhoneError(Account))
            return;

        CurCDLab = CDLab;
        GameApp.Instance.Platform.SendSmsToUser(Account, () =>
        {
            StartCoroutine("SetPhoneVerifyValidityLimit");
        });
        /*if (GetPhoneVerify(Account))
        {
            StartCoroutine("SetPhoneVerifyValidityLimit", CDLab);
        }*/
    }
    #endregion

    #region _第三方快捷登录
    public void OnClick_WeChatLogin()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【微信登录】");

        //GameApp.Instance.CommonHintDlg.OpenHintBox("", EHintBoxStyle.eStyle_ComingSoon);

        StartCoroutine("_WeChatLoginFailure");
        GameApp.SendMsg.StartWaitUI();
        ShareCtrl.WeChatLogin(
            (openid, unionid, accessToken, nickname) =>
            {
                IsWeChatLoginFailure = false;

                Account = nickname;
                PlayerPrefs.SetString(AccountLDKey, Account);
                //VerificCode = "123456";
                /*Debug.Log("openid：" + openid);
                Debug.Log("unionid：" + unionid);
                Debug.Log("accessToken：" + accessToken);
                Debug.Log("nickname：" + nickname);*/

                RecordNickName = nickname;

                GameApp.Instance.CurAccountType = AccountType.Official_WeChat;

                GameApp.Instance.Platform.WeChatLogin(unionid, openid, accessToken);

                GameApp.SendMsg.EndWaitUI();
            },
            (errorMsg) =>
            {
                StopCoroutine("_WeChatLoginFailure");
                GameApp.Instance.CommonMsgDlg.OpenMsgBox("微信登录失败，\n" + errorMsg);
            });
    }
    bool IsWeChatLoginFailure = false;
    IEnumerator _WeChatLoginFailure()
    {
        IsWeChatLoginFailure = true;
        yield return new WaitForSeconds(4.5f);
        if(IsWeChatLoginFailure)
        {
            GameApp.Instance.CommonMsgDlg.OpenMsgBox("微信登录失败，等待回执超时！");
        }
    }
    public void OnClick_QQLogin()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【QQ登录】");

        GameApp.Instance.CommonHintDlg.OpenHintBox("", EHintBoxStyle.eStyle_ComingSoon);
        return;

        StartCoroutine("_QQLoginFailure");
        GameApp.SendMsg.StartWaitUI();
        ShareCtrl.QQLogin(
            (userID, nickname) =>
            {
                IsQQLoginFailure = false;

                Account = userID;
                VerificCode = "123456";

                RecordNickName = nickname;

                GameApp.Instance.CurAccountType = AccountType.Official_QQ;

                

                GameApp.SendMsg.EndWaitUI();
            },
            (errorMsg) =>
            {
                StopCoroutine("_QQLoginFailure");
                GameApp.Instance.CommonMsgDlg.OpenMsgBox("QQ登录失败，\n" + errorMsg);
            });
    }
    bool IsQQLoginFailure = false;
    IEnumerator _QQLoginFailure()
    {
        IsQQLoginFailure = true;
        yield return new WaitForSeconds(4.5f);
        if (IsQQLoginFailure)
        {
            GameApp.Instance.CommonMsgDlg.OpenMsgBox("QQ登录失败，等待回执超时！");
        }
    }

    public void OnClick_SinaMicroBlogLogin()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【微博登录】");

        GameApp.Instance.CommonHintDlg.OpenHintBox("", EHintBoxStyle.eStyle_ComingSoon);
    }

    [HideInInspector]
    public string RecordNickName = string.Empty;
    public string GetNickName()
    {
        if(RecordNickName.Length > 0)
        {
            return RecordNickName;
        }
        else
        {
            return "玩家昵称七个字";
        }
    }
    #endregion

    #region _工具型函数
    //检测手机号
    public bool IsMobilePhoneError(string PhoneNumStr)
    {
        if (PhoneNumStr.Length == 0)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("请输入手机号码！");
            return true;
        }
        if (!IsMobilePhone(PhoneNumStr))
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("请输入有效的手机号码！（以1开头的11位数字）");
            return true;
        }

        return false;
    }
    //检测验证码
    public bool IsVerificationCodeError(string VerificationCodeStr)
    {
        if (VerificationCodeStr.Length == 0)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("请输入验证码！");
            return true;
        }
        if (!IsVerificationCode(VerificationCodeStr))
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("请输入有效的验证码！（5位数字）");
            return true;
        }

        return false;
    }
    //检测密码
    public bool IsPasswordError(string PasswordStr)
    {
        if (PasswordStr.Length == 0)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("请输入密码！");
            return true;
        }
        if (!IsPassword(PasswordStr))
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("请输入有效的密码！（1～6位数字）");
            return true;
        }

        return false;
    }
    //判断输入的字符串是否是一个合法的手机号
    public bool IsMobilePhone(string input)
    {
        Regex regex = new Regex("^1\\d{10}$");
        return regex.IsMatch(input);
    }
    //判断输入的字符串是否是一个合法的验证码
    public bool IsVerificationCode(string input)
    {
        Regex regex = new Regex("^\\d{5}$");
        return regex.IsMatch(input);
    }
    //判断输入的字符串是否是一个合法的密码
    public bool IsPassword(string input)
    {
        Regex regex = new Regex("^\\d{1,6}$");
        return regex.IsMatch(input);
    }
    //获取时间戳
    private long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }
    //设置验证码倒计时
    private UILabel CurCDLab = null;
    IEnumerator SetPhoneVerifyValidityLimit()
    {
        if (CurCDLab == null)
            yield break;

        UIButton but = CurCDLab.transform.parent.GetComponent<UIButton>();
        but.isEnabled = false;

        int cd = 60;
        while (cd > 0)
        {
            CurCDLab.text = StringBuilderTool.ToString(cd, "秒后重新获取");
            yield return new WaitForSeconds(1);
            cd--;
        }

        but.isEnabled = true;
        CurCDLab.text = "发送验证码";
    }
    #endregion

    /*#region _测试用的获取手机短信验证码
    private string UrlRoot = "http://openapi.gdalpha.com:9998/user/";
    private string appid = "BrY7T0Y6HMjI1ePU";
    private string appkey = "YlU4VETeABoJohi8";
    private bool GetPhoneVerify(string PhoneNum)
    {
        Dictionary<string, object> dataDic = new Dictionary<string, object>();
        dataDic.Add("account", PhoneNum);
        dataDic.Add("account_type", 1);
        dataDic.Add("user_ip", Network.player.ipAddress);
        dataDic.Add("terminal_type", "PHONE");
        dataDic.Add("get_verify", 1);
        dataDic.Add("ts", GetTimeStamp());

        string res = PostData(UrlRoot + "phoneVerify", dataDic);
        Debug.Log(res);
        Dictionary<string, object> resData = Json.Deserialize(res) as Dictionary<string, object>;
        int result = int.Parse(resData["result"].ToString());
        switch (result)
        {
            case 0:
                Debug.Log("获取短信验证码成功！");
                GameApp.Instance.CommonHintDlg.OpenHintBox("获取短信验证码成功，请注意查收短信！");
                return true;
            default:
                GameApp.Instance.CommonHintDlg.OpenHintBox("获取短信验证码失败！(" + resData["msg"].ToString() + ")");
                return false;
        }
    }
    private string PostData(string postUrl, Dictionary<string, object> dataDic)
    {
        string data = Json.Serialize(dataDic);

        string res1 = (string)hash_hmac(appid + data, appkey);
        string sign = Convert.ToBase64String(Encoding.UTF8.GetBytes(res1));

        string res = PostWebRequest(postUrl,
                "{\"appid\":\"" + appid + "\",\"sign\":\"" + sign + "\",\"data\":" + data + "}");
        return res;
    }
    private object hash_hmac(string signatureString, string secretKey, bool raw_output = false)
    {
        var enc = Encoding.UTF8;
        HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(secretKey));
        hmac.Initialize();

        byte[] buffer = enc.GetBytes(signatureString);
        if (raw_output)
        {
            return hmac.ComputeHash(buffer);
        }
        else
        {
            return BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
        }
    }
    private string PostWebRequest(string postUrl, string paramData)
    {
        // 把字符串转换为bype数组  
        byte[] bytes = Encoding.UTF8.GetBytes(paramData);

        HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
        webReq.Method = "POST";
        webReq.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        webReq.ContentLength = bytes.Length;
        using (Stream newStream = webReq.GetRequestStream())
        {
            newStream.Write(bytes, 0, bytes.Length);
        }
        using (WebResponse res = webReq.GetResponse())
        {
            //在这里对接收到的页面内容进行处理  
            Stream responseStream = res.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string str = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            //返回：服务器响应流   
            return str;
        }
    }
    #endregion*/

    #region _播放剧情视频
    public Camera VideoCamera;
    public VideoPlayer StoryVideoVP;
    public GameObject SkipStoryVideoBtn;
    public void ShowStoryVideo()
    {
        StartCoroutine("_ShowStoryVideo");
    }
    IEnumerator _ShowStoryVideo()
    {
        if (PlayStoryVideo())
        {
            while (!StoryVideoVP.isPlaying)
                yield return new WaitForEndOfFrame();

            SkipStoryVideoBtn.SetActive(true);
            VideoCamera.clearFlags = CameraClearFlags.SolidColor;
            VideoCamera.backgroundColor = Color.black;
            
            while (StoryVideoVP.isPlaying)
                yield return new WaitForEndOfFrame();

            StopVideo();
        }
    }
    /// <summary>播放战前视频</summary>
    string PrewarAudio = string.Empty;
    public bool PlayStoryVideo()
    {
        StoryVideoVP.audioOutputMode = VideoAudioOutputMode.AudioSource;
        StoryVideoVP.SetTargetAudioSource(0, StoryVideoVP.gameObject.GetComponent<AudioSource>());
        StoryVideoVP.playOnAwake = false;
        StoryVideoVP.IsAudioTrackEnabled(0);

        StoryVideoVP.clip = Resources.Load<VideoClip>("Video/Story");
        StoryVideoVP.Play();

        StoryVideoVP.gameObject.SetActive(true);

        return true;
    }

    /// <summary>跳过视频</summary>
    public void SkipVideo()
    {
        if (StoryVideoVP.gameObject.activeSelf)
        {
            if (StoryVideoVP.isPlaying)
            {
                StopCoroutine("_ShowStoryVideo");
                StopVideo();
            }
        }
    }
    void StopVideo()
    {
        StoryVideoVP.Stop();

        SkipStoryVideoBtn.SetActive(false);
        VideoCamera.clearFlags = CameraClearFlags.Depth;

        RoleUI.Show(true);
    }
    #endregion
}
