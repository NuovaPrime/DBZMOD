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

        public static string GetAssjNamePreference()
        {
            return ConfigModel.isSaiyanGradeNames ? "Super Saiyan Grade 2" : "Ascended Super Saiyan";
        }

        public static string GetUssjNamePreference()
        {
            return ConfigModel.isSaiyanGradeNames ? "Super Saiyan Grade 3" : "Ultra Super Saiyan";
        }

        // the typical color used for super saiyan transformation Text, except God
        public static readonly Color defaultTransformationTextColor = new Color(219, 219, 48);

        public static readonly Color godTransformationTextColor = new Color(229, 20, 51);

        // the following are cached info classes that get passed around for all sorts of things.
        public static readonly BuffInfo ssj1 = new BuffInfo(MenuSelectionID.SSJ1, BuffKeyNames.ssj1, "Super Saiyan 1", defaultTransformationTextColor);
        public static readonly BuffInfo ssj2 = new BuffInfo(MenuSelectionID.SSJ2, BuffKeyNames.ssj2, "Super Saiyan 2", defaultTransformationTextColor);
        public static readonly BuffInfo ssj3 = new BuffInfo(MenuSelectionID.SSJ3, BuffKeyNames.ssj3, "Super Saiyan 3", defaultTransformationTextColor);
        public static readonly BuffInfo ssjg = new BuffInfo(MenuSelectionID.SSJG, BuffKeyNames.ssjg, "Super Saiyan God", godTransformationTextColor);
        public static readonly BuffInfo ssjb = new BuffInfo(MenuSelectionID.Ssjb, BuffKeyNames.ssjb, null, defaultTransformationTextColor);
        public static readonly BuffInfo lssj = new BuffInfo(MenuSelectionID.LSSJ1, BuffKeyNames.lssj, "Legendary Super Saiyan", defaultTransformationTextColor);
        public static readonly BuffInfo lssj2 = new BuffInfo(MenuSelectionID.LSSJ2, BuffKeyNames.lssj2, "Legendary Super Saiyan 2", defaultTransformationTextColor);
        public static readonly BuffInfo assj = new BuffInfo(MenuSelectionID.None, BuffKeyNames.assj, GetAssjNamePreference(), defaultTransformationTextColor);
        public static readonly BuffInfo ussj = new BuffInfo(MenuSelectionID.None, BuffKeyNames.ussj, GetUssjNamePreference(), defaultTransformationTextColor);
        public static readonly BuffInfo kaioken = new BuffInfo(MenuSelectionID.None, BuffKeyNames.kaioken, null, defaultTransformationTextColor);
        public static readonly BuffInfo superKaioken = new BuffInfo(MenuSelectionID.None, BuffKeyNames.superKaioken, null, defaultTransformationTextColor);
        public static readonly BuffInfo kaiokenFatigue = new BuffInfo(MenuSelectionID.None, BuffKeyNames.kaiokenFatigue, null, defaultTransformationTextColor);
        public static readonly BuffInfo transformationExhaustion = new BuffInfo(MenuSelectionID.None, BuffKeyNames.transformationExhaustion, null, defaultTransformationTextColor);
        public static readonly BuffInfo spectrum = new BuffInfo(MenuSelectionID.Spectrum, BuffKeyNames.spectrum, "Super Saiyan Spectrum", defaultTransformationTextColor);

        public static readonly BuffInfo[] buffInfoList = { ssj1, ssj2, ssj3, ssjg, lssj, lssj2, assj, ussj, kaioken, superKaioken, kaiokenFatigue, transformationExhaustion, spectrum };

        // returns the buff Id of a transformation menu selection
        public static BuffInfo GetBuffFromMenuSelection(MenuSelectionID menuId)
        {
            return buffInfoList.FirstOrDefault(x => x.menuId == menuId);
        }

        public static BuffInfo GetBuffByKeyName(string keyName)
        {
            return buffInfoList.FirstOrDefault(x => x.buffKeyName == keyName);
        }

        // returns a list of transformation steps specific to non-legendary SSJ players
        public static readonly BuffInfo[] ssjBuffs = { ssj1, ssj2, ssj3, ssjg };


        // a list of transformation steps specific to legendary SSJ players
        public static readonly BuffInfo[] legendaryBuffs = {ssj1, lssj, lssj2};

        // a list of transformation steps from SSJ1 through ascended SSJ forms
        public static BuffInfo[] ascensionBuffs = {ssj1, assj, ussj};

        // A bunch of lists joined together, in order to contain every possible transformation buff.
        // when adding new ones, make sure they wind up here in some form, even if you just have to add them one at a time.
        // (Union() excludes duplicates automatically)
        public static List<BuffInfo> AllBuffs()
        {
            return buffInfoList.Where(x => x.buffKeyName != BuffKeyNames.kaiokenFatigue && x.buffKeyName != BuffKeyNames.transformationExhaustion).ToList();
        }
    }
}
