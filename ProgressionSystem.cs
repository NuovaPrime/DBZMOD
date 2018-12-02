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

namespace DBZMOD
{
    public class ProgressionSystem
    {
        //vars
        private float m_kiExperience = 0;

        //methods

        
        //Getters
        public float GetKiExperience()
        {
            return m_kiExperience;
        }

        //setters
        public void SetKiExperience(float value)
        {
            m_kiExperience = value;

            if(m_kiExperience < 0)
            {
                m_kiExperience = 0;
            }
        }

        //adders
        public void AddKiExperience(float value)
        {
            SetKiExperience(GetKiExperience() + value);
        }

        public void Update(TriggersSet triggersSet, Player player)
        {
            ProcessKiExperienceGain(MyPlayer.ModPlayer(player));
        }

        private void ProcessKiExperienceGain(MyPlayer modplayer)
        {
            if (modplayer.IsTransformed)
            {
                AddKiExperience(0.2f);
            }

            if(modplayer.IsFlying)
            {
                AddKiExperience(0.1f);
            }
        }

        public bool ProcessCost(float cost)
        {
            if (GetKiExperience() >= cost)
            {
                AddKiExperience(-cost);
                return true;
            }
            else
            {
                Main.NewText("Can't Afford!");
                return false;
            }
        }


        public void UpgradeKiMax(int kiamount, Player player)
        {
            int EXPCOST = kiamount * 100;

            if (ProcessCost(EXPCOST))
            {
                MyPlayer.ModPlayer(player).KiMax2 += kiamount;
            }
        }

    }
}

