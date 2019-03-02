using System;
using System.Collections.Generic;
using System.Text;

namespace ShareLib.RenderStruct {

    public class RenderSettings {
        public RenderSettings() {
            KeepIndependentAttachLine = false;
            CoordinateSystem = CoordinateSystemType.MinecraftCoordinate;
        }

        public bool KeepIndependentAttachLine { get; set; }
        public CoordinateSystemType CoordinateSystem { get; set; }
    }

    public enum CoordinateSystemType {
        MinecraftCoordinate,
        ScreenCoordinate
    }

}
