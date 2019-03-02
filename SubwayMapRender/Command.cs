using ShareLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubwayMapRender {
    public static class Command {

        public static bool CommandProcessor(string command, ref ShareLib.RenderStruct.RenderSettings obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            if (sp.Count == 0) {
                ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                return true;
            }
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "render":
                    Render.RenderKernel(obj);
                    break;
                case "tt":
                    try {
                        var cache = Tutorial();
                        obj = cache;
                    } catch (Exception) {
                        ConsoleAssistance.WriteLine("Error occured! All setting is lost", ConsoleColor.Red);
                    }
                    break;
                case "ls":
                    OutputRenderSettings(obj);
                    break;
                case "exit":
                    //save settings
                    ConfigManager.Write<ShareLib.RenderStruct.RenderSettings>(obj, ConfigManager.RenderSettingsFile);
                    return false;
                case "help":
                    Help();
                    break;
                default:
                    ConsoleAssistance.WriteLine("Unknow command", ConsoleColor.Red);
                    break;
            }

            return true;
        }

        static ShareLib.RenderStruct.RenderSettings Tutorial() {
            var res = new ShareLib.RenderStruct.RenderSettings();

            return res;
        }

        static void OutputRenderSettings(ShareLib.RenderStruct.RenderSettings settings) {

        }

        static void Help() {
            Console.WriteLine("Subway Map Render - Create and render a subway map for you with website formation.");
            Console.WriteLine("");
            Console.WriteLine("General commands:");
            Console.WriteLine("\trender - render your work and output with website formation");
            Console.WriteLine("\ttt - start a tutorial to help you to set your render config");
            Console.WriteLine("\tls - display current render config");
            Console.WriteLine("\texit - exit app");
            Console.WriteLine("\thelp - print this message");
            Console.WriteLine("");
        }

    }
}
