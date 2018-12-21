using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DBZMOD.Items.DragonBalls
{
    public abstract class DragonBallItem : ModItem
    {

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
            dbTagCompound.Add("WorldDragonBallKey", DBZWorld.WorldDragonBallKey);
            return dbTagCompound;
        }

        public override void Load(TagCompound tag)
        {
            var dbKey = tag.GetInt("WorldDragonBallKey");
            if (dbKey != DBZWorld.WorldDragonBallKey)
            {
                // this dragon ball was pulled in from another world. Turn it into a rock.
            }
            base.Load(tag);
        }
    }
}
