using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MailItemUnit : MonoBehaviour
{
    [HideInInspector]
    public MailItemInfo CurMII = null;

    public UISprite Bg;
    public UISprite HasItemSign;
    public UILabel Name;
    public UILabel Residue;

    void Start()
    {

    }

    /// <summary> 查看按钮 </summary>
    public void OnClick_Look()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【查看】");

        this.Select(true);

        GameApp.Instance.HomePageUI.MailUI.ShowMailDetail(this);
    }

    public void Set(MailItemInfo mii)
    {
        CurMII = mii;

        Name.text = mii.Name;

        Residue.text = StringBuilderTool.ToString(mii.Residue, "天");
        
        if (mii.IsReadAndGet)
        {
            Bg.spriteName = "bg_yeqian_0";

            if (mii.IsHasItem())
                HasItemSign.spriteName = "icon_baoxiang_0_hui";
        }
        else
        {
            Bg.spriteName = "bg_yeqian_2";

            if (mii.IsHasItem())
                HasItemSign.spriteName = "icon_baoxiang_0";
        }

        LastBgSpriteName = Bg.spriteName;
    }

    public void ReadAndGet()
    {
        CurMII.IsReadAndGet = true;

        Bg.spriteName = "bg_yeqian_0";

        if (CurMII.IsHasItem())
            HasItemSign.spriteName = "icon_baoxiang_0_hui";

        LastBgSpriteName = Bg.spriteName;

        //PlayerPrefs.SetInt(SerPlayerData.GetAccountID() + "_Temp_Mail_" + CurMII.ID + "_IsGet", 1);
    }

    private string LastBgSpriteName = string.Empty;
    public void Select(bool isSel)
    {
        if(isSel)
        {
            Bg.spriteName = "bg_yeqian_1";
        }
        else
        {
            Bg.spriteName = LastBgSpriteName;
        }
    }
}
