using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Magic;
using Terraria.GameContent;

namespace Regressus.Projectiles.Magic
{
    public class VisionP : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetDefaults()
        {
            Projectile.height = 65;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 65;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        Vector2 mousePos;
        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 40, 40, ModContent.DustType<Dusts.TestDust>(), 0, 0, 0, default, 1.75f);
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 45 && Projectile.ai[1] == 0)
            {
                for (int i = -1; i < 2; i++)
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Utils.RotatedBy(Projectile.velocity, (double)(MathHelper.ToRadians(16f) * (float)i)), ModContent.ProjectileType<VisionP>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 1);
            }
            /*if (Projectile.velocity != Vector2.Zero)
                d.velocity = Vector2.Zero;
            else
                d.velocity = new Vector2(0, -4);*/
        }
    }
}
