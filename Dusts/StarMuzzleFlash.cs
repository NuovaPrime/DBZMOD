using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Dusts
{
	class StarMuzzleFlash : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = true;
			dust.frame = new Rectangle(0, 0, 16, 16);
			dust.alpha = 0;
		}

		public override bool Update(Dust dust)
		{
			dust.alpha += 1;
			if (dust.alpha == 5)
			{
				dust.frame = new Rectangle(0, 26, 26, 26);
			}
			if (dust.alpha == 10)
			{
				dust.frame = new Rectangle(0, 52, 26, 26);
			}
			if (dust.alpha == 15)
			{
				dust.frame = new Rectangle(0, 104, 16, 16);
			}
			if (dust.alpha > 20)
			{
				dust.active = false;
			}

			float light = 0.35f * dust.scale * ((20f - dust.alpha) / 20f);
			Lighting.AddLight(dust.position, light, light, light);

			return false;
		}
	}
}
