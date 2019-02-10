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

            //copy necessary file
            File.Copy(Path.Combine(Environment.CurrentDirectory, "Templates", "index.html"), Path.Combine(Environment.CurrentDirectory, "output", "index.html"), true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, "Templates", "index.css"), Path.Combine(Environment.CurrentDirectory, "output", "index.css"), true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, "Templates", "index.js"), Path.Combine(Environment.CurrentDirectory, "output", "index.js"), true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, "Templates", "mui.css"), Path.Combine(Environment.CurrentDirectory, "output", "mui.css"), true);

            //output file
            /*
            Line 6 <title>{title}</title>
            Line 31 <td class="mui--text-title">{title}</td>
            Line 44 output svg: <svg xmlns="http://www.w3.org/2000/svg" version="1.1" width="100%" height="100%" style="flex: 1;">
            Line 53 output line list (with button style)
            */
            var readerHtml = new StreamReader(Path.Combine(Environment.CurrentDirectory, "output", "index.html"), Encoding.UTF8);
            var fsHtml = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "output", "generate.html"), false, Encoding.UTF8);
            var fsCss = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "output", "generate.css"), false, Encoding.UTF8);
            var fsJs = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "output", "generate.js"), false, Encoding.UTF8);

            //write title 1================================================================================
            CopyLimitedLine(5, readerHtml, fsHtml);
            readerHtml.ReadLine();
            fsHtml.WriteLine($"<title>{map.Name}</title>");

            //write title 2================================================================================
            CopyLimitedLine(31 - 6 - 1, readerHtml, fsHtml);
            readerHtml.ReadLine();
            fsHtml.WriteLine($"<td class=\"mui--text-title\">{map.Name}</td>");

            //write svg body================================================================================
            CopyLimitedLine(44 - 31 - 1, readerHtml, fsHtml);
            readerHtml.ReadLine();

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
            fsHtml.WriteLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"100%\" height=\"100%\" style=\"flex: 1;\"><g id=\"uiSvgRoot\">");

            //write line =================================
            //output line and matched js
            //write js head
            fsJs.WriteLine("function generatedCodeShowLine(name) {");
            fsJs.WriteLine("switch(name) {");

            int x = 0, y = 0, _x = 0, _y = 0;
            foreach (var line in map.LineList) {
                if (line.NodeList.Count > 1) {
                    string _lineName = line.LineName.Replace(" ", "-");
                    for (int i = 0; i < line.NodeList.Count - 1; i++) {
                        CoordinateConverter(line.NodeList[i].NodePosition.X, line.NodeList[i].NodePosition.Z, negX, negZ, ref x, ref y);
                        CoordinateConverter(line.NodeList[i + 1].NodePosition.X, line.NodeList[i + 1].NodePosition.Z, negX, negZ, ref _x, ref _y);
                        //write html
                        fsHtml.WriteLine($"<line onclick=\"showWindowLine('{_lineName}-{i}')\" x1=\"{x}\" y1=\"{y}\" x2=\"{_x}\" y2=\"{_y}\" class=\"smr-display-line smr-display--{_lineName} smr-svg-line smr-svg-line--{_lineName}{(line.NodeList[i].FollowingRailIsBuilding ? " smr-svg-line-building" : "")}\"/>");
                        //write js
                        fsJs.WriteLine($"case '{_lineName}-{i}':");
                        fsJs.WriteLine($"setWindowLine('{line.LineColor.ToString()}', '{line.LineName}');");
                        foreach (var builders in line.NodeList[i].FollowingBuilder) {
                            fsJs.WriteLine($"addWindowLineBuider('{builders.Segment}', '{builders.Builder}');");
                        }
                        fsJs.WriteLine("break;");
                    }
                }
            }

            //write js foot
            fsJs.WriteLine("}}");


            //write station ================================

            //attach data dict
            var attachStation = new Dictionary<string, string>();
            foreach (var line in map.LineList) {
                string _lineName = line.LineName.Replace(" ", "-");
                foreach (var node in line.NodeList) {
                    if (node.AttachedStationId != "") {
                        if (attachStation.ContainsKey(node.AttachedStationId)) attachStation[node.AttachedStationId] += $" {_lineName}";
                        else attachStation.Add(node.AttachedStationId, $" {_lineName}");
                    }
                }
            }

            //write js head
            fsJs.WriteLine("");
            fsJs.WriteLine("function generatedCodeShowStation(id) {");
            fsJs.WriteLine("switch(id) {");

            //write station point
            string className = "";
            foreach (var station in map.StationList) {
                //write html
                if (attachStation.ContainsKey(station.StationId)) className = attachStation[station.StationId];
                else className = "";
                fsHtml.WriteLine($"<g onclick=\"showWindowStation('{station.StationId}')\" class=\"smr-display-station{className}\">");

                CoordinateConverter(station.Position.X, station.Position.Z, negX, negZ, ref x, ref y);
                if (station.IsBuilding) fsHtml.WriteLine($"<circle cx=\"{x - 10}\" cy=\"{y - 10}\" r=\"10\" style=\"stroke: black; stroke-width; fill: gray;\"/>");
                else fsHtml.WriteLine($"<circle cx=\"{x - 10}\" cy=\"{y - 10}\" r=\"10\" style=\"stroke: black; stroke-width; fill: white;\"/>");

                fsHtml.WriteLine("</g>");

                //write js
                fsJs.WriteLine($"case '{station.StationId}':");
                fsJs.WriteLine($"setWindowStation('{station.StationName}', '{station.StationSubtitle}', '{station.StationId}', '{station.StationDescription}', '{station.Position.X}', '{station.Position.Y}', '{station.Position.Z}');");
                foreach (var builders in station.Builder) {
                    fsJs.WriteLine($"addWindowStationBuider('{builders.Segment}', '{builders.Builder}');");
                }
                foreach (var layout in station.StationLayoutList) {
                    fsJs.WriteLine($"addWindowStationLayout('{layout.Floor}', '{layout.IsHorizonStationLayout.ToString().ToLower()}', '{DataStruct.Converter.RailLayoutListToString(layout.RailLayoutList)}')");
                }
                fsJs.WriteLine("break;");
            }

            //write js foot
            fsJs.WriteLine("}}");

            //finish svg foot
            fsHtml.WriteLine($"</g></svg>");

            //write line btn body================================================================================
            CopyLimitedLine(53 - 44 - 1, readerHtml, fsHtml);
            readerHtml.ReadLine();
            foreach (var line in map.LineList) {
                string _lineName = line.LineName.Replace(" ", "-");
                fsHtml.WriteLine($"<button onclick=\"setDisplay('{_lineName}');\" class=\"mui-btn mui-btn--raised mui-btn--primary\">");
                fsHtml.WriteLine($"<p><font color=\"{line.LineColor.ToString()}\">█▌</font> {line.LineName}</p>");
                fsHtml.WriteLine("</button>");
            }

            //write html foot================================================================================
            CopyToEnd(readerHtml, fsHtml);

            readerHtml.Close();
            readerHtml.Dispose();
            fsHtml.Close();
            fsHtml.Dispose();
            fsCss.Close();
            fsCss.Dispose();
            fsJs.Close();
            fsJs.Dispose();

            //remove and rename
            File.Delete(Path.Combine(Environment.CurrentDirectory, "output", "index.html"));
            File.Move(Path.Combine(Environment.CurrentDirectory, "output", "generate.html"), Path.Combine(Environment.CurrentDirectory, "output", "index.html"));
        }

        static void CopyLimitedLine(int lineCount, StreamReader fsOri, StreamWriter fsTarget) {
            for (int copied = 0; copied < lineCount; copied++) {
                fsTarget.WriteLine(fsOri.ReadLine());
            }
        }

        static void CopyToEnd(StreamReader fsOri, StreamWriter fsTarget) {
            var str = "";
            while (true) {
                str = fsOri.ReadLine();
                if (str is null) break;
                fsTarget.WriteLine(str);
            }
        }

        static bool CheckData(DataStruct.SubwayMap map) {
            foreach (var line in map.LineList) {
                foreach (var node in line.NodeList) {
                    goto next;
                }
            }
            return false;

            next:
            return true;
        }

        static void WriteLineColor(string lineName, DataStruct.Color col, StreamWriter fs) {
            lineName = lineName.Replace(" ", "-");
            fs.WriteLine($".smr-svg-line--{lineName} {{");
            fs.WriteLine($"stroke: {col.ToString()};");
            fs.WriteLine("}");
            fs.WriteLine($".smr-window-layout--{lineName} {{");
            fs.WriteLine($"fill: {col.ToString()};");
            fs.WriteLine("}");
        }

        static void CoordinateConverter(int mapX, int mapZ, int startX, int startZ, ref int screenX, ref int screenY) {
            screenX = mapX - startX;
            screenY = mapZ - startZ;
        }
    }
}
