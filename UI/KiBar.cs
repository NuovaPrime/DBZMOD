using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace DBZMOD.UI
{
	internal class KiBar : UIState
	{
        public ResourceBar kibar;
		public static bool visible = false;

		public override void OnInitialize()
		{
			kibar = new ResourceBar(ResourceBarMode.Ki, 24, 128);
			kibar.Left.Set(515f, 0f);
			kibar.Top.Set(20f, 0f);
            kibar.OnMouseDown += new UIElement.MouseEvent(DragStart);
			kibar.OnMouseUp += new UIElement.MouseEvent(DragEnd);
			Append(kibar);
		}

		Vector2 _offset;
        public bool dragging = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            _offset = new Vector2(evt.MousePosition.X - kibar.Left.Pixels, evt.MousePosition.Y - kibar.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            kibar.Left.Set(end.X - _offset.X, 0f);
            kibar.Top.Set(end.Y - _offset.Y, 0f);

            Recalculate();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 mousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (kibar.ContainsPoint(mousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                kibar.Left.Set(mousePosition.X - _offset.X, 0f);
                kibar.Top.Set(mousePosition.Y - _offset.Y, 0f);
                Recalculate();
            }
        }
	}
}