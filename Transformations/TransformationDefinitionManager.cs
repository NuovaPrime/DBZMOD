using System.Collections.Generic;
using DBZMOD.Buffs;
using DBZMOD.Dynamicity;
using DBZMOD.Enums;
using DBZMOD.Managers;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations
{
    public class TransformationDefinitionManager : NoddedManager<TransformationDefinition>
    {
        public static readonly Color 
            defaultTransformationTextColor = new Color(219, 219, 48),
            godTransformationTextColor = new Color(229, 20, 51);

        internal override void DefaultInitialize()
        {
            KaiokenDefinition = Add(new TransformationDefinition(BuffKeyNames.kaioken, null, defaultTransformationTextColor));
            SuperKaiokenDefinition = Add(new TransformationDefinition(BuffKeyNames.superKaioken, null, defaultTransformationTextColor, canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.ssj1));
            KaiokenFatigueDefinition = Add(new TransformationDefinition(BuffKeyNames.kaiokenFatigue, null, defaultTransformationTextColor));

            SSJ1Definition = Add(new TransformationDefinition(BuffKeyNames.ssj1, "Super Saiyan", defaultTransformationTextColor, GFX.ssj1ButtonImage, "Only through failure with a powerful foe will true power awaken.", true, BuffKeyNames.ssj1));
            ASSJDefinition = Add(new TransformationDefinition(BuffKeyNames.assj, FormBuffHelper.GetASSJNamePreference(), defaultTransformationTextColor, canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.ssj1, parents: SSJ1Definition));
            USSJDefinition = Add(new TransformationDefinition(BuffKeyNames.ussj, FormBuffHelper.GetUSSJNamePreference(), defaultTransformationTextColor, canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.ssj1, parents: ASSJDefinition));

            SSJ2Definition = Add(new TransformationDefinition(BuffKeyNames.ssj2, "Super Saiyan 2", defaultTransformationTextColor, GFX.ssj2ButtonImage, "One may awaken their true power through extreme pressure while ascended.", true, BuffKeyNames.ssj2, 
                p => p.SSJ1Achived && !p.IsLegendary(), parents: SSJ1Definition));
            SSJ3Definition = Add(new TransformationDefinition(BuffKeyNames.ssj3, "Super Saiyan 3", defaultTransformationTextColor, GFX.ssj3ButtonImage, "The power of an ancient foe may be the key to unlocking greater power.", true, BuffKeyNames.ssj3, 
                p => p.SSJ2Achieved && !p.IsLegendary(), parents: SSJ2Definition));

            SSJGDefinition = Add(new TransformationDefinition(BuffKeyNames.ssjg, "Super Saiyan God", godTransformationTextColor, GFX.ssjgButtonImage, "The godlike power of the lunar star could awaken something beyond mortal comprehension.", true, BuffKeyNames.ssjg,
                p => !p.IsLegendary(), selectionRequirements: (p, t) => t.PlayerHasTransformationAndNotLegendary(p), parents: SSJ1Definition));
            SSJBDefinition = Add(new TransformationDefinition(BuffKeyNames.ssjb, null, defaultTransformationTextColor, null, "Set Text Here", true, BuffKeyNames.ssjb, null,
                selectionRequirementsFailed: (p, t) => !p.LSSJAchieved, parents: SSJ1Definition));

            LSSJDefinition = Add(new TransformationDefinition(BuffKeyNames.lssj, "Legendary Super Saiyan", defaultTransformationTextColor, GFX.lssjButtonImage, unlockRequirements: p => p.IsLegendary(), canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.lssj, parents: SSJ1Definition));
            LSSJ2Definition = Add(new TransformationDefinition(BuffKeyNames.lssj2, "Legendary Super Saiyan 2", defaultTransformationTextColor, GFX.lssj2ButtonImage, canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.lssj2, parents: LSSJDefinition));

            TransformationExhaustionDefinition = Add(new TransformationDefinition(BuffKeyNames.transformationExhaustion, null, defaultTransformationTextColor));
            SpectrumDefinition = Add(new TransformationDefinition(BuffKeyNames.spectrum, "Super Saiyan Spectrum", defaultTransformationTextColor, canBeMastered: true, masterFormBuffKeyName: BuffKeyNames.spectrum,
                selectionRequirements: (p, t) => p.player.name == "Nuova", selectionRequirementsFailed: (p, t) => false));
        }

        public TransformationDefinition KaiokenDefinition { get; private set; }
        public TransformationDefinition SuperKaiokenDefinition { get; private set; }
        public TransformationDefinition KaiokenFatigueDefinition { get; private set; }

        public TransformationDefinition SSJ1Definition { get; private set; } 
        public TransformationDefinition ASSJDefinition { get; private set; }
        public TransformationDefinition USSJDefinition { get; private set; }

        public TransformationDefinition SSJ2Definition { get; private set; }
        public TransformationDefinition SSJ3Definition { get; private set; }

        public TransformationDefinition SSJGDefinition { get; private set; }
        public TransformationDefinition SSJBDefinition { get; private set; }

        public TransformationDefinition LSSJDefinition { get; private set; }
        public TransformationDefinition LSSJ2Definition { get; private set; }

        public TransformationDefinition TransformationExhaustionDefinition { get; private set; }
        public TransformationDefinition SpectrumDefinition { get; private set; }
    }
}
