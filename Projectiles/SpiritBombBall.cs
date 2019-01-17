using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using DBZMOD.Util;
using DBZMOD.Destruction;

namespace DBZMOD.Projectiles
{
    public class SpiritBombBall : KiProjectile
    {
        int rocksFloating = 0;
        const int MAX_ROCKS = 25;
        const float BASE_SCALE = 1f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Bomb");
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.light = 1f;
            projectile.aiStyle = 1;
            aiType = 14;
            projectile.friendly = true;
            projectile.extraUpdates = 2;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
            projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            KiDrainRate = 10;
        }        

        public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255, 255, 255, 100);
        }

        private float HeldTime
        {
            get
            {
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }

        private bool isInitialized = false;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (!isInitialized)
            {
                HeldTime = 1;
                projectile.scale = 1f;
                isInitialized = true;
            }

            // cancel channeling if the projectile is maxed
            if (projectile.scale > 12 && player.channel)
            {
                player.channel = false;
            }

            if (player.channel && HeldTime > 0)
            {
                projectile.scale += 0.02f;
                projectile.position = player.Center + new Vector2(0, -20 - (projectile.scale * 17));

                // reduced from 25.
                for (int d = 0; d < 15; d++)
                {
                    // loop hitch for variance.
                    if (Main.rand.NextFloat() < 0.3f)
                        continue;

                    float angle = Main.rand.NextFloat(360);
                    float angleRad = MathHelper.ToRadians(angle);
                    Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                    Dust tDust = Dust.NewDustDirect(projectile.position + (position * (20 + 12.5f * projectile.scale)), projectile.width, projectile.height, 15, 0f, 0f, 213, default(Color), 2.0f);
                    tDust.velocity = Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2;
                    tDust.noGravity = true;
                }

                //Rock effect
                if (DBZMOD.IsTickRateElapsed(10) && rocksFloating < MAX_ROCKS)
                {
                    // only some of the time, keeps it a little more varied.
                    if (Main.rand.NextFloat() < 0.6f)
                    {
                        rocksFloating++;
                        BaseFloatingDestructionProj.SpawnNewFloatingRock(player, projectile);
                    }
                }

                projectile.netUpdate = true;

                if (projectile.timeLeft < 399)
                {
                    projectile.timeLeft = 400;
                }

                MyPlayer.ModPlayer(player).AddKi(-2, true, false);
                ProjectileUtil.ApplyChannelingSlowdown(player);

                projectile.netUpdate2 = true;

                // depleted check, release the ball
                if (MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    player.channel = false;
                }
                int soundtimer = 0;
                soundtimer++;
                if (soundtimer > 120)
                {
                    SoundUtil.PlayCustomSound("Sounds/SpiritBombCharge", player, 0.5f);
                    soundtimer = 0;
                }
            } else if (HeldTime > 0)
            {
                HeldTime = 0;
                projectile.timeLeft = (int)Math.Ceiling(projectile.scale * 15) + 400;                
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * 3.5f;
                projectile.tileCollide = false;
                projectile.damage *= (int)projectile.scale / 2;
                SoundUtil.PlayCustomSound("Sounds/SpiritBombFire", player);
            }
        }

        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            projectile.scale -= 0.25f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float radius = projectile.width * projectile.scale / 2f;
            float rSquared = radius * radius;

            return rSquared > Vector2.DistanceSquared(Vector2.Clamp(projectile.Center, targetHitbox.TopLeft(), targetHitbox.BottomRight()), projectile.Center);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            int radius = (int)Math.Ceiling(projectile.width / 2f * projectile.scale);
            DBZMOD.Circle.ApplyShader(radius);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}