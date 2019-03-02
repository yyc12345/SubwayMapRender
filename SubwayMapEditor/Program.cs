using ShareLib;
using System;

namespace SubwayMapEditor {
    class Program {
        static void Main(string[] args) {
            ConsoleAssistance.WriteLine("Welcome to use Subway Map Editor.", ConsoleColor.Yellow);

            //load
            var globalConfig = ConfigManager.Read<ShareLib.DataStruct.SubwayMap>(ConfigManager.SubwayMapFile);
            ConsoleAssistance.WriteLine("Load saved subway map OK.", ConsoleColor.Yellow);

            string command = "";
            while (true) {
                ConsoleAssistance.Write("> ", ConsoleColor.Green);
                command = ConsoleAssistance.ReadLine();
                if (!Command.CommandProcessor(command, globalConfig)) break;
            }

            ConsoleAssistance.WriteLine("Thank you for your using.", ConsoleColor.Yellow);

        }

    }
}
