using DBZMOD.Enums;
using Microsoft.Xna.Framework;

namespace DBZMOD.Transformations
{
    // helper class for storing all the details about a transformation that need to be referenced later.
    public class TransformationDefinition : IHasUnlocalizedName
    {
        /// <summary>
        ///     Instantiate a new buff info, typically a form like ssj or kaioken.
        /// </summary>
        /// <param name="menuId">The menu Id of the form, if it exists in the menu.</param>
        /// <param name="buffKey">The key name of the buff used in the mod.</param>
        /// <param name="transText">The text displayed when transforming.</param>
        /// <param name="transTextColor">The color of the transformation text.</param>
        /// <param name="canBeMastered">Whether the form has a mastery rating.</param>
        /// <param name="masterFormBuffKeyName">What form the buff's ki cost is associated with for mastery (like ASSJ and USSJ being SSJ1, for example)</param>
        public TransformationDefinition(MenuSelectionID menuId, string buffKey, string transText, Color transTextColor, bool canBeMastered = false, string masterFormBuffKeyName = null)
        {
            UnlocalizedName = buffKey;

            MenuId = menuId;
            TransformationText = transText;
            TransformationTextColor = transTextColor;
            HasMastery = canBeMastered;
            MasteryBuffKeyName = masterFormBuffKeyName;
        }

        public int GetBuffId()
        {
            return DBZMOD.instance.BuffType(this.UnlocalizedName);
        }

        public string UnlocalizedName { get; }

        public MenuSelectionID MenuId { get; }

        public string TransformationText { get; }

        public bool HasMastery { get; }

        public Color TransformationTextColor { get; }

        public string MasteryBuffKeyName { get; }
    }
}
