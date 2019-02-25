using System;
using System.Collections.Generic;
using DBZMOD.Dynamicity;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;

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
        /// <param name="text">The text displayed when transforming.</param>
        /// <param name="textColor">The color of the transformation text.</param>
        /// <param name="baseDamageMultiplier"></param>
        /// <param name="baseSpeedMultiplier"></param>
        /// <param name="baseDefenseBonus"></param>
        /// <param name="baseKiSkillDrainMultiplier"></param>
        /// <param name="baseKiDrainRate"></param>
        /// <param name="baseKiDrainRateMastery"></param>
        /// <param name="baseHealthDrainRate"></param>
        /// <param name="appearance"></param>
        /// <param name="buff"></param>
        /// <param name="duration"></param>
        /// <param name="buffIconGetter">The icon to display in the Transformation Menu; leave as null if you don't want to display the transformation.</param>
        /// <param name="failureText">The text displayed when the player fails to achieve (select in the menu) the transformation.</param>
        /// <param name="extraTooltipText"></param>
        /// <param name="canBeMastered">Whether the form has a mastery rating.</param>
        /// <param name="masterFormBuffKeyName">What form the buff's ki cost is associated with for mastery (like ASSJ and USSJ being SSJ1, for example)</param>
        /// <param name="transformationRequirements"></param>
        /// <param name="unlockRequirements">The requirements to unlock the form. Will be checked after <param name="parents">parents</param>.</param>
        /// <param name="selectionRequirementsFailed"></param>
        /// <param name="parents">The transformations that need to be unlocked before this transformation can also be unlocked.</param>
        protected TransformationDefinition(string unlocalizedName, string text, Color textColor,
            float baseDamageMultiplier, float baseSpeedMultiplier, int baseDefenseBonus, float baseKiSkillDrainMultiplier, float baseKiDrainRate, float baseKiDrainRateMastery, float baseHealthDrainRate,
            TransformationAppearanceDefinition appearance,
            Type buff, int duration = FormBuffHelper.ABSURDLY_LONG_BUFF_DURATION, Func<Texture2D> buffIconGetter = null, string failureText = null, string extraTooltipText = null, bool canBeMastered = false, string masterFormBuffKeyName = null, 
            Predicate<MyPlayer> unlockRequirements = null, Func<MyPlayer, TransformationDefinition, bool> selectionRequirementsFailed = null, bool requiresAllParents = true, params TransformationDefinition[] parents)
        {
            UnlocalizedName = unlocalizedName;
            Text = text;
            TextColor = textColor;

            BaseDamageMultiplier = baseDamageMultiplier;
            BaseSpeedMultiplier = baseSpeedMultiplier;
            BaseDefenseBonus = baseDefenseBonus;
            BaseKiSkillDrainMultiplier = baseKiSkillDrainMultiplier;
            BaseKiDrainRate = baseKiDrainRate;
            BaseKiDrainRateMastery = baseKiDrainRateMastery;
            BaseHealthDrainRate = baseHealthDrainRate;

            Buff = buff;
            Duration = duration;

            BuffIconGetter = buffIconGetter;

            FailureText = failureText;
            ExtraTooltipText = extraTooltipText;

            Appearance = appearance;

            if (unlockRequirements == null)
                unlockRequirements = p => true;

            UnlockRequirements = unlockRequirements;

            if (selectionRequirementsFailed == null)
                selectionRequirementsFailed = (p, t) => true;

            SelectionRequirementsFailed = selectionRequirementsFailed;

            HasMastery = canBeMastered;
            MasteryBuffKeyName = masterFormBuffKeyName;

            RequiresAllParents = requiresAllParents;
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

        #region Usability Methods

        internal bool PlayerHasTransformation(MyPlayer player) => player.PlayerTransformations.ContainsKey(this);

        internal bool PlayerHasTransformationAndNotLegendary(MyPlayer player) => PlayerHasTransformation(player) && !player.IsLegendary();

        /// <summary>Checks wether or not the player has all/any parents (depends on <see cref="RequiresAllParents"/>. If this check passes, it executes the <see cref="UnlockRequirements"/> for any specific requirements.</summary>
        /// <param name="player"></param>
        /// <returns>true if the palyer can unlock the transformation; otherwise false.</returns>
        public bool CanPlayerUnlock(MyPlayer player)
        {
            for (int i = 0; i < Parents.Length; i++)
            {
                if (RequiresAllParents & !player.HasTransformation(Parents[i]))
                    return false;

                if (!RequiresAllParents && player.HasTransformation(Parents[i]))
                    return true;
            }

            return UnlockRequirements.Invoke(player);
        }

        /// <summary>Checks wether or not the player unlocked the transformation and meets the transformation requirements in <see cref="MeetsTransformationRequirements"/>.</summary>
        /// <param name="player"></param>
        /// <returns>true if the player can transform into this transformation; otherwise false.</returns>
        public bool CanTransformInto(MyPlayer player) => player.HasTransformation(this) && MeetsTransformationRequirements(player);

        public virtual bool MeetsSelectionRequirements(MyPlayer player) => PlayerHasTransformation(player) && UnlockRequirements.Invoke(player);

        /// <summary>The specific transformation requirements, f.e. UI Omen requires the player to be low on health and fighting a boss with substantial health.</summary>
        /// <param name="player"></param>
        /// <returns>true if not overriden or if the player meets the transformation requirements; otherwise false.</returns>
        public virtual bool MeetsTransformationRequirements(MyPlayer player) => UnlockRequirements.Invoke(player);

        #endregion

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

        #region MyRegion

        public virtual void OnPlayerTransformed(MyPlayer player) { }

        /// <summary>Called whenever the player loses the buff associated to the transformation.</summary>
        /// <param name="player">The player in question.</param>
        /// <param name="buffIndex"></param>
        public virtual void OnTransformationBuffLost(MyPlayer player, ref int buffIndex) { }

        public virtual void OnMasteryGained(MyPlayer player, float mastery) { }

        public virtual void OnPlayerUpdate(MyPlayer player) { }

        public virtual void OnPlayerPreKill(MyPlayer player, List<NPC> activeBosses, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) { }

        #endregion

        public override string ToString() => UnlocalizedName;


        public string UnlocalizedName { get; }

        public string Text { get; }
        public Color TextColor { get; }

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

        public Type Buff { get; }
        public int Duration { get; }

        public Func<Texture2D> BuffIconGetter { get; }

        public string FailureText { get; }

        public string ExtraTooltipText { get; }

        internal Func<MyPlayer, TransformationDefinition, bool> SelectionRequirements { get; }

        internal Func<MyPlayer, TransformationDefinition, bool> SelectionRequirementsFailed { get; }


        public string MasteryBuffKeyName { get; }

        internal Predicate<MyPlayer> UnlockRequirements { get; }


        public bool RequiresAllParents { get; }

        public TransformationDefinition[] Parents { get; }

        // TODO Complete many transformations
        //protected virtual List<TransformationDefinition> CompatibleTransformations { get; } = new List<TransformationDefinition>();
    }
}
