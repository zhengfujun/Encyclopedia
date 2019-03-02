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
/// <summary>
/// 封装一个完整的协议包
/// </summary>
public class RecPacketCls
{
    public int idx = 0;
	public int iFlag = -1;
	public MemoryStream ms = null;
	public SocketClient mSocketClient = null;
	
    public RecPacketCls(SocketClient _sc,MemoryStream _ms,int _fg,int _idx)
	{
		ms = _ms;
        idx = _idx;
		iFlag = _fg;
		mSocketClient = _sc;
	}
}

/// <summary>
/// 对收到的字节进行解析处理
/// </summary>
public class ManagMsgCls
{
    public List<RecPacketCls> RecPacketList = new List<RecPacketCls>();
	public List<byte> ByteList = new List<byte>();

	SocketClient mSocketClient = null;
    int count = 0;
	
	public ManagMsgCls(SocketClient _sc)
	{
		mSocketClient = _sc;
	}
	
	public void UpdatePacket(byte[] ByteBuff, int offset, int count)
	{
        for (int i = 0; i < count; ++i)
            ByteList.Add(ByteBuff[offset + i]);

        ManageRecMsgBefore();
	}
	
	/// <summary>
	/// 做拼包粘包操作
	/// </summary>
	void ManageRecMsgBefore()
	{
		if(ByteList.Count < 2)
			return;
		
		byte[] LenByte = new byte[2];
		ByteList.CopyTo(0,LenByte,0,2);
		short ProtoLen = BitConverter.ToInt16(LenByte,0);
        ProtoLen = Endian.Switch(ProtoLen);
		
		if(ProtoLen + 2 <= ByteList.Count)
		{
			byte[] ByteBuff = new byte[ProtoLen + 2];
			ByteList.CopyTo(0,ByteBuff,0,ProtoLen + 2);
			ByteList.RemoveRange(0,ProtoLen + 2);
			ManageCompleteRec(ByteBuff);
		}
	}

	/// <summary>
	/// 处理一个完整包
	/// </summary>
	void ManageCompleteRec(byte[] ByteBuff)
	{
		byte[] FlagByte = new byte[4];
		byte[] DataByte = new byte[ByteBuff.Length - 6];
		
		Array.Copy(ByteBuff,2,FlagByte,0,FlagByte.Length);
		Array.Copy(ByteBuff,6,DataByte,0,DataByte.Length);
		
		int iFlag = BitConverter.ToInt32(FlagByte,0);
        iFlag = Endian.Switch(iFlag);
		
		MemoryStream ms = new MemoryStream();
		ms.Write(DataByte,0,DataByte.Length);
		ms.Seek(0,SeekOrigin.Begin);
        if (mSocketClient == null)
		{
            Debug.Log("Socket is Null " + ((MsgType)iFlag).ToString());
			return;
        }
        RecPacketCls rec = new RecPacketCls(mSocketClient,ms,iFlag,++count);
        lock (Game_Recv_Client.RecPacketList) 
        {
            RecPacketList.Add(rec);

            Game_Recv_Client.RecPacketList.Add(rec);
            Sort();
            //Debug.Log(mSocketClient.ClaName + " Receive " + ((MsgType)iFlag).ToString() + "------ByteListCount is------" + ByteList.Count.ToString());
            //Debug.Log("ByteList Leave byte is   :    " + ByteList.Count);
            ManageRecMsgBefore();
        }
	}

    public void RemovePacket(RecPacketCls _rpc)
    {
        if (RecPacketList.Contains(_rpc))
            RecPacketList.Remove(_rpc);
    }

    void Sort()
    {
        if (mSocketClient == null)
            return;

        if (mSocketClient.mSocketClientType != Game_Recv_Client.mPacketSortType_Socekt)
            return;
        
        if (Game_Recv_Client.mPacketSortType_Socekt != SocketClientType.Socket_Null)
        {
            List<RecPacketCls> TempList = new List<RecPacketCls>();
            foreach (RecPacketCls rpc in RecPacketList)
            {
                if (Game_Recv_Client.RecPacketList.Contains(rpc))
                {
                    TempList.Add(rpc);
                    Game_Recv_Client.RecPacketList.Remove(rpc);
                }
            }
            TempList.AddRange(Game_Recv_Client.RecPacketList);
            Game_Recv_Client.RecPacketList.Clear();
            Game_Recv_Client.RecPacketList.AddRange(TempList);
        }
    }
}