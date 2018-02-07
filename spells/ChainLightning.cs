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
//This Spell Handler is written by Dinberg, originally for use with The Marvellous Contraption but now released to all.
using System;
using System.Collections;
using System.Collections.Generic;
using DOL.AI.Brain;
using DOL.GS.PacketHandler;
using DOL.GS.Keeps;
using DOL.Events;
using DOL.Language;

namespace DOL.GS.Spells
{
    /// <summary>
    /// 
    /// </summary>
    [SpellHandlerAttribute("ChainedDD")]
    public class ChainedDD : DirectDamageSpellHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //I've got these two voids here to prevent the spellhandler's defaults from interfering with my choreographed technique.
        public override void SendEffectAnimation(GameObject target, ushort boltDuration, bool noSound, byte success)
        {
        }

        public override void SendEffectAnimation(GameObject target, ushort clientEffect, ushort boltDuration, bool noSound, byte success)
        {
        }

        public override int CalculateSpellResistChance(GameLiving target)
        {
            return 0; //spell can never be resisted initially, only once the bolt hits the target.
        }

        /// <summary>
        /// This custom method prevents interference from non-zero radius (used now to denote the 'chain-range')
        /// </summary>
        /// <param name="castTarget"></param>
        /// <returns></returns>
        public override IList SelectTargets(GameObject castTarget)
        {
            ArrayList list = new ArrayList();
            if (castTarget is GameLiving == false)
                return list;
            if (GameServer.ServerRules.IsAllowedToAttack(Caster, (GameLiving)castTarget, true))
                list.Add(castTarget);
            return list;
        }

        /// <summary>
        /// execute direct effect
        /// </summary>
        /// <param name="target">target that gets the damage</param>
        /// <param name="effectiveness">factor from 0..1 (0%-100%)</param>
        public override void OnDirectEffect(GameLiving target, double effectiveness)
        {
            currentSource = Caster;
            currentTarget = target;
            TryLightning();
        }


        private GameLiving currentTarget;
        private GameLiving currentSource;
        private double currentDamageNerf = 1.0;
        int recursions = 0;

        public void TryLightning()
        {
            GamePlayer CheckPlayer = Caster as GamePlayer;
            if (CheckPlayer == null)
                CheckPlayer = currentTarget as GamePlayer;

            if (CheckPlayer == null)
                CheckPlayer = currentSource as GamePlayer;



            if (CheckPlayer != null)
                CheckPlayer.Out.SendCheckLOS(currentSource, currentTarget, new CheckLOSResponse(LightningLOSReply));
        }

        /// <summary>
        /// Begins the callback between this gameliving source and target (commences a bolt flying between them)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void DealLightning()
        {
            int ticksToTarget = currentSource.GetDistanceTo(currentTarget) * 100 / 85; // 85 units per 1/10s
            int delay = 1 + ticksToTarget / 100;
            foreach (GamePlayer player in currentTarget.GetPlayersInRadius(WorldMgr.VISIBILITY_DISTANCE))
            {
                player.Out.SendSpellEffectAnimation(currentSource, currentTarget, m_spell.ClientEffect, (ushort)(delay), false, 1);
            }
            BoltAction bolt = new BoltAction(this);
            bolt.Start(1 + ticksToTarget);
        }

        /// <summary>
        /// Fired once the chained DD hits its target.
        /// Deals damage and jumps to the next target where possible.
        /// </summary>
        public void LightningHits()
        {
            if (Util.Chance(base.CalculateSpellResistChance(currentTarget)))
            {
                MessageToCaster("Your spell is resisted!", eChatType.CT_SpellResisted);
                return;
            }

            //First we damage our current target.
            DealDamage(currentTarget, currentDamageNerf);

            currentDamageNerf *= ((double)Spell.LifeDrainReturn / 100.0);
            recursions++;
            if (recursions <= (int)Spell.Value)
            {
                //Find an enemy we can strike next to this current target.
                List<GameLiving> enemies = new List<GameLiving>();
                foreach (GameNPC npc in currentTarget.GetNPCsInRadius((ushort)Spell.Radius))
                    if (GameServer.ServerRules.IsAllowedToAttack(Caster, npc, false))
                        enemies.Add(npc);
                foreach (GamePlayer pc in currentTarget.GetPlayersInRadius((ushort)Spell.Radius))
                    if (GameServer.ServerRules.IsAllowedToAttack(Caster, pc, false))
                        enemies.Add(pc);

                if (enemies.Count == 0)
                    return;

                currentSource = currentTarget;
                currentTarget = enemies[Util.Random(enemies.Count - 1)];
                if (currentSource != currentTarget)
                    TryLightning();
                enemies.Clear();
            }
        }

        /// <summary>
        /// This action represents a delay as the chained DD bounces from target to target.
        /// </summary>
        protected class BoltAction : RegionAction
        {
            /// <summary>
            /// The spell handler
            /// </summary>
            protected readonly ChainedDD m_handler;

            /// <summary>
            /// Constructs a new BoltOnTargetAction
            /// </summary>
            /// <param name="actionSource">The action source</param>
            /// <param name="boltTarget">The bolt target</param>
            /// <param name="spellHandler"></param>
            public BoltAction(ChainedDD spellHandler)
                : base(spellHandler.Caster)
            {
                m_handler = spellHandler;
            }

            protected override void OnTick()
            {
                m_handler.LightningHits();
            }
        }

        protected virtual void LightningLOSReply(GamePlayer player, ushort response, ushort targetOID)
        {
            if (player == null || Caster.ObjectState != GameObject.eObjectState.Active)
                return;

            if ((response & 0x100) == 0x100)
            {
                DealLightning();
            }
        }

        public override IList<string> DelveInfo
        {
            get
            {
                var list = new List<string>(32);
                //list.Add("Function: " + (Spell.SpellType == "" ? "(not implemented)" : Spell.SpellType));
                //list.Add(" "); //empty line
                list.Add(Spell.Description);
                list.Add(" "); //empty line
                if (Spell.InstrumentRequirement != 0)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.InstrumentRequire", GlobalConstants.InstrumentTypeToName(Spell.InstrumentRequirement)));

                list.Add("Base Damage: " + Spell.Damage.ToString("0.###;0.###'%'"));
                list.Add("Rebound Damage Factor: " + (Spell.LifeDrainReturn).ToString("###") + "%");
                list.Add("Maximum Rebounds: " + Spell.Value.ToString("0.##"));

                list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.Target", Spell.Target));
                if (Spell.Range != 0)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.Range", Spell.Range));
                if (Spell.Duration >= ushort.MaxValue * 1000)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.Duration") + " Permanent.");
                else if (Spell.Duration > 60000)
                    list.Add(string.Format(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.Duration") + " " + Spell.Duration / 60000 + ":" + (Spell.Duration % 60000 / 1000).ToString("00") + " min"));
                else if (Spell.Duration != 0)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.Duration") + " " + (Spell.Duration / 1000).ToString("0' sec';'Permanent.';'Permanent.'"));
                if (Spell.Frequency != 0)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.Frequency", (Spell.Frequency * 0.001).ToString("0.0")));
                if (Spell.Power != 0)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.PowerCost", Spell.Power.ToString("0;0'%'")));
                list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.CastingTime", (Spell.CastTime * 0.001).ToString("0.0## sec;-0.0## sec;'instant'")));
                if (Spell.RecastDelay > 60000)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.RecastTime") + " " + (Spell.RecastDelay / 60000).ToString() + ":" + (Spell.RecastDelay % 60000 / 1000).ToString("00") + " min");
                else if (Spell.RecastDelay > 0)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.RecastTime") + " " + (Spell.RecastDelay / 1000).ToString() + " sec");
                if (Spell.Concentration != 0)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.ConcentrationCost", Spell.Concentration));
                if (Spell.Radius != 0)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.Radius", Spell.Radius));
                if (Spell.DamageType != eDamageType.Natural)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.Damage", GlobalConstants.DamageTypeToName(Spell.DamageType)));
                if (Spell.IsFocus)
                    list.Add(LanguageMgr.GetTranslation(ServerProperties.Properties.SERV_LANGUAGE, "DelveInfo.Focus"));

                return list;
            }
        }

        // constructor
        public ChainedDD(GameLiving caster, Spell spell, SpellLine line) : base(caster, spell, line) { }
    }
}
