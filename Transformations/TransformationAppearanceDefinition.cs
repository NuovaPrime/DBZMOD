using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Models;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations
{
    public struct TransformationAppearanceDefinition
    {
        public readonly AuraAnimationInfo auraAnimation;
        public readonly ReadOnlyColor lightColor;
        public readonly HairAppearance hair;
        public readonly Color? eyeColor;

        public TransformationAppearanceDefinition(AuraAnimationInfo auraAnimation, ReadOnlyColor lightColor, HairAppearance hair, Color? eyeColor)
        {
            this.auraAnimation = auraAnimation;
            this.lightColor = lightColor;
            this.hair = hair;
            this.eyeColor = eyeColor;
        }

        public TransformationAppearanceDefinition(AuraAnimationInfo auraAnimation, ReadOnlyColor lightColor, string hairTexture, ReadOnlyColor hairColor, int? hairShader, Color? eyeColor)
        {
            this.auraAnimation = auraAnimation;
            this.lightColor = lightColor;
            this.hair = new HairAppearance(hairTexture, hairColor, hairShader);
            this.eyeColor = eyeColor;
        }
    }

    public class ReadOnlyColor
    {
        public readonly float
            red, green, blue;

        public ReadOnlyColor(float red, float green, float blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public static implicit operator Color(ReadOnlyColor color)
        {
            return new Color((int) color.red, (int)color.green, (int)color.blue);
        }
    }

    public class HairAppearance
    {
        public readonly string hairTexture;
        public readonly ReadOnlyColor hairColor;
        public readonly int? hairShader;

        public HairAppearance(string hairTexture, ReadOnlyColor hairColor, int? hairShader)
        {
            this.hairTexture = hairTexture;
            this.hairColor = hairColor;
            this.hairShader = hairShader;
        }
    }
}