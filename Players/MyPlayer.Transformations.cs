using System;
using System.Collections.Generic;
using DBZMOD.Transformations;

namespace DBZMOD
{
    public partial class MyPlayer
    {
        internal Dictionary<TransformationDefinition, PlayerTransformation> PlayerTransformations { get; private set; }

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
        public bool LSSJAchieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.LSSJDefinition);
        [Obsolete]
        public bool LSSJ2Achieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.LSSJ2Definition);

        [Obsolete]
        public bool SSJGAchieved => HasTransformation(DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition);
    }
}
