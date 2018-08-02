﻿using System;
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
    internal class TransMenu : UIState
    {
        public UIPanel backPanel;
        private UIText ssjbuttontext;
        private UIText ssj2buttontext;
        private UIText ssj3buttontext;
        public static bool menuvisible = false;
        private UIImageButton ssjButtonTexture;
        private UIImageButton ssj2ButtonTexture;
        private UIImageButton ssj3ButtonTexture;
        private const float padding = 5f;
        public static int MenuSelection = 0;
        public static bool SSJ1On;
        public static bool SSJ2On;
        public static bool SSJ3On;
        //MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();;

        public override void OnInitialize()
        {
            backPanel = new UIPanel();
            backPanel.Width.Set(360f, 0f);
            backPanel.Height.Set(240f, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 2f - backPanel.Height.Pixels / 2f, 0f);
            backPanel.BackgroundColor = new Color(73, 94, 171);
            backPanel.OnMouseDown += new MouseEvent(DragStart);
            backPanel.OnMouseUp += new MouseEvent(DragEnd);
            base.Append(backPanel);
            base.OnInitialize();

            var SSJ1Button = GFX.SSJ1ButtonImage;
            ssjButtonTexture = new UIImageButton(SSJ1Button);
            ssjButtonTexture.Width.Set(SSJ1Button.Width, 0f);
            ssjButtonTexture.Height.Set(SSJ1Button.Width, 0f);
            ssjButtonTexture.Left.Set(padding, 0f);
            ssjButtonTexture.Top.Set(padding, 0f);
            ssjButtonTexture.OnClick += TrySelectingSSJ1;
            backPanel.Append(ssjButtonTexture);
            
            ssjbuttontext = new UIText("Super Saiyan");
            ssjbuttontext.Width.Set(32f, 0f);
            ssjbuttontext.Height.Set(32f, 0f);
            ssjbuttontext.Left.Set(padding - 4f, 0f);
            ssjbuttontext.Top.Set(padding + 25f, 0f);
            ssjButtonTexture.Append(ssjbuttontext);

            var SSJ2Button = GFX.SSJ2ButtonImage;
            ssj2ButtonTexture = new UIImageButton(SSJ2Button);
            ssj2ButtonTexture.Width.Set(SSJ2Button.Width, 0f);
            ssj2ButtonTexture.Height.Set(SSJ2Button.Width, 0f);
            ssj2ButtonTexture.Left.Set(padding + SSJ1Button.Width, 0f);
            ssj2ButtonTexture.Top.Set(padding, 0f);
            ssj2ButtonTexture.OnClick += TrySelectingSSJ2;
            backPanel.Append(ssj2ButtonTexture);

            ssj2buttontext = new UIText("Super Saiyan 2");
            ssj2buttontext.Width.Set(32f, 0f);
            ssj2buttontext.Height.Set(32f, 0f);
            ssj2buttontext.Left.Set(padding - 4f, 0f);
            ssj2buttontext.Top.Set(padding + 25f, 0f);
            ssj2ButtonTexture.Append(ssj2buttontext);

            var SSJ3Button = GFX.SSJ3ButtonImage;
            ssj3ButtonTexture = new UIImageButton(SSJ3Button);
            ssj3ButtonTexture.Width.Set(SSJ3Button.Width, 0f);
            ssj3ButtonTexture.Height.Set(SSJ3Button.Width, 0f);
            ssj3ButtonTexture.Left.Set(padding + SSJ2Button.Width * 2, 0f);
            ssj3ButtonTexture.Top.Set(padding, 0f);
            ssj3ButtonTexture.OnClick += TrySelectingSSJ3;
            backPanel.Append(ssj3ButtonTexture);

            ssj3buttontext = new UIText("Super Saiyan 3");
            ssj3buttontext.Width.Set(32f, 0f);
            ssj3buttontext.Height.Set(32f, 0f);
            ssj3buttontext.Left.Set(padding - 4f, 0f);
            ssj3buttontext.Top.Set(padding + 25f, 0f);
            ssj3ButtonTexture.Append(ssj3buttontext);
        }

        private void TrySelectingSSJ1(UIMouseEvent evt, UIElement listeningelement)
        {
            if(SSJ1On)
            {
                MenuSelection = 1;
                Main.PlaySound(SoundID.MenuTick);
                //Main.NewText("SSJ1 Mastery =" + player.MasteryLevel1 + "/" + player.MasteryMax1);
            }
            if (!SSJ1On)
            {
                Main.PlaySound(SoundID.MenuClose);
            }
        }
        private void TrySelectingSSJ2(UIMouseEvent evt, UIElement listeningelement)
        {
            if (SSJ2On)
            {
                MenuSelection = 2;
                Main.PlaySound(SoundID.MenuTick);
                //Main.NewText("SSJ2 Mastery =" + player.MasteryLevel2 + "/" + player.MasteryMax2);
            }
            if (!SSJ2On)
            {
                Main.PlaySound(SoundID.MenuClose);
            }
        }
        private void TrySelectingSSJ3(UIMouseEvent evt, UIElement listeningelement)
        {
            if (SSJ3On)
            {
                MenuSelection = 3;
                Main.PlaySound(SoundID.MenuTick);
                //Main.NewText("SSJ3 Mastery = " + player.MasteryLevel3 + " / " + player.MasteryMax3);
            }
            if (!SSJ3On)
            {
                Main.PlaySound(SoundID.MenuClose);
            }
        }
        Vector2 offset;
        public bool dragging = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            offset = new Vector2(evt.MousePosition.X - backPanel.Left.Pixels, evt.MousePosition.Y - backPanel.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            backPanel.Left.Set(end.X - offset.X, 0f);
            backPanel.Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (backPanel.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                backPanel.Left.Set(MousePosition.X - offset.X, 0f);
                backPanel.Top.Set(MousePosition.Y - offset.Y, 0f);
                Recalculate();
            }
        }
    }
}