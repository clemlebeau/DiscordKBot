using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using DiscordKBot.Commands;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;

namespace DiscordKBot
{
    class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            string json = string.Empty;

            using (FileStream fs = File.OpenRead("config.json"))
            {
                using (StreamReader sr = new StreamReader(fs, new UTF8Encoding(false)))
                {
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            ConfigJson configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            DiscordConfiguration config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,

            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;
            Client.MessageCreated += OnMessageCreated;

            CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                IgnoreExtraArguments = false,
                CaseSensitive = false
            };

            InteractivityConfiguration interactivityConfiguration = new InteractivityConfiguration
            {

            };

            Client.UseInteractivity(interactivityConfiguration);

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<KBotCommands>();
            //Commands.RegisterCommands<FunCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }

        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {
            if (!e.Author.IsBot)
            {
                string message = e.Message.Content.ToString();
                
                #region K chart
                List<string> messageEmotions = GetEmotions(message);
                if (messageEmotions.Count > 0)
                {
                    string messageToSend = CreateMessage(messageEmotions, e.Message);
                    await e.Message.Channel.SendMessageAsync(messageToSend).ConfigureAwait(false);
                }
                #endregion

                foreach (string didiask in didiasks)
                {
                    if (message.ToLower().Contains(didiask))
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            await e.Message.Channel.SendMessageAsync($"{e.Author.Mention} STFU!").ConfigureAwait(false);
                            ulong userId = e.Message.Author.Id;
                            DiscordMember member = await e.Guild.GetMemberAsync(userId).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        private string[] didiasks = {"did i ask", "did anyone ask", "didiask", "didanyoneask", "did iask", "didi ask", "didanyone ask", "did anyoneask" };

        private string[] ks = { "okay", "oké", "okey", "oke", "ok", "kay", "_k", "_K", "_K.", "_O.K.", "okayyyy", "kk", "kkk", "mk" };
        private string[] emotions = { "Happy", "Gay", "Bored", "Very bored", "Bland", "Annoyed", "Mad", "Very mad", "Extremely mad", "A boomer", "Horny asf", "Hyperactive", "A white supremacist", "Bored + mad" };

        private List<string> GetEmotions(string message)
        {
            List<string> messageEmotions = new List<string>();

            string[] tokens = $"{message}".Split(' ');
            for (int i = 0; i < ks.Length; i++)
            {
                bool containsEmotion = false;
                bool caseSensitive = ks[i][0] == '_';
                string checkString = caseSensitive ? ks[i].Substring(1) : ks[i];

                foreach (string token in tokens)
                {
                    if (containsEmotion) continue;
                    containsEmotion = caseSensitive ? token.Equals(checkString) : token.ToLower().Equals(checkString);
                }
                //if (caseSensitive)
                //{
                //    //if (token.Contains($" { checkString} ") || token.Trim().Equals(checkString))
                //    //{
                //    //    containsEmotion = true;
                //    //}

                //}
                //else
                //{
                //    //if (message.ToLower().Contains($" { checkString} ") || message.ToLower().Trim().Equals(checkString))
                //    //{
                //    //    containsEmotion = true;
                //    //}
                //}

                if (containsEmotion)
                {
                    messageEmotions.Add(emotions[i]);
                }
            }

            return messageEmotions;
        }

        private string CreateMessage(List<string> messageEmotions, DSharpPlus.Entities.DiscordMessage discordMessage)
        {
            //discordMessage.Author.Mention;
            string msg = $"According to the ***Universal K Chart***, {discordMessage.Author.Mention} is ";
            if (messageEmotions.Count > 1)
            {
                for (int i = 0; i < messageEmotions.Count; i++)
                {
                    if (i == messageEmotions.Count - 2)
                    {
                        msg += $"{messageEmotions[i].ToLower()} ";
                    }
                    else if (i < messageEmotions.Count - 1)
                    {
                        msg += $"{messageEmotions[i].ToLower()}, ";
                    }
                    else
                    {
                        msg += $"and {messageEmotions[i].ToLower()}.";
                    }
                }
            }
            else
            {
                msg += $"{messageEmotions[0].ToLower()}.";
            }
            return msg;
        }
    }
}
