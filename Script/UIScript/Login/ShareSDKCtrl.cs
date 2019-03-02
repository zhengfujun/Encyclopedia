using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;
using MyMiniJSON;

public class ShareSDKCtrl : MonoBehaviour
{
	public GUISkin demoSkin;
	public ShareSDK ssdk;

    public UITextList PrintDebugInfo;
    private PlatformType CurPlatformType;

	void Start ()
	{	
		ssdk = gameObject.GetComponent<ShareSDK>();
		ssdk.authHandler = OnAuthResultHandler;
		ssdk.shareHandler = OnShareResultHandler;
		ssdk.showUserHandler = OnGetUserInfoResultHandler;
		ssdk.getFriendsHandler = OnGetFriendsResultHandler;
		ssdk.followFriendHandler = OnFollowFriendResultHandler;
	}

	// Update is called once per frame
	/*void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}*/

    internal delegate void LoginCallBack(string userID, string nickname);
    private LoginCallBack CurCB = null;
    internal delegate void LoginCallBack4WeChat(string openid, string unionid, string accessToken, string nickname);
    private LoginCallBack4WeChat CurCB4WeChat = null;
    internal delegate void FailureCallBack(string errorMsg);
    private FailureCallBack CurFailureCB = null;
    internal void WeChatLogin(LoginCallBack4WeChat cb, FailureCallBack fcb)
    {
        CurPlatformType = PlatformType.WeChat;

        ssdk.Authorize(CurPlatformType);
        CurCB4WeChat += cb;
        CurFailureCB += fcb;

        //
        /*string res = "{\"country\":\"\", \"province\":\"\", \"nickname\":\"\u732b\u62d6\u9aa1\u62c9\", \"credential\":{\"access_token\":\"16_dKg-yNotgs3urDeZUGDbgSRkqh3O68CrFMFERufG_w9PBjM0rloVJhS0conFX-R184-h1ClBikAmyeXz4p_9F3_eeYsPEXYLdxiUrq3sw-k\", \"openid\":\"oRaVf5m9CxHO9NUwOTe5KdAUKsbQ\", \"scope\":\"snsapi_userinfo\", \"refresh_token\":\"16_B2Z9Bk0-ZTNt6vpm5F9bAfsw4GfSX6QdCRBb-OecuCizAh8b7YYtU5lbbMgXDr7hEpz9RZjzIkp7AzghiIKpfSI7Bn1IrdMEvQWhW82jubE\", \"expires_in\":7200, \"unionid\":\"oUqxL1Bbbw0zFBxL4bMRWhb4hbzI\"}, \"unionid\":\"oUqxL1Bbbw0zFBxL4bMRWhb4hbzI\", \"openid\":\"oRaVf5m9CxHO9NUwOTe5KdAUKsbQ\", \"privilege\":[], \"city\":\"\", \"sex\":0, \"language\":\"zh_CN\", \"headimgurl\":\"\"}";
        LitJson.JsonData jsonRes = LitJson.JsonMapper.ToObject(res);
        LitJson.JsonData credentialData = jsonRes["credential"];

        CurCB4WeChat(jsonRes["openid"].ToString(), jsonRes["unionid"].ToString(), credentialData["access_token"].ToString(), jsonRes["nickname"].ToString());
        CurCB4WeChat = null;*/
        //
    }
    internal void QQLogin(LoginCallBack cb, FailureCallBack fcb)
    {
        CurPlatformType = PlatformType.QQ;

        ssdk.Authorize(CurPlatformType);
        CurCB += cb;
        CurFailureCB += fcb;
    }
    void Login(LoginCallBack cb, FailureCallBack fcb)
    {
        
    }

	void OnGUI ()
	{
		/*GUI.skin = demoSkin;
		
		float scale = 1.0f;

		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			scale = Screen.width / 320;
		}
		
		//float btnWidth = 165 * scale;
		float btnWidth= Screen.width / 5 * 2;
		float btnHeight = Screen.height / 25;
		float btnTop = 30 * scale;
		float btnGap = 20 * scale;
		GUI.skin.button.fontSize = Convert.ToInt32(13 * scale);

		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Authorize"))
		{
            PrintDebugInfo.Add("Authorize ssdk = " + ssdk);
            CurPlatformType = PlatformType.QQ;
            ssdk.Authorize(CurPlatformType);
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap, btnTop, btnWidth, btnHeight), "Get User Info"))
		{
            PrintDebugInfo.Add("Get User Info");
            ssdk.GetUserInfo(PlatformType.QQ);
		}*/

		/*btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Show Share Menu"))
		{
			ShareContent content = new ShareContent();

			//(Android only) 隐藏九宫格里面不需要用到的平台（仅仅是不显示平台）
			//(Android only) 也可以把jar包删除或者把Enabl属性e改成false（对应平台的全部功能将用不了）
			String[] platfsList = {((int)PlatformType.QQ).ToString(), ((int)PlatformType.Facebook).ToString(), ((int)PlatformType.TencentWeibo).ToString()};
			content.SetHidePlatforms (platfsList);

			content.SetText("this is a test string.");
			content.SetImageUrl("http://ww3.sinaimg.cn/mw690/be159dedgw1evgxdt9h3fj218g0xctod.jpg");
			content.SetTitle("test title");

			//(Android only) 针对Android绕过审核的多图分享，传图片String数组 
			String[] imageArray =  {"/sdcard/test.jpg", "http://f1.webshare.mob.com/dimgs/1c950a7b02087bf41bc56f07f7d3572c11dfcf36.jpg", "/sdcard/test.jpg"};
			content.SetImageArray (imageArray);

			content.SetTitleUrl("http://www.mob.com");
			content.SetSite("Mob-ShareSDK");
			content.SetSiteUrl("http://www.mob.com");
			content.SetUrl("http://www.mob.com");
			content.SetComment("test description");
			content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
			content.SetShareType(ContentType.Image);

			//不同平台分享不同内容
			ShareContent customizeShareParams = new ShareContent();
			customizeShareParams.SetText("Sina share content");
			customizeShareParams.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
			customizeShareParams.SetShareType(ContentType.Text);
			customizeShareParams.SetObjectID("SinaID");
			content.SetShareContentCustomize(PlatformType.SinaWeibo, customizeShareParams);
			//优先客户端分享
			// content.SetEnableClientShare(true);

			//使用微博API接口应用内分享 iOS only
			 // content.SetEnableSinaWeiboAPIShare(true);

			//通过分享菜单分享
			ssdk.ShowPlatformList (null, content, 100, 100);
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap, btnTop, btnWidth, btnHeight), "Show Share View"))
		{
			ShareContent content = new ShareContent();
			content.SetText("this is a test string.");
			content.SetImageUrl("http://ww3.sinaimg.cn/mw690/be159dedgw1evgxdt9h3fj218g0xctod.jpg");
			content.SetTitle("test title");
			content.SetTitleUrl("http://www.mob.com");
			content.SetSite("Mob-ShareSDK");
			content.SetSiteUrl("http://www.mob.com");
			content.SetUrl("http://www.mob.com");
			content.SetComment("test description");
			content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
			content.SetShareType(ContentType.Image);

			ssdk.ShowShareContentEditor (PlatformType.SinaWeibo, content);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Share Content"))
		{
			ShareContent content = new ShareContent();
			content.SetText("this is a test string.");
			content.SetImageUrl("http://ww3.sinaimg.cn/mw690/be159dedgw1evgxdt9h3fj218g0xctod.jpg");
			content.SetTitle("test title");
//			content.SetTitleUrl("http://www.mob.com");
//			content.SetSite("Mob-ShareSDK");
			// content.SetSiteUrl("http://www.mob.com");
			content.SetUrl("http://qjsj.youzu.com/jycs/");
//			content.SetComment("test description");
//			content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
			content.SetShareType(ContentType.Webpage);
			ssdk.ShareContent (PlatformType.WeChat, content);
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap, btnTop, btnWidth, btnHeight), "Get Friends SinaWeibo "))
		{
			//获取新浪微博好友，第一页，每页15条数据
			print ("Click Btn Of Get Friends SinaWeibo");
			ssdk.GetFriendList (PlatformType.SinaWeibo, 15, 0);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Get Token SinaWeibo "))
		{
			Hashtable authInfo = ssdk.GetAuthInfo (PlatformType.SinaWeibo);			
			print ("share result :");
			print (MiniJSON.jsonEncode(authInfo));
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap , btnTop, btnWidth, btnHeight), "Close SSO Auth"))
		{
			ssdk.DisableSSO (true);			
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 - btnWidth, btnTop, btnWidth, btnHeight), "Remove Authorize "))
		{
			ssdk.CancelAuthorize (PlatformType.SinaWeibo);			
		}
			
		if (GUI.Button(new Rect((Screen.width - btnGap) / 2 + btnGap, btnTop, btnWidth, btnHeight), "Add Friend "))
		{
			//关注新浪微博
			ssdk.AddFriend (PlatformType.SinaWeibo, "3189087725");			
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2 , btnTop, btnWidth, btnHeight), "ShareWithContentName"))
		{
			Hashtable customFields = new Hashtable ();
			customFields["imgUrl"] = "http://ww1.sinaimg.cn/mw690/006dJESWgw1f6iyb8bzraj31kw0v67a2.jpg";
			//根据配置文件分享【本接口功能仅暂时支持iOS】
			ssdk.ShareWithContentName(PlatformType.SinaWeibo, "ShareSDK", customFields);		
		}

		btnWidth += 80 * scale;
		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "ShowShareMenuWithContentName"))
		{
			Hashtable customFields = new Hashtable ();
			customFields["imgUrl"] = "http://ww1.sinaimg.cn/mw690/006dJESWgw1f6iyb8bzraj31kw0v67a2.jpg";
			//根据配置文件展示分享菜单分享【本接口功能仅暂时支持iOS】
			ssdk.ShowPlatformListWithContentName ("ShareSDK", customFields, null, 100, 100);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "ShowShareViewWithContentName"))
		{
			Hashtable customFields = new Hashtable ();
			//根据配置文件展示编辑界面分享【本接口功能仅暂时支持iOS】
			customFields["imgUrl"] = "http://ww1.sinaimg.cn/mw690/006dJESWgw1f6iyb8bzraj31kw0v67a2.jpg";
			ssdk.ShowShareContentEditorWithContentName(PlatformType.SinaWeibo, "ShareSDK", customFields);		
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "SMS Authorize"))
		{
			ssdk.Authorize(PlatformType.SMS);		
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Share wxMiniProgram (ios only)"))
		{
			ShareContent content = new ShareContent ();
			content.SetTitle ("MiniProgram");
			content.SetText ("test MiniProgram");
			content.SetUrl("http://www.mob.com");
			content.SetMiniProgramPath ("pages/index/index");
			content.SetThumbImageUrl ("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1527484508213&di=d993c2ca41fec50717d137718511120f&imgtype=0&src=http%3A%2F%2Fimg5.2345.com%2Fduoteimg%2FzixunImg%2Flocal%2F2017%2F05%2F03%2F14938009295612.jpg");
			content.SetMiniProgramHdThumbImage ("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1522154322305&di=7f4bf3d0803fe8c2c66c140f0a6ea0b4&imgtype=0&src=http%3A%2F%2Fa4.topitme.com%2Fo%2F201007%2F29%2F12803876734174.jpg");
			content.SetMiniProgramUserName ("gh_afb25ac019c9");
			content.SetMiniProgramWithShareTicket (true);
			content.SetMiniProgramType (0);
			content.SetShareType (ContentType.MiniProgram);

			ShareContent shareContent = new ShareContent ();
			shareContent.SetShareContentCustomize (PlatformType.WeChat, content);
			ssdk.ShareContent (PlatformType.WeChat, shareContent);
		}*/
	}
	
	void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
		if (state == ResponseState.Success)
        {
            if (result != null && result.Count > 0)
            {
                string res = MiniJSON.jsonEncode(result);
                //PrintDebugInfo.Add("Authorize Success !" + "Platform:" + type + " Result:" + res);
                Debug.Log("==========================================");
                Debug.Log(res);
                Debug.Log("==========================================");
                                
                /*switch(CurPlatformType)
                {
                    case PlatformType.WeChat:
                        {
                            PrintDebugInfo.Add("解析：");
                            PrintDebugInfo.Add("country：" + resData["country"].ToString());
                            PrintDebugInfo.Add("refresh_token：" + resData["refresh_token"].ToString());
                            PrintDebugInfo.Add("province：" + resData["province"].ToString());
                            PrintDebugInfo.Add("gender：" + resData["gender"].ToString());
                            PrintDebugInfo.Add("icon：" + resData["icon"].ToString());
                            PrintDebugInfo.Add("unionid：" + resData["unionid"].ToString());
                            PrintDebugInfo.Add("openid：" + resData["openid"].ToString());
                            PrintDebugInfo.Add("nickname：" + resData["nickname"].ToString());
                            PrintDebugInfo.Add("userID：" + resData["userID"].ToString());
                            PrintDebugInfo.Add("city：" + resData["city"].ToString());
                            PrintDebugInfo.Add("expiresTime：" + resData["expiresTime"].ToString());
                            PrintDebugInfo.Add("expiresIn：" + resData["expiresIn"].ToString());
                            PrintDebugInfo.Add("token：" + resData["token"].ToString());
                        }
                        break;
                    case PlatformType.QQ:
                        {
                            PrintDebugInfo.Add("解析：");
                            PrintDebugInfo.Add("expiresIn：" + resData["expiresIn"].ToString());
                            PrintDebugInfo.Add("pfkey：" + resData["pfkey"].ToString());
                            PrintDebugInfo.Add("secret：" + resData["secret"].ToString());
                            PrintDebugInfo.Add("iconQzone：" + resData["iconQzone"].ToString());
                            PrintDebugInfo.Add("gender：" + resData["gender"].ToString());
                            PrintDebugInfo.Add("icon：" + resData["icon"].ToString());
                            PrintDebugInfo.Add("pay_token：" + resData["pay_token"].ToString());
                            PrintDebugInfo.Add("unionid：" + resData["unionid"].ToString());
                            PrintDebugInfo.Add("nickname：" + resData["nickname"].ToString());
                            PrintDebugInfo.Add("pf：" + resData["pf"].ToString());
                            PrintDebugInfo.Add("secretType：" + resData["secretType"].ToString());
                            PrintDebugInfo.Add("userID：" + resData["userID"].ToString());
                            PrintDebugInfo.Add("expiresTime：" + resData["expiresTime"].ToString());
                            PrintDebugInfo.Add("token：" + resData["token"].ToString());
                        }
                        break;
                }*/

                /*iOS 微信返回值
                {
                    "country":"", 
                    "province":"", 
                    "nickname":"\u732b\u62d6\u9aa1\u62c9", 
                    "credential":
                    {
                        "access_token":"16_dKg-yNotgs3urDeZUGDbgSRkqh3O68CrFMFERufG_w9PBjM0rloVJhS0conFX-R184-h1ClBikAmyeXz4p_9F3_eeYsPEXYLdxiUrq3sw-k", 
                        "openid":"oRaVf5m9CxHO9NUwOTe5KdAUKsbQ", 
                        "scope":"snsapi_userinfo", 
                        "refresh_token":"16_B2Z9Bk0-ZTNt6vpm5F9bAfsw4GfSX6QdCRBb-OecuCizAh8b7YYtU5lbbMgXDr7hEpz9RZjzIkp7AzghiIKpfSI7Bn1IrdMEvQWhW82jubE", 
                        "expires_in":7200, 
                        "unionid":"oUqxL1Bbbw0zFBxL4bMRWhb4hbzI"
                    }, 
                    "unionid":"oUqxL1Bbbw0zFBxL4bMRWhb4hbzI", 
                    "openid":"oRaVf5m9CxHO9NUwOTe5KdAUKsbQ", 
                    "privilege":[], 
                    "city":"", 
                    "sex":0, 
                    "language":"zh_CN", 
                    "headimgurl":"http://thirdwx.qlogo.cn/mmopen/vi_32/b9KoSo6KV9AlDjeViaEhFssSSBv9iaCB0NTF3rw3BXBic1yF4q6CicUjOicLElH2TPliaqiceVEG4ljIahic5JQmyVWcLg/132"
                }*/


                if (CurCB != null)
                {
                    Dictionary<string, object> resData = Json.Deserialize(res) as Dictionary<string, object>;
                    CurCB(resData["userID"].ToString(), resData["nickname"].ToString());
                    CurCB = null;
                }

                if (CurCB4WeChat != null)
                {
                    LitJson.JsonData jsonRes = LitJson.JsonMapper.ToObject(res);
                    LitJson.JsonData credentialData = jsonRes["credential"];
                    
                    CurCB4WeChat(jsonRes["openid"].ToString(), jsonRes["unionid"].ToString(), credentialData["access_token"].ToString(), jsonRes["nickname"].ToString());
                    CurCB4WeChat = null;
                }
            }
            else
            {
                PrintDebugInfo.Add("Authorize Success !" + " Platform:" + type);
            }
        }
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
            PrintDebugInfo.Add("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
            if (CurFailureCB != null)
            {
                CurFailureCB(result["msg"].ToString());
            }
            CurFailureCB = null;
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
            PrintDebugInfo.Add("cancel !");
            if (CurFailureCB != null)
            {
                CurFailureCB("用户取消授权登录！");
            }
            CurFailureCB = null;
		}
	}
	
	void OnGetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
		if (state == ResponseState.Success)
		{
			print ("get user info result :");
			print (MiniJSON.jsonEncode(result));
            print("AuthInfo:" + MiniJSON.jsonEncode(ssdk.GetAuthInfo(PlatformType.WeChat)));
            PrintDebugInfo.Add("AuthInfo:" + MiniJSON.jsonEncode(ssdk.GetAuthInfo(PlatformType.WeChat)));
            print ("Get userInfo success !Platform :" + type );
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
            PrintDebugInfo.Add("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
            print("cancel !");
            PrintDebugInfo.Add("cancel !");
		}
	}
	
	void OnShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("share successfully - share result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnGetFriendsResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{			
			print ("get friend list result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnFollowFriendResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("Follow friend successfully !");
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
}
