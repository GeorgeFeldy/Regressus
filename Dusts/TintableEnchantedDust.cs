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
			dust.velocity.Y *= 0.98f;
			dust.velocity.X *= 0.98f;

			int rand = Main.rand.Next(3);	

			if (!dust.noLightEmittence)
			{
				float light = dust.scale;
				if (rand != 0)
					light = dust.scale * 0.8f;

				if (dust.noLight)
					dust.velocity *= 0.95f;

				if (light > 1f)
					light = 1f;

				if (rand == 0)
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), light * 0.45f, light * 0.55f, light);
				else if (rand == 1)
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), light * 0.95f, light * 0.95f, light * 0.45f);
				else if (rand == 2)
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), light, light * 0.55f, light * 0.75f);
			}

			return false;

		}
       
    }
}
