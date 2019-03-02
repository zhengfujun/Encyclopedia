using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * @brief  场景控制类
*/
public class SceneControl : MonoBehaviour
{
    public static readonly string Login = "Login";//登录
    public static readonly string HomePage = "HomePage";//主界面
    public static readonly string Travel = "Travel";//旅行
    
    //public static readonly string Fight = "Fight";//Demo用战斗场景
    //public static readonly string Fight_BaiEJi_1 = "Fight_BaiEJi_1";//白垩纪战斗场景

    public string CurSceneName = "";

    private bool IsFirstIntoMainMenu = true;

    private AsyncOperation asyn = null;

    void Awake()
    {
        CurSceneName = SceneManager.GetActiveScene().name;
    }
    void Update()
    {
        if (asyn != null)
            GameApp.Instance.LoadingDlg.SetLoadingBar(asyn.progress);
    }

    public void ChangeScene(string nextScene)
    {
        if (nextScene == HomePage || nextScene == Login)
            GameApp.Instance.LoadingDlg.SetLoadingPicName("CommonLoading");
        else if (nextScene == Travel)
            GameApp.Instance.LoadingDlg.SetLoadingPicName("TravelLoading");
        else
        {
            if (GameApp.Instance.CurFightStageCfg != null)
                GameApp.Instance.LoadingDlg.SetLoadingPicName(GameApp.Instance.CurFightStageCfg.LoadingPic);
            else
                GameApp.Instance.LoadingDlg.SetLoadingPicName("CommonLoading");
        }

        if (!string.IsNullOrEmpty(nextScene))
        {
            asyn = null;

            GameApp.Instance.LoadingDlg.Show(
                new EventDelegate(() =>
                {                    
                    CurSceneName = nextScene;
                    //Debug.Log("执行_NextScene");
                    StartCoroutine("_NextScene");
                }));
        }
    }   
    
    IEnumerator _NextScene()
    {
        yield return new WaitForSeconds(0);
        
        Debug.Log("加载场景：" + CurSceneName);
        asyn = SceneManager.LoadSceneAsync(CurSceneName);
        yield return asyn;
                
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        
        yield return new WaitForSeconds(0.5f);

        GameApp.Instance.LoadingDlg.Hide();
    }   
}
