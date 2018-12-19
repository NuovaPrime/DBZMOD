using DBZMOD;
using DBZMOD.Projectiles.Auras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace Projectiles.Auras
{
    public abstract class AuraProjectile : ModProjectile
    {
        public bool IsSSJAura;
        public bool IsKaioAura;
        public bool IsGodAura;
        public bool AuraActive;
        public Vector2 AuraOffset;
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];

            // if we're in the middle of aura animations, return until they're over, and keep the projectile hidden
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.hide = modPlayer.IsTransformationAnimationPlaying;

            // make sure behind the scenes we're still playing keep-alive.
            if (projectile.timeLeft < 2)
            {
                projectile.timeLeft = 10;
            }

            // don't run AI on the hidden projectile, this prevents sounds from playing, etc.
            return !projectile.hide;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;

            HandleFrameAnimation(player);

            FlightSystem.HandleFlightAuraRotation(player, projectile, AuraOffset);
        }

        public void HandleFrameAnimation(Player player)
        {
            // if we're in the middle of aura animations, return until they're over, and keep the projectile hidden
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

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
        }
    }
}