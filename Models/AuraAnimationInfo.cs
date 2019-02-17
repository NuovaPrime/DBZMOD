using DBZMOD.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBZMOD.Models
{
    public class AuraAnimationInfo
    {
        public int id;
        public string auraAnimationSpriteName;
        public int frames;
        public int frameTimerLimit;
        public int priority;
        public BlendState blendState;
        public string startupSoundName;
        public string loopSoundName;
        public int loopSoundDuration;
        public bool isKaiokenAura;
        public bool isFormAura;
        public bool isStarting;
        public int startingFrames;
        public int startingFrameCounter;
        public DustDelegate doStartingDust;        
        public DustDelegate doDust;
        public delegate void DustDelegate(MyPlayer modPlayer, AuraAnimationInfo info);

        public AuraAnimationInfo()
        {
        }

        public AuraAnimationInfo(AuraID id, string spriteName, int frames, int frameTimer, BlendState blendState, string startupSound, string loopSoundName, int loopSoundDuration, bool isForm, bool isKaioken, DustDelegate dustDelegate, int startingFrames, DustDelegate startingDustDelegate, int priority)
        {
            this.id = (int)id;
            auraAnimationSpriteName = spriteName;
            this.frames = frames;
            frameTimerLimit = frameTimer;
            this.blendState = blendState;
            startupSoundName = startupSound;
            this.loopSoundName = loopSoundName;
            this.loopSoundDuration = loopSoundDuration;
            isKaiokenAura = isKaioken;
            isFormAura = isForm;
            doDust = dustDelegate;
            this.startingFrames = startingFrames;
            startingFrameCounter = 0;
            doStartingDust = startingDustDelegate;
            this.priority = priority;
        }

        public Texture2D GetTexture()
        {
            return DBZMOD.Instance.GetTexture(auraAnimationSpriteName);
        }

        public int GetHeight()
        {
            return GetTexture().Height / frames;
        }

        public int GetWidth()
        {
            return GetTexture().Width;
        }

        public Tuple<float, Vector2> GetAuraRotationAndPosition(MyPlayer modPlayer)
        {
            // update handler to reorient the charge up aura after the aura offsets are defined.
            bool isPlayerMostlyStationary = Math.Abs(modPlayer.player.velocity.X) <= 6F && Math.Abs(modPlayer.player.velocity.Y) <= 6F;
            float rotation = 0f;
            Vector2 position = Vector2.Zero;
            float scale = GetAuraScale(modPlayer);
            int auraOffsetY = GetAuraOffsetY(modPlayer);
            if (modPlayer.isFlying && !isPlayerMostlyStationary && !modPlayer.isPlayerUsingKiWeapon)
            {
                // ever so slightly shift the aura down a tad.
                var forwardOffset = (int)Math.Floor(modPlayer.player.height * 0.75f);
                double rotationOffset = modPlayer.player.fullRotation <= 0f ? (float)Math.PI : -(float)Math.PI;
                rotation = (float)(modPlayer.player.fullRotation + rotationOffset);

                // using the angle of attack, construct the cartesian offsets of the aura based on the height of both things
                double widthRadius = modPlayer.player.width / 4;
                double heightRadius = modPlayer.player.height / 4;
                double auraWidthRadius = GetWidth() / 4;
                double auraHeightRadius = GetHeight() / 4;

                // for right now, I'm just doing this with some hard coding. When we get more aura work done
                // we can try to unify this code a bit.
                double widthOffset = auraWidthRadius - (widthRadius + auraOffsetY + forwardOffset);
                double heightOffset = auraHeightRadius - (heightRadius + auraOffsetY + forwardOffset);
                double cartesianOffsetX = widthOffset * Math.Cos(modPlayer.player.fullRotation);
                double cartesianOffsetY = heightOffset * Math.Sin(modPlayer.player.fullRotation);

                Vector2 cartesianOffset = modPlayer.player.Center + new Vector2((float)-cartesianOffsetY, (float)cartesianOffsetX);

                // offset the aura
                position = cartesianOffset;
            }
            else
            {
                position = modPlayer.player.Center + new Vector2(0f, auraOffsetY);
                rotation = 0f;
            }
            return new Tuple<float, Vector2>(rotation, position);
        }

        public void ProcessDust(MyPlayer modPlayer)
        {
            if (doDust != null)
                doDust(modPlayer, this);
        }

        public void ProcessStartingDust(MyPlayer modPlayer)
        {
            if (doStartingDust != null)
                doStartingDust(modPlayer, this);
        }

        public Vector2 GetCenter(MyPlayer modPlayer)
        {
            return GetAuraRotationAndPosition(modPlayer).Item2 + new Vector2(GetWidth(), GetHeight()) * 0.5f;
        }

        public int GetAuraOffsetY(MyPlayer modPlayer)
        {
            var frameHeight = GetHeight();
            var scale = GetAuraScale(modPlayer);
            // easy automatic aura offset.
            return (int)(-((frameHeight / 2) * scale - (modPlayer.player.height * 0.6f)));
        }

        public float GetAuraScale(MyPlayer modPlayer)
        {
            // universal scale handling
            // scale is based on kaioken level, which gets set to 0
            var baseScale = 1.0f;

            // special scaling for Kaioken auras only
            if (isKaiokenAura)
            {
                return baseScale * (0.9f + 0.1f * modPlayer.kaiokenLevel);
            }
            else
            {
                return baseScale;
            }
        }
    }
}
