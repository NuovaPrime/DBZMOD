using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.TestItems
{
    public class UIOmenTestItem : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.consumable = false;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.expert = true;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("UI Omen Test Item");
            Tooltip.SetDefault("Activates Ultra Instinct -Sign-\nWIP, this is just to show the aura sprite.");
        }


        public override bool UseItem(Player player)
        {
            player.AddBuff(mod.BuffType("UIOmenBuff"), 180000);
            Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("FalseUIAuraProj"), 0, 0, player.whoAmI);
            return true;

        }
    }
}
