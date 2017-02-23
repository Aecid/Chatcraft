using System;
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

namespace Chatcraft
{
        public sealed class BotClient : TelegramBotClient
        {
            private static readonly Lazy<BotClient> lazy =
                new Lazy<BotClient>(() => new BotClient("304466149:AAETkid6XNgwuQi3ui9P9IFQ6C55Ifn3gSA"));

            public static BotClient Instance { get { return lazy.Value; } }

            private BotClient(string token)
            :base(token)
                {
                }

        }
    }
