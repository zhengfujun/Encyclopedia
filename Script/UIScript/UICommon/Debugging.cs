using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//using System.Diagnostics;
//using UnityEngine.EventSystems;
using System.IO;

//全局调试
public class Debugging : MonoBehaviour
{
    public GUISkin skin;

    #region _FPS相关变量
    private float frequency = 0.5f;
    private float itsAccumulatedFrames = 0;
    private int itsFramesInInterval = 0;
    private float itsTimeLeft;
    private float itsCurrentFPS = 0.0f;
    private Color FpsColor;
    #endregion

    [HideInInspector]
    private bool isShowWindow = false;
    private bool isShowDeviceInfoWindow = false;
//#if UNITY_IPHONE || UNITY_ANDROID
//    float m_fScreenWidth = 960;
//    float m_fScreenHeight = 640;
//#endif
    // scale factor
    //float m_fScaleWidth;
    //float m_fScaleHeight;

    //private string inputOrder = "";
    
    /*void Start()
    {

    }*/

    void Update()
    {
        if (Const.IsDevelopMode)
        {
            itsTimeLeft -= Time.deltaTime;
            itsAccumulatedFrames += Time.timeScale / Time.deltaTime;
            itsFramesInInterval++;

            if (itsTimeLeft < 0.0f)
            {
                itsCurrentFPS = itsAccumulatedFrames / itsFramesInInterval;
                itsTimeLeft = frequency;
                itsAccumulatedFrames = 0.0f;
                itsFramesInInterval = 0;
                
                if (itsCurrentFPS < 15)
                {
                    FpsColor = Color.red;
                }
                else if (itsCurrentFPS >= 15 && itsCurrentFPS < 24)
                {
                    FpsColor = Color.yellow;
                }
                else
                {
                    FpsColor = Color.green;
                } 
            }
        }
    }

    public void ShowGMOrderWindow()
    {
        if (isShowDeviceInfoWindow)
        {
            isShowDeviceInfoWindow = false;
            return;
        }

        isShowWindow = !isShowWindow;

        foreach (UICamera c in UICamera.list)
        {
            c.useMouse = c.useTouch = !isShowWindow;
        }
    }

    void OnGUI()
    {
        if (Const.IsDevelopMode)
        {
#if UNITY_IPHONE || UNITY_ANDROID
            GUILayout.Space(Screen.width / 1280);
#endif
            //GUI.skin = skin;

            GUI.skin.label.fontSize = 16;
            GUI.color = FpsColor;
            GUI.Label(new Rect(Screen.width - 76, -2, 120, 30), string.Format("FPS:{0}", (int)itsCurrentFPS));
        }
    }
}


