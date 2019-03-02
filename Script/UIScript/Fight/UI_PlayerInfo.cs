using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerInfo : MonoBehaviour
{
    private PlayerData RecordPD = null;
    private common.PVE_Room_Player RecordRP = null;

    public UISprite Bg;
    public string MyselfBgSpriteName;
    public string OtherBgSpriteName;
    //public UISprite SignIcon;
    //public UILabel SignDes;

    private bool useBigHeadPortrait = false;
    public UISprite HeadPortrait;

    public UILabel Name;

    public UILabel Score;
    public UILabel AddScoreHint;

    //public UILabel MagicPower;
    //public UILabel AddMagicPowerHint;

    public UILabel GoldCoin;
    public UILabel AddGoldCoinHint;

    public UILabel Diamond;
    
    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void OnDestroy()
    {
        if (RecordPD != null)
        {
            if (HeadPortrait != null)
                RecordPD.ChangeRoleID -= UpdateUI_ChangeRoleID;

            if (Name != null)
                RecordPD.ChangeName -= UpdateUI_ChangeName;

            if (Score != null)
                RecordPD.ChangeScore -= UpdateUI_ChangeScore;

            //if (MagicPower != null)
            //    RecordPD.ChangeMagicPower -= UpdateUI_ChangeMagicPower;

            if (GoldCoin != null)
                RecordPD.ChangeGoldCoin -= UpdateUI_ChangeGoldCoin;

            if (Diamond != null)
                RecordPD.ChangeDiamond -= UpdateUI_ChangeDiamond;
        }
    }

    public void UseBigHeadPortrait()
    {
        useBigHeadPortrait = true;
    }

    public void Init(PlayerData pd)
    {
        if (pd == null)
            return;
        RecordPD = pd;

        if (HeadPortrait != null)
        {
            RoleConfig rc = null;
            if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue((int)RecordPD.RoleID, out rc))
            {
                HeadPortrait.spriteName = (useBigHeadPortrait ? rc.PortraitEx : rc.Portrait);
            }
            RecordPD.ChangeRoleID += UpdateUI_ChangeRoleID;
        }

        if (Name != null)
        {
            Name.text = RecordPD.Name;
            RecordPD.ChangeName += UpdateUI_ChangeName;
        }

        if (Score != null)
        {
            Score.text = RecordPD.Score.ToString();
            RecordPD.ChangeScore += UpdateUI_ChangeScore;
        }

        /*if (MagicPower != null)
        {
            MagicPower.text = RecordPD.MagicPower.ToString();
            RecordPD.ChangeMagicPower += UpdateUI_ChangeMagicPower;
        }*/

        if (GoldCoin != null)
        {
            GoldCoin.text = RecordPD.GoldCoin.ToString();
            RecordPD.ChangeGoldCoin += UpdateUI_ChangeGoldCoin;
        }

        if (Diamond != null)
        {
            Diamond.text = RecordPD.Diamond.ToString();
            RecordPD.ChangeDiamond += UpdateUI_ChangeDiamond;
        }
    }

    public void Init(common.PVE_Room_Player rp)
    {
        if (rp == null)
            return;
        RecordRP = rp;

        if (Bg != null)
        {
            if (DefaultRule.PlayerIDToAccountID(rp.id) == GameApp.AccountID)
            {
                Bg.spriteName = MyselfBgSpriteName;
            }
            else
            {
                Bg.spriteName = OtherBgSpriteName;
            }
        }

        if (HeadPortrait != null)
        {
            RecordRP.icon = Math.Max(RecordRP.icon, 1);
            RoleConfig rc = null;
            if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue((int)RecordRP.icon, out rc))
            {
                HeadPortrait.spriteName = (useBigHeadPortrait ? rc.PortraitEx : rc.Portrait);
            }
        }

        if (Name != null)
        {
            Name.text = RecordRP.name;
        }

        if (Score != null)
        {
            Score.text = RecordRP.score.ToString();
        }
    }

    public void SetScore(int score)
    {
        new Task(ShowScoreChange(score));
    }
    IEnumerator ShowScoreChange(int s)
    {
        if (GameApp.Instance.FightUI != null)
        {
            if (GameApp.Instance.FightUI.QandAUI != null)
            {
                while (GameApp.Instance.FightUI.QandAUI.gameObject.activeSelf)
                    yield return new WaitForEndOfFrame();
            }
        }

        GameApp.Instance.SoundInstance.PlaySe("AddScore");
        GameApp.Instance.SoundInstance.PlaySe("AddGoldCoin");

        UpdateUI_ChangeScore(s);
    }

    public void UpdateUI_ChangeRoleID(uint roleID)
    {
        RoleConfig rc = null;
        if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue((int)RecordPD.RoleID, out rc))
        {
            HeadPortrait.spriteName = (useBigHeadPortrait ? rc.PortraitEx : rc.Portrait);
        }
    }

    public void UpdateUI_ChangeName(string name)
    {
        Name.text = name;
    }

    public void UpdateUI_ChangeScore(int score)
    {
        if(!ShowAddScoreHint(score - int.Parse(Score.text)))
        {
            Score.text = score.ToString();
        }
    }
    private bool ShowAddScoreHint(int addValue)
    {
        if (AddScoreHint == null)
            return false;

        AddScoreHint.text = (addValue > 0 ? "+" : "") + addValue;
        StartCoroutine("_ShowAddScoreHint", addValue);

        return true;
    }
    IEnumerator _ShowAddScoreHint(int Increment)
    {
        AddScoreHint.transform.localScale = Vector3.zero;
        TweenScale.Begin(AddScoreHint.gameObject, 0.2f, Vector3.one);

        int i = 0;
        int cur = int.Parse(Score.text);
        if (Increment >= 0)
        {
            while (i <= Increment)
            {
                Score.text = (cur + i).ToString();
                i++;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (i >= Increment)
            {
                Score.text = (cur + i).ToString();
                i--;
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(0.2f);
        TweenScale.Begin(AddScoreHint.gameObject, 0.2f, Vector3.zero);
    }

    /*public void UpdateUI_ChangeMagicPower(int mp)
    {
        if (!ShowAddMagicPowerHint(mp - int.Parse(MagicPower.text)))
        {
            MagicPower.text = mp.ToString();
        }
    }
    private bool ShowAddMagicPowerHint(int addValue)
    {
        if (AddMagicPowerHint == null)
            return false;

        AddMagicPowerHint.text = (addValue > 0 ? "+" : "") + addValue;
        StartCoroutine("_ShowAddMagicPowerHint", addValue);

        return true;
    }
    IEnumerator _ShowAddMagicPowerHint(int Increment)
    {
        AddMagicPowerHint.transform.localScale = Vector3.zero;
        TweenScale.Begin(AddMagicPowerHint.gameObject, 0.2f, Vector3.one);

        int i = 0;
        int cur = int.Parse(MagicPower.text);
        if (Increment >= 0)
        {
            while (i <= Increment)
            {
                MagicPower.text = (cur + i).ToString();
                i++;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (i >= Increment)
            {
                MagicPower.text = (cur + i).ToString();
                i--;
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(0.5f);
        TweenScale.Begin(AddMagicPowerHint.gameObject, 0.2f, Vector3.zero);
    }*/

    public void UpdateUI_ChangeGoldCoin(int gc)
    {
        if(!ShowAddGoldCoinHint(gc - int.Parse(GoldCoin.text)))
        {
            GoldCoin.text = gc.ToString();
        }
    }
    private bool ShowAddGoldCoinHint(int addValue)
    {
        if (AddGoldCoinHint == null)
            return false;

        AddGoldCoinHint.text = (addValue > 0 ? "+" : "") + addValue;
        StartCoroutine("_ShowAddGoldCoinHint", addValue);

        return true;
    }
    IEnumerator _ShowAddGoldCoinHint(int Increment)
    {
        AddGoldCoinHint.transform.localScale = Vector3.zero;
        TweenScale.Begin(AddGoldCoinHint.gameObject, 0.2f, Vector3.one);

        int i = 0;
        int cur = int.Parse(GoldCoin.text);
        if (Increment >= 0)
        {
            while (i <= Increment)
            {
                GoldCoin.text = (cur + i).ToString();
                i++;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (i >= Increment)
            {
                GoldCoin.text = (cur + i).ToString();
                i--;
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(0.2f);
        TweenScale.Begin(AddGoldCoinHint.gameObject, 0.2f, Vector3.zero);
    }

    public void UpdateUI_ChangeDiamond(int diamond)
    {
        Diamond.text = diamond.ToString();
    }
}
