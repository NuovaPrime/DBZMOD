using System.Collections.Generic;
using System.Linq;
using DBZMOD.Models;
using Terraria;

namespace DBZMOD.Util
{
    public class HitStunManager
    {
        private List<HitStunInfo> _trackedHitStuns = new List<HitStunInfo>();

        public void Update()
        {
            // remove any hitstuns from the tracker that have expired, for any reason.
            _trackedHitStuns.RemoveAll(x => x.IsExpired());

            _trackedHitStuns.ForEach(x => x.ApplyHitstun());
        }

        public bool IsExistingTrackedStun(NPC target)
        {
            return _trackedHitStuns.Any(x => x.targetId == target.whoAmI);
        }

        public void DoHitStun(NPC target, int duration, float slowedTo, float recoversDuringFramePercent)
        {
            if (IsExistingTrackedStun(target))
            {
                return;
            }

            _trackedHitStuns.Add(new HitStunInfo(target, duration, slowedTo, recoversDuringFramePercent));
        }
    }
}