using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;

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
			Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
            float pointX = Main.mouseX + Main.screenPosition.X - pointPoisition.X;
            float pointY = Main.mouseY + Main.screenPosition.Y - pointPoisition.Y;
            float zoomScale = Main.screenHeight / Main.GameViewMatrix.Zoom.Y;

            float pitch = (float)Math.Sqrt(pointX * pointX + pointY * pointY);
            pitch /= zoomScale / 2f;
            if (pitch > 1f)
                pitch = 1f;

            float speedX = pointX * 0.02f; 
            float speedY = pointY * 0.02f;
            velocity = new Vector2(speedX, speedY);
 
            pitch = pitch * 2f - 1f;
            Math.Clamp(pitch, -1f, 1f);

            pitch = (float)Math.Round(pitch * (float)Player.musicNotes);
            pitch /= Player.musicNotes;


            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, pitch, 1f); 
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(0, 0);
	}
}
