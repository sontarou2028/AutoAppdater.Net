using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace AutoAppdater.MainSenderHost
{
    internal struct SystemId
    {
        public const int System = 0;
        public const int Log = 1;
        public const int Console = 2;
        public const int Command = 3;
    }
    internal struct LibraryAuthor
    {
        public const string Name = "AutoAppdater.Net";
        public const string Version = "1.0.0";
        public const string Framework = "net9.0";
    }
    public struct SenderAuthor
    {
        public string Name;
        public string Version;
    }
    public enum TargetVersionOption : byte
    {
        Upper,
        Lower,
        Match,
    }
    public struct TargetAuthor
    {
        public string TargetName;
        public string TargetVersion;
        public TargetVersionOption Option;
        public string? UUID;
    }
    public class CopyData
    {
        public SenderAuthor LibraryAuthor { get; }
        public SenderAuthor? ModAuthor { get; }
        public TargetAuthor? TargetAuthor { get; }
        public int SenderId { get; }
        public int? Int { get; }
        public long? Long { get; }
        public float? Float { get; }
        public double? Double { get; }
        public char? Char { get; }
        public string? Str { get; }
        public object[]? ObjArr { get; }
        public CopyData(SenderAuthor? ModAuthor, TargetAuthor? TargetAuthor,
        int SenderId, int? Int, long? Long, float? Float, double? Double, char? Char, string? Str, object[]? ObjArr)
        {
            this.ModAuthor = ModAuthor;
            this.TargetAuthor = TargetAuthor;
            this.SenderId = SenderId;
            this.Int = Int;
            this.Long = Long;
            this.Float = Float;
            this.Double = Double;
            this.Char = Char;
            this.Str = Str;
            this.ObjArr = ObjArr;
        }
    }
    public class CourierData : CopyData
    {
        public CourierData(SenderAuthor? ModAuthor, TargetAuthor? TargetAuthor,
        int SenderId, int? Int, long? Long, float? Float, double? Double, char? Char, string? Str,object[]? ObjArr)
        : base(ModAuthor,TargetAuthor,SenderId, Int, Long, Float, Double, Char, Str, ObjArr)
        {
            //senderId check
            if (SenderId == SystemId.System || SenderId == SystemId.Log ||
            SenderId == SystemId.Console || SenderId == SystemId.Command) throw new InvalidOperationException("Cannot use a system reserved ID.");
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    struct COPYDATASTRUCT32
    {
        public UInt32 dwData;
        public UInt32 cbData;
        public IntPtr lpData;
    }
    internal static class MainSender
    {
        const uint WM_COPYDATA = 0x004A;
        const string MainProcess_Name = "AutoAppdater";
        const string MainProcess_MainWindowName = "MainWindow";
        //const int Send_Success = 0;
        const int Send_Error_MainProc_NotFound = -1;
        const int Send_Error_MainProc_IllegalTypeOrVersion = -2;
        [DllImport("user32.dll", SetLastError = true)]
        static extern Int32 SendMessage(IntPtr hWnd, uint Msg, Int32 wParam, ref COPYDATASTRUCT32 lParam);
        internal static int Send(CopyData data)
        {
            //check
            Process[] p = Process.GetProcessesByName(MainProcess_Name);
            if (p.Length == 0) return Send_Error_MainProc_NotFound;
            if (p[0].MainWindowTitle != MainProcess_MainWindowName) return Send_Error_MainProc_IllegalTypeOrVersion;
            //obj
            string cobj = JsonSerializer.Serialize(data);
            COPYDATASTRUCT32 cd = new COPYDATASTRUCT32();
            cd.dwData = 0;
            cd.cbData = (UInt32)cobj.Length - 1;
            cd.lpData = Marshal.StringToHGlobalAnsi(cobj);
            SendMessage(p[0].MainWindowHandle, WM_COPYDATA, 0,ref cd);
            return Marshal.GetLastWin32Error();
        }
        public static int Send(CourierData data)
        {
            //check
            Process[] p = Process.GetProcessesByName(MainProcess_Name);
            if (p.Length == 0) return Send_Error_MainProc_NotFound;
            if (p[0].MainWindowTitle != MainProcess_MainWindowName) return Send_Error_MainProc_IllegalTypeOrVersion;
            //obj
            string cobj = JsonSerializer.Serialize(data);
            COPYDATASTRUCT32 cd = new COPYDATASTRUCT32();
            cd.dwData = 0;
            cd.cbData = (UInt32)cobj.Length - 1;
            cd.lpData = Marshal.StringToHGlobalAnsi(cobj);
            SendMessage(p[0].MainWindowHandle, WM_COPYDATA, 0,ref cd);
            return Marshal.GetLastWin32Error();
        }
    }
}