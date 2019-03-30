using System.IO;
using DBZMOD.Extensions;
using DBZMOD.Transformations;
using DBZMOD.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Network
{
    internal class TransformationPacketHandler : PacketHandler
    {
        public const byte
            SYNC_TRANSFORMATION_ADDED = 1,
            SYNC_TRANSFORMATIONS_REMOVED = 2;

        public TransformationPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            fromWho = reader.ReadInt32();

            switch (reader.ReadByte())
            {
                case (SYNC_TRANSFORMATION_ADDED):
                    ReceivePlayerTransformedInto(reader, fromWho);
                    break;
                case (SYNC_TRANSFORMATIONS_REMOVED):
                    ReceiveTransformationRemoved(reader, fromWho);
                    break;
            }
        }

        /*public void SendFormChanges(int toWho, int fromWho, int whichPlayer, string transformationUnlocalizedName)
        {
            ModPacket packet = GetPacket(SYNC_TRANSFORMATIONS, fromWho);
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(transformationUnlocalizedName);
            packet.Send(toWho, fromWho);
        }*/

        /*public void ReceiveFormChanges(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            string transformationName = reader.ReadString();

            if (Main.netMode == NetmodeID.Server)
            {
                SendFormChanges(-1, fromWho, whichPlayer, transformationName);
            }
            else
            {
                // make sure the player has the buff on every client                    
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().DoTransform(DBZMOD.Instance.TransformationDefinitionManager[transformationName]);
            }
        }*/

        #region PlayerTransformedInto

        public void ReceivePlayerTransformedInto(BinaryReader reader, int fromWho)
        {
            TransformationDefinition transformation = DBZMOD.Instance.TransformationDefinitionManager[reader.ReadString()];
            int whichPlayer = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
                SendPlayerTransformedInto(-1, fromWho, whichPlayer, transformation);
            else           
            {
                MyPlayer myPlayer = Main.player[whichPlayer].GetModPlayer<MyPlayer>();
                
                if (myPlayer.player.whoAmI == whichPlayer)
                    myPlayer.ActiveTransformations.Add(transformation);
            }
        }

        public void SendPlayerTransformedInto(int toWho, int fromWho, int whichPlayer, TransformationDefinition transformation)
        {
            ModPacket packet = GetPacket(SYNC_TRANSFORMATION_ADDED, fromWho);

            packet.Write(transformation.UnlocalizedName);
            packet.Send(toWho, fromWho);
        }

        #endregion


        #region PlayerTransformationRemoved

        public void ReceiveTransformationRemoved(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            TransformationDefinition transformation = DBZMOD.Instance.TransformationDefinitionManager[reader.ReadString()];

            if (Main.netMode == NetmodeID.Server)
                SendTransformationRemoved(-1, fromWho, whichPlayer, transformation);
            else
                Main.player[whichPlayer].GetModPlayer<MyPlayer>().ActiveTransformations.Remove(transformation);
        }

        public void SendTransformationRemoved(int toWho, int fromWho, int whichPlayer, TransformationDefinition transformation)
        {
            ModPacket packet = GetPacket(SYNC_TRANSFORMATIONS_REMOVED, fromWho);
            packet.Write(whichPlayer);
            packet.Write(transformation.UnlocalizedName);
            packet.Send(toWho, fromWho);
        }

        #endregion
    }
}
