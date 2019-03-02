using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class TalkingData : MonoBehaviour
{
    //int index = 1;
    //int level = 1;
    //string gameserver = "";
    //TDGAAccount account;

    const int left = 300;
    const int height = 50;
    const int top = 20;
    int width = Screen.width - left * 2;
    const int step = 60;

    //string ret = "";

    void OnGUI()
    {
        return;
        //GUI.Label(new Rect(5, 5, 3000, 5000.0f), ret);
        int i = 0;
        //GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 20), "Demo Menu");

        /*if (GUI.Button(new Rect(left, top + step * i++, width, height), "Create User"))
        {
            GameApp.Instance.TDAccount = TDGAAccount.SetAccount("User" + index);
            index++;
        }*/

        /*if (GUI.Button(new Rect(left, top + step * i++, width, height), "Set Account Type"))
        {
            if (GameApp.Instance.TDAccount != null)
            {
                GameApp.Instance.TDAccount.SetAccountType(AccountType.WEIXIN);
            }
        }*/

        /*if (GUI.Button(new Rect(left, top + step * i++, width, height), "Account Level +1"))
        {
            if (GameApp.Instance.TDAccount != null)
            {
                GameApp.Instance.TDAccount.SetLevel(level++);
            }
        }*/

        /*if (GUI.Button(new Rect(left, top + step * i++, width, height), "Chagen Game Server + 'a'"))
        {
            if (GameApp.Instance.TDAccount != null)
            {
                gameserver += "a";
                GameApp.Instance.TDAccount.SetGameServer(gameserver);
            }
        }*/

        if (GUI.Button(new Rect(left, top + step * i++, width, height), "Charge Request 10"))
        {
            TDGAVirtualCurrency.OnChargeRequest("order01", "iap", 10, "CH", 10, "PT");
        }

        if (GUI.Button(new Rect(left, top + step * i++, width, height), "Charge Success 10"))
        {
            TDGAVirtualCurrency.OnChargeSuccess("order01");
        }

        /*if (GUI.Button(new Rect(left, top + step * i++, width, height), "Reward 100"))
        {
            TDGAVirtualCurrency.OnReward(100, "reason");
        }*/

        /*if (GUI.Button(new Rect(left, top + step * i++, width, height), "Mission Begin"))
        {
            TDGAMission.OnBegin("miss001");
        }

        if (GUI.Button(new Rect(left, top + step * i++, width, height), "Mission Completed"))
        {
            TDGAMission.OnCompleted("miss001");
        }*/

        /*if (GUI.Button(new Rect(left, top + step * i++, width, height), "Item Purchase 10"))
        {
            TDGAItem.OnPurchase("itemid001", 10, 10);
        }*/

        /*if (GUI.Button(new Rect(left, top + step * i++, width, height), "Item Use 1"))
        {
            TDGAItem.OnUse("itemid001", 1);
        }*/

        if (GUI.Button(new Rect(left, top + step * i++, width, height), "Custome Event"))
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StartApp" + "StartAppTime", "startAppMac" + "#" + "02/01/2013 09:52:24");
            dic.Add("IntValue", 1);
            TalkingDataGA.OnEvent("action_id", dic);
        }
    }

    void Start()
    {
        //ret += "start...!!!!!!!!!!" + "\n";
#if UNITY_IPHONE
#if UNITY_5 || UNITY_5_6_OR_NEWER
		UnityEngine.iOS.NotificationServices.RegisterForNotifications(
			UnityEngine.iOS.NotificationType.Alert |
			UnityEngine.iOS.NotificationType.Badge |
			UnityEngine.iOS.NotificationType.Sound);
#else
		NotificationServices.RegisterForRemoteNotificationTypes(
			RemoteNotificationType.Alert |
			RemoteNotificationType.Badge |
			RemoteNotificationType.Sound);
#endif
#endif
        TalkingDataGA.OnStart("D9842FE72860480EA4A0735BEF152DEB", "Alpha");

        //文档
        //http://doc.talkingdata.com/posts/65
    }

    void Update()
    {
        /*if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }*/
#if UNITY_IPHONE
		TalkingDataGA.SetDeviceToken();
		TalkingDataGA.HandlePushMessage();
#endif
    }

    void OnDestroy()
    {
        TalkingDataGA.OnEnd();
        //ret += "onDestroy" + "\n";
    }

    /*void Awake()
    {
        ret += "Awake" + "\n";
    }

    void OnEnable()
    {
        ret += "OnEnable" + "\n";
    }

    void OnDisable()
    {
        ret += "OnDisable" + "\n";
    }*/
}
