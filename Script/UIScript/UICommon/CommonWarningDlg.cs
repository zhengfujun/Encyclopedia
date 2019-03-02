using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 通用警告框
/// </summary>
public class CommonWarningDlg : UIAppearEffect
{
    public static bool IsOpening = false;

    public GameObject BoxObj;   //警告框结点

    public UILabel WarningText; //警告文本
            
    public void ToShow(bool isActive)
    {
        BoxObj.SetActive(isActive);
        IsOpening = false;
    }

    void Start()
    {
        GameApp.Instance.CommonWarningDlg = this;
    }

    internal void OpenWarningBox(string warningText)
    {
        if (BoxObj.activeSelf)
        {
            return;
        }

        base.Open(AppearType.Popup, BoxObj);

        WarningText.text = warningText;
    }

    public void OnClick_ToSetting()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        base.Close(AppearType.Diffusion, () => { ToShow(false); });

        if (!GameApp.Instance.HomePageUI.NewSettingUI.AppearEffect.isShowing)
            GameApp.Instance.HomePageUI.NewSettingUI.Show(true,2);
    }

    public void OnClick_Exit()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        base.Close(AppearType.Diffusion, () => { ToShow(false); });

        if (GameApp.Instance.LeSDKInstance)
            GameApp.Instance.LeSDKInstance.Exit();
    }
}
