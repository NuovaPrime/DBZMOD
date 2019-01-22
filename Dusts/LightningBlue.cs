using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Dusts
{
    public class LightningBlue : ModDust
    {
        private int _dustTimer;
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = true;
            //dust.color = new Color(0, 220, 230);
            dust.scale = 1.8f;
            dust.noGravity = true;
            //dust.velocity /= 2f;
            dust.alpha = 0;
        }
        public override bool Update(Dust dust)
        {
            _dustTimer++;
            if (_dustTimer > 60)
            {
                dust.active = false;
                _dustTimer = 0;
            }
            return false;
        }
    }
}