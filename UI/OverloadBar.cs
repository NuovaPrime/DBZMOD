using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace DBZMOD.UI
{
	internal class OverloadBar : UIState
	{
        public ResourceBar overloadBar;
		public static bool visible = false;

		public override void OnInitialize()
		{
			overloadBar = new ResourceBar(ResourceBarMode.Overload, 24, 108);
			overloadBar.Left.Set(725f, 0f);
			overloadBar.Top.Set(22f, 0f);
            overloadBar.OnMouseDown += new UIElement.MouseEvent(DragStart);
			overloadBar.OnMouseUp += new UIElement.MouseEvent(DragEnd);
			Append(overloadBar);
		}

		Vector2 _offset;
        public bool dragging = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            _offset = new Vector2(evt.MousePosition.X - overloadBar.Left.Pixels, evt.MousePosition.Y - overloadBar.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            overloadBar.Left.Set(end.X - _offset.X, 0f);
            overloadBar.Top.Set(end.Y - _offset.Y, 0f);

            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 mousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (overloadBar.ContainsPoint(mousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                overloadBar.Left.Set(mousePosition.X - _offset.X, 0f);
                overloadBar.Top.Set(mousePosition.Y - _offset.Y, 0f);
                Recalculate();
            }
        }
	}
}