using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNetDiscordBot.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private IServiceProvider _services;
        public static readonly char cmd_prefix = Program._config["cmd_prefix"][0];
        public static readonly int exp_cooldown = int.Parse(Program._config["exp_cooldown"]);
        private ExperienceService _expservice;
        Discord.Addons.Preconditions.RatelimitAttribute ratelimit;

        public CommandHandlerService(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _commands = commands;
            _client = client;
            _services = services;
            _expservice = new ExperienceService();
            ratelimit = new Discord.Addons.Preconditions.RatelimitAttribute(1, exp_cooldown, Discord.Addons.Preconditions.Measure.Minutes);
        }

        public async Task InitializeAsync(IServiceProvider services)
        {
            _services = services;
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;
            // ...and our exp "handler"
            _client.MessageReceived += AwardExperienceAsync;

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix
            if (!(message.HasCharPrefix(cmd_prefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);

            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired,
            // as it may clog up the request queue should a user spam a
            // command.
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }

        public async Task AwardExperienceAsync(SocketMessage messageParam)
        {
            // Don't process the message if it was a system message, or if it was a DM
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Don't process the message if it was supposed to be a command
            int argPos = 0;
            if (message.HasCharPrefix(cmd_prefix, ref argPos))
                return;

            var user = message.Author;

            if (user.IsBot) return;
            var channel = message.Channel as SocketGuildChannel;
            if (channel == null) return;

            var context = new SocketCommandContext(_client, message);

            if (CharacterUtilityService.CharacterExists(user))
            {
                byte level = ExperienceService.GetCharacterLevel(user);

                // passing the 5 minute check
                if (ratelimit.BastardizedCheckPermissionsAsync(context, _services) && level < 50)
                    ExperienceService.AwardExp(user);

                byte newLevel = ExperienceService.GetCharacterLevel(user);
                if (newLevel > level)
                {
                    await context.Channel.SendMessageAsync(user.Mention + " is now level " + newLevel + "!");
                    await ExperienceService.CharacterLevelUp(user);
                }
            }
            else
                return;
        }
    }
}
