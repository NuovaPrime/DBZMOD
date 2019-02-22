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
            HAIR_MENU_DIRECTORY = UI_DIRECTORY + "HairMenu/",
            KI_BAR = KI_BAR_DIRECTORY + "KiBar",
            OVERLOAD_BAR_SEGMENT = UI_DIRECTORY + "OverloadBar",
            OVERLOAD_BAR_FRAME = UI_DIRECTORY + "OverloadBarFrame",
            BACK_PANEL = UI_DIRECTORY + "BackPanel",
            WISH_BACK_PANEL = UI_DIRECTORY + "WishBackPanel",
            HAIR_BACK_PANEL = HAIR_MENU_DIRECTORY + "HairBackPanel",
            HAIR_CONFIRM_BUTTON = HAIR_MENU_DIRECTORY + "ConfirmButton",
            KEEP_HAIR_BUTTON = HAIR_MENU_DIRECTORY + "KeepHairButton",
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
            HAIR_DIRECTORY = "HAIR/",
            STYLE_PREVIEWS_DIRECTORY = HAIR_MENU_DIRECTORY + "StylePreviews/",
            STYLE_ONE_DIRECTORY = STYLE_PREVIEWS_DIRECTORY + "Style1/",
            STYLE_TWO_DIRECTORY = STYLE_PREVIEWS_DIRECTORY + "Style2/",
            STYLE_THREE_DIRECTORY = STYLE_PREVIEWS_DIRECTORY + "Style3/",
            STYLE_FOUR_DIRECTORY = STYLE_PREVIEWS_DIRECTORY + "Style4/",
            STYLE_FIVE_DIRECTORY = STYLE_PREVIEWS_DIRECTORY + "Style5/",
            STYLE_ONE_BASE = STYLE_ONE_DIRECTORY + "Base",
            STYLE_ONE_SSJ = STYLE_ONE_DIRECTORY + "SuperSaiyan1",
            STYLE_ONE_SSJ2 = STYLE_ONE_DIRECTORY + "SuperSaiyan2",
            STYLE_ONE_SSJ3 = STYLE_ONE_DIRECTORY + "SuperSaiyan3",
            STYLE_ONE_SSJ4 = STYLE_ONE_DIRECTORY + "SuperSaiyan4",
            STYLE_TWO_BASE = STYLE_TWO_DIRECTORY + "Base",
            STYLE_TWO_SSJ = STYLE_TWO_DIRECTORY + "SuperSaiyan1",
            STYLE_TWO_SSJ2 = STYLE_TWO_DIRECTORY + "SuperSaiyan2",
            STYLE_TWO_SSJ3 = STYLE_TWO_DIRECTORY + "SuperSaiyan3",
            STYLE_TWO_SSJ4 = STYLE_TWO_DIRECTORY + "SuperSaiyan4",
            STYLE_THREE_BASE = STYLE_THREE_DIRECTORY + "Base",
            STYLE_THREE_SSJ = STYLE_THREE_DIRECTORY + "SuperSaiyan1",
            STYLE_THREE_SSJ2 = STYLE_THREE_DIRECTORY + "SuperSaiyan2",
            STYLE_THREE_SSJ3 = STYLE_THREE_DIRECTORY + "SuperSaiyan3",
            STYLE_THREE_SSJ4 = STYLE_THREE_DIRECTORY + "SuperSaiyan4",
            STYLE_FOUR_BASE = STYLE_FOUR_DIRECTORY + "Base",
            STYLE_FOUR_SSJ = STYLE_FOUR_DIRECTORY + "SuperSaiyan1",
            STYLE_FOUR_SSJ2 = STYLE_FOUR_DIRECTORY + "SuperSaiyan2",
            STYLE_FOUR_SSJ3 = STYLE_FOUR_DIRECTORY + "SuperSaiyan3",
            STYLE_FOUR_SSJ4 = STYLE_FOUR_DIRECTORY + "SuperSaiyan4",
            STYLE_FIVE_BASE = STYLE_FIVE_DIRECTORY + "Base",
            STYLE_FIVE_SSJ = STYLE_FIVE_DIRECTORY + "SuperSaiyan1",
            STYLE_FIVE_SSJ2 = STYLE_FIVE_DIRECTORY + "SuperSaiyan2",
            STYLE_FIVE_SSJ3 = STYLE_FIVE_DIRECTORY + "SuperSaiyan3",
            STYLE_FIVE_SSJ4 = STYLE_FIVE_DIRECTORY + "SuperSaiyan4";


        public static Dictionary<Trait, KiBarTexture> KiBarTextures { get; set; }

        public static Texture2D
            overloadBarSegment,
            overloadBarFrame,
            bg,
            backPanel,
            wishBackPanel,
            hairBackPanel,
            hairConfirmButton,
            keepHairButton,
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
            whiteSquare,
            style1BasePreview,
            style1SSJPreview,
            style1SSJ2Preview,
            style1SSJ3Preview,
            style1SSJ4Preview,
            style2BasePreview,
            style2SSJPreview,
            style2SSJ2Preview,
            style2SSJ3Preview,
            style2SSJ4Preview,
            style3BasePreview,
            style3SSJPreview,
            style3SSJ2Preview,
            style3SSJ3Preview,
            style3SSJ4Preview,
            style4BasePreview,
            style4SSJPreview,
            style4SSJ2Preview,
            style4SSJ3Preview,
            style4SSJ4Preview,
            style5BasePreview,
            style5SSJPreview,
            style5SSJ2Preview,
            style5SSJ3Preview,
            style5SSJ4Preview;

        public static void LoadGFX(Mod mod)
        {
            LoadKiBarTextures(mod);
            overloadBarSegment = mod.GetTexture(OVERLOAD_BAR_SEGMENT);
            overloadBarFrame = mod.GetTexture(OVERLOAD_BAR_FRAME);
            bg = mod.GetTexture(BG);
            backPanel = mod.GetTexture(BACK_PANEL);
            wishBackPanel = mod.GetTexture(WISH_BACK_PANEL);
            hairBackPanel = mod.GetTexture(HAIR_BACK_PANEL);
            hairConfirmButton = mod.GetTexture(HAIR_CONFIRM_BUTTON);
            keepHairButton = mod.GetTexture(KEEP_HAIR_BUTTON);
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

            style1BasePreview = mod.GetTexture(STYLE_ONE_BASE);
            style1SSJPreview = mod.GetTexture(STYLE_ONE_SSJ);
            style1SSJ2Preview = mod.GetTexture(STYLE_ONE_SSJ2);
            style1SSJ3Preview = mod.GetTexture(STYLE_ONE_SSJ3);
            style1SSJ4Preview = mod.GetTexture(STYLE_ONE_SSJ4);
            style2BasePreview = mod.GetTexture(STYLE_TWO_BASE);
            style2SSJPreview = mod.GetTexture(STYLE_TWO_SSJ);
            style2SSJ2Preview = mod.GetTexture(STYLE_TWO_SSJ2);
            style2SSJ3Preview = mod.GetTexture(STYLE_TWO_SSJ3);
            style2SSJ4Preview = mod.GetTexture(STYLE_TWO_SSJ4);
            style3BasePreview = mod.GetTexture(STYLE_THREE_BASE);
            style3SSJPreview = mod.GetTexture(STYLE_THREE_SSJ);
            style3SSJ2Preview = mod.GetTexture(STYLE_THREE_SSJ2);
            style3SSJ3Preview = mod.GetTexture(STYLE_THREE_SSJ3);
            style3SSJ4Preview = mod.GetTexture(STYLE_THREE_SSJ4);
            style4BasePreview = mod.GetTexture(STYLE_FOUR_BASE);
            style4SSJPreview = mod.GetTexture(STYLE_FOUR_SSJ);
            style4SSJ2Preview = mod.GetTexture(STYLE_FOUR_SSJ2);
            style4SSJ3Preview = mod.GetTexture(STYLE_FOUR_SSJ3);
            style4SSJ4Preview = mod.GetTexture(STYLE_FOUR_SSJ4);
            style5BasePreview = mod.GetTexture(STYLE_FIVE_BASE);
            style5SSJPreview = mod.GetTexture(STYLE_FIVE_SSJ);
            style5SSJ2Preview = mod.GetTexture(STYLE_FIVE_SSJ2);
            style5SSJ3Preview = mod.GetTexture(STYLE_FIVE_SSJ3);
            style5SSJ4Preview = mod.GetTexture(STYLE_FIVE_SSJ4);
        }

        public static void UnloadGfx()
        {
            KiBarTextures.Clear();
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
            hairBackPanel = null;
            hairConfirmButton = null;
            keepHairButton = null;
            unknownImage = null;
            wishBackPanel = null;
            wishforPower = null;
            wishforWealth = null;
            wishforImmortality = null;
            wishforGenetics = null;
            wishforSkill = null;
            wishforAwakening = null;
            grantButton = null;
            style1BasePreview = null;
            style1SSJPreview = null;
            style1SSJ2Preview = null;
            style1SSJ3Preview = null;
            style1SSJ4Preview = null;
            style2BasePreview = null;
            style2SSJPreview = null;
            style2SSJ2Preview = null;
            style2SSJ3Preview = null;
            style2SSJ4Preview = null;
            style3BasePreview = null;
            style3SSJPreview = null;
            style3SSJ2Preview = null;
            style3SSJ3Preview = null;
            style3SSJ4Preview = null;
            style4BasePreview = null;
            style4SSJPreview = null;
            style4SSJ2Preview = null;
            style4SSJ3Preview = null;
            style4SSJ4Preview = null;
            style5BasePreview = null;
            style5SSJPreview = null;
            style5SSJ2Preview = null;
            style5SSJ3Preview = null;
            style5SSJ4Preview = null;
        }
    }
}