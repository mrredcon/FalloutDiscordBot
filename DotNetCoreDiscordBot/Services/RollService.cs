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
            if (skills == null) return null;

            // RNG influenced by character luck except when its 5
            int rngResult = (int)Math.Round((rand.Next(1, 101) * (special.Luck / 10.0 - .5 + 1.0))),
                skillAmount = skills.skillDict[skill.ToString()];
            
            // compares your roll with your skills, and how much better you did than the bare minimum
            double successPercent = (double)(skillAmount - rngResult) / skillAmount;
            successPercent = Math.Round(successPercent, 2) * 100;
            // says much you failed in percent (sometimes gets to 300% and higher o-o )
            double failurePercent = (double)rngResult / skillAmount - 1;
            failurePercent = Math.Round(failurePercent, 2) * 100;

            StringBuilder result = new StringBuilder();

            if (rngResult <= skillAmount)
            {
                if (successPercent >= 90)
                {
                    // criticaler success (holy shit)
                    result.Append("**CRITICAL " + skill.ToString().ToUpper() + " SUCCESS!!!**");
                }
                else if (successPercent >= 80)
                {
                    // critical success
                    result.Append("**CRITICAL " + skill.ToString().ToUpper() + " SUCCESS!**");
                }
                else if (successPercent >= 60)
                {
                    // purty good (great) success
                    result.Append("__GREAT " + skill.ToString().ToUpper() + " SUCCESS__");
                }
                else if (successPercent >= 40)
                {
                    // good success
                    result.Append("*Very good " + skill.ToString() + " success*");
                }
                else if (successPercent >= 25)
                {
                    // decent
                    result.Append("*Good " + skill.ToString() + " success*");
                }
                else if (successPercent >= 10)
                {
                    // decent
                    result.Append("*Above average " + skill.ToString() + " success*");
                }
                else
                {
                    // close call!
                    result.Append("__***CLOSE CALL! " + skill.ToString() + " success***__");
                }
                result.Append(" for " + user.Username + ": did **" + successPercent + "%** better than needed!");
            }
            else
            {
                if (failurePercent >= 90)
                {
                    // criticaler failure (holy shit
                    result.Append("**CRITICAL " + skill.ToString().ToUpper() + " FAILURE!!!**");
                }
                else if (failurePercent >= 80)
                {
                    // critical failure
                    result.Append("**CRITICAL " + skill.ToString().ToUpper() + " FAILURE!**");
                }
                else if (failurePercent >= 60)
                {
                    // purty good (great) failure
                    result.Append("__TERRIBLE " + skill.ToString().ToUpper() + " FAILURE__");
                }
                else if (failurePercent >= 40)
                {
                    // good failure
                    result.Append("*Pretty bad " + skill.ToString() + " failure*");
                }
                else if (failurePercent >= 25)
                {
                    // decent
                    result.Append("*Bad " + skill.ToString() + " failure*");
                }
                else if (failurePercent >= 10)
                {
                    // decent
                    result.Append("*Above average " + skill.ToString() + " failure*");
                }
                else
                {
                    // close call!
                    result.Append("__***Heartbreaking " + skill.ToString() + " failure***__");
                }
                result.Append(" for " + user.Username + ": did **" + failurePercent + "%** worse than needed!");
            }

            return result.ToString();
        }
        public string GetRollResult(Character.SpecialEnum rollSpecial, SocketUser user)
        {
            Random rand = new Random();
            //var skills = CharacterUtilityService.GetCharacterSkills(user);
            var charSpecial = CharacterUtilityService.GetCharacterSpecial(user);
            var charTraits = CharacterUtilityService.GetCharacterTraits(user);
            //if (charSpecial == null) return null;

            // RNG influenced by character luck except when its 5
            int rngResult = (int)Math.Round((rand.Next(1, 11) * (charSpecial.Luck / 10.0 - .5 + 1.0))),
                specialAmount = (byte)typeof(CharacterStats.SPECIAL).GetProperty(rollSpecial.ToString()).GetValue(charSpecial);

            int difference = specialAmount - rngResult;
            // compares your roll with your skills, and how much better you did than the bare minimum
            double successPercent = (double)difference / specialAmount;
            successPercent = Math.Round(successPercent, 2) * 100;
            // says much you failed in percent (sometimes gets to 300% and higher o.o )
            double failurePercent = (double)rngResult / specialAmount;
            failurePercent = Math.Round(failurePercent, 2) * 100;

            StringBuilder result = new StringBuilder();

            if (rngResult <= specialAmount)
            {
                if (successPercent >= 90)
                {
                    // criticaler success (holy shit)
                    result.Append("**CRITICAL " + rollSpecial.ToString().ToUpper() + " SUCCESS!!!**");
                }
                else if (successPercent >= 80)
                {
                    // critical success
                    result.Append("**CRITICAL " + rollSpecial.ToString().ToUpper() + " SUCCESS!**");
                }
                else if (successPercent >= 60)
                {
                    // purty good (great) success
                    result.Append("__GREAT " + rollSpecial.ToString().ToUpper() + " SUCCESS__");
                }
                else if (successPercent >= 40)
                {
                    // good success
                    result.Append("*Very good " + rollSpecial.ToString() + " success*");
                }
                else if (successPercent >= 25)
                {
                    // decent
                    result.Append("*Good " + rollSpecial.ToString() + " success*");
                }
                else if (successPercent >= 10)
                {
                    // decent
                    result.Append("*Above average " + rollSpecial.ToString() + " success*");
                }
                else
                {
                    // close call!
                    result.Append("__***CLOSE CALL! " + rollSpecial.ToString() + " success***__");
                }
                result.Append(" for " + user.Username + ": did **" + successPercent + "%** better than needed!");
            }
            else
            {
                if (failurePercent >= 90)
                {
                    // criticaler failure (holy shit
                    result.Append("**CRITICAL " + rollSpecial.ToString().ToUpper() + " FAILURE!!!**");
                }
                else if (failurePercent >= 80)
                {
                    // critical failure
                    result.Append("**CRITICAL " + rollSpecial.ToString().ToUpper() + " FAILURE!**");
                }
                else if (failurePercent >= 60)
                {
                    // purty good (great) failure
                    result.Append("__GREAT " + rollSpecial.ToString().ToUpper() + " FAILURE__");
                }
                else if (failurePercent >= 40)
                {
                    // good failure
                    result.Append("*Very good " + rollSpecial.ToString() + " failure*");
                }
                else if (failurePercent >= 25)
                {
                    // decent
                    result.Append("*Good " + rollSpecial.ToString() + " failure*");
                }
                else if (failurePercent >= 10)
                {
                    // decent
                    result.Append("*Above average " + rollSpecial.ToString() + " failure*");
                }
                else
                {
                    // close call!
                    result.Append("__***Heartbreaking " + rollSpecial.ToString() + " failure***__");
                }
                result.Append(" for " + user.Username + ": did **" + failurePercent + "%** worse than needed!");
                if (rollSpecial.Equals(Character.SpecialEnum.Agility) && charTraits.Contains(new CharacterStats.Trait.SmallFrame()))
                    result.Append("\nCharacter has a **Small Frame!**");
            }

            return result.ToString();
        }
    }
}
