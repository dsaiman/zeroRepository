﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
namespace MasterOfThorns
{
    class Modes
    {
        private Obj_AI_Hero target;
        private Skills skills;
        private Program p;
        private int delay = 1000;
       
        public Skills getSkills()
        {
            return skills;
        }
     
        public void load(Program p)
        {
            skills = new Skills();
            this.p =p;
            target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Magical);
        }

        public Obj_AI_Hero getTarget()
        {
            target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Magical);
            return target;
        }
  
        public bool zyraZombie()
        {
            return ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name ==
                   ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Name ||
                   ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name ==
                   ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name;
        }

        public void laneClear()
        {          
            var minion = MinionManager.GetMinions(skills.getQ().Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = p.getMenu().Item("QL").GetValue<bool>();
            var useW = p.getMenu().Item("WL").GetValue<bool>();
            var useE = p.getMenu().Item("EL").GetValue<bool>();
            int min = p.getMenu().Item("seth").GetValue<Slider>().Value;
            int w = p.getMenu().Item("sethW").GetValue<Slider>().Value;
            int q = p.getMenu().Item("sethQ").GetValue<Slider>().Value;
       //     Game.PrintChat("q: " + q + " w: " + w + " e: " + min);
            //    if (!useQ && !useW && !useE) return;
            if (useQ && !useW && !useE)  skills.qCast(minion,q);
            else if (!useQ && useW && !useE) skills.wCast(minion,w);
            else if (!useQ && !useW && useE) skills.eCast(minion, min);
            else if (!useQ && useW && useE) 
            {
                  skills.eCast(minion, min);
                  if (skills.getE().IsReady() && skills.getE().IsInRange(minion))
                      skills.wCast(minion,w);
            }
            else if (useQ && !useW && useE) 
            {
                skills.eCast(minion, min);
                skills.qCast(minion,q);
            }
            else if (useQ && useW && !useE) 
            {
                skills.qCast(minion,q);
                if (skills.getQ().IsReady() && skills.getQ().IsInRange(minion))
                    skills.wCast(minion,w);
            }
            else if (useQ && useW && useE)
            {
                skills.eCast(minion, min);
                if (skills.getE().IsReady() && skills.getE().IsInRange(minion))
                {
                    skills.wCast(minion,w);
                    Utility.DelayAction.Add(delay, () => skills.qCast(minion,q));
                    if (skills.getQ().IsReady() && skills.getQ().IsInRange(minion))                    
                        Utility.DelayAction.Add(delay, () => skills.wCast(minion,w));                                    
                }
            }
            else
                return;    
        }
 
        public void jungleClear()
        {
            var minion = MinionManager.GetMinions(skills.getQ().Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = p.getMenu().Item("QJ").GetValue<bool>();
            var useW =p.getMenu().Item("WJ").GetValue<bool>();
            var useE =p.getMenu().Item("EJ").GetValue<bool>();
            int min = p.getMenu().Item("seth").GetValue<Slider>().Value;
            int q = p.getMenu().Item("sethQ").GetValue<Slider>().Value;
            int w = p.getMenu().Item("sethW").GetValue<Slider>().Value;
         //    Game.PrintChat("min: " + min);
        //    if (!useQ && !useW && !useE) return;
            if (useQ && !useW && !useE) skills.qCast(minion,q);
            else if (!useQ && useW && !useE) skills.wCast(minion,w);
            else if (!useQ && !useW && useE) skills.eCast(minion,min);           
            else if (!useQ && useW && useE) 
            {
                skills.eCast(minion, min);       
                if (skills.getE().IsReady() && skills.getE().IsInRange(minion))
                    skills.wCast(minion,w);
              
            }
            else if (useQ && !useW && useE) 
            {
                skills.eCast(minion, min);        
                skills.qCast(minion,q);             
            }
            else if (useQ && useW && !useE) 
            {
                skills.qCast(minion,q);
                if (skills.getQ().IsReady() && skills.getQ().IsInRange(minion)) 
                    skills.wCast(minion,w);                
            }
            else if (useQ && useW && useE)
            {
            //    Game.PrintChat("juhngle: ");
                skills.eCast(minion, min);
                if (skills.getE().IsReady() && skills.getE().IsInRange(minion))
                {
                    skills.wCast(minion,w);                   
                        Utility.DelayAction.Add(delay, () => skills.qCast(minion,q));
                        if (skills.getQ().IsReady() && skills.getQ().IsInRange(minion))                        
                            Utility.DelayAction.Add(delay, () => skills.wCast(minion,w));                                          
                }               
            }
            else
                return;    
 /*           
  * 
  * Funciona pero saca dos plantas iguales:
  * ----------
  *  skills.eCast(minion);
                if (skills.getE().IsReady() && skills.getE().IsInRange(minion))
                {
                    skills.wCast(minion);
                    Game.PrintChat("1 planta");

                   Utility.DelayAction.Add(50, () => skills.qCast(minion));
                    if (skills.getQ().IsReady())
                    {
                        Game.PrintChat("2 planta");
                        Utility.DelayAction.Add(400, () => skills.wCast(minion));
                        return true;
                    }
                    return true;
                }
                return false;
  * /*/
        }
        public void harrash(Obj_AI_Hero target)
        {            
            var useQ = p.getMenu().Item("QH").GetValue<bool>();
            var useW = p.getMenu().Item("WH").GetValue<bool>();
            int q = p.getMenu().Item("sethQ").GetValue<Slider>().Value;
            int w = p.getMenu().Item("sethW").GetValue<Slider>().Value;
         
            Game.PrintChat("haras useQ: "+useQ);
            //    if (!useQ && !useW && !useE) return;
            if (useQ && !useW)
                skills.qCast(getTarget(),q);
            else if (!useQ && useW)
                skills.wCast(getTarget(),w);
            else if (useQ && useW)
            {
                skills.qCast(getTarget(),q);
                if (skills.getQ().IsReady())
                    skills.wCast(getTarget(),w);
            }
            else
                return;     

        }

        public void flee(Obj_AI_Hero target)
        {
            if (skills.getE().IsReady())
            {
                int min = p.getMenu().Item("seth").GetValue<Slider>().Value;
                p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150));
                skills.eCast(target, min);
            }
            else
                return;
        }

        public void onlyR(Obj_AI_Hero target) // Comprobar
        {          
            if (skills.getR().IsReady()) //Añadir para cuantos campeones
            {
                p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150));
                var min = p.getMenu().Item("minEnemys").GetValue<Slider>().Value;
                skills.rCastHit(target, min);
            }
        }

        public void combo(Obj_AI_Hero target)
        {         
            var useQ = p.getMenu().Item("QC").GetValue<bool>();
            var useW = p.getMenu().Item("WC").GetValue<bool>();
            var useE = p.getMenu().Item("EC").GetValue<bool>();
            var min = p.getMenu().Item("seth").GetValue<Slider>().Value;
            int q = p.getMenu().Item("sethQ").GetValue<Slider>().Value;
            int w = p.getMenu().Item("sethW").GetValue<Slider>().Value;
            //    if (!useQ && !useW && !useE) return;
            if (useQ && !useW && !useE)  skills.qCast(getTarget(),q);            
            else if (!useQ && useW && !useE) skills.wCast(getTarget(),w);             
            else if (!useQ && !useW && useE) skills.eCast(getTarget(), min);
            else if (!useQ && useW && useE)
            {                
                skills.eCast(getTarget(), min);
                if (skills.getE().IsReady() && skills.getE().IsInRange(getTarget()))
                    skills.wCast(getTarget(),w); 
            }
            else if (useQ && !useW && useE) 
            {               
                skills.eCast(getTarget(), min);
                skills.qCast(getTarget(),q);
            }
            else if (useQ && useW && !useE) 
            {
                skills.qCast(getTarget(),q);
                if (skills.getQ().IsReady() && skills.getQ().IsInRange(getTarget()))
                    skills.wCast(getTarget(),w); 
            }
            else if (useQ && useW && useE)
            {
                    skills.eCast(target,min);
            if (skills.getE().IsReady())
            skills.wCast(target,w);
            skills.qCast(target,q);
           if(skills.getQ().IsReady())
           skills.wCast(target,w);
            /*    skills.eCast(getTarget(), min);
                if (skills.getE().IsReady() && skills.getE().IsInRange(getTarget()))
                    skills.wCast(getTarget(), w);
                
                 
                     skills.qCast(getTarget(), q);
                    if (skills.getQ().IsReady() && skills.getQ().IsInRange(getTarget()))  
                       skills.wCast(getTarget(),w);  */
                  
                //        Utility.DelayAction.Add(delay, () => skills.wCast(getTarget(),w));         
                //    skills.wCast(getTarget(),w);
                   // Utility.DelayAction.Add(delay, () => skills.qCast(getTarget(),q));
                 //   if (skills.getQ().IsReady() && skills.getQ().IsInRange(getTarget()))                    
                //        Utility.DelayAction.Add(delay, () => skills.wCast(getTarget(),w));         
                
            }
            else
                return;
        }

        public void rCombo(Obj_AI_Hero target)
        {        
            p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150)); //¿? No entiedo 
            var useQ = p.getMenu().Item("QrC").GetValue<bool>();
            var useE = p.getMenu().Item("ErC").GetValue<bool>();
            int min = p.getMenu().Item("seth").GetValue<Slider>().Value;
            int q = p.getMenu().Item("sethQ").GetValue<Slider>().Value;
            int w = p.getMenu().Item("sethW").GetValue<Slider>().Value;
            int r = p.getMenu().Item("sethR").GetValue<Slider>().Value;
            //    if (!useQ && !useW && !useE) return;
            if (useQ && !useE) 
            {
                skills.eCast(getTarget(),min);
                skills.rCast(getTarget(),r);
                skills.qCast(getTarget(),q);
            }
            else if (!useQ && useE)
            {
                skills.rCast(getTarget(),r);
                skills.eCast(getTarget(), min);
                if (skills.getE().IsReady() && skills.getE().IsInRange(getTarget()))
                    skills.wCast(getTarget(),w);                 
            }
            else if (useQ && useE)
            {

                skills.eCast(getTarget(), min);
                if (skills.getE().IsReady() && skills.getE().IsInRange(getTarget()))
                {
                    skills.wCast(getTarget(),w);
                    Utility.DelayAction.Add(delay, () => skills.qCast(getTarget(),q));
                    if (skills.getQ().IsReady() && skills.getQ().IsInRange(getTarget()))
                        Utility.DelayAction.Add(delay, () => skills.wCast(getTarget(),w));
                }
                skills.rCast(getTarget(),r);               
            }
            else
                return;   
        }
    }
}
