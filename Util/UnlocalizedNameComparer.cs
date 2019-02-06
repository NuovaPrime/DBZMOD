using System.Collections.Generic;

namespace DBZMOD.Util
{
    public class UnlocalizedNameComparer : IEqualityComparer<IHasUnlocalizedName>
    {
        public bool Equals(IHasUnlocalizedName unlocalizedName1, IHasUnlocalizedName unlocalizedName2)
        {
            return string.Compare(unlocalizedName1.UnlocalizedName, unlocalizedName2.UnlocalizedName, true) == 0;
        }

        public int GetHashCode(IHasUnlocalizedName unlocalizedName)
        {
            return unlocalizedName.UnlocalizedName.GetHashCode();
        }
    }
}