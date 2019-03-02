using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECheckerboardType//棋格类型
{
    eNull,
    eNormal,   //普通外圈
    eBoss      //Boss内圈
};
public enum ERoleType//角色类型
{
    ePlayer_0,  //1号玩家
    ePlayer_1,  //2号玩家
    ePlayer_2,  //3号玩家
    ePlayer_3,  //4号玩家
};

public class ChessControl : MonoBehaviour
{
    public MainFightCameraControl CameraControl;

    private ERoleType CurRoleType = ERoleType.ePlayer_0;

    public List<ChessPieces> TypeToTargetDic = new List<ChessPieces>();

    private ECheckerboardType[] CurCheckerboardType = new ECheckerboardType[4]
        {
            ECheckerboardType.eNormal,
            ECheckerboardType.eNormal,
            ECheckerboardType.eNormal,
            ECheckerboardType.eNormal
        };

    public GameObject NormalCheckerboard;
    private List<Square> NormalSquaresLst = new List<Square>();
    public GameObject BossCheckerboard;
    private List<Square> BossSquaresLst = new List<Square>();

    private bool EnableMove = true;
    public int[] CurSch4Mormal = new int[4] { 0, 0, 0, 0 };//当前进度
    public int[] CurSch4Boss = new int[4] { 0, 0, 0, 0 };
    private Action MoveEndCB = null;

    public AngelControl AngelCtrl;

    /*private Dictionary<int, EEventType> NormalSquaresEventDic = new Dictionary<int, EEventType>()
    {
        { 0,EEventType.eStart},
        { 1,EEventType.eQandA},
        { 2,EEventType.eQandA},
        { 3,EEventType.eQandA},
        { 4,EEventType.eMove},
        { 5,EEventType.eQandA},
        { 6,EEventType.eQandA},
        { 7,EEventType.eQandA},
        { 8,EEventType.eMove},
        { 9,EEventType.eQandA},
        {10,EEventType.eToBoss},
        {11,EEventType.eQandA},
        {12,EEventType.eQandA},
        {13,EEventType.eQandA},
        {14,EEventType.eMove},
        {15,EEventType.eQandA},
        {16,EEventType.eQandA},
        {17,EEventType.eQandA},
        {18,EEventType.eShop},
        {19,EEventType.eQandA},
        {20,EEventType.eQandA},
        {21,EEventType.eMove},
        {22,EEventType.eQandA},
        {23,EEventType.eQandA}
    };
    private Dictionary<int, EEventType> BossSquaresEventDic = new Dictionary<int, EEventType>()
    {
        { 0,EEventType.eNull},
        { 1,EEventType.eNull},
        { 2,EEventType.eBoss},
        { 3,EEventType.eNull}
    };*/

    void Awake()
    {
        //TypeToTargetDic[ERoleType.eProtagonist] = ProtagonistNode;
        //TypeToTargetDic[ERoleType.eOpponent] = OpponentNode;

        //构建棋格列表
        for (int i = 0; i < NormalCheckerboard.transform.childCount; i++)
        {
            Square s = NormalCheckerboard.transform.GetChild(i).GetComponent<Square>();
            //s.EventType = NormalSquaresEventDic[i];
            NormalSquaresLst.Add(s);
        }
        for (int i = 0; i < BossCheckerboard.transform.childCount; i++)
        {
            Square s = BossCheckerboard.transform.GetChild(i).GetComponent<Square>();
            //s.EventType = BossSquaresEventDic[i];
            BossSquaresLst.Add(s);
        }
    }

    void Start()
    {
        /*if (GameApp.Instance.CurFightStageCfg != null)
        {
            AngelCtrl.Create(NormalCheckerboard.transform.GetChild(UnityEngine.Random.Range(GameApp.Instance.CurFightStageCfg.AngelRandomAppearMinGrid, GameApp.Instance.CurFightStageCfg.AngelRandomAppearMaxGrid)));
        }
        else
        {
            AngelCtrl.Create(NormalCheckerboard.transform.GetChild(UnityEngine.Random.Range(15, 18)));
        }*/
    }

    public void InitChessPieces(List<uint> RoleIDs)
    {
        for (int i = 0; i < TypeToTargetDic.Count; i++)
        {
            if (i < RoleIDs.Count)
                TypeToTargetDic[i].Init(RoleIDs[i]);
            else
                TypeToTargetDic[i].gameObject.SetActive(false);
        }
    }

/*#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            //JumpSwitchCheckerboard(ECheckerboardType.eNormal);
            SwitchCamFocus(ERoleType.eProtagonist);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            SwitchCamFocus(ERoleType.eOpponent);
        }
    }
#endif*/

    static public Action<Square> create;
    public void CreateSquaresHintLab(GameObject labUnit)
    {
        create = (s) =>
        {
            GameObject newUnit = NGUITools.AddChild(labUnit.transform.parent.gameObject, labUnit);
            newUnit.SetActive(true);
            newUnit.GetComponent<UIFollowTarget>().target = s.transform;
            UILabel lab = newUnit.GetComponentInChildren<UILabel>();
            s.SetHint(ref lab);
        };

        for (int i = 0; i < NormalSquaresLst.Count; i++)
        {
            create(NormalSquaresLst[i]);
        }
        for (int i = 0; i < BossSquaresLst.Count; i++)
        {
            create(BossSquaresLst[i]);
        }
    }

    #region _控制角色移动
    public void Move(ERoleType _type, int _num, Action _end)
    {
        if (!EnableMove)
            return;

        CurRoleType = _type;

        ECheckerboardType cType = CurCheckerboardType[(int)CurRoleType];

        MoveEndCB = _end;
        EnableMove = false;

        if (_num > 0)
        {
            switch (cType)
            {
                case ECheckerboardType.eNormal:
                    Advance(_num);
                    break;
                case ECheckerboardType.eBoss:
                    AdvanceOnBossCheckerboard(_num);
                    break;
            }
        }
        else if (_num < 0)
        {
            switch (cType)
            {
                case ECheckerboardType.eNormal:
                    Retreat(_num);
                    break;
                case ECheckerboardType.eBoss:

                    break;
            }
        }
        else
        {
            EnableMove = true;

            if (MoveEndCB != null)
            {
                MoveEndCB();
                MoveEndCB = null;
            }
        }

    }
    public void StopMove()
    {
        StopCoroutine("StageMove");
        EnableMove = true;
    }

    //获得当前移动的角色
    public ChessPieces GetRole()
    {
        return TypeToTargetDic[(int)CurRoleType];
    }
    //获得当前进度
    int GetSchedule()
    {
        switch (CurCheckerboardType[(int)CurRoleType])
        {
            case ECheckerboardType.eNormal:
                return CurSch4Mormal[(int)CurRoleType];
            case ECheckerboardType.eBoss:
                return CurSch4Boss[(int)CurRoleType];
        }
                
        return 0;
    }
    //更改进度
    void ChangeSchedule(int value)
    {
        switch (CurCheckerboardType[(int)CurRoleType])
        {
            case ECheckerboardType.eNormal:
                CurSch4Mormal[(int)CurRoleType] = value;
                break;
            case ECheckerboardType.eBoss:
                CurSch4Boss[(int)CurRoleType] = value;
                break;
        }
    }

    //前进
    void Advance(int GridNum)
    {
        Transform[] pathTrans = new Transform[GridNum + 1];

        int _idx = 0;
        int _schedule = GetSchedule();
        for (int i = 0; i < GridNum + 1; i++)
        {
            _idx = _schedule + i;
            if (_idx >= NormalSquaresLst.Count)
            {
                _idx -= NormalSquaresLst.Count;
            }
            pathTrans[i] = NormalSquaresLst[_idx].transform;
        }

        StartCoroutine("StageMove", pathTrans);
    }
    void AdvanceOnBossCheckerboard(int GridNum)
    {
        Transform[] pathTrans = new Transform[GridNum + 1];

        int _idx = 0;
        int _schedule = GetSchedule();
        for (int i = 0; i < GridNum + 1; i++)
        {
            _idx = _schedule + i;
            while (_idx >= BossSquaresLst.Count)
            {
                _idx -= BossSquaresLst.Count;
            }
            pathTrans[i] = BossSquaresLst[_idx].transform;
        }

        StartCoroutine("StageMove", pathTrans);
    }
    //后退
    void Retreat(int GridNum)
    {
        GridNum = Math.Abs(GridNum);

        Transform[] pathTrans = new Transform[GridNum + 1];

        int _idx = 0;
        int _schedule = GetSchedule();
        for (int i = 0; i < GridNum + 1; i++)
        {
            _idx = _schedule - i;
            if (_idx < 0)
            {
                _idx += NormalSquaresLst.Count;
            }
            pathTrans[i] = NormalSquaresLst[_idx].transform;
        }

        StartCoroutine("StageMove", pathTrans);
    }
    //内圈外圈之间跳跃
    private Action JumpOverCallback = null;
    public void JumpSwitchCheckerboard(ERoleType Rtype, ECheckerboardType Ctype, Action Callback)
    {
        Transform[] pathTrans = new Transform[2];
        switch (Ctype)
        {
            case ECheckerboardType.eNormal:
                GameApp.Instance.SoundInstance.PlayBgm("BGM_MainGame_Normal");

                pathTrans[0] = BossSquaresLst[GetSchedule()].transform;
                pathTrans[1] = NormalSquaresLst[0].transform;
                break;
            case ECheckerboardType.eBoss:
                GameApp.Instance.SoundInstance.PlayBgm("BGM_MainGame_Boss");

                pathTrans[0] = NormalSquaresLst[GetSchedule()].transform;
                pathTrans[1] = BossSquaresLst[0].transform;                
                break;
        }
        CurCheckerboardType[(int)Rtype] = Ctype;

        ChangeSchedule(0);

        JumpOverCallback = Callback;

        StartCoroutine(Teleport(pathTrans, Callback));
    }
    IEnumerator StageMove(Transform[] pathTrans)
    {
        for (int i = 0; i < pathTrans.Length - 1; i++)
        {
            GameApp.Instance.SoundInstance.PlaySe("Jump");

            Vector3 targetPos = pathTrans[i + 1].localPosition;
            bool superposition = RoleAvoid(pathTrans[i + 1]);
            if(superposition)
            {
                targetPos += GetRole().GetToSidePos(CurRoleType, pathTrans[i + 1]);
            }

            Hashtable args = new Hashtable();
            args.Add("time", 0.5f);
            args.Add("movetopath", false);
            args.Add("orienttopath", true);
            args.Add("position", targetPos);
            args.Add("islocal", true);
            args.Add("easeType", iTween.EaseType.linear);
            args.Add("onstart", "MoveStart");
            args.Add("onstarttarget", gameObject);
            args.Add("onstartparams", i + 1 < pathTrans.Length - 1);
            args.Add("oncomplete", "MoveEnd");
            args.Add("oncompletetarget", gameObject);
            args.Add("oncompleteparams", pathTrans[i + 1]);
            iTween.MoveTo(GetRole().gameObject, args);
            yield return new WaitForSeconds(0.7f);
        }

        Square EndSquare = pathTrans[pathTrans.Length - 1].GetComponent<Square>();

        Action end = () =>
        {
            EnableMove = true;

            EndSquare.Do(() =>
                {
                    if (MoveEndCB != null)
                    {
                        MoveEndCB();
                        MoveEndCB = null;
                    }
                });
        };

        if (!AngelCtrl.Detection(EndSquare.transform, () =>
            {
                end();
            }))
        {
            end();
        }
    }
    //移动开始
    void MoveStart(bool normal)
    {
        if (normal)
        {
            CreateNormalTDEff = true;
            GetRole().Jump();
        }
        else
        {
            CreateNormalTDEff = false;
            GetRole().BigJump();
        }
    }
    //移动结束
    void MoveEnd(Transform pathTran)
    {
        Square s = pathTran.GetComponent<Square>();
        s.Sink();

        CreateTouchDownEffect(s.Model.transform);

        ChangeSchedule(s.Index);

        GetRole().SetLocationGrid(pathTran);
    }
    //瞬移
    IEnumerator Teleport(Transform[] pathTrans, Action Callback)
    {
        GameApp.Instance.SoundInstance.PlaySe("Teleport");

        GameObject role = GetRole().gameObject;
        GameObject shunyiEff = Resources.Load<GameObject>("Prefabs/Effect/Teleport");
        if (role == null || shunyiEff == null)
            yield break;

        GameObject eff1 = GameObject.Instantiate(shunyiEff);
        eff1.transform.parent = pathTrans[0];
        eff1.transform.localPosition = Vector3.zero;
        eff1.transform.localEulerAngles = new Vector3(-90, 0, 0);
        eff1.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(0.2f);
        TweenScale.Begin(role, 0.2f, new Vector3(0.1f, 2, 0.1f));
        //yield return new WaitForSeconds(0.2f);

        GameObject eff2 = GameObject.Instantiate(shunyiEff);
        eff2.transform.parent = pathTrans[1];
        eff2.transform.localPosition = Vector3.zero;
        eff2.transform.localEulerAngles = new Vector3(-90, 0, 0);
        eff2.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(0.2f);
        role.transform.position = pathTrans[1].position;
        TweenScale.Begin(role, 0.2f, Vector3.one);
        yield return new WaitForSeconds(0.5f);

        Square EndSquare = pathTrans[1].GetComponent<Square>();
        if (EndSquare.EventType == EEventType.eBoss)
        {
            GameApp.Instance.FightUI.OpenQandA(ELibraryType.eBoss, MoveEndCB);
        }
        else
        {
            if (JumpOverCallback != null)
                JumpOverCallback();
        }
    }
    private bool RoleAvoid(Transform trans)
    {
        /*switch (CurRoleType)
        {
            case ERoleType.eProtagonist:
                {
                    ChessPieces cp = TypeToTargetDic[(int)ERoleType.eOpponent];
                    if(cp.IsOnThisGrid(trans))
                    {
                        cp.ToSide(ERoleType.eOpponent);
                        return true;
                    }
                }
                break;
            case ERoleType.eOpponent:
                {
                    ChessPieces cp = TypeToTargetDic[(int)ERoleType.eProtagonist];
                    if (cp.IsOnThisGrid(trans))
                    {
                        cp.ToSide(ERoleType.eProtagonist);
                        return true;
                    }
                }
                break;
        }*/
        return false;
    }
    #endregion

    #region _切换相机焦点
    public void SwitchCamFocus(ERoleType type)
    {
        CameraControl.CamTarget.Target = TypeToTargetDic[(int)type].transform;
    }
    #endregion

    bool CreateNormalTDEff = true;
    private void CreateTouchDownEffect(Transform Parent)
    {
        GameApp.Instance.SoundInstance.PlaySe(CreateNormalTDEff ? "FallGround" : "FallGroundBig");

        GameObject luodiEff = Resources.Load<GameObject>("Prefabs/Effect/" + (CreateNormalTDEff ? "TouchDown_Normal" : "TouchDown_Big"));
        if (luodiEff != null)
        {
            GameObject eff = GameObject.Instantiate(luodiEff);
            eff.transform.parent = Parent;
            eff.transform.localPosition = new Vector3(0, 0.5f, 0);
            eff.transform.localEulerAngles = Vector3.zero;
            eff.transform.localScale = Vector3.one;
        }
    }
}
