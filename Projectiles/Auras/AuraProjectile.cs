using DBZMOD;
using DBZMOD.Projectiles.Auras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

namespace DBZMOD.Projectiles.Auras
{
    public abstract class AuraProjectile : ModProjectile
    {
        public bool IsSSJAura;
        public bool IsKaioAura;
        public bool IsGodAura;
        public bool AuraActive;
        public Vector2 ScaledAuraOffset;
        public Vector2 OriginalAuraOffset;
        public float OriginalScale;
        public bool HasComplexBlendStates = false;

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override bool PreAI()
        {
            // make sure behind the scenes we're still playing keep-alive.
            if (projectile.timeLeft < 2)
            {
                projectile.timeLeft = 10;
            }

            Player player = Main.player[projectile.owner];
            return AuraProjectileAnimationShouldSkipAIWhenHidden(player);
        }

        public bool AuraProjectileAnimationShouldSkipAIWhenHidden(Player player)
        {
            // skip on auras that have special hide handling.
            if (ProjectileID.Sets.DontAttachHideToAlpha[projectile.type])
            {
                return true;
            }

            // if we're in the middle of aura animations, return until they're over, and keep the projectile hidden
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.hide = modPlayer.IsTransformationAnimationPlaying;

            // don't run AI on the hidden projectile, this prevents sounds from playing, etc.
            return !projectile.hide;
        }

		//flicker fix
		public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255, 255, 255, 255);
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            HandleFrameAnimation(player);

            FlightSystem.HandleFlightAuraRotation(player, projectile, ScaledAuraOffset);
        }

        public void HandleFrameAnimation(Player player)
        {
            // if we're in the middle of aura animations, return until they're over, and keep the projectile hidden
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            bool isKaiokenAura = projectile.modProjectile is KaiokenAuraProj;

            // universal scale handling
            // scale is based on kaioken level, which gets set to 0
            OriginalScale = 1.0f + (0.1f * modPlayer.KaiokenLevel) - (isKaiokenAura ? -0.1f : 0f);

            float scaleMult = HasComplexBlendStates ? Main.GameZoomTarget : 1f;

            // special scaling for Kaioken auras only
            if (Transformations.IsAnythingOtherThanKaioken(player) && isKaiokenAura)
            {
                projectile.scale = OriginalScale * scaleMult * 1.2f;
            }
            else
            {
                projectile.scale = OriginalScale * scaleMult;
            }

            // easy automatic aura offset.
            OriginalAuraOffset.Y = (player.height * 0.6f - projectile.height / 2) * projectile.scale;

            // correct scaling
            if (ScaledAuraOffset != OriginalAuraOffset)
                ScaledAuraOffset = OriginalAuraOffset;

            int frameCounterLimit = 3;

            // normal frame progression
            projectile.frameCounter++;

            // double the frame counter speed if charging
            if (modPlayer.IsCharging)
                projectile.frameCounter++;

            if (projectile.frameCounter > frameCounterLimit)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }			

            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;
        }
    }
}