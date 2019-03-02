using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RecordVersion
{
    static private string VersionFilePath = "Config/TextConfigs/Version";

    static private string CurRecordVersion = "";

    static public string Read()
    {
        TextAsset ta = (TextAsset)Resources.Load(VersionFilePath);
        if (ta == null)
        {
            NGUIDebug.Log("加载记录版本文件失败： " + VersionFilePath);
            return "";
        }
        ByteReader reader = new ByteReader(ta);
        try
        {
            CurRecordVersion = reader.ReadLine();

            Debug.Log("读取：" + CurRecordVersion);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return CurRecordVersion;
    }

    static public void Write(string ver)
    {
        CurRecordVersion = ver;

        string FilePath = Application.dataPath + "/Resources/" + VersionFilePath+".txt";
        string[] str = ver.Split(';');
        File.WriteAllLines(FilePath, str);

        Debug.Log("写入：" + CurRecordVersion);

#if UNITY_EDITOR
        string newPath = FilePath.Substring(FilePath.IndexOf("Assets"));
        UnityEditor.AssetDatabase.ImportAsset(newPath);
#endif
    }
}
