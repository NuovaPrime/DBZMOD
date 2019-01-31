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
using DBZMOD.Transformations;
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

        internal static TransformationDefinition[]
            transformationDefinitionList =
            {
                TransformationDefinitionManager.KaiokenDefinition,
                TransformationDefinitionManager.SuperKaiokenDefinition,
                TransformationDefinitionManager.KaiokenFatigueDefinition,

                TransformationDefinitionManager.SSJ1Definition,
                TransformationDefinitionManager.ASSJDefinition,
                TransformationDefinitionManager.USSJDefinition,

                TransformationDefinitionManager.SSJ2Definition,
                TransformationDefinitionManager.SSJ3Definition,
                TransformationDefinitionManager.SSJGDefinition,

                TransformationDefinitionManager.LSSJDefinition,
                TransformationDefinitionManager.LSSJ2Definition,

                TransformationDefinitionManager.TransformationExhaustionDefinition,
                TransformationDefinitionManager.SpectrumDefinition
            },

            // returns a list of transformation steps specific to non-legendary SSJ players
            ssjBuffs =
            {
                TransformationDefinitionManager.SSJ1Definition,
                TransformationDefinitionManager.SSJ2Definition,
                TransformationDefinitionManager.SSJ3Definition,
                TransformationDefinitionManager.SSJGDefinition
            },

            // a list of transformation steps from SSJ1 through ascended SSJ forms
            ascensionBuffs =
            {
                TransformationDefinitionManager.SSJ1Definition,
                TransformationDefinitionManager.ASSJDefinition,
                TransformationDefinitionManager.USSJDefinition
            },

            // a list of transformation steps specific to legendary SSJ players
            legendaryBuffs =
            {
                TransformationDefinitionManager.SSJ1Definition,
                TransformationDefinitionManager.LSSJDefinition,
                TransformationDefinitionManager.LSSJ2Definition
            };

        public static void Initialize()
        {

        }

        public static string GetASSJNamePreference()
        {
            return ConfigModel.isSaiyanGradeNames ? "Super Saiyan Grade 2" : "Ascended Super Saiyan";
        }

        public static string GetUSSJNamePreference()
        {
            return ConfigModel.isSaiyanGradeNames ? "Super Saiyan Grade 3" : "Ultra Super Saiyan";
        }

        // returns the buff Id of a transformation menu selection
        public static TransformationDefinition GetBuffFromMenuSelection(MenuSelectionID menuId)
        {
            return transformationDefinitionList.FirstOrDefault(x => x.MenuId == menuId);
        }

        public static TransformationDefinition GetBuffByKeyName(string keyName)
        {
            return transformationDefinitionList.FirstOrDefault(x => x.UnlocalizedName == keyName);
        }

        // list containing all the form buffs that aren't debuffs.
        public static List<TransformationDefinition> AllBuffs()
        {
            return transformationDefinitionList.Where(x => x.UnlocalizedName != BuffKeyNames.kaiokenFatigue && x.UnlocalizedName != BuffKeyNames.transformationExhaustion).ToList();
        }

        public static TransformationDefinitionManager TransformationDefinitionManager => DBZMOD.instance.TransformationDefinitionManager;
    }
}
