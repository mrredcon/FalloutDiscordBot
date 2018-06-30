using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiscordBot.Services
{
    public class RollService
    {
        public string GetRollResult(Character.SkillEnum skill, SocketUser user)
        {
            Random rand = new Random();

            var userId = user.Id;

            return "I'm not implemented yet!";
        }
    }
}
