using DBZMOD.Projectiles.Auras;
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
    internal class FlightAuraPacketHandler : PacketHandler
    {
        public const byte SyncAuras = 1;

        public FlightAuraPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SyncAuras):
                    ReceiveAuraInFlightChanges(reader, fromWho);
                    break;
            }
        }

        public void SendAuraInFlightChanges(int toWho, int fromWho, int whichPlayer, float positionX, float positionY, float rotation)
        {
            ModPacket packet = GetPacket(SyncAuras, fromWho);
            // this indicates we're the originator of the packet. include our player.
            packet.Write(whichPlayer);
            packet.Write(positionX);
            packet.Write(positionY);
            packet.Write(rotation);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveAuraInFlightChanges(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            float rotation = reader.ReadSingle();
            if (Main.netMode == NetmodeID.Server)
            {                
                SendAuraInFlightChanges(-1, fromWho, whichPlayer, positionX, positionY, rotation);
            }
            else
            {
                foreach(Projectile proj in Main.projectile)
                {
                    if (proj.modProjectile == null)
                        continue;
                    if (proj.owner == whichPlayer && proj.modProjectile.GetType().IsAssignableFrom(typeof(BaseAuraProj)))
                    {
                        proj.position.X = positionX;
                        proj.position.Y = positionY;
                        proj.rotation = rotation;
                    }
                }
            }
        }
    }
}
