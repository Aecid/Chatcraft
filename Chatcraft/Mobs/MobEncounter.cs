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
            string mobName = "\nВам повстречался " + mob.name + "!";
            if (!mob.pic.Equals(string.Empty))
            {
                photo = session.SendPhoto(mob.pic, mobName);
            }
            else
            {
                encounter += mobName;
            }
            encounter += "\nВы немедленно ввязались в драку";
            var mobHP = mob.hp;
            var charHP = session.GetHP();
            bool outcome = true;
            while ( mobHP > 0 && charHP > 0 )
            {
                int mobDmg = mob.atk + Helper.rnd.Next(0, 2);
                mobDmg = mobDmg <= session.GetDefense() ? 1 : mobDmg - session.GetDefense();
                encounter += "\n" + mob.name + " ударил вас на ⚔" + mobDmg + " урона";
                charHP -= mobDmg;
                encounter += " (❤️" + charHP + ")";
                if (charHP > 0) {
                    int charAtk = session.GetAttack();
                    charAtk -= mob.def;
                encounter += "\nВы ударили [" + mob.name + "] на ⚔" + charAtk + " урона.";
                mobHP -= charAtk;
                }
                if (mobHP <= 0)
                { 
                    session.AddStatsCounter("Убито врагов \""+mob.name+"\"");
                    var gotGold = mob.level + Helper.rnd.Next(0, 2);
                    encounter += "\nВы победили! " + mob.name + " повержен!";
                    encounter += "\nВы нашли:";
                    encounter += "\nЗолото: 💰" + gotGold;
                    if (mob.level >= session.GetLevel())
                    {
                        int exp = mob.level >= session.level ? mob.level : (mob.level + session.level)*2;
                        encounter += "\nОпыт: 🔥" + exp;
                        session.AddExp(exp);
                    }
                    session.AddGold(gotGold);
                    session.DealDamage(session.GetHP() - charHP);

                    foreach (var item in mob.lootTable)
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
                    encounter += "\n" + mob.name + " убил вас и поглумился над вашим трупом";
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
