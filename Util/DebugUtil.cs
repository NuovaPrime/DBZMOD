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
        public static bool isDebug = false;
        public static void Log(string someString)
        {
            if (!isDebug)
                return;
            if (Main.netMode == NetmodeID.Server)
                Console.WriteLine(someString);
            else
                Main.NewText(someString);
        }
    }
}
