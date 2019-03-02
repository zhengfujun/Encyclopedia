using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieStudio.DrawingAndColoring.Logic;


public class UI_ToolUnit : MonoBehaviour
{
    public EColorinToolType ColorinToolType = EColorinToolType.ePencil;

    public UISprite[] Bg;

    public GameObject SelSpr;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < Bg.Length; i++ )
            Bg[i].color = color;
    }

    public void ShowSelSpr(bool isShow)
    {
        SelSpr.SetActive(isShow);
    }

    public void OnClick_Sel()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log(StringBuilderTool.ToInfoString(gameObject.name, "UI_ToolUnit OnClick_Sel"));

        for(int i = 0; i < transform.parent.childCount; i++)
        {
            Transform child = transform.parent.GetChild(i);
            UI_ToolUnit tu = child.GetComponent<UI_ToolUnit>();
            if(tu != null)
            {
                tu.ShowSelSpr(false);
            }
        }

        ShowSelSpr(true);

        GameApp.Instance.HomePageUI.Coloring.SelTool(ColorinToolType);
    }
}
