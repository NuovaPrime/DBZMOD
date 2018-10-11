using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace DBZMOD.UI
{
	internal class KiBar : UIState
	{
        public ResourceBar Kibar;
		public static bool visible = false;

		public override void OnInitialize()
		{
			Kibar = new ResourceBar(ResourceBarMode.KI, 7, 92);
			Kibar.Left.Set(515f, 0f);
			Kibar.Top.Set(49f, 0f);
            Kibar.OnMouseDown += new UIElement.MouseEvent(DragStart);
			Kibar.OnMouseUp += new UIElement.MouseEvent(DragEnd);
			Append(Kibar);
		}
		Vector2 offset;
        public bool dragging = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            offset = new Vector2(evt.MousePosition.X - Kibar.Left.Pixels, evt.MousePosition.Y - Kibar.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            Kibar.Left.Set(end.X - offset.X, 0f);
            Kibar.Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (Kibar.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                Kibar.Left.Set(MousePosition.X - offset.X, 0f);
                Kibar.Top.Set(MousePosition.Y - offset.Y, 0f);
                Recalculate();
            }
        }
	}
}