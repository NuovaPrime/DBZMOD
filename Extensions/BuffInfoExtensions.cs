using System.Linq;
using DBZMOD.Models;
using DBZMOD.Transformations;
using DBZMOD.Util;

namespace DBZMOD.Extensions
{
    /// <summary>
    ///     Class for extensions on our custom transformation buff model, BuffInfo
    /// </summary>
    public static class BuffInfoExtensions
    {
        public static bool IsSSJ(this TransformationDefinition buff)
        {
            return FormBuffHelper.ssjBuffs.Any(x => x.Equals(buff));
        }

        public static bool IsSpectrum(this TransformationDefinition buff)
        {
            return buff.Equals(DBZMOD.instance.TransformationDefinitionManager.SpectrumDefinition);
        }

        public static bool IsKaioken(this TransformationDefinition buff)
        {
            return buff.Equals(DBZMOD.instance.TransformationDefinitionManager.KaiokenDefinition);
        }

        public static bool IsAnyKaioken(this TransformationDefinition buff)
        {
            return buff.IsKaioken() || buff.IsSuperKaioken();
        }

        public static bool IsSuperKaioken(this TransformationDefinition buff)
        {
            return buff.Equals(DBZMOD.instance.TransformationDefinitionManager.SuperKaiokenDefinition);
        }

        public static bool IsDevBuffed(this TransformationDefinition buff)
        {
            return buff.IsSpectrum();
        }

        public static bool IsAnythingOtherThanKaioken(this TransformationDefinition buff)
        {
            return buff.IsLSSJ() || buff.IsSSJ() || buff.IsSSJG() || buff.IsDevBuffed() || buff.IsAscended();
        }

        public static bool IsLSSJ(this TransformationDefinition buff)
        {
            return FormBuffHelper.legendaryBuffs.Any(x => x.Equals(buff));
        }

        public static bool IsSSJG(this TransformationDefinition buff)
        {
            return buff.Equals(DBZMOD.instance.TransformationDefinitionManager.SSJGDefinition);
        }

        public static bool IsAscended(this TransformationDefinition buff)
        {
            return buff.Equals(DBZMOD.instance.TransformationDefinitionManager.ASSJDefinition) || buff.Equals(DBZMOD.instance.TransformationDefinitionManager.USSJDefinition);
        }
    }
}