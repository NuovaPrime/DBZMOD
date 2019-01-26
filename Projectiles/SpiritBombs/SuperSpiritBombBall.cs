using System;
using System.Collections.Generic;
using DBZMOD.Destruction;
using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Projectiles.SpiritBombs
{
    public class SuperSpiritBombBall : KiProjectile
    {
        int _rocksFloating = 0;
        const int MAX_ROCKS = 25;
        const float BASE_SCALE = 1f;
        const float SCALE_INCREASE = 0.15f;
        const float TRAVEL_SPEED_COEFFICIENT = 8f;
        int _soundtimer = 0;
        KeyValuePair<uint, SoundEffectInstance> _soundInfo;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Spirit Bomb");
            ProjectileUtil.RegisterMassiveBlast(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.light = 1f;
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1600;
            projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            kiDrainRate = 10;
        }

        public float HeldTime
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

        //public override void Kill(int timeLeft)
        //{
        //    base.Kill(timeLeft);

        //    Player player = Main.player[projectile.owner];
        //    MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
        //    modPlayer.isMassiveBlastInUse = false;
        //}

        private bool _isInitialized = false;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            if (!_isInitialized)
            {
                modPlayer.isMassiveBlastCharging = true;
                //modPlayer.isMassiveBlastInUse = true;
                HeldTime = 1;
                _isInitialized = true;
            }

            if (player.channel && modPlayer.isMassiveBlastCharging)
            {
                projectile.scale = BASE_SCALE + SCALE_INCREASE * HeldTime;
                projectile.position = player.Center + new Vector2(0, -40 - (projectile.scale * 17));
                HeldTime++;

                // reduced from 25.
                for (int d = 0; d < 15; d++)
                {
                    // loop hitch for variance.
                    if (Main.rand.NextFloat() < 0.3f)
                        continue;

                    float angle = Main.rand.NextFloat(360);
                    float angleRad = MathHelper.ToRadians(angle);
                    Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                    Dust tDust = Dust.NewDustDirect(projectile.position + (position * (20 + 12.5f * projectile.scale)), projectile.width, projectile.height, 230, 0f, 0f, 213, default(Color), 2.0f);
                    tDust.velocity = Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2;
                    tDust.noGravity = true;
                }

                //Rock effect
                if (DBZMOD.IsTickRateElapsed(10) && _rocksFloating < MAX_ROCKS)
                {
                    // only some of the time, keeps it a little more varied.
                    if (Main.rand.NextFloat() < 0.6f)
                    {
                        _rocksFloating++;
                        BaseFloatingDestructionProj.SpawnNewFloatingRock(player, projectile);
                    }
                }

                if (projectile.timeLeft < 399)
                {
                    projectile.timeLeft = 400;
                }

                MyPlayer.ModPlayer(player).AddKi(-5, true, false);
                ProjectileUtil.ApplyChannelingSlowdown(player);
                
                // depleted check, release the ball
                if (MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    player.channel = false;
                }
                if (_soundtimer == 0)
                {
                    _soundInfo = SoundUtil.PlayCustomSound("Sounds/SpiritBombCharge", player, 0.5f);
                }
                _soundtimer++;
                if (_soundtimer > 120)
                {
                    _soundtimer = 0;
                }
            }
            else if (modPlayer.isMassiveBlastCharging)
            {
                modPlayer.isMassiveBlastCharging = false;
                float projectileWidthFactor = projectile.width * projectile.scale / TRAVEL_SPEED_COEFFICIENT;
                projectile.timeLeft = (int)Math.Ceiling(projectileWidthFactor) + 180;
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * TRAVEL_SPEED_COEFFICIENT;
                projectile.tileCollide = false;
                projectile.damage *= (int)projectile.scale / 2;
                _soundInfo = SoundUtil.KillTrackedSound(_soundInfo);
                SoundUtil.PlayCustomSound("Sounds/SpiritBombFire", player);
            }
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
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
            DBZMOD.circle.ApplyShader(radius);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}