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
            
            int difference = skillAmount - rngResult;
            // compares your roll with your skills, and how much better you did than the bare minimum
            double successPercent = (double)difference / skillAmount;
            successPercent = Math.Round(successPercent, 2) * 100;
            // says much you failed in percent (sometimes gets to 300% and higher o.o )
            double failurePercent = (double)rngResult / skillAmount;
            failurePercent = Math.Round(failurePercent, 2) * 100;

            string result = "";

            //Console.WriteLine(skillAmount + " " + rngResult + " " + difference + " " + successPercent + " " + failurePercent);

            if (rngResult <= skillAmount)
            {
                if (successPercent >= 90)
                {
                    // criticaler success (holy shit)
                    result = "**CRITICAL " + skill.ToString().ToUpper() + " SUCCESS!!!**";
                }
                if (successPercent >= 80)
                {
                    // critical success
                    result = "**CRITICAL " + skill.ToString().ToUpper() + " SUCCESS!**";
                }
                else if (successPercent >= 60)
                {
                    // purty good (great) success
                    result = "__GREAT " + skill.ToString().ToUpper() + " SUCCESS__";
                }
                else if (successPercent >= 40)
                {
                    // good success
                    result = "*Very good " + skill.ToString() + " success*";
                }
                else if (successPercent >= 25)
                {
                    // decent
                    result = "*Good " + skill.ToString() + " success*";
                }
                else if (successPercent >= 10)
                {
                    // decent
                    result = "*Above average " + skill.ToString() + " success*";
                }
                else
                {
                    // close call!
                    result = "__***CLOSE CALL! " + skill.ToString() + " success***__";
                }
                result += " for " + user.Username + ": did " + successPercent + "% better than needed!";
            }
            else
            {
                //// TODO: fix this completely its broken as hell
                //differencePercentage = Math.Round(differencePercentage, 2);
                //differencePercentage *= -1;

                if (failurePercent >= 90)
                {
                    // criticaler failure (holy shit
                    result = "**CRITICAL " + skill.ToString().ToUpper() + " FAILURE!!!**";
                }
                if (failurePercent >= 80)
                {
                    // critical failure
                    result = "**CRITICAL " + skill.ToString().ToUpper() + " FAILURE!**";
                }
                else if (failurePercent >= 60)
                {
                    // purty good (great) failure
                    result = "__GREAT " + skill.ToString().ToUpper() + " FAILURE__";
                }
                else if (failurePercent >= 40)
                {
                    // good failure
                    result = "*Very good " + skill.ToString() + " failure*";
                }
                else if (failurePercent >= 25)
                {
                    // decent
                    result = "*Good " + skill.ToString() + " failure*";
                }
                else if (failurePercent >= 10)
                {
                    // decent
                    result = "*Above average " + skill.ToString() + " failure*";
                }
                else
                {
                    // close call!
                    result = "__***Heartbreaking " + skill.ToString() + " failure***__";
                }
                result += " for " + user.Username + ": did **" + failurePercent + "%** worse than needed!";
            }

            return result;
        }
        public string GetRollResult(Character.SpecialEnum rollSpecial, SocketUser user)
        {
            Random rand = new Random();
            //var skills = CharacterUtilityService.GetCharacterSkills(user);
            var charSpecial = CharacterUtilityService.GetCharacterSpecial(user);
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

            string result = "";

            //Console.WriteLine(specialAmount + " " + rngResult + " " + difference + " " + successPercent + " " + failurePercent);

            if (rngResult <= specialAmount)
            {
                if (successPercent >= 90)
                {
                    // criticaler success (holy shit)
                    result = "**CRITICAL " + rollSpecial.ToString().ToUpper() + " SUCCESS!!!**";
                }
                if (successPercent >= 80)
                {
                    // critical success
                    result = "**CRITICAL " + rollSpecial.ToString().ToUpper() + " SUCCESS!**";
                }
                else if (successPercent >= 60)
                {
                    // purty good (great) success
                    result = "__GREAT " + rollSpecial.ToString().ToUpper() + " SUCCESS__";
                }
                else if (successPercent >= 40)
                {
                    // good success
                    result = "*Very good " + rollSpecial.ToString() + " success*";
                }
                else if (successPercent >= 25)
                {
                    // decent
                    result = "*Good " + rollSpecial.ToString() + " success*";
                }
                else if (successPercent >= 10)
                {
                    // decent
                    result = "*Above average " + rollSpecial.ToString() + " success*";
                }
                else
                {
                    // close call!
                    result = "__***CLOSE CALL! " + rollSpecial.ToString() + " success***__";
                }
                result += " for " + user.Username + ": did " + successPercent + "% better than needed!";
            }
            else
            {
                //// TODO: fix this completely its broken as hell
                //differencePercentage = Math.Round(differencePercentage, 2);
                //differencePercentage *= -1;

                if (failurePercent >= 90)
                {
                    // criticaler failure (holy shit
                    result = "**CRITICAL " + rollSpecial.ToString().ToUpper() + " FAILURE!!!**";
                }
                if (failurePercent >= 80)
                {
                    // critical failure
                    result = "**CRITICAL " + rollSpecial.ToString().ToUpper() + " FAILURE!**";
                }
                else if (failurePercent >= 60)
                {
                    // purty good (great) failure
                    result = "__GREAT " + rollSpecial.ToString().ToUpper() + " FAILURE__";
                }
                else if (failurePercent >= 40)
                {
                    // good failure
                    result = "*Very good " + rollSpecial.ToString() + " failure*";
                }
                else if (failurePercent >= 25)
                {
                    // decent
                    result = "*Good " + rollSpecial.ToString() + " failure*";
                }
                else if (failurePercent >= 10)
                {
                    // decent
                    result = "*Above average " + rollSpecial.ToString() + " failure*";
                }
                else
                {
                    // close call!
                    result = "__***Heartbreaking " + rollSpecial.ToString() + " failure***__";
                }
                result += " for " + user.Username + ": did **" + failurePercent + "%** worse than needed!";
            }

            return result;
        }
    }
}
