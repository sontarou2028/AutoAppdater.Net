using AutoAppdater.MainSenderHost;
using System.Text.Json;

namespace AutoAppdater.Receiver
{
    public static class ReceiverStream
    {
        public static CopyData? StringToCopyData(string text)
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