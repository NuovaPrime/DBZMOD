using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using DBZMOD.Enums;
using System;
using System.Collections.Generic;
using DBZMOD.Dynamicity;
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

        public const float
            PADDINGX = 30f,
            PADDINGY = PADDINGX,
            SMALL_SPACE = 15f;

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

            float
                rowXOffset = PADDINGX,
                rowYOffset = PADDINGY;

            // 125 is the width of the text ?
            InitText(ref _titleText, "Transformation Tree", 1, Gfx.backPanel.Bounds.X, -32, Color.White);

            NodeTree<TransformationDefinition> tDefTree = DBZMOD.Instance.TransformationDefinitionManager.Tree;

            foreach (KeyValuePair<TransformationDefinition, ManyToManyNode<TransformationDefinition>> rootedTree in tDefTree.RootedTree)
            {
                // A root element of the transformation tree should always have an BuffIcon.
                if (rootedTree.Key.BuffIcon == null) continue;

                RecursiveDrawTransformation(tDefTree.Tree, rootedTree.Value, ref rowXOffset, ref rowYOffset);

                rowXOffset = PADDINGX;
                rowYOffset += rootedTree.Key.BuffIcon.Height + SMALL_SPACE;
            }
        }

        private void RecursiveDrawTransformation(Dictionary<TransformationDefinition, ManyToManyNode<TransformationDefinition>> tree, ManyToManyNode<TransformationDefinition> mtmn, ref float xOffset, ref float yOffset)
        {
            if (mtmn.Current.BuffIcon == null) return;

            UIImageButton transformationButton = null;
            UIImage lockedImage = null, unknownImage = null;

            InitButton(ref transformationButton, mtmn.Current.BuffIcon, new MouseEvent((evt, element) => TrySelectingTransformation(mtmn.Current, evt, element)),
                xOffset, yOffset, backPanelImage);

            InitImage(ref unknownImage, Gfx.unknownImage, 0, 0, transformationButton);
            unknownImage.ImageScale = 0f;

            InitImage(ref lockedImage, Gfx.lockedImage, 0, 0, unknownImage);
            lockedImage.ImageScale = 0f;

            _imagePairs.Add(mtmn.Current, new UIImagePair(transformationButton, lockedImage, unknownImage));

            for (int i = 0; i < mtmn.Next.Count; i++)
            {
                TransformationDefinition nextDef = mtmn.Next[i];
                if (nextDef.BuffIcon == null) continue;

                RecursiveDrawTransformation(tree, tree[nextDef], ref xOffset, ref yOffset);

                if (i + 1 < mtmn.Next.Count)
                    yOffset += mtmn.Current.BuffIcon.Height + SMALL_SPACE;
            }

            xOffset += mtmn.Current.BuffIcon.Width + SMALL_SPACE;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();

            foreach (KeyValuePair<TransformationDefinition, UIImagePair> kvp in _imagePairs)
            {
                bool unlockable = kvp.Key.CanPlayerUnlock(player);

                //kvp.Value.lockedImage.ImageScale = unlockable ? 0f : 1f;
                //kvp.Value.unknownImage.ImageScale = unlockable && player.HasTransformation(kvp.Key) ? 0f : 1f;
            }
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
        public UIImage lockedImage, unknownImage;

        public UIImagePair(UIImageButton button, UIImage lockedImage, UIImage unknownImage)
        {
            this.button = button;
            this.lockedImage = lockedImage;
            this.unknownImage = unknownImage;
        }
    }
}