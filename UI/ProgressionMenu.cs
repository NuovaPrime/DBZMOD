using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace DBZMOD.UI
{
    internal class ProgressionMenu : EasyMenu
    {
        public static bool menuvisible = false;

        private UIText _titleText;
        public UIImage backPanelImage;

        private UIText _kiExperienceText;

        private Player _player;
        public const float PADDINGX = 10f;
        public const float PADDINGY = 30f;

        public override void OnInitialize()
        {
            backPanel = new UIPanel();
            backPanel.Width.Set(306f, 0f);
            backPanel.Height.Set(128f, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 2f - backPanel.Height.Pixels / 2f, 0f);
            backPanel.BackgroundColor = new Color(0, 0, 0, 0);
            backPanel.OnMouseDown += new MouseEvent(DragStart);
            backPanel.OnMouseUp += new MouseEvent(DragEnd);
            Append(backPanel);

            backPanelImage = new UIImage(GFX.backPanel);
            backPanelImage.Width.Set(GFX.backPanel.Width, 0f);
            backPanelImage.Height.Set(GFX.backPanel.Height, 0f);
            backPanelImage.Left.Set(-12, 0f);
            backPanelImage.Top.Set(-12, 0f);
            backPanel.Append(backPanelImage);
            float row1OffsetX = 0.0f;

            InitText(ref _titleText, "Progression Menu", 1, 55, -32, Color.White);

            InitText(ref _kiExperienceText, "Ki Experience: ???", 1, 0, 0, Color.Cyan);

            base.OnInitialize();
        }

        public static void ToggleVisibility()
        {
            menuvisible = !menuvisible;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MyPlayer modplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            if(modplayer != null)
            {
                if(modplayer.GetProgressionSystem() != null)
                {
                    _kiExperienceText.SetText("Ki Experience: " + modplayer.GetProgressionSystem().GetKiExperience());
                }
            }

        }


    }
}