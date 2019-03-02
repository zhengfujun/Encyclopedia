using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoleConfig
{
    #region Configuration 文件中数据段
    /// <summary> 角色ID </summary>
    public int RoleID;

    /// <summary> 模型名 </summary>
    public string ModelName;
    
    /// <summary> 模型高度 </summary>
    public float ModelHigh;
    
    /// <summary> 名称 </summary>
    public string Name;

    /// <summary> 头像(小) </summary>
    public string Portrait;
    /// <summary> 头像(大) </summary>
    public string PortraitEx;

    /// <summary>语音</summary>
    public string Voice;

    /// <summary> 解锁类型（0:无需解锁，默认开放使用；1:通关某关卡；2:充值一定金额） </summary>
    public int UnLockType;

    /// <summary> 解锁条件 </summary>
    public string UnLockCondition;
    #endregion

    #region Extend 扩展数据段
    /// <summary> 获取解锁条件文字描述 </summary>
    public string GetUnLockConditionDes()
    {
        switch(UnLockType)
        {
            default:
            case 0:
                return "";
            case 1:
                string[] TempSplit = UnLockCondition.Split('_');
                if(TempSplit.Length == 3)
                {
                    ChapterConfig cc = null;
                    if (CsvConfigTables.Instance.ChapterCsvDic.TryGetValue(int.Parse(TempSplit[0]), out cc))
                    {
                        GroupConfig gc = null;
                        if (CsvConfigTables.Instance.GroupCsvDic.TryGetValue(int.Parse(TempSplit[1]), out gc))
                        {
                            StageConfig sc = null;
                            if (CsvConfigTables.Instance.StageCsvDic.TryGetValue(int.Parse(TempSplit[2]), out sc))
                            {
                                return StringBuilderTool.ToInfoString("通关 " + cc.Name, "-", gc.Name, "-" + sc.Name);
                            }
                        }
                    }    
                }
                return "解锁条件数据格式错误！";
            case 2:
                return "充值" + float.Parse(UnLockCondition) + "元";
        }
    }
    #endregion
}
