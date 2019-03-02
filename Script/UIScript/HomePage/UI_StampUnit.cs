using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieStudio.DrawingAndColoring.Logic;

public class UI_StampUnit : MonoBehaviour
{
    public UITexture Pattern;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void Set(Texture pic)
    {
        Pattern.mainTexture = pic;
    }

    public void OnClick_Sel()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log(StringBuilderTool.ToInfoString(gameObject.name, "UI_StampUnit OnClick_Sel"));

        GameApp.Instance.HomePageUI.Coloring.ChangeStamp(int.Parse(MyTools.GetLastString(gameObject.name, '_')));
    }
}
