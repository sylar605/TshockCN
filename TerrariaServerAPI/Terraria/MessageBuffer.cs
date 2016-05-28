
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Net;
using TerrariaApi.Server;

namespace Terraria
{
	public class MessageBuffer
	{
		public const int readBufferMax = 131070;

		public const int writeBufferMax = 131070;

		public bool broadcast;

		public byte[] readBuffer = new byte[131070];

		//public byte[] writeBuffer = new byte[131070];

		public bool writeLocked;

		public int messageLength;

		public int totalData;

		public int whoAmI;

		public int spamCount;

		public int maxSpam;

		public bool checkBytes;

		public MemoryStream readerStream;

		//public MemoryStream writerStream;

		public BinaryReader reader;

		//public BinaryWriter writer;

		public MessageBuffer()
		{
		}

		public void GetData(int start, int length)
		{
			List<Point> points;
			List<Point> points1;
			int num;
			TileEntity tileEntity;
			if (this.whoAmI >= 256)
			{
				Netplay.Connection.TimeOutTimer = 0;
			}
			else
			{
				Netplay.Clients[this.whoAmI].TimeOutTimer = 0;
			}
			byte num1 = 0;
			int num2 = 0;
			num2 = start + 1;
			num1 = this.readBuffer[start];

			if (Enum.IsDefined(typeof(PacketTypes), (int)num1) == false)
			{
				return;
			}

			if (ServerApi.Hooks.InvokeNetGetData(ref num1, this, ref num2, ref length))
			{
				return;
			}
			
			//Main.rxMsg = Main.rxMsg + 1;
			//Main.rxData = Main.rxData + length;
			
			if (Main.verboseNetplay)
			{
				int num3 = start;
				while (num3 < start + length)
				{
					num3++;
				}
				for (int i = start; i < start + length; i++)
				{
					byte num4 = this.readBuffer[i];
				}
			}
			if (num1 != 38 && Netplay.Clients[this.whoAmI].State == -1)
			{
				NetMessage.SendData(2, this.whoAmI, -1, Lang.mp[1], 0, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			if (Netplay.Clients[this.whoAmI].State < 10 && num1 > 12 && num1 != 93 && num1 != 16 && num1 != 42 && num1 != 50 && num1 != 38 && num1 != 68)
			{
				ServerApi.LogWriter.ServerWriteLine(string.Format("getdata: slot {0}: msg id {1} on client state {2}", whoAmI, num1,  Netplay.Clients[this.whoAmI].State), TraceLevel.Warning);
				NetMessage.BootPlayer(this.whoAmI, Lang.mp[2]);
			}
			if (this.reader == null)
			{
				this.ResetReader();
			}
			this.reader.BaseStream.Position = (long)num2;
			byte num5 = num1;

			switch (num5)
			{
				case 1:
				{
					if (Main.dedServ && Netplay.IsBanned(Netplay.Clients[this.whoAmI].Socket.GetRemoteAddress()))
					{
						NetMessage.SendData(2, this.whoAmI, -1, Lang.mp[3], 0, 0f, 0f, 0f, 0, 0, 0);
						return;
					}
					if (Netplay.Clients[this.whoAmI].State != 0)
					{
						return;
					}
					if (this.reader.ReadString() != string.Concat("Terraria", Main.curRelease))
					{
						NetMessage.SendData(2, this.whoAmI, -1, Lang.mp[4], 0, 0f, 0f, 0f, 0, 0, 0);
						return;
					}
					if (string.IsNullOrEmpty(Netplay.ServerPassword))
					{
						Netplay.Clients[this.whoAmI].State = 1;
						NetMessage.SendData(3, this.whoAmI, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
						return;
					}
					Netplay.Clients[this.whoAmI].State = -1;
					NetMessage.SendData(37, this.whoAmI, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 4:
				{
					int num7 = this.reader.ReadByte();
					num7 = this.whoAmI;
					if (num7 == Main.myPlayer && !Main.ServerSideCharacter)
					{
						return;
					}
					Player player = Main.player[num7];
					player.whoAmI = num7;
					player.skinVariant = this.reader.ReadByte();
					player.skinVariant = (int)MathHelper.Clamp((float)player.skinVariant, 0f, 9f);
					player.hair = this.reader.ReadByte();
					if (player.hair >= 134)
					{
						player.hair = 0;
					}
					player.name = this.reader.ReadString().Trim().Trim();
					player.hairDye = this.reader.ReadByte();
					BitsByte bitsByte = this.reader.ReadByte();
					for (int q = 0; q < 8; q++)
					{
						player.hideVisual[q] = bitsByte[q];
					}
					bitsByte = this.reader.ReadByte();
					for (int r = 0; r < 2; r++)
					{
						player.hideVisual[r + 8] = bitsByte[r];
					}
					player.hideMisc = this.reader.ReadByte();
					player.hairColor = this.reader.ReadRGB();
					player.skinColor = this.reader.ReadRGB();
					player.eyeColor = this.reader.ReadRGB();
					player.shirtColor = this.reader.ReadRGB();
					player.underShirtColor = this.reader.ReadRGB();
					player.pantsColor = this.reader.ReadRGB();
					player.shoeColor = this.reader.ReadRGB();
					BitsByte bitsByte1 = this.reader.ReadByte();
					player.difficulty = 0;
					if (bitsByte1[0])
					{
						player.difficulty += 1;
					}
					if (bitsByte1[1])
					{
						player.difficulty += 2;
					}
					player.extraAccessory = bitsByte1[2];
					bool flag = false;
					if (Netplay.Clients[this.whoAmI].State < 10)
					{
						for (int s = 0; s < 255; s++)
						{
							if (s != num7 && player.name == Main.player[s].name && Netplay.Clients[s].IsActive)
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						if (!ServerApi.Hooks.InvokeNetNameCollision(num7, player.name))
						{
							NetMessage.SendData(2, this.whoAmI, -1, string.Concat(player.name, " ", Lang.mp[5]), 0, 0f, 0f, 0f, 0, 0, 0);
						}
						return;
					}
					if (player.name.Length > Player.nameLen)
					{
						NetMessage.SendData(2, this.whoAmI, -1, "Name is too long.", 0, 0f, 0f, 0f, 0, 0, 0);
						return;
					}
					if (player.name == "")
					{
						NetMessage.SendData(2, this.whoAmI, -1, "Empty name.", 0, 0f, 0f, 0f, 0, 0, 0);
						return;
					}
					Netplay.Clients[this.whoAmI].Name = player.name;
					Netplay.Clients[this.whoAmI].Name = player.name;
					NetMessage.SendData(4, -1, this.whoAmI, player.name, num7, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 5:
				{
					int num8 = this.reader.ReadByte();
					num8 = this.whoAmI;
					if (num8 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[num8].IsStackingItems())
					{
						return;
					}
					Player item1 = Main.player[num8];
					lock (item1)
					{
						int num9 = this.reader.ReadByte();
						int num10 = this.reader.ReadInt16();
						int num11 = this.reader.ReadByte();
						int num12 = this.reader.ReadInt16();
						Item[] itemArray = null;
						int num13 = 0;
						bool flag1 = false;
						if (num9 > 58 + (int)item1.armor.Length + (int)item1.dye.Length + (int)item1.miscEquips.Length + (int)item1.miscDyes.Length + (int)item1.bank.item.Length + (int)item1.bank2.item.Length)
						{
							flag1 = true;
						}
						else if (num9 > 58 + (int)item1.armor.Length + (int)item1.dye.Length + (int)item1.miscEquips.Length + (int)item1.miscDyes.Length + (int)item1.bank.item.Length)
						{
							num13 = num9 - 58 - ((int)item1.armor.Length + (int)item1.dye.Length + (int)item1.miscEquips.Length + (int)item1.miscDyes.Length + (int)item1.bank.item.Length) - 1;
							itemArray = item1.bank2.item;
						}
						else if (num9 > 58 + (int)item1.armor.Length + (int)item1.dye.Length + (int)item1.miscEquips.Length + (int)item1.miscDyes.Length)
						{
							num13 = num9 - 58 - ((int)item1.armor.Length + (int)item1.dye.Length + (int)item1.miscEquips.Length + (int)item1.miscDyes.Length) - 1;
							itemArray = item1.bank.item;
						}
						else if (num9 > 58 + (int)item1.armor.Length + (int)item1.dye.Length + (int)item1.miscEquips.Length)
						{
							num13 = num9 - 58 - ((int)item1.armor.Length + (int)item1.dye.Length + (int)item1.miscEquips.Length) - 1;
							itemArray = item1.miscDyes;
						}
						else if (num9 > 58 + (int)item1.armor.Length + (int)item1.dye.Length)
						{
							num13 = num9 - 58 - ((int)item1.armor.Length + (int)item1.dye.Length) - 1;
							itemArray = item1.miscEquips;
						}
						else if (num9 > 58 + (int)item1.armor.Length)
						{
							num13 = num9 - 58 - (int)item1.armor.Length - 1;
							itemArray = item1.dye;
						}
						else if (num9 <= 58)
						{
							num13 = num9;
							itemArray = item1.inventory;
						}
						else
						{
							num13 = num9 - 58 - 1;
							itemArray = item1.armor;
						}
						if (flag1)
						{
							item1.trashItem = new Item();
							item1.trashItem.netDefaults(num12);
							item1.trashItem.stack = num10;
							item1.trashItem.Prefix(num11);
						}
						else if (num9 > 58)
						{
							itemArray[num13] = new Item();
							itemArray[num13].netDefaults(num12);
							itemArray[num13].stack = num10;
							itemArray[num13].Prefix(num11);
						}
						else
						{
							int num14 = itemArray[num13].type;
							int num15 = itemArray[num13].stack;
							itemArray[num13] = new Item();
							itemArray[num13].netDefaults(num12);
							itemArray[num13].stack = num10;
							itemArray[num13].Prefix(num11);
							if (num8 == Main.myPlayer && num13 == 58)
							{
								Main.mouseItem = itemArray[num13].Clone();
							}
						}
						if (num8 == this.whoAmI)
						{
							NetMessage.SendData(5, -1, this.whoAmI, "", num8, (float)num9, (float)num11, 0f, 0, 0, 0);
						}
						return;
					}
				}
				case 6:
				{
					if (Netplay.Clients[this.whoAmI].State == 1)
					{
						Netplay.Clients[this.whoAmI].State = 2;
						Netplay.Clients[this.whoAmI].ResetSections();
					}
					NetMessage.SendData(7, this.whoAmI, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
					Main.SyncAnInvasion(this.whoAmI);
					return;
				}
				case 8:
				{
					int sectionX = this.reader.ReadInt32();
					int sectionY = this.reader.ReadInt32();
					bool flag2 = true;
					if (sectionX == -1 || sectionY == -1)
					{
						flag2 = false;
					}
					else if (sectionX < 10 || sectionX > Main.maxTilesX - 10)
					{
						flag2 = false;
					}
					else if (sectionY < 10 || sectionY > Main.maxTilesY - 10)
					{
						flag2 = false;
					}
					int sectionX1 = Netplay.GetSectionX(Main.spawnTileX) - 2;
					int sectionY1 = Netplay.GetSectionY(Main.spawnTileY) - 1;
					int num16 = sectionX1 + 5;
					int num17 = sectionY1 + 3;
					if (sectionX1 < 0)
					{
						sectionX1 = 0;
					}
					if (num16 >= Main.maxSectionsX)
					{
						num16 = Main.maxSectionsX - 1;
					}
					if (sectionY1 < 0)
					{
						sectionY1 = 0;
					}
					if (num17 >= Main.maxSectionsY)
					{
						num17 = Main.maxSectionsY - 1;
					}
					int count = (num16 - sectionX1) * (num17 - sectionY1);
					List<Point> points2 = new List<Point>();
					for (int x = sectionX1; x < num16; x++)
					{
						for (int y = sectionY1; y < num17; y++)
						{
							points2.Add(new Point(x, y));
						}
					}
					int num18 = -1;
					int num19 = -1;
					if (flag2)
					{
						sectionX = Netplay.GetSectionX(sectionX) - 2;
						sectionY = Netplay.GetSectionY(sectionY) - 1;
						num18 = sectionX + 5;
						num19 = sectionY + 3;
						if (sectionX < 0)
						{
							sectionX = 0;
						}
						if (num18 >= Main.maxSectionsX)
						{
							num18 = Main.maxSectionsX - 1;
						}
						if (sectionY < 0)
						{
							sectionY = 0;
						}
						if (num19 >= Main.maxSectionsY)
						{
							num19 = Main.maxSectionsY - 1;
						}
						for (int a = sectionX; a < num18; a++)
						{
							for (int b = sectionY; b < num19; b++)
							{
								if (a < sectionX1 || a >= num16 || b < sectionY1 || b >= num17)
								{
									points2.Add(new Point(a, b));
									count++;
								}
							}
						}
					}
					int num20 = 1;
					PortalHelper.SyncPortalsOnPlayerJoin(this.whoAmI, 1, points2, out points, out points1);
					count = count + points.Count;
					if (Netplay.Clients[this.whoAmI].State == 2)
					{
						Netplay.Clients[this.whoAmI].State = 3;
					}
					NetMessage.SendData(9, this.whoAmI, -1, Lang.inter[44], count, 0f, 0f, 0f, 0, 0, 0);
					Netplay.Clients[this.whoAmI].StatusText2 = "is receiving tile data";
					RemoteClient clients = Netplay.Clients[this.whoAmI];
					clients.StatusMax = clients.StatusMax + count;
					for (int c = sectionX1; c < num16; c++)
					{
						for (int d = sectionY1; d < num17; d++)
						{
							NetMessage.SendSection(this.whoAmI, c, d, false);
						}
					}
					NetMessage.SendData(11, this.whoAmI, -1, "", sectionX1, (float)sectionY1, (float)(num16 - 1), (float)(num17 - 1), 0, 0, 0);
					if (flag2)
					{
						for (int e = sectionX; e < num18; e++)
						{
							for (int f = sectionY; f < num19; f++)
							{
								NetMessage.SendSection(this.whoAmI, e, f, true);
							}
						}
						NetMessage.SendData(11, this.whoAmI, -1, "", sectionX, (float)sectionY, (float)(num18 - 1), (float)(num19 - 1), 0, 0, 0);
					}
					for (int g = 0; g < points.Count; g++)
					{
						NetMessage.SendSection(this.whoAmI, points[g].X, points[g].Y, true);
					}
					for (int h = 0; h < points1.Count; h++)
					{
						NetMessage.SendData(11, this.whoAmI, -1, "", points1[h].X - num20, (float)(points1[h].Y - num20), (float)(points1[h].X + num20 + 1), (float)(points1[h].Y + num20 + 1), 0, 0, 0);
					}
					for (int i1 = 0; i1 < 400; i1++)
					{
						if (Main.item[i1].active)
						{
							NetMessage.SendData(21, this.whoAmI, -1, "", i1, 0f, 0f, 0f, 0, 0, 0);
							NetMessage.SendData(22, this.whoAmI, -1, "", i1, 0f, 0f, 0f, 0, 0, 0);
						}
					}
					for (int j1 = 0; j1 < 200; j1++)
					{
						if (Main.npc[j1].active)
						{
							NetMessage.SendData(23, this.whoAmI, -1, "", j1, 0f, 0f, 0f, 0, 0, 0);
						}
					}
					for (int k1 = 0; k1 < 1000; k1++)
					{
						if (Main.projectile[k1].active && (Main.projPet[Main.projectile[k1].type] || Main.projectile[k1].netImportant))
						{
							NetMessage.SendData(27, this.whoAmI, -1, "", k1, 0f, 0f, 0f, 0, 0, 0);
						}
					}
					for (int l1 = 0; l1 < 251; l1++)
					{
						NetMessage.SendData(83, this.whoAmI, -1, "", l1, 0f, 0f, 0f, 0, 0, 0);
					}
					NetMessage.SendData(49, this.whoAmI, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
					NetMessage.SendData(57, this.whoAmI, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
					NetMessage.SendData(7, this.whoAmI, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
					NetMessage.SendData(103, -1, -1, "", NPC.MoonLordCountdown, 0f, 0f, 0f, 0, 0, 0);
					NetMessage.SendData(101, this.whoAmI, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 12:
				{
					int num21 = this.reader.ReadByte();
					num21 = this.whoAmI;
					Player player4 = Main.player[num21];
					player4.SpawnX = this.reader.ReadInt16();
					player4.SpawnY = this.reader.ReadInt16();
					player4.Spawn();
					if (Netplay.Clients[this.whoAmI].State < 3)
					{
						return;
					}
					if (Netplay.Clients[this.whoAmI].State != 3)
					{
						NetMessage.SendData(12, -1, this.whoAmI, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
						return;
					}
					Netplay.Clients[this.whoAmI].State = 10;
					NetMessage.greetPlayer(this.whoAmI);
					NetMessage.buffer[this.whoAmI].broadcast = true;
					NetMessage.syncPlayers();
					NetMessage.SendData(12, -1, this.whoAmI, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
					NetMessage.SendData(74, this.whoAmI, -1, Main.player[this.whoAmI].name, Main.anglerQuest, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 13:
				{
					int num22 = this.reader.ReadByte();
					if (num22 == Main.myPlayer && !Main.ServerSideCharacter)
					{
						return;
					}
					num22 = this.whoAmI;
					Player item2 = Main.player[num22];
					BitsByte bitsByte7 = this.reader.ReadByte();
					item2.controlUp = bitsByte7[0];
					item2.controlDown = bitsByte7[1];
					item2.controlLeft = bitsByte7[2];
					item2.controlRight = bitsByte7[3];
					item2.controlJump = bitsByte7[4];
					item2.controlUseItem = bitsByte7[5];
					item2.direction = (bitsByte7[6] ? 1 : -1);
					BitsByte bitsByte8 = this.reader.ReadByte();
					if (!bitsByte8[0])
					{
						item2.pulley = false;
					}
					else
					{
						item2.pulley = true;
						item2.pulleyDir = (byte)((bitsByte8[1] ? 2 : 1));
					}
					item2.selectedItem = this.reader.ReadByte();
					item2.position = this.reader.ReadVector2();
					if (bitsByte8[2])
					{
						item2.velocity = this.reader.ReadVector2();
					}
					else
					{
						item2.velocity = Vector2.Zero;
					}
					item2.vortexStealthActive = bitsByte8[3];
					item2.gravDir = (float)((bitsByte8[4] ? 1 : -1));
					if (Netplay.Clients[this.whoAmI].State != 10)
					{
						return;
					}
					NetMessage.SendData(13, -1, this.whoAmI, "", num22, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 16:
				{
					int num24 = this.reader.ReadByte();
					if (num24 == Main.myPlayer && !Main.ServerSideCharacter)
					{
						return;
					}
					num24 = this.whoAmI;
					Player player5 = Main.player[num24];
					player5.statLife = this.reader.ReadInt16();
					player5.statLifeMax = this.reader.ReadInt16();
					if (player5.statLifeMax < 100)
					{
						player5.statLifeMax = 100;
					}
					player5.dead = player5.statLife <= 0;
					NetMessage.SendData(16, -1, this.whoAmI, "", num24, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 17:
				{
					byte num25 = this.reader.ReadByte();
					int num26 = this.reader.ReadInt16();
					int num27 = this.reader.ReadInt16();
					short num28 = this.reader.ReadInt16();
					int num29 = this.reader.ReadByte();
					bool flag3 = num28 == 1;
					if (!WorldGen.InWorld(num26, num27, 3))
					{
						return;
					}
					if (Main.tile[num26, num27] == null)
					{
						Main.tile[num26, num27] = new Tile();
					}
					if (!flag3)
					{
						if (num25 == 0 || num25 == 2 || num25 == 4)
						{
							RemoteClient spamDeleteBlock = Netplay.Clients[this.whoAmI];
							spamDeleteBlock.SpamDeleteBlock = spamDeleteBlock.SpamDeleteBlock + 1f;
						}
						if (num25 == 1 || num25 == 3)
						{
							RemoteClient spamAddBlock = Netplay.Clients[this.whoAmI];
							spamAddBlock.SpamAddBlock = spamAddBlock.SpamAddBlock + 1f;
						}
					}
					if (!Netplay.Clients[this.whoAmI].TileSections[Netplay.GetSectionX(num26), Netplay.GetSectionY(num27)])
					{
						flag3 = true;
					}
					if (num25 == 0)
					{
						WorldGen.KillTile(num26, num27, flag3, false, false);
					}
					if (num25 == 1)
					{
						WorldGen.PlaceTile(num26, num27, num28, false, true, -1, num29);
					}
					if (num25 == 2)
					{
						WorldGen.KillWall(num26, num27, flag3);
					}
					if (num25 == 3)
					{
						WorldGen.PlaceWall(num26, num27, num28, false);
					}
					if (num25 == 4)
					{
						WorldGen.KillTile(num26, num27, flag3, false, true);
					}
					if (num25 == 5)
					{
						WorldGen.PlaceWire(num26, num27);
					}
					if (num25 == 6)
					{
						WorldGen.KillWire(num26, num27);
					}
					if (num25 == 7)
					{
						WorldGen.PoundTile(num26, num27);
					}
					if (num25 == 8)
					{
						WorldGen.PlaceActuator(num26, num27);
					}
					if (num25 == 9)
					{
						WorldGen.KillActuator(num26, num27);
					}
					if (num25 == 10)
					{
						WorldGen.PlaceWire2(num26, num27);
					}
					if (num25 == 11)
					{
						WorldGen.KillWire2(num26, num27);
					}
					if (num25 == 12)
					{
						WorldGen.PlaceWire3(num26, num27);
					}
					if (num25 == 13)
					{
						WorldGen.KillWire3(num26, num27);
					}
					if (num25 == 14)
					{
						WorldGen.SlopeTile(num26, num27, num28);
					}
					if (num25 == 15)
					{
						Minecart.FrameTrack(num26, num27, true, false);
					}
					if (num25 == 16)
					{
						WorldGen.PlaceWire4(num26, num27);
					}
					if (num25 == 17)
					{
						WorldGen.KillWire4(num26, num27);
					}
					if (num25 == 18)
					{
						Wiring.SetCurrentUser(this.whoAmI);
						Wiring.PokeLogicGate(num26, num27);
						Wiring.SetCurrentUser(-1);
						return;
					}
					if (num25 == 19)
					{
						Wiring.SetCurrentUser(this.whoAmI);
						Wiring.Actuate(num26, num27);
						Wiring.SetCurrentUser(-1);
						return;
					}
					NetMessage.SendData(17, -1, this.whoAmI, "", (int)num25, (float)num26, (float)num27, (float)num28, num29, 0, 0);
					if (num25 != 1 || num28 != 53)
					{
						return;
					}
					NetMessage.SendTileSquare(-1, num26, num27, 1);
					return;
				}
				case 19:
				{
					byte num30 = this.reader.ReadByte();
					int num31 = this.reader.ReadInt16();
					int num32 = this.reader.ReadInt16();
					int num33 = (this.reader.ReadByte() == 0 ? -1 : 1);
					if (num30 == 0)
					{
						WorldGen.OpenDoor(num31, num32, num33);
					}
					else if (num30 == 1)
					{
						WorldGen.CloseDoor(num31, num32, true);
					}
					else if (num30 == 2)
					{
						WorldGen.ShiftTrapdoor(num31, num32, num33 == 1, 1);
					}
					else if (num30 == 3)
					{
						WorldGen.ShiftTrapdoor(num31, num32, num33 == 1, 0);
					}
					else if (num30 == 4)
					{
						WorldGen.ShiftTallGate(num31, num32, false);
					}
					else if (num30 == 5)
					{
						WorldGen.ShiftTallGate(num31, num32, true);
					}
					int num34 = this.whoAmI;
					byte num35 = num30;
					float single = (float)num31;
					float single1 = (float)num32;
					//not sure what this does
					float variable = 0;
					if (num33 == 1)
					{
						variable = 1;
					}
					NetMessage.SendData(19, -1, num34, "", (int)num35, single, single1, (float)variable, 0, 0, 0);
					return;
				}
				case 20:
				{
					short num36 = this.reader.ReadInt16();
					int num37 = this.reader.ReadInt16();
					int num38 = this.reader.ReadInt16();
					if (!WorldGen.InWorld(num37, num38, 3))
					{
						return;
					}
					BitsByte bitsByte9 = 0;
					BitsByte bitsByte10 = 0;
					Tile tile = null;
					for (int l1 = num37; l1 < num37 + num36; l1++)
					{
						for (int m1 = num38; m1 < num38 + num36; m1++)
						{
							if (Main.tile[l1, m1] == null)
							{
								Main.tile[l1, m1] = new Tile();
							}
							tile = Main.tile[l1, m1];
							bool flag4 = tile.active();
							bitsByte9 = this.reader.ReadByte();
							bitsByte10 = this.reader.ReadByte();
							tile.active(bitsByte9[0]);
							Tile tile1 = tile;
							byte wall = 0;
							if (bitsByte9[2])
							{
								wall = 1;
							}
							tile1.wall = wall;
							bool item3 = bitsByte9[3];
							tile.wire(bitsByte9[4]);
							tile.halfBrick(bitsByte9[5]);
							tile.actuator(bitsByte9[6]);
							tile.inActive(bitsByte9[7]);
							tile.wire2(bitsByte10[0]);
							tile.wire3(bitsByte10[1]);
							if (bitsByte10[2])
							{
								tile.color(this.reader.ReadByte());
							}
							if (bitsByte10[3])
							{
								tile.wallColor(this.reader.ReadByte());
							}
							if (tile.active())
							{
								int num39 = tile.type;
								tile.type = this.reader.ReadUInt16();
								if (Main.tileFrameImportant[tile.type])
								{
									tile.frameX = this.reader.ReadInt16();
									tile.frameY = this.reader.ReadInt16();
								}
								else if (!flag4 || tile.type != num39)
								{
									tile.frameX = -1;
									tile.frameY = -1;
								}
								byte num40 = 0;
								if (bitsByte10[4])
								{
									num40 = (byte)(num40 + 1);
								}
								if (bitsByte10[5])
								{
									num40 = (byte)(num40 + 2);
								}
								if (bitsByte10[6])
								{
									num40 = (byte)(num40 + 4);
								}
								tile.slope(num40);
							}
							tile.wire4(bitsByte10[7]);
							if (tile.wall > 0)
							{
								tile.wall = this.reader.ReadByte();
							}
							if (item3)
							{
								tile.liquid = this.reader.ReadByte();
								tile.liquidType((int)this.reader.ReadByte());
							}
						}
					}
					WorldGen.RangeFrame(num37, num38, num37 + num36, num38 + num36);
					NetMessage.SendData((int)num1, -1, this.whoAmI, "", num36, (float)num37, (float)num38, 0f, 0, 0, 0);
					return;
				}
				case 21:
				case 90:
				{
					int num41 = this.reader.ReadInt16();
					Vector2 vector2 = this.reader.ReadVector2();
					Vector2 vector21 = this.reader.ReadVector2();
					int num42 = this.reader.ReadInt16();
					int num43 = this.reader.ReadByte();
					int num44 = this.reader.ReadByte();
					int num45 = this.reader.ReadInt16();
					if (Main.itemLockoutTime[num41] > 0)
					{
						return;
					}
					if (num45 != 0)
					{
						bool flag5 = false;
						if (num41 == 400)
						{
							flag5 = true;
						}
						if (flag5)
						{
							Item item5 = new Item();
							item5.netDefaults(num45);
							num41 = Item.NewItem((int)vector2.X, (int)vector2.Y, item5.width, item5.height, item5.type, num42, true, 0, false);
						}
						Item item6 = Main.item[num41];
						item6.netDefaults(num45);
						item6.Prefix(num43);
						item6.stack = num42;
						item6.position = vector2;
						item6.velocity = vector21;
						item6.active = true;
						item6.owner = Main.myPlayer;
						if (!flag5)
						{
							NetMessage.SendData(21, -1, this.whoAmI, "", num41, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
						NetMessage.SendData(21, -1, -1, "", num41, 0f, 0f, 0f, 0, 0, 0);
						if (num44 == 0)
						{
							Main.item[num41].ownIgnore = this.whoAmI;
							Main.item[num41].ownTime = 100;
						}
						Main.item[num41].FindOwner(num41);
						return;
					}
					if (num41 >= 400)
					{
						return;
					}
					Main.item[num41].active = false;
					NetMessage.SendData(21, -1, -1, "", num41, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 22:
				{
					int num46 = this.reader.ReadInt16();
					int num47 = this.reader.ReadByte();
					if (Main.item[num46].owner != this.whoAmI)
					{
						return;
					}
					Main.item[num46].owner = num47;
					if (num47 != Main.myPlayer)
					{
						Main.item[num46].keepTime = 0;
					}
					else
					{
						Main.item[num46].keepTime = 15;
					}
					Main.item[num46].owner = 255;
					Main.item[num46].keepTime = 15;
					NetMessage.SendData(22, -1, -1, "", num46, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 24:
				{
					int num55 = this.reader.ReadInt16();
					int num56 = this.reader.ReadByte();
					num56 = this.whoAmI;
					Player player6 = Main.player[num56];
					Main.npc[num55].StrikeNPC(player6.inventory[player6.selectedItem].damage, player6.inventory[player6.selectedItem].knockBack, player6.direction, false, false, false);
					NetMessage.SendData(24, -1, this.whoAmI, "", num55, (float)num56, 0f, 0f, 0, 0, 0);
					//NetMessage.SendData(23, -1, -1, "", num55, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 25:
				{
					int num57 = this.reader.ReadByte();
					num57 = this.whoAmI;
					Color color = this.reader.ReadRGB();
					color = new Color(255, 255, 255);
					string str = this.reader.ReadString();
					string lower = str.ToLower();
					if (lower == Lang.mp[6] || lower == Lang.mp[21])
					{
						string str2 = "";
						for (int p1 = 0; p1 < 255; p1++)
						{
							if (Main.player[p1].active)
							{
								str2 = (str2 != "" ? string.Concat(str2, ", ", Main.player[p1].name) : Main.player[p1].name);
							}
						}
						NetMessage.SendData(25, this.whoAmI, -1, string.Concat(Lang.mp[7], " ", str2, "."), 255, 255f, 240f, 20f, 0, 0, 0);
						return;
					}
					if (lower.StartsWith("/me "))
					{
						NetMessage.SendData(25, -1, -1, string.Concat("*", Main.player[this.whoAmI].name, " ", str.Substring(4)), 255, 200f, 100f, 0f, 0, 0, 0);
						return;
					}
					if (lower == Lang.mp[8])
					{
						object[] objArray = new object[] { "*", Main.player[this.whoAmI].name, " ", Lang.mp[9], " ", Main.rand.Next(1, 101) };
						NetMessage.SendData(25, -1, -1, string.Concat(objArray), 255, 255f, 240f, 20f, 0, 0, 0);
						return;
					}
					if (lower.StartsWith("/p "))
					{
						int num58 = Main.player[this.whoAmI].team;
						color = Main.teamColor[num58];
						if (num58 == 0)
						{
							NetMessage.SendData(25, this.whoAmI, -1, Lang.mp[10], 255, 255f, 240f, 20f, 0, 0, 0);
							return;
						}
						for (int q1 = 0; q1 < 255; q1++)
						{
							if (Main.player[q1].team == num58)
							{
								NetMessage.SendData(25, q1, -1, str.Substring(3), num57, (float)color.R, (float)color.G, (float)color.B, 0, 0, 0);
							}
						}
						return;
					}
					if (Main.player[this.whoAmI].difficulty == 2)
					{
						color = Main.hcColor;
					}
					else if (Main.player[this.whoAmI].difficulty == 1)
					{
						color = Main.mcColor;
					}
					NetMessage.SendData(25, -1, -1, str, num57, (float)color.R, (float)color.G, (float)color.B, 0, 0, 0);
					if (!Main.dedServ)
					{
						return;
					}
					Console.WriteLine(string.Concat("<", Main.player[this.whoAmI].name, "> ", str));
					return;
				}
				case 26:
				{
					int num59 = this.reader.ReadByte();
					if (this.whoAmI != num59 && (!Main.player[num59].hostile || !Main.player[this.whoAmI].hostile))
					{
						return;
					}
					int num60 = this.reader.ReadByte() - 1;
					int num61 = this.reader.ReadInt16();
					string str3 = this.reader.ReadString();
					BitsByte bitsByte12 = this.reader.ReadByte();
					bool flag6 = bitsByte12[0];
					bool item7 = bitsByte12[1];
					int cooldownCounter = bitsByte12[2] ? 0 : -1;
					if (bitsByte12[3])
					{
						cooldownCounter = 1;
					}
					Main.player[num59].Hurt(num61, num60, flag6, true, str3, item7, cooldownCounter);
					NetMessage.SendData(26, -1, this.whoAmI, str3, num59, (float)num60, (float)num61, (float)(flag6 ? 1 : 0), item7 ? 1 : 0, cooldownCounter, 0);
					return;
				}
				case 27:
				{
					int num67 = this.reader.ReadInt16();
					Vector2 vector24 = this.reader.ReadVector2();
					Vector2 vector25 = this.reader.ReadVector2();
					float single4 = this.reader.ReadSingle();
					int num68 = this.reader.ReadInt16();
					int num69 = this.reader.ReadByte();
					int num70 = this.reader.ReadInt16();
					BitsByte bitsByte13 = this.reader.ReadByte();
					float[] singleArray1 = new float[Projectile.maxAI];

					if (num70 < 0 || num70 >= Main.maxProjectileTypes)
						return;

					for (int s1 = 0; s1 < Projectile.maxAI; s1++)
					{
						if (!bitsByte13[s1])
						{
							singleArray1[s1] = 0f;
						}
						else
						{
							singleArray1[s1] = this.reader.ReadSingle();
						}
					}
					int num83 = (int)(bitsByte13[Projectile.maxAI] ? this.reader.ReadInt16() : -1);
					if (num83 >= 1000)
					{
						num83 = -1;
					}

					num69 = this.whoAmI;
					if (Main.projHostile[num70])
					{
						return;
					}
					int num71 = 1000;
					int num72 = 0;
					while (num72 < 1000)
					{
						if (Main.projectile[num72].owner != num69 || Main.projectile[num72].identity != num67 || !Main.projectile[num72].active)
						{
							num72++;
						}
						else
						{
							num71 = num72;
							break;
						}
					}
					if (num71 == 1000)
					{
						int num73 = 0;
						while (num73 < 1000)
						{
							if (Main.projectile[num73].active)
							{
								num73++;
							}
							else
							{
								num71 = num73;
								break;
							}
						}
					}
					Projectile projectile = Main.projectile[num71];
					if (!projectile.active || projectile.type != num70)
					{
						projectile.SetDefaults(num70);
						RemoteClient spamProjectile = Netplay.Clients[this.whoAmI];
						spamProjectile.SpamProjectile = spamProjectile.SpamProjectile + 1f;
					}
					projectile.identity = num67;
					projectile.position = vector24;
					projectile.velocity = vector25;
					projectile.type = num70;
					projectile.damage = num68;
					projectile.knockBack = single4;
					projectile.owner = num69;
					for (int t1 = 0; t1 < Projectile.maxAI; t1++)
					{
						projectile.ai[t1] = singleArray1[t1];
					}
					if (num83 >= 0)
					{
						projectile.projUUID = num83;
						Main.projectileIdentity[num69, num83] = num71;
					}
					projectile.ProjectileFixDesperation();
					NetMessage.SendData(27, -1, this.whoAmI, "", num71, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 28:
				{
					int num71 = this.reader.ReadInt16();
					int num72 = this.reader.ReadInt16();
					float single5 = this.reader.ReadSingle();
					int num73 = this.reader.ReadByte() - 1;
					byte num74 = this.reader.ReadByte();
					Main.npc[num71].PlayerInteraction(this.whoAmI);
					if (num72 < 0)
					{
						Main.npc[num71].life = 0;
						Main.npc[num71].HitEffect(0, 10);
						Main.npc[num71].active = false;
					}
					else
					{
						Main.npc[num71].StrikeNPC(num72, single5, num73, num74 == 1, false, true);
					}
					NetMessage.SendData(28, -1, this.whoAmI, "", num71, (float)num72, single5, (float)num73, (int)num74, 0, 0);
					if (Main.npc[num71].life > 0)
					{
						Main.npc[num71].netUpdate = true;
						return;
					}
					//NetMessage.SendData(23, -1, -1, "", num71, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 29:
				{
					int num75 = this.reader.ReadInt16();
					int num76 = this.reader.ReadByte();
					num76 = this.whoAmI;
					int num77 = 0;
					while (num77 < 1000)
					{
						if (Main.projectile[num77].owner != num76 || Main.projectile[num77].identity != num75 || !Main.projectile[num77].active)
						{
							num77++;
						}
						else
						{
							Main.projectile[num77].Kill();
							break;
						}
					}
					NetMessage.SendData(29, -1, this.whoAmI, "", num75, (float)num76, 0f, 0f, 0, 0, 0);
					return;
				}
				case 30:
				{
					int num78 = this.reader.ReadByte();
					num78 = this.whoAmI;
					bool flag7 = this.reader.ReadBoolean();
					Main.player[num78].hostile = flag7;
					NetMessage.SendData(30, -1, this.whoAmI, "", num78, 0f, 0f, 0f, 0, 0, 0);
					string str5 = string.Concat(" ", Lang.mp[(flag7 ? 11 : 12)]);
					Color color1 = Main.teamColor[Main.player[num78].team];
					NetMessage.SendData(25, -1, -1, string.Concat(Main.player[num78].name, str5), 255, (float)color1.R, (float)color1.G, (float)color1.B, 0, 0, 0);
					return;
				}
				case 31:
				{
					int num79 = this.reader.ReadInt16();
					int num80 = this.reader.ReadInt16();
					int num81 = Chest.FindChest(num79, num80);
					if (num81 <= -1 || Chest.UsingChest(num81) != -1)
					{
						return;
					}
					for (int t1 = 0; t1 < 40; t1++)
					{
						NetMessage.SendData(32, this.whoAmI, -1, "", num81, (float)t1, 0f, 0f, 0, 0, 0);
					}
					NetMessage.SendData(33, this.whoAmI, -1, "", num81, 0f, 0f, 0f, 0, 0, 0);
					Main.player[this.whoAmI].chest = num81;
					if (Main.myPlayer == this.whoAmI)
					{
						Main.recBigList = false;
					}
					Recipe.FindRecipes();
					NetMessage.SendData(80, -1, this.whoAmI, "", this.whoAmI, (float)num81, 0f, 0f, 0, 0, 0);

					if (Main.tile[num79, num80].frameX < 36 || Main.tile[num79, num80].frameX >= 72)
					{
						return;
					}
					AchievementsHelper.HandleSpecialEvent(Main.player[this.whoAmI], 16);
					return;
				}
				case 32:
				{
					int num82 = this.reader.ReadInt16();
					int num83 = this.reader.ReadByte();
					int num84 = this.reader.ReadInt16();
					int num85 = this.reader.ReadByte();
					int num86 = this.reader.ReadInt16();
					if (Main.chest[num82] == null)
					{
						Main.chest[num82] = new Chest(false);
					}
					if (Main.chest[num82].item[num83] == null)
					{
						Main.chest[num82].item[num83] = new Item();
					}
					Main.chest[num82].item[num83].netDefaults(num86);
					Main.chest[num82].item[num83].Prefix(num85);
					Main.chest[num82].item[num83].stack = num84;
					Recipe.FindRecipes();
					return;
				}
				case 33:
				{
					int chestID = this.reader.ReadInt16();
					int chestX = this.reader.ReadInt16();
					int chestY = this.reader.ReadInt16();
					int nameLen = this.reader.ReadByte();
					string chestName = string.Empty;
					if (nameLen != 0)
					{
						if (nameLen <= 20)
						{
							chestName = this.reader.ReadString();
						}
						else if (nameLen != 255)
						{
							nameLen = 0;
						}
					}
					if (nameLen != 0)
					{
						int num91 = Main.player[this.whoAmI].chest;
						Chest chest = Main.chest[num91];
						chest.name = chestName;
						//get chest name
						NetMessage.SendData(69, -1, this.whoAmI, chestName, num91, (float)chest.x, (float)chest.y, 0f, 0, 0, 0);
					}
					Main.player[this.whoAmI].chest = chestID;
					Recipe.FindRecipes();
					//sync player chest	index
					NetMessage.SendData(80, -1, this.whoAmI, "", this.whoAmI, (float)chestID, 0f, 0f, 0, 0, 0);
					return;
				}
				case 34:
				{
					byte num92 = this.reader.ReadByte();
					int num93 = this.reader.ReadInt16();
					int num94 = this.reader.ReadInt16();
					int num95 = this.reader.ReadInt16();

					if (num93 > Main.maxTilesX || num94 > Main.maxTilesY)
					{
						return;
					}

					if (num92 == 0)
					{
						int num97 = WorldGen.PlaceChest(num93, num94, 21, false, num95);
						if (num97 != -1)
						{
							NetMessage.SendData(34, -1, -1, "", (int)num92, (float)num93, (float)num94, (float)num95, num97, 0, 0);
							return;
						}
						NetMessage.SendData(34, this.whoAmI, -1, "", (int)num92, (float)num93, (float)num94, (float)num95, num97, 0, 0);
						Item.NewItem(num93 * 16, num94 * 16, 32, 32, Chest.chestItemSpawn[num95], 1, true, 0, false);
						return;
					}
					if (num92 == 2)
					{
						int num98 = WorldGen.PlaceChest(num93, num94, 88, false, num95);
						if (num98 != -1)
						{
							NetMessage.SendData(34, -1, -1, "", (int)num92, (float)num93, (float)num94, (float)num95, num98, 0, 0);
							return;
						}
						NetMessage.SendData(34, this.whoAmI, -1, "", (int)num92, (float)num93, (float)num94, (float)num95, num98, 0, 0);
						Item.NewItem(num93 * 16, num94 * 16, 32, 32, Chest.dresserItemSpawn[num95], 1, true, 0, false);
						return;
					}
					Tile tile3 = Main.tile[num93, num94];
					if (tile3.type != 21 || num92 != 1)
					{
						if (tile3.type != 88 || num92 != 3)
						{
							return;
						}
						num93 = num93 - tile3.frameX % 54 / 18;
						if (tile3.frameY % 36 != 0)
						{
							num94--;
						}
						int num99 = Chest.FindChest(num93, num94);
						WorldGen.KillTile(num93, num94, false, false, false);
						if (tile3.active())
						{
							return;
						}
						NetMessage.SendData(34, -1, -1, "", (int)num92, (float)num93, (float)num94, 0f, num99, 0, 0);
						return;
					}
					else
					{
						if (tile3.frameX % 36 != 0)
						{
							num93--;
						}
						if (tile3.frameY % 36 != 0)
						{
							num94--;
						}
						int num100 = Chest.FindChest(num93, num94);
						WorldGen.KillTile(num93, num94, false, false, false);
						if (tile3.active())
						{
							return;
						}
						NetMessage.SendData(34, -1, -1, "", (int)num92, (float)num93, (float)num94, 0f, num100, 0, 0);
						return;
					}
				}
				case 35:
				{
					int num101 = this.reader.ReadByte();
					num101 = this.whoAmI;
					int num102 = this.reader.ReadInt16();
					if (num101 != Main.myPlayer || Main.ServerSideCharacter)
					{
						Main.player[num101].HealEffect(num102, true);
					}
					NetMessage.SendData(35, -1, this.whoAmI, "", num101, (float)num102, 0f, 0f, 0, 0, 0);
					return;
				}
				case 36:
				{
					int num103 = this.reader.ReadByte();
					num103 = this.whoAmI;
					Player player8 = Main.player[num103];
					player8.zone1 = this.reader.ReadByte();
					player8.zone2 = this.reader.ReadByte();
					NetMessage.SendData(36, -1, this.whoAmI, "", num103, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 38:
				{
					if (this.reader.ReadString() != Netplay.ServerPassword)
					{
						NetMessage.SendData(2, this.whoAmI, -1, Lang.mp[1], 0, 0f, 0f, 0f, 0, 0, 0);
						return;
					}
					Netplay.Clients[this.whoAmI].State = 1;
					NetMessage.SendData(3, this.whoAmI, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 40:
				{
					int num105 = this.reader.ReadByte();
					num105 = this.whoAmI;
					int num106 = this.reader.ReadInt16();
					Main.player[num105].talkNPC = num106;
					NetMessage.SendData(40, -1, this.whoAmI, "", num105, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 41:
				{
					int num107 = this.reader.ReadByte();
					num107 = this.whoAmI;
					Player player9 = Main.player[num107];
					float single6 = this.reader.ReadSingle();
					int num108 = this.reader.ReadInt16();
					player9.itemRotation = single6;
					player9.itemAnimation = num108;
					player9.channel = player9.inventory[player9.selectedItem].channel;
					NetMessage.SendData(41, -1, this.whoAmI, "", num107, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 42:
				{
					int num109 = this.reader.ReadByte();
					num109 = this.whoAmI;
					if (Main.myPlayer == num109 && !Main.ServerSideCharacter)
					{
						return;
					}
					int num110 = this.reader.ReadInt16();
					int num111 = this.reader.ReadInt16();
					Main.player[num109].statMana = num110;
					Main.player[num109].statManaMax = num111;
					return;
				}
				case 43:
				{
					int num112 = this.reader.ReadByte();
					num112 = this.whoAmI;
					int num113 = this.reader.ReadInt16();
					if (num112 != Main.myPlayer)
					{
						Main.player[num112].ManaEffect(num113);
					}
					NetMessage.SendData(43, -1, this.whoAmI, "", num112, (float)num113, 0f, 0f, 0, 0, 0);
					return;
				}
				case 44:
				{
					int num114 = this.reader.ReadByte();
					num114 = this.whoAmI;
					int num115 = this.reader.ReadByte() - 1;
					int num116 = this.reader.ReadInt16();
					byte num117 = this.reader.ReadByte();
					string str6 = this.reader.ReadString();
					Main.player[num114].KillMe((double)num116, num115, num117 == 1, str6);
					NetMessage.SendData(44, -1, this.whoAmI, str6, num114, (float)num115, (float)num116, (float)num117, 0, 0, 0);
					return;
				}
				case 45:
				{
					int num118 = this.reader.ReadByte();
					num118 = this.whoAmI;
					int num119 = this.reader.ReadByte();
					Player player10 = Main.player[num118];
					int num120 = player10.team;
					player10.team = num119;
					Color color2 = Main.teamColor[num119];
					NetMessage.SendData(45, -1, this.whoAmI, "", num118, 0f, 0f, 0f, 0, 0, 0);
					string str7 = string.Concat(" ", Lang.mp[13 + num119]);
					if (num119 == 5)
					{
						str7 = string.Concat(" ", Lang.mp[22]);
					}
					for (int u1 = 0; u1 < 255; u1++)
					{
						if (u1 == this.whoAmI || num120 > 0 && Main.player[u1].team == num120 || num119 > 0 && Main.player[u1].team == num119)
						{
							NetMessage.SendData(25, u1, -1, string.Concat(player10.name, str7), 255, (float)color2.R, (float)color2.G, (float)color2.B, 0, 0, 0);
						}
					}
					return;
				}
				case 46:
				{
					int num121 = this.reader.ReadInt16();
					int num122 = this.reader.ReadInt16();
					int num123 = Sign.ReadSign(num121, num122, true);
					if (num123 < 0)
					{
						return;
					}
					NetMessage.SendData(47, this.whoAmI, -1, "", num123, (float)this.whoAmI, 0f, 0f, 0, 0, 0);
					return;
				}
				case 47:
				{
					int num124 = this.reader.ReadInt16();
					int num125 = this.reader.ReadInt16();
					int num126 = this.reader.ReadInt16();
					string str8 = this.reader.ReadString();
					string str9 = "";
					if (Main.sign[num124] != null)
					{
						str9 = Main.sign[num124].text;
					}
					Main.sign[num124] = new Sign();
					Main.sign[num124].x = num125;
					Main.sign[num124].y = num126;
					Sign.TextSign(num124, str8);
					int num127 = this.reader.ReadByte();
					if (str9 != str8)
					{
						num127 = this.whoAmI;
						NetMessage.SendData(47, -1, this.whoAmI, "", num124, (float)num127, 0f, 0f, 0, 0, 0);
					}
					return;

				}
				case 48:
				{
					int num128 = this.reader.ReadInt16();
					int num129 = this.reader.ReadInt16();
					byte num130 = this.reader.ReadByte();
					byte num131 = this.reader.ReadByte();
					if (Netplay.spamCheck)
					{
						int num132 = this.whoAmI;
						int x1 = (int)(Main.player[num132].position.X + (float)(Main.player[num132].width / 2));
						int y1 = (int)(Main.player[num132].position.Y + (float)(Main.player[num132].height / 2));
						int num133 = 10;
						int num134 = x1 - num133;
						int num135 = x1 + num133;
						int num136 = y1 - num133;
						int num137 = y1 + num133;
						if (num128 < num134 || num128 > num135 || num129 < num136 || num129 > num137)
						{
							NetMessage.BootPlayer(this.whoAmI, "Cheating attempt detected: Liquid spam");
							return;
						}
					}
					if (Main.tile[num128, num129] == null)
					{
						Main.tile[num128, num129] = new Tile();
					}
					lock (Main.tile[num128, num129])
					{
						Main.tile[num128, num129].liquid = num130;
						Main.tile[num128, num129].liquidType((int)num131);
						WorldGen.SquareTileFrame(num128, num129, true);
						return;
					}
				}
				case 49:
				{
					if (Netplay.Connection.State != 6)
					{
						return;
					}
					Netplay.Connection.State = 10;
					Main.ActivePlayerFileData.StartPlayTimer();
					Player.EnterWorld(Main.player[Main.myPlayer]);
					Main.player[Main.myPlayer].Spawn();
					return;
				}
				case 50:
				{
					int num138 = this.reader.ReadByte();
					num138 = this.whoAmI;
					Player player11 = Main.player[num138];
					for (int v1 = 0; v1 < 22; v1++)
					{
						player11.buffType[v1] = this.reader.ReadByte();
						if (player11.buffType[v1] <= 0)
						{
							player11.buffTime[v1] = 0;
						}
						else
						{
							player11.buffTime[v1] = 60;
						}
					}
					NetMessage.SendData(50, -1, this.whoAmI, "", num138, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 51:
				{
					byte num139 = this.reader.ReadByte();
					byte num140 = this.reader.ReadByte();
					if (num140 == 1)
					{
						NPC.SpawnSkeletron();
						return;
					}
					if (num140 == 2)
					{
						NetMessage.SendData(51, -1, this.whoAmI, "", (int)num139, (float)num140, 0f, 0f, 0, 0, 0);
						return;
					}
					if (num140 != 3)
					{
						if (num140 != 4)
						{
							return;
						}
						Main.npc[num139].BigMimicSpawnSmoke();
						return;
					}
					else
					{
						Main.Sundialing();
						return;
					}
				}
				case 52:
				{
					int ldap = (int)this.reader.ReadByte();
					int ad = (int)this.reader.ReadInt16();
					int winfs = (int)this.reader.ReadInt16();
					if (ldap == 1)
					{
						Chest.Unlock(ad, winfs);
						NetMessage.SendData(52, -1, this.whoAmI, "", 0, (float)ldap, (float)ad, (float)winfs, 0, 0, 0);
						NetMessage.SendTileSquare(-1, ad, winfs, 2);
					}
					if (ldap != 2)
					{
						return;
					}
					WorldGen.UnlockDoor(ad, winfs);
					NetMessage.SendData(52, -1, this.whoAmI, "", 0, (float)ldap, (float)ad, (float)winfs, 0, 0, 0);
					NetMessage.SendTileSquare(-1, ad, winfs, 2);
					return;
				}
				case 53:
				{
					int num145 = this.reader.ReadInt16();
					int num146 = this.reader.ReadByte();
					int num147 = this.reader.ReadInt16();

					Main.npc[num145].AddBuff(num146, num147, false);
					//NetMessage.SendData(54, -1, -1, "", num145, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 55:
				{
					int num149 = this.reader.ReadByte();
					int num150 = this.reader.ReadByte();
					int num151 = this.reader.ReadInt16();
					if (num149 != this.whoAmI && !Main.pvpBuff[num150])
					{
						return;
					}
					NetMessage.SendData(55, num149, -1, "", num149, (float)num150, (float)num151, 0f, 0, 0, 0);
					return;
				}
				case 56:
				{
					int num152 = this.reader.ReadInt16();
					if (num152 < 0 || num152 >= 200)
					{
						return;
					}
					NetMessage.SendData(56, this.whoAmI, -1, Main.npc[num152].displayName, num152, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 58:
				{
					int num153 = this.reader.ReadByte();
					num153 = this.whoAmI;
					float single7 = this.reader.ReadSingle();
					NetMessage.SendData(58, -1, this.whoAmI, "", this.whoAmI, single7, 0f, 0f, 0, 0, 0);
					return;
				}
				case 59:
				{
					int num155 = this.reader.ReadInt16();
					int num156 = this.reader.ReadInt16();
					if (num155 < 0 || num155 >= Main.maxTilesX)
						return;
					if (num156 < 0 || num156 >= Main.maxTilesY)
						return;
					if (Main.tile[num155, num156].type != 135)
					{
						Wiring.SetCurrentUser(this.whoAmI);
						Wiring.HitSwitch(num155, num156);
						Wiring.SetCurrentUser(-1);
					}
					NetMessage.SendData(59, -1, this.whoAmI, "", num155, (float)num156, 0f, 0f, 0, 0, 0);
					return;
				}
				case 60:
				{
					int num157 = this.reader.ReadInt16();
					int num158 = this.reader.ReadInt16();
					int num159 = this.reader.ReadInt16();
					byte num160 = this.reader.ReadByte();
					if (num157 >= 200)
					{
						NetMessage.BootPlayer(this.whoAmI, "cheating attempt detected: Invalid kick-out");
						return;
					}
					if (num160 == 0)
					{
						WorldGen.kickOut(num157);
						return;
					}
					WorldGen.moveRoom(num158, num159, num157);
					return;
				}
				case 61:
				{
					int num161 = this.reader.ReadInt16();
					int num162 = this.reader.ReadInt16();
					if (num162 >= 0 && num162 < 540 && NPCID.Sets.MPAllowedEnemies[num162])
					{
						if (num162 == 75)
						{
							for (int x11 = 0; x11 < 200; x11++)
							{
								if (!Main.npc[x11].townNPC)
								{
									Main.npc[x11].active = false;
								}
							}
						}
						if (NPC.AnyNPCs(num162))
						{
							return;
						}
						NPC.SpawnOnPlayer(num161, num162);
						return;
					}
					else if (num162 == -4)
					{
						if (Main.dayTime)
						{
							return;
						}
						NetMessage.SendData(25, -1, -1, Lang.misc[31], 255, 50f, 255f, 130f, 0, 0, 0);
						Main.startPumpkinMoon();
						NetMessage.SendData(7, -1, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.SendData(78, -1, -1, "", 0, 1f, 2f, 1f, 0, 0, 0);
						return;
					}
					else if (num162 == -5)
					{
						if (Main.dayTime)
						{
							return;
						}
						NetMessage.SendData(25, -1, -1, Lang.misc[34], 255, 50f, 255f, 130f, 0, 0, 0);
						Main.startSnowMoon();
						NetMessage.SendData(7, -1, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.SendData(78, -1, -1, "", 0, 1f, 1f, 1f, 0, 0, 0);
						return;
					}
					else if (num162 == -6)
					{
						if (Main.dayTime && !Main.eclipse)
						{
							NetMessage.SendData(25, -1, -1, Lang.misc[20], 255, 50f, 255f, 130f, 0, 0, 0);
							Main.eclipse = true;
							NetMessage.SendData(7, -1, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
						return;
					}
					else
					{
						if (num162 == -7)
						{
							NetMessage.SendData(25, -1, -1, "martian moon toggled", 255, 50f, 255f, 130f, 0, 0, 0);
							Main.invasionDelay = 0;
							Main.StartInvasion(4);
							NetMessage.SendData(7, -1, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
							NetMessage.SendData(78, -1, -1, "", 0, 1f, (float)(Main.invasionType + 2), 0f, 0, 0, 0);
							return;
						}
						if (num162 == -8)
						{
							if (NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
							{
								WorldGen.StartImpendingDoom();
								NetMessage.SendData(7, -1, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
								return;
							}
							return;
						}
						else
						{
							if (num162 < 0)
							{
								int num158 = 1;
								if (num162 > -5)
								{
									num158 = -num162;
								}
								if (num158 > 0 && Main.invasionType == 0)
								{
									Main.invasionDelay = 0;
									Main.StartInvasion(num158);
								}
								NetMessage.SendData(78, -1, -1, "", 0, 1f, (float)(Main.invasionType + 2), 0f, 0, 0, 0);
								return;
							}
							return;
						}
					}
				}
				case 62:
				{
					int num164 = this.reader.ReadByte();
					int num165 = this.reader.ReadByte();
					num164 = this.whoAmI;
					if (num165 == 1)
					{
						Main.player[num164].NinjaDodge();
					}
					if (num165 == 2)
					{
						Main.player[num164].ShadowDodge();
					}
					NetMessage.SendData(62, -1, this.whoAmI, "", num164, (float)num165, 0f, 0f, 0, 0, 0);
					return;
				}
				case 63:
				{
					int num166 = this.reader.ReadInt16();
					int num167 = this.reader.ReadInt16();
					byte num168 = this.reader.ReadByte();
					WorldGen.paintTile(num166, num167, num168, false);
					NetMessage.SendData(63, -1, this.whoAmI, "", num166, (float)num167, (float)num168, 0f, 0, 0, 0);
					return;
				}
				case 64:
				{
					int num169 = this.reader.ReadInt16();
					int num170 = this.reader.ReadInt16();
					byte num171 = this.reader.ReadByte();
					WorldGen.paintWall(num169, num170, num171, false);
					NetMessage.SendData(64, -1, this.whoAmI, "", num169, (float)num170, (float)num171, 0f, 0, 0, 0);
					return;
				}
				case 65:
				{
					BitsByte bitsByte14 = this.reader.ReadByte();
					int num172 = this.reader.ReadInt16();
					num172 = this.whoAmI;
					Vector2 vector26 = this.reader.ReadVector2();
					int num173 = 0;
					int num174 = 0;
					if (bitsByte14[0])
					{
						num173++;
					}
					if (bitsByte14[1])
					{
						num173 = num173 + 2;
					}
					if (bitsByte14[2])
					{
						num174++;
					}
					if (bitsByte14[3])
					{
						num174 = num174 + 2;
					}
					if (num173 == 0)
					{
						Main.player[num172].Teleport(vector26, num174, 0);
					}
					else if (num173 == 1)
					{
						Main.npc[num172].Teleport(vector26, num174, 0);
					}
					else if (num173 == 2)
					{
						Main.player[num172].Teleport(vector26, num174, 0);
						RemoteClient.CheckSection(this.whoAmI, vector26, 1);
						NetMessage.SendData(65, -1, -1, "", 0, (float)num172, vector26.X, vector26.Y, num174, 0, 0);
						int num175 = -1;
						float single8 = 9999f;
						for (int y11 = 0; y11 < 255; y11++)
						{
							if (Main.player[y11].active && y11 != this.whoAmI)
							{
								Vector2 vector27 = Main.player[y11].position - Main.player[this.whoAmI].position;
								if (vector27.Length() < single8)
								{
									single8 = vector27.Length();
									num175 = y11;
								}
							}
						}
						if (num175 >= 0)
						{
							NetMessage.SendData(25, -1, -1, string.Concat(Main.player[this.whoAmI].name, " has teleported to ", Main.player[num175].name), 255, 250f, 250f, 0f, 0, 0, 0);
						}
					}
					if (num173 != 0)
					{
						return;
					}
					NetMessage.SendData(65, -1, this.whoAmI, "", 0, (float)num172, vector26.X, vector26.Y, num174, 0, 0);
					return;
				}
				case 66:
				{
					int num176 = this.reader.ReadByte();
					int num177 = this.reader.ReadInt16();
					if (num177 <= 0)
					{
						return;
					}
					Player player13 = Main.player[num176];
					Player player14 = player13;
					player14.statLife = player14.statLife + num177;
					if (player13.statLife > player13.statLifeMax2)
					{
						player13.statLife = player13.statLifeMax2;
					}
					player13.HealEffect(num177, false);
					NetMessage.SendData(66, -1, this.whoAmI, "", num176, (float)num177, 0f, 0f, 0, 0, 0);
					return;
				}
				case 68:
				{
					this.reader.ReadString();
					return;
				}
				case 69:
				{
					int num178 = this.reader.ReadInt16();
					int num179 = this.reader.ReadInt16();
					int num180 = this.reader.ReadInt16();

					if (num178 < -1 || num178 >= 1000)
					{
						return;
					}
					if (num178 == -1)
					{
						num178 = Chest.FindChest(num179, num180);
						if (num178 == -1)
						{
							return;
						}
					}
					Chest chest2 = Main.chest[num178];
					if (chest2.x != num179 || chest2.y != num180)
					{
						return;
					}
					NetMessage.SendData(69, this.whoAmI, -1, chest2.name, num178, (float)num179, (float)num180, 0f, 0, 0, 0);
					return;
				}
				case 70:
				{
					int num181 = this.reader.ReadInt16();
					int num182 = this.reader.ReadByte();
					num182 = this.whoAmI;
					if (num181 < 200 && num181 >= 0)
					{
						NPC.CatchNPC(num181, num182);
						return;
					}
					return;
				}
				case 71:
				{
					int num183 = this.reader.ReadInt32();
					int num184 = this.reader.ReadInt32();
					int num185 = this.reader.ReadInt16();
					byte num186 = this.reader.ReadByte();
					NPC.ReleaseNPC(num183, num184, num185, (int)num186, this.whoAmI);
					return;
				}
				case 73:
				{
					Main.player[this.whoAmI].TeleportationPotion();
					return;
				}
				case 75:
				{
					string str10 = Main.player[this.whoAmI].name;
					if (Main.anglerWhoFinishedToday.Contains(str10))
					{
						return;
					}
					Main.anglerWhoFinishedToday.Add(str10);
					return;
				}
				case 76:
				{
					int num187 = this.reader.ReadByte();
					if (num187 == Main.myPlayer && !Main.ServerSideCharacter)
					{
						return;
					}
					num187 = this.whoAmI;
					Main.player[num187].anglerQuestsFinished = this.reader.ReadInt32();
					NetMessage.SendData(76, -1, this.whoAmI, "", num187, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 77:
				{
					short num188 = this.reader.ReadInt16();
					ushort num189 = this.reader.ReadUInt16();
					short num190 = this.reader.ReadInt16();
					Animation.NewTemporaryAnimation(num188, num189, num190, this.reader.ReadInt16());
					return;
				}
				case 79:
				{
					int num191 = this.reader.ReadInt16();
					int num192 = this.reader.ReadInt16();
					short num193 = this.reader.ReadInt16();
					int num194 = this.reader.ReadInt16();
					int num195 = this.reader.ReadByte();
					int num196 = this.reader.ReadSByte();
					num = (!this.reader.ReadBoolean() ? -1 : 1);
					RemoteClient remoteClient = Netplay.Clients[this.whoAmI];
					remoteClient.SpamAddBlock = remoteClient.SpamAddBlock + 1f;
					if (!Netplay.Clients[this.whoAmI].TileSections[Netplay.GetSectionX(num191), Netplay.GetSectionY(num192)])
					{
						return;
					}
					WorldGen.PlaceObject(num191, num192, num193, false, num194, num195, num196, num);
					NetMessage.SendObjectPlacment(this.whoAmI, num191, num192, num193, num194, num195, num196, num);
					return;
				}
				case 82:
				{
					NetManager.Instance.Read(this.reader, this.whoAmI);
					return;
				}
				case 84:
				{
					byte num203 = this.reader.ReadByte();
					float single9 = this.reader.ReadSingle();
					Main.player[num203].stealth = single9;
					NetMessage.SendData(84, -1, this.whoAmI, "", (int)num203, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 85:
				{
					int num204 = this.whoAmI;
					byte num205 = this.reader.ReadByte();
					if (num204 >= 255 || num205 >= 58)
					{
						return;
					}
					Chest.ServerPlaceItem(this.whoAmI, (int)num205);
					return;
				}
				case 86:
				{
					int num206 = this.reader.ReadInt32();
					if (this.reader.ReadBoolean())
					{
						TileEntity tileEntity1 = TileEntity.Read(this.reader, true);
						tileEntity1.ID = num206;
						TileEntity.ByID[tileEntity1.ID] = tileEntity1;
						TileEntity.ByPosition[tileEntity1.Position] = tileEntity1;
						return;
					}
					if (!TileEntity.ByID.TryGetValue(num206, out tileEntity) || !(tileEntity is TETrainingDummy) && !(tileEntity is TEItemFrame) && !(tileEntity is TELogicSensor))
					{
						return;
					}
					TileEntity.ByID.Remove(num206);
					TileEntity.ByPosition.Remove(tileEntity.Position);
					return;
				}
				case 89:
				{
					int num212 = this.reader.ReadInt16();
					int num213 = this.reader.ReadInt16();
					int num214 = this.reader.ReadInt16();
					int num215 = this.reader.ReadByte();
					int num216 = this.reader.ReadInt16();
					TEItemFrame.TryPlacing(num212, num213, num214, num215, num216);
					return;
				}
				case 92:
				{
					int num222 = this.reader.ReadInt16();
					float single10 = this.reader.ReadSingle();
					float single11 = this.reader.ReadSingle();
					float single12 = this.reader.ReadSingle();
					NPC nPC2 = Main.npc[num222];
					nPC2.extraValue = nPC2.extraValue + single10;
					NetMessage.SendData(92, -1, -1, "", num222, Main.npc[num222].extraValue, single11, single12, 0, 0, 0);
					return;
				}
				case 95:
				{
					ushort num223 = this.reader.ReadUInt16();
					if (num223 < 0 || num223 >= 1000)
					{
						return;
					}
					Projectile projectile1 = Main.projectile[num223];
					if (projectile1.type != 602)
					{
						return;
					}
					projectile1.Kill();
					NetMessage.SendData(29, -1, -1, "", projectile1.whoAmI, (float)projectile1.owner, 0f, 0f, 0, 0, 0);
					return;
				}
				case 96:
				{
					int num224 = this.reader.ReadByte();
					Player player15 = Main.player[num224];
					int num225 = this.reader.ReadInt16();
					Vector2 vector28 = this.reader.ReadVector2();
					Vector2 vector29 = this.reader.ReadVector2();
					player15.lastPortalColorIndex = num225 + (num225 % 2 == 0 ? 1 : -1);
					player15.Teleport(vector28, 4, num225);
					player15.velocity = vector29;
					return;
				}
				case 99:
				{
					int num226 = this.reader.ReadByte();
					num226 = this.whoAmI;
					Player player16 = Main.player[num226];
					player16.MinionTargetPoint = this.reader.ReadVector2();
					NetMessage.SendData(99, -1, this.whoAmI, "", num226, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				case 100:
				{
					int num227 = this.reader.ReadUInt16();
					NPC nPC3 = Main.npc[num227];
					int num228 = this.reader.ReadInt16();
					Vector2 vector210 = this.reader.ReadVector2();
					Vector2 vector211 = this.reader.ReadVector2();
					nPC3.lastPortalColorIndex = num228 + (num228 % 2 == 0 ? 1 : -1);
					nPC3.Teleport(vector210, 4, num228);
					nPC3.velocity = vector211;
					return;
				}
				case 102:
				{
					int num229 = this.reader.ReadByte();
					byte num230 = this.reader.ReadByte();
					Vector2 vector212 = this.reader.ReadVector2();
					num229 = this.whoAmI;
					NetMessage.SendData(102, -1, -1, "", num229, (float)num230, vector212.X, vector212.Y, 0, 0, 0);
					return;
				}
				case 105:
				{
					int i3 = (int)this.reader.ReadInt16();
					int j3 = (int)this.reader.ReadInt16();
					bool on = this.reader.ReadBoolean();
					WorldGen.ToggleGemLock(i3, j3, on);
					return;
				}
				case 109:
				{
					int num210 = (int)this.reader.ReadInt16();
					int num211 = (int)this.reader.ReadInt16();
					int num212 = (int)this.reader.ReadInt16();
					int num213 = (int)this.reader.ReadInt16();
					WiresUI.Settings.MultiToolMode toolMode = (WiresUI.Settings.MultiToolMode)this.reader.ReadByte();
					int num214 = this.whoAmI;
					WiresUI.Settings.MultiToolMode toolMode2 = WiresUI.Settings.ToolMode;
					WiresUI.Settings.ToolMode = toolMode;
					Wiring.MassWireOperation(new Point(num210, num211), new Point(num212, num213), Main.player[num214]);
					WiresUI.Settings.ToolMode = toolMode2;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		public void Reset()
		{
			this.readBuffer = new byte[131070];
			//this.writeBuffer = new byte[131070];
			this.writeLocked = false;
			this.messageLength = 0;
			this.totalData = 0;
			this.spamCount = 0;
			this.broadcast = false;
			this.checkBytes = false;
			this.ResetReader();
			this.ResetWriter();
		}

		public void ResetReader()
		{
			lock (this)
			{
				if (this.readerStream != null)
				{
					this.readerStream.Close();
				}
				this.readerStream = new MemoryStream(this.readBuffer);
				this.reader = new BinaryReader(this.readerStream);
			}
		}

		public void ResetWriter()
		{
			//lock (this)
			//{
			//	if (this.writerStream != null)
			//	{
			//		this.writerStream.Close();
			//	}
			//	this.writerStream = new MemoryStream(this.writeBuffer);
			//	this.writer = new BinaryWriter(this.writerStream);
			//}
		}
	}
}