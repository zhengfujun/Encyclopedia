using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//走马灯（滚动公告）
public class RollingNoticeDlg : MonoBehaviour
{
    public GameObject SprBg;
    public UILabel LabNotice;

    private List<string> NoticesLst = new List<string>();
    private bool Rollinging = false;

    //未到显示时段的，等待加入显示列表的公告
    private class WaitNoticeInfo
    {
        public DateTime beginTime;
        public string content;
        public WaitNoticeInfo(DateTime t, string str)
        {
            beginTime = t;
            content = str;
        }
    }
    private List<WaitNoticeInfo> WaitNoticesLst = new List<WaitNoticeInfo>();
    
    void Start()
    {
        GameApp.Instance.RollingNoticeDlg = this;

        InvokeRepeating("UpdateWaitNoticesLst", 0, 60);
    }

    internal void AddRollingNotice(string hintText)
    {
        NoticesLst.Add(hintText);

        if (!Rollinging)
        {
            TweenAlpha.Begin(SprBg, 0.2f, 0.75f);

            BeginRolling();
        }
    }

    void UpdateWaitNoticesLst()
    {
        if (WaitNoticesLst.Count > 0)
        {
            for (int i = 0; i < WaitNoticesLst.Count; i++)
            {
                if (DateTime.Now >= WaitNoticesLst[i].beginTime)
                {
                    AddRollingNotice(WaitNoticesLst[i].content);
                    WaitNoticesLst.RemoveAt(i);
                    continue;
                }
            }
        }
    }

    float offectX = 0f;
    IEnumerator DoingRolling()
    {
        yield return new WaitForEndOfFrame();

        while (LabNotice.transform.localPosition.x > 600 - 1200 - LabNotice.width)
        {
            offectX += 2f;
            LabNotice.transform.localPosition = new Vector3(600 - offectX, -2, 0);
            yield return new WaitForSeconds(0.01f);
        }

        NoticesLst.RemoveAt(0);
        yield return new WaitForEndOfFrame();

        if (NoticesLst.Count > 0)
        {
            BeginRolling();
        }
        else
        {
            Rollinging = false;
            TweenAlpha.Begin(SprBg, 0.2f, 0f);
        }
    }

    void BeginRolling()
    {
        offectX = 0f;
        Rollinging = true;
        LabNotice.text = NoticesLst[0];
        LabNotice.transform.localPosition = new Vector3(600, -2, 0);
        StartCoroutine("DoingRolling");
    }
}
