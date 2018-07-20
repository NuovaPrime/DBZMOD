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
        private const string KIBAR = UI_DIRECTORY + "KiBar";
        private const string SSJ1BUTTON = UI_DIRECTORY + "SSJ1ButtonImage";
        private const string SSJ2BUTTON = UI_DIRECTORY + "SSJ2ButtonImage";
        private const string BG = UI_DIRECTORY + "Bg";
        private const string HAIR_DIRECTORY = "HAIR/";
        //private const string SSJ1HAIR = HAIR_DIRECTORY + "SSJ1Hair";
        //private const string SSJ2HAIR = HAIR_DIRECTORY + "SSJ2Hair";

        public static Texture2D KiBar;
        public static Texture2D Bg;
        public static Texture2D SSJ1ButtonImage;
        public static Texture2D SSJ2ButtonImage;
        // public static Texture2D SSJ1Hair;
        //public static Texture2D SSJ2Hair;

        public static void LoadGFX(Mod mod)
        {
            KiBar = mod.GetTexture(KIBAR);
            Bg = mod.GetTexture(BG);
            SSJ1ButtonImage = mod.GetTexture(SSJ1BUTTON);
            SSJ2ButtonImage = mod.GetTexture(SSJ2BUTTON);
            //SSJ1Hair = mod.GetTexture(SSJ1HAIR);
            //SSJ2Hair = mod.GetTexture(SSJ2HAIR);

        }

        public static void UnloadGFX()
        {
            KiBar = null;
            Bg = null;
            SSJ1ButtonImage = null;
            SSJ2ButtonImage = null;
            //SSJ1Hair = null;
            //SSJ2Hair = null;
            //SSJHairDraw.Hair = null;
        }
    }
}