using AutoAppdater.MainSenderHost;
using System.Text.Json;

namespace AutoAppdater.Receivers
{
    public class Receiver
    {
        public void Message(string js)
        {
            CopyData? c = ReceiverStream.StringToCopyData(js);
            if (c == null) return;
            if (c.SenderId == SystemId.System || c.SenderId == SystemId.Log ||
            c.SenderId == SystemId.Console || c.SenderId == SystemId.Command)
                Task.Run(() => SystemProcedure(ref c));
            else
                Task.Run(() => Procedure(ref c));
        }
        internal void SystemProcedure(ref CopyData data)
        {
            
        }
        public void Procedure(ref CopyData data)
        {
            //overridable
        }
    }
    public static class ReceiverStream
    {
        internal static CopyData? StringToCopyData(string text)
        {
            try
            {
                return JsonSerializer.Deserialize<CopyData>(text);
            }
            catch
            {
                return null;
            }
        }
    }
}