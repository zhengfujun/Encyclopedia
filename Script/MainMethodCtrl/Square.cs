using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEventType//事件类型
{
    eNull,  //空事件
    eStart, //起点
    eQandA, //问答
    eMove,  //随机移动
    eShop,  //商店
    eToBoss,//进入内圈
    eBoss   //Boss战
};

public enum EGridDirType//格子方向类型
{
    eZ_Positive,  //Z轴正方向
    eZ_Negative,  //Z轴负方向
    eX_Positive,  //X轴正方向
    eX_Negative,  //X轴负方向
};

public enum EGridModelType//格子模型类型
{
    eGrid_Cretaceous_Box_GO,                //白垩纪_带Go标记的盒子
    eGrid_Cretaceous_Grass_Olivine,         //白垩纪_草风格方台_黄绿
    eGrid_Cretaceous_Grass_Green,           //白垩纪_草风格方台_绿
    eGrid_Cretaceous_Grass_LightGreen,      //白垩纪_草风格方台_淡绿
    eGrid_Cretaceous_Stone_DarkBrown,       //白垩纪_石头风格方台_深褐
    eGrid_Cretaceous_Stone_LightBrown,      //白垩纪_石头风格方台_浅褐
    eGrid_Cretaceous_Grass_Blueness,        //白垩纪_草风格方台_草青
    eGrid_Cretaceous_Grass_DarkBlueness,    //白垩纪_草风格方台_草绿
    eGrid_Cretaceous_Grass_LightBlue,       //白垩纪_草风格方台_淡蓝
    eGrid_Cretaceous_Box_Yellow,            //白垩纪_黄盒子

    eGrid_Triassic_Mushroom_Red,            //三叠纪_红蘑菇
    eGrid_Triassic_Mushroom_Yellow,         //三叠纪_黄蘑菇
    eGrid_Triassic_OctPlatform_LightBlue,   //三叠纪_八角台子_浅蓝
    eGrid_Triassic_Stone_Colours,           //三叠纪_石头_彩色
    eGrid_Triassic_Stone_White,             //三叠纪_石头_白色
    eGrid_Triassic_Flower_Yellow,           //三叠纪_花_黄色
};

public enum ESignModelType//标志模型类型
{
    eNull,
    eSign_Arrows,        //箭头
    eSign_Skull,         //骷髅头
    eSign_Shopping,      //购物车
    eSign_Exclamation    //叹号
};
public enum ESignDirType//标志方向类型
{
    eNull,      //不转
    eRotate90,  //转90度
    eRotate180, //转180度
    eRotate270, //转270度
    eCustom     //自定义
};

public class Square : MonoBehaviour
{
    public int Index = -1;
    private string EventTypeString
    {
        get
        {
            switch(EventType)
            {
                default:
                case EEventType.eNull:return "空";
                case EEventType.eStart: return "起点";
                case EEventType.eQandA: return "问答";
                case EEventType.eMove: return "随机移动";
                case EEventType.eShop: return "商店";
                case EEventType.eToBoss: return "进入内圈";
                case EEventType.eBoss: return "Boss战";
            }
        }
    }

    private string GridModelTypeString
    {
        get
        {
            switch (GridModelType)
            {
                default:
                case EGridModelType.eGrid_Cretaceous_Box_GO: return "Grid_Cretaceous_Box_GO";
                case EGridModelType.eGrid_Cretaceous_Grass_Olivine: return "Grid_Cretaceous_Grass_Olivine";
                case EGridModelType.eGrid_Cretaceous_Grass_Green: return "Grid_Cretaceous_Grass_Green";
                case EGridModelType.eGrid_Cretaceous_Grass_LightGreen: return "Grid_Cretaceous_Grass_LightGreen";
                case EGridModelType.eGrid_Cretaceous_Stone_DarkBrown: return "Grid_Cretaceous_Stone_DarkBrown";
                case EGridModelType.eGrid_Cretaceous_Stone_LightBrown: return "Grid_Cretaceous_Stone_LightBrown";
                case EGridModelType.eGrid_Cretaceous_Grass_Blueness: return "Grid_Cretaceous_Grass_Blueness";
                case EGridModelType.eGrid_Cretaceous_Grass_DarkBlueness: return "Grid_Cretaceous_Grass_DarkBlueness";
                case EGridModelType.eGrid_Cretaceous_Grass_LightBlue: return "Grid_Cretaceous_Grass_LightBlue";
                case EGridModelType.eGrid_Cretaceous_Box_Yellow: return "Grid_Cretaceous_Box_Yellow";
                case EGridModelType.eGrid_Triassic_Mushroom_Red: return "Grid_Triassic_Mushroom_Red";
                case EGridModelType.eGrid_Triassic_Mushroom_Yellow: return "Grid_Triassic_Mushroom_Yellow";
                case EGridModelType.eGrid_Triassic_OctPlatform_LightBlue: return "Grid_Triassic_OctPlatform_LightBlue";
                case EGridModelType.eGrid_Triassic_Stone_Colours: return "Grid_Triassic_Stone_Colours";
                case EGridModelType.eGrid_Triassic_Stone_White: return "Grid_Triassic_Stone_White";
                case EGridModelType.eGrid_Triassic_Flower_Yellow: return "Grid_Triassic_Flower_Yellow";
            }
        }
    }

    private string SignModelTypeString
    {
        get
        {
            switch (SignModelType)
            {
                default:
                case ESignModelType.eNull: return "";
                case ESignModelType.eSign_Arrows: return "Sign_Arrows";
                case ESignModelType.eSign_Skull: return "Sign_Skull";
                case ESignModelType.eSign_Shopping: return "Sign_Shopping";
                case ESignModelType.eSign_Exclamation: return "Sign_Exclamation";
            }
        }
    }

    public EGridDirType GridDirType = EGridDirType.eZ_Positive;
    public EGridModelType GridModelType = EGridModelType.eGrid_Cretaceous_Grass_Olivine;
    public ESignModelType SignModelType = ESignModelType.eNull;
    public ESignDirType SignDirType = ESignDirType.eNull;
    public int SignAngle = 0;

    public EEventType EventType = EEventType.eNull;

    public GameObject Model;
    private TweenPosition tp;

    private bool isSinking = false;

    public string LastModifyTime = "";

    void Start()
    {
        Index = int.Parse(MyTools.GetLastString(gameObject.name, '_'));

        Model = transform.Find("Model").gameObject;
        tp = Model.GetComponent<TweenPosition>();

        switch (EventType)
        {
            case EEventType.eStart:
            case EEventType.eToBoss:
                {
                    Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                    rb.useGravity = false;

                    BoxCollider bc = gameObject.AddComponent<BoxCollider>();
                    bc.isTrigger = true;
                    bc.size = Vector3.one * 2;
                }
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (GameApp.Instance.FightUI == null)
            return;

        if (!GameApp.Instance.FightUI.GameBegin)
            return;

        if (collider.name.IndexOf("Player_") == 0)
        {
#if UNITY_EDITOR
            Debug.Log(StringBuilderTool.ToInfoString("Trigger: ", gameObject.name, "与", collider.name, " 接触！ 类型：", EventType.ToString()));
#endif
            switch (EventType)
            {
                case EEventType.eStart:
                    {
                        GameApp.Instance.FightUI.ShowBeginPointHint();
                    }
                    break;
                case EEventType.eToBoss:
                    {
                        GameApp.Instance.FightUI.ShowGotoBossHint();
                    }
                    break;
            }
        }
    }  

    void Update()
    {
        if (isSinking)
        {
            Transform roleTran = GameApp.Instance.FightUI.ChessCtrl.GetRole().transform;
            roleTran.GetChild(0).position = new Vector3(roleTran.position.x, tp.transform.position.y + 0.45f, roleTran.position.z);
        }
    }

    public void Sink()
    {
        isSinking = true;
        tp.enabled = true;
        tp.PlayForward();
        StartCoroutine("Recover");
    }
    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.1f);
        tp.PlayReverse();
        yield return new WaitForSeconds(0.1f);
        isSinking = false;
    }

    public void SetHint(ref UILabel hintLab)
    {
        hintLab.text = "";

        string type = MyTools.GetFirstString(gameObject.name, new char[] { '_' });
        switch (type)
        {
            case "N":
                hintLab.text += "外圈\n";
                break;
            case "B":
                hintLab.text += "内圈\n";
                break;
        }

        int idx = int.Parse(MyTools.GetLastString(gameObject.name, '_'));
        hintLab.text += StringBuilderTool.ToString("序号：", idx, "\n事件：", EventTypeString);
    }

    public void Do(Action DoOverCallback)
    {
        switch(EventType)
        {
            case EEventType.eNull:
                {
                    DoOverCallback();
                }
                break;
            case EEventType.eStart:
                {
                    DoOverCallback();
                }
                break;
            case EEventType.eQandA:
                {
                    GameApp.Instance.FightUI.OpenQandA(ELibraryType.eNormal, DoOverCallback);
                }
                break;
            case EEventType.eMove:
                {
                    GameApp.Instance.FightUI.GetRandomMove();
                }
                break;
            case EEventType.eShop:
                {
                    GameApp.Instance.FightUI.OpenShop(DoOverCallback);
                }
                break;
            case EEventType.eToBoss:
                {
                    GameApp.Instance.FightUI.ShowUnableGotoBossHint(DoOverCallback);
                }
                break;
            case EEventType.eBoss:
                {
                    GameApp.Instance.FightUI.OpenQandA(ELibraryType.eBoss, DoOverCallback);
                }
                break;
        }
    }

    public void CreateGrid()
    {
        if (Model == null)
            Model = transform.Find("Model").gameObject;

        for (int i = 0; i < Model.transform.childCount; i++)
        {
            Transform child = Model.transform.GetChild(i);
            if(child.name.Contains("Grid_"))
            {
                DestroyImmediate(child.gameObject);
                break;
            }
        }

        GameObject obj = Resources.Load<GameObject>("Prefabs/Scene/Grid/" + GridModelTypeString);
        if (obj != null)
        {
            GameObject Grid = GameObject.Instantiate(obj);
            Grid.transform.parent = Model.transform;
            Grid.transform.localPosition = Vector3.zero;
            Grid.transform.localEulerAngles = Vector3.zero;
            Grid.transform.localScale = Vector3.one;
        }
    }

    public void CreateSign()
    {
        if (Model == null)
            Model = transform.Find("Model").gameObject;

        for (int i = 0; i < Model.transform.childCount; i++)
        {
            Transform child = Model.transform.GetChild(i);
            if (child.name.Contains("Sign_"))
            {
                DestroyImmediate(child.gameObject);
                break;
            }
        }

        GameObject obj = Resources.Load<GameObject>("Prefabs/Scene/Sign/" + SignModelTypeString);
        if (obj != null)
        {
            GameObject Sign = GameObject.Instantiate(obj);
            Sign.transform.parent = Model.transform;
            Sign.transform.localPosition = Vector3.zero;
            Sign.transform.localScale = Vector3.one;

            RotateSign();
        }
    }
    public void RotateSign()
    {
        Transform Sign = null;
        for (int i = 0; i < Model.transform.childCount; i++)
        {
            Transform child = Model.transform.GetChild(i);
            if (child.name.Contains("Sign_"))
            {
                Sign = child;
                break;
            }
        }
        if (Sign != null)
        {
            switch (SignDirType)
            {
                case ESignDirType.eNull:
                    Sign.transform.localEulerAngles = Vector3.zero;
                    break;
                case ESignDirType.eRotate90:
                    Sign.transform.localEulerAngles = new Vector3(0, 90, 0);
                    break;
                case ESignDirType.eRotate180:
                    Sign.transform.localEulerAngles = new Vector3(0, 180, 0);
                    break;
                case ESignDirType.eRotate270:
                    Sign.transform.localEulerAngles = new Vector3(0, -90, 0);
                    break;
                case ESignDirType.eCustom:
                    Sign.transform.localEulerAngles = new Vector3(0, SignAngle, 0);
                    break;

            }
        }
    }

    public void UpdateModifyTime()
    {
        LastModifyTime = StringBuilderTool.ToInfoString(
            "最后修改时间：", DateTime.Now.ToString(),
            "\n修改者名称：", SystemInfo.deviceName,
            "\n修改者IP地址：", MyTools.GetIPAddress());
    }
}
