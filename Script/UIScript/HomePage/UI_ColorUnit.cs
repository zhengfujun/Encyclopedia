using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieStudio.DrawingAndColoring.Logic;

public class UI_ColorUnit : MonoBehaviour
{
    public UISprite Bg;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void Set(ToolContent content)
    {
        Bg.color = content.gradientColor.Evaluate(0);
    }

    public void OnClick_Sel()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log(StringBuilderTool.ToInfoString(gameObject.name, "UI_ColorUnit OnClick_Sel"));

        GameApp.Instance.HomePageUI.Coloring.ChangeToolColor(int.Parse(MyTools.GetLastString(gameObject.name, '_')));
    }
}
