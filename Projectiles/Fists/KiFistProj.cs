using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;

namespace DBZMOD.Projectiles.Fists
{
    // helper class that handles tracking the metadata of a particular control, its state, how long it's held/been since press/release.
    private class ControlStateMetadata
    {
        // enum declaring the state of a given control at any point in time.
        public ControlStates state;

        // dual purpose int, either tracking the input delay before Pressed/PressedAndReleased reset.
        public int inputTimer;

        // if the button is being held for a protracted period of time (light attacks/heavy attacks?), this flag lets the methods know what to do with the input timer.
        public bool isHeld;
    }

    public class KiFistProj : KiProjectile
    {
        #region Variables
        private TriggersSet triggersSet;
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

        #region Enums
        // enum to handle dash direction states
        [Flags]
        public enum ControlInputTypes : short
        {
            None = 0,
            Up = 1,
            Down = 1 << 1,
            Left = 1 << 2,
            Right = 1 << 3,
            LightAttack = 1 << 4,
            HeavyAttack = 1 << 5,
            Block = LightAttack & HeavyAttack,
            UpLeft = Up & Left,
            UpRight = Up & Right,
            DownLeft = Down & Left,
            DownRight = Down & Right,
            Any = Up | Down | Left | Right | LightAttack | HeavyAttack
        };

        // the possible states a control can be in.
        public enum ControlStates
        {
            Released, // neutral, unpressed. Synonymous to "None"
            PressedOnce, // first detection of an input triggers this
            PressedAndReleased, // next phase, the key is released, but was pressed in the input window (for dashing)
            PressedTwice, // next phase, after key release, but still in the input window (for dashing), pressed a second time.
            PressedAndHeld // alternate phase, after pressedOnce, if the key is never released. Starts a held counter (?)
        }

        // the results to ultimately scan for "Should thing happen"
        public enum Controls
        {
            None = 0,
            LightPunch = 1,
            HeavyPunch = 2,
            DashUp = 3,
            DashDown = 4,
            DashLeft = 5,
            DashRight = 6,
            DashUpLeft = 7,
            DashUpRight = 8,
            DashDownLeft = 9,
            DashDownRight = 10,
            Block = 11
        }

        public Dictionary<Controls, ControlStateMetadata> _controlDictionary = null;
        public Dictionary<Controls, ControlStateMetadata> ControlDictionary
        {
            get
            {
                return _controlDictionary;
            }
            set
            {
                _controlDictionary = value;
            }
        }
                
        #endregion

        #region ControlStateHandler
        // scrape trigger states and return new state based on previous state.
        public ControlInputTypes GetCurrentInputState(TriggersSet inputStateTriggerSet)
        {
            ControlInputTypes currentFlagInputState = ControlInputTypes.None;
            if (inputStateTriggerSet.Up)
            {
                currentFlagInputState |= ControlInputTypes.Up;
            }

            if (inputStateTriggerSet.Down)
            {
                currentFlagInputState |= ControlInputTypes.Down;
            }

            if (inputStateTriggerSet.Left)
            {
                currentFlagInputState |= ControlInputTypes.Left;
            }

            if (inputStateTriggerSet.Right)
            {
                currentFlagInputState |= ControlInputTypes.Right;
            }

            if (inputStateTriggerSet.MouseLeft)
            {
                currentFlagInputState |= ControlInputTypes.LightAttack;
            }

            if (inputStateTriggerSet.MouseRight)
            {
                currentFlagInputState |= ControlInputTypes.HeavyAttack;
            }

            // if the current input state contains literally anything
            if (currentFlagInputState.HasFlag(ControlInputTypes.Any))
            {                
                // remove the "none" state.
                currentFlagInputState = currentFlagInputState & ~ControlInputTypes.None;
            }

            return currentFlagInputState;
        }


        #endregion

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.hide = true;
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = 1;
            projectile.timeLeft = 2;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.penetrate = -1;
            projectile.netUpdate = true;
        }
        public override void AI()
        {
            Player player = Main.LocalPlayer;
            Vector2 projvelocity = Vector2.Normalize(Main.MouseWorld - projectile.position) * ShootSpeed;

            projectile.Center = player.Center + new Vector2(-10, -20);
            #region Mouse Clicks
            if (triggersSet.MouseLeft) //left click, i think it's called per frame.
            {
                HoldTimer++;
                if (HoldTimer > 60)
                {
                    LightPunchHeld = true;
                    LightPunchPressed = false;
                    if(LightPunchHeld && MyPlayer.ModPlayer(player).CanUseFlurry)
                    {
                        FlurryTimer++;

                    }

                }
                if(!LightPunchPressed)
                {
                    LightPunchPressed = true;
                    ShootSpeed = 2;
                    Projectile.NewProjectile(projectile.position, projvelocity, BasicFistProjSelect(), BasicPunchDamage, 5);
                }

            }
            if (triggersSet.MouseRight) //Right click
            {
                if (!player.HasBuff(mod.BuffType("HeavyPunchCooldown")) && MyPlayer.ModPlayer(player).CanUseHeavyHit)
                {
                    Projectile.NewProjectile(projectile.position, projvelocity, mod.ProjectileType("KiFistProjHeavy"), HeavyPunchDamage, 50);
                }
            }
            if (triggersSet.MouseRight && triggersSet.MouseLeft)//both click, for blocking
            {
                BlockTimer++;
                if(BlockTimer < 30)
                {
                    MyPlayer.ModPlayer(player).BlockState = 1;
                    if(BlockTimer > 30 && BlockTimer < 120)
                    {
                        MyPlayer.ModPlayer(player).BlockState = 2;
                        if(BlockTimer > 120)
                        {
                            MyPlayer.ModPlayer(player).BlockState = 3;
                        }
                    }
                }
            }
            else
            {
                MyPlayer.ModPlayer(player).BlockState = 0;
            }
            #endregion

            #region Dash Checks
            // check initial left input for dash
            if (triggersSet.Left && !IsDashLeftJustPressed)
            {
                IsDashLeftJustPressed = true;
                DashTimer = 0;
            }

            // same for right
            if (triggersSet.Right && !IsDashRightJustPressed)
            {
                IsDashRightJustPressed = true;
                DashTimer = 0;
            }

            // same for up

            // same for down

            // check for control release, while the flags set above are true.
            if (!triggersSet.Left && IsDashLeftJustPressed)
            {
                IsDashLeftGapped = true;
            }

            // same for right
            if (!triggersSet.Right && IsDashRightJustPressed) {
                IsDashRightGapped = true;
            }

            if (triggersSet.Left && IsDashLeftJustPressed && IsDashLeftGapped)
            {
                IsDashLeftGapped = false;
                IsDashLeftJustPressed = false;
                MyPlayer.ModPlayer(player).IsDashing = true;
                //do dash left
            }

            if (triggersSet.Right && IsDashRightJustPressed && IsDashRightGapped)
            {
                IsDashRightGapped = false;
                IsDashRightJustPressed = false;
                MyPlayer.ModPlayer(player).IsDashing = true;
                //do dash right
            }

            if (IsDashLeftJustPressed)
            {
                DashTimer++;
                if (DashTimer > 15)
                {
                    IsDashLeftJustPressed = false;
                    DashTimer = 0;
                }
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
            if(EyeDowned)
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
        public int BasicFistProjSelect()
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
    }
}
