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
        // checks if two vanilla items are equal to one another. I'm doing it by name because literally nothing else seems to work. :(
        public static bool IsItemNamed(this Item item, string itemName)
        {
            return item.Name.Equals(itemName);
        }

        // checks if the player has a vanilla item equipped.
        public static bool IsItemEquipped(this Player player, string itemName)
        {
            foreach (var armorItem in player.armor)
            {
                if (armorItem.IsItemNamed(itemName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
