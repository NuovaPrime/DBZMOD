using DBZMOD.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;

namespace DBZMOD.Util
{
    /// <summary>
    ///     class of the util that handles tracking the metadata of a particular control, its state, how long it's held/been since press/release.
    /// </summary>
    class ControlStateMetadata
    {
        // enum declaring the state of a given control at any point in time.
        public ControlStates state;

        // dual purpose int, either tracking the input delay before Pressed/PressedAndReleased reset.
        public int inputTimer;

        // if the button is being held for a protracted period of time (light attacks/heavy attacks?), this flag lets the methods know what to do with the input timer.
        public bool isHeld;
    }

    /// <summary>
    ///     Class containing methods useful for manipulating and processing control and control state for various mod player functions.
    /// </summary>
    class ControlHelper
    {
        public Dictionary<Controls, ControlStateMetadata> _controlDictionary = null;
        public Dictionary<Controls, ControlStateMetadata> ControlDictionary
        {
            get
            {
                return _controlDictionary;
            }
            set
            {
                _controlDictionary = value;
            }
        }

        /// <summary>
        ///     Scrape trigger states and return new state based on previous state.
        /// </summary>
        /// <param name="player">The player the input is being processed for.</param>
        /// <param name="inputStateTriggerSet">The triggerset belonging to the owner (typically a player) of the entity firing this process (presumably kifist projectile)</param>
        /// <returns></returns>
        public ControlInputTypes GetCurrentInputState(Player player, TriggersSet inputStateTriggerSet)
        {            
            ControlInputTypes currentFlagInputState = ControlInputTypes.None;
            if (inputStateTriggerSet.Up)
            {
                currentFlagInputState |= ControlInputTypes.Up;
            }

            if (inputStateTriggerSet.Down)
            {
                currentFlagInputState |= ControlInputTypes.Down;
            }

            if (inputStateTriggerSet.Left)
            {
                currentFlagInputState |= ControlInputTypes.Left;
            }

            if (inputStateTriggerSet.Right)
            {
                currentFlagInputState |= ControlInputTypes.Right;
            }

            if (inputStateTriggerSet.MouseLeft)
            {
                currentFlagInputState |= ControlInputTypes.LightAttack;
            }

            if (inputStateTriggerSet.MouseRight)
            {
                currentFlagInputState |= ControlInputTypes.HeavyAttack;
            }

            // if the current input state contains literally anything
            if (currentFlagInputState.HasFlag(ControlInputTypes.Any))
            {
                // remove the "none" state.
                currentFlagInputState = currentFlagInputState & ~ControlInputTypes.None;
            }

            return currentFlagInputState;
        }
    }    
}