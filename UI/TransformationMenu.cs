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

        public static TransformationDefinition SelectedTransformation { get; set; }

        public const int
            PADDINGX = 30,
            PADDINGY = PADDINGX,
            SMALL_SPACE = 15;

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
                if (rootedTree.Key.BuffIcon == null) continue;

                RecursiveDrawTransformation(tDefTree.Tree, rootedTree.Value, ref rowXOffset, ref rowYOffset);

                rowXOffset = PADDINGX;
                //rowYOffset += rootedTree.Key.BuffIcon.Height + SMALL_SPACE;
            }
        }

        private void RecursiveDrawTransformation(Dictionary<TransformationDefinition, ManyToManyNode<TransformationDefinition>> tree, ManyToManyNode<TransformationDefinition> mtmn, ref int xOffset, ref int yOffset)
        {
            if (mtmn.Current.BuffIcon == null) return;

            UIImageButton transformationButton = null;
            UIImage unknownImage = null, unknownImageGray = null, lockedImage = null;

            InitButton(ref transformationButton, mtmn.Current.BuffIcon, new MouseEvent((evt, element) => TrySelectingTransformation(mtmn.Current, evt, element)),
                xOffset, yOffset, backPanelImage);

            InitImage(ref unknownImage, GFX.unknownImage, 0, 0, transformationButton);
            unknownImage.ImageScale = 0f;

            InitImage(ref unknownImageGray, GFX.unknownImageGray, 0, 0, unknownImage);
            unknownImage.ImageScale = 0f;

            InitImage(ref lockedImage, GFX.lockedImage, 0, 0, unknownImageGray);
            lockedImage.ImageScale = 0f;

            _imagePairs.Add(mtmn.Current, new UIImagePair(transformationButton, unknownImage, unknownImageGray, lockedImage));
            _imagePositions.Add(mtmn.Current, new Point(xOffset, yOffset));
            xOffset += mtmn.Current.BuffIcon.Width + SMALL_SPACE;

            int localXOffset = xOffset;

            for (int i = 0; i < mtmn.Next.Count; i++)
            {
                TransformationDefinition nextDef = mtmn.Next[i];
                if (nextDef.BuffIcon == null) continue;

                List<Vector2> points = new List<Vector2>();
                points.Add(_imagePositions[mtmn.Current].ToVector2());

                for (int j = 0; j < mtmn.Previous.Count; j++)
                {
                    if (mtmn.Previous[j].BuffIcon == null) continue;

                    points.Add(_imagePositions[mtmn.Previous[j]].ToVector2());
                }

                _polyLinesToDraw.Add(points.ToArray());

                RecursiveDrawTransformation(tree, tree[nextDef], ref xOffset, ref yOffset);

                if (i + 1 < mtmn.Next.Count)
                    yOffset += mtmn.Current.BuffIcon.Height + SMALL_SPACE;

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

                kvp.Value.unknownImage.ImageScale = unlockable ? 0f : 1f;
                kvp.Value.unknownImageGray.ImageScale = unlockable && player.HasTransformation(kvp.Key) ? 0f : 1f;
                kvp.Value.lockedImage.ImageScale = unlockable ? 0f : 1f;
            }

            for (int i = 0; i < _polyLinesToDraw.Count; i++)
                if (_polyLinesToDraw[i].Length > 1)
                Main.spriteBatch.DrawPolyLine(_polyLinesToDraw[i], Color.White);
        }

        private static void TrySelectingTransformation(TransformationDefinition def, UIMouseEvent evt, UIElement listeningElement)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            if (player.PlayerTransformations.ContainsKey(def) && def.MeetsSelectionRequirements(player))
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