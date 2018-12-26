using DBZMOD;
using Enums;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

namespace Network
{    
    internal class PlayerPacketHandler : PacketHandler
    {
        // this timer keeps the ki sync from firing *every frame* which really hurts performance.
        public const byte SyncPlayer = 43;
        public const byte SyncTriggers = 44;
        public const byte RequestForSyncFromJoinedPlayer = 45;
        public const byte RequestDragonBallKeySync = 46;

        public PlayerPacketHandler(byte handlerType) : base(handlerType)
        {
        }
        
        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SyncPlayer):
                    ReceiveChangedPlayerData(reader, fromWho);
                    break;
                case (SyncTriggers):
                    ReceiveSyncTriggers(reader, fromWho);
                    break;
                case (RequestForSyncFromJoinedPlayer):
                    int whichPlayersDataNeedsRelay = reader.ReadInt32();
                    SendPlayerInfoToPlayerFromOtherPlayer(fromWho, whichPlayersDataNeedsRelay);
                    break;
                case (RequestDragonBallKeySync):
                    SendDragonBallKeyToPlayer(fromWho);
                    break;
            }
        }

        public void RequestServerSendDragonBallKey(int toWho, int fromWho)
        {
            ModPacket packet = GetPacket(RequestDragonBallKeySync, fromWho);
            packet.Send(toWho, fromWho);
        }

        public void RequestPlayerSendTheirInfo(int toWho, int fromWho, int playerWhoseDataIneed)
        {
            ModPacket packet = GetPacket(RequestForSyncFromJoinedPlayer, fromWho);
            packet.Write(playerWhoseDataIneed);
            packet.Send(toWho, fromWho);
        }

        // should always be to server, from joining player
        public void SendPlayerInfoToPlayerFromOtherPlayer(int toWho, int fromWho)
        {
            Player player = Main.player[fromWho];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            SendChangedKiMax2(toWho, fromWho, fromWho, modPlayer.KiMax2);
            SendChangedKiMax3(toWho, fromWho, fromWho, modPlayer.KiMax3);
            SendChangedKiMaxMult(toWho, fromWho, fromWho, modPlayer.KiMaxMult);
            SendChangedIsTransforming(toWho, fromWho, fromWho, modPlayer.IsTransforming);
            SendChangedFragment1(toWho, fromWho, fromWho, modPlayer.Fragment1);
            SendChangedFragment2(toWho, fromWho, fromWho, modPlayer.Fragment2);
            SendChangedFragment3(toWho, fromWho, fromWho, modPlayer.Fragment3);
            SendChangedFragment4(toWho, fromWho, fromWho, modPlayer.Fragment4);
            SendChangedFragment5(toWho, fromWho, fromWho, modPlayer.Fragment5);
            SendChangedIsCharging(toWho, fromWho, fromWho, modPlayer.IsCharging);
            SendChangedJungleMessage(toWho, fromWho, fromWho, modPlayer.JungleMessage);
            SendChangedHellMessage(toWho, fromWho, fromWho, modPlayer.HellMessage);
            SendChangedEvilMessage(toWho, fromWho, fromWho, modPlayer.EvilMessage);
            SendChangedMushroomMessage(toWho, fromWho, fromWho, modPlayer.MushroomMessage);
            SendChangedIsHoldingKiWeapon(toWho, fromWho, fromWho, modPlayer.IsHoldingKiWeapon);
            SendChangedTraitChecked(toWho, fromWho, fromWho, modPlayer.traitChecked);
            SendChangedPlayerTrait(toWho, fromWho, fromWho, modPlayer.playerTrait);
            SendChangedIsFlying(toWho, fromWho, fromWho, modPlayer.IsFlying);
            SendChangedIsTransformationAnimationPlaying(toWho, fromWho, fromWho, modPlayer.IsTransformationAnimationPlaying);
            SendChangedKiCurrent(toWho, fromWho, fromWho, modPlayer.GetKi());
        }

        public void SendDragonBallKeyToPlayer(int toWho)
        {
            ModPacket packet = GetPacket(RequestDragonBallKeySync, 256);
            var dbWorld = DBZMOD.DBZMOD.instance.GetModWorld("DBZWorld") as DBZWorld;
            packet.Write(dbWorld.WorldDragonBallKey);
            packet.Send(toWho, 256);
        }

        public void SendChangedTriggerLeft(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {            
            var packet = GetPacket(SyncTriggers, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerLeft);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerRight(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            var packet = GetPacket(SyncTriggers, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerRight);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerUp(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            var packet = GetPacket(SyncTriggers, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerUp);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerDown(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            var packet = GetPacket(SyncTriggers, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerDown);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiMax2(int toWho, int fromWho, int whichPlayer, int kiMax2)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KiMax2);
            packet.Write(whichPlayer);
            packet.Write(kiMax2);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiMax3(int toWho, int fromWho, int whichPlayer, int kiMax3)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KiMax3);
            packet.Write(whichPlayer);
            packet.Write(kiMax3);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiMaxMult(int toWho, int fromWho, int whichPlayer, float KiMaxMult)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KiMaxMult);
            packet.Write(whichPlayer);
            packet.Write(KiMaxMult);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsTransforming(int toWho, int fromWho, int whichPlayer, bool IsTransforming)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsTransforming);
            packet.Write(whichPlayer);
            packet.Write(IsTransforming);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment1(int toWho, int fromWho, int whichPlayer, bool Fragment1)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment1);
            packet.Write(whichPlayer);
            packet.Write(Fragment1);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment2(int toWho, int fromWho, int whichPlayer, bool Fragment2)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment2);
            packet.Write(whichPlayer);
            packet.Write(Fragment2);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment3(int toWho, int fromWho, int whichPlayer, bool Fragment3)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment3);
            packet.Write(whichPlayer);
            packet.Write(Fragment3);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment4(int toWho, int fromWho, int whichPlayer, bool Fragment4)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment4);
            packet.Write(whichPlayer);
            packet.Write(Fragment4);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment5(int toWho, int fromWho, int whichPlayer, bool Fragment5)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment5);
            packet.Write(whichPlayer);
            packet.Write(Fragment5);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsCharging(int toWho, int fromWho, int whichPlayer, bool IsCharging)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsCharging);
            packet.Write(whichPlayer);
            packet.Write(IsCharging);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedJungleMessage(int toWho, int fromWho, int whichPlayer, bool JungleMessage)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.JungleMessage);
            packet.Write(whichPlayer);
            packet.Write(JungleMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedHellMessage(int toWho, int fromWho, int whichPlayer, bool HellMessage)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.HellMessage);
            packet.Write(whichPlayer);
            packet.Write(HellMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedEvilMessage(int toWho, int fromWho, int whichPlayer, bool EvilMessage)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.EvilMessage);
            packet.Write(whichPlayer);
            packet.Write(EvilMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedMushroomMessage(int toWho, int fromWho, int whichPlayer, bool MushroomMessage)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.MushroomMessage);
            packet.Write(whichPlayer);
            packet.Write(MushroomMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsHoldingKiWeapon(int toWho, int fromWho, int whichPlayer, bool IsHoldingKiWeapon)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsHoldingKiWeapon);
            packet.Write(whichPlayer);
            packet.Write(IsHoldingKiWeapon);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTraitChecked(int toWho, int fromWho, int whichPlayer, bool traitChecked)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.traitChecked);
            packet.Write(whichPlayer);
            packet.Write(traitChecked);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedPlayerTrait(int toWho, int fromWho, int whichPlayer, string playerTrait)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.playerTrait);
            packet.Write(whichPlayer);
            packet.Write(playerTrait);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsFlying(int toWho, int fromWho, int whichPlayer, bool IsFlying)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsFlying);
            packet.Write(whichPlayer);
            packet.Write(IsFlying);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsTransformationAnimationPlaying(int toWho, int fromWho, int whichPlayer, bool IsTransformationAnimationPlaying)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsTransformationAnimationPlaying);
            packet.Write(whichPlayer);
            packet.Write(IsTransformationAnimationPlaying);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedChargeMoveSpeed(int toWho, int fromWho, int whichPlayer, float chargeMoveSpeed)
        {            
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.ChargeMoveSpeed);
            packet.Write(whichPlayer);
            packet.Write(chargeMoveSpeed);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedBonusSpeedMultiplier(int toWho, int fromWho, int whichPlayer, float bonusSpeedMultiplier)
        {            
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int) PlayerVarSyncEnum.BonusSpeedMultiplier);
            packet.Write(whichPlayer);
            packet.Write(bonusSpeedMultiplier);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedWishActive(int toWho, int fromWho, int whichPlayer, bool WishActive)
        {
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.WishActive);
            packet.Write(whichPlayer);
            packet.Write(WishActive);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiCurrent(int toWho, int fromWho, int whichPlayer, float kiCurrent)
        {          
            var packet = GetPacket(SyncPlayer, fromWho); ;
            packet.Write((int)PlayerVarSyncEnum.KiCurrent);
            packet.Write(whichPlayer);
            packet.Write(kiCurrent);
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
                packet = GetPacket(SyncTriggers, fromWho);
                packet.Write((int)syncEnum);
                packet.Write(playerNum);
            }

            switch(syncEnum)
            {
                case PlayerVarSyncEnum.TriggerLeft:
                    player.IsLeftHeld = isHeld;
                    if (Main.netMode == NetmodeID.Server)                    {
                        packet.Write(isHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerRight:
                    player.IsRightHeld = isHeld;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(isHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerUp:
                    player.IsUpHeld = isHeld;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(isHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerDown:
                    player.IsDownHeld = isHeld;
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
                packet = GetPacket(SyncPlayer, fromWho);
                packet.Write((int)syncEnum);
                packet.Write(playerNum);
            }
            switch (syncEnum)
            {
                case PlayerVarSyncEnum.KiMax2:
                    player.KiMax2 = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.KiMax2);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.KiMax3:
                    player.KiMax3 = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.KiMax3);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.KiMaxMult:
                    player.KiMaxMult = reader.ReadSingle();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.KiMaxMult);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsTransforming:
                    player.IsTransforming = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsTransforming);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment1:
                    player.Fragment1 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment1);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment2:
                    player.Fragment2 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment2);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment3:
                    player.Fragment3 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment3);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment4:
                    player.Fragment4 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment4);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment5:
                    player.Fragment5 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment5);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsCharging:
                    player.IsCharging = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsCharging);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.JungleMessage:
                    player.JungleMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.JungleMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.HellMessage:
                    player.HellMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.HellMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.EvilMessage:
                    player.EvilMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.EvilMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.MushroomMessage:
                    player.MushroomMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.MushroomMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsHoldingKiWeapon:
                    player.IsHoldingKiWeapon = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsHoldingKiWeapon);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.traitChecked:
                    player.traitChecked = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.traitChecked);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.playerTrait:
                    player.playerTrait = reader.ReadString();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.playerTrait);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsFlying:
                    player.IsFlying = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsFlying);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsTransformationAnimationPlaying:
                    player.IsTransformationAnimationPlaying = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsTransformationAnimationPlaying);
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
                case PlayerVarSyncEnum.WishActive:
                    player.WishActive = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.WishActive);
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
