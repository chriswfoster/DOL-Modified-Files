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
              
                player.Out.SendMessage(String.Format("Hello, welcome to Deity Node! This info console should get you the help you need."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("From here, you have a handful of options. You can jump to [getting started], learn about [speccing], [rules], or find a way to [contact] an admin for support."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
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
                player.Out.SendMessage(String.Format("Well, first this is a PvP server, that's probably PvE heavy. There is NO INSTA 50. And there's only 1 real class on this server."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Aurulite should drop from every mob on this server. It's used to purchase a lot of content available in starter areas (primarily specs and armor). There is probably some RNG item drops that you'll encounter. XP rate is set to x7 I think."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Return to [beginning] of info console."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }
            if (text.ToLower() == "contact")
            {
                player.Out.SendMessage(String.Format("Hey guys, never hesitate to reach out to me if you need help. If your character is bugged, you're stuck, or you have some ideas... "), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("You can email me at chriswfoster@gmail.com or join our discord at https://discord.gg/TCkYBVx."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Return to [beginning] of info console."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }
            if (text.ToLower() == "speccing")
            {
                player.Out.SendMessage(String.Format("Currently, you have access to 10 specs. -3 if you spec in plate, -2 if you spec in chain. So you can have 7 specs total if plate is included in the mix."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Speccing is simple on Deity Node. Visit one of the gnomes that teach your desired spec, purchase the spec with aurulite, drop the spec on the gnome. There is an Ultimate Trainer that will level all your specs up to par with 1 right click interact. You may visit him as many times as you wish, currently 100% free. Spec removal is simple, just click the spec you wish to remove. No refunds :)"), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
                player.Out.SendMessage(String.Format("Return to [beginning] of info console."), eChatType.CT_Say, eChatLoc.CL_PopupWindow);
            }
            
            return false;


        }
        

    }
}
