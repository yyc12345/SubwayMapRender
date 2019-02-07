using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SubwayMapRender {
    public static class Render {

        public static void RenderKernel(DataStruct.SubwayMap map) {

            if (!CheckData(map)) {
                ConsoleAssistance.WriteLine("Your current map data is not suit for render. Please add more infomation.", ConsoleColor.Red);
                return;
            }

            //set folder
            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");

            //output file
            var fsHtml = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "output", "index.html"), false, Encoding.UTF8);
            var fsCss = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "output", "index.css"), false, Encoding.UTF8);
            //var fsJs = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "output", "index.js"), false, Encoding.UTF8);

            //write css for line and station's style
            fsCss.WriteLine(".render-line {");
            fsCss.WriteLine("stroke-width: 5px;");
            fsCss.WriteLine("stroke-linecap: round;");
            fsCss.WriteLine("}");

            fsCss.WriteLine(".render-line-building {");
            fsCss.WriteLine("stroke-dasharray: 10,10;");
            fsCss.WriteLine("}");

            fsCss.WriteLine(".render-station {");
            fsCss.WriteLine("fill: #7f7f7f;");
            fsCss.WriteLine("stroke: #7f7f7f;");
            fsCss.WriteLine("}");

            fsCss.WriteLine(".render-station-building {");
            fsCss.WriteLine("fill: #dfdfdf;");
            fsCss.WriteLine("stroke: #dfdfdf;");
            fsCss.WriteLine("}");

            //write html head
            fsHtml.Write("<!DOCTYPE html><html><head><link rel=\"stylesheet\" type=\"text/css\" href=\"index.css\"></head><body>");

            //calc map size and write css at the same time
            int negX = int.MaxValue, negZ = int.MaxValue, posX = int.MinValue, posZ = int.MinValue;
            foreach (var line in map.LineList) {
                WriteLineColor(line.LineName, line.LineColor, fsCss);
                foreach (var node in line.NodeList) {
                    if (node.NodePosition.X > posX) posX = node.NodePosition.X;
                    if (node.NodePosition.X < negX) negX = node.NodePosition.X;
                    if (node.NodePosition.Z > posZ) posZ = node.NodePosition.Z;
                    if (node.NodePosition.Z < negZ) negZ = node.NodePosition.Z;
                }
            }

            int width = posX - negX;
            int height = posZ - negZ;

            //write svg size
            fsHtml.Write($"<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"{width}\" height=\"{height}\"><g id=\"root\">");

            //output line
            int x = 0, y = 0, _x = 0, _y = 0;
            foreach (var line in map.LineList) {
                if (line.NodeList.Count > 1) {
                    string _lineName = line.LineName.Replace(" ", "-");
                    for (int i = 0; i < line.NodeList.Count - 1; i++) {
                        CoordinateConverter(line.NodeList[i].NodePosition.X, line.NodeList[i].NodePosition.Z, negX, negZ, ref x, ref y);
                        CoordinateConverter(line.NodeList[i + 1].NodePosition.X, line.NodeList[i + 1].NodePosition.Z, negX, negZ, ref _x, ref _y);
                        fsHtml.Write($"<line x1=\"{x}\" y1=\"{y}\" x2=\"{_x}\" y2=\"{_y}\" class=\"render-line render-line--{_lineName}{(line.NodeList[i].FollowingRailIsBuilding ? " render-line-building" : "")}\"/>");
                    }
                }
            }

            //write html foot
            fsHtml.Write("</g></svg></body></html>");

            fsHtml.Close();
            fsHtml.Dispose();
            fsCss.Close();
            fsCss.Dispose();
        }

        static bool CheckData(DataStruct.SubwayMap map) {
            foreach(var line in map.LineList) {
                foreach(var node in line.NodeList) {
                    goto next;
                }
            }
            return false;

            next:
            return true;
        }

        static void WriteLineColor(string lineName, DataStruct.Color col, StreamWriter fs) {
            lineName = lineName.Replace(" ", "-");
            fs.Write($".render-line--{lineName} {{");
            fs.WriteLine($"stroke: {col.ToString()};");
            fs.WriteLine("}");
        }

        static void CoordinateConverter(int mapX, int mapZ, int startX, int startZ, ref int screenX, ref int screenY) {
            screenX = mapX - startX;
            screenY = mapZ - startZ;
        }
    }
}
