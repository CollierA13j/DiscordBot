using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;

namespace DiscordBot.Modules
{
    public class RoleModule : ModuleBase<SocketCommandContext>
    {
        [Command("react")]
        public async Task ReactionRoleAsync()
        {
            RestUserMessage msg = await Context.Channel.SendMessageAsync("React to me!");
            await msg.AddReactionAsync(new Emoji("👌"));
            Global.ReactionMessageID = msg.Id;
        }
    }
}
