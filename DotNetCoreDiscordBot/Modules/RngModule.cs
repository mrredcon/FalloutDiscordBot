﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Addons.Preconditions;
using DotNetDiscordBot.Services;

namespace DotNetDiscordBot.Modules
{
    public class RngModule : ModuleBase<SocketCommandContext>
    {
        [Group("roll")]
        public class RollModule : ModuleBase<SocketCommandContext>
        {
            public RollService RollService { get; set; }
            
            //[Command("repair")]
            //public async Task RollRepairAsync()
            //{
            //    var playerId = Context.User.Id;
            //}
            [Command("guns"), Ratelimit(1,5,Measure.Seconds)]
            public async Task RollGuns()
            {
                await ReplyAsync("guns!");
            }
        }
    }
}
