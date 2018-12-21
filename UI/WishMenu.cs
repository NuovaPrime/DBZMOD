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
        public UIImage wishbackPanelImage;
        private UIImageButton WishButtonTest;


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

            InitText(ref titleText, "Test", 55, -32, Color.White, wishbackPanel);

            InitButton(ref WishButtonTest, GFX.WishIconEmpty, new MouseEvent(SelectButtonTest), 10, 20, wishbackPanelImage);

        }
        public void SelectButtonTest(UIMouseEvent evt, UIElement listeningelement)
        {

        }
    }
}