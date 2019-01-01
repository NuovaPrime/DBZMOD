using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Enums;
using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace DBZMOD.Models
{
    public class AuraAnimationInfo
    {
        public int ID;
        public string AuraAnimationSpriteName;
        public int Frames;
        public int FrameTimerLimit;
        public int CurrentFrame = 0;
        public int FrameTimer = 0;
        public int Owner;
        public int Priority;
        public BlendState BlendState;
        public string StartupSoundName;
        public string LoopSoundName;
        public int LoopSoundDuration;
        public bool IsKaiokenAura;
        public bool IsFormAura;
        public bool IsStarting;
        public int StartingFrames;
        public int StartingFrameCounter;
        public DustDelegate DoStartingDust;        
        public DustDelegate DoDust;
        public delegate void DustDelegate(AuraAnimationInfo info);

        public AuraAnimationInfo()
        {
        }

        public AuraAnimationInfo(AuraID id, MyPlayer owner, string spriteName, int frames, int frameTimer, BlendState blendState, string startupSound, string loopSoundName, int loopSoundDuration, bool isForm, bool isKaioken, DustDelegate dustDelegate, int startingFrames, DustDelegate startingDustDelegate, int priority)
        {
            ID = (int)id;
            AuraAnimationSpriteName = spriteName;
            Frames = frames;
            FrameTimerLimit = frameTimer;
            BlendState = blendState;
            StartupSoundName = startupSound;
            LoopSoundName = loopSoundName;
            LoopSoundDuration = loopSoundDuration;
            IsKaiokenAura = isKaioken;
            IsFormAura = isForm;
            DoDust = dustDelegate;
            Owner = owner.player.whoAmI;
            StartingFrames = startingFrames;
            StartingFrameCounter = 0;
            DoStartingDust = startingDustDelegate;
            Priority = priority;
        }

        public Texture2D GetTexture()
        {
            return DBZMOD.instance.GetTexture(AuraAnimationSpriteName);
        }

        public int GetHeight()
        {
            return GetTexture().Height / Frames;
        }

        public int GetWidth()
        {
            return GetTexture().Width;
        }

        public Player GetPlayerOwner()
        {
            return Main.player[Owner];
        }

        public Tuple<float, Vector2> GetAuraRotationAndPosition()
        {
            var player = GetPlayerOwner();
            // update handler to reorient the charge up aura after the aura offsets are defined.
            bool isPlayerMostlyStationary = Math.Abs(player.velocity.X) <= 6F && Math.Abs(player.velocity.Y) <= 6F;
            float rotation = 0f;
            Vector2 position = Vector2.Zero;
            var modPlayer = player.GetModPlayer<MyPlayer>();
            float scale = GetAuraScale();
            int auraOffsetY = GetAuraOffsetY();
            if (MyPlayer.ModPlayer(player).IsFlying && !isPlayerMostlyStationary && !FlightSystem.IsPlayerUsingKiWeapon(modPlayer))
            {
                double rotationOffset = player.fullRotation <= 0f ? (float)Math.PI : -(float)Math.PI;
                rotation = (float)(player.fullRotation + rotationOffset);

                // using the angle of attack, construct the cartesian offsets of the aura based on the height of both things
                double widthRadius = player.width / 4;
                double heightRadius = player.height / 4;
                double auraWidthRadius = GetWidth() / 4;
                double auraHeightRadius = GetHeight() / 4;

                // for right now, I'm just doing this with some hard coding. When we get more aura work done
                // we can try to unify this code a bit.                
                double forwardOffset = 32;
                double widthOffset = auraWidthRadius - (widthRadius + (auraOffsetY + forwardOffset));
                double heightOffset = auraHeightRadius - (heightRadius + (auraOffsetY + forwardOffset));
                double cartesianOffsetX = widthOffset * Math.Cos(player.fullRotation);
                double cartesianOffsetY = heightOffset * Math.Sin(player.fullRotation);

                Vector2 cartesianOffset = player.Center + new Vector2((float)-cartesianOffsetY, (float)cartesianOffsetX);

                // offset the aura
                position = cartesianOffset;
            }
            else
            {
                position = player.Center + new Vector2(0f, auraOffsetY);
                rotation = 0f;
            }
            return new Tuple<float, Vector2>(rotation, position);
        }

        public void ProcessDust()
        {
            if (DoDust != null)
                DoDust(this);
        }

        public void ProcessStartingDust()
        {
            if (DoStartingDust != null)
                DoStartingDust(this);
        }

        public Vector2 GetCenter()
        {
            return GetAuraRotationAndPosition().Item2 + new Vector2(GetWidth(), GetHeight()) * 0.5f;
        }

        public int GetAuraOffsetY()
        {
            var player = GetPlayerOwner();
            var frameHeight = GetHeight();
            var scale = GetAuraScale();
            // easy automatic aura offset.
            return (int)((player.height * 0.6f - frameHeight / 2) * scale);
        }

        public float GetAuraScale()
        {
            var modPlayer = GetPlayerOwner().GetModPlayer<MyPlayer>();
            // universal scale handling
            // scale is based on kaioken level, which gets set to 0
            var baseScale = 1.0f + (0.1f * modPlayer.KaiokenLevel) - (IsKaiokenAura ? -0.1f : 0f);

            // special scaling for Kaioken auras only
            if (Transformations.IsAnythingOtherThanKaioken(modPlayer.player) && IsKaiokenAura)
            {
                return baseScale * 1.2f;
            }
            else
            {
                return baseScale;
            }
        }
    }
}
