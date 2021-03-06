﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using ShareLib;

namespace SubwayMapEditor {
    public static class Command {

        static Regex hexColorRegex = new Regex("#[0123456789abcdefABCDEF]{6}");
        static Regex railLayoutRegex = new Regex("[PVUDLRpvudlr]{1}#(\\S|\\s)*");

        /// <summary>
        /// Command processor
        /// </summary>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        /// <returns>return true for continue to run.</returns>
        public static bool CommandProcessor(string command, ShareLib.DataStruct.SubwayMap obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            if (sp.Count == 0) {
                ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                return true;
            }
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "name":
                    if (sp.Count == 0) {
                        ConsoleAssistance.Write("Current subway map name: ", ConsoleColor.Yellow);
                        Console.WriteLine(obj.Name);
                    } else if (sp.Count == 1) obj.Name = sp[0];
                    else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "line":
                    if (sp.Count == 0) {
                        var innerCommand = "";
                        while (true) {
                            ConsoleAssistance.Write("Line editor> ", ConsoleColor.Green);
                            innerCommand = ConsoleAssistance.ReadLine();
                            if (!LineProcessor(innerCommand, obj.LineList)) break;
                        }
                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "station":
                    if (sp.Count == 0) {
                        var innerCommand2 = "";
                        while (true) {
                            ConsoleAssistance.Write("Station editor> ", ConsoleColor.Green);
                            innerCommand2 = ConsoleAssistance.ReadLine();
                            if (!StationProcessor(innerCommand2, obj.StationList)) break;
                        }
                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                //normal command
                /*
                case "render":
                    Render.RenderKernel(obj);
                    break;
                */
                case "import":
                    if (sp.Count == 1) {
                        ConsoleAssistance.WriteLine("import is a dangerous command. It will load all script and run it without any error judgement! It couldn't be stopped before all of commands has been executed!", ConsoleColor.Yellow);
                        var confirm = new Random().Next(100, 9999);
                        ConsoleAssistance.WriteLine($"Type this random number to confirm your operation: {confirm}", ConsoleColor.Yellow);
                        if (Console.ReadLine() == confirm.ToString()) {
                            if (System.IO.File.Exists(sp[0])) ConsoleAssistance.AppendImportedCommands(sp[0]);
                            else ConsoleAssistance.WriteLine("Cannot find specific file", ConsoleColor.Red);
                        }
                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "save":
                    ConfigManager.Write<ShareLib.DataStruct.SubwayMap>(obj, ConfigManager.SubwayMapFile);
                    break;
                case "exit":
                    ConsoleAssistance.WriteLine("Are you sure that you want to exit? exit command couldn't save your work automatically! Please use save command in advance.", ConsoleColor.Yellow);
                    var confirm2 = new Random().Next(100, 9999);
                    ConsoleAssistance.WriteLine($"Type this random number to confirm your operation: {confirm2}", ConsoleColor.Yellow);
                    return (Console.ReadLine() != confirm2.ToString());
                case "help":
                    Help();
                    break;
                default:
                    ConsoleAssistance.WriteLine("Unknow command", ConsoleColor.Red);
                    break;
            }

            return true;
        }

        static bool BuilderProcessor(string command, List<ShareLib.DataStruct.BuilderItem> obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            if (sp.Count == 0) {
                ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                return true;
            }
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "ls":
                    if (sp.Count == 0) {
                        OutputHelper.OutputBuilderList(obj);
                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "new":
                    if (sp.Count == 0) obj.Add(new ShareLib.DataStruct.BuilderItem());
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

                        obj.Insert(index, new ShareLib.DataStruct.BuilderItem());

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

                        if (index < 0 || index >= obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj[index] = new ShareLib.DataStruct.BuilderItem(sp[1], sp[2]);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "back":
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

        static bool LineProcessor(string command, List<ShareLib.DataStruct.LineItem> obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            if (sp.Count == 0) {
                ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                return true;
            }
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "ls":
                    if (sp.Count == 0) {
                        OutputHelper.OutputLineList(obj);
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
                        OutputHelper.OutputLineItem(data);

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

                        obj.Add(new ShareLib.DataStruct.LineItem() { LineName = sp[0] });

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
                    if (sp.Count == 3) {
                        //search
                        var search = from item in obj
                                     where item.LineName == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        if (sp[1] != "~" && (!hexColorRegex.IsMatch(sp[1]))) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (sp[1] != "~")
                            search.First().LineColor = ShareLib.DataStruct.Converter.HexStringToColor(hexColorRegex.Match(sp[1]).Value);
                        if (sp[2] != "~")
                            search.First().AttachLine = sp[2];
                        
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
                            innerCommand = ConsoleAssistance.ReadLine();
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
                    ConsoleAssistance.WriteLine("Unknow command", ConsoleColor.Red);
                    break;
            }

            return true;
        }

        static bool StationProcessor(string command, List<ShareLib.DataStruct.StationItem> obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            if (sp.Count == 0) {
                ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                return true;
            }
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "ls":
                    if (sp.Count == 0) {
                        OutputHelper.OutputStationList(obj);
                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "info":
                    if (sp.Count == 1) {
                        //search
                        var search = from item in obj
                                     where item.StationId == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        //list
                        var data = search.First();
                        OutputHelper.OutputStationItem(data);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "new":
                    if (sp.Count == 1) {
                        //search
                        var search = from item in obj
                                     where item.StationId == sp[0]
                                     select item;
                        if (search.Any()) {
                            ConsoleAssistance.WriteLine("Existed id", ConsoleColor.Red);
                            return true;
                        }

                        obj.Add(new ShareLib.DataStruct.StationItem() { StationId = sp[0] });

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "rm":
                    if (sp.Count == 1) {
                        //search
                        var search = from item in obj
                                     where item.StationId == sp[0]
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
                                     where item.StationId == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        //check name
                        var search2 = from item in obj
                                      where item.StationId == sp[1]
                                      select item;
                        if (search2.Any()) {
                            ConsoleAssistance.WriteLine("Existed id", ConsoleColor.Red);
                            return true;
                        }

                        search.First().StationId = sp[1];

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "edit":
                    if (sp.Count == 7) {
                        //search
                        var search = from item in obj
                                     where item.StationId == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        //check param
                        bool isBuilding = false;
                        int renderDirection = 0, renderOffset = 0;
                        try {
                            if (sp[3] != "~") isBuilding = bool.Parse(sp[3]);
                            if (sp[4] != "~") renderDirection = int.Parse(sp[4]);
                            if (sp[5] != "~") renderOffset = int.Parse(sp[5]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        var cache = search.First();
                        if (sp[1] != "~") cache.StationName = sp[1];
                        if (sp[2] != "~") cache.StationSubtitle = sp[2];
                        if (sp[3] != "~") cache.IsBuilding = isBuilding;
                        if (sp[4] != "~") cache.RenderDirection = renderDirection;
                        if (sp[5] != "~") cache.RenderOffset = renderOffset;
                        if (sp[6] != "~") cache.StationDescription = sp[6];

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "pos":
                    if (sp.Count == 4) {
                        //search
                        var search = from item in obj
                                     where item.StationId == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        //check param
                        int x = 0, y = 0, z = 0;
                        try {
                            if (sp[1] != "~")
                                x = int.Parse(sp[1]);
                            if (sp[2] != "~")
                                y = int.Parse(sp[2]);
                            if (sp[3] != "~")
                                z = int.Parse(sp[3]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        var operate = search.First();
                        if (sp[1] == "~") x = operate.Position.X;
                        if (sp[2] == "~") y = operate.Position.Y;
                        if (sp[3] == "~") z = operate.Position.Z;

                        operate.Position = new ShareLib.DataStruct.Coordinate(x, y, z);

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "builder":
                    if (sp.Count == 1) {
                        //search
                        var search = from item in obj
                                     where item.StationId == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        var inputObj = search.First();
                        var innerCommand = "";
                        while (true) {
                            ConsoleAssistance.Write($"Builder editor ({inputObj.StationId} - {inputObj.StationName})> ", ConsoleColor.Green);
                            innerCommand = ConsoleAssistance.ReadLine();
                            if (!BuilderProcessor(innerCommand, inputObj.Builder)) break;
                        }

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "layout":
                    if (sp.Count == 1) {
                        //search
                        var search = from item in obj
                                     where item.StationId == sp[0]
                                     select item;
                        if (!search.Any()) {
                            ConsoleAssistance.WriteLine("No matched item", ConsoleColor.Red);
                            return true;
                        }

                        var inputObj = search.First();
                        var innerCommand = "";
                        while (true) {
                            ConsoleAssistance.Write($"Layout editor ({inputObj.StationId} - {inputObj.StationName})> ", ConsoleColor.Green);
                            innerCommand = ConsoleAssistance.ReadLine();
                            if (!LayoutProcessor(innerCommand, inputObj.StationLayoutList)) break;
                        }

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "back":
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

        static bool NodeProcessor(string command, List<ShareLib.DataStruct.LineNodeItem> obj, string workSpaceDesc) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            if (sp.Count == 0) {
                ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                return true;
            }
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "ls":
                    if (sp.Count == 0) {
                        OutputHelper.OutputNodeList(obj);
                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "new":
                    if (sp.Count == 0) obj.Add(new ShareLib.DataStruct.LineNodeItem());
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

                        obj.Insert(index, new ShareLib.DataStruct.LineNodeItem());

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
                        int index, x = 0, y = 0, z = 0;
                        try {
                            index = int.Parse(sp[0]);
                            if (sp[1] != "~")
                                x = int.Parse(sp[1]);
                            if (sp[2] != "~")
                                y = int.Parse(sp[2]);
                            if (sp[3] != "~")
                                z = int.Parse(sp[3]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index >= obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        if (sp[1] == "~") x = obj[index].NodePosition.X;
                        if (sp[2] == "~") y = obj[index].NodePosition.Y;
                        if (sp[3] == "~") z = obj[index].NodePosition.Z;

                        obj[index].NodePosition = new ShareLib.DataStruct.Coordinate(x, y, z);

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

                        if (index < 0 || index >= obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        obj[index].AttachedStationId = sp[1];

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "following":
                    if (sp.Count == 3) {
                        //check param
                        int index, railWidth = 0;
                        bool isBuilding = false;
                        try {
                            index = int.Parse(sp[0]);
                            if (sp[1] != "~")
                                railWidth = int.Parse(sp[1]);
                            if (sp[2] != "~")
                                isBuilding = bool.Parse(sp[2]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index >= obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        if (sp[1] != "~")
                            obj[index].FollowingRailwayWidth = railWidth;
                        if (sp[2] != "~")
                            obj[index].FollowingRailIsBuilding = isBuilding;


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

                        if (index < 0 || index >= obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        var inputObj = obj[index];
                        var innerCommand = "";
                        while (true) {
                            ConsoleAssistance.Write($"Builder editor ({workSpaceDesc} Node:{index})> ", ConsoleColor.Green);
                            innerCommand = ConsoleAssistance.ReadLine();
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
                    ConsoleAssistance.WriteLine("Unknow command", ConsoleColor.Red);
                    break;
            }

            return true;
        }

        static bool LayoutProcessor(string command, List<ShareLib.DataStruct.StationLayoutItem> obj) {
            if (command == "") return true;

            var sp = CommandSplitter.SplitCommand(command);
            if (sp.Count == 0) {
                ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                return true;
            }
            var main = sp[0];
            sp.RemoveAt(0);
            switch (main) {
                case "ls":
                    if (sp.Count == 0) {
                        OutputHelper.OutputLayoutList(obj);
                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "new":
                    if (sp.Count == 0) obj.Add(new ShareLib.DataStruct.StationLayoutItem());
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

                        obj.Insert(index, new ShareLib.DataStruct.StationLayoutItem());

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
                        bool isHorizon = true;
                        try {
                            index = int.Parse(sp[0]);
                            if (sp[2] != "~")
                                isHorizon = bool.Parse(sp[2]);
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index >= obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        if (sp[1] != "~")
                            obj[index].Floor = sp[1];
                        if (sp[2] != "~")
                            obj[index].IsHorizonStationLayout = isHorizon;

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "rail":
                    if (sp.Count == 2) {
                        //check param
                        int index;
                        string[] spData;
                        try {
                            index = int.Parse(sp[0]);
                            spData = sp[1].Split(",");
                        } catch (Exception) {
                            ConsoleAssistance.WriteLine("Wrong formation", ConsoleColor.Red);
                            return true;
                        }

                        if (index < 0 || index >= obj.Count) {
                            ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                            return true;
                        }

                        //detect metadata
                        foreach (var item in spData) {
                            if (!railLayoutRegex.IsMatch(item)) {
                                ConsoleAssistance.WriteLine("Illegal parameter", ConsoleColor.Red);
                                return true;
                            }
                        }

                        obj[index].RailLayoutList.Clear();
                        foreach (var item in spData) {
                            obj[index].RailLayoutList.Add(new ShareLib.DataStruct.RailLayoutItem() {
                                Toward = ShareLib.DataStruct.Converter.StringToToward(item[0]),
                                AttachLine = item.Length <= 2 ? "" : item.Substring(2, item.Length - 2)
                            });
                        }

                    } else ConsoleAssistance.WriteLine("Illegal parameter count", ConsoleColor.Red);
                    break;
                case "back":
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

        static void Help() {
            Console.WriteLine("Subway Map Render - Create and render a subway map for you with website formation.");
            Console.WriteLine("");
            Console.WriteLine("General commands:");
            //Console.WriteLine("\trender - render your work and output with website formation");
            Console.WriteLine("\tsave - save current work");
            Console.WriteLine("\timport [file] - import a script file and run it immediately");
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
            Console.WriteLine("\tedit [line-name] [color] [attach-line] - change specific line's color");
            Console.WriteLine("\tnode [line-name] - invoke node editor");
            Console.WriteLine("\tback - back to main editor");

            Console.WriteLine("");
            Console.WriteLine("Station editor commands:");
            Console.WriteLine("\tls - list currently existed stations");
            Console.WriteLine("\tinfo [id] - show specific station's infomation");
            Console.WriteLine("\tnew [id] - create a new station");
            Console.WriteLine("\trm [id] - remove a station");
            Console.WriteLine("\tre [id] [new-id] - rename a station");
            Console.WriteLine("\tedit [id] [name] [subtitle] [is-building] [render-direction] [render-offset] [description] - set specific station's data");
            Console.WriteLine("\tpos [id] [x] [y] [z] - set station's position");
            Console.WriteLine("\tbuilder [id] - invoke builder editor");
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
