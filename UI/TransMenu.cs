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
        private UIText ssjbuttontext;
        private UIText ssj2buttontext;
        private UIText ssj3buttontext;
        private UIText lssjbuttontext;
        private UIImageButton ssjButtonTexture;
        private UIImageButton ssj2ButtonTexture;
        private UIImageButton ssj3ButtonTexture;
        private UIImageButton lssjButtonTexture;
        public static int MenuSelection = 0;
        public static bool SSJ1On;
        public static bool SSJ2On;
        public static bool SSJ3On;
        public static bool LSSJOn;
        private Player player;
        public const float PADDING = 7f;

        public override void OnInitialize()
        {
            base.OnInitialize();
            backPanel.BackgroundColor = new Color(100, 100, 100);

            InitButton(ref ssjButtonTexture, GFX.SSJ1ButtonImage, new MouseEvent(TrySelectingSSJ1), PADDING);
            InitText(ref ssjbuttontext, "SSJ1", -5.0f, 77.0f, Color.White, ssjButtonTexture);

            InitButton(ref ssj2ButtonTexture, GFX.SSJ2ButtonImage, new MouseEvent(TrySelectingSSJ2), PADDING + GFX.SSJ1ButtonImage.Width);
            InitText(ref ssj2buttontext, "SSJ2", -5.0f, 77.0f, Color.White, ssj2ButtonTexture);

            InitButton(ref ssj3ButtonTexture, GFX.SSJ3ButtonImage, new MouseEvent(TrySelectingSSJ3), PADDING + GFX.SSJ2ButtonImage.Width * 2);
            InitText(ref ssj3buttontext, "SSJ3", -5.0f, 77.0f, Color.White, ssj3ButtonTexture);

            InitButton(ref lssjButtonTexture, GFX.LSSJButtonImage, new MouseEvent(TrySelectingLSSJ), PADDING + GFX.SSJ3ButtonImage.Width * 3);
            InitText(ref lssjbuttontext, "LSSJ", -5.0f, 77.0f, Color.White, lssjButtonTexture);
        }

        private void TrySelectingSSJ1(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (SSJ1On)
            {
                MenuSelection = 1;
                Main.PlaySound(SoundID.MenuTick);
                Main.NewText("SSJ1 Mastery = " + player.MasteryLevel1 + "/" + player.MasteryMax1);
            }
            if (!SSJ1On)
            {
                Main.PlaySound(SoundID.MenuClose);
            }
        }
        private void TrySelectingSSJ2(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (SSJ2On && !player.hasLegendary)
            {
                MenuSelection = 2;
                Main.PlaySound(SoundID.MenuTick);
                Main.NewText("SSJ2 Mastery = " + player.MasteryLevel2 + "/" + player.MasteryMax2);
            }
            if (!SSJ2On)
            {
                Main.PlaySound(SoundID.MenuClose);
            }
        }
        private void TrySelectingSSJ3(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (SSJ3On && !player.hasLegendary)
            {
                MenuSelection = 3;
                Main.PlaySound(SoundID.MenuTick);
                Main.NewText("SSJ3 Mastery = " + player.MasteryLevel3 + " / " + player.MasteryMax3);
            }
            if (!SSJ3On)
            {
                Main.PlaySound(SoundID.MenuClose);
            }
        }

        private void TrySelectingLSSJ(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (LSSJOn)
            {
                MenuSelection = 4;
                Main.PlaySound(SoundID.MenuTick);
            }
            if (!LSSJOn)
            {
                Main.PlaySound(SoundID.MenuClose);
            }
        }

    }
}