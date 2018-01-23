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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DOL.GS;
using DOL.Database;
using System.Collections;
using DOL.GS.Spells;
using log4net;
using System.Reflection;
using DOL.GS.PacketHandler;
using DOL.Events;
using DOL.Language;
using DOL.GS.Movement;


namespace DOL.GS
{
    
        /// <summary>
        /// Stable master that sells and takes horse route tickes
        /// </summary>
    [NPCGuildScript("Spec Assigner", eRealm.None)]
    public class GameSpecAssigner : GameMerchant
    {
            /// <summary>
            /// Constructs a new stable master
            /// </summary>
        public GameSpecAssigner()
        {
        }

         private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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

        public override bool ReceiveItem(GameLiving source, InventoryItem item)
        {
            if (source == null || item == null) return false;

            if (source is GamePlayer)
            {
                GamePlayer player = (GamePlayer)source;

                if (item.Item_Type == 888 && isItemInMerchantList(item))
                {

                    var spectogive = item.Name;
                   if (spectogive.ToLower() == "darkness")
                    {
                        player.AddSpecialization(SkillBase.GetSpecialization(Specs.Darkness));
                        player.AddSpellLine(SkillBase.GetSpellLine("Darkness"));
                        player.AddSpecialization(SkillBase.GetSpecialization(Specs.Augmentation));
                        player.AddSpellLine(SkillBase.GetSpellLine("Augmentation Spec"));
                        player.AddSpellLine(SkillBase.GetSpellLine("AugmentationSpec"));
                        player.Out.SendUpdatePlayer();
                        player.Out.SendUpdatePoints();
                        player.Out.SendUpdatePlayerSkills();
                        player.SaveIntoDatabase();
                        player.UpdatePlayerStatus();
                    }
                }
                return false;
            }
            return false;
        }








        /// <summary>
        /// Player right-clicked the teleporter.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool Interact(GamePlayer player)
        {
            if (!base.Interact(player))
            {
                return false;
            }
            else
            {
                SayTo(player, "Hello traveler, purchase an ability scroll, and hand it to me. I'll try to teach you the ability. I'm learning new things every day - myself.");
                
            }
            return false;
        }



        /// <summary>
        /// Use the NPC Guild Name to find all the valid destinations for this teleporter
        /// </summary>


        private bool isItemInMerchantList(InventoryItem item)
        {
            if (m_tradeItems != null)
            {
                foreach (DictionaryEntry de in m_tradeItems.GetAllItems())
                {
                    ItemTemplate compareItem = de.Value as ItemTemplate;
                    if (compareItem != null)
                    {
                        if (compareItem.Id_nb == item.Id_nb)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }












        public override bool WhisperReceive(GameLiving source, string text)

        {

            if (!base.WhisperReceive(source, text) || !(source is GamePlayer))
                return false;
            GamePlayer player = source as GamePlayer;


            if (text.ToLower() == "darkness")
            {
                player.AddSpecialization(SkillBase.GetSpecialization(Specs.Darkness));
                player.AddSpellLine(SkillBase.GetSpellLine("Darkness"));
                player.AddSpecialization(SkillBase.GetSpecialization(Specs.Augmentation));
                player.AddSpellLine(SkillBase.GetSpellLine("Augmentation Spec"));
                player.AddSpellLine(SkillBase.GetSpellLine("AugmentationSpec"));
                player.Out.SendUpdatePlayer();
                player.Out.SendUpdatePoints();
                player.Out.SendUpdatePlayerSkills();
                player.SaveIntoDatabase();
                player.UpdatePlayerStatus();
            }

            if (text.ToLower() == "stormcalling")
            {
                player.Out.SendMessage("Should have other spell line now!", DOL.GS.PacketHandler.eChatType.CT_Staff, DOL.GS.PacketHandler.eChatLoc.CL_SystemWindow);
                player.AddSpecialization(SkillBase.GetSpecialization(Specs.Stormcalling));
                player.AddSpellLine(SkillBase.GetSpellLine("Stormcalling"));
                player.Out.SendUpdatePlayer();
                player.Out.SendUpdatePoints();
                player.Out.SendUpdatePlayerSkills();
                player.SaveIntoDatabase();
                player.UpdatePlayerStatus();
            }



                return false;


        }
     

    }
}
