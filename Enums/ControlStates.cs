using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBZMOD.Enums
{
    /// <summary>
    ///     The possible states a control can be in, needed for heuristic/behavior detection by the mod at any given point.
    /// </summary>
    public enum ControlStates
    {
        Released, // neutral, unpressed. Synonymous to "None"
        PressedOnce, // first detection of an input triggers this
        PressedAndReleased, // next phase, the key is released, but was pressed in the input window (for dashing)
        PressedTwice, // next phase, after key release, but still in the input window (for dashing), pressed a second time.
        PressedAndHeld // alternate phase, after pressedOnce, if the key is never released. Starts a held counter (?)
    }
}
