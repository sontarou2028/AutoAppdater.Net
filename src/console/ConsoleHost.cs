using AutoAppdater.Consoles;
using AutoAppdater.MainSenderHost;

namespace AutoAppdater.ConsoleHosts
{
    public enum ConsoleType
    {
        Internal,
        External,
        Attatch,
        NoWindow,
    }
    public class ConsoleAuthor
    {
        public ConsoleType Type { get; }
        public string? WindowTitle { get; }
        public string? TargetWindowTitle { get; }
        public int? SizeX { get; }
        public int? SizeY { get; }
        public ConsoleAuthor(ConsoleType type, string? windowTitle, int? sizeX, int? sizeY)
        {
            if (type == ConsoleType.Internal)
            {
                if (windowTitle != null) throw new Exception();
                if (sizeX != null) throw new Exception();
                if (sizeY != null) throw new Exception();
                Type = type;
            }
            else if (type == ConsoleType.External)
            {
                Type = type;
                WindowTitle = windowTitle;
                SizeX = sizeX;
                SizeY = sizeY;
            }
            else if (type == ConsoleType.Attatch)
            {
                if (sizeX != null) throw new Exception();
                if (sizeY != null) throw new Exception();
                Type = type;
                TargetWindowTitle = windowTitle;
            }
            else if (type == ConsoleType.NoWindow)
            {
                if (windowTitle != null) throw new Exception();
                if (sizeX != null) throw new Exception();
                if (sizeY != null) throw new Exception();
                Type = type;
            }
        }
    }
    internal class ConsoleHost
    {
        internal const int SendInfo_Error_SerializeFail = -3;
        internal const int SendInfo_Id_Read = 1;
        internal const int SendInfo_Id_Write = 2;
        internal const int SendInfo_Id_Open = 3;
        internal const int SendInfo_Id_Close = 4;
        internal const int SendInfo_Id_Flush = 5;
        struct CongifValue
        {

        }
        struct DefaultValue
        {

        }
        ConsoleAuthor Auth;
        SenderAuthor Sender;
        object senderLocker = new object();
        public ConsoleHost(ConsoleAuthor author, SenderAuthor sender)
        {
            Auth = author;
            Sender = sender;
        }
        internal int SendInfo(DisplayInfo info)
        {
            lock (senderLocker)
            {
                try
                {
                    return MainSender.Send(new CopyData(Sender, null, SystemId.Console, SendInfo_Id_Write, null, null, null, null, null, [Auth, info]));
                }
                catch
                {
                    return SendInfo_Error_SerializeFail;
                }
            }
        }
        public int OpenWindow()
        {
            lock (senderLocker)
                return MainSender.Send(new CopyData(Sender, null, SystemId.Console, SendInfo_Id_Open, null, null, null, null, null, [Auth]));
        }
        public int CloseWindow()
        {
            lock (senderLocker)
                return MainSender.Send(new CopyData(Sender, null, SystemId.Console, SendInfo_Id_Close, null, null, null, null, null, [Auth]));
        }
        public int FlushWindow()
        {
            lock (senderLocker)
            return MainSender.Send(new CopyData(Sender, null, SystemId.Console, SendInfo_Id_Flush, null, null, null, null, null, [Auth]));
        }
    }
}