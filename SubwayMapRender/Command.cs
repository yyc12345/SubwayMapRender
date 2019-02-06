using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace SubwayMapRender {
    public static class Command {

        static Regex hexColorRegex = new Regex("#[0123456789abcdefABCDEF]{6}");

        /// <summary>
        /// Command processor
        /// </summary>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        /// <returns>return true for continue to run.</returns>
        public static bool CommandProcessor(string command, DataStruct.SubwayMap obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "name":
                    if (sp.Count == 0) {
                        ConsoleAssistance.Write("Current subway map name: ");
                        Console.WriteLine(obj.Name);
                    } else if (sp.Count == 1) obj.Name = sp[0];
                    else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "line":
                    var innerCommand = "";
                    while (true) {
                        ConsoleAssistance.Write("Line editor> ", ConsoleColor.Green);
                        innerCommand = Console.ReadLine();
                        if (!LineProcessor(innerCommand, obj.LineList)) break;
                    }
                    break;
                case "station":
                    var innerCommand2 = "";
                    while (true) {
                        ConsoleAssistance.Write("Station editor> ", ConsoleColor.Green);
                        innerCommand2 = Console.ReadLine();
                        if (!StationProcessor(innerCommand2, obj.StationList)) break;
                    }
                    break;
                //normal command
                case "render":
                    break;
                case "save":
                    ConfigManager.Write(obj);
                    break;
                case "exit":
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

        static bool BuilderProcessor(string command, List<DataStruct.BuilderItem> obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "ls":
                    if (sp.Count == 0) {
                        int innerIndex = 0;
                        foreach (var innerItem in obj) {
                            Console.WriteLine($"\t{innerIndex}\t{innerItem.Segment}\t{innerItem.Builder}");
                            innerIndex++;
                        }

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "new":
                    if (sp.Count == 0) obj.Add(new DataStruct.BuilderItem());
                    else if (sp.Count == 1) {
                        //check param
                        int index;
                        try {
                            index = int.Parse(sp[0]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index > obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj.Insert(index, new DataStruct.BuilderItem());

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "rm":
                    if (sp.Count == 1) {
                        //check param
                        int index;
                        try {
                            index = int.Parse(sp[0]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index >= obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj.RemoveAt(index);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "mv":
                    if (sp.Count == 2) {
                        //check param
                        int index, newI;
                        try {
                            index = int.Parse(sp[0]);
                            newI = int.Parse(sp[1]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if ((index < 0 || index >= obj.Count) || (newI < 0 || newI > obj.Count - 1)) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        var cache = obj[index];
                        obj.RemoveAt(index);
                        obj.Insert(newI, cache);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "edit":
                    if (sp.Count == 3) {
                        //check param
                        int index;
                        try {
                            index = int.Parse(sp[0]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index > obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj[index] = new DataStruct.BuilderItem(sp[1], sp[2]);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "back":
                    return false;
                case "help":
                    Help();
                    break;
                default:
                    break;
            }

            return true;
        }

        static bool LineProcessor(string command, List<DataStruct.LineItem> obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "ls":
                    if (sp.Count == 0) {
                        ConsoleAssistance.WriteLine("Name\tColor\tNode count", ConsoleColor.Yellow);
                        foreach (var item in obj) {
                            Console.WriteLine($"{item.LineName}\t{item.LineColor.ToString()}\t{item.NodeList.Count}");
                        }
                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "info":
                    if (sp.Count == 1) {
                        //search
                        var search = from item in obj
                                     where item.LineName == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        //list
                        var data = search.First();
                        ConsoleAssistance.Write("Line name: ");
                        Console.WriteLine(data.LineName);
                        ConsoleAssistance.Write("Line color: ");
                        Console.WriteLine(data.LineColor.ToString());
                        ConsoleAssistance.Write("Line node list: ");
                        Console.WriteLine("Index\tPosition\tAttached station id\tRail width\tIs building?");
                        int index = 0;
                        foreach (var item in data.NodeList) {
                            Console.WriteLine($"{index}\t{item.NodePosition.ToString()}\t{item.AttachedStationId}\t{item.FollowingRailwayWidth}\t{item.FollowingRailIsBuilding}");
                            Console.WriteLine("\tIndex\tSegment\tBuilder");
                            int innerIndex = 0;
                            foreach (var innerItem in item.FollowingBuilder) {
                                Console.WriteLine($"\t{innerIndex}\t{innerItem.Segment}\t{innerItem.Builder}");
                                innerIndex++;
                            }
                            index++;
                        }

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "new":
                    if (sp.Count == 1) {
                        //search
                        var search = from item in obj
                                     where item.LineName == sp[0]
                                     select item;
                        if (search.Any()) {
                            ConsoleAssistance.WriteLine("Existed name", ConsoleColor.Red);
                            return true;
                        }

                        obj.Add(new DataStruct.LineItem() { LineName = sp[0] });

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "rm":
                    if (sp.Count == 1) {
                        //search
                        var search = from item in obj
                                     where item.LineName == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        obj.Remove(search.First());

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "re":
                    if (sp.Count == 2) {
                        //search
                        var search = from item in obj
                                     where item.LineName == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        //check name
                        var search2 = from item in obj
                                      where item.LineName == sp[1]
                                      select item;
                        if (search2.Any()) {
                            ConsoleAssistance.WriteLine("Existed name", ConsoleColor.Red);
                            return true;
                        }

                        search.First().LineName = sp[1];

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "edit":
                    if (sp.Count == 2) {
                        //search
                        var search = from item in obj
                                     where item.LineName == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        if (!hexColorRegex.IsMatch(sp[1])) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        search.First().LineColor = DataStruct.Converter.HexStringToColor(hexColorRegex.Match(sp[1]).Value);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "node":
                    if (sp.Count == 1) {
                        //search
                        var search = from item in obj
                                     where item.LineName == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        //run node editor
                        var inputObj = search.First();
                        var innerCommand = "";
                        while (true) {
                            ConsoleAssistance.Write($"Node editor ({inputObj.LineName})> ", ConsoleColor.Green);
                            innerCommand = Console.ReadLine();
                            if (!NodeProcessor(innerCommand, inputObj.NodeList, inputObj.LineName)) break;
                        }

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "back":
                    return false;
                case "help":
                    Help();
                    break;
                default:
                    break;
            }

            return true;
        }

        static bool StationProcessor(string command, List<DataStruct.StationItem> obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "back":
                    return false;
                case "help":
                    Help();
                    break;
                default:
                    break;
            }

            return true;
        }

        static bool NodeProcessor(string command, List<DataStruct.LineNodeItem> obj, string workSpaceDesc) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "ls":
                    if (sp.Count == 0) {
                        int index = 0;
                        foreach (var item in obj) {
                            Console.WriteLine($"{index}\t{item.NodePosition.ToString()}\t{item.AttachedStationId}\t{item.FollowingRailwayWidth}\t{item.FollowingRailIsBuilding}");
                            Console.WriteLine("\tIndex\tSegment\tBuilder");
                            int innerIndex = 0;
                            foreach (var innerItem in item.FollowingBuilder) {
                                Console.WriteLine($"\t{innerIndex}\t{innerItem.Segment}\t{innerItem.Builder}");
                                innerIndex++;
                            }
                            index++;
                        }
                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "new":
                    if (sp.Count == 0) obj.Add(new DataStruct.LineNodeItem());
                    else if (sp.Count == 1) {
                        //check param
                        int index;
                        try {
                            index = int.Parse(sp[0]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index > obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj.Insert(index, new DataStruct.LineNodeItem());

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "rm":
                    if (sp.Count == 1) {
                        //check param
                        int index;
                        try {
                            index = int.Parse(sp[0]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index >= obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj.RemoveAt(index);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "mv":
                    if (sp.Count == 2) {
                        //check param
                        int index, newI;
                        try {
                            index = int.Parse(sp[0]);
                            newI = int.Parse(sp[1]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if ((index < 0 || index >= obj.Count) || (newI < 0 || newI > obj.Count - 1)) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        var cache = obj[index];
                        obj.RemoveAt(index);
                        obj.Insert(newI, cache);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "pos":
                    if (sp.Count == 4) {
                        //check param
                        int index, x, y, z;
                        try {
                            index = int.Parse(sp[0]);
                            x = int.Parse(sp[1]);
                            y = int.Parse(sp[2]);
                            z = int.Parse(sp[3]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index > obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj[index].NodePosition = new DataStruct.Coordinate(x, y, z);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "attach":
                    if (sp.Count == 2) {
                        //check param
                        int index = 0;
                        try {
                            index = int.Parse(sp[0]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index > obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj[index].AttachedStationId = sp[1];

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "following":
                    if (sp.Count == 3) {
                        //check param
                        int index, railWidth;
                        bool isBuilding;
                        try {
                            index = int.Parse(sp[0]);
                            railWidth = int.Parse(sp[1]);
                            isBuilding = bool.Parse(sp[2]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index > obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj[index].FollowingRailIsBuilding = isBuilding;
                        obj[index].FollowingRailwayWidth = railWidth;

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "builder":
                    if (sp.Count == 1) {
                        //check param
                        int index;
                        try {
                            index = int.Parse(sp[0]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index > obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        var inputObj = obj[index];
                        var innerCommand = "";
                        while (true) {
                            ConsoleAssistance.Write($"Builder editor ({workSpaceDesc} Node:{index})> ", ConsoleColor.Green);
                            innerCommand = Console.ReadLine();
                            if (!BuilderProcessor(innerCommand, inputObj.FollowingBuilder)) break;
                        }

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "back":
                    return false;
                case "help":
                    Help();
                    break;
                default:
                    break;
            }

            return true;
        }

        static bool LayoutProcessor(string command, List<DataStruct.RailLayoutItem> obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "back":
                    return false;
                case "help":
                    Help();
                    break;
                default:
                    break;
            }

            return true;
        }

        static void Help() {
            Console.WriteLine("Subway Map Render - Create and render a subway map for you with website formation.");
            Console.WriteLine("");
            Console.WriteLine("General commands:");
            Console.WriteLine("\trender - render your work and output with website formation");
            Console.WriteLine("\tsave - save current work");
            Console.WriteLine("\texit - exit app");
            Console.WriteLine("\thelp - print this message");
            Console.WriteLine("");
            Console.WriteLine("Work commands:");
            Console.WriteLine("\tname [new-name] - display or set a new name for this map");
            Console.WriteLine("\tline - invoke line editor");
            Console.WriteLine("\tstation - invoke station editor");

            Console.WriteLine("");
            Console.WriteLine("Line editor command:");
            Console.WriteLine("\tls - list currently existed lines");
            Console.WriteLine("\tinfo [line-name] - show specific line's infomation");
            Console.WriteLine("\tnew [line-name] - create new line (new)");
            Console.WriteLine("\trm [line-name] - remove a line (rm)");
            Console.WriteLine("\tre [line-name] [new-name] - rename a line");
            Console.WriteLine("\tedit [line-name] [color] - change specific line's color");
            Console.WriteLine("\tnode [line-name] - invoke node editor");
            Console.WriteLine("\tback - back to main editor");

            Console.WriteLine("");
            Console.WriteLine("Station editor commands:");
            Console.WriteLine("\tls - list currently existed stations");
            Console.WriteLine("\tinfo [id] - show specific station's infomation");
            Console.WriteLine("\tnew [id] - create a new station");
            Console.WriteLine("\trm [id] - remove a station");
            Console.WriteLine("\tre [id] [new-id] - rename a station");
            Console.WriteLine("\tedit [id] [name] [is-building] - set specific station's data");
            Console.WriteLine("\tbuilder sel - invoke builder editor");
            Console.WriteLine("\tlayout [id] - invoke layout editor");
            Console.WriteLine("\tback - back to main editor");

            Console.WriteLine("");
            Console.WriteLine("Node editor commands:");
            Console.WriteLine("\tls - list currently existed node infomation");
            Console.WriteLine("\tnew [insert-index] - create new node");
            Console.WriteLine("\trm [index] - remove a node");
            Console.WriteLine("\tmv [sel-index] [insert-index] - rearrange a node");
            Console.WriteLine("\tpos [index] [x] [y] [z] - set node's position");
            Console.WriteLine("\tattach [index] [station-id] - set node's attached station id");
            Console.WriteLine("\tfollowing [index] [rail-width] [is-building] - set following rail's data");
            Console.WriteLine("\tbuilder [index] - invoke builder editor");
            Console.WriteLine("\tback - back to line editor");

            Console.WriteLine("");
            Console.WriteLine("Layout editor commands:");
            Console.WriteLine("\tls - list current layout");
            Console.WriteLine("\tnew [insert-index] - create new layout");
            Console.WriteLine("\trm [index] - remove a layout");
            Console.WriteLine("\tmv [sel-index] [insert-index] - rearrange a layout");
            Console.WriteLine("\tedit [index] [floor] [is-horizon-layout] - edit layout config");
            Console.WriteLine("\trail [index] [metadata] - edit rail layout. read guide for more infomation about this commands formation");
            Console.WriteLine("\tback - back to station editor");

            Console.WriteLine("");
            Console.WriteLine("Builder editor commands:");
            Console.WriteLine("\tls - list currently existed builder infomation");
            Console.WriteLine("\tnew [insert-index] - create a new builder item");
            Console.WriteLine("\trm [index] - remove a builder item");
            Console.WriteLine("\tmv [sel-index] [insert-index] - rearrange a builder infomation");
            Console.WriteLine("\tedit [index] [segment] [builder] - edit specific item");
            Console.WriteLine("\tback - back to previous editor");
        }


    }
}
