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
        private int m_kiExperience = 0;

        //methods

        
        //Getters
        public int GetKiExperience()
        {
            return m_kiExperience;
        }

        //setters
        public void SetKiExperience(int value)
        {
            m_kiExperience = value;

            if(m_kiExperience < 0)
            {
                m_kiExperience = 0;
            }
        }

        //adders
        public void AddKiExperience(int value)
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
                AddKiExperience(2);
            }

            if(modplayer.IsFlying)
            {
                AddKiExperience(1);
            }
        }

        bool ProcessCost(int cost)
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


        void UpgradeKiMax(int kiamount, Player player)
        {
            int EXPCOST = kiamount * 100;

            if (ProcessCost(EXPCOST))
            {
                MyPlayer.ModPlayer(player).KiMax += kiamount;
            }
        }

    }
}

