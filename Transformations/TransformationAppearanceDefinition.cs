using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Models;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations
{
    public struct TransformationAppearanceDefinition
    {
        public readonly AuraAnimationInfo auraAnimationInfo;
        public readonly ReadOnlyColor lightColor, hairColor;
        public readonly string hairTexture;
        public readonly int? hairShader;
        public readonly Color? eyeColor;

        public TransformationAppearanceDefinition(AuraAnimationInfo auraAnimationInfo, ReadOnlyColor lightColor, string hairTexture, ReadOnlyColor hairColor, int? hairShader, Color? eyeColor)
        {
            this.auraAnimationInfo = auraAnimationInfo;
            this.lightColor = lightColor;
            this.hairTexture = hairTexture;
            this.hairColor = hairColor;
            this.hairShader = hairShader;
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
}