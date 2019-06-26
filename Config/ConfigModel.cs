using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace DBZMOD.Config
{
    static class ConfigModel
    {
        static string _configPath = Path.Combine(Main.SavePath, "Mod Configs", "DBZMOD.json");
        static Preferences _configuration = new Preferences(_configPath);

        public static bool isChargeToggled = false;

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
            if (_configuration.Load())
            {
                _configuration.Get("IsChargeToggled", ref isChargeToggled);
                return true;
            }
            return false;
        }

        //Creates a config file. This will only be called if the config file doesn't exist yet or it's invalid. 
        static void CreateConfig()
        {
            _configuration.Clear();
            _configuration.Put("IsChargeToggled", isChargeToggled);
            _configuration.Save();
        }
    }
}
