namespace DBZMOD.Dynamicity
{
    public interface IHasParents<out T>
    {
        T[] Parents { get; }
    }
}