using DBZMOD.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using DBZMOD.Util;
using Terraria.ID;

namespace DBZMOD.Items.DragonBalls
{
    public abstract class DragonBallItem : ModItem
    {
        //public int? worldDragonBallKey = null;

        // the most important thing basically.
        public override bool CloneNewInstances => true;
        
        public override void RightClick(Player player)
        {
            if (ItemHelper.PlayerHasAllDragonBalls(player))
            {
                player.GetModPlayer<MyPlayer>().wishActive = !player.GetModPlayer<MyPlayer>().wishActive;
                WishMenu.menuVisible = player.GetModPlayer<MyPlayer>().wishActive;
            }
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = 0;
            item.rare = -12;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = false;            
        }

        public static string GetDragonBallItemTypeFromNumber(int whichDragonBall)
        {
            switch (whichDragonBall)
            {
                case 1:
                    return "OneStarDB";
                case 2:
                    return "TwoStarDB";
                case 3:
                    return "ThreeStarDB";
                case 4:
                    return "FourStarDB";
                case 5:
                    return "FiveStarDB";
                case 6:
                    return "SixStarDB";
                case 7:
                    return "SevenStarDB";
                default:
                    return "";
            }
        }
    }
}
