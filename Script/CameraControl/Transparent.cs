using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 将阻挡英雄的障碍物设置为半透明
/// 为达到效果，障碍物的材质被更改为Transparent/Diffuse
/// </summary>
public class Transparent : MonoBehaviour
{
    public bool Need = false;


    private Transform __selfTrans;
    public Transform selfTrans {
        get {
            if (__selfTrans == null) {
                __selfTrans = transform;
            }
            return __selfTrans;
        }
    }

    private enum ETaskState
    {
        Null,
        toLucency,  //处于变透明中
        toRestore   //处于恢复正常中
    }
    private class HTTask
    {
        public GameObject DisposeObj;
        public ETaskState State
        {
            set
            {
                if(value == ETaskState.toLucency)
                {
                    Lucency();
                }
                else if (value == ETaskState.toRestore)
                {
                    Restore();
                }
            }
        }
        public Shader[] OriginalShader;

        Color obj_color = new Color(1, 1, 1, 1);
        Task LucencyTask = null;

        public HTTask(GameObject obj)
        {
            DisposeObj = obj;
            State = ETaskState.Null;

            Renderer r = obj.GetComponent<Renderer>();
            if (r == null)
            {
                //Debug.Log(obj.name + "结点上没有Renderer组件");
            }
            else
            {
                OriginalShader = new Shader[r.materials.Length];
                for (int i = 0; i < r.materials.Length; i++)
                {
                    OriginalShader[i] = r.materials[i].shader;
                }
            }
        }

        public void Lucency()
        {
            LucencyTask = new Task(_Lucency());
        }
        IEnumerator _Lucency()
        {
            Shader __shader = null;
            Renderer r = DisposeObj.GetComponent<Renderer>();
			if(null == r)
				yield break;
            if (r.gameObject.tag == "Tree")
            {
                __shader = Shader.Find("Transparent/VertexLitWithZForTree");
                //__shader = Shader.Find("Transparent/VertexLitWithZForTree");
            }
            else
            {
                __shader = Shader.Find("Transparent/VertexLitWithZ");
                //__shader = Shader.Find("Transparent/VertexLitWithZ");
                
            }

            for (int i = 0; i < r.materials.Length; i++)
            {
                r.materials[i].shader = __shader;
            }

            while (obj_color.a > 0.3f)
            {
                for (int i = 0; i < r.materials.Length; i++)
                {
					if(r.materials[i].HasProperty("_Color")){
						obj_color = r.materials[i].color;
						obj_color.a -= 0.05f;
						r.materials[i].SetColor("_Color", obj_color);
					}
                    
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public void Restore()
        {
            new Task(_Restore());
        }
        IEnumerator _Restore()
        {
            LucencyTask.Stop();

            if (DisposeObj == null)
                yield break;

            Renderer r = DisposeObj.GetComponent<Renderer>();
            while (obj_color.a < 1f)
            {
                for (int i = 0; i < r.materials.Length; i++)
                {
					if(r.materials[i].HasProperty("_Color")){
						obj_color = r.materials[i].color;
						obj_color.a += 0.05f;
						r.materials[i].SetColor("_Color", obj_color);
					}
                    
                }

                yield return new WaitForEndOfFrame();
            }

            if (r == null)
                yield break;

            if(r.materials != null)
            {
                for (int i = 0; i < r.materials.Length; i++)
                {
                    r.materials[i].shader = OriginalShader[i];
                }
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("HTTask::_Restore " + DisposeObj.name);
            }
#endif
        }
    }

    private List<int> NeedRemoveLst = new List<int>();
    private Dictionary<int, HTTask> RunningLst = new Dictionary<int, HTTask>();
    private Dictionary<int, HTTask> NewLst = new Dictionary<int, HTTask>();

    private Vector3 playerObjPos
    {
        get
        {
            if (player != null)
            {
                return player.position + new Vector3(0, 1.0f, 0);
            }
            return transform.parent.position;
        }
    }
    private Transform player
    {
        get
        {
            if (_player == null)
            {
                if (GameApp.Instance.FightUI == null)
                    return null;

                _player = GameApp.Instance.FightUI.ChessCtrl.CameraControl.CamTarget.transform;
            }
            return _player;
        }
    }
    private Transform _player = null;

    //void Start()
    //{

    //}
    Vector3 selfPos;
    RaycastHit[] hitInfo;
    int useLength;
    void Update()
    {
        //return;

        if (Need && Time.frameCount % 5 == 0)
        {
            //为了调式时看的清楚画的线
            Debug.DrawLine(playerObjPos, transform.position, Color.red);
            selfPos = selfTrans.position;
            hitInfo = Physics.RaycastAll(selfPos, playerObjPos - selfPos);
            useLength = hitInfo.Length;
            if (useLength > 0)
            {
                NewLst.Clear();
                GameObject __obj;
                int __layer;
                string __name;
                for (int j = 0; j < useLength; j++)
                {
                    __obj = hitInfo[j].transform.gameObject;
                    __layer = __obj.layer;
                    __name = __obj.name;
                    if (__name.Contains("Tree_") ||
                        __name.Contains("Sky_tree"))
                    {
                        //NewLst.Add(hitInfo[j].transform.gameObject.GetInstanceID(), new HTTask(hitInfo[j].transform.gameObject)); 
                        HTTask hTaskTmp = null;
                        //借用layer作为key
                        __layer = __obj.GetInstanceID();
                        if (!NewLst.TryGetValue(__layer, out hTaskTmp)) {
                            NewLst.Add(__layer,new HTTask(__obj));
                        }
                        //NewLst[__obj.GetInstanceID()] = new HTTask(__obj); 
                    }
                }

               
                if (NewLst.Count > 0) {

                    //新列表与旧列表比较
                    foreach (KeyValuePair<int, HTTask> pair1 in NewLst)
                    {
                        /*  bool isOldHave = false;
                        foreach (KeyValuePair<int, HTTask> pair2 in RunningLst)
                        {
                            if (pair1.Key == pair2.Key)
                            {
                                isOldHave = true;
                            }
                        }
                        if (!isOldHave)
                        {
                            RunningLst.Add(pair1.Key, pair1.Value);
                            pair1.Value.State = ETaskState.toLucency;
                        }*/

                        if (!RunningLst.ContainsKey(pair1.Key)) {
                            RunningLst.Add(pair1.Key, pair1.Value);
                            pair1.Value.State = ETaskState.toLucency;
                        }

                    }
                    //旧列表与新列表比较
                    NeedRemoveLst.Clear();
                    foreach (KeyValuePair<int, HTTask> pair1 in RunningLst)
                    {
                       /* bool isNewHave = false;
                        foreach (KeyValuePair<int, HTTask> pair2 in NewLst)
                        {
                            if (pair1.Key == pair2.Key)
                            {
                                isNewHave = true;
                            }
                        }
                        if (!isNewHave)
                        {
                            pair1.Value.State = ETaskState.toRestore;
                            NeedRemoveLst.Add(pair1.Key);
                        }*/

                        if (!NewLst.ContainsKey(pair1.Key)) {
                            pair1.Value.State = ETaskState.toRestore;
                            NeedRemoveLst.Add(pair1.Key);
                        }
                    }
                    for (int i = 0; i < NeedRemoveLst.Count; i++)
                    {
                        RunningLst.Remove(NeedRemoveLst[i]);
                    }
                }               
            }

            //设置本结点位置
            selfTrans.position = Vector3.MoveTowards(selfTrans.parent.position, playerObjPos, -5f);
        }
    }
}

/*
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 将阻挡英雄的障碍物设置为半透明
/// 为达到效果，障碍物的材质被更改为Transparent/Diffuse
/// </summary>
public class Transparent : MonoBehaviour
{
    public bool Need = false;

    //记录上次的对象
    private List<GameObject> listTranObj = new List<GameObject>();
    private Dictionary<string, string[]> listShaders = new Dictionary<string, string[]>();

    private Vector3 playerObjPos
    {
        get
        {
            if (player != null)
            {
                return player.position + new Vector3(0, 1.0f, 0);
            }
            return transform.parent.position;
        }

    }
    private Transform player
    {
        get
        {
            if (_player == null)
            {
                if ((GameApp.Instance.SceneMgrInstance.CurSceneName.Contains(SceneManage.Fight + "_") || GameApp.Instance.SceneMgrInstance.CurSceneName == SceneManage.PVPFight)
                    && FightConst.Instance != null && FightConst.Instance.mainHero != null)
                {
                    _player = FightConst.Instance.mainHero.transform;
                }
                else if (GameApp.Instance.SceneMgrInstance.CurSceneName == SceneManage.MainCity && MainCityControl.mainPlayer)
                {
                    _player = MainCityControl.mainPlayer.transform;
                }
                else if(GameApp.Instance.SceneMgrInstance.CurSceneName.Contains(SceneManage.HuntingScene + "_"))
                {
                    if(TeamControl.mainPlayer != null)
                    _player = TeamControl.mainPlayer.transform;
                }
            }
            return _player;
        }
    }
    private Transform _player = null;

    void Start()
    {

    }

    void SetHalfTransparent(GameObject obj, bool bIsTran, string[] recordShaders = null)
    {
        try
        {
            if (null == obj)
            {
                return;
            }
            Color obj_color;
            if (bIsTran)
            {
                string[] lastShaders = new string[10];
                for (int i = 0; i < lastShaders.Length; i++)
                {
                    lastShaders[i] = "";
                }
                for (int i = 0; i < obj.GetComponent<Renderer>().materials.Length; i++)
                {
                    lastShaders[i] = obj.GetComponent<Renderer>().materials[i].shader.name;
                    //Debug.Log("记录 " + i + " = " + lastShaders[i]);
                    obj.GetComponent<Renderer>().materials[i].shader = Shader.Find("Transparent/Diffuse");
                    obj_color = obj.GetComponent<Renderer>().materials[i].color;
                    obj_color.a -= 0.05f;
                    if (obj_color.a < 0.3f)
                    {
                        obj_color.a = 0.3f;
                    }
                    //Debug.Log("A " + obj_color.a);
                    obj.GetComponent<Renderer>().materials[i].SetColor("_Color", obj_color);
                }
                listTranObj.Add(obj);
                listShaders.Add(obj.name, lastShaders);
            }
            else
            {
                for (int i = 0; i < obj.GetComponent<Renderer>().materials.Length; i++)
                {
                    obj.GetComponent<Renderer>().materials[i].shader = Shader.Find(recordShaders[i]);
                    if (obj.GetComponent<Renderer>().materials[i].HasProperty("_Color"))
                    {
                        obj_color = obj.GetComponent<Renderer>().materials[i].color;
                        //obj_color.a += 0.05f;
                        //if (obj_color.a > 1.0f)
                        {
                            obj_color.a = 1.0f;
                        }
                        //Debug.Log("B " + obj_color.a);
                        obj.GetComponent<Renderer>().materials[i].SetColor("_Color", obj_color);
                    }
                }
            }
        }
        catch (System.Exception)
        {
            //Debug.Log("看到我说明不该半透明的被检测为阻挡物了，加判断筛选 = " + obj.name);
        }
    }

    void AddTranObj(GameObject obj)
    {
        SetHalfTransparent(obj, true);
    }

    void ClearTranObj()
    {
        for (int i = 0; i < listTranObj.Count; i++)
        {
            if (listTranObj[i])
            {
                SetHalfTransparent(listTranObj[i], false, listShaders[listTranObj[i].name]);
            }
        }
        listTranObj.Clear();
        listShaders.Clear();
    }

    RaycastHit[] LastHitInfo = null;
    void Update()
    {
        if (Need && Time.frameCount % 2 == 0)
        {
            //为了调式时看的清楚画的线
            //Debug.DrawLine(playerObjPos, transform.position, Color.red);

            RaycastHit[] hitInfo = Physics.RaycastAll(transform.position, playerObjPos - transform.position);
            if (hitInfo.Length > 2)
            {
                for (int j = 0; j < hitInfo.Length; j++)
                {
                    if (hitInfo[j].transform.gameObject.layer != LayerMask.NameToLayer("Ground"))
                    {
                        //Debug.Log(hitInfo[j].transform.gameObject.name);
                        AddTranObj(hitInfo[j].transform.gameObject);
                    }
                }
                LastHitInfo = hitInfo;
            }

            //Debug.Log("A " + hitInfo.Length + " " + (LastHitInfo == hitInfo));
            if (hitInfo.Length == 2 || LastHitInfo != hitInfo)
            {
                ClearTranObj();
            }
        }
    }
}

*/