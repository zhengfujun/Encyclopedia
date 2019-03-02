using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 滚动文本单元 </summary>
public class RollLabelUnit : MonoBehaviour
{
    private UICenterOnChild COC;
    private UILabel Lab;

    internal delegate void SetSelCallBack(string text);
    private SetSelCallBack CurCB = null;

    void Awake()
    {
        COC = gameObject.GetComponentInParent<UICenterOnChild>();
        Lab = gameObject.GetComponent<UILabel>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (COC.centeredObject == gameObject)
        {
            //Debug.Log(Lab.text);
            if (CurCB != null)
            {
                CurCB(Lab.text);
            }
        }
    }

    public void Refresh(string text)
    {
        if (text == Lab.text)
        {
            COC.springStrength = 1000;
            new Task(_CenterOn());
        }
    }

    IEnumerator _CenterOn()
    {
        yield return new WaitForEndOfFrame();

        UICenterOnClick coc = gameObject.GetComponent<UICenterOnClick>();
        coc.OnClick();

        COC.springStrength = 8;
    }

    internal void SetSelCallBackFun(SetSelCallBack CallBackFun)
    {
        CurCB += CallBackFun;
    }
}
