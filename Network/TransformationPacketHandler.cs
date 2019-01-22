using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DBZMOD.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.Util;

namespace Network
{
    internal class TransformationPacketHandler : PacketHandler
    {
        public const byte SyncTransformations = 1;

        public TransformationPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SyncTransformations):
                    ReceiveFormChanges(reader, fromWho);
                    break;
            }
        }

        public void SendFormChanges(int toWho, int fromWho, int whichPlayer, string buffKeyName, int duration)
        {
            ModPacket packet = GetPacket(SyncTransformations, fromWho);  
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(buffKeyName);
            packet.Write(duration);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveFormChanges(BinaryReader reader, int fromWho)
        {            
            int whichPlayer = reader.ReadInt32();
            string buffKeyName = reader.ReadString();
            int duration = reader.ReadInt32();
            if (Main.netMode == NetmodeID.Server)
            {
                SendFormChanges(-1, fromWho, whichPlayer, buffKeyName, duration);
            }
            else
            {
                Player thePlayer = Main.player[whichPlayer];
                // handle form removal if duration is 0
                if (duration == 0)
                {
                    Transformations.RemoveTransformation(thePlayer, buffKeyName);                    
                } else
                {
                    // make sure the player has the buff on every client                    
                    Transformations.DoTransform(thePlayer, Transformations.GetBuffByKeyName(buffKeyName), DBZMOD.DBZMOD.instance);
                }
            }
        }
    }
}
