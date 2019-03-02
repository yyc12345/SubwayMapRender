using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShareLib {

    public static class ConfigManager {

        public static readonly string SubwayMapFile = "config.cfg";
        public static readonly string RenderSettingsFile = "render.cfg";

        public static T Read<T>(string file_name) where T : new() {
            if (!File.Exists(file_name)) Init<T>(file_name);
            var fs = new StreamReader(file_name, Encoding.UTF8);
            var cache = fs.ReadToEnd();
            fs.Close();
            fs.Dispose();
            return JsonConvert.DeserializeObject<T>(cache);
        }

        static void Init<T>(string file_name) where T : new() {
            var fs = new StreamWriter(file_name, false, Encoding.UTF8);
            var data = new T();
            fs.Write(JsonConvert.SerializeObject(data));
            fs.Close();
            fs.Dispose();
        }

        public static void Write<T>(T data, string file_name) where T : new() {
            var fs = new StreamWriter(file_name, false, Encoding.UTF8);
            fs.Write(JsonConvert.SerializeObject(data));
            fs.Close();
            fs.Dispose();
        }
    }

}
