using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

//通过此类实现逻辑类的单件实例化
public class SimpleSingleton<T> where T : new()
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}

//全局型功能初始化入口
//通过此类实现组件类的单件实例化
public class Singlton : MonoBehaviour
{
    private static int InitCnt = 0;
    private static GameObject go;

    void Awake()
    {
        if (InitCnt > 0)
        {
            DestroyImmediate(gameObject);
            return;
        }
        InitCnt = 1;

        go = gameObject;
        DontDestroyOnLoad(gameObject);
        Application.runInBackground = true;

#if UNITY_STANDALONE_WIN
        //PC版强制某分辨率运行
        Screen.SetResolution(1280, 720, false);
#endif
        //设置日志输出状态
        Debug.unityLogger.logEnabled = GameApp.Instance.IsDebugLog;

        //程序不休眠
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Application.targetFrameRate = 40;

        Singlton.GetInstance<Debugging>();

        InvokeRepeating("CheckTime", 0f, 60f); 
    }

    void CheckTime()
    {
        UI_NewSetting.CheckSleepTime();
        UI_NewSetting.CheckUseDuration();
    }

    /*void Start()
    {

    }*/

    /// <summary>
    /// 查找ngui事件
    /// </summary>
    /// <returns><c>true</c>, if process touch was shoulded, <c>false</c> otherwise.</returns>
    /// <param name="fingerInder">Finger inder.</param>
    /// <param name="position">Position.</param>
    bool ShouldProcessTouch(int fingerInder, Vector2 position)
    {
		bool isui = false;
		for (int i = 0; i < UICamera.list.size; i++) {
			UICamera camera = UICamera.list[i];
			if(camera){
				Ray ray = camera.cachedCamera.ScreenPointToRay(position);
				bool touchui = Physics.Raycast(ray, float.PositiveInfinity, 1<<LayerMask.NameToLayer("UI") | 1<<LayerMask.NameToLayer("CommunalUI"));
				if(touchui)
					isui = true;
			}
		}

		if (isui)
			return false;

        return true;
    }

    public static T GetInstance<T>() where T :UnityEngine.Component
    {
        //T _t = Instance.gameObject.GetComponent<T>();
        if (go != null)
        {

            T _t = go.GetComponent<T>();
            if (_t == null)
            {
                _t = go.AddComponent<T>();
            }
            return _t;
        }
        else
        {
            Debug.LogError("缺失Singlton全局单件根节点，请从Login场景进入或手动拖入Resources\\Prefabs\\UI\\Singlton");
            return null;
        }
    }

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GameApp.Instance.CommonMsgDlg.OpenMsgBox(Localization.Get("isQuit"),
            (isClickOK) =>
            {
                if (isClickOK)
                {
                    Application.Quit();
                }
            });
        }
    }*/

    /*void OnGUI()
    {

    }*/
}