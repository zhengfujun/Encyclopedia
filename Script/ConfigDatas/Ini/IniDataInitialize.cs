using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 加载Ini格式的配置文件
/// </summary>
public class IniDataInitialize : MonoBehaviour
{
    void Awake()
    {
        IniParser ClientConstConfig = new IniParser("Config/IniConfigs/ClientConstConfig");

        Const.PlatformUrlType = int.Parse(ClientConstConfig.GetSetting("Platform", "PlatformUrlType"));

        //Const.AccountServerIP = ClientConstConfig.GetSetting("AccountServer", "IP");
        //Const.AccountServerPort = int.Parse(ClientConstConfig.GetSetting("AccountServer", "Port"));

        Const.GameServerIP = ClientConstConfig.GetSetting("GameServer", "IP");
        Const.GameServerPort = int.Parse(ClientConstConfig.GetSetting("GameServer", "Port"));

        Const.AccountType = int.Parse(ClientConstConfig.GetSetting("Account", "AccountType"));
        Const.CustomAccount = ClientConstConfig.GetSetting("Account", "CustomAccount");
        Const.CustomPassword = ClientConstConfig.GetSetting("Account", "CustomPassword");

        Const.IsDevelopMode = bool.Parse(ClientConstConfig.GetSetting("Common", "IsDevelopMode"));

        Const.ChangeAzimuthDuration = float.Parse(ClientConstConfig.GetSetting("Variable", "ChangeAzimuthDuration"));
    }
}
