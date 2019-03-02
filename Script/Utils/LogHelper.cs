using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Net.Sockets;
public class LogHelper
{
    //brief IP地址
    public static string ipAddress = "192.168.31.197";
    //brief 端口
    public static int ipPort = 22222;
    /// <summary>
    /// 输出网络日志
    /// </summary>
    /// <param name="message">需要发送的消息</param>
    public static void NetLog(object message)
    {
        if (message == null) 
            return;

        NGUIDebug.Log(message);

        //新开一个线程开始UDP发送消息，可用网络调试小助手接收消息
        new Thread(delegate()
        {
            UdpClient udp = new UdpClient(ipAddress, ipPort);
            byte[] data = Encoding.Default.GetBytes(Convert.ToString(message));
            udp.Send(data, data.Length);
            udp.Close();
        }) { IsBackground = true }.Start();
    }
}