using DBZMOD;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.BukuujutsuVols
{
    public class BukuujutsuVol1 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.consumable = true;
            item.maxStack = 1;
            if (!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Bookread").WithPitchVariance(.1f);
            }
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.rare = 4;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bukuujutsu Guide Vol 1 - Chorus Aeria");
            Tooltip.SetDefault("'It has an ancient technique inscribed in it, holding it makes you feel lighter.'");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).flightUnlocked = true;
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText("You have unlocked flight.\nBind a key to flight toggle to use it.");
                return true;
            }
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).flightUnlocked)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StableKiCrystal", 100);
            recipe.AddIngredient(ItemID.ManaCrystal, 3);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
