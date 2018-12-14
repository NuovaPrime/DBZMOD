using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using System;

namespace DBZMOD
{
    public class DBZWorld : ModWorld
    {
        private static bool GenerateGohanHouse = false;
        public static int StartPositionX = 0;
        public static int StartPositionY = 0;
        public override void ResetNearbyTileEffects()
        {
            MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
            modPlayer.kiLantern = false;
        }
        #region Gohan House Bytes

        //18 tiles long, 14 tiles high

        //0 = air, 1 = grey stucco, 2 = blue dynasty shingles, 3 = Smooth marble, 4 = dynasty wood
        private static readonly byte[,] GohanHouseTiles =
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,2,2,2,2,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0,0,0},
            {0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0},
            {2,0,0,0,2,2,2,2,2,2,2,2,2,2,0,0,0,2},
            {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
            {0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
            {0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
            {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0}
        };
        //0 = nothing, 1 = wood wall, 2 = living wood wall, 3 = grey stucco wall, 4 == glass wall
        private static readonly byte[,] GohanHouseWalls =
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,3,3,2,3,3,3,3,2,3,3,0,0,0,0},
            {0,0,0,0,3,3,2,4,4,4,4,2,3,3,0,0,0,0},
            {0,0,0,0,3,3,2,4,4,4,4,2,3,3,0,0,0,0},
            {0,0,0,0,3,3,2,4,4,4,4,2,3,3,0,0,0,0},
            {0,0,0,0,1,1,2,1,1,1,1,2,1,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        //0=none, 1=bottom-left, 2=bottom-right, 3=top-left, 4=top-right, 5=half
        private static readonly byte[,] GohanHouseSlopes =
{
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,0,0,0,0,0,0,0,0,2,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0}
        };
        //0 = nothing, 1 = dynasty door, 2 = dynasty table, 3 = dynasty cup, 4 = large dynasty lantern, 5 = dynasty lantern, 6 = shadewood cabinet, 7 = 4 star dragon ball
        private static readonly byte[,] GohanHouseObjects =
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,4,0,0,0,0,0,5,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,2,0,0,0,6,0,0,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        #endregion



        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            var index = tasks.FindIndex(x => x.Name == "Planting Trees");
            if (index != -1)
            {
                tasks.Add(new PassLegacy("[DBZMOD] Gohan House", AddGohanHouse));
            }
        }

        public void AddGohanHouse(GenerationProgress progress = null)
        {
            if (GenerateGohanHouse)
            {
                return;
            }
            try
            {
                bool Success = MakeGohanHouse(progress);
                if (Success)
                {
                    GenerateGohanHouse = true;
                }
            }
            catch (Exception exception)
            {
                Main.NewText("Oh no, an error happened! Report this to NuovaPrime or MercuriusXeno and send them the file Terraria/ModLoader/Logs/Logs.txt");
                ErrorLogger.Log(exception);
            }
        }

        bool MakeGohanHouse(GenerationProgress progress)
        {
            string GohanHouseGen = "Creating the house of a legend.";
            if (progress != null)
            {
                progress.Message = GohanHouseGen;
                progress.Set(0.50f);
            }
            StartPositionX = WorldGen.genRand.Next(Main.maxTilesX / 2 + 5, Main.spawnTileX + 50);
            for (var Attempts = 0; Attempts < 10000; Attempts++)
            {
                for (var i = 0; i < 25; i++)
                {
                    StartPositionY = 190;
                    do
                    {
                        StartPositionY++;
                    }
                    while ((!Main.tile[StartPositionX + i, StartPositionY].active() && StartPositionY < Main.worldSurface) || Main.tile[StartPositionX + i, StartPositionY].type == TileID.Trees || Main.tile[StartPositionX + i, StartPositionY].type == 27);
                    if (!Main.tile[StartPositionX, StartPositionY].active() || Main.tile[StartPositionX, StartPositionY].liquid > 0)
                    {
                        StartPositionX++;
                    }
                    if (Main.tile[StartPositionX + i, StartPositionY].active())
                    {
                        if (Main.tile[StartPositionX, StartPositionY].liquid > 0)
                            StartPositionX = WorldGen.genRand.Next(Main.maxTilesX / 2 + 5, Main.spawnTileX + 50);
                        goto GenerateBuild;
                    }
                }
            }
            goto GenerateBuild;
            return false;

            GenerateBuild:
            for (var X = 0; X < GohanHouseTiles.GetLength(1); X++)
            {
                for (var Y = 0; Y < GohanHouseTiles.GetLength(0); Y++)
                {
                    var tile = Framing.GetTileSafely(StartPositionX + X, StartPositionY - Y);
                    switch (GohanHouseTiles[Y, X])
                    {
                        case 0:
                            break;
                        case 1:
                            tile.type = TileID.GrayStucco;
                            tile.active(true);
                            break;
                        case 2:
                            tile.type = TileID.BlueDynastyShingles;
                            tile.active(true);
                            break;
                        case 3:
                            tile.type = TileID.Marble;
                            tile.active(true);
                            break;
                        case 4:
                            tile.type = TileID.DynastyWood;
                            tile.active(true);
                            break;
                    }
                    switch (GohanHouseWalls[Y, X])
                    {
                        case 0:
                            tile.wall = 0;
                            break;
                        case 1:
                            tile.wall = WallID.Wood;
                            break;
                        case 2:
                            tile.wall = WallID.LivingWood;
                            break;
                        case 3:
                            tile.wall = WallID.Gray;
                            break;
                        case 4:
                            tile.wall = WallID.Glass;
                            break;
                    }
                    tile.slope(GohanHouseSlopes[Y, X]);
                }
            }
            for (var X = 0; X < GohanHouseTiles.GetLength(1); X++)
            {
                for (var Y = 0; Y < GohanHouseTiles.GetLength(0); Y++)
                {
                    var tile = Framing.GetTileSafely(StartPositionX + X, StartPositionY + 2 - Y);
                    switch (GohanHouseObjects[Y, X])
                    {
                        case 0:
                            break;
                        case 1:
                            WorldGen.PlaceObject(StartPositionX + X, StartPositionY + 2 - Y, TileID.ClosedDoor, true, 29);
                            break;
                        case 2:
                            WorldGen.PlaceObject(StartPositionX + X, StartPositionY + 2 - Y, TileID.Tables, true, 26);
                            break;
                        case 3:
                            WorldGen.PlaceObject(StartPositionX + X, StartPositionY + 2 - Y, TileID.Bottles, true, 6);
                            break;
                        case 4:
                            WorldGen.PlaceObject(StartPositionX + X, StartPositionY + 2 - Y, TileID.Chandeliers, true, 23);
                            tile.color(28);
                            break;
                        case 5:
                            WorldGen.PlaceObject(StartPositionX + X, StartPositionY + 2 - Y, TileID.HangingLanterns, true, 27);
                            break;
                        case 6:
                            WorldGen.PlaceObject(StartPositionX + X, StartPositionY + 2 - Y, TileID.Dressers, true, 5);
                            break;
                        case 7:
                            WorldGen.PlaceObject(StartPositionX + X, StartPositionY + 2 - Y, (ushort)ModLoader.GetMod("DBZMOD").TileType("FourStarDBTile"));
                            break;
                    }
                }
            }
            return true;
        }
        /*public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int shiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (shiniesIndex == -1)
            {
                return;
            }
            tasks.Insert(shiniesIndex + 12, new PassLegacy("Wasteland", WastelandBiome));
        }
        private void WastelandBiome(GenerationProgress progress)
        {
            progress.Message = "Ageing the world";
            /*int startPositionX = WorldGen.genRand.Next(0, Main.maxTilesX - 2);
            int startPositionY = (int)Main.worldSurface;
            int size = 0;
            if (Main.maxTilesX == 4200 && Main.maxTilesY == 1200)
            {
                size = 105;
            }
            if (Main.maxTilesX == 6300 && Main.maxTilesY == 1800)
            {
                size = 198;
            }
            if (Main.maxTilesX == 8400 && Main.maxTilesY == 2400)
            {
                size = 270;
            }
            if (Main.tile[startPositionX, startPositionY].type == TileID.SnowBlock)
            {
                var startX = startPositionX;
                var startY = startPositionY;
                startX = startX - 100;
                startY = startY - 100;
                startY++;
                for (int x = startX - size; x <= startX + size; x++)
                {
                    for (int y = startY - size; y <= startY + size; y++)
                    {
                        if (Vector2.Distance(new Vector2(startX, startY), new Vector2(x, y)) <= size)
                        {
                            if (Main.tile[x, y].wall == WallID.Sandstone || Main.tile[x, y].wall == WallID.HardenedSand)
                            {
                                Main.tile[x, y].wall = (ushort)mod.WallType("CoarseRockWall");
                            }
                            if (Main.tile[x, y].type == TileID.Sandstone || Main.tile[x, y].type == TileID.Sand)
                            {
                                Main.tile[x, y].type = (ushort)mod.TileType("CoarseRockTile");
                            }
                        }
                    }
                }
            }*/
        /*for (int i = 0; i < Main.maxTilesX / 900; i++)
        {



            int y = ((int)WorldGen.worldSurfaceLow - Main.maxTilesY);
            int x = (int)Main.worldSurface;

            WorldGen.TileRunner(x, y, WorldGen.genRand.Next(40, 100), WorldGen.genRand.Next(50, 200), mod.TileType("CoarseRockTile"), true, 0f, 0f, false, true);
        }

        int X = WorldGen.genRand.Next(1, Main.maxTilesX - 300);
        int firstY = WorldGen.genRand.Next((int)WorldGen.rockLayer, (int)WorldGen.rockLayer + 100);
        int Y = RaycastDown(X, firstY);
        for (int i = 0; i < Main.maxTilesX / 600; i++)
        {
            WorldGen.TileRunner(X, Y, 300, WorldGen.genRand.Next(100, 200), mod.TileType("CoarseRockTile"), false, 0f, 0f, true, true);
        }
    }
    public int RaycastDown(int x, int y)
    {
        while (!Main.tile[x, y].active())
        {
            y++;
        }
        return y;
    }*/
    }
}
