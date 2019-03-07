using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Social;

namespace DBZMOD.Transformations.DeveloperForms.Webmilio
{
    public sealed class SoulStealerTransformation : TransformationDefinition
    {
        public SoulStealerTransformation(params TransformationDefinition[] parents) : base("Soul Stealer", Color.DarkViolet,
            1f, 1f, 0, 1f, 0f, 0f, 0f, 
            new TransformationAppearanceDefinition(AuraAnimations.soulStealerAura, 
                new ReadOnlyColor(Color.DarkViolet.R / 255f, Color.DarkViolet.G / 255f, Color.DarkViolet.B / 255f), null, new ReadOnlyColor(Color.DarkViolet.R, Color.DarkViolet.G, Color.DarkViolet.B), null, null, Color.DarkViolet),
            typeof(SoulStealerBuff),
            buffIconGetter: () => GFX.soulStealerButtonImage, hasMenuIcon: true, canBeMastered: true,
            parents: parents)
        {
        }

        public override void OnPlayerLoad(MyPlayer player, TagCompound tag)
        {
            if (IsCorrectPlayer() && !player.HasTransformation(this))
                player.PlayerTransformations.Add(this, new PlayerTransformation(this));

            if (!IsCorrectPlayer() && player.HasTransformation(this))
                player.PlayerTransformations.Remove(this);
        }

        public override void OnPlayerSave(MyPlayer player, TagCompound tag)
        {
            
        }

        public override void OnPlayerHitAnything(MyPlayer player, float x, float y, Entity victim)
        {
            NPC npc = victim as NPC;

            if (npc != null && !npc.active)
            {
                float gain = (npc.lifeMax / (float) player.player.statLifeMax2) / 1100;
                SetMobKilledMultiplier(player, GetMobKilledMultiplier(player) + gain);
            }
        }

        public override float GetDamageMultiplier(MyPlayer player)
        {
            return base.ModifiedDamageMultiplier + GetMobKilledMultiplier(player);
        }

        public float GetMobKilledMultiplier(MyPlayer player)
        {
            string mobKilledMultiplier = this.UnlocalizedName + "_" + "MobKilledMultiplier";
            PlayerTransformation playerTransformation = player.PlayerTransformations[this];

            if (!playerTransformation.ExtraInformation.ContainsKey(mobKilledMultiplier))
                playerTransformation.ExtraInformation.Add(mobKilledMultiplier, 0f);

            return (float) playerTransformation.ExtraInformation[mobKilledMultiplier];
        }

        public void SetMobKilledMultiplier(MyPlayer player, float multiplier)
        {
            string mobKilledMultiplier = this.UnlocalizedName + "_" + "MobKilledMultiplier";
            PlayerTransformation playerTransformation = player.PlayerTransformations[this];

            if (!playerTransformation.ExtraInformation.ContainsKey(mobKilledMultiplier))
                playerTransformation.ExtraInformation.Add(mobKilledMultiplier, 0f);

            playerTransformation.ExtraInformation[mobKilledMultiplier] = multiplier;
        }

        public override bool DoesShowInMenu(MyPlayer player) => IsCorrectPlayer();

        private bool IsCorrectPlayer() => SteamHelper.SteamID64 == "76561198046878487";
    }

    public sealed class SoulStealerBuff : TransformationBuff
    {
        public SoulStealerBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.SoulStealerDefinition)
        {
        }
    }
}
