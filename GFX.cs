using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace DBZMOD
{

    public static class Gfx
    {
        private static readonly string[] traitKiBarTypes = new string[]{ "Corrupt", "Divine", "Legendary", "Pacifist", "Shinseiju", "Primal", "Prodigy", string.Empty };

        public class KiBarTexture
        {
            public Texture2D KiBar;
            public Texture2D KiBarFrame;

            public KiBarTexture(string traitType, Mod mod)
            {
                string textureName = KI_BAR.Replace("/KiBar/KiBar", $"/KiBar/{traitType}KiBar");
                string frameName = textureName + "Frame";
                this.KiBar = mod.GetTexture(textureName);
                this.KiBarFrame = mod.GetTexture(frameName);
            }
        }

        public static KiBarTexture GetKiBar(MyPlayer player)
        {
            string playerTrait = player.playerTrait;
            return kiBarTextures[playerTrait];
        }

        public static void LoadKiBarTextures(Mod mod)
        {
            kiBarTextures = new Dictionary<string, KiBarTexture>();
            foreach (string kiBarType in traitKiBarTypes)
            {
                KiBarTexture kiBarTexture = new KiBarTexture(kiBarType, mod);
                kiBarTextures[kiBarType] = kiBarTexture;
            }
        }

        private const string GUI_DIRECTORY = "GFX/";
        private const string UI_DIRECTORY = "UI/";
        private const string BUTTON_DIRECTORY = "UI/Buttons/";
        private const string KI_BAR_DIRECTORY = UI_DIRECTORY + "KiBar/";
        private const string KI_BAR = KI_BAR_DIRECTORY + "KiBar";
        private const string OVERLOAD_BAR = UI_DIRECTORY + "OverloadBar";
        private const string BACK_PANEL = UI_DIRECTORY + "BackPanel";
        private const string WISH_BACK_PANEL = UI_DIRECTORY + "WishBackPanel";
        private const string GRANT_BUTTON = BUTTON_DIRECTORY + "GrantButton";
        private const string SSJ1_BUTTON = BUTTON_DIRECTORY + "SSJ1ButtonImage";
        private const string SSJ2_BUTTON = BUTTON_DIRECTORY + "SSJ2ButtonImage";
        private const string SSJ3_BUTTON = BUTTON_DIRECTORY + "SSJ3ButtonImage";
        private const string SSJG_BUTTON = BUTTON_DIRECTORY + "SSJGButtonImage";
        private const string LSSJ_BUTTON = BUTTON_DIRECTORY + "LSSJButtonImage";
        private const string LSSJ2_BUTTON = BUTTON_DIRECTORY + "LSSJ2ButtonImage";
        private const string SSJS_BUTTON = BUTTON_DIRECTORY + "SSJSButtonImage";
        private const string WISH_FOR_POWER = BUTTON_DIRECTORY + "WishforPower";
        private const string WISH_FOR_WEALTH = BUTTON_DIRECTORY + "WishforWealth";
        private const string WISH_FOR_IMMORTALITY = BUTTON_DIRECTORY + "WishforImmortality";
        private const string WISH_FOR_GENETICS = BUTTON_DIRECTORY + "WishforGenetics";
        private const string WISH_FOR_SKILL = BUTTON_DIRECTORY + "WishforSkill";
        private const string WISH_FOR_AWAKENING = BUTTON_DIRECTORY + "WishforAwakening";
        private const string BG = UI_DIRECTORY + "Bg";
        private const string LOCKED = UI_DIRECTORY + "LockedImage";
        private const string UNKNOWN = UI_DIRECTORY + "UnknownImage";
        private const string HAIR_DIRECTORY = "HAIR/";

        public static Dictionary<string, KiBarTexture> kiBarTextures;
        public static Texture2D overloadBar;
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
            LoadKiBarTextures(mod);
            overloadBar = mod.GetTexture(OVERLOAD_BAR);
            bg = mod.GetTexture(BG);
            backPanel = mod.GetTexture(BACK_PANEL);
            wishBackPanel = mod.GetTexture(WISH_BACK_PANEL);
            grantButton = mod.GetTexture(GRANT_BUTTON);
            ssj1ButtonImage = mod.GetTexture(SSJ1_BUTTON);
            ssj2ButtonImage = mod.GetTexture(SSJ2_BUTTON);
            ssj3ButtonImage = mod.GetTexture(SSJ3_BUTTON);
            ssjgButtonImage = mod.GetTexture(SSJG_BUTTON);
            lssjButtonImage = mod.GetTexture(LSSJ_BUTTON);
            lssj2ButtonImage = mod.GetTexture(LSSJ2_BUTTON);
            ssjsButtonImage = mod.GetTexture(SSJS_BUTTON);
            lockedImage = mod.GetTexture(LOCKED);
            unknownImage = mod.GetTexture(UNKNOWN);
            wishforPower = mod.GetTexture(WISH_FOR_POWER);
            wishforWealth = mod.GetTexture(WISH_FOR_WEALTH);
            wishforImmortality = mod.GetTexture(WISH_FOR_IMMORTALITY);
            wishforGenetics = mod.GetTexture(WISH_FOR_GENETICS);
            wishforSkill = mod.GetTexture(WISH_FOR_SKILL);
            wishforAwakening = mod.GetTexture(WISH_FOR_AWAKENING);
        }

        public static void UnloadGfx()
        {
            kiBarTextures.Clear();
            overloadBar = null;
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