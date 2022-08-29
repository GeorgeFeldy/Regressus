using Microsoft.Xna.Framework;
using Regressus.Items.Weapons.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Regressus.Tiles.Shrine
{
    public class EnchantedTrumpetTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Origin = new Point16(0, 2);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            DustType = DustID.Stone;
            HitSound = SoundID.Dig;

            AddMapEntry(new Color(64, 64, 64), Language.GetText("Enchanted Slingshot"));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<EnchantedTrumpet>());
        }

    }

}
