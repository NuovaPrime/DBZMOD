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
using DBZMOD.Items.DragonBalls;
using Network;
using DBZMOD.Util;

namespace DBZMOD
{
    public class DBZWorld : ModWorld
    {
        // initialize dragon ball locations to an empty set of points. These get loaded in the world data load segment.
        public Point[] DragonBallLocations = new Point[7] { new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1, -1) };
        public List<Vector2> KiBeacons = new List<Vector2>();
        // the world dragon ball key is a special random integer that gets created with the world. When the player takes a dragon ball in tile form, the item created
        // has its key set. Taking that ball to another world results in it transforming into something that sucks and doesn't work and isn't a dragon ball.        
        public int WorldDragonBallKey = 0;

        // helper utility method for snagging the currently loaded world.
        public static DBZWorld GetWorld()
        {
            return DBZMOD.instance.GetModWorld("DBZWorld") as DBZWorld;
        }

        public override TagCompound Save()
        {
            var dbtWorldTag = new TagCompound();
            dbtWorldTag.Add("WorldDragonBallKey", WorldDragonBallKey);
            dbtWorldTag.Add("OneStarDragonBallX", DragonBallLocations[0].X);
            dbtWorldTag.Add("OneStarDragonBallY", DragonBallLocations[0].Y);
            dbtWorldTag.Add("TwoStarDragonBallX", DragonBallLocations[1].X);
            dbtWorldTag.Add("TwoStarDragonBallY", DragonBallLocations[1].Y);
            dbtWorldTag.Add("ThreeStarDragonBallX", DragonBallLocations[2].X);
            dbtWorldTag.Add("ThreeStarDragonBallY", DragonBallLocations[2].Y);
            dbtWorldTag.Add("FourStarDragonBallX", DragonBallLocations[3].X);
            dbtWorldTag.Add("FourStarDragonBallY", DragonBallLocations[3].Y);
            dbtWorldTag.Add("FiveStarDragonBallX", DragonBallLocations[4].X);
            dbtWorldTag.Add("FiveStarDragonBallY", DragonBallLocations[4].Y);
            dbtWorldTag.Add("SixStarDragonBallX", DragonBallLocations[5].X);
            dbtWorldTag.Add("SixStarDragonBallY", DragonBallLocations[5].Y);
            dbtWorldTag.Add("SevenStarDragonBallX", DragonBallLocations[6].X);
            dbtWorldTag.Add("SevenStarDragonBallY", DragonBallLocations[6].Y);
            dbtWorldTag.Add("KiBeacons", KiBeacons);
            return dbtWorldTag;
        }

        public override void Load(TagCompound tag)
        {
            var dbKey = tag.GetInt("WorldDragonBallKey");
            WorldDragonBallKey = dbKey;
            var oneStarPoint = new Point(tag.GetInt("OneStarDragonBallX"), tag.GetInt("OneStarDragonBallY"));
            var twoStarPoint = new Point(tag.GetInt("TwoStarDragonBallX"), tag.GetInt("TwoStarDragonBallY"));
            var threeStarPoint = new Point(tag.GetInt("ThreeStarDragonBallX"), tag.GetInt("ThreeStarDragonBallY"));
            var fourStarPoint = new Point(tag.GetInt("FourStarDragonBallX"), tag.GetInt("FourStarDragonBallY"));
            var fiveStarPoint = new Point(tag.GetInt("FiveStarDragonBallX"), tag.GetInt("FiveStarDragonBallY"));
            var sixStarPoint = new Point(tag.GetInt("SixStarDragonBallX"), tag.GetInt("SixStarDragonBallY"));
            var sevenStarPoint = new Point(tag.GetInt("SevenStarDragonBallX"), tag.GetInt("SevenStarDragonBallY"));
            DragonBallLocations[0] = oneStarPoint;
            DragonBallLocations[1] = twoStarPoint;
            DragonBallLocations[2] = threeStarPoint;
            DragonBallLocations[3] = fourStarPoint;
            DragonBallLocations[4] = fiveStarPoint;
            DragonBallLocations[5] = sixStarPoint;
            DragonBallLocations[6] = sevenStarPoint;
            KiBeacons = tag.ContainsKey("KiBeacons") ? (List<Vector2>)tag.GetList<Vector2>("KiBeacons") : new List<Vector2>();
            base.Load(tag);
        }

        public override void PostWorldGen()
        {
            base.PostWorldGen();
            GenerateGohanHouse = false;
            IsGohanHouseCleaned = false;
            IsDragonBallPlacementDone = false;
            IsGohanHouseOffsetSet = false;
        }

        private bool GenerateGohanHouse = false;
        private bool IsGohanHouseCleaned = false;
        public bool IsDragonBallPlacementDone = false;
        public int GohanHouseStartPositionX = 0;
        public int GohanHouseStartPositionY = 0;
        public override void ResetNearbyTileEffects()
        {
            MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
            modPlayer.kiLantern = false;
            modPlayer.OneStarDBNearby = false;
            modPlayer.TwoStarDBNearby = false;
            modPlayer.ThreeStarDBNearby = false;
            modPlayer.FourStarDBNearby = false;
            modPlayer.FiveStarDBNearby = false;
            modPlayer.SixStarDBNearby = false;
            modPlayer.SevenStarDBNearby = false;
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

            tasks.Insert(tasks.Count - 1, new PassLegacy("[DBZMOD] Placing dragon balls", PlaceDragonballs));
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

        public void PlaceDragonballs(GenerationProgress progress = null)
        {
            try
            {
                bool Success = AttemptToPlaceDragonballsInWorld(progress);
                if (Success)
                {
                    IsDragonBallPlacementDone = true;
                }
            }
            catch (Exception exception)
            {
                Main.NewText("Oh no, an error happened [PlacingDragonballs]! Report this to NuovaPrime or MercuriusXeno and send them the file Terraria/ModLoader/Logs/Logs.txt");
                ErrorLogger.Log(exception);
            }
        }

        bool MakeGohanHouse(GenerationProgress progress)
        {            
            string GohanHouseGen = "Creating the house of a legend.";
            if (progress != null)
            {
                progress.Message = GohanHouseGen;
                progress.Set(0.25f);
            }

            // before we do anything, create a new World Key for this world
            WorldDragonBallKey = Main.rand.Next(1, int.MaxValue);

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
                        if (Main.tile[GohanHouseStartPositionX, GohanHouseStartPositionY].liquid > 0) { 
                            GohanHouseStartPositionX = WorldGen.genRand.Next(Main.maxTilesX / 2 - 70, Main.spawnTileX - 25);
                        }
                        else
                        {
                            goto GenerateBuild;
                        }                        
                    }
                }
            }
            goto GenerateBuild;

        GenerateBuild:
            GenerateGohanStructureWithByteArrays(true);
            return true;
        }

        // flag tracking whether the initial house creation point has been offset by the building's height, should only occur once.
        public bool IsGohanHouseOffsetSet = false;
        public void GenerateGohanStructureWithByteArrays(bool isFirstPass)
        {
            if (!IsGohanHouseOffsetSet)
            {
                GohanHouseStartPositionY -= GohanHouseTiles.GetLength(0);
                IsGohanHouseOffsetSet = true;
            }

            // if we're here it means we are ready to generate our structure

            // tiles
            for (var X = 0; X < GohanHouseTiles.GetLength(1); X++)
            {
                for (var Y = 0; Y < GohanHouseTiles.GetLength(0); Y++)
                {
                    int offsetX = GohanHouseStartPositionX + X;
                    int offsetY = GohanHouseStartPositionY + Y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    switch (GohanHouseTiles[Y, X])
                    {
                        case 0:
                            if (isFirstPass)
                            {
                                tile.type = 0;
                                tile.active(false);
                            }
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
                    int offsetY = GohanHouseStartPositionY + Y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    tile.slope(GohanHouseSlopes[Y, X]);
                    tile.halfBrick(false);
                }
            }
            // walls
            for (var X = 0; X < GohanHouseWalls.GetLength(1); X++)
            {
                for (var Y = 0; Y < GohanHouseWalls.GetLength(0); Y++)
                {
                    int offsetX = GohanHouseStartPositionX + X;
                    int offsetY = GohanHouseStartPositionY + Y;
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
                    int offsetY = GohanHouseStartPositionY + Y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    // break rocks!
                    if (tile.type == TileID.SmallPiles || tile.type == TileID.LargePiles || tile.type == TileID.LargePiles2 || tile.type == TileID.Dirt || tile.type == TileID.Stone)
                    {
                        // nullify tiles?
                        WorldGen.KillTile(offsetX, offsetY);
                        tile = Framing.GetTileSafely(offsetX, offsetY);
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
                            if (!isFirstPass)
                            {
                                TryPlacingDragonball(4, offsetX, offsetY);
                            }
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
                    int offsetY = GohanHouseStartPositionY + GohanHouseTiles.GetLength(0) + Y;
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

            GenerateGohanStructureWithByteArrays(false);

            DoDragonBallCleanupCheck();
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

        public static bool IsExistingDragonBall(int whichDragonball)
        {
            int dbIndex = whichDragonball - 1;
            var coordinates = GetWorld().DragonBallLocations[dbIndex];
            if (coordinates == new Point(-1, -1))
                return false;
            var dbTile = Framing.GetTileSafely(coordinates.X, coordinates.Y);
            if (dbTile.type == (ushort)ModLoader.GetMod("DBZMOD").TileType(GetDragonBallTileTypeFromNumber(whichDragonball)))
                return true;
            return false;
        }

        public static string GetDragonBallTileTypeFromNumber(int whichDragonBall)
        {
            switch (whichDragonBall) {
                case 1:
                    return "OneStarDBTile";
                case 2:
                    return "TwoStarDBTile";
                case 3:
                    return "ThreeStarDBTile";
                case 4:
                    return "FourStarDBTile";
                case 5:
                    return "FiveStarDBTile";
                case 6:
                    return "SixStarDBTile";
                case 7:
                    return "SevenStarDBTile";
                default:
                    return "";
            }
        }               

        public static bool IsDragonBallWithPlayers(int whichDragonball, Player ignorePlayer = null)
        {
            for (var i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i] == null)
                    continue;
                if (Main.player[i].whoAmI != i)
                    continue;
                if (ignorePlayer != null && Main.player[i].whoAmI == ignorePlayer.whoAmI)
                    continue;
                var player = Main.player[i];                
                if (InventoryContainsDragonball(whichDragonball, player.inventory))
                    return true;
                // bank, bank2 and bank3 are player safes (piggy banks)
                if (InventoryContainsDragonball(whichDragonball, player.bank.item))
                    return true;

                if (InventoryContainsDragonball(whichDragonball, player.bank2.item))
                    return true;

                if (InventoryContainsDragonball(whichDragonball, player.bank3.item))
                    return true;
            }
            return false;
        }

        public static bool InventoryContainsDragonball(int whichDragonball, Item[] inventory)
        {
            for (var i = 0; i < inventory.Length; i++)
            {                
                var item = inventory[i];
                if (item == null)
                    continue;

                if (item.modItem == null)
                    continue;

                if (!(item.modItem is DragonBallItem))
                    continue;

                var dbItem = item.modItem as DragonBallItem;
                if (dbItem.WhichDragonBall == whichDragonball && dbItem.WorldDragonBallKey == GetWorld().WorldDragonBallKey)
                    return true;                
            }
            return false;
        }

        public static bool IsDragonBallInventoried(int whichDragonball)
        {
            for (var i = 0; i < Main.chest.Length; i++)
            {
                if (Main.chest[i] == null)
                    continue;
                var inventory = Main.chest[i].item;
                if (InventoryContainsDragonball(whichDragonball, inventory))
                    return true;
            }
            return false;
        }

        public static bool TryPlacingDragonball(int whichDragonball, int offsetX, int offsetY, Player ignorePlayer = null)
        {
            // dragon ball already exists, bail out.
            if (IsExistingDragonBall(whichDragonball))
                return true;

            // dragon ball is in a connected player's inventory. This isn't fool proof.
            if (IsDragonBallWithPlayers(whichDragonball, ignorePlayer))
                return true;

            // dragon ball is in a chest somewhere. This also isn't fool proof.
            if (IsDragonBallInventoried(whichDragonball))
                return true;

            TileObject toBePlaced;
            if (TileObject.CanPlace(offsetX, offsetY, DBZMOD.instance.TileType(GetDragonBallTileTypeFromNumber(whichDragonball)), 0, -1, out toBePlaced, true, false))
            {
                WorldGen.PlaceObject(offsetX, offsetY, DBZMOD.instance.TileType(GetDragonBallTileTypeFromNumber(whichDragonball)), true);
                int dbIndex = whichDragonball - 1;
                GetWorld().DragonBallLocations[dbIndex] = new Point(offsetX, offsetY);
                return true;
            } else
            {
                return false;
            }            
        }

        public static bool AttemptToPlaceDragonballsInWorld(GenerationProgress progress = null)
        {
            string PlacingDragonballs = "Placing Dragon Balls";
            if (progress != null)
            {
                progress.Message = PlacingDragonballs;
                progress.Set(0.25f);
            }

            for (int i = 0; i <= 6; i++)
            {
                bool shouldTryToSpawn = !IsExistingDragonBall(i + 1);

                if (shouldTryToSpawn)
                {
                    Point safeCoordinates = GetSafeDragonBallCoordinates();
                    while (!TryPlacingDragonball(i + 1, safeCoordinates.X, safeCoordinates.Y))
                    {
                        safeCoordinates = GetSafeDragonBallCoordinates();
                    }
                }
            }
            return true;
        }

        public static void DoDragonBallCleanupCheck(Player ignorePlayer = null)
        {
            // only fire this server side or single player.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            // loop over the saved locations for each dragonball
            for (var i = 0; i < GetWorld().DragonBallLocations.Length; i++)
            {
                bool shouldTryToSpawn = !IsExistingDragonBall(i + 1);                

                if (shouldTryToSpawn)
                {
                    Point safeCoordinates = GetSafeDragonBallCoordinates();
                    while (!TryPlacingDragonball(i + 1, safeCoordinates.X, safeCoordinates.Y, ignorePlayer))
                    {
                        safeCoordinates = GetSafeDragonBallCoordinates();
                    }
                }
            }
        }

        public static bool IsInvalidTileForDragonBallPlacement(Tile tile)
        {
            return tile.type == TileID.SmallPiles
                || tile.type == TileID.LargePiles
                || tile.type == TileID.LargePiles2
                || tile.type == TileID.Containers
                || tile.type == TileID.Containers2
                || tile.type == TileID.FakeContainers
                || tile.type == TileID.FakeContainers;
        }

        public static Point GetSafeDragonBallCoordinates()
        {
            var underworldHeight = Main.maxTilesY - 220;
            
            var surfaceHeight = (int)Math.Floor(Main.worldSurface * 0.30f);

            bool IsSafeTile = false;
            while (!IsSafeTile)
            {
                int thresholdX = (int)Math.Floor(Main.maxTilesX * 0.1f);
                var randX = Main.rand.Next(thresholdX, Main.maxTilesX - thresholdX);
                var randY = Main.rand.Next(surfaceHeight, underworldHeight);
                var tile = Framing.GetTileSafely(randX, randY);
                if (IsInvalidTileForDragonBallPlacement(tile))
                {
                    continue;
                }
                var tileAbove = Framing.GetTileSafely(randX, randY - 1);
                if (tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && !tileAbove.active())
                {
                    IsSafeTile = true;
                    return new Point(randX, randY - 1);
                }
            }
            
            // made up stuff.
            return new Point(-1, -1);
        }

        public static void SyncWorldDragonBallKey(Player player)
        {
            NetworkHelper.playerSync.RequestServerSendDragonBallKey(256, player.whoAmI);
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
