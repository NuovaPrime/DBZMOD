using System.Collections.Generic;
using DBZMOD.Managers;
using DBZMOD.Transformations.Exhaustion;
using DBZMOD.Transformations.Kaiokens.Kaioken;
using DBZMOD.Transformations.Kaiokens.SuperKaioken;
using DBZMOD.Transformations.Kaiokens.KaoikenFatigue;
using DBZMOD.Transformations.LSSJ.LSSJ1;
using DBZMOD.Transformations.SSJ.SSJ1;
using DBZMOD.Transformations.SSJ.SSJ1.ASSJ;
using DBZMOD.Transformations.SSJ.SSJ1.USSJ;
using DBZMOD.Transformations.SSJ.SSJ2;
using DBZMOD.Transformations.SSJ.SSJ3;
using DBZMOD.Transformations.SSJ.SSJ4s.SSJ4;
using DBZMOD.Transformations.SSJ.SSJBs.SSJB;
using DBZMOD.Transformations.SSJ.SSJBs.SSJR;
using DBZMOD.Transformations.SSJ.SSJG;
using DBZMOD.Transformations.UltraInstincts.Omen;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations
{
    public class TransformationDefinitionManager : NoddedManager<TransformationDefinition>
    {
        private readonly List<TransformationDefinition> _kaiokens = new List<TransformationDefinition>();

        public static readonly Color 
            defaultTransformationTextColor = new Color(219, 219, 48),
            godTransformationTextColor = new Color(229, 20, 51),
            blueTransformationTextColor = new Color(51, 20, 229),
            roseTransformationTextColor = new Color(244, 69, 209),
            uiOmenTransformationTextColor = new Color(225, 255, 255);

        internal override void DefaultInitialize()
        {
            KaiokenDefinition = Add(new KaiokenTransformation(), true) as KaiokenTransformation;
            SuperKaiokenDefinition = Add(new SuperKaiokenTransformation(), true) as SuperKaiokenTransformation;
            KaiokenFatigueDefinition = Add(new KaiokenFatigueTransformation()) as KaiokenFatigueTransformation;

            SSJ1Definition = Add(new SSJ1Transformation()) as SSJ1Transformation;
            ASSJDefinition = Add(new ASSJTransformation(SSJ1Definition)) as ASSJTransformation;
            USSJDefinition = Add(new USSJTransformation(ASSJDefinition)) as USSJTransformation;

            SSJ2Definition = Add(new SSJ2Transformation(SSJ1Definition)) as SSJ2Transformation;
            SSJ3Definition = Add(new SSJ3Transformation(SSJ2Definition)) as SSJ3Transformation;
            SSJ4Definition = Add(new SSJ4Transformation(SSJ3Definition)) as SSJ4Transformation;

            SSJGDefinition = Add(new SSJGTransformation(SSJ3Definition)) as SSJGTransformation;
            SSJBDefinition = Add(new SSJBTransformation(SSJGDefinition)) as SSJBTransformation;
            SSJRDefinition = Add(new SSJRTransformation(SSJGDefinition)) as SSJRTransformation;
            /*SSJBDefinition = Add(new TransformationDefinition(BuffKeyNames.ssjb, null, defaultTransformationTextColor, null, "Set Text Here", true, BuffKeyNames.ssjb, null,
                selectionRequirementsFailed: (p, t) => !p.LSSJAchieved, parents: SSJ1Definition));*/

            // TODO Add SSJB and SSJR as parents.
            UIOmenTransformation = Add(new UIOmenTransformation()) as UIOmenTransformation;

            LSSJDefinition = Add(new LSSJ1Transformation(SSJ1Definition)) as LSSJ1Transformation;

            TransformationExhaustionDefinition = Add(new TransformationExhaustionTransformation()) as TransformationExhaustionTransformation;

            base.DefaultInitialize();
        }

        public TransformationDefinition Add(TransformationDefinition item, bool isKaioken)
        {
            TransformationDefinition transformation = base.Add(item);

            if (item == transformation)
                _kaiokens.Add(item);

            return transformation;
        }

        public bool IsKaioken(TransformationDefinition transformation) => _kaiokens.Contains(transformation);

        public bool IsKaioken(IList<TransformationDefinition> transformations)
        {
            for (int i = 0; i < transformations.Count; i++)
                if (IsKaioken(transformations[i]))
                    return true;

            return false;
        }

        public KaiokenTransformation KaiokenDefinition { get; private set; }
        public SuperKaiokenTransformation SuperKaiokenDefinition { get; private set; }
        public KaiokenFatigueTransformation KaiokenFatigueDefinition { get; private set; }

        public SSJ1Transformation SSJ1Definition { get; private set; } 
        public ASSJTransformation ASSJDefinition { get; private set; }
        public USSJTransformation USSJDefinition { get; private set; }

        public SSJ2Transformation SSJ2Definition { get; private set; }
        public SSJ3Transformation SSJ3Definition { get; private set; }
        public SSJ4Transformation SSJ4Definition { get; private set; }

        public SSJGTransformation SSJGDefinition { get; private set; }
        public SSJBTransformation SSJBDefinition { get; private set; }
        public SSJRTransformation SSJRDefinition { get; private set; }

        public UIOmenTransformation UIOmenTransformation { get; private set; }

        public LSSJ1Transformation LSSJDefinition { get; private set; }

        public TransformationExhaustionTransformation TransformationExhaustionDefinition { get; private set; }
    }
}