using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace DBZMOD
{
    public static class GFX
    {

        private const string GUI_DIRECTORY = "GFX/";
        private const string UI_DIRECTORY = "UI/";
        private const string BUTTON_DIRECTORY = "UI/Buttons/";
        private const string KIBAR = UI_DIRECTORY + "KiBar";
        private const string OVERLOADBAR = UI_DIRECTORY + "OverloadBar";
        private const string KIBARLIGHTNING = UI_DIRECTORY + "KiBarLightning";
        private const string BACKPANEL = UI_DIRECTORY + "BackPanel";
        private const string WISHBACKPANEL = UI_DIRECTORY + "WishBackPanel";
        private const string GRANTBUTTON = BUTTON_DIRECTORY + "GrantButton";
        private const string SSJ1BUTTON = BUTTON_DIRECTORY + "SSJ1ButtonImage";
        private const string SSJ2BUTTON = BUTTON_DIRECTORY + "SSJ2ButtonImage";
        private const string SSJ3BUTTON = BUTTON_DIRECTORY + "SSJ3ButtonImage";
        private const string SSJGBUTTON = BUTTON_DIRECTORY + "SSJGButtonImage";
        private const string LSSJBUTTON = BUTTON_DIRECTORY + "LSSJButtonImage";
        private const string LSSJ2BUTTON = BUTTON_DIRECTORY + "LSSJ2ButtonImage";
        private const string SSJSBUTTON = BUTTON_DIRECTORY + "SSJSButtonImage";
        private const string WISHFORPOWER = BUTTON_DIRECTORY + "WishforPower";
        private const string WISHFORWEALTH = BUTTON_DIRECTORY + "WishforWealth";
        private const string WISHFORIMMORTALITY = BUTTON_DIRECTORY + "WishforImmortality";
        private const string WISHFORGENETICS = BUTTON_DIRECTORY + "WishforGenetics";
        private const string WISHFORSKILL = BUTTON_DIRECTORY + "WishforSkill";
        private const string WISHFORAWAKENING = BUTTON_DIRECTORY + "WishforAwakening";
        private const string BG = UI_DIRECTORY + "Bg";
        private const string LOCKED = UI_DIRECTORY + "LockedImage";
        private const string UNKNOWN = UI_DIRECTORY + "UnknownImage";
        private const string HAIR_DIRECTORY = "HAIR/";

        public static Texture2D KiBar;
        public static Texture2D OverloadBar;
        public static Texture2D KiBarLightning;
        public static Texture2D Bg;
        public static Texture2D BackPanel;
        public static Texture2D WishBackPanel;
        public static Texture2D GrantButton;
        public static Texture2D SSJ1ButtonImage;
        public static Texture2D SSJ2ButtonImage;
        public static Texture2D SSJ3ButtonImage;
        public static Texture2D SSJGButtonImage;
        public static Texture2D LSSJButtonImage;
        public static Texture2D LSSJ2ButtonImage;
        public static Texture2D SSJSButtonImage;
        public static Texture2D WishforPower;
        public static Texture2D WishforWealth;
        public static Texture2D WishforImmortality;
        public static Texture2D WishforGenetics;
        public static Texture2D WishforSkill;
        public static Texture2D WishforAwakening;
        public static Texture2D LockedImage;
        public static Texture2D UnknownImage;
        public static void LoadGFX(Mod mod)
        {
            KiBar = mod.GetTexture(KIBAR);
            OverloadBar = mod.GetTexture(OVERLOADBAR);
            KiBarLightning = mod.GetTexture(KIBARLIGHTNING);
            Bg = mod.GetTexture(BG);
            BackPanel = mod.GetTexture(BACKPANEL);
            WishBackPanel = mod.GetTexture(WISHBACKPANEL);
            GrantButton = mod.GetTexture(GRANTBUTTON);
            SSJ1ButtonImage = mod.GetTexture(SSJ1BUTTON);
            SSJ2ButtonImage = mod.GetTexture(SSJ2BUTTON);
            SSJ3ButtonImage = mod.GetTexture(SSJ3BUTTON);
            SSJGButtonImage = mod.GetTexture(SSJGBUTTON);
            LSSJButtonImage = mod.GetTexture(LSSJBUTTON);
            LSSJ2ButtonImage = mod.GetTexture(LSSJ2BUTTON);
            SSJSButtonImage = mod.GetTexture(SSJSBUTTON);
            LockedImage = mod.GetTexture(LOCKED);
            UnknownImage = mod.GetTexture(UNKNOWN);
            WishforPower = mod.GetTexture(WISHFORPOWER);
            WishforWealth = mod.GetTexture(WISHFORWEALTH);
            WishforImmortality = mod.GetTexture(WISHFORIMMORTALITY);
            WishforGenetics = mod.GetTexture(WISHFORGENETICS);
            WishforSkill = mod.GetTexture(WISHFORSKILL);
            WishforAwakening = mod.GetTexture(WISHFORAWAKENING);

        }

        public static void UnloadGFX()
        {
            KiBar = null;
            OverloadBar = null;
            KiBarLightning = null;
            Bg = null;
            SSJ1ButtonImage = null;
            SSJ2ButtonImage = null;
            SSJ3ButtonImage = null;
            LSSJButtonImage = null;
            LSSJ2ButtonImage = null;
            SSJGButtonImage = null;
            SSJSButtonImage = null;
            LockedImage = null;
            BackPanel = null;
            UnknownImage = null;
            WishBackPanel = null;
            WishforPower = null;
            WishforWealth = null;
            WishforImmortality = null;
            WishforGenetics = null;
            WishforSkill = null;
            WishforAwakening = null;
            GrantButton = null;
        }
    }
}