/*
TShock, a server mod for Terraria
Copyright (C) 2011-2015 Nyx Studios (fka. The TShock Team)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Rests;

namespace TShockAPI
{
	/// <summary>ConfigFile - The config file class, which contains the configuration for a server that is serialized into JSON and deserialized on load.</summary>
	public class ConfigFile
	{
        /// <summary>Map File in root flood</summary>
        [Description("地图文件是否存放在根目录")]
        public bool WorldsInRoot = false;


        /// <summary>InvasionMultiplier - The equation for calculating invasion size = 100 + (multiplier * (number of active players > 200 hp)).</summary>
        [Description(
			"入侵怪物数量=这个数字*200血以上玩家数量 + 100"
			)]
		public int InvasionMultiplier = 1;

		/// <summary>DefaultMaximumSpawns - The default max spawns per wave.</summary>
		[Description("默认最大刷怪数量。数字越大，怪物越多。")]
		public int DefaultMaximumSpawns = 5;

		/// <summary>DefaultSpawnRate - The default spawn rate.</summary>
		[Description("默认刷怪频率。数字越小，刷怪越快。")]
		public int DefaultSpawnRate = 600;

		/// <summary>ServerPort - The configured server port.</summary>
		[Description("服务器端口。")]
		public int ServerPort = 7777;

		/// <summary>EnableWhitelist - boolean if the whitelist functionality should be turned on.</summary>
		[Description("是否开启白名单。")]
		public bool EnableWhitelist;

		/// <summary>InfiniteInvasion - Whether or not infinite invasion mode should be on.</summary>
		[Description(
			"是否开启无限入侵（2000000+只怪物）。"
			)]
		public bool InfiniteInvasion;

		/// <summary>PvPMode - The server PvP mode (normal, always, or disabled).</summary>
		[Description("\"normal\"普通模式 \"always\"强制PVP模式 \"disabled\"禁止PVP模式。")]
		public string PvPMode = "normal";

		/// <summary>SpawnProtection - Enables the spawn protection system.</summary>
		[Description("是否进行复活点保护。")]
		public bool SpawnProtection = true;

		/// <summary>SpawnProtectionRadius - The spawn protection tile radius.</summary>
		[Description("复活点保护的范围。")]
		public int SpawnProtectionRadius = 10;

		/// <summary>MaxSlots - The server's max slots.</summary>
		[Description(
			"最大玩家数量。"
			)]
		public int MaxSlots = 8;

		/// <summary>RangeChecks - Whether or not the anti-grief system based on range should be enabled.</summary>
		[Description("范围检查。")]
		public bool RangeChecks = true;

		/// <summary>DisableBuild - Whether or not building should be enabled.</summary>
		[Description("全图保护。")]
		public bool DisableBuild;

		/// <summary>SuperAdminChatRGB - The chat color for the superadmin group.</summary>
		[Description("超级管理员聊天颜色。")]
		public int[] SuperAdminChatRGB = { 255, 0, 0 };

		/// <summary>SuperAdminChatPrefix - The superadmin chat prefix.</summary>
		[Description("超级管理员聊天前缀。")]
		public string SuperAdminChatPrefix = "(管理员) ";

		/// <summary>SuperAdminChatSuffix - The superadmin chat suffix.</summary>
		[Description("超级管理员聊天后缀。")]
		public string SuperAdminChatSuffix = "";

		/// <summary>BackupInterval - The backup frequency in minutes.</summary>
		[Description(
			"备份间隔 单位分钟。")]
		public int BackupInterval;

		/// <summary>BackupKeepFor - Backup max age in minutes.</summary>
		[Description("地图备份保留时间。")]
		public int BackupKeepFor = 60;

		/// <summary>RememberLeavePos - Whether or not to remember where an IP player was when they left.</summary>
		[Description(
			"记录玩家最后位置，下次进入服务器时传送回去。"
			)]
		public bool RememberLeavePos;

		/// <summary>HardcoreOnly - Whether or not HardcoreOnly should be enabled.</summary>
		[Description("仅允许困难模式的玩家进入服务器。")]
		public bool HardcoreOnly;

		/// <summary>MediumcoreOnly - Whether or not MediumCore only players should be enabled.</summary>
		[Description("仅允许中等模式的玩家进入服务器。")]
		public bool MediumcoreOnly;

		/// <summary>KickOnMediumcoreDeath - Whether or not to kick mediumcore players on death.</summary>
		[Description("踢出死亡的中等难度的玩家。")]
		public bool KickOnMediumcoreDeath;

		/// <summary>BanOnMediumcoreDeath - Whether or not to ban mediumcore players on death.</summary>
		[Description("封禁死亡的中等难度的玩家。")]
		public bool BanOnMediumcoreDeath;

		[Description("是否自动保存地图，建议开启。")]
		public bool AutoSave = true;
		[Description("自动保存的时候是否进行提示。")]
		public bool AnnounceSave = true;

		[Description("允许输错密码次数。")]
		public int MaximumLoginAttempts = 3;

		[Description("远程服务器名。")]
		public string ServerName = "";
		[Description("用服务器名代替地图名。")]
		public bool UseServerName = false;
		[Description("没吊用。")]
		public string MasterServer = "127.0.0.1";

		[Description("数据库类型，\"sqlite\" 或 \"mysql\"")]
		public string StorageType = "sqlite";

		[Description("MySQL主机名。")]
		public string MySqlHost = "localhost:3306";
		[Description("数据库名。")]
		public string MySqlDbName = "";
		[Description("数据库用户名。")]
		public string MySqlUsername = "";
		[Description("数据库密码。")]
		public string MySqlPassword = "";

		[Description("封禁死亡的中等难度的玩家。")]
		public string MediumcoreBanReason = "死亡而被封禁";
		[Description("踢出死亡的中等难度的玩家。")]
		public string MediumcoreKickReason = "死亡而被踢出";

		[Description("使用GetGroupForIPExpensive。")]
		public bool EnableDNSHostResolution;

		[Description("自动封禁IP。")]
		public bool EnableIPBans = true;

		[Description("自动封禁UUID。")]
		public bool EnableUUIDBans = true;

		[Description("自动封禁玩家名。")]
		public bool EnableBanOnUsernames;

		[Description("默认注册玩家所在组。")]
		public string DefaultRegistrationGroupName = "default";

		[Description("默认未注册玩家所在组。")]
		public string DefaultGuestGroupName = "guest";

		[Description("不对玩家显示日志。")]
		public bool DisableSpewLogs = true;

		[Description("预防OnSecondUpdate刷出大量日志。")]
		public bool DisableSecondUpdateLogs = false;

		[Description("密码加密算法 \"sha512\", \"sha256\", \"md5\"。")]
		public string HashAlgorithm = "sha512";

        [Obsolete("PacketBuffered is no longer used")]

        [Description("缓冲包。")]
		public bool BufferPackets = true;

		[Description("服务器满时提示信息。")]
		public string ServerFullReason = "服务器已满。";

		[Description("不在白名单而被踢时提示信息。")]
		public string WhitelistKickReason = "你不在白名单里。";

		[Description("服务器爆满时提示信息。")]
		public string ServerFullNoReservedReason = "服务器爆满。";

		[Description("崩溃时保存地图。")]
		public bool SaveWorldOnCrash = true;

		[Description("显示玩家IP国家")]
		public bool EnableGeoIP;

		[Description("远程端口要求强制认证。")]
		public bool EnableTokenEndpointAuthentication;

		[Description("远程控制。")]
		public bool RestApiEnabled;

		[Description("远程控制端口。")]
		public int RestApiPort = 7878;

		[Description("禁止墓碑。")]
		public bool DisableTombstones = true;

		[Description("对管理员显示玩家IP。")]
		public bool DisplayIPToAdmins;

		[Description("自动踢出代理玩家。")]
		public bool KickProxyUsers = true;

		[Description("锁定肉山前。")]
		public bool DisableHardmode;

		[Description("禁止打地牢骷髅。")]
		public bool DisableDungeonGuardian;

		[Description("禁止小丑丢炸弹")]
		public bool DisableClownBombs;

		[Description("禁止雪人丢雪球")]
		public bool DisableSnowBalls;

		[Description(
			"聊天格式 {0}组名 {1}前缀 {2}玩家名 {3}后缀 {4}说的话"
			)]
		public string ChatFormat = "{1}{2}{3}: {4}";

		[Description("头顶显示聊天文字。格式同上。")]
		public string ChatAboveHeadsFormat = "{2}";

		[Description("锁定时间。normal正常 day白天 night晚上。")]
		public string ForceTime = "normal";

		[Description("每秒方块破坏上限")]
		public int TileKillThreshold = 60;

		[Description("每秒方块放置上限")]
		public int TilePlaceThreshold = 20;

		[Description("每秒液体放置上限")]
		public int TileLiquidThreshold = 15;

		[Description("每秒弹幕发射上限")]
		public int ProjectileThreshold = 50;

		[Description("忽略弹幕碎片")]
		public bool ProjIgnoreShrapnel = true;

		[Description("强制登录。")]
		public bool RequireLogin;

		[Description(
			"禁止隐身PVP"
			)]
		public bool DisableInvisPvP;

		[Description("石化后可移动距离。")]
		public int MaxRangeForDisabled = 10;

		[Description("服务器密码")]
		public string ServerPassword = "";

		[Description("保护领地里的箱子")]
		public bool RegionProtectChests;

		[Description("禁止进服前登录。")]
		public bool DisableLoginBeforeJoin;

		[Description("强制密码登录。")]
		public bool DisableUUIDLogin;

		[Description("踢出异常客户端。")]
		public bool KickEmptyUUID;

		[Description("允许自由注册。")]
		public bool AllowRegisterAnyUsername;

		[Description("允许自由登录。")]
		public bool AllowLoginAnyUsername = true;

		[Description("攻击力上限。")]
		public int MaxDamage = 1175;

		[Description("弹幕攻击力上限。")]
		public int MaxProjDamage = 1175;

		[Description("踢出受伤超过攻击力上限的玩家")]
		public bool KickOnDamageThresholdBroken = false;

		[Description("忽略弹幕更新检查")]
		public bool IgnoreProjUpdate = false;

		[Description("忽略弹幕消灭检查")]
		public bool IgnoreProjKill = false;

		[Description("忽略所有no clip检查（什么鬼）")]
		public bool IgnoreNoClip = false;

		[Description("地图被保护时允许玩家使用寒冰魔杖")]
		public bool AllowIce = false;

		[Description("允许血腥传播")]
		public bool AllowCrimsonCreep = true;

		[Description("允许腐化传播")]
		public bool AllowCorruptionCreep = true;

		[Description("允许神圣传播")]
		public bool AllowHallowCreep = true;

		[Description("雕像200像素刷物品上限")]
		public int StatueSpawn200 = 3;

		[Description("雕像600像素刷物品上限")]
		public int StatueSpawn600 = 6;

		[Description("雕像全图刷物品上限")]
		public int StatueSpawnWorld = 10;

		[Description("预防命令刷出被封禁物品。")]
		public bool PreventBannedItemSpawn = false;

		[Description("玩家死后不能修改地图。")]
		public bool PreventDeadModification = true;

		[Description("头顶显示聊天文字。")]
		public bool EnableChatAboveHeads = false;

		[Description("开启圣诞")]
		public bool ForceXmas = false;

		[Description("允许特定组用命令刷出被封禁物品。")]
		public bool AllowAllowedGroupsToSpawnBannedItems = false;

		[Description("忽略箱子中的作弊物品。")]
		public bool IgnoreChestStacksOnLoad = false;

		[Description("日志保存文件夹。")]
		public string LogPath = "tshock";

		[Description("保存SQL记录至文件。")]
		public bool UseSqlLogs = false;

		[Description("SQL记录前失败次数")] 
		public int RevertToTextLogsOnSqlFailures = 10;

		[Description("预防非法放置方式")]
		public bool PreventInvalidPlaceStyle = true;

		[Description("服务器广播颜色。")]
		public int[] BroadcastRGB = { 127, 255, 212 };

		// TODO: Get rid of this when the old REST permission model is removed.
		[Description(
			"远程使用新的权限"
			)]
		public bool RestUseNewPermissionModel = true;

		[Description("A dictionary of REST tokens that external applications may use to make queries to your server.")]
		public Dictionary<string, SecureRest.TokenData> ApplicationRestTokens = new Dictionary<string, SecureRest.TokenData>();

		[Description("预留空位数。")]
		public int ReservedSlots = 20;

		[Description("预留空位数。")]
		public bool LogRest = false;

		[Description("复活时间。")]
		public int RespawnSeconds = 5;

		[Description("Boss战复活时间。")]
		public int RespawnBossSeconds = 10;

		[Description("每秒涂漆上限")]
		public int TilePaintThreshold = 15;

		[Description("开启最大缓冲。")]
		public bool EnableMaxBytesInBuffer = false;

		[Description("缓冲大小。")]
		public int MaxBytesInBuffer = 5242880;

		[Description("强制万圣。")]
		public bool ForceHalloween = false;

		[Description("允许任何人破坏草和罐子。")]
		public bool AllowCutTilesAndBreakables = false;

		[Description("命令标志")]
		public string CommandSpecifier = "/";

		[Description("静默命令标志")]
		public string CommandSilentSpecifier = ".";
		
		[Description("困难难度死亡踢出")]
		public bool KickOnHardcoreDeath;
		
		[Description("困难难度死亡封禁")]
		public bool BanOnHardcoreDeath;
		
		[Description("困难难度死亡封禁")]
		public string HardcoreBanReason = "死亡而被封禁";
		
		[Description("困难难度死亡踢出")]
		public string HardcoreKickReason = "死亡而被踢出";

		[Description("匿名召唤Boss或入侵")]
		public bool AnonymousBossInvasions = true;

		[Description("最大血量")]
		public int MaxHP = 500;

		[Description("最大魔法")]
		public int MaxMP = 200;

		[Description("玩家退出时保存地图")]
		public bool SaveWorldOnLastPlayerExit = true;

		[Description("BCrypt加密工作因子")]
		public int BCryptWorkFactor = 7;

		[Description("最小密码长度")]
		public int MinimumPasswordLength = 4;

		[Description("远程请求间隔")]
		public int RESTMaximumRequestsPerInterval = 5;

		[Description("远程请求减少速度")]
		public int RESTRequestBucketDecreaseIntervalMinutes = 1;

		[Description("远程请求只限制错误密码")]
		public bool RESTLimitOnlyFailedLoginRequests = true;

		[Obsolete("This is being removed in future versions of TShock due to Terraria fixes.")]
		[Description("允许使用挖掘机，非常危险，可能被毁服。")] public bool
			VeryDangerousDoNotChangeEnableDrillContainmentUnit = true;

		[Description("显示自动保存信息")] public bool ShowBackupAutosaveMessages = true;

		/// <summary>
		/// Reads a configuration file from a given path
		/// </summary>
		/// <param name="path">string path</param>
		/// <returns>ConfigFile object</returns>
		public static ConfigFile Read(string path)
		{
			if (!File.Exists(path))
				{DumpDescriptions();{DumpDescriptions();{DumpDescriptions();{DumpDescriptions();{DumpDescriptions();return new ConfigFile();}}}}}
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Read(fs);
			}
		}

		/// <summary>
		/// Reads the configuration file from a stream
		/// </summary>
		/// <param name="stream">stream</param>
		/// <returns>ConfigFile object</returns>
		public static ConfigFile Read(Stream stream)
		{
			using (var sr = new StreamReader(stream))
			{
				var cf = JsonConvert.DeserializeObject<ConfigFile>(sr.ReadToEnd());
				if (ConfigRead != null)
					ConfigRead(cf);
				return cf;
			}
		}

		/// <summary>
		/// Writes the configuration to a given path
		/// </summary>
		/// <param name="path">string path - Location to put the config file</param>
		public void Write(string path)
		{
			using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
			{
				Write(fs);
			}
		}

		/// <summary>
		/// Writes the configuration to a stream
		/// </summary>
		/// <param name="stream">stream</param>
		public void Write(Stream stream)
		{
			var str = JsonConvert.SerializeObject(this, Formatting.Indented);
			using (var sw = new StreamWriter(stream))
			{
				sw.Write(str);
			}
		}

		/// <summary>
		/// On config read hook
		/// </summary>
		public static Action<ConfigFile> ConfigRead;


        /// <summary>
        /// Dumps all configuration options to a text file in Markdown format
        /// </summary>
        public static void DumpDescriptions()
		{
			var sb = new StringBuilder();
			var defaults = new ConfigFile();

			foreach (var field in defaults.GetType().GetFields().OrderBy(f => f.Name))
			{
				if (field.IsStatic)
					continue;

				var name = field.Name;
				var type = field.FieldType.Name;

				var descattr =
					field.GetCustomAttributes(false).FirstOrDefault(o => o is DescriptionAttribute) as DescriptionAttribute;
				var desc = descattr != null && !string.IsNullOrWhiteSpace(descattr.Description) ? descattr.Description : "None";

				var def = field.GetValue(defaults);

				sb.AppendLine("{0}  ".SFormat(name));
				sb.AppendLine("类型: {0}  ".SFormat(type));
				sb.AppendLine("描述: {0}  ".SFormat(desc));
				sb.AppendLine("默认值: \"{0}\"  ".SFormat(def));
				sb.AppendLine();
			}

			File.WriteAllText("Config介绍.txt", sb.ToString());
		}
	}
}