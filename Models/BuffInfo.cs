using DBZMOD.Enums;
using Microsoft.Xna.Framework;

namespace DBZMOD.Models
{
    // helper class for storing all the details about a transformation that need to be referenced later.
    public class BuffInfo
    {
        public string buffKeyName;
        public MenuSelectionID menuId;
        public string displayName;
        public Color transformationTextColor;
        public bool hasMastery;
        public string masteryBuffKeyName;

        /// <summary>
        ///     Instantiate a new buff info, typically a form like ssj or kaioken.
        /// </summary>
        /// <param name="menuId">The menu Id of the form, if it exists in the menu.</param>
        /// <param name="buffKey">The key name of the buff used in the mod.</param>
        /// <param name="transText">The text displayed when transforming.</param>
        /// <param name="transTextColor">The color of the transformation text.</param>
        /// <param name="canBeMastered">Whether the form has a mastery rating.</param>
        /// <param name="masterFormBuffKeyName">What form the buff's ki cost is associated with for mastery (like ASSJ and USSJ being SSJ1, for example)</param>
        public BuffInfo(MenuSelectionID menuId, string buffKey, string transText, Color transTextColor, bool canBeMastered = false, string masterFormBuffKeyName = null)
        {
            this.menuId = menuId;
            this.buffKeyName = buffKey;
            this.displayName = transText;
            this.transformationTextColor = transTextColor;
            this.hasMastery = canBeMastered;
            this.masteryBuffKeyName = masterFormBuffKeyName;
        }

        public int GetBuffId()
        {
            return DBZMOD.instance.BuffType(this.buffKeyName);
        }
    }
}
