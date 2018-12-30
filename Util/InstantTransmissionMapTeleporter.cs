using Microsoft.Xna.Framework;
using Network;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameInput;

namespace DBZMOD.Util
{
    public class InstantTransmissionMapTeleporter
    {
        public static InstantTransmissionMapTeleporter instance;

        public InstantTransmissionMapTeleporter()
        {
            instance = this;
        }

        public void PostDrawFullScreenMap()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            var player = modPlayer.player;
            if (modPlayer.IsInstantTransmission1Unlocked)
            {
                if (!modPlayer.HasKi(modPlayer.GetInstantTransmissionTeleportKiCost()))
                {
                    Main.spriteBatch.DrawString(Main.fontMouseText, "Your Ki is too low to use instant transmission.", new Vector2(15, Main.screenHeight - 120), Color.White);
                }
                else
                {
                    Main.spriteBatch.DrawString(Main.fontMouseText, "Press the Instant Transmission key an NPC, beacon or player icon to teleport using Ki.", new Vector2(15, Main.screenHeight - 120), Color.White);

                    if (MyPlayer.InstantTransmission.JustPressed)
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

                        // loop over non-hostile players and npcs
                        NPC npcTarget = SeekNPCTarget(cursorWorldPosition);
                        Player playerTarget = SeekPlayerTarget(cursorWorldPosition);
                        // temp disabled beacon targets until they have a tile/sprite, then more work done
                        // Tile beaconTarget = SeekBeaconTarget(cursorWorldPosition);
                        if (npcTarget != null || playerTarget != null)
                        {
                            modPlayer.AddKi(-modPlayer.GetInstantTransmissionTeleportKiCost());
                            modPlayer.AddInstantTransmissionChaosDebuff(Vector2.Distance(cursorWorldPosition, player.Center));
                            if (Main.netMode == 0) // single
                            {
                                player.Teleport(cursorWorldPosition, 1, 0);
                                player.position = cursorWorldPosition;
                                player.velocity = Vector2.Zero;
                                player.fallStart = (int)(player.position.Y / 16f);
                            }
                            else
                            {
                                NetworkHelper.playerSync.RequestTeleport(256, player.whoAmI, cursorWorldPosition);
                            }
                        }
                    }
                }
            }
        }

        public NPC SeekNPCTarget(Vector2 cursorWorldPosition)
        {
            for (int npcIndex = 0; npcIndex < 200; npcIndex++)
            {
                if (Main.npc[npcIndex] != null && Main.npc[npcIndex].active && Main.npc[npcIndex].townNPC)
                {
                    var mapScale = Main.mapMinimapScale;
                    var mapAlphaThing = (byte)(255f * Main.mapMinimapAlpha);
                    var mapX = (float)Main.miniMapX;
                    var mapY = (float)Main.miniMapY;
                    float mapScreenPosX = (Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f;
                    float mapScreenPosY = (Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f;
                    // var mapOffsetX = -(mapScreenPosX - (float)((int)((Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f))) * mapScale;
                    // var mapOffsetY = -(mapScreenPosY - (float)((int)((Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f))) * mapScale;
                    var widthRatio = (float)Main.miniMapWidth / mapScale;
                    var heightRatio = (float)Main.miniMapHeight / mapScale;
                    var widthOffsetByRatio = (float)((int)mapScreenPosX) - widthRatio / 2f;
                    var heightOffsetByRatio = (float)((int)mapScreenPosY) - heightRatio / 2f;
                    // float uiScale = (mapScale * 0.25f * 2f + 1f) / 3f; // what is this spaghetti shit
                    float uiScale = Main.UIScale;
                    float npcPosX = ((Main.npc[npcIndex].position.X + (float)(Main.npc[npcIndex].width / 2)) / 16f - widthOffsetByRatio) * mapScale;
                    float npcPosY = ((Main.npc[npcIndex].position.Y + Main.npc[npcIndex].gfxOffY + (float)(Main.npc[npcIndex].height / 2)) / 16f - heightOffsetByRatio) * mapScale;
                    npcPosX += mapX;
                    npcPosY += mapY;
                    npcPosX -= 6f;
                    npcPosY -= 2f;
                    npcPosY -= 2f - mapScale / 5f * 2f;
                    npcPosX -= 10f * mapScale;
                    npcPosY -= 10f * mapScale;
                    float npcLowerXBound = npcPosX + 4f - 14f * uiScale;
                    float npcLowerYBound = npcPosY + 2f - 14f * uiScale;
                    float npcUpperXBound = npcLowerXBound + 28f * uiScale;
                    float npcUpperYBound = npcLowerYBound + 28f * uiScale;
                    if (!Main.player[npcIndex].dead)
                    {
                        if ((float)Main.mouseX >= npcLowerXBound && (float)Main.mouseX <= npcUpperXBound && (float)Main.mouseY >= npcLowerYBound && (float)Main.mouseY <= npcUpperYBound)
                        {
                            return Main.npc[npcIndex];
                        }
                    }
                }
            }

            return null;
        }

        public Player SeekPlayerTarget(Vector2 cursorWorldPosition)
        {
            for (int playerIndex = 0; playerIndex < 255; playerIndex++)
            {
                // no nulls
                if (Main.player[playerIndex] == null)
                    continue;
                // no weird shit
                if (Main.player[playerIndex].whoAmI != playerIndex)
                    continue;
                // no hostiles
                bool isSameTeam = Main.player[Main.myPlayer].team == Main.player[playerIndex].team && Main.player[playerIndex].team != 0;
                if ((Main.player[Main.myPlayer].hostile || !Main.player[playerIndex].hostile) && !isSameTeam)
                    continue;
                // no dead bodies
                if (!Main.player[playerIndex].active || Main.player[playerIndex].dead)
                    continue;
                // why would you do this
                if (Main.player[playerIndex].whoAmI == Main.myPlayer)
                    continue;
                var mapScale = Main.mapMinimapScale;
                var mapAlphaThing = (byte)(255f * Main.mapMinimapAlpha);
                var mapX = (float)Main.miniMapX;
                var mapY = (float)Main.miniMapY;
                float mapScreenPosX = (Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f;
                float mapScreenPosY = (Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f;
                // var mapOffsetX = -(mapScreenPosX - (float)((int)((Main.screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f))) * mapScale;
                // var mapOffsetY = -(mapScreenPosY - (float)((int)((Main.screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f))) * mapScale;
                var widthRatio = (float)Main.miniMapWidth / mapScale;
                var heightRatio = (float)Main.miniMapHeight / mapScale;
                var widthOffsetByRatio = (float)((int)mapScreenPosX) - widthRatio / 2f;
                var heightOffsetByRatio = (float)((int)mapScreenPosY) - heightRatio / 2f;
                // float uiScale = (mapScale * 0.25f * 2f + 1f) / 3f; // what is this spaghetti shit
                float uiScale = Main.UIScale;
                float playerPosX = ((Main.player[playerIndex].position.X + (float)(Main.player[playerIndex].width / 2)) / 16f - widthOffsetByRatio) * mapScale;
                float playerPosY = ((Main.player[playerIndex].position.Y + Main.player[playerIndex].gfxOffY + (float)(Main.player[playerIndex].height / 2)) / 16f - heightOffsetByRatio) * mapScale;
                playerPosX += mapX;
                playerPosY += mapY;
                playerPosX -= 6f;
                playerPosY -= 2f;
                playerPosY -= 2f - mapScale / 5f * 2f;
                playerPosX -= 10f * mapScale;
                playerPosY -= 10f * mapScale;
                float playerLowerXBound = playerPosX + 4f - 14f * uiScale;
                float playerLowerYBound = playerPosY + 2f - 14f * uiScale;
                float playerUpperXBound = playerLowerXBound + 28f * uiScale;
                float playerUpperYBound = playerLowerYBound + 28f * uiScale;
                if (!Main.player[playerIndex].dead)
                {
                    if ((float)Main.mouseX >= playerLowerXBound && (float)Main.mouseX <= playerUpperXBound && (float)Main.mouseY >= playerLowerYBound && (float)Main.mouseY <= playerUpperYBound)
                    {
                        return Main.player[playerIndex];
                    }
                }
            }

            return null;
        }
    }
}
