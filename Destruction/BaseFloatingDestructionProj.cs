﻿using System;
 using Microsoft.Xna.Framework;
using Terraria;
 using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Destruction
{

    public class BaseFloatingDestructionProj : ModProjectile
    {
        public bool isReleased = false;
        public float yVariance = 0f;
        public float initialRotationVariance = 0f;
        public float scaleVariance = 0f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Base Block Destruction");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.alpha = 1;
            projectile.height = 28;
            projectile.aiStyle = -1;
            projectile.damage = 0;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 10;
        }

        public override void AI()
        {
            var player = Main.player[projectile.owner];

            if (yVariance == 0)
            {
                yVariance = ((Main.rand.NextFloat() - 0.5f) * 30);
            }

            if (initialRotationVariance == 0f)
            {
                initialRotationVariance = Main.rand.NextFloat(-180, 180f);
                projectile.rotation = initialRotationVariance;
            }

            if (scaleVariance == 0f)
            {
                scaleVariance = Main.rand.NextFloat() + 0.5f;
                projectile.scale = scaleVariance;
            }

            // let go of the projectile if the player gets too far from it, but not until it's above the player's feet (prevents it from despawning below the player)
            if (projectile.Center.Y < (player.position.Y + player.height) && Vector2.Distance(player.Center, projectile.Center) > 250f)
            {
                isReleased = true;
            }

            if (!isReleased)
            {
                // decaying upward momentum
                projectile.velocity *= 0.995f;

                projectile.timeLeft = 10;
                if (projectile.position.Y <= player.position.Y + yVariance)
                {
                    projectile.velocity.Y = -0.15f;
                }

                float randomRotationWithThreshold = Main.rand.NextFloat() - 0.5f;

                if (Math.Abs(randomRotationWithThreshold) > 0.45)
                    randomRotationWithThreshold = 0;

                projectile.rotation += randomRotationWithThreshold * 0.2f;

                if (!player.channel)
                {
                    isReleased = true;
                }

                // spawn a bit of dust
                if (Main.rand.NextFloat() < 0.1f)
                {
                    float dustScale = Math.Max(0.2f, projectile.scale * 0.6f);
                    Dust.NewDustPerfect(projectile.Center, DustID.Dirt, new Vector2((Main.rand.NextFloat() - 0.5f) * 0.1f, 4f), 0, default(Color), dustScale);
                    projectile.scale *= 0.90f;
                    if (projectile.scale < 0.1f)
                    {
                        isReleased = true;
                        projectile.Kill();
                    }
                }
            }
            else
            {
                if (!projectile.tileCollide)
                    projectile.tileCollide = true;
                projectile.timeLeft = 15;
                // projectile falls to the ground
                projectile.velocity = Vector2.UnitY * 4f;
            }
        }

        public static void SpawnNewFloatingRock(Player player, Projectile parentProjectile)
        {            
            // only use one of these at a time.
            var xOffset = player.Center.X + ((Main.rand.NextFloat() - 0.5f) * 500);
            var yOffset = (player.position.Y + (player.height + 500));

            // mostly dirt, sometimes rocks.
            if (Main.rand.NextFloat() < 0.6)
            {
                Projectile.NewProjectile(xOffset, yOffset, 0, -10f, DBZMOD.Instance.ProjectileType("DirtBlockDestruction"), parentProjectile.damage, 0f, parentProjectile.owner);
            }
            else
            {
                Projectile.NewProjectile(xOffset, yOffset, 0, -10f, DBZMOD.Instance.ProjectileType("StoneBlockDestruction"), parentProjectile.damage, 0f, parentProjectile.owner);
            }
        }

        public override void Kill(int timeLeft)
        {
            var player = Main.player[projectile.owner];
            // player's still channeling, so let us spawn more rocks
            if (player.channel)
            {                
                SpawnNewFloatingRock(player, projectile);
            }
        }
    }
}