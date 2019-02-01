using DBZMOD.Extensions;
using DBZMOD.Players;
using Terraria;
using DBZMOD.Util;
using PlayerExtensions = DBZMOD.Extensions.PlayerExtensions;

namespace DBZMOD
{
    public class ProgressionSystem
    {
        //vars
        private float _mKiExperience = 0;

        //methods

        
        //Getters
        public float GetKiExperience()
        {
            return _mKiExperience;
        }

        //setters
        public void SetKiExperience(float value)
        {
            _mKiExperience = value;

            if(_mKiExperience < 0)
            {
                _mKiExperience = 0;
            }
        }

        //adders
        public void AddKiExperience(float value)
        {
            SetKiExperience(GetKiExperience() + value);
        }

        public void Update(Player player)
        {
            ProcessKiExperienceGain(MyPlayer.ModPlayer(player));
        }

        private void ProcessKiExperienceGain(MyPlayer modPlayer)
        {
            if (modPlayer.player.IsPlayerTransformed())
            {
                AddKiExperience(0.2f);
            }

            if(modPlayer.isFlying)
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
            int expcost = kiamount * 100;

            if (ProcessCost(expcost))
            {
                MyPlayer.ModPlayer(player).kiMax3 += kiamount;
            }
        }

    }
}

