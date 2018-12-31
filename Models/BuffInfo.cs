using DBZMOD.Enums;
using Microsoft.Xna.Framework;

namespace DBZMOD.Models
{
    // helper class for storing all the details about a transformation that need to be referenced later.
    public class BuffInfo
    {
        public string BuffKeyName;
        public MenuSelectionID MenuId;
        public string TransformationText;
        public Color TransformationTextColor;

        public BuffInfo(MenuSelectionID menuId, string buffKey, string transText, Color transTextColor)
        {
            this.MenuId = menuId;
            this.BuffKeyName = buffKey;
            this.TransformationText = transText;
            this.TransformationTextColor = transTextColor;
        }

        public int GetBuffId()
        {
            return DBZMOD.instance.BuffType(this.BuffKeyName);
        }
    }
}
