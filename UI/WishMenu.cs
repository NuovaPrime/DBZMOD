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
            base.OnInitialize();

            wishbackPanelImage = new UIImage(GFX.WishBackPanel);
            wishbackPanelImage.Width.Set(GFX.WishBackPanel.Width, 0f);
            wishbackPanelImage.Height.Set(GFX.WishBackPanel.Height, 0f);
            wishbackPanelImage.Left.Set(-12, 0f);
            wishbackPanelImage.Top.Set(-12, 0f);
            wishbackPanel.Append(wishbackPanelImage);

            descTextValue = "";

            InitText(ref titleText, "I Wish for...", 0.6f, 4, 6, Color.Yellow, wishbackPanel);

            InitText(ref descText, descTextValue, 1, 10, 62, Color.Yellow, wishbackPanel);

            InitButton(ref WishButtonTest, GFX.WishIconEmpty, new MouseEvent(SelectButtonTest), 10, 20, wishbackPanel);

        }
        public void SelectButtonTest(UIMouseEvent evt, UIElement listeningelement)
        {
            descTextValue = "Test";
        }
    }
}