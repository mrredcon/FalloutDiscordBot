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
        [Command("addskill"), Ratelimit(1, 5, Measure.Seconds)]
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
        [Command("addperk"), Ratelimit(1, 10, Measure.Seconds)]
        [Summary("Add a perk rank to a given perk. Must have perk points available.")]
        public async Task AddPerk(string perkToAdd)
        {
            await ReplyAsync(Services.ExperienceService.AddPerk(Context.User, perkToAdd));
        }
        [Command("viewperks"), Ratelimit(1, 10, Measure.Seconds)]
        [Summary("View perks that you meet the requirements for.")]
        public async Task ViewPerks()
        {
            var character = Services.CharacterLoadService.LoadCharacter(Context.User);

            if (character == null)
            {
                await ReplyAsync("I couldn't find your character! " + Context.User.Mention + ".");
                return;
            }

            List<CharacterStats.Perk> potentialPerks = new List<CharacterStats.Perk>();

            foreach (var perk in Services.CharacterUtilityService.GetAllPerks())
            {
                if (character.CharPerks.Contains(perk)) // add another rank to perk instead of adding it to list
                {
                    var charPerk = character.CharPerks.Find(x => x.Name.Equals(perk.Name));
                    if (charPerk.CurrentPerkRanks < perk.PerkRanks)
                        potentialPerks.Add(perk);
                    continue;
                }
                if (perk.LevelRequirement <= Services.ExperienceService.GetCharacterLevel(character))
                {

                }
            }
        }
    }
}
