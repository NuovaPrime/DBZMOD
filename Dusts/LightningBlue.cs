using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Dusts
{
    public class LightningBlue : ModDust
    {
        private int DustTimer;
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = true;
            dust.color = new Color(0, 220, 230);
            dust.scale = 1.8f;
            dust.noGravity = true;
            //dust.velocity /= 2f;
            dust.alpha = 0;
        }

        public override bool Update(Dust dust)
        {
            DustTimer++;
            if (DustTimer > 60)
            {
                dust.active = false;
                DustTimer = 0;
            }
            return false;
        }
    }
}