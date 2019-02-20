using System.Linq;
using DBZMOD.Transformations;
using DBZMOD.Utilities;

namespace DBZMOD.Extensions
{
    /// <summary>
    ///     Class for extensions on our custom transformation buff model, BuffInfo
    /// </summary>
    public static class BuffInfoExtensions
    {
        public static bool IsSSJ(this TransformationDefinition buff)
        {
            return FormBuffHelper.ssjBuffs.Any(x => x == buff);
        }

        public static bool IsSpectrum(this TransformationDefinition buff)
        {
            return buff == DBZMOD.Instance.TransformationDefinitionManager.SpectrumDefinition;
        }

        public static bool IsKaioken(this TransformationDefinition buff)
        {
            return buff == DBZMOD.Instance.TransformationDefinitionManager.KaiokenDefinition;
        }

        public static bool IsAnyKaioken(this TransformationDefinition buff)
        {
            return buff.IsKaioken() || buff.IsSuperKaioken();
        }

        public static bool IsSuperKaioken(this TransformationDefinition buff)
        {
            return buff == DBZMOD.Instance.TransformationDefinitionManager.SuperKaiokenDefinition;
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
            return FormBuffHelper.legendaryBuffs.Any(x => x == buff);
        }

        public static bool IsSSJG(this TransformationDefinition buff)
        {
            return buff == DBZMOD.Instance.TransformationDefinitionManager.SSJGDefinition;
        }

        public static bool IsAscended(this TransformationDefinition buff)
        {
            return buff == DBZMOD.Instance.TransformationDefinitionManager.ASSJDefinition || buff == DBZMOD.Instance.TransformationDefinitionManager.USSJDefinition;
        }
    }
}