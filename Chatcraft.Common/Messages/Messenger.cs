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


namespace Chatcraft.Messages
{
    /// <summary>
    /// мессенджер
    /// </summary>
    class Messenger
    {
        /// <summary>
        /// Послать фото
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="photo">фото</param>
        public static async void SendPhoto(Message message, string photo)
        {
            await BotClient.Instance.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

            string file = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "//img//"+photo;

            var fileName = file.Split('/').Last();

            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fts = new FileToSend(fileName, fileStream);
                await BotClient.Instance.SendPhotoAsync(message.Chat.Id, fts, "Hi there, " + message.Chat.FirstName + " [" + message.Chat.Username + "] " + message.Chat.LastName + "!");
            }
        }
    }
}
