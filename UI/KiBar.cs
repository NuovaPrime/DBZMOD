using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace DBZMOD.UI
{
    internal class KiBar: UIState
    {
        public UIPanel Kibar;
        public UIImage ki;
        public static bool visible = false;

        public override void OnInitialize()
        {
            ResourceBar Bar = new ResourceBar(ResourceBarMode.KI, 6, 86);
            Bar.Left.Set(500f, 0f); //175
            Bar.Top.Set(40f, 0f);

            var BarTexture = GFX.KiBar;
            UIImage ki = new UIImage(BarTexture);
            ki.Left.Set(-20, 0f);
            ki.Top.Set(-8, 0f);
            ki.Width.Set(80, 0f);
            ki.Height.Set(18, 0f);
            Bar.Append(ki);
            //ki.OnMouseDown += new UIElement.MouseEvent(DragStart);
            //ki.OnMouseUp += new UIElement.MouseEvent(DragEnd);



            base.Append(Bar);
        }
        /*Vector2 offset;
        public bool dragging = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            offset = new Vector2(evt.MousePosition.X - ki.Left.Pixels, evt.MousePosition.Y - ki.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            ki.Left.Set(end.X - offset.X, 0f);
            ki.Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (ki.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                ki.Left.Set(MousePosition.X - offset.X, 0f);
                ki.Top.Set(MousePosition.Y - offset.Y, 0f);
                Recalculate();
            }
        }*/
    }
}