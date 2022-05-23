using System;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace Moriah.Arezzo.GuitarGame
{

    [StructLayout(LayoutKind.Sequential)]
    public struct Win32Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}", Left, Top, Right, Bottom);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct MonitorInfoEx
    {
        public int Size;
        public Win32Rect Monitor;
        public Win32Rect WorkArea;
        public uint Flags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DisplayManager.DeviceNameCharacterCount)]
        public string DeviceName;

        public void Init()
        {
            this.Size = 40 + 2 * DisplayManager.DeviceNameCharacterCount;
            this.DeviceName = string.Empty;
        }
    }

    
    public class DisplayManager
    {
        // size of a device name string
        internal const int DeviceNameCharacterCount = 32;


        private delegate bool MonitorEnumProcDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Win32Rect lprcMonitor, uint dwData);

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProcDelegate lpfnEnum, uint dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        private static ObservableCollection<DisplayInfo> _displays = new ObservableCollection<DisplayInfo>();
        public static ObservableCollection<DisplayInfo> Displays
        {
            get { return _displays; }
        }


        public static void LoadDisplays()
        {
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnumProc, 0);
        }

        [AllowReversePInvokeCalls]
        internal static bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref Win32Rect lprcMonitor, uint dwData)
        {
            var monitor = new MonitorInfoEx();
            monitor.Init();

            bool success = GetMonitorInfo(hMonitor, ref monitor);

            if (success)
            {
                var display = new DisplayInfo();

                display.MonitorName = monitor.DeviceName;

                display.Width = monitor.Monitor.Right - monitor.Monitor.Left;
                display.Height = monitor.Monitor.Bottom - monitor.Monitor.Top;

                display.MonitorArea = monitor.Monitor;
                display.WorkArea = monitor.WorkArea;
                display.IsPrimary = (monitor.Flags > 0);

                _displays.Add(display);
            }

            return true;
        }
    }
}
