using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Regressus.Items.Weapons.Magic
{
    public class EnchantedTrumpet : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 28;
            Item.damage = 18;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.mana = 5;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 4.5f;
            Item.shoot = ModContent.ProjectileType<EnchantedMusicNote>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // TODO:
            //  add here speed modifier based on mouse position
            //  pass pitch modifier to proj.ai[0]
            //  pass 1f to proj.ai[1] to play sound
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI); 
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(0, 0);
	}
}
