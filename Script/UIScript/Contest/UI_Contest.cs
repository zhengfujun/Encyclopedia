using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_Contest : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public GameObject LstGrid;
    public GameObject ContestUnitPrefab;

    public GameObject ConditionHintRoot;
    public UILabel ConditionDes;

    public GameObject MatchingRoot;
    public Transform[] WrapContentRoot;

    public GameObject DetailsRoot;
    public UILabel NameInDetails;
    public UILabel TimeInDetails;
    public UILabel RuleInDetails;
    public UI_AwardItem[] EntranceItemInDetails;

    /*void Awake()
    {

    }*/

    void Start()
    {

    }

    /*void OnDestroy()
    {

    }*/

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            StartCoroutine("_NormalEndScroll_Succeed");
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            StartCoroutine("_NormalEndScroll_Failure");
        }
    }*/

    public void Show(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            GameApp.Instance.UICurrency.Show(true);

            if (LstGrid.transform.childCount == 0)
            {
                GameObject newUnit = NGUITools.AddChild(LstGrid, ContestUnitPrefab);
                newUnit.SetActive(true);
                newUnit.name = "ContestUnit_" + 1;
                newUnit.GetComponent<UI_ContestUnit>().Set(1, "15万金币赛", "明日\n[00FF54]17:00[-]", new Dictionary<int, int>() { { 1002, 512 } }, new Dictionary<int, int>() { { 1002, 512 }, { 1003, 1024 } });

                GameObject newUnit2 = NGUITools.AddChild(LstGrid, ContestUnitPrefab);
                newUnit2.SetActive(true);
                newUnit2.name = "ContestUnit_" + 2;
                newUnit2.GetComponent<UI_ContestUnit>().Set(2, "挑战赛", "今日\n[00FF54]20:00[-]", new Dictionary<int, int>() { { 1001, 128 } }, new Dictionary<int, int>() { { 1002, 2048 } });

                GameObject newUnit3 = NGUITools.AddChild(LstGrid, ContestUnitPrefab);
                newUnit3.SetActive(true);
                newUnit3.name = "ContestUnit_" + 3;
                newUnit3.GetComponent<UI_ContestUnit>().Set(3, "冠军赛", "比赛进行中\n[EC3636]00:48:00[-]", new Dictionary<int, int>() { { 1000, 64 } }, new Dictionary<int, int>() { { 1002, 9527 } });

                GameObject newUnit4 = NGUITools.AddChild(LstGrid, ContestUnitPrefab);
                newUnit4.SetActive(true);
                newUnit4.name = "ContestUnit_" + 4;
                newUnit4.GetComponent<UI_ContestUnit>().Set(4, "自由竞速赛", "9.28\n[00FF54]20:00[-]", new Dictionary<int, int>() { { 1000, 16800 } }, new Dictionary<int, int>() { { 1001, 9527 } });

                GameObject newUnit5 = NGUITools.AddChild(LstGrid, ContestUnitPrefab);
                newUnit5.SetActive(true);
                newUnit5.name = "ContestUnit_" + 5;
                newUnit5.GetComponent<UI_ContestUnit>().Set(5, "知识大赛", "10.3\n[00FF54]21:00[-]", new Dictionary<int, int>() { { 1002, 99998 } }, new Dictionary<int, int>() { { 1001, 100 }, { 1000, 600 } });
                 
                LstGrid.transform.GetComponent<UIGrid>().repositionNow = true;
                LstGrid.transform.parent.GetComponent<UIScrollView>().ResetPosition();
            }
        }
        else
        {
            GameApp.Instance.UICurrency.Show(false);

            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    public void ShowConditionHint(string des)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        TweenAlpha.Begin(ConditionHintRoot,0.2f,1);
        ConditionDes.text = des;
    }

    public void ShowMatching()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        TweenAlpha.Begin(MatchingRoot, 0.2f, 1);

        StartCoroutine("_ScrollResult");
        StartCoroutine("_BeginScroll");
    }

    Coroutine[] ScrollCoro = null;
    float[] ScrollInterval = null;
    int[] ScrollRes = null;
    bool[] ScrollStop = null;
    IEnumerator _ScrollResult()
    {
        yield return new WaitForSeconds(3.0f);

        int resType = UnityEngine.Random.Range(0, 2);
        switch(resType)
        {
            case 0:
                StartCoroutine("_NormalEndScroll_Failure");
                break;
            case 1:
                StartCoroutine("_NormalEndScroll_Succeed");
                break;
        }
    }
    IEnumerator _BeginScroll()
    {
        ScrollCoro = new Coroutine[3] { null, null, null };
        ScrollInterval = new float[3] { 0.1f, 0.1f, 0.1f };
        ScrollRes = new int[3] { 0, 0, 0 };
        ScrollStop = new bool[3] { false, false, false };

        for (int i = 0; i < WrapContentRoot.Length; i++)
        {
            Transform child = WrapContentRoot[i].GetChild(0);
            UICenterOnChild coc = child.GetComponent<UICenterOnChild>();
            coc.CenterOn(child);
        }

        for (int i = 0; i < WrapContentRoot.Length; i++)
        {
            ScrollCoro[i] = StartCoroutine("_Scroll", i);

            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator _Scroll(int index)
    {
        int j = 0;
        while (true)
        {
            if (index == 0 && ScrollStop[index])
                yield break;

            if (index != 0 && ScrollStop[0] && ScrollRes[0] == ScrollRes[index])
                yield break;

            Transform child = WrapContentRoot[index].GetChild(j);
            UICenterOnChild coc = child.GetComponent<UICenterOnChild>();
            coc.CenterOn(child);
            ScrollRes[index] = j;

            yield return new WaitForSeconds(ScrollInterval[index]);

            j++;
            if(j >= WrapContentRoot[index].childCount)
                j = 0;
        }
    }
    IEnumerator _NormalEndScroll_Succeed()
    {
        while (ScrollInterval[0] < 0.2f)
        {
            ScrollInterval[0] += 0.005f;
            yield return new WaitForEndOfFrame();
        }
        ScrollStop[0] = true;

        yield return new WaitForSeconds(0.2f);

        while (ScrollInterval[1] < 0.2f)
        {
            ScrollInterval[1] += 0.005f;
            yield return new WaitForEndOfFrame();
        }
        ScrollStop[1] = true;

        yield return new WaitForSeconds(0.2f);

        while (ScrollInterval[2] < 0.2f)
        {
            ScrollInterval[2] += 0.005f;
            yield return new WaitForEndOfFrame();
        } 
        ScrollStop[2] = true;

        yield return new WaitForSeconds(0.5f);
        GameApp.Instance.CommonMsgDlg.OpenMsgBox("匹配成功！", (ok) =>
            {
                OnClick_HideMatching();
            });
    }
    IEnumerator _NormalEndScroll_Failure()
    {
        while (ScrollInterval[0] < 0.2f)
        {
            ScrollInterval[0] += 0.005f;
            yield return new WaitForEndOfFrame();
        }
        int curRes = ScrollRes[0];
        StopCoroutine(ScrollCoro[0]);

        yield return new WaitForSeconds(0.2f);

        while (ScrollInterval[1] < 0.2f)
        {
            ScrollInterval[1] += 0.005f;
            yield return new WaitForEndOfFrame();
        }
        while (ScrollRes[0] == ScrollRes[1])
        {
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(ScrollCoro[1]);

        yield return new WaitForSeconds(0.2f);

        while (ScrollInterval[2] < 0.2f)
        {
            ScrollInterval[2] += 0.005f;
            yield return new WaitForEndOfFrame();
        }
        while (ScrollRes[1] == ScrollRes[2])
        {
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(ScrollCoro[2]);

        yield return new WaitForSeconds(0.5f);
        GameApp.Instance.CommonMsgDlg.OpenMsgBox("匹配失败！", (ok) =>
            {
                OnClick_HideMatching();
            });
    }
    void _ForceEndScroll()
    {
        StopCoroutine("_ScrollResult");
        StopCoroutine("_BeginScroll");
        for (int i = 0; i < ScrollCoro.Length; i++)
            StopCoroutine(ScrollCoro[i]);
    }

    UI_ContestUnit CurDetailsCU = null;
    public void ShowDetails(UI_ContestUnit cu)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        CurDetailsCU = cu;
        NameInDetails.text = CurDetailsCU.Name.text;
        TimeInDetails.text = CurDetailsCU.Time.text.Replace("\n","：");
        RuleInDetails.text = "规则就是没有规则，随意哦！";
        for (int i = 0; i < CurDetailsCU.EntranceItem.Length; i++)
        {
            EntranceItemInDetails[i].gameObject.SetActive(CurDetailsCU.EntranceItem[i].gameObject.activeSelf);

            EntranceItemInDetails[i].Icon.spriteName = CurDetailsCU.EntranceItem[i].Icon.spriteName;
            //EntranceItemInDetails[i].Icon.MakePixelPerfect();

            EntranceItemInDetails[i].Name.text = CurDetailsCU.EntranceItem[i].Name.text;
        }

        TweenAlpha.Begin(DetailsRoot, 0.2f, 1);

    }
    public void OnClick_HideDetails()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        TweenAlpha.Begin(DetailsRoot, 0.2f, 0);

    }
    public void OnClick_SignUpInDetails()
    {
        CurDetailsCU.OnClick_SignUp();
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }

    public void OnClick_HideConditionHint()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        TweenAlpha.Begin(ConditionHintRoot, 0.2f, 0);
    }

    public void OnClick_HideMatching()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        _ForceEndScroll();
        TweenAlpha.Begin(MatchingRoot, 0.2f, 0);
    }
    #endregion
}
