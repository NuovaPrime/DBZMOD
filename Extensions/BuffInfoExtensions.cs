using System.Linq;
using DBZMOD.Models;
using DBZMOD.Util;

namespace DBZMOD.Extensions
{
    /// <summary>
    ///     Class for extensions on our custom transformation buff model, BuffInfo
    /// </summary>
    public static class BuffInfoExtensions
    {
        public static bool IsSsj(this BuffInfo buff)
        {
            return FormBuffHelper.ssjBuffs.Contains(buff);
        }

        public static bool IsSpectrum(this BuffInfo buff)
        {
            return FormBuffHelper.spectrum.GetBuffId() == buff.GetBuffId();
        }

        public static bool IsKaioken(this BuffInfo buff)
        {
            return buff.GetBuffId() == FormBuffHelper.kaioken.GetBuffId();
        }

        public static bool IsAnyKaioken(this BuffInfo buff)
        {
            return buff.IsKaioken() || buff.IsSuperKaioken();
        }

        public static bool IsSuperKaioken(this BuffInfo buff)
        {
            return buff.GetBuffId() == FormBuffHelper.superKaioken.GetBuffId();
        }

        public static bool IsDevBuffed(this BuffInfo buff)
        {
            return buff.IsSpectrum();
        }

        public static bool IsAnythingOtherThanKaioken(this BuffInfo buff)
        {
            return buff.IsLssj() || buff.IsSsj() || buff.IsSsjg() || buff.IsDevBuffed() || buff.IsAscended();
        }

        public static bool IsLssj(this BuffInfo buff)
        {
            return FormBuffHelper.legendaryBuffs.Contains(buff);
        }

        public static bool IsSsjg(this BuffInfo buff)
        {
            return FormBuffHelper.ssjg.GetBuffId() == buff.GetBuffId();
        }

        public static bool IsAscended(this BuffInfo buff)
        {
            return buff == FormBuffHelper.assj || buff == FormBuffHelper.ussj;
        }
    }
}