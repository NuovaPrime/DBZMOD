using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using DBZMOD.Enums.Hair_Enums;
using DBZMOD.Extensions;
using DBZMOD.Utilities;

namespace DBZMOD.UI.HairMenu
{
    internal class HairMenu : EasyMenu
    {
        public static bool menuVisible;
        private UIText _titleText;
        private UIImageButton _confirmButton;
        public UIImage backPanelImage;

        public static BaseHairSelectionID baseHairSelectionId;

        public override void OnInitialize()
        {
            base.OnInitialize();

            backPanel = new UIPanel();
            backPanel.Width.Set(384f, 0f);
            backPanel.Height.Set(407f, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 2f - backPanel.Height.Pixels / 2f, 0f);
            backPanel.BackgroundColor = new Color(0, 0, 0, 0);

            backPanelImage = new UIImage(Gfx.hairBackPanel);
            backPanelImage.Width.Set(Gfx.hairBackPanel.Width, 0f);
            backPanelImage.Height.Set(Gfx.hairBackPanel.Height, 0f);
            backPanelImage.Left.Set(-12, 0f);
            backPanelImage.Top.Set(-12, 0f);

            InitText(ref _titleText, "Hair Selection Menu", 0.66f, 8, -4, Color.White, backPanelImage);

            /*InitButton(ref _wishButtonPower, Gfx.wishforPower, new MouseEvent(SelectButtonPower), 10, 22, backPanelImage);

            InitButton(ref _wishButtonWealth, Gfx.wishforWealth, new MouseEvent(SelectButtonWealth), 55, 22, backPanelImage);

            InitButton(ref _wishButtonImmortality, Gfx.wishforImmortality, new MouseEvent(SelectButtonImmortality), 100, 22, backPanelImage);

            InitButton(ref _wishButtonGenetics, Gfx.wishforGenetics, new MouseEvent(SelectButtonGenetics), 145, 22, backPanelImage);

            InitButton(ref _wishButtonSkill, Gfx.wishforSkill, new MouseEvent(SelectButtonSkill), 190, 22, backPanelImage);

            InitButton(ref _wishButtonAwakening, Gfx.wishforAwakening, new MouseEvent(SelectButtonAwakening), 235, 22, backPanelImage);*/

            InitButton(ref _confirmButton, Gfx.hairConfirmButton, new MouseEvent(ConfirmHair), Gfx.hairBackPanel.Width - Gfx.hairConfirmButton.Width - 12, Gfx.hairBackPanel.Height - Gfx.hairConfirmButton.Height - 7, backPanelImage);
            

            backPanel.Append(backPanelImage);

            Append(backPanel);
        }

        private void ConfirmHair(UIMouseEvent evt, UIElement listeningElement)
        {

        }
    }
}