using UnityEngine;
using System.Collections;

public class AutoScale : MonoBehaviour
{
    float standard_width = 1280f;
    float standard_height = UIRoot.ScreenHeight;
    float device_width = 0f;
    float device_height = 0f;

    void Start()
    {
        device_width = Screen.width;
        device_height = Screen.height;

        //Debug.Log(gameObject.name + " DW:" + device_width + " DH:" + device_height);

        if (Const.IsLowAspectRatioFullScreen)
        {
            if ((int)standard_height == 720)
            {
                SetBackgroundSize();
            }
            else
            {
                UIWidget m_back_sprite = GetComponent<UIWidget>();
                if (m_back_sprite == null)
                    return;

                m_back_sprite.MakePixelPerfect();
                m_back_sprite.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnHeight;
                m_back_sprite.height = (int)standard_height;
            }
        }
    }

    private void SetBackgroundSize()
    {
        UIWidget m_back_sprite = GetComponent<UIWidget>();
        if (m_back_sprite == null)
            return;

        m_back_sprite.MakePixelPerfect();
        float back_width = m_back_sprite.width;
        float back_height = m_back_sprite.height;

        float standard_aspect = standard_width / standard_height;
        float device_aspect = device_width / device_height;
        float extend_aspect = 0f;
        float scale = 0f;

        //Debug.Log("standard_aspect = " + standard_aspect);
        //Debug.Log("device_aspect = " + device_aspect);
        if ((int)(device_aspect * 100) >= (int)(standard_aspect * 100)) //按宽度适配
        {
            //Debug.Log("按宽度适配");
            scale = device_aspect / standard_aspect;

            extend_aspect = back_width / standard_width;
        }
        else //按高度适配
        {
            //Debug.Log("按高度适配");
            scale = standard_aspect / device_aspect;

            extend_aspect = back_height / standard_height;
        }

        if (extend_aspect >= scale) //冗余尺寸足以适配，无须放大
        {

        }
        else   //冗余尺寸不足以适配，在此基础上放大
        {
            scale /= extend_aspect;
            //Debug.Log("scale = " + scale);

            if (m_back_sprite.width == standard_width)
            {
                //Debug.Log("m_back_sprite.width = " + m_back_sprite.width);
                m_back_sprite.width = (int)(m_back_sprite.width * scale);
            }
            if (m_back_sprite.height == standard_height)
            {
                //Debug.Log("m_back_sprite.height = " + m_back_sprite.height);
                m_back_sprite.height = (int)(m_back_sprite.height * scale);
            }
        }
    }
}