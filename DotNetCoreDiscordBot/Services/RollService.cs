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
            var skills = CharacterUtilityService.GetCharacterSkills(user);
            var special = CharacterUtilityService.GetCharacterSpecial(user);
            if (skills == null || special == null) return null;

            // RNG influenced by character luck except when its 5
            int rngResult = (int)Math.Round((rand.Next(1, 101) * (special.Luck / 10.0 - .5 + 1.0))),
                skillAmount = skills.skillDict[skill.ToString()];
            int difference = skillAmount - rngResult;
            double differencePercentage = difference / skillAmount * 100;

            string result = "";

            if (rngResult <= skillAmount)
            {
                differencePercentage = Math.Round(differencePercentage, 2);

                if (differencePercentage >= 90)
                {
                    // criticaler success (holy shit)
                    result = "**CRITICAL " + skill.ToString().ToUpper() + " SUCCESS!!!**";
                }
                if (differencePercentage >= 80)
                {
                    // critical success
                    result = "**CRITICAL " + skill.ToString().ToUpper() + " SUCCESS!**";
                }
                else if (differencePercentage >= 60)
                {
                    // purty good (great) success
                    result = "__GREAT " + skill.ToString().ToUpper() + " SUCCESS__";
                }
                else if (differencePercentage >= 40)
                {
                    // good success
                    result = "*Very good " + skill.ToString() + " success*";
                }
                else if (differencePercentage >= 25)
                {
                    // decent
                    result = "*Good " + skill.ToString() + " success*";
                }
                else if (differencePercentage >= 10)
                {
                    // decent
                    result = "*Above average " + skill.ToString() + " success*";
                }
                else
                {
                    // close call!
                    result = "__***CLOSE CALL!***___ " + skill.ToString() + " success";
                }
                result += " for " + user.Username + ": did " + differencePercentage + "% better than needed!";
            }
            else
            {
                differencePercentage = Math.Round(differencePercentage, 2);
                differencePercentage *= -1;

                if (differencePercentage >= 90)
                {
                    // criticaler failure (holy shit
                    result = "**CRITICAL " + skill.ToString().ToUpper() + " FAILURE!!!**";
                }
                if (differencePercentage >= 80)
                {
                    // critical failure
                    result = "**CRITICAL " + skill.ToString().ToUpper() + " FAILURE!**";
                }
                else if (differencePercentage >= 60)
                {
                    // purty good (great) failure
                    result = "__GREAT " + skill.ToString().ToUpper() + " FAILURE__";
                }
                else if (differencePercentage >= 40)
                {
                    // good failure
                    result = "*Very good " + skill.ToString() + " failure*";
                }
                else if (differencePercentage >= 25)
                {
                    // decent
                    result = "*Good " + skill.ToString() + " failure*";
                }
                else if (differencePercentage >= 10)
                {
                    // decent
                    result = "*Above average " + skill.ToString() + " failure*";
                }
                else
                {
                    // close call!
                    result = "__***Heartbreaking***___ " + skill.ToString() + " failure";
                }
                result += " for " + user.Username + ": did " + differencePercentage + "% worse than needed!";
            }

            return result;
        }
    }
}
