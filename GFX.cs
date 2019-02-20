using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace DBZMOD
{

    public static class GFX
    {
        private static readonly string[] traitKiBarTypes = new string[]{ "Corrupt", "Divine", "Legendary", "Pacifist", "Shinseiju", "Primal", "Prodigy", string.Empty };

        public class KiBarTexture
        {
            public Texture2D KiBarSegment;
            public Texture2D KiBarFrame;

            public KiBarTexture(string traitType, Mod mod)
            {
                string textureName = KI_BAR.Replace("/KiBar/KiBar", $"/KiBar/{traitType}KiBar");
                string frameName = textureName + "Frame";
                this.KiBarSegment = mod.GetTexture(textureName);
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

        private const string
            GUI_DIRECTORY = "GFX/",
            UI_DIRECTORY = "UI/",
            BUTTON_DIRECTORY = "UI/Buttons/",
            KI_BAR_DIRECTORY = UI_DIRECTORY + "KiBar/",
            KI_BAR = KI_BAR_DIRECTORY + "KiBar",
            OVERLOAD_BAR_SEGMENT = UI_DIRECTORY + "OverloadBar",
            OVERLOAD_BAR_FRAME = UI_DIRECTORY + "OverloadBarFrame",
            BACK_PANEL = UI_DIRECTORY + "BackPanel",
            WISH_BACK_PANEL = UI_DIRECTORY + "WishBackPanel",
            GRANT_BUTTON = BUTTON_DIRECTORY + "GrantButton",
            SSJ1_BUTTON = BUTTON_DIRECTORY + "SSJ1ButtonImage",
            SSJ2_BUTTON = BUTTON_DIRECTORY + "SSJ2ButtonImage",
            SSJ3_BUTTON = BUTTON_DIRECTORY + "SSJ3ButtonImage",
            SSJG_BUTTON = BUTTON_DIRECTORY + "SSJGButtonImage",
            LSSJ_BUTTON = BUTTON_DIRECTORY + "LSSJButtonImage",
            LSSJ2_BUTTON = BUTTON_DIRECTORY + "LSSJ2ButtonImage",
            SSJS_BUTTON = BUTTON_DIRECTORY + "SSJSButtonImage",
            WISH_FOR_POWER = BUTTON_DIRECTORY + "WishforPower",
            WISH_FOR_WEALTH = BUTTON_DIRECTORY + "WishforWealth",
            WISH_FOR_IMMORTALITY = BUTTON_DIRECTORY + "WishforImmortality",
            WISH_FOR_GENETICS = BUTTON_DIRECTORY + "WishforGenetics",
            WISH_FOR_SKILL = BUTTON_DIRECTORY + "WishforSkill",
            WISH_FOR_AWAKENING = BUTTON_DIRECTORY + "WishforAwakening",
            BG = UI_DIRECTORY + "Bg",
            LOCKED = UI_DIRECTORY + "LockedImage",
            UNKNOWN = UI_DIRECTORY + "UnknownImage",
            UNKNOWN_GRAY = UI_DIRECTORY + "UnknownImageGray",
            WHITE_SQUARE = UI_DIRECTORY + "WhiteSquare",
            HAIR_DIRECTORY = "HAIR/";

        public static Dictionary<string, KiBarTexture> kiBarTextures;

        public static Texture2D
            overloadBarSegment,
            overloadBarFrame,
            bg,
            backPanel,
            wishBackPanel,
            grantButton,
            ssj1ButtonImage,
            ssj2ButtonImage,
            ssj3ButtonImage,
            ssjgButtonImage,
            lssjButtonImage,
            lssj2ButtonImage,
            ssjsButtonImage,
            wishforPower,
            wishforWealth,
            wishforImmortality,
            wishforGenetics,
            wishforSkill,
            wishforAwakening,
            lockedImage,
            unknownImage,
            unknownImageGray,
            whiteSquare;
        

        public static void LoadGfx(Mod mod)
        {
            LoadKiBarTextures(mod);
            overloadBarSegment = mod.GetTexture(OVERLOAD_BAR_SEGMENT);
            overloadBarFrame = mod.GetTexture(OVERLOAD_BAR_FRAME);
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
            unknownImageGray = mod.GetTexture(UNKNOWN_GRAY);
            whiteSquare = mod.GetTexture(WHITE_SQUARE);

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
            overloadBarSegment = null;
            overloadBarFrame = null;
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