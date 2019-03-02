using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 角色界面--模型单元 </summary>
public class ModelUnit : MonoBehaviour
{
    /// <summary> 列表父节点上的控制模型单元居中显示的组件 </summary>
    private UICenterOnChild COC;

    /// <summary> 角色ID，对应RoleConfig中RoleID </summary>
    private uint RoleID;
    /// <summary> 角色配置表数据 </summary>
    private RoleConfig rc = null;

    /// <summary> 角色名称 </summary>
    public UILabel Name;

    /// <summary> 被选中标记 </summary>
    public GameObject SelSign;
    /// <summary> 选择按钮 </summary>
    public GameObject SelBtn;
    /// <summary> 被锁定标记 </summary>
    public GameObject LockIcon;
    /// <summary> 解锁条件按钮 </summary>
    public GameObject UnLockConditionBtnObj;
    /// <summary> 解锁条件文字描述 </summary>
    public UILabel UnLockConditionLab;
    /// <summary> 站台 </summary>
    public GameObject Platform;

    /// <summary> 3D模型节点 </summary>
    private Transform Root;
    /// <summary> 角色动画 </summary>
    private Animation animation;
    private Animator animator;

    private float RoleModelScale = 360f;
    
    void Awake()
    {
        COC = gameObject.GetComponentInParent<UICenterOnChild>();
        //COC.springStrength = 1000;

        RoleID = uint.Parse(gameObject.name);
        if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue((int)RoleID, out rc))
        {
            if (GameApp.Instance.UILogin != null)
            {
                if (rc.UnLockType != 0)
                {
                    //UIWrapContent wc = transform.parent.GetComponent<UIWrapContent>();
                    DestroyImmediate(gameObject);
                    //wc.SortAlphabetically();
                }
            }
        }
    }

    void Start()
    {
        if(CsvConfigTables.Instance.RoleCsvDic.TryGetValue((int)RoleID,out rc))
        {
            GameObject model = Resources.Load<GameObject>(StringBuilderTool.ToString("Prefabs/Actor/", rc.ModelName));
            if (model != null)
            {
                GameObject obj = GameObject.Instantiate(model);
                MyTools.setLayerDeep(obj, LayerMask.NameToLayer("UI"));
                Root = obj.transform;
                Root.parent = transform;
                Root.localPosition = new Vector3(0, -216, -300);
                Root.localEulerAngles = new Vector3(0, RoleID < 1000 ? 180 : -140, 0);
                Root.localScale = Vector3.one * RoleModelScale;

                animation = Root.GetComponent<Animation>();
                animator = Root.GetComponent<Animator>();

                Name.text = rc.Name;

                if (rc.UnLockType != 0)
                {
                    LockIcon.SetActive(true);
                    UnLockConditionLab.text = rc.GetUnLockConditionDes();
                }
            }
        }

        //Refresh();
    }

    public void Refresh()
    {
        if (GameApp.Instance.MainPlayerData.RoleID == RoleID)
        {
            SelSign.SetActive(true);

            COC.springStrength = 1000;
            new Task(_CenterOn());
            //StartCoroutine("_CenterOn");
        }
    }

    IEnumerator _CenterOn()
    {
        while(!gameObject.activeInHierarchy)
            yield return new WaitForEndOfFrame();

        UICenterOnClick coc = gameObject.GetComponent<UICenterOnClick>();
        coc.OnClick();

        COC.springStrength = 8;

        if (!enablePlayAnim)
            yield break;
        enablePlayAnim = false;
        StartCoroutine("PlayShowAnim");
    }

     float ddd = 0.8f;
    void Update()
    {
        if (Root == null)
            return;

        Root.transform.localScale = Vector3.one * (RoleModelScale - (RoleModelScale - (RoleModelScale - 40f)) * Mathf.Abs(transform.position.x) / ddd);

        Name.transform.localPosition = new Vector3(0, 225f - (RoleModelScale - Root.transform.localScale.x) * 2f, -400);
        Name.transform.localScale = Vector3.one * (1f - (RoleModelScale - Root.transform.localScale.x) / RoleModelScale);

        SelSign.transform.localScale = Name.transform.localScale;

        LockIcon.transform.localScale = Name.transform.localScale;

        Platform.transform.localScale = Name.transform.localScale;
        Platform.transform.localPosition = new Vector3(0, -174 - (1 - Platform.transform.localScale.x) * 30f, 0);

        if(SelBtn.activeSelf)
        {
            if (COC.centeredObject != gameObject ||
                GameApp.Instance.MainPlayerData.RoleID == RoleID)
            {
                SelBtn.SetActive(false);
            }
        }
        else
        {
            if (COC.centeredObject == gameObject &&
                GameApp.Instance.MainPlayerData.RoleID != RoleID)
            {
                if (!LockIcon.activeSelf)
                {
                    SelBtn.SetActive(true);

                    if (!enablePlayAnim)
                        return;
                    enablePlayAnim = false;
                    StartCoroutine("PlayShowAnim");
                }
            }
        }

        if(SelSign.activeSelf)
        {
            if(GameApp.Instance.MainPlayerData.RoleID != RoleID)
            {
                SelSign.SetActive(false);
            }
        }
        else
        {
            if (GameApp.Instance.MainPlayerData.RoleID == RoleID)
            {
                SelSign.SetActive(true);
            }
        }

        if (LockIcon.activeSelf)
        {
            if (UnLockConditionBtnObj.activeSelf)
            {
                if (COC.centeredObject != gameObject)
                {
                    UnLockConditionBtnObj.SetActive(false);
                }
            }
            else
            {
                if (COC.centeredObject == gameObject)
                {
                    ObjShow(UnLockConditionBtnObj);
                }
            }
        }
    }

    void ObjShow(GameObject obj)
    {
        TweenAlpha[] TAs = obj.GetComponents<TweenAlpha>();
        for (int i = 0; i < TAs.Length; i++)
            DestroyImmediate(TAs[i]);

        obj.SetActive(true);

        TweenAlpha ta1 = TweenAlpha.Begin(obj, 0f, 0f);
        ta1.onFinished.Add(new EventDelegate(() =>
            {
                Destroy(ta1);
            }));

        TweenAlpha ta2 = TweenAlpha.Begin(obj, 0.2f, 1f);
        ta2.onFinished.Add(new EventDelegate(() =>
            {
                Destroy(ta2);
            }));
    }

    public void OnClick()
    {
        GameApp.Instance.SoundInstance.StopAllSe();

        if (COC.centeredObject != gameObject)
            return;

        if (LockIcon.activeSelf)
            return;

        if (!enablePlayAnim)
            return;
        enablePlayAnim = false;
        StartCoroutine("PlayShowAnim");
    }

    bool enablePlayAnim = true;
    IEnumerator PlayShowAnim()
    {
        StartCoroutine("StopPlayAnim");
        StartCoroutine("WaitLoopPlayAnim");
        yield return new WaitForSeconds(1f);

        if (GameApp.Instance.HomePageUI != null)
        {
            if (GameApp.Instance.HomePageUI.RoleUI.isShow)
                GameApp.Instance.SoundInstance.PlayVoice(rc.Voice);
        }
        else if (GameApp.Instance.UILogin != null)
        {
            if (GameApp.Instance.UILogin.RoleUI.isShow)
                GameApp.Instance.SoundInstance.PlayVoice(rc.Voice);
        }

        if (animation != null)
        {
            animation.CrossFade("attack", 0.2f);
            yield return new WaitForSeconds(animation["attack"].length);
            animation.CrossFade("idle", 0.2f);
        }
        else if (animator != null)
        {
            animator.CrossFade("attack", 0.2f);
            yield return new WaitForSeconds(AnimatorLength("attack"));
        }

        yield return new WaitForSeconds(0.2f);
        enablePlayAnim = true;
        StopCoroutine("StopPlayAnim");
    }
    IEnumerator PlaySelAnim(Action cb)
    {
        if (animation != null)
        {
            animation.CrossFade("win", 0.2f);
            yield return new WaitForSeconds(animation["win"].length);
            animation.CrossFade("idle", 0.2f);
        }
        else if (animator != null)
        {
            animator.CrossFade("win", 0.2f);
            yield return new WaitForSeconds(AnimatorLength("win"));
        }

        yield return new WaitForSeconds(0.2f);
        enablePlayAnim = true;

        if (cb != null)
            cb();
    }
    IEnumerator StopPlayAnim()
    {
        while (COC.centeredObject == gameObject)
            yield return new WaitForEndOfFrame();
        StopCoroutine("PlayShowAnim");
        StopCoroutine("WaitLoopPlayAnim");
        if (animation != null)
        {
            animation.CrossFade("idle", 0.2f);
        }
        else if (animator != null)
        {
            animator.CrossFade("idle", 0.2f);
        }
        enablePlayAnim = true;
    }
    IEnumerator WaitLoopPlayAnim()
    {
        yield return new WaitForSeconds(10f);
        if (COC.centeredObject != gameObject)
            yield break;

        enablePlayAnim = false;
        StartCoroutine("PlayShowAnim");
    }

    #region _按钮
    /// <summary> 点击选择 </summary>
    public void OnClick_Sel()
    {
        GameApp.Instance.SoundInstance.StopAllSe();
        GameApp.Instance.SoundInstance.PlaySe("button");

        Debug.Log("点击【选择】" + Name.text);

        if (COC.centeredObject != gameObject)
            return;

        if (GameApp.Instance.UILogin == null)
        {
            Action SelOver = () =>
            {
                GameApp.Instance.SoundInstance.StopAllSe();
                if (GameApp.Instance.HomePageUI != null)
                {
                    GameApp.Instance.HomePageUI.RoleUI.Show(false);
                }
                else if (GameApp.Instance.UILogin != null)
                {
                    GameApp.Instance.UILogin.RoleUI.Show(false);
                }
            };
            StartCoroutine("PlaySelAnim", SelOver);

            GameApp.Instance.MainPlayerData.RoleID = RoleID;
            GameApp.SendMsg.SetAvatar(RoleID);
        }
        else
        {
            GameApp.Instance.UILogin.RoleUI.SelOver(RoleID,Root);
        }
    }
    /// <summary> 点击解锁条件 </summary>
    public void OnClick_UnLockCondition()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【解锁条件】");

    }
    #endregion

    public float AnimatorLength(string motionName)
    {
        AnimationClip[] anis = animator.runtimeAnimatorController.animationClips;
        float atime = 0;
        for (int i = 0; i < anis.Length; ++i)
        {
            if ((motionName == "attack" && (anis[i].name == "NormalATK" || anis[i].name == "standA_idleA")) ||
                (motionName == "win" && (anis[i].name == "Victory" || anis[i].name == "standA_idleA")))
            {
                atime = anis[i].length;
                break;
            }
        }
        return atime;
    }
}
