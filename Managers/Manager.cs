using System.Collections.Generic;

namespace DBZMOD.Managers
{
    public abstract class Manager<T> where T : IHasUnlocalizedName
    {
        protected readonly List<T> byIndex = new List<T>();
        protected readonly Dictionary<string, T> byNames = new Dictionary<string, T>();

        public virtual T Add(T item)
        {
            if (byIndex.Contains(item) || byNames.ContainsKey(item.UnlocalizedName)) return byNames[item.UnlocalizedName];

            byIndex.Add(item);
            byNames.Add(item.UnlocalizedName, item);
            return item;
        }

        public virtual bool Remove(T item)
        {
            if (!byIndex.Contains(item)) return false;

            byIndex.Remove(item);
            byNames.Remove(item.UnlocalizedName);
            return true;
        }

        public virtual bool Contains(T item) => byIndex.Contains(item);

        public virtual bool Contains(string unlocalizedName) => byNames.ContainsKey(unlocalizedName);

        internal virtual void DefaultInitialize()
        {
            Initialized = true;
        }

        public int GetIndex(T item) => byIndex.IndexOf(item);
        public int GetIndex(string unlocalizedName) => GetIndex(byNames[unlocalizedName]);

        internal virtual void Clear()
        {
            byIndex.Clear();
            byNames.Clear();
        }

        public T this[int index] => byIndex[index];

        public T this[string name] => byNames[name];

        public int Count => byIndex.Count;

        public bool Initialized { get; private set; }
    }
}
