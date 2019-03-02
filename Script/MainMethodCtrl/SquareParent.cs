using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareParent : MonoBehaviour
{
    public ECheckerboardType GridType = ECheckerboardType.eNormal;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateNewGrid()
    {
        string name = "";
        switch(GridType)
        {
            case ECheckerboardType.eNull:
                name += "_";
                break;
            case ECheckerboardType.eNormal:
                name += "N_";
                break;
            case ECheckerboardType.eBoss:
                name += "B_";
                break;
        }
        int MaxIndex = -1;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            int index = int.Parse(MyTools.GetLastString(child.name, '_'));
            if (index > MaxIndex)
            {
                MaxIndex = index;
            }
        }
        name += (++MaxIndex).ToString();

        GameObject obj = Resources.Load<GameObject>("Prefabs/Scene/Grid/SquareParent");
        if (obj != null)
        {
            GameObject newGrid = GameObject.Instantiate(obj);
            newGrid.name = name;
            newGrid.transform.parent = transform;
            newGrid.transform.localPosition = Vector3.zero;
            newGrid.transform.localEulerAngles = Vector3.zero;
            newGrid.transform.localScale = Vector3.one;

            Square s = newGrid.GetComponent<Square>();
            s.GridDirType = EGridDirType.eZ_Positive;
            s.GridModelType = EGridModelType.eGrid_Cretaceous_Grass_Olivine;
            s.SignModelType = ESignModelType.eNull;
            s.SignDirType = ESignDirType.eNull;
            s.EventType = EEventType.eNull;
            s.CreateGrid();
        }
    }
}
