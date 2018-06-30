using Discord.Addons.Preconditions;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiscordBot.Modules
{
    public class CharacterMiscModule : ModuleBase<SocketCommandContext>
    {
        [Command("viewstat"), Ratelimit(1, 1, Measure.Minutes)]
        public async Task ViewStatsAsync(string statToView)
        {
            var user = Context.User;
            await ViewStatsAsync(user, statToView);
        }
        [Command("viewstat"), Ratelimit(1, 0.5, Measure.Minutes)]
        public async Task ViewStatsAsync(SocketUser user, string statToView)
        {
            if (Services.CharacterUtilityService.CharacterExists(user))
            {
                var character = Services.CharacterLoadService.LoadCharacter(user);
                StringBuilder message = new StringBuilder();
                message.Append("\n**" + character.Name + " (" + user.Username + ")" + ":**\n");

                if (statToView.Equals("exp") || statToView.Equals("level") || statToView.Equals("experience") || statToView.Equals("xp"))
                {
                    var charLevel = Services.ExperienceService.GetCharacterLevel(Context.User);
                    var charExp = Services.ExperienceService.GetCharacterExpPoints(user);

                    message.Append(
                        "**Level: **" + charLevel + "\n" +
                        "**Experience Points:** " + charExp + "/" + (charExp + Services.ExperienceService.GetLevelExp(charLevel)) + "\n"
                        );
                    if (character.RemainingSkillPoints > 0)
                        message.Append("**Skill Points:** " + character.RemainingSkillPoints);
                    if (character.RemainingPerkPoints > 0)
                        message.Append("**Perk Points:** " + character.RemainingPerkPoints);

                    await ReplyAsync(message.ToString());
                }
                else if (statToView.Equals("skill") || statToView.Equals("skills"))
                {
                    message.Append("**Skills:**\n");
                    foreach (var skill in character.CharSkills.skillDict)
                        message.Append("**" + skill.Key + "**" + ": " + skill.Value + "\n");
                    if (character.RemainingSkillPoints > 0)
                        message.Append("**Skill Points:** " + character.RemainingSkillPoints);
                    await ReplyAsync(message.ToString());
                }
                else if (statToView.Equals("perk") || statToView.Equals("perks"))
                {
                    message.Append("**Perks:**\n");
                    if (character.CharPerks.Count == 0)
                        message.Append("Don't have any!\n");
                    else
                        foreach (var perk in character.CharPerks)
                            message.Append(perk.Name + " ");

                    if (character.RemainingPerkPoints > 0)
                        message.Append("\n**Perk Points:** " + character.RemainingPerkPoints);

                    await ReplyAsync(message.ToString());
                }
                else
                    await ReplyAsync("Invalid option specified, valid options are: xp, skills, or perks.");
            }
            else
                await ReplyAsync("Could not find your character!");
        }
    }
}
