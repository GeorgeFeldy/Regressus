using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Regressus.Items.Weapons.Magic
{
    public class Vision : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates a fragment of a dark future.");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 90;
            Item.crit = 45;
            Item.damage = 145;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.mana = 5;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20;
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 20.5f;
            Item.shoot = ModContent.ProjectileType<VisionP>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltipLine = tooltips.Find((TooltipLine x) => x.Name == "ItemName");
            tooltipLine.overrideColor = new Color(148, 0, 209, Main.DiscoB);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //for (int i = -1; i < 2; i++)
            Projectile.NewProjectile(source, position, /*Utils.RotatedBy(velocity, (double)(MathHelper.ToRadians(16f) * (float)i))*/ velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
