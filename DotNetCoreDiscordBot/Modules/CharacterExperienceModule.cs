using Discord.Commands;
using Discord.Addons.Preconditions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiscordBot.Modules
{
    public class CharacterExperienceModule : ModuleBase<SocketCommandContext>
    {
        [Command("addskill"), Ratelimit(1, 0.833, Measure.Minutes)]
        [Summary("Add skill points to a given skill. Must have skill points available.")]
        public async Task AddSkillPoints(string skill, int points)
        {
            var character = Services.CharacterLoadService.LoadCharacter(Context.User);
            if (character == null)
            {
                await ReplyAsync("I couldn't find your character!");
                return;
            }
            if (character.CharSkills.skillDict.ContainsKey(skill))
            {
                if ((character.CharSkills.skillDict[skill] += (byte)points) >= 100)
                {
                    await ReplyAsync("That skill is already maxed out!");
                }
                if (points <= character.RemainingSkillPoints)
                {
                    character.CharSkills.skillDict[skill] += (byte)points;
                    character.RemainingSkillPoints -= points;
                    await ReplyAsync("Added " + points + " points to " + skill + ".");
                    Services.CharacterUtilityService.OverwriteCharacter(character);
                }
                else
                    await ReplyAsync("You don't have enough skill points available to do that!");
            }
        }
        [Command("addperk"), Ratelimit(1, 0.167, Measure.Minutes)]
        [Summary("Add perk points to a given skill. Must have skill points available.")]
        public async Task AddPerks(string perkToAdd, int points)
        {
            var character = Services.CharacterLoadService.LoadCharacter(Context.User);
            int oldCount = character.CharPerks.Count;

            if (character == null)
            {
                await ReplyAsync("I couldn't find your character!" + Context.User.Mention + ".");
                return;
            }
            if (character.RemainingPerkPoints <= 0)
            {
                await ReplyAsync("You don't have any perk points, " + Context.User.Mention + ".");
                return;
            }

            foreach (var perk in Services.CharacterUtilityService.GetAllPerks())
                if (perk.Name.ToLower().Equals(perkToAdd.ToLower())) // parameter case insensitivity
                {
                    if (character.CharPerks.Contains(perk)) // add another rank to perk instead of adding it to list
                    {
                        var charPerk = character.CharPerks.Find(x => x.Name.Equals(perk.Name));
                        if (charPerk.CurrentPerkRanks < perk.PerkRanks)
                        {
                            charPerk.CurrentPerkRanks++;
                            Services.CharacterUtilityService.OverwriteCharacter(character);
                            await ReplyAsync("Added a rank to given perk!");
                            return;
                        }
                        else
                        {
                            await ReplyAsync("You already have that perk at its max rank!");
                        }
                    }
                    else
                    {
                        character.CharPerks.Add(perk);
                        Services.CharacterUtilityService.OverwriteCharacter(character);
                        await ReplyAsync("Added perk!");
                        return;
                    }
                }
            if (oldCount == character.CharPerks.Count) // make sure we added a perk
                await ReplyAsync("Failed to add the given perk.  (Check spelling!)");
        }
    }
}
