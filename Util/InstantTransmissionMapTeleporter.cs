using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public void HandleDrawingKiBeacons()
        {
            Texture2D texture = DBZMOD.instance.GetTexture("UI/KiBeaconMap");

            // desperation starts here
            foreach (var beaconLocation in DBZWorld.GetWorld().KiBeacons)
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
            if (modPlayer.IsInstantTransmission1Unlocked)
            {
                if (!modPlayer.HasKi(modPlayer.GetInstantTransmissionTeleportKiCost()))
                {
                    Main.spriteBatch.DrawString(Main.fontMouseText, "Your Ki is too low to use instant transmission.", new Vector2(15, Main.screenHeight - 120), Color.White);
                }
                else
                {
                    Main.spriteBatch.DrawString(Main.fontMouseText, "Press the Instant Transmission key on an NPC, beacon or player icon to teleport using Ki.", new Vector2(15, Main.screenHeight - 120), Color.White);

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
                        Vector2 beaconTarget = SeekBeaconTarget(cursorWorldPosition);
                        if (npcTarget != null || playerTarget != null || beaconTarget != Vector2.Zero)
                        {
                            float distance = Vector2.Distance(player.Center, cursorWorldPosition);
                            if (modPlayer.TryTransmission(cursorWorldPosition, distance))
                            {
                                // dunno if either of these works
                                Main.mapFullscreen = false;
                            }
                        }
                    }
                }
            }
        }

        public Vector2 SeekBeaconTarget(Vector2 cursorWorldPosition)
        {
            float mapScale = Main.mapFullscreenScale;
            foreach (var beaconLocation in DBZWorld.GetWorld().KiBeacons)
            {
                float beaconWidth = 48;
                float beaconHeight = 48;
                float beaconPosX = beaconLocation.X;
                float beaconLocationY = beaconLocation.Y - 32f;

                // sprite icons are "larger than they appear", so we give a bunch of padding to the hit detection routine, exaggerating the bounds
                float leftMargin = beaconWidth * (1f + (0.5f * mapScale));
                float rightMargin = beaconWidth * (2f + (0.5f * mapScale));
                float topMargin = beaconHeight * (1f + (0.5f * mapScale));
                float bottomMargin = beaconHeight * (2f + (0.5f * mapScale));
                float beaconLowerXBound = beaconPosX - leftMargin;
                float beaconLowerYBound = beaconLocationY - topMargin;
                float beaconUpperXBound = beaconPosX + rightMargin;
                float beaconUpperYBound = beaconLocationY + bottomMargin;
                if (cursorWorldPosition.X >= beaconLowerXBound && cursorWorldPosition.X <= beaconUpperXBound && cursorWorldPosition.Y >= beaconLowerYBound && cursorWorldPosition.Y <= beaconUpperYBound)
                {
                    return beaconLocation;
                }
            }

            return Vector2.Zero;
        }

        public NPC SeekNPCTarget(Vector2 cursorWorldPosition)
        {
            float mapScale = Main.mapFullscreenScale;
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
            var mapScale = Main.mapFullscreenScale;
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
