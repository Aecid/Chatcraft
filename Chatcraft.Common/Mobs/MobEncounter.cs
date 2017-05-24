using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatcraft.Pages
{
    /// <summary>
    /// Создатель мобов
    /// </summary>
    public static class MobEncounter
    {
        /// <summary>
        /// Создать моба и начасть сражение с игроком
        /// </summary>
        /// <param name="session"></param>
        /// <param name="mob"></param>
        /// <returns></returns>
        public static bool Start(Player session, Mob mob)
        {
            Stopwatch s = new Stopwatch();
            Task photo = null;
            s.Start();
            StringBuilder encounter = new StringBuilder();
            string mobName = "\nВам повстречался " + mob.Name + "!";
            if (!mob.Pic.Equals(string.Empty))
            {
                photo = session.SendPhoto(mob.Pic, mobName);
            }
            else
            {
                encounter.Append(mobName);
            }
            encounter.Append("\nВы немедленно ввязались в драку");
            var mobHP = mob.HP;
            var charHP = session.GetHP();
            bool outcome = true;
            while ( mobHP > 0 && charHP > 0 )
            {
                int mobDmg = mob.Atk + Helper.Rnd.Next(0, 2);
                mobDmg = mobDmg <= session.GetDefense() ? 1 : mobDmg - session.GetDefense();
                encounter.Append($"\n{mob.Name} ударил вас на ⚔{mobDmg} урона");
                charHP -= mobDmg;
                encounter.Append($" (❤️{charHP})");
                if (charHP > 0) {
                    int charAtk = session.GetAttack();
                    charAtk -= mob.Def;
                encounter.Append($"\nВы ударили [{mob.Name}] на ⚔{charAtk} урона.");
                mobHP -= charAtk;
                }
                if (mobHP <= 0)
                { 
                    session.AddStatsCounter($"Убито врагов \"{mob.Name}\"");
                    var gotGold = mob.Level + Helper.Rnd.Next(0, 2);
                    encounter.Append($"\nВы победили! {mob.Name} повержен!");
                    encounter.Append("\nВы нашли:");
                    encounter.Append($"\nЗолото: 💰{gotGold}");
                    if (mob.Level >= session.GetLevel())
                    {
                        int exp = mob.Level >= session.Level ? mob.Level : (mob.Level + session.Level)*2;
                        encounter.Append($"\nОпыт: 🔥{exp}");
                        session.AddExp(exp);
                    }
                    session.AddGold(gotGold);
                    session.DealDamage(session.GetHP() - charHP);

                    foreach (var item in mob.LootTable)
                    {
                        if (Helper.Rnd.Next(0, 100) < item.Value)
                        {
                            session.Items.Add(item.Key);
                            encounter.Append($"\n{Items.GetItemName(item.Key)}");
                        }
                    }


                    outcome = true;
                    break;
                }
                if (charHP <= 0)
                {
                    encounter.Append($"\n{mob.Name} убил вас и поглумился над вашим трупом");
                    session.DealDamage(session.GetMaxHP());
                    
                    outcome = false;
                    break;
                }

            }
 
            if (photo != null) photo.ContinueWith(async (t) =>  { await session.SendMessage(encounter.ToString()).ContinueWith(
                (d) =>
                {
                    if (!outcome) QuestsPage.FailCurrentQuest(session);
                }
                ); });
            else session.SendMessage(encounter.ToString()).ContinueWith(
                (d) =>
                {
                    if (!outcome) QuestsPage.FailCurrentQuest(session);
                }
                );

            return outcome;
        }
    }
}
