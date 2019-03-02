using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Csv类型全局数据结构
/// </summary>
public class CsvConfigTables : SimpleSingleton<CsvConfigTables>
{
    /// <summary> 参数表数据 </summary>
    public Dictionary<int, ParameterConfig> ParameterCsvDic = new Dictionary<int, ParameterConfig>();

    /// <summary> 角色表数据 </summary>
    public Dictionary<int, RoleConfig> RoleCsvDic = new Dictionary<int, RoleConfig>();

    /// <summary> 魔卡表数据 </summary>
    public Dictionary<int, MagicCardConfig> MagicCardCsvDic = new Dictionary<int, MagicCardConfig>();

    /// <summary> 题库数据 </summary>
    public Dictionary<int, QuestionConfig> QuestionCsvDic = new Dictionary<int, QuestionConfig>();
    
    /// <summary> 验证问题库数据 </summary>
    public Dictionary<int, ValidationLibraryConfig> ValidationLibraryCsvDic = new Dictionary<int, ValidationLibraryConfig>();

    /// <summary> 道具数据 </summary>
    public Dictionary<int, ItemConfig> ItemCsvDic = new Dictionary<int, ItemConfig>();

    /// <summary> 商店数据 </summary>
    public Dictionary<int, ShopConfig> ShopCsvDic = new Dictionary<int, ShopConfig>();

    /// <summary> 怪物数据 </summary>
    public Dictionary<int, MonsterConfig> MonsterCsvDic = new Dictionary<int, MonsterConfig>();

    /// <summary> 关卡数据 </summary>
    public Dictionary<int, StageConfig> StageCsvDic = new Dictionary<int, StageConfig>();
    /// <summary> 关卡组数据 </summary>
    public Dictionary<int, GroupConfig> GroupCsvDic = new Dictionary<int, GroupConfig>();
    /// <summary> 章节数据 </summary>
    public Dictionary<int, ChapterConfig> ChapterCsvDic = new Dictionary<int, ChapterConfig>();

    /// <summary> 成就数据 </summary>
    public Dictionary<int, AchievementConfig> AchievementCsvDic = new Dictionary<int, AchievementConfig>();

    /// <summary> 活动数据 </summary>
    public Dictionary<int, ActivityConfig> ActivityCsvDic = new Dictionary<int, ActivityConfig>();
}
