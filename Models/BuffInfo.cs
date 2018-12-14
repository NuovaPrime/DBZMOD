using Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Util
{
    // helper class for storing all the details about a transformation that need to be referenced later.
    public class BuffInfo
    {
        public string BuffKeyName;
        public float Volume;
        public string SoundKey;
        public string[] ProjectileKeys;
        public Type[] AuraProjectileTypes;
        public MenuSelectionID MenuId;
        public string TransformationText;
        public Color TransformationTextColor;

        public BuffInfo(MenuSelectionID menuId, string buffKey, float volume, string soundKey, string transText, Color transTextColor, Type[] auraTypes, string[] projKeys)
        {
            this.MenuId = menuId;
            this.BuffKeyName = buffKey;
            this.Volume = volume;
            this.SoundKey = soundKey;
            this.TransformationText = transText;
            this.TransformationTextColor = transTextColor;
            this.AuraProjectileTypes = auraTypes;
            this.ProjectileKeys = projKeys;
        }

        public int GetBuffId()
        {
            return DBZMOD.DBZMOD.instance.BuffType(this.BuffKeyName);
        }
    }
}
