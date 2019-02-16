using System.Collections.Generic;

namespace DBZMOD.Dynamicity
{
    public class ManyToManyNode<T> where T : class
    {
        public ManyToManyNode(T[] previous, T current, T[] next)
        {
            Previous = previous;
            Current = current;
            Next = next;
        }

        public T[] Previous { get; }

        public T Current { get; }

        public T[] Next { get; }
    }
}
