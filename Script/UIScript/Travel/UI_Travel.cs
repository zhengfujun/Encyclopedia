using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using common;

public class UI_Travel : MonoBehaviour
{
    public UILabel MainTitle;
    public GameObject BackBtn;

    public UI_GashaponMachine GashaponMachineUI;
    public UI_NewMagicBook MagicBookUI;

    public GameObject BaoBaoLongModel;

    public GameObject GetOutHint;
    public GameObject ComeBackHint;

    public GameObject AwardPanel;
    public UI_MagicCard AwardCard;
    public UILabel AwardHint;

    public GameObject CourtyardRoot;
    public GameObject RoomRoot;
    public GameObject SeafloorRoot;
    public Camera CourtyardCam;
    public Camera RoomCam;
    public Camera SeafloorCam;
    [HideInInspector]
    public Camera SceneCam;

    public UI_Travel_Shop TravelShop;
    public UI_Travel_LetterBox TravelLetterBox;
    public UI_Travel_Present TravelPresent;
    public UI_Travel_Backpack TravelBackpack;

    public UI_Travel_Seafloor TravelSeafloor;
    public GameObject SeafloorEntrance;

    void Awake()
    {
        GameApp.Instance.TravelUI = this;

#if UNITY_ANDROID
        SeafloorEntrance.SetActive(true);
#elif UNITY_IPHONE
        SeafloorEntrance.SetActive(false);
#endif

        if((int)UIRoot.ScreenHeight == 720)
        {
            CourtyardCam.fieldOfView = 60;
            RoomCam.fieldOfView = 80;
        }
        else
        {
            CourtyardCam.fieldOfView = 80;
            RoomCam.fieldOfView = 96;
        }
    }

    void Start()
    {
        SceneCam = CourtyardCam;
        CourtyardRoot.SetActive(true);
        RoomRoot.SetActive(false);
        SeafloorRoot.SetActive(false);

        if(GameApp.Instance.PlayerData != null)
        {
            int CurSugarCntOnTree = (int)GameApp.Instance.PlayerData.m_player_base.m_settle_candy;
            for (int i = 0; i < CurSugarCntOnTree; i++)
            {
                CreateSugar(i);
            }

            UpdateBaoBaoLongState();

            if (GameApp.Instance.NeedShowGetOutHint)
                ShowGetOutHint();

            if (GameApp.Instance.NeedShowComeBackHint)
                ShowComeBackHint();
        }
    }

    void OnDestroy()
    {
        GameApp.Instance.TravelUI = null;
    }

    public bool IsSomeUIShowing()
    {
        return (GashaponMachineUI.AppearEffect.isShowing ||
                MagicBookUI.AppearEffect.isShowing ||
                GameApp.Instance.TravelUI.TravelShop.isShowing ||
                GameApp.Instance.TravelUI.TravelLetterBox.isShowing ||
                GameApp.Instance.TravelUI.TravelPresent.isShowing ||
                GameApp.Instance.TravelUI.TravelBackpack.isShowing);
    }

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            CreateSugar(1);
            CreateSugar(3);
            CreateSugar(4);
            CreateSugar(5);
            CreateSugar(7);
        }
    }*/

    //public void Show(bool isShow)
    //{
        //if (isShow)
        //{
            //GameApp.Instance.FadeHelperInstance.FadeIn(0.05f, Color.black, () =>
            //{
                //gameObject.SetActive(true);

                //AppearEffect.transform.localScale = Vector3.one * 0.01f;
                //AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject, 0, () =>
                //{
                    //GameApp.Instance.FadeHelperInstance.FadeOut(0.05f);
                //});

                //GameApp.Instance.UICurrency.transform.localPosition = new Vector3(-10,0,0);
                //GameApp.Instance.UICurrency.Show(true);

                //CourtyardRoot.SetActive(true);
                //RoomRoot.SetActive(false);
            //});
        /*}
        else
        {
            GameApp.Instance.UICurrency.transform.localPosition = Vector3.zero;
            GameApp.Instance.UICurrency.Show(false);

            GameApp.Instance.FadeHelperInstance.FadeIn(0.05f, Color.black, () =>
            {
                AppearEffect.Close(AppearType.Popup, () =>
                {
                    GameApp.Instance.FadeHelperInstance.FadeOut(0.05f);

                    gameObject.SetActive(false);
                });
            });
        }*/
    //}

    public void ShowGetOutHint()
    {
        TweenAlpha.Begin(GetOutHint, 0.2f, 1);
        GameApp.Instance.NeedShowGetOutHint = false;
    }
    public void ShowComeBackHint()
    {
        TweenAlpha.Begin(ComeBackHint, 0.2f, 1);
        GameApp.Instance.NeedShowComeBackHint = false;
    }

    public void UpdateBaoBaoLongState()
    {
        switch ((ActionType)GameApp.Instance.PlayerData.m_player_action.m_type)
        {
            case ActionType.ActionType_Travel:
                BaoBaoLongModel.SetActive(false);
                break;
            case ActionType.ActionType_Playing:
                BaoBaoLongModel.SetActive(true);
                break;
        }
    }

    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        if (SeafloorRoot.activeSelf)
        {
            OnClick_BackToCourtyardFromSeafloor();
            return;
        }

        GameApp.Instance.SceneCtlInstance.ChangeScene(SceneControl.HomePage);
    }
    /// <summary> 关闭通知 </summary>
    public void OnClick_CloseNotice(GameObject root)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭通知】");

        TweenAlpha.Begin(root,0.2f,0);

        if(GameApp.Instance.TravelAwardLst.Count > 0)
        {
            foreach (KeyValuePair<int, int> pair in GameApp.Instance.TravelAwardLst)
            {
                ItemConfig ItemCfg = null;
                CsvConfigTables.Instance.ItemCsvDic.TryGetValue(pair.Key, out ItemCfg);
                if (ItemCfg != null)
                {
                    if (ItemCfg.Type == 2)
                    {
                        GameApp.Instance.TravelAwardLst.Remove(pair.Key);

                        AwardPanel.SetActive(true);
                        TweenAlpha.Begin(AwardPanel, 0f, 1).from = 0;
                        TweenScale.Begin(AwardPanel, 0.2f, Vector3.one).from = Vector3.zero;
                        AwardCard.UnconditionalShow(ItemCfg.ItemID);

                        AwardHint.text = StringBuilderTool.ToString("宝宝龙为您带回了 [FEE209]", AwardCard.FullName(), "[-]");

                        break;
                    }
                }
            }

            if (GameApp.Instance.TravelAwardLst.Count > 0)
            {
                GameApp.Instance.GetItemsDlg.OpenGetItemsBox(GameApp.Instance.TravelAwardLst);
                GameApp.Instance.TravelAwardLst.Clear();
            }
        }
    }
    /// <summary> 点击关闭奖励卡牌界面 </summary>
    public void OnClick_CloseAward()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【关闭奖励界面】");

        TweenAlpha.Begin(AwardPanel, 0.2f, 0).onFinished.Add(new EventDelegate(() =>
        {
            AwardPanel.SetActive(false);
            AwardPanel.transform.localScale = Vector3.zero;
        }));
    }
    // <summary> 点击糖果树 </summary>
    public void OnClick_SugarTree()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【糖果树】");

        
    }
    // <summary> 点击扭蛋 </summary>
    public void OnClick_Gashapon()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【扭蛋】");

        GashaponMachineUI.Show(true);
    }
    // <summary> 点击商店 </summary>
    public void OnClick_Shop()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【商店】");

        TravelShop.Show();
    }
    // <summary> 点击房间 </summary>
    public void OnClick_Room()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【房间】");

        SceneCam = RoomCam;

        GameApp.Instance.FadeHelperInstance.FadeIn(0.1f, Color.black, () =>
            {
                CourtyardRoot.SetActive(false);
                RoomRoot.SetActive(true);

                MainTitle.gameObject.SetActive(false);
                BackBtn.SetActive(false);

                GameApp.Instance.FadeHelperInstance.FadeOut(0.1f);
            });
    }
    // <summary> 点击魔法水塘 </summary>
    public void OnClick_Seafloor()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【魔法水塘】");

        SceneCam = SeafloorCam;

        GameApp.Instance.FadeHelperInstance.FadeIn(0.1f, Color.black, () =>
        {
            MainTitle.text = "魔法水塘";

            CourtyardRoot.SetActive(false);
            SeafloorRoot.SetActive(true);

            TravelSeafloor.Show();

            GameApp.Instance.FadeHelperInstance.FadeOut(0.1f);
        });
    }
    // <summary> 点击由魔法水塘返回庭院 </summary>
    public void OnClick_BackToCourtyardFromSeafloor()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【由魔法水塘返回庭院】");

        SceneCam = CourtyardCam;

        GameApp.Instance.FadeHelperInstance.FadeIn(0.1f, Color.black, () =>
        {
            MainTitle.text = "旅行";

            CourtyardRoot.SetActive(true);
            SeafloorRoot .SetActive(false);

            TravelSeafloor.Hide();

            GameApp.Instance.FadeHelperInstance.FadeOut(0.1f);
        });
    }

    // <summary> 点击信箱 </summary>
    public void OnClick_LetterBox()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【信箱】");

        TravelLetterBox.Show();
    }
    // <summary> 点击背包 </summary>
    public void OnClick_Bag()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【背包】");

        TravelBackpack.Show();
    }
    // <summary> 点击庭院 </summary>
    public void OnClick_Courtyard()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【庭院】");

        SceneCam = CourtyardCam;

        GameApp.Instance.FadeHelperInstance.FadeIn(0.1f, Color.black, () =>
        {
            CourtyardRoot.SetActive(true);
            RoomRoot.SetActive(false);

            MainTitle.gameObject.SetActive(true);
            BackBtn.SetActive(true);

            GameApp.Instance.FadeHelperInstance.FadeOut(0.1f);
        });
    }
    // <summary> 点击道具 </summary>
    public void OnClick_Item()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【道具】");

        TravelBackpack.DirectShowShowItemLst();
    }
    // <summary> 点击礼物 </summary>
    public void OnClick_Present()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【礼物】");

        TravelPresent.Show();
    }
    // <summary> 点击书架 </summary>
    public void OnClick_Bookrack()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【书架】");

        MagicBookUI.Show(true);
    }
    #endregion

    #region _Sugar 糖果相关操作
    public List<GameObject> SugarObjLst = new List<GameObject>();
    // <summary> 生成糖果 </summary>
    public void CreateSugar(int Idx)
    {
        if (Idx >= 0 && Idx < SugarObjLst.Count)
            SugarObjLst[Idx].gameObject.SetActive(true);
    }
    // <summary> 摘取糖果 </summary>
    public void PickSugar(int Idx)
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
#if UNITY_EDITOR
        Debug.Log(StringBuilderTool.ToString("摘取糖果：", Idx));
#endif
        if (Idx >= 0 && Idx < SugarObjLst.Count)
        {
            Vector3 oldPos = SugarObjLst[Idx].transform.position;
            TweenPosition tp = TweenPosition.Begin(SugarObjLst[Idx].gameObject, 0.2f, new Vector3(-2.31f, -2.87f, 0.81f));
            tp.onFinished.Add(new EventDelegate(() =>
            {
                SugarObjLst[Idx].gameObject.SetActive(false);
                SugarObjLst[Idx].transform.position = oldPos;

                GameApp.SendMsg.GetOfflineCandy(1);
            }));
        }
    }
    #endregion
}
