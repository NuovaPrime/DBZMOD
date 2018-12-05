using Terraria;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD.Prefixes
{
    public class BasePrefix : ModPrefix
    {
        public virtual void SetDefaults()
        {
            DisplayName.SetDefault("BasePrefix");
        }

        public virtual float RollChance(Item item)
        {
            return 3f;
        }

        public virtual bool CanRoll(Item item)
        {
            if (item.modItem is KiItem)
            {
                return true;
            }
            return false;
        }

        public override PrefixCategory Category { get { return PrefixCategory.AnyWeapon; } }

        public override void Apply(Item item)
        {
            //apply normal modifiers
            ApplyItemModifier(item);

            //only apply ki item modifiers if its a ki item.
            if (item.modItem is KiItem)
            {
                ApplyKiItemModifier(item);
            }
        }

        //this will apply a modifier as just a regular item
        public virtual void ApplyItemModifier(Item item) 
        {
            //Empty, can be overriden
        }

        //you apply ki item only modifiers here
        public virtual void ApplyKiItemModifier(Item item) 
        {
            //Empty, can be overriden
        }

    }
}
