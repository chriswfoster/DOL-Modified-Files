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
    public class ClassPicker : GameNPC
    {

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
        public override bool Interact(GamePlayer player)
        {
            if (!base.Interact(player))
            {
                return false;
            }
            else
            {
                SayTo(player, "Prisoner! Quickly now, before the other guards arrive! I can release you into this world, but you have to make something of yourself.");
                SayTo(player, "Are you ready to become a [fighter]? This is your only way out.");
                
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
   

            if (text.ToLower() == "fighter")
            {
                player.Out.SendMessage("Great, a fighter, whatever. Off you go!", DOL.GS.PacketHandler.eChatType.CT_Staff, DOL.GS.PacketHandler.eChatLoc.CL_SystemWindow);
                SetClass(player, 14);
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindRegion = 1;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindXpos = 561654;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindYpos = 510725;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindZpos = 2327;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindHeading = 1009;
                player.SaveIntoDatabase();
                player.MoveTo(
                    (ushort)player.BindRegion,
                    player.BindXpos,
                    player.BindYpos,
                    (ushort)player.BindZpos,
                    (ushort)player.BindHeading
                );
            }
            if (text.ToLower() == "mage")
            {
                player.Out.SendMessage("Fine, a mage, whatever. Time to go!", DOL.GS.PacketHandler.eChatType.CT_Staff, DOL.GS.PacketHandler.eChatLoc.CL_SystemWindow);
                SetClass(player, 18);
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindRegion = 1;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindXpos = 561654;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindYpos = 510725;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindZpos = 2327;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindHeading = 1009;
                player.SaveIntoDatabase();
                player.MoveTo(
                    (ushort)player.BindRegion,
                    player.BindXpos,
                    player.BindYpos,
                    (ushort)player.BindZpos,
                    (ushort)player.BindHeading
                );
            }
            if (text.ToLower() == "rogue")
            {
                player.Out.SendMessage("How surprising, another rogue. See ya!", DOL.GS.PacketHandler.eChatType.CT_Staff, DOL.GS.PacketHandler.eChatLoc.CL_SystemWindow);
                SetClass(player, 17);
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindRegion = 1;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindXpos = 561654;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindYpos = 510725;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindZpos = 2327;
                player.Client.Account.Characters[player.Client.ActiveCharIndex].BindHeading = 1009;
                player.SaveIntoDatabase();
                player.MoveTo(
                    (ushort)player.BindRegion,
                    player.BindXpos,
                    player.BindYpos,
                    (ushort)player.BindZpos,
                    (ushort)player.BindHeading
                );
            }
            return false;


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
