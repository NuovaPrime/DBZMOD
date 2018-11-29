using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBZMOD.Enums
{
    /// <summary>
    ///     A bitwise enum containing the combination of all possible inputs for simultaneous Should-Do-Whatever checks
    /// </summary>
    [Flags]
    public enum ControlInputTypes : short
    {
        None = 0,
        Up = 1,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        LightAttack = 1 << 4,
        HeavyAttack = 1 << 5,
        Block = LightAttack & HeavyAttack,
        UpLeft = Up & Left,
        UpRight = Up & Right,
        DownLeft = Down & Left,
        DownRight = Down & Right,
        Any = Up | Down | Left | Right | LightAttack | HeavyAttack
    }
}
