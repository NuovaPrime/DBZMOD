using System.IO;
using DBZMOD.Extensions;
using DBZMOD.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Network
{
    internal class TransformationPacketHandler : PacketHandler
    {
        public const byte 
            SYNC_TRANSFORMATIONS = 1,
            SYNC_TRANSFORMATIONS_CLEARED = 2;

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
                case (SYNC_TRANSFORMATIONS_CLEARED):
                    ReceiveTransformationsCleared(reader, fromWho);
                    break;
            }
        }

        public void SendFormChanges(int toWho, int fromWho, int whichPlayer, string transformationUnlocalizedName, int duration)
        {
            ModPacket packet = GetPacket(SYNC_TRANSFORMATIONS, fromWho);  
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(transformationUnlocalizedName);
            packet.Write(duration);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveFormChanges(BinaryReader reader, int fromWho)
        {            
            int whichPlayer = reader.ReadInt32();
            string transformationName = reader.ReadString();
            int duration = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendFormChanges(-1, fromWho, whichPlayer, transformationName, duration);
            }
            else
            {
                Player player = Main.player[whichPlayer];

                // handle form removal if duration is 0
                if (duration == 0)
                {
                    player.GetModPlayer<MyPlayer>().RemoveTransformation(DBZMOD.Instance.TransformationDefinitionManager[transformationName]);
                } else
                {
                    // make sure the player has the buff on every client                    
                    player.GetModPlayer<MyPlayer>().DoTransform(DBZMOD.Instance.TransformationDefinitionManager[transformationName]);
                }
            }
        }

        public void ReceiveTransformationsCleared(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
                SendTransformationCleared(-1, fromWho, whichPlayer);
            else
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().ClearAllTransformations();
        }

        public void SendTransformationCleared(int toWho, int fromWho, int whichPlayer)
        {
            ModPacket packet = GetPacket(SYNC_TRANSFORMATIONS_CLEARED, fromWho);
            packet.Write(whichPlayer);
            packet.Send(toWho, fromWho);
        }
    }
}
