using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using common;

public class TaskInfoDataStruct
{
    public string TaskInfo
    {
        get
        {
            if (GameApp.Instance.PlayerData != null)
                return GameApp.Instance.PlayerData.m_player_base.m_player_task;
            else
                return string.Empty;
        }
        set
        {
            GameApp.SendMsg.SetTaskData(value);
        }
    }

    //测试数值1
    public int TempValue1
    {
        get
        {
            return _tempValue1;
        }
        set
        {
            _tempValue1 = value;
        }
    }
    private int _tempValue1 = 0;

    //测试数值2
    public int TempValue2
    {
        get
        {
            return _tempValue2;
        }
        set
        {
            _tempValue2 = value;
        }
    }
    private int _tempValue2 = 0;

    public TaskInfoDataStruct()
    {
        if (GameApp.Instance.PlayerData != null)
        {
            if (GameApp.Instance.PlayerData.m_player_base.m_player_task.Length == 0)
            {
                TempValue1 = 0;

                TempValue2 = 0;

                LstDataToString();
            }
            else
            {
                StringToLstData();
            }
        }
    }

    private void LstDataToString()
    {

        TaskInfo = StringBuilderTool.ToString(TempValue1, "#", TempValue2);
#if UNITY_EDITOR
        Debug.Log("最新任务数据：");
        Debug.Log(TaskInfo);
#endif
    }

    private void StringToLstData()
    {
        string[] temps = TaskInfo.Split('#');
        if (temps.Length == 2)
        {
            TempValue1 = int.Parse(temps[0]);

            TempValue2 = int.Parse(temps[1]);
        }
    }
}