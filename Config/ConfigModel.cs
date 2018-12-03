using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace Config
{
    static class ConfigModel
    {
        static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", "DBZMOD.json");
        static Preferences Configuration = new Preferences(ConfigPath);

        public static bool IsChargeToggled = false;

        public static void Load()
        {
            //Reading the config file
            bool success = ReadConfig();

            if (!success)
            {
                ErrorLogger.Log("Failed to read DBZMOD's config file! Recreating config...");
                CreateConfig();
            }
        }

        //Returns "true" if the config file was found and successfully loaded.
        static bool ReadConfig()
        {
            if (Configuration.Load())
            {
                Configuration.Get("IsChargeToggled", ref IsChargeToggled);
                return true;
            }
            return false;
        }

        //Creates a config file. This will only be called if the config file doesn't exist yet or it's invalid. 
        static void CreateConfig()
        {
            Configuration.Clear();
            Configuration.Put("IsChargeToggled", IsChargeToggled);
            Configuration.Save();
        }
    }
}
