using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace Util
{
    public static class SoundUtil
    {
        public static DBZMOD.DBZMOD _mod;
        public static DBZMOD.DBZMOD mod
        {
            get
            {
                if (_mod == null)
                {
                    _mod = DBZMOD.DBZMOD.instance;
                }

                return _mod;
            }
        }

        public static void PlayVanillaSound(int soundId, Player player = null, float volume = 1f, float pitchVariance = 0f)
        {
            Vector2 location = player != null ? player.Center : Vector2.Zero;
            PlayVanillaSound(soundId, location, volume, pitchVariance);
        }

        public static void PlayVanillaSound(int soundId, Vector2 location, float volume = 1f, float pitchVariance = 0f)
        {
            if (Main.dedServ)
                return;
            if (location == Vector2.Zero)
            {
                Main.PlaySound(soundId, (int)location.X, (int)location.Y, 1, volume, pitchVariance);
            }
            else
            {
                // this method doesn't return a sound effect instance, it just plays a sound.
                Main.PlaySound(soundId, -1, -1, 1, volume, pitchVariance);
            }
        }

        public static SoundEffectInstance PlayVanillaSound(int soundId, Vector2 location, int style)
        {
            if (Main.dedServ)
                return null;

            // this method doesn't return a sound effect instance, it just plays a sound.
            return Main.PlaySound(soundId, (int)location.X, (int)location.Y, style);
        }


        

        public static ReLogic.Utilities.SlotId PlayVanillaSound(Terraria.Audio.LegacySoundStyle soundId, Player player = null, float volume = 1f, float pitchVariance = 0f)
        {
            Vector2 location = player != null ? player.Center : Vector2.Zero;
            return PlayVanillaSound(soundId, location, volume, pitchVariance);
        }

        public static ReLogic.Utilities.SlotId PlayVanillaSound(Terraria.Audio.LegacySoundStyle soundId, Vector2 location, float volume = 1f, float pitchVariance = 0f)
        {
            if (Main.dedServ)
                return ReLogic.Utilities.SlotId.Invalid;
            if (location == Vector2.Zero)
            {
                return Main.PlayTrackedSound(soundId.WithVolume(volume).WithPitchVariance(pitchVariance), location);
            } else
            {
                // this method doesn't return a sound effect instance, it just plays a sound.
                return Main.PlayTrackedSound(soundId.WithVolume(volume).WithPitchVariance(pitchVariance), location);
            }            
        }

        public static ReLogic.Utilities.SlotId PlayCustomSound(string soundId, Player player = null, float volume = 1f, float pitchVariance = 0f)
        {
            Vector2 location = player != null ? player.Center : Vector2.Zero;
            return PlayCustomSound(soundId, location, volume, pitchVariance);
        }

        public static ReLogic.Utilities.SlotId PlayCustomSound(string soundId, Vector2 location, float volume = 1f, float pitchVariance = 0f)
        {
            if (Main.dedServ)
                return ReLogic.Utilities.SlotId.Invalid;
            if (location == Vector2.Zero)
            {
                return Main.PlayTrackedSound(GetCustomStyle(soundId));
            } else
            {
                return Main.PlayTrackedSound(GetCustomStyle(soundId, volume, pitchVariance), location);
            }
        }

        public static Terraria.Audio.LegacySoundStyle GetCustomStyle(string soundId, float volume = 1f, float pitchVariance = 0f)
        {
            return mod.GetLegacySoundSlot(SoundType.Custom, soundId).WithVolume(volume).WithPitchVariance(pitchVariance);
        }
    }
}
