using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InputHint : MonoBehaviour
{
    private UIInput inp;
    private UILabel lab;

    private string recordTxt;

    void Awake()
    {
        inp = transform.parent.GetComponent<UIInput>();

        lab = gameObject.GetComponent<UILabel>();
        recordTxt = lab.text;

        Refresh();
    }

    void Update()
    {
        if(Time.frameCount%5 == 0)
        {
            Refresh();
        }
    }

    void Refresh()
    {
        if (inp.value.Length == 0)
        {
            lab.text = recordTxt;
        }
        else
        {
            lab.text = string.Empty;
        }
    }
}
