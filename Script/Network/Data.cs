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

public class MsgCls
{
    public byte[] byteBuff;

    public int send_num = 0;

    public MsgCls()
    {
    }
    public void Clear()
    {
        if (byteBuff != null)
            Array.Clear(byteBuff, 0, byteBuff.Length);
        send_num = 0;
    }

    /// <summary>
    /// 协议包转化为字节
    /// </summary>
    public void ObjectToByte<T>(T ProtocolPacket, int ProtocolType) where T : IMessage
    {
        MemoryStream ms = new MemoryStream();
        Serializer.Serialize(ms, ProtocolPacket);

        byteBuff = new byte[ms.Length + 6];

        Endian.WriteShort(byteBuff, 0, (short)(ms.Length + 4));
        Endian.WriteInt(byteBuff, 2, ProtocolType);

        ms.Seek(0, SeekOrigin.Begin);
        ms.Read(byteBuff, 6, (int)ms.Length);
     
        send_num = byteBuff.Length;

        ms.Dispose();
    }
}