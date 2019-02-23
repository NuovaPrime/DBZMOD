using System;
using System.Collections.Generic;
using DBZMOD.Network;
using DBZMOD.Transformations;
using DBZMOD.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD
{
    public partial class MyPlayer
    {
        public TransformationDefinition GetCurrentTransformation()
        {
            for (int i = 0; i < player.buffType.Length; i++)
            {
                ModBuff buff = BuffLoader.GetBuff(player.buffType[i]);
                TransformationBuff transBuff = buff as TransformationBuff;

                if (transBuff != null)
                    return transBuff.TransformationDefinition;
            }

            return null;
        }

        public bool CanTransform(TransformationDefinition transformation)
        {
            if (transformation == null || IsTransformBlocked() || IsExhaustedFromTransformation())
                return false;

            return transformation.MeetsSelectionRequirements(this);
        }

        public void AddTransformation(TransformationDefinition transformation, int duration)
        {
            player.AddBuff(transformation.GetBuffId(), duration, false);

            if (!string.IsNullOrWhiteSpace(transformation.TransformationText))
                CombatText.NewText(player.Hitbox, transformation.TransformationTextColor, transformation.TransformationText, false, false);

            if (!Main.dedServ && Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
                NetworkHelper.formSync.SendFormChanges(256, player.whoAmI, player.whoAmI, transformation.MasteryBuffKeyName, duration);

            isTransformationAnimationPlaying = true;
        }

        public void DoTransform(TransformationDefinition transformation)
        {
            // don't.. try to apply the same transformation. This just stacks projectile auras and looks dumb.
            if (transformation == GetCurrentTransformation()) return;

            // make sure to swap kaioken with super kaioken when appropriate.
            if (transformation == DBZMOD.Instance.TransformationDefinitionManager.SuperKaiokenDefinition)
                RemoveTransformation(DBZMOD.Instance.TransformationDefinitionManager.KaiokenDefinition);

            // remove all *transformation* buffs from the player.
            // this needs to know we're powering down a step or not
            EndTransformations();

            // add whatever buff it is for a really long time.
            AddTransformation(transformation, FormBuffHelper.ABSURDLY_LONG_BUFF_DURATION);
        }

        public void EndTransformations()
        { 
            ClearAllTransformations();
            isTransformationAnimationPlaying = false;
            transformationFrameTimer = 0;

            isTransforming = false;
        }

        public void ClearAllTransformations()
        {
            TransformationDefinition transformation = GetCurrentTransformation();

            if (transformation != null)
                RemoveTransformation(transformation);
        }

        public void RemoveTransformation(TransformationDefinition transformation)
        {
            player.ClearBuff(transformation.GetBuffId());

            if (!Main.dedServ && Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
                NetworkHelper.formSync.SendFormChanges(256, player.whoAmI, player.whoAmI, transformation.UnlocalizedName, 0);
        }

        public void HandleTransformations()
        {
            TransformationDefinition targetTransformation = null;
            TransformationDefinition transformation = GetCurrentTransformation();

            // player has just pressed the normal transform button one time, which serves two functions.
            if (IsTransformingUpOneStep())
            {
                if (transformation != null)
                {
                    // TODO Dynamicize this
                    // player is ascending transformation, pushing for ASSJ or USSJ depending on what form they're in.
                    if (IsAscendingTransformation() && CanAscend())
                    {
                        if (transformation == TransformationDefinitionManager.SSJ1Definition)
                            targetTransformation = TransformationDefinitionManager.ASSJDefinition;
                        else if (transformation == TransformationDefinitionManager.ASSJDefinition)
                            targetTransformation = TransformationDefinitionManager.USSJDefinition;
                    }
                }
                else
                {
                    // first attempt to step up to the selected form in the menu.
                    targetTransformation = UI.TransformationMenu.SelectedTransformation;
                }

                // TODO Add this again maybe ?
                // if for any reason we haven't gotten our transformation target, try the next step instead.
                /*if (targetTransformation == null)
                {
                    targetTransformation = player.GetNextTransformationStep();
                }*/
            }
            else if (IsPoweringDownOneStep() && transformation != TransformationDefinitionManager.KaiokenDefinition)
            {
                if (transformation.Parents.Length == 1)
                    // player is powering down a transformation state.
                    targetTransformation = transformation.Parents[0];
            }

            // if we made it this far without a target, it means for some reason we can't change transformations.
            if (targetTransformation == null)
                return;

            // finally, check that the transformation is really valid and then do it.
            if (CanTransform(targetTransformation))
                DoTransform(targetTransformation);
        }

        #region Transformation Achieved Hooks

        [Obsolete]
        public bool SSJ1Achived => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition);

        [Obsolete]
        public bool ASSJAchieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.ASSJDefinition);

        [Obsolete]
        public bool USSJAchieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.USSJDefinition);

        [Obsolete]
        public bool SSJ2Achieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJ2Definition);

        [Obsolete]
        public bool SSJ3Achieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJ3Definition);

        [Obsolete]
        public bool SSJ4Achieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJ4Definition);

        [Obsolete]
        public bool LSSJAchieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition);

        [Obsolete]
        public bool SSJGAchieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition);

        #endregion

        public bool IsPlayerTransformed() => GetCurrentTransformation() != null;

        public bool IsPlayerTransformation(TransformationDefinition transformation) => GetCurrentTransformation() == transformation;

        public bool IsTransformBlocked() => isTransforming || IsPlayerImmobilized() || IsKiDepleted();

        public bool IsExhaustedFromTransformation() => player.HasBuff(DBZMOD.Instance.TransformationDefinitionManager.TransformationExhaustionDefinition.GetBuffId());
        public bool IsTiredFromKaioken() => player.HasBuff(DBZMOD.Instance.TransformationDefinitionManager.KaiokenFatigueDefinition.GetBuffId());

        internal Dictionary<TransformationDefinition, PlayerTransformation> PlayerTransformations { get; private set; }

        internal TransformationDefinitionManager TransformationDefinitionManager => DBZMOD.Instance.TransformationDefinitionManager;
    }
}
