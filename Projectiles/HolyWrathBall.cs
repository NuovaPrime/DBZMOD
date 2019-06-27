using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using DBZMOD.Util;
using Microsoft.Xna.Framework.Audio;

namespace DBZMOD.Projectiles
{
    public class HolyWrathBall : KiProjectile
    {
        const float BASE_SCALE = 0.15f;
        const float SCALE_INCREASE = 0.015f;
        const float TRAVEL_SPEED_COEFFICIENT = 18f;
        int _soundtimer = 0;
        KeyValuePair<uint, SoundEffectInstance> _soundInfo;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Holy Wrath Ball");
            Main.projFrames[projectile.type] = 1;
            ProjectileHelper.RegisterMassiveBlast(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 226;
            projectile.height = 226;
            projectile.light = 1f;
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
            projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            kiDrainRate = 12;
        }

        public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255, 255, 255, 100);
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

            // cancel channeling if the projectile is maxed
            if (player.channel && projectile.scale > 3.0)
            {
                player.channel = false;
            }

            if (player.channel && modPlayer.isMassiveBlastCharging)
            {
                projectile.scale = BASE_SCALE + SCALE_INCREASE * HeldTime;
                Vector2 projectileOffset = new Vector2(-projectile.width * 0.5f, -projectile.height * 0.5f);
                projectileOffset += new Vector2(0, -(80 + projectile.scale * 115f));
                projectile.position = player.Center + projectileOffset;
                HeldTime++;

                for (int d = 0; d < 25; d++)
                {
                    float angle = Main.rand.NextFloat(360);
                    float angleRad = MathHelper.ToRadians(angle);
                    Vector2 dustPosition = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                    Dust tDust = Dust.NewDustDirect(projectile.Center + (dustPosition * (40 + 110 * projectile.scale)), projectile.width, projectile.height, 169, 0f, 0f, 213, default(Color), 1.5f);
                    tDust.velocity = Vector2.Normalize((projectile.Center + projectile.Size / 2f) - tDust.position) * 2;
                    tDust.noGravity = true;
                }

                //Rock effect
                projectile.ai[1]++;
                if (projectile.ai[1] % 7 == 0)
                    Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("StoneBlockDestruction"), projectile.damage, 0f, projectile.owner);
                Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-500, 600), projectile.Center.Y + 1000, 0, -10, mod.ProjectileType("DirtBlockDestruction"), projectile.damage, 0f, projectile.owner);
                
                if (projectile.timeLeft < 399)
                {
                    projectile.timeLeft = 400;
                }

                MyPlayer.ModPlayer(player).AddKi(-2, true, false);
                ProjectileHelper.ApplyChannelingSlowdown(player);

                // depleted check, release the ball
                if (MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    player.channel = false;
                }
            }
            else if (modPlayer.isMassiveBlastCharging)
            {
                modPlayer.isMassiveBlastCharging = false;
                float projectileWidthFactor = projectile.width * projectile.scale / TRAVEL_SPEED_COEFFICIENT;
                projectile.timeLeft = (int)Math.Ceiling(projectileWidthFactor) + 180;
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * TRAVEL_SPEED_COEFFICIENT;
                projectile.tileCollide = false;
                projectile.damage *= (int)Math.Ceiling(projectile.scale / 3);
            }
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
        }

        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            projectile.scale -= 0.025f;
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