using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.NPCs.Bosses
{
    //Thanks a bit to examplemod's flutterslime for helping with organization
	public class FriezaShip : ModNPC
	{
        private Vector2 hoverDistance = new Vector2(130, 180);
        private float hoverCooldown = 500;

        private int YHoverTimer = 0;
        private int XHoverTimer = 0;

        const int AIStageSlot = 0;
        const int AITimerSlot = 1;

        public float AIStage
        {
            get { return npc.ai[AIStageSlot]; }
            set { npc.ai[AIStageSlot] = value; }
        }

        public float AITimer
        {
            get { return npc.ai[AITimerSlot]; }
            set { npc.ai[AITimerSlot] = value; }
        }

        const int Stage_Hover = 0;
        const int Stage_Dash = 1;
        const int Stage_Barrage = 2;
        const int Stage_Homing = 3;
        const int Stage_Saiba = 4;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("A Frieza Force Ship");
			Main.npcFrameCount[npc.type] = 8;
		}

        public override void SetDefaults()
        {
            npc.width = 110;
            npc.height = 80;
            npc.damage = 26;
            npc.defense = 10;
            npc.lifeMax = 3600;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = Item.buyPrice(0, 3, 25, 80);
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        //To-Do: Add the rest of the stages to the AI. Make green saibaman code.
        //Make the speed of the ship's movements increase with less health, less time between stages as well?
        //Boss loot: Drops Undecided material that's used to create a guardian class armor set (frieza cyborg set). Alternates drops between a weapon and accessory, accessory is arm cannon mk2, weapon is a frieza force beam rifle. Expert item is the mechanical amplifier.
        //Spawn condition: Near the ocean you can find a frieza henchmen, if he runs away then you'll get an indicator saying the ship will be coming the next morning.


        public override void AI()
        {
            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            
            //Runaway if no players are alive
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }

            //Make sure the stages loop back around
            if(AIStage == 5)
            {
                AIStage = Stage_Hover;
            }

            //Speed between stages drastically increased with health lost
            if(npc.life < npc.lifeMax * 0.70f)
            {
                hoverCooldown = 400;
                if (npc.life < npc.lifeMax * 0.40f)
                {
                    hoverCooldown = 300;
                }
                if (npc.life < npc.lifeMax * 0.15f)
                {
                    hoverCooldown = 100;
                }
            }

            
            //General movement (stage 0)
            if (AIStage == Stage_Hover)
            {
                //Y Hovering
                if (Vector2.Distance(new Vector2(0, player.position.Y), new Vector2(0, npc.position.Y)) != hoverDistance.Y)
                {

                    if (Vector2.Distance(new Vector2(0, player.position.Y), new Vector2(0, npc.position.Y)) > hoverDistance.Y)
                    {
                        //float hoverSpeedY = (2f + Main.rand.NextFloat(3, 8));
                        //Add a little bit of delay before moving, this lets melee players possibly get a hit in
                        YHoverTimer++;
                        if(YHoverTimer > 15)
                        {
                            npc.velocity.Y = 2f;
                        }
                    }
                    else if (Vector2.Distance(new Vector2(0, player.position.Y), new Vector2(0, npc.position.Y)) < hoverDistance.Y)
                    {
                        //float hoverSpeedY = (-2f + Main.rand.NextFloat(-3, -8));
                        YHoverTimer++;
                        if (YHoverTimer > 15)
                        {
                            npc.velocity.Y = -2f;
                        }
                    }
                }
                else
                {
                    npc.velocity.Y = 0;
                    YHoverTimer = 0;
                }
                //X Hovering, To-Do: Make the ship not just center itself on the player, have some left and right alternating movement?
                if (Vector2.Distance(new Vector2(0, player.position.X), new Vector2(0, npc.position.X)) != hoverDistance.X)
                {
                    //float hoverSpeedY = (-2f + Main.rand.NextFloat(-3, -8));
                    XHoverTimer++;
                    if (XHoverTimer > 30)
                    {
                        npc.velocity.X = (2.5f * npc.direction);
                    }
                }
                else
                {
                    npc.velocity.X = 0;
                    XHoverTimer = 0;
                }
                
                //Next Stage
                AITimer++;
                if (AITimer > hoverCooldown)
                {
                    StageAdvance();
                    AITimer = 0;
                }

            }




            //To-Do: Dash attack (stage 1) - Rams into the player with increased contact damage
            bool locationSelected = false;
            if(AIStage == Stage_Dash)
            {
                if (!locationSelected)
                {

                    if (Vector2.Distance(new Vector2(0, player.position.Y), new Vector2(0, npc.position.Y)) > hoverDistance.Y)
                    {
                        //float hoverSpeedY = (2f + Main.rand.NextFloat(3, 8));
                        //Add a little bit of delay before moving, this lets melee players possibly get a hit in
                        YHoverTimer++;
                        if (YHoverTimer > 15)
                        {
                            npc.velocity.Y = 5f;
                        }
                    }
                    else if (Vector2.Distance(new Vector2(0, player.position.Y), new Vector2(0, npc.position.Y)) < hoverDistance.Y)
                    {
                        //float hoverSpeedY = (-2f + Main.rand.NextFloat(-3, -8));
                        YHoverTimer++;
                        if (YHoverTimer > 15)
                        {
                            npc.velocity.Y = -5f;
                        }
                    }
                }
                else
                {
                    npc.velocity.Y = 0;
                    YHoverTimer = 0;
                }
                if (!locationSelected)
                {
                    //X Hovering, To-Do: Make the ship not just center itself on the player, have some left and right alternating movement?
                    if (Vector2.Distance(new Vector2(0, player.position.X), new Vector2(0, npc.position.X)) != hoverDistance.X)
                    {
                        //float hoverSpeedY = (-2f + Main.rand.NextFloat(-3, -8));
                        XHoverTimer++;
                        if (XHoverTimer > 30)
                        {
                            npc.velocity.X = (8f * npc.direction);
                        }
                    }
                    else
                    {
                        npc.velocity.X = 0;
                        XHoverTimer = 0;
                    }
                }

                if(Vector2.Distance(new Vector2(0, player.position.Y), new Vector2(0, npc.position.Y)) == hoverDistance.Y && Vector2.Distance(new Vector2(0, player.position.X), new Vector2(0, npc.position.X)) == hoverDistance.X)
                {
                    locationSelected = true;
                    AITimer++;
                    if(AITimer > 60)
                    {
                        npc.noTileCollide = false;
                        npc.velocity.Y = 5f;
                    }
                }





                StageAdvance();
                AITimer = 0;
            }

            //Vertical projectile barrage (stage 2) - Fires a barrage of projectiles upwards that randomly spread out and fall downwards which explode on ground contact

            if (AIStage == Stage_Barrage)
            {
                npc.velocity.Y = 0;
                npc.velocity.X = 0;

                if (AITimer == 0)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);

                    if (npc.life < npc.lifeMax * 0.70f) //Fire 4 extra projectiles if below 70% health
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    }
                }

                AITimer++;
                if (AITimer > 60)
                {
                    if (npc.life < npc.lifeMax * 0.60f)
                    {
                        StageAdvance();
                    }
                    else
                    {
                        AIStage = Stage_Hover;
                    }
                    AITimer = 0;
                }
            }

            //Vertical projectile barrage + homing (stage 3) - Fires 2 projectiles in opposite arcs diagonally from the ship, after 3 seconds they stop, after 1 second both will fly towards the player.
            // These projectiles are stronger than the barrage ones, but also slower.

            if (AIStage == Stage_Homing)
            {
                npc.velocity.Y = 0;
                npc.velocity.X = 0;

                if (AITimer == 0)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2.5f, -1f, mod.ProjectileType("FFHomingBlast"), npc.damage / 3, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -2.5f, -1f, mod.ProjectileType("FFHomingBlast"), npc.damage / 3, 3f, Main.myPlayer);

                    if (npc.life < npc.lifeMax * 0.50f) //Fire an extra projectile upwards if below 50% health
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -1f, mod.ProjectileType("FFHomingBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    }
                }
                AITimer++;
                if (AITimer > 60)
                {
                    if (npc.life < npc.lifeMax * 0.40f)
                    {
                        StageAdvance();
                    }
                    else
                    {
                        AIStage = Stage_Hover;
                    }
                    AITimer = 0;
                }
            }

            //To-Do: Summon saibamen (stage 4) - Summons a green saiba from the ship, green dust when this happens to make it look smoother (Perhaps make this something after 40% HP)
            if (Main.netMode != 1 && AIStage == Stage_Saiba)
            {
                if (AITimer == 0)
                {
                    int saiba = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("SaibaGreen"));
                    Main.npc[saiba].netUpdate = true;
                    npc.netUpdate = true;
                }
                AITimer++;
                if (AITimer > 60)
                {
                    StageAdvance();
                    AITimer = 0;
                }
            }

            //Main.NewText(AIStage);
        }

        private void StageAdvance()
        {
            //if Below 30% health, randomly pick a stage
            if(npc.life < npc.lifeMax * 0.30f)
            {
                int NextStage = Main.rand.Next(0, 4);

                AIStage = NextStage;

            }
            //otherwise, go to next stage
            else
            {
                AIStage++;
            }

        }

        //Animations
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if(AIStage == Stage_Barrage || AIStage == Stage_Homing)
            {
                npc.frameCounter += 3;
            }
            else
            {
                npc.frameCounter++;
            }
            if (npc.frameCounter > 4)
            {
                frame++;
                npc.frameCounter = 0;
            }
            if(frame > 7) //Make it 7 because 0 is counted as a frame, making it 8 frames
            {
                frame = 0;
            }

            npc.frame.Y = frameHeight * frame;
        }

        /*public override void NPCLoot()
        {
            if (Main.rand.Next(20) == 0)
            {
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MajinNucleus"));
                }
            }
        }*/
    }
}
