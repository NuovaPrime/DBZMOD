using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using DBZMOD.Enums;
using System;
using System.Collections.Generic;
using DBZMOD.Transformations;
using DBZMOD.Utilities;

namespace DBZMOD.UI
{
    internal class TransformationMenu : EasyMenu
    {
        public static bool menuVisible = false;

        public UIImage backPanelImage;
        private UIText _titleText;

        private readonly Dictionary<TransformationDefinition, UIImagePair> _imagePairs = new Dictionary<TransformationDefinition, UIImagePair>();

        public static TransformationDefinition SelectedTransformation { get; set; }

        public const float PADDINGX = 30f;
        public const float PADDINGY = PADDINGX;

        public override void OnInitialize()
        {
            base.OnInitialize();

            // TODO : Fix panel not being drageable all over its surface.

            backPanel = new UIPanel();
            backPanel.Width.Set(Gfx.backPanel.Width, 0f);
            backPanel.Height.Set(Gfx.backPanel.Height, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 2f - backPanel.Height.Pixels / 2f, 0f);
            backPanel.BackgroundColor = new Color(0, 0, 0, 0);
            backPanel.OnMouseDown += new MouseEvent(DragStart);
            backPanel.OnMouseUp += new MouseEvent(DragEnd);
            Append(backPanel);

            backPanelImage = new UIImage(Gfx.backPanel);
            backPanelImage.Width.Set(Gfx.backPanel.Width, 0f);
            backPanelImage.Height.Set(Gfx.backPanel.Height, 0f);
            backPanelImage.Left.Set(-12, 0f);
            backPanelImage.Top.Set(-12, 0f);
            backPanel.Append(backPanelImage);
            float row1OffsetX = 0.0f;

            // 125 is the width of the text ?
            InitText(ref _titleText, "Transformation Tree", 1, Gfx.backPanel.Bounds.X, -32, Color.White);

            TransformationDefinitionManager tDefMan = DBZMOD.Instance.TransformationDefinitionManager;

            row1OffsetX = PADDINGX;

            int j = 0;
            for (int i = 0; i < tDefMan.Count; i++)
            {
                TransformationDefinition def = tDefMan[i];
                if (def.BuffIcon == null) continue;

                UIImageButton transformationButton = null;
                UIImage lockedImage = null, unknownImage = null;

                InitButton(ref transformationButton, def.BuffIcon, new MouseEvent((evt, element) => TrySelectingTransformation(def, evt, element)), 
                    row1OffsetX, PADDINGY, backPanelImage);
                
                InitImage(ref lockedImage, Gfx.lockedImage, 0, 0, transformationButton);
                lockedImage.ImageScale = 0f;

                InitImage(ref unknownImage, Gfx.unknownImage, 0, 0, transformationButton);
                unknownImage.ImageScale = 0f;

                _imagePairs.Add(def, new UIImagePair(transformationButton, lockedImage, unknownImage));
                row1OffsetX += def.BuffIcon.Width + 15;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            // TODO Make this use Dynamicity

            TransformationDefinitionManager tDefMan = DBZMOD.Instance.TransformationDefinitionManager;
            for (int i = 0; i < tDefMan.Count; i++)
            {
                TransformationDefinition def = tDefMan[i];

                if (def.BuffIcon == null) continue;
                _imagePairs[def].lockedImage.ImageScale = player.PlayerTransformations.ContainsKey(def) ? 0f : 1f;
            }
        }

        private void TrySelectingTransformation(TransformationDefinition def, UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            if (player.PlayerTransformations.ContainsKey(def))
            {
                SoundHelper.PlayVanillaSound(SoundID.MenuTick);

                if (SelectedTransformation != def)
                {
                    SelectedTransformation = def;
                    Main.NewText($"Selected {def.TransformationText}, Mastery: {Math.Round(100f * def.GetPlayerMastery(player), 2)}%");
                }
                else
                    Main.NewText($"{def.TransformationText} Mastery: {Math.Round(100f * def.GetPlayerMastery(player), 2)}%");
            }
            else if (def.SelectionRequirementsFailed.Invoke(player, def))
            {
                SoundHelper.PlayVanillaSound(SoundID.MenuClose);

                if (def.TransformationFailureText == null) return;
                Main.NewText(def.TransformationFailureText);
            }
        }
    }

    struct UIImagePair
    {
        public UIImageButton button;
        public UIImage lockedImage, unknownImage;

        public UIImagePair(UIImageButton button, UIImage lockedImage, UIImage unknownImage)
        {
            this.button = button;
            this.lockedImage = lockedImage;
            this.unknownImage = unknownImage;
        }
    }
}