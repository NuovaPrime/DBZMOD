using DBZMOD;
using DBZMOD.Projectiles.Auras;
using Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace Util
{
    // class for helping out with all the buff integers, lists of buffs in order, presence of buffs/abstraction
    public static class Transformations
    {
        private static Mod mod = null;

        // the following are just convenience variables to help store the auto-generated Buff IDs so they can be referenced later in plain english.
        public static int SSJ1
        {
            get
            {
                return mod.BuffType("SSJ1Buff");
            }
        }

        public static int SSJ1Kaioken
        {
            get
            {
                return mod.BuffType("SSJ1KaiokenBuff");
            }
        }
        
        public static int SSJ2
        {
            get
            {
                return mod.BuffType("SSJ2Buff");
            }
        }

        public static int SSJ3
        {
            get
            {
                return mod.BuffType("SSJ3Buff");
            }
        }

        public static int SSJG
        {
            get
            {
                return mod.BuffType("SSJGBuff");
            }
        }

        public static int SSJB
        {
            get
            {
                return mod.BuffType("SSJBBuff");
            }
        }

        public static int LSSJ
        {
            get
            {
                return mod.BuffType("LSSJBuff");
            }
        }

        public static int LSSJ2
        {
            get
            {
                return mod.BuffType("LSSJ2Buff");
            }
        }

        public static int ASSJ
        {
            get
            {
                return mod.BuffType("ASSJBuff");
            }
        }

        public static int USSJ
        {
            get
            {
                return mod.BuffType("USSJBuff");
            }
        }

        public static int Kaioken
        {
            get
            {
                return mod.BuffType("KaiokenBuff");
            }
        }

        public static int Kaioken3
        {
            get
            {
                return mod.BuffType("KaiokenBuffX3");
            }
        }

        public static int Kaioken10
        {
            get
            {
                return mod.BuffType("KaiokenBuffX10");
            }
        }

        public static int Kaioken20
        {
            get
            {
                return mod.BuffType("KaiokenBuffX20");
            }
        }

        public static int Kaioken100
        {
            get
            {
                return mod.BuffType("KaiokenBuffX100");
            }
        }

        // called at mod entry point to give this static class a reference to the mod's many things.
        public static void Initialize(Mod initMod)
        {
            mod = initMod;
        }

        // returns a list of transformation steps specific to non-legendary SSJ players
        public static int[] TransformationSSJSteps()
        {
            int[] buffs = { SSJ1, SSJ2, SSJ3, SSJG };
            return buffs;
        }

        // A list of the "normal" SSJ transformation steps (1, 2, 3, G for now.)
        public static List<int> SSJBuffs()
        {
            return new List<int>(TransformationSSJSteps());
        }

        // a list of transformation steps specific to kaioken usage
        public static int[] TransformationKaiokenSteps()
        {
            int[] buffs = { Kaioken, Kaioken3, Kaioken10, Kaioken20, Kaioken100 };
            return buffs;
        }

        // A list of the kaioken transformation steps.
        public static List<int> KaiokenBuffs()
        {
            return new List<int>(TransformationKaiokenSteps());
        }

        // a list of transformation steps specific to legendary SSJ players
        public static int[] TransformationLegendarySteps()
        {
            int[] buffs = { SSJ1, LSSJ, LSSJ2 };
            return buffs;
        }

        // a list of transformation steps from SSJ1 through ascended SSJ forms
        public static int[] TransformationAscensionSteps()
        {
            int[] buffs = { SSJ1, ASSJ, USSJ };
            return buffs;
        }

        // A list of the ascended transformation steps.
        public static List<int> AscensionBuffs()
        {
            return new List<int>(TransformationAscensionSteps());
        }

        // A list of the legendary transformation steps.
        public static List<int> LegendaryBuffs()
        {
            return new List<int>(TransformationLegendarySteps()).ToList();
        }

        // A bunch of lists joined together, in order to contain every possible transformation buff.
        // when adding new ones, make sure they wind up here in some form, even if you just have to add them one at a time.
        // (Union() excludes duplicates automatically)
        public static List<int> AllBuffs()
        {
            var almostAllBuffs = LegendaryBuffs().Union(SSJBuffs()).Union(KaiokenBuffs()).Union(AscensionBuffs()).ToList();
            almostAllBuffs.Add(SSJ1Kaioken); // kaioken doesn't really fit in anywhere, so it gets added last.
            return almostAllBuffs;
        }

        // whether the player is in any of the transformation states. Relies on AllBuffs() containing every possible transformation buff.
        public static bool IsPlayerTransformed(Player player)
        {
            foreach(int buffId in AllBuffs())
            {
                if (player.HasBuff(buffId))
                    return true;
            }

            return false;
        }

        // returns steps specific to combining SSJ with Kaioken, there's literally two steps. You always step down to ssj1.
        public static int[] TransformationSSJKaiokenSteps()
        {
            int[] buffs = { SSJ1, SSJ1Kaioken };
            return buffs;
        }

        // whether the buff ID is one of the Kaioken states, this excludes SSJ1Kaioken for Next/Previous behavior reasons.
        public static bool IsKaioken(int buff)
        {
            return KaiokenBuffs().Contains(buff);
        }

        // whether the player is in one of the Kaioken states, this excludes SSJ1Kaioken for Next/Previous behavior reasons.
        public static bool IsKaioken(Player player)
        {
            foreach(int buffId in KaiokenBuffs())
            {
                if (player.HasBuff(buffId))
                    return true;
            }

            return false;
        }

        // whether the buff ID is one of the legendary states; caution, this includes SSJ1 for "Next/Previous" behavior reasons.
        public static bool IsLegendary(int buff)
        {
            return LegendaryBuffs().Contains(buff);
        }

        // whether the player is in one of the legendary states; caution, this includes SSJ1 for "Next/Previous" behavior reasons.
        public static bool IsLegendary(Player player)
        {
            foreach (int buffId in LegendaryBuffs())
            {
                if (player.HasBuff(buffId))
                    return true;
            }

            return false;
        }

        // whether the buff Id is one of the "normal" SSJ States (1, 2, 3, G)
        public static bool IsSSJ(int buff)
        {
            return SSJBuffs().Contains(buff);
        }

        // whether the player is in *any* of the "normal" SSJ states (1, 2, 3, G)
        public static bool IsSSJ(Player player)
        {
            foreach (int buffId in SSJBuffs())
            {
                if (player.HasBuff(buffId))
                    return true;
            }

            return false;
        }

        // fairly special state, whether the player is in SSJ1 and Kaioken combined, which has some special behaviors.
        public static bool IsSSJ1Kaioken(Player player)
        {
            return player.HasBuff(SSJ1Kaioken);
        }

        // whether the player is in SSJG, specifically, for now.
        public static bool IsGodlike(Player player)
        {
            return player.HasBuff(SSJG);
        }

        // specifically whether or not the player is in SSJ1, not to be confused with SSJ, "any" SSJ non-legendary form.
        public static bool IsSSJ1(Player player)
        {
            return player.HasBuff(SSJ1);
        }

        // specifically whether or not the player is in ASSJ, used primarily for checking if ascension is valid.
        public static bool IsASSJ(Player player)
        {
            return player.HasBuff(ASSJ);
        }

        // specifically whether or not the player is in ASSJ, used primarily for checking if ascension is valid.
        public static bool IsUSSJ(Player player)
        {
            return player.HasBuff(USSJ);
        }

        // returns the buff Id of a transformation menu selection
        public static int GetBuffIdFromMenuSelection(MenuSelectionID buffId)
        {
            switch (buffId)
            {
                case MenuSelectionID.SSJ1:
                    return SSJ1;
                case MenuSelectionID.SSJ2:
                    return SSJ2;
                case MenuSelectionID.SSJ3:
                    return SSJ3;
                case MenuSelectionID.SSJG:
                    return SSJG;
                case MenuSelectionID.LSSJ1:
                    return LSSJ;
                case MenuSelectionID.LSSJ2:
                    return LSSJ2;
                case MenuSelectionID.SSJB:
                    return SSJB;                    
                case MenuSelectionID.UI:
                case MenuSelectionID.None:
                default:
                    return -1;
            }
        }

        // overload method of CanTransform(player, buffId [as int])
        public static bool CanTransform(Player player, MenuSelectionID buffId)
        {
            return CanTransform(player, GetBuffIdFromMenuSelection(buffId));
        }

        // whether the buff ID is one of the ascended states.
        public static bool IsAscended(int buffId)
        {
            return buffId == ASSJ || buffId == USSJ;
        }

        // whether the player is in either ascended state, used when ascending (charge + transform)
        public static bool IsAscended(Player player)
        {
            return player.HasBuff(ASSJ) || player.HasBuff(USSJ);
        }

        // handle all the conditions of a transformation which would prevent the player from reaching that state. Return false if you can't transform.
        // this doesn't include a few bits of player state which get handled in the player class. See MyPlayer CanTransform for more flags.
        public static bool CanTransform(Player player, int buffId)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            if (buffId == SSJ1)
                return !IsKaioken(player) && modPlayer.SSJ1Achieved;
            if (buffId == SSJ2)
                return !IsKaioken(player) && modPlayer.playerTrait != "Legendary" && modPlayer.SSJ2Achieved;
            if (buffId == SSJ3)
                return !IsKaioken(player) && modPlayer.playerTrait != "Legendary" && modPlayer.SSJ3Achieved;
            if (buffId == SSJG)
                return !IsKaioken(player) && modPlayer.playerTrait != "Legendary" && modPlayer.SSJGAchieved;
            if (buffId == LSSJ)
                return !IsKaioken(player) && modPlayer.playerTrait == "Legendary" && modPlayer.LSSJAchieved;
            if (buffId == LSSJ2)
                return !IsKaioken(player) && modPlayer.playerTrait == "Legendary" && modPlayer.LSSJ2Achieved;
            //if (buffId == SSJB)
            //  return !IsKaioken(player) && modPlayer.SSJBAchieved;
            if (buffId == ASSJ)
                return !IsKaioken(player) && (IsSSJ1(player) || IsUSSJ(player)) && modPlayer.ASSJAchieved;
            if (buffId == USSJ)
                return !IsKaioken(player) && IsASSJ(player) && modPlayer.USSJAchieved;
            if (buffId == Kaioken)
                return !IsSSJ(player) && !IsLegendary(player) && !IsAscended(player) && modPlayer.KaioAchieved;
            if (buffId == Kaioken3)
                return !IsSSJ(player) && !IsLegendary(player) && !IsAscended(player) && modPlayer.KaioFragment1;
            if (buffId == Kaioken10)
                return !IsSSJ(player) && !IsLegendary(player) && !IsAscended(player) && modPlayer.KaioFragment2;
            if (buffId == Kaioken20)
                return !IsSSJ(player) && !IsLegendary(player) && !IsAscended(player) && modPlayer.KaioFragment3;
            if (buffId == Kaioken100)
                return !IsSSJ(player) && !IsLegendary(player) && !IsAscended(player) && modPlayer.KaioFragment4;
            if (buffId == SSJ1Kaioken)
                return IsSSJ1(player) && modPlayer.KaioAchieved;
            return false;
        }

        // handle actually spawning the aura starter projectile, playing the sound, displaying the text, doing the deal.
        public static void DoTransform(Player player, MenuSelectionID buffId)
        {
            DoTransform(player, GetBuffIdFromMenuSelection(buffId));
        }

        // wipes out all transformation buffs, requires them to be a part of the AllBuffs() union (it's a bunch of lists joined together).
        public static void ClearAllTransformations(Player player)
        {
            foreach(int buffId in AllBuffs())
            {
                if (player.HasBuff(buffId))
                    player.ClearBuff(buffId);
            }
        }

        // if the transformation uses a specific volume, this returns the volume.
        public static float GetVolumeForBuff(int buffId)
        {
            // KaioStart is the quietest of all.
            if (buffId == Kaioken)
                return 0.5f;

            // Sounds at 0.6f.
            if (buffId == SSJ1 || buffId == Kaioken3 || buffId == Kaioken10)
                return 0.6f;

            // ASSJ is 1.0 for some reason.
            if (buffId == ASSJ)
                return 1.0f;

            // Kaioken 100 and SSJ Kaioken are 0.8f
            if (buffId == Kaioken100 || buffId == SSJ1Kaioken)
                return 0.8f;

            // other buffs are 0.7f.
            if (AllBuffs().Contains(buffId))
                return 0.7f;            

            // something bad has happened, but it's probably fine. We must be missing an explicit volume.
            return 1.0f;
        }

        // simply whether the transformation has a startup sound.
        public static bool HasSoundEffectForBuff(int buffId)
        {
            return GetSoundEffectForBuff(buffId).Equals(string.Empty);
        }

        // figure out which sound to use for a given Buff ID
        public static string GetSoundEffectForBuff(int buffId)
        {
            // kaioken buffs go first. Anything here returns kaioken sounds.
            // the first kaioken is the only one that uses AuraStart
            if (Kaioken == buffId)
                return "Sounds/KaioAuraStart";

            if (KaiokenBuffs().Contains(buffId) || buffId == SSJ1Kaioken)
                return "Sounds/KaioAuraAscend";

            // then anything else gets caught here. Even though AllBuffs contains Kaiokens, this won't ever return SSJ sounds for Kaioken, because of above.
            if (AllBuffs().Contains(buffId))
                return "Sounds/SSJAscension";

            // something bad has happened, or maybe there just isn't a sound effect.
            return string.Empty;
        }

        // handle playing the sound for the transformation startup if applicable.
        public static void DoSoundEffectForBuff(Player player, int buffId)
        {
            float volume = Math.Min(1f, GetVolumeForBuff(buffId));
            string effect = GetSoundEffectForBuff(buffId);
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, effect).WithVolume(volume));
        }

        // get the color for the transformation text, if applicable.
        public static Color GetColorForTransformation(int buffId)
        {
            if (SSJG == buffId)
                return new Color(229, 20, 51);

            // something bad has happened or just isn't explicitly handled. Return the most common color used.
            return new Color(219, 219, 48);
        }

        // simply whether there is a transformation name to display.
        public static bool HasTextForTransformation(int buffId)
        {
            return GetTextForTransformation(buffId).Equals(string.Empty);
        }

        // get the friendly display name of the transformation, if applicable.
        public static string GetTextForTransformation(int buffId)
        {   
            if (buffId == SSJ1)
                return "Super Saiyan 1";
            if (buffId == SSJ2)
                return "Super Saiyan 2";
            if (buffId == SSJ3)
                return "Super Saiyan 3";
            if (buffId == SSJG)
                return "Super Saiyan God";
            if (buffId == ASSJ)
                return "Ascended Super Saiyan";
            if (buffId == USSJ)
                return "Ultra Super Saiyan";
            if (buffId == LSSJ)
                return "Legendary Super Saiyan";
            if (buffId == LSSJ2)
                return "Legendary Super Saiyan 2";            

            // to catch errors, or show that something isn't explicitly handled.
            return string.Empty;
        }

        // print text for the transformation, if applicable, with its respective color.
        public static void DoCombatTextForTransformation(Player player, int buffId)
        {
            if (!HasTextForTransformation(buffId))
                return;
            Color color = GetColorForTransformation(buffId);
            string text = GetTextForTransformation(buffId);
            CombatText.NewText(player.Hitbox, color, text, false, false);
        }

        // figure out which aura to use for the transformation based on the Buff ID
        public static string GetProjectileTypeForBuff(int buffId)
        {
            if (buffId == SSJ1)
                return "SSJ1AuraProjStart";
            if (buffId == SSJ2)
                return "SSJ2AuraProj";
            if (buffId == SSJ3)
                return "SSJ3AuraProj";
            if (buffId == SSJG)
                return "SSJGTransformStart";
            if (buffId == LSSJ)
                return "LSSJAuraProj";
            if (buffId == LSSJ2)
                return "LSSJ2AuraProj";
            if (buffId == ASSJ) // I couldn't find any unique ones for ASSJ and USSJ
                return "SSJ3AuraProj";
            if (buffId == USSJ)
                return "SSJ3AuraProj";
            if (buffId == Kaioken)
                return "KaiokenAuraProj";
            if (buffId == Kaioken3)
                return "KaiokenAuraProjx3";
            if (buffId == Kaioken10)
                return "KaiokenAuraProjx10";
            if (buffId == Kaioken20)
                return "KaiokenAuraProjx20";
            if (buffId == Kaioken100)
                return "KaiokenAuraProjx100";

            // something very bad has happened and you will not be going to space.
            return string.Empty;
        }

        // create the projectile representing the transformation aura.
        public static void DoProjectileForBuff(Player player, int buffId)
        {
            // SSJ1 Kaioken is special, it gets two.
            if (buffId == SSJ1Kaioken)
            {
                string ssjProjectile = GetProjectileTypeForBuff(SSJ1);
                string kaiokenProjectile = GetProjectileTypeForBuff(Kaioken);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType(ssjProjectile), 0, 0, player.whoAmI);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType(kaiokenProjectile), 0, 0, player.whoAmI);
            }
            else
            {
                string projectileType = GetProjectileTypeForBuff(buffId);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType(projectileType), 0, 0, player.whoAmI);
            }
        }

        // add projectile auras to this list to make them killable when powering up and down between forms.
        // this prevents auras from superimposing.
        public static Type[] AuraTypes()
        {
            Type[] types = {
                typeof(BaseAuraProj),
                typeof(FalseUIAuraProj),
                typeof(KaiokenAuraProj),
                typeof(KaiokenAuraProjx10),
                typeof(KaiokenAuraProjx100),
                typeof(KaiokenAuraProjx20),
                typeof(KaiokenAuraProjx3),
                typeof(LSSJ2AuraProj),
                typeof(LSSJAuraProj),
                typeof(SSJ1AuraProj),
                typeof(SSJ2AuraProj),
                typeof(SSJ3AuraProj),
                typeof(SSJGAuraProj)
            };
            return types;
        }

        // utilizes above array of types to prevent auras from superimposing when switching states.
        public static void KillPlayerAuras(Player player)
        {
            foreach(Projectile proj in Main.projectile)
            {
                if (Main.player[proj.owner] == player)
                {
                    if(proj.modProjectile != null)
                    {
                        // if it's an instance of an aura projectile kill it.
                        if (AuraTypes().Contains(proj.modProjectile.GetType()))
                        {
                            proj.Kill();
                        }
                    }
                }
            }
        }

        // actually handle transforming. Takes quite a few steps to clean up after itself and do all the things.
        public static void DoTransform(Player player, int buffId)
        {
            int currentTransformation = GetCurrentTransformation(player);

            MyPlayer modPlayer = MyPlayer.ModPlayer(player);

            // special circumstances when powering down from SSJ1 Kaioken to SSJ1 - inflict the tired debuff.
            if (currentTransformation == SSJ1Kaioken && buffId == SSJ1)
            {
                player.AddBuff(mod.BuffType("TiredDebuff"), (int)(modPlayer.KaiokenTimer * 2));
                modPlayer.KaiokenTimer = 0;
            }

            // don't.. try to apply the same transformation. This just stacks projectile auras and looks dumb.
            if (buffId == currentTransformation)
                return;

            // wipe out existing auras the player has to prevent superimposing.
            KillPlayerAuras(player);

            // remove all *transformation* buffs from the player.
            ClearAllTransformations(player);


            // if the buff needs dust aura, do that.
            if (SSJBuffs().Contains(buffId) || buffId == ASSJ || buffId == USSJ)
                modPlayer.SSJDustAura();

            // add whatever buff it is for a really long time.
            player.AddBuff(buffId, 666666, false);

            // create the projectile starter if applicable.
            DoProjectileForBuff(player, buffId);

            if (!Main.dedServ)
                DoSoundEffectForBuff(player, buffId);

            DoCombatTextForTransformation(player, buffId);
        }

        // return the first located transformation of a given player. Assumes there should only ever be one, returns the first it finds.
        public static int GetCurrentTransformation(Player player)
        {
            foreach(int buffId in AllBuffs())
            {
                if (player.HasBuff(buffId))
                {
                    return buffId;
                }
            }

            // is the player transformed? Something bad may have happened.
            return -1;
        }

        // based on some conditions, figure out what the next "step" of transformation should be.
        public static int GetNextTransformationStep(Player player)
        {
            int currentTransformation = GetCurrentTransformation(player);

            // SSJ1 is always the starting point if there isn't a current form tree to step through.
            if (currentTransformation == -1)
                return SSJ1;

            // the player is legendary and doing a legendary step up.
            if (IsLegendary(currentTransformation) && MyPlayer.ModPlayer(player).playerTrait == "Legendary")
            {
                for (int i = 0; i < TransformationLegendarySteps().Length; i++)
                {
                    if (TransformationLegendarySteps()[i] == currentTransformation && i < TransformationLegendarySteps().Length - 1)
                    {
                        return TransformationLegendarySteps()[i + 1];
                    }
                }
            }

            // the player isn't legendary and is doing a normal step up.
            if (IsSSJ(currentTransformation) && MyPlayer.ModPlayer(player).playerTrait != "Legendary")
            {
                for (int i = 0; i < TransformationSSJSteps().Length; i++)
                {
                    if (TransformationSSJSteps()[i] == currentTransformation && i < TransformationSSJSteps().Length - 1)
                    {
                        return TransformationSSJSteps()[i + 1];
                    }
                }
            }

            // whatever happened here, the function couldn't find a next step. Either the player is maxed in their steps, or something bad happened.
            return currentTransformation;
        }

        // based on some conditions, figure out what the previous "step" of transformation should be. Note this includes ascension stepping down.
        public static int GetPreviousTransformationStep(Player player)
        {
            int currentTransformation = GetCurrentTransformation(player);

            // the player is legendary and doing a legendary step down.
            if (IsLegendary(currentTransformation) && MyPlayer.ModPlayer(player).playerTrait == "Legendary")
            {
                for (int i = 0; i < TransformationLegendarySteps().Length; i++)
                {
                    if (TransformationLegendarySteps()[i] == currentTransformation && i > 0)
                    {
                        return TransformationLegendarySteps()[i - 1];
                    }
                }
            }

            // the player isn't legendary and is doing a normal step down.
            if (IsSSJ(currentTransformation) && MyPlayer.ModPlayer(player).playerTrait != "Legendary")
            {
                for (int i = 0; i < TransformationSSJSteps().Length; i++)
                {
                    if (TransformationSSJSteps()[i] == currentTransformation && i > 0)
                    {
                        return TransformationSSJSteps()[i - 1];
                    }
                }
            }

            // figure out what the step down for ascension should be, if the player is in an ascended form.
            if (IsAscended(currentTransformation))
            {
                for (int i = 0; i < TransformationAscensionSteps().Length; i++)
                {
                    if (TransformationAscensionSteps()[i] == currentTransformation && i > 0)
                    {
                        return TransformationAscensionSteps()[i - 1];
                    }
                }
            }

            // either the player is at minimum or something bad has happened.
            return currentTransformation;
        }

        // based on some conditions, figure out what the next "step" of kaioken should be.
        public static int GetNextKaiokenStep(Player player)
        {
            int currentTransformation = GetCurrentTransformation(player);

            // player is going into SSJ1 Kaioken, which is unique.
            if (currentTransformation == SSJ1)
                return SSJ1Kaioken;

            // special handling for Kaioken from no transformed state.
            if (currentTransformation == -1)
                return Kaioken;

            // the player is doing some kind of kaioken transformation step up.
            if (IsKaioken(currentTransformation))
            {
                for (int i = 0; i < TransformationKaiokenSteps().Length; i++)
                {
                    if (TransformationKaiokenSteps()[i] == currentTransformation && i < TransformationKaiokenSteps().Length - 1)
                    {
                        return TransformationKaiokenSteps()[i + 1];
                    }
                }
            }

            // whatever happened here, the function couldn't find a next step. Either the player is maxed in their steps, or something bad happened.
            return currentTransformation;
        }

        // based on some conditions, figure out what the previous "step" of kaioken should be.
        public static int GetPreviousKaiokenStep(Player player)
        {
            int currentTransformation = GetCurrentTransformation(player);

            // player is going into SSJ1 Kaioken, which is unique.
            if (currentTransformation == SSJ1Kaioken)
                return SSJ1;

            // the player is doing some kind of kaioken transformation step up.
            if (IsKaioken(currentTransformation))
            {
                for (int i = 0; i < TransformationKaiokenSteps().Length; i++)
                {
                    if (TransformationKaiokenSteps()[i] == currentTransformation && i > 0)
                    {
                        return TransformationKaiokenSteps()[i - 1];
                    }
                }
            }

            // whatever happened here, the function couldn't find a next step. Either the player is maxed in their steps, or something bad happened.
            return currentTransformation;
        }

        // based on some conditions, figure out what the next "step" of ascension should be.
        public static int GetNextAscensionStep(Player player)
        {
            int currentTransformation = GetCurrentTransformation(player);

            if (IsAscended(currentTransformation))
            {
                for (int i = 0; i < TransformationAscensionSteps().Length; i++) {
                    if (TransformationAscensionSteps()[i] == currentTransformation && i < TransformationAscensionSteps().Length - 1)
                    {
                        return TransformationAscensionSteps()[i + 1];
                    }
                }
            }

            return currentTransformation;
        }
    }
}
