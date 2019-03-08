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

        // TODO Remove this and make it unlock correctly.
        public override void OnPlayerUnlockTransformation(MyPlayer player, TransformationDefinition transformation)
        {
            if (transformation == player.TransformationDefinitionManager.SSJ3Definition)
                TryUnlock(player);
        }
    }

    public sealed class MysticBuff : TransformationBuff
    {
        public MysticBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.MysticDefinition)
        {
        }
    }
}
