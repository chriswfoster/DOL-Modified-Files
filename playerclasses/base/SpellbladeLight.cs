﻿/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
using System;
using System.Collections.Generic;

namespace DOL.GS.PlayerClass
{
    /// <summary>
    /// Albion Base Fighter Class
    /// </summary>
    [CharacterClassAttribute((int)eCharacterClass.SpellbladeLight, "Spellblade - Light", "Spellblade - Light")]
    public class SpellbladeLight : CharacterClassBase
    {
        private static readonly string[] AutotrainableSkills = new[] { Specs.HandToHand, Specs.Axe, Specs.Sword, Specs.Thrust };
        public SpellbladeLight()
            : base()
        {
            m_profession = "PlayerClass.Profession.HouseofThor";
            m_specializationMultiplier = 40;
            m_primaryStat = eStat.STR;
            m_secondaryStat = eStat.INT;
            m_tertiaryStat = eStat.DEX;
            m_manaStat = eStat.INT;
            m_wsbase = 440;
            m_baseHP = 880;
        }


        public override bool CanUseLefthandedWeapon
        {
            get { return true; }
        }


        public override string GetTitle(GamePlayer player, int level)
        {
            return HasAdvancedFromBaseClass() ? base.GetTitle(player, level) : base.GetTitle(player, 0);
        }


        // public override eClassType ClassType
        // {
        //     get { return eClassType.ListCaster; }
        // }

        public override eClassType ClassType
        {
            get { return eClassType.Hybrid; }
        }


        public override IList<string> GetAutotrainableSkills()
        {
            return AutotrainableSkills;
        }

        public override GameTrainer.eChampionTrainerType ChampionTrainerType()
        {
            return GameTrainer.eChampionTrainerType.Fighter;
        }

        public override bool HasAdvancedFromBaseClass()
        {
            return true;
        }

    }
}
