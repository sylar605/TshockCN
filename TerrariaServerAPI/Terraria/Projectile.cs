
using System;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.UI;
using Terraria.ID;
using TerrariaApi.Server;
namespace Terraria
{
	public class Projectile : Entity
	{
		public bool arrow;
		public int numHits;
		public bool bobber;
		public bool netImportant;
		public bool noDropItem;
		public static int maxAI = 2;
		public bool counterweight;
		public float scale = 1f;
		public float rotation;
		public int type;
		public int alpha;
		public short glowMask;
		public int owner = 255;
		public float[] ai = new float[Projectile.maxAI];
		public float[] localAI = new float[Projectile.maxAI];
		public float gfxOffY;
		public float stepSpeed = 1f;
		public int aiStyle;
		public int timeLeft;
		public int soundDelay;
		public int damage;
		public int spriteDirection = 1;
		public bool hostile;
		public float knockBack;
		public bool friendly;
		public int penetrate = 1;
		private int[] npcImmune = new int[200];
		private bool updatedNPCImmunity;
		public int maxPenetrate = 1;
		public int identity;
		public float light;
		public bool netUpdate;
		public bool netUpdate2;
		public int netSpam;
		public Vector2[] oldPos = new Vector2[10];
		public float[] oldRot = new float[10];
		public int[] oldSpriteDirection = new int[10];
		public bool minion;
		public float minionSlots;
		public int minionPos;
		public int restrikeDelay;
		public bool tileCollide;
		public int extraUpdates;
		public int numUpdates;
		public bool ignoreWater;
		public bool hide;
		public bool ownerHitCheck;
		public int[] playerImmune = new int[255];
		public string miscText = "";
		public bool melee;
		public bool ranged;
		public bool thrown;
		public bool magic;
		public bool coldDamage;
		public bool noEnchantments;
		public bool trap;
		public bool npcProj;
		public int frameCounter;
		public int frame;
		public bool manualDirectionChange;
		public int projUUID = -1;
		private static float[] _CompanionCubeScreamCooldown = new float[255];
		public float Opacity
		{
			get
			{
				return 1f - (float)this.alpha / 255f;
			}
			set
			{
				this.alpha = (int)MathHelper.Clamp((1f - value) * 255f, 0f, 255f);
			}
		}
		public int MaxUpdates
		{
			get
			{
				return this.extraUpdates + 1;
			}
			set
			{
				this.extraUpdates = value - 1;
			}
		}
		public void SetDefaults(int Type)
		{
			this.counterweight = false;
			this.arrow = false;
			this.bobber = false;
			this.numHits = 0;
			this.netImportant = false;
			this.manualDirectionChange = false;
			int num = 10;
			if (Type >= 0)
			{
				num = ProjectileID.Sets.TrailCacheLength[Type];
			}
			if (num != this.oldPos.Length)
			{
				Array.Resize<Vector2>(ref this.oldPos, num);
				Array.Resize<float>(ref this.oldRot, num);
				Array.Resize<int>(ref this.oldSpriteDirection, num);
			}
			for (int i = 0; i < this.oldPos.Length; i++)
			{
				this.oldPos[i].X = 0f;
				this.oldPos[i].Y = 0f;
				this.oldRot[i] = 0f;
				this.oldSpriteDirection[i] = 0;
			}
			for (int j = 0; j < Projectile.maxAI; j++)
			{
				this.ai[j] = 0f;
				this.localAI[j] = 0f;
			}
			for (int k = 0; k < 255; k++)
			{
				this.playerImmune[k] = 0;
			}
			for (int l = 0; l < 200; l++)
			{
				this.npcImmune[l] = 0;
			}
			this.noDropItem = false;
			this.minion = false;
			this.minionSlots = 0f;
			this.soundDelay = 0;
			this.spriteDirection = 1;
			this.melee = false;
			this.ranged = false;
			this.thrown = false;
			this.magic = false;
			this.ownerHitCheck = false;
			this.hide = false;
			this.lavaWet = false;
			this.wetCount = 0;
			this.wet = false;
			this.ignoreWater = false;
			this.hostile = false;
			this.netUpdate = false;
			this.netUpdate2 = false;
			this.netSpam = 0;
			this.numUpdates = 0;
			this.extraUpdates = 0;
			this.identity = 0;
			this.restrikeDelay = 0;
			this.light = 0f;
			this.penetrate = 1;
			this.tileCollide = true;
			this.position = Vector2.Zero;
			this.velocity = Vector2.Zero;
			this.aiStyle = 0;
			this.alpha = 0;
			this.glowMask = -1;
			this.type = Type;
			this.active = true;
			this.rotation = 0f;
			this.scale = 1f;
			this.owner = 255;
			this.timeLeft = 3600;
			this.name = "";
			this.friendly = false;
			this.damage = 0;
			this.knockBack = 0f;
			this.miscText = "";
			this.coldDamage = false;
			this.noEnchantments = false;
			this.trap = false;
			this.npcProj = false;
			this.frame = 0;
			this.frameCounter = 0;
			if (this.type == 1)
			{
				this.arrow = true;
				this.name = "Wooden Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 2)
			{
				this.arrow = true;
				this.name = "Fire Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.light = 1f;
				this.ranged = true;
			}
			else if (this.type == 3)
			{
				this.name = "Shuriken";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 4;
				this.thrown = true;
			}
			else if (this.type == 4)
			{
				this.arrow = true;
				this.name = "Unholy Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.light = 0.35f;
				this.penetrate = 5;
				this.ranged = true;
			}
			else if (this.type == 5)
			{
				this.arrow = true;
				this.name = "Jester's Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.light = 0.4f;
				this.penetrate = -1;
				this.timeLeft = 120;
				this.alpha = 100;
				this.ignoreWater = true;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 6)
			{
				this.name = "Enchanted Boomerang";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 3;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.light = 0.4f;
			}
			else if (this.type == 7 || this.type == 8)
			{
				this.name = "Vilethorn";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 4;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.alpha = 255;
				this.ignoreWater = true;
				this.magic = true;
			}
			else if (this.type == 9)
			{
				this.name = "Starfury";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 5;
				this.friendly = true;
				this.penetrate = 2;
				this.alpha = 50;
				this.scale = 0.8f;
				this.tileCollide = false;
				this.melee = true;
			}
			else if (this.type == 10)
			{
				this.name = "Purification Powder";
				this.width = 64;
				this.height = 64;
				this.aiStyle = 6;
				this.friendly = true;
				this.tileCollide = false;
				this.penetrate = -1;
				this.alpha = 255;
				this.ignoreWater = true;
			}
			else if (this.type == 11)
			{
				this.name = "Vile Powder";
				this.width = 48;
				this.height = 48;
				this.aiStyle = 6;
				this.friendly = true;
				this.tileCollide = false;
				this.penetrate = -1;
				this.alpha = 255;
				this.ignoreWater = true;
			}
			else if (this.type == 12)
			{
				this.name = "Falling Star";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 5;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 50;
				this.light = 1f;
			}
			else if (this.type == 13)
			{
				this.netImportant = true;
				this.name = "Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 14)
			{
				this.name = "Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 15)
			{
				this.name = "Ball of Fire";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 8;
				this.friendly = true;
				this.light = 0.8f;
				this.alpha = 100;
				this.magic = true;
			}
			else if (this.type == 16)
			{
				this.name = "Magic Missile";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 9;
				this.friendly = true;
				this.light = 0.8f;
				this.alpha = 100;
				this.magic = true;
			}
			else if (this.type == 17)
			{
				this.name = "Dirt Ball";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.ignoreWater = true;
			}
			else if (this.type == 18)
			{
				this.netImportant = true;
				this.name = "Shadow Orb";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 11;
				this.friendly = true;
				this.light = 0.9f;
				this.alpha = 150;
				this.tileCollide = false;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.ignoreWater = true;
				this.scale = 0.8f;
			}
			else if (this.type == 19)
			{
				this.name = "Flamarang";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 3;
				this.friendly = true;
				this.penetrate = -1;
				this.light = 1f;
				this.melee = true;
			}
			else if (this.type == 20)
			{
				this.name = "Green Laser";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 3;
				this.light = 0.75f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.4f;
				this.timeLeft = 600;
				this.magic = true;
			}
			else if (this.type == 21)
			{
				this.name = "Bone";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 2;
				this.scale = 1.2f;
				this.friendly = true;
				this.thrown = true;
			}
			else if (this.type == 22)
			{
				this.name = "Water Stream";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 12;
				this.friendly = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.ignoreWater = true;
				this.magic = true;
			}
			else if (this.type == 23)
			{
				this.name = "Harpoon";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 13;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.ranged = true;
			}
			else if (this.type == 24)
			{
				this.name = "Spiky Ball";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 14;
				this.friendly = true;
				this.penetrate = 6;
				this.thrown = true;
			}
			else if (this.type == 25)
			{
				this.name = "Ball 'O Hurt";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 15;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 0.8f;
			}
			else if (this.type == 26)
			{
				this.name = "Blue Moon";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 15;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 0.8f;
			}
			else if (this.type == 27)
			{
				this.name = "Water Bolt";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 8;
				this.friendly = true;
				this.alpha = 255;
				this.timeLeft /= 2;
				this.penetrate = 10;
				this.magic = true;
			}
			else if (this.type == 28)
			{
				this.name = "Bomb";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
			}
			else if (this.type == 29)
			{
				this.name = "Dynamite";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
			}
			else if (this.type == 30)
			{
				this.name = "Grenade";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.thrown = true;
			}
			else if (this.type == 31)
			{
				this.name = "Sand Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 32)
			{
				this.name = "Ivy Whip";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 33)
			{
				this.name = "Thorn Chakram";
				this.width = 38;
				this.height = 38;
				this.aiStyle = 3;
				this.friendly = true;
				this.scale = 0.9f;
				this.penetrate = -1;
				this.melee = true;
			}
			else if (this.type == 34)
			{
				this.name = "Flamelash";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 9;
				this.friendly = true;
				this.light = 0.8f;
				this.alpha = 100;
				this.penetrate = 1;
				this.magic = true;
			}
			else if (this.type == 35)
			{
				this.name = "Sunfury";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 15;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 0.8f;
			}
			else if (this.type == 36)
			{
				this.name = "Meteor Shot";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 2;
				this.light = 0.6f;
				this.alpha = 255;
				this.scale = 1.4f;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 37)
			{
				this.name = "Sticky Bomb";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
			}
			else if (this.type == 38)
			{
				this.name = "Harpy Feather";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 0;
				this.hostile = true;
				this.penetrate = -1;
				this.aiStyle = 1;
				this.tileCollide = true;
			}
			else if (this.type == 39)
			{
				this.name = "Mud Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 40)
			{
				this.name = "Ash Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 41)
			{
				this.arrow = true;
				this.name = "Hellfire Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
				this.light = 0.3f;
			}
			else if (this.type == 42)
			{
				this.name = "Sand Ball";
				this.knockBack = 8f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 43)
			{
				this.name = "Tombstone";
				this.knockBack = 12f;
				this.width = 24;
				this.height = 24;
				this.aiStyle = 17;
				this.penetrate = -1;
			}
			else if (this.type == 44)
			{
				this.name = "Demon Sickle";
				this.width = 48;
				this.height = 48;
				this.alpha = 100;
				this.light = 0.2f;
				this.aiStyle = 18;
				this.hostile = true;
				this.penetrate = -1;
				this.tileCollide = true;
				this.scale = 0.9f;
			}
			else if (this.type == 45)
			{
				this.name = "Demon Scythe";
				this.width = 48;
				this.height = 48;
				this.alpha = 100;
				this.light = 0.2f;
				this.aiStyle = 18;
				this.friendly = true;
				this.penetrate = 5;
				this.tileCollide = true;
				this.scale = 0.9f;
				this.magic = true;
			}
			else if (this.type == 46)
			{
				this.name = "Dark Lance";
				this.width = 20;
				this.height = 20;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.1f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 47)
			{
				this.name = "Trident";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.1f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 48)
			{
				this.name = "Throwing Knife";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 2;
				this.thrown = true;
			}
			else if (this.type == 49)
			{
				this.name = "Spear";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.2f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 50)
			{
				this.netImportant = true;
				this.name = "Glowstick";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 14;
				this.penetrate = -1;
				this.alpha = 75;
				this.light = 1f;
				this.timeLeft *= 5;
			}
			else if (this.type == 51)
			{
				this.name = "Seed";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 1;
				this.friendly = true;
			}
			else if (this.type == 52)
			{
				this.name = "Wooden Boomerang";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 3;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
			}
			else if (this.type == 53)
			{
				this.netImportant = true;
				this.name = "Sticky Glowstick";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 14;
				this.penetrate = -1;
				this.alpha = 75;
				this.light = 1f;
				this.timeLeft *= 5;
				this.tileCollide = false;
			}
			else if (this.type == 54)
			{
				this.name = "Poisoned Knife";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 2;
				this.thrown = true;
			}
			else if (this.type == 55)
			{
				this.name = "Stinger";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 0;
				this.hostile = true;
				this.penetrate = -1;
				this.aiStyle = 1;
				this.tileCollide = true;
			}
			else if (this.type == 56)
			{
				this.name = "Ebonsand Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 57)
			{
				this.name = "Cobalt Chainsaw";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 58)
			{
				this.name = "Mythril Chainsaw";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 1.08f;
			}
			else if (this.type == 59)
			{
				this.name = "Cobalt Drill";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 0.9f;
			}
			else if (this.type == 60)
			{
				this.name = "Mythril Drill";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 0.9f;
			}
			else if (this.type == 61)
			{
				this.name = "Adamantite Chainsaw";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 1.16f;
			}
			else if (this.type == 62)
			{
				this.name = "Adamantite Drill";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 0.9f;
			}
			else if (this.type == 63)
			{
				this.name = "The Dao of Pow";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 15;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
			}
			else if (this.type == 64)
			{
				this.name = "Mythril Halberd";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.25f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 65)
			{
				this.name = "Ebonsand Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.penetrate = -1;
				this.extraUpdates = 1;
			}
			else if (this.type == 66)
			{
				this.name = "Adamantite Glaive";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.27f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 67)
			{
				this.name = "Pearl Sand Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 68)
			{
				this.name = "Pearl Sand Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.penetrate = -1;
				this.extraUpdates = 1;
			}
			else if (this.type == 69)
			{
				this.name = "Holy Water";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 1;
			}
			else if (this.type == 70)
			{
				this.name = "Unholy Water";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 1;
			}
			else if (this.type == 621)
			{
				this.name = "Blood Water";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 1;
			}
			else if (this.type == 71)
			{
				this.name = "Silt Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 72)
			{
				this.netImportant = true;
				this.name = "Blue Fairy";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 11;
				this.friendly = true;
				this.light = 0.9f;
				this.tileCollide = false;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.ignoreWater = true;
				this.scale = 0.8f;
			}
			else if (this.type == 73 || this.type == 74)
			{
				this.netImportant = true;
				this.name = "Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
				this.light = 0.4f;
			}
			else if (this.type == 75)
			{
				this.name = "Happy Bomb";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 16;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 76 || this.type == 77 || this.type == 78)
			{
				if (this.type == 76)
				{
					this.width = 10;
					this.height = 22;
				}
				else if (this.type == 77)
				{
					this.width = 18;
					this.height = 24;
				}
				else
				{
					this.width = 22;
					this.height = 24;
				}
				this.name = "Note";
				this.aiStyle = 21;
				this.friendly = true;
				this.ranged = true;
				this.alpha = 100;
				this.light = 0.3f;
				this.penetrate = -1;
				this.timeLeft = 180;
				this.magic = true;
			}
			else if (this.type == 79)
			{
				this.name = "Rainbow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 9;
				this.friendly = true;
				this.light = 0.8f;
				this.alpha = 255;
				this.magic = true;
			}
			else if (this.type == 80)
			{
				this.name = "Ice Block";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 22;
				this.friendly = true;
				this.magic = true;
				this.tileCollide = false;
				this.light = 0.5f;
				this.coldDamage = true;
			}
			else if (this.type == 81)
			{
				this.name = "Wooden Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.hostile = true;
				this.ranged = true;
			}
			else if (this.type == 82)
			{
				this.name = "Flaming Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.hostile = true;
				this.ranged = true;
			}
			else if (this.type == 83)
			{
				this.name = "Eye Laser";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = 3;
				this.light = 0.75f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.7f;
				this.timeLeft = 600;
				this.magic = true;
			}
			else if (this.type == 84)
			{
				this.name = "Pink Laser";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = 3;
				this.light = 0.75f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.magic = true;
			}
			else if (this.type == 85)
			{
				this.name = "Flames";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 23;
				this.friendly = true;
				this.alpha = 255;
				this.penetrate = 3;
				this.extraUpdates = 2;
				this.ranged = true;
			}
			else if (this.type == 86)
			{
				this.netImportant = true;
				this.name = "Pink Fairy";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 11;
				this.friendly = true;
				this.light = 0.9f;
				this.tileCollide = false;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.ignoreWater = true;
				this.scale = 0.8f;
			}
			else if (this.type == 87)
			{
				this.netImportant = true;
				this.name = "Pink Fairy";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 11;
				this.friendly = true;
				this.light = 0.9f;
				this.tileCollide = false;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.ignoreWater = true;
				this.scale = 0.8f;
			}
			else if (this.type == 88)
			{
				this.name = "Purple Laser";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 3;
				this.light = 0.75f;
				this.alpha = 255;
				this.extraUpdates = 4;
				this.scale = 1.4f;
				this.timeLeft = 600;
				this.magic = true;
			}
			else if (this.type == 89)
			{
				this.name = "Crystal Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 90)
			{
				this.name = "Crystal Shard";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 24;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 50;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.ranged = true;
				this.tileCollide = false;
			}
			else if (this.type == 91)
			{
				this.arrow = true;
				this.name = "Holy Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 92)
			{
				this.name = "Hallow Star";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 5;
				this.friendly = true;
				this.penetrate = 2;
				this.alpha = 50;
				this.scale = 0.8f;
				this.tileCollide = false;
				this.magic = true;
			}
			else if (this.type == 93)
			{
				this.light = 0.15f;
				this.name = "Magic Dagger";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 2;
				this.magic = true;
			}
			else if (this.type == 94)
			{
				this.ignoreWater = true;
				this.name = "Crystal Storm";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 24;
				this.friendly = true;
				this.light = 0.5f;
				this.alpha = 50;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.magic = true;
				this.tileCollide = true;
				this.penetrate = 1;
			}
			else if (this.type == 95)
			{
				this.name = "Cursed Flame";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 8;
				this.friendly = true;
				this.light = 0.8f;
				this.alpha = 100;
				this.magic = true;
				this.penetrate = 2;
			}
			else if (this.type == 96)
			{
				this.name = "Cursed Flame";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 8;
				this.hostile = true;
				this.light = 0.8f;
				this.alpha = 100;
				this.magic = true;
				this.penetrate = -1;
				this.scale = 0.9f;
				this.scale = 1.3f;
			}
			else if (this.type == 97)
			{
				this.name = "Cobalt Naginata";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.1f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 98)
			{
				this.name = "Poison Dart";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
				this.trap = true;
			}
			else if (this.type == 99)
			{
				this.name = "Boulder";
				this.width = 31;
				this.height = 31;
				this.aiStyle = 25;
				this.friendly = true;
				this.hostile = true;
				this.ranged = true;
				this.penetrate = -1;
				this.trap = true;
			}
			else if (this.type == 100)
			{
				this.name = "Death Laser";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = 3;
				this.light = 0.75f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.8f;
				this.timeLeft = 2700;
				this.magic = true;
			}
			else if (this.type == 101)
			{
				this.name = "Eye Fire";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 23;
				this.hostile = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 3;
				this.magic = true;
			}
			else if (this.type == 102)
			{
				this.name = "Bomb";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 16;
				this.hostile = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 103)
			{
				this.arrow = true;
				this.name = "Cursed Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.light = 1f;
				this.ranged = true;
			}
			else if (this.type == 104)
			{
				this.name = "Cursed Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 105)
			{
				this.name = "Gungnir";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.3f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 106)
			{
				this.name = "Light Disc";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 3;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.light = 0.4f;
			}
			else if (this.type == 107)
			{
				this.name = "Hamdrax";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 1.1f;
			}
			else if (this.type == 108)
			{
				this.name = "Explosives";
				this.width = 260;
				this.height = 260;
				this.aiStyle = 16;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.alpha = 255;
				this.timeLeft = 2;
				this.trap = true;
			}
			else if (this.type == 109)
			{
				this.name = "Snow Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.hostile = true;
				this.scale = 0.9f;
				this.penetrate = -1;
				this.coldDamage = true;
				this.thrown = true;
			}
			else if (this.type == 110)
			{
				this.name = "Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
				this.light = 0.5f;
				this.alpha = 255;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 111)
			{
				this.netImportant = true;
				this.name = "Bunny";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 112)
			{
				this.netImportant = true;
				this.name = "Penguin";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 113)
			{
				this.name = "Ice Boomerang";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 3;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.light = 0.4f;
				this.coldDamage = true;
			}
			else if (this.type == 114)
			{
				this.name = "Unholy Trident";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 27;
				this.magic = true;
				this.penetrate = 3;
				this.light = 0.5f;
				this.alpha = 255;
				this.friendly = true;
			}
			else if (this.type == 115)
			{
				this.name = "Unholy Trident";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 27;
				this.hostile = true;
				this.magic = true;
				this.penetrate = -1;
				this.light = 0.5f;
				this.alpha = 255;
			}
			else if (this.type == 116)
			{
				this.name = "Sword Beam";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 27;
				this.melee = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.friendly = true;
			}
			else if (this.type == 117)
			{
				this.arrow = true;
				this.name = "Bone Arrow";
				this.extraUpdates = 2;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 118)
			{
				this.name = "Ice Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 28;
				this.alpha = 255;
				this.melee = true;
				this.penetrate = 1;
				this.friendly = true;
				this.coldDamage = true;
			}
			else if (this.type == 119)
			{
				this.name = "Frost Bolt";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 28;
				this.alpha = 255;
				this.melee = true;
				this.penetrate = 2;
				this.friendly = true;
			}
			else if (this.type == 120)
			{
				this.arrow = true;
				this.name = "Frost Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
				this.coldDamage = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 121)
			{
				this.name = "Amethyst Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 1;
				this.friendly = true;
			}
			else if (this.type == 122)
			{
				this.name = "Topaz Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 1;
				this.friendly = true;
			}
			else if (this.type == 123)
			{
				this.name = "Sapphire Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 1;
				this.friendly = true;
			}
			else if (this.type == 124)
			{
				this.name = "Emerald Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 2;
				this.friendly = true;
			}
			else if (this.type == 125)
			{
				this.name = "Ruby Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 2;
				this.friendly = true;
			}
			else if (this.type == 126)
			{
				this.name = "Diamond Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 2;
				this.friendly = true;
			}
			else if (this.type == 127)
			{
				this.netImportant = true;
				this.name = "Turtle";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 128)
			{
				this.name = "Frost Blast";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 28;
				this.alpha = 255;
				this.penetrate = -1;
				this.friendly = false;
				this.hostile = true;
				this.coldDamage = true;
			}
			else if (this.type == 129)
			{
				this.name = "Rune Blast";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 28;
				this.alpha = 255;
				this.penetrate = -1;
				this.friendly = false;
				this.hostile = true;
				this.tileCollide = false;
			}
			else if (this.type == 130)
			{
				this.name = "Mushroom Spear";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.2f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 131)
			{
				this.name = "Mushroom";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 30;
				this.friendly = true;
				this.penetrate = 1;
				this.tileCollide = false;
				this.melee = true;
				this.light = 0.5f;
			}
			else if (this.type == 132)
			{
				this.name = "Terra Beam";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 27;
				this.melee = true;
				this.penetrate = 3;
				this.light = 0.5f;
				this.alpha = 255;
				this.friendly = true;
			}
			else if (this.type == 133)
			{
				this.name = "Grenade";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 134)
			{
				this.name = "Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 135)
			{
				this.name = "Proximity Mine";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 136)
			{
				this.name = "Grenade";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 137)
			{
				this.name = "Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 138)
			{
				this.name = "Proximity Mine";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 139)
			{
				this.name = "Grenade";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 140)
			{
				this.name = "Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 141)
			{
				this.name = "Proximity Mine";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 142)
			{
				this.name = "Grenade";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 143)
			{
				this.name = "Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 144)
			{
				this.name = "Proximity Mine";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 145)
			{
				this.name = "Pure Spray";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 31;
				this.friendly = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 146)
			{
				this.name = "Hallow Spray";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 31;
				this.friendly = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 147)
			{
				this.name = "Corrupt Spray";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 31;
				this.friendly = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 148)
			{
				this.name = "Mushroom Spray";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 31;
				this.friendly = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 149)
			{
				this.name = "Crimson Spray";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 31;
				this.friendly = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 150 || this.type == 151 || this.type == 152)
			{
				this.name = "Nettle Burst";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 4;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.alpha = 255;
				this.ignoreWater = true;
				this.magic = true;
			}
			else if (this.type == 153)
			{
				this.name = "The Rotted Fork";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.1f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 154)
			{
				this.name = "The Meatball";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 15;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 0.8f;
			}
			else if (this.type == 155)
			{
				this.netImportant = true;
				this.name = "Beach Ball";
				this.width = 44;
				this.height = 44;
				this.aiStyle = 32;
				this.friendly = true;
			}
			else if (this.type == 156)
			{
				this.name = "Light Beam";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 27;
				this.melee = true;
				this.light = 0.5f;
				this.alpha = 255;
				this.friendly = true;
			}
			else if (this.type == 157)
			{
				this.name = "Night Beam";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 27;
				this.melee = true;
				this.light = 0.5f;
				this.alpha = 255;
				this.friendly = true;
				this.scale = 1.2f;
			}
			else if (this.type == 158)
			{
				this.name = "Copper Coin";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 159)
			{
				this.name = "Silver Coin";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 160)
			{
				this.name = "Gold Coin";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 161)
			{
				this.name = "Platinum Coin";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 162)
			{
				this.name = "Cannonball";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 4;
				this.alpha = 255;
			}
			else if (this.type == 163)
			{
				this.netImportant = true;
				this.name = "Flare";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 33;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.timeLeft = 36000;
			}
			else if (this.type == 164)
			{
				this.name = "Landmine";
				this.width = 128;
				this.height = 128;
				this.aiStyle = 16;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.alpha = 255;
				this.timeLeft = 2;
			}
			else if (this.type == 165)
			{
				this.netImportant = true;
				this.name = "Web";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 166)
			{
				this.name = "Snow Ball";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 2;
				this.friendly = true;
				this.ranged = true;
				this.coldDamage = true;
			}
			else if (this.type == 167 || this.type == 168 || this.type == 169 || this.type == 170)
			{
				this.name = "Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 34;
				this.friendly = true;
				this.ranged = true;
				this.timeLeft = 45;
			}
			else if (this.type == 171 || this.type == 505 || this.type == 506)
			{
				this.name = "Rope Coil";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 35;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft = 400;
			}
			else if (this.type == 172)
			{
				this.arrow = true;
				this.name = "Frostburn Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.light = 1f;
				this.ranged = true;
				this.coldDamage = true;
			}
			else if (this.type == 173)
			{
				this.name = "Enchanted Beam";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 27;
				this.melee = true;
				this.penetrate = 1;
				this.light = 0.2f;
				this.alpha = 255;
				this.friendly = true;
			}
			else if (this.type == 174)
			{
				this.name = "Ice Spike";
				this.alpha = 255;
				this.width = 6;
				this.height = 6;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
				this.coldDamage = true;
			}
			else if (this.type == 175)
			{
				this.name = "Baby Eater";
				this.width = 34;
				this.height = 34;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 176)
			{
				this.name = "Jungle Spike";
				this.alpha = 255;
				this.width = 6;
				this.height = 6;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 177)
			{
				this.name = "Icewater Spit";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 28;
				this.alpha = 255;
				this.penetrate = -1;
				this.friendly = false;
				this.hostile = true;
				this.coldDamage = true;
			}
			else if (this.type == 178)
			{
				this.name = "Confetti";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.alpha = 255;
				this.penetrate = -1;
				this.timeLeft = 2;
			}
			else if (this.type == 179)
			{
				this.name = "Slush Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 180)
			{
				this.name = "Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
				this.light = 0.5f;
				this.alpha = 255;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 181)
			{
				this.name = "Bee";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 36;
				this.friendly = true;
				this.penetrate = 3;
				this.alpha = 255;
				this.timeLeft = 600;
				this.extraUpdates = 3;
			}
			else if (this.type == 182)
			{
				this.light = 0.15f;
				this.name = "Possessed Hatchet";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 3;
				this.friendly = true;
				this.penetrate = 10;
				this.melee = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 183)
			{
				this.name = "Beenade";
				this.width = 14;
				this.height = 22;
				this.aiStyle = 14;
				this.penetrate = 1;
				this.ranged = true;
				this.timeLeft = 180;
				this.thrown = true;
				this.friendly = true;
			}
			else if (this.type == 184)
			{
				this.name = "Poison Dart";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 1;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
				this.trap = true;
			}
			else if (this.type == 185)
			{
				this.name = "Spiky Ball";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 14;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 900;
				this.trap = true;
			}
			else if (this.type == 186)
			{
				this.name = "Spear";
				this.width = 10;
				this.height = 14;
				this.aiStyle = 37;
				this.friendly = true;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 300;
				this.trap = true;
			}
			else if (this.type == 187)
			{
				this.name = "Flamethrower";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 38;
				this.alpha = 255;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.timeLeft = 60;
				this.trap = true;
			}
			else if (this.type == 188)
			{
				this.name = "Flames";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 23;
				this.friendly = true;
				this.hostile = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.trap = true;
			}
			else if (this.type == 189)
			{
				this.name = "Wasp";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 36;
				this.friendly = true;
				this.penetrate = 4;
				this.alpha = 255;
				this.timeLeft = 600;
				this.magic = true;
				this.extraUpdates = 3;
			}
			else if (this.type == 190)
			{
				this.name = "Mechanical Piranha";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 39;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.ranged = true;
			}
			else if (this.type >= 191 && this.type <= 194)
			{
				this.netImportant = true;
				this.name = "Pygmy";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 26;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 1f;
				if (this.type == 192)
				{
					this.scale = 1.025f;
				}
				if (this.type == 193)
				{
					this.scale = 1.05f;
				}
				if (this.type == 194)
				{
					this.scale = 1.075f;
				}
			}
			else if (this.type == 195)
			{
				this.tileCollide = false;
				this.name = "Pygmy";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
			}
			else if (this.type == 196)
			{
				this.name = "Smoke Bomb";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 14;
				this.penetrate = -1;
				this.scale = 0.8f;
			}
			else if (this.type == 197)
			{
				this.netImportant = true;
				this.name = "Baby Skeletron Head";
				this.width = 42;
				this.height = 42;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 198)
			{
				this.netImportant = true;
				this.name = "Baby Hornet";
				this.width = 26;
				this.height = 26;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 199)
			{
				this.netImportant = true;
				this.name = "Tiki Spirit";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 200)
			{
				this.netImportant = true;
				this.name = "Pet Lizard";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 201)
			{
				this.name = "Tombstone";
				this.knockBack = 12f;
				this.width = 24;
				this.height = 24;
				this.aiStyle = 17;
				this.penetrate = -1;
			}
			else if (this.type == 202)
			{
				this.name = "Tombstone";
				this.knockBack = 12f;
				this.width = 24;
				this.height = 24;
				this.aiStyle = 17;
				this.penetrate = -1;
			}
			else if (this.type == 203)
			{
				this.name = "Tombstone";
				this.knockBack = 12f;
				this.width = 24;
				this.height = 24;
				this.aiStyle = 17;
				this.penetrate = -1;
			}
			else if (this.type == 204)
			{
				this.name = "Tombstone";
				this.knockBack = 12f;
				this.width = 24;
				this.height = 24;
				this.aiStyle = 17;
				this.penetrate = -1;
			}
			else if (this.type == 205)
			{
				this.name = "Tombstone";
				this.knockBack = 12f;
				this.width = 24;
				this.height = 24;
				this.aiStyle = 17;
				this.penetrate = -1;
			}
			else if (this.type == 206)
			{
				this.name = "Leaf";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 40;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 600;
				this.magic = true;
			}
			else if (this.type == 207)
			{
				this.name = "Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 208)
			{
				this.netImportant = true;
				this.name = "Parrot";
				this.width = 18;
				this.height = 36;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 209)
			{
				this.name = "Truffle";
				this.width = 12;
				this.height = 32;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.light = 0.5f;
			}
			else if (this.type == 210)
			{
				this.netImportant = true;
				this.name = "Sapling";
				this.width = 14;
				this.height = 30;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 211)
			{
				this.netImportant = true;
				this.name = "Wisp";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.light = 1f;
				this.ignoreWater = true;
			}
			else if (this.type == 212)
			{
				this.name = "Palladium Pike";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.12f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 213)
			{
				this.name = "Palladium Drill";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 0.92f;
			}
			else if (this.type == 214)
			{
				this.name = "Palladium Chainsaw";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 215)
			{
				this.name = "Orichalcum Halberd";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.27f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 216)
			{
				this.name = "Orichalcum Drill";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 0.93f;
			}
			else if (this.type == 217)
			{
				this.name = "Orichalcum Chainsaw";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 1.12f;
			}
			else if (this.type == 218)
			{
				this.name = "Titanium Trident";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.28f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 219)
			{
				this.name = "Titanium Drill";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 0.95f;
			}
			else if (this.type == 220)
			{
				this.name = "Titanium Chainsaw";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 1.2f;
			}
			else if (this.type == 221)
			{
				this.name = "Flower Petal";
				this.width = 20;
				this.height = 20;
				this.aiStyle = 41;
				this.friendly = true;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.timeLeft = 120;
				this.penetrate = -1;
				this.scale = 1f + (float)Main.rand.Next(30) * 0.01f;
				this.extraUpdates = 2;
			}
			else if (this.type == 222)
			{
				this.name = "Chlorophyte Partisan";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.3f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 223)
			{
				this.name = "Chlorophyte Drill";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 1f;
			}
			else if (this.type == 224)
			{
				this.name = "Chlorophyte Chainsaw";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 1.1f;
			}
			else if (this.type == 225)
			{
				this.arrow = true;
				this.penetrate = 2;
				this.name = "Chlorophyte Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 226)
			{
				this.netImportant = true;
				this.name = "Crystal Leaf";
				this.width = 22;
				this.height = 42;
				this.aiStyle = 42;
				this.friendly = true;
				this.tileCollide = false;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.light = 0.4f;
				this.ignoreWater = true;
			}
			else if (this.type == 227)
			{
				this.netImportant = true;
				this.tileCollide = false;
				this.light = 0.1f;
				this.name = "Crystal Leaf";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 43;
				this.friendly = true;
				this.penetrate = 1;
				this.timeLeft = 180;
			}
			else if (this.type == 228)
			{
				this.tileCollide = false;
				this.name = "Spore Cloud";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 44;
				this.friendly = true;
				this.scale = 1.1f;
				this.penetrate = -1;
			}
			else if (this.type == 229)
			{
				this.name = "Chlorophyte Orb";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 44;
				this.friendly = true;
				this.penetrate = -1;
				this.light = 0.2f;
			}
			else if (this.type >= 230 && this.type <= 235)
			{
				this.netImportant = true;
				this.name = "Gem Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 236)
			{
				this.netImportant = true;
				this.name = "Baby Dino";
				this.width = 34;
				this.height = 34;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 237)
			{
				this.netImportant = true;
				this.name = "Rain Cloud";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 45;
				this.penetrate = -1;
			}
			else if (this.type == 238)
			{
				this.tileCollide = false;
				this.ignoreWater = true;
				this.name = "Rain Cloud";
				this.width = 54;
				this.height = 28;
				this.aiStyle = 45;
				this.penetrate = -1;
			}
			else if (this.type == 239)
			{
				this.ignoreWater = true;
				this.name = "Rain";
				this.width = 4;
				this.height = 40;
				this.aiStyle = 45;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft = 300;
				this.scale = 1.1f;
				this.magic = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 240)
			{
				this.name = "Cannonball";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 2;
				this.hostile = true;
				this.penetrate = -1;
				this.alpha = 255;
			}
			else if (this.type == 241)
			{
				this.name = "Crimsand Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 242)
			{
				this.name = "Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.extraUpdates = 7;
				this.scale = 1.18f;
				this.timeLeft = 600;
				this.ranged = true;
				this.ignoreWater = true;
			}
			else if (this.type == 243)
			{
				this.name = "Blood Cloud";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 45;
				this.penetrate = -1;
			}
			else if (this.type == 244)
			{
				this.tileCollide = false;
				this.ignoreWater = true;
				this.name = "Blood Cloud";
				this.width = 54;
				this.height = 28;
				this.aiStyle = 45;
				this.penetrate = -1;
			}
			else if (this.type == 245)
			{
				this.ignoreWater = true;
				this.name = "Blood Rain";
				this.width = 4;
				this.height = 40;
				this.aiStyle = 45;
				this.friendly = true;
				this.penetrate = 2;
				this.timeLeft = 300;
				this.scale = 1.1f;
				this.magic = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 246)
			{
				this.name = "Stynger";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
				this.alpha = 255;
				this.extraUpdates = 1;
			}
			else if (this.type == 247)
			{
				this.name = "Flower Pow";
				this.width = 34;
				this.height = 34;
				this.aiStyle = 15;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
			}
			else if (this.type == 248)
			{
				this.name = "Flower Pow";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 1;
				this.friendly = true;
				this.melee = true;
			}
			else if (this.type == 249)
			{
				this.name = "Stynger";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 2;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 250)
			{
				this.name = "Rainbow";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 46;
				this.penetrate = -1;
				this.magic = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.scale = 1.25f;
			}
			else if (this.type == 251)
			{
				this.name = "Rainbow";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 46;
				this.friendly = true;
				this.penetrate = -1;
				this.magic = true;
				this.alpha = 255;
				this.light = 0.3f;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.scale = 1.25f;
			}
			else if (this.type == 252)
			{
				this.name = "Chlorophyte Jackhammer";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 1.1f;
			}
			else if (this.type == 253)
			{
				this.name = "Ball of Frost";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 8;
				this.friendly = true;
				this.light = 0.8f;
				this.alpha = 100;
				this.magic = true;
			}
			else if (this.type == 254)
			{
				this.name = "Magnet Sphere";
				this.width = 38;
				this.height = 38;
				this.aiStyle = 47;
				this.magic = true;
				this.timeLeft = 660;
				this.light = 0.5f;
			}
			else if (this.type == 255)
			{
				this.name = "Magnet Sphere";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 48;
				this.friendly = true;
				this.magic = true;
				this.extraUpdates = 100;
				this.timeLeft = 100;
			}
			else if (this.type == 256)
			{
				this.netImportant = true;
				this.tileCollide = false;
				this.name = "Skeletron Hand";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.scale = 1f;
				this.timeLeft *= 10;
			}
			else if (this.type == 257)
			{
				this.name = "Frost Beam";
				this.ignoreWater = true;
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
				this.light = 0.75f;
				this.alpha = 255;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.magic = true;
				this.coldDamage = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 258)
			{
				this.name = "Fireball";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 8;
				this.hostile = true;
				this.penetrate = -1;
				this.alpha = 100;
				this.timeLeft = 300;
			}
			else if (this.type == 259)
			{
				this.name = "Eye Beam";
				this.ignoreWater = true;
				this.tileCollide = false;
				this.width = 8;
				this.height = 8;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
				this.light = 0.3f;
				this.scale = 1.1f;
				this.magic = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 260)
			{
				this.name = "Heat Ray";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 48;
				this.friendly = true;
				this.magic = true;
				this.extraUpdates = 100;
				this.timeLeft = 200;
				this.penetrate = -1;
			}
			else if (this.type == 261)
			{
				this.name = "Boulder";
				this.width = 32;
				this.height = 34;
				this.aiStyle = 14;
				this.friendly = true;
				this.penetrate = 6;
				this.magic = true;
				this.ignoreWater = true;
			}
			else if (this.type == 262)
			{
				this.name = "Golem Fist";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 13;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.melee = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 263)
			{
				this.name = "Ice Sickle";
				this.width = 34;
				this.height = 34;
				this.alpha = 100;
				this.light = 0.5f;
				this.aiStyle = 18;
				this.friendly = true;
				this.penetrate = 5;
				this.tileCollide = true;
				this.scale = 1f;
				this.melee = true;
				this.timeLeft = 180;
				this.coldDamage = true;
			}
			else if (this.type == 264)
			{
				this.ignoreWater = true;
				this.name = "Rain";
				this.width = 4;
				this.height = 40;
				this.aiStyle = 45;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 120;
				this.scale = 1.1f;
				this.extraUpdates = 1;
			}
			else if (this.type == 265)
			{
				this.name = "Poison Fang";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 1;
				this.alpha = 255;
				this.friendly = true;
				this.magic = true;
				this.penetrate = 4;
			}
			else if (this.type == 266)
			{
				this.netImportant = true;
				this.alpha = 75;
				this.name = "Baby Slime";
				this.width = 24;
				this.height = 16;
				this.aiStyle = 26;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 1f;
			}
			else if (this.type == 267)
			{
				this.alpha = 255;
				this.name = "Poison Dart";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 268)
			{
				this.netImportant = true;
				this.name = "Eye Spring";
				this.width = 18;
				this.height = 32;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 269)
			{
				this.netImportant = true;
				this.name = "Baby Snowman";
				this.width = 20;
				this.height = 26;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 270)
			{
				this.name = "Skull";
				this.width = 26;
				this.height = 26;
				this.aiStyle = 1;
				this.alpha = 255;
				this.friendly = true;
				this.magic = true;
				this.penetrate = 3;
			}
			else if (this.type == 271)
			{
				this.name = "Boxing Glove";
				this.width = 20;
				this.height = 20;
				this.aiStyle = 13;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.melee = true;
				this.scale = 1.2f;
			}
			else if (this.type == 272)
			{
				this.name = "Bananarang";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 3;
				this.friendly = true;
				this.scale = 0.9f;
				this.penetrate = -1;
				this.melee = true;
			}
			else if (this.type == 273)
			{
				this.name = "Chain Knife";
				this.width = 26;
				this.height = 26;
				this.aiStyle = 13;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.melee = true;
			}
			else if (this.type == 274)
			{
				this.name = "Death Sickle";
				this.width = 42;
				this.height = 42;
				this.alpha = 100;
				this.light = 0.5f;
				this.aiStyle = 18;
				this.friendly = true;
				this.penetrate = 5;
				this.tileCollide = false;
				this.scale = 1.1f;
				this.melee = true;
				this.timeLeft = 180;
			}
			else if (this.type == 275)
			{
				this.alpha = 255;
				this.name = "Seed";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.hostile = true;
			}
			else if (this.type == 276)
			{
				this.alpha = 255;
				this.name = "Poison Seed";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.hostile = true;
			}
			else if (this.type == 277)
			{
				this.alpha = 255;
				this.name = "Thorn Ball";
				this.width = 38;
				this.height = 38;
				this.aiStyle = 14;
				this.hostile = true;
			}
			else if (this.type == 278)
			{
				this.arrow = true;
				this.name = "Ichor Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.light = 1f;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 279)
			{
				this.name = "Ichor Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.25f;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 280)
			{
				this.name = "Golden Shower";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 12;
				this.friendly = true;
				this.alpha = 255;
				this.penetrate = 5;
				this.extraUpdates = 2;
				this.ignoreWater = true;
				this.magic = true;
			}
			else if (this.type == 281)
			{
				this.name = "Explosive Bunny";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 49;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 600;
			}
			else if (this.type == 282)
			{
				this.arrow = true;
				this.name = "Venom Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 283)
			{
				this.name = "Venom Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.25f;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 284)
			{
				this.name = "Party Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.3f;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 285)
			{
				this.name = "Nano Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.3f;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 286)
			{
				this.name = "Explosive Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.3f;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 287)
			{
				this.name = "Golden Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.light = 0.5f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.3f;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 288)
			{
				this.name = "Golden Shower";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 12;
				this.hostile = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.ignoreWater = true;
				this.magic = true;
			}
			else if (this.type == 289)
			{
				this.name = "Confetti";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.alpha = 255;
				this.penetrate = -1;
				this.timeLeft = 2;
			}
			else if (this.type == 290)
			{
				this.name = "Shadow Beam";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 48;
				this.hostile = true;
				this.magic = true;
				this.extraUpdates = 100;
				this.timeLeft = 100;
				this.penetrate = -1;
			}
			else if (this.type == 291)
			{
				this.name = "Inferno";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 50;
				this.hostile = true;
				this.alpha = 255;
				this.magic = true;
				this.tileCollide = false;
				this.penetrate = -1;
			}
			else if (this.type == 292)
			{
				this.name = "Inferno";
				this.width = 130;
				this.height = 130;
				this.aiStyle = 50;
				this.hostile = true;
				this.alpha = 255;
				this.magic = true;
				this.tileCollide = false;
				this.penetrate = -1;
			}
			else if (this.type == 293)
			{
				this.name = "Lost Soul";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 51;
				this.hostile = true;
				this.alpha = 255;
				this.magic = true;
				this.tileCollide = false;
				this.penetrate = -1;
				this.extraUpdates = 1;
			}
			else if (this.type == 294)
			{
				this.name = "Shadow Beam";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 48;
				this.friendly = true;
				this.magic = true;
				this.extraUpdates = 100;
				this.timeLeft = 300;
				this.penetrate = -1;
			}
			else if (this.type == 295)
			{
				this.name = "Inferno";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 50;
				this.friendly = true;
				this.alpha = 255;
				this.magic = true;
				this.tileCollide = true;
			}
			else if (this.type == 296)
			{
				this.name = "Inferno";
				this.width = 150;
				this.height = 150;
				this.aiStyle = 50;
				this.friendly = true;
				this.alpha = 255;
				this.magic = true;
				this.tileCollide = false;
				this.penetrate = -1;
			}
			else if (this.type == 297)
			{
				this.name = "Lost Soul";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 51;
				this.friendly = true;
				this.alpha = 255;
				this.magic = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 298)
			{
				this.name = "Spirit Heal";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 52;
				this.alpha = 255;
				this.magic = true;
				this.tileCollide = false;
				this.extraUpdates = 3;
			}
			else if (this.type == 299)
			{
				this.name = "Shadowflames";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 1;
				this.hostile = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.magic = true;
				this.ignoreWater = true;
				this.tileCollide = false;
			}
			else if (this.type == 300)
			{
				this.name = "Paladin's Hammer";
				this.width = 38;
				this.height = 38;
				this.aiStyle = 2;
				this.hostile = true;
				this.penetrate = -1;
				this.ignoreWater = true;
				this.tileCollide = false;
			}
			else if (this.type == 301)
			{
				this.name = "Paladin's Hammer";
				this.width = 38;
				this.height = 38;
				this.aiStyle = 3;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.extraUpdates = 2;
			}
			else if (this.type == 302)
			{
				this.name = "Sniper Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
				this.light = 0.3f;
				this.alpha = 255;
				this.extraUpdates = 7;
				this.scale = 1.18f;
				this.timeLeft = 300;
				this.ranged = true;
				this.ignoreWater = true;
			}
			else if (this.type == 303)
			{
				this.name = "Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.hostile = true;
				this.penetrate = -1;
				this.ranged = true;
			}
			else if (this.type == 304)
			{
				this.name = "Vampire Knife";
				this.alpha = 255;
				this.width = 30;
				this.height = 30;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 1;
				this.melee = true;
				this.light = 0.2f;
				this.ignoreWater = true;
				this.extraUpdates = 0;
			}
			else if (this.type == 305)
			{
				this.name = "Vampire Heal";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 52;
				this.alpha = 255;
				this.tileCollide = false;
				this.extraUpdates = 10;
			}
			else if (this.type == 306)
			{
				this.name = "Eater's Bite";
				this.alpha = 255;
				this.width = 14;
				this.height = 14;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 1;
				this.melee = true;
				this.ignoreWater = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 307)
			{
				this.name = "Tiny Eater";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 36;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 600;
				this.melee = true;
				this.extraUpdates = 3;
			}
			else if (this.type == 308)
			{
				this.name = "Frost Hydra";
				this.width = 80;
				this.height = 74;
				this.aiStyle = 53;
				this.timeLeft = 7200;
				this.light = 0.25f;
				this.ignoreWater = true;
				this.coldDamage = true;
			}
			else if (this.type == 309)
			{
				this.name = "Frost Blast";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 28;
				this.alpha = 255;
				this.penetrate = 1;
				this.friendly = true;
				this.extraUpdates = 3;
				this.coldDamage = true;
			}
			else if (this.type == 310)
			{
				this.netImportant = true;
				this.name = "Blue Flare";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 33;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.timeLeft = 36000;
			}
			else if (this.type == 311)
			{
				this.name = "Candy Corn";
				this.width = 10;
				this.height = 12;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 3;
				this.alpha = 255;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 312)
			{
				this.name = "Jack 'O Lantern";
				this.alpha = 255;
				this.width = 32;
				this.height = 32;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
				this.timeLeft = 300;
			}
			else if (this.type == 313)
			{
				this.netImportant = true;
				this.name = "Spider";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 314)
			{
				this.netImportant = true;
				this.name = "Squashling";
				this.width = 24;
				this.height = 40;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 315)
			{
				this.netImportant = true;
				this.name = "Bat Hook";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 316)
			{
				this.alpha = 255;
				this.name = "Bat";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 36;
				this.friendly = true;
				this.penetrate = 1;
				this.timeLeft = 600;
				this.magic = true;
			}
			else if (this.type == 317)
			{
				this.netImportant = true;
				this.name = "Raven";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 54;
				this.penetrate = 1;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 1f;
			}
			else if (this.type == 318)
			{
				this.name = "Rotten Egg";
				this.width = 12;
				this.height = 14;
				this.aiStyle = 2;
				this.friendly = true;
				this.thrown = true;
			}
			else if (this.type == 319)
			{
				this.netImportant = true;
				this.name = "Black Cat";
				this.width = 36;
				this.height = 30;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 320)
			{
				this.name = "Bloody Machete";
				this.width = 34;
				this.height = 34;
				this.aiStyle = 3;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
			}
			else if (this.type == 321)
			{
				this.name = "Flaming Jack";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 55;
				this.friendly = true;
				this.melee = true;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 322)
			{
				this.netImportant = true;
				this.name = "Wood Hook";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 323)
			{
				this.penetrate = 10;
				this.name = "Stake";
				this.extraUpdates = 3;
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.alpha = 255;
				this.friendly = true;
				this.ranged = true;
				this.scale = 0.8f;
			}
			else if (this.type == 324)
			{
				this.netImportant = true;
				this.name = "Cursed Sapling";
				this.width = 26;
				this.height = 38;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 325)
			{
				this.alpha = 255;
				this.penetrate = -1;
				this.name = "Flaming Wood";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.hostile = true;
				this.tileCollide = false;
			}
			else if (this.type >= 326 && this.type <= 328)
			{
				this.name = "Greek Fire";
				if (this.type == 326)
				{
					this.width = 14;
					this.height = 16;
				}
				else if (this.type == 327)
				{
					this.width = 12;
					this.height = 14;
				}
				else
				{
					this.width = 6;
					this.height = 12;
				}
				this.aiStyle = 14;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 360;
			}
			else if (this.type == 329)
			{
				this.name = "Flaming Scythe";
				this.width = 80;
				this.height = 80;
				this.light = 0.25f;
				this.aiStyle = 56;
				this.hostile = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft = 420;
			}
			else if (this.type == 330)
			{
				this.name = "Star Anise";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 6;
				this.thrown = true;
			}
			else if (this.type == 331)
			{
				this.netImportant = true;
				this.name = "Candy Cane Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 332)
			{
				this.netImportant = true;
				this.name = "Christmas Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
				this.light = 0.5f;
			}
			else if (this.type == 333)
			{
				this.name = "Fruitcake Chakram";
				this.width = 38;
				this.height = 38;
				this.aiStyle = 3;
				this.friendly = true;
				this.scale = 0.9f;
				this.penetrate = -1;
				this.melee = true;
			}
			else if (this.type == 334)
			{
				this.netImportant = true;
				this.name = "Puppy";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 335)
			{
				this.name = "Ornament";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 1;
				this.melee = true;
			}
			else if (this.type == 336)
			{
				this.name = "Pine Needle";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.magic = true;
				this.scale = 0.8f;
				this.extraUpdates = 1;
			}
			else if (this.type == 337)
			{
				this.name = "Blizzard";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.magic = true;
				this.tileCollide = false;
				this.coldDamage = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 338 || this.type == 339 || this.type == 340 || this.type == 341)
			{
				this.name = "Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.penetrate = -1;
				this.friendly = true;
				this.ranged = true;
				this.scale = 0.9f;
			}
			else if (this.type == 342)
			{
				this.name = "North Pole";
				this.width = 22;
				this.height = 2;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.1f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.coldDamage = true;
				this.tileCollide = false;
			}
			else if (this.type == 343)
			{
				this.alpha = 255;
				this.name = "North Pole";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 57;
				this.friendly = true;
				this.melee = true;
				this.scale = 1.1f;
				this.penetrate = 3;
				this.coldDamage = true;
			}
			else if (this.type == 344)
			{
				this.name = "North Pole";
				this.width = 26;
				this.height = 26;
				this.aiStyle = 1;
				this.friendly = true;
				this.scale = 0.9f;
				this.alpha = 255;
				this.melee = true;
				this.coldDamage = true;
			}
			else if (this.type == 345)
			{
				this.name = "Pine Needle";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.scale = 0.8f;
			}
			else if (this.type == 346)
			{
				this.name = "Ornament";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 14;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 300;
			}
			else if (this.type == 347)
			{
				this.name = "Ornament";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 2;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 348)
			{
				this.name = "Frost Wave";
				this.aiStyle = 1;
				this.width = 48;
				this.height = 48;
				this.hostile = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.coldDamage = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 349)
			{
				this.name = "Frost Shard";
				this.aiStyle = 1;
				this.width = 12;
				this.height = 12;
				this.hostile = true;
				this.penetrate = -1;
				this.coldDamage = true;
			}
			else if (this.type == 350)
			{
				this.alpha = 255;
				this.penetrate = -1;
				this.name = "Missile";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.hostile = true;
				this.tileCollide = false;
				this.timeLeft /= 2;
			}
			else if (this.type == 351)
			{
				this.alpha = 255;
				this.penetrate = -1;
				this.name = "Present";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 58;
				this.hostile = true;
				this.tileCollide = false;
			}
			else if (this.type == 352)
			{
				this.name = "Spike";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 14;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft /= 3;
			}
			else if (this.type == 353)
			{
				this.netImportant = true;
				this.name = "Baby Grinch";
				this.width = 18;
				this.height = 28;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 354)
			{
				this.name = "Crimsand Ball";
				this.knockBack = 6f;
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
				this.friendly = true;
				this.penetrate = -1;
				this.extraUpdates = 1;
			}
			else if (this.type == 355)
			{
				this.name = "Venom Fang";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 1;
				this.alpha = 255;
				this.friendly = true;
				this.magic = true;
				this.penetrate = 7;
			}
			else if (this.type == 356)
			{
				this.name = "Spectre Wrath";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 59;
				this.alpha = 255;
				this.magic = true;
				this.tileCollide = false;
				this.extraUpdates = 3;
			}
			else if (this.type == 357)
			{
				this.name = "Pulse Bolt";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 6;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.2f;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 358)
			{
				this.name = "Water Gun";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 60;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.ignoreWater = true;
			}
			else if (this.type == 359)
			{
				this.name = "Frost Bolt";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 28;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 2;
				this.friendly = true;
				this.coldDamage = true;
			}
			else if ((this.type >= 360 && this.type <= 366) || this.type == 381 || this.type == 382)
			{
				this.name = "Bobber";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 61;
				this.penetrate = -1;
				this.bobber = true;
			}
			else if (this.type == 367)
			{
				this.name = "Obsidian Swordfish";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.scale = 1.1f;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 368)
			{
				this.name = "Swordfish";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 19;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 369)
			{
				this.name = "Sawtooth Shark";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 370)
			{
				this.name = "Love Potion";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 1;
			}
			else if (this.type == 371)
			{
				this.name = "Foul Potion";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 1;
			}
			else if (this.type == 372)
			{
				this.name = "Fish Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 373)
			{
				this.netImportant = true;
				this.name = "Hornet";
				this.width = 24;
				this.height = 26;
				this.aiStyle = 62;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 1f;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 374)
			{
				this.name = "Hornet Stinger";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 0;
				this.friendly = true;
				this.penetrate = 1;
				this.aiStyle = 1;
				this.tileCollide = true;
				this.scale *= 0.9f;
			}
			else if (this.type == 375)
			{
				this.netImportant = true;
				this.name = "Flying Imp";
				this.width = 34;
				this.height = 26;
				this.aiStyle = 62;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 1f;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 376)
			{
				this.name = "Imp Fireball";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 0;
				this.friendly = true;
				this.penetrate = -1;
				this.aiStyle = 1;
				this.tileCollide = true;
				this.timeLeft = 100;
				this.alpha = 255;
				this.extraUpdates = 1;
			}
			else if (this.type == 377)
			{
				this.name = "Spider Turret";
				this.width = 66;
				this.height = 50;
				this.aiStyle = 53;
				this.timeLeft = 7200;
				this.ignoreWater = true;
			}
			else if (this.type == 378)
			{
				this.name = "Spider Egg";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 14;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft = 60;
				this.scale = 0.9f;
			}
			else if (this.type == 379)
			{
				this.name = "Baby Spider";
				this.width = 14;
				this.height = 10;
				this.aiStyle = 63;
				this.friendly = true;
				this.timeLeft = 300;
				this.penetrate = 1;
			}
			else if (this.type == 380)
			{
				this.netImportant = true;
				this.name = "Zephyr Fish";
				this.width = 26;
				this.height = 26;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 383)
			{
				this.name = "Anchor";
				this.width = 34;
				this.height = 34;
				this.aiStyle = 3;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
			}
			else if (this.type == 384)
			{
				this.name = "Sharknado";
				this.width = 150;
				this.height = 42;
				this.hostile = true;
				this.penetrate = -1;
				this.aiStyle = 64;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.alpha = 255;
				this.timeLeft = 540;
			}
			else if (this.type == 385)
			{
				this.name = "Sharknado Bolt";
				this.width = 30;
				this.height = 30;
				this.hostile = true;
				this.penetrate = -1;
				this.aiStyle = 65;
				this.alpha = 255;
				this.timeLeft = 300;
			}
			else if (this.type == 386)
			{
				this.name = "Cthulunado";
				this.width = 150;
				this.height = 42;
				this.hostile = true;
				this.penetrate = -1;
				this.aiStyle = 64;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.alpha = 255;
				this.timeLeft = 840;
			}
			else if (this.type == 387)
			{
				this.netImportant = true;
				this.name = "Retanimini";
				this.width = 40;
				this.height = 20;
				this.aiStyle = 66;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 0.5f;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.friendly = true;
			}
			else if (this.type == 388)
			{
				this.netImportant = true;
				this.name = "Spazmamini";
				this.width = 40;
				this.height = 20;
				this.aiStyle = 66;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 0.5f;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.friendly = true;
			}
			else if (this.type == 389)
			{
				this.name = "Mini Retina Laser";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 3;
				this.light = 0.75f;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.2f;
				this.timeLeft = 600;
			}
			else if (this.type == 390 || this.type == 391 || this.type == 392)
			{
				this.name = "Venom Spider";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 26;
				this.penetrate = -1;
				this.netImportant = true;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 0.75f;
				if (this.type == 391)
				{
					this.name = "Jumper Spider";
				}
				if (this.type == 392)
				{
					this.name = "Dangerous Spider";
				}
			}
			else if (this.type == 393 || this.type == 394 || this.type == 395)
			{
				this.name = "One Eyed Pirate";
				this.width = 20;
				this.height = 30;
				this.aiStyle = 67;
				this.penetrate = -1;
				this.netImportant = true;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 1f;
				if (this.type == 394)
				{
					this.name = "Soulscourge Pirate";
				}
				if (this.type == 395)
				{
					this.name = "Pirate Captain";
				}
			}
			else if (this.type == 396)
			{
				this.name = "Slime Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
				this.alpha = 100;
			}
			else if (this.type == 397)
			{
				this.name = "Sticky Grenade";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.thrown = true;
				this.tileCollide = false;
			}
			else if (this.type == 398)
			{
				this.netImportant = true;
				this.name = "Mini Minotaur";
				this.width = 18;
				this.height = 38;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 399)
			{
				this.name = "Molotov Cocktail";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 68;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.thrown = true;
				this.noEnchantments = true;
			}
			else if (this.type >= 400 && this.type <= 402)
			{
				this.name = "Molotov Fire";
				if (this.type == 400)
				{
					this.width = 14;
					this.height = 16;
				}
				else if (this.type == 401)
				{
					this.width = 12;
					this.height = 14;
				}
				else
				{
					this.width = 6;
					this.height = 12;
				}
				this.penetrate = 3;
				this.aiStyle = 14;
				this.friendly = true;
				this.timeLeft = 360;
				this.ranged = true;
				this.noEnchantments = true;
			}
			else if (this.type == 403)
			{
				this.netImportant = true;
				this.name = "Track Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 404)
			{
				this.name = "Flairon";
				this.width = 26;
				this.height = 26;
				this.aiStyle = 69;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.melee = true;
			}
			else if (this.type == 405)
			{
				this.name = "Flairon Bubble";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 70;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 90;
				this.melee = true;
				this.noEnchantments = true;
			}
			else if (this.type == 406)
			{
				this.name = "Slime Gun";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 60;
				this.alpha = 255;
				this.penetrate = -1;
				this.extraUpdates = 2;
				this.ignoreWater = true;
			}
			else if (this.type == 407)
			{
				this.netImportant = true;
				this.name = "Tempest";
				this.width = 28;
				this.height = 40;
				this.aiStyle = 62;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.friendly = true;
				this.minionSlots = 1f;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 408)
			{
				this.name = "Mini Sharkron";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.alpha = 255;
				this.ignoreWater = true;
			}
			else if (this.type == 409)
			{
				this.name = "Typhoon";
				this.width = 30;
				this.height = 30;
				this.penetrate = -1;
				this.aiStyle = 71;
				this.alpha = 255;
				this.timeLeft = 360;
				this.friendly = true;
				this.tileCollide = true;
				this.extraUpdates = 2;
				this.magic = true;
				this.ignoreWater = true;
			}
			else if (this.type == 410)
			{
				this.name = "Bubble";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 72;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 50;
				this.magic = true;
				this.ignoreWater = true;
			}
			else if (this.type >= 411 && this.type <= 414)
			{
				switch (this.type)
				{
				case 411:
					this.name = "Copper Coins";
					break;
				case 412:
					this.name = "Silver Coins";
					break;
				case 413:
					this.name = "Gold Coins";
					break;
				case 414:
					this.name = "Platinum Coins";
					break;
				}
				this.width = 10;
				this.height = 10;
				this.aiStyle = 10;
			}
			else if (this.type == 415 || this.type == 416 || this.type == 417 || this.type == 418)
			{
				this.name = "Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 34;
				this.friendly = true;
				this.ranged = true;
				this.timeLeft = 45;
			}
			else if (this.type >= 419 && this.type <= 422)
			{
				this.name = "Firework Fountain";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 73;
				this.friendly = true;
			}
			else if (this.type == 423)
			{
				this.netImportant = true;
				this.name = "UFO";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 62;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.friendly = true;
				this.minionSlots = 1f;
				this.ignoreWater = true;
			}
			else if (this.type >= 424 && this.type <= 426)
			{
				this.name = "Meteor";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 1;
				this.friendly = true;
				this.magic = true;
				this.tileCollide = false;
				this.extraUpdates = 2;
			}
			else if (this.type == 427)
			{
				this.name = "Vortex Chainsaw";
				this.width = 22;
				this.height = 56;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.glowMask = 2;
			}
			else if (this.type == 428)
			{
				this.name = "Vortex Drill";
				this.width = 26;
				this.height = 54;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.glowMask = 3;
			}
			else if (this.type == 429)
			{
				this.name = "Nebula Chainsaw";
				this.width = 18;
				this.height = 56;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.glowMask = 7;
			}
			else if (this.type == 430)
			{
				this.name = "Nebula Drill";
				this.width = 30;
				this.height = 54;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.glowMask = 8;
			}
			else if (this.type == 431)
			{
				this.name = "Solar Flare Chainsaw";
				this.width = 28;
				this.height = 64;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 432)
			{
				this.name = "Solar Flare Drill";
				this.width = 30;
				this.height = 54;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
			}
			else if (this.type == 610)
			{
				this.name = "Stardust Chainsaw";
				this.width = 28;
				this.height = 64;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.glowMask = 179;
			}
			else if (this.type == 609)
			{
				this.name = "Stardust Drill";
				this.width = 30;
				this.height = 54;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.glowMask = 180;
			}
			else if (this.type == 433)
			{
				this.name = "UFO Ray";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 48;
				this.friendly = true;
				this.extraUpdates = 100;
				this.timeLeft = 100;
				this.ignoreWater = true;
			}
			else if (this.type == 434)
			{
				this.name = "Scutlix Laser";
				this.width = 1;
				this.height = 1;
				this.aiStyle = 74;
				this.friendly = true;
				this.extraUpdates = 100;
				this.penetrate = -1;
			}
			else if (this.type == 435)
			{
				this.name = "Electric Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.hostile = true;
				this.ignoreWater = true;
			}
			else if (this.type == 436)
			{
				this.name = "Brain Scrambling Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.hostile = true;
				this.ignoreWater = true;
			}
			else if (this.type == 437)
			{
				this.name = "Gigazapper Spearhead";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.hostile = true;
				this.extraUpdates = 2;
				this.ignoreWater = true;
			}
			else if (this.type == 438)
			{
				this.name = "Laser Ray";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 1;
				this.hostile = true;
				this.alpha = 255;
				this.extraUpdates = 3;
				this.ignoreWater = true;
			}
			else if (this.type == 439)
			{
				this.name = "Laser Machinegun";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 75;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.magic = true;
				this.ignoreWater = true;
			}
			else if (this.type == 440)
			{
				this.name = "Laser";
				this.width = 5;
				this.height = 5;
				this.aiStyle = 1;
				this.friendly = true;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1f;
				this.timeLeft = 600;
				this.magic = true;
				this.ignoreWater = true;
			}
			else if (this.type == 441)
			{
				this.name = "Scutlix Crosshair";
				this.width = 1;
				this.height = 1;
				this.aiStyle = 76;
				this.ignoreWater = true;
				this.tileCollide = false;
			}
			else if (this.type == 442)
			{
				this.name = "Electrosphere Missile";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.friendly = true;
				this.alpha = 255;
				this.scale = 1f;
				this.timeLeft = 600;
				this.ranged = true;
			}
			else if (this.type == 443)
			{
				this.name = "Electrosphere";
				this.width = 80;
				this.height = 80;
				this.aiStyle = 77;
				this.friendly = true;
				this.alpha = 255;
				this.scale = 1f;
				this.ranged = true;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.penetrate = -1;
			}
			else if (this.type == 444)
			{
				this.name = "Xenopopper";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 78;
				this.friendly = true;
				this.alpha = 255;
				this.scale = 1f;
				this.ranged = true;
				this.ignoreWater = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 445)
			{
				this.name = "Laser Drill";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 75;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.melee = true;
				this.ignoreWater = true;
				this.ownerHitCheck = true;
			}
			else if (this.type == 446)
			{
				this.netImportant = true;
				this.name = "Anti-Gravity Hook";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
				this.light = 0.7f;
			}
			else if (this.type == 447)
			{
				this.name = "Martian Deathray";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 79;
				this.hostile = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.timeLeft = 240;
			}
			else if (this.type == 448)
			{
				this.name = "Martian Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 80;
				this.hostile = true;
				this.penetrate = -1;
				this.tileCollide = false;
			}
			else if (this.type == 449)
			{
				this.name = "Saucer Laser";
				this.width = 5;
				this.height = 5;
				this.aiStyle = 1;
				this.hostile = true;
				this.alpha = 255;
				this.extraUpdates = 1;
				this.scale = 1f;
				this.timeLeft = 600;
				this.ignoreWater = true;
			}
			else if (this.type == 450)
			{
				this.name = "Saucer Scrap";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 14;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 360;
			}
			else if (this.type == 451)
			{
				this.name = "Influx Waver";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 81;
				this.melee = true;
				this.penetrate = 3;
				this.light = 0.2f;
				this.alpha = 255;
				this.friendly = true;
			}
			else if (this.type == 452)
			{
				this.name = "Phantasmal Eye";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 82;
				this.hostile = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.timeLeft = 600;
			}
			else if (this.type == 453)
			{
				this.name = "Drill Crosshair";
				this.width = 1;
				this.height = 1;
				this.aiStyle = 76;
				this.ignoreWater = true;
				this.tileCollide = false;
			}
			else if (this.type == 454)
			{
				this.name = "Phantasmal Sphere";
				this.width = 46;
				this.height = 46;
				this.aiStyle = 83;
				this.hostile = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.timeLeft = 600;
				this.tileCollide = false;
			}
			else if (this.type == 455)
			{
				this.name = "Phantasmal Deathray";
				this.width = 36;
				this.height = 36;
				this.aiStyle = 84;
				this.hostile = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.timeLeft = 600;
				this.tileCollide = false;
			}
			else if (this.type == 456)
			{
				this.name = "Moon Leech";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 85;
				this.hostile = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.timeLeft = 600;
				this.tileCollide = false;
			}
			else if (this.type == 459)
			{
				this.name = "Charged Blaster Orb";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 1;
				this.friendly = true;
				this.magic = true;
				this.alpha = 255;
				this.scale = 1f;
				this.ignoreWater = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 460)
			{
				this.name = "Charged Blaster Cannon";
				this.width = 14;
				this.height = 18;
				this.aiStyle = 75;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.magic = true;
				this.ignoreWater = true;
			}
			else if (this.type == 461)
			{
				this.name = "Charged Blaster Laser";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 84;
				this.friendly = true;
				this.magic = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.tileCollide = false;
				this.hide = true;
			}
			else if (this.type == 462)
			{
				this.name = "Phantasmal Bolt";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 1;
				this.hostile = true;
				this.alpha = 255;
				this.extraUpdates = 3;
				this.ignoreWater = true;
				this.tileCollide = false;
			}
			else if (this.type == 463)
			{
				this.name = "Vicious Powder";
				this.width = 48;
				this.height = 48;
				this.aiStyle = 6;
				this.friendly = true;
				this.tileCollide = false;
				this.penetrate = -1;
				this.alpha = 255;
				this.ignoreWater = true;
			}
			else if (this.type == 464)
			{
				this.name = "Ice Mist";
				this.width = 60;
				this.height = 60;
				this.aiStyle = 86;
				this.hostile = true;
				this.tileCollide = false;
				this.penetrate = -1;
				this.alpha = 255;
				this.ignoreWater = true;
			}
			else if (this.type == 467)
			{
				this.name = "Fireball";
				this.width = 40;
				this.height = 40;
				this.aiStyle = 1;
				this.hostile = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 468)
			{
				this.name = "Shadow Fireball";
				this.width = 40;
				this.height = 40;
				this.aiStyle = 1;
				this.hostile = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 465)
			{
				this.name = "Lightning Orb";
				this.width = 80;
				this.height = 80;
				this.aiStyle = 88;
				this.hostile = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.tileCollide = false;
			}
			else if (this.type == 466)
			{
				this.name = "Lightning Orb Arc";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 88;
				this.hostile = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.tileCollide = true;
				this.extraUpdates = 4;
				this.timeLeft = 120 * (this.extraUpdates + 1);
			}
			else if (this.type == 491)
			{
				this.name = "Flying Knife";
				this.width = 26;
				this.height = 26;
				this.aiStyle = 9;
				this.friendly = true;
				this.magic = true;
				this.penetrate = -1;
			}
			else if (this.type == 500)
			{
				this.name = "Crimson Heart";
				this.width = 20;
				this.height = 20;
				this.aiStyle = 67;
				this.penetrate = -1;
				this.netImportant = true;
				this.timeLeft *= 5;
				this.friendly = true;
				this.ignoreWater = true;
				this.scale = 0.8f;
			}
			else if (this.type == 499)
			{
				this.netImportant = true;
				this.name = "Baby Face Monster";
				this.width = 34;
				this.height = 34;
				this.aiStyle = 26;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 469)
			{
				this.alpha = 255;
				this.arrow = true;
				this.name = "Bee Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 470)
			{
				this.name = "Sticky Dynamite";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
			}
			else if (this.type == 471)
			{
				this.name = "Bone";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 2;
				this.scale = 1.2f;
				this.hostile = true;
				this.ranged = true;
			}
			else if (this.type == 472)
			{
				this.name = "Web spit";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 0;
				this.hostile = true;
				this.penetrate = -1;
				this.aiStyle = 1;
				this.tileCollide = true;
				this.timeLeft = 50;
			}
			else if (this.type == 474)
			{
				this.arrow = true;
				this.name = "Bone Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 473)
			{
				this.netImportant = true;
				this.name = "Spelunker Glowstick";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 14;
				this.penetrate = -1;
				this.alpha = 75;
				this.light = 1f;
				this.timeLeft *= 2;
			}
			else if (this.type == 475)
			{
				this.name = "Vine Rope Coil";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 35;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft = 400;
			}
			else if (this.type == 476)
			{
				this.name = "Soul Drain";
				this.width = 200;
				this.height = 200;
				this.aiStyle = -1;
				this.friendly = true;
				this.tileCollide = false;
				this.penetrate = -1;
				this.alpha = 255;
				this.ignoreWater = true;
				this.timeLeft = 3;
			}
			else if (this.type == 477)
			{
				this.alpha = 255;
				this.name = "Crystal Dart";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 7;
				this.extraUpdates = 1;
				this.ranged = true;
			}
			else if (this.type == 478)
			{
				this.alpha = 255;
				this.name = "Cursed Dart";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.friendly = true;
				this.timeLeft = 300;
				this.ranged = true;
			}
			else if (this.type == 479)
			{
				this.alpha = 255;
				this.name = "Ichor Dart";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 480)
			{
				this.alpha = 255;
				this.name = "Cursed Flame";
				this.width = 12;
				this.height = 12;
				this.penetrate = 3;
				this.aiStyle = 14;
				this.friendly = true;
				this.timeLeft = 120;
				this.ranged = true;
				this.noEnchantments = true;
			}
			else if (this.type == 481)
			{
				this.name = "Chain Guillotine";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 13;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.melee = true;
				this.extraUpdates = 0;
			}
			else if (this.type == 482)
			{
				this.name = "Cursed Flames";
				this.width = 16;
				this.height = 200;
				this.aiStyle = 87;
				this.friendly = true;
				this.tileCollide = false;
				this.penetrate = 20;
				this.alpha = 255;
				this.ignoreWater = true;
				this.timeLeft = 2700;
			}
			else if (this.type == 483)
			{
				this.arrow = true;
				this.name = "Seedler";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 14;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 484)
			{
				this.arrow = true;
				this.name = "Seedler";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 485)
			{
				this.arrow = true;
				this.name = "Hellwing";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
				this.penetrate = -1;
			}
			else if (this.type >= 486 && this.type <= 489)
			{
				this.name = "Hook";
				if (this.type == 486)
				{
					this.width = 12;
					this.height = 12;
				}
				else if (this.type == 487)
				{
					this.width = 22;
					this.height = 22;
				}
				else if (this.type == 488)
				{
					this.width = 12;
					this.height = 12;
					this.light = 0.3f;
				}
				else if (this.type == 489)
				{
					this.width = 20;
					this.height = 16;
				}
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 492)
			{
				this.netImportant = true;
				this.name = "Magic Lantern";
				this.width = 18;
				this.height = 32;
				this.aiStyle = 90;
				this.friendly = true;
				this.penetrate = -1;
				this.timeLeft *= 5;
			}
			else if (this.type == 490)
			{
				this.name = "Lightning Ritual";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 89;
				this.hostile = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.timeLeft = 600;
				this.netImportant = true;
			}
			else if (this.type == 493 || this.type == 494)
			{
				this.name = "Crystal Vile Shard";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 4;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.alpha = 255;
				this.ignoreWater = true;
				this.magic = true;
				this.light = 0.2f;
			}
			else if (this.type == 495)
			{
				this.arrow = true;
				this.name = "Shadowflame Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
				this.penetrate = 3;
			}
			else if (this.type == 496)
			{
				this.alpha = 255;
				this.name = "Shadowflame";
				this.width = 40;
				this.height = 40;
				this.aiStyle = 91;
				this.friendly = true;
				this.magic = true;
				this.MaxUpdates = 3;
				this.penetrate = 3;
			}
			else if (this.type == 497)
			{
				this.name = "Shadowflame Knife";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 3;
				this.melee = true;
			}
			else if (this.type == 498)
			{
				this.name = "Nail";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 180;
			}
			else if (this.type == 501)
			{
				this.name = "Flask";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 2;
				this.scale = 1.1f;
				this.hostile = true;
				this.ranged = true;
			}
			else if (this.type == 502)
			{
				this.name = "Meowmere";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 8;
				this.friendly = true;
				this.melee = true;
				this.penetrate = 5;
			}
			else if (this.type == 503)
			{
				this.name = "Star Wrath";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 5;
				this.friendly = true;
				this.penetrate = 2;
				this.alpha = 255;
				this.tileCollide = false;
				this.melee = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 504)
			{
				this.name = "Spark";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 2;
				this.friendly = true;
				this.magic = true;
				this.alpha = 255;
				this.penetrate = 2;
			}
			else if (this.type == 507)
			{
				this.name = "Javelin";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 1;
				this.friendly = true;
				this.melee = true;
				this.penetrate = 3;
			}
			else if (this.type == 508)
			{
				this.name = "Javelin";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 1;
				this.hostile = true;
				this.melee = true;
				this.penetrate = -1;
			}
			else if (this.type == 509)
			{
				this.name = "Butcher's Chainsaw";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 20;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ownerHitCheck = true;
				this.melee = true;
				this.scale = 1.2f;
			}
			else if (this.type == 510)
			{
				this.name = "Toxic Flask";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 1;
				this.magic = true;
			}
			else if (this.type == 511)
			{
				this.name = "Toxic Cloud";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 92;
				this.friendly = true;
				this.penetrate = -1;
				this.scale = 1.1f;
				this.magic = true;
			}
			else if (this.type == 512)
			{
				this.name = "Toxic Cloud";
				this.width = 40;
				this.height = 38;
				this.aiStyle = 92;
				this.friendly = true;
				this.penetrate = -1;
				this.scale = 1.1f;
				this.magic = true;
			}
			else if (this.type == 513)
			{
				this.name = "Toxic Cloud";
				this.width = 30;
				this.height = 28;
				this.aiStyle = 92;
				this.friendly = true;
				this.penetrate = -1;
				this.scale = 1.1f;
				this.magic = true;
			}
			else if (this.type == 514)
			{
				this.name = "Nail";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 93;
				this.friendly = true;
				this.penetrate = 3;
				this.alpha = 255;
				this.ranged = true;
			}
			else if (this.type == 515)
			{
				this.netImportant = true;
				this.name = "Bouncy Glowstick";
				this.width = 6;
				this.height = 6;
				this.aiStyle = 14;
				this.penetrate = -1;
				this.alpha = 75;
				this.light = 1f;
				this.timeLeft *= 5;
			}
			else if (this.type == 516)
			{
				this.name = "Bouncy Bomb";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
			}
			else if (this.type == 517)
			{
				this.name = "Bouncy Grenade";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.thrown = true;
			}
			else if (this.type == 518)
			{
				this.name = "Coin Portal";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 94;
				this.friendly = true;
				this.alpha = 255;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 519)
			{
				this.name = "Bomb Fish";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
			}
			else if (this.type == 520)
			{
				this.name = "Frost Daggerfish";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 3;
				this.thrown = true;
			}
			else if (this.type == 521)
			{
				this.name = "Crystal Charge";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 1;
				this.friendly = true;
			}
			else if (this.type == 522)
			{
				this.name = "Crystal Charge";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 1;
				this.friendly = true;
			}
			else if (this.type == 523)
			{
				this.name = "Toxic Bubble";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 95;
				this.alpha = 255;
				this.ranged = true;
				this.penetrate = 1;
				this.friendly = true;
			}
			else if (this.type == 524)
			{
				this.name = "Ichor Splash";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 96;
				this.friendly = true;
				this.alpha = 255;
				this.penetrate = -1;
				this.ignoreWater = true;
				this.melee = true;
				this.extraUpdates = 5;
			}
			else if (this.type == 525)
			{
				this.name = "Flying Piggy Bank";
				this.width = 30;
				this.height = 24;
				this.aiStyle = 97;
				this.tileCollide = false;
				this.timeLeft = 10800;
			}
			else if (this.type == 526)
			{
				this.name = "Energy";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 98;
				this.tileCollide = false;
				this.timeLeft = 120;
				this.alpha = 255;
			}
			else if (this.type >= 527 && this.type <= 531)
			{
				this.name = "Tombstone";
				this.knockBack = 12f;
				this.width = 24;
				this.height = 24;
				this.aiStyle = 17;
				this.penetrate = -1;
			}
			else if (this.type == 532)
			{
				this.name = "XBone";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 1;
				this.scale = 1f;
				this.friendly = true;
				this.thrown = true;
				this.penetrate = 3;
				this.extraUpdates = 1;
			}
			else if (this.type == 533)
			{
				this.netImportant = true;
				this.name = "Deadly Sphere";
				this.width = 20;
				this.height = 20;
				this.aiStyle = 66;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.minionSlots = 1f;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.friendly = true;
			}
			else if (this.type == 534)
			{
				this.extraUpdates = 0;
				this.name = "Yoyo";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 99;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 1f;
			}
			else if (this.type >= 541 && this.type <= 555)
			{
				this.extraUpdates = 0;
				this.name = "Yoyo";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 99;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 1f;
				if (this.type == 547)
				{
					this.scale = 1.1f;
				}
				if (this.type == 554)
				{
					this.scale = 1.2f;
				}
				if (this.type == 555)
				{
					this.scale = 1.15f;
				}
				if (this.type == 551 || this.type == 550)
				{
					this.scale = 1.1f;
				}
			}
			else if (this.type >= 562 && this.type <= 564)
			{
				this.extraUpdates = 0;
				this.name = "Yoyo";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 99;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 1f;
				if (this.type == 563)
				{
					this.scale = 1.05f;
				}
				if (this.type == 564)
				{
					this.scale = 1.075f;
				}
			}
			else if (this.type == 603)
			{
				this.extraUpdates = 0;
				this.name = "Terrarian";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 99;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 1.15f;
			}
			else if (this.type == 604)
			{
				this.extraUpdates = 0;
				this.name = "Terrarian";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 115;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 1.2f;
			}
			else if (this.type >= 556 && this.type <= 561)
			{
				this.extraUpdates = 0;
				this.name = "Counterweight";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 99;
				this.friendly = true;
				this.penetrate = -1;
				this.melee = true;
				this.scale = 1f;
				this.counterweight = true;
			}
			else if (this.type == 535)
			{
				this.name = "Medusa Ray";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 100;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.magic = true;
				this.ignoreWater = true;
			}
			else if (this.type == 536)
			{
				this.name = "Medusa Ray";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 101;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.magic = true;
				this.ignoreWater = true;
			}
			else if (this.type == 537)
			{
				this.name = "Stardust Laser";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 84;
				this.hostile = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.timeLeft = 240;
				this.tileCollide = false;
			}
			else if (this.type == 538)
			{
				this.name = "Twinkle";
				this.width = 12;
				this.height = 12;
				this.aiStyle = 14;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 120;
				this.extraUpdates = 1;
				this.alpha = 255;
			}
			else if (this.type == 539)
			{
				this.name = "Flow Invader";
				this.width = 18;
				this.height = 30;
				this.aiStyle = 102;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 600;
			}
			else if (this.type == 540)
			{
				this.name = "Starmark";
				this.width = 20;
				this.height = 20;
				this.aiStyle = 103;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 300;
				this.alpha = 255;
			}
			else if (this.type == 565)
			{
				this.name = "Brain of Confusion";
				this.width = 28;
				this.height = 28;
				this.aiStyle = 104;
				this.penetrate = -1;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.alpha = 255;
				this.scale = 0.8f;
			}
			else if (this.type == 566)
			{
				this.name = "Bee";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 36;
				this.friendly = true;
				this.penetrate = 4;
				this.alpha = 255;
				this.timeLeft = 660;
				this.extraUpdates = 3;
			}
			else if (this.type == 567 || this.type == 568)
			{
				this.name = "Spore";
				if (this.type == 567)
				{
					this.width = 14;
					this.height = 14;
				}
				else
				{
					this.width = 16;
					this.height = 16;
				}
				this.aiStyle = 105;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 3600;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type >= 569 && this.type <= 571)
			{
				this.name = "Spore";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 106;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.timeLeft = 3600;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 575)
			{
				this.name = "Nebula Sphere";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 107;
				this.hostile = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft = 420;
				this.alpha = 255;
			}
			else if (this.type == 573)
			{
				this.name = "Nebula Piercer";
				this.width = 18;
				this.height = 30;
				this.aiStyle = 102;
				this.hostile = true;
				this.penetrate = -1;
				this.timeLeft = 600;
			}
			else if (this.type == 574)
			{
				this.name = "Nebula Eye";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 102;
				this.hostile = true;
				this.timeLeft = 600;
				this.tileCollide = false;
			}
			else if (this.type == 572)
			{
				this.name = "Poison Spit";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.alpha = 255;
				this.penetrate = -1;
				this.friendly = false;
				this.hostile = true;
			}
			else if (this.type == 576)
			{
				this.name = "Nebula Laser";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.2f;
				this.timeLeft = 600;
			}
			else if (this.type == 577)
			{
				this.name = "Vortex Laser";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1.2f;
				this.timeLeft = 600;
			}
			else if (this.type == 578 || this.type == 579)
			{
				this.name = "Vortex";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 108;
				this.friendly = true;
				this.alpha = 255;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.hostile = true;
				this.hide = true;
			}
			else if (this.type == 580)
			{
				this.name = "Vortex Lightning";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 88;
				this.hostile = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.tileCollide = true;
				this.extraUpdates = 4;
				this.timeLeft = 600;
			}
			else if (this.type == 581)
			{
				this.name = "Alien Goop";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.alpha = 255;
				this.penetrate = -1;
				this.friendly = false;
				this.hostile = true;
			}
			else if (this.type == 582)
			{
				this.name = "Mechanic's Wrench";
				this.width = 20;
				this.height = 20;
				this.aiStyle = 109;
				this.friendly = true;
				this.penetrate = -1;
				this.MaxUpdates = 2;
			}
			else if (this.type == 583)
			{
				this.name = "Syringe";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 2;
				this.friendly = true;
				this.scale = 0.8f;
			}
			else if (this.type == 589)
			{
				this.name = "Christmas Ornament";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 2;
				this.friendly = true;
			}
			else if (this.type == 584)
			{
				this.name = "Syringe";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 110;
				this.friendly = true;
				this.scale = 0.8f;
				this.penetrate = 3;
			}
			else if (this.type == 585)
			{
				this.name = "Skull";
				this.width = 26;
				this.height = 26;
				this.aiStyle = 1;
				this.alpha = 255;
				this.friendly = true;
				this.penetrate = 3;
			}
			else if (this.type == 586)
			{
				this.name = "Dryad's ward";
				this.width = 26;
				this.height = 26;
				this.aiStyle = 111;
				this.alpha = 255;
				this.friendly = true;
				this.penetrate = -1;
			}
			else if (this.type == 587)
			{
				this.name = "Paintball";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.alpha = 255;
				this.friendly = true;
				this.ranged = true;
			}
			else if (this.type == 588)
			{
				this.name = "Confetti Grenade";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
				this.thrown = true;
			}
			else if (this.type == 590)
			{
				this.name = "Truffle Spore";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 112;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.timeLeft = 900;
				this.tileCollide = false;
				this.ignoreWater = true;
			}
			else if (this.type == 591)
			{
				this.name = "Minecart Laser";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 101;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ignoreWater = true;
			}
			else if (this.type == 592)
			{
				this.name = "Laser Ray";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 1;
				this.hostile = true;
				this.alpha = 255;
				this.extraUpdates = 3;
				this.ignoreWater = true;
			}
			else if (this.type == 593)
			{
				this.name = "Prophecy's End";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 1;
				this.hostile = true;
				this.alpha = 255;
				this.extraUpdates = 1;
				this.ignoreWater = true;
			}
			else if (this.type == 594)
			{
				this.name = "Blowup Smoke";
				this.width = 40;
				this.height = 40;
				this.aiStyle = 1;
				this.alpha = 255;
				this.extraUpdates = 2;
			}
			else if (this.type == 595)
			{
				this.name = "Arkhalis";
				this.width = 68;
				this.height = 64;
				this.aiStyle = 75;
				this.friendly = true;
				this.tileCollide = false;
				this.melee = true;
				this.penetrate = -1;
				this.ownerHitCheck = true;
			}
			else if (this.type == 596)
			{
				this.name = "Desert Spirit's Curse";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 107;
				this.hostile = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.timeLeft = 180;
				this.tileCollide = false;
			}
			else if (this.type == 597)
			{
				this.name = "Amber Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 2;
				this.friendly = true;
			}
			else if (this.type == 598)
			{
				this.name = "Bone Javelin";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 113;
				this.friendly = true;
				this.melee = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.hide = true;
			}
			else if (this.type == 599)
			{
				this.name = "Bone Dagger";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 2;
				this.friendly = true;
				this.penetrate = 6;
				this.thrown = true;
			}
			else if (this.type == 600)
			{
				this.name = "Portal Gun";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 75;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ignoreWater = true;
			}
			else if (this.type == 601)
			{
				this.name = "Portal Bolt";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.alpha = 255;
				this.friendly = true;
				this.extraUpdates = 30;
			}
			else if (this.type == 602)
			{
				this.name = "Portal Gate";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 114;
				this.alpha = 255;
				this.friendly = true;
				this.tileCollide = false;
				this.netImportant = true;
			}
			else if (this.type == 605)
			{
				this.name = "Slime Spike";
				this.alpha = 255;
				this.width = 6;
				this.height = 6;
				this.aiStyle = 1;
				this.hostile = true;
				this.penetrate = -1;
			}
			else if (this.type == 606)
			{
				this.name = "Laser";
				this.width = 5;
				this.height = 5;
				this.aiStyle = 1;
				this.friendly = true;
				this.alpha = 255;
				this.extraUpdates = 2;
				this.scale = 1f;
				this.timeLeft = 600;
				this.ignoreWater = true;
			}
			else if (this.type == 607)
			{
				this.name = "Solar Flare";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 116;
				this.friendly = true;
				this.alpha = 255;
				this.timeLeft = 600;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.penetrate = -1;
			}
			else if (this.type == 608)
			{
				this.name = "Solar Radiance";
				this.width = 160;
				this.height = 160;
				this.aiStyle = 117;
				this.friendly = true;
				this.alpha = 255;
				this.timeLeft = 3;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.penetrate = -1;
				this.hide = true;
			}
			else if (this.type == 611)
			{
				this.name = "Solar Eruption";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 75;
				this.friendly = true;
				this.melee = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.hide = true;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.updatedNPCImmunity = true;
			}
			else if (this.type == 612)
			{
				this.name = "Solar Eruption";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 117;
				this.friendly = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.timeLeft = 60;
				this.tileCollide = false;
				this.penetrate = -1;
				this.updatedNPCImmunity = true;
			}
			else if (this.type == 613)
			{
				this.netImportant = true;
				this.name = "Stardust Cell";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 62;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.friendly = true;
				this.minionSlots = 1f;
				this.ignoreWater = true;
			}
			else if (this.type == 614)
			{
				this.name = "Stardust Cell";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 113;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
			}
			else if (this.type == 615)
			{
				this.name = "Vortex Beater";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 75;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ranged = true;
				this.ignoreWater = true;
			}
			else if (this.type == 616)
			{
				this.name = "Vortex Rocket";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 1;
				this.friendly = true;
				this.penetrate = 1;
				this.alpha = 255;
				this.ranged = true;
				this.extraUpdates = 2;
				this.timeLeft = 90 * this.MaxUpdates;
			}
			else if (this.type == 617)
			{
				this.name = "Nebula Arcanum";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 118;
				this.friendly = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.hide = true;
				this.magic = true;
				this.penetrate = 3;
				this.updatedNPCImmunity = true;
			}
			else if (this.type == 618)
			{
				this.name = "Nebula Arcanum";
				this.tileCollide = false;
				this.width = 18;
				this.height = 30;
				this.aiStyle = 119;
				this.penetrate = -1;
				this.timeLeft = 420;
				this.magic = true;
				this.friendly = true;
				this.updatedNPCImmunity = true;
			}
			else if (this.type == 619)
			{
				this.name = "Nebula Arcanum";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 1;
				this.friendly = true;
			}
			else if (this.type == 620)
			{
				this.name = "Nebula Arcanum";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 29;
				this.alpha = 255;
				this.magic = true;
				this.penetrate = 1;
				this.friendly = true;
			}
			else if (this.type == 622)
			{
				this.name = "Blowup Smoke";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.alpha = 255;
				this.extraUpdates = 2;
			}
			else if (this.type == 623)
			{
				this.netImportant = true;
				this.name = "Stardust Guardian";
				this.width = 50;
				this.height = 80;
				this.aiStyle = 120;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.friendly = true;
				this.minionSlots = 0f;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.netImportant = true;
			}
			else if (this.type == 624)
			{
				this.name = "Starburst";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 117;
				this.friendly = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.timeLeft = 60;
				this.tileCollide = false;
				this.penetrate = -1;
			}
			else if (this.type >= 625 && this.type <= 628)
			{
				if (this.type == 625 || this.type == 628)
				{
					this.netImportant = true;
				}
				if (this.type == 626 || this.type == 627)
				{
					this.minionSlots = 0.5f;
				}
				this.name = "Stardust Dragon";
				this.width = 24;
				this.height = 24;
				this.aiStyle = 121;
				this.penetrate = -1;
				this.timeLeft *= 5;
				this.minion = true;
				this.friendly = true;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.alpha = 255;
				this.hide = true;
			}
			else if (this.type == 629)
			{
				this.name = "Released Energy";
				this.width = 8;
				this.height = 8;
				this.aiStyle = 122;
				this.hostile = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.timeLeft = 3600;
				this.tileCollide = false;
				this.penetrate = -1;
				this.extraUpdates = 2;
			}
			else if (this.type == 630)
			{
				this.name = "Phantasm";
				this.width = 22;
				this.height = 22;
				this.aiStyle = 75;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.hide = true;
				this.ranged = true;
				this.ignoreWater = true;
			}
			else if (this.type == 631)
			{
				this.arrow = true;
				this.name = "Phantasm";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 122;
				this.friendly = true;
				this.ranged = true;
				this.tileCollide = false;
				this.alpha = 255;
				this.ignoreWater = true;
				this.extraUpdates = 1;
			}
			else if (this.type == 633)
			{
				this.name = "Last Prism";
				this.width = 14;
				this.height = 18;
				this.aiStyle = 75;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.magic = true;
				this.ignoreWater = true;
			}
			else if (this.type == 632)
			{
				this.name = "Last Prism";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 84;
				this.friendly = true;
				this.magic = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.tileCollide = false;
			}
			else if (this.type == 634)
			{
				this.name = "Nebula Blaze";
				this.width = 40;
				this.height = 40;
				this.aiStyle = 1;
				this.friendly = true;
				this.alpha = 255;
				this.ignoreWater = true;
				this.extraUpdates = 2;
				this.magic = true;
			}
			else if (this.type == 635)
			{
				this.name = "Nebula Blaze Ex";
				this.width = 40;
				this.height = 40;
				this.aiStyle = 1;
				this.friendly = true;
				this.alpha = 255;
				this.friendly = true;
				this.extraUpdates = 3;
				this.magic = true;
			}
			else if (this.type == 636)
			{
				this.name = "Daybreak";
				this.width = 16;
				this.height = 16;
				this.aiStyle = 113;
				this.friendly = true;
				this.melee = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.hide = true;
				this.MaxUpdates = 2;
			}
			else if (this.type == 637)
			{
				this.name = "Bouncy Dynamite";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 16;
				this.friendly = true;
				this.penetrate = -1;
			}
			else if (this.type == 638)
			{
				this.name = "Luminite Bullet";
				this.width = 4;
				this.height = 4;
				this.aiStyle = 1;
				this.friendly = true;
				this.alpha = 255;
				this.extraUpdates = 5;
				this.timeLeft = 600;
				this.ranged = true;
				this.ignoreWater = true;
				this.updatedNPCImmunity = true;
				this.penetrate = -1;
			}
			else if (this.type == 639)
			{
				this.arrow = true;
				this.name = "Luminite Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
				this.MaxUpdates = 2;
				this.timeLeft = this.MaxUpdates * 45;
				this.ignoreWater = true;
				this.updatedNPCImmunity = true;
				this.alpha = 255;
				this.penetrate = 4;
			}
			else if (this.type == 640)
			{
				this.name = "Luminite Arrow";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.ranged = true;
				this.MaxUpdates = 3;
				this.timeLeft = 90;
				this.ignoreWater = true;
				this.updatedNPCImmunity = true;
				this.alpha = 255;
				this.penetrate = 4;
			}
			else if (this.type == 642)
			{
				this.name = "Lunar Portal Laser";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 84;
				this.friendly = true;
				this.penetrate = -1;
				this.alpha = 255;
				this.tileCollide = false;
				this.updatedNPCImmunity = true;
			}
			else if (this.type == 641)
			{
				this.name = "Lunar Portal";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 123;
				this.timeLeft = 7200;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.alpha = 255;
				this.hide = true;
			}
			else if (this.type == 643)
			{
				this.name = "Rainbow Crystal";
				this.width = 32;
				this.height = 32;
				this.aiStyle = 123;
				this.timeLeft = 7200;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.alpha = 255;
			}
			else if (this.type == 644)
			{
				this.name = "Rainbow Explosion";
				this.width = 14;
				this.height = 14;
				this.aiStyle = 112;
				this.penetrate = 1;
				this.timeLeft = 900;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.alpha = 255;
			}
			else if (this.type == 645)
			{
				this.name = "Lunar Flare";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 1;
				this.friendly = true;
				this.magic = true;
				this.tileCollide = false;
				this.extraUpdates = 5;
				this.penetrate = -1;
				this.updatedNPCImmunity = true;
			}
			else if (this.type >= 646 && this.type <= 649)
			{
				this.name = "Lunar Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 650)
			{
				this.name = "Suspicious Looking Tentacle";
				this.width = 20;
				this.height = 20;
				this.aiStyle = 124;
				this.penetrate = -1;
				this.netImportant = true;
				this.timeLeft *= 5;
				this.friendly = true;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.manualDirectionChange = true;
			}
			else if (this.type == 651)
			{
				this.name = "Wire Kite";
				this.width = 10;
				this.height = 10;
				this.aiStyle = 125;
				this.friendly = true;
				this.ignoreWater = true;
				this.tileCollide = false;
				this.penetrate = -1;
			}
			else if (this.type == 652)
			{
				this.name = "Static Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			}
			else if (this.type == 653)
			{
				this.name = "Companion Cube";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 67;
				this.penetrate = -1;
				this.netImportant = true;
				this.timeLeft *= 5;
				this.friendly = true;
				this.ignoreWater = true;
				this.scale = 0.8f;
			}
			else if (this.type == 654)
			{
				this.name = "Geyser";
				this.width = 30;
				this.height = 30;
				this.aiStyle = 126;
				this.alpha = 255;
				this.tileCollide = false;
				this.ignoreWater = true;
				this.timeLeft = 120;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
				this.trap = true;
			}
			else if (this.type == 655)
			{
				this.name = "Bee Hive";
				this.width = 31;
				this.height = 31;
				this.aiStyle = 25;
				this.friendly = true;
				this.hostile = true;
				this.penetrate = -1;
				this.trap = true;
			}
			else
			{
				this.active = false;
			}
			this.width = (int)((float)this.width * this.scale);
			this.height = (int)((float)this.height * this.scale);
			this.maxPenetrate = this.penetrate;
			ServerApi.Hooks.InvokeProjectileSetDefaults(ref Type, this);
		}
		public static int GetNextSlot()
		{
			int result = 1000;
			for (int i = 0; i < 1000; i++)
			{
				if (!Main.projectile[i].active)
				{
					result = i;
					break;
				}
			}
			return result;
		}
		public static int NewProjectile(float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0f, float ai1 = 0f)
		{
			int num = 1000;
			for (int i = 0; i < 1000; i++)
			{
				if (!Main.projectile[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == 1000)
			{
				return num;
			}
			Projectile projectile = Main.projectile[num];
			projectile.SetDefaults(Type);
			projectile.position.X = X - (float)projectile.width * 0.5f;
			projectile.position.Y = Y - (float)projectile.height * 0.5f;
			projectile.owner = Owner;
			projectile.velocity.X = SpeedX;
			projectile.velocity.Y = SpeedY;
			projectile.damage = Damage;
			projectile.knockBack = KnockBack;
			projectile.identity = num;
			projectile.gfxOffY = 0f;
			projectile.stepSpeed = 1f;
			projectile.wet = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
			if (projectile.ignoreWater)
			{
				projectile.wet = false;
			}
			projectile.honeyWet = Collision.honey;
			if (projectile.aiStyle == 1)
			{
				while (projectile.velocity.X >= 16f || projectile.velocity.X <= -16f || projectile.velocity.Y >= 16f || projectile.velocity.Y < -16f)
				{
					Projectile expr_10B_cp_0 = projectile;
					expr_10B_cp_0.velocity.X = expr_10B_cp_0.velocity.X * 0.97f;
					Projectile expr_122_cp_0 = projectile;
					expr_122_cp_0.velocity.Y = expr_122_cp_0.velocity.Y * 0.97f;
				}
			}
			if (Owner == Main.myPlayer)
			{
				if (Type == 206)
				{
					projectile.ai[0] = (float)Main.rand.Next(-100, 101) * 0.0005f;
					projectile.ai[1] = (float)Main.rand.Next(-100, 101) * 0.0005f;
				}
				else if (Type == 335)
				{
					projectile.ai[1] = (float)Main.rand.Next(4);
				}
				else if (Type == 358)
				{
					projectile.ai[1] = (float)Main.rand.Next(10, 31) * 0.1f;
				}
				else if (Type == 406)
				{
					projectile.ai[1] = (float)Main.rand.Next(10, 21) * 0.1f;
				}
				else
				{
					projectile.ai[0] = ai0;
					projectile.ai[1] = ai1;
				}
			}
			if (Type == 434)
			{
				projectile.ai[0] = projectile.position.X;
				projectile.ai[1] = projectile.position.Y;
			}
			if (Type > 0 && Type < 656)
			{
				if (ProjectileID.Sets.NeedsUUID[Type])
				{
					projectile.projUUID = projectile.identity;
				}
				if (ProjectileID.Sets.StardustDragon[Type])
				{
					int num2 = Main.projectile[(int)projectile.ai[0]].projUUID;
					if (num2 >= 0)
					{
						projectile.ai[0] = (float)num2;
					}
				}
			}
			if (Main.netMode != 0 && Owner == Main.myPlayer)
			{
				NetMessage.SendData(27, -1, -1, "", num, 0f, 0f, 0f, 0, 0, 0);
			}
			if (Owner == Main.myPlayer)
			{
				if (Type == 28)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 516)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 519)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 29)
				{
					projectile.timeLeft = 300;
				}
				if (Type == 470)
				{
					projectile.timeLeft = 300;
				}
				if (Type == 637)
				{
					projectile.timeLeft = 300;
				}
				if (Type == 30)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 517)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 37)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 75)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 133)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 136)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 139)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 142)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 397)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 419)
				{
					projectile.timeLeft = 600;
				}
				if (Type == 420)
				{
					projectile.timeLeft = 600;
				}
				if (Type == 421)
				{
					projectile.timeLeft = 600;
				}
				if (Type == 422)
				{
					projectile.timeLeft = 600;
				}
				if (Type == 588)
				{
					projectile.timeLeft = 180;
				}
				if (Type == 443)
				{
					projectile.timeLeft = 300;
				}
			}
			if (Type == 249)
			{
				projectile.frame = Main.rand.Next(5);
			}
			return num;
		}
		public void StatusNPC(int i)
		{
			if (this.melee && Main.player[this.owner].meleeEnchant > 0 && !this.noEnchantments)
			{
				int meleeEnchant = (int)Main.player[this.owner].meleeEnchant;
				if (meleeEnchant == 1)
				{
					Main.npc[i].AddBuff(70, 60 * Main.rand.Next(5, 10), false);
				}
				if (meleeEnchant == 2)
				{
					Main.npc[i].AddBuff(39, 60 * Main.rand.Next(3, 7), false);
				}
				if (meleeEnchant == 3)
				{
					Main.npc[i].AddBuff(24, 60 * Main.rand.Next(3, 7), false);
				}
				if (meleeEnchant == 5)
				{
					Main.npc[i].AddBuff(69, 60 * Main.rand.Next(10, 20), false);
				}
				if (meleeEnchant == 6)
				{
					Main.npc[i].AddBuff(31, 60 * Main.rand.Next(1, 4), false);
				}
				if (meleeEnchant == 8)
				{
					Main.npc[i].AddBuff(20, 60 * Main.rand.Next(5, 10), false);
				}
				if (meleeEnchant == 4)
				{
					Main.npc[i].AddBuff(72, 120, false);
				}
			}
			if (this.type == 195)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.npc[i].AddBuff(70, 60 * Main.rand.Next(10, 21), false);
				}
				else
				{
					Main.npc[i].AddBuff(20, 60 * Main.rand.Next(10, 21), false);
				}
			}
			if (this.type == 567 || this.type == 568)
			{
				Main.npc[i].AddBuff(20, 60 * Main.rand.Next(5, 11), false);
			}
			if (this.type == 598)
			{
				Main.npc[i].AddBuff(169, 900, false);
			}
			if (this.type == 636)
			{
				Main.npc[i].AddBuff(189, 300, false);
			}
			if (this.type == 611)
			{
				Main.npc[i].AddBuff(189, 300, false);
			}
			if (this.type == 612)
			{
				Main.npc[i].AddBuff(189, 300, false);
			}
			if (this.type == 614)
			{
				Main.npc[i].AddBuff(183, 900, false);
			}
			if (this.type == 585)
			{
				Main.npc[i].AddBuff(153, 60 * Main.rand.Next(5, 11), false);
			}
			if (this.type == 583)
			{
				Main.npc[i].AddBuff(20, 60 * Main.rand.Next(3, 6), false);
			}
			if (this.type == 524)
			{
				Main.npc[i].AddBuff(69, 60 * Main.rand.Next(3, 8), false);
			}
			if (this.type == 504 && Main.rand.Next(3) == 0)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.npc[i].AddBuff(24, Main.rand.Next(60, 180), false);
				}
				else
				{
					Main.npc[i].AddBuff(24, Main.rand.Next(30, 120), false);
				}
			}
			if (this.type == 545 && Main.rand.Next(3) == 0)
			{
				Main.npc[i].AddBuff(24, Main.rand.Next(60, 240), false);
			}
			if (this.type == 553)
			{
				Main.npc[i].AddBuff(24, Main.rand.Next(180, 480), false);
			}
			if (this.type == 552 && Main.rand.Next(3) != 0)
			{
				Main.npc[i].AddBuff(44, Main.rand.Next(120, 320), false);
			}
			if (this.type == 495)
			{
				Main.npc[i].AddBuff(153, Main.rand.Next(120, 300), false);
			}
			if (this.type == 497)
			{
				Main.npc[i].AddBuff(153, Main.rand.Next(60, 180), false);
			}
			if (this.type == 496)
			{
				Main.npc[i].AddBuff(153, Main.rand.Next(240, 480), false);
			}
			if (this.type == 476)
			{
				Main.npc[i].AddBuff(151, 30, false);
			}
			if (this.type == 523)
			{
				Main.npc[i].AddBuff(20, 60 * Main.rand.Next(10, 30), false);
			}
			if (this.type == 478 || this.type == 480)
			{
				Main.npc[i].AddBuff(39, 60 * Main.rand.Next(3, 7), false);
			}
			if (this.type == 479)
			{
				Main.npc[i].AddBuff(69, 60 * Main.rand.Next(7, 15), false);
			}
			if (this.type == 379)
			{
				Main.npc[i].AddBuff(70, 60 * Main.rand.Next(4, 7), false);
			}
			if (this.type >= 390 && this.type <= 392)
			{
				Main.npc[i].AddBuff(70, 60 * Main.rand.Next(2, 5), false);
			}
			if (this.type == 374)
			{
				Main.npc[i].AddBuff(20, 60 * Main.rand.Next(4, 7), false);
			}
			if (this.type == 376)
			{
				Main.npc[i].AddBuff(24, 60 * Main.rand.Next(3, 7), false);
			}
			if (this.type >= 399 && this.type <= 402)
			{
				Main.npc[i].AddBuff(24, 60 * Main.rand.Next(3, 7), false);
			}
			if (this.type == 295 || this.type == 296)
			{
				Main.npc[i].AddBuff(24, 60 * Main.rand.Next(8, 16), false);
			}
			if ((this.melee || this.ranged) && Main.player[this.owner].frostBurn && !this.noEnchantments)
			{
				Main.npc[i].AddBuff(44, 60 * Main.rand.Next(5, 15), false);
			}
			if (this.melee && Main.player[this.owner].magmaStone && !this.noEnchantments)
			{
				if (Main.rand.Next(7) == 0)
				{
					Main.npc[i].AddBuff(24, 360, false);
				}
				else if (Main.rand.Next(3) == 0)
				{
					Main.npc[i].AddBuff(24, 120, false);
				}
				else
				{
					Main.npc[i].AddBuff(24, 60, false);
				}
			}
			if (this.type == 287)
			{
				Main.npc[i].AddBuff(72, 120, false);
			}
			if (this.type == 285)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.npc[i].AddBuff(31, 180, false);
				}
				else
				{
					Main.npc[i].AddBuff(31, 60, false);
				}
			}
			if (this.type == 2 && Main.rand.Next(3) == 0)
			{
				Main.npc[i].AddBuff(24, 180, false);
			}
			if (this.type == 172)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.npc[i].AddBuff(44, 180, false);
				}
			}
			else if (this.type == 15)
			{
				if (Main.rand.Next(2) == 0)
				{
					Main.npc[i].AddBuff(24, 300, false);
				}
			}
			else if (this.type == 253)
			{
				if (Main.rand.Next(2) == 0)
				{
					Main.npc[i].AddBuff(44, 480, false);
				}
			}
			else if (this.type == 19)
			{
				if (Main.rand.Next(5) == 0)
				{
					Main.npc[i].AddBuff(24, 180, false);
				}
			}
			else if (this.type == 33)
			{
				if (Main.rand.Next(5) == 0)
				{
					Main.npc[i].AddBuff(20, 420, false);
				}
			}
			else if (this.type == 34)
			{
				if (Main.rand.Next(2) == 0)
				{
					Main.npc[i].AddBuff(24, Main.rand.Next(240, 480), false);
				}
			}
			else if (this.type == 35)
			{
				if (Main.rand.Next(4) == 0)
				{
					Main.npc[i].AddBuff(24, 180, false);
				}
			}
			else if (this.type == 54)
			{
				if (Main.rand.Next(2) == 0)
				{
					Main.npc[i].AddBuff(20, 600, false);
				}
			}
			else if (this.type == 267)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.npc[i].AddBuff(20, 3600, false);
				}
				else
				{
					Main.npc[i].AddBuff(20, 1800, false);
				}
			}
			else if (this.type == 63)
			{
				if (Main.rand.Next(5) != 0)
				{
					Main.npc[i].AddBuff(31, 60 * Main.rand.Next(2, 5), false);
				}
			}
			else if (this.type == 85 || this.type == 188)
			{
				Main.npc[i].AddBuff(24, 1200, false);
			}
			else if (this.type == 95 || this.type == 103 || this.type == 104)
			{
				Main.npc[i].AddBuff(39, 420, false);
			}
			else if (this.type == 278 || this.type == 279 || this.type == 280)
			{
				Main.npc[i].AddBuff(69, 600, false);
			}
			else if (this.type == 282 || this.type == 283)
			{
				Main.npc[i].AddBuff(70, 600, false);
			}
			if (this.type == 163 || this.type == 310)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.npc[i].AddBuff(24, 600, false);
					return;
				}
				Main.npc[i].AddBuff(24, 300, false);
				return;
			}
			else
			{
				if (this.type == 98)
				{
					Main.npc[i].AddBuff(20, 600, false);
					return;
				}
				if (this.type == 184)
				{
					Main.npc[i].AddBuff(20, 900, false);
					return;
				}
				if (this.type == 265)
				{
					Main.npc[i].AddBuff(20, 1800, false);
					return;
				}
				if (this.type == 355)
				{
					Main.npc[i].AddBuff(70, 1800, false);
				}
				return;
			}
		}
		public void StatusPvP(int i)
		{
			if (this.melee && Main.player[this.owner].meleeEnchant > 0 && !this.noEnchantments)
			{
				int meleeEnchant = (int)Main.player[this.owner].meleeEnchant;
				if (meleeEnchant == 1)
				{
					Main.player[i].AddBuff(70, 60 * Main.rand.Next(5, 10), true);
				}
				if (meleeEnchant == 2)
				{
					Main.player[i].AddBuff(39, 60 * Main.rand.Next(3, 7), true);
				}
				if (meleeEnchant == 3)
				{
					Main.player[i].AddBuff(24, 60 * Main.rand.Next(3, 7), true);
				}
				if (meleeEnchant == 5)
				{
					Main.player[i].AddBuff(69, 60 * Main.rand.Next(10, 20), true);
				}
				if (meleeEnchant == 6)
				{
					Main.player[i].AddBuff(31, 60 * Main.rand.Next(1, 4), true);
				}
				if (meleeEnchant == 8)
				{
					Main.player[i].AddBuff(20, 60 * Main.rand.Next(5, 10), true);
				}
			}
			if (this.type == 295 || this.type == 296)
			{
				Main.player[i].AddBuff(24, 60 * Main.rand.Next(8, 16), true);
			}
			if (this.type == 478 || this.type == 480)
			{
				Main.player[i].AddBuff(39, 60 * Main.rand.Next(3, 7), true);
			}
			if ((this.melee || this.ranged) && Main.player[this.owner].frostBurn && !this.noEnchantments)
			{
				Main.player[i].AddBuff(44, 60 * Main.rand.Next(1, 8), false);
			}
			if (this.melee && Main.player[this.owner].magmaStone && !this.noEnchantments)
			{
				if (Main.rand.Next(4) == 0)
				{
					Main.player[i].AddBuff(24, 360, true);
				}
				else if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(24, 240, true);
				}
				else
				{
					Main.player[i].AddBuff(24, 120, true);
				}
			}
			if (this.type == 2 && Main.rand.Next(3) == 0)
			{
				Main.player[i].AddBuff(24, 180, false);
			}
			if (this.type == 172)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.player[i].AddBuff(44, 240, false);
				}
			}
			else if (this.type == 15)
			{
				if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(24, 300, false);
				}
			}
			else if (this.type == 253)
			{
				if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(44, 480, false);
				}
			}
			else if (this.type == 19)
			{
				if (Main.rand.Next(5) == 0)
				{
					Main.player[i].AddBuff(24, 180, false);
				}
			}
			else if (this.type == 33)
			{
				if (Main.rand.Next(5) == 0)
				{
					Main.player[i].AddBuff(20, 420, false);
				}
			}
			else if (this.type == 34)
			{
				if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(24, 240, false);
				}
			}
			else if (this.type == 35)
			{
				if (Main.rand.Next(4) == 0)
				{
					Main.player[i].AddBuff(24, 180, false);
				}
			}
			else if (this.type == 54)
			{
				if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(20, 600, false);
				}
			}
			else if (this.type == 267)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.player[i].AddBuff(20, 3600, true);
				}
				else
				{
					Main.player[i].AddBuff(20, 1800, true);
				}
			}
			else if (this.type == 63)
			{
				if (Main.rand.Next(3) != 0)
				{
					Main.player[i].AddBuff(31, 120, true);
				}
			}
			else if (this.type == 85 || this.type == 188)
			{
				Main.player[i].AddBuff(24, 1200, false);
			}
			else if (this.type == 95 || this.type == 103 || this.type == 104)
			{
				Main.player[i].AddBuff(39, 420, true);
			}
			else if (this.type == 278 || this.type == 279 || this.type == 280)
			{
				Main.player[i].AddBuff(69, 900, true);
			}
			else if (this.type == 282 || this.type == 283)
			{
				Main.player[i].AddBuff(70, 600, true);
			}
			if (this.type == 163 || this.type == 310)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.player[i].AddBuff(24, 600, true);
					return;
				}
				Main.player[i].AddBuff(24, 300, true);
				return;
			}
			else
			{
				if (this.type == 265)
				{
					Main.player[i].AddBuff(20, 1200, true);
					return;
				}
				if (this.type == 355)
				{
					Main.player[i].AddBuff(70, 1800, true);
				}
				return;
			}
		}
		public void ghostHurt(int dmg, Vector2 Position)
		{
			if (!this.magic)
			{
				return;
			}
			int num = this.damage / 2;
			if (dmg / 2 <= 1)
			{
				return;
			}
			int num2 = 1000;
			if (Main.player[Main.myPlayer].ghostDmg > (float)num2)
			{
				return;
			}
			Main.player[Main.myPlayer].ghostDmg += (float)num;
			int[] array = new int[200];
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].CanBeChasedBy(this, false))
				{
					float num5 = Math.Abs(Main.npc[i].position.X + (float)(Main.npc[i].width / 2) - this.position.X + (float)(this.width / 2)) + Math.Abs(Main.npc[i].position.Y + (float)(Main.npc[i].height / 2) - this.position.Y + (float)(this.height / 2));
					if (num5 < 800f)
					{
						if (Collision.CanHit(this.position, 1, 1, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) && num5 > 50f)
						{
							array[num4] = i;
							num4++;
						}
						else if (num4 == 0)
						{
							array[num3] = i;
							num3++;
						}
					}
				}
			}
			if (num3 == 0 && num4 == 0)
			{
				return;
			}
			int num6;
			if (num4 > 0)
			{
				num6 = array[Main.rand.Next(num4)];
			}
			else
			{
				num6 = array[Main.rand.Next(num3)];
			}
			float num7 = 4f;
			float num8 = (float)Main.rand.Next(-100, 101);
			float num9 = (float)Main.rand.Next(-100, 101);
			float num10 = (float)Math.Sqrt((double)(num8 * num8 + num9 * num9));
			num10 = num7 / num10;
			num8 *= num10;
			num9 *= num10;
			Projectile.NewProjectile(Position.X, Position.Y, num8, num9, 356, num, 0f, this.owner, (float)num6, 0f);
		}
		public void ghostHeal(int dmg, Vector2 Position)
		{
			float num = 0.2f;
			num -= (float)this.numHits * 0.05f;
			if (num <= 0f)
			{
				return;
			}
			float num2 = (float)dmg * num;
			if ((int)num2 <= 0)
			{
				return;
			}
			if (Main.player[Main.myPlayer].lifeSteal <= 0f)
			{
				return;
			}
			Main.player[Main.myPlayer].lifeSteal -= num2;
			if (!this.magic)
			{
				return;
			}
			float num3 = 0f;
			int num4 = this.owner;
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active && !Main.player[i].dead && ((!Main.player[this.owner].hostile && !Main.player[i].hostile) || Main.player[this.owner].team == Main.player[i].team))
				{
					float num5 = Math.Abs(Main.player[i].position.X + (float)(Main.player[i].width / 2) - this.position.X + (float)(this.width / 2)) + Math.Abs(Main.player[i].position.Y + (float)(Main.player[i].height / 2) - this.position.Y + (float)(this.height / 2));
					if (num5 < 1200f && (float)(Main.player[i].statLifeMax2 - Main.player[i].statLife) > num3)
					{
						num3 = (float)(Main.player[i].statLifeMax2 - Main.player[i].statLife);
						num4 = i;
					}
				}
			}
			Projectile.NewProjectile(Position.X, Position.Y, 0f, 0f, 298, 0, 0f, this.owner, (float)num4, num2);
		}
		public void vampireHeal(int dmg, Vector2 Position)
		{
			float num = (float)dmg * 0.075f;
			if ((int)num == 0)
			{
				return;
			}
			if (Main.player[Main.myPlayer].lifeSteal <= 0f)
			{
				return;
			}
			Main.player[Main.myPlayer].lifeSteal -= num;
			int num2 = this.owner;
			Projectile.NewProjectile(Position.X, Position.Y, 0f, 0f, 305, 0, 0f, this.owner, (float)num2, num);
		}
		public void StatusPlayer(int i)
		{
			if (this.type == 472)
			{
				Main.player[i].AddBuff(149, Main.rand.Next(30, 150), true);
			}
			if (this.type == 467)
			{
				Main.player[i].AddBuff(24, Main.rand.Next(30, 150), true);
			}
			if (this.type == 581)
			{
				if (Main.expertMode)
				{
					Main.player[i].AddBuff(164, Main.rand.Next(300, 540), true);
				}
				else if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(164, Main.rand.Next(360, 720), true);
				}
			}
			if (this.type == 572 && Main.rand.Next(3) != 0)
			{
				Main.player[i].AddBuff(20, Main.rand.Next(120, 240), true);
			}
			if (this.type == 276)
			{
				if (Main.expertMode)
				{
					Main.player[i].AddBuff(20, Main.rand.Next(120, 540), true);
				}
				else if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(20, Main.rand.Next(180, 420), true);
				}
			}
			if (this.type == 436 && Main.rand.Next(5) >= 2)
			{
				Main.player[i].AddBuff(31, 300, true);
			}
			if (this.type == 435 && Main.rand.Next(3) != 0)
			{
				Main.player[i].AddBuff(144, 300, true);
			}
			if (this.type == 437)
			{
				Main.player[i].AddBuff(144, 60 * Main.rand.Next(4, 9), true);
			}
			if (this.type == 348)
			{
				if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(46, 600, true);
				}
				else
				{
					Main.player[i].AddBuff(46, 300, true);
				}
				if (Main.rand.Next(3) != 0)
				{
					if (Main.rand.Next(16) == 0)
					{
						Main.player[i].AddBuff(47, 60, true);
					}
					else if (Main.rand.Next(12) == 0)
					{
						Main.player[i].AddBuff(47, 40, true);
					}
					else if (Main.rand.Next(8) == 0)
					{
						Main.player[i].AddBuff(47, 20, true);
					}
				}
			}
			if (this.type == 349)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.player[i].AddBuff(46, 600, true);
				}
				else if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(46, 300, true);
				}
			}
			if (this.type >= 399 && this.type <= 402)
			{
				Main.npc[i].AddBuff(24, 60 * Main.rand.Next(3, 7), false);
			}
			if (this.type == 55)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.player[i].AddBuff(20, 600, true);
				}
				else if (Main.expertMode)
				{
					Main.player[i].AddBuff(20, Main.rand.Next(60, 300), true);
				}
			}
			if (this.type == 44 && Main.rand.Next(3) == 0)
			{
				Main.player[i].AddBuff(22, 900, true);
			}
			if (this.type == 293)
			{
				Main.player[i].AddBuff(80, 60 * Main.rand.Next(2, 7), true);
			}
			if (this.type == 82 && Main.rand.Next(3) == 0)
			{
				Main.player[i].AddBuff(24, 420, true);
			}
			if (this.type == 285)
			{
				if (Main.rand.Next(3) == 0)
				{
					Main.player[i].AddBuff(31, 180, true);
				}
				else
				{
					Main.player[i].AddBuff(31, 60, true);
				}
			}
			if (this.type == 96 || this.type == 101)
			{
				if (Main.rand.Next(6) == 0)
				{
					Main.player[i].AddBuff(39, 480, true);
				}
				else if (Main.rand.Next(4) == 0)
				{
					Main.player[i].AddBuff(39, 300, true);
				}
				else if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(39, 180, true);
				}
			}
			else if (this.type == 288)
			{
				Main.player[i].AddBuff(69, 900, true);
			}
			else if (this.type == 253 && Main.rand.Next(2) == 0)
			{
				Main.player[i].AddBuff(44, 600, true);
			}
			if (this.type == 291 || this.type == 292)
			{
				Main.player[i].AddBuff(24, 60 * Main.rand.Next(8, 16), true);
			}
			if (this.type == 98)
			{
				Main.player[i].AddBuff(20, 600, true);
			}
			if (this.type == 184)
			{
				Main.player[i].AddBuff(20, 900, true);
			}
			if (this.type == 290)
			{
				Main.player[i].AddBuff(32, 60 * Main.rand.Next(5, 16), true);
			}
			if (this.type == 174)
			{
				Main.player[i].AddBuff(46, 1200, true);
				if (!Main.player[i].frozen && Main.rand.Next(20) == 0)
				{
					Main.player[i].AddBuff(47, 90, true);
				}
				else if (!Main.player[i].frozen && Main.expertMode && Main.rand.Next(20) == 0)
				{
					Main.player[i].AddBuff(47, 60, true);
				}
			}
			if (this.type == 257)
			{
				Main.player[i].AddBuff(46, 2700, true);
				if (!Main.player[i].frozen && Main.rand.Next(5) == 0)
				{
					Main.player[i].AddBuff(47, 60, true);
				}
			}
			if (this.type == 177)
			{
				Main.player[i].AddBuff(46, 1500, true);
				if (!Main.player[i].frozen && Main.rand.Next(10) == 0)
				{
					Main.player[i].AddBuff(47, Main.rand.Next(30, 120), true);
				}
			}
			if (this.type == 176)
			{
				if (Main.rand.Next(4) == 0)
				{
					Main.player[i].AddBuff(20, 1200, true);
					return;
				}
				if (Main.rand.Next(2) == 0)
				{
					Main.player[i].AddBuff(20, 300, true);
				}
			}
		}

		public bool CanHit(Entity ent)
		{
			return Collision.CanHit(Main.player[this.owner].position, Main.player[this.owner].width, Main.player[this.owner].height, ent.position, ent.width, ent.height) || Collision.CanHitLine(Main.player[this.owner].Center + new Vector2((float)(Main.player[this.owner].direction * Main.player[this.owner].width / 2), Main.player[this.owner].gravDir * (float)(-(float)Main.player[this.owner].height) / 3f), 0, 0, ent.Center + new Vector2(0f, (float)(-(float)ent.height / 3)), 0, 0) || Collision.CanHitLine(Main.player[this.owner].Center + new Vector2((float)(Main.player[this.owner].direction * Main.player[this.owner].width / 2), Main.player[this.owner].gravDir * (float)(-(float)Main.player[this.owner].height) / 3f), 0, 0, ent.Center, 0, 0) || Collision.CanHitLine(Main.player[this.owner].Center + new Vector2((float)(Main.player[this.owner].direction * Main.player[this.owner].width / 2), 0f), 0, 0, ent.Center + new Vector2(0f, (float)(ent.height / 3)), 0, 0);
		}

		public void Damage()
		{
			if (this.type == 18 || this.type == 72 || this.type == 86 || this.type == 87 || this.aiStyle == 31 || this.aiStyle == 32 || this.type == 226 || this.type == 378 || this.type == 613 || this.type == 650 || (this.type == 434 && this.localAI[0] != 0f) || this.type == 439 || this.type == 444 || (this.type == 451 && ((int)(this.ai[0] - 1f) / this.penetrate == 0 || this.ai[1] < 5f) && this.ai[0] != 0f) || (this.type == 500 || this.type == 653 || this.type == 460 || this.type == 633 || this.type == 600 || this.type == 601 || this.type == 602 || this.type == 535 || (this.type == 631 && this.localAI[1] == 0f)) || this.type == 651)
			{
				return;
			}
			if (this.aiStyle == 93 && this.ai[0] != 0f && this.ai[0] != 2f)
			{
				return;
			}
			if (this.aiStyle == 10 && this.localAI[1] == -1f)
			{
				return;
			}
			if (Main.projPet[this.type] && this.type != 266 && this.type != 407 && this.type != 317 && (this.type != 388 || this.ai[0] != 2f) && (this.type < 390 || this.type > 392) && (this.type < 393 || this.type > 395) && (this.type != 533 || this.ai[0] < 6f || this.ai[0] > 8f) && (this.type < 625 || this.type > 628))
			{
				return;
			}
			Rectangle myRect = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
			if (this.type == 85 || this.type == 101)
			{
				int num = 30;
				myRect.X -= num;
				myRect.Y -= num;
				myRect.Width += num * 2;
				myRect.Height += num * 2;
			}
			if (this.type == 188)
			{
				int num2 = 20;
				myRect.X -= num2;
				myRect.Y -= num2;
				myRect.Width += num2 * 2;
				myRect.Height += num2 * 2;
			}
			if (this.aiStyle == 29)
			{
				int num3 = 4;
				myRect.X -= num3;
				myRect.Y -= num3;
				myRect.Width += num3 * 2;
				myRect.Height += num3 * 2;
			}
			if (this.friendly && this.owner == Main.myPlayer && !this.npcProj)
			{
				if ((this.aiStyle == 16 && this.type != 338 && this.type != 339 && this.type != 340 && this.type != 341 && (this.timeLeft <= 1 || this.type == 108 || this.type == 164)) || (this.type == 286 && this.localAI[1] == -1f))
				{
					int myPlayer = Main.myPlayer;
					if (Main.player[myPlayer].active && !Main.player[myPlayer].dead && !Main.player[myPlayer].immune && (!this.ownerHitCheck || this.CanHit(Main.player[myPlayer])))
					{
						Rectangle rectangle = new Rectangle((int)Main.player[myPlayer].position.X, (int)Main.player[myPlayer].position.Y, Main.player[myPlayer].width, Main.player[myPlayer].height);
						if (myRect.Intersects(rectangle))
						{
							if (Main.player[myPlayer].position.X + (float)(Main.player[myPlayer].width / 2) < this.position.X + (float)(this.width / 2))
							{
								this.direction = -1;
							}
							else
							{
								this.direction = 1;
							}
							int num4 = Main.DamageVar((float)this.damage);
							this.StatusPlayer(myPlayer);
							Main.player[myPlayer].Hurt(num4, this.direction, true, false, Lang.deathMsg(this.owner, -1, this.whoAmI, -1), false);
							if (this.trap)
							{
								Main.player[myPlayer].trapDebuffSource = true;
								if (Main.player[myPlayer].dead)
								{
									AchievementsHelper.HandleSpecialEvent(Main.player[myPlayer], 4);
								}
							}
						}
					}
				}
				if (this.aiStyle != 45 && this.aiStyle != 92 && this.aiStyle != 105 && this.aiStyle != 106 && this.type != 463 && this.type != 69 && this.type != 70 && this.type != 621 && this.type != 10 && this.type != 11 && this.type != 379 && this.type != 407 && this.type != 476 && this.type != 623 && (this.type < 625 || this.type > 628))
				{
					int num5 = (int)(this.position.X / 16f);
					int num6 = (int)((this.position.X + (float)this.width) / 16f) + 1;
					int num7 = (int)(this.position.Y / 16f);
					int num8 = (int)((this.position.Y + (float)this.height) / 16f) + 1;
					if (num5 < 0)
					{
						num5 = 0;
					}
					if (num6 > Main.maxTilesX)
					{
						num6 = Main.maxTilesX;
					}
					if (num7 < 0)
					{
						num7 = 0;
					}
					if (num8 > Main.maxTilesY)
					{
						num8 = Main.maxTilesY;
					}
					AchievementsHelper.CurrentlyMining = true;
					for (int i = num5; i < num6; i++)
					{
						for (int j = num7; j < num8; j++)
						{
							if (Main.tile[i, j] != null && Main.tileCut[(int)Main.tile[i, j].type] && Main.tile[i, j + 1] != null && Main.tile[i, j + 1].type != 78 && Main.tile[i, j + 1].type != 380)
							{
								WorldGen.KillTile(i, j, false, false, false);
								if (Main.netMode != 0)
								{
									NetMessage.SendData(17, -1, -1, "", 0, (float)i, (float)j, 0f, 0, 0, 0);
								}
							}
						}
					}
					if (this.type == 461 || this.type == 632 || this.type == 642)
					{
						Utils.PlotTileLine(base.Center, base.Center + this.velocity * this.localAI[1], (float)this.width * this.scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
					}
					else if (this.type == 611)
					{
						Utils.PlotTileLine(base.Center, base.Center + this.velocity, (float)this.width * this.scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
					}
					AchievementsHelper.CurrentlyMining = false;
				}
			}
			if (this.owner == Main.myPlayer)
			{
				if (this.damage > 0)
				{
					for (int k = 0; k < 200; k++)
					{
						bool flag = !this.updatedNPCImmunity || this.npcImmune[k] == 0;
						if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && flag && ((this.friendly && (!Main.npc[k].friendly || this.type == 318 || (Main.npc[k].type == 22 && this.owner < 255 && Main.player[this.owner].killGuide) || (Main.npc[k].type == 54 && this.owner < 255 && Main.player[this.owner].killClothier))) || (this.hostile && Main.npc[k].friendly)) && (this.owner < 0 || Main.npc[k].immune[this.owner] == 0 || this.maxPenetrate == 1))
						{
							bool flag2 = false;
							if (this.type == 11 && (Main.npc[k].type == 47 || Main.npc[k].type == 57))
							{
								flag2 = true;
							}
							else if (this.type == 31 && Main.npc[k].type == 69)
							{
								flag2 = true;
							}
							else if (Main.npc[k].trapImmune && this.trap)
							{
								flag2 = true;
							}
							else if (Main.npc[k].immortal && this.npcProj)
							{
								flag2 = true;
							}
							if (!flag2 && (Main.npc[k].noTileCollide || !this.ownerHitCheck || this.CanHit(Main.npc[k])))
							{
								bool flag3;
								if (Main.npc[k].type == 414)
								{
									Rectangle rect = Main.npc[k].getRect();
									int num9 = 8;
									rect.X -= num9;
									rect.Y -= num9;
									rect.Width += num9 * 2;
									rect.Height += num9 * 2;
									flag3 = this.Colliding(myRect, rect);
								}
								else
								{
									flag3 = this.Colliding(myRect, Main.npc[k].getRect());
								}
								if (flag3)
								{
									if (this.type == 604)
									{
										Main.player[this.owner].Counterweight(Main.npc[k].Center, this.damage, this.knockBack);
									}
									if (Main.npc[k].reflectingProjectiles && this.CanReflect())
									{
										Main.npc[k].ReflectProjectile(this.whoAmI);
										return;
									}
									if (this.type > 0 && this.type < 656 && ProjectileID.Sets.StardustDragon[this.type])
									{
										float num11 = (this.scale - 1f) * 100f;
										num11 = Utils.Clamp<float>(num11, 0f, 50f);
										this.damage = (int)((float)this.damage * (1f + num11 * 0.23f));
									}
									int num10 = Main.DamageVar((float)this.damage);
									bool flag4 = !this.npcProj && !this.trap;
									if (this.type == 604)
									{
										this.friendly = false;
										this.ai[1] = 1000f;
									}
									if ((this.type == 400 || this.type == 401 || this.type == 402) && Main.npc[k].type >= 13 && Main.npc[k].type <= 15)
									{
										num10 = (int)((double)num10 * 0.65);
										if (this.penetrate > 1)
										{
											this.penetrate--;
										}
									}
									if (this.type == 504)
									{
										float num11 = (60f - this.ai[0]) / 2f;
										this.ai[0] += num11;
									}
									if (this.aiStyle == 3 && this.type != 301)
									{
										if (this.ai[0] == 0f)
										{
											this.velocity.X = -this.velocity.X;
											this.velocity.Y = -this.velocity.Y;
											this.netUpdate = true;
										}
										this.ai[0] = 1f;
									}
									else if (this.type == 582)
									{
										if (this.ai[0] != 0f)
										{
											this.direction *= -1;
										}
									}
									else if (this.type == 612)
									{
										this.direction = Main.player[this.owner].direction;
									}
									else if (this.type == 624)
									{
										float num12 = 1f;
										if (Main.npc[k].knockBackResist > 0f)
										{
											num12 = 1f / Main.npc[k].knockBackResist;
										}
										this.knockBack = 4f * num12;
										if (Main.npc[k].Center.X < base.Center.X)
										{
											this.direction = 1;
										}
										else
										{
											this.direction = -1;
										}
									}
									else if (this.aiStyle == 16)
									{
										if (this.timeLeft > 3)
										{
											this.timeLeft = 3;
										}
										if (Main.npc[k].position.X + (float)(Main.npc[k].width / 2) < this.position.X + (float)(this.width / 2))
										{
											this.direction = -1;
										}
										else
										{
											this.direction = 1;
										}
									}
									else if (this.aiStyle == 68)
									{
										if (this.timeLeft > 3)
										{
											this.timeLeft = 3;
										}
										if (Main.npc[k].position.X + (float)(Main.npc[k].width / 2) < this.position.X + (float)(this.width / 2))
										{
											this.direction = -1;
										}
										else
										{
											this.direction = 1;
										}
									}
									else if (this.aiStyle == 50)
									{
										if (Main.npc[k].position.X + (float)(Main.npc[k].width / 2) < this.position.X + (float)(this.width / 2))
										{
											this.direction = -1;
										}
										else
										{
											this.direction = 1;
										}
									}
									if (this.type == 509)
									{
										int num13 = Main.rand.Next(2, 6);
										for (int l = 0; l < num13; l++)
										{
											Vector2 vector = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
											vector += this.velocity * 3f;
											vector.Normalize();
											vector *= (float)Main.rand.Next(35, 81) * 0.1f;
											int num14 = (int)((double)this.damage * 0.5);
											Projectile.NewProjectile(base.Center.X, base.Center.Y, vector.X, vector.Y, 504, num14, this.knockBack * 0.2f, this.owner, 0f, 0f);
										}
									}
									if (this.type == 598 || this.type == 636 || this.type == 614)
									{
										this.ai[0] = 1f;
										this.ai[1] = (float)k;
										this.velocity = (Main.npc[k].Center - base.Center) * 0.75f;
										this.netUpdate = true;
									}
									if (this.type >= 511 && this.type <= 513)
									{
										this.timeLeft = 0;
									}
									if (this.type == 524)
									{
										this.netUpdate = true;
										this.ai[0] += 50f;
									}
									if (this.aiStyle == 39)
									{
										if (this.ai[1] == 0f)
										{
											this.ai[1] = (float)(k + 1);
											this.netUpdate = true;
										}
										if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) < this.position.X + (float)(this.width / 2))
										{
											this.direction = 1;
										}
										else
										{
											this.direction = -1;
										}
									}
									if (this.type == 41 && this.timeLeft > 1)
									{
										this.timeLeft = 1;
									}
									bool flag5 = false;
									if (flag4)
									{
										if (this.melee && Main.rand.Next(1, 101) <= Main.player[this.owner].meleeCrit)
										{
											flag5 = true;
										}
										if (this.ranged && Main.rand.Next(1, 101) <= Main.player[this.owner].rangedCrit)
										{
											flag5 = true;
										}
										if (this.magic && Main.rand.Next(1, 101) <= Main.player[this.owner].magicCrit)
										{
											flag5 = true;
										}
										if (this.thrown && Main.rand.Next(1, 101) <= Main.player[this.owner].thrownCrit)
										{
											flag5 = true;
										}
									}
									if (this.aiStyle == 99)
									{
										Main.player[this.owner].Counterweight(Main.npc[k].Center, this.damage, this.knockBack);
										if (Main.npc[k].Center.X < Main.player[this.owner].Center.X)
										{
											this.direction = -1;
										}
										else
										{
											this.direction = 1;
										}
										if (this.ai[0] >= 0f)
										{
											Vector2 vector2 = base.Center - Main.npc[k].Center;
											vector2.Normalize();
											float num15 = 16f;
											this.velocity *= -0.5f;
											this.velocity += vector2 * num15;
											this.netUpdate = true;
											this.localAI[0] += 20f;
											if (!Collision.CanHit(this.position, this.width, this.height, Main.player[this.owner].position, Main.player[this.owner].width, Main.player[this.owner].height))
											{
												this.localAI[0] += 40f;
												num10 = (int)((double)num10 * 0.75);
											}
										}
									}
									if (this.aiStyle == 93)
									{
										if (this.ai[0] == 0f)
										{
											this.ai[1] = 0f;
											int num16 = -k - 1;
											this.ai[0] = (float)num16;
											this.velocity = Main.npc[k].Center - base.Center;
										}
										if (this.ai[0] == 2f)
										{
											num10 = (int)((double)num10 * 1.35);
										}
										else
										{
											num10 = (int)((double)num10 * 0.15);
										}
									}
									if (flag4)
									{
										int num17 = Item.NPCtoBanner(Main.npc[k].BannerID());
										if (num17 >= 0)
										{
											Main.player[Main.myPlayer].lastCreatureHit = num17;
										}
									}
									if (Main.netMode != 2 && flag4)
									{
										int num18 = Item.NPCtoBanner(Main.npc[k].BannerID());
										if (num18 > 0 && Main.player[this.owner].NPCBannerBuff[num18])
										{
											if (Main.expertMode)
											{
												num10 *= 2;
											}
											else
											{
												num10 = (int)((double)num10 * 1.5);
											}
										}
									}
									if (Main.expertMode)
									{
										if ((this.type == 30 || this.type == 28 || this.type == 29 || this.type == 470 || this.type == 517 || this.type == 588 || this.type == 637) && Main.npc[k].type >= 13 && Main.npc[k].type <= 15)
										{
											num10 /= 5;
										}
										if (this.type == 280 && ((Main.npc[k].type >= 134 && Main.npc[k].type <= 136) || Main.npc[k].type == 139))
										{
											num10 = (int)((double)num10 * 0.75);
										}
									}
									if (Main.netMode != 2 && Main.npc[k].type == 439 && this.type >= 0 && this.type <= 656 && ProjectileID.Sets.Homing[this.type])
									{
										num10 = (int)((float)num10 * 0.75f);
									}
									if (this.type == 497 && this.penetrate != 1)
									{
										this.ai[0] = 25f;
										float num19 = this.velocity.Length();
										Vector2 vector3 = Main.npc[k].Center - base.Center;
										vector3.Normalize();
										vector3 *= num19;
										this.velocity = -vector3 * 0.9f;
										this.netUpdate = true;
									}
									if (this.type == 323 && (Main.npc[k].type == 158 || Main.npc[k].type == 159))
									{
										num10 *= 10;
									}
									if (this.type == 294)
									{
										this.damage = (int)((double)this.damage * 0.8);
									}
									if (this.type == 477 && this.penetrate > 1)
									{
										int[] array = new int[10];
										int num20 = 0;
										int num21 = 700;
										int num22 = 20;
										for (int m = 0; m < 200; m++)
										{
											if (m != k && Main.npc[m].CanBeChasedBy(this, false))
											{
												float num23 = (base.Center - Main.npc[m].Center).Length();
												if (num23 > (float)num22 && num23 < (float)num21 && Collision.CanHitLine(base.Center, 1, 1, Main.npc[m].Center, 1, 1))
												{
													array[num20] = m;
													num20++;
													if (num20 >= 9)
													{
														break;
													}
												}
											}
										}
										if (num20 > 0)
										{
											num20 = Main.rand.Next(num20);
											Vector2 vector4 = Main.npc[array[num20]].Center - base.Center;
											float num24 = this.velocity.Length();
											vector4.Normalize();
											this.velocity = vector4 * num24;
											this.netUpdate = true;
										}
									}
									if (this.type == 261)
									{
										float num25 = (float)Math.Sqrt((double)(this.velocity.X * this.velocity.X + this.velocity.Y * this.velocity.Y));
										if (num25 < 1f)
										{
											num25 = 1f;
										}
										num10 = (int)((float)num10 * num25 / 8f);
									}
									this.StatusNPC(k);
									if (flag4 && this.type != 221 && this.type != 227 && this.type != 614)
									{
										Main.player[this.owner].OnHit(Main.npc[k].Center.X, Main.npc[k].Center.Y, Main.npc[k]);
									}
									if (this.type == 317)
									{
										this.ai[1] = -1f;
										this.netUpdate = true;
									}
									if (flag4 && !this.hostile && Main.player[this.owner].armorPenetration > 0)
									{
										num10 += Main.npc[k].checkArmorPenetration(Main.player[this.owner].armorPenetration);
									}
									int num26;
									if (flag4)
									{
										num26 = (int)Main.npc[k].StrikeNPC(num10, this.knockBack, this.direction, flag5, false, false);
									}
									else
									{
										num26 = (int)Main.npc[k].StrikeNPCNoInteraction(num10, this.knockBack, this.direction, flag5, false, false);
									}
									if (flag4 && Main.player[this.owner].accDreamCatcher)
									{
										Main.player[this.owner].addDPS(num26);
									}
									if (flag4 && !Main.npc[k].immortal)
									{
										if (this.type == 304 && num26 > 0 && Main.npc[k].lifeMax > 5)
										{
											this.vampireHeal(num26, new Vector2(Main.npc[k].Center.X, Main.npc[k].Center.Y));
										}
										if (Main.npc[k].value > 0f && Main.player[this.owner].coins && Main.rand.Next(5) == 0)
										{
											int num27 = 71;
											if (Main.rand.Next(10) == 0)
											{
												num27 = 72;
											}
											if (Main.rand.Next(100) == 0)
											{
												num27 = 73;
											}
											int num28 = Item.NewItem((int)Main.npc[k].position.X, (int)Main.npc[k].position.Y, Main.npc[k].width, Main.npc[k].height, num27, 1, false, 0, false);
											Main.item[num28].stack = Main.rand.Next(1, 11);
											Main.item[num28].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
											Main.item[num28].velocity.X = (float)Main.rand.Next(10, 31) * 0.2f * (float)this.direction;
											if (Main.netMode == 1)
											{
												NetMessage.SendData(21, -1, -1, "", num28, 0f, 0f, 0f, 0, 0, 0);
											}
										}
										if (num26 > 0 && Main.npc[k].lifeMax > 5 && this.friendly && !this.hostile && this.aiStyle != 59)
										{
											if (Main.npc[k].canGhostHeal)
											{
												if (Main.player[this.owner].ghostHeal)
												{
													this.ghostHeal(num26, new Vector2(Main.npc[k].Center.X, Main.npc[k].Center.Y));
												}
												if (Main.player[this.owner].ghostHurt)
												{
													this.ghostHurt(num26, new Vector2(Main.npc[k].Center.X, Main.npc[k].Center.Y));
												}
												if (Main.player[this.owner].setNebula && Main.player[this.owner].nebulaCD == 0 && Main.rand.Next(3) == 0)
												{
													Main.player[this.owner].nebulaCD = 30;
													int num29 = Utils.SelectRandom<int>(Main.rand, new int[]
													{
														3453,
														3454,
														3455
													});
													int num30 = Item.NewItem((int)Main.npc[k].position.X, (int)Main.npc[k].position.Y, Main.npc[k].width, Main.npc[k].height, num29, 1, false, 0, false);
													Main.item[num30].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
													Main.item[num30].velocity.X = (float)Main.rand.Next(10, 31) * 0.2f * (float)this.direction;
													if (Main.netMode == 1)
													{
														NetMessage.SendData(21, -1, -1, "", num30, 0f, 0f, 0f, 0, 0, 0);
													}
												}
											}
											if (this.melee && Main.player[this.owner].beetleOffense)
											{
												if (Main.player[this.owner].beetleOrbs == 0)
												{
													Main.player[this.owner].beetleCounter += (float)(num26 * 3);
												}
												else if (Main.player[this.owner].beetleOrbs == 1)
												{
													Main.player[this.owner].beetleCounter += (float)(num26 * 2);
												}
												else
												{
													Main.player[this.owner].beetleCounter += (float)num26;
												}
												Main.player[this.owner].beetleCountdown = 0;
											}
											if (this.arrow && this.type != 631 && Main.player[this.owner].phantasmTime > 0)
											{
												Vector2 source = Main.player[this.owner].position + Main.player[this.owner].Size * Utils.RandomVector2(Main.rand, 0f, 1f);
												Vector2 vector5 = Main.npc[k].DirectionFrom(source) * 6f;
												Projectile.NewProjectile(source.X, source.Y, vector5.X, vector5.Y, 631, this.damage, 0f, this.owner, (float)k, 0f);
												Projectile.NewProjectile(source.X, source.Y, vector5.X, vector5.Y, 631, this.damage, 0f, this.owner, (float)k, 15f);
												Projectile.NewProjectile(source.X, source.Y, vector5.X, vector5.Y, 631, this.damage, 0f, this.owner, (float)k, 30f);
											}
										}
									}
									if (flag4 && this.melee && Main.player[this.owner].meleeEnchant == 7)
									{
										Projectile.NewProjectile(Main.npc[k].Center.X, Main.npc[k].Center.Y, Main.npc[k].velocity.X, Main.npc[k].velocity.Y, 289, 0, 0f, this.owner, 0f, 0f);
									}
									if (Main.netMode != 0)
									{
										if (flag5)
										{
											NetMessage.SendData(28, -1, -1, "", k, (float)num10, this.knockBack, (float)this.direction, 1, 0, 0);
										}
										else
										{
											NetMessage.SendData(28, -1, -1, "", k, (float)num10, this.knockBack, (float)this.direction, 0, 0, 0);
										}
									}
									if (this.type >= 390 && this.type <= 392)
									{
										this.localAI[1] = 20f;
									}
									if (this.type == 434)
									{
										this.numUpdates = 0;
									}
									else if (this.type == 598 || this.type == 636 || this.type == 614)
									{
										this.damage = 0;
										int num31 = 6;
										if (this.type == 614)
										{
											num31 = 10;
										}
										if (this.type == 636)
										{
											num31 = 8;
										}
										Point[] array2 = new Point[num31];
										int num32 = 0;
										for (int n = 0; n < 1000; n++)
										{
											if (n != this.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == this.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == (float)k)
											{
												array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
												if (num32 >= array2.Length)
												{
													break;
												}
											}
										}
										if (num32 >= array2.Length)
										{
											int num33 = 0;
											for (int num34 = 1; num34 < array2.Length; num34++)
											{
												if (array2[num34].Y < array2[num33].Y)
												{
													num33 = num34;
												}
											}
											Main.projectile[array2[num33].X].Kill();
										}
									}
									else if (this.type == 632)
									{
										Main.npc[k].immune[this.owner] = 5;
									}
									else if (this.type == 514)
									{
										Main.npc[k].immune[this.owner] = 1;
									}
									else if (this.type == 611)
									{
										if (this.localAI[1] <= 0f)
										{
											Projectile.NewProjectile(Main.npc[k].Center.X, Main.npc[k].Center.Y, 0f, 0f, 612, this.damage, 10f, this.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
										}
										this.localAI[1] = 4f;
									}
									else if (this.type == 595)
									{
										Main.npc[k].immune[this.owner] = 5;
									}
									else if (this.type >= 625 && this.type <= 628)
									{
										Main.npc[k].immune[this.owner] = 6;
									}
									else if (this.type == 286)
									{
										Main.npc[k].immune[this.owner] = 5;
									}
									else if (this.type == 514)
									{
										Main.npc[k].immune[this.owner] = 3;
									}
									else if (this.type == 443)
									{
										Main.npc[k].immune[this.owner] = 8;
									}
									else if (this.type >= 424 && this.type <= 426)
									{
										Main.npc[k].immune[this.owner] = 5;
									}
									else if (this.type == 634 || this.type == 635)
									{
										Main.npc[k].immune[this.owner] = 5;
									}
									else if (this.type == 246)
									{
										Main.npc[k].immune[this.owner] = 7;
									}
									else if (this.type == 249)
									{
										Main.npc[k].immune[this.owner] = 7;
									}
									else if (this.type == 190)
									{
										Main.npc[k].immune[this.owner] = 8;
									}
									else if (this.type == 409)
									{
										Main.npc[k].immune[this.owner] = 6;
									}
									else if (this.type == 407)
									{
										Main.npc[k].immune[this.owner] = 20;
									}
									else if (this.type == 311)
									{
										Main.npc[k].immune[this.owner] = 7;
									}
									else if (this.type == 582)
									{
										Main.npc[k].immune[this.owner] = 7;
										if (this.ai[0] != 1f)
										{
											this.ai[0] = 1f;
											this.netUpdate = true;
										}
									}
									else
									{
										if (this.type == 451)
										{
											if (this.ai[0] == 0f)
											{
												this.ai[0] += (float)this.penetrate;
											}
											else
											{
												this.ai[0] -= (float)(this.penetrate + 1);
											}
											this.ai[1] = 0f;
											this.netUpdate = true;
											break;
										}
										if (this.penetrate != 1)
										{
											Main.npc[k].immune[this.owner] = 10;
										}
									}
									if (this.penetrate > 0 && this.type != 317)
									{
										if (this.type == 357)
										{
											this.damage = (int)((double)this.damage * 0.9);
										}
										this.penetrate--;
										if (this.penetrate == 0)
										{
											break;
										}
									}
									if (this.aiStyle == 7)
									{
										this.ai[0] = 1f;
										this.damage = 0;
										this.netUpdate = true;
									}
									else if (this.aiStyle == 13)
									{
										this.ai[0] = 1f;
										this.netUpdate = true;
									}
									else if (this.aiStyle == 69)
									{
										this.ai[0] = 1f;
										this.netUpdate = true;
									}
									else if (this.type == 607)
									{
										this.ai[0] = 1f;
										this.netUpdate = true;
										this.friendly = false;
									}
									else if (this.type == 638 || this.type == 639 || this.type == 640)
									{
										this.npcImmune[k] = -1;
										Main.npc[k].immune[this.owner] = 0;
										this.damage = (int)((double)this.damage * 0.96);
									}
									else if (this.type == 617)
									{
										this.npcImmune[k] = 8;
										Main.npc[k].immune[this.owner] = 0;
									}
									else if (this.type == 618)
									{
										this.npcImmune[k] = 20;
										Main.npc[k].immune[this.owner] = 0;
									}
									else if (this.type == 642)
									{
										this.npcImmune[k] = 10;
										Main.npc[k].immune[this.owner] = 0;
									}
									else if (this.type == 611 || this.type == 612)
									{
										this.npcImmune[k] = 6;
										Main.npc[k].immune[this.owner] = 4;
									}
									else if (this.type == 645)
									{
										this.npcImmune[k] = -1;
										Main.npc[k].immune[this.owner] = 0;
										if (this.ai[1] != -1f)
										{
											this.ai[0] = 0f;
											this.ai[1] = -1f;
											this.netUpdate = true;
										}
									}
									this.numHits++;
								}
							}
						}
					}
				}
				if (this.damage > 0 && Main.player[Main.myPlayer].hostile)
				{
					for (int num35 = 0; num35 < 255; num35++)
					{
						if (num35 != this.owner && Main.player[num35].active && !Main.player[num35].dead && !Main.player[num35].immune && Main.player[num35].hostile && this.playerImmune[num35] <= 0 && (Main.player[Main.myPlayer].team == 0 || Main.player[Main.myPlayer].team != Main.player[num35].team) && (!this.ownerHitCheck || Collision.CanHit(Main.player[this.owner].position, Main.player[this.owner].width, Main.player[this.owner].height, Main.player[num35].position, Main.player[num35].width, Main.player[num35].height)))
						{
							bool flag5 = this.Colliding(myRect, Main.player[num35].getRect());
							if (flag5)
							{
								if (this.aiStyle == 3)
								{
									if (this.ai[0] == 0f)
									{
										this.velocity.X = -this.velocity.X;
										this.velocity.Y = -this.velocity.Y;
										this.netUpdate = true;
									}
									this.ai[0] = 1f;
								}
								else if (this.aiStyle == 16)
								{
									if (this.timeLeft > 3)
									{
										this.timeLeft = 3;
									}
									if (Main.player[num35].position.X + (float)(Main.player[num35].width / 2) < this.position.X + (float)(this.width / 2))
									{
										this.direction = -1;
									}
									else
									{
										this.direction = 1;
									}
								}
								else if (this.aiStyle == 68)
								{
									if (this.timeLeft > 3)
									{
										this.timeLeft = 3;
									}
									if (Main.player[num35].position.X + (float)(Main.player[num35].width / 2) < this.position.X + (float)(this.width / 2))
									{
										this.direction = -1;
									}
									else
									{
										this.direction = 1;
									}
								}
								if (this.type == 41 && this.timeLeft > 1)
								{
									this.timeLeft = 1;
								}
								bool flag6 = false;
								if (this.melee && Main.rand.Next(1, 101) <= Main.player[this.owner].meleeCrit)
								{
									flag6 = true;
								}
								int num36 = Main.DamageVar((float)this.damage);
								if (!Main.player[num35].immune)
								{
									this.StatusPvP(num35);
								}
								if (this.type != 221 && this.type != 227 && this.type != 614)
								{
									Main.player[this.owner].OnHit(Main.player[num35].Center.X, Main.player[num35].Center.Y, Main.player[num35]);
								}
								int num37 = (int)Main.player[num35].Hurt(num36, this.direction, true, false, Lang.deathMsg(this.owner, -1, this.whoAmI, -1), flag6);
								if (num37 > 0 && Main.player[this.owner].ghostHeal && this.friendly && !this.hostile)
								{
									this.ghostHeal(num37, new Vector2(Main.player[num35].Center.X, Main.player[num35].Center.Y));
								}
								if (this.type == 304 && num37 > 0)
								{
									this.vampireHeal(num37, new Vector2(Main.player[num35].Center.X, Main.player[num35].Center.Y));
								}
								if (this.melee && Main.player[this.owner].meleeEnchant == 7)
								{
									Projectile.NewProjectile(Main.player[num35].Center.X, Main.player[num35].Center.Y, Main.player[num35].velocity.X, Main.player[num35].velocity.Y, 289, 0, 0f, this.owner, 0f, 0f);
								}
								if (Main.netMode != 0)
								{
									if (flag6)
									{
										NetMessage.SendData(26, -1, -1, Lang.deathMsg(this.owner, -1, this.whoAmI, -1), num35, (float)this.direction, (float)num36, 1f, 1, 0, 0);
									}
									else
									{
										NetMessage.SendData(26, -1, -1, Lang.deathMsg(this.owner, -1, this.whoAmI, -1), num35, (float)this.direction, (float)num36, 1f, 0, 0, 0);
									}
								}
								this.playerImmune[num35] = 40;
								if (this.penetrate > 0)
								{
									this.penetrate--;
									if (this.penetrate == 0)
									{
										break;
									}
								}
								if (this.aiStyle == 7)
								{
									this.ai[0] = 1f;
									this.damage = 0;
									this.netUpdate = true;
								}
								else if (this.aiStyle == 13)
								{
									this.ai[0] = 1f;
									this.netUpdate = true;
								}
								else if (this.aiStyle == 69)
								{
									this.ai[0] = 1f;
									this.netUpdate = true;
								}
							}
						}
					}
				}
			}
			if (this.type == 10 && Main.netMode != 1)
			{
				for (int num38 = 0; num38 < 200; num38++)
				{
					if (Main.npc[num38].active && Main.npc[num38].type == 534)
					{
						Rectangle rectangle2 = new Rectangle((int)Main.npc[num38].position.X, (int)Main.npc[num38].position.Y, Main.npc[num38].width, Main.npc[num38].height);
						if (myRect.Intersects(rectangle2))
						{
							Main.npc[num38].Transform(441);
						}
					}
				}
			}
			if (this.type == 11 && Main.netMode != 1)
			{
				for (int num39 = 0; num39 < 200; num39++)
				{
					if (Main.npc[num39].active)
					{
						if (Main.npc[num39].type == 46 || Main.npc[num39].type == 303)
						{
							Rectangle rectangle3 = new Rectangle((int)Main.npc[num39].position.X, (int)Main.npc[num39].position.Y, Main.npc[num39].width, Main.npc[num39].height);
							if (myRect.Intersects(rectangle3))
							{
								Main.npc[num39].Transform(47);
							}
						}
						else if (Main.npc[num39].type == 55)
						{
							Rectangle rectangle4 = new Rectangle((int)Main.npc[num39].position.X, (int)Main.npc[num39].position.Y, Main.npc[num39].width, Main.npc[num39].height);
							if (myRect.Intersects(rectangle4))
							{
								Main.npc[num39].Transform(57);
							}
						}
						else if (Main.npc[num39].type == 148 || Main.npc[num39].type == 149)
						{
							Rectangle rectangle5 = new Rectangle((int)Main.npc[num39].position.X, (int)Main.npc[num39].position.Y, Main.npc[num39].width, Main.npc[num39].height);
							if (myRect.Intersects(rectangle5))
							{
								Main.npc[num39].Transform(168);
							}
						}
					}
				}
			}
			if (this.type == 463 && Main.netMode != 1)
			{
				for (int num40 = 0; num40 < 200; num40++)
				{
					if (Main.npc[num40].active)
					{
						if (Main.npc[num40].type == 46 || Main.npc[num40].type == 303)
						{
							Rectangle rectangle6 = new Rectangle((int)Main.npc[num40].position.X, (int)Main.npc[num40].position.Y, Main.npc[num40].width, Main.npc[num40].height);
							if (myRect.Intersects(rectangle6))
							{
								Main.npc[num40].Transform(464);
							}
						}
						else if (Main.npc[num40].type == 55)
						{
							Rectangle rectangle7 = new Rectangle((int)Main.npc[num40].position.X, (int)Main.npc[num40].position.Y, Main.npc[num40].width, Main.npc[num40].height);
							if (myRect.Intersects(rectangle7))
							{
								Main.npc[num40].Transform(465);
							}
						}
						else if (Main.npc[num40].type == 148 || Main.npc[num40].type == 149)
						{
							Rectangle rectangle8 = new Rectangle((int)Main.npc[num40].position.X, (int)Main.npc[num40].position.Y, Main.npc[num40].width, Main.npc[num40].height);
							if (myRect.Intersects(rectangle8))
							{
								Main.npc[num40].Transform(470);
							}
						}
					}
				}
			}
			if (Main.netMode != 2 && this.hostile && Main.myPlayer < 255 && this.damage > 0)
			{
				int myPlayer2 = Main.myPlayer;
				if (Main.player[myPlayer2].active && !Main.player[myPlayer2].dead && !Main.player[myPlayer2].immune)
				{
					bool flag7 = this.Colliding(myRect, Main.player[myPlayer2].getRect());
					if (flag7)
					{
						int hitDirection = this.direction;
						if (Main.player[myPlayer2].position.X + (float)(Main.player[myPlayer2].width / 2) < this.position.X + (float)(this.width / 2))
						{
							hitDirection = -1;
						}
						else
						{
							hitDirection = 1;
						}
						int num41 = Main.DamageVar((float)this.damage);
						if (!Main.player[myPlayer2].immune)
						{
							this.StatusPlayer(myPlayer2);
						}
						if (Main.player[myPlayer2].resistCold && this.coldDamage)
						{
							num41 = (int)((float)num41 * 0.7f);
						}
						if (Main.expertMode)
						{
							num41 = (int)((float)num41 * Main.expertDamage);
						}
						int cooldownCounter = -1;
						if (this.type == 455 || this.type == 452 || this.type == 454 || this.type == 462)
						{
							cooldownCounter = 1;
						}
						Main.player[myPlayer2].Hurt(num41 * 2, hitDirection, false, false, Lang.deathMsg(-1, -1, this.whoAmI, -1), false, cooldownCounter);
						if (this.trap)
						{
							Main.player[myPlayer2].trapDebuffSource = true;
							if (Main.player[myPlayer2].dead)
							{
								AchievementsHelper.HandleSpecialEvent(Main.player[myPlayer2], 4);
							}
						}
						if (this.type == 435)
						{
							this.penetrate--;
						}
						if (this.type == 436)
						{
							this.penetrate--;
						}
						if (this.type == 437)
						{
							this.penetrate--;
						}
					}
				}
			}
		}
		public bool Colliding(Rectangle myRect, Rectangle targetRect)
		{
			if (this.type == 598 && targetRect.Width > 8 && targetRect.Height > 8)
			{
				targetRect.Inflate(-targetRect.Width / 8, -targetRect.Height / 8);
			}
			else if (this.type == 614 && targetRect.Width > 8 && targetRect.Height > 8)
			{
				targetRect.Inflate(-targetRect.Width / 8, -targetRect.Height / 8);
			}
			else if (this.type == 636 && targetRect.Width > 8 && targetRect.Height > 8)
			{
				targetRect.Inflate(-targetRect.Width / 8, -targetRect.Height / 8);
			}
			else if (this.type == 607)
			{
				myRect.X += (int)this.velocity.X;
				myRect.Y += (int)this.velocity.Y;
			}
			if (myRect.Intersects(targetRect))
			{
				return true;
			}
			if (this.type == 461)
			{
				float num = 0f;
				if (Collision.CheckAABBvLineCollision(targetRect.TopLeft(), targetRect.Size(), base.Center, base.Center + this.velocity * this.localAI[1], 22f * this.scale, ref num))
				{
					return true;
				}
			}
			else if (this.type == 642)
			{
				float num2 = 0f;
				if (Collision.CheckAABBvLineCollision(targetRect.TopLeft(), targetRect.Size(), base.Center, base.Center + this.velocity * this.localAI[1], 30f * this.scale, ref num2))
				{
					return true;
				}
			}
			else if (this.type == 632)
			{
				float num3 = 0f;
				if (Collision.CheckAABBvLineCollision(targetRect.TopLeft(), targetRect.Size(), base.Center, base.Center + this.velocity * this.localAI[1], 22f * this.scale, ref num3))
				{
					return true;
				}
			}
			else if (this.type == 455)
			{
				float num4 = 0f;
				if (Collision.CheckAABBvLineCollision(targetRect.TopLeft(), targetRect.Size(), base.Center, base.Center + this.velocity * this.localAI[1], 36f * this.scale, ref num4))
				{
					return true;
				}
			}
			else if (this.type == 611)
			{
				float num5 = 0f;
				if (Collision.CheckAABBvLineCollision(targetRect.TopLeft(), targetRect.Size(), base.Center, base.Center + this.velocity, 16f * this.scale, ref num5))
				{
					return true;
				}
			}
			else if (this.type == 537)
			{
				float num6 = 0f;
				if (Collision.CheckAABBvLineCollision(targetRect.TopLeft(), targetRect.Size(), base.Center, base.Center + this.velocity * this.localAI[1], 22f * this.scale, ref num6))
				{
					return true;
				}
			}
			else if (this.type == 466 || this.type == 580)
			{
				for (int i = 0; i < this.oldPos.Length; i++)
				{
					myRect.X = (int)this.oldPos[i].X;
					myRect.Y = (int)this.oldPos[i].Y;
					if (myRect.Intersects(targetRect))
					{
						return true;
					}
				}
			}
			else if (this.type == 464 && this.ai[1] != 1f)
			{
				Vector2 vector = new Vector2(0f, -720f).RotatedBy((double)this.velocity.ToRotation(), default(Vector2));
				float num7 = this.ai[0] % 45f / 45f;
				Vector2 spinningpoint = vector * num7;
				for (int j = 0; j < 6; j++)
				{
					float num8 = (float)j * 6.28318548f / 6f;
					Vector2 center = base.Center + spinningpoint.RotatedBy((double)num8, default(Vector2));
					if (Utils.CenteredRectangle(center, new Vector2(30f, 30f)).Intersects(targetRect))
					{
						return true;
					}
				}
			}
			return false;
		}

		public Rectangle getRect()
		{
			return new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
		}
		public void Update(int i)
		{
			if (!this.active)
			{
				return;
			}
			this.numUpdates = this.extraUpdates;
			while (this.numUpdates >= 0)
			{
				this.numUpdates--;
				if (this.type == 640 && this.ai[1] > 0f)
				{
					this.ai[1] -= 1f;
				}
				else
				{
					if (this.position.X <= Main.leftWorld || this.position.X + (float)this.width >= Main.rightWorld || this.position.Y <= Main.topWorld || this.position.Y + (float)this.height >= Main.bottomWorld)
					{
						this.active = false;
						return;
					}
					if (this.minion && this.numUpdates == -1 && this.type != 625 && this.type != 628)
					{
						this.minionPos = Main.player[this.owner].numMinions;
						if (Main.player[this.owner].slotsMinions + this.minionSlots > (float)Main.player[this.owner].maxMinions && this.owner == Main.myPlayer)
						{
							if (this.type == 627 || this.type == 626)
							{
								Projectile projectile = Main.projectile[(int)this.ai[0]];
								if (projectile.type != 625)
								{
									projectile.localAI[1] = this.localAI[1];
								}
								projectile = Main.projectile[(int)this.localAI[1]];
								projectile.ai[0] = this.ai[0];
								projectile.ai[1] = 1f;
								projectile.netUpdate = true;
							}
							this.Kill();
						}
						else
						{
							Main.player[this.owner].numMinions++;
							Main.player[this.owner].slotsMinions += this.minionSlots;
						}
					}
					float num15 = 1f + Math.Abs(this.velocity.X) / 3f;
					if (this.gfxOffY > 0f)
					{
						this.gfxOffY -= num15 * this.stepSpeed;
						if (this.gfxOffY < 0f)
						{
							this.gfxOffY = 0f;
						}
					}
					else if (this.gfxOffY < 0f)
					{
						this.gfxOffY += num15 * this.stepSpeed;
						if (this.gfxOffY > 0f)
						{
							this.gfxOffY = 0f;
						}
					}
					if (this.gfxOffY > 16f)
					{
						this.gfxOffY = 16f;
					}
					if (this.gfxOffY < -16f)
					{
						this.gfxOffY = -16f;
					}
					Vector2 vector = this.velocity;
					this.oldVelocity = this.velocity;
					this.whoAmI = i;
					if (this.soundDelay > 0)
					{
						this.soundDelay--;
					}
					this.netUpdate = false;
					for (int j = 0; j < 255; j++)
					{
						if (this.playerImmune[j] > 0)
						{
							this.playerImmune[j]--;
						}
					}
					if (this.updatedNPCImmunity)
					{
						for (int k = 0; k < 200; k++)
						{
							if (this.npcImmune[k] > 0)
							{
								this.npcImmune[k]--;
							}
						}
					}
					this.AI();
					if (this.owner < 255 && !Main.player[this.owner].active)
					{
						this.Kill();
					}
					if (this.type == 242 || this.type == 302 || this.type == 638)
					{
						this.wet = false;
					}
					if (!this.ignoreWater)
					{
						bool flag;
						bool flag2;
						try
						{
							flag = Collision.LavaCollision(this.position, this.width, this.height);
							flag2 = Collision.WetCollision(this.position, this.width, this.height);
							if (flag)
							{
								this.lavaWet = true;
							}
							if (Collision.honey)
							{
								this.honeyWet = true;
							}
						}
						catch
						{
							this.active = false;
							return;
						}
						if (this.wet && !this.lavaWet)
						{
							if (this.type == 85 || this.type == 15 || this.type == 34 || this.type == 188)
							{
								this.Kill();
							}
							if (this.type == 2)
							{
								this.type = 1;
								this.light = 0f;
							}
						}
						if (this.type == 80)
						{
							flag2 = false;
							this.wet = false;
							if (flag && this.ai[0] >= 0f)
							{
								this.Kill();
							}
						}
						if (flag2)
						{
							this.wet = true;
						}
						else if (this.wet)
						{
							this.wet = false;
							if (this.type == 155)
							{
								this.velocity.Y = this.velocity.Y * 0.5f;
							}
							else if (this.wetCount == 0)
							{
								this.wetCount = 10;
							}
						}
						if (!this.wet)
						{
							this.lavaWet = false;
							this.honeyWet = false;
						}
						if (this.wetCount > 0)
						{
							this.wetCount -= 1;
						}
					}
					this.oldPosition = this.position;
					this.oldDirection = this.direction;
					int num25;
					int num26;
					this.HandleMovement(velocity, out num25, out num26);
					if ((this.aiStyle != 3 || this.ai[0] != 1f) && (this.aiStyle != 7 || this.ai[0] != 1f) && (this.aiStyle != 13 || this.ai[0] != 1f) && this.aiStyle != 65 && this.aiStyle != 69 && this.aiStyle != 114 && this.aiStyle != 123 && this.aiStyle != 112 && !this.manualDirectionChange && this.aiStyle != 67 && this.aiStyle != 26 && this.aiStyle != 15)
					{
						if (this.velocity.X < 0f)
						{
							this.direction = -1;
						}
						else
						{
							this.direction = 1;
						}
					}
					if (this.active)
					{
						if (!this.npcProj && this.friendly && Main.player[this.owner].magicQuiver && this.extraUpdates < 1 && this.arrow)
						{
							this.extraUpdates = 1;
						}
						this.Damage();
						if (this.type == 434 && this.localAI[0] == 0f && this.numUpdates == 0)
						{
							this.extraUpdates = 1;
							this.velocity = Vector2.Zero;
							this.localAI[0] = 1f;
							this.localAI[1] = 0.9999f;
							this.netUpdate = true;
						}
						if (Main.netMode != 1 && (this.type == 99 || this.type == 444))
						{
							Collision.SwitchTiles(this.position, this.width, this.height, this.oldPosition, 3);
						}
						if (ProjectileID.Sets.TrailingMode[this.type] == 0)
						{
							for (int num47 = this.oldPos.Length - 1; num47 > 0; num47--)
							{
								this.oldPos[num47] = this.oldPos[num47 - 1];
							}
							this.oldPos[0] = this.position;
						}
						else if (ProjectileID.Sets.TrailingMode[this.type] == 1)
						{
							if (this.frameCounter == 0 || this.oldPos[0] == Vector2.Zero)
							{
								for (int num48 = this.oldPos.Length - 1; num48 > 0; num48--)
								{
									this.oldPos[num48] = this.oldPos[num48 - 1];
								}
								this.oldPos[0] = this.position;
							}
						}
						else if (ProjectileID.Sets.TrailingMode[this.type] == 2)
						{
							for (int num55 = this.oldPos.Length - 1; num55 > 0; num55--)
							{
								this.oldPos[num55] = this.oldPos[num55 - 1];
								this.oldRot[num55] = this.oldRot[num55 - 1];
								this.oldSpriteDirection[num55] = this.oldSpriteDirection[num55 - 1];
							}
							this.oldPos[0] = this.position;
							this.oldRot[0] = this.rotation;
							this.oldSpriteDirection[0] = this.spriteDirection;
						}
						this.timeLeft--;
						if (this.timeLeft <= 0)
						{
							this.Kill();
						}
						if (this.penetrate == 0)
						{
							this.Kill();
						}
						if (!this.active || this.owner != Main.myPlayer)
						{
							continue;
						}
						if (this.netUpdate2)
						{
							this.netUpdate = true;
						}
						if (!this.active)
						{
							this.netSpam = 0;
						}
						if (this.netUpdate)
						{
							if (this.netSpam < 60)
							{
								this.netSpam += 5;
								NetMessage.SendData(27, -1, -1, "", i, 0f, 0f, 0f, 0, 0, 0);
								this.netUpdate2 = false;
							}
							else
							{
								this.netUpdate2 = true;
							}
						}
						if (this.netSpam > 0)
						{
							this.netSpam--;
							continue;
						}
						continue;
					}
					return;
				}
			}
			this.netUpdate = false;
		}
		public void FishingCheck()
		{
			int num = (int)(base.Center.X / 16f);
			int num2 = (int)(base.Center.Y / 16f);
			if (Main.tile[num, num2].liquid < 0)
			{
				num2++;
			}
			bool flag = false;
			bool flag2 = false;
			int num3 = num;
			int num4 = num;
			while (num3 > 10 && Main.tile[num3, num2].liquid > 0)
			{
				if (WorldGen.SolidTile(num3, num2))
				{
					break;
				}
				num3--;
			}
			while (num4 < Main.maxTilesX - 10 && Main.tile[num4, num2].liquid > 0 && !WorldGen.SolidTile(num4, num2))
			{
				num4++;
			}
			int num5 = 0;
			for (int i = num3; i <= num4; i++)
			{
				int num6 = num2;
				while (Main.tile[i, num6].liquid > 0 && !WorldGen.SolidTile(i, num6) && num6 < Main.maxTilesY - 10)
				{
					num5++;
					num6++;
					if (Main.tile[i, num6].lava())
					{
						flag = true;
					}
					else if (Main.tile[i, num6].honey())
					{
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				num5 = (int)((double)num5 * 1.5);
			}
			if (num5 < 75)
			{
				Main.player[this.owner].displayedFishingInfo = "Not Enough Water!";
				return;
			}
			int num7 = Main.player[this.owner].FishingLevel();
			if (num7 == 0)
			{
				return;
			}
			Main.player[this.owner].displayedFishingInfo = num7 + " Fishing Power";
			if (num7 < 0)
			{
				if (num7 == -1)
				{
					Main.player[this.owner].displayedFishingInfo = "Warning!";
					if ((num < 380 || num > Main.maxTilesX - 380) && num5 > 1000 && !NPC.AnyNPCs(370))
					{
						this.ai[1] = (float)(Main.rand.Next(-180, -60) - 100);
						this.localAI[1] = (float)num7;
						this.netUpdate = true;
					}
				}
				return;
			}
			int num8 = 300;
			float num9 = (float)(Main.maxTilesX / 4200);
			num9 *= num9;
			float num10 = (float)((double)(this.position.Y / 16f - (60f + 10f * num9)) / (Main.worldSurface / 6.0));
			if ((double)num10 < 0.25)
			{
				num10 = 0.25f;
			}
			if (num10 > 1f)
			{
				num10 = 1f;
			}
			num8 = (int)((float)num8 * num10);
			float num11 = (float)num5 / (float)num8;
			if (num11 < 1f)
			{
				num7 = (int)((float)num7 * num11);
			}
			num11 = 1f - num11;
			if (num5 < num8)
			{
				Main.player[this.owner].displayedFishingInfo = string.Concat(new object[]
				{
					num7,
					" (",
					-Math.Round((double)(num11 * 100f)),
					"%) Fishing Power"
				});
			}
			int num12 = (num7 + 75) / 2;
			if (Main.player[this.owner].wet)
			{
				return;
			}
			if (Main.rand.Next(100) > num12)
			{
				return;
			}
			int num13 = 0;
			int num14;
			if ((double)num2 < Main.worldSurface * 0.5)
			{
				num14 = 0;
			}
			else if ((double)num2 < Main.worldSurface)
			{
				num14 = 1;
			}
			else if ((double)num2 < Main.rockLayer)
			{
				num14 = 2;
			}
			else if (num2 < Main.maxTilesY - 300)
			{
				num14 = 3;
			}
			else
			{
				num14 = 4;
			}
			int num15 = 150;
			int num16 = num15 / num7;
			int num17 = num15 * 2 / num7;
			int num18 = num15 * 7 / num7;
			int num19 = num15 * 15 / num7;
			int num20 = num15 * 30 / num7;
			if (num16 < 2)
			{
				num16 = 2;
			}
			if (num17 < 3)
			{
				num17 = 3;
			}
			if (num18 < 4)
			{
				num18 = 4;
			}
			if (num19 < 5)
			{
				num19 = 5;
			}
			if (num20 < 6)
			{
				num20 = 6;
			}
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			if (Main.rand.Next(num16) == 0)
			{
				flag3 = true;
			}
			if (Main.rand.Next(num17) == 0)
			{
				flag4 = true;
			}
			if (Main.rand.Next(num18) == 0)
			{
				flag5 = true;
			}
			if (Main.rand.Next(num19) == 0)
			{
				flag6 = true;
			}
			if (Main.rand.Next(num20) == 0)
			{
				flag7 = true;
			}
			int num21 = 10;
			if (Main.player[this.owner].cratePotion)
			{
				num21 += 10;
			}
			int num22 = Main.anglerQuestItemNetIDs[Main.anglerQuest];
			if (Main.player[this.owner].HasItem(num22))
			{
				num22 = -1;
			}
			if (Main.anglerQuestFinished)
			{
				num22 = -1;
			}
			if (flag)
			{
				if (Main.player[this.owner].inventory[Main.player[this.owner].selectedItem].type != 2422)
				{
					return;
				}
				if (flag7)
				{
					num13 = 2331;
				}
				else if (flag6)
				{
					num13 = 2312;
				}
				else if (flag5)
				{
					num13 = 2315;
				}
			}
			else if (flag2)
			{
				if (flag5 || (flag4 && Main.rand.Next(2) == 0))
				{
					num13 = 2314;
				}
				else if (flag4 && num22 == 2451)
				{
					num13 = 2451;
				}
			}
			else if (Main.rand.Next(50) > num7 && Main.rand.Next(50) > num7 && num5 < num8)
			{
				num13 = Main.rand.Next(2337, 2340);
			}
			else if (Main.rand.Next(100) < num21)
			{
				if (flag6 || flag7)
				{
					num13 = 2336;
				}
				else if (flag5 && Main.player[this.owner].ZoneCorrupt)
				{
					num13 = 3203;
				}
				else if (flag5 && Main.player[this.owner].ZoneCrimson)
				{
					num13 = 3204;
				}
				else if (flag5 && Main.player[this.owner].ZoneHoly)
				{
					num13 = 3207;
				}
				else if (flag5 && Main.player[this.owner].ZoneDungeon)
				{
					num13 = 3205;
				}
				else if (flag5 && Main.player[this.owner].ZoneJungle)
				{
					num13 = 3208;
				}
				else if (flag5 && num14 == 0)
				{
					num13 = 3206;
				}
				else if (flag4)
				{
					num13 = 2335;
				}
				else
				{
					num13 = 2334;
				}
			}
			else if (flag7 && Main.rand.Next(5) == 0)
			{
				num13 = 2423;
			}
			else if (flag7 && Main.rand.Next(5) == 0)
			{
				num13 = 3225;
			}
			else if (flag7 && Main.rand.Next(10) == 0)
			{
				num13 = 2420;
			}
			else if (!flag7 && !flag6 && flag4 && Main.rand.Next(5) == 0)
			{
				num13 = 3196;
			}
			else
			{
				bool flag8 = Main.player[this.owner].ZoneCorrupt;
				bool flag9 = Main.player[this.owner].ZoneCrimson;
				if (flag8 && flag9)
				{
					if (Main.rand.Next(2) == 0)
					{
						flag9 = false;
					}
					else
					{
						flag8 = false;
					}
				}
				if (flag8)
				{
					if (flag7 && Main.hardMode && Main.player[this.owner].ZoneSnow && num14 == 3 && Main.rand.Next(3) != 0)
					{
						num13 = 2429;
					}
					else if (flag7 && Main.hardMode && Main.rand.Next(2) == 0)
					{
						num13 = 3210;
					}
					else if (flag5)
					{
						num13 = 2330;
					}
					else if (flag4 && num22 == 2454)
					{
						num13 = 2454;
					}
					else if (flag4 && num22 == 2485)
					{
						num13 = 2485;
					}
					else if (flag4 && num22 == 2457)
					{
						num13 = 2457;
					}
					else if (flag4)
					{
						num13 = 2318;
					}
				}
				else if (flag9)
				{
					if (flag7 && Main.hardMode && Main.player[this.owner].ZoneSnow && num14 == 3 && Main.rand.Next(3) != 0)
					{
						num13 = 2429;
					}
					else if (flag7 && Main.hardMode && Main.rand.Next(2) == 0)
					{
						num13 = 3211;
					}
					else if (flag4 && num22 == 2477)
					{
						num13 = 2477;
					}
					else if (flag4 && num22 == 2463)
					{
						num13 = 2463;
					}
					else if (flag4)
					{
						num13 = 2319;
					}
					else if (flag3)
					{
						num13 = 2305;
					}
				}
				else if (Main.player[this.owner].ZoneHoly)
				{
					if (flag7 && Main.hardMode && Main.player[this.owner].ZoneSnow && num14 == 3 && Main.rand.Next(3) != 0)
					{
						num13 = 2429;
					}
					else if (flag7 && Main.hardMode && Main.rand.Next(2) == 0)
					{
						num13 = 3209;
					}
					else if (num14 > 1 && flag6)
					{
						num13 = 2317;
					}
					else if (num14 > 1 && flag5 && num22 == 2465)
					{
						num13 = 2465;
					}
					else if (num14 < 2 && flag5 && num22 == 2468)
					{
						num13 = 2468;
					}
					else if (flag5)
					{
						num13 = 2310;
					}
					else if (flag4 && num22 == 2471)
					{
						num13 = 2471;
					}
					else if (flag4)
					{
						num13 = 2307;
					}
				}
				if (num13 == 0 && Main.player[this.owner].ZoneSnow)
				{
					if (num14 < 2 && flag4 && num22 == 2467)
					{
						num13 = 2467;
					}
					else if (num14 == 1 && flag4 && num22 == 2470)
					{
						num13 = 2470;
					}
					else if (num14 >= 2 && flag4 && num22 == 2484)
					{
						num13 = 2484;
					}
					else if (num14 > 1 && flag4 && num22 == 2466)
					{
						num13 = 2466;
					}
					else if ((flag3 && Main.rand.Next(12) == 0) || (flag4 && Main.rand.Next(6) == 0))
					{
						num13 = 3197;
					}
					else if (flag4)
					{
						num13 = 2306;
					}
					else if (flag3)
					{
						num13 = 2299;
					}
					else if (num14 > 1 && Main.rand.Next(3) == 0)
					{
						num13 = 2309;
					}
				}
				if (num13 == 0 && Main.player[this.owner].ZoneJungle)
				{
					if (Main.hardMode && flag7 && Main.rand.Next(2) == 0)
					{
						num13 = 3018;
					}
					else if (num14 == 1 && flag4 && num22 == 2452)
					{
						num13 = 2452;
					}
					else if (num14 == 1 && flag4 && num22 == 2483)
					{
						num13 = 2483;
					}
					else if (num14 == 1 && flag4 && num22 == 2488)
					{
						num13 = 2488;
					}
					else if (num14 >= 1 && flag4 && num22 == 2486)
					{
						num13 = 2486;
					}
					else if (num14 > 1 && flag4)
					{
						num13 = 2311;
					}
					else if (flag4)
					{
						num13 = 2313;
					}
					else if (flag3)
					{
						num13 = 2302;
					}
				}
				if (num13 == 0 && Main.shroomTiles > 200 && flag4 && num22 == 2475)
				{
					num13 = 2475;
				}
				if (num13 == 0)
				{
					if (num14 <= 1 && (num < 380 || num > Main.maxTilesX - 380) && num5 > 1000)
					{
						if (flag6 && Main.rand.Next(2) == 0)
						{
							num13 = 2341;
						}
						else if (flag6)
						{
							num13 = 2342;
						}
						else if (flag5 && Main.rand.Next(5) == 0)
						{
							num13 = 2438;
						}
						else if (flag5 && Main.rand.Next(2) == 0)
						{
							num13 = 2332;
						}
						else if (flag4 && num22 == 2480)
						{
							num13 = 2480;
						}
						else if (flag4 && num22 == 2481)
						{
							num13 = 2481;
						}
						else if (flag4)
						{
							num13 = 2316;
						}
						else if (flag3 && Main.rand.Next(2) == 0)
						{
							num13 = 2301;
						}
						else if (flag3)
						{
							num13 = 2300;
						}
						else
						{
							num13 = 2297;
						}
					}
					else
					{
						int arg_CB8_0 = Main.sandTiles;
					}
				}
				if (num13 == 0)
				{
					if (num14 < 2 && flag4 && num22 == 2461)
					{
						num13 = 2461;
					}
					else if (num14 == 0 && flag4 && num22 == 2453)
					{
						num13 = 2453;
					}
					else if (num14 == 0 && flag4 && num22 == 2473)
					{
						num13 = 2473;
					}
					else if (num14 == 0 && flag4 && num22 == 2476)
					{
						num13 = 2476;
					}
					else if (num14 < 2 && flag4 && num22 == 2458)
					{
						num13 = 2458;
					}
					else if (num14 < 2 && flag4 && num22 == 2459)
					{
						num13 = 2459;
					}
					else if (num14 == 0 && flag4)
					{
						num13 = 2304;
					}
					else if (num14 > 0 && num14 < 3 && flag4 && num22 == 2455)
					{
						num13 = 2455;
					}
					else if (num14 == 1 && flag4 && num22 == 2479)
					{
						num13 = 2479;
					}
					else if (num14 == 1 && flag4 && num22 == 2456)
					{
						num13 = 2456;
					}
					else if (num14 == 1 && flag4 && num22 == 2474)
					{
						num13 = 2474;
					}
					else if (num14 > 1 && flag5 && Main.rand.Next(5) == 0)
					{
						if (Main.hardMode && Main.rand.Next(2) == 0)
						{
							num13 = 2437;
						}
						else
						{
							num13 = 2436;
						}
					}
					else if (num14 > 1 && flag7)
					{
						num13 = 2308;
					}
					else if (num14 > 1 && flag6 && Main.rand.Next(2) == 0)
					{
						num13 = 2320;
					}
					else if (num14 > 1 && flag5)
					{
						num13 = 2321;
					}
					else if (num14 > 1 && flag4 && num22 == 2478)
					{
						num13 = 2478;
					}
					else if (num14 > 1 && flag4 && num22 == 2450)
					{
						num13 = 2450;
					}
					else if (num14 > 1 && flag4 && num22 == 2464)
					{
						num13 = 2464;
					}
					else if (num14 > 1 && flag4 && num22 == 2469)
					{
						num13 = 2469;
					}
					else if (num14 > 2 && flag4 && num22 == 2462)
					{
						num13 = 2462;
					}
					else if (num14 > 2 && flag4 && num22 == 2482)
					{
						num13 = 2482;
					}
					else if (num14 > 2 && flag4 && num22 == 2472)
					{
						num13 = 2472;
					}
					else if (num14 > 2 && flag4 && num22 == 2460)
					{
						num13 = 2460;
					}
					else if (num14 > 1 && flag4 && Main.rand.Next(4) != 0)
					{
						num13 = 2303;
					}
					else if (num14 > 1 && (flag4 || flag3 || Main.rand.Next(4) == 0))
					{
						if (Main.rand.Next(4) == 0)
						{
							num13 = 2303;
						}
						else
						{
							num13 = 2309;
						}
					}
					else if (flag4 && num22 == 2487)
					{
						num13 = 2487;
					}
					else if (num5 > 1000 && flag3)
					{
						num13 = 2298;
					}
					else
					{
						num13 = 2290;
					}
				}
			}
			if (num13 > 0)
			{
				if (Main.player[this.owner].sonarPotion)
				{
					Item item = new Item();
					item.SetDefaults(num13, false);
					item.position = this.position;
					ItemText.NewText(item, 1, true, false);
				}
				float num23 = (float)num7;
				this.ai[1] = (float)Main.rand.Next(-240, -90) - num23;
				this.localAI[1] = (float)num13;
				this.netUpdate = true;
			}
		}

		private void HandleMovement(Vector2 wetVelocity, out int overrideWidth, out int overrideHeight)
		{
			bool flag = false;
			overrideWidth = -1;
			overrideHeight = -1;
			bool flag2 = false;
			bool? flag3 = ProjectileID.Sets.ForcePlateDetection[this.type];
			bool flag4 = flag3.HasValue && !flag3.Value;
			bool flag5 = flag3.HasValue && flag3.Value;
			if (this.tileCollide)
			{
				Vector2 velocity = this.velocity;
				bool flag6 = true;
				if (Main.projPet[this.type])
				{
					flag6 = false;
					if (Main.player[this.owner].position.Y + (float)Main.player[this.owner].height - 12f > this.position.Y + (float)this.height)
					{
						flag6 = true;
					}
				}
				if (this.type == 500)
				{
					flag6 = false;
					if (Main.player[this.owner].Bottom.Y > base.Bottom.Y + 4f)
					{
						flag6 = true;
					}
				}
				if (this.type == 653)
				{
					flag6 = false;
					if (Main.player[this.owner].Bottom.Y > base.Bottom.Y + 4f)
					{
						flag6 = true;
					}
				}
				if (this.aiStyle == 62)
				{
					flag6 = true;
				}
				if (this.aiStyle == 66)
				{
					flag6 = true;
				}
				if (this.type == 317)
				{
					flag6 = true;
				}
				if (this.type == 373)
				{
					flag6 = true;
				}
				if (this.aiStyle == 53)
				{
					flag6 = false;
				}
				if (this.type == 9 || this.type == 12 || this.type == 15 || this.type == 13 || this.type == 31 || this.type == 39 || this.type == 40)
				{
					flag6 = false;
				}
				if (this.type == 24)
				{
					flag6 = false;
				}
				if (this.aiStyle == 29 || this.type == 28 || this.aiStyle == 49)
				{
					overrideWidth = this.width - 8;
					overrideHeight = this.height - 8;
				}
				else if (this.type == 250 || this.type == 267 || this.type == 297 || this.type == 323 || this.type == 3)
				{
					overrideWidth = 6;
					overrideHeight = 6;
				}
				else if (this.type == 308)
				{
					overrideWidth = 26;
					overrideHeight = this.height;
				}
				else if (this.type == 261 || this.type == 277)
				{
					overrideWidth = 26;
					overrideHeight = 26;
				}
				else if (this.type == 481 || this.type == 491 || this.type == 106 || this.type == 262 || this.type == 271 || this.type == 270 || this.type == 272 || this.type == 273 || this.type == 274 || this.type == 280 || this.type == 288 || this.type == 301 || this.type == 320 || this.type == 333 || this.type == 335 || this.type == 343 || this.type == 344 || this.type == 497 || this.type == 496 || this.type == 6 || this.type == 19 || this.type == 113 || this.type == 52 || this.type == 520 || this.type == 523 || this.type == 585 || this.type == 598 || this.type == 599 || this.type == 636)
				{
					overrideWidth = 10;
					overrideHeight = 10;
				}
				else if (this.type == 514)
				{
					overrideWidth = 4;
					overrideHeight = 4;
				}
				else if (this.type == 248 || this.type == 247 || this.type == 507 || this.type == 508)
				{
					overrideWidth = this.width - 12;
					overrideHeight = this.height - 12;
				}
				else if (this.aiStyle == 18 || this.type == 254)
				{
					overrideWidth = this.width - 36;
					overrideHeight = this.height - 36;
				}
				else if (this.type == 182 || this.type == 190 || this.type == 33 || this.type == 229 || this.type == 237 || this.type == 243)
				{
					overrideWidth = this.width - 20;
					overrideHeight = this.height - 20;
				}
				else if (this.aiStyle == 27)
				{
					overrideWidth = this.width - 12;
					overrideHeight = this.height - 12;
				}
				else if (this.type == 533 && this.ai[0] >= 6f)
				{
					overrideWidth = this.width + 6;
					overrideHeight = this.height + 6;
				}
				else if (this.type == 582 || this.type == 634 || this.type == 635)
				{
					overrideWidth = 8;
					overrideHeight = 8;
				}
				else if (this.type == 617)
				{
					overrideWidth = (int)(20f * this.scale);
					overrideHeight = (int)(20f * this.scale);
				}
				if (((this.type != 440 && this.type != 449 && this.type != 606) || this.ai[1] != 1f) && (this.type != 466 || this.localAI[1] != 1f) && (this.type != 580 || this.localAI[1] <= 0f) && (this.type != 640 || this.localAI[1] <= 0f))
				{
					if (this.aiStyle == 10)
					{
						if (this.type == 42 || this.type == 65 || this.type == 68 || this.type == 354 || (this.type == 31 && this.ai[0] == 2f))
						{
							this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height, flag6, flag6, 1);
						}
						else
						{
							this.velocity = Collision.AnyCollision(this.position, this.velocity, this.width, this.height, true);
						}
					}
					else
					{
						Vector2 vector = this.position;
						int num = (overrideWidth != -1) ? overrideWidth : this.width;
						int num2 = (overrideHeight != -1) ? overrideHeight : this.height;
						if (overrideHeight != -1 || overrideWidth != -1)
						{
							vector = new Vector2(this.position.X + (float)(this.width / 2) - (float)(num / 2), this.position.Y + (float)(this.height / 2) - (float)(num2 / 2));
						}
						if (this.wet)
						{
							if (this.honeyWet)
							{
								Vector2 velocity2 = this.velocity;
								this.velocity = Collision.TileCollision(vector, this.velocity, num, num2, flag6, flag6, 1);
								wetVelocity = this.velocity * 0.25f;
								if (this.velocity.X != velocity2.X)
								{
									wetVelocity.X = this.velocity.X;
								}
								if (this.velocity.Y != velocity2.Y)
								{
									wetVelocity.Y = this.velocity.Y;
								}
							}
							else
							{
								Vector2 velocity3 = this.velocity;
								this.velocity = Collision.TileCollision(vector, this.velocity, num, num2, flag6, flag6, 1);
								wetVelocity = this.velocity * 0.5f;
								if (this.velocity.X != velocity3.X)
								{
									wetVelocity.X = this.velocity.X;
								}
								if (this.velocity.Y != velocity3.Y)
								{
									wetVelocity.Y = this.velocity.Y;
								}
							}
						}
						else
						{
							int num3 = Math.Min(num, num2);
							if (num3 < 3)
							{
								num3 = 3;
							}
							if (num3 > 16)
							{
								num3 = 16;
							}
							if (this.velocity.Length() > (float)num3)
							{
								Vector2 vector2 = Collision.TileCollision(vector, this.velocity, num, num2, flag6, flag6, 1);
								float num4 = this.velocity.Length();
								float num5 = (float)num3;
								Vector2 vector3 = Vector2.Normalize(this.velocity);
								if (vector2.Y == 0f)
								{
									vector3.Y = 0f;
								}
								Vector2 vector4 = Vector2.Zero;
								Vector2 arg_954_0 = Vector2.Zero;
								Vector2 vector5 = Vector2.Zero;
								int num6 = 0;
								while (num4 > 0f)
								{
									num6++;
									if (num6 > 300)
									{
										break;
									}
									Vector2 vector6 = vector;
									float num7 = num4;
									if (num7 > num5)
									{
										num7 = num5;
									}
									num4 -= num7;
									Vector2 velocity4 = vector3 * num7;
									Vector2 vector7 = Collision.TileCollision(vector, velocity4, num, num2, flag6, flag6, 1);
									vector += vector7;
									this.velocity = vector7;
									if (!Main.projPet[this.type])
									{
										Vector4 vector8 = Collision.SlopeCollision(vector, this.velocity, num, num2, 0f, true);
										Vector2 vector9 = this.position - vector;
										if (vector.X != vector8.X)
										{
											flag = true;
										}
										if (vector.Y != vector8.Y)
										{
											flag = true;
										}
										if (this.velocity.X != vector8.Z)
										{
											flag = true;
										}
										if (this.velocity.Y != vector8.W)
										{
											flag = true;
										}
										vector.X = vector8.X;
										vector.Y = vector8.Y;
										vector5 += vector + vector9 - this.position;
										this.velocity.X = vector8.Z;
										this.velocity.Y = vector8.W;
									}
									flag2 = true;
									if (this.owner == Main.myPlayer && vector != vector6 && !flag4)
									{
										Collision.SwitchTiles(vector, num, num2, vector6, 4);
									}
									vector7 = this.velocity;
									vector4 += vector7;
								}
								this.velocity = vector4;
								if (Math.Abs(this.velocity.X - velocity.X) < 0.0001f)
								{
									this.velocity.X = velocity.X;
								}
								if (Math.Abs(this.velocity.Y - velocity.Y) < 0.0001f)
								{
									this.velocity.Y = velocity.Y;
								}
								if (!Main.projPet[this.type])
								{
									Vector4 vector10 = Collision.SlopeCollision(vector, this.velocity, num, num2, 0f, true);
									Vector2 vector11 = this.position - vector;
									if (vector.X != vector10.X)
									{
										flag = true;
									}
									if (vector.Y != vector10.Y)
									{
										flag = true;
									}
									if (this.velocity.X != vector10.Z)
									{
										flag = true;
									}
									if (this.velocity.Y != vector10.W)
									{
										flag = true;
									}
									vector.X = vector10.X;
									vector.Y = vector10.Y;
									this.position = vector + vector11;
									this.velocity.X = vector10.Z;
									this.velocity.Y = vector10.W;
								}
							}
							else
							{
								this.velocity = Collision.TileCollision(vector, this.velocity, num, num2, flag6, flag6, 1);
								if (!Main.projPet[this.type])
								{
									Vector4 vector12 = Collision.SlopeCollision(vector, this.velocity, num, num2, 0f, true);
									Vector2 vector13 = this.position - vector;
									if (vector.X != vector12.X)
									{
										flag = true;
									}
									if (vector.Y != vector12.Y)
									{
										flag = true;
									}
									if (this.velocity.X != vector12.Z)
									{
										flag = true;
									}
									if (this.velocity.Y != vector12.W)
									{
										flag = true;
									}
									vector.X = vector12.X;
									vector.Y = vector12.Y;
									this.position = vector + vector13;
									this.velocity.X = vector12.Z;
									this.velocity.Y = vector12.W;
								}
							}
						}
					}
				}
				if (velocity != this.velocity)
				{
					flag = true;
				}
				if (flag)
				{
					if (this.type == 434)
					{
						this.position += this.velocity;
						this.numUpdates = 0;
					}
					else if (this.type == 601)
					{
						if (this.owner == Main.myPlayer)
						{
							PortalHelper.TryPlacingPortal(this, velocity, this.velocity);
						}
						this.position += this.velocity;
						this.Kill();
					}
					else if (this.type == 451)
					{
						this.ai[0] = 1f;
						this.ai[1] = 0f;
						this.netUpdate = true;
						this.velocity = velocity / 2f;
					}
					else if (this.type == 645)
					{
						this.ai[0] = 0f;
						this.ai[1] = -1f;
						this.netUpdate = true;
					}
					else if (this.type == 584)
					{
						bool flag7 = false;
						if (this.velocity.X != velocity.X)
						{
							this.velocity.X = velocity.X * -0.75f;
							flag7 = true;
						}
						if ((this.velocity.Y != velocity.Y && velocity.Y > 2f) || this.velocity.Y == 0f)
						{
							this.velocity.Y = velocity.Y * -0.75f;
							flag7 = true;
						}
						if (flag7)
						{
							float num8 = velocity.Length() / this.velocity.Length();
							if (num8 == 0f)
							{
								num8 = 1f;
							}
							this.velocity /= num8;
							this.penetrate--;
						}
					}
					else if (this.type == 532)
					{
						bool flag8 = false;
						if (this.velocity.X != velocity.X)
						{
							this.velocity.X = velocity.X * -0.75f;
							flag8 = true;
						}
						if ((this.velocity.Y != velocity.Y && velocity.Y > 2f) || this.velocity.Y == 0f)
						{
							this.velocity.Y = velocity.Y * -0.75f;
							flag8 = true;
						}
						if (flag8)
						{
							float num9 = velocity.Length() / this.velocity.Length();
							if (num9 == 0f)
							{
								num9 = 1f;
							}
							this.velocity /= num9;
							this.penetrate--;
							Collision.HitTiles(this.position, velocity, this.width, this.height);
						}
					}
					else if (this.type == 533)
					{
						float num10 = 1f;
						bool flag9 = false;
						if (this.velocity.X != velocity.X)
						{
							this.velocity.X = velocity.X * -num10;
							flag9 = true;
						}
						if (this.velocity.Y != velocity.Y || this.velocity.Y == 0f)
						{
							this.velocity.Y = velocity.Y * -num10 * 0.5f;
							flag9 = true;
						}
						if (flag9)
						{
							float num11 = velocity.Length() / this.velocity.Length();
							if (num11 == 0f)
							{
								num11 = 1f;
							}
							this.velocity /= num11;
							if (this.ai[0] == 7f && (double)this.velocity.Y < -0.1)
							{
								this.velocity.Y = this.velocity.Y + 0.1f;
							}
							if (this.ai[0] >= 6f && this.ai[0] < 9f)
							{
								Collision.HitTiles(this.position, velocity, this.width, this.height);
							}
						}
					}
					else if (this.type == 502)
					{
						this.ai[0] += 1f;
						if (this.ai[0] >= 5f)
						{
							this.position += this.velocity;
							this.Kill();
						}
						else
						{
							if (this.velocity.Y != velocity.Y)
							{
								this.velocity.Y = -velocity.Y;
							}
							if (this.velocity.X != velocity.X)
							{
								this.velocity.X = -velocity.X;
							}
						}
						Vector2 spinningpoint = new Vector2(0f, -3f - this.ai[0]).RotatedByRandom(3.1415927410125732);
						float num12 = 10f + this.ai[0] * 4f;
						Vector2 vector14 = new Vector2(1.05f, 1f);
						if (Main.myPlayer == this.owner)
						{
							int width = this.width;
							int height = this.height;
							int num15 = this.penetrate;
							this.position = base.Center;
							this.width = (this.height = 40 + 8 * (int)this.ai[0]);
							base.Center = this.position;
							this.penetrate = -1;
							this.Damage();
							this.penetrate = num15;
							this.position = base.Center;
							this.width = width;
							this.height = height;
							base.Center = this.position;
						}
					}
					else if (this.type == 653)
					{
						if (this.velocity.Y != velocity.Y && this.velocity.Y == 0f && velocity.Y > 1f && velocity.Y < 4f)
						{
							this.velocity.Y = -velocity.Y * 2f;
						}
					}
					else if (this.type == 444)
					{
						if (this.velocity.X != velocity.X)
						{
							this.velocity.X = -velocity.X;
						}
						if (this.velocity.Y != velocity.Y)
						{
							this.velocity.Y = -velocity.Y;
						}
						this.ai[0] = this.velocity.ToRotation();
					}
					else if (this.type == 617)
					{
						if (this.velocity.X != velocity.X)
						{
							this.velocity.X = -velocity.X * 0.35f;
						}
						if (this.velocity.Y != velocity.Y)
						{
							this.velocity.Y = -velocity.Y * 0.35f;
						}
					}
					else if (this.type == 440 || this.type == 449 || this.type == 606)
					{
						if (this.ai[1] != 1f)
						{
							this.ai[1] = 1f;
							this.position += this.velocity;
							this.velocity = velocity;
						}
					}
					else if (this.type == 466 || this.type == 580 || this.type == 640)
					{
						if (this.localAI[1] < 1f)
						{
							this.localAI[1] += 2f;
							this.position += this.velocity;
							this.velocity = Vector2.Zero;
						}
					}
					else if (this.aiStyle == 54)
					{
						if (this.velocity.X != velocity.X)
						{
							this.velocity.X = velocity.X * -0.6f;
						}
						if (this.velocity.Y != velocity.Y)
						{
							this.velocity.Y = velocity.Y * -0.6f;
						}
					}
					else if (!Main.projPet[this.type] && this.type != 500 && this.type != 650)
					{
						if (this.aiStyle == 99)
						{
							if (this.type >= 556 && this.type <= 561)
							{
								bool flag10 = false;
								if (this.velocity.X != velocity.X)
								{
									flag10 = true;
									this.velocity.X = velocity.X * -1f;
								}
								if (this.velocity.Y != velocity.Y)
								{
									flag10 = true;
									this.velocity.Y = velocity.Y * -1f;
								}
								if (flag10)
								{
									Vector2 vector15 = Main.player[this.owner].Center - base.Center;
									vector15.Normalize();
									vector15 *= this.velocity.Length();
									vector15 *= 0.25f;
									this.velocity *= 0.75f;
									this.velocity += vector15;
									if (this.velocity.Length() > 6f)
									{
										this.velocity *= 0.5f;
									}
								}
							}
						}
						else if (this.type == 604)
						{
							if (this.velocity.X != velocity.X)
							{
								this.velocity.X = -velocity.X;
							}
							if (this.velocity.Y != velocity.Y)
							{
								this.velocity.Y = -velocity.Y;
							}
						}
						else if (this.type == 379)
						{
							if (this.velocity.X != velocity.X)
							{
								this.velocity.X = velocity.X * -0.6f;
							}
							if (this.velocity.Y != velocity.Y && velocity.Y > 2f)
							{
								this.velocity.Y = velocity.Y * -0.6f;
							}
						}
						else if (this.type == 491)
						{
							if (this.ai[0] <= 0f)
							{
								this.ai[0] = -10f;
							}
							if (this.velocity.X != velocity.X && Math.Abs(velocity.X) > 0f)
							{
								this.velocity.X = velocity.X * -1f;
							}
							if (this.velocity.Y != velocity.Y && Math.Abs(velocity.Y) > 0f)
							{
								this.velocity.Y = velocity.Y * -1f;
							}
						}
						else if ((this.type >= 515 && this.type <= 517) || this.type == 637)
						{
							if (this.velocity.X != velocity.X && Math.Abs(velocity.X) > 1f)
							{
								this.velocity.X = velocity.X * -0.9f;
							}
							if (this.velocity.Y != velocity.Y && Math.Abs(velocity.Y) > 1f)
							{
								this.velocity.Y = velocity.Y * -0.9f;
							}
						}
						else if (this.type == 409)
						{
							if (this.velocity.X != velocity.X)
							{
								this.velocity.X = velocity.X * -1f;
							}
							if (this.velocity.Y != velocity.Y)
							{
								this.velocity.Y = velocity.Y * -1f;
							}
						}
						else if (this.type == 254)
						{
							this.tileCollide = false;
							this.velocity = velocity;
							if (this.timeLeft > 30)
							{
								this.timeLeft = 30;
							}
						}
						else if (this.type == 225 && this.penetrate > 0)
						{
							this.velocity.X = -velocity.X;
							this.velocity.Y = -velocity.Y;
							this.penetrate--;
						}
						else if (this.type == 155)
						{
							if (this.ai[1] > 10f)
							{
								string text = string.Concat(new object[]
								{
									this.name,
									" was hit ",
									this.ai[1],
									" times before touching the ground!"
								});
								if (Main.netMode == 0)
								{
									Main.NewText(text, 255, 240, 20, false);
								}
								else if (Main.netMode == 2)
								{
									NetMessage.SendData(25, -1, -1, text, 255, 255f, 240f, 20f, 0, 0, 0);
								}
							}
							this.ai[1] = 0f;
							if (this.velocity.X != velocity.X)
							{
								this.velocity.X = velocity.X * -0.6f;
							}
							if (this.velocity.Y != velocity.Y && velocity.Y > 2f)
							{
								this.velocity.Y = velocity.Y * -0.6f;
							}
						}
						else if (this.aiStyle == 33)
						{
							if (this.localAI[0] == 0f)
							{
								if (this.wet)
								{
									this.position += velocity / 2f;
								}
								else
								{
									this.position += velocity;
								}
								this.velocity *= 0f;
								this.localAI[0] = 1f;
							}
						}
						else if (this.type != 308)
						{
							if (this.type == 477)
							{
								if (this.velocity.Y != velocity.Y || this.velocity.X != velocity.X)
								{
									this.penetrate--;
									if (this.penetrate <= 0)
									{
										this.Kill();
									}
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = -velocity.X;
									}
									if (this.velocity.Y != velocity.Y)
									{
										this.velocity.Y = -velocity.Y;
									}
								}
								if (this.penetrate > 0 && this.owner == Main.myPlayer)
								{
									int[] array = new int[10];
									int num16 = 0;
									int num17 = 700;
									int num18 = 20;
									for (int i = 0; i < 200; i++)
									{
										if (Main.npc[i].CanBeChasedBy(this, false))
										{
											float num19 = (base.Center - Main.npc[i].Center).Length();
											if (num19 > (float)num18 && num19 < (float)num17 && Collision.CanHitLine(base.Center, 1, 1, Main.npc[i].Center, 1, 1))
											{
												array[num16] = i;
												num16++;
												if (num16 >= 9)
												{
													break;
												}
											}
										}
									}
									if (num16 > 0)
									{
										num16 = Main.rand.Next(num16);
										Vector2 vector16 = Main.npc[array[num16]].Center - base.Center;
										float num20 = this.velocity.Length();
										vector16.Normalize();
										this.velocity = vector16 * num20;
										this.netUpdate = true;
									}
								}
							}
							else if (this.type == 94)
							{
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = -velocity.X;
								}
								if (this.velocity.Y != velocity.Y)
								{
									this.velocity.Y = -velocity.Y;
								}
							}
							else if (this.type == 496)
							{
								if (this.velocity.X != velocity.X)
								{
									if (Math.Abs(this.velocity.X) < 1f)
									{
										this.velocity.X = -velocity.X;
									}
									else
									{
										this.Kill();
									}
								}
								if (this.velocity.Y != velocity.Y)
								{
									if (Math.Abs(this.velocity.Y) < 1f)
									{
										this.velocity.Y = -velocity.Y;
									}
									else
									{
										this.Kill();
									}
								}
							}
							else if (this.type == 311)
							{
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = -velocity.X;
									this.ai[1] += 1f;
								}
								if (this.velocity.Y != velocity.Y)
								{
									this.velocity.Y = -velocity.Y;
									this.ai[1] += 1f;
								}
								if (this.ai[1] > 4f)
								{
									this.Kill();
								}
							}
							else if (this.type == 312)
							{
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = -velocity.X;
									this.ai[1] += 1f;
								}
								if (this.velocity.Y != velocity.Y)
								{
									this.velocity.Y = -velocity.Y;
									this.ai[1] += 1f;
								}
							}
							else if (this.type == 522 || this.type == 620)
							{
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = -velocity.X;
								}
								if (this.velocity.Y != velocity.Y)
								{
									this.velocity.Y = -velocity.Y;
								}
							}
							else if (this.type == 524)
							{
								this.ai[0] += 100f;
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = -velocity.X;
								}
								if (this.velocity.Y != velocity.Y)
								{
									this.velocity.Y = -velocity.Y;
								}
							}
							else if (this.aiStyle == 93)
							{
								if (this.velocity != velocity)
								{
									this.ai[1] = 0f;
									this.ai[0] = 1f;
									this.netUpdate = true;
									this.tileCollide = false;
									this.position += this.velocity;
									this.velocity = velocity;
									this.velocity.Normalize();
									this.velocity *= 3f;
								}
							}
							else if (this.type == 281)
							{
								float num21 = Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y);
								if (num21 < 2f || this.ai[1] == 2f)
								{
									this.ai[1] = 2f;
								}
								else
								{
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = -velocity.X * 0.5f;
									}
									if (this.velocity.Y != velocity.Y)
									{
										this.velocity.Y = -velocity.Y * 0.5f;
									}
								}
							}
							else if (this.type == 290 || this.type == 294)
							{
								if (this.velocity.X != velocity.X)
								{
									this.position.X = this.position.X + this.velocity.X;
									this.velocity.X = -velocity.X;
								}
								if (this.velocity.Y != velocity.Y)
								{
									this.position.Y = this.position.Y + this.velocity.Y;
									this.velocity.Y = -velocity.Y;
								}
							}
							else if ((this.type == 181 || this.type == 189 || this.type == 357 || this.type == 566) && this.penetrate > 0)
							{
								if (this.type == 357)
								{
									this.damage = (int)((double)this.damage * 0.9);
								}
								this.penetrate--;
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = -velocity.X;
								}
								if (this.velocity.Y != velocity.Y)
								{
									this.velocity.Y = -velocity.Y;
								}
							}
							else if (this.type == 307 && this.ai[1] < 5f)
							{
								this.ai[1] += 1f;
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = -velocity.X;
								}
								if (this.velocity.Y != velocity.Y)
								{
									this.velocity.Y = -velocity.Y;
								}
							}
							else if (this.type == 99)
							{
								if (this.velocity.Y != velocity.Y && velocity.Y > 5f)
								{
									Collision.HitTiles(this.position, this.velocity, this.width, this.height);
									this.velocity.Y = -velocity.Y * 0.2f;
								}
								if (this.velocity.X != velocity.X)
								{
									this.Kill();
								}
							}
							else if (this.type == 444)
							{
								if (this.velocity.Y != velocity.Y && velocity.Y > 5f)
								{
									Collision.HitTiles(this.position, this.velocity, this.width, this.height);
									this.velocity.Y = -velocity.Y * 0.2f;
								}
								if (this.velocity.X != velocity.X)
								{
									this.Kill();
								}
							}
							else if (this.type == 36)
							{
								if (this.penetrate > 1)
								{
									Collision.HitTiles(this.position, this.velocity, this.width, this.height);
									this.penetrate--;
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = -velocity.X;
									}
									if (this.velocity.Y != velocity.Y)
									{
										this.velocity.Y = -velocity.Y;
									}
								}
								else
								{
									this.Kill();
								}
							}
							else if (this.aiStyle == 21)
							{
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = -velocity.X;
								}
								if (this.velocity.Y != velocity.Y)
								{
									this.velocity.Y = -velocity.Y;
								}
							}
							else if (this.aiStyle == 17)
							{
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = velocity.X * -0.75f;
								}
								if (this.velocity.Y != velocity.Y && (double)velocity.Y > 1.5)
								{
									this.velocity.Y = velocity.Y * -0.7f;
								}
							}
							else if (this.aiStyle == 15)
							{
								bool flag11 = false;
								if (velocity.X != this.velocity.X)
								{
									if (Math.Abs(velocity.X) > 4f)
									{
										flag11 = true;
									}
									this.position.X = this.position.X + this.velocity.X;
									this.velocity.X = -velocity.X * 0.2f;
								}
								if (velocity.Y != this.velocity.Y)
								{
									if (Math.Abs(velocity.Y) > 4f)
									{
										flag11 = true;
									}
									this.position.Y = this.position.Y + this.velocity.Y;
									this.velocity.Y = -velocity.Y * 0.2f;
								}
								this.ai[0] = 1f;
								if (flag11)
								{
									this.netUpdate = true;
									Collision.HitTiles(this.position, this.velocity, this.width, this.height);
								}
								if (this.wet)
								{
									wetVelocity = this.velocity;
								}
							}
							else if (this.aiStyle == 39)
							{
								Collision.HitTiles(this.position, this.velocity, this.width, this.height);
								if (this.type == 33 || this.type == 106)
								{
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = -velocity.X;
									}
									if (this.velocity.Y != velocity.Y)
									{
										this.velocity.Y = -velocity.Y;
									}
								}
								else
								{
									this.ai[0] = 1f;
									if (this.aiStyle == 3)
									{
										this.velocity.X = -velocity.X;
										this.velocity.Y = -velocity.Y;
									}
								}
								this.netUpdate = true;
							}
							else if (this.aiStyle == 3 || this.aiStyle == 13 || this.aiStyle == 69 || this.aiStyle == 109)
							{
								Collision.HitTiles(this.position, this.velocity, this.width, this.height);
								if (this.type == 33 || this.type == 106)
								{
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = -velocity.X;
									}
									if (this.velocity.Y != velocity.Y)
									{
										this.velocity.Y = -velocity.Y;
									}
								}
								else
								{
									this.ai[0] = 1f;
									if ((this.aiStyle == 3 || this.aiStyle == 109) && this.type != 383)
									{
										this.velocity.X = -velocity.X;
										this.velocity.Y = -velocity.Y;
									}
								}
								this.netUpdate = true;
							}
							else if (this.aiStyle == 8 && this.type != 96)
							{
								this.ai[0] += 1f;
								if ((this.ai[0] >= 5f && this.type != 253) || (this.type == 253 && this.ai[0] >= 8f))
								{
									this.position += this.velocity;
									this.Kill();
								}
								else
								{
									if (this.type == 15 && this.velocity.Y > 4f)
									{
										if (this.velocity.Y != velocity.Y)
										{
											this.velocity.Y = -velocity.Y * 0.8f;
										}
									}
									else if (this.velocity.Y != velocity.Y)
									{
										this.velocity.Y = -velocity.Y;
									}
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = -velocity.X;
									}
								}
							}
							else if (this.aiStyle == 61)
							{
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = velocity.X * -0.3f;
								}
								if (this.velocity.Y != velocity.Y && velocity.Y > 1f)
								{
									this.velocity.Y = velocity.Y * -0.3f;
								}
							}
							else if (this.aiStyle == 14)
							{
								if (this.type == 261 && ((this.velocity.X != velocity.X && (velocity.X < -3f || velocity.X > 3f)) || (this.velocity.Y != velocity.Y && (velocity.Y < -3f || velocity.Y > 3f))))
								{
									Collision.HitTiles(this.position, this.velocity, this.width, this.height);
								}
								if (this.type >= 326 && this.type <= 328 && this.velocity.X != velocity.X)
								{
									this.velocity.X = velocity.X * -0.1f;
								}
								if (this.type >= 400 && this.type <= 402)
								{
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = velocity.X * -0.1f;
									}
								}
								else if (this.type == 50)
								{
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = velocity.X * -0.2f;
									}
									if (this.velocity.Y != velocity.Y && (double)velocity.Y > 1.5)
									{
										this.velocity.Y = velocity.Y * -0.2f;
									}
								}
								else if (this.type == 185)
								{
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = velocity.X * -0.9f;
									}
									if (this.velocity.Y != velocity.Y && velocity.Y > 1f)
									{
										this.velocity.Y = velocity.Y * -0.9f;
									}
								}
								else if (this.type == 277)
								{
									if (this.velocity.X != velocity.X)
									{
										this.velocity.X = velocity.X * -0.9f;
									}
									if (this.velocity.Y != velocity.Y && velocity.Y > 3f)
									{
										this.velocity.Y = velocity.Y * -0.9f;
									}
								}
								else if (this.type != 480)
								{
									if (this.type == 450)
									{
										if (this.velocity.X != velocity.X)
										{
											this.velocity.X = velocity.X * -0.1f;
										}
									}
									else
									{
										if (this.velocity.X != velocity.X)
										{
											this.velocity.X = velocity.X * -0.5f;
										}
										if (this.velocity.Y != velocity.Y && velocity.Y > 1f)
										{
											this.velocity.Y = velocity.Y * -0.5f;
										}
									}
								}
							}
							else if (this.aiStyle == 16)
							{
								if (this.velocity.X != velocity.X)
								{
									this.velocity.X = velocity.X * -0.4f;
									if (this.type == 29)
									{
										this.velocity.X = this.velocity.X * 0.8f;
									}
								}
								if (this.velocity.Y != velocity.Y && (double)velocity.Y > 0.7 && this.type != 102)
								{
									this.velocity.Y = velocity.Y * -0.4f;
									if (this.type == 29)
									{
										this.velocity.Y = this.velocity.Y * 0.8f;
									}
								}
								if (this.type == 134 || this.type == 137 || this.type == 140 || this.type == 143 || this.type == 303 || (this.type >= 338 && this.type <= 341))
								{
									this.velocity *= 0f;
									this.alpha = 255;
									this.timeLeft = 3;
								}
							}
							else if (this.aiStyle == 68)
							{
								this.velocity *= 0f;
								this.alpha = 255;
								this.timeLeft = 3;
							}
							else if (this.aiStyle != 9 || this.owner == Main.myPlayer)
							{
								this.position += this.velocity;
								this.Kill();
							}
						}
					}
				}
			}
			this.UpdatePosition(wetVelocity);
			if (!flag2 && !flag4 && this.owner == Main.myPlayer && (this.tileCollide || flag5) && this.position != this.oldPosition)
			{
				Vector2 position = this.position;
				Vector2 oldPosition = this.oldPosition;
				int num22 = (overrideWidth != -1) ? overrideWidth : this.width;
				int num23 = (overrideHeight != -1) ? overrideHeight : this.height;
				if (overrideHeight != -1 || overrideWidth != -1)
				{
					position = new Vector2(this.position.X + (float)(this.width / 2) - (float)(num22 / 2), this.position.Y + (float)(this.height / 2) - (float)(num23 / 2));
					oldPosition = new Vector2(this.oldPosition.X + (float)(this.width / 2) - (float)(num22 / 2), this.oldPosition.Y + (float)(this.height / 2) - (float)(num23 / 2));
				}
				Collision.SwitchTiles(position, num22, num23, oldPosition, 4);
			}
		}

		private void UpdatePosition(Vector2 wetVelocity)
		{
			if (this.aiStyle != 4 && this.aiStyle != 38)
			{
				if (this.aiStyle == 84)
				{
					return;
				}
				if (this.aiStyle == 7 && this.ai[0] == 2f)
				{
					return;
				}
				if ((this.type == 440 || this.type == 449 || this.type == 606) && this.ai[1] == 1f)
				{
					return;
				}
				if (this.aiStyle == 93 && this.ai[0] < 0f)
				{
					return;
				}
				if (this.type == 540)
				{
					return;
				}
				if (this.wet)
				{
					this.position += wetVelocity;
				}
				else
				{
					this.position += this.velocity;
				}
				if (Main.projPet[this.type] && this.tileCollide)
				{
					Vector4 vector = Collision.SlopeCollision(this.position, this.velocity, this.width, this.height, 0f, false);
					this.position.X = vector.X;
					this.position.Y = vector.Y;
					this.velocity.X = vector.Z;
					this.velocity.Y = vector.W;
				}
			}
		}

		public bool CanReflect()
		{
			return this.active && this.friendly && !this.hostile && this.damage > 0 && (this.aiStyle == 1 || this.aiStyle == 2 || this.aiStyle == 8 || this.aiStyle == 21 || this.aiStyle == 24 || this.aiStyle == 28 || this.aiStyle == 29);
		}
		public float GetPrismHue(float indexing)
		{
			return (float)((int)indexing) / 6f;
		}
		public static int GetByUUID(int owner, float uuid)
		{
			return Projectile.GetByUUID(owner, (int)uuid);
		}
		public static int GetByUUID(int owner, int uuid)
		{
			if (uuid < 0 || uuid >= 1000 || owner < 0 || owner >= 255)
			{
				return -1;
			}
			int num = Main.projectileIdentity[owner, uuid];
			if (num >= 0 && Main.projectile[num].active)
			{
				return num;
			}
			return -1;
		}
		public void ProjectileFixDesperation()
		{
			if (this.owner < 0 || this.owner >= 1000)
			{
				return;
			}
			int num = this.type;
			if (num != 461 && num != 632)
			{
				switch (num)
				{
					case 642:
					case 644:
						break;
					case 643:
						return;
					default:
						return;
				}
			}
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].owner == this.owner && (float)Main.projectile[i].identity == this.ai[1] && Main.projectile[i].active)
				{
					this.ai[1] = (float)i;
					return;
				}
			}
		}
		public void AI()
		{
			if (ServerApi.Hooks.InvokeProjectileAIUpdate(this))
			{
				return;
			}
			if (this.aiStyle == 1)
			{
				this.AI_001();
				return;
			}
			if (this.aiStyle == 2)
			{
				if (this.type == 304 && this.localAI[0] == 0f)
				{
					this.localAI[0] += 1f;
					this.alpha = 0;
				}
				if (this.type == 335)
				{
					this.frame = (int)this.ai[1];
				}
				if (this.type == 510)
				{
					this.rotation += Math.Abs(this.velocity.X) * 0.04f * (float)this.direction;
				}
				else
				{
					this.rotation += (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.03f * (float)this.direction;
				}
				if (this.type == 162)
				{
					if (this.ai[1] == 0f)
					{
						this.ai[1] = 1f;
					}
					this.ai[0] += 1f;
					if (this.ai[0] >= 18f)
					{
						this.velocity.Y = this.velocity.Y + 0.28f;
						this.velocity.X = this.velocity.X * 0.99f;
					}
					if (this.ai[0] > 2f)
					{
						this.alpha = 0;
					}
				}
				else if (this.type == 281)
				{
					if (this.ai[1] == 0f)
					{
						this.ai[1] = 1f;
					}
					this.ai[0] += 1f;
					if (this.ai[0] >= 18f)
					{
						this.velocity.Y = this.velocity.Y + 0.28f;
						this.velocity.X = this.velocity.X * 0.99f;
					}
					if (this.ai[0] > 2f)
					{
						this.alpha = 0;
					}
				}
				else if (this.type == 240)
				{
					if (this.ai[1] == 0f)
					{
						this.ai[1] = 1f;
					}
					this.ai[0] += 1f;
					if (this.ai[0] >= 16f)
					{
						this.velocity.Y = this.velocity.Y + 0.18f;
						this.velocity.X = this.velocity.X * 0.991f;
					}
					if (this.ai[0] > 2f)
					{
						this.alpha = 0;
					}
				}
				else if (this.type == 497)
				{
					this.ai[0] += 1f;
					if (this.ai[0] >= 30f)
					{
						this.velocity.X = this.velocity.X * 0.99f;
						this.velocity.Y = this.velocity.Y + 0.5f;
					}
					else
					{
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
					}
				}
				else if (this.type == 249)
				{
					this.ai[0] += 1f;
					if (this.ai[0] >= 0f)
					{
						this.velocity.Y = this.velocity.Y + 0.25f;
					}
				}
				else if (this.type == 347)
				{
					this.ai[0] += 1f;
					if (this.ai[0] >= 5f)
					{
						this.velocity.Y = this.velocity.Y + 0.25f;
					}
				}
				else if (this.type == 501)
				{
					this.ai[0] += 1f;
					if (this.ai[0] >= 18f)
					{
						this.velocity.X = this.velocity.X * 0.995f;
						this.velocity.Y = this.velocity.Y + 0.2f;
					}
				}
				else if (this.type == 504)
				{
					this.alpha = 255;
					this.ai[0] += 1f;
					if (this.ai[0] > 3f)
					{
						int num15 = 100;
						if (this.ai[0] > 20f)
						{
							int num16 = 40;
							float num17 = this.ai[0] - 20f;
							num15 = (int)(100f * (1f - num17 / (float)num16));
							if (num17 >= (float)num16)
							{
								this.Kill();
							}
						}
						if (this.ai[0] <= 10f)
						{
							num15 = (int)this.ai[0] * 10;
						}
					}
					if (this.ai[0] >= 20f)
					{
						this.velocity.X = this.velocity.X * 0.99f;
						this.velocity.Y = this.velocity.Y + 0.1f;
					}
				}
				else if (this.type == 69 || this.type == 70 || this.type == 621)
				{
					this.ai[0] += 1f;
					if (this.ai[0] >= 10f)
					{
						this.velocity.Y = this.velocity.Y + 0.25f;
						this.velocity.X = this.velocity.X * 0.99f;
					}
				}
				else if (this.type == 166)
				{
					this.ai[0] += 1f;
					if (this.ai[0] >= 20f)
					{
						this.velocity.Y = this.velocity.Y + 0.3f;
						this.velocity.X = this.velocity.X * 0.98f;
					}
				}
				else if (this.type == 300)
				{
					if (this.ai[0] == 0f)
					{
					}
					this.ai[0] += 1f;
					if (this.ai[0] >= 60f)
					{
						this.velocity.Y = this.velocity.Y + 0.2f;
						this.velocity.X = this.velocity.X * 0.99f;
					}
				}
				else if (this.type == 306)
				{
					this.alpha -= 50;
					if (this.alpha < 0)
					{
						this.alpha = 0;
					}
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 0.785f;
				}
				else if (this.type == 304)
				{
					this.ai[0] += 1f;
					if (this.ai[0] >= 30f)
					{
						this.alpha += 10;
						this.damage = (int)((double)this.damage * 0.9);
						this.knockBack = (float)((int)((double)this.knockBack * 0.9));
						if (this.alpha >= 255)
						{
							this.active = false;
						}
					}
					if (this.ai[0] < 30f)
					{
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
					}
				}
				else if (this.type == 370 || this.type == 371)
				{
					this.ai[0] += 1f;
					if (this.ai[0] >= 15f)
					{
						this.velocity.Y = this.velocity.Y + 0.3f;
						this.velocity.X = this.velocity.X * 0.98f;
					}
				}
				else
				{
					this.ai[0] += 1f;
					if (this.ai[0] >= 20f)
					{
						this.velocity.Y = this.velocity.Y + 0.4f;
						this.velocity.X = this.velocity.X * 0.97f;
					}
					else if (this.type == 48 || this.type == 54 || this.type == 93 || this.type == 520 || this.type == 599)
					{
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
					}
				}
				if (this.velocity.Y > 16f)
				{
					this.velocity.Y = 16f;
				}
				if (this.type == 54 && Main.rand.Next(20) == 0)
				{
					return;
				}
			}
			else if (this.aiStyle == 3)
			{
				if (this.ai[0] == 0f)
				{
					this.ai[1] += 1f;
					if (this.type == 106 && this.ai[1] >= 45f)
					{
						this.ai[0] = 1f;
						this.ai[1] = 0f;
						this.netUpdate = true;
					}
					if (this.type == 320 || this.type == 383)
					{
						if (this.ai[1] >= 10f)
						{
							this.velocity.Y = this.velocity.Y + 0.5f;
							if (this.type == 383 && this.velocity.Y < 0f)
							{
								this.velocity.Y = this.velocity.Y + 0.35f;
							}
							this.velocity.X = this.velocity.X * 0.95f;
							if (this.velocity.Y > 16f)
							{
								this.velocity.Y = 16f;
							}
							if (this.type == 383 && Vector2.Distance(base.Center, Main.player[this.owner].Center) > 800f)
							{
								this.ai[0] = 1f;
							}
						}
					}
					else if (this.type == 182)
					{
						if (this.velocity.X > 0f)
						{
							this.spriteDirection = 1;
						}
						else if (this.velocity.X < 0f)
						{
							this.spriteDirection = -1;
						}
						float num30 = this.position.X;
						float num31 = this.position.Y;
						bool flag = false;
						if (this.ai[1] > 10f)
						{
							for (int num32 = 0; num32 < 200; num32++)
							{
								if (Main.npc[num32].CanBeChasedBy(this, false))
								{
									float num33 = Main.npc[num32].position.X + (float)(Main.npc[num32].width / 2);
									float num34 = Main.npc[num32].position.Y + (float)(Main.npc[num32].height / 2);
									float num35 = Math.Abs(this.position.X + (float)(this.width / 2) - num33) + Math.Abs(this.position.Y + (float)(this.height / 2) - num34);
									if (num35 < 800f && Collision.CanHit(new Vector2(this.position.X + (float)(this.width / 2), this.position.Y + (float)(this.height / 2)), 1, 1, Main.npc[num32].position, Main.npc[num32].width, Main.npc[num32].height))
									{
										num30 = num33;
										num31 = num34;
										flag = true;
									}
								}
							}
						}
						if (!flag)
						{
							num30 = this.position.X + (float)(this.width / 2) + this.velocity.X * 100f;
							num31 = this.position.Y + (float)(this.height / 2) + this.velocity.Y * 100f;
							if (this.ai[1] >= 30f)
							{
								this.ai[0] = 1f;
								this.ai[1] = 0f;
								this.netUpdate = true;
							}
						}
						float num36 = 12f;
						float num37 = 0.25f;
						Vector2 vector = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
						float num38 = num30 - vector.X;
						float num39 = num31 - vector.Y;
						float num40 = (float)Math.Sqrt((double)(num38 * num38 + num39 * num39));
						num40 = num36 / num40;
						num38 *= num40;
						num39 *= num40;
						if (this.velocity.X < num38)
						{
							this.velocity.X = this.velocity.X + num37;
							if (this.velocity.X < 0f && num38 > 0f)
							{
								this.velocity.X = this.velocity.X + num37 * 2f;
							}
						}
						else if (this.velocity.X > num38)
						{
							this.velocity.X = this.velocity.X - num37;
							if (this.velocity.X > 0f && num38 < 0f)
							{
								this.velocity.X = this.velocity.X - num37 * 2f;
							}
						}
						if (this.velocity.Y < num39)
						{
							this.velocity.Y = this.velocity.Y + num37;
							if (this.velocity.Y < 0f && num39 > 0f)
							{
								this.velocity.Y = this.velocity.Y + num37 * 2f;
							}
						}
						else if (this.velocity.Y > num39)
						{
							this.velocity.Y = this.velocity.Y - num37;
							if (this.velocity.Y > 0f && num39 < 0f)
							{
								this.velocity.Y = this.velocity.Y - num37 * 2f;
							}
						}
					}
					else if (this.type == 301)
					{
						if (this.ai[1] >= 20f)
						{
							this.ai[0] = 1f;
							this.ai[1] = 0f;
							this.netUpdate = true;
						}
					}
					else if (this.ai[1] >= 30f)
					{
						this.ai[0] = 1f;
						this.ai[1] = 0f;
						this.netUpdate = true;
					}
				}
				else
				{
					this.tileCollide = false;
					float num41 = 9f;
					float num42 = 0.4f;
					if (this.type == 19)
					{
						num41 = 13f;
						num42 = 0.6f;
					}
					else if (this.type == 33)
					{
						num41 = 15f;
						num42 = 0.8f;
					}
					else if (this.type == 182)
					{
						num41 = 16f;
						num42 = 1.2f;
					}
					else if (this.type == 106)
					{
						num41 = 16f;
						num42 = 1.2f;
					}
					else if (this.type == 272)
					{
						num41 = 15f;
						num42 = 1f;
					}
					else if (this.type == 333)
					{
						num41 = 12f;
						num42 = 0.6f;
					}
					else if (this.type == 301)
					{
						num41 = 15f;
						num42 = 3f;
					}
					else if (this.type == 320)
					{
						num41 = 15f;
						num42 = 3f;
					}
					else if (this.type == 383)
					{
						num41 = 16f;
						num42 = 4f;
					}
					Vector2 vector2 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
					float num43 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector2.X;
					float num44 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector2.Y;
					float num45 = (float)Math.Sqrt((double)(num43 * num43 + num44 * num44));
					if (num45 > 3000f)
					{
						this.Kill();
					}
					num45 = num41 / num45;
					num43 *= num45;
					num44 *= num45;
					if (this.type == 383)
					{
						Vector2 vector3 = new Vector2(num43, num44) - this.velocity;
						if (vector3 != Vector2.Zero)
						{
							Vector2 vector4 = vector3;
							vector4.Normalize();
							this.velocity += vector4 * Math.Min(num42, vector3.Length());
						}
					}
					else
					{
						if (this.velocity.X < num43)
						{
							this.velocity.X = this.velocity.X + num42;
							if (this.velocity.X < 0f && num43 > 0f)
							{
								this.velocity.X = this.velocity.X + num42;
							}
						}
						else if (this.velocity.X > num43)
						{
							this.velocity.X = this.velocity.X - num42;
							if (this.velocity.X > 0f && num43 < 0f)
							{
								this.velocity.X = this.velocity.X - num42;
							}
						}
						if (this.velocity.Y < num44)
						{
							this.velocity.Y = this.velocity.Y + num42;
							if (this.velocity.Y < 0f && num44 > 0f)
							{
								this.velocity.Y = this.velocity.Y + num42;
							}
						}
						else if (this.velocity.Y > num44)
						{
							this.velocity.Y = this.velocity.Y - num42;
							if (this.velocity.Y > 0f && num44 < 0f)
							{
								this.velocity.Y = this.velocity.Y - num42;
							}
						}
					}
					if (Main.myPlayer == this.owner)
					{
						Rectangle rectangle = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
						Rectangle rectangle2 = new Rectangle((int)Main.player[this.owner].position.X, (int)Main.player[this.owner].position.Y, Main.player[this.owner].width, Main.player[this.owner].height);
						if (rectangle.Intersects(rectangle2))
						{
							this.Kill();
						}
					}
				}
				if (this.type == 106)
				{
					this.rotation += 0.3f * (float)this.direction;
					return;
				}
				if (this.type != 383)
				{
					this.rotation += 0.4f * (float)this.direction;
					return;
				}
				if (this.ai[0] == 0f)
				{
					Vector2 velocity = this.velocity;
					velocity.Normalize();
					this.rotation = (float)Math.Atan2((double)velocity.Y, (double)velocity.X) + 1.57f;
					return;
				}
				Vector2 vector5 = base.Center - Main.player[this.owner].Center;
				vector5.Normalize();
				this.rotation = (float)Math.Atan2((double)vector5.Y, (double)vector5.X) + 1.57f;
				return;
			}
			else if (this.aiStyle == 4)
			{
				this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
				if (this.ai[0] == 0f)
				{
					if (this.type >= 150 && this.type <= 152 && this.ai[1] == 0f && this.alpha == 255 && Main.rand.Next(2) == 0)
					{
						this.type++;
						this.netUpdate = true;
					}
					this.alpha -= 50;
					if (this.type >= 150 && this.type <= 152)
					{
						this.alpha -= 25;
					}
					else if (this.type == 493 || this.type == 494)
					{
						this.alpha -= 50;
					}
					if (this.alpha <= 0)
					{
						this.alpha = 0;
						this.ai[0] = 1f;
						if (this.ai[1] == 0f)
						{
							this.ai[1] += 1f;
							this.position += this.velocity * 1f;
						}
						if (this.type == 7 && Main.myPlayer == this.owner)
						{
							int num46 = this.type;
							if (this.ai[1] >= 6f)
							{
								num46++;
							}
							int num47 = Projectile.NewProjectile(this.position.X + this.velocity.X + (float)(this.width / 2), this.position.Y + this.velocity.Y + (float)(this.height / 2), this.velocity.X, this.velocity.Y, num46, this.damage, this.knockBack, this.owner, 0f, 0f);
							Main.projectile[num47].damage = this.damage;
							Main.projectile[num47].ai[1] = this.ai[1] + 1f;
							NetMessage.SendData(27, -1, -1, "", num47, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
						if (this.type == 494 && Main.myPlayer == this.owner)
						{
							int num48 = this.type;
							if (this.ai[1] >= (float)(7 + Main.rand.Next(2)))
							{
								num48--;
							}
							int num49 = this.damage;
							float num50 = this.knockBack;
							if (num48 == 493)
							{
								num49 = (int)((double)this.damage * 1.25);
								num50 = this.knockBack * 1.25f;
							}
							int number = Projectile.NewProjectile(this.position.X + this.velocity.X + (float)(this.width / 2), this.position.Y + this.velocity.Y + (float)(this.height / 2), this.velocity.X, this.velocity.Y, num48, num49, num50, this.owner, 0f, this.ai[1] + 1f);
							NetMessage.SendData(27, -1, -1, "", number, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
						if ((this.type == 150 || this.type == 151) && Main.myPlayer == this.owner)
						{
							int num51 = this.type;
							if (this.type == 150)
							{
								num51 = 151;
							}
							else if (this.type == 151)
							{
								num51 = 150;
							}
							if (this.ai[1] >= 10f && this.type == 151)
							{
								num51 = 152;
							}
							int num52 = Projectile.NewProjectile(this.position.X + this.velocity.X + (float)(this.width / 2), this.position.Y + this.velocity.Y + (float)(this.height / 2), this.velocity.X, this.velocity.Y, num51, this.damage, this.knockBack, this.owner, 0f, 0f);
							Main.projectile[num52].damage = this.damage;
							Main.projectile[num52].ai[1] = this.ai[1] + 1f;
							NetMessage.SendData(27, -1, -1, "", num52, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
					}
				}
				else
				{
					if (this.type >= 150 && this.type <= 152)
					{
						this.alpha += 3;
					}
					else if (this.type == 493 || this.type == 494)
					{
						this.alpha += 7;
					}
					else
					{
						this.alpha += 5;
					}
					if (this.alpha >= 255)
					{
						this.Kill();
						return;
					}
				}
			}
			else if (this.aiStyle == 5)
			{
				if (this.type == 503)
				{
					if (base.Center.Y > this.ai[1])
					{
						this.tileCollide = true;
					}
				}
				else if (this.type == 92)
				{
					if (this.position.Y > this.ai[1])
					{
						this.tileCollide = true;
					}
				}
				else
				{
					if (this.ai[1] == 0f && !Collision.SolidCollision(this.position, this.width, this.height))
					{
						this.ai[1] = 1f;
						this.netUpdate = true;
					}
					if (this.ai[1] != 0f)
					{
						this.tileCollide = true;
					}
				}
				if (this.type == 503)
				{
					this.alpha -= 15;
					int num58 = 150;
					if (base.Center.Y >= this.ai[1])
					{
						num58 = 0;
					}
					if (this.alpha < num58)
					{
						this.alpha = num58;
					}
					this.localAI[0] += (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.01f * (float)this.direction;
				}
				else
				{
					if (this.localAI[0] == 0f)
					{
						this.localAI[0] = 1f;
					}
					this.alpha += (int)(25f * this.localAI[0]);
					if (this.alpha > 200)
					{
						this.alpha = 200;
						this.localAI[0] = -1f;
					}
					if (this.alpha < 0)
					{
						this.alpha = 0;
						this.localAI[0] = 1f;
					}
				}
				if (this.type == 503)
				{
					this.rotation = this.velocity.ToRotation() - 1.57079637f;
				}
				else
				{
					this.rotation += (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.01f * (float)this.direction;
				}
				if (this.ai[1] == 1f || this.type == 92)
				{
					this.light = 0.9f;
					if (Main.rand.Next(20) == 0)
					{
						return;
					}
				}
			}
			else if (this.aiStyle == 6)
			{
				this.velocity *= 0.95f;
				this.ai[0] += 1f;
				if (this.ai[0] == 180f)
				{
					this.Kill();
				}
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
				if (this.type == 10 || this.type == 11 || this.type == 463)
				{
					int num63 = (int)(this.position.X / 16f) - 1;
					int num64 = (int)((this.position.X + (float)this.width) / 16f) + 2;
					int num65 = (int)(this.position.Y / 16f) - 1;
					int num66 = (int)((this.position.Y + (float)this.height) / 16f) + 2;
					if (num63 < 0)
					{
						num63 = 0;
					}
					if (num64 > Main.maxTilesX)
					{
						num64 = Main.maxTilesX;
					}
					if (num65 < 0)
					{
						num65 = 0;
					}
					if (num66 > Main.maxTilesY)
					{
						num66 = Main.maxTilesY;
					}
					for (int num67 = num63; num67 < num64; num67++)
					{
						for (int num68 = num65; num68 < num66; num68++)
						{
							Vector2 vector7;
							vector7.X = (float)(num67 * 16);
							vector7.Y = (float)(num68 * 16);
							if (this.position.X + (float)this.width > vector7.X && this.position.X < vector7.X + 16f && this.position.Y + (float)this.height > vector7.Y && this.position.Y < vector7.Y + 16f && Main.myPlayer == this.owner && Main.tile[num67, num68].active())
							{
								if (this.type == 10)
								{
									if (Main.tile[num67, num68].type == 23 || Main.tile[num67, num68].type == 199)
									{
										Main.tile[num67, num68].type = 2;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 25 || Main.tile[num67, num68].type == 203)
									{
										Main.tile[num67, num68].type = 1;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 112 || Main.tile[num67, num68].type == 234)
									{
										Main.tile[num67, num68].type = 53;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 163 || Main.tile[num67, num68].type == 200)
									{
										Main.tile[num67, num68].type = 161;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 400 || Main.tile[num67, num68].type == 401)
									{
										Main.tile[num67, num68].type = 396;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 398 || Main.tile[num67, num68].type == 399)
									{
										Main.tile[num67, num68].type = 397;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
								}
								else if (this.type == 11 || this.type == 463)
								{
									if (Main.tile[num67, num68].type == 109)
									{
										Main.tile[num67, num68].type = 2;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 116)
									{
										Main.tile[num67, num68].type = 53;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 117)
									{
										Main.tile[num67, num68].type = 1;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 164)
									{
										Main.tile[num67, num68].type = 161;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 403)
									{
										Main.tile[num67, num68].type = 396;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
									if (Main.tile[num67, num68].type == 402)
									{
										Main.tile[num67, num68].type = 397;
										WorldGen.SquareTileFrame(num67, num68, true);
										if (Main.netMode == 1)
										{
											NetMessage.SendTileSquare(-1, num67, num68, 1);
										}
									}
								}
							}
						}
					}
					return;
				}
			}
			else if (this.aiStyle == 7)
			{
				if (Main.player[this.owner].dead || Main.player[this.owner].stoned || Main.player[this.owner].webbed || Main.player[this.owner].frozen)
				{
					this.Kill();
					return;
				}
				Vector2 mountedCenter = Main.player[this.owner].MountedCenter;
				Vector2 vector8 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
				float num69 = mountedCenter.X - vector8.X;
				float num70 = mountedCenter.Y - vector8.Y;
				float num71 = (float)Math.Sqrt((double)(num69 * num69 + num70 * num70));
				this.rotation = (float)Math.Atan2((double)num70, (double)num69) - 1.57f;
				if (this.type == 256)
				{
					this.rotation = (float)Math.Atan2((double)num70, (double)num69) + 3.92500019f;
				}
				if (this.type == 446)
				{
					this.localAI[0] += 1f;
					if (this.localAI[0] >= 28f)
					{
						this.localAI[0] = 0f;
					}
					DelegateMethods.v3_1 = new Vector3(0f, 0.4f, 0.3f);
				}
				if (this.type == 652 && ++this.frameCounter >= 7)
				{
					this.frameCounter = 0;
					if (++this.frame >= Main.projFrames[this.type])
					{
						this.frame = 0;
					}
				}
				if (this.type >= 646 && this.type <= 649)
				{
					Vector3 zero = Vector3.Zero;
					switch (this.type)
					{
						case 646:
							zero = new Vector3(0.7f, 0.5f, 0.1f);
							break;
						case 647:
							zero = new Vector3(0f, 0.6f, 0.7f);
							break;
						case 648:
							zero = new Vector3(0.6f, 0.2f, 0.6f);
							break;
						case 649:
							zero = new Vector3(0.6f, 0.6f, 0.9f);
							break;
					}
					DelegateMethods.v3_1 = zero;
				}
				if (this.ai[0] == 0f)
				{
					if ((num71 > 300f && this.type == 13) || (num71 > 400f && this.type == 32) || (num71 > 440f && this.type == 73) || (num71 > 440f && this.type == 74) || (num71 > 250f && this.type == 165) || (num71 > 350f && this.type == 256) || (num71 > 500f && this.type == 315) || (num71 > 550f && this.type == 322) || (num71 > 400f && this.type == 331) || (num71 > 550f && this.type == 332) || (num71 > 400f && this.type == 372) || (num71 > 300f && this.type == 396) || (num71 > 550f && this.type >= 646 && this.type <= 649) || (num71 > 600f && this.type == 652) || (num71 > 480f && this.type >= 486 && this.type <= 489) || (num71 > 500f && this.type == 446))
					{
						this.ai[0] = 1f;
					}
					else if (this.type >= 230 && this.type <= 235)
					{
						int num72 = 300 + (this.type - 230) * 30;
						if (num71 > (float)num72)
						{
							this.ai[0] = 1f;
						}
					}
					Vector2 vector9 = base.Center - new Vector2(5f);
					Vector2 vector10 = base.Center + new Vector2(5f);
					Point point = (vector9 - new Vector2(16f)).ToTileCoordinates();
					Point point2 = (vector10 + new Vector2(32f)).ToTileCoordinates();
					int num73 = point.X;
					int num74 = point2.X;
					int num75 = point.Y;
					int num76 = point2.Y;
					if (num73 < 0)
					{
						num73 = 0;
					}
					if (num74 > Main.maxTilesX)
					{
						num74 = Main.maxTilesX;
					}
					if (num75 < 0)
					{
						num75 = 0;
					}
					if (num76 > Main.maxTilesY)
					{
						num76 = Main.maxTilesY;
					}
					for (int num77 = num73; num77 < num74; num77++)
					{
						int num78 = num75;
						while (num78 < num76)
						{
							if (Main.tile[num77, num78] == null)
							{
								Main.tile[num77, num78] = new Tile();
							}
							Vector2 vector11;
							vector11.X = (float)(num77 * 16);
							vector11.Y = (float)(num78 * 16);
							if (vector9.X + 10f > vector11.X && vector9.X < vector11.X + 16f && vector9.Y + 10f > vector11.Y && vector9.Y < vector11.Y + 16f && Main.tile[num77, num78].nactive() && (Main.tileSolid[(int)Main.tile[num77, num78].type] || Main.tile[num77, num78].type == 314) && (this.type != 403 || Main.tile[num77, num78].type == 314))
							{
								if (Main.player[this.owner].grapCount < 10)
								{
									Main.player[this.owner].grappling[Main.player[this.owner].grapCount] = this.whoAmI;
									Main.player[this.owner].grapCount++;
								}
								if (Main.myPlayer == this.owner)
								{
									int num79 = 0;
									int num80 = -1;
									int num81 = 100000;
									if (this.type == 73 || this.type == 74)
									{
										for (int num82 = 0; num82 < 1000; num82++)
										{
											if (num82 != this.whoAmI && Main.projectile[num82].active && Main.projectile[num82].owner == this.owner && Main.projectile[num82].aiStyle == 7 && Main.projectile[num82].ai[0] == 2f)
											{
												Main.projectile[num82].Kill();
											}
										}
									}
									else
									{
										int num83 = 3;
										if (this.type == 165)
										{
											num83 = 8;
										}
										if (this.type == 256)
										{
											num83 = 2;
										}
										if (this.type == 372)
										{
											num83 = 2;
										}
										if (this.type == 652)
										{
											num83 = 1;
										}
										if (this.type >= 646 && this.type <= 649)
										{
											num83 = 4;
										}
										for (int num84 = 0; num84 < 1000; num84++)
										{
											if (Main.projectile[num84].active && Main.projectile[num84].owner == this.owner && Main.projectile[num84].aiStyle == 7)
											{
												if (Main.projectile[num84].timeLeft < num81)
												{
													num80 = num84;
													num81 = Main.projectile[num84].timeLeft;
												}
												num79++;
											}
										}
										if (num79 > num83)
										{
											Main.projectile[num80].Kill();
										}
									}
								}
								WorldGen.KillTile(num77, num78, true, true, false);
								this.velocity.X = 0f;
								this.velocity.Y = 0f;
								this.ai[0] = 2f;
								this.position.X = (float)(num77 * 16 + 8 - this.width / 2);
								this.position.Y = (float)(num78 * 16 + 8 - this.height / 2);
								this.damage = 0;
								this.netUpdate = true;
								if (Main.myPlayer == this.owner)
								{
									NetMessage.SendData(13, -1, -1, "", this.owner, 0f, 0f, 0f, 0, 0, 0);
									break;
								}
								break;
							}
							else
							{
								num78++;
							}
						}
						if (this.ai[0] == 2f)
						{
							return;
						}
					}
					return;
				}
				if (this.ai[0] == 1f)
				{
					float num85 = 11f;
					if (this.type == 32)
					{
						num85 = 15f;
					}
					if (this.type == 73 || this.type == 74)
					{
						num85 = 17f;
					}
					if (this.type == 315)
					{
						num85 = 20f;
					}
					if (this.type == 322)
					{
						num85 = 22f;
					}
					if (this.type >= 230 && this.type <= 235)
					{
						num85 = 11f + (float)(this.type - 230) * 0.75f;
					}
					if (this.type == 446)
					{
						num85 = 20f;
					}
					if (this.type >= 486 && this.type <= 489)
					{
						num85 = 18f;
					}
					if (this.type >= 646 && this.type <= 649)
					{
						num85 = 24f;
					}
					if (this.type == 652)
					{
						num85 = 24f;
					}
					if (this.type == 332)
					{
						num85 = 17f;
					}
					if (num71 < 24f)
					{
						this.Kill();
					}
					num71 = num85 / num71;
					num69 *= num71;
					num70 *= num71;
					this.velocity.X = num69;
					this.velocity.Y = num70;
					return;
				}
				if (this.ai[0] == 2f)
				{
					int num86 = (int)(this.position.X / 16f) - 1;
					int num87 = (int)((this.position.X + (float)this.width) / 16f) + 2;
					int num88 = (int)(this.position.Y / 16f) - 1;
					int num89 = (int)((this.position.Y + (float)this.height) / 16f) + 2;
					if (num86 < 0)
					{
						num86 = 0;
					}
					if (num87 > Main.maxTilesX)
					{
						num87 = Main.maxTilesX;
					}
					if (num88 < 0)
					{
						num88 = 0;
					}
					if (num89 > Main.maxTilesY)
					{
						num89 = Main.maxTilesY;
					}
					bool flag2 = true;
					for (int num90 = num86; num90 < num87; num90++)
					{
						for (int num91 = num88; num91 < num89; num91++)
						{
							if (Main.tile[num90, num91] == null)
							{
								Main.tile[num90, num91] = new Tile();
							}
							Vector2 vector12;
							vector12.X = (float)(num90 * 16);
							vector12.Y = (float)(num91 * 16);
							if (this.position.X + (float)(this.width / 2) > vector12.X && this.position.X + (float)(this.width / 2) < vector12.X + 16f && this.position.Y + (float)(this.height / 2) > vector12.Y && this.position.Y + (float)(this.height / 2) < vector12.Y + 16f && Main.tile[num90, num91].nactive() && (Main.tileSolid[(int)Main.tile[num90, num91].type] || Main.tile[num90, num91].type == 314 || Main.tile[num90, num91].type == 5))
							{
								flag2 = false;
							}
						}
					}
					if (flag2)
					{
						this.ai[0] = 1f;
						return;
					}
					if (Main.player[this.owner].grapCount < 10)
					{
						Main.player[this.owner].grappling[Main.player[this.owner].grapCount] = this.whoAmI;
						Main.player[this.owner].grapCount++;
						return;
					}
				}
			}
			else if (this.aiStyle == 8)
			{
				if (this.type == 258 && this.localAI[0] == 0f)
				{
					this.localAI[0] = 1f;
				}
				if (this.type == 96 && this.localAI[0] == 0f)
				{
					this.localAI[0] = 1f;
				}
				if (this.type != 27 && this.type != 96 && this.type != 258)
				{
					this.ai[1] += 1f;
				}
				if (this.ai[1] >= 20f)
				{
					this.velocity.Y = this.velocity.Y + 0.2f;
				}
				if (this.type == 502)
				{
					this.rotation = this.velocity.ToRotation() + 1.57079637f;
					if (this.velocity.X != 0f)
					{
						this.spriteDirection = (this.direction = Math.Sign(this.velocity.X));
					}
				}
				else
				{
					this.rotation += 0.3f * (float)this.direction;
				}
				if (this.velocity.Y > 16f)
				{
					this.velocity.Y = 16f;
					return;
				}
			}
			else if (this.aiStyle == 9)
			{
				if (Main.myPlayer == this.owner && this.ai[0] <= 0f)
				{
					if (Main.player[this.owner].channel)
					{
						float num113 = 12f;
						if (this.type == 16)
						{
							num113 = 15f;
						}
						if (this.type == 491)
						{
							num113 = 20f;
						}
						Vector2 vector13 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
						float num114 = (float)Main.mouseX + Main.screenPosition.X - vector13.X;
						float num115 = (float)Main.mouseY + Main.screenPosition.Y - vector13.Y;
						if (Main.player[this.owner].gravDir == -1f)
						{
							num115 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector13.Y;
						}
						float num116 = (float)Math.Sqrt((double)(num114 * num114 + num115 * num115));
						num116 = (float)Math.Sqrt((double)(num114 * num114 + num115 * num115));
						if (this.ai[0] < 0f)
						{
							this.ai[0] += 1f;
						}
						if (this.type == 491 && num116 < 100f)
						{
							if (this.velocity.Length() < num113)
							{
								this.velocity *= 1.1f;
								if (this.velocity.Length() > num113)
								{
									this.velocity.Normalize();
									this.velocity *= num113;
								}
							}
							if (this.ai[0] == 0f)
							{
								this.ai[0] = -10f;
							}
						}
						else if (num116 > num113)
						{
							num116 = num113 / num116;
							num114 *= num116;
							num115 *= num116;
							int num117 = (int)(num114 * 1000f);
							int num118 = (int)(this.velocity.X * 1000f);
							int num119 = (int)(num115 * 1000f);
							int num120 = (int)(this.velocity.Y * 1000f);
							if (num117 != num118 || num119 != num120)
							{
								this.netUpdate = true;
							}
							if (this.type == 491)
							{
								Vector2 vector14 = new Vector2(num114, num115);
								this.velocity = (this.velocity * 4f + vector14) / 5f;
							}
							else
							{
								this.velocity.X = num114;
								this.velocity.Y = num115;
							}
						}
						else
						{
							int num121 = (int)(num114 * 1000f);
							int num122 = (int)(this.velocity.X * 1000f);
							int num123 = (int)(num115 * 1000f);
							int num124 = (int)(this.velocity.Y * 1000f);
							if (num121 != num122 || num123 != num124)
							{
								this.netUpdate = true;
							}
							this.velocity.X = num114;
							this.velocity.Y = num115;
						}
					}
					else if (this.ai[0] <= 0f)
					{
						this.netUpdate = true;
						if (this.type != 491)
						{
							float num125 = 12f;
							Vector2 vector15 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
							float num126 = (float)Main.mouseX + Main.screenPosition.X - vector15.X;
							float num127 = (float)Main.mouseY + Main.screenPosition.Y - vector15.Y;
							if (Main.player[this.owner].gravDir == -1f)
							{
								num127 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector15.Y;
							}
							float num128 = (float)Math.Sqrt((double)(num126 * num126 + num127 * num127));
							if (num128 == 0f || this.ai[0] < 0f)
							{
								vector15 = new Vector2(Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2), Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2));
								num126 = this.position.X + (float)this.width * 0.5f - vector15.X;
								num127 = this.position.Y + (float)this.height * 0.5f - vector15.Y;
								num128 = (float)Math.Sqrt((double)(num126 * num126 + num127 * num127));
							}
							num128 = num125 / num128;
							num126 *= num128;
							num127 *= num128;
							this.velocity.X = num126;
							this.velocity.Y = num127;
							if (this.velocity.X == 0f && this.velocity.Y == 0f)
							{
								this.Kill();
							}
						}
						this.ai[0] = 1f;
					}
				}
				if (this.type == 491)
				{
					this.localAI[0] += 1f;
					if (this.ai[0] > 0f && this.localAI[0] > 15f)
					{
						this.tileCollide = false;
						Vector2 vector16 = Main.player[this.owner].Center - base.Center;
						if (vector16.Length() < 20f)
						{
							this.Kill();
						}
						vector16.Normalize();
						vector16 *= 25f;
						this.velocity = (this.velocity * 5f + vector16) / 6f;
					}
					if (this.ai[0] < 0f || (this.velocity.X == 0f && this.velocity.Y == 0f))
					{
						this.rotation += 0.3f;
					}
					else if (this.ai[0] > 0f)
					{
						this.rotation += 0.3f * (float)this.direction;
					}
					else
					{
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
					}
				}
				else if (this.type == 34)
				{
					this.rotation += 0.3f * (float)this.direction;
				}
				else if (this.velocity.X != 0f || this.velocity.Y != 0f)
				{
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) - 2.355f;
				}
				if (this.velocity.Y > 16f)
				{
					this.velocity.Y = 16f;
					return;
				}
			}
			else if (this.aiStyle == 10)
			{
				this.tileCollide = true;
				this.localAI[1] = 0f;
				if (Main.myPlayer == this.owner && this.ai[0] == 0f)
				{
					this.tileCollide = false;
					if (Main.player[this.owner].channel)
					{
						this.localAI[1] = -1f;
						float num140 = 12f;
						Vector2 vector17 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
						float num141 = (float)Main.mouseX + Main.screenPosition.X - vector17.X;
						float num142 = (float)Main.mouseY + Main.screenPosition.Y - vector17.Y;
						if (Main.player[this.owner].gravDir == -1f)
						{
							num142 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector17.Y;
						}
						float num143 = (float)Math.Sqrt((double)(num141 * num141 + num142 * num142));
						num143 = (float)Math.Sqrt((double)(num141 * num141 + num142 * num142));
						if (num143 > num140)
						{
							num143 = num140 / num143;
							num141 *= num143;
							num142 *= num143;
							if (num141 != this.velocity.X || num142 != this.velocity.Y)
							{
								this.netUpdate = true;
							}
							this.velocity.X = num141;
							this.velocity.Y = num142;
						}
						else
						{
							if (num141 != this.velocity.X || num142 != this.velocity.Y)
							{
								this.netUpdate = true;
							}
							this.velocity.X = num141;
							this.velocity.Y = num142;
						}
					}
					else
					{
						this.ai[0] = 1f;
						this.netUpdate = true;
					}
				}
				if (this.ai[0] == 1f && this.type != 109)
				{
					if (this.type == 42 || this.type == 65 || this.type == 68 || this.type == 354)
					{
						this.ai[1] += 1f;
						if (this.ai[1] >= 60f)
						{
							this.ai[1] = 60f;
							this.velocity.Y = this.velocity.Y + 0.2f;
						}
					}
					else
					{
						this.velocity.Y = this.velocity.Y + 0.41f;
					}
				}
				else if (this.ai[0] == 2f && this.type != 109)
				{
					this.velocity.Y = this.velocity.Y + 0.2f;
					if ((double)this.velocity.X < -0.04)
					{
						this.velocity.X = this.velocity.X + 0.04f;
					}
					else if ((double)this.velocity.X > 0.04)
					{
						this.velocity.X = this.velocity.X - 0.04f;
					}
					else
					{
						this.velocity.X = 0f;
					}
				}
				this.rotation += 0.1f;
				if (this.velocity.Y > 10f)
				{
					this.velocity.Y = 10f;
					return;
				}
			}
			else if (this.aiStyle == 11)
			{
				if (this.type == 72 || this.type == 86 || this.type == 87)
				{
					if (this.velocity.X > 0f)
					{
						this.spriteDirection = -1;
					}
					else if (this.velocity.X < 0f)
					{
						this.spriteDirection = 1;
					}
					this.rotation = this.velocity.X * 0.1f;
					this.frameCounter++;
					if (this.frameCounter >= 4)
					{
						this.frame++;
						this.frameCounter = 0;
					}
					if (this.frame >= 4)
					{
						this.frame = 0;
					}
				}
				else
				{
					this.rotation += 0.02f;
				}
				if (Main.myPlayer == this.owner)
				{
					if (this.type == 72)
					{
						if (Main.player[Main.myPlayer].blueFairy)
						{
							this.timeLeft = 2;
						}
					}
					else if (this.type == 86)
					{
						if (Main.player[Main.myPlayer].redFairy)
						{
							this.timeLeft = 2;
						}
					}
					else if (this.type == 87)
					{
						if (Main.player[Main.myPlayer].greenFairy)
						{
							this.timeLeft = 2;
						}
					}
					else if (Main.player[Main.myPlayer].lightOrb)
					{
						this.timeLeft = 2;
					}
				}
				if (Main.player[this.owner].dead)
				{
					this.Kill();
					return;
				}
				float num146 = 3f;
				if (this.type == 72 || this.type == 86 || this.type == 87)
				{
					num146 = 3.75f;
				}
				Vector2 vector18 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
				float num147 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector18.X;
				float num148 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector18.Y;
				int num149 = 70;
				if (this.type == 18)
				{
					if (Main.player[this.owner].controlUp)
					{
						num148 = Main.player[this.owner].position.Y - 40f - vector18.Y;
						num147 -= 6f;
						num149 = 4;
					}
					else if (Main.player[this.owner].controlDown)
					{
						num148 = Main.player[this.owner].position.Y + (float)Main.player[this.owner].height + 40f - vector18.Y;
						num147 -= 6f;
						num149 = 4;
					}
				}
				float num150 = (float)Math.Sqrt((double)(num147 * num147 + num148 * num148));
				num150 = (float)Math.Sqrt((double)(num147 * num147 + num148 * num148));
				if (this.type == 72 || this.type == 86 || this.type == 87)
				{
					num149 = 40;
				}
				if (num150 > 800f)
				{
					this.position.X = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - (float)(this.width / 2);
					this.position.Y = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - (float)(this.height / 2);
					return;
				}
				if (num150 > (float)num149)
				{
					num150 = num146 / num150;
					num147 *= num150;
					num148 *= num150;
					this.velocity.X = num147;
					this.velocity.Y = num148;
					return;
				}
				this.velocity.X = 0f;
				this.velocity.Y = 0f;
				return;
			}
			else if (this.aiStyle == 12)
			{
				if (this.type == 288 && this.localAI[0] == 0f)
				{
					this.localAI[0] = 1f;
				}
				if (this.type == 280 || this.type == 288)
				{
					this.scale -= 0.002f;
					if (this.scale <= 0f)
					{
						this.Kill();
					}
					if (this.type == 288)
					{
						this.ai[0] = 4f;
					}
					if (this.ai[0] <= 3f)
					{
						this.ai[0] += 1f;
						return;
					}
					this.velocity.Y = this.velocity.Y + 0.075f;
					if (Main.rand.Next(8) == 0)
					{
						return;
					}
				}
				else
				{
					this.scale -= 0.02f;
					if (this.scale <= 0f)
					{
						this.Kill();
					}
					if (this.ai[0] > 3f)
					{
						this.velocity.Y = this.velocity.Y + 0.2f;
						return;
					}
					this.ai[0] += 1f;
					return;
				}
			}
			else if (this.aiStyle == 13)
			{
				if (Main.player[this.owner].dead)
				{
					this.Kill();
					return;
				}
				if (this.type != 481)
				{
					Main.player[this.owner].itemAnimation = 5;
					Main.player[this.owner].itemTime = 5;
				}
				if (this.alpha == 0)
				{
					if (this.position.X + (float)(this.width / 2) > Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2))
					{
						Main.player[this.owner].ChangeDir(1);
					}
					else
					{
						Main.player[this.owner].ChangeDir(-1);
					}
				}
				if (this.type == 481)
				{
					if (this.ai[0] == 0f)
					{
						this.extraUpdates = 0;
					}
					else
					{
						this.extraUpdates = 1;
					}
				}
				Vector2 vector19 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
				float num166 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector19.X;
				float num167 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector19.Y;
				float num168 = (float)Math.Sqrt((double)(num166 * num166 + num167 * num167));
				if (this.ai[0] == 0f)
				{
					if (num168 > 700f)
					{
						this.ai[0] = 1f;
					}
					else if (this.type == 262 && num168 > 500f)
					{
						this.ai[0] = 1f;
					}
					else if (this.type == 271 && num168 > 200f)
					{
						this.ai[0] = 1f;
					}
					else if (this.type == 273 && num168 > 150f)
					{
						this.ai[0] = 1f;
					}
					else if (this.type == 481 && num168 > 350f)
					{
						this.ai[0] = 1f;
					}
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
					this.ai[1] += 1f;
					if (this.ai[1] > 5f)
					{
						this.alpha = 0;
					}
					if (this.type == 262 && this.ai[1] > 8f)
					{
						this.ai[1] = 8f;
					}
					if (this.type == 271 && this.ai[1] > 8f)
					{
						this.ai[1] = 8f;
					}
					if (this.type == 273 && this.ai[1] > 8f)
					{
						this.ai[1] = 8f;
					}
					if (this.type == 481 && this.ai[1] > 8f)
					{
						this.ai[1] = 8f;
					}
					if (this.type == 404 && this.ai[1] > 8f)
					{
						this.ai[1] = 0f;
					}
					if (this.ai[1] >= 10f)
					{
						this.ai[1] = 15f;
						this.velocity.Y = this.velocity.Y + 0.3f;
					}
					if (this.type == 262 && this.velocity.X < 0f)
					{
						this.spriteDirection = -1;
					}
					else if (this.type == 262)
					{
						this.spriteDirection = 1;
					}
					if (this.type == 271 && this.velocity.X < 0f)
					{
						this.spriteDirection = -1;
						return;
					}
					if (this.type == 271)
					{
						this.spriteDirection = 1;
						return;
					}
				}
				else if (this.ai[0] == 1f)
				{
					this.tileCollide = false;
					this.rotation = (float)Math.Atan2((double)num167, (double)num166) - 1.57f;
					float num169 = 20f;
					if (this.type == 262)
					{
						num169 = 30f;
					}
					if (num168 < 50f)
					{
						this.Kill();
					}
					num168 = num169 / num168;
					num166 *= num168;
					num167 *= num168;
					this.velocity.X = num166;
					this.velocity.Y = num167;
					if (this.type == 262 && this.velocity.X < 0f)
					{
						this.spriteDirection = 1;
					}
					else if (this.type == 262)
					{
						this.spriteDirection = -1;
					}
					if (this.type == 271 && this.velocity.X < 0f)
					{
						this.spriteDirection = 1;
						return;
					}
					if (this.type == 271)
					{
						this.spriteDirection = -1;
						return;
					}
				}
			}
			else if (this.aiStyle == 14)
			{
				if (this.type == 473 && Main.netMode != 2)
				{
					this.localAI[0] += 1f;
					if (this.localAI[0] >= 10f)
					{
						this.localAI[0] = 0f;
					}
				}
				if (this.type == 352)
				{
					if (this.localAI[1] == 0f)
					{
						this.localAI[1] = 1f;
					}
					this.alpha += (int)(25f * this.localAI[1]);
					if (this.alpha <= 0)
					{
						this.alpha = 0;
						this.localAI[1] = 1f;
					}
					else if (this.alpha >= 255)
					{
						this.alpha = 255;
						this.localAI[1] = -1f;
					}
					this.scale += this.localAI[1] * 0.01f;
				}
				if (this.type == 346)
				{
					if (this.localAI[0] == 0f)
					{
						this.localAI[0] = 1f;
					}
					this.frame = (int)this.ai[1];
					if (this.owner == Main.myPlayer && this.timeLeft == 1)
					{
						for (int num176 = 0; num176 < 5; num176++)
						{
							float num177 = 10f;
							Vector2 vector21 = new Vector2(base.Center.X, base.Center.Y);
							float num178 = (float)Main.rand.Next(-20, 21);
							float num179 = (float)Main.rand.Next(-20, 0);
							float num180 = (float)Math.Sqrt((double)(num178 * num178 + num179 * num179));
							num180 = num177 / num180;
							num178 *= num180;
							num179 *= num180;
							num178 *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
							num179 *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
							Projectile.NewProjectile(vector21.X, vector21.Y, num178, num179, 347, 40, 0f, Main.myPlayer, 0f, this.ai[1]);
						}
					}
				}
				if (this.type == 53)
				{
					try
					{
						Vector2 vector22 = Collision.TileCollision(this.position, this.velocity, this.width, this.height, false, false, 1);
						int num184 = (int)(this.position.X / 16f) - 1;
						int num185 = (int)((this.position.X + (float)this.width) / 16f) + 2;
						int num186 = (int)(this.position.Y / 16f) - 1;
						int num187 = (int)((this.position.Y + (float)this.height) / 16f) + 2;
						if (num184 < 0)
						{
							num184 = 0;
						}
						if (num185 > Main.maxTilesX)
						{
							num185 = Main.maxTilesX;
						}
						if (num186 < 0)
						{
							num186 = 0;
						}
						if (num187 > Main.maxTilesY)
						{
							num187 = Main.maxTilesY;
						}
						for (int num188 = num184; num188 < num185; num188++)
						{
							for (int num189 = num186; num189 < num187; num189++)
							{
								if (Main.tile[num188, num189] != null && Main.tile[num188, num189].nactive() && (Main.tileSolid[(int)Main.tile[num188, num189].type] || (Main.tileSolidTop[(int)Main.tile[num188, num189].type] && Main.tile[num188, num189].frameY == 0)))
								{
									Vector2 vector23;
									vector23.X = (float)(num188 * 16);
									vector23.Y = (float)(num189 * 16);
									if (this.position.X + (float)this.width > vector23.X && this.position.X < vector23.X + 16f && this.position.Y + (float)this.height > vector23.Y && this.position.Y < vector23.Y + 16f)
									{
										this.velocity.X = 0f;
										this.velocity.Y = -0.2f;
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
#if DEBUG
						Console.WriteLine(ex);
						System.Diagnostics.Debugger.Break();

#endif
					}
				}
				if (this.type == 277)
				{
					if (this.alpha > 0)
					{
						this.alpha -= 30;
						if (this.alpha < 0)
						{
							this.alpha = 0;
						}
					}
					if (Main.expertMode)
					{
						float num190 = 12f;
						int num191 = (int)Player.FindClosest(base.Center, 1, 1);
						Vector2 vector24 = Main.player[num191].Center - base.Center;
						vector24.Normalize();
						vector24 *= num190;
						int num192 = 200;
						this.velocity.X = (this.velocity.X * (float)(num192 - 1) + vector24.X) / (float)num192;
						if (this.velocity.Length() > 16f)
						{
							this.velocity.Normalize();
							this.velocity *= 16f;
						}
					}
				}
				if (this.type == 261 || this.type == 277)
				{
					this.ai[0] += 1f;
					if (this.ai[0] > 15f)
					{
						this.ai[0] = 15f;
						if (this.velocity.Y == 0f && this.velocity.X != 0f)
						{
							this.velocity.X = this.velocity.X * 0.97f;
							if ((double)this.velocity.X > -0.01 && (double)this.velocity.X < 0.01)
							{
								this.Kill();
							}
						}
						this.velocity.Y = this.velocity.Y + 0.2f;
					}
					this.rotation += this.velocity.X * 0.05f;
				}
				else if (this.type == 378)
				{
					if (this.localAI[0] == 0f)
					{
						this.localAI[0] += 1f;
					}
					Rectangle rectangle3 = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
					for (int num193 = 0; num193 < 200; num193++)
					{
						if (Main.npc[num193].CanBeChasedBy(this, true))
						{
							Rectangle rectangle4 = new Rectangle((int)Main.npc[num193].position.X, (int)Main.npc[num193].position.Y, Main.npc[num193].width, Main.npc[num193].height);
							if (rectangle3.Intersects(rectangle4))
							{
								this.Kill();
								return;
							}
						}
					}
					this.ai[0] += 1f;
					if (this.ai[0] > 10f)
					{
						this.ai[0] = 90f;
						if (this.velocity.Y == 0f && this.velocity.X != 0f)
						{
							this.velocity.X = this.velocity.X * 0.96f;
							if ((double)this.velocity.X > -0.01 && (double)this.velocity.X < 0.01)
							{
								this.Kill();
							}
						}
						this.velocity.Y = this.velocity.Y + 0.2f;
					}
					this.rotation += this.velocity.X * 0.1f;
				}
				else if (this.type == 483)
				{
					this.ai[0] += 1f;
					if (this.ai[0] > 5f)
					{
						if (this.owner == Main.myPlayer && this.ai[0] > (float)Main.rand.Next(20, 130))
						{
							this.Kill();
						}
						if (this.velocity.Y == 0f && this.velocity.X != 0f)
						{
							this.velocity.X = this.velocity.X * 0.97f;
							if ((double)this.velocity.X > -0.01 && (double)this.velocity.X < 0.01)
							{
								this.velocity.X = 0f;
								this.netUpdate = true;
							}
						}
						this.velocity.Y = this.velocity.Y + 0.3f;
						this.velocity.X = this.velocity.X * 0.99f;
					}
					this.rotation += this.velocity.X * 0.05f;
				}
				else if (this.type == 538)
				{
					this.ai[0] += 1f;
					if (this.ai[0] > 60f || this.velocity.Y >= 0f)
					{
						this.alpha += 6;
						this.velocity *= 0.5f;
					}
					else if (this.ai[0] > 5f)
					{
						this.velocity.Y = this.velocity.Y + 0.1f;
						this.velocity.X = this.velocity.X * 1.025f;
						this.alpha -= 23;
						this.scale = 0.8f * (255f - (float)this.alpha) / 255f;
						if (this.alpha < 0)
						{
							this.alpha = 0;
						}
					}
					if (this.alpha >= 255 && this.ai[0] > 5f)
					{
						this.Kill();
						return;
					}
				}
				else
				{
					this.ai[0] += 1f;
					if (this.ai[0] > 5f)
					{
						this.ai[0] = 5f;
						if (this.velocity.Y == 0f && this.velocity.X != 0f)
						{
							this.velocity.X = this.velocity.X * 0.97f;
							if ((double)this.velocity.X > -0.01 && (double)this.velocity.X < 0.01)
							{
								this.velocity.X = 0f;
								this.netUpdate = true;
							}
						}
						this.velocity.Y = this.velocity.Y + 0.2f;
					}
					this.rotation += this.velocity.X * 0.1f;
				}
				if (this.type == 538)
				{
					if (this.localAI[1] == 0f)
					{
						this.localAI[1] = 1f;
					}
				}
				if (this.type == 450)
				{
					if (this.ai[1] == 0f)
					{
						this.ai[1] = 1f;
					}
					if (++this.frameCounter >= 3)
					{
						this.frameCounter = 0;
						if (++this.frame >= 5)
						{
							this.frame = 0;
						}
					}
					if ((double)this.velocity.Y < 0.25 && (double)this.velocity.Y > 0.15)
					{
						this.velocity.X = this.velocity.X * 0.8f;
					}
					this.rotation = -this.velocity.X * 0.05f;
				}
				if (this.type == 480)
				{
					this.alpha = 255;
				}
				if ((this.type >= 326 && this.type <= 328) || (this.type >= 400 && this.type <= 402))
				{
					if (this.wet)
					{
						this.Kill();
					}
					if (this.ai[1] == 0f && this.type >= 326 && this.type <= 328)
					{
						this.ai[1] = 1f;
					}
					if ((double)this.velocity.Y < 0.25 && (double)this.velocity.Y > 0.15)
					{
						this.velocity.X = this.velocity.X * 0.8f;
					}
					this.rotation = -this.velocity.X * 0.05f;
				}
				if (this.velocity.Y > 16f)
				{
					this.velocity.Y = 16f;
					return;
				}
			}
			else if (this.aiStyle == 15)
			{
				if (Main.player[this.owner].dead)
				{
					this.Kill();
					return;
				}
				Main.player[this.owner].itemAnimation = 10;
				Main.player[this.owner].itemTime = 10;
				if (this.position.X + (float)(this.width / 2) > Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2))
				{
					Main.player[this.owner].ChangeDir(1);
					this.direction = 1;
				}
				else
				{
					Main.player[this.owner].ChangeDir(-1);
					this.direction = -1;
				}
				Vector2 mountedCenter2 = Main.player[this.owner].MountedCenter;
				Vector2 vector25 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
				float num205 = mountedCenter2.X - vector25.X;
				float num206 = mountedCenter2.Y - vector25.Y;
				float num207 = (float)Math.Sqrt((double)(num205 * num205 + num206 * num206));
				if (this.ai[0] == 0f)
				{
					float num208 = 160f;
					if (this.type == 63)
					{
						num208 *= 1.5f;
					}
					if (this.type == 247)
					{
						num208 *= 1.5f;
					}
					this.tileCollide = true;
					if (num207 > num208)
					{
						this.ai[0] = 1f;
						this.netUpdate = true;
					}
					else if (!Main.player[this.owner].channel)
					{
						if (this.velocity.Y < 0f)
						{
							this.velocity.Y = this.velocity.Y * 0.9f;
						}
						this.velocity.Y = this.velocity.Y + 1f;
						this.velocity.X = this.velocity.X * 0.9f;
					}
				}
				else if (this.ai[0] == 1f)
				{
					float num209 = 14f / Main.player[this.owner].meleeSpeed;
					float num210 = 0.9f / Main.player[this.owner].meleeSpeed;
					float num211 = 300f;
					if (this.type == 63)
					{
						num211 *= 1.5f;
						num209 *= 1.5f;
						num210 *= 1.5f;
					}
					if (this.type == 247)
					{
						num211 *= 1.5f;
						num209 = 15.9f;
						num210 *= 2f;
					}
					Math.Abs(num205);
					Math.Abs(num206);
					if (this.ai[1] == 1f)
					{
						this.tileCollide = false;
					}
					if (!Main.player[this.owner].channel || num207 > num211 || !this.tileCollide)
					{
						this.ai[1] = 1f;
						if (this.tileCollide)
						{
							this.netUpdate = true;
						}
						this.tileCollide = false;
						if (num207 < 20f)
						{
							this.Kill();
						}
					}
					if (!this.tileCollide)
					{
						num210 *= 2f;
					}
					int num212 = 60;
					if (this.type == 247)
					{
						num212 = 100;
					}
					if (num207 > (float)num212 || !this.tileCollide)
					{
						num207 = num209 / num207;
						num205 *= num207;
						num206 *= num207;
						new Vector2(this.velocity.X, this.velocity.Y);
						float num213 = num205 - this.velocity.X;
						float num214 = num206 - this.velocity.Y;
						float num215 = (float)Math.Sqrt((double)(num213 * num213 + num214 * num214));
						num215 = num210 / num215;
						num213 *= num215;
						num214 *= num215;
						this.velocity.X = this.velocity.X * 0.98f;
						this.velocity.Y = this.velocity.Y * 0.98f;
						this.velocity.X = this.velocity.X + num213;
						this.velocity.Y = this.velocity.Y + num214;
					}
					else
					{
						if (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y) < 6f)
						{
							this.velocity.X = this.velocity.X * 0.96f;
							this.velocity.Y = this.velocity.Y + 0.2f;
						}
						if (Main.player[this.owner].velocity.X == 0f)
						{
							this.velocity.X = this.velocity.X * 0.96f;
						}
					}
				}
				if (this.type != 247)
				{
					this.rotation = (float)Math.Atan2((double)num206, (double)num205) - this.velocity.X * 0.1f;
					return;
				}
				if (this.velocity.X < 0f)
				{
					this.rotation -= (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.01f;
				}
				else
				{
					this.rotation += (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.01f;
				}
				float num216 = this.position.X;
				float num217 = this.position.Y;
				float num218 = 600f;
				bool flag4 = false;
				if (this.owner == Main.myPlayer)
				{
					this.localAI[1] += 1f;
					if (this.localAI[1] > 20f)
					{
						this.localAI[1] = 20f;
						for (int num219 = 0; num219 < 200; num219++)
						{
							if (Main.npc[num219].CanBeChasedBy(this, false))
							{
								float num220 = Main.npc[num219].position.X + (float)(Main.npc[num219].width / 2);
								float num221 = Main.npc[num219].position.Y + (float)(Main.npc[num219].height / 2);
								float num222 = Math.Abs(this.position.X + (float)(this.width / 2) - num220) + Math.Abs(this.position.Y + (float)(this.height / 2) - num221);
								if (num222 < num218 && Collision.CanHit(this.position, this.width, this.height, Main.npc[num219].position, Main.npc[num219].width, Main.npc[num219].height))
								{
									num218 = num222;
									num216 = num220;
									num217 = num221;
									flag4 = true;
								}
							}
						}
					}
				}
				if (flag4)
				{
					this.localAI[1] = 0f;
					float num223 = 14f;
					vector25 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
					num205 = num216 - vector25.X;
					num206 = num217 - vector25.Y;
					num207 = (float)Math.Sqrt((double)(num205 * num205 + num206 * num206));
					num207 = num223 / num207;
					num205 *= num207;
					num206 *= num207;
					Projectile.NewProjectile(vector25.X, vector25.Y, num205, num206, 248, (int)((double)this.damage / 1.5), this.knockBack / 2f, Main.myPlayer, 0f, 0f);
					return;
				}
			}
			else if (this.aiStyle == 16)
			{
				if (this.type == 108 || this.type == 164)
				{
					this.ai[0] += 1f;
					if (this.ai[0] > 3f)
					{
						this.Kill();
					}
				}
				if (this.type != 37 && this.type != 397 && this.type != 470)
				{
					if (this.type != 519)
					{
						goto IL_990C;
					}
				}
				try
				{
					int num224 = (int)(this.position.X / 16f) - 1;
					int num225 = (int)((this.position.X + (float)this.width) / 16f) + 2;
					int num226 = (int)(this.position.Y / 16f) - 1;
					int num227 = (int)((this.position.Y + (float)this.height) / 16f) + 2;
					if (num224 < 0)
					{
						num224 = 0;
					}
					if (num225 > Main.maxTilesX)
					{
						num225 = Main.maxTilesX;
					}
					if (num226 < 0)
					{
						num226 = 0;
					}
					if (num227 > Main.maxTilesY)
					{
						num227 = Main.maxTilesY;
					}
					for (int num228 = num224; num228 < num225; num228++)
					{
						for (int num229 = num226; num229 < num227; num229++)
						{
							if (Main.tile[num228, num229] != null && Main.tile[num228, num229].nactive() && (Main.tileSolid[(int)Main.tile[num228, num229].type] || (Main.tileSolidTop[(int)Main.tile[num228, num229].type] && Main.tile[num228, num229].frameY == 0)))
							{
								Vector2 vector26;
								vector26.X = (float)(num228 * 16);
								vector26.Y = (float)(num229 * 16);
								if (this.position.X + (float)this.width - 4f > vector26.X && this.position.X + 4f < vector26.X + 16f && this.position.Y + (float)this.height - 4f > vector26.Y && this.position.Y + 4f < vector26.Y + 16f)
								{
									this.velocity.X = 0f;
									this.velocity.Y = -0.2f;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
#if DEBUG
					Console.WriteLine(ex);
					System.Diagnostics.Debugger.Break();

#endif
				}
			IL_990C:
				if (this.type == 519)
				{
					this.localAI[1] += 1f;
					float num230 = 180f - this.localAI[1];
					if (num230 < 0f)
					{
						num230 = 0f;
					}
					this.frameCounter++;
					if (num230 < 15f)
					{
						this.frameCounter++;
					}
					if ((float)this.frameCounter >= (num230 / 10f + 6f) / 2f)
					{
						this.frame++;
						this.frameCounter = 0;
						if (this.frame >= Main.projFrames[this.type])
						{
							this.frame = 0;
						}
					}
				}
				if (this.type == 102)
				{
					if (this.velocity.Y > 10f)
					{
						this.velocity.Y = 10f;
					}
					if (this.localAI[0] == 0f)
					{
						this.localAI[0] = 1f;
					}
					this.frameCounter++;
					if (this.frameCounter > 3)
					{
						this.frame++;
						this.frameCounter = 0;
					}
					if (this.frame > 1)
					{
						this.frame = 0;
					}
					if (this.velocity.Y == 0f)
					{
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 128;
						this.height = 128;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.damage = 40;
						this.knockBack = 8f;
						this.timeLeft = 3;
						this.netUpdate = true;
					}
				}
				if (this.type == 303 && this.timeLeft <= 3 && this.hostile)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 128;
					this.height = 128;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
				}
				if (this.owner == Main.myPlayer && this.timeLeft <= 3)
				{
					this.tileCollide = false;
					this.ai[1] = 0f;
					this.alpha = 255;
					if (this.type == 28 || this.type == 37 || this.type == 75 || this.type == 516 || this.type == 519)
					{
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 128;
						this.height = 128;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.damage = 100;
						this.knockBack = 8f;
					}
					else if (this.type == 29 || this.type == 470 || this.type == 637)
					{
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 250;
						this.height = 250;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.damage = 250;
						this.knockBack = 10f;
					}
					else if (this.type == 30 || this.type == 397 || this.type == 517 || this.type == 588)
					{
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 128;
						this.height = 128;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.knockBack = 8f;
					}
					else if (this.type == 133 || this.type == 134 || this.type == 135 || this.type == 136 || this.type == 137 || this.type == 138 || this.type == 338 || this.type == 339)
					{
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 128;
						this.height = 128;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.knockBack = 8f;
					}
					else if (this.type == 139 || this.type == 140 || this.type == 141 || this.type == 142 || this.type == 143 || this.type == 144 || this.type == 340 || this.type == 341)
					{
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 200;
						this.height = 200;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.knockBack = 10f;
					}
				}
				else
				{
					if (this.type != 30 && this.type != 517 && this.type != 588 && this.type != 397 && this.type != 108 && this.type != 133 && this.type != 134 && this.type != 135 && this.type != 136 && this.type != 137 && this.type != 138 && this.type != 139 && this.type != 140 && this.type != 141 && this.type != 142 && this.type != 143 && this.type != 144 && this.type != 164 && this.type != 303 && this.type < 338 && this.type < 341)
					{
						this.damage = 0;
					}
					if (this.type == 338 || this.type == 339 || this.type == 340 || this.type == 341)
					{
						this.localAI[1] += 1f;
						if (this.localAI[1] > 6f)
						{
							this.alpha = 0;
						}
						else
						{
							this.alpha = (int)(255f - 42f * this.localAI[1]) + 100;
							if (this.alpha > 255)
							{
								this.alpha = 255;
							}
						}
						for (int num231 = 0; num231 < 2; num231++)
						{
							float num232 = 0f;
							float num233 = 0f;
							if (num231 == 1)
							{
								num232 = this.velocity.X * 0.5f;
								num233 = this.velocity.Y * 0.5f;
							}
						}
						float num236 = this.position.X;
						float num237 = this.position.Y;
						float num238 = 600f;
						bool flag5 = false;
						this.ai[0] += 1f;
						if (this.ai[0] > 30f)
						{
							this.ai[0] = 30f;
							for (int num239 = 0; num239 < 200; num239++)
							{
								if (Main.npc[num239].CanBeChasedBy(this, false))
								{
									float num240 = Main.npc[num239].position.X + (float)(Main.npc[num239].width / 2);
									float num241 = Main.npc[num239].position.Y + (float)(Main.npc[num239].height / 2);
									float num242 = Math.Abs(this.position.X + (float)(this.width / 2) - num240) + Math.Abs(this.position.Y + (float)(this.height / 2) - num241);
									if (num242 < num238 && Collision.CanHit(this.position, this.width, this.height, Main.npc[num239].position, Main.npc[num239].width, Main.npc[num239].height))
									{
										num238 = num242;
										num236 = num240;
										num237 = num241;
										flag5 = true;
									}
								}
							}
						}
						if (!flag5)
						{
							num236 = this.position.X + (float)(this.width / 2) + this.velocity.X * 100f;
							num237 = this.position.Y + (float)(this.height / 2) + this.velocity.Y * 100f;
						}
						float num243 = 16f;
						Vector2 vector27 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
						float num244 = num236 - vector27.X;
						float num245 = num237 - vector27.Y;
						float num246 = (float)Math.Sqrt((double)(num244 * num244 + num245 * num245));
						num246 = num243 / num246;
						num244 *= num246;
						num245 *= num246;
						this.velocity.X = (this.velocity.X * 11f + num244) / 12f;
						this.velocity.Y = (this.velocity.Y * 11f + num245) / 12f;
					}
					else if (this.type == 134 || this.type == 137 || this.type == 140 || this.type == 143 || this.type == 303)
					{
						if (Math.Abs(this.velocity.X) < 15f && Math.Abs(this.velocity.Y) < 15f)
						{
							this.velocity *= 1.1f;
						}
					}
					else if (this.type == 135 || this.type == 138 || this.type == 141 || this.type == 144)
					{
						if ((double)this.velocity.X > -0.2 && (double)this.velocity.X < 0.2 && (double)this.velocity.Y > -0.2 && (double)this.velocity.Y < 0.2)
						{
							this.alpha += 2;
							if (this.alpha > 200)
							{
								this.alpha = 200;
							}
						}
						else
						{
							this.alpha = 0;
						}
					}
				}
				this.ai[0] += 1f;
				if (this.type == 338 || this.type == 339 || this.type == 340 || this.type == 341)
				{
					if (this.velocity.X < 0f)
					{
						this.spriteDirection = -1;
						this.rotation = (float)Math.Atan2((double)(-(double)this.velocity.Y), (double)(-(double)this.velocity.X)) - 1.57f;
					}
					else
					{
						this.spriteDirection = 1;
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
					}
				}
				else if (this.type == 134 || this.type == 137 || this.type == 140 || this.type == 143 || this.type == 303)
				{
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
				}
				else if (this.type == 135 || this.type == 138 || this.type == 141 || this.type == 144)
				{
					this.velocity.Y = this.velocity.Y + 0.2f;
					this.velocity *= 0.97f;
					if ((double)this.velocity.X > -0.1 && (double)this.velocity.X < 0.1)
					{
						this.velocity.X = 0f;
					}
					if ((double)this.velocity.Y > -0.1 && (double)this.velocity.Y < 0.1)
					{
						this.velocity.Y = 0f;
					}
				}
				else if (this.type == 133 || this.type == 136 || this.type == 139 || this.type == 142)
				{
					if (this.ai[0] > 15f)
					{
						if (this.velocity.Y == 0f)
						{
							this.velocity.X = this.velocity.X * 0.95f;
						}
						this.velocity.Y = this.velocity.Y + 0.2f;
					}
				}
				else if (((this.type == 30 || this.type == 397 || this.type == 517 || this.type == 588) && this.ai[0] > 10f) || (this.type != 30 && this.type != 397 && this.type != 517 && this.type != 588 && this.ai[0] > 5f))
				{
					this.ai[0] = 10f;
					if (this.velocity.Y == 0f && this.velocity.X != 0f)
					{
						this.velocity.X = this.velocity.X * 0.97f;
						if (this.type == 29 || this.type == 470 || this.type == 637)
						{
							this.velocity.X = this.velocity.X * 0.99f;
						}
						if ((double)this.velocity.X > -0.01 && (double)this.velocity.X < 0.01)
						{
							this.velocity.X = 0f;
							this.netUpdate = true;
						}
					}
					this.velocity.Y = this.velocity.Y + 0.2f;
				}
				if (this.type == 519)
				{
					this.rotation += this.velocity.X * 0.06f;
					return;
				}
				if (this.type != 134 && this.type != 137 && this.type != 140 && this.type != 143 && this.type != 303 && (this.type < 338 || this.type > 341))
				{
					this.rotation += this.velocity.X * 0.1f;
					return;
				}
			}
			else if (this.aiStyle == 17)
			{
				if (this.velocity.Y == 0f)
				{
					this.velocity.X = this.velocity.X * 0.98f;
				}
				this.rotation += this.velocity.X * 0.1f;
				this.velocity.Y = this.velocity.Y + 0.2f;
				if (this.owner == Main.myPlayer)
				{
					int num254 = (int)((this.position.X + (float)(this.width / 2)) / 16f);
					int num255 = (int)((this.position.Y + (float)this.height - 4f) / 16f);
					if (Main.tile[num254, num255] != null && !Main.tile[num254, num255].active())
					{
						int num256 = 0;
						if (this.type >= 201 && this.type <= 205)
						{
							num256 = this.type - 200;
						}
						if (this.type >= 527 && this.type <= 531)
						{
							num256 = this.type - 527 + 6;
						}
						WorldGen.PlaceTile(num254, num255, 85, false, false, this.owner, num256);
						if (Main.tile[num254, num255].active())
						{
							if (Main.netMode != 0)
							{
								NetMessage.SendData(17, -1, -1, "", 1, (float)num254, (float)num255, 85f, num256, 0, 0);
							}
							int num257 = Sign.ReadSign(num254, num255, true);
							if (num257 >= 0)
							{
								Sign.TextSign(num257, this.miscText);
							}
							this.Kill();
							return;
						}
					}
				}
			}
			else if (this.aiStyle == 18)
			{
				if (this.ai[1] == 0f && this.type == 44)
				{
					this.ai[1] = 1f;
				}
				if (this.type != 263 && this.type != 274)
				{
					this.rotation += (float)this.direction * 0.8f;
					this.ai[0] += 1f;
					if (this.ai[0] >= 30f)
					{
						if (this.ai[0] < 100f)
						{
							this.velocity *= 1.06f;
						}
						else
						{
							this.ai[0] = 200f;
						}
					}
					return;
				}
				if (this.type == 274 && this.velocity.X < 0f)
				{
					this.spriteDirection = -1;
				}
				this.rotation += (float)this.direction * 0.05f;
				this.rotation += (float)this.direction * 0.5f * ((float)this.timeLeft / 180f);
				if (this.type == 274)
				{
					this.velocity *= 0.96f;
					return;
				}
				this.velocity *= 0.95f;
				return;
			}
			else if (this.aiStyle == 19)
			{
				Vector2 vector28 = Main.player[this.owner].RotatedRelativePoint(Main.player[this.owner].MountedCenter, true);
				this.direction = Main.player[this.owner].direction;
				Main.player[this.owner].heldProj = this.whoAmI;
				Main.player[this.owner].itemTime = Main.player[this.owner].itemAnimation;
				this.position.X = vector28.X - (float)(this.width / 2);
				this.position.Y = vector28.Y - (float)(this.height / 2);
				if (!Main.player[this.owner].frozen)
				{
					if (this.type == 46)
					{
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 3f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 1.6f;
						}
						else
						{
							this.ai[0] += 1.4f;
						}
					}
					else if (this.type == 105)
					{
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 3f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 2.4f;
						}
						else
						{
							this.ai[0] += 2.1f;
						}
					}
					else if (this.type == 367)
					{
						this.spriteDirection = -this.direction;
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 3f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 1.6f;
						}
						else
						{
							this.ai[0] += 1.5f;
						}
					}
					else if (this.type == 368)
					{
						this.spriteDirection = -this.direction;
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 3f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 1.5f;
						}
						else
						{
							this.ai[0] += 1.4f;
						}
					}
					else if (this.type == 222)
					{
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 3f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 2.4f;
							if (this.localAI[0] == 0f && Main.myPlayer == this.owner)
							{
								this.localAI[0] = 1f;
								Projectile.NewProjectile(base.Center.X + this.velocity.X * this.ai[0], base.Center.Y + this.velocity.Y * this.ai[0], this.velocity.X, this.velocity.Y, 228, this.damage, this.knockBack, this.owner, 0f, 0f);
							}
						}
						else
						{
							this.ai[0] += 2.1f;
						}
					}
					else if (this.type == 342)
					{
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 3f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 2.4f;
							if (this.localAI[0] == 0f && Main.myPlayer == this.owner)
							{
								this.localAI[0] = 1f;
								if (Collision.CanHit(Main.player[this.owner].position, Main.player[this.owner].width, Main.player[this.owner].height, new Vector2(base.Center.X + this.velocity.X * this.ai[0], base.Center.Y + this.velocity.Y * this.ai[0]), this.width, this.height))
								{
									Projectile.NewProjectile(base.Center.X + this.velocity.X * this.ai[0], base.Center.Y + this.velocity.Y * this.ai[0], this.velocity.X * 2.4f, this.velocity.Y * 2.4f, 343, (int)((double)this.damage * 0.8), this.knockBack * 0.85f, this.owner, 0f, 0f);
								}
							}
						}
						else
						{
							this.ai[0] += 2.1f;
						}
					}
					else if (this.type == 47)
					{
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 4f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 1.2f;
						}
						else
						{
							this.ai[0] += 0.9f;
						}
					}
					else if (this.type == 153)
					{
						this.spriteDirection = -this.direction;
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 4f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 1.5f;
						}
						else
						{
							this.ai[0] += 1.3f;
						}
					}
					else if (this.type == 49)
					{
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 4f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 1.1f;
						}
						else
						{
							this.ai[0] += 0.85f;
						}
					}
					else if (this.type == 64 || this.type == 215)
					{
						this.spriteDirection = -this.direction;
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 3f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 1.9f;
						}
						else
						{
							this.ai[0] += 1.7f;
						}
					}
					else if (this.type == 66 || this.type == 97 || this.type == 212 || this.type == 218)
					{
						this.spriteDirection = -this.direction;
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 3f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 2.1f;
						}
						else
						{
							this.ai[0] += 1.9f;
						}
					}
					else if (this.type == 130)
					{
						this.spriteDirection = -this.direction;
						if (this.ai[0] == 0f)
						{
							this.ai[0] = 3f;
							this.netUpdate = true;
						}
						if (Main.player[this.owner].itemAnimation < Main.player[this.owner].itemAnimationMax / 3)
						{
							this.ai[0] -= 1.3f;
						}
						else
						{
							this.ai[0] += 1f;
						}
					}
				}
				this.position += this.velocity * this.ai[0];
				if (this.type == 130)
				{
					if (this.ai[1] == 0f || this.ai[1] == 4f || this.ai[1] == 8f || this.ai[1] == 12f || this.ai[1] == 16f || this.ai[1] == 20f || this.ai[1] == 24f)
					{
						Projectile.NewProjectile(this.position.X + (float)(this.width / 2), this.position.Y + (float)(this.height / 2), this.velocity.X, this.velocity.Y, 131, this.damage / 3, 0f, this.owner, 0f, 0f);
					}
					this.ai[1] += 1f;
				}
				if (Main.player[this.owner].itemAnimation == 0)
				{
					this.Kill();
				}
				this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 2.355f;
				if (this.spriteDirection == -1)
				{
					this.rotation -= 1.57f;
				}
				if (this.type == 46)
				{
					return;
				}
				if (this.type == 105)
				{
					if (Main.rand.Next(4) == 0)
					{
						return;
					}
				}
				else if (this.type == 153)
				{
					return;
				}
			}
			else if (this.aiStyle == 20)
			{
				if (this.type == 252)
				{
					this.frameCounter++;
					if (this.frameCounter >= 4)
					{
						this.frameCounter = 0;
						this.frame++;
					}
					if (this.frame > 3)
					{
						this.frame = 0;
					}
				}
				if (this.type == 509)
				{
					this.frameCounter++;
					if (this.frameCounter >= 2)
					{
						this.frameCounter = 0;
						this.frame++;
					}
					if (this.frame > 1)
					{
						this.frame = 0;
					}
				}
				if (this.soundDelay <= 0)
				{
					this.soundDelay = 30;
				}
				Vector2 vector29 = Main.player[this.owner].RotatedRelativePoint(Main.player[this.owner].MountedCenter, true);
				if (Main.myPlayer == this.owner)
				{
					if (Main.player[this.owner].channel)
					{
						float num264 = Main.player[this.owner].inventory[Main.player[this.owner].selectedItem].shootSpeed * this.scale;
						Vector2 vector30 = vector29;
						float num265 = (float)Main.mouseX + Main.screenPosition.X - vector30.X;
						float num266 = (float)Main.mouseY + Main.screenPosition.Y - vector30.Y;
						if (Main.player[this.owner].gravDir == -1f)
						{
							num266 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector30.Y;
						}
						float num267 = (float)Math.Sqrt((double)(num265 * num265 + num266 * num266));
						num267 = (float)Math.Sqrt((double)(num265 * num265 + num266 * num266));
						num267 = num264 / num267;
						num265 *= num267;
						num266 *= num267;
						if (num265 != this.velocity.X || num266 != this.velocity.Y)
						{
							this.netUpdate = true;
						}
						this.velocity.X = num265;
						this.velocity.Y = num266;
					}
					else
					{
						this.Kill();
					}
				}
				if (this.velocity.X > 0f)
				{
					Main.player[this.owner].ChangeDir(1);
				}
				else if (this.velocity.X < 0f)
				{
					Main.player[this.owner].ChangeDir(-1);
				}
				this.spriteDirection = this.direction;
				Main.player[this.owner].ChangeDir(this.direction);
				Main.player[this.owner].heldProj = this.whoAmI;
				Main.player[this.owner].itemTime = 2;
				Main.player[this.owner].itemAnimation = 2;
				this.position.X = vector29.X - (float)(this.width / 2);
				this.position.Y = vector29.Y - (float)(this.height / 2);
				this.rotation = (float)(Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.5700000524520874);
				if (Main.player[this.owner].direction == 1)
				{
					Main.player[this.owner].itemRotation = (float)Math.Atan2((double)(this.velocity.Y * (float)this.direction), (double)(this.velocity.X * (float)this.direction));
				}
				else
				{
					Main.player[this.owner].itemRotation = (float)Math.Atan2((double)(this.velocity.Y * (float)this.direction), (double)(this.velocity.X * (float)this.direction));
				}
				this.velocity.X = this.velocity.X * (1f + (float)Main.rand.Next(-3, 4) * 0.01f);
				if (Main.rand.Next(6) == 0)
				{
					return;
				}
			}
			else if (this.aiStyle == 21)
			{
				this.rotation = this.velocity.X * 0.1f;
				this.spriteDirection = -this.direction;
				if (this.ai[1] == 1f)
				{
					this.ai[1] = 0f;
					Main.harpNote = this.ai[0];
					return;
				}
			}
			else if (this.aiStyle == 22)
			{
				if (this.velocity.X == 0f && this.velocity.Y == 0f)
				{
					this.alpha = 255;
				}
				if (this.ai[1] < 0f)
				{
					if (this.velocity.X > 0f)
					{
						this.rotation += 0.3f;
					}
					else
					{
						this.rotation -= 0.3f;
					}
					int num270 = (int)(this.position.X / 16f) - 1;
					int num271 = (int)((this.position.X + (float)this.width) / 16f) + 2;
					int num272 = (int)(this.position.Y / 16f) - 1;
					int num273 = (int)((this.position.Y + (float)this.height) / 16f) + 2;
					if (num270 < 0)
					{
						num270 = 0;
					}
					if (num271 > Main.maxTilesX)
					{
						num271 = Main.maxTilesX;
					}
					if (num272 < 0)
					{
						num272 = 0;
					}
					if (num273 > Main.maxTilesY)
					{
						num273 = Main.maxTilesY;
					}
					int num274 = (int)this.position.X + 4;
					int num275 = (int)this.position.Y + 4;
					for (int num276 = num270; num276 < num271; num276++)
					{
						for (int num277 = num272; num277 < num273; num277++)
						{
							if (Main.tile[num276, num277] != null && Main.tile[num276, num277].active() && Main.tile[num276, num277].type != 127 && Main.tileSolid[(int)Main.tile[num276, num277].type] && !Main.tileSolidTop[(int)Main.tile[num276, num277].type])
							{
								Vector2 vector31;
								vector31.X = (float)(num276 * 16);
								vector31.Y = (float)(num277 * 16);
								if ((float)(num274 + 8) > vector31.X && (float)num274 < vector31.X + 16f && (float)(num275 + 8) > vector31.Y && (float)num275 < vector31.Y + 16f)
								{
									this.Kill();
								}
							}
						}
					}
					return;
				}
				if (this.ai[0] < 0f)
				{
					int num282 = (int)this.position.X / 16;
					int num283 = (int)this.position.Y / 16;
					if (Main.tile[num282, num283] == null || !Main.tile[num282, num283].active())
					{
						this.Kill();
					}
					this.ai[0] -= 1f;
					if (this.ai[0] <= -900f && (Main.myPlayer == this.owner || Main.netMode == 2) && Main.tile[num282, num283].active() && Main.tile[num282, num283].type == 127)
					{
						WorldGen.KillTile(num282, num283, false, false, false);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(17, -1, -1, "", 0, (float)num282, (float)num283, 0f, 0, 0, 0);
						}
						this.Kill();
						return;
					}
				}
				else
				{
					int num284 = (int)(this.position.X / 16f) - 1;
					int num285 = (int)((this.position.X + (float)this.width) / 16f) + 2;
					int num286 = (int)(this.position.Y / 16f) - 1;
					int num287 = (int)((this.position.Y + (float)this.height) / 16f) + 2;
					if (num284 < 0)
					{
						num284 = 0;
					}
					if (num285 > Main.maxTilesX)
					{
						num285 = Main.maxTilesX;
					}
					if (num286 < 0)
					{
						num286 = 0;
					}
					if (num287 > Main.maxTilesY)
					{
						num287 = Main.maxTilesY;
					}
					int num288 = (int)this.position.X + 4;
					int num289 = (int)this.position.Y + 4;
					for (int num290 = num284; num290 < num285; num290++)
					{
						for (int num291 = num286; num291 < num287; num291++)
						{
							if (Main.tile[num290, num291] != null && Main.tile[num290, num291].nactive() && Main.tile[num290, num291].type != 127 && Main.tileSolid[(int)Main.tile[num290, num291].type] && !Main.tileSolidTop[(int)Main.tile[num290, num291].type])
							{
								Vector2 vector32;
								vector32.X = (float)(num290 * 16);
								vector32.Y = (float)(num291 * 16);
								if ((float)(num288 + 8) > vector32.X && (float)num288 < vector32.X + 16f && (float)(num289 + 8) > vector32.Y && (float)num289 < vector32.Y + 16f)
								{
									this.Kill();
								}
							}
						}
					}
					if (this.lavaWet)
					{
						this.Kill();
					}
					if (this.active)
					{
						int num293 = (int)this.ai[0];
						int num294 = (int)this.ai[1];
						if (WorldGen.SolidTile(num293, num294))
						{
							if (Math.Abs(this.velocity.X) > Math.Abs(this.velocity.Y))
							{
								if (base.Center.Y < (float)(num294 * 16 + 8) && !WorldGen.SolidTile(num293, num294 - 1))
								{
									num294--;
								}
								else if (!WorldGen.SolidTile(num293, num294 + 1))
								{
									num294++;
								}
								else if (!WorldGen.SolidTile(num293, num294 - 1))
								{
									num294--;
								}
								else if (base.Center.X < (float)(num293 * 16 + 8) && !WorldGen.SolidTile(num293 - 1, num294))
								{
									num293--;
								}
								else if (!WorldGen.SolidTile(num293 + 1, num294))
								{
									num293++;
								}
								else if (!WorldGen.SolidTile(num293 - 1, num294))
								{
									num293--;
								}
							}
							else if (base.Center.X < (float)(num293 * 16 + 8) && !WorldGen.SolidTile(num293 - 1, num294))
							{
								num293--;
							}
							else if (!WorldGen.SolidTile(num293 + 1, num294))
							{
								num293++;
							}
							else if (!WorldGen.SolidTile(num293 - 1, num294))
							{
								num293--;
							}
							else if (base.Center.Y < (float)(num294 * 16 + 8) && !WorldGen.SolidTile(num293, num294 - 1))
							{
								num294--;
							}
							else if (!WorldGen.SolidTile(num293, num294 + 1))
							{
								num294++;
							}
							else if (!WorldGen.SolidTile(num293, num294 - 1))
							{
								num294--;
							}
						}
						if (this.velocity.X > 0f)
						{
							this.rotation += 0.3f;
						}
						else
						{
							this.rotation -= 0.3f;
						}
						if (Main.myPlayer == this.owner)
						{
							int num295 = (int)((this.position.X + (float)(this.width / 2)) / 16f);
							int num296 = (int)((this.position.Y + (float)(this.height / 2)) / 16f);
							bool flag6 = false;
							if (num295 == num293 && num296 == num294)
							{
								flag6 = true;
							}
							if (((this.velocity.X <= 0f && num295 <= num293) || (this.velocity.X >= 0f && num295 >= num293)) && ((this.velocity.Y <= 0f && num296 <= num294) || (this.velocity.Y >= 0f && num296 >= num294)))
							{
								flag6 = true;
							}
							if (flag6)
							{
								if (WorldGen.PlaceTile(num293, num294, 127, false, false, this.owner, 0))
								{
									if (Main.netMode == 1)
									{
										NetMessage.SendData(17, -1, -1, "", 1, (float)((int)this.ai[0]), (float)((int)this.ai[1]), 127f, 0, 0, 0);
									}
									this.damage = 0;
									this.ai[0] = -1f;
									this.velocity *= 0f;
									this.alpha = 255;
									this.position.X = (float)(num293 * 16);
									this.position.Y = (float)(num294 * 16);
									this.netUpdate = true;
									return;
								}
								this.ai[1] = -1f;
								return;
							}
						}
					}
				}
			}
			else
			{
				if (this.aiStyle == 23)
				{
					if (this.type == 188 && this.ai[0] < 8f)
					{
						this.ai[0] = 8f;
					}
					if (this.timeLeft > 60)
					{
						this.timeLeft = 60;
					}
					if (this.ai[0] > 7f)
					{
						this.ai[0] += 1f;
					}
					else
					{
						this.ai[0] += 1f;
					}
					this.rotation += 0.3f * (float)this.direction;
					return;
				}
				if (this.aiStyle == 24)
				{
					this.light = this.scale * 0.5f;
					this.rotation += this.velocity.X * 0.2f;
					this.ai[1] += 1f;
					if (this.type == 94)
					{
						this.velocity *= 0.985f;
						if (this.ai[1] > 130f)
						{
							this.scale -= 0.05f;
							if ((double)this.scale <= 0.2)
							{
								this.scale = 0.2f;
								this.Kill();
								return;
							}
						}
					}
					else
					{
						this.velocity *= 0.96f;
						if (this.ai[1] > 15f)
						{
							this.scale -= 0.05f;
							if ((double)this.scale <= 0.2)
							{
								this.scale = 0.2f;
								this.Kill();
								return;
							}
						}
					}
				}
				else if (this.aiStyle == 25)
				{
					if (this.ai[0] != 0f && this.velocity.Y <= 0f && this.velocity.X == 0f)
					{
						float num302 = 0.5f;
						int i2 = (int)((this.position.X - 8f) / 16f);
						int num303 = (int)(this.position.Y / 16f);
						bool flag7 = false;
						bool flag8 = false;
						if (WorldGen.SolidTile(i2, num303) || WorldGen.SolidTile(i2, num303 + 1))
						{
							flag7 = true;
						}
						i2 = (int)((this.position.X + (float)this.width + 8f) / 16f);
						if (WorldGen.SolidTile(i2, num303) || WorldGen.SolidTile(i2, num303 + 1))
						{
							flag8 = true;
						}
						if (flag7)
						{
							this.velocity.X = num302;
						}
						else if (flag8)
						{
							this.velocity.X = -num302;
						}
						else
						{
							i2 = (int)((this.position.X - 8f - 16f) / 16f);
							num303 = (int)(this.position.Y / 16f);
							flag7 = false;
							flag8 = false;
							if (WorldGen.SolidTile(i2, num303) || WorldGen.SolidTile(i2, num303 + 1))
							{
								flag7 = true;
							}
							i2 = (int)((this.position.X + (float)this.width + 8f + 16f) / 16f);
							if (WorldGen.SolidTile(i2, num303) || WorldGen.SolidTile(i2, num303 + 1))
							{
								flag8 = true;
							}
							if (flag7)
							{
								this.velocity.X = num302;
							}
							else if (flag8)
							{
								this.velocity.X = -num302;
							}
							else
							{
								i2 = (int)((this.position.X + 4f) / 16f);
								num303 = (int)((this.position.Y + (float)this.height + 8f) / 16f);
								if (WorldGen.SolidTile(i2, num303) || WorldGen.SolidTile(i2, num303 + 1))
								{
									flag7 = true;
								}
								if (!flag7)
								{
									this.velocity.X = num302;
								}
								else
								{
									this.velocity.X = -num302;
								}
							}
						}
					}
					this.rotation += this.velocity.X * 0.06f;
					this.ai[0] = 1f;
					if (this.velocity.Y > 16f)
					{
						this.velocity.Y = 16f;
					}
					if (this.velocity.Y <= 6f)
					{
						if (this.velocity.X > 0f && this.velocity.X < 7f)
						{
							this.velocity.X = this.velocity.X + 0.05f;
						}
						if (this.velocity.X < 0f && this.velocity.X > -7f)
						{
							this.velocity.X = this.velocity.X - 0.05f;
						}
					}
					this.velocity.Y = this.velocity.Y + 0.3f;
					if (this.type == 655 && this.wet)
					{
						this.Kill();
						return;
					}
				}
				else
				{
					if (this.aiStyle == 26)
					{
						this.AI_026();
						return;
					}
					if (this.aiStyle == 27)
					{
						if (this.type == 115)
						{
							this.ai[0] += 1f;
							if (this.ai[0] < 30f)
							{
								this.velocity *= 1.125f;
							}
						}
						if (this.type == 115 && this.localAI[1] < 5f)
						{
							this.localAI[1] = 5f;
						}
						if (this.localAI[1] < 15f)
						{
							this.localAI[1] += 1f;
						}
						else
						{
							if (this.localAI[0] == 0f)
							{
								this.scale -= 0.02f;
								this.alpha += 30;
								if (this.alpha >= 250)
								{
									this.alpha = 255;
									this.localAI[0] = 1f;
								}
							}
							else if (this.localAI[0] == 1f)
							{
								this.scale += 0.02f;
								this.alpha -= 30;
								if (this.alpha <= 0)
								{
									this.alpha = 0;
									this.localAI[0] = 0f;
								}
							}
						}
						if (this.ai[1] == 0f)
						{
							this.ai[1] = 1f;
						}
						if (this.type == 157)
						{
							this.rotation += (float)this.direction * 0.4f;
							this.spriteDirection = this.direction;
						}
						else
						{
							this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 0.785f;
						}
						if (this.velocity.Y > 16f)
						{
							this.velocity.Y = 16f;
							return;
						}
					}
					else if (this.aiStyle == 28)
					{
						if (this.ai[1] == 0f)
						{
							this.ai[1] = 1f;
							return;
						}
					}
					else if (this.aiStyle == 29)
					{
						if (this.type == 619)
						{
							int num323 = (int)this.ai[0];
							return;
						}
						if (this.type == 620)
						{
							int num327 = (int)this.ai[0];
							this.ai[1] += 1f;
							float num328 = (60f - this.ai[1]) / 60f;
							if (this.ai[1] > 40f)
							{
								this.Kill();
							}
							this.velocity.Y = this.velocity.Y + 0.2f;
							if (this.velocity.Y > 18f)
							{
								this.velocity.Y = 18f;
							}
							this.velocity.X = this.velocity.X * 0.98f;
							return;
						}
						if (this.type == 521)
						{
							return;
						}
						if (this.type == 522)
						{
							this.ai[1] += 1f;
							float num335 = (60f - this.ai[1]) / 60f;
							if (this.ai[1] > 40f)
							{
								this.Kill();
							}
							this.velocity.Y = this.velocity.Y + 0.2f;
							if (this.velocity.Y > 18f)
							{
								this.velocity.Y = 18f;
							}
							this.velocity.X = this.velocity.X * 0.98f;
							return;
						}
						int num339 = this.type - 121 + 86;
						if (this.type == 597)
						{
							num339 = 262;
						}
						if (this.ai[1] == 0f)
						{
							this.ai[1] = 1f;
							return;
						}
					}
					else if (this.aiStyle == 30)
					{
						this.velocity *= 0.8f;
						this.rotation += 0.2f;
						this.alpha += 4;
						if (this.alpha >= 255)
						{
							this.Kill();
							return;
						}
					}
					else
					{
						if (this.aiStyle == 31)
						{
							int conversionType = 0;
							if (this.type == 146)
							{
								conversionType = 2;
							}
							if (this.type == 147)
							{
								conversionType = 1;
							}
							if (this.type == 148)
							{
								conversionType = 3;
							}
							if (this.type == 149)
							{
								conversionType = 4;
							}
							if (this.owner == Main.myPlayer)
							{
								WorldGen.Convert((int)(this.position.X + (float)(this.width / 2)) / 16, (int)(this.position.Y + (float)(this.height / 2)) / 16, conversionType, 2);
							}
							if (this.timeLeft > 133)
							{
								this.timeLeft = 133;
							}
							if (this.ai[0] > 7f)
							{
								this.ai[0] += 1f;
							}
							else
							{
								this.ai[0] += 1f;
							}
							this.rotation += 0.3f * (float)this.direction;
							return;
						}
						if (this.aiStyle == 32)
						{
							this.timeLeft = 10;
							this.ai[0] += 1f;
							if (this.ai[0] >= 20f)
							{
								this.ai[0] = 15f;
								for (int num346 = 0; num346 < 255; num346++)
								{
									Rectangle rectangle5 = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
									if (Main.player[num346].active)
									{
										Rectangle rectangle6 = new Rectangle((int)Main.player[num346].position.X, (int)Main.player[num346].position.Y, Main.player[num346].width, Main.player[num346].height);
										if (rectangle5.Intersects(rectangle6))
										{
											this.ai[0] = 0f;
											this.velocity.Y = -4.5f;
											if (this.velocity.X > 2f)
											{
												this.velocity.X = 2f;
											}
											if (this.velocity.X < -2f)
											{
												this.velocity.X = -2f;
											}
											this.velocity.X = (this.velocity.X + (float)Main.player[num346].direction * 1.75f) / 2f;
											this.velocity.X = this.velocity.X + Main.player[num346].velocity.X * 3f;
											this.velocity.Y = this.velocity.Y + Main.player[num346].velocity.Y;
											if (this.velocity.X > 6f)
											{
												this.velocity.X = 6f;
											}
											if (this.velocity.X < -6f)
											{
												this.velocity.X = -6f;
											}
											this.netUpdate = true;
											this.ai[1] += 1f;
										}
									}
								}
							}
							if (this.velocity.X == 0f && this.velocity.Y == 0f)
							{
								this.Kill();
							}
							this.rotation += 0.02f * this.velocity.X;
							if (this.velocity.Y == 0f)
							{
								this.velocity.X = this.velocity.X * 0.98f;
							}
							else if (this.wet)
							{
								this.velocity.X = this.velocity.X * 0.99f;
							}
							else
							{
								this.velocity.X = this.velocity.X * 0.995f;
							}
							if ((double)this.velocity.X > -0.03 && (double)this.velocity.X < 0.03)
							{
								this.velocity.X = 0f;
							}
							if (this.wet)
							{
								this.ai[1] = 0f;
								if (this.velocity.Y > 0f)
								{
									this.velocity.Y = this.velocity.Y * 0.95f;
								}
								this.velocity.Y = this.velocity.Y - 0.1f;
								if (this.velocity.Y < -4f)
								{
									this.velocity.Y = -4f;
								}
								if (this.velocity.X == 0f)
								{
									this.Kill();
								}
							}
							else
							{
								this.velocity.Y = this.velocity.Y + 0.1f;
							}
							if (this.velocity.Y > 10f)
							{
								this.velocity.Y = 10f;
								return;
							}
						}
						else
						{
							if (this.aiStyle == 33)
							{
								if (this.alpha > 0)
								{
									this.alpha -= 50;
									if (this.alpha < 0)
									{
										this.alpha = 0;
									}
								}
								float num347 = 4f;
								float num348 = this.ai[0];
								float num349 = this.ai[1];
								if (num348 == 0f && num349 == 0f)
								{
									num348 = 1f;
								}
								float num350 = (float)Math.Sqrt((double)(num348 * num348 + num349 * num349));
								num350 = num347 / num350;
								num348 *= num350;
								num349 *= num350;
								if (this.alpha < 70)
								{
								}
								if (this.localAI[0] == 0f)
								{
									this.ai[0] = this.velocity.X;
									this.ai[1] = this.velocity.Y;
									this.localAI[1] += 1f;
									if (this.localAI[1] >= 30f)
									{
										this.velocity.Y = this.velocity.Y + 0.09f;
										this.localAI[1] = 30f;
									}
								}
								else
								{
									if (!Collision.SolidCollision(this.position, this.width, this.height))
									{
										this.localAI[0] = 0f;
										this.localAI[1] = 30f;
									}
									this.damage = 0;
								}
								if (this.velocity.Y > 16f)
								{
									this.velocity.Y = 16f;
								}
								this.rotation = (float)Math.Atan2((double)this.ai[1], (double)this.ai[0]) + 1.57f;
								return;
							}
							if (this.aiStyle == 34)
							{
								this.rotation = this.velocity.ToRotation() + 1.57079637f;
								if (this.ai[1] == 1f)
								{
									this.ai[0] += 1f;
									if (this.ai[0] > 2f)
									{
										return;
									}
								}
								else
								{
									if (this.type < 415 || this.type > 418)
									{
										return;
									}
									this.ai[0] += 1f;
									if (this.ai[0] > 4f)
									{
										return;
									}
								}
							}
							else if (this.aiStyle == 35)
							{
								this.ai[0] += 1f;
								if (this.ai[0] > 30f)
								{
									this.velocity.Y = this.velocity.Y + 0.2f;
									this.velocity.X = this.velocity.X * 0.985f;
									if (this.velocity.Y > 14f)
									{
										this.velocity.Y = 14f;
									}
								}
								this.rotation += (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * (float)this.direction * 0.02f;
								if (this.owner == Main.myPlayer)
								{
									Vector2 vector33 = Collision.TileCollision(this.position, this.velocity, this.width, this.height, true, true, 1);
									bool flag9 = false;
									if (vector33 != this.velocity)
									{
										flag9 = true;
									}
									else
									{
										int num358 = (int)(base.Center.X + this.velocity.X) / 16;
										int num359 = (int)(base.Center.Y + this.velocity.Y) / 16;
										if (Main.tile[num358, num359] != null && Main.tile[num358, num359].active() && Main.tile[num358, num359].bottomSlope())
										{
											flag9 = true;
											this.position.Y = (float)(num359 * 16 + 16 + 8);
											this.position.X = (float)(num358 * 16 + 8);
										}
									}
									if (flag9)
									{
										int num360 = 213;
										if (this.type == 475)
										{
											num360 = 353;
										}
										if (this.type == 506)
										{
											num360 = 366;
										}
										if (this.type == 505)
										{
											num360 = 365;
										}
										int num361 = (int)(this.position.X + (float)(this.width / 2)) / 16;
										int num362 = (int)(this.position.Y + (float)(this.height / 2)) / 16;
										this.position += vector33;
										int num363 = 10;
										if (Main.tile[num361, num362] != null)
										{
											while (Main.tile[num361, num362] != null && Main.tile[num361, num362].active())
											{
												if (!Main.tileRope[(int)Main.tile[num361, num362].type])
												{
													break;
												}
												num362++;
											}
											while (num363 > 0)
											{
												num363--;
												if (Main.tile[num361, num362] == null)
												{
													break;
												}
												if (Main.tile[num361, num362].active() && (Main.tileCut[(int)Main.tile[num361, num362].type] || Main.tile[num361, num362].type == 165))
												{
													WorldGen.KillTile(num361, num362, false, false, false);
													NetMessage.SendData(17, -1, -1, "", 0, (float)num361, (float)num362, 0f, 0, 0, 0);
												}
												if (!Main.tile[num361, num362].active())
												{
													WorldGen.PlaceTile(num361, num362, num360, false, false, -1, 0);
													NetMessage.SendData(17, -1, -1, "", 1, (float)num361, (float)num362, (float)num360, 0, 0, 0);
													this.ai[1] += 1f;
												}
												else
												{
													num363 = 0;
												}
												num362++;
											}
											this.Kill();
											return;
										}
									}
								}
							}
							else if (this.aiStyle == 36)
							{
								if (this.type != 307 && this.wet && !this.honeyWet)
								{
									this.Kill();
								}
								if (this.alpha > 0)
								{
									this.alpha -= 50;
								}
								else
								{
									this.extraUpdates = 0;
								}
								if (this.alpha < 0)
								{
									this.alpha = 0;
								}
								if (this.type == 307)
								{
									this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) - 1.57f;
									this.frameCounter++;
									if (this.frameCounter >= 6)
									{
										this.frame++;
										this.frameCounter = 0;
									}
									if (this.frame >= 2)
									{
										this.frame = 0;
									}
								}
								else
								{
									if (this.type == 316)
									{
										if (this.velocity.X > 0f)
										{
											this.spriteDirection = -1;
										}
										else if (this.velocity.X < 0f)
										{
											this.spriteDirection = 1;
										}
									}
									else if (this.velocity.X > 0f)
									{
										this.spriteDirection = 1;
									}
									else if (this.velocity.X < 0f)
									{
										this.spriteDirection = -1;
									}
									this.rotation = this.velocity.X * 0.1f;
									this.frameCounter++;
									if (this.frameCounter >= 3)
									{
										this.frame++;
										this.frameCounter = 0;
									}
									if (this.frame >= 3)
									{
										this.frame = 0;
									}
								}
								float num368 = this.position.X;
								float num369 = this.position.Y;
								float num370 = 100000f;
								bool flag10 = false;
								this.ai[0] += 1f;
								if (this.ai[0] > 30f)
								{
									this.ai[0] = 30f;
									for (int num371 = 0; num371 < 200; num371++)
									{
										if (Main.npc[num371].CanBeChasedBy(this, false) && (!Main.npc[num371].wet || this.type == 307))
										{
											float num372 = Main.npc[num371].position.X + (float)(Main.npc[num371].width / 2);
											float num373 = Main.npc[num371].position.Y + (float)(Main.npc[num371].height / 2);
											float num374 = Math.Abs(this.position.X + (float)(this.width / 2) - num372) + Math.Abs(this.position.Y + (float)(this.height / 2) - num373);
											if (num374 < 800f && num374 < num370 && Collision.CanHit(this.position, this.width, this.height, Main.npc[num371].position, Main.npc[num371].width, Main.npc[num371].height))
											{
												num370 = num374;
												num368 = num372;
												num369 = num373;
												flag10 = true;
											}
										}
									}
								}
								if (!flag10)
								{
									num368 = this.position.X + (float)(this.width / 2) + this.velocity.X * 100f;
									num369 = this.position.Y + (float)(this.height / 2) + this.velocity.Y * 100f;
								}
								else if (this.type == 307)
								{
									this.friendly = true;
								}
								float num375 = 6f;
								float num376 = 0.1f;
								if (this.type == 189)
								{
									num375 = 7f;
									num376 = 0.15f;
								}
								if (this.type == 307)
								{
									num375 = 9f;
									num376 = 0.2f;
								}
								if (this.type == 316)
								{
									num375 = 10f;
									num376 = 0.25f;
								}
								if (this.type == 566)
								{
									num375 = 6.8f;
									num376 = 0.14f;
								}
								Vector2 vector34 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
								float num377 = num368 - vector34.X;
								float num378 = num369 - vector34.Y;
								float num379 = (float)Math.Sqrt((double)(num377 * num377 + num378 * num378));
								num379 = num375 / num379;
								num377 *= num379;
								num378 *= num379;
								if (this.velocity.X < num377)
								{
									this.velocity.X = this.velocity.X + num376;
									if (this.velocity.X < 0f && num377 > 0f)
									{
										this.velocity.X = this.velocity.X + num376 * 2f;
									}
								}
								else if (this.velocity.X > num377)
								{
									this.velocity.X = this.velocity.X - num376;
									if (this.velocity.X > 0f && num377 < 0f)
									{
										this.velocity.X = this.velocity.X - num376 * 2f;
									}
								}
								if (this.velocity.Y < num378)
								{
									this.velocity.Y = this.velocity.Y + num376;
									if (this.velocity.Y < 0f && num378 > 0f)
									{
										this.velocity.Y = this.velocity.Y + num376 * 2f;
										return;
									}
								}
								else if (this.velocity.Y > num378)
								{
									this.velocity.Y = this.velocity.Y - num376;
									if (this.velocity.Y > 0f && num378 < 0f)
									{
										this.velocity.Y = this.velocity.Y - num376 * 2f;
										return;
									}
								}
							}
							else if (this.aiStyle == 37)
							{
								if (this.ai[1] == 0f)
								{
									this.ai[1] = this.position.Y - 5f;
									this.localAI[0] = base.Center.X - this.velocity.X * 1.5f;
									this.localAI[1] = base.Center.Y - this.velocity.Y * 1.5f;
								}
								Vector2 vector34 = new Vector2(this.localAI[0], this.localAI[1]);
								this.rotation = (base.Center - vector34).ToRotation() - 1.57079637f;
								if (this.ai[0] == 0f)
								{
									if (Collision.SolidCollision(this.position, this.width, this.height))
									{
										this.velocity.Y = this.velocity.Y * -1f;
										this.ai[0] += 1f;
										return;
									}
									float num380 = Vector2.Distance(base.Center, vector34);
									if (num380 > 300f)
									{
										this.velocity *= -1f;
										this.ai[0] += 1f;
										return;
									}
								}
								else if (Collision.SolidCollision(this.position, this.width, this.height) || Vector2.Distance(base.Center, vector34) < this.velocity.Length())
								{
									this.Kill();
									return;
								}
							}
							else if (this.aiStyle == 38)
							{
								this.ai[0] += 1f;
								if (this.ai[0] >= 6f)
								{
									this.ai[0] = 0f;
									if (Main.myPlayer == this.owner)
									{
										Projectile.NewProjectile(this.position.X, this.position.Y, this.velocity.X, this.velocity.Y, 188, this.damage, this.knockBack, this.owner, 0f, 0f);
										return;
									}
								}
							}
							else if (this.aiStyle == 39)
							{
								this.alpha -= 50;
								if (this.alpha < 0)
								{
									this.alpha = 0;
								}
								if (Main.player[this.owner].dead)
								{
									this.Kill();
									return;
								}
								if (this.alpha == 0)
								{
									Main.player[this.owner].itemAnimation = 5;
									Main.player[this.owner].itemTime = 5;
									if (this.position.X + (float)(this.width / 2) > Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2))
									{
										Main.player[this.owner].ChangeDir(1);
									}
									else
									{
										Main.player[this.owner].ChangeDir(-1);
									}
								}
								Vector2 vector35 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
								float num381 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector35.X;
								float num382 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector35.Y;
								float num383 = (float)Math.Sqrt((double)(num381 * num381 + num382 * num382));
								if (!Main.player[this.owner].channel && this.alpha == 0)
								{
									this.ai[0] = 1f;
									this.ai[1] = -1f;
								}
								if (this.ai[1] > 0f && num383 > 1500f)
								{
									this.ai[1] = 0f;
									this.ai[0] = 1f;
								}
								if (this.ai[1] > 0f)
								{
									this.tileCollide = false;
									int num384 = (int)this.ai[1] - 1;
									if (Main.npc[num384].active && Main.npc[num384].life > 0)
									{
										float num385 = 16f;
										vector35 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
										num381 = Main.npc[num384].position.X + (float)(Main.npc[num384].width / 2) - vector35.X;
										num382 = Main.npc[num384].position.Y + (float)(Main.npc[num384].height / 2) - vector35.Y;
										num383 = (float)Math.Sqrt((double)(num381 * num381 + num382 * num382));
										if (num383 < num385)
										{
											this.velocity.X = num381;
											this.velocity.Y = num382;
											if (num383 > num385 / 2f)
											{
												if (this.velocity.X < 0f)
												{
													this.spriteDirection = -1;
													this.rotation = (float)Math.Atan2((double)(-(double)this.velocity.Y), (double)(-(double)this.velocity.X));
												}
												else
												{
													this.spriteDirection = 1;
													this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X);
												}
											}
										}
										else
										{
											num383 = num385 / num383;
											num381 *= num383;
											num382 *= num383;
											this.velocity.X = num381;
											this.velocity.Y = num382;
											if (this.velocity.X < 0f)
											{
												this.spriteDirection = -1;
												this.rotation = (float)Math.Atan2((double)(-(double)this.velocity.Y), (double)(-(double)this.velocity.X));
											}
											else
											{
												this.spriteDirection = 1;
												this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X);
											}
										}
										this.ai[0] = 1f;
									}
									else
									{
										this.ai[1] = 0f;
										float num386 = this.position.X;
										float num387 = this.position.Y;
										float num388 = 3000f;
										int num389 = -1;
										for (int num390 = 0; num390 < 200; num390++)
										{
											if (Main.npc[num390].CanBeChasedBy(this, false))
											{
												float num391 = Main.npc[num390].position.X + (float)(Main.npc[num390].width / 2);
												float num392 = Main.npc[num390].position.Y + (float)(Main.npc[num390].height / 2);
												float num393 = Math.Abs(this.position.X + (float)(this.width / 2) - num391) + Math.Abs(this.position.Y + (float)(this.height / 2) - num392);
												if (num393 < num388 && Collision.CanHit(this.position, this.width, this.height, Main.npc[num390].position, Main.npc[num390].width, Main.npc[num390].height))
												{
													num388 = num393;
													num386 = num391;
													num387 = num392;
													num389 = num390;
												}
											}
										}
										if (num389 >= 0)
										{
											float num394 = 16f;
											vector35 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
											num381 = num386 - vector35.X;
											num382 = num387 - vector35.Y;
											num383 = (float)Math.Sqrt((double)(num381 * num381 + num382 * num382));
											num383 = num394 / num383;
											num381 *= num383;
											num382 *= num383;
											this.velocity.X = num381;
											this.velocity.Y = num382;
											this.ai[0] = 0f;
											this.ai[1] = (float)(num389 + 1);
										}
									}
								}
								else if (this.ai[0] == 0f)
								{
									if (num383 > 700f)
									{
										this.ai[0] = 1f;
									}
									if (this.velocity.X < 0f)
									{
										this.spriteDirection = -1;
										this.rotation = (float)Math.Atan2((double)(-(double)this.velocity.Y), (double)(-(double)this.velocity.X));
									}
									else
									{
										this.spriteDirection = 1;
										this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X);
									}
								}
								else if (this.ai[0] == 1f)
								{
									this.tileCollide = false;
									if (this.velocity.X < 0f)
									{
										this.spriteDirection = 1;
										this.rotation = (float)Math.Atan2((double)(-(double)this.velocity.Y), (double)(-(double)this.velocity.X));
									}
									else
									{
										this.spriteDirection = -1;
										this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X);
									}
									if (this.velocity.X < 0f)
									{
										this.spriteDirection = -1;
										this.rotation = (float)Math.Atan2((double)(-(double)this.velocity.Y), (double)(-(double)this.velocity.X));
									}
									else
									{
										this.spriteDirection = 1;
										this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X);
									}
									float num395 = 20f;
									if (num383 < 70f)
									{
										this.Kill();
									}
									num383 = num395 / num383;
									num381 *= num383;
									num382 *= num383;
									this.velocity.X = num381;
									this.velocity.Y = num382;
								}
								this.frameCounter++;
								if (this.frameCounter >= 4)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame >= 4)
								{
									this.frame = 0;
									return;
								}
							}
							else
							{
								if (this.aiStyle == 40)
								{
									this.localAI[0] += 1f;
									if (this.localAI[0] > 3f)
									{
										this.localAI[0] = 100f;
										this.alpha -= 50;
										if (this.alpha < 0)
										{
											this.alpha = 0;
										}
									}
									this.frameCounter++;
									if (this.frameCounter >= 3)
									{
										this.frame++;
										this.frameCounter = 0;
									}
									if (this.frame >= 5)
									{
										this.frame = 0;
									}
									this.velocity.X = this.velocity.X + this.ai[0];
									this.velocity.Y = this.velocity.Y + this.ai[1];
									this.localAI[1] += 1f;
									if (this.localAI[1] == 50f)
									{
										this.localAI[1] = 51f;
										this.ai[0] = (float)Main.rand.Next(-100, 101) * 6E-05f;
										this.ai[1] = (float)Main.rand.Next(-100, 101) * 6E-05f;
									}
									if (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y) > 16f)
									{
										this.velocity.X = this.velocity.X * 0.95f;
										this.velocity.Y = this.velocity.Y * 0.95f;
									}
									if (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y) < 12f)
									{
										this.velocity.X = this.velocity.X * 1.05f;
										this.velocity.Y = this.velocity.Y * 1.05f;
									}
									this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 3.14f;
									return;
								}
								if (this.aiStyle == 41)
								{
									if (this.localAI[0] == 0f)
									{
										this.localAI[0] = 1f;
										this.frame = Main.rand.Next(3);
									}
									this.rotation += this.velocity.X * 0.01f;
									return;
								}
								if (this.aiStyle == 42)
								{
									if (!Main.player[this.owner].crystalLeaf)
									{
										this.Kill();
										return;
									}
									this.position.X = Main.player[this.owner].Center.X - (float)(this.width / 2);
									this.position.Y = Main.player[this.owner].Center.Y - (float)(this.height / 2) + Main.player[this.owner].gfxOffY - 60f;
									if (Main.player[this.owner].gravDir == -1f)
									{
										this.position.Y = this.position.Y + 120f;
										this.rotation = 3.14f;
									}
									else
									{
										this.rotation = 0f;
									}
									this.position.X = (float)((int)this.position.X);
									this.position.Y = (float)((int)this.position.Y);
									float num396 = (float)Main.mouseTextColor / 200f - 0.35f;
									num396 *= 0.2f;
									this.scale = num396 + 0.95f;
									if (this.owner == Main.myPlayer)
									{
										if (this.ai[0] != 0f)
										{
											this.ai[0] -= 1f;
											return;
										}
										float num397 = this.position.X;
										float num398 = this.position.Y;
										float num399 = 700f;
										bool flag11 = false;
										for (int num400 = 0; num400 < 200; num400++)
										{
											if (Main.npc[num400].CanBeChasedBy(this, true))
											{
												float num401 = Main.npc[num400].position.X + (float)(Main.npc[num400].width / 2);
												float num402 = Main.npc[num400].position.Y + (float)(Main.npc[num400].height / 2);
												float num403 = Math.Abs(this.position.X + (float)(this.width / 2) - num401) + Math.Abs(this.position.Y + (float)(this.height / 2) - num402);
												if (num403 < num399 && Collision.CanHit(this.position, this.width, this.height, Main.npc[num400].position, Main.npc[num400].width, Main.npc[num400].height))
												{
													num399 = num403;
													num397 = num401;
													num398 = num402;
													flag11 = true;
												}
											}
										}
										if (flag11)
										{
											float num404 = 12f;
											Vector2 vector36 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
											float num405 = num397 - vector36.X;
											float num406 = num398 - vector36.Y;
											float num407 = (float)Math.Sqrt((double)(num405 * num405 + num406 * num406));
											num407 = num404 / num407;
											num405 *= num407;
											num406 *= num407;
											Projectile.NewProjectile(base.Center.X - 4f, base.Center.Y, num405, num406, 227, Player.crystalLeafDamage, (float)Player.crystalLeafKB, this.owner, 0f, 0f);
											this.ai[0] = 50f;
											return;
										}
									}
								}
								else
								{
									if (this.aiStyle == 43)
									{
										if (this.localAI[1] == 0f)
										{
											this.localAI[1] += 1f;
										}
										this.ai[0] = (float)Main.rand.Next(-100, 101) * 0.0025f;
										this.ai[1] = (float)Main.rand.Next(-100, 101) * 0.0025f;
										if (this.localAI[0] == 0f)
										{
											this.scale += 0.05f;
											if ((double)this.scale > 1.2)
											{
												this.localAI[0] = 1f;
											}
										}
										else
										{
											this.scale -= 0.05f;
											if ((double)this.scale < 0.8)
											{
												this.localAI[0] = 0f;
											}
										}
										this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 3.14f;
										return;
									}
									if (this.aiStyle == 44)
									{
										if (this.type == 228)
										{
											this.velocity *= 0.96f;
											this.alpha += 4;
											if (this.alpha > 255)
											{
												this.Kill();
											}
										}
										else if (this.type == 229)
										{
											this.ai[0] += 1f;
											if (this.ai[0] > 20f)
											{
												this.velocity.Y = this.velocity.Y + 0.3f;
												this.velocity.X = this.velocity.X * 0.98f;
											}
										}
										this.frameCounter++;
										if (this.frameCounter > 5)
										{
											this.frame++;
											this.frameCounter = 0;
										}
										if (this.frame >= Main.projFrames[this.type])
										{
											this.frame = 0;
											return;
										}
									}
									else if (this.aiStyle == 45)
									{
										if (this.type == 237 || this.type == 243)
										{
											float num411 = this.ai[0];
											float num412 = this.ai[1];
											if (num411 != 0f && num412 != 0f)
											{
												bool flag12 = false;
												bool flag13 = false;
												if ((this.velocity.X < 0f && base.Center.X < num411) || (this.velocity.X > 0f && base.Center.X > num411))
												{
													flag12 = true;
												}
												if ((this.velocity.Y < 0f && base.Center.Y < num412) || (this.velocity.Y > 0f && base.Center.Y > num412))
												{
													flag13 = true;
												}
												if (flag12 && flag13)
												{
													this.Kill();
												}
											}
											this.rotation += this.velocity.X * 0.02f;
											this.frameCounter++;
											if (this.frameCounter > 4)
											{
												this.frameCounter = 0;
												this.frame++;
												if (this.frame > 3)
												{
													this.frame = 0;
													return;
												}
											}
										}
										else if (this.type == 238 || this.type == 244)
										{
											this.frameCounter++;
											if (this.frameCounter > 8)
											{
												this.frameCounter = 0;
												this.frame++;
												if (this.frame > 5)
												{
													this.frame = 0;
												}
											}
											this.ai[1] += 1f;
											if (this.type == 244 && this.ai[1] >= 3600f)
											{
												this.alpha += 5;
												if (this.alpha > 255)
												{
													this.alpha = 255;
													this.Kill();
												}
											}
											else if (this.type == 238 && this.ai[1] >= 7200f)
											{
												this.alpha += 5;
												if (this.alpha > 255)
												{
													this.alpha = 255;
													this.Kill();
												}
											}
											else
											{
												this.ai[0] += 1f;
												if (this.type == 244)
												{
													if (this.ai[0] > 10f)
													{
														this.ai[0] = 0f;
														if (this.owner == Main.myPlayer)
														{
															int num413 = (int)(this.position.X + 14f + (float)Main.rand.Next(this.width - 28));
															int num414 = (int)(this.position.Y + (float)this.height + 4f);
															Projectile.NewProjectile((float)num413, (float)num414, 0f, 5f, 245, this.damage, 0f, this.owner, 0f, 0f);
														}
													}
												}
												else if (this.ai[0] > 8f)
												{
													this.ai[0] = 0f;
													if (this.owner == Main.myPlayer)
													{
														int num415 = (int)(this.position.X + 14f + (float)Main.rand.Next(this.width - 28));
														int num416 = (int)(this.position.Y + (float)this.height + 4f);
														Projectile.NewProjectile((float)num415, (float)num416, 0f, 5f, 239, this.damage, 0f, this.owner, 0f, 0f);
													}
												}
											}
											this.localAI[0] += 1f;
											if (this.localAI[0] >= 10f)
											{
												this.localAI[0] = 0f;
												int num417 = 0;
												int num418 = 0;
												float num419 = 0f;
												int num420 = this.type;
												for (int num421 = 0; num421 < 1000; num421++)
												{
													if (Main.projectile[num421].active && Main.projectile[num421].owner == this.owner && Main.projectile[num421].type == num420 && Main.projectile[num421].ai[1] < 3600f)
													{
														num417++;
														if (Main.projectile[num421].ai[1] > num419)
														{
															num418 = num421;
															num419 = Main.projectile[num421].ai[1];
														}
													}
												}
												if (this.type == 244)
												{
													if (num417 > 1)
													{
														Main.projectile[num418].netUpdate = true;
														Main.projectile[num418].ai[1] = 36000f;
														return;
													}
												}
												else if (num417 > 2)
												{
													Main.projectile[num418].netUpdate = true;
													Main.projectile[num418].ai[1] = 36000f;
													return;
												}
											}
										}
										else
										{
											if (this.type == 239)
											{
												this.alpha = 50;
												return;
											}
											if (this.type == 245)
											{
												this.alpha = 100;
												return;
											}
											if (this.type == 264)
											{
												this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
												return;
											}
										}
									}
									else if (this.aiStyle == 46)
									{
										int num422 = 1200;
										if (this.type == 250)
										{
											if (this.owner == Main.myPlayer)
											{
												this.localAI[0] += 1f;
												if (this.localAI[0] > 4f)
												{
													this.localAI[0] = 3f;
													Projectile.NewProjectile(base.Center.X, base.Center.Y, this.velocity.X * 0.001f, this.velocity.Y * 0.001f, 251, this.damage, this.knockBack, this.owner, 0f, 0f);
												}
												if (this.timeLeft > num422)
												{
													this.timeLeft = num422;
												}
											}
											float num423 = 1f;
											if (this.velocity.Y < 0f)
											{
												num423 -= this.velocity.Y / 3f;
											}
											this.ai[0] += num423;
											if (this.ai[0] > 30f)
											{
												this.velocity.Y = this.velocity.Y + 0.5f;
												if (this.velocity.Y > 0f)
												{
													this.velocity.X = this.velocity.X * 0.95f;
												}
												else
												{
													this.velocity.X = this.velocity.X * 1.05f;
												}
											}
											float num424 = this.velocity.X;
											float num425 = this.velocity.Y;
											float num426 = (float)Math.Sqrt((double)(num424 * num424 + num425 * num425));
											num426 = 15.95f * this.scale / num426;
											num424 *= num426;
											num425 *= num426;
											this.velocity.X = num424;
											this.velocity.Y = num425;
											this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) - 1.57f;
											return;
										}
										if (this.localAI[0] == 0f)
										{
											if (this.velocity.X > 0f)
											{
												this.spriteDirection = -1;
												this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) - 1.57f;
											}
											else
											{
												this.spriteDirection = 1;
												this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) - 1.57f;
											}
											this.localAI[0] = 1f;
											this.timeLeft = num422;
										}
										this.velocity.X = this.velocity.X * 0.98f;
										this.velocity.Y = this.velocity.Y * 0.98f;
										if (this.rotation == 0f)
										{
											this.alpha = 255;
											return;
										}
										if (this.timeLeft < 10)
										{
											this.alpha = 255 - (int)(255f * (float)this.timeLeft / 10f);
											return;
										}
										if (this.timeLeft > num422 - 10)
										{
											int num427 = num422 - this.timeLeft;
											this.alpha = 255 - (int)(255f * (float)num427 / 10f);
											return;
										}
										this.alpha = 0;
										return;
									}
									else if (this.aiStyle == 47)
									{
										if (this.ai[0] == 0f)
										{
											this.ai[0] = this.velocity.X;
											this.ai[1] = this.velocity.Y;
										}
										if (this.velocity.X > 0f)
										{
											this.rotation += (Math.Abs(this.velocity.Y) + Math.Abs(this.velocity.X)) * 0.001f;
										}
										else
										{
											this.rotation -= (Math.Abs(this.velocity.Y) + Math.Abs(this.velocity.X)) * 0.001f;
										}
										this.frameCounter++;
										if (this.frameCounter > 6)
										{
											this.frameCounter = 0;
											this.frame++;
											if (this.frame > 4)
											{
												this.frame = 0;
											}
										}
										if (Math.Sqrt((double)(this.velocity.X * this.velocity.X + this.velocity.Y * this.velocity.Y)) > 2.0)
										{
											this.velocity *= 0.98f;
										}
										for (int num428 = 0; num428 < 1000; num428++)
										{
											if (num428 != this.whoAmI && Main.projectile[num428].active && Main.projectile[num428].owner == this.owner && Main.projectile[num428].type == this.type && this.timeLeft > Main.projectile[num428].timeLeft && Main.projectile[num428].timeLeft > 30)
											{
												Main.projectile[num428].timeLeft = 30;
											}
										}
										int[] array = new int[20];
										int num429 = 0;
										float num430 = 300f;
										bool flag14 = false;
										for (int num431 = 0; num431 < 200; num431++)
										{
											if (Main.npc[num431].CanBeChasedBy(this, false))
											{
												float num432 = Main.npc[num431].position.X + (float)(Main.npc[num431].width / 2);
												float num433 = Main.npc[num431].position.Y + (float)(Main.npc[num431].height / 2);
												float num434 = Math.Abs(this.position.X + (float)(this.width / 2) - num432) + Math.Abs(this.position.Y + (float)(this.height / 2) - num433);
												if (num434 < num430 && Collision.CanHit(base.Center, 1, 1, Main.npc[num431].Center, 1, 1))
												{
													if (num429 < 20)
													{
														array[num429] = num431;
														num429++;
													}
													flag14 = true;
												}
											}
										}
										if (this.timeLeft < 30)
										{
											flag14 = false;
										}
										if (flag14)
										{
											int num435 = Main.rand.Next(num429);
											num435 = array[num435];
											float num436 = Main.npc[num435].position.X + (float)(Main.npc[num435].width / 2);
											float num437 = Main.npc[num435].position.Y + (float)(Main.npc[num435].height / 2);
											this.localAI[0] += 1f;
											if (this.localAI[0] > 8f)
											{
												this.localAI[0] = 0f;
												float num438 = 6f;
												Vector2 vector37 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
												vector37 += this.velocity * 4f;
												float num439 = num436 - vector37.X;
												float num440 = num437 - vector37.Y;
												float num441 = (float)Math.Sqrt((double)(num439 * num439 + num440 * num440));
												num441 = num438 / num441;
												num439 *= num441;
												num440 *= num441;
												Projectile.NewProjectile(vector37.X, vector37.Y, num439, num440, 255, this.damage, this.knockBack, this.owner, 0f, 0f);
												return;
											}
										}
									}
									else if (this.aiStyle == 48)
									{
										if (this.type == 255)
										{
											this.alpha = 255;
											return;
										}
										if (this.type == 433)
										{
											this.alpha = 255;
											return;
										}
										if (this.type == 290)
										{
											this.localAI[0] += 1f;
											if (this.localAI[0] > 3f)
											{
												this.alpha = 255;
												return;
											}
										}
										else if (this.type == 294)
										{
											this.localAI[0] += 1f;
											if (this.localAI[0] > 9f)
											{
												this.alpha = 255;
												return;
											}
										}
										else
										{
											this.localAI[0] += 1f;
											if (this.localAI[0] > 3f)
											{
												this.alpha = 255;
												return;
											}
										}
									}
									else if (this.aiStyle == 49)
									{
										if (this.ai[1] == 0f)
										{
											this.ai[1] = 1f;
										}
										if (this.ai[1] == 1f)
										{
											if (this.velocity.X > 0f)
											{
												this.direction = 1;
											}
											else if (this.velocity.X < 0f)
											{
												this.direction = -1;
											}
											this.spriteDirection = this.direction;
											this.ai[0] += 1f;
											this.rotation += this.velocity.X * 0.05f + (float)this.direction * 0.05f;
											if (this.ai[0] >= 18f)
											{
												this.velocity.Y = this.velocity.Y + 0.28f;
												this.velocity.X = this.velocity.X * 0.99f;
											}
											if ((double)this.velocity.Y > 15.9)
											{
												this.velocity.Y = 15.9f;
											}
											if (this.ai[0] > 2f)
											{
												this.alpha = 0;
												if (this.ai[0] == 3f)
												{
													return;
												}
											}
										}
										else if (this.ai[1] == 2f)
										{
											this.rotation = 0f;
											this.velocity.X = this.velocity.X * 0.95f;
											this.velocity.Y = this.velocity.Y + 0.2f;
											return;
										}
									}
									else if (this.aiStyle == 50)
									{
										if (this.type == 291)
										{
											if (this.localAI[0] == 0f)
											{
												this.localAI[0] += 1f;
											}
											bool flag15 = false;
											bool flag16 = false;
											if (this.velocity.X < 0f && this.position.X < this.ai[0])
											{
												flag15 = true;
											}
											if (this.velocity.X > 0f && this.position.X > this.ai[0])
											{
												flag15 = true;
											}
											if (this.velocity.Y < 0f && this.position.Y < this.ai[1])
											{
												flag16 = true;
											}
											if (this.velocity.Y > 0f && this.position.Y > this.ai[1])
											{
												flag16 = true;
											}
											if (flag15 && flag16)
											{
												this.Kill();
											}
											return;
										}
										if (this.type == 295)
										{
											return;
										}
										if (this.localAI[0] == 0f)
										{
											this.localAI[0] += 1f;
										}
										this.ai[0] += 1f;
										if (this.type == 296)
										{
											this.ai[0] += 3f;
										}
										float num462 = 25f;
										if (this.ai[0] > 180f)
										{
											num462 -= (this.ai[0] - 180f) / 2f;
										}
										if (num462 <= 0f)
										{
											num462 = 0f;
											this.Kill();
										}
										if (this.type == 296)
										{
											num462 *= 0.7f;
										}
										return;
									}
									else if (this.aiStyle == 51)
									{
										if (this.type == 297)
										{
											this.localAI[0] += 1f;
										}
										else
										{
											if (this.localAI[0] == 0f)
											{
												this.localAI[0] += 1f;
											}
										}
										float num473 = base.Center.X;
										float num474 = base.Center.Y;
										float num475 = 400f;
										bool flag17 = false;
										if (this.type == 297)
										{
											for (int num476 = 0; num476 < 200; num476++)
											{
												if (Main.npc[num476].CanBeChasedBy(this, false) && Collision.CanHit(base.Center, 1, 1, Main.npc[num476].Center, 1, 1))
												{
													float num477 = Main.npc[num476].position.X + (float)(Main.npc[num476].width / 2);
													float num478 = Main.npc[num476].position.Y + (float)(Main.npc[num476].height / 2);
													float num479 = Math.Abs(this.position.X + (float)(this.width / 2) - num477) + Math.Abs(this.position.Y + (float)(this.height / 2) - num478);
													if (num479 < num475)
													{
														num475 = num479;
														num473 = num477;
														num474 = num478;
														flag17 = true;
													}
												}
											}
										}
										else
										{
											num475 = 200f;
											for (int num480 = 0; num480 < 255; num480++)
											{
												if (Main.player[num480].active && !Main.player[num480].dead)
												{
													float num481 = Main.player[num480].position.X + (float)(Main.player[num480].width / 2);
													float num482 = Main.player[num480].position.Y + (float)(Main.player[num480].height / 2);
													float num483 = Math.Abs(this.position.X + (float)(this.width / 2) - num481) + Math.Abs(this.position.Y + (float)(this.height / 2) - num482);
													if (num483 < num475)
													{
														num475 = num483;
														num473 = num481;
														num474 = num482;
														flag17 = true;
													}
												}
											}
										}
										if (flag17)
										{
											float num484 = 3f;
											if (this.type == 297)
											{
												num484 = 6f;
											}
											Vector2 vector43 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
											float num485 = num473 - vector43.X;
											float num486 = num474 - vector43.Y;
											float num487 = (float)Math.Sqrt((double)(num485 * num485 + num486 * num486));
											num487 = num484 / num487;
											num485 *= num487;
											num486 *= num487;
											if (this.type == 297)
											{
												this.velocity.X = (this.velocity.X * 20f + num485) / 21f;
												this.velocity.Y = (this.velocity.Y * 20f + num486) / 21f;
												return;
											}
											this.velocity.X = (this.velocity.X * 100f + num485) / 101f;
											this.velocity.Y = (this.velocity.Y * 100f + num486) / 101f;
											return;
										}
									}
									else if (this.aiStyle == 52)
									{
										int num488 = (int)this.ai[0];
										float num489 = 4f;
										Vector2 vector44 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
										float num490 = Main.player[num488].Center.X - vector44.X;
										float num491 = Main.player[num488].Center.Y - vector44.Y;
										float num492 = (float)Math.Sqrt((double)(num490 * num490 + num491 * num491));
										if (num492 < 50f && this.position.X < Main.player[num488].position.X + (float)Main.player[num488].width && this.position.X + (float)this.width > Main.player[num488].position.X && this.position.Y < Main.player[num488].position.Y + (float)Main.player[num488].height && this.position.Y + (float)this.height > Main.player[num488].position.Y)
										{
											if (this.owner == Main.myPlayer)
											{
												int num493 = (int)this.ai[1];
												Main.player[num488].HealEffect(num493, false);
												Main.player[num488].statLife += num493;
												if (Main.player[num488].statLife > Main.player[num488].statLifeMax2)
												{
													Main.player[num488].statLife = Main.player[num488].statLifeMax2;
												}
												NetMessage.SendData(66, -1, -1, "", num488, (float)num493, 0f, 0f, 0, 0, 0);
											}
											this.Kill();
										}
										num492 = num489 / num492;
										num490 *= num492;
										num491 *= num492;
										this.velocity.X = (this.velocity.X * 15f + num490) / 16f;
										this.velocity.Y = (this.velocity.Y * 15f + num491) / 16f;
										if (this.type == 305)
										{
											return;
										}
										return;
									}
									else if (this.aiStyle == 53)
									{
										if (this.localAI[0] == 0f)
										{
											this.localAI[1] = 1f;
											this.localAI[0] = 1f;
											this.ai[0] = 120f;
											if (this.type == 377)
											{
												this.frame = 4;
											}
										}
										this.velocity.X = 0f;
										this.velocity.Y = this.velocity.Y + 0.2f;
										if (this.velocity.Y > 16f)
										{
											this.velocity.Y = 16f;
										}
										bool flag18 = false;
										float num507 = base.Center.X;
										float num508 = base.Center.Y;
										float num509 = 1000f;
										for (int num510 = 0; num510 < 200; num510++)
										{
											if (Main.npc[num510].CanBeChasedBy(this, false))
											{
												float num511 = Main.npc[num510].position.X + (float)(Main.npc[num510].width / 2);
												float num512 = Main.npc[num510].position.Y + (float)(Main.npc[num510].height / 2);
												float num513 = Math.Abs(this.position.X + (float)(this.width / 2) - num511) + Math.Abs(this.position.Y + (float)(this.height / 2) - num512);
												if (num513 < num509 && Collision.CanHit(this.position, this.width, this.height, Main.npc[num510].position, Main.npc[num510].width, Main.npc[num510].height))
												{
													num509 = num513;
													num507 = num511;
													num508 = num512;
													flag18 = true;
												}
											}
										}
										if (flag18)
										{
											float num514 = num507;
											float num515 = num508;
											num507 -= base.Center.X;
											num508 -= base.Center.Y;
											if (num507 < 0f)
											{
												this.spriteDirection = -1;
											}
											else
											{
												this.spriteDirection = 1;
											}
											int num516;
											if (num508 > 0f)
											{
												num516 = 0;
											}
											else if (Math.Abs(num508) > Math.Abs(num507) * 3f)
											{
												num516 = 4;
											}
											else if (Math.Abs(num508) > Math.Abs(num507) * 2f)
											{
												num516 = 3;
											}
											else if (Math.Abs(num507) > Math.Abs(num508) * 3f)
											{
												num516 = 0;
											}
											else if (Math.Abs(num507) > Math.Abs(num508) * 2f)
											{
												num516 = 1;
											}
											else
											{
												num516 = 2;
											}
											if (this.type == 308)
											{
												this.frame = num516 * 2;
											}
											else if (this.type == 377)
											{
												this.frame = num516;
											}
											if (this.ai[0] > 40f && this.localAI[1] == 0f && this.type == 308)
											{
												this.frame++;
											}
											if (this.ai[0] <= 0f)
											{
												this.localAI[1] = 0f;
												this.ai[0] = 60f;
												if (Main.myPlayer == this.owner)
												{
													float num517 = 6f;
													int num518 = 309;
													if (this.type == 377)
													{
														num518 = 378;
														num517 = 9f;
													}
													Vector2 vector45 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
													if (num516 == 0)
													{
														vector45.Y += 12f;
														vector45.X += (float)(24 * this.spriteDirection);
													}
													else if (num516 == 1)
													{
														vector45.Y += 0f;
														vector45.X += (float)(24 * this.spriteDirection);
													}
													else if (num516 == 2)
													{
														vector45.Y -= 2f;
														vector45.X += (float)(24 * this.spriteDirection);
													}
													else if (num516 == 3)
													{
														vector45.Y -= 6f;
														vector45.X += (float)(14 * this.spriteDirection);
													}
													else if (num516 == 4)
													{
														vector45.Y -= 14f;
														vector45.X += (float)(2 * this.spriteDirection);
													}
													if (this.spriteDirection < 0)
													{
														vector45.X += 10f;
													}
													float num519 = num514 - vector45.X;
													float num520 = num515 - vector45.Y;
													float num521 = (float)Math.Sqrt((double)(num519 * num519 + num520 * num520));
													num521 = num517 / num521;
													num519 *= num521;
													num520 *= num521;
													int num522 = this.damage;
													Projectile.NewProjectile(vector45.X, vector45.Y, num519, num520, num518, num522, this.knockBack, Main.myPlayer, 0f, 0f);
												}
											}
										}
										else if (this.ai[0] <= 60f && (this.frame == 1 || this.frame == 3 || this.frame == 5 || this.frame == 7 || this.frame == 9))
										{
											this.frame--;
										}
										if (this.ai[0] > 0f)
										{
											this.ai[0] -= 1f;
											return;
										}
									}
									else if (this.aiStyle == 54)
									{
										if (this.type == 317)
										{
											if (Main.player[Main.myPlayer].dead)
											{
												Main.player[Main.myPlayer].raven = false;
											}
											if (Main.player[Main.myPlayer].raven)
											{
												this.timeLeft = 2;
											}
										}
										for (int num523 = 0; num523 < 1000; num523++)
										{
											if (num523 != this.whoAmI && Main.projectile[num523].active && Main.projectile[num523].owner == this.owner && Main.projectile[num523].type == this.type && Math.Abs(this.position.X - Main.projectile[num523].position.X) + Math.Abs(this.position.Y - Main.projectile[num523].position.Y) < (float)this.width)
											{
												if (this.position.X < Main.projectile[num523].position.X)
												{
													this.velocity.X = this.velocity.X - 0.05f;
												}
												else
												{
													this.velocity.X = this.velocity.X + 0.05f;
												}
												if (this.position.Y < Main.projectile[num523].position.Y)
												{
													this.velocity.Y = this.velocity.Y - 0.05f;
												}
												else
												{
													this.velocity.Y = this.velocity.Y + 0.05f;
												}
											}
										}
										float num524 = this.position.X;
										float num525 = this.position.Y;
										float num526 = 900f;
										bool flag19 = false;
										int num527 = 500;
										if (this.ai[1] != 0f || this.friendly)
										{
											num527 = 1400;
										}
										if (Math.Abs(base.Center.X - Main.player[this.owner].Center.X) + Math.Abs(base.Center.Y - Main.player[this.owner].Center.Y) > (float)num527)
										{
											this.ai[0] = 1f;
										}
										if (this.ai[0] == 0f)
										{
											this.tileCollide = true;
											for (int num528 = 0; num528 < 200; num528++)
											{
												if (Main.npc[num528].CanBeChasedBy(this, false))
												{
													float num529 = Main.npc[num528].position.X + (float)(Main.npc[num528].width / 2);
													float num530 = Main.npc[num528].position.Y + (float)(Main.npc[num528].height / 2);
													float num531 = Math.Abs(this.position.X + (float)(this.width / 2) - num529) + Math.Abs(this.position.Y + (float)(this.height / 2) - num530);
													if (num531 < num526 && Collision.CanHit(this.position, this.width, this.height, Main.npc[num528].position, Main.npc[num528].width, Main.npc[num528].height))
													{
														num526 = num531;
														num524 = num529;
														num525 = num530;
														flag19 = true;
													}
												}
											}
										}
										else
										{
											this.tileCollide = false;
										}
										if (!flag19)
										{
											this.friendly = true;
											float num532 = 8f;
											if (this.ai[0] == 1f)
											{
												num532 = 12f;
											}
											Vector2 vector46 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
											float num533 = Main.player[this.owner].Center.X - vector46.X;
											float num534 = Main.player[this.owner].Center.Y - vector46.Y - 60f;
											float num535 = (float)Math.Sqrt((double)(num533 * num533 + num534 * num534));
											if (num535 < 100f && this.ai[0] == 1f && !Collision.SolidCollision(this.position, this.width, this.height))
											{
												this.ai[0] = 0f;
											}
											if (num535 > 2000f)
											{
												this.position.X = Main.player[this.owner].Center.X - (float)(this.width / 2);
												this.position.Y = Main.player[this.owner].Center.Y - (float)(this.width / 2);
											}
											if (num535 > 70f)
											{
												num535 = num532 / num535;
												num533 *= num535;
												num534 *= num535;
												this.velocity.X = (this.velocity.X * 20f + num533) / 21f;
												this.velocity.Y = (this.velocity.Y * 20f + num534) / 21f;
											}
											else
											{
												if (this.velocity.X == 0f && this.velocity.Y == 0f)
												{
													this.velocity.X = -0.15f;
													this.velocity.Y = -0.05f;
												}
												this.velocity *= 1.01f;
											}
											this.friendly = false;
											this.rotation = this.velocity.X * 0.05f;
											this.frameCounter++;
											if (this.frameCounter >= 4)
											{
												this.frameCounter = 0;
												this.frame++;
											}
											if (this.frame > 3)
											{
												this.frame = 0;
											}
											if ((double)Math.Abs(this.velocity.X) > 0.2)
											{
												this.spriteDirection = -this.direction;
												return;
											}
										}
										else
										{
											if (this.ai[1] == -1f)
											{
												this.ai[1] = 17f;
											}
											if (this.ai[1] > 0f)
											{
												this.ai[1] -= 1f;
											}
											if (this.ai[1] == 0f)
											{
												this.friendly = true;
												float num536 = 8f;
												Vector2 vector47 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
												float num537 = num524 - vector47.X;
												float num538 = num525 - vector47.Y;
												float num539 = (float)Math.Sqrt((double)(num537 * num537 + num538 * num538));
												if (num539 < 100f)
												{
													num536 = 10f;
												}
												num539 = num536 / num539;
												num537 *= num539;
												num538 *= num539;
												this.velocity.X = (this.velocity.X * 14f + num537) / 15f;
												this.velocity.Y = (this.velocity.Y * 14f + num538) / 15f;
											}
											else
											{
												this.friendly = false;
												if (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y) < 10f)
												{
													this.velocity *= 1.05f;
												}
											}
											this.rotation = this.velocity.X * 0.05f;
											this.frameCounter++;
											if (this.frameCounter >= 4)
											{
												this.frameCounter = 0;
												this.frame++;
											}
											if (this.frame < 4)
											{
												this.frame = 4;
											}
											if (this.frame > 7)
											{
												this.frame = 4;
											}
											if ((double)Math.Abs(this.velocity.X) > 0.2)
											{
												this.spriteDirection = -this.direction;
												return;
											}
										}
									}
									else if (this.aiStyle == 55)
									{
										this.frameCounter++;
										if (this.frameCounter > 0)
										{
											this.frame++;
											this.frameCounter = 0;
											if (this.frame > 2)
											{
												this.frame = 0;
											}
										}
										if (this.velocity.X < 0f)
										{
											this.spriteDirection = -1;
											this.rotation = (float)Math.Atan2((double)(-(double)this.velocity.Y), (double)(-(double)this.velocity.X));
										}
										else
										{
											this.spriteDirection = 1;
											this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X);
										}
										if (this.ai[0] >= 0f && this.ai[0] < 200f)
										{
											int num540 = (int)this.ai[0];
											if (Main.npc[num540].active)
											{
												float num541 = 8f;
												Vector2 vector48 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
												float num542 = Main.npc[num540].position.X - vector48.X;
												float num543 = Main.npc[num540].position.Y - vector48.Y;
												float num544 = (float)Math.Sqrt((double)(num542 * num542 + num543 * num543));
												num544 = num541 / num544;
												num542 *= num544;
												num543 *= num544;
												this.velocity.X = (this.velocity.X * 14f + num542) / 15f;
												this.velocity.Y = (this.velocity.Y * 14f + num543) / 15f;
											}
											else
											{
												float num545 = 1000f;
												for (int num546 = 0; num546 < 200; num546++)
												{
													if (Main.npc[num546].CanBeChasedBy(this, false))
													{
														float num547 = Main.npc[num546].position.X + (float)(Main.npc[num546].width / 2);
														float num548 = Main.npc[num546].position.Y + (float)(Main.npc[num546].height / 2);
														float num549 = Math.Abs(this.position.X + (float)(this.width / 2) - num547) + Math.Abs(this.position.Y + (float)(this.height / 2) - num548);
														if (num549 < num545 && Collision.CanHit(this.position, this.width, this.height, Main.npc[num546].position, Main.npc[num546].width, Main.npc[num546].height))
														{
															num545 = num549;
															this.ai[0] = (float)num546;
														}
													}
												}
											}
											return;
										}
										this.Kill();
										return;
									}
									else
									{
										if (this.aiStyle == 56)
										{
											if (this.localAI[0] == 0f)
											{
												this.localAI[0] = 1f;
												this.rotation = this.ai[0];
												this.spriteDirection = -(int)this.ai[1];
											}
											if (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y) < 16f)
											{
												this.velocity *= 1.05f;
											}
											if (this.velocity.X < 0f)
											{
												this.direction = -1;
											}
											else
											{
												this.direction = 1;
											}
											this.rotation += (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.025f * (float)this.direction;
											return;
										}
										if (this.aiStyle == 57)
										{
											this.ai[0] += 1f;
											if (this.ai[0] > 30f)
											{
												this.ai[0] = 30f;
												this.velocity.Y = this.velocity.Y + 0.25f;
												if (this.velocity.Y > 16f)
												{
													this.velocity.Y = 16f;
												}
												this.velocity.X = this.velocity.X * 0.995f;
											}
											this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
											this.alpha -= 50;
											if (this.alpha < 0)
											{
												this.alpha = 0;
											}
											if (this.owner == Main.myPlayer)
											{
												this.localAI[0] += 1f;
												if (this.localAI[0] >= 4f)
												{
													this.localAI[0] = 0f;
													int num552 = 0;
													for (int num553 = 0; num553 < 1000; num553++)
													{
														if (Main.projectile[num553].active && Main.projectile[num553].owner == this.owner && Main.projectile[num553].type == 344)
														{
															num552++;
														}
													}
													float num554 = (float)this.damage * 0.8f;
													if (num552 > 100)
													{
														float num555 = (float)(num552 - 100);
														num555 = 1f - num555 / 100f;
														num554 *= num555;
													}
													if (num552 > 100)
													{
														this.localAI[0] -= 1f;
													}
													if (num552 > 120)
													{
														this.localAI[0] -= 1f;
													}
													if (num552 > 140)
													{
														this.localAI[0] -= 1f;
													}
													if (num552 > 150)
													{
														this.localAI[0] -= 1f;
													}
													if (num552 > 160)
													{
														this.localAI[0] -= 1f;
													}
													if (num552 > 165)
													{
														this.localAI[0] -= 1f;
													}
													if (num552 > 170)
													{
														this.localAI[0] -= 2f;
													}
													if (num552 > 175)
													{
														this.localAI[0] -= 3f;
													}
													if (num552 > 180)
													{
														this.localAI[0] -= 4f;
													}
													if (num552 > 185)
													{
														this.localAI[0] -= 5f;
													}
													if (num552 > 190)
													{
														this.localAI[0] -= 6f;
													}
													if (num552 > 195)
													{
														this.localAI[0] -= 7f;
													}
													if (num554 > (float)this.damage * 0.1f)
													{
														Projectile.NewProjectile(base.Center.X, base.Center.Y, 0f, 0f, 344, (int)num554, this.knockBack * 0.55f, this.owner, 0f, (float)Main.rand.Next(3));
														return;
													}
												}
											}
										}
										else
										{
											if (this.aiStyle == 58)
											{
												this.alpha -= 50;
												if (this.alpha < 0)
												{
													this.alpha = 0;
												}
												if (this.ai[0] == 0f)
												{
													this.frame = 0;
													this.ai[1] += 1f;
													if (this.ai[1] > 30f)
													{
														this.velocity.Y = this.velocity.Y + 0.1f;
													}
													if (this.velocity.Y >= 0f)
													{
														this.ai[0] = 1f;
													}
												}
												if (this.ai[0] == 1f)
												{
													this.frame = 1;
													this.velocity.Y = this.velocity.Y + 0.1f;
													if (this.velocity.Y > 3f)
													{
														this.velocity.Y = 3f;
													}
													this.velocity.X = this.velocity.X * 0.99f;
												}
												this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
												return;
											}
											if (this.aiStyle == 59)
											{
												this.ai[1] += 1f;
												if (this.ai[1] >= 60f)
												{
													this.friendly = true;
													int num556 = (int)this.ai[0];
													if (!Main.npc[num556].active)
													{
														int[] array2 = new int[200];
														int num557 = 0;
														for (int num558 = 0; num558 < 200; num558++)
														{
															if (Main.npc[num558].CanBeChasedBy(this, false))
															{
																float num559 = Math.Abs(Main.npc[num558].position.X + (float)(Main.npc[num558].width / 2) - this.position.X + (float)(this.width / 2)) + Math.Abs(Main.npc[num558].position.Y + (float)(Main.npc[num558].height / 2) - this.position.Y + (float)(this.height / 2));
																if (num559 < 800f)
																{
																	array2[num557] = num558;
																	num557++;
																}
															}
														}
														if (num557 == 0)
														{
															this.Kill();
															return;
														}
														num556 = array2[Main.rand.Next(num557)];
														this.ai[0] = (float)num556;
													}
													float num560 = 4f;
													Vector2 vector49 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
													float num561 = Main.npc[num556].Center.X - vector49.X;
													float num562 = Main.npc[num556].Center.Y - vector49.Y;
													float num563 = (float)Math.Sqrt((double)(num561 * num561 + num562 * num562));
													num563 = num560 / num563;
													num561 *= num563;
													num562 *= num563;
													int num564 = 30;
													this.velocity.X = (this.velocity.X * (float)(num564 - 1) + num561) / (float)num564;
													this.velocity.Y = (this.velocity.Y * (float)(num564 - 1) + num562) / (float)num564;
												}
												return;
											}
											if (this.aiStyle == 60)
											{
												this.scale -= 0.015f;
												if (this.scale <= 0f)
												{
													this.velocity *= 5f;
													this.oldVelocity = this.velocity;
													this.Kill();
												}
												if (this.ai[0] <= 3f)
												{
													this.ai[0] += 1f;
													return;
												}
												int num569 = 103;
												if (this.type == 406)
												{
													num569 = 137;
												}
												if (this.owner == Main.myPlayer)
												{
													Rectangle rectangle7 = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
													for (int num570 = 0; num570 < 200; num570++)
													{
														if (Main.npc[num570].active && !Main.npc[num570].dontTakeDamage && Main.npc[num570].lifeMax > 1)
														{
															Rectangle rectangle8 = new Rectangle((int)Main.npc[num570].position.X, (int)Main.npc[num570].position.Y, Main.npc[num570].width, Main.npc[num570].height);
															if (rectangle7.Intersects(rectangle8))
															{
																Main.npc[num570].AddBuff(num569, 1500, false);
																this.Kill();
															}
														}
													}
													for (int num571 = 0; num571 < 255; num571++)
													{
														if (num571 != this.owner && Main.player[num571].active && !Main.player[num571].dead)
														{
															Rectangle rectangle9 = new Rectangle((int)Main.player[num571].position.X, (int)Main.player[num571].position.Y, Main.player[num571].width, Main.player[num571].height);
															if (rectangle7.Intersects(rectangle9))
															{
																Main.player[num571].AddBuff(num569, 1500, false);
																this.Kill();
															}
														}
													}
												}
												this.ai[0] += this.ai[1];
												if (this.ai[0] > 30f)
												{
													this.velocity.Y = this.velocity.Y + 0.1f;
												}
											}
											else if (this.aiStyle == 61)
											{
												this.timeLeft = 60;
												if (Main.player[this.owner].inventory[Main.player[this.owner].selectedItem].fishingPole == 0 || Main.player[this.owner].CCed || Main.player[this.owner].noItems)
												{
													this.Kill();
												}
												else if (Main.player[this.owner].inventory[Main.player[this.owner].selectedItem].shoot != this.type)
												{
													this.Kill();
												}
												else if (Main.player[this.owner].pulley)
												{
													this.Kill();
												}
												else if (Main.player[this.owner].dead)
												{
													this.Kill();
												}
												if (this.ai[1] > 0f && this.localAI[1] >= 0f)
												{
													this.localAI[1] = -1f;
												}
												if (this.ai[0] >= 1f)
												{
													if (this.ai[0] == 2f)
													{
														this.ai[0] += 1f;
													}
													if (this.localAI[0] < 100f)
													{
														this.localAI[0] += 1f;
													}
													this.tileCollide = false;
													float num590 = 15.9f;
													int num591 = 10;
													Vector2 vector51 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
													float num592 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector51.X;
													float num593 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector51.Y;
													float num594 = (float)Math.Sqrt((double)(num592 * num592 + num593 * num593));
													if (num594 > 3000f)
													{
														this.Kill();
													}
													num594 = num590 / num594;
													num592 *= num594;
													num593 *= num594;
													this.velocity.X = (this.velocity.X * (float)(num591 - 1) + num592) / (float)num591;
													this.velocity.Y = (this.velocity.Y * (float)(num591 - 1) + num593) / (float)num591;
													if (Main.myPlayer == this.owner)
													{
														Rectangle rectangle10 = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
														Rectangle rectangle11 = new Rectangle((int)Main.player[this.owner].position.X, (int)Main.player[this.owner].position.Y, Main.player[this.owner].width, Main.player[this.owner].height);
														if (rectangle10.Intersects(rectangle11))
														{
															if (this.ai[1] > 0f && this.ai[1] < 3602f)
															{
																int num595 = (int)this.ai[1];
																Item item = new Item();
																item.SetDefaults(num595, false);
																if (num595 == 3196)
																{
																	int num596 = Main.player[this.owner].FishingLevel();
																	int minValue = (num596 / 20 + 3) / 2;
																	int num597 = (num596 / 10 + 6) / 2;
																	if (Main.rand.Next(50) < num596)
																	{
																		num597++;
																	}
																	if (Main.rand.Next(100) < num596)
																	{
																		num597++;
																	}
																	if (Main.rand.Next(150) < num596)
																	{
																		num597++;
																	}
																	if (Main.rand.Next(200) < num596)
																	{
																		num597++;
																	}
																	int stack = Main.rand.Next(minValue, num597 + 1);
																	item.stack = stack;
																}
																if (num595 == 3197)
																{
																	int num598 = Main.player[this.owner].FishingLevel();
																	int minValue2 = (num598 / 4 + 15) / 2;
																	int num599 = (num598 / 2 + 30) / 2;
																	if (Main.rand.Next(50) < num598)
																	{
																		num599 += 4;
																	}
																	if (Main.rand.Next(100) < num598)
																	{
																		num599 += 4;
																	}
																	if (Main.rand.Next(150) < num598)
																	{
																		num599 += 4;
																	}
																	if (Main.rand.Next(200) < num598)
																	{
																		num599 += 4;
																	}
																	int stack2 = Main.rand.Next(minValue2, num599 + 1);
																	item.stack = stack2;
																}
																Item item2 = Main.player[this.owner].GetItem(this.owner, item, false, false);
																if (item2.stack > 0)
																{
																	int number2 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, num595, 1, false, 0, true);
																	if (Main.netMode == 1)
																	{
																		NetMessage.SendData(21, -1, -1, "", number2, 1f, 0f, 0f, 0, 0, 0);
																	}
																}
																else
																{
																	item.position.X = base.Center.X - (float)(item.width / 2);
																	item.position.Y = base.Center.Y - (float)(item.height / 2);
																	item.active = true;
																	ItemText.NewText(item, 0, false, false);
																}
															}
															this.Kill();
														}
													}
													this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
													return;
												}
												bool flag20 = false;
												Vector2 vector52 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
												float num600 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector52.X;
												float num601 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector52.Y;
												this.rotation = (float)Math.Atan2((double)num601, (double)num600) + 1.57f;
												float num602 = (float)Math.Sqrt((double)(num600 * num600 + num601 * num601));
												if (num602 > 900f)
												{
													this.ai[0] = 1f;
												}
												if (this.wet)
												{
													this.rotation = 0f;
													this.velocity.X = this.velocity.X * 0.9f;
													int num603 = (int)(base.Center.X + (float)((this.width / 2 + 8) * this.direction)) / 16;
													int num604 = (int)(base.Center.Y / 16f);
													float arg_18826_0 = this.position.Y / 16f;
													int num605 = (int)((this.position.Y + (float)this.height) / 16f);
													if (Main.tile[num603, num604] == null)
													{
														Main.tile[num603, num604] = new Tile();
													}
													if (Main.tile[num603, num605] == null)
													{
														Main.tile[num603, num605] = new Tile();
													}
													if (this.velocity.Y > 0f)
													{
														this.velocity.Y = this.velocity.Y * 0.5f;
													}
													num603 = (int)(base.Center.X / 16f);
													num604 = (int)(base.Center.Y / 16f);
													float num606 = this.position.Y + (float)this.height;
													if (Main.tile[num603, num604 - 1] == null)
													{
														Main.tile[num603, num604 - 1] = new Tile();
													}
													if (Main.tile[num603, num604] == null)
													{
														Main.tile[num603, num604] = new Tile();
													}
													if (Main.tile[num603, num604 + 1] == null)
													{
														Main.tile[num603, num604 + 1] = new Tile();
													}
													if (Main.tile[num603, num604 - 1].liquid > 0)
													{
														num606 = (float)(num604 * 16);
														num606 -= (float)(Main.tile[num603, num604 - 1].liquid / 16);
													}
													else if (Main.tile[num603, num604].liquid > 0)
													{
														num606 = (float)((num604 + 1) * 16);
														num606 -= (float)(Main.tile[num603, num604].liquid / 16);
													}
													else if (Main.tile[num603, num604 + 1].liquid > 0)
													{
														num606 = (float)((num604 + 2) * 16);
														num606 -= (float)(Main.tile[num603, num604 + 1].liquid / 16);
													}
													if (base.Center.Y > num606)
													{
														this.velocity.Y = this.velocity.Y - 0.1f;
														if (this.velocity.Y < -8f)
														{
															this.velocity.Y = -8f;
														}
														if (base.Center.Y + this.velocity.Y < num606)
														{
															this.velocity.Y = num606 - base.Center.Y;
														}
													}
													else
													{
														this.velocity.Y = num606 - base.Center.Y;
													}
													if ((double)this.velocity.Y >= -0.01 && (double)this.velocity.Y <= 0.01)
													{
														flag20 = true;
													}
												}
												else
												{
													if (this.velocity.Y == 0f)
													{
														this.velocity.X = this.velocity.X * 0.95f;
													}
													this.velocity.X = this.velocity.X * 0.98f;
													this.velocity.Y = this.velocity.Y + 0.2f;
													if (this.velocity.Y > 15.9f)
													{
														this.velocity.Y = 15.9f;
													}
												}
												if (this.ai[1] != 0f)
												{
													flag20 = true;
												}
												if (flag20)
												{
													if (this.ai[1] == 0f && Main.myPlayer == this.owner)
													{
														int num607 = Main.player[this.owner].FishingLevel();
														if (num607 == -9000)
														{
															this.localAI[1] += 5f;
															this.localAI[1] += (float)Main.rand.Next(1, 3);
															if (this.localAI[1] > 660f)
															{
																this.localAI[1] = 0f;
																this.FishingCheck();
																return;
															}
														}
														else
														{
															if (Main.rand.Next(300) < num607)
															{
																this.localAI[1] += (float)Main.rand.Next(1, 3);
															}
															this.localAI[1] += (float)(num607 / 30);
															this.localAI[1] += (float)Main.rand.Next(1, 3);
															if (Main.rand.Next(60) == 0)
															{
																this.localAI[1] += 60f;
															}
															if (this.localAI[1] > 660f)
															{
																this.localAI[1] = 0f;
																this.FishingCheck();
																return;
															}
														}
													}
													else if (this.ai[1] < 0f)
													{
														if (this.velocity.Y == 0f || (this.honeyWet && (double)this.velocity.Y >= -0.01 && (double)this.velocity.Y <= 0.01))
														{
															this.velocity.Y = (float)Main.rand.Next(100, 500) * 0.015f;
															this.velocity.X = (float)Main.rand.Next(-100, 101) * 0.015f;
															this.wet = false;
															this.lavaWet = false;
															this.honeyWet = false;
														}
														this.ai[1] += (float)Main.rand.Next(1, 5);
														if (this.ai[1] >= 0f)
														{
															this.ai[1] = 0f;
															this.localAI[1] = 0f;
															this.netUpdate = true;
															return;
														}
													}
												}
											}
											else
											{
												if (this.aiStyle == 62)
												{
													this.AI_062();
													return;
												}
												if (this.aiStyle == 63)
												{
													if (!Main.player[this.owner].active)
													{
														this.active = false;
														return;
													}
													Vector2 vector53 = this.position;
													bool flag21 = false;
													float num608 = 500f;
													for (int num609 = 0; num609 < 200; num609++)
													{
														NPC nPC = Main.npc[num609];
														if (nPC.CanBeChasedBy(this, false))
														{
															float num610 = Vector2.Distance(nPC.Center, base.Center);
															if (((Vector2.Distance(base.Center, vector53) > num610 && num610 < num608) || !flag21) && Collision.CanHit(this.position, this.width, this.height, nPC.position, nPC.width, nPC.height))
															{
																num608 = num610;
																vector53 = nPC.Center;
																flag21 = true;
															}
														}
													}
													if (!flag21)
													{
														this.velocity.X = this.velocity.X * 0.95f;
													}
													else
													{
														float num611 = 5f;
														float num612 = 0.08f;
														if (this.velocity.Y == 0f)
														{
															bool flag22 = false;
															if (base.Center.Y - 50f > vector53.Y)
															{
																flag22 = true;
															}
															if (flag22)
															{
																this.velocity.Y = -6f;
															}
														}
														else
														{
															num611 = 8f;
															num612 = 0.12f;
														}
														this.velocity.X = this.velocity.X + (float)Math.Sign(vector53.X - base.Center.X) * num612;
														if (this.velocity.X < -num611)
														{
															this.velocity.X = -num611;
														}
														if (this.velocity.X > num611)
														{
															this.velocity.X = num611;
														}
													}
													float num613 = 0f;
													Collision.StepUp(ref this.position, ref this.velocity, this.width, this.height, ref num613, ref this.gfxOffY, 1, false, 0);
													if (this.velocity.Y != 0f)
													{
														this.frame = 3;
													}
													else
													{
														if (Math.Abs(this.velocity.X) > 0.2f)
														{
															this.frameCounter++;
														}
														if (this.frameCounter >= 9)
														{
															this.frameCounter = 0;
														}
														if (this.frameCounter >= 6)
														{
															this.frame = 2;
														}
														else if (this.frameCounter >= 3)
														{
															this.frame = 1;
														}
														else
														{
															this.frame = 0;
														}
													}
													if (this.velocity.X != 0f)
													{
														this.direction = Math.Sign(this.velocity.X);
													}
													this.spriteDirection = -this.direction;
													this.velocity.Y = this.velocity.Y + 0.2f;
													if (this.velocity.Y > 16f)
													{
														this.velocity.Y = 16f;
														return;
													}
												}
												else if (this.aiStyle == 64)
												{
													int num614 = 10;
													int num615 = 15;
													float num616 = 1f;
													int num617 = 150;
													int num618 = 42;
													if (this.type == 386)
													{
														num614 = 16;
														num615 = 16;
														num616 = 1.5f;
													}
													if (this.velocity.X != 0f)
													{
														this.direction = (this.spriteDirection = -Math.Sign(this.velocity.X));
													}
													this.frameCounter++;
													if (this.frameCounter > 2)
													{
														this.frame++;
														this.frameCounter = 0;
													}
													if (this.frame >= 6)
													{
														this.frame = 0;
													}
													if (this.localAI[0] == 0f && Main.myPlayer == this.owner)
													{
														this.localAI[0] = 1f;
														this.position.X = this.position.X + (float)(this.width / 2);
														this.position.Y = this.position.Y + (float)(this.height / 2);
														this.scale = ((float)(num614 + num615) - this.ai[1]) * num616 / (float)(num615 + num614);
														this.width = (int)((float)num617 * this.scale);
														this.height = (int)((float)num618 * this.scale);
														this.position.X = this.position.X - (float)(this.width / 2);
														this.position.Y = this.position.Y - (float)(this.height / 2);
														this.netUpdate = true;
													}
													if (this.ai[1] != -1f)
													{
														this.scale = ((float)(num614 + num615) - this.ai[1]) * num616 / (float)(num615 + num614);
														this.width = (int)((float)num617 * this.scale);
														this.height = (int)((float)num618 * this.scale);
													}
													if (!Collision.SolidCollision(this.position, this.width, this.height))
													{
														this.alpha -= 30;
														if (this.alpha < 60)
														{
															this.alpha = 60;
														}
														if (this.type == 386 && this.alpha < 100)
														{
															this.alpha = 100;
														}
													}
													else
													{
														this.alpha += 30;
														if (this.alpha > 150)
														{
															this.alpha = 150;
														}
													}
													if (this.ai[0] > 0f)
													{
														this.ai[0] -= 1f;
													}
													if (this.ai[0] == 1f && this.ai[1] > 0f && this.owner == Main.myPlayer)
													{
														this.netUpdate = true;
														Vector2 center = base.Center;
														center.Y -= (float)num618 * this.scale / 2f;
														float num619 = ((float)(num614 + num615) - this.ai[1] + 1f) * num616 / (float)(num615 + num614);
														center.Y -= (float)num618 * num619 / 2f;
														center.Y += 2f;
														Projectile.NewProjectile(center.X, center.Y, this.velocity.X, this.velocity.Y, this.type, this.damage, this.knockBack, this.owner, 10f, this.ai[1] - 1f);
														int num620 = 4;
														if (this.type == 386)
														{
															num620 = 2;
														}
														if ((int)this.ai[1] % num620 == 0 && this.ai[1] != 0f)
														{
															int num621 = 372;
															if (this.type == 386)
															{
																num621 = 373;
															}
															int num622 = NPC.NewNPC((int)center.X, (int)center.Y, num621, 0, 0f, 0f, 0f, 0f, 255);
															Main.npc[num622].velocity = this.velocity;
															Main.npc[num622].netUpdate = true;
															if (this.type == 386)
															{
																Main.npc[num622].ai[2] = (float)this.width;
																Main.npc[num622].ai[3] = -1.5f;
															}
														}
													}
													if (this.ai[0] <= 0f)
													{
														float num623 = 0.104719758f;
														float num624 = (float)this.width / 5f;
														if (this.type == 386)
														{
															num624 *= 2f;
														}
														float num625 = (float)(Math.Cos((double)(num623 * -(double)this.ai[0])) - 0.5) * num624;
														this.position.X = this.position.X - num625 * (float)(-(float)this.direction);
														this.ai[0] -= 1f;
														num625 = (float)(Math.Cos((double)(num623 * -(double)this.ai[0])) - 0.5) * num624;
														this.position.X = this.position.X + num625 * (float)(-(float)this.direction);
														return;
													}
												}
												else if (this.aiStyle == 65)
												{
													if (this.ai[1] > 0f)
													{
														int num626 = (int)this.ai[1] - 1;
														if (num626 < 255)
														{
															this.localAI[0] += 1f;
															if (this.localAI[0] > 10f)
															{
																this.alpha -= 5;
																if (this.alpha < 100)
																{
																	this.alpha = 100;
																}
																this.rotation += this.velocity.X * 0.1f;
																this.frame = (int)(this.localAI[0] / 3f) % 3;
															}
															Vector2 vector56 = Main.player[num626].Center - base.Center;
															float num630 = 4f;
															num630 += this.localAI[0] / 20f;
															this.velocity = Vector2.Normalize(vector56) * num630;
															if (vector56.Length() < 50f)
															{
																this.Kill();
															}
														}
													}
													else
													{
														float num631 = 0.209439516f;
														float num632 = 4f;
														float num633 = (float)(Math.Cos((double)(num631 * this.ai[0])) - 0.5) * num632;
														this.velocity.Y = this.velocity.Y - num633;
														this.ai[0] += 1f;
														num633 = (float)(Math.Cos((double)(num631 * this.ai[0])) - 0.5) * num632;
														this.velocity.Y = this.velocity.Y + num633;
														this.localAI[0] += 1f;
														if (this.localAI[0] > 10f)
														{
															this.alpha -= 5;
															if (this.alpha < 100)
															{
																this.alpha = 100;
															}
															this.rotation += this.velocity.X * 0.1f;
															this.frame = (int)(this.localAI[0] / 3f) % 3;
														}
													}
													if (this.wet)
													{
														this.position.Y = this.position.Y - 16f;
														this.Kill();
														return;
													}
												}
												else if (this.aiStyle == 66)
												{
													float num634 = 0f;
													float num635 = 0f;
													float num636 = 0f;
													float num637 = 0f;
													if (this.type == 387 || this.type == 388)
													{
														num634 = 700f;
														num635 = 800f;
														num636 = 1200f;
														num637 = 150f;
														if (Main.player[this.owner].dead)
														{
															Main.player[this.owner].twinsMinion = false;
														}
														if (Main.player[this.owner].twinsMinion)
														{
															this.timeLeft = 2;
														}
													}
													if (this.type == 533)
													{
														num634 = 1500f;
														num635 = 900f;
														num636 = 1500f;
														num637 = 450f;
														if (Main.player[this.owner].dead)
														{
															Main.player[this.owner].DeadlySphereMinion = false;
														}
														if (Main.player[this.owner].DeadlySphereMinion)
														{
															this.timeLeft = 2;
														}
													}
													float num638 = 0.05f;
													for (int num639 = 0; num639 < 1000; num639++)
													{
														bool flag23 = (Main.projectile[num639].type == 387 || Main.projectile[num639].type == 388) && (this.type == 387 || this.type == 388);
														if (!flag23)
														{
															flag23 = (this.type == 533 && Main.projectile[num639].type == 533);
														}
														if (num639 != this.whoAmI && Main.projectile[num639].active && Main.projectile[num639].owner == this.owner && flag23 && Math.Abs(this.position.X - Main.projectile[num639].position.X) + Math.Abs(this.position.Y - Main.projectile[num639].position.Y) < (float)this.width)
														{
															if (this.position.X < Main.projectile[num639].position.X)
															{
																this.velocity.X = this.velocity.X - num638;
															}
															else
															{
																this.velocity.X = this.velocity.X + num638;
															}
															if (this.position.Y < Main.projectile[num639].position.Y)
															{
																this.velocity.Y = this.velocity.Y - num638;
															}
															else
															{
																this.velocity.Y = this.velocity.Y + num638;
															}
														}
													}
													bool flag24 = false;
													if (this.ai[0] == 2f && this.type == 388)
													{
														this.ai[1] += 1f;
														this.extraUpdates = 1;
														this.rotation = this.velocity.ToRotation() + 3.14159274f;
														this.frameCounter++;
														if (this.frameCounter > 1)
														{
															this.frame++;
															this.frameCounter = 0;
														}
														if (this.frame > 2)
														{
															this.frame = 0;
														}
														if (this.ai[1] > 40f)
														{
															this.ai[1] = 1f;
															this.ai[0] = 0f;
															this.extraUpdates = 0;
															this.numUpdates = 0;
															this.netUpdate = true;
														}
														else
														{
															flag24 = true;
														}
													}
													if (this.type == 533 && this.ai[0] >= 3f && this.ai[0] <= 5f)
													{
														int num640 = 2;
														flag24 = true;
														this.velocity *= 0.9f;
														this.ai[1] += 1f;
														int num641 = (int)this.ai[1] / num640 + (int)(this.ai[0] - 3f) * 8;
														if (num641 < 4)
														{
															this.frame = 17 + num641;
														}
														else if (num641 < 5)
														{
															this.frame = 0;
														}
														else if (num641 < 8)
														{
															this.frame = 1 + num641 - 5;
														}
														else if (num641 < 11)
														{
															this.frame = 11 - num641;
														}
														else if (num641 < 12)
														{
															this.frame = 0;
														}
														else if (num641 < 16)
														{
															this.frame = num641 - 2;
														}
														else if (num641 < 20)
														{
															this.frame = 29 - num641;
														}
														else if (num641 < 21)
														{
															this.frame = 0;
														}
														else
														{
															this.frame = num641 - 4;
														}
														if (this.ai[1] > (float)(num640 * 8))
														{
															this.ai[0] -= 3f;
															this.ai[1] = 0f;
														}
													}
													if (this.type == 533 && this.ai[0] >= 6f && this.ai[0] <= 8f)
													{
														this.ai[1] += 1f;
														this.MaxUpdates = 2;
														if (this.ai[0] == 7f)
														{
															this.rotation = this.velocity.ToRotation() + 3.14159274f;
														}
														else
														{
															this.rotation += 0.5235988f;
														}
														int num642 = 0;
														switch ((int)this.ai[0])
														{
															case 6:
																this.frame = 5;
																num642 = 40;
																break;
															case 7:
																this.frame = 13;
																num642 = 30;
																break;
															case 8:
																this.frame = 17;
																num642 = 30;
																break;
														}
														if (this.ai[1] > (float)num642)
														{
															this.ai[1] = 1f;
															this.ai[0] -= 6f;
															this.localAI[0] += 1f;
															this.extraUpdates = 0;
															this.numUpdates = 0;
															this.netUpdate = true;
														}
														else
														{
															flag24 = true;
														}
													}
													if (flag24)
													{
														return;
													}
													Vector2 vector58 = this.position;
													bool flag25 = false;
													if (this.ai[0] != 1f && (this.type == 387 || this.type == 388))
													{
														this.tileCollide = true;
													}
													if (this.type == 533 && this.ai[0] < 9f)
													{
														this.tileCollide = true;
													}
													if (this.tileCollide && WorldGen.SolidTile(Framing.GetTileSafely((int)base.Center.X / 16, (int)base.Center.Y / 16)))
													{
														this.tileCollide = false;
													}
													for (int num646 = 0; num646 < 200; num646++)
													{
														NPC nPC2 = Main.npc[num646];
														if (nPC2.CanBeChasedBy(this, false))
														{
															float num647 = Vector2.Distance(nPC2.Center, base.Center);
															if (((Vector2.Distance(base.Center, vector58) > num647 && num647 < num634) || !flag25) && Collision.CanHitLine(this.position, this.width, this.height, nPC2.position, nPC2.width, nPC2.height))
															{
																num634 = num647;
																vector58 = nPC2.Center;
																flag25 = true;
															}
														}
													}
													float num648 = num635;
													if (flag25)
													{
														num648 = num636;
													}
													Player player = Main.player[this.owner];
													if (Vector2.Distance(player.Center, base.Center) > num648)
													{
														if (this.type == 387 || this.type == 388)
														{
															this.ai[0] = 1f;
														}
														if (this.type == 533 && this.ai[0] < 9f)
														{
															this.ai[0] += (float)(3 * (3 - (int)(this.ai[0] / 3f)));
														}
														this.tileCollide = false;
														this.netUpdate = true;
													}
													if ((this.type == 388 || this.type == 387) && flag25 && this.ai[0] == 0f)
													{
														Vector2 vector59 = vector58 - base.Center;
														float num649 = vector59.Length();
														vector59.Normalize();
														if (num649 > 200f)
														{
															float num650 = 6f;
															if (this.type == 388)
															{
																num650 = 8f;
															}
															vector59 *= num650;
															this.velocity = (this.velocity * 40f + vector59) / 41f;
														}
														else
														{
															float num651 = 4f;
															vector59 *= -num651;
															this.velocity = (this.velocity * 40f + vector59) / 41f;
														}
													}
													else
													{
														bool flag26 = false;
														if (!flag26)
														{
															flag26 = (this.ai[0] == 1f && (this.type == 387 || this.type == 388));
														}
														if (!flag26)
														{
															flag26 = (this.ai[0] >= 9f && this.type == 533);
														}
														float num652 = 6f;
														if (this.type == 533)
														{
															num652 = 12f;
														}
														if (flag26)
														{
															num652 = 15f;
														}
														Vector2 center2 = base.Center;
														Vector2 vector60 = player.Center - center2 + new Vector2(0f, -60f);
														float num653 = vector60.Length();
														if (num653 > 200f && num652 < 8f)
														{
															num652 = 8f;
														}
														if (num653 < num637 && flag26 && !Collision.SolidCollision(this.position, this.width, this.height))
														{
															if (this.type == 387 || this.type == 388)
															{
																this.ai[0] = 0f;
															}
															if (this.type == 533)
															{
																this.ai[0] -= 9f;
															}
															this.netUpdate = true;
														}
														if (num653 > 2000f)
														{
															this.position.X = Main.player[this.owner].Center.X - (float)(this.width / 2);
															this.position.Y = Main.player[this.owner].Center.Y - (float)(this.height / 2);
															this.netUpdate = true;
														}
														if (num653 > 70f)
														{
															vector60.Normalize();
															vector60 *= num652;
															this.velocity = (this.velocity * 40f + vector60) / 41f;
														}
														else if (this.velocity.X == 0f && this.velocity.Y == 0f)
														{
															this.velocity.X = -0.15f;
															this.velocity.Y = -0.05f;
														}
													}
													if (this.type == 388)
													{
														this.rotation = this.velocity.ToRotation() + 3.14159274f;
													}
													if (this.type == 387)
													{
														if (flag25)
														{
															this.rotation = (vector58 - base.Center).ToRotation() + 3.14159274f;
														}
														else
														{
															this.rotation = this.velocity.ToRotation() + 3.14159274f;
														}
													}
													if (this.type == 533 && (this.ai[0] < 3f || this.ai[0] >= 9f))
													{
														this.rotation += this.velocity.X * 0.04f;
													}
													if (this.type == 388 || this.type == 387)
													{
														this.frameCounter++;
														if (this.frameCounter > 3)
														{
															this.frame++;
															this.frameCounter = 0;
														}
														if (this.frame > 2)
														{
															this.frame = 0;
														}
													}
													else if (this.type == 533)
													{
														if (this.ai[0] < 3f || this.ai[0] >= 9f)
														{
															this.frameCounter++;
															if (this.frameCounter >= 24)
															{
																this.frameCounter = 0;
															}
															int num654 = this.frameCounter / 4;
															this.frame = 4 + num654;
															int num655 = (int)this.ai[0];
															switch (num655)
															{
																case 0:
																	break;
																case 1:
																	goto IL_1AB7D;
																case 2:
																	goto IL_1AB98;
																default:
																	switch (num655)
																	{
																		case 9:
																			break;
																		case 10:
																			goto IL_1AB7D;
																		case 11:
																			goto IL_1AB98;
																		default:
																			goto IL_1ABC2;
																	}
																	break;
															}
															this.frame = 4 + num654;
															goto IL_1ABC2;
															IL_1AB7D:
															num654 = this.frameCounter / 8;
															this.frame = 14 + num654;
															goto IL_1ABC2;
															IL_1AB98:
															num654 = this.frameCounter / 3;
															if (num654 >= 4)
															{
																num654 -= 4;
															}
															this.frame = 17 + num654;
														}
														IL_1ABC2:

														if (this.ai[1] > 0f && (this.type == 387 || this.type == 388))
														{
															this.ai[1] += (float)Main.rand.Next(1, 4);
														}
														if (this.ai[1] > 90f && this.type == 387)
														{
															this.ai[1] = 0f;
															this.netUpdate = true;
														}
														if (this.ai[1] > 40f && this.type == 388)
														{
															this.ai[1] = 0f;
															this.netUpdate = true;
														}
														if (this.ai[1] > 0f && this.type == 533)
														{
															this.ai[1] += 1f;
															int num659 = 10;
															if (this.ai[1] > (float)num659)
															{
																this.ai[1] = 0f;
																this.netUpdate = true;
															}
														}
														if (this.ai[0] == 0f && (this.type == 387 || this.type == 388))
														{
															if (this.type == 387)
															{
																float num660 = 8f;
																int num661 = 389;
																if (flag25 && this.ai[1] == 0f)
																{
																	this.ai[1] += 1f;
																	if (Main.myPlayer == this.owner && Collision.CanHitLine(this.position, this.width, this.height, vector58, 0, 0))
																	{
																		Vector2 vector62 = vector58 - base.Center;
																		vector62.Normalize();
																		vector62 *= num660;
																		int num662 = Projectile.NewProjectile(base.Center.X, base.Center.Y, vector62.X, vector62.Y, num661, (int)((float)this.damage * 0.8f), 0f, Main.myPlayer, 0f, 0f);
																		Main.projectile[num662].timeLeft = 300;
																		this.netUpdate = true;
																	}
																}
															}
															if (this.type == 388 && this.ai[1] == 0f && flag25 && num634 < 500f)
															{
																this.ai[1] += 1f;
																if (Main.myPlayer == this.owner)
																{
																	this.ai[0] = 2f;
																	Vector2 vector63 = vector58 - base.Center;
																	vector63.Normalize();
																	this.velocity = vector63 * 8f;
																	this.netUpdate = true;
																	return;
																}
															}
														}
														else if (this.type == 533 && this.ai[0] < 3f)
														{
															int num663 = 0;
															switch ((int)this.ai[0])
															{
																case 0:
																case 3:
																case 6:
																	num663 = 400;
																	break;
																case 1:
																case 4:
																case 7:
																	num663 = 400;
																	break;
																case 2:
																case 5:
																case 8:
																	num663 = 600;
																	break;
															}
															if (this.ai[1] == 0f && flag25 && num634 < (float)num663)
															{
																this.ai[1] += 1f;
																if (Main.myPlayer == this.owner)
																{
																	if (this.localAI[0] >= 3f)
																	{
																		this.ai[0] += 4f;
																		if (this.ai[0] == 6f)
																		{
																			this.ai[0] = 3f;
																		}
																		this.localAI[0] = 0f;
																		return;
																	}
																	this.ai[0] += 6f;
																	Vector2 vector64 = vector58 - base.Center;
																	vector64.Normalize();
																	float num664 = (this.ai[0] == 8f) ? 12f : 10f;
																	this.velocity = vector64 * num664;
																	this.netUpdate = true;
																	return;
																}
															}
														}
													}
												}
												else if (this.aiStyle == 67)
												{
													Player player2 = Main.player[this.owner];
													if (!player2.active)
													{
														this.active = false;
														return;
													}
													bool flag27 = this.type == 393 || this.type == 394 || this.type == 395;
													if (flag27)
													{
														if (player2.dead)
														{
															player2.pirateMinion = false;
														}
														if (player2.pirateMinion)
														{
															this.timeLeft = 2;
														}
													}
													if (this.type == 500)
													{
														if (player2.dead)
														{
															player2.crimsonHeart = false;
														}
														if (player2.crimsonHeart)
														{
															this.timeLeft = 2;
														}
													}
													if (this.type == 653)
													{
														if (player2.dead)
														{
															player2.companionCube = false;
														}
														if (player2.companionCube)
														{
															this.timeLeft = 2;
														}
													}
													Vector2 vector65 = player2.Center;
													if (flag27)
													{
														vector65.X -= (float)((15 + player2.width / 2) * player2.direction);
														vector65.X -= (float)(this.minionPos * 40 * player2.direction);
													}
													if (this.type == 500)
													{
														vector65.X -= (float)((15 + player2.width / 2) * player2.direction);
														vector65.X -= (float)(40 * player2.direction);
													}
													if (this.type == 653)
													{
														float num665 = (float)(15 + (player2.crimsonHeart ? 40 : 0));
														vector65.X -= (num665 + (float)(player2.width / 2)) * (float)player2.direction;
														vector65.X -= (float)(40 * player2.direction);
													}
													if (this.type == 500)
													{
														int num665 = 6;
														if (this.frame == 0 || this.frame == 2)
														{
															num665 = 12;
														}
														if (++this.frameCounter >= num665)
														{
															this.frameCounter = 0;
															if (++this.frame >= Main.projFrames[this.type])
															{
																this.frame = 0;
															}
														}
														this.rotation += this.velocity.X / 20f;
														Vector2 vector66 = (-Vector2.UnitY).RotatedBy((double)this.rotation, default(Vector2)).RotatedBy((double)((float)this.direction * 0.2f), default(Vector2));
													}
													if (this.type == 653)
													{
														this.rotation += this.velocity.X / 20f;
														if (this.velocity.Y == 0f)
														{
															this.rotation = this.rotation.AngleTowards(0f, 0.7f);
														}
														if (this.owner >= 0 && this.owner < 255)
														{
															Projectile._CompanionCubeScreamCooldown[this.owner] -= 1f;
															if (Projectile._CompanionCubeScreamCooldown[this.owner] < 0f)
															{
																Projectile._CompanionCubeScreamCooldown[this.owner] = 0f;
															}
														}
														Tile tileSafely = Framing.GetTileSafely(base.Center);
														if (tileSafely.liquid > 0 && tileSafely.lava())
														{
															this.localAI[0] += 1f;
														}
														else
														{
															this.localAI[0] -= 1f;
														}
														this.localAI[0] = MathHelper.Clamp(this.localAI[0], 0f, 20f);
														if (this.localAI[0] >= 20f)
														{
															if (Projectile._CompanionCubeScreamCooldown[this.owner] == 0f)
															{
																Projectile._CompanionCubeScreamCooldown[this.owner] = 3600f;
															}
															this.Kill();
														}
														else if (this.localAI[1] > 0f)
														{
															this.localAI[1] -= 1f;
														}
														this.localAI[1] = MathHelper.Clamp(this.localAI[1], -3600f, 120f);
														if (this.localAI[1] > (float)Main.rand.Next(30, 120) && !player2.immune && player2.velocity == Vector2.Zero)
														{
															if (Main.rand.Next(5) == 0)
															{
																this.localAI[1] = -600f;
															}
															else
															{
																player2.Hurt(3, 0, false, false, Lang.deathMsg(-1, -1, -1, 6), false, -1);
																player2.immune = false;
																player2.immuneTime = 0;
																this.localAI[1] = (float)(-300 + Main.rand.Next(30) * -10);
															}
														}
													}
													bool flag28 = true;
													if (this.type == 500 || this.type == 653)
													{
														flag28 = false;
													}
													int num667 = -1;
													float num668 = 450f;
													if (flag27)
													{
														num668 = 800f;
													}
													int num669 = 15;
													if (this.ai[0] == 0f && flag28)
													{
														for (int num670 = 0; num670 < 200; num670++)
														{
															NPC nPC3 = Main.npc[num670];
															if (nPC3.CanBeChasedBy(this, false))
															{
																float num671 = (nPC3.Center - base.Center).Length();
																if (num671 < num668)
																{
																	num667 = num670;
																	num668 = num671;
																}
															}
														}
													}
													if (this.ai[0] == 1f)
													{
														this.tileCollide = false;
														float num672 = 0.2f;
														float num673 = 10f;
														int num674 = 200;
														if (num673 < Math.Abs(player2.velocity.X) + Math.Abs(player2.velocity.Y))
														{
															num673 = Math.Abs(player2.velocity.X) + Math.Abs(player2.velocity.Y);
														}
														Vector2 vector67 = player2.Center - base.Center;
														float num675 = vector67.Length();
														if (num675 > 2000f)
														{
															this.position = player2.Center - new Vector2((float)this.width, (float)this.height) / 2f;
														}
														if (num675 < (float)num674 && player2.velocity.Y == 0f && this.position.Y + (float)this.height <= player2.position.Y + (float)player2.height && !Collision.SolidCollision(this.position, this.width, this.height))
														{
															this.ai[0] = 0f;
															this.netUpdate = true;
															if (this.velocity.Y < -6f)
															{
																this.velocity.Y = -6f;
															}
														}
														if (num675 >= 60f)
														{
															vector67.Normalize();
															vector67 *= num673;
															if (this.velocity.X < vector67.X)
															{
																this.velocity.X = this.velocity.X + num672;
																if (this.velocity.X < 0f)
																{
																	this.velocity.X = this.velocity.X + num672 * 1.5f;
																}
															}
															if (this.velocity.X > vector67.X)
															{
																this.velocity.X = this.velocity.X - num672;
																if (this.velocity.X > 0f)
																{
																	this.velocity.X = this.velocity.X - num672 * 1.5f;
																}
															}
															if (this.velocity.Y < vector67.Y)
															{
																this.velocity.Y = this.velocity.Y + num672;
																if (this.velocity.Y < 0f)
																{
																	this.velocity.Y = this.velocity.Y + num672 * 1.5f;
																}
															}
															if (this.velocity.Y > vector67.Y)
															{
																this.velocity.Y = this.velocity.Y - num672;
																if (this.velocity.Y > 0f)
																{
																	this.velocity.Y = this.velocity.Y - num672 * 1.5f;
																}
															}
														}
														if (this.velocity.X != 0f)
														{
															this.spriteDirection = Math.Sign(this.velocity.X);
														}
														if (flag27)
														{
															this.frameCounter++;
															if (this.frameCounter > 3)
															{
																this.frame++;
																this.frameCounter = 0;
															}
															if (this.frame < 10 | this.frame > 13)
															{
																this.frame = 10;
															}
															this.rotation = this.velocity.X * 0.1f;
														}
													}
													if (this.ai[0] == 2f)
													{
														this.friendly = true;
														this.spriteDirection = this.direction;
														this.rotation = 0f;
														this.frame = 4 + (int)((float)num669 - this.ai[1]) / (num669 / 3);
														if (this.velocity.Y != 0f)
														{
															this.frame += 3;
														}
														this.velocity.Y = this.velocity.Y + 0.4f;
														if (this.velocity.Y > 10f)
														{
															this.velocity.Y = 10f;
														}
														this.ai[1] -= 1f;
														if (this.ai[1] <= 0f)
														{
															this.ai[1] = 0f;
															this.ai[0] = 0f;
															this.friendly = false;
															this.netUpdate = true;
															return;
														}
													}
													if (num667 >= 0)
													{
														float num676 = 400f;
														float num677 = 20f;
														if (flag27)
														{
															num676 = 700f;
														}
														if ((double)this.position.Y > Main.worldSurface * 16.0)
														{
															num676 *= 0.7f;
														}
														NPC nPC4 = Main.npc[num667];
														Vector2 center3 = nPC4.Center;
														float num678 = (center3 - base.Center).Length();
														Collision.CanHit(this.position, this.width, this.height, nPC4.position, nPC4.width, nPC4.height);
														if (num678 < num676)
														{
															vector65 = center3;
															if (center3.Y < base.Center.Y - 30f && this.velocity.Y == 0f)
															{
																float num679 = Math.Abs(center3.Y - base.Center.Y);
																if (num679 < 120f)
																{
																	this.velocity.Y = -10f;
																}
																else if (num679 < 210f)
																{
																	this.velocity.Y = -13f;
																}
																else if (num679 < 270f)
																{
																	this.velocity.Y = -15f;
																}
																else if (num679 < 310f)
																{
																	this.velocity.Y = -17f;
																}
																else if (num679 < 380f)
																{
																	this.velocity.Y = -18f;
																}
															}
														}
														if (num678 < num677)
														{
															this.ai[0] = 2f;
															this.ai[1] = (float)num669;
															this.netUpdate = true;
														}
													}
													if (this.ai[0] == 0f && num667 < 0)
													{
														float num680 = 500f;
														if (this.type == 500)
														{
															num680 = 200f;
														}
														if (this.type == 653)
														{
															num680 = 170f;
														}
														if (Main.player[this.owner].rocketDelay2 > 0)
														{
															this.ai[0] = 1f;
															this.netUpdate = true;
														}
														Vector2 vector68 = player2.Center - base.Center;
														if (vector68.Length() > 2000f)
														{
															this.position = player2.Center - new Vector2((float)this.width, (float)this.height) / 2f;
														}
														else if (vector68.Length() > num680 || Math.Abs(vector68.Y) > 300f)
														{
															this.ai[0] = 1f;
															this.netUpdate = true;
															if (this.velocity.Y > 0f && vector68.Y < 0f)
															{
																this.velocity.Y = 0f;
															}
															if (this.velocity.Y < 0f && vector68.Y > 0f)
															{
																this.velocity.Y = 0f;
															}
														}
													}
													if (this.ai[0] == 0f)
													{
														this.tileCollide = true;
														float num681 = 0.5f;
														float num682 = 4f;
														float num683 = 4f;
														float num684 = 0.1f;
														if (num683 < Math.Abs(player2.velocity.X) + Math.Abs(player2.velocity.Y))
														{
															num683 = Math.Abs(player2.velocity.X) + Math.Abs(player2.velocity.Y);
															num681 = 0.7f;
														}
														int num685 = 0;
														bool flag29 = false;
														float num686 = vector65.X - base.Center.X;
														if (Math.Abs(num686) > 5f)
														{
															if (num686 < 0f)
															{
																num685 = -1;
																if (this.velocity.X > -num682)
																{
																	this.velocity.X = this.velocity.X - num681;
																}
																else
																{
																	this.velocity.X = this.velocity.X - num684;
																}
															}
															else
															{
																num685 = 1;
																if (this.velocity.X < num682)
																{
																	this.velocity.X = this.velocity.X + num681;
																}
																else
																{
																	this.velocity.X = this.velocity.X + num684;
																}
															}
															if (!flag27)
															{
																flag29 = true;
															}
														}
														else
														{
															this.velocity.X = this.velocity.X * 0.9f;
															if (Math.Abs(this.velocity.X) < num681 * 2f)
															{
																this.velocity.X = 0f;
															}
														}
														if (num685 != 0)
														{
															int num687 = (int)(this.position.X + (float)(this.width / 2)) / 16;
															int num688 = (int)this.position.Y / 16;
															num687 += num685;
															num687 += (int)this.velocity.X;
															for (int num689 = num688; num689 < num688 + this.height / 16 + 1; num689++)
															{
																if (WorldGen.SolidTile(num687, num689))
																{
																	flag29 = true;
																}
															}
														}
														if (this.type == 500 && this.velocity.X != 0f)
														{
															flag29 = true;
														}
														if (this.type == 653 && this.velocity.X != 0f)
														{
															flag29 = true;
														}
														Collision.StepUp(ref this.position, ref this.velocity, this.width, this.height, ref this.stepSpeed, ref this.gfxOffY, 1, false, 0);
														if (this.velocity.Y == 0f && flag29)
														{
															int num690 = 0;
															while (num690 < 3)
															{
																int num691 = (int)(this.position.X + (float)(this.width / 2)) / 16;
																if (num690 == 0)
																{
																	num691 = (int)this.position.X / 16;
																}
																if (num690 == 2)
																{
																	num691 = (int)(this.position.X + (float)this.width) / 16;
																}
																int num692 = (int)(this.position.Y + (float)this.height) / 16 + 1;
																if (WorldGen.SolidTile(num691, num692) || Main.tile[num691, num692].halfBrick())
																{
																	goto Block_1931;
																}
																if (TileID.Sets.Platforms[(int)Main.tile[num691, num692].type] && Main.tile[num691, num692].active() && !Main.tile[num691, num692].inActive())
																{
																	goto Block_1931;
																}
																IL_1C28D:
																num690++;
																continue;
																Block_1931:
																try
																{
																	num691 = (int)(this.position.X + (float)(this.width / 2)) / 16;
																	num692 = (int)(this.position.Y + (float)(this.height / 2)) / 16;
																	num691 += num685;
																	num691 += (int)this.velocity.X;
																	if (!WorldGen.SolidTile(num691, num692 - 1) && !WorldGen.SolidTile(num691, num692 - 2))
																	{
																		this.velocity.Y = -5.1f;
																	}
																	else if (!WorldGen.SolidTile(num691, num692 - 2))
																	{
																		this.velocity.Y = -7.1f;
																	}
																	else if (WorldGen.SolidTile(num691, num692 - 5))
																	{
																		this.velocity.Y = -11.1f;
																	}
																	else if (WorldGen.SolidTile(num691, num692 - 4))
																	{
																		this.velocity.Y = -10.1f;
																	}
																	else
																	{
																		this.velocity.Y = -9.1f;
																	}
																}
																catch
																{
																	this.velocity.Y = -9.1f;
																}
																goto IL_1C28D;
															}
														}
														if (this.velocity.X > num683)
														{
															this.velocity.X = num683;
														}
														if (this.velocity.X < -num683)
														{
															this.velocity.X = -num683;
														}
														if (this.velocity.X < 0f)
														{
															this.direction = -1;
														}
														if (this.velocity.X > 0f)
														{
															this.direction = 1;
														}
														if (this.velocity.X > num681 && num685 == 1)
														{
															this.direction = 1;
														}
														if (this.velocity.X < -num681 && num685 == -1)
														{
															this.direction = -1;
														}
														this.spriteDirection = this.direction;
														if (flag27)
														{
															this.rotation = 0f;
															if (this.velocity.Y == 0f)
															{
																if (this.velocity.X == 0f)
																{
																	this.frame = 0;
																	this.frameCounter = 0;
																}
																else if (Math.Abs(this.velocity.X) >= 0.5f)
																{
																	this.frameCounter += (int)Math.Abs(this.velocity.X);
																	this.frameCounter++;
																	if (this.frameCounter > 10)
																	{
																		this.frame++;
																		this.frameCounter = 0;
																	}
																	if (this.frame >= 4)
																	{
																		this.frame = 0;
																	}
																}
																else
																{
																	this.frame = 0;
																	this.frameCounter = 0;
																}
															}
															else if (this.velocity.Y != 0f)
															{
																this.frameCounter = 0;
																this.frame = 14;
															}
														}
														this.velocity.Y = this.velocity.Y + 0.4f;
														if (this.velocity.Y > 10f)
														{
															this.velocity.Y = 10f;
														}
													}
													if (flag27)
													{
														this.localAI[0] += 1f;
														if (this.velocity.X == 0f)
														{
															this.localAI[0] += 1f;
														}
														if (this.localAI[0] >= (float)Main.rand.Next(900, 1200))
														{
															return;
														}
													}
												}
												else if (this.aiStyle == 68)
												{
													this.rotation += 0.25f * (float)this.direction;
													this.ai[0] += 1f;
													if (this.ai[0] >= 3f)
													{
														this.alpha -= 40;
														if (this.alpha < 0)
														{
															this.alpha = 0;
														}
													}
													if (this.ai[0] >= 15f)
													{
														this.velocity.Y = this.velocity.Y + 0.2f;
														if (this.velocity.Y > 16f)
														{
															this.velocity.Y = 16f;
														}
														this.velocity.X = this.velocity.X * 0.99f;
													}
													if (this.alpha == 0)
													{
													}
													this.spriteDirection = this.direction;
													if (this.owner == Main.myPlayer && this.timeLeft <= 3)
													{
														this.tileCollide = false;
														this.alpha = 255;
														this.position.X = this.position.X + (float)(this.width / 2);
														this.position.Y = this.position.Y + (float)(this.height / 2);
														this.width = 80;
														this.height = 80;
														this.position.X = this.position.X - (float)(this.width / 2);
														this.position.Y = this.position.Y - (float)(this.height / 2);
														this.knockBack = 8f;
													}
													if (this.wet && this.timeLeft > 3)
													{
														this.timeLeft = 3;
														return;
													}
												}
												else if (this.aiStyle == 69)
												{
													Vector2 vector70 = Main.player[this.owner].Center - base.Center;
													this.rotation = vector70.ToRotation() - 1.57f;
													if (Main.player[this.owner].dead)
													{
														this.Kill();
														return;
													}
													Main.player[this.owner].itemAnimation = 10;
													Main.player[this.owner].itemTime = 10;
													if (vector70.X < 0f)
													{
														Main.player[this.owner].ChangeDir(1);
														this.direction = 1;
													}
													else
													{
														Main.player[this.owner].ChangeDir(-1);
														this.direction = -1;
													}
													Main.player[this.owner].itemRotation = (vector70 * -1f * (float)this.direction).ToRotation();
													this.spriteDirection = ((vector70.X > 0f) ? -1 : 1);
													if (this.ai[0] == 0f && vector70.Length() > 400f)
													{
														this.ai[0] = 1f;
													}
													if (this.ai[0] == 1f || this.ai[0] == 2f)
													{
														float num699 = vector70.Length();
														if (num699 > 1500f)
														{
															this.Kill();
															return;
														}
														if (num699 > 600f)
														{
															this.ai[0] = 2f;
														}
														this.tileCollide = false;
														float num700 = 20f;
														if (this.ai[0] == 2f)
														{
															num700 = 40f;
														}
														this.velocity = Vector2.Normalize(vector70) * num700;
														if (vector70.Length() < num700)
														{
															this.Kill();
															return;
														}
													}
													this.ai[1] += 1f;
													if (this.ai[1] > 5f)
													{
														this.alpha = 0;
													}
													if ((int)this.ai[1] % 4 == 0 && this.owner == Main.myPlayer)
													{
														Vector2 vector71 = vector70 * -1f;
														vector71.Normalize();
														vector71 *= (float)Main.rand.Next(45, 65) * 0.1f;
														vector71 = vector71.RotatedBy((Main.rand.NextDouble() - 0.5) * 1.5707963705062866, default(Vector2));
														Projectile.NewProjectile(base.Center.X, base.Center.Y, vector71.X, vector71.Y, 405, this.damage, this.knockBack, this.owner, -10f, 0f);
														return;
													}
												}
												else
												{
													if (this.aiStyle == 70)
													{
														if (this.ai[0] == 0f)
														{
															float num701 = 500f;
															int num702 = -1;
															for (int num703 = 0; num703 < 200; num703++)
															{
																NPC nPC5 = Main.npc[num703];
																if (nPC5.CanBeChasedBy(this, false) && Collision.CanHit(this.position, this.width, this.height, nPC5.position, nPC5.width, nPC5.height))
																{
																	float num704 = (nPC5.Center - base.Center).Length();
																	if (num704 < num701)
																	{
																		num702 = num703;
																		num701 = num704;
																	}
																}
															}
															this.ai[0] = (float)(num702 + 1);
															if (this.ai[0] == 0f)
															{
																this.ai[0] = -15f;
															}
															if (this.ai[0] > 0f)
															{
																float num705 = (float)Main.rand.Next(35, 75) / 30f;
																this.velocity = (this.velocity * 20f + Vector2.Normalize(Main.npc[(int)this.ai[0] - 1].Center - base.Center + new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101))) * num705) / 21f;
																this.netUpdate = true;
															}
														}
														else if (this.ai[0] > 0f)
														{
															Vector2 vector72 = Vector2.Normalize(Main.npc[(int)this.ai[0] - 1].Center - base.Center);
															this.velocity = (this.velocity * 40f + vector72 * 12f) / 41f;
														}
														else
														{
															this.ai[0] += 1f;
															this.alpha -= 25;
															if (this.alpha < 50)
															{
																this.alpha = 50;
															}
															this.velocity *= 0.95f;
														}
														if (this.ai[1] == 0f)
														{
															this.ai[1] = (float)Main.rand.Next(80, 121) / 100f;
															this.netUpdate = true;
														}
														this.scale = this.ai[1];
														return;
													}
													if (this.aiStyle == 71)
													{
														this.localAI[1] += 1f;
														if (this.localAI[1] > 10f && Main.rand.Next(3) == 0)
														{
															this.alpha -= 5;
															if (this.alpha < 50)
															{
																this.alpha = 50;
															}
															this.rotation += this.velocity.X * 0.1f;
															this.frame = (int)(this.localAI[1] / 3f) % 3;
														}
														int num709 = -1;
														Vector2 vector75 = base.Center;
														float num710 = 500f;
														if (this.localAI[0] > 0f)
														{
															this.localAI[0] -= 1f;
														}
														if (this.ai[0] == 0f && this.localAI[0] == 0f)
														{
															for (int num711 = 0; num711 < 200; num711++)
															{
																NPC nPC6 = Main.npc[num711];
																if (nPC6.CanBeChasedBy(this, false) && (this.ai[0] == 0f || this.ai[0] == (float)(num711 + 1)))
																{
																	Vector2 center4 = nPC6.Center;
																	float num712 = Vector2.Distance(center4, vector75);
																	if (num712 < num710 && Collision.CanHit(this.position, this.width, this.height, nPC6.position, nPC6.width, nPC6.height))
																	{
																		num710 = num712;
																		vector75 = center4;
																		num709 = num711;
																	}
																}
															}
															if (num709 >= 0)
															{
																this.ai[0] = (float)(num709 + 1);
																this.netUpdate = true;
															}
														}
														if (this.localAI[0] == 0f && this.ai[0] == 0f)
														{
															this.localAI[0] = 30f;
														}
														bool flag30 = false;
														if (this.ai[0] != 0f)
														{
															int num713 = (int)(this.ai[0] - 1f);
															if (Main.npc[num713].active && !Main.npc[num713].dontTakeDamage && Main.npc[num713].immune[this.owner] == 0)
															{
																float num714 = Main.npc[num713].position.X + (float)(Main.npc[num713].width / 2);
																float num715 = Main.npc[num713].position.Y + (float)(Main.npc[num713].height / 2);
																float num716 = Math.Abs(this.position.X + (float)(this.width / 2) - num714) + Math.Abs(this.position.Y + (float)(this.height / 2) - num715);
																if (num716 < 1000f)
																{
																	flag30 = true;
																	vector75 = Main.npc[num713].Center;
																}
															}
															else
															{
																this.ai[0] = 0f;
																flag30 = false;
																this.netUpdate = true;
															}
														}
														if (flag30)
														{
															Vector2 v = vector75 - base.Center;
															float num717 = this.velocity.ToRotation();
															float num718 = v.ToRotation();
															double num719 = (double)(num718 - num717);
															if (num719 > 3.1415926535897931)
															{
																num719 -= 6.2831853071795862;
															}
															if (num719 < -3.1415926535897931)
															{
																num719 += 6.2831853071795862;
															}
															this.velocity = this.velocity.RotatedBy(num719 * 0.10000000149011612, default(Vector2));
														}
														float num720 = this.velocity.Length();
														this.velocity.Normalize();
														this.velocity *= num720 + 0.0025f;
														return;
													}
													if (this.aiStyle == 72)
													{
														this.localAI[0] += 1f;
														if (this.localAI[0] > 5f)
														{
															this.alpha -= 25;
															if (this.alpha < 50)
															{
																this.alpha = 50;
															}
														}
														this.velocity *= 0.96f;
														if (this.ai[1] == 0f)
														{
															this.ai[1] = (float)Main.rand.Next(60, 121) / 100f;
															this.netUpdate = true;
														}
														this.scale = this.ai[1];
														this.position = base.Center;
														int num721 = 14;
														int num722 = 14;
														this.width = (int)((float)num721 * this.ai[1]);
														this.height = (int)((float)num722 * this.ai[1]);
														this.position -= new Vector2((float)(this.width / 2), (float)(this.height / 2));
														return;
													}
													if (this.aiStyle == 73)
													{
														int num723 = (int)this.ai[0];
														int num724 = (int)this.ai[1];
														Tile tile = Main.tile[num723, num724];
														if (tile == null || !tile.active() || tile.type != 338)
														{
															this.Kill();
															return;
														}
														float num725 = 2f;
														float num726 = (float)this.timeLeft / 60f;
														if (num726 < 1f)
														{
															num725 *= num726;
														}
														if (this.type == 422)
														{
															return;
														}
													}
													else if (this.aiStyle == 74)
													{
														if (this.extraUpdates == 1)
														{
															this.localAI[0] *= this.localAI[1];
															this.localAI[1] -= 0.001f;
															if ((double)this.localAI[0] < 0.01)
															{
																this.Kill();
																return;
															}
														}
													}
													else
													{
														if (this.aiStyle == 75)
														{
															this.AI_075();
															return;
														}
														if (this.aiStyle == 76)
														{
															Player player3 = Main.player[this.owner];
															player3.heldProj = this.whoAmI;
															if (this.type == 441)
															{
																if (player3.mount.Type != 9)
																{
																	this.Kill();
																	return;
																}
															}
															else if (this.type == 453 && player3.mount.Type != 8)
															{
																this.Kill();
																return;
															}
															if (Main.myPlayer != this.owner)
															{
																this.position.X = player3.position.X + this.ai[0];
																this.position.Y = player3.position.Y + this.ai[1];
																if (this.type == 441)
																{
																	if (!player3.mount.AbilityCharging)
																	{
																		player3.mount.StartAbilityCharge(player3);
																	}
																}
																else if (this.type == 453 && !player3.mount.AbilityActive)
																{
																	player3.mount.UseAbility(player3, this.position, false);
																}
																player3.mount.AimAbility(player3, this.position);
																return;
															}
															this.position.X = Main.screenPosition.X + (float)Main.mouseX;
															this.position.Y = Main.screenPosition.Y + (float)Main.mouseY;
															if (this.ai[0] != this.position.X - player3.position.X || this.ai[1] != this.position.Y - player3.position.Y)
															{
																this.netUpdate = true;
															}
															this.ai[0] = this.position.X - player3.position.X;
															this.ai[1] = this.position.Y - player3.position.Y;
															player3.mount.AimAbility(player3, this.position);
															if (!player3.channel)
															{
																player3.mount.UseAbility(player3, this.position, false);
																this.Kill();
																return;
															}
														}
														else
														{
															if (this.aiStyle == 77)
															{
																if (this.ai[1] == 1f)
																{
																	this.friendly = false;
																	if (this.alpha < 255)
																	{
																		this.alpha += 51;
																	}
																	if (this.alpha >= 255)
																	{
																		this.alpha = 255;
																		this.Kill();
																		return;
																	}
																}
																else
																{
																	if (this.alpha > 0)
																	{
																		this.alpha -= 50;
																	}
																	if (this.alpha < 0)
																	{
																		this.alpha = 0;
																	}
																}
																float num739 = 30f;
																float num740 = num739 * 4f;
																this.ai[0] += 1f;
																if (this.ai[0] > num740)
																{
																	this.ai[0] = 0f;
																}
																Vector2 vector81 = -Vector2.UnitY.RotatedBy((double)(6.28318548f * this.ai[0] / num739), default(Vector2));
																float val = 0.75f + vector81.Y * 0.25f;
																float val2 = 0.8f - vector81.Y * 0.2f;
																float num741 = Math.Max(val, val2);
																this.position += new Vector2((float)this.width, (float)this.height) / 2f;
																this.width = (this.height = (int)(80f * num741));
																this.position -= new Vector2((float)this.width, (float)this.height) / 2f;
																this.frameCounter++;
																if (this.frameCounter >= 3)
																{
																	this.frameCounter = 0;
																	this.frame++;
																	if (this.frame >= 4)
																	{
																		this.frame = 0;
																	}
																}
																return;
															}
															if (this.aiStyle == 78)
															{
																if (this.alpha > 0)
																{
																	this.alpha -= 30;
																}
																if (this.alpha < 0)
																{
																	this.alpha = 0;
																}
																Vector2 v2 = this.ai[0].ToRotationVector2();
																float num747 = this.velocity.ToRotation();
																float num748 = v2.ToRotation();
																double num749 = (double)(num748 - num747);
																if (num749 > 3.1415926535897931)
																{
																	num749 -= 6.2831853071795862;
																}
																if (num749 < -3.1415926535897931)
																{
																	num749 += 6.2831853071795862;
																}
																this.velocity = this.velocity.RotatedBy(num749 * 0.05000000074505806, default(Vector2));
																this.velocity *= 0.96f;
																this.rotation = this.velocity.ToRotation() - 1.57079637f;
																if (Main.myPlayer == this.owner && this.timeLeft > 60)
																{
																	this.timeLeft = 60;
																	return;
																}
															}
															else if (this.aiStyle == 79)
															{
																bool flag31 = true;
																int num750 = (int)this.ai[0] - 1;
																if (this.type == 447 && (this.ai[0] == 0f || ((!Main.npc[num750].active || Main.npc[num750].type != 392) && (!Main.npc[num750].active || Main.npc[num750].type != 395 || Main.npc[num750].ai[3] % 120f < 60f || Main.npc[num750].ai[0] != 2f))))
																{
																	flag31 = false;
																}
																if (!flag31)
																{
																	this.Kill();
																	return;
																}
																NPC nPC7 = Main.npc[num750];
																float num751 = nPC7.Center.Y + 46f;
																int num752 = (int)nPC7.Center.X / 16;
																int num753 = (int)num751 / 16;
																int num754 = 0;
																bool flag32 = Main.tile[num752, num753].nactive() && Main.tileSolid[(int)Main.tile[num752, num753].type] && !Main.tileSolidTop[(int)Main.tile[num752, num753].type];
																if (flag32)
																{
																	num754 = 1;
																}
																else
																{
																	while (num754 < 150 && num753 + num754 < Main.maxTilesY)
																	{
																		int num755 = num753 + num754;
																		bool flag33 = Main.tile[num752, num755].nactive() && Main.tileSolid[(int)Main.tile[num752, num755].type] && !Main.tileSolidTop[(int)Main.tile[num752, num755].type];
																		if (flag33)
																		{
																			num754--;
																			break;
																		}
																		num754++;
																	}
																}
																this.position.X = nPC7.Center.X - (float)(this.width / 2);
																this.position.Y = num751;
																this.height = (num754 + 1) * 16;
																int num756 = (int)this.position.Y + this.height;
																if (Main.tile[num752, num756 / 16].nactive() && Main.tileSolid[(int)Main.tile[num752, num756 / 16].type] && !Main.tileSolidTop[(int)Main.tile[num752, num756 / 16].type])
																{
																	int num757 = num756 % 16;
																	this.height -= num757 - 2;
																}
																if (this.type == 447 && ++this.frameCounter >= 5)
																{
																	this.frameCounter = 0;
																	if (++this.frame >= 4)
																	{
																		this.frame = 0;
																		return;
																	}
																}
															}
															else
															{
																if (this.aiStyle == 80)
																{
																	if (this.ai[0] == 0f && this.ai[1] > 0f)
																	{
																		this.ai[1] -= 1f;
																	}
																	else if (this.ai[0] == 0f && this.ai[1] == 0f)
																	{
																		this.ai[0] = 1f;
																		this.ai[1] = (float)Player.FindClosest(this.position, this.width, this.height);
																		this.netUpdate = true;
																		float num761 = this.velocity.Length();
																		this.velocity = Vector2.Normalize(this.velocity) * (num761 + 4f);
																	}
																	else if (this.ai[0] == 1f)
																	{
																		this.tileCollide = true;
																		this.localAI[1] += 1f;
																		float num764 = 180f;
																		float num765 = 0f;
																		float num766 = 30f;
																		if (this.localAI[1] == num764)
																		{
																			this.Kill();
																			return;
																		}
																		if (this.localAI[1] >= num765 && this.localAI[1] < num765 + num766)
																		{
																			Vector2 v3 = Main.player[(int)this.ai[1]].Center - base.Center;
																			float num767 = this.velocity.ToRotation();
																			float num768 = v3.ToRotation();
																			double num769 = (double)(num768 - num767);
																			if (num769 > 3.1415926535897931)
																			{
																				num769 -= 6.2831853071795862;
																			}
																			if (num769 < -3.1415926535897931)
																			{
																				num769 += 6.2831853071795862;
																			}
																			this.velocity = this.velocity.RotatedBy(num769 * 0.20000000298023224, default(Vector2));
																		}
																	}
																	this.rotation = this.velocity.ToRotation() + 1.57079637f;
																	if (++this.frameCounter >= 3)
																	{
																		this.frameCounter = 0;
																		if (++this.frame >= 3)
																		{
																			this.frame = 0;
																		}
																	}
																	for (int num774 = 0; num774 < 255; num774++)
																	{
																		Player player4 = Main.player[num774];
																		if (player4.active && !player4.dead && Vector2.Distance(player4.Center, base.Center) <= 42f)
																		{
																			this.Kill();
																			return;
																		}
																	}
																	return;
																}
																if (this.aiStyle == 81)
																{
																	int num775 = this.penetrate;
																	if (this.ai[0] == 0f)
																	{
																		this.tileCollide = true;
																		this.localAI[0] += 1f;
																		float num780 = 0.01f;
																		int num781 = 5;
																		int num782 = num781 * 15;
																		int num783 = 0;
																		if (this.localAI[0] > 7f)
																		{
																			if (this.localAI[1] == 0f)
																			{
																				this.scale -= num780;
																				this.alpha += num781;
																				if (this.alpha > num782)
																				{
																					this.alpha = num782;
																					this.localAI[1] = 1f;
																				}
																			}
																			else if (this.localAI[1] == 1f)
																			{
																				this.scale += num780;
																				this.alpha -= num781;
																				if (this.alpha <= num783)
																				{
																					this.alpha = num783;
																					this.localAI[1] = 0f;
																				}
																			}
																		}
																		this.rotation = this.velocity.ToRotation() + 0.7853982f;
																	}
																	else if (this.ai[0] >= (float)1 && this.ai[0] < (float)(1 + num775))
																	{
																		this.tileCollide = false;
																		this.alpha += 15;
																		this.velocity *= 0.98f;
																		this.localAI[0] = 0f;
																		if (this.alpha >= 255)
																		{
																			if (this.ai[0] == 1f)
																			{
																				this.Kill();
																				return;
																			}
																			int num784 = -1;
																			Vector2 vector86 = base.Center;
																			float num785 = 250f;
																			for (int num786 = 0; num786 < 200; num786++)
																			{
																				NPC nPC8 = Main.npc[num786];
																				if (nPC8.CanBeChasedBy(this, false))
																				{
																					Vector2 center6 = nPC8.Center;
																					float num787 = Vector2.Distance(center6, base.Center);
																					if (num787 < num785)
																					{
																						num785 = num787;
																						vector86 = center6;
																						num784 = num786;
																					}
																				}
																			}
																			if (num784 >= 0)
																			{
																				this.netUpdate = true;
																				this.ai[0] += (float)num775;
																				this.position = vector86 + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * 100f - new Vector2((float)this.width, (float)this.height) / 2f;
																				this.velocity = Vector2.Normalize(vector86 - base.Center) * 15f;
																				this.rotation = this.velocity.ToRotation() + 0.7853982f;
																			}
																			else
																			{
																				this.Kill();
																			}
																		}
																	}
																	else if (this.ai[0] >= (float)(1 + num775) && this.ai[0] < (float)(1 + num775 * 2))
																	{
																		this.scale = 0.9f;
																		this.tileCollide = false;
																		this.rotation = this.velocity.ToRotation() + 0.7853982f;
																		this.ai[1] += 1f;
																		if (this.ai[1] >= 15f)
																		{
																			this.alpha += 51;
																			this.velocity *= 0.8f;
																			if (this.alpha >= 255)
																			{
																				this.Kill();
																			}
																		}
																		else
																		{
																			this.alpha -= 125;
																			if (this.alpha < 0)
																			{
																				this.alpha = 0;
																			}
																			this.velocity *= 0.98f;
																		}
																		this.localAI[0] += 1f;
																	}
																	return;
																}
																if (this.aiStyle == 82)
																{
																	this.alpha -= 40;
																	if (this.alpha < 0)
																	{
																		this.alpha = 0;
																	}
																	if (this.ai[0] == 0f)
																	{
																		this.localAI[0] += 1f;
																		if (this.localAI[0] >= 45f)
																		{
																			this.localAI[0] = 0f;
																			this.ai[0] = 1f;
																			this.ai[1] = -this.ai[1];
																			this.netUpdate = true;
																		}
																		this.velocity.X = this.velocity.RotatedBy((double)this.ai[1], default(Vector2)).X;
																		this.velocity.X = MathHelper.Clamp(this.velocity.X, -6f, 6f);
																		this.velocity.Y = this.velocity.Y - 0.08f;
																		if (this.velocity.Y > 0f)
																		{
																			this.velocity.Y = this.velocity.Y - 0.2f;
																		}
																		if (this.velocity.Y < -7f)
																		{
																			this.velocity.Y = -7f;
																		}
																	}
																	else if (this.ai[0] == 1f)
																	{
																		this.localAI[0] += 1f;
																		if (this.localAI[0] >= 90f)
																		{
																			this.localAI[0] = 0f;
																			this.ai[0] = 2f;
																			this.ai[1] = (float)Player.FindClosest(this.position, this.width, this.height);
																			this.netUpdate = true;
																		}
																		this.velocity.X = this.velocity.RotatedBy((double)this.ai[1], default(Vector2)).X;
																		this.velocity.X = MathHelper.Clamp(this.velocity.X, -6f, 6f);
																		this.velocity.Y = this.velocity.Y - 0.08f;
																		if (this.velocity.Y > 0f)
																		{
																			this.velocity.Y = this.velocity.Y - 0.2f;
																		}
																		if (this.velocity.Y < -7f)
																		{
																			this.velocity.Y = -7f;
																		}
																	}
																	else if (this.ai[0] == 2f)
																	{
																		Vector2 vector89 = Main.player[(int)this.ai[1]].Center - base.Center;
																		if (vector89.Length() < 30f)
																		{
																			this.Kill();
																			return;
																		}
																		vector89.Normalize();
																		vector89 *= 14f;
																		vector89 = Vector2.Lerp(this.velocity, vector89, 0.6f);
																		if (vector89.Y < 6f)
																		{
																			vector89.Y = 6f;
																		}
																		float num797 = 0.4f;
																		if (this.velocity.X < vector89.X)
																		{
																			this.velocity.X = this.velocity.X + num797;
																			if (this.velocity.X < 0f && vector89.X > 0f)
																			{
																				this.velocity.X = this.velocity.X + num797;
																			}
																		}
																		else if (this.velocity.X > vector89.X)
																		{
																			this.velocity.X = this.velocity.X - num797;
																			if (this.velocity.X > 0f && vector89.X < 0f)
																			{
																				this.velocity.X = this.velocity.X - num797;
																			}
																		}
																		if (this.velocity.Y < vector89.Y)
																		{
																			this.velocity.Y = this.velocity.Y + num797;
																			if (this.velocity.Y < 0f && vector89.Y > 0f)
																			{
																				this.velocity.Y = this.velocity.Y + num797;
																			}
																		}
																		else if (this.velocity.Y > vector89.Y)
																		{
																			this.velocity.Y = this.velocity.Y - num797;
																			if (this.velocity.Y > 0f && vector89.Y < 0f)
																			{
																				this.velocity.Y = this.velocity.Y - num797;
																			}
																		}
																	}
																	this.rotation = this.velocity.ToRotation() + 1.57079637f;
																	return;
																}
																if (this.aiStyle == 83)
																{
																	if (this.alpha > 200)
																	{
																		this.alpha = 200;
																	}
																	this.alpha -= 5;
																	if (this.alpha < 0)
																	{
																		this.alpha = 0;
																	}
																	float num799 = (float)this.alpha / 255f;
																	this.scale = 1f - num799;
																	if (this.ai[0] >= 0f)
																	{
																		this.ai[0] += 1f;
																	}
																	if (this.ai[0] == -1f)
																	{
																		this.frame = 1;
																		this.extraUpdates = 1;
																	}
																	else if (this.ai[0] < 30f)
																	{
																		this.position = Main.npc[(int)this.ai[1]].Center - new Vector2((float)this.width, (float)this.height) / 2f - this.velocity;
																	}
																	else
																	{
																		this.velocity *= 0.96f;
																		if (++this.frameCounter >= 6)
																		{
																			this.frameCounter = 0;
																			if (++this.frame >= 2)
																			{
																				this.frame = 0;
																			}
																		}
																	}
																	if (this.alpha < 40)
																	{
																		for (int num800 = 0; num800 < 2; num800++)
																		{
																			float num801 = (float)Main.rand.NextDouble() * 1f - 0.5f;
																			if (num801 < -0.5f)
																			{
																				num801 = -0.5f;
																			}
																			if (num801 > 0.5f)
																			{
																				num801 = 0.5f;
																			}
																		}
																		return;
																	}
																}
																else if (this.aiStyle == 84)
																{
																	Vector2? vector91 = null;
																	if (this.velocity.HasNaNs() || this.velocity == Vector2.Zero)
																	{
																		this.velocity = -Vector2.UnitY;
																	}
																	if (this.type == 455 && Main.npc[(int)this.ai[1]].active && Main.npc[(int)this.ai[1]].type == 396)
																	{
																		Vector2 vector92 = new Vector2(27f, 59f);
																		Vector2 vector93 = Utils.Vector2FromElipse(Main.npc[(int)this.ai[1]].localAI[0].ToRotationVector2(), vector92 * Main.npc[(int)this.ai[1]].localAI[1]);
																		this.position = Main.npc[(int)this.ai[1]].Center + vector93 - new Vector2((float)this.width, (float)this.height) / 2f;
																	}
																	else if (this.type == 455 && Main.npc[(int)this.ai[1]].active && Main.npc[(int)this.ai[1]].type == 400)
																	{
																		Vector2 vector94 = new Vector2(30f, 30f);
																		Vector2 vector95 = Utils.Vector2FromElipse(Main.npc[(int)this.ai[1]].localAI[0].ToRotationVector2(), vector94 * Main.npc[(int)this.ai[1]].localAI[1]);
																		this.position = Main.npc[(int)this.ai[1]].Center + vector95 - new Vector2((float)this.width, (float)this.height) / 2f;
																	}
																	else if (this.type == 537 && Main.npc[(int)this.ai[1]].active && Main.npc[(int)this.ai[1]].type == 411)
																	{
																		Vector2 vector96 = new Vector2((float)(Main.npc[(int)this.ai[1]].direction * 6), -4f);
																		this.position = Main.npc[(int)this.ai[1]].Center + vector96 - base.Size / 2f + new Vector2(0f, -Main.npc[(int)this.ai[1]].gfxOffY);
																	}
																	else if (this.type == 461 && Main.projectile[(int)this.ai[1]].active && Main.projectile[(int)this.ai[1]].type == 460)
																	{
																		Vector2 vector97 = Vector2.Normalize(Main.projectile[(int)this.ai[1]].velocity);
																		this.position = Main.projectile[(int)this.ai[1]].Center + vector97 * 16f - new Vector2((float)this.width, (float)this.height) / 2f + new Vector2(0f, -Main.projectile[(int)this.ai[1]].gfxOffY);
																		this.velocity = Vector2.Normalize(Main.projectile[(int)this.ai[1]].velocity);
																	}
																	else if (this.type == 642 && Main.projectile[(int)this.ai[1]].active && Main.projectile[(int)this.ai[1]].type == 641)
																	{
																		base.Center = Main.projectile[(int)this.ai[1]].Center;
																		this.velocity = Vector2.Normalize(Main.projectile[(int)this.ai[1]].ai[1].ToRotationVector2());
																	}
																	else
																	{
																		if (this.type != 632 || !Main.projectile[(int)this.ai[1]].active || Main.projectile[(int)this.ai[1]].type != 633)
																		{
																			this.Kill();
																			return;
																		}
																		float num803 = (float)((int)this.ai[0]) - 2.5f;
																		Vector2 vector98 = Vector2.Normalize(Main.projectile[(int)this.ai[1]].velocity);
																		Projectile projectile = Main.projectile[(int)this.ai[1]];
																		float num804 = num803 * 0.5235988f;
																		Vector2 vector99 = Vector2.Zero;
																		float num805;
																		float num806;
																		float num807;
																		float num808;
																		if (projectile.ai[0] < 180f)
																		{
																			num805 = 1f - projectile.ai[0] / 180f;
																			num806 = 20f - projectile.ai[0] / 180f * 14f;
																			if (projectile.ai[0] < 120f)
																			{
																				num807 = 20f - 4f * (projectile.ai[0] / 120f);
																				this.Opacity = projectile.ai[0] / 120f * 0.4f;
																			}
																			else
																			{
																				num807 = 16f - 10f * ((projectile.ai[0] - 120f) / 60f);
																				this.Opacity = 0.4f + (projectile.ai[0] - 120f) / 60f * 0.6f;
																			}
																			num808 = -22f + projectile.ai[0] / 180f * 20f;
																		}
																		else
																		{
																			num805 = 0f;
																			num807 = 1.75f;
																			num806 = 6f;
																			this.Opacity = 1f;
																			num808 = -2f;
																		}
																		float num809 = (projectile.ai[0] + num803 * num807) / (num807 * 6f) * 6.28318548f;
																		num804 = Vector2.UnitY.RotatedBy((double)num809, default(Vector2)).Y * 0.5235988f * num805;
																		vector99 = (Vector2.UnitY.RotatedBy((double)num809, default(Vector2)) * new Vector2(4f, num806)).RotatedBy((double)projectile.velocity.ToRotation(), default(Vector2));
																		this.position = projectile.Center + vector98 * 16f - base.Size / 2f + new Vector2(0f, -Main.projectile[(int)this.ai[1]].gfxOffY);
																		this.position += projectile.velocity.ToRotation().ToRotationVector2() * num808;
																		this.position += vector99;
																		this.velocity = Vector2.Normalize(projectile.velocity).RotatedBy((double)num804, default(Vector2));
																		this.scale = 1.4f * (1f - num805);
																		this.damage = projectile.damage;
																		if (projectile.ai[0] >= 180f)
																		{
																			this.damage *= 3;
																			vector91 = new Vector2?(projectile.Center);
																		}
																		if (!Collision.CanHitLine(Main.player[this.owner].Center, 0, 0, projectile.Center, 0, 0))
																		{
																			vector91 = new Vector2?(Main.player[this.owner].Center);
																		}
																		this.friendly = (projectile.ai[0] > 30f);
																	}
																	if (this.velocity.HasNaNs() || this.velocity == Vector2.Zero)
																	{
																		this.velocity = -Vector2.UnitY;
																	}
																	if (this.type == 461)
																	{
																		this.ai[0] += 1f;
																		if (this.ai[0] >= 300f)
																		{
																			this.Kill();
																			return;
																		}
																		this.scale = (float)Math.Sin((double)(this.ai[0] * 3.14159274f / 300f)) * 10f;
																		if (this.scale > 1f)
																		{
																			this.scale = 1f;
																		}
																	}
																	if (this.type == 455)
																	{
																		float num810 = 1f;
																		if (Main.npc[(int)this.ai[1]].type == 400)
																		{
																			num810 = 0.4f;
																		}
																		this.localAI[0] += 1f;
																		if (this.localAI[0] >= 180f)
																		{
																			this.Kill();
																			return;
																		}
																		this.scale = (float)Math.Sin((double)(this.localAI[0] * 3.14159274f / 180f)) * 10f * num810;
																		if (this.scale > num810)
																		{
																			this.scale = num810;
																		}
																	}
																	if (this.type == 642)
																	{
																		float num811 = 1f;
																		this.localAI[0] += 1f;
																		if (this.localAI[0] >= 50f)
																		{
																			this.Kill();
																			return;
																		}
																		this.scale = (float)Math.Sin((double)(this.localAI[0] * 3.14159274f / 50f)) * 10f * num811;
																		if (this.scale > num811)
																		{
																			this.scale = num811;
																		}
																	}
																	if (this.type == 537)
																	{
																		float num812 = 0.8f;
																		this.localAI[0] += 1f;
																		if (this.localAI[0] >= 60f)
																		{
																			this.Kill();
																			return;
																		}
																		this.scale = (float)Math.Sin((double)(this.localAI[0] * 3.14159274f / 60f)) * 10f * num812;
																		if (this.scale > num812)
																		{
																			this.scale = num812;
																		}
																	}
																	float num813 = this.velocity.ToRotation();
																	if (this.type == 455)
																	{
																		num813 += this.ai[0];
																	}
																	this.rotation = num813 - 1.57079637f;
																	this.velocity = num813.ToRotationVector2();
																	float num814 = 0f;
																	float num815 = 0f;
																	Vector2 vector100 = base.Center;
																	if (vector91.HasValue)
																	{
																		vector100 = vector91.Value;
																	}
																	if (this.type == 455)
																	{
																		num814 = 3f;
																		num815 = (float)this.width;
																	}
																	else if (this.type == 461)
																	{
																		num814 = 2f;
																		num815 = 0f;
																	}
																	else if (this.type == 642)
																	{
																		num814 = 2f;
																		num815 = 0f;
																	}
																	else if (this.type == 632)
																	{
																		num814 = 2f;
																		num815 = 0f;
																	}
																	else if (this.type == 537)
																	{
																		num814 = 2f;
																		num815 = 0f;
																	}
																	float[] array3 = new float[(int)num814];
																	int num816 = 0;
																	while ((float)num816 < num814)
																	{
																		float num817 = (float)num816 / (num814 - 1f);
																		Vector2 vector101 = vector100 + this.velocity.RotatedBy(1.5707963705062866, default(Vector2)) * (num817 - 0.5f) * num815 * this.scale;
																		int num818 = (int)vector101.X / 16;
																		int num819 = (int)vector101.Y / 16;
																		Vector2 vector102 = vector101 + this.velocity * 16f * 150f;
																		int num820 = (int)vector102.X / 16;
																		int num821 = (int)vector102.Y / 16;
																		Tuple<int, int> tuple;
																		float num822;
																		if (!Collision.TupleHitLine(num818, num819, num820, num821, 0, 0, new List<Tuple<int, int>>(), out tuple))
																		{
																			num822 = new Vector2((float)Math.Abs(num818 - tuple.Item1), (float)Math.Abs(num819 - tuple.Item2)).Length() * 16f;
																		}
																		else if (tuple.Item1 == num820 && tuple.Item2 == num821)
																		{
																			num822 = 2400f;
																		}
																		else
																		{
																			num822 = new Vector2((float)Math.Abs(num818 - tuple.Item1), (float)Math.Abs(num819 - tuple.Item2)).Length() * 16f;
																		}
																		array3[num816] = num822;
																		num816++;
																	}
																	float num823 = 0f;
																	for (int num824 = 0; num824 < array3.Length; num824++)
																	{
																		num823 += array3[num824];
																	}
																	num823 /= num814;
																	float num825 = 0.5f;
																	if (this.type == 632)
																	{
																		num825 = 0.75f;
																	}
																	this.localAI[1] = MathHelper.Lerp(this.localAI[1], num823, num825);
																	if (this.type == 455)
																	{
																		DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
																	}
																	else if (this.type == 642)
																	{
																		DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
																	}
																	if (this.type == 461)
																	{
																		DelegateMethods.v3_1 = new Vector3(0.4f, 0.85f, 0.9f);
																	}
																	if (this.type == 537)
																	{
																		DelegateMethods.v3_1 = new Vector3(0.4f, 0.85f, 0.9f);
																	}
																	if (this.type == 632 && Math.Abs(this.localAI[1] - num823) < 100f && this.scale > 0.15f)
																	{
																		float prismHue = this.GetPrismHue(this.ai[0]);
																		Color color = Main.HslToRgb(prismHue, 1f, 0.5f);
																		color.A = 0;
																		Vector2 vector115 = base.Center + this.velocity * (this.localAI[1] - 14.5f * this.scale);
																		float x = Main.RgbToHsl(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB)).X;
																		DelegateMethods.v3_1 = color.ToVector3() * 0.3f;
																		return;
																	}
																}
																else if (this.aiStyle == 85)
																{
																	Vector2 vector118 = new Vector2(0f, 216f);
																	this.alpha -= 15;
																	if (this.alpha < 0)
																	{
																		this.alpha = 0;
																	}
																	int num851 = (int)Math.Abs(this.ai[0]) - 1;
																	int num852 = (int)this.ai[1];
																	if (!Main.npc[num851].active || Main.npc[num851].type != 396)
																	{
																		this.Kill();
																		return;
																	}
																	this.localAI[0] += 1f;
																	if (this.localAI[0] >= 330f && this.ai[0] > 0f && Main.netMode != 1)
																	{
																		this.ai[0] *= -1f;
																		this.netUpdate = true;
																	}
																	if (Main.netMode != 1 && this.ai[0] > 0f && (!Main.player[(int)this.ai[1]].active || Main.player[(int)this.ai[1]].dead))
																	{
																		this.ai[0] *= -1f;
																		this.netUpdate = true;
																	}
																	this.rotation = (Main.npc[(int)Math.Abs(this.ai[0]) - 1].Center - Main.player[(int)this.ai[1]].Center + vector118).ToRotation() + 1.57079637f;
																	if (this.ai[0] > 0f)
																	{
																		Vector2 vector119 = Main.player[(int)this.ai[1]].Center - base.Center;
																		if (vector119.X != 0f || vector119.Y != 0f)
																		{
																			this.velocity = Vector2.Normalize(vector119) * Math.Min(16f, vector119.Length());
																		}
																		else
																		{
																			this.velocity = Vector2.Zero;
																		}
																		if (vector119.Length() < 20f && this.localAI[1] == 0f)
																		{
																			this.localAI[1] = 1f;
																			Main.player[num852].AddBuff(145, 600, true);
																			return;
																		}
																	}
																	else
																	{
																		if (this.localAI[1] == 1f)
																		{
																			int num853 = Main.player[num852].HasBuff(145);
																			if (num853 != -1)
																			{
																				Main.player[num852].DelBuff(num853);
																			}
																		}
																		Vector2 vector120 = Main.npc[(int)Math.Abs(this.ai[0]) - 1].Center - base.Center + vector118;
																		if (vector120.X != 0f || vector120.Y != 0f)
																		{
																			this.velocity = Vector2.Normalize(vector120) * Math.Min(16f, vector120.Length());
																		}
																		else
																		{
																			this.velocity = Vector2.Zero;
																		}
																		if (vector120.Length() < 20f)
																		{
																			this.Kill();
																			return;
																		}
																	}
																}
																else if (this.aiStyle == 86)
																{
																	if (this.localAI[1] == 0f)
																	{
																		this.localAI[1] = 1f;
																	}
																	this.ai[0] += 1f;
																	if (this.ai[1] == 1f)
																	{
																		if (this.ai[0] >= 130f)
																		{
																			this.alpha += 10;
																		}
																		else
																		{
																			this.alpha -= 10;
																		}
																		if (this.alpha < 0)
																		{
																			this.alpha = 0;
																		}
																		if (this.alpha > 255)
																		{
																			this.alpha = 255;
																		}
																		if (this.ai[0] >= 150f)
																		{
																			this.Kill();
																			return;
																		}
																		if (this.ai[0] % 30f == 0f && Main.netMode != 1)
																		{
																			Vector2 vector121 = this.rotation.ToRotationVector2();
																			Projectile.NewProjectile(base.Center.X, base.Center.Y, vector121.X, vector121.Y, 464, this.damage, this.knockBack, this.owner, 0f, 0f);
																		}
																		this.rotation += 0.104719758f;
																		return;
																	}
																	else
																	{
																		this.position -= this.velocity;
																		if (this.ai[0] >= 40f)
																		{
																			this.alpha += 3;
																		}
																		else
																		{
																			this.alpha -= 40;
																		}
																		if (this.alpha < 0)
																		{
																			this.alpha = 0;
																		}
																		if (this.alpha > 255)
																		{
																			this.alpha = 255;
																		}
																		if (this.ai[0] >= 45f)
																		{
																			this.Kill();
																			return;
																		}
																		Vector2 vector122 = new Vector2(0f, -720f).RotatedBy((double)this.velocity.ToRotation(), default(Vector2));
																		float num854 = this.ai[0] % 45f / 45f;
																		Vector2 spinningpoint = vector122 * num854;
																		return;
																	}
																}
																else
																{
																	if (this.aiStyle == 87)
																	{
																		this.position.Y = this.ai[0];
																		this.height = (int)this.ai[1];
																		if (base.Center.X > Main.player[this.owner].Center.X)
																		{
																			this.direction = 1;
																		}
																		else
																		{
																			this.direction = -1;
																		}
																		this.velocity.X = (float)this.direction * 1E-06f;
																		if (this.owner == Main.myPlayer)
																		{
																			for (int num858 = 0; num858 < 1000; num858++)
																			{
																				if (Main.projectile[num858].active && num858 != this.whoAmI && Main.projectile[num858].type == this.type && Main.projectile[num858].owner == this.owner && Main.projectile[num858].timeLeft > this.timeLeft)
																				{
																					this.Kill();
																					return;
																				}
																			}
																		}
																		return;
																	}
																	if (this.aiStyle == 88)
																	{
																		if (this.type == 465)
																		{
																			if (this.localAI[1] == 0f)
																			{
																				this.localAI[1] = 1f;
																			}
																			if (this.ai[0] < 180f)
																			{
																				this.alpha -= 5;
																				if (this.alpha < 0)
																				{
																					this.alpha = 0;
																				}
																			}
																			else
																			{
																				this.alpha += 5;
																				if (this.alpha > 255)
																				{
																					this.alpha = 255;
																					this.Kill();
																					return;
																				}
																			}
																			this.ai[0] += 1f;
																			if (this.ai[0] % 30f == 0f && this.ai[0] < 180f && Main.netMode != 1)
																			{
																				int[] array4 = new int[5];
																				Vector2[] array5 = new Vector2[5];
																				int num862 = 0;
																				float num863 = 2000f;
																				for (int num864 = 0; num864 < 255; num864++)
																				{
																					if (Main.player[num864].active && !Main.player[num864].dead)
																					{
																						Vector2 center9 = Main.player[num864].Center;
																						float num865 = Vector2.Distance(center9, base.Center);
																						if (num865 < num863 && Collision.CanHit(base.Center, 1, 1, center9, 1, 1))
																						{
																							array4[num862] = num864;
																							array5[num862] = center9;
																							if (++num862 >= array5.Length)
																							{
																								break;
																							}
																						}
																					}
																				}
																				for (int num866 = 0; num866 < num862; num866++)
																				{
																					Vector2 vector124 = array5[num866] - base.Center;
																					float ai = (float)Main.rand.Next(100);
																					Vector2 vector125 = Vector2.Normalize(vector124.RotatedByRandom(0.78539818525314331)) * 7f;
																					Projectile.NewProjectile(base.Center.X, base.Center.Y, vector125.X, vector125.Y, 466, this.damage, 0f, Main.myPlayer, vector124.ToRotation(), ai);
																				}
																			}
																			if (++this.frameCounter >= 4)
																			{
																				this.frameCounter = 0;
																				if (++this.frame >= Main.projFrames[this.type])
																				{
																					this.frame = 0;
																				}
																			}
																			if (this.alpha < 150 && this.ai[0] < 180f)
																			{
																				for (int num867 = 0; num867 < 1; num867++)
																				{
																					float num868 = (float)Main.rand.NextDouble() * 1f - 0.5f;
																					if (num868 < -0.5f)
																					{
																						num868 = -0.5f;
																					}
																					if (num868 > 0.5f)
																					{
																						num868 = 0.5f;
																					}
																				}
																				for (int num870 = 0; num870 < 1; num870++)
																				{
																					float num871 = (float)Main.rand.NextDouble() * 1f - 0.5f;
																					if (num871 < -0.5f)
																					{
																						num871 = -0.5f;
																					}
																					if (num871 > 0.5f)
																					{
																						num871 = 0.5f;
																					}
																				}
																				return;
																			}
																		}
																		else if (this.type == 466)
																		{
																			this.frameCounter++;
																			if (this.velocity == Vector2.Zero)
																			{
																				if (this.frameCounter >= this.extraUpdates * 2)
																				{
																					this.frameCounter = 0;
																					bool flag34 = true;
																					for (int num873 = 1; num873 < this.oldPos.Length; num873++)
																					{
																						if (this.oldPos[num873] != this.oldPos[0])
																						{
																							flag34 = false;
																						}
																					}
																					if (flag34)
																					{
																						this.Kill();
																						return;
																					}
																				}
																				if (Main.rand.Next(this.extraUpdates) == 0)
																				{
																					if (Main.rand.Next(5) == 0)
																					{
																						return;
																					}
																				}
																			}
																			else if (this.frameCounter >= this.extraUpdates * 2)
																			{
																				this.frameCounter = 0;
																				float num879 = this.velocity.Length();
																				Random random = new Random((int)this.ai[1]);
																				int num880 = 0;
																				Vector2 spinningpoint2 = -Vector2.UnitY;
																				Vector2 vector130;
																				do
																				{
																					int num881 = random.Next();
																					this.ai[1] = (float)num881;
																					num881 %= 100;
																					float f = (float)num881 / 100f * 6.28318548f;
																					vector130 = f.ToRotationVector2();
																					if (vector130.Y > 0f)
																					{
																						vector130.Y *= -1f;
																					}
																					bool flag35 = false;
																					if (vector130.Y > -0.02f)
																					{
																						flag35 = true;
																					}
																					if (vector130.X * (float)(this.extraUpdates + 1) * 2f * num879 + this.localAI[0] > 40f)
																					{
																						flag35 = true;
																					}
																					if (vector130.X * (float)(this.extraUpdates + 1) * 2f * num879 + this.localAI[0] < -40f)
																					{
																						flag35 = true;
																					}
																					if (!flag35)
																					{
																						goto IL_2363F;
																					}
																				}
																				while (num880++ < 100);
																				this.velocity = Vector2.Zero;
																				this.localAI[1] = 1f;
																				goto IL_23647;
																				IL_2363F:
																				spinningpoint2 = vector130;
																				IL_23647:
																				if (this.velocity != Vector2.Zero)
																				{
																					this.localAI[0] += spinningpoint2.X * (float)(this.extraUpdates + 1) * 2f * num879;
																					this.velocity = spinningpoint2.RotatedBy((double)(this.ai[0] + 1.57079637f), default(Vector2)) * num879;
																					this.rotation = this.velocity.ToRotation() + 1.57079637f;
																					return;
																				}
																			}
																		}
																		else if (this.type == 580)
																		{
																			if (this.localAI[1] == 0f && this.ai[0] >= 900f)
																			{
																				this.ai[0] -= 1000f;
																				this.localAI[1] = -1f;
																			}
																			this.frameCounter++;
																			if (this.velocity == Vector2.Zero)
																			{
																				if (this.frameCounter >= this.extraUpdates * 2)
																				{
																					this.frameCounter = 0;
																					bool flag36 = true;
																					for (int num882 = 1; num882 < this.oldPos.Length; num882++)
																					{
																						if (this.oldPos[num882] != this.oldPos[0])
																						{
																							flag36 = false;
																						}
																					}
																					if (flag36)
																					{
																						this.Kill();
																						return;
																					}
																				}
																				if (Main.rand.Next(this.extraUpdates) == 0 && (this.velocity != Vector2.Zero || Main.rand.Next((this.localAI[1] == 2f) ? 2 : 6) == 0))
																				{
																					if (Main.rand.Next(5) == 0)
																					{
																						return;
																					}
																				}
																			}
																			else if (this.frameCounter >= this.extraUpdates * 2)
																			{
																				this.frameCounter = 0;
																				float num888 = this.velocity.Length();
																				Random random2 = new Random((int)this.ai[1]);
																				int num889 = 0;
																				Vector2 spinningpoint3 = -Vector2.UnitY;
																				Vector2 vector133;
																				do
																				{
																					int num890 = random2.Next();
																					this.ai[1] = (float)num890;
																					num890 %= 100;
																					float f2 = (float)num890 / 100f * 6.28318548f;
																					vector133 = f2.ToRotationVector2();
																					if (vector133.Y > 0f)
																					{
																						vector133.Y *= -1f;
																					}
																					bool flag37 = false;
																					if (vector133.Y > -0.02f)
																					{
																						flag37 = true;
																					}
																					if (vector133.X * (float)(this.extraUpdates + 1) * 2f * num888 + this.localAI[0] > 40f)
																					{
																						flag37 = true;
																					}
																					if (vector133.X * (float)(this.extraUpdates + 1) * 2f * num888 + this.localAI[0] < -40f)
																					{
																						flag37 = true;
																					}
																					if (!flag37)
																					{
																						goto IL_23BA3;
																					}
																				}
																				while (num889++ < 100);
																				this.velocity = Vector2.Zero;
																				if (this.localAI[1] < 1f)
																				{
																					this.localAI[1] += 2f;
																					goto IL_23BAB;
																				}
																				goto IL_23BAB;
																				IL_23BA3:
																				spinningpoint3 = vector133;
																				IL_23BAB:
																				if (this.velocity != Vector2.Zero)
																				{
																					this.localAI[0] += spinningpoint3.X * (float)(this.extraUpdates + 1) * 2f * num888;
																					this.velocity = spinningpoint3.RotatedBy((double)(this.ai[0] + 1.57079637f), default(Vector2)) * num888;
																					this.rotation = this.velocity.ToRotation() + 1.57079637f;
																					if (Main.rand.Next(4) == 0 && Main.netMode != 1 && this.localAI[1] == 0f)
																					{
																						float num891 = (float)Main.rand.Next(-3, 4) * 1.04719758f / 3f;
																						Vector2 vector134 = this.ai[0].ToRotationVector2().RotatedBy((double)num891, default(Vector2)) * this.velocity.Length();
																						if (!Collision.CanHitLine(base.Center, 0, 0, base.Center + vector134 * 50f, 0, 0))
																						{
																							Projectile.NewProjectile(base.Center.X - vector134.X, base.Center.Y - vector134.Y, vector134.X, vector134.Y, this.type, this.damage, this.knockBack, this.owner, vector134.ToRotation() + 1000f, this.ai[1]);
																							return;
																						}
																					}
																				}
																			}
																		}
																	}
																	else if (this.aiStyle == 89)
																	{
																		if (this.ai[1] == -1f)
																		{
																			this.alpha += 12;
																		}
																		else if (this.ai[0] < 300f)
																		{
																			this.alpha -= 5;
																		}
																		else
																		{
																			this.alpha += 12;
																		}
																		if (this.alpha < 0)
																		{
																			this.alpha = 0;
																		}
																		if (this.alpha > 255)
																		{
																			this.alpha = 255;
																		}
																		this.scale = 1f - (float)this.alpha / 255f;
																		this.scale *= 0.6f;
																		this.rotation += 0.0149599658f;
																		if (this.localAI[1] == 0f)
																		{
																			this.localAI[1] = 1f;
																		}
																		this.ai[0] += 1f;
																		if (this.ai[0] >= 60f)
																		{
																		}
																		if (this.ai[0] == 300f && this.ai[1] != -1f && Main.netMode != 1)
																		{
																			if (!NPC.AnyNPCs(454))
																			{
																				this.ai[1] = (float)NPC.NewNPC((int)base.Center.X, (int)base.Center.Y, 454, 0, 0f, 0f, 0f, 0f, 255);
																			}
																			else
																			{
																				this.ai[1] = (float)NPC.NewNPC((int)base.Center.X, (int)base.Center.Y, 521, 0, 0f, 0f, 0f, 0f, 255);
																			}
																		}
																		else if (this.ai[0] == 320f)
																		{
																			this.Kill();
																			return;
																		}
																		bool flag38 = false;
																		if (this.ai[1] == -1f)
																		{
																			if (this.alpha == 255)
																			{
																				flag38 = true;
																			}
																		}
																		else
																		{
																			flag38 = (this.ai[1] < 0f || !Main.npc[(int)this.ai[1]].active);
																			if ((flag38 || Main.npc[(int)this.ai[1]].type != 439) && (flag38 || Main.npc[(int)this.ai[1]].type != 454) && (flag38 || Main.npc[(int)this.ai[1]].type != 521))
																			{
																				flag38 = true;
																			}
																		}
																		if (flag38)
																		{
																			this.Kill();
																			return;
																		}
																	}
																	else if (this.aiStyle == 90)
																	{
																		if (Main.player[this.owner].dead)
																		{
																			this.Kill();
																		}
																		if (Main.myPlayer == this.owner && Main.player[this.owner].magicLantern)
																		{
																			this.timeLeft = 2;
																		}
																		if (this.tileCollide)
																		{
																			if (!Collision.CanHit(this.position, this.width, this.height, Main.player[this.owner].Center, 1, 1))
																			{
																				this.tileCollide = false;
																			}
																			else if (!Collision.SolidCollision(this.position, this.width, this.height) && Collision.CanHitLine(this.position, this.width, this.height, Main.player[this.owner].Center, 1, 1))
																			{
																				this.tileCollide = true;
																			}
																		}
																		this.direction = Main.player[this.owner].direction;
																		this.spriteDirection = this.direction;
																		this.localAI[0] += 1f;
																		if (this.localAI[0] >= 10f)
																		{
																			this.localAI[0] = 0f;
																		}
																		Vector2 vector138 = Main.player[this.owner].Center - base.Center;
																		vector138.X += (float)(40 * this.direction);
																		vector138.Y -= 40f;
																		float num905 = vector138.Length();
																		if (num905 > 1000f)
																		{
																			base.Center = Main.player[this.owner].Center;
																		}
																		float num906 = 3f;
																		float num907 = 4f;
																		if (num905 > 200f)
																		{
																			num907 += (num905 - 200f) * 0.1f;
																			this.tileCollide = false;
																		}
																		if (num905 < num907)
																		{
																			this.velocity *= 0.25f;
																			num907 = num905;
																		}
																		if (vector138.X != 0f || vector138.Y != 0f)
																		{
																			vector138.Normalize();
																			vector138 *= num907;
																		}
																		this.velocity = (this.velocity * (num906 - 1f) + vector138) / num906;
																		if (this.velocity.Length() > 6f)
																		{
																			float num908 = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
																			if ((double)Math.Abs(this.rotation - num908) >= 3.14)
																			{
																				if (num908 < this.rotation)
																				{
																					this.rotation -= 6.28f;
																				}
																				else
																				{
																					this.rotation += 6.28f;
																				}
																			}
																			this.rotation = (this.rotation * 4f + num908) / 5f;
																			this.frameCounter++;
																			if (this.frameCounter > 4)
																			{
																				this.frameCounter = 0;
																				this.frame++;
																				if (this.frame > 7)
																				{
																					this.frame = 4;
																				}
																			}
																			if (this.frame < 4)
																			{
																				this.frame = 7;
																				return;
																			}
																		}
																		else
																		{
																			if ((double)this.rotation > 3.14)
																			{
																				this.rotation -= 6.28f;
																			}
																			if ((double)this.rotation > -0.01 && (double)this.rotation < 0.01)
																			{
																				this.rotation = 0f;
																			}
																			else
																			{
																				this.rotation *= 0.9f;
																			}
																			this.frameCounter++;
																			if (this.frameCounter > 6)
																			{
																				this.frameCounter = 0;
																				this.frame++;
																				if (this.frame > 3)
																				{
																					this.frame = 0;
																					return;
																				}
																			}
																		}
																	}
																	else if (this.aiStyle == 91)
																	{
																		Vector2 center10 = base.Center;
																		this.scale = 1f - this.localAI[0];
																		this.width = (int)(20f * this.scale);
																		this.height = this.width;
																		this.position.X = center10.X - (float)(this.width / 2);
																		this.position.Y = center10.Y - (float)(this.height / 2);
																		if ((double)this.localAI[0] < 0.1)
																		{
																			this.localAI[0] += 0.01f;
																		}
																		else
																		{
																			this.localAI[0] += 0.025f;
																		}
																		if (this.localAI[0] >= 0.95f)
																		{
																			this.Kill();
																		}
																		this.velocity.X = this.velocity.X + this.ai[0] * 1.5f;
																		this.velocity.Y = this.velocity.Y + this.ai[1] * 1.5f;
																		if (this.velocity.Length() > 16f)
																		{
																			this.velocity.Normalize();
																			this.velocity *= 16f;
																		}
																		this.ai[0] *= 1.05f;
																		this.ai[1] *= 1.05f;
																		if (this.scale < 1f)
																		{
																			return;
																		}
																	}
																	else
																	{
																		if (this.aiStyle == 92)
																		{
																			this.tileCollide = false;
																			this.ai[1] += 1f;
																			if (this.ai[1] > 60f)
																			{
																				this.ai[0] += 10f;
																			}
																			if (this.ai[0] > 255f)
																			{
																				this.Kill();
																				this.ai[0] = 255f;
																			}
																			this.alpha = (int)(100.0 + (double)this.ai[0] * 0.7);
																			this.rotation += this.velocity.X * 0.1f;
																			this.rotation += (float)this.direction * 0.003f;
																			this.velocity *= 0.96f;
																			Rectangle rectangle12 = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
																			for (int num911 = 0; num911 < 1000; num911++)
																			{
																				if (num911 != this.whoAmI && Main.projectile[num911].active && Main.projectile[num911].type >= 511 && Main.projectile[num911].type <= 513)
																				{
																					Rectangle rectangle13 = new Rectangle((int)Main.projectile[num911].position.X, (int)Main.projectile[num911].position.Y, Main.projectile[num911].width, Main.projectile[num911].height);
																					if (rectangle12.Intersects(rectangle13))
																					{
																						Vector2 vector139 = Main.projectile[num911].Center - base.Center;
																						if (vector139.X == 0f && vector139.Y == 0f)
																						{
																							if (num911 < this.whoAmI)
																							{
																								vector139.X = -1f;
																								vector139.Y = 1f;
																							}
																							else
																							{
																								vector139.X = 1f;
																								vector139.Y = -1f;
																							}
																						}
																						vector139.Normalize();
																						vector139 *= 0.005f;
																						this.velocity -= vector139;
																						Main.projectile[num911].velocity += vector139;
																					}
																				}
																			}
																			return;
																		}
																		if (this.aiStyle == 93)
																		{
																			if (this.alpha > 0)
																			{
																				this.alpha -= 25;
																				if (this.alpha <= 0)
																				{
																					this.alpha = 0;
																				}
																			}
																			if (this.velocity.Y > 18f)
																			{
																				this.velocity.Y = 18f;
																			}
																			if (this.ai[0] == 0f)
																			{
																				this.ai[1] += 1f;
																				if (this.ai[1] > 20f)
																				{
																					this.velocity.Y = this.velocity.Y + 0.1f;
																					this.velocity.X = this.velocity.X * 0.992f;
																				}
																				this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
																				return;
																			}
																			this.tileCollide = false;
																			if (this.ai[0] == 1f)
																			{
																				this.tileCollide = false;
																				this.velocity *= 0.6f;
																			}
																			else
																			{
																				this.tileCollide = false;
																				int num912 = (int)(-(int)this.ai[0]);
																				num912--;
																				this.position = Main.npc[num912].Center - this.velocity;
																				this.position.X = this.position.X - (float)(this.width / 2);
																				this.position.Y = this.position.Y - (float)(this.height / 2);
																				if (!Main.npc[num912].active || Main.npc[num912].life < 0)
																				{
																					this.tileCollide = true;
																					this.ai[0] = 0f;
																					this.ai[1] = 20f;
																					this.velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
																					this.velocity.Normalize();
																					this.velocity *= 6f;
																					this.netUpdate = true;
																				}
																				else if (this.velocity.Length() > (float)((Main.npc[num912].width + Main.npc[num912].height) / 3))
																				{
																					this.velocity *= 0.99f;
																				}
																			}
																			if (this.ai[0] != 0f)
																			{
																				this.ai[1] += 1f;
																				if (this.ai[1] > 90f)
																				{
																					this.Kill();
																					return;
																				}
																			}
																		}
																		else
																		{
																			if (this.aiStyle == 94)
																			{
																				if (++this.frameCounter >= 4)
																				{
																					this.frameCounter = 0;
																					if (++this.frame >= Main.projFrames[this.type])
																					{
																						this.frame = 0;
																					}
																				}
																				this.ai[0] += 1f;
																				if (this.ai[0] <= 40f)
																				{
																					this.alpha -= 5;
																					if (this.alpha < 0)
																					{
																						this.alpha = 0;
																					}
																					this.velocity *= 0.85f;
																					if (this.ai[0] == 40f)
																					{
																						this.netUpdate = true;
																						switch (Main.rand.Next(3))
																						{
																							case 0:
																								this.ai[1] = 10f;
																								break;
																							case 1:
																								this.ai[1] = 15f;
																								break;
																							case 2:
																								this.ai[1] = 30f;
																								break;
																						}
																					}
																				}
																				else if (this.ai[0] <= 60f)
																				{
																					this.velocity = Vector2.Zero;
																					if (this.ai[0] == 60f)
																					{
																						this.netUpdate = true;
																					}
																				}
																				else if (this.ai[0] <= 210f)
																				{
																					if (Main.netMode != 1 && (this.localAI[0] += 1f) >= this.ai[1])
																					{
																						this.localAI[0] = 0f;
																						int num913 = Item.NewItem((int)base.Center.X, (int)base.Center.Y, 0, 0, 73, 1, false, 0, false);
																						Main.item[num913].velocity = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * new Vector2(3f, 2f) * (Main.rand.NextFloat() * 0.5f + 0.5f) - Vector2.UnitY * 1f;
																					}
																					if (this.ai[0] == 210f)
																					{
																						this.netUpdate = true;
																					}
																				}
																				else
																				{
																					this.scale -= 0.0333333351f;
																					this.alpha += 15;
																					if (this.ai[0] == 239f)
																					{
																						this.netUpdate = true;
																					}
																					if (this.ai[0] == 240f)
																					{
																						this.Kill();
																					}
																				}
																				return;
																			}
																			if (this.aiStyle == 95)
																			{
																				if (this.localAI[0] > 2f)
																				{
																					this.alpha -= 20;
																					if (this.alpha < 100)
																					{
																						this.alpha = 100;
																					}
																				}
																				else
																				{
																					this.localAI[0] += 1f;
																				}
																				if (this.ai[0] > 30f)
																				{
																					if (this.velocity.Y > -8f)
																					{
																						this.velocity.Y = this.velocity.Y - 0.05f;
																					}
																					this.velocity.X = this.velocity.X * 0.98f;
																				}
																				else
																				{
																					this.ai[0] += 1f;
																				}
																				this.rotation = this.velocity.X * 0.1f;
																				if (this.wet)
																				{
																					if (this.velocity.Y > 0f)
																					{
																						this.velocity.Y = this.velocity.Y * 0.98f;
																					}
																					if (this.velocity.Y > -8f)
																					{
																						this.velocity.Y = this.velocity.Y - 0.2f;
																					}
																					this.velocity.X = this.velocity.X * 0.94f;
																					return;
																				}
																			}
																			else
																			{
																				if (this.aiStyle == 96)
																				{
																					this.ai[0] += 0.6f;
																					if (this.ai[0] > 500f)
																					{
																						this.Kill();
																					}
																					this.velocity.Y = this.velocity.Y + 0.008f;
																					return;
																				}
																				if (this.aiStyle == 97)
																				{
																					this.frameCounter++;
																					float num921 = 4f;
																					if ((float)this.frameCounter < num921 * 1f)
																					{
																						this.frame = 0;
																					}
																					else if ((float)this.frameCounter < num921 * 2f)
																					{
																						this.frame = 1;
																					}
																					else if ((float)this.frameCounter < num921 * 3f)
																					{
																						this.frame = 2;
																					}
																					else if ((float)this.frameCounter < num921 * 4f)
																					{
																						this.frame = 3;
																					}
																					else if ((float)this.frameCounter < num921 * 5f)
																					{
																						this.frame = 4;
																					}
																					else if ((float)this.frameCounter < num921 * 6f)
																					{
																						this.frame = 3;
																					}
																					else if ((float)this.frameCounter < num921 * 7f)
																					{
																						this.frame = 2;
																					}
																					else if ((float)this.frameCounter < num921 * 8f)
																					{
																						this.frame = 1;
																					}
																					else
																					{
																						this.frameCounter = 0;
																						this.frame = 0;
																					}
																					if (this.owner == Main.myPlayer)
																					{
																						for (int num922 = 0; num922 < 1000; num922++)
																						{
																							if (num922 != this.whoAmI && Main.projectile[num922].active && Main.projectile[num922].owner == this.owner && Main.projectile[num922].type == this.type)
																							{
																								if (this.timeLeft >= Main.projectile[num922].timeLeft)
																								{
																									Main.projectile[num922].Kill();
																								}
																								else
																								{
																									this.Kill();
																								}
																							}
																						}
																					}
																					if (this.ai[0] == 0f)
																					{
																						if ((double)this.velocity.Length() < 0.1)
																						{
																							this.velocity.X = 0f;
																							this.velocity.Y = 0f;
																							this.ai[0] = 1f;
																							this.ai[1] = 45f;
																							return;
																						}
																						this.velocity *= 0.94f;
																						if (this.velocity.X < 0f)
																						{
																							this.direction = -1;
																						}
																						else
																						{
																							this.direction = 1;
																						}
																						this.spriteDirection = this.direction;
																						return;
																					}
																					else
																					{
																						if (Main.player[this.owner].Center.X < base.Center.X)
																						{
																							this.direction = -1;
																						}
																						else
																						{
																							this.direction = 1;
																						}
																						this.spriteDirection = this.direction;
																						this.ai[1] += 1f;
																						float num923 = 0.005f;
																						if (this.ai[1] > 0f)
																						{
																							this.velocity.Y = this.velocity.Y - num923;
																						}
																						else
																						{
																							this.velocity.Y = this.velocity.Y + num923;
																						}
																						if (this.ai[1] >= 90f)
																						{
																							this.ai[1] *= -1f;
																							return;
																						}
																					}
																				}
																				else if (this.aiStyle == 98)
																				{
																					Vector2 vector142 = new Vector2(this.ai[0], this.ai[1]);
																					Vector2 vector143 = vector142 - base.Center;
																					if (vector143.Length() < this.velocity.Length())
																					{
																						this.Kill();
																						return;
																					}
																					vector143.Normalize();
																					vector143 *= 15f;
																					this.velocity = Vector2.Lerp(this.velocity, vector143, 0.1f);
																					return;
																				}
																				else
																				{
																					if (this.aiStyle == 99 && this.type >= 556 && this.type <= 561)
																					{
																						this.AI_099_1();
																						return;
																					}
																					if (this.aiStyle == 99)
																					{
																						this.AI_099_2();
																						return;
																					}
																					if (this.aiStyle == 100)
																					{
																						Player player5 = Main.player[this.owner];
																						Vector2 zero2 = Vector2.Zero;
																						if (this.type == 535)
																						{
																							zero2.X = (float)player5.direction * 6f;
																							zero2.Y = player5.gravDir * -14f;
																							this.ai[0] += 1f;
																							int num926 = 0;
																							if (this.ai[0] >= 60f)
																							{
																								num926++;
																							}
																							if (this.ai[0] >= 180f)
																							{
																								num926++;
																							}
																							if (this.ai[0] >= 240f)
																							{
																								this.Kill();
																								return;
																							}
																							bool flag40 = false;
																							if (this.ai[0] == 60f || this.ai[0] == 180f)
																							{
																								flag40 = true;
																							}
																							bool flag41 = this.ai[0] >= 180f;
																							if (flag41)
																							{
																								if (this.frame < 8)
																								{
																									this.frame = 8;
																								}
																								if (this.frame >= 12)
																								{
																									this.frame = 8;
																								}
																								this.frameCounter++;
																								if (++this.frameCounter >= 5)
																								{
																									this.frameCounter = 0;
																									if (++this.frame >= 12)
																									{
																										this.frame = 8;
																									}
																								}
																							}
																							else if (++this.frameCounter >= 5)
																							{
																								this.frameCounter = 0;
																								if (++this.frame >= 8)
																								{
																									this.frame = 0;
																								}
																							}
																							Vector2 center11 = player5.Center;
																							Vector2 vector144 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - center11;
																							if (player5.gravDir == -1f)
																							{
																								vector144.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - center11.Y;
																							}
																							Vector2 velocity2 = new Vector2((float)Math.Sign((vector144.X == 0f) ? ((float)player5.direction) : vector144.X), 0f);
																							if (velocity2.X != this.velocity.X || velocity2.Y != this.velocity.Y)
																							{
																								this.netUpdate = true;
																							}
																							this.velocity = velocity2;
																							if (this.soundDelay <= 0 && !flag41)
																							{
																								this.soundDelay = 10;
																								this.soundDelay *= 2;
																							}
																							if (Main.myPlayer == this.owner)
																							{
																								bool flag42 = !flag40 || player5.CheckMana(player5.inventory[player5.selectedItem].mana, true, false);
																								bool flag43 = player5.channel && flag42;
																								if ((!flag41 && !flag43) || this.ai[0] == 180f)
																								{
																									Vector2 vector149 = player5.Center + new Vector2((float)(player5.direction * 4), player5.gravDir * 2f);
																									int num932 = this.damage * (1 + num926);
																									vector149 = base.Center;
																									int num933 = 0;
																									float num934 = 0f;
																									for (int num935 = 0; num935 < 200; num935++)
																									{
																										NPC nPC9 = Main.npc[num935];
																										if (nPC9.active && base.Distance(nPC9.Center) < 500f && nPC9.CanBeChasedBy(this, false) && Collision.CanHitLine(nPC9.position, nPC9.width, nPC9.height, vector149, 0, 0))
																										{
																											Vector2 v4 = nPC9.Center - vector149;
																											num934 += v4.ToRotation();
																											num933++;
																											int num936 = Projectile.NewProjectile(vector149.X, vector149.Y, v4.X, v4.Y, 536, 0, 0f, this.owner, (float)this.whoAmI, 0f);
																											Main.projectile[num936].Center = nPC9.Center;
																											Main.projectile[num936].damage = num932;
																											Main.projectile[num936].Damage();
																											Main.projectile[num936].damage = 0;
																											Main.projectile[num936].Center = vector149;
																											this.ai[0] = 180f;
																										}
																									}
																									if (num933 != 0)
																									{
																										num934 /= (float)num933;
																									}
																									else
																									{
																										num934 = ((player5.direction == 1) ? 0f : 3.14159274f);
																									}
																									for (int num937 = 0; num937 < 6; num937++)
																									{
																										Vector2 vector150 = Vector2.Zero;
																										if (Main.rand.Next(4) != 0)
																										{
																											vector150 = Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)num934, default(Vector2)) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f);
																										}
																										else
																										{
																											vector150 = Vector2.UnitX.RotatedByRandom(6.2831854820251465) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f);
																										}
																										Projectile.NewProjectile(vector149.X, vector149.Y, vector150.X, vector150.Y, 536, 0, 0f, this.owner, (float)this.whoAmI, 0f);
																									}
																									this.ai[0] = 180f;
																									this.netUpdate = true;
																								}
																							}
																						}
																						this.rotation = ((player5.gravDir == 1f) ? 0f : 3.14159274f);
																						this.spriteDirection = this.direction;
																						this.timeLeft = 2;
																						Vector2 vector151 = Main.OffsetsPlayerOnhand[player5.bodyFrame.Y / 56] * 2f;
																						if (player5.direction != 1)
																						{
																							vector151.X = (float)player5.bodyFrame.Width - vector151.X;
																						}
																						vector151 -= (player5.bodyFrame.Size() - new Vector2((float)player5.width, 42f)) / 2f;
																						base.Center = (player5.position + vector151 + zero2 - this.velocity).Floor();
																						player5.ChangeDir(this.direction);
																						player5.heldProj = this.whoAmI;
																						player5.itemTime = 2;
																						player5.itemAnimation = 2;
																						return;
																					}
																					if (this.aiStyle == 101)
																					{
																						float num938 = 20f;
																						this.localAI[0] += 1f;
																						this.alpha = (int)MathHelper.Lerp(0f, 255f, this.localAI[0] / num938);
																						int num939 = (int)this.ai[0];
																						int num940 = -1;
																						int num941 = -1;
																						int num655 = this.type;
																						if (num655 != 536)
																						{
																							if (num655 == 591)
																							{
																								num941 = 1;
																							}
																						}
																						else
																						{
																							num940 = 535;
																							num941 = 0;
																						}
																						if (num941 == 1)
																						{
																							if (this.localAI[0] >= num938 || num939 < 0 || num939 > 255 || !Main.player[num939].active || Main.player[num939].dead)
																							{
																								this.Kill();
																								return;
																							}
																							if (this.type == 591)
																							{
																								base.Center = Mount.GetMinecartMechPoint(Main.player[num939], 20, -19) - this.velocity;
																								this.rotation = this.velocity.ToRotation() + 1.57079637f;
																								if (Math.Sign(this.velocity.X) != Math.Sign(Main.player[num939].velocity.X) && Main.player[num939].velocity.X != 0f)
																								{
																									this.Kill();
																									return;
																								}
																							}
																							else
																							{
																								base.Center = Main.player[num939].Center - this.velocity;
																							}
																						}
																						else if (num941 == 0)
																						{
																							if (this.localAI[0] >= num938 || num939 < 0 || num939 > 1000 || !Main.projectile[num939].active || Main.projectile[num939].type != num940)
																							{
																								this.Kill();
																								return;
																							}
																							base.Center = Main.projectile[num939].Center - this.velocity;
																						}
																						this.rotation = this.velocity.ToRotation() + 1.57079637f;
																						return;
																					}
																					if (this.aiStyle == 102)
																					{
																						int num942 = 0;
																						float num943 = 0f;
																						float num944 = 0f;
																						float num945 = 0f;
																						int num946 = -1;
																						int num947 = 0;
																						float num948 = 0f;
																						bool flag44 = true;
																						bool flag45 = false;
																						bool flag46 = false;
																						int num655 = this.type;
																						if (num655 != 539)
																						{
																							switch (num655)
																							{
																								case 573:
																									num942 = 424;
																									num943 = 90f;
																									num948 = 20f;
																									flag44 = false;
																									flag45 = true;
																									break;
																								case 574:
																									num942 = 420;
																									num943 = 180f;
																									num944 = 0.15f;
																									num945 = 0.075f;
																									num948 = 8f;
																									flag44 = false;
																									num946 = 576;
																									num947 = 65;
																									if (Main.expertMode)
																									{
																										num947 = 50;
																									}
																									flag46 = true;
																									break;
																							}
																						}
																						else
																						{
																							num942 = 407;
																							num943 = 210f;
																							num944 = 0.15f;
																							num945 = 0.075f;
																							num948 = 16f;
																						}
																						if (flag46)
																						{
																							int num949 = (int)this.ai[1];
																							if (!Main.npc[num949].active || Main.npc[num949].type != num942)
																							{
																								this.Kill();
																								return;
																							}
																							this.timeLeft = 2;
																						}
																						this.ai[0] += 1f;
																						if (this.ai[0] < num943)
																						{
																							bool flag47 = true;
																							int num950 = (int)this.ai[1];
																							if (Main.npc[num950].active && Main.npc[num950].type == num942)
																							{
																								if (!flag45 && Main.npc[num950].oldPos[1] != Vector2.Zero)
																								{
																									this.position += Main.npc[num950].position - Main.npc[num950].oldPos[1];
																								}
																							}
																							else
																							{
																								this.ai[0] = num943;
																								flag47 = false;
																							}
																							if (flag47 && !flag45)
																							{
																								this.velocity += new Vector2((float)Math.Sign(Main.npc[num950].Center.X - base.Center.X), (float)Math.Sign(Main.npc[num950].Center.Y - base.Center.Y)) * new Vector2(num944, num945);
																								if (this.velocity.Length() > 6f)
																								{
																									this.velocity *= 6f / this.velocity.Length();
																								}
																							}
																							if (this.type == 539)
																							{
																								if (++this.frameCounter >= 4)
																								{
																									this.frameCounter = 0;
																									if (++this.frame >= Main.projFrames[this.type])
																									{
																										this.frame = 0;
																									}
																								}
																								this.rotation = this.velocity.X * 0.1f;
																							}
																							if (this.type == 573)
																							{
																								this.alpha = 255;
																							}
																							if (this.type == 574)
																							{
																								if (flag47)
																								{
																									int target = Main.npc[num950].target;
																									float num954 = this.velocity.ToRotation();
																									if (Collision.CanHitLine(base.Center, 0, 0, Main.player[target].Center, 0, 0))
																									{
																										num954 = base.DirectionTo(Main.player[target].Center).ToRotation();
																									}
																									this.rotation = this.rotation.AngleLerp(num954 + 1.57079637f, 0.2f);
																								}
																								this.frame = 1;
																							}
																						}
																						if (this.ai[0] == num943)
																						{
																							bool flag48 = true;
																							int num955 = -1;
																							if (!flag44)
																							{
																								int num956 = (int)this.ai[1];
																								if (Main.npc[num956].active && Main.npc[num956].type == num942)
																								{
																									num955 = Main.npc[num956].target;
																								}
																								else
																								{
																									flag48 = false;
																								}
																							}
																							else
																							{
																								flag48 = false;
																							}
																							if (!flag48)
																							{
																								num955 = (int)Player.FindClosest(this.position, this.width, this.height);
																							}
																							Vector2 vector152 = Main.player[num955].Center - base.Center;
																							vector152.X += (float)Main.rand.Next(-50, 51);
																							vector152.Y += (float)Main.rand.Next(-50, 51);
																							vector152.X *= (float)Main.rand.Next(80, 121) * 0.01f;
																							vector152.Y *= (float)Main.rand.Next(80, 121) * 0.01f;
																							Vector2 vector153 = Vector2.Normalize(vector152);
																							if (vector153.HasNaNs())
																							{
																								vector153 = Vector2.UnitY;
																							}
																							if (num946 == -1)
																							{
																								this.velocity = vector153 * num948;
																								this.netUpdate = true;
																							}
																							else
																							{
																								if (Main.netMode != 1 && Collision.CanHitLine(base.Center, 0, 0, Main.player[num955].Center, 0, 0))
																								{
																									Projectile.NewProjectile(base.Center.X, base.Center.Y, vector153.X * num948, vector153.Y * num948, num946, num947, 1f, Main.myPlayer, 0f, 0f);
																								}
																								this.ai[0] = 0f;
																							}
																						}
																						if (this.ai[0] >= num943)
																						{
																							this.rotation = this.rotation.AngleLerp(this.velocity.ToRotation() + 1.57079637f, 0.4f);
																							if (this.type == 539)
																							{
																								if (++this.frameCounter >= 2)
																								{
																									this.frameCounter = 0;
																									if (++this.frame >= Main.projFrames[this.type])
																									{
																										this.frame = 0;
																									}
																								}
																							}
																							if (this.type == 573)
																							{
																								this.alpha = 0;
																								return;
																							}
																						}
																					}
																					else if (this.aiStyle == 103)
																					{
																						this.scale = this.ai[1];
																						this.ai[0] += 1f;
																						if (this.ai[0] >= 30f)
																						{
																							this.alpha += 25;
																							if (this.alpha >= 250)
																							{
																								this.Kill();
																								return;
																							}
																						}
																						else if (this.ai[0] >= 0f)
																						{
																							this.alpha -= 25;
																							if (this.alpha < 0)
																							{
																								this.alpha = 0;
																								if (this.localAI[1] == 0f && Main.netMode != 1 && this.localAI[0] != 0f)
																								{
																									this.localAI[1] = 1f;
																									NPC.NewNPC((int)base.Center.X, (int)base.Bottom.Y, (int)this.localAI[0], 0, 0f, 0f, 0f, 0f, 255);
																									return;
																								}
																							}
																						}
																					}
																					else
																					{
																						if (this.aiStyle == 104)
																						{
																							if (this.ai[0] == 1f)
																							{
																								this.scale *= 0.995f;
																								this.alpha += 3;
																								if (this.alpha >= 250)
																								{
																									this.Kill();
																								}
																							}
																							else
																							{
																								this.scale *= 1.01f;
																								this.alpha -= 7;
																								if (this.alpha < 0)
																								{
																									this.alpha = 0;
																									this.ai[0] = 1f;
																								}
																							}
																							this.frameCounter++;
																							if (this.frameCounter > 6)
																							{
																								this.frameCounter = 0;
																								this.frame++;
																								if (this.frame > 3)
																								{
																									this.frame = 0;
																								}
																							}
																							this.velocity.Y = this.velocity.Y - 0.03f;
																							this.velocity.X = this.velocity.X * 0.97f;
																							return;
																						}
																						if (this.aiStyle == 105)
																						{
																							this.localAI[0] += 1f;
																							if (this.localAI[0] >= 90f)
																							{
																								this.localAI[0] *= -1f;
																							}
																							if (this.localAI[0] >= 0f)
																							{
																								this.scale += 0.003f;
																							}
																							else
																							{
																								this.scale -= 0.003f;
																							}
																							this.rotation += 0.0025f * this.scale;
																							float num961 = 1f;
																							float num962 = 1f;
																							if (this.identity % 6 == 0)
																							{
																								num962 *= -1f;
																							}
																							if (this.identity % 6 == 1)
																							{
																								num961 *= -1f;
																							}
																							if (this.identity % 6 == 2)
																							{
																								num962 *= -1f;
																								num961 *= -1f;
																							}
																							if (this.identity % 6 == 3)
																							{
																								num962 = 0f;
																							}
																							if (this.identity % 6 == 4)
																							{
																								num961 = 0f;
																							}
																							this.localAI[1] += 1f;
																							if (this.localAI[1] > 60f)
																							{
																								this.localAI[1] = -180f;
																							}
																							if (this.localAI[1] >= -60f)
																							{
																								this.velocity.X = this.velocity.X + 0.002f * num962;
																								this.velocity.Y = this.velocity.Y + 0.002f * num961;
																							}
																							else
																							{
																								this.velocity.X = this.velocity.X - 0.002f * num962;
																								this.velocity.Y = this.velocity.Y - 0.002f * num961;
																							}
																							this.ai[0] += 1f;
																							if (this.ai[0] > 5400f)
																							{
																								this.damage = 0;
																								this.ai[1] = 1f;
																								if (this.alpha < 255)
																								{
																									this.alpha += 5;
																									if (this.alpha > 255)
																									{
																										this.alpha = 255;
																									}
																								}
																								else if (this.owner == Main.myPlayer)
																								{
																									this.Kill();
																								}
																							}
																							else
																							{
																								float num963 = (base.Center - Main.player[this.owner].Center).Length() / 100f;
																								if (num963 > 4f)
																								{
																									num963 *= 1.1f;
																								}
																								if (num963 > 5f)
																								{
																									num963 *= 1.2f;
																								}
																								if (num963 > 6f)
																								{
																									num963 *= 1.3f;
																								}
																								if (num963 > 7f)
																								{
																									num963 *= 1.4f;
																								}
																								if (num963 > 8f)
																								{
																									num963 *= 1.5f;
																								}
																								if (num963 > 9f)
																								{
																									num963 *= 1.6f;
																								}
																								if (num963 > 10f)
																								{
																									num963 *= 1.7f;
																								}
																								if (!Main.player[this.owner].sporeSac)
																								{
																									num963 += 100f;
																								}
																								this.ai[0] += num963;
																								if (this.alpha > 50)
																								{
																									this.alpha -= 10;
																									if (this.alpha < 50)
																									{
																										this.alpha = 50;
																									}
																								}
																							}
																							bool flag49 = false;
																							Vector2 center12 = new Vector2(0f, 0f);
																							float num964 = 280f;
																							for (int num965 = 0; num965 < 200; num965++)
																							{
																								if (Main.npc[num965].CanBeChasedBy(this, false))
																								{
																									float num966 = Main.npc[num965].position.X + (float)(Main.npc[num965].width / 2);
																									float num967 = Main.npc[num965].position.Y + (float)(Main.npc[num965].height / 2);
																									float num968 = Math.Abs(this.position.X + (float)(this.width / 2) - num966) + Math.Abs(this.position.Y + (float)(this.height / 2) - num967);
																									if (num968 < num964)
																									{
																										num964 = num968;
																										center12 = Main.npc[num965].Center;
																										flag49 = true;
																									}
																								}
																							}
																							if (flag49)
																							{
																								Vector2 vector154 = center12 - base.Center;
																								vector154.Normalize();
																								vector154 *= 0.75f;
																								this.velocity = (this.velocity * 10f + vector154) / 11f;
																								return;
																							}
																							if ((double)this.velocity.Length() > 0.2)
																							{
																								this.velocity *= 0.98f;
																								return;
																							}
																						}
																						else if (this.aiStyle == 106)
																						{
																							this.rotation += this.velocity.X * 0.02f;
																							if (this.velocity.X < 0f)
																							{
																								this.rotation -= Math.Abs(this.velocity.Y) * 0.02f;
																							}
																							else
																							{
																								this.rotation += Math.Abs(this.velocity.Y) * 0.02f;
																							}
																							this.velocity *= 0.98f;
																							this.ai[0] += 1f;
																							if (this.ai[0] >= 60f)
																							{
																								if (this.alpha < 255)
																								{
																									this.alpha += 5;
																									if (this.alpha > 255)
																									{
																										this.alpha = 255;
																										return;
																									}
																								}
																								else if (this.owner == Main.myPlayer)
																								{
																									this.Kill();
																									return;
																								}
																							}
																							else if (this.alpha > 80)
																							{
																								this.alpha -= 30;
																								if (this.alpha < 80)
																								{
																									this.alpha = 80;
																									return;
																								}
																							}
																						}
																						else if (this.aiStyle == 107)
																						{
																							float num969 = 10f;
																							float num970 = 5f;
																							float num971 = 40f;
																							if (this.type == 575)
																							{
																								if (this.timeLeft > 30 && this.alpha > 0)
																								{
																									this.alpha -= 25;
																								}
																								if (this.timeLeft > 30 && this.alpha < 128 && Collision.SolidCollision(this.position, this.width, this.height))
																								{
																									this.alpha = 128;
																								}
																								if (this.alpha < 0)
																								{
																									this.alpha = 0;
																								}
																								if (++this.frameCounter > 4)
																								{
																									this.frameCounter = 0;
																									if (++this.frame >= 4)
																									{
																										this.frame = 0;
																									}
																								}
																							}
																							else if (this.type == 596)
																							{
																								num969 = 10f;
																								num970 = 7.5f;
																								if (this.timeLeft > 30 && this.alpha > 0)
																								{
																									this.alpha -= 25;
																								}
																								if (this.timeLeft > 30 && this.alpha < 128 && Collision.SolidCollision(this.position, this.width, this.height))
																								{
																									this.alpha = 128;
																								}
																								if (this.alpha < 0)
																								{
																									this.alpha = 0;
																								}
																								if (++this.frameCounter > 4)
																								{
																									this.frameCounter = 0;
																									if (++this.frame >= 4)
																									{
																										this.frame = 0;
																									}
																								}
																								this.ai[1] += 1f;
																								float arg_282A8_0 = this.ai[1] / 180f;
																								for (float num973 = 0f; num973 < 3f; num973 += 1f)
																								{
																									if (Main.rand.Next(3) != 0)
																									{
																										return;
																									}
																								}
																								if (this.timeLeft < 4)
																								{
																									int num974 = 40;
																									if (Main.expertMode)
																									{
																										num974 = 30;
																									}
																									this.position = base.Center;
																									this.width = (this.height = 60);
																									base.Center = this.position;
																									this.damage = num974;
																								}
																							}
																							int num976 = (int)this.ai[0];
																							if (num976 >= 0 && Main.player[num976].active && !Main.player[num976].dead)
																							{
																								if (base.Distance(Main.player[num976].Center) > num971)
																								{
																									Vector2 vector155 = base.DirectionTo(Main.player[num976].Center);
																									if (vector155.HasNaNs())
																									{
																										vector155 = Vector2.UnitY;
																									}
																									this.velocity = (this.velocity * (num969 - 1f) + vector155 * num970) / num969;
																									return;
																								}
																							}
																							else
																							{
																								if (this.timeLeft > 30)
																								{
																									this.timeLeft = 30;
																								}
																								if (this.ai[0] != -1f)
																								{
																									this.ai[0] = -1f;
																									this.netUpdate = true;
																									return;
																								}
																							}
																						}
																						else if (this.aiStyle == 108)
																						{
																							if (this.type == 578 && this.localAI[0] == 0f)
																							{
																								this.localAI[0] = 1f;
																								int num977 = (int)Player.FindClosest(base.Center, 0, 0);
																								Vector2 vector156 = Main.player[num977].Center - base.Center;
																								if (vector156 == Vector2.Zero)
																								{
																									vector156 = Vector2.UnitY;
																								}
																								this.ai[1] = vector156.ToRotation();
																								this.netUpdate = true;
																							}
																							this.ai[0] += 1f;
																							if (this.ai[0] <= 90f)
																							{
																								if (this.type == 579)
																								{
																								}
																								if (this.type == 578 && Main.rand.Next(2) == 0)
																								{
																									return;
																								}
																							}
																							else if (this.ai[0] <= 90f)
																							{
																								this.scale = (this.ai[0] - 50f) / 40f;
																								this.alpha = 255 - (int)(255f * this.scale);
																								this.rotation -= 0.157079637f;
																								if (this.type == 578)
																								{
																									if (this.ai[0] == 90f && Main.netMode != 1)
																									{
																										Vector2 vector168 = this.ai[1].ToRotationVector2() * 8f;
																										float ai2 = (float)Main.rand.Next(80);
																										Projectile.NewProjectile(base.Center.X - vector168.X, base.Center.Y - vector168.Y, vector168.X, vector168.Y, 580, 15, 1f, Main.myPlayer, this.ai[1], ai2);
																										return;
																									}
																								}
																								else if (this.type == 579 && this.ai[0] == 90f && Main.netMode != 1)
																								{
																									for (int num980 = 0; num980 < 2; num980++)
																									{
																										int num981 = NPC.NewNPC((int)base.Center.X, (int)base.Center.Y, 427, this.whoAmI, 0f, 0f, 0f, 0f, 255);
																										Main.npc[num981].velocity = -Vector2.UnitY.RotatedByRandom(6.2831854820251465) * (float)Main.rand.Next(4, 9) - Vector2.UnitY * 2f;
																										Main.npc[num981].netUpdate = true;
																									}
																									return;
																								}
																							}
																							else
																							{
																								if (this.ai[0] > 120f)
																								{
																									this.scale = 1f - (this.ai[0] - 120f) / 60f;
																									this.alpha = 255 - (int)(255f * this.scale);
																									this.rotation -= 0.104719758f;
																									if (this.alpha >= 255)
																									{
																										this.Kill();
																									}
																									return;
																								}
																								this.scale = 1f;
																								this.alpha = 0;
																								this.rotation -= 0.05235988f;
																								return;
																							}
																						}
																						else
																						{
																							if (this.aiStyle == 109)
																							{
																								if (this.localAI[1] == 0f)
																								{
																									this.localAI[1] = this.velocity.Length();
																								}
																								if (this.ai[0] == 0f)
																								{
																									this.localAI[0] += 1f;
																									if (this.localAI[0] > 30f)
																									{
																										this.ai[0] = 1f;
																										this.localAI[0] = 0f;
																										return;
																									}
																								}
																								else if (this.ai[0] == 1f)
																								{
																									Vector2 vector173 = Vector2.Zero;
																									if (this.type != 582 || !Main.npc[(int)this.ai[1]].active || Main.npc[(int)this.ai[1]].type != 124)
																									{
																										this.Kill();
																										return;
																									}
																									vector173 = Main.npc[(int)this.ai[1]].Center;
																									this.tileCollide = false;
																									float num984 = this.localAI[1];
																									Vector2 vector174 = vector173 - base.Center;
																									if (vector174.Length() < num984)
																									{
																										this.Kill();
																										return;
																									}
																									vector174.Normalize();
																									vector174 *= num984;
																									this.velocity = Vector2.Lerp(this.velocity, vector174, 0.04f);
																								}
																								this.rotation += 0.314159274f;
																								return;
																							}
																							if (this.aiStyle == 110)
																							{
																								if (this.localAI[1] == 0f)
																								{
																									this.localAI[1] = this.velocity.Length();
																								}
																								Vector2 vector175 = Vector2.Zero;
																								if (!Main.npc[(int)this.ai[0]].active || !Main.npc[(int)this.ai[0]].townNPC)
																								{
																									this.Kill();
																									return;
																								}
																								vector175 = Main.npc[(int)this.ai[0]].Center;
																								float num985 = this.localAI[1];
																								Vector2 vector176 = vector175 - base.Center;
																								if (vector176.Length() < num985 || base.Hitbox.Intersects(Main.npc[(int)this.ai[0]].Hitbox))
																								{
																									this.Kill();
																									int num986 = Main.npc[(int)this.ai[0]].lifeMax - Main.npc[(int)this.ai[0]].life;
																									if (num986 > 20)
																									{
																										num986 = 20;
																									}
																									if (num986 > 0)
																									{
																										Main.npc[(int)this.ai[0]].life += num986;
																										Main.npc[(int)this.ai[0]].HealEffect(num986, true);
																									}
																									return;
																								}
																								vector176.Normalize();
																								vector176 *= num985;
																								if (vector176.Y < this.velocity.Y)
																								{
																									vector176.Y = this.velocity.Y;
																								}
																								vector176.Y += 1f;
																								this.velocity = Vector2.Lerp(this.velocity, vector176, 0.04f);
																								this.rotation += this.velocity.X * 0.05f;
																								return;
																							}
																							else if (this.aiStyle == 111)
																							{
																								if (!Main.npc[(int)this.ai[1]].active || Main.npc[(int)this.ai[1]].type != 20 || Main.npc[(int)this.ai[1]].ai[0] != 14f)
																								{
																									this.Kill();
																									return;
																								}
																								this.ai[0] += 1f;
																								this.rotation += 0.0104719754f;
																								this.scale = this.ai[0] / 100f;
																								if (this.scale > 1f)
																								{
																									this.scale = 1f;
																								}
																								this.alpha = (int)(255f * (1f - this.scale));
																								float num987 = 300f;
																								if (this.ai[0] >= 100f)
																								{
																									num987 = MathHelper.Lerp(300f, 600f, (this.ai[0] - 100f) / 200f);
																								}
																								if (num987 > 600f)
																								{
																									num987 = 600f;
																								}
																								if (this.ai[0] >= 500f)
																								{
																									this.alpha = (int)MathHelper.Lerp(0f, 255f, (this.ai[0] - 500f) / 100f);
																									num987 = MathHelper.Lerp(600f, 1200f, (this.ai[0] - 500f) / 100f);
																									this.rotation += 0.0104719754f;
																								}
																								if (this.ai[0] >= 30f && Main.netMode != 2)
																								{
																									Player player6 = Main.player[Main.myPlayer];
																									if (player6.active && !player6.dead && base.Distance(player6.Center) <= num987 && player6.HasBuff(165) == -1)
																									{
																										player6.AddBuff(165, 120, true);
																									}
																								}
																								if (this.ai[0] >= 30f && this.ai[0] % 10f == 0f && Main.netMode != 1)
																								{
																									for (int num993 = 0; num993 < 200; num993++)
																									{
																										NPC nPC10 = Main.npc[num993];
																										if (nPC10.type != 488 && nPC10.active && base.Distance(nPC10.Center) <= num987)
																										{
																											if (nPC10.townNPC && (nPC10.HasBuff(165) == -1 || nPC10.buffTime[nPC10.HasBuff(165)] <= 20))
																											{
																												nPC10.AddBuff(165, 120, false);
																											}
																											else if (!nPC10.friendly && nPC10.lifeMax > 5 && !nPC10.dontTakeDamage && (nPC10.HasBuff(186) == -1 || nPC10.buffTime[nPC10.HasBuff(186)] <= 20) && (nPC10.dryadBane || Collision.CanHit(base.Center, 1, 1, nPC10.position, nPC10.width, nPC10.height)))
																											{
																												nPC10.AddBuff(186, 120, false);
																											}
																										}
																									}
																								}
																								if (this.ai[0] >= 570f)
																								{
																									this.Kill();
																									return;
																								}
																							}
																							else if (this.aiStyle == 112)
																							{
																								if (this.type == 590)
																								{
																									if (++this.frameCounter >= 4)
																									{
																										this.frameCounter = 0;
																										if (++this.frame >= 3)
																										{
																											this.frame = 0;
																										}
																									}
																									if (this.alpha > 0)
																									{
																										this.alpha -= 15;
																									}
																									if (this.alpha < 0)
																									{
																										this.alpha = 0;
																									}
																									this.velocity = new Vector2(0f, (float)Math.Sin((double)(6.28318548f * this.ai[0] / 180f)) * 0.15f);
																									this.ai[0] += 1f;
																									if (this.ai[0] >= 180f)
																									{
																										this.ai[0] = 0f;
																									}
																								}
																								if (this.type == 644)
																								{
																									Color newColor2 = Main.HslToRgb(this.ai[0], 1f, 0.5f);
																									int num999 = (int)this.ai[1];
																									if (num999 < 0 || num999 >= 1000 || (!Main.projectile[num999].active && Main.projectile[num999].type != 643))
																									{
																										this.ai[1] = -1f;
																									}
																									else
																									{
																										DelegateMethods.v3_1 = newColor2.ToVector3() * 0.5f;
																									}
																									if (this.localAI[0] == 0f)
																									{
																										this.localAI[0] = Main.rand.NextFloat() * 0.8f + 0.8f;
																										this.direction = ((Main.rand.Next(2) > 0) ? 1 : -1);
																									}
																									this.rotation = this.localAI[1] / 40f * 6.28318548f * (float)this.direction;
																									if (this.alpha > 0)
																									{
																										this.alpha -= 8;
																									}
																									if (this.alpha < 0)
																									{
																										this.alpha = 0;
																									}
																									if (Main.rand.Next(10) == 0)
																									{
																										float num1002 = 1f + Main.rand.NextFloat() * 2f;
																										float fadeIn = 1f + Main.rand.NextFloat();
																										float num1003 = 1f + Main.rand.NextFloat();
																										Vector2 vector181 = Utils.RandomVector2(Main.rand, -1f, 1f);
																										if (vector181 != Vector2.Zero)
																										{
																											vector181.Normalize();
																										}
																										vector181 *= 20f + Main.rand.NextFloat() * 100f;
																										Vector2 vector182 = base.Center + vector181;
																										Point point3 = vector182.ToTileCoordinates();
																										bool flag50 = true;
																										if (!WorldGen.InWorld(point3.X, point3.Y, 0))
																										{
																											flag50 = false;
																										}
																										if (flag50 && WorldGen.SolidTile(point3.X, point3.Y))
																										{
																											flag50 = false;
																										}
																									}
																									this.scale = this.Opacity / 2f * this.localAI[0];
																									this.velocity = Vector2.Zero;
																									this.localAI[1] += 1f;
																									if (this.localAI[1] >= 60f)
																									{
																										this.Kill();
																										return;
																									}
																								}
																							}
																							else if (this.aiStyle == 113)
																							{
																								int num1004 = 25;
																								if (this.type == 614)
																								{
																									num1004 = 63;
																								}
																								if (this.alpha > 0)
																								{
																									this.alpha -= num1004;
																								}
																								if (this.alpha < 0)
																								{
																									this.alpha = 0;
																								}
																								if (this.ai[0] == 0f)
																								{
																									bool flag51 = this.type == 614;
																									if (flag51)
																									{
																										int num1005 = (int)this.ai[1];
																										if (!Main.npc[num1005].active)
																										{
																											this.Kill();
																											return;
																										}
																										this.velocity.ToRotation();
																										Vector2 vector183 = Main.npc[num1005].Center - base.Center;
																										if (vector183 != Vector2.Zero)
																										{
																											vector183.Normalize();
																											vector183 *= 14f;
																										}
																										float num1006 = 5f;
																										this.velocity = (this.velocity * (num1006 - 1f) + vector183) / num1006;
																									}
																									else
																									{
																										this.ai[1] += 1f;
																										if (this.ai[1] >= 45f)
																										{
																											float num1007 = 0.98f;
																											float num1008 = 0.35f;
																											if (this.type == 636)
																											{
																												num1007 = 0.995f;
																												num1008 = 0.15f;
																											}
																											this.ai[1] = 45f;
																											this.velocity.X = this.velocity.X * num1007;
																											this.velocity.Y = this.velocity.Y + num1008;
																										}
																										this.rotation = this.velocity.ToRotation() + 1.57079637f;
																									}
																								}
																								if (this.ai[0] == 1f)
																								{
																									this.ignoreWater = true;
																									this.tileCollide = false;
																									int num1009 = 15;
																									if (this.type == 636)
																									{
																										num1009 = 5 * this.MaxUpdates;
																									}
																									bool flag52 = false;
																									bool flag53 = false;
																									this.localAI[0] += 1f;
																									if (this.localAI[0] % 30f == 0f)
																									{
																										flag53 = true;
																									}
																									int num1010 = (int)this.ai[1];
																									if (this.localAI[0] >= (float)(60 * num1009))
																									{
																										flag52 = true;
																									}
																									else if (num1010 < 0 || num1010 >= 200)
																									{
																										flag52 = true;
																									}
																									else if (Main.npc[num1010].active && !Main.npc[num1010].dontTakeDamage)
																									{
																										base.Center = Main.npc[num1010].Center - this.velocity * 2f;
																										this.gfxOffY = Main.npc[num1010].gfxOffY;
																										if (flag53)
																										{
																											Main.npc[num1010].HitEffect(0, 1.0);
																										}
																									}
																									else
																									{
																										flag52 = true;
																									}
																									if (flag52)
																									{
																										this.Kill();
																									}
																								}
																							}
																							else if (this.aiStyle == 114)
																							{
																								if (Main.netMode == 2 && this.localAI[0] == 0f)
																								{
																									PortalHelper.SyncPortalSections(base.Center, 1);
																									this.localAI[0] = 1f;
																								}
																								this.timeLeft = 3;
																								bool flag54 = false;
																								if (!Main.player[this.owner].active || Main.player[this.owner].dead || base.Distance(Main.player[this.owner].Center) > 12800f)
																								{
																									flag54 = true;
																								}
																								if (!flag54 && !WorldGen.InWorld((int)base.Center.X / 16, (int)base.Center.Y / 16))
																								{
																									flag54 = true;
																								}
																								if (!flag54 && !PortalHelper.SupportedTilesAreFine(base.Center, this.ai[0]))
																								{
																									flag54 = true;
																								}
																								if (flag54)
																								{
																									this.Kill();
																									return;
																								}
																								Color portalColor = PortalHelper.GetPortalColor(this.owner, (int)this.ai[1]);
																								this.alpha -= 25;
																								if (this.alpha < 0)
																								{
																									this.alpha = 0;
																								}
																								if (++this.frameCounter >= 6)
																								{
																									this.frameCounter = 0;
																									if (++this.frame >= Main.projFrames[this.type])
																									{
																										this.frame = 0;
																									}
																								}
																								this.rotation = this.ai[0] - 1.57079637f;
																								return;
																							}
																							else if (this.aiStyle == 115)
																							{
																								this.velocity *= 0.985f;
																								this.rotation += this.velocity.X * 0.2f;
																								if (this.velocity.X > 0f)
																								{
																									this.rotation += 0.08f;
																								}
																								else
																								{
																									this.rotation -= 0.08f;
																								}
																								this.ai[1] += 1f;
																								if (this.ai[1] > 30f)
																								{
																									this.alpha += 10;
																									if (this.alpha >= 255)
																									{
																										this.alpha = 255;
																										this.Kill();
																										return;
																									}
																								}
																							}
																							else
																							{
																								if (this.aiStyle == 116)
																								{
																									if (this.localAI[0] == 0f)
																									{
																										this.rotation = this.ai[1];
																										this.localAI[0] = 1f;
																									}
																									Player player7 = Main.player[this.owner];
																									if (player7.setSolar)
																									{
																										this.timeLeft = 2;
																									}
																									float num1011 = (float)player7.miscCounter / 300f * 12.566371f + this.ai[1];
																									num1011 = MathHelper.WrapAngle(num1011);
																									this.rotation = this.rotation.AngleLerp(num1011, 0.05f);
																									this.alpha -= 15;
																									if (this.alpha < 0)
																									{
																										this.alpha = 0;
																									}
																									this.velocity = this.rotation.ToRotationVector2() * 100f - player7.velocity;
																									base.Center = player7.Center - this.velocity;
																									return;
																								}
																								if (this.aiStyle == 117)
																								{
																									this.ai[1] += 0.01f;
																									this.scale = this.ai[1];
																									this.ai[0] += 1f;
																									if (this.ai[0] >= (float)(3 * Main.projFrames[this.type]))
																									{
																										this.Kill();
																										return;
																									}
																									if (++this.frameCounter >= 3)
																									{
																										this.frameCounter = 0;
																										if (++this.frame >= Main.projFrames[this.type])
																										{
																											this.hide = true;
																										}
																									}
																									this.alpha -= 63;
																									if (this.alpha < 0)
																									{
																										this.alpha = 0;
																									}
																									bool flag55 = this.type == 612;
																									bool flag56 = this.type == 624;
																									if (this.ai[0] == 1f)
																									{
																										this.position = base.Center;
																										this.width = (this.height = (int)(52f * this.scale));
																										base.Center = this.position;
																										this.Damage();
																										if (flag55)
																										{
																											if (flag56)
																											{
																												return;
																											}
																										}
																									}
																								}
																								else if (this.aiStyle == 118)
																								{
																									this.ai[0] += 1f;
																									int num1035 = 0;
																									if (this.velocity.Length() <= 4f)
																									{
																										num1035 = 1;
																									}
																									this.alpha -= 15;
																									if (this.alpha < 0)
																									{
																										this.alpha = 0;
																									}
																									if (num1035 == 0)
																									{
																										this.rotation -= 0.104719758f;
																										if (this.ai[0] >= 30f)
																										{
																											this.velocity *= 0.98f;
																											this.scale += 0.00744680827f;
																											if (this.scale > 1.3f)
																											{
																												this.scale = 1.3f;
																											}
																											this.rotation -= 0.0174532924f;
																										}
																										if (this.velocity.Length() < 4.1f)
																										{
																											this.velocity.Normalize();
																											this.velocity *= 4f;
																											this.ai[0] = 0f;
																										}
																									}
																									else if (num1035 == 1)
																									{
																										this.rotation -= 0.104719758f;
																										if (this.ai[0] % 30f == 0f && this.ai[0] < 241f && Main.myPlayer == this.owner)
																										{
																											Vector2 vector189 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * 12f;
																											Projectile.NewProjectile(base.Center.X, base.Center.Y, vector189.X, vector189.Y, 618, this.damage / 2, 0f, this.owner, 0f, (float)this.whoAmI);
																										}
																										Vector2 vector190 = base.Center;
																										float num1037 = 800f;
																										bool flag57 = false;
																										int num1038 = 0;
																										if (this.ai[1] == 0f)
																										{
																											for (int num1039 = 0; num1039 < 200; num1039++)
																											{
																												if (Main.npc[num1039].CanBeChasedBy(this, false))
																												{
																													Vector2 center13 = Main.npc[num1039].Center;
																													if (base.Distance(center13) < num1037 && Collision.CanHit(new Vector2(this.position.X + (float)(this.width / 2), this.position.Y + (float)(this.height / 2)), 1, 1, Main.npc[num1039].position, Main.npc[num1039].width, Main.npc[num1039].height))
																													{
																														num1037 = base.Distance(center13);
																														vector190 = center13;
																														flag57 = true;
																														num1038 = num1039;
																													}
																												}
																											}
																											if (flag57)
																											{
																												if (this.ai[1] != (float)(num1038 + 1))
																												{
																													this.netUpdate = true;
																												}
																												this.ai[1] = (float)(num1038 + 1);
																											}
																											flag57 = false;
																										}
																										if (this.ai[1] != 0f)
																										{
																											int num1040 = (int)(this.ai[1] - 1f);
																											if (Main.npc[num1040].active && Main.npc[num1040].CanBeChasedBy(this, true) && base.Distance(Main.npc[num1040].Center) < 1000f)
																											{
																												flag57 = true;
																												vector190 = Main.npc[num1040].Center;
																											}
																										}
																										if (!this.friendly)
																										{
																											flag57 = false;
																										}
																										if (flag57)
																										{
																											float num1041 = 4f;
																											Vector2 vector191 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
																											float num1042 = vector190.X - vector191.X;
																											float num1043 = vector190.Y - vector191.Y;
																											float num1044 = (float)Math.Sqrt((double)(num1042 * num1042 + num1043 * num1043));
																											num1044 = num1041 / num1044;
																											num1042 *= num1044;
																											num1043 *= num1044;
																											int num1045 = 8;
																											this.velocity.X = (this.velocity.X * (float)(num1045 - 1) + num1042) / (float)num1045;
																											this.velocity.Y = (this.velocity.Y * (float)(num1045 - 1) + num1043) / (float)num1045;
																										}
																									}
																									if (this.ai[0] >= 600f)
																									{
																										this.Kill();
																										return;
																									}
																								}
																								else if (this.aiStyle == 119)
																								{
																									int num1046 = 0;
																									float num1047 = 0f;
																									float num1048 = 0f;
																									float num1049 = 0f;
																									bool flag58 = false;
																									bool flag59 = false;
																									int num655 = this.type;
																									if (num655 == 618)
																									{
																										num1046 = 617;
																										num1047 = 420f;
																										num1048 = 0.15f;
																										num1049 = 0.15f;
																									}
																									if (flag59)
																									{
																										int num1050 = (int)this.ai[1];
																										if (!Main.projectile[num1050].active || Main.projectile[num1050].type != num1046)
																										{
																											this.Kill();
																											return;
																										}
																										this.timeLeft = 2;
																									}
																									this.ai[0] += 1f;
																									if (this.ai[0] < num1047)
																									{
																										bool flag60 = true;
																										int num1051 = (int)this.ai[1];
																										if (Main.projectile[num1051].active && Main.projectile[num1051].type == num1046)
																										{
																											if (!flag58 && Main.projectile[num1051].oldPos[1] != Vector2.Zero)
																											{
																												this.position += Main.projectile[num1051].position - Main.projectile[num1051].oldPos[1];
																											}
																											if (base.Center.HasNaNs())
																											{
																												this.Kill();
																												return;
																											}
																										}
																										else
																										{
																											this.ai[0] = num1047;
																											flag60 = false;
																											this.Kill();
																										}
																										if (flag60 && !flag58)
																										{
																											this.velocity += new Vector2((float)Math.Sign(Main.projectile[num1051].Center.X - base.Center.X), (float)Math.Sign(Main.projectile[num1051].Center.Y - base.Center.Y)) * new Vector2(num1048, num1049);
																											if (this.velocity.Length() > 6f)
																											{
																												this.velocity *= 6f / this.velocity.Length();
																											}
																										}
																										if (this.type == 618)
																										{
																											this.alpha = 255;
																											return;
																										}
																										this.Kill();
																										return;
																									}
																								}
																								else if (this.aiStyle == 120)
																								{
																									Player player8 = Main.player[this.owner];
																									if (!player8.active)
																									{
																										this.active = false;
																										return;
																									}
																									bool flag61 = this.type == 623;
																									Vector2 vector192 = player8.Center;
																									float num1053 = 100f;
																									float num1054 = 300f;
																									float num1055 = 100f;
																									float num1056 = 100f;
																									if (flag61)
																									{
																										if (player8.dead)
																										{
																											player8.stardustGuardian = false;
																										}
																										if (player8.stardustGuardian)
																										{
																											this.timeLeft = 2;
																										}
																										num1053 = 150f;
																										num1054 = 250f;
																										num1055 = 200f;
																										vector192.X -= (float)((5 + player8.width / 2) * player8.direction);
																										vector192.Y -= 25f;
																										if (this.ai[0] != 3f && this.alpha == 255)
																										{
																											this.alpha = 0;
																										}
																										if (this.localAI[0] > 0f)
																										{
																											this.localAI[0] -= 1f;
																										}
																									}
																									if (this.ai[0] != 0f)
																									{
																										Main.player[this.owner].tankPet = this.whoAmI;
																										Main.player[this.owner].tankPetReset = false;
																									}
																									if (this.ai[0] == 0f)
																									{
																										if (player8.HasMinionTarget)
																										{
																											this.ai[0] = 3f;
																											this.netUpdate = true;
																										}
																										base.Center = Vector2.Lerp(base.Center, vector192, 0.2f);
																										this.velocity *= 0.8f;
																										this.direction = (this.spriteDirection = player8.direction);
																										if (flag61 && ++this.frameCounter >= 9)
																										{
																											this.frameCounter = 0;
																											if (++this.frame >= Main.projFrames[this.type] - 4)
																											{
																												this.frame = 0;
																											}
																										}
																									}
																									else if (this.ai[0] == 1f)
																									{
																										if (player8.HasMinionTarget)
																										{
																											vector192 = player8.MinionTargetPoint;
																										}
																										else
																										{
																											this.ai[0] = 0f;
																											this.netUpdate = true;
																										}
																										int num1059 = -1;
																										bool flag62 = true;
																										if (flag61 && Math.Abs(base.Center.X - vector192.X) > num1053 + 20f)
																										{
																											flag62 = false;
																										}
																										if (flag62)
																										{
																											for (int num1060 = 0; num1060 < 200; num1060++)
																											{
																												NPC nPC11 = Main.npc[num1060];
																												if (nPC11.CanBeChasedBy(this, false))
																												{
																													float num1061 = base.Distance(nPC11.Center);
																													if (num1061 < num1054)
																													{
																														num1059 = num1060;
																													}
																												}
																											}
																										}
																										if (num1059 != -1)
																										{
																											NPC nPC12 = Main.npc[num1059];
																											this.direction = (this.spriteDirection = (nPC12.Center.X > base.Center.X).ToDirectionInt());
																											float num1062 = Math.Abs(vector192.X - base.Center.X);
																											float num1063 = Math.Abs(nPC12.Center.X - base.Center.X);
																											float num1064 = Math.Abs(vector192.Y - base.Center.Y);
																											float num1065 = Math.Abs(nPC12.Center.Y - base.Bottom.Y);
																											float num1066 = (float)(nPC12.Center.Y > base.Bottom.Y).ToDirectionInt();
																											if ((num1062 < num1053 || (vector192.X - base.Center.X) * (float)this.direction < 0f) && num1063 > 20f && num1063 < num1053 - num1062 + 100f)
																											{
																												this.velocity.X = this.velocity.X + 0.1f * (float)this.direction;
																											}
																											else
																											{
																												this.velocity.X = this.velocity.X * 0.7f;
																											}
																											if ((num1064 < num1056 || (vector192.Y - base.Bottom.Y) * num1066 < 0f) && num1065 > 10f && num1065 < num1056 - num1064 + 10f)
																											{
																												this.velocity.Y = this.velocity.Y + 0.1f * num1066;
																											}
																											else
																											{
																												this.velocity.Y = this.velocity.Y * 0.7f;
																											}
																											if (this.localAI[0] == 0f && this.owner == Main.myPlayer && num1063 < num1055)
																											{
																												this.ai[1] = 0f;
																												this.ai[0] = 2f;
																												this.netUpdate = true;
																												this.localAI[0] = 90f;
																											}
																										}
																										else
																										{
																											if (Math.Abs(vector192.X - base.Center.X) > num1053 + 40f)
																											{
																												this.ai[0] = 3f;
																												this.netUpdate = true;
																											}
																											else if (Math.Abs(vector192.X - base.Center.X) > 20f)
																											{
																												this.direction = (this.spriteDirection = (vector192.X > base.Center.X).ToDirectionInt());
																												this.velocity.X = this.velocity.X + 0.06f * (float)this.direction;
																											}
																											else
																											{
																												this.velocity.X = this.velocity.X * 0.8f;
																												this.direction = (this.spriteDirection = (player8.Center.X < base.Center.X).ToDirectionInt());
																											}
																											if (Math.Abs(vector192.Y - base.Center.Y) > num1056)
																											{
																												this.ai[0] = 3f;
																												this.netUpdate = true;
																											}
																											else if (Math.Abs(vector192.Y - base.Center.Y) > 10f)
																											{
																												this.velocity.Y = this.velocity.Y + 0.06f * (float)Math.Sign(vector192.Y - base.Center.Y);
																											}
																											else
																											{
																												this.velocity.Y = this.velocity.Y * 0.8f;
																											}
																										}
																										if (flag61 && ++this.frameCounter >= 9)
																										{
																											this.frameCounter = 0;
																											if (++this.frame >= Main.projFrames[this.type] - 4)
																											{
																												this.frame = 0;
																											}
																										}
																									}
																									else if (this.ai[0] == 2f)
																									{
																										this.velocity.X = this.velocity.X * 0.9f;
																										this.ai[1] += 1f;
																										float num1067 = 0f;
																										if (flag61)
																										{
																											num1067 = 20f;
																											if (this.ai[1] == 10f && this.owner == Main.myPlayer)
																											{
																												int num1068 = (int)(20f * Main.player[this.owner].minionDamage);
																												Projectile.NewProjectile(base.Center.X, base.Center.Y, 0f, 0f, 624, num1068, 6f, this.owner, 0f, 5f);
																											}
																										}
																										if (this.ai[1] >= num1067)
																										{
																											this.ai[1] = 0f;
																											this.ai[0] = 1f;
																											this.netUpdate = true;
																										}
																										if (flag61)
																										{
																											if (this.frame < Main.projFrames[this.type] - 4)
																											{
																												this.frame = Main.projFrames[this.type] - 1;
																												this.frameCounter = 0;
																											}
																											if (++this.frameCounter >= 5)
																											{
																												this.frameCounter = 0;
																												if (--this.frame < Main.projFrames[this.type] - 5)
																												{
																													this.frame = Main.projFrames[this.type] - 1;
																												}
																											}
																										}
																									}
																									if (this.ai[0] == 3f)
																									{
																										if (player8.HasMinionTarget)
																										{
																											vector192 = player8.MinionTargetPoint;
																										}
																										else
																										{
																											this.ai[0] = 0f;
																											this.netUpdate = true;
																										}
																										if (this.alpha == 0)
																										{
																											this.alpha = 255;
																										}
																										this.velocity *= 0.7f;
																										base.Center = Vector2.Lerp(base.Center, vector192, 0.2f);
																										if (base.Distance(vector192) < 10f)
																										{
																											this.ai[0] = 1f;
																											this.netUpdate = true;
																											return;
																										}
																									}
																								}
																								else if (this.aiStyle == 121)
																								{
																									Player player9 = Main.player[this.owner];
																									if ((int)Main.time % 120 == 0)
																									{
																										this.netUpdate = true;
																									}
																									if (!player9.active)
																									{
																										this.active = false;
																										return;
																									}
																									bool flag63 = this.type == 625;
																									bool flag64 = this.type == 625 || this.type == 626 || this.type == 627 || this.type == 628;
																									int num1051 = 10;
																									if (flag64)
																									{
																										if (player9.dead)
																										{
																											player9.stardustDragon = false;
																										}
																										if (player9.stardustDragon)
																										{
																											this.timeLeft = 2;
																										}
																										num1051 = 30;
																									}
																									if (flag63)
																									{
																										Vector2 center14 = player9.Center;
																										float num1077 = 700f;
																										float num1078 = 1000f;
																										int num1079 = -1;
																										if (base.Distance(center14) > 2000f)
																										{
																											base.Center = center14;
																											this.netUpdate = true;
																										}
																										bool flag65 = true;
																										if (flag65)
																										{
																											for (int num1080 = 0; num1080 < 200; num1080++)
																											{
																												NPC nPC13 = Main.npc[num1080];
																												if (nPC13.CanBeChasedBy(this, false) && player9.Distance(nPC13.Center) < num1078)
																												{
																													float num1081 = base.Distance(nPC13.Center);
																													if (num1081 < num1077)
																													{
																														num1079 = num1080;
																														bool arg_2D71A_0 = nPC13.boss;
																													}
																												}
																											}
																										}
																										if (num1079 != -1)
																										{
																											NPC nPC14 = Main.npc[num1079];
																											Vector2 vector193 = nPC14.Center - base.Center;
																											(vector193.X > 0f).ToDirectionInt();
																											(vector193.Y > 0f).ToDirectionInt();
																											float num1082 = 0.4f;
																											if (vector193.Length() < 600f)
																											{
																												num1082 = 0.6f;
																											}
																											if (vector193.Length() < 300f)
																											{
																												num1082 = 0.8f;
																											}
																											if (vector193.Length() > nPC14.Size.Length() * 0.75f)
																											{
																												this.velocity += Vector2.Normalize(vector193) * num1082 * 1.5f;
																												if (Vector2.Dot(this.velocity, vector193) < 0.25f)
																												{
																													this.velocity *= 0.8f;
																												}
																											}
																											float num1083 = 30f;
																											if (this.velocity.Length() > num1083)
																											{
																												this.velocity = Vector2.Normalize(this.velocity) * num1083;
																											}
																										}
																										else
																										{
																											float num1084 = 0.2f;
																											Vector2 vector194 = center14 - base.Center;
																											if (vector194.Length() < 200f)
																											{
																												num1084 = 0.12f;
																											}
																											if (vector194.Length() < 140f)
																											{
																												num1084 = 0.06f;
																											}
																											if (vector194.Length() > 100f)
																											{
																												if (Math.Abs(center14.X - base.Center.X) > 20f)
																												{
																													this.velocity.X = this.velocity.X + num1084 * (float)Math.Sign(center14.X - base.Center.X);
																												}
																												if (Math.Abs(center14.Y - base.Center.Y) > 10f)
																												{
																													this.velocity.Y = this.velocity.Y + num1084 * (float)Math.Sign(center14.Y - base.Center.Y);
																												}
																											}
																											else if (this.velocity.Length() > 2f)
																											{
																												this.velocity *= 0.96f;
																											}
																											if (Math.Abs(this.velocity.Y) < 1f)
																											{
																												this.velocity.Y = this.velocity.Y - 0.1f;
																											}
																											float num1085 = 15f;
																											if (this.velocity.Length() > num1085)
																											{
																												this.velocity = Vector2.Normalize(this.velocity) * num1085;
																											}
																										}
																										this.rotation = this.velocity.ToRotation() + (float)(Math.PI / 2f);
																										int direction = this.direction;
																										this.direction = (this.spriteDirection = ((this.velocity.X > 0f) ? 1 : -1));
																										if (direction != this.direction)
																										{
																											this.netUpdate = true;
																										}
																										this.position = base.Center;
																										this.scale = 1f + this.localAI[0] * 0.01f;
																										this.width = (this.height = (int)((float)num1051 * this.scale));
																										base.Center = this.position;
																										if (this.alpha > 0)
																										{
																											this.alpha -= 42;
																											if (this.alpha < 0)
																											{
																												this.alpha = 0;
																											}
																										}
																									}
																									else
																									{
																										bool flag66 = false;
																										Vector2 vector195 = Vector2.Zero;
																										Vector2 arg_2DBC1_0 = Vector2.Zero;
																										float num1088 = 0f;
																										float num1089 = 0f;
																										float num1090 = 1f;
																										if (this.ai[1] == 1f)
																										{
																											this.ai[1] = 0f;
																											this.netUpdate = true;
																										}
																										int byUUID = Projectile.GetByUUID(this.owner, (int)this.ai[0]);
																										if (flag64 && byUUID >= 0 && Main.projectile[byUUID].active && (Main.projectile[byUUID].type == 625 || Main.projectile[byUUID].type == 626 || Main.projectile[byUUID].type == 627))
																										{
																											flag66 = true;
																											vector195 = Main.projectile[byUUID].Center;
																											Vector2 arg_2DE6A_0 = Main.projectile[byUUID].velocity;
																											num1088 = Main.projectile[byUUID].rotation;
																											float num1065 = MathHelper.Clamp(Main.projectile[byUUID].scale, 0f, 50f);
																											num1089 = num1065;
																											num1090 = 16f;
																											int arg_2DEC0_0 = Main.projectile[byUUID].alpha;
																											Main.projectile[byUUID].localAI[0] = this.localAI[0] + 1f;
																											if (Main.projectile[byUUID].type != 625)
																											{
																												Main.projectile[byUUID].localAI[1] = (float)this.whoAmI;
																											}
																											if (this.owner == Main.myPlayer && Main.projectile[byUUID].type == 625 && this.type == 628)
																											{
																												Main.projectile[byUUID].Kill();
																												this.Kill();
																												return;
																											}
																										}
																										if (!flag66)
																										{
																											return;
																										}
																										this.alpha -= 42;
																										if (this.alpha < 0)
																										{
																											this.alpha = 0;
																										}
																										this.velocity = Vector2.Zero;
																										Vector2 vector134 = vector195 - base.Center;
																										if (num1088 != this.rotation)
																										{
																											float num1068 = MathHelper.WrapAngle(num1088 - this.rotation);
																											vector134 = vector134.RotatedBy((double)(num1068 * 0.1f), default(Vector2));
																										}
																										this.rotation = vector134.ToRotation() + 1.57079637f;
																										this.position = base.Center;
																										this.scale = num1090;
																										this.width = (this.height = (int)((float)num1051 * this.scale));
																										base.Center = this.position;
																										if (vector134 != Vector2.Zero)
																										{
																											base.Center = vector195 - Vector2.Normalize(vector134) * num1090 * num1090;
																										}
																										this.spriteDirection = ((vector134.X > 0f) ? 1 : -1);
																										return;
																									}
																								}
																								else if (this.aiStyle == 122)
																								{
																									int num1096 = (int)this.ai[0];
																									bool flag67 = false;
																									if (num1096 == -1 || !Main.npc[num1096].active)
																									{
																										flag67 = true;
																									}
																									if (flag67)
																									{
																										if (this.type == 629)
																										{
																											this.Kill();
																											return;
																										}
																										if (this.type == 631 && this.ai[0] != -1f)
																										{
																											this.ai[0] = -1f;
																											this.netUpdate = true;
																										}
																									}
																									if (!flag67 && base.Hitbox.Intersects(Main.npc[num1096].Hitbox))
																									{
																										this.Kill();
																										if (this.type == 631)
																										{
																											this.localAI[1] = 1f;
																											this.Damage();
																										}
																										return;
																									}
																									if (this.type == 629)
																									{
																										Vector2 vector197 = Main.npc[num1096].Center - base.Center;
																										this.velocity = Vector2.Normalize(vector197) * 5f;
																									}
																									if (this.type == 631)
																									{
																										if (this.ai[1] > 0f)
																										{
																											this.ai[1] -= 1f;
																											this.velocity = Vector2.Zero;
																											return;
																										}
																										if (flag67)
																										{
																											if (this.velocity == Vector2.Zero)
																											{
																												this.Kill();
																											}
																											this.tileCollide = true;
																											this.alpha += 10;
																											if (this.alpha > 255)
																											{
																												this.Kill();
																											}
																										}
																										else
																										{
																											Vector2 vector198 = Main.npc[num1096].Center - base.Center;
																											this.velocity = Vector2.Normalize(vector198) * 12f;
																											this.alpha -= 15;
																											if (this.alpha < 0)
																											{
																												this.alpha = 0;
																											}
																										}
																										this.rotation = this.velocity.ToRotation() - 1.57079637f;
																										return;
																									}
																								}
																								else if (this.aiStyle == 123)
																								{
																									bool flag68 = this.type == 641;
																									bool flag69 = this.type == 643;
																									float num1097 = 1000f;
																									this.velocity = Vector2.Zero;
																									if (flag68)
																									{
																										this.alpha -= 5;
																										if (this.alpha < 0)
																										{
																											this.alpha = 0;
																										}
																										if (this.direction == 0)
																										{
																											this.direction = Main.player[this.owner].direction;
																										}
																										this.rotation -= (float)this.direction * 6.28318548f / 120f;
																										this.scale = this.Opacity;
																									}
																									if (flag69)
																									{
																										this.alpha -= 5;
																										if (this.alpha < 0)
																										{
																											this.alpha = 0;
																										}
																										if (this.direction == 0)
																										{
																											this.direction = Main.player[this.owner].direction;
																										}
																										if (++this.frameCounter >= 3)
																										{
																											this.frameCounter = 0;
																											if (++this.frame >= Main.projFrames[this.type])
																											{
																												this.frame = 0;
																											}
																										}
																										this.localAI[0] += 1f;
																										if (this.localAI[0] >= 60f)
																										{
																											this.localAI[0] = 0f;
																										}
																									}
																									if (this.ai[0] < 0f)
																									{
																										this.ai[0] += 1f;
																										if (flag68)
																										{
																											this.ai[1] -= (float)this.direction * 0.3926991f / 50f;
																										}
																									}
																									if (this.ai[0] == 0f)
																									{
																										int num1099 = -1;
																										float num1100 = num1097;
																										for (int num1101 = 0; num1101 < 200; num1101++)
																										{
																											NPC nPC15 = Main.npc[num1101];
																											if (nPC15.CanBeChasedBy(this, false))
																											{
																												float num1102 = base.Distance(nPC15.Center);
																												if (num1102 < num1100 && Collision.CanHitLine(base.Center, 0, 0, nPC15.Center, 0, 0))
																												{
																													num1100 = num1102;
																													num1099 = num1101;
																												}
																											}
																										}
																										if (num1099 != -1)
																										{
																											this.ai[0] = 1f;
																											this.ai[1] = (float)num1099;
																											this.netUpdate = true;
																											return;
																										}
																									}
																									if (this.ai[0] > 0f)
																									{
																										int num1103 = (int)this.ai[1];
																										if (!Main.npc[num1103].CanBeChasedBy(this, false))
																										{
																											this.ai[0] = 0f;
																											this.ai[1] = 0f;
																											this.netUpdate = true;
																											return;
																										}
																										this.ai[0] += 1f;
																										float num1104 = 30f;
																										if (flag69)
																										{
																											num1104 = 5f;
																										}
																										if (this.ai[0] >= num1104)
																										{
																											Vector2 vector201 = base.DirectionTo(Main.npc[num1103].Center);
																											if (vector201.HasNaNs())
																											{
																												vector201 = Vector2.UnitY;
																											}
																											float num1105 = vector201.ToRotation();
																											int num1106 = (vector201.X > 0f) ? 1 : -1;
																											if (flag68)
																											{
																												this.direction = num1106;
																												this.ai[0] = -60f;
																												this.ai[1] = num1105 + (float)num1106 * 3.14159274f / 16f;
																												this.netUpdate = true;
																												if (this.owner == Main.myPlayer)
																												{
																													Projectile.NewProjectile(base.Center.X, base.Center.Y, vector201.X, vector201.Y, 642, this.damage, this.knockBack, this.owner, 0f, (float)this.whoAmI);
																												}
																											}
																											if (flag69)
																											{
																												this.direction = num1106;
																												this.ai[0] = -20f;
																												this.netUpdate = true;
																												if (this.owner == Main.myPlayer)
																												{
																													Vector2 vector202 = Main.npc[num1103].position + Main.npc[num1103].Size * Utils.RandomVector2(Main.rand, 0f, 1f) - base.Center;
																													for (int num1107 = 0; num1107 < 3; num1107++)
																													{
																														Vector2 vector203 = base.Center + vector202;
																														if (num1107 > 0)
																														{
																															vector203 = base.Center + vector202.RotatedByRandom(0.78539818525314331) * (Main.rand.NextFloat() * 0.5f + 0.75f);
																														}
																														float x2 = Main.RgbToHsl(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB)).X;
																														Projectile.NewProjectile(vector203.X, vector203.Y, 0f, 0f, 644, this.damage, this.knockBack, this.owner, x2, (float)this.whoAmI);
																													}
																													return;
																												}
																											}
																										}
																									}
																								}
																								else if (this.aiStyle == 124)
																								{
																									Player player10 = Main.player[this.owner];
																									if (player10.dead)
																									{
																										this.Kill();
																										return;
																									}
																									if (Main.myPlayer == this.owner && player10.suspiciouslookingTentacle)
																									{
																										this.timeLeft = 2;
																									}
																									this.direction = (this.spriteDirection = player10.direction);
																									Vector3 v3_ = new Vector3(0.5f, 0.9f, 1f) * 1.5f;
																									DelegateMethods.v3_1 = v3_;
																									Vector2 vector204 = new Vector2((float)(player10.direction * 30), -20f);
																									Vector2 vector205 = player10.MountedCenter + vector204;
																									float num1108 = Vector2.Distance(base.Center, vector205);
																									if (num1108 > 1000f)
																									{
																										base.Center = player10.Center + vector204;
																									}
																									Vector2 vector206 = vector205 - base.Center;
																									float num1109 = 4f;
																									if (num1108 < num1109)
																									{
																										this.velocity *= 0.25f;
																									}
																									if (vector206 != Vector2.Zero)
																									{
																										if (vector206.Length() < num1109)
																										{
																											this.velocity = vector206;
																										}
																										else
																										{
																											this.velocity = vector206 * 0.1f;
																										}
																									}
																									if (this.velocity.Length() > 6f)
																									{
																										float num1110 = this.velocity.ToRotation() + 1.57079637f;
																										if (Math.Abs(this.rotation - num1110) >= 3.14159274f)
																										{
																											if (num1110 < this.rotation)
																											{
																												this.rotation -= 6.28318548f;
																											}
																											else
																											{
																												this.rotation += 6.28318548f;
																											}
																										}
																										float num1111 = 12f;
																										this.rotation = (this.rotation * (num1111 - 1f) + num1110) / num1111;
																										if (++this.frameCounter >= 4)
																										{
																											this.frameCounter = 0;
																											if (++this.frame >= Main.projFrames[this.type])
																											{
																												this.frame = 0;
																											}
																										}
																									}
																									else
																									{
																										if (this.rotation > 3.14159274f)
																										{
																											this.rotation -= 6.28318548f;
																										}
																										if (this.rotation > -0.005f && this.rotation < 0.005f)
																										{
																											this.rotation = 0f;
																										}
																										else
																										{
																											this.rotation *= 0.96f;
																										}
																										if (++this.frameCounter >= 6)
																										{
																											this.frameCounter = 0;
																											if (++this.frame >= Main.projFrames[this.type])
																											{
																												this.frame = 0;
																											}
																										}
																									}
																									if (this.ai[0] > 0f && (this.ai[0] += 1f) >= 60f)
																									{
																										this.ai[0] = 0f;
																										this.ai[1] = 0f;
																									}
																									if (Main.rand.Next(15) == 0)
																									{
																										float num1112 = -1f;
																										int num1113 = 17;
																										if ((base.Center - player10.Center).Length() < (float)Main.screenWidth)
																										{
																											int num1114 = (int)base.Center.X / 16;
																											int num1115 = (int)base.Center.Y / 16;
																											num1114 = (int)MathHelper.Clamp((float)num1114, (float)(num1113 + 1), (float)(Main.maxTilesX - num1113 - 1));
																											num1115 = (int)MathHelper.Clamp((float)num1115, (float)(num1113 + 1), (float)(Main.maxTilesY - num1113 - 1));
																											for (int num1116 = num1114 - num1113; num1116 <= num1114 + num1113; num1116++)
																											{
																												for (int num1117 = num1115 - num1113; num1117 <= num1115 + num1113; num1117++)
																												{
																													int num1118 = Main.rand.Next(8);
																													if (num1118 < 4)
																													{
																														Vector2 vector207 = new Vector2((float)(num1114 - num1116), (float)(num1115 - num1117));
																														if (vector207.Length() < (float)num1113 && Main.tile[num1116, num1117] != null && Main.tile[num1116, num1117].active())
																														{
																															bool flag70 = false;
																															if (Main.tile[num1116, num1117].type == 185 && Main.tile[num1116, num1117].frameY == 18)
																															{
																																if (Main.tile[num1116, num1117].frameX >= 576 && Main.tile[num1116, num1117].frameX <= 882)
																																{
																																	flag70 = true;
																																}
																															}
																															else if (Main.tile[num1116, num1117].type == 186 && Main.tile[num1116, num1117].frameX >= 864 && Main.tile[num1116, num1117].frameX <= 1170)
																															{
																																flag70 = true;
																															}
																															if (flag70 || Main.tileSpelunker[(int)Main.tile[num1116, num1117].type] || (Main.tileAlch[(int)Main.tile[num1116, num1117].type] && Main.tile[num1116, num1117].type != 82))
																															{
																																float num1119 = base.Distance(new Vector2((float)(num1116 * 16 + 8), (float)(num1117 * 16 + 8)));
																																if (num1119 < num1112 || num1112 == -1f)
																																{
																																	num1112 = num1119;
																																	this.ai[0] = 1f;
																																	this.ai[1] = base.AngleTo(new Vector2((float)(num1116 * 16 + 8), (float)(num1117 * 16 + 8)));
																																}
																															}
																														}
																													}
																												}
																											}
																										}
																									}
																									float f3 = this.localAI[0] % 6.28318548f - 3.14159274f;
																									float num1121 = (float)Math.IEEERemainder((double)this.localAI[1], 1.0);
																									if (num1121 < 0f)
																									{
																										num1121 += 1f;
																									}
																									float num1122 = (float)Math.Floor((double)this.localAI[1]);
																									float num1123 = 0.999f;
																									int num1124 = 0;
																									float num1125 = 0.1f;
																									bool flag71 = player10.velocity.Length() > 3f;
																									int num1126 = -1;
																									int num1127 = -1;
																									float num1128 = 300f;
																									float num1129 = 500f;
																									for (int num1130 = 0; num1130 < 200; num1130++)
																									{
																										NPC nPC16 = Main.npc[num1130];
																										if (nPC16.active && nPC16.chaseable && !nPC16.dontTakeDamage && !nPC16.immortal)
																										{
																											float num1131 = base.Distance(nPC16.Center);
																											if (nPC16.friendly || nPC16.lifeMax <= 5)
																											{
																												if (num1131 < num1128 && !flag71)
																												{
																													num1128 = num1131;
																													num1127 = num1130;
																												}
																											}
																											else if (num1131 < num1129)
																											{
																												num1129 = num1131;
																												num1126 = num1130;
																											}
																										}
																									}
																									float num1132;
																									if (flag71)
																									{
																										num1132 = base.AngleTo(base.Center + player10.velocity);
																										num1124 = 1;
																										num1121 = MathHelper.Clamp(num1121 + 0.05f, 0f, num1123);
																										num1122 += (float)Math.Sign(-10f - num1122);
																									}
																									else if (num1126 != -1)
																									{
																										num1132 = base.AngleTo(Main.npc[num1126].Center);
																										num1124 = 2;
																										num1121 = MathHelper.Clamp(num1121 + 0.05f, 0f, num1123);
																										num1122 += (float)Math.Sign(-12f - num1122);
																									}
																									else if (num1127 != -1)
																									{
																										num1132 = base.AngleTo(Main.npc[num1127].Center);
																										num1124 = 3;
																										num1121 = MathHelper.Clamp(num1121 + 0.05f, 0f, num1123);
																										num1122 += (float)Math.Sign(6f - num1122);
																									}
																									else if (this.ai[0] > 0f)
																									{
																										num1132 = this.ai[1];
																										num1121 = MathHelper.Clamp(num1121 + (float)Math.Sign(0.75f - num1121) * 0.05f, 0f, num1123);
																										num1124 = 4;
																										num1122 += (float)Math.Sign(10f - num1122);
																									}
																									else
																									{
																										num1132 = ((player10.direction == 1) ? 0f : 3.14160275f);
																										num1121 = MathHelper.Clamp(num1121 + (float)Math.Sign(0.75f - num1121) * 0.05f, 0f, num1123);
																										num1122 += (float)Math.Sign(0f - num1122);
																										num1125 = 0.12f;
																									}
																									Vector2 vector208 = num1132.ToRotationVector2();
																									num1132 = Vector2.Lerp(f3.ToRotationVector2(), vector208, num1125).ToRotation();
																									this.localAI[0] = num1132 + (float)num1124 * 6.28318548f + 3.14159274f;
																									this.localAI[1] = num1122 + num1121;
																									return;
																								}
																								else
																								{
																									if (this.aiStyle == 125)
																									{
																										Player player11 = Main.player[this.owner];
																										if (Main.myPlayer == this.owner)
																										{
																											if (this.localAI[1] > 0f)
																											{
																												this.localAI[1] -= 1f;
																											}
																											if (player11.noItems || player11.CCed || player11.dead)
																											{
																												this.Kill();
																											}
																											else if (Main.mouseRight && Main.mouseRightRelease)
																											{
																												this.Kill();
																												player11.mouseInterface = true;
																												Main.blockMouse = true;
																											}
																											else if (!player11.channel)
																											{
																												if (this.localAI[0] == 0f)
																												{
																													this.localAI[0] = 1f;
																												}
																												this.Kill();
																											}
																											else if (this.localAI[1] == 0f)
																											{
																												Vector2 vector211 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
																												if (player11.gravDir == -1f)
																												{
																													vector211.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y;
																												}
																												if (vector211 != base.Center)
																												{
																													this.netUpdate = true;
																													base.Center = vector211;
																													this.localAI[1] = 1f;
																												}
																												if (this.ai[0] == 0f && this.ai[1] == 0f)
																												{
																													this.ai[0] = (float)((int)base.Center.X / 16);
																													this.ai[1] = (float)((int)base.Center.Y / 16);
																													this.netUpdate = true;
																													this.velocity = Vector2.Zero;
																												}
																											}
																											this.velocity = Vector2.Zero;
																											Point point5 = new Vector2(this.ai[0], this.ai[1]).ToPoint();
																											Point point6 = base.Center.ToTileCoordinates();
																											Math.Abs(point5.X - point6.X);
																											Math.Abs(point5.Y - point6.Y);
																											int num1132 = Math.Sign(point6.X - point5.X);
																											int num1133 = Math.Sign(point6.Y - point5.Y);
																											Point point7 = default(Point);
																											bool flag72 = false;
																											bool flag73 = player11.direction == 1;
																											int num1134;
																											int num1135;
																											int num1136;
																											if (flag73)
																											{
																												point7.X = point5.X;
																												num1134 = point5.Y;
																												num1135 = point6.Y;
																												num1136 = num1133;
																											}
																											else
																											{
																												point7.Y = point5.Y;
																												num1134 = point5.X;
																												num1135 = point6.X;
																												num1136 = num1132;
																											}
																											int num1137 = num1134;
																											while (num1137 != num1135 && !flag72)
																											{
																												if (flag73)
																												{
																													point7.Y = num1137;
																												}
																												else
																												{
																													point7.X = num1137;
																												}
																												if (WorldGen.InWorld(point7.X, point7.Y, 1))
																												{
																													Tile tile2 = Main.tile[point7.X, point7.Y];
																												}
																												num1137 += num1136;
																											}
																											if (flag73)
																											{
																												point7.Y = point6.Y;
																												num1134 = point5.X;
																												num1135 = point6.X;
																												num1136 = num1132;
																											}
																											else
																											{
																												point7.X = point6.X;
																												num1134 = point5.Y;
																												num1135 = point6.Y;
																												num1136 = num1133;
																											}
																											int num1138 = num1134;
																											while (num1138 != num1135 && !flag72)
																											{
																												if (!flag73)
																												{
																													point7.Y = num1138;
																												}
																												else
																												{
																													point7.X = num1138;
																												}
																												if (WorldGen.InWorld(point7.X, point7.Y, 1))
																												{
																													Tile tile2 = Main.tile[point7.X, point7.Y];
																												}
																												num1138 += num1136;
																											}
																										}
																										int num1139 = Math.Sign(player11.velocity.X);
																										if (num1139 != 0)
																										{
																											player11.ChangeDir(num1139);
																										}
																										player11.heldProj = this.whoAmI;
																										player11.itemTime = 2;
																										player11.itemAnimation = 2;
																										player11.itemRotation = 0f;
																										return;
																									}
																									if (this.aiStyle == 126)
																									{
																										int num1140 = Math.Sign(this.velocity.Y);
																										int num1141 = (num1140 == -1) ? 0 : 1;
																										if (this.ai[0] == 0f)
																										{
																											if (!Collision.SolidCollision(this.position + new Vector2(0f, (float)((num1140 == -1) ? (this.height - 48) : 0)), this.width, 48) && !Collision.WetCollision(this.position + new Vector2(0f, (float)((num1140 == -1) ? (this.height - 20) : 0)), this.width, 20))
																											{
																												this.velocity = new Vector2(0f, (float)Math.Sign(this.velocity.Y) * 0.001f);
																												this.ai[0] = 1f;
																												this.ai[1] = 0f;
																												this.timeLeft = 60;
																											}
																											this.ai[1] += 1f;
																											if (this.ai[1] >= 60f)
																											{
																												this.Kill();
																											}
																										}
																										if (this.ai[0] == 1f)
																										{
																											this.velocity = new Vector2(0f, (float)Math.Sign(this.velocity.Y) * 0.001f);
																											if (num1140 != 0)
																											{
																												int num1144 = 16;
																												while (num1144 < 320 && !Collision.SolidCollision(this.position + new Vector2(0f, (float)((num1140 == -1) ? (this.height - num1144 - 16) : 0)), this.width, num1144 + 16))
																												{
																													num1144 += 16;
																												}
																												if (num1140 == -1)
																												{
																													this.position.Y = this.position.Y + (float)this.height;
																													this.height = num1144;
																													this.position.Y = this.position.Y - (float)num1144;
																												}
																												else
																												{
																													this.height = num1144;
																												}
																											}
																											this.ai[1] += 1f;
																											if (this.ai[1] >= 60f)
																											{
																												this.Kill();
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private void AI_001()
		{
			if (this.type == 469 && this.wet && !this.honeyWet)
			{
				this.Kill();
			}
			if (this.type == 601)
			{
				Color portalColor = PortalHelper.GetPortalColor(this.owner, (int)this.ai[0]);
				Vector3 vector = portalColor.ToVector3();
				vector *= 0.5f;
				if (this.alpha > 0 && this.alpha <= 15)
				{
				}
				this.alpha -= 15;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (++this.frameCounter >= 4)
				{
					this.frameCounter = 0;
					if (++this.frame >= Main.projFrames[this.type])
					{
						this.frame = 0;
					}
				}
			}
			if (this.type == 472)
			{
				if (this.localAI[0] == 0f)
				{
					this.localAI[0] = 1f;
				}
			}
			if (this.type == 323)
			{
				this.alpha -= 50;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			if (this.type == 436)
			{
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
				this.alpha -= 40;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				this.spriteDirection = this.direction;
				this.frameCounter++;
				if (this.frameCounter >= 3)
				{
					this.frame++;
					this.frameCounter = 0;
					if (this.frame >= 4)
					{
						this.frame = 0;
					}
				}
			}
			if (this.type == 467)
			{
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
				else if (this.ai[1] == 1f && Main.netMode != 1)
				{
					int num2 = -1;
					float num3 = 2000f;
					for (int k = 0; k < 255; k++)
					{
						if (Main.player[k].active && !Main.player[k].dead)
						{
							Vector2 center = Main.player[k].Center;
							float num4 = Vector2.Distance(center, base.Center);
							if ((num4 < num3 || num2 == -1) && Collision.CanHit(base.Center, 1, 1, center, 1, 1))
							{
								num3 = num4;
								num2 = k;
							}
						}
					}
					if (num3 < 20f)
					{
						this.Kill();
						return;
					}
					if (num2 != -1)
					{
						this.ai[1] = 21f;
						this.ai[0] = (float)num2;
						this.netUpdate = true;
					}
				}
				else if (this.ai[1] > 20f && this.ai[1] < 200f)
				{
					this.ai[1] += 1f;
					int num5 = (int)this.ai[0];
					if (!Main.player[num5].active || Main.player[num5].dead)
					{
						this.ai[1] = 1f;
						this.ai[0] = 0f;
						this.netUpdate = true;
					}
					else
					{
						float num6 = this.velocity.ToRotation();
						Vector2 vector2 = Main.player[num5].Center - base.Center;
						if (vector2.Length() < 20f)
						{
							this.Kill();
							return;
						}
						float targetAngle = vector2.ToRotation();
						if (vector2 == Vector2.Zero)
						{
							targetAngle = num6;
						}
						float num7 = num6.AngleLerp(targetAngle, 0.008f);
						this.velocity = new Vector2(this.velocity.Length(), 0f).RotatedBy((double)num7, default(Vector2));
					}
				}
				if (this.ai[1] >= 1f && this.ai[1] < 20f)
				{
					this.ai[1] += 1f;
					if (this.ai[1] == 20f)
					{
						this.ai[1] = 1f;
					}
				}
				this.alpha -= 40;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				this.spriteDirection = this.direction;
				this.frameCounter++;
				if (this.frameCounter >= 3)
				{
					this.frame++;
					this.frameCounter = 0;
					if (this.frame >= 4)
					{
						this.frame = 0;
					}
				}
				this.localAI[0] += 1f;
				if (this.localAI[0] == 12f)
				{
					this.localAI[0] = 0f;
				}
			}
			if (this.type == 468)
			{
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
				else if (this.ai[1] == 1f && Main.netMode != 1)
				{
					int num13 = -1;
					float num14 = 2000f;
					for (int num15 = 0; num15 < 255; num15++)
					{
						if (Main.player[num15].active && !Main.player[num15].dead)
						{
							Vector2 center2 = Main.player[num15].Center;
							float num16 = Vector2.Distance(center2, base.Center);
							if ((num16 < num14 || num13 == -1) && Collision.CanHit(base.Center, 1, 1, center2, 1, 1))
							{
								num14 = num16;
								num13 = num15;
							}
						}
					}
					if (num14 < 20f)
					{
						this.Kill();
						return;
					}
					if (num13 != -1)
					{
						this.ai[1] = 21f;
						this.ai[0] = (float)num13;
						this.netUpdate = true;
					}
				}
				else if (this.ai[1] > 20f && this.ai[1] < 200f)
				{
					this.ai[1] += 1f;
					int num17 = (int)this.ai[0];
					if (!Main.player[num17].active || Main.player[num17].dead)
					{
						this.ai[1] = 1f;
						this.ai[0] = 0f;
						this.netUpdate = true;
					}
					else
					{
						float num18 = this.velocity.ToRotation();
						Vector2 vector7 = Main.player[num17].Center - base.Center;
						if (vector7.Length() < 20f)
						{
							this.Kill();
							return;
						}
						float targetAngle2 = vector7.ToRotation();
						if (vector7 == Vector2.Zero)
						{
							targetAngle2 = num18;
						}
						float num19 = num18.AngleLerp(targetAngle2, 0.01f);
						this.velocity = new Vector2(this.velocity.Length(), 0f).RotatedBy((double)num19, default(Vector2));
					}
				}
				if (this.ai[1] >= 1f && this.ai[1] < 20f)
				{
					this.ai[1] += 1f;
					if (this.ai[1] == 20f)
					{
						this.ai[1] = 1f;
					}
				}
				this.alpha -= 40;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				this.spriteDirection = this.direction;
				this.frameCounter++;
				if (this.frameCounter >= 3)
				{
					this.frame++;
					this.frameCounter = 0;
					if (this.frame >= 4)
					{
						this.frame = 0;
					}
				}
				this.localAI[0] += 1f;
				if (this.localAI[0] == 12f)
				{
					this.localAI[0] = 0f;
				}
			}
			if (this.type == 634 || this.type == 635)
			{
				float num28 = 5f;
				float num29 = 250f;
				float num30 = 6f;
				Vector2 vector12 = new Vector2(8f, 10f);
				Vector3 rgb = new Vector3(0.7f, 0.1f, 0.5f);
				int num32 = 4 * this.MaxUpdates;
				int num33 = Utils.SelectRandom<int>(Main.rand, new int[]
				{
					242,
					73,
					72,
					71,
					255
				});
				if (this.type == 635)
				{
					vector12 = new Vector2(10f, 20f);
					num29 = 500f;
					num32 = 3 * this.MaxUpdates;
					rgb = new Vector3(0.4f, 0.6f, 0.9f);
					num33 = Utils.SelectRandom<int>(Main.rand, new int[]
					{
						242,
						59,
						88
					});
				}
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
					this.localAI[0] = (float)(-(float)Main.rand.Next(48));
				}
				else if (this.ai[1] == 1f && this.owner == Main.myPlayer)
				{
					int num35 = -1;
					float num36 = num29;
					for (int num37 = 0; num37 < 200; num37++)
					{
						if (Main.npc[num37].active && Main.npc[num37].CanBeChasedBy(this, false))
						{
							Vector2 center3 = Main.npc[num37].Center;
							float num38 = Vector2.Distance(center3, base.Center);
							if (num38 < num36 && num35 == -1 && Collision.CanHitLine(base.Center, 1, 1, center3, 1, 1))
							{
								num36 = num38;
								num35 = num37;
							}
						}
					}
					if (num36 < 20f)
					{
						this.Kill();
						return;
					}
					if (num35 != -1)
					{
						this.ai[1] = num28 + 1f;
						this.ai[0] = (float)num35;
						this.netUpdate = true;
					}
				}
				else if (this.ai[1] > num28)
				{
					this.ai[1] += 1f;
					int num39 = (int)this.ai[0];
					if (!Main.npc[num39].active || !Main.npc[num39].CanBeChasedBy(this, false))
					{
						this.ai[1] = 1f;
						this.ai[0] = 0f;
						this.netUpdate = true;
					}
					else
					{
						this.velocity.ToRotation();
						Vector2 vector13 = Main.npc[num39].Center - base.Center;
						if (vector13.Length() < 20f)
						{
							this.Kill();
							return;
						}
						if (vector13 != Vector2.Zero)
						{
							vector13.Normalize();
							vector13 *= num30;
						}
						float num40 = 30f;
						this.velocity = (this.velocity * (num40 - 1f) + vector13) / num40;
					}
				}
				if (this.ai[1] >= 1f && this.ai[1] < num28)
				{
					this.ai[1] += 1f;
					if (this.ai[1] == num28)
					{
						this.ai[1] = 1f;
					}
				}
				this.alpha -= 40;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				this.spriteDirection = this.direction;
				this.frameCounter++;
				if (this.frameCounter >= num32)
				{
					this.frame++;
					this.frameCounter = 0;
					if (this.frame >= 4)
					{
						this.frame = 0;
					}
				}
				this.rotation = this.velocity.ToRotation();
				this.localAI[0] += 1f;
				if (this.localAI[0] == 48f)
				{
					this.localAI[0] = 0f;
				}
			}
			if (this.type == 459)
			{
				this.alpha -= 30;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				this.spriteDirection = this.direction;
				this.frameCounter++;
				if (this.frameCounter >= 3)
				{
					this.frame++;
					this.frameCounter = 0;
					if (this.frame >= 3)
					{
						this.frame = 0;
					}
				}
				this.position = base.Center;
				this.scale = this.ai[1];
				this.width = (this.height = (int)(22f * this.scale));
				base.Center = this.position;
				int num51;
				if ((double)this.scale < 0.85)
				{
					num51 = ((Main.rand.Next(3) == 0) ? 1 : 0);
				}
				else
				{
					num51 = 1;
					this.penetrate = -1;
					this.maxPenetrate = -1;
				}
			}
			if (this.type == 442)
			{
				this.frame = 0;
				if (this.alpha != 0)
				{
					this.localAI[0] += 1f;
					if (this.localAI[0] >= 4f)
					{
						this.alpha -= 90;
						if (this.alpha < 0)
						{
							this.alpha = 0;
							this.localAI[0] = 2f;
						}
					}
				}
				if (Vector2.Distance(base.Center, new Vector2(this.ai[0], this.ai[1]) * 16f + Vector2.One * 8f) <= 16f)
				{
					this.Kill();
					return;
				}
				if (this.alpha == 0)
				{
					this.localAI[1] += 1f;
					if (this.localAI[1] >= 120f)
					{
						this.Kill();
						return;
					}
					this.localAI[0] += 1f;
					if (this.localAI[0] == 3f)
					{
						this.localAI[0] = 0f;
					}
				}
			}
			if (this.type == 440 || this.type == 449 || this.type == 606)
			{
				if (this.alpha > 0)
				{
					this.alpha -= 25;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				float num56 = 100f;
				float num57 = 3f;
				if (this.type == 606)
				{
					num56 = 150f;
					num57 = 5f;
				}
				if (this.ai[1] == 0f)
				{
					this.localAI[0] += num57;
					if (this.localAI[0] > num56)
					{
						this.localAI[0] = num56;
					}
				}
				else
				{
					this.localAI[0] -= num57;
					if (this.localAI[0] <= 0f)
					{
						this.Kill();
						return;
					}
				}
			}
			if (this.type == 593)
			{
				if (++this.frameCounter >= 12)
				{
					if (++this.frame >= Main.projFrames[this.type])
					{
						this.frame = 0;
					}
					this.frameCounter = 0;
				}
			}
			if (this.type == 462)
			{
				if (++this.frameCounter >= 9)
				{
					this.frameCounter = 0;
					if (++this.frame >= 5)
					{
						this.frame = 0;
					}
				}
			}
			if (this.type == 437)
			{
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
				if (this.localAI[0] == 0f)
				{
					this.localAI[0] = 1f;
				}
			}
			if (this.type == 435)
			{
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
				this.alpha -= 40;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				this.spriteDirection = this.direction;
				this.frameCounter++;
				if (this.frameCounter >= 3)
				{
					this.frame++;
					this.frameCounter = 0;
					if (this.frame >= 4)
					{
						this.frame = 0;
					}
				}
			}
			if (this.type == 408)
			{
				this.alpha -= 40;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				this.spriteDirection = this.direction;
			}
			if (this.type == 275 || this.type == 276)
			{
				this.frameCounter++;
				if (this.frameCounter > 1)
				{
					this.frameCounter = 0;
					this.frame++;
					if (this.frame > 1)
					{
						this.frame = 0;
					}
				}
			}
			if (this.type == 174)
			{
				this.alpha -= 50;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
			}
			else if (this.type == 605)
			{
				this.alpha -= 50;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
			}
			else if (this.type == 176)
			{
				this.alpha -= 50;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
			}
			if (this.type == 350)
			{
				this.alpha -= 100;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
			}
			if (this.type == 325)
			{
				this.alpha -= 100;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (this.ai[1] == 0f)
				{
					this.ai[1] = 1f;
				}
			}
			if (this.type == 469)
			{
				this.localAI[1] += 1f;
				if (this.localAI[1] > 2f)
				{
					this.alpha -= 50;
					if (this.alpha < 0)
					{
						this.alpha = 0;
					}
				}
			}
			else if (this.type == 83 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 408 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 259 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 110 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 302 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 438 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 593 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 592 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 462 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 84 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 389 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 257 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 100 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 98 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 184 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 195 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 275 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 276 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if ((this.type == 81 || this.type == 82) && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 180 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 248 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 576 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 577 && this.ai[1] == 0f)
			{
				this.ai[1] = 1f;
			}
			else if (this.type == 639)
			{
				if (this.localAI[0] == 0f && this.localAI[1] == 0f)
				{
					this.localAI[0] = base.Center.X;
					this.localAI[1] = base.Center.Y;
					this.ai[0] = this.velocity.X;
					this.ai[1] = this.velocity.Y;
				}
				this.alpha -= 25;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			else if (this.type == 640)
			{
				this.alpha -= 25;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (this.velocity == Vector2.Zero)
				{
					this.ai[0] = 0f;
					bool flag = true;
					for (int num79 = 1; num79 < this.oldPos.Length; num79++)
					{
						if (this.oldPos[num79] != this.oldPos[0])
						{
							flag = false;
						}
					}
					if (flag)
					{
						this.Kill();
						return;
					}
				}
			}
			else if (this.type == 376)
			{
				this.localAI[0] += 1f;
				if (this.localAI[0] > 3f)
				{
					if (this.wet && !this.lavaWet)
					{
						this.Kill();
						return;
					}
				}
			}
			if (this.type == 163 || this.type == 310)
			{
				if (this.alpha > 0)
				{
					this.alpha -= 25;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			int num96 = this.type;
			if (num96 <= 100)
			{
				if (num96 <= 36)
				{
					if (num96 != 14 && num96 != 20 && num96 != 36)
					{
						goto IL_49CE;
					}
				}
				else
				{
					switch (num96)
					{
					case 83:
					case 84:
						break;
					default:
						if (num96 != 89 && num96 != 100)
						{
							goto IL_49CE;
						}
						break;
					}
				}
			}
			else if (num96 <= 161)
			{
				if (num96 != 104 && num96 != 110)
				{
					switch (num96)
					{
					case 158:
					case 159:
					case 160:
					case 161:
						break;
					default:
						goto IL_49CE;
					}
				}
			}
			else if (num96 <= 287)
			{
				if (num96 != 180)
				{
					switch (num96)
					{
					case 279:
					case 283:
					case 284:
					case 285:
					case 286:
					case 287:
						break;
					case 280:
					case 281:
					case 282:
						goto IL_49CE;
					default:
						goto IL_49CE;
					}
				}
			}
			else if (num96 != 389)
			{
				switch (num96)
				{
				case 576:
				case 577:
					this.localAI[1] += 1f;
					if (this.localAI[1] <= 2f)
					{
						goto IL_49CE;
					}
					if (this.alpha > 0)
					{
						this.alpha -= 15;
					}
					if (this.alpha < 0)
					{
						this.alpha = 0;
						goto IL_49CE;
					}
					goto IL_49CE;
				default:
					goto IL_49CE;
				}
			}
			if (this.alpha > 0)
			{
				this.alpha -= 15;
			}
			if (this.alpha < 0)
			{
				this.alpha = 0;
			}
			IL_49CE:
			if (this.type == 242 || this.type == 302 || this.type == 438 || this.type == 462 || this.type == 592)
			{
				float num98 = (float)Math.Sqrt((double)(this.velocity.X * this.velocity.X + this.velocity.Y * this.velocity.Y));
				if (this.alpha > 0)
				{
					this.alpha -= (int)((byte)((double)num98 * 0.9));
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			if (this.type == 638)
			{
				float num99 = this.velocity.Length();
				if (this.alpha > 0)
				{
					this.alpha -= (int)((byte)((double)num99 * 0.3));
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				Rectangle hitbox = base.Hitbox;
				hitbox.Offset((int)this.velocity.X, (int)this.velocity.Y);
				bool flag2 = false;
				for (int num100 = 0; num100 < 200; num100++)
				{
					if (Main.npc[num100].active && !Main.npc[num100].dontTakeDamage && Main.npc[num100].immune[this.owner] == 0 && this.npcImmune[num100] == 0 && Main.npc[num100].Hitbox.Intersects(hitbox))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
				}
			}
			if (this.type == 257 || this.type == 593)
			{
				if (this.alpha > 0)
				{
					this.alpha -= 10;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			if (this.type == 88)
			{
				if (this.alpha > 0)
				{
					this.alpha -= 10;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			if (this.type == 532)
			{
				this.ai[0] += 1f;
			}
			bool flag3 = true;
			num96 = this.type;
			if (num96 <= 299)
			{
				if (num96 <= 110)
				{
					if (num96 <= 38)
					{
						if (num96 <= 14)
						{
							if (num96 != 5 && num96 != 14)
							{
								goto IL_51CE;
							}
						}
						else if (num96 != 20)
						{
							switch (num96)
							{
							case 36:
							case 38:
								break;
							case 37:
								goto IL_51CE;
							default:
								goto IL_51CE;
							}
						}
					}
					else if (num96 <= 89)
					{
						if (num96 != 55)
						{
							switch (num96)
							{
							case 83:
							case 84:
							case 88:
							case 89:
								break;
							case 85:
							case 86:
							case 87:
								goto IL_51CE;
							default:
								goto IL_51CE;
							}
						}
					}
					else
					{
						switch (num96)
						{
						case 98:
						case 100:
							break;
						case 99:
							goto IL_51CE;
						default:
							if (num96 != 104 && num96 != 110)
							{
								goto IL_51CE;
							}
							break;
						}
					}
				}
				else if (num96 <= 248)
				{
					if (num96 <= 180)
					{
						switch (num96)
						{
						case 158:
						case 159:
						case 160:
						case 161:
							break;
						default:
							if (num96 != 180)
							{
								goto IL_51CE;
							}
							break;
						}
					}
					else if (num96 != 184 && num96 != 242 && num96 != 248)
					{
						goto IL_51CE;
					}
				}
				else if (num96 <= 265)
				{
					switch (num96)
					{
					case 257:
					case 259:
						break;
					case 258:
						goto IL_51CE;
					default:
						if (num96 != 265)
						{
							goto IL_51CE;
						}
						break;
					}
				}
				else if (num96 != 270)
				{
					switch (num96)
					{
					case 279:
					case 283:
					case 284:
					case 285:
					case 286:
					case 287:
						break;
					case 280:
					case 281:
					case 282:
						goto IL_51CE;
					default:
						if (num96 != 299)
						{
							goto IL_51CE;
						}
						break;
					}
				}
			}
			else if (num96 <= 462)
			{
				if (num96 <= 376)
				{
					if (num96 <= 325)
					{
						if (num96 != 302)
						{
							switch (num96)
							{
							case 323:
							case 325:
								break;
							case 324:
								goto IL_51CE;
							default:
								goto IL_51CE;
							}
						}
					}
					else
					{
						switch (num96)
						{
						case 348:
						case 349:
						case 350:
							break;
						default:
							if (num96 != 355)
							{
								switch (num96)
								{
								case 374:
								case 376:
									break;
								case 375:
									goto IL_51CE;
								default:
									goto IL_51CE;
								}
							}
							break;
						}
					}
				}
				else if (num96 <= 442)
				{
					if (num96 != 389)
					{
						switch (num96)
						{
						case 435:
						case 436:
						case 438:
						case 440:
						case 442:
							break;
						case 437:
						case 439:
						case 441:
							goto IL_51CE;
						default:
							goto IL_51CE;
						}
					}
				}
				else if (num96 != 449 && num96 != 459 && num96 != 462)
				{
					goto IL_51CE;
				}
			}
			else if (num96 <= 585)
			{
				if (num96 <= 485)
				{
					switch (num96)
					{
					case 467:
					case 468:
					case 469:
					case 472:
						break;
					case 470:
					case 471:
						goto IL_51CE;
					default:
						switch (num96)
						{
						case 483:
						case 484:
						case 485:
							break;
						default:
							goto IL_51CE;
						}
						break;
					}
				}
				else if (num96 != 498)
				{
					switch (num96)
					{
					case 576:
					case 577:
						break;
					default:
						if (num96 != 585)
						{
							goto IL_51CE;
						}
						break;
					}
				}
			}
			else if (num96 <= 601)
			{
				switch (num96)
				{
				case 592:
				case 593:
					break;
				default:
					if (num96 != 601)
					{
						goto IL_51CE;
					}
					break;
				}
			}
			else if (num96 != 606 && num96 != 616)
			{
				switch (num96)
				{
				case 634:
				case 635:
				case 638:
				case 639:
					break;
				case 636:
				case 637:
					goto IL_51CE;
				default:
					goto IL_51CE;
				}
			}
			flag3 = false;
			IL_51CE:
			if (flag3)
			{
				this.ai[0] += 1f;
			}
			if (this.type == 270)
			{
				int num104 = (int)Player.FindClosest(base.Center, 1, 1);
				this.ai[1] += 1f;
				if (this.ai[1] < 110f && this.ai[1] > 30f)
				{
					float num105 = this.velocity.Length();
					Vector2 vector26 = Main.player[num104].Center - base.Center;
					vector26.Normalize();
					vector26 *= num105;
					this.velocity = (this.velocity * 24f + vector26) / 25f;
					this.velocity.Normalize();
					this.velocity *= num105;
				}
				if (this.ai[0] < 0f)
				{
					if (this.velocity.Length() < 18f)
					{
						this.velocity *= 1.02f;
					}
					if (this.localAI[0] == 0f)
					{
						this.localAI[0] = 1f;
					}
					this.friendly = false;
					this.hostile = true;
				}
			}
			if (this.type == 585)
			{
				if (this.localAI[0] == 0f)
				{
					this.localAI[0] = 1f;
				}
				if (this.alpha > 0)
				{
					this.alpha -= 50;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				this.frameCounter++;
				if (this.frameCounter >= 12)
				{
					this.frameCounter = 0;
				}
				this.frame = this.frameCounter / 2;
				if (this.frame > 3)
				{
					this.frame = 6 - this.frame;
				}
				Vector3 vector27 = NPCID.Sets.MagicAuraColor[54].ToVector3();
			}
			if (this.type == 594)
			{
				int num111 = (int)(43f - this.ai[1]) / 13;
				if (num111 < 1)
				{
					num111 = 1;
				}
				this.ai[1] += 1f;
				if (this.ai[1] > (float)(43 * this.MaxUpdates))
				{
					this.Kill();
					return;
				}
			}
			if (this.type == 622)
			{
				this.ai[1] += 1f;
				if (this.ai[1] > (float)(23 * this.MaxUpdates))
				{
					this.Kill();
					return;
				}
			}
			if (this.type == 587)
			{
				Color newColor = Main.HslToRgb(this.ai[1], 1f, 0.5f);
				newColor.A = 200;
				this.localAI[0] += 1f;
				if (this.localAI[0] >= 2f)
				{
						this.frame++;
						if (this.frame > 2)
						{
							this.frame = 0;
						}
				}
			}
			if (this.type == 349)
			{
				this.frame = (int)this.ai[0];
				this.velocity.Y = this.velocity.Y + 0.2f;
				if (this.localAI[0] == 0f || this.localAI[0] == 2f)
				{
					this.scale += 0.01f;
					this.alpha -= 50;
					if (this.alpha <= 0)
					{
						this.localAI[0] = 1f;
						this.alpha = 0;
					}
				}
				else if (this.localAI[0] == 1f)
				{
					this.scale -= 0.01f;
					this.alpha += 50;
					if (this.alpha >= 255)
					{
						this.localAI[0] = 2f;
						this.alpha = 255;
					}
				}
			}
			if (this.type == 348)
			{
				if (this.localAI[1] == 0f)
				{
					this.localAI[1] = 1f;
				}
				if (this.ai[0] == 0f || this.ai[0] == 2f)
				{
					this.scale += 0.01f;
					this.alpha -= 50;
					if (this.alpha <= 0)
					{
						this.ai[0] = 1f;
						this.alpha = 0;
					}
				}
				else if (this.ai[0] == 1f)
				{
					this.scale -= 0.01f;
					this.alpha += 50;
					if (this.alpha >= 255)
					{
						this.ai[0] = 2f;
						this.alpha = 255;
					}
				}
			}
			if (this.type == 572)
			{
				if (this.localAI[0] == 0f)
				{
					this.localAI[0] = 1f;
				}
			}
			else if (this.type == 581)
			{
				if (this.localAI[0] == 0f)
				{
					this.localAI[0] = 1f;
				}
			}
			if (this.type == 299)
			{
				this.localAI[0] += 1f;
			}
			else if (this.type == 270)
			{
				if (this.ai[0] < 0f)
				{
					this.alpha = 0;
				}
				if (this.alpha > 0)
				{
					this.alpha -= 50;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				this.frame++;
				if (this.frame > 2)
				{
					this.frame = 0;
				}
			}
			if (this.type == 259)
			{
				if (this.alpha > 0)
				{
					this.alpha -= 10;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			if (this.type == 265)
			{
				if (this.alpha > 0)
				{
					this.alpha -= 50;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			if (this.type == 355)
			{
				if (this.alpha > 0)
				{
					this.alpha -= 50;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			if (this.type == 357)
			{
				if (this.alpha > 0)
				{
					this.alpha -= 25;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
			}
			else if (this.type == 207)
			{
				float num144 = (float)Math.Sqrt((double)(this.velocity.X * this.velocity.X + this.velocity.Y * this.velocity.Y));
				float num145 = this.localAI[0];
				if (num145 == 0f)
				{
					this.localAI[0] = num144;
					num145 = num144;
				}
				if (this.alpha > 0)
				{
					this.alpha -= 25;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				float num146 = this.position.X;
				float num147 = this.position.Y;
				float num148 = 300f;
				bool flag4 = false;
				int num149 = 0;
				if (this.ai[1] == 0f)
				{
					for (int num150 = 0; num150 < 200; num150++)
					{
						if (Main.npc[num150].CanBeChasedBy(this, false) && (this.ai[1] == 0f || this.ai[1] == (float)(num150 + 1)))
						{
							float num151 = Main.npc[num150].position.X + (float)(Main.npc[num150].width / 2);
							float num152 = Main.npc[num150].position.Y + (float)(Main.npc[num150].height / 2);
							float num153 = Math.Abs(this.position.X + (float)(this.width / 2) - num151) + Math.Abs(this.position.Y + (float)(this.height / 2) - num152);
							if (num153 < num148 && Collision.CanHit(new Vector2(this.position.X + (float)(this.width / 2), this.position.Y + (float)(this.height / 2)), 1, 1, Main.npc[num150].position, Main.npc[num150].width, Main.npc[num150].height))
							{
								num148 = num153;
								num146 = num151;
								num147 = num152;
								flag4 = true;
								num149 = num150;
							}
						}
					}
					if (flag4)
					{
						this.ai[1] = (float)(num149 + 1);
					}
					flag4 = false;
				}
				if (this.ai[1] > 0f)
				{
					int num154 = (int)(this.ai[1] - 1f);
					if (Main.npc[num154].active && Main.npc[num154].CanBeChasedBy(this, true) && !Main.npc[num154].dontTakeDamage)
					{
						float num155 = Main.npc[num154].position.X + (float)(Main.npc[num154].width / 2);
						float num156 = Main.npc[num154].position.Y + (float)(Main.npc[num154].height / 2);
						float num157 = Math.Abs(this.position.X + (float)(this.width / 2) - num155) + Math.Abs(this.position.Y + (float)(this.height / 2) - num156);
						if (num157 < 1000f)
						{
							flag4 = true;
							num146 = Main.npc[num154].position.X + (float)(Main.npc[num154].width / 2);
							num147 = Main.npc[num154].position.Y + (float)(Main.npc[num154].height / 2);
						}
					}
					else
					{
						this.ai[1] = 0f;
					}
				}
				if (!this.friendly)
				{
					flag4 = false;
				}
				if (flag4)
				{
					float num158 = num145;
					Vector2 vector28 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
					float num159 = num146 - vector28.X;
					float num160 = num147 - vector28.Y;
					float num161 = (float)Math.Sqrt((double)(num159 * num159 + num160 * num160));
					num161 = num158 / num161;
					num159 *= num161;
					num160 *= num161;
					int num162 = 8;
					this.velocity.X = (this.velocity.X * (float)(num162 - 1) + num159) / (float)num162;
					this.velocity.Y = (this.velocity.Y * (float)(num162 - 1) + num160) / (float)num162;
				}
			}
			else if (this.type == 81 || this.type == 91)
			{
				if (this.ai[0] >= 20f)
				{
					this.ai[0] = 20f;
					this.velocity.Y = this.velocity.Y + 0.07f;
				}
			}
			else if (this.type == 174 || this.type == 605)
			{
				if (this.ai[0] >= 5f)
				{
					this.ai[0] = 5f;
					this.velocity.Y = this.velocity.Y + 0.15f;
				}
			}
			else if (this.type == 337)
			{
				if (this.position.Y > Main.player[this.owner].position.Y - 300f)
				{
					this.tileCollide = true;
				}
				if ((double)this.position.Y < Main.worldSurface * 16.0)
				{
					this.tileCollide = true;
				}
				this.frame = (int)this.ai[1];
			}
			else if (this.type == 645)
			{
				if (this.ai[1] != -1f && this.position.Y > this.ai[1])
				{
					this.tileCollide = true;
				}
				if (this.position.HasNaNs())
				{
					this.Kill();
					return;
				}
				if (this.ai[1] == -1f)
				{
					this.ai[0] += 1f;
					this.velocity = Vector2.Zero;
					this.tileCollide = false;
					this.penetrate = -1;
					this.position = base.Center;
					this.width = (this.height = 140);
					base.Center = this.position;
					this.alpha -= 10;
					if (this.alpha < 0)
					{
						this.alpha = 0;
					}
					if (++this.frameCounter >= this.MaxUpdates * 3)
					{
						this.frameCounter = 0;
						this.frame++;
					}
					if (this.ai[0] >= (float)(Main.projFrames[this.type] * this.MaxUpdates * 3))
					{
						this.Kill();
					}
					return;
				}
				this.alpha = 255;
				if (this.numUpdates == 0)
				{
					int num164 = -1;
					float num165 = 60f;
					for (int num166 = 0; num166 < 200; num166++)
					{
						NPC nPC = Main.npc[num166];
						if (nPC.CanBeChasedBy(this, false))
						{
							float num167 = base.Distance(nPC.Center);
							if (num167 < num165 && Collision.CanHitLine(base.Center, 0, 0, nPC.Center, 0, 0))
							{
								num165 = num167;
								num164 = num166;
							}
						}
					}
					if (num164 != -1)
					{
						this.ai[0] = 0f;
						this.ai[1] = -1f;
						this.netUpdate = true;
						return;
					}
				}
			}
			else if (this.type >= 424 && this.type <= 426)
			{
				if (this.position.Y > Main.player[this.owner].position.Y - 300f)
				{
					this.tileCollide = true;
				}
				if ((double)this.position.Y < Main.worldSurface * 16.0)
				{
					this.tileCollide = true;
				}
				this.scale = this.ai[1];
				this.rotation += this.velocity.X * 2f;
			}
			else if (this.type == 344)
			{
				if (WorldGen.SolidTile((int)this.position.X / 16, (int)(this.position.Y + this.velocity.Y) / 16 + 1) || WorldGen.SolidTile((int)(this.position.X + (float)this.width) / 16, (int)(this.position.Y + this.velocity.Y) / 16 + 1))
				{
					this.Kill();
					return;
				}
				this.localAI[1] += 1f;
				if (this.localAI[1] > 5f)
				{
					this.alpha -= 50;
					if (this.alpha < 0)
					{
						this.alpha = 0;
					}
				}
				this.frame = (int)this.ai[1];
				if (this.localAI[1] > 20f)
				{
					this.localAI[1] = 20f;
					this.velocity.Y = this.velocity.Y + 0.15f;
				}
				this.rotation += Main.windSpeed * 0.2f;
				this.velocity.X = this.velocity.X + Main.windSpeed * 0.1f;
			}
			else if (this.type == 336 || this.type == 345)
			{
				if (this.type == 345 && this.localAI[0] == 0f)
				{
					this.localAI[0] = 1f;
				}
				if (this.ai[0] >= 50f)
				{
					this.ai[0] = 50f;
					this.velocity.Y = this.velocity.Y + 0.5f;
				}
			}
			else if (this.type == 246)
			{
				this.alpha -= 20;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (this.ai[0] >= 60f)
				{
					this.ai[0] = 60f;
					this.velocity.Y = this.velocity.Y + 0.15f;
				}
			}
			else if (this.type == 311)
			{
				if (this.alpha > 0)
				{
					this.alpha -= 50;
				}
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (this.ai[0] >= 30f)
				{
					this.ai[0] = 30f;
					if (this.ai[1] == 0f)
					{
						this.ai[1] = 1f;
					}
					this.velocity.Y = this.velocity.Y + 0.5f;
				}
			}
			else if (this.type == 312)
			{
				if (this.ai[0] >= 5f)
				{
					this.alpha = 0;
				}
				if (this.ai[0] >= 20f)
				{
					this.ai[0] = 30f;
					this.velocity.Y = this.velocity.Y + 0.5f;
				}
			}
			else if (this.type != 239 && this.type != 264)
			{
				if (this.type == 176)
				{
					if (this.ai[0] >= 15f)
					{
						this.ai[0] = 15f;
						this.velocity.Y = this.velocity.Y + 0.05f;
					}
				}
				else if (this.type == 275 || this.type == 276)
				{
					if (this.alpha > 0)
					{
						this.alpha -= 30;
					}
					if (this.alpha < 0)
					{
						this.alpha = 0;
					}
					if (this.ai[0] >= 35f)
					{
						this.ai[0] = 35f;
						this.velocity.Y = this.velocity.Y + 0.025f;
					}
					if (Main.expertMode)
					{
						float num170 = 18f;
						int num171 = (int)Player.FindClosest(base.Center, 1, 1);
						Vector2 vector29 = Main.player[num171].Center - base.Center;
						vector29.Normalize();
						vector29 *= num170;
						int num172 = 70;
						this.velocity = (this.velocity * (float)(num172 - 1) + vector29) / (float)num172;
						if (this.velocity.Length() < 14f)
						{
							this.velocity.Normalize();
							this.velocity *= 14f;
						}
						this.tileCollide = false;
						if (this.timeLeft > 180)
						{
							this.timeLeft = 180;
						}
					}
				}
				else if (this.type == 172)
				{
					if (this.ai[0] >= 17f)
					{
						this.ai[0] = 17f;
						this.velocity.Y = this.velocity.Y + 0.085f;
					}
				}
				else if (this.type == 117)
				{
					if (this.ai[0] >= 35f)
					{
						this.ai[0] = 35f;
						this.velocity.Y = this.velocity.Y + 0.06f;
					}
				}
				else if (this.type == 120)
				{
					if (this.ai[0] >= 30f)
					{
						this.ai[0] = 30f;
						this.velocity.Y = this.velocity.Y + 0.05f;
					}
				}
				else if (this.type == 195)
				{
					if (this.ai[0] >= 20f)
					{
						this.ai[0] = 20f;
						this.velocity.Y = this.velocity.Y + 0.075f;
						this.tileCollide = true;
					}
					else
					{
						this.tileCollide = false;
					}
				}
				else if (this.type == 267 || this.type == 477 || this.type == 478 || this.type == 479)
				{
					this.localAI[0] += 1f;
					if (this.localAI[0] > 3f)
					{
						this.alpha = 0;
					}
					if (this.ai[0] >= 20f)
					{
						this.ai[0] = 20f;
						if (this.type != 477)
						{
							this.velocity.Y = this.velocity.Y + 0.075f;
						}
					}
					if (this.type == 479 && Main.myPlayer == this.owner)
					{
						if (this.ai[1] >= 0f)
						{
							this.penetrate = -1;
						}
						else if (this.penetrate < 0)
						{
							this.penetrate = 1;
						}
						if (this.ai[1] >= 0f)
						{
							this.ai[1] += 1f;
						}
						if (this.ai[1] > (float)Main.rand.Next(5, 30))
						{
							this.ai[1] = -1000f;
							float num174 = this.velocity.Length();
							Vector2 velocity = this.velocity;
							velocity.Normalize();
							int num175 = Main.rand.Next(2, 4);
							if (Main.rand.Next(4) == 0)
							{
								num175++;
							}
							for (int num176 = 0; num176 < num175; num176++)
							{
								Vector2 vector30 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
								vector30.Normalize();
								vector30 += velocity * 2f;
								vector30.Normalize();
								vector30 *= num174;
								Projectile.NewProjectile(base.Center.X, base.Center.Y, vector30.X, vector30.Y, this.type, this.damage, this.knockBack, this.owner, 0f, -1000f);
							}
						}
					}
					if (this.type == 478 && Main.myPlayer == this.owner)
					{
						this.ai[1] += 1f;
						if (this.ai[1] > (float)Main.rand.Next(5, 20))
						{
							if (this.timeLeft > 40)
							{
								this.timeLeft -= 20;
							}
							this.ai[1] = 0f;
							Projectile.NewProjectile(base.Center.X, base.Center.Y, 0f, 0f, 480, (int)((double)this.damage * 0.8), this.knockBack * 0.5f, this.owner, 0f, 0f);
						}
					}
				}
				else if (this.type == 408)
				{
					if (this.ai[0] >= 45f)
					{
						this.ai[0] = 45f;
						this.velocity.Y = this.velocity.Y + 0.05f;
					}
				}
				else if (this.type == 616)
				{
					if (this.alpha < 170)
					{
					}
					float num180 = (float)Math.Sqrt((double)(this.velocity.X * this.velocity.X + this.velocity.Y * this.velocity.Y));
					float num181 = this.localAI[0];
					if (num181 == 0f)
					{
						this.localAI[0] = num180;
						num181 = num180;
					}
					if (this.alpha > 0)
					{
						this.alpha -= 25;
					}
					if (this.alpha < 0)
					{
						this.alpha = 0;
					}
					float num182 = this.position.X;
					float num183 = this.position.Y;
					float num184 = 800f;
					bool flag6 = false;
					int num185 = 0;
					this.ai[0] += 1f;
					if (this.ai[0] > 20f)
					{
						this.ai[0] -= 1f;
						if (this.ai[1] == 0f)
						{
							for (int num186 = 0; num186 < 200; num186++)
							{
								if (Main.npc[num186].CanBeChasedBy(this, false) && (this.ai[1] == 0f || this.ai[1] == (float)(num186 + 1)))
								{
									float num187 = Main.npc[num186].position.X + (float)(Main.npc[num186].width / 2);
									float num188 = Main.npc[num186].position.Y + (float)(Main.npc[num186].height / 2);
									float num189 = Math.Abs(this.position.X + (float)(this.width / 2) - num187) + Math.Abs(this.position.Y + (float)(this.height / 2) - num188);
									if (num189 < num184 && Collision.CanHit(new Vector2(this.position.X + (float)(this.width / 2), this.position.Y + (float)(this.height / 2)), 1, 1, Main.npc[num186].position, Main.npc[num186].width, Main.npc[num186].height))
									{
										num184 = num189;
										num182 = num187;
										num183 = num188;
										flag6 = true;
										num185 = num186;
									}
								}
							}
							if (flag6)
							{
								this.ai[1] = (float)(num185 + 1);
							}
							flag6 = false;
						}
						if (this.ai[1] != 0f)
						{
							int num190 = (int)(this.ai[1] - 1f);
							if (Main.npc[num190].active && Main.npc[num190].CanBeChasedBy(this, true))
							{
								float num191 = Main.npc[num190].position.X + (float)(Main.npc[num190].width / 2);
								float num192 = Main.npc[num190].position.Y + (float)(Main.npc[num190].height / 2);
								float num193 = Math.Abs(this.position.X + (float)(this.width / 2) - num191) + Math.Abs(this.position.Y + (float)(this.height / 2) - num192);
								if (num193 < 1000f)
								{
									flag6 = true;
									num182 = Main.npc[num190].position.X + (float)(Main.npc[num190].width / 2);
									num183 = Main.npc[num190].position.Y + (float)(Main.npc[num190].height / 2);
								}
							}
						}
						if (!this.friendly)
						{
							flag6 = false;
						}
						if (flag6)
						{
							float num194 = num181;
							Vector2 vector31 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
							float num195 = num182 - vector31.X;
							float num196 = num183 - vector31.Y;
							float num197 = (float)Math.Sqrt((double)(num195 * num195 + num196 * num196));
							num197 = num194 / num197;
							num195 *= num197;
							num196 *= num197;
							int num198 = 8;
							this.velocity.X = (this.velocity.X * (float)(num198 - 1) + num195) / (float)num198;
							this.velocity.Y = (this.velocity.Y * (float)(num198 - 1) + num196) / (float)num198;
						}
					}
				}
				else if (this.type == 507 || this.type == 508)
				{
					if (this.ai[0] > 45f)
					{
						this.velocity.X = this.velocity.X * 0.98f;
						this.velocity.Y = this.velocity.Y + 0.3f;
					}
				}
				else if (this.type == 495)
				{
					if (this.ai[0] >= 30f)
					{
						this.ai[0] = 30f;
						this.velocity.Y = this.velocity.Y + 0.04f;
					}
				}
				else if (this.type == 498)
				{
					if (this.localAI[0] == 0f)
					{
						this.localAI[0] += 1f;
					}
					this.ai[0] += 1f;
					if (this.ai[0] >= 50f)
					{
						this.velocity.X = this.velocity.X * 0.98f;
						this.velocity.Y = this.velocity.Y + 0.15f;
						this.rotation += (float)this.direction * 0.5f;
					}
					else
					{
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
					}
				}
				else if (this.type == 437)
				{
					if (this.ai[0] >= 12f)
					{
						if (this.ai[0] >= 20f)
						{
							this.Kill();
						}
						this.alpha += 30;
					}
				}
				else if (this.type != 442 && this.type != 634 && this.type != 635)
				{
					if (this.type == 639)
					{
						if (this.timeLeft <= this.MaxUpdates * 45 - 14)
						{
							this.velocity.Y = this.velocity.Y + 0.1f;
						}
					}
					else if (this.ai[0] >= 15f)
					{
						this.ai[0] = 15f;
						this.velocity.Y = this.velocity.Y + 0.1f;
					}
				}
			}
			if (this.type == 248)
			{
				if (this.velocity.X < 0f)
				{
					this.rotation -= (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.05f;
				}
				else
				{
					this.rotation += (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.05f;
				}
			}
			else if (this.type == 270 || this.type == 585 || this.type == 601)
			{
				this.spriteDirection = this.direction;
				if (this.direction < 0)
				{
					this.rotation = (float)Math.Atan2((double)(-(double)this.velocity.Y), (double)(-(double)this.velocity.X));
				}
				else
				{
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X);
				}
			}
			else if (this.type == 311)
			{
				if (this.ai[1] != 0f)
				{
					this.rotation += this.velocity.X * 0.1f + (float)Main.rand.Next(-10, 11) * 0.025f;
				}
				else
				{
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
				}
			}
			else if (this.type == 312)
			{
				this.rotation += this.velocity.X * 0.02f;
			}
			else if (this.type == 408)
			{
				this.rotation = this.velocity.ToRotation();
				if (this.direction == -1)
				{
					this.rotation += 3.14159274f;
				}
			}
			else if (this.type == 435 || this.type == 459)
			{
				this.rotation = this.velocity.ToRotation();
				if (this.direction == -1)
				{
					this.rotation += 3.14159274f;
				}
			}
			else if (this.type == 436)
			{
				this.rotation = this.velocity.ToRotation();
				this.rotation += 3.14159274f;
				if (this.direction == -1)
				{
					this.rotation += 3.14159274f;
				}
			}
			else if (this.type == 469)
			{
				if (this.velocity.X > 0f)
				{
					this.spriteDirection = -1;
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
				}
				else
				{
					this.spriteDirection = 1;
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
				}
			}
			else if (this.type == 477)
			{
				if (this.localAI[1] < 5f)
				{
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
					this.localAI[1] += 1f;
				}
				else
				{
					this.rotation = (this.rotation * 2f + (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f) / 3f;
				}
			}
			else if (this.type == 532)
			{
				this.rotation += 0.2f + Math.Abs(this.velocity.X) * 0.1f;
			}
			else if (this.type == 483)
			{
				this.rotation += this.velocity.X * 0.05f;
			}
			else if (this.type == 485)
			{
				Vector2 vector32 = new Vector2(this.ai[0], this.ai[1]);
				this.velocity = (this.velocity * 39f + vector32) / 40f;
				this.frameCounter++;
				if (this.frameCounter >= 2)
				{
					this.frameCounter = 0;
					this.frame++;
					if (this.frame >= 5)
					{
						this.frame = 0;
					}
				}
				if (this.velocity.X < 0f)
				{
					this.spriteDirection = -1;
					this.rotation = (float)Math.Atan2((double)(-(double)this.velocity.Y), (double)(-(double)this.velocity.X));
				}
				else
				{
					this.spriteDirection = 1;
					this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X);
				}
			}
			else if (this.type == 640)
			{
				if (this.velocity != Vector2.Zero)
				{
					this.rotation = this.velocity.ToRotation() + 1.57079637f;
				}
			}
			else if (this.type != 344 && this.type != 498)
			{
				this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
			}
			if (this.velocity.Y > 16f)
			{
				this.velocity.Y = 16f;
			}
		}
		private void AI_026()
		{
			if (!Main.player[this.owner].active)
			{
				this.active = false;
				return;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num = 85;
			if (this.type == 324)
			{
				num = 120;
			}
			if (this.type == 112)
			{
				num = 100;
			}
			if (this.type == 127)
			{
				num = 50;
			}
			if (this.type >= 191 && this.type <= 194)
			{
				if (this.lavaWet)
				{
					this.ai[0] = 1f;
					this.ai[1] = 0f;
				}
				num = 60 + 30 * this.minionPos;
			}
			else if (this.type == 266)
			{
				num = 60 + 30 * this.minionPos;
			}
			if (this.type == 111)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].bunny = false;
				}
				if (Main.player[this.owner].bunny)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 112)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].penguin = false;
				}
				if (Main.player[this.owner].penguin)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 334)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].puppy = false;
				}
				if (Main.player[this.owner].puppy)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 353)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].grinch = false;
				}
				if (Main.player[this.owner].grinch)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 127)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].turtle = false;
				}
				if (Main.player[this.owner].turtle)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 175)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].eater = false;
				}
				if (Main.player[this.owner].eater)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 197)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].skeletron = false;
				}
				if (Main.player[this.owner].skeletron)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 198)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].hornet = false;
				}
				if (Main.player[this.owner].hornet)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 199)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].tiki = false;
				}
				if (Main.player[this.owner].tiki)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 200)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].lizard = false;
				}
				if (Main.player[this.owner].lizard)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 208)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].parrot = false;
				}
				if (Main.player[this.owner].parrot)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 209)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].truffle = false;
				}
				if (Main.player[this.owner].truffle)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 210)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].sapling = false;
				}
				if (Main.player[this.owner].sapling)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 324)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].cSapling = false;
				}
				if (Main.player[this.owner].cSapling)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 313)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].spider = false;
				}
				if (Main.player[this.owner].spider)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 314)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].squashling = false;
				}
				if (Main.player[this.owner].squashling)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 211)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].wisp = false;
				}
				if (Main.player[this.owner].wisp)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 236)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].dino = false;
				}
				if (Main.player[this.owner].dino)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 499)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].babyFaceMonster = false;
				}
				if (Main.player[this.owner].babyFaceMonster)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 266)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].slime = false;
				}
				if (Main.player[this.owner].slime)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 268)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].eyeSpring = false;
				}
				if (Main.player[this.owner].eyeSpring)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 269)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].snowman = false;
				}
				if (Main.player[this.owner].snowman)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 319)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].blackCat = false;
				}
				if (Main.player[this.owner].blackCat)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 380)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].zephyrfish = false;
				}
				if (Main.player[this.owner].zephyrfish)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type >= 191 && this.type <= 194)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].pygmy = false;
				}
				if (Main.player[this.owner].pygmy)
				{
					this.timeLeft = Main.rand.Next(2, 10);
				}
			}
			if (this.type >= 390 && this.type <= 392)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].spiderMinion = false;
				}
				if (Main.player[this.owner].spiderMinion)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 398)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].miniMinotaur = false;
				}
				if (Main.player[this.owner].miniMinotaur)
				{
					this.timeLeft = 2;
				}
			}
			if ((this.type >= 191 && this.type <= 194) || this.type == 266 || (this.type >= 390 && this.type <= 392))
			{
				num = 10;
				int num2 = 40 * (this.minionPos + 1) * Main.player[this.owner].direction;
				if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) < this.position.X + (float)(this.width / 2) - (float)num + (float)num2)
				{
					flag = true;
				}
				else if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) > this.position.X + (float)(this.width / 2) + (float)num + (float)num2)
				{
					flag2 = true;
				}
			}
			else if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) < this.position.X + (float)(this.width / 2) - (float)num)
			{
				flag = true;
			}
			else if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) > this.position.X + (float)(this.width / 2) + (float)num)
			{
				flag2 = true;
			}
			if (this.type == 175)
			{
				float num3 = 0.1f;
				this.tileCollide = false;
				int num4 = 300;
				Vector2 vector = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
				float num5 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector.X;
				float num6 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector.Y;
				if (this.type == 127)
				{
					num6 = Main.player[this.owner].position.Y - vector.Y;
				}
				float num7 = (float)Math.Sqrt((double)(num5 * num5 + num6 * num6));
				float num8 = 7f;
				if (num7 < (float)num4 && Main.player[this.owner].velocity.Y == 0f && this.position.Y + (float)this.height <= Main.player[this.owner].position.Y + (float)Main.player[this.owner].height && !Collision.SolidCollision(this.position, this.width, this.height))
				{
					this.ai[0] = 0f;
					if (this.velocity.Y < -6f)
					{
						this.velocity.Y = -6f;
					}
				}
				if (num7 < 150f)
				{
					if (Math.Abs(this.velocity.X) > 2f || Math.Abs(this.velocity.Y) > 2f)
					{
						this.velocity *= 0.99f;
					}
					num3 = 0.01f;
					if (num5 < -2f)
					{
						num5 = -2f;
					}
					if (num5 > 2f)
					{
						num5 = 2f;
					}
					if (num6 < -2f)
					{
						num6 = -2f;
					}
					if (num6 > 2f)
					{
						num6 = 2f;
					}
				}
				else
				{
					if (num7 > 300f)
					{
						num3 = 0.2f;
					}
					num7 = num8 / num7;
					num5 *= num7;
					num6 *= num7;
				}
				if (Math.Abs(num5) > Math.Abs(num6) || num3 == 0.05f)
				{
					if (this.velocity.X < num5)
					{
						this.velocity.X = this.velocity.X + num3;
						if (num3 > 0.05f && this.velocity.X < 0f)
						{
							this.velocity.X = this.velocity.X + num3;
						}
					}
					if (this.velocity.X > num5)
					{
						this.velocity.X = this.velocity.X - num3;
						if (num3 > 0.05f && this.velocity.X > 0f)
						{
							this.velocity.X = this.velocity.X - num3;
						}
					}
				}
				if (Math.Abs(num5) <= Math.Abs(num6) || num3 == 0.05f)
				{
					if (this.velocity.Y < num6)
					{
						this.velocity.Y = this.velocity.Y + num3;
						if (num3 > 0.05f && this.velocity.Y < 0f)
						{
							this.velocity.Y = this.velocity.Y + num3;
						}
					}
					if (this.velocity.Y > num6)
					{
						this.velocity.Y = this.velocity.Y - num3;
						if (num3 > 0.05f && this.velocity.Y > 0f)
						{
							this.velocity.Y = this.velocity.Y - num3;
						}
					}
				}
				this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) - 1.57f;
				this.frameCounter++;
				if (this.frameCounter > 6)
				{
					this.frame++;
					this.frameCounter = 0;
				}
				if (this.frame > 1)
				{
					this.frame = 0;
					return;
				}
			}
			else if (this.type == 197)
			{
				float num9 = 0.1f;
				this.tileCollide = false;
				int num10 = 300;
				Vector2 vector2 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
				float num11 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector2.X;
				float num12 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector2.Y;
				if (this.type == 127)
				{
					num12 = Main.player[this.owner].position.Y - vector2.Y;
				}
				float num13 = (float)Math.Sqrt((double)(num11 * num11 + num12 * num12));
				float num14 = 3f;
				if (num13 > 500f)
				{
					this.localAI[0] = 10000f;
				}
				if (this.localAI[0] >= 10000f)
				{
					num14 = 14f;
				}
				if (num13 < (float)num10 && Main.player[this.owner].velocity.Y == 0f && this.position.Y + (float)this.height <= Main.player[this.owner].position.Y + (float)Main.player[this.owner].height && !Collision.SolidCollision(this.position, this.width, this.height))
				{
					this.ai[0] = 0f;
					if (this.velocity.Y < -6f)
					{
						this.velocity.Y = -6f;
					}
				}
				if (num13 < 150f)
				{
					if (Math.Abs(this.velocity.X) > 2f || Math.Abs(this.velocity.Y) > 2f)
					{
						this.velocity *= 0.99f;
					}
					num9 = 0.01f;
					if (num11 < -2f)
					{
						num11 = -2f;
					}
					if (num11 > 2f)
					{
						num11 = 2f;
					}
					if (num12 < -2f)
					{
						num12 = -2f;
					}
					if (num12 > 2f)
					{
						num12 = 2f;
					}
				}
				else
				{
					if (num13 > 300f)
					{
						num9 = 0.2f;
					}
					num13 = num14 / num13;
					num11 *= num13;
					num12 *= num13;
				}
				if (this.velocity.X < num11)
				{
					this.velocity.X = this.velocity.X + num9;
					if (num9 > 0.05f && this.velocity.X < 0f)
					{
						this.velocity.X = this.velocity.X + num9;
					}
				}
				if (this.velocity.X > num11)
				{
					this.velocity.X = this.velocity.X - num9;
					if (num9 > 0.05f && this.velocity.X > 0f)
					{
						this.velocity.X = this.velocity.X - num9;
					}
				}
				if (this.velocity.Y < num12)
				{
					this.velocity.Y = this.velocity.Y + num9;
					if (num9 > 0.05f && this.velocity.Y < 0f)
					{
						this.velocity.Y = this.velocity.Y + num9;
					}
				}
				if (this.velocity.Y > num12)
				{
					this.velocity.Y = this.velocity.Y - num9;
					if (num9 > 0.05f && this.velocity.Y > 0f)
					{
						this.velocity.Y = this.velocity.Y - num9;
					}
				}
				this.localAI[0] += (float)Main.rand.Next(10);
				if (this.localAI[0] > 10000f)
				{
					if (this.localAI[1] == 0f)
					{
						if (this.velocity.X < 0f)
						{
							this.localAI[1] = -1f;
						}
						else
						{
							this.localAI[1] = 1f;
						}
					}
					this.rotation += 0.25f * this.localAI[1];
					if (this.localAI[0] > 12000f)
					{
						this.localAI[0] = 0f;
					}
				}
				else
				{
					this.localAI[1] = 0f;
					float num15 = this.velocity.X * 0.1f;
					if (this.rotation > num15)
					{
						this.rotation -= (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.01f;
						if (this.rotation < num15)
						{
							this.rotation = num15;
						}
					}
					if (this.rotation < num15)
					{
						this.rotation += (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) * 0.01f;
						if (this.rotation > num15)
						{
							this.rotation = num15;
						}
					}
				}
				if ((double)this.rotation > 6.28)
				{
					this.rotation -= 6.28f;
				}
				if ((double)this.rotation < -6.28)
				{
					this.rotation += 6.28f;
					return;
				}
			}
			else if (this.type == 198 || this.type == 380)
			{
				float num16 = 0.4f;
				if (this.type == 380)
				{
					num16 = 0.3f;
				}
				this.tileCollide = false;
				int num17 = 100;
				Vector2 vector3 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
				float num18 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector3.X;
				float num19 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector3.Y;
				num19 += (float)Main.rand.Next(-10, 21);
				num18 += (float)Main.rand.Next(-10, 21);
				num18 += (float)(60 * -(float)Main.player[this.owner].direction);
				num19 -= 60f;
				if (this.type == 127)
				{
					num19 = Main.player[this.owner].position.Y - vector3.Y;
				}
				float num20 = (float)Math.Sqrt((double)(num18 * num18 + num19 * num19));
				float num21 = 14f;
				if (this.type == 380)
				{
					num21 = 6f;
				}
				if (num20 < (float)num17 && Main.player[this.owner].velocity.Y == 0f && this.position.Y + (float)this.height <= Main.player[this.owner].position.Y + (float)Main.player[this.owner].height && !Collision.SolidCollision(this.position, this.width, this.height))
				{
					this.ai[0] = 0f;
					if (this.velocity.Y < -6f)
					{
						this.velocity.Y = -6f;
					}
				}
				if (num20 < 50f)
				{
					if (Math.Abs(this.velocity.X) > 2f || Math.Abs(this.velocity.Y) > 2f)
					{
						this.velocity *= 0.99f;
					}
					num16 = 0.01f;
				}
				else
				{
					if (this.type == 380)
					{
						if (num20 < 100f)
						{
							num16 = 0.1f;
						}
						if (num20 > 300f)
						{
							num16 = 0.4f;
						}
					}
					else if (this.type == 198)
					{
						if (num20 < 100f)
						{
							num16 = 0.1f;
						}
						if (num20 > 300f)
						{
							num16 = 0.6f;
						}
					}
					num20 = num21 / num20;
					num18 *= num20;
					num19 *= num20;
				}
				if (this.velocity.X < num18)
				{
					this.velocity.X = this.velocity.X + num16;
					if (num16 > 0.05f && this.velocity.X < 0f)
					{
						this.velocity.X = this.velocity.X + num16;
					}
				}
				if (this.velocity.X > num18)
				{
					this.velocity.X = this.velocity.X - num16;
					if (num16 > 0.05f && this.velocity.X > 0f)
					{
						this.velocity.X = this.velocity.X - num16;
					}
				}
				if (this.velocity.Y < num19)
				{
					this.velocity.Y = this.velocity.Y + num16;
					if (num16 > 0.05f && this.velocity.Y < 0f)
					{
						this.velocity.Y = this.velocity.Y + num16 * 2f;
					}
				}
				if (this.velocity.Y > num19)
				{
					this.velocity.Y = this.velocity.Y - num16;
					if (num16 > 0.05f && this.velocity.Y > 0f)
					{
						this.velocity.Y = this.velocity.Y - num16 * 2f;
					}
				}
				if ((double)this.velocity.X > 0.25)
				{
					this.direction = -1;
				}
				else if ((double)this.velocity.X < -0.25)
				{
					this.direction = 1;
				}
				this.spriteDirection = this.direction;
				this.rotation = this.velocity.X * 0.05f;
				this.frameCounter++;
				int num22 = 2;
				if (this.type == 380)
				{
					num22 = 5;
				}
				if (this.frameCounter > num22)
				{
					this.frame++;
					this.frameCounter = 0;
				}
				if (this.frame > 3)
				{
					this.frame = 0;
					return;
				}
			}
			else if (this.type == 211)
			{
				float num23 = 0.2f;
				float num24 = 5f;
				this.tileCollide = false;
				Vector2 vector4 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
				float num25 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector4.X;
				float num26 = Main.player[this.owner].position.Y + Main.player[this.owner].gfxOffY + (float)(Main.player[this.owner].height / 2) - vector4.Y;
				if (Main.player[this.owner].controlLeft)
				{
					num25 -= 120f;
				}
				else if (Main.player[this.owner].controlRight)
				{
					num25 += 120f;
				}
				if (Main.player[this.owner].controlDown)
				{
					num26 += 120f;
				}
				else
				{
					if (Main.player[this.owner].controlUp)
					{
						num26 -= 120f;
					}
					num26 -= 60f;
				}
				float num27 = (float)Math.Sqrt((double)(num25 * num25 + num26 * num26));
				if (num27 > 1000f)
				{
					this.position.X = this.position.X + num25;
					this.position.Y = this.position.Y + num26;
				}
				if (this.localAI[0] == 1f)
				{
					if (num27 < 10f && Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y) < num24 && Main.player[this.owner].velocity.Y == 0f)
					{
						this.localAI[0] = 0f;
					}
					num24 = 12f;
					if (num27 < num24)
					{
						this.velocity.X = num25;
						this.velocity.Y = num26;
					}
					else
					{
						num27 = num24 / num27;
						this.velocity.X = num25 * num27;
						this.velocity.Y = num26 * num27;
					}
					if ((double)this.velocity.X > 0.5)
					{
						this.direction = -1;
					}
					else if ((double)this.velocity.X < -0.5)
					{
						this.direction = 1;
					}
					this.spriteDirection = this.direction;
					this.rotation -= (0.2f + Math.Abs(this.velocity.X) * 0.025f) * (float)this.direction;
					this.frameCounter++;
					if (this.frameCounter > 3)
					{
						this.frame++;
						this.frameCounter = 0;
					}
					if (this.frame < 5)
					{
						this.frame = 5;
					}
					if (this.frame > 9)
					{
						this.frame = 5;
					}
					return;
				}
				if (num27 > 200f)
				{
					this.localAI[0] = 1f;
				}
				if ((double)this.velocity.X > 0.5)
				{
					this.direction = -1;
				}
				else if ((double)this.velocity.X < -0.5)
				{
					this.direction = 1;
				}
				this.spriteDirection = this.direction;
				if (num27 < 10f)
				{
					this.velocity.X = num25;
					this.velocity.Y = num26;
					this.rotation = this.velocity.X * 0.05f;
					if (num27 < num24)
					{
						this.position += this.velocity;
						this.velocity *= 0f;
						num23 = 0f;
					}
					this.direction = -Main.player[this.owner].direction;
				}
				num27 = num24 / num27;
				num25 *= num27;
				num26 *= num27;
				if (this.velocity.X < num25)
				{
					this.velocity.X = this.velocity.X + num23;
					if (this.velocity.X < 0f)
					{
						this.velocity.X = this.velocity.X * 0.99f;
					}
				}
				if (this.velocity.X > num25)
				{
					this.velocity.X = this.velocity.X - num23;
					if (this.velocity.X > 0f)
					{
						this.velocity.X = this.velocity.X * 0.99f;
					}
				}
				if (this.velocity.Y < num26)
				{
					this.velocity.Y = this.velocity.Y + num23;
					if (this.velocity.Y < 0f)
					{
						this.velocity.Y = this.velocity.Y * 0.99f;
					}
				}
				if (this.velocity.Y > num26)
				{
					this.velocity.Y = this.velocity.Y - num23;
					if (this.velocity.Y > 0f)
					{
						this.velocity.Y = this.velocity.Y * 0.99f;
					}
				}
				if (this.velocity.X != 0f || this.velocity.Y != 0f)
				{
					this.rotation = this.velocity.X * 0.05f;
				}
				this.frameCounter++;
				if (this.frameCounter > 3)
				{
					this.frame++;
					this.frameCounter = 0;
				}
				if (this.frame > 4)
				{
					this.frame = 0;
					return;
				}
			}
			else if (this.type == 199)
			{
				float num29 = 0.1f;
				this.tileCollide = false;
				int num30 = 200;
				Vector2 vector5 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
				float num31 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector5.X;
				float num32 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector5.Y;
				num32 -= 60f;
				num31 -= 2f;
				if (this.type == 127)
				{
					num32 = Main.player[this.owner].position.Y - vector5.Y;
				}
				float num33 = (float)Math.Sqrt((double)(num31 * num31 + num32 * num32));
				float num34 = 4f;
				float num35 = num33;
				if (num33 < (float)num30 && Main.player[this.owner].velocity.Y == 0f && this.position.Y + (float)this.height <= Main.player[this.owner].position.Y + (float)Main.player[this.owner].height && !Collision.SolidCollision(this.position, this.width, this.height))
				{
					this.ai[0] = 0f;
					if (this.velocity.Y < -6f)
					{
						this.velocity.Y = -6f;
					}
				}
				if (num33 < 4f)
				{
					this.velocity.X = num31;
					this.velocity.Y = num32;
					num29 = 0f;
				}
				else
				{
					if (num33 > 350f)
					{
						num29 = 0.2f;
						num34 = 10f;
					}
					num33 = num34 / num33;
					num31 *= num33;
					num32 *= num33;
				}
				if (this.velocity.X < num31)
				{
					this.velocity.X = this.velocity.X + num29;
					if (this.velocity.X < 0f)
					{
						this.velocity.X = this.velocity.X + num29;
					}
				}
				if (this.velocity.X > num31)
				{
					this.velocity.X = this.velocity.X - num29;
					if (this.velocity.X > 0f)
					{
						this.velocity.X = this.velocity.X - num29;
					}
				}
				if (this.velocity.Y < num32)
				{
					this.velocity.Y = this.velocity.Y + num29;
					if (this.velocity.Y < 0f)
					{
						this.velocity.Y = this.velocity.Y + num29;
					}
				}
				if (this.velocity.Y > num32)
				{
					this.velocity.Y = this.velocity.Y - num29;
					if (this.velocity.Y > 0f)
					{
						this.velocity.Y = this.velocity.Y - num29;
					}
				}
				this.direction = -Main.player[this.owner].direction;
				this.spriteDirection = 1;
				this.rotation = this.velocity.Y * 0.05f * (float)(-(float)this.direction);
				if (num35 >= 50f)
				{
					this.frameCounter++;
					if (this.frameCounter > 6)
					{
						this.frameCounter = 0;
						if (this.velocity.X < 0f)
						{
							if (this.frame < 2)
							{
								this.frame++;
							}
							if (this.frame > 2)
							{
								this.frame--;
								return;
							}
						}
						else
						{
							if (this.frame < 6)
							{
								this.frame++;
							}
							if (this.frame > 6)
							{
								this.frame--;
								return;
							}
						}
					}
				}
				else
				{
					this.frameCounter++;
					if (this.frameCounter > 6)
					{
						this.frame += this.direction;
						this.frameCounter = 0;
					}
					if (this.frame > 7)
					{
						this.frame = 0;
					}
					if (this.frame < 0)
					{
						this.frame = 7;
						return;
					}
				}
			}
			else
			{
				if (this.ai[1] == 0f)
				{
					int num36 = 500;
					if (this.type == 127)
					{
						num36 = 200;
					}
					if (this.type == 208)
					{
						num36 = 300;
					}
					if ((this.type >= 191 && this.type <= 194) || this.type == 266 || (this.type >= 390 && this.type <= 392))
					{
						num36 += 40 * this.minionPos;
						if (this.localAI[0] > 0f)
						{
							num36 += 500;
						}
						if (this.type == 266 && this.localAI[0] > 0f)
						{
							num36 += 100;
						}
						if (this.type >= 390 && this.type <= 392 && this.localAI[0] > 0f)
						{
							num36 += 400;
						}
					}
					if (Main.player[this.owner].rocketDelay2 > 0)
					{
						this.ai[0] = 1f;
					}
					Vector2 vector6 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
					float num37 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector6.X;
					if (this.type >= 191)
					{
						int arg_2689_0 = this.type;
					}
					float num38 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector6.Y;
					float num39 = (float)Math.Sqrt((double)(num37 * num37 + num38 * num38));
					if (num39 > 2000f)
					{
						this.position.X = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - (float)(this.width / 2);
						this.position.Y = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - (float)(this.height / 2);
					}
					else if (num39 > (float)num36 || (Math.Abs(num38) > 300f && (((this.type < 191 || this.type > 194) && this.type != 266 && (this.type < 390 || this.type > 392)) || this.localAI[0] <= 0f)))
					{
						if (this.type != 324)
						{
							if (num38 > 0f && this.velocity.Y < 0f)
							{
								this.velocity.Y = 0f;
							}
							if (num38 < 0f && this.velocity.Y > 0f)
							{
								this.velocity.Y = 0f;
							}
						}
						this.ai[0] = 1f;
					}
				}
				if (this.type == 209 && this.ai[0] != 0f)
				{
					if (Main.player[this.owner].velocity.Y == 0f && this.alpha >= 100)
					{
						this.position.X = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - (float)(this.width / 2);
						this.position.Y = Main.player[this.owner].position.Y + (float)Main.player[this.owner].height - (float)this.height;
						this.ai[0] = 0f;
						return;
					}
					this.velocity.X = 0f;
					this.velocity.Y = 0f;
					this.alpha += 5;
					if (this.alpha > 255)
					{
						this.alpha = 255;
						return;
					}
				}
				else if (this.ai[0] != 0f)
				{
					float num40 = 0.2f;
					int num41 = 200;
					if (this.type == 127)
					{
						num41 = 100;
					}
					if (this.type >= 191 && this.type <= 194)
					{
						num40 = 0.5f;
						num41 = 100;
					}
					this.tileCollide = false;
					Vector2 vector7 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
					float num42 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector7.X;
					if ((this.type >= 191 && this.type <= 194) || this.type == 266 || (this.type >= 390 && this.type <= 392))
					{
						num42 -= (float)(40 * Main.player[this.owner].direction);
						float num43 = 700f;
						if (this.type >= 191 && this.type <= 194)
						{
							num43 += 100f;
						}
						bool flag5 = false;
						int num44 = -1;
						for (int j = 0; j < 200; j++)
						{
							if (Main.npc[j].CanBeChasedBy(this, false))
							{
								float num45 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
								float num46 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
								float num47 = Math.Abs(Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - num45) + Math.Abs(Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - num46);
								if (num47 < num43)
								{
									if (Collision.CanHit(this.position, this.width, this.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height))
									{
										num44 = j;
									}
									flag5 = true;
									break;
								}
							}
						}
						if (!flag5)
						{
							num42 -= (float)(40 * this.minionPos * Main.player[this.owner].direction);
						}
						if (flag5 && num44 >= 0)
						{
							this.ai[0] = 0f;
						}
					}
					float num48 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector7.Y;
					if (this.type == 127)
					{
						num48 = Main.player[this.owner].position.Y - vector7.Y;
					}
					float num49 = (float)Math.Sqrt((double)(num42 * num42 + num48 * num48));
					float num50 = 10f;
					float num51 = num49;
					if (this.type == 111)
					{
						num50 = 11f;
					}
					if (this.type == 127)
					{
						num50 = 9f;
					}
					if (this.type == 324)
					{
						num50 = 20f;
					}
					if (this.type >= 191 && this.type <= 194)
					{
						num40 = 0.4f;
						num50 = 12f;
						if (num50 < Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y))
						{
							num50 = Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y);
						}
					}
					if (this.type == 208 && Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y) > 4f)
					{
						num41 = -1;
					}
					if (num49 < (float)num41 && Main.player[this.owner].velocity.Y == 0f && this.position.Y + (float)this.height <= Main.player[this.owner].position.Y + (float)Main.player[this.owner].height && !Collision.SolidCollision(this.position, this.width, this.height))
					{
						this.ai[0] = 0f;
						if (this.velocity.Y < -6f)
						{
							this.velocity.Y = -6f;
						}
					}
					if (num49 < 60f)
					{
						num42 = this.velocity.X;
						num48 = this.velocity.Y;
					}
					else
					{
						num49 = num50 / num49;
						num42 *= num49;
						num48 *= num49;
					}
					if (this.type == 324)
					{
						if (num51 > 1000f)
						{
							if ((double)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) < (double)num50 - 1.25)
							{
								this.velocity *= 1.025f;
							}
							if ((double)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) > (double)num50 + 1.25)
							{
								this.velocity *= 0.975f;
							}
						}
						else if (num51 > 600f)
						{
							if (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y) < num50 - 1f)
							{
								this.velocity *= 1.05f;
							}
							if (Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y) > num50 + 1f)
							{
								this.velocity *= 0.95f;
							}
						}
						else if (num51 > 400f)
						{
							if ((double)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) < (double)num50 - 0.5)
							{
								this.velocity *= 1.075f;
							}
							if ((double)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) > (double)num50 + 0.5)
							{
								this.velocity *= 0.925f;
							}
						}
						else
						{
							if ((double)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) < (double)num50 - 0.25)
							{
								this.velocity *= 1.1f;
							}
							if ((double)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) > (double)num50 + 0.25)
							{
								this.velocity *= 0.9f;
							}
						}
						this.velocity.X = (this.velocity.X * 34f + num42) / 35f;
						this.velocity.Y = (this.velocity.Y * 34f + num48) / 35f;
					}
					else
					{
						if (this.velocity.X < num42)
						{
							this.velocity.X = this.velocity.X + num40;
							if (this.velocity.X < 0f)
							{
								this.velocity.X = this.velocity.X + num40 * 1.5f;
							}
						}
						if (this.velocity.X > num42)
						{
							this.velocity.X = this.velocity.X - num40;
							if (this.velocity.X > 0f)
							{
								this.velocity.X = this.velocity.X - num40 * 1.5f;
							}
						}
						if (this.velocity.Y < num48)
						{
							this.velocity.Y = this.velocity.Y + num40;
							if (this.velocity.Y < 0f)
							{
								this.velocity.Y = this.velocity.Y + num40 * 1.5f;
							}
						}
						if (this.velocity.Y > num48)
						{
							this.velocity.Y = this.velocity.Y - num40;
							if (this.velocity.Y > 0f)
							{
								this.velocity.Y = this.velocity.Y - num40 * 1.5f;
							}
						}
					}
					if (this.type == 111)
					{
						this.frame = 7;
					}
					if (this.type == 112)
					{
						this.frame = 2;
					}
					if (this.type >= 191 && this.type <= 194 && this.frame < 12)
					{
						this.frame = Main.rand.Next(12, 18);
						this.frameCounter = 0;
					}
					if (this.type != 313)
					{
						if ((double)this.velocity.X > 0.5)
						{
							this.spriteDirection = -1;
						}
						else if ((double)this.velocity.X < -0.5)
						{
							this.spriteDirection = 1;
						}
					}
					if (this.type == 398)
					{
						if ((double)this.velocity.X > 0.5)
						{
							this.spriteDirection = 1;
						}
						else if ((double)this.velocity.X < -0.5)
						{
							this.spriteDirection = -1;
						}
					}
					if (this.type == 112)
					{
						if (this.spriteDirection == -1)
						{
							this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
						}
						else
						{
							this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.57f;
						}
					}
					else if (this.type >= 390 && this.type <= 392)
					{
						int num52 = (int)(base.Center.X / 16f);
						int num53 = (int)(base.Center.Y / 16f);
						if (Main.tile[num52, num53] != null && Main.tile[num52, num53].wall > 0)
						{
							this.rotation = this.velocity.ToRotation() + 1.57079637f;
							this.frameCounter += (int)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y));
							if (this.frameCounter > 5)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 7)
							{
								this.frame = 4;
							}
							if (this.frame < 4)
							{
								this.frame = 7;
							}
						}
						else
						{
							this.frameCounter++;
							if (this.frameCounter > 2)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame < 8 || this.frame > 10)
							{
								this.frame = 8;
							}
							this.rotation = this.velocity.X * 0.1f;
						}
					}
					else if (this.type == 334)
					{
						this.frameCounter++;
						if (this.frameCounter > 1)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 7 || this.frame > 10)
						{
							this.frame = 7;
						}
						this.rotation = this.velocity.X * 0.1f;
					}
					else if (this.type == 353)
					{
						this.frameCounter++;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 10 || this.frame > 13)
						{
							this.frame = 10;
						}
						this.rotation = this.velocity.X * 0.05f;
					}
					else if (this.type == 127)
					{
						this.frameCounter += 3;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame <= 5 || this.frame > 15)
						{
							this.frame = 6;
						}
						this.rotation = this.velocity.X * 0.1f;
					}
					else if (this.type == 269)
					{
						if (this.frame == 6)
						{
							this.frameCounter = 0;
						}
						else if (this.frame < 4 || this.frame > 6)
						{
							this.frameCounter = 0;
							this.frame = 4;
						}
						else
						{
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
						}
						this.rotation = this.velocity.X * 0.05f;
					}
					else if (this.type == 266)
					{
						this.frameCounter++;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 2 || this.frame > 5)
						{
							this.frame = 2;
						}
						this.rotation = this.velocity.X * 0.1f;
					}
					else if (this.type == 324)
					{
						this.frameCounter++;
						if (this.frameCounter > 1)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 6 || this.frame > 9)
						{
							this.frame = 6;
						}
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.58f;
					}
					else if (this.type == 268)
					{
						this.frameCounter++;
						if (this.frameCounter > 4)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 6 || this.frame > 7)
						{
							this.frame = 6;
						}
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.58f;
					}
					else if (this.type == 200)
					{
						this.frameCounter += 3;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame <= 5 || this.frame > 9)
						{
							this.frame = 6;
						}
						this.rotation = this.velocity.X * 0.1f;
					}
					else if (this.type == 208)
					{
						this.rotation = this.velocity.X * 0.075f;
						this.frameCounter++;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 4)
						{
							this.frame = 1;
						}
						if (this.frame < 1)
						{
							this.frame = 1;
						}
					}
					else if (this.type == 236)
					{
						this.rotation = this.velocity.Y * 0.05f * (float)this.direction;
						if (this.velocity.Y < 0f)
						{
							this.frameCounter += 2;
						}
						else
						{
							this.frameCounter++;
						}
						if (this.frameCounter >= 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 12)
						{
							this.frame = 9;
						}
						if (this.frame < 9)
						{
							this.frame = 9;
						}
					}
					else if (this.type == 499)
					{
						this.rotation = this.velocity.Y * 0.05f * (float)this.direction;
						if (this.velocity.Y < 0f)
						{
							this.frameCounter += 2;
						}
						else
						{
							this.frameCounter++;
						}
						if (this.frameCounter >= 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame >= 12)
						{
							this.frame = 8;
						}
						if (this.frame < 8)
						{
							this.frame = 8;
						}
					}
					else if (this.type == 314)
					{
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.58f;
						this.frameCounter++;
						if (this.frameCounter >= 3)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 12)
						{
							this.frame = 7;
						}
						if (this.frame < 7)
						{
							this.frame = 7;
						}
					}
					else if (this.type == 319)
					{
						this.rotation = this.velocity.X * 0.05f;
						this.frameCounter++;
						if (this.frameCounter >= 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 10)
						{
							this.frame = 6;
						}
						if (this.frame < 6)
						{
							this.frame = 6;
						}
					}
					else if (this.type == 210)
					{
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 1.58f;
						this.frameCounter += 3;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 11)
						{
							this.frame = 7;
						}
						if (this.frame < 7)
						{
							this.frame = 7;
						}
					}
					else if (this.type == 313)
					{
						this.position.Y = this.position.Y + (float)this.height;
						this.height = 54;
						this.position.Y = this.position.Y - (float)this.height;
						this.position.X = this.position.X + (float)(this.width / 2);
						this.width = 54;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.rotation += this.velocity.X * 0.01f;
						this.frameCounter = 0;
						this.frame = 11;
					}
					else if (this.type == 398)
					{
						this.frameCounter++;
						if (this.frameCounter > 1)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 6 || this.frame > 9)
						{
							this.frame = 6;
						}
						this.rotation = this.velocity.X * 0.1f;
					}
					else if (this.spriteDirection == -1)
					{
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X);
					}
					else
					{
						this.rotation = (float)Math.Atan2((double)this.velocity.Y, (double)this.velocity.X) + 3.14f;
					}
					if (this.type >= 191 && this.type <= 194)
					{
						return;
					}
					if (this.type == 499)
					{
						return;
					}
					if (this.type != 398 && this.type != 390 && this.type != 391 && this.type != 392 && this.type != 127 && this.type != 200 && this.type != 208 && this.type != 210 && this.type != 236 && this.type != 266 && this.type != 268 && this.type != 269 && this.type != 313 && this.type != 314 && this.type != 319 && this.type != 324 && this.type != 334 && this.type != 353)
					{
						return;
					}
				}
				else
				{
					if (this.type >= 191 && this.type <= 194)
					{
						float num57 = (float)(40 * this.minionPos);
						int num58 = 30;
						int num59 = 60;
						this.localAI[0] -= 1f;
						if (this.localAI[0] < 0f)
						{
							this.localAI[0] = 0f;
						}
						if (this.ai[1] > 0f)
						{
							this.ai[1] -= 1f;
						}
						else
						{
							float num60 = this.position.X;
							float num61 = this.position.Y;
							float num62 = 100000f;
							float num63 = num62;
							int num64 = -1;
							for (int l = 0; l < 200; l++)
							{
								if (Main.npc[l].CanBeChasedBy(this, false))
								{
									float num65 = Main.npc[l].position.X + (float)(Main.npc[l].width / 2);
									float num66 = Main.npc[l].position.Y + (float)(Main.npc[l].height / 2);
									float num67 = Math.Abs(this.position.X + (float)(this.width / 2) - num65) + Math.Abs(this.position.Y + (float)(this.height / 2) - num66);
									if (num67 < num62)
									{
										if (num64 == -1 && num67 <= num63)
										{
											num63 = num67;
											num60 = num65;
											num61 = num66;
										}
										if (Collision.CanHit(this.position, this.width, this.height, Main.npc[l].position, Main.npc[l].width, Main.npc[l].height))
										{
											num62 = num67;
											num60 = num65;
											num61 = num66;
											num64 = l;
										}
									}
								}
							}
							if (num64 == -1 && num63 < num62)
							{
								num62 = num63;
							}
							float num68 = 400f;
							if ((double)this.position.Y > Main.worldSurface * 16.0)
							{
								num68 = 200f;
							}
							if (num62 < num68 + num57 && num64 == -1)
							{
								float num69 = num60 - (this.position.X + (float)(this.width / 2));
								if (num69 < -5f)
								{
									flag = true;
									flag2 = false;
								}
								else if (num69 > 5f)
								{
									flag2 = true;
									flag = false;
								}
							}
							else if (num64 >= 0 && num62 < 800f + num57)
							{
								this.localAI[0] = (float)num59;
								float num70 = num60 - (this.position.X + (float)(this.width / 2));
								if (num70 > 300f || num70 < -300f)
								{
									if (num70 < -50f)
									{
										flag = true;
										flag2 = false;
									}
									else if (num70 > 50f)
									{
										flag2 = true;
										flag = false;
									}
								}
								else if (this.owner == Main.myPlayer)
								{
									this.ai[1] = (float)num58;
									float num71 = 12f;
									Vector2 vector8 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)(this.height / 2) - 8f);
									float num72 = num60 - vector8.X + (float)Main.rand.Next(-20, 21);
									float num73 = Math.Abs(num72) * 0.1f;
									num73 = num73 * (float)Main.rand.Next(0, 100) * 0.001f;
									float num74 = num61 - vector8.Y + (float)Main.rand.Next(-20, 21) - num73;
									float num75 = (float)Math.Sqrt((double)(num72 * num72 + num74 * num74));
									num75 = num71 / num75;
									num72 *= num75;
									num74 *= num75;
									int num76 = this.damage;
									int num77 = 195;
									int num78 = Projectile.NewProjectile(vector8.X, vector8.Y, num72, num74, num77, num76, this.knockBack, Main.myPlayer, 0f, 0f);
									Main.projectile[num78].timeLeft = 300;
									if (num72 < 0f)
									{
										this.direction = -1;
									}
									if (num72 > 0f)
									{
										this.direction = 1;
									}
									this.netUpdate = true;
								}
							}
						}
					}
					bool flag6 = false;
					Vector2 vector9 = Vector2.Zero;
					bool flag7 = false;
					if (this.type == 266 || (this.type >= 390 && this.type <= 392))
					{
						float num79 = (float)(40 * this.minionPos);
						int num80 = 60;
						this.localAI[0] -= 1f;
						if (this.localAI[0] < 0f)
						{
							this.localAI[0] = 0f;
						}
						if (this.ai[1] > 0f)
						{
							this.ai[1] -= 1f;
						}
						else
						{
							float num81 = this.position.X;
							float num82 = this.position.Y;
							float num83 = 100000f;
							float num84 = num83;
							int num85 = -1;
							for (int m = 0; m < 200; m++)
							{
								if (Main.npc[m].CanBeChasedBy(this, false))
								{
									float num86 = Main.npc[m].position.X + (float)(Main.npc[m].width / 2);
									float num87 = Main.npc[m].position.Y + (float)(Main.npc[m].height / 2);
									float num88 = Math.Abs(this.position.X + (float)(this.width / 2) - num86) + Math.Abs(this.position.Y + (float)(this.height / 2) - num87);
									if (num88 < num83)
									{
										if (num85 == -1 && num88 <= num84)
										{
											num84 = num88;
											num81 = num86;
											num82 = num87;
										}
										if (Collision.CanHit(this.position, this.width, this.height, Main.npc[m].position, Main.npc[m].width, Main.npc[m].height))
										{
											num83 = num88;
											num81 = num86;
											num82 = num87;
											num85 = m;
										}
									}
								}
							}
							if (this.type >= 390 && this.type <= 392 && !Collision.SolidCollision(this.position, this.width, this.height))
							{
								this.tileCollide = true;
							}
							if (num85 == -1 && num84 < num83)
							{
								num83 = num84;
							}
							else if (num85 >= 0)
							{
								flag6 = true;
								vector9 = new Vector2(num81, num82) - base.Center;
								if (this.type >= 390 && this.type <= 392)
								{
									if (Main.npc[num85].position.Y > this.position.Y + (float)this.height)
									{
										int num89 = (int)(base.Center.X / 16f);
										int num90 = (int)((this.position.Y + (float)this.height + 1f) / 16f);
										if (Main.tile[num89, num90] != null && Main.tile[num89, num90].active() && Main.tile[num89, num90].type == 19)
										{
											this.tileCollide = false;
										}
									}
									Rectangle rectangle = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
									Rectangle rectangle2 = new Rectangle((int)Main.npc[num85].position.X, (int)Main.npc[num85].position.Y, Main.npc[num85].width, Main.npc[num85].height);
									int num91 = 10;
									rectangle2.X -= num91;
									rectangle2.Y -= num91;
									rectangle2.Width += num91 * 2;
									rectangle2.Height += num91 * 2;
									if (rectangle.Intersects(rectangle2))
									{
										flag7 = true;
										Vector2 vector10 = Main.npc[num85].Center - base.Center;
										if (this.velocity.Y > 0f && vector10.Y < 0f)
										{
											this.velocity.Y = this.velocity.Y * 0.5f;
										}
										if (this.velocity.Y < 0f && vector10.Y > 0f)
										{
											this.velocity.Y = this.velocity.Y * 0.5f;
										}
										if (this.velocity.X > 0f && vector10.X < 0f)
										{
											this.velocity.X = this.velocity.X * 0.5f;
										}
										if (this.velocity.X < 0f && vector10.X > 0f)
										{
											this.velocity.X = this.velocity.X * 0.5f;
										}
										if (vector10.Length() > 14f)
										{
											vector10.Normalize();
											vector10 *= 14f;
										}
										this.rotation = (this.rotation * 5f + vector10.ToRotation() + 1.57079637f) / 6f;
										this.velocity = (this.velocity * 9f + vector10) / 10f;
										for (int n = 0; n < 1000; n++)
										{
											if (this.whoAmI != n && this.owner == Main.projectile[n].owner && Main.projectile[n].type >= 390 && Main.projectile[n].type <= 392 && (Main.projectile[n].Center - base.Center).Length() < 15f)
											{
												float num92 = 0.5f;
												if (base.Center.Y > Main.projectile[n].Center.Y)
												{
													Projectile expr_4CA4_cp_0 = Main.projectile[n];
													expr_4CA4_cp_0.velocity.Y = expr_4CA4_cp_0.velocity.Y - num92;
													this.velocity.Y = this.velocity.Y + num92;
												}
												else
												{
													Projectile expr_4CD5_cp_0 = Main.projectile[n];
													expr_4CD5_cp_0.velocity.Y = expr_4CD5_cp_0.velocity.Y + num92;
													this.velocity.Y = this.velocity.Y - num92;
												}
												if (base.Center.X > Main.projectile[n].Center.X)
												{
													this.velocity.X = this.velocity.X + num92;
													Projectile expr_4D37_cp_0 = Main.projectile[n];
													expr_4D37_cp_0.velocity.X = expr_4D37_cp_0.velocity.X - num92;
												}
												else
												{
													this.velocity.X = this.velocity.X - num92;
													Projectile expr_4D68_cp_0 = Main.projectile[n];
													expr_4D68_cp_0.velocity.Y = expr_4D68_cp_0.velocity.Y + num92;
												}
											}
										}
									}
								}
							}
							float num93 = 300f;
							if ((double)this.position.Y > Main.worldSurface * 16.0)
							{
								num93 = 150f;
							}
							if (this.type >= 390 && this.type <= 392)
							{
								num93 = 500f;
								if ((double)this.position.Y > Main.worldSurface * 16.0)
								{
									num93 = 250f;
								}
							}
							if (num83 < num93 + num79 && num85 == -1)
							{
								float num94 = num81 - (this.position.X + (float)(this.width / 2));
								if (num94 < -5f)
								{
									flag = true;
									flag2 = false;
								}
								else if (num94 > 5f)
								{
									flag2 = true;
									flag = false;
								}
							}
							bool flag8 = false;
							if (this.type >= 390 && this.type <= 392 && this.localAI[1] > 0f)
							{
								flag8 = true;
								this.localAI[1] -= 1f;
							}
							if (num85 >= 0 && num83 < 800f + num79)
							{
								this.friendly = true;
								this.localAI[0] = (float)num80;
								float num95 = num81 - (this.position.X + (float)(this.width / 2));
								if (num95 < -10f)
								{
									flag = true;
									flag2 = false;
								}
								else if (num95 > 10f)
								{
									flag2 = true;
									flag = false;
								}
								if (num82 < base.Center.Y - 100f && num95 > -50f && num95 < 50f && this.velocity.Y == 0f)
								{
									float num96 = Math.Abs(num82 - base.Center.Y);
									if (num96 < 120f)
									{
										this.velocity.Y = -10f;
									}
									else if (num96 < 210f)
									{
										this.velocity.Y = -13f;
									}
									else if (num96 < 270f)
									{
										this.velocity.Y = -15f;
									}
									else if (num96 < 310f)
									{
										this.velocity.Y = -17f;
									}
									else if (num96 < 380f)
									{
										this.velocity.Y = -18f;
									}
								}
								if (flag8)
								{
									this.friendly = false;
									if (this.velocity.X < 0f)
									{
										flag = true;
									}
									else if (this.velocity.X > 0f)
									{
										flag2 = true;
									}
								}
							}
							else
							{
								this.friendly = false;
							}
						}
					}
					if (this.ai[1] != 0f)
					{
						flag = false;
						flag2 = false;
					}
					else if (this.type >= 191 && this.type <= 194 && this.localAI[0] == 0f)
					{
						this.direction = Main.player[this.owner].direction;
					}
					else if (this.type >= 390 && this.type <= 392)
					{
						int num97 = (int)(base.Center.X / 16f);
						int num98 = (int)(base.Center.Y / 16f);
						if (Main.tile[num97, num98] != null && Main.tile[num97, num98].wall > 0)
						{
							flag2 = (flag = false);
						}
					}
					if (this.type == 127)
					{
						if ((double)this.rotation > -0.1 && (double)this.rotation < 0.1)
						{
							this.rotation = 0f;
						}
						else if (this.rotation < 0f)
						{
							this.rotation += 0.1f;
						}
						else
						{
							this.rotation -= 0.1f;
						}
					}
					else if (this.type != 313 && !flag7)
					{
						this.rotation = 0f;
					}
					if (this.type < 390 || this.type > 392)
					{
						this.tileCollide = true;
					}
					float num99 = 0.08f;
					float num100 = 6.5f;
					if (this.type == 127)
					{
						num100 = 2f;
						num99 = 0.04f;
					}
					if (this.type == 112)
					{
						num100 = 6f;
						num99 = 0.06f;
					}
					if (this.type == 334)
					{
						num100 = 8f;
						num99 = 0.08f;
					}
					if (this.type == 268)
					{
						num100 = 8f;
						num99 = 0.4f;
					}
					if (this.type == 324)
					{
						num99 = 0.1f;
						num100 = 3f;
					}
					if ((this.type >= 191 && this.type <= 194) || this.type == 266 || (this.type >= 390 && this.type <= 392))
					{
						num100 = 6f;
						num99 = 0.2f;
						if (num100 < Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y))
						{
							num100 = Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y);
							num99 = 0.3f;
						}
					}
					if (this.type >= 390 && this.type <= 392)
					{
						num99 *= 2f;
					}
					if (flag)
					{
						if ((double)this.velocity.X > -3.5)
						{
							this.velocity.X = this.velocity.X - num99;
						}
						else
						{
							this.velocity.X = this.velocity.X - num99 * 0.25f;
						}
					}
					else if (flag2)
					{
						if ((double)this.velocity.X < 3.5)
						{
							this.velocity.X = this.velocity.X + num99;
						}
						else
						{
							this.velocity.X = this.velocity.X + num99 * 0.25f;
						}
					}
					else
					{
						this.velocity.X = this.velocity.X * 0.9f;
						if (this.velocity.X >= -num99 && this.velocity.X <= num99)
						{
							this.velocity.X = 0f;
						}
					}
					if (this.type == 208)
					{
						this.velocity.X = this.velocity.X * 0.95f;
						if ((double)this.velocity.X > -0.1 && (double)this.velocity.X < 0.1)
						{
							this.velocity.X = 0f;
						}
						flag = false;
						flag2 = false;
					}
					if (flag || flag2)
					{
						int num101 = (int)(this.position.X + (float)(this.width / 2)) / 16;
						int j2 = (int)(this.position.Y + (float)(this.height / 2)) / 16;
						if (this.type == 236)
						{
							num101 += this.direction;
						}
						if (flag)
						{
							num101--;
						}
						if (flag2)
						{
							num101++;
						}
						num101 += (int)this.velocity.X;
						if (WorldGen.SolidTile(num101, j2))
						{
							flag4 = true;
						}
					}
					if (Main.player[this.owner].position.Y + (float)Main.player[this.owner].height - 8f > this.position.Y + (float)this.height)
					{
						flag3 = true;
					}
					if (this.type == 268 && this.frameCounter < 10)
					{
						flag4 = false;
					}
					Collision.StepUp(ref this.position, ref this.velocity, this.width, this.height, ref this.stepSpeed, ref this.gfxOffY, 1, false, 0);
					if (this.velocity.Y == 0f || this.type == 200)
					{
						if (!flag3 && (this.velocity.X < 0f || this.velocity.X > 0f))
						{
							int num102 = (int)(this.position.X + (float)(this.width / 2)) / 16;
							int j3 = (int)(this.position.Y + (float)(this.height / 2)) / 16 + 1;
							if (flag)
							{
								num102--;
							}
							if (flag2)
							{
								num102++;
							}
							WorldGen.SolidTile(num102, j3);
						}
						if (flag4)
						{
							int num103 = (int)(this.position.X + (float)(this.width / 2)) / 16;
							int num104 = (int)(this.position.Y + (float)this.height) / 16 + 1;
							if (WorldGen.SolidTile(num103, num104) || Main.tile[num103, num104].halfBrick() || Main.tile[num103, num104].slope() > 0 || this.type == 200)
							{
								if (this.type == 200)
								{
									this.velocity.Y = -3.1f;
								}
								else
								{
									try
									{
										num103 = (int)(this.position.X + (float)(this.width / 2)) / 16;
										num104 = (int)(this.position.Y + (float)(this.height / 2)) / 16;
										if (flag)
										{
											num103--;
										}
										if (flag2)
										{
											num103++;
										}
										num103 += (int)this.velocity.X;
										if (!WorldGen.SolidTile(num103, num104 - 1) && !WorldGen.SolidTile(num103, num104 - 2))
										{
											this.velocity.Y = -5.1f;
										}
										else if (!WorldGen.SolidTile(num103, num104 - 2))
										{
											this.velocity.Y = -7.1f;
										}
										else if (WorldGen.SolidTile(num103, num104 - 5))
										{
											this.velocity.Y = -11.1f;
										}
										else if (WorldGen.SolidTile(num103, num104 - 4))
										{
											this.velocity.Y = -10.1f;
										}
										else
										{
											this.velocity.Y = -9.1f;
										}
									}
									catch
									{
										this.velocity.Y = -9.1f;
									}
								}
								if (this.type == 127)
								{
									this.ai[0] = 1f;
								}
							}
						}
						else if (this.type == 266 && (flag || flag2))
						{
							this.velocity.Y = this.velocity.Y - 6f;
						}
					}
					if (this.velocity.X > num100)
					{
						this.velocity.X = num100;
					}
					if (this.velocity.X < -num100)
					{
						this.velocity.X = -num100;
					}
					if (this.velocity.X < 0f)
					{
						this.direction = -1;
					}
					if (this.velocity.X > 0f)
					{
						this.direction = 1;
					}
					if (this.velocity.X > num99 && flag2)
					{
						this.direction = 1;
					}
					if (this.velocity.X < -num99 && flag)
					{
						this.direction = -1;
					}
					if (this.type != 313)
					{
						if (this.direction == -1)
						{
							this.spriteDirection = 1;
						}
						if (this.direction == 1)
						{
							this.spriteDirection = -1;
						}
					}
					if (this.type == 398)
					{
						this.spriteDirection = this.direction;
					}
					if (this.type >= 191 && this.type <= 194)
					{
						if (this.ai[1] > 0f)
						{
							if (this.localAI[1] == 0f)
							{
								this.localAI[1] = 1f;
								this.frame = 1;
							}
							if (this.frame != 0)
							{
								this.frameCounter++;
								if (this.frameCounter > 4)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame == 4)
								{
									this.frame = 0;
								}
							}
						}
						else if (this.velocity.Y == 0f)
						{
							this.localAI[1] = 0f;
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame < 5)
								{
									this.frame = 5;
								}
								if (this.frame >= 11)
								{
									this.frame = 5;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else if (this.velocity.Y < 0f)
						{
							this.frameCounter = 0;
							this.frame = 4;
						}
						else if (this.velocity.Y > 0f)
						{
							this.frameCounter = 0;
							this.frame = 4;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
						}
						float arg_5ADC_0 = this.velocity.Y;
						return;
					}
					if (this.type == 268)
					{
						if (this.velocity.Y == 0f)
						{
							if (this.frame > 5)
							{
								this.frameCounter = 0;
							}
							if (this.velocity.X == 0f)
							{
								int num105 = 3;
								this.frameCounter++;
								if (this.frameCounter < num105)
								{
									this.frame = 0;
								}
								else if (this.frameCounter < num105 * 2)
								{
									this.frame = 1;
								}
								else if (this.frameCounter < num105 * 3)
								{
									this.frame = 2;
								}
								else if (this.frameCounter < num105 * 4)
								{
									this.frame = 3;
								}
								else
								{
									this.frameCounter = num105 * 4;
								}
							}
							else
							{
								this.velocity.X = this.velocity.X * 0.8f;
								this.frameCounter++;
								int num106 = 3;
								if (this.frameCounter < num106)
								{
									this.frame = 0;
								}
								else if (this.frameCounter < num106 * 2)
								{
									this.frame = 1;
								}
								else if (this.frameCounter < num106 * 3)
								{
									this.frame = 2;
								}
								else if (this.frameCounter < num106 * 4)
								{
									this.frame = 3;
								}
								else if (flag || flag2)
								{
									this.velocity.X = this.velocity.X * 2f;
									this.frame = 4;
									this.velocity.Y = -6.1f;
									this.frameCounter = 0;
								}
								else
								{
									this.frameCounter = num106 * 4;
								}
							}
						}
						else if (this.velocity.Y < 0f)
						{
							this.frameCounter = 0;
							this.frame = 5;
						}
						else
						{
							this.frame = 4;
							this.frameCounter = 3;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 269)
					{
						if (this.velocity.Y >= 0f && (double)this.velocity.Y <= 0.8)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 3)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frameCounter = 0;
							this.frame = 2;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 313)
					{
						int num110 = (int)(base.Center.X / 16f);
						int num111 = (int)(base.Center.Y / 16f);
						if (Main.tile[num110, num111] != null && Main.tile[num110, num111].wall > 0)
						{
							this.position.Y = this.position.Y + (float)this.height;
							this.height = 34;
							this.position.Y = this.position.Y - (float)this.height;
							this.position.X = this.position.X + (float)(this.width / 2);
							this.width = 34;
							this.position.X = this.position.X - (float)(this.width / 2);
							float num112 = 4f;
							Vector2 vector11 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
							float num113 = Main.player[this.owner].Center.X - vector11.X;
							float num114 = Main.player[this.owner].Center.Y - vector11.Y;
							float num115 = (float)Math.Sqrt((double)(num113 * num113 + num114 * num114));
							float num116 = num112 / num115;
							num113 *= num116;
							num114 *= num116;
							if (num115 < 120f)
							{
								this.velocity.X = this.velocity.X * 0.9f;
								this.velocity.Y = this.velocity.Y * 0.9f;
								if ((double)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) < 0.1)
								{
									this.velocity *= 0f;
								}
							}
							else
							{
								this.velocity.X = (this.velocity.X * 9f + num113) / 10f;
								this.velocity.Y = (this.velocity.Y * 9f + num114) / 10f;
							}
							if (num115 >= 120f)
							{
								this.spriteDirection = this.direction;
								this.rotation = (float)Math.Atan2((double)(this.velocity.Y * (float)(-(float)this.direction)), (double)(this.velocity.X * (float)(-(float)this.direction)));
							}
							this.frameCounter += (int)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y));
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 10)
							{
								this.frame = 5;
							}
							if (this.frame < 5)
							{
								this.frame = 10;
								return;
							}
						}
						else
						{
							this.rotation = 0f;
							if (this.direction == -1)
							{
								this.spriteDirection = 1;
							}
							if (this.direction == 1)
							{
								this.spriteDirection = -1;
							}
							this.position.Y = this.position.Y + (float)this.height;
							this.height = 30;
							this.position.Y = this.position.Y - (float)this.height;
							this.position.X = this.position.X + (float)(this.width / 2);
							this.width = 30;
							this.position.X = this.position.X - (float)(this.width / 2);
							if (this.velocity.Y >= 0f && (double)this.velocity.Y <= 0.8)
							{
								if (this.velocity.X == 0f)
								{
									this.frame = 0;
									this.frameCounter = 0;
								}
								else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
								{
									this.frameCounter += (int)Math.Abs(this.velocity.X);
									this.frameCounter++;
									if (this.frameCounter > 6)
									{
										this.frame++;
										this.frameCounter = 0;
									}
									if (this.frame > 3)
									{
										this.frame = 0;
									}
								}
								else
								{
									this.frame = 0;
									this.frameCounter = 0;
								}
							}
							else
							{
								this.frameCounter = 0;
								this.frame = 4;
							}
							this.velocity.Y = this.velocity.Y + 0.4f;
							if (this.velocity.Y > 10f)
							{
								this.velocity.Y = 10f;
								return;
							}
						}
					}
					else if (this.type >= 390 && this.type <= 392)
					{
						int num117 = (int)(base.Center.X / 16f);
						int num118 = (int)(base.Center.Y / 16f);
						if (Main.tile[num117, num118] != null && Main.tile[num117, num118].wall > 0)
						{
							this.position.Y = this.position.Y + (float)this.height;
							this.height = 34;
							this.position.Y = this.position.Y - (float)this.height;
							this.position.X = this.position.X + (float)(this.width / 2);
							this.width = 34;
							this.position.X = this.position.X - (float)(this.width / 2);
							float num119 = 9f;
							float num120 = (float)(40 * (this.minionPos + 1));
							Vector2 vector12 = Main.player[this.owner].Center - base.Center;
							if (flag6)
							{
								vector12 = vector9;
								num120 = 10f;
							}
							else if (!Collision.CanHitLine(base.Center, 1, 1, Main.player[this.owner].Center, 1, 1))
							{
								this.ai[0] = 1f;
							}
							if (vector12.Length() < num120)
							{
								this.velocity *= 0.9f;
								if ((double)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y)) < 0.1)
								{
									this.velocity *= 0f;
								}
							}
							else if (vector12.Length() < 800f || !flag6)
							{
								this.velocity = (this.velocity * 9f + Vector2.Normalize(vector12) * num119) / 10f;
							}
							if (vector12.Length() >= num120)
							{
								this.spriteDirection = this.direction;
								this.rotation = this.velocity.ToRotation() + 1.57079637f;
							}
							else
							{
								this.rotation = vector12.ToRotation() + 1.57079637f;
							}
							this.frameCounter += (int)(Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y));
							if (this.frameCounter > 5)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 7)
							{
								this.frame = 4;
							}
							if (this.frame < 4)
							{
								this.frame = 7;
								return;
							}
						}
						else
						{
							if (!flag7)
							{
								this.rotation = 0f;
							}
							if (this.direction == -1)
							{
								this.spriteDirection = 1;
							}
							if (this.direction == 1)
							{
								this.spriteDirection = -1;
							}
							this.position.Y = this.position.Y + (float)this.height;
							this.height = 30;
							this.position.Y = this.position.Y - (float)this.height;
							this.position.X = this.position.X + (float)(this.width / 2);
							this.width = 30;
							this.position.X = this.position.X - (float)(this.width / 2);
							if (!flag6 && !Collision.CanHitLine(base.Center, 1, 1, Main.player[this.owner].Center, 1, 1))
							{
								this.ai[0] = 1f;
							}
							if (!flag7 && this.frame >= 4 && this.frame <= 7)
							{
								Vector2 vector13 = Main.player[this.owner].Center - base.Center;
								if (flag6)
								{
									vector13 = vector9;
								}
								float num121 = -vector13.Y;
								if (vector13.Y <= 0f)
								{
									if (num121 < 120f)
									{
										this.velocity.Y = -10f;
									}
									else if (num121 < 210f)
									{
										this.velocity.Y = -13f;
									}
									else if (num121 < 270f)
									{
										this.velocity.Y = -15f;
									}
									else if (num121 < 310f)
									{
										this.velocity.Y = -17f;
									}
									else if (num121 < 380f)
									{
										this.velocity.Y = -18f;
									}
								}
							}
							if (flag7)
							{
								this.frameCounter++;
								if (this.frameCounter > 3)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame >= 8)
								{
									this.frame = 4;
								}
								if (this.frame <= 3)
								{
									this.frame = 7;
								}
							}
							else if (this.velocity.Y >= 0f && (double)this.velocity.Y <= 0.8)
							{
								if (this.velocity.X == 0f)
								{
									this.frame = 0;
									this.frameCounter = 0;
								}
								else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
								{
									this.frameCounter += (int)Math.Abs(this.velocity.X);
									this.frameCounter++;
									if (this.frameCounter > 5)
									{
										this.frame++;
										this.frameCounter = 0;
									}
									if (this.frame > 2)
									{
										this.frame = 0;
									}
								}
								else
								{
									this.frame = 0;
									this.frameCounter = 0;
								}
							}
							else
							{
								this.frameCounter = 0;
								this.frame = 3;
							}
							this.velocity.Y = this.velocity.Y + 0.4f;
							if (this.velocity.Y > 10f)
							{
								this.velocity.Y = 10f;
								return;
							}
						}
					}
					else if (this.type == 314)
					{
						if (this.velocity.Y >= 0f && (double)this.velocity.Y <= 0.8)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 6)
								{
									this.frame = 1;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frameCounter = 0;
							this.frame = 7;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 319)
					{
						if (this.velocity.Y >= 0f && (double)this.velocity.Y <= 0.8)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 8)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 5)
								{
									this.frame = 2;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frameCounter = 0;
							this.frame = 1;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 236)
					{
						if (this.velocity.Y >= 0f && (double)this.velocity.Y <= 0.8)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								if (this.frame < 2)
								{
									this.frame = 2;
								}
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 8)
								{
									this.frame = 2;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frameCounter = 0;
							this.frame = 1;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 499)
					{
						if (this.velocity.Y >= 0f && (double)this.velocity.Y <= 0.8)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								if (this.frame < 2)
								{
									this.frame = 2;
								}
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame >= 8)
								{
									this.frame = 2;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frameCounter = 0;
							this.frame = 1;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 266)
					{
						if (this.velocity.Y >= 0f && (double)this.velocity.Y <= 0.8)
						{
							if (this.velocity.X == 0f)
							{
								this.frameCounter++;
							}
							else
							{
								this.frameCounter += 3;
							}
						}
						else
						{
							this.frameCounter += 5;
						}
						if (this.frameCounter >= 20)
						{
							this.frameCounter -= 20;
							this.frame++;
						}
						if (this.frame > 1)
						{
							this.frame = 0;
						}
						if (this.wet && Main.player[this.owner].position.Y + (float)Main.player[this.owner].height < this.position.Y + (float)this.height && this.localAI[0] == 0f)
						{
							if (this.velocity.Y > -4f)
							{
								this.velocity.Y = this.velocity.Y - 0.2f;
							}
							if (this.velocity.Y > 0f)
							{
								this.velocity.Y = this.velocity.Y * 0.95f;
							}
						}
						else
						{
							this.velocity.Y = this.velocity.Y + 0.4f;
						}
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 334)
					{
						if (this.velocity.Y == 0f)
						{
							if (this.velocity.X == 0f)
							{
								if (this.frame > 0)
								{
									this.frameCounter += 2;
									if (this.frameCounter > 6)
									{
										this.frame++;
										this.frameCounter = 0;
									}
									if (this.frame >= 7)
									{
										this.frame = 0;
									}
								}
								else
								{
									this.frame = 0;
									this.frameCounter = 0;
								}
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs((double)this.velocity.X * 0.75);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame >= 7 || this.frame < 1)
								{
									this.frame = 1;
								}
							}
							else if (this.frame > 0)
							{
								this.frameCounter += 2;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame >= 7)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else if (this.velocity.Y < 0f)
						{
							this.frameCounter = 0;
							this.frame = 2;
						}
						else if (this.velocity.Y > 0f)
						{
							this.frameCounter = 0;
							this.frame = 4;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 353)
					{
						if (this.velocity.Y == 0f)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 9)
								{
									this.frame = 2;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else if (this.velocity.Y < 0f)
						{
							this.frameCounter = 0;
							this.frame = 1;
						}
						else if (this.velocity.Y > 0f)
						{
							this.frameCounter = 0;
							this.frame = 1;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 111)
					{
						if (this.velocity.Y == 0f)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame >= 7)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else if (this.velocity.Y < 0f)
						{
							this.frameCounter = 0;
							this.frame = 4;
						}
						else if (this.velocity.Y > 0f)
						{
							this.frameCounter = 0;
							this.frame = 6;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 112)
					{
						if (this.velocity.Y == 0f)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame >= 3)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else if (this.velocity.Y < 0f)
						{
							this.frameCounter = 0;
							this.frame = 2;
						}
						else if (this.velocity.Y > 0f)
						{
							this.frameCounter = 0;
							this.frame = 2;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 127)
					{
						if (this.velocity.Y == 0f)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.1 || (double)this.velocity.X > 0.1)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 5)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 200)
					{
						if (this.velocity.Y == 0f)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.1 || (double)this.velocity.X > 0.1)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 5)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.rotation = this.velocity.X * 0.1f;
							this.frameCounter++;
							if (this.velocity.Y < 0f)
							{
								this.frameCounter += 2;
							}
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 9)
							{
								this.frame = 6;
							}
							if (this.frame < 6)
							{
								this.frame = 6;
							}
						}
						this.velocity.Y = this.velocity.Y + 0.1f;
						if (this.velocity.Y > 4f)
						{
							this.velocity.Y = 4f;
							return;
						}
					}
					else if (this.type == 208)
					{
						if (this.velocity.Y == 0f && this.velocity.X == 0f)
						{
							if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) < this.position.X + (float)(this.width / 2))
							{
								this.direction = -1;
							}
							else if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) > this.position.X + (float)(this.width / 2))
							{
								this.direction = 1;
							}
							this.rotation = 0f;
							this.frame = 0;
						}
						else
						{
							this.rotation = this.velocity.X * 0.075f;
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 4)
							{
								this.frame = 1;
							}
							if (this.frame < 1)
							{
								this.frame = 1;
							}
						}
						this.velocity.Y = this.velocity.Y + 0.1f;
						if (this.velocity.Y > 4f)
						{
							this.velocity.Y = 4f;
							return;
						}
					}
					else if (this.type == 209)
					{
						if (this.alpha > 0)
						{
							this.alpha -= 5;
							if (this.alpha < 0)
							{
								this.alpha = 0;
							}
						}
						if (this.velocity.Y == 0f)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.1 || (double)this.velocity.X > 0.1)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 11)
								{
									this.frame = 2;
								}
								if (this.frame < 2)
								{
									this.frame = 2;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frame = 1;
							this.frameCounter = 0;
							this.rotation = 0f;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 324)
					{
						if (this.velocity.Y == 0f)
						{
							if ((double)this.velocity.X < -0.1 || (double)this.velocity.X > 0.1)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 5)
								{
									this.frame = 2;
								}
								if (this.frame < 2)
								{
									this.frame = 2;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frameCounter = 0;
							this.frame = 1;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 14f)
						{
							this.velocity.Y = 14f;
							return;
						}
					}
					else if (this.type == 210)
					{
						if (this.velocity.Y == 0f)
						{
							if ((double)this.velocity.X < -0.1 || (double)this.velocity.X > 0.1)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 6)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.rotation = this.velocity.X * 0.05f;
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 11)
							{
								this.frame = 7;
							}
							if (this.frame < 7)
							{
								this.frame = 7;
							}
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
							return;
						}
					}
					else if (this.type == 398)
					{
						if (this.velocity.Y == 0f)
						{
							if (this.velocity.X == 0f)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)this.velocity.X < -0.8 || (double)this.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(this.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame >= 5)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else if (this.velocity.Y != 0f)
						{
							this.frameCounter = 0;
							this.frame = 5;
						}
						this.velocity.Y = this.velocity.Y + 0.4f;
						if (this.velocity.Y > 10f)
						{
							this.velocity.Y = 10f;
						}
					}
				}
			}
		}
		private void AI_062()
		{
			if (this.type == 373)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].hornetMinion = false;
				}
				if (Main.player[this.owner].hornetMinion)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 375)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].impMinion = false;
				}
				if (Main.player[this.owner].impMinion)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 407)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].sharknadoMinion = false;
				}
				if (Main.player[this.owner].sharknadoMinion)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 423)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].UFOMinion = false;
				}
				if (Main.player[this.owner].UFOMinion)
				{
					this.timeLeft = 2;
				}
			}
			if (this.type == 613)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].stardustMinion = false;
				}
				if (Main.player[this.owner].stardustMinion)
				{
					this.timeLeft = 2;
				}
				if (this.localAI[1] > 0f)
				{
					this.localAI[1] -= 1f;
				}
			}
			if (this.type == 423)
			{
				if (this.ai[0] == 2f)
				{
					this.ai[1] -= 1f;
					this.tileCollide = false;
					if (this.ai[1] != 0f)
					{
						return;
					}
					this.ai[1] = 30f;
					this.ai[0] = 0f;
					this.velocity /= 5f;
					this.velocity.Y = 0f;
					this.extraUpdates = 0;
					this.numUpdates = 0;
					this.netUpdate = true;
					this.extraUpdates = 0;
					this.numUpdates = 0;
				}
				if (this.extraUpdates > 1)
				{
					this.extraUpdates = 0;
				}
				if (this.numUpdates > 1)
				{
					this.numUpdates = 0;
				}
			}
			if (this.type == 613)
			{
				if (this.ai[0] == 2f)
				{
					this.ai[1] -= 1f;
					this.tileCollide = false;
					if (this.ai[1] > 3f)
					{
						float num2 = 2f - (float)this.numUpdates / 30f;
						if (this.scale > 0f)
						{
						}
					}
					if (this.ai[1] != 0f)
					{
						return;
					}
					this.ai[1] = 30f;
					this.ai[0] = 0f;
					this.velocity /= 5f;
					this.velocity.Y = 0f;
					this.extraUpdates = 0;
					this.numUpdates = 0;
					this.netUpdate = true;
					this.extraUpdates = 0;
					this.numUpdates = 0;
				}
				if (this.extraUpdates > 1)
				{
					this.extraUpdates = 0;
				}
				if (this.numUpdates > 1)
				{
					this.numUpdates = 0;
				}
			}
			if (this.type == 423 && this.localAI[0] > 0f)
			{
				this.localAI[0] -= 1f;
			}
			if (this.type == 613 && this.localAI[0] > 0f)
			{
				this.localAI[0] -= 1f;
			}
			float num8 = 0.05f;
			float num9 = (float)this.width;
			if (this.type == 407)
			{
				num8 = 0.1f;
				num9 *= 2f;
			}
			for (int j = 0; j < 1000; j++)
			{
				if (j != this.whoAmI && Main.projectile[j].active && Main.projectile[j].owner == this.owner && Main.projectile[j].type == this.type && Math.Abs(this.position.X - Main.projectile[j].position.X) + Math.Abs(this.position.Y - Main.projectile[j].position.Y) < num9)
				{
					if (this.position.X < Main.projectile[j].position.X)
					{
						this.velocity.X = this.velocity.X - num8;
					}
					else
					{
						this.velocity.X = this.velocity.X + num8;
					}
					if (this.position.Y < Main.projectile[j].position.Y)
					{
						this.velocity.Y = this.velocity.Y - num8;
					}
					else
					{
						this.velocity.Y = this.velocity.Y + num8;
					}
				}
			}
			Vector2 vector = this.position;
			float num10 = 400f;
			if (this.type == 423)
			{
				num10 = 300f;
			}
			if (this.type == 613)
			{
				num10 = 300f;
			}
			bool flag = false;
			int num11 = -1;
			this.tileCollide = true;
			if (this.type == 407)
			{
				this.tileCollide = false;
				if (Collision.SolidCollision(this.position, this.width, this.height))
				{
					this.alpha += 20;
					if (this.alpha > 150)
					{
						this.alpha = 150;
					}
				}
				else
				{
					this.alpha -= 50;
					if (this.alpha < 60)
					{
						this.alpha = 60;
					}
				}
			}
			if (this.type == 407 || this.type == 613 || this.type == 423)
			{
				Vector2 center = Main.player[this.owner].Center;
				Vector2 vector2 = new Vector2(0.5f);
				if (this.type == 423)
				{
					vector2.Y = 0f;
				}
				for (int k = 0; k < 200; k++)
				{
					NPC nPC = Main.npc[k];
					if (nPC.CanBeChasedBy(this, false))
					{
						Vector2 vector3 = nPC.position + nPC.Size * vector2;
						float num12 = Vector2.Distance(vector3, center);
						if (((Vector2.Distance(center, vector) > num12 && num12 < num10) || !flag) && Collision.CanHitLine(this.position, this.width, this.height, nPC.position, nPC.width, nPC.height))
						{
							num10 = num12;
							vector = vector3;
							flag = true;
							num11 = k;
						}
					}
				}
			}
			else
			{
				for (int l = 0; l < 200; l++)
				{
					NPC nPC2 = Main.npc[l];
					if (nPC2.CanBeChasedBy(this, false))
					{
						float num13 = Vector2.Distance(nPC2.Center, base.Center);
						if (((Vector2.Distance(base.Center, vector) > num13 && num13 < num10) || !flag) && Collision.CanHitLine(this.position, this.width, this.height, nPC2.position, nPC2.width, nPC2.height))
						{
							num10 = num13;
							vector = nPC2.Center;
							flag = true;
							num11 = l;
						}
					}
				}
			}
			int num14 = 500;
			if (flag)
			{
				num14 = 1000;
			}
			if (flag && this.type == 423)
			{
				num14 = 1200;
			}
			if (flag && this.type == 613)
			{
				num14 = 1350;
			}
			Player player = Main.player[this.owner];
			float num15 = Vector2.Distance(player.Center, base.Center);
			if (num15 > (float)num14)
			{
				this.ai[0] = 1f;
				this.netUpdate = true;
			}
			if (this.ai[0] == 1f)
			{
				this.tileCollide = false;
			}
			if (flag && this.ai[0] == 0f)
			{
				Vector2 vector4 = vector - base.Center;
				float num16 = vector4.Length();
				vector4.Normalize();
				if (this.type == 423)
				{
					vector4 = vector - Vector2.UnitY * 80f;
					int num17 = (int)vector4.Y / 16;
					if (num17 < 0)
					{
						num17 = 0;
					}
					Tile tile = Main.tile[(int)vector4.X / 16, num17];
					if (tile != null && tile.active() && Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type])
					{
						vector4 += Vector2.UnitY * 16f;
						tile = Main.tile[(int)vector4.X / 16, (int)vector4.Y / 16];
						if (tile != null && tile.active() && Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type])
						{
							vector4 += Vector2.UnitY * 16f;
						}
					}
					vector4 -= base.Center;
					num16 = vector4.Length();
					vector4.Normalize();
					if (num16 > 300f && num16 <= 800f && this.localAI[0] == 0f)
					{
						this.ai[0] = 2f;
						this.ai[1] = (float)((int)(num16 / 10f));
						this.extraUpdates = (int)this.ai[1];
						this.velocity = vector4 * 10f;
						this.localAI[0] = 60f;
						return;
					}
				}
				if (this.type == 613)
				{
					vector4 = vector;
					Vector2 vector5 = base.Center - vector4;
					if (vector5 == Vector2.Zero)
					{
						vector5 = -Vector2.UnitY;
					}
					vector5.Normalize();
					vector4 += vector5 * 60f;
					int num18 = (int)vector4.Y / 16;
					if (num18 < 0)
					{
						num18 = 0;
					}
					Tile tile2 = Main.tile[(int)vector4.X / 16, num18];
					if (tile2 != null && tile2.active() && Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type])
					{
						vector4 += Vector2.UnitY * 16f;
						tile2 = Main.tile[(int)vector4.X / 16, (int)vector4.Y / 16];
						if (tile2 != null && tile2.active() && Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type])
						{
							vector4 += Vector2.UnitY * 16f;
						}
					}
					vector4 -= base.Center;
					num16 = vector4.Length();
					vector4.Normalize();
					if (num16 > 400f && num16 <= 800f && this.localAI[0] == 0f)
					{
						this.ai[0] = 2f;
						this.ai[1] = (float)((int)(num16 / 10f));
						this.extraUpdates = (int)this.ai[1];
						this.velocity = vector4 * 10f;
						this.localAI[0] = 60f;
						return;
					}
				}
				if (this.type == 407)
				{
					if (num16 > 400f)
					{
						float num19 = 2f;
						vector4 *= num19;
						this.velocity = (this.velocity * 20f + vector4) / 21f;
					}
					else
					{
						this.velocity *= 0.96f;
					}
				}
				if (num16 > 200f)
				{
					float num20 = 6f;
					vector4 *= num20;
					this.velocity.X = (this.velocity.X * 40f + vector4.X) / 41f;
					this.velocity.Y = (this.velocity.Y * 40f + vector4.Y) / 41f;
				}
				else if (this.type == 423 || this.type == 613)
				{
					if (num16 > 70f && num16 < 130f)
					{
						float num21 = 7f;
						if (num16 < 100f)
						{
							num21 = -3f;
						}
						vector4 *= num21;
						this.velocity = (this.velocity * 20f + vector4) / 21f;
						if (Math.Abs(vector4.X) > Math.Abs(vector4.Y))
						{
							this.velocity.X = (this.velocity.X * 10f + vector4.X) / 11f;
						}
					}
					else
					{
						this.velocity *= 0.97f;
					}
				}
				else if (this.type == 375)
				{
					if (num16 < 150f)
					{
						float num22 = 4f;
						vector4 *= -num22;
						this.velocity.X = (this.velocity.X * 40f + vector4.X) / 41f;
						this.velocity.Y = (this.velocity.Y * 40f + vector4.Y) / 41f;
					}
					else
					{
						this.velocity *= 0.97f;
					}
				}
				else if (this.velocity.Y > -1f)
				{
					this.velocity.Y = this.velocity.Y - 0.1f;
				}
			}
			else
			{
				if (!Collision.CanHitLine(base.Center, 1, 1, Main.player[this.owner].Center, 1, 1))
				{
					this.ai[0] = 1f;
				}
				float num23 = 6f;
				if (this.ai[0] == 1f)
				{
					num23 = 15f;
				}
				if (this.type == 407)
				{
					num23 = 9f;
				}
				Vector2 center2 = base.Center;
				Vector2 vector6 = player.Center - center2 + new Vector2(0f, -60f);
				if (this.type == 407)
				{
					vector6 += new Vector2(0f, 40f);
				}
				if (this.type == 375)
				{
					this.ai[1] = 3600f;
					this.netUpdate = true;
					vector6 = player.Center - center2;
					int num24 = 1;
					for (int m = 0; m < this.whoAmI; m++)
					{
						if (Main.projectile[m].active && Main.projectile[m].owner == this.owner && Main.projectile[m].type == this.type)
						{
							num24++;
						}
					}
					vector6.X -= (float)(10 * Main.player[this.owner].direction);
					vector6.X -= (float)(num24 * 40 * Main.player[this.owner].direction);
					vector6.Y -= 10f;
				}
				float num25 = vector6.Length();
				if (num25 > 200f && num23 < 9f)
				{
					num23 = 9f;
				}
				if (this.type == 375)
				{
					num23 = (float)((int)((double)num23 * 0.75));
				}
				if (num25 < 100f && this.ai[0] == 1f && !Collision.SolidCollision(this.position, this.width, this.height))
				{
					this.ai[0] = 0f;
					this.netUpdate = true;
				}
				if (num25 > 2000f)
				{
					this.position.X = Main.player[this.owner].Center.X - (float)(this.width / 2);
					this.position.Y = Main.player[this.owner].Center.Y - (float)(this.width / 2);
				}
				if (this.type == 375)
				{
					if (num25 > 10f)
					{
						vector6.Normalize();
						if (num25 < 50f)
						{
							num23 /= 2f;
						}
						vector6 *= num23;
						this.velocity = (this.velocity * 20f + vector6) / 21f;
					}
					else
					{
						this.direction = Main.player[this.owner].direction;
						this.velocity *= 0.9f;
					}
				}
				else if (this.type == 407)
				{
					if (Math.Abs(vector6.X) > 40f || Math.Abs(vector6.Y) > 10f)
					{
						vector6.Normalize();
						vector6 *= num23;
						vector6 *= new Vector2(1.25f, 0.65f);
						this.velocity = (this.velocity * 20f + vector6) / 21f;
					}
					else
					{
						if (this.velocity.X == 0f && this.velocity.Y == 0f)
						{
							this.velocity.X = -0.15f;
							this.velocity.Y = -0.05f;
						}
						this.velocity *= 1.01f;
					}
				}
				else if (num25 > 70f)
				{
					vector6.Normalize();
					vector6 *= num23;
					this.velocity = (this.velocity * 20f + vector6) / 21f;
				}
				else
				{
					if (this.velocity.X == 0f && this.velocity.Y == 0f)
					{
						this.velocity.X = -0.15f;
						this.velocity.Y = -0.05f;
					}
					this.velocity *= 1.01f;
				}
			}
			this.rotation = this.velocity.X * 0.05f;
			this.frameCounter++;
			if (this.type == 373)
			{
				if (this.frameCounter > 1)
				{
					this.frame++;
					this.frameCounter = 0;
				}
				if (this.frame > 2)
				{
					this.frame = 0;
				}
			}
			if (this.type == 375)
			{
				if (this.frameCounter >= 16)
				{
					this.frameCounter = 0;
				}
				this.frame = this.frameCounter / 4;
				if (this.ai[1] > 0f && this.ai[1] < 16f)
				{
					this.frame += 4;
				}
			}
			if (this.type == 407)
			{
				int num27 = 2;
				if (this.frameCounter >= 6 * num27)
				{
					this.frameCounter = 0;
				}
				this.frame = this.frameCounter / num27;
			}
			if (this.type == 423 || this.type == 613)
			{
				int num29 = 3;
				if (this.frameCounter >= 4 * num29)
				{
					this.frameCounter = 0;
				}
				this.frame = this.frameCounter / num29;
			}
			if (this.velocity.X > 0f)
			{
				this.spriteDirection = (this.direction = -1);
			}
			else if (this.velocity.X < 0f)
			{
				this.spriteDirection = (this.direction = 1);
			}
			if (this.type == 373)
			{
				if (this.ai[1] > 0f)
				{
					this.ai[1] += (float)Main.rand.Next(1, 4);
				}
				if (this.ai[1] > 90f)
				{
					this.ai[1] = 0f;
					this.netUpdate = true;
				}
			}
			else if (this.type == 375)
			{
				if (this.ai[1] > 0f)
				{
					this.ai[1] += 1f;
					if (Main.rand.Next(3) == 0)
					{
						this.ai[1] += 1f;
					}
				}
				if (this.ai[1] > (float)Main.rand.Next(180, 900))
				{
					this.ai[1] = 0f;
					this.netUpdate = true;
				}
			}
			else if (this.type == 407)
			{
				if (this.ai[1] > 0f)
				{
					this.ai[1] += 1f;
					if (Main.rand.Next(3) != 0)
					{
						this.ai[1] += 1f;
					}
				}
				if (this.ai[1] > 60f)
				{
					this.ai[1] = 0f;
					this.netUpdate = true;
				}
			}
			else if (this.type == 423)
			{
				if (this.ai[1] > 0f)
				{
					this.ai[1] += 1f;
					if (Main.rand.Next(3) != 0)
					{
						this.ai[1] += 1f;
					}
				}
				if (this.ai[1] > 30f)
				{
					this.ai[1] = 0f;
					this.netUpdate = true;
				}
			}
			else if (this.type == 613)
			{
				if (this.ai[1] > 0f)
				{
					this.ai[1] += 1f;
					if (Main.rand.Next(3) != 0)
					{
						this.ai[1] += 1f;
					}
				}
				if (this.ai[1] > 60f)
				{
					this.ai[1] = 0f;
					this.netUpdate = true;
				}
			}
			if (this.ai[0] == 0f)
			{
				float num30 = 0f;
				int num31 = 0;
				if (this.type == 373)
				{
					num30 = 10f;
					num31 = 374;
				}
				else if (this.type == 375)
				{
					num30 = 11f;
					num31 = 376;
				}
				else if (this.type == 407)
				{
					num30 = 14f;
					num31 = 408;
				}
				else if (this.type == 423)
				{
					num30 = 4f;
					num31 = 433;
				}
				else if (this.type == 613)
				{
					num30 = 14f;
					num31 = 614;
				}
				if (flag)
				{
					if (this.type == 375)
					{
						if ((vector - base.Center).X > 0f)
						{
							this.spriteDirection = (this.direction = -1);
						}
						else if ((vector - base.Center).X < 0f)
						{
							this.spriteDirection = (this.direction = 1);
						}
					}
					if (this.type == 407 && Collision.SolidCollision(this.position, this.width, this.height))
					{
						return;
					}
					if (this.type == 423)
					{
						if (Math.Abs((vector - base.Center).ToRotation() - 1.57079637f) > 0.7853982f)
						{
							this.velocity += Vector2.Normalize(vector - base.Center - Vector2.UnitY * 80f);
							return;
						}
						if ((vector - base.Center).Length() > 400f)
						{
							return;
						}
						if (this.ai[1] == 0f)
						{
							this.ai[1] += 1f;
							if (Main.myPlayer == this.owner)
							{
								Vector2 vector7 = vector - base.Center;
								vector7.Normalize();
								vector7 *= num30;
								Projectile.NewProjectile(base.Center.X, base.Center.Y, vector7.X, vector7.Y, num31, this.damage, 0f, Main.myPlayer, 0f, 0f);
								this.netUpdate = true;
								return;
							}
						}
					}
					else if (this.ai[1] == 0f && this.type == 613)
					{
						if ((vector - base.Center).Length() > 500f)
						{
							return;
						}
						if (this.ai[1] == 0f)
						{
							this.ai[1] += 1f;
							if (Main.myPlayer == this.owner)
							{
								Vector2 vector8 = vector - base.Center;
								vector8.Normalize();
								vector8 *= num30;
								int num32 = Projectile.NewProjectile(base.Center.X, base.Center.Y, vector8.X, vector8.Y, num31, this.damage, 0f, Main.myPlayer, 0f, (float)num11);
								Main.projectile[num32].timeLeft = 300;
								Main.projectile[num32].netUpdate = true;
								this.velocity -= vector8 / 3f;
								this.netUpdate = true;
							}
							return;
						}
					}
					else if (this.ai[1] == 0f)
					{
						this.ai[1] += 1f;
						if (Main.myPlayer == this.owner)
						{
							Vector2 vector10 = vector - base.Center;
							vector10.Normalize();
							vector10 *= num30;
							int num35 = Projectile.NewProjectile(base.Center.X, base.Center.Y, vector10.X, vector10.Y, num31, this.damage, 0f, Main.myPlayer, 0f, 0f);
							Main.projectile[num35].timeLeft = 300;
							Main.projectile[num35].netUpdate = true;
							this.netUpdate = true;
						}
					}
				}
			}
		}
		private void AI_075()
		{
			Player player = Main.player[this.owner];
			float num = 1.57079637f;
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
			if (this.type == 439)
			{
				this.ai[0] += 1f;
				int num2 = 0;
				if (this.ai[0] >= 40f)
				{
					num2++;
				}
				if (this.ai[0] >= 80f)
				{
					num2++;
				}
				if (this.ai[0] >= 120f)
				{
					num2++;
				}
				int num3 = 24;
				int num4 = 6;
				this.ai[1] += 1f;
				bool flag = false;
				if (this.ai[1] >= (float)(num3 - num4 * num2))
				{
					this.ai[1] = 0f;
					flag = true;
				}
				this.frameCounter += 1 + num2;
				if (this.frameCounter >= 4)
				{
					this.frameCounter = 0;
					this.frame++;
					if (this.frame >= 6)
					{
						this.frame = 0;
					}
				}
				if (this.ai[1] == 1f && this.ai[0] != 1f)
				{
					Vector2 vector2 = Vector2.UnitX * 24f;
					vector2 = vector2.RotatedBy((double)(this.rotation - 1.57079637f), default(Vector2));
					Vector2 vector3 = base.Center + vector2;
				}
				if (flag && Main.myPlayer == this.owner)
				{
					bool flag2 = player.channel && player.CheckMana(player.inventory[player.selectedItem].mana, true, false) && !player.noItems && !player.CCed;
					if (flag2)
					{
						float num6 = player.inventory[player.selectedItem].shootSpeed * this.scale;
						Vector2 vector4 = vector;
						Vector2 vector5 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - vector4;
						if (player.gravDir == -1f)
						{
							vector5.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector4.Y;
						}
						Vector2 vector6 = Vector2.Normalize(vector5);
						if (float.IsNaN(vector6.X) || float.IsNaN(vector6.Y))
						{
							vector6 = -Vector2.UnitY;
						}
						vector6 *= num6;
						if (vector6.X != this.velocity.X || vector6.Y != this.velocity.Y)
						{
							this.netUpdate = true;
						}
						this.velocity = vector6;
						int num7 = 440;
						float num8 = 14f;
						int num9 = 7;
						for (int j = 0; j < 2; j++)
						{
							vector4 = base.Center + new Vector2((float)Main.rand.Next(-num9, num9 + 1), (float)Main.rand.Next(-num9, num9 + 1));
							Vector2 spinningpoint = Vector2.Normalize(this.velocity) * num8;
							spinningpoint = spinningpoint.RotatedBy(Main.rand.NextDouble() * 0.19634954631328583 - 0.098174773156642914, default(Vector2));
							if (float.IsNaN(spinningpoint.X) || float.IsNaN(spinningpoint.Y))
							{
								spinningpoint = -Vector2.UnitY;
							}
							Projectile.NewProjectile(vector4.X, vector4.Y, spinningpoint.X, spinningpoint.Y, num7, this.damage, this.knockBack, this.owner, 0f, 0f);
						}
					}
					else
					{
						this.Kill();
					}
				}
			}
			if (this.type == 445)
			{
				this.localAI[0] += 1f;
				if (this.localAI[0] >= 60f)
				{
					this.localAI[0] = 0f;
				}
				if (Vector2.Distance(vector, base.Center) >= 5f)
				{
					float num10 = this.localAI[0] / 60f;
					if (num10 > 0.5f)
					{
						num10 = 1f - num10;
					}
					Vector3 vector7 = new Vector3(0f, 1f, 0.7f);
					Vector3 vector8 = new Vector3(0f, 0.7f, 1f);
					Vector3 vector9 = Vector3.Lerp(vector7, vector8, 1f - num10 * 2f) * 0.5f;
					if (Vector2.Distance(vector, base.Center) >= 30f)
					{
						Vector2 vector10 = base.Center - vector;
						vector10.Normalize();
						vector10 *= Vector2.Distance(vector, base.Center) - 30f;
						DelegateMethods.v3_1 = vector9 * 0.8f;
					}
				}
				if (Main.myPlayer == this.owner)
				{
					if (this.localAI[1] > 0f)
					{
						this.localAI[1] -= 1f;
					}
					if (!player.channel || player.noItems || player.CCed)
					{
						this.Kill();
					}
					else if (this.localAI[1] == 0f)
					{
						Vector2 vector11 = vector;
						Vector2 vector12 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - vector11;
						if (player.gravDir == -1f)
						{
							vector12.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector11.Y;
						}
						Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
						if (tile.active())
						{
							vector12 = new Vector2((float)Player.tileTargetX, (float)Player.tileTargetY) * 16f + Vector2.One * 8f - vector11;
							this.localAI[1] = 2f;
						}
						vector12 = Vector2.Lerp(vector12, this.velocity, 0.7f);
						if (float.IsNaN(vector12.X) || float.IsNaN(vector12.Y))
						{
							vector12 = -Vector2.UnitY;
						}
						float num11 = 30f;
						if (vector12.Length() < num11)
						{
							vector12 = Vector2.Normalize(vector12) * num11;
						}
						int tileBoost = player.inventory[player.selectedItem].tileBoost;
						int num12 = -Player.tileRangeX - tileBoost + 1;
						int num13 = Player.tileRangeX + tileBoost - 1;
						int num14 = -Player.tileRangeY - tileBoost;
						int num15 = Player.tileRangeY + tileBoost - 1;
						int num16 = 12;
						bool flag3 = false;
						if (vector12.X < (float)(num12 * 16 - num16))
						{
							flag3 = true;
						}
						if (vector12.Y < (float)(num14 * 16 - num16))
						{
							flag3 = true;
						}
						if (vector12.X > (float)(num13 * 16 + num16))
						{
							flag3 = true;
						}
						if (vector12.Y > (float)(num15 * 16 + num16))
						{
							flag3 = true;
						}
						if (flag3)
						{
							Vector2 vector13 = Vector2.Normalize(vector12);
							float num17 = -1f;
							if (vector13.X < 0f && ((float)(num12 * 16 - num16) / vector13.X < num17 || num17 == -1f))
							{
								num17 = (float)(num12 * 16 - num16) / vector13.X;
							}
							if (vector13.X > 0f && ((float)(num13 * 16 + num16) / vector13.X < num17 || num17 == -1f))
							{
								num17 = (float)(num13 * 16 + num16) / vector13.X;
							}
							if (vector13.Y < 0f && ((float)(num14 * 16 - num16) / vector13.Y < num17 || num17 == -1f))
							{
								num17 = (float)(num14 * 16 - num16) / vector13.Y;
							}
							if (vector13.Y > 0f && ((float)(num15 * 16 + num16) / vector13.Y < num17 || num17 == -1f))
							{
								num17 = (float)(num15 * 16 + num16) / vector13.Y;
							}
							vector12 = vector13 * num17;
						}
						if (vector12.X != this.velocity.X || vector12.Y != this.velocity.Y)
						{
							this.netUpdate = true;
						}
						this.velocity = vector12;
					}
				}
			}
			if (this.type == 460)
			{
				this.ai[0] += 1f;
				int num18 = 0;
				if (this.ai[0] >= 60f)
				{
					num18++;
				}
				if (this.ai[0] >= 180f)
				{
					num18++;
				}
				bool flag4 = false;
				if (this.ai[0] == 60f || this.ai[0] == 180f || (this.ai[0] > 180f && this.ai[0] % 20f == 0f))
				{
					flag4 = true;
				}
				bool flag5 = this.ai[0] >= 180f;
				int num19 = 10;
				if (!flag5)
				{
					this.ai[1] += 1f;
				}
				bool flag6 = false;
				if (flag5 && this.ai[0] % 20f == 0f)
				{
					flag6 = true;
				}
				if (this.ai[1] >= (float)num19 && !flag5)
				{
					this.ai[1] = 0f;
					flag6 = true;
					if (!flag5)
					{
						float num20 = player.inventory[player.selectedItem].shootSpeed * this.scale;
						Vector2 vector14 = vector;
						Vector2 vector15 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - vector14;
						if (player.gravDir == -1f)
						{
							vector15.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector14.Y;
						}
						Vector2 vector16 = Vector2.Normalize(vector15);
						if (float.IsNaN(vector16.X) || float.IsNaN(vector16.Y))
						{
							vector16 = -Vector2.UnitY;
						}
						vector16 *= num20;
						if (vector16.X != this.velocity.X || vector16.Y != this.velocity.Y)
						{
							this.netUpdate = true;
						}
						this.velocity = vector16;
					}
				}
				if (flag6 && Main.myPlayer == this.owner)
				{
					bool flag7 = !flag4 || player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
					bool flag8 = player.channel && flag7 && !player.noItems && !player.CCed;
					if (flag8)
					{
						if (this.ai[0] == 180f)
						{
							Vector2 center = base.Center;
							Vector2 vector20 = Vector2.Normalize(this.velocity);
							if (float.IsNaN(vector20.X) || float.IsNaN(vector20.Y))
							{
								vector20 = -Vector2.UnitY;
							}
							int num24 = (int)((float)this.damage * 3f);
							int num25 = Projectile.NewProjectile(center.X, center.Y, vector20.X, vector20.Y, 461, num24, this.knockBack, this.owner, 0f, (float)this.whoAmI);
							this.ai[1] = (float)num25;
							this.netUpdate = true;
						}
						else if (flag5)
						{
							Projectile projectile = Main.projectile[(int)this.ai[1]];
							if (!projectile.active || projectile.type != 461)
							{
								this.Kill();
								return;
							}
						}
					}
					else
					{
						if (!flag5)
						{
							int num26 = 459;
							float num27 = 10f;
							Vector2 center2 = base.Center;
							Vector2 vector21 = Vector2.Normalize(this.velocity) * num27;
							if (float.IsNaN(vector21.X) || float.IsNaN(vector21.Y))
							{
								vector21 = -Vector2.UnitY;
							}
							float num28 = 0.7f + (float)num18 * 0.3f;
							int num29 = (num28 < 1f) ? this.damage : ((int)((float)this.damage * 2f));
							Projectile.NewProjectile(center2.X, center2.Y, vector21.X, vector21.Y, num26, num29, this.knockBack, this.owner, 0f, num28);
						}
						this.Kill();
					}
				}
			}
			if (this.type == 633)
			{
				float num30 = 30f;
				if (this.ai[0] > 90f)
				{
					num30 = 15f;
				}
				if (this.ai[0] > 120f)
				{
					num30 = 5f;
				}
				this.damage = (int)((float)player.inventory[player.selectedItem].damage * player.magicDamage);
				this.ai[0] += 1f;
				this.ai[1] += 1f;
				bool flag9 = false;
				if (this.ai[0] % num30 == 0f)
				{
					flag9 = true;
				}
				int num31 = 10;
				bool flag10 = false;
				if (this.ai[0] % num30 == 0f)
				{
					flag10 = true;
				}
				if (this.ai[1] >= 1f)
				{
					this.ai[1] = 0f;
					flag10 = true;
					if (Main.myPlayer == this.owner)
					{
						float num32 = player.inventory[player.selectedItem].shootSpeed * this.scale;
						Vector2 vector22 = vector;
						Vector2 vector23 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - vector22;
						if (player.gravDir == -1f)
						{
							vector23.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector22.Y;
						}
						Vector2 vector24 = Vector2.Normalize(vector23);
						if (float.IsNaN(vector24.X) || float.IsNaN(vector24.Y))
						{
							vector24 = -Vector2.UnitY;
						}
						vector24 = Vector2.Normalize(Vector2.Lerp(vector24, Vector2.Normalize(this.velocity), 0.92f));
						vector24 *= num32;
						if (vector24.X != this.velocity.X || vector24.Y != this.velocity.Y)
						{
							this.netUpdate = true;
						}
						this.velocity = vector24;
					}
				}
				this.frameCounter++;
				int num33 = (this.ai[0] < 120f) ? 4 : 1;
				if (this.frameCounter >= num33)
				{
					this.frameCounter = 0;
					if (++this.frame >= 5)
					{
						this.frame = 0;
					}
				}
				if (this.soundDelay <= 0)
				{
					this.soundDelay = num31;
					this.soundDelay *= 2;
				}
				if (flag10 && Main.myPlayer == this.owner)
				{
					bool flag11 = !flag9 || player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
					bool flag12 = player.channel && flag11 && !player.noItems && !player.CCed;
					if (flag12)
					{
						if (this.ai[0] == 1f)
						{
							Vector2 center3 = base.Center;
							Vector2 vector25 = Vector2.Normalize(this.velocity);
							if (float.IsNaN(vector25.X) || float.IsNaN(vector25.Y))
							{
								vector25 = -Vector2.UnitY;
							}
							int num34 = this.damage;
							for (int l = 0; l < 6; l++)
							{
								Projectile.NewProjectile(center3.X, center3.Y, vector25.X, vector25.Y, 632, num34, this.knockBack, this.owner, (float)l, (float)this.whoAmI);
							}
							this.netUpdate = true;
						}
					}
					else
					{
						this.Kill();
					}
				}
			}
			if (this.type == 595)
			{
				num = 0f;
				if (this.spriteDirection == -1)
				{
					num = 3.14159274f;
				}
				if (++this.frame >= Main.projFrames[this.type])
				{
					this.frame = 0;
				}
				if (Main.myPlayer == this.owner)
				{
					if (player.channel && !player.noItems && !player.CCed)
					{
						float num35 = 1f;
						if (player.inventory[player.selectedItem].shoot == this.type)
						{
							num35 = player.inventory[player.selectedItem].shootSpeed * this.scale;
						}
						Vector2 vector26 = Main.MouseWorld - vector;
						vector26.Normalize();
						if (vector26.HasNaNs())
						{
							vector26 = Vector2.UnitX * (float)player.direction;
						}
						vector26 *= num35;
						if (vector26.X != this.velocity.X || vector26.Y != this.velocity.Y)
						{
							this.netUpdate = true;
						}
						this.velocity = vector26;
					}
					else
					{
						this.Kill();
					}
				}
			}
			if (this.type == 600)
			{
				this.ai[0] += 1f;
				if (Main.myPlayer == this.owner && this.ai[0] == 1f)
				{
					float num37 = player.inventory[player.selectedItem].shootSpeed * this.scale;
					Vector2 vector28 = vector;
					Vector2 vector29 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - vector28;
					if (player.gravDir == -1f)
					{
						vector29.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector28.Y;
					}
					Vector2 vector30 = Vector2.Normalize(vector29);
					if (float.IsNaN(vector30.X) || float.IsNaN(vector30.Y))
					{
						vector30 = -Vector2.UnitY;
					}
					vector30 *= num37;
					if (vector30.X != this.velocity.X || vector30.Y != this.velocity.Y)
					{
						this.netUpdate = true;
					}
					this.velocity = vector30;
					int num38 = 601;
					float num39 = 3f;
					vector28 = base.Center;
					Vector2 vector31 = Vector2.Normalize(this.velocity) * num39;
					if (float.IsNaN(vector31.X) || float.IsNaN(vector31.Y))
					{
						vector31 = -Vector2.UnitY;
					}
					Projectile.NewProjectile(vector28.X, vector28.Y, vector31.X, vector31.Y, num38, this.damage, this.knockBack, this.owner, this.ai[1], 0f);
				}
				if (this.ai[0] >= 30f)
				{
					this.Kill();
				}
			}
			if (this.type == 611)
			{
				if (this.localAI[1] > 0f)
				{
					this.localAI[1] -= 1f;
				}
				this.alpha -= 42;
				if (this.alpha < 0)
				{
					this.alpha = 0;
				}
				if (this.localAI[0] == 0f)
				{
					this.localAI[0] = this.velocity.ToRotation();
				}
				float num40 = (float)((this.localAI[0].ToRotationVector2().X >= 0f) ? 1 : -1);
				if (this.ai[1] <= 0f)
				{
					num40 *= -1f;
				}
				Vector2 vector32 = (num40 * (this.ai[0] / 30f * 6.28318548f - 1.57079637f)).ToRotationVector2();
				vector32.Y *= (float)Math.Sin((double)this.ai[1]);
				if (this.ai[1] <= 0f)
				{
					vector32.Y *= -1f;
				}
				vector32 = vector32.RotatedBy((double)this.localAI[0], default(Vector2));
				this.ai[0] += 1f;
				if (this.ai[0] < 30f)
				{
					this.velocity += 48f * vector32;
				}
				else
				{
					this.Kill();
				}
			}
			if (this.type == 615)
			{
				num = 0f;
				if (this.spriteDirection == -1)
				{
					num = 3.14159274f;
				}
				this.ai[0] += 1f;
				int num41 = 0;
				if (this.ai[0] >= 40f)
				{
					num41++;
				}
				if (this.ai[0] >= 80f)
				{
					num41++;
				}
				if (this.ai[0] >= 120f)
				{
					num41++;
				}
				int num42 = 5;
				int num43 = 0;
				this.ai[1] -= 1f;
				bool flag13 = false;
				int num44 = -1;
				if (this.ai[1] <= 0f)
				{
					this.ai[1] = (float)(num42 - num43 * num41);
					flag13 = true;
					int num45 = (int)this.ai[0] / (num42 - num43 * num41);
					if (num45 % 7 == 0)
					{
						num44 = 0;
					}
				}
				this.frameCounter += 1 + num41;
				if (this.frameCounter >= 4)
				{
					this.frameCounter = 0;
					this.frame++;
					if (this.frame >= Main.projFrames[this.type])
					{
						this.frame = 0;
					}
				}
				if (this.soundDelay <= 0)
				{
					this.soundDelay = num42 - num43 * num41;
				}
				if (flag13 && Main.myPlayer == this.owner)
				{
					bool flag14 = player.channel && player.HasAmmo(player.inventory[player.selectedItem], true) && !player.noItems && !player.CCed;
					int num46 = 14;
					float num47 = 14f;
					int weaponDamage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
					float weaponKnockback = player.inventory[player.selectedItem].knockBack;
					if (flag14)
					{
						player.PickAmmo(player.inventory[player.selectedItem], ref num46, ref num47, ref flag14, ref weaponDamage, ref weaponKnockback, false);
						float num50 = player.inventory[player.selectedItem].shootSpeed * this.scale;
						Vector2 vector33 = vector;
						Vector2 vector34 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - vector33;
						if (player.gravDir == -1f)
						{
							vector34.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector33.Y;
						}
						Vector2 vector35 = Vector2.Normalize(vector34);
						if (float.IsNaN(vector35.X) || float.IsNaN(vector35.Y))
						{
							vector35 = -Vector2.UnitY;
						}
						vector35 *= num50;
						vector35 = vector35.RotatedBy(Main.rand.NextDouble() * 0.13089969754219055 - 0.065449848771095276, default(Vector2));
						if (vector35.X != this.velocity.X || vector35.Y != this.velocity.Y)
						{
							this.netUpdate = true;
						}
						this.velocity = vector35;
						for (int m = 0; m < 1; m++)
						{
							Vector2 spinningpoint2 = Vector2.Normalize(this.velocity) * num47;
							spinningpoint2 = spinningpoint2.RotatedBy(Main.rand.NextDouble() * 0.19634954631328583 - 0.098174773156642914, default(Vector2));
							if (float.IsNaN(spinningpoint2.X) || float.IsNaN(spinningpoint2.Y))
							{
								spinningpoint2 = -Vector2.UnitY;
							}
							Projectile.NewProjectile(vector33.X, vector33.Y, spinningpoint2.X, spinningpoint2.Y, num46, weaponDamage, weaponKnockback, this.owner, 0f, 0f);
						}
						if (num44 == 0)
						{
							num46 = 616;
							num47 = 8f;
							for (int n = 0; n < 1; n++)
							{
								Vector2 spinningpoint3 = Vector2.Normalize(this.velocity) * num47;
								spinningpoint3 = spinningpoint3.RotatedBy(Main.rand.NextDouble() * 0.39269909262657166 - 0.19634954631328583, default(Vector2));
								if (float.IsNaN(spinningpoint3.X) || float.IsNaN(spinningpoint3.Y))
								{
									spinningpoint3 = -Vector2.UnitY;
								}
								Projectile.NewProjectile(vector33.X, vector33.Y, spinningpoint3.X, spinningpoint3.Y, num46, weaponDamage + 20, weaponKnockback * 1.25f, this.owner, 0f, 0f);
							}
						}
					}
					else
					{
						this.Kill();
					}
				}
			}
			if (this.type == 630)
			{
				num = 0f;
				if (this.spriteDirection == -1)
				{
					num = 3.14159274f;
				}
				this.ai[0] += 1f;
				int num51 = 0;
				if (this.ai[0] >= 40f)
				{
					num51++;
				}
				if (this.ai[0] >= 80f)
				{
					num51++;
				}
				if (this.ai[0] >= 120f)
				{
					num51++;
				}
				int num52 = 24;
				int num53 = 2;
				this.ai[1] -= 1f;
				bool flag15 = false;
				if (this.ai[1] <= 0f)
				{
					this.ai[1] = (float)(num52 - num53 * num51);
					flag15 = true;
					int arg_1F11_0 = (int)this.ai[0] / (num52 - num53 * num51);
				}
				bool flag16 = player.channel && player.HasAmmo(player.inventory[player.selectedItem], true) && !player.noItems && !player.CCed;
				if (this.localAI[0] > 0f)
				{
					this.localAI[0] -= 1f;
				}
				if (this.soundDelay <= 0 && flag16)
				{
					this.soundDelay = num52 - num53 * num51;
					this.localAI[0] = 12f;
				}
				player.phantasmTime = 2;
				if (flag15 && Main.myPlayer == this.owner)
				{
					int num54 = 14;
					float num55 = 14f;
					int num56 = player.inventory[player.selectedItem].damage;
					float num57 = player.inventory[player.selectedItem].knockBack;
					if (flag16)
					{
						player.PickAmmo(player.inventory[player.selectedItem], ref num54, ref num55, ref flag16, ref num56, ref num57, false);
						float num58 = player.inventory[player.selectedItem].shootSpeed * this.scale;
						Vector2 vector36 = vector;
						Vector2 vector37 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - vector36;
						if (player.gravDir == -1f)
						{
							vector37.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector36.Y;
						}
						Vector2 vector38 = Vector2.Normalize(vector37);
						if (float.IsNaN(vector38.X) || float.IsNaN(vector38.Y))
						{
							vector38 = -Vector2.UnitY;
						}
						vector38 *= num58;
						if (vector38.X != this.velocity.X || vector38.Y != this.velocity.Y)
						{
							this.netUpdate = true;
						}
						this.velocity = vector38 * 0.55f;
						for (int num59 = 0; num59 < 4; num59++)
						{
							Vector2 vector39 = Vector2.Normalize(this.velocity) * num55 * (0.6f + Main.rand.NextFloat() * 0.8f);
							if (float.IsNaN(vector39.X) || float.IsNaN(vector39.Y))
							{
								vector39 = -Vector2.UnitY;
							}
							Vector2 vector40 = vector36 + Utils.RandomVector2(Main.rand, -15f, 15f);
							Projectile.NewProjectile(vector40.X, vector40.Y, vector39.X, vector39.Y, num54, num56, num57, this.owner, 0f, 0f);
						}
					}
					else
					{
						this.Kill();
					}
				}
			}
			this.position = player.RotatedRelativePoint(player.MountedCenter, true) - base.Size / 2f;
			this.rotation = this.velocity.ToRotation() + num;
			this.spriteDirection = this.direction;
			this.timeLeft = 2;
			player.ChangeDir(this.direction);
			player.heldProj = this.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2((double)(this.velocity.Y * (float)this.direction), (double)(this.velocity.X * (float)this.direction));
			if (this.type == 460 || this.type == 611)
			{
				Vector2 vector41 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
				if (player.direction != 1)
				{
					vector41.X = (float)player.bodyFrame.Width - vector41.X;
				}
				if (player.gravDir != 1f)
				{
					vector41.Y = (float)player.bodyFrame.Height - vector41.Y;
				}
				vector41 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
				base.Center = player.RotatedRelativePoint(player.position + vector41, true) - this.velocity;
			}
			if (this.type == 615)
			{
				this.position.Y = this.position.Y + player.gravDir * 2f;
			}
		}
		private void AI_099_1()
		{
			this.timeLeft = 6;
			bool flag = true;
			float num = 250f;
			float num2 = 0.1f;
			float num3 = 15f;
			float num4 = 12f;
			num *= 0.5f;
			num3 *= 0.8f;
			num4 *= 1.5f;
			if (this.owner == Main.myPlayer)
			{
				bool flag2 = false;
				for (int i = 0; i < 1000; i++)
				{
					if (Main.projectile[i].active && Main.projectile[i].owner == this.owner && Main.projectile[i].aiStyle == 99 && (Main.projectile[i].type < 556 || Main.projectile[i].type > 561))
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					this.ai[0] = -1f;
					this.netUpdate = true;
				}
			}
			if (Main.player[this.owner].yoyoString)
			{
				num += num * 0.25f + 10f;
			}
			this.rotation += 0.5f;
			if (Main.player[this.owner].dead)
			{
				this.Kill();
				return;
			}
			if (!flag)
			{
				Main.player[this.owner].heldProj = this.whoAmI;
				Main.player[this.owner].itemAnimation = 2;
				Main.player[this.owner].itemTime = 2;
				if (this.position.X + (float)(this.width / 2) > Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2))
				{
					Main.player[this.owner].ChangeDir(1);
					this.direction = 1;
				}
				else
				{
					Main.player[this.owner].ChangeDir(-1);
					this.direction = -1;
				}
			}
			if (this.ai[0] == 0f || this.ai[0] == 1f)
			{
				if (this.ai[0] == 1f)
				{
					num *= 0.75f;
				}
				num4 *= 0.5f;
				bool flag3 = false;
				Vector2 vector = Main.player[this.owner].Center - base.Center;
				if ((double)vector.Length() > (double)num * 0.9)
				{
					flag3 = true;
				}
				if (vector.Length() > num)
				{
					float num5 = vector.Length() - num;
					Vector2 vector2;
					vector2.X = vector.Y;
					vector2.Y = vector.X;
					vector.Normalize();
					vector *= num;
					this.position = Main.player[this.owner].Center - vector;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					float num6 = this.velocity.Length();
					this.velocity.Normalize();
					if (num5 > num6 - 1f)
					{
						num5 = num6 - 1f;
					}
					this.velocity *= num6 - num5;
					num6 = this.velocity.Length();
					Vector2 vector3 = new Vector2(base.Center.X, base.Center.Y);
					Vector2 vector4 = new Vector2(Main.player[this.owner].Center.X, Main.player[this.owner].Center.Y);
					if (vector3.Y < vector4.Y)
					{
						vector2.Y = Math.Abs(vector2.Y);
					}
					else if (vector3.Y > vector4.Y)
					{
						vector2.Y = -Math.Abs(vector2.Y);
					}
					if (vector3.X < vector4.X)
					{
						vector2.X = Math.Abs(vector2.X);
					}
					else if (vector3.X > vector4.X)
					{
						vector2.X = -Math.Abs(vector2.X);
					}
					vector2.Normalize();
					vector2 *= this.velocity.Length();
					new Vector2(vector2.X, vector2.Y);
					if (Math.Abs(this.velocity.X) > Math.Abs(this.velocity.Y))
					{
						Vector2 vector5 = this.velocity;
						vector5.Y += vector2.Y;
						vector5.Normalize();
						vector5 *= this.velocity.Length();
						if ((double)Math.Abs(vector2.X) < 0.1 || (double)Math.Abs(vector2.Y) < 0.1)
						{
							this.velocity = vector5;
						}
						else
						{
							this.velocity = (vector5 + this.velocity * 2f) / 3f;
						}
					}
					else
					{
						Vector2 vector6 = this.velocity;
						vector6.X += vector2.X;
						vector6.Normalize();
						vector6 *= this.velocity.Length();
						if ((double)Math.Abs(vector2.X) < 0.2 || (double)Math.Abs(vector2.Y) < 0.2)
						{
							this.velocity = vector6;
						}
						else
						{
							this.velocity = (vector6 + this.velocity * 2f) / 3f;
						}
					}
				}
				if (Main.myPlayer == this.owner)
				{
					if (Main.player[this.owner].channel)
					{
						Vector2 vector7 = new Vector2((float)(Main.mouseX - Main.lastMouseX), (float)(Main.mouseY - Main.lastMouseY));
						if (this.velocity.X != 0f || this.velocity.Y != 0f)
						{
							if (flag)
							{
								vector7 *= -1f;
							}
							if (flag3)
							{
								if (base.Center.X < Main.player[this.owner].Center.X && vector7.X < 0f)
								{
									vector7.X = 0f;
								}
								if (base.Center.X > Main.player[this.owner].Center.X && vector7.X > 0f)
								{
									vector7.X = 0f;
								}
								if (base.Center.Y < Main.player[this.owner].Center.Y && vector7.Y < 0f)
								{
									vector7.Y = 0f;
								}
								if (base.Center.Y > Main.player[this.owner].Center.Y && vector7.Y > 0f)
								{
									vector7.Y = 0f;
								}
							}
							this.velocity += vector7 * num2;
							this.netUpdate = true;
						}
					}
					else
					{
						this.ai[0] = 10f;
						this.netUpdate = true;
					}
				}
				if (flag || this.type == 562 || this.type == 547 || this.type == 555 || this.type == 564 || this.type == 552 || this.type == 563 || this.type == 549 || this.type == 550 || this.type == 554 || this.type == 553 || this.type == 603)
				{
					float num7 = 800f;
					Vector2 vector8 = default(Vector2);
					bool flag4 = false;
					if (this.type == 549)
					{
						num7 = 200f;
					}
					if (this.type == 554)
					{
						num7 = 400f;
					}
					if (this.type == 553)
					{
						num7 = 250f;
					}
					if (this.type == 603)
					{
						num7 = 320f;
					}
					for (int j = 0; j < 200; j++)
					{
						if (Main.npc[j].CanBeChasedBy(this, false))
						{
							float num8 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
							float num9 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
							float num10 = Math.Abs(this.position.X + (float)(this.width / 2) - num8) + Math.Abs(this.position.Y + (float)(this.height / 2) - num9);
							if (num10 < num7 && (this.type != 563 || num10 >= 200f) && Collision.CanHit(this.position, this.width, this.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height) && (double)(Main.npc[j].Center - Main.player[this.owner].Center).Length() < (double)num * 0.9)
							{
								num7 = num10;
								vector8.X = num8;
								vector8.Y = num9;
								flag4 = true;
							}
						}
					}
					if (flag4)
					{
						vector8 -= base.Center;
						vector8.Normalize();
						if (this.type == 563)
						{
							vector8 *= 4f;
							this.velocity = (this.velocity * 14f + vector8) / 15f;
						}
						else if (this.type == 553)
						{
							vector8 *= 5f;
							this.velocity = (this.velocity * 12f + vector8) / 13f;
						}
						else if (this.type == 603)
						{
							vector8 *= 16f;
							this.velocity = (this.velocity * 9f + vector8) / 10f;
						}
						else if (this.type == 554)
						{
							vector8 *= 8f;
							this.velocity = (this.velocity * 6f + vector8) / 7f;
						}
						else
						{
							vector8 *= 6f;
							this.velocity = (this.velocity * 7f + vector8) / 8f;
						}
					}
				}
				if (this.velocity.Length() > num3)
				{
					this.velocity.Normalize();
					this.velocity *= num3;
				}
				if (this.velocity.Length() < num4)
				{
					this.velocity.Normalize();
					this.velocity *= num4;
					return;
				}
			}
			else
			{
				this.tileCollide = false;
				Vector2 vector9 = Main.player[this.owner].Center - base.Center;
				if (vector9.Length() < 40f || vector9.HasNaNs())
				{
					this.Kill();
					return;
				}
				float num11 = num3 * 1.5f;
				if (this.type == 546)
				{
					num11 *= 1.5f;
				}
				if (this.type == 554)
				{
					num11 *= 1.25f;
				}
				if (this.type == 555)
				{
					num11 *= 1.35f;
				}
				if (this.type == 562)
				{
					num11 *= 1.25f;
				}
				float num12 = 12f;
				vector9.Normalize();
				vector9 *= num11;
				this.velocity = (this.velocity * (num12 - 1f) + vector9) / num12;
			}
		}
		private void AI_099_2()
		{
			bool flag = false;
			for (int i = 0; i < this.whoAmI; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == this.owner && Main.projectile[i].type == this.type)
				{
					flag = true;
				}
			}
			if (this.owner == Main.myPlayer)
			{
				this.localAI[0] += 1f;
				if (flag)
				{
					this.localAI[0] += (float)Main.rand.Next(10, 31) * 0.1f;
				}
				float num = this.localAI[0] / 60f;
				num /= (1f + Main.player[this.owner].meleeSpeed) / 2f;
				if (this.type == 541 && num > 3f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 548 && num > 5f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 542 && num > 7f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 543 && num > 6f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 544 && num > 8f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 534 && num > 9f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 564 && num > 11f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 545 && num > 13f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 563 && num > 10f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 562 && num > 8f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 553 && num > 12f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 546 && num > 16f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 552 && num > 15f)
				{
					this.ai[0] = -1f;
				}
				if (this.type == 549 && num > 14f)
				{
					this.ai[0] = -1f;
				}
			}
			if (this.type == 603 && this.owner == Main.myPlayer)
			{
				this.localAI[1] += 1f;
				if (this.localAI[1] >= 6f)
				{
					float num2 = 400f;
					Vector2 vector = this.velocity;
					Vector2 vector2 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					vector2.Normalize();
					vector2 *= (float)Main.rand.Next(10, 41) * 0.1f;
					if (Main.rand.Next(3) == 0)
					{
						vector2 *= 2f;
					}
					vector *= 0.25f;
					vector += vector2;
					for (int j = 0; j < 200; j++)
					{
						if (Main.npc[j].CanBeChasedBy(this, false))
						{
							float num3 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
							float num4 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
							float num5 = Math.Abs(this.position.X + (float)(this.width / 2) - num3) + Math.Abs(this.position.Y + (float)(this.height / 2) - num4);
							if (num5 < num2 && Collision.CanHit(this.position, this.width, this.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height))
							{
								num2 = num5;
								vector.X = num3;
								vector.Y = num4;
								vector -= base.Center;
								vector.Normalize();
								vector *= 8f;
							}
						}
					}
					vector *= 0.8f;
					Projectile.NewProjectile(base.Center.X, base.Center.Y, vector.X, vector.Y, 604, this.damage, this.knockBack, this.owner, 0f, 0f);
					this.localAI[1] = 0f;
				}
			}
			bool flag2 = false;
			if (this.type >= 556 && this.type <= 561)
			{
				flag2 = true;
			}
			if (Main.player[this.owner].dead)
			{
				this.Kill();
				return;
			}
			if (!flag2 && !flag)
			{
				Main.player[this.owner].heldProj = this.whoAmI;
				Main.player[this.owner].itemAnimation = 2;
				Main.player[this.owner].itemTime = 2;
				if (this.position.X + (float)(this.width / 2) > Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2))
				{
					Main.player[this.owner].ChangeDir(1);
					this.direction = 1;
				}
				else
				{
					Main.player[this.owner].ChangeDir(-1);
					this.direction = -1;
				}
			}
			if (this.velocity.HasNaNs())
			{
				this.Kill();
			}
			this.timeLeft = 6;
			float num6 = 10f;
			float num7 = 200f;
			if (this.type == 541)
			{
				num7 = 130f;
				num6 = 9f;
			}
			else if (this.type == 548)
			{
				num7 = 170f;
				num6 = 11f;
			}
			else if (this.type == 542)
			{
				num7 = 195f;
				num6 = 12.5f;
			}
			else if (this.type == 543)
			{
				num7 = 207f;
				num6 = 12f;
			}
			else if (this.type == 544)
			{
				num7 = 215f;
				num6 = 13f;
			}
			else if (this.type == 534)
			{
				num7 = 220f;
				num6 = 13f;
			}
			else if (this.type == 564)
			{
				num7 = 225f;
				num6 = 14f;
			}
			else if (this.type == 545)
			{
				num7 = 235f;
				num6 = 14f;
			}
			else if (this.type == 562)
			{
				num7 = 235f;
				num6 = 15f;
			}
			else if (this.type == 563)
			{
				num7 = 250f;
				num6 = 12f;
			}
			else if (this.type == 546)
			{
				num7 = 275f;
				num6 = 17f;
			}
			else if (this.type == 552)
			{
				num7 = 270f;
				num6 = 14f;
			}
			else if (this.type == 553)
			{
				num7 = 275f;
				num6 = 15f;
			}
			else if (this.type == 547)
			{
				num7 = 280f;
				num6 = 17f;
			}
			else if (this.type == 549)
			{
				num7 = 290f;
				num6 = 16f;
			}
			else if (this.type == 554)
			{
				num7 = 340f;
				num6 = 16f;
			}
			else if (this.type == 550 || this.type == 551)
			{
				num7 = 370f;
				num6 = 16f;
			}
			else if (this.type == 555)
			{
				num7 = 360f;
				num6 = 16.5f;
			}
			else if (this.type == 603)
			{
				num7 = 400f;
				num6 = 17.5f;
			}
			if (Main.player[this.owner].yoyoString)
			{
				num7 = num7 * 1.25f + 30f;
			}
			num7 /= (1f + Main.player[this.owner].meleeSpeed * 3f) / 4f;
			num6 /= (1f + Main.player[this.owner].meleeSpeed * 3f) / 4f;
			float num10 = 14f - num6 / 2f;
			float num11 = 5f + num6 / 2f;
			if (flag)
			{
				num11 += 20f;
			}
			if (this.ai[0] >= 0f)
			{
				if (this.velocity.Length() > num6)
				{
					this.velocity *= 0.98f;
				}
				bool flag3 = false;
				bool flag4 = false;
				Vector2 vector3 = Main.player[this.owner].Center - base.Center;
				if (vector3.Length() > num7)
				{
					flag3 = true;
					if ((double)vector3.Length() > (double)num7 * 1.3)
					{
						flag4 = true;
					}
				}
				if (this.owner == Main.myPlayer)
				{
					if (!Main.player[this.owner].channel || Main.player[this.owner].stoned || Main.player[this.owner].frozen)
					{
						this.ai[0] = -1f;
						this.ai[1] = 0f;
						this.netUpdate = true;
					}
					else
					{
						Vector2 vector4 = Main.ReverseGravitySupport(Main.MouseScreen, 0f) + Main.screenPosition;
						float x = vector4.X;
						float y = vector4.Y;
						Vector2 vector5 = new Vector2(x, y) - Main.player[this.owner].Center;
						if (vector5.Length() > num7)
						{
							vector5.Normalize();
							vector5 *= num7;
							vector5 = Main.player[this.owner].Center + vector5;
							x = vector5.X;
							y = vector5.Y;
						}
						if (this.ai[0] != x || this.ai[1] != y)
						{
							Vector2 vector6 = new Vector2(x, y);
							Vector2 vector7 = vector6 - Main.player[this.owner].Center;
							if (vector7.Length() > num7 - 1f)
							{
								vector7.Normalize();
								vector7 *= num7 - 1f;
								vector6 = Main.player[this.owner].Center + vector7;
								x = vector6.X;
								y = vector6.Y;
							}
							this.ai[0] = x;
							this.ai[1] = y;
							this.netUpdate = true;
						}
					}
				}
				if (flag4 && this.owner == Main.myPlayer)
				{
					this.ai[0] = -1f;
					this.netUpdate = true;
				}
				if (this.ai[0] >= 0f)
				{
					if (flag3)
					{
						num10 /= 2f;
						num6 *= 2f;
						if (base.Center.X > Main.player[this.owner].Center.X && this.velocity.X > 0f)
						{
							this.velocity.X = this.velocity.X * 0.5f;
						}
						if (base.Center.Y > Main.player[this.owner].Center.Y && this.velocity.Y > 0f)
						{
							this.velocity.Y = this.velocity.Y * 0.5f;
						}
						if (base.Center.X < Main.player[this.owner].Center.X && this.velocity.X > 0f)
						{
							this.velocity.X = this.velocity.X * 0.5f;
						}
						if (base.Center.Y < Main.player[this.owner].Center.Y && this.velocity.Y > 0f)
						{
							this.velocity.Y = this.velocity.Y * 0.5f;
						}
					}
					Vector2 vector8 = new Vector2(this.ai[0], this.ai[1]);
					Vector2 vector9 = vector8 - base.Center;
					this.velocity.Length();
					float num12 = vector9.Length();
					if (vector9.Length() > num11)
					{
						vector9.Normalize();
						float num13 = (num12 > num6 * 2f) ? num6 : (num12 / 2f);
						vector9 *= num13;
						this.velocity = (this.velocity * (num10 - 1f) + vector9) / num10;
					}
					else if (flag)
					{
						if ((double)this.velocity.Length() < (double)num6 * 0.6)
						{
							vector9 = this.velocity;
							vector9.Normalize();
							vector9 *= num6 * 0.6f;
							this.velocity = (this.velocity * (num10 - 1f) + vector9) / num10;
						}
					}
					else
					{
						this.velocity *= 0.8f;
					}
					if (flag && !flag3 && (double)this.velocity.Length() < (double)num6 * 0.6)
					{
						this.velocity.Normalize();
						this.velocity *= num6 * 0.6f;
					}
				}
			}
			else
			{
				num10 = (float)((int)((double)num10 * 0.8));
				num6 *= 1.5f;
				this.tileCollide = false;
				Vector2 vector10 = Main.player[this.owner].position - base.Center;
				float num12 = vector10.Length();
				if (num12 < num6 + 10f || num12 == 0f)
				{
					this.Kill();
				}
				else
				{
					vector10.Normalize();
					vector10 *= num6;
					this.velocity = (this.velocity * (num10 - 1f) + vector10) / num10;
				}
			}
			this.rotation += 0.45f;
		}
		public void Kill()
		{
			if (!this.active)
			{
				return;
			}
			int num = this.timeLeft;
			this.timeLeft = 0;
			if (this.type == 634 || this.type == 635)
			{
				int num2 = Utils.SelectRandom<int>(Main.rand, new int[]
				{
					242,
					73,
					72,
					71,
					255
				});
				int height = 50;
				Vector2 vector = (this.rotation - 1.57079637f).ToRotationVector2();
				Vector2 vector2 = vector * this.velocity.Length() * (float)this.MaxUpdates;
				if (this.type == 635)
				{
					num2 = Utils.SelectRandom<int>(Main.rand, new int[]
					{
						242,
						59,
						88
					});
					vector2 *= 0.5f;
				}
				this.position = base.Center;
				this.width = (this.height = height);
				base.Center = this.position;
				this.maxPenetrate = -1;
				this.penetrate = -1;
				this.Damage();
			}
			else if (this.type == 651)
			{
				if (this.localAI[0] == 1f && this.owner == Main.myPlayer)
				{
					Player master = Main.player[this.owner];
					Point ps = new Vector2(this.ai[0], this.ai[1]).ToPoint();
					Point pe = base.Center.ToTileCoordinates();
					if (Main.netMode == 1)
					{
						NetMessage.SendData(109, -1, -1, "", ps.X, (float)ps.Y, (float)pe.X, (float)pe.Y, (int)WiresUI.Settings.ToolMode, 0, 0);
					}
					else
					{
						Wiring.MassWireOperation(ps, pe, master);
					}
				}
			}
			else if (this.type == 641)
			{
				if (this.owner == Main.myPlayer)
				{
					for (int k = 0; k < 1000; k++)
					{
						if (Main.projectile[k].active && Main.projectile[k].owner == this.owner && Main.projectile[k].type == 642)
						{
							Main.projectile[k].Kill();
						}
					}
				}
			}
			else if (this.type == 643)
			{
				if (this.owner == Main.myPlayer)
				{
					for (int l = 0; l < 1000; l++)
					{
						if (Main.projectile[l].active && Main.projectile[l].owner == this.owner && Main.projectile[l].type == 644)
						{
							Main.projectile[l].Kill();
						}
					}
				}
			}
			else if (this.type == 645)
			{
				bool flag = WorldGen.SolidTile(Framing.GetTileSafely((int)this.position.X / 16, (int)this.position.Y / 16));
			}
			else if (this.type == 636)
			{
				Rectangle hitbox = base.Hitbox;
				for (int num13 = 0; num13 < 6; num13 += 3)
				{
					hitbox.X = (int)this.oldPos[num13].X;
					hitbox.Y = (int)this.oldPos[num13].Y;
				}
			}
			if (this.type == 644)
			{
				Vector2 spinningpoint = new Vector2(0f, -3f).RotatedByRandom(3.1415927410125732);
				float num18 = (float)Main.rand.Next(7, 13);
				Vector2 vector3 = new Vector2(2.1f, 2f);
				Color newColor = Main.HslToRgb(this.ai[0], 1f, 0.5f);
				newColor.A = 255;
				
				if (Main.myPlayer == this.owner)
				{
					this.friendly = true;
					int width = this.width;
					int height2 = this.height;
					int num23 = this.penetrate;
					this.position = base.Center;
					this.width = (this.height = 60);
					base.Center = this.position;
					this.penetrate = -1;
					this.maxPenetrate = -1;
					this.Damage();
					this.penetrate = num23;
					this.position = base.Center;
					this.width = width;
					this.height = height2;
					base.Center = this.position;
				}
			}
			if (this.type == 608)
			{
				this.maxPenetrate = -1;
				this.penetrate = -1;
				this.Damage();
			}
			else if (this.type == 617)
			{
				this.position = base.Center;
				this.width = (this.height = 176);
				base.Center = this.position;
				this.maxPenetrate = -1;
				this.penetrate = -1;
				this.Damage();
				if (Main.myPlayer == this.owner)
				{
					for (int num44 = 0; num44 < 1000; num44++)
					{
						if (Main.projectile[num44].active && Main.projectile[num44].type == 618 && Main.projectile[num44].ai[1] == (float)this.whoAmI)
						{
							Main.projectile[num44].Kill();
						}
					}
					int num45 = Main.rand.Next(5, 9);
					int num46 = Main.rand.Next(5, 9);
					int num47 = Utils.SelectRandom<int>(Main.rand, new int[]
					{
						86,
						90
					});
					int num48 = (num47 == 86) ? 90 : 86;
					for (int num49 = 0; num49 < num45; num49++)
					{
						Vector2 vector4 = base.Center + Utils.RandomVector2(Main.rand, -30f, 30f);
						Vector2 vector5 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						while (vector5.X == 0f && vector5.Y == 0f)
						{
							vector5 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						}
						vector5.Normalize();
						if (vector5.Y > 0.2f)
						{
							vector5.Y *= -1f;
						}
						vector5 *= (float)Main.rand.Next(70, 101) * 0.1f;
						Projectile.NewProjectile(vector4.X, vector4.Y, vector5.X, vector5.Y, 620, (int)((double)this.damage * 0.8), this.knockBack * 0.8f, this.owner, (float)num47, 0f);
					}
					for (int num50 = 0; num50 < num46; num50++)
					{
						Vector2 vector6 = base.Center + Utils.RandomVector2(Main.rand, -30f, 30f);
						Vector2 vector7 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						while (vector7.X == 0f && vector7.Y == 0f)
						{
							vector7 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						}
						vector7.Normalize();
						if (vector7.Y > 0.4f)
						{
							vector7.Y *= -1f;
						}
						vector7 *= (float)Main.rand.Next(40, 81) * 0.1f;
						Projectile.NewProjectile(vector6.X, vector6.Y, vector7.X, vector7.Y, 620, (int)((double)this.damage * 0.8), this.knockBack * 0.8f, this.owner, (float)num48, 0f);
					}
				}
			}
			else if (this.type == 620 || this.type == 618)
			{
				if (this.type == 618)
				{
					this.ai[0] = 86f;
				}
			}
			else if (this.type == 619)
			{
				if (Main.myPlayer == this.owner)
				{
					int num55 = Main.rand.Next(3, 6);
					for (int num56 = 0; num56 < num55; num56++)
					{
						Vector2 vector8 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						while (vector8.X == 0f && vector8.Y == 0f)
						{
							vector8 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						}
						vector8.Normalize();
						vector8 *= (float)Main.rand.Next(70, 101) * 0.1f;
						Projectile.NewProjectile(this.oldPosition.X + (float)(this.width / 2), this.oldPosition.Y + (float)(this.height / 2), vector8.X, vector8.Y, 620, (int)((double)this.damage * 0.8), this.knockBack * 0.8f, this.owner, this.ai[0], 0f);
					}
				}
			}
			if (this.type == 601)
			{
			}
			if (this.type == 596)
			{
				this.position = base.Center;
				this.width = (this.height = 60);
				base.Center = this.position;
				int num58 = 40;
				if (Main.expertMode)
				{
					num58 = 30;
				}
				this.damage = num58;
				this.Damage();
			}
			if (this.type == 539)
			{
				this.position = base.Center;
				this.width = (this.height = 80);
				base.Center = this.position;
				this.Damage();
			}
			if (this.type == 501)
			{
				int num93 = 20;
				this.position.X = this.position.X - (float)num93;
				this.position.Y = this.position.Y - (float)num93;
				this.width += num93 * 2;
				this.height += num93 * 2;
				num93 += 20;
				this.position.X = this.position.X - (float)num93;
				this.position.Y = this.position.Y - (float)num93;
				this.width += num93 * 2;
				this.height += num93 * 2;
				this.Damage();
			}
			if (this.type == 629 && Main.netMode != 1)
			{
				int num101 = Main.npc[(int)this.ai[0]].type;
				if (num101 <= 493)
				{
					if (num101 != 422)
					{
						if (num101 == 493)
						{
							if (NPC.ShieldStrengthTowerStardust != 0)
							{
								Main.npc[(int)this.ai[0]].ai[3] = 1f;
							}
							NPC.ShieldStrengthTowerStardust = (int)MathHelper.Clamp((float)(NPC.ShieldStrengthTowerStardust - 1), 0f, (float)NPC.ShieldStrengthTowerMax);
						}
					}
					else
					{
						if (NPC.ShieldStrengthTowerVortex != 0)
						{
							Main.npc[(int)this.ai[0]].ai[3] = 1f;
						}
						NPC.ShieldStrengthTowerVortex = (int)MathHelper.Clamp((float)(NPC.ShieldStrengthTowerVortex - 1), 0f, (float)NPC.ShieldStrengthTowerMax);
					}
				}
				else if (num101 != 507)
				{
					if (num101 == 517)
					{
						if (NPC.ShieldStrengthTowerSolar != 0)
						{
							Main.npc[(int)this.ai[0]].ai[3] = 1f;
						}
						NPC.ShieldStrengthTowerSolar = (int)MathHelper.Clamp((float)(NPC.ShieldStrengthTowerSolar - 1), 0f, (float)NPC.ShieldStrengthTowerMax);
					}
				}
				else
				{
					if (NPC.ShieldStrengthTowerNebula != 0)
					{
						Main.npc[(int)this.ai[0]].ai[3] = 1f;
					}
					NPC.ShieldStrengthTowerNebula = (int)MathHelper.Clamp((float)(NPC.ShieldStrengthTowerNebula - 1), 0f, (float)NPC.ShieldStrengthTowerMax);
				}
				Main.npc[(int)this.ai[0]].netUpdate = true;
				NetMessage.SendData(101, -1, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
			}
			if (this.aiStyle == 105 && this.owner == Main.myPlayer && this.ai[1] == 0f)
			{
				Vector2 vector12 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector12.Normalize();
				vector12 *= 0.3f;
				Projectile.NewProjectile(base.Center.X, base.Center.Y, vector12.X, vector12.Y, Main.rand.Next(569, 572), this.damage, 0f, this.owner, 0f, 0f);
			}
			if (this.type == 452)
			{
				this.position = base.Center;
				this.width = (this.height = 144);
				this.position.X = this.position.X - (float)(this.width / 2);
				this.position.Y = this.position.Y - (float)(this.height / 2);
				this.Damage();
			}
			if (this.type == 454)
			{
				this.position = base.Center;
				this.width = (this.height = 208);
				this.position.X = this.position.X - (float)(this.width / 2);
				this.position.Y = this.position.Y - (float)(this.height / 2);
				this.Damage();
			}
			if (this.type == 467)
			{
				this.position = base.Center;
				this.width = (this.height = 176);
				base.Center = this.position;
				this.Damage();
			}
			if (this.type == 468)
			{
				this.position = base.Center;
				this.width = (this.height = 176);
				base.Center = this.position;
				this.Damage();
			}
			else if (this.type == 483)
			{
				if (this.owner == Main.myPlayer)
				{
					int num137 = Main.rand.Next(4, 8);
					int[] array = new int[num137];
					int num138 = 0;
					for (int num139 = 0; num139 < 200; num139++)
					{
						if (Main.npc[num139].CanBeChasedBy(this, true) && Collision.CanHitLine(this.position, this.width, this.height, Main.npc[num139].position, Main.npc[num139].width, Main.npc[num139].height))
						{
							array[num138] = num139;
							num138++;
							if (num138 == num137)
							{
								break;
							}
						}
					}
					if (num138 > 1)
					{
						for (int num140 = 0; num140 < 100; num140++)
						{
							int num141 = Main.rand.Next(num138);
							int num142;
							for (num142 = num141; num142 == num141; num142 = Main.rand.Next(num138))
							{
							}
							int num143 = array[num141];
							array[num141] = array[num142];
							array[num142] = num143;
						}
					}
					Vector2 vector13 = new Vector2(-1f, -1f);
					for (int num144 = 0; num144 < num138; num144++)
					{
						Vector2 vector14 = Main.npc[array[num144]].Center - base.Center;
						vector14.Normalize();
						vector13 += vector14;
					}
					vector13.Normalize();
					for (int num145 = 0; num145 < num137; num145++)
					{
						float num146 = (float)Main.rand.Next(8, 15);
						Vector2 vector15 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						vector15.Normalize();
						if (num138 > 0)
						{
							vector15 += vector13;
							vector15.Normalize();
						}
						vector15 *= num146;
						if (num138 > 0)
						{
							num138--;
							vector15 = Main.npc[array[num138]].Center - base.Center;
							vector15.Normalize();
							vector15 *= num146;
						}
						Projectile.NewProjectile(base.Center.X, base.Center.Y, vector15.X, vector15.Y, 484, (int)((double)this.damage * 0.7), this.knockBack * 0.7f, this.owner, 0f, 0f);
					}
				}
				if (this.owner == Main.myPlayer)
				{
					int num154 = 100;
					this.position.X = this.position.X - (float)(num154 / 2);
					this.position.Y = this.position.Y - (float)(num154 / 2);
					this.width += num154;
					this.height++;
					this.penetrate = -1;
					this.Damage();
				}
			}
			else if (this.type == 521)
			{
				if (Main.myPlayer == this.owner)
				{
					int num161 = Main.rand.Next(3, 6);
					for (int num162 = 0; num162 < num161; num162++)
					{
						Vector2 vector16 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						while (vector16.X == 0f && vector16.Y == 0f)
						{
							vector16 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						}
						vector16.Normalize();
						vector16 *= (float)Main.rand.Next(70, 101) * 0.1f;
						Projectile.NewProjectile(this.oldPosition.X + (float)(this.width / 2), this.oldPosition.Y + (float)(this.height / 2), vector16.X, vector16.Y, 522, (int)((double)this.damage * 0.8), this.knockBack * 0.8f, this.owner, 0f, 0f);
					}
				}
			}
			if (this.type == 459)
			{
				if (this.scale >= 1f)
				{
					this.position = base.Center;
					this.width = (this.height = 144);
					base.Center = this.position;
					this.Damage();
				}
			}
			if (this.owner != Main.myPlayer && this.type == 453 && Main.player[this.owner].mount.AbilityActive)
			{
				Main.player[this.owner].mount.UseAbility(Main.player[this.owner], this.position, false);
			}
			if (this.type == 441)
			{
				Main.player[this.owner].mount.StopAbilityCharge();
			}
			if (this.type == 444)
			{
				int num174 = Main.rand.Next(5, 9);
				if (this.owner == Main.myPlayer)
				{
					Vector2 vector17 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
					if (Main.player[this.owner].gravDir == -1f)
					{
						vector17.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y;
					}
					Vector2 vector18 = Vector2.Normalize(vector17 - base.Center);
					vector18 *= this.localAI[1];
					Projectile.NewProjectile(base.Center.X, base.Center.Y, vector18.X, vector18.Y, (int)this.localAI[0], this.damage, this.knockBack, this.owner, 0f, 0f);
				}
			}
			if (this.type == 639 || this.type == 640)
			{
				if (this.owner == Main.myPlayer && this.type == 639)
				{
					int num182 = num + 1;
					int nextSlot = Projectile.GetNextSlot();
					if (Main.ProjectileUpdateLoopIndex < nextSlot && Main.ProjectileUpdateLoopIndex != -1)
					{
						num182++;
					}
					Vector2 vector19 = new Vector2(this.ai[0], this.ai[1]);
					Projectile.NewProjectile(this.localAI[0], this.localAI[1], vector19.X, vector19.Y, 640, this.damage, this.knockBack, this.owner, 0f, (float)num182);
				}
			}
			if (this.type == 442)
			{
				if (Main.myPlayer == this.owner)
				{
					Rectangle rectangle = new Rectangle((int)base.Center.X - 40, (int)base.Center.Y - 40, 80, 80);
					for (int num195 = 0; num195 < 1000; num195++)
					{
						if (num195 != this.whoAmI && Main.projectile[num195].active && Main.projectile[num195].owner == this.owner && Main.projectile[num195].type == 443 && Main.projectile[num195].getRect().Intersects(rectangle))
						{
							Main.projectile[num195].ai[1] = 1f;
							Main.projectile[num195].velocity = (base.Center - Main.projectile[num195].Center) / 5f;
							Main.projectile[num195].netUpdate = true;
						}
					}
					Projectile.NewProjectile(base.Center.X, base.Center.Y, 0f, 0f, 443, this.damage, 0f, this.owner, 0f, 0f);
				}
			}
			if (this.type == 448)
			{
				this.position = base.Center;
				this.width = (this.height = 112);
				this.position.X = this.position.X - (float)(this.width / 2);
				this.position.Y = this.position.Y - (float)(this.height / 2);
				this.Damage();
			}
			if (this.type == 616)
			{
				this.position = base.Center;
				this.width = (this.height = 80);
				this.position.X = this.position.X - (float)(this.width / 2);
				this.position.Y = this.position.Y - (float)(this.height / 2);
				this.Damage();
			}
			if (this.type == 510)
			{
				if (this.owner == Main.myPlayer)
				{
					int num221 = Main.rand.Next(20, 31);
					for (int num222 = 0; num222 < num221; num222++)
					{
						Vector2 vector21 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						vector21.Normalize();
						vector21 *= (float)Main.rand.Next(10, 201) * 0.01f;
						Projectile.NewProjectile(base.Center.X, base.Center.Y, vector21.X, vector21.Y, 511 + Main.rand.Next(3), this.damage, 1f, this.owner, 0f, (float)Main.rand.Next(-45, 1));
					}
				}
			}
			if (this.type == 385)
			{
				if (this.owner == Main.myPlayer)
				{
					if (this.ai[1] < 1f)
					{
						int num230 = Main.expertMode ? 25 : 40;
						int num231 = Projectile.NewProjectile(base.Center.X - (float)(this.direction * 30), base.Center.Y - 4f, (float)(-(float)this.direction) * 0.01f, 0f, 384, num230, 4f, this.owner, 16f, 15f);
						Main.projectile[num231].netUpdate = true;
					}
					else
					{
						int num232 = (int)(base.Center.Y / 16f);
						int num233 = (int)(base.Center.X / 16f);
						int num234 = 100;
						if (num233 < 10)
						{
							num233 = 10;
						}
						if (num233 > Main.maxTilesX - 10)
						{
							num233 = Main.maxTilesX - 10;
						}
						if (num232 < 10)
						{
							num232 = 10;
						}
						if (num232 > Main.maxTilesY - num234 - 10)
						{
							num232 = Main.maxTilesY - num234 - 10;
						}
						for (int num235 = num232; num235 < num232 + num234; num235++)
						{
							Tile tile = Main.tile[num233, num235];
							if (tile.active() && (Main.tileSolid[(int)tile.type] || tile.liquid != 0))
							{
								num232 = num235;
								break;
							}
						}
						int num236 = Main.expertMode ? 50 : 80;
						int num237 = Projectile.NewProjectile((float)(num233 * 16 + 8), (float)(num232 * 16 - 24), 0f, 0f, 386, num236, 4f, Main.myPlayer, 16f, 24f);
						Main.projectile[num237].netUpdate = true;
					}
				}
			}
			else if (this.type >= 424 && this.type <= 426)
			{
				this.position.X = this.position.X + (float)(this.width / 2);
				this.position.Y = this.position.Y + (float)(this.height / 2);
				this.width = (int)(128f * this.scale);
				this.height = (int)(128f * this.scale);
				this.position.X = this.position.X - (float)(this.width / 2);
				this.position.Y = this.position.Y - (float)(this.height / 2);
				if (this.owner == Main.myPlayer)
				{
					this.localAI[1] = -1f;
					this.maxPenetrate = 0;
					this.Damage();
				}
			}
			if (this.type == 399)
			{
				
				if (Main.myPlayer == this.owner)
				{
					for (int num251 = 0; num251 < 6; num251++)
					{
						float num252 = -this.velocity.X * (float)Main.rand.Next(20, 50) * 0.01f + (float)Main.rand.Next(-20, 21) * 0.4f;
						float num253 = -Math.Abs(this.velocity.Y) * (float)Main.rand.Next(30, 50) * 0.01f + (float)Main.rand.Next(-20, 5) * 0.4f;
						Projectile.NewProjectile(base.Center.X + num252, base.Center.Y + num253, num252, num253, 400 + Main.rand.Next(3), (int)((double)this.damage * 0.5), 0f, this.owner, 0f, 0f);
					}
				}
			}
			else if (this.type == 291)
			{
				if (this.owner == Main.myPlayer)
				{
					Projectile.NewProjectile(base.Center.X, base.Center.Y, 0f, 0f, 292, this.damage, this.knockBack, this.owner, 0f, 0f);
				}
			}
			else if (this.type == 295)
			{
				if (this.owner == Main.myPlayer)
				{
					Projectile.NewProjectile(base.Center.X, base.Center.Y, 0f, 0f, 296, (int)((double)this.damage * 0.65), this.knockBack, this.owner, 0f, 0f);
				}
			}
			else if (this.type == 237 && this.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(base.Center.X, base.Center.Y, 0f, 0f, 238, this.damage, this.knockBack, this.owner, 0f, 0f);
			}
			else if (this.type == 243 && this.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(base.Center.X, base.Center.Y, 0f, 0f, 244, this.damage, this.knockBack, this.owner, 0f, 0f);
			}
			else if (this.type == 475 || this.type == 505 || this.type == 506)
			{
				if (this.ai[1] < 10f)
				{
					Vector2 position = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
					float num333 = -this.velocity.X;
					float num334 = -this.velocity.Y;
					float num335 = 1f;
					if (this.ai[0] <= 17f)
					{
						num335 = this.ai[0] / 17f;
					}
					int num336 = (int)(30f * num335);
					float num337 = 1f;
					if (this.ai[0] <= 30f)
					{
						num337 = this.ai[0] / 30f;
					}
					float num338 = 0.4f * num337;
					float num339 = num338;
					num334 += num339;
					for (int num340 = 0; num340 < num336; num340++)
					{
						float num341 = (float)Math.Sqrt((double)(num333 * num333 + num334 * num334));
						float num342 = 5.6f;
						if (Math.Abs(num333) + Math.Abs(num334) < 1f)
						{
							num342 *= Math.Abs(num333) + Math.Abs(num334) / 1f;
						}
						num341 = num342 / num341;
						num333 *= num341;
						num334 *= num341;
						Math.Atan2((double)num334, (double)num333);
						position.X += num333;
						position.Y += num334;
						num333 = -this.velocity.X;
						num334 = -this.velocity.Y;
						num339 += num338;
						num334 += num339;
					}
				}
			}
			else if (this.type == 171)
			{
				if (this.ai[1] < 10f)
				{
					Vector2 position2 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
					float num346 = -this.velocity.X;
					float num347 = -this.velocity.Y;
					float num348 = 1f;
					if (this.ai[0] <= 17f)
					{
						num348 = this.ai[0] / 17f;
					}
					int num349 = (int)(30f * num348);
					float num350 = 1f;
					if (this.ai[0] <= 30f)
					{
						num350 = this.ai[0] / 30f;
					}
					float num351 = 0.4f * num350;
					float num352 = num351;
					num347 += num352;
					for (int num353 = 0; num353 < num349; num353++)
					{
						float num354 = (float)Math.Sqrt((double)(num346 * num346 + num347 * num347));
						float num355 = 5.6f;
						if (Math.Abs(num346) + Math.Abs(num347) < 1f)
						{
							num355 *= Math.Abs(num346) + Math.Abs(num347) / 1f;
						}
						num354 = num355 / num354;
						num346 *= num354;
						num347 *= num354;
						Math.Atan2((double)num347, (double)num346);
						position2.X += num346;
						position2.Y += num347;
						num346 = -this.velocity.X;
						num347 = -this.velocity.Y;
						num352 += num351;
						num347 += num352;
					}
				}
			}
			else if (!Main.projPet[this.type])
			{
				if (this.type == 99)
				{
					for (int num372 = 0; num372 < 30; num372++)
					{
						this.velocity *= 1.9f;
					}
				}
				else if (this.type == 655)
				{
					if (Main.netMode != 1 && !this.wet)
					{
						int num376 = 2;
						if (Main.rand.Next(3) == 0)
						{
							num376++;
						}
						if (Main.rand.Next(3) == 0)
						{
							num376++;
						}
						if (Main.rand.Next(3) == 0)
						{
							num376++;
						}
						for (int num377 = 0; num377 < num376; num377++)
						{
							int num378 = Main.rand.Next(210, 212);
							int num379 = NPC.NewNPC((int)base.Center.X, (int)base.Center.Y, num378, 1, 0f, 0f, 0f, 0f, 255);
							Main.npc[num379].velocity.X = (float)Main.rand.Next(-200, 201) * 0.002f;
							Main.npc[num379].velocity.Y = (float)Main.rand.Next(-200, 201) * 0.002f;
							Main.npc[num379].netUpdate = true;
						}
						if (Main.rand.Next(4) == 0)
						{
							int num380 = NPC.NewNPC((int)base.Center.X, (int)base.Center.Y, 42, 1, 0f, 0f, 0f, 0f, 255);
							Main.npc[num380].netDefaults(-16);
							Main.npc[num380].velocity.X = (float)Main.rand.Next(-200, 201) * 0.001f;
							Main.npc[num380].velocity.Y = (float)Main.rand.Next(-200, 201) * 0.001f;
							Main.npc[num380].netUpdate = true;
						}
					}
				}
				else if (this.type == 91 || this.type == 92)
				{
					if ((this.type == 91 || (this.type == 92 && this.ai[0] > 0f)) && this.owner == Main.myPlayer)
					{
						float num378 = this.position.X + (float)Main.rand.Next(-400, 400);
						float num379 = this.position.Y - (float)Main.rand.Next(600, 900);
						Vector2 vector28 = new Vector2(num378, num379);
						float num380 = this.position.X + (float)(this.width / 2) - vector28.X;
						float num381 = this.position.Y + (float)(this.height / 2) - vector28.Y;
						int num382 = 22;
						float num383 = (float)Math.Sqrt((double)(num380 * num380 + num381 * num381));
						num383 = (float)num382 / num383;
						num380 *= num383;
						num381 *= num383;
						int num384 = this.damage;
						int num385 = Projectile.NewProjectile(num378, num379, num380, num381, 92, num384, this.knockBack, this.owner, 0f, 0f);
						if (this.type == 91)
						{
							Main.projectile[num385].ai[1] = this.position.Y;
							Main.projectile[num385].ai[0] = 1f;
						}
						else
						{
							Main.projectile[num385].ai[1] = this.position.Y;
						}
					}
				}
				else if (this.type == 89)
				{
					if (this.type == 89 && this.owner == Main.myPlayer)
					{
						for (int num388 = 0; num388 < 3; num388++)
						{
							float num389 = -this.velocity.X * (float)Main.rand.Next(40, 70) * 0.01f + (float)Main.rand.Next(-20, 21) * 0.4f;
							float num390 = -this.velocity.Y * (float)Main.rand.Next(40, 70) * 0.01f + (float)Main.rand.Next(-20, 21) * 0.4f;
							Projectile.NewProjectile(this.position.X + num389, this.position.Y + num390, num389, num390, 90, (int)((double)this.damage * 0.5), 0f, this.owner, 0f, 0f);
						}
					}
				}
				else if (this.type == 80)
				{
					int num412 = (int)this.position.X / 16;
					int num413 = (int)this.position.Y / 16;
					if (Main.tile[num412, num413] == null)
					{
						Main.tile[num412, num413] = new Tile();
					}
					if (Main.tile[num412, num413].type == 127 && Main.tile[num412, num413].active())
					{
						WorldGen.KillTile(num412, num413, false, false, false);
					}
				}
				else if (this.type == 478)
				{
					if (this.owner == Main.myPlayer)
					{
						Projectile.NewProjectile(base.Center.X, base.Center.Y, 0f, 0f, 480, (int)((double)this.damage * 0.8), this.knockBack * 0.5f, this.owner, 0f, 0f);
					}
				}
				else if (this.type == 477 || this.type == 479)
				{
					Collision.HitTiles(this.position, this.velocity, this.width, this.height);
				}
				else if (this.type == 9 || this.type == 12 || this.type == 503)
				{
					if (this.type == 503)
					{
						this.velocity /= 2f;
					}
				}
				else if (this.type == 281)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 128;
					this.height = 128;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					this.Damage();
				}
				else if (this.type == 162)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 128;
					this.height = 128;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					this.Damage();
				}
				else if (this.type == 240)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 96;
					this.height = 96;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					this.Damage();
				}
				else if (this.type == 286)
				{
					if (this.owner == Main.myPlayer)
					{
						this.localAI[1] = -1f;
						this.maxPenetrate = 0;
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 80;
						this.height = 80;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.Damage();
					}
				}
				else if (this.type == 14 || this.type == 20 || this.type == 36 || this.type == 83 || this.type == 84 || this.type == 389 || this.type == 104 || this.type == 279 || this.type == 100 || this.type == 110 || this.type == 180 || this.type == 207 || this.type == 357 || this.type == 242 || this.type == 302 || this.type == 257 || this.type == 259 || this.type == 285 || this.type == 287 || this.type == 576 || this.type == 577)
				{
					Collision.HitTiles(this.position, this.velocity, this.width, this.height);
				}
				else if (this.type == 638)
				{
					Collision.HitTiles(this.position, this.velocity, this.width, this.height);
				}
				else if (this.type == 16)
				{
					if (this.type == 16 && this.penetrate == 1)
					{
						this.maxPenetrate = -1;
						this.penetrate = -1;
						int num477 = 60;
						this.position.X = this.position.X - (float)(num477 / 2);
						this.position.Y = this.position.Y - (float)(num477 / 2);
						this.width += num477;
						this.height += num477;
						this.tileCollide = false;
						this.velocity *= 0.01f;
						this.Damage();
						this.scale = 0.01f;
					}
					this.position.X = this.position.X + (float)(this.width / 2);
					this.width = 10;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.height = 10;
					this.position.Y = this.position.Y - (float)(this.height / 2);
				}
				else if (this.type == 41)
				{
					if (this.owner == Main.myPlayer)
					{
						this.penetrate = -1;
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 64;
						this.height = 64;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.Damage();
					}
				}
				else if (this.type == 514)
				{
					if (this.owner == Main.myPlayer)
					{
						this.penetrate = -1;
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 112;
						this.height = 112;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.ai[0] = 2f;
						this.Damage();
					}
				}
				else if (this.type == 306)
				{
					if (this.owner == Main.myPlayer)
					{
						int num515 = 2;
						if (Main.rand.Next(10) == 0)
						{
							num515++;
						}
						if (Main.rand.Next(10) == 0)
						{
							num515++;
						}
						if (Main.rand.Next(10) == 0)
						{
							num515++;
						}
						for (int num516 = 0; num516 < num515; num516++)
						{
							float num517 = (float)Main.rand.Next(-35, 36) * 0.02f;
							float num518 = (float)Main.rand.Next(-35, 36) * 0.02f;
							num517 *= 10f;
							num518 *= 10f;
							Projectile.NewProjectile(this.position.X, this.position.Y, num517, num518, 307, (int)((double)this.damage * 0.7), (float)((int)((double)this.knockBack * 0.35)), Main.myPlayer, 0f, 0f);
						}
					}
				}
				else if (this.type == 469)
				{
					if (this.owner == Main.myPlayer)
					{
						int num519 = 6;
						for (int num520 = 0; num520 < num519; num520++)
						{
							if (num520 % 2 != 1 || Main.rand.Next(3) == 0)
							{
								Vector2 vector29 = this.position;
								Vector2 vector30 = this.oldVelocity;
								vector30.Normalize();
								vector30 *= 8f;
								float num521 = (float)Main.rand.Next(-35, 36) * 0.01f;
								float num522 = (float)Main.rand.Next(-35, 36) * 0.01f;
								vector29 -= vector30 * (float)num520;
								num521 += this.oldVelocity.X / 6f;
								num522 += this.oldVelocity.Y / 6f;
								int num523 = Projectile.NewProjectile(vector29.X, vector29.Y, num521, num522, Main.player[this.owner].beeType(), Main.player[this.owner].beeDamage(this.damage / 3), Main.player[this.owner].beeKB(0f), Main.myPlayer, 0f, 0f);
								Main.projectile[num523].magic = false;
								Main.projectile[num523].ranged = true;
								Main.projectile[num523].penetrate = 2;
							}
						}
					}
				}
				else if (this.type == 183)
				{
					if (this.owner == Main.myPlayer)
					{
						int num527 = Main.rand.Next(15, 25);
						for (int num528 = 0; num528 < num527; num528++)
						{
							float speedX = (float)Main.rand.Next(-35, 36) * 0.02f;
							float speedY = (float)Main.rand.Next(-35, 36) * 0.02f;
							Projectile.NewProjectile(this.position.X, this.position.Y, speedX, speedY, Main.player[this.owner].beeType(), Main.player[this.owner].beeDamage(this.damage), Main.player[this.owner].beeKB(0f), Main.myPlayer, 0f, 0f);
						}
					}
				}
				else if (this.aiStyle == 34)
				{
					if (this.owner != Main.myPlayer)
					{
						this.timeLeft = 60;
					}
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 192;
					this.height = 192;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					this.penetrate = -1;
					this.Damage();
				}
				else if (this.type == 312)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 22;
					this.height = 22;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);

					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 128;
					this.height = 128;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					this.Damage();
				}
				else if (this.type == 133 || this.type == 134 || this.type == 135 || this.type == 136 || this.type == 137 || this.type == 138 || this.type == 303 || this.type == 338 || this.type == 339)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 22;
					this.height = 22;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					
				}
				else if (this.type == 139 || this.type == 140 || this.type == 141 || this.type == 142 || this.type == 143 || this.type == 144 || this.type == 340 || this.type == 341)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 80;
					this.height = 80;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 10;
					this.height = 10;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
				}
				else if (this.type == 246)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 150;
					this.height = 150;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					this.penetrate = -1;
					this.maxPenetrate = 0;
					this.Damage();
					if (this.owner == Main.myPlayer)
					{
						int num645 = Main.rand.Next(2, 6);
						for (int num646 = 0; num646 < num645; num646++)
						{
							float num647 = (float)Main.rand.Next(-100, 101);
							num647 += 0.01f;
							float num648 = (float)Main.rand.Next(-100, 101);
							num647 -= 0.01f;
							float num649 = (float)Math.Sqrt((double)(num647 * num647 + num648 * num648));
							num649 = 8f / num649;
							num647 *= num649;
							num648 *= num649;
							int num650 = Projectile.NewProjectile(base.Center.X - this.oldVelocity.X, base.Center.Y - this.oldVelocity.Y, num647, num648, 249, this.damage, this.knockBack, this.owner, 0f, 0f);
							Main.projectile[num650].maxPenetrate = 0;
						}
					}
				}
				else if (this.type == 249)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 100;
					this.height = 100;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					this.penetrate = -1;
					this.Damage();
				}
				else if (this.type == 588)
				{
					this.position = base.Center;
					this.width = (this.height = 22);
					base.Center = this.position;
				}
				else if (this.type == 28 || this.type == 30 || this.type == 37 || this.type == 75 || this.type == 102 || this.type == 164 || this.type == 397 || this.type == 517 || this.type == 516 || this.type == 519)
				{
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 22;
					this.height = 22;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
					if (this.type == 102)
					{
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 128;
						this.height = 128;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
						this.damage = 40;
						this.Damage();
					}
				}
				else if (this.type == 29 || this.type == 108 || this.type == 470 || this.type == 637)
				{
					if (this.type == 29)
					{
						this.position.X = this.position.X + (float)(this.width / 2);
						this.position.Y = this.position.Y + (float)(this.height / 2);
						this.width = 200;
						this.height = 200;
						this.position.X = this.position.X - (float)(this.width / 2);
						this.position.Y = this.position.Y - (float)(this.height / 2);
					}
					this.position.X = this.position.X + (float)(this.width / 2);
					this.position.Y = this.position.Y + (float)(this.height / 2);
					this.width = 10;
					this.height = 10;
					this.position.X = this.position.X - (float)(this.width / 2);
					this.position.Y = this.position.Y - (float)(this.height / 2);
				}
			}
			if (this.owner == Main.myPlayer)
			{
				if (this.type == 28 || this.type == 29 || this.type == 37 || this.type == 108 || this.type == 136 || this.type == 137 || this.type == 138 || this.type == 142 || this.type == 143 || this.type == 144 || this.type == 339 || this.type == 341 || this.type == 470 || this.type == 516 || this.type == 519 || this.type == 637)
				{
					int num710 = 3;
					if (this.type == 28 || this.type == 37 || this.type == 516 || this.type == 519)
					{
						num710 = 4;
					}
					if (this.type == 29 || this.type == 470 || this.type == 637)
					{
						num710 = 7;
					}
					if (this.type == 142 || this.type == 143 || this.type == 144 || this.type == 341)
					{
						num710 = 5;
					}
					if (this.type == 108)
					{
						num710 = 10;
					}
					int num711 = (int)(this.position.X / 16f - (float)num710);
					int num712 = (int)(this.position.X / 16f + (float)num710);
					int num713 = (int)(this.position.Y / 16f - (float)num710);
					int num714 = (int)(this.position.Y / 16f + (float)num710);
					if (num711 < 0)
					{
						num711 = 0;
					}
					if (num712 > Main.maxTilesX)
					{
						num712 = Main.maxTilesX;
					}
					if (num713 < 0)
					{
						num713 = 0;
					}
					if (num714 > Main.maxTilesY)
					{
						num714 = Main.maxTilesY;
					}
					bool flag3 = false;
					for (int num715 = num711; num715 <= num712; num715++)
					{
						for (int num716 = num713; num716 <= num714; num716++)
						{
							float num717 = Math.Abs((float)num715 - this.position.X / 16f);
							float num718 = Math.Abs((float)num716 - this.position.Y / 16f);
							double num719 = Math.Sqrt((double)(num717 * num717 + num718 * num718));
							if (num719 < (double)num710 && Main.tile[num715, num716] != null && Main.tile[num715, num716].wall == 0)
							{
								flag3 = true;
								break;
							}
						}
					}
					AchievementsHelper.CurrentlyMining = true;
					for (int num720 = num711; num720 <= num712; num720++)
					{
						for (int num721 = num713; num721 <= num714; num721++)
						{
							float num722 = Math.Abs((float)num720 - this.position.X / 16f);
							float num723 = Math.Abs((float)num721 - this.position.Y / 16f);
							double num724 = Math.Sqrt((double)(num722 * num722 + num723 * num723));
							if (num724 < (double)num710)
							{
								bool flag4 = true;
								if (Main.tile[num720, num721] != null && Main.tile[num720, num721].active())
								{
									flag4 = true;
									if (Main.tileDungeon[(int)Main.tile[num720, num721].type] || Main.tile[num720, num721].type == 21 || Main.tile[num720, num721].type == 26 || Main.tile[num720, num721].type == 107 || Main.tile[num720, num721].type == 108 || Main.tile[num720, num721].type == 111 || Main.tile[num720, num721].type == 226 || Main.tile[num720, num721].type == 237 || Main.tile[num720, num721].type == 221 || Main.tile[num720, num721].type == 222 || Main.tile[num720, num721].type == 223 || Main.tile[num720, num721].type == 211 || Main.tile[num720, num721].type == 404)
									{
										flag4 = false;
									}
									if (!Main.hardMode && Main.tile[num720, num721].type == 58)
									{
										flag4 = false;
									}
									if (flag4)
									{
										WorldGen.KillTile(num720, num721, false, false, false);
										if (!Main.tile[num720, num721].active() && Main.netMode != 0)
										{
											NetMessage.SendData(17, -1, -1, "", 0, (float)num720, (float)num721, 0f, 0, 0, 0);
										}
									}
								}
								if (flag4)
								{
									for (int num725 = num720 - 1; num725 <= num720 + 1; num725++)
									{
										for (int num726 = num721 - 1; num726 <= num721 + 1; num726++)
										{
											if (Main.tile[num725, num726] != null && Main.tile[num725, num726].wall > 0 && flag3)
											{
												WorldGen.KillWall(num725, num726, false);
												if (Main.tile[num725, num726].wall == 0 && Main.netMode != 0)
												{
													NetMessage.SendData(17, -1, -1, "", 2, (float)num725, (float)num726, 0f, 0, 0, 0);
												}
											}
										}
									}
								}
							}
						}
					}
					AchievementsHelper.CurrentlyMining = false;
				}
				if (Main.netMode != 0)
				{
					NetMessage.SendData(29, -1, -1, "", this.identity, (float)this.owner, 0f, 0f, 0, 0, 0);
				}
				if (!this.noDropItem)
				{
					int num727 = -1;
					if (this.aiStyle == 10)
					{
						int num728 = (int)(this.position.X + (float)(this.width / 2)) / 16;
						int num729 = (int)(this.position.Y + (float)(this.width / 2)) / 16;
						int num730 = 0;
						int num731 = 2;
						if (this.type == 109)
						{
							num730 = 147;
							num731 = 0;
						}
						if (this.type == 31)
						{
							num730 = 53;
							num731 = 0;
						}
						if (this.type == 42)
						{
							num730 = 53;
							num731 = 0;
						}
						if (this.type == 56)
						{
							num730 = 112;
							num731 = 0;
						}
						if (this.type == 65)
						{
							num730 = 112;
							num731 = 0;
						}
						if (this.type == 67)
						{
							num730 = 116;
							num731 = 0;
						}
						if (this.type == 68)
						{
							num730 = 116;
							num731 = 0;
						}
						if (this.type == 71)
						{
							num730 = 123;
							num731 = 0;
						}
						if (this.type == 39)
						{
							num730 = 59;
							num731 = 176;
						}
						if (this.type == 40)
						{
							num730 = 57;
							num731 = 172;
						}
						if (this.type == 179)
						{
							num730 = 224;
							num731 = 0;
						}
						if (this.type == 241)
						{
							num730 = 234;
							num731 = 0;
						}
						if (this.type == 354)
						{
							num730 = 234;
							num731 = 0;
						}
						if (this.type == 411)
						{
							num730 = 330;
							num731 = 71;
						}
						if (this.type == 412)
						{
							num730 = 331;
							num731 = 72;
						}
						if (this.type == 413)
						{
							num730 = 332;
							num731 = 73;
						}
						if (this.type == 414)
						{
							num730 = 333;
							num731 = 74;
						}
						if (this.type == 109)
						{
							int num732 = (int)Player.FindClosest(this.position, this.width, this.height);
							if ((double)(base.Center - Main.player[num732].Center).Length() > (double)Main.maxScreenW * 0.75)
							{
								num730 = -1;
								num731 = 593;
							}
						}
						if (Main.tile[num728, num729].halfBrick() && this.velocity.Y > 0f && Math.Abs(this.velocity.Y) > Math.Abs(this.velocity.X))
						{
							num729--;
						}
						if (!Main.tile[num728, num729].active() && num730 >= 0)
						{
							bool flag5 = false;
							if (num729 < Main.maxTilesY - 2 && Main.tile[num728, num729 + 1] != null && Main.tile[num728, num729 + 1].active() && Main.tile[num728, num729 + 1].type == 314)
							{
								flag5 = true;
							}
							if (!flag5)
							{
								WorldGen.PlaceTile(num728, num729, num730, false, true, -1, 0);
							}
							if (!flag5 && Main.tile[num728, num729].active() && (int)Main.tile[num728, num729].type == num730)
							{
								if (Main.tile[num728, num729 + 1].halfBrick() || Main.tile[num728, num729 + 1].slope() != 0)
								{
									WorldGen.SlopeTile(num728, num729 + 1, 0);
									if (Main.netMode == 2)
									{
										NetMessage.SendData(17, -1, -1, "", 14, (float)num728, (float)(num729 + 1), 0f, 0, 0, 0);
									}
								}
								if (Main.netMode != 0)
								{
									NetMessage.SendData(17, -1, -1, "", 1, (float)num728, (float)num729, (float)num730, 0, 0, 0);
								}
							}
							else if (num731 > 0)
							{
								num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, num731, 1, false, 0, false);
							}
						}
						else if (num731 > 0)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, num731, 1, false, 0, false);
						}
					}
					if (this.type == 1 && Main.rand.Next(3) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 40, 1, false, 0, false);
					}
					if (this.type == 474 && Main.rand.Next(3) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 3003, 1, false, 0, false);
					}
					if (this.type == 103 && Main.rand.Next(6) == 0)
					{
						if (Main.rand.Next(3) == 0)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 545, 1, false, 0, false);
						}
						else
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 40, 1, false, 0, false);
						}
					}
					if (this.type == 2 && Main.rand.Next(3) == 0)
					{
						if (Main.rand.Next(3) == 0)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 41, 1, false, 0, false);
						}
						else
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 40, 1, false, 0, false);
						}
					}
					if (this.type == 172 && Main.rand.Next(3) == 0)
					{
						if (Main.rand.Next(3) == 0)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 988, 1, false, 0, false);
						}
						else
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 40, 1, false, 0, false);
						}
					}
					if (this.type == 171)
					{
						if (this.ai[1] == 0f)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 985, 1, false, 0, false);
							Main.item[num727].noGrabDelay = 0;
						}
						else if (this.ai[1] < 10f)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 965, (int)(10f - this.ai[1]), false, 0, false);
							Main.item[num727].noGrabDelay = 0;
						}
					}
					if (this.type == 475)
					{
						if (this.ai[1] == 0f)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 3005, 1, false, 0, false);
							Main.item[num727].noGrabDelay = 0;
						}
						else if (this.ai[1] < 10f)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 2996, (int)(10f - this.ai[1]), false, 0, false);
							Main.item[num727].noGrabDelay = 0;
						}
					}
					if (this.type == 505)
					{
						if (this.ai[1] == 0f)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 3079, 1, false, 0, false);
							Main.item[num727].noGrabDelay = 0;
						}
						else if (this.ai[1] < 10f)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 3077, (int)(10f - this.ai[1]), false, 0, false);
							Main.item[num727].noGrabDelay = 0;
						}
					}
					if (this.type == 506)
					{
						if (this.ai[1] == 0f)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 3080, 1, false, 0, false);
							Main.item[num727].noGrabDelay = 0;
						}
						else if (this.ai[1] < 10f)
						{
							num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 3078, (int)(10f - this.ai[1]), false, 0, false);
							Main.item[num727].noGrabDelay = 0;
						}
					}
					if (this.type == 91 && Main.rand.Next(6) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 516, 1, false, 0, false);
					}
					if (this.type == 50 && Main.rand.Next(3) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 282, 1, false, 0, false);
					}
					if (this.type == 515 && Main.rand.Next(3) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 3112, 1, false, 0, false);
					}
					if (this.type == 53 && Main.rand.Next(3) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 286, 1, false, 0, false);
					}
					if (this.type == 48 && Main.rand.Next(2) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 279, 1, false, 0, false);
					}
					if (this.type == 54 && Main.rand.Next(2) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 287, 1, false, 0, false);
					}
					if (this.type == 3 && Main.rand.Next(2) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 42, 1, false, 0, false);
					}
					if (this.type == 4 && Main.rand.Next(4) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 47, 1, false, 0, false);
					}
					if (this.type == 12 && this.damage > 500)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 75, 1, false, 0, false);
					}
					if (this.type == 155)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 859, 1, false, 0, false);
					}
					if (this.type == 598 && Main.rand.Next(4) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 3378, 1, false, 0, false);
					}
					if (this.type == 599 && Main.rand.Next(4) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 3379, 1, false, 0, false);
					}
					if (this.type == 21 && Main.rand.Next(2) == 0)
					{
						num727 = Item.NewItem((int)this.position.X, (int)this.position.Y, this.width, this.height, 154, 1, false, 0, false);
					}
					if (Main.netMode == 1 && num727 >= 0)
					{
						NetMessage.SendData(21, -1, -1, "", num727, 1f, 0f, 0f, 0, 0, 0);
					}
				}
				if (this.type == 69 || this.type == 70 || this.type == 621)
				{
					int i2 = (int)(this.position.X + (float)(this.width / 2)) / 16;
					int j2 = (int)(this.position.Y + (float)(this.height / 2)) / 16;
					if (this.type == 69)
					{
						WorldGen.Convert(i2, j2, 2, 4);
					}
					if (this.type == 70)
					{
						WorldGen.Convert(i2, j2, 1, 4);
					}
					if (this.type == 621)
					{
						WorldGen.Convert(i2, j2, 4, 4);
					}
				}
				if (this.type == 370 || this.type == 371)
				{
					float num733 = 80f;
					int num734 = 119;
					if (this.type == 371)
					{
						num734 = 120;
					}
					for (int num735 = 0; num735 < 255; num735++)
					{
						Player player = Main.player[num735];
						if (player.active && !player.dead && Vector2.Distance(base.Center, player.Center) < num733)
						{
							player.AddBuff(num734, 1800, true);
						}
					}
					for (int num736 = 0; num736 < 200; num736++)
					{
						NPC nPC = Main.npc[num736];
						if (nPC.active && nPC.life > 0 && Vector2.Distance(base.Center, nPC.Center) < num733)
						{
							nPC.AddBuff(num734, 1800, false);
						}
					}
				}
				if (this.type == 378)
				{
					int num737 = Main.rand.Next(2, 4);
					if (Main.rand.Next(5) == 0)
					{
						num737++;
					}
					for (int num738 = 0; num738 < num737; num738++)
					{
						float num739 = this.velocity.X;
						float num740 = this.velocity.Y;
						num739 *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
						num740 *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
						Projectile.NewProjectile(base.Center.X, base.Center.Y, num739, num740, 379, this.damage, this.knockBack, this.owner, 0f, 0f);
					}
				}
			}
			this.active = false;
		}
		public Color GetAlpha(Color newColor)
		{
			if (this.type == 270)
			{
				return new Color(255, 255, 255, Main.rand.Next(0, 255));
			}
			int num;
			int num2;
			int num3;
			if (this.type == 650)
			{
				num = (int)((double)newColor.R * 1.5);
				num2 = (int)((double)newColor.G * 1.5);
				num3 = (int)((double)newColor.B * 1.5);
				if (num > 255)
				{
				}
				if (num2 > 255)
				{
				}
				if (num3 > 255)
				{
				}
			}
			else
			{
				if (this.type == 604 || this.type == 631)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 0);
				}
				if (this.type == 636)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 64 - this.alpha / 4);
				}
				if (this.type == 603 || this.type == 633)
				{
					return new Color(255, 255, 255, 200);
				}
				if (this.type == 623 || (this.type >= 625 && this.type <= 628))
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 255 - this.alpha);
				}
				if (this.type == 645 || this.type == 643)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 127 - this.alpha / 2);
				}
				if (this.type == 611)
				{
					return new Color(255, 255, 255, 200);
				}
				if (this.type == 640 || this.type == 644)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 0);
				}
				if (this.type == 612)
				{
					return new Color(255, 255, 255, 127);
				}
				if (this.aiStyle == 105)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 255 - this.alpha);
				}
				if (this.type == 554)
				{
					return new Color(200, 200, 200, 200);
				}
				if (this.type == 601)
				{
					return PortalHelper.GetPortalColor(this.owner, (int)this.ai[0]);
				}
				if (this.type == 602)
				{
					Color portalColor = PortalHelper.GetPortalColor(this.owner, (int)this.ai[1]);
					portalColor.A = 227;
					return portalColor;
				}
				if (this.type == 585)
				{
					byte a = newColor.A;
					newColor = Color.Lerp(newColor, Color.White, 0.5f);
					newColor.A = a;
					return newColor;
				}
				if (this.type == 573 || this.type == 578 || this.type == 579 || this.type == 617 || this.type == 641)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 255 - this.alpha);
				}
				if (this.type == 9 || this.type == 490)
				{
					return Color.White;
				}
				if (this.type == 575 || this.type == 596)
				{
					if (this.timeLeft < 30)
					{
						float num4 = (float)this.timeLeft / 30f;
						this.alpha = (int)(255f - 255f * num4);
					}
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 128 - this.alpha / 2);
				}
				if (this.type == 546)
				{
					return new Color(255, 200, 255, 200);
				}
				if (this.type == 553)
				{
					return new Color(255, 255, 200, 200);
				}
				if (this.type == 540)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 0);
				}
				if (this.type == 498)
				{
					return new Color(255, 100, 20, 200);
				}
				if (this.type == 538)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 255 - this.alpha);
				}
				if (this.type == 518)
				{
					float num5 = 1f - (float)this.alpha / 255f;
					return new Color((int)(200f * num5), (int)(200f * num5), (int)(200f * num5), (int)(100f * num5));
				}
				if (this.type == 518 || this.type == 595)
				{
					Color color = Color.Lerp(newColor, Color.White, 0.85f);
					color.A = 128;
					return color * (1f - (float)this.alpha / 255f);
				}
				if (this.type == 536 || this.type == 607)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 63 - this.alpha / 4);
				}
				if (this.type == 591)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 63 - this.alpha / 4);
				}
				if (this.type == 493 || this.type == 494)
				{
					return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 255 - this.alpha);
				}
				if (this.type == 492)
				{
					return new Color(255, 255, 255, 255);
				}
				if (this.type == 491)
				{
					return new Color(255, 255, 255, 255);
				}
				if (this.type == 485 || this.type == 502)
				{
					return new Color(255, 255, 255, 200);
				}
				if (this.type == 488)
				{
					return new Color(255, 255, 255, 255);
				}
				if (this.type == 477 || this.type == 478 || this.type == 479)
				{
					if (this.alpha == 0)
					{
						return new Color(255, 255, 255, 200);
					}
					return new Color(0, 0, 0, 0);
				}
				else
				{
					if (this.type == 473)
					{
						return new Color(255, 255, 255, 255);
					}
					if (this.type == 50 || this.type == 53 || this.type == 515)
					{
						return new Color(255, 255, 255, 0);
					}
					if (this.type == 92)
					{
						return new Color(255, 255, 255, 0);
					}
					if (this.type == 91)
					{
						return new Color(200, 200, 200, 0);
					}
					if (this.type == 34 || this.type == 15 || this.type == 93 || this.type == 94 || this.type == 95 || this.type == 96 || this.type == 253 || this.type == 258 || (this.type == 102 && this.alpha < 255))
					{
						return new Color(200, 200, 200, 25);
					}
					if (this.type == 465)
					{
						return new Color(255, 255, 255, 0) * (1f - (float)this.alpha / 255f);
					}
					if (this.type == 503)
					{
						Color color2 = Color.Lerp(newColor, Color.White, 0.5f) * (1f - (float)this.alpha / 255f);
						Color color3 = Color.Lerp(Color.Purple, Color.White, 0.33f);
						float num6 = 0.25f + (float)Math.Cos((double)this.localAI[0]) * 0.25f;
						return Color.Lerp(color2, color3, num6);
					}
					if (this.type == 467)
					{
						return new Color(255, 255, 255, 255) * (1f - (float)this.alpha / 255f);
					}
					if (this.type == 634 || this.type == 635)
					{
						return new Color(255, 255, 255, 127) * this.Opacity;
					}
					if (this.type == 451)
					{
						return new Color(255, 255, 255, 200) * ((255f - (float)this.alpha) / 255f);
					}
					if (this.type == 454 || this.type == 452)
					{
						return new Color(255, 255, 255, 255) * (1f - (float)this.alpha / 255f);
					}
					if (this.type == 464)
					{
						return new Color(255, 255, 255, 255) * ((255f - (float)this.alpha) / 255f);
					}
					if (this.type == 450)
					{
						return new Color(200, 200, 200, 255 - this.alpha);
					}
					if (this.type == 459)
					{
						return new Color(255, 255, 255, 200);
					}
					if (this.type == 447)
					{
						return new Color(255, 255, 255, 200);
					}
					if (this.type == 446)
					{
						Color color4 = Color.Lerp(newColor, Color.White, 0.8f);
						return color4 * (1f - (float)this.alpha / 255f);
					}
					if (this.type >= 646 && this.type <= 649)
					{
						Color color5 = Color.Lerp(newColor, Color.White, 0.8f);
						return color5 * (1f - (float)this.alpha / 255f);
					}
					if (this.type == 445)
					{
						return new Color(255, 255, 255, 128) * (1f - (float)this.alpha / 255f);
					}
					if (this.type == 440 || this.type == 449 || this.type == 606)
					{
						num = 255 - this.alpha;
						num2 = 255 - this.alpha;
						num3 = 255 - this.alpha;
					}
					else
					{
						if (this.type == 444)
						{
							return newColor * (1f - (float)this.alpha / 255f);
						}
						if (this.type == 443)
						{
							return new Color(255, 255, 255, 128) * (1f - (float)this.alpha / 255f);
						}
						if (this.type == 438)
						{
							return new Color(255, 255, 255, 128) * (1f - (float)this.alpha / 255f);
						}
						if (this.type == 592)
						{
							return new Color(255, 255, 255, 128) * (1f - (float)this.alpha / 255f);
						}
						if (this.type == 437)
						{
							return new Color(255, 255, 255, 0) * (1f - (float)this.alpha / 255f);
						}
						if (this.type == 462)
						{
							return new Color(255, 255, 255, 128) * (1f - (float)this.alpha / 255f);
						}
						if (this.type == 352)
						{
							return new Color(250, 250, 250, this.alpha);
						}
						if (this.type == 435)
						{
							newColor = Color.Lerp(newColor, Color.White, 0.8f);
							return new Color((int)newColor.R, (int)newColor.G, (int)newColor.B, 25);
						}
						if (this.type == 436)
						{
							newColor = Color.Lerp(newColor, Color.White, 0.8f);
							return new Color((int)newColor.R, (int)newColor.G, (int)newColor.B, 25);
						}
						if (this.type == 409)
						{
							return new Color(250, 250, 250, 200);
						}
						if (this.type == 348 || this.type == 349)
						{
							return new Color(200, 200, 200, this.alpha);
						}
						if (this.type == 337)
						{
							return new Color(250, 250, 250, 150);
						}
						if (this.type >= 424 && this.type <= 426)
						{
							byte b = 150;
							if (newColor.R < b)
							{
								newColor.R = b;
							}
							if (newColor.G < b)
							{
								newColor.G = b;
							}
							if (newColor.B < b)
							{
								newColor.B = b;
							}
							return new Color((int)newColor.R, (int)newColor.G, (int)newColor.B, 255);
						}
						if (this.type == 431 || this.type == 432)
						{
							return new Color(250, 250, 250, 255 - this.alpha);
						}
						if (this.type == 343 || this.type == 344)
						{
							float num7 = 1f - (float)this.alpha / 255f;
							return new Color((int)(250f * num7), (int)(250f * num7), (int)(250f * num7), (int)(100f * num7));
						}
						if (this.type == 332)
						{
							return new Color(255, 255, 255, 255);
						}
						if (this.type == 329)
						{
							return new Color(200, 200, 200, 50);
						}
						if (this.type >= 326 && this.type <= 328)
						{
							return Color.Transparent;
						}
						if (this.type >= 400 && this.type <= 402)
						{
							return Color.Transparent;
						}
						if (this.type == 324 && this.frame >= 6 && this.frame <= 9)
						{
							return new Color(255, 255, 255, 255);
						}
						if (this.type == 16)
						{
							return new Color(255, 255, 255, 0);
						}
						if (this.type == 321)
						{
							return new Color(200, 200, 200, 0);
						}
						if (this.type == 76 || this.type == 77 || this.type == 78)
						{
							return new Color(255, 255, 255, 0);
						}
						if (this.type == 308)
						{
							return new Color(200, 200, 255, 125);
						}
						if (this.type == 263)
						{
							if (this.timeLeft < 255)
							{
								return new Color(255, 255, 255, (int)((byte)this.timeLeft));
							}
							return new Color(255, 255, 255, 255);
						}
						else if (this.type == 274)
						{
							if (this.timeLeft < 85)
							{
								byte b2 = (byte)(this.timeLeft * 3);
								byte b3 = (byte)(100f * ((float)b2 / 255f));
								return new Color((int)b2, (int)b2, (int)b2, (int)b3);
							}
							return new Color(255, 255, 255, 100);
						}
						else
						{
							if (this.type == 5)
							{
								return new Color(255, 255, 255, 0);
							}
							if (this.type == 300 || this.type == 301)
							{
								return new Color(250, 250, 250, 50);
							}
							if (this.type == 304)
							{
								return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, (int)((byte)((float)(255 - this.alpha) / 3f)));
							}
							if (this.type == 116 || this.type == 132 || this.type == 156 || this.type == 157 || this.type == 157 || this.type == 173)
							{
								if (this.localAI[1] >= 15f)
								{
									return new Color(255, 255, 255, this.alpha);
								}
								if (this.localAI[1] < 5f)
								{
									return Color.Transparent;
								}
								int num8 = (int)((this.localAI[1] - 5f) / 10f * 255f);
								return new Color(num8, num8, num8, num8);
							}
							else
							{
								if (this.type == 254)
								{
									if (this.timeLeft < 30)
									{
										float num9 = (float)this.timeLeft / 30f;
										this.alpha = (int)(255f - 255f * num9);
									}
									return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 0);
								}
								if (this.type == 265 || this.type == 355)
								{
									if (this.alpha > 0)
									{
										return Color.Transparent;
									}
									return new Color(255, 255, 255, 0);
								}
								else if (this.type == 270 && this.ai[0] >= 0f)
								{
									if (this.alpha > 0)
									{
										return Color.Transparent;
									}
									return new Color(255, 255, 255, 200);
								}
								else if (this.type == 257)
								{
									if (this.alpha > 200)
									{
										return Color.Transparent;
									}
									return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 0);
								}
								else if (this.type == 259)
								{
									if (this.alpha > 200)
									{
										return Color.Transparent;
									}
									return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 0);
								}
								else
								{
									if (this.type >= 150 && this.type <= 152)
									{
										return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 255 - this.alpha);
									}
									if (this.type == 250)
									{
										return Color.Transparent;
									}
									if (this.type == 251)
									{
										num = 255 - this.alpha;
										num2 = 255 - this.alpha;
										num3 = 255 - this.alpha;
										return new Color(num, num2, num3, 0);
									}
									if (this.type == 131)
									{
										return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 0);
									}
									if (this.type == 211)
									{
										return new Color(255, 255, 255, 0);
									}
									if (this.type == 229)
									{
										return new Color(255, 255, 255, 50);
									}
									if (this.type == 221)
									{
										return new Color(255, 255, 255, 200);
									}
									if (this.type == 20)
									{
										if (this.alpha <= 150)
										{
											return new Color(255, 255, 255, 0);
										}
										return new Color(0, 0, 0, 0);
									}
									else if (this.type == 207)
									{
										num = 255 - this.alpha;
										num2 = 255 - this.alpha;
										num3 = 255 - this.alpha;
									}
									else if (this.type == 242)
									{
										if (this.alpha < 140)
										{
											return new Color(255, 255, 255, 100);
										}
										return Color.Transparent;
									}
									else
									{
										if (this.type == 638)
										{
											return new Color(255, 255, 255, 100) * this.Opacity;
										}
										if (this.type == 209)
										{
											num = (int)newColor.R - this.alpha;
											num2 = (int)newColor.G - this.alpha;
											num3 = (int)newColor.B - this.alpha / 2;
										}
										else
										{
											if (this.type == 130)
											{
												return new Color(255, 255, 255, 175);
											}
											if (this.type == 182)
											{
												return new Color(255, 255, 255, 200);
											}
											if (this.type == 226)
											{
												num = 255;
												num2 = 255;
												num3 = 255;
												float num10 = (float)Main.mouseTextColor / 200f - 0.3f;
												num = (int)((float)num * num10);
												num2 = (int)((float)num2 * num10);
												num3 = (int)((float)num3 * num10);
												num += 50;
												if (num > 255)
												{
													num = 255;
												}
												num2 += 50;
												if (num2 > 255)
												{
													num2 = 255;
												}
												num3 += 50;
												if (num3 > 255)
												{
													num3 = 255;
												}
												return new Color(num, num2, num3, 200);
											}
											if (this.type == 227)
											{
												num2 = (num = (num3 = 255));
												float num11 = (float)Main.mouseTextColor / 100f - 1.6f;
												num = (int)((float)num * num11);
												num2 = (int)((float)num2 * num11);
												num3 = (int)((float)num3 * num11);
												int num12 = (int)(100f * num11);
												num += 50;
												if (num > 255)
												{
													num = 255;
												}
												num2 += 50;
												if (num2 > 255)
												{
													num2 = 255;
												}
												num3 += 50;
												if (num3 > 255)
												{
													num3 = 255;
												}
												return new Color(num, num2, num3, num12);
											}
											if (this.type == 114 || this.type == 115)
											{
												if (this.localAI[1] >= 15f)
												{
													return new Color(255, 255, 255, this.alpha);
												}
												if (this.localAI[1] < 5f)
												{
													return Color.Transparent;
												}
												int num13 = (int)((this.localAI[1] - 5f) / 10f * 255f);
												return new Color(num13, num13, num13, num13);
											}
											else if (this.type == 83 || this.type == 88 || this.type == 89 || this.type == 90 || this.type == 100 || this.type == 104 || this.type == 279 || (this.type >= 283 && this.type <= 287))
											{
												if (this.alpha < 200)
												{
													return new Color(255 - this.alpha, 255 - this.alpha, 255 - this.alpha, 0);
												}
												return Color.Transparent;
											}
											else
											{
												if (this.type == 34 || this.type == 35 || this.type == 15 || this.type == 19 || this.type == 44 || this.type == 45)
												{
													return Color.White;
												}
												if (this.type == 79)
												{
													num = Main.DiscoR;
													num2 = Main.DiscoG;
													num3 = Main.DiscoB;
													return default(Color);
												}
												if (this.type == 9 || this.type == 15 || this.type == 34 || this.type == 50 || this.type == 53 || this.type == 76 || this.type == 77 || this.type == 78 || this.type == 92 || this.type == 91)
												{
													num = (int)newColor.R - this.alpha / 3;
													num2 = (int)newColor.G - this.alpha / 3;
													num3 = (int)newColor.B - this.alpha / 3;
												}
												else
												{
													if (this.type == 18)
													{
														return new Color(255, 255, 255, 50);
													}
													if (this.type == 16 || this.type == 44 || this.type == 45)
													{
														num = (int)newColor.R;
														num2 = (int)newColor.G;
														num3 = (int)newColor.B;
													}
													else if (this.type == 12 || this.type == 72 || this.type == 86 || this.type == 87)
													{
														return new Color(255, 255, 255, (int)newColor.A - this.alpha);
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			float num14 = (float)(255 - this.alpha) / 255f;
			num = (int)((float)newColor.R * num14);
			num2 = (int)((float)newColor.G * num14);
			num3 = (int)((float)newColor.B * num14);
			int num15 = (int)newColor.A - this.alpha;
			if (num15 < 0)
			{
				num15 = 0;
			}
			if (num15 > 255)
			{
				num15 = 255;
			}
			return new Color(num, num2, num3, num15);
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"type:",
				this.type,
				"name:",
				this.name,
				", active:",
				this.active,
				", whoAmI:",
				this.whoAmI,
				", identity:",
				this.identity,
				", ai0:",
				this.ai[0],
				" , uuid:",
				this.projUUID
			});
		}
	}
}
