using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiscordBot
{
    class Program
    {
        private DiscordSocketClient _client;
        public static IConfiguration _config;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _config = BuildConfig();

            _client.Log += Log;

            var services = BuildServiceProvider();
            await services.GetRequiredService<Services.CommandHandlerService>().InitializeAsync(services);

            await _client.LoginAsync(TokenType.Bot, _config["token"]);
            await _client.StartAsync();

            System.IO.Directory.CreateDirectory(Character.saveLoc);

            await Task.Delay(-1);
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton<CommandService>()
            .AddSingleton<Services.CommandHandlerService>()
            .AddSingleton<Services.CharacterCreateService>()
            .AddSingleton<Services.CharacterLoadService>()
            .AddSingleton<Services.CharacterUtilityService>()
            .AddSingleton<Services.ExperienceService>()
            .AddSingleton<Services.RollService>()
            .BuildServiceProvider();

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("config.json")
                .Build();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
