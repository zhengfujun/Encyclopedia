using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 出现类型
/// </summary>
public enum AppearType
{
    /// <summary>
    /// 弹出
    /// </summary>
    Popup,

    /// <summary>
    /// 扩散
    /// </summary>
    Diffusion
}

/// <summary>
/// UI出现特效
/// </summary>
public class UIAppearEffect : MonoBehaviour
{
    public AppearType type = AppearType.Popup;
    private GameObject BeingContObj;
    public float OpenConsuming = 0.2f;
    private Action OpenOverCB = null;
    private Action CloseOverCB = null;

    [HideInInspector]
    public bool isShowing = false;

    /// <summary>
    /// 显示UI
    /// </summary>
    /// <param name="type">出现类型</param>
    /// <param name="obj">被控制的结点</param>
    public void Open(AppearType _type, GameObject obj, float delay = 0, Action callback = null)
    {
        if (!obj)
        {
            return;
        }
        BeingContObj = obj;
        type = _type;

        OpenOverCB = callback;

        if (!BeingContObj.activeSelf)
        {
            BeingContObj.SetActive(true);
            if (type == AppearType.Popup)
            {
                BeingContObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                TweenAlpha.Begin(gameObject, 0, 1);
            }
        }
        iTween.Stop(gameObject);
        StartCoroutine("_Open", delay);
    }

    IEnumerator _Open(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (type == AppearType.Popup)
        {
            GameApp.Instance.SoundInstance.PlaySe("PopIn");

            iTween.ValueTo(gameObject, iTween.Hash("name", "OpenMsgBox",
                                "from", 0, "to", 1,
                                "easetype", iTween.EaseType.easeOutBack, "loopType", iTween.LoopType.none,
                                "onstart", "OpenStart", "onupdate", "Update4Popup", "oncomplete", "OpenEnd",
                                "time", OpenConsuming));
        }

        isShowing = true;
    }

    public void Close(Action callback = null)
    {
        if (!BeingContObj)
        {
            if (callback != null)
            {
                callback();
            }
            return;
        }

        CloseOverCB = callback;
        if (type == AppearType.Popup)
        {
            GameApp.Instance.SoundInstance.PlaySe("PopOut");

            iTween.ValueTo(gameObject,iTween.Hash("name", "CloseMsgBox",
                                "from", 1,"to", 0,
                                "easetype", iTween.EaseType.easeInBack,"loopType", iTween.LoopType.none,
                                "onupdate", "Update4Popup","oncomplete", "CloseEnd",
                                "time", OpenConsuming));
        }
        if (type == AppearType.Diffusion)
        {
            iTween.ValueTo(gameObject, iTween.Hash("name", "CloseMsgBox",
                                "from", 1, "to", 1.2f,
                                "easetype", iTween.EaseType.easeOutCubic, "loopType", iTween.LoopType.none,
                                "onupdate", "Update4Popup", "oncomplete", "CloseEnd",
                                "time", OpenConsuming));
            TweenAlpha.Begin(gameObject, OpenConsuming / 2f, 0);
        }
    }

    public void Close(AppearType _type, Action callback = null)
    {
        type = _type;
        Close(callback);
    }

    #region Extend 弹出型
    void OpenStart()
    {
        BeingContObj.SetActive(true);
    }
    void Update4Popup(float v)
    {
        BeingContObj.transform.localScale = new Vector3(v, v, 1);
    }
    void OpenEnd()
    {
        if (OpenOverCB != null)
        {
            OpenOverCB();
        }
    }
    void CloseEnd()
    {
        BeingContObj.SetActive(false);
        if(CloseOverCB != null)
        {
            CloseOverCB();
        }
        iTween.Stop(gameObject);

        isShowing = false;
    }
    #endregion
}
