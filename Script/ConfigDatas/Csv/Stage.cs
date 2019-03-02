using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageConfig
{
    #region Configuration 文件中数据段
    /// <summary> 关卡ID </summary>
    public int StageID;

    /// <summary> 章节ID </summary>
    public int ChapterID;

    /// <summary> 关卡组ID </summary>
    public int GroupID;

    /// <summary> 排列序号 </summary>
    public int ArrayIndex;

    /// <summary> 前置关卡 </summary>
    public int PreStageIdx;

    /// <summary> 后续关卡 </summary>
    public int AftStageIdx;

    /// <summary> 关卡名称 </summary>
    public string Name;

    /// <summary> 图标资源名 </summary>
    public string Icon;

    /// <summary> 缩略图资源名 </summary>
    public string Thumb;

    /// <summary> 战斗场景名 </summary>
    public string FightSceneName;

    /// <summary> 红天使随机出现最小格子序号 </summary>
    public int AngelRandomAppearMinGrid;

    /// <summary> 红天使随机出现最大格子序号 </summary>
    public int AngelRandomAppearMaxGrid;

    /// <summary> 通关固定奖励 </summary>
    public List<int> FixedAward;

    /// <summary> 切换至战斗场景时的Loading图资源名 </summary>
    public string LoadingPic;

    /// <summary> 入场动画 </summary>
    public string EnterAnimation;

    /// <summary> 场景介绍 </summary>
    public string SceneIntroduce;

    /// <summary> 场景解说语音 </summary>
    public string Voice;
    #endregion

    #region Extend 扩展数据段

    #endregion
}
