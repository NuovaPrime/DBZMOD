using System.IO;
using DBZMOD.Extensions;
using DBZMOD.Util;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PlayerExtensions = DBZMOD.Extensions.PlayerExtensions;

namespace DBZMOD.Network
{
    internal class TransformationPacketHandler : PacketHandler
    {
        public const byte SYNC_TRANSFORMATIONS = 1;

        public TransformationPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SYNC_TRANSFORMATIONS):
                    ReceiveFormChanges(reader, fromWho);
                    break;
            }
        }

        public void SendFormChanges(int toWho, int fromWho, int whichPlayer, string buffKeyName, int duration)
        {
            ModPacket packet = GetPacket(SYNC_TRANSFORMATIONS, fromWho);  
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
                Player player = Main.player[whichPlayer];
                // handle form removal if duration is 0
                if (duration == 0)
                {
                    player.RemoveTransformation(buffKeyName);                    
                } else
                {
                    // make sure the player has the buff on every client                    
                    player.DoTransform(FormBuffHelper.GetBuffByKeyName(buffKeyName), global::DBZMOD.DBZMOD.Instance);
                }
            }
        }
    }
}
