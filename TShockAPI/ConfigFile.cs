﻿/*
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
		/// <summary>InvasionMultiplier - The equation for calculating invasion size = 100 + (multiplier * (number of active players > 200 hp)).</summary>
		[Description(
			"The equation for calculating invasion size is 100 + (multiplier * (number of active players with greater than 200 health))."
			)]
		public int InvasionMultiplier = 1;

		/// <summary>DefaultMaximumSpawns - The default max spawns per wave.</summary>
		[Description("The default maximum mobs that will spawn per wave. Higher means more mobs in that wave.")]
		public int DefaultMaximumSpawns = 5;

		/// <summary>DefaultSpawnRate - The default spawn rate.</summary>
		[Description("The delay between waves. Lower values lead to more mobs.")]
		public int DefaultSpawnRate = 600;

		/// <summary>ServerPort - The configured server port.</summary>
		[Description("The port the server runs on.")]
		public int ServerPort = 7777;

		/// <summary>EnableWhitelist - boolean if the whitelist functionality should be turned on.</summary>
		[Description("Enable or disable the whitelist based on IP addresses in whitelist.txt")]
		public bool EnableWhitelist;

		/// <summary>InfiniteInvasion - Whether or not infinite invasion mode should be on.</summary>
		[Description(
			"Enable the ability for invasion size to never decrease. Make sure to run /invade, and note that this adds 2 million+ goblins to the spawn queue for the map."
			)]
		public bool InfiniteInvasion;

		/// <summary>PvPMode - The server PvP mode (normal, always, or disabled).</summary>
		[Description("Set the server pvp mode. Valid types are, \"normal\", \"always\", and \"disabled.\"")]
		public string PvPMode = "normal";

		/// <summary>SpawnProtection - Enables the spawn protection system.</summary>
		[Description("Prevents tiles from being placed within SpawnProtectionRadius of the default spawn.")]
		public bool SpawnProtection = true;

		/// <summary>SpawnProtectionRadius - The spawn protection tile radius.</summary>
		[Description("Radius from spawn tile for SpawnProtection.")]
		public int SpawnProtectionRadius = 10;

		/// <summary>MaxSlots - The server's max slots.</summary>
		[Description(
			"Max slots for the server. If you want people to be kicked with \"Server is full\" set this to how many players you want max and then set Terraria max players to 2 higher."
			)]
		public int MaxSlots = 8;

		/// <summary>RangeChecks - Whether or not the anti-grief system based on range should be enabled.</summary>
		[Description("Global protection agent for any block distance based anti-grief check.")]
		public bool RangeChecks = true;

		/// <summary>DisableBuild - Whether or not building should be enabled.</summary>
		[Description("Disables any building; placing of blocks")]
		public bool DisableBuild;

		/// <summary>SuperAdminChatRGB - The chat color for the superadmin group.</summary>
		[Description("#.#.#. = Red/Blue/Green - RGB Colors for the Admin Chat Color. Max value: 255")]
		public int[] SuperAdminChatRGB = { 255, 0, 0 };

		/// <summary>SuperAdminChatPrefix - The superadmin chat prefix.</summary>
		[Description("Super admin group chat prefix")]
		public string SuperAdminChatPrefix = "(Admin) ";

		/// <summary>SuperAdminChatSuffix - The superadmin chat suffix.</summary>
		[Description("Super admin group chat suffix")]
		public string SuperAdminChatSuffix = "";

		/// <summary>BackupInterval - The backup frequency in minutes.</summary>
		[Description(
			"Backup frequency in minutes. So, a value of 60 = 60 minutes. Backups are stored in the \\tshock\\backups folder.")]
		public int BackupInterval;

		/// <summary>BackupKeepFor - Backup max age in minutes.</summary>
		[Description("How long backups are kept in minutes. 2880 = 2 days.")]
		public int BackupKeepFor = 60;

		/// <summary>RememberLeavePos - Whether or not to remember where an IP player was when they left.</summary>
		[Description(
			"Remembers where a player left off. It works by remembering the IP, NOT the character.  \neg. When you try to disconnect, and reconnect to be automatically placed at spawn, you'll be at your last location. Note: Won't save after server restarts."
			)]
		public bool RememberLeavePos;

		/// <summary>HardcoreOnly - Whether or not HardcoreOnly should be enabled.</summary>
		[Description("Hardcore players ONLY. This means softcore players cannot join.")]
		public bool HardcoreOnly;

		/// <summary>MediumcoreOnly - Whether or not MediumCore only players should be enabled.</summary>
		[Description("Mediumcore players ONLY. This means softcore players cannot join.")]
		public bool MediumcoreOnly;

		/// <summary>KickOnMediumcoreDeath - Whether or not to kick mediumcore players on death.</summary>
		[Description("Kicks a mediumcore player on death.")]
		public bool KickOnMediumcoreDeath;

		/// <summary>BanOnMediumcoreDeath - Whether or not to ban mediumcore players on death.</summary>
		[Description("Bans a mediumcore player on death.")]
		public bool BanOnMediumcoreDeath;

		[Description("Enable/disable Terraria's built in auto save.")]
		public bool AutoSave = true;
		[Description("Enable/disable save announcements.")]
		public bool AnnounceSave = true;

		[Description("Number of failed login attempts before kicking the player.")]
		public int MaximumLoginAttempts = 3;

		[Description("Used when replying to a rest /status request or sent to the client when UseServerName is true.")]
		public string ServerName = "";
		[Description("Sends ServerName in place of the world name to clients.")]
		public bool UseServerName = false;
		[Description("Not implemented.")]
		public string MasterServer = "127.0.0.1";

		[Description("Valid types are \"sqlite\" and \"mysql\"")]
		public string StorageType = "sqlite";

		[Description("The MySQL hostname and port to direct connections to")]
		public string MySqlHost = "localhost:3306";
		[Description("Database name to connect to")]
		public string MySqlDbName = "";
		[Description("Database username to connect with")]
		public string MySqlUsername = "";
		[Description("Database password to connect with")]
		public string MySqlPassword = "";

		[Description("Bans a mediumcore player on death.")]
		public string MediumcoreBanReason = "Death results in a ban";
		[Description("Kicks a mediumcore player on death.")]
		public string MediumcoreKickReason = "Death results in a kick";

		[Description("Enables DNS resolution of incoming connections with GetGroupForIPExpensive.")]
		public bool EnableDNSHostResolution;

		[Description("Enables kicking of banned users by matching their IP Address.")]
		public bool EnableIPBans = true;

		[Description("Enables kicking of banned users by matching their client UUID.")]
		public bool EnableUUIDBans = true;

		[Description("Enables kicking of banned users by matching their Character Name.")]
		public bool EnableBanOnUsernames;

		[Description("Selects the default group name to place new registrants under.")]
		public string DefaultRegistrationGroupName = "default";

		[Description("Selects the default group name to place non registered users under")]
		public string DefaultGuestGroupName = "guest";

		[Description("Force-disable printing logs to players with the log permission.")]
		public bool DisableSpewLogs = true;

		[Description("Prevents OnSecondUpdate checks from writing to the log file")]
		public bool DisableSecondUpdateLogs = false;

		[Description("Valid types are \"sha512\", \"sha256\", \"md5\", append with \"-xp\" for the xp supported algorithms.")]
		public string HashAlgorithm = "sha512";

		[Obsolete("PacketBuffered is no longer used")]
		[Description("Buffers up the packets and sends them out at the end of each frame.")]
		public bool BufferPackets = true;

		[Description("String that is used when kicking people when the server is full.")]
		public string ServerFullReason = "Server is full";

		[Description("String that is used when a user is kicked due to not being on the whitelist.")]
		public string WhitelistKickReason = "You are not on the whitelist.";

		[Description("String that is used when kicking people when the server is full with no reserved slots.")]
		public string ServerFullNoReservedReason = "Server is full. No reserved slots open.";

		[Description("This will save the world if Terraria crashes from an unhandled exception.")]
		public bool SaveWorldOnCrash = true;

		[Description("This will announce a player's location on join")]
		public bool EnableGeoIP;

		[Description("This will turn on token requirement for the public REST API endpoints.")]
		public bool EnableTokenEndpointAuthentication;

		[Description("Enable/disable the rest api.")]
		public bool RestApiEnabled;

		[Description("This is the port which the rest api will listen on.")]
		public int RestApiPort = 7878;

		[Description("Disable tombstones for all players.")]
		public bool DisableTombstones = true;

		[Description("Displays a player's IP on join to everyone who has the log permission.")]
		public bool DisplayIPToAdmins;

		[Description("Kicks users using a proxy as identified with the GeoIP database.")]
		public bool KickProxyUsers = true;

		[Description("Disables hardmode, can't never be activated. Overrides /starthardmode.")]
		public bool DisableHardmode;

		[Description("Disables the dungeon guardian from being spawned by player packets, this will instead force a respawn.")]
		public bool DisableDungeonGuardian;

		[Description("Disables clown bomb projectiles from spawning.")]
		public bool DisableClownBombs;

		[Description("Disables snow ball projectiles from spawning.")]
		public bool DisableSnowBalls;

		[Description(
			"Changes ingame chat format: {0} = Group Name, {1} = Group Prefix, {2} = Player Name, {3} = Group Suffix, {4} = Chat Message"
			)]
		public string ChatFormat = "{1}{2}{3}: {4}";

		[Description("Change the player name when using chat above heads. This begins with a player name wrapped in brackets, as per Terraria's formatting. Same formatting as ChatFormat(minus the text aka {4}).")]
		public string ChatAboveHeadsFormat = "{2}";

		[Description("Force the world time to be normal, day, or night.")]
		public string ForceTime = "normal";

		[Description("Disables/reverts a player if this number of tile kills is exceeded within 1 second.")]
		public int TileKillThreshold = 60;

		[Description("Disables/reverts a player if this number of tile places is exceeded within 1 second.")]
		public int TilePlaceThreshold = 20;

		[Description("Disables a player if this number of liquid sets is exceeded within 1 second.")]
		public int TileLiquidThreshold = 15;

		[Description("Disable a player if this number of projectiles is created within 1 second.")]
		public int ProjectileThreshold = 50;

		[Description("Ignore shrapnel from crystal bullets for projectile threshold.")]
		public bool ProjIgnoreShrapnel = true;

		[Description("Requires all players to register or login before being allowed to play.")]
		public bool RequireLogin;

		[Description(
			"Disables invisibility potions from being used in PvP (Note, can be used in the client, but the effect isn't sent to the rest of the server)."
			)]
		public bool DisableInvisPvP;

		[Description("The maximum distance players disabled for various reasons can move from.")]
		public int MaxRangeForDisabled = 10;

		[Description("Server password required to join the server.")]
		public string ServerPassword = "";

		[Description("Protect chests with region and build permissions.")]
		public bool RegionProtectChests;

		[Description("Disable users from being able to login with account password when joining.")]
		public bool DisableLoginBeforeJoin;

		[Description("Disable users from being able to login with their client UUID.")]
		public bool DisableUUIDLogin;

		[Description("Kick clients that don't send a UUID to the server.")]
		public bool KickEmptyUUID;

		[Description("Allows users to register any username with /register.")]
		public bool AllowRegisterAnyUsername;

		[Description("Allows users to login with any username with /login.")]
		public bool AllowLoginAnyUsername = true;

		[Description("The maximum damage a player/npc can inflict.")]
		public int MaxDamage = 1175;

		[Description("The maximum damage a projectile can inflict.")]
		public int MaxProjDamage = 1175;

		[Description("Kicks a user if set to true, if they inflict more damage then the max damage.")]
		public bool KickOnDamageThresholdBroken = false;

		[Description("Ignores checking to see if player 'can' update a projectile.")]
		public bool IgnoreProjUpdate = false;

		[Description("Ignores checking to see if player 'can' kill a projectile.")]
		public bool IgnoreProjKill = false;

		[Description("Ignores all no clip checks for players.")]
		public bool IgnoreNoClip = false;

		[Description("Allow ice placement even when user does not have canbuild.")]
		public bool AllowIce = false;

		[Description("Allows crimson to spread when a world is hardmode.")]
		public bool AllowCrimsonCreep = true;

		[Description("Allows corruption to spread when a world is hardmode.")]
		public bool AllowCorruptionCreep = true;

		[Description("Allows hallow to spread when a world is hardmode.")]
		public bool AllowHallowCreep = true;

		[Description("How many things a statue can spawn within 200 pixels(?) before it stops spawning. Default = 3")]
		public int StatueSpawn200 = 3;

		[Description("How many things a statue can spawn within 600 pixels(?) before it stops spawning. Default = 6")]
		public int StatueSpawn600 = 6;

		[Description("How many things a statue spawns can exist in the world before it stops spawning. Default = 10")]
		public int StatueSpawnWorld = 10;

		[Description("Prevent banned items from being /i or /give.")]
		public bool PreventBannedItemSpawn = false;

		[Description("Prevent players from interacting with the world if dead.")]
		public bool PreventDeadModification = true;

		[Description("Displays chat messages above players' heads, but will disable chat prefixes to compensate.")]
		public bool EnableChatAboveHeads = false;

		[Description("Force Christmas-only events to occur all year.")]
		public bool ForceXmas = false;

		[Description("Allows groups on the banned item allowed list to spawn banned items.")]
		public bool AllowAllowedGroupsToSpawnBannedItems = false;

		[Description("Allows stacks in chests to be beyond the stack limit")]
		public bool IgnoreChestStacksOnLoad = false;

		[Description("The path of the directory where logs should be written into.")]
		public string LogPath = "tshock";

		[Description("Save logs to an SQL database instead of a text file. Default = false")]
		public bool UseSqlLogs = false;

		[Description("Number of times the SQL log must fail to insert logs before falling back to the text log")] 
		public int RevertToTextLogsOnSqlFailures = 10;

		[Description("Prevents players from placing tiles with an invalid style.")]
		public bool PreventInvalidPlaceStyle = true;

		[Description("#.#.#. = Red/Blue/Green - RGB Colors for broadcasts. Max value: 255.")]
		public int[] BroadcastRGB = { 127, 255, 212 };

		// TODO: Get rid of this when the old REST permission model is removed.
		[Description(
			"Whether the REST API should use the new permission model. Note: The old permission model will become depracted in the future."
			)]
		public bool RestUseNewPermissionModel = true;

		[Description("A dictionary of REST tokens that external applications may use to make queries to your server.")]
		public Dictionary<string, SecureRest.TokenData> ApplicationRestTokens = new Dictionary<string, SecureRest.TokenData>();

		[Description("The number of reserved slots past your max server slot that can be joined by reserved players")]
		public int ReservedSlots = 20;

		[Description("The number of reserved slots past your max server slot that can be joined by reserved players")]
		public bool LogRest = false;

		[Description("The number of seconds a player must wait before being respawned.")]
		public int RespawnSeconds = 5;

		[Description("The number of seconds a player must wait before being respawned if there is a boss nearby.")]
		public int RespawnBossSeconds = 10;

		[Description("Disables a player if this number of tiles is painted within 1 second.")]
		public int TilePaintThreshold = 15;

		[Description("Enables max packet bufferer size.")]
		public bool EnableMaxBytesInBuffer = false;

		[Description("Number of bytes in the packet buffer before we disconnect the player.")]
		public int MaxBytesInBuffer = 5242880;

		[Description("Forces your world to be in Halloween mode regardless of the data.")]
		public bool ForceHalloween = false;

		[Description("Allows anyone to break grass, pots, etc.")]
		public bool AllowCutTilesAndBreakables = false;

		[Description("Specifies which string starts a command.")]
		public string CommandSpecifier = "/";

		[Description("Specifies which string starts a command silently.")]
		public string CommandSilentSpecifier = ".";
		
		[Description("Kicks a hardcore player on death.")]
		public bool KickOnHardcoreDeath;
		
		[Description("Bans a hardcore player on death.")]
		public bool BanOnHardcoreDeath;
		
		[Description("Bans a hardcore player on death.")]
		public string HardcoreBanReason = "Death results in a ban";
		
		[Description("Kicks a hardcore player on death.")]
		public string HardcoreKickReason = "Death results in a kick";

		[Description("Whether bosses or invasions should be anonymously spawned.")]
		public bool AnonymousBossInvasions = true;

		[Description("The maximum allowable HP, before equipment buffs.")]
		public int MaxHP = 500;

		[Description("The maximum allowable MP, before equipment buffs.")]
		public int MaxMP = 200;

		[Description("Determines if the server should save the world if the last player exits.")]
		public bool SaveWorldOnLastPlayerExit = true;

		[Description("Determines the BCrypt work factor to use. If increased, all passwords will be upgraded to new work-factor on verify. The number of computational rounds is 2^n. Increase with caution. Range: 5-31.")]
		public int BCryptWorkFactor = 7;

		[Description("The minimum password length for new user accounts. Minimum value is 4.")]
		public int MinimumPasswordLength = 4;

		[Description("The maximum REST requests in the bucket before denying requests. Minimum value is 5.")]
		public int RESTMaximumRequestsPerInterval = 5;

		[Description("How often in minutes the REST requests bucket is decreased by one. Minimum value is 1 minute.")]
		public int RESTRequestBucketDecreaseIntervalMinutes = 1;

		[Description("Whether we should limit only the max failed login requests, or all login requests")]
		public bool RESTLimitOnlyFailedLoginRequests = true;

		[Description("Show backup autosave messages.")] public bool ShowBackupAutosaveMessages = true;

		/// <summary>
		/// Reads a configuration file from a given path
		/// </summary>
		/// <param name="path">string path</param>
		/// <returns>ConfigFile object</returns>
		public static ConfigFile Read(string path)
		{
			if (!File.Exists(path))
				return new ConfigFile();
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
				sb.AppendLine("Type: {0}  ".SFormat(type));
				sb.AppendLine("Description: {0}  ".SFormat(desc));
				sb.AppendLine("Default: \"{0}\"  ".SFormat(def));
				sb.AppendLine();
			}

			File.WriteAllText("ConfigDescriptions.txt", sb.ToString());
		}
	}
}