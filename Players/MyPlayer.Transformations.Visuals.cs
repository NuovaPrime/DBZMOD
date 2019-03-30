using System.Collections.Generic;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Models;
using Terraria;

namespace DBZMOD
{
    public partial class MyPlayer
    {
        /// <summary>
        ///     Return the aura effect currently active on the player.
        /// </summary>
        /// <param name="player">The player being checked</param>
        public AuraAnimationInfo GetAuraEffectOnPlayer()
        {
            if (player.dead)
                return null;

            if (ActiveTransformations.Count > 0)
                return ActiveTransformations[0].Appearance.auraAnimation;

            if (isCharging && ActiveTransformations.Count == 0)
                return AuraAnimations.chargeAura;

            return null;
        }
    }
}
