using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using DBZMOD.Enums;
using System;
using System.Collections.Generic;
using DBZMOD.Dynamicity;
using DBZMOD.Extensions;
using DBZMOD.Transformations;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework.Graphics;

namespace DBZMOD.UI
{
    internal class TransformationMenu : EasyMenu
    {
        public static bool menuVisible = false;

        public UIImage backPanelImage;
        private UIText _titleText;

        private readonly Dictionary<TransformationDefinition, UIImagePair> _imagePairs = new Dictionary<TransformationDefinition, UIImagePair>();
        private readonly Dictionary<TransformationDefinition, Point> _imagePositions = new Dictionary<TransformationDefinition, Point>();
        private readonly List<Vector2[]> _polyLinesToDraw = new List<Vector2[]>();

        public const int
            PADDINGX = 30,
            PADDINGY = PADDINGX,
            SMALL_SPACE = 4;

        public override void OnInitialize()
        {
            base.OnInitialize();

            // TODO : Fix panel not being drageable all over its surface.

            backPanel = new UIPanel();
            backPanel.Width.Set(GFX.backPanel.Width, 0f);
            backPanel.Height.Set(GFX.backPanel.Height, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 2f - backPanel.Height.Pixels / 2f, 0f);
            backPanel.BackgroundColor = new Color(0, 0, 0, 0);
            backPanel.OnMouseDown += new MouseEvent(DragStart);
            backPanel.OnMouseUp += new MouseEvent(DragEnd);
            Append(backPanel);

            backPanelImage = new UIImage(GFX.backPanel);
            backPanelImage.Width.Set(GFX.backPanel.Width, 0f);
            backPanelImage.Height.Set(GFX.backPanel.Height, 0f);
            backPanelImage.Left.Set(-12, 0f);
            backPanelImage.Top.Set(-12, 0f);
            backPanel.Append(backPanelImage);

            int
                rowXOffset = PADDINGX,
                rowYOffset = PADDINGY;

            // 125 is the width of the text ?
            InitText(ref _titleText, "Transformation Tree", 1, GFX.backPanel.Bounds.X, -32, Color.White);

            NodeTree<TransformationDefinition> tDefTree = DBZMOD.Instance.TransformationDefinitionManager.Tree;

            foreach (KeyValuePair<TransformationDefinition, ManyToManyNode<TransformationDefinition>> rootedTree in tDefTree.RootedTree)
            {
                // A root element of the transformation tree should always have an BuffIcon.
                if (rootedTree.Key.BuffIconGetter == null)
                    continue;

                RecursiveDrawTransformation(tDefTree.Tree, rootedTree.Value, ref rowXOffset, ref rowYOffset);

                rowXOffset = PADDINGX;
                rowYOffset += rootedTree.Key.BuffIconGetter().Height + SMALL_SPACE;
            }
        }

        private void RecursiveDrawTransformation(Dictionary<TransformationDefinition, ManyToManyNode<TransformationDefinition>> tree, ManyToManyNode<TransformationDefinition> mtmn, ref int xOffset, ref int yOffset)
        {
            if (!mtmn.Current.HasMenuIcon || !mtmn.Current.CheckPrePlayerConditions()) return;
            Texture2D buffIcon = mtmn.Current.BuffIconGetter.Invoke();

            if (buffIcon == null) return;

            UIImageButton transformationButton = null;
            UIImage unknownImage = null, unknownImageGray = null, lockedImage = null;

            InitButton(ref transformationButton, buffIcon, new MouseEvent((evt, element) => TrySelectingTransformation(mtmn.Current, evt, element)),
                xOffset, yOffset, backPanelImage);

            InitImage(ref unknownImage, GFX.unknownImage, 0, 0, transformationButton);
            unknownImage.ImageScale = 0f;

            InitImage(ref unknownImageGray, GFX.unknownImageGray, 0, 0, unknownImage);
            unknownImage.ImageScale = 0f;

            InitImage(ref lockedImage, GFX.lockedImage, 0, 0, unknownImageGray);
            lockedImage.ImageScale = 0f;

            _imagePairs.Add(mtmn.Current, new UIImagePair(transformationButton, unknownImage, unknownImageGray, lockedImage));
            _imagePositions.Add(mtmn.Current, new Point(xOffset, yOffset));
            xOffset += buffIcon.Width + SMALL_SPACE;

            int localXOffset = xOffset;

            for (int i = 0; i < mtmn.Next.Count; i++)
            {
                TransformationDefinition nextDef = mtmn.Next[i];
                if (nextDef.BuffIconGetter == null) continue;

                List<Vector2> points = new List<Vector2>();
                points.Add(_imagePositions[mtmn.Current].ToVector2());

                for (int j = 0; j < mtmn.Previous.Count; j++)
                {
                    if (mtmn.Previous[j].BuffIconGetter == null) continue;

                    points.Add(_imagePositions[mtmn.Previous[j]].ToVector2());
                }

                _polyLinesToDraw.Add(points.ToArray());

                RecursiveDrawTransformation(tree, tree[nextDef], ref xOffset, ref yOffset);

                if (i + 1 < mtmn.Next.Count)
                    yOffset += buffIcon.Height + SMALL_SPACE * 2;

                xOffset = localXOffset;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            foreach (KeyValuePair<TransformationDefinition, UIImagePair> kvp in _imagePairs)
            {
                bool unlockable = kvp.Key.CanPlayerUnlock(player);
                bool visible = kvp.Key.DoesShowInMenu(player);

                if (!visible)
                {
                    kvp.Value.button.Width = StyleDimension.Empty;
                    kvp.Value.button.Height = StyleDimension.Empty;
                    kvp.Value.button.SetVisibility(0f, 0f);
                }

                kvp.Value.unknownImage.ImageScale = visible && unlockable ? 0f : 1f;
                kvp.Value.unknownImageGray.ImageScale = visible && unlockable && player.HasTransformation(kvp.Key) ? 0f : 1f;
                kvp.Value.lockedImage.ImageScale = visible && unlockable ? 0f : 1f;
            }

            // Disabled as it crashes with SpriteBatch.
            /*for (int i = 0; i < _polyLinesToDraw.Count; i++)
                if (_polyLinesToDraw[i].Length > 1)
                    Main.spriteBatch.DrawPolyLine(_polyLinesToDraw[i], Color.White);*/
        }

        private static void TrySelectingTransformation(TransformationDefinition def, UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            if (def.DoesShowInMenu(player) && player.PlayerTransformations.ContainsKey(def) && def.MeetsSelectionRequirements(player))
            {
                SoundHelper.PlayVanillaSound(SoundID.MenuTick);

                if (player.SelectedTransformation != def)
                {
                    player.SelectedTransformation = def;
                    Main.NewText($"Selected {def.Text}, Mastery: {Math.Round(100f * def.GetPlayerMastery(player), 2)}%");
                }
                else
                    Main.NewText($"{def.Text} Mastery: {Math.Round(100f * def.GetPlayerMastery(player), 2)}%");
            }
            else if (def.SelectionRequirementsFailed.Invoke(player, def))
            {
                SoundHelper.PlayVanillaSound(SoundID.MenuClose);

                if (def.FailureText == null) return;
                Main.NewText(def.FailureText);
            }
        }
    }

    struct UIImagePair
    {
        public UIImageButton button;
        public UIImage unknownImage, unknownImageGray, lockedImage;

        public UIImagePair(UIImageButton button, UIImage unknownImage, UIImage unknownImageGray, UIImage lockedImage)
        {
            this.button = button;
            this.unknownImage = unknownImage;
            this.unknownImageGray = unknownImageGray;
            this.lockedImage = lockedImage;
        }
    }
}