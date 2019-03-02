using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
public class StringBuilderTool
{
    public static StringBuilder sBuilder = new StringBuilder();
    public static int length;
    private static readonly object _lockObj = new object();

    public static string ToSymbolString(string symbol, params object[] args)
    {
        lock (_lockObj)
        {
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            length = args.Length;
            for (int id = 0; id < length; id++)
            {
                sBuilder.Append(symbol).Append(args[id]);
            }
            return sBuilder.ToString();
        }
    }

    public static string ToString(params object[] args)
    {
        lock (_lockObj) {
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            length = args.Length;
            for (int id = 0; id < length; id++)
            {
                sBuilder.Append(args[id]);
            }
            return sBuilder.ToString();
        }
    }

    public static string ToSymbolStringInfo(string symbol, params string[] infoStr)
    {
        lock (_lockObj)
        {
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            length = infoStr.Length;
            for (int id = 0; id < length; id++)
            {
                sBuilder.Append(symbol).Append(infoStr[id]);
            }
            return sBuilder.ToString();
        }
    }


    public static string ToInfoString(params string[] infoStr)
    {
        lock (_lockObj) {
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            length = infoStr.Length;
            for (int id = 0; id < length; id++)
            {
                sBuilder.Append(infoStr[id]);
            }
            return sBuilder.ToString();
        }
    }
    public static string ToListString(string[] infoStr)
    {
        lock (_lockObj) {
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            length = infoStr.Length;
            for (int id = 0; id < length; id++)
            {
                sBuilder.Append(infoStr[id]);
            }
            return sBuilder.ToString();
        }
    }
    public static string AppendFormat(string formatStr, params object[] args)
    {
        lock (_lockObj) {
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            sBuilder.AppendFormat(formatStr, args);
            return sBuilder.ToString();
        }
    }

    //相关限制条件 提前限制
    public static string ReplaceString(string parentStr, string origStr, string replaceStr)
    {
        lock (_lockObj) {
            //sBuilder = new StringBuilder(parentStr);
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            sBuilder.Append(parentStr);
            return sBuilder.Replace(origStr, replaceStr).ToString();
        }
    }

    public static string RemoveString(string parentStr, int start, int length)
    {
        lock (_lockObj)
        {
            //sBuilder = new StringBuilder(parentStr);
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            sBuilder.Append(parentStr);
            return sBuilder.Remove(start, length).ToString();
        }
    }

    public static string RemoveString(string parentStr, int start)
    {
        lock (_lockObj)
        {
            //sBuilder = new StringBuilder(parentStr);
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            sBuilder.Append(parentStr);
            return sBuilder.Remove(start, parentStr.Length - start).ToString();
        }
    }

    public static string SubString(string parentStr, int start, int length)
    {
        lock (_lockObj)
        {
            //sBuilder = new StringBuilder(parentStr);
            if (sBuilder.Length != 0) sBuilder.Length = 0;
            sBuilder.Append(parentStr);
            if (start > 0) sBuilder.Remove(0, start);
            return sBuilder.Remove(start + length, parentStr.Length - start - length).ToString();
        }
    }

    static List<string> strList = new List<string>();
    static StringBuilder curStr = new StringBuilder();

    static List<char> charList = new List<char>();
    public static string[] Split(string parentStr,char splitChar,bool isRemoveEmpty) {
        lock (_lockObj)
        {
            strList.Clear();
            char indexChar = ' ';
            for (int index = 0, length = parentStr.Length; index < length; index++)
            {
                indexChar = parentStr[index];
                if ((indexChar == splitChar))
                {
                    if (charList.Count == 0)
                    {
                        if (!isRemoveEmpty)
                        {
                            strList.Add("");
                        }
                    }
                    else
                    {
                        strList.Add(curStr.Append(charList.ToArray()).ToString());
                        curStr.Length = 0;
                        charList.Clear();
                    }
                }
                else {
                    charList.Add(indexChar);
                    if (index == length - 1)
                    {
                        strList.Add(curStr.Append(charList.ToArray()).ToString());
                        charList.Clear();
                        curStr.Length = 0;
                    }
                }
            }
            return strList.ToArray();
        }
    }


    public static string[] SplitString(string parentStr, char splitChar, bool isRemoveEmpty)
    {
        lock (_lockObj)
        {
            if (strList.Count >0) strList.Clear();
            curStr.Append(parentStr);
            char indexChar = ' ';
            int oldIndex = -1;
            for (int index = 0, length = curStr.Length; index < length; index++)
            {
                indexChar = curStr[index];
                if (indexChar == splitChar)
                {
                    if (index - oldIndex <= 1)
                    {
                        if (!isRemoveEmpty)
                        {
                            strList.Add("");
                        }
                    }
                    else
                    {
                        strList.Add(curStr.ToString(oldIndex + 1,index - 1 - oldIndex));
                    }
                    oldIndex = index;
                }
                else
                {
                    if (index == length - 1)
                    {
                        strList.Add(curStr.ToString(oldIndex + 1, index - oldIndex));
                    }
                }
            }
            curStr.Length = 0;
            return strList.ToArray();
        }
    }
}
