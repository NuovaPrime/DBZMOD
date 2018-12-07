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
    internal abstract class PacketHandler
    {
        internal byte HandlerType { get; set; }

        public abstract void HandlePacket(BinaryReader reader, int fromWho);

        protected PacketHandler(byte handlerType)
        {
            HandlerType = handlerType;
        }

        protected ModPacket GetPacket(byte packetType, int fromWho)
        {
            var p = DBZMOD.DBZMOD.instance.GetPacket();
            p.Write(HandlerType);
            p.Write(packetType);
            // this seems to fuck shit up and I don't understand why blushie put it in the example.
            //if (Main.netMode == NetmodeID.Server)
            //{
            //    p.Write((byte)fromWho);
            //}
            return p;
        }
    }
}
