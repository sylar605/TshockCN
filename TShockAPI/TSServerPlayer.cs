﻿using System;
using System.Collections.Generic;
using Terraria;
using TShockAPI;
using TShockAPI.DB;

namespace TShockAPI
{
	public class TSServerPlayer : TSPlayer
	{
		public static string AccountName = "ServerConsole";

		public TSServerPlayer()
			: base("Server")
		{
			Group = new SuperAdminGroup();
			User = new User { Name = AccountName };
		}

		public override void SendErrorMessage(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(msg);
			Console.ResetColor();
		}

		public override void SendInfoMessage(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(msg);
			Console.ResetColor();
		}

		public override void SendSuccessMessage(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(msg);
			Console.ResetColor();
		}

		public override void SendWarningMessage(string msg)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine(msg);
			Console.ResetColor();
		}

		public override void SendMessage(string msg, Color color)
		{
			SendMessage(msg, color.R, color.G, color.B);
		}

		public override void SendMessage(string msg, byte red, byte green, byte blue)
		{
			Console.WriteLine(msg);
		}

		public void SetFullMoon()
		{
			Main.dayTime = false;
			Main.moonPhase = 0;
			Main.time = 0.0;
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
		}

		public void SetBloodMoon(bool bloodMoon)
		{
			if (bloodMoon)
			{
				Main.dayTime = false;
				Main.bloodMoon = true;
				Main.time = 0.0;
			}
			else
				Main.bloodMoon = false;
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
		}

		public void SetFrostMoon(bool snowMoon)
		{
			if (snowMoon)
			{
				Main.dayTime = false;
				Main.snowMoon = true;
				Main.time = 0.0;
			}
			else
				Main.snowMoon = false;
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
		}

		public void SetPumpkinMoon(bool pumpkinMoon)
		{
			if (pumpkinMoon)
			{
				Main.dayTime = false;
				Main.pumpkinMoon = true;
				Main.time = 0.0;
			}
			else
				Main.pumpkinMoon = false;
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
		}

		public void SetEclipse(bool eclipse)
		{
			if (eclipse)
			{
				Main.dayTime = Main.eclipse = true;
				Main.time = 0.0;
			}
			else
				Main.eclipse = false;
			TSPlayer.All.SendData(PacketTypes.WorldInfo);
		}

		public void SetTime(bool dayTime, double time)
		{
			Main.dayTime = dayTime;
			Main.time = time;
			TSPlayer.All.SendData(PacketTypes.TimeSet, "", dayTime ? 1 : 0, (int)time, Main.sunModY, Main.moonModY);
		}

		public void SpawnNPC(int type, string name, int amount, int startTileX, int startTileY, int tileXRange = 100,
			int tileYRange = 50)
		{
			for (int i = 0; i < amount; i++)
			{
				int spawnTileX;
				int spawnTileY;
				TShock.Utils.GetRandomClearTileWithInRange(startTileX, startTileY, tileXRange, tileYRange, out spawnTileX,
															 out spawnTileY);
				int npcid = NPC.NewNPC(spawnTileX * 16, spawnTileY * 16, type, 0);
				// This is for special slimes
				Main.npc[npcid].SetDefaults(name);
			}
		}

		public void StrikeNPC(int npcid, int damage, float knockBack, int hitDirection)
		{
			// Main.rand is thread static.
			if (Main.rand == null)
				Main.rand = new Random();

			Main.npc[npcid].StrikeNPC(damage, knockBack, hitDirection);
			NetMessage.SendData((int)PacketTypes.NpcStrike, -1, -1, "", npcid, damage, knockBack, hitDirection);
		}

		public void RevertTiles(Dictionary<Vector2, Tile> tiles)
		{
			// Update Main.Tile first so that when tile sqaure is sent it is correct
			foreach (KeyValuePair<Vector2, Tile> entry in tiles)
			{
				Main.tile[(int)entry.Key.X, (int)entry.Key.Y] = entry.Value;
			}
			// Send all players updated tile sqaures
			foreach (Vector2 coords in tiles.Keys)
			{
				All.SendTileSquare((int)coords.X, (int)coords.Y, 3);
			}
		}
	}
}