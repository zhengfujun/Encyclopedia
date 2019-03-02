using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDecorateModelType//装饰物模型类型
{
    eNull,
    eDecorate_1,   //1号装饰物
    eDecorate_2    //2号装饰物
};

public class Decorate : MonoBehaviour
{
    public int Index = -1;

    private string DecorateModelTypeString
    {
        get
        {
            switch (DecorateModelType)
            {
                default:
                case EDecorateModelType.eNull: return "";
                case EDecorateModelType.eDecorate_1: return "Decorate_1";
                case EDecorateModelType.eDecorate_2: return "Decorate_2";
            }
        }
    }

    public EDecorateModelType DecorateModelType = EDecorateModelType.eNull;

    void Start()
    {
        Index = int.Parse(MyTools.GetLastString(gameObject.name, '_'));
    }

    public void CreateDecorate()
    {
        MyTools.DestroyImmediateChildNodes(transform);

        if (DecorateModelType == EDecorateModelType.eNull)
            return;

        GameObject obj = Resources.Load<GameObject>("Prefabs/Scene/Decorate/" + DecorateModelTypeString);
        if (obj != null)
        {
            GameObject Grid = GameObject.Instantiate(obj);
            Grid.transform.parent = transform;
            Grid.transform.localPosition = Vector3.zero;
            Grid.transform.localEulerAngles = Vector3.zero;
            Grid.transform.localScale = Vector3.one;
        }
    }
}
