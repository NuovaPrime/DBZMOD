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

namespace DBZMOD.UI
{
    internal class TransMenu : EasyMenu
    {
        public static bool menuvisible = false;
        private UIText titleText;
        private UIText ssjbuttontext;
        private UIText ssj2buttontext;
        private UIText ssj3buttontext;
        private UIText lssjbuttontext;
        private UIImageButton ssjButtonTexture;
        private UIImageButton ssj2ButtonTexture;
        private UIImageButton ssj3ButtonTexture;
        private UIImageButton lssjButtonTexture;

        private UIImage lineSSJ1toSSJ2;
        private UIImage lineSSJ2toSSJ3;
        private UIImage lineSSJ1toLSSJ;

        public static int MenuSelection = 0;
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
            backPanel.BackgroundColor = new Color(100, 100, 100);

            float Row1_OffsetX = 0.0f;

            InitText(ref titleText, "Transformation Tree", 75, 0.0f, Color.White);

            Row1_OffsetX = PADDINGX;
            InitButton(ref ssjButtonTexture, GFX.SSJ1ButtonImage, new MouseEvent(TrySelectingSSJ1),
                Row1_OffsetX, 
                PADDINGY);

            Row1_OffsetX = PADDINGX + GFX.SSJ1ButtonImage.Width;
            InitImage(ref lineSSJ1toSSJ2, GFX.LineHorizontalImage,
                Row1_OffsetX,
                PADDINGY + GFX.SSJ1ButtonImage.Height / 2);

            Row1_OffsetX = PADDINGX + GFX.SSJ1ButtonImage.Width + GFX.LineHorizontalImage.Width;
            InitButton(ref ssj2ButtonTexture, GFX.SSJ2ButtonImage, new MouseEvent(TrySelectingSSJ2),
                Row1_OffsetX, 
                PADDINGY);

            Row1_OffsetX = PADDINGX + GFX.SSJ1ButtonImage.Width * 2 + GFX.LineHorizontalImage.Width;
            InitImage(ref lineSSJ2toSSJ3, GFX.LineHorizontalImage,
                Row1_OffsetX,
                PADDINGY + GFX.SSJ1ButtonImage.Height / 2);

            Row1_OffsetX = PADDINGX + GFX.SSJ2ButtonImage.Width * 2 + GFX.LineHorizontalImage.Width * 2;
            InitButton(ref ssj3ButtonTexture, GFX.SSJ3ButtonImage, new MouseEvent(TrySelectingSSJ3),
                Row1_OffsetX, 
                PADDINGY);

            Row1_OffsetX = PADDINGX;
            InitImage(ref lineSSJ1toLSSJ, GFX.LineLImage,
                Row1_OffsetX,
                PADDINGY + GFX.SSJ1ButtonImage.Height);

            InitButton(ref lssjButtonTexture, GFX.LSSJButtonImage, new MouseEvent(TrySelectingLSSJ), 
                PADDINGX + GFX.SSJ1ButtonImage.Width + GFX.LineHorizontalImage.Width, 
                GFX.SSJ1ButtonImage.Height + PADDINGY);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            //SSJ1
            ssjButtonTexture.SetVisibility(player.SSJ1Achieved ? 1.0f : 0.0f, player.SSJ1Achieved ? 0.5f : 0.0f);

            //SSJ2
            ssj2ButtonTexture.SetVisibility(player.SSJ2Achieved && !player.hasLegendary ? 1.0f : 0.0f, player.SSJ2Achieved && !player.hasLegendary ? 0.5f : 0.0f);
            lineSSJ1toSSJ2.ImageScale = player.SSJ2Achieved && !player.hasLegendary ? 1.0f : 0.0f;

            //SSJ3
            ssj3ButtonTexture.SetVisibility(player.SSJ3Achieved && !player.hasLegendary ? 1.0f : 0.0f, player.SSJ3Achieved && !player.hasLegendary ? 0.5f : 0.0f);
            lineSSJ2toSSJ3.ImageScale = player.SSJ3Achieved && !player.hasLegendary ? 1.0f : 0.0f;

            //LSSJ
            lssjButtonTexture.SetVisibility(player.LSSJAchieved ? 1.0f : 0.0f, player.LSSJAchieved ? 0.5f : 0.0f);
            lineSSJ1toLSSJ.ImageScale = player.LSSJAchieved ? 1.0f : 0.0f;

        }

        private void TrySelectingSSJ1(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if(player.SSJ1Achieved)
            {
                MenuSelection = 1;
                Main.PlaySound(SoundID.MenuTick);
                Main.NewText("SSJ1 Mastery = " + player.MasteryLevel1 + "/" + player.MasteryMax1);
            }
        }

        private void TrySelectingSSJ2(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if(player.SSJ2Achieved && !player.hasLegendary)
            {
                MenuSelection = 2;
                Main.PlaySound(SoundID.MenuTick);
                Main.NewText("SSJ2 Mastery = " + player.MasteryLevel2 + "/" + player.MasteryMax2);
            }
        }
        private void TrySelectingSSJ3(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if(player.SSJ3Achieved && !player.hasLegendary)
            {
                MenuSelection = 3;
                Main.PlaySound(SoundID.MenuTick);
                Main.NewText("SSJ3 Mastery = " + player.MasteryLevel3 + " / " + player.MasteryMax3);
            }
        }

        private void TrySelectingLSSJ(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.LSSJAchieved)
            {
                MenuSelection = 4;
                Main.PlaySound(SoundID.MenuTick);
            }
        }

    }
}