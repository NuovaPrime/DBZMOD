using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using DBZMOD.Enums;
using DBZMOD.Extensions;
using DBZMOD.Transformations;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework.Graphics;

namespace DBZMOD.UI.HairMenu
{
    internal class HairMenu : EasyMenu
    {
        public static bool menuVisible;
        private UIText _titleText;
        private UIText _descText;
        private UIText _gokuText;
        private UIText _gogetaText;
        private UIText _vegetaText;
        private UIText _raditzText;
        private UIText _brolyText;
        private UIImageButton _confirmButton;
        private UIHoverImageButton _keepHairButton;
        #region Style buttons
        private UIImageButton _style1BaseButton;
        private UIImageButton _style1SSJButton;
        private UIImageButton _style1SSJ2Button;
        private UIImageButton _style1SSJ3Button;
        private UIImageButton _style1SSJ4Button;
        private UIImageButton _style2BaseButton;
        private UIImageButton _style2SSJButton;
        private UIImageButton _style2SSJ2Button;
        private UIImageButton _style2SSJ3Button;
        private UIImageButton _style2SSJ4Button;
        private UIImageButton _style3BaseButton;
        private UIImageButton _style3SSJButton;
        private UIImageButton _style3SSJ2Button;
        private UIImageButton _style3SSJ3Button;
        private UIImageButton _style3SSJ4Button;
        private UIImageButton _style4BaseButton;
        private UIImageButton _style4SSJButton;
        private UIImageButton _style4SSJ2Button;
        private UIImageButton _style4SSJ3Button;
        private UIImageButton _style4SSJ4Button;
        private UIImageButton _style5BaseButton;
        private UIImageButton _style5SSJButton;
        private UIImageButton _style5SSJ2Button;
        private UIImageButton _style5SSJ3Button;
        private UIImageButton _style5SSJ4Button;
        #endregion
        public UIImage backPanelImage;

        private bool baseChecked = false;
        private bool ssjChecked = false;
        private bool ssj2Checked = false;
        private bool ssj3Checked = false;
        private bool ssj4Checked = false;

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

            InitText(ref _titleText, "Hair Selection Menu", 0.88f, 8, -16, Color.White, backPanelImage);

            InitText(ref _descText, "Choose a style for your hair in each of the following columns, \nyou can mix and match from any category. \nYou must select a style for each form presented below.", 0.62f, 8, 6, Color.White, backPanelImage);

            InitText(ref _gokuText, "Style 1: Goku", 0.56f, 12, 58, Color.White, backPanelImage);

            InitButton(ref _style1BaseButton, GFX.style1BasePreview, new MouseEvent((evt, element) => SelectBaseHair(1, evt, element)), 32, 80, backPanelImage);

            InitButton(ref _style1SSJButton, GFX.style1SSJPreview, new MouseEvent((evt, element) => SelectSSJHair(1, evt, element)), 102, 80, backPanelImage);

            InitButton(ref _style1SSJ2Button, GFX.style1SSJ2Preview, new MouseEvent((evt, element) => SelectSSJ2Hair(1, evt, element)), 172, 80, backPanelImage);

            InitButton(ref _style1SSJ3Button, GFX.style1SSJ3Preview, new MouseEvent((evt, element) => SelectSSJ3Hair(1, evt, element)), 242, 74, backPanelImage);

            InitButton(ref _style1SSJ4Button, GFX.style1SSJ4Preview, new MouseEvent((evt, element) => SelectSSJ4Hair(1, evt, element)), 312, 76, backPanelImage);

            InitText(ref _gogetaText, "Style 2: Gogeta", 0.56f, 12, 120, Color.White, backPanelImage);

            InitButton(ref _style2BaseButton, GFX.style2BasePreview, new MouseEvent((evt, element) => SelectBaseHair(2, evt, element)), 36, 142, backPanelImage);

            InitButton(ref _style2SSJButton, GFX.style2SSJPreview, new MouseEvent((evt, element) => SelectSSJHair(2, evt, element)), 102, 142, backPanelImage);

            InitButton(ref _style2SSJ2Button, GFX.style2SSJ2Preview, new MouseEvent((evt, element) => SelectSSJ2Hair(2, evt, element)), 172, 142, backPanelImage);

            InitButton(ref _style2SSJ3Button, GFX.style2SSJ3Preview, new MouseEvent((evt, element) => SelectSSJ3Hair(2, evt, element)), 242, 136, backPanelImage);

            InitButton(ref _style2SSJ4Button, GFX.style2SSJ4Preview, new MouseEvent((evt, element) => SelectSSJ4Hair(2, evt, element)), 312, 138, backPanelImage);

            InitText(ref _gogetaText, "Style 3: Vegeta", 0.56f, 12, 182, Color.White, backPanelImage);

            InitButton(ref _style3BaseButton, GFX.style3BasePreview, new MouseEvent((evt, element) => SelectBaseHair(3, evt, element)), 36, 204, backPanelImage);

            InitButton(ref _style3SSJButton, GFX.style3SSJPreview, new MouseEvent((evt, element) => SelectSSJHair(3, evt, element)), 102, 204, backPanelImage);

            InitButton(ref _style3SSJ2Button, GFX.style3SSJ2Preview, new MouseEvent((evt, element) => SelectSSJ2Hair(3, evt, element)), 172, 204, backPanelImage);

            InitButton(ref _style3SSJ3Button, GFX.style3SSJ3Preview, new MouseEvent((evt, element) => SelectSSJ3Hair(3, evt, element)), 242, 200, backPanelImage);

            InitButton(ref _style3SSJ4Button, GFX.style3SSJ4Preview, new MouseEvent((evt, element) => SelectSSJ4Hair(3, evt, element)), 312, 202, backPanelImage);

            InitText(ref _gogetaText, "Style 4: Raditz", 0.56f, 12, 244, Color.White, backPanelImage);

            InitButton(ref _style4BaseButton, GFX.style4BasePreview, new MouseEvent((evt, element) => SelectBaseHair(4, evt, element)), 36, 266, backPanelImage);

            InitButton(ref _style4SSJButton, GFX.style4SSJPreview, new MouseEvent((evt, element) => SelectSSJHair(4, evt, element)), 102, 266, backPanelImage);

            InitButton(ref _style4SSJ2Button, GFX.style4SSJ2Preview, new MouseEvent((evt, element) => SelectSSJ2Hair(4, evt, element)), 172, 266, backPanelImage);

            InitButton(ref _style4SSJ3Button, GFX.style4SSJ3Preview, new MouseEvent((evt, element) => SelectSSJ3Hair(4, evt, element)), 242, 262, backPanelImage);

            InitButton(ref _style4SSJ4Button, GFX.style4SSJ4Preview, new MouseEvent((evt, element) => SelectSSJ4Hair(4, evt, element)), 312, 264, backPanelImage);

            InitText(ref _gogetaText, "Style 5: Broly", 0.56f, 12, 306, Color.White, backPanelImage);

            InitButton(ref _style5BaseButton, GFX.style5BasePreview, new MouseEvent((evt, element) => SelectBaseHair(5, evt, element)), 36, 328, backPanelImage);

            InitButton(ref _style5SSJButton, GFX.style5SSJPreview, new MouseEvent((evt, element) => SelectSSJHair(5, evt, element)), 102, 328, backPanelImage);

            InitButton(ref _style5SSJ2Button, GFX.style5SSJ2Preview, new MouseEvent((evt, element) => SelectSSJ2Hair(5, evt, element)), 172, 328, backPanelImage);

            InitButton(ref _style5SSJ3Button, GFX.style5SSJ3Preview, new MouseEvent((evt, element) => SelectSSJ3Hair(5, evt, element)), 242, 324, backPanelImage);

            InitButton(ref _style5SSJ4Button, GFX.style5SSJ4Preview, new MouseEvent((evt, element) => SelectSSJ4Hair(5, evt, element)), 312, 326, backPanelImage);

            /*InitButton(ref _wishButtonWealth, Gfx.wishforWealth, new MouseEvent(SelectButtonWealth), 55, 22, backPanelImage);

            InitButton(ref _wishButtonImmortality, Gfx.wishforImmortality, new MouseEvent(SelectButtonImmortality), 100, 22, backPanelImage);

            InitButton(ref _wishButtonGenetics, Gfx.wishforGenetics, new MouseEvent(SelectButtonGenetics), 145, 22, backPanelImage);

            InitButton(ref _wishButtonSkill, Gfx.wishforSkill, new MouseEvent(SelectButtonSkill), 190, 22, backPanelImage);

            InitButton(ref _wishButtonAwakening, Gfx.wishforAwakening, new MouseEvent(SelectButtonAwakening), 235, 22, backPanelImage);*/

            InitButton(ref _confirmButton, GFX.hairConfirmButton, new MouseEvent(ConfirmHair), GFX.hairBackPanel.Width - GFX.hairConfirmButton.Width - 12, GFX.hairBackPanel.Height - GFX.hairConfirmButton.Height - 7, backPanelImage);

            InitHoverTextButton(ref _keepHairButton, GFX.keepHairButton, new MouseEvent((evt, element) => SelectBaseHair(0, evt, element)), 12, 372, backPanelImage, "Use the hair you chose at character creation for your base form hairstyle instead of the above ones.");


            backPanel.Append(backPanelImage);

            Append(backPanel);
        }

        private void ConfirmHair(UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (baseChecked && ssjChecked && ssj2Checked && ssj3Checked && ssj4Checked)
            {
                SoundHelper.PlayVanillaSound(SoundID.MenuTick);
                menuVisible = false;
                player.hairChecked = true;
            }
            else
            {
                SoundHelper.PlayVanillaSound(SoundID.MenuClose);
            }
        }

        // TODO Change this to dynamicity.
        private void SelectBaseHair(int Choice, UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            player.ssjHairStyles[HairAppearance.BASE_HAIRSTYLE_KEY] = Choice;
            SoundHelper.PlayVanillaSound(SoundID.MenuTick);
            baseChecked = true;
        }

        private void SelectSSJHair(int Choice, UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            player.ssjHairStyles[HairAppearance.SSJ1_HAIRSTYLE_KEY] = Choice;
            SoundHelper.PlayVanillaSound(SoundID.MenuTick);
            ssjChecked = true;
        }

        private void SelectSSJ2Hair(int Choice, UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            player.ssjHairStyles[HairAppearance.SSJ2_HAIRSTYLE_KEY] = Choice;
            SoundHelper.PlayVanillaSound(SoundID.MenuTick);
            ssj2Checked = true;
        }
        private void SelectSSJ3Hair(int Choice, UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            player.ssjHairStyles[HairAppearance.SSJ3_HAIRSTYLE_KEY] = Choice;
            SoundHelper.PlayVanillaSound(SoundID.MenuTick);
            ssj3Checked = true;
        }
        private void SelectSSJ4Hair(int Choice, UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            player.ssjHairStyles[HairAppearance.SSJ4_HAIRSTYLE_KEY] = Choice;
            SoundHelper.PlayVanillaSound(SoundID.MenuTick);
            ssj4Checked = true;
        }
    }
}