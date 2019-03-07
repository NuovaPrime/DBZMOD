using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations.UltraInstincts.Omen
{
    public sealed class UIOmenTransformation : TransformationDefinition
    {
        // TODO Add methods for OnTransformationExit
        public UIOmenTransformation(params TransformationDefinition[] parents) : base("Ultra Instinct Omen", TransformationDefinitionManager.uiOmenTransformationTextColor,
            10.0f, 10.0f, 0, 5f, 1000f / 60, 1000f / 60, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.uiOmenAura, new ReadOnlyColor(0.3f, 0.3f, 0.3f), 
                new HairAppearance(null, new ReadOnlyColor(225, 255, 255), 1, null), Color.Silver),
            typeof(UIOmenBuff), duration: 25 * 60,
            buffIconGetter: () => GFX.uiOmenButtonImage, hasMenuIcon: true,
            canBeMastered: true,
            requiresAllParents: false, parents: parents)
        {
        }

        public override void OnTransformationBuffExpired(MyPlayer player, ref int buffIndex)
        {
            int buffTime = player.player.buffTime[buffIndex];

            if (buffTime == 0)
            {
                player.player.statLife = 1;
                player.player.lifeRegen = 0;
                player.player.lifeRegenTime = 0;
            }
        }
    }

    public sealed class UIOmenBuff : TransformationBuff
    {
        public UIOmenBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.UIOmenTransformation)
        {
        }
    }
}
