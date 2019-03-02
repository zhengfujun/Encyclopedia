using System;
using System.IO;
using System.Collections;
using UnityEngine;

public class IniParser
{
    private Hashtable keyPairs = new Hashtable();

    private struct SectionPair
    {
        public String Section;
        public String Key;
    }

    /// <summary>
    /// Opens the INI file at the given path and enumerates the values in the IniParser.
    /// </summary>
    /// <param name="iniPath">Full path to INI file.</param>
    public IniParser(string _loadPath)
    {
        String strLine = null;
        String currentRoot = null;
        String[] keyPair = null;

        TextAsset ta = (TextAsset)Resources.Load(_loadPath);
        if (ta == null)
        {
            NGUIDebug.Log("加载Ini配置文件失败： " + _loadPath);
            return;
        }
        ByteReader reader = new ByteReader(ta);

        try
        {
            strLine = reader.ReadLine();

            while (strLine != null)
            {
                strLine = strLine.Trim().ToUpper();

                if (strLine != "")
                {
                    if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                    {
                        currentRoot = strLine.Substring(1, strLine.Length - 2);
                    }
                    else
                    {
                        keyPair = strLine.Split(new char[] { '=' }, 2);

                        SectionPair sectionPair;
                        String value = null;

                        if (currentRoot == null)
                            currentRoot = "ROOT";

                        sectionPair.Section = currentRoot;
                        sectionPair.Key = keyPair[0];

                        if (keyPair.Length > 1)
                            value = keyPair[1];

                        keyPairs.Add(sectionPair, value);
                    }
                }
                strLine = reader.ReadLine();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Returns the value for the given section, key pair.
    /// </summary>
    /// <param name="sectionName">Section name.</param>
    /// <param name="settingName">Key name.</param>
    public String GetSetting(String sectionName, String settingName)
    {
        SectionPair sectionPair;
        sectionPair.Section = sectionName.ToUpper();
        sectionPair.Key = settingName.ToUpper();

        return (String)keyPairs[sectionPair];
    }

    /// <summary>
    /// Adds or replaces a setting to the table to be saved.
    /// </summary>
    /// <param name="sectionName">Section to add under.</param>
    /// <param name="settingName">Key name to add.</param>
    /// <param name="settingValue">Value of key.</param>
    public void AddSetting(String sectionName, String settingName, String settingValue)
    {
        SectionPair sectionPair;
        sectionPair.Section = sectionName.ToUpper();
        sectionPair.Key = settingName.ToUpper();

        if (keyPairs.ContainsKey(sectionPair))
            keyPairs.Remove(sectionPair);

        keyPairs.Add(sectionPair, settingValue);
    }
}