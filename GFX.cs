using System.Collections.Generic;
using DBZMOD.Traits;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace DBZMOD
{

    public static class GFX
    {
        public class KiBarTexture
        {
            public Texture2D 
                kiBarSegment,
                kiBarFrame;

            public KiBarTexture(Trait trait, Mod mod)
            {
                string fileName = char.ToUpper(trait.UnlocalizedName[0]) + trait.UnlocalizedName.Substring(1);

                string textureName = KI_BAR.Replace("/KiBar/KiBar", $"/KiBar/{fileName}KiBar");
                string frameName = textureName + "Frame";
                this.kiBarSegment = mod.GetTexture(textureName);
                this.kiBarFrame = mod.GetTexture(frameName);
            }
        }

        public static KiBarTexture GetKiBar(MyPlayer player)
        {
            return KiBarTextures[player.PlayerTrait];
        }

        public static void LoadKiBarTextures(Mod mod)
        {
            KiBarTextures = new Dictionary<Trait, KiBarTexture>();

            // TODO Remove null check

            for (int i = 0; i < DBZMOD.Instance.TraitManager.Count; i++)
            {
                Trait trait = DBZMOD.Instance.TraitManager[i];

                string processedTraitName = char.ToUpper(trait.UnlocalizedName[0]) + trait.UnlocalizedName.Substring(1);
                KiBarTextures.Add(trait, new KiBarTexture(trait, mod));
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
            SSJ4_BUTTON = BUTTON_DIRECTORY + "SSJ4ButtonImage",

            SSJG_BUTTON = BUTTON_DIRECTORY + "SSJGButtonImage",
            SSJB_BUTTON = BUTTON_DIRECTORY + "SSJBButtonImage",

            LSSJ_BUTTON = BUTTON_DIRECTORY + "LSSJButtonImage",
            
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

        public static Dictionary<Trait, KiBarTexture> KiBarTextures { get; set; }

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
            ssj4ButtonImage,

            ssjgButtonImage,
            ssjbButtonImage,
            
            lssjButtonImage,
            
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
        

        public static void LoadGFX(Mod mod)
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
            ssj4ButtonImage = mod.GetTexture(SSJ4_BUTTON);

            ssjgButtonImage = mod.GetTexture(SSJG_BUTTON);
            ssjbButtonImage = mod.GetTexture(SSJB_BUTTON);

            lssjButtonImage = mod.GetTexture(LSSJ_BUTTON);

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

        public static void UnloadGFX()
        {
            KiBarTextures.Clear();
            overloadBarSegment = null;
            overloadBarFrame = null;
            bg = null;

            ssj1ButtonImage = null;
            ssj2ButtonImage = null;
            ssj3ButtonImage = null;

            ssjgButtonImage = null;

            lssjButtonImage = null;

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