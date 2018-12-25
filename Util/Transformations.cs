using Buffs;
using DBZMOD;
using DBZMOD.Projectiles.Auras;
using DBZMOD.Projectiles.Auras.Dev;
using Enums;
using Microsoft.Xna.Framework;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Util
{
    // class for helping out with all the buff integers, lists of buffs in order, presence of buffs/abstraction
    public static class Transformations
    {
        public const int ABSURDLY_LONG_BUFF_DURATION = 666666;

        private static Mod _modInstance;
        public static Mod modInstance
        {
            get
            {
                if (_modInstance == null)
                {
                    _modInstance = DBZMOD.DBZMOD.instance;
                }
                return _modInstance;
            }
        }

        private static Dictionary<string, BuffInfo> _buffInfoDict;
        // Using the buff internal name as the key. The BuffId is inside.
        public static Dictionary<string, BuffInfo> BuffInfoDict
        {
            get
            {
                if (_buffInfoDict == null)
                {
                    SeedBuffInfoDictionary();
                }
                return _buffInfoDict;
            }
        }

        // called on startup to cache the values of the mod's buffs in our own internal dictionary for faster lookups.
        public static void SeedBuffInfoDictionary()
        {
            _buffInfoDict = new Dictionary<string, BuffInfo>();
            _buffInfoDict[SSJ1.BuffKeyName] = SSJ1;
            _buffInfoDict[SSJ2.BuffKeyName] = SSJ2;
            _buffInfoDict[SSJ3.BuffKeyName] = SSJ3;
            _buffInfoDict[SSJG.BuffKeyName] = SSJG;
            _buffInfoDict[LSSJ.BuffKeyName] = LSSJ;
            _buffInfoDict[LSSJ2.BuffKeyName] = LSSJ2;
            _buffInfoDict[ASSJ.BuffKeyName] = ASSJ;
            _buffInfoDict[USSJ.BuffKeyName] = USSJ;
            _buffInfoDict[Kaioken.BuffKeyName] = Kaioken;
            _buffInfoDict[SuperKaioken.BuffKeyName] = SuperKaioken;
            _buffInfoDict[KaiokenFatigue.BuffKeyName] = KaiokenFatigue;
            _buffInfoDict[TransformationExhaustion.BuffKeyName] = TransformationExhaustion;
            _buffInfoDict[SPECTRUM.BuffKeyName] = SPECTRUM;
        }

        // the following are cached info classes that get passed around for all sorts of things.
        private static BuffInfo _SSJ1;
        public static BuffInfo SSJ1
        {
            get
            {
                if (_SSJ1 == null)
                {
                    _SSJ1 = new BuffInfo(MenuSelectionID.SSJ1, BuffKeyNames.SSJ1, 0.6f, "Sounds/SSJAscension",
                        "Super Saiyan 1", DefaultTransformationTextColor, new Type[] { typeof(SSJ1AuraProj) }, new string[] { "SSJ1AuraProj" });
                }
                return _SSJ1;
            }
        }

        public static BuffInfo _SSJ2;
        public static BuffInfo SSJ2
        {
            get
            {
                if (_SSJ2 == null)
                {
                    _SSJ2 = new BuffInfo(MenuSelectionID.SSJ2, BuffKeyNames.SSJ2, 0.7f, "Sounds/SSJAscension",
                         "Super Saiyan 2", DefaultTransformationTextColor, new Type[] { typeof(SSJ2AuraProj) }, new string[] { "SSJ2AuraProj" });
                }
                return _SSJ2;
            }
        }

        public static BuffInfo _SSJ3;
        public static BuffInfo SSJ3
        {
            get
            {
                if (_SSJ3 == null)
                {
                    _SSJ3 = new BuffInfo(MenuSelectionID.SSJ3, BuffKeyNames.SSJ3, 0.7f, "Sounds/SSJAscension",
                         "Super Saiyan 3", DefaultTransformationTextColor, new Type[] { typeof(SSJ3AuraProj) }, new string[] { "SSJ3AuraProj" });
                }
                return _SSJ3;
            }
        }

        public static BuffInfo _SSJG;
        public static BuffInfo SSJG
        {
            get
            {
                if (_SSJG == null)
                {
                    _SSJG = new BuffInfo(MenuSelectionID.SSJG, BuffKeyNames.SSJG, 0.7f, "Sounds/SSJAscension",
                         "Super Saiyan God", DefaultTransformationTextColor, new Type[] { typeof(SSJGAuraProj) }, new string[] { "SSJGAuraProj" });
                }
                return _SSJG;
            }
        }

        public static BuffInfo _SSJB;
        public static BuffInfo SSJB
        {
            get
            {
                if (_SSJB == null)
                {
                    _SSJB = new BuffInfo(MenuSelectionID.SSJB, BuffKeyNames.SSJB, 0.7f, "Sounds/SSJAscension",
                         null, DefaultTransformationTextColor, new Type[] { }, new string[] { });
                }
                return _SSJB;
            }
        }

        public static BuffInfo _LSSJ;
        public static BuffInfo LSSJ
        {
            get
            {
                if (_LSSJ == null)
                {
                    _LSSJ = new BuffInfo(MenuSelectionID.LSSJ1, BuffKeyNames.LSSJ, 0.7f, "Sounds/SSJAscension",
                         "Legendary Super Saiyan", DefaultTransformationTextColor, new Type[] { typeof(LSSJAuraProj) }, new string[] { "LSSJAuraProj" });
                }
                return _LSSJ;
            }
        }

        public static BuffInfo _LSSJ2;
        public static BuffInfo LSSJ2
        {
            get
            {
                if (_LSSJ2 == null)
                {
                    _LSSJ2 = new BuffInfo(MenuSelectionID.LSSJ2, BuffKeyNames.LSSJ2, 0.7f, "Sounds/SSJAscension",
                         "Legendary Super Saiyan 2", DefaultTransformationTextColor, new Type[] { typeof(LSSJ2AuraProj) }, new string[] { "LSSJ2AuraProj" });
                }
                return _LSSJ2;
            }
        }

        public static BuffInfo _ASSJ;
        public static BuffInfo ASSJ
        {
            get
            {
                if (_ASSJ == null)
                {
                    _ASSJ = new BuffInfo(MenuSelectionID.None, BuffKeyNames.ASSJ, 1.0f, "Sounds/SSJAscension",
                         "Ascended Super Saiyan", DefaultTransformationTextColor, new Type[] { typeof(SSJ1AuraProj) }, new string[] { "SSJ1AuraProj" });
                }
                return _ASSJ;
            }
        }

        public static BuffInfo _USSJ;
        public static BuffInfo USSJ
        {
            get
            {
                if (_USSJ == null)
                {
                    _USSJ = new BuffInfo(MenuSelectionID.None, BuffKeyNames.USSJ, 0.7f, "Sounds/SSJAscension",
                         "Ultra Super Saiyan", DefaultTransformationTextColor, new Type[] { typeof(SSJ1AuraProj) }, new string[] { "SSJ1AuraProj" });
                }
                return _USSJ;
            }
        }

        public static BuffInfo _Kaioken;
        public static BuffInfo Kaioken
        {
            get
            {
                if (_Kaioken == null)
                {
                    _Kaioken = new BuffInfo(MenuSelectionID.None, BuffKeyNames.Kaioken, 0.5f, "Sounds/KaioAuraStart",
                         null, DefaultTransformationTextColor, new Type[] { typeof(KaiokenAuraProj) }, new string[] { "KaiokenAuraProj" });
                }
                return _Kaioken;
            }
        }

        // the difference between Kaioken and super kaioken is purely cosmetic. They're the same buff.
        public static BuffInfo _SuperKaioken;
        public static BuffInfo SuperKaioken
        {
            get
            {
                if (_SuperKaioken == null)
                {
                    _SuperKaioken = new BuffInfo(MenuSelectionID.None, BuffKeyNames.SuperKaioken, 0.5f, "Sounds/KaioAuraStart",
                         null, DefaultTransformationTextColor, new Type[] { typeof(SuperKaiokenAuraProj) }, new string[] { "SuperKaiokenAuraProj" });
                }
                return _SuperKaioken;
            }
        }

        // the two debuffs from transforms
        public static BuffInfo _KaiokenFatigue;
        public static BuffInfo KaiokenFatigue
        {
            get
            {
                if (_KaiokenFatigue == null)
                {
                    _KaiokenFatigue = new BuffInfo(MenuSelectionID.None, BuffKeyNames.KaiokenFatigue, 0.0f, null,
                         null, DefaultTransformationTextColor, new Type[] { }, new string[] { });
                }
                return _KaiokenFatigue;
            }
        }
        public static BuffInfo _TransformationExhaustion;
        public static BuffInfo TransformationExhaustion
        {
            get
            {
                if (_TransformationExhaustion == null)
                {
                    _TransformationExhaustion = new BuffInfo(MenuSelectionID.None, BuffKeyNames.TransformationExhaustion, 0.0f, null,
                         null, DefaultTransformationTextColor, new Type[] { }, new string[] { });
                }
                return _TransformationExhaustion;
            }
        }
        public static BuffInfo _SPECTRUM;
        public static BuffInfo SPECTRUM
        {
            get
            {
                if (_SPECTRUM == null)
                {
                    _SPECTRUM = new BuffInfo(MenuSelectionID.Spectrum, BuffKeyNames.Spectrum, 0.7f, "Sounds/SSJAscension",
                         "Super Saiyan Spectrum", DefaultTransformationTextColor, new Type[] { typeof(SSJSpectrumAuraProj) }, new string[] { "SSJSpectrumAuraProj" });
                }
                return _SPECTRUM;
            }
        }

        // returns the buff Id of a transformation menu selection
        public static BuffInfo GetBuffFromMenuSelection(MenuSelectionID menuId)
        {
            // don't return any of the buffs by menu selection if there isn't a selection. That's bad.
            if (menuId == MenuSelectionID.None)
                return null;

            return BuffInfoDict.Values.Where(x => x.MenuId == menuId).FirstOrDefault();
        }

        // the typical color used for super saiyan transformation Text, except God
        public static Color DefaultTransformationTextColor = new Color(219, 219, 48);

        public static Color GodTransformationTextColor = new Color(229, 20, 51);

        public static BuffInfo GetBuffByKeyName(string keyName)
        {
            return BuffInfoDict[keyName];
        }

        // returns a list of transformation steps specific to non-legendary SSJ players
        public static BuffInfo[] SSJBuffs()
        {
            BuffInfo[] buffs = { SSJ1, SSJ2, SSJ3, SSJG };
            return buffs;
        }

        // a list of transformation steps specific to kaioken usage
        public static BuffInfo[] KaiokenBuffs()
        {
            BuffInfo[] buffs = { Kaioken, SuperKaioken };
            return buffs;
        }

        // a list of transformation steps specific to legendary SSJ players
        public static BuffInfo[] LegendaryBuffs()
        {
            BuffInfo[] buffs = { SSJ1, LSSJ, LSSJ2 };
            return buffs;
        }

        // a list of transformation steps from SSJ1 through ascended SSJ forms
        public static BuffInfo[] AscensionBuffs()
        {
            BuffInfo[] buffs = { SSJ1, ASSJ, USSJ };
            return buffs;
        }

        // A bunch of lists joined together, in order to contain every possible transformation buff.
        // when adding new ones, make sure they wind up here in some form, even if you just have to add them one at a time.
        // (Union() excludes duplicates automatically)
        public static List<BuffInfo> AllBuffs()
        {
            return BuffInfoDict.Values.Where(x => x.BuffKeyName != BuffKeyNames.KaiokenFatigue && x.BuffKeyName != BuffKeyNames.TransformationExhaustion).ToList();
        }

        // whether the player is in any of the transformation states. Relies on AllBuffs() containing every possible transformation buff.
        public static bool IsPlayerTransformed(Player player)
        {
            foreach (BuffInfo buff in AllBuffs())
            {
                if (player.HasBuff(buff.GetBuffId()))
                    return true;
            }

            return false;
        }

        public static bool PlayerHasBuffIn(Player player, BuffInfo[] buffs)
        {
            foreach (BuffInfo buff in buffs)
            {
                if (player.HasBuff(buff.GetBuffId()))
                    return true;
            }

            return false;
        }

        public static bool BuffIsIn(BuffInfo buff, BuffInfo[] buffs) { return buffs.Contains(buff); }

        // whether the buff Id is one of the "normal" SSJ States (1, 2, 3, G)
        public static bool IsSSJ(BuffInfo buff)
        {
            return SSJBuffs().Contains(buff);
        }

        // whether the player is in *any* of the "normal" SSJ states (1, 2, 3, G)
        public static bool IsSSJ(Player player)
        {
            return PlayerHasBuffIn(player, SSJBuffs());
        }

		// whether the player is in SSJ2
        public static bool IsSSJ2(Player player)
        {
            return player.HasBuff(SSJ2.GetBuffId());
        }

		// whether the player is in long hair mode
		 public static bool IsSSJ3(Player player)
        {
            return player.HasBuff(SSJ3.GetBuffId());
        }

        // FAIRY special state, whether the player is in Nuova's Dev form, SPECTRUM, which is fabulous.
        public static bool IsSpectrum(Player player)
        {
            return player.HasBuff(SPECTRUM.GetBuffId());
        }

        // FAIRY special state, whether the player is in Nuova's Dev form, SPECTRUM, which is fabulous.
        public static bool IsSpectrum(BuffInfo buff)
        {
            return SPECTRUM.GetBuffId() == buff.GetBuffId();
        }

        // whether the buff ID is one of the Kaioken states
        public static bool IsKaioken(BuffInfo buff)
        {
            return KaiokenBuffs().Contains(buff);
        }

        // whether the player is in one of the Kaioken states
        public static bool IsKaioken(Player player)
        {
            return PlayerHasBuffIn(player, KaiokenBuffs());
        }

        public static bool IsDevBuffed(BuffInfo buff)
        {
            return IsSpectrum(buff);
        }

        public static bool IsDevBuffed(Player player)
        {
            return IsSpectrum(player);
        }

        // a bool for whether the player is in a state other than Kaioken (a form)
        public static bool IsAnythingOtherThanKaioken(BuffInfo buff)
        {
            return IsLSSJ(buff) || IsSSJ(buff) || IsGodlikeBuff(buff) || IsDevBuffed(buff) || IsAscended(buff);
        }

        // a bool for whether the player is in a state other than Kaioken (a form)
        public static bool IsAnythingOtherThanKaioken(Player player)
        {
            return IsLSSJ(player) || IsSSJ(player) || IsGodlike(player) || IsDevBuffed(player) || IsAscended(player);
        }

        // whether the buff ID is one of the legendary states; caution, this includes SSJ1 for "Next/Previous" behavior reasons.
        public static bool IsLSSJ(BuffInfo buff)
        {
            return LegendaryBuffs().Contains(buff);
        }

        // whether the player is in one of the legendary states; caution, this includes SSJ1 for "Next/Previous" behavior reasons.
        public static bool IsLSSJ(Player player)
        {
            return PlayerHasBuffIn(player, LegendaryBuffs());
        }

        // whether the buff is SSJG, specifically
        public static bool IsGodlikeBuff(BuffInfo buff)
        {
            return SSJG.GetBuffId() == buff.GetBuffId();
        }

        // whether the player is in SSJG, specifically, for now.
        public static bool IsGodlike(Player player)
        {
            return player.HasBuff(SSJG.GetBuffId());
        }

        // specifically whether or not the player is in SSJ1, not to be confused with SSJ, "any" SSJ non-legendary form.
        public static bool IsSSJ1(Player player)
        {
            return player.HasBuff(SSJ1.GetBuffId());
        }

        // specifically whether or not the player is in ASSJ, used primarily for checking if ascension is valid.
        public static bool IsASSJ(Player player)
        {
            return player.HasBuff(ASSJ.GetBuffId());
        }

        // specifically whether or not the player is in ASSJ, used primarily for checking if ascension is valid.
        public static bool IsUSSJ(Player player)
        {
            return player.HasBuff(USSJ.GetBuffId());
        }

        // overload method of CanTransform(player, buffId [as int])
        public static bool CanTransform(Player player, MenuSelectionID menuId)
        {
            return CanTransform(player, GetBuffFromMenuSelection(menuId));
        }

        // whether the buff ID is one of the ascended states.
        public static bool IsAscended(BuffInfo buff)
        {
            return buff == ASSJ || buff == USSJ;
        }

        // whether the player is in either ascended state, used when ascending (charge + transform)
        public static bool IsAscended(Player player)
        {
            return player.HasBuff(ASSJ.GetBuffId()) || player.HasBuff(USSJ.GetBuffId());
        }
        public static bool IsTransformBlocked(Player player)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            return modPlayer.IsTransforming || player.channel || modPlayer.IsPlayerImmobilized();
        }
        
        // handle all the conditions of a transformation which would prevent the player from reaching that state. Return false if you can't transform.
        // this doesn't include a few bits of player state which get handled in the player class. See MyPlayer CanTransform for more flags.
        public static bool CanTransform(Player player, BuffInfo buff)
        {
            if (buff == null)
                return false;

            if (IsTransformBlocked(player))
                return false;
            
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            if (buff == SSJ1)
                return modPlayer.SSJ1Achieved && !Transformations.IsExhaustedFromTransformation(player);
            if (buff == SSJ2)
                return !modPlayer.IsPlayerLegendary() && modPlayer.SSJ2Achieved && !Transformations.IsExhaustedFromTransformation(player);
            if (buff == SSJ3)
                return !modPlayer.IsPlayerLegendary() && modPlayer.SSJ3Achieved && !Transformations.IsExhaustedFromTransformation(player);
            if (buff == SSJG)
                return !modPlayer.IsPlayerLegendary() && modPlayer.SSJGAchieved && !Transformations.IsExhaustedFromTransformation(player);
            if (buff == LSSJ)
                return modPlayer.IsPlayerLegendary() && modPlayer.LSSJAchieved && !Transformations.IsExhaustedFromTransformation(player);
            if (buff == LSSJ2)
                return modPlayer.IsPlayerLegendary() && modPlayer.LSSJ2Achieved && !Transformations.IsExhaustedFromTransformation(player);
            if (buff == ASSJ)
                return (IsSSJ1(player) || IsUSSJ(player)) && modPlayer.ASSJAchieved && !Transformations.IsExhaustedFromTransformation(player);
            if (buff == USSJ)
                return IsASSJ(player) && modPlayer.USSJAchieved && !Transformations.IsExhaustedFromTransformation(player);
            if (buff == Kaioken || buff == SuperKaioken)
                return modPlayer.KaioAchieved && !Transformations.IsTiredFromKaioken(player);
            if (buff == SPECTRUM)
                return player.name == "Nuova";
            return false;
        }

        public static void AddKaiokenExhaustion(Player player, int multiplier)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);
            player.AddBuff(KaiokenFatigue.GetBuffId(), (int)Math.Ceiling(modPlayer.KaiokenTimer * multiplier));
            modPlayer.KaiokenTimer = 0f;
        }

        public static void AddTransformationExhaustion(Player player)
        {
            player.AddBuff(TransformationExhaustion.GetBuffId(), 600);
        }

        public static bool IsExhaustedFromTransformation(Player player) { return player.HasBuff(TransformationExhaustion.GetBuffId()); }

        public static bool IsTiredFromKaioken(Player player) { return player.HasBuff(KaiokenFatigue.GetBuffId()); }

        // wipes out all transformation buffs, requires them to be a part of the AllBuffs() union (it's a bunch of lists joined together).
        public static void ClearAllTransformations(Player player, bool isPoweringDown, bool isOneStep, BuffInfo transformationToKeep = null)
        {
            foreach (BuffInfo buff in AllBuffs())
            {
                // don't clear buffs the player doesn't have, obviously.
                if (!player.HasBuff(buff.GetBuffId()))
                    continue;

                //dont clear the buff we want to keep
                if (buff == transformationToKeep)
                    continue;

                RemoveTransformation(player, buff.BuffKeyName);
            }
        }

        // clear a single transformation buff from the target player
        public static void RemoveTransformation(Player player, string buffKeyName)
        {
            BuffInfo buff = GetBuffByKeyName(buffKeyName);

            foreach (Type auraType in buff.AuraProjectileTypes)
            {
                FindAndKillPlayerAurasOfType(player, auraType);
            }

            player.ClearBuff(buff.GetBuffId());

            if (!Main.dedServ && Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
            {                
                NetworkHelper.formSync.SendFormChanges(256, player.whoAmI, player.whoAmI, buffKeyName, 0);                
            }
        }

        // enforces the prevention of superimposed auras on a given player
        public static void FindAndKillPlayerAurasOfType(Player player, Type auraType)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (Main.player[proj.owner] != player)
                    continue;

                if (proj.modProjectile == null)
                    continue;

                // if it's an instance of an aura projectile kill it.
                if (proj.modProjectile.GetType().Equals(auraType))
                    proj.Kill();
            }
        }

        // create the projectile(s) representing the transformation aura.
        public static void DoProjectileForBuff(Player player, List<BuffInfo> buffs)
        {
            Comparison<BuffInfo> buffCompare = delegate (BuffInfo a, BuffInfo b)
            {
                return a.BuffKeyName.Equals("SuperKaiokenAuraProj") ? - 1 : a.BuffKeyName.CompareTo(b.BuffKeyName);
            };
            buffs.Sort(buffCompare);
            foreach (string projectileKey in buffs.SelectMany(x => x.ProjectileKeys))
            {
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, DBZMOD.DBZMOD.instance.ProjectileType(projectileKey), 0, 0, player.whoAmI);
            }            
        }

        // actually handle transforming. Takes quite a few steps to clean up after itself and do all the things.
        public static void DoTransform(Player player, BuffInfo buff, Mod mod, bool isOneStep)
        {
            MyPlayer modPlayer = MyPlayer.ModPlayer(player);

            bool isPoweringDown = buff == null;

            // don't.. try to apply the same transformation. This just stacks projectile auras and looks dumb.
            if (buff == GetCurrentTransformation(player, true, false) || buff == GetCurrentTransformation(player, false, true))
                return;

            // make sure to swap kaioken with super kaioken when appropriate.
            if (buff == SuperKaioken)
            {
                RemoveTransformation(player, Kaioken.BuffKeyName);
            }

            BuffInfo transformationToKeep = null;

            if (!isPoweringDown)
            {
                //if player tring to do a ssj transformation
                if (IsAnythingOtherThanKaioken(buff))
                {
                    //keep kaioken buff
                    transformationToKeep = GetCurrentTransformation(player, false, true);
                }
                else if (IsKaioken(buff))//if player trying to do a kaioken transformation
                {
                    //keep ssj buff
                    transformationToKeep = GetCurrentTransformation(player, true, false);
                }
            }

            // remove all *transformation* buffs from the player.
            // this needs to know we're powering down a step or not
            EndTransformations(player, isPoweringDown, isOneStep, transformationToKeep);

            // add whatever buff it is for a really long time.
            AddTransformation(player, buff.BuffKeyName, ABSURDLY_LONG_BUFF_DURATION);
        }

        public static void EndTransformations(Player player, bool isPoweringDown, bool isOneStep, BuffInfo transformationToKeep = null)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            // automatically applies debuffs.
            bool isOnlyRemovingKaioken = IsKaioken(player) && IsAnythingOtherThanKaioken(player);
            ClearAllTransformations(player, isPoweringDown, isOneStep, transformationToKeep);
            modPlayer.IsTransformationAnimationPlaying = false;
            modPlayer.TransformationFrameTimer = 0;

            // don't kill sounds if the only thing being removed is kaioken.
            if (!isOnlyRemovingKaioken)
                modPlayer.TransformationSoundInfo = SoundUtil.KillTrackedSound(modPlayer.TransformationSoundInfo); 
            
            modPlayer.IsTransforming = false;
        }

        public static void AddTransformation(Player player, string buffKeyName, int duration)
        {
            BuffInfo buff = GetBuffByKeyName(buffKeyName);
            player.AddBuff(buff.GetBuffId(), ABSURDLY_LONG_BUFF_DURATION, false);

            if (!string.IsNullOrEmpty(buff.TransformationText))
                CombatText.NewText(player.Hitbox, buff.TransformationTextColor, buff.TransformationText, false, false);

            // if the buff needs dust aura, do that.
            if (SSJBuffs().Contains(buff) || buff == ASSJ || buff == USSJ)
                player.GetModPlayer<MyPlayer>().SSJDustAura();

            if (!Main.dedServ && Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer) {
                NetworkHelper.formSync.SendFormChanges(256, player.whoAmI, player.whoAmI, buffKeyName, duration);
            }

            // find out if the player is already transformed - and if so, are we stacking Kaioken? If we are, we need to reapply some projectiles in a particular order.
            bool isKaioken = IsKaioken(buff);
            bool isPlayerCurrentlyTransformed = IsAnythingOtherThanKaioken(player);

            if (isKaioken && isPlayerCurrentlyTransformed)
            {
                List<BuffInfo> buffList = new List<BuffInfo> { buff, SuperKaioken };
                DoProjectileForBuff(player, buffList);
            } else
            {
                // create the projectile starter if applicable.
                DoProjectileForBuff(player, new List<BuffInfo> { buff });
            }

            // start the transformation animation, if one exists. This auto cancels if nothing is there to play.
            player.GetModPlayer<MyPlayer>().IsTransformationAnimationPlaying = true;

            if (buff.SoundKey != null)
                SoundUtil.PlayCustomSound(buff.SoundKey, player, buff.Volume);
        }

        // return the first located transformation of a given player. Assumes there should only ever be one, returns the first it finds.
        public static BuffInfo GetCurrentTransformation(Player player, bool isIgnoringKaioken, bool isIgnoringNonKaioken)
        {
            foreach (BuffInfo buff in AllBuffs())
            {
                if (IsKaioken(buff) && isIgnoringKaioken)
                    continue;

                if (IsAnythingOtherThanKaioken(buff) && isIgnoringNonKaioken)
                    continue;

                if (player.HasBuff(buff.GetBuffId()))
                {
                    return buff;
                }
            }

            // is the player transformed? Something bad may have happened.
            return null;
        }

        // based on some conditions, figure out what the next "step" of transformation should be.
        public static BuffInfo GetNextTransformationStep(Player player)
        {
            BuffInfo currentTransformation = GetCurrentTransformation(player, true, false);

            // SSJ1 is always the starting point if there isn't a current form tree to step through.
            if (currentTransformation == null)
                return SSJ1;

            // the player is legendary and doing a legendary step up.
            if (IsLSSJ(currentTransformation) && MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                for (int i = 0; i < LegendaryBuffs().Length; i++)
                {
                    if (LegendaryBuffs()[i] == currentTransformation && i < LegendaryBuffs().Length - 1)
                    {
                        return LegendaryBuffs()[i + 1];
                    }
                }
            }

            // the player isn't legendary and is doing a normal step up.
            if (IsSSJ(currentTransformation) && !MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                for (int i = 0; i < SSJBuffs().Length; i++)
                {
                    if (SSJBuffs()[i] == currentTransformation && i < SSJBuffs().Length - 1)
                    {
                        return SSJBuffs()[i + 1];
                    }
                }
            }

            // whatever happened here, the function couldn't find a next step. Either the player is maxed in their steps, or something bad happened.
            return null;
        }

        // based on some conditions, figure out what the previous "step" of transformation should be. Note this includes ascension stepping down.
        public static BuffInfo GetPreviousTransformationStep(Player player)
        {
            BuffInfo currentTransformation = GetCurrentTransformation(player, true, false);

            // the player is legendary and doing a legendary step down.
            if (IsLSSJ(currentTransformation) && MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                for (int i = 0; i < LegendaryBuffs().Length; i++)
                {
                    if (LegendaryBuffs()[i] == currentTransformation && i > 0)
                    {
                        return LegendaryBuffs()[i - 1];
                    }
                }
            }

            // the player isn't legendary and is doing a normal step down.
            if (IsSSJ(currentTransformation) && !MyPlayer.ModPlayer(player).IsPlayerLegendary())
            {
                for (int i = 0; i < SSJBuffs().Length; i++)
                {
                    if (SSJBuffs()[i] == currentTransformation && i > 0)
                    {
                        return SSJBuffs()[i - 1];
                    }
                }
            }

            // figure out what the step down for ascension should be, if the player is in an ascended form.
            if (IsAscended(currentTransformation))
            {
                for (int i = 0; i < AscensionBuffs().Length; i++)
                {
                    if (AscensionBuffs()[i] == currentTransformation && i > 0)
                    {
                        return AscensionBuffs()[i - 1];
                    }
                }
            }

            // either the player is at minimum or something bad has happened.
            return null;
        }

        // based on some conditions, figure out what the next "step" of ascension should be.
        public static BuffInfo GetNextAscensionStep(Player player)
        {
            BuffInfo currentTransformation = GetCurrentTransformation(player, true, false);

            if (IsAscended(currentTransformation) || IsSSJ1(player))
            {
                for (int i = 0; i < AscensionBuffs().Length; i++)
                {
                    if (AscensionBuffs()[i] == currentTransformation && i < AscensionBuffs().Length - 1)
                    {
                        return AscensionBuffs()[i + 1];
                    }
                }
            }

            return currentTransformation;
        }
    }
}
