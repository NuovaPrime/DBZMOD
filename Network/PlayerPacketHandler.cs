using System.IO;
using DBZMOD.Enums;
using DBZMOD.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Network
{
    internal class PlayerPacketHandler : PacketHandler
    {
        public const byte SYNC_PLAYER = 43;
        public const byte SYNC_TRIGGERS = 44;
        public const byte REQUEST_FOR_SYNC_FROM_JOINED_PLAYER = 45;
        public const byte REQUEST_DRAGON_BALL_KEY_SYNC = 46;
        public const byte REQUEST_TELEPORT_MESSAGE = 47;
        public const byte REQUEST_KI_BEACON_INITIAL_SYNC = 49;
        public const byte SEND_KI_BEACON_INITIAL_SYNC = 50;
        public const byte SYNC_KI_BEACON_ADD = 51;
        public const byte SYNC_KI_BEACON_REMOVE = 52;
        public const byte DESTROY_AND_RESPAWN_DRAGON_BALLS = 53;
        public const byte SYNC_DRAGON_BALL_CHANGE = 54;

        public PlayerPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SYNC_PLAYER):
                    ReceiveChangedPlayerData(reader, fromWho);
                    break;
                case (SYNC_TRIGGERS):
                    ReceiveSyncTriggers(reader, fromWho);
                    break;
                case (SYNC_KI_BEACON_ADD):
                    ReceiveKiBeaconAdd(reader, fromWho);
                    break;
                case (SYNC_KI_BEACON_REMOVE):
                    ReceiveKiBeaconRemove(reader, fromWho);
                    break;
                case (SYNC_DRAGON_BALL_CHANGE):
                    ReceiveDragonBallChange(reader, fromWho);
                    break;
                case (REQUEST_KI_BEACON_INITIAL_SYNC):
                    ReceiveKiBeaconInitialSyncRequest(fromWho);
                    break;
                case (SEND_KI_BEACON_INITIAL_SYNC):
                    ReceiveKiBeaconInitialSync(reader, fromWho);
                    break;
                case (REQUEST_FOR_SYNC_FROM_JOINED_PLAYER):
                    int whichPlayersDataNeedsRelay = reader.ReadInt32();
                    SendPlayerInfoToPlayerFromOtherPlayer(fromWho, whichPlayersDataNeedsRelay);
                    break;
                case (REQUEST_TELEPORT_MESSAGE):
                    ProcessRequestTeleport(reader, fromWho);
                    break;
            }
        }

        public void SendDestroyAndRespawnDragonBalls(int toWho, int fromWho, int initiatingPlayer)
        {
            ModPacket packet = GetPacket(DESTROY_AND_RESPAWN_DRAGON_BALLS, fromWho);
            packet.Write(initiatingPlayer);
            packet.Send(toWho, initiatingPlayer);
        }

        public void SendKiBeaconAdd(int toWho, int fromWho, Vector2 kiBeaconLocation)
        {
            ModPacket packet = GetPacket(SYNC_KI_BEACON_ADD, fromWho);
            packet.Write(kiBeaconLocation.X);
            packet.Write(kiBeaconLocation.Y);
            packet.Send(toWho, fromWho);
        }

        public void SendKiBeaconRemove(int toWho, int fromWho, Vector2 kiBeaconLocation)
        {
            ModPacket packet = GetPacket(SYNC_KI_BEACON_REMOVE, fromWho);
            packet.Write(kiBeaconLocation.X);
            packet.Write(kiBeaconLocation.Y);
            packet.Send(toWho, fromWho);
        }

        // handle a single ki beacon update (including removals)
        public void ReceiveKiBeaconAdd(BinaryReader reader, int fromWho)
        {
            var coordX = reader.ReadSingle();
            var coordY = reader.ReadSingle();
            var location = new Vector2(coordX, coordY);
            var dbWorld = DBZWorld.GetWorld();
            if (!dbWorld.kiBeacons.Contains(location))
                dbWorld.kiBeacons.Add(location);
            if (Main.netMode == NetmodeID.Server)
                SendKiBeaconAdd(-1, fromWho, location);
        }

        // handle a single ki beacon update (including removals)
        public void ReceiveKiBeaconRemove(BinaryReader reader, int fromWho)
        {
            var coordX = reader.ReadSingle();
            var coordY = reader.ReadSingle();
            var location = new Vector2(coordX, coordY);
            var dbWorld = DBZWorld.GetWorld();
            if (dbWorld.kiBeacons.Contains(location))
                dbWorld.kiBeacons.Remove(location);
            if (Main.netMode == NetmodeID.Server)
                SendKiBeaconRemove(-1, fromWho, location);
        }

        public void SendAllDragonBallLocations(int toWho = -1)
        {
            DBZWorld world = DBZWorld.GetWorld();
            for (var i = 0; i < world.CachedDragonBallLocations.Length; i++)
            {
                if (toWho == -1) 
                    SendDragonBallChange(i + 1, world.GetCachedDragonBallLocation(i + 1));
                else
                    SendDragonBallChange(toWho, i + 1, world.GetCachedDragonBallLocation(i + 1));
            }
        }

        public void SendDragonBallChange(int toWho, int whichDragonBall, Point dragonBallLocation)
        {
            SendDragonBallChange(toWho, 256, whichDragonBall, dragonBallLocation);
        }

        public void SendDragonBallChange(int whichDragonBall, Point dragonBallLocation)
        {
            for (var i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player == null)
                    continue;
                if (player.whoAmI != i)
                    continue;
                SendDragonBallChange(i, 256, whichDragonBall, dragonBallLocation);
            }
        }

        public void SendDragonBallChange(int toWho, int fromWho, int whichDragonBall, Point dragonBallLocation)
        {
            ModPacket packet = GetPacket(SYNC_DRAGON_BALL_CHANGE, fromWho);
            packet.Write(whichDragonBall);
            packet.Write(dragonBallLocation.X);
            packet.Write(dragonBallLocation.Y);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveDragonBallChange(BinaryReader reader, int fromWho)
        {
            DBZWorld world = DBZWorld.GetWorld();
            var whichDragonBall = reader.ReadInt32();
            var locationX = reader.ReadInt32();
            var locationY = reader.ReadInt32();
            world.CacheDragonBallLocation(whichDragonBall, new Point(locationX, locationY));
            DebugHelper.Log($"Receiving dragon ball {whichDragonBall} location at {locationX} {locationY}");
        }

        public void RequestServerSendKiBeaconInitialSync(int toWho, int fromWho)
        {
            ModPacket packet = GetPacket(REQUEST_KI_BEACON_INITIAL_SYNC, fromWho);
            packet.Send(toWho, fromWho);
        }

        // handle a request to receive ki beacon sync from server
        public void ReceiveKiBeaconInitialSyncRequest(int toWho)
        {
            var dbWorld = DBZWorld.GetWorld();
            var numIndexes = dbWorld.kiBeacons.Count;
            // if there aren't any, no sync needed, skip this.
            if (numIndexes == 0)
                return;
            ModPacket packet = GetPacket(SEND_KI_BEACON_INITIAL_SYNC, 256);
            // attach the number of indexes to be unpacked so the client knows when to stop reading on the other side.
            packet.Write(numIndexes);
            // loop over beacon locations and write each to the packet.
            foreach (var kiBeacon in dbWorld.kiBeacons)
            {
                // each beacon position coordinate is two floats.
                packet.Write(kiBeacon.X);
                packet.Write(kiBeacon.Y);
            }
            packet.Send(toWho, -1);
        }

        // handle receiving ki beacon sync from the server.
        public void ReceiveKiBeaconInitialSync(BinaryReader reader, int toWho)
        {
            var dbWorld = DBZWorld.GetWorld();
            var numIndexes = reader.ReadInt32();
            // presume whatever we have in ours is wrong and wipe it out.
            dbWorld.kiBeacons.Clear();
            for (var i = 0; i < numIndexes; i++)
            {
                var coordX = reader.ReadSingle();
                var coordY = reader.ReadSingle();
                dbWorld.kiBeacons.Add(new Vector2(coordX, coordY));
            }
        }

        //public void ReceiveDragonBallKeySyncRequest(int toWho)
        //{
        //    ModPacket packet = GetPacket(SEND_DRAGON_BALL_KEY_SYNC, 256);
        //    var dbWorld = DBZWorld.GetWorld();
        //    packet.Write(dbWorld.worldDragonBallKey);
        //    // new stuff, send the player all the dragon ball points.
        //    packet.Write(dbWorld.GetDragonBallLocation(1).X);
        //    packet.Write(dbWorld.GetDragonBallLocation(1).Y);
        //    packet.Write(dbWorld.GetDragonBallLocation(2).X);
        //    packet.Write(dbWorld.GetDragonBallLocation(2).Y);
        //    packet.Write(dbWorld.GetDragonBallLocation(3).X);
        //    packet.Write(dbWorld.GetDragonBallLocation(3).Y);
        //    packet.Write(dbWorld.GetDragonBallLocation(4).X);
        //    packet.Write(dbWorld.GetDragonBallLocation(4).Y);
        //    packet.Write(dbWorld.GetDragonBallLocation(5).X);
        //    packet.Write(dbWorld.GetDragonBallLocation(5).Y);
        //    packet.Write(dbWorld.GetDragonBallLocation(6).X);
        //    packet.Write(dbWorld.GetDragonBallLocation(6).Y);
        //    packet.Write(dbWorld.GetDragonBallLocation(7).X);
        //    packet.Write(dbWorld.GetDragonBallLocation(7).Y);
        //    packet.Send(toWho, -1);
        //}

        //public void ReceiveDragonBallKeySync(BinaryReader reader, int fromWho)
        //{
        //    var dbWorld = DBZWorld.GetWorld();
        //    var dbKey = reader.ReadInt32();
        //    var db1X = reader.ReadInt32();
        //    var db1Y = reader.ReadInt32();
        //    var db2X = reader.ReadInt32();
        //    var db2Y = reader.ReadInt32();
        //    var db3X = reader.ReadInt32();
        //    var db3Y = reader.ReadInt32();
        //    var db4X = reader.ReadInt32();
        //    var db4Y = reader.ReadInt32();
        //    var db5X = reader.ReadInt32();
        //    var db5Y = reader.ReadInt32();
        //    var db6X = reader.ReadInt32();
        //    var db6Y = reader.ReadInt32();
        //    var db7X = reader.ReadInt32();
        //    var db7Y = reader.ReadInt32();
        //    dbWorld.SetDragonBallLocation(1, new Point(db1X, db1Y), false);
        //    dbWorld.SetDragonBallLocation(2, new Point(db2X, db2Y), false);
        //    dbWorld.SetDragonBallLocation(3, new Point(db3X, db3Y), false);
        //    dbWorld.SetDragonBallLocation(4, new Point(db4X, db4Y), false);
        //    dbWorld.SetDragonBallLocation(5, new Point(db5X, db5Y), false);
        //    dbWorld.SetDragonBallLocation(6, new Point(db6X, db6Y), false);
        //    dbWorld.SetDragonBallLocation(7, new Point(db7X, db7Y), false);
        //    dbWorld.worldDragonBallKey = dbKey;
        //}

        public void RequestServerSendDragonBallKey(int toWho, int fromWho)
        {
            ModPacket packet = GetPacket(REQUEST_DRAGON_BALL_KEY_SYNC, fromWho);
            packet.Send(toWho, fromWho);
        }

        public void RequestPlayerSendTheirInfo(int toWho, int fromWho, int playerWhoseDataIneed)
        {
            ModPacket packet = GetPacket(REQUEST_FOR_SYNC_FROM_JOINED_PLAYER, fromWho);
            packet.Write(playerWhoseDataIneed);
            packet.Send(toWho, fromWho);
        }

        public void RequestTeleport(int toWho, int fromWho, Vector2 mapPosition)
        {
            ModPacket packet = GetPacket(REQUEST_TELEPORT_MESSAGE, fromWho);
            packet.WriteVector2(mapPosition);
            packet.Send();
        }

        public void ProcessRequestTeleport(BinaryReader reader, int fromWho)
        {
            Vector2 destination = reader.ReadVector2();
            Main.player[fromWho].Teleport(destination, 1, 0);
            RemoteClient.CheckSection(fromWho, destination, 1);
            NetMessage.SendData(65, -1, -1, null, 0, fromWho, destination.X, destination.Y, 1, 0, 0);
        }

        // should always be to server, from joining player
        public void SendPlayerInfoToPlayerFromOtherPlayer(int toWho, int fromWho)
        {
            Player player = Main.player[fromWho];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            SendChangedKiMax2(toWho, fromWho, fromWho, modPlayer.kiMax2);
            SendChangedKiMax3(toWho, fromWho, fromWho, modPlayer.kiMax3);
            SendChangedKiMaxMult(toWho, fromWho, fromWho, modPlayer.kiMaxMult);
            SendChangedIsTransforming(toWho, fromWho, fromWho, modPlayer.isTransforming);
            SendChangedFragment1(toWho, fromWho, fromWho, modPlayer.fragment1);
            SendChangedFragment2(toWho, fromWho, fromWho, modPlayer.fragment2);
            SendChangedFragment3(toWho, fromWho, fromWho, modPlayer.fragment3);
            SendChangedFragment4(toWho, fromWho, fromWho, modPlayer.fragment4);
            SendChangedFragment5(toWho, fromWho, fromWho, modPlayer.fragment5);
            SendChangedIsCharging(toWho, fromWho, fromWho, modPlayer.isCharging);
            SendChangedJungleMessage(toWho, fromWho, fromWho, modPlayer.jungleMessage);
            SendChangedHellMessage(toWho, fromWho, fromWho, modPlayer.hellMessage);
            SendChangedEvilMessage(toWho, fromWho, fromWho, modPlayer.evilMessage);
            SendChangedMushroomMessage(toWho, fromWho, fromWho, modPlayer.mushroomMessage);
            SendChangedIsHoldingKiWeapon(toWho, fromWho, fromWho, modPlayer.isHoldingKiWeapon);
            SendChangedTraitChecked(toWho, fromWho, fromWho, modPlayer.traitChecked);
            SendChangedPlayerTrait(toWho, fromWho, fromWho, modPlayer.playerTrait);
            SendChangedIsFlying(toWho, fromWho, fromWho, modPlayer.isFlying);
            SnedChangedPowerWishesLeft(toWho, fromWho, fromWho, modPlayer.powerWishesLeft);
            SendChangedIsTransformationAnimationPlaying(toWho, fromWho, fromWho, modPlayer.isTransformationAnimationPlaying);
            SendChangedKiCurrent(toWho, fromWho, fromWho, modPlayer.GetKi());
        }

        public void SendChangedTriggerMouseRight(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            var packet = GetPacket(SYNC_TRIGGERS, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerMouseRight);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerMouseLeft(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            var packet = GetPacket(SYNC_TRIGGERS, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerMouseLeft);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerLeft(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            var packet = GetPacket(SYNC_TRIGGERS, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerLeft);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerRight(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            var packet = GetPacket(SYNC_TRIGGERS, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerRight);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerUp(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            var packet = GetPacket(SYNC_TRIGGERS, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerUp);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerDown(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            var packet = GetPacket(SYNC_TRIGGERS, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerDown);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiMax2(int toWho, int fromWho, int whichPlayer, int kiMax2)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KiMax2);
            packet.Write(whichPlayer);
            packet.Write(kiMax2);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiMax3(int toWho, int fromWho, int whichPlayer, int kiMax3)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KiMax3);
            packet.Write(whichPlayer);
            packet.Write(kiMax3);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiMaxMult(int toWho, int fromWho, int whichPlayer, float kiMaxMult)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KiMaxMult);
            packet.Write(whichPlayer);
            packet.Write(kiMaxMult);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsTransforming(int toWho, int fromWho, int whichPlayer, bool isTransforming)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.isTransforming);
            packet.Write(whichPlayer);
            packet.Write(isTransforming);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment1(int toWho, int fromWho, int whichPlayer, bool fragment1)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment1);
            packet.Write(whichPlayer);
            packet.Write(fragment1);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment2(int toWho, int fromWho, int whichPlayer, bool fragment2)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment2);
            packet.Write(whichPlayer);
            packet.Write(fragment2);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment3(int toWho, int fromWho, int whichPlayer, bool fragment3)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment3);
            packet.Write(whichPlayer);
            packet.Write(fragment3);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment4(int toWho, int fromWho, int whichPlayer, bool fragment4)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment4);
            packet.Write(whichPlayer);
            packet.Write(fragment4);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment5(int toWho, int fromWho, int whichPlayer, bool fragment5)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment5);
            packet.Write(whichPlayer);
            packet.Write(fragment5);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsCharging(int toWho, int fromWho, int whichPlayer, bool isCharging)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsCharging);
            packet.Write(whichPlayer);
            packet.Write(isCharging);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedJungleMessage(int toWho, int fromWho, int whichPlayer, bool jungleMessage)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.JungleMessage);
            packet.Write(whichPlayer);
            packet.Write(jungleMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedHellMessage(int toWho, int fromWho, int whichPlayer, bool hellMessage)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.HellMessage);
            packet.Write(whichPlayer);
            packet.Write(hellMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedEvilMessage(int toWho, int fromWho, int whichPlayer, bool evilMessage)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.EvilMessage);
            packet.Write(whichPlayer);
            packet.Write(evilMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedMushroomMessage(int toWho, int fromWho, int whichPlayer, bool mushroomMessage)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.MushroomMessage);
            packet.Write(whichPlayer);
            packet.Write(mushroomMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsHoldingKiWeapon(int toWho, int fromWho, int whichPlayer, bool isHoldingKiWeapon)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsHoldingKiWeapon);
            packet.Write(whichPlayer);
            packet.Write(isHoldingKiWeapon);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTraitChecked(int toWho, int fromWho, int whichPlayer, bool traitChecked)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TraitChecked);
            packet.Write(whichPlayer);
            packet.Write(traitChecked);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedPlayerTrait(int toWho, int fromWho, int whichPlayer, string playerTrait)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.PlayerTrait);
            packet.Write(whichPlayer);
            packet.Write(playerTrait);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsFlying(int toWho, int fromWho, int whichPlayer, bool isFlying)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsFlying);
            packet.Write(whichPlayer);
            packet.Write(isFlying);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsTransformationAnimationPlaying(int toWho, int fromWho, int whichPlayer, bool isTransformationAnimationPlaying)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsTransformationAnimationPlaying);
            packet.Write(whichPlayer);
            packet.Write(isTransformationAnimationPlaying);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedChargeMoveSpeed(int toWho, int fromWho, int whichPlayer, float chargeMoveSpeed)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.ChargeMoveSpeed);
            packet.Write(whichPlayer);
            packet.Write(chargeMoveSpeed);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedBonusSpeedMultiplier(int toWho, int fromWho, int whichPlayer, float bonusSpeedMultiplier)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.BonusSpeedMultiplier);
            packet.Write(whichPlayer);
            packet.Write(bonusSpeedMultiplier);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedWishActive(int toWho, int fromWho, int whichPlayer, bool wishActive)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.WishActive);
            packet.Write(whichPlayer);
            packet.Write(wishActive);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKaiokenLevel(int toWho, int fromWho, int whichPlayer, int kaiokenLevel)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KaiokenLevel);
            packet.Write(whichPlayer);
            packet.Write(kaiokenLevel);
            packet.Send(toWho, fromWho);
        }
        
        public void SendChangedMouseWorldOctant(int toWho, int fromWho, int whichPlayer, int mouseWorldOctant)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.MouseWorldOctant);
            packet.Write(whichPlayer);
            packet.Write(mouseWorldOctant);
            packet.Send(toWho, fromWho);
        }

        public void SnedChangedPowerWishesLeft(int toWho, int fromWho, int whichPlayer, int powerWishesLeft)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho);
            packet.Write((int)PlayerVarSyncEnum.PowerWishesLeft);
            packet.Write(whichPlayer);
            packet.Write(powerWishesLeft);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedDirection(int toWho, int fromWho, int whichPlayer, int direction)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho); ;
            packet.Write((int)PlayerVarSyncEnum.FacingDirection);
            packet.Write(whichPlayer);
            packet.Write(direction);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiCurrent(int toWho, int fromWho, int whichPlayer, float kiCurrent)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho); ;
            packet.Write((int)PlayerVarSyncEnum.KiCurrent);
            packet.Write(whichPlayer);
            packet.Write(kiCurrent);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedHeldProjectile(int toWho, int fromWho, int whichPlayer, int projHeld)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho); ;
            packet.Write((int)PlayerVarSyncEnum.HeldProjectile);
            packet.Write(whichPlayer);
            packet.Write(projHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedMassiveBlastCharging(int toWho, int fromWho, int whichPlayer, bool isMassiveBlastCharging)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho); ;
            packet.Write((int)PlayerVarSyncEnum.IsMassiveBlastCharging);
            packet.Write(whichPlayer);
            packet.Write(isMassiveBlastCharging);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedMassiveBlastInUse(int toWho, int fromWho, int whichPlayer, bool isMassiveBlastInUse)
        {
            var packet = GetPacket(SYNC_PLAYER, fromWho); ;
            packet.Write((int)PlayerVarSyncEnum.IsMassiveBlastInUse);
            packet.Write(whichPlayer);
            packet.Write(isMassiveBlastInUse);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveSyncTriggers(BinaryReader reader, int fromWho)
        {
            PlayerVarSyncEnum syncEnum = (PlayerVarSyncEnum)reader.ReadInt32();
            int playerNum = reader.ReadInt32();
            MyPlayer player = Main.player[playerNum].GetModPlayer<MyPlayer>();
            bool isHeld = reader.ReadBoolean();
            // if this is a server, start to assemble the relay packet.
            ModPacket packet = null;
            if (Main.netMode == NetmodeID.Server)
            {
                packet = GetPacket(SYNC_TRIGGERS, fromWho);
                packet.Write((int)syncEnum);
                packet.Write(playerNum);
            }

            switch (syncEnum)
            {
                case PlayerVarSyncEnum.TriggerMouseLeft:
                    player.isMouseLeftHeld = isHeld;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(isHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerMouseRight:
                    player.isMouseRightHeld = isHeld;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(isHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerLeft:
                    player.isLeftHeld = isHeld;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(isHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerRight:
                    player.isRightHeld = isHeld;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(isHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerUp:
                    player.isUpHeld = isHeld;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(isHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerDown:
                    player.isDownHeld = isHeld;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(isHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
            }
        }

        public void ReceiveChangedPlayerData(BinaryReader reader, int fromWho)
        {
            //Console.WriteLine("Receiving player sync change packet!");
            PlayerVarSyncEnum syncEnum = (PlayerVarSyncEnum)reader.ReadInt32();
            int playerNum = reader.ReadInt32();
            MyPlayer player = Main.player[playerNum].GetModPlayer<MyPlayer>();

            // if this is a server, start to assemble the relay packet.
            ModPacket packet = null;
            if (Main.netMode == NetmodeID.Server)
            {
                packet = GetPacket(SYNC_PLAYER, fromWho);
                packet.Write((int)syncEnum);
                packet.Write(playerNum);
            }
            switch (syncEnum)
            {
                case PlayerVarSyncEnum.KiMax2:
                    player.kiMax2 = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.kiMax2);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.KiMax3:
                    player.kiMax3 = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.kiMax3);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.KiMaxMult:
                    player.kiMaxMult = reader.ReadSingle();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.kiMaxMult);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.isTransforming:
                    player.isTransforming = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.isTransforming);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment1:
                    player.fragment1 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.fragment1);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment2:
                    player.fragment2 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.fragment2);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment3:
                    player.fragment3 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.fragment3);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment4:
                    player.fragment4 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.fragment4);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment5:
                    player.fragment5 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.fragment5);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsCharging:
                    player.isCharging = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.isCharging);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.JungleMessage:
                    player.jungleMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.jungleMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.HellMessage:
                    player.hellMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.hellMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.EvilMessage:
                    player.evilMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.evilMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.MushroomMessage:
                    player.mushroomMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.mushroomMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsHoldingKiWeapon:
                    player.isHoldingKiWeapon = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.isHoldingKiWeapon);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TraitChecked:
                    player.traitChecked = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.traitChecked);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.PlayerTrait:
                    player.playerTrait = reader.ReadString();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.playerTrait);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsFlying:
                    player.isFlying = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.isFlying);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsTransformationAnimationPlaying:
                    player.isTransformationAnimationPlaying = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.isTransformationAnimationPlaying);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.ChargeMoveSpeed:
                    player.chargeMoveSpeed = reader.ReadSingle();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.chargeMoveSpeed);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.BonusSpeedMultiplier:
                    player.bonusSpeedMultiplier = reader.ReadSingle();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.bonusSpeedMultiplier);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.MouseWorldOctant:
                    player.mouseWorldOctant = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.mouseWorldOctant);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.PowerWishesLeft:
                    player.powerWishesLeft = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.powerWishesLeft);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.HeldProjectile:
                    player.player.heldProj = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.player.heldProj);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsMassiveBlastCharging:
                    player.isMassiveBlastCharging = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.isMassiveBlastCharging);
                        packet.Send(-1, fromWho);
                    }
                    break;
                //case PlayerVarSyncEnum.IsMassiveBlastInUse:
                //    player.isMassiveBlastInUse = reader.ReadBoolean();
                //    if (Main.netMode == NetmodeID.Server)
                //    {
                //        packet.Write(player.isMassiveBlastInUse);
                //        packet.Send(-1, fromWho);
                //    }
                //    break;
                case PlayerVarSyncEnum.WishActive:
                    player.wishActive = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.wishActive);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.FacingDirection:
                    player.player.ChangeDir(reader.ReadInt32());
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.player.direction);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.KiCurrent:
                    player.SetKi(reader.ReadSingle(), true);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.GetKi());
                        packet.Send(-1, fromWho);
                    }
                    break;
            }
        }
    }
}
