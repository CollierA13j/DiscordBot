using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Services
{
    internal class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            // Hook the MessageReceived event into command handler
            _client.MessageReceived += HandleCommandAsync;
        }

        // Processes a message to test for command keywords
        private async Task HandleCommandAsync(SocketMessage rawMessage)
        {
            var msg = rawMessage as SocketUserMessage;

            // ignore system messages or DMs
            if ((msg == null) || (msg.Channel == msg.Author.GetOrCreateDMChannelAsync()))
                return;

            // ignore messages from bots
            var context = new SocketCommandContext(_client, msg);
            if (context.User.IsBot) return;

            // variable to track where prefix is
            int argPos = 0;

            // Ignore messages without prefix
            if (msg.HasCharPrefix('!', ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos, null);
            }

        }
    }
}
