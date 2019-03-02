using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_Launch : MonoBehaviour
{
    static private bool IsFirstRun = true;

    public GameObject BgTex;
    //public GameObject Warning;

    void Start()
    {
        if (!IsFirstRun)
        {
            Destroy(gameObject);
            return;
        }
        
        StartCoroutine("FadeOut");

        IsFirstRun = false;
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.0f);

        TweenAlpha ta = TweenAlpha.Begin(BgTex, 1.0f, 0);
        yield return new WaitForSeconds(0.5f);
        
        //TweenAlpha ta = TweenAlpha.Begin(Warning, 1.0f, 0);
        ta.onFinished.Add(new EventDelegate(() =>
        {
            Destroy(gameObject);
        }));

        //yield return new WaitForSeconds(0.5f);

#if UNITY_ANDROID && !UNITY_EDITOR
        GameApp.Instance.UILogin.LeSDKLogin();
#else
        GameApp.Instance.UILogin.ShowLoginType();
#endif
    }
}
