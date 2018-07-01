using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiscordBot.Services
{
    public class ExperienceService
    {
        public static void AwardExp(SocketUser user)
        {
            var character = CharacterLoadService.LoadCharacter(user);
            int expOnMessage = int.Parse(Program._config["exp_on_message"]);

            int actualExp = (int)Math.Round(expOnMessage * (character.CharSpecial.Intelligence / 10.0 + 1));

            if (character != null)
                character.ExpPoints += actualExp;
            else
                return;

            CharacterUtilityService.OverwriteCharacter(character);
        }
        public static string AddPerk(SocketUser user, string perkToAdd)
        {
            var character = CharacterLoadService.LoadCharacter(user);
            int oldCount = character.CharPerks.Count;

            if (character == null)
            {
                return "Couldn't find character!";
            }
            if (character.RemainingPerkPoints <= 0)
            {
                return "You don't have any skill points!";
            }

            foreach (var perk in CharacterUtilityService.GetAllPerks())
            {
                if (perk.Name.ToLower().Equals(perkToAdd.ToLower())) // parameter case insensitivity
                {
                    if (character.CharPerks.Contains(perk)) // add another rank to perk instead of adding it to list
                    {
                        var charPerk = character.CharPerks.Find(x => x.Name.Equals(perk.Name));
                        if (charPerk.CurrentPerkRanks < perk.PerkRanks)
                        {
                            charPerk.CurrentPerkRanks++;
                            CharacterUtilityService.OverwriteCharacter(character);
                            return "Added a rank to the given perk!";
                        }
                        else
                        {
                            return "You already have that stat at its max rank!";
                        }
                    }
                    else
                    {
                        // TODO: actually check Skill and S.P.E.C.I.A.L. requirements
                        if (character.CharSpecial.IsGreaterThanOrEqualTo(perk.SpecialRequirement) &&
                            character.CharSkills.IsGreaterThanOrEqualTo(perk.SkillRequirement))
                        {
                            character.CharPerks.Add(perk);
                            CharacterUtilityService.OverwriteCharacter(character);
                            return "Added perk!";
                        }
                    }
                }
            }
            if (oldCount == character.CharPerks.Count) // make sure we added a perk
                return "Failed to add the given perk.  (Check spelling!)";
            else
                return "idk wtf happened bro tell someone how you got here";
        }
        public static byte GetCharacterLevel(SocketUser user)
        {
            if (CharacterUtilityService.CharacterExists(user)) // fancy way
            {
                return GetCharacterLevel(CharacterLoadService.LoadCharacter(user));
            }
            else
                return 0;
        }
        public static byte GetCharacterLevel(Character character)
        {
            if (character.ExpPoints == 0) return 1;

            int remaining_xp = character.ExpPoints;
            byte level = 0;
            while (remaining_xp >= GetLevelExp(level))
            {
                remaining_xp -= GetLevelExp(level);
                level += 1;
            }

            return level;
        }
        public static int GetCharacterExpPoints(SocketUser user)
        {
            if (CharacterUtilityService.CharacterExists(user))
            {
                return GetCharacterExpPoints(CharacterLoadService.LoadCharacter(user));
            }
            else
                return -1;
        }
        public static int GetCharacterExpPoints(Character character)
        {
            return character.ExpPoints;
        }
        public static int GetLevelExp(int level)
        {
            int result = 5 * ((int)Math.Pow(level, 2)) + 50 * level + 100; // shamelessly stolen from Mee6
            return result;
        }
        public static void AddSkillPoints(Character character)
        {
            byte intelligence = character.CharSpecial.Intelligence;
            byte level = GetCharacterLevel(character);
            byte skillPoints = 0;

            if (intelligence % 2 == 1)
                if (level % 2 == 0)
                    skillPoints++;

            skillPoints += (byte)(10 + (intelligence / 2));

            character.RemainingSkillPoints += skillPoints;
        }
        public static async Task CharacterLevelUp(SocketUser user)
        {
            var dmChannel = await user.GetOrCreateDMChannelAsync();

            var character = CharacterLoadService.LoadCharacter(user);

            AddSkillPoints(character); // that doesn't seem right...I should probably move that to Character

            var cPre = CommandHandlerService.cmd_prefix;

            await dmChannel.SendMessageAsync("You're now level " + GetCharacterLevel(character) + ", and have " + character.RemainingSkillPoints + " skill points remaining to add. " +
                "You have " + character.RemainingPerkPoints + " remaining perk points.  " +
                "Use " + cPre + "addskill to add skill points, and " + cPre + "addperk to add perks.  Use " + cPre + "viewskills to look at skill names.");
            await Task.Delay(1000);
            CharacterUtilityService.OverwriteCharacter(character);
        }
        //public static int GetRemainingExp(SocketUser user)
        //{
        //    var character = CharacterLoadService.LoadCharacter(user); // less fancy way
        //    if (character != null)
        //        return GetRemainingExp(character);
        //    else
        //        return -1;
        //}
        //public static int GetRemainingExp(Character character)
        //{
        //    if (GetCharacterLevel(character) == 1) return 100;

        //    int x = 0;
        //    for (int level = 0; level < GetCharacterLevel(character); level++)
        //    {
        //        x += GetLevelExp(level);
        //    }
        //    return character.ExpPoints - x;
        //}
    }
}
