using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReconnectMsg
{
    public SocketClient sc = null;
    public string content = "";
    public ReconnectMsg(SocketClient _sc,string _content)
    {
        sc = _sc;
        content = _content;
    }
}

public class SocketManager : SimpleSingleton<SocketManager>
{
    public System.DateTime lastPacketTime = System.DateTime.Now;
    public System.DateTime currPacketTime = System.DateTime.Now;
    List<SocketClient> ReconnectSocketList = new List<SocketClient>();
    List<ReconnectMsg> ReconnectMsgList = new List<ReconnectMsg>();

    public void AddReconnectSocket(string str, SocketClient curSocketClient)
    {
        if (!Application.isPlaying)
            return;

        if (ReconnectSocketList.Contains(curSocketClient))
            return;
        
        ReconnectSocketList.Add(curSocketClient);
        ReconnectMsgList.Add(new ReconnectMsg(curSocketClient,str));
        ReconnectServer();
    }

    public void RemoveFromReconnectSocketList(SocketClient sc)
    {
        if (ReconnectSocketList.Contains(sc))
            ReconnectSocketList.Remove(sc);
    }

    ReconnectMsg _reconnectMsg = null;
    /// <summary>
    /// 重连服务器
    /// </summary>
    public void ReconnectServer()
    {
        if (ReconnectMsgList.Count == 0)
            return;
        
        if (_reconnectMsg != null || GameObject.Find("CommonMag") != null)
            return;
        
        _reconnectMsg = ReconnectMsgList[0];
        ReconnectMsgList.RemoveAt(0);

        if (!_reconnectMsg.sc.IsConnecting && !_reconnectMsg.sc.IsConnected)
        {
            if (_reconnectMsg.sc.mReconnectType == ReconnectType.Reauto)
            {
                _reconnectMsg.sc.IsInitSuccess = false;
                _reconnectMsg.sc.Reconnect();
                _reconnectMsg = null;
            }
            else if (_reconnectMsg.sc.mReconnectType == ReconnectType.Reexteral || _reconnectMsg.sc.mReconnectType == ReconnectType.Renull)
            {
                GameApp.Instance.CommonMsgDlg.OpenMsgBox(_reconnectMsg.content, ConfirmReconnectServer);
            }
        }
    }

    void ConfirmReconnectServer(bool isConnect)
    {
        if (_reconnectMsg == null || _reconnectMsg.sc == null)
        {
            _reconnectMsg = null;
            return;
        }

        if (isConnect)
        {
            _reconnectMsg.sc.IsInitSuccess = false;
            
            _reconnectMsg.sc.Reconnect();
        }
        else
        {
            Application.Quit();
        }

        _reconnectMsg = null;
    }

    public bool isForceReconnect = false;
    public void ForceReconnect(bool isReconnect)
    {
        currPacketTime = System.DateTime.Now;
        if ((currPacketTime - lastPacketTime).TotalSeconds < 60)
        {
            return;
        }

        if (!isReconnect)
            return;

        isForceReconnect = true;
        ReconnectMsgList.Clear();
        ReconnectSocketList.Clear();
        foreach (KeyValuePair<SocketClientType,SocketClient> pair in SocketClient.SocketDic)
        {
            if (pair.Value != null)
            {
                pair.Value.Close();
            }
        }
                
        if (GameApp.Instance.CommonMsgDlg)
        {
            GameApp.Instance.CommonMsgDlg.Close();
        }
        foreach (KeyValuePair<SocketClientType,SocketClient> pair in SocketClient.SocketDic)
        {
            if (pair.Value != null)
            {
                pair.Value.ForceReconnect();
            }
        }
    }
}
