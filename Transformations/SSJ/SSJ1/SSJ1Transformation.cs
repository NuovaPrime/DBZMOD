using DBZMOD.Buffs;
using DBZMOD.Effects.Animations.Aura;
using Microsoft.Xna.Framework;
using Terraria;

namespace DBZMOD.Transformations.SSJ.SSJ1
{
    public sealed class SSJ1Transformation : TransformationDefinition
    {
        public const float
            LIGHTING_RED = 0.2f,
            LIGHTING_GREEN = 0.2f,
            LIGHTING_BLUE = 0f;

        public SSJ1Transformation() : base("Super Saiyan", TransformationDefinitionManager.defaultTransformationTextColor, 
            1.50f, 1.50f, 4, 1.25f, 1, 0.5f, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.ssj1Aura, new ReadOnlyColor(LIGHTING_RED, LIGHTING_GREEN, LIGHTING_BLUE), 
                new HairAppearance("Hairs/SSJ1/SSJ1Hair", null, null, HairAppearance.SSJ1_HAIRSTYLE_KEY), Color.Turquoise), 
            typeof(SSJ1Buff),
            buffIconGetter: () => GFX.ssj1ButtonImage, hasMenuIcon: true,
            failureText: "Only through failure with a powerful foe will true power awaken.", canBeMastered: true)
        {
        }

        public override void OnMasteryGained(MyPlayer player, float mastery)
        {
            if (mastery >= 0.5f && !player.ASSJAchieved)
            {
                DBZMOD.Instance.TransformationDefinitionManager.ASSJDefinition.TryUnlock(player);
                Main.NewText("Your SSJ1 Mastery has been upgraded.\nHold charge and transform while in SSJ1\nto ascend.");
            }
            else if (mastery >= 0.75f && !player.USSJAchieved)
            {
                DBZMOD.Instance.TransformationDefinitionManager.USSJDefinition.Unlock(player);
                Main.NewText("Your SSJ1 Mastery has been upgraded.\nHold charge and transform while in ASSJ\nto ascend.");
            }
        }
    }

    public sealed class SSJ1Buff : TransformationBuff
    {
        public SSJ1Buff() : base(DBZMOD.Instance.TransformationDefinitionManager.SSJ1Definition)
        {
        }
    }
}
