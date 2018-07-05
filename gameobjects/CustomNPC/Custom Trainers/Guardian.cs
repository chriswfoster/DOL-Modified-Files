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

namespace DOL.GS
{
    /// <summary>
    /// Simple Teleporter.
    /// This teleporter uses the npc guild name to determine available teleport locations in the Teleport table
    /// PackageID is used for the text displayed to the player
    /// 
    /// Example:
    /// Add this npc to the world and set guild name to 'My Teleports'
    /// Go to a location you want to teleport too and use the command /teleport 'location name' 'My Teleports'
    /// 
    /// You can whisper refresh to this teleporter to reload the teleport locations
    /// </summary>
    /// <author>Tolakram; from SI teleporter created by Aredhel</author>
    public class GuardianTrainer : GameNPC
    {


        protected int targetClassID = 52;                              
        protected string targetClassName = "guardian";                 
        protected string targetClassDescription = "The Guardian class has 1 job, and it's to protects other players. The Guardian is very hard to kill. However it is very unlikely that the guardian should be able to kill anyone himself. The Guardian comes with shield spec (no stuns), resists, and more to keep the team alive.";
        protected int targetClassRace1 = 6;
        protected int targetClassRace2 = 16;


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

        /// <summary>
        /// Player right-clicked the teleporter.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        /// 



        public virtual bool CanTrain(GamePlayer player)
        {
            return player.CharacterClass.ID == targetClassID;
        }



        public override bool Interact(GamePlayer player)
        {

            if (!base.Interact(player))
            {
                return false;
            }
            else
            {
                // Turn to face player
                TurnTo(player, 10000);
                SayTo(player, $"Hey there, do you want to learn about the [{targetClassName}] class?");                            //change value here

            }
            // Unknown class must be used for multitrainer
            if (CanTrain(player))
            {
                player.Out.SendTrainerWindow();

                player.GainExperience(GameLiving.eXPSource.Other, 0);//levelup

                if (player.FreeLevelState == 2)
                {
                    player.LastFreeLevel = player.Level;
                    //long xp = GameServer.ServerRules.GetExperienceForLevel(player.PlayerCharacter.LastFreeLevel + 3) - GameServer.ServerRules.GetExperienceForLevel(player.PlayerCharacter.LastFreeLevel + 2);
                    long xp = player.GetExperienceNeededForLevel(player.LastFreeLevel + 1) - player.GetExperienceNeededForLevel(player.LastFreeLevel);
                    //player.PlayerCharacter.LastFreeLevel = player.Level;
                    player.GainExperience(GameLiving.eXPSource.Other, xp);
                    player.LastFreeLeveled = DateTime.Now;
                    player.Out.SendPlayerFreeLevelUpdate();
                }
            }
            return false;
        }



        /// <summary>
        /// Use the NPC Guild Name to find all the valid destinations for this teleporter
        /// </summary>
        public override bool WhisperReceive(GameLiving source, string text)
        {


            if (!base.WhisperReceive(source, text) || !(source is GamePlayer))
                return false;
            GamePlayer player = source as GamePlayer;


            if (text.ToLower() == $"{targetClassName}".ToLower())
            {
                player.Out.SendMessage(String.Format($"{targetClassDescription}"), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format($"You must be a Half-Ogre or Troll to be a true {targetClassName}"), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format($"Would you like to [become a {targetClassName}] today?"), eChatType.CT_Say, eChatLoc.CL_PopupWindow);

            }
            if (text.ToLower() == $"become a {targetClassName}".ToLower() && (player.Race == targetClassRace1 || player.Race == targetClassRace2))
            {
                player.Out.SendCustomDialog($"Are you sure you want to become a {targetClassName}?", new CustomDialogResponse(WarriorTrainerPrompt));

            }
            if (text.ToLower() == $"become a {targetClassName}".ToLower() && (player.Race != targetClassRace1 || player.Race != targetClassRace2))
            {
                player.Out.SendMessage($"Hah, you can't be a {targetClassName}. You must select the correct race(s) for this class!", DOL.GS.PacketHandler.eChatType.CT_Staff, DOL.GS.PacketHandler.eChatLoc.CL_SystemWindow);
                player.Out.SendMessage(String.Format($"Hah, you can't be a {targetClassName}. You must select the correct race(s) for this class!"), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }

            return false;


        }
        protected virtual void WarriorTrainerPrompt(GamePlayer player, byte response)
        {
            if (response != 0x01)
            {

                return;
            }
            if (player.Class != targetClassID)
            {
                SetClass(player, targetClassID);
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindRegion = 1;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindXpos = 561654;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindYpos = 510725;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindZpos = 2327;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindHeading = 1009;
                player.SaveIntoDatabase();
                player.Out.SendMessage($"Great choice, {targetClassName}! Off you go then!", DOL.GS.PacketHandler.eChatType.CT_Staff, DOL.GS.PacketHandler.eChatLoc.CL_SystemWindow);
            }
            if (player.Class == targetClassID)
            {
                player.Out.SendMessage("You're already a " + player.Class + ".", DOL.GS.PacketHandler.eChatType.CT_Staff, DOL.GS.PacketHandler.eChatLoc.CL_SystemWindow);
                player.Out.SendMessage(String.Format("You're already a " + player.Class + "."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }
        }


        public void SetClass(GamePlayer target, int classID)
        {
            //remove all their tricks and abilities!
            target.RemoveAllSpecs();
            target.RemoveAllSpellLines();
            target.RemoveAllStyles();

            //reset before, and after changing the class.
            target.Reset();
            target.SetCharacterClass(classID);
            target.Reset();

            //this is just for additional updates
            //that add all the new class changes.
            target.OnLevelUp(0);

            target.Out.SendUpdatePlayer();
            target.Out.SendUpdatePlayerSkills();
            target.Out.SendUpdatePoints();
        }

    }
}
