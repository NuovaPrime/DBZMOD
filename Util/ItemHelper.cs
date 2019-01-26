using System.Linq;
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
                where item?.modItem != null
                where item.modItem is DragonBallItem
                select (DragonBallItem)item.modItem
            ).Any(
                dbItem => dbItem.item.type == ItemHelper.GetItemTypeFromName(DragonBallItem.GetDragonBallItemTypeFromNumber(whichDragonBall))
            );
        }

        ///// <summary>
        /////     Return the slot of the inventory a dragon ball was in after deleting it.
        /////     This is used to replace a dragon ball with an inert dragon ball brought from other worlds.
        ///// </summary>
        ///// <returns></returns>
        //public static int RemoveDragonBall(Item[] inventory, int dbKey, int whichDragonBall)
        //{
        //    string ballType = DragonBallItem.GetDragonBallItemTypeFromNumber(whichDragonBall);
        //    int ballTypeInt = GetItemTypeFromName(ballType);
        //    for (var i = 0; i < inventory.Length; i++)
        //    {
        //        var item = inventory[i];
        //        if (item == null)
        //            continue;

        //        if (item.modItem == null)
        //            continue;

        //        if (item.type != ballTypeInt)
        //            continue;

        //        if (item.modItem is DragonBallItem)
        //        {
        //            var dBall = item.modItem as DragonBallItem;
        //            if (dBall.worldDragonBallKey == dbKey)
        //            {
        //                inventory[i].TurnToAir();
        //                return i;
        //            }
        //        }
        //    }

        //    // dragonball wasn't found, return an oob index.
        //    return -1;
        //}

        ///// <summary>
        /////     Return the slot of the inventory a dragon ball was in after deleting it.
        /////     This is used to replace a dragon ball with an inert dragon ball brought from other worlds.
        ///// </summary>
        ///// <returns></returns>
        //public static int RemoveStoneBall(Item[] inventory, int dbKey, int whichDragonBall)
        //{
        //    string ballType = DragonBallItem.GetStoneBallFromNumber(whichDragonBall);
        //    int ballTypeInt = GetItemTypeFromName(ballType);
        //    for (var i = 0; i < inventory.Length; i++)
        //    {
        //        var item = inventory[i];
        //        if (item == null)
        //            continue;

        //        if (item.modItem == null)
        //            continue;

        //        if (item.type != ballTypeInt)
        //            continue;

        //        if (item.modItem is DragonBallItem)
        //        {
        //            var dBall = item.modItem as DragonBallItem;
        //            if (dBall.worldDragonBallKey == dbKey)
        //            {
        //                inventory[i].TurnToAir();
        //                return i;
        //            }
        //        }
        //    }

        //    // dragonball wasn't found, return an oob index.
        //    return -1;
        //}

        //public static void ScanPlayerForIllegitimateDragonBalls(Player player)
        //{
        //    ScanInventoryForIllegitimateDragonBalls(player.inventory);
        //    ScanInventoryForIllegitimateDragonBalls(player.bank.item);
        //    ScanInventoryForIllegitimateDragonBalls(player.bank2.item);
        //    ScanInventoryForIllegitimateDragonBalls(player.bank3.item);
        //}

        //public static void ScanInventoryForIllegitimateDragonBalls(Item[] inventory)
        //{
        //    for (var i = 0; i < inventory.Length; i++)
        //    {
        //        var item = inventory[i];
        //        if (item == null)
        //            continue;

        //        if (item.modItem == null)
        //            continue;

        //        if (!(item.modItem is DragonBallItem))
        //            continue;

        //        var dbItem = item.modItem as DragonBallItem;
        //        var dbWorld = DBZMOD.instance.GetModWorld("DBZWorld") as DBZWorld;
        //        if (dbItem.worldDragonBallKey != dbWorld.worldDragonBallKey)
        //        {
        //            SwapDragonBallWithStoneBall(inventory, dbItem.worldDragonBallKey.Value, dbItem.GetWhichDragonBall());
        //        }
        //        else
        //        {
        //            if (DragonBallItem.IsItemStoneBall(dbItem.item.type))
        //            {
        //                // this stone ball is being re-legitimized, which is not a word
        //                SwapStoneBallWithDragonBall(inventory, dbItem.worldDragonBallKey.Value, dbItem.GetWhichDragonBall());
        //            }
        //        }

        //    }
        //}

        //public static void SwapStoneBallWithDragonBall(Item[] inventory, int worldDragonBallKey, int whichDragonBall)
        //{
        //    // this stone ball was pulled back into its world. Turn it into a dragon ball.
        //    int dbKey = worldDragonBallKey;
        //    int dbSlot = ItemHelper.RemoveStoneBall(inventory, worldDragonBallKey, whichDragonBall);
        //    // something went wrong, abort.
        //    if (dbSlot == -1)
        //        return;
        //    var dbType = DragonBallItem.GetDragonBallItemTypeFromNumber(whichDragonBall);
        //    var newDragonBall = DBZMOD.instance.GetItem(DragonBallItem.GetDragonBallItemTypeFromNumber(whichDragonBall)).item.DeepClone();
        //    var dbData = newDragonBall.modItem as DragonBallItem;
        //    dbData.worldDragonBallKey = dbKey;
        //    inventory[dbSlot] = newDragonBall;
        //}

        //public static void SwapDragonBallWithStoneBall(Item[] inventory, int worldDragonBallKey, int whichDragonBall)
        //{
        //    // this dragon ball was pulled in from another world. Turn it into a rock.
        //    int dbKey = worldDragonBallKey;
        //    int dbSlot = ItemHelper.RemoveDragonBall(inventory, worldDragonBallKey, whichDragonBall);
        //    // something went wrong, abort.
        //    if (dbSlot == -1)
        //        return;            
        //    var newStoneBall = DBZMOD.instance.GetItem(DragonBallItem.GetStoneBallFromNumber(whichDragonBall)).item.DeepClone();
        //    var dbData = newStoneBall.modItem as DragonBallItem;
        //    dbData.worldDragonBallKey = dbKey;
        //    inventory[dbSlot] = newStoneBall;
        //}
    }
}
