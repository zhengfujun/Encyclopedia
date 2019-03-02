using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 宠物名称
/// </summary>
public class RandomPetName : RandomName
{
    protected override void Start()
    {
        base.Start();

        GameApp.Instance.RandomPetNameInstance = this;
    }
}
