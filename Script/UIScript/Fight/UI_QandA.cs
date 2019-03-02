using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELibraryType//题库类型
{
    eNormal,   //普通
    eBoss      //Boss
};

[System.Serializable]
public class UIPlayerInfoArrays
{
    public UI_PlayerInfo[] Array;

    public int Length
    {
        get
        {
            return Array.Length;
        }
    }
    public UI_PlayerInfo this[int index]
    {
        get
        {
            return Array[index];
        }
    }
    public UIPlayerInfoArrays()
    {
        this.Array = new UI_PlayerInfo[4];
    }
}

[System.Serializable]
public class UISpriteArrays
{
    public UISprite[] Array;

    public int Length
    {
        get
        {
            return Array.Length;
        }
    }
    public UISprite this[int index]
    {
        get
        {
            return Array[index];
        }
    }
    public UISpriteArrays()
    {
        this.Array = new UISprite[4];
    }
}

public class UI_QandA : MonoBehaviour
{
    private List<int> NormalQLLst = new List<int>();
    private List<int> BossQLLst = new List<int>();

    private QuestionConfig CurQuestionCfg = null;

    private ELibraryType CurType = ELibraryType.eNormal;

    private bool isRight = false;
    private int RightNum = 0;
    private int ErrorNum = 0;

    //private bool isEarningsHalved = false;

    public UIAppearEffect AppearEffect;

    public UIPlayerInfoArrays[] PlayerInfoArrays;

    public UITexture BattleBg;
    public UISprite BattleBgFrame;

    //public GameObject CourageState;
    public GameObject Bullet;
    public GameObject Explosion;
    public GameObject Shield;

    public UISpriteArrays[] StateArrays;

    private int CountdownNum = 999;
    public UILabel CountdownLab;
    public UISprite CountdownIcon;

    public Transform MainPlayerModelRoot;
    private GameObject MainPlayer;

    private MonsterConfig MonsterCfg = null;
    public Transform MonsterModelRoot;
    private GameObject Monster;
    //public UILabel MonsterMagicPower;

    public UILabel QuestionStem;
    public UIButton[] AnswerBtn;
    public UILabel[] AnswerOptions;
    public UITexture[] OptionPic;
    public UISprite[] ResultSpr;

    //public UISprite[] OptionBgColor;
    //private string[] OptionBgColorSpriteName = new string[4] { "fight-kapai-z-huang", "fight-kapai-z-hong", "fight-kapai-z-lan", "fight-kapai-z-lv" };

    public GameObject UseItemHint;
    public UI_Item[] Items;
    private int[] ItemIDs = new int[3] { 101,102,103 };

    public GameObject Settlement;
    public UILabel[] ScoreReward;
    public UILabel[] GoldReward;
    //public UI_Item[] CourageItems;
    //public GameObject MagicPowerNotEnough;
    public UILabel AutoCloseCD;

    private Action CloseCB = null;
    
    void Start()
    {
        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            for (int i = 0; i < PlayerInfoArrays.Length; i++)
            {
                if (i < GameApp.Instance.CurRoomPlayerLst.Count)
                {
                    for (int j = 0; j < PlayerInfoArrays[i].Length; j++)
                    {
                        PlayerInfoArrays[i][j].Init(GameApp.Instance.CurRoomPlayerLst[i]);
                    }
                }
                else
                {
                    for (int j = 0; j < PlayerInfoArrays[i].Length; j++)
                    {
                        PlayerInfoArrays[i][j].gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            IsChallenger = true;

            for (int i = 0; i < PlayerInfoArrays[0].Length; i++)
                PlayerInfoArrays[0][i].Init(GameApp.Instance.MainPlayerData);
            for (int i = 0; i < PlayerInfoArrays[1].Length; i++)
                PlayerInfoArrays[1][i].Init(GameApp.Instance.AIRobotData);

            for (int i = 0; i < PlayerInfoArrays[2].Length; i++)
                PlayerInfoArrays[2][i].gameObject.SetActive(false);
            for (int i = 0; i < PlayerInfoArrays[3].Length; i++)
                PlayerInfoArrays[3][i].gameObject.SetActive(false);
        }

        for (int i = 0; i < Items.Length; i++)
            Items[i].Set(ItemIDs[i], GameApp.Instance.MainPlayerData);

        //for (int i = 0; i < CourageItems.Length; i++)
        //    CourageItems[i].Set(102);
    }

    /*void OnDestroy()
    {

    }*/

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {

        }
    }*/
    private int GetMyselfIndex()
    {
        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
            {
                if (DefaultRule.PlayerIDToAccountID(GameApp.Instance.CurRoomPlayerLst[i].id) == GameApp.AccountID)
                {
                    return i;
                }
            }
        }
        return 0;
    }

    public void Show(bool isShow, uint QuestionID = 0, ELibraryType type = ELibraryType.eNormal, Action CloseCallback = null)
    {
        if (isShow)
        {
            GameApp.Instance.FightUI.CurEventTypeIsQandA = true;
            //GameApp.Instance.UICurrency.Show(true);
            
            UpdateScore();

            if (QuestionID == 0)
            {
                if (NormalQLLst.Count == 0)
                {
                    foreach (KeyValuePair<int, QuestionConfig> pair in CsvConfigTables.Instance.QuestionCsvDic)
                    {
                        if (pair.Value.LibraryType == 0 && pair.Value.SystemID == GameApp.Instance.CurFightStageCfg.GroupID%100)
                            NormalQLLst.Add(pair.Key);
                    }
                }
                if (BossQLLst.Count == 0)
                {
                    foreach (KeyValuePair<int, QuestionConfig> pair in CsvConfigTables.Instance.QuestionCsvDic)
                    {
                        if (pair.Value.LibraryType == 1 && pair.Value.SystemID == GameApp.Instance.CurFightStageCfg.GroupID%100)
                            BossQLLst.Add(pair.Key);
                    }
                }
            }

            for (int i = 0; i < StateArrays.Length; i++)
            {
                for (int j = 0; j < StateArrays[i].Length; j++)
                {
                    StateArrays[i][j].spriteName = "i-shalou";
                }
            }

            for (int k = 0; k < Items.Length; k++)
                Items[k].SetIsEnabled(true);

            //CourageState.SetActive(false);
            //for (int h = 0; h < CourageItems.Length; h++)
            //    CourageItems[h].gameObject.SetActive(false);

            CurType = type;
            RightNum = 0;
            ErrorNum = 0;

            isRight = false;
            IsRobotAnsweredFinished = false;
            AnswerPlayerNum = 0;
            CloseQAUINum = 0;

            //isEarningsHalved = false;

            CurQuestionCfg = null;

            BattleBgFrame.spriteName = "bg_daan_pt";

            ChallengerName.text = "";
            for (int i = 0; i < AnswerBtn.Length; i++)
            {
                AnswerBtn[i].isEnabled = true;
            }
            CountdownLab.gameObject.SetActive(true);

            switch (CurType)
            {
                case ELibraryType.eNormal:
                    {
                        BattleBg.mainTexture = Resources.Load("BigUITexture/fight-changjing01") as Texture;

                        CountdownNum = GameApp.Instance.GetParameter("NormalQandACD");

                        if (QuestionID == 0)
                        {
                            int r = UnityEngine.Random.Range(0, NormalQLLst.Count);
                            if (CsvConfigTables.Instance.QuestionCsvDic.TryGetValue(NormalQLLst[r], out CurQuestionCfg))
                            {
                                NormalQLLst.RemoveAt(r);
                            }
                        }
                        else
                        {
                            if (!CsvConfigTables.Instance.QuestionCsvDic.TryGetValue((int)QuestionID, out CurQuestionCfg))
                            {
                                GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToString("题库中未找到ID为", QuestionID, "的题目！"));
                                return;
                            }
                        }

                        CsvConfigTables.Instance.MonsterCsvDic.TryGetValue(3/*UnityEngine.Random.Range(1, 3)*/, out MonsterCfg);

                        //MonsterMagicPower.transform.parent.gameObject.SetActive(true);
                        
                        if (GameApp.Instance.PlayerData == null || GameApp.Instance.IsFightingRobot)
                            RobotRespondence();
                    }
                    break;
                case ELibraryType.eBoss:
                    {
                        BattleBg.mainTexture = Resources.Load("BigUITexture/fight-changjing02") as Texture;

                        CountdownNum = GameApp.Instance.GetParameter("BossQandACD");

                        if (QuestionID == 0)
                        {
                            int r = UnityEngine.Random.Range(0, BossQLLst.Count);
                            if (CsvConfigTables.Instance.QuestionCsvDic.TryGetValue(BossQLLst[r], out CurQuestionCfg))
                            {
                                BossQLLst.RemoveAt(r);
                            }
                        }
                        else
                        {
                            if (!CsvConfigTables.Instance.QuestionCsvDic.TryGetValue((int)QuestionID, out CurQuestionCfg))
                            {
                                GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToString("题库中未找到ID为", QuestionID, "的题目！"));
                                return;
                            }
                        }

                        CsvConfigTables.Instance.MonsterCsvDic.TryGetValue(4, out MonsterCfg);

                        //MonsterMagicPower.transform.parent.gameObject.SetActive(false);

                        int MyselfIndex = GetMyselfIndex();
                        for (int i = 0; i < StateArrays.Length; i++)
                        {
                            if (i != MyselfIndex)
                            {
                                for (int j = 0; j < StateArrays[i].Length; j++)
                                {
                                    StateArrays[i][j].spriteName = "i-yanjing";
                                }
                            }
                        }
                    }
                    break;
            }

            CreatefFightVisualize(CloseCallback);
        }
        else
        {
            GameApp.Instance.SoundInstance.StopSe(CurQuestionCfg.Voice);
            GameApp.Instance.SoundInstance.StopSe(CurQuestionCfg.WrongVoice);

            //GameApp.Instance.UICurrency.Show(false);

            StopCoroutine("Countdown");
            StopCoroutine("_AutoCloseCountdown");

            TweenControl.Instance.TweenScaleEffect(Settlement, 0.2f, 1, 0);

            Destroy(MainPlayer);
            Destroy(Monster);

            if (gameObject.activeSelf)
            {
                AppearEffect.Close(AppearType.Popup, () =>
                {
                    for (int j = 0; j < AnswerBtn.Length; j++)
                    {
                        AnswerBtn[j].isEnabled = true;
                        //OptionBgColor[j].spriteName = OptionBgColorSpriteName[j];

                        ResultSpr[j].gameObject.SetActive(false);
                    }

                    if (IsRunCloseCB)
                    {
                        if (CloseCB != null)
                        {
                            CloseCB();
                            CloseCB = null;
                        }
                    }

                    gameObject.SetActive(false);
                });
            }
            else
            {
                if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
                {
                    GameApp.Instance.FightUI.EnableNextPlayerThrowDice();
                }
            }

            if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
                GameApp.SendMsg.AnswerQuestion((uint)CurQuestionCfg.QuestionID + 1000, false);
        }
    }

    private int CurBossQuestionIndex = 0;
    private List<uint> BossQuestionIDLst = new List<uint>();
    public UILabel ChallengerName;
    public void Show(ulong PlayerID, List<uint> QuestionIDLst, Action CloseCallback = null)
    {
        GameApp.Instance.FightUI.CurEventTypeIsQandA = true;

        IsChallenger = (GameApp.Instance.CurRoomPlayerLst[GetMyselfIndex()].id == PlayerID);
        if(IsChallenger)
        {
            ChallengerName.text = "";
            for (int i = 0; i < AnswerBtn.Length; i++)
            {
                AnswerBtn[i].isEnabled = true;
            }
            CountdownLab.gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
            {
                if (GameApp.Instance.CurRoomPlayerLst[i].id == PlayerID)
                {
                    CurChallengerName = GameApp.Instance.CurRoomPlayerLst[i].name;
                    ChallengerName.text = StringBuilderTool.ToInfoString(CurChallengerName, "正在挑战魔王...");
                    break;
                }
            }
            for (int i = 0; i < AnswerBtn.Length; i++)
            {
                AnswerBtn[i].isEnabled = false;
            }
            CountdownLab.gameObject.SetActive(false);
        }

        UpdateScore();

        BossQuestionIDLst = QuestionIDLst;

        for (int i = 0; i < StateArrays.Length; i++)
        {
            for (int j = 0; j < StateArrays[i].Length; j++)
            {
                StateArrays[i][j].spriteName = "i-shalou";
            }
        }

        for (int k = 0; k < Items.Length; k++)
            Items[k].SetIsEnabled(true);

        CurType = ELibraryType.eBoss;
        RightNum = 0;
        ErrorNum = 0;

        isRight = false;
        IsRobotAnsweredFinished = false;
        AnswerPlayerNum = 0;
        CloseQAUINum = 0;
        CurBossQuestionIndex = 0;

        CurQuestionCfg = null;

        BattleBgFrame.spriteName = "bg_daan_pt";

        BattleBg.mainTexture = Resources.Load("BigUITexture/fight-changjing02") as Texture;

        CountdownNum = GameApp.Instance.GetParameter("BossQandACD");

        if (!CsvConfigTables.Instance.QuestionCsvDic.TryGetValue((int)BossQuestionIDLst[CurBossQuestionIndex], out CurQuestionCfg))
        {
            GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToString("题库中未找到ID为", BossQuestionIDLst[CurBossQuestionIndex], "的题目！"));
            return;
        }

        CsvConfigTables.Instance.MonsterCsvDic.TryGetValue(4, out MonsterCfg);

        int MyselfIndex = GetMyselfIndex();
        for (int i = 0; i < StateArrays.Length; i++)
        {
            if (i != MyselfIndex)
            {
                for (int j = 0; j < StateArrays[i].Length; j++)
                {
                    StateArrays[i][j].spriteName = "i-yanjing";
                }
            }
        }

        CreatefFightVisualize(CloseCallback);
    }

    private void UpdateScore()
    {
        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            for (int i = 0; i < PlayerInfoArrays.Length; i++)
            {
                if (i < GameApp.Instance.CurRoomPlayerLst.Count)
                {
                    for (int j = 0; j < PlayerInfoArrays[i].Length; j++)
                    {
                        UILabel Scorelab = PlayerInfoArrays[i][j].Score;
                        if (Scorelab != null)
                            Scorelab.text = GameApp.Instance.CurRoomPlayerLst[i].score.ToString();
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < PlayerInfoArrays[0].Length; j++)
            {
                UILabel Scorelab = PlayerInfoArrays[0][j].Score;
                if (Scorelab != null)
                    Scorelab.text = GameApp.Instance.MainPlayerData.Score.ToString();
            }
            for (int j = 0; j < PlayerInfoArrays[1].Length; j++)
            {
                UILabel Scorelab = PlayerInfoArrays[1][j].Score;
                if (Scorelab != null)
                    Scorelab.text = GameApp.Instance.AIRobotData.Score.ToString();
            }
        }
    }

    private void CreatefFightVisualize(Action CloseCallback)
    {
        RoleConfig rc = null;
        if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue((int)GameApp.Instance.MainPlayerData.RoleID, out rc))
        {
            GameObject MainPlayerObj = Resources.Load<GameObject>(StringBuilderTool.ToInfoString("Prefabs/Actor/", rc.ModelName));
            if (MainPlayerObj != null)
            {
                MainPlayer = GameObject.Instantiate(MainPlayerObj);
                MyTools.setLayerDeep(MainPlayer, LayerMask.NameToLayer("UI"));
                MainPlayer.transform.parent = MainPlayerModelRoot;
                MainPlayer.transform.localPosition = new Vector3(0, -102, -116);
                MainPlayer.transform.localEulerAngles = new Vector3(0, 118, 0);
                MainPlayer.transform.localScale = Vector3.one * 150;
            }
        }

        GameObject MonsterObj = Resources.Load<GameObject>(StringBuilderTool.ToInfoString("Prefabs/Actor/", MonsterCfg.ModelName));
        if (MonsterObj != null)
        {
            Monster = GameObject.Instantiate(MonsterObj);
            MyTools.setLayerDeep(Monster, LayerMask.NameToLayer("UI"));
            Monster.transform.parent = MonsterModelRoot;
            Monster.transform.localPosition = new Vector3(0, -92, -116);
            Monster.transform.localEulerAngles = new Vector3(0, -118, 0);
            Monster.transform.localScale = Vector3.one * MonsterCfg.UIScale;
        }

        if (CurQuestionCfg != null)
        {
            QuestionStem.text = CurQuestionCfg.QuestionStem;
#if UNITY_EDITOR
            QuestionStem.text += StringBuilderTool.ToInfoString("\n[00ff0040]正确答案：", CurQuestionCfg.RightAnswers, "[-]");
#endif
            AnswerOptions[0].text = CurQuestionCfg.AnswerOptionText_A;
            AnswerOptions[1].text = CurQuestionCfg.AnswerOptionText_B;
            AnswerOptions[2].text = CurQuestionCfg.AnswerOptionText_C;
            AnswerOptions[3].text = CurQuestionCfg.AnswerOptionText_D;

            OptionPic[0].mainTexture = Resources.Load(StringBuilderTool.ToInfoString("AnswerPicture/", CurQuestionCfg.AnswerOptionPic_A)) as Texture;
            OptionPic[1].mainTexture = Resources.Load(StringBuilderTool.ToInfoString("AnswerPicture/", CurQuestionCfg.AnswerOptionPic_B)) as Texture;
            OptionPic[2].mainTexture = Resources.Load(StringBuilderTool.ToInfoString("AnswerPicture/", CurQuestionCfg.AnswerOptionPic_C)) as Texture;
            OptionPic[3].mainTexture = Resources.Load(StringBuilderTool.ToInfoString("AnswerPicture/", CurQuestionCfg.AnswerOptionPic_D)) as Texture;
        }

        for (int j = 0; j < AnswerBtn.Length; j++)
        {
            AnswerBtn[j].gameObject.SetActive(false);
        }

        gameObject.SetActive(true);
        TweenControl.Instance.TweenScaleEffect(Settlement, 0, 1, 0);

        StartCoroutine("DelayShow");
        StartCoroutine("DelayShowUseItemHint");

        IsRunCloseCB = true;
        CloseCB = CloseCallback;
    }

    public void RecordCloseCallback(Action CloseCallback)
    {
        RightNum = 0;
        ErrorNum = 0;

        CloseCB = CloseCallback;
    }

    IEnumerator DelayShow()
    {
        //yield return new WaitForSeconds(0.1f);
        //transform.localPosition = new Vector3(-100, 0, 0);
        AppearEffect.transform.localScale = Vector3.one * 0.01f;
        //yield return new WaitForSeconds(0.1f);
        AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);
        yield return new WaitForSeconds(AppearEffect.OpenConsuming);
        StartCoroutine("Countdown");

        if (CurQuestionCfg.YesOrNo == 0)
        {
            for (int j = 0; j < AnswerBtn.Length; j++)
            {
                AnswerBtn[j].gameObject.SetActive(false);
                AnswerBtn[j].transform.localPosition = new Vector3(-370 + j * 230, -138, 0);
                AnswerBtn[j].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int j = 0; j < AnswerBtn.Length; j++)
            {
                AnswerBtn[j].gameObject.SetActive(false);
                AnswerBtn[j].transform.localPosition = new Vector3(-140 + j * 230, -138, 0);
                AnswerBtn[j].gameObject.SetActive(j < 2);
            }
        }

        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
        {
            if (GameApp.Instance.CurRoomPlayerLst[i].win_num > 0)
            {
                ExcludeOneErrorAnswer();
                GameApp.Instance.CommonHintDlg.OpenHintBox("小伙伴已通过冒险，为你去掉了一个错误答案！");
                break;
            }
        }

        GameApp.Instance.SoundInstance.PlayVoice(CurQuestionCfg.Voice);
    }

    IEnumerator Countdown()
    {
        if (CurType == ELibraryType.eBoss && !IsChallenger)
        {
            yield break;
        }

        CountdownIcon.color = Color.white;
        CountdownIcon.transform.localScale = Vector3.one;

        int cd = CountdownNum;
        while (cd >= 0)
        {
            cd--;
            CountdownLab.text = cd.ToString();

            if (cd == 10)
            {
                CountdownIcon.color = new Color(1f, 0.7f, 0.7f);
                CountdownIcon.transform.localScale = Vector3.one * 1.2f;
            }
            else if (cd < 10)
            {
                GameApp.Instance.SoundInstance.PlaySe("countdown");
                iTween.ShakePosition(CountdownLab.gameObject, new Vector3(0.04f, 0.02f, 0), 0.2f);
            }

            if (cd == 0)
            {
                Debug.Log("显示问答结算");
                switch (CurType)
                {
                    case ELibraryType.eNormal:
                        {
                            int MyselfIndex = GetMyselfIndex();
                            for (int j = 0; j < StateArrays[MyselfIndex].Length; j++)
                            {
                                StateArrays[MyselfIndex][j].spriteName = "i-cuo";
                            }

                            ShowSettlement();
                        }
                        break;
                    case ELibraryType.eBoss:
                        {
                            StartCoroutine("ChallengeBossFailure");
                        }
                        break;
                }
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }
    /// <summary> 点击播放题目语音 </summary>
    public void OnClick_StemVoice()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【播放题目语音】");

        GameApp.Instance.SoundInstance.StopSe(CurQuestionCfg.Voice);
        GameApp.Instance.SoundInstance.PlayVoice(CurQuestionCfg.Voice);
    }
    /// <summary> 点击选项 </summary>
    public void OnClick_Options(GameObject btn)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【选项】" + btn.name);

        if (CurType == ELibraryType.eBoss && !IsChallenger)
        {
            return;
        }

        int idx = int.Parse(MyTools.GetLastString(btn.name, '_'));

        GameApp.Instance.SoundInstance.StopSe(CurQuestionCfg.Voice);

        HideUseItemHint();

        if ((CurQuestionCfg.RightAnswers == "A" && idx == 1) ||
            (CurQuestionCfg.RightAnswers == "B" && idx == 2) ||
            (CurQuestionCfg.RightAnswers == "C" && idx == 3) ||
            (CurQuestionCfg.RightAnswers == "D" && idx == 4))
        {
            GameApp.Instance.SoundInstance.PlaySe(StringBuilderTool.ToString("RightAnswer_", UnityEngine.Random.Range(0, 3)));
            isRight = true;
        }
        else
        {
            //GameApp.Instance.SoundInstance.PlaySe(StringBuilderTool.ToString("ErrorAnswer_", UnityEngine.Random.Range(0, 3)));
            GameApp.Instance.SoundInstance.StopSe(CurQuestionCfg.Voice);
            GameApp.Instance.SoundInstance.PlayVoice(CurQuestionCfg.WrongVoice);
            isRight = false;
        }

        for (int i = 0; i < AnswerBtn.Length; i++)
        {
            AnswerBtn[i].isEnabled = false;
            //OptionBgColor[i].spriteName = "fight-kapai-z-hui";
        }

        //UISprite ResultSpr = btn.transform.Find("Result").GetComponent<UISprite>();
        if (isRight)
        {
            ResultSpr[idx - 1].spriteName = "a-dui";
            AnswerBtn[idx - 1].gameObject.GetComponent<UISprite>().spriteName = "bg_daan_xz";
            //OptionBgColor[idx - 1].spriteName = OptionBgColorSpriteName[idx - 1];

            int MyselfIndex = GetMyselfIndex();
            for (int j = 0; j < StateArrays[MyselfIndex].Length; j++)
            {
                StateArrays[MyselfIndex][j].spriteName = "i-dui";
            }

            RightNum++;
        }
        else
        {
            ResultSpr[idx - 1].spriteName = "a-cuo";

            int MyselfIndex = GetMyselfIndex();
            for (int j = 0; j < StateArrays[MyselfIndex].Length; j++)
            {
                StateArrays[MyselfIndex][j].spriteName = "i-cuo";
            }

            int RightIdx = -1;
            switch (CurQuestionCfg.RightAnswers)
            {
                case "A": RightIdx = 0; break;
                case "B": RightIdx = 1; break;
                case "C": RightIdx = 2; break;
                case "D": RightIdx = 3; break;
            }
            //AnswerOptions[RightIdx].transform.parent.GetComponent<UIButton>().isEnabled = true;
            //UISprite RightResultSpr = ResultSpr[RightIdx].transform.parent.Find("Result").GetComponent<UISprite>();
            ResultSpr[RightIdx].spriteName = "a-dui";
            ResultSpr[RightIdx].MakePixelPerfect();
            ResultSpr[RightIdx].gameObject.SetActive(true);

            AnswerBtn[RightIdx].gameObject.GetComponent<UISprite>().spriteName = "bg_daan_xz";
            //OptionBgColor[RightIdx].spriteName = OptionBgColorSpriteName[RightIdx];

            ErrorNum++;
        }
        ResultSpr[idx - 1].MakePixelPerfect();
        ResultSpr[idx - 1].gameObject.SetActive(true);

        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            switch (CurType)
            {
                case ELibraryType.eNormal:
                    {
                        GameApp.SendMsg.AnswerQuestion((uint)CurQuestionCfg.QuestionID, isRight);
                    }
                    break;
                case ELibraryType.eBoss:
                    {
                        if (RightNum >= 2)
                        {
                            GameApp.SendMsg.BossQuestionAnswerSucceed();
                        }
                        else if (ErrorNum >= 2)
                        {
                            GameApp.SendMsg.BossQuestionAnswerFailure();
                        }
                        else
                        {
                            GameApp.SendMsg.AnswerQuestion(BossQuestionIDLst[CurBossQuestionIndex], isRight);
                        }
                    }
                    break;
            }
        }
        else
        {
            switch (CurType)
            {
                case ELibraryType.eNormal:
                    {
                        StartCoroutine("DelayShowSettlement");
                    }
                    break;
                case ELibraryType.eBoss:
                    {
                        if (RightNum >= 2)
                        {
                            StartCoroutine("ChallengeBossSucceed");
                        }
                        else if (ErrorNum >= 2)
                        {
                            StartCoroutine("ChallengeBossFailure");
                        }
                        else
                        {
                            StartCoroutine("Attack");
                            SetNewQuestion(1.5f);
                        }
                    }
                    break;
            }
        }
    }

    private bool IsChallenger = false;
    private string CurChallengerName = string.Empty;
    private int AnswerPlayerNum = 0;
    private int CloseQAUINum = 0;
    public void AnswerQuestionResult(ulong playerID, bool _isRight)
    {
        isRight = _isRight;

        if (CurType == ELibraryType.eBoss && !IsChallenger)
        {
            if (isRight)
            {
                RightNum++;

                if (RightNum == 2)
                {
                    GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString(CurChallengerName, "答对两题！挑战魔王成功！"));
                    StartCoroutine("ChallengeBossSucceed");
                }
                else
                {
                    GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString(CurChallengerName, "答对一题！"));
                    StartCoroutine("Attack");
                    SetNewQuestion(1.5f);
                }
            }
            else
            {
                ErrorNum++;

                if (ErrorNum == 2)
                {
                    GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString(CurChallengerName, "答错两题！挑战魔王失败！"));
                    StartCoroutine("ChallengeBossFailure");
                }
                else
                {
                    GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString(CurChallengerName, "答错一题！"));
                    StartCoroutine("Attack");
                    SetNewQuestion(1.5f);
                }
            }
               
            return;
        }

        AnswerPlayerNum++;
        
        int Index = GetMyselfIndex();

        if (GameApp.Instance.CurRoomPlayerLst[Index].win_num > 0)
        {
            Show(false);
            return;
        }

        if (DefaultRule.PlayerIDToAccountID(playerID) == GameApp.AccountID)
        {
            switch (CurType)
            {
                case ELibraryType.eNormal:
                    {
                        StartCoroutine("DelayShowSettlement");
                    }
                    break;
                case ELibraryType.eBoss:
                    {
                        if (RightNum >= 2)
                        {
                            StartCoroutine("ChallengeBossSucceed");
                        }
                        else if (ErrorNum >= 2)
                        {
                            StartCoroutine("ChallengeBossFailure");
                        }
                        else
                        {
                            StartCoroutine("Attack");
                            SetNewQuestion(1.5f);
                        }
                    }
                    break;
            }
        }
        else
        {
            for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
            {
                if (GameApp.Instance.CurRoomPlayerLst[i].id == playerID)
                {
                    Index = i;
                    break;
                }
            }
            for (int j = 0; j < StateArrays[Index].Length; j++)
            {
                StateArrays[Index][j].spriteName = (isRight ? "i-dui" : "i-cuo");
            }
        }

        int Score = isRight ? GameApp.Instance.GetParameter("AnswerRightScore") : 0;
        int Gold = isRight ? GameApp.Instance.GetParameter("AnswerRightGold") : 0;

        ScoreReward[Index].text = StringBuilderTool.ToString("+", Score);
        GoldReward[Index].text = StringBuilderTool.ToString("+", Gold);
    }
    public void AddCloseQAUINum()
    {
        CloseQAUINum++;
    }
    public bool IsAllCloseQAUI()
    {
        int playerCnt = 0;
        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
        {
            if (GameApp.Instance.CurRoomPlayerLst[i].win_num == 0)
            {
                playerCnt++;
            }
        }

        return (CloseQAUINum >= playerCnt);
    }

    /// <summary> 点击怪物模型 </summary>
    public void OnClick_Monster()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【怪物模型】");

        StartCoroutine("_PlayMonsterClickAnim");
    }
    IEnumerator _PlayMonsterClickAnim()
    {
        Animation anim = Monster.GetComponent<Animation>();
        anim.CrossFade("click", 0.2f);
        yield return new WaitForSeconds(anim["click"].length);
        anim.CrossFade("standby", 0.2f);
    }
    #endregion

    IEnumerator ChallengeBossSucceed()
    {
        StartCoroutine("Attack");
        yield return new WaitForSeconds(2.0f);

        if (CurType == ELibraryType.eBoss && !IsChallenger)
        {
            yield return new WaitForSeconds(0.5f);

            if (CloseCB != null)
                CloseCB();
            /*GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToInfoString(CurChallengerName, "挑战魔王成功！\n获得了本局游戏的胜利！"), (ok) =>
                        {
                            GameApp.Instance.FightUI.OnClick_BackToHomePage();
                        });*/
        }
        else
        {
            GameApp.Instance.FightUI.ShowSettlement();
        }
        
        Show(false);
    }

    bool IsRunCloseCB = true;
    IEnumerator ChallengeBossFailure()
    {
        StartCoroutine("Attack");
        yield return new WaitForSeconds(2.0f);

        IsRunCloseCB = false;
        Show(false);
        GameApp.Instance.FightUI.ShowChallengeFailureHint(CloseCB);
    }

    IEnumerator DelayShowSettlement()
    {
        StopCoroutine("Countdown");
        yield return new WaitForSeconds(1);

        /*if(GameApp.Instance.MainPlayerData.MagicPower < MonsterCfg.MagicPower)
        {
            isEarningsHalved = true;
        }
        else
        {
            isEarningsHalved = false;
            GameApp.Instance.MainPlayerData.MagicPower -= MonsterCfg.MagicPower;
        }*/

        StartCoroutine("Attack");
        yield return new WaitForSeconds(1.5f);

        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            int playerCnt = 0;
            for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
            {
                if (GameApp.Instance.CurRoomPlayerLst[i].win_num == 0)
                {
                    playerCnt++;
                }
            }

            while (AnswerPlayerNum < playerCnt)
                yield return new WaitForEndOfFrame();
        }
        else
        {
            while (!IsRobotAnsweredFinished)
                yield return new WaitForEndOfFrame();
        }

        ShowSettlement();
    }

    IEnumerator Attack()
    {
        GameApp.Instance.SoundInstance.PlaySe("Bullet");

        BattleBgFrame.spriteName = "bg_daan_xz";

        Bullet.SetActive(true);
        TweenPosition.Begin(Bullet, 0.5f, new Vector3(450, 0, -300));
        yield return new WaitForSeconds(0.5f);
        Bullet.transform.localPosition = Vector3.zero;
        Bullet.SetActive(false);

        if (isRight)
        {
            Animator Anim = Monster.GetComponent<Animator>();
            if (Anim != null)
            {
                Anim.CrossFade("Hit", 0.1f);
            }

            Explosion.SetActive(true);
            GameApp.Instance.SoundInstance.PlaySe("Explosion");
        }
        else
        {
            Animator Anim = Monster.GetComponent<Animator>();
            if (Anim != null)
            {
                Anim.CrossFade("Miss", 0.1f);
            }

            Shield.SetActive(true);
            GameApp.Instance.SoundInstance.PlaySe("Shield");
        }

        yield return new WaitForSeconds(1.0f);
        Explosion.SetActive(false);
        Shield.SetActive(false);
    }

    private void ShowSettlement()
    {
        TweenControl.Instance.TweenScaleEffect(Settlement, 0.2f, 0, 1);

        StartCoroutine("_AutoCloseCountdown");

        if (GameApp.Instance.PlayerData == null || GameApp.Instance.IsFightingRobot)
        {
            if (isRight)
            {
                //new Task(ShowAwardAfterClose(Score, Gold));

                int Score = isRight ? GameApp.Instance.GetParameter("AnswerRightScore") : 0;
                int Gold = isRight ? GameApp.Instance.GetParameter("AnswerRightGold") : 0;

                GameApp.Instance.MainPlayerData.Score += Score;
                GameApp.Instance.MainPlayerData.GoldCoin += Gold;

                for (int j = 0; j < PlayerInfoArrays[0].Length; j++)
                {
                    UILabel Scorelab = PlayerInfoArrays[0][j].Score;
                    if (Scorelab != null)
                        Scorelab.text = GameApp.Instance.MainPlayerData.Score.ToString();
                }

                ScoreReward[0].text = StringBuilderTool.ToString("+", Score);
                GoldReward[0].text = StringBuilderTool.ToString("+", Gold);
            }
            else
            {
                ScoreReward[0].text = "+0";
                GoldReward[0].text = "+0";
            }
        }
    }
    /*IEnumerator ShowAwardAfterClose(int s,int g)
    {
        while (gameObject.activeSelf)
            yield return new WaitForEndOfFrame();

        GameApp.Instance.SoundInstance.PlaySe("AddScore");
        GameApp.Instance.SoundInstance.PlaySe("AddGoldCoin");

        GameApp.Instance.MainPlayerData.Score += s;
        GameApp.Instance.MainPlayerData.GoldCoin += g;
    }*/

    public void SetNewQuestion(float delay = 0)
    {
        StartCoroutine("_SetNewQuestion", delay);
    }
    IEnumerator _SetNewQuestion(float delay)
    {
        yield return new WaitForSeconds(delay);

        int MyselfIndex = GetMyselfIndex();
        for (int j = 0; j < StateArrays[MyselfIndex].Length; j++)
        {
            StateArrays[MyselfIndex][j].spriteName = "i-shalou";
        }

        for (int j = 0; j < AnswerBtn.Length; j++)
        {
            AnswerBtn[j].isEnabled = ((CurType == ELibraryType.eBoss && !IsChallenger) ? false : true);
            AnswerBtn[j].gameObject.SetActive(false);
            //OptionBgColor[j].spriteName = OptionBgColorSpriteName[j];

            ResultSpr[j].gameObject.SetActive(false);
        }

        switch (CurType)
        {
            case ELibraryType.eNormal:
                {
                    int r = UnityEngine.Random.Range(0, NormalQLLst.Count);
                    if (CsvConfigTables.Instance.QuestionCsvDic.TryGetValue(NormalQLLst[r], out CurQuestionCfg))
                    {
                        NormalQLLst.RemoveAt(r);
                    }
                }
                break;
            case ELibraryType.eBoss:
                {
                    if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
                     {
                         CurBossQuestionIndex++;
                         if (!CsvConfigTables.Instance.QuestionCsvDic.TryGetValue((int)BossQuestionIDLst[CurBossQuestionIndex], out CurQuestionCfg))
                         {
                             GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToString("题库中未找到ID为", BossQuestionIDLst[CurBossQuestionIndex], "的题目！"));
                             yield break;
                         }
                     }
                     else
                     {
                         int r = UnityEngine.Random.Range(0, BossQLLst.Count);
                         if (CsvConfigTables.Instance.QuestionCsvDic.TryGetValue(BossQLLst[r], out CurQuestionCfg))
                         {
                             BossQLLst.RemoveAt(r);
                         }
                     }
                }
                break;
        }

        if (CurQuestionCfg != null)
        {
            QuestionStem.text = CurQuestionCfg.QuestionStem;
#if UNITY_EDITOR
            QuestionStem.text += ("\n[00ff0040]正确答案：" + CurQuestionCfg.RightAnswers + "[-]");
#endif
            AnswerOptions[0].text = CurQuestionCfg.AnswerOptionText_A;
            AnswerOptions[1].text = CurQuestionCfg.AnswerOptionText_B;
            AnswerOptions[2].text = CurQuestionCfg.AnswerOptionText_C;
            AnswerOptions[3].text = CurQuestionCfg.AnswerOptionText_D;

            OptionPic[0].mainTexture = Resources.Load(StringBuilderTool.ToInfoString("AnswerPicture/", CurQuestionCfg.AnswerOptionPic_A)) as Texture;
            OptionPic[1].mainTexture = Resources.Load(StringBuilderTool.ToInfoString("AnswerPicture/", CurQuestionCfg.AnswerOptionPic_B)) as Texture;
            OptionPic[2].mainTexture = Resources.Load(StringBuilderTool.ToInfoString("AnswerPicture/", CurQuestionCfg.AnswerOptionPic_C)) as Texture;
            OptionPic[3].mainTexture = Resources.Load(StringBuilderTool.ToInfoString("AnswerPicture/", CurQuestionCfg.AnswerOptionPic_D)) as Texture;
        }

        yield return new WaitForSeconds(0.5f);

        if (CurQuestionCfg.YesOrNo == 0)
        {
            for (int j = 0; j < AnswerBtn.Length; j++)
            {
                AnswerBtn[j].gameObject.SetActive(true);
                AnswerBtn[j].transform.localPosition = new Vector3(-370 + j * 230, -138, 0);
            }
        }
        else
        {
            for (int j = 0; j < AnswerBtn.Length; j++)
            {
                AnswerBtn[j].gameObject.SetActive(j < 2);
                AnswerBtn[j].transform.localPosition = new Vector3(-140 + j * 230, -138, 0);
            }
        }

        GameApp.Instance.SoundInstance.StopAllSe();
        GameApp.Instance.SoundInstance.PlayVoice(CurQuestionCfg.Voice);
    }

    public void ExcludeOneErrorAnswer()
    {
        switch (CurQuestionCfg.RightAnswers)
        {
            case "A":
                AnswerBtn[1].isEnabled = false;
                //OptionBgColor[1].spriteName = "fight-kapai-z-hui";
                break;
            case "B":
                AnswerBtn[0].isEnabled = false;
                //OptionBgColor[2].spriteName = "fight-kapai-z-hui";
                break;
            case "C":
                AnswerBtn[3].isEnabled = false;
                //OptionBgColor[3].spriteName = "fight-kapai-z-hui";
                break;
            case "D":
                AnswerBtn[2].isEnabled = false;
                //OptionBgColor[0].spriteName = "fight-kapai-z-hui";
                break;
        }
    }

    public void SetCourageState()
    {
        //CourageState.SetActive(true);

        //CourageItems[0].gameObject.SetActive(true);
    }

    IEnumerator DelayShowUseItemHint()
    {
        yield return new WaitForSeconds(10.0f);

        List<int> AlternativeLst = new List<int>();
        for (int i = 0; i < Items.Length; i++)
        {
            if(Items[i].ItemEnableUse())
            {
                AlternativeLst.Add(i);
            }
        }
        if(AlternativeLst.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, AlternativeLst.Count);

            UseItemHint.transform.localPosition = Items[AlternativeLst[index]].transform.localPosition;
            UseItemHint.SetActive(true);
        }
    }
    public void HideUseItemHint()
    {
        StopCoroutine("DelayShowUseItemHint");
        UseItemHint.SetActive(false);
    }

    IEnumerator _AutoCloseCountdown()
    {
        int cd = 10;
        while (cd > 0)
        {
            AutoCloseCD.text = StringBuilderTool.ToString("点击屏幕继续游戏\n[AAEE00]", cd--, "秒[-]");
            yield return new WaitForSeconds(1);
        }
        OnClick_Back();
    }

    #region _机器人相关
    private void RobotRespondence()
    {
        new Task(RobotDelayRespondence());
    }

    private bool IsRobotAnsweredFinished = false;
    IEnumerator RobotDelayRespondence()
    {
        while (!gameObject.activeInHierarchy)
            yield return new WaitForEndOfFrame();

        int delay = UnityEngine.Random.Range(
            GameApp.Instance.GetParameter("RandomRespondenceBeginTime"), 
            GameApp.Instance.GetParameter("RandomRespondenceEndTime") + 1);
        yield return new WaitForSeconds(delay);

        int r1 = UnityEngine.Random.Range(0, 100);
        bool right = (r1 < GameApp.Instance.GetParameter("Accuracy"));

        int r2 = UnityEngine.Random.Range(0, 100);
        bool useItem = (r2 < GameApp.Instance.GetParameter("UseItemPR"));
        if (useItem)
        {
            useItem = GameApp.Instance.AIRobotData.UseItem(102);
        }

        if (right)
        {
            int MyselfIndex = GetMyselfIndex();
            for (int i = 0; i < StateArrays.Length; i++)
            {
                if (i != MyselfIndex)
                {
                    for (int j = 0; j < StateArrays[i].Length; j++)
                    {
                        StateArrays[i][j].spriteName = "i-dui";
                    }
                }
            }

            //CourageItems[1].gameObject.SetActive(useItem);

            int Score = GameApp.Instance.GetParameter("AnswerRightScore");
            int Gold = GameApp.Instance.GetParameter("AnswerRightGold");
            ScoreReward[1].text = StringBuilderTool.ToString("+",Score);// + (useItem ? MonsterCfg.ScoreReward * 2 : MonsterCfg.ScoreReward);
            GoldReward[1].text = StringBuilderTool.ToString("+", Gold);// + MonsterCfg.GoldReward;

            GameApp.Instance.AIRobotData.Score += Score;//(useItem ? MonsterCfg.ScoreReward * 2 : MonsterCfg.ScoreReward);
            GameApp.Instance.AIRobotData.GoldCoin += Gold;//MonsterCfg.GoldReward;
        }
        else
        {
            int MyselfIndex = GetMyselfIndex();
            for (int i = 0; i < StateArrays.Length; i++)
            {
                if (i != MyselfIndex)
                {
                    for (int j = 0; j < StateArrays[i].Length; j++)
                    {
                        StateArrays[i][j].spriteName = "i-cuo";
                    }
                }
            }

            ScoreReward[1].text = "+0";
            GoldReward[1].text = "+0";
        }

        IsRobotAnsweredFinished = true;
    }
    #endregion
}
