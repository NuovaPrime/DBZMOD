using DBZMOD.Models;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Effects.Animations.Aura
{
    public class AuraDrawLayer : PlayerLayer
    {
        AuraAnimationInfo Aura;

        public AuraDrawLayer(string mod, string name, PlayerLayer parent, AuraAnimationInfo aura, Action<PlayerDrawInfo> layer) : base(mod, name, parent, layer)
        {
            Aura = aura;
        }
    }
}
