using System;
using System.Linq;
using DBZMOD.Buffs;
using DBZMOD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Enums;
using Util;

namespace DBZMOD.UI
{
    internal class TransMenu : EasyMenu
    {
        public static bool menuvisible = false;
        private UIText titleText;
        public UIImage backPanelImage;
        private UIImageButton ssjButtonTexture;
        private UIImageButton ssj2ButtonTexture;
        private UIImageButton ssj3ButtonTexture;
        private UIImageButton lssjButtonTexture;
        private UIImageButton lssj2ButtonTexture;
        private UIImageButton ssjgButtonTexture;
        private UIImageButton ssjSButtonTexture;
        private UIImage lockedImage1;
        private UIImage lockedImage2;
        private UIImage lockedImage3;
        private UIImage lockedImageG;
        private UIImage lockedImageL1;
        private UIImage lockedImageL2;
        private UIImage unknownImage2;
        private UIImage unknownImage3;
        private UIImage unknownImageG;
        private UIImage unknownImageL1;
        private UIImage unknownImageL2;


        public static MenuSelectionID MenuSelection;
        public static bool SSJ1On;
        public static bool SSJ2On;
        public static bool SSJ3On;
        public static bool LSSJOn;
        private Player player;
        public const float PADDINGX = 10f;
        public const float PADDINGY = 30f;

        public override void OnInitialize()
        {
            base.OnInitialize();

            backPanel = new UIPanel();
            backPanel.Width.Set(306f, 0f);
            backPanel.Height.Set(128f, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 2f - backPanel.Height.Pixels / 2f, 0f);
            backPanel.BackgroundColor = new Color(0, 0, 0, 0);
            backPanel.OnMouseDown += new MouseEvent(DragStart);
            backPanel.OnMouseUp += new MouseEvent(DragEnd);
            Append(backPanel);

            backPanelImage = new UIImage(GFX.BackPanel);
            backPanelImage.Width.Set(GFX.BackPanel.Width, 0f);
            backPanelImage.Height.Set(GFX.BackPanel.Height, 0f);
            backPanelImage.Left.Set(-12, 0f);
            backPanelImage.Top.Set(-12, 0f);
            backPanel.Append(backPanelImage);
            float Row1_OffsetX = 0.0f;

            InitText(ref titleText, "Transformation Tree", 1, 55, -32, Color.White);

            Row1_OffsetX = PADDINGX;
            InitButton(ref ssjButtonTexture, GFX.SSJ1ButtonImage, new MouseEvent(TrySelectingSSJ1),
                Row1_OffsetX - 2,
                PADDINGY - 20,
                backPanelImage);

            InitImage(ref lockedImage1, GFX.LockedImage,
                0,
                0,
                ssjButtonTexture);

            Row1_OffsetX = PADDINGX + GFX.SSJ1ButtonImage.Width;
            InitButton(ref ssj2ButtonTexture, GFX.SSJ2ButtonImage, new MouseEvent(TrySelectingSSJ2),
                Row1_OffsetX + 14,
                PADDINGY - 20,
                backPanelImage);

            InitImage(ref lockedImage2, GFX.LockedImage,
                0,
                0,
                ssj2ButtonTexture);

            InitImage(ref unknownImage2, GFX.UnknownImage,
                0,
                0,
                ssj2ButtonTexture);

            Row1_OffsetX = PADDINGX + GFX.SSJ2ButtonImage.Width * 2;
            InitButton(ref ssj3ButtonTexture, GFX.SSJ3ButtonImage, new MouseEvent(TrySelectingSSJ3),
                Row1_OffsetX + 22,
                PADDINGY - 20,
                backPanelImage);

            InitImage(ref lockedImage3, GFX.LockedImage,
                0,
                0,
                ssj3ButtonTexture);

            InitImage(ref unknownImage3, GFX.UnknownImage,
                0,
                0,
                ssj3ButtonTexture);

            InitButton(ref lssjButtonTexture, GFX.LSSJButtonImage, new MouseEvent(TrySelectingLSSJ),
                PADDINGX + 14 + GFX.SSJ1ButtonImage.Width,
                GFX.SSJ1ButtonImage.Height + PADDINGY - 10,
                backPanelImage);

            InitImage(ref lockedImageL1, GFX.LockedImage,
                0,
                0,
                lssjButtonTexture);

            InitImage(ref unknownImageL1, GFX.UnknownImage,
                0,
                0,
                lssjButtonTexture);

            Row1_OffsetX = PADDINGX + GFX.SSJ3ButtonImage.Width * 3;
            InitButton(ref ssjgButtonTexture, GFX.SSJGButtonImage, new MouseEvent(TrySelectingSSJG),
                Row1_OffsetX + 30,
                PADDINGY - 20,
                backPanelImage);

            InitImage(ref lockedImageG, GFX.LockedImage,
                0,
                0,
                ssjgButtonTexture);

            InitImage(ref unknownImageG, GFX.UnknownImage,
                0,
                0,
                ssjgButtonTexture);

            InitButton(ref lssj2ButtonTexture, GFX.LSSJ2ButtonImage, new MouseEvent(TrySelectingLSSJ2),
                PADDINGX + 22 + GFX.SSJ1ButtonImage.Width * 2,
                GFX.SSJ1ButtonImage.Height + PADDINGY - 10,
                backPanelImage);

            InitImage(ref lockedImageL2, GFX.LockedImage,
                0,
                0,
                lssj2ButtonTexture);

            InitImage(ref unknownImageL2, GFX.UnknownImage,
                0,
                0,
                lssj2ButtonTexture);

            InitButton(ref ssjSButtonTexture, GFX.SSJSButtonImage, new MouseEvent(TrySelectingSSJS),
                PADDINGX + 14 + GFX.SSJ1ButtonImage.Width,
                PADDINGY + 55,
                backPanelImage);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            Player player = Main.LocalPlayer;

            lockedImage1.ImageScale = !modplayer.SSJ1Achieved ? 1.0f : 0.0f;

            if (player.name == "Nuova")
            {
                ssjSButtonTexture.SetVisibility(1.0f, 0.5f);
            }
            else
            {
                ssjSButtonTexture.SetVisibility(0.0f, 0.0f);
            }

            if (modplayer.IsPlayerLegendary())
            {
                lockedImageL1.ImageScale = !modplayer.LSSJAchieved ? 1.0f : 0.0f;

                lockedImageL2.ImageScale = !modplayer.LSSJ2Achieved ? 1.0f : 0.0f;

                lockedImage2.ImageScale = 1.0f;

                unknownImage2.ImageScale = 0.0f;

                lockedImage3.ImageScale = 0.0f;

                unknownImage3.ImageScale = 1.0f;

                lockedImageG.ImageScale = 0.0f;

                unknownImageG.ImageScale = 1.0f;

                unknownImageL1.ImageScale = 0.0f;

                unknownImageL2.ImageScale = !modplayer.LSSJAchieved ? 1.0f : 0.0f;
            }
            else
            {
                unknownImageL1.ImageScale = 0.0f;
                unknownImageL2.ImageScale = 1.0f;
                unknownImage2.ImageScale = 0.0f;
                unknownImage3.ImageScale = 0.0f;
                unknownImageG.ImageScale = 0.0f;
                lockedImage2.ImageScale = !modplayer.SSJ2Achieved ? 1.0f : 0.0f;

                lockedImage3.ImageScale = !modplayer.SSJ3Achieved ? 1.0f : 0.0f;

                lockedImageG.ImageScale = !modplayer.SSJGAchieved ? 1.0f : 0.0f;

                lockedImageL1.ImageScale = 1.0f;
                lockedImageL2.ImageScale = 0.0f;

            }
        }

        /*
            Menu Selection ID's
            1 = SSJ1
            2 = SSJ2
            3 = SSJ3
            4 = LSSJ1
            5 = SSJG
            6 = LSSJ2
            7 = SSJB/SSJBE
            8 = LSSJG
            9 = UI/MUI
         */
        private void TrySelectingSSJ1(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.SSJ1Achieved)
            {
                MenuSelection = MenuSelectionID.SSJ1;
                SoundUtil.PlayVanillaSound(SoundID.MenuTick);
                Main.NewText("SSJ1 Mastery = " + player.MasteryLevel1 + "/" + player.MasteryMax1);
            }
            else
            {
                SoundUtil.PlayVanillaSound(SoundID.MenuClose);
                Main.NewText("Only through failure with a powerful foe will true power awaken.");
            }
        }

        private void TrySelectingSSJ2(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.SSJ2Achieved && !player.IsPlayerLegendary())
            {
                MenuSelection = MenuSelectionID.SSJ2;
                SoundUtil.PlayVanillaSound(SoundID.MenuTick);
                Main.NewText("SSJ2 Mastery = " + player.MasteryLevel2 + "/" + player.MasteryMax2);
            }
            else if (!player.LSSJAchieved)
            {
                SoundUtil.PlayVanillaSound(SoundID.MenuClose);
                Main.NewText("One may awaken their true power through extreme pressure while ascended.");
            }
        }
        private void TrySelectingSSJ3(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.SSJ3Achieved && !player.IsPlayerLegendary())
            {
                MenuSelection = MenuSelectionID.SSJ3;
                SoundUtil.PlayVanillaSound(SoundID.MenuTick);
                Main.NewText("SSJ3 Mastery = " + player.MasteryLevel3 + "/" + player.MasteryMax3);
            }
            else if (!player.LSSJAchieved)
            {
                SoundUtil.PlayVanillaSound(SoundID.MenuClose);
                Main.NewText("The power of an ancient foe may be the key to unlocking greater power.");
            }
        }
        private void TrySelectingLSSJ(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.LSSJAchieved)
            {
                MenuSelection = MenuSelectionID.LSSJ1;
                SoundUtil.PlayVanillaSound(SoundID.MenuTick);
            }
            else if (!player.SSJ2Achieved)
            {
                SoundUtil.PlayVanillaSound(SoundID.MenuClose);
                Main.NewText("The rarest saiyans may be able to achieve a form beyond anything a normal saiyan could obtain.");
            }
        }

        private void TrySelectingLSSJ2(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.LSSJ2Achieved)
            {
                MenuSelection = MenuSelectionID.LSSJ2;
                SoundUtil.PlayVanillaSound(SoundID.MenuTick);
            }
            else if (!player.LSSJ2Achieved)
            {
                SoundUtil.PlayVanillaSound(SoundID.MenuClose);
                Main.NewText("A legendary saiyan sometimes may lose complete control upon being pushed into a critical state.");
            }
        }
        private void TrySelectingSSJG(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.SSJGAchieved && !player.IsPlayerLegendary())
            {
                MenuSelection = MenuSelectionID.SSJG;
                SoundUtil.PlayVanillaSound(SoundID.MenuTick);
            }
            else if (!player.LSSJAchieved)
            {
                SoundUtil.PlayVanillaSound(SoundID.MenuClose);
                Main.NewText("The godlike power of the lunar star could awaken something beyond mortal comprehension.");
            }
        }
        private void TrySelectingSSJS(UIMouseEvent evt, UIElement listeningelement)
        {
            Player player = Main.LocalPlayer;
            if (player.name == "Nuova")
            {
                MenuSelection = MenuSelectionID.Spectrum;
                SoundUtil.PlayVanillaSound(SoundID.MenuTick);
            }
        }

    }
}