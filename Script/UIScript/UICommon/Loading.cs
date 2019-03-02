using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 效果类型
/// </summary>
public enum LoadingType
{
    FadeOut        //渐隐
}

/// <summary>
/// 玩家加载界面类型
/// </summary>
public enum PlayerLoadingType
{
    Single,
    Multi
}

/// <summary>
/// 场景切换效果
/// </summary>
public class Loading : MonoBehaviour
{
    public static bool InLoading = false;
    public static bool BeginHide = false;

    public GameObject FadeOutRoot;
    public UIPanel FadeOutPanel;
    public GameObject FadeOutPlane;
    private UITexture FadeOutTexture;
    private string LoadingPicName = string.Empty;

    public GameObject[] LoadingPoint;

    public UISlider ProgressBar;

    //public UILabel[] scheduleLab;
    //public UILabel HintTxtLab;

    public float LoadingConsuming = 0.8f;

    private LoadingType loadingType = LoadingType.FadeOut;
    private EventDelegate curCallBack;

    public GameObject SinglePlayerLoading;
    public GameObject MultiPlayerLoading;
    public UI_LoadingPlayerState[] LPS;
    private PlayerLoadingType CurPLType = PlayerLoadingType.Single;
    
    void Start()
    {
        GameApp.Instance.LoadingDlg = this;

        FadeOutTexture = FadeOutPlane.GetComponent<UITexture>();
        //for (int i = 0; i < scheduleLab.Length; i++)
        //    scheduleLab[i].transform.parent.gameObject.SetActive(false);

        //HintTxtLab.transform.parent.gameObject.SetActive(false);
    }

    public void SetPlayerLoadingType(PlayerLoadingType PLType, UI_TeamUnit[] CurTeam = null)
    {
        CurPLType = PLType;
        switch (CurPLType)
        {
            case PlayerLoadingType.Single:
                {
                    SinglePlayerLoading.SetActive(true);
                    MultiPlayerLoading.SetActive(false);
                }
                break;
            case PlayerLoadingType.Multi:
                {
                    SinglePlayerLoading.SetActive(false);
                    MultiPlayerLoading.SetActive(true);

                    if (CurTeam != null)
                    {
                        int ValidPlayerCnt = 0;
                        for (int i = 0; i < CurTeam.Length; i++)
                        {
                            if (CurTeam[i].IsValid)
                            {
                                ValidPlayerCnt++;
                                LPS[i].Set(GameApp.Instance.CurRoomPlayerLst[i].id, CurTeam[i].Portrait, CurTeam[i].PortraitBg);
                            }
                        }
                        switch (ValidPlayerCnt)
                        {
                            case 2:
                                MultiPlayerLoading.transform.localPosition = new Vector3(0, 150, 0);
                                break;
                            case 3:
                                MultiPlayerLoading.transform.localPosition = new Vector3(0, 250, 0);
                                break;
                            case 4:
                                MultiPlayerLoading.transform.localPosition = new Vector3(0, 350, 0);
                                break;
                        }
                    }
                }
                break;
        }
    }

    public void SetPlayerLoadOver(ulong playerID)
    {
        for (int i = 0; i < LPS.Length; i++)
        {
            if (LPS[i].PlayerID == playerID)
            {
                LPS[i].LoadingOver();
            }
        }
    }

    public void SetLoadingPicName(string PicName)
    {
        LoadingPicName = PicName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="textruename"></param>加载图片的路径名字   
    public void Show(EventDelegate callback)
    {
        InLoading = true;
        BeginHide = false;
        //gameObject.SetActive(true);

        loadingType = LoadingType.FadeOut;

        curCallBack = callback;

        FadeOutTexture.mainTexture = Resources.Load(StringBuilderTool.ToInfoString("BackGround/", LoadingPicName)) as Texture;

        FadeOutRoot.SetActive(true);
        iTween.ValueTo(gameObject,
                        iTween.Hash(
                        "name", "FadeOut",
                        "from", 0f,
                        "to", 1f,
                        "easetype", iTween.EaseType.easeInOutCubic,
                        "loopType", iTween.LoopType.none,
                        "onupdate", "ChangeFadeOutPlaneAlpha",
                        "oncomplete", "ShowOver",
                        "time", LoadingConsuming * 1.5f));

        StartCoroutine("RunShowLoadingPoint");
        ProgressBar.value = 0;
    }
    public void Hide()
    {
        if (CurPLType == PlayerLoadingType.Multi)
        {
            GameApp.SendMsg.LoadState(1);
            return;
        }

        UnconditionalHide();
    }

    public void UnconditionalHide()
    {
        BeginHide = true;

        iTween.ValueTo(gameObject,
                            iTween.Hash(
                            "name", "FadeOut",
                            "from", 1f,
                            "to", 0f,
                            "easetype", iTween.EaseType.easeInOutCubic,
                            "loopType", iTween.LoopType.none,
                            "onupdate", "ChangeFadeOutPlaneAlpha",
                            "oncomplete", "HideOver",
                            "time", LoadingConsuming * 1.5f));

        //for (int i = 0; i < scheduleLab.Length; i++)
        //    scheduleLab[i].transform.parent.gameObject.SetActive(false);

        //HintTxtLab.transform.parent.gameObject.SetActive(false);
    }

    void ChangeFadeOutPlaneAlpha(float v)
    {
        FadeOutPanel.alpha = v;
    }

    void ShowOver()
    {
        if (curCallBack != null)
        {
            curCallBack.Execute();
            curCallBack = null;
        }
    }

    void HideOver()
    {
        FadeOutRoot.SetActive(false);

        InLoading = false;
        BeginHide = false;
        //gameObject.SetActive(false);
        Resources.UnloadAsset(FadeOutTexture.mainTexture);
    }

    IEnumerator RunShowLoadingPoint()
    {
        int CurShowCnt = 0;
        while (InLoading)
        {
            CurShowCnt++;
            if (CurShowCnt > 3)
                CurShowCnt = 0;

            for (int i = 0; i < LoadingPoint.Length; i++)
            {
                LoadingPoint[i].SetActive(i < CurShowCnt);
            }
            
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void SetLoadingBar(float increment)
    {
        ProgressBar.value += increment;
    }
}
