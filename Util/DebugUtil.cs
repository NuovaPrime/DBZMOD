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
        public static bool isDebug = true;
        public static void Log(string someString)
        {
            if (!isDebug)
                return;
            if (Main.netMode == NetmodeID.Server)
                Console.WriteLine(someString);
            else
                Main.NewText(someString);
        }

        // helper utils to throttle output spam by only displaying things on a per-time basis.
        public static bool IsSecondElapsed()
        {
            return IsTimeElapsed(60);
        }

        public static bool IsTimeElapsed(int frames)
        {
            if (frames == 0)
                return false;
            return Main.time % frames == 0;
        }
    }
}
