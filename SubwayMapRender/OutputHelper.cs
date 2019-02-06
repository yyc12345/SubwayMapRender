using System;
using System.Collections.Generic;
using System.Text;

namespace SubwayMapRender {
    public static class OutputHelper {

        public static void OutputLineList(List<DataStruct.LineItem> obj) {
            ConsoleAssistance.WriteLine("Name\tColor\tNode count", ConsoleColor.Yellow);
            foreach (var item in obj) {
                Console.WriteLine($"{item.LineName}\t{item.LineColor.ToString()}\t{item.NodeList.Count}");
            }
        }

        public static void OutputLineItem(DataStruct.LineItem data) {
            ConsoleAssistance.Write("Line name: ", ConsoleColor.Yellow);
            Console.WriteLine(data.LineName);
            ConsoleAssistance.Write("Line color: ", ConsoleColor.Yellow);
            Console.WriteLine(data.LineColor.ToString());
            ConsoleAssistance.WriteLine("Line node list: ", ConsoleColor.Yellow);
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
        }

        public static void OutputStationList(List<DataStruct.StationItem> obj) {
            ConsoleAssistance.WriteLine("Id\tName\tIs building?\tRender direction\tRender Offset\tBuilder count\tLayout count", ConsoleColor.Yellow);
            foreach (var item in obj) {
                Console.WriteLine($"{item.StationId}\t{item.StationName}\t{item.IsBuilding}\t{item.RenderDirection}\t{item.RenderOffset}\t{item.Builder.Count}\t{item.StationLayoutList.Count}");
            }
        }

        public static void OutputStationItem(DataStruct.StationItem data) {
            ConsoleAssistance.Write("Station id: ", ConsoleColor.Yellow);
            Console.WriteLine(data.StationId);
            ConsoleAssistance.Write("Station name: ", ConsoleColor.Yellow);
            Console.WriteLine(data.StationName);
            ConsoleAssistance.Write("Is building: ", ConsoleColor.Yellow);
            Console.WriteLine(data.IsBuilding);
            ConsoleAssistance.Write("Render direction: ", ConsoleColor.Yellow);
            Console.WriteLine(data.RenderDirection);
            ConsoleAssistance.Write("Render offset: ", ConsoleColor.Yellow);
            Console.WriteLine(data.RenderOffset);
            ConsoleAssistance.WriteLine("Builder list: ", ConsoleColor.Yellow);
            Console.WriteLine("Index\tSegment\tBuilder");
            int index = 0;
            foreach (var item in data.Builder) {
                Console.WriteLine($"{index}\t{item.Segment}\t{item.Builder}");
                index++;
            }
            ConsoleAssistance.WriteLine("Layout list: ", ConsoleColor.Yellow);
            Console.WriteLine("Index\tFloor\tIs horizon layout\tRail count");
            index = 0;
            foreach (var item in data.StationLayoutList) {
                Console.WriteLine($"{index}\t{item.Floor}\t{item.IsHorizonStationLayout}\t{item.RailLayoutList.Count}");
                index++;
            }
        }

        public static void OutputNodeList(List<DataStruct.LineNodeItem> obj) {
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
        }

        public static void OutputBuilderList(List<DataStruct.BuilderItem> obj) {
            int innerIndex = 0;
            foreach (var innerItem in obj) {
                Console.WriteLine($"\t{innerIndex}\t{innerItem.Segment}\t{innerItem.Builder}");
                innerIndex++;
            }
        }

        public static void OutputLayoutList(List<DataStruct.StationLayoutItem> data) {
            Console.WriteLine("Index\tFloor\tIs horizon layout\tRail count");
            int index = 0;
            foreach (var item in data) {
                Console.WriteLine($"{index}\t{item.Floor}\t{item.IsHorizonStationLayout}\t{item.RailLayoutList.Count}");
                index++;
            }
        }

    }
}
