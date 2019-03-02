using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//全局常(变)量
public static class Const
{
    #region _规划到ClientConstConfig.ini中的变量，不用在此赋值
    //Platform
    //平台地址类型
    public static int PlatformUrlType;

    //AccountServer
    //账号服IP
    //public static string AccountServerIP;
    //账号服端口
    //public static int AccountServerPort;
    
    //GameServer
    //游戏服IP
    public static string GameServerIP;
    //游戏服端口
    public static int GameServerPort;

    //Account
    //登录账号类型
    public static int AccountType;
    //自定义登录账号
    public static string CustomAccount;
    //自定义登录密码
    public static string CustomPassword;

    //Common
    //是否处于开发调试模式
    public static bool IsDevelopMode;

    //Variable
    //战斗主相机方位角改变时长
    public static float ChangeAzimuthDuration;
    #endregion

    //语言
    //SimplifiedChinese 简体中文
    //TraditionalChinese 繁体中文
    //English 英文
    public static string Language = "SimplifiedChinese";
    
    //低频更新时间间隔
    public static float LFInterval = 0.04f;
    //低宽高比的屏幕是否全屏
    public static bool IsLowAspectRatioFullScreen = true;

    //ping包发包的频率
    public const float PingTime = 1f;
    //ping包的最长判断时间
    public const float RelayTime = 60f;

    //水下生物卡片初始ID
    public static int FishCardFirshID = 50201;
}