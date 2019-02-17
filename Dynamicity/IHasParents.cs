namespace DBZMOD.Dynamicity
{
    public interface IHasParents<T>
    {
        T[] Parents { get; }
    }
}