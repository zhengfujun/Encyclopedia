using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 通用消息框
/// </summary>
public class CommonMsgDlg : UIAppearEffect
{
    public static bool IsOpening = false;

    public GameObject BoxObj;   //消息框结点

    public UILabel MsgText; //消息文本

    public GameObject OKBtnObj;     //确定按钮
    public GameObject CloseBtnObj;  //取消按钮

    internal delegate void BtnCallBack(bool isOk);
    private BtnCallBack CurBtnCB = null;
    
    public void ToShow(bool isActive)
    {
        BoxObj.SetActive(isActive);
        IsOpening = false;
    }

    void Start()
    {
        GameApp.Instance.CommonMsgDlg = this;
    }

    //显示通用消息框 
    //msgText消息文本，styleType消息框风格，CallBackFun事件处理
    internal void OpenMsgBox(string msgText, BtnCallBack CallBackFun = null)
    {
        if (BoxObj.activeSelf)
        {
            if(!MsgText.text.Contains(msgText))
            {
                Debug.Log("消息排队： " + msgText);
                StartCoroutine(AgainMsg(msgText, CallBackFun));
            }
            return;
        }

        base.Open(AppearType.Popup, BoxObj);

        MsgText.text = msgText;

        CurBtnCB += CallBackFun;
        IsOpening = true;

        OKBtnObj.transform.localPosition = new Vector3(138, -140, 0);
        CloseBtnObj.SetActive(true);
    }

    internal void OpenSimpleMsgBox(string msgText, BtnCallBack CallBackFun = null)
    {
        if (BoxObj.activeSelf)
        {
            if (!MsgText.text.Contains(msgText))
            {
                Debug.Log("消息排队： " + msgText);
                StartCoroutine(AgainMsg(msgText, CallBackFun));
            }
            return;
        }

        base.Open(AppearType.Popup, BoxObj);

        MsgText.text = msgText;

        CurBtnCB += CallBackFun;
        IsOpening = true;

        OKBtnObj.transform.localPosition = new Vector3(0, -140, 0);
        CloseBtnObj.SetActive(false);
    }

    public void OnClick_OK()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (CurBtnCB != null)
        {
            CurBtnCB(true);
        }

        base.Close(AppearType.Diffusion, () => { ToShow(false); });
        CurBtnCB = null;
    }

    public void OnClick_Cancel()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (CurBtnCB != null)
        {
            CurBtnCB(false);
        }

        base.Close(AppearType.Diffusion, () => { ToShow(false); });
        CurBtnCB = null;
    }

    IEnumerator AgainMsg(string msgText, BtnCallBack CallBackFun)
    {
        while (IsOpening)
            yield return new WaitForEndOfFrame();

        OpenMsgBox(msgText, CallBackFun);
    }
}
