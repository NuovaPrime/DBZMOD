using DBZMOD.Enums;
using DBZMOD.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using DBZMOD.Buffs;
using DBZMOD.Config;
using DBZMOD.Extensions;
using DBZMOD.Network;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Util
{
    // class for helping out with all the buff integers, lists of buffs in order, presence of buffs/abstraction
    public static class FormBuffHelper
    {
        public const int ABSURDLY_LONG_BUFF_DURATION = 666666;

        // the typical color used for super saiyan transformation Text, except God
        public static readonly Color defaultTransformationTextColor = new Color(219, 219, 48);

        public static readonly Color godTransformationTextColor = new Color(229, 20, 51);

        // the following are cached info classes that get passed around for all sorts of things.
        public static readonly BuffInfo 
            ssj1 = new BuffInfo(MenuSelectionID.SSJ1, BuffKeyNames.ssj1, "Super Saiyan 1", defaultTransformationTextColor, true, BuffKeyNames.ssj1),
            assj = new BuffInfo(MenuSelectionID.None, BuffKeyNames.assj, GetASSJNamePreference(), defaultTransformationTextColor, true, BuffKeyNames.ssj1),
            ussj = new BuffInfo(MenuSelectionID.None, BuffKeyNames.ussj, GetUSSJNamePreference(), defaultTransformationTextColor, true, BuffKeyNames.ssj1),
            ssj2 = new BuffInfo(MenuSelectionID.SSJ2, BuffKeyNames.ssj2, "Super Saiyan 2", defaultTransformationTextColor, true, BuffKeyNames.ssj2),
            ssj3 = new BuffInfo(MenuSelectionID.SSJ3, BuffKeyNames.ssj3, "Super Saiyan 3", defaultTransformationTextColor, true, BuffKeyNames.ssj3),
            ssjg = new BuffInfo(MenuSelectionID.SSJG, BuffKeyNames.ssjg, "Super Saiyan God", godTransformationTextColor, true, BuffKeyNames.ssjg),
            ssjb = new BuffInfo(MenuSelectionID.Ssjb, BuffKeyNames.ssjb, null, defaultTransformationTextColor, true),
            lssj = new BuffInfo(MenuSelectionID.LSSJ1, BuffKeyNames.lssj, "Legendary Super Saiyan", defaultTransformationTextColor, true, BuffKeyNames.lssj),
            lssj2 = new BuffInfo(MenuSelectionID.LSSJ2, BuffKeyNames.lssj2, "Legendary Super Saiyan 2", defaultTransformationTextColor, true, BuffKeyNames.lssj2),
            kaioken = new BuffInfo(MenuSelectionID.None, BuffKeyNames.kaioken, null, defaultTransformationTextColor),
            superKaioken = new BuffInfo(MenuSelectionID.None, BuffKeyNames.superKaioken, null, defaultTransformationTextColor, true, BuffKeyNames.ssj1),
            kaiokenFatigue = new BuffInfo(MenuSelectionID.None, BuffKeyNames.kaiokenFatigue, null, defaultTransformationTextColor),
            transformationExhaustion = new BuffInfo(MenuSelectionID.None, BuffKeyNames.transformationExhaustion, null, defaultTransformationTextColor),
            spectrum = new BuffInfo(MenuSelectionID.Spectrum, BuffKeyNames.spectrum, "Super Saiyan Spectrum", defaultTransformationTextColor, true, BuffKeyNames.spectrum);

        public static readonly BuffInfo[]
            buffInfoList = {ssj1, ssj2, ssj3, ssjg, lssj, lssj2, assj, ussj, kaioken, superKaioken, kaiokenFatigue, transformationExhaustion, spectrum},

            // returns a list of transformation steps specific to non-legendary SSJ players
            ssjBuffs = {ssj1, ssj2, ssj3, ssjg},

            // a list of transformation steps from SSJ1 through ascended SSJ forms
            ascensionBuffs = { ssj1, assj, ussj },

            // a list of transformation steps specific to legendary SSJ players
            legendaryBuffs = {ssj1, lssj, lssj2};

        public static string GetASSJNamePreference()
        {
            return ConfigModel.isSaiyanGradeNames ? "Super Saiyan Grade 2" : "Ascended Super Saiyan";
        }

        public static string GetUSSJNamePreference()
        {
            return ConfigModel.isSaiyanGradeNames ? "Super Saiyan Grade 3" : "Ultra Super Saiyan";
        }

        // returns the buff Id of a transformation menu selection
        public static BuffInfo GetBuffFromMenuSelection(MenuSelectionID menuId)
        {
            return buffInfoList.FirstOrDefault(x => x.menuId == menuId);
        }

        public static BuffInfo GetBuffByKeyName(string keyName)
        {
            return buffInfoList.FirstOrDefault(x => x.buffKeyName == keyName);
        }

        // list containing all the form buffs that aren't debuffs.
        public static List<BuffInfo> AllBuffs()
        {
            return buffInfoList.Where(x => x.buffKeyName != BuffKeyNames.kaiokenFatigue && x.buffKeyName != BuffKeyNames.transformationExhaustion).ToList();
        }
    }
}
