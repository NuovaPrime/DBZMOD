using DBZMOD.Enums;
using Microsoft.Xna.Framework;

namespace DBZMOD.Models
{
    // helper class for storing all the details about a transformation that need to be referenced later.
    public class BuffInfo
    {
        public string buffKeyName;
        public MenuSelectionID menuId;
        public string transformationText;
        public Color transformationTextColor;

        public BuffInfo(MenuSelectionID menuId, string buffKey, string transText, Color transTextColor)
        {
            this.menuId = menuId;
            this.buffKeyName = buffKey;
            this.transformationText = transText;
            this.transformationTextColor = transTextColor;
        }

        public int GetBuffId()
        {
            return DBZMOD.instance.BuffType(this.buffKeyName);
        }
    }
}
