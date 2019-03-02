using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_Achievement : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public GameObject LstGrid;
    public GameObject AchievementUnitPrefab;

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

            GameApp.Instance.UICurrency.OnlyShowState();

            MyTools.DestroyImmediateChildNodes(LstGrid.transform);
            if (LstGrid.transform.childCount == 0)
            {
                foreach (KeyValuePair<int, AchievementConfig> pair in CsvConfigTables.Instance.AchievementCsvDic)
                {
                    if (pair.Value.Predecessors == -1)
                    {
                        GameObject newUnit = NGUITools.AddChild(LstGrid, AchievementUnitPrefab);
                        newUnit.SetActive(true);
                        newUnit.name = "AchievementUnit_" + pair.Key;

                        newUnit.GetComponent<UI_AchievementUnit>().Set(pair.Key);
                    }
                }

                List<UI_AchievementUnit> tempEnableGetAwardTULst = new List<UI_AchievementUnit>();
                List<UI_AchievementUnit> tempPutThroughTULst = new List<UI_AchievementUnit>();
                List<UI_AchievementUnit> tempInProgressTULst = new List<UI_AchievementUnit>();

                for (int i = 0; i < LstGrid.transform.childCount; i++)
                {
                    Transform child = LstGrid.transform.GetChild(i);
                    UI_AchievementUnit tu = child.GetComponent<UI_AchievementUnit>();

                    if (tu.IsHasAwardEnableGet)
                    {
                        tempEnableGetAwardTULst.Add(tu);
                    }
                    else if (tu.IsPutThrough)
                    {
                        tempPutThroughTULst.Add(tu);
                    }
                    else
                    {
                        tempInProgressTULst.Add(tu);
                    }
                }
                foreach (UI_AchievementUnit tu in tempEnableGetAwardTULst)
                    tu.transform.parent = null;
                foreach (UI_AchievementUnit tu in tempInProgressTULst)
                    tu.transform.parent = null;
                foreach (UI_AchievementUnit tu in tempPutThroughTULst)
                    tu.transform.parent = null;

                foreach (UI_AchievementUnit tu in tempEnableGetAwardTULst)
                    tu.transform.parent = LstGrid.transform;
                foreach (UI_AchievementUnit tu in tempInProgressTULst)
                    tu.transform.parent = LstGrid.transform;
                foreach (UI_AchievementUnit tu in tempPutThroughTULst)
                    tu.transform.parent = LstGrid.transform;

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

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }
    #endregion
}
