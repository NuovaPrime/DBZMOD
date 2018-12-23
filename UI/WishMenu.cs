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
    internal class WishMenu : EasyMenu
    {
        public static bool menuvisible = false;
        private UIText titleText;
        private UIText descText;
        private UIText wishText;
        private UIText grantText;
        public UIImage backPanelImage;
        private UIImageButton WishButtonPower;
        private UIImageButton WishButtonWealth;
        private UIImageButton WishButtonImmortality;
        private UIImageButton WishButtonGenetics;
        private UIImageButton WishButtonSkill;
        private UIImageButton WishButtonAwakening;
        private UIImageButton GrantButton;
        private static string descTextValue = "Select one of the wishes above to grant your deepest desire." +
            "\nCertain wishes have limits.";
        private static string wishTextValue = "";


        public static WishSelectionID WishSelection;

        public override void OnInitialize()
        {
            base.OnInitialize();

            backPanel = new UIPanel();
            backPanel.Width.Set(364f, 0f);
            backPanel.Height.Set(192f, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 2f - backPanel.Height.Pixels / 2f, 0f);
            backPanel.BackgroundColor = new Color(0, 0, 0, 0);

            backPanelImage = new UIImage(GFX.WishBackPanel);
            backPanelImage.Width.Set(GFX.WishBackPanel.Width, 0f);
            backPanelImage.Height.Set(GFX.WishBackPanel.Height, 0f);
            backPanelImage.Left.Set(-12, 0f);
            backPanelImage.Top.Set(-12, 0f);

            InitText(ref titleText, "I Wish for...", 0.66f, 8, 6, Color.Yellow, backPanelImage);

            InitText(ref wishText, wishTextValue, 0.8f, 10, 82, new Color(244, 203, 39), backPanelImage);

            InitText(ref descText, descTextValue, 0.66f, 10, 100, Color.Yellow, backPanelImage);

            InitButton(ref WishButtonPower, GFX.WishforPower, new MouseEvent(SelectButtonPower), 10, 22, backPanelImage);

            InitButton(ref WishButtonWealth, GFX.WishforWealth, new MouseEvent(SelectButtonWealth), 55, 22, backPanelImage);

            InitButton(ref WishButtonImmortality, GFX.WishforImmortality, new MouseEvent(SelectButtonImmortality), 100, 22, backPanelImage);

            InitButton(ref WishButtonGenetics, GFX.WishforGenetics, new MouseEvent(SelectButtonGenetics), 145, 22, backPanelImage);

            InitButton(ref WishButtonSkill, GFX.WishforSkill, new MouseEvent(SelectButtonSkill), 190, 22, backPanelImage);

            InitButton(ref WishButtonAwakening, GFX.WishforAwakening, new MouseEvent(SelectButtonAwakening), 235, 22, backPanelImage);
            
            InitButton(ref GrantButton, GFX.GrantButton, new MouseEvent(GrantWish), GFX.WishBackPanel.Width - GFX.GrantButton.Width - 12, GFX.WishBackPanel.Height - GFX.GrantButton.Height - 12, backPanelImage);
            
            InitText(ref grantText, "Grant Wish", 0.66f, 14, -12, Color.Yellow, GrantButton);

            backPanel.Append(backPanelImage);

            Append(backPanel);
        }
        
        private void SelectButtonPower(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            wishTextValue = "Power";
            descTextValue = "Wish for a permanent increase in\nMaximum Health, Maximum Ki and Damage.\nWish limit = 5, Wishes left = " + modplayer.PowerWishesLeft;                
            WishSelection = WishSelectionID.Power;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }
        private void SelectButtonWealth(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            wishTextValue = "Wealth";
            descTextValue = "Wish for money beyond your wildest dreams,\n10 Platinum coins + lucky coin.\nWish limit = ∞, Wishes left = ∞";
            WishSelection = WishSelectionID.Wealth;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }
        private void SelectButtonImmortality(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            wishTextValue = "Immortality";
            descTextValue = "Wish for infinite life, reviving at full life\nfor the next 3 deaths.\nWish limit = 1, Wishes left = " + modplayer.ImmortalityWishesLeft;
            WishSelection = WishSelectionID.Immortality;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }
        private void SelectButtonGenetics(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            wishTextValue = "Genetic Reroll";
            descTextValue = "Wish for a new [guaranteed] genetic trait.\nAll traits have an equal chance of being rolled.\nWish limit = ∞, Wishes left = ∞";
            WishSelection = WishSelectionID.Genetic;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }
        private void SelectButtonSkill(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            wishTextValue = "Not Added Yet";
            descTextValue = "";
            //wishTextValue = "Skill";
            //descTextValue = "Wish for a powerful attack. What you get depends on\nhow many times you have wished this.\nWish limit = 3, Wishes left = " + modplayer.SkillWishesLeft;
            WishSelection = WishSelectionID.Skill;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }
        private void SelectButtonAwakening(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            wishTextValue = "Awakening";
            descTextValue = "Wish to awaken your latent power,\nUnlocks the next available form.\nWish limit = 3, Wishes left = " + modplayer.AwakeningWishesLeft;
            WishSelection = WishSelectionID.Awakening;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }
        private void GrantWish(UIMouseEvent evt, UIElement listeningelement)
        {
            Player player = Main.LocalPlayer;
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if(WishSelection == WishSelectionID.Power)
            {
                if(modplayer.PowerWishesLeft > 0)
                {
                    DoPowerWish();
                    SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
                }
                else
                {
                    Main.PlaySound(SoundID.MenuClose);
                }
            }
            if (WishSelection == WishSelectionID.Wealth)
            {
                DoWealthWish();
                SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
            }
            if (WishSelection == WishSelectionID.Immortality)
            {
                if(modplayer.ImmortalityWishesLeft > 0)
                {
                    DoImmortalityWish();
                    SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
                }
                else
                {
                    Main.PlaySound(SoundID.MenuClose);
                }
            }
            if (WishSelection == WishSelectionID.Genetic)
            {
                DoGeneticWish();
                SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
            }
            if (WishSelection == WishSelectionID.Awakening)
            {
                if (modplayer.AwakeningWishesLeft > 0)
                {
                    DoAwakeningWish();
                    SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
                }
                else
                {
                    Main.PlaySound(SoundID.MenuClose);
                }
            }
            Initialize();
            DBZMOD.ActivateWishmenu();
        }
        private void DoPowerWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            modplayer.PowerHealthBonus += 20;
            modplayer.KiMax3 += 500;
            modplayer.PowerWishMulti += 1.10f;
            modplayer.PowerWishesLeft -= 1;
            menuvisible = false;
            modplayer.WishActive = false;
        }
        private void DoWealthWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            player.QuickSpawnItem(ItemID.PlatinumCoin, 10);
            player.QuickSpawnItem(ItemID.LuckyCoin, 1);
            menuvisible = false;
            modplayer.WishActive = false;
        }
        private void DoImmortalityWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            modplayer.ImmortalityRevivesLeft += 3;
            modplayer.ImmortalityWishesLeft -= 1;
            menuvisible = false;
            modplayer.WishActive = false;
        }
        private void DoGeneticWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            modplayer.playerTrait = null;
            modplayer.ChooseTraitNoLimits();
            menuvisible = false;
            modplayer.WishActive = false;
        }
        private void DoAwakeningWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            modplayer.AwakeningWishesLeft -= 1;
            modplayer.AwakeningFormUnlock();
            menuvisible = false;
            modplayer.WishActive = false;
        }
    }
}