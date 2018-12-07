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
    internal class KiFragmentPacketHandler : PacketHandler
    {
        public const byte SyncFragments = 1;

        public KiFragmentPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SyncFragments):
                    ReceiveFragmentChanges(reader, fromWho);
                    break;
            }
        }

        public void SendFragmentChanges(int toWho, int fromWho, int whichPlayer, bool fragment1, bool fragment2, bool fragment3, bool fragment4, bool fragment5)
        {
            ModPacket packet = GetPacket(SyncFragments, fromWho);
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);            
            packet.Write(fragment1);
            packet.Write(fragment2);
            packet.Write(fragment3);
            packet.Write(fragment4);
            packet.Write(fragment5);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveFragmentChanges(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            bool fragment1 = reader.ReadBoolean();
            bool fragment2 = reader.ReadBoolean();
            bool fragment3 = reader.ReadBoolean();
            bool fragment4 = reader.ReadBoolean();
            bool fragment5 = reader.ReadBoolean();
            if (Main.netMode == NetmodeID.Server)
            {
                SendFragmentChanges(-1, fromWho, whichPlayer, fragment1, fragment2, fragment3, fragment4, fragment5);
            }
            else
            {
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().Fragment1 = fragment1;
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().Fragment2 = fragment2;
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().Fragment3 = fragment3;
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().Fragment4 = fragment4;
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().Fragment5 = fragment5;
            }
        }
    }
}
