/*
 *   D2 account stash by Tolakram from Storm D2
 */
using System;
using System.Collections;
using System.Collections.Generic;
using DOL.Database;
using DOL.GS;
using DOL.GS.PacketHandler;

namespace DOL.Storm
{

    public class D2AccountStash : GameStaticItem, IGameInventoryObject
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This is used to synchronize actions on the vault.
        /// </summary>
        protected object m_vaultSync = new object();

        public object LockObject()
        {
            return m_vaultSync;
        }

        /// <summary>
        /// What is the first client slot of the window used to view this object?
        /// </summary>
        public virtual int FirstClientSlot
        {
            get { return (int)eInventorySlot.FirstVault; }
        }

        /// <summary>
        /// Last slot of the client window that shows this inventory
        /// </summary>
        public int LastClientSlot
        {
            get { return (int)eInventorySlot.LastVault; }
        }

        /// <summary>
        /// First slot in the DB.
        /// </summary>
        public virtual int FirstDBSlot
        {
            get { return 2000; }
        }

        /// <summary>
        /// Last slot in the DB.
        /// </summary>
        public virtual int LastDBSlot
        {
            get { return 2039; }
        }

        public virtual string GetOwner(GamePlayer player)
        {
            if (player == null)
            {
                log.Error("D2 Account Stash GetOwner(): player cannot be null!");
                return "PlayerIsNullError";
            }

            // it's an account vault so always return the players account objectId
            return player.Client.Account.ObjectId;
        }

        /// <summary>
        /// Do we handle a search?
        /// </summary>
        public bool SearchInventory(GamePlayer player, MarketSearch.SearchData searchData)
        {
            return false; // not applicable
        }

        /// <summary>
        /// Inventory of the Consignment Merchant, mapped to client slots
        /// </summary>
        public virtual Dictionary<int, InventoryItem> GetClientInventory(GamePlayer player)
        {
            return this.GetClientItems(player);
        }


        public override IList GetExamineMessages(GamePlayer player)
        {
            IList list = new ArrayList();
            list.Add("[Right click to display your account stash]");
            return list;
        }

        /// <summary>
        /// Player interacting with this vault.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool Interact(GamePlayer player)
        {
            if (!base.Interact(player))
                return false;

            player.Out.SendInventoryItemsUpdate(GetClientInventory(player), eInventoryWindowType.PlayerVault);
            return true;
        }

        /// <summary>
        /// List of items in this vault.
        /// </summary>
        public IList<InventoryItem> DBItems(GamePlayer player)
        {
            string sqlWhere = String.Format("OwnerID = '{0}' and SlotPosition >= {1} and SlotPosition <= {2}", GetOwner(player), FirstDBSlot, LastDBSlot);
            return GameServer.Database.SelectObjects<InventoryItem>(sqlWhere);
        }

        /// <summary>
        /// Is this a move request for an account vault
        /// </summary>
        /// <param name="player"></param>
        /// <param name="fromSlot"></param>
        /// <param name="toSlot"></param>
        /// <returns></returns>
        public virtual bool CanHandleMove(GamePlayer player, ushort fromSlot, ushort toSlot)
        {
            if (player == null || player.TargetObject != this)
                return false;

            return this.CanHandleRequest(player, fromSlot, toSlot);
        }

        /// <summary>
        /// Move an item from, to or inside this vault
        /// </summary>
        public virtual bool MoveItem(GamePlayer player, ushort fromClientSlot, ushort toClientSlot)
        {
            if (player == null || player.TargetObject != this)
                return false;

            if (this.CanHandleRequest(player, fromClientSlot, toClientSlot) == false)
                return false;

            // let's move it

            lock (m_vaultSync)
            {
                if (fromClientSlot == toClientSlot)
                {
                    NotifyObservers(player, null);
                }
                else if (fromClientSlot >= FirstClientSlot && fromClientSlot <= LastClientSlot)
                {
                    if (toClientSlot >= FirstClientSlot && toClientSlot <= LastClientSlot)
                    {
                        NotifyObservers(player, this.MoveItemInsideObject(player, (eInventorySlot)fromClientSlot, (eInventorySlot)toClientSlot));
                    }
                    else
                    {
                        NotifyObservers(player, this.MoveItemFromObject(player, (eInventorySlot)fromClientSlot, (eInventorySlot)toClientSlot));
                    }
                }
                else if (toClientSlot >= FirstClientSlot && toClientSlot <= LastClientSlot)
                {
                    NotifyObservers(player, this.MoveItemToObject(player, (eInventorySlot)fromClientSlot, (eInventorySlot)toClientSlot));
                }

                return true;
            }
        }


        /// <summary>
        /// Add an item to this object
        /// </summary>
        public virtual bool OnAddItem(GamePlayer player, InventoryItem item)
        {
            return true; // we don't have to do anything
        }

        /// <summary>
        /// Remove an item from this object
        /// </summary>
        public virtual bool OnRemoveItem(GamePlayer player, InventoryItem item)
        {
            return true; // we don't have to do anything
        }


        /// <summary>
        /// Not applicable for vaults
        /// </summary>
        public virtual bool SetSellPrice(GamePlayer player, ushort clientSlot, uint price)
        {
            return true; // we don't have to do anything after an item is added
        }

        public virtual void AddObserver(GamePlayer player)
        {
            // not applicable
        }

        public virtual void RemoveObserver(GamePlayer player)
        {
            // not applicable
        }

        /// <summary>
        /// Send inventory updates to all players actively viewing this vault;
        /// players that are too far away will be considered inactive.
        /// </summary>
        /// <param name="updateItems"></param>
        protected virtual void NotifyObservers(GamePlayer player, IDictionary<int, InventoryItem> updateItems)
        {
            player.Client.Out.SendInventoryItemsUpdate(updateItems, eInventoryWindowType.Update);
        }
    }
}