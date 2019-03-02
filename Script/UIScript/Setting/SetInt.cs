using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInt : MonoBehaviour
{
    public UILabel des;
    public UIInput input;

    internal delegate void IntCallBack(int value);
    private IntCallBack CurCB = null;

    void Start()
    {

    }

    void Update()
    {

    }

    internal void Set(string desStr, int defInt, IntCallBack CallBackFun)
    {
        des.text = desStr + "：";
        input.value = defInt.ToString();

        CurCB += CallBackFun;
    }

    public void OnClickOK()
    {
        if (CurCB != null)
        {
            CurCB(int.Parse(input.value));
        }
    }
}
