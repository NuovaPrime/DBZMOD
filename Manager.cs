using System.Collections.Generic;

namespace DBZMOD
{
    public abstract class Manager<T> where T: IHasUnlocalizedName
    {
        protected readonly List<T> byIndex = new List<T>();
        protected Dictionary<string, T> byNames = new Dictionary<string, T>();

        public bool Add(T item)
        {
            if (byIndex.Contains(item) || byNames.ContainsKey(item.UnlocalizedName)) return false;

            byIndex.Add(item);
            byNames.Add(item.UnlocalizedName, item);
            return true;
        }

        public T this[int index]
        {
            get { return byIndex[index]; }
        }

        public T this[string name]
        {
            get { return byNames[name]; }
        }
    }
}
