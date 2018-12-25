using DBZMOD.Items.DragonBalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Util;

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
        public static int RemoveDragonBall(Item[] inventory, int dbKey, int whichDragonball)
        {
            string ballType = DragonBallItem.GetDragonBallItemTypeFromNumber(whichDragonball);
            int ballTypeInt = GetItemTypeFromName(ballType);
            for (var i = 0; i < inventory.Length; i++)
            {
                var item = inventory[i];
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
                        inventory[i].TurnToAir();
                        return i;
                    }
                }
            }

            // dragonball wasn't found, return an oob index.
            return -1;
        }

        public static void ScanPlayerForIllegitimateDragonballs(Player player)
        {
            ScanInventoryForIllegitimateDragonballs(player.inventory);
            ScanInventoryForIllegitimateDragonballs(player.bank.item);
            ScanInventoryForIllegitimateDragonballs(player.bank2.item);
            ScanInventoryForIllegitimateDragonballs(player.bank3.item);
        }

        public static void ScanInventoryForIllegitimateDragonballs(Item[] inventory)
        {
            for (var i = 0; i < inventory.Length; i++)
            {
                var item = inventory[i];
                if (item == null)
                    continue;

                if (item.modItem == null)
                    continue;

                if (!(item.modItem is DragonBallItem))
                    continue;

                var dbItem = item.modItem as DragonBallItem;
                var dbWorld = DBZMOD.instance.GetModWorld("DBZWorld") as DBZWorld;
                if (dbItem.WorldDragonBallKey != dbWorld.WorldDragonBallKey)
                {
                    SwapDragonBallWithStoneBall(inventory, dbItem.WorldDragonBallKey.Value, dbItem.WhichDragonBall);
                }
                else
                {
                    if (dbItem.item.type == GetItemTypeFromName("StoneBall"))
                    {
                        // this stone ball is being re-legitimized, which is not a word
                        SwapStoneBallWithDragonBall(inventory, dbItem.WorldDragonBallKey.Value, dbItem.WhichDragonBall);
                    }
                }

            }
        }

        public static void SwapStoneBallWithDragonBall(Item[] inventory, int WorldDragonBallKey, int WhichDragonBall)
        {
            // this stone ball was pulled back into its world. Turn it into a dragon ball.
            int dbKey = WorldDragonBallKey;
            int whichDball = WhichDragonBall;
            int dbSlot = ItemHelper.RemoveDragonBall(inventory, WorldDragonBallKey, WhichDragonBall);
            // something went wrong, abort.
            if (dbSlot == -1)
                return;
            var dbType = DragonBallItem.GetDragonBallItemTypeFromNumber(whichDball);
            var newDragonBall = DBZMOD.instance.GetItem(DragonBallItem.GetDragonBallItemTypeFromNumber(whichDball)).item.DeepClone();
            var dbData = newDragonBall.modItem as DragonBallItem;
            dbData.WorldDragonBallKey = dbKey;
            dbData.WhichDragonBall = whichDball;
            inventory[dbSlot] = newDragonBall;
        }

        public static void SwapDragonBallWithStoneBall(Item[] inventory, int WorldDragonBallKey, int WhichDragonBall)
        {
            // this dragon ball was pulled in from another world. Turn it into a rock.
            int dbKey = WorldDragonBallKey;
            int whichDball = WhichDragonBall;
            int dbSlot = ItemHelper.RemoveDragonBall(inventory, WorldDragonBallKey, WhichDragonBall);
            // something went wrong, abort.
            if (dbSlot == -1)
                return;
            var newStoneBall = DBZMOD.instance.GetItem("StoneBall").item.DeepClone();
            var dbData = newStoneBall.modItem as DragonBallItem;
            dbData.WorldDragonBallKey = dbKey;
            dbData.WhichDragonBall = whichDball;
            inventory[dbSlot] = newStoneBall;
        }
    }
}
