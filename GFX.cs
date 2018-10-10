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
        private const string BACKPANEL = UI_DIRECTORY + "BackPanel";
        private const string SSJ1BUTTON = BUTTON_DIRECTORY + "SSJ1ButtonImage";
        private const string SSJ2BUTTON = BUTTON_DIRECTORY + "SSJ2ButtonImage";
        private const string SSJ3BUTTON = BUTTON_DIRECTORY + "SSJ3ButtonImage";
        private const string SSJGBUTTON = BUTTON_DIRECTORY + "SSJGButtonImage";
        private const string LSSJBUTTON = BUTTON_DIRECTORY + "LSSJButtonImage";
        private const string BG = UI_DIRECTORY + "Bg";
        private const string LOCKED = UI_DIRECTORY + "LockedImage";
        private const string HAIR_DIRECTORY = "HAIR/";

        public static Texture2D KiBar;
        public static Texture2D Bg;
        public static Texture2D BackPanel;
        public static Texture2D SSJ1ButtonImage;
        public static Texture2D SSJ2ButtonImage;
        public static Texture2D SSJ3ButtonImage;
        public static Texture2D SSJGButtonImage;
        public static Texture2D LSSJButtonImage;
        public static Texture2D LockedImage;
        public static void LoadGFX(Mod mod)
        {
            KiBar = mod.GetTexture(KIBAR);
            Bg = mod.GetTexture(BG);
            BackPanel = mod.GetTexture(BACKPANEL);
            SSJ1ButtonImage = mod.GetTexture(SSJ1BUTTON);
            SSJ2ButtonImage = mod.GetTexture(SSJ2BUTTON);
            SSJ3ButtonImage = mod.GetTexture(SSJ3BUTTON);
            SSJGButtonImage = mod.GetTexture(SSJGBUTTON);
            LSSJButtonImage = mod.GetTexture(LSSJBUTTON);
            LockedImage = mod.GetTexture(LOCKED);

        }

        public static void UnloadGFX()
        {
            KiBar = null;
            Bg = null;
            SSJ1ButtonImage = null;
            SSJ2ButtonImage = null;
            SSJ3ButtonImage = null;
            LSSJButtonImage = null;
            SSJGButtonImage = null;
            LockedImage = null;
            BackPanel = null;
        }
    }
}