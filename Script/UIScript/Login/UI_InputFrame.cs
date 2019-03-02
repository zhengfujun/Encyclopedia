using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InputFrame : MonoBehaviour
{
    private UIInput inp;
    private UISprite spr;

    void Awake()
    {
        inp = transform.parent.GetComponent<UIInput>();

        spr = gameObject.GetComponent<UISprite>();

        Refresh();
    }

    void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            Refresh();
        }
    }

    void Refresh()
    {
        if (inp.isSelected)
        {
            spr.color = new Color(1, 1, 1, 0.4f);
        }
        else
        {
            spr.color = new Color(1, 1, 1, 0);
        }
    }
}
