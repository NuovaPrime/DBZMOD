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
using DBZMOD.Network;
using DBZMOD;
using DBZMOD.Util;

namespace DBZMOD
{
    public class DBZWorld : ModWorld
    {
        // initialize dragon ball locations to an empty set of points. These get loaded in the world data load segment.        
        public List<Vector2> kiBeacons = new List<Vector2>();

        // helper utility method for snagging the currently loaded world.
        public static DBZWorld GetWorld()
        {
            return DBZMOD.Instance.GetModWorld("DBZWorld") as DBZWorld;
        }

        public override TagCompound Save()
        {
            var dbtWorldTag = new TagCompound
            {
                {"KiBeacons", kiBeacons}
            };
            for(var i = 0; i < 7; i++)
            {
                var dbCache = GetCachedDragonBallLocation(i + 1);
                var cacheKeyNameX = $"DragonBall{i + 1}LocationX";
                var cacheKeyNameY = $"DragonBall{i + 1}LocationY";
                dbtWorldTag.Add(cacheKeyNameX, dbCache.X);
                dbtWorldTag.Add(cacheKeyNameY, dbCache.Y);
            }
                        
            return dbtWorldTag;
        }

        public override void Load(TagCompound tag)
        {
            kiBeacons = tag.ContainsKey("KiBeacons") ? (List<Vector2>)tag.GetList<Vector2>("KiBeacons") : new List<Vector2>();

            // cleanup ki beacon list, not sure why this is necessary.
            CleanupKiBeaconList();

            for (var i = 0; i < 7; i++)
            {
                var cacheKeyNameX = $"DragonBall{i + 1}LocationX";
                var cacheKeyNameY = $"DragonBall{i + 1}LocationY";
                if (tag.ContainsKey(cacheKeyNameX) && tag.ContainsKey(cacheKeyNameY))
                {
                    var dbX = tag.GetInt(cacheKeyNameX);
                    var dbY = tag.GetInt(cacheKeyNameY);
                    var dbLocation = new Point(dbX, dbY);
                    CacheDragonBallLocation(i + 1, dbLocation);
                }
            }

            base.Load(tag);
        }

        // handle retrograde cleanup immediately after the first update tick.
        private bool _initialized;
        public override void PostUpdate()
        {
            if (!_initialized)
            {
                _initialized = true;
                HandleRetrogradeCleanup();
            }
            else
            {
                // every 10 seconds, check dragon ball locations, but don't run the full cleanup detail.
                if (Main.netMode != NetmodeID.MultiplayerClient && DBZMOD.IsTickRateElapsed(600))
                    CleanupAndRegenerateDragonBalls(false);
            }

        }

        public void CleanupKiBeaconList()
        {
            var listToRemove = new List<Vector2>();
            foreach (var location in kiBeacons)
            {
                var tile = Framing.GetTileSafely((int)location.X / 16, (int)location.Y / 16);
                if (tile.type == mod.TileType("KiBeaconTile"))
                    continue;
                listToRemove.Add(location);
            }

            kiBeacons = kiBeacons.Except(listToRemove).ToList();
        }

        public override void PostWorldGen()
        {
            base.PostWorldGen();
            _generateGohanHouse = false;
            _isGohanHouseCleaned = false;
            isDragonBallPlacementDone = false;
            isGohanHouseOffsetSet = false;
        }

        private bool _generateGohanHouse = false;
        private bool _isGohanHouseCleaned = false;
        public bool isDragonBallPlacementDone = false;
        public int gohanHouseStartPositionX = 0;
        public int gohanHouseStartPositionY = 0;
        public override void ResetNearbyTileEffects()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
            modPlayer.kiLantern = false;
        }

        #region Gohan House Bytes

        //18 tiles long, 16 tiles high (with dirt on the bottom)

        //0 = air, 1 = grey stucco, 2 = blue dynasty shingles, 3 = Smooth marble, 4 = dynasty wood, 5 = dirt
        private static readonly byte[,] _gohanHouseTiles =
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
        private static readonly byte[,] _gohanHouseWalls =
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
        private static readonly byte[,] _gohanHouseSlopes =
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
        private static readonly byte[,] _gohanHouseObjects =
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

            tasks.Insert(tasks.Count - 1, new PassLegacy("[DBZMOD] Placing dragon balls", PlaceDragonBalls));
        }

        public void AddGohanHouse(GenerationProgress progress = null)
        {
            if (_generateGohanHouse)
                return;

            try
            {
                bool success = MakeGohanHouse(progress);
                if (success)
                {
                    _generateGohanHouse = true;
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
            if (!_generateGohanHouse)
                return;

            // bail if already done.
            if (_isGohanHouseCleaned)
                return;

            try
            {
                var success = RunGohanCleanupRoutine(progress);
                if (success)
                {
                    _isGohanHouseCleaned = true;
                }
            }
            catch (Exception exception)
            {
                Main.NewText("Oh no, an error happened [CleanupGohanHouse]! Report this to NuovaPrime or MercuriusXeno and send them the file Terraria/ModLoader/Logs/Logs.txt");
                ErrorLogger.Log(exception);
            }
        }

        public void PlaceDragonBalls(GenerationProgress progress = null)
        {
            try
            {
                var success = AttemptToPlaceDragonBallsInWorld(progress);
                if (success)
                {
                    isDragonBallPlacementDone = true;
                }
            }
            catch (Exception exception)
            {
                Main.NewText("Oh no, an error happened [PlacingDragonBalls]! Report this to NuovaPrime or MercuriusXeno and send them the file Terraria/ModLoader/Logs/Logs.txt");
                ErrorLogger.Log(exception);
            }
        }

        bool MakeGohanHouse(GenerationProgress progress)
        {            
            string gohanHouseGen = "Creating the house of a legend.";
            if (progress != null)
            {
                progress.Message = gohanHouseGen;
                progress.Set(0.25f);
            }

            gohanHouseStartPositionX = WorldGen.genRand.Next(Main.maxTilesX / 2 - 70, Main.spawnTileX - 25);
            for (var attempts = 0; attempts < 10000; attempts++)
            {
                for (var i = 0; i < 25; i++)
                {
                    gohanHouseStartPositionY = 190;
                    do
                    {
                        gohanHouseStartPositionY++;
                    }
                    while ((!Main.tile[gohanHouseStartPositionX + i, gohanHouseStartPositionY].active() && gohanHouseStartPositionY < Main.worldSurface) || Main.tile[gohanHouseStartPositionX + i, gohanHouseStartPositionY].type == TileID.Trees || Main.tile[gohanHouseStartPositionX + i, gohanHouseStartPositionY].type == 27);
                    if (!Main.tile[gohanHouseStartPositionX, gohanHouseStartPositionY].active() || Main.tile[gohanHouseStartPositionX, gohanHouseStartPositionY].liquid > 0)
                    {
                        gohanHouseStartPositionX++;
                    }
                    if (Main.tile[gohanHouseStartPositionX + i, gohanHouseStartPositionY].active())
                    {
                        if (Main.tile[gohanHouseStartPositionX, gohanHouseStartPositionY].liquid > 0) { 
                            gohanHouseStartPositionX = WorldGen.genRand.Next(Main.maxTilesX / 2 - 70, Main.spawnTileX - 25);
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
        public bool isGohanHouseOffsetSet = false;
        public void GenerateGohanStructureWithByteArrays(bool isFirstPass)
        {
            if (!isGohanHouseOffsetSet)
            {
                gohanHouseStartPositionY -= _gohanHouseTiles.GetLength(0);
                isGohanHouseOffsetSet = true;
            }

            // if we're here it means we are ready to generate our structure

            // tiles
            for (var x = 0; x < _gohanHouseTiles.GetLength(1); x++)
            {
                for (var y = 0; y < _gohanHouseTiles.GetLength(0); y++)
                {
                    var offsetX = gohanHouseStartPositionX + x;
                    var offsetY = gohanHouseStartPositionY + y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    if (_gohanHouseTiles[y, x] == 0)
                    {
                        if (!isFirstPass) continue;
                        tile.type = 0;
                        tile.active(false);
                    }
                    else if (_gohanHouseTiles[y, x] == 1)
                    {
                        tile.type = TileID.GrayStucco;
                        tile.active(true);
                    }
                    else if (_gohanHouseTiles[y, x] == 2)
                    {
                        tile.type = TileID.BlueDynastyShingles;
                        tile.active(true);
                    }
                    else if (_gohanHouseTiles[y, x] == 3)
                    {
                        tile.type = TileID.MarbleBlock;
                        tile.active(true);
                    }
                    else if (_gohanHouseTiles[y, x] == 4)
                    {
                        tile.type = TileID.DynastyWood;
                        tile.active(true);
                    }
                    else if (_gohanHouseTiles[y, x] == 5)
                    {
                        tile.type = TileID.Grass;
                        tile.active(true);
                    }
                }
            }
            for (var x = 0; x < _gohanHouseSlopes.GetLength(1); x++)
            {
                for (var y = 0; y < _gohanHouseSlopes.GetLength(0); y++)
                {
                    var offsetX = gohanHouseStartPositionX + x;
                    var offsetY = gohanHouseStartPositionY + y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    tile.slope(_gohanHouseSlopes[y, x]);
                    tile.halfBrick(false);
                }
            }
            // walls
            for (var x = 0; x < _gohanHouseWalls.GetLength(1); x++)
            {
                for (var y = 0; y < _gohanHouseWalls.GetLength(0); y++)
                {
                    var offsetX = gohanHouseStartPositionX + x;
                    var offsetY = gohanHouseStartPositionY + y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    if (_gohanHouseWalls[y, x] == 0)
                        tile.wall = 0;
                    else if (_gohanHouseWalls[y, x] == 1)
                        tile.wall = WallID.Wood;
                    else if (_gohanHouseWalls[y, x] == 2)
                        tile.wall = WallID.LivingWood;
                    else if (_gohanHouseWalls[y, x] == 3)
                        tile.wall = WallID.Gray;
                    else if (_gohanHouseWalls[y, x] == 4) tile.wall = WallID.Glass;
                }
            }
            // Objects
            for (var x = 0; x < _gohanHouseObjects.GetLength(1); x++)
            {
                // house objects are different.. they go in reverse (ground up) so that the bottle placement actually works.
                for (var y = _gohanHouseObjects.GetLength(0) - 1; y >= 0; y--)
                {
                    var offsetX = gohanHouseStartPositionX + x;
                    var offsetY = gohanHouseStartPositionY + y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    // break rocks!
                    if (tile.type == TileID.SmallPiles || tile.type == TileID.LargePiles || tile.type == TileID.LargePiles2 || tile.type == TileID.Dirt || tile.type == TileID.Stone)
                    {
                        // nullify tiles?
                        WorldGen.KillTile(offsetX, offsetY);
                        tile = Framing.GetTileSafely(offsetX, offsetY);
                    }

                    if (_gohanHouseObjects[y, x] == 0)
                    {
                    }
                    else if (_gohanHouseObjects[y, x] == 1)
                    {
                        WorldGen.PlaceObject(offsetX, offsetY, TileID.ClosedDoor, true, 28); // confirmed dynasty door
                    }
                    else if (_gohanHouseObjects[y, x] == 2)
                    {
                        WorldGen.PlaceObject(offsetX, offsetY, TileID.Tables, true, 25); // confirmed dynasty table
                    }
                    else if (_gohanHouseObjects[y, x] == 3)
                    {
                        WorldGen.PlaceObject(offsetX, offsetY, TileID.Bottles, true, 5); // confirmed dynasty cup
                    }
                    else if (_gohanHouseObjects[y, x] == 4)
                    {
                        WorldGen.PlaceObject(offsetX, offsetY, TileID.Chandeliers, true,
                            22); // confirmed large dynasty lantern
                        tile.color(28);
                    }
                    else if (_gohanHouseObjects[y, x] == 5)
                    {
                        WorldGen.PlaceObject(offsetX, offsetY, TileID.HangingLanterns, true,
                            26); // confirmed dynasty hanging lantern (small one)
                    }
                    else if (_gohanHouseObjects[y, x] == 6)
                    {
                        // WorldGen.PlaceObject(offsetX, offsetY, TileID.Dressers, true, 4); // confirmed shadewood dresser
                        WorldGen.PlaceChest(offsetX, offsetY, TileID.Dressers, false, 4);
                    }
                    else if (_gohanHouseObjects[y, x] == 7)
                    {
                        if (!isFirstPass)
                        {
                            TryPlacingDragonBall(4, offsetX, offsetY);
                        }
                    }
                }
            }

            // sample tiles at the origin (it's to the right, this isn't perfect)
            var sampleTile = Framing.GetTileSafely(gohanHouseStartPositionX, gohanHouseStartPositionY + 1);
            var isSnowBiome = sampleTile.type == TileID.SnowBlock || sampleTile.type == TileID.IceBlock;


            // experimental, also doesn't work when the tiles below are snow... which happens at spawn sometimes.
            // put dirt under the house and make sure gaps are filled. this might look weird.
            for (var y = 0; y < 5; y++)
            {
                for (var x = -1 - (y * 2); x < _gohanHouseTiles.GetLength(1) + 1 + (y * 2); x++)
                {
                    var offsetX = gohanHouseStartPositionX + x;
                    var offsetY = gohanHouseStartPositionY + _gohanHouseTiles.GetLength(0) + y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    var isEdge = IsAnySideExposed(offsetX, offsetY);
                    tile.type = isSnowBiome ? TileID.SnowBlock : (isEdge ? TileID.Grass : TileID.Dirt);
                    // if it's a slope, remove slope. quit putting gaps in the ground terraria.
                    tile.slope(0);
                    tile.halfBrick(false);
                    tile.active(true);
                }
            }
        }

        private bool RunGohanCleanupRoutine(GenerationProgress progress)
        {
            // we already have the starting position, just cut straight to the build cleanup.
            const string gohanHouseGen = "Cleaning up Grandpa's House...";
            if (progress != null)
            {
                progress.Message = gohanHouseGen;
                progress.Set(0.50f);
            }

            GenerateGohanStructureWithByteArrays(false);
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

        public bool TryPlacingDragonBall(int whichDragonBall, int offsetX, int offsetY)
        {            
            // dragon ball already exists, bail out.
            if (IsExistingDragonBall(whichDragonBall))
                return true;
            TileObject tileObjectOut;
            int dragonBallType = GetDbType(whichDragonBall);
            if (!TileObject.CanPlace(offsetX, offsetY, dragonBallType, 0, -1, out tileObjectOut, true,
                false)) return false;

            CacheDragonBallLocation(whichDragonBall, new Point(offsetX, offsetY));

            WorldGen.PlaceObject(offsetX, offsetY, dragonBallType, true);
            NetMessage.SendTileSquare(-1, offsetX, offsetY, 1, TileChangeType.None);
            if (Main.netMode == NetmodeID.Server)
            {
                NetworkHelper.playerSync.SendAllDragonBallLocations();
            }

            return true;
        }

        public bool AttemptToPlaceDragonBallsInWorld(GenerationProgress progress = null)
        {
            const string placingDragonBalls = "Placing Dragon Balls";
            if (progress != null)
            {
                progress.Message = placingDragonBalls;
                progress.Set(0.25f);
            }

            for (var i = 0; i < 7; i++)
            {
                TryPlacingDragonBall(i + 1);
            }
            return true;
        }

        public void TryPlacingDragonBall(int whichDragonBall)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            var isAttemptingToSpawnDragonBall = !IsExistingDragonBall(whichDragonBall);

            if (!isAttemptingToSpawnDragonBall) return;
            var safeCoordinates = GetSafeDragonBallCoordinates();
            while (!TryPlacingDragonBall(whichDragonBall, safeCoordinates.X, safeCoordinates.Y))
            {
                safeCoordinates = GetSafeDragonBallCoordinates();
            }
        }

        private bool _kiBeaconCleanupCheck = false;
        public override void PreUpdate()
        {
            if (!_kiBeaconCleanupCheck)
            {
                _kiBeaconCleanupCheck = true;
            }
        }

        public void HandleRetrogradeCleanup(Player ignorePlayer = null)
        {
            // only fire this server side or single player.
            if (Main.netMode != NetmodeID.MultiplayerClient)
                CleanupAndRegenerateDragonBalls(true);
        }

        public void CleanupAndRegenerateDragonBalls(bool isCleanupNeeded)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            Point[] dragonBallFirstFound = Enumerable.Repeat(Point.Zero, 7).ToArray();

            if (isCleanupNeeded)
            {
                DebugHelper.Log("Server is running cleanup routine.");
                // destroy all but the first located dragon ball of any given type in the world, with no items.
                for (var i = 0; i < Main.maxTilesX; i++)
                {
                    for (var j = 0; j < Main.maxTilesY; j++)
                    {
                        var tile = Main.tile[i, j];
                        // var tile = Framing.GetTileSafely(i, j);
                        if (tile == null)
                            continue;
                        if (!tile.active())
                            continue;
                        var dbNum = GetDragonBallNumberFromType(tile.type);
                        if (dbNum == 0)
                            continue;
                        var thisDragonBallLocation = new Point(i, j);
                        if (dragonBallFirstFound[dbNum - 1].Equals(Point.Zero))
                        {
                            dragonBallFirstFound[dbNum - 1] = thisDragonBallLocation;
                        }
                        else
                        {
                            WorldGen.KillTile(i, j, false, false, true);
                        }
                    }
                }
            }
            else
            {
                dragonBallFirstFound = CachedDragonBallLocations;
                DebugHelper.Log("Server is running dragon ball confirmation routine.");
            }

            // figure out if new dragon balls need to be spawned (are any missing?)
            for (var i = 0; i < dragonBallFirstFound.Length; i++)
            {
                // check that the found locations are still valid
                Point testLocation = dragonBallFirstFound[i];
                if (!IsDragonBallLocation(testLocation.X, testLocation.Y))
                {
                    DebugHelper.Log($"Server thinks dragon ball {i + 1} is missing.");
                    // if this isn't a dragon ball, erase it.
                    dragonBallFirstFound[i] = Point.Zero;
                    CacheDragonBallLocation(i + 1, Point.Zero);
                }
                
                if (dragonBallFirstFound[i].Equals(Point.Zero))
                {
                    TryPlacingDragonBall(i + 1);
                }
            }
        }

        public bool IsExistingDragonBall(int whichDragonBall)
        {
            var existingLocation = GetCachedDragonBallLocation(whichDragonBall);
            if (existingLocation.Equals(Point.Zero))
                return false;
            return true;
        }

        public Point[] CachedDragonBallLocations { get; } = Enumerable.Repeat(Point.Zero, 7).ToArray();

        public Point GetCachedDragonBallLocation(int whichDragonBall)
        {
            return CachedDragonBallLocations[whichDragonBall - 1];
        }

        public void CacheDragonBallLocation(int whichDragonBall, Point location)
        {
            CachedDragonBallLocations[whichDragonBall - 1] = location;
        }

        public bool IsDragonBallLocation(int i, int j)
        {
            var tile = Main.tile[i, j];
            if (tile == null)
                return false;
            if (!tile.active())
                return false;
            for (var d = 0; d < 7; d++)
            {
                bool isDragonBall = GetDbType(d + 1) == tile.type;
                if (isDragonBall)
                    return true;
            }
            return false;
        }

        public int?[] dbTypes = Enumerable.Repeat((int?)null, 7).ToArray();

        public string GetDragonBallNumberName(int whichDragonBall)
        {
            switch (whichDragonBall)
            {
                case 1:
                    return "One";
                case 2:
                    return "Two";
                case 3:
                    return "Three";
                case 4:
                    return "Four";
                case 5:
                    return "Five";
                case 6:
                    return "Six";
                case 7:
                    return "Seven";
                default:
                    return string.Empty;
            }
        }

        public int GetDbType(int whichDragonBall)
        {
            if (dbTypes[whichDragonBall - 1].HasValue)
                    return dbTypes[whichDragonBall - 1].Value;
            var dragonBallWord = GetDragonBallNumberName(whichDragonBall);
            dbTypes[whichDragonBall - 1] = DBZMOD.Instance?.TileType($"{dragonBallWord}StarDBTile");
            if (dbTypes[whichDragonBall - 1].HasValue)
                return dbTypes[whichDragonBall - 1].Value;
            return 0;
        }

        public int GetDragonBallNumberFromType(int type)
        {
            for (int i = 1; i <= 7; i++)
            {
                if (type == GetDbType(i))
                {
                    return i;
                }
            }

            return 0;
        }

        // the following walls are *natural* [not placed] dungeon walls and the lihzard temple wall, respectively.
        // these prevent the dragon ball from spawning.
        private static readonly int[] _invalidDragonBallWalls = new int[] { 7, 9, 94, 95, 96, 97, 98, 99, 87 };
        public static bool IsInvalidTileForDragonBallPlacement(Tile tile)
        {
            // quick wall check
            if (_invalidDragonBallWalls.Contains(tile.wall))
                return false;
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

            // restrict debug mode dragon ball spawns to the surface, for testing purposes
            if (DebugHelper.IsDebugModeOn())
            {
                underworldHeight = (int)Math.Floor(surfaceHeight / 0.30f);
            }

            while (true)
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
                    return new Point(randX, randY - 1);
                }
            }
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
