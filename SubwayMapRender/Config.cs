using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SubwayMapRender {

    public static class ConfigManager {
        public static DataStruct.SubwayMap Read() {
            if (!File.Exists("config.cfg")) Init();
            var fs = new StreamReader("config.cfg", Encoding.UTF8);
            var cache = fs.ReadToEnd();
            fs.Close();
            fs.Dispose();
            return JsonConvert.DeserializeObject<DataStruct.SubwayMap>(cache);
        }

        static void Init() {
            var fs = new StreamWriter("config.cfg", false, Encoding.UTF8);
            var data = new DataStruct.SubwayMap();
            fs.Write(JsonConvert.SerializeObject(data));
            fs.Close();
            fs.Dispose();
        }

        public static void Write(DataStruct.SubwayMap data) {
            var fs = new StreamWriter("config.cfg", false, Encoding.UTF8);
            fs.Write(JsonConvert.SerializeObject(data));
            fs.Close();
            fs.Dispose();
        }
    }

}
