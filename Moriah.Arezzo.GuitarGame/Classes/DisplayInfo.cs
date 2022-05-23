
namespace Moriah.Arezzo.GuitarGame
{
    public class DisplayInfo
    {
        public string MonitorName { get; internal set; }
        public Win32Rect MonitorArea { get; internal set; }
        public Win32Rect WorkArea { get; internal set; }
        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public bool IsPrimary { get; internal set; }
    }
}
