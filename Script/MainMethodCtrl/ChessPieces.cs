using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieces : MonoBehaviour
{
    //protected uint RoleID = 0;
    private TweenPosition tp;

    protected Transform RecordLocationGrid;

    //protected virtual void Awake()
    //{

    //}

    //protected virtual void Start()
    public void Init(uint RoleID)
    {
        tp = gameObject.GetComponentInChildren<TweenPosition>(true);
        MyTools.DestroyChildNodes(tp.transform);

        RoleID = Math.Max(RoleID, 1);
        RoleConfig rc = null;
        if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue((int)RoleID, out rc))
        {
            GameObject model = Resources.Load<GameObject>("Prefabs/Actor/" + rc.ModelName);
            if (model != null)
            {
                GameObject obj = GameObject.Instantiate(model);
                obj.transform.parent = tp.transform;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localEulerAngles = Vector3.zero;
                obj.transform.localScale = Vector3.one * 2;
            }
        }
        else
        {
            Debug.LogError("ChessPieces::Start 角色ID未设置！");
        }
    }

    /*protected virtual void Update()
    {

    }*/

    /*protected void SetInitPos(ERoleType type)
    {
        switch (type)
        {
            case ERoleType.eProtagonist:
                //transform.localPosition = new Vector3(-0.6f, 0, -1);
                break;
            case ERoleType.eOpponent:
                //transform.localPosition = new Vector3(-1.6f, 0, -1);
                break;
        }
    }*/

    public void ToSide(ERoleType type)
    {
        transform.localPosition += GetToSidePos(type);
    }
    public Vector3 GetToSidePos(ERoleType type, Transform grid = null)
    {
        /*float offect = 0.4f;
        Square s = (grid == null ? RecordLocationGrid : grid).GetComponent<Square>();
        switch (s.GridDirType)
        {
            case EGridDirType.eZ_Positive:
                switch (type)
                {
                    case ERoleType.eProtagonist:
                        return new Vector3(offect, 0, 0);
                    case ERoleType.eOpponent:
                        return new Vector3(-offect, 0, 0);
                }
                break;
            case EGridDirType.eZ_Negative:
                switch (type)
                {
                    case ERoleType.eProtagonist:
                        return new Vector3(-offect, 0, 0);
                    case ERoleType.eOpponent:
                        return new Vector3(offect, 0, 0);
                }
                break;
            case EGridDirType.eX_Positive:
                switch (type)
                {
                    case ERoleType.eProtagonist:
                        return new Vector3(0, 0, -offect);
                    case ERoleType.eOpponent:
                        return new Vector3(0, 0, offect);
                }
                break;
            case EGridDirType.eX_Negative:
                switch (type)
                {
                    case ERoleType.eProtagonist:
                        return new Vector3(0, 0, offect);
                    case ERoleType.eOpponent:
                        return new Vector3(0, 0, -offect);
                }
                break;
        }*/
        
        return Vector3.zero;
    }

    public void Jump()
    {
        tp.to = new Vector3(0, 1, 0);
        tp.enabled = true;
        tp.PlayForward();
        StartCoroutine("Recover");
    }
    public void BigJump()
    {
        tp.to = new Vector3(0,2,0);
        tp.enabled = true;
        tp.PlayForward();
        StartCoroutine("Recover");
    }
    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.25f);
        tp.PlayReverse();
    }

    public void SetLocationGrid(Transform grid)
    {
        RecordLocationGrid = grid;

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
    public bool IsOnThisGrid(Transform grid)
    {
        if (RecordLocationGrid != null)
            return RecordLocationGrid.name == grid.name;
        else
            return false;
    }
}
