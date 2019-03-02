﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System;
//引入此程序集可以调用IOS代码了。

public class CompatibilityIP
{
    [DllImport("__Internal")]
    private static extern string getIPv6(string host, string port);

    private static string GetIPv6(string host, string port)
    {
    #if UNITY_IPHONE && !UNITY_EDITOR
        return getIPv6(host, port);
    #endif
        return host;
    }

    public static void GetIpType(string serverIp, string serverPort, out string newServerIp, out AddressFamily newServerAddressFamily)
    {
        newServerAddressFamily = AddressFamily.InterNetwork;
        newServerIp = serverIp;
        try
        {
            string mIPv6 = GetIPv6(serverIp, serverPort);
            if (!string.IsNullOrEmpty(mIPv6))
            {
                string[] strTemp = Regex.Split(mIPv6, "&&");
                if (strTemp.Length >= 2)
                {
                    string type = strTemp[1];
                    if (type == "ipv6")
                    {
                        newServerIp = strTemp[0];
                        newServerAddressFamily = AddressFamily.InterNetworkV6;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}