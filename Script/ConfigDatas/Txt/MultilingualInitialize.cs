using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
/// 加载多语言表
/// </summary>
public class MultilingualInitialize : MonoBehaviour
{
    public TextAsset LanguageFile;

    void Awake()
    {
        Localization.loadFunction = LoadFunction;
        Localization.language = Const.Language;
    }

    byte[] LoadFunction(string FileName)
    {
        return Encoding.UTF8.GetBytes(LanguageFile.text);
    }
}
