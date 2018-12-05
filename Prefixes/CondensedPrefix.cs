using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class CondensedPrefix : BasePrefix
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Condensed");
        }

        public override void ApplyItemModifier(Item item)
        {
            item.damage = (int)(item.damage * 1.10f);
            item.shootSpeed *= 0.85f;
        }

        public override void ApplyKiItemModifier(Item item)
        {
            //none yet?
        }
    }
}