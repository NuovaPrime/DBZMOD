using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Util;

namespace DBZMOD.Items.DragonBalls
{
    public abstract class DragonBallItem : ModItem
    {
        public int WhichDragonBall = 0;
        public int? WorldDragonBallKey = null;

        // the most important thing basically.
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
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
            item.consumable = true;            
        }

        public override TagCompound Save()
        {
            var dbTagCompound = new TagCompound();
            dbTagCompound.Add("WorldDragonBallKey", WorldDragonBallKey);
            return dbTagCompound;
        }

        public override void UpdateInventory(Player player)
        {
            DoDragonBallPickupCheck(this, player);
        }

        public override bool OnPickup(Player player)
        {
            DoDragonBallPickupCheck(this, player);
            return true;
        }

        public void DoDragonBallPickupCheck(DragonBallItem item, Player player)
        {
            // first thing's first, if this is a real dragon ball, we know it's legit cos it ain't a rock, and inerts don't spawn in world.
            SetDragonBallWorldKey(item, player);

            // check to see if the dragon ball destroyed is actually the real one (the one that matches the world location)
            // if not, eat this dragon ball, the player is cheating.
            DoDragonBallLegitimacyCheck(item, player);
        }

        public void SetDragonBallWorldKey(DragonBallItem item, Player player)
        {
            if (item.item.type != DBZMOD.instance.GetItem("StoneBall").item.type)
            {
                // we already have a dragon ball key, abandon ship.
                if (item.WorldDragonBallKey > 0)
                    return;
                // it's legit, set its dragon ball key
                var world = DBZMOD.instance.GetModWorld("DBZWorld") as DBZWorld;

                item.WorldDragonBallKey = world.WorldDragonBallKey;
            }
        }

        public void DoDragonBallLegitimacyCheck(DragonBallItem item, Player player)
        {
            var dbLocation = DBZWorld.GetWorld().DragonBallLocations[item.WhichDragonBall - 1];
            // something bad has happened, don't proceed
            if (dbLocation == new Point(-1, -1))
                return;
            var dbTile = Framing.GetTileSafely(dbLocation.X, dbLocation.Y);
            var dbTileType = DBZMOD.instance.TileType(DBZWorld.GetDragonBallTileTypeFromNumber(item.WhichDragonBall));
            if (dbTile.type == dbTileType)
            {
                // the player is cheating. if we're in debug mode this is fine, but destroy that tile.
                if (DebugUtil.isDebug)
                {
                    Main.NewText("Debugged in a Dragon Ball, destroying the original.");
                    WorldGen.KillTile(dbLocation.X, dbLocation.Y, false, false, true);
                    DBZWorld.GetWorld().DragonBallLocations[item.WhichDragonBall - 1] = new Point(-1, -1);
                }
                else
                {
                    Main.NewText("Cheated Dragon Balls taste awful.");
                    ItemHelper.RemoveDragonBall(player.inventory, item.WorldDragonBallKey.Value, item.WhichDragonBall);
                }
            }
            else
            {
                // remove the dragon ball location from the world - it ain't there no more.            
                DBZWorld.GetWorld().DragonBallLocations[item.WhichDragonBall - 1] = new Point(-1, -1);
            }
        }

        public override void Load(TagCompound tag)
        {
            WorldDragonBallKey = tag.GetInt("WorldDragonBallKey");
            base.Load(tag);
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
