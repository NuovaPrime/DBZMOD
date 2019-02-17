using System.Collections.Generic;

namespace DBZMOD.Dynamicity
{
    public class NodeTree
    {
        public static Dictionary<T, ManyToManyNode<T>> BuildTree<T>(List<T> items) where T : class, IHasParents<T>
        {
            Dictionary<T, ManyToManyNode<T>> tree = new Dictionary<T, ManyToManyNode<T>>();

            foreach (T item in items)
            {
                if (!tree.ContainsKey(item))
                    tree.Add(item, new ManyToManyNode<T>());

                tree[item].AddPrevious(item.Parents);
                tree[item].Current = item;

                for (int i = 0; i < item.Parents.Length; i++)
                    tree[item.Parents[i]].AddNext(item);
            }

            return tree;
        }
    }
}
