using System.Collections.Generic;
using DBZMOD.Effects.Animations.Aura;
using DBZMOD.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.IO;

namespace DBZMOD.Transformations.DeveloperForms.Webmilio
{
    public sealed class SoulStealerTransformation : TransformationDefinition
    {
        private const string
            SOULSTEALER_PREFIX = "SoulStealer_",
            SOULPOWER_TAG = SOULSTEALER_PREFIX + "SoulPowerCount",
            DIMINISHINGRETURNS_MOBCOUNT_PREFIX = SOULSTEALER_PREFIX + "DiminishingReturns_MobCount";

        public SoulStealerTransformation(params TransformationDefinition[] parents) : base("Soul Stealer", Color.DarkViolet,
            1f, 1f, 0, 1f, 0f, 0f, 0f,
            new TransformationAppearanceDefinition(AuraAnimations.soulStealerAura,
                new ReadOnlyColor(Color.DarkViolet.R / 100f, Color.DarkViolet.G / 100f, Color.DarkViolet.B / 100f), null, new ReadOnlyColor(Color.DarkViolet.R, Color.DarkViolet.G, Color.DarkViolet.B), null, null, Color.DarkViolet),
            typeof(SoulStealerBuff),
            buffIconGetter: () => GFX.soulStealerButtonImage, hasMenuIcon: true, canBeMastered: true,
            parents: parents)
        {
        }

        public override void OnPlayerLoad(MyPlayer player, TagCompound tag)
        {
            if (CheckPrePlayerConditions() && !player.HasTransformation(this))
                player.PlayerTransformations.Add(this, new PlayerTransformation(this));
            if (!CheckPrePlayerConditions() && player.HasTransformation(this))
                player.PlayerTransformations.Remove(this);

            PlayerTransformation playerTransformation = player.PlayerTransformations[this];

            if (playerTransformation != null)
            {
                if (!playerTransformation.ExtraInformation.ContainsKey(SOULPOWER_TAG))
                    playerTransformation.ExtraInformation.Add(SOULPOWER_TAG, 0f);

                if (!playerTransformation.ExtraInformation.ContainsKey(DIMINISHINGRETURNS_MOBCOUNT_PREFIX))
                    playerTransformation.ExtraInformation.Add(DIMINISHINGRETURNS_MOBCOUNT_PREFIX, new Dictionary<string, int>());

                foreach (KeyValuePair<string, object> kvp in tag)
                {
                    if (!kvp.Key.StartsWith(DIMINISHINGRETURNS_MOBCOUNT_PREFIX)) continue;

                    ((Dictionary<string, int>)playerTransformation.ExtraInformation[DIMINISHINGRETURNS_MOBCOUNT_PREFIX]).Add(kvp.Key.Substring(DIMINISHINGRETURNS_MOBCOUNT_PREFIX.Length), int.Parse(kvp.Value.ToString()));
                }

                SetSoulPower(player, tag.GetFloat(SOULPOWER_TAG));
            }
        }

        public override void OnPlayerSave(MyPlayer player, TagCompound tag)
        {
            tag.Add(SOULPOWER_TAG, GetSoulPower(player));
            PlayerTransformation playerTransformation = player.PlayerTransformations[this];

            Dictionary<string, int> mobCount = (Dictionary<string, int>) playerTransformation.ExtraInformation[DIMINISHINGRETURNS_MOBCOUNT_PREFIX];

            foreach (KeyValuePair<string, int> kvp in mobCount)
                tag.Add(DIMINISHINGRETURNS_MOBCOUNT_PREFIX + kvp.Key, kvp.Value);
        }

        public override void OnPlayerKillAnything(MyPlayer player, NPC npc)
        {
            Dictionary<string, int> dimishingReturnsDictionary = GetDiminishingReturnsDictionary(player);
            string npcType = npc.TypeName.Replace(" ", "");

            if (!dimishingReturnsDictionary.ContainsKey(npcType))
                dimishingReturnsDictionary.Add(npcType, 1);

            dimishingReturnsDictionary[npcType]++;
            AddSoulPower(player, npc);
        }

        public override float GetDamageMultiplier(MyPlayer player)
        {
            return base.ModifiedDamageMultiplier + GetSoulPower(player) / 1100;
        }

        public override float GetSpeedMultiplier(MyPlayer player)
        {
            return base.ModifiedSpeedMultiplier + GetSoulPower(player) / 7700;
        }

        public float GetSoulPower(MyPlayer player) => (float)player.PlayerTransformations[this].ExtraInformation[SOULPOWER_TAG];

        public void SetSoulPower(MyPlayer player, float multiplier) => player.PlayerTransformations[this].ExtraInformation[SOULPOWER_TAG] = multiplier;

        public void AddSoulPower(MyPlayer player, NPC npc)
        {
            float gain = (npc.lifeMax / (float) player.player.statLifeMax2) / GetMobKilledCount(player, npc.TypeName.Replace(" ", ""));

            if (npc.boss && gain < 110)
                gain = 110;
            else if (gain < 1)
                gain = 1;

            SetSoulPower(player, GetSoulPower(player) + gain);
        }
        
        public override bool DoesShowInMenu(MyPlayer player) => CheckPrePlayerConditions();

        public override bool CheckPrePlayerConditions() => SteamHelper.SteamID64 == "76561198046878487";

        private int GetMobKilledCount(MyPlayer player, string npcTypeName) => GetDiminishingReturnsDictionary(player)[npcTypeName];

        public Dictionary<string, int> GetDiminishingReturnsDictionary(MyPlayer player) => ((Dictionary<string, int>) player.PlayerTransformations[this].ExtraInformation[DIMINISHINGRETURNS_MOBCOUNT_PREFIX]);
    }

    public sealed class SoulStealerBuff : TransformationBuff
    {
        public SoulStealerBuff() : base(DBZMOD.Instance.TransformationDefinitionManager.SoulStealerDefinition)
        {
        }

        public override string AssembleTransBuffDescription(MyPlayer player) => base.AssembleTransBuffDescription(player) + "\nSoul Power: " + ((SoulStealerTransformation)TransformationDefinition).GetSoulPower(player);
    }
}
