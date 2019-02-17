using System.Collections.Generic;

namespace DBZMOD.Dynamicity
{
    public class ManyToManyNode<T>
    {
        public ManyToManyNode()
        {
        }

        public ManyToManyNode(List<T> previous, T current, List<T> next)
        {
            Previous = previous;
            Current = current;
            Next = next;
        }

        public void AddPrevious(params T[] previous)
        {
            for (int i = 0; i < previous.Length; i++)
                if (!Previous.Contains(previous[i]))
                    Previous.Add(previous[i]);
        }

        public void AddPrevious(List<T> previous)
        {
            for (int i = 0; i < previous.Count; i++)
                if (!Previous.Contains(previous[i]))
                    Previous.Add(previous[i]);
        }

        public void AddNext(params T[] next)
        {
            for (int i = 0; i < next.Length; i++)
                if (!Next.Contains(next[i]))
                    Next.Add(next[i]);
        }

        public void AddNext(List<T> next)
        {
            for (int i = 0; i < next.Count; i++)
                if (!Next.Contains(next[i]))
                    Next.Add(next[i]);
        }

        public List<T> Previous { get; set; }

        public T Current { get; set; }

        public List<T> Next { get; set; }
    }
}
