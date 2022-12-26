using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Regressus.Dusts
{
    public class TintableEnchantedDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
 
        }

        public override bool Update(Dust dust)
        {
			dust.velocity *= 0.98f;
			int rand = Main.rand.Next(3);

			dust.scale *= 0.95f;
 
			if (dust.scale < 0.2f)
				dust.active = false;

            float r = dust.color.R / 255;
            float g = dust.color.G / 255;
            float b = dust.color.B / 255;

            Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), r, g, b);


			return false;

		}
       
    }
}
