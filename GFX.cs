using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace DBZMOD
{
    public static class Gfx
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
        private const string SSJ1_BUTTON = BUTTON_DIRECTORY + "SSJ1ButtonImage";
        private const string SSJ2_BUTTON = BUTTON_DIRECTORY + "SSJ2ButtonImage";
        private const string SSJ3_BUTTON = BUTTON_DIRECTORY + "SSJ3ButtonImage";
        private const string SSJGBUTTON = BUTTON_DIRECTORY + "SSJGButtonImage";
        private const string LSSJBUTTON = BUTTON_DIRECTORY + "LSSJButtonImage";
        private const string LSSJ2_BUTTON = BUTTON_DIRECTORY + "LSSJ2ButtonImage";
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

        public static Texture2D kiBar;
        public static Texture2D overloadBar;
        public static Texture2D kiBarLightning;
        public static Texture2D bg;
        public static Texture2D backPanel;
        public static Texture2D wishBackPanel;
        public static Texture2D grantButton;
        public static Texture2D ssj1ButtonImage;
        public static Texture2D ssj2ButtonImage;
        public static Texture2D ssj3ButtonImage;
        public static Texture2D ssjgButtonImage;
        public static Texture2D lssjButtonImage;
        public static Texture2D lssj2ButtonImage;
        public static Texture2D ssjsButtonImage;
        public static Texture2D wishforPower;
        public static Texture2D wishforWealth;
        public static Texture2D wishforImmortality;
        public static Texture2D wishforGenetics;
        public static Texture2D wishforSkill;
        public static Texture2D wishforAwakening;
        public static Texture2D lockedImage;
        public static Texture2D unknownImage;
        public static void LoadGfx(Mod mod)
        {
            kiBar = mod.GetTexture(KIBAR);
            overloadBar = mod.GetTexture(OVERLOADBAR);
            kiBarLightning = mod.GetTexture(KIBARLIGHTNING);
            bg = mod.GetTexture(BG);
            backPanel = mod.GetTexture(BACKPANEL);
            wishBackPanel = mod.GetTexture(WISHBACKPANEL);
            grantButton = mod.GetTexture(GRANTBUTTON);
            ssj1ButtonImage = mod.GetTexture(SSJ1_BUTTON);
            ssj2ButtonImage = mod.GetTexture(SSJ2_BUTTON);
            ssj3ButtonImage = mod.GetTexture(SSJ3_BUTTON);
            ssjgButtonImage = mod.GetTexture(SSJGBUTTON);
            lssjButtonImage = mod.GetTexture(LSSJBUTTON);
            lssj2ButtonImage = mod.GetTexture(LSSJ2_BUTTON);
            ssjsButtonImage = mod.GetTexture(SSJSBUTTON);
            lockedImage = mod.GetTexture(LOCKED);
            unknownImage = mod.GetTexture(UNKNOWN);
            wishforPower = mod.GetTexture(WISHFORPOWER);
            wishforWealth = mod.GetTexture(WISHFORWEALTH);
            wishforImmortality = mod.GetTexture(WISHFORIMMORTALITY);
            wishforGenetics = mod.GetTexture(WISHFORGENETICS);
            wishforSkill = mod.GetTexture(WISHFORSKILL);
            wishforAwakening = mod.GetTexture(WISHFORAWAKENING);

        }

        public static void UnloadGfx()
        {
            kiBar = null;
            overloadBar = null;
            kiBarLightning = null;
            bg = null;
            ssj1ButtonImage = null;
            ssj2ButtonImage = null;
            ssj3ButtonImage = null;
            lssjButtonImage = null;
            lssj2ButtonImage = null;
            ssjgButtonImage = null;
            ssjsButtonImage = null;
            lockedImage = null;
            backPanel = null;
            unknownImage = null;
            wishBackPanel = null;
            wishforPower = null;
            wishforWealth = null;
            wishforImmortality = null;
            wishforGenetics = null;
            wishforSkill = null;
            wishforAwakening = null;
            grantButton = null;
        }
    }
}