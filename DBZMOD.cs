using DBZMOD.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using DBZMOD.Effects;
using Terraria.Graphics.Effects;
using System.IO;
using DBZMOD.Config;
using DBZMOD.Network;
using DBZMOD.Traits;
using DBZMOD.Transformations;
using DBZMOD.Utilities;
using Leveled;

namespace DBZMOD
{
    public class DBZMOD : Mod
    {
        private UIFlatPanel _uiFlatPanel;

        private static TransformationMenu _transformationMenu;
        private static UserInterface _transMenuInterface;

        private static WishMenu _wishMenu;
        private static UserInterface _wishMenuInterface;

        private static KiBar _kibar;
        private static UserInterface _kiBarInterface;

        private static ProgressionMenu _progressionMenu;
        private static UserInterface _progressionMenuInterface;

        private static OverloadBar _overloadbar;
        private static UserInterface _overloadBarInterface;

        private static InstantTransmissionMapHelper _instantTransmissionMapTeleporter;

        internal static CircleShader circle;

        private ResourceBar _resourceBar;

        public bool thoriumLoaded;
        public bool tremorLoaded;
        public bool enigmaLoaded;
        public bool battlerodsLoaded;
        public bool expandedSentriesLoaded;

        private TransformationDefinitionManager _transformationDefinitionManager;
        private TraitManager _traitManager;

        public DBZMOD()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };

            Instance = this;
        }

        public override void Load()
        {
            // loads the mod's configuration file.
            ConfigModel.Load();

            //Loot.EMMLoader.RegisterMod(this);
            //Loot.EMMLoader.SetupContent(this);

            tremorLoaded = ModLoader.GetMod("Tremor") != null;
            thoriumLoaded = ModLoader.GetMod("ThoriumMod") != null;
            enigmaLoaded = ModLoader.GetMod("Laugicality") != null;
            battlerodsLoaded = ModLoader.GetMod("UnuBattleRods") != null;
            expandedSentriesLoaded = ModLoader.GetMod("ExpandedSentries") != null;

            MyPlayer.kaiokenKey = RegisterHotKey("Kaioken", "J");
            MyPlayer.energyCharge = RegisterHotKey("Energy Charge", "C");
            MyPlayer.transform = RegisterHotKey("Transform", "X");
            MyPlayer.powerDown = RegisterHotKey("Power Down", "V");
            MyPlayer.speedToggle = RegisterHotKey("Speed Toggle", "Z");
            MyPlayer.quickKi = RegisterHotKey("Quick Ki", "N");
            MyPlayer.transMenu = RegisterHotKey("Transformation Menu", "K");
            MyPlayer.instantTransmission = RegisterHotKey("Instant Transmission", "I");
            //MyPlayer.ProgressionMenuKey = RegisterHotKey("Progression Menu", "P");
            MyPlayer.flyToggle = RegisterHotKey("Flight Toggle", "Q");
            MyPlayer.armorBonus = RegisterHotKey("Armor Bonus", "Y");

            if (!Main.dedServ)
            {
                GFX.LoadGFX(this);
                KiBar.visible = true;

                ActivateTransMenu();
                ActivateWishmenu();
                ActivateProgressionMenu();
                ActivateKiBar();
                ActivateOverloadBar();

                _instantTransmissionMapTeleporter = new InstantTransmissionMapHelper();

                circle = new CircleShader(new Ref<Effect>(GetEffect("Effects/CircleShader")), "Pass1");

                Filters.Scene["DBZMOD:GodSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.1f, 0.1f).UseOpacity(0.7f), EffectPriority.VeryHigh);
                SkyManager.Instance["DBZMOD:GodSky"] = new GodSky();
                Filters.Scene["DBZMOD:WishSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.1f, 0.1f, 0.1f).UseOpacity(0.7f), EffectPriority.VeryHigh);
                SkyManager.Instance["DBZMOD:WishSky"] = new WishSky();
            }
        }

        public override void Unload()
        {
            TransformationDefinitionManager.Clear();

            GFX.UnloadGFX();
            KiBar.visible = false;
            OverloadBar.visible = false;
            Instance = null;
            TransformationMenu.menuVisible = false;
            ProgressionMenu.menuvisible = false;
            WishMenu.menuVisible = false;
            UIFlatPanel.backgroundTexture = null;

            Leveled = null;
        }

        public override void PostSetupContent()
        {
            Leveled = ModLoader.GetMod("Leveled");

            if (Leveled != null)
                LeveledSupport.Initialize();
        }

        public static void ActivateTransMenu()
        {
            _transformationMenu = new TransformationMenu();
            _transformationMenu.Activate();
            _transMenuInterface = new UserInterface();
            _transMenuInterface.SetState(_transformationMenu);
        }

        public static void ActivateWishmenu()
        {
            _wishMenu = new WishMenu();
            _wishMenu.Activate();
            _wishMenuInterface = new UserInterface();
            _wishMenuInterface.SetState(_wishMenu);
        }

        public static void ActivateProgressionMenu()
        {
            _progressionMenu = new ProgressionMenu();
            _progressionMenu.Activate();
            _progressionMenuInterface = new UserInterface();
            _progressionMenuInterface.SetState(_progressionMenu);
        }

        public static void ActivateKiBar()
        {
            _kibar = new KiBar();
            _kibar.Activate();
            _kiBarInterface = new UserInterface();
            _kiBarInterface.SetState(_kibar);
        }

        public static void ActivateOverloadBar()
        {
            _overloadbar = new OverloadBar();
            _overloadbar.Activate();
            _overloadBarInterface = new UserInterface();
            _overloadBarInterface.SetState(_overloadbar);
        }

        public override void AddRecipeGroups()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " Ki Fragment", new int[]
            {
                ItemType("KiFragment1"),
                ItemType("KiFragment2"),
                ItemType("KiFragment3"),
                ItemType("KiFragment4"),
                ItemType("KiFragment5")
            });
#pragma warning restore CS0618 // Type or member is obsolete

            RecipeGroup.RegisterGroup("DBZMOD:KiFragment", group);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (_transMenuInterface != null && TransformationMenu.menuVisible)
            {
                _transMenuInterface.Update(gameTime);
            }

            if (_wishMenuInterface != null && WishMenu.menuVisible)
            {
                _wishMenuInterface.Update(gameTime);
            }

            if (_progressionMenuInterface != null && ProgressionMenu.menuvisible)
            {
                _progressionMenu.Update(gameTime);
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
                            _kiBarInterface.Update(Main._drawInterfaceGameTime);
                            _kibar.Draw(Main.spriteBatch);
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
                    "DBZMOD: Menus",
                    delegate
                    {
                        if (TransformationMenu.menuVisible)
                        {
                            _transMenuInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
                        }

                        if (ProgressionMenu.menuvisible)
                        {
                            _progressionMenuInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
                        }

                        if (WishMenu.menuVisible)
                        {
                            _wishMenuInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
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
                            _overloadBarInterface.Update(Main._drawInterfaceGameTime);
                            _overloadbar.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        // unabashedly stolen from Jopo with love, responsible for the instant transmission functionality we want out of book 1 with some assembly required
        public override void PostDrawFullscreenMap(ref string mouseText)
        {
            InstantTransmissionMapHelper.instance.PostDrawFullScreenMap();
        }

        // packet handling goes here
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetworkHelper.HandlePacket(reader, whoAmI);
        }

        public static uint GetTicks()
        {
            return Main.GameUpdateCount;
        }

        public static bool IsTickRateElapsed(int i)
        {
            return GetTicks() > 0 && GetTicks() % i == 0;
        }

        public TransformationDefinitionManager TransformationDefinitionManager
        {
            get
            {
                if (_transformationDefinitionManager == null) _transformationDefinitionManager = new TransformationDefinitionManager();
                if (!_transformationDefinitionManager.Initialized) _transformationDefinitionManager.DefaultInitialize();

                return _transformationDefinitionManager;
            }
        }

        public TraitManager TraitManager
        {
            get
            {
                if (_traitManager == null) _traitManager = new TraitManager();
                if (!_traitManager.Initialized) _traitManager.DefaultInitialize();

                return _traitManager;
            }
        }

        internal static DBZMOD Instance { get; private set; }

        internal static Mod Leveled { get; private set; }
    }
}


