using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordKBot.Commands
{
    public class KBotCommands : BaseCommandModule
    {
        [Command("chart")]
        public async Task Chart(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Okay : Happy\nOké : Gay\nOkey : Bored\nOke : Very bored\nOk : Bland\nKay : Annoyed\nk : Mad\nK : Very mad\nO.K. : Boomer\nOkayyyy : Horny asf\nKk : Hyperactive\nKkk : White supremacist\nMk : Bored + mad").ConfigureAwait(false);

        }

        [Command("ineedafriend")]
        public async Task INeedAFriend(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("No friends, no worries!\nYou can add your own KBot to your server for countless hours of ||~~pain~~|| fun.\nClick to add me: https://discord.com/oauth2/authorize?client_id=782400932917805056&scope=bot&permissions=1").ConfigureAwait(false);
        }

        [Command("didanyoneask")]
        public async Task DidAnyoneAsk(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Dear {ctx.Member.Mention}, please stfu.").ConfigureAwait(false);
            DiscordDmChannel dm = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);
            await dm.SendMessageAsync("In case you need to be told twice, stfu.").ConfigureAwait(false);
        }
    }
}
