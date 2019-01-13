using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Util
{
    static class DebugUtil
    {
        private static bool isDebug = true;
        public static bool IsDebugModeOn()
        {
            return isDebug;
        }
        public static void Log(string someString)
        {
            if (!isDebug)
                return;
            if (Main.netMode == NetmodeID.Server)
                Console.WriteLine(someString);
            else
                Main.NewText(someString);
        }

        //public static void ServerLog(string someString)
        //{
        //    if (Main.netMode == NetmodeID.Server)
        //        Console.WriteLine(someString);
        //}

        // helper utils to throttle output spam by only displaying things on a per-time basis.
        public static bool IsSecondElapsed()
        {
            return IsTimeElapsed(60);
        }

        public static bool IsTimeElapsed(int frames)
        {
            return DBZMOD.IsTickRateElapsed(frames);
        }
    }
}
