using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 随机名称
/// </summary>
public class RandomName : MonoBehaviour
{
    public TextAsset RandomNameTxtFile;

    List<string> firstNames = new List<string>();
    List<string> secondNames = new List<string>();

    protected virtual void Start()
    {
        if (firstNames.Count == 0 && secondNames.Count == 0)
        {
            firstNames = GetNameStringList(RandomNameTxtFile.text, 0);
            secondNames = GetNameStringList(RandomNameTxtFile.text, 1);
        }
    }

    /// <summary>
    /// 获得随机角色名称
    /// </summary>
    public string GenerateRandomName()
    {
        int firstSelector = UnityEngine.Random.Range(0, firstNames.Count);
        int secondSelector = UnityEngine.Random.Range(0, secondNames.Count);

        //Debug.Log(firstSelector + " " + secondSelector);
        return (firstNames[firstSelector] + secondNames[secondSelector]);
    }

    /// <summary>
    /// 获得姓名表中字符串组
    /// </summary>
    List<string> GetNameStringList(string fullString, int index)
    {
        string[] lineArray = fullString.Split('\n');
        if (index < lineArray.Length)
        {
            string[] tempStrings = lineArray[index].Split(',');
            List<string> tempStrList = new List<string>(tempStrings.Length - 1);
            for (int i = 1; i < tempStrings.Length; i++)
            {
                tempStrings[i] = tempStrings[i].Replace("\r", "");
                tempStrList.Add(tempStrings[i]);
            }
            return tempStrList;
        }
        else
        {
            Debug.LogError("Index out of range , maybe bigger than 1!");
            return null;
        }
    }
}
