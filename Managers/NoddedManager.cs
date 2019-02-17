using DBZMOD.Dynamicity;
using DBZMOD.Transformations;

namespace DBZMOD.Managers
{
    public abstract class NoddedManager<T> : Manager<T>, ICanBuildNodeTree<T> where T : IHasUnlocalizedName, IHasParents<T>
    {
        private NodeTree<T> _tree;

        public NodeTree<T> BuildNodeTree() => NodeTree<T>.BuildTree(byIndex);

        public NodeTree<T> Tree => _tree ?? (_tree = BuildNodeTree());
    }
}
