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
            var table = new ConsoleTable("Id", "Name", "Is building?", "Render direction", "Render Offset", "Builder count", "Layout count");
            foreach (var item in obj) {
                table.AddRow(item.StationId, item.StationName, item.IsBuilding.ToString(), item.RenderDirection.ToString(), item.RenderOffset.ToString(), item.Builder.Count.ToString(), item.StationLayoutList.Count.ToString());
            }
            Console.Write(table.ToStringAlternative());
            Console.WriteLine();
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
            OutputBuilderList(data.Builder);
            ConsoleAssistance.WriteLine("Layout list: ", ConsoleColor.Yellow);
            
            Console.WriteLine("Index\tFloor\tIs horizon layout\tRail count");
            int index = 0;
            foreach (var item in data.StationLayoutList) {
                Console.WriteLine($"{index}\t{item.Floor}\t{item.IsHorizonStationLayout}\t{item.RailLayoutList.Count}");
                index++;
            }
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
            Console.WriteLine("Index\tFloor\tIs horizon layout\tRail count");
            int index = 0;
            foreach (var item in data) {
                Console.WriteLine($"{index}\t{item.Floor}\t{item.IsHorizonStationLayout}\t{item.RailLayoutList.Count}");
                index++;
            }
        }

    }
}
