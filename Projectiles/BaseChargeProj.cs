using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;
using Util;
using Terraria.Enums;

namespace DBZMOD.Projectiles
{
    // unabashedly stolen from blushie's laser example, and then customized WIP
	public class BaseChargeProj : ModProjectile
    {
        // refactored portions of KiProjectile that make Charge Balls do what they do        
        public float ChargeLimit = 4;

        // the maximum ChargeLimit of the attack after bonuses from gear (etc) are applied
        public float FinalChargeLimit = 4;

        // the rate at which charge level increases while channeling
        public float ChargeRate = 0.016f; // approximately 1 level per second.

        // Rate at which Ki is drained while channeling
        public int ChargeKiDrainRate = 1;

        // determines the frequency at which ki drain ticks. Bigger numbers mean slower drain.
        public int ChargeKiDrainWindow = 2;

        // Rate at which Ki is drained while firing the beam
        public int FireKiDrainRate = 1;        

        // the rate at which the charge decays when not channeling
        public float DecayRate = 0.016f; // very slow decay when not channeling

        // the charge level of the projectile currently, increases with channeling and decays without it.
        public float ChargeLevel = 0.0f;

        //The distance charge particle from the player center
        private const float MoveDistance = 30f;

        // the charge ball is just a single texture.
        // these two vars specify its draw origin and size, this is a holdover from when it shared a texture sheet with other beam components.
        public Point ChargeOrigin = new Point(0, 0);
        public Point ChargeSize = new Point(30, 30);

        private Rectangle _chargeRectangle;
        public Rectangle ChargeRectangle
        {
            get
            {
                if (_chargeRectangle == null)
                {
                    _chargeRectangle = new Rectangle(ChargeOrigin.X, ChargeOrigin.Y, ChargeSize.X, ChargeSize.Y);
                }
                return _chargeRectangle;
            }
        }

        public override void SetDefaults()
        {
            projectile.width = ChargeSize.X;
            projectile.height = ChargeSize.Y;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            DrawChargeBall(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center,
                projectile.velocity, 1f, Color.White, (int)MoveDistance);
            return false;
        }

        // The core function of drawing a charge ball
        public void DrawChargeBall(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 orientation, float scale = 1f, Color color = default(Color), int transDist = 50)
        {
            // DebugUtil.Log(string.Format("Drawing charge ball... start vec {0}, {1}, orientation {2}, {3} hidden {4}", start.X, start.Y, orientation.X, orientation.Y, projectile.hide));
            spriteBatch.Draw(texture, start + orientation * transDist,// - Main.screenPosition,
                ChargeRectangle, Color.White, orientation.ToRotation(), new Vector2(ChargeSize.X * .5f, ChargeSize.Y * .5f), scale, 0, 0);
        }

        public override ModProjectile NewInstance(Projectile projectileClone)
        {
            DebugUtil.Log(string.Format("Cloning new instance of projectile, or trying to"));
            return base.NewInstance(projectileClone);
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }

        public void UpdateChargeBallLocationAndDirection(Player player, Vector2 mouseVector)
        {
            // Multiplayer support here, only run this code if the client running it is the owner of the projectile
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 diff = mouseVector - player.Center;
                diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = mouseVector.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }
            projectile.position = player.Center + projectile.velocity * MoveDistance;
            projectile.timeLeft = 2;
            int dir = projectile.direction;
            player.ChangeDir(dir);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
        }

        public void HandleChargingKi(Player player)
        {

            FinalChargeLimit = ChargeLimit + MyPlayer.ModPlayer(player).ChargeLimitAdd;

            // stop channeling if the player is out of ki
            if (MyPlayer.ModPlayer(player).IsKiDepleted())
            {
                player.channel = false;
            }

            // keep alive routine.
            if (projectile.timeLeft < 4)
            {
                projectile.timeLeft = 10;
            }

            // track whether charge level has changed by snapshotting it.
            float oldChargeLevel = ChargeLevel;

            // The energy in the projectile decays if the player stops channeling.
            if (!player.channel)
            {
                if (ChargeLevel > 0f)
                {
                    ChargeLevel = Math.Max(0, ChargeLevel - DecayRate);
                    DoDust(0.2f, true);
                } else
                {
                    projectile.Kill();
                }
            }

            // increment the charge timer if channeling and apply slowdown effect
            if (player.channel && projectile.active)
            {
                // drain ki from the player when charging
                if (Main.time > 0 && Math.Ceiling(Main.time % 2) == 0 && !MyPlayer.ModPlayer(player).IsKiDepleted())
                {
                    MyPlayer.ModPlayer(player).AddKi(-ChargeKiDrainRate);
                }

                ChargeLevel = Math.Min(FinalChargeLimit, ChargeRate + ChargeLevel);
                ProjectileUtil.ApplyChannelingSlowdown(player);
                DoDust(0.2f, false);
            }

            // If we just crossed a threshold, display combat text for the charge level increase.
            if (Math.Floor(oldChargeLevel) != Math.Floor(ChargeLevel) && oldChargeLevel != ChargeLevel)
            {
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 204, 255), (int)ChargeLevel, false, false);
            }
            
        }

        public void DoDust(float dustFrequency, bool isDecaying)
        {
            // snazzy charge up dust, reduced to less than 1 per frame.
            if (Main.rand.NextFloat() < dustFrequency)
            {
                float angle = Main.rand.NextFloat(360);
                float angleRad = MathHelper.ToRadians(angle);
                Vector2 position = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));

                Dust tDust = Dust.NewDustDirect(projectile.position + (position * (10 + 2.0f * projectile.scale)), projectile.width, projectile.height, 169, 0f, 0f, 213, default(Color), projectile.scale);
                tDust.velocity = (Vector2.Normalize((projectile.position + (projectile.Size / 2)) - tDust.position) * 2) * (isDecaying ? -1 : 1);
                tDust.noGravity = true;
            }
        }

        public void HandleFiring(Player player, Vector2 mouseVector)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            // minimum charge level
            if (ChargeLevel >= 0)
            {

            }
        }

        public void HandleChargeBallVisibility()
        {
            projectile.hide = ChargeLevel <= 0f;
        }

        // The AI of the projectile
        public override void AI()
        {
            Vector2 mouseVector = Main.MouseWorld;
            Player player = Main.player[projectile.owner];

            HandleChargeBallVisibility();

            UpdateChargeBallLocationAndDirection(player, mouseVector);

            HandleChargingKi(player);

            HandleFiring(player, mouseVector);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}