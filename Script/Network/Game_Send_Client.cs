using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;
using common;

public class Game_Send_Client : MonoBehaviour
{
    #region _Mono
    void Awake()
    {

    }

    void Start()
    {
        GameApp.SendMsg = this;

        if (GameApp.RecvMsg == null)
            GameApp.RecvMsg = new Game_Recv_Client();
    }

    /// <summary>
    /// 结束游戏时关闭套接字
    /// </summary>
    void OnDisable()
    {
        /*if (GameApp.SocketClient_User != null)
        {
            GameApp.SocketClient_User.Close(true);
        }*/

        if (GameApp.SocketClient_Game != null)
        {
            GameApp.SocketClient_Game.Close(true);
        }
    }
#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        foreach (SocketClient sClient in SocketClient.SocketDic.Values)
        {
            if (sClient != null) sClient.Close();
        }
    }
#endif
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SocketManager.Instance.ForceReconnect(true);
        //    StartWaitUI();
        //}

        if (GameApp.RecvMsg == null)
            return;

        SocketManager.Instance.ReconnectServer();
        GameApp.RecvMsg.BegainManagePacket();

        /*if (GameApp.SocketClient_User != null)
        {
            GameApp.SocketClient_User.ClientTimer();
        }*/

        if (GameApp.SocketClient_Game != null)
        {
            GameApp.SocketClient_Game.ClientTimer();
        }
    }    
    #endregion

    #region _WaitUI 等待通讯结束
    private static GameObject _WaitPoint = null;
    public static GameObject WaitPoint
    {
        get
        {
            if (_WaitPoint == null)
            {
                _WaitPoint = GameObject.Find("Singlton/CommunalDlg/Camera/Anchor-BottomRight/WaitPoint");
            }
            return _WaitPoint;
        }
    }

    private int CommunicatingCount = 0;
    private GameObject _CommunicatingObj = null;
    public GameObject CommunicatingObj
    {
        get
        {
            if (_CommunicatingObj == null && WaitPoint != null)
            {
                _CommunicatingObj = GameObject.Instantiate(Resources.Load("Prefabs/UI/UI_Wait")) as GameObject;
                _CommunicatingObj.name = _CommunicatingObj.name.Substring(0, _CommunicatingObj.name.Length - 7);
                MyTools.BindChild(WaitPoint.transform, _CommunicatingObj);
                _CommunicatingObj.SetActive(false);
            }
            return _CommunicatingObj;
        }
    }
    public void DestoryWait()
    {
        StartCoroutine(DelayDestoryWaitUI());
    }

    IEnumerator DelayDestoryWaitUI()
    {
        yield return new WaitForSeconds(0.2f);
        EndWaitUI();
    }
    public void StartWaitUI()
    {
        
        if (WaitPoint == null)
            return;

        if (CommunicatingObj == null)
            return;

        CommunicatingCount++;
        if (CommunicatingObj != null)
        {
            CommunicatingObj.SetActive(true);
            //if (!GameApp.LoadingObjList.Contains(CommunicatingObj))
            //    GameApp.LoadingObjList.Add(CommunicatingObj);
        }
    }

    public void EndWaitUI()
    {
        if (CommunicatingCount <= 0)
            return;

        CommunicatingCount--;
        if (CommunicatingCount == 0 && CommunicatingObj != null)
        {
            CommunicatingObj.SetActive(false);

            //GameApp.LoadingObjList.Clear();
        }
    }
    #endregion

    #region _SendProtocol 发包操作

    #region _Base
    /// <summary>
    /// 发包
    /// </summary>
    private void Send<T>(T ProtocolPacket, SocketClient curSocketClient, int ProtocolType, bool isLoading = true) where T : IMessage
    {
        if (curSocketClient == null)
        {
            Debug.LogError(StringBuilderTool.ToInfoString(((MsgType)ProtocolType).ToString()," curSocketClient is null"));
            return;
        }

        if (!curSocketClient.IsConnected)
        {
//#if UNITY_EDITOR
            Debug.Log(StringBuilderTool.ToString(curSocketClient.ClaName, " Send_NotConnectInMyTool 与服务器的连接已断开", ((MsgType)ProtocolType).ToString()));
//#endif
            SocketManager.Instance.AddReconnectSocket(StringBuilderTool.ToInfoString(curSocketClient.ClaName, " 与服务器的连接失败！请稍后"), curSocketClient);
            return;
        }

        curSocketClient.StartSendMessage<T>(ProtocolPacket, ProtocolType);

        if (isLoading)
        {
            StartWaitUI();
        }
    }
    #endregion

    #region _GM
    public void GMOrder(string order)
    {
        //AddExp xxx
        //AddItem xxx xxx

        Msg_Client2Logic_GM_Req packet = new Msg_Client2Logic_GM_Req();
        packet.m_gm_cmd = order;
        Send<Msg_Client2Logic_GM_Req>(packet, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_GM_Req, false);
    }
    public void SetGM(int GMSign)
    {
        Msg_Client2Logic_SetGM_Req sgm = new Msg_Client2Logic_SetGM_Req();
        sgm.gm = GMSign;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("设置GM标记：", GMSign));
#endif
        Send<Msg_Client2Logic_SetGM_Req>(sgm, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_SetGM_Req);
    }
    #endregion

    #region _Login
    /// <summary> 创建账号服连接 </summary>
    /*public void CreateUserServerSocket()
    {
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("连接账号服 IP:", Const.AccountServerIP, " 端口:", Const.AccountServerPort));
#endif
        GameApp.SocketClient_User = SocketClient.SocketConnect(Const.AccountServerIP, Const.AccountServerPort, "SocketClient_User");
    }*/
    /// <summary> 登录 </summary>
    /*public void Login(string account, string password)
    {
        Msg_Client2Account_Login_Req login = new Msg_Client2Account_Login_Req();
        login.m_account_name = account;
        login.m_account_key = password;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("账号:", login.m_account_name, " 密码:", login.m_account_key));
#endif
        Send<Msg_Client2Account_Login_Req>(login, GameApp.SocketClient_User, (int)MsgType.enum_Msg_Client2Account_Login_Req);
    }*/
    /// <summary> 创建游戏服连接 </summary>
    public void CreateGameServerSocket()
    {
#if UNITY_EDITOR
        Const.GameServerIP = "58.246.123.230";//编辑器模式下连接测试服务器
#endif
        Debug.Log(StringBuilderTool.ToString("连接游戏服 IP:", Const.GameServerIP, " 端口:", Const.GameServerPort));
        GameApp.SocketClient_Game = SocketClient.SocketConnect(Const.GameServerIP, Const.GameServerPort, "SocketClient_Game");
    }
    /// <summary> 连接游戏服 </summary>
    public void ConnectGameServer()
    {
        GameApp.ServerIndex = 1;
        Msg_Client2Gate_Connect_Req center = new Msg_Client2Gate_Connect_Req();
        center.m_account_id = GameApp.AccountID;
        center.m_key = GameApp.ServerKey;
        center.m_server_id = (uint)GameApp.ServerIndex;
        center.m_ip = MyTools.GetIPAddress();
        center.m_imei = SystemInfo.deviceModel;
//#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("登录游戏服：账号ID[", GameApp.AccountID, "]，授权码[", GameApp.ServerKey, "]，服务器ID[", GameApp.ServerIndex, "]"));
//#endif
        Send<Msg_Client2Gate_Connect_Req>(center, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Gate_Connect_Req);
    }
    #endregion

    #region _Player
    public void CreatePlayer(string PlayeName)
    {
        Msg_Client2Logic_Create_Player_Req cp = new Msg_Client2Logic_Create_Player_Req();
        cp.m_name = PlayeName;
//#if UNITY_EDITOR
        Debug.Log("发送创建玩家");
//#endif
        Send<Msg_Client2Logic_Create_Player_Req>(cp, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_Create_Player_Req);
    }
    public void CheckPlayerName(string PlayeName)
    {
        /*
         3.Player中加一个宠物名称
            和m_player_name（玩家角色名称）一样的
            配套的一系列协议都一样：
            enum_Msg_Client2Logic_CheckPlayerName_Req，检测
            enum_Msg_Client2Logic_SetPlayerName_Req，设置
         4.Player中加一个宠物ID
         
        
1.PVE_Room_Player中增加两个变量
  private ProtoMemberUInt32 _WinNum;    //获胜序号
  private ProtoMemberUInt32 _GameState; //游戏状态
 
2.新加一个协议，设置获胜序号
  客服端发送：
  message Msg_Client2Logic_PVE_SetWinNum_Room_Req
  {
      int WinNum;	// 获胜序号（对应PVE_Room_Player中的_WinNum）
  }
  服务器广播已有的enum_Msg_Logic2Client_PVE_Player_Info_Room_Broadcast

3.新加一个协议，设置游戏状态
  客服端发送：
  message Msg_Client2Logic_PVE_SetGameState_Room_Req
  {
      int GameState;	// 游戏状态（对应PVE_Room_Player中的_GameState）
  }
  服务器广播已有的enum_Msg_Logic2Client_PVE_Player_Info_Room_Broadcast
 
4.enum_Msg_Logic2Client_PVE_Devil_Question_Room_Res魔王的题目列表回复协议要改成广播，
  数据结构中增加玩家ID，即
  Msg_Logic2Client_PVE_Devil_Question_Room_Broadcast
  {
      private ProtoMemberUInt64 _player_id;	//玩家id （新增）
      private ProtoMemberUInt32List _questions;	（原有）
  }
          
          
          
          
         */
        Msg_Client2Logic_CheckPlayerName_Req cpn = new Msg_Client2Logic_CheckPlayerName_Req();
        cpn.m_player_name = PlayeName;
#if UNITY_EDITOR
        Debug.Log("检测玩家名");
#endif
        Send<Msg_Client2Logic_CheckPlayerName_Req>(cpn, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_CheckPlayerName_Req);
    }
    public void SetPlayerName(string PlayeName)
    {
        Msg_Client2Logic_SetPlayerName_Req cpn = new Msg_Client2Logic_SetPlayerName_Req();
        cpn.m_player_name = PlayeName;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("设置玩家名", PlayeName));
#endif
        Send<Msg_Client2Logic_SetPlayerName_Req>(cpn, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_SetPlayerName_Req);
    }
    public void GetPlayerData()
    {
        Msg_Client2Logic_Player_Data_Req pd = new Msg_Client2Logic_Player_Data_Req();
        pd.m_imei = SystemInfo.deviceModel;
//#if UNITY_EDITOR
        Debug.Log("发送获取玩家数据");
//#endif
        Send<Msg_Client2Logic_Player_Data_Req>(pd, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_Player_Data_Req);
    }
    public void SetAvatar(uint AvatarID)
    {
        Msg_Client2Logic_Change_Avatar_Req ca = new Msg_Client2Logic_Change_Avatar_Req();
        ca.id = AvatarID;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送设置换装ID", AvatarID));
#endif
        Send<Msg_Client2Logic_Change_Avatar_Req>(ca, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_Change_Avatar_Req);
    }
    public void SetTaskData(string TaskInfoStr)
    {
        Msg_Client2Logic_SaveTaskInfo_Req sti = new Msg_Client2Logic_SaveTaskInfo_Req();
        sti.taskInfo = TaskInfoStr;
#if UNITY_EDITOR
        Debug.Log("发送任务数据");
#endif
        Send<Msg_Client2Logic_SaveTaskInfo_Req>(sti, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_SaveTaskInfo_Req);
    }
    public void SaveContestMemInfo(string ContestMemInfo)
    {
        Msg_Client2Logic_SaveContestMemInfo_Req scmi = new Msg_Client2Logic_SaveContestMemInfo_Req();
        scmi.memInfo = ContestMemInfo;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("保存赛事备忘数据 ", ContestMemInfo));
#endif
        Send<Msg_Client2Logic_SaveContestMemInfo_Req>(scmi, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_SaveContestMemInfo_Req);
    }
    public void GetServerTime()
    {
#if UNITY_EDITOR
        Debug.Log("获得服务器当前时间");
#endif
        Send<Msg_Client2Logic_ServerTime_Req>(new Msg_Client2Logic_ServerTime_Req(), GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_ServerTime_Req);
    }
    #endregion

    #region _Mail
    public void OpenMail(uint ID)
    {
        Msg_Client2Logic_OpenMail_Req om = new Msg_Client2Logic_OpenMail_Req();
        om.id = ID;
#if UNITY_EDITOR
        Debug.Log("发送打开邮件");
#endif
        Send<Msg_Client2Logic_OpenMail_Req>(om, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_OpenMail_Req);
    }
    public void DeleteMail(uint ID)
    {
        Msg_Client2Logic_DeleteMail_Req om = new Msg_Client2Logic_DeleteMail_Req();
        om.id = ID;
#if UNITY_EDITOR
        Debug.Log("发送删除邮件");
#endif
        Send<Msg_Client2Logic_DeleteMail_Req>(om, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_DeleteMail_Req);
    }
    #endregion

    #region _Gacha
    public void Gacha(uint type)
    {
        Msg_Client2Logic_Gacha_Req g = new Msg_Client2Logic_Gacha_Req();
        g.type = type;
#if UNITY_EDITOR
        Debug.Log("发送扭蛋");
#endif
        Send<Msg_Client2Logic_Gacha_Req>(g, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_Gacha_Req);
    }
    #endregion

    #region _Rank
    public void GetRank(RankType type)
    {
        Msg_Client2Logic_Rank_Req r = new Msg_Client2Logic_Rank_Req();
        r.type = (uint)type;
        r.from = 1;
        r.to = 100;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送获取排行榜数据：", type, " ", r.from, " ", r.to));
#endif
        Send<Msg_Client2Logic_Rank_Req>(r, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_Rank_Req);
    }
    #endregion

    #region _Compose
    public void Compose(ComposeType type,uint ID)
    {
        Msg_Client2Logic_Compose_Req c = new Msg_Client2Logic_Compose_Req();
        c.type = (uint)type;
        c.id = ID;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送合成：", type, " ", ID));
#endif
        Send<Msg_Client2Logic_Compose_Req>(c, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_Compose_Req);
    }
    #endregion

    #region _Room
    public void EnableEnterStage(uint StageID)
    {
        Msg_Client2Logic_PVE_Enter_Req e = new Msg_Client2Logic_PVE_Enter_Req();
        e.m_id = StageID;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送检测是否允许进入关卡：", StageID));
#endif
        Send<Msg_Client2Logic_PVE_Enter_Req>(e, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Enter_Req);
    }
    public void CreateRoom(uint StageID)
    {
        Msg_Client2Logic_PVE_Create_Room_Req c = new Msg_Client2Logic_PVE_Create_Room_Req();
        c.m_id = StageID;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送创建房间：", StageID));
#endif
        Send<Msg_Client2Logic_PVE_Create_Room_Req>(c, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Create_Room_Req);
    }
    public void JoinRoom(uint StageID)
    {
        Msg_Client2Logic_PVE_Join_Room_Req j = new Msg_Client2Logic_PVE_Join_Room_Req();
        j.m_id = StageID;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送加入房间：", StageID));
#endif
        Send<Msg_Client2Logic_PVE_Join_Room_Req>(j, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Join_Room_Req);
    }
    public void QuitRoom()
    {
#if UNITY_EDITOR
        Debug.Log("发送退出房间");
#endif
        Send<Msg_Client2Logic_PVE_Quit_Room_Req>(new Msg_Client2Logic_PVE_Quit_Room_Req(), GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Quit_Room_Req);
    }
    public void ReadyInRoom(bool IsReady)
    {
        Msg_Client2Logic_PVE_Ready_Room_Req r = new Msg_Client2Logic_PVE_Ready_Room_Req();
        r.is_ready = IsReady;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送在房间中准备：", IsReady));
#endif
        Send<Msg_Client2Logic_PVE_Ready_Room_Req>(r, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Ready_Room_Req);
    }
    public void StartGame()
    {
#if UNITY_EDITOR
        Debug.Log("发送开始游戏");
#endif
        Send<Msg_Client2Logic_PVE_Start_Game_Room_Req>(new Msg_Client2Logic_PVE_Start_Game_Room_Req(), GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Start_Game_Room_Req);
    }
    public void Throw(uint DiceNum)
    {
        Msg_Client2Logic_PVE_Throw_Room_Req t = new Msg_Client2Logic_PVE_Throw_Room_Req();
        t.dice_num = DiceNum;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送投掷筛子：", DiceNum));
#endif
        Send<Msg_Client2Logic_PVE_Throw_Room_Req>(t, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Throw_Room_Req);
    }
    public void TriggerGridEffect(EEventType EventType)
    {
        Msg_Client2Logic_PVE_Trigger_Effect_Room_Req te = new Msg_Client2Logic_PVE_Trigger_Effect_Room_Req();
        switch (EventType)
        {
            default:
            case EEventType.eNull:
                break;
            case EEventType.eStart:
                te.type = RoomPosType.RoomPosType_Start;
                break;
            case EEventType.eQandA:
                te.type = RoomPosType.RoomPosType_Question;
                break;
            case EEventType.eMove:
                te.type = RoomPosType.RoomPosType_Move;
                break;
            case EEventType.eShop:
                break;
            case EEventType.eToBoss:
                te.type = RoomPosType.RoomPosType_ToInnerRound;
                break;
            case EEventType.eBoss:
                te.type = RoomPosType.RoomPosType_DevilQuestion;
                break;
        }
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送触发格子事件：", te.type));
#endif
        Send<Msg_Client2Logic_PVE_Trigger_Effect_Room_Req>(te, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Trigger_Effect_Room_Req);
    }
    public void BossQuestionAnswerFailure()
    {
        Msg_Client2Logic_PVE_Trigger_Effect_Room_Req te = new Msg_Client2Logic_PVE_Trigger_Effect_Room_Req();
        te.type = RoomPosType.RoomPosType_DevilQuestionToInner;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送魔王答题失败回到内圈"));
#endif
        Send<Msg_Client2Logic_PVE_Trigger_Effect_Room_Req>(te, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Trigger_Effect_Room_Req);
    }
    public void BossQuestionAnswerSucceed()
    {
        Msg_Client2Logic_PVE_Trigger_Effect_Room_Req te = new Msg_Client2Logic_PVE_Trigger_Effect_Room_Req();
        te.type = RoomPosType.RoomPosType_DevilAnswerRight;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送魔王答题成功"));
#endif
        Send<Msg_Client2Logic_PVE_Trigger_Effect_Room_Req>(te, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Trigger_Effect_Room_Req);
    }
    public void AnswerQuestion(uint questionID,bool isRight)
    {
        Msg_Client2Logic_PVE_Answer_Question_Room_Req aq = new Msg_Client2Logic_PVE_Answer_Question_Room_Req();
        aq.question_id = questionID;
        aq.answer = isRight;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送问题答案"));
#endif
        Send<Msg_Client2Logic_PVE_Answer_Question_Room_Req>(aq, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Answer_Question_Room_Req);

#if UNITY_ANDROID
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("问题ID：", questionID);
        dic.Add("结果：", isRight);
        TalkingDataGA.OnEvent("回答问题", dic);
#endif
    }
    public void SetGameState(uint GameState)
    {
        Msg_Client2Logic_PVE_SetGameState_Room_Req sgs = new Msg_Client2Logic_PVE_SetGameState_Room_Req();
        sgs.game_state = GameState;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送游戏状态：", GameState));
#endif
        Send<Msg_Client2Logic_PVE_SetGameState_Room_Req>(sgs, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_SetGameState_Room_Req);
    }
    public void SetWinNum()
    {
        uint CurMaxWinNum = 0;
        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
        {
            if(CurMaxWinNum < GameApp.Instance.CurRoomPlayerLst[i].win_num)
            {
                CurMaxWinNum = GameApp.Instance.CurRoomPlayerLst[i].win_num;
            }
        }

        Msg_Client2Logic_PVE_SetWinNum_Room_Req swn = new Msg_Client2Logic_PVE_SetWinNum_Room_Req();
        swn.win_num = (CurMaxWinNum + 1);
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送获胜序号：", swn.win_num));
#endif
        Send<Msg_Client2Logic_PVE_SetWinNum_Room_Req>(swn, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_SetWinNum_Room_Req);
    }
    public void LoadState(int StateIndex)
    {
        Msg_Client2Logic_LoadState_Req ls = new Msg_Client2Logic_LoadState_Req();
        ls.loadState = StateIndex;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送加载状态：", StateIndex));
#endif
        Send<Msg_Client2Logic_LoadState_Req>(ls, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_LoadState_Req);
    }
    public void PVEFinish(uint ChapterID, uint Result = 1)
    {
        Msg_Client2Logic_PVE_Finish_Req pveF = new Msg_Client2Logic_PVE_Finish_Req();
        pveF.m_id = ChapterID;
        pveF.m_result = Result;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("发送关卡通关 ChapterID = ", ChapterID, "  Result = ", Result));
#endif
        Send<Msg_Client2Logic_PVE_Finish_Req>(pveF, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_PVE_Finish_Req);
    }
    #endregion

    #region _Travel
    public void BuyItem(uint ItemID)
    {
        Msg_Client2Logic_Buy_Shop_Req buy = new Msg_Client2Logic_Buy_Shop_Req();
        buy.id = ItemID;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("购买道具：", ItemID));
#endif
        Send<Msg_Client2Logic_Buy_Shop_Req>(buy, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_Buy_Shop_Req);
    }
    public void PutItem(ulong ID, uint OldPos, uint NewPos)
    {
        Msg_Client2Logic_Put_Bag_Req put = new Msg_Client2Logic_Put_Bag_Req();
        put.id = ID;
        put.oldPos = OldPos;
        put.newPos = NewPos;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("放置道具：", ID, "由位置", (PosType)OldPos, "放置到", (PosType)NewPos));
#endif
        Send<Msg_Client2Logic_Put_Bag_Req>(put, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_Put_Bag_Req);
    }
    public void GetOfflineCandy(int PickNum)
    {
        Msg_Client2Logic_Get_Offline_Candy_Req goc = new Msg_Client2Logic_Get_Offline_Candy_Req();
        goc.nums = (uint)PickNum;
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("获取离线糖果：", PickNum));
#endif
        Send<Msg_Client2Logic_Get_Offline_Candy_Req>(goc, GameApp.SocketClient_Game, (int)MsgType.enum_Msg_Client2Logic_Get_Offline_Candy_Req);
    }
    #endregion
    
    #endregion
}