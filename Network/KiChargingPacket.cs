using DBZMOD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Network
{
    internal class KiChargingPacketHandler : PacketHandler
    {
        public const byte SyncCharge = 1;

        public KiChargingPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SyncCharge):
                    ReceiveChargeChanges(reader, fromWho);
                    break;
            }
        }

        public void SendChargeChanges(int toWho, int fromWho, int whichPlayer, bool isCharging)
        {
            ModPacket packet = GetPacket(SyncCharge, fromWho);
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(isCharging);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveChargeChanges(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            bool isCharging = reader.ReadBoolean();
            if (Main.netMode == NetmodeID.Server)
            {
                SendChargeChanges(-1, fromWho, whichPlayer, isCharging);
            }
            else
            {
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().IsCharging = isCharging;
            }
        }
    }
}
