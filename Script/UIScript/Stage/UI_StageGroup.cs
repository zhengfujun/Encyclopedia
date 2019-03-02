using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class UI_StageGroup : MonoBehaviour
{
    private List<UI_Method> StageObjs = new List<UI_Method>();

    //private GameObject TableRoot;
    //private TweenPosition tp;

    private int CurChapterID = 0;
    private int CurGroupID = 0;

    //[HideInInspector]
    //public bool AllStageDotShow = false;

    private int CurGroupIndex = 0;

    public UITexture GroupExplain_Pic;
    public UILabel GroupExplain_Title;
    public UILabel GroupExplain_Explain;

    void Awake()
    {
        CurGroupIndex = int.Parse(MyTools.GetLastString(gameObject.name, '_'));
    }

    void Start()
    {

    }

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            //MyTools.SetStageProgress(0,2);
            PlayerPrefs.DeleteAll();
        }
    }*/

    public void UpdateStageState()
    {
        switch (CurGroupIndex)
        {
            case 1:
                SetStageState(SerPlayerData.GetMainStageProgress()/*Const.IsStageAllOpen ? 4 : MyTools.GetStageProgress(CurIndex)*/);
                break;
            case 2:
                int StageProgress = 0;
                if (GameApp.Instance.PlayerData != null)
                {
                    string key_zoo = StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_Zoo_StageProgress");
                    if (PlayerPrefs.HasKey(key_zoo))
                    {
                        StageProgress = PlayerPrefs.GetInt(key_zoo);
                    }
                    else
                    {
                        PlayerPrefs.SetInt(key_zoo, 0);
                    }
                }
                else
                {
                    StageProgress = 999;
                }
                SetStageState(StageProgress);
                break;
            case 3:
                SetStageState(999);
                break;
        }


        /*if (AllStageDotShow)
        {
            GameApp.Instance.SelModeUI.StageGroupLabs[CurIndex].transform.parent.Find("Bg").GetComponent<UISprite>().spriteName = "gamemode_button_normal_g";
            GameApp.Instance.SelModeUI.StageGroupLabs[CurIndex].transform.parent.Find("SelBg").gameObject.SetActive(false);

            GameObject nextBtn = transform.parent.parent.Find("Anchor_Left").Find("Mode_" + (CurIndex + 2)).gameObject;
            GameApp.Instance.SelModeUI.SwitchShowStageGroup(nextBtn);
        }
        else
        {
            GameApp.Instance.SelModeUI.StageGroupLabs[CurIndex].transform.parent.Find("Bg").GetComponent<UISprite>().spriteName = "gamemode_button_normal";
        }*/
    }

    private StageConfig GetSC(int ArrayIndex)
    {
        foreach (KeyValuePair<int, StageConfig> pair in CsvConfigTables.Instance.StageCsvDic)
        {
            if (pair.Value.ChapterID == CurChapterID && pair.Value.GroupID == CurGroupID && pair.Value.ArrayIndex == ArrayIndex)
            {
                return pair.Value;
            }
        }
        return null;
    }
    public void SetStageInfo(int ChapterID, int GroupID)
    {
        CurChapterID = ChapterID;
        CurGroupID = GroupID;

        //TableRoot = transform.parent.gameObject;
        //tp = TableRoot.GetComponent<TweenPosition>();

        if (StageObjs.Count > 0)
            return;

        
        if(GroupExplain_Pic != null &&
            GroupExplain_Title != null &&
            GroupExplain_Explain != null)
        {
            GroupConfig gc = null;
            CsvConfigTables.Instance.GroupCsvDic.TryGetValue(CurGroupID, out gc);
            if(gc != null)
            {
                GroupExplain_Pic.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("MagicCardTheme/", gc.ThemePic)) as Texture;
                GroupExplain_Title.text = gc.Name;
                GroupExplain_Explain.text = gc.Explain;
            }
            return;
        }

        switch (CurGroupIndex)
        {
            case 1:
            case 2:
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Transform child = transform.GetChild(i);
                        if (child.name.Contains("Stage"))
                        {
                            StageObjs.Add(child.GetComponent<UI_Stage>());
                        }
                    }

                    for (int i = 0; i < StageObjs.Count; i++)
                    {
                        StageConfig sc = GetSC(i);
                        if (sc != null)
                            ((UI_Stage)StageObjs[i]).SetInfo(sc);
                    }
                }
                break;
            case 3:
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Transform child = transform.GetChild(i);
                        if (child.name.Contains("Stage"))
                        {
                            StageObjs.Add(child.GetComponent<UI_Painting>());
                        }
                    }

                    List<int> EnableUseFishCardIDLst = new List<int>();
                    foreach (KeyValuePair<int, MagicCardConfig> pair in CsvConfigTables.Instance.MagicCardCsvDic)
                    {
                        if (pair.Value.ThemeID == 103)//只显示已做资源的鱼
                        {
                            EnableUseFishCardIDLst.Add(pair.Value.CardID);
                        }
                    }

                    for (int i = 0; i < StageObjs.Count; i++)
                    {
                        if (i < EnableUseFishCardIDLst.Count)
                        {
                            MagicCardConfig MCCfg = null;
                            CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue(/*Const.FishCardFirshID + i*/EnableUseFishCardIDLst[i], out MCCfg);
                            if (MCCfg != null)
                            {
                                ((UI_Painting)StageObjs[i]).SetInfo(MCCfg);
                            }
                        }
                        else
                        {
                            StageObjs[i].gameObject.SetActive(false);
                            Transform lineObj = StageObjs[i].transform.parent.Find(StringBuilderTool.ToString("Line_", i - 1, "_", i));
                            if (lineObj != null)
                                lineObj.gameObject.SetActive(false);
                        }
                    }
                }
                break;
        }
    }

    public void SetStageState(int CurUnlockIdx)
    {
        for (int i = 0; i < StageObjs.Count; i++)
        {
            if (StageObjs[i].CurIndex == -1)
                continue;

            if (StageObjs[i].CurIndex < CurUnlockIdx)
            {
                StageObjs[i].SetState(EStageState.ePass);
            }
            else if (StageObjs[i].CurIndex == CurUnlockIdx)
            {
                StageObjs[i].SetState(EStageState.eUnlock);
            }
            else if (StageObjs[i].CurIndex > CurUnlockIdx)
            {
                StageObjs[i].SetState(EStageState.eLock);
            }
        }

        /*AllStageDotShow = false;
        int dotShowCnt = 0;
        for (int i = 0; i < StageObjs.Length; i++)
        {
            if (StageObjs[i].StageState == EStageState.eLock)
            {
                dotShowCnt++;
            }
        }
        if(dotShowCnt == StageObjs.Length)
        {
            AllStageDotShow = true;
        }*/
    }
}
