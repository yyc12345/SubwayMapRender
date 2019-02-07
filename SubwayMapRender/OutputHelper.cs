using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTables;

namespace SubwayMapRender {
    public static class OutputHelper {

        public static void OutputLineList(List<DataStruct.LineItem> obj) {
            var table = new ConsoleTable("Name", "Color", "Node count");
            foreach (var item in obj) {
                table.AddRow(item.LineName, item.LineColor.ToString(), item.NodeList.Count.ToString());
            }
            Console.Write(table.ToStringAlternative());
            Console.WriteLine();
        }

        public static void OutputLineItem(DataStruct.LineItem data) {
            ConsoleAssistance.Write("Line name: ", ConsoleColor.Yellow);
            Console.WriteLine(data.LineName);
            ConsoleAssistance.Write("Line color: ", ConsoleColor.Yellow);
            Console.WriteLine(data.LineColor.ToString());
            ConsoleAssistance.WriteLine("Line node list: ", ConsoleColor.Yellow);
            OutputNodeList(data.NodeList);
        }

        public static void OutputStationList(List<DataStruct.StationItem> obj) {
            var table = new ConsoleTable("Id", "Name", "Subtitle", "Is building?", "Render direction", "Render Offset", "Builder count", "Layout count", "Description");
            foreach (var item in obj) {
                table.AddRow(item.StationId, item.StationName, item.StationSubtitle, item.IsBuilding.ToString(), item.RenderDirection.ToString(), item.RenderOffset.ToString(), item.Builder.Count.ToString(), item.StationLayoutList.Count.ToString(), item.StationDescription);
            }
            Console.Write(table.ToStringAlternative());
            Console.WriteLine();
        }

        public static void OutputStationItem(DataStruct.StationItem data) {
            ConsoleAssistance.Write("Station id: ", ConsoleColor.Yellow);
            Console.WriteLine(data.StationId);
            ConsoleAssistance.Write("Station name: ", ConsoleColor.Yellow);
            Console.WriteLine(data.StationName);
            ConsoleAssistance.Write("Station subtitle: ", ConsoleColor.Yellow);
            Console.WriteLine(data.StationSubtitle);
            ConsoleAssistance.Write("Is building: ", ConsoleColor.Yellow);
            Console.WriteLine(data.IsBuilding);
            ConsoleAssistance.Write("Render direction: ", ConsoleColor.Yellow);
            Console.WriteLine(data.RenderDirection);
            ConsoleAssistance.Write("Render offset: ", ConsoleColor.Yellow);
            Console.WriteLine(data.RenderOffset);
            ConsoleAssistance.Write("Station description: ", ConsoleColor.Yellow);
            Console.WriteLine(data.StationDescription);
            ConsoleAssistance.WriteLine("Builder list: ", ConsoleColor.Yellow);
            OutputBuilderList(data.Builder);
            ConsoleAssistance.WriteLine("Layout list: ", ConsoleColor.Yellow);
            OutputLayoutList(data.StationLayoutList);
        }

        public static void OutputNodeList(List<DataStruct.LineNodeItem> obj) {
            var table = new ConsoleTable("Index", "Position", "Attached station id", "Rail width", "Is building?");
            int index = 0;
            foreach (var item in obj) {
                table.AddRow(index.ToString(), item.NodePosition.ToString(), item.AttachedStationId, item.FollowingRailwayWidth.ToString(), item.FollowingRailIsBuilding.ToString());
                table.AddRow("==Builder==>", "Index", "Segment", "Builder", "");
                int innerIndex = 0;
                foreach (var innerItem in item.FollowingBuilder) {
                    table.AddRow("", innerIndex.ToString(), innerItem.Segment, innerItem.Builder, "");
                    innerIndex++;
                }
                index++;
            }
            Console.Write(table.ToStringAlternative());
            Console.WriteLine();
        }

        public static void OutputBuilderList(List<DataStruct.BuilderItem> obj) {
            var table = new ConsoleTable("Index", "Segment", "Builder");
            int innerIndex = 0;
            foreach (var item in obj) {
                table.AddRow(innerIndex.ToString(), item.Segment, item.Builder);
                innerIndex++;
            }
            Console.Write(table.ToStringAlternative());
            Console.WriteLine();
        }

        public static void OutputLayoutList(List<DataStruct.StationLayoutItem> data) {
            int index = 0;
            foreach (var item in data) {
                Console.WriteLine("----------------------------");
                ConsoleAssistance.Write("Index: ", ConsoleColor.Yellow);
                Console.WriteLine(index);
                ConsoleAssistance.Write("Floor: ", ConsoleColor.Yellow);
                Console.WriteLine(item.Floor);
                ConsoleAssistance.WriteLine("Rail layout: ", ConsoleColor.Yellow);

                if (item.RailLayoutList.Count != 0) {
                    string[] rails = new string[item.RailLayoutList.Count];
                    int innerindex = 0;
                    int max = 0;
                    foreach (var inner in item.RailLayoutList) {
                        rails[innerindex] = OutputToward(inner.Toward) + inner.AttachLine;
                        if (rails[innerindex].Length > max) max = rails[innerindex].Length;
                        innerindex++;
                    }

                    if (item.IsHorizonStationLayout) {
                        foreach (var inner in rails) {
                            Console.WriteLine(inner);
                        }
                    } else {
                        string cache = "";
                        for (int i = 0; i < max; i++) {
                            foreach (var getter in rails) {
                                if (getter.Length > i) cache += getter[i];
                                else cache += " ";
                            }
                            Console.WriteLine(cache);
                            cache = "";
                        }
                    }
                }

                index++;
            }
        }

        static string OutputToward(DataStruct.RailToward toward) {
            switch (toward) {
                case DataStruct.RailToward.None:
                    return "█████";
                case DataStruct.RailToward.Up:
                    return "↑↑↑↑↑";
                case DataStruct.RailToward.Down:
                    return "↓↓↓↓↓";
                case DataStruct.RailToward.Left:
                    return "←←←←←";
                case DataStruct.RailToward.Right:
                    return "→→→→→";
                default:
                    return "█████";
            }
        }

    }
}
