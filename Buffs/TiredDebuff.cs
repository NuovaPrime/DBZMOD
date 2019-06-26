using Terraria;
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
            MyPlayer.ModPlayer(player).KiDamage *= 0.8f;
            if (DBZMOD.instance.thoriumLoaded)
            {
                ThoriumEffects(player);
            }
            if (DBZMOD.instance.tremorLoaded)
            {
                TremorEffects(player);
            }
            if (DBZMOD.instance.enigmaLoaded)
            {
                EnigmaEffects(player);
            }
            if (DBZMOD.instance.battlerodsLoaded)
            {
                BattleRodEffects(player);
            }
            if (DBZMOD.instance.expandedSentriesLoaded)
            {
                ExpandedSentriesEffects(player);
            }
            if (DBZMOD.instance.calamityLoaded)
            {
                CalamityEffects(player);
            }
        }
        public void ThoriumEffects(Player player)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage *= 0.8f;
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost *= 0.8f;
        }

        public void TremorEffects(Player player)
        {
            player.GetModPlayer<Tremor.MPlayer>(ModLoader.GetMod("Tremor")).alchemicalDamage *= 0.8f;
        }

        public void EnigmaEffects(Player player)
        {
            player.GetModPlayer<Laugicality.LaugicalityPlayer>(ModLoader.GetMod("Laugicality")).mysticDamage *= 0.8f;
        }

        public void BattleRodEffects(Player player)
        {
            player.GetModPlayer<UnuBattleRods.FishPlayer>(ModLoader.GetMod("UnuBattleRods")).bobberDamage *= 0.8f;
        }

        public void ExpandedSentriesEffects(Player player)
        {
            player.GetModPlayer<ExpandedSentries.ESPlayer>(ModLoader.GetMod("ExpandedSentries")).sentryDamage *= 0.8f;
        }
        public void CalamityEffects(Player player)
        {
            //player.GetModPlayer<CalamityMod.CalamityCustomThrowingDamagePlayer>(ModLoader.GetMod("CalamityMod")).throwingDamage *= 0.8f;
        }
    }
}
