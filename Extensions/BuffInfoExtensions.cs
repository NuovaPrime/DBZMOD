using DBZMOD.Transformations;

namespace DBZMOD.Extensions
{
    /// <summary>
    ///     Class for extensions on our custom transformation buff model, BuffInfo
    /// </summary>
    public static class BuffInfoExtensions
    {
        public static bool IsAscended(this TransformationDefinition buff)
        {
            return buff == DBZMOD.Instance.TransformationDefinitionManager.ASSJDefinition || buff == DBZMOD.Instance.TransformationDefinitionManager.USSJDefinition;
        }
    }
}