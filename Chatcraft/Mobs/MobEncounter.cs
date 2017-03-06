using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft.Pages
{
    public static class MobEncounter
    {
        public static bool Start(Session session, Mob mob)
        {
            Stopwatch s = new Stopwatch();
            Task photo = null;
            s.Start();
            string encounter = "";
            string mobName = "\nВам повстречался " + mob.Name + "!";
            if (!mob.Pic.Equals(string.Empty))
            {
                photo = session.SendPhoto(mob.Pic, mobName);
            }
            else
            {
                encounter += mobName;
            }
            encounter += "\nВы немедленно ввязались в драку";
            var mobHP = mob.HP;
            var charHP = session.GetHP();
            bool outcome = true;
            while ( mobHP > 0 && charHP > 0 )
            {
                int mobDmg = mob.Atk + Helper.rnd.Next(0, 2);
                mobDmg = mobDmg <= session.GetDefense() ? 1 : mobDmg - session.GetDefense();
                encounter += "\n" + mob.Name + " ударил вас на ⚔" + mobDmg + " урона";
                charHP -= mobDmg;
                encounter += " (❤️" + charHP + ")";
                if (charHP > 0) {
                    int charAtk = session.GetAttack();
                    charAtk -= mob.Def;
                encounter += "\nВы ударили [" + mob.Name + "] на ⚔" + charAtk + " урона.";
                mobHP -= charAtk;
                }
                if (mobHP <= 0)
                { 
                    session.AddStatsCounter("Убито врагов \""+mob.Name+"\"");
                    var gotGold = mob.Level + Helper.rnd.Next(0, 2);
                    encounter += "\nВы победили! " + mob.Name + " повержен!";
                    encounter += "\nВы нашли:";
                    encounter += "\nЗолото: 💰" + gotGold;
                    if (mob.Level >= session.GetLevel())
                    {
                        int exp = mob.Level >= session.Level ? mob.Level : (mob.Level + session.Level)*2;
                        encounter += "\nОпыт: 🔥" + exp;
                        session.AddExp(exp);
                    }
                    session.AddGold(gotGold);
                    session.DealDamage(session.GetHP() - charHP);

                    foreach (var item in mob.LootTable)
                    {
                        if (Helper.rnd.Next(0, 100) < item.Value)
                        {
                            session.items.Add(item.Key);
                            encounter += "\n"+Items.GetItemName(item.Key);
                        }
                    }


                    outcome = true;
                    break;
                }
                if (charHP <= 0)
                {
                    encounter += "\n" + mob.Name + " убил вас и поглумился над вашим трупом";
                    session.DealDamage(session.GetMaxHP());
                    outcome = false;
                    break;
                }

            }
 
            if (photo != null) photo.ContinueWith(async (t) =>  { await session.SendMessage(encounter).ContinueWith(
                (d) =>
                {
                    if (!outcome) QuestsPage.FailCurrentQuest(session);
                }
                ); });
            else session.SendMessage(encounter).ContinueWith(
                (d) =>
                {
                    if (!outcome) QuestsPage.FailCurrentQuest(session);
                }
                );

            return outcome;
        }
    }
}
