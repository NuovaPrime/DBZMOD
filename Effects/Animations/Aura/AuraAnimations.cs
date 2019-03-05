using DBZMOD.Enums;
using DBZMOD.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace DBZMOD.Effects.Animations.Aura
{
    public static class AuraAnimations
    {
        private static string GetAnimationSpriteName(string spriteName)
        {
            return string.Format("Effects/Animations/Aura/{0}", spriteName);
        }

        public static void DoChargeDust(MyPlayer modPlayer, AuraAnimationInfo auraInfo)
        {
            var position = auraInfo.GetAuraRotationAndPosition(modPlayer).Item2;

            for (int d = 0; d < 1; d++)
            {
                if (Main.rand.NextFloat() < 0.5f)
                {
                    Dust dust = Dust.NewDustDirect(position, auraInfo.GetWidth(), auraInfo.GetHeight(), 63, 0f, 0f, 0, new Color(255, 255, 255), 0.75f);
                    dust.noGravity = true;
                }
                if (Main.rand.NextFloat() < 0.25f)
                {
                    Dust dust = Dust.NewDustDirect(position, auraInfo.GetWidth(), auraInfo.GetHeight(), 63, 0f, 0f, 0, new Color(255, 255, 255), 1.5f);
                    dust.noGravity = true;
                }
            }
        }

        public static AuraAnimationInfo ssj1Aura = new AuraAnimationInfo(AuraID.SSJ1, GetAnimationSpriteName("SSJ1Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust), 1);        
        public static AuraAnimationInfo assjAura = new AuraAnimationInfo(AuraID.ASSJ, GetAnimationSpriteName("SSJ1Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust), 1);        
        public static AuraAnimationInfo ussjAura = new AuraAnimationInfo(AuraID.USSJ, GetAnimationSpriteName("SSJ1Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust), 1);

        public static AuraAnimationInfo ssj2Aura = new AuraAnimationInfo(AuraID.SSJ2, GetAnimationSpriteName("SSJ2Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSJ2", 510, true, false, null, 0, null, 1);
        public static AuraAnimationInfo ssj3Aura = new AuraAnimationInfo(AuraID.SSJ3, GetAnimationSpriteName("SSJ3Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSJ3", 260, true, false, null, 0, null, 1);
        public static AuraAnimationInfo ssj4Aura = new AuraAnimationInfo(AuraID.SSJ4, GetAnimationSpriteName("SSJ1Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust), 1);

        public static AuraAnimationInfo ssjgAura = new AuraAnimationInfo(AuraID.SSJG, GetAnimationSpriteName("SSJGAura"), 8, 3, BlendState.AlphaBlend, "Sounds/SSJAscension", "Sounds/SSG", 340, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1);
        public static AuraAnimationInfo ssjbAura = new AuraAnimationInfo(AuraID.SSJB, GetAnimationSpriteName("SSJBAura"), 8, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSG", 340, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1);
        public static AuraAnimationInfo ssjrAura = new AuraAnimationInfo(AuraID.SSJR, GetAnimationSpriteName("SSJRAura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSG", 340, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1);

        public static AuraAnimationInfo uiOmenAura = new AuraAnimationInfo(AuraID.UIOmen, GetAnimationSpriteName("UIOmenAura"), 15, 4, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSG", 340, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1);

        public static AuraAnimationInfo createFalseUIAura = new AuraAnimationInfo(AuraID.FalseUI, GetAnimationSpriteName("FalseUIAura"), 15, 4, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, new AuraAnimationInfo.DustDelegate(DoFalseUIDust), 0, null, 1);
        public static AuraAnimationInfo createKaiokenAura = new AuraAnimationInfo(AuraID.Kaioken, GetAnimationSpriteName("KaiokenAura"), 4, 3, BlendState.Additive, "Sounds/KaioAuraStart", "Sounds/EnergyCharge", 22, true, true, null, 0, null, 0);
        public static AuraAnimationInfo createSuperKaiokenAura = new AuraAnimationInfo(AuraID.SuperKaioken, GetAnimationSpriteName("SuperKaiokenAura"), 4, 3, BlendState.Additive, "Sounds/KaioAuraStart", "Sounds/EnergyCharge", 22, true, true, null, 0, null, 0);
        
        public static AuraAnimationInfo lssjAura = new AuraAnimationInfo(AuraID.LSSJ, GetAnimationSpriteName("LSSJAura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSJ2", 510, true, false, null, 0, null, 1);
        
        public static AuraAnimationInfo chargeAura = new AuraAnimationInfo(AuraID.Charge, GetAnimationSpriteName("BaseAura"), 4, 3, BlendState.Additive, "Sounds/EnergyChargeStart", "Sounds/EnergyCharge", 22, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1);

        public static void DoSSJ1Dust(MyPlayer modPlayer, AuraAnimationInfo aura)
        {
            const float aurawidth = 3.0f;

            for (int i = 0; i < 20; i++)
            {
                float xPos = ((Vector2.UnitX * 5.0f) + (Vector2.UnitX * (Main.rand.Next(-10, 10) * aurawidth))).X;
                float yPos = ((Vector2.UnitY * modPlayer.player.height) - (Vector2.UnitY * Main.rand.Next(0, modPlayer.player.height))).Y - 0.5f;

                Dust tDust = Dust.NewDustDirect(modPlayer.player.position + new Vector2(xPos, yPos), 1, 1, 87, 0f, 0f, 0, new Color(0, 0, 0, 0), 0.4f * Main.rand.Next(1, 4));

                if ((Math.Abs((tDust.position - (modPlayer.player.position + (Vector2.UnitX * 7.0f))).X)) < 10)
                {
                    tDust.scale *= 0.75f;
                }

                Vector2 dir = -(tDust.position - ((modPlayer.player.position + (Vector2.UnitX * 5.0f)) - (Vector2.UnitY * modPlayer.player.height)));
                dir.Normalize();

                tDust.velocity = new Vector2(dir.X * 2.0f, -1 * Main.rand.Next(1, 5));
                tDust.noGravity = true;
            }
        }

        public static void DoFalseUIDust(MyPlayer modPlayer, AuraAnimationInfo aura)
        {
            //blue dust
            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust;
                Vector2 center = aura.GetCenter(modPlayer);
                Vector2 position = center + new Vector2(-15, -20);
                dust = Terraria.Dust.NewDustDirect(position, 42, 58, 187, 0f, -5.526316f, 0, new Color(255, 255, 255), 0.8552632f);
                dust.noGravity = true;
                dust.fadeIn = 0.5131579f;
            }
            //white dust
            if (Main.rand.NextFloat() < 0.5263158f)
            {
                Dust dust;
                Vector2 center = aura.GetCenter(modPlayer);
                Vector2 position = center + new Vector2(-17, -10);
                dust = Terraria.Dust.NewDustDirect(position, 26, 52, 63, 0f, -7.368421f, 0, new Color(255, 255, 255), 0.8552632f);
                dust.noGravity = true;
                dust.fadeIn = 0.7894737f;
            }
        }

        public static void DoFabulousDust(MyPlayer modPlayer, AuraAnimationInfo aura)
        {
            var position = aura.GetAuraRotationAndPosition(modPlayer).Item2;
            if (Main.rand.NextFloat() < 0.5f)
            {
                Dust dust = Dust.NewDustDirect(position, aura.GetWidth(), aura.GetHeight(), 91, 0f, 0f, 0, new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B), 0.75f);
                dust.noGravity = true;
            }
            if (Main.rand.NextFloat() < 0.25f)
            {
                Dust dust = Dust.NewDustDirect(position, aura.GetWidth(), aura.GetHeight(), 91, 0f, 0f, 0, new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B), 0.25f);
                dust.noGravity = true;
            }
        }

        /// <summary>
        ///     Return the aura effect currently active on the player.
        /// </summary>
        /// <param name="player">The player being checked</param>
        public static AuraAnimationInfo GetAuraEffectOnPlayer(this MyPlayer player)
        {
            if (player.player.dead)
                return null;

            if (player.ActiveTransformations.Count > 0)
                return player.ActiveTransformations[0].Appearance.auraAnimation;

            if (player.isCharging && player.ActiveTransformations.Count == 0)
                return chargeAura;

            return null;
        }
    }
}
