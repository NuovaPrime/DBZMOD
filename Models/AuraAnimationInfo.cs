using DBZMOD.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using DBZMOD.Extensions;

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
        public int bottomEdgeOffset;
        public bool isKaiokenAura;
        public bool isFormAura;
        public int startingFrames;
        public int startingFrameCounter;
        public DustDelegate doStartingDust;        
        public DustDelegate doDust;
        public delegate void DustDelegate(MyPlayer modPlayer, AuraAnimationInfo info);

        public AuraAnimationInfo()
        {
        }

        /// <summary>
        ///     Instantiate a template for aura animation info. This constructor passes in all the values an animation needs to do its job.
        ///     Includes things like what sound to play, and some misc values that normalize aura behaviors.
        /// </summary>
        /// <param name="id">An identifier used in determining what auras should play or are playing.</param>
        /// <param name="spriteName">The name of the texture being used by the animation.</param>
        /// <param name="bottomEdgeOffset">How many pixels down to offset the sprite during animations. Used when a sprite sits too high naturally due to its sprite position.</param>
        /// <param name="frames">The number of frames contained in the spritesheet, used to automatically handle animation as well as frame height.</param>
        /// <param name="frameTimer">The speed at which the animation's frame timer should progress frames. Lower values mean faster animation.</param>
        /// <param name="blendState">The blend state to be used during the draw call - typically either AlphaBlend or Additive.</param>
        /// <param name="startupSound">The sound that should be played when powering up.</param>
        /// <param name="loopSoundName">The sound that should be played during transformation, which loops.</param>
        /// <param name="loopSoundDuration">The high-resolution duration of the sound that is looping, so that it loops without gaps.</param>
        /// <param name="isForm">Whether the aura is for a form (transformation) vs charging up or a kaioken.</param>
        /// <param name="isKaioken">Whether the aura is, specifically, kaioken, which has scaling and levels, making it different from other transformations.</param>
        /// <param name="dustDelegate">The delegate responsible for creating dust when the player is in transformation.</param>
        /// <param name="startingFrames">The number of startup frames that dust should play for when a transformation has startup dust.</param>
        /// <param name="startingDustDelegate">The delegate responsible for creating dust when the player first transforms.</param>
        /// <param name="priority">Deprecated rank of priority of the transformation, was to be used when auras could superimpose on one another, to determine draw order.</param>
        public AuraAnimationInfo(AuraID id, string spriteName, int bottomEdgeOffset, int frames, int frameTimer, BlendState blendState, string startupSound, string loopSoundName, int loopSoundDuration, bool isForm, bool isKaioken, DustDelegate dustDelegate, int startingFrames, DustDelegate startingDustDelegate, int priority)
        {
            this.id = (int)id;
            auraAnimationSpriteName = spriteName;
            this.bottomEdgeOffset = bottomEdgeOffset;
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
            return (int)Math.Ceiling(-((frameHeight / 2f) * scale - (modPlayer.player.height * 0.6f))) + bottomEdgeOffset;
        }

        public float GetAuraScale(MyPlayer modPlayer)
        {
            // universal scale handling
            // scale is based on kaioken level, which gets set to 0
            var baseScale = 0.8f;

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
