using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 加载Csv格式的配置文件
/// </summary>
public class CsvDataInitialize : MonoBehaviour
{
    void Awake()
    {
        LoadCsvAsset<ParameterConfig>(ref CsvConfigTables.Instance.ParameterCsvDic, "Parameter");
        LoadCsvAsset<RoleConfig>(ref CsvConfigTables.Instance.RoleCsvDic, "Role");
        LoadCsvAsset<MagicCardConfig>(ref CsvConfigTables.Instance.MagicCardCsvDic, "Card");
        LoadCsvAsset<QuestionConfig>(ref CsvConfigTables.Instance.QuestionCsvDic, "Question");
        LoadCsvAsset<ValidationLibraryConfig>(ref CsvConfigTables.Instance.ValidationLibraryCsvDic, "ValidationLibrary");
        LoadCsvAsset<ItemConfig>(ref CsvConfigTables.Instance.ItemCsvDic, "Item");
        LoadCsvAsset<ShopConfig>(ref CsvConfigTables.Instance.ShopCsvDic, "Shop");
        LoadCsvAsset<MonsterConfig>(ref CsvConfigTables.Instance.MonsterCsvDic, "Monster");
        LoadCsvAsset<StageConfig>(ref CsvConfigTables.Instance.StageCsvDic, "Stage");
        LoadCsvAsset<GroupConfig>(ref CsvConfigTables.Instance.GroupCsvDic, "Group");
        LoadCsvAsset<ChapterConfig>(ref CsvConfigTables.Instance.ChapterCsvDic, "Chapter");
        LoadCsvAsset<AchievementConfig>(ref CsvConfigTables.Instance.AchievementCsvDic, "Achievement");
        LoadCsvAsset<ActivityConfig>(ref CsvConfigTables.Instance.ActivityCsvDic, "Activity");
    }

    void LoadCsvAsset<T>(ref Dictionary<int, T> dic, string CsvFileName)
    {
        TextAsset ta = (TextAsset)Resources.Load("Config/CsvConfigs/" + CsvFileName);
        if (ta == null)
            NGUIDebug.Log("加载Csv配置文件失败 1： " + CsvFileName);

        CSVObjMapperExt mapper = new CSVObjMapperExt();
        dic = mapper.GetIntDictFromCSVString<T>(ta.text);
    }

    void LoadCsvAsset<T>(ref Dictionary<uint, T> dic, string CsvFileName)
    {
        TextAsset ta = (TextAsset)Resources.Load("Config/CsvConfigs/" + CsvFileName);
        if (ta == null)
            NGUIDebug.Log("加载Csv配置文件失败 2： " + CsvFileName);

        CSVObjMapperExt mapper = new CSVObjMapperExt();
        dic = mapper.GetUintDictFromCSVString<T>(ta.text);
    }

    void LoadCsvAsset<T>(ref List<T> lst, string CsvFileName)
    {
        TextAsset ta = (TextAsset)Resources.Load("Config/CsvConfigs/" + CsvFileName);
        if (ta == null)
            NGUIDebug.Log("加载Csv配置文件失败 3： " + CsvFileName);

        CSVObjMapperExt mapper = new CSVObjMapperExt();
        lst = mapper.GetListFromCSVstring<T>(ta.text);
    }

    void LoadCsvAsset<T>(ref T t, string CsvFileName)
    {
        TextAsset ta = (TextAsset)Resources.Load("Config/CsvConfigs/" + CsvFileName);
        if (ta == null)
            NGUIDebug.Log("加载Csv配置文件失败 4： " + CsvFileName);

        CSVObjMapperExt mapper = new CSVObjMapperExt();
        t = mapper.GetClassFromCSVstring<T>(ta.text);
    }
}
