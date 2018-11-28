using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD
{
    // I built these as extension methods, so they require a using statement in classes that want to consume them outside of the DBZMOD namespace.
    public static class ItemHelper
    {

        // checks if a mod item is equal to a vanilla item. Supported by vanilla item method.
        public static bool IsItemEqual(this ModItem item, Item comparedToItem)
        {
            return item.item.IsItemEqual(comparedToItem);
        }

        // checks if two vanilla items are equal to one another.
        public static bool IsItemEqual(this Item item, Item comparedToItem)
        {
            return item.netID == comparedToItem.netID;
        }

        // checks if the player has a mod item equipped. Supported by vanilla item method.
        public static bool IsItemEquipped(this Player player, ModItem item)
        {
            return player.IsItemEquipped(item.item);
        }

        // checks if the player has a vanilla item equipped.
        public static bool IsItemEquipped(this Player player, Item item)
        {
            foreach (var armorItem in player.armor)
            {
                if (armorItem.IsItemEqual(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
