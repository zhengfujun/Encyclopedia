using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common;

public class UI_Fight : MonoBehaviour
{
    public bool GameBegin = false;

    public UI_QandA QandAUI;
    [HideInInspector]
    public bool CurEventTypeIsQandA = false;

    public UI_ShopInFight ShopInFightUI;

    public ChessControl ChessCtrl;

    public GameObject RoundSign;
    public UILabel RoundHint;

    public List<UI_PlayerInfo> UIPlayerInfoLst;

    public GameObject BeginGame;

    public UIButton DiceBtn;
    public GameObject DialObj;
    public TweenRotation PointerTR;
    public GameObject ClickDiceHint;
    private int DiceHintShowCnt = 0;
    public UILabel ClickDiceCountdown;
    public Animation[] DiceCountAnim;
    public UILabel ProtagonistTopSteps;
    public UILabel OpponentTopSteps;

    public UILabel ThrowDictResult;

    public UILabel BeginPointHint;

    public GameObject SquaresHintLab;

    public UILabel RandomMoveLab;
    private Dictionary<int, string> RandomMoveLst = new Dictionary<int, string>()
        {
            {0,"原地不动"},
            {1,"前进1步"},
            {2,"前进2步"},
            {3,"前进3步"},
            {4,"前进4步"},
            {5,"前进5步"},
            {6,"前进6步"},
            {-1,"后退1步"},
            {-2,"后退2步"},
            {-3,"后退3步"},
            {-4,"后退4步"},
            {-5,"后退5步"},
            {-6,"后退6步"},
        };

    public UILabel GotoBossHint;
    public GameObject UnableGotoBossHintObj;
    public UILabel UGHAutoHideDes;
    //private Action CloseUnableGotoBossHintCallback = null;
    public GameObject ChallengeFailureHintObj;

    public GameObject Settlement;
    public GameObject TreasureRoot;
    public GameObject ClickHint;
    public UILabel TRTitle;
    public UILabel AutoOpenHint;
    public GameObject CardRoot;
    public UILabel CRTitle;
    public UI_MagicCard[] CRCard;
    public UIButton LookOnBtn;
    public GameObject TreasureBtn;
    public Animator animTreasure;
    private int CurClickTreasureCnt = 0;
    private int MaxClickTreasureCnt = 3;

    public ERoleType RecordCurRoleType = ERoleType.ePlayer_0;

    public UITextList PrintDebugInfo;

    public UILabel RobotShoppingHint;

    public GameObject OpenTestBtn;
    public GameObject TestFunRoot;

    private int PlayerThrowDictCD;
    private int BossTicketsScore;
    private int StartPointScore;

    private bool ShowEnterAnimation = false;
    private bool SkipEnterAnim = false;
    public GameObject[] Show4EnterAnim;
    public GameObject[] Hide4EnterAnim;
    public UILabel SceneIntroduce;

    [HideInInspector]
    public int lastTeamMemCnt = 0;

    void Awake()
    {
        GameApp.Instance.FightUI = this;
    }

    public void init()
    {
        List<uint> RoleIDs = new List<uint>();

        for (int i = 0; i < UIPlayerInfoLst.Count; i++)
        {
            if (i < GameApp.Instance.CurRoomPlayerLst.Count)
            {
                RoleIDs.Add(GameApp.Instance.CurRoomPlayerLst[i].icon);
                UIPlayerInfoLst[i].Init(GameApp.Instance.CurRoomPlayerLst[i]);
                UIPlayerInfoLst[i].gameObject.SetActive(true);
            }
            else
            {
                UIPlayerInfoLst[i].gameObject.SetActive(false);
            }
        }

        ChessCtrl.InitChessPieces(RoleIDs);
    }

    void Start()
    {
        GameApp.Instance.SoundInstance.PlayBgm("BGM_MainGame_Normal");

        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            lastTeamMemCnt = GameApp.Instance.CurRoomPlayerLst.Count;
            init();
        }
        else
        {
            //每局游戏开始，金币，魔力值和积分都需要重置，积分和金币从0开始计算。
            GameApp.Instance.MainPlayerData.Score = 0;
            GameApp.Instance.MainPlayerData.GoldCoin = 0;
            //GameApp.Instance.MainPlayerData.MagicPower = UI_NewMagicBook.CalcMagicPower();

            GameApp.Instance.AIRobotData.Score = 0;
            GameApp.Instance.AIRobotData.GoldCoin = 0;
            //GameApp.Instance.AIRobotData.MagicPower = (int)(UI_NewMagicBook.CalcMagicPower() * 0.8f);

            UIPlayerInfoLst[0].Init(GameApp.Instance.MainPlayerData);
            UIPlayerInfoLst[1].Init(GameApp.Instance.AIRobotData);
            UIPlayerInfoLst[2].gameObject.SetActive(false);
            UIPlayerInfoLst[3].gameObject.SetActive(false);

            List<uint> RoleIDs = new List<uint>();
            RoleIDs.Add(GameApp.Instance.MainPlayerData.RoleID);
            RoleIDs.Add(GameApp.Instance.AIRobotData.RoleID);
            ChessCtrl.InitChessPieces(RoleIDs);

            if (GameApp.Instance.CurFightStageCfg == null)
            {
                foreach (KeyValuePair<int, StageConfig> pair in CsvConfigTables.Instance.StageCsvDic)
                {
                    if (pair.Value.FightSceneName == GameApp.Instance.SceneCtlInstance.CurSceneName)
                    {
                        GameApp.Instance.CurFightStageCfg = pair.Value;
                        break;
                    }
                }
            }
            ShowEnterAnimation = (GameApp.Instance.CurFightStageCfg.EnterAnimation.Length > 0);
        }

        DiceBtn.isEnabled = false;

        if (GameApp.Instance.GetParameter("ShowCustomDice") > 0)
        {
            OpenTestBtn.SetActive(true);
            DiceBtn.transform.localPosition = new Vector3(-254, 94, 0);
        }

        PlayerThrowDictCD = GameApp.Instance.GetParameter("PlayerThrowDictCD");
        BossTicketsScore = GameApp.Instance.GetParameter("BossTicketsScore");
        StartPointScore = GameApp.Instance.GetParameter("StartPointScore");

        StartCoroutine("Begin");
    }

    void OnDestroy()
    {
        GameApp.Instance.FightUI = null;
    }

    IEnumerator Begin()
    {
        if (ShowEnterAnimation)
        {
            GameApp.Instance.UICurrency.Show(false);
            for(int i = 0; i < Show4EnterAnim.Length; i++)
            {
                Show4EnterAnim[i].SetActive(true);
            }
            for (int i = 0; i < Hide4EnterAnim.Length; i++)
            {
                Hide4EnterAnim[i].SetActive(false);
            }

            SceneIntroduce.text = GameApp.Instance.CurFightStageCfg.SceneIntroduce;

            float AnimationLength = 0;

            GameObject CameraAnim = Resources.Load<GameObject>(StringBuilderTool.ToString("Prefabs/Effect/", GameApp.Instance.CurFightStageCfg.EnterAnimation));
            if (CameraAnim != null)
            {
                GameApp.Instance.SoundInstance.PlayVoice(GameApp.Instance.CurFightStageCfg.Voice);

                GameObject CameraAnimObj = GameObject.Instantiate(CameraAnim);
                CameraAnimObj.transform.parent = GameApp.Instance.FightUI.ChessCtrl.CameraControl.transform;
                CameraAnimObj.transform.localPosition = Vector3.zero;
                CameraAnimObj.transform.localEulerAngles = Vector3.zero;
                CameraAnimObj.transform.localScale = Vector3.one;

                Animator OwnAnim = CameraAnimObj.GetComponentInChildren<Animator>(true);
                AnimationClip[] anis = OwnAnim.runtimeAnimatorController.animationClips;
                if (anis.Length > 0)
                {
                    AnimationLength = anis[0].length;
                }

                float beginTime = Time.realtimeSinceStartup;
                while (!SkipEnterAnim)
                {
                    if (Time.realtimeSinceStartup - beginTime >= AnimationLength + 0.5f)
                        SkipEnterAnim = true;
                    else
                        yield return new WaitForSeconds(Time.deltaTime);
                }

                Destroy(CameraAnimObj);
                GameApp.Instance.SoundInstance.StopSe(GameApp.Instance.CurFightStageCfg.Voice);

                GameApp.Instance.UICurrency.Show(true);
                for (int i = 0; i < Show4EnterAnim.Length; i++)
                {
                    Show4EnterAnim[i].SetActive(false);
                }
                for (int i = 0; i < Hide4EnterAnim.Length; i++)
                {
                    Hide4EnterAnim[i].SetActive(true);
                }
            }
        }

        while (Loading.InLoading)
            yield return new WaitForEndOfFrame();

        if (GameApp.Instance.LoadingDlg != null)
            GameApp.Instance.LoadingDlg.SetPlayerLoadingType(PlayerLoadingType.Single);

        GameApp.Instance.SoundInstance.PlaySe("BeginGame");
        iTween.ScaleTo(BeginGame,Vector3.one,0.2f);
        yield return new WaitForSeconds(1f);
        iTween.ScaleTo(BeginGame, Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.2f);

        GameBegin = true;
//#if UNITY_EDITOR
//        ChessCtrl.CreateSquaresHintLab(SquaresHintLab);
//#endif
        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            EnableNextPlayerThrowDice();
        }
        else
        {
            CurRoleType = ERoleType.ePlayer_0;
            CurUIPlayerInfo = UIPlayerInfoLst[0];
            WaitClickDice();

            if (GameApp.Instance.FirstChessGuideUI != null)
                GameApp.Instance.FirstChessGuideUI.SetGuideState(9);
        }
    }

    private int NextPlayerRoundIndex = 0;
    private PVE_Room_Player CurActionPlayer = null;
    private UI_PlayerInfo CurUIPlayerInfo = null;
    private ERoleType CurRoleType = ERoleType.ePlayer_0;
    public void ThrowDiceResult(ulong PlayerID, uint Dice_num)
    {
        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
        {
            if (GameApp.Instance.CurRoomPlayerLst[i].id == PlayerID)
            {
                CurActionPlayer = GameApp.Instance.CurRoomPlayerLst[i];
                CurUIPlayerInfo = UIPlayerInfoLst[i];
                NextPlayerRoundIndex = i;
                break;
            }
        }
        
        if(Dice_num > 100)
        {
            int DiceCnt1 = (int)Dice_num / 100;
            int DiceCnt2 = (int)Dice_num % 100;
            StartCoroutine(ShowThrowTwoDict(DiceCnt1, DiceCnt2));
#if UNITY_EDITOR
            Debug.Log("玩家" + CurActionPlayer.name + "摇出：" + DiceCnt1 + " " + DiceCnt2);
#endif
        }
        else
        {
            if (Dice_num == 0)
            {
                NextPlayerRoundIndex++;
                if (NextPlayerRoundIndex >= GameApp.Instance.CurRoomPlayerLst.Count)
                {
                    NextPlayerRoundIndex = 0;
                }

                int WinPlayerCnt = 0;
                for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
                {
                    if (GameApp.Instance.CurRoomPlayerLst[i].win_num > 0)
                    {
                        WinPlayerCnt++;
                    }
                }
                if (WinPlayerCnt >= GameApp.Instance.CurRoomPlayerLst.Count)
                {
                    if (!Settlement.activeSelf)
                    {
                        GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox("所有小伙伴均已通过冒险，点击确定离开关卡。", (ok) =>
                        {
                            GameApp.Instance.FightUI.OnClick_BackToHomePage();
                        });
                    }
                    return;
                }

                GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString(CurActionPlayer.name, "已获胜，跳过掷骰子！"));
                EnableNextPlayerThrowDice();

                return;
            }
            else
            {
                StartCoroutine("ShowThrowOneDict", (int)Dice_num);
            }
#if UNITY_EDITOR
            Debug.Log("玩家" + CurActionPlayer.name + "摇出：" + Dice_num);
#endif
        }

        //Action Next = () =>
        //    {
                NextPlayerRoundIndex++;
                if (NextPlayerRoundIndex >= GameApp.Instance.CurRoomPlayerLst.Count)
                {
                    NextPlayerRoundIndex = 0;
                }
       //     };

        /*Next();
        int winNum = 0;
        while(GameApp.Instance.CurRoomPlayerLst[NextPlayerRoundIndex].win_num > 0)
        {
            Next();
            winNum++;
            if(winNum == GameApp.Instance.CurRoomPlayerLst.Count)
            {
                break;
            }
        }*/
    }

    public void EnableNextPlayerThrowDice()
    {
        StartCoroutine("BeforePlayerThrowDice");        
    }
    IEnumerator BeforePlayerThrowDice()
    {
        if (CurEventTypeIsQandA)
        {
            while (!QandAUI.IsAllCloseQAUI())
                yield return new WaitForEndOfFrame();

            CurEventTypeIsQandA = false;
        }

        if (NextPlayerRoundIndex >= GameApp.Instance.CurRoomPlayerLst.Count)
        {
            NextPlayerRoundIndex = 0;
        }

        switch (NextPlayerRoundIndex)
        {
            case 0: CurRoleType = ERoleType.ePlayer_0; break;
            case 1: CurRoleType = ERoleType.ePlayer_1; break;
            case 2: CurRoleType = ERoleType.ePlayer_2; break;
            case 3: CurRoleType = ERoleType.ePlayer_3; break;
        }

        yield return new WaitForSeconds(1.0f);

        RoundSign.transform.localPosition = new Vector3(298, -134 - NextPlayerRoundIndex * 96, 0);

        RoundHint.gameObject.SetActive(true);
        RoundHint.text = "轮到 " + GameApp.Instance.CurRoomPlayerLst[NextPlayerRoundIndex].name + " 掷骰子";

        if (GameApp.Instance.CurRoomPlayerLst[NextPlayerRoundIndex].win_num == 0)
            ChessCtrl.SwitchCamFocus(CurRoleType);

        yield return new WaitForSeconds(1.0f);

        if (NextPlayerRoundIndex < GameApp.Instance.CurRoomPlayerLst.Count)
        {
            if (DefaultRule.PlayerIDToAccountID(GameApp.Instance.CurRoomPlayerLst[NextPlayerRoundIndex].id) == GameApp.AccountID)
            {
                if (GameApp.Instance.CurRoomPlayerLst[NextPlayerRoundIndex].win_num > 0)
                {
                    GameApp.SendMsg.Throw(0);
                }
                else
                {
                    WaitClickDice();
                }
            }
            else
            {
                if (GameApp.Instance.CurRoomPlayerLst[NextPlayerRoundIndex].win_num > 0 &&
                    GameApp.Instance.CurRoomPlayerLst[NextPlayerRoundIndex].game_state == 1)
                {
                    GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString(GameApp.Instance.CurRoomPlayerLst[NextPlayerRoundIndex].name, "已获胜，跳过掷骰子！"));
                
                    NextPlayerRoundIndex++;
                    if (NextPlayerRoundIndex >= GameApp.Instance.CurRoomPlayerLst.Count)
                    {
                        NextPlayerRoundIndex = 0;
                    }

                    EnableNextPlayerThrowDice();
                }
            }
        }

        RoundHint.gameObject.SetActive(false);
    }

    public void ChangePlayerScore(ulong PlayerID, uint score)
    {
        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
        {
            if (GameApp.Instance.CurRoomPlayerLst[i].id == PlayerID)
            {
                UIPlayerInfoLst[i].SetScore((int)score);
                break;
            }
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.A))
        {
            //QandAUI.Show(true, ELibraryType.eNormal);
            //GameApp.SendMsg.QuitRoom();
            GameApp.Instance.FightUI.ShowSettlement();
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            //GetRandomMove();
            //ChessCtrl.StopMove();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            //ShopInFightUI.Show(true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            //ShowGotoBossHint();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            //ShowUnableGotoBossHint(null);
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            //ShowSettlement();
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            /*ShowChallengeFailureHint(() =>
            {
                Debug.Log("返回至外圈 完成");
            });*/
        }
#endif

        if(IsDicePress)
        {
            if(PressDiceBeginTime + 2f <= Time.realtimeSinceStartup)
            {
                IsDicePress = false;

                TweenAlpha.Begin(DialObj,0.1f,1);

                PointerTR.transform.localEulerAngles = Vector3.zero;
                PointerTR.ResetToBeginning();
                PointerTR.from = Vector3.zero;
                PointerTR.PlayForward();

                IsRockDice = true;
            }
        }
    }
    //
    bool IsDicePress = false;
    bool IsRockDice = false;
    float PressDiceBeginTime = 0f;
    //
    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Exit()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        GameApp.Instance.CommonMsgDlg.OpenMsgBox("确定退出本局游戏？",
                    (isExit) =>
                    {
                        if (isExit)
                        {
                            GameApp.Instance.SceneCtlInstance.ChangeScene(SceneControl.HomePage);

                            GameApp.SendMsg.QuitRoom();
                            GameApp.Instance.CommonHintDlg.OpenHintBox("已退出本局游戏！");
                        }
                    });
    }

    /// <summary> 点击跳过入场动画 </summary>
    public void OnClick_SkipEnterAnim()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【跳过入场动画】");

        SkipEnterAnim = true;
    }

    /// <summary> 点击骰子 </summary>
    public void OnClick_Dice()
    {
#if UNITY_EDITOR
        Debug.Log("点击【骰子】");
#endif
        IsDicePress = false;
        ClickDice();
    }
    /// <summary> 按住骰子 </summary>
    public void OnPress_Dice()
    {
#if UNITY_EDITOR
        Debug.Log("按住【骰子】");
#endif
        PressDiceBeginTime = Time.realtimeSinceStartup;
        IsDicePress = true;

        ClickDiceHint.SetActive(false);
    }
    /// <summary> 松开骰子 </summary>
    public void OnRelease_Dice()
    {
#if UNITY_EDITOR
        Debug.Log("松开【骰子】");
#endif
        if (IsRockDice)
        {
            float angle = PointerTR.transform.localEulerAngles.z;
            int cnt = 0;
            //Debug.Log("PointerTR角度：" + angle);
            if (angle > 270f && angle <= 297f)
            {
                //Debug.Log("5或6");
                cnt = UnityEngine.Random.Range(5, 7);
            }
            else if (angle > 297f && angle <= 333f)
            {
                //Debug.Log("3或4");
                cnt = UnityEngine.Random.Range(3, 5);
            }
            else if (angle > 333f && angle <= 359.9999f || angle == 0f)
            {
                //Debug.Log("1或2");
                cnt = UnityEngine.Random.Range(1, 3);
            }
            ClickDice(cnt);
        }
        else
        {
            IsDicePress = false;
        }
    }
    /// <summary> 点击打开测试界面 </summary>
    public void OnClick_OpenTest()
    {
        Debug.Log("点击【打开测试界面】");

        TestFunRoot.SetActive(!TestFunRoot.activeSelf);
    }
    /// <summary> 点击自定义骰子 </summary>
    public void OnClick_CustomDice(GameObject btn)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【自定义骰子】" + btn.name);

        int cnt = int.Parse(MyTools.GetLastString(btn.name, '_'));
        ClickDice(cnt);
    }
    /// <summary> 点击加积分 </summary>
    public void OnClick_ScoreAdd()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【加积分】");

        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            for (int i = 0; i < UIPlayerInfoLst.Count; i++)
            {
                if (UIPlayerInfoLst[i].gameObject.activeSelf)
                    UIPlayerInfoLst[i].Score.text = "200";
            }
        }
        else
        {
            GameApp.Instance.MainPlayerData.Score += 100;
            GameApp.Instance.AIRobotData.Score += 100;
        }
    }
    /// <summary> 点击关闭不能挑战魔王的提示框 </summary>
    public void OnClick_UnableGotoBossHintOK()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【不能挑战魔王的提示框】");

        StopCoroutine("DelayHideUnableGotoBossHint");
        UnableGotoBossHintObj.SetActive(false);
    }
    /// <summary> 点击宝箱 </summary>
    public void OnClick_Treasure()
    {
        Debug.Log("点击【宝箱】");

        CurClickTreasureCnt++;
        if(CurClickTreasureCnt > MaxClickTreasureCnt)
        {
            //打开
            StopCoroutine("UpdateAutoOpenTreasureHint");

            CreateAward();
        }
        else
        {
            //变大
            GameApp.Instance.SoundInstance.PlaySe("ClickTreasure");

            if (ClickHint.activeSelf)
                ClickHint.SetActive(false);

            TreasureBtn.transform.localScale *= 1.1f;
            iTween.ShakePosition(TreasureBtn, new Vector3(0.05f, 0.03f, 0), 0.2f);
        }
    }
    /// <summary> 点击返回主界面 </summary>
    public void OnClick_BackToHomePage()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【返回主界面】");

        GameApp.SendMsg.SetGameState(1);
        GameApp.Instance.SceneCtlInstance.ChangeScene(SceneControl.HomePage);
    }
    /// <summary> 点击继续观战 </summary>
    public void OnClick_LookOn()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【继续观战】");

        Settlement.SetActive(false);
        //ChoiceMoveObject(ERoleType.eNull);
    }
    #endregion
    private void WaitClickDice()
    {
        DiceBtn.isEnabled = true;
        if (DiceHintShowCnt == 0)
        {
            ClickDiceHint.SetActive(true);
            DiceHintShowCnt++;
        }
        else
        {
            StartCoroutine("DelayShowClickDiceHint");
        }
        StopCoroutine("DiceCountdown");
        StartCoroutine("DiceCountdown");
    }
    IEnumerator DiceCountdown()
    {
        int cd = PlayerThrowDictCD;
        while (cd > 0)
        {
            ClickDiceCountdown.text = StringBuilderTool.ToString("剩余 ", (cd--), " 秒");
            yield return new WaitForSeconds(1);
        }
        ClickDice();
    }
    IEnumerator DelayShowClickDiceHint()
    {
        yield return new WaitForSeconds(10);
        ClickDiceHint.SetActive(true);
    }
    private void ClickDice(int CustomDiceCnt = 0)
    {
        GameApp.Instance.SoundInstance.PlaySe("ThrowDict");

        if (GameApp.Instance.FirstChessGuideUI != null)
            GameApp.Instance.FirstChessGuideUI.SetGuideState(10);

        IsDicePress = false;
        IsRockDice = false;
        PointerTR.enabled = false;
        TweenAlpha.Begin(DialObj, 0.1f, 0).delay = 2.0f;

        DiceBtn.isEnabled = false;
        ClickDiceCountdown.text = "";

        StopCoroutine("DelayShowClickDiceHint");
        ClickDiceHint.SetActive(false);

        StopCoroutine("DiceCountdown");

        if (ChessCtrl.AngelCtrl.Use() && CustomDiceCnt == 0)
        {
            int DiceCnt1 = UnityEngine.Random.Range(1, DiceCountAnim.Length);
            int DiceCnt2 = UnityEngine.Random.Range(1, DiceCountAnim.Length);
            if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
            {
                GameApp.SendMsg.Throw((uint)(DiceCnt1 * 100 + DiceCnt2));
            }
            else
            {
                StartCoroutine(ShowThrowTwoDict(DiceCnt1, DiceCnt2));
                Debug.Log("摇出：" + DiceCnt1 + " " + DiceCnt2);
            }
        }
        else
        {
            int DiceCnt = (CustomDiceCnt == 0 ? UnityEngine.Random.Range(1, DiceCountAnim.Length) : CustomDiceCnt);
            if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
            {
                GameApp.SendMsg.Throw((uint)DiceCnt);
            }
            else
            {
                StartCoroutine("ShowThrowOneDict", DiceCnt);
                Debug.Log("摇出：" + DiceCnt);
            }
        }
    }
    IEnumerator ShowThrowOneDict(int cnt)
    {
        Animation ma = DiceCountAnim[Mathf.Abs(cnt)];
        ma.gameObject.SetActive(true);
        yield return new WaitForSeconds(ma.clip.length);
        ThrowDictResult.gameObject.SetActive(true);
        ThrowDictResult.text = "投掷结果：" + cnt + "点";
        yield return new WaitForSeconds(0.5f);
        ThrowDictResult.gameObject.SetActive(false);
        ma.gameObject.SetActive(false);

        //StartCoroutine("UpdateProtagonistTopSteps", Mathf.Abs(cnt));

        ChessCtrl.Move(CurRoleType, cnt,
            () =>
            {
                //if (Settlement.activeSelf)
                //    return;

                if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
                {
                    EnableNextPlayerThrowDice();
                }
                else
                {
                    ChoiceMoveObject(CurRoleType);
                }
            });
    }
    IEnumerator ShowThrowTwoDict(int cnt1,int cnt2)
    {
        Animation anim1 = DiceCountAnim[Mathf.Abs(cnt1)];
        GameObject newLeftDictPar = new GameObject();
        newLeftDictPar.transform.parent = anim1.transform.parent;
        newLeftDictPar.transform.localPosition = new Vector3(-3,0,0);
        newLeftDictPar.transform.localScale = Vector3.one;
        GameObject leftDict = GameObject.Instantiate(anim1.gameObject);
        leftDict.transform.parent = newLeftDictPar.transform;
        leftDict.SetActive(true);
        Animation leftAnim = leftDict.GetComponent<Animation>();

        Animation anim2 = DiceCountAnim[Mathf.Abs(cnt2)];
        GameObject newRightDictPar = new GameObject();
        newRightDictPar.transform.parent = anim2.transform.parent;
        newRightDictPar.transform.localPosition = new Vector3(3, 0, 0);
        newRightDictPar.transform.localScale = Vector3.one;
        GameObject rightDict = GameObject.Instantiate(anim2.gameObject);
        rightDict.transform.parent = newRightDictPar.transform;
        rightDict.SetActive(true);
        //Animation rightAnim = rightDict.GetComponent<Animation>();

        yield return new WaitForSeconds(leftAnim.clip.length);
        ThrowDictResult.gameObject.SetActive(true);
        ThrowDictResult.text = "投掷结果：" + (cnt1 + cnt2) + "点";
        yield return new WaitForSeconds(1.0f);
        ThrowDictResult.gameObject.SetActive(false);
        ChessCtrl.AngelCtrl.AngelUI.ShowHasAngelHint(false);
        Destroy(newLeftDictPar);
        Destroy(newRightDictPar);

        //StartCoroutine("UpdateProtagonistTopSteps", (cnt1 + cnt2));
        ChessCtrl.Move(CurRoleType, (cnt1 + cnt2),
            () =>
            {
                //if (Settlement.activeSelf)
                //    return;

                if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
                {
                    EnableNextPlayerThrowDice();
                }
                else
                {
                    ChoiceMoveObject(CurRoleType);
                }
            });
    }
    /*IEnumerator UpdateProtagonistTopSteps(int cnt)
    {
        ProtagonistTopSteps.gameObject.SetActive(true);
        while (cnt > 0)
        {
            ProtagonistTopSteps.text = (cnt--).ToString();
            yield return new WaitForSeconds(0.7f);
        }
        ProtagonistTopSteps.text = "";
        ProtagonistTopSteps.gameObject.SetActive(false);
    }*/

    public void GetRandomMove()
    {
        StartCoroutine("RollRandomMove");
    }
    IEnumerator RollRandomMove()
    {
        RandomMoveLab.gameObject.SetActive(true);

        int cnt = 10;
        int resSymbol = 0;
        int resNum = 0;
        int key = 0;
        while (cnt > 0)
        {
            cnt--;
            resSymbol = UnityEngine.Random.Range(0, 2);
            resNum = UnityEngine.Random.Range(0, 7);
            key = (resSymbol == 0 ? -1 : 1) * resNum;
            RandomMoveLab.text = RandomMoveLst[key];
            yield return new WaitForSeconds(0.2f);
        }

        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            if (DefaultRule.PlayerIDToAccountID(CurActionPlayer.id) == GameApp.AccountID)
            {
                GameApp.SendMsg.TriggerGridEffect(EEventType.eMove);
            }
        }
        else
        {
            RandomMoveLab.text = StringBuilderTool.ToInfoString("随机到：", RandomMoveLst[key]);

            yield return new WaitForSeconds(1.0f);

            ChessCtrl.Move(RecordCurRoleType, key,
                () =>
                {
                    ChoiceMoveObject(RecordCurRoleType);
                });

            RandomMoveLab.gameObject.SetActive(false);
        }
    }
    public void RandomMoveResult(ulong PlayerID, MoveDirectionType MoveType, uint StepNum)
    {
        int key = 0;
        switch(MoveType)
        {
            case MoveDirectionType.MoveDirectionType_Before:
                switch(StepNum)
                {
                    case 1: key = 1; break;
                    case 2: key = 2; break;
                    case 3: key = 3; break;
                    case 4: key = 4; break;
                    case 5: key = 5; break;
                    case 6: key = 6; break;
                }
                break;
            case MoveDirectionType.MoveDirectionType_After:
                switch (StepNum)
                {
                    case 1: key = -1; break;
                    case 2: key = -2; break;
                    case 3: key = -3; break;
                    case 4: key = -4; break;
                    case 5: key = -5; break;
                    case 6: key = -6; break;
                }
                break;
            case MoveDirectionType.MoveDirectionType_Stand:
                key = 0;
                break;
        }

        StartCoroutine("DelayRandomMove", key);
    }
    IEnumerator DelayRandomMove(int key)
    {
        RandomMoveLab.text = StringBuilderTool.ToInfoString("随机到：", RandomMoveLst[key]);

        yield return new WaitForSeconds(1.0f);

        ChessCtrl.Move(CurRoleType, key,
                () =>
                {
                    //if (Settlement.activeSelf)
                    //    return;

                    EnableNextPlayerThrowDice();
                });

        RandomMoveLab.gameObject.SetActive(false);
    }

    public void ShowBeginPointHint()
    {
        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            if (DefaultRule.PlayerIDToAccountID(CurActionPlayer.id) == GameApp.AccountID)
            {
                GameApp.SendMsg.TriggerGridEffect(EEventType.eStart);

                BeginPointHint.text = StringBuilderTool.ToString(CurActionPlayer.name, "经过起点获得", StartPointScore, "积分");
                BeginPointHint.gameObject.SetActive(true);
                StartCoroutine("DelayHideBeginPointHint");
            }
        }
        else
        {
            GameApp.Instance.SoundInstance.PlaySe("AddScore");

            switch (RecordCurRoleType)
            {
                case ERoleType.ePlayer_0:
                    GameApp.Instance.MainPlayerData.Score += StartPointScore;
                    break;
                case ERoleType.ePlayer_1:
                    GameApp.Instance.AIRobotData.Score += StartPointScore;
                    break;
            }

            BeginPointHint.gameObject.SetActive(true);
            StartCoroutine("DelayHideBeginPointHint");
        }
    }

    IEnumerator DelayHideBeginPointHint()
    {
        yield return new WaitForSeconds(2.0f);
        BeginPointHint.gameObject.SetActive(false);
    }

    public void OpenQandA(ELibraryType type, Action CloseCallback)
    {
        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            RecordType4OpenQandA = type;
            RecordCloseCallback4OpenQandA = CloseCallback;

            if (DefaultRule.PlayerIDToAccountID(CurActionPlayer.id) == GameApp.AccountID)
            {
                switch (type)
                {
                    case ELibraryType.eNormal:
                        GameApp.SendMsg.TriggerGridEffect(EEventType.eQandA);
                        break;
                    case ELibraryType.eBoss:
                        GameApp.SendMsg.TriggerGridEffect(EEventType.eBoss);
                        break;
                }
            }
            else
            {
                if (type == ELibraryType.eBoss)
                {
                    QandAUI.RecordCloseCallback(RecordCloseCallback4OpenQandA);
                    GameApp.Instance.CommonHintDlg.OpenHintBox(CurActionPlayer.name + "开始挑战魔王！");
                }
            }
        }
        else
        {
            if (RecordCurRoleType == ERoleType.ePlayer_1 && type == ELibraryType.eBoss)
            {
                GameApp.Instance.CommonHintDlg.OpenHintBox("暂不处理机器人的Boss事件");
                CloseCallback();
                return;
            }

            QandAUI.Show(true, 0, type, CloseCallback);
        }
    }
    private ELibraryType RecordType4OpenQandA = ELibraryType.eNormal;
    private Action RecordCloseCallback4OpenQandA = null;
    public void OpenNormalQandA(uint QuestionID)
    {
        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
        {
            if (DefaultRule.PlayerIDToAccountID(GameApp.Instance.CurRoomPlayerLst[i].id) == GameApp.AccountID)
            {
                if (GameApp.Instance.CurRoomPlayerLst[i].win_num > 0)
                {
                    Debug.Log("已胜利，不处理答题！");
                    return;
                }
            }
        }

        StartCoroutine("DelayOpenNormalQandA", QuestionID);
    }
    public void OpenBossQandA(ulong PlayerID, List<uint> QuestionIDLst)
    {
        StartCoroutine(DelayOpenBossQandA(PlayerID, QuestionIDLst));
    }
    IEnumerator DelayOpenNormalQandA(uint QuestionID)
    {
        while (RecordCloseCallback4OpenQandA == null)
        {
            yield return new WaitForEndOfFrame();
        }
        QandAUI.Show(true, QuestionID, RecordType4OpenQandA, RecordCloseCallback4OpenQandA);
    }
    IEnumerator DelayOpenBossQandA(ulong PlayerID, List<uint> QuestionIDLst)
    {
        while (RecordCloseCallback4OpenQandA == null)
        {
            yield return new WaitForEndOfFrame();
        }
        QandAUI.Show(PlayerID, QuestionIDLst, RecordCloseCallback4OpenQandA);
    }

    public void OpenShop(Action CloseCallback)
    {
        /*switch (RecordCurRoleType)
        {
            case ERoleType.eProtagonist:
                ShopInFightUI.Show(true, CloseCallback);
                break;
            case ERoleType.eOpponent:
                StartCoroutine("RobotShopping", CloseCallback);
                break;
        }*/
    }

    public void ShowGotoBossHint(/*Action ShowOverCallback*/)
    {
        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            if (int.Parse(CurUIPlayerInfo.Score.text) >= BossTicketsScore)
            {
                if (DefaultRule.PlayerIDToAccountID(CurActionPlayer.id) == GameApp.AccountID)
                {
                    GameApp.SendMsg.TriggerGridEffect(EEventType.eToBoss);
                }
                else
                {
                    //ChessCtrl.StopMove();
                }
            }
        }
        else
        {
            switch (RecordCurRoleType)
            {
                case ERoleType.ePlayer_0:
                    {
                        if (GameApp.Instance.MainPlayerData.Score >= BossTicketsScore)
                        {
                            GameApp.Instance.SoundInstance.PlaySe("GotoBoss");
                            GotoBossHint.text = GameApp.Instance.MainPlayerData.Name + " 准备挑战魔王";
                            GotoBossHint.gameObject.SetActive(true);
                            ChessCtrl.StopMove();
                            StartCoroutine("DelayHideGotoBossHint"/*, ShowOverCallback*/);
                        }
                    }
                    break;
                case ERoleType.ePlayer_1:
                    {
                        GameApp.Instance.CommonHintDlg.OpenHintBox("暂不处理机器人经过传送点事件");
                    }
                    break;
            }
        }
    }
    public void GotoBossResult()
    {
        GameApp.Instance.SoundInstance.PlaySe("GotoBoss");
        GotoBossHint.text = StringBuilderTool.ToString(CurActionPlayer.name, "准备挑战魔王");
        GotoBossHint.gameObject.SetActive(true);
        ChessCtrl.StopMove();
        StartCoroutine("DelayHideGotoBossHint");
    }

    IEnumerator DelayHideGotoBossHint(/*Action ShowOverCallback*/)
    {
        yield return new WaitForSeconds(1.0f);

        GotoBossHint.gameObject.SetActive(false);

        iTween.ShakePosition(CurUIPlayerInfo.gameObject, new Vector3(0.05f, 0.03f, 0), 1.0f);
        GameApp.Instance.MainPlayerData.Score -= BossTicketsScore;

        ChessCtrl.JumpSwitchCheckerboard(CurRoleType, ECheckerboardType.eBoss, () =>
            {
                if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
                {
                    EnableNextPlayerThrowDice();
                }
                else
                {
                    ChoiceMoveObject(RecordCurRoleType);
                }
            });
    }

    public void ShowUnableGotoBossHint(Action CloseCallback)
    {
        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            if (DefaultRule.PlayerIDToAccountID(CurActionPlayer.id) == GameApp.AccountID)
            {
                if (int.Parse(CurUIPlayerInfo.Score.text) < BossTicketsScore)
                {
                    iTween.ShakePosition(CurUIPlayerInfo.gameObject, new Vector3(0.05f, 0.03f, 0), 1.0f);

                    UnableGotoBossHintObj.SetActive(true);
                    //CloseUnableGotoBossHintCallback = CloseCallback;
                    StartCoroutine("DelayHideUnableGotoBossHint");

                    if (CloseCallback != null)
                    {
                        CloseCallback();
                        CloseCallback = null;
                    }
                }
            }
            else
            {
                if (CloseCallback != null)
                {
                    CloseCallback();
                    CloseCallback = null;
                }
            }
        }
        else
        {
            switch (RecordCurRoleType)
            {
                case ERoleType.ePlayer_0:
                    {
                        if (GameApp.Instance.MainPlayerData.Score < BossTicketsScore)
                        {
                            iTween.ShakePosition(UIPlayerInfoLst[0].gameObject, new Vector3(0.05f, 0.03f, 0), 1.0f);

                            UnableGotoBossHintObj.SetActive(true);
                            //CloseUnableGotoBossHintCallback = CloseCallback;
                            StartCoroutine("DelayHideUnableGotoBossHint");

                            if (CloseCallback != null)
                            {
                                CloseCallback();
                                CloseCallback = null;
                            }
                        }
                    }
                    break;
                case ERoleType.ePlayer_1:
                    {
                        GameApp.Instance.CommonHintDlg.OpenHintBox("暂不处理机器人传送点事件");
                        CloseCallback();
                    }
                    break;
            }
        }
    }
    IEnumerator DelayHideUnableGotoBossHint()
    {
        int cd = 3;
        while (cd > 0)
        {
            UGHAutoHideDes.text = StringBuilderTool.ToString(cd, "秒");
            cd--;
            yield return new WaitForSeconds(1.0f);
        }
        UnableGotoBossHintObj.SetActive(false);        
    }

    public void ShowChallengeFailureHint(Action ShowOverCallback)
    {
        GameApp.Instance.SoundInstance.PlaySe("lost");
        GameApp.Instance.SoundInstance.PlaySe("ChallengeFailure");

        if (DefaultRule.PlayerIDToAccountID(CurActionPlayer.id) == GameApp.AccountID)
        {
            ChallengeFailureHintObj.gameObject.SetActive(true);
        }
        StartCoroutine("DelayHideChallengeFailureHint", ShowOverCallback);
    }
    IEnumerator DelayHideChallengeFailureHint(Action ShowOverCallback)
    {
        if (DefaultRule.PlayerIDToAccountID(CurActionPlayer.id) == GameApp.AccountID)
        {
            yield return new WaitForSeconds(2.0f);
            ChallengeFailureHintObj.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        ChessCtrl.JumpSwitchCheckerboard(CurRoleType, ECheckerboardType.eNormal, null);

        yield return new WaitForSeconds(0.5f);
        if (ShowOverCallback != null)
            ShowOverCallback();
    }

    public void ShowSettlement()
    {
        Settlement.SetActive(true);

        if (GameApp.Instance.PlayerData != null && !GameApp.Instance.IsFightingRobot)
        {
            for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
            {
                if (DefaultRule.PlayerIDToAccountID(GameApp.Instance.CurRoomPlayerLst[i].id) == GameApp.AccountID)
                {
                    TRTitle.text = StringBuilderTool.ToInfoString("恭喜 ", GameApp.Instance.CurRoomPlayerLst[i].name, " 获得");
                    break;
                }
            }

            GameApp.SendMsg.SetWinNum();
            /*if (GameApp.Instance.CurRoomPlayerLst.Count > 1)
            {
                AwardCardNum = 2;
            }
            else
            {
                AwardCardNum = 1;
            }*/
            //GameApp.SendMsg.QuitRoom();
        }
        else
        {
            LookOnBtn.isEnabled = false;

            TRTitle.text = StringBuilderTool.ToInfoString("恭喜 ", GameApp.Instance.MainPlayerData.Name, " 获得");
        }

        StartCoroutine("UpdateAutoOpenTreasureHint");

        GameApp.Instance.SoundInstance.PlayBgm("BGM_Settlement");
        GameApp.Instance.SoundInstance.PlaySe("win");

        DiceBtn.isEnabled = false;
        ClickDiceCountdown.text = "";

        StopCoroutine("DelayShowClickDiceHint");
        ClickDiceHint.SetActive(false);

        StopCoroutine("DiceCountdown");
#if UNITY_ANDROID
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("时间：", DateTime.Now.ToString());
        TalkingDataGA.OnEvent("挑战魔王成功", dic);
#endif
        if (GameApp.Instance.CurFightStageCfg.GroupID == 101)
        {
            GameApp.SendMsg.PVEFinish((uint)GameApp.Instance.CurFightStageCfg.ArrayIndex + 1);
        }
        else if (GameApp.Instance.CurFightStageCfg.GroupID == 102)
        {
            string key_zoo = StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_Zoo_StageProgress");
            PlayerPrefs.SetInt(key_zoo, PlayerPrefs.GetInt(key_zoo)+1);
        }

    }
    IEnumerator UpdateAutoOpenTreasureHint()
    {
        int cd = 10;
        while (cd > 0)
        {
            AutoOpenHint.text = StringBuilderTool.ToString(cd, "秒");
            cd--;
            yield return new WaitForSeconds(1.0f);
        }

        CreateAward();
    }

    private void CreateAward()
    {
        GameApp.Instance.SoundInstance.PlaySe("OpenTreasure");

        StartCoroutine("OpenTreasure");
    }
    //int AwardCardNum = 1;
    IEnumerator OpenTreasure()
    {
        int WinPlayerCnt = 0;
        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
        {
            if (GameApp.Instance.CurRoomPlayerLst[i].win_num > 0)
            {
                WinPlayerCnt++;
            }
        }
        if (WinPlayerCnt >= GameApp.Instance.CurRoomPlayerLst.Count)
            LookOnBtn.isEnabled = false;

        animTreasure.Play("Close to Open");
        yield return new WaitForSeconds(0.6f);

        TreasureRoot.SetActive(false);
        CardRoot.SetActive(true);

        if (GameApp.Instance.CurFightStageCfg == null)
        {
            GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox("关卡配置数据错误！不能显示通关奖励！");
            yield break;
        }

        List<int> CardIDs = new List<int>();
        int AwardCardNum = 1;
        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
        {
            if (DefaultRule.PlayerIDToAccountID(GameApp.Instance.CurRoomPlayerLst[i].id) == GameApp.AccountID)
            {
                AwardCardNum = (GameApp.Instance.CurRoomPlayerLst[i].win_num == 1 ? 2 : 1);//只有首个击败Boss的玩家才能获得2张卡牌
                break;
            }
        }
        switch(AwardCardNum)
        {
            default:
            case 1:
                int r = UnityEngine.Random.Range(0, GameApp.Instance.CurFightStageCfg.FixedAward.Count);
                CardIDs.Add(GameApp.Instance.CurFightStageCfg.FixedAward[r]);

                CRCard[0].transform.localPosition = new Vector3(0, 36, 0);
                CRCard[1].gameObject.SetActive(false);
                break;
            case 2:
                for (int i = 0; i < GameApp.Instance.CurFightStageCfg.FixedAward.Count; i++)
                {
                    CardIDs.Add(GameApp.Instance.CurFightStageCfg.FixedAward[i]);
                }
                CRCard[0].transform.localPosition = new Vector3(-140, 36, 0);
                CRCard[1].transform.localPosition = new Vector3(140, 36, 0);
                CRCard[1].gameObject.SetActive(true);
                break;
        }
        
        string CardDes = string.Empty;
        for (int i = 0; i < CardIDs.Count; i++)
        {
            CRCard[i].UnconditionalShow(CardIDs[i]);
            CardDes += StringBuilderTool.ToInfoString(CRCard[i].FullName(),(i != CardIDs.Count-1) ? "、" : "");
        }
        CRTitle.text = StringBuilderTool.ToString("恭喜您，获得 [FEE209]", CardDes, "[-]");
    }

    #region _机器人相关
    private void ChoiceMoveObject(ERoleType type)
    {
        if (GameApp.Instance.GetParameter("EnableAIRobot") == 0)
        {
            WaitClickDice();
            return;
        }

        if(CardRoot.activeSelf)
        {
            RecordCurRoleType = ERoleType.ePlayer_1;
            RoundSign.transform.localPosition = new Vector3(298, -230, 0);
            ChessCtrl.SwitchCamFocus(ERoleType.ePlayer_1);
            StartCoroutine("RobotMove");
            return;
        }

        StartCoroutine("WaitChoiceMoveObject", type);
    }
    IEnumerator WaitChoiceMoveObject(ERoleType type)
    {
        yield return new WaitForSeconds(1.0f);

        switch (type)
        {
            case ERoleType.ePlayer_0:
                RecordCurRoleType = ERoleType.ePlayer_1;
                RoundSign.transform.localPosition = new Vector3(298, -230, 0);
                RoundHint.text = "轮到 " + GameApp.Instance.AIRobotData.Name + " 掷骰子了";
                ChessCtrl.SwitchCamFocus(ERoleType.ePlayer_1);
                break;
            case ERoleType.ePlayer_1:
                RecordCurRoleType = ERoleType.ePlayer_0;
                RoundSign.transform.localPosition = new Vector3(298, -134, 0);
                RoundHint.text = "轮到 " + GameApp.Instance.MainPlayerData.Name + " 掷骰子了";
                ChessCtrl.SwitchCamFocus(ERoleType.ePlayer_0);
                break;
        }
        RoundHint.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        switch (RecordCurRoleType)
        {
            case ERoleType.ePlayer_0:
                WaitClickDice();
                break;
            case ERoleType.ePlayer_1:
                StartCoroutine("RobotMove");
                break;
        }

        RoundHint.gameObject.SetActive(false);
    }
    IEnumerator RobotMove()
    {
        int delay = UnityEngine.Random.Range(
            GameApp.Instance.GetParameter("RandomThrowDictBeginTime"), 
            GameApp.Instance.GetParameter("RandomThrowDictEndTime") + 1);
        yield return new WaitForSeconds(delay);

        GameApp.Instance.SoundInstance.PlaySe("ThrowDict");

        int cnt = UnityEngine.Random.Range(1, DiceCountAnim.Length);
        Animation ma = DiceCountAnim[Mathf.Abs(cnt)];
        ma.gameObject.SetActive(true);
        yield return new WaitForSeconds(ma.clip.length);
        ThrowDictResult.gameObject.SetActive(true);
        ThrowDictResult.text = "投掷结果：" + cnt + "点";
        yield return new WaitForSeconds(0.5f);
        ThrowDictResult.gameObject.SetActive(false);
        ma.gameObject.SetActive(false);

        //StartCoroutine("UpdateOpponentTopSteps", Mathf.Abs(cnt));
        ChessCtrl.Move(ERoleType.ePlayer_1, cnt,
            () =>
            {
                ChoiceMoveObject(ERoleType.ePlayer_1);
            });
    }
    /*IEnumerator UpdateOpponentTopSteps(int cnt)
    {
        OpponentTopSteps.gameObject.SetActive(true);
        while (cnt > 0)
        {
            OpponentTopSteps.text = (cnt--).ToString();
            yield return new WaitForSeconds(0.7f);
        }
        OpponentTopSteps.text = "";
        OpponentTopSteps.gameObject.SetActive(false);
    }*/

    IEnumerator RobotShopping(Action BuyOverCallback)
    {
        RobotShoppingHint.text = GameApp.Instance.AIRobotData.Name + " 购物中... 请稍后...";
        RobotShoppingHint.gameObject.SetActive(true);

#if UNITY_EDITOR
        //PrintDebugInfo.gameObject.SetActive(true);
        //PrintDebugInfo.Add("机器人开始购物");
        Debug.Log("机器人开始购物");
#endif
        bool buy = true;
        while (buy)
        {
            buy = false;
            foreach(KeyValuePair<int, ShopConfig> pair in CsvConfigTables.Instance.ShopCsvDic)
            {
                if (pair.Value.Type == 10)
                {
                    if (GameApp.Instance.AIRobotData.GoldCoin >= pair.Value.NowPriceValue)
                    {
                        GameApp.Instance.AIRobotData.GoldCoin -= pair.Value.NowPriceValue;
                        GameApp.Instance.AIRobotData.AddItem(pair.Value.ItemID, 1);
#if UNITY_EDITOR
                        //PrintDebugInfo.Add("机器人购买了" + pair.Value.GetName());
                        Debug.Log("机器人购买了" + pair.Value.GetName());
#endif
                        buy = true;
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }
#if UNITY_EDITOR
        //PrintDebugInfo.Add("机器人购物结束");
        Debug.Log("机器人购物结束");
#endif
        yield return new WaitForSeconds(1.0f);
        RobotShoppingHint.gameObject.SetActive(false);
        if (BuyOverCallback != null)
            BuyOverCallback();

//#if UNITY_EDITOR
        //yield return new WaitForSeconds(2.0f);
        //PrintDebugInfo.gameObject.SetActive(false);
//#endif
    }
    #endregion

    public void PrintScreenLog(string msg)
    {
        if (!PrintDebugInfo.gameObject.activeSelf)
            PrintDebugInfo.gameObject.SetActive(true);

        PrintDebugInfo.Add(msg);
        //CurUIPlayerInfo.Name.text += ("\n" + msg);
    }
}
