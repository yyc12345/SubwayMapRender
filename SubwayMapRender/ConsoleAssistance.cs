using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubwayMapRender {

    public static class ConsoleAssistance {

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

        /// <summary>
        /// the update of console.writeline()
        /// </summary>
        /// <param name="str"></param>
        /// <param name="foreground"></param>
        /// <param name="background"></param>
        /// <param name="previousForeground"></param>
        /// <param name="previousBackground"></param>
        public static void WriteLine(string str, ConsoleColor foreground = ConsoleColor.White,
            ConsoleColor background = ConsoleColor.Black) {

            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;

            Console.WriteLine(str);

            //restore
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// the update of console.write()
        /// </summary>
        /// <param name="str"></param>
        /// <param name="foreground"></param>
        /// <param name="background"></param>
        /// <param name="previousForeground"></param>
        /// <param name="previousBackground"></param>
        public static void Write(string str, ConsoleColor foreground = ConsoleColor.White,
            ConsoleColor background = ConsoleColor.Black) {

            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;

            Console.Write(str);

            //restore
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }


    }
}
