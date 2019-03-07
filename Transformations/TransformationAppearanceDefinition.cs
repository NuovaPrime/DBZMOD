using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Managers;
using DBZMOD.Models;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Transformations
{
    public struct TransformationAppearanceDefinition
    {
        public readonly AuraAnimationInfo auraAnimation;
        public readonly ReadOnlyColor lightColor;
        public readonly HairAppearance hair;
        public readonly int? hairStyle;
        public readonly Color? eyeColor;

        public TransformationAppearanceDefinition(AuraAnimationInfo auraAnimation, ReadOnlyColor lightColor, HairAppearance hair, int hairStyle, Color? eyeColor)
        {
            this.auraAnimation = auraAnimation;
            this.lightColor = lightColor;
            this.hair = hair;
            this.hairStyle = hairStyle;
            this.eyeColor = eyeColor;
        }

        public TransformationAppearanceDefinition(AuraAnimationInfo auraAnimation, ReadOnlyColor lightColor, string hairTexture, ReadOnlyColor hairColor, int? hairShader, int? hairStyle, Color? eyeColor)
        {
            this.auraAnimation = auraAnimation;
            this.lightColor = lightColor;
            this.hair = new HairAppearance(hairTexture, hairColor, hairShader);
            this.hairStyle = hairStyle;
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

    public class HairStyleAppearance : NoddedManager<TransformationDefinition>
    {
        public readonly int? hairStyle;

        public HairStyleAppearance(int? hairStyle)
        {
            this.hairStyle = hairStyle;
        }
        MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();

        //to-do, change this to be not shit
        #region Hair Style Checks
        public int GetBaseStyle()
        {
            return modPlayer.baseHairStyle;
        }
        public int GetSSJ1Style()
        {
            return modPlayer.ssj1HairStyle;
        }
        public int GetSSJ2Style()
        {
            return modPlayer.ssj2HairStyle;
        }
        public int GetSSJ3Style()
        {
            return modPlayer.ssj3HairStyle;
        }
        public int GetSSJ4Style()
        {
            return modPlayer.ssj4HairStyle;
        }
        public int GetSSJGStyle()
        {
            return modPlayer.baseHairStyle;
        }
        public int GetSSJBStyle()
        {
            return modPlayer.ssj1HairStyle;
        }
        public int GetSSJRStyle()
        {
            return modPlayer.ssj1HairStyle;
        }
        public int GetASSJStyle()
        {
            return modPlayer.ssj1HairStyle;
        }
        public int GetSSJCStyle()
        {
            return modPlayer.ssj1HairStyle;
        }
        public int GetLSSJStyle()
        {
            return modPlayer.ssj2HairStyle;
        }
        public int GetSSJBEStyle()
        {
            return modPlayer.ssj2HairStyle;
        }
        public int GetSSJKKStyle()
        {
            return modPlayer.ssj1HairStyle;
        }
        public int GetSSJBKKStyle()
        {
            return modPlayer.ssj1HairStyle;
        }
        public int GetSSJRKKStyle()
        {
            return modPlayer.ssj1HairStyle;
        }

        #endregion


        internal override void DefaultInitialize()
        {
            SSJRHairStyle = GetSSJRStyle();
            SSJBHairStyle = GetSSJBStyle();
            SSJGHairStyle = GetSSJGStyle();
            SSJ4HairStyle = GetSSJ4Style();
            SSJ3HairStyle = GetSSJ3Style();
            SSJ2HairStyle = GetSSJ2Style();
            SSJ1HairStyle = GetSSJ1Style();
            ASSJHairStyle = GetASSJStyle();
            USSJHairStyle = GetASSJStyle();
            LSSJHairStyle = GetLSSJStyle();
            SSJKKHairStyle = GetSSJKKStyle();

            base.DefaultInitialize();
        }


        public static int SSJ1HairStyle { get; private set; }
        public static int ASSJHairStyle { get; private set; }
        public static int USSJHairStyle { get; private set; }

        public static int SSJKKHairStyle { get; private set; }

        public static int LSSJHairStyle { get; private set; }

        public static int SSJ2HairStyle { get; private set; }
        public static int SSJ3HairStyle { get; private set; }
        public static int SSJ4HairStyle { get; private set; }

        public static int SSJGHairStyle { get; private set; }
        public static int SSJBHairStyle { get; private set; }
        public static int SSJRHairStyle { get; private set; }
    }
}