using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace DBZMOD.UI
{
    internal class EasyMenu : UIState
    {
        public UIPanel backPanel;

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.

            // Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

        public void InitButton(ref UIImageButton buttonToInitialise, Texture2D buttonTexture, MouseEvent buttonOnClick, float offsetX = 0, float offsetY = 0, UIElement parentElement = null)
        {
            buttonToInitialise = new UIImageButton(buttonTexture);

            buttonToInitialise.Width.Set(buttonTexture.Width, 0.0f);
            buttonToInitialise.Height.Set(buttonTexture.Height, 0.0f);
            buttonToInitialise.Left.Set(offsetX, 0f);
            buttonToInitialise.Top.Set(offsetY, 0f);
            buttonToInitialise.OnClick += buttonOnClick;

            if (parentElement == null)
            {
                backPanel.Append(buttonToInitialise);
            }
            else
            {
                parentElement.Append(buttonToInitialise);
            }
        }

        public void InitHoverTextButton(ref UIHoverImageButton buttonToInitialise, Texture2D buttonTexture, MouseEvent buttonOnClick, float offsetX = 0, float offsetY = 0, UIElement parentElement = null, string textOnHover = null)
        {
            buttonToInitialise = new UIHoverImageButton(buttonTexture, textOnHover);

            buttonToInitialise.Width.Set(buttonTexture.Width, 0.0f);
            buttonToInitialise.Height.Set(buttonTexture.Height, 0.0f);
            buttonToInitialise.Left.Set(offsetX, 0f);
            buttonToInitialise.Top.Set(offsetY, 0f);
            buttonToInitialise.OnClick += buttonOnClick;

            if (parentElement == null)
            {
                backPanel.Append(buttonToInitialise);
            }
            else
            {
                parentElement.Append(buttonToInitialise);
            }
        }

        public void InitImage(ref UIImage imageToInitialise, Texture2D imageTexture, float offsetX = 0, float offsetY = 0, UIElement parentElement = null)
        {
            imageToInitialise = new UIImage(imageTexture);

            imageToInitialise.Width.Set(imageTexture.Width, 0.0f);
            imageToInitialise.Height.Set(imageTexture.Height, 0.0f);
            imageToInitialise.Left.Set(offsetX, 0f);
            imageToInitialise.Top.Set(offsetY, 0f);

            if (parentElement == null)
            {
                backPanel.Append(imageToInitialise);
            }
            else
            {
                parentElement.Append(imageToInitialise);
            }
        }

        public void InitText(ref UIText textToInitialise, string text, float scale = 1, float offsetX = 0, float offsetY = 0, Color textColour = default(Color), UIElement parentElement = null)
        {
            textToInitialise = new UIText(text, scale);

            textToInitialise.Width.Set(16f, 0f);
            textToInitialise.Height.Set(16f, 0f);
            textToInitialise.Left.Set(offsetX, 0f);
            textToInitialise.Top.Set(offsetY, 0f);
            textToInitialise.TextColor = textColour;

            if (parentElement == null)
            {
                backPanel.Append(textToInitialise);
            }
            /*if (parentElement == wishbackPanel)
            {
                wishbackPanel.Append(TextToInitialise);
            }*/
            else
            {
                parentElement.Append(textToInitialise);
            }
        }

        Vector2 _offset;
        public bool dragging = false;

        protected void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            _offset = new Vector2(evt.MousePosition.X - backPanel.Left.Pixels, evt.MousePosition.Y - backPanel.Top.Pixels);
            dragging = true;
        }

        protected void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            backPanel.Left.Set(end.X - _offset.X, 0f);
            backPanel.Top.Set(end.Y - _offset.Y, 0f);

            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Vector2 mousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);

            if (backPanel.ContainsPoint(mousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                backPanel.Left.Set(mousePosition.X - _offset.X, 0f);
                backPanel.Top.Set(mousePosition.Y - _offset.Y, 0f);
                Recalculate();
            }
        }
    }
}