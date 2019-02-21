using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Traits
{
    public abstract class TraitBuff : ModBuff
    {
        protected TraitBuff(Trait trait)
        {
            Trait = trait;
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault(Trait.Text);
            Description.SetDefault(Trait.Description);

            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public Trait Trait { get; }
    }
}
