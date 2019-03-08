using System;
using System.Reflection;
using Terraria.ModLoader;

namespace DBZMOD.Utilities
{
    public static class SteamHelper
    {
        public static void Initialize()
        {
            try
            {
                SteamID64 = typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null).ToString();
            }
            catch (Exception)
            {
                //Console.WriteLine("Unable to fetch SteamID, assuming no steam is present.");
            }
        }

        public static string SteamID64 { get; private set; }
    }
}
