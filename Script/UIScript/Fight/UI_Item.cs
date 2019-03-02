using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Item : MonoBehaviour
{
    private PlayerData RecordPD = null;

    private ItemConfig ItemCfg = null;

    private UIButton Btn;

    public UISprite Icon;
    public UILabel Name;

    private int ItemCount;
    public UILabel Cnt;

    void Start()
    {
        Btn = gameObject.GetComponent<UIButton>();
    }

    void OnDestroy()
    {
        if (RecordPD != null)
        {
            RecordPD.ChangeItem -= UpdateUI_ChangeItem;
        }
    }

    /*void Update()
    {

    }*/

    public void Set(int ItemID, PlayerData pd = null)
    {
        if (CsvConfigTables.Instance.ItemCsvDic.TryGetValue(ItemID, out ItemCfg))
        {
            Icon.spriteName = ItemCfg.Icon;
            Name.text = ItemCfg.Name;

            RecordPD = pd;
            if (RecordPD != null)
            {
                RecordPD.ChangeItem += UpdateUI_ChangeItem;

                if (Cnt != null)
                {
                    ItemCount = RecordPD.GetItemCount(ItemID);
                    Cnt.text = "" + ItemCount;
                }
            }
        }
    }

    public void UpdateUI_ChangeItem()
    {
        ItemCount = GameApp.Instance.MainPlayerData.GetItemCount(ItemCfg.ItemID);
        Cnt.text = "" + ItemCount;
    }

    /// <summary> 点击使用道具 </summary>
    public void OnClick_Use()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【使用道具】" + ItemCfg.Name);

        if (GameApp.Instance.MainPlayerData.UseItem(ItemCfg.ItemID))
        {
            switch (ItemCfg.ItemID)
            {
                case 101:
                    GameApp.Instance.FightUI.QandAUI.ExcludeOneErrorAnswer();
                    break;
                case 102:
                    GameApp.Instance.FightUI.QandAUI.SetCourageState();
                    break;
                case 103:
                    GameApp.Instance.FightUI.QandAUI.SetNewQuestion();
                    break;
            }

            GameApp.Instance.SoundInstance.StopAllSe();
            GameApp.Instance.SoundInstance.PlayVoice(ItemCfg.Voice);

            SetIsEnabled(false);

            GameApp.Instance.FightUI.QandAUI.HideUseItemHint();
        }
        else
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("没有该道具，不能使用！");
        }
    }

    public void SetIsEnabled(bool isEnabled)
    {
        if (Btn != null)
            Btn.isEnabled = isEnabled;
    }

    public bool ItemEnableUse()
    {
        return ItemCount > 0;
    }
}
