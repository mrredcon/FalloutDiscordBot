using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiscordBot
{
    public class CharacterStats
    {
        [Serializable]
        public class SPECIAL
        {
            // TODO: maybe convert to dictionary like I did with Skills
            public byte Strength { get; }
            public byte Perception { get; }
            public byte Endurance { get; }
            public byte Charisma { get; }
            public byte Intelligence { get; }
            public byte Agility { get; }
            public byte Luck { get; }

            public SPECIAL(byte str, byte per, byte end, byte cha, byte @int, byte agl, byte luc)
            {
                // Check to make sure all S.P.E.C.I.A.L. stats are in the range of 1-10, otherwise throw an exception
                if ((str >= 1 && str <= 10) && (per >= 1 && per <= 10) && (end >= 1 && end <= 10) && (cha >= 1 && cha <= 10) && (@int >= 1 && @int <= 10)
                    && (agl >= 1 && agl <= 10) && (luc >= 1 && luc <= 10))
                {
                    Strength = str;
                    Perception = per;
                    Endurance = end;
                    Charisma = cha;
                    Intelligence = @int;
                    Agility = agl;
                    Luck = luc;
                }
                else
                    throw new ArgumentOutOfRangeException("SPECIAL constructor parameters", "One or more S.P.E.C.I.A.L. parameters were out of the range of 1-10 inclusive.");
            }
            // Attempts to turn a string into a constructed SPECIAL
            public SPECIAL(string special)
            {
                if (special.Length != 7)
                    throw new ArgumentException("Given string was too long!");

                byte[] specialVals = new byte[7];
                byte temp;
                for(int stat = 0; stat < 7; stat++)
                {
                    if (byte.TryParse(special[stat].ToString(), out temp))
                        specialVals[stat] = temp;
                    else if (special[stat] == 'X')
                        specialVals[stat] = 10;
                    else
                        throw new ArgumentException("Given string contained an unparsable character! (Character wasn't 0-9 or 'X'.)");
                }
                temp = 0;
                foreach (var specialVal in specialVals)
                    temp += specialVal;

                if (temp != 40)
                    throw new ArgumentException("Given SPECIAL did not total to 40!");

                Strength = specialVals[0]; Perception = specialVals[1]; Endurance = specialVals[2]; Charisma = specialVals[3];
                Intelligence = specialVals[4]; Agility = specialVals[5]; Luck = specialVals[6];
            }
        }
        [Serializable]
        public class Skills
        {
            public Dictionary<String, byte> skillDict;

            public Skills()
            {
                skillDict = new Dictionary<String, byte>(StringComparer.InvariantCultureIgnoreCase)
                {
                    {"Barter", 1 },
                    {"EnergyWeapons", 1 },
                    {"Explosives", 1 },
                    {"Guns", 1 },
                    {"Lockpick", 1 },
                    {"Medicine", 1 },
                    {"MeleeWeapons", 1 },
                    {"Repair", 1 },
                    {"Science", 1 },
                    {"Sneak", 1 },
                    {"Speech", 1 },
                    {"Survival", 1 },
                    {"Unarmed", 1 }
                };
            }

            public Skills(byte barter, byte energyWeapons, byte explosives, byte guns, byte lockpick, byte medicine, byte meleeWeapons,
                byte repair, byte science, byte sneak, byte speech, byte survival, byte unarmed)
            {
                skillDict = new Dictionary<String, byte>(StringComparer.InvariantCultureIgnoreCase)
                {
                    {"Barter", barter },
                    {"EnergyWeapons", energyWeapons },
                    {"Explosives", explosives },
                    {"Guns", guns },
                    {"Lockpick", lockpick },
                    {"Medicine", medicine },
                    {"MeleeWeapons", meleeWeapons },
                    {"Repair", repair },
                    {"Science", science },
                    {"Sneak", sneak },
                    {"Speech", speech },
                    {"Survival", survival },
                    {"Unarmed", unarmed }
                };
                foreach (var item in skillDict)
                    if (!CheckRange(item.Value)) // if its not in range...
                        throw new ArgumentOutOfRangeException(item.Key, "Skill was not in range from 1-100 inclusive.");
            }
            private bool CheckRange(byte skillValue)
            {
                if (skillValue >= 1 && skillValue <= 100)
                    return true;
                else
                    return false;
            }
        }
        [Serializable]
        public abstract class Trait
        {
            public abstract string Name { get; }
            public abstract string Description { get; }
            public abstract string Abreviation { get; }
            [Serializable]
            public class BuiltToDestroy : Trait
            {
                public override string Name => "Built to Destroy";
                public override string Description => "+3% crit hit chance (when doing a Guns roll, if they pass it, factor in BtD?).";
                public override string Abreviation => "bd";
            }
            [Serializable]
            public class FastShot : Trait
            {
                public override string Name => "Fast Shot";
                public override string Description => "// TODO: -20 Ranged Weapons Skills, but get to roll twice?";
                public override string Abreviation => "fs";
            }
            [Serializable]
            public class FourEyes : Trait
            {
                public override string Name => "Four Eyes";
                public override string Description => "Don't know what it does yet.  Maybe increases your PER +1, but reduces your ranged weapon rolls?";
                public override string Abreviation => "fe";
            }
            [Serializable]
            public class GoodNatured : Trait
            {
                public override string Name => "Good Natured";
                public override string Description => "increase Speech, Medicine, Repair, Science, Barter +5, decreases Energy Weapons, Explosives, Guns, Melee Weapons and Unarmed skills -5.";
                public override string Abreviation => "gn";
            }
            [Serializable]
            public class HeavyHanded : Trait
            {
                public override string Name => "Heavy Handed";
                public override string Description => "Melee and unarmed do 20% more damage. Melee and unarmed do 60% less critical hit damage. (Maybe never make melee crit??)";
                public override string Abreviation => "hh";
            }
            [Serializable]
            public class LooseCannon : Trait
            {
                public override string Name => "Loose Cannon";
                public override string Description => "Don't know what it does yet.";
                public override string Abreviation => "lc";
            }
            [Serializable]
            public class SmallFrame : Trait
            {
                public override string Name => "Small Frame";
                public override string Description => "+1 agility, +25% limb damage (inform players when failing an agility check)";
                public override string Abreviation => "sf";
            }
            [Serializable]
            public class Skilled : Trait
            {
                public override string Name => "Built to Destroy";
                public override string Description => "+5 to every skill, -10% exp gained";
                public override string Abreviation => "sk";
            }
            [Serializable]
            public class TriggerDiscipline : Trait
            {
                public override string Name => "Trigger Discipline";
                public override string Description => "+20 ranged weapon skills, (no crits?) (Or maybe can only do one Guns check per battle?";
                public override string Abreviation => "td";
            }
            [Serializable]
            public class WildWasteland : Trait
            {
                public override string Name => "Wild Wasteland";
                public override string Description => "Someone'll figure out wwhat this'll do.";
                public override string Abreviation => "ww";
            }
        }
        [Serializable]
        public abstract class Perk
        {
            public abstract string Name { get; }
            public abstract string Description { get; }
            public abstract byte LevelRequirement { get; }
            public abstract CharacterStats.SPECIAL SpecialRequirement { get; }
            public abstract CharacterStats.Skills SkillRequirement { get; }
            public abstract byte PerkRanks { get; }
            public byte CurrentPerkRanks { get; set; }

            public class RapidReload : Perk
            {
                public override string Name => "Rapid Reload";
                public override string Description => "Not really sure what it does at the moment.";
                public override byte LevelRequirement => 2;
                public override CharacterStats.SPECIAL SpecialRequirement => new CharacterStats.SPECIAL(1, 1, 1, 1, 1, 5, 1); // 5 agility
                public override CharacterStats.Skills SkillRequirement => new CharacterStats.Skills(1, 1, 1, 30, 1, 1, 1, 1, 1, 1, 1, 1, 1); // 30 guns
                public override byte PerkRanks => 1;
            }
        }
    }
    [Serializable]
    public class Character
    {
        public ulong DiscordId { get; set; }
        public string Name { get; set; }
        public int ExpPoints { get; set; }
        public CharacterStats.SPECIAL CharSpecial { get; set; }
        public CharacterStats.Skills CharSkills { get; set; }
        public int RemainingSkillPoints { get; set; }
        public byte RemainingPerkPoints { get; set; }
        public List<CharacterStats.Trait> CharTraits { get; set; }
        public List<CharacterStats.Perk> CharPerks { get; set; }
        public static readonly string saveLoc = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"DotNetCoreDiscordBot\Characters\");
        public static readonly string fileExtension = ".sav";

        public Character(SocketUser user, string charName, CharacterStats.SPECIAL special, CharacterStats.Skills charSkills, 
            SkillEnum skillTag1, SkillEnum skillTag2, SkillEnum skillTag3, List<CharacterStats.Trait> traits)
        {
            if (skillTag1.Equals(skillTag2) || skillTag1.Equals(skillTag3) || skillTag2.Equals(skillTag3))
                throw new ArgumentException("All tagged skills must be unique.");

            // Adds 15 to each tagged skill
            foreach (var skill in charSkills.skillDict.ToList())
            {
                string skillName = skill.Key.ToLower();
                if (skillName == skillTag1.ToString() || skillName == skillTag2.ToString() || skillName == skillTag3.ToString())
                    charSkills.skillDict[skill.Key] += 15;
            }

            DiscordId = user.Id;
            Name = charName;
            CharSpecial = special;
            CharSkills = charSkills;
            if (traits != null)
                CharTraits = traits;
            else
                CharTraits = new List<CharacterStats.Trait>();
            CharPerks = new List<CharacterStats.Perk>();
            RemainingSkillPoints = 0;
            ExpPoints = 0;
        }
        public enum SkillEnum
        {
            barter, energyweapons, explosives, guns, lockpick, medicine, meleeweapons, repair, science, sneak, speech, survival, unarmed
        }
    }
}
