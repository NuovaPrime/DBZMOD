using System.Linq;
using DBZMOD.Items.DragonBalls;
using DBZMOD.Util;
using Terraria;

namespace DBZMOD.Extensions
{
    /// <summary>
    ///     Class housing all item extensions
    /// </summary>
    public static class ItemExtensions
    {
        public static bool IsItemNamed(this Item item, string itemName)
        {
            return item.Name.Equals(itemName);
        }

        /// <summary>
        ///     Whether a single dragon ball of a specific type is present in an inventory.
        /// </summary>
        /// <param name="inventory">The inventory being checked.</param>
        /// <param name="whichDragonBall">Which (int) dragon ball we're looking for.</param>
        /// <returns></returns>
        public static bool IsDragonBallPresent(this Item[] inventory, int whichDragonBall)
        {
            return
            (
                from item in inventory
                where item != null && item.modItem != null
                where item.modItem is DragonBallItem
                select (DragonBallItem)item.modItem
            ).Any(
                dbItem => dbItem.item.type == ItemHelper.GetItemTypeFromName(DragonBallItem.GetDragonBallItemTypeFromNumber(whichDragonBall))
            );
        }
    }
}