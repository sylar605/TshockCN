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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Terraria;
using Terraria.ID;
using TShockAPI.DB;
using TerrariaApi.Server;
using TShockAPI.Hooks;

namespace TShockAPI
{
	public delegate void CommandDelegate(CommandArgs args);

	public class CommandArgs : EventArgs
	{
		public string Message { get; private set; }
		public TSPlayer Player { get; private set; }
		public bool Silent { get; private set; }

		/// <summary>
		/// Parameters passed to the arguement. Does not include the command name.
		/// IE '/kick "jerk face"' will only have 1 argument
		/// </summary>
		public List<string> Parameters { get; private set; }

		public Player TPlayer
		{
			get { return Player.TPlayer; }
		}

		public CommandArgs(string message, TSPlayer ply, List<string> args)
		{
			Message = message;
			Player = ply;
			Parameters = args;
			Silent = false;
		}

		public CommandArgs(string message, bool silent, TSPlayer ply, List<string> args)
		{
			Message = message;
			Player = ply;
			Parameters = args;
			Silent = silent;
		}
	}

	public class Command
	{
		/// <summary>
		/// Gets or sets whether to allow non-players to use this command.
		/// </summary>
		public bool AllowServer { get; set; }
		/// <summary>
		/// Gets or sets whether to do logging of this command.
		/// </summary>
		public bool DoLog { get; set; }
		/// <summary>
		/// Gets or sets the help text of this command.
		/// </summary>
		public string HelpText { get; set; }
        /// <summary>
        /// Gets or sets an extended description of this command.
        /// </summary>
        public string[] HelpDesc { get; set; }
		/// <summary>
		/// Gets the name of the command.
		/// </summary>
		public string Name { get { return Names[0]; } }
		/// <summary>
		/// Gets the names of the command.
		/// </summary>
		public List<string> Names { get; protected set; }
		/// <summary>
		/// Gets the permissions of the command.
		/// </summary>
		public List<string> Permissions { get; protected set; }

		private CommandDelegate commandDelegate;
		public CommandDelegate CommandDelegate
		{
			get { return commandDelegate; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				commandDelegate = value;
			}
	 	}

		public Command(List<string> permissions, CommandDelegate cmd, params string[] names)
			: this(cmd, names)
		{
			Permissions = permissions;
		}

		public Command(string permissions, CommandDelegate cmd, params string[] names)
			: this(cmd, names)
		{
			Permissions = new List<string> { permissions };
		}

		public Command(CommandDelegate cmd, params string[] names)
		{
			if (cmd == null)
				throw new ArgumentNullException("cmd");
			if (names == null || names.Length < 1)
				throw new ArgumentException("names");

			AllowServer = true;
			CommandDelegate = cmd;
			DoLog = true;
<<<<<<< HEAD
			HelpText = "No help available.";
=======
			HelpText = "No help available。";
>>>>>>> e2683b6... 手动加入RYH修改的文件
      HelpDesc = null;
			Names = new List<string>(names);
			Permissions = new List<string>();
		}

		public bool Run(string msg, bool silent, TSPlayer ply, List<string> parms)
		{
			if (!CanRun(ply))
				return false;

			try
			{
				CommandDelegate(new CommandArgs(msg, silent, ply, parms));
			}
			catch (Exception e)
			{
<<<<<<< HEAD
				ply.SendErrorMessage("Command failed, check logs for more details.");
=======
				ply.SendErrorMessage("命令使用失败。检查日志获取更多细节。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				TShock.Log.Error(e.ToString());
			}

			return true;
		}

		public bool Run(string msg, TSPlayer ply, List<string> parms)
		{
			return Run(msg, false, ply, parms);
		}

		public bool HasAlias(string name)
		{
			return Names.Contains(name);
		}

		public bool CanRun(TSPlayer ply)
		{
			if (Permissions == null || Permissions.Count < 1)
				return true;
			foreach (var Permission in Permissions)
			{
<<<<<<< HEAD
				if (ply.HasPermission(Permission))
=======
				if (ply.Group.HasPermission(Permission))
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return true;
			}
			return false;
		}
	}

	public static class Commands
	{
		public static List<Command> ChatCommands = new List<Command>();
		public static ReadOnlyCollection<Command> TShockCommands = new ReadOnlyCollection<Command>(new List<Command>());

		public static string Specifier
		{
			get { return string.IsNullOrWhiteSpace(TShock.Config.CommandSpecifier) ? "/" : TShock.Config.CommandSpecifier; }
		}

		public static string SilentSpecifier
		{
			get { return string.IsNullOrWhiteSpace(TShock.Config.CommandSilentSpecifier) ? "." : TShock.Config.CommandSilentSpecifier; }
		}

		private delegate void AddChatCommand(string permission, CommandDelegate command, params string[] names);

		public static void InitCommands()
		{
			List<Command> tshockCommands = new List<Command>(100);
			Action<Command> add = (cmd) => 
			{
				tshockCommands.Add(cmd);
				ChatCommands.Add(cmd);
			};

<<<<<<< HEAD
			add(new Command(AuthToken, "auth")
			{
				AllowServer = false,
				HelpText = "Used to authenticate as superadmin when first setting up TShock."
			});
			add(new Command(Permissions.authverify, AuthVerify, "auth-verify")
			{
				HelpText = "Used to verify that you have correctly set up TShock."
			});
			add(new Command(Permissions.user, ManageUsers, "user")
			{
				DoLog = false,
				HelpText = "Manages user accounts."
			});

			#region Account Commands
			add(new Command(Permissions.canlogin, AttemptLogin, "login")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = "Logs you into an account."
			});
			add(new Command(Permissions.canlogout, Logout, "logout")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = "Logs you out of your current account."
			});
			add(new Command(Permissions.canchangepassword, PasswordUser, "password")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = "Changes your account's password."
			});
			add(new Command(Permissions.canregister, RegisterUser, "register")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = "Registers you an account."
			});
			#endregion
			#region Admin Commands
			add(new Command(Permissions.ban, Ban, "ban")
			{
				HelpText = "Manages player bans."
			});
			add(new Command(Permissions.broadcast, Broadcast, "broadcast", "bc", "say")
			{
				HelpText = "Broadcasts a message to everyone on the server."
			});
			add(new Command(Permissions.logs, DisplayLogs, "displaylogs")
			{
				HelpText = "Toggles whether you receive server logs."
			});
			add(new Command(Permissions.managegroup, Group, "group")
			{
				HelpText = "Manages groups."
			});
			add(new Command(Permissions.manageitem, ItemBan, "itemban")
			{
				HelpText = "Manages item bans."
			});
            add(new Command(Permissions.manageprojectile, ProjectileBan, "projban")
            {
                HelpText = "Manages projectile bans."
            });
			add(new Command(Permissions.managetile, TileBan, "tileban")
			{
				HelpText = "Manages tile bans."
			});
			add(new Command(Permissions.manageregion, Region, "region")
			{
				HelpText = "Manages regions."
			});
			add(new Command(Permissions.kick, Kick, "kick")
			{
				HelpText = "Removes a player from the server."
			});
			add(new Command(Permissions.mute, Mute, "mute", "unmute")
			{
				HelpText = "Prevents a player from talking."
			});
			add(new Command(Permissions.savessc, OverrideSSC, "overridessc", "ossc")
			{
				HelpText = "Overrides serverside characters for a player, temporarily."
			});
			add(new Command(Permissions.savessc, SaveSSC, "savessc")
			{
				HelpText = "Saves all serverside characters."
			});
			add(new Command(Permissions.settempgroup, TempGroup, "tempgroup")
			{
				HelpText = "Temporarily sets another player's group."
			});
			add(new Command(Permissions.userinfo, GrabUserUserInfo, "userinfo", "ui")
			{
				HelpText = "Shows information about a user."
			});
			#endregion
			#region Annoy Commands
			add(new Command(Permissions.annoy, Annoy, "annoy")
			{
				HelpText = "Annoys a player for an amount of time."
			});
			add(new Command(Permissions.annoy, Confuse, "confuse")
			{
				HelpText = "Confuses a player for an amount of time."
			});
			add(new Command(Permissions.annoy, Rocket, "rocket")
			{
				HelpText = "Rockets a player upwards. Requires SSC."
			});
			add(new Command(Permissions.annoy, FireWork, "firework")
			{
				HelpText = "Spawns fireworks at a player."
			});
			#endregion
			#region Configuration Commands
			add(new Command(Permissions.maintenance, CheckUpdates, "checkupdates")
			{
				HelpText = "Checks for TShock updates."
			});
			add(new Command(Permissions.maintenance, Off, "off", "exit")
			{
				HelpText = "Shuts down the server while saving."
			});
			add(new Command(Permissions.maintenance, OffNoSave, "off-nosave", "exit-nosave")
			{
				HelpText = "Shuts down the server without saving."
			});
			add(new Command(Permissions.cfgreload, Reload, "reload")
			{
				HelpText = "Reloads the server configuration file."
			});
			add(new Command(Permissions.maintenance, Restart, "restart")
			{
				HelpText = "Restarts the server."
			});
			add(new Command(Permissions.cfgpassword, ServerPassword, "serverpassword")
			{
				HelpText = "Changes the server password."
			});
			add(new Command(Permissions.maintenance, GetVersion, "version")
			{
				HelpText = "Shows the TShock version."
			});
			/* Does nothing atm.
			 * 
			 * add(new Command(Permissions.updateplugins, UpdatePlugins, "updateplugins")
			{
			});*/
			add(new Command(Permissions.whitelist, Whitelist, "whitelist")
			{
				HelpText = "Manages the server whitelist."
			});
			#endregion
			#region Item Commands
			add(new Command(Permissions.item, Give, "give", "g")
			{
				HelpText = "Gives another player an item."
			});
			add(new Command(Permissions.item, Item, "item", "i")
			{
				AllowServer = false,
				HelpText = "Gives yourself an item."
			});
			#endregion
			#region NPC Commands
			add(new Command(Permissions.butcher, Butcher, "butcher")
			{
				HelpText = "Kills hostile NPCs or NPCs of a certain type."
			});
			add(new Command(Permissions.renamenpc, RenameNPC, "renamenpc")
			{
				HelpText = "Renames an NPC."
			});
			add(new Command(Permissions.invade, Invade, "invade")
			{
				HelpText = "Starts an NPC invasion."
			});
			add(new Command(Permissions.maxspawns, MaxSpawns, "maxspawns")
			{
				HelpText = "Sets the maximum number of NPCs."
			});
			add(new Command(Permissions.spawnboss, SpawnBoss, "spawnboss", "sb")
			{
				AllowServer = false,
				HelpText = "Spawns a number of bosses around you."
			});
			add(new Command(Permissions.spawnmob, SpawnMob, "spawnmob", "sm")
			{
				AllowServer = false,
				HelpText = "Spawns a number of mobs around you."
			});
			add(new Command(Permissions.spawnrate, SpawnRate, "spawnrate")
			{
				HelpText = "Sets the spawn rate of NPCs."
			});
			add(new Command(Permissions.clearangler, ClearAnglerQuests, "clearangler")
			{
				HelpText = "Resets the list of users who have completed an angler quest that day."
			});
			#endregion
			#region TP Commands
			add(new Command(Permissions.home, Home, "home")
			{
				AllowServer = false,
				HelpText = "Sends you to your spawn point."
			});
			add(new Command(Permissions.spawn, Spawn, "spawn")
			{
				AllowServer = false,
				HelpText = "Sends you to the world's spawn point."
			});
			add(new Command(Permissions.tp, TP, "tp")
			{
				AllowServer = false,
				HelpText = "Teleports a player to another player."
			});
			add(new Command(Permissions.tpothers, TPHere, "tphere")
			{
				AllowServer = false,
				HelpText = "Teleports a player to yourself."
			});
			add(new Command(Permissions.tpnpc, TPNpc, "tpnpc")
			{
				AllowServer = false,
				HelpText = "Teleports you to an npc."
			});
			add(new Command(Permissions.tppos, TPPos, "tppos")
			{
				AllowServer = false,
				HelpText = "Teleports you to tile coordinates."
			});
			add(new Command(Permissions.getpos, GetPos, "pos")
			{
				AllowServer = false,
				HelpText = "Returns the user's or specified user's current position."
			});
			add(new Command(Permissions.tpallow, TPAllow, "tpallow")
			{
				AllowServer = false,
				HelpText = "Toggles whether other people can teleport you."
			});
			#endregion
			#region World Commands
			add(new Command(Permissions.toggleexpert, ToggleExpert, "expert", "expertmode")
			{
					HelpText = "Toggles expert mode."
			});
			add(new Command(Permissions.antibuild, ToggleAntiBuild, "antibuild")
			{
				HelpText = "Toggles build protection."
			});
			add(new Command(Permissions.bloodmoon, Bloodmoon, "bloodmoon")
			{
				HelpText = "Sets a blood moon."
			});
			add(new Command(Permissions.grow, Grow, "grow")
			{
				AllowServer = false,
				HelpText = "Grows plants at your location."
			});
			add(new Command(Permissions.dropmeteor, DropMeteor, "dropmeteor")
			{
				HelpText = "Drops a meteor somewhere in the world."
			});
			add(new Command(Permissions.eclipse, Eclipse, "eclipse")
			{
				HelpText = "Sets an eclipse."
			});
			add(new Command(Permissions.halloween, ForceHalloween, "forcehalloween")
			{
				HelpText = "Toggles halloween mode (goodie bags, pumpkins, etc)."
			});
			add(new Command(Permissions.xmas, ForceXmas, "forcexmas")
			{
				HelpText = "Toggles christmas mode (present spawning, santa, etc)."
			});
			add(new Command(Permissions.fullmoon, Fullmoon, "fullmoon")
			{
				HelpText = "Sets a full moon."
			});
			add(new Command(Permissions.hardmode, Hardmode, "hardmode")
			{
				HelpText = "Toggles the world's hardmode status."
			});
			add(new Command(Permissions.editspawn, ProtectSpawn, "protectspawn")
			{
				HelpText = "Toggles spawn protection."
			});
			add(new Command(Permissions.rain, Rain, "rain")
			{
				HelpText = "Toggles the rain."
			});
			add(new Command(Permissions.worldsave, Save, "save")
			{
				HelpText = "Saves the world file."
			});
			add(new Command(Permissions.worldspawn, SetSpawn, "setspawn")
			{
				AllowServer = false,
				HelpText = "Sets the world's spawn point to your location."
			});
			add(new Command(Permissions.worldsettle, Settle, "settle")
			{
				HelpText = "Forces all liquids to update immediately."
			});
			add(new Command(Permissions.time, Time, "time")
			{
				HelpText = "Sets the world time."
			});
			add(new Command(Permissions.wind, Wind, "wind")
			{
				HelpText = "Changes the wind speed."
			});
			add(new Command(Permissions.worldinfo, WorldInfo, "world")
			{
				HelpText = "Shows information about the current world."
			});
			#endregion
			#region Other Commands
			add(new Command(Permissions.buff, Buff, "buff")
			{
				AllowServer = false,
				HelpText = "Gives yourself a buff for an amount of time."
			});
			add(new Command(Permissions.clear, Clear, "clear")
			{
				HelpText = "Clears item drops or projectiles."
			});
			add(new Command(Permissions.buffplayer, GBuff, "gbuff", "buffplayer")
			{
				HelpText = "Gives another player a buff for an amount of time."
			});
			add(new Command(Permissions.godmode, ToggleGodMode, "godmode")
			{
				HelpText = "Toggles godmode on a player."
			});
			add(new Command(Permissions.heal, Heal, "heal")
			{
				HelpText = "Heals a player in HP and MP."
			});
			add(new Command(Permissions.kill, Kill, "kill")
			{
				HelpText = "Kills another player."
			});
			add(new Command(Permissions.cantalkinthird, ThirdPerson, "me")
			{
				HelpText = "Sends an action message to everyone."
			});
			add(new Command(Permissions.canpartychat, PartyChat, "party", "p")
			{
				AllowServer = false,
				HelpText = "Sends a message to everyone on your team."
			});
			add(new Command(Permissions.whisper, Reply, "reply", "r")
			{
				HelpText = "Replies to a PM sent to you."
			});
			add(new Command(Rests.RestPermissions.restmanage, ManageRest, "rest")
			{
				HelpText = "Manages the REST API."
			});
			add(new Command(Permissions.slap, Slap, "slap")
			{
				HelpText = "Slaps a player, dealing damage."
			});
			add(new Command(Permissions.serverinfo, ServerInfo, "serverinfo")
			{
				HelpText = "Shows the server information."
			});
			add(new Command(Permissions.warp, Warp, "warp")
			{
				HelpText = "Teleports you to a warp point or manages warps."
			});
			add(new Command(Permissions.whisper, Whisper, "whisper", "w", "tell")
			{
				HelpText = "Sends a PM to a player."
			});
			#endregion

			add(new Command(Aliases, "aliases")
			{
				HelpText = "Shows a command's aliases."
			});
			add(new Command(Help, "help")
			{
				HelpText = "Lists commands or gives help on them."
			});
			add(new Command(Motd, "motd")
			{
				HelpText = "Shows the message of the day."
			});
			add(new Command(ListConnectedPlayers, "playing", "online", "who")
			{
				HelpText = "Shows the currently connected players."
			});
			add(new Command(Rules, "rules")
			{
				HelpText = "Shows the server's rules."
=======
			add(new Command(AuthToken, "认证", "auth")
			{
				AllowServer = false,
				HelpText = "Used to authenticate as superadmin when first setting up TShock。"
			});
			add(new Command(Permissions.authverify, AuthVerify, "关闭认证", "auth-verify")
			{
				HelpText = "Used to verify that you have correctly set up TShock。"
			});
			add(new Command(Permissions.user, ManageUsers, "用户", "user")
			{
				DoLog = false,
				HelpText = "Manages user accounts。"
			});

			#region Account Commands
			add(new Command(Permissions.canlogin, AttemptLogin, "登入", "登录", "登陆", "login")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = "Logs you into an account。"
			});
			add(new Command(Permissions.canlogout, Logout, "登出", "logout")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = "Logs you out of your current account。"
			});
			add(new Command(Permissions.canchangepassword, PasswordUser, "改密码", "password")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = "Changes your account's password。"
			});
			add(new Command(Permissions.canregister, RegisterUser, "注册", "register")
			{
				AllowServer = false,
				DoLog = false,
				HelpText = "Registers you an account。"
			});
			#endregion
			#region Admin Commands
			add(new Command(Permissions.ban, Ban, "封", "ban")
			{
				HelpText = "Manages player封禁。"
			});
			add(new Command(Permissions.broadcast, Broadcast, "说", "broadcast", "bc", "say")
			{
				HelpText = "Broadcasts a message to everyone on the server。"
			});
			add(new Command(Permissions.logs, DisplayLogs, "显示日志", "displaylogs")
			{
				HelpText = "Toggles whether you receive server logs。"
			});
			add(new Command(Permissions.managegroup, Group, "组", "group")
			{
				HelpText = "Manages groups。"
			});
			add(new Command(Permissions.manageitem, ItemBan, "封物品", "itemban")
			{
				HelpText = "Manages item封禁。"
			});
            add(new Command(Permissions.manageprojectile, ProjectileBan, "封弹幕", "projban")
            {
                HelpText = "Manages projectile封禁。"
            });
			add(new Command(Permissions.managetile, TileBan, "封方块", "tileban")
			{
				HelpText = "Manages tile封禁。"
			});
			add(new Command(Permissions.manageregion, Region, "领地", "region")
			{
				HelpText = "Manages regions。"
			});
			add(new Command(Permissions.kick, Kick, "踢", "kick")
			{
				HelpText = "Removes a player from the server。"
			});
			add(new Command(Permissions.mute, Mute, "禁言", "mute", "unmute")
			{
				HelpText = "Prevents a player from talking。"
			});
			add(new Command(Permissions.savessc, OverrideSSC, "免开荒", "overridessc", "ossc")
			{
				HelpText = "Overrides serverside characters for a player, temporarily。"
			});
			add(new Command(Permissions.savessc, SaveSSC, "存档", "savessc")
			{
				HelpText = "Saves all serverside characters。"
			});
			add(new Command(Permissions.settempgroup, TempGroup, "临时组", "tempgroup")
			{
				HelpText = "Temporarily sets another player's group。"
			});
			add(new Command(Permissions.userinfo, GrabUserUserInfo, "用户信息", "userinfo", "ui")
			{
				HelpText = "Shows information about a user。"
			});
			#endregion
			#region Annoy Commands
			add(new Command(Permissions.annoy, Annoy, "骚扰", "annoy")
			{
				HelpText = "Annoys a player for an amount of time。"
			});
			add(new Command(Permissions.annoy, Confuse, "混乱", "confuse")
			{
				HelpText = "Confuses a player for an amount of time。"
			});
			add(new Command(Permissions.annoy, Rocket, "火箭", "rocket")
			{
				HelpText = "Rockets a player upwards. Requires SSC。"
			});
			add(new Command(Permissions.annoy, FireWork, "烟花", "firework")
			{
				HelpText = "Spawns fireworks at a player。"
			});
			#endregion
			#region Configuration Commands
			add(new Command(Permissions.maintenance, CheckUpdates, "检查更新", "checkupdates")
			{
				HelpText = "Checks for TShock updates。"
			});
			add(new Command(Permissions.maintenance, Off, "关服", "off", "exit")
			{
				HelpText = "Shuts down the server while saving。"
			});
			add(new Command(Permissions.maintenance, OffNoSave, "不保存关服", "off-nosave", "exit-nosave")
			{
				HelpText = "Shuts down the server without saving。"
			});
			add(new Command(Permissions.cfgreload, Reload, "重载", "reload")
			{
				HelpText = "Reloads the server configuration file。"
			});
			add(new Command(Permissions.maintenance, Restart, "重启", "restart")
			{
				HelpText = "Restarts the server。"
			});
			add(new Command(Permissions.cfgpassword, ServerPassword, "服务器密码", "serverpassword")
			{
				HelpText = "Changes the server password。"
			});
			add(new Command(Permissions.maintenance, GetVersion, "版本", "version")
			{
				HelpText = "Shows the TShock version。"
			});
			/* Does nothing atm.
			 * 
			 * add(new Command(Permissions.updateplugins, UpdatePlugins, "升级插件", "updateplugins")
			{
			});*/
			add(new Command(Permissions.whitelist, Whitelist, "白名单", "whitelist")
			{
				HelpText = "Manages the server whitelist。"
			});
			#endregion
			#region Item Commands
			add(new Command(Permissions.item, Give, "给", "give", "g")
			{
				HelpText = "Gives another player an item。"
			});
			add(new Command(Permissions.item, Item, "刷", "item", "i")
			{
				AllowServer = false,
				HelpText = "Gives yourself an item。"
			});
			#endregion
			#region NPC Commands
			add(new Command(Permissions.butcher, Butcher, "杀", "butcher")
			{
				HelpText = "Kills hostile NPCs or NPCs of a certain type。"
			});
			add(new Command(Permissions.renamenpc, RenameNPC, "重命名NPC", "renamenpc")
			{
				HelpText = "Renames an NPC。"
			});
			add(new Command(Permissions.invade, Invade, "入侵", "invade")
			{
				HelpText = "Starts an NPC invasion。"
			});
			add(new Command(Permissions.maxspawns, MaxSpawns, "最大刷怪", "maxspawns")
			{
				HelpText = "Sets the maximum number of NPCs。"
			});
			add(new Command(Permissions.spawnboss, SpawnBoss, "刷Boss", "spawnboss", "sb")
			{
				AllowServer = false,
				HelpText = "Spawns a number of bosses around you。"
			});
			add(new Command(Permissions.spawnmob, SpawnMob, "刷怪", "spawnmob", "sm")
			{
				AllowServer = false,
				HelpText = "Spawns a number of mobs around you。"
			});
			add(new Command(Permissions.spawnrate, SpawnRate, "刷怪率", "spawnrate")
			{
				HelpText = "Sets the spawn rate of NPCs。"
			});
			add(new Command(Permissions.clearangler, ClearAnglerQuests, "清渔夫任务", "clearangler")
			{
				HelpText = "Resets the list of users who have completed an angler quest that day。"
			});
			#endregion
			#region TP Commands
			add(new Command(Permissions.home, Home, "回城", "home")
			{
				AllowServer = false,
				HelpText = "Sends you to your spawn point。"
			});
			add(new Command(Permissions.spawn, Spawn, "回家", "spawn")
			{
				AllowServer = false,
				HelpText = "Sends you to the world's spawn point。"
			});
			add(new Command(Permissions.tp, TP, "传送", "tp")
			{
				AllowServer = false,
				HelpText = "Teleports a player to another player。"
			});
			add(new Command(Permissions.tpothers, TPHere, "拉人", "tphere")
			{
				AllowServer = false,
				HelpText = "Teleports a player to yourself。"
			});
			add(new Command(Permissions.tpnpc, TPNpc, "去NPC", "tpnpc")
			{
				AllowServer = false,
				HelpText = "Teleports you to an npc。"
			});
			add(new Command(Permissions.tppos, TPPos, "去坐标", "tppos")
			{
				AllowServer = false,
				HelpText = "Teleports you to tile coordinates。"
			});
			add(new Command(Permissions.getpos, GetPos, "查坐标", "pos")
			{
				AllowServer = false,
				HelpText = "Returns the user's or specified user's current position。"
			});
			add(new Command(Permissions.tpallow, TPAllow, "传送保护", "tpallow")
			{
				AllowServer = false,
				HelpText = "Toggles whether other people can teleport you。"
			});
			#endregion
			#region World Commands
			add(new Command(Permissions.toggleexpert, ToggleExpert, "专家", "expert", "expertmode")
			{
					HelpText = "Toggles expert mode。"
			});
			add(new Command(Permissions.antibuild, ToggleAntiBuild, "全图保护", "antibuild")
			{
				HelpText = "Toggles build protection。"
			});
			add(new Command(Permissions.bloodmoon, Bloodmoon, "血月", "bloodmoon")
			{
				HelpText = "Sets a blood moon。"
			});
			add(new Command(Permissions.grow, Grow, "种", "grow")
			{
				AllowServer = false,
				HelpText = "Grows plants at your location。"
			});
			add(new Command(Permissions.dropmeteor, DropMeteor, "陨石", "dropmeteor")
			{
				HelpText = "Drops a meteor somewhere in the world。"
			});
			add(new Command(Permissions.eclipse, Eclipse, "日食", "eclipse")
			{
				HelpText = "Sets an eclipse。"
			});
			add(new Command(Permissions.halloween, ForceHalloween, "万圣", "forcehalloween")
			{
				HelpText = "Toggles halloween mode (goodie bags, pumpkins, etc)。"
			});
			add(new Command(Permissions.xmas, ForceXmas, "圣诞", "forcexmas")
			{
				HelpText = "Toggles christmas mode (present spawning, santa, etc)。"
			});
			add(new Command(Permissions.fullmoon, Fullmoon, "满月", "fullmoon")
			{
				HelpText = "Sets a full moon。"
			});
			add(new Command(Permissions.hardmode, Hardmode, "肉山", "hardmode")
			{
				HelpText = "Toggles the world's hardmode status。"
			});
			add(new Command(Permissions.editspawn, ProtectSpawn, "复活点保护", "protectspawn")
			{
				HelpText = "Toggles spawn protection。"
			});
			add(new Command(Permissions.rain, Rain, "下雨", "rain")
			{
				HelpText = "Toggles the rain。"
			});
			add(new Command(Permissions.worldsave, Save, "保存", "save")
			{
				HelpText = "Saves the world file。"
			});
			add(new Command(Permissions.worldspawn, SetSpawn, "设出生点", "setspawn")
			{
				AllowServer = false,
				HelpText = "Sets the world's spawn point to your location。"
			});
			add(new Command(Permissions.worldsettle, Settle, "平衡液体", "settle")
			{
				HelpText = "Forces all liquids to update immediately。"
			});
			add(new Command(Permissions.time, Time, "时间", "time")
			{
				HelpText = "Sets the world time。"
			});
			add(new Command(Permissions.wind, Wind, "风", "wind")
			{
				HelpText = "Changes the wind speed。"
			});
			add(new Command(Permissions.worldinfo, WorldInfo, "地图", "world")
			{
				HelpText = "Shows information about the current world。"
			});
			#endregion
			#region Other Commands
			add(new Command(Permissions.buff, Buff, "状态", "buff")
			{
				AllowServer = false,
				HelpText = "Gives yourself a buff for an amount of time。"
			});
			add(new Command(Permissions.clear, Clear, "清理", "clear")
			{
				HelpText = "Clears item drops or projectiles。"
			});
			add(new Command(Permissions.buffplayer, GBuff, "给状态", "gbuff", "buffplayer")
			{
				HelpText = "Gives another player a buff for an amount of time。"
			});
			add(new Command(Permissions.godmode, ToggleGodMode, "上帝", "godmode")
			{
				HelpText = "Toggles godmode on a player。"
			});
			add(new Command(Permissions.heal, Heal, "回血", "heal")
			{
				HelpText = "Heals a player in HP and MP。"
			});
			add(new Command(Permissions.kill, Kill, "秒杀", "kill")
			{
				HelpText = "Kills another player。"
			});
			add(new Command(Permissions.cantalkinthird, ThirdPerson, "卖萌", "me")
			{
				HelpText = "Sends an action message to everyone。"
			});
			add(new Command(Permissions.canpartychat, PartyChat, "队伍", "party", "p")
			{
				AllowServer = false,
				HelpText = "Sends a message to everyone on your team。"
			});
			add(new Command(Permissions.whisper, Reply, "回", "reply", "r")
			{
				HelpText = "Replies to a PM sent to you。"
			});
			add(new Command(Rests.RestPermissions.restmanage, ManageRest, "远程", "rest")
			{
				HelpText = "Manages the REST API。"
			});
			add(new Command(Permissions.slap, Slap, "扇人", "slap")
			{
				HelpText = "Slaps a player, dealing damage。"
			});
			add(new Command(Permissions.serverinfo, ServerInfo, "服务器信息", "serverinfo")
			{
				HelpText = "Shows the server information。"
			});
			add(new Command(Permissions.warp, Warp, "跃迁", "warp")
			{
				HelpText = "Teleports you to a warp point or manages warps。"
			});
			add(new Command(Permissions.whisper, Whisper, "私聊", "whisper", "w", "tell")
			{
				HelpText = "Sends a PM to a player。"
			});
			#endregion

			add(new Command(Aliases, "同义", "aliases")
			{
				HelpText = "Shows a command's aliases。"
			});
			add(new Command(Help, "帮助", "help")
			{
				HelpText = "Lists commands or gives help on them。"
			});
			add(new Command(Motd, "公告", "motd")
			{
				HelpText = "Shows the message of the day。"
			});
			add(new Command(ListConnectedPlayers, "在线", "playing", "online", "who")
			{
				HelpText = "Shows the currently connected players。"
			});
			add(new Command(Rules, "规则", "rules")
			{
				HelpText = "Shows the server's rules。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
			});

			TShockCommands = new ReadOnlyCollection<Command>(tshockCommands);
		}

		public static bool HandleCommand(TSPlayer player, string text)
		{
			string cmdText = text.Remove(0, 1);
			string cmdPrefix = text[0].ToString();
			bool silent = false;

			if (cmdPrefix == SilentSpecifier)
				silent = true;

			var args = ParseParameters(cmdText);
			if (args.Count < 1)
				return false;

			string cmdName = args[0].ToLower();
			args.RemoveAt(0);

			IEnumerable<Command> cmds = ChatCommands.FindAll(c => c.HasAlias(cmdName));

			if (Hooks.PlayerHooks.OnPlayerCommand(player, cmdName, cmdText, args, ref cmds, cmdPrefix))
				return true;

			if (cmds.Count() == 0)
			{
				if (player.AwaitingResponse.ContainsKey(cmdName))
				{
					Action<CommandArgs> call = player.AwaitingResponse[cmdName];
					player.AwaitingResponse.Remove(cmdName);
					call(new CommandArgs(cmdText, player, args));
					return true;
				}
<<<<<<< HEAD
				player.SendErrorMessage("Invalid command entered. Type {0}help for a list of valid commands.", Specifier);
=======
				player.SendErrorMessage("没有这个命令。输入{0}帮助 查看可用命令列表。", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return true;
			}
			foreach (Command cmd in cmds)
			{
				if (!cmd.CanRun(player))
				{
<<<<<<< HEAD
					TShock.Utils.SendLogs(string.Format("{0} tried to execute {1}{2}.", player.Name, Specifier, cmdText), Color.PaleVioletRed, player);
					player.SendErrorMessage("You do not have access to this command.");
				}
				else if (!cmd.AllowServer && !player.RealPlayer)
				{
					player.SendErrorMessage("You must use this command in-game.");
=======
					TShock.Utils.SendLogs(string.Format("{0} 试图执行 {1}{2}。", player.Name, Specifier, cmdText), Color.PaleVioletRed, player);
					player.SendErrorMessage("你没有权限使用这个命令。");
				}
				else if (!cmd.AllowServer && !player.RealPlayer)
				{
					player.SendErrorMessage("你必须在游戏中使用这个命令。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
				else
				{
					if (cmd.DoLog)
<<<<<<< HEAD
						TShock.Utils.SendLogs(string.Format("{0} executed: {1}{2}.", player.Name, silent ? SilentSpecifier : Specifier, cmdText), Color.PaleVioletRed, player);
=======
						TShock.Utils.SendLogs(string.Format("{0} 执行了 {1}{2}。", player.Name, silent ? SilentSpecifier : Specifier, cmdText), Color.PaleVioletRed, player);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					cmd.Run(cmdText, silent, player, args);
				}
			}
			return true;
		}

		/// <summary>
		/// Parses a string of parameters into a list. Handles quotes.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static List<String> ParseParameters(string str)
		{
			var ret = new List<string>();
			var sb = new StringBuilder();
			bool instr = false;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];

				if (c == '\\' && ++i < str.Length)
				{
					if (str[i] != '"' && str[i] != ' ' && str[i] != '\\')
						sb.Append('\\');
					sb.Append(str[i]);
				}
				else if (c == '"')
				{
					instr = !instr;
					if (!instr)
					{
						ret.Add(sb.ToString());
						sb.Clear();
					}
					else if (sb.Length > 0)
					{
						ret.Add(sb.ToString());
						sb.Clear();
					}
				}
				else if (IsWhiteSpace(c) && !instr)
				{
					if (sb.Length > 0)
					{
						ret.Add(sb.ToString());
						sb.Clear();
					}
				}
				else
					sb.Append(c);
			}
			if (sb.Length > 0)
				ret.Add(sb.ToString());

			return ret;
		}

		private static bool IsWhiteSpace(char c)
		{
			return c == ' ' || c == '\t' || c == '\n';
		}

		#region Account commands

		private static void AttemptLogin(CommandArgs args)
		{
			if (args.Player.LoginAttempts > TShock.Config.MaximumLoginAttempts && (TShock.Config.MaximumLoginAttempts != -1))
			{
<<<<<<< HEAD
				TShock.Log.Warn(String.Format("{0} ({1}) had {2} or more invalid login attempts and was kicked automatically.",
					args.Player.IP, args.Player.Name, TShock.Config.MaximumLoginAttempts));
				TShock.Utils.Kick(args.Player, "Too many invalid login attempts.");
=======
				TShock.Log.Warn(String.Format("{0} ({1}) 因多次输错密码而被踢。",
					args.Player.IP, args.Player.Name, TShock.Config.MaximumLoginAttempts));
				TShock.Utils.Kick(args.Player, "输错密码多次");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			if (args.Player.IsLoggedIn)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("You are already logged in, and cannot login again.");
=======
				args.Player.SendErrorMessage("你已登录，不能重复登录。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
            
			User user = TShock.Users.GetUserByName(args.Player.Name);
			string password = "";
			bool usingUUID = false;
			if (args.Parameters.Count == 0 && !TShock.Config.DisableUUIDLogin)
			{
				if (PlayerHooks.OnPlayerPreLogin(args.Player, args.Player.Name, ""))
					return;
				usingUUID = true;
			}
			else if (args.Parameters.Count == 1)
			{
				if (PlayerHooks.OnPlayerPreLogin(args.Player, args.Player.Name, args.Parameters[0]))
					return;
				password = args.Parameters[0];
			}
			else if (args.Parameters.Count == 2 && TShock.Config.AllowLoginAnyUsername)
			{
				if (String.IsNullOrEmpty(args.Parameters[0]))
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Bad login attempt.");
=======
					args.Player.SendErrorMessage("Bad login attempt。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}

				if (PlayerHooks.OnPlayerPreLogin(args.Player, args.Parameters[0], args.Parameters[1]))
					return;

				user = TShock.Users.GetUserByName(args.Parameters[0]);
				password = args.Parameters[1];
			}
			else
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Syntax: {0}login - Logs in using your UUID and character name", Specifier);
				args.Player.SendErrorMessage("        {0}login <password> - Logs in using your password and character name", Specifier);
				args.Player.SendErrorMessage("        {0}login <username> <password> - Logs in using your username and password", Specifier);
				args.Player.SendErrorMessage("If you forgot your password, there is no way to recover it.");
=======
				args.Player.SendErrorMessage("帮助: {0}登入 - 使用玩家名和UUID登入。", Specifier);
				args.Player.SendErrorMessage("      {0}登入 <密码> - 使用玩家名和密码登入", Specifier);
				args.Player.SendErrorMessage("      {0}登入 <用户名> <密码> - 使用用户名和密码登入", Specifier);
				args.Player.SendErrorMessage("请妥善保管密码，密码无法找回。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			try
			{
				if (user == null)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("A user by that name does not exist.");
=======
					args.Player.SendErrorMessage("用户不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
				else if (user.VerifyPassword(password) ||
						(usingUUID && user.UUID == args.Player.UUID && !TShock.Config.DisableUUIDLogin &&
						!String.IsNullOrWhiteSpace(args.Player.UUID)))
				{
					args.Player.PlayerData = TShock.CharacterDB.GetPlayerData(args.Player, user.ID);

					var group = TShock.Utils.GetGroup(user.Group);

					if (Main.ServerSideCharacter)
					{
<<<<<<< HEAD
						if (args.Player.HasPermission(Permissions.bypassssc))
=======
						if (group.HasPermission(Permissions.bypassssc))
>>>>>>> e2683b6... 手动加入RYH修改的文件
						{
							args.Player.IgnoreActionsForClearingTrashCan = false;
						}
						args.Player.PlayerData.RestoreCharacter(args.Player);
					}
					args.Player.LoginFailsBySsi = false;

<<<<<<< HEAD
					if (args.Player.HasPermission(Permissions.ignorestackhackdetection))
						args.Player.IgnoreActionsForCheating = "none";

					if (args.Player.HasPermission(Permissions.usebanneditem))
=======
					if (group.HasPermission(Permissions.ignorestackhackdetection))
						args.Player.IgnoreActionsForCheating = "none";

					if (group.HasPermission(Permissions.usebanneditem))
>>>>>>> e2683b6... 手动加入RYH修改的文件
						args.Player.IgnoreActionsForDisabledArmor = "none";

					args.Player.Group = group;
					args.Player.tempGroup = null;
					args.Player.User = user;
					args.Player.IsLoggedIn = true;
					args.Player.IgnoreActionsForInventory = "none";

					if (!args.Player.IgnoreActionsForClearingTrashCan && Main.ServerSideCharacter)
					{
						args.Player.PlayerData.CopyCharacter(args.Player);
						TShock.CharacterDB.InsertPlayerData(args.Player);
					}
<<<<<<< HEAD
					args.Player.SendSuccessMessage("Authenticated as " + user.Name + " successfully.");

					TShock.Log.ConsoleInfo(args.Player.Name + " authenticated successfully as user: " + user.Name + ".");
=======
					args.Player.SendSuccessMessage("成功认证为" + user.Name + "。");

					TShock.Log.ConsoleInfo(args.Player.Name + "成功认证为" + user.Name + "。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					if ((args.Player.LoginHarassed) && (TShock.Config.RememberLeavePos))
					{
						if (TShock.RememberedPos.GetLeavePos(args.Player.Name, args.Player.IP) != Vector2.Zero)
						{
							Vector2 pos = TShock.RememberedPos.GetLeavePos(args.Player.Name, args.Player.IP);
							args.Player.Teleport((int) pos.X*16, (int) pos.Y*16);
						}
						args.Player.LoginHarassed = false;

					}
					TShock.Users.SetUserUUID(user, args.Player.UUID);

					Hooks.PlayerHooks.OnPlayerPostLogin(args.Player);
				}
				else
				{
					if (usingUUID && !TShock.Config.DisableUUIDLogin)
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("UUID does not match this character!");
					}
					else
					{
						args.Player.SendErrorMessage("Invalid password!");
					}
					TShock.Log.Warn(args.Player.IP + " failed to authenticate as user: " + user.Name + ".");
=======
						args.Player.SendErrorMessage("UUID不匹配。");
					}
					else
					{
						args.Player.SendErrorMessage("密码错误。");
					}
					TShock.Log.Warn(args.Player.IP + " 试图登入为 " + user.Name + "。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					args.Player.LoginAttempts++;
				}
			}
			catch (Exception ex)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("There was an error processing your request.");
=======
				args.Player.SendErrorMessage("发生未知错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				TShock.Log.Error(ex.ToString());
			}
		}

		private static void Logout(CommandArgs args)
		{
			if (!args.Player.IsLoggedIn)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("You are not logged in.");
=======
				args.Player.SendErrorMessage("未登入。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			PlayerHooks.OnPlayerLogout(args.Player);


			if (Main.ServerSideCharacter)
			{
<<<<<<< HEAD
				args.Player.IgnoreActionsForInventory = String.Format("Server side characters is enabled! Please {0}register or {0}login to play!", Commands.Specifier);
=======
				args.Player.IgnoreActionsForInventory = String.Format("本服务器强制开荒。请{0}注册 或 {0}登入 进行游戏。", Commands.Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				if (!args.Player.IgnoreActionsForClearingTrashCan && (!args.Player.Dead || args.Player.TPlayer.difficulty != 2))
				{
					args.Player.PlayerData.CopyCharacter(args.Player);
					TShock.CharacterDB.InsertPlayerData(args.Player);
				}
			}

			args.Player.PlayerData = new PlayerData(args.Player);
			args.Player.Group = TShock.Groups.GetGroupByName(TShock.Config.DefaultGuestGroupName);
			args.Player.tempGroup = null;
			if (args.Player.tempGroupTimer != null)
			{
				args.Player.tempGroupTimer.Stop();
			}
			args.Player.User = null;
			args.Player.IsLoggedIn = false;

<<<<<<< HEAD
			args.Player.SendSuccessMessage("You have been successfully logged out of your account.");
			if (Main.ServerSideCharacter)
			{
				args.Player.SendWarningMessage("Server side characters are enabled. You need to be logged in to play.");
=======
			args.Player.SendSuccessMessage("已登出。");
			if (Main.ServerSideCharacter)
			{
				args.Player.SendWarningMessage("本服务器强制开荒。请{0}登入 进行游戏。", Commands.Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void PasswordUser(CommandArgs args)
		{
			try
			{
				if (args.Player.IsLoggedIn && args.Parameters.Count == 2)
				{
					string password = args.Parameters[0];
					if (args.Player.User.VerifyPassword(password))
					{
						try
						{
<<<<<<< HEAD
							args.Player.SendSuccessMessage("You changed your password!");
							TShock.Users.SetUserPassword(args.Player.User, args.Parameters[1]); // SetUserPassword will hash it for you.
							TShock.Log.ConsoleInfo(args.Player.IP + " named " + args.Player.Name + " changed the password of account " +
							                       args.Player.User.Name + ".");
						}
						catch (ArgumentOutOfRangeException)
						{
							args.Player.SendErrorMessage("Password must be greater than or equal to " + TShock.Config.MinimumPasswordLength + " characters.");
=======
							args.Player.SendSuccessMessage("密码修改成功。");
							TShock.Users.SetUserPassword(args.Player.User, args.Parameters[1]); // SetUserPassword will hash it for you.
							TShock.Log.ConsoleInfo(args.Player.IP + " 玩家名 " + args.Player.Name + " 修改了 " +
							                       args.Player.User.Name + "的密码。");
						}
						catch (ArgumentOutOfRangeException)
						{
							args.Player.SendErrorMessage("密码至少要有" + TShock.Config.MinimumPasswordLength + "个字符。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
					else
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("You failed to change your password!");
						TShock.Log.ConsoleError(args.Player.IP + " named " + args.Player.Name + " failed to change password for account: " +
						                        args.Player.User.Name + ".");
=======
						args.Player.SendErrorMessage("修改密码失败。");
						TShock.Log.ConsoleError(args.Player.IP + " 玩家名 " + args.Player.Name + " 试图修改 " +
						                        args.Player.User.Name + "的密码。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
				}
				else
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Not logged in or invalid syntax! Proper syntax: {0}password <oldpassword> <newpassword>", Specifier);
=======
					args.Player.SendErrorMessage("未登录或格式错误。 格式: {0}改密码 <旧密码> <新密码>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			}
			catch (UserManagerException ex)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Sorry, an error occured: " + ex.Message + ".");
				TShock.Log.ConsoleError("PasswordUser returned an error: " + ex);
=======
				args.Player.SendErrorMessage("发生错误: " + ex.Message + "。");
				TShock.Log.ConsoleError("PasswordUser发生错误: " + ex);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void RegisterUser(CommandArgs args)
		{
			try
			{
				var user = new User();
				string echoPassword = "";
				if (args.Parameters.Count == 1)
				{
					user.Name = args.Player.Name;
					echoPassword = args.Parameters[0];
					try
					{
						user.CreateBCryptHash(args.Parameters[0]);
					}
					catch (ArgumentOutOfRangeException)
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("Password must be greater than or equal to " + TShock.Config.MinimumPasswordLength + " characters.");
=======
						args.Player.SendErrorMessage("密码至少要有" + TShock.Config.MinimumPasswordLength + "个字符。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						return;
					}
				}
				else if (args.Parameters.Count == 2 && TShock.Config.AllowRegisterAnyUsername)
				{
					user.Name = args.Parameters[0];
					echoPassword = args.Parameters[1];
					try
					{
						user.CreateBCryptHash(args.Parameters[1]);
					}
					catch (ArgumentOutOfRangeException)
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("Password must be greater than or equal to " + TShock.Config.MinimumPasswordLength + " characters.");
=======
						args.Player.SendErrorMessage("密码至少要有" + TShock.Config.MinimumPasswordLength + "个字符。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						return;
					}
				}
				else
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}register <password>", Specifier);
=======
					args.Player.SendErrorMessage("格式错误。 格式: {0}注册 <密码>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}

				user.Group = TShock.Config.DefaultRegistrationGroupName; // FIXME -- we should get this from the DB. --Why?
				user.UUID = args.Player.UUID;

                if (TShock.Users.GetUserByName(user.Name) == null && user.Name != TSServerPlayer.AccountName) // Cheap way of checking for existance of a user
				{
<<<<<<< HEAD
					args.Player.SendSuccessMessage("Account \"{0}\" has been registered.", user.Name);
					args.Player.SendSuccessMessage("Your password is {0}.", echoPassword);
					TShock.Users.AddUser(user);
					TShock.Log.ConsoleInfo("{0} registered an account: \"{1}\".", args.Player.Name, user.Name);
				}
				else
				{
					args.Player.SendErrorMessage("Account " + user.Name + " has already been registered.");
					TShock.Log.ConsoleInfo(args.Player.Name + " failed to register an existing account: " + user.Name);
=======
					args.Player.SendSuccessMessage("用户 \"{0}\" 已被注册。请直接登录或换玩家名进行游戏。", user.Name);
					args.Player.SendSuccessMessage("你的密码是{0}。", echoPassword);
					TShock.Users.AddUser(user);
					TShock.Log.ConsoleInfo("{0} 注册了 \"{1}\"。", args.Player.Name, user.Name);
				}
				else
				{
					args.Player.SendErrorMessage("用户 " + user.Name + " 已被注册。");
					TShock.Log.ConsoleInfo(args.Player.Name + " 试图注册 " + user.Name + " 失败。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			}
			catch (UserManagerException ex)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Sorry, an error occured: " + ex.Message + ".");
				TShock.Log.ConsoleError("RegisterUser returned an error: " + ex);
=======
				args.Player.SendErrorMessage("发生错误: " + ex.Message + "。");
				TShock.Log.ConsoleError("RegisterUser发生错误: " + ex);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void ManageUsers(CommandArgs args)
		{
			// This guy needs to be here so that people don't get exceptions when they type /user
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid user syntax. Try {0}user help.", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 输入'{0}用户 help' 查看帮助", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			string subcmd = args.Parameters[0];

			// Add requires a username, password, and a group specified.
			if (subcmd == "add"  && args.Parameters.Count == 4)
			{
				var user = new User();

				user.Name = args.Parameters[1];
				try
				{
					user.CreateBCryptHash(args.Parameters[2]);
				}
				catch (ArgumentOutOfRangeException)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Password must be greater than or equal to " + TShock.Config.MinimumPasswordLength + " characters.");
=======
					args.Player.SendErrorMessage("密码至少要有" + TShock.Config.MinimumPasswordLength + "个字符。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				user.Group = args.Parameters[3];
				
				try
				{
					TShock.Users.AddUser(user);
<<<<<<< HEAD
					args.Player.SendSuccessMessage("Account " + user.Name + " has been added to group " + user.Group + "!");
					TShock.Log.ConsoleInfo(args.Player.Name + " added Account " + user.Name + " to group " + user.Group);
				}
				catch (GroupNotExistsException)
				{
					args.Player.SendErrorMessage("Group " + user.Group + " does not exist!");
				}
				catch (UserExistsException)
				{
					args.Player.SendErrorMessage("User " + user.Name + " already exists!");
				}
				catch (UserManagerException e)
				{
					args.Player.SendErrorMessage("User " + user.Name + " could not be added, check console for details.");
=======
					args.Player.SendSuccessMessage("用户 " + user.Name + " 被添加到组 " + user.Group + "中。");
					TShock.Log.ConsoleInfo(args.Player.Name + " 添加用户 " + user.Name + " 到组 " + user.Group + "中。");
				}
				catch (GroupNotExistsException)
				{
					args.Player.SendErrorMessage("用户组 " + user.Group + " 不存在。");
				}
				catch (UserExistsException)
				{
					args.Player.SendErrorMessage("用户 " + user.Name + " 已经存在。");
				}
				catch (UserManagerException e)
				{
					args.Player.SendErrorMessage("无法添加用户 " + user.Name + " ，查看后台获得更多信息。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					TShock.Log.ConsoleError(e.ToString());
				}
			}
				// User deletion requires a username
			else if (subcmd == "del" && args.Parameters.Count == 2)
			{
				var user = new User();
				user.Name = args.Parameters[1];

				try
				{
					TShock.Users.RemoveUser(user);
<<<<<<< HEAD
					args.Player.SendSuccessMessage("Account removed successfully.");
					TShock.Log.ConsoleInfo(args.Player.Name + " successfully deleted account: " + args.Parameters[1] + ".");
				}
				catch (UserNotExistException)
				{
					args.Player.SendErrorMessage("The user " + user.Name + " does not exist! Deleted nobody!");
=======
					args.Player.SendSuccessMessage("用户删除成功。");
					TShock.Log.ConsoleInfo(args.Player.Name + " 删除了用户 " + args.Parameters[1] + "。");
				}
				catch (UserNotExistException)
				{
					args.Player.SendErrorMessage("用户 " + user.Name + " 不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
				catch (UserManagerException ex)
				{
					args.Player.SendErrorMessage(ex.Message);
					TShock.Log.ConsoleError(ex.ToString());
				}
			}
			
			// Password changing requires a username, and a new password to set
			else if (subcmd == "password" && args.Parameters.Count == 3)
			{
				var user = new User();
				user.Name = args.Parameters[1];

				try
				{
					TShock.Users.SetUserPassword(user, args.Parameters[2]);
<<<<<<< HEAD
					TShock.Log.ConsoleInfo(args.Player.Name + " changed the password of account " + user.Name);
					args.Player.SendSuccessMessage("Password change succeeded for " + user.Name + ".");
				}
				catch (UserNotExistException)
				{
					args.Player.SendErrorMessage("User " + user.Name + " does not exist!");
				}
				catch (UserManagerException e)
				{
					args.Player.SendErrorMessage("Password change for " + user.Name + " failed! Check console!");
=======
					TShock.Log.ConsoleInfo(args.Player.Name + " 修改了 " + user.Name + " 的密码。");
					args.Player.SendSuccessMessage("成功修改了 " + user.Name + " 的密码。");
				}
				catch (UserNotExistException)
				{
					args.Player.SendErrorMessage("用户 " + user.Name + " 不存在。");
				}
				catch (UserManagerException e)
				{
					args.Player.SendErrorMessage("无法修改用户 " + user.Name + " 的密码，查看后台获得更多信息。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					TShock.Log.ConsoleError(e.ToString());
				}
				catch (ArgumentOutOfRangeException)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Password must be greater than or equal to " + TShock.Config.MinimumPasswordLength + " characters.");
=======
					args.Player.SendErrorMessage("密码至少要有" + TShock.Config.MinimumPasswordLength + "个字符。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			}
			// Group changing requires a username or IP address, and a new group to set
			else if (subcmd == "group" && args.Parameters.Count == 3)
			{
				var user = new User();
				user.Name = args.Parameters[1];

				try
				{
					TShock.Users.SetUserGroup(user, args.Parameters[2]);
<<<<<<< HEAD
					TShock.Log.ConsoleInfo(args.Player.Name + " changed account " + user.Name + " to group " + args.Parameters[2] + ".");
					args.Player.SendSuccessMessage("Account " + user.Name + " has been changed to group " + args.Parameters[2] + "!");
				}
				catch (GroupNotExistsException)
				{
					args.Player.SendErrorMessage("That group does not exist!");
				}
				catch (UserNotExistException)
				{
					args.Player.SendErrorMessage("User " + user.Name + " does not exist!");
				}
				catch (UserManagerException e)
				{
					args.Player.SendErrorMessage("User " + user.Name + " could not be added. Check console for details.");
=======
					TShock.Log.ConsoleInfo(args.Player.Name + " 更改用户 " + user.Name + " 到组 " + args.Parameters[2] + "中。");
					args.Player.SendSuccessMessage("用户 " + user.Name + " 被更改到组 " + args.Parameters[2] + "中。");
				}
				catch (GroupNotExistsException)
				{
					args.Player.SendErrorMessage("用户组不存在。");
				}
				catch (UserNotExistException)
				{
					args.Player.SendErrorMessage("用户 " + user.Name + " 不存在。");
				}
				catch (UserManagerException e)
				{
					args.Player.SendErrorMessage("添加用户 " + user.Name + " 失败，查看后台获得更多信息。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					TShock.Log.ConsoleError(e.ToString());
				}
			}
			else if (subcmd == "help")
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("Use command help:");
				args.Player.SendInfoMessage("{0}user add username password group   -- Adds a specified user", Specifier);
				args.Player.SendInfoMessage("{0}user del username                  -- Removes a specified user", Specifier);
				args.Player.SendInfoMessage("{0}user password username newpassword -- Changes a user's password", Specifier);
				args.Player.SendInfoMessage("{0}user group username newgroup       -- Changes a user's group", Specifier);
			}
			else
			{
				args.Player.SendErrorMessage("Invalid user syntax. Try {0}user help.", Specifier);
=======
				args.Player.SendInfoMessage("命令帮助:");
				args.Player.SendInfoMessage("{0}用户 add 用户名 密码 用户组   -- 添加用户", Specifier);
				args.Player.SendInfoMessage("{0}用户 del 用户名                  -- 删除用户", Specifier);
				args.Player.SendInfoMessage("{0}用户 password 用户名 密码 -- 更改用户密码", Specifier);
				args.Player.SendInfoMessage("{0}用户 group 用户名 用户组       -- 更改用户组", Specifier);
			}
			else
			{
				args.Player.SendErrorMessage("格式错误。 输入'{0}用户 help' 查看帮助", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		#endregion

		#region Stupid commands

		private static void ServerInfo(CommandArgs args)
		{
<<<<<<< HEAD
			args.Player.SendInfoMessage("Memory usage: " + Process.GetCurrentProcess().WorkingSet64);
			args.Player.SendInfoMessage("Allocated memory: " + Process.GetCurrentProcess().VirtualMemorySize64);
			args.Player.SendInfoMessage("Total processor time: " + Process.GetCurrentProcess().TotalProcessorTime);
			args.Player.SendInfoMessage("WinVer: " + Environment.OSVersion);
			args.Player.SendInfoMessage("Proc count: " + Environment.ProcessorCount);
			args.Player.SendInfoMessage("Machine name: " + Environment.MachineName);
=======
			args.Player.SendInfoMessage("使用内存 " + Process.GetCurrentProcess().WorkingSet64);
			args.Player.SendInfoMessage("分配内存 " + Process.GetCurrentProcess().VirtualMemorySize64);
			args.Player.SendInfoMessage("处理器时间 " + Process.GetCurrentProcess().TotalProcessorTime);
			args.Player.SendInfoMessage("系统版本: " + Environment.OSVersion);
			args.Player.SendInfoMessage("处理器数量: " + Environment.ProcessorCount);
			args.Player.SendInfoMessage("机器名: " + Environment.MachineName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void WorldInfo(CommandArgs args)
		{
<<<<<<< HEAD
			args.Player.SendInfoMessage("World name: " + (TShock.Config.UseServerName ? TShock.Config.ServerName : Main.worldName));
			args.Player.SendInfoMessage("World size: {0}x{1}", Main.maxTilesX, Main.maxTilesY);
			args.Player.SendInfoMessage("World ID: " + Main.worldID);
=======
			args.Player.SendInfoMessage("地图名 " + (TShock.Config.UseServerName ? TShock.Config.ServerName : Main.worldName));
			args.Player.SendInfoMessage("地图大小: {0}x{1}", Main.maxTilesX, Main.maxTilesY);
			args.Player.SendInfoMessage("地图ID: " + Main.worldID);
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		#endregion

		#region Player Management Commands

		private static void GrabUserUserInfo(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}userinfo <player>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}用户信息 <玩家名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			var players = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (players.Count < 1)
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player.");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			else if (players.Count > 1)
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			else
			{
				var message = new StringBuilder();
<<<<<<< HEAD
				message.Append("IP Address: ").Append(players[0].IP);
				if (players[0].User != null && players[0].IsLoggedIn)
					message.Append(" | Logged in as: ").Append(players[0].User.Name).Append(" | Group: ").Append(players[0].Group.Name);
=======
				message.Append("IP地址: ").Append(players[0].IP);
				if (players[0].User != null && players[0].IsLoggedIn)
					message.Append(" | 登录为: ").Append(players[0].User.Name).Append(" | 用户组: ").Append(players[0].Group.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				args.Player.SendSuccessMessage(message.ToString());
			}
		}

		private static void Kick(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}kick <player> [reason]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}踢 <用户名> [原因]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			if (args.Parameters[0].Length == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Missing player name.");
=======
				args.Player.SendErrorMessage("请输入用户名。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			string plStr = args.Parameters[0];
			var players = TShock.Utils.FindPlayer(plStr);
			if (players.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (players.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			}
			else
			{
				string reason = args.Parameters.Count > 1
									? String.Join(" ", args.Parameters.GetRange(1, args.Parameters.Count - 1))
<<<<<<< HEAD
									: "Misbehaviour.";
				if (!TShock.Utils.Kick(players[0], reason, !args.Player.RealPlayer, false, args.Player.Name))
				{
					args.Player.SendErrorMessage("You can't kick another admin!");
=======
									: "行为不检。";
				if (!TShock.Utils.Kick(players[0], reason, !args.Player.RealPlayer, false, args.Player.Name))
				{
					args.Player.SendErrorMessage("你不能踢管理员。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			}
		}

		private static void Ban(CommandArgs args)
		{
			string subcmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();
			switch (subcmd)
			{
				case "add":
					#region Add ban
					{
						if (args.Parameters.Count < 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}ban add <player> [reason]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封 add <用户名> [原因]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						List<TSPlayer> players = TShock.Utils.FindPlayer(args.Parameters[1]);
<<<<<<< HEAD
						string reason = args.Parameters.Count > 2 ? String.Join(" ", args.Parameters.Skip(2)) : "Misbehavior.";
=======
						string reason = args.Parameters.Count > 2 ? String.Join(" ", args.Parameters.Skip(2)) : "行为不检。";
>>>>>>> e2683b6... 手动加入RYH修改的文件
						if (players.Count == 0)
						{
							var user = TShock.Users.GetUserByName(args.Parameters[1]);
							if (user != null)
							{
								bool force = !args.Player.RealPlayer;

								if (user.Name == args.Player.Name && !force)
								{
<<<<<<< HEAD
									args.Player.SendErrorMessage("You can't ban yourself!");
=======
									args.Player.SendErrorMessage("你不能封禁你自己。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
									return;
								}

								if (TShock.Groups.GetGroupByName(user.Group).HasPermission(Permissions.immunetoban) && !force)
<<<<<<< HEAD
									args.Player.SendErrorMessage("You can't ban {0}!", user.Name);
=======
									args.Player.SendErrorMessage("你不能封禁 {0}。", user.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								else
								{
									if (user.KnownIps == null)
									{
<<<<<<< HEAD
										args.Player.SendErrorMessage("Cannot ban {0} because they have no IPs to ban.", user.Name);
=======
										args.Player.SendErrorMessage("无法封禁 {0} ，因为找不到IP。", user.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
										return;
									}
									var knownIps = JsonConvert.DeserializeObject<List<string>>(user.KnownIps);
									TShock.Bans.AddBan(knownIps.Last(), user.Name, user.UUID, reason, false, args.Player.User.Name);
									if (String.IsNullOrWhiteSpace(args.Player.User.Name))
									{
										if (args.Silent)
										{
<<<<<<< HEAD
											args.Player.SendInfoMessage("{0} was {1}banned for '{2}'.", user.Name, force ? "Force " : "", reason);
										}
										else
										{
											TSPlayer.All.SendInfoMessage("{0} was {1}banned for '{2}'.", user.Name, force ? "Force " : "", reason);
=======
											args.Player.SendInfoMessage("{0}被封禁了，原因是'{1}'。", user.Name, reason);
										}
										else
										{
											TSPlayer.All.SendInfoMessage("{0}被封禁了，原因是'{1}'。", user.Name, reason);
>>>>>>> e2683b6... 手动加入RYH修改的文件
										}
									}
									else
									{
										if (args.Silent)
										{
<<<<<<< HEAD
											args.Player.SendInfoMessage("{0}banned {1} for '{2}'.", force ? "Force " : "", user.Name, reason);
										}
										else
										{
											TSPlayer.All.SendInfoMessage("{0} {1}banned {2} for '{3}'.", args.Player.Name, force ? "Force " : "", user.Name, reason);
=======
											args.Player.SendInfoMessage("你封禁了{1}，原因是'{2}'。", user.Name, reason);
										}
										else
										{
											TSPlayer.All.SendInfoMessage("{0}封禁了{2}，原因是'{3}'。", args.Player.Name, force ? "Force " : "", user.Name, reason);
>>>>>>> e2683b6... 手动加入RYH修改的文件
										}
									}
								}
							}
							else
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid player or account!");
=======
								args.Player.SendErrorMessage("用户不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
						else if (players.Count > 1)
							TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
						else
						{
							if (!TShock.Utils.Ban(players[0], reason, !args.Player.RealPlayer, args.Player.User.Name))
<<<<<<< HEAD
								args.Player.SendErrorMessage("You can't ban {0}!", players[0].Name);
=======
								args.Player.SendErrorMessage("你不能封禁 {0}。", players[0].Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
					#endregion
					return;
				case "addip":
					#region Add IP ban
					{
						if (args.Parameters.Count < 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}ban addip <ip> [reason]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封 addip <IP> [原因]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string ip = args.Parameters[1];
						string reason = args.Parameters.Count > 2
											? String.Join(" ", args.Parameters.GetRange(2, args.Parameters.Count - 2))
<<<<<<< HEAD
											: "Manually added IP address ban.";
						TShock.Bans.AddBan(ip, "", "", reason, false, args.Player.User.Name);
						args.Player.SendSuccessMessage("Banned IP {0}.", ip);
=======
											: "IP地址封禁。";
						TShock.Bans.AddBan(ip, "", "", reason, false, args.Player.User.Name);
						args.Player.SendSuccessMessage("封禁了IP {0}。", ip);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "addtemp":
					#region Add temp ban
					{
						if (args.Parameters.Count < 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}ban addtemp <player> <time> [reason]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封 addtemp <用户名> <时间> [原因]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						int time;
						if (!TShock.Utils.TryParseTime(args.Parameters[2], out time))
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid time string! Proper format: _d_h_m_s, with at least one time specifier.");
							args.Player.SendErrorMessage("For example, 1d and 10h-30m+2m are both valid time strings, but 2 is not.");
=======
							args.Player.SendErrorMessage("时间格式错误。 格式： _d_h_m_s, 日时分秒至少有一个。");
							args.Player.SendErrorMessage("例如，1d和10h-30m+2m都是可用的时间，但2不是。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string reason = args.Parameters.Count > 3
											? String.Join(" ", args.Parameters.Skip(3))
<<<<<<< HEAD
											: "Misbehavior.";
=======
											: "行为不检。";
>>>>>>> e2683b6... 手动加入RYH修改的文件

						List<TSPlayer> players = TShock.Utils.FindPlayer(args.Parameters[1]);
						if (players.Count == 0)
						{
							var user = TShock.Users.GetUserByName(args.Parameters[1]);
							if (user != null)
							{
								bool force = !args.Player.RealPlayer;
								if (TShock.Groups.GetGroupByName(user.Group).HasPermission(Permissions.immunetoban) && !force)
<<<<<<< HEAD
									args.Player.SendErrorMessage("You can't ban {0}!", user.Name);
=======
									args.Player.SendErrorMessage("你不能封禁 {0}。", user.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								else
								{
									var knownIps = JsonConvert.DeserializeObject<List<string>>(user.KnownIps);
									TShock.Bans.AddBan(knownIps.Last(), user.Name, user.UUID, reason, false, args.Player.User.Name, DateTime.UtcNow.AddSeconds(time).ToString("s"));
									if (String.IsNullOrWhiteSpace(args.Player.User.Name))
									{
										if (args.Silent)
										{
<<<<<<< HEAD
											args.Player.SendInfoMessage("{0} was {1}banned for '{2}'.", user.Name, force ? "force " : "", reason);
										}
										else
										{
											TSPlayer.All.SendInfoMessage("{0} was {1}banned for '{2}'.", user.Name, force ? "force " : "", reason);
=======
											args.Player.SendInfoMessage("{0}被封禁了，原因是'{1}'。'。", user.Name, reason);
										}
										else
										{
											TSPlayer.All.SendInfoMessage("{0}被封禁了，原因是'{1}'。'。", user.Name, reason);
>>>>>>> e2683b6... 手动加入RYH修改的文件
										}
									}
									else
									{
										if (args.Silent)
										{
<<<<<<< HEAD
											args.Player.SendInfoMessage("{0} was {1}banned for '{2}'.", user.Name, force ? "force " : "", reason);
=======
											args.Player.SendInfoMessage("{0}被封禁了，原因是'{1}'。'。", user.Name, reason);
>>>>>>> e2683b6... 手动加入RYH修改的文件
										}
										else
										{
											TSPlayer.All.SendInfoMessage("{0} {1}banned {2} for '{3}'.", args.Player.Name, force ? "force " : "", user.Name, reason);
										}
									}
								}
							}
							else
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid player or account!");
=======
								args.Player.SendErrorMessage("用户不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						}
						else if (players.Count > 1)
							TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
						else
						{
<<<<<<< HEAD
							if (args.Player.RealPlayer && players[0].HasPermission(Permissions.immunetoban))
							{
								args.Player.SendErrorMessage("You can't ban {0}!", players[0].Name);
=======
							if (args.Player.RealPlayer && players[0].Group.HasPermission(Permissions.immunetoban))
							{
								args.Player.SendErrorMessage("你不能封禁 {0}。", players[0].Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}

							if (TShock.Bans.AddBan(players[0].IP, players[0].Name, players[0].UUID, reason,
								false, args.Player.Name, DateTime.UtcNow.AddSeconds(time).ToString("s")))
							{
<<<<<<< HEAD
								players[0].Disconnect(String.Format("Banned: {0}", reason));
=======
								players[0].Disconnect(String.Format("被封禁: {0}", reason));
>>>>>>> e2683b6... 手动加入RYH修改的文件
								string verb = args.Player.RealPlayer ? "Force " : "";
								if (args.Player.RealPlayer)
									if (args.Silent)
									{
<<<<<<< HEAD
										args.Player.SendSuccessMessage("{0}banned {1} for '{2}'", verb, players[0].Name, reason);
									}
									else
									{
										TSPlayer.All.SendSuccessMessage("{0} {1}banned {2} for '{3}'", args.Player.Name, verb, players[0].Name, reason);
=======
										args.Player.SendSuccessMessage("封禁了{1}，原因是'{2}'", verb, players[0].Name, reason);
									}
									else
									{
										TSPlayer.All.SendSuccessMessage("{0}封禁了{2}，原因是'{3}'", args.Player.Name, verb, players[0].Name, reason);
>>>>>>> e2683b6... 手动加入RYH修改的文件
									}
								else
								{
									if (args.Silent) 
									{
<<<<<<< HEAD
										args.Player.SendSuccessMessage("{0}banned {1} for '{2}'", verb, players[0].Name, reason);
									}
									else
									{
										TSPlayer.All.SendSuccessMessage("{0} was {1}banned for '{2}'", players[0].Name, verb, reason);
=======
										args.Player.SendSuccessMessage("封禁了{1}，原因是'{2}'", verb, players[0].Name, reason);
									}
									else
									{
										TSPlayer.All.SendSuccessMessage("{0}被封禁了，原因是'{2}'。'", players[0].Name, verb, reason);
>>>>>>> e2683b6... 手动加入RYH修改的文件
									}
								}
							}
							else
<<<<<<< HEAD
								args.Player.SendErrorMessage("Failed to ban {0}, check logs.", players[0].Name);
=======
								args.Player.SendErrorMessage("试图封禁{0}失败，查看日志获得更多信息。", players[0].Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
					#endregion
					return;
				case "del":
					#region Delete ban
					{
						if (args.Parameters.Count != 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}ban del <player>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封 del <用户名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string plStr = args.Parameters[1];
						Ban ban = TShock.Bans.GetBanByName(plStr, false);
						if (ban != null)
						{
							if (TShock.Bans.RemoveBan(ban.Name, true))
<<<<<<< HEAD
								args.Player.SendSuccessMessage("Unbanned {0} ({1}).", ban.Name, ban.IP);
							else
								args.Player.SendErrorMessage("Failed to unban {0} ({1}), check logs.", ban.Name, ban.IP);
						}
						else
							args.Player.SendErrorMessage("No bans for {0} exist.", plStr);
=======
								args.Player.SendSuccessMessage("解除封禁 {0} ({1})。", ban.Name, ban.IP);
							else
								args.Player.SendErrorMessage("无法解除封禁{0} ({1})，查看日志获得更多信息。", ban.Name, ban.IP);
						}
						else
							args.Player.SendErrorMessage("{0}没有被封禁。。", plStr);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "delip":
					#region Delete IP ban
					{
						if (args.Parameters.Count != 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}ban delip <ip>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封 delip <ip>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string ip = args.Parameters[1];
						Ban ban = TShock.Bans.GetBanByIp(ip);
						if (ban != null)
						{
							if (TShock.Bans.RemoveBan(ban.IP, false))
<<<<<<< HEAD
								args.Player.SendSuccessMessage("Unbanned IP {0} ({1}).", ban.IP, ban.Name);
							else
								args.Player.SendErrorMessage("Failed to unban IP {0} ({1}), check logs.", ban.IP, ban.Name);
						}
						else
							args.Player.SendErrorMessage("IP {0} is not banned.", ip);
=======
								args.Player.SendSuccessMessage("解除封禁IP {0} ({1})。", ban.IP, ban.Name);
							else
								args.Player.SendErrorMessage("无法解除IP封禁{0} ({1})，查看日志获得更多信息。", ban.IP, ban.Name);
						}
						else
							args.Player.SendErrorMessage("IP {0} 没有被封禁。", ip);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "help":
					#region Help
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						var lines = new List<string>
						{
<<<<<<< HEAD
							"add <player> [reason] - Bans a player or user account if the player is not online.",
							"addip <ip> [reason] - Bans an IP.",
							"addtemp <player> <time> [reason] - Temporarily bans a player.",
							"del <player> - Unbans a player.",
							"delip <ip> - Unbans an IP.",
							"list [page] - Lists all player bans.",
							"listip [page] - Lists all IP bans."
=======
							"add <玩家名> [原因] - 封禁一个玩家。",
							"addip <IP> [原因] - 封禁一个IP。",
							"addtemp <玩家名> <时长> [原因] - 临时封禁一个玩家。",
							"del <玩家名> - 解除封禁。",
							"delip <IP> - 解除IP封禁。",
							"list [页码] - 列出所有封禁。",
							"listip [页码] - 列出所有IP封禁。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
                        };
						
						PaginationTools.SendPage(args.Player, pageNumber, lines,
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Ban Sub-Commands ({0}/{1}):",
								FooterFormat = "Type {0}ban help {{0}} for more sub-commands.".SFormat(Specifier)
=======
								HeaderFormat = "封 子命令 ({0}/{1}):",
								FooterFormat = "输入 {0}封 help {{0}} 查看子命令。".SFormat(Specifier)
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						);
					}
					#endregion
					return;
				case "list":
					#region List bans
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
						{
							return;
						}

						List<Ban> bans = TShock.Bans.GetBans();

						var nameBans = from ban in bans
									   where !String.IsNullOrEmpty(ban.Name)
									   select ban.Name;

						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(nameBans),
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Bans ({0}/{1}):",
								FooterFormat = "Type {0}ban list {{0}} for more.".SFormat(Specifier),
								NothingToDisplayString = "There are currently no bans."
=======
								HeaderFormat = "封禁 ({0}/{1}):",
								FooterFormat = "输入 {0}封 list {{0}} 查看更多。".SFormat(Specifier),
								NothingToDisplayString = "目前没有封禁。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
							});
					}
					#endregion
					return;
				case "listip":
					#region List IP bans
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
						{
							return;
						}

						List<Ban> bans = TShock.Bans.GetBans();

						var ipBans = from ban in bans
									 where String.IsNullOrEmpty(ban.Name)
									 select ban.IP;

						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(ipBans),
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "IP Bans ({0}/{1}):",
								FooterFormat = "Type {0}ban listip {{0}} for more.".SFormat(Specifier),
								NothingToDisplayString = "There are currently no IP bans."
=======
								HeaderFormat = "IP封禁 ({0}/{1}):",
								FooterFormat = "输入 {0}封 listip {{0}} 查看更多。".SFormat(Specifier),
								NothingToDisplayString = "目前没有IP封禁。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
							});
					}
					#endregion
					return;
				default:
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid subcommand! Type {0}ban help for more information.", Specifier);
=======
					args.Player.SendErrorMessage("格式错误。 输入{0}ban help 查看更多信息。", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
			}
		}

		private static void Whitelist(CommandArgs args)
		{
			if (args.Parameters.Count == 1)
			{
				using (var tw = new StreamWriter(FileTools.WhitelistPath, true))
				{
					tw.WriteLine(args.Parameters[0]);
				}
<<<<<<< HEAD
				args.Player.SendSuccessMessage("Added " + args.Parameters[0] + " to the whitelist.");
=======
				args.Player.SendSuccessMessage("添加 " + args.Parameters[0] + " 到白名单。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void DisplayLogs(CommandArgs args)
		{
			args.Player.DisplayLogs = (!args.Player.DisplayLogs);
<<<<<<< HEAD
			args.Player.SendSuccessMessage("You will " + (args.Player.DisplayLogs ? "now" : "no longer") + " receive logs.");
=======
			args.Player.SendSuccessMessage("你 " + (args.Player.DisplayLogs ? "将" : "不再") + " 收到日志。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void SaveSSC(CommandArgs args)
		{
			if (Main.ServerSideCharacter)
			{
<<<<<<< HEAD
				args.Player.SendSuccessMessage("SSC has been saved.");
=======
				args.Player.SendSuccessMessage("强制开荒已存档。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				foreach (TSPlayer player in TShock.Players)
				{
					if (player != null && player.IsLoggedIn && !player.IgnoreActionsForClearingTrashCan)
					{
						TShock.CharacterDB.InsertPlayerData(player);
					}
				}
			}
		}

		private static void OverrideSSC(CommandArgs args)
		{
			if (!Main.ServerSideCharacter)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Server Side Characters is disabled.");
=======
				args.Player.SendErrorMessage("强制开荒已关闭。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			if( args.Parameters.Count < 1 )
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Correct usage: {0}overridessc|{0}ossc <player name>", Specifier);
=======
				args.Player.SendErrorMessage("正确用法： {0}免开荒 <玩家名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			string playerNameToMatch = string.Join(" ", args.Parameters);
			var matchedPlayers = TShock.Utils.FindPlayer(playerNameToMatch);
			if( matchedPlayers.Count < 1 )
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("No players matched \"{0}\".", playerNameToMatch);
=======
				args.Player.SendErrorMessage("没有找到玩家 \"{0}\"。", playerNameToMatch);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			else if( matchedPlayers.Count > 1 )
			{
				TShock.Utils.SendMultipleMatchError(args.Player, matchedPlayers.Select(p => p.Name));
				return;
			}

			TSPlayer matchedPlayer = matchedPlayers[0];
			if (matchedPlayer.IsLoggedIn)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Player \"{0}\" is already logged in.", matchedPlayer.Name);
=======
				args.Player.SendErrorMessage("玩家 \"{0}\" is already logged in。", matchedPlayer.Name);//无需翻译，没人用这个命令
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			if (!matchedPlayer.LoginFailsBySsi)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Player \"{0}\" has to perform a /login attempt first.", matchedPlayer.Name);
=======
				args.Player.SendErrorMessage("玩家 \"{0}\" has to perform a /login attempt first。", matchedPlayer.Name);//无需翻译，没人用这个命令
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			if (matchedPlayer.IgnoreActionsForClearingTrashCan)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Player \"{0}\" has to reconnect first.", matchedPlayer.Name);
=======
				args.Player.SendErrorMessage("玩家 \"{0}\" has to reconnect first。", matchedPlayer.Name);//无需翻译，没人用这个命令
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			TShock.CharacterDB.InsertPlayerData(matchedPlayer);
<<<<<<< HEAD
			args.Player.SendSuccessMessage("SSC of player \"{0}\" has been overriden.", matchedPlayer.Name);
=======
			args.Player.SendSuccessMessage("\"{0}\" 的存档已被覆盖。", matchedPlayer.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void ForceHalloween(CommandArgs args)
		{
			TShock.Config.ForceHalloween = !TShock.Config.ForceHalloween;
			Main.checkHalloween();
			if (args.Silent) 
<<<<<<< HEAD
				args.Player.SendInfoMessage("{0}abled halloween mode!", (TShock.Config.ForceHalloween ? "en" : "dis"));
			else
				TSPlayer.All.SendInfoMessage("{0} {1}abled halloween mode!", args.Player.Name, (TShock.Config.ForceHalloween ? "en" : "dis"));
=======
				args.Player.SendInfoMessage("{0}了万圣。", (TShock.Config.ForceHalloween ? "开启" : "关闭"));
			else
				TSPlayer.All.SendInfoMessage("{0} {1}了万圣。", args.Player.Name, (TShock.Config.ForceHalloween ? "开启" : "关闭"));
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void ForceXmas(CommandArgs args)
		{
			TShock.Config.ForceXmas = !TShock.Config.ForceXmas;
			Main.CheckXMas();
			if (args.Silent)
<<<<<<< HEAD
				args.Player.SendInfoMessage("{0}abled Christmas mode!", (TShock.Config.ForceXmas ? "en" : "dis"));
			else
				TSPlayer.All.SendInfoMessage("{0} {1}abled Christmas mode!", args.Player.Name, (TShock.Config.ForceXmas ? "en" : "dis"));
=======
				args.Player.SendInfoMessage("{0}了圣诞。", (TShock.Config.ForceXmas ? "开启" : "关闭"));
			else
				TSPlayer.All.SendInfoMessage("{0} {1}了圣诞。", args.Player.Name, (TShock.Config.ForceXmas ? "开启" : "关闭"));
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void TempGroup(CommandArgs args)
		{
			if (args.Parameters.Count < 2)
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("Invalid usage");
				args.Player.SendInfoMessage("Usage: {0}tempgroup <username> <new group> [time]", Specifier);
=======
				args.Player.SendInfoMessage("格式错误。");
				args.Player.SendInfoMessage("格式:  {0}临时组 <用户名> <组> [时长]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			List<TSPlayer> ply = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (ply.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Could not find player {0}.", args.Parameters[0]);
=======
				args.Player.SendErrorMessage("找不到玩家{0}。", args.Parameters[0]);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			if (ply.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, ply.Select(p => p.User.Name));
			}

			if (!TShock.Groups.GroupExists(args.Parameters[1]))
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Could not find group {0}", args.Parameters[1]);
=======
				args.Player.SendErrorMessage("找不到{0}组。", args.Parameters[1]);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			if (args.Parameters.Count > 2)
			{
				int time;
				if (!TShock.Utils.TryParseTime(args.Parameters[2], out time))
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid time string! Proper format: _d_h_m_s, with at least one time specifier.");
					args.Player.SendErrorMessage("For example, 1d and 10h-30m+2m are both valid time strings, but 2 is not.");
=======
					args.Player.SendErrorMessage("时间格式错误。 格式： _d_h_m_s, 日时分秒至少有一个。");
					args.Player.SendErrorMessage("例如，1d和10h-30m+2m都是可用的时间，但2不是。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}

				ply[0].tempGroupTimer = new System.Timers.Timer(time * 1000);
				ply[0].tempGroupTimer.Elapsed += ply[0].TempGroupTimerElapsed;
				ply[0].tempGroupTimer.Start();
			}

			Group g = TShock.Utils.GetGroup(args.Parameters[1]);

			ply[0].tempGroup = g;

			if (args.Parameters.Count < 3)
			{
<<<<<<< HEAD
				args.Player.SendSuccessMessage(String.Format("You have changed {0}'s group to {1}", ply[0].Name, g.Name));
				ply[0].SendSuccessMessage(String.Format("Your group has temporarily been changed to {0}", g.Name));
			}
			else
			{
				args.Player.SendSuccessMessage(String.Format("You have changed {0}'s group to {1} for {2}",
					ply[0].Name, g.Name, args.Parameters[2]));
				ply[0].SendSuccessMessage(String.Format("Your group has been changed to {0} for {1}",
=======
				args.Player.SendSuccessMessage(String.Format("你把{0}设置到了{1}组。", ply[0].Name, g.Name));
				ply[0].SendSuccessMessage(String.Format("你被调至了{0}组。", g.Name));
			}
			else
			{
				args.Player.SendSuccessMessage(String.Format("你把{0}设置到了{1}组，时间{2}。",
					ply[0].Name, g.Name, args.Parameters[2]));
				ply[0].SendSuccessMessage(String.Format("你被调至了{0}组，时间{1}。",
>>>>>>> e2683b6... 手动加入RYH修改的文件
					g.Name, args.Parameters[2]));
			}
		}

		#endregion Player Management Commands

		#region Server Maintenence Commands

		private static void Broadcast(CommandArgs args)
		{
			string message = string.Join(" ", args.Parameters);

			TShock.Utils.Broadcast(
<<<<<<< HEAD
				"(Server Broadcast) " + message, 
=======
				"(服务器广播) " + message, 
>>>>>>> e2683b6... 手动加入RYH修改的文件
				Convert.ToByte(TShock.Config.BroadcastRGB[0]), Convert.ToByte(TShock.Config.BroadcastRGB[1]), 
				Convert.ToByte(TShock.Config.BroadcastRGB[2]));
		}

		private static void Off(CommandArgs args)
		{

			if (Main.ServerSideCharacter)
			{
				foreach (TSPlayer player in TShock.Players)
				{
					if (player != null && player.IsLoggedIn && !player.IgnoreActionsForClearingTrashCan)
					{
						player.SaveServerCharacter();
					}
				}
			}

<<<<<<< HEAD
			string reason = ((args.Parameters.Count > 0) ? "Server shutting down: " + String.Join(" ", args.Parameters) : "Server shutting down!");
=======
			string reason = ((args.Parameters.Count > 0) ? "Server shutting down: " + String.Join(" ", args.Parameters) : "服务器关闭。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			TShock.Utils.StopServer(true, reason);
		}
		
		private static void Restart(CommandArgs args)
		{
<<<<<<< HEAD
			if (TShock.NoRestart)
			{
				args.Player.SendErrorMessage("This command has been disabled.");
				return;
			}

			if (ServerApi.RunningMono)
			{
				TShock.Log.ConsoleInfo("Sorry, this command has not yet been implemented in Mono.");
			}
			else
			{
				string reason = ((args.Parameters.Count > 0) ? "Server shutting down: " + String.Join(" ", args.Parameters) : "Server shutting down!");
=======
			if (ServerApi.RunningMono)
			{
				TShock.Log.ConsoleInfo("这个命令在Mono中未执行。");
			}
			else
			{
				string reason = ((args.Parameters.Count > 0) ? "Server shutting down: " + String.Join(" ", args.Parameters) : "服务器关闭。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				TShock.Utils.RestartServer(true, reason);
			}
		}

		private static void OffNoSave(CommandArgs args)
		{
<<<<<<< HEAD
			string reason = ((args.Parameters.Count > 0) ? "Server shutting down: " + String.Join(" ", args.Parameters) : "Server shutting down!");
=======
			string reason = ((args.Parameters.Count > 0) ? "Server shutting down: " + String.Join(" ", args.Parameters) : "服务器关闭。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			TShock.Utils.StopServer(false, reason);
		}

		private static void CheckUpdates(CommandArgs args)
		{
<<<<<<< HEAD
			args.Player.SendInfoMessage("An update check has been queued.");
=======
			args.Player.SendInfoMessage("准备检查更新。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			try
			{
				TShock.UpdateManager.UpdateCheck(null);
			}
			catch (Exception)
			{
				//swallow the exception
				return;
			}
		}

		private static void ManageRest(CommandArgs args)
		{
			string subCommand = "help";
			if (args.Parameters.Count > 0)
				subCommand = args.Parameters[0];

			switch(subCommand.ToLower())
			{
				case "listusers":
				{
					int pageNumber;
					if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
						return;

					Dictionary<string,int> restUsersTokens = new Dictionary<string,int>();
					foreach (Rests.SecureRest.TokenData tokenData in TShock.RestApi.Tokens.Values)
					{
						if (restUsersTokens.ContainsKey(tokenData.Username))
							restUsersTokens[tokenData.Username]++;
						else
							restUsersTokens.Add(tokenData.Username, 1);
					}

					List<string> restUsers = new List<string>(
						restUsersTokens.Select(ut => string.Format("{0} ({1} tokens)", ut.Key, ut.Value)));

					PaginationTools.SendPage(
						args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(restUsers), new PaginationTools.Settings {
<<<<<<< HEAD
							NothingToDisplayString = "There are currently no active REST users.",
							HeaderFormat = "Active REST Users ({0}/{1}):",
							FooterFormat = "Type {0}rest listusers {{0}} for more.".SFormat(Specifier)
=======
							NothingToDisplayString = "目前没有活跃远程用户。",
							HeaderFormat = "活跃远程用户 ({0}/{1}):",
							FooterFormat = "输入 {0}远程 listusers {{0}} 查看更多。".SFormat(Specifier)
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					);

					break;
				}
				case "destroytokens":
				{
					TShock.RestApi.Tokens.Clear();
<<<<<<< HEAD
					args.Player.SendSuccessMessage("All REST tokens have been destroyed.");
=======
					args.Player.SendSuccessMessage("All REST tokens have been destroyed。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					break;
				}
				default:
				{
<<<<<<< HEAD
					args.Player.SendInfoMessage("Available REST Sub-Commands:");
					args.Player.SendMessage("listusers - Lists all REST users and their current active tokens.", Color.White);
					args.Player.SendMessage("destroytokens - Destroys all current REST tokens.", Color.White);
=======
					args.Player.SendInfoMessage("可用的REST字命令:");
					args.Player.SendMessage("listusers - Lists all REST users and their current active tokens。", Color.White);
					args.Player.SendMessage("destroytokens - Destroys all current REST tokens。", Color.White);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					break;
				}
			}
		}

		#endregion Server Maintenence Commands

        #region Cause Events and Spawn Monsters Commands

		private static void DropMeteor(CommandArgs args)
		{
			WorldGen.spawnMeteor = false;
			WorldGen.dropMeteor();
			if (args.Silent)
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("A meteor has been triggered.");
			}
			else
			{
				TSPlayer.All.SendInfoMessage("{0} triggered a meteor.", args.Player.Name);
=======
				args.Player.SendInfoMessage("一颗陨石坠落了.");
			}
			else
			{
				TSPlayer.All.SendInfoMessage("{0} 召来了陨石。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Fullmoon(CommandArgs args)
		{
			TSPlayer.Server.SetFullMoon();
			if (args.Silent)
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("Started a full moon.");
			}
			else
			{
				TSPlayer.All.SendInfoMessage("{0} started a full moon.", args.Player.Name);
=======
				args.Player.SendInfoMessage("满月降临。");
			}
			else
			{
				TSPlayer.All.SendInfoMessage("{0} 引来了满月。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Bloodmoon(CommandArgs args)
		{
			TSPlayer.Server.SetBloodMoon(!Main.bloodMoon);
			if (args.Silent)
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("{0}ed a blood moon.", Main.bloodMoon ? "start" : "stopp");
			}
			else
			{
				TSPlayer.All.SendInfoMessage("{0} {1}ed a blood moon.", args.Player.Name, Main.bloodMoon ? "start" : "stopp");
=======
				args.Player.SendInfoMessage("血色之月已经{0}。", Main.bloodMoon ? "升起" : "降落");
			}
			else
			{
				TSPlayer.All.SendInfoMessage("{0} 血色之月已经{1}。", args.Player.Name, Main.bloodMoon ? "升起" : "降落");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Eclipse(CommandArgs args)
		{
			TSPlayer.Server.SetEclipse(!Main.eclipse);
			if (args.Silent)
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("{0}ed an eclipse.", Main.eclipse ? "start" : "stopp");
			}
			else
			{
				TSPlayer.All.SendInfoMessage("{0} {1}ed an eclipse.", args.Player.Name, Main.eclipse ? "start" : "stopp");
=======
				args.Player.SendInfoMessage("{0}了日食。", Main.eclipse ? "开启" : "关闭");
			}
			else
			{
				TSPlayer.All.SendInfoMessage("{0} {1}了日食。", args.Player.Name, Main.eclipse ? "开启" : "关闭");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Invade(CommandArgs args)
		{
			if (Main.invasionSize <= 0)
			{
				if (args.Parameters.Count < 1)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}invade <invasion type> [wave]", Specifier);
=======
					args.Player.SendErrorMessage("格式错误。 格式: {0}入侵 <名称> [波数]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}

				int wave = 1;
				switch (args.Parameters[0].ToLower())
				{
					case "goblin":
					case "goblins":
<<<<<<< HEAD
						TSPlayer.All.SendInfoMessage("{0} has started a goblin army invasion.", args.Player.Name);
=======
						TSPlayer.All.SendInfoMessage("{0} 引来了哥布林军队入侵。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						TShock.StartInvasion(1);
						break;

					case "snowman":
					case "snowmen":
<<<<<<< HEAD
						TSPlayer.All.SendInfoMessage("{0} has started a snow legion invasion.", args.Player.Name);
=======
						TSPlayer.All.SendInfoMessage("{0} 引来了雪人军团入侵。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						TShock.StartInvasion(2);
						break;

					case "pirate":
					case "pirates":
<<<<<<< HEAD
						TSPlayer.All.SendInfoMessage("{0} has started a pirate invasion.", args.Player.Name);
=======
						TSPlayer.All.SendInfoMessage("{0} 引来了海盗入侵。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						TShock.StartInvasion(3);
						break;

					case "pumpkin":
					case "pumpkinmoon":
						if (args.Parameters.Count > 1)
						{
							if (!int.TryParse(args.Parameters[1], out wave) || wave <= 0)
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid wave!");
=======
								args.Player.SendErrorMessage("波数错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
								break;
							}
						}

						TSPlayer.Server.SetPumpkinMoon(true);
						Main.bloodMoon = false;
						NPC.waveKills = 0f;
						NPC.waveCount = wave;
<<<<<<< HEAD
						TSPlayer.All.SendInfoMessage("{0} started the pumpkin moon at wave {1}!", args.Player.Name, wave);
=======
						TSPlayer.All.SendInfoMessage("{0} 召来了南瓜月第 {1} 波。", args.Player.Name, wave);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						break;

					case "frost":
					case "frostmoon":
						if (args.Parameters.Count > 1)
						{
							if (!int.TryParse(args.Parameters[1], out wave) || wave <= 0)
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid wave!");
=======
								args.Player.SendErrorMessage("波数错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}
						}

						TSPlayer.Server.SetFrostMoon(true);
						Main.bloodMoon = false;
						NPC.waveKills = 0f;
						NPC.waveCount = wave;
<<<<<<< HEAD
						TSPlayer.All.SendInfoMessage("{0} started the frost moon at wave {1}!", args.Player.Name, wave);
=======
						TSPlayer.All.SendInfoMessage("{0} 召来了霜月第 {1} 波。", args.Player.Name, wave);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						break;

					case "martian":
					case "martians":
<<<<<<< HEAD
						TSPlayer.All.SendInfoMessage("{0} has started a martian invasion.", args.Player.Name);
=======
						TSPlayer.All.SendInfoMessage("{0} 引来了火星入侵。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						TShock.StartInvasion(4);
						break;
				}
			}
			else
			{
<<<<<<< HEAD
				TSPlayer.All.SendInfoMessage("{0} has ended the invasion.", args.Player.Name);
=======
				TSPlayer.All.SendInfoMessage("{0} 结束了入侵。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				Main.invasionSize = 0;
			}
		}

		private static void ClearAnglerQuests(CommandArgs args)
		{
			if (args.Parameters.Count > 0)
			{
				var result = Main.anglerWhoFinishedToday.RemoveAll(s => s.ToLower().Equals(args.Parameters[0].ToLower()));
				if (result > 0)
				{
<<<<<<< HEAD
					args.Player.SendSuccessMessage("Removed {0} players from the angler quest completion list for today.", result);
=======
					args.Player.SendSuccessMessage("清除{0}个玩家今天完成的渔夫任务。", result);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					foreach (TSPlayer ply in TShock.Players.Where(p => p!= null && p.Active && p.TPlayer.name.ToLower().Equals(args.Parameters[0].ToLower())))
					{
						//this will always tell the client that they have not done the quest today.
						ply.SendData((PacketTypes)74, "");
					}
				}
				else
<<<<<<< HEAD
					args.Player.SendErrorMessage("Failed to find any users by that name on the list.");
=======
					args.Player.SendErrorMessage("找不到该名称。");
>>>>>>> e2683b6... 手动加入RYH修改的文件

			}
			else
			{
				Main.anglerWhoFinishedToday.Clear();
				NetMessage.SendAnglerQuest();
<<<<<<< HEAD
				args.Player.SendSuccessMessage("Cleared all users from the angler quest completion list for today.");
=======
				args.Player.SendSuccessMessage("清除所有玩家今天完成的渔夫任务。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void ToggleExpert(CommandArgs args)
		{
			Main.expertMode = !Main.expertMode;
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
<<<<<<< HEAD
			args.Player.SendSuccessMessage("Expert mode is now {0}.", Main.expertMode ? "on" : "off");
=======
			args.Player.SendSuccessMessage("专家模式已 {0}。", Main.expertMode ? "开启" : "关闭");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void Hardmode(CommandArgs args)
		{
			if (Main.hardMode)
			{
				Main.hardMode = false;
				TSPlayer.All.SendData(PacketTypes.WorldInfo);
<<<<<<< HEAD
				args.Player.SendSuccessMessage("Hardmode is now off.");
=======
				args.Player.SendSuccessMessage("设为肉山前。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (!TShock.Config.DisableHardmode)
			{
				WorldGen.StartHardmode();
<<<<<<< HEAD
				args.Player.SendSuccessMessage("Hardmode is now on.");
			}
			else
			{
				args.Player.SendErrorMessage("Hardmode is disabled via config.");
=======
				args.Player.SendSuccessMessage("设为肉山后。");
			}
			else
			{
				args.Player.SendErrorMessage("配置文件禁止肉山。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void SpawnBoss(CommandArgs args)
		{
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}spawnboss <boss type> [amount]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}刷Boss <boss type> [amount]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			int amount = 1;
			if (args.Parameters.Count == 2 && (!int.TryParse(args.Parameters[1], out amount) || amount <= 0))
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid boss amount!");
=======
				args.Player.SendErrorMessage("数量错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			NPC npc = new NPC();
			switch (args.Parameters[0].ToLower())
			{
				case "*":
				case "all":
					int[] npcIds = { 4, 13, 35, 50, 125, 126, 127, 134, 222, 245, 262, 266, 370, 398 };
					TSPlayer.Server.SetTime(false, 0.0);
					foreach (int i in npcIds)
					{
						npc.SetDefaults(i);
						TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
					}
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned all bosses {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了所有Boss{1}次。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "brain":
				case "brain of cthulhu":
					npc.SetDefaults(266);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned the Brain of Cthulhu {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个克苏鲁之脑。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "destroyer":
					npc.SetDefaults(134);
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned the Destroyer {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个钢铁毁灭者。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "duke":
				case "duke fishron":
				case "fishron":
					npc.SetDefaults(370);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned Duke Fishron {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个猪鲨公爵。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "eater":
				case "eater of worlds":
					npc.SetDefaults(13);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned the Eater of Worlds {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个世界吞噬者。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "eye":
				case "eye of cthulhu":
					npc.SetDefaults(4);
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned the Eye of Cthulhu {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个克苏鲁之眼。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "golem":
					npc.SetDefaults(245);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned Golem {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个石巨人。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "king":
				case "king slime":
					npc.SetDefaults(50);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned King Slime {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个史莱姆王。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "plantera":
					npc.SetDefaults(262);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned Plantera {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个世纪之花。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "prime":
				case "skeletron prime":
					npc.SetDefaults(127);
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned Skeletron Prime {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个骷髅总理。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "queen":
				case "queen bee":
					npc.SetDefaults(222);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned Queen Bee {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个蜂后。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "skeletron":
					npc.SetDefaults(35);
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned Skeletron {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个地牢守卫。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "twins":
					TSPlayer.Server.SetTime(false, 0.0);
					npc.SetDefaults(125);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
					npc.SetDefaults(126);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned the Twins {1} time(s).", args.Player.Name, amount);
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个双子魔眼。", args.Player.Name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "wof":
				case "wall of flesh":
					if (Main.wof >= 0)
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("There is already a Wall of Flesh!");
=======
						args.Player.SendErrorMessage("不能同时大两个血肉之墙。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						return;
					}
					if (args.Player.Y / 16f < Main.maxTilesY - 205)
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("You must spawn the Wall of Flesh in hell!");
						return;
					}
					NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
					TSPlayer.All.SendSuccessMessage("{0} has spawned the Wall of Flesh.", args.Player.Name);
=======
						args.Player.SendErrorMessage("必须在地狱中召唤血肉之墙。");
						return;
					}
					NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
					TSPlayer.All.SendSuccessMessage("{0} 召唤了血肉之墙。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				case "moon":
				case "moon lord":
					npc.SetDefaults(398);
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
<<<<<<< HEAD
					TSPlayer.All.SendSuccessMessage("{0} has spawned the Moon Lord {1} time(s).", args.Player.Name, amount);
					return;
				default:
					args.Player.SendErrorMessage("Invalid boss type!");
=======
					TSPlayer.All.SendSuccessMessage("{0} 召唤了{1}个月神。", args.Player.Name, amount);
					return;
				default:
					args.Player.SendErrorMessage("类型错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
			}
		}

		private static void SpawnMob(CommandArgs args)
		{
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}spawnmob <mob type> [amount]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}刷怪 <mob type> [amount]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			if (args.Parameters[0].Length == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid mob type!");
=======
				args.Player.SendErrorMessage("没有这个怪物。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			int amount = 1;
			if (args.Parameters.Count == 2 && !int.TryParse(args.Parameters[1], out amount))
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}spawnmob <mob type> [amount]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}刷怪 <mob type> [amount]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			amount = Math.Min(amount, Main.maxNPCs);

			var npcs = TShock.Utils.GetNPCByIdOrName(args.Parameters[0]);
			if (npcs.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid mob type!");
=======
				args.Player.SendErrorMessage("没有这个怪物。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (npcs.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, npcs.Select(n => n.name));
			}
			else
			{
				var npc = npcs[0];
				if (npc.type >= 1 && npc.type < Main.maxNPCTypes && npc.type != 113)
				{
					TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY, 50, 20);
					if (args.Silent)
					{
<<<<<<< HEAD
						args.Player.SendSuccessMessage("Spawned {0} {1} time(s).", npc.name, amount);
					}
					else
					{
						TSPlayer.All.SendSuccessMessage("{0} has spawned {1} {2} time(s).", args.Player.Name, npc.name, amount);
=======
						args.Player.SendSuccessMessage("召唤 {1} 个 {0} 。", npc.name, amount);
					}
					else
					{
						TSPlayer.All.SendSuccessMessage("{0} 召唤了{2}个{1}。", args.Player.Name, npc.name, amount);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
				}
				else if (npc.type == 113)
				{
					if (Main.wof >= 0 || (args.Player.Y / 16f < (Main.maxTilesY - 205)))
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("Can't spawn Wall of Flesh!");
=======
						args.Player.SendErrorMessage("无法召唤血肉之墙。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						return;
					}
					NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
					if (args.Silent)
					{
<<<<<<< HEAD
						args.Player.SendSuccessMessage("Spawned Wall of Flesh!");
					}
					else
					{
						TSPlayer.All.SendSuccessMessage("{0} has spawned a Wall of Flesh!", args.Player.Name);
=======
						args.Player.SendSuccessMessage("召唤血肉之墙。");
					}
					else
					{
						TSPlayer.All.SendSuccessMessage("{0} 召唤了血肉之墙。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
				}
				else
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid mob type!");
=======
					args.Player.SendErrorMessage("没有这个怪物。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			}
		}

		#endregion Cause Events and Spawn Monsters Commands

		#region Teleport Commands

		private static void Home(CommandArgs args)
		{
			args.Player.Spawn();
<<<<<<< HEAD
			args.Player.SendSuccessMessage("Teleported to your spawnpoint.");
=======
			args.Player.SendSuccessMessage("回家。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void Spawn(CommandArgs args)
		{
			if (args.Player.Teleport(Main.spawnTileX*16, (Main.spawnTileY*16) -48))
<<<<<<< HEAD
				args.Player.SendSuccessMessage("Teleported to the map's spawnpoint.");
=======
				args.Player.SendSuccessMessage("回城。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void TP(CommandArgs args)
		{
			if (args.Parameters.Count != 1 && args.Parameters.Count != 2)
			{
<<<<<<< HEAD
				if (args.Player.HasPermission(Permissions.tpothers))
					args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tp <player> [player 2]", Specifier);
				else
					args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tp <player>", Specifier);
=======
				if (args.Player.Group.HasPermission(Permissions.tpothers))
					args.Player.SendErrorMessage("格式错误。 格式: {0}传送 <玩家名> [player 2]", Specifier);
				else
					args.Player.SendErrorMessage("格式错误。 格式: {0}传送 <玩家名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			if (args.Parameters.Count == 1)
			{
				var players = TShock.Utils.FindPlayer(args.Parameters[0]);
				if (players.Count == 0)
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid player!");
=======
					args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				else if (players.Count > 1)
					TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
				else
				{
					var target = players[0];
<<<<<<< HEAD
					if (!target.TPAllow && !args.Player.HasPermission(Permissions.tpoverride))
					{
						args.Player.SendErrorMessage("{0} has disabled players from teleporting.", target.Name);
=======
					if (!target.TPAllow && !args.Player.Group.HasPermission(Permissions.tpoverride))
					{
						args.Player.SendErrorMessage("{0} 禁止别人传送。", target.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						return;
					}
					if (args.Player.Teleport(target.TPlayer.position.X, target.TPlayer.position.Y))
					{
<<<<<<< HEAD
						args.Player.SendSuccessMessage("Teleported to {0}.", target.Name);
						if (!args.Player.HasPermission(Permissions.tpsilent))
							target.SendInfoMessage("{0} teleported to you.", args.Player.Name);
=======
						args.Player.SendSuccessMessage("传送到 {0}。", target.Name);
						if (!args.Player.Group.HasPermission(Permissions.tpsilent))
							target.SendInfoMessage("{0} 传送到了你。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
				}
			}
			else
			{
<<<<<<< HEAD
				if (!args.Player.HasPermission(Permissions.tpothers))
				{
					args.Player.SendErrorMessage("You do not have access to this command.");
=======
				if (!args.Player.Group.HasPermission(Permissions.tpothers))
				{
					args.Player.SendErrorMessage("你没有权限使用这个命令。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}

				var players1 = TShock.Utils.FindPlayer(args.Parameters[0]);
				var players2 = TShock.Utils.FindPlayer(args.Parameters[1]);

				if (players2.Count == 0)
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid player!");
=======
					args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				else if (players2.Count > 1)
					TShock.Utils.SendMultipleMatchError(args.Player, players2.Select(p => p.Name));
				else if (players1.Count == 0)
				{
					if (args.Parameters[0] == "*")
					{
<<<<<<< HEAD
						if (!args.Player.HasPermission(Permissions.tpallothers))
						{
							args.Player.SendErrorMessage("You do not have access to this command.");
=======
						if (!args.Player.Group.HasPermission(Permissions.tpallothers))
						{
							args.Player.SendErrorMessage("你没有权限使用这个命令。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						var target = players2[0];
						foreach (var source in TShock.Players.Where(p => p != null && p != args.Player))
						{
<<<<<<< HEAD
							if (!target.TPAllow && !args.Player.HasPermission(Permissions.tpoverride))
=======
							if (!target.TPAllow && !args.Player.Group.HasPermission(Permissions.tpoverride))
>>>>>>> e2683b6... 手动加入RYH修改的文件
								continue;
							if (source.Teleport(target.TPlayer.position.X, target.TPlayer.position.Y))
							{
								if (args.Player != source)
								{
<<<<<<< HEAD
									if (args.Player.HasPermission(Permissions.tpsilent))
										source.SendSuccessMessage("You were teleported to {0}.", target.Name);
									else
										source.SendSuccessMessage("{0} teleported you to {1}.", args.Player.Name, target.Name);
								}
								if (args.Player != target)
								{
									if (args.Player.HasPermission(Permissions.tpsilent))
										target.SendInfoMessage("{0} was teleported to you.", source.Name);
									if (!args.Player.HasPermission(Permissions.tpsilent))
										target.SendInfoMessage("{0} teleported {1} to you.", args.Player.Name, source.Name);
								}
							}
						}
						args.Player.SendSuccessMessage("Teleported everyone to {0}.", target.Name);
					}
					else
						args.Player.SendErrorMessage("Invalid player!");
=======
									if (args.Player.Group.HasPermission(Permissions.tpsilent))
										source.SendSuccessMessage("你被传送到 {0}。", target.Name);
									else
										source.SendSuccessMessage("{0} 将你传送到 {1}。", args.Player.Name, target.Name);
								}
								if (args.Player != target)
								{
									if (args.Player.Group.HasPermission(Permissions.tpsilent))
										target.SendInfoMessage("{0} 被传送到你。", source.Name);
									if (!args.Player.Group.HasPermission(Permissions.tpsilent))
										target.SendInfoMessage("{0} 传送 {1} 到你。", args.Player.Name, source.Name);
								}
							}
						}
						args.Player.SendSuccessMessage("将所有人传送到{0}。", target.Name);
					}
					else
						args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
				else if (players1.Count > 1)
					TShock.Utils.SendMultipleMatchError(args.Player, players1.Select(p => p.Name));
				else
				{
					var source = players1[0];
<<<<<<< HEAD
					if (!source.TPAllow && !args.Player.HasPermission(Permissions.tpoverride))
					{
						args.Player.SendErrorMessage("{0} has disabled players from teleporting.", source.Name);
						return;
					}
					var target = players2[0];
					if (!target.TPAllow && !args.Player.HasPermission(Permissions.tpoverride))
					{
						args.Player.SendErrorMessage("{0} has disabled players from teleporting.", target.Name);
						return;
					}
					args.Player.SendSuccessMessage("Teleported {0} to {1}.", source.Name, target.Name);
=======
					if (!source.TPAllow && !args.Player.Group.HasPermission(Permissions.tpoverride))
					{
						args.Player.SendErrorMessage("{0} 禁止别人传送。", source.Name);
						return;
					}
					var target = players2[0];
					if (!target.TPAllow && !args.Player.Group.HasPermission(Permissions.tpoverride))
					{
						args.Player.SendErrorMessage("{0} 禁止别人传送。", target.Name);
						return;
					}
					args.Player.SendSuccessMessage("传送 {0} 到 {1}。", source.Name, target.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					if (source.Teleport(target.TPlayer.position.X, target.TPlayer.position.Y))
					{
						if (args.Player != source)
						{
<<<<<<< HEAD
							if (args.Player.HasPermission(Permissions.tpsilent))
								source.SendSuccessMessage("You were teleported to {0}.", target.Name);
							else
								source.SendSuccessMessage("{0} teleported you to {1}.", args.Player.Name, target.Name);
						}
						if (args.Player != target)
						{
							if (args.Player.HasPermission(Permissions.tpsilent))
								target.SendInfoMessage("{0} was teleported to you.", source.Name);
							if (!args.Player.HasPermission(Permissions.tpsilent))
								target.SendInfoMessage("{0} teleported {1} to you.", args.Player.Name, source.Name);
=======
							if (args.Player.Group.HasPermission(Permissions.tpsilent))
								source.SendSuccessMessage("你被传送到 {0}。", target.Name);
							else
								source.SendSuccessMessage("{0} 将你传送到 {1}。", args.Player.Name, target.Name);
						}
						if (args.Player != target)
						{
							if (args.Player.Group.HasPermission(Permissions.tpsilent))
								target.SendInfoMessage("{0} 被传送到你。", source.Name);
							if (!args.Player.Group.HasPermission(Permissions.tpsilent))
								target.SendInfoMessage("{0} 传送 {1} 到你。", args.Player.Name, source.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
				}
			}
		}

		private static void TPHere(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				if (args.Player.HasPermission(Permissions.tpallothers))
					args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tphere <player|*>", Specifier);
				else
					args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tphere <player>", Specifier);
=======
				if (args.Player.Group.HasPermission(Permissions.tpallothers))
					args.Player.SendErrorMessage("格式错误。 格式: {0}拉人 <player|*>", Specifier);
				else
					args.Player.SendErrorMessage("格式错误。 格式: {0}拉人 <玩家名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			string playerName = String.Join(" ", args.Parameters);
			var players = TShock.Utils.FindPlayer(playerName);
			if (players.Count == 0)
			{
				if (playerName == "*")
				{
<<<<<<< HEAD
					if (!args.Player.HasPermission(Permissions.tpallothers))
					{
						args.Player.SendErrorMessage("You do not have permission to use this command.");
=======
					if (!args.Player.Group.HasPermission(Permissions.tpallothers))
					{
						args.Player.SendErrorMessage("没有权限。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						return;
					}
					for (int i = 0; i < Main.maxPlayers; i++)
					{
						if (Main.player[i].active && (Main.player[i] != args.TPlayer))
						{
							if (TShock.Players[i].Teleport(args.TPlayer.position.X, args.TPlayer.position.Y))
<<<<<<< HEAD
								TShock.Players[i].SendSuccessMessage(String.Format("You were teleported to {0}.", args.Player.Name));
						}
					}
					args.Player.SendSuccessMessage("Teleported everyone to yourself.");
				}
				else
					args.Player.SendErrorMessage("Invalid player!");
=======
								TShock.Players[i].SendSuccessMessage(String.Format("你被传送到{0}。", args.Player.Name));
						}
					}
					args.Player.SendSuccessMessage("将所有人传送过来。");
				}
				else
					args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (players.Count > 1)
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			else
			{
				var plr = players[0];
				if (plr.Teleport(args.TPlayer.position.X, args.TPlayer.position.Y))
				{
<<<<<<< HEAD
					plr.SendInfoMessage("You were teleported to {0}.", args.Player.Name);
					args.Player.SendSuccessMessage("Teleported {0} to yourself.", plr.Name);
=======
					plr.SendInfoMessage("你被传送到 {0}。", args.Player.Name);
					args.Player.SendSuccessMessage("将 {0} 传送来。", plr.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			}
		}

		private static void TPNpc(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tpnpc <NPC>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}去NPC <NPC>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			var npcStr = string.Join(" ", args.Parameters);
			var matches = new List<NPC>();
			foreach (var npc in Main.npc.Where(npc => npc.active))
			{
				if (string.Equals(npc.name, npcStr, StringComparison.CurrentCultureIgnoreCase))
				{
					matches = new List<NPC> { npc };
					break;
				}
				if (npc.name.ToLower().StartsWith(npcStr.ToLower()))
					matches.Add(npc);
			}

			if (matches.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, matches.Select(n => n.name));
				return;
			}
			if (matches.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid NPC!");
=======
				args.Player.SendErrorMessage("找不到此NPC。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			var target = matches[0];
			args.Player.Teleport(target.position.X, target.position.Y);
<<<<<<< HEAD
			args.Player.SendSuccessMessage("Teleported to the '{0}'.", target.name);
=======
			args.Player.SendSuccessMessage("传送到'{0}'。", target.name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void GetPos(CommandArgs args)
		{
			var player = args.Player.Name;
			if (args.Parameters.Count > 0)
			{
				player = String.Join(" ", args.Parameters);
			}

			var players = TShock.Utils.FindPlayer(player);
			if (players.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (players.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			}
			else
			{
<<<<<<< HEAD
				args.Player.SendSuccessMessage("Location of {0} is ({1}, {2}).", players[0].Name, players[0].TileX, players[0].TileY);
=======
				args.Player.SendSuccessMessage("{0} 的位置是 ({1}, {2})。", players[0].Name, players[0].TileX, players[0].TileY);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void TPPos(CommandArgs args)
		{
			if (args.Parameters.Count != 2)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tppos <tile x> <tile y>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}去坐标 <tile x> <tile y>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			int x, y;
			if (!int.TryParse(args.Parameters[0], out x) || !int.TryParse(args.Parameters[1], out y))
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid tile positions!");
=======
				args.Player.SendErrorMessage("位置错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			x = Math.Max(0, x);
			y = Math.Max(0, y);
			x = Math.Min(x, Main.maxTilesX - 1);
			y = Math.Min(y, Main.maxTilesY - 1);

			args.Player.Teleport(16 * x, 16 * y);
<<<<<<< HEAD
			args.Player.SendSuccessMessage("Teleported to {0}, {1}!", x, y);
=======
			args.Player.SendSuccessMessage("传送到 {0}, {1}。", x, y);
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void TPAllow(CommandArgs args)
		{
			if (!args.Player.TPAllow)
<<<<<<< HEAD
				args.Player.SendSuccessMessage("You have removed your teleportation protection.");
			if (args.Player.TPAllow)
                args.Player.SendSuccessMessage("You have enabled teleportation protection.");
=======
				args.Player.SendSuccessMessage("关闭传送保护。");
			if (args.Player.TPAllow)
                args.Player.SendSuccessMessage("开启传送保护。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			args.Player.TPAllow = !args.Player.TPAllow;
		}

		private static void Warp(CommandArgs args)
		{
<<<<<<< HEAD
		    bool hasManageWarpPermission = args.Player.HasPermission(Permissions.managewarp);
=======
		    bool hasManageWarpPermission = args.Player.Group.HasPermission(Permissions.managewarp);
>>>>>>> e2683b6... 手动加入RYH修改的文件
            if (args.Parameters.Count < 1)
            {
                if (hasManageWarpPermission)
                {
<<<<<<< HEAD
                    args.Player.SendInfoMessage("Invalid syntax! Proper syntax: {0}warp [command] [arguments]", Specifier);
                    args.Player.SendInfoMessage("Commands: add, del, hide, list, send, [warpname]");
                    args.Player.SendInfoMessage("Arguments: add [warp name], del [warp name], list [page]");
                    args.Player.SendInfoMessage("Arguments: send [player] [warp name], hide [warp name] [Enable(true/false)]");
                    args.Player.SendInfoMessage("Examples: {0}warp add foobar, {0}warp hide foobar true, {0}warp foobar", Specifier);
=======
                    args.Player.SendInfoMessage("格式错误。 格式: {0}跃迁 [子命令] [参数]", Specifier);
                    args.Player.SendInfoMessage("命令: add, del, hide, list, send, [跃迁点名]");
                    args.Player.SendInfoMessage("参数: add [跃迁点名], del [跃迁点名], list [页码]");
                    args.Player.SendInfoMessage("参数: send [玩家名] [跃迁点名], hide [跃迁点名] [Enable(true/false)]");
                    args.Player.SendInfoMessage("例如， {0}跃迁 add 跃迁点1, {0}warp hide 跃迁点1 true, {0}warp 跃迁点1", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
                    return;
                }
                else
                {
<<<<<<< HEAD
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}warp [name] or {0}warp list <page>", Specifier);
=======
                    args.Player.SendErrorMessage("格式错误。 格式: {0}跃迁 [name] or {0}warp list <page>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
                    return;
                }
            }

			if (args.Parameters[0].Equals("list"))
            {
                #region List warps
				int pageNumber;
				if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
					return;
				IEnumerable<string> warpNames = from warp in TShock.Warps.Warps
												where !warp.IsPrivate
												select warp.Name;
				PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(warpNames),
					new PaginationTools.Settings
					{
<<<<<<< HEAD
						HeaderFormat = "Warps ({0}/{1}):",
						FooterFormat = "Type {0}warp list {{0}} for more.".SFormat(Specifier),
						NothingToDisplayString = "There are currently no warps defined."
=======
						HeaderFormat = "跃迁点 ({0}/{1}):",
						FooterFormat = "输入 {0}跃迁 list {{0}} 查看更多。".SFormat(Specifier),
						NothingToDisplayString = "目前没有跃迁点。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
					});
                #endregion
            }
            else if (args.Parameters[0].ToLower() == "add" && hasManageWarpPermission)
            {
                #region Add warp
                if (args.Parameters.Count == 2)
                {
                    string warpName = args.Parameters[1];
                    if (warpName == "list" || warpName == "hide" || warpName == "del" || warpName == "add")
                    {
<<<<<<< HEAD
                        args.Player.SendErrorMessage("Name reserved, use a different name.");
                    }
                    else if (TShock.Warps.Add(args.Player.TileX, args.Player.TileY, warpName))
                    {
                        args.Player.SendSuccessMessage("Warp added: " + warpName);
                    }
                    else
                    {
                        args.Player.SendErrorMessage("Warp " + warpName + " already exists.");
                    }
                }
                else
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}warp add [name]", Specifier);
=======
                        args.Player.SendErrorMessage("名称保留，请换用另一个。。");
                    }
                    else if (TShock.Warps.Add(args.Player.TileX, args.Player.TileY, warpName))
                    {
                        args.Player.SendSuccessMessage("新建跃迁点: " + warpName);
                    }
                    else
                    {
                        args.Player.SendErrorMessage("Warp " + warpName + " 已经存在。");
                    }
                }
                else
                    args.Player.SendErrorMessage("格式错误。 格式: {0}跃迁 add [name]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
                #endregion
            }
            else if (args.Parameters[0].ToLower() == "del" && hasManageWarpPermission)
            {
                #region Del warp
                if (args.Parameters.Count == 2)
                {
                    string warpName = args.Parameters[1];
					if (TShock.Warps.Remove(warpName))
					{
<<<<<<< HEAD
						args.Player.SendSuccessMessage("Warp deleted: " + warpName);
					}
					else
						args.Player.SendErrorMessage("Could not find the specified warp.");
                }
                else
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}warp del [name]", Specifier);
=======
						args.Player.SendSuccessMessage("删除跃迁点: " + warpName);
					}
					else
						args.Player.SendErrorMessage("找不到该点。");
                }
                else
                    args.Player.SendErrorMessage("格式错误。 格式: {0}跃迁 del [name]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
                #endregion
            }
            else if (args.Parameters[0].ToLower() == "hide" && hasManageWarpPermission)
            {
                #region Hide warp
                if (args.Parameters.Count == 3)
                {
                    string warpName = args.Parameters[1];
                    bool state = false;
                    if (Boolean.TryParse(args.Parameters[2], out state))
                    {
                        if (TShock.Warps.Hide(args.Parameters[1], state))
                        {
                            if (state)
<<<<<<< HEAD
                                args.Player.SendSuccessMessage("Warp " + warpName + " is now private.");
                            else
                                args.Player.SendSuccessMessage("Warp " + warpName + " is now public.");
                        }
                        else
                            args.Player.SendErrorMessage("Could not find specified warp.");
                    }
                    else
                        args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}warp hide [name] <true/false>", Specifier);
                }
                else
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}warp hide [name] <true/false>", Specifier);
                #endregion
            }
            else if (args.Parameters[0].ToLower() == "send" && args.Player.HasPermission(Permissions.tpothers))
=======
                                args.Player.SendSuccessMessage("跃迁点 " + warpName + " 设为私人。");
                            else
                                args.Player.SendSuccessMessage("跃迁点 " + warpName + " 设为公开。");
                        }
                        else
                            args.Player.SendErrorMessage("找不到该点。");
                    }
                    else
                        args.Player.SendErrorMessage("格式错误。 格式: {0}跃迁 hide [name] <true/false>", Specifier);
                }
                else
                    args.Player.SendErrorMessage("格式错误。 格式: {0}跃迁 hide [name] <true/false>", Specifier);
                #endregion
            }
            else if (args.Parameters[0].ToLower() == "send" && args.Player.Group.HasPermission(Permissions.tpothers))
>>>>>>> e2683b6... 手动加入RYH修改的文件
            {
                #region Warp send
                if (args.Parameters.Count < 3)
                {
<<<<<<< HEAD
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}warp send [player] [warpname]", Specifier);
=======
                    args.Player.SendErrorMessage("格式错误。 格式: {0}跃迁 send [player] [warpname]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
                    return;
                }

                var foundplr = TShock.Utils.FindPlayer(args.Parameters[1]);
                if (foundplr.Count == 0)
                {
<<<<<<< HEAD
                    args.Player.SendErrorMessage("Invalid player!");
=======
                    args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
                    return;
                }
                else if (foundplr.Count > 1)
                {
					TShock.Utils.SendMultipleMatchError(args.Player, foundplr.Select(p => p.Name));
                    return;
                }

                string warpName = args.Parameters[2];
                var warp = TShock.Warps.Find(warpName);
                var plr = foundplr[0];
				if (warp.Position != Point.Zero)
				{
					if (plr.Teleport(warp.Position.X * 16, warp.Position.Y * 16))
					{
<<<<<<< HEAD
						plr.SendSuccessMessage(String.Format("{0} warped you to {1}.", args.Player.Name, warpName));
						args.Player.SendSuccessMessage(String.Format("You warped {0} to {1}.", plr.Name, warpName));
=======
						plr.SendSuccessMessage(String.Format("{0} 将你传送到点{1}。", args.Player.Name, warpName));
						args.Player.SendSuccessMessage(String.Format("你将{0}传送到点{1}。", plr.Name, warpName));
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
				}
				else
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Specified warp not found.");
=======
					args.Player.SendErrorMessage("找不到该点。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
                #endregion
            }
            else
            {
                string warpName = String.Join(" ", args.Parameters);
                var warp = TShock.Warps.Find(warpName);
                if (warp != null)
                {
					if (args.Player.Teleport(warp.Position.X * 16, warp.Position.Y * 16))
<<<<<<< HEAD
                        args.Player.SendSuccessMessage("Warped to " + warpName + ".");
                }
                else
                {
                    args.Player.SendErrorMessage("The specified warp was not found.");
=======
                        args.Player.SendSuccessMessage("跃迁到 " + warpName + "。");
                }
                else
                {
                    args.Player.SendErrorMessage("找不到该点。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
                }
            }
		}

		#endregion Teleport Commands

		#region Group Management

		private static void Group(CommandArgs args)
		{
			string subCmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();

			switch (subCmd)
			{
				case "add":
					#region Add group
					{
						if (args.Parameters.Count < 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}group add <group name> [permissions]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}组 add <组名> [permissions]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string groupName = args.Parameters[1];
						args.Parameters.RemoveRange(0, 2);
						string permissions = String.Join(",", args.Parameters);

						try
						{
							TShock.Groups.AddGroup(groupName, null, permissions, TShockAPI.Group.defaultChatColor);
<<<<<<< HEAD
							args.Player.SendSuccessMessage("The group was added successfully!");
						}
						catch (GroupExistsException)
						{
							args.Player.SendErrorMessage("That group already exists!");
=======
							args.Player.SendSuccessMessage("添加组成功。");
						}
						catch (GroupExistsException)
						{
							args.Player.SendErrorMessage("That group 已经存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
						catch (GroupManagerException ex)
						{
							args.Player.SendErrorMessage(ex.ToString());
						}
					}
					#endregion
					return;
				case "addperm":
					#region Add permissions
					{
						if (args.Parameters.Count < 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}group addperm <group name> <permissions...>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}组 addperm <组名> <权限>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string groupName = args.Parameters[1];
						args.Parameters.RemoveRange(0, 2);
						if (groupName == "*")
						{
							foreach (Group g in TShock.Groups)
							{
								TShock.Groups.AddPermissions(g.Name, args.Parameters);
							}
<<<<<<< HEAD
							args.Player.SendSuccessMessage("Modified all groups.");
=======
							args.Player.SendSuccessMessage("修改全部组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}
						try
						{
							string response = TShock.Groups.AddPermissions(groupName, args.Parameters);
							if (response.Length > 0)
							{
								args.Player.SendSuccessMessage(response);
							}
							return;
						}
						catch (GroupManagerException ex)
						{
							args.Player.SendErrorMessage(ex.ToString());
						}
					}
					#endregion
					return;
				case "help":
					#region Help
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						var lines = new List<string>
						{
<<<<<<< HEAD
							"add <name> <permissions...> - Adds a new group.",
							"addperm <group> <permissions...> - Adds permissions to a group.",
							"color <group> <rrr,ggg,bbb> - Changes a group's chat color.",
							"del <group> - Deletes a group.",
							"delperm <group> <permissions...> - Removes permissions from a group.",
							"list [page] - Lists groups.",
							"listperm <group> [page] - Lists a group's permissions.",
							"parent <group> <parent group> - Changes a group's parent group.",
							"prefix <group> <prefix> - Changes a group's prefix.",
							"suffix <group> <suffix> - Changes a group's suffix."
=======
							"add <名称> <权限> - 添加一个组。",
							"addperm <组> <权限> - 给用户组添加权限。",
							"color <组> <红,绿,蓝> - 更改一个组的聊天颜色。",
							"del <组> - 删除用户组。",
							"delperm <组> <权限> - 删除用户组权限。",
							"list [页码] - 列出所有组。",
							"listperm <组> [页码] - 列出组的权限。",
							"parent <组> <父组> - 更改用户组的父组。",
							"prefix <组> <前缀> - 更改用户组的前缀。",
							"suffix <组> <后缀> - 更改用户组的后缀。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
                        };

						PaginationTools.SendPage(args.Player, pageNumber, lines,
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Group Sub-Commands ({0}/{1}):",
								FooterFormat = "Type {0}group help {{0}} for more sub-commands.".SFormat(Specifier)
=======
								HeaderFormat = "组 子命令 ({0}/{1}):",
								FooterFormat = "输入 {0}组 help {{0}} 查看子命令。".SFormat(Specifier)
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						);
					}
					#endregion
					return;
				case "parent":
					#region Parent
					{
						if (args.Parameters.Count < 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}group parent <group name> [new parent group name]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}组 parent <组名> [new parent group name]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string groupName = args.Parameters[1];
						Group group = TShock.Groups.GetGroupByName(groupName);
						if (group == null)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("No such group \"{0}\".", groupName);
=======
							args.Player.SendErrorMessage("没有找到组 \"{0}\"。", groupName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						if (args.Parameters.Count > 2)
						{
							string newParentGroupName = string.Join(" ", args.Parameters.Skip(2));
							if (!string.IsNullOrWhiteSpace(newParentGroupName) && !TShock.Groups.GroupExists(newParentGroupName))
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("No such group \"{0}\".", newParentGroupName);
=======
								args.Player.SendErrorMessage("没有找到组 \"{0}\"。", newParentGroupName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}

							try
							{
								TShock.Groups.UpdateGroup(groupName, newParentGroupName, group.Permissions, group.ChatColor, group.Suffix, group.Prefix);

								if (!string.IsNullOrWhiteSpace(newParentGroupName))
<<<<<<< HEAD
									args.Player.SendSuccessMessage("Parent of group \"{0}\" set to \"{1}\".", groupName, newParentGroupName);
								else
									args.Player.SendSuccessMessage("Removed parent of group \"{0}\".", groupName);
=======
									args.Player.SendSuccessMessage("成功将 \"{0}\" 的父组设置为 \"{1}\"。", groupName, newParentGroupName);
								else
									args.Player.SendSuccessMessage("删除了 \"{0}\" 的父组。", groupName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
							catch (GroupManagerException ex)
							{
								args.Player.SendErrorMessage(ex.Message);
							}
						}
						else
						{
							if (group.Parent != null)
<<<<<<< HEAD
								args.Player.SendSuccessMessage("Parent of \"{0}\" is \"{1}\".", group.Name, group.Parent.Name);
							else
								args.Player.SendSuccessMessage("Group \"{0}\" has no parent.", group.Name);
=======
								args.Player.SendSuccessMessage("\"{0}\" 的父组是 \"{1}\"。", group.Name, group.Parent.Name);
							else
								args.Player.SendSuccessMessage(" \"{0}\" 没有父组。", group.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
					#endregion
					return;
				case "suffix":
					#region Suffix
					{
						if (args.Parameters.Count < 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}group suffix <group name> [new suffix]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}组 suffix <组名> [new suffix]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string groupName = args.Parameters[1];
						Group group = TShock.Groups.GetGroupByName(groupName);
						if (group == null)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("No such group \"{0}\".", groupName);
=======
							args.Player.SendErrorMessage("没有找到组 \"{0}\"。", groupName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						if (args.Parameters.Count > 2)
						{
							string newSuffix = string.Join(" ", args.Parameters.Skip(2));

							try
							{
								TShock.Groups.UpdateGroup(groupName, group.ParentName, group.Permissions, group.ChatColor, newSuffix, group.Prefix);

								if (!string.IsNullOrWhiteSpace(newSuffix))
<<<<<<< HEAD
									args.Player.SendSuccessMessage("Suffix of group \"{0}\" set to \"{1}\".", groupName, newSuffix);
								else
									args.Player.SendSuccessMessage("Removed suffix of group \"{0}\".", groupName);
=======
									args.Player.SendSuccessMessage("成功将 \"{0}\" 的前缀设置为 \"{1}\"。", groupName, newSuffix);
								else
									args.Player.SendSuccessMessage("删除了 \"{0}\" 的后缀。", groupName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
							catch (GroupManagerException ex)
							{
								args.Player.SendErrorMessage(ex.Message);
							}
						}
						else
						{
							if (!string.IsNullOrWhiteSpace(group.Suffix))
<<<<<<< HEAD
								args.Player.SendSuccessMessage("Suffix of \"{0}\" is \"{1}\".", group.Name, group.Suffix);
							else
								args.Player.SendSuccessMessage("Group \"{0}\" has no suffix.", group.Name);
=======
								args.Player.SendSuccessMessage("\"{0}\" 的后缀是 \"{1}\"。", group.Name, group.Suffix);
							else
								args.Player.SendSuccessMessage(" \"{0}\" 没有后缀。", group.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
					#endregion
					return;
				case "prefix":
					#region Prefix
					{
						if (args.Parameters.Count < 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}group prefix <group name> [new prefix]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}组 prefix <组名> [new prefix]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string groupName = args.Parameters[1];
						Group group = TShock.Groups.GetGroupByName(groupName);
						if (group == null)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("No such group \"{0}\".", groupName);
=======
							args.Player.SendErrorMessage("没有找到组 \"{0}\"。", groupName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						if (args.Parameters.Count > 2)
						{
							string newPrefix = string.Join(" ", args.Parameters.Skip(2));

							try
							{
								TShock.Groups.UpdateGroup(groupName, group.ParentName, group.Permissions, group.ChatColor, group.Suffix, newPrefix);

								if (!string.IsNullOrWhiteSpace(newPrefix))
<<<<<<< HEAD
									args.Player.SendSuccessMessage("Prefix of group \"{0}\" set to \"{1}\".", groupName, newPrefix);
								else
									args.Player.SendSuccessMessage("Removed prefix of group \"{0}\".", groupName);
=======
									args.Player.SendSuccessMessage("成功将 \"{0}\" 的前缀设置为 \"{1}\"。", groupName, newPrefix);
								else
									args.Player.SendSuccessMessage("删除了 \"{0}\" 的前缀。", groupName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
							catch (GroupManagerException ex)
							{
								args.Player.SendErrorMessage(ex.Message);
							}
						}
						else
						{
							if (!string.IsNullOrWhiteSpace(group.Prefix))
<<<<<<< HEAD
								args.Player.SendSuccessMessage("Prefix of \"{0}\" is \"{1}\".", group.Name, group.Prefix);
							else
								args.Player.SendSuccessMessage("Group \"{0}\" has no prefix.", group.Name);
=======
								args.Player.SendSuccessMessage("\"{0}\" 的前缀是 \"{1}\"。", group.Name, group.Prefix);
							else
								args.Player.SendSuccessMessage(" \"{0}\" 没有前缀。", group.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
					#endregion
					return;
				case "color":
					#region Color
					{
						if (args.Parameters.Count < 2 || args.Parameters.Count > 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}group color <group name> [new color(000,000,000)]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}组 color <组名> [new color(000,000,000)]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string groupName = args.Parameters[1];
						Group group = TShock.Groups.GetGroupByName(groupName);
						if (group == null)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("No such group \"{0}\".", groupName);
=======
							args.Player.SendErrorMessage("没有找到组 \"{0}\"。", groupName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						if (args.Parameters.Count == 3)
						{
							string newColor = args.Parameters[2];

							String[] parts = newColor.Split(',');
							byte r;
							byte g;
							byte b;
							if (parts.Length == 3 && byte.TryParse(parts[0], out r) && byte.TryParse(parts[1], out g) && byte.TryParse(parts[2], out b))
							{
								try
								{
									TShock.Groups.UpdateGroup(groupName, group.ParentName, group.Permissions, newColor, group.Suffix, group.Prefix);

<<<<<<< HEAD
									args.Player.SendSuccessMessage("Color of group \"{0}\" set to \"{1}\".", groupName, newColor);
=======
									args.Player.SendSuccessMessage("成功将 \"{0}\" 的颜色设置为\"{1}\"。", groupName, newColor);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								}
								catch (GroupManagerException ex)
								{
									args.Player.SendErrorMessage(ex.Message);
								}
							}
							else
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid syntax for color, expected \"rrr,ggg,bbb\"");
=======
								args.Player.SendErrorMessage("颜色格式错误。标准：\"红,绿,蓝\"（都是数字）");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						}
						else
						{
<<<<<<< HEAD
							args.Player.SendSuccessMessage("Color of \"{0}\" is \"{1}\".", group.Name, group.ChatColor);
=======
							args.Player.SendSuccessMessage("\"{0}\" 的颜色是 \"{1}\"。", group.Name, group.ChatColor);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
					#endregion
					return;
				case "del":
					#region Delete group
					{
						if (args.Parameters.Count != 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}group del <group name>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}组 del <组名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						try
						{
							string response = TShock.Groups.DeleteGroup(args.Parameters[1]);
							if (response.Length > 0)
							{
								args.Player.SendSuccessMessage(response);
							}
						}
						catch (GroupManagerException ex)
						{
							args.Player.SendErrorMessage(ex.ToString());
						}
					}
					#endregion
					return;
				case "delperm":
					#region Delete permissions
					{
						if (args.Parameters.Count < 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}group delperm <group name> <permissions...>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}组 delperm <组名> <权限>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						string groupName = args.Parameters[1];
						args.Parameters.RemoveRange(0, 2);
						if (groupName == "*")
						{
							foreach (Group g in TShock.Groups)
							{
								TShock.Groups.DeletePermissions(g.Name, args.Parameters);
							}
<<<<<<< HEAD
							args.Player.SendSuccessMessage("Modified all groups.");
=======
							args.Player.SendSuccessMessage("修改全部组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}
						try
						{
							string response = TShock.Groups.DeletePermissions(groupName, args.Parameters);
							if (response.Length > 0)
							{
								args.Player.SendSuccessMessage(response);
							}
							return;
						}
						catch (GroupManagerException ex)
						{
							args.Player.SendErrorMessage(ex.ToString());
						}
					}
					#endregion
					return;
				case "list":
					#region List groups
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;
						var groupNames = from grp in TShock.Groups.groups
										 select grp.Name;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(groupNames),
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Groups ({0}/{1}):",
								FooterFormat = "Type {0}group list {{0}} for more.".SFormat(Specifier)
=======
								HeaderFormat = "组 ({0}/{1}):",
								FooterFormat = "输入 {0}组 list {{0}} 查看更多。".SFormat(Specifier)
>>>>>>> e2683b6... 手动加入RYH修改的文件
							});
					}
					#endregion
					return;
				case "listperm":
					#region List permissions
					{
						if (args.Parameters.Count == 1)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}group listperm <group name> [page]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}组 listperm <组名> [页码]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 2, args.Player, out pageNumber))
							return;

						if (!TShock.Groups.GroupExists(args.Parameters[1]))
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid group.");
=======
							args.Player.SendErrorMessage("没有这个组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}
						Group grp = TShock.Utils.GetGroup(args.Parameters[1]);
						List<string> permissions = grp.TotalPermissions;

						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(permissions),
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Permissions for " + grp.Name + " ({0}/{1}):",
								FooterFormat = "Type {0}group listperm {1} {{0}} for more.".SFormat(Specifier, grp.Name),
								NothingToDisplayString = "There are currently no permissions for " + grp.Name + "."
=======
								HeaderFormat = grp.Name + " 的权限 ({0}/{1}):",
								FooterFormat = "输入 {0}组 listperm {1} {{0}} 查看更多。".SFormat(Specifier, grp.Name),
								NothingToDisplayString = grp.Name + " 没有权限。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
							});
					}
					#endregion
					return;
			}
		}
		#endregion Group Management

		#region Item Management

		private static void ItemBan(CommandArgs args)
		{
			string subCmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();
			switch (subCmd)
			{
				case "add":
					#region Add item
					{
						if (args.Parameters.Count != 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}itemban add <item name>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封物品 add <物品名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[1]);
						if (items.Count == 0)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid item.");
=======
							args.Player.SendErrorMessage("没有这个物品。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
						else if (items.Count > 1)
						{
							TShock.Utils.SendMultipleMatchError(args.Player, items.Select(i => i.name));
						}
						else
						{
							TShock.Itembans.AddNewBan(items[0].name);
<<<<<<< HEAD
							args.Player.SendSuccessMessage("Banned " + items[0].name + ".");
=======
							args.Player.SendSuccessMessage("封禁 " + items[0].name + "。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
					#endregion
					return;
				case "allow":
					#region Allow group to item
					{
						if (args.Parameters.Count != 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}itemban allow <item name> <group name>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封物品 allow <物品名> <组名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[1]);
						if (items.Count == 0)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid item.");
=======
							args.Player.SendErrorMessage("没有这个物品。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
						else if (items.Count > 1)
						{
							TShock.Utils.SendMultipleMatchError(args.Player, items.Select(i => i.name));
						}
						else
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid group.");
=======
								args.Player.SendErrorMessage("没有这个组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}

							ItemBan ban = TShock.Itembans.GetItemBanByName(items[0].name);
							if (ban == null)
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("{0} is not banned.", items[0].name);
=======
								args.Player.SendErrorMessage("{0} 没有被封禁。", items[0].name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}
							if (!ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.Itembans.AllowGroup(items[0].name, args.Parameters[2]);
<<<<<<< HEAD
								args.Player.SendSuccessMessage("{0} has been allowed to use {1}.", args.Parameters[2], items[0].name);
							}
							else
							{
								args.Player.SendWarningMessage("{0} is already allowed to use {1}.", args.Parameters[2], items[0].name);
=======
								args.Player.SendSuccessMessage("{0} 被允许使用物品 {1}。", args.Parameters[2], items[0].name);
							}
							else
							{
								args.Player.SendWarningMessage("{0} 已被允许使用 {1}。", args.Parameters[2], items[0].name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						}
					}
					#endregion
					return;
				case "del":
					#region Delete item
					{
						if (args.Parameters.Count != 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}itemban del <item name>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封物品 del <物品名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[1]);
						if (items.Count == 0)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid item.");
=======
							args.Player.SendErrorMessage("没有这个物品。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
						else if (items.Count > 1)
						{
							TShock.Utils.SendMultipleMatchError(args.Player, items.Select(i => i.name));
						}
						else
						{
							TShock.Itembans.RemoveBan(items[0].name);
<<<<<<< HEAD
							args.Player.SendSuccessMessage("Unbanned " + items[0].name + ".");
=======
							args.Player.SendSuccessMessage("解除封禁 " + items[0].name + "。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
					#endregion
					return;
				case "disallow":
					#region Disllow group from item
					{
						if (args.Parameters.Count != 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}itemban disallow <item name> <group name>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封物品 disallow <物品名> <组名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[1]);
						if (items.Count == 0)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid item.");
=======
							args.Player.SendErrorMessage("没有这个物品。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
						else if (items.Count > 1)
						{
							TShock.Utils.SendMultipleMatchError(args.Player, items.Select(i => i.name));
						}
						else
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid group.");
=======
								args.Player.SendErrorMessage("没有这个组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}

							ItemBan ban = TShock.Itembans.GetItemBanByName(items[0].name);
							if (ban == null)
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("{0} is not banned.", items[0].name);
=======
								args.Player.SendErrorMessage("{0} 没有被封禁。", items[0].name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}
							if (ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.Itembans.RemoveGroup(items[0].name, args.Parameters[2]);
<<<<<<< HEAD
								args.Player.SendSuccessMessage("{0} has been disallowed to use {1}.", args.Parameters[2], items[0].name);
							}
							else
							{
								args.Player.SendWarningMessage("{0} is already disallowed to use {1}.", args.Parameters[2], items[0].name);
=======
								args.Player.SendSuccessMessage("{0} 被禁止使用物品 {1}。", args.Parameters[2], items[0].name);
							}
							else
							{
								args.Player.SendWarningMessage("{0} 已被禁止使用 {1}。", args.Parameters[2], items[0].name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						}
					}
					#endregion
					return;
				case "help":
					#region Help
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						var lines = new List<string>
						{
<<<<<<< HEAD
							"add <item> - Adds an item ban.",
							"allow <item> <group> - Allows a group to use an item.",
							"del <item> - Deletes an item ban.",
							"disallow <item> <group> - Disallows a group from using an item.",
							"list [page] - Lists all item bans."
=======
							"add <物品名> - 封禁一种物品。",
							"allow <物品名> <组> - 允许一个组使用特定物品。",
							"del <物品名> - 解除物品封禁。",
							"disallow <物品名> <组> - 禁止一个组使用特定物品。",
							"list [页码] - 列出所有物品封禁。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
                        };

						PaginationTools.SendPage(args.Player, pageNumber, lines,
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Item Ban Sub-Commands ({0}/{1}):",
								FooterFormat = "Type {0}itemban help {{0}} for more sub-commands.".SFormat(Specifier)
=======
								HeaderFormat = "物品封禁 子命令 ({0}/{1}):",
								FooterFormat = "输入 {0}封物品 help {{0}} 查看子命令。".SFormat(Specifier)
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						);
					}
					#endregion
					return;
				case "list":
					#region List items
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;
						IEnumerable<string> itemNames = from itemBan in TShock.Itembans.ItemBans
														select itemBan.Name;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(itemNames),
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Item bans ({0}/{1}):",
								FooterFormat = "Type {0}itemban list {{0}} for more.".SFormat(Specifier),
								NothingToDisplayString = "There are currently no banned items."
=======
								HeaderFormat = "物品封禁 ({0}/{1}):",
								FooterFormat = "输入 {0}封物品 list {{0}} 查看更多。".SFormat(Specifier),
								NothingToDisplayString = "目前没有物品封禁。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
							});
					}
					#endregion
					return;
			}
		}
		#endregion Item Management

		#region Projectile Management

		private static void ProjectileBan(CommandArgs args)
		{
			string subCmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();
			switch (subCmd)
			{
				case "add":
					#region Add projectile
					{
						if (args.Parameters.Count != 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}projban add <proj id>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封弹幕 add <proj id>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}
						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id > 0 && id < Main.maxProjectileTypes)
						{
							TShock.ProjectileBans.AddNewBan(id);
<<<<<<< HEAD
							args.Player.SendSuccessMessage("Banned projectile {0}.", id);
						}
						else
							args.Player.SendErrorMessage("Invalid projectile ID!");
=======
							args.Player.SendSuccessMessage("封禁了弹幕 {0}。", id);
						}
						else
							args.Player.SendErrorMessage("没有这个弹幕。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "allow":
					#region Allow group to projectile
					{
						if (args.Parameters.Count != 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}projban allow <id> <group>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封弹幕 allow <id> <组>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id > 0 && id < Main.maxProjectileTypes)
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid group.");
=======
								args.Player.SendErrorMessage("没有这个组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}

							ProjectileBan ban = TShock.ProjectileBans.GetBanById(id);
							if (ban == null)
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Projectile {0} is not banned.", id);
=======
								args.Player.SendErrorMessage("弹幕 {0} 没有被封禁。", id);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}
							if (!ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.ProjectileBans.AllowGroup(id, args.Parameters[2]);
<<<<<<< HEAD
								args.Player.SendSuccessMessage("{0} has been allowed to use projectile {1}.", args.Parameters[2], id);
							}
							else
								args.Player.SendWarningMessage("{0} is already allowed to use projectile {1}.", args.Parameters[2], id);
						}
						else
							args.Player.SendErrorMessage("Invalid projectile ID!");
=======
								args.Player.SendSuccessMessage("{0} 被允许使用弹幕 {1}。", args.Parameters[2], id);
							}
							else
								args.Player.SendWarningMessage("{0} 已被允许使用弹幕 {1}。", args.Parameters[2], id);
						}
						else
							args.Player.SendErrorMessage("没有这个弹幕。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "del":
					#region Delete projectile
					{
						if (args.Parameters.Count != 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}projban del <id>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封弹幕 del <id>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id > 0 && id < Main.maxProjectileTypes)
						{
							TShock.ProjectileBans.RemoveBan(id);
<<<<<<< HEAD
							args.Player.SendSuccessMessage("Unbanned projectile {0}.", id);
							return;
						}
						else
							args.Player.SendErrorMessage("Invalid projectile ID!");
=======
							args.Player.SendSuccessMessage("解除封禁弹幕 {0}。", id);
							return;
						}
						else
							args.Player.SendErrorMessage("没有这个弹幕。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "disallow":
					#region Disallow group from projectile
					{
						if (args.Parameters.Count != 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}projban disallow <id> <group name>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封弹幕 disallow <id> <组名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id > 0 && id < Main.maxProjectileTypes)
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid group.");
=======
								args.Player.SendErrorMessage("没有这个组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}

							ProjectileBan ban = TShock.ProjectileBans.GetBanById(id);
							if (ban == null)
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Projectile {0} is not banned.", id);
=======
								args.Player.SendErrorMessage("弹幕 {0} 没有被封禁。", id);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}
							if (ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.ProjectileBans.RemoveGroup(id, args.Parameters[2]);
<<<<<<< HEAD
								args.Player.SendSuccessMessage("{0} has been disallowed from using projectile {1}.", args.Parameters[2], id);
								return;
							}
							else
								args.Player.SendWarningMessage("{0} is already prevented from using projectile {1}.", args.Parameters[2], id);
						}
						else
							args.Player.SendErrorMessage("Invalid projectile ID!");
=======
								args.Player.SendSuccessMessage("{0} 被禁止使用弹幕 {1}。", args.Parameters[2], id);
								return;
							}
							else
								args.Player.SendWarningMessage("{0} 已被禁止使用弹幕 {1}。", args.Parameters[2], id);
						}
						else
							args.Player.SendErrorMessage("没有这个弹幕。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "help":
					#region Help
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						var lines = new List<string>
						{
<<<<<<< HEAD
							"add <projectile ID> - Adds a projectile ban.",
							"allow <projectile ID> <group> - Allows a group to use a projectile.",
							"del <projectile ID> - Deletes an projectile ban.",
							"disallow <projectile ID> <group> - Disallows a group from using a projectile.",
							"list [page] - Lists all projectile bans."
=======
							"add <ID> - 封禁一种弹幕。",
							"allow <ID> <组> - 允许一个组使用特定弹幕。",
							"del <ID> - 解除弹幕封禁。",
							"disallow <ID> <组> - 禁止一个组使用特定弹幕。",
							"list [页码] - 列出所有弹幕封禁。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
                        };

						PaginationTools.SendPage(args.Player, pageNumber, lines,
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Projectile Ban Sub-Commands ({0}/{1}):",
								FooterFormat = "Type {0}projban help {{0}} for more sub-commands.".SFormat(Specifier)
=======
								HeaderFormat = "弹幕封禁 子命令 ({0}/{1}):",
								FooterFormat = "输入 {0}封弹幕 help {{0}} 查看子命令。".SFormat(Specifier)
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						);
					}
					#endregion
					return;
				case "list":
					#region List projectiles
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;
						IEnumerable<Int16> projectileIds = from projectileBan in TShock.ProjectileBans.ProjectileBans
														   select projectileBan.ID;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(projectileIds),
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Projectile bans ({0}/{1}):",
								FooterFormat = "Type {0}projban list {{0}} for more.".SFormat(Specifier),
								NothingToDisplayString = "There are currently no banned projectiles."
=======
								HeaderFormat = "弹幕封禁 ({0}/{1}):",
								FooterFormat = "输入 {0}封弹幕 list {{0}} 查看更多。".SFormat(Specifier),
								NothingToDisplayString = "目前没有弹幕封禁。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
							});
					}
					#endregion
					return;
			}
		}
		#endregion Projectile Management

		#region Tile Management
		private static void TileBan(CommandArgs args)
		{
			string subCmd = args.Parameters.Count == 0 ? "help" : args.Parameters[0].ToLower();
			switch (subCmd)
			{
				case "add":
					#region Add tile
					{
						if (args.Parameters.Count != 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tileban add <tile id>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封方块 add <ID>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}
						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id >= 0 && id < Main.maxTileSets)
						{
							TShock.TileBans.AddNewBan(id);
<<<<<<< HEAD
							args.Player.SendSuccessMessage("Banned tile {0}.", id);
						}
						else
							args.Player.SendErrorMessage("Invalid tile ID!");
=======
							args.Player.SendSuccessMessage("封禁了方块 {0}。", id);
						}
						else
							args.Player.SendErrorMessage("ID错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "allow":
					#region Allow group to place tile
					{
						if (args.Parameters.Count != 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tileban allow <id> <group>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封方块 allow <id> <组>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id >= 0 && id < Main.maxTileSets)
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid group.");
=======
								args.Player.SendErrorMessage("没有这个组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}

							TileBan ban = TShock.TileBans.GetBanById(id);
							if (ban == null)
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Tile {0} is not banned.", id);
=======
								args.Player.SendErrorMessage("方块 {0} 没有被封禁。", id);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}
							if (!ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.TileBans.AllowGroup(id, args.Parameters[2]);
<<<<<<< HEAD
								args.Player.SendSuccessMessage("{0} has been allowed to place tile {1}.", args.Parameters[2], id);
							}
							else
								args.Player.SendWarningMessage("{0} is already allowed to place tile {1}.", args.Parameters[2], id);
						}
						else
							args.Player.SendErrorMessage("Invalid tile ID!");
=======
								args.Player.SendSuccessMessage("{0} 被允许使用方块 {1}。", args.Parameters[2], id);
							}
							else
								args.Player.SendWarningMessage("{0} 已被允许使用方块 {1}。", args.Parameters[2], id);
						}
						else
							args.Player.SendErrorMessage("ID错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "del":
					#region Delete tile ban
					{
						if (args.Parameters.Count != 2)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tileban del <id>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封方块 del <id>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id >= 0 && id < Main.maxTileSets)
						{
							TShock.TileBans.RemoveBan(id);
<<<<<<< HEAD
							args.Player.SendSuccessMessage("Unbanned tile {0}.", id);
							return;
						}
						else
							args.Player.SendErrorMessage("Invalid tile ID!");
=======
							args.Player.SendSuccessMessage("解除封禁方块 {0}。", id);
							return;
						}
						else
							args.Player.SendErrorMessage("ID错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "disallow":
					#region Disallow group from placing tile
					{
						if (args.Parameters.Count != 3)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}tileban disallow <id> <group name>", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}封方块 disallow <id> <组名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							return;
						}

						short id;
						if (Int16.TryParse(args.Parameters[1], out id) && id >= 0 && id < Main.maxTileSets)
						{
							if (!TShock.Groups.GroupExists(args.Parameters[2]))
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Invalid group.");
=======
								args.Player.SendErrorMessage("没有这个组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}

							TileBan ban = TShock.TileBans.GetBanById(id);
							if (ban == null)
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Tile {0} is not banned.", id);
=======
								args.Player.SendErrorMessage("方块 {0} 没有被封禁。", id);
>>>>>>> e2683b6... 手动加入RYH修改的文件
								return;
							}
							if (ban.AllowedGroups.Contains(args.Parameters[2]))
							{
								TShock.TileBans.RemoveGroup(id, args.Parameters[2]);
<<<<<<< HEAD
								args.Player.SendSuccessMessage("{0} has been disallowed from placing tile {1}.", args.Parameters[2], id);
								return;
							}
							else
								args.Player.SendWarningMessage("{0} is already prevented from placing tile {1}.", args.Parameters[2], id);
						}
						else
							args.Player.SendErrorMessage("Invalid tile ID!");
=======
								args.Player.SendSuccessMessage("{0} 被禁止使用方块 {1}。", args.Parameters[2], id);
								return;
							}
							else
								args.Player.SendWarningMessage("{0} 已被禁止使用方块 {1}。", args.Parameters[2], id);
						}
						else
							args.Player.SendErrorMessage("ID错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					#endregion
					return;
				case "help":
					#region Help
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						var lines = new List<string>
						{
<<<<<<< HEAD
							"add <tile ID> - Adds a tile ban.",
							"allow <tile ID> <group> - Allows a group to place a tile.",
							"del <tile ID> - Deletes a tile ban.",
							"disallow <tile ID> <group> - Disallows a group from place a tile.",
							"list [page] - Lists all tile bans."
=======
							"add <ID> - 封禁一种方块。",
							"allow <ID> <组> - 允许一个组使用特定方块。",
							"del <ID> - 解除方块封禁。",
							"disallow <ID> <组> - 禁止一个组使用特定方块。",
							"list [页码] - 列出所有方块封禁。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
                        };

						PaginationTools.SendPage(args.Player, pageNumber, lines,
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Tile Ban Sub-Commands ({0}/{1}):",
								FooterFormat = "Type {0}tileban help {{0}} for more sub-commands.".SFormat(Specifier)
=======
								HeaderFormat = "方块封禁 子命令 ({0}/{1}):",
								FooterFormat = "输入 {0}封方块 help {{0}} 查看子命令。".SFormat(Specifier)
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						);
					}
					#endregion
					return;
				case "list":
					#region List tile bans
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;
						IEnumerable<Int16> tileIds = from tileBan in TShock.TileBans.TileBans
														   select tileBan.ID;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(tileIds),
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Tile bans ({0}/{1}):",
								FooterFormat = "Type {0}tileban list {{0}} for more.".SFormat(Specifier),
								NothingToDisplayString = "There are currently no banned tiles."
=======
								HeaderFormat = "方块封禁 ({0}/{1}):",
								FooterFormat = "输入 {0}封方块 list {{0}} 查看更多。".SFormat(Specifier),
								NothingToDisplayString = "目前没有方块封禁。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
							});
					}
					#endregion
					return;
			}
		}
		#endregion Tile Management

		#region Server Config Commands

		private static void SetSpawn(CommandArgs args)
		{
			Main.spawnTileX = args.Player.TileX + 1;
			Main.spawnTileY = args.Player.TileY + 3;
			SaveManager.Instance.SaveWorld(false);
<<<<<<< HEAD
			args.Player.SendSuccessMessage("Spawn has now been set at your location.");
=======
			args.Player.SendSuccessMessage("复活点已设置。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void Reload(CommandArgs args)
		{
			TShock.Utils.Reload(args.Player);

			args.Player.SendSuccessMessage(
<<<<<<< HEAD
				"Configuration, permissions, and regions reload complete. Some changes may require a server restart.");
=======
				"配置文件、权限和领地已重载。部分设置需要重启服务器以生效。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void ServerPassword(CommandArgs args)
		{
			if (args.Parameters.Count != 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}serverpassword \"<new password>\"", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}服务器密码 \"<new password>\"", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			string passwd = args.Parameters[0];
			TShock.Config.ServerPassword = passwd;
<<<<<<< HEAD
			args.Player.SendSuccessMessage(string.Format("Server password has been changed to: {0}.", passwd));
=======
			args.Player.SendSuccessMessage(string.Format("设置服务器密码为{0}。", passwd));
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void Save(CommandArgs args)
		{
			SaveManager.Instance.SaveWorld(false);
			foreach (TSPlayer tsply in TShock.Players.Where(tsply => tsply != null))
			{
				tsply.SaveServerCharacter();
			}
<<<<<<< HEAD
			args.Player.SendSuccessMessage("Save succeeded.");
=======
			args.Player.SendSuccessMessage("保存成功。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void Settle(CommandArgs args)
		{
			if (Liquid.panicMode)
			{
<<<<<<< HEAD
				args.Player.SendWarningMessage("Liquids are already settling!");
				return;
			}
			Liquid.StartPanic();
			args.Player.SendInfoMessage("Settling liquids.");
=======
				args.Player.SendWarningMessage("已平衡液体。");
				return;
			}
			Liquid.StartPanic();
			args.Player.SendInfoMessage("液体平衡。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void MaxSpawns(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("Current maximum spawns: {0}", TShock.Config.DefaultMaximumSpawns);
=======
				args.Player.SendInfoMessage("当前最大刷怪 {0}", TShock.Config.DefaultMaximumSpawns);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			if (String.Equals(args.Parameters[0], "default", StringComparison.CurrentCultureIgnoreCase))
			{
				TShock.Config.DefaultMaximumSpawns = NPC.defaultMaxSpawns = 5;
				if (args.Silent) 
				{
<<<<<<< HEAD
					args.Player.SendInfoMessage("Changed the maximum spawns to 5.");
				}
				else {
					TSPlayer.All.SendInfoMessage("{0} changed the maximum spawns to 5.", args.Player.Name);
=======
					args.Player.SendInfoMessage("将最大刷怪设为 5。");
				}
				else {
					TSPlayer.All.SendInfoMessage("{0} 更改当前最大刷怪为 5。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
				return;
			}

			int maxSpawns = -1;
			if (!int.TryParse(args.Parameters[0], out maxSpawns) || maxSpawns < 0 || maxSpawns > Main.maxNPCs)
			{
<<<<<<< HEAD
				args.Player.SendWarningMessage("Invalid maximum spawns!  Acceptable range is {0} to {1}", 0, Main.maxNPCs);
=======
				args.Player.SendWarningMessage("最大刷怪错误。范围：0到{1}", 0, Main.maxNPCs);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			TShock.Config.DefaultMaximumSpawns = NPC.defaultMaxSpawns = maxSpawns;
			if (args.Silent)
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("Changed the maximum spawns to {0}.", maxSpawns);
			}
			else {
				TSPlayer.All.SendInfoMessage("{0} changed the maximum spawns to {1}.", args.Player.Name, maxSpawns);
=======
				args.Player.SendInfoMessage("将最大刷怪设为 {0}。", maxSpawns);
			}
			else {
				TSPlayer.All.SendInfoMessage("{0} 更改当前最大刷怪为o {1}。", args.Player.Name, maxSpawns);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void SpawnRate(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("Current spawn rate: {0}", TShock.Config.DefaultSpawnRate);
=======
				args.Player.SendInfoMessage("当前刷怪率 {0}", TShock.Config.DefaultSpawnRate);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			if (String.Equals(args.Parameters[0], "default", StringComparison.CurrentCultureIgnoreCase))
			{
				TShock.Config.DefaultSpawnRate = NPC.defaultSpawnRate = 600;
				if (args.Silent) 
				{
<<<<<<< HEAD
					args.Player.SendInfoMessage("Changed the spawn rate to 600.");
				}
				else {
					TSPlayer.All.SendInfoMessage("{0} changed the spawn rate to 600.", args.Player.Name);
=======
					args.Player.SendInfoMessage("将刷怪率设为 600。");
				}
				else {
					TSPlayer.All.SendInfoMessage("{0} 更改当前刷怪率为 600。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
				return;
			}

			int spawnRate = -1;
			if (!int.TryParse(args.Parameters[0], out spawnRate) || spawnRate < 0)
			{
<<<<<<< HEAD
				args.Player.SendWarningMessage("Invalid spawn rate!");
=======
				args.Player.SendWarningMessage("刷怪率错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			TShock.Config.DefaultSpawnRate = NPC.defaultSpawnRate = spawnRate;
			if (args.Silent) 
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("Changed the spawn rate to {0}.", spawnRate);
			}
			else {
				TSPlayer.All.SendInfoMessage("{0} changed the spawn rate to {1}.", args.Player.Name, spawnRate);
=======
				args.Player.SendInfoMessage("将刷怪率设为 {0}。", spawnRate);
			}
			else {
				TSPlayer.All.SendInfoMessage("{0} 更改当前刷怪率为 {1}。", args.Player.Name, spawnRate);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		#endregion Server Config Commands

		#region Time/PvpFun Commands

		private static void Time(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
				double time = Main.time / 3600.0;
				time += 4.5;
				if (!Main.dayTime)
					time += 15.0;
				time = time % 24.0;
<<<<<<< HEAD
				args.Player.SendInfoMessage("The current time is {0}:{1:D2}.", (int)Math.Floor(time), (int)Math.Round((time % 1.0) * 60.0));
=======
				args.Player.SendInfoMessage("当前时间 {0}:{1:D2}。", (int)Math.Floor(time), (int)Math.Round((time % 1.0) * 60.0));
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			
			switch (args.Parameters[0].ToLower())
			{
				case "day":
					TSPlayer.Server.SetTime(true, 0.0);
<<<<<<< HEAD
					TSPlayer.All.SendInfoMessage("{0} set the time to 4:30.", args.Player.Name);
					break;
				case "night":
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.All.SendInfoMessage("{0} set the time to 19:30.", args.Player.Name);
					break;
				case "noon":
					TSPlayer.Server.SetTime(true, 27000.0);
					TSPlayer.All.SendInfoMessage("{0} set the time to 12:00.", args.Player.Name);
					break;
				case "midnight":
					TSPlayer.Server.SetTime(false, 16200.0);
					TSPlayer.All.SendInfoMessage("{0} set the time to 0:00.", args.Player.Name);
=======
					TSPlayer.All.SendInfoMessage("{0} 将时间调整到4:30。", args.Player.Name);
					break;
				case "night":
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.All.SendInfoMessage("{0} 将时间调整到19:30。", args.Player.Name);
					break;
				case "noon":
					TSPlayer.Server.SetTime(true, 27000.0);
					TSPlayer.All.SendInfoMessage("{0} 将时间调整到12:00。", args.Player.Name);
					break;
				case "midnight":
					TSPlayer.Server.SetTime(false, 16200.0);
					TSPlayer.All.SendInfoMessage("{0} 将时间调整到0:00。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					break;
				default:
					string[] array = args.Parameters[0].Split(':');
					if (array.Length != 2)
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("Invalid time string! Proper format: hh:mm, in 24-hour time.");
=======
						args.Player.SendErrorMessage("时间格式错误。 格式： 时:分, 24小时制。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						return;
					}

					int hours;
					int minutes;
					if (!int.TryParse(array[0], out hours) || hours < 0 || hours > 23
						|| !int.TryParse(array[1], out minutes) || minutes < 0 || minutes > 59)
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("Invalid time string! Proper format: hh:mm, in 24-hour time.");
=======
						args.Player.SendErrorMessage("时间格式错误。 格式： 时:分, 24小时制。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						return;
					}

					decimal time = hours + (minutes / 60.0m);
					time -= 4.50m;
					if (time < 0.00m)
						time += 24.00m;

					if (time >= 15.00m)
					{
						TSPlayer.Server.SetTime(false, (double)((time - 15.00m) * 3600.0m));
					}
					else
					{
						TSPlayer.Server.SetTime(true, (double)(time * 3600.0m));
					}
<<<<<<< HEAD
					TSPlayer.All.SendInfoMessage("{0} set the time to {1}:{2:D2}.", args.Player.Name, hours, minutes);
=======
					TSPlayer.All.SendInfoMessage("{0} 将时间调整到{1}:{2:D2}。", args.Player.Name, hours, minutes);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					break;
			}
		}

		private static void Rain(CommandArgs args)
		{
<<<<<<< HEAD
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}rain [slime] <stop/start>", Specifier);
				return;
			}

			int switchIndex = 0;
			if (args.Parameters.Count == 2 && args.Parameters[0].ToLowerInvariant() == "slime")
			{
				switchIndex = 1;
			}

			switch (args.Parameters[switchIndex].ToLower())
			{
				case "start":
					if (switchIndex == 1)
					{
						Main.StartSlimeRain(false);
						TSPlayer.All.SendData(PacketTypes.WorldInfo);
						TSPlayer.All.SendInfoMessage("{0} caused it to rain slime.", args.Player.Name);
					}
					else
					{
						Main.StartRain();
						TSPlayer.All.SendData(PacketTypes.WorldInfo);
						TSPlayer.All.SendInfoMessage("{0} caused it to rain.", args.Player.Name);
					}
					break;
				case "stop":
					if (switchIndex == 1)
					{
						Main.StopSlimeRain(false);
						TSPlayer.All.SendData(PacketTypes.WorldInfo);
						TSPlayer.All.SendInfoMessage("{0} ended the slimey downpour.", args.Player.Name);
					}
					else
					{
						Main.StopRain();
						TSPlayer.All.SendData(PacketTypes.WorldInfo);
						TSPlayer.All.SendInfoMessage("{0} ended the downpour.", args.Player.Name);
					}
					break;
				default:
					args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}rain [slime] <stop/start>", Specifier);
=======
			if (args.Parameters.Count != 1)
			{
				args.Player.SendErrorMessage("格式错误。 格式: {0}下雨 <stop/start>", Specifier);
				return;
			}

			switch (args.Parameters[0].ToLower())
			{
				case "start":
					Main.StartRain();
					TSPlayer.All.SendInfoMessage("{0} 召来了雨。", args.Player.Name);
					break;
				case "stop":
					Main.StopRain();
					TSPlayer.All.SendInfoMessage("{0} 结束了大雨。", args.Player.Name);
					break;
				default:
					args.Player.SendErrorMessage("格式错误。 格式: {0}下雨 <stop/start>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					break;

			}
		}

		private static void Slap(CommandArgs args)
		{
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}slap <player> [damage]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}扇人 <玩家名> [damage]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			if (args.Parameters[0].Length == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			string plStr = args.Parameters[0];
			var players = TShock.Utils.FindPlayer(plStr);
			if (players.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (players.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			}
			else
			{
				var plr = players[0];
				int damage = 5;
				if (args.Parameters.Count == 2)
				{
					int.TryParse(args.Parameters[1], out damage);
				}
<<<<<<< HEAD
				if (!args.Player.HasPermission(Permissions.kill))
=======
				if (!args.Player.Group.HasPermission(Permissions.kill))
>>>>>>> e2683b6... 手动加入RYH修改的文件
				{
					damage = TShock.Utils.Clamp(damage, 15, 0);
				}
				plr.DamagePlayer(damage);
<<<<<<< HEAD
				TSPlayer.All.SendInfoMessage("{0} slapped {1} for {2} damage.", args.Player.Name, plr.Name, damage);
				TShock.Log.Info("{0} slapped {1} for {2} damage.", args.Player.Name, plr.Name, damage);
=======
				TSPlayer.All.SendInfoMessage("{0} 扇了 {1} 一巴掌，造成了 {2} 点伤害。", args.Player.Name, plr.Name, damage);
				TShock.Log.Info("{0} 扇了 {1} ，造成了 {2} 点伤害。", args.Player.Name, plr.Name, damage);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Wind(CommandArgs args)
		{
			if (args.Parameters.Count != 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}wind <speed>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}风 <speed>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			float speed;
			if (!float.TryParse(args.Parameters[0], out speed))
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid wind speed!");
=======
				args.Player.SendErrorMessage("风速错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			Main.windSpeed = speed;
			Main.windSpeedSet = speed;
			Main.windSpeedSpeed = 0f;
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
<<<<<<< HEAD
			TSPlayer.All.SendInfoMessage("{0} changed the wind speed to {1}.", args.Player.Name, speed);
=======
			TSPlayer.All.SendInfoMessage("{0} 更改风速为 {1}。", args.Player.Name, speed);
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		#endregion Time/PvpFun Commands

        #region Region Commands

		private static void Region(CommandArgs args)
		{
			string cmd = "help";
			if (args.Parameters.Count > 0)
			{
				cmd = args.Parameters[0].ToLower();
			}
			switch (cmd)
			{
				case "name":
					{
						{
<<<<<<< HEAD
							args.Player.SendInfoMessage("Hit a block to get the name of the region");
=======
							args.Player.SendInfoMessage("敲一个方块获取领地名。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							args.Player.AwaitingName = true;
							args.Player.AwaitingNameParameters = args.Parameters.Skip(1).ToArray();
						}
						break;
					}
				case "set":
					{
						int choice = 0;
						if (args.Parameters.Count == 2 &&
							int.TryParse(args.Parameters[1], out choice) &&
							choice >= 1 && choice <= 2)
						{
<<<<<<< HEAD
							args.Player.SendInfoMessage("Hit a block to Set Point " + choice);
=======
							args.Player.SendInfoMessage("敲一个方块设置点 " + choice + "。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							args.Player.AwaitingTempPoint = choice;
						}
						else
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /region set <1/2>");
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 set <1/2>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
						break;
					}
				case "define":
					{
						if (args.Parameters.Count > 1)
						{
							if (!args.Player.TempPoints.Any(p => p == Point.Zero))
							{
								string regionName = String.Join(" ", args.Parameters.GetRange(1, args.Parameters.Count - 1));
								var x = Math.Min(args.Player.TempPoints[0].X, args.Player.TempPoints[1].X);
								var y = Math.Min(args.Player.TempPoints[0].Y, args.Player.TempPoints[1].Y);
								var width = Math.Abs(args.Player.TempPoints[0].X - args.Player.TempPoints[1].X);
								var height = Math.Abs(args.Player.TempPoints[0].Y - args.Player.TempPoints[1].Y);

								if (TShock.Regions.AddRegion(x, y, width, height, regionName, args.Player.User.Name,
															 Main.worldID.ToString()))
								{
									args.Player.TempPoints[0] = Point.Zero;
									args.Player.TempPoints[1] = Point.Zero;
<<<<<<< HEAD
									args.Player.SendInfoMessage("Set region " + regionName);
								}
								else
								{
									args.Player.SendErrorMessage("Region " + regionName + " already exists");
=======
									args.Player.SendInfoMessage("设置领地 " + regionName);
								}
								else
								{
									args.Player.SendErrorMessage("领地 " + regionName + " 已经存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
								}
							}
							else
							{
<<<<<<< HEAD
								args.Player.SendErrorMessage("Points not set up yet");
							}
						}
						else
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region define <name>", Specifier);
=======
								args.Player.SendErrorMessage("未设立点。");
							}
						}
						else
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 define <名称>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						break;
					}
				case "protect":
					{
						if (args.Parameters.Count == 3)
						{
							string regionName = args.Parameters[1];
							if (args.Parameters[2].ToLower() == "true")
							{
								if (TShock.Regions.SetRegionState(regionName, true))
<<<<<<< HEAD
									args.Player.SendInfoMessage("Protected region " + regionName);
								else
									args.Player.SendErrorMessage("Could not find specified region");
=======
									args.Player.SendInfoMessage("已保护领地 " + regionName);
								else
									args.Player.SendErrorMessage("找不到该领地。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
							else if (args.Parameters[2].ToLower() == "false")
							{
								if (TShock.Regions.SetRegionState(regionName, false))
<<<<<<< HEAD
									args.Player.SendInfoMessage("Unprotected region " + regionName);
								else
									args.Player.SendErrorMessage("Could not find specified region");
							}
							else
								args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region protect <name> <true/false>", Specifier);
						}
						else
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /region protect <name> <true/false>", Specifier);
=======
									args.Player.SendInfoMessage("取消领地保护 " + regionName);
								else
									args.Player.SendErrorMessage("找不到该领地。");
							}
							else
								args.Player.SendErrorMessage("格式错误。 格式: {0}领地 protect <名称> <true/false>", Specifier);
						}
						else
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 protect <领地名> <true/false>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						break;
					}
				case "delete":
					{
						if (args.Parameters.Count > 1)
						{
							string regionName = String.Join(" ", args.Parameters.GetRange(1, args.Parameters.Count - 1));
							if (TShock.Regions.DeleteRegion(regionName))
							{
<<<<<<< HEAD
								args.Player.SendInfoMessage("Deleted region \"{0}\".", regionName);
							}
							else
								args.Player.SendErrorMessage("Could not find the specified region!");
						}
						else
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region delete <name>", Specifier);
=======
								args.Player.SendInfoMessage("删除领地 \"{0}\"。", regionName);
							}
							else
								args.Player.SendErrorMessage("找不到该领地。");
						}
						else
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 delete <名称>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						break;
					}
				case "clear":
					{
						args.Player.TempPoints[0] = Point.Zero;
						args.Player.TempPoints[1] = Point.Zero;
<<<<<<< HEAD
						args.Player.SendInfoMessage("Cleared temporary points.");
=======
						args.Player.SendInfoMessage("清除临时点。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						args.Player.AwaitingTempPoint = 0;
						break;
					}
				case "allow":
					{
						if (args.Parameters.Count > 2)
						{
							string playerName = args.Parameters[1];
							string regionName = "";

							for (int i = 2; i < args.Parameters.Count; i++)
							{
								if (regionName == "")
								{
									regionName = args.Parameters[2];
								}
								else
								{
									regionName = regionName + " " + args.Parameters[i];
								}
							}
							if (TShock.Users.GetUserByName(playerName) != null)
							{
								if (TShock.Regions.AddNewUser(regionName, playerName))
								{
<<<<<<< HEAD
									args.Player.SendInfoMessage("Added user " + playerName + " to " + regionName);
								}
								else
									args.Player.SendErrorMessage("Region " + regionName + " not found");
							}
							else
							{
								args.Player.SendErrorMessage("Player " + playerName + " not found");
							}
						}
						else
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region allow <name> <region>", Specifier);
=======
									args.Player.SendInfoMessage("允许 " + playerName + " 玩家修改领地 " + regionName);
								}
								else
									args.Player.SendErrorMessage("领地 " + regionName + " 不存在。");
							}
							else
							{
								args.Player.SendErrorMessage("没有找到玩家 " + playerName + " 。");
							}
						}
						else
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 allow <名称> <领地>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						break;
					}
				case "remove":
					if (args.Parameters.Count > 2)
					{
						string playerName = args.Parameters[1];
						string regionName = "";

						for (int i = 2; i < args.Parameters.Count; i++)
						{
							if (regionName == "")
							{
								regionName = args.Parameters[2];
							}
							else
							{
								regionName = regionName + " " + args.Parameters[i];
							}
						}
						if (TShock.Users.GetUserByName(playerName) != null)
						{
							if (TShock.Regions.RemoveUser(regionName, playerName))
							{
<<<<<<< HEAD
								args.Player.SendInfoMessage("Removed user " + playerName + " from " + regionName);
							}
							else
								args.Player.SendErrorMessage("Region " + regionName + " not found");
						}
						else
						{
							args.Player.SendErrorMessage("Player " + playerName + " not found");
						}
					}
					else
						args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region remove <name> <region>", Specifier);
=======
								args.Player.SendInfoMessage("禁止 " + playerName + " 玩家修改领地 " + regionName);
							}
							else
								args.Player.SendErrorMessage("领地 " + regionName + " 不存在。");
						}
						else
						{
							args.Player.SendErrorMessage("没有找到玩家 " + playerName + " 。");
						}
					}
					else
						args.Player.SendErrorMessage("格式错误。 格式: {0}领地 remove <名称> <领地>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					break;
				case "allowg":
					{
						if (args.Parameters.Count > 2)
						{
							string group = args.Parameters[1];
							string regionName = "";

							for (int i = 2; i < args.Parameters.Count; i++)
							{
								if (regionName == "")
								{
									regionName = args.Parameters[2];
								}
								else
								{
									regionName = regionName + " " + args.Parameters[i];
								}
							}
							if (TShock.Groups.GroupExists(group))
							{
								if (TShock.Regions.AllowGroup(regionName, group))
								{
<<<<<<< HEAD
									args.Player.SendInfoMessage("Added group " + group + " to " + regionName);
								}
								else
									args.Player.SendErrorMessage("Region " + regionName + " not found");
							}
							else
							{
								args.Player.SendErrorMessage("Group " + group + " not found");
							}
						}
						else
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region allowg <group> <region>", Specifier);
=======
									args.Player.SendInfoMessage("允许 " + group + " 组修改领地 " + regionName);
								}
								else
									args.Player.SendErrorMessage("领地 " + regionName + " 不存在。");
							}
							else
							{
								args.Player.SendErrorMessage("用户组 " + group + " 不存在。");
							}
						}
						else
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 allowg <组> <领地>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						break;
					}
				case "removeg":
					if (args.Parameters.Count > 2)
					{
						string group = args.Parameters[1];
						string regionName = "";

						for (int i = 2; i < args.Parameters.Count; i++)
						{
							if (regionName == "")
							{
								regionName = args.Parameters[2];
							}
							else
							{
								regionName = regionName + " " + args.Parameters[i];
							}
						}
						if (TShock.Groups.GroupExists(group))
						{
							if (TShock.Regions.RemoveGroup(regionName, group))
							{
<<<<<<< HEAD
								args.Player.SendInfoMessage("Removed group " + group + " from " + regionName);
							}
							else
								args.Player.SendErrorMessage("Region " + regionName + " not found");
						}
						else
						{
							args.Player.SendErrorMessage("Group " + group + " not found");
						}
					}
					else
						args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region removeg <group> <region>", Specifier);
=======
								args.Player.SendInfoMessage("禁止 " + group + " 组修改领地 " + regionName);
							}
							else
								args.Player.SendErrorMessage("领地 " + regionName + " 不存在。");
						}
						else
						{
							args.Player.SendErrorMessage("用户组 " + group + " 不存在。");
						}
					}
					else
						args.Player.SendErrorMessage("格式错误。 格式: {0}领地 removeg <组> <领地>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					break;
				case "list":
					{
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out pageNumber))
							return;

						IEnumerable<string> regionNames = from region in TShock.Regions.Regions
														  where region.WorldID == Main.worldID.ToString()
														  select region.Name;
						PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(regionNames),
							new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = "Regions ({0}/{1}):",
								FooterFormat = "Type {0}region list {{0}} for more.".SFormat(Specifier),
								NothingToDisplayString = "There are currently no regions defined."
=======
								HeaderFormat = "领地 ({0}/{1}):",
								FooterFormat = "输入 {0}领地 list {{0}} 查看更多。".SFormat(Specifier),
								NothingToDisplayString = "目前没有领地。"
>>>>>>> e2683b6... 手动加入RYH修改的文件
							});
						break;
					}
				case "info":
					{
						if (args.Parameters.Count == 1 || args.Parameters.Count > 4)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region info <region> [-d] [page]", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 info <领地> [-d] [页码]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							break;
						}

						string regionName = args.Parameters[1];
						bool displayBoundaries = args.Parameters.Skip(2).Any(
							p => p.Equals("-d", StringComparison.InvariantCultureIgnoreCase)
						);

						Region region = TShock.Regions.GetRegionByName(regionName);
						if (region == null)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Region \"{0}\" does not exist.", regionName);
=======
							args.Player.SendErrorMessage("领地 \"{0}\" 不存在。", regionName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							break;
						}

						int pageNumberIndex = displayBoundaries ? 3 : 2;
						int pageNumber;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, pageNumberIndex, args.Player, out pageNumber))
							break;

						List<string> lines = new List<string>
                        {
<<<<<<< HEAD
                            string.Format("X: {0}; Y: {1}; W: {2}; H: {3}, Z: {4}", region.Area.X, region.Area.Y, region.Area.Width, region.Area.Height, region.Z),
                            string.Concat("Owner: ", region.Owner),
                            string.Concat("Protected: ", region.DisableBuild.ToString()),
=======
                            string.Format("X: {0}; Y: {1}; 宽: {2}; 高: {3}, 序: {4}", region.Area.X, region.Area.Y, region.Area.Width, region.Area.Height, region.Z),
                            string.Concat("拥有者: ", region.Owner),
                            string.Concat("被保护: ", region.DisableBuild.ToString()),
>>>>>>> e2683b6... 手动加入RYH修改的文件
                        };

						if (region.AllowedIDs.Count > 0)
						{
							IEnumerable<string> sharedUsersSelector = region.AllowedIDs.Select(userId =>
							{
								User user = TShock.Users.GetUserByID(userId);
								if (user != null)
									return user.Name;

								return string.Concat("{ID: ", userId, "}");
							});
							List<string> extraLines = PaginationTools.BuildLinesFromTerms(sharedUsersSelector.Distinct());
<<<<<<< HEAD
							extraLines[0] = "Shared with: " + extraLines[0];
=======
							extraLines[0] = "分享于 " + extraLines[0];
>>>>>>> e2683b6... 手动加入RYH修改的文件
							lines.AddRange(extraLines);
						}
						else
						{
<<<<<<< HEAD
							lines.Add("Region is not shared with any users.");
=======
							lines.Add("该领地没有分享给任何玩家。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}

						if (region.AllowedGroups.Count > 0)
						{
							List<string> extraLines = PaginationTools.BuildLinesFromTerms(region.AllowedGroups.Distinct());
<<<<<<< HEAD
							extraLines[0] = "Shared with groups: " + extraLines[0];
=======
							extraLines[0] = "分享于 " + extraLines[0];
>>>>>>> e2683b6... 手动加入RYH修改的文件
							lines.AddRange(extraLines);
						}
						else
						{
<<<<<<< HEAD
							lines.Add("Region is not shared with any groups.");
=======
							lines.Add("该领地没有分享给任何组。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}

						PaginationTools.SendPage(
							args.Player, pageNumber, lines, new PaginationTools.Settings
							{
<<<<<<< HEAD
								HeaderFormat = string.Format("Information About Region \"{0}\" ({{0}}/{{1}}):", region.Name),
								FooterFormat = string.Format("Type {0}region info {1} {{0}} for more information.", Specifier, regionName)
=======
								HeaderFormat = string.Format("关于领地 \"{0}\" 的信息 ({{0}}/{{1}}):", region.Name),
								FooterFormat = string.Format("输入 {0}领地 info {1} {{0}} 查看更多信息。", Specifier, regionName)
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
						);

						if (displayBoundaries)
						{
							Rectangle regionArea = region.Area;
							foreach (Point boundaryPoint in Utils.Instance.EnumerateRegionBoundaries(regionArea))
							{
								// Preferring dotted lines as those should easily be distinguishable from actual wires.
								if ((boundaryPoint.X + boundaryPoint.Y & 1) == 0)
								{
									// Could be improved by sending raw tile data to the client instead but not really 
									// worth the effort as chances are very low that overwriting the wire for a few 
									// nanoseconds will cause much trouble.
									Tile tile = Main.tile[boundaryPoint.X, boundaryPoint.Y];
									bool oldWireState = tile.wire();
									tile.wire(true);

									try
									{
										args.Player.SendTileSquare(boundaryPoint.X, boundaryPoint.Y, 1);
									}
									finally
									{
										tile.wire(oldWireState);
									}
								}
							}

							Timer boundaryHideTimer = null;
							boundaryHideTimer = new Timer((state) =>
							{
								foreach (Point boundaryPoint in Utils.Instance.EnumerateRegionBoundaries(regionArea))
									if ((boundaryPoint.X + boundaryPoint.Y & 1) == 0)
										args.Player.SendTileSquare(boundaryPoint.X, boundaryPoint.Y, 1);

								Debug.Assert(boundaryHideTimer != null);
								boundaryHideTimer.Dispose();
							},
								null, 5000, Timeout.Infinite
							);
						}

						break;
					}
				case "z":
					{
						if (args.Parameters.Count == 3)
						{
							string regionName = args.Parameters[1];
							int z = 0;
							if (int.TryParse(args.Parameters[2], out z))
							{
								if (TShock.Regions.SetZ(regionName, z))
<<<<<<< HEAD
									args.Player.SendInfoMessage("Region's z is now " + z);
								else
									args.Player.SendErrorMessage("Could not find specified region");
							}
							else
								args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region z <name> <#>", Specifier);
						}
						else
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region z <name> <#>", Specifier);
=======
									args.Player.SendInfoMessage("领地序 " + z);
								else
									args.Player.SendErrorMessage("找不到该领地。");
							}
							else
								args.Player.SendErrorMessage("格式错误。 格式: {0}领地 z <名称> <#>", Specifier);
						}
						else
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 z <名称> <#>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						break;
					}
				case "resize":
				case "expand":
					{
						if (args.Parameters.Count == 4)
						{
							int direction;
							switch (args.Parameters[2])
							{
								case "u":
								case "up":
									{
										direction = 0;
										break;
									}
								case "r":
								case "right":
									{
										direction = 1;
										break;
									}
								case "d":
								case "down":
									{
										direction = 2;
										break;
									}
								case "l":
								case "left":
									{
										direction = 3;
										break;
									}
								default:
									{
										direction = -1;
										break;
									}
							}
							int addAmount;
							int.TryParse(args.Parameters[3], out addAmount);
							if (TShock.Regions.ResizeRegion(args.Parameters[1], addAmount, direction))
							{
<<<<<<< HEAD
								args.Player.SendInfoMessage("Region Resized Successfully!");
								TShock.Regions.Reload();
							}
							else
								args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region resize <region> <u/d/l/r> <amount>", Specifier);
						}
						else
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region resize <region> <u/d/l/r> <amount>", Specifier);
=======
								args.Player.SendInfoMessage("重建成功。");
								TShock.Regions.Reload();
							}
							else
								args.Player.SendErrorMessage("格式错误。 格式: {0}领地 resize <领地> <u/d/l/r> <amount>", Specifier);
						}
						else
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 resize <领地> <u/d/l/r> <amount>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
						break;
					}
				case "tp":
					{
<<<<<<< HEAD
						if (!args.Player.HasPermission(Permissions.tp))
						{
							args.Player.SendErrorMessage("You don't have the necessary permission to do that.");
=======
						if (!args.Player.Group.HasPermission(Permissions.tp))
						{
							args.Player.SendErrorMessage("没有权限。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							break;
						}
						if (args.Parameters.Count <= 1)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}region tp <region>.", Specifier);
=======
							args.Player.SendErrorMessage("格式错误。 格式: {0}领地 tp <领地>。", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							break;
						}

						string regionName = string.Join(" ", args.Parameters.Skip(1));
						Region region = TShock.Regions.GetRegionByName(regionName);
						if (region == null)
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Region \"{0}\" does not exist.", regionName);
=======
							args.Player.SendErrorMessage("领地 \"{0}\" 不存在。", regionName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
							break;
						}

						args.Player.Teleport(region.Area.Center.X * 16, region.Area.Center.Y * 16);
						break;
					}
				case "help":
				default:
					{
						int pageNumber;
						int pageParamIndex = 0;
						if (args.Parameters.Count > 1)
							pageParamIndex = 1;
						if (!PaginationTools.TryParsePageNumber(args.Parameters, pageParamIndex, args.Player, out pageNumber))
							return;

						List<string> lines = new List<string> {
<<<<<<< HEAD
                          "set <1/2> - Sets the temporary region points.",
                          "clear - Clears the temporary region points.",
                          "define <name> - Defines the region with the given name.",
                          "delete <name> - Deletes the given region.",
                          "name [-u][-z][-p] - Shows the name of the region at the given point.",
                          "list - Lists all regions.",
                          "resize <region> <u/d/l/r> <amount> - Resizes a region.",
                          "allow <user> <region> - Allows a user to a region.",
                          "remove <user> <region> - Removes a user from a region.",
                          "allowg <group> <region> - Allows a user group to a region.",
                          "removeg <group> <region> - Removes a user group from a region.",
                          "info <region> [-d] - Displays several information about the given region.",
                          "protect <name> <true/false> - Sets whether the tiles inside the region are protected or not.",
                          "z <name> <#> - Sets the z-order of the region.",
                        };
						if (args.Player.HasPermission(Permissions.tp))
							lines.Add("tp <region> - Teleports you to the given region's center.");
=======
                          "set <1/2> - 设置临时点。",
                          "clear - 清空临时点。",
                          "define <名称> - 确认领地。",
                          "delete <名称> - 删除领地。",
                          "name [-u][-z][-p] - 显示给定点所在的领地名。",
                          "list - 列出所有领地。",
                          "resize <领地> <u/d/l/r> <数值> - 重设领地大小。",
                          "allow <用户名> <领地> - 允许一个用户修改特定领地。",
                          "remove <用户名> <领地> - 禁止一个用户修改特定领地。",
                          "allowg <组> <领地> - 允许一个组修改特定领地。",
                          "removeg <组> <领地> - 禁止一个组修改特定领地。",
                          "info <领地> [-d] - 显示给定领地的信息。",
                          "protect <名称> <true/false> - 设置领地保护。",
                          "z <name> <#> - Sets the z-order of the region.",
                        };
						if (args.Player.Group.HasPermission(Permissions.tp))
							lines.Add("tp <领地> - 把你传送到该领地的中心。");
>>>>>>> e2683b6... 手动加入RYH修改的文件

						PaginationTools.SendPage(
						  args.Player, pageNumber, lines,
						  new PaginationTools.Settings
						  {
<<<<<<< HEAD
							  HeaderFormat = "Available Region Sub-Commands ({0}/{1}):",
							  FooterFormat = "Type {0}region {{0}} for more sub-commands.".SFormat(Specifier)
=======
							  HeaderFormat = "领地 子命令({0}/{1}):",
							  FooterFormat = "输入 {0}领地 {{0}} 查看子命令。".SFormat(Specifier)
>>>>>>> e2683b6... 手动加入RYH修改的文件
						  }
						);
						break;
					}
			}
		}

        #endregion Region Commands

		#region World Protection Commands

		private static void ToggleAntiBuild(CommandArgs args)
		{
			TShock.Config.DisableBuild = !TShock.Config.DisableBuild;
<<<<<<< HEAD
			TSPlayer.All.SendSuccessMessage(string.Format("Anti-build is now {0}.", (TShock.Config.DisableBuild ? "on" : "off")));
=======
			TSPlayer.All.SendSuccessMessage(string.Format("全图保护已{0}。", (TShock.Config.DisableBuild ? "开启" : "关闭")));
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void ProtectSpawn(CommandArgs args)
		{
			TShock.Config.SpawnProtection = !TShock.Config.SpawnProtection;
<<<<<<< HEAD
			TSPlayer.All.SendSuccessMessage(string.Format("Spawn is now {0}.", (TShock.Config.SpawnProtection ? "protected" : "open")));
=======
			TSPlayer.All.SendSuccessMessage(string.Format("复活点保护已{0}。", (TShock.Config.SpawnProtection ? "开启" : "关闭")));
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		#endregion World Protection Commands

		#region General Commands

		private static void Help(CommandArgs args)
		{
			if (args.Parameters.Count > 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}help <command/page>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}帮助 <命令/页码>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			int pageNumber;
			if (args.Parameters.Count == 0 || int.TryParse(args.Parameters[0], out pageNumber))
			{
				if (!PaginationTools.TryParsePageNumber(args.Parameters, 0, args.Player, out pageNumber))
				{
					return;
				}

				IEnumerable<string> cmdNames = from cmd in ChatCommands
											   where cmd.CanRun(args.Player) && (cmd.Name != "auth" || TShock.AuthToken != 0)
											   select Specifier + cmd.Name;

				PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(cmdNames),
					new PaginationTools.Settings
					{
<<<<<<< HEAD
						HeaderFormat = "Commands ({0}/{1}):",
						FooterFormat = "Type {0}help {{0}} for more.".SFormat(Specifier)
=======
						HeaderFormat = "命令 ({0}/{1}):",
						FooterFormat = "输入 {0}帮助 {{0}} 查看更多。".SFormat(Specifier)
>>>>>>> e2683b6... 手动加入RYH修改的文件
					});
			}
			else
			{
				string commandName = args.Parameters[0].ToLower();
				if (commandName.StartsWith(Specifier))
				{
					commandName = commandName.Substring(1);
				}

				Command command = ChatCommands.Find(c => c.Names.Contains(commandName));
				if (command == null)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid command.");
=======
					args.Player.SendErrorMessage("没有这个命令。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				if (!command.CanRun(args.Player))
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("You do not have access to this command.");
					return;
				}

				args.Player.SendSuccessMessage("{0}{1} help: ", Specifier, command.Name);
=======
					args.Player.SendErrorMessage("你没有权限使用这个命令。");
					return;
				}

				args.Player.SendSuccessMessage("{0}{1} 帮助: ", Specifier, command.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
                if (command.HelpDesc == null)
                {
                    args.Player.SendInfoMessage(command.HelpText);
                    return;
                }
                foreach (string line in command.HelpDesc)
                {
                    args.Player.SendInfoMessage(line);
                }
			}
		}

		private static void GetVersion(CommandArgs args)
		{
<<<<<<< HEAD
			args.Player.SendInfoMessage("TShock: {0} ({1}).", TShock.VersionNum, TShock.VersionCodename);
=======
			args.Player.SendInfoMessage("TShock: {0} ({1}) 汉化版 Beta 1", TShock.VersionNum, TShock.VersionCodename);
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void ListConnectedPlayers(CommandArgs args)
		{
			bool invalidUsage = (args.Parameters.Count > 2);

			bool displayIdsRequested = false;
			int pageNumber = 1;
			if (!invalidUsage) 
			{
				foreach (string parameter in args.Parameters)
				{
					if (parameter.Equals("-i", StringComparison.InvariantCultureIgnoreCase))
					{
						displayIdsRequested = true;
						continue;
					}

					if (!int.TryParse(parameter, out pageNumber))
					{
						invalidUsage = true;
						break;
					}
				}
			}
			if (invalidUsage)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid usage, proper usage: {0}who [-i] [pagenumber]", Specifier);
				return;
			}
			if (displayIdsRequested && !args.Player.HasPermission(Permissions.seeids))
			{
				args.Player.SendErrorMessage("You don't have the required permission to list player ids.");
				return;
			}

			args.Player.SendSuccessMessage("Online Players ({0}/{1})", TShock.Utils.ActivePlayers(), TShock.Config.MaxSlots);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}在线 [-i] [pagenumber]", Specifier);
				return;
			}
			if (displayIdsRequested && !args.Player.Group.HasPermission(Permissions.seeids))
			{
				args.Player.SendErrorMessage("没有权限。");
				return;
			}

			args.Player.SendSuccessMessage("当前在线玩家 ({0}/{1})", TShock.Utils.ActivePlayers(), TShock.Config.MaxSlots);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			PaginationTools.SendPage(
				args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(TShock.Utils.GetPlayers(displayIdsRequested)), 
				new PaginationTools.Settings 
				{
					IncludeHeader = false,
<<<<<<< HEAD
					FooterFormat = string.Format("Type {0}who {1}{{0}} for more.", Specifier, displayIdsRequested ? "-i " : string.Empty)
=======
					FooterFormat = string.Format("输入 {0}在线 {1}{{0}} 查看更多。", Specifier, displayIdsRequested ? "-i " : string.Empty)
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			);
		}

		private static void AuthToken(CommandArgs args)
		{
			if (TShock.AuthToken == 0)
			{
<<<<<<< HEAD
				args.Player.SendWarningMessage("Auth is disabled. This incident has been logged.");
				TShock.Utils.ForceKick(args.Player, "Auth system is disabled.", true, true);
				TShock.Log.Warn("{0} attempted to use {1}auth even though it's disabled.", args.Player.IP, Specifier);
=======
				args.Player.SendWarningMessage("认证系统已关闭。你的行为已被记录。");
				TShock.Utils.ForceKick(args.Player, "认证系统已关闭。", true, true);
				TShock.Log.Warn("认证系统关闭后，{0}试图进行超级管理员认证。", args.Player.IP, Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			int givenCode = Convert.ToInt32(args.Parameters[0]);
			if (givenCode == TShock.AuthToken && args.Player.Group.Name != "superadmin")
			{
				try
				{
					args.Player.Group = TShock.Utils.GetGroup("superadmin");
<<<<<<< HEAD
					args.Player.SendInfoMessage("Superadmin has been temporarily given to you. It will be removed on logout.");
					args.Player.SendInfoMessage("Please use the following to create a permanent account for you.");
					args.Player.SendInfoMessage("{0}user add <username> <password> superadmin", Specifier);
					args.Player.SendInfoMessage("Creates: <username> with the password <password> as part of the superadmin group.");
					args.Player.SendInfoMessage("Please use {0}login <username> <password> after this process.", Specifier);
					args.Player.SendInfoMessage("If you understand, please {0}login <username> <password> now, and type {0}auth-verify.", Specifier);
=======
					args.Player.SendInfoMessage("你现在已经是临时超级管理员了。");
					args.Player.SendInfoMessage("请按照下面的指引成为永久超级管理员。");
					args.Player.SendInfoMessage("{0}用户 add <用户名> <密码> superadmin", Specifier);
					args.Player.SendInfoMessage("这个命令可以将<用户名>和<密码>设为永久的超级管理员。");
					args.Player.SendInfoMessage("添加用户后，输入{0}登入 <用户名> <密码>", Specifier);
					args.Player.SendInfoMessage("如果理解的话，请输入现在{0}登入 <用户名> <密码>，然后输入{0}关闭认证。", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
				catch (UserManagerException ex)
				{
					TShock.Log.ConsoleError(ex.ToString());
					args.Player.SendErrorMessage(ex.Message);
				}
				return;
			}

			if (args.Player.Group.Name == "superadmin")
			{
<<<<<<< HEAD
				args.Player.SendInfoMessage("Please disable the auth system! If you need help, consult the forums. https://tshock.co/");
				args.Player.SendInfoMessage("This account is superadmin, please do the following to finish your install:");
				args.Player.SendInfoMessage("Please use {0}login <username> <password> to login from now on.", Specifier);
				args.Player.SendInfoMessage("If you understand, please {0}login <username> <password> now, and type {0}auth-verify.", Specifier);
				return;
			}

			args.Player.SendErrorMessage("Incorrect auth code. This incident has been logged.");
			TShock.Log.Warn(args.Player.IP + " attempted to use an incorrect auth code.");
=======
				args.Player.SendInfoMessage("请关闭认证系统。如果需要帮助，请联系我们的论坛https://tshock.co/");
				args.Player.SendInfoMessage("你现在已经是超级管理员了。请按照下面的指引做。");
				args.Player.SendInfoMessage("添加用户后，输入{0}登入 <用户名> <密码>", Specifier);
				args.Player.SendInfoMessage("如果理解的话，请输入现在{0}登入 <用户名> <密码>，然后输入{0}关闭认证。", Specifier);
				return;
			}

			args.Player.SendErrorMessage("认证码错误。你的行为已被记录。");
			TShock.Log.Warn(args.Player.IP + " 试图使用错误认证码进行超级管理员认证。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void AuthVerify(CommandArgs args)
		{
			if (TShock.AuthToken == 0)
			{
<<<<<<< HEAD
				args.Player.SendWarningMessage("It appears that you have already turned off the auth token.");
				args.Player.SendWarningMessage("If this is a mistake, delete auth.lck.");
				return;
			}

			args.Player.SendSuccessMessage("Your new account has been verified, and the /auth system has been turned off.");
			args.Player.SendSuccessMessage("You can always use the /user command to manage players. Don't just delete the auth.lck.");
			args.Player.SendSuccessMessage("Share your server, talk with other admins, and more on our forums -- https://tshock.co/");
			args.Player.SendSuccessMessage("Thank you for using TShock for Terraria!");
=======
				args.Player.SendWarningMessage("你已经关闭了认证系统。");
				args.Player.SendWarningMessage("如果是误操作，请删除auth.lck。");
				return;
			}

			args.Player.SendSuccessMessage("你的账户已经设立，认证系统已关闭。");
			args.Player.SendSuccessMessage("你可以使用/用户 来管理玩家。");
			args.Player.SendSuccessMessage("TShock官方论坛：https://tshock.co/");
			args.Player.SendSuccessMessage("欢迎使用TShock汉化版。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			FileTools.CreateFile(Path.Combine(TShock.SavePath, "auth.lck"));
			File.Delete(Path.Combine(TShock.SavePath, "authcode.txt"));
			TShock.AuthToken = 0;
		}

		private static void ThirdPerson(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}me <text>", Specifier);
				return;
			}
			if (args.Player.mute)
				args.Player.SendErrorMessage("You are muted.");
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}卖萌 <文字>", Specifier);
				return;
			}
			if (args.Player.mute)
				args.Player.SendErrorMessage("你被禁言了。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			else
				TSPlayer.All.SendMessage(string.Format("*{0} {1}", args.Player.Name, String.Join(" ", args.Parameters)), 205, 133, 63);
		}

		private static void PartyChat(CommandArgs args)
		{
			if (args.Parameters.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}p <team chat text>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}队伍 <队内聊天>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			int playerTeam = args.Player.Team;

			if (args.Player.mute)
<<<<<<< HEAD
				args.Player.SendErrorMessage("You are muted.");
=======
				args.Player.SendErrorMessage("你被禁言了。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			else if (playerTeam != 0)
			{
				string msg = string.Format("<{0}> {1}", args.Player.Name, String.Join(" ", args.Parameters));
				foreach (TSPlayer player in TShock.Players)
				{
					if (player != null && player.Active && player.Team == playerTeam)
						player.SendMessage(msg, Main.teamColor[playerTeam].R, Main.teamColor[playerTeam].G, Main.teamColor[playerTeam].B);
				}
			}
			else
<<<<<<< HEAD
				args.Player.SendErrorMessage("You are not in a party!");
=======
				args.Player.SendErrorMessage("你不在队伍中。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void Mute(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}mute <player> [reason]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}禁言 <玩家名> [原因]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			var players = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (players.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (players.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			}
<<<<<<< HEAD
			else if (players[0].HasPermission(Permissions.mute))
			{
				args.Player.SendErrorMessage("You cannot mute this player.");
=======
			else if (players[0].Group.HasPermission(Permissions.mute))
			{
				args.Player.SendErrorMessage("你不能禁言这个玩家。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (players[0].mute)
			{
				var plr = players[0];
				plr.mute = false;
<<<<<<< HEAD
				TSPlayer.All.SendInfoMessage("{0} has been unmuted by {1}.", plr.Name, args.Player.Name);
			}
			else
			{
				string reason = "No reason specified.";
=======
				TSPlayer.All.SendInfoMessage("{0} 被 {1} 解除禁言。", plr.Name, args.Player.Name);
			}
			else
			{
				string reason = "没有说明原因。";
>>>>>>> e2683b6... 手动加入RYH修改的文件
				if (args.Parameters.Count > 1)
					reason = String.Join(" ", args.Parameters.ToArray(), 1, args.Parameters.Count - 1);
				var plr = players[0];
				plr.mute = true;
<<<<<<< HEAD
				TSPlayer.All.SendInfoMessage("{0} has been muted by {1} for {2}.", plr.Name, args.Player.Name, reason);
=======
				TSPlayer.All.SendInfoMessage("{0} 被 {1} 禁言，原因是 {2}。", plr.Name, args.Player.Name, reason);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Motd(CommandArgs args)
		{
			TShock.Utils.ShowFileToUser(args.Player, "motd.txt");
		}

		private static void Rules(CommandArgs args)
		{
			TShock.Utils.ShowFileToUser(args.Player, "rules.txt");
		}

		private static void Whisper(CommandArgs args)
		{
			if (args.Parameters.Count < 2)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}whisper <player> <text>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}私聊 <玩家名> <text>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			var players = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (players.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (players.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			}
			else if (args.Player.mute)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("You are muted.");
=======
				args.Player.SendErrorMessage("你被禁言了。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else
			{
				var plr = players[0];
				var msg = string.Join(" ", args.Parameters.ToArray(), 1, args.Parameters.Count - 1);
<<<<<<< HEAD
				plr.SendMessage(String.Format("<From {0}> {1}", args.Player.Name, msg), Color.MediumPurple);
				args.Player.SendMessage(String.Format("<To {0}> {1}", plr.Name, msg), Color.MediumPurple);
=======
				plr.SendMessage(String.Format("<来自 {0}的私聊> {1}", args.Player.Name, msg), Color.MediumPurple);
				args.Player.SendMessage(String.Format("<发往 {0}的私聊> {1}", plr.Name, msg), Color.MediumPurple);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				plr.LastWhisper = args.Player;
				args.Player.LastWhisper = plr;
			}
		}

		private static void Reply(CommandArgs args)
		{
			if (args.Player.mute)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("You are muted.");
=======
				args.Player.SendErrorMessage("你被禁言了。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (args.Player.LastWhisper != null)
			{
				var msg = string.Join(" ", args.Parameters);
<<<<<<< HEAD
				args.Player.LastWhisper.SendMessage(String.Format("<From {0}> {1}", args.Player.Name, msg), Color.MediumPurple);
				args.Player.SendMessage(String.Format("<To {0}> {1}", args.Player.LastWhisper.Name, msg), Color.MediumPurple);
			}
			else
			{
				args.Player.SendErrorMessage("You haven't previously received any whispers. Please use {0}whisper to whisper to other people.", Specifier);
=======
				args.Player.LastWhisper.SendMessage(String.Format("<来自 {0}的私聊> {1}", args.Player.Name, msg), Color.MediumPurple);
				args.Player.SendMessage(String.Format("<发往 {0}的私聊> {1}", args.Player.LastWhisper.Name, msg), Color.MediumPurple);
			}
			else
			{
				args.Player.SendErrorMessage("你没收到过私聊。输入{0}私聊 来和其他玩家私聊。", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Annoy(CommandArgs args)
		{
			if (args.Parameters.Count != 2)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}annoy <player> <seconds to annoy>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}骚扰 <玩家名> <seconds to annoy>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			int annoy = 5;
			int.TryParse(args.Parameters[1], out annoy);

			var players = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (players.Count == 0)
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			else if (players.Count > 1)
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			else
			{
				var ply = players[0];
<<<<<<< HEAD
				args.Player.SendSuccessMessage("Annoying " + ply.Name + " for " + annoy + " seconds.");
=======
				args.Player.SendSuccessMessage("骚扰 " + ply.Name + "  " + annoy + " 秒。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				(new Thread(ply.Whoopie)).Start(annoy);
			}
		}

		private static void Confuse(CommandArgs args)
		{
			if (args.Parameters.Count != 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}confuse <player>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}混乱 <玩家名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			var players = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (players.Count == 0)
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			else if (players.Count > 1)
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			else
			{
				var ply = players[0];
				ply.Confused = !ply.Confused;
<<<<<<< HEAD
				args.Player.SendSuccessMessage("{0} is {1} confused.", ply.Name, ply.Confused ? "now" : "no longer");
=======
				args.Player.SendSuccessMessage("{0} {1}被混乱。", ply.Name, ply.Confused ? "正" : "不再");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Rocket(CommandArgs args)
		{
			if (args.Parameters.Count != 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}rocket <player>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}火箭 <玩家名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			var players = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (players.Count == 0)
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			else if (players.Count > 1)
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			else
			{
				var ply = players[0];

				if (ply.IsLoggedIn && Main.ServerSideCharacter)
				{
					ply.TPlayer.velocity.Y = -50;
					TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", ply.Index);
<<<<<<< HEAD
					args.Player.SendSuccessMessage("Rocketed {0}.", ply.Name);
				}
				else
				{
					args.Player.SendErrorMessage("Failed to rocket player: Not logged in or not SSC mode.");
=======
					args.Player.SendSuccessMessage("火箭发射 {0}。", ply.Name);
				}
				else
				{
					args.Player.SendErrorMessage("无法发射火箭：该玩家未登录或服务器非强制开荒。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			}
		}

		private static void FireWork(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}firework <player> [red|green|blue|yellow]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}烟花 <玩家名> [red|green|blue|yellow]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			var players = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (players.Count == 0)
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			else if (players.Count > 1)
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			else
			{
				int type = 167;
				if (args.Parameters.Count > 1)
				{
					if (args.Parameters[1].ToLower() == "green")
						type = 168;
					else if (args.Parameters[1].ToLower() == "blue")
						type = 169;
					else if (args.Parameters[1].ToLower() == "yellow")
						type = 170;
				}
				var ply = players[0];
				int p = Projectile.NewProjectile(ply.TPlayer.position.X, ply.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
				Main.projectile[p].Kill();
<<<<<<< HEAD
				args.Player.SendSuccessMessage("Launched Firework on {0}.", ply.Name);
=======
				args.Player.SendSuccessMessage("发射火箭于 {0}。", ply.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Aliases(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}aliases <command or alias>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}同义 <命令>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			
			string givenCommandName = string.Join(" ", args.Parameters);
			if (string.IsNullOrWhiteSpace(givenCommandName)) {
<<<<<<< HEAD
				args.Player.SendErrorMessage("Please enter a proper command name or alias.");
=======
				args.Player.SendErrorMessage("请输入一个命令以查询同义。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			string commandName;
			if (givenCommandName[0] == Specifier[0])
				commandName = givenCommandName.Substring(1);
			else
				commandName = givenCommandName;

			bool didMatch = false;
			foreach (Command matchingCommand in ChatCommands.Where(cmd => cmd.Names.IndexOf(commandName) != -1)) {
				if (matchingCommand.Names.Count > 1)
					args.Player.SendInfoMessage(
<<<<<<< HEAD
					    "Aliases of {0}{1}: {0}{2}", Specifier, matchingCommand.Name, string.Join(", {0}".SFormat(Specifier), matchingCommand.Names.Skip(1)));
				else
					args.Player.SendInfoMessage("{0}{1} defines no aliases.", Specifier, matchingCommand.Name);
=======
					    "与{0}{1}同义的命令：{0}{2}", Specifier, matchingCommand.Name, string.Join(", {0}".SFormat(Specifier), matchingCommand.Names.Skip(1)));
				else
					args.Player.SendInfoMessage("{0}{1} 没有同义。", Specifier, matchingCommand.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件

				didMatch = true;
			}

			if (!didMatch)
<<<<<<< HEAD
				args.Player.SendErrorMessage("No command or command alias matching \"{0}\" found.", givenCommandName);
=======
				args.Player.SendErrorMessage("没有与 \"{0}\" 同义的命令。", givenCommandName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		#endregion General Commands

		#region Cheat Commands

		private static void Clear(CommandArgs args)
		{
			if (args.Parameters.Count != 1 && args.Parameters.Count != 2)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}clear <item/npc/projectile> [radius]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}清理 <item/npc/projectile> [半径]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			int radius = 50;
			if (args.Parameters.Count == 2)
			{
				if (!int.TryParse(args.Parameters[1], out radius) || radius <= 0)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid radius.");
=======
					args.Player.SendErrorMessage("半径错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
			}

			switch (args.Parameters[0].ToLower())
			{
				case "item":
				case "items":
					{
						int cleared = 0;
						for (int i = 0; i < Main.maxItems; i++)
						{
							float dX = Main.item[i].position.X - args.Player.X;
							float dY = Main.item[i].position.Y - args.Player.Y;

							if (Main.item[i].active && dX * dX + dY * dY <= radius * radius * 256f)
							{
								Main.item[i].active = false;
								TSPlayer.All.SendData(PacketTypes.ItemDrop, "", i);
								cleared++;
							}
						}
<<<<<<< HEAD
						args.Player.SendSuccessMessage("Deleted {0} items within a radius of {1}.", cleared, radius);
=======
						args.Player.SendSuccessMessage("清除了半径{1}里的{0}个物品。", cleared, radius);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					break;
				case "npc":
				case "npcs":
					{
						int cleared = 0;
						for (int i = 0; i < Main.maxNPCs; i++)
						{
							float dX = Main.npc[i].position.X - args.Player.X;
							float dY = Main.npc[i].position.Y - args.Player.Y;

							if (Main.npc[i].active && dX * dX + dY * dY <= radius * radius * 256f)
							{
								Main.npc[i].active = false;
								Main.npc[i].type = 0;
								TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
								cleared++;
							}
						}
<<<<<<< HEAD
						args.Player.SendSuccessMessage("Deleted {0} NPCs within a radius of {1}.", cleared, radius);
=======
						args.Player.SendSuccessMessage("清除了半径{1}里的{0}个NPC。", cleared, radius);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					break;
				case "proj":
				case "projectile":
				case "projectiles":
					{
						int cleared = 0;
						for (int i = 0; i < Main.maxProjectiles; i++)
						{
							float dX = Main.projectile[i].position.X - args.Player.X;
							float dY = Main.projectile[i].position.Y - args.Player.Y;

							if (Main.projectile[i].active && dX * dX + dY * dY <= radius * radius * 256f)
							{
								Main.projectile[i].active = false;
								Main.projectile[i].type = 0;
								TSPlayer.All.SendData(PacketTypes.ProjectileNew, "", i);
								cleared++;
							}
						}
<<<<<<< HEAD
						args.Player.SendSuccessMessage("Deleted {0} projectiles within a radius of {1}.", cleared, radius);
					}
					break;
				default:
					args.Player.SendErrorMessage("Invalid clear option!");
=======
						args.Player.SendSuccessMessage("清除了半径{1}里的{0}个弹幕。", cleared, radius);
					}
					break;
				default:
					args.Player.SendErrorMessage("选项错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					break;
			}
		}

		private static void Kill(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}kill <player>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}秒杀 <玩家名>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			string plStr = String.Join(" ", args.Parameters);
			var players = TShock.Utils.FindPlayer(plStr);
			if (players.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (players.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
			}
			else
			{
				var plr = players[0];
				plr.DamagePlayer(999999);
<<<<<<< HEAD
				args.Player.SendSuccessMessage(string.Format("You just killed {0}!", plr.Name));
				plr.SendErrorMessage("{0} just killed you!", args.Player.Name);
=======
				args.Player.SendSuccessMessage(string.Format("你秒杀了{0}。", plr.Name));
				plr.SendErrorMessage("{0} 秒杀了你。", args.Player.Name);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Butcher(CommandArgs args)
		{
			if (args.Parameters.Count > 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}butcher [mob type]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}杀 [怪物类型]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			int npcId = 0;

			if (args.Parameters.Count == 1)
			{
				var npcs = TShock.Utils.GetNPCByIdOrName(args.Parameters[0]);
				if (npcs.Count == 0)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid mob type!");
=======
					args.Player.SendErrorMessage("没有这个怪物。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				else if (npcs.Count > 1)
				{
					TShock.Utils.SendMultipleMatchError(args.Player, npcs.Select(n => n.name));
					return;
				}
				else
				{
					npcId = npcs[0].netID;
				}
			}

			int kills = 0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && ((npcId == 0 && !Main.npc[i].townNPC && Main.npc[i].netID != NPCID.TargetDummy) || Main.npc[i].netID == npcId))
				{
<<<<<<< HEAD
					TSPlayer.Server.StrikeNPC(i, (int)(Main.npc[i].life + (Main.npc[i].defense*0.5)), 0, 0);
					kills++;
				}
			}
			TSPlayer.All.SendInfoMessage("{0} butchered {1} NPCs.", args.Player.Name, kills);
=======
					TSPlayer.Server.StrikeNPC(i, 99999, 0, 0);
					kills++;
				}
			}
			TSPlayer.All.SendInfoMessage("{0} 秒杀了 {1} 个怪物", args.Player.Name, kills);
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}
		
		private static void Item(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}item <item name/id> [item amount] [prefix id/name]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}刷 <ID或名称> [数量] [前缀]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			int amountParamIndex = -1;
			int itemAmount = 0;
			for (int i = 1; i < args.Parameters.Count; i++)
			{
				if (int.TryParse(args.Parameters[i], out itemAmount))
				{
					amountParamIndex = i;
					break;
				}
			}

			string itemNameOrId;
			if (amountParamIndex == -1)
				itemNameOrId = string.Join(" ", args.Parameters);
			else
				itemNameOrId = string.Join(" ", args.Parameters.Take(amountParamIndex));

			Item item;
			List<Item> matchedItems = TShock.Utils.GetItemByIdOrName(itemNameOrId);
			if (matchedItems.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid item type!");
=======
				args.Player.SendErrorMessage("没有这个物品。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			else if (matchedItems.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, matchedItems.Select(i => i.name));
				return;
			}
			else
			{
				item = matchedItems[0];
			}
			if (item.type < 1 && item.type >= Main.maxItemTypes)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("The item type {0} is invalid.", itemNameOrId);
=======
				args.Player.SendErrorMessage("找不到物品 {0} 。", itemNameOrId);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			int prefixId = 0;
			if (amountParamIndex != -1 && args.Parameters.Count > amountParamIndex + 1)
			{
				string prefixidOrName = args.Parameters[amountParamIndex + 1];
				var prefixIds = TShock.Utils.GetPrefixByIdOrName(prefixidOrName);

				if (item.accessory && prefixIds.Contains(42))
				{
					prefixIds.Remove(42);
					prefixIds.Remove(76);
					prefixIds.Add(76);
				}
				else if (!item.accessory && prefixIds.Contains(42))
					prefixIds.Remove(76);

				if (prefixIds.Count > 1) 
				{
					TShock.Utils.SendMultipleMatchError(args.Player, prefixIds.Select(p => p.ToString()));
					return;
				}
				else if (prefixIds.Count == 0) 
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("No prefix matched \"{0}\".", prefixidOrName);
=======
					args.Player.SendErrorMessage("没有找到前缀 \"{0}\"。", prefixidOrName);
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				else
				{
					prefixId = prefixIds[0];
				}
			}

			if (args.Player.InventorySlotAvailable || (item.type > 70 && item.type < 75) || item.ammo > 0 || item.type == 58 || item.type == 184)
			{
				if (itemAmount == 0 || itemAmount > item.maxStack)
					itemAmount = item.maxStack;

				if (args.Player.GiveItemCheck(item.type, item.name, item.width, item.height, itemAmount, prefixId))
				{
					item.prefix = (byte)prefixId;
<<<<<<< HEAD
					args.Player.SendSuccessMessage("Gave {0} {1}(s).", itemAmount, item.AffixName());
				}
				else
				{
					args.Player.SendErrorMessage("You cannot spawn banned items.");
=======
					args.Player.SendSuccessMessage("给了{0}个{1}。", itemAmount, item.AffixName());
				}
				else
				{
					args.Player.SendErrorMessage("你不能刷被封禁的物品。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			}
			else
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Your inventory seems full.");
=======
				args.Player.SendErrorMessage("你的背包满了。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}
		
		private static void RenameNPC(CommandArgs args)
		{
			if (args.Parameters.Count != 2)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}renameNPC <guide, nurse, etc.> <newname>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}重命名NPC <guide, nurse, etc.> <newname>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			int npcId = 0;
			if (args.Parameters.Count == 2)
			{
				List<NPC> npcs = TShock.Utils.GetNPCByIdOrName(args.Parameters[0]);
				if (npcs.Count == 0)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid mob type!");
=======
					args.Player.SendErrorMessage("没有这个怪物。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				else if (npcs.Count > 1)
				{
					TShock.Utils.SendMultipleMatchError(args.Player, npcs.Select(n => n.name));
					return;
				}
				else if (args.Parameters[1].Length >200)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("New name is too large!");
=======
					args.Player.SendErrorMessage("名称过长。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				else
				{
					npcId = npcs[0].netID;
				}
			}
			int done=0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && ((npcId == 0 && !Main.npc[i].townNPC) || (Main.npc[i].netID == npcId && Main.npc[i].townNPC)))
				{
				Main.npc[i].displayName= args.Parameters[1];
				NetMessage.SendData(56, -1, -1, args.Parameters[1], i, 0f, 0f, 0f, 0);
				done++;
				}
			}
			if (done >0 )
			{
<<<<<<< HEAD
			TSPlayer.All.SendInfoMessage("{0} renamed the {1}.", args.Player.Name, args.Parameters[0]);
			}
			else
			{
			args.Player.SendErrorMessage("Could not rename {0}!", args.Parameters[0]);
=======
			TSPlayer.All.SendInfoMessage("{0}重命名了{1}。", args.Player.Name, args.Parameters[0]);
			}
			else
			{
			args.Player.SendErrorMessage("无法重命名{0}。", args.Parameters[0]);
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}
		
		private static void Give(CommandArgs args)
		{
			if (args.Parameters.Count < 2)
			{
				args.Player.SendErrorMessage(
<<<<<<< HEAD
					"Invalid syntax! Proper syntax: {0}give <item type/id> <player> [item amount] [prefix id/name]", Specifier);
=======
					"格式错误。 格式: {0}给 <ID或名称> <玩家名> [数量] [前缀]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			if (args.Parameters[0].Length == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Missing item name/id.");
=======
				args.Player.SendErrorMessage("请输入物品名或ID。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			if (args.Parameters[1].Length == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Missing player name.");
=======
				args.Player.SendErrorMessage("请输入用户名。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			int itemAmount = 0;
			int prefix = 0;
			var items = TShock.Utils.GetItemByIdOrName(args.Parameters[0]);
			args.Parameters.RemoveAt(0);
			string plStr = args.Parameters[0];
			args.Parameters.RemoveAt(0);
			if (args.Parameters.Count == 1)
				int.TryParse(args.Parameters[0], out itemAmount);
			else if (args.Parameters.Count == 2)
			{
				int.TryParse(args.Parameters[0], out itemAmount);
				var prefixIds = TShock.Utils.GetPrefixByIdOrName(args.Parameters[1]);
				if (items[0].accessory && prefixIds.Contains(42))
				{
					prefixIds.Remove(42);
					prefixIds.Remove(76);
					prefixIds.Add(76);
				}
				else if (!items[0].accessory && prefixIds.Contains(42))
					prefixIds.Remove(76);
				if (prefixIds.Count == 1)
					prefix = prefixIds[0];
			}

			if (items.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid item type!");
=======
				args.Player.SendErrorMessage("没有这个物品。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
			else if (items.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, items.Select(i => i.name));
			}
			else
			{
				var item = items[0];
				if (item.type >= 1 && item.type < Main.maxItemTypes)
				{
					var players = TShock.Utils.FindPlayer(plStr);
					if (players.Count == 0)
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("Invalid player!");
=======
						args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					}
					else if (players.Count > 1)
					{
						TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
					}
					else
					{
						var plr = players[0];
						if (plr.InventorySlotAvailable || (item.type > 70 && item.type < 75) || item.ammo > 0 || item.type == 58 || item.type == 184)
						{
							if (itemAmount == 0 || itemAmount > item.maxStack)
								itemAmount = item.maxStack;
							if (plr.GiveItemCheck(item.type, item.name, item.width, item.height, itemAmount, prefix))
							{
<<<<<<< HEAD
								args.Player.SendSuccessMessage(string.Format("Gave {0} {1} {2}(s).", plr.Name, itemAmount, item.name));
								plr.SendSuccessMessage(string.Format("{0} gave you {1} {2}(s).", args.Player.Name, itemAmount, item.name));
							}
							else
							{
								args.Player.SendErrorMessage("You cannot spawn banned items.");
=======
								args.Player.SendSuccessMessage(string.Format("给了{0} {1}个{2}。", plr.Name, itemAmount, item.name));
								plr.SendSuccessMessage(string.Format("{0} 给了你 {1}个{2}。", args.Player.Name, itemAmount, item.name));
							}
							else
							{
								args.Player.SendErrorMessage("你不能刷被封禁的物品。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
							}
							
						}
						else
						{
<<<<<<< HEAD
							args.Player.SendErrorMessage("Player does not have free slots!");
=======
							args.Player.SendErrorMessage("该玩家背包已满。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						}
					}
				}
				else
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid item type!");
=======
					args.Player.SendErrorMessage("没有这个物品。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				}
			}
		}

		private static void Heal(CommandArgs args)
		{
			TSPlayer playerToHeal;
			if (args.Parameters.Count > 0)
			{
				string plStr = String.Join(" ", args.Parameters);
				var players = TShock.Utils.FindPlayer(plStr);
				if (players.Count == 0)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid player!");
=======
					args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				else if (players.Count > 1)
				{
					TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
					return;
				}
				else
				{
					playerToHeal = players[0];
				}
			}
			else if (!args.Player.RealPlayer)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("You can't heal yourself!");
=======
				args.Player.SendErrorMessage("你不能帮自己回血。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			else
			{
				playerToHeal = args.Player;
			}

			playerToHeal.Heal();
			if (playerToHeal == args.Player)
			{
<<<<<<< HEAD
				args.Player.SendSuccessMessage("You just got healed!");
			}
			else
			{
				args.Player.SendSuccessMessage(string.Format("You just healed {0}", playerToHeal.Name));
				playerToHeal.SendSuccessMessage(string.Format("{0} just healed you!", args.Player.Name));
=======
				args.Player.SendSuccessMessage("你被回满血了。");
			}
			else
			{
				args.Player.SendSuccessMessage(string.Format("你回满了{0}的血。", playerToHeal.Name));
				playerToHeal.SendSuccessMessage(string.Format("{0} 治疗了你。", args.Player.Name));
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Buff(CommandArgs args)
		{
			if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}buff <buff id/name> [time(seconds)]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}状态 <ID或名称> [时长(秒)]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			int id = 0;
			int time = 60;
			if (!int.TryParse(args.Parameters[0], out id))
			{
				var found = TShock.Utils.GetBuffByName(args.Parameters[0]);
				if (found.Count == 0)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid buff name!");
=======
					args.Player.SendErrorMessage("名称错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				else if (found.Count > 1)
				{
					TShock.Utils.SendMultipleMatchError(args.Player, found.Select(f => Main.buffName[f]));
					return;
				}
				id = found[0];
			}
			if (args.Parameters.Count == 2)
				int.TryParse(args.Parameters[1], out time);
			if (id > 0 && id < Main.maxBuffTypes)
			{
				if (time < 0 || time > short.MaxValue)
					time = 60;
				args.Player.SetBuff(id, time*60);
				args.Player.SendSuccessMessage(string.Format("You have buffed yourself with {0}({1}) for {2} seconds!",
													  TShock.Utils.GetBuffName(id), TShock.Utils.GetBuffDescription(id), (time)));
			}
			else
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid buff ID!");
=======
				args.Player.SendErrorMessage("ID错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void GBuff(CommandArgs args)
		{
			if (args.Parameters.Count < 2 || args.Parameters.Count > 3)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}gbuff <player> <buff id/name> [time(seconds)]", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}给状态 <玩家名> <ID或名称> [时长(秒)]", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			int id = 0;
			int time = 60;
			var foundplr = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (foundplr.Count == 0)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid player!");
=======
				args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			else if (foundplr.Count > 1)
			{
				TShock.Utils.SendMultipleMatchError(args.Player, foundplr.Select(p => p.Name));
				return;
			}
			else
			{
				if (!int.TryParse(args.Parameters[1], out id))
				{
					var found = TShock.Utils.GetBuffByName(args.Parameters[1]);
					if (found.Count == 0)
					{
<<<<<<< HEAD
						args.Player.SendErrorMessage("Invalid buff name!");
=======
						args.Player.SendErrorMessage("名称错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
						return;
					}
					else if (found.Count > 1)
					{
						TShock.Utils.SendMultipleMatchError(args.Player, found.Select(b => Main.buffName[b]));
						return;
					}
					id = found[0];
				}
				if (args.Parameters.Count == 3)
					int.TryParse(args.Parameters[2], out time);
				if (id > 0 && id < Main.maxBuffTypes)
				{
					if (time < 0 || time > short.MaxValue)
						time = 60;
					foundplr[0].SetBuff(id, time*60);
					args.Player.SendSuccessMessage(string.Format("You have buffed {0} with {1}({2}) for {3} seconds!",
														  foundplr[0].Name, TShock.Utils.GetBuffName(id),
														  TShock.Utils.GetBuffDescription(id), (time)));
					foundplr[0].SendSuccessMessage(string.Format("{0} has buffed you with {1}({2}) for {3} seconds!",
														  args.Player.Name, TShock.Utils.GetBuffName(id),
														  TShock.Utils.GetBuffDescription(id), (time)));
				}
				else
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid buff ID!");
=======
					args.Player.SendErrorMessage("ID错误。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		private static void Grow(CommandArgs args)
		{
			if (args.Parameters.Count != 1)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}grow <tree/epictree/mushroom/cactus/herb>", Specifier);
=======
				args.Player.SendErrorMessage("格式错误。 格式: {0}种 <tree/epictree/mushroom/cactus/herb>", Specifier);
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			var name = "Fail";
			var x = args.Player.TileX;
			var y = args.Player.TileY + 3;

			if (!TShock.Regions.CanBuild(x, y, args.Player))
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("You're not allowed to change tiles here!");
=======
				args.Player.SendErrorMessage("不允许修改此处的方块。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}

			switch (args.Parameters[0].ToLower())
			{
				case "tree":
					for (int i = x - 1; i < x + 2; i++)
					{
						Main.tile[i, y].active(true);
						Main.tile[i, y].type = 2;
						Main.tile[i, y].wall = 0;
					}
					Main.tile[x, y - 1].wall = 0;
					WorldGen.GrowTree(x, y);
					name = "Tree";
					break;
				case "epictree":
					for (int i = x - 1; i < x + 2; i++)
					{
						Main.tile[i, y].active(true);
						Main.tile[i, y].type = 2;
						Main.tile[i, y].wall = 0;
					}
					Main.tile[x, y - 1].wall = 0;
					Main.tile[x, y - 1].liquid = 0;
					Main.tile[x, y - 1].active(true);
					WorldGen.GrowEpicTree(x, y);
					name = "Epic Tree";
					break;
				case "mushroom":
					for (int i = x - 1; i < x + 2; i++)
					{
						Main.tile[i, y].active(true);
						Main.tile[i, y].type = 70;
						Main.tile[i, y].wall = 0;
					}
					Main.tile[x, y - 1].wall = 0;
					WorldGen.GrowShroom(x, y);
					name = "Mushroom";
					break;
				case "cactus":
					Main.tile[x, y].type = 53;
					WorldGen.GrowCactus(x, y);
					name = "Cactus";
					break;
				case "herb":
					Main.tile[x, y].active(true);
					Main.tile[x, y].frameX = 36;
					Main.tile[x, y].type = 83;
					WorldGen.GrowAlch(x, y);
					name = "Herb";
					break;
				default:
<<<<<<< HEAD
					args.Player.SendErrorMessage("Unknown plant!");
					return;
			}
			args.Player.SendTileSquare(x, y);
			args.Player.SendSuccessMessage("Tried to grow a " + name + ".");
=======
					args.Player.SendErrorMessage("没有这种植物。");
					return;
			}
			args.Player.SendTileSquare(x, y);
			args.Player.SendSuccessMessage("尝试种植 " + name + "。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
		}

		private static void ToggleGodMode(CommandArgs args)
		{
			TSPlayer playerToGod;
			if (args.Parameters.Count > 0)
			{
<<<<<<< HEAD
				if (!args.Player.HasPermission(Permissions.godmodeother))
				{
					args.Player.SendErrorMessage("You do not have permission to god mode another player!");
=======
				if (!args.Player.Group.HasPermission(Permissions.godmodeother))
				{
					args.Player.SendErrorMessage("没有权限。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				string plStr = String.Join(" ", args.Parameters);
				var players = TShock.Utils.FindPlayer(plStr);
				if (players.Count == 0)
				{
<<<<<<< HEAD
					args.Player.SendErrorMessage("Invalid player!");
=======
					args.Player.SendErrorMessage("玩家不存在。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
					return;
				}
				else if (players.Count > 1)
				{
					TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
					return;
				}
				else
				{
					playerToGod = players[0];
				}
			}
			else if (!args.Player.RealPlayer)
			{
<<<<<<< HEAD
				args.Player.SendErrorMessage("You can't god mode a non player!");
=======
				args.Player.SendErrorMessage("只能将玩家设为上帝模式。");
>>>>>>> e2683b6... 手动加入RYH修改的文件
				return;
			}
			else
			{
				playerToGod = args.Player;
			}

			playerToGod.GodMode = !playerToGod.GodMode;

			if (playerToGod == args.Player)
			{
<<<<<<< HEAD
				args.Player.SendSuccessMessage(string.Format("You are {0} in god mode.", args.Player.GodMode ? "now" : "no longer"));
			}
			else
			{
				args.Player.SendSuccessMessage(string.Format("{0} is {1} in god mode.", playerToGod.Name, playerToGod.GodMode ? "now" : "no longer"));
				playerToGod.SendSuccessMessage(string.Format("You are {0} in god mode.", playerToGod.GodMode ? "now" : "no longer"));
=======
				args.Player.SendSuccessMessage(string.Format("你{0}处于上帝模式。", playerToGod.GodMode ? "正" : "不再"));
			}
			else
			{
				args.Player.SendSuccessMessage(string.Format("{0} {1}处于上帝模式", playerToGod.Name, playerToGod.GodMode ? "正" : "不再"));
				playerToGod.SendSuccessMessage(string.Format("你{0}处于上帝模式。", playerToGod.GodMode ? "正" : "不再"));
>>>>>>> e2683b6... 手动加入RYH修改的文件
			}
		}

		#endregion Cheat Comamnds
	}
}
