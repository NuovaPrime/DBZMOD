using System;
using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBZMOD.Transformations.Mystic
{
    public sealed class MysticTransformation : TransformationDefinition
    {
        public MysticTransformation(params TransformationDefinition[] parents) : base("Mystic", TransformationDefinitionManager.uiOmenTransformationTextColor, 
            2.9f, 2.9f, 12, 1f, 0f, 0f, 0f, 
            new TransformationAppearanceDefinition(AuraAnimations.mysticAura, new ReadOnlyColor(0.6f, 0.6f, 0.6f), null, null),
            typeof(MysticBuff),
            exhaustsPlayer: false, buffIconGetter: () => GFX.mysticButtonImage, hasMenuIcon: true, canBeMastered: true, parents: parents)
        {
        }
    }

    public sealed class MysticBuff : TransformationBuff
    {
        public MysticBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.MysticDefinition)
        {
        }
    }
}
