using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using DBZMOD;
using Terraria;

namespace DBZMOD
{
    public abstract class TransBuff : ModBuff
    {
        public float DamageMulti;
        public float SpeedMulti;
        public float KaioLightValue;
        public float KiDrainBuffMulti;
        public bool IsKaioken;
        public float SSJLightValue;
        public bool IsSSJ;
        public int HealthDrainRate;
        public int KiDrainRate;
        private int KiDrainTimer;
        public bool RealismModeOn;
        public override void Update(Player player, ref int buffIndex)
        {
            if(DBZWorld.RealismMode)
            {
                RealismModeOn = true;
            }
            else
            {
                RealismModeOn = false;
                KiDrainAdd(player);
            }
            if(IsKaioken)
            {
                MyPlayer.ModPlayer(player).hasKaioken = true;
                Lighting.AddLight(player.Center, KaioLightValue, 0f, 0f);
            }
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
            player.lifeRegenTime = 0;
            player.lifeRegen -= HealthDrainRate;
            if (IsSSJ)
            {
                MyPlayer.ModPlayer(player).IsTransformed = true;
                KiDrainTimer++;
                if(KiDrainTimer > 1)
                {
                    MyPlayer.ModPlayer(player).KiCurrent -= KiDrainRate;
                    KiDrainTimer = 0;
                }
                Lighting.AddLight(player.Center, 1f, 1f, 0f);
            }
            if (MyPlayer.ModPlayer(player).speedToggled)
            {
                player.moveSpeed += SpeedMulti;
                player.maxRunSpeed += SpeedMulti;
                player.runAcceleration += SpeedMulti;
            }
            else if (!MyPlayer.ModPlayer(player).speedToggled)
            {
                player.moveSpeed += 2f;
                player.maxRunSpeed += 2f;
                player.runAcceleration += 2f;
            }
            player.meleeDamage += DamageMulti;
            player.rangedDamage += DamageMulti;
            player.magicDamage += DamageMulti;
            player.minionDamage += DamageMulti;
            player.thrownDamage += DamageMulti;
            MyPlayer.ModPlayer(player).KiDamage += DamageMulti;
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
            if(IsSSJ)
            {
                if(MyPlayer.ModPlayer(player).KiCurrent <= 0)
                {
                    player.ClearBuff(mod.BuffType("SSJ1Buff"));
                    player.ClearBuff(mod.BuffType("ASSJBuff"));
                    player.ClearBuff(mod.BuffType("USSJBuff"));
                    player.ClearBuff(mod.BuffType("SSJ1KaiokenBuff"));
                    player.ClearBuff(mod.BuffType("SSJ2Buff"));
                    player.ClearBuff(mod.BuffType("SSJ3Buff"));
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/PowerDown").WithVolume(.3f));
                }
            }


        }
        public void ThoriumEffects(Player player)
        {
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).symphonicDamage += DamageMulti;
            player.GetModPlayer<ThoriumMod.ThoriumPlayer>(ModLoader.GetMod("ThoriumMod")).radiantBoost += DamageMulti;
        }
        public void TremorEffects(Player player)
        {
            player.GetModPlayer<Tremor.MPlayer>(ModLoader.GetMod("Tremor")).alchemicalDamage += DamageMulti;
        }
        public void EnigmaEffects(Player player)
        {
            player.GetModPlayer<Laugicality.LaugicalityPlayer>(ModLoader.GetMod("Laugicality")).mysticDamage += DamageMulti;
        }
        public void BattleRodEffects(Player player)
        {
            player.GetModPlayer<UnuBattleRods.FishPlayer>(ModLoader.GetMod("UnuBattleRods")).bobberDamage += DamageMulti;
        }
        private void KiDrainAdd(Player player)
        {
            if(!DBZWorld.RealismMode)
            {
                MyPlayer.ModPlayer(player).KiDrainMulti = KiDrainBuffMulti;
            }
        }
    }
}

