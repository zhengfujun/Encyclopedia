using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text;
using common;
using System.Linq;

/// <summary>
/// 客户端收包处理类
/// </summary>
public class Game_Recv_Client
{
    private static SocketClientType _PacketSortType_Socekt = SocketClientType.Socket_Null;
    public static SocketClientType mPacketSortType_Socekt
    {
        get
        {
            return _PacketSortType_Socekt;
        }
        set
        {
            if (_PacketSortType_Socekt != value)
            {
                maxcount = 3;
                if (value != SocketClientType.Socket_Null)
                    ReSort(value);
                else
                    maxcount = 100;
                _PacketSortType_Socekt = value;
            }
        }
    }

    /// <summary>
    /// 用于存储完整包的列表
    /// </summary>
    public static List<RecPacketCls> RecPacketList = new List<RecPacketCls>();
    /// <summary>
    /// 用于连接成功或失败的列表
    /// </summary>
    public static List<ConnectResultMsg> OtherList = new List<ConnectResultMsg>();
    public static Dictionary<MsgType, System.Action> RecvMsgCallBackDic = new Dictionary<MsgType, Action>();

    public static int processcount = 0;  //一帧处理的包个数
    public static int maxcount = 100;

    static void ReSort(SocketClientType _type)
    {
        List<RecPacketCls> TempList = new List<RecPacketCls>();
        lock (RecPacketList)
        {
            foreach (RecPacketCls rpc in RecPacketList)
            {
                if (rpc.mSocketClient.mSocketClientType == _type)
                {
                    TempList.Add(rpc);
                }
            }

            foreach (RecPacketCls rpc in TempList)
            {
                RecPacketList.Remove(rpc);
            }

            TempList.AddRange(RecPacketList);
            RecPacketList.Clear();
            RecPacketList.AddRange(TempList);
        }
    }

    void ManageOtherPacket()
    {
        if (OtherList.Count != 0)
        {
            GameApp.SendMsg.DestoryWait();

            switch (OtherList[0].type)
            {
                case ConnectResultType.Msg_Connect_Success:
                    {
                        SocketClient sc = (SocketClient)OtherList[0].param;

                        Debug.Log(StringBuilderTool.ToInfoString("连接成功：[", sc.ClaName, "]"));

                        if (sc.mSocketClientType == SocketClientType.Socket_User)
                        {

                        }
                        if (sc.mSocketClientType == SocketClientType.Socket_Game)
                        {
                            GameApp.SendMsg.ConnectGameServer();
                        }
                    }
                    break;
                case ConnectResultType.Msg_Connect_Fail:
                    {
                        SocketClient sc = (SocketClient)OtherList[0].param;

                        Debug.Log(StringBuilderTool.ToInfoString(sc.ClaName, " ServerConnect,failed!"));

                        SocketManager.Instance.AddReconnectSocket(StringBuilderTool.ToInfoString(sc.ClaName, Localization.Get("connectionFailed")), sc);
                    }
                    break;
                case ConnectResultType.Msg_Connect_Break:
                    {
                        SocketClient sc = (SocketClient)OtherList[0].param;
                        SocketManager.Instance.AddReconnectSocket(StringBuilderTool.ToInfoString(sc.ClaName, Localization.Get("disconnected")), sc);

                        Debug.Log(sc.ClaName + " ServerConnect,break!");
                    }
                    break;
                default:
                    break;
            }
            OtherList.RemoveAt(0);
        }
    }

    public void BegainManagePacket()
    {
        //if (processcount >= maxcount - 10)
        //{
            //Debug.Log(processcount);
        //}
        processcount = 0;
        ManagePacket();
    }

    //List<RecPacketCls> TempList = new List<RecPacketCls>();
    public void ManagePacket()
    {
        while (processcount < maxcount)
        {
            ManageOtherPacket();

            if (RecPacketList.Count == 0)
                return;

            RecPacketCls RecPacls = null;

            lock (RecPacketList)
            {
                RecPacls = RecPacketList[0];
                RecPacketList.RemoveAt(0);
            }
            if (RecPacls == null)
            {
#if UNITY_EDITOR
                Debug.Log(StringBuilderTool.ToInfoString(DateTime.Now.ToString(), " ", DateTime.Now.Millisecond.ToString().PadLeft(4, ' '), " ", "Game_Recv_Client---RecPacls---", RecPacketList.Count.ToString()));
#endif
                return;
            }

            if (RecPacls.mSocketClient == null)
            {
#if UNITY_EDITOR
                Debug.Log(StringBuilderTool.ToInfoString(DateTime.Now.ToString(), " ", DateTime.Now.Millisecond.ToString().PadLeft(4, ' '), " ", "Game_Recv_Client---SocketClient---", RecPacketList.Count.ToString()));
#endif
                return;
            }

            if (RecPacls.ms == null)
            {
#if UNITY_EDITOR
                Debug.Log(StringBuilderTool.ToInfoString(DateTime.Now.ToString(), " ", DateTime.Now.Millisecond.ToString().PadLeft(4, ' '), " ", "Game_Recv_Client---ms---", RecPacketList.Count.ToString()));
#endif
                return;
            }

            SocketClient Scl = RecPacls.mSocketClient;
            MemoryStream ms = RecPacls.ms;
            int iFlag = RecPacls.iFlag;

            Scl.mManagMsgCls.RemovePacket(RecPacls);

            MsgType curMsgType = (MsgType)iFlag;

            if (curMsgType != MsgType.enum_Msg_Ping_Res)
                GameApp.SendMsg.DestoryWait();

            Scl.PingTimeIn();
            //Debug.Log("rec11 ..........curMsgType:.." + curMsgType.ToString() + Time.realtimeSinceStartup);
            processcount++;
            SocketManager.Instance.lastPacketTime = System.DateTime.Now;
            switch (curMsgType)
            {
                /// <summary> 心跳包 </summary>
                case MsgType.enum_Msg_Ping_Res:
                    {
                    }
                    break;
                /// <summary> 玩家登录账号服验证账号密码 </summary>
                /*case MsgType.enum_Msg_Account2Client_Login_Res:
                    {
                        Msg_Account2Client_Login_Res packet = new Msg_Account2Client_Login_Res();
                        packet = common.Serializer.Deserialize<Msg_Account2Client_Login_Res>(ms, packet);

                        if (packet.m_res == (int)AccountRes.AccountRes_Login_Success)
                        {
                            GameApp.ServerKey = packet.m_key;
                            GameApp.AccountID = packet.m_account_id;

                            //GameApp.Instance.CommonHintDlg.OpenHintBox("账号：[" + packet.m_account_id + "] 登录成功");
#if UNITY_EDITOR
                            Debug.Log(StringBuilderTool.ToString("登录账号服成功：账号[", packet.m_account_id, "]，授权码[", packet.m_key, "]"));
#endif
                            //创建游戏服连接
                            GameApp.SendMsg.CreateGameServerSocket();
                        }
                        else
                        {
                            GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToString("登录账号服失败：错误码[", (AccountRes)packet.m_res, "]"));
                        }
                    }
                    break;*/
                /// <summary> 玩家登录所选择的服务器（游戏服）</summary>
                case MsgType.enum_Msg_Gate2Client_Connect_Res:
                    {
                        Msg_Gate2Client_Connect_Res packet = new Msg_Gate2Client_Connect_Res();
                        packet = common.Serializer.Deserialize<Msg_Gate2Client_Connect_Res>(ms, packet);

                        if (packet.m_res == (int)LogicRes.Connect_Success)
                        {
//#if UNITY_EDITOR
                            Debug.Log("连接游戏服成功");
//#endif
                            GameApp.Instance.WaitLoadHintDlg.OpenHintBox("正在获取角色信息...");

                            GameApp.SendMsg.GetPlayerData();
                        }
                        else
                        {
                            GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToString("连接游戏服失败：错误码[", (LogicRes)packet.m_res, "]"));
                        }
                    }
                    break;
                /// <summary> 创建玩家 </summary>
                case MsgType.enum_Msg_Logic2Client_Create_Player_Res:
                    {
                        Msg_Logic2Client_Create_Player_Res packet = new Msg_Logic2Client_Create_Player_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Create_Player_Res>(ms, packet);

                        if (packet.m_res == (int)LogicRes.CreatePlayer_Success)
                        {
//#if UNITY_EDITOR
                            Debug.Log("创建玩家成功");
//#endif
                            GameApp.SendMsg.GetPlayerData();

                            //GameApp.SendMsg.GMOrder("AddItem 1001 800000");
                            //GameApp.SendMsg.GMOrder("AddItem 1002 200000");
                            GameApp.SendMsg.GMOrder("AddItem 1003 500");
                            //GameApp.SendMsg.GMOrder("AddItem 20001 20");
                            //GameApp.SendMsg.GMOrder("AddItem 20002 20");
                        }
                        else
                        {
//#if UNITY_EDITOR
                            Debug.Log(StringBuilderTool.ToString("创建玩家失败：错误码[", packet.m_res, "]"));
//#endif
                        }
                    }
                    break;
                /// <summary> 检测玩家名 </summary>
                case MsgType.enum_Msg_Logic2Client_CheckPlayerName_Res:
                    {
                        Msg_Logic2Client_CheckPlayerName_Res packet = new Msg_Logic2Client_CheckPlayerName_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_CheckPlayerName_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("检测玩家名 结果[", (LogicRes)packet.m_res, "]"));
#endif
                        if (GameApp.Instance.UILogin != null)
                        {
                            if(GameApp.Instance.UILogin.RoleUI != null)
                            {
                                GameApp.Instance.UILogin.RoleUI.CheckNameRes((LogicRes)packet.m_res, packet.m_player_name);
                            }
                        }

                        if (GameApp.Instance.HomePageUI != null)
                        {
                            if(GameApp.Instance.HomePageUI.NewSettingUI != null)
                            {
                                GameApp.Instance.HomePageUI.NewSettingUI.CheckNameRes((LogicRes)packet.m_res);
                            }
                        }
                    }
                    break;
                /// <summary> 设置玩家名 </summary>
                case MsgType.enum_Msg_Logic2Client_SetPlayerName_Res:
                    {
                        Msg_Logic2Client_SetPlayerName_Res packet = new Msg_Logic2Client_SetPlayerName_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_SetPlayerName_Res>(ms, packet);

                        if (GameApp.Instance.HomePageUI != null)
                        {
                            if (GameApp.Instance.HomePageUI.NewSettingUI != null)
                            {
                                GameApp.Instance.HomePageUI.NewSettingUI.ModifNameRes((LogicRes)packet.m_res);
                            }
                        }

                        if (GameApp.Instance.Platform != null)
                            GameApp.Instance.Platform.CheckRoleName(packet.m_player_name);
                    }
                    break;
                /// <summary> 设置GM标记 </summary>
                case MsgType.enum_Msg_Logic2Client_SetGM_Res:
                    {
                        Msg_Logic2Client_SetGM_Res packet = new Msg_Logic2Client_SetGM_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_SetGM_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到设置GM标记的回复 ", packet.m_gm, " ", (LogicRes)packet.m_res));
#endif
                        GameApp.Instance.PlayerData.m_gm = packet.m_gm;
                    }
                    break;
                /// <summary> 玩家数据 </summary>
                case MsgType.enum_Msg_Logic2Client_Player_Data_Res:
                    {
                        Msg_Logic2Client_Player_Data_Res packet = new Msg_Logic2Client_Player_Data_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Player_Data_Res>(ms, packet);

                        GameApp.Instance.WaitLoadHintDlg.CloseHintBox();

                        if (packet.m_exists)
                        {
//#if UNITY_EDITOR
                            Debug.Log("获取玩家数据成功");
//#endif
                            GameApp.Instance.PlayerData = packet.m_player;

                            if (GameApp.Instance.Platform != null)
                                GameApp.Instance.Platform.CheckRoleName(packet.m_player.m_player_name);

                            GameApp.Instance.MainPlayerData.Name = packet.m_player.m_player_name;
                            GameApp.SendMsg.SetAvatar(GameApp.Instance.MainPlayerData.RoleID);

                            GameApp.Instance.SceneCtlInstance.ChangeScene(SceneControl.HomePage);
#if UNITY_EDITOR
                            Player p = packet.m_player;
                            Debug.Log(StringBuilderTool.ToString("角色(账号)ID：", p.m_account_id));
                            Debug.Log(StringBuilderTool.ToString("所属服ID：", p.m_server_id));
                            Debug.Log(StringBuilderTool.ToString("玩家角色名称：", p.m_player_name));
                            Debug.Log(StringBuilderTool.ToString("GM账号标记：", p.m_gm));
                            
                            // 玩家基础信息
                            PlayerBase pb = packet.m_player.m_player_base;
                            Debug.Log("玩家基础信息---------------------------------------------------");
                            Debug.Log(StringBuilderTool.ToString("玩家等级：", pb.m_player_lv));
                            Debug.Log(StringBuilderTool.ToString("玩家经验：", pb.m_player_exp));
                            Debug.Log(StringBuilderTool.ToString("vip等级：", pb.m_player_vip));
                            Debug.Log(StringBuilderTool.ToString("角色创建时间：", MyTools.getTime((int)pb.m_player_create).ToString()));
                            Debug.Log(StringBuilderTool.ToString("角色上一次的上线时间：", MyTools.getTime((int)pb.m_player_last_online).ToString()));
                            Debug.Log(StringBuilderTool.ToString("角色上一次的下线时间：", MyTools.getTime((int)pb.m_player_last_offline).ToString()));
                            Debug.Log(StringBuilderTool.ToString("任务数据：", pb.m_player_task));
                            Debug.Log(StringBuilderTool.ToString("赛事备忘数据：", pb.m_player_contestMemoInfo));
                            Debug.Log(StringBuilderTool.ToString("换装ID：", pb.m_avatar));
                            Debug.Log(StringBuilderTool.ToString("上次免费扭蛋时间：", MyTools.getTime((int)pb.m_free_gacha_time).ToString()));
                            Debug.Log(StringBuilderTool.ToString("结算的糖果：", pb.m_settle_candy));
                            Debug.Log(StringBuilderTool.ToString("结算的糖果时间：", MyTools.getTime((int)pb.m_settle_candy_time).ToString()));

                            // 玩家背包信息
                            PlayerBag bag = packet.m_player.m_player_bag;
                            Debug.Log("玩家背包信息---------------------------------------------------");
                            for (int i = 0; i < bag.m_items.Count; i++)
                            {
                                PlayerBagItem pbi = bag.m_items[i];
                                Debug.Log(StringBuilderTool.ToString("[", i, "]道具流水ID：", pbi.m_id, "配置表ID：", pbi.m_item_id, " 道具数量：", pbi.m_item_count));
                            }

                            // 玩家PVE信息
                            Debug.Log("玩家PVE信息----------------------------------------------------");
                            for (int i = 0; i < packet.m_player.m_player_pve.m_pves.Count; i++)
                            {
                                Debug.Log(StringBuilderTool.ToString("关卡进度：（", i, ")", packet.m_player.m_player_pve.m_pves[i].m_id));
                            }

                            //玩家赛事信息
                            Debug.Log("玩家赛事信息---------------------------------------------------");
                            PlayerContest pc = packet.m_player.m_player_contest;
                            Debug.Log(StringBuilderTool.ToString("参与的开启活动的GM玩家ID：", pc.gm_id));
                            Debug.Log(StringBuilderTool.ToString("已存的比赛配置信息：", pc.setup));
                            Debug.Log(StringBuilderTool.ToString("作为GM开启的赛事ID：", pc.open_id));
                            Debug.Log(StringBuilderTool.ToString("参与的开启Boss战斗活动的gm玩家ID：", pc.boss_gm_id));
                            Debug.Log(StringBuilderTool.ToString("作为GM开启的Boss战斗赛事ID：", pc.boss_open_id));

                            // 玩家邮件信息
                            Debug.Log("玩家邮件信息---------------------------------------------------");
                            for (int i = 0; i < p.m_player_mail.m_mails.Count; i++)
                            {
                                PlayerMailItem pmi = p.m_player_mail.m_mails[i];
                                Debug.Log(StringBuilderTool.ToString("第[", i, "]封\n",
                                    "邮件id：", pmi.m_id,
                                    " 邮件状态：" + pmi.m_state,
                                    " 邮件类型：" + pmi.m_type,
                                    " 邮件时间：" + pmi.m_time,
                                    " 邮件头：" + pmi.m_header,
                                    " 邮件内容：" + pmi.m_content));
                                for (int j = 0; j < pmi.m_appendix.Count; j++)
                                {
                                    MailAppendixItem mai = pmi.m_appendix[j];
                                    Debug.Log(StringBuilderTool.ToString("[", j, "]道具ID：", mai.m_id, " 道具数量：", mai.m_count));
                                }
                            }

                            //动作信息
                            Debug.Log("动作信息-------------------------------------------------------");
                            PlayerAction pa = p.m_player_action;
                            Debug.Log(StringBuilderTool.ToString("动作类型：", (ActionType)pa.m_type));
                            Debug.Log(StringBuilderTool.ToString("开始时间：", MyTools.getTime((int)pa.m_begin_time).ToString()));
                            Debug.Log(StringBuilderTool.ToString("结束时间：", MyTools.getTime((int)pa.m_end_time).ToString()));

                            //玩家旅行背包
                            Debug.Log("玩家旅行背包---------------------------------------------------");
                            for (int i = 0; i < p.m_player_travel_bag.m_items.Count; i++)
                            {
                                PlayerTravelBagItem ptbi = p.m_player_travel_bag.m_items[i];
                                Debug.Log(StringBuilderTool.ToString("位置:", ptbi.m_id, " ID:", ptbi.m_item_id));
                            }
#endif
#if UNITY_ANDROID
                            GameApp.Instance.TDAccount = TDGAAccount.SetAccount(GameApp.Instance.PlayerData.m_account_id.ToString());
                            if (GameApp.Instance.TDAccount != null)
                            {
                                GameApp.Instance.TDAccount.SetAccountType(GameApp.Instance.CurAccountType);
                                GameApp.Instance.TDAccount.SetLevel((int)GameApp.Instance.PlayerData.m_player_base.m_player_lv);
                                GameApp.Instance.TDAccount.SetGameServer(GameApp.ServerIndex + "服");
                            }
#endif
                        }
                        else
                        {
//#if UNITY_EDITOR
                            Debug.Log("获取玩家数据失败，创建新玩家");
//#endif
                            GameApp.Instance.UILogin.ShowStoryVideo();
                        }
                    }
                    break;
                /// <summary> 扭蛋 </summary>
                case MsgType.enum_Msg_Logic2Client_Gacha_Res:
                    {
                        Msg_Logic2Client_Gacha_Res packet = new Msg_Logic2Client_Gacha_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Gacha_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("返回扭蛋结果：", (LogicRes)packet.m_res, " 类型：", packet.type));
#endif
                        if ((LogicRes)packet.m_res != LogicRes.Common_Process_Success)
                        {
                            if ((LogicRes)packet.m_res == LogicRes.Common_Item_Not_Enough)
                            {
                                GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToString("扭蛋币不足！请前往市场购买"));
                            }
                            else
                            {
                                GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox(StringBuilderTool.ToString("扭蛋失败！（错误码：" + (LogicRes)packet.m_res + "）"));
                            }
                        }
                    }
                    break;
                /// <summary> 合成 </summary>
                case MsgType.enum_Msg_Logic2Client_Compose_Res:
                    {
                        Msg_Logic2Client_Compose_Res packet = new Msg_Logic2Client_Compose_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Compose_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("返回合成结果：", (LogicRes)packet.m_res, " ID：", packet.id, " 类型：", packet.type));
#endif
                        if ((LogicRes)packet.m_res != LogicRes.Common_Process_Success)
                        {
                            GameApp.Instance.CommonMsgDlg.OpenMsgBox(StringBuilderTool.ToString("合成失败！（错误码：" + (LogicRes)packet.m_res + "）"));
                        }
                    }
                    break;
                /// <summary> 更新玩家基础信息 </summary>
                case MsgType.enum_Msg_Logic2Client_PlayerBase_Res:
                    {
                        Msg_Logic2Client_PlayerBase_Res packet = new Msg_Logic2Client_PlayerBase_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PlayerBase_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log("更新玩家基础信息");
#endif
                        if (GameApp.Instance.PlayerData == null)
                        {
                            Debug.LogError("消息异常，GameApp.Instance.PlayerData为空时收到了enum_Msg_Logic2Client_PlayerBase_Res！");
                            break;
                        }

                        if (GameApp.Instance.PlayerData.m_player_base != null &&
                            packet.m_player_base.m_player_lv > GameApp.Instance.PlayerData.m_player_base.m_player_lv)
                        {
                            //升级了
                            if (GameApp.Instance.FightUI != null)
                            {

                            }
                        }
                        //
                        GameApp.Instance.PlayerData.m_player_base = packet.m_player_base;
#if UNITY_EDITOR
                        PlayerBase pb = packet.m_player_base;
                        Debug.Log(StringBuilderTool.ToString("玩家等级：", pb.m_player_lv));
                        Debug.Log(StringBuilderTool.ToString("玩家经验：", pb.m_player_exp));
                        Debug.Log(StringBuilderTool.ToString("vip等级：", pb.m_player_vip));
                        Debug.Log(StringBuilderTool.ToString("角色创建时间：", MyTools.getTime((int)pb.m_player_create).ToString()));
                        Debug.Log(StringBuilderTool.ToString("角色上一次的上线时间：", MyTools.getTime((int)pb.m_player_last_online).ToString()));
                        Debug.Log(StringBuilderTool.ToString("角色上一次的下线时间：", MyTools.getTime((int)pb.m_player_last_offline).ToString()));
                        Debug.Log(StringBuilderTool.ToString("任务数据：", pb.m_player_task));
                        Debug.Log(StringBuilderTool.ToString("赛事备忘数据：", pb.m_player_contestMemoInfo));
                        Debug.Log(StringBuilderTool.ToString("换装ID：", pb.m_avatar));
                        Debug.Log(StringBuilderTool.ToString("上次免费扭蛋时间：", MyTools.getTime((int)pb.m_free_gacha_time).ToString()));
                        Debug.Log(StringBuilderTool.ToString("结算的糖果：", pb.m_settle_candy));
                        Debug.Log(StringBuilderTool.ToString("结算的糖果时间：", MyTools.getTime((int)pb.m_settle_candy_time).ToString()));
#endif
#if UNITY_ANDROID
                        if (GameApp.Instance.TDAccount != null)
                        {
                            GameApp.Instance.TDAccount.SetLevel((int)GameApp.Instance.PlayerData.m_player_base.m_player_lv);
                        }
#endif
                    }
                    break;
                
                /// <summary> 更新背包信息(全部) </summary>
                case MsgType.enum_Msg_Logic2Client_Bag_Res:
                    {
                        Msg_Logic2Client_Bag_Res packet = new Msg_Logic2Client_Bag_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Bag_Res>(ms, packet);

                        GameApp.Instance.PlayerData.m_player_bag = packet.m_player_bag;
#if UNITY_EDITOR
                        Debug.Log("更新背包信息(全部)");
                        for (int i = 0; i < packet.m_player_bag.m_items.Count; i++)
                        {
                            PlayerBagItem pbi = packet.m_player_bag.m_items[i];
                            Debug.Log(StringBuilderTool.ToString("[", i, "]道具流水ID：", pbi.m_id, "配置表ID：", pbi.m_item_id, " 道具数量：", pbi.m_item_count));
                        }
#endif

                        if (GameApp.Instance.UICurrency != null)
                        {
                            GameApp.Instance.UICurrency.Refresh();
                        }
                    }
                    break;
                /// <summary> 更新背包信息(单个) </summary>
                case MsgType.enum_Msg_Logic2Client_One_Item_Notify:
                    {
                        Msg_Logic2Client_One_Item_Notify packet = new Msg_Logic2Client_One_Item_Notify();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_One_Item_Notify>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("更新背包信息(单个) 道具流水ID：", packet.id, "配置表ID：", packet.item_id, " 道具数量：", packet.item_count));
#endif
                        bool hasSameItem = false;
                        for (int i = 0; i < GameApp.Instance.PlayerData.m_player_bag.m_items.Count; i++)
                        {
                            PlayerBagItem pbi = GameApp.Instance.PlayerData.m_player_bag.m_items[i];
                            if (pbi.m_item_id == packet.item_id)
                            {
                                hasSameItem = true;
                                if (pbi.m_id == packet.id)
                                {
                                    pbi.m_item_count = packet.item_count;

                                    if (pbi.m_item_count <= 0)
                                    {
                                        GameApp.Instance.PlayerData.m_player_bag.m_items.RemoveAt(i);
                                    }
                                    break;
                                }
                                else
                                {
                                    GameApp.Instance.PlayerData.m_player_bag.m_items.Add(new PlayerBagItem(packet.id, packet.item_id, packet.item_count));
                                    break;
                                }
                            }
                        }
                        if (!hasSameItem)
                        {
                            GameApp.Instance.PlayerData.m_player_bag.m_items.Add(new PlayerBagItem(packet.id, packet.item_id, packet.item_count));
                        }

                        if (GameApp.Instance.UICurrency != null)
                        {
                            GameApp.Instance.UICurrency.Refresh();
                        }
                    }
                    break;
                /// <summary> 更新旅行背包信息 </summary>
                case MsgType.enum_Msg_Logic2Client_Travel_Bag_Res:
                    {
                        Msg_Logic2Client_Travel_Bag_Res packet = new Msg_Logic2Client_Travel_Bag_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Travel_Bag_Res>(ms, packet);

                        GameApp.Instance.PlayerData.m_player_travel_bag = packet.m_player_bag;
#if UNITY_EDITOR
                        Debug.Log("更新旅行背包信息");
                        for (int i = 0; i < packet.m_player_bag.m_items.Count; i++)
                        {
                            PlayerTravelBagItem ptbi = packet.m_player_bag.m_items[i];
                            Debug.Log(StringBuilderTool.ToString("位置:", ptbi.m_id, " ID:", ptbi.m_item_id));
                        }
#endif
                    }
                    break;
                /// <summary> 掉落数据 </summary>
                case MsgType.enum_Msg_Logic2Client_PVEDropInfo_Res:
                    {
                        Msg_Logic2Client_PVEDropInfo_Res packet = new Msg_Logic2Client_PVEDropInfo_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVEDropInfo_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log("收到掉落数据");
                        Debug.Log(StringBuilderTool.ToString("掉落类型：", (DropItemType)packet.m_type));
#endif
#if UNITY_ANDROID
                        for (int i = 0; i < packet.m_items.Count; i++)
                        {
                            PlayerBagItem pbi = packet.m_items[i];
#if UNITY_EDITOR
                            Debug.Log(StringBuilderTool.ToString("[", i, "]道具流水ID：", pbi.m_id, "配置表ID：", pbi.m_item_id, " 道具数量：", pbi.m_item_count));
#endif
                            if (pbi.m_item_id / 50000 == 1)
                            {
                                Dictionary<string, object> dic = new Dictionary<string, object>();
                                dic.Add("卡牌ID：", pbi.m_item_id);
                                dic.Add("卡牌数量：", pbi.m_item_count);
                                dic.Add("途径：", ((DropItemType)packet.m_type).ToString());
                                TalkingDataGA.OnEvent("获得卡牌", dic);
                            }
                        }
#endif
                        switch ((DropItemType)packet.m_type)
                        {
                            case DropItemType.DropItemType_PVE://pve掉落
                                {
                                    
                                }
                                break;
                            case DropItemType.DropItemType_Box://宝箱掉落
                                {
                                    
                                }
                                break;
                            case DropItemType.DropItemType_Common://常规获得
                                {

                                }
                                break;
                            case DropItemType.DropItemType_Gacha://扭蛋获得
                                {
                                    if (GameApp.Instance.HomePageUI != null)
                                    {
                                        if (GameApp.Instance.HomePageUI.GashaponMachineUI != null)
                                        {
                                            GameApp.Instance.HomePageUI.GashaponMachineUI.Gashapon((int)packet.m_items[0].m_item_id);
                                        }
                                    }
                                    if (GameApp.Instance.TravelUI != null)
                                    {
                                        if (GameApp.Instance.TravelUI.GashaponMachineUI != null)
                                        {
                                            GameApp.Instance.TravelUI.GashaponMachineUI.Gashapon((int)packet.m_items[0].m_item_id);
                                        }
                                    }  
                                }
                                break;
                            case DropItemType.DropItemType_Compose://合成获得
                                {
                                    if (GameApp.Instance.HomePageUI != null)
                                    {
                                        if (GameApp.Instance.HomePageUI.MagicBookUI != null)
                                        {
                                            GameApp.Instance.HomePageUI.MagicBookUI.CompoundSuccess((int)packet.m_items[0].m_item_id);
                                        }
                                    }
                                    else if (GameApp.Instance.TravelUI != null)
                                    {
                                        if (GameApp.Instance.TravelUI.MagicBookUI != null)
                                        {
                                            GameApp.Instance.TravelUI.MagicBookUI.CompoundSuccess((int)packet.m_items[0].m_item_id);
                                        }
                                    }
                                }
                                break;
                            case DropItemType.DropItemType_AnswerRight://答题正确
                                {
                                    if (GameApp.Instance.UICurrency != null)
                                    {
                                        if (packet.m_items.Count > 0)
                                        {
                                            PlayerBagItem pbi = packet.m_items[0];
                                            if(pbi.m_item_id == 1001)
                                            {
                                                GameApp.Instance.UICurrency.AccumulationGold(pbi.m_item_count);
                                            }
                                        }
                                    }
                                }
                                break;
                            case DropItemType.DropItemType_Buy://购买"
                                {
                                    if (GameApp.Instance.TravelUI != null)
                                    {
                                        if (GameApp.Instance.TravelUI.TravelShop != null)
                                        {
                                            GameApp.Instance.TravelUI.TravelShop.BuyRes();
                                        }
                                    }
                                }
                                break;
                            case DropItemType.DropItemType_GetCandy://获得糖果
                                {

                                }
                                break;
                            case DropItemType.DropItemType_GetGift://旅行礼物
                                {
                                    GameApp.Instance.TravelAwardLst.Clear();
                                    for (int i = 0; i < packet.m_items.Count; i++)
                                    {
                                        PlayerBagItem pbi = packet.m_items[i];
                                        GameApp.Instance.TravelAwardLst.Add((int)pbi.m_item_id, (int)pbi.m_item_count);
                                    }
                                }
                                break;
                            case DropItemType.DropItemType_Recharge://充值获得
                                {
                                    if(GameApp.Instance.HomePageUI != null)
                                    {
                                        if(GameApp.Instance.HomePageUI.RechargeUI != null)
                                        {
                                            GameApp.Instance.HomePageUI.RechargeUI.RefreshPrivilege();
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                /// <summary> 更新PVE信息 </summary>
                case MsgType.enum_Msg_Logic2Client_PlayerPVE_Res:
                    {
                        Msg_Logic2Client_PlayerPVE_Res packet = new Msg_Logic2Client_PlayerPVE_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PlayerPVE_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log("刷新PVE信息");
                        for (int i = 0; i < packet.info.m_pves.Count; i++)
                        {
                            Debug.Log(StringBuilderTool.ToString("关卡进度：（", i, ")", packet.info.m_pves[i].m_id));
                        }
#endif
                        GameApp.Instance.PlayerData.m_player_pve = packet.info;
                    }
                    break;
                /// <summary> 设置换装ID </summary>
                case MsgType.enum_Msg_Logic2Client_Change_Avatar_Res:
                    {
                        Msg_Logic2Client_Change_Avatar_Res packet = new Msg_Logic2Client_Change_Avatar_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Change_Avatar_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到设置换装ID的回复", " 换装ID：", packet.id, " 结果：", (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
                            GameApp.Instance.PlayerData.m_player_base.m_avatar = packet.id;
                        }
                    }
                    break;
                /// <summary> 排行榜 </summary>
                case MsgType.enum_Msg_Logic2Client_Rank_Res:
                    {
                        Msg_Logic2Client_Rank_Res packet = new Msg_Logic2Client_Rank_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Rank_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("返回排行榜数据结果：", (LogicRes)packet.m_res, " ", packet.type));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
#if UNITY_EDITOR
                            for (int i = 0; i < packet.infos.Count; i++)
                            {
                                PlayerRankInfo pri = packet.infos[i];
                                Debug.Log(StringBuilderTool.ToString("[", i, "]玩家 ID：", pri.id, " 名字：", pri.name, " 积分：", pri.score));
                            }
#endif
                            GameApp.Instance.HomePageUI.RankingUI.RefreshRank(packet.infos);
                        }
                    }
                    break;
                /// <summary> 更新任务信息 </summary>
                case MsgType.enum_Msg_Logic2Client_SaveTaskInfo_Res:
                    {
                        Msg_Logic2Client_SaveTaskInfo_Res packet = new Msg_Logic2Client_SaveTaskInfo_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_SaveTaskInfo_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("刷新任务信息 结果：", (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {

                        }
                    }
                    break;
                /// <summary> 更新赛事备忘数据  </summary>
                case MsgType.enum_Msg_Logic2Client_SaveContestMemInfo_Res:
                    {
                        Msg_Logic2Client_SaveContestMemInfo_Res packet = new Msg_Logic2Client_SaveContestMemInfo_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_SaveContestMemInfo_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("更新赛事备忘数据 结果：", (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {

                        }
                    }
                    break;
                /// <summary> 获得服务器当前时间 </summary>
                case MsgType.enum_Msg_Logic2Client_ServerTime_Res:
                    {
                        Msg_Logic2Client_ServerTime_Res packet = new Msg_Logic2Client_ServerTime_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_ServerTime_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToInfoString("服务器当前时间：", MyTools.getTime((int)packet.m_server_time).ToString()));
#endif
                    }
                    break;
#region 邮件
                /// <summary> 打开邮件  </summary>
                case MsgType.enum_Msg_Logic2Client_OpenMail_Res:
                    {
                        Msg_Logic2Client_OpenMail_Res packet = new Msg_Logic2Client_OpenMail_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_OpenMail_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("打开邮件 ID：", packet.id, " 结果：", (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
                            if (GameApp.Instance.HomePageUI.MailUI != null)
                                GameApp.Instance.HomePageUI.MailUI.OpenMailRes();
                        }
                    }
                    break;
                /// <summary> 删除邮件 </summary>
                case MsgType.enum_Msg_Logic2Client_DeleteMail_Res:
                    {
                        Msg_Logic2Client_DeleteMail_Res packet = new Msg_Logic2Client_DeleteMail_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_DeleteMail_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("删除邮件 ID：", packet.id, " 结果：", (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
                            if (GameApp.Instance.HomePageUI.MailUI != null)
                                GameApp.Instance.HomePageUI.MailUI.DeleteMailRes();
                        }
                    }
                    break;
                /// <summary> 添加邮件 </summary>
                case MsgType.enum_Msg_Logic2Client_AddMailInfo_Res:
                    {
                        Msg_Logic2Client_AddMailInfo_Res packet = new Msg_Logic2Client_AddMailInfo_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_AddMailInfo_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log("添加邮件");
                        Debug.Log(StringBuilderTool.ToString(
                            "邮件id：", packet.m_mail.m_id,
                            " 邮件状态：" + packet.m_mail.m_state,
                            " 邮件类型：" + packet.m_mail.m_type,
                            " 邮件时间：" + packet.m_mail.m_time,
                            " 邮件头：" + packet.m_mail.m_header,
                            " 邮件内容：" + packet.m_mail.m_content));
                        for (int j = 0; j < packet.m_mail.m_appendix.Count; j++)
                        {
                            MailAppendixItem mai = packet.m_mail.m_appendix[j];
                            Debug.Log(StringBuilderTool.ToString("[", j, "]道具ID：", mai.m_id, " 道具数量：", mai.m_count));
                        }
#endif
                        UI_Mail.AddMail(packet.m_mail);
                    }
                    break;
#endregion
                /// <summary> 更新动作信息 </summary>
                case MsgType.enum_Msg_Logic2Client_PlayerAction_Res:
                    {
                        Msg_Logic2Client_PlayerAction_Res packet = new Msg_Logic2Client_PlayerAction_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PlayerAction_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log("更新动作信息");
                        Debug.Log(StringBuilderTool.ToString("动作类型：", (ActionType)packet.m_player_action.m_type));
                        Debug.Log(StringBuilderTool.ToString("开始时间：", MyTools.getTime((int)packet.m_player_action.m_begin_time).ToString()));
                        Debug.Log(StringBuilderTool.ToString("结束时间：", MyTools.getTime((int)packet.m_player_action.m_end_time).ToString()));
#endif
                        GameApp.Instance.PlayerData.m_player_action = packet.m_player_action;

                        if (GameApp.Instance.TravelUI != null)
                        {
                            GameApp.Instance.TravelUI.UpdateBaoBaoLongState();
                        } 
                    }
                    break;
                /// <summary> 购买道具 </summary>
                case MsgType.enum_Msg_Logic2Client_Buy_Shop_Res:
                    {
                        Msg_Logic2Client_Buy_Shop_Res packet = new Msg_Logic2Client_Buy_Shop_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Buy_Shop_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到购买道具的回复", " 道具ID：" + packet.id, " 结果：" + (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
                            
                        }
                    }
                    break;
                /// <summary> 放置道具 </summary>
                case MsgType.enum_Msg_Logic2Client_Put_Bag_Res:
                    {
                        Msg_Logic2Client_Put_Bag_Res packet = new Msg_Logic2Client_Put_Bag_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Put_Bag_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到放置道具的回复 道具ID：", packet.id, " 旧位置：", (PosType)packet.oldPos, " 新位置：", (PosType)packet.newPos, " 结果：" + (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
                            if (GameApp.Instance.TravelUI != null)
                            {
                                GameApp.Instance.TravelUI.TravelBackpack.SetGridRes();
                            }
                        }
                        else
                        {
                            GameApp.Instance.CommonMsgDlg.OpenMsgBox(StringBuilderTool.ToString("放置物品失败！（错误码：" + (LogicRes)packet.m_res + "）"));
                        }
                    }
                    break;
                /// <summary> 获取离线糖果 </summary>
                case MsgType.enum_Msg_Logic2Client_Get_Offline_Candy_Res:
                    {
                        Msg_Logic2Client_Get_Offline_Candy_Res packet = new Msg_Logic2Client_Get_Offline_Candy_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_Get_Offline_Candy_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到获取离线糖果的回复 结果：" + (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {

                        }
                    }
                    break;
                /// <summary> 外出通知 </summary>
                case MsgType.enum_Msg_Logic2ClientTravel_Go_Out_Notify:
                    {
                        Msg_Logic2ClientTravel_Go_Out_Notify packet = new Msg_Logic2ClientTravel_Go_Out_Notify();
                        packet = common.Serializer.Deserialize<Msg_Logic2ClientTravel_Go_Out_Notify>(ms, packet);
#if UNITY_EDITOR
                        Debug.LogError("收到外出通知");
#endif
                        GameApp.Instance.NeedShowGetOutHint = true;

                        if (PlayerPrefs.HasKey("TempLetter"))
                        {
                            string msg = PlayerPrefs.GetString("TempLetter");
                            msg += StringBuilderTool.ToInfoString("#", "宝宝龙出发去旅行啦！\n",DateTime.Now.ToString());
                            PlayerPrefs.SetString("TempLetter", msg);
                        }
                        else
                        {
                            PlayerPrefs.SetString("TempLetter", StringBuilderTool.ToInfoString("宝宝龙出发去旅行啦！\n", DateTime.Now.ToString()));
                        }
                    }
                    break;
                /// <summary> 回家通知 </summary>
                case MsgType.enum_Msg_Logic2ClientTravel_Come_Back_Notify:
                    {
                        Msg_Logic2ClientTravel_Come_Back_Notify packet = new Msg_Logic2ClientTravel_Come_Back_Notify();
                        packet = common.Serializer.Deserialize<Msg_Logic2ClientTravel_Come_Back_Notify>(ms, packet);
#if UNITY_EDITOR
                        Debug.LogError("收到回家通知");
#endif
                        GameApp.Instance.NeedShowComeBackHint = true;

                        if (PlayerPrefs.HasKey("TempLetter"))
                        {
                            string msg = PlayerPrefs.GetString("TempLetter");
                            msg += StringBuilderTool.ToInfoString("#", "宝宝龙平安归来啦！\n", DateTime.Now.ToString());
                            PlayerPrefs.SetString("TempLetter", msg);
                        }
                        else
                        {
                            PlayerPrefs.SetString("TempLetter", StringBuilderTool.ToInfoString("宝宝龙平安归来啦！\n", DateTime.Now.ToString()));
                        }
                    }
                    break;
                /// <summary> 是否允许进入关卡 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Enter_Res:
                    {
                        Msg_Logic2Client_PVE_Enter_Res packet = new Msg_Logic2Client_PVE_Enter_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Enter_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到检测是否允许进入关卡的回复"," 关卡ID：", packet.m_id, " 结果：", (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
                            GameApp.Instance.HomePageUI.StageMapUI.ShowSelMode();
                        }
                    }
                    break;
                /// <summary> 创建房间 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Create_Room_Res:
                    {
                        Msg_Logic2Client_PVE_Create_Room_Res packet = new Msg_Logic2Client_PVE_Create_Room_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Create_Room_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到创建房间的回复"," 关卡ID：" + packet.m_id," 房间ID：" + packet.room_id," 结果：" + (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
                            GameApp.SendMsg.ReadyInRoom(true);
                            GameApp.Instance.HomePageUI.StageMapUI.ShowStageDetails();
                        }
                    }
                    break;
                /// <summary> 加入房间 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Join_Room_Res:
                    {
                        Msg_Logic2Client_PVE_Join_Room_Res packet = new Msg_Logic2Client_PVE_Join_Room_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Join_Room_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到加入房间的回复", " 关卡ID：" + packet.m_id, " 房间ID：" + packet.room_id, " 结果：" + (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
                            GameApp.SendMsg.ReadyInRoom(true);
                            GameApp.Instance.HomePageUI.StageMapUI.ShowStageDetails();
                        }
                        else if ((LogicRes)packet.m_res == LogicRes.Room_Not_Suitable)//没有合适房间
                        {
                            //GameApp.Instance.CommonMsgDlg.OpenSimpleMsgBox("没有匹配到合适的房间！");

                            GameApp.Instance.CurRoomPlayerLst.Clear();
                            GameApp.Instance.CurRoomPlayerLst.Add(new PVE_Room_Player((ulong)GameApp.Instance.MainPlayerData.RoleID, GameApp.Instance.MainPlayerData.Name, GameApp.Instance.MainPlayerData.RoleID, true, 0, 0, 0));
                            GameApp.Instance.CurRoomPlayerLst.Add(new PVE_Room_Player((ulong)GameApp.Instance.AIRobotData.RoleID, GameApp.Instance.AIRobotData.Name, GameApp.Instance.AIRobotData.RoleID, true, 0, 0, 0));
                            GameApp.Instance.HomePageUI.StageMapUI.RefreshRoomInfo(true);
                            GameApp.Instance.HomePageUI.StageMapUI.ShowStageDetails();
                        }
                    }
                    break;
                /// <summary> 房间数据广播 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Room_Broadcast:
                    {
                        Msg_Logic2Client_PVE_Room_Broadcast packet = new Msg_Logic2Client_PVE_Room_Broadcast();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Room_Broadcast>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到房间数据广播"," 房间ID：", packet.room_id," 关卡ID：", packet.barrier_id));                        
                        //房间玩家列表
                        for (int i = 0; i < packet.players.Count; i++)
                        {
                            PVE_Room_Player player = packet.players[i];
                            Debug.Log(StringBuilderTool.ToString(
                                "玩家序号：", i, 
                                " ID：", player.id, 
                                " 名字：", player.name, 
                                " 头像：", player.icon, 
                                " 准备状态：", player.ready,
                                " 积分：", player.score,
                                " 获胜序号：", player.win_num,
                                " 游戏状态：", player.game_state));
                        }
#endif
                        //List<common.PVE_Room_Player> OldRoomPlayerLst = GameApp.Instance.CurRoomPlayerLst.GetRange(0, GameApp.Instance.CurRoomPlayerLst.Count);
                        GameApp.Instance.CurRoomPlayerLst = packet.players;
                        
                        if (GameApp.Instance.HomePageUI != null)
                        {
                            GameApp.Instance.HomePageUI.StageMapUI.RefreshRoomInfo(/*(int)packet.room_id, (int)packet.barrier_id*/);
                        }
                        /*if(GameApp.Instance.FightUI != null)
                        {
                            if (GameApp.Instance.FightUI.lastTeamMemCnt > GameApp.Instance.CurRoomPlayerLst.Count)
                            {
                                foreach (common.PVE_Room_Player player in OldRoomPlayerLst)
                                {
                                    if (!GameApp.Instance.CurRoomPlayerLst.Contains(player))
                                    {
                                        GameApp.Instance.CommonHintDlg.OpenHintBox(StringBuilderTool.ToInfoString(player.name, "退出了！"));

                                        break;
                                    }
                                }
                                //Debug.LogError(StringBuilderTool.ToInfoString(player.name, "退出了！"));

                                GameApp.Instance.FightUI.init();
                                GameApp.Instance.FightUI.EnableNextPlayerThrowDice();
                                GameApp.Instance.FightUI.lastTeamMemCnt = GameApp.Instance.CurRoomPlayerLst.Count;
                            }
                        }*/
                    }
                    break;
                /// <summary> 开始游戏 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Start_Game_Room_Res:
                    {
                        Msg_Logic2Client_PVE_Start_Game_Room_Res packet = new Msg_Logic2Client_PVE_Start_Game_Room_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Start_Game_Room_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到开始游戏的回复  结果：", (LogicRes)packet.m_res));
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
                            GameApp.Instance.LoadingDlg.SetPlayerLoadingType(PlayerLoadingType.Multi, GameApp.Instance.HomePageUI.StageMapUI.TeamMembers);

                            GameApp.Instance.SceneCtlInstance.ChangeScene(GameApp.Instance.CurFightStageCfg.FightSceneName);
                        }
                    }
                    break;
                /// <summary> 投掷筛子广播 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Throw_Room_broadcast:
                    {
                        Msg_Logic2Client_PVE_Throw_Room_broadcast packet = new Msg_Logic2Client_PVE_Throw_Room_broadcast();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Throw_Room_broadcast>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到投掷筛子广播", " 玩家ID：", packet.player_id, " 筛子数：", packet.dice_num));
#endif
                        if (GameApp.Instance.FightUI != null)
                        {
                            GameApp.Instance.FightUI.ThrowDiceResult(packet.player_id, packet.dice_num);
                        }
                    }
                    break;
                /// <summary> 触发格子事件广播 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Trigger_Effect_Room_Broadcast:
                    {
                        Msg_Logic2Client_PVE_Trigger_Effect_Room_Broadcast packet = new Msg_Logic2Client_PVE_Trigger_Effect_Room_Broadcast();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Trigger_Effect_Room_Broadcast>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到触发格子事件的广播", " 玩家ID：", packet.player_id, " 事件类型：", packet.type));
#endif
                        if( packet.type == RoomPosType.RoomPosType_ToInnerRound)
                        {
                            if (GameApp.Instance.FightUI != null)
                            {
                                GameApp.Instance.FightUI.GotoBossResult();
                            }
                        }
                        else if (packet.type == RoomPosType.RoomPosType_DevilQuestionToInner)
                        {
                            if (GameApp.Instance.FightUI != null)
                            {
                                if (GameApp.Instance.FightUI.QandAUI != null)
                                {
                                    GameApp.Instance.FightUI.QandAUI.AnswerQuestionResult(packet.player_id, false);
                                }
                            }
                        }
                        else if (packet.type == RoomPosType.RoomPosType_DevilAnswerRight)
                        {
                            if (GameApp.Instance.FightUI != null)
                            {
                                if (GameApp.Instance.FightUI.QandAUI != null)
                                {
                                    GameApp.Instance.FightUI.QandAUI.AnswerQuestionResult(packet.player_id, true);
                                }
                            }
                        }
                    }
                    break;
                /// <summary> 房间内玩家信息广播 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Player_Info_Room_Broadcast:
                    {
                        Msg_Logic2Client_PVE_Player_Info_Room_Broadcast packet = new Msg_Logic2Client_PVE_Player_Info_Room_Broadcast();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Player_Info_Room_Broadcast>(ms, packet);
                        PVE_Room_Player player = packet.player;
#if UNITY_EDITOR
                        Debug.Log("收到房间内玩家信息广播");
                        bool IsMysel = (DefaultRule.PlayerIDToAccountID(player.id) == GameApp.AccountID);
                        Debug.Log(StringBuilderTool.ToString("玩家 ID：", player.id,
                                                            (IsMysel ? "(自己)" : ""), " 名字：", player.name,
                                                            " 头像：", player.icon,
                                                            " 准备状态：", player.ready,
                                                            " 积分：", player.score,
                                                            " 获胜序号：", player.win_num,
                                                            " 游戏状态：", player.game_state));
#endif
                        for (int i = 0; i < GameApp.Instance.CurRoomPlayerLst.Count; i++)
                        {
                            if(GameApp.Instance.CurRoomPlayerLst[i].id == player.id)
                            {
                                GameApp.Instance.CurRoomPlayerLst[i] = player;
                            }
                        }
                        if (GameApp.Instance.FightUI != null)
                        {
                            GameApp.Instance.FightUI.ChangePlayerScore(player.id, player.score);
                        } 
                    }
                    break;
                /// <summary> 问题广播 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Question_Room_Broadcast:
                    {
                        Msg_Logic2Client_PVE_Question_Room_Broadcast packet = new Msg_Logic2Client_PVE_Question_Room_Broadcast();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Question_Room_Broadcast>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到问题广播", " 玩家ID：", packet.player_id, " 问题ID：", packet.question_id));
#endif
                        if (GameApp.Instance.FightUI != null)
                        {
                            GameApp.Instance.FightUI.OpenNormalQandA(packet.question_id);
                        }
                    }
                    break;
                /// <summary> 回答问题广播 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Answer_Question_Room_Broadcast:
                    {
                        Msg_Logic2Client_PVE_Answer_Question_Room_Broadcast packet = new Msg_Logic2Client_PVE_Answer_Question_Room_Broadcast();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Answer_Question_Room_Broadcast>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到回答问题广播", " 玩家ID：", packet.player_id, " 问题ID：", packet.question_id, " 是否正确：", packet.answer));
#endif
                        if (GameApp.Instance.FightUI != null)
                        {
                            if (GameApp.Instance.FightUI.QandAUI != null)
                            {
                                if (packet.question_id > 1000)
                                {
#if UNITY_EDITOR
                                    Debug.Log(StringBuilderTool.ToString("玩家[" + packet.player_id + "]关闭了答题界面"));
#endif
                                    GameApp.Instance.FightUI.QandAUI.AddCloseQAUINum();
                                }
                                else
                                {
                                    GameApp.Instance.FightUI.QandAUI.AnswerQuestionResult(packet.player_id, packet.answer);
                                }
                            }
                        }
                    }
                    break;
                /// <summary> 移动广播 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Move_Room_Broadcast:
                    {
                        Msg_Logic2Client_PVE_Move_Room_Broadcast packet = new Msg_Logic2Client_PVE_Move_Room_Broadcast();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Move_Room_Broadcast>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到移动广播", " 玩家ID：", packet.player_id, " 移动类型：", packet.type, " 步数：", packet.num));
#endif
                        if (GameApp.Instance.FightUI != null)
                        {
                            GameApp.Instance.FightUI.RandomMoveResult(packet.player_id, packet.type, packet.num);
                        }
                    }
                    break;
                /// <summary> 魔王的问题列表广播 </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Devil_Question_Room_Broadcast:
                    {
                        Msg_Logic2Client_PVE_Devil_Question_Room_Broadcast packet = new Msg_Logic2Client_PVE_Devil_Question_Room_Broadcast();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Devil_Question_Room_Broadcast>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到魔王的问题列表广播 挑战魔王的玩家ID：", packet.player_id));
                        for (int i = 0; i < packet.questions.Count; i++)
                        {
                            Debug.Log(StringBuilderTool.ToString("序号：", i, " 问题ID：", packet.questions[i]));
                        }
#endif
                        if (GameApp.Instance.FightUI != null)
                        {
                            GameApp.Instance.FightUI.OpenBossQandA(packet.player_id, packet.questions);
                        }
                    }
                    break;

                default:
#if UNITY_EDITOR
                    Debug.Log(StringBuilderTool.ToString("收到未处理的消息：" + curMsgType));
#endif
                    break;
                /// <summary> 加载状态广播 </summary>
                case MsgType.enum_Msg_Logic2Client_LoadState_Broadcast:
                    {
                        Msg_Logic2Client_LoadState_Broadcast packet = new Msg_Logic2Client_LoadState_Broadcast();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_LoadState_Broadcast>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log(StringBuilderTool.ToString("收到加载状态广播", " 玩家ID：", packet.playerID, " 加载状态：", packet.loadState));
#endif
                        switch(packet.loadState)
                        {
                            case 1:
                                {
                                    if (GameApp.Instance.LoadingDlg != null)
                                    {
                                        if (GameApp.Instance.CurRoomPlayerLoadStateLst.ContainsKey(packet.playerID))
                                        {
                                            GameApp.Instance.CurRoomPlayerLoadStateLst[packet.playerID] = 1;

                                            GameApp.Instance.LoadingDlg.SetPlayerLoadOver(packet.playerID);
                                        }

                                        bool isAllPlayerLoadingOver = true;
                                        foreach (KeyValuePair<ulong,int> pair in GameApp.Instance.CurRoomPlayerLoadStateLst)
                                        {
                                            if(pair.Value != 1)
                                            {
                                                isAllPlayerLoadingOver = false;
                                                break;
                                            }
                                        }
                                        if (isAllPlayerLoadingOver)
                                        {
                                            GameApp.Instance.LoadingDlg.UnconditionalHide();
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                /// <summary>
                /// PVE战役通关
                /// </summary>
                case MsgType.enum_Msg_Logic2Client_PVE_Finish_Res:
                    {
                        Msg_Logic2Client_PVE_Finish_Res packet = new Msg_Logic2Client_PVE_Finish_Res();
                        packet = common.Serializer.Deserialize<Msg_Logic2Client_PVE_Finish_Res>(ms, packet);
#if UNITY_EDITOR
                        Debug.Log("PVE战役通关");
                        Debug.Log("关卡ID：" + packet.m_id);
                        Debug.Log("战役通关信息：" + packet.m_result);
                        Debug.Log("结果：" + (LogicRes)packet.m_res);
#endif
                        if ((LogicRes)packet.m_res == LogicRes.Common_Process_Success)
                        {
#if UNITY_EDITOR
                            Debug.Log("PVE战役通关成功");
#endif
                            //if (packet.m_id / 100 == 5)
                            //{
                                //PlayerPrefs.SetInt(StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_StageProgress"), (int)packet.m_id);
                                //GameApp.Instance.PlayerData.m_player_pve.m_main_id = packet.m_id;

                                //GameApp.Instance.TIDS.SetMainStageProgress((int)packet.m_id);
                            //}
                        }
                    }
                    break;
            }

            if (RecvMsgCallBackDic.ContainsKey(curMsgType) && RecvMsgCallBackDic[curMsgType] != null)
                RecvMsgCallBackDic[curMsgType]();

            ms.Dispose();
        }
    }
    public static void AddRecvCBAction(MsgType mt, System.Action ac)
    {
        if (!RecvMsgCallBackDic.ContainsKey(mt))
            RecvMsgCallBackDic[mt] = null;
        RecvMsgCallBackDic[mt] += ac;
    }

    public static void RemoveRecvCBAction(MsgType mt, System.Action ac)
    {
        if (!RecvMsgCallBackDic.ContainsKey(mt))
            RecvMsgCallBackDic[mt] = null;
        RecvMsgCallBackDic[mt] -= ac;
    }
}
