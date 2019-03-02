using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetString : MonoBehaviour
{
    public UILabel des;
    public UIInput input;

    internal delegate void StringCallBack(string value);
    private StringCallBack CurCB = null;

    void Start()
    {

    }

    void Update()
    {

    }

    internal void Set(string desStr, string defStr, StringCallBack CallBackFun)
    {
        des.text = desStr + "：";
        input.value = defStr;

        CurCB += CallBackFun;
    }

    public void OnClickOK()
    {
        if (CurCB != null)
        {
            CurCB(input.value);
        }
    }
}
