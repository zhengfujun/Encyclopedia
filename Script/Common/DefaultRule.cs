using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 默认规则
/// </summary>
static public class DefaultRule
{
    #region _Formatting ID拼装
    /// 由主人账号和英雄ID获得英雄唯一标识
    /// </summary>
    static public ulong MakeHeroUID(int MasterAccountID, int HeroID)
    {
        return (ulong)(MasterAccountID * 1000 + HeroID);
    }
    /// 由主人账号和借用英雄ID获得英雄唯一标识
    /// </summary>
    static public ulong MakeBorrowHeroUID(int MasterAccountID, int BorrowHeroID)
    {
        return (ulong)(MasterAccountID * 1000000 + BorrowHeroID);
    }
    /// <summary>
    /// 由帐号ID、所在服ID、玩家序号获得玩家ID
    /// </summary>
    static public string MakePlayerID(int AccountID, int ServerID, int PlayerIndex)
    {
        return StringBuilderTool.ToInfoString(AccountID.ToString(), "-", ServerID.ToString(), "-", PlayerIndex.ToString());
    }
    /// <summary>
    /// Makes the manster I.
    /// </summary>
    /// <returns>The manster I.</returns>
    /// <param name="mapid">Mapid.</param>
    /// <param name="mapIndex">Map index.</param>
    static public ulong MakeMansterID(int mapid, int mapIndex)
    {
        return (ulong)(mapid * 100 + mapIndex);
    }
    static public int MakeSkillID(int actorid, int skillidx)
    {
        return actorid * 100 + skillidx;
    }

    static public int SkillID2SkillIndex(int skillid, int actorid)
    {
        return skillid - actorid * 100;
    }

    /// <summary>
    ///通过accountid和serverid 获得playerid
    /// </summary>
    /// <returns>The player I.</returns>
    /// <param name="accountid">Accountid.</param>
    /// <param name="serverid">Serverid.</param>
    static public ulong MakePlayerID(uint accountid, int serverid)
    {
        return ((ulong)serverid << 32) + (ulong)accountid;
    }

    /// <summary>
    /// 通过playerid获得accountid
    /// </summary>
    /// <returns>The identifier to account I.</returns>
    /// <param name="playerid">Playerid.</param>
    static public uint PlayerIDToAccountID(ulong playerid)
    {
        uint accountid = (uint)(playerid & 0x00000000ffffffff);
        return accountid;
    }

    /// <summary>
    /// 通过playerid获得serverid
    /// </summary>
    /// <returns>The identifier to server I.</returns>
    /// <param name="playerId">Player identifier.</param>
    static public uint PlayerIDToServerID(ulong playerId)
    {
        uint serverid = (uint)((playerId >> 32) & 0x00000000ffffffff);
        return serverid;
    }
    #endregion

    #region _Formula 属性计算公式
    static public int CaculateAttackDamage()
    {
		return 0;
    }
    #endregion
}
