using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System;
#if(!UNITY_WEBPLAYER)
using System.Net.NetworkInformation;
#endif
using System.Net.Sockets;

//全局工具类
static public class MyTools
{
    #region _File 文件操作
    static public void ClearFile(string filePath)
    {
#if !NETFX_CORE
        StreamWriter sw;
        FileInfo t = new FileInfo(filePath);
        if (t.Exists)
        {
#if(!UNITY_WEBPLAYER)
            t.Delete();
#endif
        }
        sw = t.CreateText();
        sw.Close();
        sw.Dispose();
#endif
    }
    static public void WriteLineTxt(string filePath, string txt)
    {
#if !NETFX_CORE
        StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8);
        /*FileInfo t = new FileInfo(filePath);
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
        }*/
        sw.WriteLine(txt);
        sw.Close();
        sw.Dispose();
#endif
    }
    #endregion

    #region _Get 获取值
    /// <summary>
    /// 获得按照特定分割符分割的字符串中的最后一个字符串
    /// </summary>
    static public string GetLastString(string res, params char[] m_splitwords)
    {
        string[] split = res.Split(m_splitwords);
        if (split.Length > 0)
        {
            return split[split.Length - 1];
        }
        return "";
    }
    /// <summary>
    /// 获得按照特定分割符分割的字符串中的第一个字符串
    /// </summary>
    static public string GetFirstString(string res, params char[] m_spiltwords)
    {
        //string words = res;
        string[] split = res.Split(m_spiltwords);
        if (split.Length > 0)
        {
            return split[0];
        }
        return "";
    }
    /// <summary>
    /// 浮点数转字符串，含小数的保留2位小数，没有小数的只显示整数
    /// </summary>
    static public string FloatToString(float floatVal)
    {
        string outStr = floatVal.ToString("F2");
        if (outStr[outStr.Length - 1] == '0')
        {
            outStr = floatVal.ToString("F1");
            if (outStr[outStr.Length - 1] == '0')
            {
                outStr = floatVal.ToString("F0");
            }
        }
        return outStr;
    }

    static public float GetBigUIGroupScale()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        if (currentAspectRatio < 1.5f)
        {
            return ((float)Screen.width / Screen.height) / (2048f / 1536f);
        }
        else
        {
            return ((float)Screen.width / Screen.height) / (1920f / 1080f);
        }
    }
    /// <summary>
    /// 获得本地IP地址
    /// </summary>
    static public string GetIPAddress()
    {
#if UNITY_EDITOR
        NetworkInterface[] NI = NetworkInterface.GetAllNetworkInterfaces();
        for (int i = 0; i < NI.Length; i++)
        {
            if (NI[i].Supports(NetworkInterfaceComponent.IPv4))
            {
                UnicastIPAddressInformationCollection uniCast = NI[i].GetIPProperties().UnicastAddresses;
                if (uniCast.Count > 0)
                {
                    foreach (UnicastIPAddressInformation uni in uniCast)
                    {
                        if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return uni.Address.ToString();
                        }
                    }
                }
            }
        }
#endif
        return string.Empty;

    }

    ///  <summary>     
    /// 移除所有委托事件   
    ///  </summary>
    static public void RemoveAllEvent<T>(T c, string name)
    {
        Delegate[] invokeList = GetObjectEventList(c, name);
        if (invokeList == null)
            return;
        foreach (Delegate del in invokeList)
        {
            typeof(T).GetEvent(name).RemoveEventHandler(c, del);
        }
    }

    ///  <summary>     
    /// 获取对象事件   
    ///  </summary>     
    ///  <param name="p_Object">对象 </param>     
    ///  <param name="p_EventName">事件名 </param>     
    ///  <returns>委托列 </returns>     
    private static Delegate[] GetObjectEventList(object p_Object, string p_EventName)
    {
        System.Reflection.FieldInfo _Field = p_Object.GetType().GetField(p_EventName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        if (_Field == null)
        {
            return null;
        }
        object _FieldValue = _Field.GetValue(p_Object);
        if (_FieldValue != null && _FieldValue is Delegate)
        {
            Delegate _ObjectDelegate = (Delegate)_FieldValue;
            return _ObjectDelegate.GetInvocationList();
        }
        return null;
    } 
    #endregion

    #region _NodeOperate 结点处理
    /// <summary>
    /// 销毁子节点
    /// </summary>
    static public void DestroyChildNodes(Transform parent)
    {
        if (parent == null)
            return;
        int childs = parent.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(parent.GetChild(i).gameObject);
            childs = parent.childCount;
        }
        //Resources.UnloadUnusedAssets();
    }
    static public void DestroyImmediateChildNodes(Transform parent)
    {
        if (parent == null)
            return;
        int childs = parent.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
            childs = parent.childCount;
        }
    }
    /// <summary>
    /// 销毁指定子节点
    /// </summary>
    static public void DestroyChildNodes(Transform parent, string deleName)
    {
        if (null == parent)
            return;
        int childs = parent.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            if (parent.GetChild(i).gameObject.name.Equals(deleName))
            {
                GameObject.Destroy(parent.GetChild(i).gameObject);
                childs = parent.childCount;
            }
        }
        //Resources.UnloadUnusedAssets();
    }
    /// <summary>
    /// 深度遍历查找子结点
    /// </summary>
    static public Transform DeepinGetChild(Transform parent, string childName)
    {
        if (childName == "")
        {
            return null;
        }
        Transform child = parent.Find(childName);
        if (child == null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                child = DeepinGetChild(parent.GetChild(i), childName);
                if (child != null)
                {
                    return child;
                }
            }
        }
        return child;
    }
    static public Transform DeepinGetChild(Transform parent, string[] childName)
    {
        if (childName == null || childName.Length == 0)
        {
            return null;
        }
        for (int i = 0; i < childName.Length; i++)
        {
            Transform tryGet = DeepinGetChild(parent, childName[i]);
            if (tryGet != null)
            {
                return tryGet;
            }
        }
        return null;
    }
    /// <summary>
    /// 深度删除疑似子结点
    /// </summary>
    static public void DeepinDeleteChild(Transform parent, string includeStr)
    {
        if (parent == null || includeStr.Length == 0)
        {
            return;
        }
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name.Contains(includeStr))
            {
                GameObject.Destroy(child.gameObject);
            }
            else
            {
                DeepinDeleteChild(child, includeStr);
            }
        }
    }

    public static Transform FightFindNode(Transform trans, string nodeName)
    {
        Transform effParent = nodeName == "" ? trans : MyTools.DeepinGetChild(trans, nodeName);
        //if(effParent == null){
        //	effParent = MyTools.DeepinGetChild(trans, "Dummy_Hit") == null ? trans : MyTools.DeepinGetChild(trans, "Dummy_Hit");
        //}
        return effParent;
    }
    /// <summary>
    /// 绑定子物体
    /// </summary>
    static public void BindChild(Transform parent, GameObject child, bool isReset = true)
    {
        if (parent == null || child == null)
            return;

        Vector3 OldScale = child.transform.localScale;
        Vector3 OldPosition = child.transform.localPosition;
        Quaternion OldQuaternion = child.transform.localRotation;

        child.transform.parent = parent;

        if (!isReset)
        {
            child.transform.localScale = OldScale;
            child.transform.localPosition = OldPosition;
            child.transform.localRotation = OldQuaternion;
            return;
        }

        child.transform.localScale = Vector3.one;
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    /// <summary>
    /// 显示或隐藏指定Tag的结点
    /// </summary>
    static private GameObject RecordGO;
    static public void SetGameObjectActiveWithTag(string TagName, bool IsShow)
    {
        GameObject obj = GameObject.FindGameObjectWithTag(TagName);
        RecordGO = (obj == null ? RecordGO : obj);
        if (RecordGO)
        {
            RecordGO.SetActive(IsShow);
        }
    }

    /// <summary>
    /// 设置层
    /// </summary>
    static public void SetLayer(GameObject target, int layerMask, bool recursively = true)
    {
        if (target == null)
            return;

        List<Transform> stack = new List<Transform>();
        stack.Add(target.transform);

        while (stack.Count > 0)
        {
            Transform tr = stack[0];
            stack.RemoveAt(0);
            tr.gameObject.layer = layerMask;

            if (recursively)
            {
                for (int ii = 0; ii < tr.childCount; ++ii)
                {
                    stack.Add(tr.GetChild(ii));
                }
            }
        }
    }
    public static void setLayerDeep(GameObject obj, int index)
    {
        obj.layer = index;
        Transform _trans = obj.transform;
        for (int i = 0; i < _trans.childCount; i++)
        {
            setLayerDeep(_trans.GetChild(i).gameObject, index);
        }
    }

	public static void SetLayerWithOutParticle(GameObject obj, int index){
		obj.layer = index;
		Transform _trans = obj.transform;
		for(int i = 0; i < _trans.childCount; i++){
			Transform child = _trans.GetChild(i);
			if(child.GetComponent<ParticleSystem>() == null){
				child.gameObject.layer = index;
			}
		}		
	}

    public static void GetAllChild(Transform target, List<Transform> list)
    {
        if (target == null) return;

        foreach (Transform tran in target)
        {
            if (!list.Contains(tran))
                list.Add(tran);

            if (tran.childCount > 0)
                GetAllChild(tran, list);
        }
    }

    public static GameObject LoadObject(GameObject obj)
    {
        return GameObject.Instantiate(obj);
    }

    public static void DeapFindChildCompents(Transform trans, List<Renderer> rets)
    {
        foreach (Transform tran in trans)
        {
            Renderer temp = tran.GetComponent<Renderer>();
            if (temp != null)
                rets.Add(temp);

            if (tran.childCount > 0)
            {
                DeapFindChildCompents(tran, rets);
            }
        }
    }
    #endregion

    #region _Geometry 几何计算
    /// <summary>
    /// 屏幕上一点是否点在3D场景某区域内
    /// </summary>
    public static bool PtInArea(Vector2 point, Camera cmr, Vector2 area, Transform trans)
    {
        float hx = area.x / 2;
        float hy = area.y / 2;
        Rect ctrlRect = new Rect();
        ctrlRect.xMin = -hx;
        ctrlRect.xMax = hx;
        ctrlRect.yMin = -hy;
        ctrlRect.yMax = hy;
        return PointInPolygonArea(ctrlRect, cmr.ScreenToWorldPoint(point), trans);
    }
    private static bool PointInPolygonArea(Rect rect, Vector2 post, Transform trans)
    {
        Vector3[] v = new Vector3[4];
        v[0] = trans.TransformPoint(rect.xMin, rect.yMax, 0);
        v[1] = trans.TransformPoint(rect.xMax, rect.yMax, 0);
        v[2] = trans.TransformPoint(rect.xMax, rect.yMin, 0);
        v[3] = trans.TransformPoint(rect.xMin, rect.yMin, 0);
        if (post.x >= v[0].x && post.x <= v[2].x && post.y >= v[2].y && post.y <= v[0].y)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 求多边形的中心点
    /// </summary>
    public static Vector3 CalPolygonCenterPoint(List<Vector3> polygon)
    {
        float fMinX = 0, fMaxX = 0, fMinZ = 0, fMaxZ = 0;
        for (int i = 0; i < polygon.Count; ++i)
        {
            Vector3 item = polygon[i];
            if (i != 0)
            {
                if (item.x < fMinX)
                {
                    fMinX = item.x;
                }
                if (item.x > fMaxX)
                {
                    fMaxX = item.x;
                }
                if (item.z < fMinZ)
                {
                    fMinZ = item.z;
                }
                if (item.z > fMaxZ)
                {
                    fMaxZ = item.z;
                }
            }
            else
            {
                fMinX = item.x;
                fMaxX = item.x;
                fMinZ = item.z;
                fMaxZ = item.z;
            }
        }

        return new Vector3((fMinX + fMaxX) / 2.0f, polygon[0].y, (fMinZ + fMaxZ) / 2.0f);  
    }
    /// <summary>
    /// 判断点pnt是否在region内主程序
    /// </summary>
    public static bool isInRegion(Vector3 pnt, List<Vector3> region)
    {
        int wn = 0, j = 0; //wn 计数器 j第二个点指针
        for (int i = 0; i < region.Count; i++)
        {
            //开始循环
            if (i == region.Count - 1)
            {
                j = 0;//如果 循环到最后一点 第二个指针指向第一点
            }
            else
            {
                j = j + 1; //如果不是 ，则找下一点
            }

            if (region[i].z <= pnt.z) // 如果多边形的点 小于等于 选定点的 Y 坐标
            {
                if (region[j].z > pnt.z) // 如果多边形的下一点 大于于 选定点的 Y 坐标
                {
                    if (isLeft(region[i], region[j], pnt) > 0)
                    {
                        wn++;
                    }
                }
            }
            else
            {
                if (region[j].z <= pnt.z)
                {
                    if (isLeft(region[i], region[j], pnt) < 0)
                    {
                        wn--;
                    }
                }
            }
        }
        if (wn == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    //判断点在线的一边
    private static int isLeft(Vector3 P0, Vector3 P1, Vector3 P2)
    {
        int abc = (int)((P1.x - P0.x) * (P2.z - P0.z) - (P2.x - P0.x) * (P1.z - P0.z));
        return abc;
    }
    /// <summary>
    /// 求直线外一点到该直线的投影点(平面，x,z)
    /// </summary>
    /// <param name="pLine1">线上一点</param>
    /// <param name="pLine2">线上另一点</param>
    /// <param name="pOut">线外指定点</param>
    public static Vector3 GetProjectivePoint(Vector3 pLine1, Vector3 pLine2, Vector3 pOut)
    {
        double k = 0f;
        if (pLine2.x - pLine1.x == 0f)
        {
            return new Vector3(pLine1.x, pOut.y, pOut.z);
        }
        else if (pLine2.z - pLine1.z == 0f)
        {
            return new Vector3(pOut.x, pOut.y, pLine1.z);
        }

        k = (double)((pLine2.z - pLine1.z) / (pLine2.x - pLine1.x));

        float x = (float)((k * pLine1.x + pOut.x / k + pOut.z - pLine1.z) / (1 / k + k));
        float z = (float)(-1 / k * (x - pOut.x) + pOut.z);
        return new Vector3(x, pOut.y, z);
    }
    
    /// <summary>
    /// operate按照一些设定参数(height、distance、RotAngle)和一些缓冲系数(rotationDamping、heightDamping)跟随targetTrans
    /// 目标为Transform
    /// </summary>
    public static void CalculateTransform4Follow(GameObject operate, Transform targetTrans, Vector3 TarOffset, float height, float distance, float RotAngle, float rotationDamping = 30.0f, float heightDamping = 20.0f)
    {
        float wantedRotationAngle = targetTrans.eulerAngles.y + RotAngle;
        float wantedHeight = targetTrans.position.y + height;
        float currentRotationAngle = operate.transform.eulerAngles.y;
        float currentHeight = operate.transform.position.y;
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        operate.transform.position = targetTrans.position;
        operate.transform.position -= currentRotation * Vector3.forward * distance;
        operate.transform.position = new Vector3(operate.transform.position.x, currentHeight, operate.transform.position.z);

        operate.transform.LookAt(targetTrans.position + TarOffset);
    }
    /// <summary>
    /// operate按照一些设定参数(height、distance、RotAngle)和一些缓冲系数(rotationDamping、heightDamping)跟随targetTrans
    /// 目标为Vector3
    /// </summary>
    public static void CalculateTransform4Follow(GameObject operate, Vector3 targetTransPos, Vector3 TarOffset, float height, float distance, float RotAngle, float rotationDamping = 30.0f, float heightDamping = 20.0f)
    {
        float wantedRotationAngle = RotAngle;
        float wantedHeight = targetTransPos.y + height;
        float currentRotationAngle = operate.transform.eulerAngles.y;
        float currentHeight = operate.transform.position.y;
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        operate.transform.position = targetTransPos;
        operate.transform.position -= currentRotation * Vector3.forward * distance;
        operate.transform.position = new Vector3(operate.transform.position.x, currentHeight, operate.transform.position.z);

        operate.transform.LookAt(targetTransPos + TarOffset);
    }
    #endregion

    #region _RegEx 正则表达式
    /// <summary>
    /// 账号正则表达式[^u4E00-u9FA5]
    /// </summary>
    public static bool IsRegexMatchAc(string input, int length = 6)
    {
        if (input.Length < length) { return false; }
        return Regex.IsMatch(input, @"^[0-9a-zA-Z]{0,8}[_]{0,1}[0-9a-zA-Z]{0,8}$");
    }
    /// <summary>
    /// 检测IP地址是否正确
    /// </summary>
    public static bool IsRegexMatchIP(string input)
    {
        return Regex.IsMatch(input, @"^((25[0-5]|2[0-4]\d|[0-1]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[0-1]?\d\d?)$");
    }
    /// <summary>
    /// 检测是否是数字
    /// </summary>
    public static bool IsInt(string inString)
    {
        Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
        return regex.IsMatch(inString.Trim());
    }
    /// <summary>
    /// 输入密码
    /// </summary>
    public static bool IsRegexMatchPwd(string input, int length = 6)
    {
        if (input.Length < length) { return false; }
        return Regex.IsMatch(input, @"^[0-9a-zA-Z]{0,12}$");
    }
    /// <summary>
    /// 名称排除符号
    /// </summary>
    public static bool IsRegexMatchName(string input)
    {
        return Regex.IsMatch(input, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
    }
    #endregion

    #region _Random 随机值
    /// <summary>
    /// 获得随机数组
    /// 0到total中选出count个，其中不包括barring
    /// </summary>
    public static int[] GetRandomNumArray4Barring(int count, int total, int barring)
    {
        int[] index = new int[total];
        bool addOne = false;
        for (int i = 0; i < total; i++)
        {
            if (barring == i)
            {
                addOne = true;
            }
            index[i] = (addOne ? i + 1 : i);
        }
        System.Random r = new System.Random();
        int[] result = new int[count];
        int id;
        for (int j = 0; j < count; j++)
        {
            id = r.Next(0, total - 1);
            result[j] = index[id];
            index[id] = index[total - 1];
            total--;
        }
        return result;
    }
    /// <summary>
    /// 获得随机数组
    /// 1到total中选出count个，其中不包括barring中的任意值
    /// </summary>
    public static int[] GetRandomNumArray4BarringEx(int count, int total, int[] barring)
    {
        int[] index = new int[total];
        for (int i = 0; i < total; i++)
        {
            index[i] = i;
        }
        System.Random r = new System.Random();
        int[] result = new int[count + barring.Length];
        int[] resultEx = new int[count];
        int id;
        for (int j = 0; j < count + barring.Length; j++)
        {
            id = r.Next(1, total - 1);
            result[j] = index[id];
            index[id] = index[total - 1];
            total--;
        }
        for (int k = 0, f = 0; k < result.Length; k++)
        {
            bool isRepeat = false;
            for (int h = 0; h < barring.Length; h++)
            {
                if (result[k] == barring[h])
                {
                    isRepeat = true;
                    break;
                }
            }
            if (!isRepeat)
            {
                resultEx[f] = result[k];
                f++;
                if (f == count)
                {
                    break;
                }
            }
        }
        return resultEx;
    }
    /// <summary>
    /// 获得随机数组
    /// 0到total中选出count个，其中包括include
    /// </summary>
    public static int[] GetRandomNumArray4Include(int count, int total, int include)
    {
        int[] index = new int[total];
        for (int i = 0; i < total; i++)
        {
            index[i] = i;
        }
        System.Random r = new System.Random();
        int[] result = new int[count];
        int id;
        for (int j = 0; j < count; j++)
        {
            id = r.Next(0, total - 1);
            result[j] = index[id];
            index[id] = index[total - 1];
            total--;
        }
        bool isHaveInclude = false;
        for (int k = 0; k < result.Length; k++)
        {
            if (result[k] == include)
            {
                isHaveInclude = true;
                break;
            }
        }
        if (!isHaveInclude)
        {
            result[0] = include;
        }
        return result;
    }
    /// <summary>
    /// 生成特定位数的唯一字符串
    /// </summary>
    /// <param name="num">特定位数</param>
    static string readyStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public static string GenerateUniqueText(int num)
    {
        string randomResult = string.Empty;
        char[] rtn = new char[num];
        Guid gid = Guid.NewGuid();
        var ba = gid.ToByteArray();
        for (var i = 0; i < num; i++)
        {
            rtn[i] = readyStr[((ba[i] + ba[num + i]) % (readyStr.Length-1))];
        }
        foreach (char r in rtn)
        {
            randomResult += r;
        }
        return randomResult;
    }
    #endregion

    #region 时间转换
#if !NETFX_CORE
    static public DateTime getTime(int _time)
    {
        string timeStamp = _time.ToString();
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(StringBuilderTool.ToInfoString(timeStamp,"0000000"));
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime dtResult = dtStart.Add(toNow);
        return dtResult;
    }
#endif

    static public int dateDiff(DateTime dtStart, DateTime dtEnd)
    {
        TimeSpan tsStart = new TimeSpan(dtStart.Ticks);
        TimeSpan tsEnd = new TimeSpan(dtEnd.Ticks);
        TimeSpan ts = tsEnd.Subtract(tsStart).Duration();
        int dateDiffSecond = ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;

        //两个时间的秒差
        return Mathf.Abs(dateDiffSecond);
    }
    /// <summary>
    /// 将某时间转换为剩余时间描述
    /// </summary>
    static public string GetSurplusTimeDes(int serverTime)
    {
        int secdelte = MyTools.dateDiff(MyTools.getTime(serverTime), DateTime.Now);
        string str = "";
        if ((secdelte / 3600) >= 24)
        {
            str = (secdelte / 3600 / 24).ToString() + "天";
        }
        else
        {
            if ((secdelte / 3600) >= 1)
            {
                str = (secdelte / 3600).ToString() + "小时";
            }
            else
            {
                str = (secdelte / 60).ToString() + "分钟";
            }
        }
        return str;
    }
    #endregion
    
}
