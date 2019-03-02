using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 玩家角色名称
/// </summary>
public class RandomPlayerName : RandomName
{
    protected override void Start()
    {
        base.Start();

        GameApp.Instance.RandomNameInstance = this;
    }
}
