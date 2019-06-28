using Leveled;
using Leveled.NPCs;
using Leveled.Players;
#pragma warning disable 612

namespace DBZMOD.ModSupport
{
    internal class LeveledSupport
    {
        public static void Initialize()
        {
            if (!LeveledNPCStats.temporaryStrVarHooks.ContainsKey(typeof(KiProjectile)))
                LeveledNPCStats.temporaryStrVarHooks.Add(typeof(KiProjectile), player => (int) player.Spirit.Total);
        }

        public static void PlayerPreUpdateMovement(MyPlayer player)
        {
            LeveledPlayer leveledPlayer = player.player.GetModPlayer<LeveledPlayer>();

            player.kiDamage *= LeveledMain.DamageMultiplier((int) leveledPlayer.Spirit.Total, leveledPlayer.Level);
        }
    }
} 
