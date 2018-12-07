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
    internal class KiChangePacketHandler : PacketHandler
    {
        public const byte SyncKiChange = 1;

        public KiChangePacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SyncKiChange):
                    ReceiveKiChanges(reader, fromWho);
                    break;
            }
        }

        public void SendKiChanges(int toWho, int fromWho, int whichPlayer, int newKi, int kiMax2, int kiMax3, float kiMaxMult, bool isLegendary)
        {
            ModPacket packet = GetPacket(SyncKiChange, fromWho);
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(newKi);
            packet.Write(kiMax2);
            packet.Write(kiMax3);
            packet.Write(kiMaxMult);
            packet.Write(isLegendary);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveKiChanges(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            int newKi = reader.ReadInt32();
            int kiMax2 = reader.ReadInt32();
            int kiMax3 = reader.ReadInt32();
            float kiMaxMult = reader.ReadSingle();
            bool isLegendary = reader.ReadBoolean();
            if (Main.netMode == NetmodeID.Server)
            {
                SendKiChanges(-1, fromWho, whichPlayer, newKi, kiMax2, kiMax3, kiMaxMult, isLegendary);
            }
            else
            {
                // take care of setting player maximums client side first, so your client thinks other clients have ki.
                MyPlayer modPlayer = Main.player[whichPlayer].GetModPlayer<MyPlayer>();
                modPlayer.KiMax2 = kiMax2;
                modPlayer.KiMax3 = kiMax3;
                modPlayer.KiMaxMult = kiMaxMult;
                if (isLegendary)
                {
                    modPlayer.player.AddBuff(modPlayer.mod.BuffType("LegendaryTrait"), 10);
                }
                modPlayer.SetKi(newKi);
            }
        }
    }
}
