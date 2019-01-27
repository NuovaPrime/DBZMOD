using System;
using Terraria;
using Terraria.ID;

namespace DBZMOD.Util
{
    static class DebugHelper
    {
        private static bool _isDebug = true;
        public static bool IsDebugModeOn()
        {
            return _isDebug;
        }
        public static void Log(string someString)
        {
            if (!_isDebug)
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

        public static bool DragonRadarDebug(Player player)
        {
            if (!IsDebugModeOn())
                return true;
            var world = DBZWorld.GetWorld();
            var count = 0;
            for (var i = 0; i < Main.maxTilesX; i++)
            {
                for (var j = 0; j < Main.maxTilesY; j++)
                {
                    if (world.IsDragonBallLocation(i, j))
                    {
                        Log($"Dragon ball found at {i} {j}");
                        count++;
                    }
                }
            }
            Log($"Debug count of dragon balls returned {count}");
            return true;
        }
    }
}
