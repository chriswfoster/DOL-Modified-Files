

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
/* Original from Etaew
 * Updates: Timx, Daeli
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DOL.Database;
using DOL.GS.Commands;
using DOL.GS.PacketHandler;

namespace DOL.GS
{
    //  [NPCGuildScript("Rogue Trainer", eRealm.Albion)]        // this attribute instructs DOL to use this script for all "Rogue Trainer" NPC's in Albion (multiple guilds are possible for one script)
    public class AllClassTrainer : GameNPC
    {
        private const string CantTrainSpec = "You can't train in this specialization again this level!";
        private const string NotEnoughPointsLeft = "You don't have that many specialization points left for this level.";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //     private List<Classes> m_availableclasses = new List<Classes>();

        /// <summary>
        /// Display the teleport indicator around this teleporters feet
        /// </summary>
        public override bool ShowTeleporterIndicator
        {
            get
            {
                return true;
            }
        }

      //  public override bool WhisperReceive(GameLiving source, string text)
       //  string tmpStr = player.Client.Account.Characters[player.Client.ActiveCharIndex].SerializedSpecs;

       // {

      //      if (!base.WhisperReceive(source, text) || !(source is GamePlayer))
       //         return false;
       //     GamePlayer player = source as GamePlayer;


       // }

      //  return false;

        public override bool Interact(GamePlayer player)
        {
            string tmpStr = player.Client.Account.Characters[player.Client.ActiveCharIndex].SerializedSpecs.Replace(@";", ",").Replace(@"|", ",");

            string[] values = {};
            foreach (string spec in tmpStr.SplitCSV())
            {
                values = spec.Split(',');
            }  
                if (values.Length >= 2)
                {
                    for (var i =0; i < values.Length; i++)
                    {
                        Specialization tempSpec = SkillBase.GetSpecialization(values[i]);
                       
                        
                     
                        i++;

                        if (tempSpec != null)
                        {
                            if (tempSpec.AllowSave)
                            {
                                int level;
                                level = 50;
                                if (player.HasSpecialization(tempSpec.KeyName))
                                {
                                    player.GetSpecializationByName(tempSpec.KeyName).Level = level;
                                   
                            }
                                else
                                {
                                    tempSpec.Level = level;
                                    player.AddSpecialization(tempSpec);
                            }

                            player.Out.SendUpdatePoints();
                            player.Out.SendUpdatePlayerSkills();
                            player.SaveIntoDatabase();
                            player.UpdatePlayerStatus();
                            SayTo(player, "Updated spec levels.");
                        }
                           
                        }
                       
                    }
                } return false;
           
        }
    }
}