using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupConfig
{
    #region Configuration 文件中数据段
    /// <summary> 关卡组ID </summary>
    public int GroupID;

    /// <summary> 关卡组名称 </summary>
    public string Name;

    /// <summary> 图标资源名 </summary>
    public string Icon;

    /// <summary> 主题图片资源名 </summary>
    public string ThemePic;

    /// <summary> 切换场景图 </summary>
    public string LoadingPic;

    /// <summary> 魔法书台子 </summary>
    public string TBpg;

    /// <summary> 魔法书背景 </summary>
    public string BHpg;

    /// <summary> 模型root点位置（0脚下、1中心） </summary>
    public int Mroot;

    /// <summary> 说明 </summary>
    public string Explain;
    #endregion

    #region Extend 扩展数据段

    #endregion
}