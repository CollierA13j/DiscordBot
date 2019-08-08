using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services;

namespace DiscordBot
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        // Starts asyncronous main method
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 100,
                AlwaysDownloadUsers = true
            });

            _client.Log += Logger.Log;
            _client.ReactionAdded += HandleReactions;

            await InitializeCommands();
            await AttemptLogin();
            await _client.StartAsync();

            // Block this task until the program is closed
            await Task.Delay(-1);
        }

        private async Task HandleReactions(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var msg = reaction.Message.Value;
            var context = new SocketCommandContext(_client, msg);

            try
            {
                if (reaction.MessageId == Global.ReactionMessageID && !context.User.IsBot)
                {
                    if (reaction.Emote.Name == "👌")
                    {
                        var user = reaction.User.Value;
                        var role = context.Guild.Roles.FirstOrDefault(x => x.Name == "testRole");
                        await (user as SocketGuildUser).AddRoleAsync(role);
                        await msg.RemoveReactionAsync(new Emoji("👌"), user);
                    }
                }
            }
            catch (Discord.Net.HttpException e)
            {
                Console.WriteLine("Discord.Net.HttpException");
                Console.WriteLine("Error Code: " + e.HttpCode.ToString());
                Console.WriteLine("Bot likely does not have correct permissions to assign roles");
                Console.WriteLine("Please check it's permissions and try again");
            }
        }

        private async Task InitializeCommands()
        {
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
        }

        // Attemps to login to bot's account; prints error if failure occurs
        private async Task AttemptLogin()
        {
            try
            {
                // It's unadvisable to hardcode your token
                await _client.LoginAsync(TokenType.Bot,
                "NTgzNzI0NDA3MzQ0NzI2MDI2.XURccw.9UxDOkk9TTTsD8Tm8xw2Agqzy6k");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Please check internet connection and bot token");
                Console.WriteLine("Press any key to continue: ");
                Console.ReadKey();
            }
        }
    }
}
