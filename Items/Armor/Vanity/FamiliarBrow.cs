using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBZMOD.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Face)]
    public class FamiliarBrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Familiar Brow");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 12;
            item.value = 3000;
            item.vanity = true;
			item.accessory = true;
        }
		
		/*public override Color? GetAlpha(Color lightColor)
        {
			return ;
        }*/
    }
}