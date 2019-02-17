using DBZMOD.Dynamicity;
using DBZMOD.Transformations;

namespace DBZMOD.Managers
{
    public abstract class NoddedManager<T> : Manager<T>, ICanBuildNodeTree<T> where T : IHasUnlocalizedName, IHasParents<T>
    {
        private bool _treeUpToDate = true;
        private NodeTree<T> _tree;

        public override bool Add(T item)
        {
            _treeUpToDate = false;
            return base.Add(item);
        }

        internal override void Clear()
        {
            _tree = null;
            _treeUpToDate = false;

            base.Clear();
        }

        public NodeTree<T> BuildNodeTree() => NodeTree<T>.BuildTree(byIndex);

        public NodeTree<T> Tree
        {
            get
            {
                if (_tree == null || !_treeUpToDate)
                {
                    _tree = BuildNodeTree();
                    _treeUpToDate = true;
                }

                return _tree;
            }
        }
    }
}
