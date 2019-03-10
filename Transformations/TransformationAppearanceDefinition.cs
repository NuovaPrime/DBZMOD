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

        public TransformationAppearanceDefinition(AuraAnimationInfo auraAnimation, ReadOnlyColor lightColor, string hairTexture, ReadOnlyColor hairColor, int? hairShader, string hairName, Color? eyeColor)
        {
            this.auraAnimation = auraAnimation;
            this.lightColor = lightColor;
            this.hair = new HairAppearance(hairTexture, hairColor, hairShader, hairName);
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

        public static ReadOnlyColor DefaultAuraLighting(Color color) => new ReadOnlyColor(color.R / 100f, color.G / 100f, color.B / 100f);

        public static ReadOnlyColor DefaultHairColor(Color color) => new ReadOnlyColor(color.R / 255f, color.G / 255f, color.B / 255f);

        public static implicit operator Color(ReadOnlyColor color)
        {
            return new Color((int) color.red, (int)color.green, (int)color.blue);
        }
    }

    public class HairAppearance
    {
        public const string
            BASE_HAIRSTYLE_KEY = "baseHairStyle",
            SSJ1_HAIRSTYLE_KEY = "ssj1HairStyle",
            SSJ2_HAIRSTYLE_KEY = "ssj2HairStyle",
            SSJ3_HAIRSTYLE_KEY = "ssj3HairStyle",
            SSJ4_HAIRSTYLE_KEY = "ssj4HairStyle";

        public readonly string hairTexture;
        public readonly ReadOnlyColor hairColor;
        public readonly int? hairShader;
        public readonly string hairName;

        public HairAppearance(string hairTexture, ReadOnlyColor hairColor, int? hairShader, string hairName)
        {
            this.hairTexture = hairTexture;
            this.hairColor = hairColor;
            this.hairShader = hairShader;
            this.hairName = hairName;
        }
    }
}