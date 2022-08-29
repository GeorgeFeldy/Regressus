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
using Regressus.Tiles.Shrine;

namespace Regressus.WorldGeneration
{
	public class ShrineBiomeSystem : ModSystem
	{
		private static List<Vector2> shrinePos;
		private static List<Vector2> shrinePosGen = new();

		private static List<int> shrineTileStyle = new();
		private static int shrineIndex = 0;


		private static List<Rectangle> shrineRectangles = new();

		public override void Load()
		{
			IL.Terraria.WorldGen.Check3x2 += ModifiyShrineLoot;
			IL.Terraria.GameContent.Biomes.EnchantedSwordBiome.Place += AddTileVariants;
			On.Terraria.GameContent.Biomes.EnchantedSwordBiome.Place += CountSuccesfulPlacements;
		}


		// adds to a list the coordinates of every succesful shrine placement 
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


			// replace the terragrim drop with enchanted sword 
			if (!c.TryGotoNext(i => i.MatchLdcI4(ItemID.Terragrim))) return;
			c.Remove();
			c.Emit(OpCodes.Ldc_I4, ItemID.EnchantedSword);

		}

		private void AddTileVariants(ILContext il)
		{
			var c = new ILCursor(il);

			if (!c.TryGotoNext(i => i.MatchLdcI4(187))) return;
			c.Remove();
			c.EmitDelegate(GetShrineType);

			if (!c.TryGotoNext(i => i.MatchLdcI4(17))) return;
			c.Remove();
			c.EmitDelegate(GetShrineStyle);
		}


		private int GetShrineType()
		{
			int type = Utils.SelectRandom<int>(WorldGen.genRand,
				//TileID.LargePiles2,
				ModContent.TileType<EnchantedTrumpetTile>()
 				//ModContent.TileType<EnchantedRemoteTile>(), 
				//ModContent.TileType<EnchantedSlingshotTile>()
				);

			if (type == TileID.LargePiles2)
				shrineTileStyle.Add(17);
			else
				shrineTileStyle.Add(0);

			return type;
		}

		private int GetShrineStyle() => shrineTileStyle[shrineIndex++];


		private bool CountSuccesfulPlacements(On.Terraria.GameContent.Biomes.EnchantedSwordBiome.orig_Place orig, EnchantedSwordBiome self, Point origin, StructureMap structures)
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
			shrineRectangles = new();
		}

		public override void PreWorldGen()
		{
			shrinePos = new();
			shrinePosGen = new();
			shrineRectangles = new();
			shrineTileStyle = new();
			shrineIndex = 0;
		}

		public override void PostWorldGen()
		{
			shrinePos = shrinePosGen;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			if (tag.TryGet("shrinePos", out shrinePos))
				ComputeShrineRectangles();
 		}

		public override void SaveWorldData(TagCompound tag)
		{
			//if (shrinePos.Count > 0)
 			tag["shrinePos"] = shrinePos;
 		}

		public static bool IsInOrNearShrine(Player player)
		{
			//Main.LocalPlayer.position = new Vector2(shrineRectangles[0].X * 16f, shrineRectangles[0].Y * 16f);

			foreach (Rectangle shrine in shrineRectangles)
			{
				// if any shrine rectangle intersects with the player's tile coordinates 
				if (shrine.Intersects(new Rectangle((int)(player.position.X / 16), (int)(player.position.Y / 16), 2, 3)))
 					return true;
 			}
			return false;
		}

		private static void ComputeShrineRectangles()
		{
			shrineRectangles = new();
			for (int i = 0; i < shrinePos.Count; i++)
			{
				// 50x50 tile coordinate rectangle around the shrine (might need a bigger one)
				shrineRectangles.Add(new Rectangle((int)(shrinePos[i].X) - 25, (int)(shrinePos[i].Y) - 25, 50, 50));
			}
		}

	}
}