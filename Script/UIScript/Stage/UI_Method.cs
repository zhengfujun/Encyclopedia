using UnityEngine;
using System.Collections;

public enum EStageState
{
    eNull,
    eLock,  //未解锁
    eUnlock,//已解锁
    ePass   //已通关
}

public class UI_Method : MonoBehaviour
{
    protected UILabel Name;
    
    [HideInInspector]
    public int CurIndex = -1;//自身序号

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        
    }

    public virtual void SetState(EStageState State)
    {

    }
}
