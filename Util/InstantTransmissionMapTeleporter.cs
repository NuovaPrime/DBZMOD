using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;

namespace DBZMOD.Util
{
    public class InstantTransmissionMapTeleporter
    {
        public static InstantTransmissionMapTeleporter instance;

        public InstantTransmissionMapTeleporter()
        {
            instance = this;
        }

        public void HandleDrawingKiBeacons()
        {
            Texture2D texture = DBZMOD.instance.GetTexture("UI/KiBeaconMap");

            // desperation starts here
            foreach (var beaconLocation in DBZWorld.GetWorld().kiBeacons)
            {
                // map coordinates represent the map tile being placed at *the center* of the screen.
                var mapX = Main.mapFullscreenPos.X;
                var mapY = Main.mapFullscreenPos.Y;

                // drawing, thus, is relative to the center, offset by the scaled distance between the target to render and the current map display "center".
                var mapOffsetX = Main.screenWidth / 2 + ((beaconLocation.X + 8f) - (mapX * 16f)) / (16 / Main.mapFullscreenScale);
                var mapOffsetY = Main.screenHeight / 2 + ((beaconLocation.Y - 8f) - (mapY * 16f)) / (16 / Main.mapFullscreenScale);
                Main.spriteBatch.Draw(texture, new Vector2(mapOffsetX, mapOffsetY), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
            }
        }

        public void PostDrawFullScreenMap()
        {
            HandleDrawingKiBeacons();
            var modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            var player = modPlayer.player;
            if (modPlayer.isInstantTransmission1Unlocked)
            {
                if (!modPlayer.HasKi(modPlayer.GetInstantTransmissionTeleportKiCost()))
                {
                    Main.spriteBatch.DrawString(Main.fontMouseText, "Your Ki is too low to use instant transmission.", new Vector2(15, Main.screenHeight - 120), Color.White);
                }
                else
                {
                    Main.spriteBatch.DrawString(Main.fontMouseText, "Press the Instant Transmission key on an NPC, beacon or player icon to teleport using Ki.", new Vector2(15, Main.screenHeight - 120), Color.White);

                    if (MyPlayer.instantTransmission.JustPressed)
                    {
                        int mapWidth = Main.maxTilesX * 16;
                        int mapHeight = Main.maxTilesY * 16;
                        Vector2 cursorPosition = new Vector2(Main.mouseX, Main.mouseY);

                        cursorPosition.X -= Main.screenWidth / 2;
                        cursorPosition.Y -= Main.screenHeight / 2;

                        Vector2 mapPosition = Main.mapFullscreenPos;
                        Vector2 cursorWorldPosition = mapPosition;

                        cursorPosition /= 16;
                        cursorPosition *= 16 / Main.mapFullscreenScale;
                        cursorWorldPosition += cursorPosition;
                        cursorWorldPosition *= 16;

                        cursorWorldPosition.Y -= player.height;
                        if (cursorWorldPosition.X < 0) cursorWorldPosition.X = 0;
                        else if (cursorWorldPosition.X + player.width > mapWidth) cursorWorldPosition.X = mapWidth - player.width;

                        if (cursorWorldPosition.Y < 0) cursorWorldPosition.Y = 0;
                        else if (cursorWorldPosition.Y + player.height > mapHeight) cursorWorldPosition.Y = mapHeight - player.height;

                        DebugUtil.Log(string.Format("Cursor position is {0} {1}", cursorWorldPosition.X, cursorWorldPosition.Y));

                        Vector2 target = SeekValidTarget(cursorWorldPosition);
                        if (target != Vector2.Zero)
                        {
                            float distance = Vector2.Distance(player.Center, target);
                            if (modPlayer.TryTransmission(target, distance))
                            {
                                Main.mapFullscreen = false;
                            }
                        }
                    }
                }
            }
        }

        public Vector2 SeekValidTarget(Vector2 cursor)
        {
            Vector2 npcTarget = SeekNPCTarget(cursor);
            if (npcTarget != Vector2.Zero)
                return npcTarget;
            Vector2 playerTarget = SeekPlayerTarget(cursor);
            if (playerTarget != Vector2.Zero)
                return playerTarget;
            Vector2 beaconTarget = SeekBeaconTarget(cursor);
            if (beaconTarget != Vector2.Zero)
                return beaconTarget;
            return Vector2.Zero;
        }

        public Vector2 SeekBeaconTarget(Vector2 cursorWorldPosition)
        {
            foreach (var beaconLocation in DBZWorld.GetWorld().kiBeacons)
            {
                var beaconCenter = beaconLocation + new Vector2(24, -24); // roughly half the size of a beacon, looking for its center, slightly "above" it.

                if (IsValidTarget(cursorWorldPosition, beaconCenter))
                {
                    return beaconLocation + new Vector2(16, -48);
                }
            }

            return Vector2.Zero;
        }

        public Vector2 SeekNPCTarget(Vector2 cursorWorldPosition)
        {
            for (int npcIndex = 0; npcIndex < 200; npcIndex++)
            {
                NPC npc = Main.npc[npcIndex];
                if (npc == null)
                    continue;
                if (!npc.active)
                    continue;
                if (!npc.townNPC)
                    continue;

                if (IsValidTarget(cursorWorldPosition, npc.Center))
                {
                    return npc.position;
                }
            }

            return Vector2.Zero;
        }

        public Vector2 SeekPlayerTarget(Vector2 cursorWorldPosition)
        {
            for (int playerIndex = 0; playerIndex < 255; playerIndex++)
            {
                Player player = Main.player[playerIndex];
                // no nulls
                if (player == null)
                    continue;
                // no weird shit
                if (player.whoAmI != playerIndex)
                    continue;
                // no hostiles
                bool isSameTeam = Main.player[Main.myPlayer].team == player.team && player.team != 0;
                if ((Main.player[Main.myPlayer].hostile || !player.hostile) && !isSameTeam)
                    continue;
                // no dead bodies
                if (!player.active || player.dead)
                    continue;
                // why would you do this
                if (player.whoAmI == Main.myPlayer)
                    continue;

                if (IsValidTarget(cursorWorldPosition, player.Center))
                {
                    return player.position;
                }               
            }

            return Vector2.Zero;
        }

        private bool IsValidTarget(Vector2 cursor, Vector2 position)
        {
            if (Vector2.Distance(cursor, position) <= 64f + 160f / Main.mapFullscreenScale)
            {
                return true;
            }

            return false;
        }
    }
}
