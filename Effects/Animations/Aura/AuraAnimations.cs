using DBZMOD.Enums;
using DBZMOD.Models;
using Microsoft.Xna.Framework;
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

        public static AuraAnimationInfo CreateChargeAura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.Charge, modPlayer, GetAnimationSpriteName("BaseAura"), 4, 3, true, "Sounds/EnergyChargeStart", "Sounds/EnergyCharge", 22, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null);
        }

        public static void DoChargeDust(AuraAnimationInfo auraInfo)
        {
            var position = auraInfo.GetAuraRotationAndPosition().Item2;

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

        public static AuraAnimationInfo CreateSSJ1Aura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.SSJ1, modPlayer, GetAnimationSpriteName("SSJ1Aura"), 4, 3, true, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust));
        }

        public static AuraAnimationInfo CreateASSJAura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.ASSJ, modPlayer, GetAnimationSpriteName("SSJ1Aura"), 4, 3, true, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust));
        }

        public static AuraAnimationInfo CreateUSSJAura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.USSJ, modPlayer, GetAnimationSpriteName("SSJ1Aura"), 4, 3, true, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust));
        }

        public static void DoSSJ1Dust(AuraAnimationInfo aura)
        {
            var player = aura.GetPlayerOwner();
            const float AURAWIDTH = 3.0f;

            for (int i = 0; i < 20; i++)
            {
                float xPos = ((Vector2.UnitX * 5.0f) + (Vector2.UnitX * (Main.rand.Next(-10, 10) * AURAWIDTH))).X;
                float yPos = ((Vector2.UnitY * player.height) - (Vector2.UnitY * Main.rand.Next(0, player.height))).Y - 0.5f;

                Dust tDust = Dust.NewDustDirect(player.position + new Vector2(xPos, yPos), 1, 1, 87, 0f, 0f, 0, new Color(0, 0, 0, 0), 0.4f * Main.rand.Next(1, 4));

                if ((Math.Abs((tDust.position - (player.position + (Vector2.UnitX * 7.0f))).X)) < 10)
                {
                    tDust.scale *= 0.75f;
                }

                Vector2 dir = -(tDust.position - ((player.position + (Vector2.UnitX * 5.0f)) - (Vector2.UnitY * player.height)));
                dir.Normalize();

                tDust.velocity = new Vector2(dir.X * 2.0f, -1 * Main.rand.Next(1, 5));
                tDust.noGravity = true;
            }
        }

        public static AuraAnimationInfo CreateFalseUIAura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.FalseUI, modPlayer, GetAnimationSpriteName("FalseUIAura"), 15, 4, true, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, new AuraAnimationInfo.DustDelegate(DoFalseUIDust), 0, null);
        }

        public static void DoFalseUIDust(AuraAnimationInfo aura)
        {
            //blue dust
            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust;
                Vector2 center = aura.GetCenter();
                Vector2 position = center + new Vector2(-15, -20);
                dust = Terraria.Dust.NewDustDirect(position, 42, 58, 187, 0f, -5.526316f, 0, new Color(255, 255, 255), 0.8552632f);
                dust.noGravity = true;
                dust.fadeIn = 0.5131579f;
            }
            //white dust
            if (Main.rand.NextFloat() < 0.5263158f)
            {
                Dust dust;
                Vector2 center = aura.GetCenter();
                Vector2 position = center + new Vector2(-17, -10);
                dust = Terraria.Dust.NewDustDirect(position, 26, 52, 63, 0f, -7.368421f, 0, new Color(255, 255, 255), 0.8552632f);
                dust.noGravity = true;
                dust.fadeIn = 0.7894737f;
            }
        }

        public static AuraAnimationInfo CreateKaiokenAura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.Kaioken, modPlayer, GetAnimationSpriteName("KaiokenAura"), 4, 3, false, "Sounds/KaioAuraStart", "Sounds/EnergyCharge", 22, true, true, null, 0, null);
        }

        public static AuraAnimationInfo CreateSuperKaiokenAura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.SuperKaioken, modPlayer, GetAnimationSpriteName("SuperKaiokenAura"), 4, 3, false, "Sounds/KaioAuraStart", "Sounds/EnergyCharge", 22, true, true, null, 0, null);
        }

        public static AuraAnimationInfo CreateLSSJ2Aura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.LSSJ2, modPlayer, GetAnimationSpriteName("LSSJ2Aura"), 4, 3, true, "Sounds/SSJAscension", "Sounds/SSJ3", 260, true, false, null, 0, null);
        }

        public static AuraAnimationInfo CreateLSSJAura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.LSSJ, modPlayer, GetAnimationSpriteName("LSSJAura"), 4, 3, true, "Sounds/SSJAscension", "Sounds/SSJ2", 510, true, false, null, 0, null);
        }

        public static AuraAnimationInfo CreateSSJ2Aura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.SSJ2, modPlayer, GetAnimationSpriteName("SSJ2Aura"), 4, 3, true, "Sounds/SSJAscension", "Sounds/SSJ2", 510, true, false, null, 0, null);
        }

        public static AuraAnimationInfo CreateSSJ3Aura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.SSJ3, modPlayer, GetAnimationSpriteName("SSJ3Aura"), 4, 3, true, "Sounds/SSJAscension", "Sounds/SSJ3", 260, true, false, null, 0, null);
        }

        public static AuraAnimationInfo CreateSSJGAura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.SSJG, modPlayer, GetAnimationSpriteName("SSJGAura"), 8, 3, true, "Sounds/SSJAscension", "Sounds/SSG", 340, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null);
        }

        public static AuraAnimationInfo CreateSpectrumAura(MyPlayer modPlayer)
        {
            return new AuraAnimationInfo(AuraID.Spectrum, modPlayer, GetAnimationSpriteName("SSJSpectrumAura"), 8, 3, true, "Sounds/SSJAscension", "Sounds/SSG", 340, true, false, new AuraAnimationInfo.DustDelegate(DoFabulousDust), 0, null);
        }

        public static void DoFabulousDust(AuraAnimationInfo aura)
        {
            var position = aura.GetAuraRotationAndPosition().Item2;
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
    }
}
