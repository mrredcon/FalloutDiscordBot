using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiscordBot.Services
{
    public class CharacterUtilityService
    {
        public static bool OverwriteCharacter(Character newCharacter)
        {
            string filePath = Character.saveLoc + newCharacter.DiscordId + Character.fileExtension;
            try
            {
                BinarySerialization.WriteToBinaryFile<Character>(filePath, newCharacter, append: false);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Failed to overwrite character!");
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public static void DeleteCharacter(SocketUser user, string charName)
        {
            if (CharacterLoadService.LoadCharacter(user).Name.Equals(charName))
            {
                File.Delete(Character.saveLoc + user.Id + Character.fileExtension);
            }
        }
        // Looks through the Assembly and only gets the Types that inherit Trait
        public static List<CharacterStats.Trait> GetAllTraits()
        {
            List<CharacterStats.Trait> traits = new List<CharacterStats.Trait>();

            foreach (var trait in typeof(CharacterStats.Trait).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(CharacterStats.Trait))))
            {
                var specificTrait = (CharacterStats.Trait)Activator.CreateInstance(trait);

                traits.Add(specificTrait);
            }

            return traits;
        }
        // Looks through the assembly where Perk is declared, find all the subclasses of Perk, and instantiate each one and put in a List
        public static List<CharacterStats.Perk> GetAllPerks()
        {
            List<CharacterStats.Perk> perks = new List<CharacterStats.Perk>();

            foreach (var perk in typeof(CharacterStats.Perk).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(CharacterStats.Perk))))
            {
                var specificPerk = (CharacterStats.Perk)Activator.CreateInstance(perk);

                perks.Add(specificPerk);
            }

            return perks;
        }
        public static bool CharacterExists(SocketUser user)
        {
            return File.Exists(Character.saveLoc + user.Id + Character.fileExtension);
        }
    }
    // Obtained from: Daniel Schroeder's (aka deadlydog) Programming Blog
    // URL: http://blog.danskingdom.com/saving-and-loading-a-c-objects-data-to-an-xml-json-or-binary-file/
    // Date obtained: June 26, 2018
    // Date article was published: March 14, 2014
    /// <summary>
    /// Functions for performing common binary Serialization operations.
    /// <para>All properties and variables will be serialized.</para>
    /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
    /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
    /// </summary>
    public static class BinarySerialization
    {
        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the XML file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }
        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the XML.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
