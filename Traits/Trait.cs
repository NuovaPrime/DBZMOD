using System;
using Terraria.ModLoader;

namespace DBZMOD.Traits
{
    public abstract class Trait : IHasUnlocalizedName
    {
        protected Trait(string unlocalizedName, string text, string description, float percentage, string buffName)
        {
            UnlocalizedName = unlocalizedName;
            Text = text;
            Description = description;

            Percentage = percentage;

            BuffName = buffName;
        }

        public virtual bool CanSee(MyPlayer player)
        {
            return true;
        }

        public virtual int GetBuffType(Mod mod) => mod.BuffType(BuffName);

        public string UnlocalizedName { get; }
        public string Text { get; }
        public string Description { get; }

        public float Percentage { get; }

        public string BuffName { get; }

        public bool Default { get; protected set; }
    }
}
