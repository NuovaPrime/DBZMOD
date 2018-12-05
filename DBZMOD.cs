using DBZMOD.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using DBZMOD.Effects;
using Config;
using Util;
using Terraria.Graphics.Effects;

namespace DBZMOD
{
    public class DBZMOD : Mod
    {
        private UserInterface KiBarInterface;
        private KiBar kibar;
	    private OverloadBar overloadbar;
        private UserInterface OverloadBarInterface;
        private UIFlatPanel UIFlatPanel;

        private TransMenu transMenu;
        private UserInterface TransMenuInterface;

        private ProgressionMenu progressionMenu;
        private ResourceBar resourceBar;
        private UserInterface ProgressionMenuInterface;

        private DBZMOD mod;
        public bool thoriumLoaded;
        public bool tremorLoaded;
        public bool enigmaLoaded;
        public bool battlerodsLoaded;
        public bool expandedSentriesLoaded;
        public static DBZMOD instance;

        internal static CircleShader Circle;

        public DBZMOD()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }
        public override void Unload()
        {
            GFX.UnloadGFX();
            KiBar.visible = false;
            OverloadBar.visible = false;
            instance = null;
            TransMenu.menuvisible = false;
            ProgressionMenu.menuvisible = false;
            TransMenu.SSJ1On = false;
            TransMenu.SSJ2On = false;
            UIFlatPanel._backgroundTexture = null;
        }

        public override void Load()
        {
            // loads the mod's configuration file.
            ConfigModel.Load();

            //Loot.EMMLoader.RegisterMod(this);
            //Loot.EMMLoader.SetupContent(this);
            instance = this;

            tremorLoaded = ModLoader.GetMod("Tremor") != null;
            thoriumLoaded = ModLoader.GetMod("ThoriumMod") != null;
            enigmaLoaded = ModLoader.GetMod("Laugicality") != null;
            battlerodsLoaded = ModLoader.GetMod("UnuBattleRods") != null;
            expandedSentriesLoaded = ModLoader.GetMod("ExpandedSentries") != null;
            MyPlayer.KaiokenKey = RegisterHotKey("Kaioken", "J");
            MyPlayer.EnergyCharge = RegisterHotKey("Energy Charge", "C");
            MyPlayer.Transform = RegisterHotKey("Transform", "X");
            MyPlayer.PowerDown = RegisterHotKey("Power Down", "V");
            MyPlayer.SpeedToggle = RegisterHotKey("Speed Toggle", "Z");
            //MyPlayer.QuickKi = RegisterHotKey("Quick Ki", "N");
            MyPlayer.TransMenu = RegisterHotKey("Transformation Menu", "K");
            //MyPlayer.ProgressionMenuKey = RegisterHotKey("Progression Menu", "P");
            MyPlayer.FlyToggle = RegisterHotKey("Flight Toggle", "Q");
            MyPlayer.ArmorBonus = RegisterHotKey("Armor Bonus", "Y");
            if(!Main.dedServ)
            {
                GFX.LoadGFX(this);
                KiBar.visible = true;

                transMenu = new TransMenu();
                transMenu.Activate();
                TransMenuInterface = new UserInterface();
                TransMenuInterface.SetState(transMenu);

                progressionMenu = new ProgressionMenu();
                progressionMenu.Activate();
                ProgressionMenuInterface = new UserInterface();
                ProgressionMenuInterface.SetState(progressionMenu);

                kibar = new KiBar();
                kibar.Activate();
                KiBarInterface = new UserInterface();
                KiBarInterface.SetState(kibar);

                overloadbar = new OverloadBar();
                overloadbar.Activate();
                OverloadBarInterface = new UserInterface();
                OverloadBarInterface.SetState(overloadbar);

                Circle = new CircleShader(new Ref<Effect>(GetEffect("Effects/CircleShader")), "Pass1");

                Filters.Scene["DBZMOD:GodSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.1f, 0.1f).UseOpacity(0.7f), EffectPriority.VeryHigh);
                SkyManager.Instance["DBZMOD:GodSky"] = new GodSky();
            }
        }
        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " Ki Fragment", new int[]
            {
            ItemType("KiFragment1"),
            ItemType("KiFragment2"),
            ItemType("KiFragment3"),
            ItemType("KiFragment4"),
            ItemType("KiFragment5")
            });
            RecipeGroup.RegisterGroup("DBZMOD:KiFragment", group);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (TransMenuInterface != null && TransMenu.menuvisible)
            {
                TransMenuInterface.Update(gameTime);
            }

            if(ProgressionMenuInterface != null && ProgressionMenu.menuvisible)
            {
                progressionMenu.Update(gameTime);
            }

        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Contains("Resource Bars"));
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "DBZMOD: Ki Bar",
                    delegate
                    {
                        if (KiBar.visible)
                        {
                            KiBarInterface.Update(Main._drawInterfaceGameTime);
                            kibar.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            int index2 = layers.FindIndex(layer => layer.Name.Contains("Resource Bars"));
            if (index2 != -1)
            {
                layers.Insert(index2, new LegacyGameInterfaceLayer(
                    "DBZMOD: Trans Menu",
                    delegate
                    {
                        if (TransMenu.menuvisible)
                        {
                            TransMenuInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
                        }

                        if(ProgressionMenu.menuvisible)
                        {
                            ProgressionMenuInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
                        }

                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            int index3 = layers.FindIndex(layer => layer.Name.Contains("Resource Bars"));
            if (index3 != -1)
            {
                layers.Insert(index3, new LegacyGameInterfaceLayer(
                    "DBZMOD: Overload Bar",
                    delegate
                    {
                        if (OverloadBar.visible)
                        {
                            OverloadBarInterface.Update(Main._drawInterfaceGameTime);
                            overloadbar.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
 

