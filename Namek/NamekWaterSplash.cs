using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Namek
{
    public class NamekWaterSplash : ModDust
    {
        public override void SetDefaults()
        {
            updateType = 33;
        }

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 170; //this is the dust visibiliti, the bigger is the value less visible
            dust.velocity *= 0.5f;  //this is the velocity of dust
            dust.velocity.Y += 1f;  //and this is the velocity of dust when it goes up
        }
    }
}