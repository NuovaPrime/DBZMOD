using System;
using System.Collections.Generic;
using DBZMOD.Dynamicity;
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
        [Obsolete("This method will be deprecated in favor of IsTransformedInto() and ActiveTransformations[].")]
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
            if (transformation == null || IsTransformBlocked() || IsExhaustedFromTransformation() || IsTransformedInto(transformation))
                return false;

            return transformation.CanTransformInto(this);
        }

        public void AddTransformation(TransformationDefinition transformation, int duration)
        {
            player.AddBuff(transformation.GetBuffId(), duration, false);
            ActiveTransformations.Add(transformation);

            transformation.OnPlayerTransformed(this);

            if (!string.IsNullOrWhiteSpace(transformation.Text))
                CombatText.NewText(player.Hitbox, transformation.TextColor, transformation.Text, false, false);

            if (!Main.dedServ && Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
                NetworkHelper.formSync.SendFormChanges(256, player.whoAmI, player.whoAmI, transformation.UnlocalizedName, duration);

            isTransformationAnimationPlaying = true;
        }

        public void DoTransform(TransformationDefinition transformation)
        {
            // don't.. try to apply the same transformation. This just stacks projectile auras and looks dumb.
            if (IsTransformedInto(transformation)) return;

            // make sure to swap kaioken with super kaioken when appropriate.
            if (transformation == TransformationDefinitionManager.SuperKaiokenDefinition)
                RemoveTransformation(TransformationDefinitionManager.KaiokenDefinition);

            // remove all *transformation* buffs from the player.
            // this needs to know we're powering down a step or not
            EndTransformations();

            // add whatever buff it is for a really long time.
            AddTransformation(transformation, transformation.Duration);
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
            for (int i = 0; i < ActiveTransformations.Count; i++)
                RemoveTransformation(ActiveTransformations[i]);
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

            // TODO Handle all of the multi-transformation aspects.
            // player has just pressed the normal transform button one time, which serves two functions.
            if (IsTransformingUpOneStep())
            {
                if (transformation != null)
                {
                    NodeTree<TransformationDefinition> tree = TransformationDefinitionManager.Tree;
                    ManyToManyNode<TransformationDefinition> mtmn = tree[transformation];

                    if (mtmn.Next.Count == 0) return;
                    
                    // Find the first available transformation.
                    for (int i = 0; i < mtmn.Next.Count; i++)
                    {
                        // TODO Verify this is the actual method to use.
                        if (CanTransform(mtmn.Next[i]))
                        {
                            targetTransformation = mtmn.Next[i];
                            break;
                        }
                    }

                    // TODO Dynamicize this
                    // player is ascending transformation, pushing for ASSJ or USSJ depending on what form they're in.
                    /*if (IsAscendingTransformation() && CanAscend())
                    {
                        if (transformation == TransformationDefinitionManager.SSJ1Definition)
                            targetTransformation = TransformationDefinitionManager.ASSJDefinition;
                        else if (transformation == TransformationDefinitionManager.ASSJDefinition)
                            targetTransformation = TransformationDefinitionManager.USSJDefinition;
                    }*/
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
            else if (IsPoweringDownOneStep() && IsPlayerTransformed() && transformation != TransformationDefinitionManager.KaiokenDefinition)
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

        #region Shortcut Properties and Methods

        public bool IsPlayerTransformed() => ActiveTransformations.Count > 0;

        public bool IsTransformedInto(TransformationDefinition transformation) => ActiveTransformations.Contains(transformation);

        public bool IsTransformBlocked() => isTransforming || IsPlayerImmobilized() || IsKiDepleted();

        #endregion

        public bool IsExhaustedFromTransformation() => player.HasBuff(DBZMOD.Instance.TransformationDefinitionManager.TransformationExhaustionDefinition.GetBuffId());
        public bool IsTiredFromKaioken() => player.HasBuff(DBZMOD.Instance.TransformationDefinitionManager.KaiokenFatigueDefinition.GetBuffId());

        public List<TransformationDefinition> ActiveTransformations { get; } = new List<TransformationDefinition>();

        internal Dictionary<TransformationDefinition, PlayerTransformation> PlayerTransformations { get; private set; }

        internal TransformationDefinitionManager TransformationDefinitionManager => DBZMOD.Instance.TransformationDefinitionManager;
    }
}
