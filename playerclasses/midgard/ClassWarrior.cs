/*
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
/* using System;
using System.Collections.Generic;

namespace DOL.GS.PlayerClass
{
	/// <summary>
	/// Midgard Warrior Class
	/// </summary>
	[CharacterClassAttribute((int)eCharacterClass.Warrior, "Warrior", "Viking")]
	public class ClassWarrior : ClassViking
	{
		private static readonly string[] AutotrainableSkills = new[] { Specs.Axe, Specs.Hammer, Specs.Sword };

		public ClassWarrior()
			: base()
		{
			m_profession = "PlayerClass.Profession.HouseofTyr";
			m_specializationMultiplier = 20;
			m_primaryStat = eStat.STR;
			m_secondaryStat = eStat.CON;
			m_tertiaryStat = eStat.DEX;
			m_wsbase = 460;
		}

		public override IList<string> GetAutotrainableSkills()
		{
			return AutotrainableSkills;
		}

		public override bool HasAdvancedFromBaseClass()
		{
			return true;
		}
	}
}
*/

/*
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
    [CharacterClassAttribute((int)eCharacterClass.Warrior, "Warrior", "Warrior")]
    public class Warrior : CharacterClassBase
    {
        private static readonly string[] AutotrainableSkills = new[] { Specs.Axe, Specs.Sword, Specs.Polearms, Specs.Hammer };
        public Warrior()
            : base()
        {
            m_profession = "PlayerClass.Profession.HouseofThor";
            m_specializationMultiplier = 100;
            m_primaryStat = eStat.STR;
            m_secondaryStat = eStat.CON;
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
            get { return eClassType.PureTank; }
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
