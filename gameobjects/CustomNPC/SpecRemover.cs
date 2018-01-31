using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOL.GS;

using System.Collections;
using DOL.GS.Spells;
using DOL.Database;
using DOL.GS.Commands;
using DOL.GS.PacketHandler;
using log4net;
using System.Reflection;

namespace DOL.GS
{
    
    public class SpecRemover : GameNPC
    {
        
        public override bool Interact(GamePlayer player)
        {
            string tmpStr = player.Client.Account.Characters[player.Client.ActiveCharIndex].SerializedSpecs.Replace(@";", ",").Replace(@"|", ",");

            string[] values = { };
            foreach (string spec in tmpStr.SplitCSV())
            {
                values = spec.Split(',');
            }
            if (values.Length >= 2)
            {
               
                List<string> specList = new List<string>();
                    for (var i = 0; i < values.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        specList.Add(values[i]);
                    }
                }
                SayTo(player, "Which spec would you like to remove? [" + string.Join("], [", specList) + "]");
            } return false;
        }
        string SELECTED_SPEC;
        public override bool WhisperReceive(GameLiving source, string text)

        {

            if (!base.WhisperReceive(source, text) || !(source is GamePlayer))
                return false;
            GamePlayer player = source as GamePlayer;


            if (text.Length > 1)
            {

                Specialization tempSpec = SkillBase.GetSpecialization(text);
            
                player.Out.SendCustomDialog("CAUTION: All spec removals final. Do you want to remove " + tempSpec.KeyName + "?", new CustomDialogResponse(PlayerResponse));
                SELECTED_SPEC = tempSpec.KeyName;
                
            }
            return false;

            
        }
        private void PlayerResponse(GamePlayer player, byte response)
        {

            if (response != 0x01) return; //declined

          
           
            SayTo(player, "Respeccing " + SELECTED_SPEC+ " for you now...");
            player.RemoveSpecialization(SELECTED_SPEC);
            SELECTED_SPEC = "";
            player.Out.SendUpdatePlayer();
            player.Out.SendUpdatePoints();
            player.Out.SendUpdatePlayerSkills();
            player.SaveIntoDatabase();
            player.UpdatePlayerStatus();
            SELECTED_SPEC = "";


        }
    }
}
