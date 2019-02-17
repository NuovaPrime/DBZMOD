using System;
using DBZMOD.Dynamicity;
using DBZMOD.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DBZMOD.Transformations
{
    // helper class for storing all the details about a transformation that need to be referenced later.
    public class TransformationDefinition : IHasUnlocalizedName, IHasParents<TransformationDefinition>
    {
        internal const string
            TRANSFORMATIONDEFINITION_PREFIX = "TransformationDefinition_",
            TRANSFORMATIONDEFINITION_HASUNLOCKED_SUFFIX = "_Unlocked",
            TRANSFORMATIONDEFINITION_MASTERY_SUFFIX = "_Mastery";

        /// <summary>
        ///     Instantiate a new buff info, typically a form like SSJ or Kaioken.
        /// </summary>
        /// <param name="menuId">The menu Id of the form, if it exists in the menu.</param>
        /// <param name="buffKey">The key name of the buff used in the mod.</param>
        /// <param name="transText">The text displayed when transforming.</param>
        /// <param name="transTextColor">The color of the transformation text.</param>
        /// <param name="canBeMastered">Whether the form has a mastery rating.</param>
        /// <param name="masterFormBuffKeyName">What form the buff's ki cost is associated with for mastery (like ASSJ and USSJ being SSJ1, for example)</param>
        public TransformationDefinition(MenuSelectionID menuId, string buffKey, string transText, Color transTextColor, Texture2D buffIcon, bool canBeMastered = false, string masterFormBuffKeyName = null, Predicate<MyPlayer> unlockRequirements = null, params TransformationDefinition[] parents)
        {
            UnlocalizedName = buffKey;

            MenuId = menuId;
            TransformationText = transText;
            TransformationTextColor = transTextColor;
            BuffIcon = buffIcon;

            HasMastery = canBeMastered;
            MasteryBuffKeyName = masterFormBuffKeyName;
            UnlockRequirements = unlockRequirements;

            Parents = parents;
        }

        /// <summary>Forces the transformation to be obtained without any cutscenes being played or requirements being checked.</summary>
        /// <param name="player">The <see cref="Player"/> to give the transformation to.</param>
        /// /// <returns>true if the transformation was unlocked; otherwise false.</returns>
        public bool Unlock(Player player) => Unlock(player.GetModPlayer<MyPlayer>());

        /// <summary>Forces the transformation to be obtained without any cutscenes being played or requirements being checked.</summary>
        /// <param name="player">The <see cref="MyPlayer"/> to give the transformation to.</param>
        /// <returns>true if the transformation was unlocked; otherwise false.</returns>
        public bool Unlock(MyPlayer player)
        {
            if (player.PlayerTransformations.ContainsKey(this)) return false;

            player.PlayerTransformations.Add(this, new PlayerTransformation(this, 0f));
            return true;
        }

        /// <summary>Tries to unlock the transformation for the player by checking if the requirements are met.</summary>
        /// <param name="player">The <see cref="Player"/> to give the transformation to.</param>
        /// <returns>true if the transformation was unlocked; otherwise false.</returns>
        public bool TryUnlock(Player player) => TryUnlock(player.GetModPlayer<MyPlayer>());

        /// <summary>Tries to unlock the transformation for the player by checking if the requirements are met.</summary>
        /// <param name="player">The <see cref="MyPlayer"/> to give the transformation to.</param>
        /// <returns>true if the transformation was unlocked; otherwise false.</returns>
        public bool TryUnlock(MyPlayer player)
        {
            for (int i = 0; i < Parents.Length; i++)
                if (!player.PlayerTransformations.ContainsKey(Parents[i]))
                    return false;

            return UnlockRequirements.Invoke(player) && Unlock(player);
        }

        public int GetBuffId()
        {
            return DBZMOD.Instance.BuffType(this.UnlocalizedName);
        }

        public string GetUnlockedTagCompoundKey() => TRANSFORMATIONDEFINITION_PREFIX + UnlocalizedName + TRANSFORMATIONDEFINITION_HASUNLOCKED_SUFFIX;

        public string GetMasteryTagCompoundKey() => TRANSFORMATIONDEFINITION_PREFIX + UnlocalizedName + TRANSFORMATIONDEFINITION_MASTERY_SUFFIX;

        public bool Equals(TransformationDefinition transformationDefinition) => UnlocalizedName == transformationDefinition?.UnlocalizedName;

        public override string ToString() => UnlocalizedName;

        public string UnlocalizedName { get; }

        public MenuSelectionID MenuId { get; }

        public string TransformationText { get; }

        public bool HasMastery { get; }

        public Color TransformationTextColor { get; }

        public Texture2D BuffIcon { get; }

        public string MasteryBuffKeyName { get; }

        public Predicate<MyPlayer> UnlockRequirements { get; }

        public TransformationDefinition[] Parents { get; }
    }
}
