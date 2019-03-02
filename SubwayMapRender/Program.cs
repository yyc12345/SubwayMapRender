using System;
using ShareLib;

namespace SubwayMapRender
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleAssistance.WriteLine("Welcome to use Subway Map Render.", ConsoleColor.Yellow);

            //load
            var globalConfig = ConfigManager.Read<ShareLib.RenderStruct.RenderSettings>(ConfigManager.RenderSettingsFile);
            ConsoleAssistance.WriteLine("Load saved render config OK.", ConsoleColor.Yellow);

            string command = "";
            while (true) {
                ConsoleAssistance.Write("> ", ConsoleColor.Green);
                command = Console.ReadLine();
                if (!Command.CommandProcessor(command, ref globalConfig)) break;
            }

            ConsoleAssistance.WriteLine("Thank you for your using.", ConsoleColor.Yellow);

        }
        
    }
}
