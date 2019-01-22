using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace DBZMOD.UI
{
	internal class OverloadBar : UIState
	{
        public ResourceBar overloadbar;
		public static bool visible = false;

		public override void OnInitialize()
		{
			overloadbar = new ResourceBar(ResourceBarMode.Overload, 9, 64);
			overloadbar.Left.Set(725f, 0f);
			overloadbar.Top.Set(46f, 0f);
            overloadbar.OnMouseDown += new UIElement.MouseEvent(DragStart);
			overloadbar.OnMouseUp += new UIElement.MouseEvent(DragEnd);
			Append(overloadbar);
		}
		Vector2 _offset;
        public bool dragging = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            _offset = new Vector2(evt.MousePosition.X - overloadbar.Left.Pixels, evt.MousePosition.Y - overloadbar.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            overloadbar.Left.Set(end.X - _offset.X, 0f);
            overloadbar.Top.Set(end.Y - _offset.Y, 0f);

            Recalculate();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 mousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (overloadbar.ContainsPoint(mousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                overloadbar.Left.Set(mousePosition.X - _offset.X, 0f);
                overloadbar.Top.Set(mousePosition.Y - _offset.Y, 0f);
                Recalculate();
            }
        }
	}
}