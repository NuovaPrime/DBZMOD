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
                                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, DBZMOD.instance.ProjectileType("TransmissionLinesProj"), 0, 0, player.whoAmI);                                
                                player.Teleport(cursorWorldPosition, -1);
                                Projectile.NewProjectile(cursorWorldPosition.X, cursorWorldPosition.Y, 0f, 0f, DBZMOD.instance.ProjectileType("TransmissionLinesProj"), 0, 0, player.whoAmI);
                                player.position = cursorWorldPosition;
                                player.velocity = Vector2.Zero;
                                player.fallStart = (int)(player.position.Y / 16f);
                                // dunno if either of these works
                                //player.mapFullScreen = false;
                                Main.mapFullscreen = false;
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
            float mapScale = Main.mapMinimapScale;
            for (int npcIndex = 0; npcIndex < 200; npcIndex++)
            {
                NPC npc = Main.npc[npcIndex];
                if (npc == null)
                    continue;
                if (!npc.active)
                    continue;
                if (!npc.townNPC)
                    continue;                
                
                float npcWidth = npc.width;
                float npcHeight = npc.height;
                float npcPosX = npc.position.X;
                float npcPosY = npc.position.Y;
                // sprite icons are "larger than they appear", so we give a bunch of padding to the hit detection routine, exaggerating the bounds
                float leftMargin = npcWidth * (1f + (0.5f * mapScale));
                float rightMargin = npcWidth * (2f + (0.5f * mapScale));
                float topMargin = npcHeight * (1f + (0.5f * mapScale));
                float bottomMargin = npcHeight * (2f + (0.5f * mapScale));
                float npcLowerXBound = npcPosX - leftMargin;
                float npcLowerYBound = npcPosY - topMargin;
                float npcUpperXBound = npcPosX + rightMargin;
                float npcUpperYBound = npcPosY + bottomMargin;
                if (cursorWorldPosition.X >= npcLowerXBound && cursorWorldPosition.X <= npcUpperXBound && cursorWorldPosition.Y >= npcLowerYBound && cursorWorldPosition.Y <= npcUpperYBound)
                {
                    return Main.npc[npcIndex];
                }
                
            }

            return null;
        }

        public Player SeekPlayerTarget(Vector2 cursorWorldPosition)
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
                var mapScale = Main.mapMinimapScale;

                float playerWidth = player.width;
                float playerHeight = player.height;
                float playerPosX = player.position.X;
                float playerPosY = player.position.Y;
                // sprite icons are "larger than they appear", so we give a bunch of padding to the hit detection routine, exaggerating the bounds
                float leftMargin = playerWidth * (1f + (0.5f * mapScale));
                float rightMargin = playerWidth * (2f + (0.5f * mapScale));
                float topMargin = playerHeight * (1f + (0.5f * mapScale)); // for some reason the heighest point of the sprite is a bit misleading, so give this some padding
                float bottomMargin = playerHeight * (2f + (0.5f * mapScale));
                float playerLowerXBound = playerPosX - leftMargin;
                float playerLowerYBound = playerPosY - topMargin;
                float playerUpperXBound = playerPosX + rightMargin;
                float playerUpperYBound = playerPosY + bottomMargin;
                if (!player.dead)
                {
                    if (cursorWorldPosition.X >= playerLowerXBound && cursorWorldPosition.X <= playerUpperXBound && cursorWorldPosition.Y >= playerLowerYBound && cursorWorldPosition.Y <= playerUpperYBound)
                    {
                        return player;
                    }
                }
            }

            return null;
        }
    }
}
