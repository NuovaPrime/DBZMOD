using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using DBZMOD.Enums;
using DBZMOD.Extensions;
using DBZMOD.Utilities;

namespace DBZMOD.UI.HairMenu
{
    internal class HairMenu : EasyMenu
    {
        public static bool menuVisible;
        private UIText _titleText;
        private UIImageButton _confirmButton;
        private UIImageButton _style1BaseButton;
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

            backPanelImage = new UIImage(GFX.hairBackPanel);
            backPanelImage.Width.Set(GFX.hairBackPanel.Width, 0f);
            backPanelImage.Height.Set(GFX.hairBackPanel.Height, 0f);
            backPanelImage.Left.Set(-12, 0f);
            backPanelImage.Top.Set(-12, 0f);

            InitText(ref _titleText, "Hair Selection Menu", 0.66f, 8, -4, Color.White, backPanelImage);

            InitButton(ref _style1BaseButton, GFX.style1BasePreview, new MouseEvent((evt, element) => SelectBaseHair(1, evt, element)), 10, 22, backPanelImage);

            /*InitButton(ref _wishButtonWealth, Gfx.wishforWealth, new MouseEvent(SelectButtonWealth), 55, 22, backPanelImage);

            InitButton(ref _wishButtonImmortality, Gfx.wishforImmortality, new MouseEvent(SelectButtonImmortality), 100, 22, backPanelImage);

            InitButton(ref _wishButtonGenetics, Gfx.wishforGenetics, new MouseEvent(SelectButtonGenetics), 145, 22, backPanelImage);

            InitButton(ref _wishButtonSkill, Gfx.wishforSkill, new MouseEvent(SelectButtonSkill), 190, 22, backPanelImage);

            InitButton(ref _wishButtonAwakening, Gfx.wishforAwakening, new MouseEvent(SelectButtonAwakening), 235, 22, backPanelImage);*/

            InitButton(ref _confirmButton, GFX.hairConfirmButton, new MouseEvent(ConfirmHair), GFX.hairBackPanel.Width - GFX.hairConfirmButton.Width - 12, GFX.hairBackPanel.Height - GFX.hairConfirmButton.Height - 7, backPanelImage);
            

            backPanel.Append(backPanelImage);

            Append(backPanel);
        }

        private void ConfirmHair(UIMouseEvent evt, UIElement listeningElement)
        {

        }
        private void SelectBaseHair(int Choice, UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            player.baseHairStyle = Choice;
            SoundHelper.PlayVanillaSound(SoundID.MenuTick);
        }
    }
}