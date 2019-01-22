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
using DBZMOD.Util;

namespace DBZMOD
{
    public class DBZWorld : ModWorld
    {
        // initialize dragon ball locations to an empty set of points. These get loaded in the world data load segment.
        private Point[] _dragonBallLocations = new Point[7] { new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1, -1) };
        public List<Vector2> kiBeacons = new List<Vector2>();
        // the world dragon ball key is a special random integer that gets created with the world. When the player takes a dragon ball in tile form, the item created
        // has its key set. Taking that ball to another world results in it transforming into something that sucks and doesn't work and isn't a dragon ball.        
        public int worldDragonBallKey = 0;

        // helper utility method for snagging the currently loaded world.
        public static DBZWorld GetWorld()
        {
            return DBZMOD.instance.GetModWorld("DBZWorld") as DBZWorld;
        }

        public void SetDragonBallLocation(int whichDragonBall, Point point, bool isMarkedDirty)
        {
            _dragonBallLocations[whichDragonBall - 1] = point;

            if (isMarkedDirty && Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (point.X == -1 && point.Y == -1)
                {
                    NetworkHelper.playerSync.SendDragonBallRemove(256, Main.myPlayer, whichDragonBall);
                }
                else
                {
                    NetworkHelper.playerSync.SendDragonBallAdd(256, Main.myPlayer, point, whichDragonBall);
                }
            }
        }

        public void SetDragonBallLocation(int whichDragonBall, int x, int y, bool isMarkedDirty)
        {
            var dragonBallCoordinates = new Point(x, y);
            SetDragonBallLocation(whichDragonBall, dragonBallCoordinates, isMarkedDirty);
        }

        public void RemoveDragonBallLocation(int whichDragonBall, bool isMarkedDirty)
        {
            SetDragonBallLocation(whichDragonBall, -1, -1, isMarkedDirty);
        }

        public Point GetDragonBallLocation(int whichDragonBall)
        {
            return _dragonBallLocations[whichDragonBall - 1];
        }

        public override TagCompound Save()
        {
            var dbtWorldTag = new TagCompound();
            dbtWorldTag.Add("WorldDragonBallKey", worldDragonBallKey);
            dbtWorldTag.Add("OneStarDragonBallX", _dragonBallLocations[0].X);
            dbtWorldTag.Add("OneStarDragonBallY", _dragonBallLocations[0].Y);
            dbtWorldTag.Add("TwoStarDragonBallX", _dragonBallLocations[1].X);
            dbtWorldTag.Add("TwoStarDragonBallY", _dragonBallLocations[1].Y);
            dbtWorldTag.Add("ThreeStarDragonBallX", _dragonBallLocations[2].X);
            dbtWorldTag.Add("ThreeStarDragonBallY", _dragonBallLocations[2].Y);
            dbtWorldTag.Add("FourStarDragonBallX", _dragonBallLocations[3].X);
            dbtWorldTag.Add("FourStarDragonBallY", _dragonBallLocations[3].Y);
            dbtWorldTag.Add("FiveStarDragonBallX", _dragonBallLocations[4].X);
            dbtWorldTag.Add("FiveStarDragonBallY", _dragonBallLocations[4].Y);
            dbtWorldTag.Add("SixStarDragonBallX", _dragonBallLocations[5].X);
            dbtWorldTag.Add("SixStarDragonBallY", _dragonBallLocations[5].Y);
            dbtWorldTag.Add("SevenStarDragonBallX", _dragonBallLocations[6].X);
            dbtWorldTag.Add("SevenStarDragonBallY", _dragonBallLocations[6].Y);
            dbtWorldTag.Add("KiBeacons", kiBeacons);
            dbtWorldTag.Add("HasDoneSavageCleanup", !shouldDoBrutalCleanup);
            return dbtWorldTag;
        }

        public override void Load(TagCompound tag)
        {
            var dbKey = tag.GetInt("WorldDragonBallKey");
            worldDragonBallKey = dbKey;
            var oneStarPoint = new Point(tag.GetInt("OneStarDragonBallX"), tag.GetInt("OneStarDragonBallY"));
            var twoStarPoint = new Point(tag.GetInt("TwoStarDragonBallX"), tag.GetInt("TwoStarDragonBallY"));
            var threeStarPoint = new Point(tag.GetInt("ThreeStarDragonBallX"), tag.GetInt("ThreeStarDragonBallY"));
            var fourStarPoint = new Point(tag.GetInt("FourStarDragonBallX"), tag.GetInt("FourStarDragonBallY"));
            var fiveStarPoint = new Point(tag.GetInt("FiveStarDragonBallX"), tag.GetInt("FiveStarDragonBallY"));
            var sixStarPoint = new Point(tag.GetInt("SixStarDragonBallX"), tag.GetInt("SixStarDragonBallY"));
            var sevenStarPoint = new Point(tag.GetInt("SevenStarDragonBallX"), tag.GetInt("SevenStarDragonBallY"));
            SetDragonBallLocation(1, oneStarPoint, false);
            SetDragonBallLocation(2, twoStarPoint, false);
            SetDragonBallLocation(3, threeStarPoint, false);
            SetDragonBallLocation(4, fourStarPoint, false);
            SetDragonBallLocation(5, fiveStarPoint, false);
            SetDragonBallLocation(6, sixStarPoint, false);
            SetDragonBallLocation(7, sevenStarPoint, false);
            if (tag.ContainsKey("HasDoneSavageCleanup"))
            {
                shouldDoBrutalCleanup = !tag.GetBool("HasDoneSavageCleanup");
            }
            kiBeacons = tag.ContainsKey("KiBeacons") ? (List<Vector2>)tag.GetList<Vector2>("KiBeacons") : new List<Vector2>();

            // cleanup ki beacon list, not sure why this is necessary.
            CleanupKiBeaconList();

            base.Load(tag);
        }

        public static void DestroyAndRespawnDragonBalls()
        {
            DebugUtil.Log("Despawning dragon balls");
            var dbzWorld = GetWorld();

            for (var i = 1; i <= 7; i++)
            {
                var location = dbzWorld.GetDragonBallLocation(i);
                DebugUtil.Log(string.Format("Despawning dragon ball at {0} {1}", location.X, location.Y));
                WorldGen.KillTile(location.X, location.Y, false, false, true);
            }

            // handles respawning all the dragon balls
            DoDragonBallCleanupCheck();
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
            MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>(mod);
            modPlayer.kiLantern = false;
            modPlayer.oneStarDbNearby = false;
            modPlayer.twoStarDbNearby = false;
            modPlayer.threeStarDbNearby = false;
            modPlayer.fourStarDbNearby = false;
            modPlayer.fiveStarDbNearby = false;
            modPlayer.sixStarDbNearby = false;
            modPlayer.sevenStarDbNearby = false;
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

            tasks.Insert(tasks.Count - 1, new PassLegacy("[DBZMOD] Placing dragon balls", PlaceDragonballs));
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
                bool success = RunGohanCleanupRoutine(progress);
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

        public void PlaceDragonballs(GenerationProgress progress = null)
        {
            try
            {
                bool success = AttemptToPlaceDragonballsInWorld(progress);
                if (success)
                {
                    isDragonBallPlacementDone = true;
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
            string gohanHouseGen = "Creating the house of a legend.";
            if (progress != null)
            {
                progress.Message = gohanHouseGen;
                progress.Set(0.25f);
            }

            // before we do anything, create a new World Key for this world
            worldDragonBallKey = Main.rand.Next(1, int.MaxValue);

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
                    int offsetX = gohanHouseStartPositionX + x;
                    int offsetY = gohanHouseStartPositionY + y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    switch (_gohanHouseTiles[y, x])
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
            for (var x = 0; x < _gohanHouseSlopes.GetLength(1); x++)
            {
                for (var y = 0; y < _gohanHouseSlopes.GetLength(0); y++)
                {
                    int offsetX = gohanHouseStartPositionX + x;
                    int offsetY = gohanHouseStartPositionY + y;
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
                    int offsetX = gohanHouseStartPositionX + x;
                    int offsetY = gohanHouseStartPositionY + y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    switch (_gohanHouseWalls[y, x])
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
            for (var x = 0; x < _gohanHouseObjects.GetLength(1); x++)
            {
                // house objects are different.. they go in reverse (ground up) so that the bottle placement actually works.
                for (var y = _gohanHouseObjects.GetLength(0) - 1; y >= 0; y--)
                {
                    int offsetX = gohanHouseStartPositionX + x;
                    int offsetY = gohanHouseStartPositionY + y;
                    var tile = Framing.GetTileSafely(offsetX, offsetY);
                    // break rocks!
                    if (tile.type == TileID.SmallPiles || tile.type == TileID.LargePiles || tile.type == TileID.LargePiles2 || tile.type == TileID.Dirt || tile.type == TileID.Stone)
                    {
                        // nullify tiles?
                        WorldGen.KillTile(offsetX, offsetY);
                        tile = Framing.GetTileSafely(offsetX, offsetY);
                    }
                    switch (_gohanHouseObjects[y, x])
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
                            // WorldGen.PlaceObject(offsetX, offsetY, TileID.Dressers, true, 4); // confirmed shadewood dresser
                            WorldGen.PlaceChest(offsetX, offsetY, TileID.Dressers, false, 4);
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
            var sampleTile = Framing.GetTileSafely(gohanHouseStartPositionX, gohanHouseStartPositionY + 1);
            bool isSnowBiome = false;
            if (sampleTile.type == TileID.SnowBlock || sampleTile.type == TileID.IceBlock)
                isSnowBiome = true;


            // experimental, also doesn't work when the tiles below are snow... which happens at spawn sometimes.
            // put dirt under the house and make sure gaps are filled. this might look weird.
            for (var y = 0; y < 5; y++)
            {
                for (var x = -1 - (y * 2); x < _gohanHouseTiles.GetLength(1) + 1 + (y * 2); x++)
                {
                    int offsetX = gohanHouseStartPositionX + x;
                    int offsetY = gohanHouseStartPositionY + _gohanHouseTiles.GetLength(0) + y;
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
            string gohanHouseGen = "Cleaning up Grandpa's House...";
            if (progress != null)
            {
                progress.Message = gohanHouseGen;
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
            var coordinates = GetWorld()._dragonBallLocations[dbIndex];
            if (coordinates.Equals(new Point(-1, -1)))
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
                // assume this runs before the inventory loop that turns stone balls into real ones - we don't care which it is, if it's legit, we don't want to spawn dragonballs replacing it.
                if (dbItem.item.type == ItemHelper.GetItemTypeFromName(DragonBallItem.GetStoneBallFromNumber(whichDragonball)) && dbItem.worldDragonBallKey == GetWorld().worldDragonBallKey)
                    return true;
                if (dbItem.item.type == ItemHelper.GetItemTypeFromName(DragonBallItem.GetDragonBallItemTypeFromNumber(whichDragonball)) && dbItem.worldDragonBallKey == GetWorld().worldDragonBallKey)
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
                GetWorld().SetDragonBallLocation(whichDragonball, new Point(offsetX, offsetY), true);
                return true;
            } else
            {
                return false;
            }            
        }

        public static bool AttemptToPlaceDragonballsInWorld(GenerationProgress progress = null)
        {
            string placingDragonballs = "Placing Dragon Balls";
            if (progress != null)
            {
                progress.Message = placingDragonballs;
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

        private bool _kiBeaconCleanupCheck = false;
        public override void PreUpdate()
        {
            if (!_kiBeaconCleanupCheck)
            {
                _kiBeaconCleanupCheck = true;
            }
        }

        public bool shouldDoBrutalCleanup = false;
        public static void DoDragonBallCleanupCheck(Player ignorePlayer = null)
        {
            // only fire this server side or single player.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (GetWorld().shouldDoBrutalCleanup)
            {
                DoBrutalCleanup();
            }

            // loop over the saved locations for each dragonball
            for (var i = 0; i < GetWorld()._dragonBallLocations.Length; i++)
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

        public static void DoBrutalCleanup()
        {
            int invalidCount = 0;
            for (var i = 0; i < Main.maxTilesX; i++)
            {
                for (var j = 0; j < Main.maxTilesY; j++)
                {
                    var tile = Framing.GetTileSafely(i, j);
                    if (tile == null)
                        continue;
                    var dbNum = GetDragonBallNumberFromType(tile.type);
                    if (dbNum == 0)
                        continue;
                    var existingLoc = GetWorld().GetDragonBallLocation(dbNum);
                    if (existingLoc.X == -1 && existingLoc.Y == -1)
                    {
                        invalidCount++;
                        WorldGen.KillTile(i, j, false, false, true);
                    } else if (existingLoc.X != i || existingLoc.Y != j)
                    {
                        invalidCount++;
                        WorldGen.KillTile(i, j, false, false, true);
                    }
                }
            }
            if (invalidCount > 0)
            {
                Main.NewText("Found " + invalidCount + " bad Dragon Balls. Sorry for the trouble. Should be cleaned up!");
            }
            GetWorld().shouldDoBrutalCleanup = false;
        }

        public int? db1;
        public int Db1Type
        {
            get
            {
                if (db1 == null)
                {
                    db1 = DBZMOD.instance.TileType("OneStarDBTile");
                }
                return db1.Value;
            }
        }

        public int? db2;
        public int Db2Type
        {
            get
            {
                if (db2 == null)
                {
                    db2 = DBZMOD.instance.TileType("OneStarDBTile");
                }
                return db2.Value;
            }
        }

        public int? db3;
        public int Db3Type
        {
            get
            {
                if (db3 == null)
                {
                    db3 = DBZMOD.instance.TileType("OneStarDBTile");
                }
                return db3.Value;
            }
        }

        public int? db4;
        public int Db4Type
        {
            get
            {
                if (db4 == null)
                {
                    db4 = DBZMOD.instance.TileType("OneStarDBTile");
                }
                return db4.Value;
            }
        }

        public int? db5;
        public int Db5Type
        {
            get
            {
                if (db5 == null)
                {
                    db5 = DBZMOD.instance.TileType("OneStarDBTile");
                }
                return db5.Value;
            }
        }

        public int? db6;
        public int Db6Type
        {
            get
            {
                if (db6 == null)
                {
                    db6 = DBZMOD.instance.TileType("OneStarDBTile");
                }
                return db6.Value;
            }
        }

        public int? db7;
        public int Db7Type
        {
            get
            {
                if (db7 == null)
                {
                    db7 = DBZMOD.instance.TileType("OneStarDBTile");
                }
                return db7.Value;
            }
        }

        public static int GetDragonBallNumberFromType(int type)
        {
            if (type == GetWorld().Db1Type)
                return 1;
            if (type == GetWorld().Db2Type)
                return 2;
            if (type == GetWorld().Db3Type)
                return 3;
            if (type == GetWorld().Db4Type)
                return 4;
            if (type == GetWorld().Db5Type)
                return 5;
            if (type == GetWorld().Db6Type)
                return 6;
            if (type == GetWorld().Db7Type)
                return 7;
            return 0;
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

            bool isSafeTile = false;
            while (!isSafeTile)
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
                    isSafeTile = true;
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
