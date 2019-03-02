using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Chapter : MonoBehaviour
{
    private int ChapterID;

    public ChapterConfig CurChapterCfg = null;

    //public Transform Root;
    public UISprite Icon;
    public UILabel Name;

    private UIFollowTarget ft = null;

    void Awake()
    {
        ChapterID = int.Parse(gameObject.name);
    }

    void Start()
    {
        ft = gameObject.GetComponent<UIFollowTarget>();
    }

    void Update()
    {
        //Root.transform.localScale = Vector3.one * (1f - (1f - 0.8f) * Mathf.Abs(transform.position.x) / 1.17f);

        //if(gameObject.name == "1")
        //{
            //float dis = Vector3.Distance(ft.target.position, GameApp.Instance.HomePageSceneMgr.CenterObj.position);
            //Debug.Log(dis);

            //float scale = Math.Max(0,1f - dis * 0.1f);
        transform.localScale = Vector3.one * (Math.Max(0, 1f - Vector3.Distance(ft.target.position, GameApp.Instance.HomePageSceneMgr.CenterObj.position) * 0.18f));
        //}
    }

    public void Set(int ChapterID)
    {
        if (CsvConfigTables.Instance.ChapterCsvDic.TryGetValue(ChapterID, out CurChapterCfg))
        {
            Name.text = CurChapterCfg.Name;
        }
    }

    public void OnClick()
    {
        //GameApp.Instance.SoundInstance.PlaySe("button");
        //Debug.Log("点击【选择】" + Name.text);

        //if (Mathf.Abs(Root.transform.localScale.x) < 0.99f)
        //    return;

        int Idx = int.Parse(gameObject.name);
        GameApp.Instance.HomePageSceneMgr.RotateEarth(Idx, 
            () => 
            {
                if (ChapterID == 1)
                {
                    GameApp.Instance.SoundInstance.PlaySe("DinosaurRoar");
                    GameApp.Instance.SoundInstance.PlaySe("Elephant");

                    GameApp.Instance.HomePageUI.StageMapUI.Show(true, ChapterID);
                }
                else
                {
                    GameApp.Instance.CommonHintDlg.OpenHintBox("", EHintBoxStyle.eStyle_ComingSoon);
                }
            });
    }
}
