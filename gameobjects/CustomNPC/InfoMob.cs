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
    class InfoMob : GameNPC
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
              
                player.Out.SendMessage(String.Format("Hello, welcome to the server! This info console should get you the help you need."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("From here, you have a handful of options. You can jump to [getting started], learn about [classes], [rules], or find a way to [contact] an admin for support."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
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


            if (text.ToLower() == "beginning")
            {
                player.Out.SendMessage(String.Format("Hello, welcome to Deity Node! This info console should get you the help you need."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("From here, you have a handful of options. You can jump to [getting started], learn about [speccing], [rules], or find a way to [contact] an admin for support."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }
            if (text.ToLower() == "rules")
            {
                player.Out.SendMessage(String.Format("The rules are pretty simple. Don't hack in pvp zones, and be nice to your fellow players. However, it is PvP, so don't hesitate to kill them at will. :)  There's no rules against grey ganking, griefing, etc. Have fun!"), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Return to [beginning] of info console."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }
            if (text.ToLower() == "getting started")
            {
                player.Out.SendMessage(String.Format("Well, first this is a PvP server, that's probably PvE heavy. There is NO INSTA 50. And there will ultimately be 7+ primary classes on this server."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Aurulite should drop from most mobs on this server. It's used to purchase a lot of content available in starter areas (primarily weapons and armor). There is probably some RNG item drops that you'll encounter. XP rate is set to x7 I think."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Return to [beginning] of info console."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }
            if (text.ToLower() == "contact")
            {
                player.Out.SendMessage(String.Format("Hey, never hesitate to reach out to me if you need help. If your character is bugged, you're stuck, or you have some ideas... "), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("You can email me at chriswfoster@gmail.com or join our discord at https://discord.gg/TCkYBVx."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Return to [beginning] of info console."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }
            if (text.ToLower() == "classes")
            {
                player.Out.SendMessage(String.Format("Currently, I'm still working on the classes. The (soon to be) available classes will be Summoner, Spellblade, Wizard, Stormlord, Guardians, Assassins, and Fighters."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("There will be 3+ tiers to each class. They'll start out with a basic spell line, then it'll enhance as they quest or find real world objects that give them the specs they seek."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Return to [beginning] of info console."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }
            
            return false;


        }
        

    }
}
