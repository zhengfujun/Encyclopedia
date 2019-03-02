using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using common;

public enum EPetSex
{
    eNull,
    eMale,
    eFemale
}

public enum EDuration
{
    e15Minute,
    e30Minute,
    e45Minute,
    e60Minute
}

public class PetInfo
{
    static public PetInfo PetInfoInstance = new PetInfo();

    public string Name
    {
        get
        {
            if (PlayerPrefs.HasKey(key_Name))
            {
                return PlayerPrefs.GetString(key_Name);
            }
            else
            {
                return "宝宝龙";
            }
        }
        set
        {
            PlayerPrefs.SetString(key_Name, value);
        }
    }

    public EPetSex Sex
    {
        get
        {
            if (PlayerPrefs.HasKey(key_Sex))
            {
                return (EPetSex)PlayerPrefs.GetInt(key_Sex);
            }
            else
            {
                return EPetSex.eNull;
            }
        }
        set
        {
            PlayerPrefs.SetInt(key_Sex, (int)value);
        }
    }

    public string AgeYeah
    {
        get
        {
            if (PlayerPrefs.HasKey(key_AgeYeah))
            {
                return PlayerPrefs.GetString(key_AgeYeah);
            }
            else
            {
                return "2006";
            }
        }
        set
        {
            PlayerPrefs.SetString(key_AgeYeah, value);
        }
    }

    public string AgeMonth
    {
        get
        {
            if (PlayerPrefs.HasKey(key_AgeMonth))
            {
                return PlayerPrefs.GetString(key_AgeMonth);
            }
            else
            {
                return "1";
            }
        }
        set
        {
            PlayerPrefs.SetString(key_AgeMonth, value);
        }
    }

    private string key_Name;
    private string key_Sex;
    private string key_AgeYeah;
    private string key_AgeMonth;
    public PetInfo()
    {
        uint AccountID = SerPlayerData.GetAccountID();
        key_Name = StringBuilderTool.ToString(AccountID, "_PetInfo_Name");
        key_Sex = StringBuilderTool.ToString(AccountID, "_PetInfo_Sex");
        key_AgeYeah = StringBuilderTool.ToString(AccountID, "_PetInfo_AgeYeah");
        key_AgeMonth = StringBuilderTool.ToString(AccountID, "_PetInfo_AgeMonth");
    }
}

public class SleepInfo
{
    static public SleepInfo SleepInfoInstance = new SleepInfo();

    public string SleepBeginTime
    {
        get
        {
            if (PlayerPrefs.HasKey(key_SleepBeginTime))
            {
                return PlayerPrefs.GetString(key_SleepBeginTime);
            }
            else
            {
                return "20:00";
            }
        }
        set
        {
            PlayerPrefs.SetString(key_SleepBeginTime, value);
        }
    }

    public string SleepEndTime
    {
        get
        {
            if (PlayerPrefs.HasKey(key_SleepEndTime))
            {
                return PlayerPrefs.GetString(key_SleepEndTime);
            }
            else
            {
                return "06:00";
            }
        }
        set
        {
            PlayerPrefs.SetString(key_SleepEndTime, value);
        }
    }

    public EDuration EnableUseDuration
    {
        get
        {
            if (PlayerPrefs.HasKey(key_EnableUseDuration))
            {
                return (EDuration)PlayerPrefs.GetInt(key_EnableUseDuration);
            }
            else
            {
                return EDuration.e15Minute;
            }
        }
        set
        {
            PlayerPrefs.SetInt(key_EnableUseDuration, (int)value);
        }
    }

    public EDuration RestUseDuration
    {
        get
        {
            if (PlayerPrefs.HasKey(key_RestUseDuration))
            {
                return (EDuration)PlayerPrefs.GetInt(key_RestUseDuration);
            }
            else
            {
                return EDuration.e15Minute;
            }
        }
        set
        {
            PlayerPrefs.SetInt(key_RestUseDuration, (int)value);
        }
    }

    private string key_SleepBeginTime;
    private string key_SleepEndTime;
    private string key_EnableUseDuration;
    private string key_RestUseDuration;
    public SleepInfo()
    {
        uint AccountID = SerPlayerData.GetAccountID();
        key_SleepBeginTime = StringBuilderTool.ToString(AccountID, "_PetInfo_SleepBeginTime");
        key_SleepEndTime = StringBuilderTool.ToString(AccountID, "_PetInfo_key_SleepEndTime");
        key_EnableUseDuration = StringBuilderTool.ToString(AccountID, "_PetInfo_EnableUseDuration");
        key_RestUseDuration = StringBuilderTool.ToString(AccountID, "_PetInfo_RestUseDuration");
    }
}

public class UI_NewSetting : MonoBehaviour
{
    public UIAppearEffect AppearEffect;
    
    public UILabel PlayerName;
    public UILabel PlayerLvLab;
    public UILabel AccountId;
    public UISprite HeadPortrait;
    public UISlider ExpPB;
    public UILabel ExpVal;

    public UIInput PlayerNameInp;

    public UIToggle[] MusicSwitch;
    public UIToggle[] SeSwitch;

    public UIToggle PetInfoTab;
    public UIInput PetNameInp;
    public UIToggle[] PetSexSwitch;
    public RollLabelUnit[] PetAgeYeahRLab;
    private string CurShowAgeYeahStr;
    public RollLabelUnit[] PetAgeMonthRLab;
    private string CurShowAgeMonthStr;
    public UIButton PetModifBtn;

    public UIToggle SleepSetTab;
    public RollLabelUnit[] SleepBeginTimeRLab;
    private string CurShowSleepBeginTimeStr;
    public RollLabelUnit[] SleepEndTimeRLab;
    private string CurShowSleepEndTimeStr;
    public UIToggle[] EnableUseDurationSwitch;
    public UIToggle[] RestUseDurationSwitch;
    public UIButton SleepModifBtn;

    public UIInput OrderInp;

    public GameObject SystemSetTab;

    /// <summary> 监测睡眠时间段 </summary>
    static public void CheckSleepTime()
    {
        string[] tempSBTSplit = SleepInfo.SleepInfoInstance.SleepBeginTime.Split(':');
        DateTime SBT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(tempSBTSplit[0]), int.Parse(tempSBTSplit[1]), 0);
        
        string[] tempSETSplit = SleepInfo.SleepInfoInstance.SleepEndTime.Split(':');
        DateTime SET = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(tempSETSplit[0]), int.Parse(tempSETSplit[1]), 0);
        TimeSpan oneDay = new TimeSpan(1,0,0,0);
        SET += oneDay;
        if (DateTime.Now >= SBT && DateTime.Now <= SET)
        {
            if (GameApp.Instance.HomePageUI != null)
                GameApp.Instance.CommonWarningDlg.OpenWarningBox(StringBuilderTool.ToInfoString("当前为睡眠时段(", SleepInfo.SleepInfoInstance.SleepBeginTime, "-", SleepInfo.SleepInfoInstance.SleepEndTime, ")，禁止使用！"));
        }
    }

    /// <summary> 监测使用时长 </summary>
    static public void CheckUseDuration()
    {
        float d = 0;
        switch(SleepInfo.SleepInfoInstance.EnableUseDuration)
        {
            case EDuration.e15Minute:
                d = 15 * 60;
                break;
            case EDuration.e30Minute:
                d = 30 * 60;
                break;
            case EDuration.e45Minute:
                d = 45 * 60;
                break;
            case EDuration.e60Minute:
                d = 60 * 60;
                break;
        }
        if (Time.realtimeSinceStartup > d)
        {
            if (GameApp.Instance.HomePageUI != null)
                GameApp.Instance.CommonWarningDlg.OpenWarningBox(StringBuilderTool.ToString("本次使用时长已超出设置时长(", d / 60, "分钟)"));
        }
    }

    void Start()
    {
        PlayerName.text = SerPlayerData.GetName();
        AccountId.text = SerPlayerData.GetAccountID().ToString();

        /////////////////////////////////////////////////////////////
        bool isMusicMuteOn = GameApp.Instance.SoundInstance.BgmMute;
        MusicSwitch[0].value = !isMusicMuteOn;
        MusicSwitch[1].value = isMusicMuteOn;

        bool isSeMuteOn = GameApp.Instance.SoundInstance.SeMute;
        SeSwitch[0].value = !isSeMuteOn;
        SeSwitch[1].value = isSeMuteOn;

        InitPetInfo();
        InvokeRepeating("RefreshPetModifBtnState", 1f, 0.2f);

        InitSleepSet();
        InvokeRepeating("RefreshSleepModifBtnState", 1f, 0.2f);

#if UNITY_EDITOR
        SystemSetTab.SetActive(true);
#endif
    }

    void InitPetInfo()
    {
        PetNameInp.value = PetInfo.PetInfoInstance.Name;

        PetSexSwitch[0].value = (PetInfo.PetInfoInstance.Sex == EPetSex.eMale);
        PetSexSwitch[1].value = (PetInfo.PetInfoInstance.Sex == EPetSex.eFemale);

        for (int i = 0; i < PetAgeYeahRLab.Length; i++)
        {
            PetAgeYeahRLab[i].Refresh(PetInfo.PetInfoInstance.AgeYeah);
            PetAgeYeahRLab[i].SetSelCallBackFun((txt) =>
            {
                //Debug.Log(StringBuilderTool.ToInfoString("宝宝龙年龄：", txt, "年"));
                CurShowAgeYeahStr = txt;
            });
        }
        for (int i = 0; i < PetAgeMonthRLab.Length; i++)
        {
            PetAgeMonthRLab[i].Refresh(PetInfo.PetInfoInstance.AgeMonth);
            PetAgeMonthRLab[i].SetSelCallBackFun((txt) =>
            {
                //Debug.Log(StringBuilderTool.ToInfoString("宝宝龙年龄：", txt, "月"));
                CurShowAgeMonthStr = txt;
            });
        }

        PetModifBtn.isEnabled = false;
    }
    void InitSleepSet()
    {
        for (int i = 0; i < SleepBeginTimeRLab.Length; i++)
        {
            SleepBeginTimeRLab[i].Refresh(SleepInfo.SleepInfoInstance.SleepBeginTime);
            SleepBeginTimeRLab[i].SetSelCallBackFun((txt) =>
            {
                //Debug.Log(StringBuilderTool.ToInfoString("睡眠开始时间：", txt));
                CurShowSleepBeginTimeStr = txt;
            });
        }
        for (int i = 0; i < SleepEndTimeRLab.Length; i++)
        {
            SleepEndTimeRLab[i].Refresh(SleepInfo.SleepInfoInstance.SleepEndTime);
            SleepEndTimeRLab[i].SetSelCallBackFun((txt) =>
            {
                //Debug.Log(StringBuilderTool.ToInfoString("睡眠结束时间：", txt));
                CurShowSleepEndTimeStr = txt;
            });
        }

        EnableUseDurationSwitch[0].value = (SleepInfo.SleepInfoInstance.EnableUseDuration == EDuration.e15Minute);
        EnableUseDurationSwitch[1].value = (SleepInfo.SleepInfoInstance.EnableUseDuration == EDuration.e30Minute);
        EnableUseDurationSwitch[2].value = (SleepInfo.SleepInfoInstance.EnableUseDuration == EDuration.e45Minute);
        EnableUseDurationSwitch[3].value = (SleepInfo.SleepInfoInstance.EnableUseDuration == EDuration.e60Minute);

        RestUseDurationSwitch[0].value = (SleepInfo.SleepInfoInstance.RestUseDuration == EDuration.e15Minute);
        RestUseDurationSwitch[1].value = (SleepInfo.SleepInfoInstance.RestUseDuration == EDuration.e30Minute);
        RestUseDurationSwitch[2].value = (SleepInfo.SleepInfoInstance.RestUseDuration == EDuration.e45Minute);
        RestUseDurationSwitch[3].value = (SleepInfo.SleepInfoInstance.RestUseDuration == EDuration.e60Minute);

        SleepModifBtn.isEnabled = false;

    }

    /// <summary> 切换页签 </summary>
    public void OnTypeToggleChange()
    {
        if (UIToggle.current.value)
        {
            switch (UIToggle.current.name)
            {
                case "Type_1":
                    InitPetInfo();
                    break;
                case "Type_2":
                    InitSleepSet();
                    break;
            }
        }
    }

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
                       
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
     
        }
    }*/

    public void Show(bool isShow, int showTabIndex = 1)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject, 0, () =>
                {
                    if (showTabIndex == 1)
                    {
                        PetInfoTab.value = true;
                    }
                    if (showTabIndex == 2)
                    {
                        SleepSetTab.value = true;
                    }
                });
        }
        else
        {
            
            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);

                CheckSleepTime();
                CheckUseDuration();
            });
        }
    }

    public void OnClick_Back()
    {
        Debug.Log("点击【退出】");

        Show(false);

        GameApp.Instance.SoundInstance.PlaySe("button");
    }

    #region _修改玩家名称
    public void OnPlayerNameInpChange()
    {
        if (GameApp.Instance.FilterSWInstance.FilterBl(PlayerNameInp.value))
        {
            PlayerNameInp.value = GameApp.Instance.FilterSWInstance.Filter(PlayerNameInp.value);
        }
    }

    /// <summary> 申请修改玩家名 </summary>
    public void ModifPlayerName()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (PlayerNameInp.value.Length == 0)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("玩家昵称不能为空！");
            return;
        }
        if (PlayerNameInp.value.Contains("*"))
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("玩家昵称中含有非法字符！");
            return;
        }

        if (SerPlayerData.GetItemCount(1000) < 200)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("钻石不足！");
            return;
        }

        GameApp.SendMsg.CheckPlayerName(PlayerNameInp.value);
    }
    /// <summary> 检测玩家名结果 </summary>
    public void CheckNameRes(LogicRes res)
    {
        Debug.Log("检测玩家名 结果[" + res + "]");
        if (res == LogicRes.CheckName_Success)
        {
            GameApp.SendMsg.SetPlayerName(PlayerNameInp.value);
        }
        else if (res == LogicRes.CheckName_Error)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("新角色名不可用！");
        }
    }
    /// <summary> 修改玩家名结果 </summary>
    public void ModifNameRes(LogicRes res)
    {
        Debug.Log("设置玩家名 结果[" + res + "]");
        if (res == LogicRes.CheckName_Success)
        {
            PlayerName.text = PlayerNameInp.value;
            GameApp.Instance.PlayerData.m_player_name = PlayerNameInp.value;
            GameApp.Instance.MainPlayerData.Name = PlayerNameInp.value;

            GameApp.SendMsg.GMOrder("AddItem 1000 -200");

            GameApp.Instance.CommonHintDlg.OpenHintBox("角色名已成功修改为“" + PlayerNameInp.value + "”！");
        }
        else
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("更改角色名失败！");
        }
    }
    #endregion

    #region _修改宝宝龙信息

    public void OnPetNameInpChange()
    {
        if (GameApp.Instance.FilterSWInstance.FilterBl(PetNameInp.value))
        {
            PetNameInp.value = GameApp.Instance.FilterSWInstance.Filter(PetNameInp.value);
        }
    }
    /// <summary> 检测宝宝龙名称结果 </summary>
    /*public void CheckPetNameRes(LogicRes res)
    {
        Debug.Log("检测宝宝龙名称 结果[" + res + "]");
        if (res == LogicRes.CheckName_Success)
        {
            GameApp.SendMsg.SetPetName(PetNameInp.value);
        }
        else if (res == LogicRes.CheckName_Error)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("新的宝宝龙名称不可用！");
        }
    }*/
    /// <summary> 修改宝宝龙名称结果 </summary>
    /*public void ModifPetNameRes(LogicRes res)
    {
        Debug.Log("设置宝宝龙名称 结果[" + res + "]");
        if (res == LogicRes.CheckName_Success)
        {
            GameApp.Instance.PlayerData.m_player_petname = PetNameInp.value;

            GameApp.Instance.CommonHintDlg.OpenHintBox("宝宝龙名称已成功修改为“" + PetNameInp.value + "”！");
        }
        else
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("更改宝宝龙名称失败！");
        }
    }*/
    /// <summary> 宝宝龙性别 </summary>
    public void OnPetSexToggleChange()
    {
        if (UIToggle.current.value)
        {
            switch (UIToggle.current.name)
            {
                case "Male":
                    Debug.Log("宝宝龙性别 小王子");

                    break;
                case "Female":
                    Debug.Log("宝宝龙性别 小公主");

                    break;
            }
        }
    }
    /// <summary> 刷新修改 </summary>
    void RefreshPetModifBtnState()
    {
        bool HasChange = (PetInfo.PetInfoInstance.Name != PetNameInp.value) ||
            (PetInfo.PetInfoInstance.Sex == EPetSex.eMale && !PetSexSwitch[0].value) ||
            (PetInfo.PetInfoInstance.Sex == EPetSex.eFemale && !PetSexSwitch[1].value) ||
            (PetInfo.PetInfoInstance.Sex == EPetSex.eNull && (PetSexSwitch[0].value || PetSexSwitch[1].value)) ||
            (PetInfo.PetInfoInstance.AgeYeah != CurShowAgeYeahStr) ||
            (PetInfo.PetInfoInstance.AgeMonth != CurShowAgeMonthStr);
        PetModifBtn.isEnabled = HasChange;
    }
    /// <summary> 修改宝宝龙信息 </summary>
    public void ModifPetInfo()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (PetNameInp.value.Length == 0)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("宝宝龙名称不能为空！");
            return;
        }
        if (PetNameInp.value.Contains("*"))
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("宝宝龙名称中含有非法字符！");
            return;
        }

        PetInfo.PetInfoInstance.Name = PetNameInp.value;
        PetInfo.PetInfoInstance.Sex = (PetSexSwitch[0].value ? EPetSex.eMale : (PetSexSwitch[1].value ? EPetSex.eFemale : EPetSex.eNull));
        PetInfo.PetInfoInstance.AgeYeah = CurShowAgeYeahStr;
        PetInfo.PetInfoInstance.AgeMonth = CurShowAgeMonthStr;

        //GameApp.SendMsg.CheckPetName(PetNameInp.value);
    }
    #endregion

    #region _修改休息设置
    /// <summary> 刷新修改 </summary>
    void RefreshSleepModifBtnState()
    {
        bool HasChange = (SleepInfo.SleepInfoInstance.SleepBeginTime != CurShowSleepBeginTimeStr) ||
            (SleepInfo.SleepInfoInstance.SleepEndTime != CurShowSleepEndTimeStr) ||
            (SleepInfo.SleepInfoInstance.EnableUseDuration == EDuration.e15Minute && !EnableUseDurationSwitch[0].value) ||
            (SleepInfo.SleepInfoInstance.EnableUseDuration == EDuration.e30Minute && !EnableUseDurationSwitch[1].value) ||
            (SleepInfo.SleepInfoInstance.EnableUseDuration == EDuration.e45Minute && !EnableUseDurationSwitch[2].value) ||
            (SleepInfo.SleepInfoInstance.EnableUseDuration == EDuration.e60Minute && !EnableUseDurationSwitch[3].value) ||
            (SleepInfo.SleepInfoInstance.RestUseDuration == EDuration.e15Minute && !RestUseDurationSwitch[0].value) ||
            (SleepInfo.SleepInfoInstance.RestUseDuration == EDuration.e30Minute && !RestUseDurationSwitch[1].value) ||
            (SleepInfo.SleepInfoInstance.RestUseDuration == EDuration.e45Minute && !RestUseDurationSwitch[2].value) ||
            (SleepInfo.SleepInfoInstance.RestUseDuration == EDuration.e60Minute && !RestUseDurationSwitch[3].value);
        SleepModifBtn.isEnabled = HasChange;
    }
    /// <summary> 修改休息设置 </summary>
    public void ModifSleepSet()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        SleepInfo.SleepInfoInstance.SleepBeginTime = CurShowSleepBeginTimeStr;
        SleepInfo.SleepInfoInstance.SleepEndTime = CurShowSleepEndTimeStr;
        if (EnableUseDurationSwitch[0].value)
            SleepInfo.SleepInfoInstance.EnableUseDuration = EDuration.e15Minute;
        else if (EnableUseDurationSwitch[1].value)
            SleepInfo.SleepInfoInstance.EnableUseDuration = EDuration.e30Minute;
        else if (EnableUseDurationSwitch[2].value)
            SleepInfo.SleepInfoInstance.EnableUseDuration = EDuration.e45Minute;
        else if (EnableUseDurationSwitch[3].value)
            SleepInfo.SleepInfoInstance.EnableUseDuration = EDuration.e60Minute;
        if (RestUseDurationSwitch[0].value)
            SleepInfo.SleepInfoInstance.RestUseDuration = EDuration.e15Minute;
        else if (RestUseDurationSwitch[1].value)
            SleepInfo.SleepInfoInstance.RestUseDuration = EDuration.e30Minute;
        else if (RestUseDurationSwitch[2].value)
            SleepInfo.SleepInfoInstance.RestUseDuration = EDuration.e45Minute;
        else if (RestUseDurationSwitch[3].value)
            SleepInfo.SleepInfoInstance.RestUseDuration = EDuration.e60Minute;
    }
    #endregion

    #region _音乐音效开关
    /// <summary> 音乐开关 </summary>
    public void OnMusicToggleChange()
    {
        if (UIToggle.current.value)
        {
            switch (UIToggle.current.name)
            {
                case "On":
                    Debug.Log("音乐 开");
                    GameApp.Instance.SoundInstance.BgmMute = false;
                    break;
                case "Off":
                    Debug.Log("音乐 关");
                    GameApp.Instance.SoundInstance.BgmMute = true;
                    break;
            }
        }
    }
    /// <summary> 音效开关 </summary>
    public void OnSeToggleChange()
    {
        if (UIToggle.current.value)
        {
            switch (UIToggle.current.name)
            {
                case "On":
                    Debug.Log("音效 开");
                    GameApp.Instance.SoundInstance.SeMute = false;
                    break;
                case "Off":
                    Debug.Log("音效 关");
                    GameApp.Instance.SoundInstance.SeMute = true;
                    break;
            }
        }
    }
    #endregion

    #region _切换账号
    public void OnClick_SwitchAccount()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (GameApp.Instance.LeSDKInstance)
            GameApp.Instance.LeSDKInstance.SwitchAccount();
    }
    #endregion

    #region _重播剧情视频
    public VideoPlayer StoryVideoVP;
    public GameObject SkipStoryVideoBtn;
    public void OnClick_RePlayCG()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        StartCoroutine("_ShowStoryVideo");
    }
    IEnumerator _ShowStoryVideo()
    {
        if (PlayStoryVideo())
        {
            while (!StoryVideoVP.isPlaying)
                yield return new WaitForEndOfFrame();

            SkipStoryVideoBtn.SetActive(true);

            while (StoryVideoVP.isPlaying)
                yield return new WaitForEndOfFrame();

            StopVideo();
        }
    }
    /// <summary>播放战前视频</summary>
    string PrewarAudio = string.Empty;
    public bool PlayStoryVideo()
    {
        StoryVideoVP.audioOutputMode = VideoAudioOutputMode.AudioSource;
        StoryVideoVP.SetTargetAudioSource(0, StoryVideoVP.gameObject.GetComponent<AudioSource>());
        StoryVideoVP.playOnAwake = false;
        StoryVideoVP.IsAudioTrackEnabled(0);

        StoryVideoVP.clip = Resources.Load<VideoClip>("Video/Story");
        StoryVideoVP.Play();

        StoryVideoVP.gameObject.SetActive(true);

        return true;
    }

    /// <summary>跳过视频</summary>
    public void SkipVideo()
    {
        if (StoryVideoVP.gameObject.activeSelf)
        {
            if (StoryVideoVP.isPlaying)
            {
                StopVideo();
            }
        }
    }
    void StopVideo()
    {
        StoryVideoVP.Stop();
        SkipStoryVideoBtn.SetActive(false);
    }
    #endregion

    #region _GM指令
    public void RunGMOrder()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        GameApp.SendMsg.GMOrder(OrderInp.value);
    }

    public void GetAllCard()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        StartCoroutine("_GetAllCard");
    }
    IEnumerator _GetAllCard()
    {
        for (int i = 50001; i <= 50062; i++)
        {
            GameApp.SendMsg.GMOrder(StringBuilderTool.ToString("AddItem ", i, " 1"));

            ItemConfig ItemCfg = null;
            CsvConfigTables.Instance.ItemCsvDic.TryGetValue(i, out ItemCfg);
            if (ItemCfg != null)
            {
                GameApp.Instance.CommonHintDlg.SpringHint(StringBuilderTool.ToString("获得1张[", ItemCfg.Name,"]卡牌"));
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void OpenDrawing()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        GameApp.Instance.SceneCtlInstance.ChangeScene("DrawingAndColoring");
    }
    public void DeleteAllPlayerPrefs()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        PlayerPrefs.DeleteAll();
    }
    public void UnlockStage()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        GameApp.SendMsg.PVEFinish(999);
    }
    public void ShowFPS()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        Const.IsDevelopMode = !Const.IsDevelopMode;
    }
    #endregion
}
