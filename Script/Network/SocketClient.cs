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

public enum ConnectResultType
{
	Msg_Connect_Break,
	Msg_Connect_Success,
	Msg_Connect_Fail
}

public enum SocketClientType
{
    Socket_Null,
    Socket_User,
    Socket_Game
}

public enum ReconnectType
{
    Renull,
    Reauto,
    Reexteral
}

public class ConnectResultMsg
{
    public bool isReconnect = false;
	public ConnectResultType type;
	public object param;

    public ConnectResultMsg(ConnectResultType _t, object _p,bool _isR)
    {
		type = _t;
		param = _p;
	}
}

public class SocketClient
{
    public static Dictionary<SocketClientType,SocketClient> SocketDic = new Dictionary<SocketClientType, SocketClient>();
    public SocketClientType mSocketClientType = SocketClientType.Socket_Null;
	/// <summary>
	/// 客户端连接服务器的计时器，超时自动断开连接
	/// </summary>
	//private ManualResetEvent TimeObjOut = new ManualResetEvent(true);

	//private ManualResetEvent PingObjOut = new ManualResetEvent(true);

    public ReconnectType mReconnectType = ReconnectType.Reexteral;

	public System.Threading.Timer timer = null;
	
	public ManagMsgCls mManagMsgCls = null;
	
	public Socket socket = null;

	public string ClaName = "";

	public string ip = "";
	
	public int port = 0;

	/// <summary>
	/// 最长连接时间
	/// </summary>
	float ConnectTime = 5f;

	/// <summary>
	/// ping包的最长判断时间
	/// </summary>
    float RelayTime = Const.RelayTime;

	/// <summary>
	/// ping包发包的频率
	/// </summary>
    float PingTime = Const.PingTime;

	//int curSendNum = 0;
	
	//bool isOver = false;

	/// <summary>
	/// 客户端连接服务器的次数
	/// </summary>
    public int ReconnectNum = 0;

    public bool IsReconnect = false;

	/// <summary>
	/// 客户端是否正在连接服务器
	/// </summary>
	public bool IsConnecting = false;

	/// <summary>
	/// 客户端连接服务器是否成功
	/// </summary>
	public bool IsInitSuccess = false;

	/// <summary>
	/// ping包是否开始记时
	/// </summary>
	public bool IsPinging = false;
	
	/// <summary>
	/// 实例化，并连接服务器
	/// </summary>
	public SocketClient(string RemoteIP,int RemotePort,string cn)
	{
        port = RemotePort;
		ip = RemoteIP;
        ClaName = cn;

        InitSocketClientType();
		StartInitSocket();
	}

    void InitSocketClientType()
    {
        if ("SocketClient_User" == ClaName)
        {
            mReconnectType = ReconnectType.Reexteral;
            mSocketClientType = SocketClientType.Socket_User;
        }
        else if ("SocketClient_Game" == ClaName)
        {
            mReconnectType = ReconnectType.Reexteral;
            mSocketClientType = SocketClientType.Socket_Game;
        }
    }

	void StartInitSocket()
	{
		if(IsConnecting)
		{
            Debug.Log(ClaName + "Socket is Init And Connecting By Someone");
            return;
		}

		if(IsInitSuccess)
        {
            Debug.Log(ClaName + "Socket IsInitSuccess is True");
            return;
        }

        GameApp.SendMsg.StartWaitUI();

		IsConnecting = true;
        IsPinging = false;
		InitSocket();
	}

	void InitSocket()
	{
		Close();

#if NETFX_CORE
		socket = new Socket();

		try
        {
            socket.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), socket); 
		}
		catch(Exception e)
		{
            Debug.LogError(ClaName + e.ToString() + "-----BegainConnectException");
        }
#elif UNITY_IPHONE// && !UNITY_EDITOR
        try
        {
            string serverIp;
            AddressFamily ipType;
            CompatibilityIP.GetIpType("mokaserver01.baobaolong.club", "21001", out serverIp, out ipType);
            Debug.Log("\n");
            Debug.Log("LOG_1_serverIp:" + serverIp + " ipType:" + ipType);
            socket = new Socket(ipType, SocketType.Stream, ProtocolType.Tcp);
            Debug.Log("LOG_2_socket:" + (socket == null).ToString());
            IPAddress address = IPAddress.Parse(serverIp);
            IPEndPoint point = new IPEndPoint(address, 21001);
            Debug.Log("LOG_3_address:" + address.ToString() + " point:" + point);

		    try
            {
                socket.BeginConnect(point, new AsyncCallback(ConnectCallback), socket);
		    }
		    catch(Exception e)
		    {
                Debug.LogError(ClaName + e.ToString() + "-----BegainConnectException");
            }
        }
        catch(Exception e1)
		{
            Debug.Log("LOG_4_IPV6形式的创建Socket失败，现使用常规方式创建Socket...");

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		    try
            {
                socket.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), socket); 
		    }
		    catch(Exception e2)
		    {
                Debug.LogError(ClaName + e2.ToString() + "-----BegainConnectException");
            }
        }
#else
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            socket.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), socket);
        }
        catch (Exception e)
        {
            Debug.LogError(ClaName + e.ToString() + "-----BegainConnectException");
        }
#endif
	}

	/// <summary>
	/// 连接服务器成功后的回调方法
	/// </summary>
	public void ConnectCallback(IAsyncResult asyncConnect)
	{
		try
		{
			socket = (Socket)asyncConnect.AsyncState;
			socket.EndConnect(asyncConnect);
            ConnectSuccess();

            mManagMsgCls = new ManagMsgCls(this);
			StartReceive();
		}
		catch(Exception e)
		{
            Debug.LogError(ClaName + " " + e.ToString() + " EndConnectException");
		}
	}

	/// <summary>
	/// 发送消息
    /// </summary>
	public void StartSendMessage<T>(T ProtocolPacket,int ProtocolType) where T : IMessage
    {
		if(!IsConnected)
			return;

        MsgCls msgcls = new MsgCls();
		msgcls.ObjectToByte<T>(ProtocolPacket,ProtocolType);
		
		AsyncSendMsg(msgcls);
	}

	public void AsyncSendMsg(MsgCls msgcls,int offset = 0)
	{
		if(!IsConnected)
			return;
        
        try
		{
			socket.BeginSend (msgcls.byteBuff,offset,msgcls.send_num,0,new AsyncCallback(SendMessage),msgcls);
		}
		catch(Exception e)
		{
            Debug.LogError(ClaName + " " + e.ToString() + "SendMessageInAsync");
        }
	}

	public void SendMessage(IAsyncResult asyncSend)
	{
		MsgCls msgcls = (MsgCls)asyncSend.AsyncState;

        try
		{
			int send_num = socket.EndSend (asyncSend);
			if(send_num < msgcls.send_num)               //异步发包的时候没有一次性的发完
			{
				msgcls.send_num -= send_num;             //再次发包时要发的字节数
				AsyncSendMsg(msgcls,send_num);
			}
		}
		catch(Exception e)
		{
            Debug.LogError(ClaName + " " + e.ToString() + " EndSendException");
        }
    }

    /// <summary>
    /// 开始接受信息
    /// </summary>
    byte[] receiveBuf = new byte[1024];
	public void StartReceive()
    {
		try
		{
            socket.BeginReceive(receiveBuf, 0, receiveBuf.Length, 0, new AsyncCallback(EndReceiveSorket), receiveBuf);
		}
		catch(Exception e)
		{
            Debug.LogError(ClaName + " " + e.ToString() + " BegainReceiveException");
		}
	}

	public void EndReceiveSorket (IAsyncResult asyncReceive)
	{
		try
		{
            if (socket == null) 
                return;

			int receive_num = socket.EndReceive(asyncReceive);

			if (receive_num != 0)
			{
				mManagMsgCls.UpdatePacket(receiveBuf, 0, receive_num);
				StartReceive();
			}
			else
			{
                IsInitSuccess = false;
                Debug.Log(ClaName + " May be disconnect by the remote");
			}
		}
		catch(Exception e)
		{
            Debug.LogError(ClaName + " " + e.ToString() + " EndReceiveException");
		}
	}

	/// <summary>
	/// 是否与服务器连接
	/// </summary>
	public bool IsConnected
	{
        get
        {
            if (socket == null)
                return false;

            return socket.Connected;
        }
	}

	/// <summary>
	/// 与服务器重连
	/// </summary>
	public void Reconnect()
	{
        if (mReconnectType == ReconnectType.Renull)
            return;

        IsReconnect = true;
		StartInitSocket();
	}

    /// <summary>
    /// 与服务器重连
    /// </summary>
    public void ForceReconnect()
    {
        Close();
        if (mReconnectType == ReconnectType.Renull)
            return;
        
        IsReconnect = true;
        StartInitSocket();
    }

	public void Close(bool isOver = false)
	{
		if(socket == null)
			return;

        Debug.Log(ClaName + " Close");
        try
        {
            if (socket.Connected)
            {
#if !NETFX_CORE
                socket.Shutdown(SocketShutdown.Both);
#endif
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message + "-->" + ex.StackTrace);
        }
        finally
        {
            socket.Close();
            socket = null;
            IsPinging = false;
            IsInitSuccess = false;
        }
	}

	/// <summary>
	/// 客户端记时操作
	/// </summary>
	public void ClientTimer()
	{
		ConnectTimer();
		PingTimer();
	}

    #region ping包相关
	/// <summary>
	/// 心跳包计时器
	/// </summary>
	public void PingTimer()
	{
		if(!IsPinging)
		{
            PingTime = Const.PingTime;
            RelayTime = Const.RelayTime;
			return;
		}

		PingTime -= Time.deltaTime;
		if(PingTime < 0)
		{
            PingTime = Const.PingTime;
            Msg_Ping_Req packet = new Msg_Ping_Req();
            packet.m_ms = (ulong)(Time.realtimeSinceStartup * 1000f);
            StartSendMessage<Msg_Ping_Req>(packet, (int)MsgType.enum_Msg_Ping_Req);
		}

		RelayTime -= Time.deltaTime;
		if(RelayTime < 0)
		{
            RelayTime = Const.RelayTime;
			PingTimeOut();
		}
	}

	/// <summary>
	/// ping包超时，连接中断
	/// </summary>
	void PingTimeOut()
	{
		Close();

        if (mReconnectType == ReconnectType.Reauto)
        {
            Reconnect();
        }
        
        Game_Recv_Client.OtherList.Add(new ConnectResultMsg(ConnectResultType.Msg_Connect_Break,this,false));

        Debug.Log(ClaName + " Msg_Connect_Break");
        Debug.Log(ClaName + " ByteList Count " + (mManagMsgCls != null ? mManagMsgCls.ByteList.Count.ToString() : "Null"));
    }

	/// <summary>
	/// 收包正常，连接正常
	/// </summary>
	public void PingTimeIn()
	{
        RelayTime = Const.RelayTime;
	}
    #endregion

    #region socket连接相关
	/// <summary>
	/// 与服务器连接计时器
	/// </summary>
	public void ConnectTimer()
	{
		if(IsConnecting)
		{
			ConnectTime -= Time.deltaTime;
			if(ConnectTime <= 0)
			{
				ConnectTime = 5f;
				ConnectFail();
			}
		}
		else
		{
			ConnectTime = 5f;
		}
	}

	/// <summary>
	/// 与服务器连接成功
	/// </summary>
	void ConnectSuccess()
	{
		ReconnectNum = 0;
		IsPinging = true;
		IsConnecting = false;
        IsInitSuccess = true;
        SocketManager.Instance.RemoveFromReconnectSocketList(this);
        Game_Recv_Client.OtherList.Add(new ConnectResultMsg(ConnectResultType.Msg_Connect_Success,(object)this,IsReconnect));
        IsReconnect = false;
    }

	/// <summary>
	/// 与服务器连接失败
	/// </summary>
	void ConnectFail()
	{
        Debug.Log(ClaName + " ConnectFail");
        IsInitSuccess = false;

		if(ReconnectNum < 3)
		{
			ReconnectNum += 1;
			InitSocket();
		}
		else
		{
            ReconnectNum = 0;
            IsReconnect = false;
			IsConnecting = false;
            Game_Recv_Client.OtherList.Add(new ConnectResultMsg(ConnectResultType.Msg_Connect_Fail,(object)this,false));
		}
	}
    #endregion
    public static void RemoveSocket(SocketClient rsc)
    {
        if (rsc == null)
            return;

        rsc.Close();
        if (SocketDic.ContainsKey(rsc.mSocketClientType))
        {
            SocketDic.Remove(rsc.mSocketClientType);
        }
        rsc = null;
    }

    public static void RemoveSocket(SocketClientType _type)
    {
        SocketClient sc = null;
        if (!SocketDic.TryGetValue(_type, out sc))
        {
            SocketDic.Remove(_type);
            sc.Close();
        }
    }
    /// <summary>
    /// 实例化socket类
    /// </summary>
    public static SocketClient SocketConnect(string RemoteIP,int RemotePort,string cn)
    {
        SocketClient sc = null;
        SocketClientType _type = GetSocketClientType(cn);

        if (!SocketDic.TryGetValue(_type, out sc))
        {
            SocketDic[_type] = new SocketClient(RemoteIP, RemotePort, cn);
            sc = SocketDic[_type];
        }
        else if (sc.ip == RemoteIP && sc.port == RemotePort)
        {
            if (!sc.IsConnected)
                sc.StartInitSocket();
        }
        else
        {
            sc.Close();
            SocketDic[_type] = new SocketClient(RemoteIP, RemotePort, cn);
            sc = SocketDic[_type];
        }
        return sc;
    }

    public static SocketClientType GetSocketClientType(string cn)
    {
        SocketClientType sct = SocketClientType.Socket_Null;
        if ("SocketClient_User" == cn)
            sct = SocketClientType.Socket_User;        
        else if ("SocketClient_Game" == cn)
            sct = SocketClientType.Socket_Game;
        return sct;
    }
}
