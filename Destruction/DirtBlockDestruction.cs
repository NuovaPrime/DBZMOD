﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Destruction
{
    public class DirtBlockDestruction : BaseFloatingDestructionProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Base Block Destruction");
        }
    }
}