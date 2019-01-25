using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using DBZMOD.Enums;
using DBZMOD.Util;

namespace DBZMOD.UI
{
    internal class WishMenu : EasyMenu
    {
        public static bool menuvisible = false;
        private UIText _titleText;
        private UIText _descText;
        private UIText _wishText;
        private UIText _grantText;
        public UIImage backPanelImage;
        private UIImageButton _wishButtonPower;
        private UIImageButton _wishButtonWealth;
        private UIImageButton _wishButtonImmortality;
        private UIImageButton _wishButtonGenetics;
        private UIImageButton _wishButtonSkill;
        private UIImageButton _wishButtonAwakening;
        private UIImageButton _grantButton;
        private static string _descTextValue = "Select one of the wishes above to grant your deepest desire." +
            "\nCertain wishes have limits.";
        private static string _wishTextValue = "";


        public static WishSelectionID wishSelection;

        public override void OnInitialize()
        {
            base.OnInitialize();

            backPanel = new UIPanel();
            backPanel.Width.Set(364f, 0f);
            backPanel.Height.Set(192f, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 2f - backPanel.Height.Pixels / 2f, 0f);
            backPanel.BackgroundColor = new Color(0, 0, 0, 0);

            backPanelImage = new UIImage(Gfx.wishBackPanel);
            backPanelImage.Width.Set(Gfx.wishBackPanel.Width, 0f);
            backPanelImage.Height.Set(Gfx.wishBackPanel.Height, 0f);
            backPanelImage.Left.Set(-12, 0f);
            backPanelImage.Top.Set(-12, 0f);

            InitText(ref _titleText, "I Wish for...", 0.66f, 8, 6, Color.Yellow, backPanelImage);

            InitText(ref _wishText, _wishTextValue, 0.8f, 10, 82, new Color(244, 203, 39), backPanelImage);

            InitText(ref _descText, _descTextValue, 0.66f, 10, 100, Color.Yellow, backPanelImage);

            InitButton(ref _wishButtonPower, Gfx.wishforPower, new MouseEvent(SelectButtonPower), 10, 22, backPanelImage);

            InitButton(ref _wishButtonWealth, Gfx.wishforWealth, new MouseEvent(SelectButtonWealth), 55, 22, backPanelImage);

            InitButton(ref _wishButtonImmortality, Gfx.wishforImmortality, new MouseEvent(SelectButtonImmortality), 100, 22, backPanelImage);

            InitButton(ref _wishButtonGenetics, Gfx.wishforGenetics, new MouseEvent(SelectButtonGenetics), 145, 22, backPanelImage);

            InitButton(ref _wishButtonSkill, Gfx.wishforSkill, new MouseEvent(SelectButtonSkill), 190, 22, backPanelImage);

            InitButton(ref _wishButtonAwakening, Gfx.wishforAwakening, new MouseEvent(SelectButtonAwakening), 235, 22, backPanelImage);
            
            InitButton(ref _grantButton, Gfx.grantButton, new MouseEvent(GrantWish), Gfx.wishBackPanel.Width - Gfx.grantButton.Width - 12, Gfx.wishBackPanel.Height - Gfx.grantButton.Height - 12, backPanelImage);
            
            InitText(ref _grantText, "Grant Wish", 0.66f, 14, -12, Color.Yellow, _grantButton);

            backPanel.Append(backPanelImage);

            Append(backPanel);
        }
        
        private void SelectButtonPower(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            _wishTextValue = "Power";
            _descTextValue = "Wish for a permanent increase in\nMaximum Health, Maximum Ki and Damage.\nWish limit = " + MyPlayer.POWER_WISH_MAXIMUM + ", Wishes left = " + modplayer.powerWishesLeft;                
            wishSelection = WishSelectionID.Power;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }

        private void SelectButtonWealth(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            _wishTextValue = "Wealth";
            _descTextValue = "Wish for money beyond your wildest dreams,\n10 Platinum coins + lucky coin.\nWish limit = ∞, Wishes left = ∞";
            wishSelection = WishSelectionID.Wealth;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }

        private void SelectButtonImmortality(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            _wishTextValue = "Immortality";
            _descTextValue = "Wish for infinite life, reviving at full life\nfor the next 3 deaths.\nWish limit = 1, Wishes left = " + modplayer.immortalityWishesLeft;
            wishSelection = WishSelectionID.Immortality;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }

        private void SelectButtonGenetics(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            _wishTextValue = "Genetic Reroll";
            _descTextValue = "Wish for a new [guaranteed] genetic trait.\nAll traits have an equal chance of being rolled.\nWish limit = ∞, Wishes left = ∞";
            wishSelection = WishSelectionID.Genetic;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }

        private void SelectButtonSkill(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            _wishTextValue = "Not Added Yet";
            _descTextValue = "";
            //wishTextValue = "Skill";
            //descTextValue = "Wish for a powerful attack. What you get depends on\nhow many times you have wished this.\nWish limit = 3, Wishes left = " + modplayer.SkillWishesLeft;
            wishSelection = WishSelectionID.Skill;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }

        private void SelectButtonAwakening(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            _wishTextValue = "Awakening";
            _descTextValue = "Wish to awaken your latent power,\nUnlocks the next available form.\nWish limit = 3, Wishes left = " + modplayer.awakeningWishesLeft;
            wishSelection = WishSelectionID.Awakening;
            Main.PlaySound(SoundID.MenuTick);
            Initialize();
            DBZMOD.ActivateWishmenu();
        }

        private void GrantWish(UIMouseEvent evt, UIElement listeningelement)
        {
            Player player = Main.LocalPlayer;
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            bool usedWish = false;
            switch (wishSelection)
            {
                case WishSelectionID.Power:
                    if (modplayer.powerWishesLeft > 0)
                    {
                        usedWish = true;
                        DoPowerWish();
                        SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
                    }
                    else
                    {
                        Main.PlaySound(SoundID.MenuClose);
                    }
                    break;
                case WishSelectionID.Wealth:
                    usedWish = true;
                    DoWealthWish();
                    SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
                    break;
                case WishSelectionID.Immortality:
                    if (modplayer.immortalityWishesLeft > 0)
                    {
                        usedWish = true;
                        DoImmortalityWish();
                        SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
                    }
                    else
                    {
                        Main.PlaySound(SoundID.MenuClose);
                    }
                    break;
                case WishSelectionID.Genetic:
                    usedWish = true;
                    DoGeneticWish();
                    SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
                    break;
                case WishSelectionID.Awakening:
                    if (modplayer.awakeningWishesLeft > 0)
                    {
                        usedWish = true;
                        DoAwakeningWish();
                        SoundUtil.PlayCustomSound("Sounds/WishGranted", player.Center);
                    }
                    else
                    {
                        Main.PlaySound(SoundID.MenuClose);
                    }
                    break;
                default:
                    break;
            }

            if (usedWish)
            {
                wishSelection = WishSelectionID.None;
                DBZWorld.DestroyAndRespawnDragonBalls();
                modplayer.wishActive = false;
                Main.PlaySound(SoundID.MenuClose);
            }

            Initialize();
            DBZMOD.ActivateWishmenu();
        }  

        private void DoPowerWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            modplayer.kiMax3 += 500;
            modplayer.powerWishesLeft -= 1;
            menuvisible = false;
            modplayer.wishActive = false;
        }

        private void DoWealthWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            player.QuickSpawnItem(ItemID.PlatinumCoin, 10);
            player.QuickSpawnItem(ItemID.LuckyCoin, 1);
            menuvisible = false;
            modplayer.wishActive = false;
        }

        private void DoImmortalityWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            modplayer.immortalityRevivesLeft += 3;
            modplayer.immortalityWishesLeft -= 1;
            menuvisible = false;
            modplayer.wishActive = false;
        }

        private void DoGeneticWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            modplayer.playerTrait = null;
            modplayer.ChooseTraitNoLimits();
            menuvisible = false;
            modplayer.wishActive = false;
        }

        private void DoAwakeningWish()
        {
            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Player player = Main.LocalPlayer;
            modplayer.awakeningWishesLeft -= 1;
            modplayer.AwakeningFormUnlock();
            menuvisible = false;
            modplayer.wishActive = false;
        }
    }
}