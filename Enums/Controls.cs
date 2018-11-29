using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBZMOD.Enums
{
    /// <summary>
    ///     The distinct control types currently being scanned for various inputs in the mod.
    /// </summary>
    public enum Controls
    {
        None = 0,
        LightPunch = 1,
        HeavyPunch = 2,
        DashUp = 3,
        DashDown = 4,
        DashLeft = 5,
        DashRight = 6,
        DashUpLeft = 7,
        DashUpRight = 8,
        DashDownLeft = 9,
        DashDownRight = 10,
        Block = 11
    }
}
