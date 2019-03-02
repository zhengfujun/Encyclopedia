using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using common;

public class MailItemInfo
{
    public uint ID;
    public string Name;
    public uint Time;
    public string TimeDes;
    public uint Residue;
    public bool IsReadAndGet;
    public string Details;
    public Dictionary<int, int> ItemLst = new Dictionary<int, int>();

    public MailItemInfo(PlayerMailItem pmi)
    {
        ID = pmi.m_id;
        Name = pmi.m_header;

        DateTime battleEndTime = MyTools.getTime((int)pmi.m_time);
        Time = (uint)(DateTime.Now.DayOfYear - battleEndTime.DayOfYear);
        Residue = (uint)(Mathf.Max(battleEndTime.DayOfYear + 30 - DateTime.Now.DayOfYear, 0));
        TimeDes = battleEndTime.ToString("yyyy-MM-dd HH:mm");

        IsReadAndGet = (pmi.m_state > 0);
        Details = pmi.m_content;
        for (int j = 0; j < pmi.m_appendix.Count; j++)
        {
            int key = (int)pmi.m_appendix[j].m_id;
            int value = (int)pmi.m_appendix[j].m_count;
            if (ItemLst.ContainsKey(key))
            {
                ItemLst[key] += value;
            }
            else
            {
                ItemLst.Add(key, value);
            }
        }
    }

    public bool IsHasItem()
    {
        return (ItemLst.Count > 0);
    }
}

public class UI_Mail : MonoBehaviour
{
    public UIAppearEffect AppearEffect;

    public GameObject NoMailHintRoot;
    public GameObject MailLstRoot;
    public GameObject DetailsRoot;

    public GameObject LstGrid;
    public GameObject MailItemUnitPrefab;
    public UILabel MailCnt;

    public UILabel Title;
    public UILabel ResidueTime;
    public UILabel Details;
    public List<UI_AwardItem> AwardItemLst;

    public GameObject GetAllBtnObj;
    public UIButton DeleteBtn;
    public UIButton GetBtn;

    [HideInInspector]
    public UI_MailItemUnit CurShowMIU = null;

    static public void AddMail(PlayerMailItem pmi)
    {
        if (!GameApp.Instance.MailItemsDic.ContainsKey(pmi.m_id))
        {
            GameApp.Instance.MailItemsDic.Add(pmi.m_id, new MailItemInfo(pmi));
        }
        else
        {
            Debug.Log("重复的邮件：" + pmi.m_id + " " + pmi.m_header);
        }
    }
    static public void DelMail(uint mailID)
    {
        if (GameApp.Instance.MailItemsDic.ContainsKey(mailID))
        {
            GameApp.Instance.MailItemsDic.Remove(mailID);
        }
        else
        {
            Debug.Log("邮件列表中没有ID为" + mailID + "的邮件");
        }
    }

    void Awake()
    {

    }

    void OnDestroy()
    {

    }

    void Start()
    {
        ShowMailDetail(null);
    }

    public void Show(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject);

            GameApp.Instance.UICurrency.Show(true);

            //AddMail(1531209933, "新手奖励","欢迎加入魔卡百科的世界，为了让你更快的学习知识，我们为你准备了一些补给。");
            //AddMail(1531209934, "一封假邮件","一封本不该出现在这里的邮件，仅供测试。");

            if (LstGrid.transform.childCount == 0)
            {
                int i = 0;
                foreach (KeyValuePair<uint, MailItemInfo> pair in GameApp.Instance.MailItemsDic)
                {
                    GameObject newUnit = NGUITools.AddChild(LstGrid, MailItemUnitPrefab);
                    newUnit.SetActive(true);
                    newUnit.name = "MailItemUnit_" + (i++);

                    newUnit.GetComponent<UI_MailItemUnit>().Set(pair.Value);
                }

                LstGrid.transform.GetComponent<UIGrid>().repositionNow = true;
                LstGrid.transform.parent.GetComponent<UIScrollView>().ResetPosition();
            }

            int MailCount = LstGrid.transform.childCount;
            MailCnt.text = StringBuilderTool.ToString("邮件数：" + MailCount);
            if (MailCount == 0)
            {
                NoMailHintRoot.SetActive(true);
                MailLstRoot.SetActive(false);
                DetailsRoot.SetActive(false);
            }
            else
            {
                NoMailHintRoot.SetActive(false);
                MailLstRoot.SetActive(true);
                DetailsRoot.SetActive(true);
            }

            InvokeRepeating("UpdateMailRedPoint", 0, 1f);
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

    void UpdateMailRedPoint()
    {
        bool hasNoRead = false;
        for (int i = 0; i < LstGrid.transform.childCount; i++)
        {
            Transform child = LstGrid.transform.GetChild(i);
            UI_MailItemUnit miu = child.GetComponent<UI_MailItemUnit>();
            if (!miu.CurMII.IsReadAndGet)
            {
                hasNoRead = true;
                break;
            }
        }
        //GameApp.Instance.UIHomePage.MailRedPoint.SetActive(hasNoRead);

        GetAllBtnObj.SetActive(hasNoRead);
    }


    /// <summary> 关闭界面 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        Show(false);
    }

    public void ShowMailDetail(UI_MailItemUnit miu)
    {
        if (miu == null)
        {
            CurShowMIU = null;
            Title.text = "";
            ResidueTime.text = "";
            Details.text = "";
            for (int k = 0; k < AwardItemLst.Count; k++)
            {
                AwardItemLst[k].gameObject.SetActive(false);
            }
            DeleteBtn.gameObject.SetActive(false);
            GetBtn.gameObject.SetActive(false);
            return;
        }
        
        if (CurShowMIU != null)
            CurShowMIU.Select(false);

        CurShowMIU = miu;

        Title.text = CurShowMIU.CurMII.Name;
        ResidueTime.text = StringBuilderTool.ToString(CurShowMIU.CurMII.Residue, "天后自动删除");
        Details.text = CurShowMIU.CurMII.Details;

        for (int k = 0; k < AwardItemLst.Count; k++)
        {
            AwardItemLst[k].ShowGetSign(false);
            AwardItemLst[k].gameObject.SetActive(false);
        }

        if (CurShowMIU.CurMII.ItemLst != null)
        {
            int i = 0;
            foreach (KeyValuePair<int, int> pair in CurShowMIU.CurMII.ItemLst)
            {
                AwardItemLst[i].ShowGetSign(CurShowMIU.CurMII.IsReadAndGet);
                AwardItemLst[i].SetItemData(pair.Key, pair.Value);
                AwardItemLst[i].gameObject.SetActive(true);
                i++;
            }
        }

        bool ShowGetBtn = (CurShowMIU.CurMII.IsHasItem() && !CurShowMIU.CurMII.IsReadAndGet);
        GetBtn.gameObject.SetActive(ShowGetBtn);
        GetBtn.isEnabled = ShowGetBtn;

        DeleteBtn.gameObject.SetActive(true);
    }

    /// <summary> 一键领取按钮 </summary>
    public void OnClick_GetAll()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【一键领取】");

        isOpenAllMail = true;
        for (int i = 0; i < LstGrid.transform.childCount; i++)
        {
            Transform child = LstGrid.transform.GetChild(i);
            UI_MailItemUnit miu = child.GetComponent<UI_MailItemUnit>();
            
            GameApp.SendMsg.OpenMail(miu.CurMII.ID);
        }
    }
    /// <summary> 领取按钮 </summary>
    public void OnClick_Get()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【领取】");

        isOpenAllMail = false;

        GameApp.SendMsg.OpenMail(CurShowMIU.CurMII.ID);
        //OpenMailRes();
    }

    public bool isOpenAllMail = false;
    public void OpenMailRes()
    {
        if (isOpenAllMail)
        {
            Dictionary<int, int> AllItemLst = new Dictionary<int, int>();
            for (int i = 0; i < LstGrid.transform.childCount; i++)
            {
                Transform child = LstGrid.transform.GetChild(i);
                UI_MailItemUnit miu = child.GetComponent<UI_MailItemUnit>();
                miu.ReadAndGet();
                /*foreach (KeyValuePair<int, int> pair in miu.CurMII.ItemLst)
                {
                    GameApp.SendMsg.GMOrder("AddItem " + pair.Key + " " + pair.Value);
                }*/
                foreach (KeyValuePair<int, int> pair in miu.CurMII.ItemLst)
                {
                    if (AllItemLst.ContainsKey(pair.Key))
                    {
                        AllItemLst[pair.Key] += pair.Value;
                    }
                    else
                    {
                        AllItemLst.Add(pair.Key, pair.Value);
                    }
                }
            }

            for (int k = 0; k < AwardItemLst.Count; k++)
            {
                if (AwardItemLst[k].gameObject.activeSelf)
                {
                    AwardItemLst[k].ShowGetSign(true);
                }
            }
            GetBtn.isEnabled = false;

            GameApp.Instance.GetItemsDlg.OpenGetItemsBox(AllItemLst);
        }
        else
        {
            CurShowMIU.ReadAndGet();
            /*foreach (KeyValuePair<int, int> pair in CurShowMIU.CurMII.ItemLst)
            {
                GameApp.SendMsg.GMOrder("AddItem " + pair.Key + " " + pair.Value);
            }*/
            for (int k = 0; k < AwardItemLst.Count; k++)
            {
                if (AwardItemLst[k].gameObject.activeSelf)
                {
                    AwardItemLst[k].ShowGetSign(true);
                }
            }
            GetBtn.isEnabled = false;

            GameApp.Instance.GetItemsDlg.OpenGetItemsBox(CurShowMIU.CurMII.ItemLst);
        }
    }

    /// <summary> 删除按钮 </summary>
    public void OnClick_Delete()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【删除】");

        if (!CurShowMIU.CurMII.IsReadAndGet)//有奖励道具 且 未领取
        {
            GameApp.Instance.CommonMsgDlg.OpenMsgBox("亲！奖励物品还未领取哦！");
            return;
        }

        GameApp.Instance.CommonMsgDlg.OpenMsgBox("亲！确定要删除此邮件吗？", (del) =>
            {
                if(del)
                {
                    GameApp.SendMsg.DeleteMail(CurShowMIU.CurMII.ID);
                    //DeleteMailRes();
                }
            });
    }
    public void DeleteMailRes()
    {
        DelMail(CurShowMIU.CurMII.ID);

        DestroyImmediate(CurShowMIU.gameObject);
        ShowMailDetail(null);

        LstGrid.transform.GetComponent<UIGrid>().enabled = true;
        //LstGrid.transform.parent.GetComponent<UIScrollView>().ResetPosition();

        int MailCount = LstGrid.transform.childCount;
        MailCnt.text = StringBuilderTool.ToString("邮件数：" + MailCount);
        if (MailCount == 0)
        {
            NoMailHintRoot.SetActive(true);
            MailLstRoot.SetActive(false);
            DetailsRoot.SetActive(false);
        }
    }
}
