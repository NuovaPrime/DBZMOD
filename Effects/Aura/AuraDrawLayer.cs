using DBZMOD.Models;
using System;
using Terraria.ModLoader;

namespace DBZMOD.Effects.Animations.Aura
{
    public class AuraDrawLayer : PlayerLayer
    {
        AuraAnimationInfo _aura;

        public AuraDrawLayer(string mod, string name, PlayerLayer parent, AuraAnimationInfo aura, Action<PlayerDrawInfo> layer) : base(mod, name, parent, layer)
        {
            _aura = aura;
        }
    }
}
