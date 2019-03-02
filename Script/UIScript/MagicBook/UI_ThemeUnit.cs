using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ThemeUnit : MonoBehaviour
{
    public UITexture Icon;
    public UILabel Name;
    public UILabel Count;
    public UISprite NewSign;

    private int ThemeID = 0;
    void OnEnable()
    {
        StatisHoldCnt();
    }

    void Update()
    {

    }

    public void Set(GroupConfig gc)
    {
        ThemeID = gc.GroupID;

        Name.text = gc.Name;
        Icon.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("MagicCardTheme/", gc.ThemePic)) as Texture;

        StatisHoldCnt();
    }

    private void StatisHoldCnt()
    {
        if (ThemeID == 0)
            return;

        int total = 0;
        int hold = 0;
        foreach (KeyValuePair<int, MagicCardConfig> pair in CsvConfigTables.Instance.MagicCardCsvDic)
        {
            if (pair.Value.ThemeID == ThemeID)
            {
                total++;
                if (SerPlayerData.GetItemCount(pair.Key) > 0)
                    hold++;
            }
        }
        Count.text = StringBuilderTool.ToString(hold, "/", total);
        Count.gameObject.SetActive(hold != 0 || total != 0);
    }

    public void OnClick()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (GameApp.Instance.HomePageUI != null)
            GameApp.Instance.HomePageUI.MagicBookUI.ShowCardLst(ThemeID);
        else if (GameApp.Instance.TravelUI != null)
            GameApp.Instance.TravelUI.MagicBookUI.ShowCardLst(ThemeID);
    }
}
