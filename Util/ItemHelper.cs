using System.Collections.Generic;
using System.Linq;
using DBZMOD.Items.Consumables.Potions;
using DBZMOD.Items.DragonBalls;
using Terraria;

namespace DBZMOD.Util
{
    // I built these as extension methods, so they require a using statement in classes that want to consume them outside of the DBZMOD namespace.
    public static class ItemHelper
    {
        // checks if two vanilla items are equal to one another. I'm doing it by name because literally nothing else seems to work. :(

        /// <summary>
        ///     Return an item type (int) using the name of an item.
        /// </summary>
        /// <param name="name">The internal name of the item.</param>
        public static int GetItemTypeFromName(string name)
        {
            if (DBZMOD.instance.GetItem(name) != null && DBZMOD.instance.GetItem(name).item != null)
                return DBZMOD.instance.GetItem(name).item.type;

            return -1;
        }
    }
}
