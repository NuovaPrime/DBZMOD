using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Dusts
{
	public class RadiantDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = false;
			dust.scale = 1.2f;
			dust.noGravity = true;
			dust.velocity /= 1.5f;
			dust.alpha = 100;
		}

		public override bool Update(Dust dust)
		{
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 0f, 0.20f, 0f);
			dust.scale -= 0.04f;
			if (dust.scale < 0.1f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}