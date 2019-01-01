using DBZMOD;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.Util;

namespace DBZMOD
{
    public abstract class BaseBeamItem : KiItem
    {
        public override void SetDefaults()
        {
            item.shootSpeed = 0f;
            item.damage = 0;
            item.knockBack = 2f;
            item.useStyle = 5;
            item.UseSound = SoundID.Item12;
            item.channel = true;
            item.useAnimation = 2;
            item.useTime = 2;
            item.width = 40;
            item.noUseGraphic = true;
            item.height = 40;
            item.autoReuse = false;            
            item.value = 120000;
            item.rare = 8;            
            KiDrain = 1;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Abstract Beam Item");
            DisplayName.SetDefault("Base Beam Item");
        }

        public override void HoldItem(Player player)
        {
            // set the ki weapon held var
            player.GetModPlayer<MyPlayer>().IsHoldingKiWeapon = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            player.channel = true;
            if (!ProjectileUtil.RecapturePlayerProjectile(player, item.shoot))
            {
                var proj = Projectile.NewProjectileDirect(player.position, player.position, item.shoot, item.damage, item.knockBack, player.whoAmI);
                player.heldProj = proj.whoAmI;
            }
            return base.AltFunctionUse(player);
        }

        // this is important, don't let the player left click, basically.
        public override bool CanUseItem(Player player)
        {
            return false;
        }

        // this is important, don't let the player left click, basically. We spawn the projectile manually because of controls.
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return false;
        }

        
    }
}
