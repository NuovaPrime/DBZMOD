using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.Potions
{
    public abstract class KiPotion : ModItem
    {
        public int potionCooldown = 3600;
        public bool isKiPotion;

        public override bool CloneNewInstances
        {
            get { return true; }
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Tooltip.SetDefault($"Restores {GetKiHealAmount()} Ki.");
        }

        public abstract int GetKiHealAmount();

        public void QuickUseItem(Player player, Item item, int quickPotionCooldown)
        {
            KiPotion potion = (KiPotion)item.modItem;
            potionCooldown = quickPotionCooldown;
            potion.UseItem(player);
        }

        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).AddKi(GetKiHealAmount(), false, false);
            player.AddBuff(mod.BuffType("KiPotionSickness"), potionCooldown);
            CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), GetKiHealAmount(), false, false);
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(mod.BuffType("KiPotionSickness"));
        }
    }
}