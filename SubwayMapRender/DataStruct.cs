using System;
using System.Collections.Generic;
using System.Text;

namespace SubwayMapRender.DataStruct {

    public static class Converter {

        public static Color HexStringToColor(string str) {
            str = str.ToLower();
            return new Color(
                (byte)(HexToDec(str[1]) * 16 + HexToDec(str[2])),
                (byte)(HexToDec(str[3]) * 16 + HexToDec(str[4])),
                (byte)(HexToDec(str[5]) * 16 + HexToDec(str[6])));
        }

        public static string ColorToHexString(Color col) {
            var res = "#";
            res += $"{DecToHex(col.R / 16)}{DecToHex(col.R % 16)}";
            res += $"{DecToHex(col.G / 16)}{DecToHex(col.G % 16)}";
            res += $"{DecToHex(col.B / 16)}{DecToHex(col.B % 16)}";
            return res;
        }

        public static RailToward StringToToward(char chr) {
            if (chr >= 'A' && chr <= 'Z') chr = (char)(chr - 'A' + 'a');
            switch (chr) {
                case 'p':
                    return RailToward.Platform;
                case 'v':
                    return RailToward.Void;
                case 'u':
                    return RailToward.Up;
                case 'd':
                    return RailToward.Down;
                case 'l':
                    return RailToward.Left;
                case 'r':
                    return RailToward.Right;
                default:
                    return RailToward.Void;
            }
        }

        public static char TowardToString(RailToward tw) {
            return (char)(tw.ToString()[0] - 'A' + 'a');
        }

        public static string RailLayoutListToString(List<RailLayoutItem> list) {
            string[] strl = new string[list.Count];
            for(int i = 0; i < list.Count; i++) {
                strl[i] = TowardToString(list[i].Toward) + "#" + list[i].AttachLine;
            }
            return String.Join(',', strl);
        }

        static int HexToDec(char chr) {
            if (chr >= 'a' && chr <= 'f') return 10 + chr - 'a';
            else return chr - '0';
        }

        static char DecToHex(int num) {
            if (num < 10) return (char)(num + '0');
            else return (char)(num - 10 + 'a');
        }

    }

    public class SubwayMap {
        public SubwayMap() {
            LineList = new List<LineItem>();
            StationList = new List<StationItem>();
            Name = "";
        }
        public string Name { get; set; }
        public List<LineItem> LineList { get; set; }
        public List<StationItem> StationList { get; set; }
    }

    //=====================================================

    public struct Coordinate {
        public Coordinate(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }
        public int X;
        public int Y;
        public int Z;
        public override string ToString() {
            return $"{X},{Y},{Z}";
        }
    }

    public struct Color {
        public Color(byte r, byte g, byte b) {
            R = r;
            G = g;
            B = b;
        }
        public byte R;
        public byte G;
        public byte B;
        public override string ToString() {
            return Converter.ColorToHexString(this);
        }
    }

    public struct BuilderItem {
        public BuilderItem(string segment, string builder) {
            Segment = segment;
            Builder = builder;
        }
        public string Segment;
        public string Builder;
    }

    //=====================================================

    public class LineItem {
        public LineItem() {
            LineName = "";
            LineColor = new Color(0, 0, 0);
            NodeList = new List<LineNodeItem>();
        }
        public string LineName { get; set; }
        public Color LineColor { get; set; }
        public List<LineNodeItem> NodeList { get; set; }
    }

    public class LineNodeItem {
        public LineNodeItem() {
            NodePosition = new Coordinate(0, 0, 0);
            AttachedStationId = "";
            FollowingBuilder = new List<BuilderItem>();
            FollowingRailwayWidth = 1;
            FollowingRailIsBuilding = false;
        }
        public Coordinate NodePosition { get; set; }
        //set blank for normal node (no station node)
        public string AttachedStationId { get; set; }
        //the following property is suit for the road from this node to next node.
        public List<BuilderItem> FollowingBuilder { get; set; }
        public int FollowingRailwayWidth { get; set; }
        public bool FollowingRailIsBuilding { get; set; }
    }

    //=====================================================

    public class StationItem {
        public StationItem() {
            StationId = "";
            StationName = "";
            StationSubtitle = "";
            StationDescription = "";
            Position = new Coordinate(0, 0, 0);
            RenderDirection = 0;
            RenderOffset = 10;
            Builder = new List<BuilderItem>();
            StationLayoutList = new List<StationLayoutItem>();
        }
        public string StationId { get; set; }
        public string StationName { get; set; }
        public string StationSubtitle { get; set; }
        public string StationDescription { get; set; }
        public Coordinate Position { get; set; }
        public bool IsBuilding { get; set; }
        public int RenderDirection { get; set; }
        public int RenderOffset { get; set; }
        public List<BuilderItem> Builder { get; set; }
        public List<StationLayoutItem> StationLayoutList { get; set; }
    }

    public class StationLayoutItem {
        public StationLayoutItem() {
            Floor = "";
            IsHorizonStationLayout = true;
            RailLayoutList = new List<RailLayoutItem>();
        }
        public string Floor { get; set; }
        public bool IsHorizonStationLayout { get; set; }
        public List<RailLayoutItem> RailLayoutList { get; set; }
    }

    public class RailLayoutItem {
        public RailLayoutItem() {
            AttachLine = "";
            Toward = RailToward.Void;
        }
        //set black for platform
        public string AttachLine { get; set; }
        public RailToward Toward { get; set; }
    }

    public enum RailToward {
        Platform,
        Void,
        Up,
        Down,
        Left,
        Right
    }

}
