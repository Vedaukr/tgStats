using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLSharp.Core;
using TLSharp.Core.Exceptions;
using TeleSharp.TL;
using TeleSharp.TL.Messages;

namespace tgStats
{
    class Program
    {
        private const int _apiId = 1073030;
        private const string _apiHash = "46cc3cc66c280ca4eddd2f4b42f683c4";
        private const string _number = "+380974760340";
        static void Main(string[] args)
        {
            Start();
        }

        private static async void Start()
        {
            var client = new TelegramClient(_apiId, _apiHash);
            await client.ConnectAsync();
            TLUser user = await AuthUser(client);
            var dialogs = (TLDialogsSlice) await client.GetUserDialogsAsync();

            foreach (var chat in dialogs.Chats)
            {
                Console.WriteLine();
            }
        }

        public static async Task<TLUser> AuthUser(TelegramClient client)
        {

            var hash = await client.SendCodeRequestAsync(_number);
            var code = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(code))
            {
                throw new Exception("CodeToAuthenticate is empty in the app.config file, fill it with the code you just got now by SMS/Telegram");
            }

            TLUser user = null;

            try
            {
                user = await client.MakeAuthAsync(_number, hash, code);
            }
            catch (InvalidPhoneCodeException ex)
            {
                throw new Exception("CodeToAuthenticate is wrong in the app.config file, fill it with the code you just got now by SMS/Telegram",
                                    ex);
            }

            return user;
        }
    }
}
