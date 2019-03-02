using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Travel_LetterBox : MonoBehaviour
{
    [HideInInspector]
    public bool isShowing = false;

    public GameObject LetterUnitPrefab;
    public UIGrid LetterGrid;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Show()
    {
        isShowing = true;

        TweenAlpha.Begin(gameObject, 0.1f, 1);

        List<LetterInfo> TempLetterLst = new List<LetterInfo>()
            /*{
                new LetterInfo("宝宝龙回家啦！", new Dictionary<int, int>() { { 1001, 200 }, { 20001, 2 } }),
                new LetterInfo("宝宝龙又回家啦！", new Dictionary<int, int>() { { 1002, 100 } }),
                new LetterInfo("宝宝龙只能回家啊！", new Dictionary<int, int>() { { 30002, 4 }, { 30004, 2 } })
            }*/;
        if (PlayerPrefs.HasKey("TempLetter"))
        {
            string msg = PlayerPrefs.GetString("TempLetter");
            string[] s = msg.Split('#');
            for(int i = 0; i < s.Length; i++)
            {
                TempLetterLst.Add(new LetterInfo(s[i], null));
            }
        }
        StartCoroutine("RefreshLetterList", TempLetterLst);
    }

    public void Hide()
    {
        isShowing = false;

        TweenAlpha.Begin(gameObject, 0.1f, 0);
    }

    IEnumerator RefreshLetterList(List<LetterInfo> LLst)
    {
        MyTools.DestroyChildNodes(LetterGrid.transform);
        for (int i = 0; i < LLst.Count; i++)
        {
            GameObject newUnit = NGUITools.AddChild(LetterGrid.gameObject, LetterUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "LetterUnit_" + i;

            UI_Travel_LetterBox_LetterUnit lu = newUnit.GetComponent<UI_Travel_LetterBox_LetterUnit>();
            lu.SetLetterData(LLst[i]);

            LetterGrid.repositionNow = true;
            LetterGrid.transform.parent.GetComponent<UIScrollView>().ResetPosition();

            yield return new WaitForEndOfFrame();
        }
    }
}
