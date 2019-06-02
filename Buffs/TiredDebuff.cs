﻿using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
    public class TiredDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Exhausted");
            Description.SetDefault("You have used too much Ki.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage *= 0.8f;
            player.rangedDamage *= 0.8f;
            player.magicDamage *= 0.8f;
            player.minionDamage *= 0.8f;
            player.thrownDamage *= 0.8f;
            MyPlayer.ModPlayer(player).kiDamage *= 0.8f;
            if (DBZMOD.Instance.thoriumLoaded)
            {
                ThoriumEffects(player);
            }
        }
        public void ThoriumEffects(Player player)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage *= 0.8f;
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost *= 0.8f;
        }
        public void EnigmaEffects(Player player)
        {
            player.GetModPlayer<Laugicality.LaugicalityPlayer>(ModLoader.GetMod("Laugicality")).MysticDamage *= 0.8f;
        }
        public void BattleRodEffects(Player player)
        {
            player.GetModPlayer<UnuBattleRods.FishPlayer>(ModLoader.GetMod("UnuBattleRods")).bobberDamage *= 0.8f;
        }
    }
}
