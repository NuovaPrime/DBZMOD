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
        public UIImage wishbackPanelImage;
        private UIImageButton WishButtonTest;
        private string descTextValue;


        public static MenuSelectionID MenuSelection;
        private Player player;

        public override void OnInitialize()
        {
            backPanel = new UIPanel();
            backPanel.Width.Set(364f, 0f);
            backPanel.Height.Set(192f, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 2f - backPanel.Height.Pixels / 2f, 0f);
            backPanel.BackgroundColor = new Color(0, 0, 0, 0);
            Append(backPanel);

            wishbackPanelImage = new UIImage(GFX.WishBackPanel);
            wishbackPanelImage.Width.Set(GFX.WishBackPanel.Width, 0f);
            wishbackPanelImage.Height.Set(GFX.WishBackPanel.Height, 0f);
            wishbackPanelImage.Left.Set(-12, 0f);
            wishbackPanelImage.Top.Set(-12, 0f);
            backPanel.Append(wishbackPanelImage);

            descTextValue = "";

            InitText(ref titleText, "I Wish for...", 0.6f, 4, 6, Color.Yellow, backPanel);

            InitText(ref descText, descTextValue, 1, 10, 62, Color.Yellow, backPanel);

            InitButton(ref WishButtonTest, GFX.WishIconEmpty, new MouseEvent(SelectButtonTest), 10, 20, backPanel);

            base.OnInitialize();

        }
        public void SelectButtonTest(UIMouseEvent evt, UIElement listeningelement)
        {
            descTextValue = "Test";
        }
    }
}