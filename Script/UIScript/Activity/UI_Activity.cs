using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_Activity : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public GameObject AccumulativeLoginUnitPrefab;
    public UIGrid AccumulativeLoginGrid;

    public List<GameObject> ActivityDetailsRootLst = new List<GameObject>();

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

        }
    }*/

    public void Show(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            StartCoroutine("RefreshAccumulativeLoginList");
        }
        else
        {
            AppearEffect.Close(AppearType.Popup, () =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    IEnumerator RefreshAccumulativeLoginList()
    {
        string AccumulativeLoginKey = StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_AccumulativeLogin");
        if (!PlayerPrefs.HasKey(AccumulativeLoginKey))
        {
            PlayerPrefs.SetString(AccumulativeLoginKey, DateTime.Now.Ticks.ToString());
        }
        string LastTime = PlayerPrefs.GetString(AccumulativeLoginKey);
        DateTime dtLastTime = new DateTime(long.Parse(LastTime));
        TimeSpan ResidueTime = DateTime.Now - dtLastTime;

        MyTools.DestroyChildNodes(AccumulativeLoginGrid.transform);

        UIScrollView sv = AccumulativeLoginGrid.transform.parent.GetComponent<UIScrollView>();
        for (int i = 1; i <= 31; i++)
        {
            GameObject newUnit = NGUITools.AddChild(AccumulativeLoginGrid.gameObject, AccumulativeLoginUnitPrefab);
            newUnit.SetActive(true);
            newUnit.name = "AccumulativeLoginUnit_" + i;
            newUnit.transform.localPosition = new Vector3(0, -140 * (i-1), 0);

            UI_AccumulativeLoginUnit alu = newUnit.GetComponent<UI_AccumulativeLoginUnit>();
            alu.Set(i, ResidueTime.Days+1);

            sv.ResetPosition();
            AccumulativeLoginGrid.repositionNow = true;
            
            yield return new WaitForEndOfFrame();
        }
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }
    /// <summary> 各类活动标签 </summary>
    private int RecordCurTypeIdx = -1;
    private int RecordLastTypeIdx = -1;
    public void OnTypeToggleChange()
    {
        if (UIToggle.current.value)
        {
            RecordCurTypeIdx = int.Parse(MyTools.GetLastString(UIToggle.current.name, '_'));

            TweenAlpha.Begin(ActivityDetailsRootLst[RecordCurTypeIdx], 0.2f, 1);

            switch (RecordCurTypeIdx)
            {
                case 0:

                    break;
                case 1:
                    
                    break;
                case 2:
                    
                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
            }

            if (RecordCurTypeIdx != RecordLastTypeIdx && RecordLastTypeIdx != -1)
            {
                TweenAlpha.Begin(ActivityDetailsRootLst[RecordLastTypeIdx], 0.2f, 0);
            }
            RecordLastTypeIdx = RecordCurTypeIdx;

            Debug.Log(UIToggle.current.name);
        }
    }
    #endregion
}
