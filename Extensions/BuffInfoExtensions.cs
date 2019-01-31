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
            return FormBuffHelper.ssjBuffs.Contains(buff);
        }

        public static bool IsSpectrum(this TransformationDefinition buff)
        {
            return DBZMOD.instance.TransformationDefinitionManager.SpectrumDefinition.GetBuffId() == buff.GetBuffId();
        }

        public static bool IsKaioken(this TransformationDefinition buff)
        {
            return buff.GetBuffId() == DBZMOD.instance.TransformationDefinitionManager.KaiokenDefinition.GetBuffId();
        }

        public static bool IsAnyKaioken(this TransformationDefinition buff)
        {
            return buff.IsKaioken() || buff.IsSuperKaioken();
        }

        public static bool IsSuperKaioken(this TransformationDefinition buff)
        {
            return buff.GetBuffId() == DBZMOD.instance.TransformationDefinitionManager.SuperKaiokenDefinition.GetBuffId();
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
            return FormBuffHelper.legendaryBuffs.Contains(buff);
        }

        public static bool IsSSJG(this TransformationDefinition buff)
        {
            return DBZMOD.instance.TransformationDefinitionManager.SSJGDefinition.GetBuffId() == buff.GetBuffId();
        }

        public static bool IsAscended(this TransformationDefinition buff)
        {
            return buff == DBZMOD.instance.TransformationDefinitionManager.ASSJDefinition || buff == DBZMOD.instance.TransformationDefinitionManager.USSJDefinition;
        }
    }
}