using System.Text;
using System.Linq;

using DOL.Database;
using DOL.GS.Commands;
using DOL.GS.PacketHandler;

namespace DOL.GS.Commands
{
    [Cmd(
        "&msell",
        //new string[] { "&trainline", "&trainskill" }, // new aliases to work around 1.105 client /train command
        ePrivLevel.Player,
        "Sell a chunk of the player's inventory.",
        "/msell <back pack slot start #> <back pack slot end #>",
        "e.g. /msell 49 63")]
    public class MsellCommandHandler : DOL.GS.Commands.AbstractCommandHandler, DOL.GS.Commands.ICommandHandler
    {
        public void OnCommand(GameClient client, string[] args)
        {
            // must add at least one parameter just to be safe
            if (args.Length > 1 && args[1].ToString() == "YES")
            {
                string arg1 = args[1].ToString();
                string arg2 = args[2].ToString();
                string args3 = arg1 + arg2;
                foreach (InventoryItem item in client.Player.Inventory.GetItemRange(eInventorySlot.FirstBackpack, eInventorySlot.LastBackpack))
                    client.Player.Inventory.RemoveItem(item);

                client.Out.SendMessage(args3, eChatType.CT_System, eChatLoc.CL_SystemWindow);
            }
            else
            {
                DisplaySyntax(client);
            }
        }
    }
}
