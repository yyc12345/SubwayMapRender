using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubwayMapEditor {

    public class ConsoleAssistance : ShareLib.ConsoleAssistance {

        static Queue<string> importedCommandList = new Queue<string>();

        public static string ReadLine() {
            if (importedCommandList.Count == 0) return Console.ReadLine();
            else {
                var cache =  importedCommandList.Dequeue();
                Console.WriteLine(cache);
                return cache;
            }

        }

        public static void AppendImportedCommands(string file) {
            var fs = new System.IO.StreamReader(file, Encoding.UTF8);
            string command = "";
            while (true) {
                command = fs.ReadLine();
                if (command is null) break;
                importedCommandList.Enqueue(command);
            }
            fs.Close();
            fs.Dispose();
        }

    }
}
