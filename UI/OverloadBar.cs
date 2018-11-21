using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace DBZMOD.UI
{
	internal class OverloadBar : UIState
	{
        public ResourceBar Overloadbar;
		public static bool visible = false;

		public override void OnInitialize()
		{
			Overloadbar = new ResourceBar(ResourceBarMode.OVERLOAD, 9, 64);
			Overloadbar.Left.Set(725f, 0f);
			Overloadbar.Top.Set(46f, 0f);
            Overloadbar.OnMouseDown += new UIElement.MouseEvent(DragStart);
			Overloadbar.OnMouseUp += new UIElement.MouseEvent(DragEnd);
			Append(Overloadbar);
		}
		Vector2 offset;
        public bool dragging = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            offset = new Vector2(evt.MousePosition.X - Overloadbar.Left.Pixels, evt.MousePosition.Y - Overloadbar.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            Overloadbar.Left.Set(end.X - offset.X, 0f);
            Overloadbar.Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (Overloadbar.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                Overloadbar.Left.Set(MousePosition.X - offset.X, 0f);
                Overloadbar.Top.Set(MousePosition.Y - offset.Y, 0f);
                Recalculate();
            }
        }
	}
}