using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 玩家数据
/// </summary> 
public class PlayerData
{
    public uint RoleID
    {
        get
        {
            return _RoleID;
        }
        set
        {
            _RoleID = value;
            if (ChangeRoleID != null)
                ChangeRoleID(_RoleID);
        }
    }
    private uint _RoleID = 0;
    public delegate void ChangeRoleIDHandler(uint roleID);
    public ChangeRoleIDHandler ChangeRoleID;

    public uint PetID
    {
        get
        {
            return _PetID;
        }
        set
        {
            _PetID = value;
        }
    }
    private uint _PetID = 1001;

    public string Name
    {
        get
        {
            return _Name;
        }
        set
        {
            _Name = value;
            if (ChangeName != null)
                ChangeName(_Name);
        }
    }
    private string _Name = "不取名可不行";
    public delegate void ChangeNameHandler(string newName);
    public ChangeNameHandler ChangeName;

    public int Score
    {
        get
        {
            return _Score;
        }
        set
        {
            _Score = value;
            if (ChangeScore != null)
                ChangeScore(_Score);
        }
    }
    private int _Score = 0;
    public delegate void ChangeScoreHandler(int newScore);
    public ChangeScoreHandler ChangeScore;

    /*public int MagicPower
    {
        get
        {
            return _MagicPower;
        }
        set
        {
            _MagicPower = value;
            if (ChangeMagicPower != null)
                ChangeMagicPower(_MagicPower);
        }
    }
    private int _MagicPower = 0;
    public delegate void ChangeMagicPowerHandler(int newMagicPower);
    public ChangeMagicPowerHandler ChangeMagicPower;*/

    public int GoldCoin
    {
        get
        {
            return _GoldCoin;
        }
        set
        {
            _GoldCoin = value;
            if (ChangeGoldCoin != null)
                ChangeGoldCoin(_GoldCoin);
        }
    }
    private int _GoldCoin = 0;
    public delegate void ChangeGoldCoinHandler(int newGoldCoin);
    public ChangeGoldCoinHandler ChangeGoldCoin;

    public int Diamond
    {
        get
        {
            return _Diamond;
        }
        set
        {
            _Diamond = value;
            if (ChangeDiamond != null)
                ChangeDiamond(_Diamond);
        }
    }
    private int _Diamond = 0;
    public delegate void ChangeDiamondHandler(int newDiamond);
    public ChangeDiamondHandler ChangeDiamond;

    public int GetItemCount(int ItemID)
    {
        int cnt = 0;
        _Item.TryGetValue(ItemID, out cnt);
        return cnt;
    }
    public void AddItem(int ItemID, int ItemCount)
    {
        if(_Item.ContainsKey(ItemID))
        {
            _Item[ItemID] += ItemCount;
        }
        else
        {
            _Item.Add(ItemID, ItemCount);
        }

        if (ChangeItem != null)
            ChangeItem();
    }
    public bool UseItem(int ItemID)
    {
        if (_Item.ContainsKey(ItemID))
        {
            _Item[ItemID] -= 1;
            if (_Item[ItemID] <= 0)
                _Item.Remove(ItemID);

            if (ChangeItem != null)
                ChangeItem();

            return true;
        }
        return false;
    }
    private Dictionary<int, int> _Item = new Dictionary<int, int>();
    public delegate void ChangeItemHandler();
    public ChangeItemHandler ChangeItem;

    public PlayerData(uint roleID, string name, /*int mp,*/ int gold, int diamond)
    {
        RoleID = roleID;
        Name = name;
        //MagicPower = mp;
        GoldCoin = gold;
        Diamond = diamond;
    }
}