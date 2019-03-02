using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using common;

/// <summary>
/// 服务器的玩家数据
/// </summary>
static public class SerPlayerData
{
    /// <summary> 获取账号ID </summary>
    static public uint GetAccountID()
    {
        if (GameApp.Instance.PlayerData != null)
        {
            return GameApp.Instance.PlayerData.m_account_id;
        }
        return 0;
    }

    /// <summary> 获取玩家昵称 </summary>
    static public string GetName()
    {
        if (GameApp.Instance.PlayerData != null)
        {
            return GameApp.Instance.PlayerData.m_player_name;
        }
        return "吃瓜群众";
    }

    /// <summary> 获取换装ID </summary>
    static public uint GetAvatarID()
    {
        if (GameApp.Instance.PlayerData != null)
        {
            return Math.Max(GameApp.Instance.PlayerData.m_player_base.m_avatar,1);
        }

        if (GameApp.Instance.UILogin != null)
            return 0;

        return 1;
    }

    /// <summary> 是否为GM账号 </summary>
    static public bool IsGM()
    {
        if (GameApp.Instance.PlayerData != null)
        {
            return GameApp.Instance.PlayerData.m_gm == 1 ? true : false;
        }
        return true;
    }

    /// <summary> 获取已存的线下活动比赛配置信息 </summary>
    static public string GetOfflineChallengeSchemeCfg()
    {
        if (GameApp.Instance.PlayerData != null)
        {
            return GameApp.Instance.PlayerData.m_player_contest.setup;
        }
        return "Offline";
    }

    /// <summary> 获取玩家等级 </summary>
    static public uint GetLv()
    {
        if (GameApp.Instance.PlayerData != null)
        {
            return Math.Max(GameApp.Instance.PlayerData.m_player_base.m_player_lv, 1);
        }
        return 1;
    }

    /// <summary> 获取玩家当前等级经验 </summary>
    static public uint GetCurLvExp()
    {
        if (GameApp.Instance.PlayerData != null)
        {
            return GameApp.Instance.PlayerData.m_player_base.m_player_exp;
        }
        return 0;
    }

    /// <summary> 获取某道具持有数量 </summary>
    static public uint GetItemCount(int ItemID)
    {
        if (GameApp.Instance.PlayerData != null)
        {
            PlayerBag pb = GameApp.Instance.PlayerData.m_player_bag;
            uint cnt = 0;
            for (int i = 0; i < pb.m_items.Count; i++)
            {
                if (pb.m_items[i].m_item_id == ItemID)
                {
                    cnt += pb.m_items[i].m_item_count;
                }
            }
            return cnt;
        }
        else
        {
            if (CsvConfigTables.Instance.MagicCardCsvDic.ContainsKey(ItemID))
            {
                return (uint)GameApp.Instance.CardHoldCountLst[ItemID];
            }
        }

        return 0;
    }

    /// <summary> 获取所有卡牌持有数量 </summary>
    static public uint GetCardCount()
    {
        uint cnt = 0;
        if (GameApp.Instance.PlayerData != null)
        {
            PlayerBag pb = GameApp.Instance.PlayerData.m_player_bag;
            for (int i = 0; i < pb.m_items.Count; i++)
            {
                MagicCardConfig CardCfg = null;
                CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue((int)pb.m_items[i].m_item_id, out CardCfg);
                if (CardCfg != null)
                {
                    cnt += pb.m_items[i].m_item_count;
                }
            }
        }

        return cnt;
    }

    /// <summary> 获取某主题卡牌的持有数量 </summary>
    static public uint GetCardCounLimitThemet(int ThemeID)
    {
        uint cnt = 0;
        if (GameApp.Instance.PlayerData != null)
        {
            PlayerBag pb = GameApp.Instance.PlayerData.m_player_bag;
            for (int i = 0; i < pb.m_items.Count; i++)
            {
                MagicCardConfig CardCfg = null;
                CsvConfigTables.Instance.MagicCardCsvDic.TryGetValue((int)pb.m_items[i].m_item_id, out CardCfg);
                if (CardCfg != null)
                {
                    if (CardCfg.ThemeID == ThemeID)
                        cnt += pb.m_items[i].m_item_count;
                }
            }
        }

        return cnt;
    }

    /// <summary> 服务器的道具流水ID对应的道具配置表ID </summary>
    static public int SerIDToItemID(ulong ID)
    {
        if (GameApp.Instance.PlayerData != null)
        {
            PlayerBag pb = GameApp.Instance.PlayerData.m_player_bag;
            for (int i = 0; i < pb.m_items.Count; i++)
            {
                if (pb.m_items[i].m_id == ID)
                {
                    return (int)pb.m_items[i].m_item_id;
                }
            }
        }

        return 0;
    }

    /// <summary> 获取主线关卡进度 </summary>
    static public int GetMainStageProgress()
    {
        if (GameApp.Instance.PlayerData != null)
        {
            int cnt = GameApp.Instance.PlayerData.m_player_pve.m_pves.Count;
            if (cnt > 0)
            {
                return (int)GameApp.Instance.PlayerData.m_player_pve.m_pves[cnt-1].m_id;
            }
            return 0;            
        }

        return 999;
    }
}
