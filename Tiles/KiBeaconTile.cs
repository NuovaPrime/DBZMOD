using DBZMOD.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DBZMOD.Tiles
{
    public class KiBeaconTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = false;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.CoordinatePadding = 2;            
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Ki Beacon");            
            AddMapEntry(new Color(223, 245, 255), name);
            dustType = mod.DustType("MetalDust");
            animationFrameHeight = 18;
            disableSmartCursor = true;
            TileObjectData.addTile(Type);
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 10)
            {
                frame++;
                frameCounter = 0;
            }
            if (frame > 3)
            {
                frame = 0;
            }
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("KiBeaconItem"));
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture;
            if (Main.canDrawColorTile(i, j))
            {
                texture = Main.tileAltTexture[Type, (int)tile.color()];
            }
            else
            {
                texture = Main.tileTexture[Type];
            }
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = 18;
            int animate = Main.tileFrame[Type] * animationFrameHeight;
            
            Main.spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY + animate, 16, height), Lighting.GetColor(i, j), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(mod.GetTexture("Tiles/KiBeaconTileGlowmask"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY + animate, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            if (tile.frameX == 36)
            {
                
                Texture2D lightTexture = mod.GetTexture("Tiles/KiBeaconTileLight");
                Texture2D lightTexture2 = mod.GetTexture("Tiles/KiBeaconTileLight2");
                Vector2 spritePosition = new Vector2(i * 16f - Main.screenPosition.X - 8, (j * 16f - Main.screenPosition.Y) - 14) + zero;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.Default, Main.instance.Rasterizer);
                int shiftAlpha = (int)(DBZMOD.GetTicks() % 65);
                if (shiftAlpha >= 32)
                {
                    shiftAlpha = 32 - (shiftAlpha - 32);
                }
                shiftAlpha = shiftAlpha * 3 + 64;
                Color transparencyColor = new Color(255, 255, 255, shiftAlpha);
                spriteBatch.Draw(lightTexture, spritePosition, new Rectangle(0, 0, lightTexture.Width, lightTexture.Height), transparencyColor, 0f, lightTexture.Size() * 0.5f, 2f, SpriteEffects.None, 0f);
                spriteBatch.Draw(lightTexture2, spritePosition + new Vector2(0, -11), new Rectangle(0, 0, lightTexture2.Width, lightTexture2.Height), transparencyColor, 0f, lightTexture2.Size() * 0.5f, 2f, SpriteEffects.None, 0f);
                spriteBatch.End();
                spriteBatch.Begin();

            }
            return false;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            var kiBeaconLocation = new Vector2(i * 16f, j * 16f);
            DBZWorld.GetWorld().kiBeacons.Add(kiBeaconLocation);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetworkHelper.playerSync.SendKiBeaconAdd(256, Main.myPlayer, kiBeaconLocation);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            var kiBeaconLocation = new Vector2(i * 16f, j * 16f);
            DBZWorld.GetWorld().kiBeacons.Remove(kiBeaconLocation);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetworkHelper.playerSync.SendKiBeaconRemove(256, Main.myPlayer, kiBeaconLocation);
        }
    }
}