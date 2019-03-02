using UnityEngine;
using System;
using System.Collections;

public class FadeHelper : MonoBehaviour
{
    Texture2D _tex;
    Texture2D FadeTex
    {
        get
        {
            if (_tex == null)
            {
                _tex = new Texture2D(1, 1);
                _tex.SetPixel(0, 0, Color.white);
                _tex.Apply();
            }
            return _tex;
        }
    }

    enum FADE_TYPE
    {
        None, IN, OUT
    }

    float beginFadeTime = 0;
    float currFadeTime;
    Color currFadeColor;
    FADE_TYPE currType = FADE_TYPE.None;

    public Action InOverFun;
    public Action OutOverFun;

    public void FadeIn(float fadeTime, Color fadeColor, Action callback = null)
    {
        this.enabled = true;
        currType = FADE_TYPE.IN;
        beginFadeTime = Time.realtimeSinceStartup;
        currFadeColor = fadeColor;
        currFadeTime = fadeTime;
        //Debug.LogError(" FadeHelper FadeIn");
        InOverFun = null;
        InOverFun = callback;

        StartCoroutine("DelayRunInOverFun", fadeTime);
    }
    IEnumerator DelayRunInOverFun(float DelayTimeLen)
    {
        yield return new WaitForSeconds(DelayTimeLen);

        if (InOverFun != null)
        {
            InOverFun();
            InOverFun = null;
        }
    }

    public void FadeOut(float fadeTime, Action callback = null)
    {
        if (currType != FADE_TYPE.IN) 
            return;
        currType = FADE_TYPE.OUT;
        beginFadeTime = Time.realtimeSinceStartup;
        currFadeTime = fadeTime;
        //Debug.LogError(" FadeHelper FadeOut");
        OutOverFun = null;
        OutOverFun = callback;

        StartCoroutine("DelayRunOutOverFun", fadeTime);
    }
    IEnumerator DelayRunOutOverFun(float DelayTimeLen)
    {
        yield return new WaitForSeconds(DelayTimeLen);

        if (OutOverFun != null)
        {
            OutOverFun();
            OutOverFun = null;
        }
    }
    
    void OnGUI()
    {
        if (currType == FADE_TYPE.IN)
        {
            Color old = GUI.color;
            Color newColor = currFadeColor;
            newColor.a = Mathf.Lerp(0, 1, (Time.realtimeSinceStartup - beginFadeTime) / this.currFadeTime);
            GUI.color = newColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTex);
            GUI.color = old;
        }
        else if (currType == FADE_TYPE.OUT)
        {
            Color old = GUI.color;
            Color newColor = currFadeColor;
            newColor.a = Mathf.Lerp(1, 0, (Time.realtimeSinceStartup - beginFadeTime) / this.currFadeTime);
            GUI.color = newColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTex);
            GUI.color = old;

            if ((Time.realtimeSinceStartup - beginFadeTime) > this.currFadeTime)
            {
                currType = FADE_TYPE.None;
                this.enabled = false;
            }
        }
    }
}
