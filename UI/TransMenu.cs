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
    internal class TransMenu : UIState
    {
        private Player player;
        public UIPanel _backPanel;
        private UIText ssjbuttontext;
        public static bool menuvisible = false;
        private UIImageButton ssjButtonTexture;
        private const float padding = 5f;

        public override void OnInitialize()
        {
            _backPanel = new UIPanel();
            _backPanel.Width.Set(135f, 0f);
            _backPanel.Height.Set(70f, 0f);
            _backPanel.Left.Set(Main.screenWidth / 2f - _backPanel.Width.Pixels / 2f, 0f);
            _backPanel.Top.Set(Main.screenHeight / 2f - _backPanel.Height.Pixels / 2f, 0f);
            _backPanel.BackgroundColor = new Color(73, 94, 171);
            base.Append(_backPanel);
            base.OnInitialize();
            
            var ssjbuttontexture = DBZMOD.instance.GetTexture("Buffs/SSJ1Buff");
            ssjButtonTexture = new UIImageButton(ssjbuttontexture);
            ssjButtonTexture.Width.Set(ssjbuttontexture.Width, 0f);
            ssjButtonTexture.Height.Set(ssjbuttontexture.Height, 0f);
            ssjButtonTexture.Left.Set(padding, 0f);
            ssjButtonTexture.Top.Set(padding, 0f);
            ssjButtonTexture.OnClick += TrySelectingSSJ1;
            _backPanel.Append(ssjButtonTexture);
            
            ssjbuttontext = new UIText("Super Saiyan");
            ssjbuttontext.Width.Set(ssjbuttontexture.Width, 0f);
            ssjbuttontext.Height.Set(ssjbuttontexture.Height, 0f);
            ssjbuttontext.Left.Set(padding, 0f);
            ssjbuttontext.Top.Set(padding, 0f);
            ssjbuttontext.Append(ssjButtonTexture);
        }

        private void TrySelectingSSJ1(UIMouseEvent evt, UIElement listeningelement)
        {
            MyPlayer.ModPlayer(player).MenuSelection = 1;
            Main.PlaySound(SoundID.MenuTick);
        }
    }
}