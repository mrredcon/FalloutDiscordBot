using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiscordBot.Modules
{
    public class CharacterCreateModule : ModuleBase<SocketCommandContext>
    {
        public Services.CharacterCreateService CharCreateService { get; set; }
        char cPre = Services.CommandHandlerService.cmd_prefix;

        [Command("createchar")]
        [Summary("Creates a new character for Fallout RP")]
        public async Task CreateCharacterAsync()
        {
            var user = Context.User;

            Discord.IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();

            // Check to make sure that the user is in a server before telling them "I'm going to DM you" (Since they might already be in a DM w/bot)
            if (Context.Message.Channel as SocketGuildChannel != null)
                await Context.Channel.SendMessageAsync(user.Mention + " " + "I'm going to DM you some instructions on how to create your character!");

            await dmChannel.SendMessageAsync("Okay, the command for creating your character is a little complex, and a bit long.\n\n" +
            "The create_char command must have at least 5 *arguments*, and at most, 7 (Traits are optional, and you can just have 1 if you want).\n" +
                "The command treats every word seperated by a space as an *argument*, so be careful if you want to have a name with more than one word. " +
                "If you want to have a name with a space, wrap in in double quotes ex: \"Randy Smith\"\n\n" +
                // TODO: implement a config file so I can cleanly put in the prefix, instead of having to type out Services.CommandHandlerService.cmd_prefix
                "The order of *arguments* go as follows: **Name S.P.E.C.I.A.L. (Skill to Tag #1) (Skill to Tag #2) (Skill to Tag #3) [Trait #1] [Trait #2]**\n" +
                "Here's an example of the complete command: " + cPre + "createchar Rick XXX2233 Repair Speech Guns ww fe\n" +
                "The above command would create a character with 10 Strength, 10 Perception, 10 Endurance, 2 Charisma, 2 Intelligence, 3 Agility, and 3 Luck, " +
                "with the tagged skills Repair, Speech, and Guns, and with the traits Wild Wasteland and Four Eyes.\n\n" +
                "To get a list of what to write for your S.P.E.C.I.A.L., Skills to Tag, Traits, type "+cPre+"statshelp");
        }
        [Command("createchar")]
        [Summary("Creates a new character for Fallout RP")]
        public async Task CreateCharacterAsync(string name, string special, string skillTag1, string skillTag2, string skillTag3)
        {
            
            if (Services.CharacterUtilityService.CharacterExists(Context.User))
            {
                await ReplyAsync(Context.User.Mention + 
                    " I've found that a character already exists with your Discord Account! To create a new one, you must first delete this one with " + cPre + "deletechar.");
                return;
            }
                
            try
            {
                CharCreateService.SaveNewCharacter(Context.User, name, new CharacterStats.SPECIAL(special), skillTag1, skillTag2, skillTag3, null);
            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync("Failed to create character, here's the exception: " + e.ToString());
                return;
            }
            if (Services.CharacterUtilityService.CharacterExists(Context.User))
                await Context.Channel.SendMessageAsync("Character (probably) created successfully!");
            else
                await Context.Channel.SendMessageAsync("I couldn't find your new character file...a problem has occured.  Bot hoster: check the console.");
        }
        [Command("createchar")]
        [Summary("Creates a new character for Fallout RP")]
        public async Task CreateCharacterAsync(string name, string special, string skillTag1, string skillTag2, string skillTag3, string trait1)
        {
            if (Services.CharacterUtilityService.CharacterExists(Context.User))
            {
                await ReplyAsync(Context.User.Mention +
                    " I've found that a character already exists with your Discord Account! To create a new one, you must first delete this one with " + cPre + "deletechar.");
                return;
            }

            List<CharacterStats.Trait> charTraits = new List<CharacterStats.Trait>();
            List<CharacterStats.Trait> traitList = Services.CharacterUtilityService.GetAllTraits();

            string trait1Lower = trait1.ToLower();

            // populate charTraits
            foreach (var trait in traitList)
            {
                string traitName = trait.Name.ToLower();
                string traitAbrev = trait.Abreviation;

                if (trait1Lower.Equals(traitName) || trait1Lower.Equals(traitAbrev))
                    charTraits.Add(trait);
            }

            try
            {
                CharCreateService.SaveNewCharacter(Context.User, name, new CharacterStats.SPECIAL(special), skillTag1, skillTag2, skillTag3, traitList);
            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync("Failed to create character, here's the exception: " + e.ToString());
                return;
            }
            if (Services.CharacterUtilityService.CharacterExists(Context.User))
                await Context.Channel.SendMessageAsync("Character (probably) created successfully!");
            else
                await Context.Channel.SendMessageAsync("I couldn't find your new character file...a problem has occured.  Bot hoster: check the console.");
        }
        [Command("createchar")]
        [Summary("Creates a new character for Fallout RP")]
        public async Task CreateCharacterAsync(string name, string special, string skillTag1, string skillTag2, string skillTag3, string trait1, string trait2)
        {
            if (Services.CharacterUtilityService.CharacterExists(Context.User))
            {
                await ReplyAsync(Context.User.Mention +
                    " I've found that a character already exists with your Discord Account! To create a new one, you must first delete this one with !deletechar.");
                return;
            }

            List<CharacterStats.Trait> charTraits = new List<CharacterStats.Trait>();
            List<CharacterStats.Trait> traitList = Services.CharacterUtilityService.GetAllTraits();

            string trait1Lower = trait1.ToLower();
            string trait2Lower = trait2.ToLower();

            // populate traitList
            foreach (var trait in traitList)
            {
                string traitName = trait.Name.ToLower();
                string traitAbrev = trait.Abreviation;

                if (trait1Lower.Equals(traitName) || trait2Lower.Equals(traitName) || 
                    trait1Lower.Equals(traitAbrev) || trait2Lower.Equals(traitAbrev))
                    charTraits.Add(trait);
            }

            try
            {
                CharCreateService.SaveNewCharacter(Context.User, name, new CharacterStats.SPECIAL(special), skillTag1, skillTag2, skillTag3, traitList);
            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync("Failed to create character, here's the exception: " + e.ToString());
                return;
            }
            if (Services.CharacterUtilityService.CharacterExists(Context.User))
                await Context.Channel.SendMessageAsync("Character (probably) created successfully!");
            else
                await Context.Channel.SendMessageAsync("I couldn't find your new character file...a problem has occured.  Bot hoster: check the console.");
        }
        [Command("statshelp")]
        [Summary("Sends the caller via DM info on how to build their character.")]
        public async Task SendStatHelpAsync()
        {
            CharacterStats.Skills skills = new CharacterStats.Skills();

            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            StringBuilder statsBuilder = new StringBuilder();

            statsBuilder.Append("**Skills:** ");

            // Reads the Skills dictionary in CharacterStats, and builds a string with it.
            foreach (var skill in skills.skillDict.Keys)
                statsBuilder.Append(skill + ", ");

            statsBuilder.Append("\n\n**Traits:** \n\n");

            foreach (var trait in Services.CharacterUtilityService.GetAllTraits())
                statsBuilder.Append("*" + trait.Name + "*" + ": " + trait.Description + "       " + "(" + trait.Abreviation + ")" + "\n");

            statsBuilder.Append("\nThe abreviations in parentheses can be used during character creation.");

            statsBuilder.Append("\n\n**S.P.E.C.I.A.L. Instructions:** ");
            statsBuilder.Append("Write out your character's S.P.E.C.I.A.L. stat values in numbers starting from Strength and ending with Luck. X signifies 10.\n\n" +
                "An example would be XXX2224, which would set that character's S.P.E.C.I.A.L. to " +
                "10 Strength, 10 Perception, 10 Endurance, 2 Charisma, 2 Intelligence, 2 Agility, and 4 Luck.\n\n" +
                "A S.P.E.C.I.A.L. must equal exactly 40 when added together; no more, no less.");

            await dmChannel.SendMessageAsync(statsBuilder.ToString());
        }
    }
}
