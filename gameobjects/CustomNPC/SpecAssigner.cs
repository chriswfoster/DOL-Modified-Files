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
    public class GameSpecAssigner : GameAuruliteMerchant
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
                string tmpStr = player.Client.Account.Characters[player.Client.ActiveCharIndex].SerializedSpecs.Replace(@";", ",").Replace(@"|", ",");
                string[] values = { };
                foreach (string spec in tmpStr.SplitCSV())
                {
                values = spec.Split(',');
                }
                int hasPlate = 0;
                int hasChain = 0;
                List<string> specList = new List<string>();
                for (var i = 0; i < values.Length; i++)
                {
                    if (i % 2 == 0 && values[i] == "Plate")
                    {
                        hasPlate = 6;
                        specList.Add(values[i]);
                    }
                    else if (i % 2 == 0 && values[i] == "Chain")
                    {
                        hasChain = 4;
                        specList.Add(values[i]);
                    }
                    else if (i % 2 == 0 && values[i] != "Chain" && values[i] != "Plate")
                    {
                        specList.Add(values[i]);
                    }
                }

                if (values.Length < (20 - hasPlate - hasChain))
                {
                    if (values.Length < 15 && item.Item_Type == 444 && item.Name == "Plate")
                    {
                        player.AddAbility(SkillBase.GetAbility("AlbArmor", 5));
                        player.AddAbility(SkillBase.GetAbility("HibArmor", 4));
                        player.AddAbility(SkillBase.GetAbility("MidArmor", 4));
                        player.AddSpecialization(SkillBase.GetSpecialization(item.Name));
                        player.Out.SendUpdatePlayer();
                        player.Out.SendUpdatePoints();
                        player.Out.SendUpdatePlayerSkills();
                        player.SaveIntoDatabase();
                        player.UpdatePlayerStatus();
                        player.Inventory.RemoveCountFromStack(item, 1);
                    }
                    else if (values.Length > 14 && item.Item_Type == 444 && item.Name == "Plate")
                    {
                        SayTo(player, "Plate occupies 3 specs, remove some specs first!");
                    }
                    else if (values.Length < 17 && item.Item_Type == 444 && item.Name == "Chain")
                    {
                        player.AddAbility(SkillBase.GetAbility("AlbArmor", 4));
                        player.AddAbility(SkillBase.GetAbility("HibArmor", 4));
                        player.AddAbility(SkillBase.GetAbility("MidArmor", 4));
                        player.AddSpecialization(SkillBase.GetSpecialization(item.Name));
                        player.Out.SendUpdatePlayer();
                        player.Out.SendUpdatePoints();
                        player.Out.SendUpdatePlayerSkills();
                        player.SaveIntoDatabase();
                        player.UpdatePlayerStatus();
                        player.Inventory.RemoveCountFromStack(item, 1);
                    }
                    else if (values.Length > 16 && item.Item_Type == 444 && item.Name == "Chain")
                    {
                        SayTo(player, "Chain and Studded occupy 2 specs, remove some specs first!");
                    }
                    else if (values.Length < 17 && item.Item_Type == 444 && item.Name == "Studded / Reinforced")
                    {
                        player.AddAbility(SkillBase.GetAbility("AlbArmor", 3));
                        player.AddAbility(SkillBase.GetAbility("HibArmor", 3));
                        player.AddAbility(SkillBase.GetAbility("MidArmor", 3));
                        player.AddSpecialization(SkillBase.GetSpecialization(item.Name));
                        player.Out.SendUpdatePlayer();
                        player.Out.SendUpdatePoints();
                        player.Out.SendUpdatePlayerSkills();
                        player.SaveIntoDatabase();
                        player.UpdatePlayerStatus();
                        player.Inventory.RemoveCountFromStack(item, 1);
                    }
                    else if (values.Length > 16 && item.Item_Type == 444 && item.Name == "Studded")
                    {
                        SayTo(player, "Chain and Studded occupy 2 specs, remove some specs first!");
                    }
                    else if (item.Item_Type == 444 && item.Name == "Leather")
                    {
                        player.AddAbility(SkillBase.GetAbility("AlbArmor", 2));
                        player.AddAbility(SkillBase.GetAbility("HibArmor", 2));
                        player.AddAbility(SkillBase.GetAbility("MidArmor", 2));
                        player.AddSpecialization(SkillBase.GetSpecialization(item.Name));
                        player.Out.SendUpdatePlayer();
                        player.Out.SendUpdatePoints();
                        player.Out.SendUpdatePlayerSkills();
                        player.SaveIntoDatabase();
                        player.UpdatePlayerStatus();
                        player.Inventory.RemoveCountFromStack(item, 1);
                    }
                    else if (item.Item_Type == 666 && isItemInMerchantList(item))
                    {
                        player.AddAbility(SkillBase.GetAbility(item.Name));
                        player.AddSpecialization(SkillBase.GetSpecialization(item.Description));
                        player.Out.SendUpdatePlayer();
                        player.Out.SendUpdatePoints();
                        player.Out.SendUpdatePlayerSkills();
                        player.SaveIntoDatabase();
                        player.UpdatePlayerStatus();
                        player.Inventory.RemoveCountFromStack(item, 1);

                    }
                    else if (item.Item_Type == 888 && isItemInMerchantList(item))
                    {
                        player.AddSpecialization(SkillBase.GetSpecialization(item.Name));
                        player.Out.SendUpdatePlayer();
                        player.Out.SendUpdatePoints();
                        player.Out.SendUpdatePlayerSkills();
                        player.SaveIntoDatabase();
                        player.UpdatePlayerStatus();
                        player.Inventory.RemoveCountFromStack(item, 1);

                    }
                    return false;

                }else if (values.Length > (19 - hasPlate - hasChain))
                {
                    SayTo(player, "You can only have 10 specs in your specialization list! Go remove some to add this one. Choose wisely!");
                }







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
      
                SayTo(player, "Hello traveler, purchase an ability scroll, and hand it to me. I'll try to teach you the ability. I'm learning new things every day - myself. MAX SPEC LIMIT: 10");
            
              
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
