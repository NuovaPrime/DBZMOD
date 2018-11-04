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

namespace DBZMOD.UI
{
    internal class ProgressionMenu : EasyMenu
    {
        public static bool menuvisible = false;

        private UIText titleText;
        public UIImage backPanelImage;

        private UIText kiExperienceText;

        private Player player;
        public const float PADDINGX = 10f;
        public const float PADDINGY = 30f;

        public override void OnInitialize()
        {
            base.OnInitialize();
            //backPanel.BackgroundColor = new Color(100, 1W00, 100);

            backPanelImage = new UIImage(GFX.BackPanel);
            backPanelImage.Width.Set(GFX.BackPanel.Width, 0f);
            backPanelImage.Height.Set(GFX.BackPanel.Height, 0f);
            backPanelImage.Left.Set(-12, 0f);
            backPanelImage.Top.Set(-12, 0f);
            backPanel.Append(backPanelImage);
            float Row1_OffsetX = 0.0f;

            InitText(ref titleText, "Progression Menu", 55, -32, Color.White);

            InitText(ref kiExperienceText, "Ki Experience: ???", 0, 0, Color.Cyan);
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
                    kiExperienceText.SetText("Ki Experience: " + modplayer.GetProgressionSystem().GetKiExperience());
                }
            }

        }


    }
}