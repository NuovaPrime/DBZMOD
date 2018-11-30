using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using DBZMOD.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics;
using Microsoft.Xna.Framework;
using DBZMOD.Projectiles;
using Terraria.ModLoader.IO;
using Terraria.ID;
using DBZMOD;
using DBZMOD.Util;
using DBZMOD.Enums;

namespace DBZMOD
{
    class FistSystem
    {
        #region Variables
        private TriggersPack triggersPack;
        private bool IsDashLeftJustPressed;
        private bool IsDashLeftGapped;
        private bool IsDashRightJustPressed;
        private bool IsDashRightGapped;
        private bool IsDashUpJustPressed;
        private bool IsDashUpGapped;
        private bool IsDashDownJustPressed;
        private bool IsDashDownGapped;
        private bool IsDashDiagonalUpHeld;
        private bool IsDashDiagonalDownHeld;
        private int DashTimer;
        private int HoldTimer;
        private int FlurryTimer;
        private int BlockTimer;
        private bool LightPunchPressed;
        private bool LightPunchHeld;
        public bool EyeDowned;
        public bool BeeDowned;
        public bool WallDowned;
        public bool PlantDowned;
        public bool DukeDowned;
        public bool MoonlordDowned;
        private int BasicPunchDamage;
        private int HeavyPunchDamage;
        private int FlurryPunchDamage;
        private int ShootSpeed;
        #endregion

        public void Update(TriggersSet triggersSet, Player player, Mod mod)
        {
            Vector2 projvelocity = Vector2.Normalize(Main.MouseWorld - player.position) * ShootSpeed;

            // returns a list of actions to be performed based on trigger states.            
            var actionsToPerform = ControlHelper.ProcessInputs(triggersSet);

            #region Mouse Clicks
            if (actionsToPerform.BlockPhase1)//both click, for blocking
            {
                MyPlayer.ModPlayer(player).BlockState = 1;
            }
            else if (actionsToPerform.BlockPhase2)
            {
                MyPlayer.ModPlayer(player).BlockState = 2;
            }
            else if (actionsToPerform.BlockPhase3)
            {
                MyPlayer.ModPlayer(player).BlockState = 3;
            } else
            {
                MyPlayer.ModPlayer(player).BlockState = 0;
                if (actionsToPerform.Flurry && MyPlayer.ModPlayer(player).CanUseFlurry)
                {
                    // Do Flurry
                }
                else if (actionsToPerform.LightAttack)
                {
                    ShootSpeed = 2;
                    Projectile.NewProjectile(player.position, projvelocity, BasicFistProjSelect(mod), BasicPunchDamage, 5);
                }
                else if (actionsToPerform.HeavyAttack)
                {
                    if (!player.HasBuff(mod.BuffType("HeavyPunchCooldown")) && MyPlayer.ModPlayer(player).CanUseHeavyHit)
                    {
                        Projectile.NewProjectile(player.position, projvelocity, mod.ProjectileType("KiFistProjHeavy"), HeavyPunchDamage, 50);
                    }
                }
            }
            #endregion

            #region Dash Checks
            if (actionsToPerform.DashUp)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(player, Controls.Up);
            }
            if (actionsToPerform.DashDown)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(player, Controls.Down);
            }
            if (actionsToPerform.DashLeft)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(player, Controls.Left);
            }
            if (actionsToPerform.DashRight)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(player, Controls.Right);
            }
            if (actionsToPerform.DashUpLeft)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(player, Controls.Up, Controls.Left);
            }
            if (actionsToPerform.DashUpRight)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(player, Controls.Up, Controls.Right);
            }
            if (actionsToPerform.DashDownLeft)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(player, Controls.Down, Controls.Left);
            }
            if (actionsToPerform.DashDownRight)
            {
                MyPlayer.ModPlayer(player).IsDashing = true;
                PerformZanzoken(player, Controls.Down, Controls.Right);
            }
            #endregion

            #region boss downed bools
            if (NPC.downedBoss1)
            {
                EyeDowned = true;
            }
            if (NPC.downedQueenBee)
            {
                BeeDowned = true;
            }
            if (Main.hardMode)
            {
                WallDowned = true;
            }
            if (NPC.downedPlantBoss)
            {
                PlantDowned = true;
            }
            if (NPC.downedFishron)
            {
                DukeDowned = true;
            }
            if (NPC.downedMoonlord)
            {
                MoonlordDowned = true;
            }
            #endregion

            #region Stat Checks
            BasicPunchDamage = 8;
            HeavyPunchDamage = BasicPunchDamage * 3;
            FlurryPunchDamage = BasicPunchDamage / 2;
            if (EyeDowned)
            {
                BasicPunchDamage += 6;
            }
            if (BeeDowned)
            {
                BasicPunchDamage += 8;
            }
            if (WallDowned)
            {
                BasicPunchDamage += 26;
            }
            if (PlantDowned)
            {
                BasicPunchDamage += 32;
            }
            if (DukeDowned)
            {
                BasicPunchDamage += 28;
            }
            if (MoonlordDowned)
            {
                BasicPunchDamage += 124;
            }

            #endregion

        }
        public int BasicFistProjSelect(Mod mod)
        {
            switch (Main.rand.Next((4)))
            {
                case 0:
                    return mod.ProjectileType("KiFistProj1");
                case 1:
                    return mod.ProjectileType("KiFistProj2");
                case 2:
                    return mod.ProjectileType("KiFistProj3");
                case 3:
                    return mod.ProjectileType("KiFistProj4");
                default:
                    return 0;

            }
        }

        // change this to change how far the player can teleport.
        public const float BASE_ZANZOKEN_TRAVEL_DISTANCE = 200f;

        public float GetZanzokenDistance()
        {
            // TODO
            // insert things that affect zanzoken distance here, like accessories or unlocked abilities.
            return BASE_ZANZOKEN_TRAVEL_DISTANCE;
        }

        // returns the vertical/horizontal Vector offsets of a 45 degree angle that travels ZANZOKEN_TRAVEL_DISTANCE.
        private float GetZanzokenDiagonalDistanceComponent()
        {
            var hypotenuse = GetZanzokenDistance();
            var componentDistance = (float)Math.Sqrt((hypotenuse * hypotenuse) / 2);
            return componentDistance;
        }

        // returns a major rectangle responsible for the long range area in the teleport "cone" to scan for enemies.
        private float GetZanzokenScanMajorSize()
        {
            var rectangleBisectWidth = (float)GetZanzokenDiagonalDistanceComponent() / 2f;
            return rectangleBisectWidth;
        }

        // returns a minor rectangle responsible for the close area in the teleport "cone" to scan for enemies.
        private float GetZanzokenScanMinorSize()
        {
            var rectangleBisectWidth = (float)GetZanzokenScanMajorSize() / 2f;
            return rectangleBisectWidth;
        }

        private bool CanZanzoken(Player player)
        {
            return !player.frozen && !player.stoned && !player.HasBuff(BuffID.Cursed);
        }

        public void PerformZanzoken(Player player, params Controls[] directions)
        {
            // checks that would prevent you from using Zanzoken
            if (!CanZanzoken(player))
            {
                return;
            }

            // if the directions array contains more than one parameter, this is a diagonal zanzoken.
            float offset = GetZanzokenDistance();
            if (directions.Length > 1)
            {
                offset = GetZanzokenDiagonalDistanceComponent();
            }

            // get the player's origin
            Vector2 origin = new Vector2(player.position.X, player.position.Y);

            // TODO  !!!
            // spawn the zanzoken projectile/effect at the player's origin

            // lazy switch to list so I can use Contains.
            List<Controls> directionList = new List<Controls>(directions);

            // figure out where the player is going to try and end up.
            float yOffset = (directionList.Contains(Controls.Up) ? -1 : (directionList.Contains(Controls.Down) ? 1 : 0)) * offset;
            float xOffset = (directionList.Contains(Controls.Left) ? -1 : (directionList.Contains(Controls.Right) ? 1 : 0)) * offset;

            // this is a lazy hack, do a pixel by pixel "scan" until the player hits something, check if its an enemy and then place them in an uncollided position
            float stepMaximum = Math.Max(Math.Abs(xOffset), Math.Abs(yOffset));
            float xStep = xOffset / stepMaximum;
            float yStep = yOffset / stepMaximum;
            for (float f = 0f; f < stepMaximum; f += 1.0f)
            {
                float xPos = xStep * f;
                float yPos = yStep * f;
                Vector2 newPosition = origin + new Vector2(xPos, yPos);

                // do a tile collision check. if this returns anything other than new position, we have collision.
                bool isCollided = Collision.SolidCollision(newPosition, player.width, player.height);
                
                if (isCollided)
                {
                    // we collided with a thing.
                    return;
                }

                player.position = newPosition;
                
                foreach(NPC npc in Main.npc)
                {
                    if (!npc.active || npc.friendly)
                        continue;
                    var npcRect = npc.getRect();
                    var playerRect = player.getRect();
                    if (npcRect.Intersects(playerRect))
                    {
                        // do npc-based teleport to a safe position
                        PerformSafeZanzokenToNPC(player, npc);
                        return;
                    }
                }                
            }
        }

        public void PerformSafeZanzokenToNPC(Player player, NPC npc)
        {
            // TODO
            // Actually do the safe zan.
        }
    }
}
