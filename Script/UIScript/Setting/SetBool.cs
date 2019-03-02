using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBool : MonoBehaviour
{
    public UILabel des;
    public UIToggle checkbox;

    internal delegate void BoolCallBack(bool value);
    private BoolCallBack CurCB = null;

    void Start()
    {

    }

    void Update()
    {

    }

    internal void Set(string desStr, bool defBool, BoolCallBack CallBackFun)
    {
        des.text = desStr + "：";
        checkbox.value = defBool;

        CurCB += CallBackFun;
    }

    public void OnValueChange(UIToggle toggle)
    {
        if (CurCB != null)
        {
            CurCB(toggle.value);
        }
    }
}
