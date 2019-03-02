using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq;

//全局数据管理类
//通用模块单件存放
public class GameApp : SimpleSingleton<GameApp>
{
    /// <summary> 服务器列表序号 </summary>
    public static int ServerIndex = 1;
    /// <summary> 账号id </summary>
    public static UInt32 AccountID = 0;
    /// <summary> 服务器的授权码 </summary>
    public static UInt32 ServerKey = 0;

    /// <summary> 处理发送数据的类 </summary>
    public static Game_Send_Client SendMsg = null;
    /// <summary> 处理接收数据的类 </summary>
    public static Game_Recv_Client RecvMsg = null;
    /// <summary> 客户端账户套接字 </summary>
    //public static SocketClient SocketClient_User = null;
    /// <summary> 客户端游戏套接字 </summary>
    public static SocketClient SocketClient_Game = null;

    /// <summary> 服务器传来的玩家数据 </summary>
    public common.Player PlayerData = null;

    /// <summary> 是否打印日志 </summary>
    public bool IsDebugLog
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
            return true;
#endif
        }
    }

    /// <summary> TalkingData账户 </summary>
#if UNITY_ANDROID
    public TDGAAccount TDAccount;
#endif
    /// <summary> 账号类型 </summary>
    public AccountType CurAccountType;

    /// <summary> 乐视SDK实例 </summary>
    public LeSDK LeSDKInstance = null;
    /// <summary> KOI 翻翻豆平台实例 </summary>
    public KOIPlatform Platform = null;

    /// <summary> 敏感词过滤  </summary>
    private FilterSensitiveWord _FilterSWInstance;
    public FilterSensitiveWord FilterSWInstance
    {
        get
        {
            if (_FilterSWInstance == null)
            {
                _FilterSWInstance = Singlton.GetInstance<FilterSensitiveWord>();
            }
            return _FilterSWInstance;
        }
    }

    /// <summary> 场景管理 </summary>
    public SceneControl SceneCtlInstance
    {
        get
        {
            if (_SceneCtlInstance == null)
            {
                _SceneCtlInstance = Singlton.GetInstance<SceneControl>();
            }
            return _SceneCtlInstance;
        }
    }
    private SceneControl _SceneCtlInstance;

    /// <summary> 声音控制器 </summary>
    public SoundController SoundInstance
    {
        get
        {
            if (_SoundInstance == null)
            {
                _SoundInstance = Singlton.GetInstance<SoundController>();
            }
            return _SoundInstance;
        }
    }
    private SoundController _SoundInstance;

    /// <summary> 通用Loading </summary>
    public Loading LoadingDlg = null;

    /// <summary> 通用消息框 </summary>
    public CommonMsgDlg CommonMsgDlg = null;

    /// <summary> 通用提示框 </summary>
    public CommonHintDlg CommonHintDlg = null;

    /// <summary> 通用警告框 </summary>
    public CommonWarningDlg CommonWarningDlg = null;

    /// <summary> 等待某事结束的提示框 </summary>
    public WaitLoadHintDlg WaitLoadHintDlg = null; 

    /// <summary> 获得道具提示框 </summary>
    public GetItemsDlg GetItemsDlg = null;

    /// <summary> 淡入淡出处理 </summary>
    public FadeHelper FadeHelperInstance
    {
        get
        {
            return Singlton.GetInstance<FadeHelper>();
        }
    }

    /// <summary> 走马灯（滚动公告） </summary>
    public RollingNoticeDlg RollingNoticeDlg = null;

    /// <summary> 获得随机角色名称 </summary>
    public RandomPlayerName RandomNameInstance = null;
    /// <summary> 获得随机宝宝龙名称 </summary>
    public RandomPetName RandomPetNameInstance = null;

    /// <summary> 登录界面 </summary>
    public UI_Login UILogin = null;

    /// <summary> 主页界面 </summary>
    public UI_HomePage HomePageUI = null;
    public HomePageScene HomePageSceneMgr = null;
    public UI_FirstChessGuide FirstChessGuideUI = null;

    /// <summary> 旅行界面 </summary>
    public UI_Travel TravelUI = null;
    public bool NeedShowGetOutHint = false;
    public bool NeedShowComeBackHint = false;
    public Dictionary<int, int> TravelAwardLst = new Dictionary<int, int>();

    /// <summary> 货币界面  </summary>
    public UI_Currency UICurrency = null;

    /// <summary> 战斗界面 </summary>
    public UI_Fight FightUI = null;

    public Dictionary<uint, MailItemInfo> MailItemsDic = new Dictionary<uint, MailItemInfo>();

    /// <summary> 主角数据 </summary>
    public PlayerData MainPlayerData
    {
        get
        {
            if (_MainPlayerData == null)
            {
                _MainPlayerData = new PlayerData(SerPlayerData.GetAvatarID(), SerPlayerData.GetName(), /*UI_NewMagicBook.CalcMagicPower(),*/ 0, 0);
            }
            return _MainPlayerData;
        }
    }
    private PlayerData _MainPlayerData;

    /// <summary> 机器人数据 </summary>
    public PlayerData AIRobotData
    {
        get
        {
            if (_AIRobotData == null)
            {
                _AIRobotData = new PlayerData(1001, "宝宝龙", /*(int)(UI_NewMagicBook.CalcMagicPower() * 0.8f),*/ 0, 0);
            }
            return _AIRobotData;
        }
    }
    private PlayerData _AIRobotData;

    /// <summary> 与机器人对战 </summary>
    public bool IsFightingRobot = false;

    /// <summary> 获得预配置参数 </summary>
    public int GetParameter(string KeyName)
    {
        foreach (KeyValuePair<int, ParameterConfig> pair in CsvConfigTables.Instance.ParameterCsvDic)
        {
            if (pair.Value.Name == KeyName)
                return pair.Value.Value;
        }
        return 0;
    }

    /// <summary> 魔卡持有状态 </summary>
    public Dictionary<int, int> CardHoldCountLst
    {
        get
        {
            if (_CardHoldCountLst == null)
            {
                _CardHoldCountLst = new Dictionary<int, int>();
                for (int i = 0; i < 40; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        _CardHoldCountLst.Add(50000 + i * 100 + j, 10);
                    }
                }
            }
            return _CardHoldCountLst;
        }
    }
    public void AddCardHoldCount(int key, int count)
    {
        if (GameApp.Instance.PlayerData != null)
        {
            GameApp.SendMsg.GMOrder(StringBuilderTool.ToString("AddItem ", key, " ", count));
        }
        else
        {
            if (_CardHoldCountLst == null)
                return;

            if (_CardHoldCountLst.ContainsKey(key))
            {
                _CardHoldCountLst[key] += count;
            }
        }
    }
    public Dictionary<int, int> _CardHoldCountLst = null;
    
    /// <summary> 当前房间内玩家信息 </summary>
    public List<common.PVE_Room_Player> CurRoomPlayerLst = new List<common.PVE_Room_Player>();
    public Dictionary<ulong, int> CurRoomPlayerLoadStateLst = new Dictionary<ulong, int>();

    /// <summary> 当前战斗关卡配置 </summary>
    public StageConfig CurFightStageCfg = null;

    /// <summary> 任务数据 </summary>
    public TaskInfoDataStruct TIDS
    {
        get
        {
            if (_TIDS == null)
            {
                _TIDS = new TaskInfoDataStruct();
#if UNITY_EDITOR
                Debug.Log("当前任务数据：");
                Debug.Log(_TIDS.TaskInfo);
#endif
            }
            return _TIDS;
        }
    }
    private TaskInfoDataStruct _TIDS = null;
}