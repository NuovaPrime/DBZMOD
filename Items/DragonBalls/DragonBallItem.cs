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

        public override bool OnPickup(Player player)
        {
            SetDragonBallWorldKey(this, player);
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            SetDragonBallWorldKey(this, player);
        }

        public void SetDragonBallWorldKey(DragonBallItem item, Player player)
        {
            // first thing's first, if this is a real dragon ball, we know it's legit cos it ain't a rock, and inerts don't spawn in world.
            if (item.item.type != DBZMOD.instance.GetItem("StoneBall").item.type)
            {
                // we already have a dragon ball key, abandon ship.
                if (item.WorldDragonBallKey > 0)
                    return;
                // it's legit, set its dragon ball key
                var world = DBZMOD.instance.GetModWorld("DBZWorld") as DBZWorld;                
                
                item.WorldDragonBallKey = world.WorldDragonBallKey;

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
