using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBZMOD.Enums
{
    // monolith of player vars that might need syncing, making it slightly easier to sync for specific objects.
    public enum PlayerVarSyncEnum
    {
        KiMax2,
        KiMax3,
        KiMaxMult,
        KiCurrent,
        IsTransforming,
        Fragment1,
        Fragment2,
        Fragment3,
        Fragment4,
        Fragment5,
        IsCharging,
        JungleMessage,
        HellMessage,
        EvilMessage,
        MushroomMessage,
        IsHoldingKiWeapon,
        traitChecked,
        playerTrait,
        IsFlying,
        IsTransformationAnimationPlaying,
        TriggerMouseLeft,
        TriggerMouseRight,        
        TriggerLeft,
        TriggerRight,
        TriggerUp,
        TriggerDown,
        ChargeMoveSpeed,
        BonusSpeedMultiplier,
        MouseWorldOctant,
        PowerWishesLeft,
        HeldProjectile,
        WishActive,
        FacingDirection
    }
}
