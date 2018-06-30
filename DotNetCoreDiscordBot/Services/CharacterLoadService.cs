using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiscordBot.Services
{
    public class CharacterLoadService
    {
        public CharacterLoadService()
        {

        }
        public static Character LoadCharacter(SocketUser user)
        {
            string filePath = Character.saveLoc + user.Id + Character.fileExtension;

            if (File.Exists(filePath))
            {
                try
                {
                    return BinarySerialization.ReadFromBinaryFile<Character>(filePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Failed to load character!");
                    Console.WriteLine(e);
                    return null;
                }
            }
            else
                return null;
        }
    }
}
