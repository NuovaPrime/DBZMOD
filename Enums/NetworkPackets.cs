using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enums
{
    // POC for network packet types the mod needs as flags you can use to do single-pass logic with consistent ordering.
    // The server/client can unpack/extrapolate what data needs updating from a single byte, hopefully.
    [Flags]
    enum NetworkPackets
    {
        Unspecified = 0,

        Rotation = 1,

        Velocity = 1<<1,

        Buff = 1<<2,

        Player = 1<<3,

        Projectile = ~Player,

        ChargeStart = 1<<4,

        ChargeStop = 1<<5
    }
}
