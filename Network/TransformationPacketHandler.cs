using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

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

        public void SendFormChanges(int toWho, int fromWho, int whichPlayer, int buffId, int duration)
        {
            if (toWho == -1)
            {
                // Console.WriteLine(string.Format("Sending players sync for {0} for form {1} for {2} frames", whichPlayer, buffId, duration));
            } else
            {
                // Main.NewText(string.Format("Sending server sync for forms: {0} for {1} frames", buffId, duration));
            }
            ModPacket packet = GetPacket(SyncTransformations, fromWho);  
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(buffId);
            packet.Write(duration);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveFormChanges(BinaryReader reader, int fromWho)
        {            
            int whichPlayer = reader.ReadInt32();
            int buffId = reader.ReadInt32();
            int duration = reader.ReadInt32();
            // Main.NewText(string.Format("Receiving sync signal for forms for player {0} for buff {1} for {2} frames", whichPlayer, buffId, duration));
            if (Main.netMode == NetmodeID.Server)
            {
                SendFormChanges(-1, fromWho, whichPlayer, buffId, duration);
            }
            else
            {
                Player thePlayer = Main.player[whichPlayer];
                // handle form removal if duration is 0
                if (duration == 0)
                {
                    Transformations.RemoveTransformation(thePlayer, buffId, true);                    
                } else
                {
                    // make sure the player has the buff on every client
                    if (!thePlayer.HasBuff(buffId))
                    {
                        Transformations.AddTransformation(thePlayer, buffId, duration, true);
                    }
                }
            }
        }
    }
}
