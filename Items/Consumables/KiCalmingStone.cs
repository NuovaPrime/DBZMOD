using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace DBZMOD.Items.Consumables
{
    public class KiCalmingStone : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 18;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.expert = true;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ki Calming Stone");
            Tooltip.SetDefault("Deactivates Realistic Mode."
        + "\nBalances some content to fit the vanilla game better."
        + "\nTransformations get drastically lower multipliers."
        + "\nTransformations increase ki usage."
        + "\nGod level transformations are locked.");
        }


        public override bool UseItem(Player player)
        {
            DBZWorld.RealismMode = false;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (DBZWorld.RealismMode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
