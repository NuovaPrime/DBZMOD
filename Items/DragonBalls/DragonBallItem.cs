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
        public int? worldDragonBallKey = null;

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
            dbTagCompound.Add("WorldDragonBallKey", worldDragonBallKey);
            return dbTagCompound;
        }

        public override void Load(TagCompound tag)
        {
            worldDragonBallKey = tag.GetInt("WorldDragonBallKey");
            base.Load(tag);
        }

        public override void UpdateInventory(Player player)
        {
            // world isn't loaded yet.
            if (DBZWorld.GetWorld().worldDragonBallKey == 0)
                return;
            if (!RemoveThisDragonBallIfUnkeyed(player))
            {
                DoDragonBallLegitimacyCheck(player);
            }
        }

        public abstract int GetWhichDragonBall();

        public bool RemoveThisDragonBallIfUnkeyed(Player player)
        {
            if (Main.netMode == NetmodeID.Server)
                return true;
            if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI != Main.myPlayer)
                return true;
            if (!worldDragonBallKey.HasValue)
            {
                if (DebugUtil.IsDebugModeOn())
                {
                    SetDragonBallWorldKey(player);
                    return false;
                }
                worldDragonBallKey = 0;
                if (IsItemStoneBall(item.type))
                {
                    ItemHelper.RemoveStoneBall(player.inventory, 0, GetWhichDragonBall());
                    return true;
                }
                else
                {
                    ItemHelper.RemoveDragonBall(player.inventory, 0, GetWhichDragonBall());
                    return true;
                }
            }
            return false;
        }

        public override bool OnPickup(Player player)
        {
            DoDragonBallPickupCheck(player);
            return true;
        }

        public void DoDragonBallPickupCheck(Player player)
        {
            // If this ball doesn't already have a world key, make sure it's set. If it does leave it alone.
            SetDragonBallWorldKey(player);

            // check to see if the dragon ball destroyed is actually the real one (the one that matches the world location)
            // if not, eat this dragon ball, the player is cheating.
            // this also handles whether or not the ball becomes inert because it's from another world.
            DoDragonBallLegitimacyCheck( player);
        }

        public void SetDragonBallWorldKey(Player player)
        {
            if (!IsItemStoneBall(item.type))
            {
                // we already have a dragon ball key, abandon ship.
                if (worldDragonBallKey > 0)
                    return;

                // it's legit, set its dragon ball key
                var world = DBZWorld.GetWorld();

                worldDragonBallKey = world.worldDragonBallKey;
            }
            return;
        }

        public void DoDragonBallLegitimacyCheck(Player player)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI != Main.myPlayer)
                return;
            if (!worldDragonBallKey.HasValue)
                return;
            // if the keys match, check to see if this dragon ball is cheated in.
            if (DBZWorld.GetWorld().worldDragonBallKey == worldDragonBallKey.Value)
            {
                bool isStoneBallTakingPlaceOfExistingDragonBall = false;
                bool isStoneBallStale = false;
                // check if this is an inert ball that needs to be restored to its glory.
                if (IsItemStoneBall(item.type))
                {
                    if (!DBZWorld.IsDragonBallWithPlayers(worldDragonBallKey.Value))
                    {
                        ItemHelper.SwapStoneBallWithDragonBall(player.inventory, worldDragonBallKey.Value, GetWhichDragonBall());
                        isStoneBallTakingPlaceOfExistingDragonBall = true;
                    } else
                    {
                        // the player is bringing a dragon ball back legitimately, but someone else found the replacement while they were gone. Tough luck.
                        isStoneBallStale = true;
                    }
                }
                var dbLocation = DBZWorld.GetWorld().GetDragonBallLocation(GetWhichDragonBall());
                // something bad has happened, don't proceed
                if (dbLocation == new Point(-1, -1))
                    return;
                var dbTile = Framing.GetTileSafely(dbLocation.X, dbLocation.Y);
                var dbTileType = DBZMOD.instance.TileType(DBZWorld.GetDragonBallTileTypeFromNumber(GetWhichDragonBall()));
                // strange: the tile is still where the server thinks it is. this means the player probably cheated in a dragon ball.
                if (dbTile.type == dbTileType)
                {
                    // the player is cheating. if we're in debug mode this is fine, but destroy that tile.
                    if (DebugUtil.IsDebugModeOn() || isStoneBallTakingPlaceOfExistingDragonBall)
                    {
                        WorldGen.KillTile(dbLocation.X, dbLocation.Y, false, false, true);
                        return;
                    }
                    else
                    {
                        if (isStoneBallStale)
                        {
                            Main.NewText(string.Format("{0} lost the {1} Star Dragonball because they left and someone else found it.", player.name, GetWhichDragonBall()));
                        }
                        else
                        {
                            Main.NewText("Cheated Dragon Balls taste awful.");
                        }
                        ItemHelper.RemoveDragonBall(player.inventory, worldDragonBallKey.Value, GetWhichDragonBall());
                        return;
                    }
                }                
            }
            else
            {
                // the keys don't match.. what we have here is a different world's dragon ball.
                ItemHelper.SwapDragonBallWithStoneBall(player.inventory, worldDragonBallKey.Value, GetWhichDragonBall());
                return;
            }
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
        public static string GetStoneBallFromNumber(int whichDragonBall)
        {
            switch (whichDragonBall)
            {
                case 1:
                    return "OneStarStoneBall";
                case 2:
                    return "TwoStarStoneBall";
                case 3:
                    return "ThreeStarStoneBall";
                case 4:
                    return "FourStarStoneBall";
                case 5:
                    return "FiveStarStoneBall";
                case 6:
                    return "SixStarStoneBall";
                case 7:
                    return "SevenStarStoneBall";
                default:
                    return "";
            }
        }

        public static bool IsItemStoneBall(int type)
        {
            for (int ballType = 1; ballType <= 7; ballType++)
            {
                if (type == DBZMOD.instance.ItemType(GetStoneBallFromNumber(ballType)))
                    return true;
            }
            return false;
        }
    }
}
