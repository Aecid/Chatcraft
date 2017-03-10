using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

using Chatcraft;

namespace Chatcraft
{
    static class Helper
    {
        public static Random Rnd = new Random();
        public static ReplyKeyboardMarkup GetKeyboard(string[] firstRow, string[] secondRow = null, bool back = false)
        {
            var firstRowKeys = new List<KeyboardButton>();
            var secondRowKeys = new List<KeyboardButton>();
            var backrow = new string[] { "Назад" };

            var keyboard = new ReplyKeyboardMarkup();
            foreach (var key in firstRow)
            {
                firstRowKeys.Add(new KeyboardButton(key));
            }

            if (secondRow != null)
            {
                foreach (var key in secondRow)
                {
                    secondRowKeys.Add(new KeyboardButton(key));
                }
            }
            else { secondRowKeys = null; }

            if (secondRow == null)
            {
                keyboard = new ReplyKeyboardMarkup(new[]
                {
                firstRowKeys.ToArray()
            }, true);
            }
            else
            {
                keyboard = new ReplyKeyboardMarkup(new[]
{
                firstRowKeys.ToArray(), secondRowKeys.ToArray()
            }, true);
            }

            if (back == true)
            {

            }

            return keyboard;
        }

        public static ReplyKeyboardMarkup GetKeyboard(string[][] keyRows)
        {
            var keyboard = new List<KeyboardButton[]>();
            foreach (var keyRow in keyRows)
            {
                var list = new List<KeyboardButton>();
                foreach (var key in keyRow)
                {
                    list.Add(new KeyboardButton(key));
                }

                keyboard.Add(list.ToArray());
            }

            return new ReplyKeyboardMarkup(keyboard.ToArray(), true);
        }

        public static InlineKeyboardMarkup GetVerticalInlineKeyboardByList(List<string> list)
        {
            var keyboardList = new List<InlineKeyboardButton[]>();

            foreach (var item in list)
            {
                keyboardList.Add(new InlineKeyboardButton[] { new InlineKeyboardButton(item) });
            }

            var keyboard = new InlineKeyboardMarkup(keyboardList.ToArray());

            return keyboard;
        }

        public static void WriteToJsonFile<Session>(string filePath, Session session, bool append = false) where Session : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(session);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static Session ReadFromJsonFile<Session>(string filePath) where Session : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Session>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public static string GetRandomLine(List<string> list)
        {
            int r = Rnd.Next(list.Count);
            return list[r];
        }

        //public static string LootTable(Dictionary<int, int>)

        public static string FormattedSpace(string val, int fixedLen)
        {
            int len = 0;
            string retVal = string.Empty;
            try
            {
                len = val.Length;
                retVal = val;
                for (int cnt = 0; cnt < fixedLen - len - 1; cnt++)
                {
                    retVal += " ";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return retVal;
        }

        public static string FormattedTable(Dictionary<string, string> leftColumn, Dictionary<string, string> rightColumn = null, int columnWidth = 20)
        {
            string table = "";

            if (rightColumn == null)
            {
                string horizontalBorder = "";
                horizontalBorder += "+-";
                for (int width = 0; width < columnWidth-1; width++)
                {
                    horizontalBorder += "-";
                }
                horizontalBorder += "-+";

                table += horizontalBorder + "\n";

                foreach (var row in leftColumn)
                {
                    if (row.Value.Equals("%divider%")) table += horizontalBorder;
                    else table += "| " + FormattedSpace(row.Key + ": " + row.Value, columnWidth) + " |";
                    table += "\n";
                }

                table += horizontalBorder;
            }

            else
            {
                string horizontalBorder = "";
                horizontalBorder += "+-";
                for (int width = 0; width < columnWidth-1; width++)
                {
                    horizontalBorder += "-";
                }
                horizontalBorder += "-+";

                table += horizontalBorder + horizontalBorder + "\n";

                int count = 0;
                foreach (var row in leftColumn)
                {
                    if (row.Value.Equals("%divider%")) table += horizontalBorder;
                    else table += "| " + FormattedSpace(row.Key + ": " + row.Value, columnWidth) + " |";

                    if (count <= rightColumn.Count() - 1)
                    {
                        var elem = rightColumn.ElementAt(count);
                        if (elem.Value.Equals("%divider%")) table += "|-" + horizontalBorder + "-|";
                        else table += "| " + FormattedSpace(elem.Key + ": " + elem.Value, columnWidth) + " |";
                        count++;
                    }
                    else table += "| " + FormattedSpace("", columnWidth) + " |";

                    table += "\n";
                }

                table += horizontalBorder + horizontalBorder;
            }

            return table;
        }

        public static int Roll(string xdy)
        {
            int result = 0;
            if (xdy.Contains("d"))
            {
                var roll = xdy.Split('d');
                var x = int.Parse(roll[0]);
                var y = int.Parse(roll[1]);

                for (int r = 1; r <= x; r++)
                {
                    result += (Rnd.Next(1, y + 1));
                }
            }

            return result;
        }

        public static int AttackDefMath(int attack, int def)
        {
            int dmg = 0;
            //var critroll = rnd.Next(1, 21);
            //dmg =

            return dmg;
        }
    }
}
