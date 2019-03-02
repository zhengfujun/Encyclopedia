using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelOnGrid : MonoBehaviour
{
    public string RecordGridName = string.Empty;

    /*void Start()
    {

    }*/

    /*void Update()
    {

    }*/

    public void Init(Transform Grid)
    {
        if (Grid == null)
            return;

        RecordGridName = Grid.name;

        transform.position = Grid.position;
        gameObject.SetActive(true);
    }

    public bool IsStepOn(Transform Grid)
    {
        bool stepon = (RecordGridName == Grid.name);
        if (stepon)
        {
            gameObject.SetActive(false);
        }
        return stepon;
    }
}
