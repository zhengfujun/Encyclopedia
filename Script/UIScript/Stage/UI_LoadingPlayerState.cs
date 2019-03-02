using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingPlayerState : MonoBehaviour
{
    [HideInInspector]
    public ulong PlayerID;

    public UISprite Portrait;
    public UISprite PortraitBg;

    public UILabel LoadState;

    public Transform LoadingIcon;

    public GameObject[] LoadingPoint;

    void OnEnable()
    {
        LoadState.gameObject.SetActive(false);
        StartCoroutine("RunShowLoadingPoint");
    }

    void Update()
    {

    }

    public void Set(ulong _PlayerID, UISprite _Portrait, UISprite _PortraitBg)
    {
        PlayerID = _PlayerID;

        Portrait.spriteName = _Portrait.spriteName;
        PortraitBg.spriteName = _PortraitBg.spriteName;

        gameObject.SetActive(true);
    }

    public void LoadingOver()
    {
        StopCoroutine("RunShowLoadingPoint");
        for (int i = 0; i < LoadingPoint.Length; i++)
        {
            LoadingPoint[i].SetActive(false);
        }
        LoadState.gameObject.SetActive(true);
    }

    IEnumerator RunShowLoadingPoint()
    {
        int CurShowCnt = 0;
        while (gameObject.activeSelf)
        {
            CurShowCnt++;
            if (CurShowCnt > 3)
                CurShowCnt = 0;

            for (int i = 0; i < LoadingPoint.Length; i++)
            {
                LoadingPoint[i].SetActive(i < CurShowCnt);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}
