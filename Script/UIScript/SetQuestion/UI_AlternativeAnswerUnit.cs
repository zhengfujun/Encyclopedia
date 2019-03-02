using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AlternativeAnswerUnit : MonoBehaviour
{
    public UISprite Bg;
    public UILabel Name;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Set(MagicCardConfig mcc)
    {
        Name.text = mcc.Name;
    }
}
