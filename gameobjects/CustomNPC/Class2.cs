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
                SayTo(player, "Hello prisoner, do you wish to do something with your life? Maybe become a [fighter], [mage], or some sort of [rogue]?");
            }
            return false;
        }

        //LoadDestinations();

        //if (string.IsNullOrEmpty(PackageID) == false)
        //	{
        //	SayTo(player, PackageID);
        //	}
        //	else
        //	{
        //		SayTo(player, "Choose a destination ...");
        //	}
        //
        //	int numDestinations = 0;
        //foreach (Teleport destination in m_destinations)
        //	{
        //		player.Out.SendMessage(String.Format("[{0}]", destination.TeleportID), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
        //		numDestinations++;
        //	}

        //	if (numDestinations == 0 && player.Client.Account.PrivLevel > (int)ePrivLevel.Player)
        //	{
        //		SayTo(player, "I have not been set up properly, I need teleport locations.  Do /teleport add \"Destination Name\" \"" + GuildName + "\"");
        //	}

        //	return true;
        //}



        /// <summary>
        /// Use the NPC Guild Name to find all the valid destinations for this teleporter
        /// </summary>
        public override bool WhisperReceive(GameLiving source, string text)

        {
            if (!base.WhisperReceive(source, text) || !(source is GamePlayer))
                return false;
            GamePlayer player = source as GamePlayer;

            if (text.ToLower() == "fighter" || text.ToLower() == "mage" || text.ToLower() == "rogue")
            {
                player.Out.SendMessage("Yay fighter", DOL.GS.PacketHandler.eChatType.CT_Staff, DOL.GS.PacketHandler.eChatLoc.CL_SystemWindow);
            }
            return false;


        }
         
    }
}
