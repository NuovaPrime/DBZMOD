namespace DBZMOD.Dynamicity
{
    public interface ICanBuildNodeTree<T> where T : IHasParents<T>
    {
        NodeTree<T> BuildNodeTree();
    }
}