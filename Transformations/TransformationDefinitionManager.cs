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
            KaiokenDefinition = Add(new TransformationDefinition(MenuSelectionID.None, BuffKeyNames.kaioken, null, defaultTransformationTextColor, null));
            SuperKaiokenDefinition = Add(new TransformationDefinition(MenuSelectionID.None, BuffKeyNames.superKaioken, null, defaultTransformationTextColor, null, true, BuffKeyNames.ssj1));
            KaiokenFatigueDefinition = Add(new TransformationDefinition(MenuSelectionID.None, BuffKeyNames.kaiokenFatigue, null, defaultTransformationTextColor, null));

            SSJ1Definition = Add( new TransformationDefinition(MenuSelectionID.SSJ1, BuffKeyNames.ssj1, "Super Saiyan 1", defaultTransformationTextColor, Gfx.ssj1ButtonImage, true, BuffKeyNames.ssj1));
            ASSJDefinition = Add(new TransformationDefinition(MenuSelectionID.None, BuffKeyNames.assj, FormBuffHelper.GetASSJNamePreference(), defaultTransformationTextColor, null, true, BuffKeyNames.ssj1, null, SSJ1Definition));
            USSJDefinition = Add( new TransformationDefinition(MenuSelectionID.None, BuffKeyNames.ussj, FormBuffHelper.GetUSSJNamePreference(), defaultTransformationTextColor, null, true, BuffKeyNames.ssj1, null, ASSJDefinition));

            SSJ2Definition = Add(new TransformationDefinition(MenuSelectionID.SSJ2, BuffKeyNames.ssj2, "Super Saiyan 2", defaultTransformationTextColor, Gfx.ssj2ButtonImage, true, BuffKeyNames.ssj2, null, SSJ1Definition));
            SSJ3Definition = Add(new TransformationDefinition(MenuSelectionID.SSJ3, BuffKeyNames.ssj3, "Super Saiyan 3", defaultTransformationTextColor, Gfx.ssj3ButtonImage, true, BuffKeyNames.ssj3, null, SSJ2Definition));

            SSJGDefinition = Add(new TransformationDefinition(MenuSelectionID.SSJG, BuffKeyNames.ssjg, "Super Saiyan God", godTransformationTextColor, Gfx.ssjgButtonImage, true, BuffKeyNames.ssjg, null, SSJ1Definition));
            SSJBDefinition = Add(new TransformationDefinition(MenuSelectionID.Ssjb, BuffKeyNames.ssjb, null, defaultTransformationTextColor, null, true, BuffKeyNames.ssjb, null, SSJ1Definition));

            LSSJDefinition = Add(new TransformationDefinition(MenuSelectionID.LSSJ1, BuffKeyNames.lssj, "Legendary Super Saiyan", defaultTransformationTextColor, Gfx.lssjButtonImage, true, BuffKeyNames.lssj));
            LSSJ2Definition = Add(new TransformationDefinition(MenuSelectionID.LSSJ2, BuffKeyNames.lssj2, "Legendary Super Saiyan 2", defaultTransformationTextColor, Gfx.lssj2ButtonImage, true, BuffKeyNames.lssj2, null, LSSJDefinition));

            TransformationExhaustionDefinition = Add(new TransformationDefinition(MenuSelectionID.None, BuffKeyNames.transformationExhaustion, null, defaultTransformationTextColor, null));
            SpectrumDefinition = Add(new TransformationDefinition(MenuSelectionID.Spectrum, BuffKeyNames.spectrum, "Super Saiyan Spectrum", defaultTransformationTextColor, null, true, BuffKeyNames.spectrum));
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
