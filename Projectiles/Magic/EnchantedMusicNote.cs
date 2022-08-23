using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Magic;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Magic
{
	public class EnchantedMusicNote : ModProjectile
	{
		public enum NoteType
		{
			Quarter,
			Eighth,
			TiedEighth
		}

		// Taken from spawned dusts colors 
		private readonly Color[] Colors = new Color[6]
		{
			new Color(129, 11, 116),  // Dark Purple
			new Color(247, 143, 185), // Pink
			new Color(96, 144, 143),  // Teal
			new Color(143, 196, 247), // Light Blue
			new Color(131, 115, 11),  // Dark Yellow
			new Color(141, 144, 96)   // Olive Green
		};

		Color CurrentColor
		{
			get => new() { PackedValue = (uint)Projectile.localAI[0] };
			set => Projectile.localAI[0] = (float)value.PackedValue;
		}

		Color NextColor
		{
			get => new() { PackedValue = (uint)Projectile.localAI[1] };
			set => Projectile.localAI[1] = (float)value.PackedValue;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Musical Note");
			Main.projFrames[Type] = 3;
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;

			int type = Main.rand.Next(Main.projFrames[Type]);
			Projectile.frame = type;

			CurrentColor = Colors[Main.rand.Next(Colors.Length)];
			GetNextColor();

			switch ((NoteType)type)
			{
				case NoteType.Quarter:
					Projectile.width = 10;
					Projectile.height = 22;
					break;
				case NoteType.Eighth:
					Projectile.width = 18;
					Projectile.height = 24;
					break;
				case NoteType.TiedEighth:
					Projectile.width = 22;
					Projectile.height = 24;
					break;
			}

			Projectile.netUpdate = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 22;
			Projectile.aiStyle = ProjAIStyleID.MusicNote; // needed for bouncing 
			Projectile.friendly = true;
			Projectile.alpha = 100;
			Projectile.light = 0.3f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 360; // was 180
			Projectile.DamageType = DamageClass.Magic;
		}

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.X * 0.1f;
			Projectile.spriteDirection = -Projectile.direction;

			if (Projectile.ai[1] == 1f)
			{
				Projectile.ai[1] = 0f;
				Main.musicPitch = Projectile.ai[0];
				SoundEngine.PlaySound(SoundID.Item26, Projectile.position);
			}

			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, GetEnchantedDustType() , 0f, 0f, 80);
				dust.noGravity = true;
				dust.velocity *= 0.2f;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, GetEnchantedDustType(), 0f, 0f, 80, default, 1.5f);
				Main.dust[dust].noGravity = true;
			}
		}


		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			SpriteEffects effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(1, 3, frameY: Projectile.frame), GetDrawColor(lightColor), Projectile.rotation, Projectile.Size / 2, 1f, effect, 0);

			return false;
		}

		private Color GetDrawColor(Color lightColor)
		{
			float cyclePeriod = 20f;
			float cycle = (float)(Main.timeForVisualEffects % cyclePeriod / cyclePeriod);
			Color color = Color.Lerp(CurrentColor, NextColor, cycle);

			if (cycle >= 0.95f)
			{
				CurrentColor = NextColor;
				GetNextColor();
			}

			color.A = (byte)Projectile.alpha;
			return color;
		}

		private void GetNextColor() 
		{ 
			do 
			{ 
				NextColor = Colors[Main.rand.Next(Colors.Length)]; 
			} 
			while (CurrentColor == NextColor);
		}

		private int GetEnchantedDustType() => Main.rand.Next(3) switch
												{
													0 => 15, // Magic dust 
													1 => 57, // Fallen star dust 1 
													_ => 58  // Fallen star dust 2
												}; 
	}
}


