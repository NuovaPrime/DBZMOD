using System;
using System.Collections.Generic;
using DBZMOD.Dynamicity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DBZMOD.Transformations
{
    // helper class for storing all the details about a transformation that need to be referenced later.
    public abstract class TransformationDefinition : IHasUnlocalizedName, IHasParents<TransformationDefinition>
    {
        internal const string
            TRANSFORMATIONDEFINITION_PREFIX = "TransformationDefinition_",
            TRANSFORMATIONDEFINITION_HASUNLOCKED_SUFFIX = "_Unlocked",
            TRANSFORMATIONDEFINITION_MASTERY_SUFFIX = "_Mastery";

        /// <summary>
        ///     Instantiate a new buff info, typically a form like SSJ or Kaioken.
        /// </summary>
        /// <param name="unlocalizedName">The key name of the buff used in the mod.</param>
        /// <param name="transText">The text displayed when transforming.</param>
        /// <param name="transTextColor">The color of the transformation text.</param>
        /// <param name="baseDamageMultiplier"></param>
        /// <param name="baseSpeedMultiplier"></param>
        /// <param name="baseDefenseBonus"></param>
        /// <param name="baseKiSkillDrainMultiplier"></param>
        /// <param name="baseKiDrainRate"></param>
        /// <param name="baseKiDrainRateMastery"></param>
        /// <param name="baseHealthDrainRate"></param>
        /// <param name="appearance"></param>
        /// <param name="buff"></param>
        /// <param name="buffIconGetter">The icon to display in the Transformation Menu; leave as null if you don't want to display the transformation.</param>
        /// <param name="transformationFailureText">The text displayed when the player fails to achieve (select in the menu) the transformation.</param>
        /// <param name="extraTooltipText"></param>
        /// <param name="canBeMastered">Whether the form has a mastery rating.</param>
        /// <param name="masterFormBuffKeyName">What form the buff's ki cost is associated with for mastery (like ASSJ and USSJ being SSJ1, for example)</param>
        /// <param name="unlockRequirements">The requirements to unlock the form. Will be checked after <param name="parents">parents</param>.</param>
        /// <param name="selectionRequirements">The requirements to select the form in the interface. Checked after verifying if the player has the transformation. Leaving this value null will default to checking wether or not the player has unlocked the transformation.</param>
        /// <param name="selectionRequirementsFailed"></param>
        /// <param name="parents">The transformations that need to be unlocked before this transformation can also be unlocked.</param>
        protected TransformationDefinition(string unlocalizedName, string transText, Color transTextColor,
            float baseDamageMultiplier, float baseSpeedMultiplier, int baseDefenseBonus, float baseKiSkillDrainMultiplier, float baseKiDrainRate, float baseKiDrainRateMastery, float baseHealthDrainRate,
            TransformationAppearanceDefinition appearance,
            Type buff, Func<Texture2D> buffIconGetter = null, string transformationFailureText = null, string extraTooltipText = null, bool canBeMastered = false, string masterFormBuffKeyName = null, 
            Predicate<MyPlayer> unlockRequirements = null, Func<MyPlayer, TransformationDefinition, bool> selectionRequirements = null, Func<MyPlayer, TransformationDefinition, bool> selectionRequirementsFailed = null, params TransformationDefinition[] parents)
        {
            UnlocalizedName = unlocalizedName;
            TransformationText = transText;

            BaseDamageMultiplier = baseDamageMultiplier;
            BaseSpeedMultiplier = baseSpeedMultiplier;
            BaseDefenseBonus = baseDefenseBonus;
            BaseKiSkillDrainMultiplier = baseKiSkillDrainMultiplier;
            BaseKiDrainRate = baseKiDrainRate;
            BaseKiDrainRateMastery = baseKiDrainRateMastery;
            BaseHealthDrainRate = baseHealthDrainRate;

            TransformationTextColor = transTextColor;

            Buff = buff;
            BuffIconGetter = buffIconGetter;

            TransformationFailureText = transformationFailureText;
            ExtraTooltipText = extraTooltipText;

            Appearance = appearance;

            if (selectionRequirements == null)
                selectionRequirements = (p, t) => PlayerHasTransformation(p);

            if (selectionRequirementsFailed == null)
                selectionRequirementsFailed = (p, t) => true;

            SelectionRequirements = selectionRequirements;
            SelectionRequirementsFailed = selectionRequirementsFailed;

            HasMastery = canBeMastered;
            MasteryBuffKeyName = masterFormBuffKeyName;
            UnlockRequirements = unlockRequirements;

            Parents = parents;
        }

        #region Unlocking

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

            return CanPlayerUnlock(player) && Unlock(player);
        }

        #endregion

        public int GetBuffId() => DBZMOD.Instance.BuffType(this.UnlocalizedName);

        #region Save

        public string GetUnlockedTagCompoundKey() => TRANSFORMATIONDEFINITION_PREFIX + UnlocalizedName + TRANSFORMATIONDEFINITION_HASUNLOCKED_SUFFIX;

        public string GetMasteryTagCompoundKey() => TRANSFORMATIONDEFINITION_PREFIX + UnlocalizedName + TRANSFORMATIONDEFINITION_MASTERY_SUFFIX;

        #endregion

        public float GetPlayerMastery(MyPlayer player) => player.PlayerTransformations[this].Mastery;


        internal bool PlayerHasTransformation(MyPlayer player) => player.PlayerTransformations.ContainsKey(this);

        internal bool PlayerHasTransformationAndNotLegendary(MyPlayer player) => PlayerHasTransformation(player) && !player.IsLegendary();

        public bool CanPlayerUnlock(MyPlayer player)
        {
            for (int i = 0; i < Parents.Length; i++)
                if (!player.HasTransformation(Parents[i]))
                    return false;

            return UnlockRequirements == null || UnlockRequirements.Invoke(player);
        }

        public bool MeetsSelectionRequirements(MyPlayer player) => PlayerHasTransformation(player) && SelectionRequirements.Invoke(player, this);

        #region Stat Affecting Methods

        public virtual float GetDamageMultiplier(MyPlayer player) => ModifiedDamageMultiplier;

        public virtual float GetSpeedMultiplier(MyPlayer player) => ModifiedSpeedMultiplier;

        public virtual int GetDefenseBonus(MyPlayer player) => ModifiedDefenseBonus;

        public virtual float GetKiSkillDrainMultiplier(MyPlayer player) => ModifiedKiSkillDrainMultiplier;

        public virtual float GetKiDrainRate(MyPlayer player) => ModifiedKiDrainRate;

        public virtual float GetKiDrainRateMastery(MyPlayer player) => ModifiedKiDrainRateMastery;

        public virtual float GetHealthDrainRate(MyPlayer player) => ModifiedHealthDrainRate;

        public virtual void GetPlayerLightModifier(ref float lightingRed, ref float lightingGreen, ref float lightingBlue)
        {
            if (Appearance.lightColor == null) return;

            lightingRed = Appearance.lightColor.red;
            lightingGreen = Appearance.lightColor.green;
            lightingBlue = Appearance.lightColor.blue;
        }

        #endregion

        public override string ToString() => UnlocalizedName;


        public string UnlocalizedName { get; }

        public string TransformationText { get; }

        public bool HasMastery { get; }

        #region Stat Affecting Properties

        public float BaseDamageMultiplier { get; }
        public virtual float ModifiedDamageMultiplier => BaseDamageMultiplier;

        public float BaseSpeedMultiplier { get; }
        public virtual float ModifiedSpeedMultiplier => BaseSpeedMultiplier;

        public int BaseDefenseBonus { get; }
        public virtual int ModifiedDefenseBonus => BaseDefenseBonus;

        public float BaseKiSkillDrainMultiplier { get; }
        public virtual float ModifiedKiSkillDrainMultiplier => BaseKiSkillDrainMultiplier;

        public float BaseKiDrainRate { get; }
        public virtual float ModifiedKiDrainRate => BaseKiDrainRate;

        public float BaseKiDrainRateMastery { get; }
        public virtual float ModifiedKiDrainRateMastery => BaseKiDrainRateMastery;

        public float BaseHealthDrainRate { get; }
        public virtual float ModifiedHealthDrainRate => BaseHealthDrainRate;

        #endregion

        public TransformationAppearanceDefinition Appearance { get; }

        public Color TransformationTextColor { get; }

        public Type Buff { get; }
        public Func<Texture2D> BuffIconGetter { get; }

        public string TransformationFailureText { get; }

        public string ExtraTooltipText { get; }

        internal Func<MyPlayer, TransformationDefinition, bool> SelectionRequirements { get; }

        internal Func<MyPlayer, TransformationDefinition, bool> SelectionRequirementsFailed { get; }


        public string MasteryBuffKeyName { get; }

        internal Predicate<MyPlayer> UnlockRequirements { get; }

        public TransformationDefinition[] Parents { get; }


        // TODO Complete many transformations
        //protected virtual List<TransformationDefinition> CompatibleTransformations { get; } = new List<TransformationDefinition>();
    }
}
