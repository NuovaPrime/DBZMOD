using System.Reflection;
using Terraria.ModLoader;

namespace DBZMOD.Utilities
{
    public static class SteamHelper
    {
        public static void Initialize()
        {
            SteamID64 = typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null).ToString();
            ;
        }

        public static string SteamID64 { get; private set; }
    }
}
