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
    public class StoneBlockDestruction : BaseFloatingDestructionProj
    {     
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Block Destruction");
        }
    }
}