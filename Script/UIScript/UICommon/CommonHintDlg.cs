using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum EHintBoxStyle
{
    eStyle_Normal,      //普通提示
    eStyle_ComingSoon   //敬请期待
}

//通用提示框
public class CommonHintDlg : MonoBehaviour
{
    public GameObject BoxObj;     //提示框结点
    public GameObject[] BgNode;     //两个不同功能的背景框结点
    public UILabel HintText;   //提示文本

    public UIGrid Grild; //弹跳框节点

    void Start()
    {
        GameApp.Instance.CommonHintDlg = this;
    }

    //显示通用提示框 
    //msgText提示文本，styleType消息框风格，CallBackFun事件处理
    internal void OpenHintBox(string hintText, EHintBoxStyle styleType = EHintBoxStyle.eStyle_Normal)
    {
        //DQGameApp.Instance.SoundInstance.PlaySe("se_warn");

        for (int i = 0; i < BgNode.Length; i++)
        {
            BgNode[i].SetActive((int)styleType == i);
        }
        BoxObj.SetActive(true);

        StopCoroutine("DelayHide");
        StartCoroutine("DelayHide", BgNode[(int)styleType]);

        HintText.text = hintText;
    }
    
    IEnumerator DelayHide(GameObject obj)
    {
        TweenColor.Begin(obj, 0.2f, Color.white);
        yield return new WaitForSeconds(2.0f);
        TweenColor.Begin(obj, 0.2f, new Color(1, 1, 1, 0));
        yield return new WaitForSeconds(0.2f);
        BoxObj.SetActive(false);
    }

    internal void SpringHint(string hintText)
    {
        UpdateSpringHint(hintText);
    }
    /// <summary>
    /// 刷新弹跳提示框
    /// </summary>
    void UpdateSpringHint(string hintText)
    {
        GameObject item = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/HintItem"));
        item.GetComponentInChildren<UILabel>().text = hintText;
        MyTools.BindChild(Grild.transform, item);

        Grild.repositionNow = true;
        StartCoroutine("DelayDestory", item);
    }
    IEnumerator DelayDestory(GameObject obj)
    {
        TweenColor.Begin(obj, 0.2f, Color.white);
        TweenScale.Begin(obj, 0.1f, Vector3.one * 1.2f);
        yield return new WaitForSeconds(0.1f);
        TweenScale.Begin(obj, 0.1f, Vector3.one);
        yield return new WaitForSeconds(2.0f);
        TweenColor.Begin(obj, 0.2f, new Color(1, 1, 1, 0));
        yield return new WaitForSeconds(0.2f);
        GameObject.Destroy(obj);
    }
}
