using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common
{
	public class Account : IMessage
	{
		private ProtoMemberUInt32 _m_account_id;	// 账号id
		private ProtoMemberString _m_account_name;	// 帐号名称
		private ProtoMemberString _m_account_key;	// 帐号密码

		public Account()
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_account_name = new ProtoMemberString(2, false);
			_m_account_key = new ProtoMemberString(3, false);
		}

		public Account(uint __m_account_id, string __m_account_name, string __m_account_key)
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_account_id.member_value = __m_account_id;
			_m_account_name = new ProtoMemberString(2, false);
			_m_account_name.member_value = __m_account_name;
			_m_account_key = new ProtoMemberString(3, false);
			_m_account_key.member_value = __m_account_key;
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public string m_account_name
		{
			get{ return _m_account_name.member_value; }
			set{ _m_account_name.member_value = value; }
		}
		public bool has_m_account_name
		{
			get{ return _m_account_name.has_value; }
		}

		public string m_account_key
		{
			get{ return _m_account_key.member_value; }
			set{ _m_account_key.member_value = value; }
		}
		public bool has_m_account_key
		{
			get{ return _m_account_key.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_account_name.Serialize(_m_account_name.member_value, ref out_stream);

			count += _m_account_key.Serialize(_m_account_key.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			string temp_m_account_name = "";
			one_count = _m_account_name.ParseFrom(ref temp_m_account_name, ref int_stream);
			if (0 < one_count)
			{
					_m_account_name.member_value = temp_m_account_name;
					count = count + one_count;
			}

			string temp_m_account_key = "";
			one_count = _m_account_key.ParseFrom(ref temp_m_account_key, ref int_stream);
			if (0 < one_count)
			{
					_m_account_key.member_value = temp_m_account_key;
					count = count + one_count;
			}

			return count;
		}
	}

	public enum AccountRes
	{
		[System.ComponentModel.Description("账号不存在")]
		AccountRes_Login_AccountError = 0x0001,			// 账号不存在
		[System.ComponentModel.Description("密码不匹配")]
		AccountRes_Login_KeyError = 0x0002,				// 密码不匹配
		[System.ComponentModel.Description("账号密码验证成功")]
		AccountRes_Login_Success = 0x0003,				// 账号密码验证成功
		[System.ComponentModel.Description("账号注册失败（用户名重复）")]
		AccountRes_Reg_AccountError = 0x0101,			// 账号注册失败（用户名重复）
		[System.ComponentModel.Description("账号注册成功")]
		AccountRes_Reg_Success = 0x0102,				// 账号注册成功
		[System.ComponentModel.Description("创建角色时，验证该账号下未激活对应玩具")]
		AccountRes_CreatePlayer_ToyNotExists = 0x0201,	// 创建角色时，验证该账号下未激活对应玩具
		[System.ComponentModel.Description("创建角色时，验证该账号下已激活对应玩具")]
		AccountRes_CreatePlayer_ToyExists = 0x0202,		// 创建角色时，验证该账号下已激活对应玩具
		[System.ComponentModel.Description("玩具激活失败，玩具唯一识别码错误")]
		AccountRes_ToyActivate_GuidError = 0x0301,		// 玩具激活失败，玩具唯一识别码错误
		[System.ComponentModel.Description("玩具激活失败，玩具类别错误")]
		AccountRes_ToyActivate_TypeError = 0x0302,		// 玩具激活失败，玩具类别错误
		[System.ComponentModel.Description("玩具已经被其他账号绑定")]
		AccountRes_ToyActivate_OthersBound = 0x0303,	// 玩具已经被其他账号绑定
		[System.ComponentModel.Description("玩具已经被本账号绑定")]
		AccountRes_ToyActivate_SelfBound = 0x0304,		// 玩具已经被本账号绑定
		[System.ComponentModel.Description("玩具激活失败，数据库绑定操作失败")]
		AccountRes_ToyActivate_BindError = 0x0305,		// 玩具激活失败，数据库绑定操作失败
		[System.ComponentModel.Description("玩具激活成功")]
		AccountRes_ToyActivate_Success = 0x0306,		// 玩具激活成功
		[System.ComponentModel.Description("检查玩具失败，玩具唯一识别码错误")]
		AccountRes_ToyCheck_GuidError = 0x0401,			// 检查玩具失败，玩具唯一识别码错误
		[System.ComponentModel.Description("检查玩具失败，玩具类别错误")]
		AccountRes_ToyCheck_TypeError = 0x0402,			// 检查玩具失败，玩具类别错误
		[System.ComponentModel.Description("玩具已经被其他账号绑定")]
		AccountRes_ToyCheck_OthersBound = 0x0403,		// 玩具已经被其他账号绑定
		[System.ComponentModel.Description("玩具已经被本账号绑定")]
		AccountRes_ToyCheck_SelfBound = 0x0404,			// 玩具已经被本账号绑定
		[System.ComponentModel.Description("检查玩具成功（未被绑定过，是一个新玩具）")]
		AccountRes_ToyCheck_Success = 0x0405,			// 检查玩具成功（未被绑定过，是一个新玩具）
	}

	public enum ActionType
	{
		[System.ComponentModel.Description("旅行")]
		ActionType_Travel = 1,						//旅行                
		[System.ComponentModel.Description("玩耍")]
		ActionType_Playing = 2,                       //玩耍
	}

	public class BossBattlePlayerInfo : IMessage
	{
		private ProtoMemberUInt64 _player_id;	// 玩家id
		private ProtoMemberUInt64 _score;	// 玩家积分
		private ProtoMemberString _player_name;	// 玩家名字
		private ProtoMemberUInt32 _player_level;	// 玩家等级
		private ProtoMemberUInt32 _player_hero;	// 玩家英雄
		private ProtoMemberUInt32 _player_equip;	// 玩家武器

		public BossBattlePlayerInfo()
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_score = new ProtoMemberUInt64(2, false);
			_player_name = new ProtoMemberString(3, false);
			_player_level = new ProtoMemberUInt32(4, false);
			_player_hero = new ProtoMemberUInt32(5, false);
			_player_equip = new ProtoMemberUInt32(6, false);
		}

		public BossBattlePlayerInfo(ulong __player_id, ulong __score, string __player_name, uint __player_level, uint __player_hero, uint __player_equip)
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_player_id.member_value = __player_id;
			_score = new ProtoMemberUInt64(2, false);
			_score.member_value = __score;
			_player_name = new ProtoMemberString(3, false);
			_player_name.member_value = __player_name;
			_player_level = new ProtoMemberUInt32(4, false);
			_player_level.member_value = __player_level;
			_player_hero = new ProtoMemberUInt32(5, false);
			_player_hero.member_value = __player_hero;
			_player_equip = new ProtoMemberUInt32(6, false);
			_player_equip.member_value = __player_equip;
		}

		public ulong player_id
		{
			get{ return _player_id.member_value; }
			set{ _player_id.member_value = value; }
		}
		public bool has_player_id
		{
			get{ return _player_id.has_value; }
		}

		public ulong score
		{
			get{ return _score.member_value; }
			set{ _score.member_value = value; }
		}
		public bool has_score
		{
			get{ return _score.has_value; }
		}

		public string player_name
		{
			get{ return _player_name.member_value; }
			set{ _player_name.member_value = value; }
		}
		public bool has_player_name
		{
			get{ return _player_name.has_value; }
		}

		public uint player_level
		{
			get{ return _player_level.member_value; }
			set{ _player_level.member_value = value; }
		}
		public bool has_player_level
		{
			get{ return _player_level.has_value; }
		}

		public uint player_hero
		{
			get{ return _player_hero.member_value; }
			set{ _player_hero.member_value = value; }
		}
		public bool has_player_hero
		{
			get{ return _player_hero.has_value; }
		}

		public uint player_equip
		{
			get{ return _player_equip.member_value; }
			set{ _player_equip.member_value = value; }
		}
		public bool has_player_equip
		{
			get{ return _player_equip.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _player_id.Serialize(_player_id.member_value, ref out_stream);

			count += _score.Serialize(_score.member_value, ref out_stream);

			count += _player_name.Serialize(_player_name.member_value, ref out_stream);

			count += _player_level.Serialize(_player_level.member_value, ref out_stream);

			count += _player_hero.Serialize(_player_hero.member_value, ref out_stream);

			count += _player_equip.Serialize(_player_equip.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_player_id = 0;
			one_count = _player_id.ParseFrom(ref temp_player_id, ref int_stream);
			if (0 < one_count)
			{
					_player_id.member_value = temp_player_id;
					count = count + one_count;
			}

			ulong temp_score = 0;
			one_count = _score.ParseFrom(ref temp_score, ref int_stream);
			if (0 < one_count)
			{
					_score.member_value = temp_score;
					count = count + one_count;
			}

			string temp_player_name = "";
			one_count = _player_name.ParseFrom(ref temp_player_name, ref int_stream);
			if (0 < one_count)
			{
					_player_name.member_value = temp_player_name;
					count = count + one_count;
			}

			uint temp_player_level = 0;
			one_count = _player_level.ParseFrom(ref temp_player_level, ref int_stream);
			if (0 < one_count)
			{
					_player_level.member_value = temp_player_level;
					count = count + one_count;
			}

			uint temp_player_hero = 0;
			one_count = _player_hero.ParseFrom(ref temp_player_hero, ref int_stream);
			if (0 < one_count)
			{
					_player_hero.member_value = temp_player_hero;
					count = count + one_count;
			}

			uint temp_player_equip = 0;
			one_count = _player_equip.ParseFrom(ref temp_player_equip, ref int_stream);
			if (0 < one_count)
			{
					_player_equip.member_value = temp_player_equip;
					count = count + one_count;
			}

			return count;
		}
	}

	public enum ComposeType
	{
		[System.ComponentModel.Description("道具合成")]
		ComposeType_Item = 1,                               //道具合成
		[System.ComponentModel.Description("货币合成")]
		ComposeType_Coin = 2,                                //货币合成
	}

	public class ContestCoupleInfo : IMessage
	{
		private ProtoMemberEmbeddedList<ContestPlayerInfo> _players;	// 玩家
		private ProtoMemberUInt32List _record;	// 进度
		private ProtoMemberUInt32 _contest_id;	// 赛事id
		private ProtoMemberUInt64 _couple_id;	//coupleid

		public ContestCoupleInfo()
		{
			_players = new ProtoMemberEmbeddedList<ContestPlayerInfo>(1, false);
			_record = new ProtoMemberUInt32List(2, false);
			_contest_id = new ProtoMemberUInt32(3, false);
			_couple_id = new ProtoMemberUInt64(4, false);
		}

		public ContestCoupleInfo(uint __contest_id, ulong __couple_id)
		{
			_players = new ProtoMemberEmbeddedList<ContestPlayerInfo>(1, false);
			_record = new ProtoMemberUInt32List(2, false);
			_contest_id = new ProtoMemberUInt32(3, false);
			_contest_id.member_value = __contest_id;
			_couple_id = new ProtoMemberUInt64(4, false);
			_couple_id.member_value = __couple_id;
		}

		public System.Collections.Generic.List<ContestPlayerInfo> players
		{
			get{ return _players.member_value; }
		}
		public bool has_players
		{
			get{ return _players.has_value; }
		}

		public System.Collections.Generic.List<uint> record
		{
			get{ return _record.member_value; }
		}
		public bool has_record
		{
			get{ return _record.has_value; }
		}

		public uint contest_id
		{
			get{ return _contest_id.member_value; }
			set{ _contest_id.member_value = value; }
		}
		public bool has_contest_id
		{
			get{ return _contest_id.has_value; }
		}

		public ulong couple_id
		{
			get{ return _couple_id.member_value; }
			set{ _couple_id.member_value = value; }
		}
		public bool has_couple_id
		{
			get{ return _couple_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			foreach(ContestPlayerInfo one_member_value in _players.member_value)
			{
				count += _players.Serialize(one_member_value, ref out_stream);
			}

			foreach(uint one_member_value in _record.member_value)
			{
				count += _record.Serialize(one_member_value, ref out_stream);
			}

			count += _contest_id.Serialize(_contest_id.member_value, ref out_stream);

			count += _couple_id.Serialize(_couple_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			while (true)
			{
				ContestPlayerInfo one_member_value = new ContestPlayerInfo();
				one_count = _players.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_players.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			while (true)
			{
				uint one_member_value = 0;
				one_count = _record.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_record.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			uint temp_contest_id = 0;
			one_count = _contest_id.ParseFrom(ref temp_contest_id, ref int_stream);
			if (0 < one_count)
			{
					_contest_id.member_value = temp_contest_id;
					count = count + one_count;
			}

			ulong temp_couple_id = 0;
			one_count = _couple_id.ParseFrom(ref temp_couple_id, ref int_stream);
			if (0 < one_count)
			{
					_couple_id.member_value = temp_couple_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class ContestPlayerInfo : IMessage
	{
		private ProtoMemberUInt64 _player_id;	// 玩家id
		private ProtoMemberInt32 _score;	// 玩家积分
		private ProtoMemberString _player_name;	// 玩家名字
		private ProtoMemberUInt32 _player_level;	// 玩家等级
		private ProtoMemberUInt32 _player_hero;	// 玩家英雄
		private ProtoMemberUInt32 _player_equip;	// 玩家武器

		public ContestPlayerInfo()
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_score = new ProtoMemberInt32(2, false);
			_player_name = new ProtoMemberString(3, false);
			_player_level = new ProtoMemberUInt32(4, false);
			_player_hero = new ProtoMemberUInt32(5, false);
			_player_equip = new ProtoMemberUInt32(6, false);
		}

		public ContestPlayerInfo(ulong __player_id, int __score, string __player_name, uint __player_level, uint __player_hero, uint __player_equip)
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_player_id.member_value = __player_id;
			_score = new ProtoMemberInt32(2, false);
			_score.member_value = __score;
			_player_name = new ProtoMemberString(3, false);
			_player_name.member_value = __player_name;
			_player_level = new ProtoMemberUInt32(4, false);
			_player_level.member_value = __player_level;
			_player_hero = new ProtoMemberUInt32(5, false);
			_player_hero.member_value = __player_hero;
			_player_equip = new ProtoMemberUInt32(6, false);
			_player_equip.member_value = __player_equip;
		}

		public ulong player_id
		{
			get{ return _player_id.member_value; }
			set{ _player_id.member_value = value; }
		}
		public bool has_player_id
		{
			get{ return _player_id.has_value; }
		}

		public int score
		{
			get{ return _score.member_value; }
			set{ _score.member_value = value; }
		}
		public bool has_score
		{
			get{ return _score.has_value; }
		}

		public string player_name
		{
			get{ return _player_name.member_value; }
			set{ _player_name.member_value = value; }
		}
		public bool has_player_name
		{
			get{ return _player_name.has_value; }
		}

		public uint player_level
		{
			get{ return _player_level.member_value; }
			set{ _player_level.member_value = value; }
		}
		public bool has_player_level
		{
			get{ return _player_level.has_value; }
		}

		public uint player_hero
		{
			get{ return _player_hero.member_value; }
			set{ _player_hero.member_value = value; }
		}
		public bool has_player_hero
		{
			get{ return _player_hero.has_value; }
		}

		public uint player_equip
		{
			get{ return _player_equip.member_value; }
			set{ _player_equip.member_value = value; }
		}
		public bool has_player_equip
		{
			get{ return _player_equip.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _player_id.Serialize(_player_id.member_value, ref out_stream);

			count += _score.Serialize(_score.member_value, ref out_stream);

			count += _player_name.Serialize(_player_name.member_value, ref out_stream);

			count += _player_level.Serialize(_player_level.member_value, ref out_stream);

			count += _player_hero.Serialize(_player_hero.member_value, ref out_stream);

			count += _player_equip.Serialize(_player_equip.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_player_id = 0;
			one_count = _player_id.ParseFrom(ref temp_player_id, ref int_stream);
			if (0 < one_count)
			{
					_player_id.member_value = temp_player_id;
					count = count + one_count;
			}

			int temp_score = 0;
			one_count = _score.ParseFrom(ref temp_score, ref int_stream);
			if (0 < one_count)
			{
					_score.member_value = temp_score;
					count = count + one_count;
			}

			string temp_player_name = "";
			one_count = _player_name.ParseFrom(ref temp_player_name, ref int_stream);
			if (0 < one_count)
			{
					_player_name.member_value = temp_player_name;
					count = count + one_count;
			}

			uint temp_player_level = 0;
			one_count = _player_level.ParseFrom(ref temp_player_level, ref int_stream);
			if (0 < one_count)
			{
					_player_level.member_value = temp_player_level;
					count = count + one_count;
			}

			uint temp_player_hero = 0;
			one_count = _player_hero.ParseFrom(ref temp_player_hero, ref int_stream);
			if (0 < one_count)
			{
					_player_hero.member_value = temp_player_hero;
					count = count + one_count;
			}

			uint temp_player_equip = 0;
			one_count = _player_equip.ParseFrom(ref temp_player_equip, ref int_stream);
			if (0 < one_count)
			{
					_player_equip.member_value = temp_player_equip;
					count = count + one_count;
			}

			return count;
		}
	}

	public enum DropItemType
	{
		[System.ComponentModel.Description("pve掉落")]
		DropItemType_PVE = 1,                               //pve掉落
		[System.ComponentModel.Description("宝箱掉落")]
		DropItemType_Box = 2,                               //宝箱掉落
		[System.ComponentModel.Description("常规获得")]
		DropItemType_Common = 3,                            //常规获得
		[System.ComponentModel.Description("扭蛋获得")]
		DropItemType_Gacha = 4,                            //扭蛋获得
		[System.ComponentModel.Description("合成获得")]
		DropItemType_Compose = 5,                            //合成获得
		[System.ComponentModel.Description("答题正确")]
		DropItemType_AnswerRight = 6,                        //答题正确
		[System.ComponentModel.Description("购买")]
		DropItemType_Buy = 7,								 //购买
		[System.ComponentModel.Description("获得糖果")]
		DropItemType_GetCandy = 8,                        //获得糖果
		[System.ComponentModel.Description("旅行礼物")]
		DropItemType_GetGift = 9,								 //旅行礼物
		[System.ComponentModel.Description("充值获得")]
		DropItemType_Recharge = 10,                            //充值获得
	}

	public enum GMErrorCode
	{
		[System.ComponentModel.Description("没有错误")]
		GMErrorCode_Success					= 1,					//没有错误
		[System.ComponentModel.Description("命令传入参数错误")]
		GMErrorCode_Param_Error 			= 2,					//命令传入参数错误
		[System.ComponentModel.Description("解包错误")]
		GMErrorCode_ParsePacket_Error		= 3,					//解包错误
		[System.ComponentModel.Description("数据库执行sql错误")]
		GMErrorCode_DbExec_Error		 	= 4,					//数据库执行sql错误
		[System.ComponentModel.Description("找不到玩家")]
		GMErrorCode_PlayerNotExists_Error	= 5,					//找不到玩家
		[System.ComponentModel.Description("内存不足")]
		GMErrorCode_MemNotEnough_Error		= 6,					//内存不足
	}

	public enum GachaType
	{
		[System.ComponentModel.Description("免费扭蛋")]
		GachaType_Free = 1,                               //免费扭蛋
		[System.ComponentModel.Description("付费扭蛋")]
		GachaType_Pay = 2,                                //付费扭蛋
	}

	public enum GmRouteMsgType
	{
		[System.ComponentModel.Description("某推送类型")]
		GmRouteMsgType_XXX					= 1,					//某推送类型
	}

	public enum ItemType
	{
		[System.ComponentModel.Description("ItemType_None=0")]
		ItemType_None = 0,                              
		[System.ComponentModel.Description("普通")]
		ItemType_Common = 1,                        //普通
		[System.ComponentModel.Description("卡牌")]
		ItemType_Card = 2,                          //卡牌
		[System.ComponentModel.Description("货币")]
		ItemType_Coin = 3,                          //货币
		[System.ComponentModel.Description("食物")]
		ItemType_Food = 10,                          //食物
		[System.ComponentModel.Description("幸运物")]
		ItemType_Luck = 11,							//幸运物
		[System.ComponentModel.Description("装备")]
		ItemType_Equip = 12,						//装备
		[System.ComponentModel.Description("礼物，旅行")]
		ItemType_Gift = 13,							//礼物，旅行
		[System.ComponentModel.Description("ItemType_Max=4")]
		ItemType_Max = 4, 
	}

	public enum LogicRes
	{
		[System.ComponentModel.Description("账号不存在")]
		LoginRes_Account_Error = 0x0001,					// 账号不存在
		[System.ComponentModel.Description("密码不匹配")]
		LoginRes_Key_Error = 0x0002,						// 密码不匹配
		[System.ComponentModel.Description("账号密码验证成功")]
		LoginRes_Success = 0x0003,							// 账号密码验证成功
		[System.ComponentModel.Description("账号id未授权")]
		Connect_AccountId_Error = 0x0004,					// 账号id未授权
		[System.ComponentModel.Description("服务器授权码key不正确")]
		Connect_Key_Error = 0x0005,							// 服务器授权码key不正确
		[System.ComponentModel.Description("连接服务器成功")]
		Connect_Success = 0x0006,							// 连接服务器成功
		[System.ComponentModel.Description("账号注册失败")]
		AccountReg_Error = 0x0007,							// 账号注册失败
		[System.ComponentModel.Description("账号注册成功")]
		AccountReg_Success = 0x0008,						// 账号注册成功
		[System.ComponentModel.Description("创建角色，角色名重复")]
		CreatePlayer_Name_Error = 0x1000,					// 创建角色，角色名重复
		[System.ComponentModel.Description("创建角色成功")]
		CreatePlayer_Success = 0x1001,						// 创建角色成功
		[System.ComponentModel.Description("角色名可用")]
		CheckName_Success = 0x1002,							// 角色名可用
		[System.ComponentModel.Description("角色名重复")]
		CheckName_Error = 0x1003,							// 角色名重复
		[System.ComponentModel.Description("设置gm成功")]
		SetGM_Success = 0x1004,								// 设置gm成功
		[System.ComponentModel.Description("设置gm失败")]
		SetGM_Error = 0x1005,								// 设置gm失败
		[System.ComponentModel.Description("协议数据未执行直接返回了")]
		Common_NoProcess_Error = 0x2000,					// 协议数据未执行直接返回了
		[System.ComponentModel.Description("玩家不在线")]
		Common_PlayerRecord_Error = 0x2001,					// 玩家不在线
		[System.ComponentModel.Description("协议数据包中缺少相关数据")]
		Common_MsgField_Error = 0x2002,						// 协议数据包中缺少相关数据
		[System.ComponentModel.Description("执行成功")]
		Common_Process_Success = 0x2003,					// 执行成功
		[System.ComponentModel.Description("数量不够")]
		Common_Num_Not_Enough = 0x2004,						// 数量不够
		[System.ComponentModel.Description("金币不够")]
		Common_Gold_Not_Enough = 0x2005,					// 金币不够
		[System.ComponentModel.Description("物品不够")]
		Common_Item_Not_Enough = 0x2006,					// 物品不够
		[System.ComponentModel.Description("客户端参数不对")]
		Common_Param_Error = 0x2007,						// 客户端参数不对
		[System.ComponentModel.Description("创建战斗局成功")]
		Battle_RoomCreat_Success = 0x4000,					// 创建战斗局成功
		[System.ComponentModel.Description("创建战斗局失败")]
		Battle_RoomCreat_Error = 0x4001,					// 创建战斗局失败
		[System.ComponentModel.Description("加入战斗局成功")]
		Battle_RoomJoin_Success = 0x4002,					// 加入战斗局成功
		[System.ComponentModel.Description("加入战斗局失败")]
		Battle_RoomJoin_Error = 0x4003,						// 加入战斗局失败
		[System.ComponentModel.Description("退出战斗局成功")]
		Battle_RoomExit_Success = 0x4004,					// 退出战斗局成功
		[System.ComponentModel.Description("退出战斗局失败")]
		Battle_RoomExit_Error = 0x4005,						// 退出战斗局失败
		[System.ComponentModel.Description("重置战斗局关卡成功")]
		Battle_RoomReset_Success = 0x4006,					// 重置战斗局关卡成功
		[System.ComponentModel.Description("重置战斗局关卡失败")]
		Battle_RoomReset_Error = 0x4007,					// 重置战斗局关卡失败
		[System.ComponentModel.Description("关卡结果提交成功")]
		PVE_SubmitResult_Success = 0x5000,					// 关卡结果提交成功
		[System.ComponentModel.Description("csv没有对应关卡id，提交失败")]
		PVE_SubmitResult_ChapterId_Error = 0x5001,			// csv没有对应关卡id，提交失败
		[System.ComponentModel.Description("csv没有对应关卡格子id，提交失败")]
		PVE_SubmitResult_TileId_Error = 0x5002,				// csv没有对应关卡格子id，提交失败
		[System.ComponentModel.Description("前置格子未通过，提交失败")]
		PVE_SubmitResult_LastTileId_Error = 0x5003,			// 前置格子未通过，提交失败
		[System.ComponentModel.Description("玩家没有该英雄，提交失败")]
		PVE_SubmitResult_HeroId_Error = 0x5004,				// 玩家没有该英雄，提交失败
		[System.ComponentModel.Description("体力不足，提交失败")]
		PVE_SubmitResult_Strength_Error = 0x5005,			// 体力不足，提交失败
		[System.ComponentModel.Description("关卡英雄回血成功")]
		PVE_RecoverHP_Success = 0x5010,						// 关卡英雄回血成功
		[System.ComponentModel.Description("玩家没有对应关卡记录，回血失败")]
		PVE_RecoverHP_ChapterRecord_Error = 0x5011,			// 玩家没有对应关卡记录，回血失败
		[System.ComponentModel.Description("玩家没有该英雄，回血失败")]
		PVE_RecoverHP_HeroId_Error = 0x5012,				// 玩家没有该英雄，回血失败
		[System.ComponentModel.Description("该关卡没有该英雄的参战记录，回血失败")]
		PVE_RecoverHP_HeroRecord_Error = 0x5013,			// 该关卡没有该英雄的参战记录，回血失败
		[System.ComponentModel.Description("道具不足，回血失败")]
		PVE_RecoverHP_ItemCount_Error = 0x5014,				// 道具不足，回血失败
		[System.ComponentModel.Description("赛事权限不足")]
		Contest_Auth_Error = 0x6000,                        // 赛事权限不足
		[System.ComponentModel.Description("赛事不存在")]
		Contest_Not_Exist_Error = 0x6001,                   // 赛事不存在
		[System.ComponentModel.Description("未参加该赛事")]
		Contest_Not_Joined_Error = 0x6002,                  // 未参加该赛事
		[System.ComponentModel.Description("没有合适房间")]
		Room_Not_Suitable = 0x7000,							// 没有合适房间
	}

	public class MailAppendixItem : IMessage
	{
		private ProtoMemberUInt32 _m_id;	
		private ProtoMemberUInt32 _m_count;	

		public MailAppendixItem()
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_count = new ProtoMemberUInt32(2, false);
		}

		public MailAppendixItem(uint __m_id, uint __m_count)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
			_m_count = new ProtoMemberUInt32(2, false);
			_m_count.member_value = __m_count;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public uint m_count
		{
			get{ return _m_count.member_value; }
			set{ _m_count.member_value = value; }
		}
		public bool has_m_count
		{
			get{ return _m_count.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _m_count.Serialize(_m_count.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			uint temp_m_count = 0;
			one_count = _m_count.ParseFrom(ref temp_m_count, ref int_stream);
			if (0 < one_count)
			{
					_m_count.member_value = temp_m_count;
					count = count + one_count;
			}

			return count;
		}
	}

	public class MapCreateMonsterInfo : IMessage
	{
		private ProtoMemberUInt64 _guid;	// 玩家
		private ProtoMemberUInt32 _monsterid;	
		private ProtoMemberUInt32 _camp;	
		private ProtoMemberUInt32List _natcamps;	
		private ProtoMemberUInt32List _tarcamps;	
		private ProtoMemberUInt32 _canHurt;	
		private ProtoMemberString _detail;	

		public MapCreateMonsterInfo()
		{
			_guid = new ProtoMemberUInt64(1, false);
			_monsterid = new ProtoMemberUInt32(2, false);
			_camp = new ProtoMemberUInt32(3, false);
			_natcamps = new ProtoMemberUInt32List(4, false);
			_tarcamps = new ProtoMemberUInt32List(5, false);
			_canHurt = new ProtoMemberUInt32(6, false);
			_detail = new ProtoMemberString(7, false);
		}

		public MapCreateMonsterInfo(ulong __guid, uint __monsterid, uint __camp, uint __canHurt, string __detail)
		{
			_guid = new ProtoMemberUInt64(1, false);
			_guid.member_value = __guid;
			_monsterid = new ProtoMemberUInt32(2, false);
			_monsterid.member_value = __monsterid;
			_camp = new ProtoMemberUInt32(3, false);
			_camp.member_value = __camp;
			_natcamps = new ProtoMemberUInt32List(4, false);
			_tarcamps = new ProtoMemberUInt32List(5, false);
			_canHurt = new ProtoMemberUInt32(6, false);
			_canHurt.member_value = __canHurt;
			_detail = new ProtoMemberString(7, false);
			_detail.member_value = __detail;
		}

		public ulong guid
		{
			get{ return _guid.member_value; }
			set{ _guid.member_value = value; }
		}
		public bool has_guid
		{
			get{ return _guid.has_value; }
		}

		public uint monsterid
		{
			get{ return _monsterid.member_value; }
			set{ _monsterid.member_value = value; }
		}
		public bool has_monsterid
		{
			get{ return _monsterid.has_value; }
		}

		public uint camp
		{
			get{ return _camp.member_value; }
			set{ _camp.member_value = value; }
		}
		public bool has_camp
		{
			get{ return _camp.has_value; }
		}

		public System.Collections.Generic.List<uint> natcamps
		{
			get{ return _natcamps.member_value; }
		}
		public bool has_natcamps
		{
			get{ return _natcamps.has_value; }
		}

		public System.Collections.Generic.List<uint> tarcamps
		{
			get{ return _tarcamps.member_value; }
		}
		public bool has_tarcamps
		{
			get{ return _tarcamps.has_value; }
		}

		public uint canHurt
		{
			get{ return _canHurt.member_value; }
			set{ _canHurt.member_value = value; }
		}
		public bool has_canHurt
		{
			get{ return _canHurt.has_value; }
		}

		public string detail
		{
			get{ return _detail.member_value; }
			set{ _detail.member_value = value; }
		}
		public bool has_detail
		{
			get{ return _detail.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _guid.Serialize(_guid.member_value, ref out_stream);

			count += _monsterid.Serialize(_monsterid.member_value, ref out_stream);

			count += _camp.Serialize(_camp.member_value, ref out_stream);

			foreach(uint one_member_value in _natcamps.member_value)
			{
				count += _natcamps.Serialize(one_member_value, ref out_stream);
			}

			foreach(uint one_member_value in _tarcamps.member_value)
			{
				count += _tarcamps.Serialize(one_member_value, ref out_stream);
			}

			count += _canHurt.Serialize(_canHurt.member_value, ref out_stream);

			count += _detail.Serialize(_detail.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_guid = 0;
			one_count = _guid.ParseFrom(ref temp_guid, ref int_stream);
			if (0 < one_count)
			{
					_guid.member_value = temp_guid;
					count = count + one_count;
			}

			uint temp_monsterid = 0;
			one_count = _monsterid.ParseFrom(ref temp_monsterid, ref int_stream);
			if (0 < one_count)
			{
					_monsterid.member_value = temp_monsterid;
					count = count + one_count;
			}

			uint temp_camp = 0;
			one_count = _camp.ParseFrom(ref temp_camp, ref int_stream);
			if (0 < one_count)
			{
					_camp.member_value = temp_camp;
					count = count + one_count;
			}

			while (true)
			{
				uint one_member_value = 0;
				one_count = _natcamps.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_natcamps.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			while (true)
			{
				uint one_member_value = 0;
				one_count = _tarcamps.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_tarcamps.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			uint temp_canHurt = 0;
			one_count = _canHurt.ParseFrom(ref temp_canHurt, ref int_stream);
			if (0 < one_count)
			{
					_canHurt.member_value = temp_canHurt;
					count = count + one_count;
			}

			string temp_detail = "";
			one_count = _detail.ParseFrom(ref temp_detail, ref int_stream);
			if (0 < one_count)
			{
					_detail.member_value = temp_detail;
					count = count + one_count;
			}

			return count;
		}
	}

	public class MapCreatePlayerInfo : IMessage
	{
		private ProtoMemberUInt64 _guid;	// 玩家
		private ProtoMemberUInt32 _maxhp;	// 最大hp
		private ProtoMemberUInt32 _maxshield;	// 最大护盾值

		public MapCreatePlayerInfo()
		{
			_guid = new ProtoMemberUInt64(1, false);
			_maxhp = new ProtoMemberUInt32(2, false);
			_maxshield = new ProtoMemberUInt32(3, false);
		}

		public MapCreatePlayerInfo(ulong __guid, uint __maxhp, uint __maxshield)
		{
			_guid = new ProtoMemberUInt64(1, false);
			_guid.member_value = __guid;
			_maxhp = new ProtoMemberUInt32(2, false);
			_maxhp.member_value = __maxhp;
			_maxshield = new ProtoMemberUInt32(3, false);
			_maxshield.member_value = __maxshield;
		}

		public ulong guid
		{
			get{ return _guid.member_value; }
			set{ _guid.member_value = value; }
		}
		public bool has_guid
		{
			get{ return _guid.has_value; }
		}

		public uint maxhp
		{
			get{ return _maxhp.member_value; }
			set{ _maxhp.member_value = value; }
		}
		public bool has_maxhp
		{
			get{ return _maxhp.has_value; }
		}

		public uint maxshield
		{
			get{ return _maxshield.member_value; }
			set{ _maxshield.member_value = value; }
		}
		public bool has_maxshield
		{
			get{ return _maxshield.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _guid.Serialize(_guid.member_value, ref out_stream);

			count += _maxhp.Serialize(_maxhp.member_value, ref out_stream);

			count += _maxshield.Serialize(_maxshield.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_guid = 0;
			one_count = _guid.ParseFrom(ref temp_guid, ref int_stream);
			if (0 < one_count)
			{
					_guid.member_value = temp_guid;
					count = count + one_count;
			}

			uint temp_maxhp = 0;
			one_count = _maxhp.ParseFrom(ref temp_maxhp, ref int_stream);
			if (0 < one_count)
			{
					_maxhp.member_value = temp_maxhp;
					count = count + one_count;
			}

			uint temp_maxshield = 0;
			one_count = _maxshield.ParseFrom(ref temp_maxshield, ref int_stream);
			if (0 < one_count)
			{
					_maxshield.member_value = temp_maxshield;
					count = count + one_count;
			}

			return count;
		}
	}

	public enum MoveDirectionType
	{
		[System.ComponentModel.Description("前进")]
		MoveDirectionType_Before = 1,                            //前进
		[System.ComponentModel.Description("后退")]
		MoveDirectionType_After = 2,                            //后退
		[System.ComponentModel.Description("不动")]
		MoveDirectionType_Stand = 3,                            //不动
	}

	public class Msg_Account2Center_Reg_Login_Key_Req : IMessage
	{
		private ProtoMemberString _m_serial_number;	// 消息流水号
		private ProtoMemberUInt32 _m_account_id;	// 验证成功的情况下，账号id
		private ProtoMemberUInt32 _m_anti_addiction;	// 防沉迷系统，该账号今日已游戏的秒数

		public Msg_Account2Center_Reg_Login_Key_Req()
		{
			_m_serial_number = new ProtoMemberString(1, false);
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_anti_addiction = new ProtoMemberUInt32(3, false);
		}

		public Msg_Account2Center_Reg_Login_Key_Req(string __m_serial_number, uint __m_account_id, uint __m_anti_addiction)
		{
			_m_serial_number = new ProtoMemberString(1, false);
			_m_serial_number.member_value = __m_serial_number;
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_account_id.member_value = __m_account_id;
			_m_anti_addiction = new ProtoMemberUInt32(3, false);
			_m_anti_addiction.member_value = __m_anti_addiction;
		}

		public string m_serial_number
		{
			get{ return _m_serial_number.member_value; }
			set{ _m_serial_number.member_value = value; }
		}
		public bool has_m_serial_number
		{
			get{ return _m_serial_number.has_value; }
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_anti_addiction
		{
			get{ return _m_anti_addiction.member_value; }
			set{ _m_anti_addiction.member_value = value; }
		}
		public bool has_m_anti_addiction
		{
			get{ return _m_anti_addiction.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_serial_number.Serialize(_m_serial_number.member_value, ref out_stream);

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_anti_addiction.Serialize(_m_anti_addiction.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_serial_number = "";
			one_count = _m_serial_number.ParseFrom(ref temp_m_serial_number, ref int_stream);
			if (0 < one_count)
			{
					_m_serial_number.member_value = temp_m_serial_number;
					count = count + one_count;
			}

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_anti_addiction = 0;
			one_count = _m_anti_addiction.ParseFrom(ref temp_m_anti_addiction, ref int_stream);
			if (0 < one_count)
			{
					_m_anti_addiction.member_value = temp_m_anti_addiction;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Account2Client_Login_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 验证结果AccountRes
		private ProtoMemberUInt32 _m_account_id;	// 验证成功的情况下，账号id
		private ProtoMemberUInt32 _m_key;	// 验证成功的情况下，服务器生成的授权码key，进入服务器需要带这个key

		public Msg_Account2Client_Login_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_key = new ProtoMemberUInt32(3, false);
		}

		public Msg_Account2Client_Login_Res(uint __m_res, uint __m_account_id, uint __m_key)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_account_id.member_value = __m_account_id;
			_m_key = new ProtoMemberUInt32(3, false);
			_m_key.member_value = __m_key;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_key
		{
			get{ return _m_key.member_value; }
			set{ _m_key.member_value = value; }
		}
		public bool has_m_key
		{
			get{ return _m_key.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_key.Serialize(_m_key.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_key = 0;
			one_count = _m_key.ParseFrom(ref temp_m_key, ref int_stream);
			if (0 < one_count)
			{
					_m_key.member_value = temp_m_key;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Account2Client_Reg_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 注册结果AccountRes

		public Msg_Account2Client_Reg_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
		}

		public Msg_Account2Client_Reg_Res(uint __m_res)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Broadcast : IMessage
	{

		public Msg_Broadcast()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Center2Account_Login_Req : IMessage
	{
		private ProtoMemberUInt32 _m_account_id;	// 角色ID
		private ProtoMemberUInt32 _m_server_id;	// 服务器ID
		private ProtoMemberUInt64 _m_time_login;	// 上线时间戳

		public Msg_Center2Account_Login_Req()
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_time_login = new ProtoMemberUInt64(3, false);
		}

		public Msg_Center2Account_Login_Req(uint __m_account_id, uint __m_server_id, ulong __m_time_login)
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_account_id.member_value = __m_account_id;
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_server_id.member_value = __m_server_id;
			_m_time_login = new ProtoMemberUInt64(3, false);
			_m_time_login.member_value = __m_time_login;
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_server_id
		{
			get{ return _m_server_id.member_value; }
			set{ _m_server_id.member_value = value; }
		}
		public bool has_m_server_id
		{
			get{ return _m_server_id.has_value; }
		}

		public ulong m_time_login
		{
			get{ return _m_time_login.member_value; }
			set{ _m_time_login.member_value = value; }
		}
		public bool has_m_time_login
		{
			get{ return _m_time_login.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_server_id.Serialize(_m_server_id.member_value, ref out_stream);

			count += _m_time_login.Serialize(_m_time_login.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_server_id = 0;
			one_count = _m_server_id.ParseFrom(ref temp_m_server_id, ref int_stream);
			if (0 < one_count)
			{
					_m_server_id.member_value = temp_m_server_id;
					count = count + one_count;
			}

			ulong temp_m_time_login = 0;
			one_count = _m_time_login.ParseFrom(ref temp_m_time_login, ref int_stream);
			if (0 < one_count)
			{
					_m_time_login.member_value = temp_m_time_login;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Center2Account_Logout_Req : IMessage
	{
		private ProtoMemberUInt32 _m_account_id;	// 角色ID
		private ProtoMemberUInt32 _m_server_id;	// 服务器ID
		private ProtoMemberUInt64 _m_time_logout;	// 下线时间戳

		public Msg_Center2Account_Logout_Req()
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_time_logout = new ProtoMemberUInt64(3, false);
		}

		public Msg_Center2Account_Logout_Req(uint __m_account_id, uint __m_server_id, ulong __m_time_logout)
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_account_id.member_value = __m_account_id;
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_server_id.member_value = __m_server_id;
			_m_time_logout = new ProtoMemberUInt64(3, false);
			_m_time_logout.member_value = __m_time_logout;
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_server_id
		{
			get{ return _m_server_id.member_value; }
			set{ _m_server_id.member_value = value; }
		}
		public bool has_m_server_id
		{
			get{ return _m_server_id.has_value; }
		}

		public ulong m_time_logout
		{
			get{ return _m_time_logout.member_value; }
			set{ _m_time_logout.member_value = value; }
		}
		public bool has_m_time_logout
		{
			get{ return _m_time_logout.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_server_id.Serialize(_m_server_id.member_value, ref out_stream);

			count += _m_time_logout.Serialize(_m_time_logout.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_server_id = 0;
			one_count = _m_server_id.ParseFrom(ref temp_m_server_id, ref int_stream);
			if (0 < one_count)
			{
					_m_server_id.member_value = temp_m_server_id;
					count = count + one_count;
			}

			ulong temp_m_time_logout = 0;
			one_count = _m_time_logout.ParseFrom(ref temp_m_time_logout, ref int_stream);
			if (0 < one_count)
			{
					_m_time_logout.member_value = temp_m_time_logout;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Center2Account_Reg_Login_Key_Res : IMessage
	{
		private ProtoMemberString _m_serial_number;	// 消息流水号
		private ProtoMemberUInt32 _m_account_id;	// 验证成功的情况下，账号id
		private ProtoMemberUInt32 _m_key;	// 服务器生成的授权码key，进入服务器需要带这个key

		public Msg_Center2Account_Reg_Login_Key_Res()
		{
			_m_serial_number = new ProtoMemberString(1, false);
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_key = new ProtoMemberUInt32(3, false);
		}

		public Msg_Center2Account_Reg_Login_Key_Res(string __m_serial_number, uint __m_account_id, uint __m_key)
		{
			_m_serial_number = new ProtoMemberString(1, false);
			_m_serial_number.member_value = __m_serial_number;
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_account_id.member_value = __m_account_id;
			_m_key = new ProtoMemberUInt32(3, false);
			_m_key.member_value = __m_key;
		}

		public string m_serial_number
		{
			get{ return _m_serial_number.member_value; }
			set{ _m_serial_number.member_value = value; }
		}
		public bool has_m_serial_number
		{
			get{ return _m_serial_number.has_value; }
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_key
		{
			get{ return _m_key.member_value; }
			set{ _m_key.member_value = value; }
		}
		public bool has_m_key
		{
			get{ return _m_key.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_serial_number.Serialize(_m_serial_number.member_value, ref out_stream);

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_key.Serialize(_m_key.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_serial_number = "";
			one_count = _m_serial_number.ParseFrom(ref temp_m_serial_number, ref int_stream);
			if (0 < one_count)
			{
					_m_serial_number.member_value = temp_m_serial_number;
					count = count + one_count;
			}

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_key = 0;
			one_count = _m_key.ParseFrom(ref temp_m_key, ref int_stream);
			if (0 < one_count)
			{
					_m_key.member_value = temp_m_key;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Center2Gate_Connect_Res : IMessage
	{
		private ProtoMemberUInt32 _m_account_id;	// 账号id
		private ProtoMemberUInt32 _m_server_id;	// 所选服id
		private ProtoMemberUInt32 _m_res;	// 进入服务器的key验证结果（LogicRes）
		private ProtoMemberString _m_ip;	// 登陆ip（网关填）
		private ProtoMemberString _m_imei;	// 设备唯一标识（客户端填）

		public Msg_Center2Gate_Connect_Res()
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_res = new ProtoMemberUInt32(3, false);
			_m_ip = new ProtoMemberString(4, false);
			_m_imei = new ProtoMemberString(5, false);
		}

		public Msg_Center2Gate_Connect_Res(uint __m_account_id, uint __m_server_id, uint __m_res, string __m_ip, string __m_imei)
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_account_id.member_value = __m_account_id;
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_server_id.member_value = __m_server_id;
			_m_res = new ProtoMemberUInt32(3, false);
			_m_res.member_value = __m_res;
			_m_ip = new ProtoMemberString(4, false);
			_m_ip.member_value = __m_ip;
			_m_imei = new ProtoMemberString(5, false);
			_m_imei.member_value = __m_imei;
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_server_id
		{
			get{ return _m_server_id.member_value; }
			set{ _m_server_id.member_value = value; }
		}
		public bool has_m_server_id
		{
			get{ return _m_server_id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public string m_ip
		{
			get{ return _m_ip.member_value; }
			set{ _m_ip.member_value = value; }
		}
		public bool has_m_ip
		{
			get{ return _m_ip.has_value; }
		}

		public string m_imei
		{
			get{ return _m_imei.member_value; }
			set{ _m_imei.member_value = value; }
		}
		public bool has_m_imei
		{
			get{ return _m_imei.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_server_id.Serialize(_m_server_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			count += _m_ip.Serialize(_m_ip.member_value, ref out_stream);

			count += _m_imei.Serialize(_m_imei.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_server_id = 0;
			one_count = _m_server_id.ParseFrom(ref temp_m_server_id, ref int_stream);
			if (0 < one_count)
			{
					_m_server_id.member_value = temp_m_server_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			string temp_m_ip = "";
			one_count = _m_ip.ParseFrom(ref temp_m_ip, ref int_stream);
			if (0 < one_count)
			{
					_m_ip.member_value = temp_m_ip;
					count = count + one_count;
			}

			string temp_m_imei = "";
			one_count = _m_imei.ParseFrom(ref temp_m_imei, ref int_stream);
			if (0 < one_count)
			{
					_m_imei.member_value = temp_m_imei;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Center2Logic_GMOperator_Req : IMessage
	{
		private ProtoMemberBytes _json_string;	
		private ProtoMemberInt32 _id;	

		public Msg_Center2Logic_GMOperator_Req()
		{
			_json_string = new ProtoMemberBytes(1, false);
			_id = new ProtoMemberInt32(2, false);
		}

		public Msg_Center2Logic_GMOperator_Req(byte[] __json_string, int __id)
		{
			_json_string = new ProtoMemberBytes(1, false);
			_json_string.member_value = __json_string;
			_id = new ProtoMemberInt32(2, false);
			_id.member_value = __id;
		}

		public byte[] json_string
		{
			get{ return _json_string.member_value; }
			set{ _json_string.member_value = value; }
		}
		public bool has_json_string
		{
			get{ return _json_string.has_value; }
		}

		public int id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _json_string.Serialize(_json_string.member_value, ref out_stream);

			count += _id.Serialize(_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			byte[] temp_json_string = new byte[1];
			one_count = _json_string.ParseFrom(ref temp_json_string, ref int_stream);
			if (0 < one_count)
			{
					_json_string.member_value = temp_json_string;
					count = count + one_count;
			}

			int temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Account_Login_Req : IMessage
	{
		private ProtoMemberString _m_account_name;	// 请求登录的账号/密码
		private ProtoMemberString _m_account_key;	

		public Msg_Client2Account_Login_Req()
		{
			_m_account_name = new ProtoMemberString(1, false);
			_m_account_key = new ProtoMemberString(2, false);
		}

		public Msg_Client2Account_Login_Req(string __m_account_name, string __m_account_key)
		{
			_m_account_name = new ProtoMemberString(1, false);
			_m_account_name.member_value = __m_account_name;
			_m_account_key = new ProtoMemberString(2, false);
			_m_account_key.member_value = __m_account_key;
		}

		public string m_account_name
		{
			get{ return _m_account_name.member_value; }
			set{ _m_account_name.member_value = value; }
		}
		public bool has_m_account_name
		{
			get{ return _m_account_name.has_value; }
		}

		public string m_account_key
		{
			get{ return _m_account_key.member_value; }
			set{ _m_account_key.member_value = value; }
		}
		public bool has_m_account_key
		{
			get{ return _m_account_key.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_name.Serialize(_m_account_name.member_value, ref out_stream);

			count += _m_account_key.Serialize(_m_account_key.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_account_name = "";
			one_count = _m_account_name.ParseFrom(ref temp_m_account_name, ref int_stream);
			if (0 < one_count)
			{
					_m_account_name.member_value = temp_m_account_name;
					count = count + one_count;
			}

			string temp_m_account_key = "";
			one_count = _m_account_key.ParseFrom(ref temp_m_account_key, ref int_stream);
			if (0 < one_count)
			{
					_m_account_key.member_value = temp_m_account_key;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Account_Reg_Req : IMessage
	{
		private ProtoMemberString _m_account_name;	// 请求注册的账号/密码
		private ProtoMemberString _m_account_key;	

		public Msg_Client2Account_Reg_Req()
		{
			_m_account_name = new ProtoMemberString(1, false);
			_m_account_key = new ProtoMemberString(2, false);
		}

		public Msg_Client2Account_Reg_Req(string __m_account_name, string __m_account_key)
		{
			_m_account_name = new ProtoMemberString(1, false);
			_m_account_name.member_value = __m_account_name;
			_m_account_key = new ProtoMemberString(2, false);
			_m_account_key.member_value = __m_account_key;
		}

		public string m_account_name
		{
			get{ return _m_account_name.member_value; }
			set{ _m_account_name.member_value = value; }
		}
		public bool has_m_account_name
		{
			get{ return _m_account_name.has_value; }
		}

		public string m_account_key
		{
			get{ return _m_account_key.member_value; }
			set{ _m_account_key.member_value = value; }
		}
		public bool has_m_account_key
		{
			get{ return _m_account_key.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_name.Serialize(_m_account_name.member_value, ref out_stream);

			count += _m_account_key.Serialize(_m_account_key.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_account_name = "";
			one_count = _m_account_name.ParseFrom(ref temp_m_account_name, ref int_stream);
			if (0 < one_count)
			{
					_m_account_name.member_value = temp_m_account_name;
					count = count + one_count;
			}

			string temp_m_account_key = "";
			one_count = _m_account_key.ParseFrom(ref temp_m_account_key, ref int_stream);
			if (0 < one_count)
			{
					_m_account_key.member_value = temp_m_account_key;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Gate_Connect_Req : IMessage
	{
		private ProtoMemberUInt32 _m_account_id;	// 账号id
		private ProtoMemberUInt32 _m_server_id;	// 所选服id
		private ProtoMemberUInt32 _m_key;	// 进入服务器的授权码key
		private ProtoMemberString _m_ip;	// 登陆ip（网关填）
		private ProtoMemberString _m_imei;	// 设备唯一标识（客户端填）

		public Msg_Client2Gate_Connect_Req()
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_key = new ProtoMemberUInt32(3, false);
			_m_ip = new ProtoMemberString(4, false);
			_m_imei = new ProtoMemberString(5, false);
		}

		public Msg_Client2Gate_Connect_Req(uint __m_account_id, uint __m_server_id, uint __m_key, string __m_ip, string __m_imei)
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_account_id.member_value = __m_account_id;
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_server_id.member_value = __m_server_id;
			_m_key = new ProtoMemberUInt32(3, false);
			_m_key.member_value = __m_key;
			_m_ip = new ProtoMemberString(4, false);
			_m_ip.member_value = __m_ip;
			_m_imei = new ProtoMemberString(5, false);
			_m_imei.member_value = __m_imei;
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_server_id
		{
			get{ return _m_server_id.member_value; }
			set{ _m_server_id.member_value = value; }
		}
		public bool has_m_server_id
		{
			get{ return _m_server_id.has_value; }
		}

		public uint m_key
		{
			get{ return _m_key.member_value; }
			set{ _m_key.member_value = value; }
		}
		public bool has_m_key
		{
			get{ return _m_key.has_value; }
		}

		public string m_ip
		{
			get{ return _m_ip.member_value; }
			set{ _m_ip.member_value = value; }
		}
		public bool has_m_ip
		{
			get{ return _m_ip.has_value; }
		}

		public string m_imei
		{
			get{ return _m_imei.member_value; }
			set{ _m_imei.member_value = value; }
		}
		public bool has_m_imei
		{
			get{ return _m_imei.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_server_id.Serialize(_m_server_id.member_value, ref out_stream);

			count += _m_key.Serialize(_m_key.member_value, ref out_stream);

			count += _m_ip.Serialize(_m_ip.member_value, ref out_stream);

			count += _m_imei.Serialize(_m_imei.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_server_id = 0;
			one_count = _m_server_id.ParseFrom(ref temp_m_server_id, ref int_stream);
			if (0 < one_count)
			{
					_m_server_id.member_value = temp_m_server_id;
					count = count + one_count;
			}

			uint temp_m_key = 0;
			one_count = _m_key.ParseFrom(ref temp_m_key, ref int_stream);
			if (0 < one_count)
			{
					_m_key.member_value = temp_m_key;
					count = count + one_count;
			}

			string temp_m_ip = "";
			one_count = _m_ip.ParseFrom(ref temp_m_ip, ref int_stream);
			if (0 < one_count)
			{
					_m_ip.member_value = temp_m_ip;
					count = count + one_count;
			}

			string temp_m_imei = "";
			one_count = _m_imei.ParseFrom(ref temp_m_imei, ref int_stream);
			if (0 < one_count)
			{
					_m_imei.member_value = temp_m_imei;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_BattleSettlement_Req : IMessage
	{
		private ProtoMemberString _detail;	

		public Msg_Client2Logic_BattleSettlement_Req()
		{
			_detail = new ProtoMemberString(1, false);
		}

		public Msg_Client2Logic_BattleSettlement_Req(string __detail)
		{
			_detail = new ProtoMemberString(1, false);
			_detail.member_value = __detail;
		}

		public string detail
		{
			get{ return _detail.member_value; }
			set{ _detail.member_value = value; }
		}
		public bool has_detail
		{
			get{ return _detail.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _detail.Serialize(_detail.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_detail = "";
			one_count = _detail.ParseFrom(ref temp_detail, ref int_stream);
			if (0 < one_count)
			{
					_detail.member_value = temp_detail;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_Data_Req : IMessage
	{
		private ProtoMemberEmbedded<UnitInfo> _m_unit;	// 角色数据信息

		public Msg_Client2Logic_Battle_Data_Req()
		{
			_m_unit = new ProtoMemberEmbedded<UnitInfo>(1, false);
			_m_unit.member_value = new UnitInfo();
		}

		public UnitInfo m_unit
		{
			get{ return _m_unit.member_value as UnitInfo; }
			set{ _m_unit.member_value = value; }
		}
		public bool has_m_unit
		{
			get{ return _m_unit.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_unit.Serialize(_m_unit.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			UnitInfo temp_m_unit = new UnitInfo();
			one_count = _m_unit.ParseFrom(temp_m_unit, ref int_stream);
			if (0 < one_count)
			{
					_m_unit.member_value = temp_m_unit;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_End_Broadcast : IMessage
	{

		public Msg_Client2Logic_Battle_End_Broadcast()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_Move_Req : IMessage
	{
		private ProtoMemberEmbedded<UnitMoveInfo> _m_move;	// 移动信息

		public Msg_Client2Logic_Battle_Move_Req()
		{
			_m_move = new ProtoMemberEmbedded<UnitMoveInfo>(1, false);
			_m_move.member_value = new UnitMoveInfo();
		}

		public UnitMoveInfo m_move
		{
			get{ return _m_move.member_value as UnitMoveInfo; }
			set{ _m_move.member_value = value; }
		}
		public bool has_m_move
		{
			get{ return _m_move.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_move.Serialize(_m_move.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			UnitMoveInfo temp_m_move = new UnitMoveInfo();
			one_count = _m_move.ParseFrom(temp_m_move, ref int_stream);
			if (0 < one_count)
			{
					_m_move.member_value = temp_m_move;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_Ready2_Req : IMessage
	{

		public Msg_Client2Logic_Battle_Ready2_Req()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_Ready3_Req : IMessage
	{

		public Msg_Client2Logic_Battle_Ready3_Req()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_Ready_Req : IMessage
	{

		public Msg_Client2Logic_Battle_Ready_Req()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_RoomCreat_Req : IMessage
	{
		private ProtoMemberUInt32 _m_chapterid;	// 关卡id
		private ProtoMemberUInt32 _m_weapon;	// 使用的武器
		private ProtoMemberUInt32 _m_fight_type;	// 战斗类型
		private ProtoMemberUInt64 _m_room_type;	// 房间类型

		public Msg_Client2Logic_Battle_RoomCreat_Req()
		{
			_m_chapterid = new ProtoMemberUInt32(1, false);
			_m_weapon = new ProtoMemberUInt32(2, false);
			_m_fight_type = new ProtoMemberUInt32(3, false);
			_m_room_type = new ProtoMemberUInt64(4, false);
		}

		public Msg_Client2Logic_Battle_RoomCreat_Req(uint __m_chapterid, uint __m_weapon, uint __m_fight_type, ulong __m_room_type)
		{
			_m_chapterid = new ProtoMemberUInt32(1, false);
			_m_chapterid.member_value = __m_chapterid;
			_m_weapon = new ProtoMemberUInt32(2, false);
			_m_weapon.member_value = __m_weapon;
			_m_fight_type = new ProtoMemberUInt32(3, false);
			_m_fight_type.member_value = __m_fight_type;
			_m_room_type = new ProtoMemberUInt64(4, false);
			_m_room_type.member_value = __m_room_type;
		}

		public uint m_chapterid
		{
			get{ return _m_chapterid.member_value; }
			set{ _m_chapterid.member_value = value; }
		}
		public bool has_m_chapterid
		{
			get{ return _m_chapterid.has_value; }
		}

		public uint m_weapon
		{
			get{ return _m_weapon.member_value; }
			set{ _m_weapon.member_value = value; }
		}
		public bool has_m_weapon
		{
			get{ return _m_weapon.has_value; }
		}

		public uint m_fight_type
		{
			get{ return _m_fight_type.member_value; }
			set{ _m_fight_type.member_value = value; }
		}
		public bool has_m_fight_type
		{
			get{ return _m_fight_type.has_value; }
		}

		public ulong m_room_type
		{
			get{ return _m_room_type.member_value; }
			set{ _m_room_type.member_value = value; }
		}
		public bool has_m_room_type
		{
			get{ return _m_room_type.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_chapterid.Serialize(_m_chapterid.member_value, ref out_stream);

			count += _m_weapon.Serialize(_m_weapon.member_value, ref out_stream);

			count += _m_fight_type.Serialize(_m_fight_type.member_value, ref out_stream);

			count += _m_room_type.Serialize(_m_room_type.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_chapterid = 0;
			one_count = _m_chapterid.ParseFrom(ref temp_m_chapterid, ref int_stream);
			if (0 < one_count)
			{
					_m_chapterid.member_value = temp_m_chapterid;
					count = count + one_count;
			}

			uint temp_m_weapon = 0;
			one_count = _m_weapon.ParseFrom(ref temp_m_weapon, ref int_stream);
			if (0 < one_count)
			{
					_m_weapon.member_value = temp_m_weapon;
					count = count + one_count;
			}

			uint temp_m_fight_type = 0;
			one_count = _m_fight_type.ParseFrom(ref temp_m_fight_type, ref int_stream);
			if (0 < one_count)
			{
					_m_fight_type.member_value = temp_m_fight_type;
					count = count + one_count;
			}

			ulong temp_m_room_type = 0;
			one_count = _m_room_type.ParseFrom(ref temp_m_room_type, ref int_stream);
			if (0 < one_count)
			{
					_m_room_type.member_value = temp_m_room_type;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_RoomExit_Req : IMessage
	{
		private ProtoMemberUInt64 _m_guid;	// 房间唯一id

		public Msg_Client2Logic_Battle_RoomExit_Req()
		{
			_m_guid = new ProtoMemberUInt64(1, false);
		}

		public Msg_Client2Logic_Battle_RoomExit_Req(ulong __m_guid)
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_guid.member_value = __m_guid;
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_RoomJoin_Req : IMessage
	{
		private ProtoMemberUInt64 _m_guid;	// 房间唯一id
		private ProtoMemberUInt32 _m_chapterid;	// 关卡id
		private ProtoMemberUInt32 _m_weapon;	// 使用的武器

		public Msg_Client2Logic_Battle_RoomJoin_Req()
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_chapterid = new ProtoMemberUInt32(2, false);
			_m_weapon = new ProtoMemberUInt32(3, false);
		}

		public Msg_Client2Logic_Battle_RoomJoin_Req(ulong __m_guid, uint __m_chapterid, uint __m_weapon)
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_guid.member_value = __m_guid;
			_m_chapterid = new ProtoMemberUInt32(2, false);
			_m_chapterid.member_value = __m_chapterid;
			_m_weapon = new ProtoMemberUInt32(3, false);
			_m_weapon.member_value = __m_weapon;
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public uint m_chapterid
		{
			get{ return _m_chapterid.member_value; }
			set{ _m_chapterid.member_value = value; }
		}
		public bool has_m_chapterid
		{
			get{ return _m_chapterid.has_value; }
		}

		public uint m_weapon
		{
			get{ return _m_weapon.member_value; }
			set{ _m_weapon.member_value = value; }
		}
		public bool has_m_weapon
		{
			get{ return _m_weapon.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			count += _m_chapterid.Serialize(_m_chapterid.member_value, ref out_stream);

			count += _m_weapon.Serialize(_m_weapon.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			uint temp_m_chapterid = 0;
			one_count = _m_chapterid.ParseFrom(ref temp_m_chapterid, ref int_stream);
			if (0 < one_count)
			{
					_m_chapterid.member_value = temp_m_chapterid;
					count = count + one_count;
			}

			uint temp_m_weapon = 0;
			one_count = _m_weapon.ParseFrom(ref temp_m_weapon, ref int_stream);
			if (0 < one_count)
			{
					_m_weapon.member_value = temp_m_weapon;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_RoomList_Req : IMessage
	{
		private ProtoMemberUInt64 _m_room_type;	// 房间类型

		public Msg_Client2Logic_Battle_RoomList_Req()
		{
			_m_room_type = new ProtoMemberUInt64(1, false);
		}

		public Msg_Client2Logic_Battle_RoomList_Req(ulong __m_room_type)
		{
			_m_room_type = new ProtoMemberUInt64(1, false);
			_m_room_type.member_value = __m_room_type;
		}

		public ulong m_room_type
		{
			get{ return _m_room_type.member_value; }
			set{ _m_room_type.member_value = value; }
		}
		public bool has_m_room_type
		{
			get{ return _m_room_type.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_room_type.Serialize(_m_room_type.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_room_type = 0;
			one_count = _m_room_type.ParseFrom(ref temp_m_room_type, ref int_stream);
			if (0 < one_count)
			{
					_m_room_type.member_value = temp_m_room_type;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_RoomReset_Req : IMessage
	{
		private ProtoMemberUInt32 _m_chapterid;	// 关卡id

		public Msg_Client2Logic_Battle_RoomReset_Req()
		{
			_m_chapterid = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_Battle_RoomReset_Req(uint __m_chapterid)
		{
			_m_chapterid = new ProtoMemberUInt32(1, false);
			_m_chapterid.member_value = __m_chapterid;
		}

		public uint m_chapterid
		{
			get{ return _m_chapterid.member_value; }
			set{ _m_chapterid.member_value = value; }
		}
		public bool has_m_chapterid
		{
			get{ return _m_chapterid.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_chapterid.Serialize(_m_chapterid.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_chapterid = 0;
			one_count = _m_chapterid.ParseFrom(ref temp_m_chapterid, ref int_stream);
			if (0 < one_count)
			{
					_m_chapterid.member_value = temp_m_chapterid;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Battle_Skill_Req : IMessage
	{
		private ProtoMemberEmbedded<UnitSkillInfo> _m_skill;	// 技能信息

		public Msg_Client2Logic_Battle_Skill_Req()
		{
			_m_skill = new ProtoMemberEmbedded<UnitSkillInfo>(1, false);
			_m_skill.member_value = new UnitSkillInfo();
		}

		public UnitSkillInfo m_skill
		{
			get{ return _m_skill.member_value as UnitSkillInfo; }
			set{ _m_skill.member_value = value; }
		}
		public bool has_m_skill
		{
			get{ return _m_skill.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_skill.Serialize(_m_skill.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			UnitSkillInfo temp_m_skill = new UnitSkillInfo();
			one_count = _m_skill.ParseFrom(temp_m_skill, ref int_stream);
			if (0 < one_count)
			{
					_m_skill.member_value = temp_m_skill;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_BossBattleHp_Req : IMessage
	{
		private ProtoMemberUInt64 _bossBattleID;	
		private ProtoMemberUInt64 _hp;	
		private ProtoMemberString _detail;	

		public Msg_Client2Logic_BossBattleHp_Req()
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_hp = new ProtoMemberUInt64(2, false);
			_detail = new ProtoMemberString(3, false);
		}

		public Msg_Client2Logic_BossBattleHp_Req(ulong __bossBattleID, ulong __hp, string __detail)
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_bossBattleID.member_value = __bossBattleID;
			_hp = new ProtoMemberUInt64(2, false);
			_hp.member_value = __hp;
			_detail = new ProtoMemberString(3, false);
			_detail.member_value = __detail;
		}

		public ulong bossBattleID
		{
			get{ return _bossBattleID.member_value; }
			set{ _bossBattleID.member_value = value; }
		}
		public bool has_bossBattleID
		{
			get{ return _bossBattleID.has_value; }
		}

		public ulong hp
		{
			get{ return _hp.member_value; }
			set{ _hp.member_value = value; }
		}
		public bool has_hp
		{
			get{ return _hp.has_value; }
		}

		public string detail
		{
			get{ return _detail.member_value; }
			set{ _detail.member_value = value; }
		}
		public bool has_detail
		{
			get{ return _detail.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _bossBattleID.Serialize(_bossBattleID.member_value, ref out_stream);

			count += _hp.Serialize(_hp.member_value, ref out_stream);

			count += _detail.Serialize(_detail.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_bossBattleID = 0;
			one_count = _bossBattleID.ParseFrom(ref temp_bossBattleID, ref int_stream);
			if (0 < one_count)
			{
					_bossBattleID.member_value = temp_bossBattleID;
					count = count + one_count;
			}

			ulong temp_hp = 0;
			one_count = _hp.ParseFrom(ref temp_hp, ref int_stream);
			if (0 < one_count)
			{
					_hp.member_value = temp_hp;
					count = count + one_count;
			}

			string temp_detail = "";
			one_count = _detail.ParseFrom(ref temp_detail, ref int_stream);
			if (0 < one_count)
			{
					_detail.member_value = temp_detail;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Buy_Shop_Req : IMessage
	{
		private ProtoMemberUInt32 _id;	// id

		public Msg_Client2Logic_Buy_Shop_Req()
		{
			_id = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_Buy_Shop_Req(uint __id)
		{
			_id = new ProtoMemberUInt32(1, false);
			_id.member_value = __id;
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Change_Avatar_Req : IMessage
	{
		private ProtoMemberUInt32 _id;	// id

		public Msg_Client2Logic_Change_Avatar_Req()
		{
			_id = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_Change_Avatar_Req(uint __id)
		{
			_id = new ProtoMemberUInt32(1, false);
			_id.member_value = __id;
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_CheckPlayerName_Req : IMessage
	{
		private ProtoMemberString _m_player_name;	// 角色名

		public Msg_Client2Logic_CheckPlayerName_Req()
		{
			_m_player_name = new ProtoMemberString(1, false);
		}

		public Msg_Client2Logic_CheckPlayerName_Req(string __m_player_name)
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_player_name.member_value = __m_player_name;
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Compose_Req : IMessage
	{
		private ProtoMemberUInt32 _type;	// 类型
		private ProtoMemberUInt32 _id;	// id

		public Msg_Client2Logic_Compose_Req()
		{
			_type = new ProtoMemberUInt32(1, false);
			_id = new ProtoMemberUInt32(2, false);
		}

		public Msg_Client2Logic_Compose_Req(uint __type, uint __id)
		{
			_type = new ProtoMemberUInt32(1, false);
			_type.member_value = __type;
			_id = new ProtoMemberUInt32(2, false);
			_id.member_value = __id;
		}

		public uint type
		{
			get{ return _type.member_value; }
			set{ _type.member_value = value; }
		}
		public bool has_type
		{
			get{ return _type.has_value; }
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _type.Serialize(_type.member_value, ref out_stream);

			count += _id.Serialize(_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_type = 0;
			one_count = _type.ParseFrom(ref temp_type, ref int_stream);
			if (0 < one_count)
			{
					_type.member_value = temp_type;
					count = count + one_count;
			}

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Contest_Create_Req : IMessage
	{
		private ProtoMemberString _setup;	// 赛事配置
		private ProtoMemberInt32 _player_num;	// 玩家个数
		private ProtoMemberUInt32 _end_time;	//赛事结束时间

		public Msg_Client2Logic_Contest_Create_Req()
		{
			_setup = new ProtoMemberString(1, false);
			_player_num = new ProtoMemberInt32(2, false);
			_end_time = new ProtoMemberUInt32(3, false);
		}

		public Msg_Client2Logic_Contest_Create_Req(string __setup, int __player_num, uint __end_time)
		{
			_setup = new ProtoMemberString(1, false);
			_setup.member_value = __setup;
			_player_num = new ProtoMemberInt32(2, false);
			_player_num.member_value = __player_num;
			_end_time = new ProtoMemberUInt32(3, false);
			_end_time.member_value = __end_time;
		}

		public string setup
		{
			get{ return _setup.member_value; }
			set{ _setup.member_value = value; }
		}
		public bool has_setup
		{
			get{ return _setup.has_value; }
		}

		public int player_num
		{
			get{ return _player_num.member_value; }
			set{ _player_num.member_value = value; }
		}
		public bool has_player_num
		{
			get{ return _player_num.has_value; }
		}

		public uint end_time
		{
			get{ return _end_time.member_value; }
			set{ _end_time.member_value = value; }
		}
		public bool has_end_time
		{
			get{ return _end_time.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _setup.Serialize(_setup.member_value, ref out_stream);

			count += _player_num.Serialize(_player_num.member_value, ref out_stream);

			count += _end_time.Serialize(_end_time.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_setup = "";
			one_count = _setup.ParseFrom(ref temp_setup, ref int_stream);
			if (0 < one_count)
			{
					_setup.member_value = temp_setup;
					count = count + one_count;
			}

			int temp_player_num = 0;
			one_count = _player_num.ParseFrom(ref temp_player_num, ref int_stream);
			if (0 < one_count)
			{
					_player_num.member_value = temp_player_num;
					count = count + one_count;
			}

			uint temp_end_time = 0;
			one_count = _end_time.ParseFrom(ref temp_end_time, ref int_stream);
			if (0 < one_count)
			{
					_end_time.member_value = temp_end_time;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Contest_Join_Req : IMessage
	{
		private ProtoMemberUInt64 _gm_id;	// gm玩家id

		public Msg_Client2Logic_Contest_Join_Req()
		{
			_gm_id = new ProtoMemberUInt64(1, false);
		}

		public Msg_Client2Logic_Contest_Join_Req(ulong __gm_id)
		{
			_gm_id = new ProtoMemberUInt64(1, false);
			_gm_id.member_value = __gm_id;
		}

		public ulong gm_id
		{
			get{ return _gm_id.member_value; }
			set{ _gm_id.member_value = value; }
		}
		public bool has_gm_id
		{
			get{ return _gm_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _gm_id.Serialize(_gm_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_gm_id = 0;
			one_count = _gm_id.ParseFrom(ref temp_gm_id, ref int_stream);
			if (0 < one_count)
			{
					_gm_id.member_value = temp_gm_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Contest_Rank_Req : IMessage
	{
		private ProtoMemberUInt32 _contest_id;	// 赛事id
		private ProtoMemberUInt32 _from;	// 开始名次
		private ProtoMemberUInt32 _to;	// 结束名次

		public Msg_Client2Logic_Contest_Rank_Req()
		{
			_contest_id = new ProtoMemberUInt32(1, false);
			_from = new ProtoMemberUInt32(2, false);
			_to = new ProtoMemberUInt32(3, false);
		}

		public Msg_Client2Logic_Contest_Rank_Req(uint __contest_id, uint __from, uint __to)
		{
			_contest_id = new ProtoMemberUInt32(1, false);
			_contest_id.member_value = __contest_id;
			_from = new ProtoMemberUInt32(2, false);
			_from.member_value = __from;
			_to = new ProtoMemberUInt32(3, false);
			_to.member_value = __to;
		}

		public uint contest_id
		{
			get{ return _contest_id.member_value; }
			set{ _contest_id.member_value = value; }
		}
		public bool has_contest_id
		{
			get{ return _contest_id.has_value; }
		}

		public uint from
		{
			get{ return _from.member_value; }
			set{ _from.member_value = value; }
		}
		public bool has_from
		{
			get{ return _from.has_value; }
		}

		public uint to
		{
			get{ return _to.member_value; }
			set{ _to.member_value = value; }
		}
		public bool has_to
		{
			get{ return _to.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _contest_id.Serialize(_contest_id.member_value, ref out_stream);

			count += _from.Serialize(_from.member_value, ref out_stream);

			count += _to.Serialize(_to.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_contest_id = 0;
			one_count = _contest_id.ParseFrom(ref temp_contest_id, ref int_stream);
			if (0 < one_count)
			{
					_contest_id.member_value = temp_contest_id;
					count = count + one_count;
			}

			uint temp_from = 0;
			one_count = _from.ParseFrom(ref temp_from, ref int_stream);
			if (0 < one_count)
			{
					_from.member_value = temp_from;
					count = count + one_count;
			}

			uint temp_to = 0;
			one_count = _to.ParseFrom(ref temp_to, ref int_stream);
			if (0 < one_count)
			{
					_to.member_value = temp_to;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Contest_Save_Setup_Req : IMessage
	{
		private ProtoMemberString _setup;	// 赛事配置

		public Msg_Client2Logic_Contest_Save_Setup_Req()
		{
			_setup = new ProtoMemberString(1, false);
		}

		public Msg_Client2Logic_Contest_Save_Setup_Req(string __setup)
		{
			_setup = new ProtoMemberString(1, false);
			_setup.member_value = __setup;
		}

		public string setup
		{
			get{ return _setup.member_value; }
			set{ _setup.member_value = value; }
		}
		public bool has_setup
		{
			get{ return _setup.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _setup.Serialize(_setup.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_setup = "";
			one_count = _setup.ParseFrom(ref temp_setup, ref int_stream);
			if (0 < one_count)
			{
					_setup.member_value = temp_setup;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Contest_Score_Req : IMessage
	{
		private ProtoMemberUInt32 _contest_id;	// 赛事id
		private ProtoMemberUInt64 _couple_id;	// couple_id
		private ProtoMemberUInt32 _chapter_id;	//关卡id
		private ProtoMemberInt32 _score;	//分数
		private ProtoMemberUInt32 _player_hero;	// 玩家英雄
		private ProtoMemberUInt32 _player_equip;	// 玩家武器

		public Msg_Client2Logic_Contest_Score_Req()
		{
			_contest_id = new ProtoMemberUInt32(1, false);
			_couple_id = new ProtoMemberUInt64(2, false);
			_chapter_id = new ProtoMemberUInt32(3, false);
			_score = new ProtoMemberInt32(4, false);
			_player_hero = new ProtoMemberUInt32(5, false);
			_player_equip = new ProtoMemberUInt32(6, false);
		}

		public Msg_Client2Logic_Contest_Score_Req(uint __contest_id, ulong __couple_id, uint __chapter_id, int __score, uint __player_hero, uint __player_equip)
		{
			_contest_id = new ProtoMemberUInt32(1, false);
			_contest_id.member_value = __contest_id;
			_couple_id = new ProtoMemberUInt64(2, false);
			_couple_id.member_value = __couple_id;
			_chapter_id = new ProtoMemberUInt32(3, false);
			_chapter_id.member_value = __chapter_id;
			_score = new ProtoMemberInt32(4, false);
			_score.member_value = __score;
			_player_hero = new ProtoMemberUInt32(5, false);
			_player_hero.member_value = __player_hero;
			_player_equip = new ProtoMemberUInt32(6, false);
			_player_equip.member_value = __player_equip;
		}

		public uint contest_id
		{
			get{ return _contest_id.member_value; }
			set{ _contest_id.member_value = value; }
		}
		public bool has_contest_id
		{
			get{ return _contest_id.has_value; }
		}

		public ulong couple_id
		{
			get{ return _couple_id.member_value; }
			set{ _couple_id.member_value = value; }
		}
		public bool has_couple_id
		{
			get{ return _couple_id.has_value; }
		}

		public uint chapter_id
		{
			get{ return _chapter_id.member_value; }
			set{ _chapter_id.member_value = value; }
		}
		public bool has_chapter_id
		{
			get{ return _chapter_id.has_value; }
		}

		public int score
		{
			get{ return _score.member_value; }
			set{ _score.member_value = value; }
		}
		public bool has_score
		{
			get{ return _score.has_value; }
		}

		public uint player_hero
		{
			get{ return _player_hero.member_value; }
			set{ _player_hero.member_value = value; }
		}
		public bool has_player_hero
		{
			get{ return _player_hero.has_value; }
		}

		public uint player_equip
		{
			get{ return _player_equip.member_value; }
			set{ _player_equip.member_value = value; }
		}
		public bool has_player_equip
		{
			get{ return _player_equip.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _contest_id.Serialize(_contest_id.member_value, ref out_stream);

			count += _couple_id.Serialize(_couple_id.member_value, ref out_stream);

			count += _chapter_id.Serialize(_chapter_id.member_value, ref out_stream);

			count += _score.Serialize(_score.member_value, ref out_stream);

			count += _player_hero.Serialize(_player_hero.member_value, ref out_stream);

			count += _player_equip.Serialize(_player_equip.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_contest_id = 0;
			one_count = _contest_id.ParseFrom(ref temp_contest_id, ref int_stream);
			if (0 < one_count)
			{
					_contest_id.member_value = temp_contest_id;
					count = count + one_count;
			}

			ulong temp_couple_id = 0;
			one_count = _couple_id.ParseFrom(ref temp_couple_id, ref int_stream);
			if (0 < one_count)
			{
					_couple_id.member_value = temp_couple_id;
					count = count + one_count;
			}

			uint temp_chapter_id = 0;
			one_count = _chapter_id.ParseFrom(ref temp_chapter_id, ref int_stream);
			if (0 < one_count)
			{
					_chapter_id.member_value = temp_chapter_id;
					count = count + one_count;
			}

			int temp_score = 0;
			one_count = _score.ParseFrom(ref temp_score, ref int_stream);
			if (0 < one_count)
			{
					_score.member_value = temp_score;
					count = count + one_count;
			}

			uint temp_player_hero = 0;
			one_count = _player_hero.ParseFrom(ref temp_player_hero, ref int_stream);
			if (0 < one_count)
			{
					_player_hero.member_value = temp_player_hero;
					count = count + one_count;
			}

			uint temp_player_equip = 0;
			one_count = _player_equip.ParseFrom(ref temp_player_equip, ref int_stream);
			if (0 < one_count)
			{
					_player_equip.member_value = temp_player_equip;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_CreateBossBattle_Req : IMessage
	{
		private ProtoMemberUInt64 _bossHP;	//boss血量
		private ProtoMemberString _setup;	// 配置
		private ProtoMemberUInt32 _endTime;	//结束时间

		public Msg_Client2Logic_CreateBossBattle_Req()
		{
			_bossHP = new ProtoMemberUInt64(1, false);
			_setup = new ProtoMemberString(2, false);
			_endTime = new ProtoMemberUInt32(3, false);
		}

		public Msg_Client2Logic_CreateBossBattle_Req(ulong __bossHP, string __setup, uint __endTime)
		{
			_bossHP = new ProtoMemberUInt64(1, false);
			_bossHP.member_value = __bossHP;
			_setup = new ProtoMemberString(2, false);
			_setup.member_value = __setup;
			_endTime = new ProtoMemberUInt32(3, false);
			_endTime.member_value = __endTime;
		}

		public ulong bossHP
		{
			get{ return _bossHP.member_value; }
			set{ _bossHP.member_value = value; }
		}
		public bool has_bossHP
		{
			get{ return _bossHP.has_value; }
		}

		public string setup
		{
			get{ return _setup.member_value; }
			set{ _setup.member_value = value; }
		}
		public bool has_setup
		{
			get{ return _setup.has_value; }
		}

		public uint endTime
		{
			get{ return _endTime.member_value; }
			set{ _endTime.member_value = value; }
		}
		public bool has_endTime
		{
			get{ return _endTime.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _bossHP.Serialize(_bossHP.member_value, ref out_stream);

			count += _setup.Serialize(_setup.member_value, ref out_stream);

			count += _endTime.Serialize(_endTime.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_bossHP = 0;
			one_count = _bossHP.ParseFrom(ref temp_bossHP, ref int_stream);
			if (0 < one_count)
			{
					_bossHP.member_value = temp_bossHP;
					count = count + one_count;
			}

			string temp_setup = "";
			one_count = _setup.ParseFrom(ref temp_setup, ref int_stream);
			if (0 < one_count)
			{
					_setup.member_value = temp_setup;
					count = count + one_count;
			}

			uint temp_endTime = 0;
			one_count = _endTime.ParseFrom(ref temp_endTime, ref int_stream);
			if (0 < one_count)
			{
					_endTime.member_value = temp_endTime;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_CreateLocalMonster_Req : IMessage
	{
		private ProtoMemberEmbedded<MapCreateMonsterInfo> _info;	

		public Msg_Client2Logic_CreateLocalMonster_Req()
		{
			_info = new ProtoMemberEmbedded<MapCreateMonsterInfo>(1, false);
			_info.member_value = new MapCreateMonsterInfo();
		}

		public MapCreateMonsterInfo info
		{
			get{ return _info.member_value as MapCreateMonsterInfo; }
			set{ _info.member_value = value; }
		}
		public bool has_info
		{
			get{ return _info.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _info.Serialize(_info.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			MapCreateMonsterInfo temp_info = new MapCreateMonsterInfo();
			one_count = _info.ParseFrom(temp_info, ref int_stream);
			if (0 < one_count)
			{
					_info.member_value = temp_info;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_CreateLocalPlayer_Req : IMessage
	{
		private ProtoMemberEmbedded<MapCreatePlayerInfo> _info;	// 玩家

		public Msg_Client2Logic_CreateLocalPlayer_Req()
		{
			_info = new ProtoMemberEmbedded<MapCreatePlayerInfo>(1, false);
			_info.member_value = new MapCreatePlayerInfo();
		}

		public MapCreatePlayerInfo info
		{
			get{ return _info.member_value as MapCreatePlayerInfo; }
			set{ _info.member_value = value; }
		}
		public bool has_info
		{
			get{ return _info.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _info.Serialize(_info.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			MapCreatePlayerInfo temp_info = new MapCreatePlayerInfo();
			one_count = _info.ParseFrom(temp_info, ref int_stream);
			if (0 < one_count)
			{
					_info.member_value = temp_info;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Create_Player_Req : IMessage
	{
		private ProtoMemberString _m_name;	// 待创建的角色名称

		public Msg_Client2Logic_Create_Player_Req()
		{
			_m_name = new ProtoMemberString(1, false);
		}

		public Msg_Client2Logic_Create_Player_Req(string __m_name)
		{
			_m_name = new ProtoMemberString(1, false);
			_m_name.member_value = __m_name;
		}

		public string m_name
		{
			get{ return _m_name.member_value; }
			set{ _m_name.member_value = value; }
		}
		public bool has_m_name
		{
			get{ return _m_name.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_name.Serialize(_m_name.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_name = "";
			one_count = _m_name.ParseFrom(ref temp_m_name, ref int_stream);
			if (0 < one_count)
			{
					_m_name.member_value = temp_m_name;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_DeleteMail_Req : IMessage
	{
		private ProtoMemberUInt32 _id;	

		public Msg_Client2Logic_DeleteMail_Req()
		{
			_id = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_DeleteMail_Req(uint __id)
		{
			_id = new ProtoMemberUInt32(1, false);
			_id.member_value = __id;
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_GM_Req : IMessage
	{
		private ProtoMemberString _m_gm_cmd;	// GM命令

		public Msg_Client2Logic_GM_Req()
		{
			_m_gm_cmd = new ProtoMemberString(1, false);
		}

		public Msg_Client2Logic_GM_Req(string __m_gm_cmd)
		{
			_m_gm_cmd = new ProtoMemberString(1, false);
			_m_gm_cmd.member_value = __m_gm_cmd;
		}

		public string m_gm_cmd
		{
			get{ return _m_gm_cmd.member_value; }
			set{ _m_gm_cmd.member_value = value; }
		}
		public bool has_m_gm_cmd
		{
			get{ return _m_gm_cmd.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_gm_cmd.Serialize(_m_gm_cmd.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_gm_cmd = "";
			one_count = _m_gm_cmd.ParseFrom(ref temp_m_gm_cmd, ref int_stream);
			if (0 < one_count)
			{
					_m_gm_cmd.member_value = temp_m_gm_cmd;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Gacha_Req : IMessage
	{
		private ProtoMemberUInt32 _type;	// 类型

		public Msg_Client2Logic_Gacha_Req()
		{
			_type = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_Gacha_Req(uint __type)
		{
			_type = new ProtoMemberUInt32(1, false);
			_type.member_value = __type;
		}

		public uint type
		{
			get{ return _type.member_value; }
			set{ _type.member_value = value; }
		}
		public bool has_type
		{
			get{ return _type.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _type.Serialize(_type.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_type = 0;
			one_count = _type.ParseFrom(ref temp_type, ref int_stream);
			if (0 < one_count)
			{
					_type.member_value = temp_type;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_GetBossBattleRank_Req : IMessage
	{
		private ProtoMemberUInt64 _bossBattleID;	//boss战ID
		private ProtoMemberUInt32 _from;	//开始名次     
		private ProtoMemberUInt32 _to;	//结束名次

		public Msg_Client2Logic_GetBossBattleRank_Req()
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_from = new ProtoMemberUInt32(2, false);
			_to = new ProtoMemberUInt32(3, false);
		}

		public Msg_Client2Logic_GetBossBattleRank_Req(ulong __bossBattleID, uint __from, uint __to)
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_bossBattleID.member_value = __bossBattleID;
			_from = new ProtoMemberUInt32(2, false);
			_from.member_value = __from;
			_to = new ProtoMemberUInt32(3, false);
			_to.member_value = __to;
		}

		public ulong bossBattleID
		{
			get{ return _bossBattleID.member_value; }
			set{ _bossBattleID.member_value = value; }
		}
		public bool has_bossBattleID
		{
			get{ return _bossBattleID.has_value; }
		}

		public uint from
		{
			get{ return _from.member_value; }
			set{ _from.member_value = value; }
		}
		public bool has_from
		{
			get{ return _from.has_value; }
		}

		public uint to
		{
			get{ return _to.member_value; }
			set{ _to.member_value = value; }
		}
		public bool has_to
		{
			get{ return _to.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _bossBattleID.Serialize(_bossBattleID.member_value, ref out_stream);

			count += _from.Serialize(_from.member_value, ref out_stream);

			count += _to.Serialize(_to.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_bossBattleID = 0;
			one_count = _bossBattleID.ParseFrom(ref temp_bossBattleID, ref int_stream);
			if (0 < one_count)
			{
					_bossBattleID.member_value = temp_bossBattleID;
					count = count + one_count;
			}

			uint temp_from = 0;
			one_count = _from.ParseFrom(ref temp_from, ref int_stream);
			if (0 < one_count)
			{
					_from.member_value = temp_from;
					count = count + one_count;
			}

			uint temp_to = 0;
			one_count = _to.ParseFrom(ref temp_to, ref int_stream);
			if (0 < one_count)
			{
					_to.member_value = temp_to;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Get_Offline_Candy_Req : IMessage
	{
		private ProtoMemberUInt32 _nums;	

		public Msg_Client2Logic_Get_Offline_Candy_Req()
		{
			_nums = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_Get_Offline_Candy_Req(uint __nums)
		{
			_nums = new ProtoMemberUInt32(1, false);
			_nums.member_value = __nums;
		}

		public uint nums
		{
			get{ return _nums.member_value; }
			set{ _nums.member_value = value; }
		}
		public bool has_nums
		{
			get{ return _nums.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _nums.Serialize(_nums.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_nums = 0;
			one_count = _nums.ParseFrom(ref temp_nums, ref int_stream);
			if (0 < one_count)
			{
					_nums.member_value = temp_nums;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_JoinBossBattle_Req : IMessage
	{
		private ProtoMemberUInt64 _gm_id;	//gm玩家id
		private ProtoMemberUInt32 _player_hero;	// 玩家英雄
		private ProtoMemberUInt32 _player_equip;	// 玩家武器

		public Msg_Client2Logic_JoinBossBattle_Req()
		{
			_gm_id = new ProtoMemberUInt64(1, false);
			_player_hero = new ProtoMemberUInt32(2, false);
			_player_equip = new ProtoMemberUInt32(3, false);
		}

		public Msg_Client2Logic_JoinBossBattle_Req(ulong __gm_id, uint __player_hero, uint __player_equip)
		{
			_gm_id = new ProtoMemberUInt64(1, false);
			_gm_id.member_value = __gm_id;
			_player_hero = new ProtoMemberUInt32(2, false);
			_player_hero.member_value = __player_hero;
			_player_equip = new ProtoMemberUInt32(3, false);
			_player_equip.member_value = __player_equip;
		}

		public ulong gm_id
		{
			get{ return _gm_id.member_value; }
			set{ _gm_id.member_value = value; }
		}
		public bool has_gm_id
		{
			get{ return _gm_id.has_value; }
		}

		public uint player_hero
		{
			get{ return _player_hero.member_value; }
			set{ _player_hero.member_value = value; }
		}
		public bool has_player_hero
		{
			get{ return _player_hero.has_value; }
		}

		public uint player_equip
		{
			get{ return _player_equip.member_value; }
			set{ _player_equip.member_value = value; }
		}
		public bool has_player_equip
		{
			get{ return _player_equip.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _gm_id.Serialize(_gm_id.member_value, ref out_stream);

			count += _player_hero.Serialize(_player_hero.member_value, ref out_stream);

			count += _player_equip.Serialize(_player_equip.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_gm_id = 0;
			one_count = _gm_id.ParseFrom(ref temp_gm_id, ref int_stream);
			if (0 < one_count)
			{
					_gm_id.member_value = temp_gm_id;
					count = count + one_count;
			}

			uint temp_player_hero = 0;
			one_count = _player_hero.ParseFrom(ref temp_player_hero, ref int_stream);
			if (0 < one_count)
			{
					_player_hero.member_value = temp_player_hero;
					count = count + one_count;
			}

			uint temp_player_equip = 0;
			one_count = _player_equip.ParseFrom(ref temp_player_equip, ref int_stream);
			if (0 < one_count)
			{
					_player_equip.member_value = temp_player_equip;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_LoadState_Req : IMessage
	{
		private ProtoMemberInt32 _loadState;	// 加载状态序号

		public Msg_Client2Logic_LoadState_Req()
		{
			_loadState = new ProtoMemberInt32(1, false);
		}

		public Msg_Client2Logic_LoadState_Req(int __loadState)
		{
			_loadState = new ProtoMemberInt32(1, false);
			_loadState.member_value = __loadState;
		}

		public int loadState
		{
			get{ return _loadState.member_value; }
			set{ _loadState.member_value = value; }
		}
		public bool has_loadState
		{
			get{ return _loadState.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _loadState.Serialize(_loadState.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			int temp_loadState = 0;
			one_count = _loadState.ParseFrom(ref temp_loadState, ref int_stream);
			if (0 < one_count)
			{
					_loadState.member_value = temp_loadState;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Log_Req : IMessage
	{
		private ProtoMemberUInt32 _m_account_id;	// 账号id
		private ProtoMemberUInt32 _m_server_id;	// 所选服id
		private ProtoMemberStringList _m_log;	// 客户端日志

		public Msg_Client2Logic_Log_Req()
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_log = new ProtoMemberStringList(3, false);
		}

		public Msg_Client2Logic_Log_Req(uint __m_account_id, uint __m_server_id)
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_account_id.member_value = __m_account_id;
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_server_id.member_value = __m_server_id;
			_m_log = new ProtoMemberStringList(3, false);
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_server_id
		{
			get{ return _m_server_id.member_value; }
			set{ _m_server_id.member_value = value; }
		}
		public bool has_m_server_id
		{
			get{ return _m_server_id.has_value; }
		}

		public System.Collections.Generic.List<string> m_log
		{
			get{ return _m_log.member_value; }
		}
		public bool has_m_log
		{
			get{ return _m_log.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_server_id.Serialize(_m_server_id.member_value, ref out_stream);

			foreach(string one_member_value in _m_log.member_value)
			{
				count += _m_log.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_server_id = 0;
			one_count = _m_server_id.ParseFrom(ref temp_m_server_id, ref int_stream);
			if (0 < one_count)
			{
					_m_server_id.member_value = temp_m_server_id;
					count = count + one_count;
			}

			while (true)
			{
				string one_member_value = "";
				one_count = _m_log.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_log.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class Msg_Client2Logic_OpenMail_Req : IMessage
	{
		private ProtoMemberUInt32 _id;	

		public Msg_Client2Logic_OpenMail_Req()
		{
			_id = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_OpenMail_Req(uint __id)
		{
			_id = new ProtoMemberUInt32(1, false);
			_id.member_value = __id;
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Open_Box_Req : IMessage
	{
		private ProtoMemberUInt32 _item_id;	// 宝箱id

		public Msg_Client2Logic_Open_Box_Req()
		{
			_item_id = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_Open_Box_Req(uint __item_id)
		{
			_item_id = new ProtoMemberUInt32(1, false);
			_item_id.member_value = __item_id;
		}

		public uint item_id
		{
			get{ return _item_id.member_value; }
			set{ _item_id.member_value = value; }
		}
		public bool has_item_id
		{
			get{ return _item_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _item_id.Serialize(_item_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_item_id = 0;
			one_count = _item_id.ParseFrom(ref temp_item_id, ref int_stream);
			if (0 < one_count)
			{
					_item_id.member_value = temp_item_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Answer_Question_Room_Req : IMessage
	{
		private ProtoMemberUInt32 _question_id;	//问题id
		private ProtoMemberBool _answer;	//是否正确

		public Msg_Client2Logic_PVE_Answer_Question_Room_Req()
		{
			_question_id = new ProtoMemberUInt32(1, false);
			_answer = new ProtoMemberBool(2, false);
		}

		public Msg_Client2Logic_PVE_Answer_Question_Room_Req(uint __question_id, bool __answer)
		{
			_question_id = new ProtoMemberUInt32(1, false);
			_question_id.member_value = __question_id;
			_answer = new ProtoMemberBool(2, false);
			_answer.member_value = __answer;
		}

		public uint question_id
		{
			get{ return _question_id.member_value; }
			set{ _question_id.member_value = value; }
		}
		public bool has_question_id
		{
			get{ return _question_id.has_value; }
		}

		public bool answer
		{
			get{ return _answer.member_value; }
			set{ _answer.member_value = value; }
		}
		public bool has_answer
		{
			get{ return _answer.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _question_id.Serialize(_question_id.member_value, ref out_stream);

			count += _answer.Serialize(_answer.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_question_id = 0;
			one_count = _question_id.ParseFrom(ref temp_question_id, ref int_stream);
			if (0 < one_count)
			{
					_question_id.member_value = temp_question_id;
					count = count + one_count;
			}

			bool temp_answer = false;
			one_count = _answer.ParseFrom(ref temp_answer, ref int_stream);
			if (0 < one_count)
			{
					_answer.member_value = temp_answer;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Create_Room_Req : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 关卡

		public Msg_Client2Logic_PVE_Create_Room_Req()
		{
			_m_id = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_PVE_Create_Room_Req(uint __m_id)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Enter_Req : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 关卡

		public Msg_Client2Logic_PVE_Enter_Req()
		{
			_m_id = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_PVE_Enter_Req(uint __m_id)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Finish_Req : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 关卡
		private ProtoMemberUInt32 _m_result;	// 战役通关信息

		public Msg_Client2Logic_PVE_Finish_Req()
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_result = new ProtoMemberUInt32(2, false);
		}

		public Msg_Client2Logic_PVE_Finish_Req(uint __m_id, uint __m_result)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
			_m_result = new ProtoMemberUInt32(2, false);
			_m_result.member_value = __m_result;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public uint m_result
		{
			get{ return _m_result.member_value; }
			set{ _m_result.member_value = value; }
		}
		public bool has_m_result
		{
			get{ return _m_result.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _m_result.Serialize(_m_result.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			uint temp_m_result = 0;
			one_count = _m_result.ParseFrom(ref temp_m_result, ref int_stream);
			if (0 < one_count)
			{
					_m_result.member_value = temp_m_result;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Join_Room_Req : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 关卡id

		public Msg_Client2Logic_PVE_Join_Room_Req()
		{
			_m_id = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_PVE_Join_Room_Req(uint __m_id)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Quit_Room_Req : IMessage
	{

		public Msg_Client2Logic_PVE_Quit_Room_Req()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Ready_Room_Req : IMessage
	{
		private ProtoMemberBool _is_ready;	//准备/取消准备

		public Msg_Client2Logic_PVE_Ready_Room_Req()
		{
			_is_ready = new ProtoMemberBool(1, false);
		}

		public Msg_Client2Logic_PVE_Ready_Room_Req(bool __is_ready)
		{
			_is_ready = new ProtoMemberBool(1, false);
			_is_ready.member_value = __is_ready;
		}

		public bool is_ready
		{
			get{ return _is_ready.member_value; }
			set{ _is_ready.member_value = value; }
		}
		public bool has_is_ready
		{
			get{ return _is_ready.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _is_ready.Serialize(_is_ready.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			bool temp_is_ready = false;
			one_count = _is_ready.ParseFrom(ref temp_is_ready, ref int_stream);
			if (0 < one_count)
			{
					_is_ready.member_value = temp_is_ready;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_RecoverHP_Req : IMessage
	{
		private ProtoMemberUInt64 _m_chapter_id;	// 关卡id（章id+回id+关id）
		private ProtoMemberEmbeddedList<PlayerPveChapterHero> _m_heros;	// 关卡参战英雄信息

		public Msg_Client2Logic_PVE_RecoverHP_Req()
		{
			_m_chapter_id = new ProtoMemberUInt64(1, false);
			_m_heros = new ProtoMemberEmbeddedList<PlayerPveChapterHero>(2, false);
		}

		public Msg_Client2Logic_PVE_RecoverHP_Req(ulong __m_chapter_id)
		{
			_m_chapter_id = new ProtoMemberUInt64(1, false);
			_m_chapter_id.member_value = __m_chapter_id;
			_m_heros = new ProtoMemberEmbeddedList<PlayerPveChapterHero>(2, false);
		}

		public ulong m_chapter_id
		{
			get{ return _m_chapter_id.member_value; }
			set{ _m_chapter_id.member_value = value; }
		}
		public bool has_m_chapter_id
		{
			get{ return _m_chapter_id.has_value; }
		}

		public System.Collections.Generic.List<PlayerPveChapterHero> m_heros
		{
			get{ return _m_heros.member_value; }
		}
		public bool has_m_heros
		{
			get{ return _m_heros.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_chapter_id.Serialize(_m_chapter_id.member_value, ref out_stream);

			foreach(PlayerPveChapterHero one_member_value in _m_heros.member_value)
			{
				count += _m_heros.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_chapter_id = 0;
			one_count = _m_chapter_id.ParseFrom(ref temp_m_chapter_id, ref int_stream);
			if (0 < one_count)
			{
					_m_chapter_id.member_value = temp_m_chapter_id;
					count = count + one_count;
			}

			while (true)
			{
				PlayerPveChapterHero one_member_value = new PlayerPveChapterHero();
				one_count = _m_heros.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_heros.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_SetGameState_Room_Req : IMessage
	{
		private ProtoMemberUInt32 _game_state;	

		public Msg_Client2Logic_PVE_SetGameState_Room_Req()
		{
			_game_state = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_PVE_SetGameState_Room_Req(uint __game_state)
		{
			_game_state = new ProtoMemberUInt32(1, false);
			_game_state.member_value = __game_state;
		}

		public uint game_state
		{
			get{ return _game_state.member_value; }
			set{ _game_state.member_value = value; }
		}
		public bool has_game_state
		{
			get{ return _game_state.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _game_state.Serialize(_game_state.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_game_state = 0;
			one_count = _game_state.ParseFrom(ref temp_game_state, ref int_stream);
			if (0 < one_count)
			{
					_game_state.member_value = temp_game_state;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_SetWinNum_Room_Req : IMessage
	{
		private ProtoMemberUInt32 _win_num;	

		public Msg_Client2Logic_PVE_SetWinNum_Room_Req()
		{
			_win_num = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_PVE_SetWinNum_Room_Req(uint __win_num)
		{
			_win_num = new ProtoMemberUInt32(1, false);
			_win_num.member_value = __win_num;
		}

		public uint win_num
		{
			get{ return _win_num.member_value; }
			set{ _win_num.member_value = value; }
		}
		public bool has_win_num
		{
			get{ return _win_num.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _win_num.Serialize(_win_num.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_win_num = 0;
			one_count = _win_num.ParseFrom(ref temp_win_num, ref int_stream);
			if (0 < one_count)
			{
					_win_num.member_value = temp_win_num;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Start_Game_Room_Req : IMessage
	{

		public Msg_Client2Logic_PVE_Start_Game_Room_Req()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_SubmitResult_Req : IMessage
	{
		private ProtoMemberEmbedded<PlayerPveChapter> _m_chapter;	// 战役通关信息

		public Msg_Client2Logic_PVE_SubmitResult_Req()
		{
			_m_chapter = new ProtoMemberEmbedded<PlayerPveChapter>(1, false);
			_m_chapter.member_value = new PlayerPveChapter();
		}

		public PlayerPveChapter m_chapter
		{
			get{ return _m_chapter.member_value as PlayerPveChapter; }
			set{ _m_chapter.member_value = value; }
		}
		public bool has_m_chapter
		{
			get{ return _m_chapter.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_chapter.Serialize(_m_chapter.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			PlayerPveChapter temp_m_chapter = new PlayerPveChapter();
			one_count = _m_chapter.ParseFrom(temp_m_chapter, ref int_stream);
			if (0 < one_count)
			{
					_m_chapter.member_value = temp_m_chapter;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Throw_Room_Req : IMessage
	{
		private ProtoMemberUInt32 _dice_num;	//筛子数

		public Msg_Client2Logic_PVE_Throw_Room_Req()
		{
			_dice_num = new ProtoMemberUInt32(1, false);
		}

		public Msg_Client2Logic_PVE_Throw_Room_Req(uint __dice_num)
		{
			_dice_num = new ProtoMemberUInt32(1, false);
			_dice_num.member_value = __dice_num;
		}

		public uint dice_num
		{
			get{ return _dice_num.member_value; }
			set{ _dice_num.member_value = value; }
		}
		public bool has_dice_num
		{
			get{ return _dice_num.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _dice_num.Serialize(_dice_num.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_dice_num = 0;
			one_count = _dice_num.ParseFrom(ref temp_dice_num, ref int_stream);
			if (0 < one_count)
			{
					_dice_num.member_value = temp_dice_num;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_PVE_Trigger_Effect_Room_Req : IMessage
	{
		private ProtoMemberEnum<RoomPosType> _type;	//格子效果类型

		public Msg_Client2Logic_PVE_Trigger_Effect_Room_Req()
		{
			_type = new ProtoMemberEnum<RoomPosType>(1, false);
		}

		public Msg_Client2Logic_PVE_Trigger_Effect_Room_Req(RoomPosType __type)
		{
			_type = new ProtoMemberEnum<RoomPosType>(1, false);
			_type.member_value = __type;
		}

		public RoomPosType type
		{
			get{ return _type.member_value; }
			set{ _type.member_value = value; }
		}
		public bool has_type
		{
			get{ return _type.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _type.Serialize((uint)_type.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_type = 0;
			one_count = _type.ParseFrom(ref temp_type, ref int_stream);
			if (0 < one_count)
			{
					_type.member_value = (RoomPosType)temp_type;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Player_Data_Req : IMessage
	{
		private ProtoMemberString _m_ip;	// 登陆ip（网关填）
		private ProtoMemberString _m_imei;	// 设备唯一标识（客户端填）

		public Msg_Client2Logic_Player_Data_Req()
		{
			_m_ip = new ProtoMemberString(2, false);
			_m_imei = new ProtoMemberString(3, false);
		}

		public Msg_Client2Logic_Player_Data_Req(string __m_ip, string __m_imei)
		{
			_m_ip = new ProtoMemberString(2, false);
			_m_ip.member_value = __m_ip;
			_m_imei = new ProtoMemberString(3, false);
			_m_imei.member_value = __m_imei;
		}

		public string m_ip
		{
			get{ return _m_ip.member_value; }
			set{ _m_ip.member_value = value; }
		}
		public bool has_m_ip
		{
			get{ return _m_ip.has_value; }
		}

		public string m_imei
		{
			get{ return _m_imei.member_value; }
			set{ _m_imei.member_value = value; }
		}
		public bool has_m_imei
		{
			get{ return _m_imei.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_ip.Serialize(_m_ip.member_value, ref out_stream);

			count += _m_imei.Serialize(_m_imei.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_ip = "";
			one_count = _m_ip.ParseFrom(ref temp_m_ip, ref int_stream);
			if (0 < one_count)
			{
					_m_ip.member_value = temp_m_ip;
					count = count + one_count;
			}

			string temp_m_imei = "";
			one_count = _m_imei.ParseFrom(ref temp_m_imei, ref int_stream);
			if (0 < one_count)
			{
					_m_imei.member_value = temp_m_imei;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Player_List_Req : IMessage
	{

		public Msg_Client2Logic_Player_List_Req()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Client2Logic_Put_Bag_Req : IMessage
	{
		private ProtoMemberUInt64 _id;	
		private ProtoMemberUInt32 _oldPos;	
		private ProtoMemberUInt32 _newPos;	

		public Msg_Client2Logic_Put_Bag_Req()
		{
			_id = new ProtoMemberUInt64(1, false);
			_oldPos = new ProtoMemberUInt32(2, false);
			_newPos = new ProtoMemberUInt32(3, false);
		}

		public Msg_Client2Logic_Put_Bag_Req(ulong __id, uint __oldPos, uint __newPos)
		{
			_id = new ProtoMemberUInt64(1, false);
			_id.member_value = __id;
			_oldPos = new ProtoMemberUInt32(2, false);
			_oldPos.member_value = __oldPos;
			_newPos = new ProtoMemberUInt32(3, false);
			_newPos.member_value = __newPos;
		}

		public ulong id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public uint oldPos
		{
			get{ return _oldPos.member_value; }
			set{ _oldPos.member_value = value; }
		}
		public bool has_oldPos
		{
			get{ return _oldPos.has_value; }
		}

		public uint newPos
		{
			get{ return _newPos.member_value; }
			set{ _newPos.member_value = value; }
		}
		public bool has_newPos
		{
			get{ return _newPos.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			count += _oldPos.Serialize(_oldPos.member_value, ref out_stream);

			count += _newPos.Serialize(_newPos.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			uint temp_oldPos = 0;
			one_count = _oldPos.ParseFrom(ref temp_oldPos, ref int_stream);
			if (0 < one_count)
			{
					_oldPos.member_value = temp_oldPos;
					count = count + one_count;
			}

			uint temp_newPos = 0;
			one_count = _newPos.ParseFrom(ref temp_newPos, ref int_stream);
			if (0 < one_count)
			{
					_newPos.member_value = temp_newPos;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Rank_Req : IMessage
	{
		private ProtoMemberUInt32 _type;	// 类型
		private ProtoMemberUInt32 _from;	// 开始名次
		private ProtoMemberUInt32 _to;	// 结束名次

		public Msg_Client2Logic_Rank_Req()
		{
			_type = new ProtoMemberUInt32(1, false);
			_from = new ProtoMemberUInt32(2, false);
			_to = new ProtoMemberUInt32(3, false);
		}

		public Msg_Client2Logic_Rank_Req(uint __type, uint __from, uint __to)
		{
			_type = new ProtoMemberUInt32(1, false);
			_type.member_value = __type;
			_from = new ProtoMemberUInt32(2, false);
			_from.member_value = __from;
			_to = new ProtoMemberUInt32(3, false);
			_to.member_value = __to;
		}

		public uint type
		{
			get{ return _type.member_value; }
			set{ _type.member_value = value; }
		}
		public bool has_type
		{
			get{ return _type.has_value; }
		}

		public uint from
		{
			get{ return _from.member_value; }
			set{ _from.member_value = value; }
		}
		public bool has_from
		{
			get{ return _from.has_value; }
		}

		public uint to
		{
			get{ return _to.member_value; }
			set{ _to.member_value = value; }
		}
		public bool has_to
		{
			get{ return _to.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _type.Serialize(_type.member_value, ref out_stream);

			count += _from.Serialize(_from.member_value, ref out_stream);

			count += _to.Serialize(_to.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_type = 0;
			one_count = _type.ParseFrom(ref temp_type, ref int_stream);
			if (0 < one_count)
			{
					_type.member_value = temp_type;
					count = count + one_count;
			}

			uint temp_from = 0;
			one_count = _from.ParseFrom(ref temp_from, ref int_stream);
			if (0 < one_count)
			{
					_from.member_value = temp_from;
					count = count + one_count;
			}

			uint temp_to = 0;
			one_count = _to.ParseFrom(ref temp_to, ref int_stream);
			if (0 < one_count)
			{
					_to.member_value = temp_to;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_SaveContestMemInfo_Req : IMessage
	{
		private ProtoMemberString _memInfo;	// 赛事数据

		public Msg_Client2Logic_SaveContestMemInfo_Req()
		{
			_memInfo = new ProtoMemberString(1, false);
		}

		public Msg_Client2Logic_SaveContestMemInfo_Req(string __memInfo)
		{
			_memInfo = new ProtoMemberString(1, false);
			_memInfo.member_value = __memInfo;
		}

		public string memInfo
		{
			get{ return _memInfo.member_value; }
			set{ _memInfo.member_value = value; }
		}
		public bool has_memInfo
		{
			get{ return _memInfo.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _memInfo.Serialize(_memInfo.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_memInfo = "";
			one_count = _memInfo.ParseFrom(ref temp_memInfo, ref int_stream);
			if (0 < one_count)
			{
					_memInfo.member_value = temp_memInfo;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_SaveTaskInfo_Req : IMessage
	{
		private ProtoMemberString _taskInfo;	// 成就数据

		public Msg_Client2Logic_SaveTaskInfo_Req()
		{
			_taskInfo = new ProtoMemberString(1, false);
		}

		public Msg_Client2Logic_SaveTaskInfo_Req(string __taskInfo)
		{
			_taskInfo = new ProtoMemberString(1, false);
			_taskInfo.member_value = __taskInfo;
		}

		public string taskInfo
		{
			get{ return _taskInfo.member_value; }
			set{ _taskInfo.member_value = value; }
		}
		public bool has_taskInfo
		{
			get{ return _taskInfo.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _taskInfo.Serialize(_taskInfo.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_taskInfo = "";
			one_count = _taskInfo.ParseFrom(ref temp_taskInfo, ref int_stream);
			if (0 < one_count)
			{
					_taskInfo.member_value = temp_taskInfo;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_ServerTime_Req : IMessage
	{

		public Msg_Client2Logic_ServerTime_Req()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Client2Logic_SetGM_Req : IMessage
	{
		private ProtoMemberInt32 _gm;	// gm

		public Msg_Client2Logic_SetGM_Req()
		{
			_gm = new ProtoMemberInt32(1, false);
		}

		public Msg_Client2Logic_SetGM_Req(int __gm)
		{
			_gm = new ProtoMemberInt32(1, false);
			_gm.member_value = __gm;
		}

		public int gm
		{
			get{ return _gm.member_value; }
			set{ _gm.member_value = value; }
		}
		public bool has_gm
		{
			get{ return _gm.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _gm.Serialize(_gm.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			int temp_gm = 0;
			one_count = _gm.ParseFrom(ref temp_gm, ref int_stream);
			if (0 < one_count)
			{
					_gm.member_value = temp_gm;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_SetPlayerName_Req : IMessage
	{
		private ProtoMemberString _m_player_name;	// 角色名

		public Msg_Client2Logic_SetPlayerName_Req()
		{
			_m_player_name = new ProtoMemberString(1, false);
		}

		public Msg_Client2Logic_SetPlayerName_Req(string __m_player_name)
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_player_name.member_value = __m_player_name;
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Client2Logic_Table_Update_Req : IMessage
	{
		private ProtoMemberString _m_table_name;	// 表格名称
		private ProtoMemberUInt32 _m_begin_bytes;	// 本次上传的起始字节
		private ProtoMemberUInt32 _m_end_bytes;	// 本次上传的结束字节
		private ProtoMemberString _m_data;	// 本次上传的表格内容
		private ProtoMemberUInt32 _m_end;	// 所有内容是否已结束，0=否，1=是

		public Msg_Client2Logic_Table_Update_Req()
		{
			_m_table_name = new ProtoMemberString(1, false);
			_m_begin_bytes = new ProtoMemberUInt32(2, false);
			_m_end_bytes = new ProtoMemberUInt32(3, false);
			_m_data = new ProtoMemberString(4, false);
			_m_end = new ProtoMemberUInt32(5, false);
		}

		public Msg_Client2Logic_Table_Update_Req(string __m_table_name, uint __m_begin_bytes, uint __m_end_bytes, string __m_data, uint __m_end)
		{
			_m_table_name = new ProtoMemberString(1, false);
			_m_table_name.member_value = __m_table_name;
			_m_begin_bytes = new ProtoMemberUInt32(2, false);
			_m_begin_bytes.member_value = __m_begin_bytes;
			_m_end_bytes = new ProtoMemberUInt32(3, false);
			_m_end_bytes.member_value = __m_end_bytes;
			_m_data = new ProtoMemberString(4, false);
			_m_data.member_value = __m_data;
			_m_end = new ProtoMemberUInt32(5, false);
			_m_end.member_value = __m_end;
		}

		public string m_table_name
		{
			get{ return _m_table_name.member_value; }
			set{ _m_table_name.member_value = value; }
		}
		public bool has_m_table_name
		{
			get{ return _m_table_name.has_value; }
		}

		public uint m_begin_bytes
		{
			get{ return _m_begin_bytes.member_value; }
			set{ _m_begin_bytes.member_value = value; }
		}
		public bool has_m_begin_bytes
		{
			get{ return _m_begin_bytes.has_value; }
		}

		public uint m_end_bytes
		{
			get{ return _m_end_bytes.member_value; }
			set{ _m_end_bytes.member_value = value; }
		}
		public bool has_m_end_bytes
		{
			get{ return _m_end_bytes.has_value; }
		}

		public string m_data
		{
			get{ return _m_data.member_value; }
			set{ _m_data.member_value = value; }
		}
		public bool has_m_data
		{
			get{ return _m_data.has_value; }
		}

		public uint m_end
		{
			get{ return _m_end.member_value; }
			set{ _m_end.member_value = value; }
		}
		public bool has_m_end
		{
			get{ return _m_end.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_table_name.Serialize(_m_table_name.member_value, ref out_stream);

			count += _m_begin_bytes.Serialize(_m_begin_bytes.member_value, ref out_stream);

			count += _m_end_bytes.Serialize(_m_end_bytes.member_value, ref out_stream);

			count += _m_data.Serialize(_m_data.member_value, ref out_stream);

			count += _m_end.Serialize(_m_end.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_table_name = "";
			one_count = _m_table_name.ParseFrom(ref temp_m_table_name, ref int_stream);
			if (0 < one_count)
			{
					_m_table_name.member_value = temp_m_table_name;
					count = count + one_count;
			}

			uint temp_m_begin_bytes = 0;
			one_count = _m_begin_bytes.ParseFrom(ref temp_m_begin_bytes, ref int_stream);
			if (0 < one_count)
			{
					_m_begin_bytes.member_value = temp_m_begin_bytes;
					count = count + one_count;
			}

			uint temp_m_end_bytes = 0;
			one_count = _m_end_bytes.ParseFrom(ref temp_m_end_bytes, ref int_stream);
			if (0 < one_count)
			{
					_m_end_bytes.member_value = temp_m_end_bytes;
					count = count + one_count;
			}

			string temp_m_data = "";
			one_count = _m_data.ParseFrom(ref temp_m_data, ref int_stream);
			if (0 < one_count)
			{
					_m_data.member_value = temp_m_data;
					count = count + one_count;
			}

			uint temp_m_end = 0;
			one_count = _m_end.ParseFrom(ref temp_m_end, ref int_stream);
			if (0 < one_count)
			{
					_m_end.member_value = temp_m_end;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Connect : IMessage
	{
		private ProtoMemberUInt32 _m_type;	// 连接方的类型，表明自己的身份（对应ServerType）
		private ProtoMemberUInt32 _m_id;	// 连接方的唯一id
		private ProtoMemberString _m_ip;	// 连接方的ip
		private ProtoMemberUInt32 _m_port;	// 连接方的port

		public Msg_Connect()
		{
			_m_type = new ProtoMemberUInt32(1, true);
			_m_id = new ProtoMemberUInt32(2, false);
			_m_ip = new ProtoMemberString(3, false);
			_m_port = new ProtoMemberUInt32(4, false);
		}

		public Msg_Connect(uint __m_type, uint __m_id, string __m_ip, uint __m_port)
		{
			_m_type = new ProtoMemberUInt32(1, true);
			_m_type.member_value = __m_type;
			_m_id = new ProtoMemberUInt32(2, false);
			_m_id.member_value = __m_id;
			_m_ip = new ProtoMemberString(3, false);
			_m_ip.member_value = __m_ip;
			_m_port = new ProtoMemberUInt32(4, false);
			_m_port.member_value = __m_port;
		}

		public uint m_type
		{
			get{ return _m_type.member_value; }
			set{ _m_type.member_value = value; }
		}
		public bool has_m_type
		{
			get{ return _m_type.has_value; }
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public string m_ip
		{
			get{ return _m_ip.member_value; }
			set{ _m_ip.member_value = value; }
		}
		public bool has_m_ip
		{
			get{ return _m_ip.has_value; }
		}

		public uint m_port
		{
			get{ return _m_port.member_value; }
			set{ _m_port.member_value = value; }
		}
		public bool has_m_port
		{
			get{ return _m_port.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_type.Serialize(_m_type.member_value, ref out_stream);

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _m_ip.Serialize(_m_ip.member_value, ref out_stream);

			count += _m_port.Serialize(_m_port.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_type = 0;
			one_count = _m_type.ParseFrom(ref temp_m_type, ref int_stream);
			if (0 < one_count)
			{
					_m_type.member_value = temp_m_type;
					count = count + one_count;
			}

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			string temp_m_ip = "";
			one_count = _m_ip.ParseFrom(ref temp_m_ip, ref int_stream);
			if (0 < one_count)
			{
					_m_ip.member_value = temp_m_ip;
					count = count + one_count;
			}

			uint temp_m_port = 0;
			one_count = _m_port.ParseFrom(ref temp_m_port, ref int_stream);
			if (0 < one_count)
			{
					_m_port.member_value = temp_m_port;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Gate2Center_Connect_Req : IMessage
	{
		private ProtoMemberUInt32 _m_account_id;	// 账号id
		private ProtoMemberUInt32 _m_server_id;	// 所选服id
		private ProtoMemberUInt32 _m_key;	// 进入服务器的授权码key
		private ProtoMemberString _m_ip;	// 登陆ip（网关填）
		private ProtoMemberString _m_imei;	// 设备唯一标识（客户端填）

		public Msg_Gate2Center_Connect_Req()
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_key = new ProtoMemberUInt32(3, false);
			_m_ip = new ProtoMemberString(4, false);
			_m_imei = new ProtoMemberString(5, false);
		}

		public Msg_Gate2Center_Connect_Req(uint __m_account_id, uint __m_server_id, uint __m_key, string __m_ip, string __m_imei)
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_account_id.member_value = __m_account_id;
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_server_id.member_value = __m_server_id;
			_m_key = new ProtoMemberUInt32(3, false);
			_m_key.member_value = __m_key;
			_m_ip = new ProtoMemberString(4, false);
			_m_ip.member_value = __m_ip;
			_m_imei = new ProtoMemberString(5, false);
			_m_imei.member_value = __m_imei;
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_server_id
		{
			get{ return _m_server_id.member_value; }
			set{ _m_server_id.member_value = value; }
		}
		public bool has_m_server_id
		{
			get{ return _m_server_id.has_value; }
		}

		public uint m_key
		{
			get{ return _m_key.member_value; }
			set{ _m_key.member_value = value; }
		}
		public bool has_m_key
		{
			get{ return _m_key.has_value; }
		}

		public string m_ip
		{
			get{ return _m_ip.member_value; }
			set{ _m_ip.member_value = value; }
		}
		public bool has_m_ip
		{
			get{ return _m_ip.has_value; }
		}

		public string m_imei
		{
			get{ return _m_imei.member_value; }
			set{ _m_imei.member_value = value; }
		}
		public bool has_m_imei
		{
			get{ return _m_imei.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_server_id.Serialize(_m_server_id.member_value, ref out_stream);

			count += _m_key.Serialize(_m_key.member_value, ref out_stream);

			count += _m_ip.Serialize(_m_ip.member_value, ref out_stream);

			count += _m_imei.Serialize(_m_imei.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_server_id = 0;
			one_count = _m_server_id.ParseFrom(ref temp_m_server_id, ref int_stream);
			if (0 < one_count)
			{
					_m_server_id.member_value = temp_m_server_id;
					count = count + one_count;
			}

			uint temp_m_key = 0;
			one_count = _m_key.ParseFrom(ref temp_m_key, ref int_stream);
			if (0 < one_count)
			{
					_m_key.member_value = temp_m_key;
					count = count + one_count;
			}

			string temp_m_ip = "";
			one_count = _m_ip.ParseFrom(ref temp_m_ip, ref int_stream);
			if (0 < one_count)
			{
					_m_ip.member_value = temp_m_ip;
					count = count + one_count;
			}

			string temp_m_imei = "";
			one_count = _m_imei.ParseFrom(ref temp_m_imei, ref int_stream);
			if (0 < one_count)
			{
					_m_imei.member_value = temp_m_imei;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Gate2Client_Connect_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 进入服务器的key验证结果（LogicRes）
		private ProtoMemberString _m_ip;	// 登陆ip
		private ProtoMemberString _m_imei;	// 设备唯一标识

		public Msg_Gate2Client_Connect_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_ip = new ProtoMemberString(2, false);
			_m_imei = new ProtoMemberString(3, false);
		}

		public Msg_Gate2Client_Connect_Res(uint __m_res, string __m_ip, string __m_imei)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
			_m_ip = new ProtoMemberString(2, false);
			_m_ip.member_value = __m_ip;
			_m_imei = new ProtoMemberString(3, false);
			_m_imei.member_value = __m_imei;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public string m_ip
		{
			get{ return _m_ip.member_value; }
			set{ _m_ip.member_value = value; }
		}
		public bool has_m_ip
		{
			get{ return _m_ip.has_value; }
		}

		public string m_imei
		{
			get{ return _m_imei.member_value; }
			set{ _m_imei.member_value = value; }
		}
		public bool has_m_imei
		{
			get{ return _m_imei.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			count += _m_ip.Serialize(_m_ip.member_value, ref out_stream);

			count += _m_imei.Serialize(_m_imei.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			string temp_m_ip = "";
			one_count = _m_ip.ParseFrom(ref temp_m_ip, ref int_stream);
			if (0 < one_count)
			{
					_m_ip.member_value = temp_m_ip;
					count = count + one_count;
			}

			string temp_m_imei = "";
			one_count = _m_imei.ParseFrom(ref temp_m_imei, ref int_stream);
			if (0 < one_count)
			{
					_m_imei.member_value = temp_m_imei;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Gate2Logic_Player_Login_Req : IMessage
	{
		private ProtoMemberUInt32 _m_server_type;	// 网关类型（ServerType）
		private ProtoMemberString _m_ip;	// 登陆ip
		private ProtoMemberString _m_imei;	// 设备唯一标识

		public Msg_Gate2Logic_Player_Login_Req()
		{
			_m_server_type = new ProtoMemberUInt32(1, false);
			_m_ip = new ProtoMemberString(2, false);
			_m_imei = new ProtoMemberString(3, false);
		}

		public Msg_Gate2Logic_Player_Login_Req(uint __m_server_type, string __m_ip, string __m_imei)
		{
			_m_server_type = new ProtoMemberUInt32(1, false);
			_m_server_type.member_value = __m_server_type;
			_m_ip = new ProtoMemberString(2, false);
			_m_ip.member_value = __m_ip;
			_m_imei = new ProtoMemberString(3, false);
			_m_imei.member_value = __m_imei;
		}

		public uint m_server_type
		{
			get{ return _m_server_type.member_value; }
			set{ _m_server_type.member_value = value; }
		}
		public bool has_m_server_type
		{
			get{ return _m_server_type.has_value; }
		}

		public string m_ip
		{
			get{ return _m_ip.member_value; }
			set{ _m_ip.member_value = value; }
		}
		public bool has_m_ip
		{
			get{ return _m_ip.has_value; }
		}

		public string m_imei
		{
			get{ return _m_imei.member_value; }
			set{ _m_imei.member_value = value; }
		}
		public bool has_m_imei
		{
			get{ return _m_imei.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_server_type.Serialize(_m_server_type.member_value, ref out_stream);

			count += _m_ip.Serialize(_m_ip.member_value, ref out_stream);

			count += _m_imei.Serialize(_m_imei.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_server_type = 0;
			one_count = _m_server_type.ParseFrom(ref temp_m_server_type, ref int_stream);
			if (0 < one_count)
			{
					_m_server_type.member_value = temp_m_server_type;
					count = count + one_count;
			}

			string temp_m_ip = "";
			one_count = _m_ip.ParseFrom(ref temp_m_ip, ref int_stream);
			if (0 < one_count)
			{
					_m_ip.member_value = temp_m_ip;
					count = count + one_count;
			}

			string temp_m_imei = "";
			one_count = _m_imei.ParseFrom(ref temp_m_imei, ref int_stream);
			if (0 < one_count)
			{
					_m_imei.member_value = temp_m_imei;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Gate2Logic_Player_Logout_Req : IMessage
	{
		private ProtoMemberUInt32 _m_server_type;	// 网关类型（ServerType）

		public Msg_Gate2Logic_Player_Logout_Req()
		{
			_m_server_type = new ProtoMemberUInt32(1, false);
		}

		public Msg_Gate2Logic_Player_Logout_Req(uint __m_server_type)
		{
			_m_server_type = new ProtoMemberUInt32(1, false);
			_m_server_type.member_value = __m_server_type;
		}

		public uint m_server_type
		{
			get{ return _m_server_type.member_value; }
			set{ _m_server_type.member_value = value; }
		}
		public bool has_m_server_type
		{
			get{ return _m_server_type.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_server_type.Serialize(_m_server_type.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_server_type = 0;
			one_count = _m_server_type.ParseFrom(ref temp_m_server_type, ref int_stream);
			if (0 < one_count)
			{
					_m_server_type.member_value = temp_m_server_type;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Center_Player_Login_Req : IMessage
	{
		private ProtoMemberUInt64 _m_time_login;	// 上线时间戳

		public Msg_Logic2Center_Player_Login_Req()
		{
			_m_time_login = new ProtoMemberUInt64(1, false);
		}

		public Msg_Logic2Center_Player_Login_Req(ulong __m_time_login)
		{
			_m_time_login = new ProtoMemberUInt64(1, false);
			_m_time_login.member_value = __m_time_login;
		}

		public ulong m_time_login
		{
			get{ return _m_time_login.member_value; }
			set{ _m_time_login.member_value = value; }
		}
		public bool has_m_time_login
		{
			get{ return _m_time_login.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_time_login.Serialize(_m_time_login.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_time_login = 0;
			one_count = _m_time_login.ParseFrom(ref temp_m_time_login, ref int_stream);
			if (0 < one_count)
			{
					_m_time_login.member_value = temp_m_time_login;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Center_Player_Logout_Req : IMessage
	{
		private ProtoMemberUInt64 _m_time_logout;	// 下线时间戳

		public Msg_Logic2Center_Player_Logout_Req()
		{
			_m_time_logout = new ProtoMemberUInt64(1, false);
		}

		public Msg_Logic2Center_Player_Logout_Req(ulong __m_time_logout)
		{
			_m_time_logout = new ProtoMemberUInt64(1, false);
			_m_time_logout.member_value = __m_time_logout;
		}

		public ulong m_time_logout
		{
			get{ return _m_time_logout.member_value; }
			set{ _m_time_logout.member_value = value; }
		}
		public bool has_m_time_logout
		{
			get{ return _m_time_logout.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_time_logout.Serialize(_m_time_logout.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_time_logout = 0;
			one_count = _m_time_logout.ParseFrom(ref temp_m_time_logout, ref int_stream);
			if (0 < one_count)
			{
					_m_time_logout.member_value = temp_m_time_logout;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2ClientTravel_Come_Back_Notify : IMessage
	{

		public Msg_Logic2ClientTravel_Come_Back_Notify()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Logic2ClientTravel_Go_Out_Notify : IMessage
	{

		public Msg_Logic2ClientTravel_Go_Out_Notify()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Logic2Client_AddMailInfo_Res : IMessage
	{
		private ProtoMemberEmbedded<PlayerMailItem> _m_mail;	// 邮件

		public Msg_Logic2Client_AddMailInfo_Res()
		{
			_m_mail = new ProtoMemberEmbedded<PlayerMailItem>(1, false);
			_m_mail.member_value = new PlayerMailItem();
		}

		public PlayerMailItem m_mail
		{
			get{ return _m_mail.member_value as PlayerMailItem; }
			set{ _m_mail.member_value = value; }
		}
		public bool has_m_mail
		{
			get{ return _m_mail.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_mail.Serialize(_m_mail.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			PlayerMailItem temp_m_mail = new PlayerMailItem();
			one_count = _m_mail.ParseFrom(temp_m_mail, ref int_stream);
			if (0 < one_count)
			{
					_m_mail.member_value = temp_m_mail;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Bag_Res : IMessage
	{
		private ProtoMemberEmbedded<PlayerBag> _m_player_bag;	// 玩家背包信息

		public Msg_Logic2Client_Bag_Res()
		{
			_m_player_bag = new ProtoMemberEmbedded<PlayerBag>(1, false);
			_m_player_bag.member_value = new PlayerBag();
		}

		public PlayerBag m_player_bag
		{
			get{ return _m_player_bag.member_value as PlayerBag; }
			set{ _m_player_bag.member_value = value; }
		}
		public bool has_m_player_bag
		{
			get{ return _m_player_bag.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_bag.Serialize(_m_player_bag.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			PlayerBag temp_m_player_bag = new PlayerBag();
			one_count = _m_player_bag.ParseFrom(temp_m_player_bag, ref int_stream);
			if (0 < one_count)
			{
					_m_player_bag.member_value = temp_m_player_bag;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_BattleSettlement_Broadcast : IMessage
	{
		private ProtoMemberString _detail;	

		public Msg_Logic2Client_BattleSettlement_Broadcast()
		{
			_detail = new ProtoMemberString(1, false);
		}

		public Msg_Logic2Client_BattleSettlement_Broadcast(string __detail)
		{
			_detail = new ProtoMemberString(1, false);
			_detail.member_value = __detail;
		}

		public string detail
		{
			get{ return _detail.member_value; }
			set{ _detail.member_value = value; }
		}
		public bool has_detail
		{
			get{ return _detail.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _detail.Serialize(_detail.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_detail = "";
			one_count = _detail.ParseFrom(ref temp_detail, ref int_stream);
			if (0 < one_count)
			{
					_detail.member_value = temp_detail;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_Begin2_Broadcast : IMessage
	{
		private ProtoMemberUInt64 _m_player_id;	// 玩家id

		public Msg_Logic2Client_Battle_Begin2_Broadcast()
		{
			_m_player_id = new ProtoMemberUInt64(1, false);
		}

		public Msg_Logic2Client_Battle_Begin2_Broadcast(ulong __m_player_id)
		{
			_m_player_id = new ProtoMemberUInt64(1, false);
			_m_player_id.member_value = __m_player_id;
		}

		public ulong m_player_id
		{
			get{ return _m_player_id.member_value; }
			set{ _m_player_id.member_value = value; }
		}
		public bool has_m_player_id
		{
			get{ return _m_player_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_id.Serialize(_m_player_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_player_id = 0;
			one_count = _m_player_id.ParseFrom(ref temp_m_player_id, ref int_stream);
			if (0 < one_count)
			{
					_m_player_id.member_value = temp_m_player_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_Begin3_Broadcast : IMessage
	{
		private ProtoMemberUInt64 _m_player_id;	// 玩家id

		public Msg_Logic2Client_Battle_Begin3_Broadcast()
		{
			_m_player_id = new ProtoMemberUInt64(1, false);
		}

		public Msg_Logic2Client_Battle_Begin3_Broadcast(ulong __m_player_id)
		{
			_m_player_id = new ProtoMemberUInt64(1, false);
			_m_player_id.member_value = __m_player_id;
		}

		public ulong m_player_id
		{
			get{ return _m_player_id.member_value; }
			set{ _m_player_id.member_value = value; }
		}
		public bool has_m_player_id
		{
			get{ return _m_player_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_id.Serialize(_m_player_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_player_id = 0;
			one_count = _m_player_id.ParseFrom(ref temp_m_player_id, ref int_stream);
			if (0 < one_count)
			{
					_m_player_id.member_value = temp_m_player_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_Begin_Broadcast : IMessage
	{
		private ProtoMemberUInt64 _m_player_id;	// 玩家id

		public Msg_Logic2Client_Battle_Begin_Broadcast()
		{
			_m_player_id = new ProtoMemberUInt64(1, false);
		}

		public Msg_Logic2Client_Battle_Begin_Broadcast(ulong __m_player_id)
		{
			_m_player_id = new ProtoMemberUInt64(1, false);
			_m_player_id.member_value = __m_player_id;
		}

		public ulong m_player_id
		{
			get{ return _m_player_id.member_value; }
			set{ _m_player_id.member_value = value; }
		}
		public bool has_m_player_id
		{
			get{ return _m_player_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_id.Serialize(_m_player_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_player_id = 0;
			one_count = _m_player_id.ParseFrom(ref temp_m_player_id, ref int_stream);
			if (0 < one_count)
			{
					_m_player_id.member_value = temp_m_player_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_Data_Broadcast : IMessage
	{
		private ProtoMemberEmbedded<UnitInfo> _m_unit;	// 角色数据信息

		public Msg_Logic2Client_Battle_Data_Broadcast()
		{
			_m_unit = new ProtoMemberEmbedded<UnitInfo>(1, false);
			_m_unit.member_value = new UnitInfo();
		}

		public UnitInfo m_unit
		{
			get{ return _m_unit.member_value as UnitInfo; }
			set{ _m_unit.member_value = value; }
		}
		public bool has_m_unit
		{
			get{ return _m_unit.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_unit.Serialize(_m_unit.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			UnitInfo temp_m_unit = new UnitInfo();
			one_count = _m_unit.ParseFrom(temp_m_unit, ref int_stream);
			if (0 < one_count)
			{
					_m_unit.member_value = temp_m_unit;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_End_Broadcast : IMessage
	{
		private ProtoMemberUInt32 _m_win_camp;	// 胜利阵营

		public Msg_Logic2Client_Battle_End_Broadcast()
		{
			_m_win_camp = new ProtoMemberUInt32(1, false);
		}

		public Msg_Logic2Client_Battle_End_Broadcast(uint __m_win_camp)
		{
			_m_win_camp = new ProtoMemberUInt32(1, false);
			_m_win_camp.member_value = __m_win_camp;
		}

		public uint m_win_camp
		{
			get{ return _m_win_camp.member_value; }
			set{ _m_win_camp.member_value = value; }
		}
		public bool has_m_win_camp
		{
			get{ return _m_win_camp.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_win_camp.Serialize(_m_win_camp.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_win_camp = 0;
			one_count = _m_win_camp.ParseFrom(ref temp_m_win_camp, ref int_stream);
			if (0 < one_count)
			{
					_m_win_camp.member_value = temp_m_win_camp;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_Move_Broadcast : IMessage
	{
		private ProtoMemberEmbedded<UnitMoveInfo> _m_move;	// 移动信息

		public Msg_Logic2Client_Battle_Move_Broadcast()
		{
			_m_move = new ProtoMemberEmbedded<UnitMoveInfo>(1, false);
			_m_move.member_value = new UnitMoveInfo();
		}

		public UnitMoveInfo m_move
		{
			get{ return _m_move.member_value as UnitMoveInfo; }
			set{ _m_move.member_value = value; }
		}
		public bool has_m_move
		{
			get{ return _m_move.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_move.Serialize(_m_move.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			UnitMoveInfo temp_m_move = new UnitMoveInfo();
			one_count = _m_move.ParseFrom(temp_m_move, ref int_stream);
			if (0 < one_count)
			{
					_m_move.member_value = temp_m_move;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_RoomCreat_Res : IMessage
	{
		private ProtoMemberEnum<LogicRes> _m_res;	// 创建结果（LogicRes）
		private ProtoMemberUInt64 _m_guid;	// 房间唯一id
		private ProtoMemberUInt32 _m_chapterid;	// 关卡id
		private ProtoMemberUInt64 _m_room_type;	// 房间类型

		public Msg_Logic2Client_Battle_RoomCreat_Res()
		{
			_m_res = new ProtoMemberEnum<LogicRes>(1, false);
			_m_guid = new ProtoMemberUInt64(2, false);
			_m_chapterid = new ProtoMemberUInt32(3, false);
			_m_room_type = new ProtoMemberUInt64(4, false);
		}

		public Msg_Logic2Client_Battle_RoomCreat_Res(LogicRes __m_res, ulong __m_guid, uint __m_chapterid, ulong __m_room_type)
		{
			_m_res = new ProtoMemberEnum<LogicRes>(1, false);
			_m_res.member_value = __m_res;
			_m_guid = new ProtoMemberUInt64(2, false);
			_m_guid.member_value = __m_guid;
			_m_chapterid = new ProtoMemberUInt32(3, false);
			_m_chapterid.member_value = __m_chapterid;
			_m_room_type = new ProtoMemberUInt64(4, false);
			_m_room_type.member_value = __m_room_type;
		}

		public LogicRes m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public uint m_chapterid
		{
			get{ return _m_chapterid.member_value; }
			set{ _m_chapterid.member_value = value; }
		}
		public bool has_m_chapterid
		{
			get{ return _m_chapterid.has_value; }
		}

		public ulong m_room_type
		{
			get{ return _m_room_type.member_value; }
			set{ _m_room_type.member_value = value; }
		}
		public bool has_m_room_type
		{
			get{ return _m_room_type.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize((uint)_m_res.member_value, ref out_stream);

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			count += _m_chapterid.Serialize(_m_chapterid.member_value, ref out_stream);

			count += _m_room_type.Serialize(_m_room_type.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = (LogicRes)temp_m_res;
					count = count + one_count;
			}

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			uint temp_m_chapterid = 0;
			one_count = _m_chapterid.ParseFrom(ref temp_m_chapterid, ref int_stream);
			if (0 < one_count)
			{
					_m_chapterid.member_value = temp_m_chapterid;
					count = count + one_count;
			}

			ulong temp_m_room_type = 0;
			one_count = _m_room_type.ParseFrom(ref temp_m_room_type, ref int_stream);
			if (0 < one_count)
			{
					_m_room_type.member_value = temp_m_room_type;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_RoomExit_Res : IMessage
	{
		private ProtoMemberEnum<LogicRes> _m_res;	// 加入结果（LogicRes）
		private ProtoMemberUInt64 _m_guid;	// 房间唯一id

		public Msg_Logic2Client_Battle_RoomExit_Res()
		{
			_m_res = new ProtoMemberEnum<LogicRes>(1, false);
			_m_guid = new ProtoMemberUInt64(2, false);
		}

		public Msg_Logic2Client_Battle_RoomExit_Res(LogicRes __m_res, ulong __m_guid)
		{
			_m_res = new ProtoMemberEnum<LogicRes>(1, false);
			_m_res.member_value = __m_res;
			_m_guid = new ProtoMemberUInt64(2, false);
			_m_guid.member_value = __m_guid;
		}

		public LogicRes m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize((uint)_m_res.member_value, ref out_stream);

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = (LogicRes)temp_m_res;
					count = count + one_count;
			}

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_RoomInfo_Broadcast : IMessage
	{
		private ProtoMemberEmbedded<RoomInfo> _m_room;	// 房间信息

		public Msg_Logic2Client_Battle_RoomInfo_Broadcast()
		{
			_m_room = new ProtoMemberEmbedded<RoomInfo>(1, false);
			_m_room.member_value = new RoomInfo();
		}

		public RoomInfo m_room
		{
			get{ return _m_room.member_value as RoomInfo; }
			set{ _m_room.member_value = value; }
		}
		public bool has_m_room
		{
			get{ return _m_room.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_room.Serialize(_m_room.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			RoomInfo temp_m_room = new RoomInfo();
			one_count = _m_room.ParseFrom(temp_m_room, ref int_stream);
			if (0 < one_count)
			{
					_m_room.member_value = temp_m_room;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_RoomJoin_Res : IMessage
	{
		private ProtoMemberEnum<LogicRes> _m_res;	// 加入结果（LogicRes）
		private ProtoMemberUInt64 _m_guid;	// 房间唯一id
		private ProtoMemberUInt32 _m_chapterid;	// 关卡id

		public Msg_Logic2Client_Battle_RoomJoin_Res()
		{
			_m_res = new ProtoMemberEnum<LogicRes>(1, false);
			_m_guid = new ProtoMemberUInt64(2, false);
			_m_chapterid = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_Battle_RoomJoin_Res(LogicRes __m_res, ulong __m_guid, uint __m_chapterid)
		{
			_m_res = new ProtoMemberEnum<LogicRes>(1, false);
			_m_res.member_value = __m_res;
			_m_guid = new ProtoMemberUInt64(2, false);
			_m_guid.member_value = __m_guid;
			_m_chapterid = new ProtoMemberUInt32(3, false);
			_m_chapterid.member_value = __m_chapterid;
		}

		public LogicRes m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public uint m_chapterid
		{
			get{ return _m_chapterid.member_value; }
			set{ _m_chapterid.member_value = value; }
		}
		public bool has_m_chapterid
		{
			get{ return _m_chapterid.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize((uint)_m_res.member_value, ref out_stream);

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			count += _m_chapterid.Serialize(_m_chapterid.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = (LogicRes)temp_m_res;
					count = count + one_count;
			}

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			uint temp_m_chapterid = 0;
			one_count = _m_chapterid.ParseFrom(ref temp_m_chapterid, ref int_stream);
			if (0 < one_count)
			{
					_m_chapterid.member_value = temp_m_chapterid;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_RoomList_Res : IMessage
	{
		private ProtoMemberEmbedded<RoomList> _m_rooms;	// 战斗局列表
		private ProtoMemberUInt64 _m_room_type;	// 房间类型

		public Msg_Logic2Client_Battle_RoomList_Res()
		{
			_m_rooms = new ProtoMemberEmbedded<RoomList>(1, false);
			_m_rooms.member_value = new RoomList();
			_m_room_type = new ProtoMemberUInt64(2, false);
		}

		public Msg_Logic2Client_Battle_RoomList_Res(ulong __m_room_type)
		{
			_m_rooms = new ProtoMemberEmbedded<RoomList>(1, false);
			_m_rooms.member_value = new RoomList();
			_m_room_type = new ProtoMemberUInt64(2, false);
			_m_room_type.member_value = __m_room_type;
		}

		public RoomList m_rooms
		{
			get{ return _m_rooms.member_value as RoomList; }
			set{ _m_rooms.member_value = value; }
		}
		public bool has_m_rooms
		{
			get{ return _m_rooms.has_value; }
		}

		public ulong m_room_type
		{
			get{ return _m_room_type.member_value; }
			set{ _m_room_type.member_value = value; }
		}
		public bool has_m_room_type
		{
			get{ return _m_room_type.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_rooms.Serialize(_m_rooms.member_value, ref out_stream);

			count += _m_room_type.Serialize(_m_room_type.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			RoomList temp_m_rooms = new RoomList();
			one_count = _m_rooms.ParseFrom(temp_m_rooms, ref int_stream);
			if (0 < one_count)
			{
					_m_rooms.member_value = temp_m_rooms;
					count = count + one_count;
			}

			ulong temp_m_room_type = 0;
			one_count = _m_room_type.ParseFrom(ref temp_m_room_type, ref int_stream);
			if (0 < one_count)
			{
					_m_room_type.member_value = temp_m_room_type;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_RoomReset_Res : IMessage
	{
		private ProtoMemberEnum<LogicRes> _m_res;	// 创建结果（LogicRes）
		private ProtoMemberUInt64 _m_guid;	// 房间唯一id
		private ProtoMemberUInt32 _m_chapterid;	// 关卡id

		public Msg_Logic2Client_Battle_RoomReset_Res()
		{
			_m_res = new ProtoMemberEnum<LogicRes>(1, false);
			_m_guid = new ProtoMemberUInt64(2, false);
			_m_chapterid = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_Battle_RoomReset_Res(LogicRes __m_res, ulong __m_guid, uint __m_chapterid)
		{
			_m_res = new ProtoMemberEnum<LogicRes>(1, false);
			_m_res.member_value = __m_res;
			_m_guid = new ProtoMemberUInt64(2, false);
			_m_guid.member_value = __m_guid;
			_m_chapterid = new ProtoMemberUInt32(3, false);
			_m_chapterid.member_value = __m_chapterid;
		}

		public LogicRes m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public uint m_chapterid
		{
			get{ return _m_chapterid.member_value; }
			set{ _m_chapterid.member_value = value; }
		}
		public bool has_m_chapterid
		{
			get{ return _m_chapterid.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize((uint)_m_res.member_value, ref out_stream);

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			count += _m_chapterid.Serialize(_m_chapterid.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = (LogicRes)temp_m_res;
					count = count + one_count;
			}

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			uint temp_m_chapterid = 0;
			one_count = _m_chapterid.ParseFrom(ref temp_m_chapterid, ref int_stream);
			if (0 < one_count)
			{
					_m_chapterid.member_value = temp_m_chapterid;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Battle_Skill_Broadcast : IMessage
	{
		private ProtoMemberEmbedded<UnitSkillInfo> _m_skill;	// 技能信息

		public Msg_Logic2Client_Battle_Skill_Broadcast()
		{
			_m_skill = new ProtoMemberEmbedded<UnitSkillInfo>(1, false);
			_m_skill.member_value = new UnitSkillInfo();
		}

		public UnitSkillInfo m_skill
		{
			get{ return _m_skill.member_value as UnitSkillInfo; }
			set{ _m_skill.member_value = value; }
		}
		public bool has_m_skill
		{
			get{ return _m_skill.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_skill.Serialize(_m_skill.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			UnitSkillInfo temp_m_skill = new UnitSkillInfo();
			one_count = _m_skill.ParseFrom(temp_m_skill, ref int_stream);
			if (0 < one_count)
			{
					_m_skill.member_value = temp_m_skill;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_BossBattleHp_Broadcast : IMessage
	{
		private ProtoMemberUInt64 _bossBattleID;	
		private ProtoMemberUInt64 _curBossHP;	//boss当前剩余血量
		private ProtoMemberString _detail;	

		public Msg_Logic2Client_BossBattleHp_Broadcast()
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_curBossHP = new ProtoMemberUInt64(2, false);
			_detail = new ProtoMemberString(3, false);
		}

		public Msg_Logic2Client_BossBattleHp_Broadcast(ulong __bossBattleID, ulong __curBossHP, string __detail)
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_bossBattleID.member_value = __bossBattleID;
			_curBossHP = new ProtoMemberUInt64(2, false);
			_curBossHP.member_value = __curBossHP;
			_detail = new ProtoMemberString(3, false);
			_detail.member_value = __detail;
		}

		public ulong bossBattleID
		{
			get{ return _bossBattleID.member_value; }
			set{ _bossBattleID.member_value = value; }
		}
		public bool has_bossBattleID
		{
			get{ return _bossBattleID.has_value; }
		}

		public ulong curBossHP
		{
			get{ return _curBossHP.member_value; }
			set{ _curBossHP.member_value = value; }
		}
		public bool has_curBossHP
		{
			get{ return _curBossHP.has_value; }
		}

		public string detail
		{
			get{ return _detail.member_value; }
			set{ _detail.member_value = value; }
		}
		public bool has_detail
		{
			get{ return _detail.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _bossBattleID.Serialize(_bossBattleID.member_value, ref out_stream);

			count += _curBossHP.Serialize(_curBossHP.member_value, ref out_stream);

			count += _detail.Serialize(_detail.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_bossBattleID = 0;
			one_count = _bossBattleID.ParseFrom(ref temp_bossBattleID, ref int_stream);
			if (0 < one_count)
			{
					_bossBattleID.member_value = temp_bossBattleID;
					count = count + one_count;
			}

			ulong temp_curBossHP = 0;
			one_count = _curBossHP.ParseFrom(ref temp_curBossHP, ref int_stream);
			if (0 < one_count)
			{
					_curBossHP.member_value = temp_curBossHP;
					count = count + one_count;
			}

			string temp_detail = "";
			one_count = _detail.ParseFrom(ref temp_detail, ref int_stream);
			if (0 < one_count)
			{
					_detail.member_value = temp_detail;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Buy_Shop_Res : IMessage
	{
		private ProtoMemberUInt32 _id;	// id
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Buy_Shop_Res()
		{
			_id = new ProtoMemberUInt32(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_Buy_Shop_Res(uint __id, uint __m_res)
		{
			_id = new ProtoMemberUInt32(1, false);
			_id.member_value = __id;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Change_Avatar_Res : IMessage
	{
		private ProtoMemberUInt32 _id;	// id
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Change_Avatar_Res()
		{
			_id = new ProtoMemberUInt32(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_Change_Avatar_Res(uint __id, uint __m_res)
		{
			_id = new ProtoMemberUInt32(1, false);
			_id.member_value = __id;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_CheckPlayerName_Res : IMessage
	{
		private ProtoMemberString _m_player_name;	// 角色名
		private ProtoMemberUInt32 _m_res;	// 验证角色名返回结果（LogicRes）

		public Msg_Logic2Client_CheckPlayerName_Res()
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_CheckPlayerName_Res(string __m_player_name, uint __m_res)
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_player_name.member_value = __m_player_name;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Compose_Res : IMessage
	{
		private ProtoMemberUInt32 _type;	// 类型
		private ProtoMemberUInt32 _id;	// id
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Compose_Res()
		{
			_type = new ProtoMemberUInt32(1, false);
			_id = new ProtoMemberUInt32(2, false);
			_m_res = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_Compose_Res(uint __type, uint __id, uint __m_res)
		{
			_type = new ProtoMemberUInt32(1, false);
			_type.member_value = __type;
			_id = new ProtoMemberUInt32(2, false);
			_id.member_value = __id;
			_m_res = new ProtoMemberUInt32(3, false);
			_m_res.member_value = __m_res;
		}

		public uint type
		{
			get{ return _type.member_value; }
			set{ _type.member_value = value; }
		}
		public bool has_type
		{
			get{ return _type.has_value; }
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _type.Serialize(_type.member_value, ref out_stream);

			count += _id.Serialize(_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_type = 0;
			one_count = _type.ParseFrom(ref temp_type, ref int_stream);
			if (0 < one_count)
			{
					_type.member_value = temp_type;
					count = count + one_count;
			}

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Contest_Create_Res : IMessage
	{
		private ProtoMemberUInt32 _contest_id;	// 赛事id
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Contest_Create_Res()
		{
			_contest_id = new ProtoMemberUInt32(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_Contest_Create_Res(uint __contest_id, uint __m_res)
		{
			_contest_id = new ProtoMemberUInt32(1, false);
			_contest_id.member_value = __contest_id;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public uint contest_id
		{
			get{ return _contest_id.member_value; }
			set{ _contest_id.member_value = value; }
		}
		public bool has_contest_id
		{
			get{ return _contest_id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _contest_id.Serialize(_contest_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_contest_id = 0;
			one_count = _contest_id.ParseFrom(ref temp_contest_id, ref int_stream);
			if (0 < one_count)
			{
					_contest_id.member_value = temp_contest_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Contest_Info_Res : IMessage
	{
		private ProtoMemberString _setup;	// 赛事配置
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）
		private ProtoMemberUInt32 _endTime;	

		public Msg_Logic2Client_Contest_Info_Res()
		{
			_setup = new ProtoMemberString(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
			_endTime = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_Contest_Info_Res(string __setup, uint __m_res, uint __endTime)
		{
			_setup = new ProtoMemberString(1, false);
			_setup.member_value = __setup;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
			_endTime = new ProtoMemberUInt32(3, false);
			_endTime.member_value = __endTime;
		}

		public string setup
		{
			get{ return _setup.member_value; }
			set{ _setup.member_value = value; }
		}
		public bool has_setup
		{
			get{ return _setup.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public uint endTime
		{
			get{ return _endTime.member_value; }
			set{ _endTime.member_value = value; }
		}
		public bool has_endTime
		{
			get{ return _endTime.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _setup.Serialize(_setup.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			count += _endTime.Serialize(_endTime.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_setup = "";
			one_count = _setup.ParseFrom(ref temp_setup, ref int_stream);
			if (0 < one_count)
			{
					_setup.member_value = temp_setup;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			uint temp_endTime = 0;
			one_count = _endTime.ParseFrom(ref temp_endTime, ref int_stream);
			if (0 < one_count)
			{
					_endTime.member_value = temp_endTime;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Contest_Join_Res : IMessage
	{
		private ProtoMemberEmbedded<ContestCoupleInfo> _info;	// 赛事信息
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Contest_Join_Res()
		{
			_info = new ProtoMemberEmbedded<ContestCoupleInfo>(1, false);
			_info.member_value = new ContestCoupleInfo();
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_Contest_Join_Res(uint __m_res)
		{
			_info = new ProtoMemberEmbedded<ContestCoupleInfo>(1, false);
			_info.member_value = new ContestCoupleInfo();
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public ContestCoupleInfo info
		{
			get{ return _info.member_value as ContestCoupleInfo; }
			set{ _info.member_value = value; }
		}
		public bool has_info
		{
			get{ return _info.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _info.Serialize(_info.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ContestCoupleInfo temp_info = new ContestCoupleInfo();
			one_count = _info.ParseFrom(temp_info, ref int_stream);
			if (0 < one_count)
			{
					_info.member_value = temp_info;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Contest_Rank_Res : IMessage
	{
		private ProtoMemberUInt32 _contest_id;	// 赛事id
		private ProtoMemberEmbeddedList<ContestCoupleInfo> _infos;	//玩家信息

		public Msg_Logic2Client_Contest_Rank_Res()
		{
			_contest_id = new ProtoMemberUInt32(1, false);
			_infos = new ProtoMemberEmbeddedList<ContestCoupleInfo>(2, false);
		}

		public Msg_Logic2Client_Contest_Rank_Res(uint __contest_id)
		{
			_contest_id = new ProtoMemberUInt32(1, false);
			_contest_id.member_value = __contest_id;
			_infos = new ProtoMemberEmbeddedList<ContestCoupleInfo>(2, false);
		}

		public uint contest_id
		{
			get{ return _contest_id.member_value; }
			set{ _contest_id.member_value = value; }
		}
		public bool has_contest_id
		{
			get{ return _contest_id.has_value; }
		}

		public System.Collections.Generic.List<ContestCoupleInfo> infos
		{
			get{ return _infos.member_value; }
		}
		public bool has_infos
		{
			get{ return _infos.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _contest_id.Serialize(_contest_id.member_value, ref out_stream);

			foreach(ContestCoupleInfo one_member_value in _infos.member_value)
			{
				count += _infos.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_contest_id = 0;
			one_count = _contest_id.ParseFrom(ref temp_contest_id, ref int_stream);
			if (0 < one_count)
			{
					_contest_id.member_value = temp_contest_id;
					count = count + one_count;
			}

			while (true)
			{
				ContestCoupleInfo one_member_value = new ContestCoupleInfo();
				one_count = _infos.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_infos.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Contest_Save_Setup_Res : IMessage
	{
		private ProtoMemberString _setup;	// 赛事配置
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Contest_Save_Setup_Res()
		{
			_setup = new ProtoMemberString(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_Contest_Save_Setup_Res(string __setup, uint __m_res)
		{
			_setup = new ProtoMemberString(1, false);
			_setup.member_value = __setup;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public string setup
		{
			get{ return _setup.member_value; }
			set{ _setup.member_value = value; }
		}
		public bool has_setup
		{
			get{ return _setup.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _setup.Serialize(_setup.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_setup = "";
			one_count = _setup.ParseFrom(ref temp_setup, ref int_stream);
			if (0 < one_count)
			{
					_setup.member_value = temp_setup;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Contest_Score_Res : IMessage
	{
		private ProtoMemberEmbedded<ContestCoupleInfo> _info;	// 赛事信息
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Contest_Score_Res()
		{
			_info = new ProtoMemberEmbedded<ContestCoupleInfo>(1, false);
			_info.member_value = new ContestCoupleInfo();
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_Contest_Score_Res(uint __m_res)
		{
			_info = new ProtoMemberEmbedded<ContestCoupleInfo>(1, false);
			_info.member_value = new ContestCoupleInfo();
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public ContestCoupleInfo info
		{
			get{ return _info.member_value as ContestCoupleInfo; }
			set{ _info.member_value = value; }
		}
		public bool has_info
		{
			get{ return _info.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _info.Serialize(_info.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ContestCoupleInfo temp_info = new ContestCoupleInfo();
			one_count = _info.ParseFrom(temp_info, ref int_stream);
			if (0 < one_count)
			{
					_info.member_value = temp_info;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_CreateBossBattle_Res : IMessage
	{
		private ProtoMemberUInt64 _bossBattleID;	
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_CreateBossBattle_Res()
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_CreateBossBattle_Res(ulong __bossBattleID, uint __m_res)
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_bossBattleID.member_value = __bossBattleID;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public ulong bossBattleID
		{
			get{ return _bossBattleID.member_value; }
			set{ _bossBattleID.member_value = value; }
		}
		public bool has_bossBattleID
		{
			get{ return _bossBattleID.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _bossBattleID.Serialize(_bossBattleID.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_bossBattleID = 0;
			one_count = _bossBattleID.ParseFrom(ref temp_bossBattleID, ref int_stream);
			if (0 < one_count)
			{
					_bossBattleID.member_value = temp_bossBattleID;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Create_Player_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 创建角色结果（LogicRes）

		public Msg_Logic2Client_Create_Player_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
		}

		public Msg_Logic2Client_Create_Player_Res(uint __m_res)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_CreatedLocalMonster_Broadcast : IMessage
	{
		private ProtoMemberEmbedded<MapCreateMonsterInfo> _info;	

		public Msg_Logic2Client_CreatedLocalMonster_Broadcast()
		{
			_info = new ProtoMemberEmbedded<MapCreateMonsterInfo>(1, false);
			_info.member_value = new MapCreateMonsterInfo();
		}

		public MapCreateMonsterInfo info
		{
			get{ return _info.member_value as MapCreateMonsterInfo; }
			set{ _info.member_value = value; }
		}
		public bool has_info
		{
			get{ return _info.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _info.Serialize(_info.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			MapCreateMonsterInfo temp_info = new MapCreateMonsterInfo();
			one_count = _info.ParseFrom(temp_info, ref int_stream);
			if (0 < one_count)
			{
					_info.member_value = temp_info;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_CreatedLocalPlayer_Broadcast : IMessage
	{
		private ProtoMemberEmbedded<MapCreatePlayerInfo> _info;	// 玩家

		public Msg_Logic2Client_CreatedLocalPlayer_Broadcast()
		{
			_info = new ProtoMemberEmbedded<MapCreatePlayerInfo>(1, false);
			_info.member_value = new MapCreatePlayerInfo();
		}

		public MapCreatePlayerInfo info
		{
			get{ return _info.member_value as MapCreatePlayerInfo; }
			set{ _info.member_value = value; }
		}
		public bool has_info
		{
			get{ return _info.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _info.Serialize(_info.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			MapCreatePlayerInfo temp_info = new MapCreatePlayerInfo();
			one_count = _info.ParseFrom(temp_info, ref int_stream);
			if (0 < one_count)
			{
					_info.member_value = temp_info;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_DeleteMail_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）
		private ProtoMemberUInt32 _id;	

		public Msg_Logic2Client_DeleteMail_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_id = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_DeleteMail_Res(uint __m_res, uint __id)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
			_id = new ProtoMemberUInt32(2, false);
			_id.member_value = __id;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			count += _id.Serialize(_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_GM_Res : IMessage
	{
		private ProtoMemberString _m_msg;	// GM命令执行结果

		public Msg_Logic2Client_GM_Res()
		{
			_m_msg = new ProtoMemberString(1, false);
		}

		public Msg_Logic2Client_GM_Res(string __m_msg)
		{
			_m_msg = new ProtoMemberString(1, false);
			_m_msg.member_value = __m_msg;
		}

		public string m_msg
		{
			get{ return _m_msg.member_value; }
			set{ _m_msg.member_value = value; }
		}
		public bool has_m_msg
		{
			get{ return _m_msg.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_msg.Serialize(_m_msg.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_msg = "";
			one_count = _m_msg.ParseFrom(ref temp_m_msg, ref int_stream);
			if (0 < one_count)
			{
					_m_msg.member_value = temp_m_msg;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Gacha_Res : IMessage
	{
		private ProtoMemberUInt32 _type;	// 类型
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Gacha_Res()
		{
			_type = new ProtoMemberUInt32(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_Gacha_Res(uint __type, uint __m_res)
		{
			_type = new ProtoMemberUInt32(1, false);
			_type.member_value = __type;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public uint type
		{
			get{ return _type.member_value; }
			set{ _type.member_value = value; }
		}
		public bool has_type
		{
			get{ return _type.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _type.Serialize(_type.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_type = 0;
			one_count = _type.ParseFrom(ref temp_type, ref int_stream);
			if (0 < one_count)
			{
					_type.member_value = temp_type;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_GetBossBattleRank_Res : IMessage
	{
		private ProtoMemberUInt64 _bossBattleID;	//boss战ID
		private ProtoMemberEmbeddedList<BossBattlePlayerInfo> _players;	
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_GetBossBattleRank_Res()
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_players = new ProtoMemberEmbeddedList<BossBattlePlayerInfo>(2, false);
			_m_res = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_GetBossBattleRank_Res(ulong __bossBattleID, uint __m_res)
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_bossBattleID.member_value = __bossBattleID;
			_players = new ProtoMemberEmbeddedList<BossBattlePlayerInfo>(2, false);
			_m_res = new ProtoMemberUInt32(3, false);
			_m_res.member_value = __m_res;
		}

		public ulong bossBattleID
		{
			get{ return _bossBattleID.member_value; }
			set{ _bossBattleID.member_value = value; }
		}
		public bool has_bossBattleID
		{
			get{ return _bossBattleID.has_value; }
		}

		public System.Collections.Generic.List<BossBattlePlayerInfo> players
		{
			get{ return _players.member_value; }
		}
		public bool has_players
		{
			get{ return _players.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _bossBattleID.Serialize(_bossBattleID.member_value, ref out_stream);

			foreach(BossBattlePlayerInfo one_member_value in _players.member_value)
			{
				count += _players.Serialize(one_member_value, ref out_stream);
			}

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_bossBattleID = 0;
			one_count = _bossBattleID.ParseFrom(ref temp_bossBattleID, ref int_stream);
			if (0 < one_count)
			{
					_bossBattleID.member_value = temp_bossBattleID;
					count = count + one_count;
			}

			while (true)
			{
				BossBattlePlayerInfo one_member_value = new BossBattlePlayerInfo();
				one_count = _players.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_players.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Get_Offline_Candy_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Get_Offline_Candy_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
		}

		public Msg_Logic2Client_Get_Offline_Candy_Res(uint __m_res)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_GmRoute_Res : IMessage
	{
		private ProtoMemberUInt32 _m_type;	// 通知类型GmRouteMsgType
		private ProtoMemberUInt64List _m_parm;	// 通知参数
		private ProtoMemberStringList _m_parm_str;	// 通知参数

		public Msg_Logic2Client_GmRoute_Res()
		{
			_m_type = new ProtoMemberUInt32(1, false);
			_m_parm = new ProtoMemberUInt64List(2, false);
			_m_parm_str = new ProtoMemberStringList(3, false);
		}

		public Msg_Logic2Client_GmRoute_Res(uint __m_type)
		{
			_m_type = new ProtoMemberUInt32(1, false);
			_m_type.member_value = __m_type;
			_m_parm = new ProtoMemberUInt64List(2, false);
			_m_parm_str = new ProtoMemberStringList(3, false);
		}

		public uint m_type
		{
			get{ return _m_type.member_value; }
			set{ _m_type.member_value = value; }
		}
		public bool has_m_type
		{
			get{ return _m_type.has_value; }
		}

		public System.Collections.Generic.List<ulong> m_parm
		{
			get{ return _m_parm.member_value; }
		}
		public bool has_m_parm
		{
			get{ return _m_parm.has_value; }
		}

		public System.Collections.Generic.List<string> m_parm_str
		{
			get{ return _m_parm_str.member_value; }
		}
		public bool has_m_parm_str
		{
			get{ return _m_parm_str.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_type.Serialize(_m_type.member_value, ref out_stream);

			foreach(ulong one_member_value in _m_parm.member_value)
			{
				count += _m_parm.Serialize(one_member_value, ref out_stream);
			}

			foreach(string one_member_value in _m_parm_str.member_value)
			{
				count += _m_parm_str.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_type = 0;
			one_count = _m_type.ParseFrom(ref temp_m_type, ref int_stream);
			if (0 < one_count)
			{
					_m_type.member_value = temp_m_type;
					count = count + one_count;
			}

			while (true)
			{
				ulong one_member_value = 0;
				one_count = _m_parm.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_parm.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			while (true)
			{
				string one_member_value = "";
				one_count = _m_parm_str.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_parm_str.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class Msg_Logic2Client_JoinBossBattle_Res : IMessage
	{
		private ProtoMemberUInt64 _bossBattleID;	
		private ProtoMemberUInt64 _curBossHP;	//boss当前剩余血量
		private ProtoMemberString _setup;	// 其他客户端用的boss配置数据
		private ProtoMemberUInt32 _battleBeginTime;	//boss战创建时的服务器时间
		private ProtoMemberUInt32 _battleEndTime;	//boss战创建时的服务器时间
		private ProtoMemberUInt32 _curServerTime;	//当前服务器时间
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_JoinBossBattle_Res()
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_curBossHP = new ProtoMemberUInt64(2, false);
			_setup = new ProtoMemberString(3, false);
			_battleBeginTime = new ProtoMemberUInt32(4, false);
			_battleEndTime = new ProtoMemberUInt32(5, false);
			_curServerTime = new ProtoMemberUInt32(6, false);
			_m_res = new ProtoMemberUInt32(7, false);
		}

		public Msg_Logic2Client_JoinBossBattle_Res(ulong __bossBattleID, ulong __curBossHP, string __setup, uint __battleBeginTime, uint __battleEndTime, uint __curServerTime, uint __m_res)
		{
			_bossBattleID = new ProtoMemberUInt64(1, false);
			_bossBattleID.member_value = __bossBattleID;
			_curBossHP = new ProtoMemberUInt64(2, false);
			_curBossHP.member_value = __curBossHP;
			_setup = new ProtoMemberString(3, false);
			_setup.member_value = __setup;
			_battleBeginTime = new ProtoMemberUInt32(4, false);
			_battleBeginTime.member_value = __battleBeginTime;
			_battleEndTime = new ProtoMemberUInt32(5, false);
			_battleEndTime.member_value = __battleEndTime;
			_curServerTime = new ProtoMemberUInt32(6, false);
			_curServerTime.member_value = __curServerTime;
			_m_res = new ProtoMemberUInt32(7, false);
			_m_res.member_value = __m_res;
		}

		public ulong bossBattleID
		{
			get{ return _bossBattleID.member_value; }
			set{ _bossBattleID.member_value = value; }
		}
		public bool has_bossBattleID
		{
			get{ return _bossBattleID.has_value; }
		}

		public ulong curBossHP
		{
			get{ return _curBossHP.member_value; }
			set{ _curBossHP.member_value = value; }
		}
		public bool has_curBossHP
		{
			get{ return _curBossHP.has_value; }
		}

		public string setup
		{
			get{ return _setup.member_value; }
			set{ _setup.member_value = value; }
		}
		public bool has_setup
		{
			get{ return _setup.has_value; }
		}

		public uint battleBeginTime
		{
			get{ return _battleBeginTime.member_value; }
			set{ _battleBeginTime.member_value = value; }
		}
		public bool has_battleBeginTime
		{
			get{ return _battleBeginTime.has_value; }
		}

		public uint battleEndTime
		{
			get{ return _battleEndTime.member_value; }
			set{ _battleEndTime.member_value = value; }
		}
		public bool has_battleEndTime
		{
			get{ return _battleEndTime.has_value; }
		}

		public uint curServerTime
		{
			get{ return _curServerTime.member_value; }
			set{ _curServerTime.member_value = value; }
		}
		public bool has_curServerTime
		{
			get{ return _curServerTime.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _bossBattleID.Serialize(_bossBattleID.member_value, ref out_stream);

			count += _curBossHP.Serialize(_curBossHP.member_value, ref out_stream);

			count += _setup.Serialize(_setup.member_value, ref out_stream);

			count += _battleBeginTime.Serialize(_battleBeginTime.member_value, ref out_stream);

			count += _battleEndTime.Serialize(_battleEndTime.member_value, ref out_stream);

			count += _curServerTime.Serialize(_curServerTime.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_bossBattleID = 0;
			one_count = _bossBattleID.ParseFrom(ref temp_bossBattleID, ref int_stream);
			if (0 < one_count)
			{
					_bossBattleID.member_value = temp_bossBattleID;
					count = count + one_count;
			}

			ulong temp_curBossHP = 0;
			one_count = _curBossHP.ParseFrom(ref temp_curBossHP, ref int_stream);
			if (0 < one_count)
			{
					_curBossHP.member_value = temp_curBossHP;
					count = count + one_count;
			}

			string temp_setup = "";
			one_count = _setup.ParseFrom(ref temp_setup, ref int_stream);
			if (0 < one_count)
			{
					_setup.member_value = temp_setup;
					count = count + one_count;
			}

			uint temp_battleBeginTime = 0;
			one_count = _battleBeginTime.ParseFrom(ref temp_battleBeginTime, ref int_stream);
			if (0 < one_count)
			{
					_battleBeginTime.member_value = temp_battleBeginTime;
					count = count + one_count;
			}

			uint temp_battleEndTime = 0;
			one_count = _battleEndTime.ParseFrom(ref temp_battleEndTime, ref int_stream);
			if (0 < one_count)
			{
					_battleEndTime.member_value = temp_battleEndTime;
					count = count + one_count;
			}

			uint temp_curServerTime = 0;
			one_count = _curServerTime.ParseFrom(ref temp_curServerTime, ref int_stream);
			if (0 < one_count)
			{
					_curServerTime.member_value = temp_curServerTime;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_LoadState_Broadcast : IMessage
	{
		private ProtoMemberUInt64 _playerID;	//玩家id（发消息的玩家ID）
		private ProtoMemberInt32 _loadState;	// 加载状态序号（和发送的参数一样）

		public Msg_Logic2Client_LoadState_Broadcast()
		{
			_playerID = new ProtoMemberUInt64(1, false);
			_loadState = new ProtoMemberInt32(2, false);
		}

		public Msg_Logic2Client_LoadState_Broadcast(ulong __playerID, int __loadState)
		{
			_playerID = new ProtoMemberUInt64(1, false);
			_playerID.member_value = __playerID;
			_loadState = new ProtoMemberInt32(2, false);
			_loadState.member_value = __loadState;
		}

		public ulong playerID
		{
			get{ return _playerID.member_value; }
			set{ _playerID.member_value = value; }
		}
		public bool has_playerID
		{
			get{ return _playerID.has_value; }
		}

		public int loadState
		{
			get{ return _loadState.member_value; }
			set{ _loadState.member_value = value; }
		}
		public bool has_loadState
		{
			get{ return _loadState.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _playerID.Serialize(_playerID.member_value, ref out_stream);

			count += _loadState.Serialize(_loadState.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_playerID = 0;
			one_count = _playerID.ParseFrom(ref temp_playerID, ref int_stream);
			if (0 < one_count)
			{
					_playerID.member_value = temp_playerID;
					count = count + one_count;
			}

			int temp_loadState = 0;
			one_count = _loadState.ParseFrom(ref temp_loadState, ref int_stream);
			if (0 < one_count)
			{
					_loadState.member_value = temp_loadState;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_One_Item_Notify : IMessage
	{
		private ProtoMemberUInt64 _id;	
		private ProtoMemberUInt32 _item_id;	
		private ProtoMemberUInt32 _item_count;	

		public Msg_Logic2Client_One_Item_Notify()
		{
			_id = new ProtoMemberUInt64(1, false);
			_item_id = new ProtoMemberUInt32(2, false);
			_item_count = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_One_Item_Notify(ulong __id, uint __item_id, uint __item_count)
		{
			_id = new ProtoMemberUInt64(1, false);
			_id.member_value = __id;
			_item_id = new ProtoMemberUInt32(2, false);
			_item_id.member_value = __item_id;
			_item_count = new ProtoMemberUInt32(3, false);
			_item_count.member_value = __item_count;
		}

		public ulong id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public uint item_id
		{
			get{ return _item_id.member_value; }
			set{ _item_id.member_value = value; }
		}
		public bool has_item_id
		{
			get{ return _item_id.has_value; }
		}

		public uint item_count
		{
			get{ return _item_count.member_value; }
			set{ _item_count.member_value = value; }
		}
		public bool has_item_count
		{
			get{ return _item_count.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			count += _item_id.Serialize(_item_id.member_value, ref out_stream);

			count += _item_count.Serialize(_item_count.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			uint temp_item_id = 0;
			one_count = _item_id.ParseFrom(ref temp_item_id, ref int_stream);
			if (0 < one_count)
			{
					_item_id.member_value = temp_item_id;
					count = count + one_count;
			}

			uint temp_item_count = 0;
			one_count = _item_count.ParseFrom(ref temp_item_count, ref int_stream);
			if (0 < one_count)
			{
					_item_count.member_value = temp_item_count;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_OpenMail_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）
		private ProtoMemberUInt32 _id;	

		public Msg_Logic2Client_OpenMail_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_id = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_OpenMail_Res(uint __m_res, uint __id)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
			_id = new ProtoMemberUInt32(2, false);
			_id.member_value = __id;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public uint id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			count += _id.Serialize(_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			uint temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Open_Box_Res : IMessage
	{
		private ProtoMemberUInt32 _item_id;	// 宝箱id
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Open_Box_Res()
		{
			_item_id = new ProtoMemberUInt32(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_Open_Box_Res(uint __item_id, uint __m_res)
		{
			_item_id = new ProtoMemberUInt32(1, false);
			_item_id.member_value = __item_id;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public uint item_id
		{
			get{ return _item_id.member_value; }
			set{ _item_id.member_value = value; }
		}
		public bool has_item_id
		{
			get{ return _item_id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _item_id.Serialize(_item_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_item_id = 0;
			one_count = _item_id.ParseFrom(ref temp_item_id, ref int_stream);
			if (0 < one_count)
			{
					_item_id.member_value = temp_item_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVEDropInfo_Res : IMessage
	{
		private ProtoMemberEmbeddedList<PlayerBagItem> _m_items;	// 掉落列表
		private ProtoMemberUInt32 _m_type;	// 掉落类型

		public Msg_Logic2Client_PVEDropInfo_Res()
		{
			_m_items = new ProtoMemberEmbeddedList<PlayerBagItem>(1, false);
			_m_type = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_PVEDropInfo_Res(uint __m_type)
		{
			_m_items = new ProtoMemberEmbeddedList<PlayerBagItem>(1, false);
			_m_type = new ProtoMemberUInt32(2, false);
			_m_type.member_value = __m_type;
		}

		public System.Collections.Generic.List<PlayerBagItem> m_items
		{
			get{ return _m_items.member_value; }
		}
		public bool has_m_items
		{
			get{ return _m_items.has_value; }
		}

		public uint m_type
		{
			get{ return _m_type.member_value; }
			set{ _m_type.member_value = value; }
		}
		public bool has_m_type
		{
			get{ return _m_type.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			foreach(PlayerBagItem one_member_value in _m_items.member_value)
			{
				count += _m_items.Serialize(one_member_value, ref out_stream);
			}

			count += _m_type.Serialize(_m_type.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			while (true)
			{
				PlayerBagItem one_member_value = new PlayerBagItem();
				one_count = _m_items.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_items.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			uint temp_m_type = 0;
			one_count = _m_type.ParseFrom(ref temp_m_type, ref int_stream);
			if (0 < one_count)
			{
					_m_type.member_value = temp_m_type;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Answer_Question_Room_Broadcast : IMessage
	{
		private ProtoMemberUInt32 _question_id;	//问题id
		private ProtoMemberBool _answer;	//是否正确
		private ProtoMemberUInt64 _player_id;	//玩家id

		public Msg_Logic2Client_PVE_Answer_Question_Room_Broadcast()
		{
			_question_id = new ProtoMemberUInt32(1, false);
			_answer = new ProtoMemberBool(2, false);
			_player_id = new ProtoMemberUInt64(3, false);
		}

		public Msg_Logic2Client_PVE_Answer_Question_Room_Broadcast(uint __question_id, bool __answer, ulong __player_id)
		{
			_question_id = new ProtoMemberUInt32(1, false);
			_question_id.member_value = __question_id;
			_answer = new ProtoMemberBool(2, false);
			_answer.member_value = __answer;
			_player_id = new ProtoMemberUInt64(3, false);
			_player_id.member_value = __player_id;
		}

		public uint question_id
		{
			get{ return _question_id.member_value; }
			set{ _question_id.member_value = value; }
		}
		public bool has_question_id
		{
			get{ return _question_id.has_value; }
		}

		public bool answer
		{
			get{ return _answer.member_value; }
			set{ _answer.member_value = value; }
		}
		public bool has_answer
		{
			get{ return _answer.has_value; }
		}

		public ulong player_id
		{
			get{ return _player_id.member_value; }
			set{ _player_id.member_value = value; }
		}
		public bool has_player_id
		{
			get{ return _player_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _question_id.Serialize(_question_id.member_value, ref out_stream);

			count += _answer.Serialize(_answer.member_value, ref out_stream);

			count += _player_id.Serialize(_player_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_question_id = 0;
			one_count = _question_id.ParseFrom(ref temp_question_id, ref int_stream);
			if (0 < one_count)
			{
					_question_id.member_value = temp_question_id;
					count = count + one_count;
			}

			bool temp_answer = false;
			one_count = _answer.ParseFrom(ref temp_answer, ref int_stream);
			if (0 < one_count)
			{
					_answer.member_value = temp_answer;
					count = count + one_count;
			}

			ulong temp_player_id = 0;
			one_count = _player_id.ParseFrom(ref temp_player_id, ref int_stream);
			if (0 < one_count)
			{
					_player_id.member_value = temp_player_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Create_Room_Res : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 关卡
		private ProtoMemberUInt64 _room_id;	//房间id
		private ProtoMemberUInt32 _m_res;	// （LogicRes）

		public Msg_Logic2Client_PVE_Create_Room_Res()
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_room_id = new ProtoMemberUInt64(2, false);
			_m_res = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_PVE_Create_Room_Res(uint __m_id, ulong __room_id, uint __m_res)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
			_room_id = new ProtoMemberUInt64(2, false);
			_room_id.member_value = __room_id;
			_m_res = new ProtoMemberUInt32(3, false);
			_m_res.member_value = __m_res;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public ulong room_id
		{
			get{ return _room_id.member_value; }
			set{ _room_id.member_value = value; }
		}
		public bool has_room_id
		{
			get{ return _room_id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _room_id.Serialize(_room_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			ulong temp_room_id = 0;
			one_count = _room_id.ParseFrom(ref temp_room_id, ref int_stream);
			if (0 < one_count)
			{
					_room_id.member_value = temp_room_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Devil_Question_Room_Broadcast : IMessage
	{
		private ProtoMemberUInt64 _player_id;	//玩家id
		private ProtoMemberUInt32List _questions;	

		public Msg_Logic2Client_PVE_Devil_Question_Room_Broadcast()
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_questions = new ProtoMemberUInt32List(2, false);
		}

		public Msg_Logic2Client_PVE_Devil_Question_Room_Broadcast(ulong __player_id)
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_player_id.member_value = __player_id;
			_questions = new ProtoMemberUInt32List(2, false);
		}

		public ulong player_id
		{
			get{ return _player_id.member_value; }
			set{ _player_id.member_value = value; }
		}
		public bool has_player_id
		{
			get{ return _player_id.has_value; }
		}

		public System.Collections.Generic.List<uint> questions
		{
			get{ return _questions.member_value; }
		}
		public bool has_questions
		{
			get{ return _questions.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _player_id.Serialize(_player_id.member_value, ref out_stream);

			foreach(uint one_member_value in _questions.member_value)
			{
				count += _questions.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_player_id = 0;
			one_count = _player_id.ParseFrom(ref temp_player_id, ref int_stream);
			if (0 < one_count)
			{
					_player_id.member_value = temp_player_id;
					count = count + one_count;
			}

			while (true)
			{
				uint one_member_value = 0;
				one_count = _questions.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_questions.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Enter_Res : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 关卡
		private ProtoMemberUInt32 _m_res;	// （LogicRes）

		public Msg_Logic2Client_PVE_Enter_Res()
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_PVE_Enter_Res(uint __m_id, uint __m_res)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Finish_Res : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 关卡
		private ProtoMemberUInt32 _m_result;	// 战役通关信息
		private ProtoMemberUInt32 _m_res;	// （LogicRes）

		public Msg_Logic2Client_PVE_Finish_Res()
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_result = new ProtoMemberUInt32(2, false);
			_m_res = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_PVE_Finish_Res(uint __m_id, uint __m_result, uint __m_res)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
			_m_result = new ProtoMemberUInt32(2, false);
			_m_result.member_value = __m_result;
			_m_res = new ProtoMemberUInt32(3, false);
			_m_res.member_value = __m_res;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public uint m_result
		{
			get{ return _m_result.member_value; }
			set{ _m_result.member_value = value; }
		}
		public bool has_m_result
		{
			get{ return _m_result.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _m_result.Serialize(_m_result.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			uint temp_m_result = 0;
			one_count = _m_result.ParseFrom(ref temp_m_result, ref int_stream);
			if (0 < one_count)
			{
					_m_result.member_value = temp_m_result;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Join_Room_Res : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 关卡
		private ProtoMemberUInt64 _room_id;	//房间id
		private ProtoMemberUInt32 _m_res;	// （LogicRes）

		public Msg_Logic2Client_PVE_Join_Room_Res()
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_room_id = new ProtoMemberUInt64(2, false);
			_m_res = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_PVE_Join_Room_Res(uint __m_id, ulong __room_id, uint __m_res)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
			_room_id = new ProtoMemberUInt64(2, false);
			_room_id.member_value = __room_id;
			_m_res = new ProtoMemberUInt32(3, false);
			_m_res.member_value = __m_res;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public ulong room_id
		{
			get{ return _room_id.member_value; }
			set{ _room_id.member_value = value; }
		}
		public bool has_room_id
		{
			get{ return _room_id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _room_id.Serialize(_room_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			ulong temp_room_id = 0;
			one_count = _room_id.ParseFrom(ref temp_room_id, ref int_stream);
			if (0 < one_count)
			{
					_room_id.member_value = temp_room_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Move_Room_Broadcast : IMessage
	{
		private ProtoMemberUInt64 _player_id;	//玩家id
		private ProtoMemberEnum<MoveDirectionType> _type;	//移动类型
		private ProtoMemberUInt32 _num;	

		public Msg_Logic2Client_PVE_Move_Room_Broadcast()
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_type = new ProtoMemberEnum<MoveDirectionType>(2, false);
			_num = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_PVE_Move_Room_Broadcast(ulong __player_id, MoveDirectionType __type, uint __num)
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_player_id.member_value = __player_id;
			_type = new ProtoMemberEnum<MoveDirectionType>(2, false);
			_type.member_value = __type;
			_num = new ProtoMemberUInt32(3, false);
			_num.member_value = __num;
		}

		public ulong player_id
		{
			get{ return _player_id.member_value; }
			set{ _player_id.member_value = value; }
		}
		public bool has_player_id
		{
			get{ return _player_id.has_value; }
		}

		public MoveDirectionType type
		{
			get{ return _type.member_value; }
			set{ _type.member_value = value; }
		}
		public bool has_type
		{
			get{ return _type.has_value; }
		}

		public uint num
		{
			get{ return _num.member_value; }
			set{ _num.member_value = value; }
		}
		public bool has_num
		{
			get{ return _num.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _player_id.Serialize(_player_id.member_value, ref out_stream);

			count += _type.Serialize((uint)_type.member_value, ref out_stream);

			count += _num.Serialize(_num.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_player_id = 0;
			one_count = _player_id.ParseFrom(ref temp_player_id, ref int_stream);
			if (0 < one_count)
			{
					_player_id.member_value = temp_player_id;
					count = count + one_count;
			}

			uint temp_type = 0;
			one_count = _type.ParseFrom(ref temp_type, ref int_stream);
			if (0 < one_count)
			{
					_type.member_value = (MoveDirectionType)temp_type;
					count = count + one_count;
			}

			uint temp_num = 0;
			one_count = _num.ParseFrom(ref temp_num, ref int_stream);
			if (0 < one_count)
			{
					_num.member_value = temp_num;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Player_Info_Room_Broadcast : IMessage
	{
		private ProtoMemberEmbedded<PVE_Room_Player> _player;	

		public Msg_Logic2Client_PVE_Player_Info_Room_Broadcast()
		{
			_player = new ProtoMemberEmbedded<PVE_Room_Player>(1, false);
			_player.member_value = new PVE_Room_Player();
		}

		public PVE_Room_Player player
		{
			get{ return _player.member_value as PVE_Room_Player; }
			set{ _player.member_value = value; }
		}
		public bool has_player
		{
			get{ return _player.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _player.Serialize(_player.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			PVE_Room_Player temp_player = new PVE_Room_Player();
			one_count = _player.ParseFrom(temp_player, ref int_stream);
			if (0 < one_count)
			{
					_player.member_value = temp_player;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Question_Room_Broadcast : IMessage
	{
		private ProtoMemberUInt64 _player_id;	//玩家id
		private ProtoMemberUInt32 _question_id;	//问题id

		public Msg_Logic2Client_PVE_Question_Room_Broadcast()
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_question_id = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_PVE_Question_Room_Broadcast(ulong __player_id, uint __question_id)
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_player_id.member_value = __player_id;
			_question_id = new ProtoMemberUInt32(2, false);
			_question_id.member_value = __question_id;
		}

		public ulong player_id
		{
			get{ return _player_id.member_value; }
			set{ _player_id.member_value = value; }
		}
		public bool has_player_id
		{
			get{ return _player_id.has_value; }
		}

		public uint question_id
		{
			get{ return _question_id.member_value; }
			set{ _question_id.member_value = value; }
		}
		public bool has_question_id
		{
			get{ return _question_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _player_id.Serialize(_player_id.member_value, ref out_stream);

			count += _question_id.Serialize(_question_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_player_id = 0;
			one_count = _player_id.ParseFrom(ref temp_player_id, ref int_stream);
			if (0 < one_count)
			{
					_player_id.member_value = temp_player_id;
					count = count + one_count;
			}

			uint temp_question_id = 0;
			one_count = _question_id.ParseFrom(ref temp_question_id, ref int_stream);
			if (0 < one_count)
			{
					_question_id.member_value = temp_question_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_RecoverHP_Res : IMessage
	{
		private ProtoMemberUInt64 _m_chapter_id;	// 关卡id（章id+回id+关id）
		private ProtoMemberUInt32 _m_res;	// 回血结果（LogicRes）

		public Msg_Logic2Client_PVE_RecoverHP_Res()
		{
			_m_chapter_id = new ProtoMemberUInt64(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_PVE_RecoverHP_Res(ulong __m_chapter_id, uint __m_res)
		{
			_m_chapter_id = new ProtoMemberUInt64(1, false);
			_m_chapter_id.member_value = __m_chapter_id;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public ulong m_chapter_id
		{
			get{ return _m_chapter_id.member_value; }
			set{ _m_chapter_id.member_value = value; }
		}
		public bool has_m_chapter_id
		{
			get{ return _m_chapter_id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_chapter_id.Serialize(_m_chapter_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_chapter_id = 0;
			one_count = _m_chapter_id.ParseFrom(ref temp_m_chapter_id, ref int_stream);
			if (0 < one_count)
			{
					_m_chapter_id.member_value = temp_m_chapter_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Room_Broadcast : IMessage
	{
		private ProtoMemberUInt64 _room_id;	//房间id
		private ProtoMemberUInt32 _barrier_id;	//关卡id
		private ProtoMemberEmbeddedList<PVE_Room_Player> _players;	

		public Msg_Logic2Client_PVE_Room_Broadcast()
		{
			_room_id = new ProtoMemberUInt64(1, false);
			_barrier_id = new ProtoMemberUInt32(2, false);
			_players = new ProtoMemberEmbeddedList<PVE_Room_Player>(3, false);
		}

		public Msg_Logic2Client_PVE_Room_Broadcast(ulong __room_id, uint __barrier_id)
		{
			_room_id = new ProtoMemberUInt64(1, false);
			_room_id.member_value = __room_id;
			_barrier_id = new ProtoMemberUInt32(2, false);
			_barrier_id.member_value = __barrier_id;
			_players = new ProtoMemberEmbeddedList<PVE_Room_Player>(3, false);
		}

		public ulong room_id
		{
			get{ return _room_id.member_value; }
			set{ _room_id.member_value = value; }
		}
		public bool has_room_id
		{
			get{ return _room_id.has_value; }
		}

		public uint barrier_id
		{
			get{ return _barrier_id.member_value; }
			set{ _barrier_id.member_value = value; }
		}
		public bool has_barrier_id
		{
			get{ return _barrier_id.has_value; }
		}

		public System.Collections.Generic.List<PVE_Room_Player> players
		{
			get{ return _players.member_value; }
		}
		public bool has_players
		{
			get{ return _players.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _room_id.Serialize(_room_id.member_value, ref out_stream);

			count += _barrier_id.Serialize(_barrier_id.member_value, ref out_stream);

			foreach(PVE_Room_Player one_member_value in _players.member_value)
			{
				count += _players.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_room_id = 0;
			one_count = _room_id.ParseFrom(ref temp_room_id, ref int_stream);
			if (0 < one_count)
			{
					_room_id.member_value = temp_room_id;
					count = count + one_count;
			}

			uint temp_barrier_id = 0;
			one_count = _barrier_id.ParseFrom(ref temp_barrier_id, ref int_stream);
			if (0 < one_count)
			{
					_barrier_id.member_value = temp_barrier_id;
					count = count + one_count;
			}

			while (true)
			{
				PVE_Room_Player one_member_value = new PVE_Room_Player();
				one_count = _players.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_players.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Start_Game_Room_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// （LogicRes）

		public Msg_Logic2Client_PVE_Start_Game_Room_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
		}

		public Msg_Logic2Client_PVE_Start_Game_Room_Res(uint __m_res)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_SubmitResult_Res : IMessage
	{
		private ProtoMemberUInt64 _m_chapter_id;	// 关卡id（章id+回id+关id）
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_PVE_SubmitResult_Res()
		{
			_m_chapter_id = new ProtoMemberUInt64(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_PVE_SubmitResult_Res(ulong __m_chapter_id, uint __m_res)
		{
			_m_chapter_id = new ProtoMemberUInt64(1, false);
			_m_chapter_id.member_value = __m_chapter_id;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public ulong m_chapter_id
		{
			get{ return _m_chapter_id.member_value; }
			set{ _m_chapter_id.member_value = value; }
		}
		public bool has_m_chapter_id
		{
			get{ return _m_chapter_id.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_chapter_id.Serialize(_m_chapter_id.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_chapter_id = 0;
			one_count = _m_chapter_id.ParseFrom(ref temp_m_chapter_id, ref int_stream);
			if (0 < one_count)
			{
					_m_chapter_id.member_value = temp_m_chapter_id;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Throw_Room_broadcast : IMessage
	{
		private ProtoMemberUInt64 _player_id;	//玩家id
		private ProtoMemberUInt32 _dice_num;	//筛子数

		public Msg_Logic2Client_PVE_Throw_Room_broadcast()
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_dice_num = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_PVE_Throw_Room_broadcast(ulong __player_id, uint __dice_num)
		{
			_player_id = new ProtoMemberUInt64(1, false);
			_player_id.member_value = __player_id;
			_dice_num = new ProtoMemberUInt32(2, false);
			_dice_num.member_value = __dice_num;
		}

		public ulong player_id
		{
			get{ return _player_id.member_value; }
			set{ _player_id.member_value = value; }
		}
		public bool has_player_id
		{
			get{ return _player_id.has_value; }
		}

		public uint dice_num
		{
			get{ return _dice_num.member_value; }
			set{ _dice_num.member_value = value; }
		}
		public bool has_dice_num
		{
			get{ return _dice_num.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _player_id.Serialize(_player_id.member_value, ref out_stream);

			count += _dice_num.Serialize(_dice_num.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_player_id = 0;
			one_count = _player_id.ParseFrom(ref temp_player_id, ref int_stream);
			if (0 < one_count)
			{
					_player_id.member_value = temp_player_id;
					count = count + one_count;
			}

			uint temp_dice_num = 0;
			one_count = _dice_num.ParseFrom(ref temp_dice_num, ref int_stream);
			if (0 < one_count)
			{
					_dice_num.member_value = temp_dice_num;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PVE_Trigger_Effect_Room_Broadcast : IMessage
	{
		private ProtoMemberEnum<RoomPosType> _type;	//格子效果类型
		private ProtoMemberUInt64 _player_id;	//玩家id

		public Msg_Logic2Client_PVE_Trigger_Effect_Room_Broadcast()
		{
			_type = new ProtoMemberEnum<RoomPosType>(1, false);
			_player_id = new ProtoMemberUInt64(2, false);
		}

		public Msg_Logic2Client_PVE_Trigger_Effect_Room_Broadcast(RoomPosType __type, ulong __player_id)
		{
			_type = new ProtoMemberEnum<RoomPosType>(1, false);
			_type.member_value = __type;
			_player_id = new ProtoMemberUInt64(2, false);
			_player_id.member_value = __player_id;
		}

		public RoomPosType type
		{
			get{ return _type.member_value; }
			set{ _type.member_value = value; }
		}
		public bool has_type
		{
			get{ return _type.has_value; }
		}

		public ulong player_id
		{
			get{ return _player_id.member_value; }
			set{ _player_id.member_value = value; }
		}
		public bool has_player_id
		{
			get{ return _player_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _type.Serialize((uint)_type.member_value, ref out_stream);

			count += _player_id.Serialize(_player_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_type = 0;
			one_count = _type.ParseFrom(ref temp_type, ref int_stream);
			if (0 < one_count)
			{
					_type.member_value = (RoomPosType)temp_type;
					count = count + one_count;
			}

			ulong temp_player_id = 0;
			one_count = _player_id.ParseFrom(ref temp_player_id, ref int_stream);
			if (0 < one_count)
			{
					_player_id.member_value = temp_player_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PlayerAction_Res : IMessage
	{
		private ProtoMemberEmbedded<PlayerAction> _m_player_action;	// 动作信息

		public Msg_Logic2Client_PlayerAction_Res()
		{
			_m_player_action = new ProtoMemberEmbedded<PlayerAction>(1, false);
			_m_player_action.member_value = new PlayerAction();
		}

		public PlayerAction m_player_action
		{
			get{ return _m_player_action.member_value as PlayerAction; }
			set{ _m_player_action.member_value = value; }
		}
		public bool has_m_player_action
		{
			get{ return _m_player_action.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_action.Serialize(_m_player_action.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			PlayerAction temp_m_player_action = new PlayerAction();
			one_count = _m_player_action.ParseFrom(temp_m_player_action, ref int_stream);
			if (0 < one_count)
			{
					_m_player_action.member_value = temp_m_player_action;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PlayerBase_Res : IMessage
	{
		private ProtoMemberEmbedded<PlayerBase> _m_player_base;	// 玩家信息

		public Msg_Logic2Client_PlayerBase_Res()
		{
			_m_player_base = new ProtoMemberEmbedded<PlayerBase>(1, false);
			_m_player_base.member_value = new PlayerBase();
		}

		public PlayerBase m_player_base
		{
			get{ return _m_player_base.member_value as PlayerBase; }
			set{ _m_player_base.member_value = value; }
		}
		public bool has_m_player_base
		{
			get{ return _m_player_base.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_base.Serialize(_m_player_base.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			PlayerBase temp_m_player_base = new PlayerBase();
			one_count = _m_player_base.ParseFrom(temp_m_player_base, ref int_stream);
			if (0 < one_count)
			{
					_m_player_base.member_value = temp_m_player_base;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_PlayerPVE_Res : IMessage
	{
		private ProtoMemberEmbedded<PlayerNewPve> _info;	// pve信息

		public Msg_Logic2Client_PlayerPVE_Res()
		{
			_info = new ProtoMemberEmbedded<PlayerNewPve>(1, false);
			_info.member_value = new PlayerNewPve();
		}

		public PlayerNewPve info
		{
			get{ return _info.member_value as PlayerNewPve; }
			set{ _info.member_value = value; }
		}
		public bool has_info
		{
			get{ return _info.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _info.Serialize(_info.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			PlayerNewPve temp_info = new PlayerNewPve();
			one_count = _info.ParseFrom(temp_info, ref int_stream);
			if (0 < one_count)
			{
					_info.member_value = temp_info;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Player_Data_Res : IMessage
	{
		private ProtoMemberBool _m_exists;	// 返回数据是否存在
		private ProtoMemberEmbedded<Player> _m_player;	// 返回角色信息

		public Msg_Logic2Client_Player_Data_Res()
		{
			_m_exists = new ProtoMemberBool(1, false);
			_m_player = new ProtoMemberEmbedded<Player>(2, false);
			_m_player.member_value = new Player();
		}

		public Msg_Logic2Client_Player_Data_Res(bool __m_exists)
		{
			_m_exists = new ProtoMemberBool(1, false);
			_m_exists.member_value = __m_exists;
			_m_player = new ProtoMemberEmbedded<Player>(2, false);
			_m_player.member_value = new Player();
		}

		public bool m_exists
		{
			get{ return _m_exists.member_value; }
			set{ _m_exists.member_value = value; }
		}
		public bool has_m_exists
		{
			get{ return _m_exists.has_value; }
		}

		public Player m_player
		{
			get{ return _m_player.member_value as Player; }
			set{ _m_player.member_value = value; }
		}
		public bool has_m_player
		{
			get{ return _m_player.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_exists.Serialize(_m_exists.member_value, ref out_stream);

			count += _m_player.Serialize(_m_player.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			bool temp_m_exists = false;
			one_count = _m_exists.ParseFrom(ref temp_m_exists, ref int_stream);
			if (0 < one_count)
			{
					_m_exists.member_value = temp_m_exists;
					count = count + one_count;
			}

			Player temp_m_player = new Player();
			one_count = _m_player.ParseFrom(temp_m_player, ref int_stream);
			if (0 < one_count)
			{
					_m_player.member_value = temp_m_player;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Player_List_Res : IMessage
	{
		private ProtoMemberBool _m_exists;	// 返回数据是否存在
		private ProtoMemberUInt32 _m_account_id;	// 角色id == 账号id
		private ProtoMemberUInt32 _m_server_id;	// 所属服id
		private ProtoMemberString _m_player_name;	// 玩家角色名称

		public Msg_Logic2Client_Player_List_Res()
		{
			_m_exists = new ProtoMemberBool(1, false);
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_server_id = new ProtoMemberUInt32(3, false);
			_m_player_name = new ProtoMemberString(4, false);
		}

		public Msg_Logic2Client_Player_List_Res(bool __m_exists, uint __m_account_id, uint __m_server_id, string __m_player_name)
		{
			_m_exists = new ProtoMemberBool(1, false);
			_m_exists.member_value = __m_exists;
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_account_id.member_value = __m_account_id;
			_m_server_id = new ProtoMemberUInt32(3, false);
			_m_server_id.member_value = __m_server_id;
			_m_player_name = new ProtoMemberString(4, false);
			_m_player_name.member_value = __m_player_name;
		}

		public bool m_exists
		{
			get{ return _m_exists.member_value; }
			set{ _m_exists.member_value = value; }
		}
		public bool has_m_exists
		{
			get{ return _m_exists.has_value; }
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_server_id
		{
			get{ return _m_server_id.member_value; }
			set{ _m_server_id.member_value = value; }
		}
		public bool has_m_server_id
		{
			get{ return _m_server_id.has_value; }
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_exists.Serialize(_m_exists.member_value, ref out_stream);

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_server_id.Serialize(_m_server_id.member_value, ref out_stream);

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			bool temp_m_exists = false;
			one_count = _m_exists.ParseFrom(ref temp_m_exists, ref int_stream);
			if (0 < one_count)
			{
					_m_exists.member_value = temp_m_exists;
					count = count + one_count;
			}

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_server_id = 0;
			one_count = _m_server_id.ParseFrom(ref temp_m_server_id, ref int_stream);
			if (0 < one_count)
			{
					_m_server_id.member_value = temp_m_server_id;
					count = count + one_count;
			}

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Put_Bag_Res : IMessage
	{
		private ProtoMemberUInt64 _id;	
		private ProtoMemberUInt32 _oldPos;	
		private ProtoMemberUInt32 _newPos;	
		private ProtoMemberUInt32 _m_res;	

		public Msg_Logic2Client_Put_Bag_Res()
		{
			_id = new ProtoMemberUInt64(1, false);
			_oldPos = new ProtoMemberUInt32(2, false);
			_newPos = new ProtoMemberUInt32(3, false);
			_m_res = new ProtoMemberUInt32(4, false);
		}

		public Msg_Logic2Client_Put_Bag_Res(ulong __id, uint __oldPos, uint __newPos, uint __m_res)
		{
			_id = new ProtoMemberUInt64(1, false);
			_id.member_value = __id;
			_oldPos = new ProtoMemberUInt32(2, false);
			_oldPos.member_value = __oldPos;
			_newPos = new ProtoMemberUInt32(3, false);
			_newPos.member_value = __newPos;
			_m_res = new ProtoMemberUInt32(4, false);
			_m_res.member_value = __m_res;
		}

		public ulong id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public uint oldPos
		{
			get{ return _oldPos.member_value; }
			set{ _oldPos.member_value = value; }
		}
		public bool has_oldPos
		{
			get{ return _oldPos.has_value; }
		}

		public uint newPos
		{
			get{ return _newPos.member_value; }
			set{ _newPos.member_value = value; }
		}
		public bool has_newPos
		{
			get{ return _newPos.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			count += _oldPos.Serialize(_oldPos.member_value, ref out_stream);

			count += _newPos.Serialize(_newPos.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			uint temp_oldPos = 0;
			one_count = _oldPos.ParseFrom(ref temp_oldPos, ref int_stream);
			if (0 < one_count)
			{
					_oldPos.member_value = temp_oldPos;
					count = count + one_count;
			}

			uint temp_newPos = 0;
			one_count = _newPos.ParseFrom(ref temp_newPos, ref int_stream);
			if (0 < one_count)
			{
					_newPos.member_value = temp_newPos;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Rank_Res : IMessage
	{
		private ProtoMemberUInt32 _type;	// 类型
		private ProtoMemberEmbeddedList<PlayerRankInfo> _infos;	//玩家信息
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_Rank_Res()
		{
			_type = new ProtoMemberUInt32(1, false);
			_infos = new ProtoMemberEmbeddedList<PlayerRankInfo>(2, false);
			_m_res = new ProtoMemberUInt32(3, false);
		}

		public Msg_Logic2Client_Rank_Res(uint __type, uint __m_res)
		{
			_type = new ProtoMemberUInt32(1, false);
			_type.member_value = __type;
			_infos = new ProtoMemberEmbeddedList<PlayerRankInfo>(2, false);
			_m_res = new ProtoMemberUInt32(3, false);
			_m_res.member_value = __m_res;
		}

		public uint type
		{
			get{ return _type.member_value; }
			set{ _type.member_value = value; }
		}
		public bool has_type
		{
			get{ return _type.has_value; }
		}

		public System.Collections.Generic.List<PlayerRankInfo> infos
		{
			get{ return _infos.member_value; }
		}
		public bool has_infos
		{
			get{ return _infos.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _type.Serialize(_type.member_value, ref out_stream);

			foreach(PlayerRankInfo one_member_value in _infos.member_value)
			{
				count += _infos.Serialize(one_member_value, ref out_stream);
			}

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_type = 0;
			one_count = _type.ParseFrom(ref temp_type, ref int_stream);
			if (0 < one_count)
			{
					_type.member_value = temp_type;
					count = count + one_count;
			}

			while (true)
			{
				PlayerRankInfo one_member_value = new PlayerRankInfo();
				one_count = _infos.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_infos.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_SaveContestMemInfo_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_SaveContestMemInfo_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
		}

		public Msg_Logic2Client_SaveContestMemInfo_Res(uint __m_res)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_SaveTaskInfo_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 提交结果（LogicRes）

		public Msg_Logic2Client_SaveTaskInfo_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
		}

		public Msg_Logic2Client_SaveTaskInfo_Res(uint __m_res)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_ServerTime_Res : IMessage
	{
		private ProtoMemberUInt64 _m_server_time;	// 服务器时间

		public Msg_Logic2Client_ServerTime_Res()
		{
			_m_server_time = new ProtoMemberUInt64(1, false);
		}

		public Msg_Logic2Client_ServerTime_Res(ulong __m_server_time)
		{
			_m_server_time = new ProtoMemberUInt64(1, false);
			_m_server_time.member_value = __m_server_time;
		}

		public ulong m_server_time
		{
			get{ return _m_server_time.member_value; }
			set{ _m_server_time.member_value = value; }
		}
		public bool has_m_server_time
		{
			get{ return _m_server_time.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_server_time.Serialize(_m_server_time.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_server_time = 0;
			one_count = _m_server_time.ParseFrom(ref temp_m_server_time, ref int_stream);
			if (0 < one_count)
			{
					_m_server_time.member_value = temp_m_server_time;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_SetGM_Res : IMessage
	{
		private ProtoMemberInt32 _m_gm;	// gm
		private ProtoMemberUInt32 _m_res;	// 设置gm结果（LogicRes）

		public Msg_Logic2Client_SetGM_Res()
		{
			_m_gm = new ProtoMemberInt32(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_SetGM_Res(int __m_gm, uint __m_res)
		{
			_m_gm = new ProtoMemberInt32(1, false);
			_m_gm.member_value = __m_gm;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public int m_gm
		{
			get{ return _m_gm.member_value; }
			set{ _m_gm.member_value = value; }
		}
		public bool has_m_gm
		{
			get{ return _m_gm.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_gm.Serialize(_m_gm.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			int temp_m_gm = 0;
			one_count = _m_gm.ParseFrom(ref temp_m_gm, ref int_stream);
			if (0 < one_count)
			{
					_m_gm.member_value = temp_m_gm;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_SetPlayerName_Res : IMessage
	{
		private ProtoMemberString _m_player_name;	// 角色名
		private ProtoMemberUInt32 _m_res;	// 设置角色名结果（LogicRes）

		public Msg_Logic2Client_SetPlayerName_Res()
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Logic2Client_SetPlayerName_Res(string __m_player_name, uint __m_res)
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_player_name.member_value = __m_player_name;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Table_Update_Res : IMessage
	{
		private ProtoMemberString _m_table_name;	// 表格名称

		public Msg_Logic2Client_Table_Update_Res()
		{
			_m_table_name = new ProtoMemberString(1, false);
		}

		public Msg_Logic2Client_Table_Update_Res(string __m_table_name)
		{
			_m_table_name = new ProtoMemberString(1, false);
			_m_table_name.member_value = __m_table_name;
		}

		public string m_table_name
		{
			get{ return _m_table_name.member_value; }
			set{ _m_table_name.member_value = value; }
		}
		public bool has_m_table_name
		{
			get{ return _m_table_name.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_table_name.Serialize(_m_table_name.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_table_name = "";
			one_count = _m_table_name.ParseFrom(ref temp_m_table_name, ref int_stream);
			if (0 < one_count)
			{
					_m_table_name.member_value = temp_m_table_name;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Client_Travel_Bag_Res : IMessage
	{
		private ProtoMemberEmbedded<PlayerTravelBag> _m_player_bag;	// 玩家旅行背包信息

		public Msg_Logic2Client_Travel_Bag_Res()
		{
			_m_player_bag = new ProtoMemberEmbedded<PlayerTravelBag>(1, false);
			_m_player_bag.member_value = new PlayerTravelBag();
		}

		public PlayerTravelBag m_player_bag
		{
			get{ return _m_player_bag.member_value as PlayerTravelBag; }
			set{ _m_player_bag.member_value = value; }
		}
		public bool has_m_player_bag
		{
			get{ return _m_player_bag.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_bag.Serialize(_m_player_bag.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			PlayerTravelBag temp_m_player_bag = new PlayerTravelBag();
			one_count = _m_player_bag.ParseFrom(temp_m_player_bag, ref int_stream);
			if (0 < one_count)
			{
					_m_player_bag.member_value = temp_m_player_bag;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Log_Write_Req : IMessage
	{
		private ProtoMemberEmbedded<PlayerLogList> _m_logs;	//日志信息	

		public Msg_Logic2Log_Write_Req()
		{
			_m_logs = new ProtoMemberEmbedded<PlayerLogList>(1, false);
			_m_logs.member_value = new PlayerLogList();
		}

		public PlayerLogList m_logs
		{
			get{ return _m_logs.member_value as PlayerLogList; }
			set{ _m_logs.member_value = value; }
		}
		public bool has_m_logs
		{
			get{ return _m_logs.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_logs.Serialize(_m_logs.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			PlayerLogList temp_m_logs = new PlayerLogList();
			one_count = _m_logs.ParseFrom(temp_m_logs, ref int_stream);
			if (0 < one_count)
			{
					_m_logs.member_value = temp_m_logs;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Player_CheckPlayerName_Req : IMessage
	{
		private ProtoMemberString _m_player_name;	// 角色名

		public Msg_Logic2Player_CheckPlayerName_Req()
		{
			_m_player_name = new ProtoMemberString(1, false);
		}

		public Msg_Logic2Player_CheckPlayerName_Req(string __m_player_name)
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_player_name.member_value = __m_player_name;
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Player_Create_Player_Req : IMessage
	{
		private ProtoMemberEmbedded<Player> _m_player;	// 角色属性

		public Msg_Logic2Player_Create_Player_Req()
		{
			_m_player = new ProtoMemberEmbedded<Player>(1, false);
			_m_player.member_value = new Player();
		}

		public Player m_player
		{
			get{ return _m_player.member_value as Player; }
			set{ _m_player.member_value = value; }
		}
		public bool has_m_player
		{
			get{ return _m_player.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player.Serialize(_m_player.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			Player temp_m_player = new Player();
			one_count = _m_player.ParseFrom(temp_m_player, ref int_stream);
			if (0 < one_count)
			{
					_m_player.member_value = temp_m_player;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Player_Player_Info_Req : IMessage
	{
		private ProtoMemberString _m_ip;	// 登陆ip
		private ProtoMemberString _m_imei;	// 设备唯一标识

		public Msg_Logic2Player_Player_Info_Req()
		{
			_m_ip = new ProtoMemberString(2, false);
			_m_imei = new ProtoMemberString(3, false);
		}

		public Msg_Logic2Player_Player_Info_Req(string __m_ip, string __m_imei)
		{
			_m_ip = new ProtoMemberString(2, false);
			_m_ip.member_value = __m_ip;
			_m_imei = new ProtoMemberString(3, false);
			_m_imei.member_value = __m_imei;
		}

		public string m_ip
		{
			get{ return _m_ip.member_value; }
			set{ _m_ip.member_value = value; }
		}
		public bool has_m_ip
		{
			get{ return _m_ip.has_value; }
		}

		public string m_imei
		{
			get{ return _m_imei.member_value; }
			set{ _m_imei.member_value = value; }
		}
		public bool has_m_imei
		{
			get{ return _m_imei.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_ip.Serialize(_m_ip.member_value, ref out_stream);

			count += _m_imei.Serialize(_m_imei.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_ip = "";
			one_count = _m_ip.ParseFrom(ref temp_m_ip, ref int_stream);
			if (0 < one_count)
			{
					_m_ip.member_value = temp_m_ip;
					count = count + one_count;
			}

			string temp_m_imei = "";
			one_count = _m_imei.ParseFrom(ref temp_m_imei, ref int_stream);
			if (0 < one_count)
			{
					_m_imei.member_value = temp_m_imei;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Player_Player_List_Req : IMessage
	{

		public Msg_Logic2Player_Player_List_Req()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Logic2Player_Player_Update_Req : IMessage
	{
		private ProtoMemberEmbedded<Player> _m_player;	// 角色属性

		public Msg_Logic2Player_Player_Update_Req()
		{
			_m_player = new ProtoMemberEmbedded<Player>(1, false);
			_m_player.member_value = new Player();
		}

		public Player m_player
		{
			get{ return _m_player.member_value as Player; }
			set{ _m_player.member_value = value; }
		}
		public bool has_m_player
		{
			get{ return _m_player.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player.Serialize(_m_player.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			Player temp_m_player = new Player();
			one_count = _m_player.ParseFrom(temp_m_player, ref int_stream);
			if (0 < one_count)
			{
					_m_player.member_value = temp_m_player;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Player_SetGM_Req : IMessage
	{
		private ProtoMemberInt32 _gm;	// gm

		public Msg_Logic2Player_SetGM_Req()
		{
			_gm = new ProtoMemberInt32(1, false);
		}

		public Msg_Logic2Player_SetGM_Req(int __gm)
		{
			_gm = new ProtoMemberInt32(1, false);
			_gm.member_value = __gm;
		}

		public int gm
		{
			get{ return _gm.member_value; }
			set{ _gm.member_value = value; }
		}
		public bool has_gm
		{
			get{ return _gm.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _gm.Serialize(_gm.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			int temp_gm = 0;
			one_count = _gm.ParseFrom(ref temp_gm, ref int_stream);
			if (0 < one_count)
			{
					_gm.member_value = temp_gm;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Logic2Player_SetPlayerName_Req : IMessage
	{
		private ProtoMemberString _m_player_name;	// 角色名

		public Msg_Logic2Player_SetPlayerName_Req()
		{
			_m_player_name = new ProtoMemberString(1, false);
		}

		public Msg_Logic2Player_SetPlayerName_Req(string __m_player_name)
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_player_name.member_value = __m_player_name;
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_MsgBeginFlag_Account : IMessage
	{

		public Msg_MsgBeginFlag_Account()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgBeginFlag_Battle : IMessage
	{

		public Msg_MsgBeginFlag_Battle()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgBeginFlag_BossBattle : IMessage
	{

		public Msg_MsgBeginFlag_BossBattle()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgBeginFlag_Contest : IMessage
	{

		public Msg_MsgBeginFlag_Contest()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgBeginFlag_GM_Client : IMessage
	{

		public Msg_MsgBeginFlag_GM_Client()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgBeginFlag_GM_Server : IMessage
	{

		public Msg_MsgBeginFlag_GM_Server()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgBeginFlag_Log : IMessage
	{

		public Msg_MsgBeginFlag_Log()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgBeginFlag_LoginLogout : IMessage
	{

		public Msg_MsgBeginFlag_LoginLogout()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgBeginFlag_PVE : IMessage
	{

		public Msg_MsgBeginFlag_PVE()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgBeginFlag_Player : IMessage
	{

		public Msg_MsgBeginFlag_Player()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgCount : IMessage
	{

		public Msg_MsgCount()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_Account : IMessage
	{

		public Msg_MsgEndFlag_Account()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_Battle : IMessage
	{

		public Msg_MsgEndFlag_Battle()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_BossBattle : IMessage
	{

		public Msg_MsgEndFlag_BossBattle()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_Contest : IMessage
	{

		public Msg_MsgEndFlag_Contest()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_GM_Client : IMessage
	{

		public Msg_MsgEndFlag_GM_Client()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_GM_Server : IMessage
	{

		public Msg_MsgEndFlag_GM_Server()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_Log : IMessage
	{

		public Msg_MsgEndFlag_Log()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_LoginLogout : IMessage
	{

		public Msg_MsgEndFlag_LoginLogout()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_PVE : IMessage
	{

		public Msg_MsgEndFlag_PVE()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_MsgEndFlag_Player : IMessage
	{

		public Msg_MsgEndFlag_Player()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Normal : IMessage
	{

		public Msg_Normal()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Ping_Req : IMessage
	{
		private ProtoMemberUInt64 _m_ms;	// 毫秒数

		public Msg_Ping_Req()
		{
			_m_ms = new ProtoMemberUInt64(1, false);
		}

		public Msg_Ping_Req(ulong __m_ms)
		{
			_m_ms = new ProtoMemberUInt64(1, false);
			_m_ms.member_value = __m_ms;
		}

		public ulong m_ms
		{
			get{ return _m_ms.member_value; }
			set{ _m_ms.member_value = value; }
		}
		public bool has_m_ms
		{
			get{ return _m_ms.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_ms.Serialize(_m_ms.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_ms = 0;
			one_count = _m_ms.ParseFrom(ref temp_m_ms, ref int_stream);
			if (0 < one_count)
			{
					_m_ms.member_value = temp_m_ms;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Ping_Res : IMessage
	{
		private ProtoMemberUInt64 _m_ms;	// 毫秒数

		public Msg_Ping_Res()
		{
			_m_ms = new ProtoMemberUInt64(1, false);
		}

		public Msg_Ping_Res(ulong __m_ms)
		{
			_m_ms = new ProtoMemberUInt64(1, false);
			_m_ms.member_value = __m_ms;
		}

		public ulong m_ms
		{
			get{ return _m_ms.member_value; }
			set{ _m_ms.member_value = value; }
		}
		public bool has_m_ms
		{
			get{ return _m_ms.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_ms.Serialize(_m_ms.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_ms = 0;
			one_count = _m_ms.ParseFrom(ref temp_m_ms, ref int_stream);
			if (0 < one_count)
			{
					_m_ms.member_value = temp_m_ms;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Player2Logic_CheckPlayerName_Res : IMessage
	{
		private ProtoMemberString _m_player_name;	// 角色名
		private ProtoMemberUInt32 _m_res;	// 验证角色名返回结果（LogicRes）

		public Msg_Player2Logic_CheckPlayerName_Res()
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Player2Logic_CheckPlayerName_Res(string __m_player_name, uint __m_res)
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_player_name.member_value = __m_player_name;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Player2Logic_Create_Player_Res : IMessage
	{
		private ProtoMemberUInt32 _m_res;	// 角色创建结果（LogicRes）

		public Msg_Player2Logic_Create_Player_Res()
		{
			_m_res = new ProtoMemberUInt32(1, false);
		}

		public Msg_Player2Logic_Create_Player_Res(uint __m_res)
		{
			_m_res = new ProtoMemberUInt32(1, false);
			_m_res.member_value = __m_res;
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Player2Logic_Player_Info_Res : IMessage
	{
		private ProtoMemberString _m_ip;	// 登陆ip
		private ProtoMemberString _m_imei;	// 设备唯一标识
		private ProtoMemberBool _m_exists;	// 返回数据是否存在
		private ProtoMemberEmbedded<Player> _m_player;	// 返回角色信息

		public Msg_Player2Logic_Player_Info_Res()
		{
			_m_ip = new ProtoMemberString(1, false);
			_m_imei = new ProtoMemberString(2, false);
			_m_exists = new ProtoMemberBool(3, false);
			_m_player = new ProtoMemberEmbedded<Player>(4, false);
			_m_player.member_value = new Player();
		}

		public Msg_Player2Logic_Player_Info_Res(string __m_ip, string __m_imei, bool __m_exists)
		{
			_m_ip = new ProtoMemberString(1, false);
			_m_ip.member_value = __m_ip;
			_m_imei = new ProtoMemberString(2, false);
			_m_imei.member_value = __m_imei;
			_m_exists = new ProtoMemberBool(3, false);
			_m_exists.member_value = __m_exists;
			_m_player = new ProtoMemberEmbedded<Player>(4, false);
			_m_player.member_value = new Player();
		}

		public string m_ip
		{
			get{ return _m_ip.member_value; }
			set{ _m_ip.member_value = value; }
		}
		public bool has_m_ip
		{
			get{ return _m_ip.has_value; }
		}

		public string m_imei
		{
			get{ return _m_imei.member_value; }
			set{ _m_imei.member_value = value; }
		}
		public bool has_m_imei
		{
			get{ return _m_imei.has_value; }
		}

		public bool m_exists
		{
			get{ return _m_exists.member_value; }
			set{ _m_exists.member_value = value; }
		}
		public bool has_m_exists
		{
			get{ return _m_exists.has_value; }
		}

		public Player m_player
		{
			get{ return _m_player.member_value as Player; }
			set{ _m_player.member_value = value; }
		}
		public bool has_m_player
		{
			get{ return _m_player.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_ip.Serialize(_m_ip.member_value, ref out_stream);

			count += _m_imei.Serialize(_m_imei.member_value, ref out_stream);

			count += _m_exists.Serialize(_m_exists.member_value, ref out_stream);

			count += _m_player.Serialize(_m_player.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_ip = "";
			one_count = _m_ip.ParseFrom(ref temp_m_ip, ref int_stream);
			if (0 < one_count)
			{
					_m_ip.member_value = temp_m_ip;
					count = count + one_count;
			}

			string temp_m_imei = "";
			one_count = _m_imei.ParseFrom(ref temp_m_imei, ref int_stream);
			if (0 < one_count)
			{
					_m_imei.member_value = temp_m_imei;
					count = count + one_count;
			}

			bool temp_m_exists = false;
			one_count = _m_exists.ParseFrom(ref temp_m_exists, ref int_stream);
			if (0 < one_count)
			{
					_m_exists.member_value = temp_m_exists;
					count = count + one_count;
			}

			Player temp_m_player = new Player();
			one_count = _m_player.ParseFrom(temp_m_player, ref int_stream);
			if (0 < one_count)
			{
					_m_player.member_value = temp_m_player;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Player2Logic_Player_List_Res : IMessage
	{
		private ProtoMemberBool _m_exists;	// 返回数据是否存在
		private ProtoMemberUInt32 _m_account_id;	// 角色id == 账号id
		private ProtoMemberUInt32 _m_server_id;	// 所属服id
		private ProtoMemberString _m_player_name;	// 玩家角色名称

		public Msg_Player2Logic_Player_List_Res()
		{
			_m_exists = new ProtoMemberBool(1, false);
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_server_id = new ProtoMemberUInt32(3, false);
			_m_player_name = new ProtoMemberString(4, false);
		}

		public Msg_Player2Logic_Player_List_Res(bool __m_exists, uint __m_account_id, uint __m_server_id, string __m_player_name)
		{
			_m_exists = new ProtoMemberBool(1, false);
			_m_exists.member_value = __m_exists;
			_m_account_id = new ProtoMemberUInt32(2, false);
			_m_account_id.member_value = __m_account_id;
			_m_server_id = new ProtoMemberUInt32(3, false);
			_m_server_id.member_value = __m_server_id;
			_m_player_name = new ProtoMemberString(4, false);
			_m_player_name.member_value = __m_player_name;
		}

		public bool m_exists
		{
			get{ return _m_exists.member_value; }
			set{ _m_exists.member_value = value; }
		}
		public bool has_m_exists
		{
			get{ return _m_exists.has_value; }
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_server_id
		{
			get{ return _m_server_id.member_value; }
			set{ _m_server_id.member_value = value; }
		}
		public bool has_m_server_id
		{
			get{ return _m_server_id.has_value; }
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_exists.Serialize(_m_exists.member_value, ref out_stream);

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_server_id.Serialize(_m_server_id.member_value, ref out_stream);

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			bool temp_m_exists = false;
			one_count = _m_exists.ParseFrom(ref temp_m_exists, ref int_stream);
			if (0 < one_count)
			{
					_m_exists.member_value = temp_m_exists;
					count = count + one_count;
			}

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_server_id = 0;
			one_count = _m_server_id.ParseFrom(ref temp_m_server_id, ref int_stream);
			if (0 < one_count)
			{
					_m_server_id.member_value = temp_m_server_id;
					count = count + one_count;
			}

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Player2Logic_SetGM_Res : IMessage
	{
		private ProtoMemberInt32 _m_gm;	// gm
		private ProtoMemberUInt32 _m_res;	// 设置gm结果（LogicRes）

		public Msg_Player2Logic_SetGM_Res()
		{
			_m_gm = new ProtoMemberInt32(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Player2Logic_SetGM_Res(int __m_gm, uint __m_res)
		{
			_m_gm = new ProtoMemberInt32(1, false);
			_m_gm.member_value = __m_gm;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public int m_gm
		{
			get{ return _m_gm.member_value; }
			set{ _m_gm.member_value = value; }
		}
		public bool has_m_gm
		{
			get{ return _m_gm.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_gm.Serialize(_m_gm.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			int temp_m_gm = 0;
			one_count = _m_gm.ParseFrom(ref temp_m_gm, ref int_stream);
			if (0 < one_count)
			{
					_m_gm.member_value = temp_m_gm;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Player2Logic_SetPlayerName_Res : IMessage
	{
		private ProtoMemberString _m_player_name;	// 角色名
		private ProtoMemberUInt32 _m_res;	// 设置角色名结果（LogicRes）

		public Msg_Player2Logic_SetPlayerName_Res()
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_res = new ProtoMemberUInt32(2, false);
		}

		public Msg_Player2Logic_SetPlayerName_Res(string __m_player_name, uint __m_res)
		{
			_m_player_name = new ProtoMemberString(1, false);
			_m_player_name.member_value = __m_player_name;
			_m_res = new ProtoMemberUInt32(2, false);
			_m_res.member_value = __m_res;
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public uint m_res
		{
			get{ return _m_res.member_value; }
			set{ _m_res.member_value = value; }
		}
		public bool has_m_res
		{
			get{ return _m_res.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			count += _m_res.Serialize(_m_res.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			uint temp_m_res = 0;
			one_count = _m_res.ParseFrom(ref temp_m_res, ref int_stream);
			if (0 < one_count)
			{
					_m_res.member_value = temp_m_res;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Route : IMessage
	{

		public Msg_Route()
		{
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			return count;
		}
	}

	public class Msg_Table_ReLoad_Req : IMessage
	{
		private ProtoMemberUInt64 _m_player_id;	// 玩家id

		public Msg_Table_ReLoad_Req()
		{
			_m_player_id = new ProtoMemberUInt64(1, false);
		}

		public Msg_Table_ReLoad_Req(ulong __m_player_id)
		{
			_m_player_id = new ProtoMemberUInt64(1, false);
			_m_player_id.member_value = __m_player_id;
		}

		public ulong m_player_id
		{
			get{ return _m_player_id.member_value; }
			set{ _m_player_id.member_value = value; }
		}
		public bool has_m_player_id
		{
			get{ return _m_player_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_id.Serialize(_m_player_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_player_id = 0;
			one_count = _m_player_id.ParseFrom(ref temp_m_player_id, ref int_stream);
			if (0 < one_count)
			{
					_m_player_id.member_value = temp_m_player_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class Msg_Write_Log : IMessage
	{
		private ProtoMemberString _m_strlog;	//日志信息	

		public Msg_Write_Log()
		{
			_m_strlog = new ProtoMemberString(1, true);
		}

		public Msg_Write_Log(string __m_strlog)
		{
			_m_strlog = new ProtoMemberString(1, true);
			_m_strlog.member_value = __m_strlog;
		}

		public string m_strlog
		{
			get{ return _m_strlog.member_value; }
			set{ _m_strlog.member_value = value; }
		}
		public bool has_m_strlog
		{
			get{ return _m_strlog.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_strlog.Serialize(_m_strlog.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			string temp_m_strlog = "";
			one_count = _m_strlog.ParseFrom(ref temp_m_strlog, ref int_stream);
			if (0 < one_count)
			{
					_m_strlog.member_value = temp_m_strlog;
					count = count + one_count;
			}

			return count;
		}
	}

	public enum PVEResult
	{
		[System.ComponentModel.Description("pve失败")]
		PVEResult_Fail = 0,                                 //pve失败
		[System.ComponentModel.Description("pve胜利")]
		PVEResult_Success = 1,                              //pve胜利
	}

	public class PVE_Room_Player : IMessage
	{
		private ProtoMemberUInt64 _id;	//玩家id
		private ProtoMemberString _name;	//玩家名字
		private ProtoMemberUInt32 _icon;	//玩家头像
		private ProtoMemberBool _ready;	//准备状态
		private ProtoMemberUInt32 _score;	//积分
		private ProtoMemberUInt32 _win_num;	//获胜序号
		private ProtoMemberUInt32 _game_state;	//游戏状态

		public PVE_Room_Player()
		{
			_id = new ProtoMemberUInt64(1, false);
			_name = new ProtoMemberString(2, false);
			_icon = new ProtoMemberUInt32(3, false);
			_ready = new ProtoMemberBool(4, false);
			_score = new ProtoMemberUInt32(5, false);
			_win_num = new ProtoMemberUInt32(6, false);
			_game_state = new ProtoMemberUInt32(7, false);
		}

		public PVE_Room_Player(ulong __id, string __name, uint __icon, bool __ready, uint __score, uint __win_num, uint __game_state)
		{
			_id = new ProtoMemberUInt64(1, false);
			_id.member_value = __id;
			_name = new ProtoMemberString(2, false);
			_name.member_value = __name;
			_icon = new ProtoMemberUInt32(3, false);
			_icon.member_value = __icon;
			_ready = new ProtoMemberBool(4, false);
			_ready.member_value = __ready;
			_score = new ProtoMemberUInt32(5, false);
			_score.member_value = __score;
			_win_num = new ProtoMemberUInt32(6, false);
			_win_num.member_value = __win_num;
			_game_state = new ProtoMemberUInt32(7, false);
			_game_state.member_value = __game_state;
		}

		public ulong id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public string name
		{
			get{ return _name.member_value; }
			set{ _name.member_value = value; }
		}
		public bool has_name
		{
			get{ return _name.has_value; }
		}

		public uint icon
		{
			get{ return _icon.member_value; }
			set{ _icon.member_value = value; }
		}
		public bool has_icon
		{
			get{ return _icon.has_value; }
		}

		public bool ready
		{
			get{ return _ready.member_value; }
			set{ _ready.member_value = value; }
		}
		public bool has_ready
		{
			get{ return _ready.has_value; }
		}

		public uint score
		{
			get{ return _score.member_value; }
			set{ _score.member_value = value; }
		}
		public bool has_score
		{
			get{ return _score.has_value; }
		}

		public uint win_num
		{
			get{ return _win_num.member_value; }
			set{ _win_num.member_value = value; }
		}
		public bool has_win_num
		{
			get{ return _win_num.has_value; }
		}

		public uint game_state
		{
			get{ return _game_state.member_value; }
			set{ _game_state.member_value = value; }
		}
		public bool has_game_state
		{
			get{ return _game_state.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			count += _name.Serialize(_name.member_value, ref out_stream);

			count += _icon.Serialize(_icon.member_value, ref out_stream);

			count += _ready.Serialize(_ready.member_value, ref out_stream);

			count += _score.Serialize(_score.member_value, ref out_stream);

			count += _win_num.Serialize(_win_num.member_value, ref out_stream);

			count += _game_state.Serialize(_game_state.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			string temp_name = "";
			one_count = _name.ParseFrom(ref temp_name, ref int_stream);
			if (0 < one_count)
			{
					_name.member_value = temp_name;
					count = count + one_count;
			}

			uint temp_icon = 0;
			one_count = _icon.ParseFrom(ref temp_icon, ref int_stream);
			if (0 < one_count)
			{
					_icon.member_value = temp_icon;
					count = count + one_count;
			}

			bool temp_ready = false;
			one_count = _ready.ParseFrom(ref temp_ready, ref int_stream);
			if (0 < one_count)
			{
					_ready.member_value = temp_ready;
					count = count + one_count;
			}

			uint temp_score = 0;
			one_count = _score.ParseFrom(ref temp_score, ref int_stream);
			if (0 < one_count)
			{
					_score.member_value = temp_score;
					count = count + one_count;
			}

			uint temp_win_num = 0;
			one_count = _win_num.ParseFrom(ref temp_win_num, ref int_stream);
			if (0 < one_count)
			{
					_win_num.member_value = temp_win_num;
					count = count + one_count;
			}

			uint temp_game_state = 0;
			one_count = _game_state.ParseFrom(ref temp_game_state, ref int_stream);
			if (0 < one_count)
			{
					_game_state.member_value = temp_game_state;
					count = count + one_count;
			}

			return count;
		}
	}

	public enum PlatformErrCode
	{
		[System.ComponentModel.Description("PlatformErrCode_JsonErr=0x0001")]
		PlatformErrCode_JsonErr = 0x0001,
		[System.ComponentModel.Description("PlatformErrCode_NoAction=0x0002")]
		PlatformErrCode_NoAction = 0x0002,
		[System.ComponentModel.Description("PlatformErrCode_ExecErr=0x0003")]
		PlatformErrCode_ExecErr = 0x0003,
		[System.ComponentModel.Description("PlatformErrCode_ParamErr=0x0004")]
		PlatformErrCode_ParamErr = 0x0004,
	}

	public class Player : IMessage
	{
		private ProtoMemberUInt32 _m_account_id;	// 角色id == 账号id
		private ProtoMemberUInt32 _m_server_id;	// 所属服id
		private ProtoMemberString _m_player_name;	// 玩家角色名称
		private ProtoMemberInt32 _m_gm;	// gm
		private ProtoMemberEmbedded<PlayerBase> _m_player_base;	// 玩家基础信息
		private ProtoMemberEmbedded<PlayerBag> _m_player_bag;	// 玩家背包信息
		private ProtoMemberEmbedded<PlayerNewPve> _m_player_pve;	// 玩家PVE信息
		private ProtoMemberEmbedded<PlayerContest> _m_player_contest;	// 玩家赛事信息
		private ProtoMemberEmbedded<PlayerMail> _m_player_mail;	// 玩家邮件信息
		private ProtoMemberEmbedded<PlayerAction> _m_player_action;	//动作信息
		private ProtoMemberEmbedded<PlayerTravelBag> _m_player_travel_bag;	//玩家旅行背包

		public Player()
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_player_name = new ProtoMemberString(3, false);
			_m_gm = new ProtoMemberInt32(4, false);
			_m_player_base = new ProtoMemberEmbedded<PlayerBase>(5, false);
			_m_player_base.member_value = new PlayerBase();
			_m_player_bag = new ProtoMemberEmbedded<PlayerBag>(6, false);
			_m_player_bag.member_value = new PlayerBag();
			_m_player_pve = new ProtoMemberEmbedded<PlayerNewPve>(8, false);
			_m_player_pve.member_value = new PlayerNewPve();
			_m_player_contest = new ProtoMemberEmbedded<PlayerContest>(10, false);
			_m_player_contest.member_value = new PlayerContest();
			_m_player_mail = new ProtoMemberEmbedded<PlayerMail>(11, false);
			_m_player_mail.member_value = new PlayerMail();
			_m_player_action = new ProtoMemberEmbedded<PlayerAction>(12, false);
			_m_player_action.member_value = new PlayerAction();
			_m_player_travel_bag = new ProtoMemberEmbedded<PlayerTravelBag>(13, false);
			_m_player_travel_bag.member_value = new PlayerTravelBag();
		}

		public Player(uint __m_account_id, uint __m_server_id, string __m_player_name, int __m_gm)
		{
			_m_account_id = new ProtoMemberUInt32(1, false);
			_m_account_id.member_value = __m_account_id;
			_m_server_id = new ProtoMemberUInt32(2, false);
			_m_server_id.member_value = __m_server_id;
			_m_player_name = new ProtoMemberString(3, false);
			_m_player_name.member_value = __m_player_name;
			_m_gm = new ProtoMemberInt32(4, false);
			_m_gm.member_value = __m_gm;
			_m_player_base = new ProtoMemberEmbedded<PlayerBase>(5, false);
			_m_player_base.member_value = new PlayerBase();
			_m_player_bag = new ProtoMemberEmbedded<PlayerBag>(6, false);
			_m_player_bag.member_value = new PlayerBag();
			_m_player_pve = new ProtoMemberEmbedded<PlayerNewPve>(8, false);
			_m_player_pve.member_value = new PlayerNewPve();
			_m_player_contest = new ProtoMemberEmbedded<PlayerContest>(10, false);
			_m_player_contest.member_value = new PlayerContest();
			_m_player_mail = new ProtoMemberEmbedded<PlayerMail>(11, false);
			_m_player_mail.member_value = new PlayerMail();
			_m_player_action = new ProtoMemberEmbedded<PlayerAction>(12, false);
			_m_player_action.member_value = new PlayerAction();
			_m_player_travel_bag = new ProtoMemberEmbedded<PlayerTravelBag>(13, false);
			_m_player_travel_bag.member_value = new PlayerTravelBag();
		}

		public uint m_account_id
		{
			get{ return _m_account_id.member_value; }
			set{ _m_account_id.member_value = value; }
		}
		public bool has_m_account_id
		{
			get{ return _m_account_id.has_value; }
		}

		public uint m_server_id
		{
			get{ return _m_server_id.member_value; }
			set{ _m_server_id.member_value = value; }
		}
		public bool has_m_server_id
		{
			get{ return _m_server_id.has_value; }
		}

		public string m_player_name
		{
			get{ return _m_player_name.member_value; }
			set{ _m_player_name.member_value = value; }
		}
		public bool has_m_player_name
		{
			get{ return _m_player_name.has_value; }
		}

		public int m_gm
		{
			get{ return _m_gm.member_value; }
			set{ _m_gm.member_value = value; }
		}
		public bool has_m_gm
		{
			get{ return _m_gm.has_value; }
		}

		public PlayerBase m_player_base
		{
			get{ return _m_player_base.member_value as PlayerBase; }
			set{ _m_player_base.member_value = value; }
		}
		public bool has_m_player_base
		{
			get{ return _m_player_base.has_value; }
		}

		public PlayerBag m_player_bag
		{
			get{ return _m_player_bag.member_value as PlayerBag; }
			set{ _m_player_bag.member_value = value; }
		}
		public bool has_m_player_bag
		{
			get{ return _m_player_bag.has_value; }
		}

		public PlayerNewPve m_player_pve
		{
			get{ return _m_player_pve.member_value as PlayerNewPve; }
			set{ _m_player_pve.member_value = value; }
		}
		public bool has_m_player_pve
		{
			get{ return _m_player_pve.has_value; }
		}

		public PlayerContest m_player_contest
		{
			get{ return _m_player_contest.member_value as PlayerContest; }
			set{ _m_player_contest.member_value = value; }
		}
		public bool has_m_player_contest
		{
			get{ return _m_player_contest.has_value; }
		}

		public PlayerMail m_player_mail
		{
			get{ return _m_player_mail.member_value as PlayerMail; }
			set{ _m_player_mail.member_value = value; }
		}
		public bool has_m_player_mail
		{
			get{ return _m_player_mail.has_value; }
		}

		public PlayerAction m_player_action
		{
			get{ return _m_player_action.member_value as PlayerAction; }
			set{ _m_player_action.member_value = value; }
		}
		public bool has_m_player_action
		{
			get{ return _m_player_action.has_value; }
		}

		public PlayerTravelBag m_player_travel_bag
		{
			get{ return _m_player_travel_bag.member_value as PlayerTravelBag; }
			set{ _m_player_travel_bag.member_value = value; }
		}
		public bool has_m_player_travel_bag
		{
			get{ return _m_player_travel_bag.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_account_id.Serialize(_m_account_id.member_value, ref out_stream);

			count += _m_server_id.Serialize(_m_server_id.member_value, ref out_stream);

			count += _m_player_name.Serialize(_m_player_name.member_value, ref out_stream);

			count += _m_gm.Serialize(_m_gm.member_value, ref out_stream);

			count += _m_player_base.Serialize(_m_player_base.member_value, ref out_stream);

			count += _m_player_bag.Serialize(_m_player_bag.member_value, ref out_stream);

			count += _m_player_pve.Serialize(_m_player_pve.member_value, ref out_stream);

			count += _m_player_contest.Serialize(_m_player_contest.member_value, ref out_stream);

			count += _m_player_mail.Serialize(_m_player_mail.member_value, ref out_stream);

			count += _m_player_action.Serialize(_m_player_action.member_value, ref out_stream);

			count += _m_player_travel_bag.Serialize(_m_player_travel_bag.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_account_id = 0;
			one_count = _m_account_id.ParseFrom(ref temp_m_account_id, ref int_stream);
			if (0 < one_count)
			{
					_m_account_id.member_value = temp_m_account_id;
					count = count + one_count;
			}

			uint temp_m_server_id = 0;
			one_count = _m_server_id.ParseFrom(ref temp_m_server_id, ref int_stream);
			if (0 < one_count)
			{
					_m_server_id.member_value = temp_m_server_id;
					count = count + one_count;
			}

			string temp_m_player_name = "";
			one_count = _m_player_name.ParseFrom(ref temp_m_player_name, ref int_stream);
			if (0 < one_count)
			{
					_m_player_name.member_value = temp_m_player_name;
					count = count + one_count;
			}

			int temp_m_gm = 0;
			one_count = _m_gm.ParseFrom(ref temp_m_gm, ref int_stream);
			if (0 < one_count)
			{
					_m_gm.member_value = temp_m_gm;
					count = count + one_count;
			}

			PlayerBase temp_m_player_base = new PlayerBase();
			one_count = _m_player_base.ParseFrom(temp_m_player_base, ref int_stream);
			if (0 < one_count)
			{
					_m_player_base.member_value = temp_m_player_base;
					count = count + one_count;
			}

			PlayerBag temp_m_player_bag = new PlayerBag();
			one_count = _m_player_bag.ParseFrom(temp_m_player_bag, ref int_stream);
			if (0 < one_count)
			{
					_m_player_bag.member_value = temp_m_player_bag;
					count = count + one_count;
			}

			PlayerNewPve temp_m_player_pve = new PlayerNewPve();
			one_count = _m_player_pve.ParseFrom(temp_m_player_pve, ref int_stream);
			if (0 < one_count)
			{
					_m_player_pve.member_value = temp_m_player_pve;
					count = count + one_count;
			}

			PlayerContest temp_m_player_contest = new PlayerContest();
			one_count = _m_player_contest.ParseFrom(temp_m_player_contest, ref int_stream);
			if (0 < one_count)
			{
					_m_player_contest.member_value = temp_m_player_contest;
					count = count + one_count;
			}

			PlayerMail temp_m_player_mail = new PlayerMail();
			one_count = _m_player_mail.ParseFrom(temp_m_player_mail, ref int_stream);
			if (0 < one_count)
			{
					_m_player_mail.member_value = temp_m_player_mail;
					count = count + one_count;
			}

			PlayerAction temp_m_player_action = new PlayerAction();
			one_count = _m_player_action.ParseFrom(temp_m_player_action, ref int_stream);
			if (0 < one_count)
			{
					_m_player_action.member_value = temp_m_player_action;
					count = count + one_count;
			}

			PlayerTravelBag temp_m_player_travel_bag = new PlayerTravelBag();
			one_count = _m_player_travel_bag.ParseFrom(temp_m_player_travel_bag, ref int_stream);
			if (0 < one_count)
			{
					_m_player_travel_bag.member_value = temp_m_player_travel_bag;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerAction : IMessage
	{
		private ProtoMemberUInt32 _m_type;	// 类型
		private ProtoMemberUInt32 _m_begin_time;	// 开始时间
		private ProtoMemberUInt32 _m_end_time;	// 结束时间

		public PlayerAction()
		{
			_m_type = new ProtoMemberUInt32(1, false);
			_m_begin_time = new ProtoMemberUInt32(2, false);
			_m_end_time = new ProtoMemberUInt32(3, false);
		}

		public PlayerAction(uint __m_type, uint __m_begin_time, uint __m_end_time)
		{
			_m_type = new ProtoMemberUInt32(1, false);
			_m_type.member_value = __m_type;
			_m_begin_time = new ProtoMemberUInt32(2, false);
			_m_begin_time.member_value = __m_begin_time;
			_m_end_time = new ProtoMemberUInt32(3, false);
			_m_end_time.member_value = __m_end_time;
		}

		public uint m_type
		{
			get{ return _m_type.member_value; }
			set{ _m_type.member_value = value; }
		}
		public bool has_m_type
		{
			get{ return _m_type.has_value; }
		}

		public uint m_begin_time
		{
			get{ return _m_begin_time.member_value; }
			set{ _m_begin_time.member_value = value; }
		}
		public bool has_m_begin_time
		{
			get{ return _m_begin_time.has_value; }
		}

		public uint m_end_time
		{
			get{ return _m_end_time.member_value; }
			set{ _m_end_time.member_value = value; }
		}
		public bool has_m_end_time
		{
			get{ return _m_end_time.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_type.Serialize(_m_type.member_value, ref out_stream);

			count += _m_begin_time.Serialize(_m_begin_time.member_value, ref out_stream);

			count += _m_end_time.Serialize(_m_end_time.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_type = 0;
			one_count = _m_type.ParseFrom(ref temp_m_type, ref int_stream);
			if (0 < one_count)
			{
					_m_type.member_value = temp_m_type;
					count = count + one_count;
			}

			uint temp_m_begin_time = 0;
			one_count = _m_begin_time.ParseFrom(ref temp_m_begin_time, ref int_stream);
			if (0 < one_count)
			{
					_m_begin_time.member_value = temp_m_begin_time;
					count = count + one_count;
			}

			uint temp_m_end_time = 0;
			one_count = _m_end_time.ParseFrom(ref temp_m_end_time, ref int_stream);
			if (0 < one_count)
			{
					_m_end_time.member_value = temp_m_end_time;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerBag : IMessage
	{
		private ProtoMemberEmbeddedList<PlayerBagItem> _m_items;	// 道具列表

		public PlayerBag()
		{
			_m_items = new ProtoMemberEmbeddedList<PlayerBagItem>(1, false);
		}

		public System.Collections.Generic.List<PlayerBagItem> m_items
		{
			get{ return _m_items.member_value; }
		}
		public bool has_m_items
		{
			get{ return _m_items.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			foreach(PlayerBagItem one_member_value in _m_items.member_value)
			{
				count += _m_items.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			while (true)
			{
				PlayerBagItem one_member_value = new PlayerBagItem();
				one_count = _m_items.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_items.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class PlayerBagItem : IMessage
	{
		private ProtoMemberUInt64 _m_id;	// 道具id
		private ProtoMemberUInt32 _m_item_id;	// 配置id
		private ProtoMemberUInt32 _m_item_count;	// 道具数量

		public PlayerBagItem()
		{
			_m_id = new ProtoMemberUInt64(1, false);
			_m_item_id = new ProtoMemberUInt32(2, false);
			_m_item_count = new ProtoMemberUInt32(3, false);
		}

		public PlayerBagItem(ulong __m_id, uint __m_item_id, uint __m_item_count)
		{
			_m_id = new ProtoMemberUInt64(1, false);
			_m_id.member_value = __m_id;
			_m_item_id = new ProtoMemberUInt32(2, false);
			_m_item_id.member_value = __m_item_id;
			_m_item_count = new ProtoMemberUInt32(3, false);
			_m_item_count.member_value = __m_item_count;
		}

		public ulong m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public uint m_item_id
		{
			get{ return _m_item_id.member_value; }
			set{ _m_item_id.member_value = value; }
		}
		public bool has_m_item_id
		{
			get{ return _m_item_id.has_value; }
		}

		public uint m_item_count
		{
			get{ return _m_item_count.member_value; }
			set{ _m_item_count.member_value = value; }
		}
		public bool has_m_item_count
		{
			get{ return _m_item_count.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _m_item_id.Serialize(_m_item_id.member_value, ref out_stream);

			count += _m_item_count.Serialize(_m_item_count.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			uint temp_m_item_id = 0;
			one_count = _m_item_id.ParseFrom(ref temp_m_item_id, ref int_stream);
			if (0 < one_count)
			{
					_m_item_id.member_value = temp_m_item_id;
					count = count + one_count;
			}

			uint temp_m_item_count = 0;
			one_count = _m_item_count.ParseFrom(ref temp_m_item_count, ref int_stream);
			if (0 < one_count)
			{
					_m_item_count.member_value = temp_m_item_count;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerBase : IMessage
	{
		private ProtoMemberUInt32 _m_player_lv;	// 玩家等级
		private ProtoMemberUInt32 _m_player_exp;	// 玩家经验
		private ProtoMemberUInt32 _m_player_vip;	// vip等级
		private ProtoMemberUInt64 _m_player_create;	// 角色创建时间
		private ProtoMemberUInt64 _m_player_last_online;	// 角色上一次的上线时间
		private ProtoMemberUInt64 _m_player_last_offline;	// 角色上一次的下线时间
		private ProtoMemberString _m_player_task;	// 任务
		private ProtoMemberString _m_player_contestMemoInfo;	// 赛事备忘数据
		private ProtoMemberUInt32 _m_avatar;	// avatarid
		private ProtoMemberUInt64 _m_free_gacha_time;	// 上次免费扭蛋时间
		private ProtoMemberUInt32 _m_settle_candy;	// 结算的糖果
		private ProtoMemberUInt32 _m_settle_candy_time;	// 结算的糖果时间

		public PlayerBase()
		{
			_m_player_lv = new ProtoMemberUInt32(1, false);
			_m_player_exp = new ProtoMemberUInt32(2, false);
			_m_player_vip = new ProtoMemberUInt32(3, false);
			_m_player_create = new ProtoMemberUInt64(4, false);
			_m_player_last_online = new ProtoMemberUInt64(5, false);
			_m_player_last_offline = new ProtoMemberUInt64(6, false);
			_m_player_task = new ProtoMemberString(7, false);
			_m_player_contestMemoInfo = new ProtoMemberString(8, false);
			_m_avatar = new ProtoMemberUInt32(9, false);
			_m_free_gacha_time = new ProtoMemberUInt64(10, false);
			_m_settle_candy = new ProtoMemberUInt32(11, false);
			_m_settle_candy_time = new ProtoMemberUInt32(12, false);
		}

		public PlayerBase(uint __m_player_lv, uint __m_player_exp, uint __m_player_vip, ulong __m_player_create, ulong __m_player_last_online, ulong __m_player_last_offline, string __m_player_task, string __m_player_contestMemoInfo, uint __m_avatar, ulong __m_free_gacha_time, uint __m_settle_candy, uint __m_settle_candy_time)
		{
			_m_player_lv = new ProtoMemberUInt32(1, false);
			_m_player_lv.member_value = __m_player_lv;
			_m_player_exp = new ProtoMemberUInt32(2, false);
			_m_player_exp.member_value = __m_player_exp;
			_m_player_vip = new ProtoMemberUInt32(3, false);
			_m_player_vip.member_value = __m_player_vip;
			_m_player_create = new ProtoMemberUInt64(4, false);
			_m_player_create.member_value = __m_player_create;
			_m_player_last_online = new ProtoMemberUInt64(5, false);
			_m_player_last_online.member_value = __m_player_last_online;
			_m_player_last_offline = new ProtoMemberUInt64(6, false);
			_m_player_last_offline.member_value = __m_player_last_offline;
			_m_player_task = new ProtoMemberString(7, false);
			_m_player_task.member_value = __m_player_task;
			_m_player_contestMemoInfo = new ProtoMemberString(8, false);
			_m_player_contestMemoInfo.member_value = __m_player_contestMemoInfo;
			_m_avatar = new ProtoMemberUInt32(9, false);
			_m_avatar.member_value = __m_avatar;
			_m_free_gacha_time = new ProtoMemberUInt64(10, false);
			_m_free_gacha_time.member_value = __m_free_gacha_time;
			_m_settle_candy = new ProtoMemberUInt32(11, false);
			_m_settle_candy.member_value = __m_settle_candy;
			_m_settle_candy_time = new ProtoMemberUInt32(12, false);
			_m_settle_candy_time.member_value = __m_settle_candy_time;
		}

		public uint m_player_lv
		{
			get{ return _m_player_lv.member_value; }
			set{ _m_player_lv.member_value = value; }
		}
		public bool has_m_player_lv
		{
			get{ return _m_player_lv.has_value; }
		}

		public uint m_player_exp
		{
			get{ return _m_player_exp.member_value; }
			set{ _m_player_exp.member_value = value; }
		}
		public bool has_m_player_exp
		{
			get{ return _m_player_exp.has_value; }
		}

		public uint m_player_vip
		{
			get{ return _m_player_vip.member_value; }
			set{ _m_player_vip.member_value = value; }
		}
		public bool has_m_player_vip
		{
			get{ return _m_player_vip.has_value; }
		}

		public ulong m_player_create
		{
			get{ return _m_player_create.member_value; }
			set{ _m_player_create.member_value = value; }
		}
		public bool has_m_player_create
		{
			get{ return _m_player_create.has_value; }
		}

		public ulong m_player_last_online
		{
			get{ return _m_player_last_online.member_value; }
			set{ _m_player_last_online.member_value = value; }
		}
		public bool has_m_player_last_online
		{
			get{ return _m_player_last_online.has_value; }
		}

		public ulong m_player_last_offline
		{
			get{ return _m_player_last_offline.member_value; }
			set{ _m_player_last_offline.member_value = value; }
		}
		public bool has_m_player_last_offline
		{
			get{ return _m_player_last_offline.has_value; }
		}

		public string m_player_task
		{
			get{ return _m_player_task.member_value; }
			set{ _m_player_task.member_value = value; }
		}
		public bool has_m_player_task
		{
			get{ return _m_player_task.has_value; }
		}

		public string m_player_contestMemoInfo
		{
			get{ return _m_player_contestMemoInfo.member_value; }
			set{ _m_player_contestMemoInfo.member_value = value; }
		}
		public bool has_m_player_contestMemoInfo
		{
			get{ return _m_player_contestMemoInfo.has_value; }
		}

		public uint m_avatar
		{
			get{ return _m_avatar.member_value; }
			set{ _m_avatar.member_value = value; }
		}
		public bool has_m_avatar
		{
			get{ return _m_avatar.has_value; }
		}

		public ulong m_free_gacha_time
		{
			get{ return _m_free_gacha_time.member_value; }
			set{ _m_free_gacha_time.member_value = value; }
		}
		public bool has_m_free_gacha_time
		{
			get{ return _m_free_gacha_time.has_value; }
		}

		public uint m_settle_candy
		{
			get{ return _m_settle_candy.member_value; }
			set{ _m_settle_candy.member_value = value; }
		}
		public bool has_m_settle_candy
		{
			get{ return _m_settle_candy.has_value; }
		}

		public uint m_settle_candy_time
		{
			get{ return _m_settle_candy_time.member_value; }
			set{ _m_settle_candy_time.member_value = value; }
		}
		public bool has_m_settle_candy_time
		{
			get{ return _m_settle_candy_time.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_player_lv.Serialize(_m_player_lv.member_value, ref out_stream);

			count += _m_player_exp.Serialize(_m_player_exp.member_value, ref out_stream);

			count += _m_player_vip.Serialize(_m_player_vip.member_value, ref out_stream);

			count += _m_player_create.Serialize(_m_player_create.member_value, ref out_stream);

			count += _m_player_last_online.Serialize(_m_player_last_online.member_value, ref out_stream);

			count += _m_player_last_offline.Serialize(_m_player_last_offline.member_value, ref out_stream);

			count += _m_player_task.Serialize(_m_player_task.member_value, ref out_stream);

			count += _m_player_contestMemoInfo.Serialize(_m_player_contestMemoInfo.member_value, ref out_stream);

			count += _m_avatar.Serialize(_m_avatar.member_value, ref out_stream);

			count += _m_free_gacha_time.Serialize(_m_free_gacha_time.member_value, ref out_stream);

			count += _m_settle_candy.Serialize(_m_settle_candy.member_value, ref out_stream);

			count += _m_settle_candy_time.Serialize(_m_settle_candy_time.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_player_lv = 0;
			one_count = _m_player_lv.ParseFrom(ref temp_m_player_lv, ref int_stream);
			if (0 < one_count)
			{
					_m_player_lv.member_value = temp_m_player_lv;
					count = count + one_count;
			}

			uint temp_m_player_exp = 0;
			one_count = _m_player_exp.ParseFrom(ref temp_m_player_exp, ref int_stream);
			if (0 < one_count)
			{
					_m_player_exp.member_value = temp_m_player_exp;
					count = count + one_count;
			}

			uint temp_m_player_vip = 0;
			one_count = _m_player_vip.ParseFrom(ref temp_m_player_vip, ref int_stream);
			if (0 < one_count)
			{
					_m_player_vip.member_value = temp_m_player_vip;
					count = count + one_count;
			}

			ulong temp_m_player_create = 0;
			one_count = _m_player_create.ParseFrom(ref temp_m_player_create, ref int_stream);
			if (0 < one_count)
			{
					_m_player_create.member_value = temp_m_player_create;
					count = count + one_count;
			}

			ulong temp_m_player_last_online = 0;
			one_count = _m_player_last_online.ParseFrom(ref temp_m_player_last_online, ref int_stream);
			if (0 < one_count)
			{
					_m_player_last_online.member_value = temp_m_player_last_online;
					count = count + one_count;
			}

			ulong temp_m_player_last_offline = 0;
			one_count = _m_player_last_offline.ParseFrom(ref temp_m_player_last_offline, ref int_stream);
			if (0 < one_count)
			{
					_m_player_last_offline.member_value = temp_m_player_last_offline;
					count = count + one_count;
			}

			string temp_m_player_task = "";
			one_count = _m_player_task.ParseFrom(ref temp_m_player_task, ref int_stream);
			if (0 < one_count)
			{
					_m_player_task.member_value = temp_m_player_task;
					count = count + one_count;
			}

			string temp_m_player_contestMemoInfo = "";
			one_count = _m_player_contestMemoInfo.ParseFrom(ref temp_m_player_contestMemoInfo, ref int_stream);
			if (0 < one_count)
			{
					_m_player_contestMemoInfo.member_value = temp_m_player_contestMemoInfo;
					count = count + one_count;
			}

			uint temp_m_avatar = 0;
			one_count = _m_avatar.ParseFrom(ref temp_m_avatar, ref int_stream);
			if (0 < one_count)
			{
					_m_avatar.member_value = temp_m_avatar;
					count = count + one_count;
			}

			ulong temp_m_free_gacha_time = 0;
			one_count = _m_free_gacha_time.ParseFrom(ref temp_m_free_gacha_time, ref int_stream);
			if (0 < one_count)
			{
					_m_free_gacha_time.member_value = temp_m_free_gacha_time;
					count = count + one_count;
			}

			uint temp_m_settle_candy = 0;
			one_count = _m_settle_candy.ParseFrom(ref temp_m_settle_candy, ref int_stream);
			if (0 < one_count)
			{
					_m_settle_candy.member_value = temp_m_settle_candy;
					count = count + one_count;
			}

			uint temp_m_settle_candy_time = 0;
			one_count = _m_settle_candy_time.ParseFrom(ref temp_m_settle_candy_time, ref int_stream);
			if (0 < one_count)
			{
					_m_settle_candy_time.member_value = temp_m_settle_candy_time;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerContest : IMessage
	{
		private ProtoMemberUInt64 _gm_id;	//参与的开启活动的gm玩家id
		private ProtoMemberString _setup;	//快速设置信息
		private ProtoMemberUInt32 _open_id;	//作为gm开启的赛事id
		private ProtoMemberUInt64 _boss_gm_id;	//参与的开启boss战斗活动的gm玩家id
		private ProtoMemberUInt32 _boss_open_id;	//作为gm开启的boss战斗赛事id

		public PlayerContest()
		{
			_gm_id = new ProtoMemberUInt64(1, false);
			_setup = new ProtoMemberString(2, false);
			_open_id = new ProtoMemberUInt32(3, false);
			_boss_gm_id = new ProtoMemberUInt64(4, false);
			_boss_open_id = new ProtoMemberUInt32(5, false);
		}

		public PlayerContest(ulong __gm_id, string __setup, uint __open_id, ulong __boss_gm_id, uint __boss_open_id)
		{
			_gm_id = new ProtoMemberUInt64(1, false);
			_gm_id.member_value = __gm_id;
			_setup = new ProtoMemberString(2, false);
			_setup.member_value = __setup;
			_open_id = new ProtoMemberUInt32(3, false);
			_open_id.member_value = __open_id;
			_boss_gm_id = new ProtoMemberUInt64(4, false);
			_boss_gm_id.member_value = __boss_gm_id;
			_boss_open_id = new ProtoMemberUInt32(5, false);
			_boss_open_id.member_value = __boss_open_id;
		}

		public ulong gm_id
		{
			get{ return _gm_id.member_value; }
			set{ _gm_id.member_value = value; }
		}
		public bool has_gm_id
		{
			get{ return _gm_id.has_value; }
		}

		public string setup
		{
			get{ return _setup.member_value; }
			set{ _setup.member_value = value; }
		}
		public bool has_setup
		{
			get{ return _setup.has_value; }
		}

		public uint open_id
		{
			get{ return _open_id.member_value; }
			set{ _open_id.member_value = value; }
		}
		public bool has_open_id
		{
			get{ return _open_id.has_value; }
		}

		public ulong boss_gm_id
		{
			get{ return _boss_gm_id.member_value; }
			set{ _boss_gm_id.member_value = value; }
		}
		public bool has_boss_gm_id
		{
			get{ return _boss_gm_id.has_value; }
		}

		public uint boss_open_id
		{
			get{ return _boss_open_id.member_value; }
			set{ _boss_open_id.member_value = value; }
		}
		public bool has_boss_open_id
		{
			get{ return _boss_open_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _gm_id.Serialize(_gm_id.member_value, ref out_stream);

			count += _setup.Serialize(_setup.member_value, ref out_stream);

			count += _open_id.Serialize(_open_id.member_value, ref out_stream);

			count += _boss_gm_id.Serialize(_boss_gm_id.member_value, ref out_stream);

			count += _boss_open_id.Serialize(_boss_open_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_gm_id = 0;
			one_count = _gm_id.ParseFrom(ref temp_gm_id, ref int_stream);
			if (0 < one_count)
			{
					_gm_id.member_value = temp_gm_id;
					count = count + one_count;
			}

			string temp_setup = "";
			one_count = _setup.ParseFrom(ref temp_setup, ref int_stream);
			if (0 < one_count)
			{
					_setup.member_value = temp_setup;
					count = count + one_count;
			}

			uint temp_open_id = 0;
			one_count = _open_id.ParseFrom(ref temp_open_id, ref int_stream);
			if (0 < one_count)
			{
					_open_id.member_value = temp_open_id;
					count = count + one_count;
			}

			ulong temp_boss_gm_id = 0;
			one_count = _boss_gm_id.ParseFrom(ref temp_boss_gm_id, ref int_stream);
			if (0 < one_count)
			{
					_boss_gm_id.member_value = temp_boss_gm_id;
					count = count + one_count;
			}

			uint temp_boss_open_id = 0;
			one_count = _boss_open_id.ParseFrom(ref temp_boss_open_id, ref int_stream);
			if (0 < one_count)
			{
					_boss_open_id.member_value = temp_boss_open_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerLogInfo : IMessage
	{
		private ProtoMemberUInt32 _m_LogType;	// log类型
		private ProtoMemberUInt32 _m_PropertyType;	// 属性类型
		private ProtoMemberUInt64 _m_PropertyValue;	// 属性值

		public PlayerLogInfo()
		{
			_m_LogType = new ProtoMemberUInt32(1, false);
			_m_PropertyType = new ProtoMemberUInt32(2, false);
			_m_PropertyValue = new ProtoMemberUInt64(3, false);
		}

		public PlayerLogInfo(uint __m_LogType, uint __m_PropertyType, ulong __m_PropertyValue)
		{
			_m_LogType = new ProtoMemberUInt32(1, false);
			_m_LogType.member_value = __m_LogType;
			_m_PropertyType = new ProtoMemberUInt32(2, false);
			_m_PropertyType.member_value = __m_PropertyType;
			_m_PropertyValue = new ProtoMemberUInt64(3, false);
			_m_PropertyValue.member_value = __m_PropertyValue;
		}

		public uint m_LogType
		{
			get{ return _m_LogType.member_value; }
			set{ _m_LogType.member_value = value; }
		}
		public bool has_m_LogType
		{
			get{ return _m_LogType.has_value; }
		}

		public uint m_PropertyType
		{
			get{ return _m_PropertyType.member_value; }
			set{ _m_PropertyType.member_value = value; }
		}
		public bool has_m_PropertyType
		{
			get{ return _m_PropertyType.has_value; }
		}

		public ulong m_PropertyValue
		{
			get{ return _m_PropertyValue.member_value; }
			set{ _m_PropertyValue.member_value = value; }
		}
		public bool has_m_PropertyValue
		{
			get{ return _m_PropertyValue.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_LogType.Serialize(_m_LogType.member_value, ref out_stream);

			count += _m_PropertyType.Serialize(_m_PropertyType.member_value, ref out_stream);

			count += _m_PropertyValue.Serialize(_m_PropertyValue.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_LogType = 0;
			one_count = _m_LogType.ParseFrom(ref temp_m_LogType, ref int_stream);
			if (0 < one_count)
			{
					_m_LogType.member_value = temp_m_LogType;
					count = count + one_count;
			}

			uint temp_m_PropertyType = 0;
			one_count = _m_PropertyType.ParseFrom(ref temp_m_PropertyType, ref int_stream);
			if (0 < one_count)
			{
					_m_PropertyType.member_value = temp_m_PropertyType;
					count = count + one_count;
			}

			ulong temp_m_PropertyValue = 0;
			one_count = _m_PropertyValue.ParseFrom(ref temp_m_PropertyValue, ref int_stream);
			if (0 < one_count)
			{
					_m_PropertyValue.member_value = temp_m_PropertyValue;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerLogList : IMessage
	{
		private ProtoMemberUInt64 _m_Time;	// log时间戳
		private ProtoMemberUInt32 _m_UnionId;	// 所属公会id
		private ProtoMemberString _m_PlayerName;	// 角色名
		private ProtoMemberString _m_PlayerIP;	// 登录ip
		private ProtoMemberString _m_PlayerIMEI;	// 登录设备id
		private ProtoMemberEmbeddedList<PlayerLogInfo> _m_Logs;	// log属性列表

		public PlayerLogList()
		{
			_m_Time = new ProtoMemberUInt64(1, false);
			_m_UnionId = new ProtoMemberUInt32(2, false);
			_m_PlayerName = new ProtoMemberString(3, false);
			_m_PlayerIP = new ProtoMemberString(4, false);
			_m_PlayerIMEI = new ProtoMemberString(5, false);
			_m_Logs = new ProtoMemberEmbeddedList<PlayerLogInfo>(6, false);
		}

		public PlayerLogList(ulong __m_Time, uint __m_UnionId, string __m_PlayerName, string __m_PlayerIP, string __m_PlayerIMEI)
		{
			_m_Time = new ProtoMemberUInt64(1, false);
			_m_Time.member_value = __m_Time;
			_m_UnionId = new ProtoMemberUInt32(2, false);
			_m_UnionId.member_value = __m_UnionId;
			_m_PlayerName = new ProtoMemberString(3, false);
			_m_PlayerName.member_value = __m_PlayerName;
			_m_PlayerIP = new ProtoMemberString(4, false);
			_m_PlayerIP.member_value = __m_PlayerIP;
			_m_PlayerIMEI = new ProtoMemberString(5, false);
			_m_PlayerIMEI.member_value = __m_PlayerIMEI;
			_m_Logs = new ProtoMemberEmbeddedList<PlayerLogInfo>(6, false);
		}

		public ulong m_Time
		{
			get{ return _m_Time.member_value; }
			set{ _m_Time.member_value = value; }
		}
		public bool has_m_Time
		{
			get{ return _m_Time.has_value; }
		}

		public uint m_UnionId
		{
			get{ return _m_UnionId.member_value; }
			set{ _m_UnionId.member_value = value; }
		}
		public bool has_m_UnionId
		{
			get{ return _m_UnionId.has_value; }
		}

		public string m_PlayerName
		{
			get{ return _m_PlayerName.member_value; }
			set{ _m_PlayerName.member_value = value; }
		}
		public bool has_m_PlayerName
		{
			get{ return _m_PlayerName.has_value; }
		}

		public string m_PlayerIP
		{
			get{ return _m_PlayerIP.member_value; }
			set{ _m_PlayerIP.member_value = value; }
		}
		public bool has_m_PlayerIP
		{
			get{ return _m_PlayerIP.has_value; }
		}

		public string m_PlayerIMEI
		{
			get{ return _m_PlayerIMEI.member_value; }
			set{ _m_PlayerIMEI.member_value = value; }
		}
		public bool has_m_PlayerIMEI
		{
			get{ return _m_PlayerIMEI.has_value; }
		}

		public System.Collections.Generic.List<PlayerLogInfo> m_Logs
		{
			get{ return _m_Logs.member_value; }
		}
		public bool has_m_Logs
		{
			get{ return _m_Logs.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_Time.Serialize(_m_Time.member_value, ref out_stream);

			count += _m_UnionId.Serialize(_m_UnionId.member_value, ref out_stream);

			count += _m_PlayerName.Serialize(_m_PlayerName.member_value, ref out_stream);

			count += _m_PlayerIP.Serialize(_m_PlayerIP.member_value, ref out_stream);

			count += _m_PlayerIMEI.Serialize(_m_PlayerIMEI.member_value, ref out_stream);

			foreach(PlayerLogInfo one_member_value in _m_Logs.member_value)
			{
				count += _m_Logs.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_Time = 0;
			one_count = _m_Time.ParseFrom(ref temp_m_Time, ref int_stream);
			if (0 < one_count)
			{
					_m_Time.member_value = temp_m_Time;
					count = count + one_count;
			}

			uint temp_m_UnionId = 0;
			one_count = _m_UnionId.ParseFrom(ref temp_m_UnionId, ref int_stream);
			if (0 < one_count)
			{
					_m_UnionId.member_value = temp_m_UnionId;
					count = count + one_count;
			}

			string temp_m_PlayerName = "";
			one_count = _m_PlayerName.ParseFrom(ref temp_m_PlayerName, ref int_stream);
			if (0 < one_count)
			{
					_m_PlayerName.member_value = temp_m_PlayerName;
					count = count + one_count;
			}

			string temp_m_PlayerIP = "";
			one_count = _m_PlayerIP.ParseFrom(ref temp_m_PlayerIP, ref int_stream);
			if (0 < one_count)
			{
					_m_PlayerIP.member_value = temp_m_PlayerIP;
					count = count + one_count;
			}

			string temp_m_PlayerIMEI = "";
			one_count = _m_PlayerIMEI.ParseFrom(ref temp_m_PlayerIMEI, ref int_stream);
			if (0 < one_count)
			{
					_m_PlayerIMEI.member_value = temp_m_PlayerIMEI;
					count = count + one_count;
			}

			while (true)
			{
				PlayerLogInfo one_member_value = new PlayerLogInfo();
				one_count = _m_Logs.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_Logs.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class PlayerMail : IMessage
	{
		private ProtoMemberEmbeddedList<PlayerMailItem> _m_mails;	// 邮件列表

		public PlayerMail()
		{
			_m_mails = new ProtoMemberEmbeddedList<PlayerMailItem>(1, false);
		}

		public System.Collections.Generic.List<PlayerMailItem> m_mails
		{
			get{ return _m_mails.member_value; }
		}
		public bool has_m_mails
		{
			get{ return _m_mails.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			foreach(PlayerMailItem one_member_value in _m_mails.member_value)
			{
				count += _m_mails.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			while (true)
			{
				PlayerMailItem one_member_value = new PlayerMailItem();
				one_count = _m_mails.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_mails.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class PlayerMailItem : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 邮件id
		private ProtoMemberUInt32 _m_type;	// 邮件状态
		private ProtoMemberUInt32 _m_state;	// 邮件类型
		private ProtoMemberUInt32 _m_time;	// 邮件时间
		private ProtoMemberString _m_header;	// 邮件头
		private ProtoMemberString _m_content;	// 邮件内容
		private ProtoMemberEmbeddedList<MailAppendixItem> _m_appendix;	// 邮件附件

		public PlayerMailItem()
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_type = new ProtoMemberUInt32(2, false);
			_m_state = new ProtoMemberUInt32(3, false);
			_m_time = new ProtoMemberUInt32(4, false);
			_m_header = new ProtoMemberString(5, false);
			_m_content = new ProtoMemberString(6, false);
			_m_appendix = new ProtoMemberEmbeddedList<MailAppendixItem>(7, false);
		}

		public PlayerMailItem(uint __m_id, uint __m_type, uint __m_state, uint __m_time, string __m_header, string __m_content)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
			_m_type = new ProtoMemberUInt32(2, false);
			_m_type.member_value = __m_type;
			_m_state = new ProtoMemberUInt32(3, false);
			_m_state.member_value = __m_state;
			_m_time = new ProtoMemberUInt32(4, false);
			_m_time.member_value = __m_time;
			_m_header = new ProtoMemberString(5, false);
			_m_header.member_value = __m_header;
			_m_content = new ProtoMemberString(6, false);
			_m_content.member_value = __m_content;
			_m_appendix = new ProtoMemberEmbeddedList<MailAppendixItem>(7, false);
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public uint m_type
		{
			get{ return _m_type.member_value; }
			set{ _m_type.member_value = value; }
		}
		public bool has_m_type
		{
			get{ return _m_type.has_value; }
		}

		public uint m_state
		{
			get{ return _m_state.member_value; }
			set{ _m_state.member_value = value; }
		}
		public bool has_m_state
		{
			get{ return _m_state.has_value; }
		}

		public uint m_time
		{
			get{ return _m_time.member_value; }
			set{ _m_time.member_value = value; }
		}
		public bool has_m_time
		{
			get{ return _m_time.has_value; }
		}

		public string m_header
		{
			get{ return _m_header.member_value; }
			set{ _m_header.member_value = value; }
		}
		public bool has_m_header
		{
			get{ return _m_header.has_value; }
		}

		public string m_content
		{
			get{ return _m_content.member_value; }
			set{ _m_content.member_value = value; }
		}
		public bool has_m_content
		{
			get{ return _m_content.has_value; }
		}

		public System.Collections.Generic.List<MailAppendixItem> m_appendix
		{
			get{ return _m_appendix.member_value; }
		}
		public bool has_m_appendix
		{
			get{ return _m_appendix.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _m_type.Serialize(_m_type.member_value, ref out_stream);

			count += _m_state.Serialize(_m_state.member_value, ref out_stream);

			count += _m_time.Serialize(_m_time.member_value, ref out_stream);

			count += _m_header.Serialize(_m_header.member_value, ref out_stream);

			count += _m_content.Serialize(_m_content.member_value, ref out_stream);

			foreach(MailAppendixItem one_member_value in _m_appendix.member_value)
			{
				count += _m_appendix.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			uint temp_m_type = 0;
			one_count = _m_type.ParseFrom(ref temp_m_type, ref int_stream);
			if (0 < one_count)
			{
					_m_type.member_value = temp_m_type;
					count = count + one_count;
			}

			uint temp_m_state = 0;
			one_count = _m_state.ParseFrom(ref temp_m_state, ref int_stream);
			if (0 < one_count)
			{
					_m_state.member_value = temp_m_state;
					count = count + one_count;
			}

			uint temp_m_time = 0;
			one_count = _m_time.ParseFrom(ref temp_m_time, ref int_stream);
			if (0 < one_count)
			{
					_m_time.member_value = temp_m_time;
					count = count + one_count;
			}

			string temp_m_header = "";
			one_count = _m_header.ParseFrom(ref temp_m_header, ref int_stream);
			if (0 < one_count)
			{
					_m_header.member_value = temp_m_header;
					count = count + one_count;
			}

			string temp_m_content = "";
			one_count = _m_content.ParseFrom(ref temp_m_content, ref int_stream);
			if (0 < one_count)
			{
					_m_content.member_value = temp_m_content;
					count = count + one_count;
			}

			while (true)
			{
				MailAppendixItem one_member_value = new MailAppendixItem();
				one_count = _m_appendix.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_appendix.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class PlayerNewPve : IMessage
	{
		private ProtoMemberEmbeddedList<PlayerOnePve> _m_pves;	// 打过的各主题的关卡id，只保留最后每个关卡最后一个

		public PlayerNewPve()
		{
			_m_pves = new ProtoMemberEmbeddedList<PlayerOnePve>(1, false);
		}

		public System.Collections.Generic.List<PlayerOnePve> m_pves
		{
			get{ return _m_pves.member_value; }
		}
		public bool has_m_pves
		{
			get{ return _m_pves.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			foreach(PlayerOnePve one_member_value in _m_pves.member_value)
			{
				count += _m_pves.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			while (true)
			{
				PlayerOnePve one_member_value = new PlayerOnePve();
				one_count = _m_pves.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_pves.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class PlayerOnePve : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 关卡id（章id+回id+关id）

		public PlayerOnePve()
		{
			_m_id = new ProtoMemberUInt32(1, false);
		}

		public PlayerOnePve(uint __m_id)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerPve : IMessage
	{
		private ProtoMemberUInt32 _m_cur_strength;	// 当前体力
		private ProtoMemberUInt32 _m_max_strength;	// 体力上限
		private ProtoMemberEmbeddedList<PlayerPveChapter> _m_chapters;	// 关卡信息

		public PlayerPve()
		{
			_m_cur_strength = new ProtoMemberUInt32(1, false);
			_m_max_strength = new ProtoMemberUInt32(2, false);
			_m_chapters = new ProtoMemberEmbeddedList<PlayerPveChapter>(3, false);
		}

		public PlayerPve(uint __m_cur_strength, uint __m_max_strength)
		{
			_m_cur_strength = new ProtoMemberUInt32(1, false);
			_m_cur_strength.member_value = __m_cur_strength;
			_m_max_strength = new ProtoMemberUInt32(2, false);
			_m_max_strength.member_value = __m_max_strength;
			_m_chapters = new ProtoMemberEmbeddedList<PlayerPveChapter>(3, false);
		}

		public uint m_cur_strength
		{
			get{ return _m_cur_strength.member_value; }
			set{ _m_cur_strength.member_value = value; }
		}
		public bool has_m_cur_strength
		{
			get{ return _m_cur_strength.has_value; }
		}

		public uint m_max_strength
		{
			get{ return _m_max_strength.member_value; }
			set{ _m_max_strength.member_value = value; }
		}
		public bool has_m_max_strength
		{
			get{ return _m_max_strength.has_value; }
		}

		public System.Collections.Generic.List<PlayerPveChapter> m_chapters
		{
			get{ return _m_chapters.member_value; }
		}
		public bool has_m_chapters
		{
			get{ return _m_chapters.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_cur_strength.Serialize(_m_cur_strength.member_value, ref out_stream);

			count += _m_max_strength.Serialize(_m_max_strength.member_value, ref out_stream);

			foreach(PlayerPveChapter one_member_value in _m_chapters.member_value)
			{
				count += _m_chapters.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_cur_strength = 0;
			one_count = _m_cur_strength.ParseFrom(ref temp_m_cur_strength, ref int_stream);
			if (0 < one_count)
			{
					_m_cur_strength.member_value = temp_m_cur_strength;
					count = count + one_count;
			}

			uint temp_m_max_strength = 0;
			one_count = _m_max_strength.ParseFrom(ref temp_m_max_strength, ref int_stream);
			if (0 < one_count)
			{
					_m_max_strength.member_value = temp_m_max_strength;
					count = count + one_count;
			}

			while (true)
			{
				PlayerPveChapter one_member_value = new PlayerPveChapter();
				one_count = _m_chapters.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_chapters.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class PlayerPveChapter : IMessage
	{
		private ProtoMemberUInt64 _m_chapter_id;	// 关卡id（章id+回id+关id）
		private ProtoMemberEmbeddedList<PlayerPveChapterTile> _m_tiles;	// 关卡格子信息
		private ProtoMemberEmbeddedList<PlayerPveChapterHero> _m_heros;	// 关卡参战英雄信息
		private ProtoMemberString _m_reached_info;	// 客户端使用的达成条件信息
		private ProtoMemberString _m_passed_info;	// 客户端使用的通关路线信息

		public PlayerPveChapter()
		{
			_m_chapter_id = new ProtoMemberUInt64(1, false);
			_m_tiles = new ProtoMemberEmbeddedList<PlayerPveChapterTile>(2, false);
			_m_heros = new ProtoMemberEmbeddedList<PlayerPveChapterHero>(3, false);
			_m_reached_info = new ProtoMemberString(4, false);
			_m_passed_info = new ProtoMemberString(5, false);
		}

		public PlayerPveChapter(ulong __m_chapter_id, string __m_reached_info, string __m_passed_info)
		{
			_m_chapter_id = new ProtoMemberUInt64(1, false);
			_m_chapter_id.member_value = __m_chapter_id;
			_m_tiles = new ProtoMemberEmbeddedList<PlayerPveChapterTile>(2, false);
			_m_heros = new ProtoMemberEmbeddedList<PlayerPveChapterHero>(3, false);
			_m_reached_info = new ProtoMemberString(4, false);
			_m_reached_info.member_value = __m_reached_info;
			_m_passed_info = new ProtoMemberString(5, false);
			_m_passed_info.member_value = __m_passed_info;
		}

		public ulong m_chapter_id
		{
			get{ return _m_chapter_id.member_value; }
			set{ _m_chapter_id.member_value = value; }
		}
		public bool has_m_chapter_id
		{
			get{ return _m_chapter_id.has_value; }
		}

		public System.Collections.Generic.List<PlayerPveChapterTile> m_tiles
		{
			get{ return _m_tiles.member_value; }
		}
		public bool has_m_tiles
		{
			get{ return _m_tiles.has_value; }
		}

		public System.Collections.Generic.List<PlayerPveChapterHero> m_heros
		{
			get{ return _m_heros.member_value; }
		}
		public bool has_m_heros
		{
			get{ return _m_heros.has_value; }
		}

		public string m_reached_info
		{
			get{ return _m_reached_info.member_value; }
			set{ _m_reached_info.member_value = value; }
		}
		public bool has_m_reached_info
		{
			get{ return _m_reached_info.has_value; }
		}

		public string m_passed_info
		{
			get{ return _m_passed_info.member_value; }
			set{ _m_passed_info.member_value = value; }
		}
		public bool has_m_passed_info
		{
			get{ return _m_passed_info.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_chapter_id.Serialize(_m_chapter_id.member_value, ref out_stream);

			foreach(PlayerPveChapterTile one_member_value in _m_tiles.member_value)
			{
				count += _m_tiles.Serialize(one_member_value, ref out_stream);
			}

			foreach(PlayerPveChapterHero one_member_value in _m_heros.member_value)
			{
				count += _m_heros.Serialize(one_member_value, ref out_stream);
			}

			count += _m_reached_info.Serialize(_m_reached_info.member_value, ref out_stream);

			count += _m_passed_info.Serialize(_m_passed_info.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_chapter_id = 0;
			one_count = _m_chapter_id.ParseFrom(ref temp_m_chapter_id, ref int_stream);
			if (0 < one_count)
			{
					_m_chapter_id.member_value = temp_m_chapter_id;
					count = count + one_count;
			}

			while (true)
			{
				PlayerPveChapterTile one_member_value = new PlayerPveChapterTile();
				one_count = _m_tiles.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_tiles.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			while (true)
			{
				PlayerPveChapterHero one_member_value = new PlayerPveChapterHero();
				one_count = _m_heros.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_heros.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			string temp_m_reached_info = "";
			one_count = _m_reached_info.ParseFrom(ref temp_m_reached_info, ref int_stream);
			if (0 < one_count)
			{
					_m_reached_info.member_value = temp_m_reached_info;
					count = count + one_count;
			}

			string temp_m_passed_info = "";
			one_count = _m_passed_info.ParseFrom(ref temp_m_passed_info, ref int_stream);
			if (0 < one_count)
			{
					_m_passed_info.member_value = temp_m_passed_info;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerPveChapterHero : IMessage
	{
		private ProtoMemberUInt32 _m_hero_id;	// 关卡参战英雄id（ActorBaseID）
		private ProtoMemberUInt32 _m_last_hp;	// 关卡参战英雄剩余血量

		public PlayerPveChapterHero()
		{
			_m_hero_id = new ProtoMemberUInt32(1, false);
			_m_last_hp = new ProtoMemberUInt32(2, false);
		}

		public PlayerPveChapterHero(uint __m_hero_id, uint __m_last_hp)
		{
			_m_hero_id = new ProtoMemberUInt32(1, false);
			_m_hero_id.member_value = __m_hero_id;
			_m_last_hp = new ProtoMemberUInt32(2, false);
			_m_last_hp.member_value = __m_last_hp;
		}

		public uint m_hero_id
		{
			get{ return _m_hero_id.member_value; }
			set{ _m_hero_id.member_value = value; }
		}
		public bool has_m_hero_id
		{
			get{ return _m_hero_id.has_value; }
		}

		public uint m_last_hp
		{
			get{ return _m_last_hp.member_value; }
			set{ _m_last_hp.member_value = value; }
		}
		public bool has_m_last_hp
		{
			get{ return _m_last_hp.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_hero_id.Serialize(_m_hero_id.member_value, ref out_stream);

			count += _m_last_hp.Serialize(_m_last_hp.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_hero_id = 0;
			one_count = _m_hero_id.ParseFrom(ref temp_m_hero_id, ref int_stream);
			if (0 < one_count)
			{
					_m_hero_id.member_value = temp_m_hero_id;
					count = count + one_count;
			}

			uint temp_m_last_hp = 0;
			one_count = _m_last_hp.ParseFrom(ref temp_m_last_hp, ref int_stream);
			if (0 < one_count)
			{
					_m_last_hp.member_value = temp_m_last_hp;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerPveChapterTile : IMessage
	{
		private ProtoMemberUInt32 _m_tile_id;	// 关卡格子id
		private ProtoMemberUInt32 _m_win;	// 战斗结果。0：尚未开始，1：打过失败，2：打过胜利

		public PlayerPveChapterTile()
		{
			_m_tile_id = new ProtoMemberUInt32(1, false);
			_m_win = new ProtoMemberUInt32(2, false);
		}

		public PlayerPveChapterTile(uint __m_tile_id, uint __m_win)
		{
			_m_tile_id = new ProtoMemberUInt32(1, false);
			_m_tile_id.member_value = __m_tile_id;
			_m_win = new ProtoMemberUInt32(2, false);
			_m_win.member_value = __m_win;
		}

		public uint m_tile_id
		{
			get{ return _m_tile_id.member_value; }
			set{ _m_tile_id.member_value = value; }
		}
		public bool has_m_tile_id
		{
			get{ return _m_tile_id.has_value; }
		}

		public uint m_win
		{
			get{ return _m_win.member_value; }
			set{ _m_win.member_value = value; }
		}
		public bool has_m_win
		{
			get{ return _m_win.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_tile_id.Serialize(_m_tile_id.member_value, ref out_stream);

			count += _m_win.Serialize(_m_win.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_tile_id = 0;
			one_count = _m_tile_id.ParseFrom(ref temp_m_tile_id, ref int_stream);
			if (0 < one_count)
			{
					_m_tile_id.member_value = temp_m_tile_id;
					count = count + one_count;
			}

			uint temp_m_win = 0;
			one_count = _m_win.ParseFrom(ref temp_m_win, ref int_stream);
			if (0 < one_count)
			{
					_m_win.member_value = temp_m_win;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerRankInfo : IMessage
	{
		private ProtoMemberUInt64 _id;	//玩家id
		private ProtoMemberString _name;	//玩家名字
		private ProtoMemberUInt32 _score;	

		public PlayerRankInfo()
		{
			_id = new ProtoMemberUInt64(1, false);
			_name = new ProtoMemberString(2, false);
			_score = new ProtoMemberUInt32(3, false);
		}

		public PlayerRankInfo(ulong __id, string __name, uint __score)
		{
			_id = new ProtoMemberUInt64(1, false);
			_id.member_value = __id;
			_name = new ProtoMemberString(2, false);
			_name.member_value = __name;
			_score = new ProtoMemberUInt32(3, false);
			_score.member_value = __score;
		}

		public ulong id
		{
			get{ return _id.member_value; }
			set{ _id.member_value = value; }
		}
		public bool has_id
		{
			get{ return _id.has_value; }
		}

		public string name
		{
			get{ return _name.member_value; }
			set{ _name.member_value = value; }
		}
		public bool has_name
		{
			get{ return _name.has_value; }
		}

		public uint score
		{
			get{ return _score.member_value; }
			set{ _score.member_value = value; }
		}
		public bool has_score
		{
			get{ return _score.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _id.Serialize(_id.member_value, ref out_stream);

			count += _name.Serialize(_name.member_value, ref out_stream);

			count += _score.Serialize(_score.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_id = 0;
			one_count = _id.ParseFrom(ref temp_id, ref int_stream);
			if (0 < one_count)
			{
					_id.member_value = temp_id;
					count = count + one_count;
			}

			string temp_name = "";
			one_count = _name.ParseFrom(ref temp_name, ref int_stream);
			if (0 < one_count)
			{
					_name.member_value = temp_name;
					count = count + one_count;
			}

			uint temp_score = 0;
			one_count = _score.ParseFrom(ref temp_score, ref int_stream);
			if (0 < one_count)
			{
					_score.member_value = temp_score;
					count = count + one_count;
			}

			return count;
		}
	}

	public class PlayerTravelBag : IMessage
	{
		private ProtoMemberEmbeddedList<PlayerTravelBagItem> _m_items;	// 列表

		public PlayerTravelBag()
		{
			_m_items = new ProtoMemberEmbeddedList<PlayerTravelBagItem>(1, false);
		}

		public System.Collections.Generic.List<PlayerTravelBagItem> m_items
		{
			get{ return _m_items.member_value; }
		}
		public bool has_m_items
		{
			get{ return _m_items.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			foreach(PlayerTravelBagItem one_member_value in _m_items.member_value)
			{
				count += _m_items.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			while (true)
			{
				PlayerTravelBagItem one_member_value = new PlayerTravelBagItem();
				one_count = _m_items.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_items.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class PlayerTravelBagItem : IMessage
	{
		private ProtoMemberUInt32 _m_id;	// 位置id
		private ProtoMemberUInt64 _m_item_id;	// 道具id

		public PlayerTravelBagItem()
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_item_id = new ProtoMemberUInt64(2, false);
		}

		public PlayerTravelBagItem(uint __m_id, ulong __m_item_id)
		{
			_m_id = new ProtoMemberUInt32(1, false);
			_m_id.member_value = __m_id;
			_m_item_id = new ProtoMemberUInt64(2, false);
			_m_item_id.member_value = __m_item_id;
		}

		public uint m_id
		{
			get{ return _m_id.member_value; }
			set{ _m_id.member_value = value; }
		}
		public bool has_m_id
		{
			get{ return _m_id.has_value; }
		}

		public ulong m_item_id
		{
			get{ return _m_item_id.member_value; }
			set{ _m_item_id.member_value = value; }
		}
		public bool has_m_item_id
		{
			get{ return _m_item_id.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_id.Serialize(_m_id.member_value, ref out_stream);

			count += _m_item_id.Serialize(_m_item_id.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			uint temp_m_id = 0;
			one_count = _m_id.ParseFrom(ref temp_m_id, ref int_stream);
			if (0 < one_count)
			{
					_m_id.member_value = temp_m_id;
					count = count + one_count;
			}

			ulong temp_m_item_id = 0;
			one_count = _m_item_id.ParseFrom(ref temp_m_item_id, ref int_stream);
			if (0 < one_count)
			{
					_m_item_id.member_value = temp_m_item_id;
					count = count + one_count;
			}

			return count;
		}
	}

	public enum PosType
	{
		[System.ComponentModel.Description("PosType_Bag_Food=1")]
		PosType_Bag_Food = 1,
		[System.ComponentModel.Description("PosType_Bag_Luck=2")]
		PosType_Bag_Luck = 2,
		[System.ComponentModel.Description("PosType_Bag_Equip1=3")]
		PosType_Bag_Equip1 = 3,
		[System.ComponentModel.Description("PosType_Bag_Equip2=4")]
		PosType_Bag_Equip2 = 4,
		[System.ComponentModel.Description("PosType_Home_Food1=11")]
		PosType_Home_Food1 = 11,
		[System.ComponentModel.Description("PosType_Home_Food2=12")]
		PosType_Home_Food2 = 12,
		[System.ComponentModel.Description("PosType_Home_Luck1=13")]
		PosType_Home_Luck1 = 13,
		[System.ComponentModel.Description("PosType_Home_Luck2=14")]
		PosType_Home_Luck2 = 14,
		[System.ComponentModel.Description("PosType_Home_Equip1=15")]
		PosType_Home_Equip1 = 15,
		[System.ComponentModel.Description("PosType_Home_Equip2=16")]
		PosType_Home_Equip2 = 16,
		[System.ComponentModel.Description("PosType_Home_Equip3=17")]
		PosType_Home_Equip3 = 17,
		[System.ComponentModel.Description("PosType_Home_Equip4=18")]
		PosType_Home_Equip4 = 18,
		[System.ComponentModel.Description("PosType_Max=19")]
		PosType_Max = 19,
	}

	public enum RankType
	{
		[System.ComponentModel.Description("RankType_None=0")]
		RankType_None = 0,                              
		[System.ComponentModel.Description("成就")]
		RankType_Achievement = 1,                        //成就
		[System.ComponentModel.Description("副本")]
		RankType_PVE = 2,                                //副本
		[System.ComponentModel.Description("卡牌")]
		RankType_Card = 3,                                //卡牌
		[System.ComponentModel.Description("RankType_Max=4")]
		RankType_Max = 4, 
	}

	public class RoomInfo : IMessage
	{
		private ProtoMemberUInt64 _m_guid;	// 房间唯一id
		private ProtoMemberUInt64List _m_player_ids;	// 房间玩家列表
		private ProtoMemberUInt32 _m_chapter_id;	// 关卡id
		private ProtoMemberStringList _m_player_names;	// 房间玩家名称列表
		private ProtoMemberUInt32List _m_player_weapon;	// 房间玩家武器
		private ProtoMemberUInt32 _m_fight_type;	// 战斗类型

		public RoomInfo()
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_player_ids = new ProtoMemberUInt64List(2, false);
			_m_chapter_id = new ProtoMemberUInt32(3, false);
			_m_player_names = new ProtoMemberStringList(4, false);
			_m_player_weapon = new ProtoMemberUInt32List(5, false);
			_m_fight_type = new ProtoMemberUInt32(6, false);
		}

		public RoomInfo(ulong __m_guid, uint __m_chapter_id, uint __m_fight_type)
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_guid.member_value = __m_guid;
			_m_player_ids = new ProtoMemberUInt64List(2, false);
			_m_chapter_id = new ProtoMemberUInt32(3, false);
			_m_chapter_id.member_value = __m_chapter_id;
			_m_player_names = new ProtoMemberStringList(4, false);
			_m_player_weapon = new ProtoMemberUInt32List(5, false);
			_m_fight_type = new ProtoMemberUInt32(6, false);
			_m_fight_type.member_value = __m_fight_type;
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public System.Collections.Generic.List<ulong> m_player_ids
		{
			get{ return _m_player_ids.member_value; }
		}
		public bool has_m_player_ids
		{
			get{ return _m_player_ids.has_value; }
		}

		public uint m_chapter_id
		{
			get{ return _m_chapter_id.member_value; }
			set{ _m_chapter_id.member_value = value; }
		}
		public bool has_m_chapter_id
		{
			get{ return _m_chapter_id.has_value; }
		}

		public System.Collections.Generic.List<string> m_player_names
		{
			get{ return _m_player_names.member_value; }
		}
		public bool has_m_player_names
		{
			get{ return _m_player_names.has_value; }
		}

		public System.Collections.Generic.List<uint> m_player_weapon
		{
			get{ return _m_player_weapon.member_value; }
		}
		public bool has_m_player_weapon
		{
			get{ return _m_player_weapon.has_value; }
		}

		public uint m_fight_type
		{
			get{ return _m_fight_type.member_value; }
			set{ _m_fight_type.member_value = value; }
		}
		public bool has_m_fight_type
		{
			get{ return _m_fight_type.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			foreach(ulong one_member_value in _m_player_ids.member_value)
			{
				count += _m_player_ids.Serialize(one_member_value, ref out_stream);
			}

			count += _m_chapter_id.Serialize(_m_chapter_id.member_value, ref out_stream);

			foreach(string one_member_value in _m_player_names.member_value)
			{
				count += _m_player_names.Serialize(one_member_value, ref out_stream);
			}

			foreach(uint one_member_value in _m_player_weapon.member_value)
			{
				count += _m_player_weapon.Serialize(one_member_value, ref out_stream);
			}

			count += _m_fight_type.Serialize(_m_fight_type.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			while (true)
			{
				ulong one_member_value = 0;
				one_count = _m_player_ids.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_player_ids.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			uint temp_m_chapter_id = 0;
			one_count = _m_chapter_id.ParseFrom(ref temp_m_chapter_id, ref int_stream);
			if (0 < one_count)
			{
					_m_chapter_id.member_value = temp_m_chapter_id;
					count = count + one_count;
			}

			while (true)
			{
				string one_member_value = "";
				one_count = _m_player_names.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_player_names.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			while (true)
			{
				uint one_member_value = 0;
				one_count = _m_player_weapon.ParseFrom(ref one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_player_weapon.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			uint temp_m_fight_type = 0;
			one_count = _m_fight_type.ParseFrom(ref temp_m_fight_type, ref int_stream);
			if (0 < one_count)
			{
					_m_fight_type.member_value = temp_m_fight_type;
					count = count + one_count;
			}

			return count;
		}
	}

	public class RoomList : IMessage
	{
		private ProtoMemberEmbeddedList<RoomInfo> _m_room;	// 战斗局列表

		public RoomList()
		{
			_m_room = new ProtoMemberEmbeddedList<RoomInfo>(1, false);
		}

		public System.Collections.Generic.List<RoomInfo> m_room
		{
			get{ return _m_room.member_value; }
		}
		public bool has_m_room
		{
			get{ return _m_room.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			foreach(RoomInfo one_member_value in _m_room.member_value)
			{
				count += _m_room.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			while (true)
			{
				RoomInfo one_member_value = new RoomInfo();
				one_count = _m_room.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_room.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public enum RoomPosType
	{
		[System.ComponentModel.Description("起点")]
		RoomPosType_Start = 1,                               //起点
		[System.ComponentModel.Description("问答")]
		RoomPosType_Question = 2,                            //问答
		[System.ComponentModel.Description("移动")]
		RoomPosType_Move = 3,								//移动
		[System.ComponentModel.Description("转内圈")]
		RoomPosType_ToInnerRound = 4,						//转内圈
		[System.ComponentModel.Description("魔王答题")]
		RoomPosType_DevilQuestion = 5,						//魔王答题
		[System.ComponentModel.Description("魔王答题失败回到内圈")]
		RoomPosType_DevilQuestionToInner = 6,				//魔王答题失败回到内圈
		[System.ComponentModel.Description("魔王答题成功")]
		RoomPosType_DevilAnswerRight = 7,				    //魔王答题成功
	}

	public enum ServerType
	{
		[System.ComponentModel.Description("账号客户端")]
		ServerType_ClientAccount = 0,		// 账号客户端
		[System.ComponentModel.Description("平台GM客户端")]
		ServerType_ClientGM = 2,			// 平台GM客户端
		[System.ComponentModel.Description("逻辑客户端")]
		ServerType_ClientLogic = 1,			// 逻辑客户端
		[System.ComponentModel.Description("账号服")]
		ServerType_AccountDB = 3,			// 账号服
		[System.ComponentModel.Description("逻辑服网关")]
		ServerType_LogicGate = 4,			// 逻辑服网关
		[System.ComponentModel.Description("逻辑服")]
		ServerType_Logic = 5,				// 逻辑服
		[System.ComponentModel.Description("角色数据库")]
		ServerType_PlayerDB = 6,			// 角色数据库
		[System.ComponentModel.Description("日志数据库")]
		ServerType_LogDB = 7,				// 日志数据库
		[System.ComponentModel.Description("中心服")]
		ServerType_Center = 8,				// 中心服
	}

	public enum UnitCampType
	{
		[System.ComponentModel.Description("阵营1")]
		UnitCampType_PVP1						= 0,				//阵营1
		[System.ComponentModel.Description("阵营2")]
		UnitCampType_PVP2						= 1,				//阵营2
		[System.ComponentModel.Description("怪")]
		UnitCampType_Monster					= 2,				//怪
	}

	public class UnitDirtyData : IMessage
	{
		private ProtoMemberEmbeddedList<UnitProperty> _m_properties;	// 属性列表

		public UnitDirtyData()
		{
			_m_properties = new ProtoMemberEmbeddedList<UnitProperty>(1, false);
		}

		public System.Collections.Generic.List<UnitProperty> m_properties
		{
			get{ return _m_properties.member_value; }
		}
		public bool has_m_properties
		{
			get{ return _m_properties.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			foreach(UnitProperty one_member_value in _m_properties.member_value)
			{
				count += _m_properties.Serialize(one_member_value, ref out_stream);
			}

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			while (true)
			{
				UnitProperty one_member_value = new UnitProperty();
				one_count = _m_properties.ParseFrom(one_member_value, ref int_stream);
				if (0 < one_count)
				{
					_m_properties.member_value.Add(one_member_value);
					count = count + one_count;
				}
				else
				{
					break;
				}
			}

			return count;
		}
	}

	public class UnitInfo : IMessage
	{
		private ProtoMemberUInt64 _m_guid;	// 单位唯一id
		private ProtoMemberUInt32 _m_type;	// 更新类型（UnitUpdateType）
		private ProtoMemberEmbedded<UnitDirtyData> _m_data;	// 属性列表（更新时脏数据方式下发）

		public UnitInfo()
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_type = new ProtoMemberUInt32(2, false);
			_m_data = new ProtoMemberEmbedded<UnitDirtyData>(3, false);
			_m_data.member_value = new UnitDirtyData();
		}

		public UnitInfo(ulong __m_guid, uint __m_type)
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_guid.member_value = __m_guid;
			_m_type = new ProtoMemberUInt32(2, false);
			_m_type.member_value = __m_type;
			_m_data = new ProtoMemberEmbedded<UnitDirtyData>(3, false);
			_m_data.member_value = new UnitDirtyData();
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public uint m_type
		{
			get{ return _m_type.member_value; }
			set{ _m_type.member_value = value; }
		}
		public bool has_m_type
		{
			get{ return _m_type.has_value; }
		}

		public UnitDirtyData m_data
		{
			get{ return _m_data.member_value as UnitDirtyData; }
			set{ _m_data.member_value = value; }
		}
		public bool has_m_data
		{
			get{ return _m_data.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			count += _m_type.Serialize(_m_type.member_value, ref out_stream);

			count += _m_data.Serialize(_m_data.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			uint temp_m_type = 0;
			one_count = _m_type.ParseFrom(ref temp_m_type, ref int_stream);
			if (0 < one_count)
			{
					_m_type.member_value = temp_m_type;
					count = count + one_count;
			}

			UnitDirtyData temp_m_data = new UnitDirtyData();
			one_count = _m_data.ParseFrom(temp_m_data, ref int_stream);
			if (0 < one_count)
			{
					_m_data.member_value = temp_m_data;
					count = count + one_count;
			}

			return count;
		}
	}

	public class UnitMoveInfo : IMessage
	{
		private ProtoMemberUInt64 _m_guid;	// 单位唯一id
		private ProtoMemberInt32 _m_fm_x;	// 起点坐标
		private ProtoMemberInt32 _m_fm_y;	
		private ProtoMemberInt32 _m_fm_z;	
		private ProtoMemberInt32 _m_to_x;	// 终点坐标/停止坐标
		private ProtoMemberInt32 _m_to_y;	
		private ProtoMemberInt32 _m_to_z;	
		private ProtoMemberInt32 _m_orient;	// 在终点朝向
		private ProtoMemberUInt32 _m_type;	// 行走操作类型
		private ProtoMemberUInt32 _m_speed;	// 速度
		private ProtoMemberString _m_string;	// 客户端自定义字符串信息

		public UnitMoveInfo()
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_fm_x = new ProtoMemberInt32(2, false);
			_m_fm_y = new ProtoMemberInt32(3, false);
			_m_fm_z = new ProtoMemberInt32(4, false);
			_m_to_x = new ProtoMemberInt32(5, false);
			_m_to_y = new ProtoMemberInt32(6, false);
			_m_to_z = new ProtoMemberInt32(7, false);
			_m_orient = new ProtoMemberInt32(8, false);
			_m_type = new ProtoMemberUInt32(9, false);
			_m_speed = new ProtoMemberUInt32(10, false);
			_m_string = new ProtoMemberString(11, false);
		}

		public UnitMoveInfo(ulong __m_guid, int __m_fm_x, int __m_fm_y, int __m_fm_z, int __m_to_x, int __m_to_y, int __m_to_z, int __m_orient, uint __m_type, uint __m_speed, string __m_string)
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_guid.member_value = __m_guid;
			_m_fm_x = new ProtoMemberInt32(2, false);
			_m_fm_x.member_value = __m_fm_x;
			_m_fm_y = new ProtoMemberInt32(3, false);
			_m_fm_y.member_value = __m_fm_y;
			_m_fm_z = new ProtoMemberInt32(4, false);
			_m_fm_z.member_value = __m_fm_z;
			_m_to_x = new ProtoMemberInt32(5, false);
			_m_to_x.member_value = __m_to_x;
			_m_to_y = new ProtoMemberInt32(6, false);
			_m_to_y.member_value = __m_to_y;
			_m_to_z = new ProtoMemberInt32(7, false);
			_m_to_z.member_value = __m_to_z;
			_m_orient = new ProtoMemberInt32(8, false);
			_m_orient.member_value = __m_orient;
			_m_type = new ProtoMemberUInt32(9, false);
			_m_type.member_value = __m_type;
			_m_speed = new ProtoMemberUInt32(10, false);
			_m_speed.member_value = __m_speed;
			_m_string = new ProtoMemberString(11, false);
			_m_string.member_value = __m_string;
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public int m_fm_x
		{
			get{ return _m_fm_x.member_value; }
			set{ _m_fm_x.member_value = value; }
		}
		public bool has_m_fm_x
		{
			get{ return _m_fm_x.has_value; }
		}

		public int m_fm_y
		{
			get{ return _m_fm_y.member_value; }
			set{ _m_fm_y.member_value = value; }
		}
		public bool has_m_fm_y
		{
			get{ return _m_fm_y.has_value; }
		}

		public int m_fm_z
		{
			get{ return _m_fm_z.member_value; }
			set{ _m_fm_z.member_value = value; }
		}
		public bool has_m_fm_z
		{
			get{ return _m_fm_z.has_value; }
		}

		public int m_to_x
		{
			get{ return _m_to_x.member_value; }
			set{ _m_to_x.member_value = value; }
		}
		public bool has_m_to_x
		{
			get{ return _m_to_x.has_value; }
		}

		public int m_to_y
		{
			get{ return _m_to_y.member_value; }
			set{ _m_to_y.member_value = value; }
		}
		public bool has_m_to_y
		{
			get{ return _m_to_y.has_value; }
		}

		public int m_to_z
		{
			get{ return _m_to_z.member_value; }
			set{ _m_to_z.member_value = value; }
		}
		public bool has_m_to_z
		{
			get{ return _m_to_z.has_value; }
		}

		public int m_orient
		{
			get{ return _m_orient.member_value; }
			set{ _m_orient.member_value = value; }
		}
		public bool has_m_orient
		{
			get{ return _m_orient.has_value; }
		}

		public uint m_type
		{
			get{ return _m_type.member_value; }
			set{ _m_type.member_value = value; }
		}
		public bool has_m_type
		{
			get{ return _m_type.has_value; }
		}

		public uint m_speed
		{
			get{ return _m_speed.member_value; }
			set{ _m_speed.member_value = value; }
		}
		public bool has_m_speed
		{
			get{ return _m_speed.has_value; }
		}

		public string m_string
		{
			get{ return _m_string.member_value; }
			set{ _m_string.member_value = value; }
		}
		public bool has_m_string
		{
			get{ return _m_string.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			count += _m_fm_x.Serialize(_m_fm_x.member_value, ref out_stream);

			count += _m_fm_y.Serialize(_m_fm_y.member_value, ref out_stream);

			count += _m_fm_z.Serialize(_m_fm_z.member_value, ref out_stream);

			count += _m_to_x.Serialize(_m_to_x.member_value, ref out_stream);

			count += _m_to_y.Serialize(_m_to_y.member_value, ref out_stream);

			count += _m_to_z.Serialize(_m_to_z.member_value, ref out_stream);

			count += _m_orient.Serialize(_m_orient.member_value, ref out_stream);

			count += _m_type.Serialize(_m_type.member_value, ref out_stream);

			count += _m_speed.Serialize(_m_speed.member_value, ref out_stream);

			count += _m_string.Serialize(_m_string.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			int temp_m_fm_x = 0;
			one_count = _m_fm_x.ParseFrom(ref temp_m_fm_x, ref int_stream);
			if (0 < one_count)
			{
					_m_fm_x.member_value = temp_m_fm_x;
					count = count + one_count;
			}

			int temp_m_fm_y = 0;
			one_count = _m_fm_y.ParseFrom(ref temp_m_fm_y, ref int_stream);
			if (0 < one_count)
			{
					_m_fm_y.member_value = temp_m_fm_y;
					count = count + one_count;
			}

			int temp_m_fm_z = 0;
			one_count = _m_fm_z.ParseFrom(ref temp_m_fm_z, ref int_stream);
			if (0 < one_count)
			{
					_m_fm_z.member_value = temp_m_fm_z;
					count = count + one_count;
			}

			int temp_m_to_x = 0;
			one_count = _m_to_x.ParseFrom(ref temp_m_to_x, ref int_stream);
			if (0 < one_count)
			{
					_m_to_x.member_value = temp_m_to_x;
					count = count + one_count;
			}

			int temp_m_to_y = 0;
			one_count = _m_to_y.ParseFrom(ref temp_m_to_y, ref int_stream);
			if (0 < one_count)
			{
					_m_to_y.member_value = temp_m_to_y;
					count = count + one_count;
			}

			int temp_m_to_z = 0;
			one_count = _m_to_z.ParseFrom(ref temp_m_to_z, ref int_stream);
			if (0 < one_count)
			{
					_m_to_z.member_value = temp_m_to_z;
					count = count + one_count;
			}

			int temp_m_orient = 0;
			one_count = _m_orient.ParseFrom(ref temp_m_orient, ref int_stream);
			if (0 < one_count)
			{
					_m_orient.member_value = temp_m_orient;
					count = count + one_count;
			}

			uint temp_m_type = 0;
			one_count = _m_type.ParseFrom(ref temp_m_type, ref int_stream);
			if (0 < one_count)
			{
					_m_type.member_value = temp_m_type;
					count = count + one_count;
			}

			uint temp_m_speed = 0;
			one_count = _m_speed.ParseFrom(ref temp_m_speed, ref int_stream);
			if (0 < one_count)
			{
					_m_speed.member_value = temp_m_speed;
					count = count + one_count;
			}

			string temp_m_string = "";
			one_count = _m_string.ParseFrom(ref temp_m_string, ref int_stream);
			if (0 < one_count)
			{
					_m_string.member_value = temp_m_string;
					count = count + one_count;
			}

			return count;
		}
	}

	public class UnitProperty : IMessage
	{
		private ProtoMemberUInt64 _m_property_type;	// 属性类型
		private ProtoMemberUInt64 _m_property_value;	// 属性值

		public UnitProperty()
		{
			_m_property_type = new ProtoMemberUInt64(1, false);
			_m_property_value = new ProtoMemberUInt64(2, false);
		}

		public UnitProperty(ulong __m_property_type, ulong __m_property_value)
		{
			_m_property_type = new ProtoMemberUInt64(1, false);
			_m_property_type.member_value = __m_property_type;
			_m_property_value = new ProtoMemberUInt64(2, false);
			_m_property_value.member_value = __m_property_value;
		}

		public ulong m_property_type
		{
			get{ return _m_property_type.member_value; }
			set{ _m_property_type.member_value = value; }
		}
		public bool has_m_property_type
		{
			get{ return _m_property_type.has_value; }
		}

		public ulong m_property_value
		{
			get{ return _m_property_value.member_value; }
			set{ _m_property_value.member_value = value; }
		}
		public bool has_m_property_value
		{
			get{ return _m_property_value.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_property_type.Serialize(_m_property_type.member_value, ref out_stream);

			count += _m_property_value.Serialize(_m_property_value.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_property_type = 0;
			one_count = _m_property_type.ParseFrom(ref temp_m_property_type, ref int_stream);
			if (0 < one_count)
			{
					_m_property_type.member_value = temp_m_property_type;
					count = count + one_count;
			}

			ulong temp_m_property_value = 0;
			one_count = _m_property_value.ParseFrom(ref temp_m_property_value, ref int_stream);
			if (0 < one_count)
			{
					_m_property_value.member_value = temp_m_property_value;
					count = count + one_count;
			}

			return count;
		}
	}

	public enum UnitPropertyType
	{
		[System.ComponentModel.Description("所属玩家的ID")]
		UnitPropertyType_PlayerGuid				= 0,				//所属玩家的ID
		[System.ComponentModel.Description("控制者的ID")]
		UnitPropertyType_ControlerGuid			= 1,				//控制者的ID
		[System.ComponentModel.Description("ActorID")]
		UnitPropertyType_ActorId				= 2,				//ActorID
		[System.ComponentModel.Description("组ID")]
		UnitPropertyType_GroupId				= 3,				//组ID
		[System.ComponentModel.Description("组下标")]
		UnitPropertyType_GroupIndex				= 4,				//组下标
		[System.ComponentModel.Description("阵营（UnitCampType）")]
		UnitPropertyType_Camp					= 5,				//阵营（UnitCampType）
		[System.ComponentModel.Description("最大血量")]
		UnitPropertyType_MaxHp					= 7,				//最大血量
		[System.ComponentModel.Description("当前血量")]
		UnitPropertyType_CurrHp					= 8,				//当前血量
		[System.ComponentModel.Description("父怪ActorID")]
		UnitPropertyType_FatherActorId			= 9,				//父怪ActorID
		[System.ComponentModel.Description("Boss状态")]
		UnitPropertyType_BossState				= 10,				//Boss状态
		[System.ComponentModel.Description("AtkerID，血量改变是谁造成的")]
		UnitPropertyType_AtkerID				= 11,				//AtkerID，血量改变是谁造成的
		[System.ComponentModel.Description("护盾")]
		UnitPropertyType_Shield					= 12,				//护盾
		[System.ComponentModel.Description("护盾时间条")]
		UnitPropertyType_ShieldTime				= 13,				//护盾时间条
		[System.ComponentModel.Description("分数")]
		UnitPropertyType_Score					= 14,				//分数
		[System.ComponentModel.Description("武器类型")]
		UnitPropertyType_Weapon					= 15,				//武器类型
	}

	public class UnitSkillInfo : IMessage
	{
		private ProtoMemberUInt64 _m_guid;	// 单位唯一id
		private ProtoMemberUInt32 _m_skill_id;	// 技能id
		private ProtoMemberUInt32 _m_result_index;	// 技能效果下标
		private ProtoMemberUInt64 _m_target_guid;	// 目标id（为0则表示未击中）
		private ProtoMemberUInt32 _m_damage;	// 造成伤害
		private ProtoMemberUInt32 _m_damage_type;	// 造成伤害的类型：暴击，闪避，弱点等
		private ProtoMemberString _m_string;	// 客户端自定义字符串信息

		public UnitSkillInfo()
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_skill_id = new ProtoMemberUInt32(2, false);
			_m_result_index = new ProtoMemberUInt32(3, false);
			_m_target_guid = new ProtoMemberUInt64(4, false);
			_m_damage = new ProtoMemberUInt32(5, false);
			_m_damage_type = new ProtoMemberUInt32(6, false);
			_m_string = new ProtoMemberString(7, false);
		}

		public UnitSkillInfo(ulong __m_guid, uint __m_skill_id, uint __m_result_index, ulong __m_target_guid, uint __m_damage, uint __m_damage_type, string __m_string)
		{
			_m_guid = new ProtoMemberUInt64(1, false);
			_m_guid.member_value = __m_guid;
			_m_skill_id = new ProtoMemberUInt32(2, false);
			_m_skill_id.member_value = __m_skill_id;
			_m_result_index = new ProtoMemberUInt32(3, false);
			_m_result_index.member_value = __m_result_index;
			_m_target_guid = new ProtoMemberUInt64(4, false);
			_m_target_guid.member_value = __m_target_guid;
			_m_damage = new ProtoMemberUInt32(5, false);
			_m_damage.member_value = __m_damage;
			_m_damage_type = new ProtoMemberUInt32(6, false);
			_m_damage_type.member_value = __m_damage_type;
			_m_string = new ProtoMemberString(7, false);
			_m_string.member_value = __m_string;
		}

		public ulong m_guid
		{
			get{ return _m_guid.member_value; }
			set{ _m_guid.member_value = value; }
		}
		public bool has_m_guid
		{
			get{ return _m_guid.has_value; }
		}

		public uint m_skill_id
		{
			get{ return _m_skill_id.member_value; }
			set{ _m_skill_id.member_value = value; }
		}
		public bool has_m_skill_id
		{
			get{ return _m_skill_id.has_value; }
		}

		public uint m_result_index
		{
			get{ return _m_result_index.member_value; }
			set{ _m_result_index.member_value = value; }
		}
		public bool has_m_result_index
		{
			get{ return _m_result_index.has_value; }
		}

		public ulong m_target_guid
		{
			get{ return _m_target_guid.member_value; }
			set{ _m_target_guid.member_value = value; }
		}
		public bool has_m_target_guid
		{
			get{ return _m_target_guid.has_value; }
		}

		public uint m_damage
		{
			get{ return _m_damage.member_value; }
			set{ _m_damage.member_value = value; }
		}
		public bool has_m_damage
		{
			get{ return _m_damage.has_value; }
		}

		public uint m_damage_type
		{
			get{ return _m_damage_type.member_value; }
			set{ _m_damage_type.member_value = value; }
		}
		public bool has_m_damage_type
		{
			get{ return _m_damage_type.has_value; }
		}

		public string m_string
		{
			get{ return _m_string.member_value; }
			set{ _m_string.member_value = value; }
		}
		public bool has_m_string
		{
			get{ return _m_string.has_value; }
		}

		public int Serialize(ref MemoryStream out_stream)
		{
			int count = 0;

			count += _m_guid.Serialize(_m_guid.member_value, ref out_stream);

			count += _m_skill_id.Serialize(_m_skill_id.member_value, ref out_stream);

			count += _m_result_index.Serialize(_m_result_index.member_value, ref out_stream);

			count += _m_target_guid.Serialize(_m_target_guid.member_value, ref out_stream);

			count += _m_damage.Serialize(_m_damage.member_value, ref out_stream);

			count += _m_damage_type.Serialize(_m_damage_type.member_value, ref out_stream);

			count += _m_string.Serialize(_m_string.member_value, ref out_stream);

			return count;
		}
		public int ParseFrom(ref MemoryStream int_stream)
		{
			int count = 0;
			int one_count = 0;

			ulong temp_m_guid = 0;
			one_count = _m_guid.ParseFrom(ref temp_m_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_guid.member_value = temp_m_guid;
					count = count + one_count;
			}

			uint temp_m_skill_id = 0;
			one_count = _m_skill_id.ParseFrom(ref temp_m_skill_id, ref int_stream);
			if (0 < one_count)
			{
					_m_skill_id.member_value = temp_m_skill_id;
					count = count + one_count;
			}

			uint temp_m_result_index = 0;
			one_count = _m_result_index.ParseFrom(ref temp_m_result_index, ref int_stream);
			if (0 < one_count)
			{
					_m_result_index.member_value = temp_m_result_index;
					count = count + one_count;
			}

			ulong temp_m_target_guid = 0;
			one_count = _m_target_guid.ParseFrom(ref temp_m_target_guid, ref int_stream);
			if (0 < one_count)
			{
					_m_target_guid.member_value = temp_m_target_guid;
					count = count + one_count;
			}

			uint temp_m_damage = 0;
			one_count = _m_damage.ParseFrom(ref temp_m_damage, ref int_stream);
			if (0 < one_count)
			{
					_m_damage.member_value = temp_m_damage;
					count = count + one_count;
			}

			uint temp_m_damage_type = 0;
			one_count = _m_damage_type.ParseFrom(ref temp_m_damage_type, ref int_stream);
			if (0 < one_count)
			{
					_m_damage_type.member_value = temp_m_damage_type;
					count = count + one_count;
			}

			string temp_m_string = "";
			one_count = _m_string.ParseFrom(ref temp_m_string, ref int_stream);
			if (0 < one_count)
			{
					_m_string.member_value = temp_m_string;
					count = count + one_count;
			}

			return count;
		}
	}

	public enum UnitUpdateType
	{
		[System.ComponentModel.Description("创建")]
		UnitUpdateType_Create					= 0,				//创建
		[System.ComponentModel.Description("更新")]
		UnitUpdateType_Update					= 1,				//更新
		[System.ComponentModel.Description("删除")]
		UnitUpdateType_Delete					= 2,				//删除
	}

	public enum MsgType
	{
		enum_Msg_Connect = 0,
		enum_Msg_Ping_Req = 1,
		enum_Msg_Ping_Res = 2,
		enum_Msg_Normal = 3,
		enum_Msg_Route = 4,
		enum_Msg_Broadcast = 5,
		enum_Msg_MsgBeginFlag_Account = 1000,
		enum_Msg_Client2Account_Login_Req = 1001,
		enum_Msg_Account2Center_Reg_Login_Key_Req = 1002,
		enum_Msg_Center2Account_Reg_Login_Key_Res = 1003,
		enum_Msg_Account2Client_Login_Res = 1004,
		enum_Msg_Client2Account_Reg_Req = 1005,
		enum_Msg_Account2Client_Reg_Res = 1006,
		enum_Msg_Center2Account_Login_Req = 1007,
		enum_Msg_Center2Account_Logout_Req = 1008,
		enum_Msg_MsgEndFlag_Account = 1009,
		enum_Msg_MsgBeginFlag_GM_Server = 2000,
		enum_Msg_Center2Logic_GMOperator_Req = 2001,
		enum_Msg_MsgEndFlag_GM_Server = 2002,
		enum_Msg_MsgBeginFlag_GM_Client = 3000,
		enum_Msg_Client2Logic_GM_Req = 3001,
		enum_Msg_Logic2Client_GM_Res = 3002,
		enum_Msg_Client2Logic_Table_Update_Req = 3003,
		enum_Msg_Logic2Client_Table_Update_Res = 3004,
		enum_Msg_Table_ReLoad_Req = 3005,
		enum_Msg_Client2Logic_Log_Req = 3006,
		enum_Msg_MsgEndFlag_GM_Client = 3007,
		enum_Msg_MsgBeginFlag_LoginLogout = 4000,
		enum_Msg_Client2Gate_Connect_Req = 4001,
		enum_Msg_Gate2Center_Connect_Req = 4002,
		enum_Msg_Center2Gate_Connect_Res = 4003,
		enum_Msg_Gate2Client_Connect_Res = 4004,
		enum_Msg_Gate2Logic_Player_Login_Req = 4005,
		enum_Msg_Gate2Logic_Player_Logout_Req = 4006,
		enum_Msg_Logic2Center_Player_Login_Req = 4007,
		enum_Msg_Logic2Center_Player_Logout_Req = 4008,
		enum_Msg_Logic2Client_GmRoute_Res = 4009,
		enum_Msg_MsgEndFlag_LoginLogout = 4010,
		enum_Msg_MsgBeginFlag_Player = 5000,
		enum_Msg_Client2Logic_Player_List_Req = 5001,
		enum_Msg_Logic2Player_Player_List_Req = 5002,
		enum_Msg_Player2Logic_Player_List_Res = 5003,
		enum_Msg_Logic2Client_Player_List_Res = 5004,
		enum_Msg_Client2Logic_Create_Player_Req = 5005,
		enum_Msg_Logic2Player_Create_Player_Req = 5006,
		enum_Msg_Player2Logic_Create_Player_Res = 5007,
		enum_Msg_Logic2Client_Create_Player_Res = 5008,
		enum_Msg_Client2Logic_CheckPlayerName_Req = 5009,
		enum_Msg_Logic2Player_CheckPlayerName_Req = 5010,
		enum_Msg_Player2Logic_CheckPlayerName_Res = 5011,
		enum_Msg_Logic2Client_CheckPlayerName_Res = 5012,
		enum_Msg_Client2Logic_SetPlayerName_Req = 5013,
		enum_Msg_Logic2Player_SetPlayerName_Req = 5014,
		enum_Msg_Player2Logic_SetPlayerName_Res = 5015,
		enum_Msg_Logic2Client_SetPlayerName_Res = 5016,
		enum_Msg_Client2Logic_SetGM_Req = 5017,
		enum_Msg_Logic2Player_SetGM_Req = 5018,
		enum_Msg_Player2Logic_SetGM_Res = 5019,
		enum_Msg_Logic2Client_SetGM_Res = 5020,
		enum_Msg_Client2Logic_Player_Data_Req = 5021,
		enum_Msg_Logic2Player_Player_Info_Req = 5022,
		enum_Msg_Player2Logic_Player_Info_Res = 5023,
		enum_Msg_Logic2Client_Player_Data_Res = 5024,
		enum_Msg_Logic2Player_Player_Update_Req = 5025,
		enum_Msg_Client2Logic_Open_Box_Req = 5026,
		enum_Msg_Logic2Client_Open_Box_Res = 5027,
		enum_Msg_Logic2Client_Bag_Res = 5028,
		enum_Msg_Logic2Client_Travel_Bag_Res = 5029,
		enum_Msg_Logic2Client_PlayerBase_Res = 5030,
		enum_Msg_Logic2Client_PlayerAction_Res = 5031,
		enum_Msg_Logic2Client_PlayerPVE_Res = 5032,
		enum_Msg_Client2Logic_SaveTaskInfo_Req = 5033,
		enum_Msg_Logic2Client_SaveTaskInfo_Res = 5034,
		enum_Msg_Client2Logic_SaveContestMemInfo_Req = 5035,
		enum_Msg_Logic2Client_SaveContestMemInfo_Res = 5036,
		enum_Msg_Logic2Client_AddMailInfo_Res = 5037,
		enum_Msg_Client2Logic_OpenMail_Req = 5038,
		enum_Msg_Logic2Client_OpenMail_Res = 5039,
		enum_Msg_Client2Logic_DeleteMail_Req = 5040,
		enum_Msg_Logic2Client_DeleteMail_Res = 5041,
		enum_Msg_Client2Logic_ServerTime_Req = 5042,
		enum_Msg_Logic2Client_ServerTime_Res = 5043,
		enum_Msg_Client2Logic_Gacha_Req = 5044,
		enum_Msg_Logic2Client_Gacha_Res = 5045,
		enum_Msg_Client2Logic_Compose_Req = 5046,
		enum_Msg_Logic2Client_Compose_Res = 5047,
		enum_Msg_Client2Logic_Rank_Req = 5048,
		enum_Msg_Logic2Client_Rank_Res = 5049,
		enum_Msg_Client2Logic_Change_Avatar_Req = 5050,
		enum_Msg_Logic2Client_Change_Avatar_Res = 5051,
		enum_Msg_Client2Logic_Buy_Shop_Req = 5052,
		enum_Msg_Logic2Client_Buy_Shop_Res = 5053,
		enum_Msg_Client2Logic_Put_Bag_Req = 5054,
		enum_Msg_Logic2Client_Put_Bag_Res = 5055,
		enum_Msg_Client2Logic_Get_Offline_Candy_Req = 5056,
		enum_Msg_Logic2Client_Get_Offline_Candy_Res = 5057,
		enum_Msg_Logic2ClientTravel_Go_Out_Notify = 5058,
		enum_Msg_Logic2ClientTravel_Come_Back_Notify = 5059,
		enum_Msg_Logic2Client_One_Item_Notify = 5060,
		enum_Msg_MsgEndFlag_Player = 5061,
		enum_Msg_MsgBeginFlag_Log = 6000,
		enum_Msg_Write_Log = 6001,
		enum_Msg_Logic2Log_Write_Req = 6002,
		enum_Msg_MsgEndFlag_Log = 6003,
		enum_Msg_MsgBeginFlag_Battle = 7000,
		enum_Msg_Client2Logic_Battle_RoomList_Req = 7001,
		enum_Msg_Logic2Client_Battle_RoomList_Res = 7002,
		enum_Msg_Client2Logic_Battle_RoomCreat_Req = 7003,
		enum_Msg_Logic2Client_Battle_RoomCreat_Res = 7004,
		enum_Msg_Client2Logic_Battle_RoomJoin_Req = 7005,
		enum_Msg_Logic2Client_Battle_RoomJoin_Res = 7006,
		enum_Msg_Client2Logic_Battle_RoomExit_Req = 7007,
		enum_Msg_Logic2Client_Battle_RoomExit_Res = 7008,
		enum_Msg_Logic2Client_Battle_RoomInfo_Broadcast = 7009,
		enum_Msg_Client2Logic_Battle_Ready_Req = 7010,
		enum_Msg_Logic2Client_Battle_Begin_Broadcast = 7011,
		enum_Msg_Client2Logic_Battle_Ready2_Req = 7012,
		enum_Msg_Logic2Client_Battle_Begin2_Broadcast = 7013,
		enum_Msg_Client2Logic_Battle_Ready3_Req = 7014,
		enum_Msg_Logic2Client_Battle_Begin3_Broadcast = 7015,
		enum_Msg_Client2Logic_Battle_End_Broadcast = 7016,
		enum_Msg_Logic2Client_Battle_End_Broadcast = 7017,
		enum_Msg_Client2Logic_Battle_Move_Req = 7018,
		enum_Msg_Logic2Client_Battle_Move_Broadcast = 7019,
		enum_Msg_Client2Logic_Battle_Skill_Req = 7020,
		enum_Msg_Logic2Client_Battle_Skill_Broadcast = 7021,
		enum_Msg_Client2Logic_Battle_Data_Req = 7022,
		enum_Msg_Logic2Client_Battle_Data_Broadcast = 7023,
		enum_Msg_Client2Logic_Battle_RoomReset_Req = 7024,
		enum_Msg_Logic2Client_Battle_RoomReset_Res = 7025,
		enum_Msg_Client2Logic_CreateLocalPlayer_Req = 7026,
		enum_Msg_Logic2Client_CreatedLocalPlayer_Broadcast = 7027,
		enum_Msg_Client2Logic_CreateLocalMonster_Req = 7028,
		enum_Msg_Logic2Client_CreatedLocalMonster_Broadcast = 7029,
		enum_Msg_Client2Logic_BattleSettlement_Req = 7030,
		enum_Msg_Logic2Client_BattleSettlement_Broadcast = 7031,
		enum_Msg_MsgEndFlag_Battle = 7032,
		enum_Msg_MsgBeginFlag_PVE = 8000,
		enum_Msg_Client2Logic_PVE_SubmitResult_Req = 8001,
		enum_Msg_Logic2Client_PVE_SubmitResult_Res = 8002,
		enum_Msg_Client2Logic_PVE_RecoverHP_Req = 8003,
		enum_Msg_Logic2Client_PVE_RecoverHP_Res = 8004,
		enum_Msg_Client2Logic_PVE_Finish_Req = 8005,
		enum_Msg_Logic2Client_PVE_Finish_Res = 8006,
		enum_Msg_Logic2Client_PVEDropInfo_Res = 8007,
		enum_Msg_Client2Logic_PVE_Enter_Req = 8008,
		enum_Msg_Logic2Client_PVE_Enter_Res = 8009,
		enum_Msg_Client2Logic_PVE_Create_Room_Req = 8010,
		enum_Msg_Logic2Client_PVE_Create_Room_Res = 8011,
		enum_Msg_Logic2Client_PVE_Room_Broadcast = 8012,
		enum_Msg_Client2Logic_PVE_Join_Room_Req = 8013,
		enum_Msg_Logic2Client_PVE_Join_Room_Res = 8014,
		enum_Msg_Client2Logic_PVE_Quit_Room_Req = 8015,
		enum_Msg_Client2Logic_PVE_Ready_Room_Req = 8016,
		enum_Msg_Client2Logic_PVE_Start_Game_Room_Req = 8017,
		enum_Msg_Logic2Client_PVE_Start_Game_Room_Res = 8018,
		enum_Msg_Client2Logic_PVE_Throw_Room_Req = 8019,
		enum_Msg_Logic2Client_PVE_Throw_Room_broadcast = 8020,
		enum_Msg_Client2Logic_PVE_Trigger_Effect_Room_Req = 8021,
		enum_Msg_Logic2Client_PVE_Player_Info_Room_Broadcast = 8022,
		enum_Msg_Logic2Client_PVE_Question_Room_Broadcast = 8023,
		enum_Msg_Client2Logic_PVE_Answer_Question_Room_Req = 8024,
		enum_Msg_Logic2Client_PVE_Answer_Question_Room_Broadcast = 8025,
		enum_Msg_Logic2Client_PVE_Move_Room_Broadcast = 8026,
		enum_Msg_Logic2Client_PVE_Trigger_Effect_Room_Broadcast = 8027,
		enum_Msg_Logic2Client_PVE_Devil_Question_Room_Broadcast = 8028,
		enum_Msg_Client2Logic_LoadState_Req = 8029,
		enum_Msg_Logic2Client_LoadState_Broadcast = 8030,
		enum_Msg_Client2Logic_PVE_SetWinNum_Room_Req = 8031,
		enum_Msg_Client2Logic_PVE_SetGameState_Room_Req = 8032,
		enum_Msg_MsgEndFlag_PVE = 8033,
		enum_Msg_MsgBeginFlag_Contest = 9000,
		enum_Msg_Client2Logic_Contest_Create_Req = 9001,
		enum_Msg_Logic2Client_Contest_Create_Res = 9002,
		enum_Msg_Client2Logic_Contest_Save_Setup_Req = 9003,
		enum_Msg_Logic2Client_Contest_Save_Setup_Res = 9004,
		enum_Msg_Client2Logic_Contest_Join_Req = 9005,
		enum_Msg_Logic2Client_Contest_Info_Res = 9006,
		enum_Msg_Logic2Client_Contest_Join_Res = 9007,
		enum_Msg_Client2Logic_Contest_Score_Req = 9008,
		enum_Msg_Logic2Client_Contest_Score_Res = 9009,
		enum_Msg_Client2Logic_Contest_Rank_Req = 9010,
		enum_Msg_Logic2Client_Contest_Rank_Res = 9011,
		enum_Msg_MsgEndFlag_Contest = 9012,
		enum_Msg_MsgBeginFlag_BossBattle = 10000,
		enum_Msg_Client2Logic_CreateBossBattle_Req = 10001,
		enum_Msg_Logic2Client_CreateBossBattle_Res = 10002,
		enum_Msg_Client2Logic_JoinBossBattle_Req = 10003,
		enum_Msg_Logic2Client_JoinBossBattle_Res = 10004,
		enum_Msg_Client2Logic_BossBattleHp_Req = 10005,
		enum_Msg_Logic2Client_BossBattleHp_Broadcast = 10006,
		enum_Msg_Client2Logic_GetBossBattleRank_Req = 10007,
		enum_Msg_Logic2Client_GetBossBattleRank_Res = 10008,
		enum_Msg_MsgEndFlag_BossBattle = 10009,
		enum_Msg_MsgCount = 10010,
	}

}

