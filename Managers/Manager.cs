using System.Collections.Generic;

namespace DBZMOD.Managers
{
    public abstract class Manager<T> where T : IHasUnlocalizedName
    {
        protected readonly List<T> byIndex = new List<T>();
        protected readonly Dictionary<string, T> byNames = new Dictionary<string, T>();

        public virtual bool Add(T item)
        {
            if (byIndex.Contains(item) || byNames.ContainsKey(item.UnlocalizedName)) return false;

            byIndex.Add(item);
            byNames.Add(item.UnlocalizedName, item);
            return true;
        }

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
