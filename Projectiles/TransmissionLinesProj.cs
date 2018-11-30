using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace Projectiles
{
    class TransmissionLinesProj : ModProjectile
    {
        public bool isInitialized = false;
        private const int MAX_FRAMES = 18;
        public float ScaleExtra;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = MAX_FRAMES;
        }

        public override void SetDefaults()
        {
            Player player = Main.player[projectile.owner];
            projectile.width = 30;
            projectile.height = 49;
            projectile.aiStyle = 0;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            projectile.alpha = 50;
            ScaleExtra = 0.1f;
            projectile.light = 0f;
            projectile.position.X = player.Center.X;
            projectile.position.Y = player.Center.Y;
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public void InitializeVanishState()
        {
            isInitialized = true;
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Transmission").WithVolume(.7f).WithPitchVariance(.2f));
        }

        public override void AI()
        {
            if (!isInitialized)
            {
                InitializeVanishState();
            }
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;

            projectile.frameCounter++;
            if (projectile.frameCounter > projectile.frame)
            {
                projectile.frame++;
            }
            if (projectile.frame >= MAX_FRAMES)
            {
                projectile.frame = 0;
            }

            projectile.scale = 1.0f + ScaleExtra;
        }
    }
}
