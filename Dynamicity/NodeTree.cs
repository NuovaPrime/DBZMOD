using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DBZMOD.Dynamicity
{
    public class NodeTree<T> where T : IHasParents<T>
    {
        protected NodeTree(Dictionary<T, ManyToManyNode<T>> tree)
        {
            Tree = tree;
        }

        public static NodeTree<T> BuildTree(List<T> items)
        {
            Dictionary<T, ManyToManyNode<T>> tree = new Dictionary<T, ManyToManyNode<T>>();

            foreach (T item in items)
            {
                if (!tree.ContainsKey(item))
                    tree.Add(item, new ManyToManyNode<T>(item));

                tree[item].AddPrevious(item.Parents);

                foreach (T parent in item.Parents)
                {
                    if (!tree.ContainsKey(parent))
                        tree.Add(parent, new ManyToManyNode<T>(parent));

                    tree[parent].AddNext(item);
                }
            }

            return new NodeTree<T>(tree);
        }

        public bool ContainsKey(T key) => Tree.ContainsKey(key);

        #region Debug Methods

        /// <summary>Prints the Tree in chat starting at the root.</summary>
        internal void PrintTree()
        {
            foreach (ManyToManyNode<T> mtmn in Tree.Values)
            {
                if (mtmn.Previous.Count > 0) continue;

                PrintTree(mtmn.Current);
            }
        }

        /// <summary>Prints a Tree in chat starting at the given <see cref="T"/>.</summary>
        /// <param name="def">The <see cref="T"/> to start at.</param>
        internal void PrintTree(T def)
        {
            ManyToManyNode<T> mtmn = this[def];
            Terraria.Main.NewText(mtmn.Current);

            foreach (T td in mtmn.Next)
                PrintTree(td);
        }

        #endregion

        public int Count => Tree.Count;

        internal Dictionary<T, ManyToManyNode<T>> Tree { get; set; }

        public ManyToManyNode<T> this[T item]
        {
            get { return Tree[item]; }
        }

        public ManyToManyNode<T> this[int index]
        {
            get
            {
                int current = 0;

                foreach (ManyToManyNode<T> node in Tree.Values)
                {
                    if (current < index) // I didn't write current++ because ReSharper is telling me its not being used.
                    {
                        current++;
                        continue;
                    } 

                    return node;
                }

                throw new IndexOutOfRangeException();
            }
        }
    }
}
