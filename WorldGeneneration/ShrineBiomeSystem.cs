using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.GameContent.Biomes;
using Regressus.Items.Weapons.Magic;
using Regressus.Items.Weapons.Summon;
using Regressus.Items.Weapons.Ranged;

namespace Regressus.WorldGeneration
{
	public class ShrineBiomeSystem : ModSystem
	{
		private static List<Vector2> shrinePos;
		private static List<Vector2> shrinePosGen = new();

		private static List<Rectangle> shrineRectangles = new();

		public override void Load()
		{
			IL.Terraria.WorldGen.Check3x2 += ModifiyShrineLoot; 
			On.Terraria.GameContent.Biomes.EnchantedSwordBiome.Place += HookCountShrines;
		}

		private void ModifiyShrineLoot(ILContext il)
		{
			var c = new ILCursor(il);

			// match frameX >= 918
			if (!c.TryGotoNext(
				i => i.MatchLdloc(10),  // loading of tile.frameX (local var 10)
				i => i.MatchLdcI4(918)  // start frame coords of enchanted sword tile
			)) return;

			// match frameX <= 970
			if (!c.TryGotoNext(
				i => i.MatchLdloc(10),  // loading of tile.frameX (local var 10)
				i => i.MatchLdcI4(970)  // end frame coords of enchanted sword tile
			)) return;

			// gives Terragrim a 1/2^31 chance to drop (easier than removing the branch altogether)
			// "The chance of getting the Terragrim are low.. but never zero" 
			if (!c.TryGotoNext(i => i.MatchLdcI4(50))) return;
			c.Remove();
			c.Emit(OpCodes.Ldc_I4, int.MaxValue);


			if (!c.TryGotoNext(i => i.MatchLdcI4(ItemID.EnchantedSword))) return;
			c.Remove();
			c.EmitDelegate(GetShrineLootPool);
		}

		private int GetShrineLootPool()
		{
			return ModContent.ItemType<EnchantedTrumpet>();
		}

		// adds to a list the coordinates of every succesful shrine placement 
		private static bool HookCountShrines(On.Terraria.GameContent.Biomes.EnchantedSwordBiome.orig_Place orig, EnchantedSwordBiome self, Point origin, StructureMap structures)
		{
			if (orig(self, origin, structures))
			{
				shrinePosGen.Add(new Vector2(origin.X, origin.Y));
				return true;
			}

			return false;
		}

		public override void OnWorldLoad()
		{
			shrinePos = new();
		}

		public override void PostWorldGen()
		{
			shrinePos = shrinePosGen;

			// must reset this in the case of user generating multiple worlds 
			shrinePosGen = new();
		}

		public override void LoadWorldData(TagCompound tag)
		{
			if (tag.TryGet("shrinePos", out shrinePos))
			{
				ComputeShrineRectangles();
			}
		}

		public override void SaveWorldData(TagCompound tag)
		{
			if (shrinePos.Count > 0)
			{
				tag["shrinePos"] = shrinePos;
			}
		}

		public static bool IsInOrNearShrine(Player player)
		{
			for (int i = 0; i < shrineRectangles.Count; i++)
			{
				// if any shrine rectangle intersects with the player's tile coordinates 
				if (shrineRectangles[i].Intersects(new Rectangle((int)(player.position.X / 16), (int)(player.position.Y / 16), 2, 3)))
				{
					return true;
				}
			}

			return false;
		}

		private void ComputeShrineRectangles()
		{
			for (int i = 0; i < shrinePos.Count; i++)
			{
				// 50x50 tile coordinate rectangle around the shrine (might need a bigger one)
				shrineRectangles.Add(new Rectangle((int)(shrinePos[i].X) - 25, (int)(shrinePos[i].Y) - 25, 50, 50));
			}
		}

	}
}