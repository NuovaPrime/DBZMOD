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
using System.Linq;
using Util;

namespace DBZMOD
{
    public class DBZWorld : ModWorld
    {
        private static bool GenerateGohanHouse = false;
        private static bool IsGohanHouseCleaned = false;
        public static int GohanHouseStartPositionX = 0;
        public static int GohanHouseStartPositionY = 0;
        public override void ResetNearbyTileEffects()
        {
            MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
            modPlayer.kiLantern = false;
        }
        #region Gohan House Bytes

        //18 tiles long, 16 tiles high (with dirt on the bottom)

        //0 = air, 1 = grey stucco, 2 = blue dynasty shingles, 3 = Smooth marble, 4 = dynasty wood, 5 = dirt
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
        //0 = nothing, 1 = wood wall, 2 = living wood wall, 3 = grey stucco wall, 4 = glass wall
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

        //0=none, 1=bottom-right, 2=bottom-left, 3=top-left, 4=top-right, 5=half
        private static readonly byte[,] GohanHouseSlopes =
{
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,2,0,0,0,0,0,0,0,0,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}
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
            {0,0,0,0,0,0,0,2,0,0,0,6,0,0,1,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        #endregion



        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            // useful debug tool type thing. Help me find the names of tasks to decide where to put this one.
            var taskNames = tasks.Select(x => x.Name).ToList();
            // I've tried injecting the task before "Piles" and "Spreading Grass", "Piles" can cause furniture interference.
            // Neither works. So far "Planting Trees" is the only one I can get to work.
            var index = tasks.FindIndex(x => x.Name == "Planting Trees");
            if (index != -1)
            {                
                tasks.Insert(index, new PassLegacy("[DBZMOD] Gohan House", AddGohanHouse));
            }

            // insert a cleanup task
            index = tasks.FindIndex(x => x.Name == "Micro Biomes");
            if (index != -1)
            {
                tasks.Insert(index, new PassLegacy("[DBZMOD] Gohan House Validation", CleanupGohanHouse));
            }
        }

        public void AddGohanHouse(GenerationProgress progress = null)
        {
            if (GenerateGohanHouse)
                return;

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
                Main.NewText("Oh no, an error happened [AddGohanHouse]! Report this to NuovaPrime or MercuriusXeno and send them the file Terraria/ModLoader/Logs/Logs.txt");
                ErrorLogger.Log(exception);
            }
        }

        public void CleanupGohanHouse(GenerationProgress progress = null)
        {
            // bail if the house never generated, something is wrong :(
            if (!GenerateGohanHouse)
                return;

            // bail if already done.
            if (IsGohanHouseCleaned)
                return;

            try
            {
                bool Success = RunGohanCleanupRoutine(progress);
                if (Success)
                {
                    IsGohanHouseCleaned = true;
                }
            }
            catch (Exception exception)
            {
                Main.NewText("Oh no, an error happened [CleanupGohanHouse]! Report this to NuovaPrime or MercuriusXeno and send them the file Terraria/ModLoader/Logs/Logs.txt");
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

            GohanHouseStartPositionX = WorldGen.genRand.Next(Main.maxTilesX / 2 - 70, Main.spawnTileX - 25);
            for (var Attempts = 0; Attempts < 10000; Attempts++)
            {
                for (var i = 0; i < 25; i++)
                {
                    GohanHouseStartPositionY = 190;
                    do
                    {
                        GohanHouseStartPositionY++;
                    }
                    while ((!Main.tile[GohanHouseStartPositionX + i, GohanHouseStartPositionY].active() && GohanHouseStartPositionY < Main.worldSurface) || Main.tile[GohanHouseStartPositionX + i, GohanHouseStartPositionY].type == TileID.Trees || Main.tile[GohanHouseStartPositionX + i, GohanHouseStartPositionY].type == 27);
                    if (!Main.tile[GohanHouseStartPositionX, GohanHouseStartPositionY].active() || Main.tile[GohanHouseStartPositionX, GohanHouseStartPositionY].liquid > 0)
                    {
                        GohanHouseStartPositionX++;
                    }
                    if (Main.tile[GohanHouseStartPositionX + i, GohanHouseStartPositionY].active())
                    {
                        if (Main.tile[GohanHouseStartPositionX, GohanHouseStartPositionY].liquid > 0)
                            GohanHouseStartPositionX = WorldGen.genRand.Next(Main.maxTilesX / 2 - 70, Main.spawnTileX - 25);
                        goto GenerateBuild;
                    }
                }
            }
            goto GenerateBuild;
            return false;

        GenerateBuild:
            GenerateGohanStructureWithByteArrays();
            return true;
        }

        public void GenerateGohanStructureWithByteArrays()
        {

            // if we're here it means we are ready to generate our structure

            // tiles
            for (var X = 0; X < GohanHouseTiles.GetLength(1); X++)
            {
                for (var Y = 0; Y < GohanHouseTiles.GetLength(0); Y++)
                {
                    int offsetX = GohanHouseStartPositionX + X;
                    int offsetY = GohanHouseStartPositionY + Y - GohanHouseTiles.GetLength(0);
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    switch (GohanHouseTiles[Y, X])
                    {
                        case 0:
                            tile.type = 0;
                            tile.active(false);
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
                            tile.type = TileID.MarbleBlock;
                            tile.active(true);
                            break;
                        case 4:
                            tile.type = TileID.DynastyWood;
                            tile.active(true);
                            break;
                        case 5:
                            tile.type = TileID.Grass;
                            tile.active(true);
                            break;
                    }
                }
            }
            for (var X = 0; X < GohanHouseSlopes.GetLength(1); X++)
            {
                for (var Y = 0; Y < GohanHouseSlopes.GetLength(0); Y++)
                {
                    int offsetX = GohanHouseStartPositionX + X;
                    int offsetY = GohanHouseStartPositionY + Y - GohanHouseSlopes.GetLength(0);
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    tile.slope(GohanHouseSlopes[Y, X]);
                }
            }
            // walls
            for (var X = 0; X < GohanHouseWalls.GetLength(1); X++)
            {
                for (var Y = 0; Y < GohanHouseWalls.GetLength(0); Y++)
                {
                    int offsetX = GohanHouseStartPositionX + X;
                    int offsetY = GohanHouseStartPositionY + Y - GohanHouseWalls.GetLength(0);
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
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
                }
            }
            // Objects
            for (var X = 0; X < GohanHouseObjects.GetLength(1); X++)
            {
                // house objects are different.. they go in reverse (ground up) so that the bottle placement actually works.
                for (var Y = GohanHouseObjects.GetLength(0) - 1; Y >= 0; Y--)
                {
                    int offsetX = GohanHouseStartPositionX + X;
                    int offsetY = GohanHouseStartPositionY + Y - GohanHouseObjects.GetLength(0);
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    // break rocks!
                    if (tile.type == TileID.SmallPiles || tile.type == TileID.LargePiles || tile.type == TileID.LargePiles2 || tile.type == TileID.Dirt || tile.type == TileID.Stone)
                    {
                        // nullify tiles?
                        WorldGen.KillTile(offsetX, offsetY);
                        tile = Framing.GetTileSafely(offsetX, offsetY);
                        Main.NewText("Test");
                    }
                    switch (GohanHouseObjects[Y, X])
                    {
                        case 0:
                            break;
                        case 1:
                            WorldGen.PlaceObject(offsetX, offsetY, TileID.ClosedDoor, true, 28); // confirmed dynasty door
                            break;
                        case 2:
                            WorldGen.PlaceObject(offsetX, offsetY, TileID.Tables, true, 25); // confirmed dynasty table
                            break;
                        case 3:
                            WorldGen.PlaceObject(offsetX, offsetY, TileID.Bottles, true, 5); // confirmed dynasty cup
                            break;
                        case 4:
                            WorldGen.PlaceObject(offsetX, offsetY, TileID.Chandeliers, true, 22); // confirmed large dynasty lantern
                            tile.color(28);
                            break;
                        case 5:
                            WorldGen.PlaceObject(offsetX, offsetY, TileID.HangingLanterns, true, 26); // confirmed dynasty hanging lantern (small one)
                            break;
                        case 6:
                            WorldGen.PlaceObject(offsetX, offsetY, TileID.Dressers, true, 4); // confirmed shadewood dresser
                            break;
                        case 7:
                            WorldGen.PlaceObject(GohanHouseStartPositionX + X, GohanHouseStartPositionY + 2 - Y, (ushort)ModLoader.GetMod("DBZMOD").TileType("FourStarDBTile"), false);
                            break;
                    }
                }
            }

            // sample tiles at the origin (it's to the right, this isn't perfect)
            var sampleTile = Framing.GetTileSafely(GohanHouseStartPositionX, GohanHouseStartPositionY + 1);
            bool isSnowBiome = false;
            if (sampleTile.type == TileID.SnowBlock || sampleTile.type == TileID.IceBlock)
                isSnowBiome = true;


            // experimental, also doesn't work when the tiles below are snow... which happens at spawn sometimes.
            // put dirt under the house and make sure gaps are filled. this might look weird.
            for (var Y = 0; Y < 5; Y++)
            {
                for (var X = -1 - (Y * 2); X < GohanHouseTiles.GetLength(1) + 1 + (Y * 2); X++)
                {
                    int offsetX = GohanHouseStartPositionX + X;
                    int offsetY = GohanHouseStartPositionY + Y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    bool isEdge = IsAnySideExposed(offsetX, offsetY);
                    tile.type = isSnowBiome ? TileID.SnowBlock : (isEdge ? TileID.Grass : TileID.Dirt);
                    // if it's a slope, unslope that shit. quit putting gaps in the ground terraria.
                    tile.slope(0);
                    tile.halfBrick(false);
                    tile.active(true);
                }
            }
        }

        bool RunGohanCleanupRoutine(GenerationProgress progress)
        {
            // we already have the starting position, just cut straight to the build cleanup.
            string GohanHouseGen = "Cleaning up Grandpa's House...";
            if (progress != null)
            {
                progress.Message = GohanHouseGen;
                progress.Set(0.50f);
            }

            GenerateGohanStructureWithByteArrays();
            return true;
        }

        public bool IsAnySideExposed(int startX, int startY)
        {
            for (var offX = -1; offX <= 1; offX++)
            {
                for (var offY = -1; offY <= 1; offY++)
                {
                    var tile = Framing.GetTileSafely(startX + offX, startY + offY);
                    if (!tile.active())
                        return true;
                }
            }
            return false;
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
