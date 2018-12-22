using DBZMOD.Items.DragonBalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DBZMOD
{
    // I built these as extension methods, so they require a using statement in classes that want to consume them outside of the DBZMOD namespace.
    public static class ItemHelper
    {
        // checks if two vanilla items are equal to one another. I'm doing it by name because literally nothing else seems to work. :(
        public static bool IsItemNamed(this Item item, string itemName)
        {
            return item.Name.Equals(itemName);
        }

        /// <summary>
        ///     checks if the player has a vanilla item equipped in a non-vanity slot.
        /// </summary>
        /// <param name="player">The player being checked.</param>
        /// <param name="itemName">The name of the item to check for.</param>
        /// <returns></returns>
        public static bool IsAccessoryEquipped(this Player player, string itemName)
        {
            // switched to using an index so it's easier to detect vanity slots.
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                if (player.armor[i].IsItemNamed(itemName))
                    return true;
            }
            return false;
        }

        public static int GetItemTypeFromName(string name)
        {
            if (DBZMOD.instance.GetItem(name) != null && DBZMOD.instance.GetItem(name).item != null)
                return DBZMOD.instance.GetItem(name).item.type;

            return -1;
        }

        /// <summary>
        ///     Return the slot of the inventory a dragon ball was in after deleting it.
        ///     This is used to replace a dragon ball with an inert dragon ball brought from other worlds.
        /// </summary>
        /// <returns></returns>
        public static int RemoveDragonBall(Player player, int dbKey, int whichDragonball)
        {
            string ballType = DragonBallItem.GetDragonBallItemTypeFromNumber(whichDragonball);
            int ballTypeInt = GetItemTypeFromName(ballType);
            for (var i = 0; i < player.inventory.Length; i++)
            {
                var item = player.inventory[i];
                if (item == null)
                    continue;

                if (item.modItem == null)
                    continue;
                
                if (item.type != ballTypeInt)
                    continue;

                if (item.modItem is DragonBallItem)
                {
                    var dBall = item.modItem as DragonBallItem;
                    if (dBall.WorldDragonBallKey == dbKey && dBall.WhichDragonBall == whichDragonball)
                    {
                        player.inventory[i] = null;
                        return i;
                    }
                }
            }

            // dragonball wasn't found, return an oob index.
            return -1;
        }
    }
}
