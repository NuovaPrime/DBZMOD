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

        public static void ConsumeOneItem(int type, Player player)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if (item == null)
                    continue;

                if (!item.type.Equals(type))
                    continue;
                
                if (item.stack.Equals(1))
                {
                    item.TurnToAir();
                }
                else
                {
                    item.stack -= 1;
                }
            }
        }

        public static void ConsumeKiPotion(Player player)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if (item == null)
                    continue;
                if (item.modItem == null)
                    continue;
                if (item.modItem is KiPotion)
                {
                    KiPotion potion = (KiPotion)item.modItem;
                    potion.ConsumeItem(player);
                }
            }
        }

        public static int GetItemTypeFromName(string name)
        {
            if (DBZMOD.instance.GetItem(name) != null && DBZMOD.instance.GetItem(name).item != null)
                return DBZMOD.instance.GetItem(name).item.type;

            return -1;
        }

        public static bool PlayerHasAllDragonBalls(Player player)
        {
            bool[] dragonBallsPresent = Enumerable.Repeat(false, 7).ToArray();
            for (int i = 0; i < dragonBallsPresent.Length; i++)
            {
                dragonBallsPresent[i] = InventoryContainsDragonBall(i + 1, player.inventory);
            }

            return dragonBallsPresent.All(x => x);
        }

        public static bool InventoryContainsDragonBall(int whichDragonBall, Item[] inventory)
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

        public static void DestroyPlayerDragonBalls(Player player)
        {
            List<int> dragonBallTypeAlreadyRemoved = new List<int>();
            foreach (var item in player.inventory)
            {
                if (item == null)
                    continue;
                if (item.modItem == null)
                    continue;
                if (item.modItem is DragonBallItem)
                {
                    // only remove one of each type of dragon ball. If the player has extras, leave them. Lucky them.
                    if (dragonBallTypeAlreadyRemoved.Contains(item.type))
                        continue;
                    dragonBallTypeAlreadyRemoved.Add(item.type);
                    item.TurnToAir();
                }
            }
        }
    }
}
