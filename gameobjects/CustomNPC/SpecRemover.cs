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
                SayTo(player, string.Join("],[", values));
                for (var i = 0; i < values.Length; i++)
                {
                    Specialization tempSpec = SkillBase.GetSpecialization(values[i]);
                    i++;

                    if (tempSpec != null)
                    {
                        if (tempSpec.AllowSave)
                        {
                            int level;
                            level = player.Level;
                           // if (player.HasSpecialization(tempSpec.KeyName))
                           // {
                          //      player.GetSpecializationByName(tempSpec.KeyName).Level = level;

                         //   }
                         //   else
                          //  {
                           //     tempSpec.Level = level;
                           //     player.AddSpecialization(tempSpec);
                           // }


                        }
                        //player.Out.SendUpdatePoints();
                       // player.Out.SendUpdatePlayerSkills();
                       // player.SaveIntoDatabase();
                       // player.UpdatePlayerStatus();
                        SayTo(player, "Done with whatever.");
                    }

                }
            } return false;
        }
        public override bool WhisperReceive(GameLiving source, string text)

        {

            if (!base.WhisperReceive(source, text) || !(source is GamePlayer))
                return false;
            GamePlayer player = source as GamePlayer;


            if (text.Length > 1)
            {

            }
            return false;
        }
    }
}
