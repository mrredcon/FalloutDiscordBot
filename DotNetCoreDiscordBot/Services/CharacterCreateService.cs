using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static DotNetDiscordBot.Character;

namespace DotNetDiscordBot.Services
{
    public class CharacterCreateService
    {
        /// <summary>
        /// Writes a new Character to file in the form of a serialized XML.
        /// </summary>
        /// <param name="user">The Discord user object of the new player.</param>
        /// <param name="charName">Name of the new character</param>
        /// <param name="special">S.P.E.C.I.A.L. stat of the new character</param>
        /// <param name="skills">The initial Skills of the new player WITH 3 Tag'd Skills</param>
        /// <param name="traits">The new character's starting traits.</param>
        public void SaveNewCharacter(SocketUser user, string charName, CharacterStats.SPECIAL special,
            string skillTag1, string skillTag2, string skillTag3, List<CharacterStats.Trait> traits)
        {
            var skills = CalculateInitialSkills(special);

            if (!(Enum.TryParse(skillTag1.ToLower(), out Character.SkillEnum skillTag1Enum)))
                throw new ArgumentException("Couldn't parse given skillTag string.", "skillTag1");
            if (!(Enum.TryParse(skillTag2.ToLower(), out Character.SkillEnum skillTag2Enum)))
                throw new ArgumentException("Couldn't parse given skillTag string.", "skillTag2");
            if (!(Enum.TryParse(skillTag3.ToLower(), out Character.SkillEnum skillTag3Enum)))
                throw new ArgumentException("Couldn't parse given skillTag string.", "skillTag3");

            var newChar = new Character(user, charName, special, skills, skillTag1Enum, skillTag2Enum, skillTag3Enum, traits);

            string filePath = Character.saveLoc + user.Id + fileExtension;

            try
            {
                BinarySerialization.WriteToBinaryFile<Character>(filePath, newChar, append: false);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Failed to save new character to disk.");
                Console.WriteLine(e);
                return;
            }
        }

        /// <summary>
        /// Takes a Character's SPECIAL stat and calculates the starting value for the Character's Skills.
        /// </summary>
        /// <param name="special">A new Character's SPECIAL stat</param>
        /// <returns>The initial value of a new Character's Skills</returns>
        public CharacterStats.Skills CalculateInitialSkills(CharacterStats.SPECIAL special)
        {
            byte barter = CalculateSkill(special.Charisma, special.Luck),
            energyWeapons = CalculateSkill(special.Perception, special.Luck),
            explosives = CalculateSkill(special.Perception, special.Luck),
            guns = CalculateSkill(special.Agility, special.Luck),
            lockpick = CalculateSkill(special.Perception, special.Luck),
            medicine = CalculateSkill(special.Intelligence, special.Luck),
            meleeWeapons = CalculateSkill(special.Strength, special.Luck),
            repair = CalculateSkill(special.Intelligence, special.Luck),
            science = CalculateSkill(special.Intelligence, special.Luck),
            sneak = CalculateSkill(special.Agility, special.Luck),
            speech = CalculateSkill(special.Charisma, special.Luck),
            survival = CalculateSkill(special.Endurance, special.Luck),
            unarmed = CalculateSkill(special.Endurance, special.Luck);

            return new CharacterStats.Skills(barter, energyWeapons, explosives, guns, lockpick, medicine, meleeWeapons, repair, science, sneak, speech, survival, unarmed);
        }
        private byte CalculateSkill(byte skillSpecialStat, byte luck)
        {
            byte result = (byte)(2 + (2 * skillSpecialStat) + Math.Round(luck / 2.0));
            //Console.WriteLine(result);
            return result;
        }
    }
}
