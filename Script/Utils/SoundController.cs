using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Holoville.HOTween;
using DG.Tweening;

public class SoundController : MonoBehaviour
{
    static int s_AmbientSoundTrack = 0;
    static int s_StopNPlayAmbientTrack = 1;
    public List<AudioSource> m_TrackBgm = new List<AudioSource>();
    public AudioSource m_TrackBgmNRest = null;
    public List<AudioSource> m_TrackSEList = new List<AudioSource>();
    public List<AudioSource> m_FixTrackSEList = new List<AudioSource>();
    public List<AudioSource> m_FootTrackSEList = new List<AudioSource>();
    private bool m_IsInit = false;
    public int m_ActiveBgmTrack = 0;
    private int m_CurrentVol = 100;
    private int m_CurrentBgmVol = 100;
    private int m_CurrentSeVol = 100;
    private string m_CurrentBgmName = "";
    private string m_CurrentBgmNRestName = "";

    public string m_CurrentSeNRestName = "";

    public List<string> m_CurrentBgmNRestNames = new List<string>();
    private float m_FadeInTime = 0;
    private bool m_StoppingFadeFlg = false;
    private bool m_BgmStopped = true;
    public bool m_CrossFade = false;
    public AudioSource m_LastActiveBgmTrack;
    private bool m_isMute = false;
    private bool m_BgmMute = false;
    private bool m_SeMute = false;
    private bool m_BgmRestPlaying = false;
    private bool m_BgmRestPausing = false;
    private bool m_BgmRestResting = false;
    public Tweener m_Twr = null;

    public bool BgmMute
    {
        get { return m_BgmMute; }
        set
        {
            if (m_BgmMute != value)
            {
                m_BgmMute = value;
                PlayerPrefs.SetInt("FFD_BGM_MUTE", value ? 1 : 0);
                SetBgmMute(value);
            }
        }
    }

    public bool SeMute
    {
        get { return m_SeMute; }
        set
        {
            if (m_SeMute != value)
            {
                m_SeMute = value;
                PlayerPrefs.SetInt("FFD_SE_MUTE", value ? 1 : 0);
                SetSeMute(value);
            }
        }
    }

    public enum E_SND_STAT
    {
        STAT_SE_FADE_BGM = 0,

        STAT_NULL
    }

    private E_SND_STAT m_Status = E_SND_STAT.STAT_NULL;
    private int m_FadeBgmSeTrk = 0;

    public bool IsInit()
    {
        return m_IsInit;
    }

    // Use this for initialization
    void Awake()
    {
        if (m_IsInit)
        {
            //  Utility.DebugLogWarning ("you can't init twice, pls check you code");
            return;
        }

        GameObject o1 = new GameObject("BgmNRestTrack");
        o1.transform.parent = transform;
        m_TrackBgmNRest = o1.AddComponent<AudioSource>();
        m_TrackBgmNRest.loop = false;
        m_TrackBgmNRest.playOnAwake = false;

        for (int i = 0; i < 10; i++)
        {
            AudioSource audiosrc = gameObject.AddComponent<AudioSource>();
            audiosrc.volume = (float)m_CurrentSeVol / 100.0f;
            audiosrc.loop = false;
            audiosrc.playOnAwake = false;
            m_TrackSEList.Add(audiosrc);
        }

        for (int i = 0; i < 3; ++i)
        {
            AudioSource audiosrc = gameObject.AddComponent<AudioSource>();
            audiosrc.volume = (float)m_CurrentSeVol / 100.0f;
            audiosrc.loop = false;
            audiosrc.playOnAwake = false;
            m_FixTrackSEList.Add(audiosrc);
        }

        for (int i = 0; i < 2; ++i)
        {
            AudioSource audiosrc = gameObject.AddComponent<AudioSource>();
            audiosrc.volume = (float)m_CurrentSeVol / 100.0f;
            audiosrc.loop = false;
            audiosrc.playOnAwake = false;
            m_FootTrackSEList.Add(audiosrc);
        }

        for (int i = 0; i < 2; ++i)
        {
            GameObject o = new GameObject(StringBuilderTool.ToString("BgmTrack", i));
            o.transform.parent = transform;
            AudioSource src = o.AddComponent(typeof(AudioSource)) as AudioSource;
            src.loop = false;
            src.playOnAwake = false;
            m_TrackBgm.Add(src);
        }

        if (PlayerPrefs.HasKey("FFD_BGM_VOL"))
        {
            m_CurrentBgmVol = PlayerPrefs.GetInt("FFD_BGM_VOL");
        }

        if (PlayerPrefs.HasKey("FFD_SE_VOL"))
        {
            m_CurrentSeVol = PlayerPrefs.GetInt("FFD_SE_VOL");
        }

        if (PlayerPrefs.HasKey("FFD_MUTE"))
        {
            m_isMute = (PlayerPrefs.GetInt("FFD_MUTE") == 1) ? true : false;
        }
        SetMute(m_isMute);

        if (PlayerPrefs.HasKey("FFD_BGM_MUTE"))
        {
            m_BgmMute = (PlayerPrefs.GetInt("FFD_BGM_MUTE") == 1);
        }
        SetBgmMute(m_BgmMute);

        if (PlayerPrefs.HasKey("FFD_SE_MUTE"))
        {
            m_SeMute = (PlayerPrefs.GetInt("FFD_SE_MUTE") == 1);
        }
        SetSeMute(m_SeMute);

        m_IsInit = true;

    }

    void SetMute(bool b)
    {
        m_isMute = b;
        int val = ((b == true) ? 1 : 0);
        PlayerPrefs.SetInt("FFD_MUTE", val);

        foreach (AudioSource src in m_TrackBgm)
        {
            src.mute = m_isMute;
        }
        foreach (AudioSource src in m_TrackSEList)
        {
            src.mute = m_isMute;
        }
        foreach (AudioSource src in m_FixTrackSEList)
        {
            src.mute = m_isMute;
        }

        m_TrackBgmNRest.mute = m_isMute;
    }

    void SetBgmMute(bool b)
    {
        foreach (AudioSource src in m_TrackBgm)
        {
            src.mute = b;
        }
        m_TrackBgmNRest.mute = b;
    }

    void SetSeMute(bool b)
    {
        foreach (AudioSource src in m_TrackSEList)
        {
            src.mute = b;
        }
        foreach (AudioSource src in m_FixTrackSEList)
        {
            src.mute = b;
        }
        foreach (AudioSource src in m_FootTrackSEList)
        {
            src.mute = b;
        }
    }

    void SetVol(int vol)
    {
        m_CurrentVol = vol;
        m_CurrentBgmVol = vol;
        m_CurrentSeVol = vol;

        PlayerPrefs.SetInt("FFD_BGM_VOL", m_CurrentBgmVol);
        PlayerPrefs.SetInt("FFD_SE_VOL", m_CurrentSeVol);

    }

    public void SetBgmVolNoRecord(int vol)
    {
        m_CurrentBgmVol = vol;
        if (m_TrackBgm[m_ActiveBgmTrack].isPlaying)
        {
            m_TrackBgm[m_ActiveBgmTrack].volume = vol / 100.0f;
        }

        if (m_TrackBgmNRest.isPlaying)
        {
            m_TrackBgmNRest.volume = vol / 100.0f;
        }
    }

    public void SetBgmVol(int vol)
    {
        m_CurrentBgmVol = vol;
    }

    void SetSeVol(int vol)
    {
        m_CurrentSeVol = vol;
    }

    float GetPlayTime(AudioSource sur)
    {
        return sur.time;
    }

    public float GetBgmPlayTime()
    {
        if (m_TrackBgm[m_ActiveBgmTrack].isPlaying)
        {
            return GetPlayTime(m_TrackBgm[m_ActiveBgmTrack]);
        }
        else if (m_TrackBgmNRest.isPlaying)
        {
            return GetPlayTime(m_TrackBgmNRest);
        }
        else
        {
            return GetPlayTime(m_TrackBgmNRest);
        }
    }

    public float GetSePlayeTime(string seName)
    {
        float time = 0;
        foreach (AudioSource src in m_TrackSEList)
        {
            if (src.clip.name.Equals(seName))
            {
                time = src.time;
                break;
            }
        }
        return time;
    }

    public bool IsBgmPlaying()
    {
        return (m_TrackBgm[m_ActiveBgmTrack].isPlaying || m_TrackBgmNRest.isPlaying);
    }

    public bool IsBgmLooping()
    {
        return (m_TrackBgm[m_ActiveBgmTrack].loop || m_TrackBgmNRest.loop);
    }

    public bool IsSeLooping(string seName)
    {
        bool loop = false;

        foreach (AudioSource src in m_TrackSEList)
        {
            if (src.clip.name.Equals(seName))
            {
                loop = src.loop;
                break;
            }
        }
        return loop;
    }

    public int GetVol()
    {
        return m_CurrentVol;
    }

    public int GetBgmVol()
    {
        return m_CurrentBgmVol;
    }

    public int GetSeVol()
    {
        return m_CurrentSeVol;
    }

    public string GetCurrentBgmName()
    {
        return m_CurrentBgmName;
    }

    public void PlayBgm(string bgmName, float fadeInTime = 0.2f, float fadeOutTime = 0.5f, bool loopBgm = true)
    {
        //m_TrackBgm[0]
        if (bgmName.Equals(m_CurrentBgmName))
        {

            if (!m_StoppingFadeFlg && m_TrackBgm[m_ActiveBgmTrack].isPlaying)
            {
                return;
            }
            else
            {
                PlayBgmClip(fadeInTime, loopBgm);
                return;
            }
        }

        if (m_TrackBgm[m_ActiveBgmTrack].clip == null)
        {
            PlayBgmClip(bgmName, fadeInTime, loopBgm);
        }
        else
        {
            if (m_TrackBgm[m_ActiveBgmTrack].isPlaying)
            {
                m_FadeInTime = fadeInTime;
                m_CurrentBgmName = bgmName;
                TweenVolume tv = TweenVolume.Begin(m_TrackBgm[m_ActiveBgmTrack].gameObject, fadeOutTime, 0);
                tv.eventReceiver = gameObject;
                tv.callWhenFinished = "LastBgmFadeEnd";
                m_LastActiveBgmTrack = m_TrackBgm[m_ActiveBgmTrack];

                if (m_CrossFade)
                {
                    m_ActiveBgmTrack = (m_ActiveBgmTrack + 1) % 2;
                    PlayBgmClip(m_CurrentBgmName, fadeInTime, loopBgm);
                }
            }
            else
            {
                PlayBgmClip(bgmName, fadeInTime, loopBgm);
            }
        }
    }

    private void PlayBgmClip(float fadeInTime, bool loopBgm = true)
    {
        m_BgmStopped = false;
        m_ActiveBgmTrack = (m_ActiveBgmTrack + 1) % 2;
        m_TrackBgm[m_ActiveBgmTrack].enabled = true;

        if (m_TrackBgm[m_ActiveBgmTrack].clip == null || !m_TrackBgm[m_ActiveBgmTrack].clip.name.Equals(m_CurrentBgmName))
        {
            AudioClip clip = Resources.Load(StringBuilderTool.ToInfoString("Sound/Bgm/", m_CurrentBgmName)) as AudioClip;
            m_TrackBgm[m_ActiveBgmTrack].clip = clip;
        }
        m_TrackBgm[m_ActiveBgmTrack].volume = 0.011f;
        m_TrackBgm[m_ActiveBgmTrack].loop = loopBgm;
        m_TrackBgm[m_ActiveBgmTrack].Play();
        TweenVolume tv = TweenVolume.Begin(m_TrackBgm[m_ActiveBgmTrack].gameObject, fadeInTime, (float)m_CurrentBgmVol / 100.0f);
        tv.eventReceiver = gameObject;
        tv.callWhenFinished = "PlayBgmFadeEnd";
    }

    private void PlayBgmClip(string bgmName, float fadeInTime, bool loopBgm = true, bool willRest = false)
    {
        AudioSource currentAs;
        if (willRest)
        {
            currentAs = m_TrackBgmNRest;
            m_CurrentBgmNRestName = bgmName;
        }
        else
        {
            m_ActiveBgmTrack = (m_ActiveBgmTrack + 1) % 2;
            currentAs = m_TrackBgm[m_ActiveBgmTrack];
            m_CurrentBgmName = bgmName;
        }

        AudioClip clip = Resources.Load(StringBuilderTool.ToInfoString("Sound/Bgm/", bgmName)) as AudioClip;
        currentAs.enabled = true;
        currentAs.clip = clip;
        currentAs.volume = 0.011f;
        currentAs.loop = loopBgm;
        currentAs.Play();
        TweenVolume tv = TweenVolume.Begin(currentAs.gameObject, fadeInTime, (float)m_CurrentBgmVol / 100.0f);
        tv.eventReceiver = gameObject;
        tv.callWhenFinished = "PlayBgmFadeEnd";
    }

    void PlayBgmFadeEnd(UITweener uit)
    {
        //       Utility.DebugLog("play bgm fade end");
        GameObject.Destroy(uit);
    }

    void LastBgmFadeEnd(UITweener uit)
    {
        GameObject.Destroy(uit);
        m_LastActiveBgmTrack.enabled = true;
        m_LastActiveBgmTrack.Stop();
        m_LastActiveBgmTrack.volume = (float)m_CurrentVol / 100.0f;
        if (!m_CrossFade)
        {
            PlayBgmClip(m_CurrentBgmName, m_FadeInTime);
        }
    }

    public void PlaySe(string seName, bool loop = false)
    {
        if (seName != "")
        {
            AudioClip clip = Resources.Load(StringBuilderTool.ToInfoString("Sound/Se/", seName)) as AudioClip;
            if (clip != null)
            {
                int track = ChooseFreeSeTrack();
                m_TrackSEList[track].clip = clip;
                m_TrackSEList[track].loop = loop;
                m_TrackSEList[track].Play();
            }
            else
            {
                //Debugger.LogError("Cant Load Sound:" + seName);
            }
        }
    }

    //public void PlaySeFullPath(string seName, bool loop = false)
    //{
    //    if (seName != "")
    //    {
    //        AudioClip clip = Resources.Load(seName) as AudioClip;
    //        if (clip != null)
    //        {
    //            int track = ChooseFreeSeTrack();
    //            m_TrackSEList[track].clip = clip;
    //            m_TrackSEList[track].loop = loop;
    //            m_TrackSEList[track].Play();
    //        }
    //        else
    //        {
    //            Debuger.LogError("Cant Load Sound"+seName);
    //        }
    //    }
    //}

    public void PlayFootStepSe(string seName, bool loop = false)
    {
        if (seName != "")
        {
            AudioClip clip = Resources.Load(StringBuilderTool.ToInfoString("Sound/Se/", seName)) as AudioClip;
            int track = ChooseFreeFootSeTrack();
            if (track != -1)
            {
                m_FootTrackSEList[track].clip = clip;
                m_FootTrackSEList[track].loop = loop;
                m_FootTrackSEList[track].Play();
            }
        }
    }

    public void StopStepSnd()
    {
        for (int i = 0; i < m_FootTrackSEList.Count; ++i)
        {
            if (m_FootTrackSEList[i].isPlaying)
            {
                m_FootTrackSEList[i].Stop();
                return;
            }
        }
    }


    public void PlayAmbient(string ambName, bool loop = true)
    {
        if (!string.IsNullOrEmpty(ambName))
        {
            AudioClip clip = Resources.Load(StringBuilderTool.ToInfoString("Sound/Ambient/", ambName)) as AudioClip;
            if (m_FixTrackSEList[s_AmbientSoundTrack].isPlaying)
            {
                m_FixTrackSEList[s_AmbientSoundTrack].Stop();
            }
            m_FixTrackSEList[s_AmbientSoundTrack].clip = clip;
            m_FixTrackSEList[s_AmbientSoundTrack].loop = loop;
            m_FixTrackSEList[s_AmbientSoundTrack].Play();
        }
    }

    void PlayNRestAmbient(string ambName)
    {
        AudioClip clip = Resources.Load(StringBuilderTool.ToInfoString("Sound/Ambient/", ambName)) as AudioClip;
        if (m_FixTrackSEList[s_StopNPlayAmbientTrack].isPlaying)
        {
            m_FixTrackSEList[s_StopNPlayAmbientTrack].Stop();
        }
        m_FixTrackSEList[s_StopNPlayAmbientTrack].clip = clip;
        m_FixTrackSEList[s_StopNPlayAmbientTrack].loop = false;
        m_FixTrackSEList[s_StopNPlayAmbientTrack].Play();
    }

    public void StopAmbient()
    {
        if (m_FixTrackSEList[s_AmbientSoundTrack].isPlaying)
        {
            m_FixTrackSEList[s_AmbientSoundTrack].Stop();
        }
    }

    public void PlayVoice(string seName)
    {
        AudioClip clip = Resources.Load("Sound/Voice/" + seName) as AudioClip;
        int track = ChooseFreeSeTrack();
        m_TrackSEList[track].clip = clip;
        m_TrackSEList[track].loop = false;
        m_TrackSEList[track].Play();
    }

    public void PlaySeAndFadeBgm(string seName)
    {
        AudioClip clip = Resources.Load(StringBuilderTool.ToInfoString("Sound/Se/", seName)) as AudioClip;

        int track = ChooseFreeSeTrack();
        m_TrackSEList[track].clip = clip;
        m_TrackSEList[track].loop = false;
        m_TrackSEList[track].Play();

        m_FadeBgmSeTrk = track;

        if (clip.length < 0.5f)
        {
            //no fade bgm, cause se is too short
            if (IsBgmPlaying())
            {
                m_TrackBgm[m_ActiveBgmTrack].volume = (float)(m_CurrentBgmVol / 300.0f);
            }
        }
        else
        {
            if (IsBgmPlaying())
            {
                TweenVolume.Begin(m_TrackBgm[m_ActiveBgmTrack].gameObject, 0.2f, (float)m_CurrentBgmVol / 300.0f);
            }
        }
        m_Status = E_SND_STAT.STAT_SE_FADE_BGM;
    }

    public void PlaySe(int ch, string seName, bool loop = false)
    {
        if (ch >= 12 || ch < 0)
        {
            //     Utility.DebugLogError ("CHANNEL OVERFLOW");
        }

        AudioClip clip = Resources.Load(StringBuilderTool.ToInfoString("Sound/Se/", seName)) as AudioClip;
        if (m_FixTrackSEList[ch].isPlaying)
        {
            m_FixTrackSEList[ch].Stop();
        }
        m_FixTrackSEList[ch].clip = clip;
        m_FixTrackSEList[ch].loop = loop;
        m_FixTrackSEList[ch].Play();
    }

    public void StopBgm(float fadeOutTime = 0.5f)
    {
        if (m_TrackBgm[m_ActiveBgmTrack].isPlaying)
        {
            m_BgmStopped = true;
            m_StoppingFadeFlg = true;
            TweenVolume tv = TweenVolume.Begin(m_TrackBgm[m_ActiveBgmTrack].gameObject, fadeOutTime, 0);
            tv.eventReceiver = gameObject;
            tv.callWhenFinished = "StopBgmFadeEnd";
            m_LastActiveBgmTrack = m_TrackBgm[m_ActiveBgmTrack];
        }
    }

    public void StopBgmImmediately()
    {
        if (m_TrackBgm[m_ActiveBgmTrack].isPlaying)
        {
            m_TrackBgm[m_ActiveBgmTrack].Stop();
        }
    }

    public bool IsSePlaying(string seName)
    {
        bool isplay = false;
        foreach (AudioSource src in m_TrackSEList)
        {
            if (src.clip == null) continue;
            if (src.clip.name.Equals(seName))
            {
                if (src.isPlaying)
                    isplay = true;
            }
        }
        return isplay;
    }

    public void StopSe(string seName)
    {
        foreach (AudioSource src in m_TrackSEList)
        {
            if (src.clip == null) return;
            if (src.clip.name.Equals(seName))
            {
                if (src.isPlaying)
                {
                    src.Stop();
                }
            }
        }

        foreach (AudioSource src in m_FixTrackSEList)
        {
            if (src.clip == null) return;
            if (src.clip.name.Equals(seName))
            {
                if (src.isPlaying)
                {
                    src.Stop();
                }
            }
        }
    }

    public void StopAllSe()
    {
        foreach (AudioSource src in m_TrackSEList)
        {
            if (src.isPlaying)
            {
                src.Stop();
            }
        }
        foreach (AudioSource src in m_FixTrackSEList)
        {
            if (src.isPlaying)
            {
                src.Stop();
            }
        }

        StopSeNRest();

    }

    public void StopAllSnd()
    {
        StopBgm();
        StopAllSe();
    }

    public void StopAllSndImmediately()
    {
        StopBgmNRest();
        StopBgmImmediately();
        StopAllSe();
    }

    void StopBgmFadeEnd(UITweener uit)
    {
        //Utility.DebugLog("Call back is here !! "+m_LastActiveBgmTrack.clip.name);
        //Utility.DebugLog("Active channel is " + m_ActiveBgmTrack );
        m_StoppingFadeFlg = false;
        m_LastActiveBgmTrack.enabled = true;
        m_LastActiveBgmTrack.volume = (float)m_CurrentBgmVol / 100.0f;
        m_LastActiveBgmTrack.Stop();
        GameObject.Destroy(uit);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_Status)
        {
            case E_SND_STAT.STAT_NULL:
                break;
            case E_SND_STAT.STAT_SE_FADE_BGM:
                if (!m_TrackSEList[m_FadeBgmSeTrk].isPlaying && !m_BgmStopped)
                {
                    m_Status = E_SND_STAT.STAT_NULL;
                    TweenVolume.Begin(m_TrackBgm[m_ActiveBgmTrack].gameObject, 0.2f, (float)m_CurrentBgmVol / 100.0f);
                    m_FadeBgmSeTrk = 0;
                }
                break;
        }
    }

    private int ChooseFreeFootSeTrack()
    {
        for (int i = 0; i < m_FootTrackSEList.Count; i++)
        {
            if (!m_FootTrackSEList[i].isPlaying)
            {
                return i;
            }
        }

        return -1;
    }

    private int ChooseFreeSeTrack()
    {
        for (int i = 0; i < m_TrackSEList.Count; i++)
        {
            if (!m_TrackSEList[i].isPlaying)
            {
                return i;
            }
        }
        int val = Random.Range(0, 3);
        //Utility.DebugLog("RANDOM Track is " + val);
        return val;
    }

    public int GetFreetSeTrack()
    {
        int cnt = 0;
        foreach (AudioSource src in m_TrackSEList)
        {
            if (!src.isPlaying)
            {
                cnt++;
            }
        }
        return cnt;
    }

    public float GetRealBGMVol()
    {
        if (m_TrackBgm[m_ActiveBgmTrack].isPlaying)
            return m_TrackBgm[m_ActiveBgmTrack].volume;
        else if (m_TrackBgmNRest.isPlaying)
            return m_TrackBgmNRest.volume;
        else
            return 0;
    }

    public void PlayRandomBgmNRest(List<string> names)
    {
        m_CurrentBgmNRestNames = names;
        if (!m_BgmRestPlaying)
        {
            m_BgmRestPausing = false;
            m_TrackBgmNRest.Stop();
            m_BgmRestPlaying = true;
            int idx = Random.Range(0, m_CurrentBgmNRestNames.Count);
            m_CurrentBgmNRestName = m_CurrentBgmNRestNames[idx];
            StartCoroutine("CheckRandomBgmStatus");
        }
        else if (m_BgmRestPlaying && m_BgmRestPausing)
        {
            if (!m_BgmRestResting)
            {
                m_TrackBgmNRest.Play();
            }
            m_BgmRestPausing = false;
        }

        m_TrackBgmNRest.volume = 0;
        if (m_Twr != null)
        {
            m_Twr.Kill();
        }
        //m_Twr = HOTween.To (m_TrackBgmNRest, 0.2f, new TweenParms ()
        //                    .Prop ("volume", m_CurrentBgmVol / 100.0f)
        //                    .Ease (EaseType.Linear)
        //                    );

        m_Twr = m_TrackBgmNRest.DOFade(m_CurrentBgmVol / 100.0f, 0.2f);
    }

    //play sound then wait for some secs
    public void PlayBgmNRest(string bgmName)
    {
        if (!m_BgmRestPlaying)
        {
            m_BgmRestPausing = false;
            m_TrackBgmNRest.Stop();
            m_BgmRestPlaying = true;
            m_CurrentBgmNRestName = bgmName;
            StartCoroutine("CheckBgmStatus");
        }
        else if (m_BgmRestPlaying && m_BgmRestPausing)
        {
            if (!m_BgmRestResting)
            {
                m_TrackBgmNRest.Play();
            }
            m_BgmRestPausing = false;
        }
        m_TrackBgmNRest.volume = 0;
        if (m_Twr != null)
        {
            m_Twr.Kill();
        }
        //m_Twr = HOTween.To (m_TrackBgmNRest, 0.2f, new TweenParms ()
        //        .Prop ("volume", m_CurrentBgmVol / 100.0f)
        //        .Ease (EaseType.Linear)
        //        );
        m_Twr = m_TrackBgmNRest.DOFade(m_CurrentBgmVol / 100.0f, 0.2f);
    }

    public void PauseBgmNRest()
    {
        m_BgmRestPausing = true;
        if (m_TrackBgmNRest.isPlaying)
        {
            if (m_Twr != null)
            {
                m_Twr.Kill();
            }
            m_Twr = m_TrackBgmNRest.DOFade(0, 0.2f)
                //m_Twr = HOTween.To (m_TrackBgmNRest, 0.2f, new TweenParms ()
                //   .Prop ("volume", 0)
                //   .Ease (EaseType.Linear)
                .OnComplete(() =>
                {
                    m_TrackBgmNRest.Pause();
                    m_TrackBgmNRest.volume = m_CurrentBgmVol / 100.0f;
                }
            );
        }
    }

    public void StopBgmNRest()
    {
        if (m_Twr != null)
        {
            m_Twr.Kill();
        }
        m_Twr = m_TrackBgmNRest.DOFade(0, 0.2f)
            //m_Twr = HOTween.To (m_TrackBgmNRest, 0.2f, new TweenParms ()
            //    .Prop ("volume", 0)
            //    .Ease (EaseType.Linear)
            .OnComplete(() =>
            {
                m_TrackBgmNRest.Stop();
                m_TrackBgmNRest.volume = m_CurrentBgmVol / 100.0f;
            }
        );

        StopCoroutine("CheckBgmStatus");
        StopCoroutine("CheckRandomBgmStatus");
        m_BgmRestPlaying = false;
        m_BgmRestPausing = false;
        m_CurrentBgmNRestName = "";
    }

    //play sound then wait for some secs
    public void PlaySeNRest(string seName)
    {
        m_CurrentSeNRestName = seName;
        StopSeNRest();
        StartCoroutine("CheckSeStatus");
        m_FixTrackSEList[s_StopNPlayAmbientTrack].volume = (float)m_CurrentSeVol / 100.0f;
    }

    public void StopSeNRest()
    {
        StopCoroutine("CheckSeStatus");
        m_FixTrackSEList[s_StopNPlayAmbientTrack].Stop();
    }

    IEnumerator CheckSeStatus()
    {
        PlayNRestAmbient(m_CurrentSeNRestName);
        while (true)
        {
            if (m_FixTrackSEList[s_StopNPlayAmbientTrack].isPlaying)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                float randTime = Random.Range(5.0f, 15.0f);
                yield return new WaitForSeconds(randTime);
                m_FixTrackSEList[s_StopNPlayAmbientTrack].Play();
            }
        }
    }

    IEnumerator CheckBgmStatus()
    {
        PlayBgmClip(m_CurrentBgmNRestName, 0.2f, false, true);
        while (m_BgmRestPlaying)
        {
            if (m_BgmRestPausing)
            {
                //Debuger.Log("Pausing !!");
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                if (m_TrackBgmNRest.isPlaying)
                {
                    m_BgmRestResting = false;
                    //Debuger.Log("Playing !!");
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    float randTime = Random.Range(20.0f, 30.0f);
                    m_BgmRestResting = true;
                    //Debuger.Log("Resting !!");
                    yield return new WaitForSeconds(randTime);
                    if (!m_BgmRestPausing)
                    {
                        //Debuger.Log("Playing Again !!");
                        m_TrackBgmNRest.Play();
                        m_BgmRestResting = false;
                    }
                }
            }
        }
    }

    IEnumerator CheckRandomBgmStatus()
    {
        PlayBgmClip(m_CurrentBgmNRestName, 0.2f, false, true);
        while (m_BgmRestPlaying)
        {
            if (m_BgmRestPausing)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                if (m_TrackBgmNRest.isPlaying)
                {
                    m_BgmRestResting = false;
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    float randTime = Random.Range(20.0f, 30.0f);
                    m_BgmRestResting = true;
                    yield return new WaitForSeconds(randTime);
                    if (!m_BgmRestPausing)
                    {

                        int idx = Random.Range(0, m_CurrentBgmNRestNames.Count);
                        m_CurrentBgmNRestName = m_CurrentBgmNRestNames[idx];
                        PlayBgmClip(m_CurrentBgmNRestName, 0.2f, false, true);

                        m_BgmRestResting = false;
                    }
                }
            }
        }
    }

    /*public void PlayBgmContinous(string startingbgm, string loopbgm)
    {

        if (!m_BgmRestPlaying)
        {
            m_BgmRestPausing = false;
            m_TrackBgmNRest.Stop();
            m_BgmRestPlaying = true;
            m_CurrentBgmNRestName = startingbgm;
            //check status;

            BtrCoroutine.Instance.Cs_StartCoroutine(Check1stBgmStatus(loopbgm));
        }

        m_TrackBgmNRest.volume = 0;
        if (m_Twr != null)
        {
            m_Twr.Kill();
        }
        //m_Twr = HOTween.To (m_TrackBgmNRest, 0.2f, new TweenParms ()
        //        .Prop ("volume", m_CurrentBgmVol / 100.0f)
        //        .Ease (EaseType.Linear)
        //        );
        m_Twr = m_TrackBgmNRest.DOFade(m_CurrentBgmVol / 100.0f, 0.2f);
    }*/

    IEnumerator Check1stBgmStatus(string nextbgm)
    {
        PlayBgmClip(m_CurrentBgmNRestName, 0.2f, false, true);

        while (m_TrackBgmNRest.isPlaying)
        {
            yield return new WaitForSeconds(0.5f);
        }

        m_CurrentBgmNRestName = nextbgm;
        PlayBgmClip(m_CurrentBgmNRestName, 0.2f, true, true);

        yield break;
    }

}
