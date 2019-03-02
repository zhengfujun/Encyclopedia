using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//等待某事结束的提示框
public class WaitLoadHintDlg : MonoBehaviour
{
    public GameObject BoxObj;  //提示框结点
    public UILabel HintText;   //提示文本

    internal delegate bool HideCallBack();
    private HideCallBack CurCB = null;

    void Start()
    {
        GameApp.Instance.WaitLoadHintDlg = this;
    }

    internal void OpenHintBox(string hintText,HideCallBack cb = null)
    {
        BoxObj.SetActive(true);

        HintText.text = hintText;

        CurCB += cb;
        if (CurCB != null)
        {
            StopCoroutine("DelayHide");
            StartCoroutine("DelayHide");
        }
    }
    public void CloseHintBox()
    {
        BoxObj.SetActive(false);
        CurCB = null;
    }

    IEnumerator DelayHide()
    {
        if (CurCB != null)
        {
            while (!CurCB())
            {
                yield return new WaitForEndOfFrame();
            }
        }
        BoxObj.SetActive(false);
    }
}
