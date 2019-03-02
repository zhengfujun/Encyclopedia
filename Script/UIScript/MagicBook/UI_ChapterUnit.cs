using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChapterUnit : MonoBehaviour
{
    public UISprite Bg;
    public UISprite Icon;
    public UILabel Name;
    public GameObject Lock;

    private ChapterConfig CurChapterCfg = null;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Set(int ChapterID)
    {
        if (CsvConfigTables.Instance.ChapterCsvDic.TryGetValue(ChapterID, out CurChapterCfg))
        {
            Name.text = CurChapterCfg.Name;

            //if (ChapterID > 1)
            //{
            //    Lock.SetActive(true);
            //    Bg.spriteName = "bg_yeqian_0";
            //    Icon.gameObject.SetActive(false);

            //    gameObject.GetComponent<BoxCollider>().enabled = false;
            //}
            //else
            //{
            if (Lock != null)
                Lock.SetActive(false);
            
            Icon.spriteName = CurChapterCfg.Icon;
            //}
        }
    }
}
